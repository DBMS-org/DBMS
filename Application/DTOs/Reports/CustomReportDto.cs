namespace Application.DTOs.Reports
{
    public class CustomReportRequestDto
    {
        public string ReportName { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? RegionId { get; set; }
        public string? ProjectId { get; set; }
        public List<string> SelectedMetrics { get; set; } = new();
        public List<string> SelectedChartTypes { get; set; } = new();
        public bool IncludeTrends { get; set; } = true;
        public bool IncludeComparisons { get; set; } = true;
        public string GroupBy { get; set; } = "Region"; // Region, Project, Month, etc.
    }

    public class CustomReportResponseDto
    {
        public string ReportName { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public ReportFilterDto Filters { get; set; } = new();
        public List<MetricDataDto> Metrics { get; set; } = new();
        public List<ChartDataDto> Charts { get; set; } = new();
    }

    public class MetricDataDto
    {
        public string MetricName { get; set; } = string.Empty;
        public string MetricLabel { get; set; } = string.Empty;
        public object Value { get; set; } = null!;
        public string Unit { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public List<MetricBreakdownDto> Breakdown { get; set; } = new();
    }

    public class MetricBreakdownDto
    {
        public string Label { get; set; } = string.Empty;
        public object Value { get; set; } = null!;
        public decimal Percentage { get; set; }
    }

    public class ChartDataDto
    {
        public string ChartId { get; set; } = string.Empty;
        public string ChartType { get; set; } = string.Empty; // Bar, Line, Pie, Gauge, etc.
        public string Title { get; set; } = string.Empty;
        public List<string> Labels { get; set; } = new();
        public List<DatasetDto> Datasets { get; set; } = new();
    }

    public class DatasetDto
    {
        public string Label { get; set; } = string.Empty;
        public List<decimal> Data { get; set; } = new();
        public string Color { get; set; } = string.Empty;
    }

    public class ExportReportRequestDto
    {
        public string ReportType { get; set; } = string.Empty;
        public string Format { get; set; } = "PDF"; // PDF or Excel
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? RegionId { get; set; }
        public bool IncludeCharts { get; set; } = true;
        public bool IncludeSummary { get; set; } = true;
        public bool IncludeDetailedData { get; set; } = true;
    }
}
