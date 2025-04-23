using FluentValidation;
using CleanArchitecture.Application.Commands;

namespace CleanArchitecture.Application.Validators
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.");
                
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
} 