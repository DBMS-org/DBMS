namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed store transaction information for reports
    /// </summary>
    public class StoreTransactionDetailDto
    {
        public int TransactionId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty; // Receipt, Issue, Transfer, Return, Adjustment, Disposal
        public string MaterialType { get; set; } = string.Empty;
        public string? BatchNumber { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public int PerformedById { get; set; }
        public string PerformedByName { get; set; } = string.Empty;
        public int? RelatedProjectId { get; set; }
        public string? RelatedProjectName { get; set; }
        public int? RelatedTransferRequestId { get; set; }
        public string? Reference { get; set; }
        public string? Notes { get; set; }
        public decimal? BalanceAfter { get; set; }
    }
}
