using Domain.Entities.Notifications;

namespace Application.Interfaces
{
    /// <summary>
    /// Repository interface for managing notifications
    /// Provides CRUD operations and specialized notification queries
    /// </summary>
    public interface INotificationRepository
    {
        // ===== CREATE =====

        /// <summary>
        /// Creates a new notification
        /// </summary>
        /// <param name="notification">Notification to create</param>
        /// <returns>Created notification with assigned ID</returns>
        Task<Notification> CreateAsync(Notification notification);

        /// <summary>
        /// Creates multiple notifications in a single transaction (bulk insert)
        /// Useful for notifying multiple users about the same event
        /// </summary>
        /// <param name="notifications">Collection of notifications to create</param>
        /// <returns>Created notifications with assigned IDs</returns>
        Task<IEnumerable<Notification>> CreateBulkAsync(IEnumerable<Notification> notifications);

        // ===== READ =====

        /// <summary>
        /// Gets a notification by its ID
        /// </summary>
        /// <param name="id">Notification ID</param>
        /// <returns>Notification if found, null otherwise</returns>
        Task<Notification?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all notifications for a specific user
        /// Ordered by creation date (newest first)
        /// Supports pagination
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="skip">Number of records to skip (for pagination)</param>
        /// <param name="take">Number of records to take (page size)</param>
        /// <returns>Collection of notifications</returns>
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, int skip = 0, int take = 50);

        /// <summary>
        /// Gets all unread notifications for a specific user
        /// Ordered by creation date (newest first)
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Collection of unread notifications</returns>
        Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(int userId);

        /// <summary>
        /// Gets the count of unread notifications for a user
        /// Used for displaying notification badges
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Number of unread notifications</returns>
        Task<int> GetUnreadCountAsync(int userId);

        /// <summary>
        /// Gets notifications by type for a specific user
        /// Useful for filtering notifications by category
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="type">Notification type</param>
        /// <returns>Collection of notifications of the specified type</returns>
        Task<IEnumerable<Notification>> GetByTypeAsync(int userId, NotificationType type);

        /// <summary>
        /// Gets notifications by priority for a specific user
        /// Useful for filtering high-priority or urgent notifications
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="priority">Notification priority</param>
        /// <returns>Collection of notifications with the specified priority</returns>
        Task<IEnumerable<Notification>> GetByPriorityAsync(int userId, NotificationPriority priority);

        /// <summary>
        /// Gets notifications related to a specific entity
        /// Useful for finding all notifications about a specific request, job, etc.
        /// </summary>
        /// <param name="entityType">Type of the related entity (e.g., "ExplosiveApprovalRequest")</param>
        /// <param name="entityId">ID of the related entity</param>
        /// <returns>Collection of related notifications</returns>
        Task<IEnumerable<Notification>> GetByRelatedEntityAsync(string entityType, int entityId);

        // ===== UPDATE =====

        /// <summary>
        /// Marks a notification as read
        /// </summary>
        /// <param name="notificationId">Notification ID</param>
        /// <returns>True if successful, false if notification not found</returns>
        Task<bool> MarkAsReadAsync(int notificationId);

        /// <summary>
        /// Marks a notification as unread
        /// </summary>
        /// <param name="notificationId">Notification ID</param>
        /// <returns>True if successful, false if notification not found</returns>
        Task<bool> MarkAsUnreadAsync(int notificationId);

        /// <summary>
        /// Marks all notifications for a user as read
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>True if successful</returns>
        Task<bool> MarkAllAsReadAsync(int userId);

        // ===== DELETE =====

        /// <summary>
        /// Deletes a specific notification
        /// </summary>
        /// <param name="notificationId">Notification ID</param>
        /// <returns>True if successful, false if notification not found</returns>
        Task<bool> DeleteAsync(int notificationId);

        /// <summary>
        /// Deletes old notifications for a user
        /// Helps maintain database performance by removing old data
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="daysOld">Age threshold in days (default: 30)</param>
        /// <returns>True if successful</returns>
        Task<bool> DeleteOldNotificationsAsync(int userId, int daysOld = 30);

        /// <summary>
        /// Deletes all notifications for a user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>True if successful</returns>
        Task<bool> DeleteAllAsync(int userId);
    }
}
