using Domain.Entities.ExplosiveInventory.Enums;

namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// Response DTO for Inventory Transfer Request
    /// </summary>
    public class TransferRequestDto
    {
        public int Id { get; set; }
        public string RequestNumber { get; set; } = string.Empty;

        // Source & Destination
        public int CentralWarehouseInventoryId { get; set; }
        public string BatchId { get; set; } = string.Empty;
        public string ExplosiveTypeName { get; set; } = string.Empty;
        public int DestinationStoreId { get; set; }
        public string DestinationStoreName { get; set; } = string.Empty;

        // Quantity
        public decimal RequestedQuantity { get; set; }
        public decimal? ApprovedQuantity { get; set; }
        public decimal FinalQuantity { get; set; }
        public string Unit { get; set; } = string.Empty;

        // Status & Workflow
        public TransferRequestStatus Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? RequiredByDate { get; set; }
        public int? DaysUntilRequired { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsUrgent { get; set; }

        // Dispatch/Delivery Tracking
        public DateTime? DispatchDate { get; set; }
        public string? TruckNumber { get; set; }
        public string? DriverName { get; set; }
        public string? DriverContactNumber { get; set; }
        public string? DispatchNotes { get; set; }
        public int? DispatchedByUserId { get; set; }
        public string? DispatchedByUserName { get; set; }
        public DateTime? DeliveryConfirmedDate { get; set; }

        // People
        public int RequestedByUserId { get; set; }
        public string RequestedByUserName { get; set; } = string.Empty;
        public int? ApprovedByUserId { get; set; }
        public string? ApprovedByUserName { get; set; }
        public int? ProcessedByUserId { get; set; }
        public string? ProcessedByUserName { get; set; }

        // Notes
        public string? RequestNotes { get; set; }
        public string? ApprovalNotes { get; set; }
        public string? RejectionReason { get; set; }

        // Metadata
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
