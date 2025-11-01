import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule, MatIconButton } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ChangeDetectionStrategy } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MaintenanceJob, MaintenanceType, MaintenanceStatus } from '../models/maintenance.models';
import { MaintenanceService } from '../services/maintenance.service';
import { LoadingSpinnerComponent } from '../shared/loading-spinner/loading-spinner.component';

/**
 * Maintenance History Component
 *
 * Displays completed maintenance records for all machines.
 * Allows filtering by date range, machine, type, and search.
 */
@Component({
  selector: 'app-maintenance-history',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatCardModule,
    MatChipsModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatTooltipModule,
    FormsModule,
    LoadingSpinnerComponent
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './maintenance-history.component.html',
  styleUrls: ['./maintenance-history.component.scss']
})
export class MaintenanceHistoryComponent implements OnInit {
  private maintenanceService = inject(MaintenanceService);

  // State signals
  allRecords = signal<MaintenanceJob[]>([]);
  isLoading = signal<boolean>(false);
  searchTerm = signal<string>('');
  selectedMachines = signal<string[]>([]);
  selectedTypes = signal<MaintenanceType[]>([]);
  dateFrom = signal<Date | null>(null);
  dateTo = signal<Date | null>(null);

  // Pagination
  currentPage = signal<number>(0);
  pageSize = signal<number>(25);
  pageSizeOptions = [10, 25, 50, 100];

  // Available filter options
  availableMachines = computed(() => {
    const machines = this.allRecords().map(r => r.machineName);
    return [...new Set(machines)].sort();
  });

  availableTypes = Object.values(MaintenanceType);

  // Filtered and paginated records
  filteredRecords = computed(() => {
    let records = this.allRecords();

    // Filter by search term
    const search = this.searchTerm().toLowerCase();
    if (search) {
      records = records.filter(r =>
        r.machineName.toLowerCase().includes(search) ||
        (r.serialNumber && r.serialNumber.toLowerCase().includes(search)) ||
        (r.project && r.project.toLowerCase().includes(search)) ||
        r.reason.toLowerCase().includes(search) ||
        (r.observations && r.observations.toLowerCase().includes(search))
      );
    }

    // Filter by machines
    const machines = this.selectedMachines();
    if (machines.length > 0) {
      records = records.filter(r => machines.includes(r.machineName));
    }

    // Filter by types
    const types = this.selectedTypes();
    if (types.length > 0) {
      records = records.filter(r => types.includes(r.type));
    }

    // Filter by date range
    const from = this.dateFrom();
    const to = this.dateTo();
    if (from) {
      records = records.filter(r => new Date(r.scheduledDate) >= from);
    }
    if (to) {
      records = records.filter(r => new Date(r.scheduledDate) <= to);
    }

    return records;
  });

  paginatedRecords = computed(() => {
    const filtered = this.filteredRecords();
    const page = this.currentPage();
    const size = this.pageSize();
    const startIndex = page * size;
    return filtered.slice(startIndex, startIndex + size);
  });

  totalRecords = computed(() => this.filteredRecords().length);

  // Table columns
  displayedColumns = ['machineName', 'serialNumber', 'project', 'date', 'type', 'technicians', 'hours', 'parts'];

  ngOnInit() {
    this.loadMaintenanceHistory();
  }

  private loadMaintenanceHistory() {
    this.isLoading.set(true);

    // Load only completed maintenance jobs
    this.maintenanceService.getMaintenanceJobs().subscribe({
      next: (jobs) => {
        const completedJobs = jobs.filter(job =>
          job.status === MaintenanceStatus.COMPLETED
        );
        this.allRecords.set(completedJobs);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading maintenance history:', error);
        this.isLoading.set(false);
      }
    });
  }

  // Filter methods
  onSearchChange(searchTerm: string) {
    this.searchTerm.set(searchTerm);
    this.currentPage.set(0); // Reset to first page
  }

  onMachineFilterChange(machines: string[]) {
    this.selectedMachines.set(machines);
    this.currentPage.set(0);
  }

  onTypeFilterChange(types: MaintenanceType[]) {
    this.selectedTypes.set(types);
    this.currentPage.set(0);
  }

  onDateFromChange(date: Date | null) {
    this.dateFrom.set(date);
    this.currentPage.set(0);
  }

  onDateToChange(date: Date | null) {
    this.dateTo.set(date);
    this.currentPage.set(0);
  }

  clearFilters() {
    this.searchTerm.set('');
    this.selectedMachines.set([]);
    this.selectedTypes.set([]);
    this.dateFrom.set(null);
    this.dateTo.set(null);
    this.currentPage.set(0);
  }

  // Pagination
  onPageChange(event: PageEvent) {
    this.currentPage.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
  }

  // Formatting utilities
  formatDate(date: Date): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  formatTime(date: Date): string {
    if (!date) return '';
    return new Date(date).toLocaleTimeString('en-US', {
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  getTypeDisplayName(type: MaintenanceType): string {
    const typeNames: Record<MaintenanceType, string> = {
      [MaintenanceType.PREVENTIVE]: 'Preventive',
      [MaintenanceType.CORRECTIVE]: 'Corrective',
      [MaintenanceType.PREDICTIVE]: 'Predictive',
      [MaintenanceType.EMERGENCY]: 'Emergency'
    };
    return typeNames[type] || type;
  }

  getTypeChipClass(type: MaintenanceType): string {
    return `type-${type.toLowerCase()}`;
  }

  getTechniciansList(assignedTo: string[]): string {
    if (!assignedTo || assignedTo.length === 0) return 'Unassigned';
    if (assignedTo.length === 1) return assignedTo[0];
    return `${assignedTo[0]} +${assignedTo.length - 1}`;
  }

  getTechniciansTooltip(assignedTo: string[]): string {
    return assignedTo.join(', ');
  }

  getPartsReplaced(parts?: string[]): string {
    if (!parts || parts.length === 0) return 'None';
    if (parts.length === 1) return parts[0];
    return `${parts.length} parts`;
  }

  getPartsTooltip(parts?: string[]): string {
    if (!parts || parts.length === 0) return 'No parts replaced';
    return parts.join(', ');
  }
}
