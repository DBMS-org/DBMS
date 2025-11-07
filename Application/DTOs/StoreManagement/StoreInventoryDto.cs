using Domain.Entities.StoreManagement.Enums;

namespace Application.DTOs.StoreManagement
{
    public class StoreInventoryDto
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public ExplosiveType ExplosiveType { get; set; }
        public decimal Quantity { get; set; }
        public decimal ReservedQuantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal MinimumStockLevel { get; set; }
        public decimal MaximumStockLevel { get; set; }
        public DateTime? LastRestockedAt { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? BatchNumber { get; set; }
        public string? Supplier { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties (for detailed views)
        public string? StoreName { get; set; }
        public decimal AvailableQuantity { get; set; }
        public bool IsLowStock { get; set; }
        public bool IsOverStock { get; set; }
        public int DaysUntilExpiry { get; set; }
    }
}