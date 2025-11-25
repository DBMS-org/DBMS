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
  selectedFilter = signal<'all' | 'unread' | 'explosive-requests' | 'projects' | 'sites' | 'drill-data' | 'proposals' | 'system'>('all');
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

        // Filter notifications relevant to Blasting Engineer role
        const blastingEngineerNotifications = notifications.filter(n => {
          const isRelevant = this.isBlastingEngineerRelevantNotification(n.type);
          console.log(`🔔 Notification ${n.id} (type ${n.type}): ${isRelevant ? 'RELEVANT ✅' : 'FILTERED OUT ❌'}`);
          return isRelevant;
        });

        console.log('🔔 Filtered notifications for Blasting Engineer:', blastingEngineerNotifications);
        this.notifications.set(blastingEngineerNotifications);
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
        const blastingEngineerNotifications = notifications.filter(n =>
          this.isBlastingEngineerRelevantNotification(n.type)
        );
        this.notifications.set(blastingEngineerNotifications);
        this.applyFilter();
        this.lastUpdate.set(new Date());
      }
    });

    this.subscriptions.push(sub);
  }

  /**
   * Check if notification type is relevant for Blasting Engineer role
   */
  private isBlastingEngineerRelevantNotification(type: any): boolean {
    // Handle both string and numeric types from backend
    let typeStr: string;
    let typeNum: number;

    if (typeof type === 'string') {
      typeStr = type;
      // Try to get the numeric value from the enum
      typeNum = (NotificationType as any)[type] ?? -1;
    } else {
      typeNum = type;
      typeStr = NotificationType[type] ?? '';
    }

    // Handle unknown/undefined types (type 0 or undefined)
    // Include them by default so they're not hidden
    if (typeNum === 0 || typeNum === -1 || typeNum === undefined || typeNum === null) {
      console.log(`⚠️ Unknown notification type detected: ${type}, including by default`);
      return true;
    }

    // Explosive approval requests (100-199)
    const isExplosiveRequest = !!(
      (typeNum >= 100 && typeNum < 200) ||
      (typeStr && (
        typeStr === 'ExplosiveRequestCreated' ||
        typeStr === 'ExplosiveRequestApproved' ||
        typeStr === 'ExplosiveRequestRejected' ||
        typeStr === 'ExplosiveRequestCancelled' ||
        typeStr === 'ExplosiveRequestExpired' ||
        typeStr === 'ExplosiveRequestUpdated'
      ))
    );

    // Project/site updates (700-799)
    const isProjectSite = !!(
      (typeNum >= 700 && typeNum < 800) ||
      (typeStr && (
        typeStr === 'ProjectCreated' ||
        typeStr === 'ProjectSiteCreated' ||
        typeStr === 'ProjectSiteUpdated' ||
        typeStr === 'BlastSimulationConfirmed' ||
        typeStr === 'PatternApproved' ||
        typeStr === 'PatternApprovalRevoked'
      ))
    );

    // Blast proposals (800-899)
    const isBlastProposal = !!(
      (typeNum >= 800 && typeNum < 900) ||
      (typeStr && (
        typeStr === 'BlastProposalSubmitted' ||
        typeStr === 'BlastProposalApproved' ||
        typeStr === 'BlastProposalRejected'
      ))
    );

    // Drill data (900-999)
    const isDrillData = !!(
      (typeNum >= 900 && typeNum < 1000) ||
      (typeStr && (
        typeStr === 'DrillDataUploaded' ||
        typeStr === 'DrillDataProcessed'
      ))
    );

    // System notifications (1000+)
    const isSystemNotification = !!(
      (typeNum >= 1000) ||
      (typeStr && (
        typeStr === 'System' ||
        typeStr === 'Info' ||
        typeStr === 'Warning' ||
        typeStr === 'Error'
      ))
    );

    return isExplosiveRequest || isProjectSite || isBlastProposal || isDrillData || isSystemNotification;
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
          return typeStr && typeStr.startsWith('ExplosiveApproval');
        });
        break;

      case 'projects':
        filtered = allNotifications.filter(n => {
          const typeStr = typeof n.type === 'string' ? n.type : NotificationType[n.type];
          return typeStr && typeStr.startsWith('ProjectSite');
        });
        break;

      case 'sites':
        filtered = allNotifications.filter(n => {
          const typeStr = typeof n.type === 'string' ? n.type : NotificationType[n.type];
          return typeStr && (
            typeStr === 'ProjectSiteCreated' ||
            typeStr === 'ProjectSiteUpdated'
          );
        });
        break;

      case 'drill-data':
        filtered = allNotifications.filter(n => {
          const typeStr = typeof n.type === 'string' ? n.type : NotificationType[n.type];
          return typeStr && (
            typeStr === 'DrillDataUploaded' ||
            typeStr === 'DrillDataProcessed'
          );
        });
        break;

      case 'proposals':
        filtered = allNotifications.filter(n => {
          const typeStr = typeof n.type === 'string' ? n.type : NotificationType[n.type];
          return typeStr && (
            typeStr === 'BlastProposalSubmitted' ||
            typeStr === 'BlastProposalApproved' ||
            typeStr === 'BlastProposalRejected'
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

  setFilter(filter: 'all' | 'unread' | 'explosive-requests' | 'projects' | 'sites' | 'drill-data' | 'proposals' | 'system') {
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
