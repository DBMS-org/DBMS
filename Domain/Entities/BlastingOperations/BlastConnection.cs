using Domain.Entities.ProjectManagement;
using Domain.Entities.DrillingOperations;

namespace Domain.Entities.BlastingOperations
{
    public class BlastConnection
    {
        public string Id { get; set; } = string.Empty;
        
        // Foreign key relationships to two DrillPoints (Point 1 and Point 2)
        public string Point1DrillPointId { get; set; } = string.Empty;
        public string Point2DrillPointId { get; set; } = string.Empty;
        
        // Connection properties
        public ConnectorType ConnectorType { get; set; }
        public int Delay { get; set; } // milliseconds
        public int Sequence { get; set; }
        
        // Project and Site context
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        public virtual DrillPoint Point1DrillPoint { get; set; } = null!;
        public virtual DrillPoint Point2DrillPoint { get; set; } = null!;
    }
    
    public class DetonatorInfo
    {
        public string Id { get; set; } = string.Empty;
        
        // Foreign key relationship to DrillPoint
        public string DrillPointId { get; set; } = string.Empty;
        
        public DetonatorType Type { get; set; }
        public int Delay { get; set; } // milliseconds
        public int Sequence { get; set; }
        
        // Project and Site context
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        public virtual DrillPoint DrillPoint { get; set; } = null!;
    }
    
    public enum ConnectorType
    {
        DetonatingCord = 0,
        Connectors = 1
    }
    
    public enum DetonatorType
    {
        Electric = 0,
        NonElectric = 1,
        Electronic = 2
    }
} 