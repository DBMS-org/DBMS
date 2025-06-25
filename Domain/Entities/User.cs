

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        


        public string Name { get; set; } = string.Empty;
        



        public string Email { get; set; } = string.Empty;
        

        public string PasswordHash { get; set; } = string.Empty;
        


        public string Role { get; set; } = string.Empty;
        


        public string Status { get; set; } = string.Empty;
        

        public string Region { get; set; } = string.Empty;
        

        public string Country { get; set; } = string.Empty;
        


        public string OmanPhone { get; set; } = string.Empty;
        


        public string CountryPhone { get; set; } = string.Empty;
        
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
} 
