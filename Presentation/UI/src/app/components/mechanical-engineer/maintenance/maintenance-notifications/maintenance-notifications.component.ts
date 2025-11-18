import { Component, OnInit, OnDestroy, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatBadgeModule } from '@angular/material/badge';
import { MatMenuModule } from '@angular/material/menu';
import { Subscription } from 'rxjs';
import { NotificationService } from '../../../../core/services/notification.service';
import { Notification } from '../../../../core/models/notification.model';
import { NotificationType } from '../../../../core/models/notification-type.enum';
import { NotificationPriority, getPriorityColor, getPriorityDisplayName } from '../../../../core/models/notification-priority.enum';
import { getTimeAgo } from '../../../../core/models/notification.model';
import { getNotificationTypeIcon } from '../../../../core/models/notification-type.enum';

@Component({
  selector: 'app-maintenance-notifications',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatChipsModule,
    MatBadgeModule,
    MatMenuModule
  ],
  templateUrl: './maintenance-notifications.component.html',
  styleUrls: ['./maintenance-notifications.component.scss']
})
export class MaintenanceNotificationsComponent implements OnInit, OnDestroy {
  private router = inject(Router);
  private notificationService = inject(NotificationService);
  private subscriptions: Subscription[] = [];

  // Notifications data
  notifications = signal<Notification[]>([]);
  filteredNotifications = signal<Notification[]>([]);
  isLoading = signal(false);
  error = signal<string | null>(null);

  // Filter state
  selectedFilter = signal<'all' | 'unread' | 'maintenance-jobs' | 'machine-assignments'>('all');

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

  // Expose utility functions to template
  getTimeAgo = getTimeAgo;
  getNotificationTypeIcon = getNotificationTypeIcon;
  getPriorityColor = getPriorityColor;
  getPriorityDisplayName = getPriorityDisplayName;
  NotificationType = NotificationType;

  ngOnInit() {
    this.loadNotifications();
    this.subscribeToNotificationUpdates();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  private loadNotifications() {
    console.log('🔧 [MechanicalEngineerNotifications] Loading notifications...');
    this.isLoading.set(true);
    const sub = this.notificationService.fetchNotifications(0, 50).subscribe({
      next: (notifications) => {
        console.log('🔧 [MechanicalEngineerNotifications] Notifications received:', notifications);
        console.log('🔧 [MechanicalEngineerNotifications] Total notifications:', notifications.length);

        const mechanicalEngineerNotifications = notifications.filter(n =>
          this.isMechanicalEngineerRelevantNotification(n.type)
        );

        console.log('🔧 [MechanicalEngineerNotifications] Filtered mechanical engineer notifications:', mechanicalEngineerNotifications);
        console.log('🔧 [MechanicalEngineerNotifications] Filtered count:', mechanicalEngineerNotifications.length);

        this.notifications.set(mechanicalEngineerNotifications);
        this.applyFilter();
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('❌ [MechanicalEngineerNotifications] Failed to load notifications:', error);
        this.error.set('Failed to load notifications. Please try again.');
        this.isLoading.set(false);
      }
    });
    this.subscriptions.push(sub);
  }

  private subscribeToNotificationUpdates() {
    const sub = this.notificationService.notifications$.subscribe({
      next: (notifications) => {
        const mechanicalEngineerNotifications = notifications.filter(n =>
          this.isMechanicalEngineerRelevantNotification(n.type)
        );
        this.notifications.set(mechanicalEngineerNotifications);
        this.applyFilter();
      }
    });
    this.subscriptions.push(sub);
  }

  private isMechanicalEngineerRelevantNotification(type: NotificationType): boolean {
    const isRelevant = (
      (type >= 400 && type < 500) || // Machine assignments
      (type >= 500 && type < 600) || // Maintenance reports
      (type >= 600 && type < 700) || // Maintenance jobs
      (type >= 1000)                 // System notifications
    );
    console.log(`🔧 [MechanicalEngineerNotifications] Checking type ${type}: ${isRelevant ? 'RELEVANT' : 'NOT RELEVANT'}`);
    return isRelevant;
  }

  applyFilter() {
    const filter = this.selectedFilter();
    const allNotifications = this.notifications();
    console.log(`🔧 [MechanicalEngineerNotifications] Applying filter: ${filter}`, allNotifications);
    let filtered: Notification[] = [];

    switch (filter) {
      case 'all':
        filtered = allNotifications;
        break;
      case 'unread':
        filtered = allNotifications.filter(n => !n.isRead);
        break;
      case 'maintenance-jobs':
        filtered = allNotifications.filter(n => n.type >= 600 && n.type < 700);
        break;
      case 'machine-assignments':
        filtered = allNotifications.filter(n => n.type >= 400 && n.type < 500);
        break;
      default:
        filtered = allNotifications;
    }

    console.log(`🔧 [MechanicalEngineerNotifications] Filtered notifications (${filter}):`, filtered);
    this.filteredNotifications.set(filtered);
  }

  setFilter(filter: 'all' | 'unread' | 'maintenance-jobs' | 'machine-assignments') {
    this.selectedFilter.set(filter);
    this.applyFilter();
  }

  markAsRead(notification: Notification) {
    if (notification.isRead) return;

    const sub = this.notificationService.markAsRead(notification.id).subscribe({
      error: (error) => {
        console.error('Failed to mark notification as read:', error);
      }
    });
    this.subscriptions.push(sub);
  }

  markAsUnread(notification: Notification) {
    if (!notification.isRead) return;

    const sub = this.notificationService.markAsUnread(notification.id).subscribe({
      error: (error) => {
        console.error('Failed to mark notification as unread:', error);
      }
    });
    this.subscriptions.push(sub);
  }

  markAllAsRead() {
    const sub = this.notificationService.markAllAsRead().subscribe({
      error: (error) => {
        console.error('Failed to mark all as read:', error);
      }
    });
    this.subscriptions.push(sub);
  }

  deleteNotification(notification: Notification, event?: Event) {
    if (event) {
      event.stopPropagation();
    }

    if (!confirm('Are you sure you want to delete this notification?')) {
      return;
    }

    const sub = this.notificationService.deleteNotification(notification.id).subscribe({
      error: (error) => {
        console.error('Failed to delete notification:', error);
        this.error.set('Failed to delete notification. Please try again.');
      }
    });
    this.subscriptions.push(sub);
  }

  handleNotificationClick(notification: Notification) {
    this.markAsRead(notification);

    if (notification.actionUrl) {
      this.router.navigateByUrl(notification.actionUrl);
    } else if (notification.relatedEntityType && notification.relatedEntityId) {
      this.navigateToRelatedEntity(notification.relatedEntityType, notification.relatedEntityId);
    }
  }

  private navigateToRelatedEntity(entityType: string, entityId: number) {
    switch (entityType.toLowerCase()) {
      case 'maintenancereport':
        this.router.navigate(['/mechanical-engineer/reports']);
        break;
      case 'maintenancejob':
        this.router.navigate(['/mechanical-engineer/maintenance/jobs']);
        break;
      case 'machineassignment':
        this.router.navigate(['/mechanical-engineer/dashboard']);
        break;
      default:
        this.router.navigate(['/mechanical-engineer/dashboard']);
    }
  }

  refresh() {
    this.loadNotifications();
  }

  getUnreadCount(): number {
    return this.notifications().filter(n => !n.isRead).length;
  }

  updateSetting(settingKey: string, value: boolean) {
    this.settings.update(current => ({
      ...current,
      [settingKey]: value
    }));
  }
}
