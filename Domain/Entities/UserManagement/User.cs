using Domain.Common;
using Domain.Entities.UserManagement;

namespace Domain.Entities.UserManagement
{
    public class User : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        
        public Email Email { get; set; } = new Email("example@example.com");
        
        public string PasswordHash { get; set; } = string.Empty;
        
        public string Role { get; set; } = string.Empty;
        
        public UserStatus Status { get; set; } = UserStatus.Active;
        
        public string Region { get; set; } = string.Empty;
        
        public string Country { get; set; } = string.Empty;
        
        public string OmanPhone { get; set; } = string.Empty;
        
        public string CountryPhone { get; set; } = string.Empty;
        
        public DateTime? LastLoginAt { get; set; }
        public string? PasswordResetCode { get; set; }
        public DateTime? PasswordResetCodeExpiry { get; set; }
        
        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<StoreManagement.Store> ManagedStores { get; set; } = new List<StoreManagement.Store>();
    }
}
