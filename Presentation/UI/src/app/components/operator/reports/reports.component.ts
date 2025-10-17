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
import { AuthService } from '../../../core/services/auth.service';

interface OperatorReport {
  id: string;
  name: string;
  description: string;
  icon: string;
  category: 'drilling-progress' | 'work-summary' | 'site-completion' | 'efficiency-metrics';
}

@Component({
  selector: 'app-operator-reports',
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
export class OperatorReportsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private snackBar = inject(MatSnackBar);
  private authService = inject(AuthService);

  // Component state
  isLoading = signal(false);
  selectedReport = signal<OperatorReport | null>(null);
  isGenerating = signal(false);
  showFilters = signal(false);

  // Filter form
  filterForm!: FormGroup;

  // Available reports
  reports = signal<OperatorReport[]>([]);

  // Dropdown options
  sites = signal<any[]>([]);
  dateRangeOptions = ['Today', 'This Week', 'This Month', 'Last Month', 'Custom'];

  // Current user
  currentUser = computed(() => this.authService.getCurrentUser());

  ngOnInit() {
    this.initializeFilterForm();
    this.loadReports();
    this.loadSites();
  }

  private initializeFilterForm() {
    this.filterForm = this.fb.group({
      site: [''],
      dateFrom: [null],
      dateTo: [null],
      dateRange: ['']
    });
  }

  private loadReports() {
    const operatorReports: OperatorReport[] = [
      {
        id: 'report-1',
        name: 'Daily Drilling Progress Report',
        description: 'Track daily drilling operations, holes completed, and overall progress metrics',
        icon: 'trending_up',
        category: 'drilling-progress'
      },
      {
        id: 'report-2',
        name: 'Work Summary Report',
        description: 'Comprehensive summary of work completed, hours logged, and tasks performed',
        icon: 'summarize',
        category: 'work-summary'
      },
      {
        id: 'report-3',
        name: 'Site Completion Report',
        description: 'Detailed report of site completion status, patterns executed, and quality metrics',
        icon: 'task_alt',
        category: 'site-completion'
      },
      {
        id: 'report-4',
        name: 'Operator Efficiency Report',
        description: 'Personal efficiency metrics, machine utilization, and performance indicators',
        icon: 'speed',
        category: 'efficiency-metrics'
      }
    ];
    this.reports.set(operatorReports);
  }

  private loadSites() {
    // In real implementation, load from service
    const mockSites = [
      { id: 'site-1', name: 'Mining Site Alpha' },
      { id: 'site-2', name: 'Construction Zone Beta' },
      { id: 'site-3', name: 'Excavation Point Gamma' }
    ];
    this.sites.set(mockSites);
  }

  selectReport(report: OperatorReport) {
    this.selectedReport.set(report);
    this.showFilters.set(true);
  }

  exportReport(format: 'pdf' | 'csv', report?: OperatorReport) {
    const selectedRep = report || this.selectedReport();
    if (!selectedRep) {
      this.snackBar.open('Please select a report first', 'Close', {
        duration: 3000
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
          duration: 3000
        }
      );

      // In real implementation, trigger download here
      console.log('Exporting operator report:', {
        report: selectedRep.name,
        format: format,
        filters: this.filterForm.value,
        operator: this.currentUser()
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
