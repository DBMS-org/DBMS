using System;
using System.Collections.Generic;

namespace API.Models;

public partial class MaintenanceReport
{
    public int Id { get; set; }

    public string TicketId { get; set; } = null!;

    public int OperatorId { get; set; }

    public int MachineId { get; set; }

    public string MachineName { get; set; } = null!;

    public string? MachineModel { get; set; }

    public string? SerialNumber { get; set; }

    public string? Location { get; set; }

    public string AffectedPart { get; set; } = null!;

    public string ProblemCategory { get; set; } = null!;

    public string CustomDescription { get; set; } = null!;

    public string? Symptoms { get; set; }

    public string? ErrorCodes { get; set; }

    public string? RecentMaintenanceHistory { get; set; }

    public string Severity { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime ReportedAt { get; set; }

    public DateTime? AcknowledgedAt { get; set; }

    public DateTime? InProgressAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public int? MechanicalEngineerId { get; set; }

    public string? MechanicalEngineerName { get; set; }

    public string? ResolutionNotes { get; set; }

    public string? EstimatedResponseTime { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Machine Machine { get; set; } = null!;

    public virtual MaintenanceJob? MaintenanceJob { get; set; }

    public virtual User? MechanicalEngineer { get; set; }

    public virtual User Operator { get; set; } = null!;
}
