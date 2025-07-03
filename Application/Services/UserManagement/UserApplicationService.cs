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
        private readonly IStructuredLogger<UserApplicationService> _logger;

        private const string AllUsersCacheKey = "all_users";
        private static string UserByIdCacheKey(int id) => $"user_{id}";

        public UserApplicationService(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IValidationService validationService,
            ICacheService cacheService,
            IStructuredLogger<UserApplicationService> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _validationService = validationService;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            using var operation = _logger.BeginOperation("GetAllUsers");
            try
            {
                var userDtos = await _cacheService.GetOrCreateAsync(AllUsersCacheKey, async () =>
                {
                    var users = await _userRepository.GetAllAsync();
                    return users.Select(MapToDto);
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
                    return user == null ? null : MapToDto(user);
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

                // Create user entity
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = hashedPassword,
                    Role = request.Role,
                    Status = UserStatus.Active,
                    Region = request.Region,
                    Country = request.Country,
                    OmanPhone = request.OmanPhone,
                    CountryPhone = request.CountryPhone,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdUser = await _userRepository.CreateAsync(user);

                // Invalidate cache
                _cacheService.Remove(AllUsersCacheKey);

                _logger.LogOperationSuccess("CreateUser", new { UserId = createdUser.Id, Email = createdUser.Email });
                return Result.Success(MapToDto(createdUser));
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

                // Update user properties
                user.Name = request.Name;
                user.Email = request.Email;
                user.Role = request.Role;
                user.Status = SafeDataConverter.ParseEnumWithDefault<UserStatus>(request.Status, user.Status, "Status");
                user.Region = request.Region;
                user.Country = request.Country;
                user.OmanPhone = request.OmanPhone;
                user.CountryPhone = request.CountryPhone;
                user.UpdatedAt = DateTime.UtcNow;

                var updateResult = await _userRepository.UpdateAsync(user);
                
                // Invalidate cache
                _cacheService.Remove(AllUsersCacheKey);
                _cacheService.Remove(UserByIdCacheKey(id));

                if (updateResult)
                {
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

        public async Task<Result> DeleteUserAsync(int id)
        {
            using var operation = _logger.BeginOperation("DeleteUser", new { UserId = id });
            try
            {
                var deleteResult = await _userRepository.DeleteAsync(id);

                // Invalidate cache
                _cacheService.Remove(AllUsersCacheKey);
                _cacheService.Remove(UserByIdCacheKey(id));

                if (deleteResult)
                {
                    _logger.LogOperationSuccess("DeleteUser", new { UserId = id });
                    return Result.Success();
                }
                else
                {
                    _logger.LogBusinessWarning("User not found for deletion", new { UserId = id });
                    return Result.Failure(ErrorCodes.Messages.UserNotFound(id));
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

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Status = user.Status.ToString(),
                Region = user.Region,
                Country = user.Country,
                OmanPhone = user.OmanPhone,
                CountryPhone = user.CountryPhone,
                CreatedAt = DateTime.SpecifyKind(user.CreatedAt, DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(user.UpdatedAt, DateTimeKind.Utc)
            };
        }
    }
} 