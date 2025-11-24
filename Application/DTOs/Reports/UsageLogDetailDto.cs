namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed machine usage log information for reports
    /// </summary>
    public class UsageLogDetailDto
    {
        public int LogId { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string MachineType { get; set; } = string.Empty;
        public int OperatorId { get; set; }
        public string OperatorName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal EngineHoursRecorded { get; set; }
        public decimal DrifterHoursRecorded { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public int? RegionId { get; set; }
        public string? RegionName { get; set; }
    }
}
