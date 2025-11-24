using System;
using System.Collections.Generic;

namespace API.Models;

public partial class RolePermission
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    public DateTime GrantedAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
