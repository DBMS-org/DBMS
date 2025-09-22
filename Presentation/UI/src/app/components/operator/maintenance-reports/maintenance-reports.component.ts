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
import { ProblemReport, OperatorMachine } from './models/maintenance-report.models';
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
        <h1>Maintenance Reports</h1>
        <button 
          mat-raised-button 
          color="warn" 
          class="report-problem-btn" 
          (click)="openReportForm()"
          [disabled]="isLoading()">
          <mat-icon>report_problem</mat-icon>
          Report Problem
        </button>
      </header>
      
      <div class="reports-content">
        @if (isLoading()) {
          <div class="loading-container">
            <mat-spinner></mat-spinner>
            <p>Loading maintenance reports...</p>
          </div>
        } @else {
          <div class="reports-grid">
            <mat-card class="summary-card">
              <mat-card-header>
                <mat-card-title>Quick Actions</mat-card-title>
              </mat-card-header>
              <mat-card-content>
                <p>Report problems with your assigned machine and track resolution status.</p>
                @if (operatorMachine()) {
                  <div class="machine-info">
                    <h4>Assigned Machine</h4>
                    <p><strong>{{ operatorMachine()?.name }}</strong></p>
                    <p>Model: {{ operatorMachine()?.model }}</p>
                    <p>Location: {{ operatorMachine()?.currentLocation }}</p>
                  </div>
                }
              </mat-card-content>
            </mat-card>
            
            <mat-card class="reports-summary-card">
              <mat-card-header>
                <mat-card-title>My Reports Summary</mat-card-title>
              </mat-card-header>
              <mat-card-content>
                <div class="summary-stats">
                  <div class="stat-item">
                    <span class="stat-number">{{ reports().length }}</span>
                    <span class="stat-label">Total Reports</span>
                  </div>
                  <div class="stat-item">
                    <span class="stat-number">{{ inProgressCount() }}</span>
                    <span class="stat-label">In Progress</span>
                  </div>
                </div>
              </mat-card-content>
            </mat-card>
            
            <mat-card class="daily-log-card">
              <mat-card-header>
                <mat-card-title>Daily Log</mat-card-title>
              </mat-card-header>
              <mat-card-content>
                <form class="daily-log-form" (ngSubmit)="submitDailyLog()" #dailyLogForm="ngForm">
                  <div class="form-row">
                    <mat-form-field appearance="outline">
                      <mat-label>Date</mat-label>
                      <input matInput [matDatepicker]="picker" [(ngModel)]="dailyLog.date" name="date" required>
                      <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
                      <mat-datepicker #picker></mat-datepicker>
                    </mat-form-field>
                  </div>
                  <div class="form-row">
                    <mat-form-field appearance="outline" class="form-col">
                      <mat-label>Engine Hours (today)</mat-label>
                      <input matInput type="number" min="0" [(ngModel)]="dailyLog.engineHours" name="engineHours" required>
                    </mat-form-field>
                    <mat-form-field appearance="outline" class="form-col">
                      <mat-label>Idle Hours (today)</mat-label>
                      <input matInput type="number" min="0" [(ngModel)]="dailyLog.idleHours" name="idleHours" required>
                    </mat-form-field>
                    <mat-form-field appearance="outline" class="form-col">
                      <mat-label>Service Hours (today)</mat-label>
                      <input matInput type="number" min="0" [(ngModel)]="dailyLog.serviceHours" name="serviceHours" required>
                    </mat-form-field>
                  </div>
                  <div class="form-row">
                    <mat-form-field appearance="outline" class="full-width">
                      <mat-label>Drilling Progress Summary</mat-label>
                      <input matInput [(ngModel)]="dailyLog.drillingSummary" name="drillingSummary" placeholder="e.g., 45/60 holes drilled, avg depth 7.5m">
                    </mat-form-field>
                  </div>
                  <div class="form-row">
                    <mat-form-field appearance="outline" class="full-width">
                      <mat-label>Issues / Notes</mat-label>
                      <textarea matInput rows="3" [(ngModel)]="dailyLog.notes" name="notes"></textarea>
                    </mat-form-field>
                  </div>
                  <div class="actions-row">
                    <button mat-stroked-button type="button" (click)="resetDailyLog()">Reset</button>
                    <button mat-flat-button color="primary" type="submit" [disabled]="!dailyLogForm.form.valid || !operatorMachine()">Submit</button>
                  </div>
                </form>
              </mat-card-content>
            </mat-card>

            <mat-card class="past-logs-card">
              <mat-card-header>
                <mat-card-title>Past Daily Logs</mat-card-title>
              </mat-card-header>
              <mat-card-content>
                @if (pastLogs().length === 0) {
                  <p class="no-logs">No logs submitted yet.</p>
                } @else {
                  <div class="table-wrapper">
                    <table mat-table [dataSource]="pastLogs()" class="mat-elevation-z1 dense-table">
                      <ng-container matColumnDef="date">
                        <th mat-header-cell *matHeaderCellDef>Date</th>
                        <td mat-cell *matCellDef="let log">{{ log.date | date:'mediumDate' }}</td>
                      </ng-container>
                      <ng-container matColumnDef="engine">
                        <th mat-header-cell *matHeaderCellDef>Engine</th>
                        <td mat-cell *matCellDef="let log">{{ log.engineHours }}</td>
                      </ng-container>
                      <ng-container matColumnDef="idle">
                        <th mat-header-cell *matHeaderCellDef>Idle</th>
                        <td mat-cell *matCellDef="let log">{{ log.idleHours }}</td>
                      </ng-container>
                      <ng-container matColumnDef="service">
                        <th mat-header-cell *matHeaderCellDef>Service</th>
                        <td mat-cell *matCellDef="let log">{{ log.serviceHours }}</td>
                      </ng-container>
                      <ng-container matColumnDef="drilling">
                        <th mat-header-cell *matHeaderCellDef>Drilling</th>
                        <td mat-cell *matCellDef="let log">{{ log.drillingSummary || '—' }}</td>
                      </ng-container>
                      <ng-container matColumnDef="notes">
                        <th mat-header-cell *matHeaderCellDef>Notes</th>
                        <td mat-cell *matCellDef="let log">{{ log.notes || '—' }}</td>
                      </ng-container>

                      <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
                      <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
                    </table>
                  </div>
                }
              </mat-card-content>
            </mat-card>
          </div>
        }
      </div>
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
      margin-bottom: 2rem;
    }

    .page-header h1 {
      margin: 0;
      color: #333;
    }

    .report-problem-btn {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }

    .reports-content { margin-top: 0.5rem; padding-bottom: 2rem; }

    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 2rem;
      text-align: center;
    }

    .reports-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
      gap: 1.25rem;
      align-items: start;
    }

    .summary-card, .reports-summary-card, .daily-log-card, .past-logs-card {
      overflow: hidden;
      border-radius: 10px;
      transition: transform 0.15s ease, box-shadow 0.2s ease;
    }
    .summary-card:hover, .reports-summary-card:hover, .daily-log-card:hover, .past-logs-card:hover {
      transform: translateY(-1px);
      box-shadow: 0 6px 16px rgba(0,0,0,0.08);
    }

    .daily-log-form .form-row {
      display: flex;
      gap: 1rem;
      flex-wrap: wrap;
      margin-bottom: 1rem;
    }

    .daily-log-form .form-col {
      flex: 1 1 200px;
    }

    .full-width { width: 100%; }

    .actions-row {
      display: flex;
      justify-content: flex-end;
      gap: 0.75rem;
      margin-top: 0.5rem;
    }

    .table-wrapper {
      overflow: auto;
      border: 1px solid #eee;
      border-radius: 8px;
      max-height: 420px;
      background: #fff;
      box-shadow: inset 0 1px 0 rgba(0,0,0,0.02);
    }

    /* Densify and improve table readability */
    table { width: 100%; }
    .dense-table .mat-mdc-header-cell, .dense-table .mat-mdc-cell {
      padding: 8px 12px;
      font-size: 0.9rem;
    }
    .dense-table .mat-mdc-header-cell {
      position: sticky;
      top: 0;
      z-index: 2;
      background: #fafafa;
      border-bottom: 1px solid #e0e0e0;
    }
    .dense-table .mat-mdc-row:nth-child(even) .mat-mdc-cell { background: #fcfcfc; }
    .dense-table .mat-mdc-row:hover .mat-mdc-cell { background: #f5faff; }

    .no-logs { color: #777; font-style: italic; }
    .summary-card, .reports-summary-card {
      height: fit-content;
    }

    .machine-info {
      margin-top: 1rem;
      padding: 1rem;
      background-color: #f5f5f5;
      border-radius: 4px;
    }

    .machine-info h4 {
      margin: 0 0 0.5rem 0;
      color: #666;
    }

    .machine-info p {
      margin: 0.25rem 0;
    }

    .summary-stats {
      display: flex;
      gap: 2rem;
    }

    .stat-item {
      display: flex;
      flex-direction: column;
      align-items: center;
    }

    .stat-number {
      font-size: 2rem;
      font-weight: bold;
      color: #1976d2;
    }

    .stat-label {
      font-size: 0.875rem;
      color: #666;
    }

    @media (max-width: 768px) {
      .page-header {
        flex-direction: column;
        gap: 1rem;
        align-items: stretch;
      }

      .reports-grid {
        grid-template-columns: 1fr;
      }

      .summary-stats {
        justify-content: space-around;
      }

      .report-problem-btn { width: 100%; }
      .dense-table .mat-mdc-header-cell, .dense-table .mat-mdc-cell {
        padding: 8px;
        font-size: 0.85rem;
      }
      .maintenance-reports-container { padding: 0.75rem; }
    }
  `]
})
export class MaintenanceReportsComponent implements OnInit {
  showReportForm = signal(false);
  operatorMachine = signal<OperatorMachine | null>(null);
  reports = signal<ProblemReport[]>([]);
  isLoading = signal(false);
  pastLogs = signal<{ date: Date; engineHours: number; idleHours: number; serviceHours: number; drillingSummary?: string; notes?: string; }[]>([]);
  displayedColumns = ['date','engine','idle','service','drilling','notes'];
  dailyLog = {
    date: new Date(),
    engineHours: 0,
    idleHours: 0,
    serviceHours: 0,
    drillingSummary: '',
    notes: ''
  };
  
  private maintenanceReportService = inject(MaintenanceReportService);
  private authService = inject(AuthService);
  private snackBar = inject(MatSnackBar);

  inProgressCount = computed(() => 
    this.reports().filter(report => 
      report.status === 'IN_PROGRESS' || report.status === 'ACKNOWLEDGED'
    ).length
  );
  
  ngOnInit() {
    this.loadOperatorMachine();
    this.loadReports();
    this.loadPastLogs();
  }
  
  private loadPastLogs() {
    const user = this.authService.getCurrentUser();
    const machineId = this.operatorMachine()?.id || 'unknown';
    try {
      const key = `operator_daily_logs_${user?.id}_${machineId}`;
      const stored = localStorage.getItem(key);
      const arr = stored ? JSON.parse(stored) : [];
      this.pastLogs.set(arr.map((x: any) => ({ ...x, date: new Date(x.date) })));
    } catch {
      this.pastLogs.set([]);
    }
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
  
  openReportForm() {
    this.showReportForm.set(true);
  }
  
  closeReportForm() {
    this.showReportForm.set(false);
  }
  
  onReportSubmitted(report: ProblemReport) {
    this.reports.update(reports => [report, ...reports]);
    this.closeReportForm();
  }

  submitDailyLog() {
    const user = this.authService.getCurrentUser();
    const machineId = this.operatorMachine()?.id || 'unknown';
    const key = `operator_daily_logs_${user?.id}_${machineId}`;
    const newLog = { ...this.dailyLog };
    try {
      const stored = localStorage.getItem(key);
      const arr = stored ? JSON.parse(stored) : [];
      arr.unshift(newLog);
      localStorage.setItem(key, JSON.stringify(arr));
      this.pastLogs.set([{ ...newLog }, ...this.pastLogs() ]);
      this.snackBar.open('Daily log submitted', 'OK', { duration: 2500 });
      this.resetDailyLog();
    } catch (e) {
      console.error(e);
      this.snackBar.open('Failed to save log locally', 'Dismiss', { duration: 3000 });
    }
  }

  resetDailyLog() {
    this.dailyLog = {
      date: new Date(),
      engineHours: 0,
      idleHours: 0,
      serviceHours: 0,
      drillingSummary: '',
      notes: ''
    };
  }
}