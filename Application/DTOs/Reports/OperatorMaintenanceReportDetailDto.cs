namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed operator maintenance report information
    /// </summary>
    public class OperatorMaintenanceReportDetailDto
    {
        public int ReportId { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string MachineType { get; set; } = string.Empty;
        public int OperatorId { get; set; }
        public string OperatorName { get; set; } = string.Empty;
        public string ProblemCategory { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty; // Low, Medium, High, Critical
        public string ProblemDescription { get; set; } = string.Empty;
        public string IssueDescription { get; set; } = string.Empty; // Alias for frontend compatibility
        public DateTime ReportedDate { get; set; }
        public DateTime ReportDate { get; set; } // Alias for frontend compatibility
        public string Status { get; set; } = string.Empty; // Pending, UnderReview, Scheduled, Resolved
        public int? AssignedEngineerId { get; set; }
        public string? AssignedEngineerName { get; set; }
        public int? RelatedMaintenanceJobId { get; set; }
        public string? ResolutionNotes { get; set; }
        public DateTime? ResolvedDate { get; set; }
    }
}
