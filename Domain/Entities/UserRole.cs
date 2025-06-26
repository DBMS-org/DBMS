namespace Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
        
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Business logic methods
        public void Revoke()
        {
            IsActive = false;
            RevokedAt = DateTime.UtcNow;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public void Activate()
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
