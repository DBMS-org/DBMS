import { Component, Inject, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';

import { AuthService } from '../../../../core/services/auth.service';
import { MaintenanceReportService } from '../../maintenance-reports/services/maintenance-report.service';
import { 
  OperatorMachine, 
  MachinePart, 
  ProblemCategory, 
  SeverityLevel,
  MACHINE_PART_OPTIONS,
  PROBLEM_CATEGORY_OPTIONS,
  SEVERITY_LEVEL_OPTIONS,
  COMMON_SYMPTOMS,
  CreateProblemReportRequest,
  ProblemReport
} from '../../maintenance-reports/models/maintenance-report.models';

interface DialogData {
  machine: OperatorMachine;
}

@Component({
  selector: 'app-machine-report-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatRadioModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatDividerModule
  ],
  template: `
    <div class="report-dialog">
      <h2 mat-dialog-title>Report Machine Issue</h2>
      
      <mat-dialog-content>
        <div class="machine-info">
          <h3>Machine Details</h3>
          <div class="info-grid">
            <div class="info-item">
              <span class="label">Name:</span>
              <span class="value">{{ data.machine.name }}</span>
            </div>
            <div class="info-item">
              <span class="label">Model:</span>
              <span class="value">{{ data.machine.model }}</span>
            </div>
            <div class="info-item">
              <span class="label">Serial Number:</span>
              <span class="value">{{ data.machine.serialNumber }}</span>
            </div>
            <div class="info-item">
              <span class="label">Location:</span>
              <span class="value">{{ data.machine.currentLocation }}</span>
            </div>
          </div>
        </div>
        
        <mat-divider></mat-divider>
        
        @if (isSubmitting()) {
          <div class="loading-container">
            <mat-spinner diameter="40"></mat-spinner>
            <p>Submitting your report...</p>
          </div>
        } @else if (error()) {
          <div class="error-container">
            <mat-icon color="warn">error</mat-icon>
            <p>{{ error() }}</p>
          </div>
        } @else {
          <form [formGroup]="reportForm" (ngSubmit)="submitReport()">
            <div class="form-section">
              <h3>Issue Details</h3>
              
              <div class="form-row">
                <mat-form-field appearance="outline" class="full-width">
                  <mat-label>Affected Part</mat-label>
                  <mat-select formControlName="affectedPart">
                    @for (option of machinePartOptions; track option.value) {
                      <mat-option [value]="option.value">
                        <div class="option-with-icon">
                          <mat-icon>{{ option.icon }}</mat-icon>
                          <span>{{ option.label }}</span>
                        </div>
                      </mat-option>
                    }
                  </mat-select>
                  <mat-error *ngIf="reportForm.get('affectedPart')?.hasError('required')">
                    Please select the affected part
                  </mat-error>
                </mat-form-field>
              </div>
              
              <div class="form-row">
                <mat-form-field appearance="outline" class="full-width">
                  <mat-label>Problem Category</mat-label>
                  <mat-select formControlName="problemCategory">
                    @for (option of problemCategoryOptions; track option.value) {
                      <mat-option [value]="option.value">
                        <div class="option-with-icon">
                          <mat-icon>{{ option.icon }}</mat-icon>
                          <span>{{ option.label }}</span>
                        </div>
                      </mat-option>
                    }
                  </mat-select>
                  <mat-error *ngIf="reportForm.get('problemCategory')?.hasError('required')">
                    Please select a problem category
                  </mat-error>
                </mat-form-field>
              </div>
              
              <div class="form-row">
                <mat-form-field appearance="outline" class="full-width">
                  <mat-label>Description</mat-label>
                  <textarea 
                    matInput 
                    formControlName="customDescription" 
                    rows="3" 
                    placeholder="Describe the issue in detail..."></textarea>
                  <mat-error *ngIf="reportForm.get('customDescription')?.hasError('required')">
                    Please provide a description
                  </mat-error>
                  <mat-error *ngIf="reportForm.get('customDescription')?.hasError('minlength')">
                    Description must be at least 10 characters
                  </mat-error>
                </mat-form-field>
              </div>
              
              <div class="form-row">
                <h4>Symptoms (select all that apply)</h4>
                <div class="symptoms-grid">
                  @for (symptom of commonSymptoms; track symptom.value) {
                    <mat-checkbox [value]="symptom.value" (change)="onSymptomToggle(symptom.value, $event.checked)">
                      {{ symptom.label }}
                    </mat-checkbox>
                  }
                </div>
              </div>
              
              <div class="form-row">
                <mat-form-field appearance="outline" class="full-width">
                  <mat-label>Error Codes (if any)</mat-label>
                  <input matInput formControlName="errorCodes" placeholder="Enter any error codes displayed">
                </mat-form-field>
              </div>
            </div>
            
            <div class="form-section">
              <h3>Issue Severity</h3>
              <div class="severity-options">
                @for (option of severityLevelOptions; track option.value) {
                  <mat-radio-button 
                    [value]="option.value" 
                    [checked]="reportForm.get('severity')?.value === option.value"
                    (change)="reportForm.get('severity')?.setValue(option.value)">
                    <div class="severity-option">
                      <div class="severity-header">
                        <span class="severity-label" [style.color]="option.color">{{ option.label }}</span>
                      </div>
                      <div class="severity-description">{{ option.description }}</div>
                    </div>
                  </mat-radio-button>
                }
                <mat-error *ngIf="reportForm.get('severity')?.hasError('required') && reportForm.get('severity')?.touched">
                  Please select a severity level
                </mat-error>
              </div>
            </div>
          </form>
        }
      </mat-dialog-content>
      
      <mat-dialog-actions align="end">
        <button mat-button [mat-dialog-close]="false" [disabled]="isSubmitting()">Cancel</button>
        <button 
          mat-raised-button 
          color="primary" 
          (click)="submitReport()" 
          [disabled]="reportForm.invalid || isSubmitting()">
          Submit Report
        </button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .report-dialog {
      max-width: 100%;
    }
    
    mat-dialog-content {
      max-height: 70vh;
      overflow-y: auto;
    }
    
    .machine-info {
      margin-bottom: 1.5rem;
    }
    
    .machine-info h3 {
      margin-top: 0;
      margin-bottom: 1rem;
      color: #333;
      font-size: 1.1rem;
    }
    
    .info-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
      gap: 1rem;
    }
    
    .info-item {
      display: flex;
      flex-direction: column;
    }
    
    .info-item .label {
      font-size: 0.85rem;
      color: #757575;
      margin-bottom: 0.25rem;
    }
    
    .info-item .value {
      font-weight: 500;
    }
    
    mat-divider {
      margin: 1.5rem 0;
    }
    
    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 2rem;
      text-align: center;
    }
    
    .error-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 1.5rem;
      text-align: center;
      background-color: #ffebee;
      border-radius: 4px;
      margin: 1rem 0;
    }
    
    .error-container mat-icon {
      font-size: 36px;
      height: 36px;
      width: 36px;
      margin-bottom: 1rem;
    }
    
    .form-section {
      margin-bottom: 2rem;
    }
    
    .form-section h3 {
      margin-top: 0;
      margin-bottom: 1rem;
      color: #333;
      font-size: 1.1rem;
    }
    
    .form-row {
      margin-bottom: 1.5rem;
    }
    
    .form-row h4 {
      margin-top: 0;
      margin-bottom: 0.75rem;
      color: #555;
      font-size: 1rem;
    }
    
    .full-width {
      width: 100%;
    }
    
    .option-with-icon {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }
    
    .symptoms-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
      gap: 0.75rem;
    }
    
    .severity-options {
      display: flex;
      flex-direction: column;
      gap: 1rem;
    }
    
    .severity-option {
      padding-left: 0.5rem;
    }
    
    .severity-header {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      margin-bottom: 0.25rem;
    }
    
    .severity-label {
      font-weight: 500;
    }
    
    .severity-description {
      font-size: 0.85rem;
      color: #757575;
    }
  `]
})
export class MachineReportDialogComponent {
  reportForm: FormGroup;
  isSubmitting = signal(false);
  error = signal<string | null>(null);
  
  machinePartOptions = MACHINE_PART_OPTIONS;
  problemCategoryOptions = PROBLEM_CATEGORY_OPTIONS;
  severityLevelOptions = SEVERITY_LEVEL_OPTIONS;
  commonSymptoms = COMMON_SYMPTOMS;
  
  private selectedSymptoms: string[] = [];
  private authService = inject(AuthService);
  private maintenanceReportService = inject(MaintenanceReportService);
  private fb = inject(FormBuilder);
  
  constructor(
    public dialogRef: MatDialogRef<MachineReportDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {
    this.reportForm = this.fb.group({
      affectedPart: [MachinePart.OTHER, Validators.required],
      problemCategory: [ProblemCategory.OTHER, Validators.required],
      customDescription: ['', [Validators.required, Validators.minLength(10)]],
      errorCodes: [''],
      severity: [SeverityLevel.MEDIUM, Validators.required]
    });
  }
  
  onSymptomToggle(symptomValue: string, checked: boolean): void {
    if (checked) {
      this.selectedSymptoms.push(symptomValue);
    } else {
      const index = this.selectedSymptoms.indexOf(symptomValue);
      if (index !== -1) {
        this.selectedSymptoms.splice(index, 1);
      }
    }
  }
  
  async submitReport(): Promise<void> {
    if (this.reportForm.invalid) {
      this.reportForm.markAllAsTouched();
      return;
    }
    
    this.isSubmitting.set(true);
    this.error.set(null);
    
    const currentUser = this.authService.getCurrentUser();
    if (!currentUser) {
      this.error.set('User information not available. Please log in again.');
      this.isSubmitting.set(false);
      return;
    }
    
    const formValue = this.reportForm.value;
    const reportData: CreateProblemReportRequest = {
      machineId: this.data.machine.id,
      machineName: this.data.machine.name,
      machineModel: this.data.machine.model,
      serialNumber: this.data.machine.serialNumber,
      location: this.data.machine.currentLocation,
      affectedPart: formValue.affectedPart,
      problemCategory: formValue.problemCategory,
      customDescription: formValue.customDescription,
      symptoms: this.selectedSymptoms,
      errorCodes: formValue.errorCodes || undefined,
      severity: formValue.severity
    };
    
    try {
      const result = await this.maintenanceReportService.submitProblemReport(reportData);
      this.dialogRef.close(result);
    } catch (error: any) {
      console.error('Error submitting report:', error);
      this.error.set(error.message || 'Failed to submit report. Please try again.');
      this.isSubmitting.set(false);
    }
  }
}
