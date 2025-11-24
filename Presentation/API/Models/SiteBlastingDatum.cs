using System;
using System.Collections.Generic;

namespace API.Models;

public partial class SiteBlastingDatum
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int SiteId { get; set; }

    public string DataType { get; set; } = null!;

    public string JsonData { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int CreatedByUserId { get; set; }

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;

    public virtual ProjectSite Site { get; set; } = null!;
}
