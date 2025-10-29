import { Component, OnInit, signal, inject, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DatePipe, NgFor, NgIf } from '@angular/common';

// PrimeNG Imports
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { DropdownModule } from 'primeng/dropdown';
import { TagModule } from 'primeng/tag';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { BadgeModule } from 'primeng/badge';
import { TooltipModule } from 'primeng/tooltip';
import { DialogModule } from 'primeng/dialog';

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
    FormsModule,
    DatePipe,
    NgIf,
    NgFor,
    ButtonModule,
    CardModule,
    TableModule,
    InputTextModule,
    InputTextareaModule,
    DropdownModule,
    TagModule,
    ProgressSpinnerModule,
    ToastModule,
    IconFieldModule,
    InputIconModule,
    BadgeModule,
    TooltipModule,
    DialogModule
  ],
  providers: [MessageService],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <p-toast />

    <div class="min-h-screen bg-gradient-to-br from-slate-50 via-blue-50 to-indigo-50 p-3">
      <!-- Page Header -->
      <div class="mb-4 backdrop-blur-xl bg-white/70 rounded-2xl shadow-2xl border border-white/50 overflow-hidden">
        <div class="bg-gradient-to-r from-indigo-600 via-purple-600 to-pink-600 p-4">
          <div class="flex flex-col md:flex-row justify-between items-start md:items-center gap-3">
            <div class="flex items-center gap-3">
              <div class="w-12 h-12 rounded-xl bg-white/20 backdrop-blur-sm flex items-center justify-center">
                <i class="pi pi-file-edit text-2xl text-white"></i>
              </div>
              <div>
                <h1 class="text-2xl md:text-3xl font-bold text-white mb-0.5">Maintenance Reports</h1>
                <p class="text-white/80 text-xs md:text-sm">Submit and track machine maintenance reports</p>
              </div>
            </div>
            <p-button
              label="Submit Report"
              icon="pi pi-plus"
              (onClick)="toggleReportForm()"
              severity="warning"
              [raised]="true"
              styleClass="bg-white/20 border-white/30 hover:bg-white/30 text-white font-semibold px-6" />
          </div>
        </div>
      </div>

      @if (isLoading()) {
        <div class="flex flex-col items-center justify-center py-12 backdrop-blur-xl bg-white/70 rounded-2xl shadow-lg">
          <p-progressSpinner styleClass="w-12 h-12" strokeWidth="3" />
          <p class="mt-3 text-slate-600 text-sm font-medium">Loading reports...</p>
        </div>
      }

      <!-- Submit Report Dialog -->
      <p-dialog
        [(visible)]="showReportForm"
        [modal]="true"
        [closable]="true"
        [draggable]="false"
        [resizable]="false"
        [style]="{width: '50vw'}"
        [breakpoints]="{'960px': '75vw', '640px': '95vw'}"
        styleClass="report-dialog">
        <ng-template pTemplate="header">
          <div class="flex items-center gap-2">
            <div class="w-8 h-8 rounded-lg bg-gradient-to-br from-indigo-500 to-purple-500 flex items-center justify-center">
              <i class="pi pi-plus-circle text-lg text-white"></i>
            </div>
            <div>
              <h2 class="text-xl font-bold text-slate-800">Submit Maintenance Report</h2>
              <p class="text-xs text-slate-500 mt-0.5">Report a machine issue or maintenance need</p>
            </div>
          </div>
        </ng-template>

        <form (ngSubmit)="submitReport()" #reportForm="ngForm" class="space-y-4 py-2">
          <!-- Problem Description -->
          <div class="space-y-1.5">
            <label class="block text-xs font-semibold text-slate-700">
              Problem Description <span class="text-red-500">*</span>
            </label>
            <textarea
              pInputTextarea
              [(ngModel)]="newReport.customDescription"
              name="description"
              required
              rows="3"
              placeholder="Describe the problem in detail..."
              class="w-full border-2 border-slate-200 rounded-lg focus:border-indigo-500 focus:ring-2 focus:ring-indigo-200 transition-all text-sm"></textarea>
          </div>

          <!-- Affected Part & Problem Category -->
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div class="space-y-1.5">
              <label class="block text-xs font-semibold text-slate-700">
                Affected Part <span class="text-red-500">*</span>
              </label>
              <p-dropdown
                [(ngModel)]="newReport.affectedPart"
                name="affectedPart"
                [options]="affectedPartOptions()"
                optionLabel="label"
                optionValue="value"
                placeholder="Select affected part"
                styleClass="w-full text-sm" />
            </div>

            <div class="space-y-1.5">
              <label class="block text-xs font-semibold text-slate-700">
                Problem Category <span class="text-red-500">*</span>
              </label>
              <p-dropdown
                [(ngModel)]="newReport.problemCategory"
                name="problemCategory"
                [options]="problemCategoryOptions()"
                optionLabel="label"
                optionValue="value"
                placeholder="Select problem category"
                styleClass="w-full text-sm" />
            </div>
          </div>

          <!-- Severity Level -->
          <div class="space-y-1.5">
            <label class="block text-xs font-semibold text-slate-700">
              Severity Level <span class="text-red-500">*</span>
            </label>
            <p-dropdown
              [(ngModel)]="newReport.severity"
              name="severity"
              [options]="severityOptions()"
              optionLabel="label"
              optionValue="value"
              placeholder="Select severity level"
              styleClass="w-full text-sm" />
          </div>
        </form>

        <ng-template pTemplate="footer">
          <div class="flex justify-end gap-2">
            <p-button
              label="Cancel"
              icon="pi pi-times"
              severity="secondary"
              [outlined]="true"
              (onClick)="toggleReportForm()"
              styleClass="px-6" />
            <p-button
              label="Submit Report"
              icon="pi pi-check"
              severity="success"
              [disabled]="!reportForm.form.valid"
              (onClick)="submitReport()"
              styleClass="px-6" />
          </div>
        </ng-template>
      </p-dialog>

      <!-- Reports Summary Stats -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-3 mb-4">
        <!-- Pending Card -->
        <div class="backdrop-blur-xl bg-gradient-to-br from-amber-50 to-orange-50 rounded-xl p-3 border border-amber-200 shadow-lg hover:shadow-xl transition-all">
          <div class="flex items-center justify-between mb-2">
            <div class="w-10 h-10 rounded-lg bg-amber-500 flex items-center justify-center shadow-md">
              <i class="pi pi-clock text-lg text-white"></i>
            </div>
            <p-badge [value]="pendingCount().toString()" severity="warning" styleClass="text-sm px-2 py-0.5" />
          </div>
          <h3 class="text-2xl font-bold text-amber-700 mb-0.5">{{ pendingCount() }}</h3>
          <p class="text-xs font-semibold text-amber-600 uppercase tracking-wide">Pending Reports</p>
        </div>

        <!-- In Progress Card -->
        <div class="backdrop-blur-xl bg-gradient-to-br from-blue-50 to-indigo-50 rounded-xl p-3 border border-blue-200 shadow-lg hover:shadow-xl transition-all">
          <div class="flex items-center justify-between mb-2">
            <div class="w-10 h-10 rounded-lg bg-blue-500 flex items-center justify-center shadow-md">
              <i class="pi pi-sync text-lg text-white"></i>
            </div>
            <p-badge [value]="inProgressCount().toString()" severity="info" styleClass="text-sm px-2 py-0.5" />
          </div>
          <h3 class="text-2xl font-bold text-blue-700 mb-0.5">{{ inProgressCount() }}</h3>
          <p class="text-xs font-semibold text-blue-600 uppercase tracking-wide">In Progress</p>
        </div>

        <!-- Completed Card -->
        <div class="backdrop-blur-xl bg-gradient-to-br from-emerald-50 to-green-50 rounded-xl p-3 border border-emerald-200 shadow-lg hover:shadow-xl transition-all">
          <div class="flex items-center justify-between mb-2">
            <div class="w-10 h-10 rounded-lg bg-emerald-500 flex items-center justify-center shadow-md">
              <i class="pi pi-check-circle text-lg text-white"></i>
            </div>
            <p-badge [value]="completedCount().toString()" severity="success" styleClass="text-sm px-2 py-0.5" />
          </div>
          <h3 class="text-2xl font-bold text-emerald-700 mb-0.5">{{ completedCount() }}</h3>
          <p class="text-xs font-semibold text-emerald-600 uppercase tracking-wide">Completed</p>
        </div>
      </div>

      <!-- Report History -->
      <p-card styleClass="backdrop-blur-xl bg-white/80 border border-indigo-200 shadow-xl">
        <ng-template pTemplate="header">
          <div class="px-4 pt-4 pb-3 bg-gradient-to-r from-indigo-50 to-purple-50 border-b border-indigo-100">
            <div class="flex flex-col md:flex-row justify-between items-start md:items-center gap-3">
              <div>
                <h2 class="text-xl font-bold text-slate-800 mb-0.5">Report History</h2>
                <p class="text-xs text-slate-600">{{ reports().length }} total reports</p>
              </div>
              <p-iconField iconPosition="left" styleClass="w-full md:w-80">
                <p-inputIcon styleClass="pi pi-search" />
                <input
                  pInputText
                  type="text"
                  [(ngModel)]="searchValue"
                  (input)="onSearch()"
                  placeholder="Search reports..."
                  class="w-full border-2 border-slate-200 rounded-xl focus:border-indigo-500" />
              </p-iconField>
            </div>
          </div>
        </ng-template>

        @if (reports().length === 0) {
          <div class="flex flex-col items-center justify-center py-12 text-center">
            <div class="w-16 h-16 rounded-full bg-gradient-to-br from-slate-100 to-slate-200 flex items-center justify-center mb-4">
              <i class="pi pi-file text-3xl text-slate-400"></i>
            </div>
            <h3 class="text-lg font-semibold text-slate-700 mb-1">No reports submitted yet</h3>
            <p class="text-sm text-slate-500 mb-4">Click "Submit Report" to create your first maintenance report</p>
            <p-button
              label="Submit First Report"
              icon="pi pi-plus"
              (onClick)="toggleReportForm()"
              severity="info"
              [raised]="true" />
          </div>
        } @else {
          <p-table
            [value]="filteredReports()"
            [paginator]="true"
            [rows]="10"
            [rowsPerPageOptions]="[5, 10, 20, 50]"
            [showCurrentPageReport]="true"
            currentPageReportTemplate="Showing {first} to {last} of {totalRecords} reports"
            styleClass="p-datatable-sm p-datatable-striped"
            [tableStyle]="{'min-width': '60rem'}">

            <!-- Problem Description Column -->
            <ng-template pTemplate="header">
              <tr class="bg-gradient-to-r from-slate-50 to-slate-100">
                <th class="font-bold text-slate-700 py-2">Problem Description</th>
                <th class="font-bold text-slate-700 py-2">Affected Part</th>
                <th class="font-bold text-slate-700 py-2">Severity</th>
                <th class="font-bold text-slate-700 py-2">Status</th>
                <th class="font-bold text-slate-700 py-2">Reported</th>
                <th class="font-bold text-slate-700 py-2 text-center">Actions</th>
              </tr>
            </ng-template>

            <ng-template pTemplate="body" let-report>
              <tr class="hover:bg-indigo-50/50 transition-colors">
                <!-- Description -->
                <td class="py-2">
                  <div class="space-y-1">
                    <div class="font-medium text-slate-800 line-clamp-2">
                      {{ report.customDescription }}
                    </div>
                    <div class="text-xs text-slate-500 font-mono">
                      {{ report.ticketId }}
                    </div>
                  </div>
                </td>

                <!-- Affected Part -->
                <td class="py-2">
                  <div class="flex items-center gap-2">
                    <i [class]="'pi ' + getPartIcon(report.affectedPart) + ' text-slate-600'"></i>
                    <span class="text-sm font-medium text-slate-700">{{ getPartDisplay(report.affectedPart) }}</span>
                  </div>
                </td>

                <!-- Severity -->
                <td class="py-2">
                  <p-tag
                    [value]="getSeverityDisplay(report.severity)"
                    [severity]="getSeveritySeverity(report.severity)"
                    [icon]="'pi ' + getSeverityIcon(report.severity)"
                    styleClass="font-semibold text-xs" />
                </td>

                <!-- Status -->
                <td class="py-2">
                  <p-tag
                    [value]="getStatusDisplay(report.status)"
                    [severity]="getStatusSeverity(report.status)"
                    [icon]="'pi ' + getStatusIcon(report.status)"
                    styleClass="font-semibold text-xs" />
                </td>

                <!-- Date -->
                <td class="py-2">
                  <div class="space-y-0.5">
                    <div class="text-sm font-medium text-slate-700">
                      {{ report.reportedAt | date:'MMM d, y' }}
                    </div>
                    <div class="text-xs text-slate-500">
                      {{ report.reportedAt | date:'h:mm a' }}
                    </div>
                  </div>
                </td>

                <!-- Actions -->
                <td class="py-2 text-center">
                  <p-button
                    icon="pi pi-eye"
                    [rounded]="true"
                    [outlined]="true"
                    severity="info"
                    size="small"
                    (onClick)="viewReportDetails(report)"
                    pTooltip="View Details"
                    tooltipPosition="left" />
                </td>
              </tr>
            </ng-template>

            <ng-template pTemplate="emptymessage">
              <tr>
                <td colspan="6" class="text-center py-8">
                  <div class="text-slate-500">No reports found matching your search</div>
                </td>
              </tr>
            </ng-template>
          </p-table>
        }
      </p-card>
    </div>
  `,
  styles: [`
    @keyframes fade-in {
      from {
        opacity: 0;
        transform: translateY(-10px);
      }
      to {
        opacity: 1;
        transform: translateY(0);
      }
    }

    .animate-fade-in {
      animation: fade-in 0.3s ease-out;
    }

    .line-clamp-2 {
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
      overflow: hidden;
    }

    ::ng-deep .p-datatable .p-datatable-tbody > tr {
      transition: all 0.2s ease;
    }

    ::ng-deep .p-datatable .p-datatable-tbody > tr:hover {
      background-color: rgba(99, 102, 241, 0.05) !important;
    }

    /* Dialog Styling */
    ::ng-deep .report-dialog .p-dialog-header {
      background: linear-gradient(135deg, #f8fafc 0%, #f1f5f9 100%);
      border-bottom: 2px solid #e2e8f0;
      padding: 1rem;
    }

    ::ng-deep .report-dialog .p-dialog-content {
      background: white;
      padding: 1rem;
    }

    ::ng-deep .report-dialog .p-dialog-footer {
      background: linear-gradient(135deg, #f8fafc 0%, #f1f5f9 100%);
      border-top: 2px solid #e2e8f0;
      padding: 0.75rem 1rem;
    }

    ::ng-deep .report-dialog {
      border-radius: 1rem;
      box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.25);
    }
  `]

})
export class MaintenanceReportsComponent implements OnInit {
  showReportForm = false;
  operatorMachine = signal<OperatorMachine | null>(null);
  reports = signal<ProblemReport[]>([]);
  isLoading = signal(false);
  searchValue = '';

  newReport = {
    affectedPart: 'OTHER' as any,
    problemCategory: 'OTHER' as any,
    customDescription: '',
    symptoms: [] as string[],
    severity: 'MEDIUM' as any
  };

  private maintenanceReportService = inject(MaintenanceReportService);
  private authService = inject(AuthService);
  private messageService = inject(MessageService);

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

  // Filtered reports based on search
  filteredReports = computed(() => {
    if (!this.searchValue) {
      return this.reports();
    }
    const search = this.searchValue.toLowerCase();
    return this.reports().filter(report =>
      report.customDescription.toLowerCase().includes(search) ||
      report.ticketId.toLowerCase().includes(search) ||
      this.getPartDisplay(report.affectedPart).toLowerCase().includes(search) ||
      this.getSeverityDisplay(report.severity).toLowerCase().includes(search) ||
      this.getStatusDisplay(report.status).toLowerCase().includes(search)
    );
  });

  // Dropdown options
  affectedPartOptions = computed(() => [
    { label: 'Drill Bit', value: 'DRILL_BIT' },
    { label: 'Drill Rod', value: 'DRILL_ROD' },
    { label: 'Shank', value: 'SHANK' },
    { label: 'Engine', value: 'ENGINE' },
    { label: 'Hydraulic System', value: 'HYDRAULIC_SYSTEM' },
    { label: 'Electrical System', value: 'ELECTRICAL_SYSTEM' },
    { label: 'Mechanical Components', value: 'MECHANICAL_COMPONENTS' },
    { label: 'Other', value: 'OTHER' }
  ]);

  problemCategoryOptions = computed(() => [
    { label: 'Engine Issues', value: 'ENGINE_ISSUES' },
    { label: 'Hydraulic Problems', value: 'HYDRAULIC_PROBLEMS' },
    { label: 'Electrical Faults', value: 'ELECTRICAL_FAULTS' },
    { label: 'Mechanical Breakdown', value: 'MECHANICAL_BREAKDOWN' },
    { label: 'Drill Bit Issues', value: 'DRILL_BIT_ISSUES' },
    { label: 'Drill Rod Problems', value: 'DRILL_ROD_PROBLEMS' },
    { label: 'Other', value: 'OTHER' }
  ]);

  severityOptions = computed(() => [
    { label: 'Low - Maintenance Needed', value: 'LOW' },
    { label: 'Medium - Minor Issues', value: 'MEDIUM' },
    { label: 'High - Performance Issues', value: 'HIGH' },
    { label: 'Critical - Machine Down', value: 'CRITICAL' }
  ]);
  
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
    this.showReportForm = !this.showReportForm;
  }

  onSearch() {
    // Search is handled by the filteredReports computed signal
    // This method exists to trigger change detection on input events
  }

  async submitReport() {
    const currentUser = this.authService.getCurrentUser();
    const machine = this.operatorMachine();

    if (!currentUser || !machine) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Unable to submit report. Please try again.',
        life: 3000
      });
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
      this.messageService.add({
        severity: 'success',
        summary: 'Success',
        detail: 'Report submitted successfully',
        life: 3000
      });
      this.resetReportForm();
      this.showReportForm = false;
    } catch (error: any) {
      console.error('Failed to submit report:', error);
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Failed to submit report. Please try again.',
        life: 3000
      });
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
      'DRILL_BIT': 'pi-wrench',
      'DRILL_ROD': 'pi-bolt',
      'SHANK': 'pi-cog',
      'ENGINE': 'pi-cog',
      'HYDRAULIC_SYSTEM': 'pi-filter',
      'ELECTRICAL_SYSTEM': 'pi-bolt',
      'MECHANICAL_COMPONENTS': 'pi-cog',
      'OTHER': 'pi-question-circle'
    };
    return iconMap[part] || 'pi-question-circle';
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
      'LOW': 'pi-info-circle',
      'MEDIUM': 'pi-exclamation-triangle',
      'HIGH': 'pi-exclamation-circle',
      'CRITICAL': 'pi-times-circle'
    };
    return iconMap[severity] || 'pi-info-circle';
  }

  getSeveritySeverity(severity: string): 'success' | 'info' | 'secondary' | 'contrast' | 'warning' | 'danger' {
    const severityMap: { [key: string]: 'success' | 'info' | 'secondary' | 'contrast' | 'warning' | 'danger' } = {
      'LOW': 'success',
      'MEDIUM': 'warning',
      'HIGH': 'warning',
      'CRITICAL': 'danger'
    };
    return severityMap[severity] || 'info';
  }

  getStatusIcon(status: string): string {
    const iconMap: { [key: string]: string } = {
      'REPORTED': 'pi-clock',
      'ACKNOWLEDGED': 'pi-eye',
      'IN_PROGRESS': 'pi-sync',
      'RESOLVED': 'pi-check-circle',
      'CLOSED': 'pi-check'
    };
    return iconMap[status] || 'pi-question-circle';
  }

  getStatusSeverity(status: string): 'success' | 'info' | 'secondary' | 'contrast' | 'warning' | 'danger' {
    const severityMap: { [key: string]: 'success' | 'info' | 'secondary' | 'contrast' | 'warning' | 'danger' } = {
      'REPORTED': 'warning',
      'ACKNOWLEDGED': 'info',
      'IN_PROGRESS': 'info',
      'RESOLVED': 'success',
      'CLOSED': 'success'
    };
    return severityMap[status] || 'secondary';
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
    this.messageService.add({
      severity: 'info',
      summary: 'Report Details',
      detail: `Viewing: ${report.ticketId}`,
      life: 2000
    });
  }
}