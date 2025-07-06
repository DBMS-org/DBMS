using Microsoft.AspNetCore.Mvc;
using Application.DTOs.UserManagement;
using Application.DTOs.Shared;
using Application.Interfaces.UserManagement;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                
                var logoutResult = await _authService.LogoutAsync(token ?? string.Empty);
                
                if (logoutResult.IsFailure)
                {
                    return BadRequest(logoutResult.Error);
                }

                return Ok(logoutResult.Value);
        }

        [HttpGet("debug/current-user")]
        public async Task<IActionResult> GetCurrentUserDebug()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var userRegion = User.FindFirst("region")?.Value;
                
                var debugInfo = new
                {
                    UserIdClaim = userIdClaim,
                    UserIdParsed = int.TryParse(userIdClaim, out var id) ? id : (int?)null,
                    UserName = userName,
                    UserEmail = userEmail,
                    UserRole = userRole,
                    UserRegion = userRegion,
                    IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
                    AllClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
                };
                
                return Ok(debugInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
} 