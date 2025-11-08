import { Component, signal, output, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy } from '@angular/core';

// PrimeNG imports
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';

// Material Menu for status updates (keeping this as it's already used in project)
import { MatMenuModule } from '@angular/material/menu';

import { MaintenanceJob, MaintenanceStatus, MaintenanceType } from '../../models/maintenance.models';

@Component({
  selector: 'app-job-list',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    ButtonModule,
    TagModule,
    TooltipModule,
    MatMenuModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './job-list.component.html',
  styleUrls: ['./job-list.component.scss']
})
export class JobListComponent {
  // Inputs
  jobs = input<MaintenanceJob[]>([]);

  // Outputs
  jobSelected = output<MaintenanceJob>();
  jobStatusChanged = output<{ job: MaintenanceJob; status: MaintenanceStatus }>();
  bulkStatusChanged = output<{ jobs: MaintenanceJob[]; status: MaintenanceStatus }>();
  machineClicked = output<MaintenanceJob>();

  // State
  isLoading = signal(false);
  selectedJobs: MaintenanceJob[] = [];
  selectAll: boolean = false;

  // Available statuses for dropdown
  availableStatuses = [
    MaintenanceStatus.SCHEDULED,
    MaintenanceStatus.IN_PROGRESS,
    MaintenanceStatus.COMPLETED,
    MaintenanceStatus.CANCELLED,
    MaintenanceStatus.OVERDUE
  ];

  // Event handlers
  onJobSelected(job: MaintenanceJob) {
    this.jobSelected.emit(job);
  }

  onSelectAllChange(event: any) {
    this.selectAll = event;
  }

  onStatusChange(job: MaintenanceJob, status: MaintenanceStatus) {
    this.jobStatusChanged.emit({ job, status });
  }

  onBulkStatusChange(status: MaintenanceStatus) {
    if (this.selectedJobs.length > 0) {
      this.bulkStatusChanged.emit({ jobs: this.selectedJobs, status });
      this.clearSelection();
    }
  }

  clearSelection() {
    this.selectedJobs = [];
    this.selectAll = false;
  }

  // Utility Methods
  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString();
  }

  formatTime(date: Date): string {
    return new Date(date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }

  getStatusDisplayName(status: MaintenanceStatus): string {
    if (!status) return 'Unknown';
    const statusNames = {
      [MaintenanceStatus.SCHEDULED]: 'Scheduled',
      [MaintenanceStatus.IN_PROGRESS]: 'In Progress',
      [MaintenanceStatus.COMPLETED]: 'Completed',
      [MaintenanceStatus.CANCELLED]: 'Cancelled',
      [MaintenanceStatus.OVERDUE]: 'Overdue'
    };
    return statusNames[status] || 'Unknown';
  }

  getStatusIcon(status: MaintenanceStatus): string {
    if (!status) return 'pi-question-circle';
    const statusIcons = {
      [MaintenanceStatus.SCHEDULED]: 'pi-clock',
      [MaintenanceStatus.IN_PROGRESS]: 'pi-play-circle',
      [MaintenanceStatus.COMPLETED]: 'pi-check-circle',
      [MaintenanceStatus.CANCELLED]: 'pi-times-circle',
      [MaintenanceStatus.OVERDUE]: 'pi-exclamation-triangle'
    };
    return statusIcons[status] || 'pi-question-circle';
  }

  getStatusSeverity(status: MaintenanceStatus): "success" | "secondary" | "info" | "warning" | "danger" | "contrast" | undefined {
    if (!status) return 'secondary';
    const statusSeverity = {
      [MaintenanceStatus.SCHEDULED]: 'info',
      [MaintenanceStatus.IN_PROGRESS]: 'warning',
      [MaintenanceStatus.COMPLETED]: 'success',
      [MaintenanceStatus.CANCELLED]: 'secondary',
      [MaintenanceStatus.OVERDUE]: 'danger'
    };
    return statusSeverity[status] as any || 'secondary';
  }

  getTypeDisplayName(type: MaintenanceType): string {
    if (!type) return 'Unknown';
    const typeNames = {
      [MaintenanceType.PREVENTIVE]: 'Preventive',
      [MaintenanceType.CORRECTIVE]: 'Corrective',
      [MaintenanceType.PREDICTIVE]: 'Predictive',
      [MaintenanceType.EMERGENCY]: 'Emergency'
    };
    return typeNames[type] || 'Unknown';
  }

  getTypeSeverity(type: MaintenanceType): "success" | "secondary" | "info" | "warning" | "danger" | "contrast" | undefined {
    if (!type) return 'secondary';
    const typeSeverity = {
      [MaintenanceType.PREVENTIVE]: 'success',
      [MaintenanceType.CORRECTIVE]: 'warning',
      [MaintenanceType.PREDICTIVE]: 'info',
      [MaintenanceType.EMERGENCY]: 'danger'
    };
    return typeSeverity[type] as any || 'secondary';
  }

  getAssignedTooltip(assignedTo: string[]): string {
    return assignedTo.join(', ');
  }
}
