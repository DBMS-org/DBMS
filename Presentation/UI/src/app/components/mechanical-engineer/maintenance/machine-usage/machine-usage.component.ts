import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { MatChipsModule } from '@angular/material/chips';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { UsageLogViewerComponent } from '../shared/usage-log-viewer/usage-log-viewer.component';
import { MaintenanceService } from '../services/maintenance.service';
import { MaintenanceJob } from '../models/maintenance.models';
import { MachineService } from '../../../../core/services/machine.service';
import { Machine } from '../../../../core/models/machine.model';

/**
 * Machine Usage Page Component
 * Displays usage metrics and usage log history for a specific machine
 *
 * Route: /mechanical-engineer/maintenance/machine-usage/:machineId
 */
@Component({
  selector: 'app-machine-usage',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    MatChipsModule,
    MatExpansionModule,
    MatProgressBarModule,
    UsageLogViewerComponent
  ],
  templateUrl: './machine-usage.component.html',
  styleUrls: ['./machine-usage.component.scss']
})
export class MachineUsageComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private maintenanceService = inject(MaintenanceService);
  private machineService = inject(MachineService);

  // State
  machineId = signal<number | null>(null);
  machineName = signal<string>('');
  machineModel = signal<string>('');
  serialNumber = signal<string>('');
  isLoading = signal<boolean>(true);

  // Machine details including service configuration
  machine = signal<Machine | null>(null);

  // Usage statistics from backend
  totalEngineHours = signal<number>(0);
  totalIdleHours = signal<number>(0);
  totalWorkingHours = signal<number>(0);
  totalFuelConsumed = signal<number>(0);
  totalDowntimeHours = signal<number>(0);
  averageDailyHours = signal<number>(0);

  // Maintenance job history
  maintenanceJobs = signal<MaintenanceJob[]>([]);
  isLoadingJobs = signal<boolean>(false);

  // View filter: 'all' | 'jobs' | 'usage'
  activeView = signal<'all' | 'jobs' | 'usage'>('all');


  // Computed service progress values
  engineServiceProgress = computed(() => {
    const m = this.machine();
    if (!m?.engineServiceInterval || m.engineServiceInterval <= 0) return 0;
    const current = m.currentEngineServiceHours || 0;
    return Math.min(100, (current / m.engineServiceInterval) * 100);
  });

  engineServiceRemaining = computed(() => {
    const m = this.machine();
    if (!m?.engineServiceInterval) return 0;
    return Math.max(0, m.engineServiceInterval - (m.currentEngineServiceHours || 0));
  });

  drifterServiceProgress = computed(() => {
    const m = this.machine();
    if (!m?.drifterServiceInterval || m.drifterServiceInterval <= 0) return 0;
    const current = m.currentDrifterServiceHours || 0;
    return Math.min(100, (current / m.drifterServiceInterval) * 100);
  });

  drifterServiceRemaining = computed(() => {
    const m = this.machine();
    if (!m?.drifterServiceInterval) return 0;
    return Math.max(0, m.drifterServiceInterval - (m.currentDrifterServiceHours || 0));
  });

  hasDrifterService = computed(() => {
    const m = this.machine();
    return !!(m?.drifterServiceInterval && m.drifterServiceInterval > 0);
  });

  ngOnInit() {
    // Get machineId from route params
    this.route.params.subscribe(params => {
      const id = params['machineId'];
      if (id) {
        this.machineId.set(Number(id));
        this.loadMachineDetails(Number(id));
        this.loadUsageStatistics(Number(id));
        this.loadMaintenanceJobs(Number(id));
      }
    });

    // Get machine info from query params
    this.route.queryParams.subscribe(queryParams => {
      if (queryParams['name']) {
        this.machineName.set(queryParams['name']);
      }
      if (queryParams['model']) {
        this.machineModel.set(queryParams['model']);
      }
      if (queryParams['serial']) {
        this.serialNumber.set(queryParams['serial']);
      }
    });
  }

  private loadMachineDetails(machineId: number) {
    this.machineService.getMachineById(machineId).subscribe({
      next: (machine) => {
        this.machine.set(machine);
        if (machine.name) this.machineName.set(machine.name);
        if (machine.model) this.machineModel.set(machine.model);
        if (machine.serialNumber) this.serialNumber.set(machine.serialNumber);
      },
      error: (err) => {
        console.error('Error loading machine details:', err);
      }
    });
  }

  private loadUsageStatistics(machineId: number) {
    this.isLoading.set(true);

    // Get usage statistics from backend API
    this.maintenanceService.getUsageStatistics(machineId, 30).subscribe({
      next: (stats) => {
        if (stats) {
          if (stats.machineName) this.machineName.set(stats.machineName);
          this.totalEngineHours.set(stats.totalEngineHours || 0);
          this.totalIdleHours.set(stats.totalIdleHours || 0);
          this.totalWorkingHours.set(stats.totalWorkingHours || 0);
          this.totalFuelConsumed.set(stats.totalFuelConsumed || 0);
          this.totalDowntimeHours.set(stats.totalDowntimeHours || 0);
          this.averageDailyHours.set(stats.averageDailyHours || 0);
        }
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error loading usage statistics:', err);
        this.isLoading.set(false);
      }
    });
  }

  goBack() {
    this.router.navigate(['/mechanical-engineer/maintenance/jobs']);
  }

  private loadMaintenanceJobs(machineId: number) {
    this.isLoadingJobs.set(true);
    this.maintenanceService.getMaintenanceJobsByMachine(machineId).subscribe({
      next: (jobs) => {
        this.maintenanceJobs.set(jobs);
        this.isLoadingJobs.set(false);
      },
      error: (err) => {
        console.error('Error loading maintenance jobs:', err);
        this.isLoadingJobs.set(false);
      }
    });
  }

  getStatusClass(status: string): string {
    switch (status?.toLowerCase()) {
      case 'completed': return 'status-completed';
      case 'inprogress': return 'status-in-progress';
      case 'scheduled': return 'status-scheduled';
      case 'cancelled': return 'status-cancelled';
      default: return '';
    }
  }

  getTypeLabel(type: string): string {
    switch (type?.toLowerCase()) {
      case 'preventive': return 'Preventive';
      case 'corrective': return 'Corrective';
      case 'emergency': return 'Emergency';
      case 'predictive': return 'Predictive';
      default: return type || 'Unknown';
    }
  }

  formatDate(date: Date | string | undefined): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString();
  }

  parsePartsReplaced(partsJson: string | string[] | undefined): string[] {
    if (!partsJson) return [];
    if (Array.isArray(partsJson)) return partsJson;
    try {
      return JSON.parse(partsJson);
    } catch {
      return [];
    }
  }

  getServiceUrgencyClass(progress: number): string {
    if (progress >= 90) return 'urgent';
    if (progress >= 75) return 'warning';
    return 'normal';
  }

  setActiveView(view: 'all' | 'jobs' | 'usage') {
    this.activeView.set(view);
  }
}
