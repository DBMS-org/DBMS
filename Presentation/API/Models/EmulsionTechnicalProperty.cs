using System;
using System.Collections.Generic;

namespace API.Models;

public partial class EmulsionTechnicalProperty
{
    public int Id { get; set; }

    public int CentralWarehouseInventoryId { get; set; }

    public decimal DensityUnsensitized { get; set; }

    public decimal DensitySensitized { get; set; }

    public int Viscosity { get; set; }

    public decimal WaterContent { get; set; }

    public decimal PH { get; set; }

    public int? DetonationVelocity { get; set; }

    public int? BubbleSize { get; set; }

    public decimal StorageTemperature { get; set; }

    public decimal? ApplicationTemperature { get; set; }

    public int Grade { get; set; }

    public string Color { get; set; } = null!;

    public int SensitizationType { get; set; }

    public decimal? SensitizerContent { get; set; }

    public int FumeClass { get; set; }

    public DateTime? QualityCheckDate { get; set; }

    public int QualityStatus { get; set; }

    public bool? PhaseSeparation { get; set; }

    public bool? Crystallization { get; set; }

    public bool? ColorConsistency { get; set; }

    public string WaterResistance { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual CentralWarehouseInventory CentralWarehouseInventory { get; set; } = null!;
}
