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
                .WithMessage("Drill hole name is required. Please provide a name.")
                .MaximumLength(100)
                .WithMessage("Drill hole name must not exceed 100 characters.");

            RuleFor(x => x.Easting)
                .NotEmpty()
                .WithMessage("Easting coordinate is required. Please enter the easting value.");

            RuleFor(x => x.Northing)
                .NotEmpty()
                .WithMessage("Northing coordinate is required. Please enter the northing value.");

            RuleFor(x => x.Elevation)
                .NotEmpty()
                .WithMessage("Elevation is required. Please enter the elevation value.");

            RuleFor(x => x.Length)
                .GreaterThan(0)
                .WithMessage("Length must be greater than 0. Please enter a valid length.");

            RuleFor(x => x.Depth)
                .GreaterThan(0)
                .WithMessage("Depth must be greater than 0. Please enter a valid depth.");

            RuleFor(x => x.Azimuth)
                .InclusiveBetween(0, 360)
                .WithMessage("Azimuth must be between 0 and 360 degrees. Please enter a valid angle.")
                .When(x => x.Azimuth.HasValue);

            RuleFor(x => x.Dip)
                .InclusiveBetween(-90, 90)
                .WithMessage("Dip must be between -90 and 90 degrees. Please enter a valid angle.")
                .When(x => x.Dip.HasValue);

            RuleFor(x => x.ActualDepth)
                .GreaterThan(0)
                .WithMessage("Actual depth must be greater than 0. Please enter a valid depth.");

            RuleFor(x => x.Stemming)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stemming cannot be negative. Please enter a value of 0 or greater.");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0)
                .WithMessage("Please select a valid project.");

            RuleFor(x => x.SiteId)
                .GreaterThan(0)
                .WithMessage("Please select a valid site.");
        }
    }
} 