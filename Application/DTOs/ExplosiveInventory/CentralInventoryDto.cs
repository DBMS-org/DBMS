using Domain.Entities.ExplosiveInventory.Enums;
using Domain.Entities.StoreManagement.Enums;

namespace Application.DTOs.ExplosiveInventory
{
    public class CentralInventoryDto
    {
        public int Id { get; set; }
        public string BatchId { get; set; } = string.Empty;
        public ExplosiveType ExplosiveType { get; set; }
        public string ExplosiveTypeName { get; set; } = string.Empty;

        public decimal Quantity { get; set; }
        public decimal AllocatedQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public string Unit { get; set; } = string.Empty;

        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DaysUntilExpiry { get; set; }
        public bool IsExpired { get; set; }
        public bool IsExpiringSoon { get; set; }

        public string Supplier { get; set; } = string.Empty;
        public string? ManufacturerBatchNumber { get; set; }

        public string StorageLocation { get; set; } = string.Empty;
        public string CentralWarehouseName { get; set; } = string.Empty;

        public InventoryStatus Status { get; set; }
        public string StatusName { get; set; } = string.Empty;

        public ANFOTechnicalPropertiesDto? ANFOProperties { get; set; }
        public EmulsionTechnicalPropertiesDto? EmulsionProperties { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
