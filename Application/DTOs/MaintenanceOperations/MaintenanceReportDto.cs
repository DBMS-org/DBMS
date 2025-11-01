namespace Application.DTOs.MaintenanceOperations
{
    public class MaintenanceReportDto
    {
        public int Id { get; set; }
        public string TicketId { get; set; } = string.Empty;
        public int OperatorId { get; set; }
        public string OperatorName { get; set; } = string.Empty;
        public string? OperatorEmail { get; set; }
        public string? OperatorPhone { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string? MachineModel { get; set; }
        public string? SerialNumber { get; set; }
        public string? Location { get; set; }
        public string? ProjectName { get; set; }
        public int? ProjectId { get; set; }
        public string? RegionName { get; set; }
        public int? RegionId { get; set; }
        public string AffectedPart { get; set; } = string.Empty;
        public string ProblemCategory { get; set; } = string.Empty;
        public string CustomDescription { get; set; } = string.Empty;
        public List<string>? Symptoms { get; set; }
        public string? ErrorCodes { get; set; }
        public string Severity { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime ReportedAt { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        public DateTime? InProgressAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public int? MechanicalEngineerId { get; set; }
        public string? MechanicalEngineerName { get; set; }
        public string? MechanicalEngineerEmail { get; set; }
        public string? MechanicalEngineerPhone { get; set; }
        public string? ResolutionNotes { get; set; }
        public string? EstimatedResponseTime { get; set; }
    }
}
