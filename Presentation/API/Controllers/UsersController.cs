using Microsoft.AspNetCore.Mvc;
using Application.DTOs.UserManagement;
using Application.Interfaces.UserManagement;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result.Value);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (result.IsFailure)
            {
                return NotFound();
            }
            return Ok(result.Value);
        }

        // POST: api/users
        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            var result = await _userService.CreateUserAsync(request);
            if (result.IsFailure)
            {
                // Check if it's a duplicate email error
                if (result.Error.Contains("Email already in use") ||
                    result.Error.Contains("already exists"))
                {
                    return Conflict(new
                    {
                        message = "Email already in use",
                        detail = "The email address you entered is already registered. Please use a different email address.",
                        errors = result.Errors.Length > 0 ? result.Errors : new[] { result.Error }
                    });
                }

                // For validation errors, return structured error response
                return BadRequest(new
                {
                    message = "Validation failed",
                    detail = "Please correct the errors below and try again.",
                    errors = result.Errors.Length > 0 ? result.Errors : new[] { result.Error }
                });
            }
            return CreatedAtAction(nameof(GetUser), new { id = result.Value.Id }, result.Value);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest(new
                {
                    message = "Validation failed",
                    detail = "The user ID in the URL does not match the ID in the request body.",
                    errors = new[] { "ID mismatch" }
                });
            }

            var result = await _userService.UpdateUserAsync(id, request);
            if (result.IsFailure)
            {
                // Check if user not found
                if (result.Error.Contains("not found"))
                {
                    return NotFound(new
                    {
                        message = "User not found",
                        detail = $"No user found with ID {id}.",
                        errors = new[] { result.Error }
                    });
                }

                // For validation errors
                return BadRequest(new
                {
                    message = "Validation failed",
                    detail = "Please correct the errors below and try again.",
                    errors = result.Errors.Length > 0 ? result.Errors : new[] { result.Error }
                });
            }
            return Ok();
        }

        // PUT: api/users/5/deactivate
        [HttpPut("{id}/deactivate")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var result = await _userService.DeactivateUserAsync(id);
            if (result.IsFailure)
            {
                // Check if user not found
                if (result.Error.Contains("not found"))
                {
                    return NotFound(new
                    {
                        message = "User not found",
                        detail = $"No user found with ID {id}.",
                        errors = new[] { result.Error }
                    });
                }

                // Check if already deactivated
                if (result.Error.Contains("already deactivated"))
                {
                    return BadRequest(new
                    {
                        message = "User already deactivated",
                        detail = "This user account is already deactivated.",
                        errors = new[] { result.Error }
                    });
                }

                return BadRequest(new
                {
                    message = "Deactivation failed",
                    detail = result.Error,
                    errors = new[] { result.Error }
                });
            }
            return Ok(new { message = "User deactivated successfully" });
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result.IsFailure)
            {
                // Check if user not found
                if (result.Error.Contains("not found") || result.Error.Contains("could not be found"))
                {
                    return NotFound(new
                    {
                        message = "User not found",
                        detail = result.Error,
                        errors = new[] { result.Error }
                    });
                }

                // For other errors
                return BadRequest(new
                {
                    message = "Deletion failed",
                    detail = result.Error,
                    errors = new[] { result.Error }
                });
            }

            return Ok(new
            {
                message = "User deleted successfully",
                detail = "The user account has been permanently deleted from the system."
            });
        }

        // GET: api/users/test-connection
        [HttpGet("test-connection")]
        [AllowAnonymous]
        public async Task<IActionResult> TestConnection()
        {
            var result = await _userService.TestConnectionAsync();
            if (result.IsFailure)
            {
                return InternalServerError(result.Error);
            }
            return Ok();
        }
    }
} 