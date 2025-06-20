using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.DTOs;
using Infrastructure.Data;
using Infrastructure.Services;
using Core.Entities;
using System.Security.Cryptography;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly JwtService _jwtService;
        private readonly IEmailService _emailService;

        public AuthController(
            ApplicationDbContext context,
            PasswordService passwordService,
            JwtService jwtService,
            IEmailService emailService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        [HttpPost("login")] //login post controller api
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                // First find user by name regardless of status
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Name == request.Username);

                if (user == null)
                {
                    return Unauthorized(new LoginResponse 
                    { 
                        Message = "Invalid name or password" 
                    });
                }

                // Check if account is inactive
                if (user.Status != "Active")
                {
                    return Unauthorized(new LoginResponse 
                    { 
                        Message = $"Your account is {user.Status.ToLower()}. Please contact your administrator." 
                    });
                }

                // Verify password
                if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
                {
                    return Unauthorized(new LoginResponse 
                    { 
                        Message = "Invalid name or password" 
                    });
                }

                // Update last login time
                user.LastLoginAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                // Generate token
                var token = _jwtService.GenerateToken(user);

                // Create user DTO
                var userDto = new UserDto
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

                return Ok(new LoginResponse
                {
                    Token = token,
                    User = userDto,
                    Message = "Login successful"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LoginResponse 
                { 
                    Message = "An error occurred during login" 
                });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] CreateUserRequest request)
        {
            try
            {
                // Check if user already exists by name or email
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Name == request.Name || u.Email == request.Email);

                if (existingUser != null)
                {
                    return BadRequest(new LoginResponse 
                    { 
                        Message = "User with this name or email already exists" 
                    });
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

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Generate token
                var token = _jwtService.GenerateToken(user);

                // Create user DTO
                var userDto = new UserDto
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

                return Ok(new LoginResponse
                {
                    Token = token,
                    User = userDto,
                    Message = "Registration successful"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LoginResponse 
                { 
                    Message = "An error occurred during registration" 
                });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse 
                    { 
                        Success = false, 
                        Message = "Invalid request data" 
                    });
                }

                // Find user by email
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email && u.Status == "Active");

                if (user == null)
                {
                    // For security, don't reveal if email exists or not
                    return Ok(new ApiResponse 
                    { 
                        Success = true, 
                        Message = "If the email exists, a reset code has been sent." 
                    });
                }

                // Generate 6-digit code
                var code = GenerateSecureCode();

                // Invalidate any existing codes for this user
                var existingCodes = await _context.PasswordResetCodes
                    .Where(c => c.UserId == user.Id && !c.IsUsed && c.ExpiresAt > DateTime.UtcNow)
                    .ToListAsync();

                foreach (var existingCode in existingCodes)
                {
                    existingCode.IsUsed = true;
                }

                // Create new reset code
                var resetCode = new PasswordResetCode
                {
                    UserId = user.Id,
                    Code = code,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(10), // 10 minutes expiration
                    CreatedAt = DateTime.UtcNow
                };

                _context.PasswordResetCodes.Add(resetCode);
                await _context.SaveChangesAsync();

                // Send email with code
                var emailSent = await _emailService.SendPasswordResetCodeAsync(user.Email, code, user.Name);

                // For testing purposes, include the code in the response
                // In production, remove the "Data" property
                return Ok(new ApiResponse 
                { 
                    Success = true, 
                    Message = emailSent 
                        ? "Reset code sent to your email address." 
                        : "Reset code generated (check console logs for testing).",
                    Data = new { 
                        TestingCode = code, // Remove this in production
                        Note = "This code is shown for testing purposes only" 
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse 
                { 
                    Success = false, 
                    Message = "An error occurred while processing your request." 
                });
            }
        }

        [HttpPost("verify-reset-code")]
        public async Task<ActionResult<ApiResponse>> VerifyResetCode([FromBody] VerifyResetCodeRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse 
                    { 
                        Success = false, 
                        Message = "Invalid request data" 
                    });
                }

                // Find user by email
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email && u.Status == "Active");

                if (user == null)
                {
                    return BadRequest(new ApiResponse 
                    { 
                        Success = false, 
                        Message = "Invalid email or code." 
                    });
                }

                // Find valid reset code
                var resetCode = await _context.PasswordResetCodes
                    .FirstOrDefaultAsync(c => c.UserId == user.Id && 
                                            c.Code == request.Code && 
                                            !c.IsUsed && 
                                            c.ExpiresAt > DateTime.UtcNow);

                if (resetCode == null)
                {
                    // Increment attempt count for rate limiting
                    var anyCode = await _context.PasswordResetCodes
                        .Where(c => c.UserId == user.Id && !c.IsUsed)
                        .OrderByDescending(c => c.CreatedAt)
                        .FirstOrDefaultAsync();

                    if (anyCode != null)
                    {
                        anyCode.AttemptCount++;
                        await _context.SaveChangesAsync();

                        if (anyCode.AttemptCount >= 5)
                        {
                            anyCode.IsUsed = true;
                            await _context.SaveChangesAsync();
                            
                            return BadRequest(new ApiResponse 
                            { 
                                Success = false, 
                                Message = "Too many incorrect attempts. Please request a new code." 
                            });
                        }
                    }

                    return BadRequest(new ApiResponse 
                    { 
                        Success = false, 
                        Message = "Invalid or expired code." 
                    });
                }

                return Ok(new ApiResponse 
                { 
                    Success = true, 
                    Message = "Code verified successfully. You can now reset your password." 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse 
                { 
                    Success = false, 
                    Message = "An error occurred while verifying the code." 
                });
            }
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse 
                    { 
                        Success = false, 
                        Message = "Invalid request data" 
                    });
                }

                // Find user by email
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email && u.Status == "Active");

                if (user == null)
                {
                    return BadRequest(new ApiResponse 
                    { 
                        Success = false, 
                        Message = "Invalid email or code." 
                    });
                }

                // Find and validate reset code
                var resetCode = await _context.PasswordResetCodes
                    .FirstOrDefaultAsync(c => c.UserId == user.Id && 
                                            c.Code == request.Code && 
                                            !c.IsUsed && 
                                            c.ExpiresAt > DateTime.UtcNow);

                if (resetCode == null)
                {
                    return BadRequest(new ApiResponse 
                    { 
                        Success = false, 
                        Message = "Invalid or expired code." 
                    });
                }

                // Update user password
                user.PasswordHash = _passwordService.HashPassword(request.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;

                // Mark code as used
                resetCode.IsUsed = true;
                resetCode.UsedAt = DateTime.UtcNow;

                // Invalidate all other codes for this user
                var otherCodes = await _context.PasswordResetCodes
                    .Where(c => c.UserId == user.Id && !c.IsUsed)
                    .ToListAsync();

                foreach (var code in otherCodes)
                {
                    code.IsUsed = true;
                }

                await _context.SaveChangesAsync();

                return Ok(new ApiResponse 
                { 
                    Success = true, 
                    Message = "Password reset successfully. You can now login with your new password." 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse 
                { 
                    Success = false, 
                    Message = "An error occurred while resetting the password." 
                });
            }
        }

        private string GenerateSecureCode()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[4];
                rng.GetBytes(bytes);
                var randomNumber = BitConverter.ToUInt32(bytes, 0);
                return (randomNumber % 1000000).ToString("D6");
            }
        }
    }
} 