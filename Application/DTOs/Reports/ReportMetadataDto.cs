namespace Application.DTOs.Reports
{
    public class ReportMetadataDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime? LastGenerated { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class ReportFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? RegionId { get; set; }
        public string? ProjectId { get; set; }
        public List<string>? Metrics { get; set; }
    }
}
