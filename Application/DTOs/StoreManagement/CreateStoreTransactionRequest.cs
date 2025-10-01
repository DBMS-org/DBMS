using System.ComponentModel.DataAnnotations;
using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;

namespace Application.DTOs.StoreManagement
{
    public class CreateStoreTransactionRequest
    {
        [Required(ErrorMessage = "Store ID is required")]
        public int StoreId { get; set; }
        
        public int? StoreInventoryId { get; set; }
        
        [Required(ErrorMessage = "Explosive type is required")]
        public ExplosiveType ExplosiveType { get; set; }
        
        [Required(ErrorMessage = "Transaction type is required")]
        public TransactionType TransactionType { get; set; }
        
        [Required(ErrorMessage = "Quantity is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }
        
        [Required(ErrorMessage = "Unit is required")]
        [StringLength(20, ErrorMessage = "Unit cannot exceed 20 characters")]
        public string Unit { get; set; } = string.Empty;
        
        [StringLength(50, ErrorMessage = "Reference number cannot exceed 50 characters")]
        public string? ReferenceNumber { get; set; }
        
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
        
        public int? RelatedStoreId { get; set; }
        
        public int? ProcessedByUserId { get; set; }
    }
}