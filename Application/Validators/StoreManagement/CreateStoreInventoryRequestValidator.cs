using FluentValidation;
using Application.DTOs.StoreManagement;

namespace Application.Validators.StoreManagement
{
    public class CreateStoreInventoryRequestValidator : BaseValidator<CreateStoreInventoryRequest>
    {
        public CreateStoreInventoryRequestValidator()
        {
            RuleFor(x => x.StoreId)
                .PositiveInteger("Store ID");

            RuleFor(x => x.ExplosiveType)
                .IsInEnum()
                .WithMessage("Invalid explosive type");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Quantity must be non-negative");

            RuleFor(x => x.Unit)
                .RequiredString("Unit", 1, 20);

            RuleFor(x => x.MinimumStockLevel)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Minimum stock level must be non-negative");

            RuleFor(x => x.MaximumStockLevel)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Maximum stock level must be non-negative")
                .GreaterThanOrEqualTo(x => x.MinimumStockLevel)
                .WithMessage("Maximum stock level must be greater than or equal to minimum stock level");

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.Now)
                .WithMessage("Expiry date must be in the future")
                .When(x => x.ExpiryDate.HasValue);

            RuleFor(x => x.BatchNumber)
                .MaximumLength(50)
                .WithMessage("Batch number cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.BatchNumber));

            RuleFor(x => x.Supplier)
                .MaximumLength(100)
                .WithMessage("Supplier cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Supplier));
        }
    }
}