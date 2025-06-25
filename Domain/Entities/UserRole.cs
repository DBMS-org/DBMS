

namespace Domain.Entities
{
    public class UserRole
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
        
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 
