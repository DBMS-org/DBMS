import { Component, OnInit, inject, signal, output, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChangeDetectionStrategy } from '@angular/core';

// PrimeNG imports
import { PanelModule } from 'primeng/panel';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { MultiSelectModule } from 'primeng/multiselect';
import { CalendarModule } from 'primeng/calendar';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';

import { JobFilters, MaintenanceStatus, MaintenanceJob } from '../../models/maintenance.models';

@Component({
  selector: 'app-job-filters',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    PanelModule,
    ButtonModule,
    InputTextModule,
    DropdownModule,
    MultiSelectModule,
    CalendarModule,
    TagModule,
    TooltipModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './job-filters.component.html',
  styleUrls: ['./job-filters.component.scss']
})
export class JobFiltersComponent implements OnInit {
  // Inputs
  jobs = input<MaintenanceJob[]>([]);
  initialFilters = input<JobFilters>({});

  // Outputs
  filtersChanged = output<JobFilters>();
  searchChanged = output<string>();

  // Signals
  activeFiltersCount = signal(0);
  availableMachineTypes = signal<string[]>([]);
  availableProjects = signal<string[]>([]);
  availableTechnicians = signal<string[]>([]);

  // Panel state - needs to be a regular boolean for PrimeNG two-way binding
  isFiltersPanelCollapsed: boolean = true;

  // Filter values
  searchTerm: string = '';
  startDate: Date | null = null;
  endDate: Date | null = null;
  selectedStatuses: MaintenanceStatus[] = [];
  selectedMachineTypes: string[] = [];
  selectedProjects: string[] = [];
  selectedTechnicians: string[] = [];

  // Configuration
  availableStatuses = [
    { value: MaintenanceStatus.SCHEDULED, label: 'Scheduled' },
    { value: MaintenanceStatus.IN_PROGRESS, label: 'In Progress' },
    { value: MaintenanceStatus.COMPLETED, label: 'Completed' },
    { value: MaintenanceStatus.CANCELLED, label: 'Cancelled' },
    { value: MaintenanceStatus.OVERDUE, label: 'Overdue' }
  ];

  ngOnInit() {
    this.initializeFilters();
    this.updateAvailableOptions();
  }

  private initializeFilters() {
    const initial = this.initialFilters();
    if (initial) {
      this.searchTerm = initial.searchTerm || '';
      this.startDate = initial.dateRange?.start || null;
      this.endDate = initial.dateRange?.end || null;
      this.selectedStatuses = initial.status || [];
      this.selectedMachineTypes = initial.machineType || [];
      this.selectedProjects = initial.project || [];
      this.selectedTechnicians = initial.assignedTo || [];
    }
    this.updateActiveFiltersCount();
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

    if (this.searchTerm) count++;
    if (this.startDate || this.endDate) count++;
    if (this.selectedStatuses?.length > 0) count++;
    if (this.selectedMachineTypes?.length > 0) count++;
    if (this.selectedProjects?.length > 0) count++;
    if (this.selectedTechnicians?.length > 0) count++;

    this.activeFiltersCount.set(count);
  }

  // Event Handlers
  toggleFiltersPanel() {
    this.isFiltersPanelCollapsed = !this.isFiltersPanelCollapsed;
  }

  onSearch() {
    this.searchChanged.emit(this.searchTerm);
    this.updateActiveFiltersCount();
  }

  clearFilters() {
    this.searchTerm = '';
    this.startDate = null;
    this.endDate = null;
    this.selectedStatuses = [];
    this.selectedMachineTypes = [];
    this.selectedProjects = [];
    this.selectedTechnicians = [];
    this.applyFilters();
  }

  applyFilters() {
    const filters: JobFilters = {};

    if (this.searchTerm) {
      filters.searchTerm = this.searchTerm;
    }

    if (this.startDate || this.endDate) {
      filters.dateRange = {
        start: this.startDate || new Date(0),
        end: this.endDate || new Date()
      };
    }

    if (this.selectedStatuses?.length > 0) {
      filters.status = this.selectedStatuses;
    }

    if (this.selectedMachineTypes?.length > 0) {
      filters.machineType = this.selectedMachineTypes;
    }

    if (this.selectedProjects?.length > 0) {
      filters.project = this.selectedProjects;
    }

    if (this.selectedTechnicians?.length > 0) {
      filters.assignedTo = this.selectedTechnicians;
    }

    this.filtersChanged.emit(filters);
    this.updateActiveFiltersCount();
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
    if (this.startDate && this.endDate) {
      return `${this.formatDate(this.startDate)} - ${this.formatDate(this.endDate)}`;
    } else if (this.startDate) {
      return `From ${this.formatDate(this.startDate)}`;
    } else if (this.endDate) {
      return `Until ${this.formatDate(this.endDate)}`;
    }
    return '';
  }

  getStatusText(): string {
    if (this.selectedStatuses.length <= 2) {
      return this.selectedStatuses.map((s: MaintenanceStatus) =>
        this.availableStatuses.find(status => status.value === s)?.label
      ).join(', ');
    }
    return `${this.selectedStatuses.length} statuses`;
  }

  getMachineTypeText(): string {
    if (this.selectedMachineTypes.length <= 2) {
      return this.selectedMachineTypes.join(', ');
    }
    return `${this.selectedMachineTypes.length} types`;
  }

  getProjectText(): string {
    if (this.selectedProjects.length <= 2) {
      return this.selectedProjects.join(', ');
    }
    return `${this.selectedProjects.length} projects`;
  }

  getAssignedToText(): string {
    if (this.selectedTechnicians.length <= 2) {
      return this.selectedTechnicians.join(', ');
    }
    return `${this.selectedTechnicians.length} technicians`;
  }

  private formatDate(date: Date): string {
    return new Date(date).toLocaleDateString();
  }
}
