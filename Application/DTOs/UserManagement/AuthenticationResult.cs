namespace Application.DTOs.UserManagement
{
    /// <summary>
    /// Represents the result of successful authentication operations (login/register)
    /// </summary>
    public class AuthenticationResult
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new UserDto();
    }

    /// <summary>
    /// Request for user logout
    /// </summary>
    public class LogoutRequest
    {
        public string Token { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response for user logout
    /// </summary>
    public class LogoutResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
} 