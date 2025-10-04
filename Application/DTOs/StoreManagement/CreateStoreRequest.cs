namespace Application.DTOs.StoreManagement
{
    public class CreateStoreRequest
    {
        public string StoreName { get; set; } = string.Empty;

        public string StoreAddress { get; set; } = string.Empty;

        public decimal StorageCapacity { get; set; }

        public string City { get; set; } = string.Empty;

        public string? AllowedExplosiveTypes { get; set; } // Comma-separated: "ANFO,Emulsion"

        public int RegionId { get; set; }

        public int? ManagerUserId { get; set; }
    }
}