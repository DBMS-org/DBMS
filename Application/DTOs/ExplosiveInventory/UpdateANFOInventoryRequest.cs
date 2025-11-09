namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// Request DTO for updating ANFO inventory batch
    /// </summary>
    public class UpdateANFOInventoryRequest
    {
        // Quantity (optional - for adjustments)
        public decimal? Quantity { get; set; }

        // Storage
        public string? StorageLocation { get; set; }

        // Quality Parameters (optional updates)
        public decimal? Density { get; set; }
        public decimal? FuelOilContent { get; set; }
        public decimal? MoistureContent { get; set; }
        public decimal? PrillSize { get; set; }
        public int? DetonationVelocity { get; set; }

        // Storage Conditions
        public decimal? StorageTemperature { get; set; }
        public decimal? StorageHumidity { get; set; }

        // Additional
        public string? Notes { get; set; }
    }
}
