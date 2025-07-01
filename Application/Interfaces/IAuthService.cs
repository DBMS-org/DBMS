using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<LoginResponse> RegisterAsync(CreateUserRequest request);
        Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<ApiResponse> VerifyResetCodeAsync(VerifyResetCodeRequest request);
        Task<ApiResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<ApiResponse> ValidateTokenAsync(string token);
    }
} 