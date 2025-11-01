import { Component, OnInit, inject, signal, output, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatExpansionModule } from '@angular/material/expansion';
import { ChangeDetectionStrategy } from '@angular/core';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

import { JobFilters, MaintenanceStatus, MaintenanceJob } from '../../models/maintenance.models';

@Component({
  selector: 'app-job-filters',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatAutocompleteModule,
    MatExpansionModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="job-filters-container">
      <mat-expansion-panel [expanded]="isExpanded()" (expandedChange)="onExpandedChange($event)">
        <mat-expansion-panel-header>
          <mat-panel-title>
            <mat-icon>filter_list</mat-icon>
            <span>Filters</span>
            @if (activeFiltersCount() > 0) {
              <mat-chip class="active-filters-chip">{{ activeFiltersCount() }}</mat-chip>
            }
          </mat-panel-title>
          <mat-panel-description>
            {{ getFilterDescription() }}
          </mat-panel-description>
        </mat-expansion-panel-header>

        <form [formGroup]="filterForm" class="filters-form">
          <!-- Search Bar -->
          <div class="search-section">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Search machines, serial numbers, or descriptions</mat-label>
              <mat-icon matPrefix>search</mat-icon>
              <input matInput 
                     formControlName="searchTerm" 
                     placeholder="Type to search..."
                     (input)="onSearchInput($event)">
              @if (filterForm.get('searchTerm')?.value) {
                <button matSuffix 
                        mat-icon-button 
                        type="button"
                        (click)="clearSearch()"
                        aria-label="Clear search">
                  <mat-icon>clear</mat-icon>
                </button>
              }
            </mat-form-field>
          </div>

          <!-- Filter Controls -->
          <div class="filter-controls">
            <!-- Date Range -->
            <div class="filter-group">
              <h4>Date Range</h4>
              <div class="date-range">
                <mat-form-field appearance="outline">
                  <mat-label>Start Date</mat-label>
                  <input matInput 
                         [matDatepicker]="startPicker" 
                         formControlName="startDate"
                         readonly>
                  <mat-datepicker-toggle matSuffix [for]="startPicker"></mat-datepicker-toggle>
                  <mat-datepicker #startPicker></mat-datepicker>
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>End Date</mat-label>
                  <input matInput 
                         [matDatepicker]="endPicker" 
                         formControlName="endDate"
                         readonly>
                  <mat-datepicker-toggle matSuffix [for]="endPicker"></mat-datepicker-toggle>
                  <mat-datepicker #endPicker></mat-datepicker>
                </mat-form-field>
              </div>
            </div>

            <!-- Status Filter -->
            <div class="filter-group">
              <h4>Status</h4>
              <mat-form-field appearance="outline">
                <mat-label>Select Status</mat-label>
                <mat-select formControlName="status" multiple>
                  @for (status of availableStatuses; track status.value) {
                    <mat-option [value]="status.value">
                      <div class="status-option">
                        <mat-icon [class]="getStatusIconClass(status.value)">{{ getStatusIcon(status.value) }}</mat-icon>
                        <span>{{ status.label }}</span>
                      </div>
                    </mat-option>
                  }
                </mat-select>
              </mat-form-field>
            </div>

            <!-- Machine Type Filter -->
            <div class="filter-group">
              <h4>Machine Type</h4>
              <mat-form-field appearance="outline">
                <mat-label>Select Machine Types</mat-label>
                <mat-select formControlName="machineType" multiple>
                  @for (type of availableMachineTypes(); track type) {
                    <mat-option [value]="type">{{ type }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>
            </div>

            <!-- Project Filter -->
            <div class="filter-group">
              <h4>Project</h4>
              <mat-form-field appearance="outline">
                <mat-label>Select Projects</mat-label>
                <mat-select formControlName="project" multiple>
                  @for (project of availableProjects(); track project) {
                    <mat-option [value]="project">{{ project }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>
            </div>

            <!-- Assigned To Filter -->
            <div class="filter-group">
              <h4>Assigned To</h4>
              <mat-form-field appearance="outline">
                <mat-label>Select Technicians</mat-label>
                <mat-select formControlName="assignedTo" multiple>
                  @for (technician of availableTechnicians(); track technician) {
                    <mat-option [value]="technician">{{ technician }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>
            </div>
          </div>

          <!-- Active Filters Display -->
          @if (activeFiltersCount() > 0) {
            <div class="active-filters">
              <h4>Active Filters</h4>
              <div class="filter-chips">
                @if (filterForm.get('searchTerm')?.value) {
                  <mat-chip (removed)="removeFilter('searchTerm')">
                    Search: "{{ filterForm.get('searchTerm')?.value }}"
                    <mat-icon matChipRemove>cancel</mat-icon>
                  </mat-chip>
                }

                @if (filterForm.get('startDate')?.value || filterForm.get('endDate')?.value) {
                  <mat-chip (removed)="removeFilter('dateRange')">
                    Date: {{ getDateRangeText() }}
                    <mat-icon matChipRemove>cancel</mat-icon>
                  </mat-chip>
                }

                @if (filterForm.get('status')?.value?.length > 0) {
                  <mat-chip (removed)="removeFilter('status')">
                    Status: {{ getStatusText() }}
                    <mat-icon matChipRemove>cancel</mat-icon>
                  </mat-chip>
                }

                @if (filterForm.get('machineType')?.value?.length > 0) {
                  <mat-chip (removed)="removeFilter('machineType')">
                    Machine Type: {{ getMachineTypeText() }}
                    <mat-icon matChipRemove>cancel</mat-icon>
                  </mat-chip>
                }

                @if (filterForm.get('project')?.value?.length > 0) {
                  <mat-chip (removed)="removeFilter('project')">
                    Project: {{ getProjectText() }}
                    <mat-icon matChipRemove>cancel</mat-icon>
                  </mat-chip>
                }

                @if (filterForm.get('assignedTo')?.value?.length > 0) {
                  <mat-chip (removed)="removeFilter('assignedTo')">
                    Assigned: {{ getAssignedToText() }}
                    <mat-icon matChipRemove>cancel</mat-icon>
                  </mat-chip>
                }
              </div>
            </div>
          }

          <!-- Action Buttons -->
          <div class="filter-actions">
            <button mat-stroked-button 
                    type="button" 
                    (click)="resetFilters()"
                    [disabled]="activeFiltersCount() === 0">
              <mat-icon>clear_all</mat-icon>
              Clear All Filters
            </button>
            
            <button mat-raised-button 
                    color="primary" 
                    type="button"
                    (click)="applyFilters()">
              <mat-icon>filter_list</mat-icon>
              Apply Filters
            </button>
          </div>
        </form>
      </mat-expansion-panel>
    </div>
  `,
  styles: [`
    .job-filters-container {
      margin-bottom: 16px;
    }

    .active-filters-chip {
      margin-left: 8px;
      background-color: #1976d2;
      color: white;
      font-size: 12px;
      min-height: 20px;
      height: 20px;
    }

    .filters-form {
      padding: 16px 0;
    }

    .search-section {
      margin-bottom: 24px;
    }

    .full-width {
      width: 100%;
    }

    .filter-controls {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 16px;
      margin-bottom: 24px;
    }

    .filter-group h4 {
      margin: 0 0 8px 0;
      font-size: 14px;
      font-weight: 500;
      color: #666;
    }

    .date-range {
      display: flex;
      gap: 8px;
    }

    .date-range mat-form-field {
      flex: 1;
    }

    .status-option {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .active-filters {
      margin-bottom: 24px;
      padding: 16px;
      background-color: #f5f5f5;
      border-radius: 4px;
    }

    .active-filters h4 {
      margin: 0 0 12px 0;
      font-size: 14px;
      font-weight: 500;
      color: #333;
    }

    .filter-chips {
      display: flex;
      flex-wrap: wrap;
      gap: 8px;
    }

    .filter-actions {
      display: flex;
      justify-content: flex-end;
      gap: 12px;
      padding-top: 16px;
      border-top: 1px solid #e0e0e0;
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

    @media (max-width: 768px) {
      .filter-controls {
        grid-template-columns: 1fr;
      }

      .date-range {
        flex-direction: column;
      }

      .filter-actions {
        flex-direction: column;
      }
    }
  `]
})
export class JobFiltersComponent implements OnInit {
  private fb = inject(FormBuilder);

  // Inputs
  jobs = input<MaintenanceJob[]>([]);
  initialFilters = input<JobFilters>({});

  // Outputs
  filtersChanged = output<JobFilters>();
  searchChanged = output<string>();

  // Signals
  isExpanded = signal(false);
  activeFiltersCount = signal(0);
  availableMachineTypes = signal<string[]>([]);
  availableProjects = signal<string[]>([]);
  availableTechnicians = signal<string[]>([]);

  // Form
  filterForm: FormGroup;

  // Configuration
  availableStatuses = [
    { value: MaintenanceStatus.SCHEDULED, label: 'Scheduled' },
    { value: MaintenanceStatus.IN_PROGRESS, label: 'In Progress' },
    { value: MaintenanceStatus.COMPLETED, label: 'Completed' },
    { value: MaintenanceStatus.CANCELLED, label: 'Cancelled' },
    { value: MaintenanceStatus.OVERDUE, label: 'Overdue' }
  ];

  constructor() {
    this.filterForm = this.fb.group({
      searchTerm: [''],
      startDate: [null],
      endDate: [null],
      status: [[]],
      machineType: [[]],
      project: [[]],
      assignedTo: [[]]
    });
  }

  ngOnInit() {
    this.initializeFilters();
    this.setupFormSubscriptions();
    this.updateAvailableOptions();
  }

  private initializeFilters() {
    const initial = this.initialFilters();
    if (initial) {
      this.filterForm.patchValue({
        searchTerm: initial.searchTerm || '',
        startDate: initial.dateRange?.start || null,
        endDate: initial.dateRange?.end || null,
        status: initial.status || [],
        machineType: initial.machineType || [],
        project: initial.project || [],
        assignedTo: initial.assignedTo || []
      });
    }
    this.updateActiveFiltersCount();
  }

  private setupFormSubscriptions() {
    // Real-time search
    this.filterForm.get('searchTerm')?.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(searchTerm => {
        this.searchChanged.emit(searchTerm);
        this.updateActiveFiltersCount();
      });

    // Other form changes
    this.filterForm.valueChanges
      .pipe(debounceTime(100))
      .subscribe(() => {
        this.updateActiveFiltersCount();
      });
  }

  private updateAvailableOptions() {
    const jobsList = this.jobs();
    
    // Extract unique machine types
    const machineTypes = [...new Set(jobsList.map(job => this.extractMachineType(job.machineName)))];
    this.availableMachineTypes.set(machineTypes.filter(type => type));

    // Extract unique projects
    const projects = [...new Set(jobsList.map(job => job.project))];
    this.availableProjects.set(projects.filter(project => project));

    // Extract unique technicians
    const technicians = [...new Set(jobsList.flatMap(job => job.assignedTo))];
    this.availableTechnicians.set(technicians.filter(tech => tech));
  }

  private extractMachineType(machineName: string): string {
    // Extract machine type from machine name (e.g., "Excavator 1" -> "Excavator")
    const match = machineName.match(/^([A-Za-z\s]+)/);
    return match ? match[1].trim() : machineName;
  }

  private updateActiveFiltersCount() {
    let count = 0;
    const formValue = this.filterForm.value;

    if (formValue.searchTerm) count++;
    if (formValue.startDate || formValue.endDate) count++;
    if (formValue.status?.length > 0) count++;
    if (formValue.machineType?.length > 0) count++;
    if (formValue.project?.length > 0) count++;
    if (formValue.assignedTo?.length > 0) count++;

    this.activeFiltersCount.set(count);
  }

  // Event Handlers
  onExpandedChange(expanded: boolean) {
    this.isExpanded.set(expanded);
  }

  onSearchInput(event: Event) {
    const target = event.target as HTMLInputElement;
    this.searchChanged.emit(target.value);
  }

  clearSearch() {
    this.filterForm.patchValue({ searchTerm: '' });
    this.searchChanged.emit('');
  }

  removeFilter(filterType: string) {
    switch (filterType) {
      case 'searchTerm':
        this.filterForm.patchValue({ searchTerm: '' });
        break;
      case 'dateRange':
        this.filterForm.patchValue({ startDate: null, endDate: null });
        break;
      case 'status':
        this.filterForm.patchValue({ status: [] });
        break;
      case 'machineType':
        this.filterForm.patchValue({ machineType: [] });
        break;
      case 'project':
        this.filterForm.patchValue({ project: [] });
        break;
      case 'assignedTo':
        this.filterForm.patchValue({ assignedTo: [] });
        break;
    }
    this.applyFilters();
  }

  resetFilters() {
    this.filterForm.reset({
      searchTerm: '',
      startDate: null,
      endDate: null,
      status: [],
      machineType: [],
      project: [],
      assignedTo: []
    });
    this.applyFilters();
  }

  applyFilters() {
    const formValue = this.filterForm.value;
    const filters: JobFilters = {};

    if (formValue.searchTerm) {
      filters.searchTerm = formValue.searchTerm;
    }

    if (formValue.startDate || formValue.endDate) {
      filters.dateRange = {
        start: formValue.startDate || new Date(0),
        end: formValue.endDate || new Date()
      };
    }

    if (formValue.status?.length > 0) {
      filters.status = formValue.status;
    }

    if (formValue.machineType?.length > 0) {
      filters.machineType = formValue.machineType;
    }

    if (formValue.project?.length > 0) {
      filters.project = formValue.project;
    }

    if (formValue.assignedTo?.length > 0) {
      filters.assignedTo = formValue.assignedTo;
    }

    this.filtersChanged.emit(filters);
  }

  // Utility Methods
  getFilterDescription(): string {
    const count = this.activeFiltersCount();
    if (count === 0) {
      return 'No filters applied';
    }
    return `${count} filter${count > 1 ? 's' : ''} applied`;
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

  getStatusIconClass(status: MaintenanceStatus): string {
    return `icon-${status.toLowerCase().replace('_', '-')}`;
  }

  getDateRangeText(): string {
    const startDate = this.filterForm.get('startDate')?.value;
    const endDate = this.filterForm.get('endDate')?.value;
    
    if (startDate && endDate) {
      return `${this.formatDate(startDate)} - ${this.formatDate(endDate)}`;
    } else if (startDate) {
      return `From ${this.formatDate(startDate)}`;
    } else if (endDate) {
      return `Until ${this.formatDate(endDate)}`;
    }
    return '';
  }

  getStatusText(): string {
    const statuses = this.filterForm.get('status')?.value || [];
    if (statuses.length <= 2) {
      return statuses.map((s: MaintenanceStatus) => 
        this.availableStatuses.find(status => status.value === s)?.label
      ).join(', ');
    }
    return `${statuses.length} statuses`;
  }

  getMachineTypeText(): string {
    const types = this.filterForm.get('machineType')?.value || [];
    if (types.length <= 2) {
      return types.join(', ');
    }
    return `${types.length} types`;
  }

  getProjectText(): string {
    const projects = this.filterForm.get('project')?.value || [];
    if (projects.length <= 2) {
      return projects.join(', ');
    }
    return `${projects.length} projects`;
  }

  getAssignedToText(): string {
    const assigned = this.filterForm.get('assignedTo')?.value || [];
    if (assigned.length <= 2) {
      return assigned.join(', ');
    }
    return `${assigned.length} technicians`;
  }

  private formatDate(date: Date): string {
    return new Date(date).toLocaleDateString();
  }
}