namespace Application.DTOs.ExplosiveInventory
{
    public class UpdateANFOInventoryRequest
    {
        public decimal? Quantity { get; set; }

        // Storage
        public string? StorageLocation { get; set; }

        public decimal? Density { get; set; }
        public decimal? FuelOilContent { get; set; }
        public decimal? MoistureContent { get; set; }
        public decimal? PrillSize { get; set; }
        public int? DetonationVelocity { get; set; }

        // Storage Conditions
        public decimal? StorageTemperature { get; set; }
        public decimal? StorageHumidity { get; set; }

        public string? Notes { get; set; }
    }
}
