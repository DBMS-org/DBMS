namespace Domain.Entities.Notifications
{
    /// <summary>
    /// Priority levels for notifications
    /// Determines urgency and how prominently notifications are displayed
    /// </summary>
    public enum NotificationPriority
    {
        /// <summary>
        /// Low priority - informational only, no immediate action needed
        /// </summary>
        Low = 0,

        /// <summary>
        /// Normal priority - standard notification
        /// </summary>
        Normal = 1,

        /// <summary>
        /// High priority - requires attention soon
        /// </summary>
        High = 2,

        /// <summary>
        /// Urgent priority - requires immediate attention
        /// </summary>
        Urgent = 3,

        /// <summary>
        /// Critical priority - critical system or safety issue
        /// </summary>
        Critical = 4
    }
}
