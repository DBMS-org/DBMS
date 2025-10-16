import { Component, OnInit, signal, inject, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { DatePipe, NgFor, NgIf } from '@angular/common';

import { MaintenanceReportService } from './services/maintenance-report.service';
import { 
  ProblemReport, 
  OperatorMachine, 
  CreateProblemReportRequest 
} from './models/maintenance-report.models';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-maintenance-reports',
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    FormsModule,
    MatTableModule,
    MatSnackBarModule,
    DatePipe,
    NgIf,
    NgFor
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="maintenance-reports-container">
      <header class="page-header">
        <h1>My Reports</h1>
        <button 
          mat-raised-button 
          color="primary" 
          class="submit-report-btn" 
          (click)="toggleReportForm()"
          [attr.aria-expanded]="showReportForm()"
          aria-controls="report-form">
          <mat-icon>{{ showReportForm() ? 'close' : 'add' }}</mat-icon>
          {{ showReportForm() ? 'Cancel' : 'Submit Report' }}
        </button>
      </header>

      @if (isLoading()) {
        <div class="loading-container">
          <mat-spinner diameter="40"></mat-spinner>
          <p>Loading reports...</p>
        </div>
      }

      <!-- Submit Report Form -->
      @if (showReportForm()) {
        <mat-card class="submit-form-card" id="report-form">
          <mat-card-header>
            <mat-card-title>Submit Maintenance Report</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <form (ngSubmit)="submitReport()" #reportForm="ngForm">
              <div class="form-row">
                <mat-form-field appearance="outline" class="full-width">
                  <mat-label>Problem Description</mat-label>
                  <textarea matInput [(ngModel)]="newReport.customDescription" name="description" required rows="3"></textarea>
                </mat-form-field>
              </div>
              
              <div class="form-row">
                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Affected Part</mat-label>
                  <select matNativeControl [(ngModel)]="newReport.affectedPart" name="affectedPart" required>
                    <option value="DRILL_BIT">Drill Bit</option>
                    <option value="DRILL_ROD">Drill Rod</option>
                    <option value="SHANK">Shank</option>
                    <option value="ENGINE">Engine</option>
                    <option value="HYDRAULIC_SYSTEM">Hydraulic System</option>
                    <option value="ELECTRICAL_SYSTEM">Electrical System</option>
                    <option value="MECHANICAL_COMPONENTS">Mechanical Components</option>
                    <option value="OTHER">Other</option>
                  </select>
                </mat-form-field>
                
                <mat-form-field appearance="outline" class="half-width">
                  <mat-label>Problem Category</mat-label>
                  <select matNativeControl [(ngModel)]="newReport.problemCategory" name="problemCategory" required>
                    <option value="ENGINE_ISSUES">Engine Issues</option>
                    <option value="HYDRAULIC_PROBLEMS">Hydraulic Problems</option>
                    <option value="ELECTRICAL_FAULTS">Electrical Faults</option>
                    <option value="MECHANICAL_BREAKDOWN">Mechanical Breakdown</option>
                    <option value="DRILL_BIT_ISSUES">Drill Bit Issues</option>
                    <option value="DRILL_ROD_PROBLEMS">Drill Rod Problems</option>
                    <option value="OTHER">Other</option>
                  </select>
                </mat-form-field>
              </div>
              
              <div class="form-row">
                <mat-form-field appearance="outline" class="full-width">
                  <mat-label>Severity Level</mat-label>
                  <select matNativeControl [(ngModel)]="newReport.severity" name="severity" required>
                    <option value="LOW">Low - Maintenance Needed</option>
                    <option value="MEDIUM">Medium - Minor Issues</option>
                    <option value="HIGH">High - Performance Issues</option>
                    <option value="CRITICAL">Critical - Machine Down</option>
                  </select>
                </mat-form-field>
              </div>
              
              <div class="form-actions">
                <button type="button" mat-button (click)="toggleReportForm()">Cancel</button>
                <button type="submit" mat-raised-button color="primary" 
                       [disabled]="!reportForm.form.valid">Submit Report</button>
              </div>
            </form>
          </mat-card-content>
        </mat-card>
      }

      <!-- Reports Summary -->
      <div class="reports-summary">
        <div class="stat-cards">
          <mat-card class="stat-card pending">
            <mat-card-content>
              <div class="stat-item">
                <span class="stat-number">{{ pendingCount() }}</span>
                <span class="stat-label">Pending</span>
              </div>
            </mat-card-content>
          </mat-card>
          
          <mat-card class="stat-card in-progress">
            <mat-card-content>
              <div class="stat-item">
                <span class="stat-number">{{ inProgressCount() }}</span>
                <span class="stat-label">In Progress</span>
              </div>
            </mat-card-content>
          </mat-card>
          
          <mat-card class="stat-card completed">
            <mat-card-content>
              <div class="stat-item">
                <span class="stat-number">{{ completedCount() }}</span>
                <span class="stat-label">Completed</span>
              </div>
            </mat-card-content>
          </mat-card>
        </div>
      </div>

      <!-- Report History -->
      <mat-card class="report-history-card">
        <mat-card-header>
          <mat-card-title>Report History</mat-card-title>
          <mat-card-subtitle>{{ reports().length }} total reports</mat-card-subtitle>
        </mat-card-header>
        <mat-card-content>
          @if (reports().length === 0) {
            <div class="empty-state">
              <mat-icon>assignment</mat-icon>
              <p>No reports submitted yet</p>
              <small>Click "Submit Report" to create your first maintenance report</small>
            </div>
          } @else {
            <div class="table-container">
              <table mat-table [dataSource]="reports()" class="reports-table">
                  <ng-container matColumnDef="title">
                    <th mat-header-cell *matHeaderCellDef>Problem Description</th>
                    <td mat-cell *matCellDef="let report">
                      <div class="description-cell">
                        <span class="description-text">{{ report.customDescription | slice:0:50 }}{{ report.customDescription.length > 50 ? '...' : '' }}</span>
                        <small class="ticket-id">{{ report.ticketId }}</small>
                      </div>
                    </td>
                  </ng-container>
                  
                  <ng-container matColumnDef="affectedPart">
                    <th mat-header-cell *matHeaderCellDef>Affected Part</th>
                    <td mat-cell *matCellDef="let report">
                      <div class="part-cell">
                        <mat-icon class="part-icon">{{ getPartIcon(report.affectedPart) }}</mat-icon>
                        <span>{{ getPartDisplay(report.affectedPart) }}</span>
                      </div>
                    </td>
                  </ng-container>
                  
                  <ng-container matColumnDef="severity">
                    <th mat-header-cell *matHeaderCellDef>Severity</th>
                    <td mat-cell *matCellDef="let report">
                      <span class="severity-badge" [ngClass]="getSeverityClass(report.severity)">
                        <mat-icon class="severity-icon">{{ getSeverityIcon(report.severity) }}</mat-icon>
                        {{ getSeverityDisplay(report.severity) }}
                      </span>
                    </td>
                  </ng-container>
                  
                  <ng-container matColumnDef="status">
                    <th mat-header-cell *matHeaderCellDef>Status</th>
                    <td mat-cell *matCellDef="let report">
                      <span class="status-badge" [ngClass]="getStatusClass(report.status)">
                        <mat-icon class="status-icon">{{ getStatusIcon(report.status) }}</mat-icon>
                        {{ getStatusDisplay(report.status) }}
                      </span>
                    </td>
                  </ng-container>
                  
                  <ng-container matColumnDef="reportedAt">
                    <th mat-header-cell *matHeaderCellDef>Reported</th>
                    <td mat-cell *matCellDef="let report">
                      <div class="date-cell">
                        <span class="date">{{ report.reportedAt | date:'MMM d, y' }}</span>
                        <small class="time">{{ report.reportedAt | date:'h:mm a' }}</small>
                      </div>
                    </td>
                  </ng-container>
                  
                  <ng-container matColumnDef="actions">
                    <th mat-header-cell *matHeaderCellDef>Actions</th>
                    <td mat-cell *matCellDef="let report">
                      <button mat-icon-button 
                              (click)="viewReportDetails(report)"
                              matTooltip="View Details"
                              class="action-button">
                        <mat-icon>visibility</mat-icon>
                      </button>
                    </td>
                  </ng-container>
                  
                  <tr mat-header-row *matHeaderRowDef="reportColumns"></tr>
                  <tr mat-row *matRowDef="let row; columns: reportColumns;" class="report-row"></tr>
                </table>
            </div>
          }
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .maintenance-reports-container {
      padding: 1rem;
      max-width: 1200px;
      margin: 0 auto;
    }

    .page-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 1.5rem;
    }

    .page-header h1 {
      margin: 0;
      color: #333;
      font-weight: 500;
    }

    .submit-report-btn {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }

    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      padding: 2rem;
      gap: 1rem;
    }

    .submit-form-card {
      margin-bottom: 1.5rem;
      border-left: 4px solid #2196f3;
    }

    .form-row {
      display: flex;
      gap: 1rem;
      margin-bottom: 1rem;
    }

    .full-width {
      width: 100%;
    }

    .half-width {
      flex: 1;
    }

    .form-actions {
      display: flex;
      justify-content: flex-end;
      gap: 1rem;
      margin-top: 1.5rem;
    }

    .reports-summary {
      margin-bottom: 1.5rem;
    }

    .stat-cards {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
      gap: 1rem;
    }

    .stat-card {
      text-align: center;
      border-left: 4px solid;
    }

    .stat-card.pending {
      border-left-color: #ff9800;
    }

    .stat-card.in-progress {
      border-left-color: #2196f3;
    }

    .stat-card.completed {
      border-left-color: #4caf50;
    }

    .stat-item {
      display: flex;
      flex-direction: column;
      align-items: center;
    }

    .stat-number {
      font-size: 2rem;
      font-weight: bold;
      color: #333;
    }

    .stat-label {
      font-size: 0.875rem;
      color: #666;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }

    .report-history-card {
      border-left: 4px solid #4caf50;
    }

    .empty-state {
      text-align: center;
      padding: 3rem 1rem;
      color: #666;
    }

    .empty-state mat-icon {
      font-size: 3rem;
      width: 3rem;
      height: 3rem;
      color: #ccc;
      margin-bottom: 1rem;
    }

    .table-container {
      overflow-x: auto;
      border: 1px solid #e0e0e0;
      border-radius: 4px;
      background: white;
    }

    .reports-table {
      width: 100%;
      min-width: 800px;

      th {
        background-color: #f5f5f5;
        font-weight: 600;
        color: #333;
        padding: 16px;
        border-bottom: 2px solid #e0e0e0;
      }

      td {
        padding: 16px;
        color: #555;
        border-bottom: 1px solid #f0f0f0;
      }

      tr:last-child td {
        border-bottom: none;
      }
    }

    .category-badge,
    .priority-badge,
    .status-badge {
      padding: 0.25rem 0.5rem;
      border-radius: 12px;
      font-size: 0.75rem;
      font-weight: 500;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }

    .category-badge {
      background-color: #e3f2fd;
      color: #1976d2;
    }

    .severity-low {
      background-color: #e8f5e8;
      color: #2e7d32;
      border: 1px solid #4caf50;
    }

    .severity-medium {
      background-color: #fffde7;
      color: #f57f17;
      border: 1px solid #ffeb3b;
    }

    .severity-high {
      background-color: #fff3e0;
      color: #ef6c00;
      border: 1px solid #ff9800;
    }

    .severity-critical {
      background-color: #ffebee;
      color: #c62828;
      border: 1px solid #ef5350;
    }

    /* Enhanced table styling */
    .reports-table {
      width: 100%;
      background: white;
      border-radius: 8px;
      overflow: hidden;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    }

    .mat-mdc-header-cell {
      background-color: #f8f9fa;
      font-weight: 600;
      color: #495057;
      border-bottom: 2px solid #e9ecef;
    }

    .report-row {
      transition: background-color 0.2s;

      &:hover {
        background-color: #f9f9f9;
      }
    }

    .description-cell {
      display: flex;
      flex-direction: column;
      gap: 4px;
    }

    .description-text {
      font-weight: 500;
      color: #212529;
    }

    .ticket-id {
      color: #6c757d;
      font-size: 0.75rem;
      font-weight: 400;
    }

    .part-cell {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .part-icon {
      color: #6c757d;
      font-size: 18px;
      width: 18px;
      height: 18px;
    }

    .date-cell {
      display: flex;
      flex-direction: column;
      gap: 2px;
    }

    .date {
      font-weight: 500;
      color: #333;
    }

    .time {
      font-size: 12px;
      color: #666;
    }

    .action-button {
      color: #007bff;
      transition: all 0.2s ease;
    }

    .action-button:hover {
      background-color: rgba(0, 123, 255, 0.1);
      transform: scale(1.05);
    }

    /* Enhanced badge styling */
    .severity-badge, .status-badge {
      display: inline-flex;
      align-items: center;
      gap: 4px;
      padding: 0 12px;
      min-height: 24px;
      height: 24px;
      border-radius: 12px;
      font-size: 12px;
      font-weight: 500;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }

    .severity-icon, .status-icon {
      font-size: 14px;
      width: 14px;
      height: 14px;
    }

    /* Severity badge colors */
    .severity-critical {
      background-color: #dc3545;
      color: white;
    }

    .severity-high {
      background-color: #fd7e14;
      color: white;
    }

    .severity-medium {
      background-color: #ffc107;
      color: #212529;
    }

    .severity-low {
      background-color: #28a745;
      color: white;
    }

    /* Status badge colors */
    .status-reported {
      background-color: #6c757d;
      color: white;
    }

    .status-acknowledged {
      background-color: #17a2b8;
      color: white;
    }

    .status-in_progress {
      background-color: #007bff;
      color: white;
    }

    .status-resolved {
      background-color: #28a745;
      color: white;
    }

    .status-closed {
      background-color: #6f42c1;
      color: white;
    }

    @media (max-width: 768px) {
      .maintenance-reports-container {
        padding: 0.75rem;
      }

      .page-header {
        flex-direction: column;
        align-items: stretch;
        gap: 1rem;
      }

      .form-row {
        flex-direction: column;
      }

      .half-width {
        width: 100%;
      }

      .stat-cards {
        grid-template-columns: 1fr;
      }

      .submit-report-btn { 
        width: 100%; 
      }
    }
  `]

})
export class MaintenanceReportsComponent implements OnInit {
  showReportForm = signal(false);
  operatorMachine = signal<OperatorMachine | null>(null);
  reports = signal<ProblemReport[]>([]);
  isLoading = signal(false);
  reportColumns = ['title', 'affectedPart', 'severity', 'status', 'reportedAt', 'actions'];
  
  newReport = {
    affectedPart: 'OTHER' as any,
    problemCategory: 'OTHER' as any,
    customDescription: '',
    symptoms: [] as string[],
    severity: 'MEDIUM' as any
  };
  
  private maintenanceReportService = inject(MaintenanceReportService);
  private authService = inject(AuthService);
  private snackBar = inject(MatSnackBar);

  // Status tracking computed properties
  pendingCount = computed(() => 
    this.reports().filter(report => 
      report.status === 'REPORTED'
    ).length
  );

  inProgressCount = computed(() => 
    this.reports().filter(report => 
      report.status === 'ACKNOWLEDGED' || report.status === 'IN_PROGRESS'
    ).length
  );

  completedCount = computed(() => 
    this.reports().filter(report => 
      report.status === 'RESOLVED' || report.status === 'CLOSED'
    ).length
  );
  
  ngOnInit() {
    this.loadOperatorMachine();
    this.loadReports();
  }
  
  private async loadOperatorMachine() {
    try {
      this.isLoading.set(true);
      const currentUser = this.authService.getCurrentUser();
      if (currentUser?.id) {
        this.maintenanceReportService.getOperatorMachine(currentUser.id).subscribe({
          next: (machine) => this.operatorMachine.set(machine),
          error: (error) => console.error('Failed to load operator machine:', error)
        });
      }
    } catch (error) {
      console.error('Error loading operator machine:', error);
    } finally {
      this.isLoading.set(false);
    }
  }
  
  private async loadReports() {
    try {
      const currentUser = this.authService.getCurrentUser();
      if (currentUser?.id) {
        this.maintenanceReportService.getOperatorReports(currentUser.id).subscribe({
          next: (reports) => this.reports.set(reports),
          error: (error) => console.error('Failed to load reports:', error)
        });
      }
    } catch (error) {
      console.error('Error loading reports:', error);
    }
  }
  
  toggleReportForm() {
    this.showReportForm.update(show => !show);
  }
  
  async submitReport() {
    const currentUser = this.authService.getCurrentUser();
    const machine = this.operatorMachine();
    
    if (!currentUser || !machine) {
      this.snackBar.open('Unable to submit report. Please try again.', 'Dismiss', { duration: 3000 });
      return;
    }

    const reportData: CreateProblemReportRequest = {
      machineId: machine.id,
      machineName: machine.name,
      machineModel: machine.model,
      serialNumber: machine.serialNumber,
      location: machine.currentLocation,
      affectedPart: this.newReport.affectedPart,
      problemCategory: this.newReport.problemCategory,
      customDescription: this.newReport.customDescription,
      symptoms: this.newReport.symptoms,
      severity: this.newReport.severity
    };

    try {
      const report = await this.maintenanceReportService.submitProblemReport(reportData);
      this.reports.update(reports => [report, ...reports]);
      this.snackBar.open('Report submitted successfully', 'OK', { duration: 2500 });
      this.resetReportForm();
      this.showReportForm.set(false);
    } catch (error: any) {
      console.error('Failed to submit report:', error);
      this.snackBar.open('Failed to submit report. Please try again.', 'Dismiss', { duration: 3000 });
    }
  }

  resetReportForm() {
    this.newReport = {
      affectedPart: 'OTHER' as any,
      problemCategory: 'OTHER' as any,
      customDescription: '',
      symptoms: [] as string[],
      severity: 'MEDIUM' as any
    };
  }

  getPartDisplay(part: string): string {
    const partMap: { [key: string]: string } = {
      'DRILL_BIT': 'Drill Bit',
      'DRILL_ROD': 'Drill Rod',
      'SHANK': 'Shank',
      'ENGINE': 'Engine',
      'HYDRAULIC_SYSTEM': 'Hydraulic System',
      'ELECTRICAL_SYSTEM': 'Electrical System',
      'MECHANICAL_COMPONENTS': 'Mechanical Components',
      'OTHER': 'Other'
    };
    return partMap[part] || part;
  }

  getPartIcon(part: string): string {
    const iconMap: { [key: string]: string } = {
      'DRILL_BIT': 'build',
      'DRILL_ROD': 'construction',
      'SHANK': 'hardware',
      'ENGINE': 'settings',
      'HYDRAULIC_SYSTEM': 'water_drop',
      'ELECTRICAL_SYSTEM': 'electrical_services',
      'MECHANICAL_COMPONENTS': 'precision_manufacturing',
      'OTHER': 'help_outline'
    };
    return iconMap[part] || 'help_outline';
  }

  getSeverityDisplay(severity: string): string {
    const severityMap: { [key: string]: string } = {
      'LOW': 'Low',
      'MEDIUM': 'Medium',
      'HIGH': 'High',
      'CRITICAL': 'Critical'
    };
    return severityMap[severity] || severity;
  }

  getSeverityClass(severity: string): string {
    return `severity-${severity.toLowerCase()}`;
  }

  getSeverityIcon(severity: string): string {
    const iconMap: { [key: string]: string } = {
      'LOW': 'info',
      'MEDIUM': 'warning',
      'HIGH': 'error',
      'CRITICAL': 'dangerous'
    };
    return iconMap[severity] || 'info';
  }

  getStatusIcon(status: string): string {
    const iconMap: { [key: string]: string } = {
      'REPORTED': 'send',
      'ACKNOWLEDGED': 'visibility',
      'IN_PROGRESS': 'build',
      'RESOLVED': 'check_circle',
      'CLOSED': 'done_all'
    };
    return iconMap[status] || 'help_outline';
  }

  getStatusDisplay(status: string): string {
    switch (status) {
      case 'REPORTED':
        return 'Pending';
      case 'ACKNOWLEDGED':
      case 'IN_PROGRESS':
        return 'In Progress';
      case 'RESOLVED':
      case 'CLOSED':
        return 'Completed';
      default:
        return status;
    }
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'REPORTED':
        return 'pending';
      case 'ACKNOWLEDGED':
      case 'IN_PROGRESS':
        return 'in-progress';
      case 'RESOLVED':
      case 'CLOSED':
        return 'completed';
      default:
        return 'pending';
    }
  }

  viewReportDetails(report: ProblemReport) {
    // This could open a dialog or navigate to a details page
    console.log('View report details:', report);
    this.snackBar.open(`Viewing details for: ${report.customDescription}`, 'OK', { duration: 2000 });
  }
}