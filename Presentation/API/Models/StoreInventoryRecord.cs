using System;
using System.Collections.Generic;

namespace API.Models;

public partial class StoreInventoryRecord
{
    public int Id { get; set; }

    public int StoreId { get; set; }

    public int ExplosiveType { get; set; }

    public decimal Quantity { get; set; }

    public string Unit { get; set; } = null!;

    public string? BatchNumber { get; set; }

    public DateTime? ManufacturingDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? Supplier { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalValue { get; set; }

    public string? StorageLocation { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
