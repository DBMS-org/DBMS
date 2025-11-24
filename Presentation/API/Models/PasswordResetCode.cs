using System;
using System.Collections.Generic;

namespace API.Models;

public partial class PasswordResetCode
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Code { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UsedAt { get; set; }

    public int AttemptCount { get; set; }

    public string Email { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
