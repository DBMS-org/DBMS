import { Component, OnInit, inject, signal, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { ChangeDetectionStrategy } from '@angular/core';

import { MaintenanceJob, MaintenanceStatus, FileAttachment } from '../../models/maintenance.models';
import { MaintenanceService } from '../../services/maintenance.service';
import { MaintenanceValidators } from '../../utils/maintenance-validators';

export interface JobStatusUpdateData {
  job: MaintenanceJob;
}

@Component({
  selector: 'app-job-status-update',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="status-update-dialog">
      <h2 mat-dialog-title>
        <mat-icon>update</mat-icon>
        Update Job Status
      </h2>

      <mat-dialog-content>
        <div class="job-info">
          <h3>{{ job().machineName }}</h3>
          <p>{{ job().project }} - {{ job().reason }}</p>
          <mat-chip [class]="getCurrentStatusClass()">
            <mat-icon>{{ getCurrentStatusIcon() }}</mat-icon>
            Current: {{ getCurrentStatusName() }}
          </mat-chip>
        </div>

        <form [formGroup]="statusForm" class="status-form">
          <!-- Status Selection -->
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>New Status</mat-label>
            <mat-select formControlName="status" (selectionChange)="onStatusChange($event.value)">
              @for (status of availableStatuses; track status.value) {
                <mat-option [value]="status.value" [disabled]="status.value === job().status">
                  <div class="status-option">
                    <mat-icon [class]="getStatusIconClass(status.value)">{{ getStatusIcon(status.value) }}</mat-icon>
                    <span>{{ status.label }}</span>
                  </div>
                </mat-option>
              }
            </mat-select>
            @if (statusForm.get('status')?.hasError('required')) {
              <mat-error>Status is required</mat-error>
            }
          </mat-form-field>

          <!-- Maintenance Report Section (shown for Completed only) -->
          @if (showMaintenanceReport()) {
            <div class="maintenance-report-section">
              <h4>
                <mat-icon>assignment</mat-icon>
                Maintenance Report
              </h4>

              <!-- Observations -->
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Observations</mat-label>
                <textarea matInput
                         formControlName="observations"
                         rows="4"
                         placeholder="Describe the work performed, findings, or issues encountered..."></textarea>
                @if (statusForm.get('observations')?.hasError('required')) {
                  <mat-error>Observations are required for completed jobs</mat-error>
                }
                @if (statusForm.get('observations')?.hasError('minlength')) {
                  <mat-error>Observations must be at least 20 characters</mat-error>
                }
                @if (statusForm.get('observations')?.hasError('maxlength')) {
                  <mat-error>Observations cannot exceed 1000 characters</mat-error>
                }
              </mat-form-field>

              <!-- Parts Replaced -->
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Parts Replaced</mat-label>
                <mat-chip-grid #chipGrid>
                  @for (part of partsReplaced(); track part) {
                    <mat-chip-row (removed)="removePart(part)">
                      {{ part }}
                      <mat-icon matChipRemove>cancel</mat-icon>
                    </mat-chip-row>
                  }
                  <input placeholder="Add part and press Enter..."
                         [matChipInputFor]="chipGrid"
                         (matChipInputTokenEnd)="addPart($event)">
                </mat-chip-grid>
              </mat-form-field>

              <!-- Actual Hours (required for Completed status) -->
              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Actual Hours</mat-label>
                <input matInput
                       type="number"
                       formControlName="actualHours"
                       step="0.5"
                       min="0"
                       max="24"
                       placeholder="Enter actual hours spent">
                <mat-hint>Estimated: {{ job().estimatedHours }}h</mat-hint>
                @if (statusForm.get('actualHours')?.hasError('required')) {
                  <mat-error>Actual hours are required for completed jobs</mat-error>
                }
                @if (statusForm.get('actualHours')?.hasError('min')) {
                  <mat-error>Hours must be greater than 0</mat-error>
                }
                @if (statusForm.get('actualHours')?.hasError('max')) {
                  <mat-error>Hours cannot exceed 24</mat-error>
                }
              </mat-form-field>

              <!-- Service Completion Checkboxes -->
              <div class="service-completion-section">
                <h5>
                  <mat-icon>build_circle</mat-icon>
                  Service Completion
                </h5>
                <p class="section-hint">Check if this job completes a scheduled service - hours will be reset automatically</p>

                <mat-checkbox formControlName="isEngineServiceCompleted" class="service-completion-checkbox">
                  <strong>Engine Service Completed</strong>
                  <span class="checkbox-hint">Resets engine service hours to 0</span>
                </mat-checkbox>

                <mat-checkbox formControlName="isDrifterServiceCompleted" class="service-completion-checkbox">
                  <strong>Drifter Service Completed</strong>
                  <span class="checkbox-hint">Resets drifter service hours to 0 (for drill rigs only)</span>
                </mat-checkbox>

                <mat-checkbox formControlName="isServiceCompleted" class="service-completion-checkbox materials-checkbox">
                  <strong>Log Consumed Drilling Components</strong>
                  <span class="checkbox-hint">Check to log drill bits, rods, and shanks used</span>
                </mat-checkbox>
              </div>

              <!-- Consumed Drilling Components (shown only if service completed) -->
              @if (statusForm.get('isServiceCompleted')?.value) {
                <div class="drilling-components-section">
                  <h5>
                    <mat-icon>build_circle</mat-icon>
                    Consumed Drilling Components
                  </h5>
                  <p class="section-hint">Enter the drilling components used during this service</p>

                  <!-- Drill Bits -->
                  <div class="component-row">
                    <mat-form-field appearance="outline" class="component-quantity">
                      <mat-label>Drill Bits Used</mat-label>
                      <input matInput
                             type="number"
                             formControlName="drillBitsUsed"
                             min="0"
                             placeholder="0">
                      @if (statusForm.get('drillBitsUsed')?.hasError('min')) {
                        <mat-error>Cannot be negative</mat-error>
                      }
                    </mat-form-field>
                    <mat-form-field appearance="outline" class="component-type">
                      <mat-label>Drill Bit Type</mat-label>
                      <input matInput
                             formControlName="drillBitType"
                             placeholder="e.g., Tungsten Carbide 45mm">
                      <mat-hint>Optional: Specify drill bit type/size</mat-hint>
                    </mat-form-field>
                  </div>

                  <!-- Drill Rods -->
                  <div class="component-row">
                    <mat-form-field appearance="outline" class="component-quantity">
                      <mat-label>Drill Rods Used</mat-label>
                      <input matInput
                             type="number"
                             formControlName="drillRodsUsed"
                             min="0"
                             placeholder="0">
                      @if (statusForm.get('drillRodsUsed')?.hasError('min')) {
                        <mat-error>Cannot be negative</mat-error>
                      }
                    </mat-form-field>
                    <mat-form-field appearance="outline" class="component-type">
                      <mat-label>Drill Rod Type</mat-label>
                      <input matInput
                             formControlName="drillRodType"
                             placeholder="e.g., Steel 3.6m">
                      <mat-hint>Optional: Specify drill rod type/length</mat-hint>
                    </mat-form-field>
                  </div>

                  <!-- Shanks -->
                  <div class="component-row">
                    <mat-form-field appearance="outline" class="component-quantity">
                      <mat-label>Shanks Used</mat-label>
                      <input matInput
                             type="number"
                             formControlName="shanksUsed"
                             min="0"
                             placeholder="0">
                      @if (statusForm.get('shanksUsed')?.hasError('min')) {
                        <mat-error>Cannot be negative</mat-error>
                      }
                    </mat-form-field>
                    <mat-form-field appearance="outline" class="component-type">
                      <mat-label>Shank Type</mat-label>
                      <input matInput
                             formControlName="shankType"
                             placeholder="e.g., R32 Thread">
                      <mat-hint>Optional: Specify shank type/threading</mat-hint>
                    </mat-form-field>
                  </div>
                </div>
              }
            </div>
          }
        </form>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button 
                type="button" 
                (click)="onCancel()"
                [disabled]="isSubmitting()">
          Cancel
        </button>
        
        <button mat-raised-button 
                color="primary" 
                type="button"
                (click)="onSubmit()"
                [disabled]="!statusForm.valid || isSubmitting()">
          @if (isSubmitting()) {
            <mat-spinner diameter="20"></mat-spinner>
          } @else {
            <mat-icon>save</mat-icon>
          }
          Update Status
        </button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .status-update-dialog {
      width: 600px;
      max-width: 90vw;
    }

    .status-update-dialog h2 {
      display: flex;
      align-items: center;
      gap: 8px;
      margin: 0;
    }

    .job-info {
      margin-bottom: 24px;
      padding: 16px;
      background-color: #f8f9fa;
      border-radius: 4px;
    }

    .job-info h3 {
      margin: 0 0 8px 0;
      font-size: 18px;
    }

    .job-info p {
      margin: 0 0 12px 0;
      color: #666;
    }

    .status-form {
      display: flex;
      flex-direction: column;
      gap: 16px;
    }

    .full-width {
      width: 100%;
    }

    .status-option {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .maintenance-report-section {
      border: 1px solid #e0e0e0;
      border-radius: 4px;
      padding: 16px;
    }

    .maintenance-report-section h4 {
      display: flex;
      align-items: center;
      gap: 8px;
      margin: 0 0 16px 0;
      font-size: 16px;
      font-weight: 500;
    }

    .service-completion-checkbox {
      display: block;
      margin: 16px 0;
      padding: 12px;
      background-color: #f8f9fa;
      border-radius: 4px;
    }

    .checkbox-hint {
      display: block;
      font-size: 12px;
      color: #666;
      margin-top: 4px;
      font-weight: normal;
    }

    .service-completion-section {
      margin-top: 16px;
      padding: 16px;
      background: linear-gradient(135deg, #e8f5e9 0%, #f1f8e9 100%);
      border-left: 4px solid #4caf50;
      border-radius: 4px;
    }

    .service-completion-section h5 {
      display: flex;
      align-items: center;
      gap: 8px;
      margin: 0 0 8px 0;
      font-size: 15px;
      font-weight: 600;
      color: #2e7d32;
    }

    .drilling-components-section {
      margin-top: 16px;
      padding: 16px;
      background: linear-gradient(135deg, #f8f5ff 0%, #fef9ff 100%);
      border-left: 4px solid #667eea;
      border-radius: 4px;
    }

    .drilling-components-section h5 {
      display: flex;
      align-items: center;
      gap: 8px;
      margin: 0 0 8px 0;
      font-size: 15px;
      font-weight: 600;
      color: #667eea;
    }

    .section-hint {
      margin: 0 0 16px 0;
      font-size: 13px;
      color: #666;
      font-style: italic;
    }

    .component-row {
      display: grid;
      grid-template-columns: 120px 1fr;
      gap: 12px;
      margin-bottom: 12px;
    }

    .component-quantity {
      width: 100%;
    }

    .component-type {
      width: 100%;
    }

    /* Status Chips */
    .status-scheduled {
      background-color: #e3f2fd;
      color: #1976d2;
    }

    .status-in-progress {
      background-color: #fff3e0;
      color: #f57c00;
    }

    .status-completed {
      background-color: #e8f5e8;
      color: #388e3c;
    }

    .status-cancelled {
      background-color: #fce4ec;
      color: #c2185b;
    }

    .status-overdue {
      background-color: #ffebee;
      color: #d32f2f;
    }

    /* Status Icons */
    .icon-scheduled {
      color: #1976d2;
    }

    .icon-in-progress {
      color: #f57c00;
    }

    .icon-completed {
      color: #388e3c;
    }

    .icon-cancelled {
      color: #c2185b;
    }

    .icon-overdue {
      color: #d32f2f;
    }
  `]
})
export class JobStatusUpdateComponent implements OnInit {
  private fb = inject(FormBuilder);
  private maintenanceService = inject(MaintenanceService);
  private snackBar = inject(MatSnackBar);
  private dialogRef = inject(MatDialogRef<JobStatusUpdateComponent>);
  private data = inject<JobStatusUpdateData>(MAT_DIALOG_DATA);

  // Signals
  job = signal<MaintenanceJob>(this.data.job);
  isSubmitting = signal(false);
  partsReplaced = signal<string[]>(this.data.job.partsReplaced || []);

  // Form
  statusForm: FormGroup;

  // Configuration
  availableStatuses = [
    { value: MaintenanceStatus.SCHEDULED, label: 'Scheduled' },
    { value: MaintenanceStatus.IN_PROGRESS, label: 'In Progress' },
    { value: MaintenanceStatus.COMPLETED, label: 'Completed' },
    { value: MaintenanceStatus.CANCELLED, label: 'Cancelled' }
  ];

  constructor() {
    this.statusForm = this.fb.group({
      status: [this.job().status, [Validators.required]],
      observations: [this.job().observations || '', [Validators.maxLength(1000)]],
      actualHours: [this.job().actualHours || null],
      isServiceCompleted: [this.job().isServiceCompleted || false],
      isEngineServiceCompleted: [this.job().isEngineServiceCompleted || false],
      isDrifterServiceCompleted: [this.job().isDrifterServiceCompleted || false],
      drillBitsUsed: [this.job().drillBitsUsed || 0, [Validators.min(0)]],
      drillBitType: [this.job().drillBitType || ''],
      drillRodsUsed: [this.job().drillRodsUsed || 0, [Validators.min(0)]],
      drillRodType: [this.job().drillRodType || ''],
      shanksUsed: [this.job().shanksUsed || 0, [Validators.min(0)]],
      shankType: [this.job().shankType || '']
    });
  }

  ngOnInit() {
    this.updateValidators();
  }

  // Computed properties
  showMaintenanceReport(): boolean {
    const status = this.statusForm.get('status')?.value;
    // Only show maintenance report section for completed status
    // Backend only supports observations, actualHours, and partsReplaced on completion
    return status === MaintenanceStatus.COMPLETED;
  }

  // Event Handlers
  onStatusChange(status: MaintenanceStatus) {
    this.updateValidators();
  }

  onCancel() {
    this.dialogRef.close();
  }

  async onSubmit() {
    if (!this.statusForm.valid) {
      return;
    }

    this.isSubmitting.set(true);

    try {
      const formValue = this.statusForm.value;
      const newStatus = formValue.status;

      let result: any;

      // Use different endpoint based on status
      if (newStatus === MaintenanceStatus.COMPLETED) {
        // Complete endpoint requires observations, actualHours, partsReplaced, and consumed components
        const completionData: any = {
          observations: formValue.observations,
          actualHours: formValue.actualHours,
          partsReplaced: this.partsReplaced(),
          isServiceCompleted: formValue.isServiceCompleted,
          isEngineServiceCompleted: formValue.isEngineServiceCompleted,
          isDrifterServiceCompleted: formValue.isDrifterServiceCompleted
        };

        // Add consumed drilling components if service was completed
        if (formValue.isServiceCompleted) {
          completionData.drillBitsUsed = formValue.drillBitsUsed || 0;
          completionData.drillBitType = formValue.drillBitType || '';
          completionData.drillRodsUsed = formValue.drillRodsUsed || 0;
          completionData.drillRodType = formValue.drillRodType || '';
          completionData.shanksUsed = formValue.shanksUsed || 0;
          completionData.shankType = formValue.shankType || '';
        }

        result = await this.maintenanceService.completeMaintenanceJob(
          this.job().id,
          completionData.observations,
          completionData.actualHours,
          completionData.partsReplaced,
          completionData.isServiceCompleted,
          completionData.isEngineServiceCompleted,
          completionData.isDrifterServiceCompleted,
          completionData.drillBitsUsed,
          completionData.drillBitType,
          completionData.drillRodsUsed,
          completionData.drillRodType,
          completionData.shanksUsed,
          completionData.shankType
        ).toPromise();
      } else {
        // Status update endpoint only accepts status
        await this.maintenanceService.updateJobStatus(this.job().id, newStatus).toPromise();
        result = {
          ...this.job(),
          status: newStatus
        };
      }

      // Show success message
      this.snackBar.open('Job status updated successfully', 'Close', {
        duration: 3000,
        panelClass: ['success-snackbar']
      });

      // Close dialog with updated job
      this.dialogRef.close(result);

    } catch (error) {
      console.error('Error updating job status:', error);
      this.snackBar.open('Failed to update job status', 'Close', {
        duration: 5000,
        panelClass: ['error-snackbar']
      });
    } finally {
      this.isSubmitting.set(false);
    }
  }

  // Parts Management
  addPart(event: any) {
    const value = (event.value || '').trim();
    if (value) {
      this.partsReplaced.update(parts => [...parts, value]);
      event.chipInput.clear();
    }
  }

  removePart(partToRemove: string) {
    this.partsReplaced.update(parts => parts.filter(part => part !== partToRemove));
  }

  // Utility Methods
  private updateValidators() {
    const status = this.statusForm.get('status')?.value;
    const observationsControl = this.statusForm.get('observations');
    const actualHoursControl = this.statusForm.get('actualHours');

    if (status === MaintenanceStatus.COMPLETED) {
      // For completed status, observations and actualHours are required
      observationsControl?.setValidators([
        Validators.required,
        Validators.minLength(20),
        Validators.maxLength(1000)
      ]);
      actualHoursControl?.setValidators([
        Validators.required,
        Validators.min(0.1),
        Validators.max(1000)
      ]);
    } else {
      // For other statuses, only max length validation
      observationsControl?.setValidators([
        Validators.maxLength(1000)
      ]);
      actualHoursControl?.setValidators([
        Validators.min(0.1),
        Validators.max(1000)
      ]);
    }

    observationsControl?.updateValueAndValidity();
    actualHoursControl?.updateValueAndValidity();
  }

  getCurrentStatusName(): string {
    return this.getStatusDisplayName(this.job().status);
  }

  getCurrentStatusIcon(): string {
    return this.getStatusIcon(this.job().status);
  }

  getCurrentStatusClass(): string {
    return this.getStatusChipClass(this.job().status);
  }

  getStatusDisplayName(status: MaintenanceStatus): string {
    const statusNames = {
      [MaintenanceStatus.SCHEDULED]: 'Scheduled',
      [MaintenanceStatus.IN_PROGRESS]: 'In Progress',
      [MaintenanceStatus.COMPLETED]: 'Completed',
      [MaintenanceStatus.CANCELLED]: 'Cancelled',
      [MaintenanceStatus.OVERDUE]: 'Overdue'
    };
    return statusNames[status];
  }

  getStatusIcon(status: MaintenanceStatus): string {
    const statusIcons = {
      [MaintenanceStatus.SCHEDULED]: 'schedule',
      [MaintenanceStatus.IN_PROGRESS]: 'play_circle',
      [MaintenanceStatus.COMPLETED]: 'check_circle',
      [MaintenanceStatus.CANCELLED]: 'cancel',
      [MaintenanceStatus.OVERDUE]: 'warning'
    };
    return statusIcons[status];
  }

  getStatusChipClass(status: MaintenanceStatus): string {
    return `status-${status.toLowerCase().replace('_', '-')}`;
  }

  getStatusIconClass(status: MaintenanceStatus): string {
    return `icon-${status.toLowerCase().replace('_', '-')}`;
  }
}