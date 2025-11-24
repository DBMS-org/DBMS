using System;
using System.Collections.Generic;

namespace API.Models;

public partial class QualityCheckRecord
{
    public int Id { get; set; }

    public int CentralWarehouseInventoryId { get; set; }

    public DateTime CheckDate { get; set; }

    public int CheckedByUserId { get; set; }

    public int Status { get; set; }

    public string CheckType { get; set; } = null!;

    public string? Findings { get; set; }

    public string? ActionTaken { get; set; }

    public bool RequiresFollowUp { get; set; }

    public DateTime? FollowUpDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual CentralWarehouseInventory CentralWarehouseInventory { get; set; } = null!;

    public virtual User CheckedByUser { get; set; } = null!;
}
