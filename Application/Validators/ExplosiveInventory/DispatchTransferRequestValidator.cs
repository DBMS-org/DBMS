using Application.DTOs.ExplosiveInventory;
using FluentValidation;

namespace Application.Validators.ExplosiveInventory
{
    public class DispatchTransferRequestValidator : AbstractValidator<DispatchTransferRequestDto>
    {
        public DispatchTransferRequestValidator()
        {
            RuleFor(x => x.TruckNumber)
                .NotEmpty().WithMessage("Truck number is required")
                .MaximumLength(50).WithMessage("Truck number cannot exceed 50 characters");

            RuleFor(x => x.DriverName)
                .NotEmpty().WithMessage("Driver name is required")
                .MaximumLength(200).WithMessage("Driver name cannot exceed 200 characters");

            RuleFor(x => x.DriverContactNumber)
                .MaximumLength(20).WithMessage("Driver contact number cannot exceed 20 characters")
                .When(x => !string.IsNullOrEmpty(x.DriverContactNumber));

            RuleFor(x => x.DispatchNotes)
                .MaximumLength(1000).WithMessage("Dispatch notes cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.DispatchNotes));
        }
    }
}
