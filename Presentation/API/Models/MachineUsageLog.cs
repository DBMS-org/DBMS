using System;
using System.Collections.Generic;

namespace API.Models;

public partial class MachineUsageLog
{
    public int Id { get; set; }

    public int MachineId { get; set; }

    public int? OperatorId { get; set; }

    public string SiteEngineer { get; set; } = null!;

    public DateTime LogDate { get; set; }

    public decimal EngineHourStart { get; set; }

    public decimal EngineHourEnd { get; set; }

    public decimal EngineHoursDelta { get; set; }

    public decimal? DrifterHourStart { get; set; }

    public decimal? DrifterHourEnd { get; set; }

    public decimal? DrifterHoursDelta { get; set; }

    public decimal IdleHours { get; set; }

    public decimal WorkingHours { get; set; }

    public decimal? FuelConsumed { get; set; }

    public bool HasDowntime { get; set; }

    public decimal? DowntimeHours { get; set; }

    public string? BreakdownDescription { get; set; }

    public string? Remarks { get; set; }

    public string Status { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Machine Machine { get; set; } = null!;

    public virtual User? Operator { get; set; }
}
