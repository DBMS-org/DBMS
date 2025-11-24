using System;
using System.Collections.Generic;

namespace API.Models;

public partial class StockMovement
{
    public int Id { get; set; }

    public string MovementNumber { get; set; } = null!;

    public int MovementType { get; set; }

    public int StoreId { get; set; }

    public int? RelatedStoreId { get; set; }

    public int? StoreInventoryRecordId { get; set; }

    public int? StockRequestId { get; set; }

    public int? StockRequestItemId { get; set; }

    public int ExplosiveType { get; set; }

    public decimal Quantity { get; set; }

    public string Unit { get; set; } = null!;

    public decimal? UnitPrice { get; set; }

    public decimal? TotalValue { get; set; }

    public string? BatchNumber { get; set; }

    public DateTime? ManufacturingDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? Supplier { get; set; }

    public DateTime MovementDate { get; set; }

    public int ProcessedById { get; set; }

    public string Reason { get; set; } = null!;

    public string? Notes { get; set; }

    public string? ReferenceNumber { get; set; }

    public decimal? BalanceBefore { get; set; }

    public decimal? BalanceAfter { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual User ProcessedBy { get; set; } = null!;

    public virtual StockRequest? StockRequest { get; set; }

    public virtual StockRequestItem? StockRequestItem { get; set; }

    public virtual StoreInventoryRecord? StoreInventoryRecord { get; set; }
}
