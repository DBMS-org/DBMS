using Domain.Entities.ProjectManagement;
using Domain.Entities.DrillingOperations;

namespace Domain.Entities.BlastingOperations
{
    public class BlastConnection
    {
        public string Id { get; set; } = string.Empty;

        public string Point1DrillPointId { get; set; } = string.Empty;
        public string Point2DrillPointId { get; set; } = string.Empty;

        public ConnectorType ConnectorType { get; set; }
        public int Delay { get; set; }
        public int Sequence { get; set; }

        public bool IsStartingHole { get; set; } = false;

        public int ProjectId { get; set; }
        public int SiteId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        public virtual DrillPoint Point1DrillPoint { get; set; } = null!;
        public virtual DrillPoint Point2DrillPoint { get; set; } = null!;
    }
    
    public class DetonatorInfo
    {
        public string Id { get; set; } = string.Empty;

        public string DrillPointId { get; set; } = string.Empty;

        public DetonatorType Type { get; set; }
        public int Delay { get; set; }
        public int Sequence { get; set; }

        public int ProjectId { get; set; }
        public int SiteId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

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