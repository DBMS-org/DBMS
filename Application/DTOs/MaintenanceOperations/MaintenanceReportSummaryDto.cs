namespace Application.DTOs.MaintenanceOperations
{
    public class MaintenanceReportSummaryDto
    {
        public int TotalReports { get; set; }
        public int PendingReports { get; set; }
        public int InProgressReports { get; set; }
        public int ResolvedReports { get; set; }
        public int ClosedReports { get; set; }
        public int CriticalReports { get; set; }
        public int HighPriorityReports { get; set; }
    }
}
