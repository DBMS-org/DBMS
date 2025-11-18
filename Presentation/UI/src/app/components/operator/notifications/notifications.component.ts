import { Component, OnInit, OnDestroy, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { Subscription } from 'rxjs';
import { NotificationService } from '../../../core/services/notification.service';
import { Notification } from '../../../core/models/notification.model';
import { NotificationType } from '../../../core/models/notification-type.enum';
import { NotificationPriority, getPriorityColor, getPriorityDisplayName } from '../../../core/models/notification-priority.enum';
import { getTimeAgo, filterNotificationsByCategory } from '../../../core/models/notification.model';
import { getNotificationTypeIcon } from '../../../core/models/notification-type.enum';

@Component({
  selector: 'app-operator-notifications',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatChipsModule
  ],
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class OperatorNotificationsComponent implements OnInit, OnDestroy {
  private router = inject(Router);
  private notificationService = inject(NotificationService);
  private subscriptions: Subscription[] = [];

  // Notifications data
  notifications = signal<Notification[]>([]);
  filteredNotifications = signal<Notification[]>([]);

  // Filter state
  selectedFilter = signal<'all' | 'unread' | 'maintenance-reports' | 'maintenance-jobs' | 'machine-assignments'>('all');

  // Loading and error states
  isLoading = signal(false);
  error = signal<string | null>(null);

  // Real-time indicator
  lastUpdate = signal<Date>(new Date());
  isOnline = signal(true);

  // Notification settings
  settings = signal({
    userManagementPush: true,
    userManagementEmail: false,
    projectUpdatePush: true,
    projectUpdateEmail: false,
    machineAssignmentPush: true,
    machineAssignmentEmail: false,
    maintenancePush: true,
    maintenanceEmail: false,
    systemPush: true,
    systemEmail: false,
    systemAlertPush: true,
    systemAlertEmail: false,
    storeUpdatePush: true,
    storeUpdateEmail: false,
    siteUpdatePush: true,
    siteUpdateEmail: false,
    drillDataPush: true,
    drillDataEmail: false,
    explosiveRequestPush: true,
    explosiveRequestEmail: false,
    inventoryUpdatePush: true,
    inventoryUpdateEmail: false,
    blastCalculationPush: true,
    blastCalculationEmail: false,
    proposalStatusPush: true,
    proposalStatusEmail: false,
    urgencyLevel: 'all'
  });

  ngOnInit() {
    this.loadNotifications();
    this.subscribeToNotificationUpdates();
  }

  ngOnDestroy() {
    // Unsubscribe from all subscriptions to prevent memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  private loadNotifications() {
    this.isLoading.set(true);
    this.error.set(null);

    const sub = this.notificationService.fetchNotifications(0, 50).subscribe({
      next: (notifications) => {
        console.log('🔔 RAW notifications from API:', notifications);
        console.log('🔔 Notification types:', notifications.map(n => ({ id: n.id, type: n.type, title: n.title })));

        // Filter notifications relevant to Operator role
        const operatorNotifications = notifications.filter(n => {
          const isRelevant = this.isOperatorRelevantNotification(n.type);
          console.log(`🔔 Notification ${n.id} (type ${n.type}): ${isRelevant ? 'RELEVANT ✅' : 'FILTERED OUT ❌'}`);
          return isRelevant;
        });

        console.log('🔔 Filtered notifications for Operator:', operatorNotifications);
        this.notifications.set(operatorNotifications);
        this.applyFilter();
        this.isLoading.set(false);
        this.lastUpdate.set(new Date());
      },
      error: (error) => {
        this.error.set('Failed to load notifications. Please try again.');
        this.isLoading.set(false);
        console.error('Error loading notifications:', error);
      }
    });

    this.subscriptions.push(sub);
  }

  private subscribeToNotificationUpdates() {
    // Subscribe to notifications observable for real-time updates
    const sub = this.notificationService.notifications$.subscribe({
      next: (notifications) => {
        const operatorNotifications = notifications.filter(n =>
          this.isOperatorRelevantNotification(n.type)
        );
        this.notifications.set(operatorNotifications);
        this.applyFilter();
        this.lastUpdate.set(new Date());
      }
    });

    this.subscriptions.push(sub);
  }

  /**
   * Check if notification type is relevant for Operator role
   */
  private isOperatorRelevantNotification(type: any): boolean {
    // Handle both numeric enum values and string enum names
    let typeValue: number;

    if (typeof type === 'number') {
      typeValue = type;
    } else if (typeof type === 'string') {
      // Try to get the numeric value from the enum using the string name
      typeValue = (NotificationType as any)[type];

      // If that didn't work, try parsing as number
      if (typeValue === undefined) {
        typeValue = parseInt(type, 10);
      }
    } else {
      return false;
    }

    if (isNaN(typeValue)) {
      console.warn('Could not parse notification type:', type);
      return false;
    }

    console.log(`Checking if type ${type} (value: ${typeValue}) is relevant for Operator`);

    // Machine Assignments (400-499)
    if (typeValue >= 400 && typeValue < 500) return true;

    // Maintenance Reports (500-599)
    if (typeValue >= 500 && typeValue < 600) return true;

    // Maintenance Jobs (600-699)
    if (typeValue >= 600 && typeValue < 700) return true;

    // Specific System & Admin notifications relevant to operators
    if (typeValue === NotificationType.ProjectSiteCreated ||
        typeValue === NotificationType.ProjectSiteUpdated ||
        typeValue === NotificationType.BlastSimulationConfirmed ||
        typeValue === NotificationType.PatternApproved ||
        typeValue === NotificationType.PatternApprovalRevoked) {
      return true;
    }

    // Generic system notifications (1000+)
    if (typeValue >= 1000) return true;

    return false;
  }

  applyFilter() {
    const filter = this.selectedFilter();
    const allNotifications = this.notifications();

    let filtered: Notification[] = [];

    switch (filter) {
      case 'all':
        filtered = allNotifications;
        break;

      case 'unread':
        filtered = allNotifications.filter(n => !n.isRead);
        break;

      case 'maintenance-reports':
        filtered = allNotifications.filter(n => {
          const typeStr = typeof n.type === 'string' ? n.type : NotificationType[n.type];
          return typeStr && typeStr.startsWith('MaintenanceReport');
        });
        break;

      case 'maintenance-jobs':
        filtered = allNotifications.filter(n => {
          const typeStr = typeof n.type === 'string' ? n.type : NotificationType[n.type];
          return typeStr && typeStr.startsWith('MaintenanceJob');
        });
        break;

      case 'machine-assignments':
        filtered = allNotifications.filter(n => {
          const typeStr = typeof n.type === 'string' ? n.type : NotificationType[n.type];
          return typeStr && typeStr.startsWith('MachineAssignment');
        });
        break;

      default:
        filtered = allNotifications;
    }

    this.filteredNotifications.set(filtered);
  }

  setFilter(filter: 'all' | 'unread' | 'maintenance-reports' | 'maintenance-jobs' | 'machine-assignments') {
    this.selectedFilter.set(filter);
    this.applyFilter();
  }

  markAsRead(notification: Notification) {
    if (!notification.isRead) {
      const sub = this.notificationService.markAsRead(notification.id).subscribe({
        next: () => {
          // Local state updated automatically via service observable
        },
        error: (error) => {
          console.error('Error marking notification as read:', error);
        }
      });

      this.subscriptions.push(sub);
    }
  }

  markAllAsRead() {
    const sub = this.notificationService.markAllAsRead().subscribe({
      next: () => {
        // Local state updated automatically via service observable
      },
      error: (error) => {
        console.error('Error marking all as read:', error);
      }
    });

    this.subscriptions.push(sub);
  }

  deleteNotification(notification: Notification) {
    const sub = this.notificationService.deleteNotification(notification.id).subscribe({
      next: () => {
        // Local state updated automatically via service observable
      },
      error: (error) => {
        console.error('Error deleting notification:', error);
      }
    });

    this.subscriptions.push(sub);
  }

  navigateToContext(notification: Notification) {
    this.markAsRead(notification);

    if (notification.actionUrl) {
      // actionUrl is already a full path string
      // Use navigateByUrl instead of navigate for string paths
      this.router.navigateByUrl(notification.actionUrl);
    }
  }

  refreshNotifications() {
    this.loadNotifications();
  }

  getUnreadCount(): number {
    return this.notifications().filter(n => !n.isRead).length;
  }

  getTimeAgo(date: Date): string {
    return getTimeAgo(date);
  }

  getNotificationIcon(notification: Notification): string {
    return getNotificationTypeIcon(notification.type);
  }

  getNotificationColor(notification: Notification): string {
    return getPriorityColor(notification.priority);
  }

  getPriorityLabel(priority: NotificationPriority): string {
    return getPriorityDisplayName(priority);
  }

  getNotificationClass(notification: Notification): string {
    // Map priority to CSS class
    switch (notification.priority) {
      case NotificationPriority.Critical:
        return 'notification-critical';
      case NotificationPriority.Urgent:
        return 'notification-urgent';
      case NotificationPriority.High:
        return 'notification-high';
      case NotificationPriority.Normal:
        return 'notification-normal';
      case NotificationPriority.Low:
        return 'notification-low';
      default:
        return 'notification-normal';
    }
  }

  updateSetting(settingKey: string, value: boolean) {
    this.settings.update(current => ({
      ...current,
      [settingKey]: value
    }));
  }
}
