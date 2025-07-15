using FluentValidation;
using Application.DTOs.UserManagement;

namespace Application.Validators.UserManagement
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
            RuleFor(x => x.Role).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Status).NotEmpty().MaximumLength(20);
            RuleFor(x => x.OmanPhone).Matches("^\\+?[1-9]\\d{1,14}$").When(x => !string.IsNullOrEmpty(x.OmanPhone));
            RuleFor(x => x.CountryPhone).Matches("^\\+?[1-9]\\d{1,14}$").When(x => !string.IsNullOrEmpty(x.CountryPhone));
        }
    }
}