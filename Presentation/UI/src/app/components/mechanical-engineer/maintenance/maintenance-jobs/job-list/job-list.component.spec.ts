import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';

import { JobListComponent } from './job-list.component';
import { MaintenanceService } from '../../services/maintenance.service';
import { MaintenanceJob, MaintenanceStatus, MaintenanceType } from '../../models/maintenance.models';

describe('JobListComponent', () => {
  let component: JobListComponent;
  let fixture: ComponentFixture<JobListComponent>;
  let mockMaintenanceService: jasmine.SpyObj<MaintenanceService>;

  const mockJobs: MaintenanceJob[] = [
    {
      id: '1',
      machineId: 'M001',
      machineName: 'Excavator 1',
      serialNumber: 'EX001',
      project: 'Project A',
      scheduledDate: new Date('2024-01-15T10:00:00'),
      type: MaintenanceType.PREVENTIVE,
      status: MaintenanceStatus.SCHEDULED,
      assignedTo: ['John Doe'],
      estimatedHours: 4,
      reason: 'Regular maintenance',
      createdAt: new Date('2024-01-01'),
      updatedAt: new Date('2024-01-01')
    },
    {
      id: '2',
      machineId: 'M002',
      machineName: 'Bulldozer 1',
      serialNumber: 'BD001',
      project: 'Project B',
      scheduledDate: new Date('2024-01-16T14:00:00'),
      type: MaintenanceType.CORRECTIVE,
      status: MaintenanceStatus.IN_PROGRESS,
      assignedTo: ['Jane Smith', 'Bob Johnson'],
      estimatedHours: 6,
      reason: 'Engine repair',
      createdAt: new Date('2024-01-02'),
      updatedAt: new Date('2024-01-02')
    }
  ];

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('MaintenanceService', ['getMaintenanceJobs']);

    await TestBed.configureTestingModule({
      imports: [JobListComponent, NoopAnimationsModule],
      providers: [
        { provide: MaintenanceService, useValue: spy }
      ]
    }).compileComponents();

    mockMaintenanceService = TestBed.inject(MaintenanceService) as jasmine.SpyObj<MaintenanceService>;
    mockMaintenanceService.getMaintenanceJobs.and.returnValue(of(mockJobs));

    fixture = TestBed.createComponent(JobListComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load jobs on init', () => {
    fixture.detectChanges();
    expect(mockMaintenanceService.getMaintenanceJobs).toHaveBeenCalled();
    expect(component.jobs()).toEqual(mockJobs);
  });

  it('should display correct number of jobs in table', () => {
    fixture.detectChanges();
    const tableRows = fixture.nativeElement.querySelectorAll('tr.job-row');
    expect(tableRows.length).toBe(mockJobs.length);
  });

  it('should emit machineClicked when machine name is clicked', () => {
    spyOn(component.machineClicked, 'emit');
    fixture.detectChanges();

    const machineButton = fixture.nativeElement.querySelector('.machine-link');
    machineButton.click();

    expect(component.machineClicked.emit).toHaveBeenCalledWith(mockJobs[0]);
  });

  it('should emit jobSelected when row is clicked', () => {
    spyOn(component.jobSelected, 'emit');
    fixture.detectChanges();

    const jobRow = fixture.nativeElement.querySelector('tr.job-row');
    jobRow.click();

    expect(component.jobSelected.emit).toHaveBeenCalledWith(mockJobs[0]);
  });

  it('should handle sorting correctly', () => {
    fixture.detectChanges();
    
    // Test sorting by machine name
    component.onSortChange({ active: 'machineName', direction: 'asc' });
    expect(component.sortField()).toBe('machineName');
    expect(component.sortDirection()).toBe('asc');

    const sortedJobs = component.sortedJobs();
    expect(sortedJobs[0].machineName).toBe('Bulldozer 1');
    expect(sortedJobs[1].machineName).toBe('Excavator 1');
  });

  it('should handle pagination correctly', () => {
    fixture.detectChanges();
    
    component.onPageChange({ pageIndex: 1, pageSize: 1, length: 2 });
    expect(component.currentPage()).toBe(1);
    expect(component.pageSize()).toBe(1);

    const paginatedJobs = component.paginatedJobs();
    expect(paginatedJobs.length).toBe(1);
    expect(paginatedJobs[0]).toEqual(mockJobs[1]);
  });

  it('should handle selection correctly', () => {
    fixture.detectChanges();
    
    // Test single selection
    component.selection.select(mockJobs[0]);
    expect(component.selection.isSelected(mockJobs[0])).toBe(true);

    // Test select all
    component.toggleAllRows();
    expect(component.isAllSelected()).toBe(true);

    // Test clear selection
    component.clearSelection();
    expect(component.selection.selected.length).toBe(0);
  });

  it('should emit bulkStatusChanged when bulk status is changed', () => {
    spyOn(component.bulkStatusChanged, 'emit');
    fixture.detectChanges();

    component.selection.select(mockJobs[0]);
    component.onBulkStatusChange(MaintenanceStatus.COMPLETED);

    expect(component.bulkStatusChanged.emit).toHaveBeenCalledWith({
      jobs: [mockJobs[0]],
      status: MaintenanceStatus.COMPLETED
    });
  });

  it('should format dates correctly', () => {
    const testDate = new Date('2024-01-15T10:30:00');
    const formattedDate = component.formatDate(testDate);
    const formattedTime = component.formatTime(testDate);

    expect(formattedDate).toBe('1/15/2024');
    expect(formattedTime).toBe('10:30 AM');
  });

  it('should return correct status display names', () => {
    expect(component.getStatusDisplayName(MaintenanceStatus.SCHEDULED)).toBe('Scheduled');
    expect(component.getStatusDisplayName(MaintenanceStatus.IN_PROGRESS)).toBe('In Progress');
    expect(component.getStatusDisplayName(MaintenanceStatus.COMPLETED)).toBe('Completed');
  });

  it('should return correct status icons', () => {
    expect(component.getStatusIcon(MaintenanceStatus.SCHEDULED)).toBe('schedule');
    expect(component.getStatusIcon(MaintenanceStatus.IN_PROGRESS)).toBe('play_circle');
    expect(component.getStatusIcon(MaintenanceStatus.COMPLETED)).toBe('check_circle');
  });

  it('should return correct type display names', () => {
    expect(component.getTypeDisplayName(MaintenanceType.PREVENTIVE)).toBe('Preventive');
    expect(component.getTypeDisplayName(MaintenanceType.CORRECTIVE)).toBe('Corrective');
    expect(component.getTypeDisplayName(MaintenanceType.PREDICTIVE)).toBe('Predictive');
    expect(component.getTypeDisplayName(MaintenanceType.EMERGENCY)).toBe('Emergency');
  });

  it('should generate correct CSS classes for status and type', () => {
    expect(component.getStatusChipClass(MaintenanceStatus.IN_PROGRESS)).toBe('status-in-progress');
    expect(component.getTypeChipClass(MaintenanceType.PREVENTIVE)).toBe('type-preventive');
  });

  it('should generate correct assigned to tooltip', () => {
    const assignedTo = ['John Doe', 'Jane Smith', 'Bob Johnson'];
    const tooltip = component.getAssignedTooltip(assignedTo);
    expect(tooltip).toBe('John Doe, Jane Smith, Bob Johnson');
  });
});