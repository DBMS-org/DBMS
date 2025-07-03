using Microsoft.AspNetCore.Mvc;
using Application.DTOs.UserManagement;
using Application.DTOs.Shared;
using Application.Interfaces.UserManagement;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _authService.LoginAsync(request);
                
                if (response.User == null || string.IsNullOrEmpty(response.Token))
                {
                    return Unauthorized(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login for username {Username}", request.Username);
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _authService.RegisterAsync(request);
                
                if (response.User == null)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration for user {Email}", request.Email);
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

                var response = await _authService.ForgotPasswordAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during forgot password for email {Email}", request.Email);
                return StatusCode(500, new ApiResponse 
                { 
                    Success = false, 
                    Message = "An error occurred while processing your request" 
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

                var response = await _authService.VerifyResetCodeAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during reset code verification for email {Email}", request.Email);
                return StatusCode(500, new ApiResponse 
                { 
                    Success = false, 
                    Message = "An error occurred while verifying the reset code" 
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

                var response = await _authService.ResetPasswordAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during password reset for email {Email}", request.Email);
                return StatusCode(500, new ApiResponse 
                { 
                    Success = false, 
                    Message = "An error occurred while resetting your password" 
                });
            }
        }

        [HttpPost("validate-token")]
        public async Task<ActionResult<ApiResponse>> ValidateToken()
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new ApiResponse 
                    { 
                        Success = false, 
                        Message = "No token provided" 
                    });
                }

                var response = await _authService.ValidateTokenAsync(token);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during token validation");
                return StatusCode(500, new ApiResponse 
                { 
                    Success = false, 
                    Message = "An error occurred while validating the token" 
                });
            }
        }
    }
} 