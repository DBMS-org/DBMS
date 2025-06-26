using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "At least one role is required")]
        [MinLength(1, ErrorMessage = "At least one role must be assigned")]
        public List<int> RoleIds { get; set; } = new List<int>();
        
        [StringLength(100, ErrorMessage = "Region cannot exceed 100 characters")]
        public string Region { get; set; } = string.Empty;
        
        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
        public string Country { get; set; } = string.Empty;
        
        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string OmanPhone { get; set; } = string.Empty;
        
        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string CountryPhone { get; set; } = string.Empty;
        
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Active"; // Default to Active
    }
} 
