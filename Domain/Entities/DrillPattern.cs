namespace Domain.Entities
{
    public class DrillPattern : BaseEntity
    {
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Pattern settings - stored separately for easy querying and business logic
        // These represent the default/template values for the pattern
        public double Spacing { get; set; } = 3.0;
        public double Burden { get; set; } = 2.5;
        public double Depth { get; set; } = 10.0;
        
        // Drill points as JSON
        public string DrillPointsJson { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        public int CreatedByUserId { get; set; }
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
        public virtual ICollection<BlastSequence> BlastSequences { get; set; } = new List<BlastSequence>();
        
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
        
        public void UpdatePattern(double spacing, double burden, double depth, string drillPointsJson)
        {
            Spacing = spacing;
            Burden = burden;
            Depth = depth;
            DrillPointsJson = drillPointsJson;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public bool HasDrillPoints()
        {
            return !string.IsNullOrEmpty(DrillPointsJson) && DrillPointsJson != "[]";
        }
    }
} 
