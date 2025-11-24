namespace Application.DTOs.Reports
{
    /// <summary>
    /// Comprehensive drilling operations report
    /// </summary>
    public class DrillingOperationsReportDto
    {
        public DateTime GeneratedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? RegionFilter { get; set; }

        // Summary Statistics
        public DrillingStatisticsDto Statistics { get; set; } = new();

        // Drilling by Project Site
        public List<ProjectSiteDrillingDto> DrillingByProjectSite { get; set; } = new();

        // Drilling by Region
        public List<RegionalDrillingDto> DrillingByRegion { get; set; } = new();

        // Blast Connections Summary
        public List<BlastConnectionSummaryDto> BlastConnections { get; set; } = new();

        // Explosive Calculation Results
        public List<ExplosiveCalculationSummaryDto> ExplosiveCalculations { get; set; } = new();

        // DETAILED DATA
        public List<DrillHoleDetailDto> AllDrillHoles { get; set; } = new();
        public List<DrillPointDetailDto> AllDrillPoints { get; set; } = new();
    }

    public class DrillingStatisticsDto
    {
        public int TotalDrillHoles { get; set; }
        public int TotalDrillPoints { get; set; }
        public int TotalBlastConnections { get; set; }
        public int TotalProjectSites { get; set; }
        public decimal AverageDrillDepth { get; set; }
        public decimal TotalDrillingMeters { get; set; }
        public int ActivePatterns { get; set; }
        public decimal AverageDiameter { get; set; }
        public int ActiveProjectSites { get; set; }
        public int TotalExplosiveCalculations { get; set; }
    }

    public class ProjectSiteDrillingDto
    {
        public int ProjectSiteId { get; set; }
        public string ProjectSiteName { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public int DrillHoleCount { get; set; }
        public int DrillPointCount { get; set; }
        public decimal TotalDepth { get; set; }
        public decimal AverageDepth { get; set; }
        public int BlastConnectionCount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public decimal CompletionPercentage { get; set; }
    }

    public class RegionalDrillingDto
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; } = string.Empty;
        public int TotalDrillHoles { get; set; }
        public int TotalDrillPoints { get; set; }
        public int ProjectSiteCount { get; set; }
        public decimal TotalDepth { get; set; }
        public decimal AverageDepth { get; set; }
        public int BlastConnectionCount { get; set; }
        public decimal UtilizationPercentage { get; set; }
    }

    public class BlastConnectionSummaryDto
    {
        public string ConnectionId { get; set; } = string.Empty;
        public string ConnectorType { get; set; } = string.Empty;
        public string Point1DrillPointId { get; set; } = string.Empty;
        public string Point2DrillPointId { get; set; } = string.Empty;
        public int Delay { get; set; }
        public int Sequence { get; set; }
        public string ProjectSiteName { get; set; } = string.Empty;
    }

    public class ExplosiveCalculationSummaryDto
    {
        public string CalculationId { get; set; } = string.Empty;
        public string ProjectSiteName { get; set; } = string.Empty;
        public decimal TotalAnfo { get; set; }
        public decimal TotalEmulsion { get; set; }
        public decimal TotalExplosive { get; set; }
        public int NumberOfFilledHoles { get; set; }
        public DateTime CalculationDate { get; set; }
    }

    public class DrillHoleDetailDto
    {
        public string HoleId { get; set; } = string.Empty;
        public string HoleName { get; set; } = string.Empty;
        public decimal Depth { get; set; }
        public decimal Easting { get; set; }
        public decimal Northing { get; set; }
        public decimal Elevation { get; set; }
        public decimal? Azimuth { get; set; }
        public decimal? Dip { get; set; }
        public string ProjectSiteName { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public int? BlastConnectionCount { get; set; }
    }

    public class DrillPointDetailDto
    {
        public string PointId { get; set; } = string.Empty;
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Depth { get; set; }
        public decimal Diameter { get; set; }
        public int Sequence { get; set; }
        public string ProjectSiteName { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
