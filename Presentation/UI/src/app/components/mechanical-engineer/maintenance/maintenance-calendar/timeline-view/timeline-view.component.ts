import { Component, input, output, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { CdkVirtualScrollViewport, CdkFixedSizeVirtualScroll } from '@angular/cdk/scrolling';
import { MaintenanceJob, MaintenanceStatus, MaintenanceType } from '../../models/maintenance.models';

interface TimelineGroup {
  date: string;
  displayDate: string;
  jobs: MaintenanceJob[];
}

interface TimelineFilters {
  status: MaintenanceStatus[];
  type: MaintenanceType[];
  searchTerm: string;
  sortBy: 'date' | 'machine' | 'status';
  sortOrder: 'asc' | 'desc';
}

@Component({
  selector: 'app-timeline-view',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatCardModule,
    MatChipsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    FormsModule,
    CdkVirtualScrollViewport,
    CdkFixedSizeVirtualScroll
  ],
  templateUrl: './timeline-view.component.html',
  styleUrl: './timeline-view.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TimelineViewComponent {
  // Input signals
  jobs = input<MaintenanceJob[]>([]);

  // Output events
  jobSelected = output<MaintenanceJob>();

  // Filter state
  filters = signal<TimelineFilters>({
    status: [],
    type: [],
    searchTerm: '',
    sortBy: 'date',
    sortOrder: 'asc'
  });

  // Enums for template
  MaintenanceStatus = MaintenanceStatus;
  MaintenanceType = MaintenanceType;

  // Status and type options
  statusOptions = Object.values(MaintenanceStatus);
  typeOptions = Object.values(MaintenanceType);

  // Computed values
  filteredAndSortedJobs = computed(() => {
    const jobs = this.jobs();
    const filters = this.filters();
    
    // Apply filters
    let filtered = jobs.filter(job => {
      // Status filter
      if (filters.status.length > 0 && !filters.status.includes(job.status)) {
        return false;
      }
      
      // Type filter
      if (filters.type.length > 0 && !filters.type.includes(job.type)) {
        return false;
      }
      
      // Search filter
      if (filters.searchTerm) {
        const searchLower = filters.searchTerm.toLowerCase();
        return job.machineName.toLowerCase().includes(searchLower) ||
               job.serialNumber.toLowerCase().includes(searchLower) ||
               job.project.toLowerCase().includes(searchLower) ||
               job.reason.toLowerCase().includes(searchLower);
      }
      
      return true;
    });
    
    // Apply sorting
    filtered.sort((a, b) => {
      let comparison = 0;
      
      switch (filters.sortBy) {
        case 'date':
          comparison = new Date(a.scheduledDate).getTime() - new Date(b.scheduledDate).getTime();
          break;
        case 'machine':
          comparison = a.machineName.localeCompare(b.machineName);
          break;
        case 'status':
          comparison = a.status.localeCompare(b.status);
          break;
      }
      
      return filters.sortOrder === 'desc' ? -comparison : comparison;
    });
    
    return filtered;
  });

  timelineGroups = computed(() => {
    const jobs = this.filteredAndSortedJobs();
    const groups: TimelineGroup[] = [];
    const groupMap = new Map<string, MaintenanceJob[]>();
    
    // Group jobs by date
    jobs.forEach(job => {
      const date = new Date(job.scheduledDate);
      const dateKey = date.toISOString().split('T')[0]; // YYYY-MM-DD format
      
      if (!groupMap.has(dateKey)) {
        groupMap.set(dateKey, []);
      }
      groupMap.get(dateKey)!.push(job);
    });
    
    // Convert to timeline groups
    groupMap.forEach((jobs, dateKey) => {
      const date = new Date(dateKey);
      groups.push({
        date: dateKey,
        displayDate: this.formatDisplayDate(date),
        jobs: jobs.sort((a, b) => 
          new Date(a.scheduledDate).getTime() - new Date(b.scheduledDate).getTime()
        )
      });
    });
    
    // Sort groups by date
    return groups.sort((a, b) => {
      const comparison = new Date(a.date).getTime() - new Date(b.date).getTime();
      return this.filters().sortOrder === 'desc' ? -comparison : comparison;
    });
  });

  // Filter methods
  updateStatusFilter(status: MaintenanceStatus[]) {
    this.filters.update(current => ({ ...current, status }));
  }

  updateTypeFilter(type: MaintenanceType[]) {
    this.filters.update(current => ({ ...current, type }));
  }

  updateSearchTerm(searchTerm: string) {
    this.filters.update(current => ({ ...current, searchTerm }));
  }

  updateSorting(sortBy: 'date' | 'machine' | 'status', sortOrder: 'asc' | 'desc') {
    this.filters.update(current => ({ ...current, sortBy, sortOrder }));
  }

  clearFilters() {
    this.filters.set({
      status: [],
      type: [],
      searchTerm: '',
      sortBy: 'date',
      sortOrder: 'asc'
    });
  }

  // Event handlers
  onJobClick(job: MaintenanceJob) {
    this.jobSelected.emit(job);
  }

  // Utility methods
  formatDisplayDate(date: Date): string {
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1);
    const yesterday = new Date(today);
    yesterday.setDate(yesterday.getDate() - 1);
    
    if (this.isSameDay(date, today)) {
      return 'Today';
    } else if (this.isSameDay(date, tomorrow)) {
      return 'Tomorrow';
    } else if (this.isSameDay(date, yesterday)) {
      return 'Yesterday';
    } else {
      return date.toLocaleDateString('en-US', { 
        weekday: 'long', 
        year: 'numeric', 
        month: 'long', 
        day: 'numeric' 
      });
    }
  }

  private isSameDay(date1: Date, date2: Date): boolean {
    return date1.getFullYear() === date2.getFullYear() &&
           date1.getMonth() === date2.getMonth() &&
           date1.getDate() === date2.getDate();
  }

  formatTime(date: Date | string): string {
    const dateObj = typeof date === 'string' ? new Date(date) : date;
    return dateObj.toLocaleTimeString('en-US', { 
      hour: '2-digit', 
      minute: '2-digit' 
    });
  }

  getStatusColor(status: MaintenanceStatus): string {
    switch (status) {
      case MaintenanceStatus.SCHEDULED:
        return '#2196f3';
      case MaintenanceStatus.IN_PROGRESS:
        return '#ff9800';
      case MaintenanceStatus.COMPLETED:
        return '#4caf50';
      case MaintenanceStatus.OVERDUE:
        return '#f44336';
      case MaintenanceStatus.CANCELLED:
        return '#9e9e9e';
      default:
        return '#2196f3';
    }
  }

  getStatusIcon(status: MaintenanceStatus): string {
    switch (status) {
      case MaintenanceStatus.SCHEDULED:
        return 'schedule';
      case MaintenanceStatus.IN_PROGRESS:
        return 'build';
      case MaintenanceStatus.COMPLETED:
        return 'check_circle';
      case MaintenanceStatus.OVERDUE:
        return 'warning';
      case MaintenanceStatus.CANCELLED:
        return 'cancel';
      default:
        return 'schedule';
    }
  }

  getTypeIcon(type: MaintenanceType): string {
    switch (type) {
      case MaintenanceType.PREVENTIVE:
        return 'schedule';
      case MaintenanceType.CORRECTIVE:
        return 'build';
      case MaintenanceType.PREDICTIVE:
        return 'analytics';
      case MaintenanceType.EMERGENCY:
        return 'warning';
      default:
        return 'build';
    }
  }

  getPriorityClass(job: MaintenanceJob): string {
    if (job.status === MaintenanceStatus.OVERDUE) {
      return 'high-priority';
    }
    
    const dueDate = new Date(job.scheduledDate);
    const today = new Date();
    const daysDiff = Math.ceil((dueDate.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
    
    if (daysDiff <= 1) {
      return 'high-priority';
    } else if (daysDiff <= 3) {
      return 'medium-priority';
    }
    
    return 'low-priority';
  }
}