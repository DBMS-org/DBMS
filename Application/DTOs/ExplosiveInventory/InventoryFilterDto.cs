using Domain.Entities.ExplosiveInventory.Enums;
using Domain.Entities.StoreManagement.Enums;

namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// Filter DTO for querying inventory
    /// </summary>
    public class InventoryFilterDto
    {
        public ExplosiveType? ExplosiveType { get; set; }
        public InventoryStatus? Status { get; set; }
        public string? Supplier { get; set; }
        public string? BatchId { get; set; }
        public bool? IsExpired { get; set; }
        public bool? IsExpiringSoon { get; set; }
        public DateTime? ManufacturingDateFrom { get; set; }
        public DateTime? ManufacturingDateTo { get; set; }
        public DateTime? ExpiryDateFrom { get; set; }
        public DateTime? ExpiryDateTo { get; set; }

        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Sorting
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }
}
