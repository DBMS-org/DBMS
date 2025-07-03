using Microsoft.AspNetCore.Mvc;
using Application.DTOs.UserManagement;
using Application.DTOs.Shared;
using Application.Interfaces.UserManagement;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService, 
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            
            if (result.IsFailure)
            {
                _logger.LogError("Error occurred while fetching users: {Error}", result.Error);
                return StatusCode(500, result.Error);
            }

            return Ok(result.Value);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);

            if (result.IsFailure)
            {
                if (result.Error.Contains("not found"))
                {
                    return NotFound(result.Error);
                }
                
                _logger.LogError("Error occurred while fetching user with ID {UserId}: {Error}", id, result.Error);
                return StatusCode(500, result.Error);
            }

            return Ok(result.Value);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUserAsync(request);

            if (result.IsFailure)
            {
                if (result.Error.Contains("already exists"))
                {
                    return BadRequest(result.Error);
                }
                
                _logger.LogError("Error occurred while creating user: {Error}", result.Error);
                return StatusCode(500, result.Error);
            }

            return CreatedAtAction(nameof(GetUser), new { id = result.Value.Id }, result.Value);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdateUserAsync(id, request);

            if (result.IsFailure)
            {
                if (result.Error.Contains("not found"))
                {
                    return NotFound(result.Error);
                }
                
                _logger.LogError("Error occurred while updating user with ID {UserId}: {Error}", id, result.Error);
                return StatusCode(500, result.Error);
            }

            return NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (result.IsFailure)
            {
                if (result.Error.Contains("not found"))
                {
                    return NotFound(result.Error);
                }
                
                _logger.LogError("Error occurred while deleting user with ID {UserId}: {Error}", id, result.Error);
                return StatusCode(500, result.Error);
            }

            return NoContent();
        }

        // GET: api/users/test-connection
        [HttpGet("test-connection")]
        public async Task<ActionResult> TestConnection()
        {
            var result = await _userService.TestConnectionAsync();

            if (result.IsFailure)
            {
                _logger.LogError("Database connection test failed: {Error}", result.Error);
                return StatusCode(500, new { 
                    message = "Database connection failed", 
                    error = result.Error 
                });
            }

            return Ok(new { 
                message = "Database connection successful!", 
                database = "DB-MS",
                timestamp = DateTime.UtcNow
            });
        }
    }
} 