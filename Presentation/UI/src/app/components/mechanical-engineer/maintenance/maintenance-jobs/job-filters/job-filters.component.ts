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
  templateUrl: './job-filters.component.html',
  styleUrls: ['./job-filters.component.scss']
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
    const projects = [...new Set(jobsList.map(job => job.project || job.projectName))];
    this.availableProjects.set(projects.filter(project => project) as string[]);

    // Extract unique technicians
    const technicians = [...new Set(jobsList.flatMap(job => job.assignedTo || []))];
    this.availableTechnicians.set(technicians.filter(tech => tech) as string[]);
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
