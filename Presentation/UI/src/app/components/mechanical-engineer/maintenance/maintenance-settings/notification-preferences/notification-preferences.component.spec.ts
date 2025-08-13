import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { of, throwError } from 'rxjs';

import { NotificationPreferencesComponent } from './notification-preferences.component';
import { MaintenanceService } from '../../services/maintenance.service';
import { MaintenanceErrorHandlerService } from '../../services/maintenance-error-handler.service';
import { NotificationPreferences } from '../../models/maintenance.models';

describe('NotificationPreferencesComponent', () => {
  let component: NotificationPreferencesComponent;
  let fixture: ComponentFixture<NotificationPreferencesComponent>;
  let mockMaintenanceService: jasmine.SpyObj<MaintenanceService>;
  let mockErrorHandler: jasmine.SpyObj<MaintenanceErrorHandlerService>;

  const mockNotificationPreferences: NotificationPreferences = {
    emailNotifications: true,
    inAppNotifications: true,
    alertWindowDays: 30,
    overdueNotifications: true
  };

  const mockExtendedPreferences = {
    ...mockNotificationPreferences,
    emailFrequency: 'immediate',
    quietHoursEnabled: false,
    quietHoursStart: '22:00',
    quietHoursEnd: '08:00',
    weekendNotifications: true,
    serviceDueEmail: true,
    serviceDueInApp: true,
    overdueEmail: true,
    overdueInApp: true,
    jobAssignedEmail: true,
    jobAssignedInApp: true,
    jobCompletedEmail: false,
    jobCompletedInApp: true,
    systemAlertsEmail: true,
    systemAlertsInApp: true,
    escalationEnabled: false,
    escalationDelayHours: 24,
    escalationRecipients: []
  };

  beforeEach(async () => {
    const maintenanceServiceSpy = jasmine.createSpyObj('MaintenanceService', [
      'getNotificationPreferences',
      'updateNotificationPreferences'
    ]);

    const errorHandlerSpy = jasmine.createSpyObj('MaintenanceErrorHandlerService', [
      'handleError'
    ]);

    await TestBed.configureTestingModule({
      imports: [
        NotificationPreferencesComponent,
        ReactiveFormsModule,
        NoopAnimationsModule,
        MatSnackBarModule
      ],
      providers: [
        { provide: MaintenanceService, useValue: maintenanceServiceSpy },
        { provide: MaintenanceErrorHandlerService, useValue: errorHandlerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(NotificationPreferencesComponent);
    component = fixture.componentInstance;
    mockMaintenanceService = TestBed.inject(MaintenanceService) as jasmine.SpyObj<MaintenanceService>;
    mockErrorHandler = TestBed.inject(MaintenanceErrorHandlerService) as jasmine.SpyObj<MaintenanceErrorHandlerService>;

    // Setup default mock returns
    mockMaintenanceService.getNotificationPreferences.and.returnValue(of(mockNotificationPreferences));
    mockMaintenanceService.updateNotificationPreferences.and.returnValue(of(mockNotificationPreferences));
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form on ngOnInit', () => {
    component.ngOnInit();
    
    expect(component.notificationForm).toBeDefined();
    expect(component.notificationForm.get('emailNotifications')).toBeDefined();
    expect(component.notificationForm.get('inAppNotifications')).toBeDefined();
    expect(component.notificationForm.get('alertWindowDays')).toBeDefined();
  });

  it('should load preferences on initialization', () => {
    component.ngOnInit();
    
    expect(mockMaintenanceService.getNotificationPreferences).toHaveBeenCalled();
  });

  it('should populate form with loaded preferences', () => {
    component.ngOnInit();
    fixture.detectChanges();
    
    expect(component.preferences()).toEqual(mockNotificationPreferences);
    expect(component.notificationForm.value.emailNotifications).toBe(true);
    expect(component.notificationForm.value.alertWindowDays).toBe(30);
  });

  it('should populate form with extended preferences', () => {
    mockMaintenanceService.getNotificationPreferences.and.returnValue(of(mockExtendedPreferences as any));
    
    component.ngOnInit();
    fixture.detectChanges();
    
    expect(component.notificationForm.value.emailFrequency).toBe('immediate');
    expect(component.notificationForm.value.quietHoursEnabled).toBe(false);
    expect(component.notificationForm.value.escalationEnabled).toBe(false);
  });

  it('should handle preferences loading error', () => {
    mockMaintenanceService.getNotificationPreferences.and.returnValue(
      throwError(() => new Error('Service error'))
    );
    
    component.ngOnInit();
    fixture.detectChanges();
    
    expect(mockErrorHandler.handleError).toHaveBeenCalled();
    // Should keep default form values
    expect(component.notificationForm.value.emailNotifications).toBe(true);
  });

  it('should save preferences successfully', async () => {
    component.ngOnInit();
    fixture.detectChanges();
    
    component.notificationForm.patchValue({
      emailNotifications: false,
      alertWindowDays: 45
    });
    
    await component.savePreferences();
    
    expect(mockMaintenanceService.updateNotificationPreferences).toHaveBeenCalled();
    expect(component.notificationForm.pristine).toBe(true);
  });

  it('should not save invalid preferences', async () => {
    component.ngOnInit();
    fixture.detectChanges();
    
    // Set invalid data
    component.notificationForm.patchValue({
      alertWindowDays: -1
    });
    
    await component.savePreferences();
    
    expect(mockMaintenanceService.updateNotificationPreferences).not.toHaveBeenCalled();
  });

  it('should handle save error', async () => {
    mockMaintenanceService.updateNotificationPreferences.and.returnValue(
      throwError(() => new Error('Save error'))
    );
    
    component.ngOnInit();
    fixture.detectChanges();
    
    await component.savePreferences();
    
    expect(mockErrorHandler.handleError).toHaveBeenCalled();
  });

  it('should reset preferences', () => {
    component.ngOnInit();
    fixture.detectChanges();
    
    // Modify form
    component.notificationForm.patchValue({
      emailNotifications: false
    });
    
    component.resetPreferences();
    
    expect(component.notificationForm.value.emailNotifications).toBe(true);
    expect(component.notificationForm.pristine).toBe(true);
  });

  it('should test notification', () => {
    spyOn(component['snackBar'], 'open');
    
    component.testNotification('Service Due');
    
    expect(component['snackBar'].open).toHaveBeenCalledWith(
      'Test Service Due notification sent!',
      'Close',
      jasmine.any(Object)
    );
  });

  it('should handle master toggle changes for email', () => {
    component.ngOnInit();
    fixture.detectChanges();
    
    component.onMasterToggleChange('emailNotifications', false);
    
    expect(component.notificationForm.value.serviceDueEmail).toBe(false);
    expect(component.notificationForm.value.overdueEmail).toBe(false);
    expect(component.notificationForm.value.jobAssignedEmail).toBe(false);
  });

  it('should handle master toggle changes for in-app', () => {
    component.ngOnInit();
    fixture.detectChanges();
    
    component.onMasterToggleChange('inAppNotifications', false);
    
    expect(component.notificationForm.value.serviceDueInApp).toBe(false);
    expect(component.notificationForm.value.overdueInApp).toBe(false);
    expect(component.notificationForm.value.jobAssignedInApp).toBe(false);
  });

  it('should add escalation recipient', () => {
    component.ngOnInit();
    fixture.detectChanges();
    
    const mockEvent = {
      value: 'test@example.com',
      chipInput: { clear: jasmine.createSpy('clear') }
    };
    
    component.addRecipient(mockEvent as any);
    
    expect(component.notificationForm.value.escalationRecipients).toContain('test@example.com');
    expect(mockEvent.chipInput.clear).toHaveBeenCalled();
  });

  it('should not add invalid email recipient', () => {
    component.ngOnInit();
    fixture.detectChanges();
    
    const mockEvent = {
      value: 'invalid-email',
      chipInput: { clear: jasmine.createSpy('clear') }
    };
    
    component.addRecipient(mockEvent as any);
    
    expect(component.notificationForm.value.escalationRecipients).not.toContain('invalid-email');
    expect(mockEvent.chipInput.clear).toHaveBeenCalled();
  });

  it('should not add duplicate recipient', () => {
    component.ngOnInit();
    fixture.detectChanges();
    
    // Add first recipient
    component.notificationForm.patchValue({
      escalationRecipients: ['test@example.com']
    });
    
    const mockEvent = {
      value: 'test@example.com',
      chipInput: { clear: jasmine.createSpy('clear') }
    };
    
    component.addRecipient(mockEvent as any);
    
    expect(component.notificationForm.value.escalationRecipients.length).toBe(1);
  });

  it('should remove escalation recipient', () => {
    component.ngOnInit();
    fixture.detectChanges();
    
    // Set initial recipients
    component.notificationForm.patchValue({
      escalationRecipients: ['test1@example.com', 'test2@example.com']
    });
    
    component.removeRecipient(0);
    
    expect(component.notificationForm.value.escalationRecipients).toEqual(['test2@example.com']);
  });

  it('should validate email correctly', () => {
    expect(component['isValidEmail']('test@example.com')).toBe(true);
    expect(component['isValidEmail']('invalid-email')).toBe(false);
    expect(component['isValidEmail']('test@')).toBe(false);
    expect(component['isValidEmail']('@example.com')).toBe(false);
  });

  it('should get field errors correctly', () => {
    component.ngOnInit();
    fixture.detectChanges();
    
    component.notificationForm.get('alertWindowDays')?.setValue(-1);
    component.notificationForm.get('alertWindowDays')?.markAsTouched();
    
    const error = component.getFieldError('alertWindowDays');
    expect(error).toBeTruthy();
  });

  it('should compute hasUnsavedChanges correctly', () => {
    component.ngOnInit();
    fixture.detectChanges();
    
    expect(component.hasUnsavedChanges()).toBe(false);
    
    component.notificationForm.markAsDirty();
    expect(component.hasUnsavedChanges()).toBe(true);
  });

  it('should compute isFormValid correctly', () => {
    component.ngOnInit();
    fixture.detectChanges();
    
    expect(component.isFormValid()).toBe(true);
    
    component.notificationForm.get('alertWindowDays')?.setValue(-1);
    expect(component.isFormValid()).toBe(false);
  });
});