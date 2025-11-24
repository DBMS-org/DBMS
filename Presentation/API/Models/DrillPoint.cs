using System;
using System.Collections.Generic;

namespace API.Models;

public partial class DrillPoint
{
    public string Id { get; set; } = null!;

    public int ProjectId { get; set; }

    public int SiteId { get; set; }

    public float X { get; set; }

    public float Y { get; set; }

    public float Depth { get; set; }

    public float Spacing { get; set; }

    public float Burden { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public float Diameter { get; set; }

    public float Stemming { get; set; }

    public float Anfo { get; set; }

    public float Emulsion { get; set; }

    public float Subdrill { get; set; }

    public float Volume { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int? CompletedByUserId { get; set; }

    public bool IsCompleted { get; set; }

    public virtual ICollection<BlastConnection> BlastConnectionDrillPointNavigations { get; set; } = new List<BlastConnection>();

    public virtual ICollection<BlastConnection> BlastConnectionDrillPoints { get; set; } = new List<BlastConnection>();

    public virtual ICollection<DetonatorInfo> DetonatorInfos { get; set; } = new List<DetonatorInfo>();

    public virtual Project Project { get; set; } = null!;

    public virtual ProjectSite Site { get; set; } = null!;
}
