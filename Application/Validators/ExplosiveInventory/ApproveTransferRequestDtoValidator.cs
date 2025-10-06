using Application.DTOs.ExplosiveInventory;
using FluentValidation;

namespace Application.Validators.ExplosiveInventory
{
    /// <summary>
    /// Validator for transfer request approval
    /// </summary>
    public class ApproveTransferRequestDtoValidator : AbstractValidator<ApproveTransferRequestDto>
    {
        public ApproveTransferRequestDtoValidator()
        {
            // Approved Quantity (optional, but if provided must be positive)
            When(x => x.ApprovedQuantity.HasValue, () =>
            {
                RuleFor(x => x.ApprovedQuantity!.Value)
                    .GreaterThan(0)
                    .WithMessage("Approved quantity must be greater than zero");
            });

            // Approval Notes (optional)
            When(x => !string.IsNullOrWhiteSpace(x.ApprovalNotes), () =>
            {
                RuleFor(x => x.ApprovalNotes)
                    .MaximumLength(1000).WithMessage("Approval notes cannot exceed 1000 characters");
            });
        }
    }
}
