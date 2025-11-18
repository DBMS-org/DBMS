/**
 * Notification Priority Enum
 * Matches backend NotificationPriority enum values exactly
 */
export enum NotificationPriority {
  Low = 0,
  Normal = 1,
  High = 2,
  Urgent = 3,
  Critical = 4
}

/**
 * Helper function to get priority display name
 */
export function getPriorityDisplayName(priority: NotificationPriority): string {
  switch (priority) {
    case NotificationPriority.Low:
      return 'Low';
    case NotificationPriority.Normal:
      return 'Normal';
    case NotificationPriority.High:
      return 'High';
    case NotificationPriority.Urgent:
      return 'Urgent';
    case NotificationPriority.Critical:
      return 'Critical';
    default:
      return 'Unknown';
  }
}

/**
 * Helper function to get priority CSS class for styling
 */
export function getPriorityClass(priority: NotificationPriority): string {
  switch (priority) {
    case NotificationPriority.Low:
      return 'priority-low';
    case NotificationPriority.Normal:
      return 'priority-normal';
    case NotificationPriority.High:
      return 'priority-high';
    case NotificationPriority.Urgent:
      return 'priority-urgent';
    case NotificationPriority.Critical:
      return 'priority-critical';
    default:
      return 'priority-normal';
  }
}

/**
 * Helper function to get priority color
 */
export function getPriorityColor(priority: NotificationPriority): string {
  switch (priority) {
    case NotificationPriority.Low:
      return '#9E9E9E';  // Gray
    case NotificationPriority.Normal:
      return '#2196F3';  // Blue
    case NotificationPriority.High:
      return '#FF9800';  // Orange
    case NotificationPriority.Urgent:
      return '#FF5722';  // Deep Orange
    case NotificationPriority.Critical:
      return '#F44336';  // Red
    default:
      return '#2196F3';
  }
}
