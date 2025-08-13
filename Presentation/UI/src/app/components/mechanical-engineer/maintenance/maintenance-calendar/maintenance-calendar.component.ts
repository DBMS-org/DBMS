import { Component, OnInit, signal, computed, inject, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CalendarViewComponent } from './calendar-view/calendar-view.component';
import { TimelineViewComponent } from './timeline-view/timeline-view.component';
import { MaintenanceMockService } from '../services/maintenance-mock.service';
import { MaintenanceErrorHandlerService, MaintenanceError } from '../services/maintenance-error-handler.service';
import { MaintenanceJob } from '../models/maintenance.models';

export type CalendarViewType = 'calendar' | 'timeline';

@Component({
  selector: 'app-maintenance-calendar',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTabsModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    CalendarViewComponent,
    TimelineViewComponent
  ],
  templateUrl: './maintenance-calendar.component.html',
  styleUrl: './maintenance-calendar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MaintenanceCalendarComponent implements OnInit {
  private maintenanceService = inject(MaintenanceMockService);
  protected errorHandler = inject(MaintenanceErrorHandlerService);

  // State signals
  currentView = signal<CalendarViewType>('calendar');
  isLoading = signal(true);
  error = signal<MaintenanceError | null>(null);
  maintenanceJobs = signal<MaintenanceJob[]>([]);
  selectedDate = signal<Date>(new Date());

  // Computed values
  isDataReady = computed(() => {
    return !this.isLoading() && !this.error();
  });

  filteredJobs = computed(() => {
    const jobs = this.maintenanceJobs();
    const selectedDate = this.selectedDate();
    
    // Return empty array if jobs are not loaded yet
    if (!jobs || jobs.length === 0) {
      return [];
    }
    
    // Filter jobs based on current view and selected date
    return jobs.filter(job => {
      const jobDate = new Date(job.scheduledDate);
      if (this.currentView() === 'calendar') {
        // For calendar view, show jobs for the selected month
        return jobDate.getMonth() === selectedDate.getMonth() && 
               jobDate.getFullYear() === selectedDate.getFullYear();
      } else {
        // For timeline view, show all jobs (filtering will be handled by the timeline component)
        return true;
      }
    });
  });

  ngOnInit() {
    this.loadCalendarData();
  }

  private loadCalendarData() {
    this.isLoading.set(true);
    this.error.set(null);

    this.maintenanceService.getMaintenanceJobs().subscribe({
      next: (jobs) => {
        this.maintenanceJobs.set(jobs || []);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error loading calendar data:', err);
        this.error.set(err as MaintenanceError);
        this.isLoading.set(false);
      }
    });
  }

  // View switching methods
  switchToCalendarView() {
    this.currentView.set('calendar');
  }

  switchToTimelineView() {
    this.currentView.set('timeline');
  }

  // Date navigation methods
  onDateSelected(date: Date) {
    this.selectedDate.set(date);
  }

  onMonthChanged(date: Date) {
    this.selectedDate.set(date);
    // Optionally reload data for the new month
    this.loadCalendarData();
  }

  // Job selection handler
  onJobSelected(job: MaintenanceJob) {
    // This will be used to open the job detail panel
    // For now, we'll just log it
    console.log('Job selected:', job);
  }

  // Retry functionality
  retryLoadData() {
    this.loadCalendarData();
  }
}