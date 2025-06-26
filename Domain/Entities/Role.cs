namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        
        // Business logic methods
        public void Activate()
        {
            IsActive = true;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public void Deactivate()
        {
            IsActive = false;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public void UpdateNormalizedName()
        {
            NormalizedName = Name.ToUpperInvariant();
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
    }
} 
