import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatBadgeModule } from '@angular/material/badge';
import { MatMenuModule } from '@angular/material/menu';

interface Notification {
  id: string;
  type: 'SCHEDULED_MAINTENANCE' | 'SERVICE_ALERT' | 'LOW_STOCK' | 'MAINTENANCE_COMPLETION';
  title: string;
  message: string;
  date: Date;
  read: boolean;
  priority: 'HIGH' | 'MEDIUM' | 'LOW';
  relatedJobId?: string;
  relatedMachineId?: string;
  relatedAccessoryId?: string;
  actionUrl?: string;
  icon: string;
  color: string;
}

interface NotificationSettings {
  scheduledMaintenancePush: boolean;
  scheduledMaintenanceEmail: boolean;
  serviceAlertPush: boolean;
  serviceAlertEmail: boolean;
  lowStockPush: boolean;
  lowStockEmail: boolean;
  maintenanceCompletionPush: boolean;
  maintenanceCompletionEmail: boolean;
  urgencyLevel: 'ALL' | 'HIGH' | 'CRITICAL';
}

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
export class MaintenanceNotificationsComponent implements OnInit {
  private router = inject(Router);

  // Notifications data
  notifications = signal<Notification[]>([]);
  filteredNotifications = signal<Notification[]>([]);

  // Filter state
  selectedFilter = signal<'all' | 'unread' | 'scheduled' | 'service-alert' | 'low-stock' | 'completion'>('all');

  // Settings
  settings = signal<NotificationSettings>({
    scheduledMaintenancePush: true,
    scheduledMaintenanceEmail: true,
    serviceAlertPush: true,
    serviceAlertEmail: true,
    lowStockPush: true,
    lowStockEmail: false,
    maintenanceCompletionPush: true,
    maintenanceCompletionEmail: false,
    urgencyLevel: 'ALL'
  });

  showSettings = signal(false);

  // Real-time indicator
  lastUpdate = signal<Date>(new Date());
  isOnline = signal(true);

  ngOnInit() {
    this.loadNotifications();
    this.applyFilter();

    // Simulate real-time updates every 30 seconds
    setInterval(() => {
      this.lastUpdate.set(new Date());
    }, 30000);
  }

  private loadNotifications() {
    const mockNotifications: Notification[] = [
      {
        id: 'notif-001',
        type: 'SCHEDULED_MAINTENANCE',
        title: 'Maintenance Scheduled',
        message: 'Machine Drill Rig DR-102 scheduled for maintenance on 2024-02-15.',
        date: new Date(Date.now() - 2 * 60 * 60 * 1000), // 2 hours ago
        read: false,
        priority: 'MEDIUM',
        relatedJobId: 'JOB-001',
        relatedMachineId: 'DR-102',
        actionUrl: '/mechanical-engineer/maintenance/jobs',
        icon: 'event',
        color: 'primary'
      },
      {
        id: 'notif-002',
        type: 'SERVICE_ALERT',
        title: 'Service Alert',
        message: 'Loader CAT 966M approaching service threshold. Maintenance required soon.',
        date: new Date(Date.now() - 4 * 60 * 60 * 1000), // 4 hours ago
        read: false,
        priority: 'HIGH',
        relatedMachineId: 'LD-103',
        actionUrl: '/mechanical-engineer/maintenance/jobs',
        icon: 'warning',
        color: 'warning'
      },
      {
        id: 'notif-003',
        type: 'LOW_STOCK',
        title: 'Low Stock Alert',
        message: 'Low stock alert: Hydraulic Filter below threshold. Only 5 units remaining.',
        date: new Date(Date.now() - 6 * 60 * 60 * 1000), // 6 hours ago
        read: false,
        priority: 'HIGH',
        relatedAccessoryId: 'SP-001',
        actionUrl: '/mechanical-engineer/dashboard',
        icon: 'inventory_2',
        color: 'critical'
      },
      {
        id: 'notif-004',
        type: 'MAINTENANCE_COMPLETION',
        title: 'Maintenance Completed',
        message: 'Maintenance completed for Excavator Komatsu PC200.',
        date: new Date(Date.now() - 1 * 24 * 60 * 60 * 1000), // 1 day ago
        read: true,
        priority: 'LOW',
        relatedJobId: 'JOB-095',
        relatedMachineId: 'EX-005',
        actionUrl: '/mechanical-engineer/maintenance/jobs',
        icon: 'check_circle',
        color: 'success'
      },
      {
        id: 'notif-005',
        type: 'SCHEDULED_MAINTENANCE',
        title: 'Maintenance Scheduled',
        message: 'Machine Loader Caterpillar 980M scheduled for maintenance on 2024-02-10.',
        date: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000), // 2 days ago
        read: true,
        priority: 'MEDIUM',
        relatedJobId: 'JOB-102',
        relatedMachineId: 'LD-201',
        actionUrl: '/mechanical-engineer/maintenance/jobs',
        icon: 'event',
        color: 'primary'
      },
      {
        id: 'notif-006',
        type: 'LOW_STOCK',
        title: 'Low Stock Alert',
        message: 'Low stock alert: Engine Oil Filter below threshold.',
        date: new Date(Date.now() - 3 * 24 * 60 * 60 * 1000), // 3 days ago
        read: true,
        priority: 'MEDIUM',
        relatedAccessoryId: 'SP-002',
        actionUrl: '/mechanical-engineer/dashboard',
        icon: 'inventory_2',
        color: 'warning'
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
      case 'scheduled':
        filtered = allNotifications.filter(n => n.type === 'SCHEDULED_MAINTENANCE');
        break;
      case 'service-alert':
        filtered = allNotifications.filter(n => n.type === 'SERVICE_ALERT');
        break;
      case 'low-stock':
        filtered = allNotifications.filter(n => n.type === 'LOW_STOCK');
        break;
      case 'completion':
        filtered = allNotifications.filter(n => n.type === 'MAINTENANCE_COMPLETION');
        break;
      default:
        filtered = allNotifications;
    }

    this.filteredNotifications.set(filtered);
  }

  setFilter(filter: 'all' | 'unread' | 'scheduled' | 'service-alert' | 'low-stock' | 'completion') {
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
      if (notification.relatedJobId) {
        this.router.navigate([notification.actionUrl], {
          queryParams: { jobId: notification.relatedJobId }
        });
      } else {
        this.router.navigate([notification.actionUrl]);
      }
    }
  }

  toggleSettings() {
    this.showSettings.update(value => !value);
  }

  updateSetting(settingKey: keyof NotificationSettings, value: any) {
    this.settings.update(settings => ({
      ...settings,
      [settingKey]: value
    }));

    // In a real app, this would save to backend
    console.log('Settings updated:', this.settings());
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
