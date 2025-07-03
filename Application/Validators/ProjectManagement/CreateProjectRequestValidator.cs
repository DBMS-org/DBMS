using FluentValidation;
using Application.DTOs.ProjectManagement;

namespace Application.Validators.ProjectManagement
{
    public class CreateProjectRequestValidator : BaseValidator<CreateProjectRequest>
    {
        public CreateProjectRequestValidator()
        {
            RuleFor(x => x.Name)
                .RequiredString("Project name", 1, 100);

            RuleFor(x => x.Region)
                .RequiredString("Region", 1, 100);

            RuleFor(x => x.Status)
                .RequiredString("Status", 1, 20);

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Description cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate)
                .WithMessage("Start date must be before or equal to end date")
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue);

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date must be after or equal to start date")
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue);

            RuleFor(x => x.AssignedUserId)
                .GreaterThan(0)
                .WithMessage("Assigned User ID must be greater than 0")
                .When(x => x.AssignedUserId.HasValue);
        }
    }
} 