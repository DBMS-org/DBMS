using Domain.Entities.StoreManagement.Enums;

namespace Application.DTOs.StoreManagement
{
    public class UpdateStoreStatusRequest
    {
        public StoreStatus Status { get; set; }
    }
}
