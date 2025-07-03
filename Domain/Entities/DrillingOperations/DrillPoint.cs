using Domain.Entities.ProjectManagement;

namespace Domain.Entities.DrillingOperations
{
    public class DrillPoint
    {
        public string Id { get; set; } = string.Empty;
        
        public double X { get; set; }
        public double Y { get; set; }
        public double Depth { get; set; }
        public double Spacing { get; set; }
        public double Burden { get; set; }
        
        // Project and Site context
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
    }
    
    public class PatternSettings
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Default";
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        public double Spacing { get; set; } = 3.0;
        public double Burden { get; set; } = 2.5;
        public double Depth { get; set; } = 10.0;
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
    }
} 