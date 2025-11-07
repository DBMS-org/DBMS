namespace Application.DTOs.UserManagement
{
    public class AuthenticationResult
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new UserDto();
    }

    public class LogoutRequest
    {
        public string Token { get; set; } = string.Empty;
    }

    public class LogoutResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
} 