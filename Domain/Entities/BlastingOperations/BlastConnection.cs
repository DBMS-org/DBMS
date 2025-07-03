using Domain.Entities.ProjectManagement;

namespace Domain.Entities.BlastingOperations
{
    public class BlastConnection
    {
        public string Id { get; set; } = string.Empty;
        
        public string FromHoleId { get; set; } = string.Empty;
        
        public string ToHoleId { get; set; } = string.Empty;
        
        public ConnectorType ConnectorType { get; set; }
        public int Delay { get; set; } // milliseconds
        public int Sequence { get; set; }
        
        // Hidden starting and ending points for connectors
        public string StartPointJson { get; set; } = string.Empty;
        public string EndPointJson { get; set; } = string.Empty;
        
        // Project and Site context
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
    }
    
    public class DetonatorInfo
    {
        public string Id { get; set; } = string.Empty;
        
        public string HoleId { get; set; } = string.Empty;
        
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
    }
    
    public enum ConnectorType
    {
        DetonatingCord,
        Connectors
    }
    
    public enum DetonatorType
    {
        Electric,
        NonElectric,
        Electronic
    }
} 