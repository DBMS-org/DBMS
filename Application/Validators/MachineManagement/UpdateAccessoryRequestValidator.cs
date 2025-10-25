using Application.DTOs.MachineManagement;
using FluentValidation;
using Domain.Entities.MachineManagement;

namespace Application.Validators.MachineManagement
{
    public class UpdateAccessoryRequestValidator : AbstractValidator<UpdateAccessoryRequest>
    {
        public UpdateAccessoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required")
                .Must(BeValidCategory).WithMessage("Invalid category");

            RuleFor(x => x.PartNumber)
                .NotEmpty().WithMessage("Part number is required")
                .MaximumLength(100).WithMessage("Part number cannot exceed 100 characters");

            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage("Unit is required")
                .Must(BeValidUnit).WithMessage("Invalid unit");

            RuleFor(x => x.MinStockLevel)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum stock level cannot be negative");

            RuleFor(x => x.Supplier)
                .NotEmpty().WithMessage("Supplier is required")
                .MaximumLength(200).WithMessage("Supplier cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Location)
                .MaximumLength(200).WithMessage("Location cannot exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Location));
        }

        private bool BeValidCategory(string category)
        {
            return Enum.TryParse<AccessoryCategory>(category, true, out _);
        }

        private bool BeValidUnit(string unit)
        {
            return Enum.TryParse<AccessoryUnit>(unit, true, out _);
        }
    }
}
