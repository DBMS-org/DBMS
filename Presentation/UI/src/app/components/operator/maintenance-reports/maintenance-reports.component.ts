import { Component, OnInit, signal, inject, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

// Angular Material Imports
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { MaintenanceReportService } from './services/maintenance-report.service';
import { ReportDetailsDialogComponent } from './report-details-dialog/report-details-dialog.component';
import {
  ProblemReport,
  OperatorMachine,
  CreateProblemReportRequest
} from './models/maintenance-report.models';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-maintenance-reports',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatTooltipModule,
    MatSnackBarModule,
    MatDialogModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './maintenance-reports.component.html',
  styleUrls: ['./maintenance-reports.component.scss']
})
export class MaintenanceReportsComponent implements OnInit {
  showReportForm = false;
  operatorMachine = signal<OperatorMachine | null>(null);
  reports = signal<ProblemReport[]>([]);
  isLoading = signal(false);
  searchValue = '';

  newReport = {
    affectedPart: 'Other' as any,
    problemCategory: 'Other' as any,
    customDescription: '',
    symptoms: [] as string[],
    severity: 'Medium' as any
  };

  private maintenanceReportService = inject(MaintenanceReportService);
  private authService = inject(AuthService);
  private snackBar = inject(MatSnackBar);
  private dialog = inject(MatDialog);

  // Status tracking computed properties
  pendingCount = computed(() =>
    this.reports().filter(report =>
      report.status === 'REPORTED'
    ).length
  );

  inProgressCount = computed(() =>
    this.reports().filter(report =>
      report.status === 'ACKNOWLEDGED' || report.status === 'IN_PROGRESS'
    ).length
  );

  completedCount = computed(() =>
    this.reports().filter(report =>
      report.status === 'RESOLVED' || report.status === 'CLOSED'
    ).length
  );

  // Filtered reports based on search
  filteredReports = computed(() => {
    if (!this.searchValue) {
      return this.reports();
    }
    const search = this.searchValue.toLowerCase();
    return this.reports().filter(report =>
      report.customDescription.toLowerCase().includes(search) ||
      report.ticketId.toLowerCase().includes(search) ||
      this.getPartDisplay(report.affectedPart).toLowerCase().includes(search) ||
      this.getSeverityDisplay(report.severity).toLowerCase().includes(search) ||
      this.getStatusDisplay(report.status).toLowerCase().includes(search)
    );
  });

  // Dropdown options
  affectedPartOptions = computed(() => [
    { label: 'Drill Bit', value: 'DrillBit' },
    { label: 'Drill Rod', value: 'DrillRod' },
    { label: 'Shank', value: 'Shank' },
    { label: 'Engine', value: 'Engine' },
    { label: 'Hydraulic System', value: 'HydraulicSystem' },
    { label: 'Electrical System', value: 'ElectricalSystem' },
    { label: 'Mechanical Components', value: 'MechanicalComponents' },
    { label: 'Other', value: 'Other' }
  ]);

  problemCategoryOptions = computed(() => [
    { label: 'Engine Issues', value: 'EngineIssues' },
    { label: 'Hydraulic Problems', value: 'HydraulicProblems' },
    { label: 'Electrical Faults', value: 'ElectricalFaults' },
    { label: 'Mechanical Breakdown', value: 'MechanicalBreakdown' },
    { label: 'Drill Bit Issues', value: 'DrillBitIssues' },
    { label: 'Drill Rod Problems', value: 'DrillRodProblems' },
    { label: 'Other', value: 'Other' }
  ]);

  severityOptions = computed(() => [
    { label: 'Low - Maintenance Needed', value: 'Low' },
    { label: 'Medium - Minor Issues', value: 'Medium' },
    { label: 'High - Performance Issues', value: 'High' },
    { label: 'Critical - Machine Down', value: 'Critical' }
  ]);

  ngOnInit() {
    this.loadOperatorMachine();
    this.loadReports();
  }

  private async loadOperatorMachine() {
    try {
      this.isLoading.set(true);
      const currentUser = this.authService.getCurrentUser();
      if (currentUser?.id) {
        this.maintenanceReportService.getOperatorMachine(currentUser.id).subscribe({
          next: (machine) => this.operatorMachine.set(machine),
          error: (error) => console.error('Failed to load operator machine:', error)
        });
      }
    } catch (error) {
      console.error('Error loading operator machine:', error);
    } finally {
      this.isLoading.set(false);
    }
  }

  private async loadReports() {
    try {
      const currentUser = this.authService.getCurrentUser();
      if (currentUser?.id) {
        this.maintenanceReportService.getOperatorReports(currentUser.id).subscribe({
          next: (reports) => {
            console.log('=== LOADED REPORTS FROM API ===');
            console.log('Total reports:', reports.length);
            if (reports.length > 0) {
              console.log('First report sample:', reports[0]);
              console.log('First report - Project Name:', reports[0].projectName);
              console.log('First report - Region Name:', reports[0].regionName);
            }
            console.log('==============================');
            this.reports.set(reports);
          },
          error: (error) => console.error('Failed to load reports:', error)
        });
      }
    } catch (error) {
      console.error('Error loading reports:', error);
    }
  }

  toggleReportForm() {
    this.showReportForm = !this.showReportForm;
  }

  onSearch() {
    // Search is handled by the filteredReports computed signal
    // This method exists to trigger change detection on input events
  }

  async submitReport() {
    const currentUser = this.authService.getCurrentUser();
    const machine = this.operatorMachine();

    if (!currentUser || !machine) {
      this.snackBar.open('Unable to submit report. Please try again.', 'Close', {
        duration: 3000
      });
      return;
    }

    const reportData: CreateProblemReportRequest = {
      machineId: machine.id,
      machineName: machine.name,
      machineModel: machine.model,
      serialNumber: machine.serialNumber,
      location: machine.currentLocation,
      affectedPart: this.newReport.affectedPart,
      problemCategory: this.newReport.problemCategory,
      customDescription: this.newReport.customDescription,
      symptoms: this.newReport.symptoms,
      severity: this.newReport.severity
    };

    try {
      const report = await this.maintenanceReportService.submitProblemReport(reportData);
      this.reports.update(reports => [report, ...reports]);
      this.snackBar.open('Issue reported successfully! Machine Manager will review your request.', 'Close', {
        duration: 4000
      });
      this.resetReportForm();
      this.showReportForm = false;
    } catch (error: any) {
      console.error('Failed to submit issue:', error);
      this.snackBar.open('Failed to submit issue. Please try again.', 'Close', {
        duration: 3000
      });
    }
  }

  resetReportForm() {
    this.newReport = {
      affectedPart: 'Other' as any,
      problemCategory: 'Other' as any,
      customDescription: '',
      symptoms: [] as string[],
      severity: 'Medium' as any
    };
  }

  getPartDisplay(part: string): string {
    const partMap: { [key: string]: string } = {
      'DRILL_BIT': 'Drill Bit',
      'DrillBit': 'Drill Bit',
      'DRILL_ROD': 'Drill Rod',
      'DrillRod': 'Drill Rod',
      'SHANK': 'Shank',
      'Shank': 'Shank',
      'ENGINE': 'Engine',
      'Engine': 'Engine',
      'HYDRAULIC_SYSTEM': 'Hydraulic System',
      'HydraulicSystem': 'Hydraulic System',
      'ELECTRICAL_SYSTEM': 'Electrical System',
      'ElectricalSystem': 'Electrical System',
      'MECHANICAL_COMPONENTS': 'Mechanical Components',
      'MechanicalComponents': 'Mechanical Components',
      'OTHER': 'Other',
      'Other': 'Other'
    };
    return partMap[part] || part;
  }

  getSeverityDisplay(severity: string): string {
    const severityMap: { [key: string]: string } = {
      'LOW': 'Low',
      'Low': 'Low',
      'MEDIUM': 'Medium',
      'Medium': 'Medium',
      'HIGH': 'High',
      'High': 'High',
      'CRITICAL': 'Critical',
      'Critical': 'Critical'
    };
    return severityMap[severity] || severity;
  }

  getStatusIcon(status: string): string {
    const iconMap: { [key: string]: string } = {
      'REPORTED': 'schedule',
      'ACKNOWLEDGED': 'visibility',
      'IN_PROGRESS': 'sync',
      'RESOLVED': 'check_circle',
      'CLOSED': 'done'
    };
    return iconMap[status] || 'help';
  }

  getStatusDisplay(status: string): string {
    switch (status) {
      case 'REPORTED':
        return 'Pending';
      case 'ACKNOWLEDGED':
      case 'IN_PROGRESS':
        return 'In Progress';
      case 'RESOLVED':
      case 'CLOSED':
        return 'Completed';
      default:
        return status;
    }
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'REPORTED':
        return 'pending';
      case 'ACKNOWLEDGED':
      case 'IN_PROGRESS':
        return 'in-progress';
      case 'RESOLVED':
      case 'CLOSED':
        return 'completed';
      default:
        return 'pending';
    }
  }

  viewReportDetails(report: ProblemReport) {
    console.log('=== MAINTENANCE REPORT DETAILS ===');
    console.log('Full Report Object:', report);
    console.log('Machine Name:', report.machineName);
    console.log('Machine Model:', report.machineModel);
    console.log('Serial Number:', report.serialNumber);
    console.log('Location:', report.location);
    console.log('Project Name:', report.projectName);
    console.log('Project ID:', report.projectId);
    console.log('Region Name:', report.regionName);
    console.log('Region ID:', report.regionId);
    console.log('Operator Name:', report.operatorName);
    console.log('Operator Email:', report.operatorEmail);
    console.log('Operator Phone:', report.operatorPhone);
    console.log('==================================');

    this.dialog.open(ReportDetailsDialogComponent, {
      data: { report },
      width: '800px',
      maxWidth: '95vw',
      maxHeight: '90vh',
      autoFocus: false
    });
  }
}
