import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { of } from 'rxjs';

import { JobStatusUpdateComponent, JobStatusUpdateData } from './job-status-update.component';
import { MaintenanceService } from '../../services/maintenance.service';
import { MaintenanceJob, MaintenanceStatus, MaintenanceType } from '../../models/maintenance.models';

describe('JobStatusUpdateComponent', () => {
  let component: JobStatusUpdateComponent;
  let fixture: ComponentFixture<JobStatusUpdateComponent>;
  let mockMaintenanceService: jasmine.SpyObj<MaintenanceService>;
  let mockDialogRef: jasmine.SpyObj<MatDialogRef<JobStatusUpdateComponent>>;
  let mockSnackBar: jasmine.SpyObj<MatSnackBar>;

  const mockJob: MaintenanceJob = {
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
  };

  const mockDialogData: JobStatusUpdateData = {
    job: mockJob
  };

  beforeEach(async () => {
    const maintenanceServiceSpy = jasmine.createSpyObj('MaintenanceService', [
      'updateMaintenanceJob',
      'uploadMaintenanceFile'
    ]);
    const dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);
    const snackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);

    await TestBed.configureTestingModule({
      imports: [JobStatusUpdateComponent, NoopAnimationsModule],
      providers: [
        { provide: MaintenanceService, useValue: maintenanceServiceSpy },
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MatSnackBar, useValue: snackBarSpy },
        { provide: MAT_DIALOG_DATA, useValue: mockDialogData }
      ]
    }).compileComponents();

    mockMaintenanceService = TestBed.inject(MaintenanceService) as jasmine.SpyObj<MaintenanceService>;
    mockDialogRef = TestBed.inject(MatDialogRef) as jasmine.SpyObj<MatDialogRef<JobStatusUpdateComponent>>;
    mockSnackBar = TestBed.inject(MatSnackBar) as jasmine.SpyObj<MatSnackBar>;

    mockMaintenanceService.updateMaintenanceJob.and.returnValue(of(mockJob));
    mockMaintenanceService.uploadMaintenanceFile.and.returnValue(of({}));

    fixture = TestBed.createComponent(JobStatusUpdateComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with job data', () => {
    fixture.detectChanges();
    expect(component.job()).toEqual(mockJob);
    expect(component.statusForm.get('status')?.value).toBe(MaintenanceStatus.SCHEDULED);
  });

  it('should display job information', () => {
    fixture.detectChanges();
    
    const jobInfo = fixture.nativeElement.querySelector('.job-info');
    expect(jobInfo.textContent).toContain('Excavator 1');
    expect(jobInfo.textContent).toContain('Project A');
    expect(jobInfo.textContent).toContain('Regular maintenance');
  });

  it('should show maintenance report section for in-progress and completed status', () => {
    fixture.detectChanges();
    
    // Initially should not show maintenance report
    expect(component.showMaintenanceReport()).toBe(false);
    
    // Change to in-progress
    component.statusForm.patchValue({ status: MaintenanceStatus.IN_PROGRESS });
    expect(component.showMaintenanceReport()).toBe(true);
    
    // Change to completed
    component.statusForm.patchValue({ status: MaintenanceStatus.COMPLETED });
    expect(component.showMaintenanceReport()).toBe(true);
  });

  it('should require actual hours for completed status', () => {
    fixture.detectChanges();
    
    component.statusForm.patchValue({ status: MaintenanceStatus.COMPLETED });
    component['updateValidators']();
    
    const actualHoursControl = component.statusForm.get('actualHours');
    expect(actualHoursControl?.hasError('required')).toBe(true);
    
    actualHoursControl?.setValue(4);
    expect(actualHoursControl?.hasError('required')).toBe(false);
  });

  it('should validate actual hours range', () => {
    fixture.detectChanges();
    
    component.statusForm.patchValue({ status: MaintenanceStatus.COMPLETED });
    component['updateValidators']();
    
    const actualHoursControl = component.statusForm.get('actualHours');
    
    // Test minimum value
    actualHoursControl?.setValue(0.1);
    expect(actualHoursControl?.hasError('min')).toBe(true);
    
    // Test maximum value
    actualHoursControl?.setValue(25);
    expect(actualHoursControl?.hasError('max')).toBe(true);
    
    // Test valid value
    actualHoursControl?.setValue(4);
    expect(actualHoursControl?.valid).toBe(true);
  });

  it('should add and remove parts correctly', () => {
    fixture.detectChanges();
    
    const mockEvent = {
      value: 'Oil Filter',
      chipInput: { clear: jasmine.createSpy('clear') }
    };
    
    component.addPart(mockEvent);
    expect(component.partsReplaced()).toContain('Oil Filter');
    expect(mockEvent.chipInput.clear).toHaveBeenCalled();
    
    component.removePart('Oil Filter');
    expect(component.partsReplaced()).not.toContain('Oil Filter');
  });

  it('should handle file selection correctly', () => {
    fixture.detectChanges();
    
    const mockFile = new File(['test'], 'test.pdf', { type: 'application/pdf' });
    const mockEvent = {
      target: { files: [mockFile], value: '' }
    } as any;
    
    component.onFileSelected(mockEvent);
    expect(component.selectedFiles()).toContain(mockFile);
  });

  it('should validate file types and sizes', () => {
    fixture.detectChanges();
    
    // Test invalid file type
    const invalidFile = new File(['test'], 'test.exe', { type: 'application/exe' });
    component['processFiles']([invalidFile]);
    expect(component.fileErrors()).toContain('test.exe: Unsupported file type');
    
    // Test file size limit
    const largeFile = new File(['x'.repeat(11 * 1024 * 1024)], 'large.pdf', { type: 'application/pdf' });
    component['processFiles']([largeFile]);
    expect(component.fileErrors()).toContain('large.pdf: File size exceeds 10MB limit');
  });

  it('should handle drag and drop events', () => {
    fixture.detectChanges();
    
    const mockDragEvent = {
      preventDefault: jasmine.createSpy('preventDefault'),
      dataTransfer: {
        files: [new File(['test'], 'test.pdf', { type: 'application/pdf' })]
      }
    } as any;
    
    component.onDragOver(mockDragEvent);
    expect(mockDragEvent.preventDefault).toHaveBeenCalled();
    expect(component.isDragOver()).toBe(true);
    
    component.onDragLeave(mockDragEvent);
    expect(component.isDragOver()).toBe(false);
    
    component.onDrop(mockDragEvent);
    expect(component.selectedFiles().length).toBe(1);
  });

  it('should remove selected files', () => {
    fixture.detectChanges();
    
    const mockFile = new File(['test'], 'test.pdf', { type: 'application/pdf' });
    component.selectedFiles.set([mockFile]);
    
    component.removeFile(mockFile);
    expect(component.selectedFiles()).not.toContain(mockFile);
  });

  it('should submit form successfully', async () => {
    fixture.detectChanges();
    
    component.statusForm.patchValue({
      status: MaintenanceStatus.IN_PROGRESS,
      observations: 'Work in progress',
      notifyMachineManager: true
    });
    
    await component.onSubmit();
    
    expect(mockMaintenanceService.updateMaintenanceJob).toHaveBeenCalledWith(
      mockJob.id,
      jasmine.objectContaining({
        status: MaintenanceStatus.IN_PROGRESS,
        observations: 'Work in progress'
      })
    );
    
    expect(mockSnackBar.open).toHaveBeenCalledWith(
      'Job status updated successfully',
      'Close',
      jasmine.any(Object)
    );
    
    expect(mockDialogRef.close).toHaveBeenCalled();
  });

  it('should handle submit errors', async () => {
    fixture.detectChanges();
    
    mockMaintenanceService.updateMaintenanceJob.and.returnValue(
      new Promise((_, reject) => reject(new Error('Update failed')))
    );
    
    component.statusForm.patchValue({
      status: MaintenanceStatus.IN_PROGRESS
    });
    
    await component.onSubmit();
    
    expect(mockSnackBar.open).toHaveBeenCalledWith(
      'Failed to update job status',
      'Close',
      jasmine.any(Object)
    );
  });

  it('should cancel dialog', () => {
    fixture.detectChanges();
    
    component.onCancel();
    expect(mockDialogRef.close).toHaveBeenCalled();
  });

  it('should return correct status display names and icons', () => {
    expect(component.getStatusDisplayName(MaintenanceStatus.SCHEDULED)).toBe('Scheduled');
    expect(component.getStatusDisplayName(MaintenanceStatus.IN_PROGRESS)).toBe('In Progress');
    
    expect(component.getStatusIcon(MaintenanceStatus.SCHEDULED)).toBe('schedule');
    expect(component.getStatusIcon(MaintenanceStatus.IN_PROGRESS)).toBe('play_circle');
  });

  it('should return correct file icons', () => {
    expect(component.getFileIcon('image/jpeg')).toBe('image');
    expect(component.getFileIcon('application/pdf')).toBe('picture_as_pdf');
    expect(component.getFileIcon('text/plain')).toBe('description');
    expect(component.getFileIcon('application/msword')).toBe('description');
    expect(component.getFileIcon('unknown/type')).toBe('attach_file');
  });

  it('should format file size correctly', () => {
    expect(component.formatFileSize(0)).toBe('0 Bytes');
    expect(component.formatFileSize(1024)).toBe('1 KB');
    expect(component.formatFileSize(1048576)).toBe('1 MB');
  });

  it('should generate correct CSS classes', () => {
    expect(component.getStatusChipClass(MaintenanceStatus.IN_PROGRESS)).toBe('status-in-progress');
    expect(component.getStatusIconClass(MaintenanceStatus.OVERDUE)).toBe('icon-overdue');
  });

  it('should get current status information correctly', () => {
    fixture.detectChanges();
    
    expect(component.getCurrentStatusName()).toBe('Scheduled');
    expect(component.getCurrentStatusIcon()).toBe('schedule');
    expect(component.getCurrentStatusClass()).toBe('status-scheduled');
  });
});