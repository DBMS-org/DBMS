namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed machine assignment information for reports
    /// </summary>
    public class MachineAssignmentDetailDto
    {
        public int AssignmentId { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string MachineType { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectStatus { get; set; } = string.Empty;
        public int? OperatorId { get; set; }
        public string? OperatorName { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int? RegionId { get; set; }
        public string? RegionName { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
