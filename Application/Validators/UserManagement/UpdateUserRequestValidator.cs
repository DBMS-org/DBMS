using FluentValidation;
using Application.DTOs.UserManagement;

namespace Application.Validators.UserManagement
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required. Please enter the user's full name.")
                .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required. Please enter a valid email address.")
                .EmailAddress()
                .WithMessage("Invalid email format. Please enter a valid email address (e.g., user@example.com).")
                .MaximumLength(255)
                .WithMessage("Email must not exceed 255 characters.");

            RuleFor(x => x.Role)
                .NotEmpty()
                .WithMessage("Role is required. Please select a role for the user.")
                .MaximumLength(50)
                .WithMessage("Role cannot exceed 50 characters.");

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status is required. Please select a status for the user.")
                .MaximumLength(20)
                .WithMessage("Status cannot exceed 20 characters.");

            RuleFor(x => x.OmanPhone)
                .Matches(@"^[\+]?[0-9\-\(\)\s]*$")
                .WithMessage("Resident-Country Phone has invalid format. Please use only numbers, spaces, dashes, and parentheses.")
                .MaximumLength(20)
                .WithMessage("Resident-Country Phone number cannot exceed 20 characters.")
                .When(x => !string.IsNullOrEmpty(x.OmanPhone));

            RuleFor(x => x.CountryPhone)
                .Matches(@"^[\+]?[0-9\-\(\)\s]*$")
                .WithMessage("Home-Country Phone has invalid format. Please use only numbers, spaces, dashes, and parentheses.")
                .MaximumLength(20)
                .WithMessage("Home-Country Phone number cannot exceed 20 characters.")
                .When(x => !string.IsNullOrEmpty(x.CountryPhone));
        }
    }
}