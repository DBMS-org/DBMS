namespace Application.DTOs.MaintenanceOperations
{
    public class MaintenanceStatsDto
    {
        public int TotalJobs { get; set; }
        public int ScheduledJobs { get; set; }
        public int InProgressJobs { get; set; }
        public int CompletedJobs { get; set; }
        public int OverdueJobs { get; set; }
        public int EmergencyJobs { get; set; }
        public decimal AverageCompletionHours { get; set; }
        public int ServiceDueCount { get; set; }
    }
}
