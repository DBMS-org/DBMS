using System;
using System.Collections.Generic;

namespace API.Models;

public partial class BlastConnection
{
    public string Id { get; set; } = null!;

    public int ProjectId { get; set; }

    public int SiteId { get; set; }

    public int ConnectorType { get; set; }

    public int Delay { get; set; }

    public int Sequence { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string Point1DrillPointId { get; set; } = null!;

    public string Point2DrillPointId { get; set; } = null!;

    public bool IsStartingHole { get; set; }

    public virtual DrillPoint DrillPoint { get; set; } = null!;

    public virtual DrillPoint DrillPointNavigation { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;

    public virtual ProjectSite Site { get; set; } = null!;
}
