using System;
using System.Collections.Generic;

namespace API.Models;

public partial class InventoryTransferRequest
{
    public int Id { get; set; }

    public string RequestNumber { get; set; } = null!;

    public int CentralWarehouseInventoryId { get; set; }

    public int DestinationStoreId { get; set; }

    public int? DispatchedByUserId { get; set; }

    public decimal RequestedQuantity { get; set; }

    public decimal? ApprovedQuantity { get; set; }

    public string Unit { get; set; } = null!;

    public int Status { get; set; }

    public DateTime RequestDate { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public DateTime? RequiredByDate { get; set; }

    public int RequestedByUserId { get; set; }

    public int? ApprovedByUserId { get; set; }

    public int? ProcessedByUserId { get; set; }

    public string? RequestNotes { get; set; }

    public string? ApprovalNotes { get; set; }

    public string? RejectionReason { get; set; }

    public int? CompletedTransactionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public DateTime? DeliveryConfirmedDate { get; set; }

    public DateTime? DispatchDate { get; set; }

    public string? DispatchNotes { get; set; }

    public string? DriverContactNumber { get; set; }

    public string? DriverName { get; set; }

    public string? TruckNumber { get; set; }

    public virtual User? ApprovedByUser { get; set; }

    public virtual CentralWarehouseInventory CentralWarehouseInventory { get; set; } = null!;

    public virtual StoreTransaction? CompletedTransaction { get; set; }

    public virtual Store DestinationStore { get; set; } = null!;

    public virtual User? DispatchedByUser { get; set; }

    public virtual User? ProcessedByUser { get; set; }

    public virtual User RequestedByUser { get; set; } = null!;
}
