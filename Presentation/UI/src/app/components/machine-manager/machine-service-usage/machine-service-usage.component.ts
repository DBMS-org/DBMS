import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Machine } from '../../../core/models/machine.model';
import { MachineService } from '../../../core/services/machine.service';
import { UsageLogViewerComponent } from '../../mechanical-engineer/maintenance/shared/usage-log-viewer/usage-log-viewer.component';
import { MaintenanceJobService, MaintenanceJobDto } from '../../../features/maintenance-management/maintenance-job.service';

@Component({
  selector: 'app-machine-service-usage',
  standalone: true,
  imports: [CommonModule, UsageLogViewerComponent],
  templateUrl: './machine-service-usage.component.html',
  styleUrls: ['./machine-service-usage.component.scss']
})
export class MachineServiceUsageComponent implements OnInit {
  machine: Machine | null = null;
  maintenanceJobs: MaintenanceJobDto[] = [];
  isLoading = true;
  isLoadingJobs = true;
  errorMessage = '';
  machineId: number = 0;

  // Filter tabs
  activeTab: 'service' | 'jobs' | 'usage' = 'service';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private machineService: MachineService,
    private maintenanceJobService: MaintenanceJobService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.machineId = +id;
      this.loadMachine(this.machineId);
      this.loadMaintenanceJobs(this.machineId);
    } else {
      this.errorMessage = 'Invalid machine ID';
      this.isLoading = false;
    }
  }

  loadMachine(id: number): void {
    this.machineService.getMachineById(id).subscribe({
      next: (machine) => {
        this.machine = machine;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load machine details';
        console.error('Error loading machine:', err);
        this.isLoading = false;
      }
    });
  }

  loadMaintenanceJobs(machineId: number): void {
    this.maintenanceJobService.getAllJobs({ machineId }).subscribe({
      next: (jobs) => {
        this.maintenanceJobs = jobs;
        this.isLoadingJobs = false;
      },
      error: (err) => {
        console.error('Error loading maintenance jobs:', err);
        this.isLoadingJobs = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/machine-manager/machine-inventory']);
  }

  // Service tracking methods
  getEngineServiceProgress(): number {
    if (!this.machine?.engineServiceInterval || this.machine.engineServiceInterval <= 0) return 0;
    const current = this.machine.currentEngineServiceHours || 0;
    const interval = this.machine.engineServiceInterval;
    return Math.min((current / interval) * 100, 100);
  }

  getEngineHoursRemaining(): number {
    if (!this.machine?.engineServiceInterval) return 0;
    const current = this.machine.currentEngineServiceHours || 0;
    const interval = this.machine.engineServiceInterval;
    return interval - current;
  }

  getDrifterServiceProgress(): number {
    if (!this.machine?.drifterServiceInterval || this.machine.drifterServiceInterval <= 0) return 0;
    const current = this.machine.currentDrifterServiceHours || 0;
    const interval = this.machine.drifterServiceInterval;
    return Math.min((current / interval) * 100, 100);
  }

  getDrifterHoursRemaining(): number {
    if (!this.machine?.drifterServiceInterval) return 0;
    const current = this.machine.currentDrifterServiceHours || 0;
    const interval = this.machine.drifterServiceInterval;
    return interval - current;
  }

  getServiceUrgencyClass(hoursRemaining: number): string {
    if (hoursRemaining < 0) return 'urgency-overdue';
    if (hoursRemaining < 20) return 'urgency-critical';
    if (hoursRemaining < 50) return 'urgency-warning';
    if (hoursRemaining < 100) return 'urgency-caution';
    return 'urgency-good';
  }

  hasEngineService(): boolean {
    return !!(this.machine?.engineServiceInterval && this.machine.engineServiceInterval > 0);
  }

  hasDrifterService(): boolean {
    return !!(this.machine?.drifterServiceInterval && this.machine.drifterServiceInterval > 0);
  }

  formatDate(date: Date | string | undefined): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Scheduled': return 'status-scheduled';
      case 'InProgress': return 'status-in-progress';
      case 'Completed': return 'status-completed';
      case 'Overdue': return 'status-overdue';
      case 'Cancelled': return 'status-cancelled';
      default: return '';
    }
  }

  getTypeClass(type: string): string {
    return 'type-' + type.toLowerCase();
  }
}
