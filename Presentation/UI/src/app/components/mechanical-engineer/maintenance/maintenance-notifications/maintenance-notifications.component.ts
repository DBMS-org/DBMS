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
  type: 'ASSIGNMENT' | 'URGENT' | 'OVERDUE' | 'COMPLETION' | 'VERIFICATION' | 'GENERAL';
  trigger: 'NEW_ASSIGNMENT' | 'URGENT_BREAKDOWN' | 'OVERDUE_JOB' | 'JOB_COMPLETED' | 'VERIFICATION_REQUIRED' | 'GENERAL_UPDATE';
  title: string;
  message: string;
  date: Date;
  read: boolean;
  priority: 'HIGH' | 'MEDIUM' | 'LOW';
  relatedJobId?: string;
  relatedMachineId?: string;
  relatedReportId?: string;
  actionUrl?: string;
  icon: string;
  color: string;
}

interface NotificationSettings {
  newAssignmentPush: boolean;
  newAssignmentEmail: boolean;
  urgentBreakdownPush: boolean;
  urgentBreakdownEmail: boolean;
  overdueJobsPush: boolean;
  overdueJobsEmail: boolean;
  jobCompletionPush: boolean;
  jobCompletionEmail: boolean;
  verificationRequiredPush: boolean;
  verificationRequiredEmail: boolean;
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
  selectedFilter = signal<'all' | 'unread' | 'assignment' | 'urgent' | 'overdue' | 'completion'>('all');

  // Settings
  settings = signal<NotificationSettings>({
    newAssignmentPush: true,
    newAssignmentEmail: true,
    urgentBreakdownPush: true,
    urgentBreakdownEmail: true,
    overdueJobsPush: true,
    overdueJobsEmail: false,
    jobCompletionPush: true,
    jobCompletionEmail: false,
    verificationRequiredPush: true,
    verificationRequiredEmail: true,
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
        type: 'URGENT',
        trigger: 'URGENT_BREAKDOWN',
        title: 'Urgent Breakdown - Drill Rig DR-102',
        message: 'Hydraulic system failure detected on Drill Rig DR-102. Immediate attention required.',
        date: new Date(Date.now() - 2 * 60 * 60 * 1000), // 2 hours ago
        read: false,
        priority: 'HIGH',
        relatedJobId: 'JOB-001',
        relatedMachineId: 'DR-102',
        actionUrl: '/mechanical-engineer/maintenance/jobs',
        icon: 'error',
        color: 'critical'
      },
      {
        id: 'notif-002',
        type: 'ASSIGNMENT',
        trigger: 'NEW_ASSIGNMENT',
        title: 'New Job Assigned',
        message: 'You have been assigned to maintenance job MNT-2024-002 for Loader CAT 966M.',
        date: new Date(Date.now() - 4 * 60 * 60 * 1000), // 4 hours ago
        read: false,
        priority: 'MEDIUM',
        relatedJobId: 'JOB-002',
        relatedMachineId: 'LD-103',
        actionUrl: '/mechanical-engineer/maintenance/jobs',
        icon: 'assignment',
        color: 'primary'
      },
      {
        id: 'notif-003',
        type: 'OVERDUE',
        trigger: 'OVERDUE_JOB',
        title: 'Overdue Maintenance Job',
        message: 'Job MNT-2024-101 for Excavator EX-005 is overdue by 2 days.',
        date: new Date(Date.now() - 6 * 60 * 60 * 1000), // 6 hours ago
        read: false,
        priority: 'HIGH',
        relatedJobId: 'JOB-101',
        relatedMachineId: 'EX-005',
        actionUrl: '/mechanical-engineer/maintenance/jobs',
        icon: 'schedule',
        color: 'warning'
      },
      {
        id: 'notif-004',
        type: 'VERIFICATION',
        trigger: 'VERIFICATION_REQUIRED',
        title: 'Verification Required',
        message: 'Job MNT-2024-095 requires QA verification before closure.',
        date: new Date(Date.now() - 1 * 24 * 60 * 60 * 1000), // 1 day ago
        read: true,
        priority: 'MEDIUM',
        relatedJobId: 'JOB-095',
        actionUrl: '/mechanical-engineer/maintenance/jobs',
        icon: 'verified',
        color: 'accent'
      },
      {
        id: 'notif-005',
        type: 'COMPLETION',
        trigger: 'JOB_COMPLETED',
        title: 'Job Completed',
        message: 'Sarah Johnson completed maintenance job MNT-2024-102 for Drill Rig DR-102.',
        date: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000), // 2 days ago
        read: true,
        priority: 'LOW',
        relatedJobId: 'JOB-102',
        relatedMachineId: 'DR-102',
        actionUrl: '/mechanical-engineer/maintenance/jobs',
        icon: 'check_circle',
        color: 'success'
      },
      {
        id: 'notif-006',
        type: 'GENERAL',
        trigger: 'GENERAL_UPDATE',
        title: 'System Notification',
        message: 'Weekly maintenance report is now available for download.',
        date: new Date(Date.now() - 3 * 24 * 60 * 60 * 1000), // 3 days ago
        read: true,
        priority: 'LOW',
        relatedReportId: 'RPT-001',
        actionUrl: '/mechanical-engineer/reports',
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
      case 'assignment':
        filtered = allNotifications.filter(n => n.type === 'ASSIGNMENT');
        break;
      case 'urgent':
        filtered = allNotifications.filter(n => n.type === 'URGENT');
        break;
      case 'overdue':
        filtered = allNotifications.filter(n => n.type === 'OVERDUE');
        break;
      case 'completion':
        filtered = allNotifications.filter(n => n.type === 'COMPLETION');
        break;
      default:
        filtered = allNotifications;
    }

    this.filteredNotifications.set(filtered);
  }

  setFilter(filter: 'all' | 'unread' | 'assignment' | 'urgent' | 'overdue' | 'completion') {
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
