namespace Application.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new UserDto();
        public string Message { get; set; } = string.Empty;
    }
} 
