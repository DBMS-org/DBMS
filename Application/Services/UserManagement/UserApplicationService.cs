using Application.DTOs.UserManagement;
using Application.DTOs.Shared;
using Application.Exceptions;
using Application.Interfaces.UserManagement;
using Application.Interfaces.Infrastructure;
using Application.Utilities;
using Domain.Entities.UserManagement;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace Application.Services.UserManagement
{
    public class UserApplicationService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IValidationService _validationService;
        private readonly ICacheService _cacheService;
        private readonly IMappingService _mappingService;
        private readonly IStructuredLogger<UserApplicationService> _logger;
        private readonly IEmailService _emailService;

        private const string AllUsersCacheKey = "all_users";
        private static string UserByIdCacheKey(int id) => $"user_{id}";

        public UserApplicationService(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IValidationService validationService,
            ICacheService cacheService,
            IMappingService mappingService,
            IStructuredLogger<UserApplicationService> logger,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _validationService = validationService;
            _cacheService = cacheService;
            _mappingService = mappingService;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            using var operation = _logger.BeginOperation("GetAllUsers");
            try
            {
                var userDtos = await _cacheService.GetOrCreateAsync(AllUsersCacheKey, async () =>
                {
                    var users = await _userRepository.GetAllAsync();
                    return _mappingService.Map<IEnumerable<UserDto>>(users);
                }, TimeSpan.FromMinutes(10));

                _logger.LogOperationSuccess("GetAllUsers", new { UserCount = userDtos.Count() });
                return Result.Success(userDtos);
            }
            catch (DbException ex)
            {
                _logger.LogDataAccessError("GetAllUsers", ex);
                return Result.Failure<IEnumerable<UserDto>>("Database error occurred while retrieving users");
            }
            catch (Exception ex)
            {
                _logger.LogUnexpectedError("GetAllUsers", ex);
                return Result.Failure<IEnumerable<UserDto>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<UserDto>> GetUserByIdAsync(int id)
        {
            using var operation = _logger.BeginOperation("GetUserById", new { UserId = id });
            try
            {
                var userDto = await _cacheService.GetOrCreateAsync(UserByIdCacheKey(id), async () =>
                {
                    var user = await _userRepository.GetByIdAsync(id);
                    return user == null ? null : _mappingService.Map<UserDto>(user);
                }, TimeSpan.FromMinutes(10));

                if (userDto == null)
                {
                    _logger.LogBusinessWarning($"User not found", new { UserId = id });
                    return Result.Failure<UserDto>(ErrorCodes.Messages.UserNotFound(id));
                }

                _logger.LogOperationSuccess("GetUserById", new { UserId = id, Email = userDto.Email });
                return Result.Success(userDto);
            }
            catch (DbException ex)
            {
                _logger.LogDataAccessError("GetUserById", ex, new { UserId = id });
                return Result.Failure<UserDto>("Database error occurred while retrieving user");
            }
            catch (Exception ex)
            {
                _logger.LogUnexpectedError("GetUserById", ex, new { UserId = id });
                return Result.Failure<UserDto>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<UserDto>> CreateUserAsync(CreateUserRequest request)
        {
            using var operation = _logger.BeginOperation("CreateUser", new { Email = request.Email, Role = request.Role });
            try
            {
                // Validate input
                var validationResult = await _validationService.ValidateAsync(request);
                if (validationResult.IsFailure)
                {
                    _logger.LogValidationFailure("User", validationResult.Errors, new { Email = request.Email });
                    return Result.Failure<UserDto>(validationResult.Errors);
                }

                // Check if user already exists
                var existingUser = await _userRepository.ExistsByNameOrEmailAsync(request.Name, request.Email);
                if (existingUser)
                {
                    _logger.LogBusinessWarning($"User creation failed - user already exists", new { Email = request.Email });
                    return Result.Failure<UserDto>(ErrorCodes.Messages.UserAlreadyExists(request.Email));
                }

                // Hash password
                var hashedPassword = _passwordService.HashPassword(request.Password);

                // Create user entity using AutoMapper
                var user = _mappingService.Map<User>(request);
                user.PasswordHash = hashedPassword; // Set password hash separately for security

                var createdUser = await _userRepository.CreateAsync(user);

                // Invalidate cache
                _cacheService.Remove(AllUsersCacheKey);

                // Send welcome email with credentials to the newly created user
                try
                {
                    await _emailService.SendNewUserCredentialsEmailAsync(
                        createdUser.Email,
                        createdUser.Name ?? "User",
                        createdUser.Email,
                        request.Password // Send the original password before it was hashed
                    );
                    _logger.LogInformation($"Welcome email with credentials sent to {createdUser.Email}");
                }
                catch (Exception emailEx)
                {
                    // Log the error but don't fail the user creation operation
                    _logger.LogBusinessWarning($"Failed to send welcome email to {createdUser.Email}", new { Exception = emailEx.Message });
                }

                _logger.LogOperationSuccess("CreateUser", new { UserId = createdUser.Id, Email = createdUser.Email });
                return Result.Success(_mappingService.Map<UserDto>(createdUser));
            }
            catch (ValidationException ex)
            {
                _logger.LogValidationFailure("User", ex.ValidationErrors, new { Email = request.Email });
                return Result.Failure<UserDto>(ex.ValidationErrors);
            }
            catch (DbException ex)
            {
                _logger.LogDataAccessError("CreateUser", ex, new { Email = request.Email });
                return Result.Failure<UserDto>("Database error occurred while creating user");
            }
            catch (Exception ex)
            {
                _logger.LogUnexpectedError("CreateUser", ex, new { Email = request.Email });
                return Result.Failure<UserDto>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            using var operation = _logger.BeginOperation("UpdateUser", new { UserId = id, Email = request.Email });
            try
            {
                // Validate input
                var validationResult = await _validationService.ValidateAsync(request);
                if (validationResult.IsFailure)
                {
                    _logger.LogValidationFailure("User", validationResult.Errors, new { UserId = id });
                    return Result.Failure(validationResult.Errors);
                }

                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogBusinessWarning($"User not found for update", new { UserId = id });
                    return Result.Failure(ErrorCodes.Messages.UserNotFound(id));
                }

                // Update user properties using AutoMapper
                _mappingService.Map(request, user);

                var updateResult = await _userRepository.UpdateAsync(user);

                // Invalidate cache
                _cacheService.Remove(AllUsersCacheKey);
                _cacheService.Remove(UserByIdCacheKey(id));

                if (updateResult)
                {
                    // Send email notification to user about profile update
                    try
                    {
                        var emailSubject = "Account Details Updated - DBMS System";
                        var emailBody = GetAccountUpdatedEmailBody(user.Name ?? "User");
                        await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);
                        _logger.LogInformation($"Account update notification email sent to {user.Email}");
                    }
                    catch (Exception emailEx)
                    {
                        // Log the error but don't fail the update operation
                        _logger.LogBusinessWarning($"Failed to send account update notification email to {user.Email}", new { Exception = emailEx.Message });
                    }

                    _logger.LogOperationSuccess("UpdateUser", new { UserId = id, Email = request.Email });
                    return Result.Success();
                }
                else
                {
                    _logger.LogBusinessWarning($"User update failed - no changes made", new { UserId = id });
                    return Result.Failure(ErrorCodes.Messages.InternalError);
                }
            }
            catch (ValidationException ex)
            {
                _logger.LogValidationFailure("User", ex.ValidationErrors, new { UserId = id });
                return Result.Failure(ex.ValidationErrors);
            }
            catch (DbException ex)
            {
                _logger.LogDataAccessError("UpdateUser", ex, new { UserId = id });
                return Result.Failure("Database error occurred while updating user");
            }
            catch (Exception ex)
            {
                _logger.LogUnexpectedError("UpdateUser", ex, new { UserId = id });
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> DeactivateUserAsync(int id)
        {
            using var operation = _logger.BeginOperation("DeactivateUser", new { UserId = id });
            try
            {
                // Get the user to deactivate
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogBusinessWarning("User not found for deactivation", new { UserId = id });
                    return Result.Failure(ErrorCodes.Messages.UserNotFound(id));
                }

                // Check if already inactive
                if (user.Status == Domain.Entities.UserManagement.UserStatus.Inactive)
                {
                    _logger.LogBusinessWarning("User is already deactivated", new { UserId = id, Email = user.Email });
                    return Result.Failure("User is already deactivated");
                }

                // Store user email and name for notification
                var userEmail = user.Email;
                var userName = user.Name ?? "User";

                // Set status to Inactive
                user.Status = Domain.Entities.UserManagement.UserStatus.Inactive;

                var updateResult = await _userRepository.UpdateAsync(user);

                // Invalidate cache
                _cacheService.Remove(AllUsersCacheKey);
                _cacheService.Remove(UserByIdCacheKey(id));

                if (updateResult)
                {
                    // Send email notification to user about account deactivation
                    try
                    {
                        await _emailService.SendAccountDeactivationEmailAsync(userEmail, userName);
                        _logger.LogInformation($"Account deactivation notification email sent to {userEmail}");
                    }
                    catch (Exception emailEx)
                    {
                        // Log the error but don't fail the deactivation operation
                        _logger.LogBusinessWarning($"Failed to send account deactivation notification email to {userEmail}", new { Exception = emailEx.Message });
                    }

                    _logger.LogOperationSuccess("DeactivateUser", new { UserId = id, Email = userEmail });
                    return Result.Success();
                }
                else
                {
                    _logger.LogBusinessWarning("User deactivation failed - no changes made", new { UserId = id });
                    return Result.Failure(ErrorCodes.Messages.InternalError);
                }
            }
            catch (DbException ex)
            {
                _logger.LogDataAccessError("DeactivateUser", ex, new { UserId = id });
                return Result.Failure("Database error occurred while deactivating user");
            }
            catch (Exception ex)
            {
                _logger.LogUnexpectedError("DeactivateUser", ex, new { UserId = id });
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> DeleteUserAsync(int id)
        {
            using var operation = _logger.BeginOperation("DeleteUser", new { UserId = id });
            try
            {
                // First, get the user details before deletion so we can send email notification
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogBusinessWarning("User not found for deletion", new { UserId = id });
                    return Result.Failure(ErrorCodes.Messages.UserNotFound(id));
                }

                // Store user email and name for notification
                var userEmail = user.Email;
                var userName = user.Name ?? "User";

                var deleteResult = await _userRepository.DeleteAsync(id);

                // Invalidate cache
                _cacheService.Remove(AllUsersCacheKey);
                _cacheService.Remove(UserByIdCacheKey(id));

                if (deleteResult)
                {
                    // Send email notification to user about account deletion
                    try
                    {
                        await _emailService.SendAccountDeletionEmailAsync(userEmail, userName);
                        _logger.LogInformation($"Account deletion notification email sent to {userEmail}");
                    }
                    catch (Exception emailEx)
                    {
                        // Log the error but don't fail the delete operation
                        _logger.LogBusinessWarning($"Failed to send account deletion notification email to {userEmail}", new { Exception = emailEx.Message });
                    }

                    _logger.LogOperationSuccess("DeleteUser", new { UserId = id, Email = userEmail });
                    return Result.Success();
                }
                else
                {
                    _logger.LogBusinessWarning("User deletion failed - no changes made", new { UserId = id });
                    return Result.Failure(ErrorCodes.Messages.InternalError);
                }
            }
            catch (DbException ex)
            {
                _logger.LogDataAccessError("DeleteUser", ex, new { UserId = id });
                return Result.Failure("Database error occurred while deleting user");
            }
            catch (Exception ex)
            {
                _logger.LogUnexpectedError("DeleteUser", ex, new { UserId = id });
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> TestConnectionAsync()
        {
            using var operation = _logger.BeginOperation("TestDbConnection");
            try
            {
                var canConnect = await _userRepository.CanConnectAsync();
                if(canConnect)
                {
                    _logger.LogOperationSuccess("TestDbConnection");
                    return Result.Success();
                }
                else
                {
                    _logger.LogBusinessWarning("Database connection test failed");
                    return Result.Failure("Database connection failed");
                }
            }
            catch (DbException ex)
            {
                _logger.LogDataAccessError("TestDbConnection", ex);
                return Result.Failure("Database connection failed");
            }
            catch (Exception ex)
            {
                _logger.LogUnexpectedError("TestDbConnection", ex);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        private string GetAccountUpdatedEmailBody(string userName)
        {
            return $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .info-box {{ background: white; border-left: 4px solid #4a90e2; padding: 20px; margin: 20px 0; border-radius: 5px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }}
                        .warning {{ background: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 5px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 30px; color: #666; }}
                        .btn {{ display: inline-block; padding: 12px 24px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Account Details Updated</h1>
                        </div>
                        <div class='content'>
                            <p>Hello <strong>{userName}</strong>,</p>

                            <div class='info-box'>
                                <p style='margin: 0; font-size: 16px;'>
                                    Your account details have been updated by admin, please review your profile.
                                </p>
                            </div>

                            <p>If you have any questions or concerns about these changes, please contact your system administrator.</p>

                            <div class='warning'>
                                <strong>Security Notice:</strong> If you did not expect these changes, please contact your administrator immediately.
                            </div>

                            <div class='footer'>
                                <p>Best regards,<br><strong>DBMS System Team</strong></p>
                                <p><small>This is an automated message, please do not reply to this email.</small></p>
                            </div>
                        </div>
                    </div>
                </body>
                </html>";
        }

    }
} 