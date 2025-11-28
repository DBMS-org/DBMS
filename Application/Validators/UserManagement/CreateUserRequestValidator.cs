using FluentValidation;
using Application.DTOs.UserManagement;

namespace Application.Validators.UserManagement
{
    public class CreateUserRequestValidator : BaseValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required. Please enter the user's full name.")
                .Length(1, 100)
                .WithMessage("Name must be between 1 and 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required. Please enter a valid email address.")
                .EmailAddress()
                .WithMessage("Invalid email format. Please enter a valid email address (e.g., user@example.com).")
                .MaximumLength(255)
                .WithMessage("Email must not exceed 255 characters.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required. Please enter a password.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter (A-Z).")
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at least one lowercase letter (a-z).")
                .Matches(@"\d")
                .WithMessage("Password must contain at least one digit (0-9).")
                .Matches(@"[^\w\s]")
                .WithMessage("Password must contain at least one special character (e.g., @, #, $, %, etc.).");

            RuleFor(x => x.Role)
                .NotEmpty()
                .WithMessage("Role is required. Please select a role for the user.")
                .Length(1, 50)
                .WithMessage("Role must be between 1 and 50 characters.");

            RuleFor(x => x.Region)
                .MaximumLength(100)
                .WithMessage("Origin/Region cannot exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Region));

            RuleFor(x => x.Country)
                .MaximumLength(100)
                .WithMessage("Country cannot exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Country));

            RuleFor(x => x.OmanPhone)
                .MaximumLength(20)
                .WithMessage("Resident-Country Phone number cannot exceed 20 characters.")
                .Matches(@"^[\+]?[0-9\-\(\)\s]*$")
                .WithMessage("Resident-Country Phone has invalid format. Please use only numbers, spaces, dashes, and parentheses.")
                .When(x => !string.IsNullOrEmpty(x.OmanPhone));

            RuleFor(x => x.CountryPhone)
                .MaximumLength(20)
                .WithMessage("Home-Country Phone number cannot exceed 20 characters.")
                .Matches(@"^[\+]?[0-9\-\(\)\s]*$")
                .WithMessage("Home-Country Phone has invalid format. Please use only numbers, spaces, dashes, and parentheses.")
                .When(x => !string.IsNullOrEmpty(x.CountryPhone));
        }
    }
} 