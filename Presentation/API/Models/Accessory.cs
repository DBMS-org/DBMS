using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Accessory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string PartNumber { get; set; } = null!;

    public string? Description { get; set; }

    public int Quantity { get; set; }

    public string Unit { get; set; } = null!;

    public int MinStockLevel { get; set; }

    public string Supplier { get; set; } = null!;

    public string? Location { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<AccessoryStockAdjustment> AccessoryStockAdjustments { get; set; } = new List<AccessoryStockAdjustment>();
}
