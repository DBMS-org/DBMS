using Domain.Common;
using Domain.Entities.ExplosiveInventory.Enums;

namespace Domain.Entities.ExplosiveInventory
{
    /// <summary>
    /// Emulsion explosive technical specifications
    /// Based on industry standards for water gel and emulsion explosives
    /// </summary>
    public class EmulsionTechnicalProperties : BaseEntity
    {
        public int CentralWarehouseInventoryId { get; private set; }

        // Density (Required)
        public decimal DensityUnsensitized { get; private set; }  // 1.30-1.45 g/cm³
        public decimal DensitySensitized { get; private set; }    // 1.10-1.30 g/cm³

        // Rheological (Required)
        public int Viscosity { get; private set; }                // 50,000-200,000 cP

        // Composition (Required)
        public decimal WaterContent { get; private set; }         // 12-16 %
        public decimal pH { get; private set; }                   // 4.5-6.5

        // Performance (Optional)
        public int? DetonationVelocity { get; private set; }      // 4500-6000 m/s
        public int? BubbleSize { get; private set; }              // 10-100 μm

        // Temperature
        public decimal StorageTemperature { get; private set; }   // -20 to 50°C
        public decimal? ApplicationTemperature { get; private set; } // 0-45°C

        // Manufacturing
        public EmulsionGrade Grade { get; private set; }
        public string Color { get; private set; } = string.Empty; // White, Pink, Blue, Red

        // Sensitization
        public SensitizationType SensitizationType { get; private set; }
        public decimal? SensitizerContent { get; private set; }   // % by weight

        // Quality Control
        public FumeClass FumeClass { get; private set; }
        public DateTime? QualityCheckDate { get; private set; }
        public QualityStatus QualityStatus { get; private set; }
        public bool? PhaseSeparation { get; private set; }
        public bool? Crystallization { get; private set; }
        public bool? ColorConsistency { get; private set; }

        // Constants
        public string WaterResistance { get; private set; } = "Excellent"; // Always Excellent for Emulsion

        // Additional
        public string? Notes { get; private set; }

        // Navigation
        public virtual CentralWarehouseInventory Inventory { get; private set; } = null!;

        // Private constructor for EF Core
        private EmulsionTechnicalProperties() { }

        // Constructor
        public EmulsionTechnicalProperties(
            int centralWarehouseInventoryId,
            decimal densityUnsensitized,
            decimal densitySensitized,
            int viscosity,
            decimal waterContent,
            decimal pH,
            decimal storageTemperature,
            EmulsionGrade grade,
            string color,
            SensitizationType sensitizationType,
            FumeClass fumeClass = FumeClass.Class1,
            QualityStatus qualityStatus = QualityStatus.Pending,
            int? detonationVelocity = null,
            int? bubbleSize = null,
            decimal? applicationTemperature = null,
            decimal? sensitizerContent = null,
            string? notes = null)
        {
            CentralWarehouseInventoryId = centralWarehouseInventoryId;
            DensityUnsensitized = densityUnsensitized;
            DensitySensitized = densitySensitized;
            Viscosity = viscosity;
            WaterContent = waterContent;
            this.pH = pH;
            DetonationVelocity = detonationVelocity;
            BubbleSize = bubbleSize;
            StorageTemperature = storageTemperature;
            ApplicationTemperature = applicationTemperature;
            Grade = grade;
            Color = color;
            SensitizationType = sensitizationType;
            SensitizerContent = sensitizerContent;
            FumeClass = fumeClass;
            QualityStatus = qualityStatus;
            Notes = notes;
            WaterResistance = "Excellent"; // Always Excellent for Emulsion

            Validate();
        }

        // Business Methods

        /// <summary>
        /// Update density specifications
        /// </summary>
        public void UpdateDensity(decimal densityUnsensitized, decimal densitySensitized)
        {
            DensityUnsensitized = densityUnsensitized;
            DensitySensitized = densitySensitized;

            Validate();
        }

        /// <summary>
        /// Update rheological properties
        /// </summary>
        public void UpdateRheology(int viscosity, decimal waterContent, decimal pH)
        {
            Viscosity = viscosity;
            WaterContent = waterContent;
            this.pH = pH;

            Validate();
        }

        /// <summary>
        /// Update storage conditions
        /// </summary>
        public void UpdateStorageConditions(decimal storageTemperature, decimal? applicationTemperature = null)
        {
            StorageTemperature = storageTemperature;
            ApplicationTemperature = applicationTemperature;

            Validate();
        }

        /// <summary>
        /// Update performance characteristics
        /// </summary>
        public void UpdatePerformance(int? detonationVelocity = null, int? bubbleSize = null)
        {
            if (detonationVelocity.HasValue) DetonationVelocity = detonationVelocity.Value;
            if (bubbleSize.HasValue) BubbleSize = bubbleSize.Value;

            Validate();
        }

        /// <summary>
        /// Perform stability check
        /// </summary>
        public void PerformStabilityCheck(bool phaseSeparation, bool crystallization, bool colorConsistency)
        {
            PhaseSeparation = phaseSeparation;
            Crystallization = crystallization;
            ColorConsistency = colorConsistency;
            QualityCheckDate = DateTime.UtcNow;

            // Auto-reject if stability issues detected
            if (phaseSeparation || crystallization || !colorConsistency)
            {
                QualityStatus = QualityStatus.Rejected;
                AddNotes($"Stability issues detected: Phase Separation={phaseSeparation}, " +
                        $"Crystallization={crystallization}, Color Consistency={colorConsistency}");
            }
        }

        /// <summary>
        /// Perform quality check
        /// </summary>
        public void PerformQualityCheck(QualityStatus status, FumeClass? fumeClass = null)
        {
            QualityStatus = status;
            QualityCheckDate = DateTime.UtcNow;

            if (fumeClass.HasValue)
                FumeClass = fumeClass.Value;
        }

        /// <summary>
        /// Approve quality
        /// </summary>
        public void Approve()
        {
            // Check stability before approval
            if (PhaseSeparation == true || Crystallization == true || ColorConsistency == false)
                throw new InvalidOperationException("Cannot approve emulsion with stability issues");

            QualityStatus = QualityStatus.Approved;
            QualityCheckDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Reject quality
        /// </summary>
        public void Reject(string reason)
        {
            QualityStatus = QualityStatus.Rejected;
            QualityCheckDate = DateTime.UtcNow;
            Notes = string.IsNullOrWhiteSpace(Notes) ? reason : $"{Notes}\nRejection: {reason}";
        }

        /// <summary>
        /// Add notes
        /// </summary>
        public void AddNotes(string additionalNotes)
        {
            if (string.IsNullOrWhiteSpace(additionalNotes))
                return;

            Notes = string.IsNullOrWhiteSpace(Notes)
                ? additionalNotes
                : $"{Notes}\n{additionalNotes}";
        }

        /// <summary>
        /// Validate all technical specifications are within acceptable ranges
        /// </summary>
        public void Validate()
        {
            var errors = new List<string>();

            // Density Unsensitized: 1.30-1.45 g/cm³
            if (DensityUnsensitized < 1.30m || DensityUnsensitized > 1.45m)
                errors.Add("Density (unsensitized) must be between 1.30-1.45 g/cm³");

            // Density Sensitized: 1.10-1.30 g/cm³
            if (DensitySensitized < 1.10m || DensitySensitized > 1.30m)
                errors.Add("Density (sensitized) must be between 1.10-1.30 g/cm³");

            // Viscosity: 50,000-200,000 cP
            if (Viscosity < 50000 || Viscosity > 200000)
                errors.Add("Viscosity must be between 50,000-200,000 cP");

            // Water Content: 12-16%
            if (WaterContent < 12m || WaterContent > 16m)
                errors.Add("Water content must be between 12-16%");

            // pH: 4.5-6.5
            if (pH < 4.5m || pH > 6.5m)
                errors.Add("pH must be between 4.5-6.5");

            // Detonation Velocity: 4500-6000 m/s (if provided)
            if (DetonationVelocity.HasValue && (DetonationVelocity.Value < 4500 || DetonationVelocity.Value > 6000))
                errors.Add("Detonation velocity must be between 4500-6000 m/s");

            // Bubble Size: 10-100 μm (if provided)
            if (BubbleSize.HasValue && (BubbleSize.Value < 10 || BubbleSize.Value > 100))
                errors.Add("Bubble size must be between 10-100 μm");

            // Storage Temperature: -20 to 50°C
            if (StorageTemperature < -20m || StorageTemperature > 50m)
                errors.Add("Storage temperature must be between -20 to 50°C");

            // Application Temperature: 0-45°C (if provided)
            if (ApplicationTemperature.HasValue && (ApplicationTemperature.Value < 0m || ApplicationTemperature.Value > 45m))
                errors.Add("Application temperature must be between 0-45°C");

            // Sensitizer Content: 0-100% (if provided)
            if (SensitizerContent.HasValue && (SensitizerContent.Value < 0m || SensitizerContent.Value > 100m))
                errors.Add("Sensitizer content must be between 0-100%");

            // Color validation
            if (string.IsNullOrWhiteSpace(Color))
                errors.Add("Color is required");

            if (errors.Any())
                throw new InvalidOperationException(
                    $"Emulsion technical properties validation failed: {string.Join("; ", errors)}");
        }

        /// <summary>
        /// Check if properties indicate safe explosive material
        /// </summary>
        public bool IsSafe()
        {
            return FumeClass == FumeClass.Class1 &&
                   QualityStatus == QualityStatus.Approved &&
                   PhaseSeparation != true &&
                   Crystallization != true &&
                   ColorConsistency != false;
        }

        /// <summary>
        /// Check if storage conditions are optimal
        /// </summary>
        public bool HasOptimalStorageConditions()
        {
            return StorageTemperature >= 15m && StorageTemperature <= 30m; // Optimal: 15-30°C
        }

        /// <summary>
        /// Check if emulsion is stable
        /// </summary>
        public bool IsStable()
        {
            return PhaseSeparation != true &&
                   Crystallization != true &&
                   ColorConsistency != false &&
                   Viscosity <= 150000; // Pumpable range
        }

        /// <summary>
        /// Check if viscosity is in pumpable range
        /// </summary>
        public bool IsPumpable()
        {
            return Viscosity < 100000; // < 100,000 cP is ideal for pumping
        }
    }
}
