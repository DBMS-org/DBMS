using System;
using System.Collections.Generic;

namespace API.Models;

public partial class MachineAssignment
{
    public int Id { get; set; }

    public int MachineId { get; set; }

    public int ProjectId { get; set; }

    public int OperatorId { get; set; }

    public string AssignedBy { get; set; } = null!;

    public DateTime AssignedDate { get; set; }

    public DateTime? ExpectedReturnDate { get; set; }

    public DateTime? ActualReturnDate { get; set; }

    public string Status { get; set; } = null!;

    public string? Location { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Machine Machine { get; set; } = null!;

    public virtual User Operator { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
