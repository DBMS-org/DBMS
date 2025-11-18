import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { BehaviorSubject, Observable, interval, throwError } from 'rxjs';
import { map, tap, catchError, switchMap, startWith } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Notification, UnreadCountResponse } from '../models/notification.model';
import { NotificationType } from '../models/notification-type.enum';
import { NotificationPriority } from '../models/notification-priority.enum';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly http: HttpClient;
  private readonly messageService: MessageService;
  private readonly defaultDuration = 3000;
  private readonly apiUrl = `${environment.apiUrl}/api/notifications`;

  // BehaviorSubjects for reactive state management
  private notificationsSubject = new BehaviorSubject<Notification[]>([]);
  private unreadCountSubject = new BehaviorSubject<number>(0);
  private loadingSubject = new BehaviorSubject<boolean>(false);
  private errorSubject = new BehaviorSubject<string | null>(null);

  // Public observables
  public notifications$ = this.notificationsSubject.asObservable();
  public unreadCount$ = this.unreadCountSubject.asObservable();
  public loading$ = this.loadingSubject.asObservable();
  public error$ = this.errorSubject.asObservable();

  constructor(http: HttpClient, messageService: MessageService) {
    this.http = http;
    this.messageService = messageService;
    // Start polling for notifications after a brief delay to avoid circular dependency issues
    setTimeout(() => this.startPolling(), 100);
  }

  /**
   * Start automatic polling for new notifications
   */
  private startPolling(): void {
    interval(30000) // 30 seconds
      .pipe(
        startWith(0), // Fetch immediately on init
        switchMap(() => this.fetchNotifications())
      )
      .subscribe({
        error: (error) => {
          console.error('Polling error:', error);
          // Don't stop polling on error, it will retry after 30 seconds
        }
      });
  }

  // ========== TOAST NOTIFICATION METHODS (PrimeNG) ==========

  /**
   * Show success toast message
   */
  showSuccess(message: string, title: string = 'Success', duration?: number): void {
    this.messageService.add({
      severity: 'success',
      summary: title,
      detail: message,
      life: duration || this.defaultDuration
    });
  }

  /**
   * Show error toast message
   */
  showError(message: string, title: string = 'Error', duration?: number): void {
    this.messageService.add({
      severity: 'error',
      summary: title,
      detail: message,
      life: duration || this.defaultDuration
    });
  }

  /**
   * Show warning toast message
   */
  showWarning(message: string, title: string = 'Warning', duration?: number): void {
    this.messageService.add({
      severity: 'warn',
      summary: title,
      detail: message,
      life: duration || this.defaultDuration
    });
  }

  /**
   * Show info toast message
   */
  showInfo(message: string, title: string = 'Info', duration?: number): void {
    this.messageService.add({
      severity: 'info',
      summary: title,
      detail: message,
      life: duration || this.defaultDuration
    });
  }

  // ========== BACKEND API METHODS ==========

  /**
   * Fetch all notifications from backend (paginated)
   */
  fetchNotifications(skip: number = 0, take: number = 50): Observable<Notification[]> {
    console.log('🔔 [NotificationService] Fetching notifications...', { skip, take, url: `${this.apiUrl}?skip=${skip}&take=${take}` });
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    return this.http.get<Notification[]>(`${this.apiUrl}?skip=${skip}&take=${take}`)
      .pipe(
        tap(rawNotifications => {
          console.log('🔔 [NotificationService] Raw notifications received from backend:', rawNotifications);
          console.log('🔔 [NotificationService] Number of notifications:', rawNotifications?.length || 0);
          if (rawNotifications && rawNotifications.length > 0) {
            console.log('🔔 [NotificationService] First notification sample:', rawNotifications[0]);
          }
        }),
        map(notifications => this.processNotifications(notifications)),
        tap(notifications => {
          console.log('🔔 [NotificationService] Processed notifications:', notifications);
          console.log('🔔 [NotificationService] Number of processed notifications:', notifications.length);
          if (notifications.length > 0) {
            console.log('🔔 [NotificationService] First processed notification sample:', notifications[0]);
          }
          this.notificationsSubject.next(notifications);
          this.updateLocalUnreadCount();
          this.loadingSubject.next(false);
        }),
        catchError(error => {
          console.error('❌ [NotificationService] Error fetching notifications:', error);
          console.error('❌ [NotificationService] Error status:', error.status);
          console.error('❌ [NotificationService] Error message:', error.message);
          this.handleError(error, 'Failed to load notifications');
          this.loadingSubject.next(false);
          return throwError(() => error);
        })
      );
  }

  /**
   * Fetch only unread notifications
   */
  fetchUnreadNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.apiUrl}/unread`)
      .pipe(
        map(notifications => this.processNotifications(notifications)),
        catchError(error => {
          this.handleError(error, 'Failed to load unread notifications');
          return throwError(() => error);
        })
      );
  }

  /**
   * Fetch unread notification count for badge display
   */
  fetchUnreadCount(): Observable<number> {
    return this.http.get<UnreadCountResponse>(`${this.apiUrl}/unread-count`)
      .pipe(
        map(response => response.count),
        tap(count => this.unreadCountSubject.next(count)),
        catchError(error => {
          this.handleError(error, 'Failed to load unread count');
          return throwError(() => error);
        })
      );
  }

  /**
   * Get a specific notification by ID
   */
  getNotificationById(id: number): Observable<Notification> {
    return this.http.get<Notification>(`${this.apiUrl}/${id}`)
      .pipe(
        map(notification => this.processNotification(notification)),
        catchError(error => {
          this.handleError(error, 'Failed to load notification');
          return throwError(() => error);
        })
      );
  }

  /**
   * Get notifications by type
   */
  getNotificationsByType(type: NotificationType): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.apiUrl}/type/${type}`)
      .pipe(
        map(notifications => this.processNotifications(notifications)),
        catchError(error => {
          this.handleError(error, 'Failed to load notifications by type');
          return throwError(() => error);
        })
      );
  }

  /**
   * Mark a notification as read
   */
  markAsRead(id: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/read`, {})
      .pipe(
        tap(() => {
          this.updateNotificationInLocalState(id, { isRead: true });
          this.updateLocalUnreadCount();
        }),
        catchError(error => {
          this.handleError(error, 'Failed to mark notification as read');
          return throwError(() => error);
        })
      );
  }

  /**
   * Mark a notification as unread
   */
  markAsUnread(id: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/unread`, {})
      .pipe(
        tap(() => {
          this.updateNotificationInLocalState(id, { isRead: false });
          this.updateLocalUnreadCount();
        }),
        catchError(error => {
          this.handleError(error, 'Failed to mark notification as unread');
          return throwError(() => error);
        })
      );
  }

  /**
   * Mark all notifications as read
   */
  markAllAsRead(): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/mark-all-read`, {})
      .pipe(
        tap(() => {
          const notifications = this.notificationsSubject.value;
          const updated = notifications.map(n => ({ ...n, isRead: true }));
          this.notificationsSubject.next(updated);
          this.unreadCountSubject.next(0);
          this.showSuccess('All notifications marked as read');
        }),
        catchError(error => {
          this.handleError(error, 'Failed to mark all as read');
          return throwError(() => error);
        })
      );
  }

  /**
   * Delete a specific notification
   */
  deleteNotification(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
      .pipe(
        tap(() => {
          const notifications = this.notificationsSubject.value.filter(n => n.id !== id);
          this.notificationsSubject.next(notifications);
          this.updateLocalUnreadCount();
          this.showInfo('Notification deleted');
        }),
        catchError(error => {
          this.handleError(error, 'Failed to delete notification');
          return throwError(() => error);
        })
      );
  }

  /**
   * Delete all notifications
   */
  deleteAllNotifications(): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/delete-all`)
      .pipe(
        tap(() => {
          this.notificationsSubject.next([]);
          this.unreadCountSubject.next(0);
          this.showInfo('All notifications deleted');
        }),
        catchError(error => {
          this.handleError(error, 'Failed to delete all notifications');
          return throwError(() => error);
        })
      );
  }

  /**
   * Delete old notifications (older than specified days)
   */
  deleteOldNotifications(daysOld: number = 30): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/delete-old?daysOld=${daysOld}`)
      .pipe(
        tap(() => {
          // Refresh notifications after deletion
          this.fetchNotifications().subscribe();
          this.showInfo(`Notifications older than ${daysOld} days deleted`);
        }),
        catchError(error => {
          this.handleError(error, 'Failed to delete old notifications');
          return throwError(() => error);
        })
      );
  }

  // ========== HELPER METHODS ==========

  /**
   * Process notifications array (convert dates, ensure type safety)
   */
  private processNotifications(notifications: Notification[]): Notification[] {
    return notifications.map(n => this.processNotification(n));
  }

  /**
   * Process a single notification (convert date strings to Date objects and ensure numeric types)
   */
  private processNotification(notification: any): Notification {
    console.log('🔄 [NotificationService] Processing notification:', notification);

    // Helper function to safely parse numbers
    const safeParseInt = (value: any, defaultValue: number = 0): number => {
      if (value === null || value === undefined) return defaultValue;
      if (typeof value === 'number') return isNaN(value) ? defaultValue : value;
      if (typeof value === 'string') {
        const parsed = parseInt(value, 10);
        return isNaN(parsed) ? defaultValue : parsed;
      }
      return defaultValue;
    };

    // Helper function to safely parse numbers that can be null
    const safeParseIntOrNull = (value: any): number | null => {
      if (value === null || value === undefined) return null;
      if (typeof value === 'number') return isNaN(value) ? null : value;
      if (typeof value === 'string') {
        const parsed = parseInt(value, 10);
        return isNaN(parsed) ? null : parsed;
      }
      return null;
    };

    // Parse the type with fallback logic
    let parsedType = safeParseInt(notification.type, 0);

    // If type is 0 or invalid, try to infer from title or relatedEntityType
    if (parsedType === 0) {
      console.warn('⚠️ [NotificationService] Invalid notification type, attempting to infer from context');
      const title = (notification.title || '').toLowerCase();
      const relatedEntityType = (notification.relatedEntityType || '').toLowerCase();

      // Infer type based on title or related entity type
      // When operator submits a maintenance report, it becomes a job for mechanical engineer
      if (title.includes('maintenance report') || relatedEntityType.includes('maintenancereport')) {
        parsedType = 600; // MaintenanceJobCreated - treat reports as jobs for mechanical engineers
      } else if (title.includes('maintenance job') || relatedEntityType.includes('maintenancejob')) {
        parsedType = 600; // MaintenanceJobCreated
      } else if (title.includes('machine') && title.includes('assigned')) {
        parsedType = 400; // MachineAssigned
      }
      console.log(`✅ [NotificationService] Inferred type: ${parsedType}`);
    }

    const processed = {
      ...notification,
      id: safeParseInt(notification.id),
      userId: safeParseInt(notification.userId),
      type: parsedType,
      priority: safeParseInt(notification.priority, 0),
      relatedEntityId: safeParseIntOrNull(notification.relatedEntityId),
      createdAt: new Date(notification.createdAt),
      readAt: notification.readAt ? new Date(notification.readAt) : null,
      updatedAt: notification.updatedAt ? new Date(notification.updatedAt) : undefined
    };

    console.log('✅ [NotificationService] Processed notification result:', processed);
    return processed;
  }

  /**
   * Update a notification in local state (optimistic update)
   */
  private updateNotificationInLocalState(id: number, updates: Partial<Notification>): void {
    const notifications = this.notificationsSubject.value;
    const updated = notifications.map(n =>
      n.id === id ? { ...n, ...updates } : n
    );
    this.notificationsSubject.next(updated);
  }

  /**
   * Update unread count from local state
   */
  private updateLocalUnreadCount(): void {
    const count = this.notificationsSubject.value.filter(n => !n.isRead).length;
    this.unreadCountSubject.next(count);
  }

  /**
   * Handle HTTP errors
   */
  private handleError(error: HttpErrorResponse, defaultMessage: string): void {
    let errorMessage = defaultMessage;

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      if (error.status === 0) {
        errorMessage = 'Unable to connect to the server. Please check your internet connection.';
      } else if (error.status === 401) {
        errorMessage = 'Unauthorized. Please log in again.';
      } else if (error.status === 403) {
        errorMessage = 'You do not have permission to access this notification.';
      } else if (error.status === 404) {
        errorMessage = 'Notification not found.';
      } else if (error.status === 500) {
        errorMessage = 'Server error. Please try again later.';
      } else {
        errorMessage = error.error?.message || defaultMessage;
      }
    }

    this.errorSubject.next(errorMessage);
    this.showError(errorMessage);
    console.error('Notification service error:', error);
  }

  /**
   * Show toast for high-priority notifications (can be called when new notification arrives)
   */
  showToastForNotification(notification: Notification): void {
    if (notification.priority >= NotificationPriority.High) {
      const duration = notification.priority === NotificationPriority.Critical ? 10000 :
                      notification.priority === NotificationPriority.Urgent ? 8000 : 5000;

      this.messageService.add({
        severity: this.mapPriorityToSeverity(notification.priority),
        summary: notification.title,
        detail: notification.message,
        life: duration
      });
    }
  }

  /**
   * Map notification priority to PrimeNG severity
   */
  private mapPriorityToSeverity(priority: NotificationPriority): 'success' | 'info' | 'warn' | 'error' {
    switch (priority) {
      case NotificationPriority.Critical:
        return 'error';
      case NotificationPriority.Urgent:
      case NotificationPriority.High:
        return 'warn';
      case NotificationPriority.Normal:
        return 'info';
      case NotificationPriority.Low:
        return 'success';
      default:
        return 'info';
    }
  }

  /**
   * Refresh notifications manually (useful for pull-to-refresh)
   */
  refreshNotifications(): void {
    this.fetchNotifications().subscribe();
    this.fetchUnreadCount().subscribe();
  }

  /**
   * Get current notifications value (synchronous)
   */
  getCurrentNotifications(): Notification[] {
    return this.notificationsSubject.value;
  }

  /**
   * Get current unread count (synchronous)
   */
  getCurrentUnreadCount(): number {
    return this.unreadCountSubject.value;
  }
}
