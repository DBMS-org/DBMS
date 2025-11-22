namespace Application.DTOs.MaintenanceOperations
{
    public class MaintenanceJobDto
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string MachineModel { get; set; } = string.Empty;
        public string? SerialNumber { get; set; }
        public int? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public int? MaintenanceReportId { get; set; }
        public string? ReportTicketId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal EstimatedHours { get; set; }
        public decimal? ActualHours { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Observations { get; set; }
        public List<string>? PartsReplaced { get; set; }
        public List<MaintenanceJobAssignmentDto>? Assignments { get; set; }

        public List<string>? Symptoms { get; set; }
        public string? ErrorCodes { get; set; }
        public string? Severity { get; set; }

        // NEW: Service & Materials
        public bool IsServiceCompleted { get; set; }
        public int? DrillBitsUsed { get; set; }
        public string? DrillBitType { get; set; }
        public int? DrillRodsUsed { get; set; }
        public string? DrillRodType { get; set; }
        public int? ShanksUsed { get; set; }
        public string? ShankType { get; set; }
    }
}
