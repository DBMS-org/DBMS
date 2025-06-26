namespace Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        // Foreign keys
        public int? AssignedUserId { get; set; }
        public int? RegionId { get; set; }
        
        // Navigation properties
        public virtual User? AssignedUser { get; set; }
        public virtual Region? Region { get; set; }
        public virtual ICollection<ProjectSite> ProjectSites { get; set; } = new List<ProjectSite>();
        public virtual ICollection<Machine> Machines { get; set; } = new List<Machine>();
        public virtual ICollection<DrillPattern> DrillPatterns { get; set; } = new List<DrillPattern>();
        public virtual ICollection<BlastSequence> BlastSequences { get; set; } = new List<BlastSequence>();
        public virtual ICollection<SiteBlastingData> SiteBlastingData { get; set; } = new List<SiteBlastingData>();
        public virtual ICollection<DrillHole> DrillHoles { get; set; } = new List<DrillHole>();
        
        // Business logic methods
        public bool IsActive() => Status.Equals("Active", StringComparison.OrdinalIgnoreCase);
        
        public bool IsCompleted() => Status.Equals("Completed", StringComparison.OrdinalIgnoreCase);
        
        public bool IsInProgress() => Status.Equals("In Progress", StringComparison.OrdinalIgnoreCase);
        
        public bool IsOverdue()
        {
            return EndDate.HasValue && EndDate.Value < DateTime.UtcNow && !IsCompleted();
        }
        
        public void AssignToUser(int userId)
        {
            AssignedUserId = userId;
            UpdateTimestamp();
        }
        
        public void AssignToRegion(int regionId)
        {
            RegionId = regionId;
            UpdateTimestamp();
        }
        
        public void CompleteProject()
        {
            Status = "Completed";
            UpdateTimestamp();
        }
        
        public int GetActiveSitesCount()
        {
            return ProjectSites.Count(ps => ps.Status.Equals("Active", StringComparison.OrdinalIgnoreCase));
        }
    }
} 
