using Microsoft.AspNetCore.Mvc;
using Application.DTOs.UserManagement;
using Application.DTOs.Shared;
using Application.Interfaces.UserManagement;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
                var authResult = await _authService.LoginAsync(request);
                
                if (authResult.IsFailure || authResult.Value == null)
                {
                return Unauthorized("Invalid credentials.");
                }

                var loginResp = new LoginResponse
                {
                    Token = authResult.Value.Token,
                    User = authResult.Value.User,
                    Message = "Login successful"
                };

                return Ok(loginResp);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {
                var regResult = await _authService.RegisterAsync(request);
                
                if (regResult.IsFailure || regResult.Value == null)
                {
                return BadRequest(regResult.Error);
                }

                var registerResp = new LoginResponse
                {
                    Token = regResult.Value.Token,
                    User = regResult.Value.User,
                    Message = "Registration successful"
                };

                return Ok(registerResp);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
                var response = await _authService.ForgotPasswordAsync(request);
                return Ok(response);
        }

        [HttpPost("verify-reset-code")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeRequest request)
        {
                var response = await _authService.VerifyResetCodeAsync(request);
                return Ok(response);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
                var response = await _authService.ResetPasswordAsync(request);
                return Ok(response);
        }

        [HttpPost("validate-token")]
        [Authorize]
        public async Task<IActionResult> ValidateToken()
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                
                if (string.IsNullOrEmpty(token))
                {
                return BadRequest("No token provided");
                }

                var response = await _authService.ValidateTokenAsync(token);
                return Ok(response);
        }
    }
} 