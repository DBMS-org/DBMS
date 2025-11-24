namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed resource breakdown for a region
    /// </summary>
    public class RegionalResourceBreakdownDto
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; } = string.Empty;
        public List<RegionMachineDto> Machines { get; set; } = new();
        public List<RegionUserDto> Users { get; set; } = new();
        public List<RegionProjectDto> Projects { get; set; } = new();
        public List<RegionStoreDto> Stores { get; set; } = new();
        public List<RegionMaintenanceJobDto> MaintenanceJobs { get; set; } = new();
        public RegionalPerformanceMetricsDto PerformanceMetrics { get; set; } = new();
    }

    public class RegionMachineDto
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string MachineType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? CurrentAssignment { get; set; }
    }

    public class RegionUserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<string> CurrentAssignments { get; set; } = new();
    }

    public class RegionProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal CompletionPercentage { get; set; }
    }

    public class RegionStoreDto
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public int TotalBatches { get; set; }
        public int LowStockItems { get; set; }
    }

    public class RegionMaintenanceJobDto
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
    }

    public class RegionalPerformanceMetricsDto
    {
        public decimal ProjectSuccessRate { get; set; }
        public decimal MachineUptime { get; set; }
        public decimal MaintenanceEfficiency { get; set; }
        public decimal ResourceUtilization { get; set; }
    }
}
