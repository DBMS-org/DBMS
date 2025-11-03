import { Component, OnInit, inject, signal, computed, output, input, ViewChild, ElementRef, AfterViewInit, OnDestroy, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { SelectionModel } from '@angular/cdk/collections';
import { ChangeDetectionStrategy } from '@angular/core';
import { fromEvent, Subject, takeUntil } from 'rxjs';

import { MaintenanceJob, MaintenanceStatus, MaintenanceType } from '../../models/maintenance.models';
import { MaintenanceService } from '../../services/maintenance.service';
import { VirtualScrollService } from '../../services/virtual-scroll.service';
import { SkeletonLoaderComponent } from '../../shared/skeleton-loader/skeleton-loader.component';

@Component({
  selector: 'app-job-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatCheckboxModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatPaginatorModule,
    MatSortModule,
    MatChipsModule,
    MatTooltipModule,
    MatDividerModule,
    ScrollingModule,
    SkeletonLoaderComponent
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './job-list.component.html',
  styleUrls: ['./job-list.component.scss']
})
export class JobListComponent implements AfterViewInit, OnDestroy {
  private virtualScrollService = inject(VirtualScrollService);
  private destroy$ = new Subject<void>();

  @ViewChild('tableContainer') tableContainer!: ElementRef;

  // Inputs
  jobs = input<MaintenanceJob[]>([]);
  isLoading = signal(false);
  currentPage = signal(0);
  pageSize = signal(25);
  sortField = signal<string>('scheduledDate');
  sortDirection = signal<'asc' | 'desc'>('asc');

  // Performance and Virtual Scrolling Configuration
  virtualScrollThreshold = signal(100); // Use virtual scrolling for > 100 items
  useVirtualScrolling = signal(true);
  showPerformanceMetrics = signal(false); // Set to true for development
  lastFilterTime = signal(0);

  // Computed values with performance optimization
  sortedJobs = computed(() => {
    const jobsList = this.jobs();
    const field = this.sortField();
    const direction = this.sortDirection();

    return [...jobsList].sort((a, b) => {
      let aValue: any = a[field as keyof MaintenanceJob];
      let bValue: any = b[field as keyof MaintenanceJob];

      if (field === 'scheduledDate') {
        aValue = new Date(aValue).getTime();
        bValue = new Date(bValue).getTime();
      }

      if (typeof aValue === 'string' && typeof bValue === 'string') {
        aValue = aValue.toLowerCase();
        bValue = bValue.toLowerCase();
      }

      if (aValue < bValue) return direction === 'asc' ? -1 : 1;
      if (aValue > bValue) return direction === 'asc' ? 1 : -1;
      return 0;
    });
  });

  // Measure sort time as a side effect (allowed), not inside a computed
  private measureSortTimeEffect = effect(() => {
    const startTime = performance.now();
    // Access to trigger computation and dependencies tracking
    void this.sortedJobs();
    const endTime = performance.now();
    this.lastFilterTime.set(Math.round(endTime - startTime));
  });

  displayedJobs = computed(() => {
    const sorted = this.sortedJobs();
    
    // Use virtual scrolling for large datasets
    if (this.useVirtualScrolling() && sorted.length > this.virtualScrollThreshold()) {
      return sorted; // Virtual scrolling handles pagination
    }
    
    // Use traditional pagination for smaller datasets
    const page = this.currentPage();
    const size = this.pageSize();
    const startIndex = page * size;
    return sorted.slice(startIndex, startIndex + size);
  });

  paginatedJobs = computed(() => {
    // For backward compatibility with mat-table
    return this.displayedJobs();
  });

  totalJobs = computed(() => this.jobs().length);

  // Outputs
  machineClicked = output<MaintenanceJob>();
  jobSelected = output<MaintenanceJob>();
  jobStatusChanged = output<{ job: MaintenanceJob; status: MaintenanceStatus }>();
  bulkStatusChanged = output<{ jobs: MaintenanceJob[]; status: MaintenanceStatus }>();

  // Table configuration
  displayedColumns = ['select', 'machine', 'serialNumber', 'project', 'scheduledDate', 'type', 'status', 'assignedTo', 'actions'];
  pageSizeOptions = [10, 25, 50, 100];
  availableStatuses = Object.values(MaintenanceStatus);

  // Selection
  selection = new SelectionModel<MaintenanceJob>(true, []);

  ngAfterViewInit() {
    // Initialize virtual scrolling if needed
    if (this.useVirtualScrolling()) {
      this.initializeVirtualScrolling();
    }

    // Set up performance monitoring in development
    if (this.showPerformanceMetrics()) {
      this.setupPerformanceMonitoring();
    }
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
    this.virtualScrollService.cleanup('job-list');
  }

  // Event Handlers
  onSortChange(sort: Sort) {
    this.sortField.set(sort.active);
    this.sortDirection.set(sort.direction as 'asc' | 'desc');
  }

  onPageChange(event: PageEvent) {
    this.currentPage.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
  }

  onRowClick(job: MaintenanceJob) {
    this.jobSelected.emit(job);
  }

  onMachineClick(job: MaintenanceJob) {
    this.machineClicked.emit(job);
  }

  onViewJobDetails(job: MaintenanceJob) {
    // Emit event to show job details in the detail panel
    this.jobSelected.emit(job);
  }

  onViewMachineOverview(job: MaintenanceJob) {
    // Emit event to show machine overview (handled by parent)
    this.machineClicked.emit(job);
  }

  onStatusChange(job: MaintenanceJob, status: MaintenanceStatus) {
    this.jobStatusChanged.emit({ job, status });
  }

  onBulkStatusChange(status: MaintenanceStatus) {
    const selectedJobs = this.selection.selected;
    this.bulkStatusChanged.emit({ jobs: selectedJobs, status });
    this.clearSelection();
  }

  // Selection Methods
  isAllSelected(): boolean {
    const numSelected = this.selection.selected.length;
    const numRows = this.paginatedJobs().length;
    return numSelected === numRows && numRows > 0;
  }

  toggleAllRows() {
    if (this.isAllSelected()) {
      this.selection.clear();
    } else {
      this.paginatedJobs().forEach(job => this.selection.select(job));
    }
  }

  clearSelection() {
    this.selection.clear();
  }

  checkboxLabel(job?: MaintenanceJob): string {
    if (!job) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }
    return `${this.selection.isSelected(job) ? 'deselect' : 'select'} job ${job.machineName}`;
  }

  // Utility Methods
  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString();
  }

  formatTime(date: Date): string {
    return new Date(date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }

  getStatusDisplayName(status: MaintenanceStatus): string {
    const statusNames = {
      [MaintenanceStatus.SCHEDULED]: 'Scheduled',
      [MaintenanceStatus.IN_PROGRESS]: 'In Progress',
      [MaintenanceStatus.COMPLETED]: 'Completed',
      [MaintenanceStatus.CANCELLED]: 'Cancelled',
      [MaintenanceStatus.OVERDUE]: 'Overdue'
    };
    return statusNames[status];
  }

  getStatusIcon(status: MaintenanceStatus): string {
    const statusIcons = {
      [MaintenanceStatus.SCHEDULED]: 'schedule',
      [MaintenanceStatus.IN_PROGRESS]: 'play_circle',
      [MaintenanceStatus.COMPLETED]: 'check_circle',
      [MaintenanceStatus.CANCELLED]: 'cancel',
      [MaintenanceStatus.OVERDUE]: 'warning'
    };
    return statusIcons[status];
  }

  getStatusChipClass(status: MaintenanceStatus): string {
    return `status-${status.toLowerCase().replace('_', '-')}`;
  }

  getStatusIconClass(status: MaintenanceStatus): string {
    return `icon-${status.toLowerCase().replace('_', '-')}`;
  }

  getTypeDisplayName(type: MaintenanceType): string {
    const typeNames = {
      [MaintenanceType.PREVENTIVE]: 'Preventive',
      [MaintenanceType.CORRECTIVE]: 'Corrective',
      [MaintenanceType.PREDICTIVE]: 'Predictive',
      [MaintenanceType.EMERGENCY]: 'Emergency'
    };
    return typeNames[type];
  }

  getTypeChipClass(type: MaintenanceType): string {
    return `type-${type.toLowerCase()}`;
  }

  getAssignedTooltip(assignedTo: string[]): string {
    return assignedTo.join(', ');
  }

  // Virtual Scrolling Methods
  onVirtualScrollIndexChange(index: number) {
    // Handle virtual scroll index changes if needed
    console.log('Virtual scroll index changed:', index);
  }

  trackByJobId(index: number, job: MaintenanceJob): number {
    return job.id;
  }

  getColumnDisplayName(column: string): string {
    const columnNames: { [key: string]: string } = {
      select: '',
      machine: 'Machine',
      serialNumber: 'Serial No.',
      project: 'Project',
      scheduledDate: 'Scheduled Date',
      type: 'Type',
      status: 'Status',
      assignedTo: 'Assigned To',
      actions: 'Actions'
    };
    return columnNames[column] || column;
  }

  // Performance and Virtual Scrolling Setup
  private initializeVirtualScrolling() {
    if (this.tableContainer) {
      const containerHeight = 600; // Fixed height for virtual scrolling
      const itemHeight = 72; // Height of each row
      
      this.virtualScrollService.initializeVirtualScroll('job-list', {
        itemHeight,
        containerHeight,
        buffer: 5,
        totalItems: this.jobs().length
      });
    }
  }

  private setupPerformanceMonitoring() {
    // Monitor performance metrics
    fromEvent(window, 'resize')
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        // Recalculate virtual scrolling on window resize
        if (this.useVirtualScrolling()) {
          this.initializeVirtualScrolling();
        }
      });
  }

  // Status Update with Loading Feedback
  onStatusChangeWithFeedback(job: MaintenanceJob, status: MaintenanceStatus) {
    // Provide immediate visual feedback
    const originalStatus = job.status;
    job.status = status; // Optimistic update
    
    this.jobStatusChanged.emit({ job, status });
    
    // If the update fails, revert the status
    // This would be handled by the parent component
  }

  // Bulk operations with progress feedback
  onBulkStatusChangeWithProgress(status: MaintenanceStatus) {
    const selectedJobs = this.selection.selected;
    const totalJobs = selectedJobs.length;
    
    // Emit with progress tracking capability
    this.bulkStatusChanged.emit({ jobs: selectedJobs, status });
    
    // Clear selection after operation
    this.clearSelection();
  }
}
