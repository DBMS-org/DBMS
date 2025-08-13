import { Component, input, output, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { MaintenanceJob, MaintenanceStatus } from '../../models/maintenance.models';

interface CalendarDay {
  date: Date;
  isCurrentMonth: boolean;
  isToday: boolean;
  isSelected: boolean;
  jobs: MaintenanceJob[];
  visibleJobs: MaintenanceJob[];
  hasMoreJobs: boolean;
}

interface CalendarWeek {
  days: CalendarDay[];
}

@Component({
  selector: 'app-calendar-view',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatCardModule
  ],
  templateUrl: './calendar-view.component.html',
  styleUrl: './calendar-view.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CalendarViewComponent {
  // Input signals
  jobs = input<MaintenanceJob[]>([]);
  selectedDate = input<Date>(new Date());

  // Output events
  dateSelected = output<Date>();
  monthChanged = output<Date>();
  jobSelected = output<MaintenanceJob>();

  // Internal state
  currentMonth = signal(new Date());
  
  // Performance configuration
  private readonly MAX_VISIBLE_JOBS_PER_DAY = 3;



  // Computed values
  monthName = computed(() => {
    const month = this.currentMonth();
    return month.toLocaleDateString('en-US', { month: 'long', year: 'numeric' });
  });

  calendarWeeks = computed(() => {
    const jobs = this.jobs();
    const selectedDate = this.selectedDate();
    let month = this.currentMonth();
    
    // Initialize or update current month based on selected date
    if (selectedDate) {
      const selectedMonth = new Date(selectedDate.getFullYear(), selectedDate.getMonth(), 1);
      if (month.getMonth() !== selectedMonth.getMonth() || 
          month.getFullYear() !== selectedMonth.getFullYear()) {
        this.currentMonth.set(selectedMonth);
        month = selectedMonth;
      }
    }
    
    return this.generateCalendarWeeks(month, jobs, selectedDate);
  });

  // Calendar generation with performance optimization for large datasets
  private generateCalendarWeeks(currentMonth: Date, jobs: MaintenanceJob[], selectedDate: Date): CalendarWeek[] {
    const weeks: CalendarWeek[] = [];
    const firstDayOfMonth = new Date(currentMonth.getFullYear(), currentMonth.getMonth(), 1);
    const lastDayOfMonth = new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1, 0);
    
    // Start from the first Sunday of the calendar view
    const startDate = new Date(firstDayOfMonth);
    startDate.setDate(startDate.getDate() - firstDayOfMonth.getDay());
    
    // End at the last Saturday of the calendar view
    const endDate = new Date(lastDayOfMonth);
    endDate.setDate(endDate.getDate() + (6 - lastDayOfMonth.getDay()));
    
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    
    // Performance optimization: Create a map of jobs by date for O(1) lookup
    const jobsByDate = new Map<string, MaintenanceJob[]>();
    jobs.forEach(job => {
      const jobDate = new Date(job.scheduledDate);
      const dateKey = this.getDateKey(jobDate);
      if (!jobsByDate.has(dateKey)) {
        jobsByDate.set(dateKey, []);
      }
      jobsByDate.get(dateKey)!.push(job);
    });
    
    let currentDate = new Date(startDate);
    
    while (currentDate <= endDate) {
      const week: CalendarWeek = { days: [] };
      
      for (let i = 0; i < 7; i++) {
        const dateKey = this.getDateKey(currentDate);
        const dayJobs = jobsByDate.get(dateKey) || [];
        
        // Sort jobs by priority (overdue first, then by status priority)
        const sortedJobs = this.sortJobsByPriority(dayJobs);
        const visibleJobs = sortedJobs.slice(0, this.MAX_VISIBLE_JOBS_PER_DAY);
        const hasMoreJobs = sortedJobs.length > this.MAX_VISIBLE_JOBS_PER_DAY;
        
        const day: CalendarDay = {
          date: new Date(currentDate),
          isCurrentMonth: currentDate.getMonth() === currentMonth.getMonth(),
          isToday: this.isSameDay(currentDate, today),
          isSelected: this.isSameDay(currentDate, selectedDate),
          jobs: sortedJobs,
          visibleJobs: visibleJobs,
          hasMoreJobs: hasMoreJobs
        };
        
        week.days.push(day);
        currentDate.setDate(currentDate.getDate() + 1);
      }
      
      weeks.push(week);
    }
    
    return weeks;
  }

  // Helper method to create consistent date keys for performance optimization
  private getDateKey(date: Date): string {
    return `${date.getFullYear()}-${date.getMonth()}-${date.getDate()}`;
  }

  // Sort jobs by priority to show most important ones first
  private sortJobsByPriority(jobs: MaintenanceJob[]): MaintenanceJob[] {
    return jobs.sort((a, b) => {
      // Priority order: OVERDUE > IN_PROGRESS > SCHEDULED > COMPLETED > CANCELLED
      const statusPriority = {
        [MaintenanceStatus.OVERDUE]: 1,
        [MaintenanceStatus.IN_PROGRESS]: 2,
        [MaintenanceStatus.SCHEDULED]: 3,
        [MaintenanceStatus.COMPLETED]: 4,
        [MaintenanceStatus.CANCELLED]: 5
      };
      
      const priorityA = statusPriority[a.status] || 6;
      const priorityB = statusPriority[b.status] || 6;
      
      if (priorityA !== priorityB) {
        return priorityA - priorityB;
      }
      
      // If same status, sort by scheduled time
      return new Date(a.scheduledDate).getTime() - new Date(b.scheduledDate).getTime();
    });
  }

  private isSameDay(date1: Date, date2: Date): boolean {
    return date1.getFullYear() === date2.getFullYear() &&
           date1.getMonth() === date2.getMonth() &&
           date1.getDate() === date2.getDate();
  }

  // Navigation methods
  previousMonth() {
    const newMonth = new Date(this.currentMonth());
    newMonth.setMonth(newMonth.getMonth() - 1);
    this.currentMonth.set(newMonth);
    this.monthChanged.emit(newMonth);
  }

  nextMonth() {
    const newMonth = new Date(this.currentMonth());
    newMonth.setMonth(newMonth.getMonth() + 1);
    this.currentMonth.set(newMonth);
    this.monthChanged.emit(newMonth);
  }

  goToToday() {
    const today = new Date();
    this.currentMonth.set(today);
    this.dateSelected.emit(today);
    this.monthChanged.emit(today);
  }

  // Event handlers
  onDayClick(day: CalendarDay) {
    this.dateSelected.emit(day.date);
  }

  onJobClick(event: Event, job: MaintenanceJob) {
    event.stopPropagation();
    this.jobSelected.emit(job);
  }

  onMoreJobsClick(event: Event, day: CalendarDay) {
    event.stopPropagation();
    // This could open a modal or expand the day view to show all jobs
    // For now, we'll emit the date selection to show the day detail
    this.dateSelected.emit(day.date);
  }

  // Keyboard navigation support
  onKeyDown(event: KeyboardEvent, day: CalendarDay) {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      this.onDayClick(day);
    }
  }

  onJobKeyDown(event: KeyboardEvent, job: MaintenanceJob) {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      event.stopPropagation();
      this.jobSelected.emit(job);
    }
  }

  // Utility methods
  getStatusColor(status: MaintenanceStatus): string {
    switch (status) {
      case MaintenanceStatus.SCHEDULED:
        return '#2196f3'; // Blue
      case MaintenanceStatus.IN_PROGRESS:
        return '#ff9800'; // Orange
      case MaintenanceStatus.COMPLETED:
        return '#4caf50'; // Green
      case MaintenanceStatus.OVERDUE:
        return '#f44336'; // Red
      case MaintenanceStatus.CANCELLED:
        return '#9e9e9e'; // Grey
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

  getJobTooltip(job: MaintenanceJob): string {
    const time = new Date(job.scheduledDate).toLocaleTimeString('en-US', { 
      hour: '2-digit', 
      minute: '2-digit' 
    });
    return `${job.machineName} - ${job.type} (${time})`;
  }
}