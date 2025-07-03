using System;

namespace Domain.Common
{
    /// <summary>
    ///     Adds common audit fields to a domain entity.
    /// </summary>
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