using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.DTOs;
using Infrastructure.Data;
using Infrastructure.Services;
using Core.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly JwtService _jwtService;

        public AuthController(
            ApplicationDbContext context,
            PasswordService passwordService,
            JwtService jwtService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Find user by name (the username field in login is actually the name)
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Name == request.Username && u.Status == "Active");

                if (user == null)
                {
                    return Unauthorized(new LoginResponse 
                    { 
                        Message = "Invalid name or password" 
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
                    Area = user.Area,
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
                    Area = request.Area,
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
                    Area = user.Area,
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
    }
} 