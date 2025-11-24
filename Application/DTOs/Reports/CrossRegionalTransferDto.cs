namespace Application.DTOs.Reports
{
    /// <summary>
    /// Cross-regional transfer information for reports
    /// </summary>
    public class CrossRegionalTransferDto
    {
        public int TransferId { get; set; }
        public int FromRegionId { get; set; }
        public string FromRegionName { get; set; } = string.Empty;
        public int FromStoreId { get; set; }
        public string FromStoreName { get; set; } = string.Empty;
        public int ToRegionId { get; set; }
        public string ToRegionName { get; set; } = string.Empty;
        public int ToStoreId { get; set; }
        public string ToStoreName { get; set; } = string.Empty;
        public string MaterialType { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal? TransferTime { get; set; } // in hours
        public string RequestedBy { get; set; } = string.Empty;
        public string? ApprovedBy { get; set; }
    }
}
