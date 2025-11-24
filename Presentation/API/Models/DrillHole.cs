using System;
using System.Collections.Generic;

namespace API.Models;

public partial class DrillHole
{
    public string Id { get; set; } = null!;

    public int? SerialNumber { get; set; }

    public string Name { get; set; } = null!;

    public decimal Easting { get; set; }

    public decimal Northing { get; set; }

    public decimal Elevation { get; set; }

    public decimal Length { get; set; }

    public decimal Depth { get; set; }

    public double? Azimuth { get; set; }

    public double? Dip { get; set; }

    public double ActualDepth { get; set; }

    public double Stemming { get; set; }

    public int ProjectId { get; set; }

    public int SiteId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
