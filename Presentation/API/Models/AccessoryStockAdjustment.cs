using System;
using System.Collections.Generic;

namespace API.Models;

public partial class AccessoryStockAdjustment
{
    public int Id { get; set; }

    public int AccessoryId { get; set; }

    public string AdjustmentType { get; set; } = null!;

    public int QuantityChanged { get; set; }

    public int PreviousQuantity { get; set; }

    public int NewQuantity { get; set; }

    public string Reason { get; set; } = null!;

    public string? Notes { get; set; }

    public string AdjustedBy { get; set; } = null!;

    public DateTime AdjustedDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Accessory Accessory { get; set; } = null!;
}
