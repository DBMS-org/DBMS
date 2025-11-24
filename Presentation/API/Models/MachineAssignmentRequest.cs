using System;
using System.Collections.Generic;

namespace API.Models;

public partial class MachineAssignmentRequest
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public string MachineType { get; set; } = null!;

    public int Quantity { get; set; }

    public string RequestedBy { get; set; } = null!;

    public DateTime RequestedDate { get; set; }

    public string Status { get; set; } = null!;

    public string Urgency { get; set; } = null!;

    public string? DetailsOrExplanation { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string? AssignedMachinesJson { get; set; }

    public string? Comments { get; set; }

    public string? ExpectedUsageDuration { get; set; }

    public DateTime? ExpectedReturnDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Project Project { get; set; } = null!;
}
