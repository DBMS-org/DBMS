namespace Domain.Entities
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
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
        
        public string GetFullPermissionName()
        {
            return $"{Module}.{Action}";
        }
    }
} 
