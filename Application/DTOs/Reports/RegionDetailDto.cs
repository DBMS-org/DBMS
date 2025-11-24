namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed region information for reports
    /// </summary>
    public class RegionDetailDto
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public int TotalProjects { get; set; }
        public int ActiveProjects { get; set; }
        public int CompletedProjects { get; set; }
        public int TotalMachines { get; set; }
        public int TotalUsers { get; set; }
        public int TotalStores { get; set; }
        public string? Geography { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
