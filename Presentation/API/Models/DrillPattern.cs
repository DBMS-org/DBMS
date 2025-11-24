using System;
using System.Collections.Generic;

namespace API.Models;

public partial class DrillPattern
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int SiteId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double Spacing { get; set; }

    public double Burden { get; set; }

    public double Depth { get; set; }

    public string DrillPointsJson { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int CreatedByUserId { get; set; }

    public int CreatedById { get; set; }

    public virtual ICollection<BlastSequence> BlastSequences { get; set; } = new List<BlastSequence>();

    public virtual User CreatedBy { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;

    public virtual ProjectSite Site { get; set; } = null!;
}
