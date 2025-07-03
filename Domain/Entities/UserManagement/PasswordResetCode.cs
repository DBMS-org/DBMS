namespace Domain.Entities.UserManagement
{
    public class PasswordResetCode
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public string Email { get; set; } = string.Empty;
        
        public string Code { get; set; } = string.Empty;
        
        public DateTime ExpiresAt { get; set; }
        
        public bool IsUsed { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UsedAt { get; set; }
        
        public int AttemptCount { get; set; } = 0;
        
        // Navigation property
        public virtual User User { get; set; } = null!;
    }
} 
