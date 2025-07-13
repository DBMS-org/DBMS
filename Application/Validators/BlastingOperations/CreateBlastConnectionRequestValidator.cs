using FluentValidation;
using Application.DTOs.BlastingOperations;

namespace Application.Validators.BlastingOperations
{
    public class CreateBlastConnectionRequestValidator : BaseValidator<CreateBlastConnectionRequest>
    {
        public CreateBlastConnectionRequestValidator()
        {
            RuleFor(x => x.ConnectionType)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Connection type is required and must not exceed 50 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("Project ID is required");

            RuleFor(x => x.SiteId)
                .NotEmpty()
                .WithMessage("Site ID is required");

            RuleFor(x => x.DrillHoleId)
                .NotEmpty()
                .WithMessage("Drill hole ID is required");
        }
    }
} 