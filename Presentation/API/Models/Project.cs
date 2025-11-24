using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? AssignedUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? RegionId { get; set; }

    public string Region { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual User? AssignedUser { get; set; }

    public virtual ICollection<BlastConnection> BlastConnections { get; set; } = new List<BlastConnection>();

    public virtual ICollection<BlastSequence> BlastSequences { get; set; } = new List<BlastSequence>();

    public virtual ICollection<DetonatorInfo> DetonatorInfos { get; set; } = new List<DetonatorInfo>();

    public virtual ICollection<DrillPattern> DrillPatterns { get; set; } = new List<DrillPattern>();

    public virtual ICollection<DrillPoint> DrillPoints { get; set; } = new List<DrillPoint>();

    public virtual ICollection<ExplosiveCalculationResult> ExplosiveCalculationResults { get; set; } = new List<ExplosiveCalculationResult>();

    public virtual ICollection<MachineAssignmentRequest> MachineAssignmentRequests { get; set; } = new List<MachineAssignmentRequest>();

    public virtual ICollection<MachineAssignment> MachineAssignments { get; set; } = new List<MachineAssignment>();

    public virtual ICollection<Machine> Machines { get; set; } = new List<Machine>();

    public virtual ICollection<MaintenanceJob> MaintenanceJobs { get; set; } = new List<MaintenanceJob>();

    public virtual ICollection<PatternSetting> PatternSettings { get; set; } = new List<PatternSetting>();

    public virtual ICollection<ProjectSite> ProjectSites { get; set; } = new List<ProjectSite>();

    public virtual Region? RegionNavigation { get; set; }

    public virtual ICollection<SiteBlastingDatum> SiteBlastingData { get; set; } = new List<SiteBlastingDatum>();

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
