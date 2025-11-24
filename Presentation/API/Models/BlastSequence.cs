using System;
using System.Collections.Generic;

namespace API.Models;

public partial class BlastSequence
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int SiteId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double DelayBetweenHoles { get; set; }

    public double DelayBetweenRows { get; set; }

    public string SimulationSettingsJson { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int CreatedByUserId { get; set; }

    public int? DrillPatternId { get; set; }

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual DrillPattern? DrillPattern { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ProjectSite Site { get; set; } = null!;
}
