using Application.DTOs.ExplosiveInventory;
using FluentValidation;

namespace Application.Validators.ExplosiveInventory
{
    /// <summary>
    /// Validator for ANFO inventory creation requests
    /// </summary>
    public class CreateANFOInventoryRequestValidator : AbstractValidator<CreateANFOInventoryRequest>
    {
        public CreateANFOInventoryRequestValidator()
        {
            // Batch ID
            RuleFor(x => x.BatchId)
                .NotEmpty().WithMessage("Batch ID is required")
                .Matches(@"^ANFO-\d{4}-\d{3}$")
                .WithMessage("Batch ID must follow format ANFO-YYYY-XXX (e.g., ANFO-2025-001)");

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

            // ===== ANFO TECHNICAL SPECIFICATIONS =====

            // Density: 0.8-0.9 g/cm³
            RuleFor(x => x.Density)
                .InclusiveBetween(0.8m, 0.9m)
                .WithMessage("Density must be between 0.8-0.9 g/cm³");

            // Fuel Oil Content: 5.5-6.0%
            RuleFor(x => x.FuelOilContent)
                .InclusiveBetween(5.5m, 6.0m)
                .WithMessage("Fuel oil content must be between 5.5-6.0%");

            // Moisture Content: < 0.2% (optional)
            When(x => x.MoistureContent.HasValue, () =>
            {
                RuleFor(x => x.MoistureContent!.Value)
                    .LessThanOrEqualTo(0.2m)
                    .WithMessage("Moisture content must be less than 0.2%");
            });

            // Prill Size: 1-3 mm (optional)
            When(x => x.PrillSize.HasValue, () =>
            {
                RuleFor(x => x.PrillSize!.Value)
                    .InclusiveBetween(1m, 3m)
                    .WithMessage("Prill size must be between 1-3 mm");
            });

            // Detonation Velocity: 3000-3500 m/s (optional)
            When(x => x.DetonationVelocity.HasValue, () =>
            {
                RuleFor(x => x.DetonationVelocity!.Value)
                    .InclusiveBetween(3000, 3500)
                    .WithMessage("Detonation velocity must be between 3000-3500 m/s");
            });

            // Storage Temperature: 5-35°C
            RuleFor(x => x.StorageTemperature)
                .InclusiveBetween(5m, 35m)
                .WithMessage("Storage temperature must be between 5-35°C");

            // Storage Humidity: < 50% RH
            RuleFor(x => x.StorageHumidity)
                .GreaterThanOrEqualTo(0m)
                .LessThanOrEqualTo(50m)
                .WithMessage("Storage humidity must be between 0-50%");

            // Grade is required (enum validation handled by type)
            RuleFor(x => x.Grade)
                .IsInEnum().WithMessage("Invalid ANFO grade");

            // Fume Class is required (enum validation)
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
