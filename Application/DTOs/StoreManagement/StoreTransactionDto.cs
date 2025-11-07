using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;

namespace Application.DTOs.StoreManagement
{
    public class StoreTransactionDto
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int? StoreInventoryId { get; set; }
        public ExplosiveType ExplosiveType { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public int? RelatedStoreId { get; set; }
        public int? ProcessedByUserId { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties (for detailed views)
        public string? StoreName { get; set; }
        public string? RelatedStoreName { get; set; }
        public string? ProcessedByUserName { get; set; }
        public string TransactionTypeName { get; set; } = string.Empty;
    }
}