using Domain.Entities.ExplosiveInventory.Enums;

namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// Filter DTO for querying transfer requests
    /// </summary>
    public class TransferRequestFilterDto
    {
        public TransferRequestStatus? Status { get; set; }
        public int? DestinationStoreId { get; set; }
        public int? RequestedByUserId { get; set; }
        public bool? IsOverdue { get; set; }
        public bool? IsUrgent { get; set; }
        public DateTime? RequestDateFrom { get; set; }
        public DateTime? RequestDateTo { get; set; }
        public DateTime? RequiredByDateFrom { get; set; }
        public DateTime? RequiredByDateTo { get; set; }

        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Sorting
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }
}
