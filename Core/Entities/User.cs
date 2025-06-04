using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Region { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;
        
        [Phone]
        [MaxLength(20)]
        public string OmanPhone { get; set; } = string.Empty;
        
        [Phone]
        [MaxLength(20)]
        public string CountryPhone { get; set; } = string.Empty;
        
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
} 