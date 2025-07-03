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
                return Conflict(result.Error);
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
                return BadRequest("ID mismatch");
            }
            
            var result = await _userService.UpdateUserAsync(id, request);
            if (result.IsFailure)
            {
                return NotFound();
            }
            return Ok();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result.IsFailure)
            {
                return NotFound();
            }
            return Ok();
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