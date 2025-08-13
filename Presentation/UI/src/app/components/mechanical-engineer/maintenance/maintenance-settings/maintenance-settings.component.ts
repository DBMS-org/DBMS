import { Component, OnInit, inject, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { MatTabsModule } from '@angular/material/tabs';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTooltipModule } from '@angular/material/tooltip';

import { MaintenanceService } from '../services/maintenance.service';
import { MaintenanceErrorHandlerService } from '../services/maintenance-error-handler.service';
import { MaintenanceValidators } from '../utils/maintenance-validators';
import { ServiceIntervalConfig, NotificationPreferences } from '../models/maintenance.models';
import { NotificationPreferencesComponent } from './notification-preferences/notification-preferences.component';

@Component({
  selector: 'app-maintenance-settings',
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
    MatDividerModule,
    MatTabsModule,
    MatSlideToggleModule,
    MatTooltipModule,
    NotificationPreferencesComponent
  ],
  templateUrl: './maintenance-settings.component.html',
  styleUrls: ['./maintenance-settings.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MaintenanceSettingsComponent implements OnInit {
  private maintenanceService = inject(MaintenanceService);
  private errorHandler = inject(MaintenanceErrorHandlerService);
  private fb = inject(FormBuilder);
  private snackBar = inject(MatSnackBar);

  // Signals for state management
  isLoading = signal(false);
  isSaving = signal(false);
  serviceIntervalConfigs = signal<ServiceIntervalConfig[]>([]);

  // Form groups
  serviceIntervalsForm!: FormGroup;

  // Computed values
  hasUnsavedChanges = computed(() =>
    this.serviceIntervalsForm?.dirty || false
  );

  // Available machine types (this would typically come from a service)
  readonly machineTypes = [
    'Excavator',
    'Bulldozer',
    'Loader',
    'Dump Truck',
    'Drill Rig',
    'Crusher',
    'Conveyor',
    'Generator'
  ];

  ngOnInit() {
    this.initializeForms();
    this.loadSettings();
  }

  private initializeForms() {
    this.serviceIntervalsForm = this.fb.group({
      intervals: this.fb.array([])
    });
  }

  private async loadSettings() {
    this.isLoading.set(true);

    try {
      // Load service interval configurations
      this.maintenanceService.getServiceIntervalConfigs().subscribe({
        next: (configs) => {
          this.serviceIntervalConfigs.set(configs);
          this.populateServiceIntervalsForm(configs);
        },
        error: (error) => {
          this.errorHandler.handleError(error);
          // Initialize with default machine types if loading fails
          this.initializeDefaultServiceIntervals();
        }
      });
    } finally {
      this.isLoading.set(false);
    }
  }

  private populateServiceIntervalsForm(configs: ServiceIntervalConfig[]) {
    const intervalsArray = this.serviceIntervalsForm.get('intervals') as FormArray;
    intervalsArray.clear();

    // Ensure we have configs for all machine types
    const configMap = new Map(configs.map(config => [config.machineType, config]));

    this.machineTypes.forEach(machineType => {
      const existingConfig = configMap.get(machineType);
      const intervalGroup = this.createServiceIntervalGroup(existingConfig || {
        machineType,
        intervalHours: undefined,
        intervalMonths: 6,
        alertWindowDays: 30
      });
      intervalsArray.push(intervalGroup);
    });
  }

  private initializeDefaultServiceIntervals() {
    const intervalsArray = this.serviceIntervalsForm.get('intervals') as FormArray;
    intervalsArray.clear();

    this.machineTypes.forEach(machineType => {
      const intervalGroup = this.createServiceIntervalGroup({
        machineType,
        intervalHours: undefined,
        intervalMonths: 6,
        alertWindowDays: 30
      });
      intervalsArray.push(intervalGroup);
    });
  }

  private createServiceIntervalGroup(config: ServiceIntervalConfig): FormGroup {
    return this.fb.group({
      machineType: [config.machineType, Validators.required],
      intervalHours: [config.intervalHours, [Validators.min(1)]],
      intervalMonths: [config.intervalMonths, [Validators.min(1)]],
      alertWindowDays: [config.alertWindowDays, [Validators.required, Validators.min(1), Validators.max(365)]]
    }, { validators: MaintenanceValidators.serviceIntervalValidator() });
  }

  get serviceIntervalsArray(): FormArray {
    return this.serviceIntervalsForm.get('intervals') as FormArray;
  }

  addServiceInterval() {
    const intervalGroup = this.createServiceIntervalGroup({
      machineType: '',
      intervalHours: undefined,
      intervalMonths: 6,
      alertWindowDays: 30
    });
    this.serviceIntervalsArray.push(intervalGroup);
  }

  removeServiceInterval(index: number) {
    this.serviceIntervalsArray.removeAt(index);
  }

  async saveServiceIntervals() {
    if (this.serviceIntervalsForm.invalid) {
      this.markFormGroupTouched(this.serviceIntervalsForm);
      this.snackBar.open('Please fix validation errors before saving', 'Close', {
        duration: 3000,
        panelClass: ['error-snackbar']
      });
      return;
    }

    this.isSaving.set(true);

    try {
      const intervals = this.serviceIntervalsForm.value.intervals as ServiceIntervalConfig[];

      // Save each interval configuration
      for (const interval of intervals) {
        if (interval.machineType) {
          await firstValueFrom(this.maintenanceService.updateServiceIntervalConfig(interval));
        }
      }

      this.serviceIntervalsForm.markAsPristine();
      this.snackBar.open('Service intervals saved successfully', 'Close', {
        duration: 3000,
        panelClass: ['success-snackbar']
      });
    } catch (error) {
      this.errorHandler.handleError(error as Error);
    } finally {
      this.isSaving.set(false);
    }
  }

  resetServiceIntervals() {
    this.loadSettings();
    this.serviceIntervalsForm.markAsPristine();
  }

  getFieldError(formGroup: FormGroup, fieldName: string): string {
    const field = formGroup.get(fieldName);
    if (field && field.errors && field.touched) {
      return MaintenanceValidators.getErrorMessage(field.errors);
    }
    return '';
  }

  getIntervalError(index: number, fieldName: string): string {
    const intervalGroup = this.serviceIntervalsArray.at(index) as FormGroup;
    return this.getFieldError(intervalGroup, fieldName);
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      } else if (control instanceof FormArray) {
        control.controls.forEach(arrayControl => {
          if (arrayControl instanceof FormGroup) {
            this.markFormGroupTouched(arrayControl);
          } else {
            arrayControl.markAsTouched();
          }
        });
      } else {
        control?.markAsTouched();
      }
    });
  }
}