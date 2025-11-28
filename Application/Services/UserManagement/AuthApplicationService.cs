using Application.DTOs.UserManagement;
using Application.DTOs.Shared;
using Application.Exceptions;
using Application.Interfaces.UserManagement;
using Application.Interfaces.Infrastructure;
using Application.Utilities;
using Domain.Entities.UserManagement;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Data;
using System.Security.Cryptography;

namespace Application.Services.UserManagement
{
    public class AuthApplicationService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IValidationService _validationService;
        private readonly IStructuredLogger<AuthApplicationService> _logger;

        public AuthApplicationService(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IJwtService jwtService,
            IEmailService emailService,
            IValidationService validationService,
            IStructuredLogger<AuthApplicationService> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _emailService = emailService;
            _validationService = validationService;
            _logger = logger;
        }

        public async Task<Result<AuthenticationResult>> LoginAsync(LoginRequest request)
        {
            using var operation = _logger.BeginOperation("UserLogin", new { Username = request.Username });
            try
            {
                // Validate input
                var validationResult = await _validationService.ValidateAsync(request);
                if (validationResult.IsFailure)
                {
                    _logger.LogValidationFailure("Login", validationResult.Errors, new { Username = request.Username });
                    return Result.Failure<AuthenticationResult>(validationResult.Errors);
                }

                // Find user by name regardless of status
                var user = await _userRepository.GetByNameAsync(request.Username);

                if (user == null)
                {
                    _logger.LogBusinessWarning("Login attempt with invalid username", new { Username = request.Username });
                    return Result.Failure<AuthenticationResult>("Invalid name or password");
                }

                // Check if account is inactive
                if (user.Status != UserStatus.Active)
                {
                    _logger.LogBusinessWarning("Login attempt with inactive account", new { Username = request.Username, Status = user.Status });

                    // Provide specific message based on status
                    string errorMessage = user.Status == UserStatus.Inactive
                        ? "Your account has been deactivated and you cannot log in. Please contact your administrator for assistance."
                        : $"Your account is {SafeDataConverter.SafeToLower(user.Status.ToString())}. Please contact your administrator.";

                    return Result.Failure<AuthenticationResult>(errorMessage);
                }

                // Verify password
                if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
                {
                    _logger.LogBusinessWarning("Login attempt with invalid password", new { Username = request.Username, UserId = user.Id });
                    return Result.Failure<AuthenticationResult>("Invalid name or password");
                }

                // Update last login time
                user.LastLoginAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                // Generate token
                var token = _jwtService.GenerateToken(user);

                // Create user DTO
                var userDto = MapToUserDto(user);

                var authResult = new AuthenticationResult
                {
                    Token = token,
                    User = userDto
                };

                _logger.LogOperationSuccess("UserLogin", new { UserId = user.Id, Username = request.Username, Role = user.Role });
                return Result.Success(authResult);
            }
            catch (ValidationException ex)
            {
                _logger.LogValidationFailure("Login", ex.ValidationErrors, new { Username = request.Username });
                return Result.Failure<AuthenticationResult>(ex.ValidationErrors);
            }
            catch (DbException ex)
            {
                _logger.LogDataAccessError("UserLogin", ex, new { Username = request.Username });
                return Result.Failure<AuthenticationResult>("Authentication service temporarily unavailable");
            }
            catch (Exception ex)
            {
                _logger.LogUnexpectedError("UserLogin", ex, new { Username = request.Username });
                return Result.Failure<AuthenticationResult>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<AuthenticationResult>> RegisterAsync(CreateUserRequest request)
        {
            using var operation = _logger.BeginOperation("UserRegistration", new { Email = request.Email, Username = request.Name });
            try
            {
                // Validate input
                var validationResult = await _validationService.ValidateAsync(request);
                if (validationResult.IsFailure)
                {
                    _logger.LogValidationFailure("Registration", validationResult.Errors, new { Email = request.Email });
                    return Result.Failure<AuthenticationResult>(validationResult.Errors);
                }

                // Check if user already exists by name or email
                var existingUser = await _userRepository.ExistsByNameOrEmailAsync(request.Name, request.Email);

                if (existingUser)
                {
                    _logger.LogBusinessWarning("Registration failed - user already exists", new { Email = request.Email });
                    return Result.Failure<AuthenticationResult>("User with this name or email already exists");
                }

                // Hash password
                var hashedPassword = _passwordService.HashPassword(request.Password);

                // Create new user
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = hashedPassword,
                    Role = request.Role,
                    Region = request.Region,
                    Country = request.Country,
                    OmanPhone = request.OmanPhone,
                    CountryPhone = request.CountryPhone,
                    Status = UserStatus.Active, // Default to Active on registration
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdUser = await _userRepository.CreateAsync(user);

                // Generate token
                var token = _jwtService.GenerateToken(createdUser);

                // Create user DTO
                var userDto = MapToUserDto(createdUser);

                var authResult = new AuthenticationResult
                {
                    Token = token,
                    User = userDto
                };

                _logger.LogOperationSuccess("UserRegistration", new { UserId = createdUser.Id, Email = createdUser.Email });
                return Result.Success(authResult);
            }
            catch (ValidationException ex)
            {
                _logger.LogValidationFailure("Registration", ex.ValidationErrors, new { Email = request.Email });
                return Result.Failure<AuthenticationResult>(ex.ValidationErrors);
            }
            catch (DbException ex)
            {
                _logger.LogDataAccessError("UserRegistration", ex, new { Email = request.Email });
                return Result.Failure<AuthenticationResult>("Registration service temporarily unavailable");
            }
            catch (Exception ex)
            {
                _logger.LogUnexpectedError("UserRegistration", ex, new { Email = request.Email });
                return Result.Failure<AuthenticationResult>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            using var operation = _logger.BeginOperation("ForgotPassword", new { Email = request.Email });
            try
            {
                // Find user by email
                var user = await _userRepository.GetByEmailAsync(request.Email);

                // Exceptional Flow: Email not found
                if (user == null)
                {
                    _logger.LogBusinessWarning("Password reset requested for non-existent email", new { Email = request.Email });
                    return Result.Failure("Email not found.");
                }

                // Check if user is inactive
                if (user.Status != UserStatus.Active)
                {
                    _logger.LogBusinessWarning("Password reset requested for inactive user", new { Email = request.Email });
                    return Result.Failure("This account is not active. Please contact your administrator.");
                }

                // Generate secure reset code
                var resetCode = GenerateSecureCode();
                user.PasswordResetCode = resetCode;
                user.PasswordResetCodeExpiry = DateTime.UtcNow.AddHours(1); // 1 hour expiry
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                // Send email with reset code
                try
                {
                    await _emailService.SendPasswordResetEmailAsync(user.Email, resetCode);
                    _logger.LogOperationSuccess("Password reset code sent", new { Email = request.Email });
                }
                catch (Exception emailEx)
                {
                    _logger.LogBusinessWarning("Failed to send password reset email, but code was generated", new { Email = request.Email, Error = emailEx.Message });
                    // Don't fail the entire operation if email fails - user can still use the code if they obtain it otherwise
                }

                _logger.LogOperationSuccess("ForgotPassword");
                return Result.Success();
            }
            catch (DbException ex)
            {
                _logger.LogDataAccessError("ForgotPassword", ex, new { Email = request.Email });
                return Result.Failure("Password reset service temporarily unavailable");
            }
            catch (Exception ex)
            {
                _logger.LogUnexpectedError("ForgotPassword", ex, new { Email = request.Email });
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> VerifyResetCodeAsync(VerifyResetCodeRequest request)
        {
            using var operation = _logger.BeginOperation("VerifyResetCode", new { Email = request.Email });
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);

                if (user == null ||
                    user.Status != UserStatus.Active ||
                    string.IsNullOrEmpty(user.PasswordResetCode) ||
                    user.PasswordResetCodeExpiry == null ||
                    user.PasswordResetCodeExpiry < DateTime.UtcNow ||
                    user.PasswordResetCode != request.Code)
                {
                    _logger.LogBusinessWarning("Invalid or expired reset code provided", new { Email = request.Email });
                    return Result.Failure("Invalid or expired reset code.");
                }

                _logger.LogOperationSuccess("VerifyResetCode", new { Email = request.Email });
                return Result.Success();
            }
            catch (DbException ex)
            {
                _logger.LogDataAccessError("VerifyResetCode", ex, new { Email = request.Email });
                return Result.Failure("Verification service temporarily unavailable.");
            }
            catch (Exception ex)
            {
                _logger.LogUnexpectedError("VerifyResetCode", ex, new { Email = request.Email });
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
        {
            using var operation = _logger.BeginOperation("ResetPassword", new { Email = request.Email });
            try
            {
                // Validate input
                var validationResult = await _validationService.ValidateAsync(request);
                if (validationResult.IsFailure)
                {
                    _logger.LogValidationFailure("ResetPassword", validationResult.Errors, new { Email = request.Email });
                    return Result.Failure(validationResult.Errors);
                }

                var user = await _userRepository.GetByEmailAsync(request.Email);

                if (user == null ||
                    user.Status != UserStatus.Active ||
                    string.IsNullOrEmpty(user.PasswordResetCode) ||
                    user.PasswordResetCodeExpiry == null ||
                    user.PasswordResetCodeExpiry < DateTime.UtcNow ||
                    user.PasswordResetCode != request.Code)
                {
                    _logger.LogBusinessWarning("Attempt to reset password with invalid or expired code", new { Email = request.Email });
                    return Result.Failure("Invalid or expired reset code.");
                }

                // Hash new password and clear reset code
                user.PasswordHash = _passwordService.HashPassword(request.NewPassword);
                user.PasswordResetCode = null;
                user.PasswordResetCodeExpiry = null;
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                _logger.LogOperationSuccess("ResetPassword", new { Email = request.Email });
                return Result.Success();
            }
            catch (ValidationException ex)
            {
                _logger.LogValidationFailure("ResetPassword", ex.ValidationErrors, new { Email = request.Email });
                return Result.Failure(ex.ValidationErrors);
            }
            catch (DbException ex)
            {
                _logger.LogDataAccessError("ResetPassword", ex, new { Email = request.Email });
                return Result.Failure("Password reset service temporarily unavailable");
            }
            catch (Exception ex)
            {
                _logger.LogUnexpectedError("ResetPassword", ex, new { Email = request.Email });
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public Task<Result> ValidateTokenAsync(string token)
        {
            using var operation = _logger.BeginOperation("ValidateToken");
            try
            {
                var isValid = _jwtService.ValidateToken(token);
                if (isValid)
                {
                    _logger.LogOperationSuccess("ValidateToken");
                    return Task.FromResult(Result.Success());
                }
                else
                {
                    _logger.LogBusinessWarning("Invalid token provided for validation");
                    return Task.FromResult(Result.Failure("Invalid token"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogUnexpectedError("ValidateToken", ex);
                return Task.FromResult(Result.Failure(ErrorCodes.Messages.InternalError));
            }
        }

        public async Task<Result<LogoutResponse>> LogoutAsync(string token)
        {
            using var operation = _logger.BeginOperation("UserLogout");
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogBusinessWarning("Logout attempted with empty token");
                    return Result.Success(new LogoutResponse
                    {
                        Success = true,
                        Message = "Logout successful"
                    });
                }

                // Validate token first
                if (!_jwtService.ValidateToken(token))
                {
                    _logger.LogBusinessWarning("Logout attempted with invalid token");
                    return Result.Success(new LogoutResponse
                    {
                        Success = true,
                        Message = "Logout successful"
                    });
                }

                // Blacklist the token
                await _jwtService.BlacklistTokenAsync(token);

                _logger.LogOperationSuccess("UserLogout");
                return Result.Success(new LogoutResponse
                {
                    Success = true,
                    Message = "Logout successful"
                });
            }
            catch (Exception ex)
            {
                _logger.LogUnexpectedError("UserLogout", ex);
                // Even if logout fails on server side, we still return success
                // to ensure client-side cleanup happens
                return Result.Success(new LogoutResponse
                {
                    Success = true,
                    Message = "Logout completed"
                });
            }
        }

        private static UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Region = user.Region,
                Country = user.Country,
                OmanPhone = user.OmanPhone,
                CountryPhone = user.CountryPhone,
                Status = user.Status.ToString()
            };
        }

        private static string GenerateSecureCode()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var code = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 1000000;
            return code.ToString("D6");
        }
    }
}