using Domain.Entities.ExplosiveInventory.Enums;

namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// DTO for ANFO technical specifications
    /// </summary>
    public class ANFOTechnicalPropertiesDto
    {
        public int Id { get; set; }

        // Quality Parameters
        public decimal Density { get; set; }
        public decimal FuelOilContent { get; set; }
        public decimal? MoistureContent { get; set; }
        public decimal? PrillSize { get; set; }
        public int? DetonationVelocity { get; set; }

        // Manufacturing
        public ANFOGrade Grade { get; set; }
        public string GradeName { get; set; } = string.Empty;

        // Storage Conditions
        public decimal StorageTemperature { get; set; }
        public decimal StorageHumidity { get; set; }

        // Quality Control
        public FumeClass FumeClass { get; set; }
        public string FumeClassName { get; set; } = string.Empty;
        public DateTime? QualityCheckDate { get; set; }
        public QualityStatus QualityStatus { get; set; }
        public string QualityStatusName { get; set; } = string.Empty;

        // Constants
        public string WaterResistance { get; set; } = "None";

        // Additional
        public string? Notes { get; set; }

        // Computed
        public bool IsSafe { get; set; }
        public bool HasAcceptableStorageConditions { get; set; }
    }
}
