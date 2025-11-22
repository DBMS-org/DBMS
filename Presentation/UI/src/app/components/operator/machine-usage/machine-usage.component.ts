import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { UsageLogService } from '../../../services/usage-log.service';
import { UsageLogDto, UsageStatisticsDto } from '../my-machines/models/usage-log.models';
import { MachineService } from '../../../core/services/machine.service';
import { Machine } from '../../../core/models/machine.model';

@Component({
  selector: 'app-operator-machine-usage',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    MatTableModule,
    MatChipsModule,
    MatTooltipModule,
    MatProgressBarModule
  ],
  templateUrl: './machine-usage.component.html',
  styleUrls: ['./machine-usage.component.scss']
})
export class OperatorMachineUsageComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private usageLogService = inject(UsageLogService);
  private machineService = inject(MachineService);

  // State
  machineId = signal<number | null>(null);
  machineName = signal<string>('');
  machineModel = signal<string>('');
  serialNumber = signal<string>('');
  isLoading = signal<boolean>(true);

  // Machine details including service configuration
  machine = signal<Machine | null>(null);

  // Usage statistics
  usageStatistics = signal<UsageStatisticsDto | null>(null);

  // Usage logs
  usageLogs = signal<UsageLogDto[]>([]);

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

  // Table columns
  displayedColumns: string[] = ['date', 'engineHours', 'drifterHours', 'workingHours', 'idleHours', 'fuel', 'downtime', 'status'];

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = params['machineId'];
      if (id) {
        this.machineId.set(Number(id));
        this.loadData(Number(id));
      }
    });

    this.route.queryParams.subscribe(queryParams => {
      if (queryParams['name']) this.machineName.set(queryParams['name']);
      if (queryParams['model']) this.machineModel.set(queryParams['model']);
      if (queryParams['serial']) this.serialNumber.set(queryParams['serial']);
    });
  }

  private loadData(machineId: number) {
    this.isLoading.set(true);

    // Load machine details for service hours
    this.machineService.getMachineById(machineId).subscribe({
      next: (machine) => {
        this.machine.set(machine);
        if (machine.name) this.machineName.set(machine.name);
        if (machine.model) this.machineModel.set(machine.model);
        if (machine.serialNumber) this.serialNumber.set(machine.serialNumber);
      },
      error: (err) => console.error('Error loading machine details:', err)
    });

    // Load statistics
    this.usageLogService.getUsageStatistics(machineId, 30).subscribe({
      next: (stats) => {
        this.usageStatistics.set(stats);
        if (stats?.machineName) this.machineName.set(stats.machineName);
      },
      error: (err) => console.error('Error loading statistics:', err)
    });

    // Load usage logs
    const endDate = new Date();
    const startDate = new Date();
    startDate.setDate(startDate.getDate() - 30);

    this.usageLogService.getUsageLogsByMachine(machineId, startDate, endDate).subscribe({
      next: (logs) => {
        this.usageLogs.set(logs || []);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error loading usage logs:', err);
        this.isLoading.set(false);
      }
    });
  }

  goBack() {
    this.router.navigate(['/operator/my-machines']);
  }

  formatDate(date: Date | string): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString();
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

  // Efficiency calculation methods
  getUtilizationRate(): number {
    const stats = this.usageStatistics();
    if (!stats || stats.totalEngineHours === 0) return 0;
    return (stats.totalWorkingHours / stats.totalEngineHours) * 100;
  }

  getAvailability(): number {
    const stats = this.usageStatistics();
    if (!stats) return 0;
    const totalPossibleHours = stats.totalEngineHours + stats.totalDowntimeHours;
    if (totalPossibleHours === 0) return 100;
    return (stats.totalEngineHours / totalPossibleHours) * 100;
  }

  getFuelEfficiency(): number {
    const stats = this.usageStatistics();
    if (!stats || stats.totalWorkingHours === 0) return 0;
    return stats.totalFuelConsumed / stats.totalWorkingHours;
  }

  getFuelEfficiencyPercent(): number {
    // Normalize fuel efficiency to a percentage (assuming 20 L/h is max/poor)
    const efficiency = this.getFuelEfficiency();
    return Math.min(100, Math.max(0, (1 - efficiency / 20) * 100));
  }

  getServiceUrgencyClass(progress: number): string {
    if (progress >= 90) return 'urgent';
    if (progress >= 75) return 'warning';
    return 'normal';
  }
}
