using System;
using System.Collections.Generic;

namespace API.Models;

public partial class ExplosiveApprovalRequest
{
    public int Id { get; set; }

    public int ProjectSiteId { get; set; }

    public int RequestedByUserId { get; set; }

    public DateTime ExpectedUsageDate { get; set; }

    public string? Comments { get; set; }

    public int Status { get; set; }

    public int Priority { get; set; }

    public int ApprovalType { get; set; }

    public int? ProcessedByUserId { get; set; }

    public DateTime? ProcessedAt { get; set; }

    public string? RejectionReason { get; set; }

    public string? AdditionalData { get; set; }

    public decimal? EstimatedDurationHours { get; set; }

    public bool SafetyChecklistCompleted { get; set; }

    public bool EnvironmentalAssessmentCompleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public string? BlastTiming { get; set; }

    public DateTime? BlastingDate { get; set; }

    public virtual User? ProcessedByUser { get; set; }

    public virtual ProjectSite ProjectSite { get; set; } = null!;

    public virtual User RequestedByUser { get; set; } = null!;
}
