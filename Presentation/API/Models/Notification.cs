using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Notification
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int Type { get; set; }

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public int Priority { get; set; }

    public bool IsRead { get; set; }

    public DateTime? ReadAt { get; set; }

    public string? RelatedEntityType { get; set; }

    public int? RelatedEntityId { get; set; }

    public string? ActionUrl { get; set; }

    public string? AdditionalData { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual User User { get; set; } = null!;
}
