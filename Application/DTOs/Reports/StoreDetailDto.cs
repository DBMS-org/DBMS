namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed store information for reports
    /// </summary>
    public class StoreDetailDto
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public int? RegionId { get; set; }
        public string? RegionName { get; set; }
        public int? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public int? StoreManagerId { get; set; }
        public string? ManagerName { get; set; }
        public int TotalBatches { get; set; }
        public int LowStockItems { get; set; }
        public int ExpiredItems { get; set; }
        public int QuarantinedItems { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ContactInfo { get; set; }
    }
}
