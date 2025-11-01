import { Component, inject, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatChipsModule } from '@angular/material/chips';
import { MatCardModule } from '@angular/material/card';
import { ProblemReport, ReportStatus } from '../models/maintenance-report.models';
import { StatusBadgeComponent } from '../shared/status-badge/status-badge.component';

@Component({
  selector: 'app-report-details-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatChipsModule,
    MatCardModule,
    StatusBadgeComponent
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="report-details-dialog">
      <div mat-dialog-title class="dialog-header">
        <div class="header-content">
          <mat-icon class="header-icon">description</mat-icon>
          <div class="header-text">
            <h2>Maintenance Report Details</h2>
            <span class="ticket-id">Ticket #{{ report.ticketId }}</span>
          </div>
        </div>
        <button mat-icon-button mat-dialog-close class="close-button">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-dialog-content class="dialog-content">
        <!-- Status Section -->
        <section class="detail-section">
          <div class="section-header">
            <mat-icon>info</mat-icon>
            <h3>Status Information</h3>
          </div>
          <div class="section-content">
            <app-status-badge [status]="report.status"></app-status-badge>
            <div class="severity-badge" [class]="'severity-' + report.severity.toLowerCase()">
              <mat-icon>{{ getSeverityIcon(report.severity) }}</mat-icon>
              <span>{{ getSeverityLabel(report.severity) }}</span>
            </div>
          </div>
        </section>

        <mat-divider></mat-divider>

        <!-- Operator Information -->
        <section class="detail-section">
          <div class="section-header">
            <mat-icon>person</mat-icon>
            <h3>Reported By</h3>
          </div>
          <div class="info-grid">
            <div class="info-item">
              <span class="info-label">Operator Name</span>
              <span class="info-value">{{ report.operatorName }}</span>
            </div>
            @if (report.operatorEmail) {
              <div class="info-item">
                <span class="info-label">Email</span>
                <span class="info-value">
                  <a [href]="'mailto:' + report.operatorEmail">{{ report.operatorEmail }}</a>
                </span>
              </div>
            }
            @if (report.operatorPhone) {
              <div class="info-item">
                <span class="info-label">Phone</span>
                <span class="info-value">
                  <a [href]="'tel:' + report.operatorPhone">{{ report.operatorPhone }}</a>
                </span>
              </div>
            }
          </div>
        </section>

        <mat-divider></mat-divider>

        <!-- Machine Information -->
        <section class="detail-section">
          <div class="section-header">
            <mat-icon>precision_manufacturing</mat-icon>
            <h3>Machine Information</h3>
          </div>
          <div class="info-grid">
            <div class="info-item">
              <span class="info-label">Machine Name</span>
              <span class="info-value">{{ report.machineName || 'N/A' }}</span>
            </div>
            <div class="info-item">
              <span class="info-label">Model</span>
              <span class="info-value">{{ report.machineModel || 'N/A' }}</span>
            </div>
            <div class="info-item">
              <span class="info-label">Serial Number</span>
              <span class="info-value">{{ report.serialNumber || 'N/A' }}</span>
            </div>
            <div class="info-item">
              <span class="info-label">Location</span>
              <span class="info-value">{{ report.location || 'N/A' }}</span>
            </div>
            <div class="info-item">
              <span class="info-label">Project</span>
              <span class="info-value">{{ report.projectName || 'Not Assigned' }}</span>
            </div>
            <div class="info-item">
              <span class="info-label">Region</span>
              <span class="info-value">{{ report.regionName || 'Not Assigned' }}</span>
            </div>
          </div>
        </section>

        <mat-divider></mat-divider>

        <!-- Problem Details -->
        <section class="detail-section">
          <div class="section-header">
            <mat-icon>report_problem</mat-icon>
            <h3>Problem Details</h3>
          </div>
          <div class="info-grid">
            <div class="info-item">
              <span class="info-label">Affected Part</span>
              <span class="info-value">{{ getMachinePartLabel(report.affectedPart) }}</span>
            </div>
            <div class="info-item">
              <span class="info-label">Problem Category</span>
              <span class="info-value">{{ getProblemCategoryLabel(report.problemCategory) }}</span>
            </div>
          </div>

          <div class="description-box">
            <h4>Description</h4>
            <p>{{ report.customDescription }}</p>
          </div>

          @if (report.symptoms && report.symptoms.length > 0) {
            <div class="symptoms-section">
              <h4>Symptoms</h4>
              <div class="symptoms-chips">
                @for (symptom of report.symptoms; track symptom) {
                  <mat-chip>
                    <mat-icon matChipAvatar>warning</mat-icon>
                    {{ formatSymptom(symptom) }}
                  </mat-chip>
                }
              </div>
            </div>
          }

          @if (report.errorCodes) {
            <div class="error-codes">
              <h4>Error Codes</h4>
              <code>{{ report.errorCodes }}</code>
            </div>
          }

          @if (report.recentMaintenanceHistory) {
            <div class="maintenance-history">
              <h4>Recent Maintenance History</h4>
              <p>{{ report.recentMaintenanceHistory }}</p>
            </div>
          }
        </section>

        <mat-divider></mat-divider>

        <!-- Timeline -->
        <section class="detail-section">
          <div class="section-header">
            <mat-icon>timeline</mat-icon>
            <h3>Timeline</h3>
          </div>
          <div class="timeline">
            <div class="timeline-item completed">
              <div class="timeline-marker">
                <mat-icon>report</mat-icon>
              </div>
              <div class="timeline-content">
                <h4>Reported</h4>
                <p class="timeline-date">{{ formatDate(report.reportedAt) }}</p>
                <p class="timeline-user">by {{ report.operatorName }}</p>
              </div>
            </div>

            @if (report.acknowledgedAt) {
              <div class="timeline-item completed">
                <div class="timeline-marker">
                  <mat-icon>visibility</mat-icon>
                </div>
                <div class="timeline-content">
                  <h4>Acknowledged</h4>
                  <p class="timeline-date">{{ formatDate(report.acknowledgedAt) }}</p>
                  @if (report.mechanicalEngineerName) {
                    <p class="timeline-user">by {{ report.mechanicalEngineerName }}</p>
                  }
                </div>
              </div>
            }

            @if (report.inProgressAt) {
              <div class="timeline-item completed">
                <div class="timeline-marker">
                  <mat-icon>build</mat-icon>
                </div>
                <div class="timeline-content">
                  <h4>In Progress</h4>
                  <p class="timeline-date">{{ formatDate(report.inProgressAt) }}</p>
                </div>
              </div>
            }

            @if (report.resolvedAt) {
              <div class="timeline-item completed">
                <div class="timeline-marker">
                  <mat-icon>check_circle</mat-icon>
                </div>
                <div class="timeline-content">
                  <h4>Resolved</h4>
                  <p class="timeline-date">{{ formatDate(report.resolvedAt) }}</p>
                </div>
              </div>
            }

            @if (report.closedAt) {
              <div class="timeline-item completed">
                <div class="timeline-marker">
                  <mat-icon>archive</mat-icon>
                </div>
                <div class="timeline-content">
                  <h4>Closed</h4>
                  <p class="timeline-date">{{ formatDate(report.closedAt) }}</p>
                </div>
              </div>
            }
          </div>
        </section>

        @if (report.resolutionNotes) {
          <mat-divider></mat-divider>
          <section class="detail-section">
            <div class="section-header">
              <mat-icon>notes</mat-icon>
              <h3>Resolution Notes</h3>
            </div>
            <div class="resolution-box">
              <p>{{ report.resolutionNotes }}</p>
            </div>
          </section>
        }

        @if (report.estimatedResponseTime) {
          <mat-divider></mat-divider>
          <section class="detail-section">
            <div class="section-header">
              <mat-icon>schedule</mat-icon>
              <h3>Estimated Response Time</h3>
            </div>
            <div class="response-time">
              <p>{{ report.estimatedResponseTime }}</p>
            </div>
          </section>
        }
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button mat-dialog-close>Close</button>
        <button mat-raised-button color="primary" (click)="printReport()">
          <mat-icon>print</mat-icon>
          Print Report
        </button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .report-details-dialog {
      display: flex;
      flex-direction: column;
      min-width: 600px;
      max-width: 800px;
    }

    .dialog-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      padding: 1.5rem;
      margin: 0;
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      color: white;
    }

    .header-content {
      display: flex;
      align-items: center;
      gap: 1rem;
    }

    .header-icon {
      font-size: 32px;
      width: 32px;
      height: 32px;
    }

    .header-text h2 {
      margin: 0;
      font-size: 1.5rem;
      font-weight: 500;
    }

    .ticket-id {
      font-size: 0.875rem;
      opacity: 0.9;
    }

    .close-button {
      color: white;
    }

    .dialog-content {
      padding: 1.5rem;
      max-height: 70vh;
      overflow-y: auto;
    }

    .detail-section {
      margin: 1.5rem 0;
    }

    .section-header {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      margin-bottom: 1rem;
      color: #333;
    }

    .section-header mat-icon {
      color: #667eea;
    }

    .section-header h3 {
      margin: 0;
      font-size: 1.125rem;
      font-weight: 500;
    }

    .section-content {
      display: flex;
      gap: 1rem;
      align-items: center;
    }

    .severity-badge {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      padding: 0.5rem 1rem;
      border-radius: 20px;
      font-weight: 500;
      font-size: 0.875rem;
    }

    .severity-badge mat-icon {
      font-size: 18px;
      width: 18px;
      height: 18px;
    }

    .severity-critical {
      background-color: #ffebee;
      color: #c62828;
    }

    .severity-high {
      background-color: #fff3e0;
      color: #e65100;
    }

    .severity-medium {
      background-color: #fffde7;
      color: #f57f17;
    }

    .severity-low {
      background-color: #e8f5e9;
      color: #2e7d32;
    }

    .info-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
      gap: 1rem;
      margin-bottom: 1rem;
    }

    .info-item {
      display: flex;
      flex-direction: column;
      gap: 0.25rem;
    }

    .info-label {
      font-size: 0.75rem;
      color: #666;
      text-transform: uppercase;
      letter-spacing: 0.5px;
      font-weight: 500;
    }

    .info-value {
      font-size: 1rem;
      color: #333;
      font-weight: 500;
    }

    .info-value a {
      color: #667eea;
      text-decoration: none;
      transition: color 0.2s;
    }

    .info-value a:hover {
      color: #764ba2;
      text-decoration: underline;
    }

    .description-box,
    .maintenance-history,
    .resolution-box,
    .response-time {
      background-color: #f5f5f5;
      padding: 1rem;
      border-radius: 8px;
      margin-top: 1rem;
    }

    .description-box h4,
    .maintenance-history h4,
    .symptoms-section h4,
    .error-codes h4 {
      margin: 0 0 0.5rem 0;
      font-size: 0.875rem;
      color: #666;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }

    .description-box p,
    .maintenance-history p,
    .resolution-box p,
    .response-time p {
      margin: 0;
      color: #333;
      line-height: 1.6;
    }

    .symptoms-section {
      margin-top: 1rem;
    }

    .symptoms-chips {
      display: flex;
      flex-wrap: wrap;
      gap: 0.5rem;
      margin-top: 0.5rem;
    }

    .symptoms-chips mat-chip {
      background-color: #fff3e0;
      color: #e65100;
    }

    .error-codes {
      margin-top: 1rem;
    }

    .error-codes code {
      display: block;
      background-color: #263238;
      color: #aed581;
      padding: 1rem;
      border-radius: 4px;
      font-family: 'Courier New', monospace;
      font-size: 0.875rem;
      margin-top: 0.5rem;
    }

    .timeline {
      position: relative;
      padding-left: 2rem;
    }

    .timeline::before {
      content: '';
      position: absolute;
      left: 1rem;
      top: 0;
      bottom: 0;
      width: 2px;
      background: linear-gradient(to bottom, #667eea, #764ba2);
    }

    .timeline-item {
      position: relative;
      margin-bottom: 2rem;
      padding-left: 2rem;
    }

    .timeline-marker {
      position: absolute;
      left: -2.5rem;
      top: 0;
      width: 2.5rem;
      height: 2.5rem;
      background: white;
      border: 3px solid #667eea;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .timeline-marker mat-icon {
      font-size: 20px;
      width: 20px;
      height: 20px;
      color: #667eea;
    }

    .timeline-item.completed .timeline-marker {
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      border-color: #764ba2;
    }

    .timeline-item.completed .timeline-marker mat-icon {
      color: white;
    }

    .timeline-content h4 {
      margin: 0 0 0.25rem 0;
      font-size: 1rem;
      color: #333;
    }

    .timeline-date {
      margin: 0;
      font-size: 0.875rem;
      color: #666;
    }

    .timeline-user {
      margin: 0.25rem 0 0 0;
      font-size: 0.75rem;
      color: #999;
      font-style: italic;
    }

    mat-dialog-actions {
      padding: 1rem 1.5rem;
      border-top: 1px solid #e0e0e0;
      margin: 0;
    }

    mat-dialog-actions button {
      margin-left: 0.5rem;
    }

    mat-divider {
      margin: 1.5rem 0;
    }

    @media (max-width: 768px) {
      .report-details-dialog {
        min-width: 100%;
        max-width: 100%;
      }

      .info-grid {
        grid-template-columns: 1fr;
      }

      .dialog-content {
        padding: 1rem;
      }
    }
  `]
})
export class ReportDetailsDialogComponent {
  data = inject(MAT_DIALOG_DATA);
  dialogRef = inject(MatDialogRef<ReportDetailsDialogComponent>);

  report: ProblemReport = this.data.report;

  getSeverityIcon(severity: string): string {
    switch (severity) {
      case 'CRITICAL': return 'error';
      case 'HIGH': return 'warning';
      case 'MEDIUM': return 'info';
      case 'LOW': return 'check_circle';
      default: return 'help';
    }
  }

  getSeverityLabel(severity: string): string {
    switch (severity) {
      case 'CRITICAL': return 'Critical';
      case 'HIGH': return 'High Priority';
      case 'MEDIUM': return 'Medium Priority';
      case 'LOW': return 'Low Priority';
      default: return severity;
    }
  }

  getMachinePartLabel(part: string): string {
    const labels: { [key: string]: string } = {
      'DRILL_BIT': 'Drill Bit',
      'DRILL_ROD': 'Drill Rod',
      'SHANK': 'Shank',
      'ENGINE': 'Engine',
      'HYDRAULIC_SYSTEM': 'Hydraulic System',
      'ELECTRICAL_SYSTEM': 'Electrical System',
      'MECHANICAL_COMPONENTS': 'Mechanical Components',
      'OTHER': 'Other'
    };
    return labels[part] || part;
  }

  getProblemCategoryLabel(category: string): string {
    const labels: { [key: string]: string } = {
      'ENGINE_ISSUES': 'Engine Issues',
      'HYDRAULIC_PROBLEMS': 'Hydraulic Problems',
      'ELECTRICAL_FAULTS': 'Electrical Faults',
      'MECHANICAL_BREAKDOWN': 'Mechanical Breakdown',
      'DRILL_BIT_ISSUES': 'Drill Bit Issues',
      'DRILL_ROD_PROBLEMS': 'Drill Rod Problems',
      'OTHER': 'Other'
    };
    return labels[category] || category;
  }

  formatSymptom(symptom: string): string {
    return symptom.replace(/_/g, ' ').replace(/\b\w/g, l => l.toUpperCase());
  }

  formatDate(date: Date | string): string {
    const d = new Date(date);
    return d.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  printReport(): void {
    window.print();
  }
}
