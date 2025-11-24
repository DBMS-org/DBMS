using System;
using System.Collections.Generic;

namespace API.Models;

public partial class StockRequest
{
    public int Id { get; set; }

    public string RequestNumber { get; set; } = null!;

    public int RequesterId { get; set; }

    public int? RequesterStoreId { get; set; }

    public int? SupplierStoreId { get; set; }

    public DateTime RequestDate { get; set; }

    public DateTime RequiredDate { get; set; }

    public int Status { get; set; }

    public string Purpose { get; set; } = null!;

    public string? Justification { get; set; }

    public string? Notes { get; set; }

    public int? ApprovedById { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string? ApprovalNotes { get; set; }

    public int? RejectedById { get; set; }

    public DateTime? RejectedDate { get; set; }

    public string? RejectionReason { get; set; }

    public DateTime? CompletedDate { get; set; }

    public string Priority { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual User? ApprovedBy { get; set; }

    public virtual User? RejectedBy { get; set; }

    public virtual User Requester { get; set; } = null!;

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    public virtual ICollection<StockRequestItem> StockRequestItems { get; set; } = new List<StockRequestItem>();
}
