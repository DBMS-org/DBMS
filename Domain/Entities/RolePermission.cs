namespace Domain.Entities
{
    public class RolePermission : BaseEntity
    {
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
        
        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; } = null!;
        
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Business logic methods
        public void Revoke()
        {
            IsActive = false;
            RevokedAt = DateTime.UtcNow;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public void Grant()
        {
            IsActive = true;
            RevokedAt = null;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public bool IsExpired()
        {
            return RevokedAt.HasValue && RevokedAt.Value < DateTime.UtcNow;
        }
    }
} 
