using FluentValidation;
using Application.DTOs.ProjectManagement;

namespace Application.Validators.ProjectManagement
{
    public class UpdateProjectRequestValidator : BaseValidator<UpdateProjectRequest>
    {
        public UpdateProjectRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Project ID is required");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Project name is required and must not exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Status)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("Status is required and must not exceed 20 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.Region)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Region is required and must not exceed 100 characters");

            RuleFor(x => x.AssignedUserId)
                .NotEmpty()
                .WithMessage("Assigned user ID is required");
        }
    }
} 