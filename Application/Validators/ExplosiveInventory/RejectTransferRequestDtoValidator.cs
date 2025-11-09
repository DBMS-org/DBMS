using Application.DTOs.ExplosiveInventory;
using FluentValidation;

namespace Application.Validators.ExplosiveInventory
{
    /// <summary>
    /// Validator for transfer request rejection
    /// </summary>
    public class RejectTransferRequestDtoValidator : AbstractValidator<RejectTransferRequestDto>
    {
        public RejectTransferRequestDtoValidator()
        {
            // Rejection Reason is required
            RuleFor(x => x.RejectionReason)
                .NotEmpty().WithMessage("Rejection reason is required")
                .MinimumLength(10).WithMessage("Rejection reason must be at least 10 characters")
                .MaximumLength(1000).WithMessage("Rejection reason cannot exceed 1000 characters");
        }
    }
}
