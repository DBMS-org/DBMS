using Application.DTOs.ExplosiveInventory;
using FluentValidation;

namespace Application.Validators.ExplosiveInventory
{
    /// <summary>
    /// Validator for transfer request creation
    /// </summary>
    public class CreateTransferRequestDtoValidator : AbstractValidator<CreateTransferRequestDto>
    {
        public CreateTransferRequestDtoValidator()
        {
            // Central Warehouse Inventory ID
            RuleFor(x => x.CentralWarehouseInventoryId)
                .GreaterThan(0).WithMessage("Central warehouse inventory ID must be valid");

            // Destination Store ID
            RuleFor(x => x.DestinationStoreId)
                .GreaterThan(0).WithMessage("Destination store ID must be valid");

            // Requested Quantity
            RuleFor(x => x.RequestedQuantity)
                .GreaterThan(0).WithMessage("Requested quantity must be greater than zero");

            // Unit
            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage("Unit is required")
                .Must(u => u == "kg" || u == "tons")
                .WithMessage("Unit must be either 'kg' or 'tons'");

            // Required By Date (optional, but if provided must be in future)
            When(x => x.RequiredByDate.HasValue, () =>
            {
                RuleFor(x => x.RequiredByDate!.Value)
                    .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                    .WithMessage("Required by date must be today or in the future");
            });

            // Request Notes (optional)
            When(x => !string.IsNullOrWhiteSpace(x.RequestNotes), () =>
            {
                RuleFor(x => x.RequestNotes)
                    .MaximumLength(1000).WithMessage("Request notes cannot exceed 1000 characters");
            });
        }
    }
}
