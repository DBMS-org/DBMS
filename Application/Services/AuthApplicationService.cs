using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Application.Services
{
    public class AuthApplicationService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthApplicationService> _logger;

        public AuthApplicationService(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IJwtService jwtService,
            IEmailService emailService,
            ILogger<AuthApplicationService> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                // Find user by name regardless of status
                var user = await _userRepository.GetByNameAsync(request.Username);

                if (user == null)
                {
                    return new LoginResponse 
                    { 
                        Message = "Invalid name or password" 
                    };
                }

                // Check if account is inactive
                if (user.Status != "Active")
                {
                    return new LoginResponse 
                    { 
                        Message = $"Your account is {user.Status.ToLower()}. Please contact your administrator." 
                    };
                }

                // Verify password
                if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
                {
                    return new LoginResponse 
                    { 
                        Message = "Invalid name or password" 
                    };
                }

                // Update last login time
                user.LastLoginAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                // Generate token
                var token = _jwtService.GenerateToken(user);

                // Create user DTO
                var userDto = MapToUserDto(user);

                return new LoginResponse
                {
                    Token = token,
                    User = userDto,
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for username {Username}", request.Username);
                return new LoginResponse 
                { 
                    Message = "An error occurred during login" 
                };
            }
        }

        public async Task<LoginResponse> RegisterAsync(CreateUserRequest request)
        {
            try
            {
                // Check if user already exists by name or email
                var existingUser = await _userRepository.ExistsByNameOrEmailAsync(request.Name, request.Email);

                if (existingUser)
                {
                    return new LoginResponse 
                    { 
                        Message = "User with this name or email already exists" 
                    };
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
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdUser = await _userRepository.CreateAsync(user);

                // Generate token
                var token = _jwtService.GenerateToken(createdUser);

                // Create user DTO
                var userDto = MapToUserDto(createdUser);

                return new LoginResponse
                {
                    Token = token,
                    User = userDto,
                    Message = "Registration successful"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user {Email}", request.Email);
                return new LoginResponse 
                { 
                    Message = "An error occurred during registration" 
                };
            }
        }

        public async Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            try
            {
                // Find user by email
                var user = await _userRepository.GetByEmailAsync(request.Email);

                if (user == null || user.Status != "Active")
                {
                    // For security, don't reveal if email exists or not
                    return new ApiResponse 
                    { 
                        Success = true, 
                        Message = "If the email exists, a password reset code has been sent." 
                    };
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
                    // Don't fail the entire operation if email fails
                }

                return new ApiResponse 
                { 
                    Success = true, 
                    Message = "If the email exists, a password reset code has been sent." 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing forgot password request for email {Email}", request.Email);
                return new ApiResponse 
                { 
                    Success = false, 
                    Message = "An error occurred while processing your request." 
                };
            }
        }

        public async Task<ApiResponse> VerifyResetCodeAsync(VerifyResetCodeRequest request)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);

                if (user == null || 
                    user.Status != "Active" ||
                    string.IsNullOrEmpty(user.PasswordResetCode) ||
                    user.PasswordResetCodeExpiry == null ||
                    user.PasswordResetCodeExpiry < DateTime.UtcNow ||
                    user.PasswordResetCode != request.Code)
                {
                    return new ApiResponse 
                    { 
                        Success = false, 
                        Message = "Invalid or expired reset code." 
                    };
                }

                return new ApiResponse 
                { 
                    Success = true, 
                    Message = "Reset code is valid. You may now set a new password." 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying reset code for email {Email}", request.Email);
                return new ApiResponse 
                { 
                    Success = false, 
                    Message = "An error occurred while verifying the reset code." 
                };
            }
        }

        public async Task<ApiResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);

                if (user == null || 
                    user.Status != "Active" ||
                    string.IsNullOrEmpty(user.PasswordResetCode) ||
                    user.PasswordResetCodeExpiry == null ||
                    user.PasswordResetCodeExpiry < DateTime.UtcNow ||
                    user.PasswordResetCode != request.Code)
                {
                    return new ApiResponse 
                    { 
                        Success = false, 
                        Message = "Invalid or expired reset code." 
                    };
                }

                // Hash new password
                user.PasswordHash = _passwordService.HashPassword(request.NewPassword);
                
                // Clear reset code
                user.PasswordResetCode = null;
                user.PasswordResetCodeExpiry = null;
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                return new ApiResponse 
                { 
                    Success = true, 
                    Message = "Password has been successfully reset." 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for email {Email}", request.Email);
                return new ApiResponse 
                { 
                    Success = false, 
                    Message = "An error occurred while resetting your password." 
                };
            }
        }

        public async Task<ApiResponse> ValidateTokenAsync(string token)
        {
            try
            {
                var isValid = _jwtService.ValidateToken(token);
                
                if (!isValid)
                {
                    return new ApiResponse 
                    { 
                        Success = false, 
                        Message = "Invalid token" 
                    };
                }
                
                return new ApiResponse 
                { 
                    Success = true, 
                    Message = "Token is valid" 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                return new ApiResponse 
                { 
                    Success = false, 
                    Message = "Token validation failed" 
                };
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
                Status = user.Status
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