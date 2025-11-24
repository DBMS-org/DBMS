using System;
using System.Collections.Generic;

namespace API.Models;

public partial class ProjectSite
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Coordinates { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsPatternApproved { get; set; }

    public bool IsSimulationConfirmed { get; set; }

    public bool IsOperatorCompleted { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int? CompletedByUserId { get; set; }

    public bool IsCompleted { get; set; }

    public virtual ICollection<BlastConnection> BlastConnections { get; set; } = new List<BlastConnection>();

    public virtual ICollection<BlastSequence> BlastSequences { get; set; } = new List<BlastSequence>();

    public virtual User? CompletedByUser { get; set; }

    public virtual ICollection<DetonatorInfo> DetonatorInfos { get; set; } = new List<DetonatorInfo>();

    public virtual ICollection<DrillPattern> DrillPatterns { get; set; } = new List<DrillPattern>();

    public virtual ICollection<DrillPoint> DrillPoints { get; set; } = new List<DrillPoint>();

    public virtual ICollection<ExplosiveApprovalRequest> ExplosiveApprovalRequests { get; set; } = new List<ExplosiveApprovalRequest>();

    public virtual ICollection<ExplosiveCalculationResult> ExplosiveCalculationResults { get; set; } = new List<ExplosiveCalculationResult>();

    public virtual ICollection<PatternSetting> PatternSettings { get; set; } = new List<PatternSetting>();

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<SiteBlastingDatum> SiteBlastingData { get; set; } = new List<SiteBlastingDatum>();
}
