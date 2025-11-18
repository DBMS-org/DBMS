using Application.Interfaces;
using Domain.Entities.Notifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Notifications
{
    /// <summary>
    /// Repository implementation for managing notifications
    /// Provides data access operations for notifications
    /// </summary>
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // ===== CREATE =====

        public async Task<Notification> CreateAsync(Notification notification)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<IEnumerable<Notification>> CreateBulkAsync(IEnumerable<Notification> notifications)
        {
            if (notifications == null)
                throw new ArgumentNullException(nameof(notifications));

            var notificationList = notifications.ToList();
            if (!notificationList.Any())
                return notificationList;

            _context.Notifications.AddRange(notificationList);
            await _context.SaveChangesAsync();
            return notificationList;
        }

        // ===== READ =====

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, int skip = 0, int take = 50)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task<IEnumerable<Notification>> GetByTypeAsync(int userId, NotificationType type)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && n.Type == type)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetByPriorityAsync(int userId, NotificationPriority priority)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && n.Priority == priority)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetByRelatedEntityAsync(string entityType, int entityId)
        {
            if (string.IsNullOrWhiteSpace(entityType))
                throw new ArgumentException("Entity type cannot be null or empty", nameof(entityType));

            return await _context.Notifications
                .Where(n => n.RelatedEntityType == entityType && n.RelatedEntityId == entityId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        // ===== UPDATE =====

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
                return false;

            notification.MarkAsRead();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsUnreadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
                return false;

            notification.MarkAsUnread();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var unreadNotifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            if (!unreadNotifications.Any())
                return true;

            foreach (var notification in unreadNotifications)
            {
                notification.MarkAsRead();
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // ===== DELETE =====

        public async Task<bool> DeleteAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
                return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOldNotificationsAsync(int userId, int daysOld = 30)
        {
            if (daysOld <= 0)
                throw new ArgumentException("Days old must be greater than zero", nameof(daysOld));

            var cutoffDate = DateTime.UtcNow.AddDays(-daysOld);

            var oldNotifications = await _context.Notifications
                .Where(n => n.UserId == userId && n.CreatedAt < cutoffDate)
                .ToListAsync();

            if (!oldNotifications.Any())
                return true;

            _context.Notifications.RemoveRange(oldNotifications);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();

            if (!notifications.Any())
                return true;

            _context.Notifications.RemoveRange(notifications);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
