using Domain.Entities.BlastingOperations;

namespace Application.DTOs.BlastingOperations
{
    public class UpdateBlastConnectionRequest
    {
        public string Id { get; set; } = string.Empty;
        public string Point1DrillPointId { get; set; } = string.Empty;
        public string Point2DrillPointId { get; set; } = string.Empty;
        public ConnectorType ConnectorType { get; set; }
        public int Delay { get; set; }
        public int Sequence { get; set; }
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
    }
} 