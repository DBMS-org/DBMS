using Application.DTOs.ExplosiveInventory;
using FluentValidation;

namespace Application.Validators.ExplosiveInventory
{
    public class CreateANFOInventoryRequestValidator : AbstractValidator<CreateANFOInventoryRequest>
    {
        public CreateANFOInventoryRequestValidator()
        {
            // Batch ID
            RuleFor(x => x.BatchId)
                .NotEmpty().WithMessage("Batch ID is required. Please provide a batch identifier.")
                .Matches(@"^ANFO-\d{4}-\d{3}$")
                .WithMessage("Batch ID must follow the format ANFO-YYYY-XXX (e.g., ANFO-2025-001).");

            // Quantity
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero. Please enter a valid amount.");

            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage("Unit is required. Please specify the unit of measurement.")
                .Must(u => u == "kg" || u == "tons")
                .WithMessage("Unit must be either 'kg' or 'tons'. Please select a valid unit.");

            // Dates
            RuleFor(x => x.ManufacturingDate)
                .NotEmpty().WithMessage("Manufacturing date is required. Please select the production date.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Manufacturing date cannot be in the future. Please select a valid date.");

            RuleFor(x => x.ExpiryDate)
                .NotEmpty().WithMessage("Expiry date is required. Please select the expiration date.")
                .GreaterThan(x => x.ManufacturingDate)
                .WithMessage("Expiry date must be after the manufacturing date. Please check your dates.");

            // Supplier
            RuleFor(x => x.Supplier)
                .NotEmpty().WithMessage("Supplier is required. Please enter the supplier name.")
                .MaximumLength(200).WithMessage("Supplier name cannot exceed 200 characters.");

            // Storage
            RuleFor(x => x.StorageLocation)
                .NotEmpty().WithMessage("Storage location is required. Please specify where this will be stored.")
                .MaximumLength(100).WithMessage("Storage location cannot exceed 100 characters.");

            RuleFor(x => x.CentralWarehouseStoreId)
                .GreaterThan(0).WithMessage("Please select a valid central warehouse store.");

            // Density: 0.8-0.9 g/cm³
            RuleFor(x => x.Density)
                .InclusiveBetween(0.8m, 0.9m)
                .WithMessage("Density must be between 0.8 and 0.9 g/cm³ for ANFO specifications.");

            // Fuel Oil Content: 5.5-6.0%
            RuleFor(x => x.FuelOilContent)
                .InclusiveBetween(5.5m, 6.0m)
                .WithMessage("Fuel oil content must be between 5.5% and 6.0% for proper ANFO composition.");

            // Moisture Content: < 0.2% (optional)
            When(x => x.MoistureContent.HasValue, () =>
            {
                RuleFor(x => x.MoistureContent!.Value)
                    .LessThanOrEqualTo(0.2m)
                    .WithMessage("Moisture content must be less than 0.2% to meet quality standards.");
            });

            // Prill Size: 1-3 mm (optional)
            When(x => x.PrillSize.HasValue, () =>
            {
                RuleFor(x => x.PrillSize!.Value)
                    .InclusiveBetween(1m, 3m)
                    .WithMessage("Prill size must be between 1 and 3 mm.");
            });

            // Detonation Velocity: 3000-3500 m/s (optional)
            When(x => x.DetonationVelocity.HasValue, () =>
            {
                RuleFor(x => x.DetonationVelocity!.Value)
                    .InclusiveBetween(3000, 3500)
                    .WithMessage("Detonation velocity must be between 3000 and 3500 m/s.");
            });

            // Storage Temperature: 5-35°C
            RuleFor(x => x.StorageTemperature)
                .InclusiveBetween(5m, 35m)
                .WithMessage("Storage temperature must be between 5°C and 35°C for safe storage.");

            // Storage Humidity: < 50% RH
            RuleFor(x => x.StorageHumidity)
                .GreaterThanOrEqualTo(0m)
                .LessThanOrEqualTo(50m)
                .WithMessage("Storage humidity must be between 0% and 50% relative humidity.");

            // Grade is required (enum validation handled by type)
            RuleFor(x => x.Grade)
                .IsInEnum().WithMessage("Please select a valid ANFO grade.");

            // Fume Class is required (enum validation)
            RuleFor(x => x.FumeClass)
                .IsInEnum().WithMessage("Please select a valid fume class.");

            // Quality Status
            RuleFor(x => x.QualityStatus)
                .IsInEnum().WithMessage("Please select a valid quality status.");

            // Notes (optional)
            When(x => !string.IsNullOrWhiteSpace(x.Notes), () =>
            {
                RuleFor(x => x.Notes)
                    .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters.");
            });
        }
    }
}
