namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed project site information for reports
    /// </summary>
    public class ProjectSiteDetailDto
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? SiteType { get; set; }
        public string Status { get; set; } = string.Empty; // Planned, Active, Completed, OnHold
        public DateTime? StartDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int BlastingOperationsCount { get; set; }
        public int DrillingOperationsCount { get; set; }
        public List<SiteMachineDto> AssignedMachines { get; set; } = new();
        public List<SiteOperatorDto> AssignedOperators { get; set; } = new();
        public string? ExplosivesUsedSummary { get; set; }
        public string? Notes { get; set; }
    }

    public class SiteMachineDto
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string MachineType { get; set; } = string.Empty;
    }

    public class SiteOperatorDto
    {
        public int OperatorId { get; set; }
        public string OperatorName { get; set; } = string.Empty;
    }
}
