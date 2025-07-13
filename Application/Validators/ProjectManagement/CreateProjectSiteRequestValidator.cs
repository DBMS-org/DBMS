using FluentValidation;
using Application.DTOs.ProjectManagement;

namespace Application.Validators.ProjectManagement
{
    public class CreateProjectSiteRequestValidator : BaseValidator<CreateProjectSiteRequest>
    {
        public CreateProjectSiteRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Site name is required and must not exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Location)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Location is required and must not exceed 200 characters");

            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("Project ID is required");

            RuleFor(x => x.Coordinates)
                .SetValidator(new CoordinatesDtoValidator())
                .When(x => x.Coordinates != null);
        }
    }

    public class CoordinatesDtoValidator : AbstractValidator<CoordinatesDto>
    {
        public CoordinatesDtoValidator()
        {
            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude must be between -90 and 90 degrees");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude must be between -180 and 180 degrees");
        }
    }
} 