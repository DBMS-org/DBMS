using System;
using System.Collections.Generic;

namespace API.Models;

public partial class PatternSetting
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ProjectId { get; set; }

    public int SiteId { get; set; }

    public float Spacing { get; set; }

    public float Burden { get; set; }

    public float Depth { get; set; }

    public float Diameter { get; set; }

    public float Stemming { get; set; }

    public virtual ICollection<ExplosiveCalculationResult> ExplosiveCalculationResults { get; set; } = new List<ExplosiveCalculationResult>();

    public virtual Project Project { get; set; } = null!;

    public virtual ProjectSite Site { get; set; } = null!;
}
