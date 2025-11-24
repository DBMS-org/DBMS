namespace Application.DTOs.Reports;

public class MaintenancePerformanceReportDto
{
    public DateTime GeneratedAt { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? RegionFilter { get; set; }

    public MaintenanceStatisticsDto Statistics { get; set; } = new();
    public List<MaintenanceTypeBreakdownDto> MaintenanceTypeBreakdown { get; set; } = new();
    public List<MaintenanceTrendDto> MaintenanceTrends { get; set; } = new();
    public List<TopPerformingMechanicDto> TopPerformingMechanics { get; set; } = new();
    public List<MaintenanceIssueDto> CriticalIssues { get; set; } = new();
    public List<RegionalMaintenanceDto> RegionalBreakdown { get; set; } = new();

    // DETAILED DATA ARRAYS
    // All Maintenance Jobs with complete details
    public List<MaintenanceJobDetailDto> AllMaintenanceJobs { get; set; } = new();

    // Mechanic/Engineer Performance Details
    public List<EngineerPerformanceDetailDto> AllEngineers { get; set; } = new();

    // Operator Maintenance Reports
    public List<OperatorMaintenanceReportDetailDto> OperatorReports { get; set; } = new();

    // Service History by Machine
    public List<MachineServiceHistoryDto> MachineServiceHistory { get; set; } = new();

    // Critical & Overdue Issues (already exists but kept for clarity)
    public List<MaintenanceIssueDto> CriticalIssuesDetailed { get; set; } = new();
}

public class MaintenanceStatisticsDto
{
    public int TotalMaintenanceRecords { get; set; }
    public int CompletedMaintenance { get; set; }
    public int InProgressMaintenance { get; set; }
    public int PendingMaintenance { get; set; }
    public decimal AverageCompletionTimeHours { get; set; }
    public decimal MaintenanceCompletionRate { get; set; }
}

public class MaintenanceTypeBreakdownDto
{
    public string MaintenanceType { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal Percentage { get; set; }
    public decimal AverageCompletionTimeHours { get; set; }
}

public class MaintenanceTrendDto
{
    public string Period { get; set; } = string.Empty;
    public int TotalMaintenance { get; set; }
    public int Completed { get; set; }
    public int InProgress { get; set; }
}

public class TopPerformingMechanicDto
{
    public int MechanicId { get; set; }
    public string MechanicName { get; set; } = string.Empty;
    public int TasksCompleted { get; set; }
    public decimal AverageCompletionTimeHours { get; set; }
    public decimal CompletionRate { get; set; }
    public int MachinesServiced { get; set; }
}

public class MaintenanceIssueDto
{
    public int MaintenanceId { get; set; }
    public string MachineIdentifier { get; set; } = string.Empty;
    public string IssueDescription { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime ReportedDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int DaysOpen { get; set; }
}

public class RegionalMaintenanceDto
{
    public string Region { get; set; } = string.Empty;
    public int TotalMaintenance { get; set; }
    public int CompletedMaintenance { get; set; }
    public decimal CompletionRate { get; set; }
}
