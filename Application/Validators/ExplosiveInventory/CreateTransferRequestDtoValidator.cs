using Application.DTOs.ExplosiveInventory;
using FluentValidation;

namespace Application.Validators.ExplosiveInventory
{
    public class CreateTransferRequestDtoValidator : AbstractValidator<CreateTransferRequestDto>
    {
        public CreateTransferRequestDtoValidator()
        {
            // Central Warehouse Inventory ID
            RuleFor(x => x.CentralWarehouseInventoryId)
                .GreaterThan(0).WithMessage("Please select a valid inventory item from the central warehouse.");

            // Destination Store ID
            RuleFor(x => x.DestinationStoreId)
                .GreaterThan(0).WithMessage("Please select a valid destination store for this transfer.");

            // Requested Quantity
            RuleFor(x => x.RequestedQuantity)
                .GreaterThan(0).WithMessage("Requested quantity must be greater than zero. Please enter a valid amount.");

            // Unit
            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage("Unit is required. Please specify the unit of measurement.")
                .Must(u => u == "kg" || u == "tons")
                .WithMessage("Unit must be either 'kg' or 'tons'. Please select a valid unit.");

            When(x => x.RequiredByDate.HasValue, () =>
            {
                RuleFor(x => x.RequiredByDate!.Value)
                    .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                    .WithMessage("Required by date must be today or in the future. Please select a valid date.");
            });

            // Request Notes (optional)
            When(x => !string.IsNullOrWhiteSpace(x.RequestNotes), () =>
            {
                RuleFor(x => x.RequestNotes)
                    .MaximumLength(1000).WithMessage("Request notes cannot exceed 1000 characters.");
            });
        }
    }
}
