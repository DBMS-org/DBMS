using FluentValidation;
using Application.DTOs.BlastingOperations;

namespace Application.Validators.BlastingOperations
{
    public class UpdateBlastConnectionRequestValidator : BaseValidator<UpdateBlastConnectionRequest>
    {
        public UpdateBlastConnectionRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Blast connection ID is required");

            RuleFor(x => x.ConnectionType)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Connection type is required and must not exceed 50 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
} 