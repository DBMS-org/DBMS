using System;
using Domain.Common;
using Domain.Entities.UserManagement;

namespace Domain.Entities.Notifications
{
    /// <summary>
    /// Represents a notification sent to a user
    /// Supports various notification types across all system workflows
    /// </summary>
    public class Notification : BaseAuditableEntity
    {
        /// <summary>
        /// User who receives this notification
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// Type of notification (for categorization and filtering)
        /// </summary>
        public NotificationType Type { get; private set; }

        /// <summary>
        /// Notification title (brief summary)
        /// </summary>
        public string Title { get; private set; } = string.Empty;

        /// <summary>
        /// Detailed notification message
        /// </summary>
        public string Message { get; private set; } = string.Empty;

        /// <summary>
        /// Priority level of the notification
        /// </summary>
        public NotificationPriority Priority { get; private set; } = NotificationPriority.Normal;

        /// <summary>
        /// Whether the notification has been read
        /// </summary>
        public bool IsRead { get; private set; } = false;

        /// <summary>
        /// Timestamp when the notification was read
        /// </summary>
        public DateTime? ReadAt { get; private set; }

        /// <summary>
        /// Type of the related entity (e.g., "ExplosiveApprovalRequest", "MaintenanceReport")
        /// Optional - used for linking notifications to specific entities
        /// </summary>
        public string? RelatedEntityType { get; private set; }

        /// <summary>
        /// ID of the related entity
        /// Optional - used for linking notifications to specific entities
        /// </summary>
        public int? RelatedEntityId { get; private set; }

        /// <summary>
        /// Action URL for navigation (e.g., "/store-manager/requests/123")
        /// Optional - allows direct navigation to related content
        /// </summary>
        public string? ActionUrl { get; private set; }

        /// <summary>
        /// Additional metadata stored as JSON
        /// Optional - for extensibility without schema changes
        /// </summary>
        public string? AdditionalData { get; private set; }

        // Navigation properties
        public virtual User User { get; private set; } = null!;

        // Private constructor for EF Core
        private Notification() { }

        /// <summary>
        /// Creates a new notification
        /// </summary>
        /// <param name="userId">ID of the user to notify</param>
        /// <param name="type">Type of notification</param>
        /// <param name="title">Brief title/summary</param>
        /// <param name="message">Detailed message</param>
        /// <param name="priority">Priority level</param>
        /// <param name="relatedEntityType">Optional: type of related entity</param>
        /// <param name="relatedEntityId">Optional: ID of related entity</param>
        /// <param name="actionUrl">Optional: URL for navigation</param>
        /// <param name="additionalData">Optional: JSON metadata</param>
        /// <returns>New notification instance</returns>
        public static Notification Create(
            int userId,
            NotificationType type,
            string title,
            string message,
            NotificationPriority priority = NotificationPriority.Normal,
            string? relatedEntityType = null,
            int? relatedEntityId = null,
            string? actionUrl = null,
            string? additionalData = null)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than zero", nameof(userId));

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required", nameof(title));

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message is required", nameof(message));

            if (title.Length > 200)
                throw new ArgumentException("Title cannot exceed 200 characters", nameof(title));

            if (message.Length > 1000)
                throw new ArgumentException("Message cannot exceed 1000 characters", nameof(message));

            return new Notification
            {
                UserId = userId,
                Type = type,
                Title = title,
                Message = message,
                Priority = priority,
                RelatedEntityType = relatedEntityType,
                RelatedEntityId = relatedEntityId,
                ActionUrl = actionUrl,
                AdditionalData = additionalData,
                IsRead = false
            };
        }

        /// <summary>
        /// Marks the notification as read
        /// </summary>
        public void MarkAsRead()
        {
            if (!IsRead)
            {
                IsRead = true;
                ReadAt = DateTime.UtcNow;
                MarkUpdated();
            }
        }

        /// <summary>
        /// Marks the notification as unread
        /// </summary>
        public void MarkAsUnread()
        {
            if (IsRead)
            {
                IsRead = false;
                ReadAt = null;
                MarkUpdated();
            }
        }

        /// <summary>
        /// Updates the notification message (for editable notifications)
        /// </summary>
        public void UpdateMessage(string newMessage)
        {
            if (string.IsNullOrWhiteSpace(newMessage))
                throw new ArgumentException("Message cannot be empty", nameof(newMessage));

            if (newMessage.Length > 1000)
                throw new ArgumentException("Message cannot exceed 1000 characters", nameof(newMessage));

            Message = newMessage;
            MarkUpdated();
        }

        /// <summary>
        /// Checks if the notification is older than the specified number of days
        /// </summary>
        public bool IsOlderThan(int days)
        {
            return CreatedAt < DateTime.UtcNow.AddDays(-days);
        }

        /// <summary>
        /// Gets a display-friendly age string (e.g., "2 hours ago", "3 days ago")
        /// </summary>
        public string GetAgeDisplay()
        {
            var timeSpan = DateTime.UtcNow - CreatedAt;

            if (timeSpan.TotalMinutes < 1)
                return "Just now";

            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} minute{((int)timeSpan.TotalMinutes == 1 ? "" : "s")} ago";

            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} hour{((int)timeSpan.TotalHours == 1 ? "" : "s")} ago";

            if (timeSpan.TotalDays < 30)
                return $"{(int)timeSpan.TotalDays} day{((int)timeSpan.TotalDays == 1 ? "" : "s")} ago";

            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)} month{((int)(timeSpan.TotalDays / 30) == 1 ? "" : "s")} ago";

            return $"{(int)(timeSpan.TotalDays / 365)} year{((int)(timeSpan.TotalDays / 365) == 1 ? "" : "s")} ago";
        }
    }
}
