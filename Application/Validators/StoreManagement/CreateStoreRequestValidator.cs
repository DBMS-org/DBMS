using FluentValidation;
using Application.DTOs.StoreManagement;

namespace Application.Validators.StoreManagement
{
    public class CreateStoreRequestValidator : BaseValidator<CreateStoreRequest>
    {
        public CreateStoreRequestValidator()
        {
            RuleFor(x => x.StoreName)
                .RequiredString("Store name", 1, 100);

            RuleFor(x => x.StoreAddress)
                .RequiredString("Store address", 1, 200);

            RuleFor(x => x.StoreManagerName)
                .RequiredString("Store manager name", 1, 100);

            RuleFor(x => x.StoreManagerContact)
                .RequiredString("Store manager contact", 1, 20);

            RuleFor(x => x.StoreManagerEmail)
                .ValidEmail();

            RuleFor(x => x.StorageCapacity)
                .GreaterThan(0)
                .WithMessage("Storage capacity must be greater than 0");

            RuleFor(x => x.City)
                .RequiredString("City", 1, 50);

            RuleFor(x => x.RegionId)
                .PositiveInteger("Region ID");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0)
                .WithMessage("Project ID must be greater than 0")
                .When(x => x.ProjectId.HasValue);

            RuleFor(x => x.ManagerUserId)
                .GreaterThan(0)
                .WithMessage("Manager User ID must be greater than 0")
                .When(x => x.ManagerUserId.HasValue);
        }
    }
}