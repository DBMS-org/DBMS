import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

interface QuickAction {
  id: string;
  title: string;
  description: string;
  icon: string;
  route: string;
  statusText?: string;
  statusCount?: number;
  colorClass: string;
}

@Component({
  selector: 'app-maintenance-quick-actions',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './maintenance-quick-actions.component.html',
  styleUrl: './maintenance-quick-actions.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MaintenanceQuickActionsComponent {
  private router = inject(Router);

  quickActions: QuickAction[] = [
    {
      id: 'schedule',
      title: 'Schedule Maintenance',
      description: 'Create and schedule new maintenance jobs for machines',
      icon: 'add_task',
      route: '/mechanical-engineer/maintenance/schedule',
      statusText: 'pending tasks',
      statusCount: 15,
      colorClass: 'schedule-action'
    },
    {
      id: 'calendar',
      title: 'View Calendar',
      description: 'See maintenance schedule in calendar and timeline views',
      icon: 'calendar_view_month',
      route: '/mechanical-engineer/maintenance/calendar',
      statusText: 'scheduled today',
      statusCount: 6,
      colorClass: 'calendar-action'
    },
    {
      id: 'jobs',
      title: 'Manage Jobs',
      description: 'View, filter, and update maintenance job statuses',
      icon: 'engineering',
      route: '/mechanical-engineer/maintenance/jobs',
      statusText: 'active jobs',
      statusCount: 23,
      colorClass: 'jobs-action'
    },
    {
      id: 'analytics',
      title: 'View Analytics',
      description: 'Maintenance performance metrics and compliance reports',
      icon: 'analytics',
      route: '/mechanical-engineer/maintenance/analytics',
      statusText: 'machines tracked',
      statusCount: 42,
      colorClass: 'analytics-action'
    },
    {
      id: 'inventory',
      title: 'Machine Inventory',
      description: 'View and manage machine inventory and status',
      icon: 'inventory_2',
      route: '/mechanical-engineer/maintenance/inventory',
      statusText: 'machines',
      statusCount: 42,
      colorClass: 'inventory-action'
    },
    {
      id: 'settings',
      title: 'Settings',
      description: 'Configure maintenance parameters and notifications',
      icon: 'settings',
      route: '/mechanical-engineer/maintenance/settings',
      colorClass: 'settings-action'
    }
  ];

  navigateToAction(action: QuickAction): void {
    this.router.navigate([action.route]);
  }

  onActionClick(action: QuickAction): void {
    this.navigateToAction(action);
  }

  onActionKeydown(event: KeyboardEvent, action: QuickAction): void {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      this.navigateToAction(action);
    }
  }
}