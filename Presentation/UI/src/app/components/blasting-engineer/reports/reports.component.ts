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

interface BlastingEngineerReport {
  id: string;
  name: string;
  description: string;
  icon: string;
  category: 'blast-sequence' | 'explosive-usage' | 'site-progress' | 'blast-effectiveness' | 'safety-compliance' | 'explosive-requests';
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
  private authService = inject(AuthService);

  // Component state
  isLoading = signal(false);
  selectedReport = signal<BlastingEngineerReport | null>(null);
  isGenerating = signal(false);
  showFilters = signal(false);

  // Filter form
  filterForm!: FormGroup;

  // Available reports
  reports = signal<BlastingEngineerReport[]>([]);

  // Dropdown options
  projects = signal<any[]>([]);
  sites = signal<any[]>([]);
  dateRangeOptions = ['Today', 'This Week', 'This Month', 'Last Month', 'Custom'];

  // Current user
  currentUser = computed(() => this.authService.getCurrentUser());

  ngOnInit() {
    this.initializeFilterForm();
    this.loadReports();
    this.loadProjects();
    this.loadSites();
  }

  private initializeFilterForm() {
    this.filterForm = this.fb.group({
      project: [''],
      site: [''],
      dateFrom: [null],
      dateTo: [null],
      dateRange: ['']
    });
  }

  private loadReports() {
    const blastingEngineerReports: BlastingEngineerReport[] = [
      {
        id: 'report-1',
        name: 'Blast Sequence Report',
        description: 'Comprehensive overview of blast sequences designed, executed, and their outcomes',
        icon: 'timeline',
        category: 'blast-sequence'
      },
      {
        id: 'report-2',
        name: 'Explosive Usage Report',
        description: 'Detailed analysis of explosive materials used across projects and sites',
        icon: 'science',
        category: 'explosive-usage'
      },
      {
        id: 'report-3',
        name: 'Site Drilling Progress Report',
        description: 'Track drilling progress, patterns executed, and completion metrics per site',
        icon: 'engineering',
        category: 'site-progress'
      },
      {
        id: 'report-4',
        name: 'Blast Pattern Effectiveness Report',
        description: 'Analysis of blast pattern performance, fragmentation results, and optimization metrics',
        icon: 'analytics',
        category: 'blast-effectiveness'
      },
      {
        id: 'report-5',
        name: 'Safety & Compliance Report',
        description: 'Safety procedures followed, compliance checks, and incident records',
        icon: 'verified_user',
        category: 'safety-compliance'
      },
      {
        id: 'report-6',
        name: 'Explosive Request History',
        description: 'History of explosive material requests, approvals, and dispatch tracking',
        icon: 'request_page',
        category: 'explosive-requests'
      }
    ];
    this.reports.set(blastingEngineerReports);
  }

  private loadProjects() {
    // In real implementation, load from service
    const mockProjects = [
      { id: 'proj-1', name: 'Highway Construction Phase 2' },
      { id: 'proj-2', name: 'Mining Site Expansion' },
      { id: 'proj-3', name: 'Quarry Development Project' }
    ];
    this.projects.set(mockProjects);
  }

  private loadSites() {
    // In real implementation, load from service
    const mockSites = [
      { id: 'site-1', name: 'North Sector Alpha' },
      { id: 'site-2', name: 'East Quarry Zone' },
      { id: 'site-3', name: 'Mining Block C' }
    ];
    this.sites.set(mockSites);
  }

  selectReport(report: BlastingEngineerReport) {
    this.selectedReport.set(report);
    this.showFilters.set(true);
  }

  exportReport(format: 'pdf' | 'csv', report?: BlastingEngineerReport) {
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
      console.log('Exporting blasting engineer report:', {
        report: selectedRep.name,
        format: format,
        filters: this.filterForm.value,
        engineer: this.currentUser()
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
