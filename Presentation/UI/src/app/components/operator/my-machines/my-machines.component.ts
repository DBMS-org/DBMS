import { Component, OnInit, signal, inject, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { MatBadgeModule } from '@angular/material/badge';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

import { AuthService } from '../../../core/services/auth.service';
import { MaintenanceReportService } from '../maintenance-reports/services/maintenance-report.service';
import { OperatorMachine, ProblemReport } from '../maintenance-reports/models/maintenance-report.models';
import { MachineReportDialogComponent } from './machine-report-dialog/machine-report-dialog.component';
import { ReportDetailsDialogComponent } from '../maintenance-reports/report-details-dialog/report-details-dialog.component';
import { MaintenanceService } from '../../mechanical-engineer/maintenance/services/maintenance.service';
import { UsageMetrics } from '../../mechanical-engineer/maintenance/models/maintenance.models';
import {
  CreateUsageLogRequest,
  UsageLogDto,
  UsageLogFormData,
  UsageLogUtils,
  UsageLogValidation,
  UsageStatisticsDto
} from './models/usage-log.models';
import { UsageLogService } from '../../../services/usage-log.service';

@Component({
  selector: 'app-my-machines',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    MatBadgeModule,
    MatChipsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatSnackBarModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ReactiveFormsModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './my-machines.component.html',
  styleUrls: ['./my-machines.component.scss']
})
export class MyMachinesComponent implements OnInit {
  assignedMachine = signal<OperatorMachine | null>(null);
  recentReports = signal<ProblemReport[]>([]);
  isLoading = signal(false);
  error = signal<string | null>(null);
  usageMetrics = signal<UsageMetrics | null>(null);
  serviceIntervalHours = signal<number>(500);

  // Enhanced usage logging properties
  usageLogForm!: FormGroup;
  selectedFiles: File[] = [];
  isSubmittingUsage = false;

  // New: Usage statistics and history
  usageStatistics = signal<UsageStatisticsDto | null>(null);
  usageLogs = signal<UsageLogDto[]>([]);
  showUsageHistory = signal(false);

  // New: Computed properties for validation
  engineHoursDelta = signal<number>(0);
  drifterHoursDelta = signal<number>(0);
  hoursValidationWarning = signal<string | null>(null);
  isDrillRig = signal<boolean>(false);

  // Legacy properties for backward compatibility
  engineDelta = 0;
  idleDelta = 0;
  serviceDelta = 0;
  
  private maintenanceReportService = inject(MaintenanceReportService);
  private authService = inject(AuthService);
  public dialog = inject(MatDialog);
  private maintenanceService = inject(MaintenanceService);
  private snackBar = inject(MatSnackBar);
  private fb = inject(FormBuilder);
  private usageLogService = inject(UsageLogService);
  private router = inject(Router);
  
  // Enhanced usage logging methods
  onFileSelected(event: any) {
    const files = event.target.files;
    if (files) {
      for (let file of files) {
        if (file.size <= 10 * 1024 * 1024) { // 10MB limit
          this.selectedFiles.push(file);
        } else {
          this.snackBar.open(`File ${file.name} is too large (max 10MB)`, 'OK', { duration: 3000 });
        }
      }
    }
  }
  
  removeFile(index: number) {
    this.selectedFiles.splice(index, 1);
  }
  
  getFileIcon(fileType: string): string {
    if (fileType.startsWith('image/')) return 'image';
    if (fileType.includes('pdf')) return 'picture_as_pdf';
    if (fileType.includes('doc')) return 'description';
    return 'attach_file';
  }
  
  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }
  
  submitEnhancedLogUsage() {
    if (!this.usageLogForm.valid || this.isSubmittingUsage) return;

    this.isSubmittingUsage = true;
    const formData = this.usageLogForm.value;
    const machine = this.assignedMachine();

    if (!machine) {
      this.snackBar.open('No machine assigned', 'OK', { duration: 3000 });
      this.isSubmittingUsage = false;
      return;
    }

    // Prepare request matching backend DTO
    const usageLogRequest: CreateUsageLogRequest = {
      machineId: machine.id,
      logDate: formData.logDate,
      engineHourStart: formData.engineHourStart,
      engineHourEnd: formData.engineHourEnd,
      drifterHourStart: this.isDrillRig() ? formData.drifterHourStart : undefined,
      drifterHourEnd: this.isDrillRig() ? formData.drifterHourEnd : undefined,
      idleHours: formData.idleHours || 0,
      workingHours: formData.workingHours || 0,
      fuelConsumed: formData.fuelConsumed || undefined,
      hasDowntime: formData.hasDowntime || false,
      downtimeHours: formData.hasDowntime ? (formData.downtimeHours || 0) : undefined,
      breakdownDescription: formData.hasDowntime ? formData.breakdownDescription : undefined,
      remarks: formData.remarks || undefined
    };

    // Call real API service
    this.usageLogService.createUsageLog(usageLogRequest).subscribe({
      next: (createdLog: UsageLogDto) => {
        // Add to local logs list
        const currentLogs = this.usageLogs();
        this.usageLogs.set([createdLog, ...currentLogs]);

        // Update local usage metrics
        const currentMetrics = this.usageMetrics();
        if (currentMetrics) {
          const updatedMetrics: UsageMetrics = {
            ...currentMetrics,
            engineHours: currentMetrics.engineHours + createdLog.engineHoursDelta,
            idleHours: currentMetrics.idleHours + createdLog.idleHours,
            serviceHours: currentMetrics.serviceHours + createdLog.engineHoursDelta,
            lastUpdated: new Date()
          };
          this.usageMetrics.set(updatedMetrics);
        }

        // Reload statistics
        this.loadUsageStatistics(machine.id);

        this.isSubmittingUsage = false;
        this.selectedFiles = [];
        this.dialog.closeAll();
        this.snackBar.open('Usage log submitted successfully!', 'OK', { duration: 3000 });

        // Reset form
        this.initializeUsageLogForm();
      },
      error: (error) => {
        console.error('Error submitting usage log:', error);
        this.isSubmittingUsage = false;

        const errorMessage = error?.error?.message || error?.message || 'Failed to submit usage log. Please try again.';
        this.snackBar.open(errorMessage, 'OK', { duration: 5000 });
      }
    });
  }

  /**
   * Load usage statistics from backend
   */
  private loadUsageStatistics(machineId: number, days: number = 30) {
    this.usageLogService.getUsageStatistics(machineId, days).subscribe({
      next: (stats) => {
        this.usageStatistics.set(stats);
      },
      error: (error) => {
        console.error('Error loading usage statistics:', error);
      }
    });
  }

  /**
   * Load usage history from backend
   */
  loadUsageHistory() {
    const machine = this.assignedMachine();
    if (!machine) return;

    const endDate = new Date();
    const startDate = new Date();
    startDate.setDate(startDate.getDate() - 30);

    this.usageLogService.getUsageLogsByMachine(machine.id, startDate, endDate).subscribe({
      next: (logs) => {
        this.usageLogs.set(logs);
      },
      error: (error) => {
        console.error('Error loading usage logs:', error);
      }
    });
  }

  /**
   * Pre-fill form with last usage log data
   */
  private prefillFormWithLastLog(machineId: number) {
    this.usageLogService.getLatestUsageLog(machineId).subscribe({
      next: (lastLog) => {
        console.log('Last usage log found:', lastLog);
        // Pre-fill engine hour start with last engine hour end
        this.usageLogForm.patchValue({
          engineHourStart: lastLog.engineHourEnd ?? 0,
          drifterHourStart: lastLog.drifterHourEnd ?? 0
        });
        // Trigger delta calculation
        this.calculateHourDeltas();
      },
      error: (err) => {
        console.log('No previous usage log found:', err?.message || 'Not found');
        // No previous log exists, that's fine - use defaults
      }
    });
  }

  activeReportsCount = computed(() => 
    this.recentReports().filter(report => 
      report.status === 'REPORTED' || 
      report.status === 'ACKNOWLEDGED' || 
      report.status === 'IN_PROGRESS'
    ).length
  );

  remainingServiceHours = computed(() => {
    const metrics = this.usageMetrics();
    if (!metrics) return null;
    const interval = this.serviceIntervalHours();
    const remainder = metrics.engineHours % interval;
    return Math.max(interval - remainder, 0);
  });
  
  ngOnInit() {
    this.loadData();
    this.initializeUsageLogForm();
  }
  
  private initializeUsageLogForm() {
    const currentDate = new Date();
    const machine = this.assignedMachine();
    const currentUser = this.authService.getCurrentUser();
    const siteEngineer = currentUser?.name || currentUser?.email || 'Unknown';

    // Check if machine is a drill rig
    const machineType = machine?.type?.toLowerCase() || '';
    this.isDrillRig.set(machineType.includes('drill') || machineType.includes('rig'));

    this.usageLogForm = this.fb.group({
      selectedMachineId: [machine?.id || '', [Validators.required]],
      logDate: [currentDate, [Validators.required]],
      siteEngineer: [{value: siteEngineer, disabled: true}],

      // New: Start/End tracking
      engineHourStart: [0, [Validators.required, Validators.min(0)]],
      engineHourEnd: [0, [Validators.required, Validators.min(0)]],
      drifterHourStart: [0, [Validators.min(0)]],
      drifterHourEnd: [0, [Validators.min(0)]],

      // Legacy fields (still needed for backward compatibility)
      engineHours: [null, [Validators.required, Validators.min(0)]],
      idleHours: [null, [Validators.required, Validators.min(0)]],
      workingHours: [null, [Validators.required, Validators.min(0)]],
      fuelConsumed: [null, [Validators.min(0)]],
      hasDowntime: [false],
      downtimeHours: [null],
      breakdownDescription: [''],
      remarks: ['']
    });

    // Setup hours calculations
    this.setupHoursCalculations();

    // Add conditional validators for downtime fields
    this.usageLogForm.get('hasDowntime')?.valueChanges.subscribe(hasDowntime => {
      const downtimeHoursControl = this.usageLogForm.get('downtimeHours');
      const breakdownDescControl = this.usageLogForm.get('breakdownDescription');

      if (hasDowntime) {
        downtimeHoursControl?.setValidators([Validators.required, Validators.min(0)]);
        breakdownDescControl?.setValidators([Validators.required, Validators.minLength(10)]);
      } else {
        downtimeHoursControl?.clearValidators();
        breakdownDescControl?.clearValidators();
      }

      downtimeHoursControl?.updateValueAndValidity();
      breakdownDescControl?.updateValueAndValidity();
    });
  }
  
  private setupHoursCalculations() {
    // Watch engine hour changes
    this.usageLogForm.get('engineHourStart')?.valueChanges.subscribe(() => this.calculateHourDeltas());
    this.usageLogForm.get('engineHourEnd')?.valueChanges.subscribe(() => this.calculateHourDeltas());

    // Watch drifter hour changes
    this.usageLogForm.get('drifterHourStart')?.valueChanges.subscribe(() => this.calculateHourDeltas());
    this.usageLogForm.get('drifterHourEnd')?.valueChanges.subscribe(() => this.calculateHourDeltas());

    // Watch idle and working hours for validation
    this.usageLogForm.get('idleHours')?.valueChanges.subscribe(() => this.checkHoursValidation());
    this.usageLogForm.get('workingHours')?.valueChanges.subscribe(() => this.checkHoursValidation());
  }

  private calculateHourDeltas() {
    const startEngine = this.usageLogForm.get('engineHourStart')?.value || 0;
    const endEngine = this.usageLogForm.get('engineHourEnd')?.value || 0;
    const engineDelta = Math.max(0, endEngine - startEngine);
    this.engineHoursDelta.set(engineDelta);

    if (this.isDrillRig()) {
      const startDrifter = this.usageLogForm.get('drifterHourStart')?.value || 0;
      const endDrifter = this.usageLogForm.get('drifterHourEnd')?.value || 0;
      const drifterDelta = Math.max(0, endDrifter - startDrifter);
      this.drifterHoursDelta.set(drifterDelta);
    }

    // Auto-update legacy engineHours field
    this.usageLogForm.patchValue({ engineHours: engineDelta }, { emitEvent: false });

    this.checkHoursValidation();
  }

  private checkHoursValidation() {
    const engineDelta = this.engineHoursDelta();
    const idleHours = this.usageLogForm.get('idleHours')?.value || 0;
    const workingHours = this.usageLogForm.get('workingHours')?.value || 0;
    const totalOperational = idleHours + workingHours;

    const difference = Math.abs(engineDelta - totalOperational);

    if (difference > 0.5 && engineDelta > 0) {
      this.hoursValidationWarning.set(
        `Note: Idle + Working hours (${totalOperational.toFixed(1)}) doesn't match Engine hours (${engineDelta.toFixed(1)})`
      );
    } else {
      this.hoursValidationWarning.set(null);
    }
  }

  toggleUsageHistory() {
    this.showUsageHistory.set(!this.showUsageHistory());
  }

  refreshData() {
    this.loadData();
  }
  
  private loadData() {
    this.isLoading.set(true);
    this.error.set(null);
    
    const currentUser = this.authService.getCurrentUser();
    if (!currentUser?.id) {
      this.error.set('User information not available. Please log in again.');
      this.isLoading.set(false);
      return;
    }
    
    // Load assigned machine
    this.maintenanceReportService.getOperatorMachine(currentUser.id).subscribe({
      next: (machine) => {
        this.assignedMachine.set(machine);
        if (machine?.id) {
          this.loadUsageMetrics(machine.id);
          // Load usage data from backend
          this.loadUsageStatistics(machine.id);
          this.loadUsageHistory();
          // Update form with machine info first
          this.initializeUsageLogForm();
          // Ensure machine is pre-selected
          this.usageLogForm.patchValue({ selectedMachineId: machine.id });
          // Prefill engine hours from last log (must be after form init)
          this.prefillFormWithLastLog(machine.id);
        }
        this.loadReports(currentUser.id);
      },
      error: (err) => {
        console.error('Failed to load operator machine:', err);
        if (err.status === 404) {
          this.error.set('You do not have any machine assigned to you.');
        } else {
          this.error.set('Failed to load your assigned machine. Please try again.');
        }
        this.isLoading.set(false);
      }
    });
  }

  private loadUsageMetrics(machineId: number) {
    this.maintenanceService.getUsageMetrics(machineId).subscribe({
      next: (metricsArr) => {
        const m = metricsArr && metricsArr.length ? metricsArr[0] : null;
        if (m) this.usageMetrics.set(m);
        // Try to load interval config (optional); fallback to default 500h if fails
        this.maintenanceService.getServiceIntervalConfigs().subscribe({
          next: (configs) => {
            // Without machineType, keep default; could enhance by matching model keywords
            const defaultInterval = 500;
            this.serviceIntervalHours.set(defaultInterval);
          },
          error: () => {
            this.serviceIntervalHours.set(500);
          }
        });
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load usage metrics:', err);
        this.usageMetrics.set(null);
        this.isLoading.set(false);
      }
    });
  }
  
  private loadReports(operatorId: number | string) {
    this.maintenanceReportService.getOperatorReports(operatorId).subscribe({
      next: (reports) => {
        // Sort by date, newest first
        const sortedReports = reports.sort((a, b) => 
          new Date(b.reportedAt).getTime() - new Date(a.reportedAt).getTime()
        );
        this.recentReports.set(sortedReports);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load machine data:', err);
        this.error.set('Failed to load your assigned machine. Please try again.');
        this.isLoading.set(false);
      }
    });
  }
  
  submitLogUsage() {
    if (!this.assignedMachine()) return;
    const m = this.usageMetrics();
    const updated: UsageMetrics = {
      machineId: this.assignedMachine()!.id,
      engineHours: (m?.engineHours || 0) + (this.engineDelta || 0),
      idleHours: (m?.idleHours || 0) + (this.idleDelta || 0),
      serviceHours: (m?.serviceHours || 0) + (this.serviceDelta || 0),
      lastUpdated: new Date()
    };
    this.usageMetrics.set(updated);

    // persist locally
    try {
      const key = `machine_usage_logs_${updated.machineId}`;
      const existing = localStorage.getItem(key);
      const arr = existing ? JSON.parse(existing) : [];
      arr.push({
        engineDelta: this.engineDelta,
        idleDelta: this.idleDelta,
        serviceDelta: this.serviceDelta,
        timestamp: new Date().toISOString()
      });
      localStorage.setItem(key, JSON.stringify(arr));
    } catch {}

    this.dialog.closeAll();
    this.engineDelta = 0;
    this.idleDelta = 0;
    this.serviceDelta = 0;
    this.snackBar.open('Usage logged successfully', 'OK', { duration: 2500 });
  }
  
  reportIssue() {
    if (!this.assignedMachine()) return;

    const dialogRef = this.dialog.open(MachineReportDialogComponent, {
      width: '600px',
      data: { machine: this.assignedMachine() }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.refreshData();
      }
    });
  }

  viewReportDetails(report: ProblemReport) {
    this.dialog.open(ReportDetailsDialogComponent, {
      width: '800px',
      maxWidth: '90vw',
      data: { report }
    });
  }
  
  getMachineStatusClass(status: string): string {
    switch (status) {
      case 'OPERATIONAL': return 'status-operational';
      case 'UNDER_MAINTENANCE': return 'status-under-maintenance';
      case 'DOWN_FOR_REPAIR': return 'status-down-for-repair';
      case 'OFFLINE': return 'status-offline';
      default: return '';
    }
  }
  
  getMachineStatusLabel(status: string): string {
    switch (status) {
      case 'OPERATIONAL': return 'Operational';
      case 'UNDER_MAINTENANCE': return 'Under Maintenance';
      case 'DOWN_FOR_REPAIR': return 'Down for Repair';
      case 'OFFLINE': return 'Offline';
      default: return status;
    }
  }
  
  getReportStatusClass(status: string): string {
    switch (status) {
      case 'REPORTED': return 'status-reported';
      case 'ACKNOWLEDGED': return 'status-acknowledged';
      case 'IN_PROGRESS': return 'status-in-progress';
      case 'RESOLVED': return 'status-resolved';
      case 'CLOSED': return 'status-closed';
      default: return '';
    }
  }
  
  getReportStatusLabel(status: string): string {
    switch (status) {
      case 'REPORTED': return 'Reported';
      case 'ACKNOWLEDGED': return 'Acknowledged';
      case 'IN_PROGRESS': return 'In Progress';
      case 'RESOLVED': return 'Resolved';
      case 'CLOSED': return 'Closed';
      default: return status;
    }
  }
  
  getProblemCategoryLabel(category: string): string {
    switch (category) {
      case 'ENGINE_ISSUES': return 'Engine Issues';
      case 'HYDRAULIC_PROBLEMS': return 'Hydraulic Problems';
      case 'ELECTRICAL_FAULTS': return 'Electrical Faults';
      case 'MECHANICAL_BREAKDOWN': return 'Mechanical Breakdown';
      case 'DRILL_BIT_ISSUES': return 'Drill Bit Issues';
      case 'DRILL_ROD_PROBLEMS': return 'Drill Rod Problems';
      case 'OTHER': return 'Other Issue';
      default: return category;
    }
  }
  
  formatDate(date: Date | string): string {
    const d = new Date(date);
    return d.toLocaleDateString() + ' ' + d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }

  getUsageLogStatusClass(status: string): string {
    switch (status) {
      case 'SUBMITTED': return 'status-submitted';
      case 'APPROVED': return 'status-approved';
      case 'REJECTED': return 'status-rejected';
      case 'DRAFT': return 'status-draft';
      default: return '';
    }
  }

  getUsageLogStatusLabel(status: string): string {
    switch (status) {
      case 'SUBMITTED': return 'Submitted';
      case 'APPROVED': return 'Approved';
      case 'REJECTED': return 'Rejected';
      case 'DRAFT': return 'Draft';
      default: return status;
    }
  }

  navigateToUsageDetails(): void {
    const machine = this.assignedMachine();
    if (!machine?.id) return;

    this.router.navigate(
      ['/operator/machine-usage', machine.id],
      {
        queryParams: {
          name: machine.name,
          model: machine.model,
          serial: machine.serialNumber
        }
      }
    );
  }
}
