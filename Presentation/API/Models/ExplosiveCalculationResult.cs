using System;
using System.Collections.Generic;

namespace API.Models;

public partial class ExplosiveCalculationResult
{
    public int Id { get; set; }

    public string CalculationId { get; set; } = null!;

    public int? PatternSettingsId { get; set; }

    public float EmulsionDensity { get; set; }

    public float AnfoDensity { get; set; }

    public float EmulsionPerHole { get; set; }

    public float TotalDepth { get; set; }

    public float AverageDepth { get; set; }

    public int NumberOfFilledHoles { get; set; }

    public float EmulsionPerMeter { get; set; }

    public float AnfoPerMeter { get; set; }

    public float EmulsionCoveringSpace { get; set; }

    public float RemainingSpace { get; set; }

    public float AnfoCoveringSpace { get; set; }

    public float TotalAnfo { get; set; }

    public float TotalEmulsion { get; set; }

    public float TotalVolume { get; set; }

    public int ProjectId { get; set; }

    public int SiteId { get; set; }

    public int OwningUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual User OwningUser { get; set; } = null!;

    public virtual PatternSetting? PatternSettings { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ProjectSite Site { get; set; } = null!;
}
