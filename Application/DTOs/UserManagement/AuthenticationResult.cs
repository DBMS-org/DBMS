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
} 