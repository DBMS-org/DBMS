using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using System.Security.Authentication;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Application.Commands;

namespace CleanArchitecture.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] User loginUser)
        {
            try
            {
                var command = new LoginCommand
                {
                    Username = loginUser.Username,
                    Password = loginUser.PasswordHash // Note: In a real app, you'd need to handle password hashing properly
                };
                
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (AuthenticationException ex)
            {
                _logger.LogWarning($"Authentication failed: {ex.Message}");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login error: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] User user)
        {
            try
            {
                var command = new ForgotPasswordCommand
                {
                    Email = user.Email
                };
                
                await _mediator.Send(command);
                
                // Always return success to prevent email enumeration attacks
                return Ok(new { message = "If your email is registered, you will receive a password reset link" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Forgot password error: {ex.Message}");
                // Still return OK to prevent email enumeration
                return Ok(new { message = "If your email is registered, you will receive a password reset link" });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] User user)
        {
            try
            {
                var command = new ResetPasswordCommand
                {
                    Email = user.Email,
                    NewPassword = user.PasswordHash // Note: In a real app, you'd need to handle password hashing properly
                };
                
                var result = await _mediator.Send(command);
                
                if (result)
                    return Ok(new { message = "Password has been reset successfully" });
                else
                    return BadRequest(new { message = "Password reset failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Reset password error: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred during password reset" });
            }
        }
        
        [Authorize]
        [HttpGet("user-info")]
        public async Task<ActionResult<User>> GetUserInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
                
            // Get user info through a query
            // This would be implemented with a MediatR query
            
            return Ok(new User
            {
                Id = int.Parse(userId),
                Username = User.FindFirst(ClaimTypes.Name)?.Value,
                Email = User.FindFirst(ClaimTypes.Email)?.Value
            });
        }
    }
}