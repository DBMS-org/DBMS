namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed inventory transfer request information for reports
    /// </summary>
    public class TransferRequestDetailDto
    {
        public int RequestId { get; set; }
        public int FromStoreId { get; set; }
        public string FromStoreName { get; set; } = string.Empty;
        public int? FromRegionId { get; set; }
        public string? FromRegionName { get; set; }
        public int ToStoreId { get; set; }
        public string ToStoreName { get; set; } = string.Empty;
        public int? ToRegionId { get; set; }
        public string? ToRegionName { get; set; }
        public string MaterialType { get; set; } = string.Empty;
        public decimal RequestedQuantity { get; set; }
        public decimal? ApprovedQuantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public int RequestedById { get; set; }
        public string RequesterName { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DateTime? RequiredByDate { get; set; }
        public string Priority { get; set; } = string.Empty; // Normal, Urgent, Critical
        public string Status { get; set; } = string.Empty; // Pending, Approved, InProgress, Completed, Rejected
        public int? ApprovedById { get; set; }
        public string? ApproverName { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? DispatchDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string? Justification { get; set; }
        public string? RejectionReason { get; set; }
        public string? TransferNotes { get; set; }
        public decimal? FulfillmentTime { get; set; } // in hours, calculated
    }
}
