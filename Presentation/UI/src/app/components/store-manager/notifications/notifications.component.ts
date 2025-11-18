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
import { getTimeAgo } from '../../../core/models/notification.model';
import { getNotificationTypeIcon } from '../../../core/models/notification-type.enum';

@Component({
  selector: 'app-store-manager-notifications',
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
export class StoreManagerNotificationsComponent implements OnInit, OnDestroy {
  private router = inject(Router);
  private notificationService = inject(NotificationService);
  private subscriptions: Subscription[] = [];

  notifications = signal<Notification[]>([]);
  filteredNotifications = signal<Notification[]>([]);
  selectedFilter = signal<'all' | 'unread' | 'explosive-requests' | 'transfer-requests' | 'inventory'>('all');
  isLoading = signal(false);
  error = signal<string | null>(null);
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
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  private loadNotifications() {
    this.isLoading.set(true);
    this.error.set(null);

    const sub = this.notificationService.fetchNotifications(0, 50).subscribe({
      next: (notifications) => {
        console.log('🔔 RAW notifications from API:', notifications);
        console.log('🔔 Notification types:', notifications.map(n => ({ id: n.id, type: n.type, title: n.title })));

        // Filter notifications relevant to Store Manager role
        const storeManagerNotifications = notifications.filter(n => {
          const isRelevant = this.isStoreManagerRelevantNotification(n.type);
          console.log(`🔔 Notification ${n.id} (type ${n.type}): ${isRelevant ? 'RELEVANT ✅' : 'FILTERED OUT ❌'}`);
          return isRelevant;
        });

        console.log('🔔 Filtered notifications for Store Manager:', storeManagerNotifications);
        this.notifications.set(storeManagerNotifications);
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
    const sub = this.notificationService.notifications$.subscribe({
      next: (notifications) => {
        const storeManagerNotifications = notifications.filter(n =>
          this.isStoreManagerRelevantNotification(n.type)
        );
        this.notifications.set(storeManagerNotifications);
        this.applyFilter();
        this.lastUpdate.set(new Date());
      }
    });

    this.subscriptions.push(sub);
  }

  /**
   * Check if notification type is relevant for Store Manager role
   */
  private isStoreManagerRelevantNotification(type: any): boolean {
    // When enum comes as string from backend, check the string value
    const typeStr = typeof type === 'string' ? type : NotificationType[type];

    // Explosive approval requests (from Blasting Engineers)
    const isExplosiveRequest = !!(typeStr && (
      typeStr === 'ExplosiveRequestCreated' ||
      typeStr === 'ExplosiveRequestApproved' ||
      typeStr === 'ExplosiveRequestRejected' ||
      typeStr === 'ExplosiveRequestCancelled'
    ));

    // Transfer request notifications (to/from Explosive Manager)
    const isTransferRequest = !!(typeStr && (
      typeStr === 'TransferRequestCreated' ||
      typeStr === 'TransferRequestUrgent' ||
      typeStr === 'TransferRequestApproved' ||
      typeStr === 'TransferRequestRejected' ||
      typeStr === 'TransferDispatched' ||
      typeStr === 'TransferCompleted' ||
      typeStr === 'TransferCancelled'
    ));

    // System notifications
    const isSystemNotification = !!(typeStr && (
      typeStr === 'System' ||
      typeStr === 'Info' ||
      typeStr === 'Warning' ||
      typeStr === 'Error'
    ));

    return isExplosiveRequest || isTransferRequest || isSystemNotification;
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

      case 'explosive-requests':
        filtered = allNotifications.filter(n => {
          const typeStr = typeof n.type === 'string' ? n.type : NotificationType[n.type];
          return typeStr && (
            typeStr === 'ExplosiveRequestCreated' ||
            typeStr === 'ExplosiveRequestApproved' ||
            typeStr === 'ExplosiveRequestRejected' ||
            typeStr === 'ExplosiveRequestCancelled'
          );
        });
        break;

      case 'transfer-requests':
        filtered = allNotifications.filter(n => {
          const typeStr = typeof n.type === 'string' ? n.type : NotificationType[n.type];
          return typeStr && typeStr.startsWith('Transfer');
        });
        break;

      case 'inventory':
        filtered = allNotifications.filter(n => {
          const typeStr = typeof n.type === 'string' ? n.type : NotificationType[n.type];
          return typeStr && (
            typeStr === 'TransferRequestCreated' ||
            typeStr === 'TransferRequestUrgent' ||
            typeStr === 'TransferDispatched' ||
            typeStr === 'TransferCompleted'
          );
        });
        break;

      default:
        filtered = allNotifications;
    }

    this.filteredNotifications.set(filtered);
  }

  setFilter(filter: 'all' | 'unread' | 'explosive-requests' | 'transfer-requests' | 'inventory') {
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

    // Navigate based on notification type
    const typeStr = typeof notification.type === 'string' ? notification.type : NotificationType[notification.type];

    // Explosive approval requests from Blasting Engineers
    if (typeStr === 'ExplosiveRequestCreated' ||
        typeStr === 'ExplosiveRequestApproved' ||
        typeStr === 'ExplosiveRequestRejected' ||
        typeStr === 'ExplosiveRequestCancelled') {
      this.router.navigate(['/store-manager/blasting-engineer-requests']);
      return;
    }

    // Transfer requests (to/from Explosive Manager)
    if (typeStr?.startsWith('Transfer')) {
      this.router.navigate(['/store-manager/request-history']);
      return;
    }

    // Default to dashboard for other notifications
    this.router.navigate(['/store-manager/dashboard']);
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
