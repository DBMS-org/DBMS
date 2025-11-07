namespace Application.DTOs.ProjectManagement
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? AssignedUserId { get; set; }
        public int? RegionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string? AssignedUserName { get; set; }
        public string? RegionName { get; set; }
        public int ProjectSitesCount { get; set; }
        public int MachinesCount { get; set; }
    }
} 