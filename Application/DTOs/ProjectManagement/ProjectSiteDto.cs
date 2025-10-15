namespace Application.DTOs.ProjectManagement
{
    public class ProjectSiteDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Coordinates { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsPatternApproved { get; set; }
        public bool IsSimulationConfirmed { get; set; }
        public bool IsOperatorCompleted { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? CompletedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties (optional, for detailed views)
        public string? ProjectName { get; set; }
        public string? ProjectRegion { get; set; }
        public string? CompletedByUserName { get; set; }
        public ProjectDto? Project { get; set; }
    }
}