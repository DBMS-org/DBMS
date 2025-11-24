namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed inventory batch information for reports
    /// </summary>
    public class InventoryBatchDetailDto
    {
        public int InventoryId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public string MaterialType { get; set; } = string.Empty; // ANFO, Emulsion, Pentolite, etc.
        public string BatchNumber { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal ReservedQuantity { get; set; }
        public decimal AvailableQuantity { get; set; } // calculated: Quantity - ReservedQuantity
        public string Unit { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
        public int? DaysUntilExpiry { get; set; } // calculated
        public string Status { get; set; } = string.Empty; // Available, Reserved, Quarantined, Expired
        public decimal MinimumStock { get; set; }
        public decimal MaximumStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierContact { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? RegionId { get; set; }
        public string? RegionName { get; set; }
        public string? Notes { get; set; }
    }
}
