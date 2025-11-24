namespace Application.DTOs.Reports
{
    public class FleetManagementReportDto
    {
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Summary Statistics
        public FleetStatisticsDto Statistics { get; set; } = new();

        // Machine Status Distribution
        public List<StatusDistributionDto> StatusDistribution { get; set; } = new();

        // Machines Due for Service
        public List<MachineDueServiceDto> MachinesDueForService { get; set; } = new();

        // Regional Distribution
        public List<RegionalFleetDto> RegionalDistribution { get; set; } = new();

        // Utilization by Type
        public List<MachineTypeUtilizationDto> UtilizationByType { get; set; } = new();

            // Availability Trend (Last 30 days)
        public List<AvailabilityTrendDto> AvailabilityTrend { get; set; } = new();

        // DETAILED DATA ARRAYS
        // All Machines with complete details
        public List<MachineDetailDto> AllMachines { get; set; } = new();

        // Current Machine Assignments
        public List<MachineAssignmentDetailDto> CurrentAssignments { get; set; } = new();

        // Assignment Request Queue
        public List<AssignmentRequestDetailDto> AssignmentRequests { get; set; } = new();

        // Machine Usage History
        public List<UsageLogDetailDto> RecentUsageLogs { get; set; } = new();

        // Accessories Inventory
        public List<AccessoryDetailDto> AccessoryInventory { get; set; } = new();
    }

    public class FleetStatisticsDto
    {
        public int TotalMachines { get; set; }
        public int AvailableMachines { get; set; }
        public int AssignedMachines { get; set; }
        public int InMaintenance { get; set; }
        public int OutOfService { get; set; }
        public decimal OverallUtilizationRate { get; set; }
        public decimal ServiceComplianceRate { get; set; }
        public int PendingAssignmentRequests { get; set; }
    }

    public class StatusDistributionDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    public class MachineDueServiceDto
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string MachineType { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public decimal CurrentHours { get; set; }
        public decimal ServiceDueHours { get; set; }
        public decimal HoursUntilService { get; set; }
        public DateTime? EstimatedServiceDate { get; set; }
        public string Region { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty; // High, Medium, Low
    }

    public class RegionalFleetDto
    {
        public string Region { get; set; } = string.Empty;
        public int TotalMachines { get; set; }
        public int AvailableMachines { get; set; }
        public int AssignedMachines { get; set; }
        public int InMaintenance { get; set; }
        public decimal UtilizationRate { get; set; }
    }

    public class MachineTypeUtilizationDto
    {
        public string MachineType { get; set; } = string.Empty;
        public int TotalCount { get; set; }
        public int InUseCount { get; set; }
        public decimal UtilizationRate { get; set; }
        public decimal AverageHoursPerMachine { get; set; }
    }

    public class AvailabilityTrendDto
    {
        public DateTime Date { get; set; }
        public int AvailableCount { get; set; }
        public int AssignedCount { get; set; }
        public int MaintenanceCount { get; set; }
        public decimal AvailabilityPercentage { get; set; }
    }
}
