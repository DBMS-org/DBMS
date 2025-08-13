import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';

import { JobDetailPanelComponent } from './job-detail-panel.component';
import { MaintenanceService } from '../../services/maintenance.service';
import { MaintenanceJob, MachineMaintenanceHistory, MaintenanceStatus, MaintenanceType, MaintenanceRecord } from '../../models/maintenance.models';

describe('JobDetailPanelComponent', () => {
  let component: JobDetailPanelComponent;
  let fixture: ComponentFixture<JobDetailPanelComponent>;
  let mockMaintenanceService: jasmine.SpyObj<MaintenanceService>;

  const mockJob: MaintenanceJob = {
    id: '1',
    machineId: 'M001',
    machineName: 'Excavator 1',
    serialNumber: 'EX001',
    project: 'Project A',
    scheduledDate: new Date('2024-01-15T10:00:00'),
    type: MaintenanceType.PREVENTIVE,
    status: MaintenanceStatus.SCHEDULED,
    assignedTo: ['John Doe', 'Jane Smith'],
    estimatedHours: 4,
    actualHours: 3.5,
    reason: 'Regular maintenance',
    observations: 'Machine running well',
    partsReplaced: ['Oil Filter', 'Air Filter'],
    attachments: [
      {
        id: 'att1',
        fileName: 'maintenance_report.pdf',
        fileType: 'application/pdf',
        fileSize: 1024000,
        uploadedAt: new Date(),
        url: '/files/att1'
      }
    ],
    createdAt: new Date('2024-01-01'),
    updatedAt: new Date('2024-01-01')
  };

  const mockMachineHistory: MachineMaintenanceHistory = {
    machineId: 'M001',
    machineName: 'Excavator 1',
    model: 'CAT 320',
    serialNumber: 'EX001',
    currentStatus: 'Operational',
    lastServiceDate: new Date('2023-12-01'),
    nextServiceDate: new Date('2024-02-01'),
    engineHours: 1250,
    serviceHours: 1200,
    idleHours: 50,
    maintenanceRecords: [
      {
        id: 'rec1',
        date: new Date('2023-12-01'),
        type: MaintenanceType.PREVENTIVE,
        description: 'Regular maintenance service',
        technician: 'John Doe',
        hoursSpent: 4,
        partsUsed: ['Oil Filter', 'Air Filter'],
        notes: 'All systems checked and working properly'
      },
      {
        id: 'rec2',
        date: new Date('2023-11-15'),
        type: MaintenanceType.CORRECTIVE,
        description: 'Hydraulic system repair',
        technician: 'Jane Smith',
        hoursSpent: 6,
        partsUsed: ['Hydraulic Seal'],
        notes: 'Replaced damaged hydraulic seal'
      }
    ]
  };

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('MaintenanceService', ['getMachineMaintenanceHistory']);

    await TestBed.configureTestingModule({
      imports: [JobDetailPanelComponent, NoopAnimationsModule],
      providers: [
        { provide: MaintenanceService, useValue: spy }
      ]
    }).compileComponents();

    mockMaintenanceService = TestBed.inject(MaintenanceService) as jasmine.SpyObj<MaintenanceService>;
    mockMaintenanceService.getMachineMaintenanceHistory.and.returnValue(of(mockMachineHistory));

    fixture = TestBed.createComponent(JobDetailPanelComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display empty panel when no job is selected', () => {
    fixture.componentRef.setInput('selectedJob', null);
    fixture.componentRef.setInput('isOpen', true);
    fixture.detectChanges();

    const emptyPanel = fixture.nativeElement.querySelector('.empty-panel');
    expect(emptyPanel).toBeTruthy();
    expect(emptyPanel.textContent).toContain('No Job Selected');
  });

  it('should load machine history when job is selected', () => {
    fixture.componentRef.setInput('selectedJob', mockJob);
    fixture.componentRef.setInput('isOpen', true);
    fixture.detectChanges();

    expect(mockMaintenanceService.getMachineMaintenanceHistory).toHaveBeenCalledWith('M001');
    expect(component.machineHistory()).toEqual(mockMachineHistory);
  });

  it('should display job details when job is selected', () => {
    fixture.componentRef.setInput('selectedJob', mockJob);
    fixture.componentRef.setInput('isOpen', true);
    fixture.detectChanges();

    const jobTitle = fixture.nativeElement.querySelector('.job-title h3');
    expect(jobTitle.textContent).toContain('Excavator 1');
  });

  it('should emit panelClosed when close button is clicked', () => {
    spyOn(component.panelClosed, 'emit');
    fixture.componentRef.setInput('selectedJob', mockJob);
    fixture.componentRef.setInput('isOpen', true);
    fixture.detectChanges();

    const closeButton = fixture.nativeElement.querySelector('.header-content button');
    closeButton.click();

    expect(component.panelClosed.emit).toHaveBeenCalled();
  });

  it('should emit editJob when edit button is clicked', () => {
    spyOn(component.editJob, 'emit');
    fixture.componentRef.setInput('selectedJob', mockJob);
    fixture.componentRef.setInput('isOpen', true);
    fixture.detectChanges();

    const editButton = fixture.nativeElement.querySelector('.panel-actions button[mat-stroked-button]');
    editButton.click();

    expect(component.editJob.emit).toHaveBeenCalledWith(mockJob);
  });

  it('should emit updateStatus when update status button is clicked', () => {
    spyOn(component.updateStatus, 'emit');
    fixture.componentRef.setInput('selectedJob', mockJob);
    fixture.componentRef.setInput('isOpen', true);
    fixture.detectChanges();

    const updateButton = fixture.nativeElement.querySelector('.panel-actions button[mat-raised-button]');
    updateButton.click();

    expect(component.updateStatus.emit).toHaveBeenCalledWith(mockJob);
  });

  it('should format dates correctly', () => {
    const testDate = new Date('2024-01-15T10:30:00');
    const formattedDate = component.formatDate(testDate);
    const formattedDateTime = component.formatDateTime(testDate);

    expect(formattedDate).toBe('1/15/2024');
    expect(formattedDateTime).toContain('1/15/2024');
    expect(formattedDateTime).toContain('10:30');
  });

  it('should format file size correctly', () => {
    expect(component.formatFileSize(0)).toBe('0 Bytes');
    expect(component.formatFileSize(1024)).toBe('1 KB');
    expect(component.formatFileSize(1024000)).toBe('1000 KB');
    expect(component.formatFileSize(1048576)).toBe('1 MB');
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

  it('should return correct type icons', () => {
    expect(component.getTypeIcon(MaintenanceType.PREVENTIVE)).toBe('schedule');
    expect(component.getTypeIcon(MaintenanceType.CORRECTIVE)).toBe('build');
    expect(component.getTypeIcon(MaintenanceType.PREDICTIVE)).toBe('analytics');
    expect(component.getTypeIcon(MaintenanceType.EMERGENCY)).toBe('warning');
  });

  it('should return correct file icons', () => {
    expect(component.getFileIcon('image/jpeg')).toBe('image');
    expect(component.getFileIcon('application/pdf')).toBe('picture_as_pdf');
    expect(component.getFileIcon('text/plain')).toBe('description');
    expect(component.getFileIcon('application/unknown')).toBe('attach_file');
  });

  it('should generate correct CSS classes', () => {
    expect(component.getStatusChipClass(MaintenanceStatus.IN_PROGRESS)).toBe('status-in-progress');
    expect(component.getTypeChipClass(MaintenanceType.PREVENTIVE)).toBe('type-preventive');
    expect(component.getStatusIconClass(MaintenanceStatus.OVERDUE)).toBe('icon-overdue');
    expect(component.getTypeIconClass(MaintenanceType.EMERGENCY)).toBe('type-icon-emergency');
  });

  it('should calculate hours since last service correctly', () => {
    fixture.componentRef.setInput('selectedJob', mockJob);
    fixture.detectChanges();

    // Mock machine history has engineHours: 1250, serviceHours: 1200
    const hoursSinceService = component.getHoursSinceLastService();
    expect(hoursSinceService).toBe(50);
  });

  it('should return correct next service class based on due date', () => {
    // Test with overdue service
    const overdueHistory = {
      ...mockMachineHistory,
      nextServiceDate: new Date(Date.now() - 7 * 24 * 60 * 60 * 1000) // 7 days ago
    };
    component.machineHistory.set(overdueHistory);
    expect(component.getNextServiceClass()).toBe('next-service-overdue');

    // Test with service due soon
    const dueSoonHistory = {
      ...mockMachineHistory,
      nextServiceDate: new Date(Date.now() + 3 * 24 * 60 * 60 * 1000) // 3 days from now
    };
    component.machineHistory.set(dueSoonHistory);
    expect(component.getNextServiceClass()).toBe('next-service-due-soon');

    // Test with service not due soon
    const notDueHistory = {
      ...mockMachineHistory,
      nextServiceDate: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000) // 30 days from now
    };
    component.machineHistory.set(notDueHistory);
    expect(component.getNextServiceClass()).toBe('');
  });

  it('should display maintenance records in timeline', () => {
    fixture.componentRef.setInput('selectedJob', mockJob);
    fixture.componentRef.setInput('isOpen', true);
    fixture.detectChanges();

    // Wait for async operations
    setTimeout(() => {
      fixture.detectChanges();
      const timelineItems = fixture.nativeElement.querySelectorAll('.timeline-item');
      expect(timelineItems.length).toBe(mockMachineHistory.maintenanceRecords.length);
    }, 100);
  });

  it('should display loading state while fetching machine history', () => {
    fixture.componentRef.setInput('selectedJob', mockJob);
    fixture.componentRef.setInput('isOpen', true);
    component.isLoadingHistory.set(true);
    fixture.detectChanges();

    const loadingContainer = fixture.nativeElement.querySelector('.loading-container');
    expect(loadingContainer).toBeTruthy();
    expect(loadingContainer.textContent).toContain('Loading machine details');
  });

  it('should handle error state when machine history fails to load', () => {
    fixture.componentRef.setInput('selectedJob', mockJob);
    fixture.componentRef.setInput('isOpen', true);
    component.machineHistory.set(null);
    component.isLoadingHistory.set(false);
    fixture.detectChanges();

    const errorMessage = fixture.nativeElement.querySelector('.error-message');
    expect(errorMessage).toBeTruthy();
    expect(errorMessage.textContent).toContain('Unable to load machine details');
  });
});