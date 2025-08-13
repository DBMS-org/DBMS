import { Component, OnInit, inject, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';
import { MatChipsModule, MatChipInputEvent } from '@angular/material/chips';

import { MaintenanceService } from '../../services/maintenance.service';
import { MaintenanceErrorHandlerService } from '../../services/maintenance-error-handler.service';
import { MaintenanceValidators } from '../../utils/maintenance-validators';
import { NotificationPreferences } from '../../models/maintenance.models';

@Component({
  selector: 'app-notification-preferences',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatSlideToggleModule,
    MatTooltipModule,
    MatDividerModule,
    MatChipsModule
  ],
  templateUrl: './notification-preferences.component.html',
  styleUrls: ['./notification-preferences.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NotificationPreferencesComponent implements OnInit {
  private maintenanceService = inject(MaintenanceService);
  private errorHandler = inject(MaintenanceErrorHandlerService);
  private fb = inject(FormBuilder);
  private snackBar = inject(MatSnackBar);

  // Signals for state management
  isLoading = signal(false);
  isSaving = signal(false);
  preferences = signal<NotificationPreferences | null>(null);

  // Form group
  notificationForm!: FormGroup;

  // Computed values
  hasUnsavedChanges = computed(() => this.notificationForm?.dirty || false);
  isFormValid = computed(() => this.notificationForm?.valid || false);

  // Notification timing options
  readonly alertWindowOptions = [
    { value: 7, label: '7 days before due' },
    { value: 14, label: '14 days before due' },
    { value: 21, label: '21 days before due' },
    { value: 30, label: '30 days before due' },
    { value: 45, label: '45 days before due' },
    { value: 60, label: '60 days before due' },
    { value: 90, label: '90 days before due' }
  ];

  // Notification frequency options
  readonly notificationFrequencies = [
    { value: 'immediate', label: 'Immediate' },
    { value: 'daily', label: 'Daily digest' },
    { value: 'weekly', label: 'Weekly summary' }
  ];

  // Notification types
  readonly notificationTypes = [
    {
      key: 'serviceDue',
      label: 'Service Due Alerts',
      description: 'Notifications when maintenance is approaching due date',
      icon: 'schedule'
    },
    {
      key: 'overdue',
      label: 'Overdue Alerts',
      description: 'Immediate alerts for overdue maintenance',
      icon: 'warning'
    },
    {
      key: 'jobAssigned',
      label: 'Job Assignments',
      description: 'Notifications when maintenance jobs are assigned to you',
      icon: 'assignment'
    },
    {
      key: 'jobCompleted',
      label: 'Job Completions',
      description: 'Notifications when maintenance jobs are completed',
      icon: 'check_circle'
    },
    {
      key: 'systemAlerts',
      label: 'System Alerts',
      description: 'Important system notifications and updates',
      icon: 'info'
    }
  ];

  ngOnInit() {
    this.initializeForm();
    this.loadPreferences();
  }

  private initializeForm() {
    this.notificationForm = this.fb.group({
      // General notification settings
      emailNotifications: [true],
      inAppNotifications: [true],
      alertWindowDays: [30, [Validators.required, Validators.min(1), Validators.max(365)]],
      overdueNotifications: [true],

      // Advanced notification settings
      emailFrequency: ['immediate'],
      quietHoursEnabled: [false],
      quietHoursStart: ['22:00'],
      quietHoursEnd: ['08:00'],
      weekendNotifications: [true],

      // Notification type preferences
      serviceDueEmail: [true],
      serviceDueInApp: [true],
      overdueEmail: [true],
      overdueInApp: [true],
      jobAssignedEmail: [true],
      jobAssignedInApp: [true],
      jobCompletedEmail: [false],
      jobCompletedInApp: [true],
      systemAlertsEmail: [true],
      systemAlertsInApp: [true],

      // Escalation settings
      escalationEnabled: [false],
      escalationDelayHours: [24, [Validators.min(1), Validators.max(168)]],
      escalationRecipients: [[]]
    });
  }

  private async loadPreferences() {
    this.isLoading.set(true);

    try {
      this.maintenanceService.getNotificationPreferences().subscribe({
        next: (preferences) => {
          this.preferences.set(preferences);
          this.populateForm(preferences);
        },
        error: (error) => {
          this.errorHandler.handleError(error as Error);
          // Keep default form values if loading fails
        }
      });
    } finally {
      this.isLoading.set(false);
    }
  }

  private populateForm(preferences: NotificationPreferences) {
    // Map the basic preferences to form
    this.notificationForm.patchValue({
      emailNotifications: preferences.emailNotifications,
      inAppNotifications: preferences.inAppNotifications,
      alertWindowDays: preferences.alertWindowDays,
      overdueNotifications: preferences.overdueNotifications
    });

    // If preferences have extended properties, map them too
    const extendedPrefs = preferences as any;
    if (extendedPrefs.emailFrequency) {
      this.notificationForm.patchValue({
        emailFrequency: extendedPrefs.emailFrequency,
        quietHoursEnabled: extendedPrefs.quietHoursEnabled || false,
        quietHoursStart: extendedPrefs.quietHoursStart || '22:00',
        quietHoursEnd: extendedPrefs.quietHoursEnd || '08:00',
        weekendNotifications: extendedPrefs.weekendNotifications !== false,

        serviceDueEmail: extendedPrefs.serviceDueEmail !== false,
        serviceDueInApp: extendedPrefs.serviceDueInApp !== false,
        overdueEmail: extendedPrefs.overdueEmail !== false,
        overdueInApp: extendedPrefs.overdueInApp !== false,
        jobAssignedEmail: extendedPrefs.jobAssignedEmail !== false,
        jobAssignedInApp: extendedPrefs.jobAssignedInApp !== false,
        jobCompletedEmail: extendedPrefs.jobCompletedEmail || false,
        jobCompletedInApp: extendedPrefs.jobCompletedInApp !== false,
        systemAlertsEmail: extendedPrefs.systemAlertsEmail !== false,
        systemAlertsInApp: extendedPrefs.systemAlertsInApp !== false,

        escalationEnabled: extendedPrefs.escalationEnabled || false,
        escalationDelayHours: extendedPrefs.escalationDelayHours || 24,
        escalationRecipients: extendedPrefs.escalationRecipients || []
      });
    }
  }

  async savePreferences() {
    if (this.notificationForm.invalid) {
      this.markFormGroupTouched(this.notificationForm);
      this.snackBar.open('Please fix validation errors before saving', 'Close', {
        duration: 3000,
        panelClass: ['error-snackbar']
      });
      return;
    }

    this.isSaving.set(true);

    try {
      const formValue = this.notificationForm.value;

      // Create the preferences object with all form values
      const preferences: NotificationPreferences & any = {
        emailNotifications: formValue.emailNotifications,
        inAppNotifications: formValue.inAppNotifications,
        alertWindowDays: formValue.alertWindowDays,
        overdueNotifications: formValue.overdueNotifications,

        // Extended properties
        emailFrequency: formValue.emailFrequency,
        quietHoursEnabled: formValue.quietHoursEnabled,
        quietHoursStart: formValue.quietHoursStart,
        quietHoursEnd: formValue.quietHoursEnd,
        weekendNotifications: formValue.weekendNotifications,

        serviceDueEmail: formValue.serviceDueEmail,
        serviceDueInApp: formValue.serviceDueInApp,
        overdueEmail: formValue.overdueEmail,
        overdueInApp: formValue.overdueInApp,
        jobAssignedEmail: formValue.jobAssignedEmail,
        jobAssignedInApp: formValue.jobAssignedInApp,
        jobCompletedEmail: formValue.jobCompletedEmail,
        jobCompletedInApp: formValue.jobCompletedInApp,
        systemAlertsEmail: formValue.systemAlertsEmail,
        systemAlertsInApp: formValue.systemAlertsInApp,

        escalationEnabled: formValue.escalationEnabled,
        escalationDelayHours: formValue.escalationDelayHours,
        escalationRecipients: formValue.escalationRecipients
      };

      await firstValueFrom(this.maintenanceService.updateNotificationPreferences(preferences));

      this.preferences.set(preferences);
      this.notificationForm.markAsPristine();
      this.snackBar.open('Notification preferences saved successfully', 'Close', {
        duration: 3000,
        panelClass: ['success-snackbar']
      });
    } catch (error) {
      this.errorHandler.handleError(error as Error);
    } finally {
      this.isSaving.set(false);
    }
  }

  resetPreferences() {
    if (this.preferences()) {
      this.populateForm(this.preferences()!);
      this.notificationForm.markAsPristine();
    } else {
      this.loadPreferences();
    }
  }

  testNotification(type: string) {
    this.snackBar.open(`Test ${type} notification sent!`, 'Close', {
      duration: 2000,
      panelClass: ['info-snackbar']
    });
  }

  onMasterToggleChange(field: 'emailNotifications' | 'inAppNotifications', enabled: boolean) {
    if (!enabled) {
      // If disabling master toggle, disable all related notifications
      if (field === 'emailNotifications') {
        this.notificationForm.patchValue({
          serviceDueEmail: false,
          overdueEmail: false,
          jobAssignedEmail: false,
          jobCompletedEmail: false,
          systemAlertsEmail: false
        });
      } else if (field === 'inAppNotifications') {
        this.notificationForm.patchValue({
          serviceDueInApp: false,
          overdueInApp: false,
          jobAssignedInApp: false,
          jobCompletedInApp: false,
          systemAlertsInApp: false
        });
      }
    }
  }

  getFieldError(fieldName: string): string {
    const field = this.notificationForm.get(fieldName);
    if (field && field.errors && field.touched) {
      return MaintenanceValidators.getErrorMessage(field.errors);
    }
    return '';
  }

  addRecipient(event: MatChipInputEvent) {
    const value = (event.value || '').trim();
    if (value && this.isValidEmail(value)) {
      const currentRecipients = this.notificationForm.get('escalationRecipients')?.value || [];
      if (!currentRecipients.includes(value)) {
        this.notificationForm.patchValue({
          escalationRecipients: [...currentRecipients, value]
        });
      }
    }
    event.chipInput!.clear();
  }

  removeRecipient(index: number) {
    const currentRecipients = this.notificationForm.get('escalationRecipients')?.value || [];
    currentRecipients.splice(index, 1);
    this.notificationForm.patchValue({
      escalationRecipients: [...currentRecipients]
    });
  }

  private isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      } else {
        control?.markAsTouched();
      }
    });
  }
}