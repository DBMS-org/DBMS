namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// Request DTO for updating Emulsion inventory batch
    /// </summary>
    public class UpdateEmulsionInventoryRequest
    {
        // Quantity (optional - for adjustments)
        public decimal? Quantity { get; set; }

        // Storage
        public string? StorageLocation { get; set; }

        // Density (optional updates)
        public decimal? DensityUnsensitized { get; set; }
        public decimal? DensitySensitized { get; set; }

        // Rheological
        public int? Viscosity { get; set; }
        public decimal? WaterContent { get; set; }
        public decimal? pH { get; set; }

        // Performance
        public int? DetonationVelocity { get; set; }
        public int? BubbleSize { get; set; }

        // Temperature
        public decimal? StorageTemperature { get; set; }
        public decimal? ApplicationTemperature { get; set; }

        // Additional
        public string? Notes { get; set; }
    }
}
