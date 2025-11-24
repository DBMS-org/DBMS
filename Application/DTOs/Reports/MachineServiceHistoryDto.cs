namespace Application.DTOs.Reports
{
    /// <summary>
    /// Service history for a specific machine
    /// </summary>
    public class MachineServiceHistoryDto
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string MachineType { get; set; } = string.Empty;
        public DateTime? LastServiceDate { get; set; }
        public DateTime? NextServiceDue { get; set; }
        public int TotalServices { get; set; } // Alias for frontend compatibility
        public int TotalServicesCompleted { get; set; }
        public decimal TotalServiceHours { get; set; } // Alias for frontend compatibility
        public decimal AverageServiceTime { get; set; } // Alias for frontend compatibility
        public int PreventiveMaintenanceCount { get; set; }
        public int CorrectiveMaintenanceCount { get; set; }
        public int EmergencyRepairsCount { get; set; }
        public decimal TotalDowntimeHours { get; set; }
        public decimal AverageRepairTime { get; set; }
        public MaterialsConsumedDto MaterialsConsumed { get; set; } = new();
        public List<RecentServiceDto> RecentServices { get; set; } = new();
    }

    /// <summary>
    /// Materials consumed for a machine
    /// </summary>
    public class MaterialsConsumedDto
    {
        public int TotalDrillBitsUsed { get; set; }
        public int TotalDrillRodsUsed { get; set; }
        public int TotalShanksUsed { get; set; }
    }

    /// <summary>
    /// Recent service record
    /// </summary>
    public class RecentServiceDto
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public DateTime? ServiceDate { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal? CompletionTime { get; set; } // in hours
    }
}
