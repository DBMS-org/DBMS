import { Component, Inject, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { Textarea } from 'primeng/inputtextarea';
import { CheckboxModule } from 'primeng/checkbox';
import { RadioButtonModule } from 'primeng/radiobutton';
import { DividerModule } from 'primeng/divider';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { CardModule } from 'primeng/card';
import { MessageModule } from 'primeng/message';

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
    MatIconModule,
    ButtonModule,
    DropdownModule,
    InputTextModule,
    Textarea,
    CheckboxModule,
    RadioButtonModule,
    DividerModule,
    ProgressSpinnerModule,
    CardModule,
    MessageModule
  ],
  template: `
    <div class="report-dialog">
      <div class="dialog-header">
        <mat-icon class="header-icon">report_problem</mat-icon>
        <h2 mat-dialog-title>Report Issue</h2>
      </div>

      <mat-dialog-content>

        @if (isSubmitting()) {
          <div class="loading-container">
            <p-progressSpinner styleClass="custom-spinner" [style]="{ width: '40px', height: '40px' }"></p-progressSpinner>
            <p>Submitting your report...</p>
          </div>
        } @else if (error()) {
          <div class="error-container">
            <mat-icon color="warn">error</mat-icon>
            <p>{{ error() }}</p>
          </div>
        } @else {
          <form [formGroup]="reportForm" (ngSubmit)="submitReport()">

              <div class="form-fields-container">
                <div class="form-row-group">
                  <div class="field full-width">
                    <label for="affectedPart" class="field-label">
                      Affected Part <span class="required-indicator">*</span>
                    </label>
                    <p-dropdown
                      id="affectedPart"
                      formControlName="affectedPart"
                      [options]="machinePartOptions"
                      optionLabel="label"
                      optionValue="value"
                      placeholder="Select affected part"
                      styleClass="w-full"
                      appendTo="body">
                      <ng-template let-option pTemplate="item">
                        <div class="option-with-icon">
                          <mat-icon>{{ option.icon }}</mat-icon>
                          <span>{{ option.label }}</span>
                        </div>
                      </ng-template>
                    </p-dropdown>
                    <small class="field-error" *ngIf="reportForm.get('affectedPart')?.hasError('required') && reportForm.get('affectedPart')?.touched">
                      Please select the affected part
                    </small>
                  </div>

                  <div class="field full-width">
                    <label for="problemCategory" class="field-label">
                      Problem Category <span class="required-indicator">*</span>
                    </label>
                    <p-dropdown
                      id="problemCategory"
                      formControlName="problemCategory"
                      [options]="problemCategoryOptions"
                      optionLabel="label"
                      optionValue="value"
                      placeholder="Select problem category"
                      styleClass="w-full"
                      appendTo="body">
                      <ng-template let-option pTemplate="item">
                        <div class="option-with-icon">
                          <mat-icon>{{ option.icon }}</mat-icon>
                          <span>{{ option.label }}</span>
                        </div>
                      </ng-template>
                    </p-dropdown>
                    <small class="field-error" *ngIf="reportForm.get('problemCategory')?.hasError('required') && reportForm.get('problemCategory')?.touched">
                      Please select a problem category
                    </small>
                  </div>
                </div>

                <div class="field full-width">
                  <label for="customDescription" class="field-label">
                    Description <span class="required-indicator">*</span>
                  </label>
                  <textarea
                    pInputTextarea
                    id="customDescription"
                    formControlName="customDescription"
                    rows="4"
                    placeholder="Describe the issue..."
                    class="w-full modern-textarea"></textarea>
                  <small class="field-error" *ngIf="reportForm.get('customDescription')?.hasError('required') && reportForm.get('customDescription')?.touched">
                    Please provide a description
                  </small>
                </div>

                <div class="field full-width">
                  <label for="severity" class="field-label">
                    Severity <span class="required-indicator">*</span>
                  </label>
                  <p-dropdown
                    id="severity"
                    formControlName="severity"
                    [options]="severityLevelOptions"
                    optionLabel="label"
                    optionValue="value"
                    placeholder="Select severity level"
                    styleClass="w-full"
                    appendTo="body">
                  </p-dropdown>
                  <small class="field-error" *ngIf="reportForm.get('severity')?.hasError('required') && reportForm.get('severity')?.touched">
                    Please select a severity level
                  </small>
                </div>
              </div>
          </form>
        }
      </mat-dialog-content>
      
      <mat-dialog-actions align="end">
        <p-button
          label="Cancel"
          severity="secondary"
          [outlined]="true"
          [mat-dialog-close]="false"
          [disabled]="isSubmitting()">
        </p-button>
        <p-button
          label="Submit"
          [loading]="isSubmitting()"
          (onClick)="submitReport()"
          [disabled]="reportForm.invalid || isSubmitting()">
        </p-button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .report-dialog {
      max-width: 100%;
    }

    /* Dialog Header */
    .dialog-header {
      display: flex;
      align-items: center;
      gap: 0.75rem;
      margin: -24px -24px 1.5rem -24px;
      padding: 1.25rem 1.5rem;
      background: linear-gradient(135deg, #1976d2, #1565c0);
      border-radius: 4px 4px 0 0;
    }

    .header-icon {
      font-size: 28px;
      width: 28px;
      height: 28px;
      color: white;
    }

    .dialog-header h2 {
      margin: 0;
      color: white;
      font-size: 1.25rem;
      font-weight: 600;
    }

    mat-dialog-content {
      padding: 0 1.5rem;
      overflow: visible !important;
      max-height: none !important;
    }

    /* Loading and Error States */
    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 3rem;
      text-align: center;
      background: white;
      border-radius: 12px;
      margin: 1rem 0;
    }

    .loading-container p {
      margin-top: 1rem;
      color: #64748b;
      font-size: 0.95rem;
      animation: pulse 2s ease-in-out infinite;
    }

    @keyframes pulse {
      0%, 100% { opacity: 1; }
      50% { opacity: 0.5; }
    }

    .error-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 2rem;
      text-align: center;
      background: linear-gradient(135deg, #fee2e2, #fecaca);
      border-radius: 12px;
      margin: 1rem 0;
      border: 1px solid #fca5a5;
      animation: shake 0.5s ease-in-out;
    }

    @keyframes shake {
      0%, 100% { transform: translateX(0); }
      25% { transform: translateX(-5px); }
      75% { transform: translateX(5px); }
    }

    .error-container mat-icon {
      font-size: 48px;
      height: 48px;
      width: 48px;
      margin-bottom: 1rem;
      color: #dc2626;
    }

    .error-container p {
      color: #991b1b;
      font-weight: 500;
    }


    .form-fields-container {
      display: flex;
      flex-direction: column;
      gap: 1.5rem;
      padding-bottom: 2rem;
      min-height: 400px;
    }

    .form-row-group {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 1.25rem;
    }

    .field {
      display: flex;
      flex-direction: column;
      gap: 0.625rem;
    }

    .field-label {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      font-weight: 600;
      color: #1e293b;
      font-size: 0.95rem;
      margin-bottom: 0.25rem;
    }

    .required-indicator {
      color: #dc2626;
      font-size: 1rem;
      margin-left: 0.125rem;
    }

    .full-width {
      width: 100%;
    }

    .w-full {
      width: 100%;
    }

    .field-error {
      display: flex;
      align-items: center;
      gap: 0.375rem;
      color: #dc2626;
      font-size: 0.85rem;
      margin-top: 0.375rem;
      padding: 0.5rem;
      background: #fee2e2;
      border-radius: 6px;
      border-left: 3px solid #dc2626;
      animation: slideIn 0.3s ease;
    }

    @keyframes slideIn {
      from {
        opacity: 0;
        transform: translateY(-5px);
      }
      to {
        opacity: 1;
        transform: translateY(0);
      }
    }

    .option-with-icon {
      display: flex;
      align-items: center;
      gap: 0.75rem;
      padding: 0.25rem 0;
    }

    .option-with-icon mat-icon {
      font-size: 20px;
      width: 20px;
      height: 20px;
      color: #1976d2;
    }

    /* Enhanced Input Styling */
    ::ng-deep .p-dropdown {
      width: 100%;
      border-radius: 10px;
      border: 2px solid #e2e8f0;
      transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
      background: linear-gradient(to bottom, #ffffff, #f9fafb);
      box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
      min-height: 46px;
    }

    ::ng-deep .p-dropdown:hover {
      border-color: #1976d2;
      box-shadow: 0 2px 8px rgba(25, 118, 210, 0.12);
      transform: translateY(-1px);
    }

    ::ng-deep .p-dropdown:not(.p-disabled).p-focus {
      border-color: #1976d2;
      box-shadow: 0 0 0 4px rgba(25, 118, 210, 0.15), 0 4px 12px rgba(25, 118, 210, 0.2);
      transform: translateY(-1px);
      background: white;
    }

    ::ng-deep .p-dropdown .p-dropdown-label {
      padding: 0.75rem 1rem;
      font-size: 0.95rem;
      color: #1e293b;
      font-weight: 500;
    }

    ::ng-deep .p-dropdown .p-dropdown-trigger {
      color: #1976d2;
      width: 2.5rem;
    }

    ::ng-deep .p-dropdown-panel {
      border-radius: 10px;
      box-shadow: 0 10px 40px rgba(0, 0, 0, 0.15) !important;
      border: 1px solid #e2e8f0 !important;
      margin-top: 0.5rem !important;
      background-color: #ffffff !important;
      background: #ffffff !important;
      z-index: 1100 !important;
      opacity: 1 !important;
    }

    ::ng-deep .p-dropdown-panel * {
      background-color: transparent;
    }

    ::ng-deep .p-dropdown-panel .p-dropdown-items-wrapper {
      background-color: #ffffff !important;
      background: #ffffff !important;
    }

    ::ng-deep .p-dropdown-panel .p-dropdown-items {
      padding: 0.5rem;
      background-color: #ffffff !important;
      background: #ffffff !important;
    }

    ::ng-deep .p-dropdown-panel .p-dropdown-item {
      border-radius: 6px;
      margin: 0.125rem 0;
      padding: 0.75rem 1rem;
      transition: all 0.2s ease;
      background-color: #ffffff !important;
      background: #ffffff !important;
    }

    ::ng-deep .p-dropdown-panel .p-dropdown-item:hover {
      background: linear-gradient(135deg, #eff6ff, #dbeafe) !important;
      background-color: #eff6ff !important;
      color: #1976d2;
    }

    ::ng-deep .p-dropdown-panel .p-dropdown-item.p-highlight {
      background: linear-gradient(135deg, #1976d2, #1565c0) !important;
      background-color: #1976d2 !important;
      color: white;
    }

    ::ng-deep .modern-textarea {
      border: 2px solid #e2e8f0;
      border-radius: 10px;
      padding: 1rem;
      font-size: 0.95rem;
      line-height: 1.6;
      transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
      resize: vertical;
      min-height: 140px;
      background: linear-gradient(to bottom, #ffffff, #f9fafb);
      box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
      font-family: inherit;
    }

    ::ng-deep .modern-textarea:hover {
      border-color: #1976d2;
      box-shadow: 0 2px 8px rgba(25, 118, 210, 0.12);
      transform: translateY(-1px);
    }

    ::ng-deep .modern-textarea:focus {
      border-color: #1976d2;
      box-shadow: 0 0 0 4px rgba(25, 118, 210, 0.15), 0 4px 12px rgba(25, 118, 210, 0.2);
      outline: none;
      background: white;
      transform: translateY(-1px);
    }

    ::ng-deep .modern-textarea::placeholder {
      color: #94a3b8;
      font-style: italic;
    }

    ::ng-deep .modern-input-group {
      border-radius: 10px;
      overflow: hidden;
      border: 2px solid #e2e8f0;
      transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
      background: white;
      box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
    }

    ::ng-deep .modern-input-group:hover {
      border-color: #1976d2;
      box-shadow: 0 2px 8px rgba(25, 118, 210, 0.12);
      transform: translateY(-1px);
    }

    ::ng-deep .modern-input-group:focus-within {
      border-color: #1976d2;
      box-shadow: 0 0 0 4px rgba(25, 118, 210, 0.15), 0 4px 12px rgba(25, 118, 210, 0.2);
      transform: translateY(-1px);
    }

    ::ng-deep .modern-input-group .p-inputgroup-addon {
      background: linear-gradient(135deg, #f8fafc, #f1f5f9);
      border: none;
      padding: 0 1rem;
      display: flex;
      align-items: center;
      justify-content: center;
      min-width: 50px;
    }

    ::ng-deep .modern-input-group .modern-input {
      border: none;
      padding: 0.75rem 1rem;
      font-size: 0.95rem;
      color: #1e293b;
      font-weight: 500;
      background: transparent;
    }

    ::ng-deep .modern-input-group .modern-input:focus {
      outline: none;
      box-shadow: none;
    }

    ::ng-deep .modern-input-group .modern-input::placeholder {
      color: #94a3b8;
      font-style: italic;
      font-weight: 400;
    }

    ::ng-deep .p-inputgroup-addon mat-icon {
      font-size: 22px;
      width: 22px;
      height: 22px;
      color: #1976d2;
    }

    /* Input focus glow effect */
    @keyframes inputGlow {
      0% {
        box-shadow: 0 0 0 0 rgba(25, 118, 210, 0.4);
      }
      50% {
        box-shadow: 0 0 0 6px rgba(25, 118, 210, 0);
      }
      100% {
        box-shadow: 0 0 0 0 rgba(25, 118, 210, 0);
      }
    }

    ::ng-deep .p-dropdown:not(.p-disabled).p-focus,
    ::ng-deep .modern-textarea:focus,
    ::ng-deep .modern-input-group:focus-within {
      animation: inputGlow 0.6s ease-out;
    }


    /* Responsive Design */
    @media (max-width: 768px) {
      .dialog-header {
        padding: 1.25rem 1rem;
        margin: -24px -24px 1rem;
      }

      .header-icon {
        font-size: 24px;
        width: 24px;
        height: 24px;
      }

      .dialog-header h2 {
        font-size: 1.125rem;
      }

      mat-dialog-content {
        padding: 0 1rem;
      }

      .form-row-group {
        grid-template-columns: 1fr;
      }
    }

    /* Smooth scrollbar */
    mat-dialog-content::-webkit-scrollbar {
      width: 8px;
    }

    mat-dialog-content::-webkit-scrollbar-track {
      background: #f1f5f9;
      border-radius: 4px;
    }

    mat-dialog-content::-webkit-scrollbar-thumb {
      background: #cbd5e1;
      border-radius: 4px;
      transition: background 0.2s ease;
    }

    mat-dialog-content::-webkit-scrollbar-thumb:hover {
      background: #94a3b8;
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

  toggleSymptomCard(symptomValue: string): void {
    const index = this.selectedSymptoms.indexOf(symptomValue);
    if (index === -1) {
      this.selectedSymptoms.push(symptomValue);
    } else {
      this.selectedSymptoms.splice(index, 1);
    }
  }

  isSymptomSelected(symptomValue: string): boolean {
    return this.selectedSymptoms.includes(symptomValue);
  }

  selectedSymptomsCount(): number {
    return this.selectedSymptoms.length;
  }

  getSeverityIcon(severity: string): string {
    switch (severity) {
      case SeverityLevel.CRITICAL:
        return 'error';
      case SeverityLevel.HIGH:
        return 'warning';
      case SeverityLevel.MEDIUM:
        return 'info';
      case SeverityLevel.LOW:
        return 'check_circle';
      default:
        return 'help';
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
