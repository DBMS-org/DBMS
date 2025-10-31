import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

// ==========================================
// MACHINE MANAGER REPORT INTERFACES
// ==========================================

interface MachineSummary {
  machineId: number;
  machineName: string;
  machineType: string;
  serialNumber: string;
  status: string;
  currentLocation: string;
  currentProject?: string;
  currentOperator?: string;
  totalUtilizationHours: number;
  lastMaintenanceDate?: Date;
  nextMaintenanceDate?: Date;
  totalMaintenanceCost: number;
  problemReportsCount: number;
  assignmentHistory: MachineAssignment[];
  maintenanceHistory: MaintenanceRecord[];
  usageLogs: UsageLog[];
}

interface MachineAssignment {
  assignmentId: number;
  projectName: string;
  operatorName: string;
  assignedDate: Date;
  returnDate?: Date;
  expectedReturnDate?: Date;
  status: string;
  location: string;
  utilizationHours: number;
}

interface MaintenanceRecord {
  jobId: number;
  type: string;
  scheduledDate: Date;
  completedDate?: Date;
  status: string;
  assignedTo: string;
  estimatedCost: number;
  actualCost?: number;
  partsReplaced: string[];
  downtime: number; // hours
}

interface UsageLog {
  logId: number;
  date: Date;
  operatorName: string;
  projectName: string;
  engineHours: number;
  idleHours: number;
  workingHours: number;
  fuelConsumed: number;
  hasDowntime: boolean;
  downtimeReason?: string;
}

interface MachineStatistics {
  totalMachines: number;
  activeMachines: number;
  idleMachines: number;
  underMaintenanceMachines: number;
  totalAssignments: number;
  activeAssignments: number;
  totalMaintenanceJobs: number;
  completedMaintenanceJobs: number;
  pendingMaintenanceJobs: number;
  totalMaintenanceCost: number;
  totalUtilizationHours: number;
  averageUtilizationRate: number;
  totalProblemReports: number;
  resolvedProblemReports: number;
}

interface ReportFilters {
  machineType?: string;
  status?: string;
  dateFrom?: Date | null;
  dateTo?: Date | null;
}

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private snackBar = inject(MatSnackBar);

  // Component state
  isLoading = signal(false);
  selectedMachine = signal<MachineSummary | null>(null);
  showDetailModal = signal(false);

  // Filter form
  filterForm!: FormGroup;

  // Machine data
  machines = signal<MachineSummary[]>([]);
  filteredMachines = signal<MachineSummary[]>([]);
  statistics = signal<MachineStatistics | null>(null);

  // Expanded machine for accordion
  expandedMachineId: number | null = null;

  // Dropdown options
  machineTypes = ['All Types', 'Drill Rig', 'Excavator', 'Loader', 'Compressor', 'Grader', 'Dozer'];
  statusOptions = ['All Status', 'Active', 'Idle', 'Under Maintenance', 'Out of Service'];

  ngOnInit() {
    this.initializeFilterForm();
    this.loadMachineData();
  }

  private initializeFilterForm() {
    this.filterForm = this.fb.group({
      machineType: [''],
      status: [''],
      dateFrom: [null],
      dateTo: [null]
    });
  }

  private loadMachineData() {
    this.isLoading.set(true);

    // Simulate API call with mock data
    setTimeout(() => {
      const mockMachines = this.generateMockMachineData();
      this.machines.set(mockMachines);
      this.filteredMachines.set(mockMachines);
      this.statistics.set(this.calculateStatistics(mockMachines));
      this.isLoading.set(false);
    }, 1000);
  }

  private generateMockMachineData(): MachineSummary[] {
    const machines: MachineSummary[] = [];
    const types = ['Drill Rig', 'Excavator', 'Loader', 'Compressor'];
    const statuses = ['Active', 'Idle', 'Under Maintenance'];
    const projects = ['Project Alpha', 'Project Beta', 'Project Gamma'];
    const operators = ['John Smith', 'Sarah Johnson', 'Ahmed Al-Balushi', 'Mohammed Al-Rashdi'];

    for (let i = 1; i <= 12; i++) {
      const type = types[i % types.length];
      const status = statuses[i % statuses.length];

      machines.push({
        machineId: i,
        machineName: `${type} ${String.fromCharCode(65 + (i % 26))}-${String(i).padStart(3, '0')}`,
        machineType: type,
        serialNumber: `${type.substring(0, 2).toUpperCase()}-SN-${String(i).padStart(4, '0')}`,
        status: status,
        currentLocation: status === 'Active' ? projects[i % projects.length] : 'Warehouse',
        currentProject: status === 'Active' ? projects[i % projects.length] : undefined,
        currentOperator: status === 'Active' ? operators[i % operators.length] : undefined,
        totalUtilizationHours: 500 + (i * 100),
        lastMaintenanceDate: new Date(2024, 2, i),
        nextMaintenanceDate: new Date(2024, 5, i),
        totalMaintenanceCost: 5000 + (i * 500),
        problemReportsCount: Math.floor(Math.random() * 5),
        assignmentHistory: this.generateMockAssignments(i),
        maintenanceHistory: this.generateMockMaintenance(i),
        usageLogs: this.generateMockUsageLogs(i)
      });
    }

    return machines;
  }

  private generateMockAssignments(machineId: number): MachineAssignment[] {
    const assignments: MachineAssignment[] = [];
    const projects = ['Project Alpha', 'Project Beta', 'Project Gamma'];
    const operators = ['John Smith', 'Sarah Johnson', 'Ahmed Al-Balushi'];

    for (let i = 0; i < 3; i++) {
      assignments.push({
        assignmentId: machineId * 100 + i,
        projectName: projects[i % projects.length],
        operatorName: operators[i % operators.length],
        assignedDate: new Date(2024, i, 15),
        returnDate: i < 2 ? new Date(2024, i + 1, 10) : undefined,
        expectedReturnDate: new Date(2024, i + 1, 15),
        status: i < 2 ? 'Completed' : 'Active',
        location: `Site ${String.fromCharCode(65 + i)}`,
        utilizationHours: 150 + (i * 50)
      });
    }

    return assignments;
  }

  private generateMockMaintenance(machineId: number): MaintenanceRecord[] {
    const maintenance: MaintenanceRecord[] = [];
    const types = ['PREVENTIVE', 'CORRECTIVE', 'EMERGENCY'];
    const engineers = ['Fatima Al-Hinai', 'Maintenance Team A', 'Maintenance Team B'];

    for (let i = 0; i < 4; i++) {
      maintenance.push({
        jobId: machineId * 200 + i,
        type: types[i % types.length],
        scheduledDate: new Date(2024, i * 2, 10),
        completedDate: i < 3 ? new Date(2024, i * 2, 12) : undefined,
        status: i < 3 ? 'Completed' : 'Scheduled',
        assignedTo: engineers[i % engineers.length],
        estimatedCost: 1000 + (i * 500),
        actualCost: i < 3 ? 1200 + (i * 550) : undefined,
        partsReplaced: i < 3 ? ['Oil Filter', 'Air Filter', 'Hydraulic Fluid'] : [],
        downtime: i < 3 ? 8 + (i * 4) : 0
      });
    }

    return maintenance;
  }

  private generateMockUsageLogs(machineId: number): UsageLog[] {
    const logs: UsageLog[] = [];
    const operators = ['John Smith', 'Sarah Johnson', 'Ahmed Al-Balushi'];
    const projects = ['Project Alpha', 'Project Beta', 'Project Gamma'];

    for (let i = 0; i < 7; i++) {
      const date = new Date();
      date.setDate(date.getDate() - i);

      logs.push({
        logId: machineId * 300 + i,
        date: date,
        operatorName: operators[i % operators.length],
        projectName: projects[i % projects.length],
        engineHours: 8 + Math.random() * 4,
        idleHours: 1 + Math.random() * 2,
        workingHours: 6 + Math.random() * 3,
        fuelConsumed: 50 + Math.random() * 30,
        hasDowntime: Math.random() > 0.8,
        downtimeReason: Math.random() > 0.8 ? 'Refueling' : undefined
      });
    }

    return logs;
  }

  private calculateStatistics(machines: MachineSummary[]): MachineStatistics {
    const totalMachines = machines.length;
    const activeMachines = machines.filter(m => m.status === 'Active').length;
    const idleMachines = machines.filter(m => m.status === 'Idle').length;
    const underMaintenanceMachines = machines.filter(m => m.status === 'Under Maintenance').length;

    let totalAssignments = 0;
    let activeAssignments = 0;
    let totalMaintenanceJobs = 0;
    let completedMaintenanceJobs = 0;
    let totalMaintenanceCost = 0;
    let totalUtilizationHours = 0;
    let totalProblemReports = 0;

    machines.forEach(machine => {
      totalAssignments += machine.assignmentHistory.length;
      activeAssignments += machine.assignmentHistory.filter(a => a.status === 'Active').length;
      totalMaintenanceJobs += machine.maintenanceHistory.length;
      completedMaintenanceJobs += machine.maintenanceHistory.filter(m => m.status === 'Completed').length;
      totalMaintenanceCost += machine.totalMaintenanceCost;
      totalUtilizationHours += machine.totalUtilizationHours;
      totalProblemReports += machine.problemReportsCount;
    });

    return {
      totalMachines,
      activeMachines,
      idleMachines,
      underMaintenanceMachines,
      totalAssignments,
      activeAssignments,
      totalMaintenanceJobs,
      completedMaintenanceJobs,
      pendingMaintenanceJobs: totalMaintenanceJobs - completedMaintenanceJobs,
      totalMaintenanceCost,
      totalUtilizationHours,
      averageUtilizationRate: totalMachines > 0 ? (activeMachines / totalMachines) * 100 : 0,
      totalProblemReports,
      resolvedProblemReports: Math.floor(totalProblemReports * 0.7)
    };
  }

  applyFilters() {
    let filtered = [...this.machines()];

    const machineType = this.filterForm.get('machineType')?.value;
    const status = this.filterForm.get('status')?.value;

    if (machineType && machineType !== 'All Types') {
      filtered = filtered.filter(m => m.machineType === machineType);
    }

    if (status && status !== 'All Status') {
      filtered = filtered.filter(m => m.status === status);
    }

    this.filteredMachines.set(filtered);
  }

  resetFilters() {
    this.filterForm.reset();
    this.filteredMachines.set(this.machines());
  }

  toggleMachineExpansion(machineId: number) {
    this.expandedMachineId = this.expandedMachineId === machineId ? null : machineId;
  }

  isMachineExpanded(machineId: number): boolean {
    return this.expandedMachineId === machineId;
  }

  viewDetailedReport(machine: MachineSummary) {
    this.selectedMachine.set(machine);
    this.showDetailModal.set(true);
  }

  closeDetailModal() {
    this.showDetailModal.set(false);
    this.selectedMachine.set(null);
  }

  exportReport(machine: MachineSummary, format: 'pdf' | 'csv' | 'excel') {
    this.snackBar.open(
      `Exporting ${machine.machineName} report as ${format.toUpperCase()}...`,
      'Close',
      {
        duration: 3000,
        panelClass: ['success-snackbar']
      }
    );

    console.log('Exporting report:', {
      machine: machine,
      format: format,
      filters: this.filterForm.value
    });
  }

  printReport(machine: MachineSummary) {
    console.log('Printing report for:', machine.machineName);
    window.print();
  }

  refreshReports() {
    this.loadMachineData();
    this.snackBar.open('Reports refreshed', 'Close', {
      duration: 2000
    });
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'active':
        return 'status-active';
      case 'idle':
        return 'status-idle';
      case 'under maintenance':
        return 'status-maintenance';
      case 'out of service':
        return 'status-outofservice';
      default:
        return 'status-default';
    }
  }

  formatDate(date: Date | undefined): string {
    if (!date) return 'N/A';
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    }).format(date);
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'OMR'
    }).format(amount);
  }

  formatNumber(num: number): string {
    return new Intl.NumberFormat('en-US', {
      maximumFractionDigits: 1
    }).format(num);
  }
}
