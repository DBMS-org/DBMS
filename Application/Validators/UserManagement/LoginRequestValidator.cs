using FluentValidation;
using Application.DTOs.UserManagement;

namespace Application.Validators.UserManagement
{
    public class LoginRequestValidator : BaseValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .RequiredString("Username", 1, 100);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required. Please enter your password.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.");
        }
    }
} 