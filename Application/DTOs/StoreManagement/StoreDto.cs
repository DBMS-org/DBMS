using Domain.Entities.StoreManagement.Enums;

namespace Application.DTOs.StoreManagement
{
    public class StoreDto
    {
        public int Id { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public string StoreAddress { get; set; } = string.Empty;
        public decimal StorageCapacity { get; set; }
        public decimal CurrentOccupancy { get; set; }
        public string City { get; set; } = string.Empty;
        public StoreStatus Status { get; set; }
        public string AllowedExplosiveTypes { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public int? ManagerUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties (for detailed views)
        public string? RegionName { get; set; }
        public string? ManagerUserName { get; set; }
        public string? ManagerUserEmail { get; set; }
        public string? ManagerUserContact { get; set; }
        public int InventoryItemsCount { get; set; }
        public decimal UtilizationPercentage { get; set; }
    }
}