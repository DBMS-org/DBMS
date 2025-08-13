import { Component, input, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaintenanceAlert, AlertType, Priority } from '../../models/maintenance.models';

@Component({
  selector: 'app-maintenance-alerts',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './maintenance-alerts.component.html',
  styleUrl: './maintenance-alerts.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MaintenanceAlertsComponent {
  serviceDueAlerts = input.required<MaintenanceAlert[]>();
  overdueAlerts = input.required<MaintenanceAlert[]>();

  // Expose enums for template use
  readonly AlertType = AlertType;
  readonly Priority = Priority;

  getSortedServiceDueAlerts(): MaintenanceAlert[] {
    return [...this.serviceDueAlerts()].sort((a, b) => {
      // Sort by due date ascending (earliest first)
      return a.dueDate.getTime() - b.dueDate.getTime();
    });
  }

  getSortedOverdueAlerts(): MaintenanceAlert[] {
    return [...this.overdueAlerts()].sort((a, b) => {
      // Sort by days past due descending (most overdue first)
      return (b.daysPastDue || 0) - (a.daysPastDue || 0);
    });
  }

  getAlertIcon(alertType: AlertType): string {
    switch (alertType) {
      case AlertType.SERVICE_DUE:
        return 'schedule';
      case AlertType.OVERDUE:
        return 'warning';
      default:
        return 'info';
    }
  }

  getPriorityClass(priority: Priority): string {
    return `priority-${priority.toLowerCase()}`;
  }

  getAlertTypeClass(alertType: AlertType): string {
    return alertType === AlertType.OVERDUE ? 'overdue' : 'service-due';
  }

  formatDaysMessage(alert: MaintenanceAlert): string {
    if (alert.alertType === AlertType.OVERDUE) {
      const days = alert.daysPastDue || 0;
      return days === 1 ? '1 day overdue' : `${days} days overdue`;
    } else {
      const days = alert.daysUntilDue || 0;
      return days === 1 ? 'Due in 1 day' : `Due in ${days} days`;
    }
  }
}