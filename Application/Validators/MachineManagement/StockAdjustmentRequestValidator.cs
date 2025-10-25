using Application.DTOs.MachineManagement;
using FluentValidation;
using Domain.Entities.MachineManagement;

namespace Application.Validators.MachineManagement
{
    public class StockAdjustmentRequestValidator : AbstractValidator<StockAdjustmentRequest>
    {
        public StockAdjustmentRequestValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Adjustment type is required")
                .Must(BeValidAdjustmentType).WithMessage("Invalid adjustment type. Valid values: Add, Remove, Set");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason is required")
                .Must(BeValidReason).WithMessage("Invalid reason. Valid values: Purchase, Usage, Damaged, Lost, Returned, Correction, Other");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Notes));
        }

        private bool BeValidAdjustmentType(string type)
        {
            return Enum.TryParse<StockAdjustmentType>(type, true, out _);
        }

        private bool BeValidReason(string reason)
        {
            return Enum.TryParse<StockAdjustmentReason>(reason, true, out _);
        }
    }
}
