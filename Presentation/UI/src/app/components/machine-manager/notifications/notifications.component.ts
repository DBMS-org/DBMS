import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';

interface Notification {
  id: string;
  type: 'ASSIGNMENT_REQUEST' | 'MACHINE_ASSIGNED' | 'MAINTENANCE_SCHEDULED' | 'LOW_STOCK' | 'MACHINE_RETURNED';
  title: string;
  message: string;
  date: Date;
  read: boolean;
  priority: 'HIGH' | 'MEDIUM' | 'LOW';
  relatedMachineId?: string;
  relatedAccessoryId?: string;
  relatedRequestId?: string;
  actionUrl?: string;
  icon: string;
  color: string;
}

@Component({
  selector: 'app-machine-manager-notifications',
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
export class MachineManagerNotificationsComponent implements OnInit {
  private router = inject(Router);

  notifications = signal<Notification[]>([]);
  filteredNotifications = signal<Notification[]>([]);
  selectedFilter = signal<'all' | 'unread' | 'assignments' | 'maintenance' | 'inventory'>('all');
  lastUpdate = signal<Date>(new Date());
  isOnline = signal(true);

  ngOnInit() {
    this.loadNotifications();
    this.applyFilter();

    setInterval(() => {
      this.lastUpdate.set(new Date());
    }, 30000);
  }

  private loadNotifications() {
    const mockNotifications: Notification[] = [
      {
        id: 'notif-001',
        type: 'ASSIGNMENT_REQUEST',
        title: 'New Assignment Request',
        message: 'General Manager requested assignment of Drill Rig DR-102 for Mining Project Alpha.',
        date: new Date(Date.now() - 2 * 60 * 60 * 1000),
        read: false,
        priority: 'HIGH',
        relatedMachineId: 'DR-102',
        relatedRequestId: 'REQ-001',
        actionUrl: '/machine-manager/assignment-requests',
        icon: 'assignment_turned_in',
        color: 'primary'
      },
      {
        id: 'notif-002',
        type: 'MACHINE_ASSIGNED',
        title: 'Machine Assigned Successfully',
        message: 'Loader CAT 966M has been assigned to Project Beta.',
        date: new Date(Date.now() - 4 * 60 * 60 * 1000),
        read: false,
        priority: 'MEDIUM',
        relatedMachineId: 'LD-103',
        actionUrl: '/machine-manager/machine-inventory',
        icon: 'check_circle',
        color: 'success'
      },
      {
        id: 'notif-003',
        type: 'MAINTENANCE_SCHEDULED',
        title: 'Maintenance Scheduled',
        message: 'Excavator Komatsu PC200 scheduled for maintenance on 2024-02-25.',
        date: new Date(Date.now() - 6 * 60 * 60 * 1000),
        read: false,
        priority: 'MEDIUM',
        relatedMachineId: 'EX-005',
        actionUrl: '/machine-manager/maintenance-management',
        icon: 'build_circle',
        color: 'accent'
      },
      {
        id: 'notif-004',
        type: 'LOW_STOCK',
        title: 'Low Stock Alert',
        message: 'Hydraulic Filter stock is running low. Only 5 units remaining.',
        date: new Date(Date.now() - 1 * 24 * 60 * 60 * 1000),
        read: true,
        priority: 'HIGH',
        relatedAccessoryId: 'ACC-001',
        actionUrl: '/machine-manager/accessories-inventory',
        icon: 'inventory_2',
        color: 'warning'
      },
      {
        id: 'notif-005',
        type: 'MACHINE_RETURNED',
        title: 'Machine Returned',
        message: 'Drill Rig Atlas Copco has been returned from Project Gamma.',
        date: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000),
        read: true,
        priority: 'LOW',
        relatedMachineId: 'DR-102',
        actionUrl: '/machine-manager/machine-inventory',
        icon: 'assignment_return',
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
      case 'assignments':
        filtered = allNotifications.filter(n =>
          n.type === 'ASSIGNMENT_REQUEST' || n.type === 'MACHINE_ASSIGNED' || n.type === 'MACHINE_RETURNED'
        );
        break;
      case 'maintenance':
        filtered = allNotifications.filter(n => n.type === 'MAINTENANCE_SCHEDULED');
        break;
      case 'inventory':
        filtered = allNotifications.filter(n => n.type === 'LOW_STOCK');
        break;
      default:
        filtered = allNotifications;
    }

    this.filteredNotifications.set(filtered);
  }

  setFilter(filter: 'all' | 'unread' | 'assignments' | 'maintenance' | 'inventory') {
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
