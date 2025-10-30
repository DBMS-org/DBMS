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
  type: 'USER_MANAGEMENT' | 'PROJECT_UPDATE' | 'MACHINE_ASSIGNMENT' | 'SYSTEM_ALERT' | 'STORE_UPDATE';
  title: string;
  message: string;
  date: Date;
  read: boolean;
  priority: 'HIGH' | 'MEDIUM' | 'LOW';
  relatedUserId?: string;
  relatedProjectId?: string;
  relatedMachineId?: string;
  actionUrl?: string;
  icon: string;
  color: string;
}

interface NotificationSettings {
  userManagementPush: boolean;
  userManagementEmail: boolean;
  projectUpdatePush: boolean;
  projectUpdateEmail: boolean;
  machineAssignmentPush: boolean;
  machineAssignmentEmail: boolean;
  systemAlertPush: boolean;
  systemAlertEmail: boolean;
  storeUpdatePush: boolean;
  storeUpdateEmail: boolean;
  urgencyLevel: 'ALL' | 'HIGH' | 'CRITICAL';
}

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
export class NotificationsComponent implements OnInit {
  private router = inject(Router);

  // Notifications data
  notifications = signal<Notification[]>([]);
  filteredNotifications = signal<Notification[]>([]);

  // Filter state
  selectedFilter = signal<'all' | 'unread' | 'users' | 'projects' | 'machines' | 'system'>('all');

  // Settings
  settings = signal<NotificationSettings>({
    userManagementPush: true,
    userManagementEmail: true,
    projectUpdatePush: true,
    projectUpdateEmail: true,
    machineAssignmentPush: true,
    machineAssignmentEmail: false,
    systemAlertPush: true,
    systemAlertEmail: true,
    storeUpdatePush: true,
    storeUpdateEmail: false,
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
        type: 'USER_MANAGEMENT',
        title: 'New User Registration',
        message: 'New operator "John Smith" has registered and requires approval.',
        date: new Date(Date.now() - 2 * 60 * 60 * 1000), // 2 hours ago
        read: false,
        priority: 'HIGH',
        relatedUserId: 'USR-101',
        actionUrl: '/admin/users',
        icon: 'person_add',
        color: 'primary'
      },
      {
        id: 'notif-002',
        type: 'PROJECT_UPDATE',
        title: 'Project Status Update',
        message: 'Project "Mining Site Alpha" status changed to In Progress.',
        date: new Date(Date.now() - 4 * 60 * 60 * 1000), // 4 hours ago
        read: false,
        priority: 'MEDIUM',
        relatedProjectId: 'PRJ-205',
        actionUrl: '/admin/project-management',
        icon: 'work_update',
        color: 'info'
      },
      {
        id: 'notif-003',
        type: 'MACHINE_ASSIGNMENT',
        title: 'Machine Assignment Request',
        message: 'New assignment request for Drill Rig DR-102 from Site Manager.',
        date: new Date(Date.now() - 6 * 60 * 60 * 1000), // 6 hours ago
        read: false,
        priority: 'HIGH',
        relatedMachineId: 'MCH-102',
        actionUrl: '/admin/machine-assignments',
        icon: 'engineering',
        color: 'warning'
      },
      {
        id: 'notif-004',
        type: 'SYSTEM_ALERT',
        title: 'System Maintenance Scheduled',
        message: 'System maintenance scheduled for tonight at 02:00 AM.',
        date: new Date(Date.now() - 8 * 60 * 60 * 1000), // 8 hours ago
        read: true,
        priority: 'MEDIUM',
        actionUrl: '/admin/dashboard',
        icon: 'settings',
        color: 'accent'
      },
      {
        id: 'notif-005',
        type: 'STORE_UPDATE',
        title: 'Low Inventory Alert',
        message: 'Store "Central Warehouse" has low stock on hydraulic filters.',
        date: new Date(Date.now() - 1 * 24 * 60 * 60 * 1000), // 1 day ago
        read: true,
        priority: 'HIGH',
        actionUrl: '/admin/stores',
        icon: 'inventory',
        color: 'critical'
      },
      {
        id: 'notif-006',
        type: 'USER_MANAGEMENT',
        title: 'User Role Updated',
        message: 'User "Sarah Johnson" role changed to Machine Manager.',
        date: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000), // 2 days ago
        read: true,
        priority: 'LOW',
        relatedUserId: 'USR-305',
        actionUrl: '/admin/users',
        icon: 'admin_panel_settings',
        color: 'success'
      },
      {
        id: 'notif-007',
        type: 'PROJECT_UPDATE',
        title: 'New Project Created',
        message: 'New project "Quarry Expansion Phase 2" has been created.',
        date: new Date(Date.now() - 3 * 24 * 60 * 60 * 1000), // 3 days ago
        read: true,
        priority: 'MEDIUM',
        relatedProjectId: 'PRJ-310',
        actionUrl: '/admin/project-management',
        icon: 'add_business',
        color: 'primary'
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
      case 'users':
        filtered = allNotifications.filter(n => n.type === 'USER_MANAGEMENT');
        break;
      case 'projects':
        filtered = allNotifications.filter(n => n.type === 'PROJECT_UPDATE');
        break;
      case 'machines':
        filtered = allNotifications.filter(n => n.type === 'MACHINE_ASSIGNMENT');
        break;
      case 'system':
        filtered = allNotifications.filter(n => n.type === 'SYSTEM_ALERT' || n.type === 'STORE_UPDATE');
        break;
      default:
        filtered = allNotifications;
    }

    this.filteredNotifications.set(filtered);
  }

  setFilter(filter: 'all' | 'unread' | 'users' | 'projects' | 'machines' | 'system') {
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
      if (notification.relatedUserId) {
        this.router.navigate([notification.actionUrl]);
      } else if (notification.relatedProjectId) {
        this.router.navigate([notification.actionUrl]);
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
