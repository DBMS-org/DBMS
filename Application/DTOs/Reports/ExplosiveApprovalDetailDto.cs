namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed explosive approval request information for reports
    /// </summary>
    public class ExplosiveApprovalDetailDto
    {
        public int RequestId { get; set; }
        public int ProjectSiteId { get; set; }
        public string SiteName { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public int BlastingEngineerId { get; set; }
        public string EngineerName { get; set; } = string.Empty;
        public string ExplosiveType { get; set; } = string.Empty;
        public decimal RequestedQuantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string? Purpose { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string Status { get; set; } = string.Empty; // Pending, Approved, Rejected, Issued
        public int? ApprovedById { get; set; }
        public string? ApproverName { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public decimal? IssuedQuantity { get; set; }
        public DateTime? IssueDate { get; set; }
        public int? StoreId { get; set; }
        public string? StoreName { get; set; }
        public string? Notes { get; set; }
    }
}
