namespace Core.DTOs
{
    public class ProjectSiteDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public CoordinatesDto? Coordinates { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsPatternApproved { get; set; }
        public bool IsSimulationConfirmed { get; set; }
        public bool IsOperatorCompleted { get; set; }
    }
    
    public class CoordinatesDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
} 