using System;

namespace Domain.Common
{
    // Base entity with audit tracking
    public abstract class BaseAuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public void MarkUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            MarkUpdated();
        }

        public void Activate()
        {
            IsActive = true;
            MarkUpdated();
        }
    }
} 