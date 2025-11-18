import { Component, OnInit, OnDestroy, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatBadgeModule } from '@angular/material/badge';
import { MatMenuModule } from '@angular/material/menu';
import { Subscription } from 'rxjs';
import { NotificationService } from '../../../core/services/notification.service';
import { Notification } from '../../../core/models/notification.model';
import { NotificationType } from '../../../core/models/notification-type.enum';
import { NotificationPriority, getPriorityColor, getPriorityDisplayName } from '../../../core/models/notification-priority.enum';
import { getTimeAgo } from '../../../core/models/notification.model';
import { getNotificationTypeIcon } from '../../../core/models/notification-type.enum';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatChipsModule,
    MatBadgeModule,
    MatMenuModule
  ],
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit, OnDestroy {
  private router = inject(Router);
  private notificationService = inject(NotificationService);
  private subscriptions: Subscription[] = [];

  notifications = signal<Notification[]>([]);
  filteredNotifications = signal<Notification[]>([]);
  selectedFilter = signal<'all' | 'unread' | 'user-management' | 'users' | 'projects' | 'machines' | 'system'>('all');
  isLoading = signal(false);
  error = signal<string | null>(null);
  lastUpdate = signal<Date>(new Date());
  isOnline = signal(true);

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

        // Admin receives all notifications
        this.notifications.set(notifications);
        this.applyFilter();
        this.isLoading.set(false);
        this.lastUpdate.set(new Date());

        console.log('🔔 Admin loaded all notifications:', notifications.length);
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
        // Admin receives all notifications
        this.notifications.set(notifications);
        this.applyFilter();
        this.lastUpdate.set(new Date());
      }
    });

    this.subscriptions.push(sub);
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

      case 'user-management':
      case 'users':
        filtered = allNotifications.filter(n => {
          const typeStr = typeof n.type === 'string' ? n.type : NotificationType[n.type];
          return typeStr && (
            typeStr === 'UserCreated' ||
            typeStr === 'UserUpdated' ||
            typeStr === 'UserDeleted' ||
            typeStr === 'ProjectSiteCreated' ||
            typeStr === 'ProjectSiteUpdated'
          );
        });
        break;

      case 'projects':
        filtered = allNotifications.filter(n => {
          const typeStr = typeof n.type === 'string' ? n.type : NotificationType[n.type];
          return typeStr && (
            typeStr.startsWith('ExplosiveApproval') ||
            typeStr.startsWith('Transfer') ||
            typeStr.startsWith('ProjectSite')
          );
        });
        break;

      case 'machines':
        filtered = allNotifications.filter(n => {
          const typeStr = typeof n.type === 'string' ? n.type : NotificationType[n.type];
          return typeStr && (
            typeStr.startsWith('MachineRequest') ||
            typeStr.startsWith('MachineAssignment')
          );
        });
        break;

      case 'system':
        filtered = allNotifications.filter(n => {
          const typeStr = typeof n.type === 'string' ? n.type : NotificationType[n.type];
          return typeStr && (
            typeStr === 'System' ||
            typeStr === 'Info' ||
            typeStr === 'Warning' ||
            typeStr === 'Error'
          );
        });
        break;

      default:
        filtered = allNotifications;
    }

    this.filteredNotifications.set(filtered);
  }

  setFilter(filter: 'all' | 'unread' | 'user-management' | 'users' | 'projects' | 'machines' | 'system') {
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

    // Check notification type for specific routing
    const typeStr = typeof notification.type === 'string' ? notification.type : NotificationType[notification.type];

    // Blast simulation confirmed notifications
    if (typeStr === 'BlastSimulationConfirmed') {
      this.router.navigate(['/admin/project-management']);
      return;
    }

    // Fallback to actionUrl if provided
    if (notification.actionUrl) {
      // actionUrl is already a full path string
      // Use navigateByUrl instead of navigate for string paths
      this.router.navigateByUrl(notification.actionUrl);
      return;
    }

    // Default to admin dashboard
    this.router.navigate(['/admin/dashboard']);
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
}
