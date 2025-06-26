namespace Domain.Entities
{
    public class User : BaseEntity
    {

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string Region { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string OmanPhone { get; set; } = string.Empty;

        public string CountryPhone { get; set; } = string.Empty;
        
        public DateTime? LastLoginAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<Machine> OperatedMachines { get; set; } = new List<Machine>();
        public virtual ICollection<Project> AssignedProjects { get; set; } = new List<Project>();
        public virtual ICollection<DrillPattern> CreatedDrillPatterns { get; set; } = new List<DrillPattern>();
        public virtual ICollection<BlastSequence> CreatedBlastSequences { get; set; } = new List<BlastSequence>();
        public virtual ICollection<SiteBlastingData> CreatedBlastingData { get; set; } = new List<SiteBlastingData>();
        public virtual ICollection<PasswordResetCode> PasswordResetCodes { get; set; } = new List<PasswordResetCode>();
        
        // Business logic methods
        public bool IsActive() => Status.Equals("Active", StringComparison.OrdinalIgnoreCase);
        
        public bool HasRole(string roleName)
        {
            return UserRoles.Any(ur => ur.IsActive && 
                                     ur.Role.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }
        
        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
            UpdateTimestamp();
        }
        
        public IEnumerable<string> GetActiveRoles()
        {
            return UserRoles.Where(ur => ur.IsActive)
                           .Select(ur => ur.Role.Name);
        }
    }
} 
