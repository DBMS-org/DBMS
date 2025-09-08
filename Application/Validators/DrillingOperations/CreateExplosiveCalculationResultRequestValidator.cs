using FluentValidation;
using Application.DTOs.DrillingOperations;

namespace Application.Validators.DrillingOperations
{
    public class CreateExplosiveCalculationResultRequestValidator : BaseValidator<CreateExplosiveCalculationResultRequest>
    {
        public CreateExplosiveCalculationResultRequestValidator()
        {
            RuleFor(x => x.CalculationId)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Calculation ID is required and must not exceed 50 characters");

            RuleFor(x => x.PatternSettingsId)
                .GreaterThan(0)
                .WithMessage("Valid pattern settings ID is required");

            RuleFor(x => x.EmulsionDensity)
                .GreaterThan(0)
                .LessThanOrEqualTo(10)
                .WithMessage("Emulsion density must be greater than 0 and not exceed 10 g/cm³");

            RuleFor(x => x.AnfoDensity)
                .GreaterThan(0)
                .LessThanOrEqualTo(5)
                .WithMessage("ANFO density must be greater than 0 and not exceed 5 g/cm³");

            RuleFor(x => x.EmulsionPerHole)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Emulsion per hole must be greater than or equal to 0");

            RuleFor(x => x.TotalDepth)
                .GreaterThan(0)
                .WithMessage("Total depth must be greater than 0");

            RuleFor(x => x.AverageDepth)
                .GreaterThan(0)
                .WithMessage("Average depth must be greater than 0");

            RuleFor(x => x.NumberOfFilledHoles)
                .GreaterThan(0)
                .WithMessage("Number of filled holes must be greater than 0");

            RuleFor(x => x.EmulsionPerMeter)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Emulsion per meter must be greater than or equal to 0");

            RuleFor(x => x.AnfoPerMeter)
                .GreaterThanOrEqualTo(0)
                .WithMessage("ANFO per meter must be greater than or equal to 0");

            RuleFor(x => x.EmulsionCoveringSpace)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Emulsion covering space must be greater than or equal to 0");

            RuleFor(x => x.RemainingSpace)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Remaining space must be greater than or equal to 0");

            RuleFor(x => x.AnfoCoveringSpace)
                .GreaterThanOrEqualTo(0)
                .WithMessage("ANFO covering space must be greater than or equal to 0");

            RuleFor(x => x.TotalAnfo)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Total ANFO must be greater than or equal to 0");

            RuleFor(x => x.TotalEmulsion)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Total emulsion must be greater than or equal to 0");

            RuleFor(x => x.TotalVolume)
                .GreaterThan(0)
                .WithMessage("Total volume must be greater than 0");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0)
                .WithMessage("Valid project ID is required");

            RuleFor(x => x.SiteId)
                .GreaterThan(0)
                .WithMessage("Valid site ID is required");

            RuleFor(x => x.OwningUserId)
                .GreaterThan(0)
                .WithMessage("Valid owning user ID is required");

            // Cross-field validation
            RuleFor(x => x)
                .Must(x => x.EmulsionCoveringSpace + x.AnfoCoveringSpace <= x.TotalDepth * x.NumberOfFilledHoles)
                .WithMessage("Total covering space cannot exceed total available space")
                .When(x => x.EmulsionCoveringSpace > 0 || x.AnfoCoveringSpace > 0);

            RuleFor(x => x)
                .Must(x => x.TotalAnfo + x.TotalEmulsion <= x.TotalVolume)
                .WithMessage("Total explosive materials cannot exceed total volume")
                .When(x => x.TotalAnfo > 0 || x.TotalEmulsion > 0);
        }
    }
}