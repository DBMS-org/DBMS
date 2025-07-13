using FluentValidation;
using Application.DTOs.DrillingOperations;

namespace Application.Validators.DrillingOperations
{
    public class CreateDrillHoleRequestValidator : BaseValidator<CreateDrillHoleRequest>
    {
        public CreateDrillHoleRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Drill hole name is required and must not exceed 100 characters");

            RuleFor(x => x.Easting)
                .NotEmpty()
                .WithMessage("Easting coordinate is required");

            RuleFor(x => x.Northing)
                .NotEmpty()
                .WithMessage("Northing coordinate is required");

            RuleFor(x => x.Elevation)
                .NotEmpty()
                .WithMessage("Elevation is required");

            RuleFor(x => x.Length)
                .GreaterThan(0)
                .WithMessage("Length must be greater than 0");

            RuleFor(x => x.Depth)
                .GreaterThan(0)
                .WithMessage("Depth must be greater than 0");

            RuleFor(x => x.Azimuth)
                .InclusiveBetween(0, 360)
                .WithMessage("Azimuth must be between 0 and 360 degrees")
                .When(x => x.Azimuth.HasValue);

            RuleFor(x => x.Dip)
                .InclusiveBetween(-90, 90)
                .WithMessage("Dip must be between -90 and 90 degrees")
                .When(x => x.Dip.HasValue);

            RuleFor(x => x.ActualDepth)
                .GreaterThan(0)
                .WithMessage("Actual depth must be greater than 0");

            RuleFor(x => x.Stemming)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stemming must be greater than or equal to 0");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0)
                .WithMessage("Valid project ID is required");

            RuleFor(x => x.SiteId)
                .GreaterThan(0)
                .WithMessage("Valid site ID is required");
        }
    }
} 