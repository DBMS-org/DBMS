using Application.DTOs.UserManagement;
using Application.DTOs.Shared;

namespace Application.Interfaces.UserManagement
{
    public interface IAuthService
    {
        Task<Result<AuthenticationResult>> LoginAsync(LoginRequest request);
        Task<Result<AuthenticationResult>> RegisterAsync(CreateUserRequest request);
        Task<Result> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<Result> VerifyResetCodeAsync(VerifyResetCodeRequest request);
        Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
        Task<Result> ValidateTokenAsync(string token);
        Task<Result<LogoutResponse>> LogoutAsync(string token);
    }
} 