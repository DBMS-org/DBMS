using System;
using System.Collections.Generic;

namespace API.Models;

public partial class MaintenanceJobAssignment
{
    public int Id { get; set; }

    public int MaintenanceJobId { get; set; }

    public int MechanicalEngineerId { get; set; }

    public DateTime AssignedAt { get; set; }

    public virtual MaintenanceJob MaintenanceJob { get; set; } = null!;

    public virtual User MechanicalEngineer { get; set; } = null!;
}
