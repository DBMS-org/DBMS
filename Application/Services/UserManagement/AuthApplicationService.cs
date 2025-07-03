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
                    return Result.Failure<AuthenticationResult>(
                        $"Your account is {SafeDataConverter.SafeToLower(user.Status.ToString())}. Please contact your administrator.");
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
            try
            {
                // Validate input
                var validationResult = await _validationService.ValidateAsync(request);
                if (validationResult.IsFailure)
                {
                    return Result.Failure<AuthenticationResult>(validationResult.Errors);
                }

                // Check if user already exists by name or email
                var existingUser = await _userRepository.ExistsByNameOrEmailAsync(request.Name, request.Email);

                if (existingUser)
                {
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
                    Status = UserStatus.Active,
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

                return Result.Success(authResult);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed for registration {Email}: {Errors}", 
                    request.Email, string.Join(", ", ex.ValidationErrors));
                return Result.Failure<AuthenticationResult>(ex.ValidationErrors);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error during registration for user {Email}", request.Email);
                return Result.Failure<AuthenticationResult>("Registration service temporarily unavailable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for user {Email}", request.Email);
                return Result.Failure<AuthenticationResult>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            try
            {
                // Find user by email
                var user = await _userRepository.GetByEmailAsync(request.Email);

                if (user == null || user.Status != UserStatus.Active)
                {
                    // For security, don't reveal if email exists or not
                    return Result.Success();
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
                    _logger.LogInformation("Password reset code sent to user {Email}", request.Email);
                }
                catch (Exception emailEx)
                {
                    _logger.LogWarning(emailEx, "Failed to send password reset email to {Email}, but code was generated", request.Email);
                    // Don't fail the entire operation if email fails - user can still use the code
                }

                return Result.Success();
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error processing forgot password request for email {Email}", request.Email);
                return Result.Failure("Password reset service temporarily unavailable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error processing forgot password request for email {Email}", request.Email);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> VerifyResetCodeAsync(VerifyResetCodeRequest request)
        {
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
                    return Result.Failure("Invalid or expired reset code.");
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying reset code for email {Email}", request.Email);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
        {
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
                    return Result.Failure("Invalid or expired reset code.");
                }

                // Hash new password
                user.PasswordHash = _passwordService.HashPassword(request.NewPassword);
                
                // Clear reset code
                user.PasswordResetCode = null;
                user.PasswordResetCodeExpiry = null;
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for email {Email}", request.Email);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> ValidateTokenAsync(string token)
        {
            try
            {
                var isValid = _jwtService.ValidateToken(token);
                
                if (!isValid)
                {
                    return Result.Failure("Invalid token");
                }
                
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                return Result.Failure(ErrorCodes.Messages.InternalError);
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