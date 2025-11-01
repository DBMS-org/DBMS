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
  template: `
    <div class="job-list-container">
      <!-- Loading State -->
      @if (isLoading()) {
        <div class="loading-container">
          <app-skeleton-loader type="table" [rows]="10" [columns]="8"></app-skeleton-loader>
        </div>
      } @else {
        <!-- Performance Metrics (Development Only) -->
        @if (showPerformanceMetrics()) {
          <div class="performance-metrics">
            <small>
              Filter Time: {{ lastFilterTime() }}ms | 
              Items: {{ totalJobs() }} → {{ displayedJobs().length }} |
              Virtual Scroll: {{ useVirtualScrolling() ? 'ON' : 'OFF' }}
            </small>
          </div>
        }

        <!-- Bulk Actions Bar - Mechanical Engineer Permissions -->
        @if (selection.hasValue()) {
        <div class="bulk-actions-bar">
          <span class="selected-count">{{ selection.selected.length }} jobs selected</span>
          <div class="bulk-actions">
            <button mat-stroked-button [matMenuTriggerFor]="bulkStatusMenu">
              <mat-icon>update</mat-icon>
              Update Status
            </button>
            <button mat-stroked-button color="warn" (click)="clearSelection()">
              <mat-icon>clear</mat-icon>
              Clear Selection
            </button>
          </div>
        </div>
      }

        <!-- Jobs Table -->
        <div class="table-container" #tableContainer>
          @if (useVirtualScrolling() && displayedJobs().length > virtualScrollThreshold()) {
            <!-- Virtual Scrolling Table for Large Datasets -->
            <cdk-virtual-scroll-viewport 
              itemSize="72" 
              class="virtual-scroll-viewport"
              (scrolledIndexChange)="onVirtualScrollIndexChange($event)">
              
              <div class="virtual-table-header">
                <div class="virtual-header-row">
                  @for (column of displayedColumns; track column) {
                    <div class="virtual-header-cell" [class]="'column-' + column">
                      {{ getColumnDisplayName(column) }}
                    </div>
                  }
                </div>
              </div>

              <div *cdkVirtualFor="let job of displayedJobs(); trackBy: trackByJobId" 
                   class="virtual-table-row"
                   [class.selected]="selection.isSelected(job)"
                   (click)="onRowClick(job)">
                
                <!-- Selection -->
                <div class="virtual-cell column-select">
                  <mat-checkbox 
                    (click)="$event.stopPropagation()"
                    (change)="$event ? selection.toggle(job) : null"
                    [checked]="selection.isSelected(job)">
                  </mat-checkbox>
                </div>

                <!-- Machine -->
                <div class="virtual-cell column-machine">
                  <div class="machine-info">
                    <button mat-button class="machine-link" (click)="onMachineClick(job)">
                      <div class="machine-name">{{ job.machineName }}</div>
                      <div class="machine-id">ID: {{ job.machineId }}</div>
                    </button>
                  </div>
                </div>

                <!-- Serial Number -->
                <div class="virtual-cell column-serialNumber">{{ job.serialNumber }}</div>

                <!-- Project -->
                <div class="virtual-cell column-project">{{ job.project || job.projectName || 'N/A' }}</div>

                <!-- Scheduled Date -->
                <div class="virtual-cell column-scheduledDate">
                  <div class="date-info">
                    <div class="date">{{ formatDate(job.scheduledDate) }}</div>
                    <div class="time">{{ formatTime(job.scheduledDate) }}</div>
                  </div>
                </div>

                <!-- Type -->
                <div class="virtual-cell column-type">
                  <mat-chip [class]="getTypeChipClass(job.type)">
                    {{ getTypeDisplayName(job.type) }}
                  </mat-chip>
                </div>

                <!-- Status -->
                <div class="virtual-cell column-status">
                  <mat-chip [class]="getStatusChipClass(job.status)">
                    <mat-icon [class]="getStatusIconClass(job.status)">{{ getStatusIcon(job.status) }}</mat-icon>
                    {{ getStatusDisplayName(job.status) }}
                  </mat-chip>
                </div>

                <!-- Assigned To -->
                <div class="virtual-cell column-assignedTo">
                  <div class="assigned-to">
                    @if (job.assignedTo && job.assignedTo.length === 1) {
                      <span>{{ job.assignedTo[0] }}</span>
                    } @else if (job.assignedTo && job.assignedTo.length > 1) {
                      <span>{{ job.assignedTo[0] }}</span>
                      <mat-chip class="count-chip" [matTooltip]="getAssignedTooltip(job.assignedTo!)">
                        +{{ job.assignedTo.length - 1 }}
                      </mat-chip>
                    } @else {
                      <span class="unassigned">Unassigned</span>
                    }
                  </div>
                </div>

                <!-- Actions -->
                <div class="virtual-cell column-actions">
                  <button mat-icon-button [matMenuTriggerFor]="actionMenu" [matMenuTriggerData]="{job: job}">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                </div>
              </div>
            </cdk-virtual-scroll-viewport>
          } @else {
            <!-- Standard Table for Smaller Datasets -->
            <table mat-table [dataSource]="paginatedJobs()" matSort (matSortChange)="onSortChange($event)" class="jobs-table">
          
          <!-- Selection Column -->
          <ng-container matColumnDef="select">
            <th mat-header-cell *matHeaderCellDef>
              <mat-checkbox 
                (change)="$event ? toggleAllRows() : null"
                [checked]="selection.hasValue() && isAllSelected()"
                [indeterminate]="selection.hasValue() && !isAllSelected()"
                [aria-label]="checkboxLabel()">
              </mat-checkbox>
            </th>
            <td mat-cell *matCellDef="let job">
              <mat-checkbox 
                (click)="$event.stopPropagation()"
                (change)="$event ? selection.toggle(job) : null"
                [checked]="selection.isSelected(job)"
                [aria-label]="checkboxLabel(job)">
              </mat-checkbox>
            </td>
          </ng-container>

          <!-- Machine ID/Name Column -->
          <ng-container matColumnDef="machine">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="machineName">Machine</th>
            <td mat-cell *matCellDef="let job">
              <div class="machine-info">
                <button mat-button class="machine-link" (click)="onMachineClick(job)">
                  <div class="machine-name">{{ job.machineName }}</div>
                  <div class="machine-id">ID: {{ job.machineId }}</div>
                </button>
              </div>
            </td>
          </ng-container>

          <!-- Serial Number Column -->
          <ng-container matColumnDef="serialNumber">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="serialNumber">Serial No.</th>
            <td mat-cell *matCellDef="let job">{{ job.serialNumber }}</td>
          </ng-container>

          <!-- Project Column -->
          <ng-container matColumnDef="project">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="project">Project</th>
            <td mat-cell *matCellDef="let job">{{ job.project }}</td>
          </ng-container>

          <!-- Scheduled Date Column -->
          <ng-container matColumnDef="scheduledDate">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="scheduledDate">Scheduled Date</th>
            <td mat-cell *matCellDef="let job">
              <div class="date-info">
                <div class="date">{{ formatDate(job.scheduledDate) }}</div>
                <div class="time">{{ formatTime(job.scheduledDate) }}</div>
              </div>
            </td>
          </ng-container>

          <!-- Type Column -->
          <ng-container matColumnDef="type">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="type">Type</th>
            <td mat-cell *matCellDef="let job">
              <mat-chip [class]="getTypeChipClass(job.type)">
                {{ getTypeDisplayName(job.type) }}
              </mat-chip>
            </td>
          </ng-container>

          <!-- Status Column -->
          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef mat-sort-header="status">Status</th>
            <td mat-cell *matCellDef="let job">
              <mat-chip [class]="getStatusChipClass(job.status)">
                <mat-icon [class]="getStatusIconClass(job.status)">{{ getStatusIcon(job.status) }}</mat-icon>
                {{ getStatusDisplayName(job.status) }}
              </mat-chip>
            </td>
          </ng-container>

          <!-- Assigned To Column -->
          <ng-container matColumnDef="assignedTo">
            <th mat-header-cell *matHeaderCellDef>Assigned To</th>
            <td mat-cell *matCellDef="let job">
              <div class="assigned-to">
                @if (job.assignedTo.length === 1) {
                  <span>{{ job.assignedTo[0] }}</span>
                } @else if (job.assignedTo.length > 1) {
                  <span>{{ job.assignedTo[0] }}</span>
                  <mat-chip class="count-chip" [matTooltip]="getAssignedTooltip(job.assignedTo)">
                    +{{ job.assignedTo.length - 1 }}
                  </mat-chip>
                } @else {
                  <span class="unassigned">Unassigned</span>
                }
              </div>
            </td>
          </ng-container>

          <!-- Actions Column -->
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let job">
              <button mat-icon-button [matMenuTriggerFor]="actionMenu" [matMenuTriggerData]="{job: job}">
                <mat-icon>more_vert</mat-icon>
              </button>
            </td>
          </ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
              <tr mat-row *matRowDef="let job; columns: displayedColumns;" 
                  class="job-row" 
                  [class.selected]="selection.isSelected(job)"
                  (click)="onRowClick(job)">
              </tr>
            </table>
          }
        </div>

        <!-- Paginator (only for non-virtual scrolling) -->
        @if (!useVirtualScrolling() || displayedJobs().length <= virtualScrollThreshold()) {
          <mat-paginator 
            [length]="totalJobs()"
            [pageSize]="pageSize()"
            [pageSizeOptions]="pageSizeOptions"
            [pageIndex]="currentPage()"
            (page)="onPageChange($event)"
            showFirstLastButtons>
          </mat-paginator>
        }
      }

      <!-- Action Menu Template - Mechanical Engineer Permissions -->
      <mat-menu #actionMenu="matMenu" class="action-menu-modern">
        <ng-template matMenuContent let-job="job">
          <div class="menu-header">
            <span class="menu-title">Actions</span>
          </div>
          <button mat-menu-item (click)="onViewJobDetails(job)" class="menu-item-primary">
            <div class="menu-item-content">
              <mat-icon class="menu-icon">description</mat-icon>
              <div class="menu-text">
                <span class="menu-label">Job Details</span>
                <span class="menu-description">View job information and progress</span>
              </div>
            </div>
          </button>
          <button mat-menu-item (click)="onViewMachineOverview(job)" class="menu-item-primary">
            <div class="menu-item-content">
              <mat-icon class="menu-icon">precision_manufacturing</mat-icon>
              <div class="menu-text">
                <span class="menu-label">Machine Overview</span>
                <span class="menu-description">View machine details and metrics</span>
              </div>
            </div>
          </button>
          <mat-divider class="menu-divider"></mat-divider>
          <button mat-menu-item [matMenuTriggerFor]="statusSubmenu" [matMenuTriggerData]="{job: job}" class="menu-item-action">
            <div class="menu-item-content">
              <mat-icon class="menu-icon status-icon">update</mat-icon>
              <div class="menu-text">
                <span class="menu-label">Update Job Status</span>
                <span class="menu-description">Change the current job status</span>
              </div>
              <mat-icon class="submenu-arrow">chevron_right</mat-icon>
            </div>
          </button>
        </ng-template>
      </mat-menu>

      <!-- Status Submenu -->
      <mat-menu #statusSubmenu="matMenu">
        <ng-template matMenuContent let-job="job">
          @for (status of availableStatuses; track status) {
            <button mat-menu-item (click)="onStatusChange(job, status)" [disabled]="job.status === status">
              <mat-icon [class]="getStatusIconClass(status)">{{ getStatusIcon(status) }}</mat-icon>
              <span>{{ getStatusDisplayName(status) }}</span>
            </button>
          }
        </ng-template>
      </mat-menu>

      <!-- Bulk Status Menu -->
      <mat-menu #bulkStatusMenu="matMenu">
        @for (status of availableStatuses; track status) {
          <button mat-menu-item (click)="onBulkStatusChange(status)">
            <mat-icon [class]="getStatusIconClass(status)">{{ getStatusIcon(status) }}</mat-icon>
            <span>{{ getStatusDisplayName(status) }}</span>
          </button>
        }
      </mat-menu>
    </div>
  `,
  styles: [`
    .job-list-container {
      display: flex;
      flex-direction: column;
      height: 100%;
    }

    .bulk-actions-bar {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 12px 16px;
      background-color: #e3f2fd;
      border-radius: 4px;
      margin-bottom: 16px;
    }

    .selected-count {
      font-weight: 500;
      color: #1976d2;
    }

    .bulk-actions {
      display: flex;
      gap: 8px;
    }

    .table-container {
      flex: 1;
      overflow: auto;
      border: 1px solid #e0e0e0;
      border-radius: 4px;
      background: white;
    }

    .jobs-table {
      width: 100%;
      min-width: 800px;

      th {
        background-color: #f5f5f5;
        font-weight: 600;
        color: #333;
        padding: 16px;
        border-bottom: 2px solid #e0e0e0;
      }

      td {
        padding: 16px;
        color: #555;
        border-bottom: 1px solid #f0f0f0;
      }

      tr:last-child td {
        border-bottom: none;
      }
    }

    .job-row {
      cursor: pointer;
      transition: background-color 0.2s;

      &:hover {
        background-color: #f9f9f9;
      }

      &.selected {
        background-color: #e3f2fd;
      }
    }

    .machine-info {
      display: flex;
      flex-direction: column;
    }

    .machine-link {
      text-align: left;
      padding: 4px 8px;
      min-height: auto;
      line-height: 1.2;
    }

    .machine-name {
      font-weight: 500;
      color: #1976d2;
    }

    .machine-id {
      font-size: 12px;
      color: #666;
    }

    .date-info {
      display: flex;
      flex-direction: column;
      gap: 2px;
    }

    .date {
      font-weight: 500;
      color: #333;
    }

    .time {
      font-size: 12px;
      color: #666;
    }

    .assigned-to {
      display: flex;
      align-items: center;
      gap: 4px;
    }

    .count-chip {
      font-size: 10px;
      min-height: 20px;
      height: 20px;
    }

    .unassigned {
      color: #999;
      font-style: italic;
    }

    .submenu-arrow {
      margin-left: auto;
    }

    .delete-action {
      color: #d32f2f;
    }

    /* Loading State */
    .loading-container {
      padding: 16px;
    }

    /* Performance Metrics */
    .performance-metrics {
      padding: 8px 16px;
      background-color: #f5f5f5;
      border-radius: 4px;
      margin-bottom: 8px;
      font-family: monospace;
    }

    /* Virtual Scrolling Styles */
    .virtual-scroll-viewport {
      height: 600px;
      border: 1px solid #e0e0e0;
      border-radius: 4px;
    }

    .virtual-table-header {
      position: sticky;
      top: 0;
      z-index: 10;
      background: white;
      border-bottom: 1px solid #e0e0e0;
    }

    .virtual-header-row {
      display: flex;
      align-items: center;
      height: 56px;
      padding: 0 16px;
      background: #f5f5f5;
      font-weight: 500;
    }

    .virtual-header-cell {
      display: flex;
      align-items: center;
      padding: 0 8px;
    }

    .virtual-table-row {
      display: flex;
      align-items: center;
      height: 72px;
      padding: 0 16px;
      border-bottom: 1px solid #e0e0e0;
      cursor: pointer;
      transition: background-color 0.2s;
    }

    .virtual-table-row:hover {
      background-color: #f5f5f5;
    }

    .virtual-table-row.selected {
      background-color: #e3f2fd;
    }

    .virtual-cell {
      display: flex;
      align-items: center;
      padding: 0 8px;
      overflow: hidden;
    }

    /* Virtual Column Widths */
    .column-select {
      width: 60px;
      flex-shrink: 0;
    }

    .column-machine {
      width: 180px;
      flex-shrink: 0;
    }

    .column-serialNumber {
      width: 120px;
      flex-shrink: 0;
    }

    .column-project {
      width: 140px;
      flex-shrink: 0;
    }

    .column-scheduledDate {
      width: 140px;
      flex-shrink: 0;
    }

    .column-type {
      width: 120px;
      flex-shrink: 0;
    }

    .column-status {
      width: 140px;
      flex-shrink: 0;
    }

    .column-assignedTo {
      width: 160px;
      flex-shrink: 0;
    }

    .column-actions {
      width: 80px;
      flex-shrink: 0;
    }

    /* Status Chips */
    .status-scheduled {
      background-color: #e3f2fd;
      color: #1976d2;
    }

    .status-in-progress {
      background-color: #fff3e0;
      color: #f57c00;
    }

    .status-completed {
      background-color: #e8f5e8;
      color: #388e3c;
    }

    .status-cancelled {
      background-color: #fce4ec;
      color: #c2185b;
    }

    .status-overdue {
      background-color: #ffebee;
      color: #d32f2f;
    }

    /* Type Chips */
    .type-preventive {
      background-color: #e8f5e8;
      color: #388e3c;
    }

    .type-corrective {
      background-color: #fff3e0;
      color: #f57c00;
    }

    .type-predictive {
      background-color: #e3f2fd;
      color: #1976d2;
    }

    .type-emergency {
      background-color: #ffebee;
      color: #d32f2f;
    }

    /* Status Icons */
    .icon-scheduled {
      color: #1976d2;
    }

    .icon-in-progress {
      color: #f57c00;
    }

    .icon-completed {
      color: #388e3c;
    }

    .icon-cancelled {
      color: #c2185b;
    }

    .icon-overdue {
      color: #d32f2f;
    }

    /* Modern Action Menu Styles */
    ::ng-deep .action-menu-modern {
      min-width: 320px;
      padding: 0;
      border-radius: 8px;
      box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15);
    }

    ::ng-deep .action-menu-modern .mat-mdc-menu-content {
      padding: 0;
    }

    ::ng-deep .action-menu-modern .menu-header {
      padding: 12px 16px;
      background: linear-gradient(135deg, #1976d2 0%, #1565c0 100%);
      color: white;
      font-weight: 600;
      font-size: 14px;
      text-transform: uppercase;
      letter-spacing: 0.5px;
      border-radius: 8px 8px 0 0;
    }

    ::ng-deep .action-menu-modern .menu-title {
      display: block;
    }

    ::ng-deep .action-menu-modern .mat-mdc-menu-item {
      height: auto;
      min-height: 64px;
      padding: 0;
      line-height: normal;
    }

    ::ng-deep .action-menu-modern .menu-item-content {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 12px 16px;
      width: 100%;
    }

    ::ng-deep .action-menu-modern .menu-icon {
      flex-shrink: 0;
      width: 24px;
      height: 24px;
      font-size: 24px;
      color: #1976d2;
    }

    ::ng-deep .action-menu-modern .status-icon {
      color: #f57c00;
    }

    ::ng-deep .action-menu-modern .menu-text {
      flex: 1;
      display: flex;
      flex-direction: column;
      gap: 2px;
    }

    ::ng-deep .action-menu-modern .menu-label {
      font-size: 14px;
      font-weight: 600;
      color: #333;
      line-height: 1.3;
    }

    ::ng-deep .action-menu-modern .menu-description {
      font-size: 12px;
      color: #666;
      line-height: 1.3;
    }

    ::ng-deep .action-menu-modern .submenu-arrow {
      flex-shrink: 0;
      width: 20px;
      height: 20px;
      font-size: 20px;
      color: #999;
      margin-left: auto;
    }

    ::ng-deep .action-menu-modern .menu-divider {
      margin: 8px 0;
      border-top-color: #e0e0e0;
    }

    ::ng-deep .action-menu-modern .menu-item-primary:hover {
      background-color: #e3f2fd;
    }

    ::ng-deep .action-menu-modern .menu-item-action:hover {
      background-color: #fff3e0;
    }

    ::ng-deep .action-menu-modern .mat-mdc-menu-item:hover .menu-icon {
      transform: scale(1.1);
      transition: transform 0.2s ease;
    }

    ::ng-deep .action-menu-modern .mat-mdc-menu-item:hover .menu-label {
      color: #1976d2;
    }

    ::ng-deep .action-menu-modern .menu-item-action:hover .menu-label {
      color: #f57c00;
    }
  `]
})
export class JobListComponent implements AfterViewInit, OnDestroy {
  private maintenanceService = inject(MaintenanceService);
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