using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities;
using Application.DTOs;
using Infrastructure.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsersController> _logger;
        private readonly PasswordService _passwordService;

        public UsersController(
            ApplicationDbContext context, 
            ILogger<UsersController> logger,
            PasswordService passwordService)
        {
            _context = context;
            _logger = logger;
            _passwordService = passwordService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        Role = u.Role,
                        Status = u.Status,
                        Region = u.Region,
                        Country = u.Country,
                        OmanPhone = u.OmanPhone,
                        CountryPhone = u.CountryPhone,
                        CreatedAt = DateTime.SpecifyKind(u.CreatedAt, DateTimeKind.Utc),
                        UpdatedAt = DateTime.SpecifyKind(u.UpdatedAt, DateTimeKind.Utc)
                    })
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users");
                return StatusCode(500, "Internal server error occurred while fetching users");
            }
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                // Debug logging to check timezone handling
                _logger.LogInformation("Raw DB timestamps for user {UserId}: CreatedAt={CreatedAt}, UpdatedAt={UpdatedAt}", 
                    id, user.CreatedAt, user.UpdatedAt);
                _logger.LogInformation("Current UTC time: {UtcNow}", DateTime.UtcNow);
                _logger.LogInformation("Current local time: {LocalNow}", DateTime.Now);

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    Status = user.Status,
                    Region = user.Region,
                    Country = user.Country,
                    OmanPhone = user.OmanPhone,
                    CountryPhone = user.CountryPhone,
                    CreatedAt = DateTime.SpecifyKind(user.CreatedAt, DateTimeKind.Utc),
                    UpdatedAt = DateTime.SpecifyKind(user.UpdatedAt, DateTimeKind.Utc)
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user with ID {UserId}", id);
                return StatusCode(500, "Internal server error occurred while fetching user");
            }
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Hash the password
                var hashedPassword = _passwordService.HashPassword(request.Password);

                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = hashedPassword,
                    Role = request.Role,
                    Status = "Active", // Default status
                    Region = request.Region,
                    Country = request.Country,
                    OmanPhone = request.OmanPhone,
                    CountryPhone = request.CountryPhone
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    Status = user.Status,
                    Region = user.Region,
                    Country = user.Country,
                    OmanPhone = user.OmanPhone,
                    CountryPhone = user.CountryPhone,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                };

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
            }
            catch (DbUpdateException ex)
            {
                // Handle unique constraint violations
                if (ex.InnerException?.Message.Contains("IX_Users_Email") == true ||
                    ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true ||
                    ex.InnerException?.Message.Contains("duplicate key") == true)
                {
                    _logger.LogWarning("Attempted to create user with duplicate email: {Email}", request.Email);
                    return BadRequest("User with this email already exists");
                }
                
                _logger.LogError(ex, "Database error occurred while creating user");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user");
                return StatusCode(500, "Internal server error occurred while creating user");
            }
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest request)
        {
            try
            {
                _logger.LogInformation("UpdateUser called for ID: {UserId} with data: {@Request}", id, request);
                
                if (id != request.Id)
                {
                    return BadRequest("ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                _logger.LogInformation("User found. Current UpdatedAt: {CurrentUpdatedAt}", user.UpdatedAt);

                user.Name = request.Name;
                user.Email = request.Email;
                user.Role = request.Role;
                user.Status = request.Status;
                user.Region = request.Region;
                user.Country = request.Country;
                user.OmanPhone = request.OmanPhone;
                user.CountryPhone = request.CountryPhone;
                user.UpdatedAt = DateTime.UtcNow;

                _logger.LogInformation("Setting new UpdatedAt: {NewUpdatedAt}", user.UpdatedAt);

                await _context.SaveChangesAsync();

                _logger.LogInformation("User updated successfully. Final UpdatedAt: {FinalUpdatedAt}", user.UpdatedAt);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user with ID {UserId}", id);
                return StatusCode(500, "Internal server error occurred while updating user");
            }
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user with ID {UserId}", id);
                return StatusCode(500, "Internal server error occurred while deleting user");
            }
        }

        // GET: api/users/test-connection
        [HttpGet("test-connection")]
        public async Task<ActionResult> TestConnection()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                if (canConnect)
                {
                    var userCount = await _context.Users.CountAsync();
                    return Ok(new { 
                        message = "Database connection successful!", 
                        database = "DB-MS",
                        userCount = userCount,
                        timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return StatusCode(500, "Unable to connect to database");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database connection test failed");
                return StatusCode(500, new { 
                    message = "Database connection failed", 
                    error = ex.Message 
                });
            }
        }
    }
} 
