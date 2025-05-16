using System.Collections.Generic;

namespace CleanArchitecture.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
} 