namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed project information for reports
    /// </summary>
    public class ProjectDetailDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty; // Planned, Active, OnHold, Completed, Cancelled
        public DateTime? StartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int? RegionId { get; set; }
        public string? RegionName { get; set; }
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public int TotalSites { get; set; }
        public int CompletedSites { get; set; }
        public List<ProjectMachineAssignmentDto> AssignedMachines { get; set; } = new();
        public List<ProjectUserAssignmentDto> AssignedUsers { get; set; } = new();
        public int? ActiveStoreId { get; set; }
        public string? ActiveStoreName { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal CompletionPercentage { get; set; }
    }

    public class ProjectMachineAssignmentDto
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string MachineType { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
    }

    public class ProjectUserAssignmentDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
    }
}
