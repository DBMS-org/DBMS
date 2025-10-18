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
      <!-- Page Header -->
      <header class="page-header">
        <div class="header-content">
          <div class="header-title">
            <mat-icon class="header-icon">description</mat-icon>
            <div class="title-text">
              <h1>Maintenance Reports</h1>
              <p class="subtitle">Submit and track machine maintenance reports</p>
            </div>
          </div>
          <div class="header-actions">
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
          </div>
        </div>
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
    /* ============================================ */
    /* OPERATOR MAINTENANCE REPORTS - PROFESSIONAL DESIGN */
    /* ============================================ */

    .maintenance-reports-container {
      min-height: 100vh;
      background: linear-gradient(135deg, #f5f7fa 0%, #e8ecf1 100%);
      padding: 2rem;
    }

    /* ============================================ */
    /* PAGE HEADER */
    /* ============================================ */

    .page-header {
      background: linear-gradient(135deg, #667eea, #764ba2);
      border-radius: 16px;
      padding: 2rem;
      margin-bottom: 2rem;
      box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
    }

    .header-content {
      display: flex;
      justify-content: space-between;
      align-items: center;
      gap: 2rem;
    }

    .header-title {
      display: flex;
      align-items: center;
      gap: 1.5rem;
    }

    .header-icon {
      width: 56px;
      height: 56px;
      font-size: 56px;
      color: rgba(255, 255, 255, 0.9);
    }

    .title-text h1 {
      margin: 0 0 0.5rem 0;
      font-size: 2rem;
      font-weight: 700;
      color: white;
      letter-spacing: -0.5px;
    }

    .subtitle {
      margin: 0;
      font-size: 1rem;
      color: rgba(255, 255, 255, 0.85);
      font-weight: 400;
    }

    .header-actions .submit-report-btn {
      padding: 0.75rem 1.5rem;
      background: rgba(255, 255, 255, 0.2);
      border: 2px solid rgba(255, 255, 255, 0.3);
      color: white;
      border-radius: 12px;
      font-weight: 600;
      font-size: 0.9rem;
      min-height: 44px;
      backdrop-filter: blur(10px);
      display: flex;
      align-items: center;
      gap: 0.5rem;
      transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
    }

    .header-actions .submit-report-btn:hover {
      background: rgba(255, 255, 255, 0.25);
      border-color: rgba(255, 255, 255, 0.5);
      transform: translateY(-2px);
      box-shadow: 0 4px 20px rgba(0, 0, 0, 0.2);
    }

    .submit-report-btn mat-icon {
      font-size: 1.2rem;
    }

    /* ============================================ */
    /* LOADING STATE */
    /* ============================================ */

    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 4rem 2rem;
      background: white;
      border-radius: 16px;
      box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
      gap: 1.5rem;
    }

    .loading-container p {
      font-size: 1.1rem;
      color: #6b7280;
      font-weight: 500;
    }

    /* ============================================ */
    /* SUBMIT FORM CARD */
    /* ============================================ */

    .submit-form-card {
      margin-bottom: 2rem;
      border-radius: 16px;
      box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
      border: 2px solid #667eea;
      overflow: hidden;
    }

    .submit-form-card ::ng-deep .mat-mdc-card-header {
      background: linear-gradient(135deg, #f8f9fa 0%, #f1f3f5 100%);
      border-bottom: 1px solid #e5e7eb;
      padding: 1.5rem;
    }

    .submit-form-card ::ng-deep .mat-mdc-card-title {
      font-size: 1.25rem;
      font-weight: 700;
      color: #1f2937;
    }

    /* ============================================ */
    /* FORM ELEMENTS */
    /* ============================================ */

    .form-row {
      display: flex;
      gap: 1rem;
      margin-bottom: 1.5rem;
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
      padding-top: 1.5rem;
      margin-top: 1.5rem;
      border-top: 1px solid #e5e7eb;
    }

    .form-actions button {
      padding: 0.75rem 1.5rem;
      border-radius: 12px;
      font-weight: 600;
      font-size: 0.9rem;
      min-height: 44px;
      transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
    }

    .form-actions button mat-icon {
      font-size: 1.2rem;
    }

    /* ============================================ */
    /* REPORTS SUMMARY */
    /* ============================================ */

    .reports-summary {
      margin-bottom: 2rem;
    }

    .stat-cards {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 1.5rem;
    }

    .stat-card {
      text-align: center;
      border-radius: 16px;
      box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
      border: 2px solid transparent;
      transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
      overflow: hidden;
    }

    .stat-card:hover {
      transform: translateY(-4px);
      box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
    }

    .stat-card.pending {
      border-color: #f59e0b;
      background: linear-gradient(135deg, #fff7ed 0%, #ffffff 100%);
    }

    .stat-card.in-progress {
      border-color: #3b82f6;
      background: linear-gradient(135deg, #dbeafe 0%, #ffffff 100%);
    }

    .stat-card.completed {
      border-color: #10b981;
      background: linear-gradient(135deg, #d1fae5 0%, #ffffff 100%);
    }

    .stat-card ::ng-deep .mat-mdc-card-content {
      padding: 2rem 1.5rem;
    }

    .stat-item {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 0.75rem;
    }

    .stat-number {
      font-size: 3rem;
      font-weight: 800;
      line-height: 1;
      letter-spacing: -1px;
    }

    .stat-card.pending .stat-number {
      color: #f59e0b;
    }

    .stat-card.in-progress .stat-number {
      color: #3b82f6;
    }

    .stat-card.completed .stat-number {
      color: #10b981;
    }

    .stat-label {
      font-size: 0.875rem;
      color: #6b7280;
      text-transform: uppercase;
      letter-spacing: 1px;
      font-weight: 700;
    }

    /* ============================================ */
    /* REPORT HISTORY CARD */
    /* ============================================ */

    .report-history-card {
      border-radius: 16px;
      box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
      border: 2px solid #667eea;
      overflow: hidden;
    }

    .report-history-card ::ng-deep .mat-mdc-card-header {
      background: linear-gradient(135deg, #f8f9fa 0%, #f1f3f5 100%);
      border-bottom: 1px solid #e5e7eb;
      padding: 1.5rem;
    }

    .report-history-card ::ng-deep .mat-mdc-card-title {
      font-size: 1.25rem;
      font-weight: 700;
      color: #1f2937;
    }

    .report-history-card ::ng-deep .mat-mdc-card-subtitle {
      color: #6b7280;
      font-weight: 500;
      margin-top: 0.25rem;
    }

    /* ============================================ */
    /* EMPTY STATE */
    /* ============================================ */

    .empty-state {
      text-align: center;
      padding: 4rem 2rem;
      background: linear-gradient(135deg, #f8f9fa 0%, #f1f3f5 100%);
      border-radius: 12px;
      border: 2px dashed #d1d5db;
    }

    .empty-state mat-icon {
      font-size: 4rem;
      width: 4rem;
      height: 4rem;
      color: #9ca3af;
      margin-bottom: 1.5rem;
    }

    .empty-state p {
      color: #4b5563;
      font-weight: 600;
      font-size: 1.125rem;
      margin-bottom: 0.5rem;
    }

    .empty-state small {
      color: #6b7280;
      font-size: 0.938rem;
    }

    /* ============================================ */
    /* TABLE CONTAINER */
    /* ============================================ */

    .table-container {
      overflow-x: auto;
      border-radius: 12px;
      background: white;
      box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1);
    }

    .reports-table {
      width: 100%;
      min-width: 900px;
    }

    .reports-table th {
      background: linear-gradient(135deg, #f8f9fa 0%, #f1f3f5 100%);
      font-weight: 700;
      color: #1f2937;
      padding: 1rem 1.5rem;
      border-bottom: 2px solid #e5e7eb;
      text-transform: uppercase;
      font-size: 0.813rem;
      letter-spacing: 0.5px;
    }

    .reports-table td {
      padding: 1.25rem 1.5rem;
      color: #374151;
      border-bottom: 1px solid #f3f4f6;
    }

    .reports-table tr:last-child td {
      border-bottom: none;
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

    /* ============================================ */
    /* RESPONSIVE DESIGN */
    /* ============================================ */

    @media (max-width: 768px) {
      .maintenance-reports-container {
        padding: 1rem;
      }

      .page-header {
        padding: 1.5rem;
      }

      .header-content {
        flex-direction: column;
        align-items: stretch;
      }

      .header-title {
        flex-direction: column;
        align-items: flex-start;
        gap: 1rem;
      }

      .header-icon {
        width: 48px;
        height: 48px;
        font-size: 48px;
      }

      .title-text h1 {
        font-size: 1.75rem;
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

      .form-actions {
        flex-direction: column;
      }

      .form-actions button {
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