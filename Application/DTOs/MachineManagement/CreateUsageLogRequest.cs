using System;

namespace Application.DTOs.MachineManagement
{
    public class CreateUsageLogRequest
    {
        public int MachineId { get; set; }
        public DateTime LogDate { get; set; }

        // Site Engineer auto-filled by backend (from logged-in operator)

        // Start/End tracking
        public decimal EngineHourStart { get; set; }
        public decimal EngineHourEnd { get; set; }
        public decimal? DrifterHourStart { get; set; }  // Only for drill rigs
        public decimal? DrifterHourEnd { get; set; }

        // Operating hours
        public decimal IdleHours { get; set; }
        public decimal WorkingHours { get; set; }
        public decimal? FuelConsumed { get; set; }

        // Downtime
        public bool HasDowntime { get; set; }
        public decimal? DowntimeHours { get; set; }
        public string? BreakdownDescription { get; set; }

        // Notes
        public string? Remarks { get; set; }
    }
}
