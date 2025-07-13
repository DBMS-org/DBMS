using FluentValidation;
using Application.DTOs.UserManagement;

namespace Application.Validators.UserManagement
{
    public class UpdateUserRequestValidator : BaseValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("First name is required and must not exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Last name is required and must not exceed 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Valid email address is required");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("Valid phone number is required");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Valid status is required");

            RuleFor(x => x.RoleIds)
                .NotEmpty()
                .WithMessage("At least one role must be assigned");
        }
    }
} 