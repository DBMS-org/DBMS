import { Component, OnInit, inject, signal, computed, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { ChangeDetectionStrategy } from '@angular/core';
import { Subject, takeUntil, debounceTime, distinctUntilChanged } from 'rxjs';

import { JobListComponent } from './job-list/job-list.component';
import { JobFiltersComponent } from './job-filters/job-filters.component';
import { JobDetailPanelComponent } from './job-detail-panel/job-detail-panel.component';
import { JobStatusUpdateComponent, JobStatusUpdateData } from './job-status-update/job-status-update.component';

import { MaintenanceJob, JobFilters, MaintenanceStatus } from '../models/maintenance.models';
import { MaintenanceService } from '../services/maintenance.service';
import { PerformanceOptimizationService } from '../services/performance-optimization.service';
import { LoadingSpinnerComponent } from '../shared/loading-spinner/loading-spinner.component';
import { OfflineIndicatorComponent } from '../shared/offline-indicator/offline-indicator.component';

@Component({
  selector: 'app-maintenance-jobs',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatSnackBarModule,
    MatIconModule,
    JobListComponent,
    JobFiltersComponent,
    JobDetailPanelComponent,
    LoadingSpinnerComponent,
    OfflineIndicatorComponent
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './maintenance-jobs.component.html',
  styleUrls: ['./maintenance-jobs.component.scss']
})
export class MaintenanceJobsComponent implements OnInit, OnDestroy {
  private maintenanceService = inject(MaintenanceService);
  private performanceService = inject(PerformanceOptimizationService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);
  private destroy$ = new Subject<void>();

  // Component state
  allJobs = signal<MaintenanceJob[]>([]);
  currentFilters = signal<JobFilters>({});
  searchTerm = signal<string>('');
  selectedJob = signal<MaintenanceJob | null>(null);
  isDetailPanelOpen = signal<boolean>(false);
  isLoading = signal<boolean>(false);

  // Debounced search and filter handling
  private searchSubject = new Subject<string>();
  private filterSubject = new Subject<JobFilters>();

  // Jobs filtered by search term and active filters
  filteredJobs = computed(() => {
    const jobs = this.allJobs();
    const filters = this.currentFilters();
    const search = this.searchTerm();

    // Use performance-optimized search and filter
    return this.maintenanceService.searchAndFilterJobs(jobs, search, filters);
  });

  ngOnInit() {
    this.loadJobs();
    this.setupPerformanceOptimizations();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
    this.performanceService.clearDatasetCaches('maintenance-jobs');
  }

  private loadJobs() {
    this.isLoading.set(true);
    this.maintenanceService.getMaintenanceJobs().subscribe({
      next: (jobs) => {
        this.allJobs.set(jobs);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading jobs:', error);
        this.showErrorMessage('Failed to load maintenance jobs');
        this.isLoading.set(false);
      }
    });
  }

  // Event Handlers with performance optimization
  onFiltersChanged(filters: JobFilters) {
    this.filterSubject.next(filters);
  }

  onSearchChanged(searchTerm: string) {
    this.searchSubject.next(searchTerm);
  }

  onMachineClicked(job: MaintenanceJob) {
    this.selectedJob.set(job);
    this.isDetailPanelOpen.set(true);
  }

  onJobSelected(job: MaintenanceJob) {
    this.selectedJob.set(job);
    this.isDetailPanelOpen.set(true);
  }

  onDetailPanelClosed() {
    this.isDetailPanelOpen.set(false);
    this.selectedJob.set(null);
  }

  // REMOVED: onEditJob() - Mechanical Engineer cannot edit jobs
  // Only status updates are allowed via the consolidated status update dialog

  onUpdateJobStatus(job: MaintenanceJob) {
    const dialogData: JobStatusUpdateData = { job };
    
    const dialogRef = this.dialog.open(JobStatusUpdateComponent, {
      width: '600px',
      maxWidth: '90vw',
      data: dialogData,
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Update the job in the list
        this.updateJobInList(result);
        this.selectedJob.set(result);
      }
    });
  }

  onJobStatusChanged(event: { job: MaintenanceJob; status: MaintenanceStatus }) {
    this.updateJobStatusWithFeedback(event.job, event.status);
  }

  onBulkStatusChanged(event: { jobs: MaintenanceJob[]; status: MaintenanceStatus }) {
    this.bulkUpdateJobStatusWithProgress(event.jobs, event.status);
  }

  // Job Management Methods with immediate feedback
  private async updateJobStatusWithFeedback(job: MaintenanceJob, status: MaintenanceStatus) {
    // Show immediate loading feedback
    const loadingMessage = this.snackBar.open('Updating job status...', '', {
      duration: 0, // Keep open until dismissed
      panelClass: ['info-snackbar']
    });

    try {
      // Optimistic update for immediate UI feedback
      const originalStatus = job.status;
      const updatedJob = { ...job, status, updatedAt: new Date() };
      this.updateJobInList(updatedJob);
      
      if (this.selectedJob()?.id === job.id) {
        this.selectedJob.set(updatedJob);
      }

      // Perform actual update
      await this.maintenanceService.updateJobStatus(job.id, status).toPromise();
      
      loadingMessage.dismiss();
      this.showSuccessMessage('Job status updated successfully');
    } catch (error) {
      // Revert optimistic update on error
      const revertedJob = { ...job, status: job.status };
      this.updateJobInList(revertedJob);
      
      if (this.selectedJob()?.id === job.id) {
        this.selectedJob.set(revertedJob);
      }

      loadingMessage.dismiss();
      console.error('Error updating job status:', error);
      this.showErrorMessage('Failed to update job status');
    }
  }

  private async bulkUpdateJobStatusWithProgress(jobs: MaintenanceJob[], status: MaintenanceStatus) {
    const totalJobs = jobs.length;
    let completedJobs = 0;

    // Show progress snackbar
    const progressMessage = this.snackBar.open(
      `Updating ${totalJobs} jobs... (0/${totalJobs})`, 
      '', 
      {
        duration: 0,
        panelClass: ['info-snackbar']
      }
    );

    try {
      // Optimistic updates for immediate feedback
      const originalJobs = jobs.map(job => ({ ...job }));
      jobs.forEach(job => {
        const updatedJob = { ...job, status, updatedAt: new Date() };
        this.updateJobInList(updatedJob);
      });

      // Perform actual bulk update
      const jobIds = jobs.map(job => job.id);
      await this.maintenanceService.bulkUpdateJobStatus(jobIds, status).toPromise();
      
      progressMessage.dismiss();
      this.showSuccessMessage(`${jobs.length} jobs updated successfully`);
    } catch (error) {
      // Revert optimistic updates on error
      const originalJobs = jobs.map(job => ({ ...job }));
      originalJobs.forEach(job => {
        this.updateJobInList(job);
      });

      progressMessage.dismiss();
      console.error('Error bulk updating job status:', error);
      this.showErrorMessage('Failed to update job statuses');
    }
  }

  private updateJobInList(updatedJob: MaintenanceJob) {
    this.allJobs.update(jobs => 
      jobs.map(job => job.id === updatedJob.id ? updatedJob : job)
    );
  }

  // Utility Methods
  private extractMachineType(machineName: string): string {
    // Extract machine type from machine name (e.g., "Excavator 1" -> "Excavator")
    const match = machineName.match(/^([A-Za-z\s]+)/);
    return match ? match[1].trim() : machineName;
  }

  private showSuccessMessage(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      panelClass: ['success-snackbar']
    });
  }

  private showErrorMessage(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 5000,
      panelClass: ['error-snackbar']
    });
  }

  private showInfoMessage(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 4000,
      panelClass: ['info-snackbar']
    });
  }

  // Performance Optimization Setup
  private setupPerformanceOptimizations() {
    // Debounced search
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(searchTerm => {
      this.searchTerm.set(searchTerm);
    });

    // Debounced filtering
    this.filterSubject.pipe(
      debounceTime(150),
      takeUntil(this.destroy$)
    ).subscribe(filters => {
      this.currentFilters.set(filters);
    });
  }
}