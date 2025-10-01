using FluentValidation;
using Application.DTOs.StoreManagement;

namespace Application.Validators.StoreManagement
{
    public class CreateStoreTransactionRequestValidator : BaseValidator<CreateStoreTransactionRequest>
    {
        public CreateStoreTransactionRequestValidator()
        {
            RuleFor(x => x.StoreId)
                .PositiveInteger("Store ID");

            RuleFor(x => x.StoreInventoryId)
                .GreaterThan(0)
                .WithMessage("Store Inventory ID must be greater than 0")
                .When(x => x.StoreInventoryId.HasValue);

            RuleFor(x => x.ExplosiveType)
                .IsInEnum()
                .WithMessage("Invalid explosive type");

            RuleFor(x => x.TransactionType)
                .IsInEnum()
                .WithMessage("Invalid transaction type");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.Unit)
                .RequiredString("Unit", 1, 20);

            RuleFor(x => x.ReferenceNumber)
                .MaximumLength(50)
                .WithMessage("Reference number cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.ReferenceNumber));

            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("Notes cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Notes));

            RuleFor(x => x.RelatedStoreId)
                .GreaterThan(0)
                .WithMessage("Related Store ID must be greater than 0")
                .When(x => x.RelatedStoreId.HasValue);

            RuleFor(x => x.ProcessedByUserId)
                .GreaterThan(0)
                .WithMessage("Processed By User ID must be greater than 0")
                .When(x => x.ProcessedByUserId.HasValue);
        }
    }
}