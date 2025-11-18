using Application.Interfaces;
using Domain.Entities.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers
{
    /// <summary>
    /// API controller for managing user notifications
    /// Provides endpoints for fetching, marking, and deleting notifications
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(
            INotificationRepository notificationRepository,
            ILogger<NotificationsController> logger)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the current user's ID from JWT claims
        /// </summary>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub")
                ?? User.FindFirst("id")
                ?? User.FindFirst("userId");

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            return userId;
        }

        /// <summary>
        /// GET /api/notifications
        /// Gets all notifications for the current user with pagination
        /// </summary>
        /// <param name="skip">Number of records to skip (default: 0)</param>
        /// <param name="take">Number of records to take (default: 50, max: 100)</param>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetNotifications([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            try
            {
                var userId = GetCurrentUserId();

                // Validate pagination parameters
                if (skip < 0) skip = 0;
                if (take < 1) take = 50;
                if (take > 100) take = 100;

                var notifications = await _notificationRepository.GetByUserIdAsync(userId, skip, take);

                _logger.LogInformation("User {UserId} retrieved {Count} notifications (skip: {Skip}, take: {Take})",
                    userId, notifications.Count(), skip, take);

                return Ok(notifications);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to get notifications");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notifications");
                return StatusCode(500, new { message = "An error occurred while retrieving notifications" });
            }
        }

        /// <summary>
        /// GET /api/notifications/unread
        /// Gets all unread notifications for the current user
        /// </summary>
        [HttpGet("unread")]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            try
            {
                var userId = GetCurrentUserId();
                var notifications = await _notificationRepository.GetUnreadByUserIdAsync(userId);

                _logger.LogInformation("User {UserId} retrieved {Count} unread notifications",
                    userId, notifications.Count());

                return Ok(notifications);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to get unread notifications");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unread notifications");
                return StatusCode(500, new { message = "An error occurred while retrieving unread notifications" });
            }
        }

        /// <summary>
        /// GET /api/notifications/unread-count
        /// Gets the count of unread notifications for the current user
        /// Used for badge display in UI
        /// </summary>
        [HttpGet("unread-count")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUnreadCount()
        {
            try
            {
                var userId = GetCurrentUserId();
                var count = await _notificationRepository.GetUnreadCountAsync(userId);

                _logger.LogDebug("User {UserId} has {Count} unread notifications", userId, count);

                return Ok(new { count });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to get unread count");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unread notification count");
                return StatusCode(500, new { message = "An error occurred while retrieving unread count" });
            }
        }

        /// <summary>
        /// GET /api/notifications/{id}
        /// Gets a specific notification by ID
        /// </summary>
        /// <param name="id">Notification ID</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var notification = await _notificationRepository.GetByIdAsync(id);

                if (notification == null)
                {
                    _logger.LogWarning("Notification {NotificationId} not found", id);
                    return NotFound(new { message = "Notification not found" });
                }

                // Verify ownership
                if (notification.UserId != userId)
                {
                    _logger.LogWarning("User {UserId} attempted to access notification {NotificationId} belonging to user {OwnerId}",
                        userId, id, notification.UserId);
                    return Forbid();
                }

                return Ok(notification);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to get notification {NotificationId}", id);
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notification {NotificationId}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the notification" });
            }
        }

        /// <summary>
        /// GET /api/notifications/type/{type}
        /// Gets notifications by type for the current user
        /// </summary>
        /// <param name="type">Notification type</param>
        [HttpGet("type/{type}")]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetNotificationsByType(NotificationType type)
        {
            try
            {
                var userId = GetCurrentUserId();
                var notifications = await _notificationRepository.GetByTypeAsync(userId, type);

                _logger.LogInformation("User {UserId} retrieved {Count} notifications of type {Type}",
                    userId, notifications.Count(), type);

                return Ok(notifications);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to get notifications by type");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notifications by type {Type}", type);
                return StatusCode(500, new { message = "An error occurred while retrieving notifications" });
            }
        }

        /// <summary>
        /// PUT /api/notifications/{id}/read
        /// Marks a notification as read
        /// </summary>
        /// <param name="id">Notification ID</param>
        [HttpPut("{id}/read")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                var userId = GetCurrentUserId();

                // Verify ownership before marking as read
                var notification = await _notificationRepository.GetByIdAsync(id);
                if (notification == null)
                {
                    _logger.LogWarning("Notification {NotificationId} not found", id);
                    return NotFound(new { message = "Notification not found" });
                }

                if (notification.UserId != userId)
                {
                    _logger.LogWarning("User {UserId} attempted to mark notification {NotificationId} as read (belongs to user {OwnerId})",
                        userId, id, notification.UserId);
                    return Forbid();
                }

                var success = await _notificationRepository.MarkAsReadAsync(id);
                if (!success)
                {
                    return NotFound(new { message = "Notification not found" });
                }

                _logger.LogInformation("User {UserId} marked notification {NotificationId} as read", userId, id);
                return Ok(new { message = "Notification marked as read" });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to mark notification as read");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification {NotificationId} as read", id);
                return StatusCode(500, new { message = "An error occurred while marking the notification as read" });
            }
        }

        /// <summary>
        /// PUT /api/notifications/{id}/unread
        /// Marks a notification as unread
        /// </summary>
        /// <param name="id">Notification ID</param>
        [HttpPut("{id}/unread")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> MarkAsUnread(int id)
        {
            try
            {
                var userId = GetCurrentUserId();

                // Verify ownership
                var notification = await _notificationRepository.GetByIdAsync(id);
                if (notification == null)
                {
                    return NotFound(new { message = "Notification not found" });
                }

                if (notification.UserId != userId)
                {
                    return Forbid();
                }

                var success = await _notificationRepository.MarkAsUnreadAsync(id);
                if (!success)
                {
                    return NotFound(new { message = "Notification not found" });
                }

                _logger.LogInformation("User {UserId} marked notification {NotificationId} as unread", userId, id);
                return Ok(new { message = "Notification marked as unread" });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to mark notification as unread");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification {NotificationId} as unread", id);
                return StatusCode(500, new { message = "An error occurred while marking the notification as unread" });
            }
        }

        /// <summary>
        /// PUT /api/notifications/mark-all-read
        /// Marks all notifications for the current user as read
        /// </summary>
        [HttpPut("mark-all-read")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> MarkAllAsRead()
        {
            try
            {
                var userId = GetCurrentUserId();
                await _notificationRepository.MarkAllAsReadAsync(userId);

                _logger.LogInformation("User {UserId} marked all notifications as read", userId);
                return Ok(new { message = "All notifications marked as read" });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to mark all notifications as read");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking all notifications as read for user");
                return StatusCode(500, new { message = "An error occurred while marking all notifications as read" });
            }
        }

        /// <summary>
        /// DELETE /api/notifications/{id}
        /// Deletes a specific notification
        /// </summary>
        /// <param name="id">Notification ID</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var userId = GetCurrentUserId();

                // Verify ownership
                var notification = await _notificationRepository.GetByIdAsync(id);
                if (notification == null)
                {
                    return NotFound(new { message = "Notification not found" });
                }

                if (notification.UserId != userId)
                {
                    _logger.LogWarning("User {UserId} attempted to delete notification {NotificationId} (belongs to user {OwnerId})",
                        userId, id, notification.UserId);
                    return Forbid();
                }

                var success = await _notificationRepository.DeleteAsync(id);
                if (!success)
                {
                    return NotFound(new { message = "Notification not found" });
                }

                _logger.LogInformation("User {UserId} deleted notification {NotificationId}", userId, id);
                return Ok(new { message = "Notification deleted successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to delete notification");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting notification {NotificationId}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the notification" });
            }
        }

        /// <summary>
        /// DELETE /api/notifications/delete-all
        /// Deletes all notifications for the current user
        /// </summary>
        [HttpDelete("delete-all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAllNotifications()
        {
            try
            {
                var userId = GetCurrentUserId();
                await _notificationRepository.DeleteAllAsync(userId);

                _logger.LogInformation("User {UserId} deleted all notifications", userId);
                return Ok(new { message = "All notifications deleted successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to delete all notifications");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting all notifications for user");
                return StatusCode(500, new { message = "An error occurred while deleting notifications" });
            }
        }

        /// <summary>
        /// DELETE /api/notifications/delete-old
        /// Deletes old notifications (older than specified days)
        /// </summary>
        /// <param name="daysOld">Age threshold in days (default: 30)</param>
        [HttpDelete("delete-old")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteOldNotifications([FromQuery] int daysOld = 30)
        {
            try
            {
                if (daysOld <= 0)
                {
                    return BadRequest(new { message = "Days old must be greater than zero" });
                }

                var userId = GetCurrentUserId();
                await _notificationRepository.DeleteOldNotificationsAsync(userId, daysOld);

                _logger.LogInformation("User {UserId} deleted notifications older than {DaysOld} days", userId, daysOld);
                return Ok(new { message = $"Notifications older than {daysOld} days deleted successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to delete old notifications");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting old notifications for user");
                return StatusCode(500, new { message = "An error occurred while deleting old notifications" });
            }
        }
    }
}
