using System;
using System.Collections.Generic;

namespace API.Models;

public partial class StoreTransaction
{
    public int Id { get; set; }

    public int StoreId { get; set; }

    public int? StoreInventoryId { get; set; }

    public string ExplosiveType { get; set; } = null!;

    public string TransactionType { get; set; } = null!;

    public decimal Quantity { get; set; }

    public string Unit { get; set; } = null!;

    public string? ReferenceNumber { get; set; }

    public string? Notes { get; set; }

    public int? RelatedStoreId { get; set; }

    public int? ProcessedByUserId { get; set; }

    public DateTime TransactionDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<InventoryTransferRequest> InventoryTransferRequests { get; set; } = new List<InventoryTransferRequest>();

    public virtual User? ProcessedByUser { get; set; }

    public virtual Store? RelatedStore { get; set; }

    public virtual Store Store { get; set; } = null!;

    public virtual StoreInventory? StoreInventory { get; set; }
}
