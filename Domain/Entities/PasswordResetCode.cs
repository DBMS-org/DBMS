namespace Domain.Entities
{
    public class PasswordResetCode : BaseEntity
    {
        public int UserId { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        
        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; }
        public int AttemptCount { get; set; } = 0;
        
        // Navigation property
        public virtual User User { get; set; } = null!;
        
        // Business logic methods
        public bool IsExpired() => DateTime.UtcNow > ExpiresAt;
        
        public bool IsValid() => !IsUsed && !IsExpired();
        
        public void MarkAsUsed()
        {
            IsUsed = true;
            UsedAt = DateTime.UtcNow;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public void IncrementAttempt()
        {
            AttemptCount++;
            UpdateTimestamp();  // ← Calling BaseEntity method
        }
        
        public bool HasMaxAttemptsExceeded(int maxAttempts = 3)
        {
            return AttemptCount >= maxAttempts;
        }
        
        public TimeSpan GetTimeUntilExpiry()
        {
            return ExpiresAt - DateTime.UtcNow;
        }
    }
} 
