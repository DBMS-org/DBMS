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
  type: 'PROJECT_UPDATE' | 'SITE_UPDATE' | 'DRILL_DATA' | 'BLAST_CALCULATION' | 'PROPOSAL_STATUS';
  title: string;
  message: string;
  date: Date;
  read: boolean;
  priority: 'HIGH' | 'MEDIUM' | 'LOW';
  relatedProjectId?: string;
  relatedSiteId?: string;
  relatedProposalId?: string;
  actionUrl?: string;
  icon: string;
  color: string;
}

interface NotificationSettings {
  projectUpdatePush: boolean;
  projectUpdateEmail: boolean;
  siteUpdatePush: boolean;
  siteUpdateEmail: boolean;
  drillDataPush: boolean;
  drillDataEmail: boolean;
  blastCalculationPush: boolean;
  blastCalculationEmail: boolean;
  proposalStatusPush: boolean;
  proposalStatusEmail: boolean;
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
  selectedFilter = signal<'all' | 'unread' | 'projects' | 'sites' | 'drill-data' | 'proposals'>('all');

  // Settings
  settings = signal<NotificationSettings>({
    projectUpdatePush: true,
    projectUpdateEmail: true,
    siteUpdatePush: true,
    siteUpdateEmail: true,
    drillDataPush: true,
    drillDataEmail: false,
    blastCalculationPush: true,
    blastCalculationEmail: true,
    proposalStatusPush: true,
    proposalStatusEmail: false,
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
        type: 'PROJECT_UPDATE',
        title: 'Project Assignment',
        message: 'You have been assigned to project "Mining Site Expansion Phase 3".',
        date: new Date(Date.now() - 2 * 60 * 60 * 1000), // 2 hours ago
        read: false,
        priority: 'HIGH',
        relatedProjectId: 'PRJ-305',
        actionUrl: '/blasting-engineer/project-management',
        icon: 'work',
        color: 'primary'
      },
      {
        id: 'notif-002',
        type: 'SITE_UPDATE',
        title: 'New Site Added',
        message: 'New site "Quarry Block C" added to project Alpha Mining.',
        date: new Date(Date.now() - 4 * 60 * 60 * 1000), // 4 hours ago
        read: false,
        priority: 'MEDIUM',
        relatedProjectId: 'PRJ-201',
        relatedSiteId: 'SITE-456',
        actionUrl: '/blasting-engineer/project-management',
        icon: 'place',
        color: 'info'
      },
      {
        id: 'notif-003',
        type: 'DRILL_DATA',
        title: 'Drill Data Uploaded',
        message: 'New drill pattern data (150 holes) uploaded for Site A.',
        date: new Date(Date.now() - 6 * 60 * 60 * 1000), // 6 hours ago
        read: false,
        priority: 'HIGH',
        relatedSiteId: 'SITE-101',
        actionUrl: '/blasting-engineer/drill-visualization',
        icon: 'scatter_plot',
        color: 'success'
      },
      {
        id: 'notif-004',
        type: 'BLAST_CALCULATION',
        title: 'Calculation Completed',
        message: 'Explosive calculations completed for drilling pattern DP-2024-001.',
        date: new Date(Date.now() - 8 * 60 * 60 * 1000), // 8 hours ago
        read: true,
        priority: 'MEDIUM',
        actionUrl: '/blasting-engineer/explosive-calculations',
        icon: 'calculate',
        color: 'accent'
      },
      {
        id: 'notif-005',
        type: 'PROPOSAL_STATUS',
        title: 'Proposal Approved',
        message: 'Your blast proposal for Site B has been approved by management.',
        date: new Date(Date.now() - 1 * 24 * 60 * 60 * 1000), // 1 day ago
        read: true,
        priority: 'HIGH',
        relatedProposalId: 'PROP-789',
        actionUrl: '/blasting-engineer/proposal-history',
        icon: 'check_circle',
        color: 'success'
      },
      {
        id: 'notif-006',
        type: 'DRILL_DATA',
        title: 'Data Quality Alert',
        message: 'Drill data quality check failed for some entries in Site C.',
        date: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000), // 2 days ago
        read: true,
        priority: 'HIGH',
        relatedSiteId: 'SITE-203',
        actionUrl: '/blasting-engineer/csv-upload',
        icon: 'warning',
        color: 'warning'
      },
      {
        id: 'notif-007',
        type: 'PROJECT_UPDATE',
        title: 'Project Status Changed',
        message: 'Project "Limestone Quarry Development" status changed to Active.',
        date: new Date(Date.now() - 3 * 24 * 60 * 60 * 1000), // 3 days ago
        read: true,
        priority: 'LOW',
        relatedProjectId: 'PRJ-150',
        actionUrl: '/blasting-engineer/project-management',
        icon: 'update',
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
      case 'projects':
        filtered = allNotifications.filter(n => n.type === 'PROJECT_UPDATE');
        break;
      case 'sites':
        filtered = allNotifications.filter(n => n.type === 'SITE_UPDATE');
        break;
      case 'drill-data':
        filtered = allNotifications.filter(n => n.type === 'DRILL_DATA');
        break;
      case 'proposals':
        filtered = allNotifications.filter(n => n.type === 'PROPOSAL_STATUS' || n.type === 'BLAST_CALCULATION');
        break;
      default:
        filtered = allNotifications;
    }

    this.filteredNotifications.set(filtered);
  }

  setFilter(filter: 'all' | 'unread' | 'projects' | 'sites' | 'drill-data' | 'proposals') {
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
