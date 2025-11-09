namespace Application.DTOs.ExplosiveInventory
{
    public class UpdateEmulsionInventoryRequest
    {
        public decimal? Quantity { get; set; }

        // Storage
        public string? StorageLocation { get; set; }

        public decimal? DensityUnsensitized { get; set; }
        public decimal? DensitySensitized { get; set; }

        public int? Viscosity { get; set; }
        public decimal? WaterContent { get; set; }
        public decimal? pH { get; set; }

        public int? DetonationVelocity { get; set; }
        public int? BubbleSize { get; set; }

        // Temperature
        public decimal? StorageTemperature { get; set; }
        public decimal? ApplicationTemperature { get; set; }

        public string? Notes { get; set; }
    }
}
