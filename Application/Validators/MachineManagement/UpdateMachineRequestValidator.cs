using FluentValidation;
using Application.DTOs.MachineManagement;

namespace Application.Validators.MachineManagement
{
    public class UpdateMachineRequestValidator : BaseValidator<UpdateMachineRequest>
    {
        public UpdateMachineRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Machine ID is required");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Machine name is required and must not exceed 100 characters");

            RuleFor(x => x.Type)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Machine type is required and must not exceed 50 characters");

            RuleFor(x => x.Model)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Machine model is required and must not exceed 50 characters");

            RuleFor(x => x.Manufacturer)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Manufacturer is required and must not exceed 100 characters");

            RuleFor(x => x.ManufacturingYear)
                .InclusiveBetween(1900, 2100)
                .WithMessage("Manufacturing year must be between 1900 and 2100");

            RuleFor(x => x.SerialNumber)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Serial number is required and must not exceed 100 characters");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Valid machine status is required");

            RuleFor(x => x.Location)
                .MaximumLength(200)
                .WithMessage("Location must not exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Location));

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
} 