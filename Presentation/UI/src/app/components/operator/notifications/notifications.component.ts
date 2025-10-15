import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';

interface Notification {
  id: string;
  type: 'PATTERN_APPROVED' | 'PATTERN_REJECTED' | 'SITE_ASSIGNED' | 'MAINTENANCE_ALERT' | 'PROJECT_UPDATE';
  title: string;
  message: string;
  date: Date;
  read: boolean;
  priority: 'HIGH' | 'MEDIUM' | 'LOW';
  relatedSiteId?: string;
  relatedProjectId?: string;
  relatedMachineId?: string;
  actionUrl?: string;
  icon: string;
  color: string;
}

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
export class OperatorNotificationsComponent implements OnInit {
  private router = inject(Router);

  // Notifications data
  notifications = signal<Notification[]>([]);
  filteredNotifications = signal<Notification[]>([]);

  // Filter state
  selectedFilter = signal<'all' | 'unread' | 'pattern' | 'maintenance' | 'project'>('all');

  // Real-time indicator
  lastUpdate = signal<Date>(new Date());
  isOnline = signal(true);

  ngOnInit() {
    this.loadNotifications();
    this.applyFilter();

    // Simulate real-time updates
    setInterval(() => {
      this.lastUpdate.set(new Date());
    }, 30000);
  }

  private loadNotifications() {
    const mockNotifications: Notification[] = [
      {
        id: 'notif-001',
        type: 'PATTERN_APPROVED',
        title: 'Drill Pattern Approved',
        message: 'Your drill pattern for Site Alpha has been approved by the Blast Engineer.',
        date: new Date(Date.now() - 2 * 60 * 60 * 1000),
        read: false,
        priority: 'HIGH',
        relatedSiteId: 'SITE-001',
        actionUrl: '/operator/my-project',
        icon: 'check_circle',
        color: 'success'
      },
      {
        id: 'notif-002',
        type: 'PATTERN_REJECTED',
        title: 'Drill Pattern Needs Revision',
        message: 'Your drill pattern for Site Beta requires modifications. Please review the feedback.',
        date: new Date(Date.now() - 4 * 60 * 60 * 1000),
        read: false,
        priority: 'HIGH',
        relatedSiteId: 'SITE-002',
        actionUrl: '/operator/my-project',
        icon: 'warning',
        color: 'warning'
      },
      {
        id: 'notif-003',
        type: 'SITE_ASSIGNED',
        title: 'New Site Assigned',
        message: 'You have been assigned to work on Site Gamma.',
        date: new Date(Date.now() - 6 * 60 * 60 * 1000),
        read: false,
        priority: 'MEDIUM',
        relatedSiteId: 'SITE-003',
        actionUrl: '/operator/my-project',
        icon: 'add_location',
        color: 'primary'
      },
      {
        id: 'notif-004',
        type: 'MAINTENANCE_ALERT',
        title: 'Machine Maintenance Alert',
        message: 'Drill Rig DR-102 requires maintenance. Please report any issues.',
        date: new Date(Date.now() - 1 * 24 * 60 * 60 * 1000),
        read: true,
        priority: 'MEDIUM',
        relatedMachineId: 'DR-102',
        actionUrl: '/operator/my-machines',
        icon: 'build',
        color: 'accent'
      },
      {
        id: 'notif-005',
        type: 'PROJECT_UPDATE',
        title: 'Project Status Updated',
        message: 'Mining Project Alpha status has been updated to "In Progress".',
        date: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000),
        read: true,
        priority: 'LOW',
        relatedProjectId: 'PROJ-001',
        actionUrl: '/operator/my-project',
        icon: 'info',
        color: 'info'
      }
    ];

    this.notifications.set(mockNotifications);
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
        filtered = allNotifications.filter(n => !n.read);
        break;
      case 'pattern':
        filtered = allNotifications.filter(n =>
          n.type === 'PATTERN_APPROVED' || n.type === 'PATTERN_REJECTED'
        );
        break;
      case 'maintenance':
        filtered = allNotifications.filter(n => n.type === 'MAINTENANCE_ALERT');
        break;
      case 'project':
        filtered = allNotifications.filter(n =>
          n.type === 'PROJECT_UPDATE' || n.type === 'SITE_ASSIGNED'
        );
        break;
      default:
        filtered = allNotifications;
    }

    this.filteredNotifications.set(filtered);
  }

  setFilter(filter: 'all' | 'unread' | 'pattern' | 'maintenance' | 'project') {
    this.selectedFilter.set(filter);
    this.applyFilter();
  }

  markAsRead(notification: Notification) {
    this.notifications.update(notifications =>
      notifications.map(n =>
        n.id === notification.id ? { ...n, read: true } : n
      )
    );
    this.applyFilter();
  }

  markAllAsRead() {
    this.notifications.update(notifications =>
      notifications.map(n => ({ ...n, read: true }))
    );
    this.applyFilter();
  }

  deleteNotification(notification: Notification) {
    this.notifications.update(notifications =>
      notifications.filter(n => n.id !== notification.id)
    );
    this.applyFilter();
  }

  navigateToContext(notification: Notification) {
    this.markAsRead(notification);

    if (notification.actionUrl) {
      this.router.navigate([notification.actionUrl]);
    }
  }

  getUnreadCount(): number {
    return this.notifications().filter(n => !n.read).length;
  }

  getTimeAgo(date: Date): string {
    const seconds = Math.floor((new Date().getTime() - date.getTime()) / 1000);

    if (seconds < 60) return 'Just now';
    if (seconds < 3600) return `${Math.floor(seconds / 60)} minutes ago`;
    if (seconds < 86400) return `${Math.floor(seconds / 3600)} hours ago`;
    if (seconds < 604800) return `${Math.floor(seconds / 86400)} days ago`;

    return date.toLocaleDateString();
  }

  getNotificationClass(notification: Notification): string {
    return `notification-${notification.color}`;
  }
}
