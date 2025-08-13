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

          <!-- Maintenance Report Section (shown for In-Progress and Completed) -->
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
            </div>
          }

          <!-- File Upload Section -->
          <div class="file-upload-section">
            <h4>
              <mat-icon>attach_file</mat-icon>
              Attachments
            </h4>

            <!-- File Upload Area -->
            <div class="file-upload-area" 
                 (dragover)="onDragOver($event)" 
                 (dragleave)="onDragLeave($event)"
                 (drop)="onDrop($event)"
                 [class.drag-over]="isDragOver()">
              <input #fileInput 
                     type="file" 
                     multiple 
                     accept=".jpg,.jpeg,.png,.pdf,.txt,.doc,.docx"
                     (change)="onFileSelected($event)"
                     style="display: none;">
              
              <div class="upload-content">
                <mat-icon>cloud_upload</mat-icon>
                <p>Drag and drop files here or <button type="button" mat-button (click)="fileInput.click()">browse</button></p>
                <small>Supported formats: JPG, PNG, PDF, TXT, DOC (Max 10MB each)</small>
              </div>
            </div>

            <!-- Selected Files List -->
            @if (selectedFiles().length > 0) {
              <div class="selected-files">
                <h5>Selected Files:</h5>
                @for (file of selectedFiles(); track file.name) {
                  <div class="file-item">
                    <mat-icon>{{ getFileIcon(file.type) }}</mat-icon>
                    <div class="file-info">
                      <span class="filename">{{ file.name }}</span>
                      <span class="filesize">{{ formatFileSize(file.size) }}</span>
                    </div>
                    <button mat-icon-button 
                            type="button"
                            (click)="removeFile(file)"
                            aria-label="Remove file">
                      <mat-icon>close</mat-icon>
                    </button>
                  </div>
                }
              </div>
            }

            <!-- File Upload Errors -->
            @if (fileErrors().length > 0) {
              <div class="file-errors">
                @for (error of fileErrors(); track error) {
                  <div class="error-message">
                    <mat-icon>error</mat-icon>
                    <span>{{ error }}</span>
                  </div>
                }
              </div>
            }
          </div>

          <!-- Notification Options -->
          <div class="notification-section">
            <h4>
              <mat-icon>notifications</mat-icon>
              Notifications
            </h4>
            
            <mat-checkbox formControlName="notifyMachineManager">
              Notify Machine Manager automatically
            </mat-checkbox>
          </div>
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

    .maintenance-report-section,
    .file-upload-section,
    .notification-section {
      border: 1px solid #e0e0e0;
      border-radius: 4px;
      padding: 16px;
    }

    .maintenance-report-section h4,
    .file-upload-section h4,
    .notification-section h4 {
      display: flex;
      align-items: center;
      gap: 8px;
      margin: 0 0 16px 0;
      font-size: 16px;
      font-weight: 500;
    }

    .file-upload-area {
      border: 2px dashed #ccc;
      border-radius: 4px;
      padding: 32px;
      text-align: center;
      transition: border-color 0.3s, background-color 0.3s;
      cursor: pointer;
    }

    .file-upload-area:hover {
      border-color: #1976d2;
      background-color: #f8f9fa;
    }

    .file-upload-area.drag-over {
      border-color: #1976d2;
      background-color: #e3f2fd;
    }

    .upload-content mat-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
      color: #666;
      margin-bottom: 16px;
    }

    .upload-content p {
      margin: 0 0 8px 0;
      font-size: 16px;
    }

    .upload-content small {
      color: #666;
      font-size: 12px;
    }

    .selected-files {
      margin-top: 16px;
    }

    .selected-files h5 {
      margin: 0 0 12px 0;
      font-size: 14px;
      font-weight: 500;
    }

    .file-item {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 8px;
      border: 1px solid #e0e0e0;
      border-radius: 4px;
      margin-bottom: 8px;
    }

    .file-info {
      flex: 1;
      display: flex;
      flex-direction: column;
    }

    .filename {
      font-weight: 500;
    }

    .filesize {
      font-size: 12px;
      color: #666;
    }

    .file-errors {
      margin-top: 16px;
    }

    .error-message {
      display: flex;
      align-items: center;
      gap: 8px;
      color: #d32f2f;
      font-size: 14px;
      margin-bottom: 8px;
    }

    .error-message mat-icon {
      font-size: 16px;
      width: 16px;
      height: 16px;
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
  isDragOver = signal(false);
  selectedFiles = signal<File[]>([]);
  fileErrors = signal<string[]>([]);
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

  maxFileSize = 10 * 1024 * 1024; // 10MB
  allowedFileTypes = [
    'image/jpeg', 'image/jpg', 'image/png',
    'application/pdf',
    'text/plain',
    'application/msword',
    'application/vnd.openxmlformats-officedocument.wordprocessingml.document'
  ];

  constructor() {
    this.statusForm = this.fb.group({
      status: [this.job().status, [Validators.required]],
      observations: [this.job().observations || '', [Validators.maxLength(1000)]],
      actualHours: [this.job().actualHours || null],
      notifyMachineManager: [true]
    });
  }

  ngOnInit() {
    this.updateValidators();
  }

  // Computed properties
  showMaintenanceReport(): boolean {
    const status = this.statusForm.get('status')?.value;
    return status === MaintenanceStatus.IN_PROGRESS || status === MaintenanceStatus.COMPLETED;
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
      const updatedJob: Partial<MaintenanceJob> = {
        status: formValue.status,
        observations: formValue.observations,
        actualHours: formValue.actualHours,
        partsReplaced: this.partsReplaced(),
        updatedAt: new Date()
      };

      // Update job status
      await this.maintenanceService.updateMaintenanceJob(this.job().id, updatedJob).toPromise();

      // Upload files if any
      if (this.selectedFiles().length > 0) {
        await this.uploadFiles();
      }

      // Show success message
      this.snackBar.open('Job status updated successfully', 'Close', {
        duration: 3000,
        panelClass: ['success-snackbar']
      });

      // Close dialog with updated job
      this.dialogRef.close({ ...this.job(), ...updatedJob });

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

  // File Handling
  onDragOver(event: DragEvent) {
    event.preventDefault();
    this.isDragOver.set(true);
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    this.isDragOver.set(false);
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    this.isDragOver.set(false);
    
    const files = Array.from(event.dataTransfer?.files || []);
    this.processFiles(files);
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    const files = Array.from(input.files || []);
    this.processFiles(files);
    input.value = ''; // Reset input
  }

  private processFiles(files: File[]) {
    const errors: string[] = [];
    const validFiles: File[] = [];

    files.forEach(file => {
      // Check file type
      if (!this.allowedFileTypes.includes(file.type)) {
        errors.push(`${file.name}: Unsupported file type`);
        return;
      }

      // Check file size
      if (file.size > this.maxFileSize) {
        errors.push(`${file.name}: File size exceeds 10MB limit`);
        return;
      }

      // Check for duplicates
      const existingFiles = this.selectedFiles();
      if (existingFiles.some(f => f.name === file.name && f.size === file.size)) {
        errors.push(`${file.name}: File already selected`);
        return;
      }

      validFiles.push(file);
    });

    // Update signals
    this.fileErrors.set(errors);
    this.selectedFiles.update(current => [...current, ...validFiles]);
  }

  removeFile(fileToRemove: File) {
    this.selectedFiles.update(files => files.filter(file => file !== fileToRemove));
  }

  private async uploadFiles(): Promise<void> {
    const files = this.selectedFiles();
    const uploadPromises = files.map(file => 
      this.maintenanceService.uploadMaintenanceFile(this.job().id, file).toPromise()
    );

    await Promise.all(uploadPromises);
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
    const actualHoursControl = this.statusForm.get('actualHours');

    if (status === MaintenanceStatus.COMPLETED) {
      actualHoursControl?.setValidators([
        Validators.required,
        Validators.min(0.5),
        Validators.max(24)
      ]);
    } else {
      actualHoursControl?.setValidators([
        Validators.min(0.5),
        Validators.max(24)
      ]);
    }

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

  getFileIcon(fileType: string): string {
    if (fileType.startsWith('image/')) {
      return 'image';
    } else if (fileType === 'application/pdf') {
      return 'picture_as_pdf';
    } else if (fileType.startsWith('text/')) {
      return 'description';
    } else if (fileType.includes('word')) {
      return 'description';
    } else {
      return 'attach_file';
    }
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }
}