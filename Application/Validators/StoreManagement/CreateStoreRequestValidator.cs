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

            RuleFor(x => x.StorageCapacity)
                .GreaterThan(0)
                .WithMessage("Storage capacity must be greater than 0. Please enter a valid capacity.");

            RuleFor(x => x.City)
                .RequiredString("City", 1, 50);

            RuleFor(x => x.RegionId)
                .PositiveInteger("Region ID");

            RuleFor(x => x.ManagerUserId)
                .GreaterThan(0)
                .WithMessage("Please select a valid manager for this store.")
                .When(x => x.ManagerUserId.HasValue);
        }
    }
}