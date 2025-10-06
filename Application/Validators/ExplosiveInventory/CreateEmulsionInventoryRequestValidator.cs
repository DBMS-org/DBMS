using Application.DTOs.ExplosiveInventory;
using FluentValidation;

namespace Application.Validators.ExplosiveInventory
{
    /// <summary>
    /// Validator for Emulsion inventory creation requests
    /// </summary>
    public class CreateEmulsionInventoryRequestValidator : AbstractValidator<CreateEmulsionInventoryRequest>
    {
        public CreateEmulsionInventoryRequestValidator()
        {
            // Batch ID
            RuleFor(x => x.BatchId)
                .NotEmpty().WithMessage("Batch ID is required")
                .Matches(@"^EMU-\d{4}-\d{3}$")
                .WithMessage("Batch ID must follow format EMU-YYYY-XXX (e.g., EMU-2025-001)");

            // Quantity
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero");

            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage("Unit is required")
                .Must(u => u == "kg" || u == "tons")
                .WithMessage("Unit must be either 'kg' or 'tons'");

            // Dates
            RuleFor(x => x.ManufacturingDate)
                .NotEmpty().WithMessage("Manufacturing date is required")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Manufacturing date cannot be in the future");

            RuleFor(x => x.ExpiryDate)
                .NotEmpty().WithMessage("Expiry date is required")
                .GreaterThan(x => x.ManufacturingDate)
                .WithMessage("Expiry date must be after manufacturing date");

            // Supplier
            RuleFor(x => x.Supplier)
                .NotEmpty().WithMessage("Supplier is required")
                .MaximumLength(200).WithMessage("Supplier name cannot exceed 200 characters");

            // Storage
            RuleFor(x => x.StorageLocation)
                .NotEmpty().WithMessage("Storage location is required")
                .MaximumLength(100).WithMessage("Storage location cannot exceed 100 characters");

            RuleFor(x => x.CentralWarehouseStoreId)
                .GreaterThan(0).WithMessage("Central warehouse store ID must be valid");

            // ===== EMULSION TECHNICAL SPECIFICATIONS =====

            // Density Unsensitized: 1.30-1.45 g/cm³
            RuleFor(x => x.DensityUnsensitized)
                .InclusiveBetween(1.30m, 1.45m)
                .WithMessage("Density (unsensitized) must be between 1.30-1.45 g/cm³");

            // Density Sensitized: 1.10-1.30 g/cm³
            RuleFor(x => x.DensitySensitized)
                .InclusiveBetween(1.10m, 1.30m)
                .WithMessage("Density (sensitized) must be between 1.10-1.30 g/cm³");

            // Viscosity: 50,000-200,000 cP
            RuleFor(x => x.Viscosity)
                .InclusiveBetween(50000, 200000)
                .WithMessage("Viscosity must be between 50,000-200,000 cP");

            // Water Content: 12-16%
            RuleFor(x => x.WaterContent)
                .InclusiveBetween(12m, 16m)
                .WithMessage("Water content must be between 12-16%");

            // pH: 4.5-6.5
            RuleFor(x => x.pH)
                .InclusiveBetween(4.5m, 6.5m)
                .WithMessage("pH must be between 4.5-6.5");

            // Storage Temperature: -20 to 50°C
            RuleFor(x => x.StorageTemperature)
                .InclusiveBetween(-20m, 50m)
                .WithMessage("Storage temperature must be between -20 to 50°C");

            // Grade is required
            RuleFor(x => x.Grade)
                .IsInEnum().WithMessage("Invalid emulsion grade");

            // Color is required
            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("Color is required")
                .MaximumLength(50).WithMessage("Color cannot exceed 50 characters");

            // Sensitization Type is required
            RuleFor(x => x.SensitizationType)
                .IsInEnum().WithMessage("Invalid sensitization type");

            // Detonation Velocity: 4500-6000 m/s (optional)
            When(x => x.DetonationVelocity.HasValue, () =>
            {
                RuleFor(x => x.DetonationVelocity!.Value)
                    .InclusiveBetween(4500, 6000)
                    .WithMessage("Detonation velocity must be between 4500-6000 m/s");
            });

            // Bubble Size: 10-100 μm (optional)
            When(x => x.BubbleSize.HasValue, () =>
            {
                RuleFor(x => x.BubbleSize!.Value)
                    .InclusiveBetween(10, 100)
                    .WithMessage("Bubble size must be between 10-100 μm");
            });

            // Application Temperature: 0-45°C (optional)
            When(x => x.ApplicationTemperature.HasValue, () =>
            {
                RuleFor(x => x.ApplicationTemperature!.Value)
                    .InclusiveBetween(0m, 45m)
                    .WithMessage("Application temperature must be between 0-45°C");
            });

            // Sensitizer Content: 0-100% (optional)
            When(x => x.SensitizerContent.HasValue, () =>
            {
                RuleFor(x => x.SensitizerContent!.Value)
                    .InclusiveBetween(0m, 100m)
                    .WithMessage("Sensitizer content must be between 0-100%");
            });

            // Fume Class is required
            RuleFor(x => x.FumeClass)
                .IsInEnum().WithMessage("Invalid fume class");

            // Quality Status
            RuleFor(x => x.QualityStatus)
                .IsInEnum().WithMessage("Invalid quality status");

            // Notes (optional)
            When(x => !string.IsNullOrWhiteSpace(x.Notes), () =>
            {
                RuleFor(x => x.Notes)
                    .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters");
            });
        }
    }
}
