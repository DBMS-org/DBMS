using System;
using System.Collections.Generic;

namespace API.Models;

public partial class CentralWarehouseInventory
{
    public int Id { get; set; }

    public string BatchId { get; set; } = null!;

    public int ExplosiveType { get; set; }

    public decimal Quantity { get; set; }

    public decimal AllocatedQuantity { get; set; }

    public string Unit { get; set; } = null!;

    public DateTime ManufacturingDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public string Supplier { get; set; } = null!;

    public string? ManufacturerBatchNumber { get; set; }

    public string StorageLocation { get; set; } = null!;

    public int Status { get; set; }

    public int CentralWarehouseStoreId { get; set; }

    public int? AnfotechnicalPropertiesId { get; set; }

    public int? EmulsionTechnicalPropertiesId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual AnfotechnicalProperty? AnfotechnicalProperty { get; set; }

    public virtual Store CentralWarehouseStore { get; set; } = null!;

    public virtual EmulsionTechnicalProperty? EmulsionTechnicalProperty { get; set; }

    public virtual ICollection<InventoryTransferRequest> InventoryTransferRequests { get; set; } = new List<InventoryTransferRequest>();

    public virtual ICollection<QualityCheckRecord> QualityCheckRecords { get; set; } = new List<QualityCheckRecord>();
}
