import { Component, OnInit, signal, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatCheckboxModule } from '@angular/material/checkbox';

interface PrebuiltReport {
  id: string;
  name: string;
  description: string;
  icon: string;
  category: 'maintenance' | 'downtime' | 'parts' | 'labor' | 'cost';
  type: 'maintenance-history' | 'machine-downtime' | 'part-replacements' | 'labor-hours' | 'repair-costs';
}

interface ReportFilter {
  dateFrom?: Date | null;
  dateTo?: Date | null;
  machineId?: string;
  priority?: string;
  status?: string;
  assignee?: string;
}

interface ScheduledReport {
  id: string;
  reportType: string;
  reportName: string;
  frequency: 'daily' | 'weekly' | 'monthly';
  recipients: string[];
  lastRun?: Date;
  nextRun: Date;
  format: 'csv' | 'excel' | 'pdf';
  active: boolean;
}

interface GeneratedReport {
  id: string;
  name: string;
  type: string;
  generatedAt: Date;
  downloadUrl: string;
  format: 'csv' | 'excel' | 'pdf';
  fileSize: string;
  status: 'ready' | 'generating' | 'failed';
}

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatTableModule,
    MatChipsModule,
    MatCheckboxModule
  ],
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private snackBar = inject(MatSnackBar);

  // Component state
  isLoading = signal(false);
  activeTab = signal<'prebuilt' | 'builder' | 'scheduled' | 'history'>('prebuilt');
  selectedReport = signal<PrebuiltReport | null>(null);
  isGenerating = signal(false);

  // Report builder form
  reportBuilderForm!: FormGroup;

  // Data signals
  prebuiltReports = signal<PrebuiltReport[]>([]);
  scheduledReports = signal<ScheduledReport[]>([]);
  generatedReports = signal<GeneratedReport[]>([]);

  // Dropdown options
  machines = signal<any[]>([]);
  priorities = ['HIGH', 'MEDIUM', 'LOW'];
  statuses = ['PENDING', 'IN_PROGRESS', 'COMPLETED', 'CANCELLED'];
  assignees = signal<string[]>([]);
  aggregateFunctions = ['Count', 'Sum', 'Average', 'Min', 'Max'];
  exportFormats = ['csv', 'excel', 'pdf'];

  // Computed values
  filteredPrebuiltReports = computed(() => {
    return this.prebuiltReports();
  });

  ngOnInit() {
    this.initializeReportBuilderForm();
    this.loadPrebuiltReports();
    this.loadScheduledReports();
    this.loadGeneratedReports();
    this.loadMachines();
    this.loadAssignees();
  }

  private initializeReportBuilderForm() {
    this.reportBuilderForm = this.fb.group({
      reportName: [''],
      reportType: ['custom'],
      dateFrom: [null],
      dateTo: [null],
      machineId: [''],
      priority: [''],
      status: [''],
      assignee: [''],
      aggregateFunction: [''],
      groupBy: [''],
      exportFormat: ['csv']
    });
  }

  private loadPrebuiltReports() {
    const reports: PrebuiltReport[] = [
      {
        id: 'report-1',
        name: 'Maintenance History',
        description: 'Complete history of all maintenance activities including jobs, parts replaced, and labor hours',
        icon: 'history',
        category: 'maintenance',
        type: 'maintenance-history'
      },
      {
        id: 'report-2',
        name: 'Machine Downtime Analysis',
        description: 'Detailed analysis of machine downtime by type, duration, and cause',
        icon: 'schedule',
        category: 'downtime',
        type: 'machine-downtime'
      },
      {
        id: 'report-3',
        name: 'Part Replacements & Usage',
        description: 'Track parts consumption, replacement frequency, and inventory usage patterns',
        icon: 'settings',
        category: 'parts',
        type: 'part-replacements'
      },
      {
        id: 'report-4',
        name: 'Labor Hours Report',
        description: 'Summary of labor hours spent on maintenance jobs by technician and machine',
        icon: 'access_time',
        category: 'labor',
        type: 'labor-hours'
      },
      {
        id: 'report-5',
        name: 'Cost of Repairs',
        description: 'Financial analysis of repair costs including parts, labor, and external services',
        icon: 'attach_money',
        category: 'cost',
        type: 'repair-costs'
      }
    ];
    this.prebuiltReports.set(reports);
  }

  private loadScheduledReports() {
    const scheduled: ScheduledReport[] = [
      {
        id: 'sched-1',
        reportType: 'maintenance-history',
        reportName: 'Weekly Maintenance Summary',
        frequency: 'weekly',
        recipients: ['manager@company.com', 'supervisor@company.com'],
        lastRun: new Date(Date.now() - 7 * 24 * 60 * 60 * 1000),
        nextRun: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000),
        format: 'pdf',
        active: true
      },
      {
        id: 'sched-2',
        reportType: 'machine-downtime',
        reportName: 'Monthly Downtime Analysis',
        frequency: 'monthly',
        recipients: ['director@company.com'],
        lastRun: new Date(Date.now() - 30 * 24 * 60 * 60 * 1000),
        nextRun: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000),
        format: 'excel',
        active: true
      }
    ];
    this.scheduledReports.set(scheduled);
  }

  private loadGeneratedReports() {
    const reports: GeneratedReport[] = [
      {
        id: 'gen-1',
        name: 'Maintenance History - October 2024',
        type: 'maintenance-history',
        generatedAt: new Date(),
        downloadUrl: '/api/reports/download/gen-1',
        format: 'pdf',
        fileSize: '2.4 MB',
        status: 'ready'
      },
      {
        id: 'gen-2',
        name: 'Machine Downtime - Q3 2024',
        type: 'machine-downtime',
        generatedAt: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000),
        downloadUrl: '/api/reports/download/gen-2',
        format: 'excel',
        fileSize: '1.8 MB',
        status: 'ready'
      }
    ];
    this.generatedReports.set(reports);
  }

  private loadMachines() {
    const machines = [
      { id: 'DR-102', name: 'Drill Rig Atlas Copco' },
      { id: 'LD-201', name: 'Loader Caterpillar 980M' },
      { id: 'LD-103', name: 'Loader CAT 966M' },
      { id: 'EX-005', name: 'Excavator Komatsu PC200' }
    ];
    this.machines.set(machines);
  }

  private loadAssignees() {
    const assignees = [
      'John Smith',
      'Sarah Johnson',
      'Mike Davis',
      'Emily Wilson'
    ];
    this.assignees.set(assignees);
  }

  // Tab navigation
  setActiveTab(tab: 'prebuilt' | 'builder' | 'scheduled' | 'history') {
    this.activeTab.set(tab);
  }

  // Prebuilt report actions
  selectPrebuiltReport(report: PrebuiltReport) {
    this.selectedReport.set(report);
  }

  generatePrebuiltReport(report: PrebuiltReport, format: 'csv' | 'excel' | 'pdf') {
    this.isGenerating.set(true);

    // Simulate report generation
    setTimeout(() => {
      const newReport: GeneratedReport = {
        id: `gen-${Date.now()}`,
        name: `${report.name} - ${new Date().toLocaleDateString()}`,
        type: report.type,
        generatedAt: new Date(),
        downloadUrl: `/api/reports/download/gen-${Date.now()}`,
        format: format,
        fileSize: '1.5 MB',
        status: 'ready'
      };

      this.generatedReports.update(reports => [newReport, ...reports]);
      this.isGenerating.set(false);

      this.snackBar.open('Report generated successfully!', 'Close', {
        duration: 3000,
        panelClass: ['success-snackbar']
      });

      // Switch to history tab to show the generated report
      this.activeTab.set('history');
    }, 2000);
  }

  // Report builder actions
  generateCustomReport() {
    if (!this.reportBuilderForm.valid) {
      this.snackBar.open('Please fill in required fields', 'Close', {
        duration: 3000,
        panelClass: ['error-snackbar']
      });
      return;
    }

    this.isGenerating.set(true);
    const formValue = this.reportBuilderForm.value;

    // Simulate custom report generation
    setTimeout(() => {
      const newReport: GeneratedReport = {
        id: `gen-${Date.now()}`,
        name: formValue.reportName || `Custom Report - ${new Date().toLocaleDateString()}`,
        type: 'custom',
        generatedAt: new Date(),
        downloadUrl: `/api/reports/download/gen-${Date.now()}`,
        format: formValue.exportFormat,
        fileSize: '2.1 MB',
        status: 'ready'
      };

      this.generatedReports.update(reports => [newReport, ...reports]);
      this.isGenerating.set(false);

      this.snackBar.open('Custom report generated successfully!', 'Close', {
        duration: 3000,
        panelClass: ['success-snackbar']
      });

      // Reset form and switch to history tab
      this.reportBuilderForm.reset({ exportFormat: 'csv' });
      this.activeTab.set('history');
    }, 2500);
  }

  // Scheduled report actions
  toggleScheduledReport(report: ScheduledReport) {
    this.scheduledReports.update(reports =>
      reports.map(r => r.id === report.id ? { ...r, active: !r.active } : r)
    );

    const message = !report.active ? 'Report scheduled activated' : 'Report scheduled deactivated';
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      panelClass: ['success-snackbar']
    });
  }

  deleteScheduledReport(report: ScheduledReport) {
    this.scheduledReports.update(reports =>
      reports.filter(r => r.id !== report.id)
    );

    this.snackBar.open('Scheduled report deleted', 'Close', {
      duration: 3000,
      panelClass: ['success-snackbar']
    });
  }

  createScheduledReport() {
    // TODO: Open dialog to create scheduled report
    this.snackBar.open('Create scheduled report feature coming soon', 'Close', {
      duration: 3000,
      panelClass: ['info-snackbar']
    });
  }

  // Generated report actions
  downloadReport(report: GeneratedReport) {
    this.snackBar.open(`Downloading ${report.name}...`, 'Close', {
      duration: 2000,
      panelClass: ['info-snackbar']
    });

    // In a real app, this would trigger the actual download
    window.open(report.downloadUrl, '_blank');
  }

  deleteReport(report: GeneratedReport) {
    this.generatedReports.update(reports =>
      reports.filter(r => r.id !== report.id)
    );

    this.snackBar.open('Report deleted', 'Close', {
      duration: 3000,
      panelClass: ['success-snackbar']
    });
  }

  // Utility methods
  getCategoryIcon(category: string): string {
    const icons: Record<string, string> = {
      maintenance: 'build',
      downtime: 'schedule',
      parts: 'settings',
      labor: 'access_time',
      cost: 'attach_money'
    };
    return icons[category] || 'description';
  }

  getFormatIcon(format: string): string {
    const icons: Record<string, string> = {
      csv: 'table_chart',
      excel: 'grid_on',
      pdf: 'picture_as_pdf'
    };
    return icons[format] || 'file_download';
  }

  getStatusClass(status: string): string {
    const classes: Record<string, string> = {
      ready: 'status-ready',
      generating: 'status-generating',
      failed: 'status-failed'
    };
    return classes[status] || '';
  }
}
