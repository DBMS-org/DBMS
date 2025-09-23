import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { NotificationService, Notification } from '../../../core/services/notification.service';

type NotificationFilter = 'all' | 'unread' | 'assignment' | 'maintenance' | 'request' | 'success' | 'error' | 'warning' | 'info';

@Component({
  selector: 'app-notification-panel',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notification-panel.component.html',
  styleUrl: './notification-panel.component.scss'
})
export class NotificationPanelComponent implements OnInit, OnDestroy {
  @Input() isOpen = false;
  @Output() closeRequested = new EventEmitter<void>();

  notifications: Notification[] = [];
  filteredNotifications: Notification[] = [];
  unreadCount = 0;
  isLoading = false;
  isRefreshing = false;
  selectedFilter: NotificationFilter = 'all';

  // Confirmation modal
  showConfirmModal = false;
  confirmModalTitle = '';
  confirmModalMessage = '';
  confirmModalAction = '';
  pendingAction: (() => void) | null = null;

  private subscriptions: Subscription[] = [];

  constructor(
    private notificationService: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadNotifications();
    this.subscribeToNotifications();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  private loadNotifications(): void {
    this.isLoading = true;
    
    const notificationsSub = this.notificationService.getNotifications().subscribe({
      next: (notifications) => {
        this.notifications = notifications;
        this.applyFilter();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading notifications:', error);
        this.isLoading = false;
      }
    });

    this.subscriptions.push(notificationsSub);
  }

  private subscribeToNotifications(): void {
    const unreadCountSub = this.notificationService.getUnreadCount().subscribe({
      next: (count) => {
        this.unreadCount = count;
      }
    });

    this.subscriptions.push(unreadCountSub);
  }

  // Filter methods
  setFilter(filter: NotificationFilter): void {
    this.selectedFilter = filter;
    this.applyFilter();
  }

  private applyFilter(): void {
    switch (this.selectedFilter) {
      case 'unread':
        this.filteredNotifications = this.notifications.filter(n => !n.read);
        break;
      case 'assignment':
      case 'maintenance':
      case 'request':
      case 'success':
      case 'error':
      case 'warning':
      case 'info':
        this.filteredNotifications = this.notifications.filter(n => n.type === this.selectedFilter);
        break;
      case 'all':
      default:
        this.filteredNotifications = [...this.notifications];
        break;
    }
  }

  getNotificationCountByType(type: Notification['type']): number {
    return this.notifications.filter(n => n.type === type).length;
  }

  // Notification actions
  handleNotificationClick(notification: Notification): void {
    if (!notification.read) {
      this.markAsRead(notification.id);
    }
  }

  markAsRead(notificationId: string, event?: Event): void {
    if (event) {
      event.stopPropagation();
    }
    this.notificationService.markAsRead(notificationId);
  }

  markAllAsRead(): void {
    if (this.unreadCount === 0) return;
    
    this.showConfirmation(
      'Mark All as Read',
      `Are you sure you want to mark all ${this.unreadCount} notifications as read?`,
      'Mark All Read',
      () => {
        this.notificationService.markAllAsRead();
      }
    );
  }

  deleteNotification(notificationId: string, event?: Event): void {
    if (event) {
      event.stopPropagation();
    }
    
    this.showConfirmation(
      'Delete Notification',
      'Are you sure you want to delete this notification? This action cannot be undone.',
      'Delete',
      () => {
        this.notificationService.deleteNotification(notificationId);
      }
    );
  }

  clearAll(): void {
    if (this.notifications.length === 0) return;
    
    this.showConfirmation(
      'Clear All Notifications',
      `Are you sure you want to delete all ${this.notifications.length} notifications? This action cannot be undone.`,
      'Clear All',
      () => {
        this.notificationService.clearAllNotifications();
      }
    );
  }

  navigateToAction(notification: Notification, event?: Event): void {
    if (event) {
      event.stopPropagation();
    }
    
    if (notification.actionUrl) {
      this.router.navigate([notification.actionUrl]);
      this.closePanel();
    }
  }

  refreshNotifications(): void {
    this.isRefreshing = true;
    
    // Simulate refresh delay
    setTimeout(() => {
      this.loadNotifications();
      this.isRefreshing = false;
    }, 1000);
  }

  closePanel(): void {
    this.closeRequested.emit();
  }

  // Confirmation modal methods
  private showConfirmation(title: string, message: string, action: string, callback: () => void): void {
    this.confirmModalTitle = title;
    this.confirmModalMessage = message;
    this.confirmModalAction = action;
    this.pendingAction = callback;
    this.showConfirmModal = true;
  }

  confirmAction(): void {
    if (this.pendingAction) {
      this.pendingAction();
    }
    this.closeConfirmModal();
  }

  closeConfirmModal(): void {
    this.showConfirmModal = false;
    this.confirmModalTitle = '';
    this.confirmModalMessage = '';
    this.confirmModalAction = '';
    this.pendingAction = null;
  }

  // Utility methods
  trackByNotificationId(index: number, notification: Notification): string {
    return notification.id;
  }

  getNotificationIcon(type: Notification['type']): string {
    switch (type) {
      case 'success':
        return 'fas fa-check-circle';
      case 'error':
        return 'fas fa-exclamation-circle';
      case 'warning':
        return 'fas fa-exclamation-triangle';
      case 'info':
        return 'fas fa-info-circle';
      case 'assignment':
        return 'fas fa-cogs';
      case 'maintenance':
        return 'fas fa-wrench';
      case 'request':
        return 'fas fa-clipboard-list';
      default:
        return 'fas fa-bell';
    }
  }

  getNotificationIconClass(type: Notification['type']): string {
    switch (type) {
      case 'success':
      case 'assignment':
        return 'icon-success';
      case 'error':
        return 'icon-error';
      case 'warning':
      case 'maintenance':
        return 'icon-warning';
      case 'info':
      case 'request':
        return 'icon-info';
      default:
        return 'icon-default';
    }
  }

  getPriorityClass(priority: Notification['priority']): string {
    switch (priority) {
      case 'urgent':
        return 'priority-urgent';
      case 'high':
        return 'priority-high';
      case 'medium':
        return 'priority-medium';
      case 'low':
        return 'priority-low';
      default:
        return 'priority-medium';
    }
  }

  formatRelativeTime(timestamp: Date): string {
    const now = new Date();
    const diffInSeconds = Math.floor((now.getTime() - timestamp.getTime()) / 1000);
    
    if (diffInSeconds < 60) {
      return 'Just now';
    }
    
    const diffInMinutes = Math.floor(diffInSeconds / 60);
    if (diffInMinutes < 60) {
      return `${diffInMinutes}m ago`;
    }
    
    const diffInHours = Math.floor(diffInMinutes / 60);
    if (diffInHours < 24) {
      return `${diffInHours}h ago`;
    }
    
    const diffInDays = Math.floor(diffInHours / 24);
    if (diffInDays < 7) {
      return `${diffInDays}d ago`;
    }
    
    const diffInWeeks = Math.floor(diffInDays / 7);
    if (diffInWeeks < 4) {
      return `${diffInWeeks}w ago`;
    }
    
    return this.formatDate(timestamp);
  }

  formatFullDateTime(timestamp: Date): string {
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'long',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      hour12: true
    }).format(timestamp);
  }

  private formatDate(timestamp: Date): string {
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'short',
      day: '2-digit'
    }).format(timestamp);
  }

  getEmptyStateTitle(): string {
    switch (this.selectedFilter) {
      case 'unread':
        return 'No Unread Notifications';
      case 'assignment':
        return 'No Assignment Notifications';
      case 'maintenance':
        return 'No Maintenance Notifications';
      case 'request':
        return 'No Request Notifications';
      case 'success':
        return 'No Success Notifications';
      case 'error':
        return 'No Error Notifications';
      case 'warning':
        return 'No Warning Notifications';
      case 'info':
        return 'No Info Notifications';
      default:
        return 'No Notifications';
    }
  }

  getEmptyStateMessage(): string {
    switch (this.selectedFilter) {
      case 'unread':
        return 'All notifications have been read.';
      case 'assignment':
        return 'No machine assignment notifications at this time.';
      case 'maintenance':
        return 'No maintenance notifications at this time.';
      case 'request':
        return 'No request notifications at this time.';
      case 'success':
        return 'No success notifications at this time.';
      case 'error':
        return 'No error notifications at this time.';
      case 'warning':
        return 'No warning notifications at this time.';
      case 'info':
        return 'No info notifications at this time.';
      default:
        return 'You\'re all caught up! New notifications will appear here.';
    }
  }
}