using FluentValidation;
using Application.DTOs.MachineManagement;

namespace Application.Validators.MachineManagement
{
    public class CreateMachineRequestValidator : BaseValidator<CreateMachineRequest>
    {
        public CreateMachineRequestValidator()
        {
            RuleFor(x => x.Name)
                .RequiredString("Machine name", 1, 100);

            RuleFor(x => x.Type)
                .RequiredString("Machine type", 1, 50);

            RuleFor(x => x.Model)
                .RequiredString("Model", 1, 100);

            RuleFor(x => x.Manufacturer)
                .RequiredString("Manufacturer", 1, 100);

            RuleFor(x => x.SerialNumber)
                .RequiredString("Serial number", 1, 100);

            RuleFor(x => x.Status)
                .RequiredString("Status", 1, 50);

            RuleFor(x => x.ProjectId)
                .PositiveInteger("Project ID");

            RuleFor(x => x.RigNo)
                .MaximumLength(50)
                .WithMessage("Rig number cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.RigNo));

            RuleFor(x => x.PlateNo)
                .MaximumLength(50)
                .WithMessage("Plate number cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.PlateNo));

            RuleFor(x => x.ChassisDetails)
                .MaximumLength(500)
                .WithMessage("Chassis details cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.ChassisDetails));

            RuleFor(x => x.CurrentLocation)
                .MaximumLength(200)
                .WithMessage("Current location cannot exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.CurrentLocation));

            RuleFor(x => x.ManufacturingYear)
                .InclusiveBetween(1900, 2100)
                .WithMessage("Manufacturing year must be between 1900 and 2100")
                .When(x => x.ManufacturingYear.HasValue);

            RuleFor(x => x.OperatorId)
                .GreaterThan(0)
                .WithMessage("Operator ID must be greater than 0")
                .When(x => x.OperatorId.HasValue);

            RuleFor(x => x.RegionId)
                .GreaterThan(0)
                .WithMessage("Region ID must be greater than 0")
                .When(x => x.RegionId.HasValue);
        }
    }
} 