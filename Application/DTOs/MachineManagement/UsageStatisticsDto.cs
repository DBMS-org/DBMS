using System;

namespace Application.DTOs.MachineManagement
{
    public class UsageStatisticsDto
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public decimal TotalEngineHours { get; set; }
        public decimal TotalIdleHours { get; set; }
        public decimal TotalWorkingHours { get; set; }
        public decimal TotalFuelConsumed { get; set; }
        public decimal TotalDowntimeHours { get; set; }
        public decimal AverageDailyHours { get; set; }
        public int DaysWithDowntime { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
