import { Component, OnInit, input, signal, inject, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSortModule } from '@angular/material/sort';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { MaintenanceService } from '../../services/maintenance.service';

/**
 * Usage Log Viewer Component
 * Reusable component to display machine usage history in table format
 *
 * Used by: Mechanical Engineer (in machine-usage page) and Machine Manager (in machine details)
 */
@Component({
  selector: 'app-usage-log-viewer',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatCardModule,
    MatIconModule,
    MatChipsModule,
    MatTooltipModule,
    MatSortModule,
    MatProgressSpinnerModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './usage-log-viewer.component.html',
  styleUrls: ['./usage-log-viewer.component.scss']
})
export class UsageLogViewerComponent implements OnInit {
  private maintenanceService = inject(MaintenanceService);

  // Inputs
  machineId = input.required<string | number>();
  displayDays = input<number>(30);

  // Signals
  usageLogs = signal<any[]>([]);
  isLoading = signal<boolean>(false);

  // Table columns
  displayedColumns: string[] = ['date', 'engineHours', 'workingHours', 'idleHours', 'fuel', 'downtime', 'status'];

  ngOnInit() {
    this.loadUsageLogs();
  }

  private loadUsageLogs() {
    this.isLoading.set(true);

    const machineId = Number(this.machineId());
    const days = this.displayDays();
    const endDate = new Date();
    const startDate = new Date();
    startDate.setDate(startDate.getDate() - days);

    this.maintenanceService.getUsageLogsByMachine(machineId, startDate, endDate).subscribe({
      next: (logs) => {
        this.usageLogs.set(logs || []);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error loading usage logs:', err);
        this.usageLogs.set([]);
        this.isLoading.set(false);
      }
    });
  }

  getStatusClass(status: string): string {
    switch (status?.toUpperCase()) {
      case 'SUBMITTED': return 'status-submitted';
      case 'APPROVED': return 'status-approved';
      case 'REJECTED': return 'status-rejected';
      case 'DRAFT': return 'status-draft';
      default: return '';
    }
  }

  getStatusLabel(status: string): string {
    switch (status?.toUpperCase()) {
      case 'SUBMITTED': return 'Submitted';
      case 'APPROVED': return 'Approved';
      case 'REJECTED': return 'Rejected';
      case 'DRAFT': return 'Draft';
      default: return status || 'Unknown';
    }
  }

  formatDate(date: Date | string): string {
    if (!date) return 'N/A';
    const d = new Date(date);
    return d.toLocaleDateString();
  }

  calculateEngineHours(log: any): number {
    return log.engineHoursDelta || 0;
  }
}
