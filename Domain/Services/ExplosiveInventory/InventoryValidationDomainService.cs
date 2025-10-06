using Domain.Entities.ExplosiveInventory;
using Domain.Entities.StoreManagement.Enums;

namespace Domain.Services.ExplosiveInventory
{
    /// <summary>
    /// Domain service for validating explosive inventory technical specifications
    /// against industry standards
    /// </summary>
    public class InventoryValidationDomainService
    {
        /// <summary>
        /// Validate ANFO technical properties against industry standards
        /// </summary>
        public ValidationResult ValidateANFOProperties(ANFOTechnicalProperties properties)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            var errors = new List<string>();
            var warnings = new List<string>();

            // Critical validations (errors)

            // Density: 0.8-0.9 g/cm³
            if (properties.Density < 0.8m || properties.Density > 0.9m)
                errors.Add($"Density {properties.Density} g/cm³ is out of acceptable range (0.8-0.9 g/cm³)");

            // Fuel Oil Content: 5.5-6.0%
            if (properties.FuelOilContent < 5.5m || properties.FuelOilContent > 6.0m)
                errors.Add($"Fuel oil content {properties.FuelOilContent}% is out of acceptable range (5.5-6.0%)");

            // Storage Temperature: 5-35°C
            if (properties.StorageTemperature < 5m || properties.StorageTemperature > 35m)
                errors.Add($"Storage temperature {properties.StorageTemperature}°C is out of acceptable range (5-35°C)");

            // Storage Humidity: < 50% RH
            if (properties.StorageHumidity > 50m)
                errors.Add($"Storage humidity {properties.StorageHumidity}% exceeds maximum (50%)");

            // Optional parameter validations (errors if provided but out of range)

            if (properties.MoistureContent.HasValue && properties.MoistureContent.Value > 0.2m)
                errors.Add($"Moisture content {properties.MoistureContent.Value}% exceeds maximum (0.2%)");

            if (properties.PrillSize.HasValue && (properties.PrillSize.Value < 1m || properties.PrillSize.Value > 3m))
                errors.Add($"Prill size {properties.PrillSize.Value} mm is out of acceptable range (1-3 mm)");

            if (properties.DetonationVelocity.HasValue &&
                (properties.DetonationVelocity.Value < 3000 || properties.DetonationVelocity.Value > 3500))
                errors.Add($"Detonation velocity {properties.DetonationVelocity.Value} m/s is out of acceptable range (3000-3500 m/s)");

            // Warning validations (acceptable but not optimal)

            // Optimal storage temperature: 15-25°C
            if (properties.StorageTemperature < 15m || properties.StorageTemperature > 25m)
                warnings.Add($"Storage temperature {properties.StorageTemperature}°C is outside optimal range (15-25°C)");

            // Optimal humidity: < 40%
            if (properties.StorageHumidity > 40m)
                warnings.Add($"Storage humidity {properties.StorageHumidity}% is above optimal level (< 40%)");

            // Optimal fuel oil content: 5.7-5.9%
            if (properties.FuelOilContent < 5.7m || properties.FuelOilContent > 5.9m)
                warnings.Add($"Fuel oil content {properties.FuelOilContent}% is outside optimal range (5.7-5.9%)");

            // Moisture warning
            if (properties.MoistureContent.HasValue && properties.MoistureContent.Value > 0.1m)
                warnings.Add($"Moisture content {properties.MoistureContent.Value}% is elevated (optimal: < 0.1%)");

            return new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors,
                Warnings = warnings
            };
        }

        /// <summary>
        /// Validate Emulsion technical properties against industry standards
        /// </summary>
        public ValidationResult ValidateEmulsionProperties(EmulsionTechnicalProperties properties)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            var errors = new List<string>();
            var warnings = new List<string>();

            // Critical validations (errors)

            // Density Unsensitized: 1.30-1.45 g/cm³
            if (properties.DensityUnsensitized < 1.30m || properties.DensityUnsensitized > 1.45m)
                errors.Add($"Density (unsensitized) {properties.DensityUnsensitized} g/cm³ is out of acceptable range (1.30-1.45 g/cm³)");

            // Density Sensitized: 1.10-1.30 g/cm³
            if (properties.DensitySensitized < 1.10m || properties.DensitySensitized > 1.30m)
                errors.Add($"Density (sensitized) {properties.DensitySensitized} g/cm³ is out of acceptable range (1.10-1.30 g/cm³)");

            // Viscosity: 50,000-200,000 cP
            if (properties.Viscosity < 50000 || properties.Viscosity > 200000)
                errors.Add($"Viscosity {properties.Viscosity} cP is out of acceptable range (50,000-200,000 cP)");

            // Water Content: 12-16%
            if (properties.WaterContent < 12m || properties.WaterContent > 16m)
                errors.Add($"Water content {properties.WaterContent}% is out of acceptable range (12-16%)");

            // pH: 4.5-6.5
            if (properties.pH < 4.5m || properties.pH > 6.5m)
                errors.Add($"pH {properties.pH} is out of acceptable range (4.5-6.5)");

            // Storage Temperature: -20 to 50°C
            if (properties.StorageTemperature < -20m || properties.StorageTemperature > 50m)
                errors.Add($"Storage temperature {properties.StorageTemperature}°C is out of acceptable range (-20 to 50°C)");

            // Optional parameter validations

            if (properties.DetonationVelocity.HasValue &&
                (properties.DetonationVelocity.Value < 4500 || properties.DetonationVelocity.Value > 6000))
                errors.Add($"Detonation velocity {properties.DetonationVelocity.Value} m/s is out of acceptable range (4500-6000 m/s)");

            if (properties.BubbleSize.HasValue &&
                (properties.BubbleSize.Value < 10 || properties.BubbleSize.Value > 100))
                errors.Add($"Bubble size {properties.BubbleSize.Value} μm is out of acceptable range (10-100 μm)");

            if (properties.ApplicationTemperature.HasValue &&
                (properties.ApplicationTemperature.Value < 0m || properties.ApplicationTemperature.Value > 45m))
                errors.Add($"Application temperature {properties.ApplicationTemperature.Value}°C is out of acceptable range (0-45°C)");

            // Warning validations

            // Optimal storage temperature: 15-30°C
            if (properties.StorageTemperature < 15m || properties.StorageTemperature > 30m)
                warnings.Add($"Storage temperature {properties.StorageTemperature}°C is outside optimal range (15-30°C)");

            // Temperature warning for degradation risk
            if (properties.StorageTemperature > 40m)
                warnings.Add($"Storage temperature {properties.StorageTemperature}°C approaching degradation risk threshold (> 60°C)");

            // Optimal water content: 14-15%
            if (properties.WaterContent < 14m || properties.WaterContent > 15m)
                warnings.Add($"Water content {properties.WaterContent}% is outside optimal range (14-15%)");

            // Optimal pH: 5.0-6.0
            if (properties.pH < 5.0m || properties.pH > 6.0m)
                warnings.Add($"pH {properties.pH} is outside optimal range (5.0-6.0)");

            // Viscosity pumpability warning
            if (properties.Viscosity > 100000)
                warnings.Add($"Viscosity {properties.Viscosity} cP may affect pumpability (optimal: < 100,000 cP)");

            // High viscosity critical
            if (properties.Viscosity > 150000)
                warnings.Add($"Viscosity {properties.Viscosity} cP is high - monitor closely");

            // Stability checks
            if (properties.PhaseSeparation == true)
                errors.Add("Phase separation detected - material is unstable");

            if (properties.Crystallization == true)
                errors.Add("Crystallization detected - material is unstable");

            if (properties.ColorConsistency == false)
                warnings.Add("Color inconsistency detected - may indicate quality issues");

            return new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors,
                Warnings = warnings
            };
        }

        /// <summary>
        /// Validate that inventory quantity is sufficient for transfer
        /// </summary>
        public ValidationResult ValidateTransferQuantity(
            CentralWarehouseInventory inventory,
            decimal requestedQuantity)
        {
            if (inventory == null)
                throw new ArgumentNullException(nameof(inventory));

            var errors = new List<string>();
            var warnings = new List<string>();

            // Check if inventory is available
            if (inventory.Status != Entities.ExplosiveInventory.Enums.InventoryStatus.Available)
                errors.Add($"Inventory batch {inventory.BatchId} is not available (status: {inventory.Status})");

            // Check if expired
            if (inventory.IsExpired)
                errors.Add($"Inventory batch {inventory.BatchId} is expired");

            // Check available quantity
            if (requestedQuantity > inventory.AvailableQuantity)
                errors.Add($"Requested quantity {requestedQuantity} {inventory.Unit} exceeds available quantity {inventory.AvailableQuantity} {inventory.Unit}");

            // Warning for expiring soon
            if (inventory.IsExpiringSoon)
                warnings.Add($"Inventory batch {inventory.BatchId} is expiring in {inventory.DaysUntilExpiry} days");

            // Warning for nearly depleting inventory
            if (requestedQuantity >= inventory.AvailableQuantity * 0.9m)
                warnings.Add($"Transfer will consume {(requestedQuantity / inventory.AvailableQuantity * 100):F1}% of available inventory");

            return new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors,
                Warnings = warnings
            };
        }

        /// <summary>
        /// Validate batch ID format
        /// </summary>
        public ValidationResult ValidateBatchId(string batchId, ExplosiveType explosiveType)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(batchId))
            {
                errors.Add("Batch ID is required");
                return new ValidationResult { IsValid = false, Errors = errors, Warnings = new List<string>() };
            }

            // Validate format based on explosive type
            var expectedPrefix = explosiveType switch
            {
                ExplosiveType.ANFO => "ANFO",
                ExplosiveType.Emulsion => "EMU",
                _ => null
            };

            if (expectedPrefix != null)
            {
                if (!batchId.StartsWith($"{expectedPrefix}-"))
                    errors.Add($"Batch ID must start with '{expectedPrefix}-' for {explosiveType} type");

                // Format: PREFIX-YYYY-XXX
                var parts = batchId.Split('-');
                if (parts.Length != 3)
                    errors.Add($"Batch ID must follow format '{expectedPrefix}-YYYY-XXX'");
                else
                {
                    if (!int.TryParse(parts[1], out int year) || year < 2000 || year > 2100)
                        errors.Add("Invalid year in batch ID");

                    if (!int.TryParse(parts[2], out int sequence) || parts[2].Length != 3)
                        errors.Add("Sequence number must be 3 digits");
                }
            }

            return new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors,
                Warnings = new List<string>()
            };
        }
    }

    /// <summary>
    /// Validation result with errors and warnings
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();

        public bool HasWarnings => Warnings.Any();
        public string ErrorMessage => string.Join("; ", Errors);
        public string WarningMessage => string.Join("; ", Warnings);

        public string GetAllMessages()
        {
            var messages = new List<string>();
            if (Errors.Any())
                messages.Add($"Errors: {ErrorMessage}");
            if (Warnings.Any())
                messages.Add($"Warnings: {WarningMessage}");

            return string.Join("\n", messages);
        }
    }
}
