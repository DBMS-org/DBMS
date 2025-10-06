using Domain.Common;
using Domain.Entities.ExplosiveInventory.Enums;

namespace Domain.Entities.ExplosiveInventory
{
    /// <summary>
    /// ANFO (Ammonium Nitrate Fuel Oil) technical specifications
    /// Based on industry standards and Dyno Nobel specifications
    /// </summary>
    public class ANFOTechnicalProperties : BaseEntity
    {
        public int CentralWarehouseInventoryId { get; private set; }

        // Quality Parameters (Required)
        public decimal Density { get; private set; }              // 0.8-0.9 g/cm³
        public decimal FuelOilContent { get; private set; }       // 5.5-6.0 %

        // Quality Parameters (Optional)
        public decimal? MoistureContent { get; private set; }     // < 0.2 %
        public decimal? PrillSize { get; private set; }           // 1-3 mm
        public int? DetonationVelocity { get; private set; }      // 3000-3500 m/s

        // Manufacturing
        public ANFOGrade Grade { get; private set; }

        // Storage Conditions
        public decimal StorageTemperature { get; private set; }   // 5-35°C
        public decimal StorageHumidity { get; private set; }      // < 50% RH

        // Quality Control
        public FumeClass FumeClass { get; private set; }
        public DateTime? QualityCheckDate { get; private set; }
        public QualityStatus QualityStatus { get; private set; }

        // Constants
        public string WaterResistance { get; private set; } = "None"; // Always 'None' for ANFO

        // Additional
        public string? Notes { get; private set; }

        // Navigation
        public virtual CentralWarehouseInventory Inventory { get; private set; } = null!;

        // Private constructor for EF Core
        private ANFOTechnicalProperties() { }

        // Constructor
        public ANFOTechnicalProperties(
            int centralWarehouseInventoryId,
            decimal density,
            decimal fuelOilContent,
            ANFOGrade grade,
            decimal storageTemperature,
            decimal storageHumidity,
            FumeClass fumeClass = FumeClass.Class1,
            QualityStatus qualityStatus = QualityStatus.Pending,
            decimal? moistureContent = null,
            decimal? prillSize = null,
            int? detonationVelocity = null,
            string? notes = null)
        {
            CentralWarehouseInventoryId = centralWarehouseInventoryId;
            Density = density;
            FuelOilContent = fuelOilContent;
            MoistureContent = moistureContent;
            PrillSize = prillSize;
            DetonationVelocity = detonationVelocity;
            Grade = grade;
            StorageTemperature = storageTemperature;
            StorageHumidity = storageHumidity;
            FumeClass = fumeClass;
            QualityStatus = qualityStatus;
            Notes = notes;
            WaterResistance = "None"; // Always None for ANFO

            Validate();
        }

        // Business Methods

        /// <summary>
        /// Update quality parameters
        /// </summary>
        public void UpdateQualityParameters(
            decimal? density = null,
            decimal? fuelOilContent = null,
            decimal? moistureContent = null,
            decimal? prillSize = null,
            int? detonationVelocity = null)
        {
            if (density.HasValue) Density = density.Value;
            if (fuelOilContent.HasValue) FuelOilContent = fuelOilContent.Value;
            if (moistureContent.HasValue) MoistureContent = moistureContent.Value;
            if (prillSize.HasValue) PrillSize = prillSize.Value;
            if (detonationVelocity.HasValue) DetonationVelocity = detonationVelocity.Value;

            Validate();
        }

        /// <summary>
        /// Update storage conditions
        /// </summary>
        public void UpdateStorageConditions(decimal temperature, decimal humidity)
        {
            StorageTemperature = temperature;
            StorageHumidity = humidity;

            Validate();
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

            // Density: 0.8-0.9 g/cm³
            if (Density < 0.8m || Density > 0.9m)
                errors.Add("Density must be between 0.8-0.9 g/cm³");

            // Fuel Oil Content: 5.5-6.0%
            if (FuelOilContent < 5.5m || FuelOilContent > 6.0m)
                errors.Add("Fuel oil content must be between 5.5-6.0%");

            // Moisture Content: < 0.2% (if provided)
            if (MoistureContent.HasValue && MoistureContent.Value > 0.2m)
                errors.Add("Moisture content must be less than 0.2%");

            // Prill Size: 1-3 mm (if provided)
            if (PrillSize.HasValue && (PrillSize.Value < 1m || PrillSize.Value > 3m))
                errors.Add("Prill size must be between 1-3 mm");

            // Detonation Velocity: 3000-3500 m/s (if provided)
            if (DetonationVelocity.HasValue && (DetonationVelocity.Value < 3000 || DetonationVelocity.Value > 3500))
                errors.Add("Detonation velocity must be between 3000-3500 m/s");

            // Storage Temperature: 5-35°C
            if (StorageTemperature < 5m || StorageTemperature > 35m)
                errors.Add("Storage temperature must be between 5-35°C");

            // Storage Humidity: < 50% RH
            if (StorageHumidity > 50m)
                errors.Add("Storage humidity must be less than 50% RH");

            if (errors.Any())
                throw new InvalidOperationException(
                    $"ANFO technical properties validation failed: {string.Join("; ", errors)}");
        }

        /// <summary>
        /// Check if properties indicate safe explosive material
        /// </summary>
        public bool IsSafe()
        {
            return FumeClass == FumeClass.Class1 &&
                   QualityStatus == QualityStatus.Approved &&
                   (!MoistureContent.HasValue || MoistureContent.Value < 0.2m);
        }

        /// <summary>
        /// Check if storage conditions are acceptable
        /// </summary>
        public bool HasAcceptableStorageConditions()
        {
            return StorageTemperature >= 5m && StorageTemperature <= 30m && // Optimal: 5-30°C
                   StorageHumidity <= 40m; // Optimal: < 40% RH
        }
    }
}
