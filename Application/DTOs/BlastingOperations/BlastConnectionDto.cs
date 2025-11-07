using Domain.Entities.BlastingOperations;

namespace Application.DTOs.BlastingOperations
{
    public class BlastConnectionDto
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public DrillPointDto? Point1DrillPoint { get; set; }
        public DrillPointDto? Point2DrillPoint { get; set; }
    }
    
    public class DrillPointDto
    {
        public string Id { get; set; } = string.Empty;
        public double X { get; set; }
        public double Y { get; set; }
        public double Depth { get; set; }
        public double Spacing { get; set; }
        public double Burden { get; set; }
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
    }
}