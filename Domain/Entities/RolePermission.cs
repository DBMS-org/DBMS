

namespace Domain.Entities
{
    public class RolePermission
    {
        public int Id { get; set; }
        
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
        
        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; } = null!;
        
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 
