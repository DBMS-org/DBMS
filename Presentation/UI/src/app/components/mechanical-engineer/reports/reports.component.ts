import { Component, OnInit, signal, inject } from '@angular/core';
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

interface MaintenanceReport {
  id: string;
  name: string;
  description: string;
  icon: string;
  category: 'maintenance-summary' | 'accessory-usage' | 'service-alerts' | 'maintenance-history';
}

interface ReportFilters {
  machine?: string;
  dateFrom?: Date | null;
  dateTo?: Date | null;
  maintenanceType?: string;
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
    MatSnackBarModule
  ],
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private snackBar = inject(MatSnackBar);

  // Component state
  isLoading = signal(false);
  selectedReport = signal<MaintenanceReport | null>(null);
  isGenerating = signal(false);
  showFilters = signal(false);

  // Filter form
  filterForm!: FormGroup;

  // Available reports
  reports = signal<MaintenanceReport[]>([]);

  // Dropdown options
  machines = signal<any[]>([]);
  maintenanceTypes = ['Preventive', 'Corrective', 'Predictive', 'Emergency'];

  ngOnInit() {
    this.initializeFilterForm();
    this.loadReports();
    this.loadMachines();
  }

  private initializeFilterForm() {
    this.filterForm = this.fb.group({
      machine: [''],
      dateFrom: [null],
      dateTo: [null],
      maintenanceType: ['']
    });
  }

  private loadReports() {
    const maintenanceReports: MaintenanceReport[] = [
      {
        id: 'report-1',
        name: 'Maintenance Summary Report',
        description: 'Overview of scheduled vs completed vs pending maintenance activities',
        icon: 'summarize',
        category: 'maintenance-summary'
      },
      {
        id: 'report-2',
        name: 'Accessory Usage Report',
        description: 'Accessories and spare parts used during maintenance operations',
        icon: 'inventory',
        category: 'accessory-usage'
      },
      {
        id: 'report-3',
        name: 'Service Alerts Report',
        description: 'Record of all service alerts received and their resolution status',
        icon: 'notifications_active',
        category: 'service-alerts'
      },
      {
        id: 'report-4',
        name: 'Maintenance History Report',
        description: 'Detailed maintenance logs filtered by date, machine, or type',
        icon: 'history',
        category: 'maintenance-history'
      }
    ];
    this.reports.set(maintenanceReports);
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

  selectReport(report: MaintenanceReport) {
    this.selectedReport.set(report);
    this.showFilters.set(true);
  }

  exportReport(format: 'pdf' | 'csv') {
    const report = this.selectedReport();
    if (!report) {
      this.snackBar.open('Please select a report first', 'Close', {
        duration: 3000,
        panelClass: ['error-snackbar']
      });
      return;
    }

    this.isGenerating.set(true);

    // Simulate report generation
    setTimeout(() => {
      this.isGenerating.set(false);

      this.snackBar.open(
        `${report.name} exported as ${format.toUpperCase()}`,
        'Close',
        {
          duration: 3000,
          panelClass: ['success-snackbar']
        }
      );

      // In real implementation, trigger download here
      console.log('Exporting report:', {
        report: report.name,
        format: format,
        filters: this.filterForm.value
      });
    }, 1500);
  }

  resetFilters() {
    this.filterForm.reset();
  }

  closeFilters() {
    this.showFilters.set(false);
    this.selectedReport.set(null);
  }

  hasNoData(): boolean {
    // In real implementation, check if filters return empty dataset
    return false;
  }
}
