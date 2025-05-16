using System.Collections.Generic;

namespace CleanArchitecture.Domain.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
} 