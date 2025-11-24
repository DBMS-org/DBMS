namespace Application.DTOs.Reports
{
    public class RegionalPerformanceReportDto
    {
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Regional Comparison Summary
        public List<RegionalComparisonDto> RegionalComparison { get; set; } = new();

        // Machine Distribution by Region
        public List<RegionalMachineDistributionDto> MachineDistribution { get; set; } = new();

        // Maintenance Statistics by Region
        public List<RegionalMaintenanceStatsDto> MaintenanceStats { get; set; } = new();

        // Service Alert Trends by Region
        public List<RegionalServiceAlertDto> ServiceAlertTrends { get; set; } = new();

        // Regional KPI Comparison
        public List<RegionalKPIDto> RegionalKPIs { get; set; } = new();

        // Top Performing Regions
        public List<TopRegionDto> TopPerformingRegions { get; set; } = new();

        // Regional Resource Allocation
        public List<RegionalResourceDto> ResourceAllocation { get; set; } = new();

        // DETAILED DATA ARRAYS
        // All Regions
        public List<RegionDetailDto> AllRegions { get; set; } = new();

        // Regional Resource Breakdown
        public List<RegionalResourceBreakdownDto> RegionalResources { get; set; } = new();

        // Cross-Regional Transfers
        public List<CrossRegionalTransferDto> CrossRegionalTransfers { get; set; } = new();
    }

    public class RegionalComparisonDto
    {
        public string Region { get; set; } = string.Empty;
        public int TotalProjects { get; set; }
        public int ActiveProjects { get; set; }
        public int TotalMachines { get; set; }
        public int AssignedMachines { get; set; }
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public decimal MachineUtilization { get; set; }
        public decimal ProjectCompletionRate { get; set; }
        public decimal OverallEfficiency { get; set; }
    }

    public class RegionalMachineDistributionDto
    {
        public string Region { get; set; } = string.Empty;
        public List<MachineTypeCountDto> MachinesByType { get; set; } = new();
        public int TotalMachines { get; set; }
        public int AvailableMachines { get; set; }
        public int InMaintenanceMachines { get; set; }
        public decimal MachineToProjectRatio { get; set; }
    }

    public class MachineTypeCountDto
    {
        public string MachineType { get; set; } = string.Empty;
        public int Count { get; set; }
        public int Available { get; set; }
        public int InUse { get; set; }
    }

    public class RegionalMaintenanceStatsDto
    {
        public string Region { get; set; } = string.Empty;
        public int TotalMaintenanceJobs { get; set; }
        public int CompletedJobs { get; set; }
        public int OverdueJobs { get; set; }
        public decimal CompletionRate { get; set; }
        public decimal AverageResponseTime { get; set; } // in hours
        public decimal AverageCompletionTime { get; set; } // in hours
        public int ActiveEngineers { get; set; }
        public decimal JobsPerEngineer { get; set; }
    }

    public class RegionalServiceAlertDto
    {
        public string Region { get; set; } = string.Empty;
        public DateTime Month { get; set; }
        public int TotalAlerts { get; set; }
        public int ResolvedAlerts { get; set; }
        public int PendingAlerts { get; set; }
        public decimal ResolutionRate { get; set; }
        public decimal AverageResolutionTime { get; set; } // in days
    }

    public class RegionalKPIDto
    {
        public string Region { get; set; } = string.Empty;
        public decimal OperationalUptime { get; set; }
        public decimal MachineAvailability { get; set; }
        public decimal MaintenanceCompliance { get; set; }
        public decimal SafetyScore { get; set; }
        public decimal ResourceUtilization { get; set; }
        public decimal CustomerSatisfaction { get; set; }
        public decimal OverallScore { get; set; }
    }

    public class TopRegionDto
    {
        public int Rank { get; set; }
        public string Region { get; set; } = string.Empty;
        public decimal OverallScore { get; set; }
        public List<string> Strengths { get; set; } = new();
        public List<string> ImprovementAreas { get; set; } = new();
        public string TopPerformanceMetric { get; set; } = string.Empty;
    }

    public class RegionalResourceDto
    {
        public string Region { get; set; } = string.Empty;
        public int Machines { get; set; }
        public int Operators { get; set; }
        public int Engineers { get; set; }
        public int Managers { get; set; }
        public int Projects { get; set; }
        public decimal ResourceDensity { get; set; }
        public string AllocationStatus { get; set; } = string.Empty; // Optimal, Under-allocated, Over-allocated
    }
}
