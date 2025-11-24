using System;
using System.Collections.Generic;

namespace API.Models;

public partial class StockRequestItem
{
    public int Id { get; set; }

    public int StockRequestId { get; set; }

    public int ExplosiveType { get; set; }

    public decimal RequestedQuantity { get; set; }

    public string Unit { get; set; } = null!;

    public decimal? ApprovedQuantity { get; set; }

    public decimal? FulfilledQuantity { get; set; }

    public string? Specifications { get; set; }

    public string? Notes { get; set; }

    public string? PreferredBatchNumber { get; set; }

    public DateTime? PreferredManufacturingDate { get; set; }

    public decimal? EstimatedUnitPrice { get; set; }

    public decimal? EstimatedTotalCost { get; set; }

    public decimal? ActualUnitPrice { get; set; }

    public decimal? ActualTotalCost { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    public virtual StockRequest StockRequest { get; set; } = null!;
}
