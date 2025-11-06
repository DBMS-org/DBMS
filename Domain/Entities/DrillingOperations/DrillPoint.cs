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
        public double Diameter { get; set; } = 0;
        public double Stemming { get; set; } = 0;
        public double Subdrill { get; set; } = 0;
        public double Volume { get; set; } = 0;
        public double ANFO { get; set; } = 0;
        public double Emulsion { get; set; } = 0;

        public int ProjectId { get; set; }
        public int SiteId { get; set; }

        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }
        public int? CompletedByUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
    }
    
    public class PatternSettings
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Default";
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        public double Spacing { get; set; } = 0;
        public double Burden { get; set; } = 0;
        public double Depth { get; set; } = 0;
        public double Diameter { get; set; } = 0;
        public double Stemming { get; set; } = 0;

        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
    }
}