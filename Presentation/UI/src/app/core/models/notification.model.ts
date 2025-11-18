import { NotificationType } from './notification-type.enum';
import { NotificationPriority } from './notification-priority.enum';

/**
 * Notification Model
 * Matches backend C# Notification entity structure exactly
 */
export interface Notification {
  id: number;
  userId: number;
  type: NotificationType;
  title: string;
  message: string;
  priority: NotificationPriority;
  isRead: boolean;
  readAt?: Date | null;
  relatedEntityType?: string | null;
  relatedEntityId?: number | null;
  actionUrl?: string | null;
  additionalData?: string | null;
  createdAt: Date;
  updatedAt?: Date;
}

/**
 * DTO for creating notifications (if needed for admin features)
 */
export interface CreateNotificationRequest {
  userId: number;
  type: NotificationType;
  title: string;
  message: string;
  priority?: NotificationPriority;
  relatedEntityType?: string;
  relatedEntityId?: number;
  actionUrl?: string;
  additionalData?: string;
}

/**
 * Response for unread count endpoint
 */
export interface UnreadCountResponse {
  count: number;
}

/**
 * Filter options for notification list
 */
export type NotificationFilter =
  | 'all'
  | 'unread'
  | 'explosive-requests'
  | 'transfer-requests'
  | 'machine-requests'
  | 'machine-assignments'
  | 'maintenance-reports'
  | 'maintenance-jobs'
  | 'system-admin';

/**
 * Helper function to check if notification is recent (within 24 hours)
 */
export function isRecentNotification(notification: Notification): boolean {
  const now = new Date();
  const createdAt = new Date(notification.createdAt);
  const hoursDiff = (now.getTime() - createdAt.getTime()) / (1000 * 60 * 60);
  return hoursDiff <= 24;
}

/**
 * Helper function to format notification time (e.g., "2 hours ago")
 */
export function getTimeAgo(date: Date | string): string {
  const now = new Date();
  const notificationDate = new Date(date);
  const seconds = Math.floor((now.getTime() - notificationDate.getTime()) / 1000);

  if (seconds < 60) {
    return 'Just now';
  }

  const minutes = Math.floor(seconds / 60);
  if (minutes < 60) {
    return `${minutes} ${minutes === 1 ? 'minute' : 'minutes'} ago`;
  }

  const hours = Math.floor(minutes / 60);
  if (hours < 24) {
    return `${hours} ${hours === 1 ? 'hour' : 'hours'} ago`;
  }

  const days = Math.floor(hours / 24);
  if (days < 7) {
    return `${days} ${days === 1 ? 'day' : 'days'} ago`;
  }

  const weeks = Math.floor(days / 7);
  if (weeks < 4) {
    return `${weeks} ${weeks === 1 ? 'week' : 'weeks'} ago`;
  }

  const months = Math.floor(days / 30);
  if (months < 12) {
    return `${months} ${months === 1 ? 'month' : 'months'} ago`;
  }

  const years = Math.floor(days / 365);
  return `${years} ${years === 1 ? 'year' : 'years'} ago`;
}

/**
 * Helper function to filter notifications by type category
 */
export function filterNotificationsByCategory(
  notifications: Notification[],
  filter: NotificationFilter
): Notification[] {
  switch (filter) {
    case 'all':
      return notifications;

    case 'unread':
      return notifications.filter(n => !n.isRead);

    case 'explosive-requests':
      return notifications.filter(n => n.type >= 100 && n.type < 200);

    case 'transfer-requests':
      return notifications.filter(n => n.type >= 200 && n.type < 300);

    case 'machine-requests':
      return notifications.filter(n => n.type >= 300 && n.type < 400);

    case 'machine-assignments':
      return notifications.filter(n => n.type >= 400 && n.type < 500);

    case 'maintenance-reports':
      return notifications.filter(n => n.type >= 500 && n.type < 600);

    case 'maintenance-jobs':
      return notifications.filter(n => n.type >= 600 && n.type < 700);

    case 'system-admin':
      return notifications.filter(n => n.type >= 700 && n.type < 800);

    default:
      return notifications;
  }
}

/**
 * Helper function to sort notifications (newest first, unread first)
 */
export function sortNotifications(notifications: Notification[]): Notification[] {
  return [...notifications].sort((a, b) => {
    // Unread notifications first
    if (a.isRead !== b.isRead) {
      return a.isRead ? 1 : -1;
    }

    // Then by created date (newest first)
    const dateA = new Date(a.createdAt).getTime();
    const dateB = new Date(b.createdAt).getTime();
    return dateB - dateA;
  });
}
