using System.ComponentModel.DataAnnotations;
using Domain.Entities.StoreManagement.Enums;

namespace Application.DTOs.StoreManagement
{
    public class UpdateStoreRequest
    {
        [Required(ErrorMessage = "Store name is required")]
        [StringLength(100, ErrorMessage = "Store name cannot exceed 100 characters")]
        public string StoreName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Store address is required")]
        [StringLength(200, ErrorMessage = "Store address cannot exceed 200 characters")]
        public string StoreAddress { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Store manager name is required")]
        [StringLength(100, ErrorMessage = "Store manager name cannot exceed 100 characters")]
        public string StoreManagerName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Store manager contact is required")]
        [StringLength(20, ErrorMessage = "Store manager contact cannot exceed 20 characters")]
        public string StoreManagerContact { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Store manager email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Store manager email cannot exceed 100 characters")]
        public string StoreManagerEmail { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Storage capacity is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Storage capacity must be greater than 0")]
        public decimal StorageCapacity { get; set; }
        
        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string City { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Status is required")]
        public StoreStatus Status { get; set; }
        
        public int? ProjectId { get; set; }
        
        public int? ManagerUserId { get; set; }
    }
}