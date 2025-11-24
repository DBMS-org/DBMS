namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed machine information for reports
    /// </summary>
    public class MachineDetailDto
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string MachineType { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Available, Assigned, InMaintenance, OutOfService
        public decimal CurrentEngineServiceHours { get; set; }
        public decimal EngineServiceInterval { get; set; }
        public decimal NextEngineServiceDue { get; set; }
        public decimal CurrentDrifterServiceHours { get; set; }
        public decimal DrifterServiceInterval { get; set; }
        public decimal NextDrifterServiceDue { get; set; }
        public DateTime? DateAcquired { get; set; }
        public DateTime? LastServiceDate { get; set; }
        public int? AssignedProjectId { get; set; }
        public string? AssignedProjectName { get; set; }
        public int? AssignedOperatorId { get; set; }
        public string? AssignedOperatorName { get; set; }
        public int? RegionId { get; set; }
        public string? RegionName { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
