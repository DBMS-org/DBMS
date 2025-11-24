using System;
using System.Collections.Generic;

namespace API.Models;

public partial class AnfotechnicalProperty
{
    public int Id { get; set; }

    public int CentralWarehouseInventoryId { get; set; }

    public decimal Density { get; set; }

    public decimal FuelOilContent { get; set; }

    public decimal? MoistureContent { get; set; }

    public decimal? PrillSize { get; set; }

    public int? DetonationVelocity { get; set; }

    public int Grade { get; set; }

    public decimal StorageTemperature { get; set; }

    public decimal StorageHumidity { get; set; }

    public int FumeClass { get; set; }

    public DateTime? QualityCheckDate { get; set; }

    public int QualityStatus { get; set; }

    public string WaterResistance { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual CentralWarehouseInventory CentralWarehouseInventory { get; set; } = null!;
}
