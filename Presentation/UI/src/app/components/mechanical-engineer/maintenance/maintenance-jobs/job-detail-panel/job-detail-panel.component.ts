import { Component, OnInit, inject, signal, computed, input, output, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTabsModule } from '@angular/material/tabs';
import { ChangeDetectionStrategy } from '@angular/core';

import { MaintenanceJob, MachineMaintenanceHistory, MaintenanceRecord, MaintenanceStatus, MaintenanceType } from '../../models/maintenance.models';
import { MaintenanceService } from '../../services/maintenance.service';

/**
 * Job Detail Panel Component - Mechanical Engineer View
 *
 * PERMISSIONS:
 * - READ ONLY for job details
 * - CAN UPDATE job status only (via consolidated button)
 * - CANNOT EDIT job details
 *
 * FEATURES:
 * - Machine overview with usage metrics
 * - Maintenance history in left sidebar
 * - Job information and progress
 * - Single consolidated status update button
 */
@Component({
  selector: 'app-job-detail-panel',
  standalone: true,
  imports: [
    CommonModule,
    MatSidenavModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatChipsModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    MatTabsModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './job-detail-panel.component.html',
  styleUrls: ['./job-detail-panel.component.scss']
})
export class JobDetailPanelComponent implements OnInit {
  private maintenanceService = inject(MaintenanceService);
  private router = inject(Router);

  // === INPUTS ===
  selectedJob = input<MaintenanceJob | null>(null);
  isOpen = input<boolean>(false);

  // === OUTPUTS ===
  // REMOVED: editJob - Mechanical Engineer cannot edit jobs
  panelClosed = output<void>();
  updateStatus = output<MaintenanceJob>(); // Only status update allowed

  // === STATE SIGNALS ===
  machineHistory = signal<MachineMaintenanceHistory | null>(null);
  isLoadingHistory = signal<boolean>(false);

  // === COMPUTED VALUES ===
  maintenanceRecords = computed(() => {
    const history = this.machineHistory();
    return history?.maintenanceRecords || [];
  });

  constructor() {
    // Watch for selected job changes and load history
    effect(() => {
      const job = this.selectedJob();
      if (job) {
        this.loadMachineHistory(job);
      } else {
        this.machineHistory.set(null);
      }
    });
  }

  ngOnInit() {
    // Initial load handled by effect
  }

  // === DATA LOADING ===
  private loadMachineHistory(job: MaintenanceJob) {
    this.isLoadingHistory.set(true);

    this.maintenanceService.getMachineMaintenanceHistory(job.machineId).subscribe({
      next: (history) => {
        // Check if history has actual data (backend returns empty object when not ready)
        const hasData = history && (history.machineName || history.model || history.serialNumber);
        this.machineHistory.set(hasData ? history : null);
        this.isLoadingHistory.set(false);
      },
      error: (error) => {
        console.error('Error loading machine history:', error);
        this.machineHistory.set(null);
        this.isLoadingHistory.set(false);
      }
    });
  }

  // === EVENT HANDLERS ===
  onOpenedChange(opened: boolean) {
    if (!opened) {
      this.panelClosed.emit();
    }
  }

  closePanel() {
    this.panelClosed.emit();
  }

  downloadAttachment(attachment: any) {
    // TODO: Implement file download
    console.log('Download attachment:', attachment);
  }

  // === FORMATTING UTILITIES ===
  formatDate(date: Date): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  formatDateTime(date: Date): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }

  // === STATUS DISPLAY UTILITIES ===
  getStatusDisplayName(status: MaintenanceStatus): string {
    const statusNames: Record<MaintenanceStatus, string> = {
      [MaintenanceStatus.SCHEDULED]: 'Scheduled',
      [MaintenanceStatus.IN_PROGRESS]: 'In Progress',
      [MaintenanceStatus.COMPLETED]: 'Completed',
      [MaintenanceStatus.CANCELLED]: 'Cancelled',
      [MaintenanceStatus.OVERDUE]: 'Overdue'
    };
    return statusNames[status] || status;
  }

  getStatusIcon(status: MaintenanceStatus): string {
    const statusIcons: Record<MaintenanceStatus, string> = {
      [MaintenanceStatus.SCHEDULED]: 'schedule',
      [MaintenanceStatus.IN_PROGRESS]: 'play_circle',
      [MaintenanceStatus.COMPLETED]: 'check_circle',
      [MaintenanceStatus.CANCELLED]: 'cancel',
      [MaintenanceStatus.OVERDUE]: 'warning'
    };
    return statusIcons[status] || 'info';
  }

  getStatusChipClass(status: MaintenanceStatus): string {
    if (!status) return 'status-unknown';
    // Convert PascalCase to kebab-case (e.g., InProgress -> in-progress)
    return `status-${status.replace(/([A-Z])/g, '-$1').toLowerCase().replace(/^-/, '')}`;
  }

  getStatusIconClass(status: MaintenanceStatus): string {
    if (!status) return 'icon-unknown';
    // Convert PascalCase to kebab-case (e.g., InProgress -> in-progress)
    return `icon-${status.replace(/([A-Z])/g, '-$1').toLowerCase().replace(/^-/, '')}`;
  }

  // === TYPE DISPLAY UTILITIES ===
  getTypeDisplayName(type: MaintenanceType): string {
    const typeNames: Record<MaintenanceType, string> = {
      [MaintenanceType.PREVENTIVE]: 'Preventive',
      [MaintenanceType.CORRECTIVE]: 'Corrective',
      [MaintenanceType.PREDICTIVE]: 'Predictive',
      [MaintenanceType.EMERGENCY]: 'Emergency'
    };
    return typeNames[type] || type;
  }

  getTypeIcon(type: MaintenanceType): string {
    const typeIcons: Record<MaintenanceType, string> = {
      [MaintenanceType.PREVENTIVE]: 'schedule',
      [MaintenanceType.CORRECTIVE]: 'build',
      [MaintenanceType.PREDICTIVE]: 'analytics',
      [MaintenanceType.EMERGENCY]: 'warning'
    };
    return typeIcons[type] || 'build';
  }

  getTypeChipClass(type: MaintenanceType): string {
    if (!type) return 'type-unknown';
    // Convert PascalCase to kebab-case (e.g., Preventive -> preventive)
    return `type-${type.replace(/([A-Z])/g, '-$1').toLowerCase().replace(/^-/, '')}`;
  }

  getTypeIconClass(type: MaintenanceType): string {
    if (!type) return 'type-icon-unknown';
    // Convert PascalCase to kebab-case (e.g., Preventive -> preventive)
    return `type-icon-${type.replace(/([A-Z])/g, '-$1').toLowerCase().replace(/^-/, '')}`;
  }

  // === FILE UTILITIES ===
  getFileIcon(fileType: string): string {
    if (fileType.startsWith('image/')) {
      return 'image';
    } else if (fileType === 'application/pdf') {
      return 'picture_as_pdf';
    } else if (fileType.startsWith('text/')) {
      return 'description';
    } else {
      return 'attach_file';
    }
  }

  // === MACHINE SERVICE UTILITIES ===
  getNextServiceClass(): string {
    const history = this.machineHistory();
    if (!history) return '';

    const nextServiceDate = new Date(history.nextServiceDate);
    const now = new Date();
    const daysUntilService = Math.ceil(
      (nextServiceDate.getTime() - now.getTime()) / (1000 * 60 * 60 * 24)
    );

    if (daysUntilService < 0) {
      return 'next-service-overdue';
    } else if (daysUntilService <= 7) {
      return 'next-service-due-soon';
    }
    return '';
  }

  getHoursSinceLastService(): number {
    const history = this.machineHistory();
    if (!history) return 0;

    // Calculate hours since last service based on engine hours and service hours
    return Math.max(0, history.engineHours - history.serviceHours);
  }

  // === CONSUMED COMPONENTS UTILITIES ===
  hasConsumedComponents(): boolean {
    const job = this.selectedJob();
    if (!job) return false;

    return !!(
      (job.drillBitsUsed && job.drillBitsUsed > 0) ||
      (job.drillRodsUsed && job.drillRodsUsed > 0) ||
      (job.shanksUsed && job.shanksUsed > 0)
    );
  }

  // === NAVIGATION ===
  navigateToMachineUsage(): void {
    const job = this.selectedJob();
    if (!job?.machineId) return;

    this.router.navigate(
      ['/mechanical-engineer/maintenance/machine-usage', job.machineId],
      {
        queryParams: {
          name: job.machineName,
          model: job.machineModel,
          serial: job.serialNumber
        }
      }
    );
  }
}
