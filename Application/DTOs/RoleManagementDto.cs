using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class AssignRoleRequest
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MinLength(1, ErrorMessage = "At least one role must be assigned")]
        public List<int> RoleIds { get; set; } = new List<int>();
    }
    
    public class RevokeRoleRequest
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MinLength(1, ErrorMessage = "At least one role must be specified")]
        public List<int> RoleIds { get; set; } = new List<int>();
    }
    
    public class UserRoleAssignmentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string RoleDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
    }
    
    public class UpdateUserRolesRequest
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MinLength(1, ErrorMessage = "At least one role must be assigned")]
        public List<int> RoleIds { get; set; } = new List<int>();
        
        // Optional: specify if old roles should be revoked or kept
        public bool RevokeExistingRoles { get; set; } = true;
    }
} 