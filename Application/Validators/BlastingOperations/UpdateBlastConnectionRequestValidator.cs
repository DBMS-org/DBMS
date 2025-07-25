using FluentValidation;
using Application.DTOs.BlastingOperations;

namespace Application.Validators.BlastingOperations
{
    public class UpdateBlastConnectionRequestValidator : BaseValidator<UpdateBlastConnectionRequest>
    {
        public UpdateBlastConnectionRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Blast connection ID is required");

            RuleFor(x => x.Point1DrillPointId)
                .NotEmpty()
                .WithMessage("Point 1 drill point ID is required");

            RuleFor(x => x.Point2DrillPointId)
                .NotEmpty()
                .WithMessage("Point 2 drill point ID is required");

            RuleFor(x => x.ConnectorType)
                .IsInEnum()
                .WithMessage("Valid connector type is required");

            RuleFor(x => x.Delay)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Delay must be greater than or equal to 0");

            RuleFor(x => x.Sequence)
                .GreaterThan(0)
                .WithMessage("Sequence must be greater than 0");

            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("Project ID is required");

            RuleFor(x => x.SiteId)
                .NotEmpty()
                .WithMessage("Site ID is required");
        }
    }
} 