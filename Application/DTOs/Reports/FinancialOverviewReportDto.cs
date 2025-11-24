namespace Application.DTOs.Reports
{
    public class FinancialOverviewReportDto
    {
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Summary Statistics
        public FinancialSummaryDto Summary { get; set; } = new();

        // Machine Utilization Costs
        public List<MachineUtilizationCostDto> MachineUtilizationCosts { get; set; } = new();

        // Maintenance Costs by Type
        public List<MaintenanceCostDto> MaintenanceCosts { get; set; } = new();

        // Inventory Value by Category
        public List<InventoryValueDto> InventoryValues { get; set; } = new();

        // Cost per Project/Region
        public List<ProjectCostDto> ProjectCosts { get; set; } = new();
        public List<RegionalCostDto> RegionalCosts { get; set; } = new();

        // Trend Analysis
        public List<CostTrendDto> CostTrends { get; set; } = new();

        // Cost Efficiency Metrics
        public CostEfficiencyDto CostEfficiency { get; set; } = new();
    }

    public class FinancialSummaryDto
    {
        public decimal TotalOperatingCost { get; set; }
        public decimal MachineCosts { get; set; }
        public decimal MaintenanceCosts { get; set; }
        public decimal InventoryCosts { get; set; }
        public decimal LaborCosts { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal NetProfit { get; set; }
        public decimal ProfitMargin { get; set; }
        public decimal ROI { get; set; } // Return on Investment
    }

    public class MachineUtilizationCostDto
    {
        public string MachineName { get; set; } = string.Empty;
        public string MachineType { get; set; } = string.Empty;
        public decimal HoursOperated { get; set; }
        public decimal CostPerHour { get; set; }
        public decimal TotalCost { get; set; }
        public decimal UtilizationRate { get; set; }
        public decimal CostEfficiency { get; set; }
        public string Region { get; set; } = string.Empty;
    }

    public class MaintenanceCostDto
    {
        public string MaintenanceType { get; set; } = string.Empty;
        public int JobCount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AverageCostPerJob { get; set; }
        public decimal LaborCost { get; set; }
        public decimal PartsCost { get; set; }
        public decimal DowntimeCost { get; set; }
    }

    public class InventoryValueDto
    {
        public string Category { get; set; } = string.Empty;
        public decimal TotalValue { get; set; }
        public int ItemCount { get; set; }
        public decimal AverageValuePerItem { get; set; }
        public decimal TurnoverRate { get; set; }
        public decimal CarryingCost { get; set; }
    }

    public class ProjectCostDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public decimal TotalCost { get; set; }
        public decimal MachineCosts { get; set; }
        public decimal MaterialCosts { get; set; }
        public decimal LaborCosts { get; set; }
        public decimal MaintenanceCosts { get; set; }
        public decimal BudgetedCost { get; set; }
        public decimal CostVariance { get; set; }
        public decimal CostEfficiency { get; set; }
    }

    public class RegionalCostDto
    {
        public string Region { get; set; } = string.Empty;
        public decimal TotalCost { get; set; }
        public decimal MachineCosts { get; set; }
        public decimal MaintenanceCosts { get; set; }
        public decimal InventoryCosts { get; set; }
        public decimal LaborCosts { get; set; }
        public int ProjectCount { get; set; }
        public decimal CostPerProject { get; set; }
        public decimal Efficiency { get; set; }
    }

    public class CostTrendDto
    {
        public DateTime Month { get; set; }
        public decimal TotalCost { get; set; }
        public decimal MachineCosts { get; set; }
        public decimal MaintenanceCosts { get; set; }
        public decimal InventoryCosts { get; set; }
        public decimal LaborCosts { get; set; }
        public decimal Revenue { get; set; }
        public decimal Profit { get; set; }
    }

    public class CostEfficiencyDto
    {
        public decimal MachineUtilizationEfficiency { get; set; }
        public decimal MaintenanceEfficiency { get; set; }
        public decimal InventoryEfficiency { get; set; }
        public decimal LaborProductivity { get; set; }
        public decimal OverallOperationalEfficiency { get; set; }
        public List<EfficiencyBenchmarkDto> Benchmarks { get; set; } = new();
    }

    public class EfficiencyBenchmarkDto
    {
        public string Metric { get; set; } = string.Empty;
        public decimal CurrentValue { get; set; }
        public decimal BenchmarkValue { get; set; }
        public decimal Variance { get; set; }
        public string Status { get; set; } = string.Empty; // Above, At, Below
    }
}
