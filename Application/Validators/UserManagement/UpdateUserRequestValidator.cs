using FluentValidation;
using Application.DTOs.UserManagement;

namespace Application.Validators.UserManagement
{
    public class UpdateUserRequestValidator : BaseValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Name)
                .RequiredString("Name", 1, 100);

            RuleFor(x => x.Email)
                .ValidEmail();

            RuleFor(x => x.Role)
                .RequiredString("Role", 1, 50);

            RuleFor(x => x.Status)
                .RequiredString("Status", 1, 20);

            RuleFor(x => x.Region)
                .MaximumLength(100)
                .WithMessage("Region cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Region));

            RuleFor(x => x.Country)
                .MaximumLength(100)
                .WithMessage("Country cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Country));

            RuleFor(x => x.OmanPhone)
                .MaximumLength(20)
                .WithMessage("Oman phone number cannot exceed 20 characters")
                .Matches(@"^[\+]?[0-9\-\(\)\s]*$")
                .WithMessage("Invalid phone number format")
                .When(x => !string.IsNullOrEmpty(x.OmanPhone));

            RuleFor(x => x.CountryPhone)
                .MaximumLength(20)
                .WithMessage("Country phone number cannot exceed 20 characters")
                .Matches(@"^[\+]?[0-9\-\(\)\s]*$")
                .WithMessage("Invalid phone number format")
                .When(x => !string.IsNullOrEmpty(x.CountryPhone));
        }
    }
} 