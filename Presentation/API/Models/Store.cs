using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Store
{
    public int Id { get; set; }

    public string StoreName { get; set; } = null!;

    public string StoreAddress { get; set; } = null!;

    public decimal StorageCapacity { get; set; }

    public string City { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int RegionId { get; set; }

    public int? ProjectId { get; set; }

    public int? ManagerUserId { get; set; }

    public int? UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public string AllowedExplosiveTypes { get; set; } = null!;

    public virtual ICollection<CentralWarehouseInventory> CentralWarehouseInventories { get; set; } = new List<CentralWarehouseInventory>();

    public virtual ICollection<InventoryTransferRequest> InventoryTransferRequests { get; set; } = new List<InventoryTransferRequest>();

    public virtual User? ManagerUser { get; set; }

    public virtual Project? Project { get; set; }

    public virtual Region Region { get; set; } = null!;

    public virtual ICollection<StoreInventory> StoreInventories { get; set; } = new List<StoreInventory>();

    public virtual ICollection<StoreTransaction> StoreTransactionRelatedStores { get; set; } = new List<StoreTransaction>();

    public virtual ICollection<StoreTransaction> StoreTransactionStores { get; set; } = new List<StoreTransaction>();

    public virtual User? User { get; set; }
}
