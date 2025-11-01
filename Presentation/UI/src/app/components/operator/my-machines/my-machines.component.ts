import { Component, OnInit, signal, inject, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
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
  MachineUsageLog,
  CreateUsageLogRequest,
  UsageLogFormData,
  UsageLogUtils,
  UsageLogValidation
} from './models/usage-log.models';

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
    
    // Convert time strings to decimal hours
    const engineHours = this.timeStringToDecimal(formData.engineHours);
    const idleHours = this.timeStringToDecimal(formData.idleHours);
    const workingHours = this.timeStringToDecimal(formData.workingHours);
    const downtimeHours = formData.hasDowntime ? this.timeStringToDecimal(formData.downtimeHours) : 0;
    
    const usageLogData: CreateUsageLogRequest = {
        machineId: formData.selectedMachineId,
        logDate: formData.logDate,
        engineHours: engineHours.toString(),
        idleHours: idleHours.toString(),
        workingHours: workingHours.toString(),
        fuelConsumed: formData.fuelConsumed || 0,
        hasDowntime: formData.hasDowntime,
        downtimeHours: downtimeHours.toString(),
        breakdownDescription: formData.breakdownDescription || '',
        remarks: formData.remarks || '',
        attachments: this.selectedFiles
      };
    
    // Simulate API call - in real app, this would be a service call
    setTimeout(() => {
      // Update local usage metrics
      const currentMetrics = this.usageMetrics();
      if (currentMetrics) {
        const updatedMetrics: UsageMetrics = {
          ...currentMetrics,
          engineHours: currentMetrics.engineHours + engineHours,
          idleHours: currentMetrics.idleHours + idleHours,
          serviceHours: currentMetrics.serviceHours + engineHours, // Engine hours contribute to service hours
          lastUpdated: new Date()
        };
        this.usageMetrics.set(updatedMetrics);
      }
      
      this.isSubmittingUsage = false;
      this.selectedFiles = [];
      this.dialog.closeAll();
      this.snackBar.open('Usage log submitted successfully!', 'OK', { duration: 3000 });
      
      // Reset form
      this.initializeUsageLogForm();
    }, 2000);
  }
  
  private timeStringToDecimal(timeStr: string): number {
    if (!timeStr) return 0;
    const [hours, minutes] = timeStr.split(':').map(Number);
    return hours + (minutes / 60);
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
    
    this.usageLogForm = this.fb.group({
      selectedMachineId: [machine?.id || '', [Validators.required]],
      logDate: [currentDate, [Validators.required]],
      engineHours: ['', [Validators.required, Validators.pattern('^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$')]],
      idleHours: ['', [Validators.required, Validators.pattern('^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$')]],
      workingHours: ['', [Validators.required, Validators.pattern('^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$')]],
      fuelConsumed: ['', [Validators.min(0)]],
      hasDowntime: [false],
      downtimeHours: [''],
      breakdownDescription: [''],
      remarks: ['']
    });
    
    // Add conditional validators for downtime fields
    this.usageLogForm.get('hasDowntime')?.valueChanges.subscribe(hasDowntime => {
      const downtimeHoursControl = this.usageLogForm.get('downtimeHours');
      const breakdownDescControl = this.usageLogForm.get('breakdownDescription');
      
      if (hasDowntime) {
        downtimeHoursControl?.setValidators([Validators.required, Validators.pattern('^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$')]);
        breakdownDescControl?.setValidators([Validators.required, Validators.minLength(10)]);
      } else {
        downtimeHoursControl?.clearValidators();
        breakdownDescControl?.clearValidators();
      }
      
      downtimeHoursControl?.updateValueAndValidity();
      breakdownDescControl?.updateValueAndValidity();
    });
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

  private loadUsageMetrics(machineId: string) {
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
}
