using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Machine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string Manufacturer { get; set; } = null!;

    public string SerialNumber { get; set; } = null!;

    public string? RigNo { get; set; }

    public string? PlateNo { get; set; }

    public string? ChassisDetails { get; set; }

    public int? ManufacturingYear { get; set; }

    public int Status { get; set; }

    public string? AssignedToProject { get; set; }

    public string? AssignedToOperator { get; set; }

    public DateTime? LastMaintenanceDate { get; set; }

    public DateTime? NextMaintenanceDate { get; set; }

    public string? SpecificationsJson { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? ProjectId { get; set; }

    public int? OperatorId { get; set; }

    public int? RegionId { get; set; }

    public string? CurrentLocation { get; set; }

    public string? Location { get; set; }

    public bool IsActive { get; set; }

    public decimal? CurrentDrifterServiceHours { get; set; }

    public decimal CurrentEngineServiceHours { get; set; }

    public decimal? DrifterServiceInterval { get; set; }

    public decimal EngineServiceInterval { get; set; }

    public DateTime? LastDrifterServiceDate { get; set; }

    public DateTime? LastEngineServiceDate { get; set; }

    public virtual ICollection<MachineAssignment> MachineAssignments { get; set; } = new List<MachineAssignment>();

    public virtual ICollection<MachineUsageLog> MachineUsageLogs { get; set; } = new List<MachineUsageLog>();

    public virtual ICollection<MaintenanceJob> MaintenanceJobs { get; set; } = new List<MaintenanceJob>();

    public virtual ICollection<MaintenanceReport> MaintenanceReports { get; set; } = new List<MaintenanceReport>();

    public virtual User? Operator { get; set; }

    public virtual Project? Project { get; set; }

    public virtual Region? Region { get; set; }
}
