import { Component, OnInit, signal, inject, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

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
    MatProgressSpinnerModule
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
      grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
      gap: 1rem;
    }

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
    }
  `]
})
export class MaintenanceReportsComponent implements OnInit {
  showReportForm = signal(false);
  operatorMachine = signal<OperatorMachine | null>(null);
  reports = signal<ProblemReport[]>([]);
  isLoading = signal(false);
  
  private maintenanceReportService = inject(MaintenanceReportService);
  private authService = inject(AuthService);

  inProgressCount = computed(() => 
    this.reports().filter(report => 
      report.status === 'IN_PROGRESS' || report.status === 'ACKNOWLEDGED'
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
}