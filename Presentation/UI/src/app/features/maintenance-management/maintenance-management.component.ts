import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { DropdownModule } from 'primeng/dropdown';
import { TooltipModule } from 'primeng/tooltip';
import { MaintenanceJobService, MaintenanceJobDto, MaintenanceStatsDto, ServiceDueAlertDto } from './maintenance-job.service';

@Component({
  selector: 'app-maintenance-management',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    TableModule,
    DropdownModule,
    TooltipModule
  ],
  templateUrl: './maintenance-management.component.html',
  styleUrls: ['./maintenance-management.component.scss']
})
export class MaintenanceManagementComponent implements OnInit {
  // Data
  maintenanceJobs: MaintenanceJobDto[] = [];
  filteredJobs: MaintenanceJobDto[] = [];
  paginatedJobs: MaintenanceJobDto[] = [];
  serviceAlerts: ServiceDueAlertDto[] = [];
  statistics: MaintenanceStatsDto = {
    totalJobs: 0,
    scheduledJobs: 0,
    inProgressJobs: 0,
    completedJobs: 0,
    overdueJobs: 0,
    emergencyJobs: 0,
    averageCompletionHours: 0,
    serviceDueCount: 0
  };

  // State
  isLoading = false;
  errorMessage = '';
  lastUpdated = new Date();

  // Filters
  searchTerm = '';
  selectedStatus = '';
  selectedType = '';

  // Sorting
  sortField = 'scheduledDate';
  sortDirection: 'asc' | 'desc' = 'desc';

  // Pagination
  currentPage = 1;
  pageSize = 25;
  totalPages = 0;

  constructor(
    private maintenanceService: MaintenanceJobService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadMaintenanceData();
    this.loadServiceAlerts();
  }

  loadMaintenanceData(): void {
    this.isLoading = true;
    this.errorMessage = '';

    const filters: any = {};
    if (this.selectedStatus) filters.status = this.selectedStatus;
    if (this.selectedType) filters.type = this.selectedType;

    this.maintenanceService.getAllJobs(filters).subscribe({
      next: (jobs) => {
        this.maintenanceJobs = jobs;
        this.applyFilters();
        this.calculateStatistics();
        this.lastUpdated = new Date();
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load maintenance data. Please try again.';
        console.error('Error loading maintenance data:', err);
        this.isLoading = false;
      }
    });
  }

  loadServiceAlerts(): void {
    this.maintenanceService.getServiceAlerts().subscribe({
      next: (alerts) => {
        this.serviceAlerts = alerts;
      },
      error: (err) => {
        console.error('Error loading service alerts:', err);
      }
    });
  }

  calculateStatistics(): void {
    this.statistics = {
      totalJobs: this.maintenanceJobs.length,
      scheduledJobs: this.maintenanceJobs.filter(j => j.status === 'Scheduled').length,
      inProgressJobs: this.maintenanceJobs.filter(j => j.status === 'InProgress').length,
      completedJobs: this.maintenanceJobs.filter(j => j.status === 'Completed').length,
      overdueJobs: this.maintenanceJobs.filter(j => j.status === 'Overdue').length,
      emergencyJobs: this.maintenanceJobs.filter(j => j.type === 'Emergency').length,
      averageCompletionHours: 0,
      serviceDueCount: this.serviceAlerts.length
    };
  }

  // Filtering
  applyFilters(): void {
    let filtered = [...this.maintenanceJobs];

    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(job =>
        job.machineName?.toLowerCase().includes(term) ||
        job.serialNumber?.toLowerCase().includes(term) ||
        job.projectName?.toLowerCase().includes(term) ||
        job.reason?.toLowerCase().includes(term)
      );
    }

    this.filteredJobs = filtered;
    this.updatePagination();
  }

  onSearchChange(): void {
    this.currentPage = 1;
    this.applyFilters();
  }

  onStatusFilterChange(): void {
    this.currentPage = 1;
    this.loadMaintenanceData();
  }

  onTypeFilterChange(): void {
    this.currentPage = 1;
    this.loadMaintenanceData();
  }

  clearAllFilters(): void {
    this.searchTerm = '';
    this.selectedStatus = '';
    this.selectedType = '';
    this.currentPage = 1;
    this.loadMaintenanceData();
  }

  // Sorting
  sortBy(field: string): void {
    if (this.sortField === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortField = field;
      this.sortDirection = 'asc';
    }

    this.filteredJobs.sort((a, b) => {
      let aValue = (a as any)[field];
      let bValue = (b as any)[field];

      if (field === 'scheduledDate' || field === 'completedDate') {
        aValue = aValue ? new Date(aValue).getTime() : 0;
        bValue = bValue ? new Date(bValue).getTime() : 0;
      }

      if (aValue < bValue) return this.sortDirection === 'asc' ? -1 : 1;
      if (aValue > bValue) return this.sortDirection === 'asc' ? 1 : -1;
      return 0;
    });

    this.updatePagination();
  }

  // Pagination
  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredJobs.length / this.pageSize);
    if (this.currentPage > this.totalPages && this.totalPages > 0) {
      this.currentPage = 1;
    }
    this.updatePaginatedRecords();
  }

  updatePaginatedRecords(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedJobs = this.filteredJobs.slice(startIndex, endIndex);
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePaginatedRecords();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePaginatedRecords();
    }
  }

  goToPage(page: number): void {
    this.currentPage = page;
    this.updatePaginatedRecords();
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.updatePagination();
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisible = 5;
    let start = Math.max(1, this.currentPage - Math.floor(maxVisible / 2));
    let end = Math.min(this.totalPages, start + maxVisible - 1);

    if (end - start + 1 < maxVisible) {
      start = Math.max(1, end - maxVisible + 1);
    }

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    return pages;
  }

  getStartIndex(): number {
    return (this.currentPage - 1) * this.pageSize;
  }

  getEndIndex(): number {
    return Math.min(this.getStartIndex() + this.pageSize, this.filteredJobs.length);
  }

  // Navigation
  viewJobDetails(job: MaintenanceJobDto): void {
    const currentUrl = this.router.url;
    if (currentUrl.includes('/admin/')) {
      this.router.navigate(['/admin/maintenance-management/job', job.id]);
    } else {
      this.router.navigate(['/machine-manager/maintenance-management/job', job.id]);
    }
  }

  // Utility
  refreshData(): void {
    this.loadMaintenanceData();
    this.loadServiceAlerts();
  }

  clearError(): void {
    this.errorMessage = '';
  }

  getAssignedEngineer(job: MaintenanceJobDto): string {
    if (job.assignments && job.assignments.length > 0) {
      return job.assignments[0].mechanicalEngineerName;
    }
    return 'Unassigned';
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Scheduled': return 'status-scheduled';
      case 'InProgress': return 'status-in-progress';
      case 'Completed': return 'status-completed';
      case 'Overdue': return 'status-overdue';
      case 'Cancelled': return 'status-cancelled';
      default: return '';
    }
  }

  getPriorityClass(alertLevel: string | null): string {
    switch (alertLevel) {
      case 'Critical': return 'priority-critical';
      case 'High': return 'priority-high';
      case 'Medium': return 'priority-medium';
      default: return 'priority-low';
    }
  }

  getAlertLevelClass(alertLevel: string): string {
    switch (alertLevel) {
      case 'Critical': return 'alert-critical';
      case 'High': return 'alert-high';
      case 'Medium': return 'alert-medium';
      default: return 'alert-low';
    }
  }
}
