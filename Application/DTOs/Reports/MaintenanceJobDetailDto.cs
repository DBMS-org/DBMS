namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed maintenance job information for reports
    /// </summary>
    public class MaintenanceJobDetailDto
    {
        public int MaintenanceJobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string MachineType { get; set; } = string.Empty;
        public string MaintenanceType { get; set; } = string.Empty; // Preventive, Corrective, Emergency
        public string Priority { get; set; } = string.Empty; // Low, Medium, High, Critical
        public string Status { get; set; } = string.Empty; // Scheduled, InProgress, Completed, Cancelled, Overdue
        public string? ProblemDescription { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal? EstimatedHours { get; set; }
        public decimal? ActualHours { get; set; }
        public bool IsEngineServiceCompleted { get; set; }
        public bool IsDrifterServiceCompleted { get; set; }
        public int DrillBitsUsed { get; set; }
        public int DrillRodsUsed { get; set; }
        public int ShanksUsed { get; set; }
        public string? CompletionNotes { get; set; }
        public List<EngineerAssignmentDto> AssignedEngineers { get; set; } = new();
        public int? RegionId { get; set; }
        public string? RegionName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Engineer assignment within a maintenance job
    /// </summary>
    public class EngineerAssignmentDto
    {
        public int EngineerId { get; set; }
        public string EngineerName { get; set; } = string.Empty;
    }
}
