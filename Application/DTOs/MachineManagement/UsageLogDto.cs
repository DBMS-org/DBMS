using System;

namespace Application.DTOs.MachineManagement
{
    public class UsageLogDto
    {
        public int Id { get; set; }

        // Relationships
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public int? OperatorId { get; set; }
        public string SiteEngineer { get; set; } = string.Empty;

        // Data
        public DateTime LogDate { get; set; }
        public decimal EngineHourStart { get; set; }
        public decimal EngineHourEnd { get; set; }
        public decimal EngineHoursDelta { get; set; }
        public decimal? DrifterHourStart { get; set; }
        public decimal? DrifterHourEnd { get; set; }
        public decimal? DrifterHoursDelta { get; set; }
        public decimal IdleHours { get; set; }
        public decimal WorkingHours { get; set; }
        public decimal? FuelConsumed { get; set; }
        public bool HasDowntime { get; set; }
        public decimal? DowntimeHours { get; set; }
        public string? BreakdownDescription { get; set; }
        public string? Remarks { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
