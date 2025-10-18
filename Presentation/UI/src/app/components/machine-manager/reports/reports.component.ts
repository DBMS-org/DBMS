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

interface MachineReport {
  id: string;
  name: string;
  description: string;
  icon: string;
  category: 'machine-utilization' | 'assignment-history' | 'inventory-status' | 'maintenance-costs';
}

interface ReportFilters {
  machine?: string;
  dateFrom?: Date | null;
  dateTo?: Date | null;
  reportType?: string;
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
  selectedReport = signal<MachineReport | null>(null);
  isGenerating = signal(false);
  showFilters = signal(false);

  // Filter form
  filterForm!: FormGroup;

  // Available reports
  reports = signal<MachineReport[]>([]);

  // Dropdown options
  machines = signal<any[]>([]);
  reportTypes = ['All Machines', 'By Location', 'By Status', 'By Age'];

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
      reportType: ['']
    });
  }

  private loadReports() {
    const machineReports: MachineReport[] = [
      {
        id: 'report-1',
        name: 'Machine Utilization Report',
        description: 'Track machine usage, idle time, and operational efficiency metrics',
        icon: 'insights',
        category: 'machine-utilization'
      },
      {
        id: 'report-2',
        name: 'Assignment History Report',
        description: 'Complete history of machine assignments, operators, and project allocations',
        icon: 'history',
        category: 'assignment-history'
      },
      {
        id: 'report-3',
        name: 'Inventory Status Report',
        description: 'Current inventory levels, machine availability, and location tracking',
        icon: 'inventory_2',
        category: 'inventory-status'
      },
      {
        id: 'report-4',
        name: 'Maintenance Costs Report',
        description: 'Analysis of maintenance expenses, part costs, and budget tracking',
        icon: 'payments',
        category: 'maintenance-costs'
      }
    ];
    this.reports.set(machineReports);
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

  selectReport(report: MachineReport) {
    this.selectedReport.set(report);
    this.showFilters.set(true);
  }

  exportReport(format: 'pdf' | 'csv', report?: MachineReport) {
    const selectedRep = report || this.selectedReport();
    if (!selectedRep) {
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
        `${selectedRep.name} exported as ${format.toUpperCase()}`,
        'Close',
        {
          duration: 3000,
          panelClass: ['success-snackbar']
        }
      );

      // In real implementation, trigger download here
      console.log('Exporting report:', {
        report: selectedRep.name,
        format: format,
        filters: this.filterForm.value
      });
    }, 1500);
  }

  generateReport() {
    this.exportReport('pdf');
  }

  refreshReports() {
    this.loadReports();
    this.snackBar.open('Reports refreshed', 'Close', {
      duration: 2000
    });
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
