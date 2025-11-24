using System;
using System.Collections.Generic;

namespace API.Models;

public partial class StoreInventory
{
    public int Id { get; set; }

    public int StoreId { get; set; }

    public string ExplosiveType { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal ReservedQuantity { get; set; }

    public string Unit { get; set; } = null!;

    public decimal MinimumStockLevel { get; set; }

    public decimal MaximumStockLevel { get; set; }

    public DateTime? LastRestockedAt { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? BatchNumber { get; set; }

    public string? Supplier { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Store Store { get; set; } = null!;

    public virtual ICollection<StoreTransaction> StoreTransactions { get; set; } = new List<StoreTransaction>();
}
