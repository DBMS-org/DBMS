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