using System.ComponentModel.DataAnnotations;
using Domain.Entities.StoreManagement.Enums;

namespace Application.DTOs.StoreManagement
{
    public class CreateStoreInventoryRequest
    {
        [Required(ErrorMessage = "Store ID is required")]
        public int StoreId { get; set; }
        
        [Required(ErrorMessage = "Explosive type is required")]
        public ExplosiveType ExplosiveType { get; set; }
        
        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Quantity must be non-negative")]
        public decimal Quantity { get; set; }
        
        [Required(ErrorMessage = "Unit is required")]
        [StringLength(20, ErrorMessage = "Unit cannot exceed 20 characters")]
        public string Unit { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Minimum stock level is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Minimum stock level must be non-negative")]
        public decimal MinimumStockLevel { get; set; }
        
        [Required(ErrorMessage = "Maximum stock level is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Maximum stock level must be non-negative")]
        public decimal MaximumStockLevel { get; set; }
        
        public DateTime? ExpiryDate { get; set; }
        
        [StringLength(50, ErrorMessage = "Batch number cannot exceed 50 characters")]
        public string? BatchNumber { get; set; }
        
        [StringLength(100, ErrorMessage = "Supplier cannot exceed 100 characters")]
        public string? Supplier { get; set; }
    }
}