import { Component, OnInit, signal, inject, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { MatBadgeModule } from '@angular/material/badge';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

import { AuthService } from '../../../core/services/auth.service';
import { MaintenanceReportService } from '../maintenance-reports/services/maintenance-report.service';
import { OperatorMachine, ProblemReport } from '../maintenance-reports/models/maintenance-report.models';
import { MachineReportDialogComponent } from './machine-report-dialog/machine-report-dialog.component';
import { MaintenanceService } from '../../mechanical-engineer/maintenance/services/maintenance.service';
import { UsageMetrics } from '../../mechanical-engineer/maintenance/models/maintenance.models';
import { 
  MachineUsageLog, 
  CreateUsageLogRequest, 
  UsageLogFormData, 
  UsageLogUtils,
  UsageLogValidation 
} from './models/usage-log.models';

@Component({
  selector: 'app-my-machines',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    MatBadgeModule,
    MatChipsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatSnackBarModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ReactiveFormsModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="my-machines-container">
      <header class="page-header">
        <h1>My Assigned Machines</h1>
        <div class="header-actions">
          <button 
            mat-raised-button 
            color="primary"
            class="refresh-btn" 
            (click)="refreshData()"
            [disabled]="isLoading()">
            <mat-icon>refresh</mat-icon>
            Refresh
          </button>
        </div>
      </header>
      
      <div class="machines-content">
        @if (isLoading()) {
          <div class="loading-container">
            <mat-spinner></mat-spinner>
            <p>Loading your assigned machines...</p>
          </div>
        } @else if (error()) {
          <div class="error-container">
            <mat-icon color="warn">error</mat-icon>
            <p>{{ error() }}</p>
            <button mat-raised-button color="primary" (click)="refreshData()">Try Again</button>
          </div>
        } @else if (!assignedMachine()) {
          <div class="no-machines-container">
            <mat-icon>precision_manufacturing</mat-icon>
            <h3>No Machines Assigned</h3>
            <p>You currently don't have any machines assigned to you.</p>
            <p>Please contact your supervisor if you believe this is an error.</p>
          </div>
        } @else {
          <div class="machine-details-container">
            <mat-card class="machine-card">
              <mat-card-header>
                <div mat-card-avatar class="machine-avatar">
                  <mat-icon>precision_manufacturing</mat-icon>
                </div>
                <mat-card-title>{{ assignedMachine()?.name }}</mat-card-title>
                <mat-card-subtitle>
                  <span [ngClass]="getMachineStatusClass(assignedMachine()?.status || '')">
                    {{ getMachineStatusLabel(assignedMachine()?.status || '') }}
                  </span>
                </mat-card-subtitle>
              </mat-card-header>
              
              <mat-card-content>
                <div class="machine-info-grid">
                  <div class="info-item">
                    <div class="info-label">
                      <mat-icon>category</mat-icon>
                      <span>Model</span>
                    </div>
                    <div class="info-value">{{ assignedMachine()?.model }}</div>
                  </div>
                  
                  <div class="info-item">
                    <div class="info-label">
                      <mat-icon>qr_code</mat-icon>
                      <span>Serial Number</span>
                    </div>
                    <div class="info-value">{{ assignedMachine()?.serialNumber }}</div>
                  </div>
                  
                  <div class="info-item">
                    <div class="info-label">
                      <mat-icon>location_on</mat-icon>
                      <span>Location</span>
                    </div>
                    <div class="info-value">{{ assignedMachine()?.currentLocation }}</div>
                  </div>
                </div>
                
                <mat-divider class="section-divider"></mat-divider>

                <div class="usage-metrics">
                  <h3>Usage Metrics</h3>
                  @if (usageMetrics()) {
                    <div class="usage-grid">
                      <div class="usage-item">
                        <div class="usage-label"><mat-icon>speed</mat-icon> Engine Hours</div>
                        <div class="usage-value">{{ usageMetrics()?.engineHours | number }}</div>
                      </div>
                      <div class="usage-item">
                        <div class="usage-label"><mat-icon>hourglass_empty</mat-icon> Idle Hours</div>
                        <div class="usage-value">{{ usageMetrics()?.idleHours | number }}</div>
                      </div>
                      <div class="usage-item">
                        <div class="usage-label"><mat-icon>build</mat-icon> Service Hours</div>
                        <div class="usage-value">{{ usageMetrics()?.serviceHours | number }}</div>
                      </div>
                      <div class="usage-item">
                        <div class="usage-label"><mat-icon>schedule</mat-icon> Remaining to Next Service</div>
                      <div class="usage-value">{{ remainingServiceHours() === null ? '—' : (remainingServiceHours()! | number) }} h</div>
                      </div>
                     </div>
                     <div class="last-updated">Last updated: {{ formatDate(usageMetrics()!.lastUpdated) }}</div>
                   } @else {
                     <p class="no-usage">No usage data available.</p>
                   }
                 </div>

                <mat-divider class="section-divider"></mat-divider>

                <div class="machine-actions">
                  <h3>Machine Actions</h3>
                  <div class="action-buttons">
                    <button
                      mat-raised-button
                      color="warn"
                      class="report-issue-btn"
                      (click)="reportIssue()">
                      <mat-icon>report_problem</mat-icon>
                      Report Issue
                    </button>

                    <button
                      mat-raised-button
                      color="primary"
                      class="log-usage-btn"
                      (click)="dialog.open(logUsageDialog)">
                      <mat-icon>edit</mat-icon>
                      Log Usage
                    </button>
                  </div>
                </div>

                <ng-template #logUsageDialog>
                  <h2 mat-dialog-title>Log Machine Usage</h2>
                  <mat-dialog-content class="log-usage-content">
                    <form [formGroup]="usageLogForm" class="usage-log-form">
                      
                      <!-- Machine Selection -->
                      <div class="form-section">
                        <h4>Machine Information</h4>
                        <mat-form-field class="full-width">
                          <mat-label>Select Machine</mat-label>
                          <mat-select formControlName="selectedMachineId" required>
                            <mat-option [value]="assignedMachine()?.id">
                              {{ assignedMachine()?.name }} ({{ assignedMachine()?.serialNumber }})
                            </mat-option>
                          </mat-select>
                          <mat-error *ngIf="usageLogForm.get('selectedMachineId')?.hasError('required')">
                            Machine selection is required
                          </mat-error>
                        </mat-form-field>
                      </div>

                      <!-- Date of Log -->
                      <div class="form-section">
                        <h4>Log Date</h4>
                        <mat-form-field class="full-width">
                          <mat-label>Date of Log</mat-label>
                          <input matInput [matDatepicker]="picker" formControlName="logDate" required>
                          <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
                          <mat-datepicker #picker></mat-datepicker>
                          <mat-error *ngIf="usageLogForm.get('logDate')?.hasError('required')">
                            Log date is required
                          </mat-error>
                        </mat-form-field>
                      </div>

                      <!-- Hours Section -->
                      <div class="form-section">
                        <h4>Operating Hours</h4>
                        <div class="hours-grid">
                          <mat-form-field>
                            <mat-label>Engine Hours (HH:MM)</mat-label>
                            <input matInput formControlName="engineHours" placeholder="00:00" pattern="^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$" required>
                            <mat-hint>Total hours machine was running</mat-hint>
                            <mat-error *ngIf="usageLogForm.get('engineHours')?.hasError('required')">
                              Engine hours are required
                            </mat-error>
                            <mat-error *ngIf="usageLogForm.get('engineHours')?.hasError('pattern')">
                              Please use HH:MM format (e.g., 08:30)
                            </mat-error>
                          </mat-form-field>

                          <mat-form-field>
                            <mat-label>Idle Hours (HH:MM)</mat-label>
                            <input matInput formControlName="idleHours" placeholder="00:00" pattern="^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$" required>
                            <mat-hint>Hours machine was ON but not working</mat-hint>
                            <mat-error *ngIf="usageLogForm.get('idleHours')?.hasError('required')">
                              Idle hours are required
                            </mat-error>
                            <mat-error *ngIf="usageLogForm.get('idleHours')?.hasError('pattern')">
                              Please use HH:MM format (e.g., 02:15)
                            </mat-error>
                          </mat-form-field>

                          <mat-form-field>
                            <mat-label>Working Hours (HH:MM)</mat-label>
                            <input matInput formControlName="workingHours" placeholder="00:00" pattern="^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$" required>
                            <mat-hint>Actual productive hours</mat-hint>
                            <mat-error *ngIf="usageLogForm.get('workingHours')?.hasError('required')">
                              Working hours are required
                            </mat-error>
                            <mat-error *ngIf="usageLogForm.get('workingHours')?.hasError('pattern')">
                              Please use HH:MM format (e.g., 06:45)
                            </mat-error>
                          </mat-form-field>
                        </div>
                      </div>

                      <!-- Fuel Consumption -->
                      <div class="form-section">
                        <h4>Fuel Consumption (Optional)</h4>
                        <mat-form-field class="full-width">
                          <mat-label>Fuel Consumed (Liters)</mat-label>
                          <input matInput type="number" formControlName="fuelConsumed" min="0" step="0.1">
                          <mat-hint>Enter fuel consumption if tracked</mat-hint>
                        </mat-form-field>
                      </div>

                      <!-- Downtime/Breakdown Section -->
                      <div class="form-section">
                        <h4>Downtime & Breakdown</h4>
                        <mat-checkbox formControlName="hasDowntime" class="downtime-checkbox">
                          Machine experienced downtime or breakdown
                        </mat-checkbox>
                        
                        <div *ngIf="usageLogForm.get('hasDowntime')?.value" class="downtime-details">
                          <mat-form-field class="full-width">
                            <mat-label>Downtime Hours (HH:MM)</mat-label>
                            <input matInput formControlName="downtimeHours" placeholder="00:00" pattern="^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$">
                            <mat-hint>Total hours machine was down</mat-hint>
                            <mat-error *ngIf="usageLogForm.get('downtimeHours')?.hasError('pattern')">
                              Please use HH:MM format (e.g., 01:30)
                            </mat-error>
                          </mat-form-field>

                          <mat-form-field class="full-width">
                            <mat-label>Breakdown Description</mat-label>
                            <textarea matInput formControlName="breakdownDescription" rows="3" placeholder="Describe the breakdown or issue..."></textarea>
                            <mat-hint>Provide details about the breakdown</mat-hint>
                          </mat-form-field>
                        </div>
                      </div>

                      <!-- Remarks/Notes -->
                      <div class="form-section">
                        <h4>Remarks & Observations</h4>
                        <mat-form-field class="full-width">
                          <mat-label>Remarks/Notes</mat-label>
                          <textarea matInput formControlName="remarks" rows="4" placeholder="Any observations, unusual noise, overheating, etc..."></textarea>
                          <mat-hint>Optional: Add any observations or notes</mat-hint>
                        </mat-form-field>
                      </div>

                      <!-- File Upload Section -->
                      <div class="form-section">
                        <h4>Attachments (Optional)</h4>
                        <div class="file-upload-area">
                          <input type="file" #fileInput multiple accept="image/*,.pdf,.doc,.docx" (change)="onFileSelected($event)" style="display: none;">
                          <button type="button" mat-stroked-button (click)="fileInput.click()">
                            <mat-icon>attach_file</mat-icon>
                            Upload Photos/Documents
                          </button>
                          <p class="upload-hint">Upload photos of broken parts, meter readings, or relevant documents</p>
                          
                          <div *ngIf="selectedFiles.length > 0" class="selected-files">
                            <h5>Selected Files:</h5>
                            <div *ngFor="let file of selectedFiles; let i = index" class="file-item">
                              <mat-icon>{{ getFileIcon(file.type) }}</mat-icon>
                              <span class="file-name">{{ file.name }}</span>
                              <span class="file-size">({{ formatFileSize(file.size) }})</span>
                              <button type="button" mat-icon-button (click)="removeFile(i)" color="warn">
                                <mat-icon>close</mat-icon>
                              </button>
                            </div>
                          </div>
                        </div>
                      </div>

                    </form>
                  </mat-dialog-content>

                  <mat-dialog-actions align="end" class="dialog-actions">
                    <button mat-button mat-dialog-close type="button">Cancel</button>
                    <button mat-flat-button color="primary" type="button" 
                            (click)="submitEnhancedLogUsage()" 
                            [disabled]="!usageLogForm.valid || isSubmittingUsage">
                      <mat-icon *ngIf="isSubmittingUsage">hourglass_empty</mat-icon>
                      {{ isSubmittingUsage ? 'Saving...' : 'Save Usage Log' }}
                    </button>
                  </mat-dialog-actions>
                </ng-template>
                
                <mat-divider class="section-divider"></mat-divider>
                
                <div class="recent-reports">
                  <h3>
                    Recent Reports 
                    <mat-chip-option 
                      *ngIf="activeReportsCount() > 0"
                      color="warn" 
                      selected 
                      [disabled]="true">
                      {{ activeReportsCount() }} Active
                    </mat-chip-option>
                  </h3>
                  
                  @if (recentReports().length === 0) {
                    <p class="no-reports-message">No recent reports for this machine.</p>
                  } @else {
                    <div class="reports-list">
                      @for (report of recentReports(); track report.id) {
                        <div class="report-item" [ngClass]="getReportStatusClass(report.status)">
                          <div class="report-header">
                            <span class="report-id">Ticket #{{ report.ticketId }}</span>
                            <span class="report-date">{{ formatDate(report.reportedAt) }}</span>
                          </div>
                          <div class="report-content">
                            <h4>{{ getProblemCategoryLabel(report.problemCategory) }}</h4>
                            <p>{{ report.customDescription }}</p>
                          </div>
                          <div class="report-footer">
                            <span class="report-status">{{ getReportStatusLabel(report.status) }}</span>
                            <button mat-button color="primary" (click)="viewReportDetails(report)">
                              Details
                            </button>
                          </div>
                        </div>
                      }
                    </div>
                  }
                </div>
              </mat-card-content>
            </mat-card>
          </div>
        }
      </div>
    </div>
  `,
  styles: [`
    .my-machines-container {
      padding: 1rem;
      max-width: 1200px;
      margin: 0 auto;
    }

    .page-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 2rem;
    }

    .page-header h1 {
      margin: 0;
      color: #333;
    }

    .header-actions {
      display: flex;
      gap: 1rem;
    }

    .refresh-btn {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }

    .loading-container, .error-container, .no-machines-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 3rem;
      text-align: center;
      background-color: #f5f5f5;
      border-radius: 8px;
      margin-top: 2rem;
    }

    .error-container mat-icon, .no-machines-container mat-icon {
      font-size: 48px;
      height: 48px;
      width: 48px;
      margin-bottom: 1rem;
    }

    .error-container mat-icon {
      color: #f44336;
    }

    .no-machines-container mat-icon {
      color: #757575;
    }

    .machine-card {
      margin-bottom: 2rem;
      overflow: hidden;
      border-radius: 10px;
    }

    .machine-avatar {
      display: flex;
      align-items: center;
      justify-content: center;
      background-color: #1976d2;
      color: white;
    }

    .machine-info-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
      gap: 1.5rem;
      margin: 1.5rem 0;
    }

    /* Usage metrics layout */
    .usage-metrics { margin: 1rem 0; }
    .usage-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
      gap: 1rem;
    }
    .usage-item {
      background: #fafafa;
      border: 1px solid #eee;
      border-radius: 8px;
      padding: 0.75rem;
      box-shadow: 0 1px 2px rgba(0,0,0,0.04);
      transition: background-color 0.2s ease, border-color 0.2s ease, box-shadow 0.2s ease;
    }
    .usage-item:hover {
      background: #fdfdfd;
      border-color: #e6e6e6;
      box-shadow: 0 2px 6px rgba(0,0,0,0.06);
    }
    .usage-label {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      color: #616161;
      font-size: 0.875rem;
    }
    .usage-value {
      font-size: 1.25rem;
      font-weight: 600;
      color: #212121;
    }
    .last-updated { margin-top: 0.5rem; font-size: 0.8rem; color: #757575; }

    /* Log usage dialog styles */
    .log-usage-content { 
      display: block; 
      min-width: 400px; 
      max-width: 700px; 
      max-height: 80vh;
      overflow-y: auto;
    }
    
    .usage-log-form {
      display: flex;
      flex-direction: column;
      gap: 1.5rem;
    }
    
    .form-section {
      border: 1px solid #e0e0e0;
      border-radius: 8px;
      padding: 1rem;
      background: #fafafa;
    }
    
    .form-section h4 {
      margin: 0 0 1rem 0;
      color: #424242;
      font-size: 1rem;
      font-weight: 500;
    }
    
    .hours-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
      gap: 1rem;
    }
    
    .downtime-checkbox {
      margin-bottom: 1rem;
    }
    
    .downtime-details {
      margin-top: 1rem;
      padding-top: 1rem;
      border-top: 1px solid #e0e0e0;
    }
    
    .file-upload-area {
      text-align: center;
      padding: 1rem;
      border: 2px dashed #ccc;
      border-radius: 8px;
      background: #f9f9f9;
    }
    
    .upload-hint {
      margin: 0.5rem 0 0 0;
      font-size: 0.875rem;
      color: #666;
    }
    
    .selected-files {
      margin-top: 1rem;
      text-align: left;
    }
    
    .selected-files h5 {
      margin: 0 0 0.5rem 0;
      font-size: 0.875rem;
      color: #424242;
    }
    
    .file-item {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      padding: 0.5rem;
      background: white;
      border: 1px solid #e0e0e0;
      border-radius: 4px;
      margin-bottom: 0.5rem;
    }
    
    .file-name {
      flex: 1;
      font-size: 0.875rem;
    }
    
    .file-size {
      font-size: 0.75rem;
      color: #666;
    }
    
    .dialog-actions {
      padding: 1rem 1.5rem;
      border-top: 1px solid #e0e0e0;
    }
    
    .full-width { 
      width: 100%; 
    }
    @media (min-width: 600px) {
      .current-metrics { grid-template-columns: repeat(3, 1fr); }
    }

    .info-item {
      display: flex;
      flex-direction: column;
      gap: 0.5rem;
    }

    .info-label {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      color: #757575;
      font-size: 0.9rem;
    }

    .info-value {
      font-size: 1.1rem;
      font-weight: 500;
    }

    .section-divider {
      margin: 1.5rem 0;
    }

    .machine-actions {
      margin: 1.5rem 0;
    }

    .machine-actions h3 {
      margin-top: 0;
      margin-bottom: 1rem;
      color: #333;
    }

    .action-buttons {
      display: flex;
      gap: 1rem;
      flex-wrap: wrap;
    }

    .report-issue-btn, .view-history-btn {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }

    .recent-reports h3 {
      display: flex;
      align-items: center;
      gap: 1rem;
      margin-top: 0;
      margin-bottom: 1rem;
      color: #333;
    }

    .no-reports-message {
      color: #757575;
      font-style: italic;
    }

    .reports-list {
      display: flex;
      flex-direction: column;
      gap: 1rem;
    }

    .report-item {
      border-left: 4px solid #ccc;
      padding: 0.75rem 1rem;
      background-color: #f9f9f9;
      border-radius: 4px;
      transition: transform 0.15s ease, background-color 0.2s ease;
    }
    .report-item:hover { background-color: #f3f6ff; transform: translateY(-1px); }
    .report-item.status-reported {
      border-left-color: #2196f3;
    }

    .report-item.status-acknowledged {
      border-left-color: #ff9800;
    }

    .report-item.status-in-progress {
      border-left-color: #9c27b0;
    }

    .report-item.status-resolved {
      border-left-color: #4caf50;
    }

    .report-item.status-closed {
      border-left-color: #757575;
    }

    .report-header {
      display: flex;
      justify-content: space-between;
      margin-bottom: 0.5rem;
      font-size: 0.85rem;
      color: #757575;
    }

    .report-content h4 {
      margin: 0 0 0.5rem 0;
      font-size: 1rem;
    }

    .report-content p {
      margin: 0;
      font-size: 0.9rem;
      color: #555;
    }

    .report-footer {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-top: 0.75rem;
    }

    .report-status {
      font-size: 0.85rem;
      font-weight: 500;
      padding: 0.25rem 0.5rem;
      border-radius: 4px;
      background-color: #e0e0e0;
    }

    .status-operational {
      color: #4caf50;
      font-weight: 500;
    }

    .status-under-maintenance {
      color: #ff9800;
      font-weight: 500;
    }

    .status-down-for-repair {
      color: #f44336;
      font-weight: 500;
    }

    .status-offline {
      color: #757575;
      font-weight: 500;
    }

    @media (max-width: 768px) {
      .page-header {
        flex-direction: column;
        gap: 1rem;
        align-items: flex-start;
      }
      .page-header h1 { font-size: 1.5rem; }
      .my-machines-container { padding: 0.75rem; }
      .machine-info-grid { grid-template-columns: 1fr; }
      .action-buttons { flex-direction: column; }
      .action-buttons button { width: 100%; }
    }
  `]
})
export class MyMachinesComponent implements OnInit {
  assignedMachine = signal<OperatorMachine | null>(null);
  recentReports = signal<ProblemReport[]>([]);
  isLoading = signal(false);
  error = signal<string | null>(null);
  usageMetrics = signal<UsageMetrics | null>(null);
  serviceIntervalHours = signal<number>(500);
  
  // Enhanced usage logging properties
  usageLogForm!: FormGroup;
  selectedFiles: File[] = [];
  isSubmittingUsage = false;
  
  // Legacy properties for backward compatibility
  engineDelta = 0;
  idleDelta = 0;
  serviceDelta = 0;
  
  private maintenanceReportService = inject(MaintenanceReportService);
  private authService = inject(AuthService);
  public dialog = inject(MatDialog);
  private maintenanceService = inject(MaintenanceService);
  private snackBar = inject(MatSnackBar);
  private fb = inject(FormBuilder);
  
  // Enhanced usage logging methods
  onFileSelected(event: any) {
    const files = event.target.files;
    if (files) {
      for (let file of files) {
        if (file.size <= 10 * 1024 * 1024) { // 10MB limit
          this.selectedFiles.push(file);
        } else {
          this.snackBar.open(`File ${file.name} is too large (max 10MB)`, 'OK', { duration: 3000 });
        }
      }
    }
  }
  
  removeFile(index: number) {
    this.selectedFiles.splice(index, 1);
  }
  
  getFileIcon(fileType: string): string {
    if (fileType.startsWith('image/')) return 'image';
    if (fileType.includes('pdf')) return 'picture_as_pdf';
    if (fileType.includes('doc')) return 'description';
    return 'attach_file';
  }
  
  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }
  
  submitEnhancedLogUsage() {
    if (!this.usageLogForm.valid || this.isSubmittingUsage) return;
    
    this.isSubmittingUsage = true;
    const formData = this.usageLogForm.value;
    
    // Convert time strings to decimal hours
    const engineHours = this.timeStringToDecimal(formData.engineHours);
    const idleHours = this.timeStringToDecimal(formData.idleHours);
    const workingHours = this.timeStringToDecimal(formData.workingHours);
    const downtimeHours = formData.hasDowntime ? this.timeStringToDecimal(formData.downtimeHours) : 0;
    
    const usageLogData: CreateUsageLogRequest = {
        machineId: formData.selectedMachineId,
        logDate: formData.logDate,
        engineHours: engineHours.toString(),
        idleHours: idleHours.toString(),
        workingHours: workingHours.toString(),
        fuelConsumed: formData.fuelConsumed || 0,
        hasDowntime: formData.hasDowntime,
        downtimeHours: downtimeHours.toString(),
        breakdownDescription: formData.breakdownDescription || '',
        remarks: formData.remarks || '',
        attachments: this.selectedFiles
      };
    
    // Simulate API call - in real app, this would be a service call
    setTimeout(() => {
      // Update local usage metrics
      const currentMetrics = this.usageMetrics();
      if (currentMetrics) {
        const updatedMetrics: UsageMetrics = {
          ...currentMetrics,
          engineHours: currentMetrics.engineHours + engineHours,
          idleHours: currentMetrics.idleHours + idleHours,
          serviceHours: currentMetrics.serviceHours + engineHours, // Engine hours contribute to service hours
          lastUpdated: new Date()
        };
        this.usageMetrics.set(updatedMetrics);
      }
      
      this.isSubmittingUsage = false;
      this.selectedFiles = [];
      this.dialog.closeAll();
      this.snackBar.open('Usage log submitted successfully!', 'OK', { duration: 3000 });
      
      // Reset form
      this.initializeUsageLogForm();
    }, 2000);
  }
  
  private timeStringToDecimal(timeStr: string): number {
    if (!timeStr) return 0;
    const [hours, minutes] = timeStr.split(':').map(Number);
    return hours + (minutes / 60);
  }

  activeReportsCount = computed(() => 
    this.recentReports().filter(report => 
      report.status === 'REPORTED' || 
      report.status === 'ACKNOWLEDGED' || 
      report.status === 'IN_PROGRESS'
    ).length
  );

  remainingServiceHours = computed(() => {
    const metrics = this.usageMetrics();
    if (!metrics) return null;
    const interval = this.serviceIntervalHours();
    const remainder = metrics.engineHours % interval;
    return Math.max(interval - remainder, 0);
  });
  
  ngOnInit() {
    this.loadData();
    this.initializeUsageLogForm();
  }
  
  private initializeUsageLogForm() {
    const currentDate = new Date();
    const machine = this.assignedMachine();
    
    this.usageLogForm = this.fb.group({
      selectedMachineId: [machine?.id || '', [Validators.required]],
      logDate: [currentDate, [Validators.required]],
      engineHours: ['', [Validators.required, Validators.pattern('^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$')]],
      idleHours: ['', [Validators.required, Validators.pattern('^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$')]],
      workingHours: ['', [Validators.required, Validators.pattern('^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$')]],
      fuelConsumed: ['', [Validators.min(0)]],
      hasDowntime: [false],
      downtimeHours: [''],
      breakdownDescription: [''],
      remarks: ['']
    });
    
    // Add conditional validators for downtime fields
    this.usageLogForm.get('hasDowntime')?.valueChanges.subscribe(hasDowntime => {
      const downtimeHoursControl = this.usageLogForm.get('downtimeHours');
      const breakdownDescControl = this.usageLogForm.get('breakdownDescription');
      
      if (hasDowntime) {
        downtimeHoursControl?.setValidators([Validators.required, Validators.pattern('^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$')]);
        breakdownDescControl?.setValidators([Validators.required, Validators.minLength(10)]);
      } else {
        downtimeHoursControl?.clearValidators();
        breakdownDescControl?.clearValidators();
      }
      
      downtimeHoursControl?.updateValueAndValidity();
      breakdownDescControl?.updateValueAndValidity();
    });
  }
  
  refreshData() {
    this.loadData();
  }
  
  private loadData() {
    this.isLoading.set(true);
    this.error.set(null);
    
    const currentUser = this.authService.getCurrentUser();
    if (!currentUser?.id) {
      this.error.set('User information not available. Please log in again.');
      this.isLoading.set(false);
      return;
    }
    
    // Load assigned machine
    this.maintenanceReportService.getOperatorMachine(currentUser.id).subscribe({
      next: (machine) => {
        this.assignedMachine.set(machine);
        if (machine?.id) {
          this.loadUsageMetrics(machine.id);
        }
        this.loadReports(currentUser.id);
      },
      error: (err) => {
        console.error('Failed to load operator machine:', err);
        if (err.status === 404) {
          this.error.set('You do not have any machine assigned to you.');
        } else {
          this.error.set('Failed to load your assigned machine. Please try again.');
        }
        this.isLoading.set(false);
      }
    });
  }

  private loadUsageMetrics(machineId: string) {
    this.maintenanceService.getUsageMetrics(machineId).subscribe({
      next: (metricsArr) => {
        const m = metricsArr && metricsArr.length ? metricsArr[0] : null;
        if (m) this.usageMetrics.set(m);
        // Try to load interval config (optional); fallback to default 500h if fails
        this.maintenanceService.getServiceIntervalConfigs().subscribe({
          next: (configs) => {
            // Without machineType, keep default; could enhance by matching model keywords
            const defaultInterval = 500;
            this.serviceIntervalHours.set(defaultInterval);
          },
          error: () => {
            this.serviceIntervalHours.set(500);
          }
        });
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load usage metrics:', err);
        this.usageMetrics.set(null);
        this.isLoading.set(false);
      }
    });
  }
  
  private loadReports(operatorId: number | string) {
    this.maintenanceReportService.getOperatorReports(operatorId).subscribe({
      next: (reports) => {
        // Sort by date, newest first
        const sortedReports = reports.sort((a, b) => 
          new Date(b.reportedAt).getTime() - new Date(a.reportedAt).getTime()
        );
        this.recentReports.set(sortedReports);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load reports:', err);
        this.error.set('Failed to load maintenance reports. Please try again.');
        this.isLoading.set(false);
      }
    });
  }
  
  submitLogUsage() {
    if (!this.assignedMachine()) return;
    const m = this.usageMetrics();
    const updated: UsageMetrics = {
      machineId: this.assignedMachine()!.id,
      engineHours: (m?.engineHours || 0) + (this.engineDelta || 0),
      idleHours: (m?.idleHours || 0) + (this.idleDelta || 0),
      serviceHours: (m?.serviceHours || 0) + (this.serviceDelta || 0),
      lastUpdated: new Date()
    };
    this.usageMetrics.set(updated);

    // persist locally
    try {
      const key = `machine_usage_logs_${updated.machineId}`;
      const existing = localStorage.getItem(key);
      const arr = existing ? JSON.parse(existing) : [];
      arr.push({
        engineDelta: this.engineDelta,
        idleDelta: this.idleDelta,
        serviceDelta: this.serviceDelta,
        timestamp: new Date().toISOString()
      });
      localStorage.setItem(key, JSON.stringify(arr));
    } catch {}

    this.dialog.closeAll();
    this.engineDelta = 0;
    this.idleDelta = 0;
    this.serviceDelta = 0;
    this.snackBar.open('Usage logged successfully', 'OK', { duration: 2500 });
  }
  
  reportIssue() {
    if (!this.assignedMachine()) return;

    const dialogRef = this.dialog.open(MachineReportDialogComponent, {
      width: '600px',
      data: { machine: this.assignedMachine() }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.refreshData();
      }
    });
  }

  viewReportDetails(report: ProblemReport) {
    // Open report details dialog or navigate to details page
    // This would be implemented in a real application
    console.log('View report details:', report);
  }
  
  getMachineStatusClass(status: string): string {
    switch (status) {
      case 'OPERATIONAL': return 'status-operational';
      case 'UNDER_MAINTENANCE': return 'status-under-maintenance';
      case 'DOWN_FOR_REPAIR': return 'status-down-for-repair';
      case 'OFFLINE': return 'status-offline';
      default: return '';
    }
  }
  
  getMachineStatusLabel(status: string): string {
    switch (status) {
      case 'OPERATIONAL': return 'Operational';
      case 'UNDER_MAINTENANCE': return 'Under Maintenance';
      case 'DOWN_FOR_REPAIR': return 'Down for Repair';
      case 'OFFLINE': return 'Offline';
      default: return status;
    }
  }
  
  getReportStatusClass(status: string): string {
    switch (status) {
      case 'REPORTED': return 'status-reported';
      case 'ACKNOWLEDGED': return 'status-acknowledged';
      case 'IN_PROGRESS': return 'status-in-progress';
      case 'RESOLVED': return 'status-resolved';
      case 'CLOSED': return 'status-closed';
      default: return '';
    }
  }
  
  getReportStatusLabel(status: string): string {
    switch (status) {
      case 'REPORTED': return 'Reported';
      case 'ACKNOWLEDGED': return 'Acknowledged';
      case 'IN_PROGRESS': return 'In Progress';
      case 'RESOLVED': return 'Resolved';
      case 'CLOSED': return 'Closed';
      default: return status;
    }
  }
  
  getProblemCategoryLabel(category: string): string {
    switch (category) {
      case 'ENGINE_ISSUES': return 'Engine Issues';
      case 'HYDRAULIC_PROBLEMS': return 'Hydraulic Problems';
      case 'ELECTRICAL_FAULTS': return 'Electrical Faults';
      case 'MECHANICAL_BREAKDOWN': return 'Mechanical Breakdown';
      case 'DRILL_BIT_ISSUES': return 'Drill Bit Issues';
      case 'DRILL_ROD_PROBLEMS': return 'Drill Rod Problems';
      case 'OTHER': return 'Other Issue';
      default: return category;
    }
  }
  
  formatDate(date: Date | string): string {
    const d = new Date(date);
    return d.toLocaleDateString() + ' ' + d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }
}
