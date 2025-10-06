using Domain.Entities.ExplosiveInventory.Enums;

namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// DTO for Emulsion technical specifications
    /// </summary>
    public class EmulsionTechnicalPropertiesDto
    {
        public int Id { get; set; }

        // Density
        public decimal DensityUnsensitized { get; set; }
        public decimal DensitySensitized { get; set; }

        // Rheological
        public int Viscosity { get; set; }

        // Composition
        public decimal WaterContent { get; set; }
        public decimal pH { get; set; }

        // Performance
        public int? DetonationVelocity { get; set; }
        public int? BubbleSize { get; set; }

        // Temperature
        public decimal StorageTemperature { get; set; }
        public decimal? ApplicationTemperature { get; set; }

        // Manufacturing
        public EmulsionGrade Grade { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        // Sensitization
        public SensitizationType SensitizationType { get; set; }
        public string SensitizationTypeName { get; set; } = string.Empty;
        public decimal? SensitizerContent { get; set; }

        // Quality Control
        public FumeClass FumeClass { get; set; }
        public string FumeClassName { get; set; } = string.Empty;
        public DateTime? QualityCheckDate { get; set; }
        public QualityStatus QualityStatus { get; set; }
        public string QualityStatusName { get; set; } = string.Empty;
        public bool? PhaseSeparation { get; set; }
        public bool? Crystallization { get; set; }
        public bool? ColorConsistency { get; set; }

        // Constants
        public string WaterResistance { get; set; } = "Excellent";

        // Additional
        public string? Notes { get; set; }

        // Computed
        public bool IsSafe { get; set; }
        public bool HasOptimalStorageConditions { get; set; }
        public bool IsStable { get; set; }
        public bool IsPumpable { get; set; }
    }
}
