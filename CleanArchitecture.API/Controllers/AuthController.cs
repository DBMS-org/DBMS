using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using System.Security.Authentication;
using CleanArchitecture.Application.DTOs;
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
        public async Task<ActionResult<AuthResultDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var command = new LoginCommand
                {
                    Username = loginDto.Username,
                    Password = loginDto.Password
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
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            try
            {
                var command = new ForgotPasswordCommand
                {
                    Email = model.Email
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
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (model.NewPassword != model.ConfirmPassword)
            {
                return BadRequest(new { message = "Passwords do not match" });
            }
            
            try
            {
                var command = new ResetPasswordCommand
                {
                    Email = model.Email,
                    Token = model.Token,
                    NewPassword = model.NewPassword
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
        public async Task<ActionResult<UserDto>> GetUserInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
                
            // Get user info through a query
            // This would be implemented with a MediatR query
            
            return Ok(new UserDto
            {
                Id = int.Parse(userId),
                Username = User.FindFirst(ClaimTypes.Name)?.Value,
                Email = User.FindFirst(ClaimTypes.Email)?.Value,
                Roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList()
            });
        }
    }
}