import { Component, OnInit, inject, signal, computed, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTabsModule } from '@angular/material/tabs';
import { ChangeDetectionStrategy } from '@angular/core';

import { MaintenanceJob, MachineMaintenanceHistory, MaintenanceRecord, MaintenanceStatus, MaintenanceType } from '../../models/maintenance.models';
import { MaintenanceService } from '../../services/maintenance.service';

@Component({
  selector: 'app-job-detail-panel',
  standalone: true,
  imports: [
    CommonModule,
    MatSidenavModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatChipsModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    MatTabsModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <mat-sidenav-container class="detail-panel-container">
      <mat-sidenav 
        #sidenav 
        position="end" 
        mode="over" 
        class="detail-panel"
        [opened]="isOpen()"
        (openedChange)="onOpenedChange($event)">
        
        @if (selectedJob()) {
          <div class="panel-header">
            <div class="header-content">
              <h2>Job Details</h2>
              <button mat-icon-button (click)="closePanel()" aria-label="Close panel">
                <mat-icon>close</mat-icon>
              </button>
            </div>
            <div class="job-title">
              <h3>{{ selectedJob()?.machineName }}</h3>
              <mat-chip [class]="getStatusChipClass(selectedJob()?.status!)">
                <mat-icon [class]="getStatusIconClass(selectedJob()?.status!)">
                  {{ getStatusIcon(selectedJob()?.status!) }}
                </mat-icon>
                {{ getStatusDisplayName(selectedJob()?.status!) }}
              </mat-chip>
            </div>
          </div>

          <div class="panel-content">
            <mat-tab-group>
              <!-- Machine Overview Tab -->
              <mat-tab label="Machine Overview">
                <div class="tab-content">
                  @if (isLoadingHistory()) {
                    <div class="loading-container">
                      <mat-spinner diameter="40"></mat-spinner>
                      <p>Loading machine details...</p>
                    </div>
                  } @else if (machineHistory()) {
                    <div class="machine-overview">
                      <!-- Machine Information Card -->
                      <mat-card class="info-card">
                        <mat-card-header>
                          <mat-card-title>
                            <mat-icon>precision_manufacturing</mat-icon>
                            Machine Information
                          </mat-card-title>
                        </mat-card-header>
                        <mat-card-content>
                          <div class="info-grid">
                            <div class="info-item">
                              <label>Machine Name:</label>
                              <span>{{ machineHistory()?.machineName }}</span>
                            </div>
                            <div class="info-item">
                              <label>Model:</label>
                              <span>{{ machineHistory()?.model }}</span>
                            </div>
                            <div class="info-item">
                              <label>Serial Number:</label>
                              <span>{{ machineHistory()?.serialNumber }}</span>
                            </div>
                            <div class="info-item">
                              <label>Current Status:</label>
                              <span class="status-text">{{ machineHistory()?.currentStatus }}</span>
                            </div>
                            <div class="info-item">
                              <label>Last Service Date:</label>
                              <span>{{ formatDate(machineHistory()?.lastServiceDate!) }}</span>
                            </div>
                            <div class="info-item">
                              <label>Next Service Date:</label>
                              <span [class]="getNextServiceClass()">
                                {{ formatDate(machineHistory()?.nextServiceDate!) }}
                              </span>
                            </div>
                          </div>
                        </mat-card-content>
                      </mat-card>

                      <!-- Usage Metrics Card -->
                      <mat-card class="info-card">
                        <mat-card-header>
                          <mat-card-title>
                            <mat-icon>analytics</mat-icon>
                            Usage Metrics
                          </mat-card-title>
                        </mat-card-header>
                        <mat-card-content>
                          <div class="metrics-grid">
                            <div class="metric-item">
                              <div class="metric-value">{{ machineHistory()?.engineHours || 0 }}</div>
                              <div class="metric-label">Engine Hours</div>
                            </div>
                            <div class="metric-item">
                              <div class="metric-value">{{ machineHistory()?.serviceHours || 0 }}</div>
                              <div class="metric-label">Service Hours</div>
                            </div>
                            <div class="metric-item">
                              <div class="metric-value">{{ machineHistory()?.idleHours || 0 }}</div>
                              <div class="metric-label">Idle Hours</div>
                            </div>
                            <div class="metric-item">
                              <div class="metric-value">{{ getHoursSinceLastService() }}</div>
                              <div class="metric-label">Hours Since Last Service</div>
                            </div>
                          </div>
                        </mat-card-content>
                      </mat-card>
                    </div>
                  } @else {
                    <div class="error-message">
                      <mat-icon>error</mat-icon>
                      <p>Unable to load machine details</p>
                    </div>
                  }
                </div>
              </mat-tab>

              <!-- Maintenance History Tab -->
              <mat-tab label="Maintenance History">
                <div class="tab-content">
                  @if (isLoadingHistory()) {
                    <div class="loading-container">
                      <mat-spinner diameter="40"></mat-spinner>
                      <p>Loading maintenance history...</p>
                    </div>
                  } @else if (maintenanceRecords().length > 0) {
                    <div class="history-timeline">
                      @for (record of maintenanceRecords(); track record.id) {
                        <div class="timeline-item">
                          <div class="timeline-marker">
                            <mat-icon [class]="getTypeIconClass(record.type)">
                              {{ getTypeIcon(record.type) }}
                            </mat-icon>
                          </div>
                          <div class="timeline-content">
                            <mat-card class="history-card">
                              <mat-card-header>
                                <div class="history-header">
                                  <div class="history-title">
                                    <h4>{{ getTypeDisplayName(record.type) }}</h4>
                                    <span class="history-date">{{ formatDateTime(record.date) }}</span>
                                  </div>
                                  <mat-chip class="hours-chip">
                                    {{ record.hoursSpent }}h
                                  </mat-chip>
                                </div>
                              </mat-card-header>
                              <mat-card-content>
                                <p class="description">{{ record.description }}</p>
                                <div class="record-details">
                                  <div class="detail-item">
                                    <mat-icon>person</mat-icon>
                                    <span>{{ record.technician }}</span>
                                  </div>
                                  @if (record.partsUsed && record.partsUsed.length > 0) {
                                    <div class="detail-item">
                                      <mat-icon>build</mat-icon>
                                      <span>Parts: {{ record.partsUsed.join(', ') }}</span>
                                    </div>
                                  }
                                  @if (record.notes) {
                                    <div class="detail-item notes">
                                      <mat-icon>note</mat-icon>
                                      <span>{{ record.notes }}</span>
                                    </div>
                                  }
                                </div>
                              </mat-card-content>
                            </mat-card>
                          </div>
                        </div>
                      }
                    </div>
                  } @else {
                    <div class="empty-state">
                      <mat-icon>history</mat-icon>
                      <p>No maintenance history available</p>
                    </div>
                  }
                </div>
              </mat-tab>

              <!-- Current Job Details Tab -->
              <mat-tab label="Current Job">
                <div class="tab-content">
                  <div class="current-job-details">
                    <!-- Job Information Card -->
                    <mat-card class="info-card">
                      <mat-card-header>
                        <mat-card-title>
                          <mat-icon>assignment</mat-icon>
                          Job Information
                        </mat-card-title>
                      </mat-card-header>
                      <mat-card-content>
                        <div class="info-grid">
                          <div class="info-item">
                            <label>Job ID:</label>
                            <span>{{ selectedJob()?.id }}</span>
                          </div>
                          <div class="info-item">
                            <label>Project:</label>
                            <span>{{ selectedJob()?.project }}</span>
                          </div>
                          <div class="info-item">
                            <label>Scheduled Date/Time:</label>
                            <span>{{ formatDateTime(selectedJob()?.scheduledDate!) }}</span>
                          </div>
                          <div class="info-item">
                            <label>Type:</label>
                            <mat-chip [class]="getTypeChipClass(selectedJob()?.type!)">
                              {{ getTypeDisplayName(selectedJob()?.type!) }}
                            </mat-chip>
                          </div>
                          <div class="info-item">
                            <label>Reason:</label>
                            <span>{{ selectedJob()?.reason }}</span>
                          </div>
                          <div class="info-item">
                            <label>Estimated Hours:</label>
                            <span>{{ selectedJob()?.estimatedHours }}h</span>
                          </div>
                          @if (selectedJob()?.actualHours) {
                            <div class="info-item">
                              <label>Actual Hours:</label>
                              <span>{{ selectedJob()?.actualHours }}h</span>
                            </div>
                          }
                        </div>
                      </mat-card-content>
                    </mat-card>

                    <!-- Assigned Technicians Card -->
                    <mat-card class="info-card">
                      <mat-card-header>
                        <mat-card-title>
                          <mat-icon>group</mat-icon>
                          Assigned Technicians
                        </mat-card-title>
                      </mat-card-header>
                      <mat-card-content>
                        @if (selectedJob()?.assignedTo && selectedJob()?.assignedTo!.length > 0) {
                          <div class="technicians-list">
                            @for (technician of selectedJob()?.assignedTo; track technician) {
                              <mat-chip class="technician-chip">
                                <mat-icon>person</mat-icon>
                                {{ technician }}
                              </mat-chip>
                            }
                          </div>
                        } @else {
                          <div class="empty-state-small">
                            <mat-icon>person_off</mat-icon>
                            <span>No technicians assigned</span>
                          </div>
                        }
                      </mat-card-content>
                    </mat-card>

                    <!-- Job Progress Card -->
                    @if (selectedJob()?.observations || selectedJob()?.partsReplaced?.length) {
                      <mat-card class="info-card">
                        <mat-card-header>
                          <mat-card-title>
                            <mat-icon>progress_activity</mat-icon>
                            Job Progress
                          </mat-card-title>
                        </mat-card-header>
                        <mat-card-content>
                          @if (selectedJob()?.observations) {
                            <div class="progress-item">
                              <label>Observations:</label>
                              <p class="observations">{{ selectedJob()?.observations }}</p>
                            </div>
                          }
                          @if (selectedJob()?.partsReplaced && selectedJob()?.partsReplaced!.length > 0) {
                            <div class="progress-item">
                              <label>Parts Replaced:</label>
                              <div class="parts-list">
                                @for (part of selectedJob()?.partsReplaced; track part) {
                                  <mat-chip class="part-chip">{{ part }}</mat-chip>
                                }
                              </div>
                            </div>
                          }
                        </mat-card-content>
                      </mat-card>
                    }

                    <!-- Attachments Card -->
                    @if (selectedJob()?.attachments && selectedJob()?.attachments!.length > 0) {
                      <mat-card class="info-card">
                        <mat-card-header>
                          <mat-card-title>
                            <mat-icon>attach_file</mat-icon>
                            Attachments
                          </mat-card-title>
                        </mat-card-header>
                        <mat-card-content>
                          <div class="attachments-list">
                            @for (attachment of selectedJob()?.attachments; track attachment.id) {
                              <div class="attachment-item">
                                <mat-icon>{{ getFileIcon(attachment.fileType) }}</mat-icon>
                                <div class="attachment-info">
                                  <span class="filename">{{ attachment.fileName }}</span>
                                  <span class="filesize">{{ formatFileSize(attachment.fileSize) }}</span>
                                </div>
                                <button mat-icon-button 
                                        [matTooltip]="'Download ' + attachment.fileName"
                                        (click)="downloadAttachment(attachment)">
                                  <mat-icon>download</mat-icon>
                                </button>
                              </div>
                            }
                          </div>
                        </mat-card-content>
                      </mat-card>
                    }
                  </div>
                </div>
              </mat-tab>
            </mat-tab-group>
          </div>

          <!-- Panel Actions -->
          <div class="panel-actions">
            <button mat-stroked-button (click)="editJob.emit(selectedJob()!)">
              <mat-icon>edit</mat-icon>
              Edit Job
            </button>
            <button mat-raised-button color="primary" (click)="updateStatus.emit(selectedJob()!)">
              <mat-icon>update</mat-icon>
              Update Status
            </button>
          </div>
        } @else {
          <div class="empty-panel">
            <mat-icon>assignment</mat-icon>
            <h3>No Job Selected</h3>
            <p>Select a job from the list to view details</p>
          </div>
        }
      </mat-sidenav>
      
      <mat-sidenav-content>
        <ng-content></ng-content>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: [`
    .detail-panel-container {
      height: 100%;
    }

    .detail-panel {
      width: 480px;
      max-width: 90vw;
    }

    .panel-header {
      padding: 16px;
      background-color: #f5f5f5;
      border-bottom: 1px solid #e0e0e0;
    }

    .header-content {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 8px;
    }

    .header-content h2 {
      margin: 0;
      font-size: 18px;
      font-weight: 500;
    }

    .job-title {
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .job-title h3 {
      margin: 0;
      font-size: 16px;
      color: #333;
    }

    .panel-content {
      flex: 1;
      overflow-y: auto;
    }

    .tab-content {
      padding: 16px;
    }

    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 40px;
      text-align: center;
    }

    .loading-container p {
      margin-top: 16px;
      color: #666;
    }

    .error-message {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 40px;
      text-align: center;
      color: #d32f2f;
    }

    .error-message mat-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
      margin-bottom: 16px;
    }

    .empty-state {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 40px;
      text-align: center;
      color: #666;
    }

    .empty-state mat-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
      margin-bottom: 16px;
    }

    .empty-state-small {
      display: flex;
      align-items: center;
      gap: 8px;
      color: #666;
      font-style: italic;
    }

    .empty-panel {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      height: 100%;
      text-align: center;
      color: #666;
    }

    .empty-panel mat-icon {
      font-size: 64px;
      width: 64px;
      height: 64px;
      margin-bottom: 16px;
    }

    .info-card {
      margin-bottom: 16px;
    }

    .info-card mat-card-title {
      display: flex;
      align-items: center;
      gap: 8px;
      font-size: 16px;
    }

    .info-grid {
      display: grid;
      grid-template-columns: 1fr;
      gap: 12px;
    }

    .info-item {
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .info-item label {
      font-weight: 500;
      color: #666;
    }

    .status-text {
      font-weight: 500;
    }

    .next-service-overdue {
      color: #d32f2f;
      font-weight: 500;
    }

    .next-service-due-soon {
      color: #f57c00;
      font-weight: 500;
    }

    .metrics-grid {
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      gap: 16px;
    }

    .metric-item {
      text-align: center;
      padding: 12px;
      background-color: #f8f9fa;
      border-radius: 4px;
    }

    .metric-value {
      font-size: 24px;
      font-weight: 600;
      color: #1976d2;
    }

    .metric-label {
      font-size: 12px;
      color: #666;
      margin-top: 4px;
    }

    .history-timeline {
      position: relative;
    }

    .timeline-item {
      display: flex;
      margin-bottom: 24px;
      position: relative;
    }

    .timeline-item:not(:last-child)::after {
      content: '';
      position: absolute;
      left: 20px;
      top: 40px;
      bottom: -24px;
      width: 2px;
      background-color: #e0e0e0;
    }

    .timeline-marker {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      background-color: #1976d2;
      display: flex;
      align-items: center;
      justify-content: center;
      margin-right: 16px;
      flex-shrink: 0;
      z-index: 1;
    }

    .timeline-marker mat-icon {
      color: white;
      font-size: 20px;
    }

    .timeline-content {
      flex: 1;
    }

    .history-card {
      margin: 0;
    }

    .history-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      width: 100%;
    }

    .history-title h4 {
      margin: 0;
      font-size: 16px;
    }

    .history-date {
      font-size: 12px;
      color: #666;
    }

    .hours-chip {
      background-color: #e3f2fd;
      color: #1976d2;
      font-size: 12px;
    }

    .description {
      margin: 8px 0;
      color: #333;
    }

    .record-details {
      display: flex;
      flex-direction: column;
      gap: 8px;
    }

    .detail-item {
      display: flex;
      align-items: flex-start;
      gap: 8px;
      font-size: 14px;
      color: #666;
    }

    .detail-item mat-icon {
      font-size: 16px;
      width: 16px;
      height: 16px;
    }

    .detail-item.notes {
      align-items: flex-start;
    }

    .technicians-list {
      display: flex;
      flex-wrap: wrap;
      gap: 8px;
    }

    .technician-chip {
      display: flex;
      align-items: center;
      gap: 4px;
    }

    .progress-item {
      margin-bottom: 16px;
    }

    .progress-item label {
      display: block;
      font-weight: 500;
      color: #666;
      margin-bottom: 8px;
    }

    .observations {
      margin: 0;
      padding: 12px;
      background-color: #f8f9fa;
      border-radius: 4px;
      border-left: 4px solid #1976d2;
    }

    .parts-list {
      display: flex;
      flex-wrap: wrap;
      gap: 8px;
    }

    .part-chip {
      background-color: #fff3e0;
      color: #f57c00;
    }

    .attachments-list {
      display: flex;
      flex-direction: column;
      gap: 8px;
    }

    .attachment-item {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 8px;
      border: 1px solid #e0e0e0;
      border-radius: 4px;
    }

    .attachment-info {
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

    .panel-actions {
      padding: 16px;
      border-top: 1px solid #e0e0e0;
      display: flex;
      gap: 12px;
      justify-content: flex-end;
    }

    /* Status and Type Chips */
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

    .type-preventive {
      background-color: #e8f5e8;
      color: #388e3c;
    }

    .type-corrective {
      background-color: #fff3e0;
      color: #f57c00;
    }

    .type-predictive {
      background-color: #e3f2fd;
      color: #1976d2;
    }

    .type-emergency {
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

    /* Type Icons */
    .type-icon-preventive {
      color: #388e3c;
    }

    .type-icon-corrective {
      color: #f57c00;
    }

    .type-icon-predictive {
      color: #1976d2;
    }

    .type-icon-emergency {
      color: #d32f2f;
    }
  `]
})
export class JobDetailPanelComponent implements OnInit {
  private maintenanceService = inject(MaintenanceService);

  // Inputs
  selectedJob = input<MaintenanceJob | null>(null);
  isOpen = input<boolean>(false);

  // Outputs
  panelClosed = output<void>();
  editJob = output<MaintenanceJob>();
  updateStatus = output<MaintenanceJob>();

  // Signals
  machineHistory = signal<MachineMaintenanceHistory | null>(null);
  isLoadingHistory = signal(false);

  // Computed values
  maintenanceRecords = computed(() => {
    const history = this.machineHistory();
    return history?.maintenanceRecords || [];
  });

  ngOnInit() {
    // Watch for selected job changes
    this.loadMachineHistory();
  }

  private async loadMachineHistory() {
    const job = this.selectedJob();
    if (!job) {
      this.machineHistory.set(null);
      return;
    }

    this.isLoadingHistory.set(true);
    try {
      this.maintenanceService.getMachineMaintenanceHistory(job.machineId).subscribe({
        next: (history) => {
          this.machineHistory.set(history);
          this.isLoadingHistory.set(false);
        },
        error: (error) => {
          console.error('Error loading machine history:', error);
          this.machineHistory.set(null);
          this.isLoadingHistory.set(false);
        }
      });
    } catch (error) {
      console.error('Error loading machine history:', error);
      this.machineHistory.set(null);
      this.isLoadingHistory.set(false);
    }
  }

  // Event Handlers
  onOpenedChange(opened: boolean) {
    if (!opened) {
      this.panelClosed.emit();
    }
  }

  closePanel() {
    this.panelClosed.emit();
  }

  downloadAttachment(attachment: any) {
    // TODO: Implement file download
    console.log('Download attachment:', attachment);
  }

  // Utility Methods
  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString();
  }

  formatDateTime(date: Date): string {
    return new Date(date).toLocaleString();
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
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

  getTypeDisplayName(type: MaintenanceType): string {
    const typeNames = {
      [MaintenanceType.PREVENTIVE]: 'Preventive',
      [MaintenanceType.CORRECTIVE]: 'Corrective',
      [MaintenanceType.PREDICTIVE]: 'Predictive',
      [MaintenanceType.EMERGENCY]: 'Emergency'
    };
    return typeNames[type];
  }

  getTypeIcon(type: MaintenanceType): string {
    const typeIcons = {
      [MaintenanceType.PREVENTIVE]: 'schedule',
      [MaintenanceType.CORRECTIVE]: 'build',
      [MaintenanceType.PREDICTIVE]: 'analytics',
      [MaintenanceType.EMERGENCY]: 'warning'
    };
    return typeIcons[type];
  }

  getTypeChipClass(type: MaintenanceType): string {
    return `type-${type.toLowerCase()}`;
  }

  getTypeIconClass(type: MaintenanceType): string {
    return `type-icon-${type.toLowerCase()}`;
  }

  getFileIcon(fileType: string): string {
    if (fileType.startsWith('image/')) {
      return 'image';
    } else if (fileType === 'application/pdf') {
      return 'picture_as_pdf';
    } else if (fileType.startsWith('text/')) {
      return 'description';
    } else {
      return 'attach_file';
    }
  }

  getNextServiceClass(): string {
    const history = this.machineHistory();
    if (!history) return '';

    const nextServiceDate = new Date(history.nextServiceDate);
    const now = new Date();
    const daysUntilService = Math.ceil((nextServiceDate.getTime() - now.getTime()) / (1000 * 60 * 60 * 24));

    if (daysUntilService < 0) {
      return 'next-service-overdue';
    } else if (daysUntilService <= 7) {
      return 'next-service-due-soon';
    }
    return '';
  }

  getHoursSinceLastService(): number {
    const history = this.machineHistory();
    if (!history) return 0;

    // Calculate hours since last service based on engine hours and service hours
    return Math.max(0, history.engineHours - history.serviceHours);
  }
}