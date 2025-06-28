using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class LoginRequest
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;  // This will actually be the Name field
        
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
} 
