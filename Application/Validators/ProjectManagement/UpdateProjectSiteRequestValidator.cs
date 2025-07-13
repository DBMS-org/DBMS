using FluentValidation;
using Application.DTOs.ProjectManagement;

namespace Application.Validators.ProjectManagement
{
    public class UpdateProjectSiteRequestValidator : BaseValidator<UpdateProjectSiteRequest>
    {
        public UpdateProjectSiteRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Site ID is required");

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

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Valid site status is required");

            RuleFor(x => x.Coordinates)
                .SetValidator(new CoordinatesDtoValidator())
                .When(x => x.Coordinates != null);
        }
    }
} 