import { ComponentFixture, TestBed } from '@angular/core/testing';
import { signal } from '@angular/core';
import { CalendarViewComponent } from './calendar-view.component';
import { MaintenanceJob, MaintenanceStatus, MaintenanceType } from '../../models/maintenance.models';

describe('CalendarViewComponent', () => {
  let component: CalendarViewComponent;
  let fixture: ComponentFixture<CalendarViewComponent>;

  const mockJobs: MaintenanceJob[] = [
    {
      id: 1,
      machineId: 1,
      machineName: 'Excavator CAT 320',
      serialNumber: 'CAT320-2023-001',
      project: 'Mining Project Alpha',
      scheduledDate: new Date(2024, 11, 15),
      type: MaintenanceType.PREVENTIVE,
      status: MaintenanceStatus.SCHEDULED,
      assignedTo: ['John Smith'],
      estimatedHours: 8,
      reason: 'Regular maintenance',
      createdAt: new Date(),
      updatedAt: new Date()
    },
    {
      id: 2,
      machineId: 2,
      machineName: 'Bulldozer Komatsu D65',
      serialNumber: 'KOMD65-2022-045',
      project: 'Construction Site Beta',
      scheduledDate: new Date(2024, 11, 15),
      type: MaintenanceType.CORRECTIVE,
      status: MaintenanceStatus.OVERDUE,
      assignedTo: ['Sarah Wilson'],
      estimatedHours: 12,
      reason: 'Hydraulic system repair',
      createdAt: new Date(),
      updatedAt: new Date()
    }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CalendarViewComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(CalendarViewComponent);
    component = fixture.componentInstance;
    
    // Set required inputs using signals
    fixture.componentRef.setInput('jobs', mockJobs);
    fixture.componentRef.setInput('selectedDate', new Date(2024, 11, 15));
    
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display month name correctly', () => {
    const monthName = component.monthName();
    expect(monthName).toContain('December');
    expect(monthName).toContain('2024');
  });

  it('should generate calendar weeks', () => {
    const weeks = component.calendarWeeks();
    expect(weeks).toBeDefined();
    expect(weeks.length).toBeGreaterThan(0);
    expect(weeks[0].days.length).toBe(7);
  });

  it('should handle job priority sorting', () => {
    const weeks = component.calendarWeeks();
    const dayWithJobs = weeks.flat()
      .map(week => week.days)
      .flat()
      .find(day => day.jobs.length > 0);
    
    if (dayWithJobs) {
      // Overdue jobs should come first
      const overdueJob = dayWithJobs.jobs.find(job => job.status === MaintenanceStatus.OVERDUE);
      const scheduledJob = dayWithJobs.jobs.find(job => job.status === MaintenanceStatus.SCHEDULED);
      
      if (overdueJob && scheduledJob) {
        const overdueIndex = dayWithJobs.jobs.indexOf(overdueJob);
        const scheduledIndex = dayWithJobs.jobs.indexOf(scheduledJob);
        expect(overdueIndex).toBeLessThan(scheduledIndex);
      }
    }
  });

  it('should limit visible jobs per day', () => {
    const weeks = component.calendarWeeks();
    const dayWithJobs = weeks.flat()
      .map(week => week.days)
      .flat()
      .find(day => day.jobs.length > 0);
    
    if (dayWithJobs) {
      expect(dayWithJobs.visibleJobs.length).toBeLessThanOrEqual(3);
    }
  });

  it('should emit dateSelected when day is clicked', () => {
    spyOn(component.dateSelected, 'emit');
    const testDate = new Date(2024, 11, 15);
    const mockDay = {
      date: testDate,
      isCurrentMonth: true,
      isToday: false,
      isSelected: false,
      jobs: [],
      visibleJobs: [],
      hasMoreJobs: false
    };

    component.onDayClick(mockDay);
    expect(component.dateSelected.emit).toHaveBeenCalledWith(testDate);
  });

  it('should emit jobSelected when job is clicked', () => {
    spyOn(component.jobSelected, 'emit');
    const mockEvent = new Event('click');
    
    component.onJobClick(mockEvent, mockJobs[0]);
    expect(component.jobSelected.emit).toHaveBeenCalledWith(mockJobs[0]);
  });

  it('should get correct status colors', () => {
    expect(component.getStatusColor(MaintenanceStatus.SCHEDULED)).toBe('#2196f3');
    expect(component.getStatusColor(MaintenanceStatus.IN_PROGRESS)).toBe('#ff9800');
    expect(component.getStatusColor(MaintenanceStatus.COMPLETED)).toBe('#4caf50');
    expect(component.getStatusColor(MaintenanceStatus.OVERDUE)).toBe('#f44336');
    expect(component.getStatusColor(MaintenanceStatus.CANCELLED)).toBe('#9e9e9e');
  });

  it('should get correct status icons', () => {
    expect(component.getStatusIcon(MaintenanceStatus.SCHEDULED)).toBe('schedule');
    expect(component.getStatusIcon(MaintenanceStatus.IN_PROGRESS)).toBe('build');
    expect(component.getStatusIcon(MaintenanceStatus.COMPLETED)).toBe('check_circle');
    expect(component.getStatusIcon(MaintenanceStatus.OVERDUE)).toBe('warning');
    expect(component.getStatusIcon(MaintenanceStatus.CANCELLED)).toBe('cancel');
  });

  it('should handle keyboard navigation', () => {
    spyOn(component.dateSelected, 'emit');
    const mockDay = {
      date: new Date(2024, 11, 15),
      isCurrentMonth: true,
      isToday: false,
      isSelected: false,
      jobs: [],
      visibleJobs: [],
      hasMoreJobs: false
    };

    const enterEvent = new KeyboardEvent('keydown', { key: 'Enter' });
    spyOn(enterEvent, 'preventDefault');
    
    component.onKeyDown(enterEvent, mockDay);
    expect(enterEvent.preventDefault).toHaveBeenCalled();
    expect(component.dateSelected.emit).toHaveBeenCalledWith(mockDay.date);
  });
});
