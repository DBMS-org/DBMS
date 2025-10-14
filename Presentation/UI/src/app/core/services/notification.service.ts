import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

export interface Notification {
  id: string;
  type: 'success' | 'error' | 'warning' | 'info' | 'assignment' | 'maintenance' | 'request';
  title: string;
  message: string;
  timestamp: Date;
  read: boolean;
  userId?: string;
  relatedId?: string; // Machine ID, Assignment ID, etc.
  relatedType?: 'machine' | 'assignment' | 'request' | 'maintenance';
  actionUrl?: string;
  priority: 'low' | 'medium' | 'high' | 'urgent';
  autoClose?: boolean;
  duration?: number; // in milliseconds
  data?: any; // Additional data for the notification
}

export interface ToastNotification {
  id: string;
  type: 'success' | 'error' | 'warning' | 'info';
  message: string;
  duration?: number;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly defaultDuration = 3000;

  private notifications$ = new BehaviorSubject<Notification[]>([]);
  private toastNotifications$ = new Subject<ToastNotification>();
  private unreadCount$ = new BehaviorSubject<number>(0);

  constructor(private messageService: MessageService) {
    // Load notifications from localStorage on service initialization
    this.loadNotificationsFromStorage();

    // Generate some mock notifications for demonstration
    this.generateMockNotifications();
  }

  // Observable getters
  getNotifications(): Observable<Notification[]> {
    return this.notifications$.asObservable();
  }

  getToastNotifications(): Observable<ToastNotification> {
    return this.toastNotifications$.asObservable();
  }

  getUnreadCount(): Observable<number> {
    return this.unreadCount$.asObservable();
  }

  // Enhanced toast methods
  showSuccess(message: string, title: string = 'Success', duration?: number): void {
    this.messageService.add({
      severity: 'success',
      summary: title,
      detail: message,
      life: duration || this.defaultDuration
    });
  }

  showError(message: string, title: string = 'Error', duration?: number): void {
    this.messageService.add({
      severity: 'error',
      summary: title,
      detail: message,
      life: duration || this.defaultDuration
    });
  }

  showWarning(message: string, title: string = 'Warning', duration?: number): void {
    this.messageService.add({
      severity: 'warn',
      summary: title,
      detail: message,
      life: duration || this.defaultDuration
    });
  }

  showInfo(message: string, title: string = 'Info', duration?: number): void {
    this.messageService.add({
      severity: 'info',
      summary: title,
      detail: message,
      life: duration || this.defaultDuration
    });
  }

  // Add notification methods
  addNotification(notification: Omit<Notification, 'id' | 'timestamp' | 'read'>): void {
    const newNotification: Notification = {
      ...notification,
      id: this.generateId(),
      timestamp: new Date(),
      read: false
    };

    const currentNotifications = this.notifications$.value;
    const updatedNotifications = [newNotification, ...currentNotifications];
    
    this.notifications$.next(updatedNotifications);
    this.updateUnreadCount();
    this.saveNotificationsToStorage();

    // Show toast for high priority notifications
    if (notification.priority === 'high' || notification.priority === 'urgent') {
      this.showToastForNotification(newNotification);
    }
  }

  // Assignment-specific notification methods
  notifyMachineAssigned(machineId: string, machineName: string, operatorName: string, projectName: string): void {
    this.addNotification({
      type: 'assignment',
      title: 'Machine Assigned',
      message: `${machineName} has been assigned to ${operatorName} for ${projectName}`,
      relatedId: machineId,
      relatedType: 'machine',
      priority: 'high',
      actionUrl: `/machine-manager/assignments/${machineId}`,
      data: { machineId, machineName, operatorName, projectName }
    });
  }

  notifyAssignmentRequest(requestId: string, projectName: string, machineType: string, quantity: number): void {
    this.addNotification({
      type: 'request',
      title: 'New Assignment Request',
      message: `Request for ${quantity} ${machineType}(s) for ${projectName}`,
      relatedId: requestId,
      relatedType: 'request',
      priority: 'high',
      actionUrl: `/machine-manager/assignment-requests`,
      data: { requestId, projectName, machineType, quantity }
    });
  }

  notifyRequestApproved(requestId: string, projectName: string): void {
    this.addNotification({
      type: 'success',
      title: 'Request Approved',
      message: `Your machine request for ${projectName} has been approved`,
      relatedId: requestId,
      relatedType: 'request',
      priority: 'medium',
      actionUrl: `/general-manager/requests/${requestId}`,
      data: { requestId, projectName }
    });
  }

  notifyRequestRejected(requestId: string, projectName: string, reason?: string): void {
    this.addNotification({
      type: 'error',
      title: 'Request Rejected',
      message: `Your machine request for ${projectName} has been rejected${reason ? ': ' + reason : ''}`,
      relatedId: requestId,
      relatedType: 'request',
      priority: 'medium',
      actionUrl: `/general-manager/requests/${requestId}`,
      data: { requestId, projectName, reason }
    });
  }

  notifyMachineReturned(machineId: string, machineName: string, operatorName: string): void {
    this.addNotification({
      type: 'info',
      title: 'Machine Returned',
      message: `${machineName} has been returned by ${operatorName}`,
      relatedId: machineId,
      relatedType: 'machine',
      priority: 'medium',
      actionUrl: `/machine-manager/inventory/${machineId}`,
      data: { machineId, machineName, operatorName }
    });
  }

  notifyMaintenanceScheduled(machineId: string, machineName: string, scheduledDate: Date): void {
    this.addNotification({
      type: 'maintenance',
      title: 'Maintenance Scheduled',
      message: `Maintenance scheduled for ${machineName} on ${this.formatDate(scheduledDate)}`,
      relatedId: machineId,
      relatedType: 'maintenance',
      priority: 'medium',
      actionUrl: `/machine-manager/maintenance/${machineId}`,
      data: { machineId, machineName, scheduledDate }
    });
  }

  notifyMaintenanceOverdue(machineId: string, machineName: string, daysPastDue: number): void {
    this.addNotification({
      type: 'warning',
      title: 'Maintenance Overdue',
      message: `${machineName} maintenance is ${daysPastDue} days overdue`,
      relatedId: machineId,
      relatedType: 'maintenance',
      priority: 'urgent',
      actionUrl: `/machine-manager/maintenance/${machineId}`,
      data: { machineId, machineName, daysPastDue }
    });
  }

  // Notification management methods
  markAsRead(notificationId: string): void {
    const notifications = this.notifications$.value;
    const updatedNotifications = notifications.map(notification =>
      notification.id === notificationId
        ? { ...notification, read: true }
        : notification
    );
    
    this.notifications$.next(updatedNotifications);
    this.updateUnreadCount();
    this.saveNotificationsToStorage();
  }

  markAllAsRead(): void {
    const notifications = this.notifications$.value;
    const updatedNotifications = notifications.map(notification => ({
      ...notification,
      read: true
    }));
    
    this.notifications$.next(updatedNotifications);
    this.updateUnreadCount();
    this.saveNotificationsToStorage();
  }

  deleteNotification(notificationId: string): void {
    const notifications = this.notifications$.value;
    const updatedNotifications = notifications.filter(
      notification => notification.id !== notificationId
    );
    
    this.notifications$.next(updatedNotifications);
    this.updateUnreadCount();
    this.saveNotificationsToStorage();
  }

  clearAllNotifications(): void {
    this.notifications$.next([]);
    this.updateUnreadCount();
    this.saveNotificationsToStorage();
  }

  // Filter methods
  getNotificationsByType(type: Notification['type']): Observable<Notification[]> {
    return new BehaviorSubject(
      this.notifications$.value.filter(notification => notification.type === type)
    ).asObservable();
  }

  getUnreadNotifications(): Observable<Notification[]> {
    return new BehaviorSubject(
      this.notifications$.value.filter(notification => !notification.read)
    ).asObservable();
  }

  // Private helper methods
  private updateUnreadCount(): void {
    const unreadCount = this.notifications$.value.filter(n => !n.read).length;
    this.unreadCount$.next(unreadCount);
  }

  private generateId(): string {
    return 'notif_' + Math.random().toString(36).substr(2, 9) + '_' + Date.now();
  }

  private showToastForNotification(notification: Notification): void {
    const duration = notification.priority === 'urgent' ? 8000 : 5000;
    const severity = this.mapNotificationTypeToSeverity(notification.type);

    this.messageService.add({
      severity: severity,
      summary: notification.title,
      detail: notification.message,
      life: duration
    });
  }

  private mapNotificationTypeToSeverity(type: Notification['type']): 'success' | 'info' | 'warn' | 'error' {
    switch (type) {
      case 'success':
      case 'assignment':
        return 'success';
      case 'error':
        return 'error';
      case 'warning':
      case 'maintenance':
        return 'warn';
      case 'info':
      case 'request':
      default:
        return 'info';
    }
  }

  private formatDate(date: Date): string {
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'short',
      day: '2-digit'
    }).format(date);
  }

  // Storage methods
  private saveNotificationsToStorage(): void {
    try {
      const notifications = this.notifications$.value;
      localStorage.setItem('app_notifications', JSON.stringify(notifications));
    } catch (error) {
      console.error('Failed to save notifications to localStorage:', error);
    }
  }

  private loadNotificationsFromStorage(): void {
    try {
      const stored = localStorage.getItem('app_notifications');
      if (stored) {
        const notifications: Notification[] = JSON.parse(stored);
        // Convert timestamp strings back to Date objects
        const processedNotifications = notifications.map(n => ({
          ...n,
          timestamp: new Date(n.timestamp)
        }));
        this.notifications$.next(processedNotifications);
        this.updateUnreadCount();
      }
    } catch (error) {
      console.error('Failed to load notifications from localStorage:', error);
    }
  }

  // Mock data generation for demonstration
  private generateMockNotifications(): void {
    // Only generate if no notifications exist
    if (this.notifications$.value.length === 0) {
      const mockNotifications: Omit<Notification, 'id' | 'timestamp' | 'read'>[] = [
        {
          type: 'assignment',
          title: 'Machine Assigned',
          message: 'CAT 320D Excavator assigned to Ahmed Al-Rashid for Al Hajar Mountain Quarry',
          priority: 'high',
          relatedId: 'M001',
          relatedType: 'machine',
          actionUrl: '/machine-manager/assignments/M001'
        },
        {
          type: 'request',
          title: 'New Assignment Request',
          message: 'Request for 2 Excavator(s) for Muscat Infrastructure Project',
          priority: 'high',
          relatedId: 'REQ-001',
          relatedType: 'request',
          actionUrl: '/machine-manager/assignment-requests'
        },
        {
          type: 'maintenance',
          title: 'Maintenance Due',
          message: 'Komatsu PC200 Excavator maintenance due in 3 days',
          priority: 'medium',
          relatedId: 'M002',
          relatedType: 'maintenance',
          actionUrl: '/machine-manager/maintenance/M002'
        },
        {
          type: 'success',
          title: 'Request Approved',
          message: 'Your machine request for Dhofar Mining Operation has been approved',
          priority: 'medium',
          relatedId: 'REQ-002',
          relatedType: 'request',
          actionUrl: '/general-manager/requests/REQ-002'
        },
        {
          type: 'warning',
          title: 'Maintenance Overdue',
          message: 'Atlas Copco ROC D7 Drill Rig maintenance is 5 days overdue',
          priority: 'urgent',
          relatedId: 'M003',
          relatedType: 'maintenance',
          actionUrl: '/machine-manager/maintenance/M003'
        }
      ];

      // Add mock notifications with staggered timestamps
      mockNotifications.forEach((notification, index) => {
        setTimeout(() => {
          this.addNotification(notification);
        }, index * 100);
      });
    }
  }
}