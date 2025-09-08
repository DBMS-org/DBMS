using FluentValidation;
using Application.DTOs.DrillingOperations;

namespace Application.Validators.DrillingOperations
{
    public class UpdateExplosiveCalculationResultRequestValidator : BaseValidator<UpdateExplosiveCalculationResultRequest>
    {
        public UpdateExplosiveCalculationResultRequestValidator()
        {
            RuleFor(x => x.CalculationId)
                .MaximumLength(50)
                .WithMessage("Calculation ID must not exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.CalculationId));

            RuleFor(x => x.PatternSettingsId)
                .GreaterThan(0)
                .WithMessage("Pattern settings ID must be greater than 0")
                .When(x => x.PatternSettingsId.HasValue);

            RuleFor(x => x.EmulsionDensity)
                .GreaterThan(0)
                .LessThanOrEqualTo(10)
                .WithMessage("Emulsion density must be greater than 0 and not exceed 10 g/cm³")
                .When(x => x.EmulsionDensity.HasValue);

            RuleFor(x => x.AnfoDensity)
                .GreaterThan(0)
                .LessThanOrEqualTo(5)
                .WithMessage("ANFO density must be greater than 0 and not exceed 5 g/cm³")
                .When(x => x.AnfoDensity.HasValue);

            RuleFor(x => x.EmulsionPerHole)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Emulsion per hole must be greater than or equal to 0")
                .When(x => x.EmulsionPerHole.HasValue);

            RuleFor(x => x.TotalDepth)
                .GreaterThan(0)
                .WithMessage("Total depth must be greater than 0")
                .When(x => x.TotalDepth.HasValue);

            RuleFor(x => x.AverageDepth)
                .GreaterThan(0)
                .WithMessage("Average depth must be greater than 0")
                .When(x => x.AverageDepth.HasValue);

            RuleFor(x => x.NumberOfFilledHoles)
                .GreaterThan(0)
                .WithMessage("Number of filled holes must be greater than 0")
                .When(x => x.NumberOfFilledHoles.HasValue);

            RuleFor(x => x.EmulsionPerMeter)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Emulsion per meter must be greater than or equal to 0")
                .When(x => x.EmulsionPerMeter.HasValue);

            RuleFor(x => x.AnfoPerMeter)
                .GreaterThanOrEqualTo(0)
                .WithMessage("ANFO per meter must be greater than or equal to 0")
                .When(x => x.AnfoPerMeter.HasValue);

            RuleFor(x => x.EmulsionCoveringSpace)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Emulsion covering space must be greater than or equal to 0")
                .When(x => x.EmulsionCoveringSpace.HasValue);

            RuleFor(x => x.RemainingSpace)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Remaining space must be greater than or equal to 0")
                .When(x => x.RemainingSpace.HasValue);

            RuleFor(x => x.AnfoCoveringSpace)
                .GreaterThanOrEqualTo(0)
                .WithMessage("ANFO covering space must be greater than or equal to 0")
                .When(x => x.AnfoCoveringSpace.HasValue);

            RuleFor(x => x.TotalAnfo)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Total ANFO must be greater than or equal to 0")
                .When(x => x.TotalAnfo.HasValue);

            RuleFor(x => x.TotalEmulsion)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Total emulsion must be greater than or equal to 0")
                .When(x => x.TotalEmulsion.HasValue);

            RuleFor(x => x.TotalVolume)
                .GreaterThan(0)
                .WithMessage("Total volume must be greater than 0")
                .When(x => x.TotalVolume.HasValue);

            // Cross-field validation for update requests
            RuleFor(x => x)
                .Must(x => {
                    if (!x.EmulsionCoveringSpace.HasValue && !x.AnfoCoveringSpace.HasValue && 
                        !x.TotalDepth.HasValue && !x.NumberOfFilledHoles.HasValue)
                        return true;
                    
                    var emulsionSpace = x.EmulsionCoveringSpace ?? 0;
                    var anfoSpace = x.AnfoCoveringSpace ?? 0;
                    var totalDepth = x.TotalDepth ?? 1; // Default to avoid division by zero
                    var holes = x.NumberOfFilledHoles ?? 1;
                    
                    return emulsionSpace + anfoSpace <= totalDepth * holes;
                })
                .WithMessage("Total covering space cannot exceed total available space")
                .When(x => x.EmulsionCoveringSpace.HasValue || x.AnfoCoveringSpace.HasValue || 
                          x.TotalDepth.HasValue || x.NumberOfFilledHoles.HasValue);

            RuleFor(x => x)
                .Must(x => {
                    if (!x.TotalAnfo.HasValue && !x.TotalEmulsion.HasValue && !x.TotalVolume.HasValue)
                        return true;
                    
                    var totalAnfo = x.TotalAnfo ?? 0;
                    var totalEmulsion = x.TotalEmulsion ?? 0;
                    var totalVolume = x.TotalVolume ?? (totalAnfo + totalEmulsion);
                    
                    return totalAnfo + totalEmulsion <= totalVolume;
                })
                .WithMessage("Total explosive materials cannot exceed total volume")
                .When(x => x.TotalAnfo.HasValue || x.TotalEmulsion.HasValue || x.TotalVolume.HasValue);
        }
    }
}