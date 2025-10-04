using Domain.Entities.StoreManagement.Enums;

namespace Application.DTOs.StoreManagement
{
    public class UpdateStoreRequest
    {
        public string StoreName { get; set; } = string.Empty;

        public string StoreAddress { get; set; } = string.Empty;

        public decimal StorageCapacity { get; set; }

        public string City { get; set; } = string.Empty;

        public string? AllowedExplosiveTypes { get; set; } // Comma-separated: "ANFO,Emulsion"

        public StoreStatus Status { get; set; }

        public int? ManagerUserId { get; set; }
    }
}