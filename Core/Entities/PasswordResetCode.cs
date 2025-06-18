using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class PasswordResetCode
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(6)]
        public string Code { get; set; } = string.Empty;
        
        [Required]
        public DateTime ExpiresAt { get; set; }
        
        public bool IsUsed { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UsedAt { get; set; }
        
        public int AttemptCount { get; set; } = 0;
        
        // Navigation property
        public virtual User User { get; set; } = null!;
    }
} 