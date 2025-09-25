import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MachineService } from '../../../core/services/machine.service';
import { NotificationService } from '../../../core/services/notification.service';
import { 
  MachineAssignment, 
  AssignmentStatus, 
  MachineType 
} from '../../../core/models/machine.model';

export interface MachineAssignmentHistory {
  id: string;
  machineId: string;
  machineName: string;
  machineType: MachineType;
  machineSerialNumber: string;
  projectId: string;
  projectName: string;
  projectLocation: string;
  operatorId: string;
  operatorName: string;
  operatorRole: string;
  assignedBy: string;
  assignedDate: Date;
  expectedReturnDate?: Date;
  actualReturnDate?: Date;
  status: AssignmentStatus;
  location?: string;
  notes?: string;
  createdAt: Date;
  updatedAt: Date;
}

@Component({
  selector: 'app-machine-assignment-history',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './machine-assignment-history.component.html',
  styleUrl: './machine-assignment-history.component.scss'
})
export class MachineAssignmentHistoryComponent implements OnInit, OnDestroy {
  assignmentHistory: MachineAssignmentHistory[] = [];
  filteredHistory: MachineAssignmentHistory[] = [];
  isLoading = false;
  error: string | null = null;
  
  // Filter properties
  searchTerm = '';
  selectedStatus: AssignmentStatus | 'ALL' = 'ALL';
  selectedMachineType: MachineType | 'ALL' = 'ALL';
  selectedDateRange: string = 'ALL';
  
  // Pagination
  currentPage = 1;
  pageSize = 20;
  totalPages = 1;
  
  // Modal states
  showDetailsModal = false;
  showReturnModal = false;
  selectedAssignment: MachineAssignmentHistory | null = null;
  machineToReturn: MachineAssignmentHistory | null = null;
  returnNotes = '';
  isReturning = false;
  
  // Enums for template
  AssignmentStatus = AssignmentStatus;
  MachineType = MachineType;
  Math = Math;
  
  // Statistics
  statistics = {
    totalAssignments: 0,
    activeAssignments: 0,
    completedAssignments: 0,
    overdueAssignments: 0
  };
  
  private subscriptions: Subscription[] = [];

  constructor(
    private machineService: MachineService,
    private notificationService: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadAssignmentHistory();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  private loadAssignmentHistory(): void {
    this.isLoading = true;
    this.error = null;
    
    // Generate mock data for demonstration
    this.assignmentHistory = this.generateMockAssignmentHistory();
    this.applyFilters();
    this.calculateStatistics();
    this.isLoading = false;
    
    // In a real application, you would call the service:
    // const sub = this.machineService.getAssignmentHistory().subscribe({
    //   next: (history) => {
    //     this.assignmentHistory = history;
    //     this.applyFilters();
    //     this.calculateStatistics();
    //     this.isLoading = false;
    //   },
    //   error: (error) => {
    //     this.error = 'Failed to load assignment history';
    //     this.isLoading = false;
    //     console.error('Error loading assignment history:', error);
    //   }
    // });
    // this.subscriptions.push(sub);
  }

  private generateMockAssignmentHistory(): MachineAssignmentHistory[] {
    const mockHistory: MachineAssignmentHistory[] = [];
    const machineNames = ['CAT 320D Excavator', 'Komatsu PC200 Excavator', 'Atlas Copco ROC D7 Drill Rig', 'Volvo L120H Loader', 'Mercedes Actros Truck'];
    const projectNames = ['Al Hajar Mountain Quarry', 'Muscat Infrastructure Project', 'Dhofar Mining Operation', 'Sohar Port Expansion', 'Nizwa Road Construction'];
    const operatorNames = ['Ahmed Al-Rashid', 'Mohammed Al-Balushi', 'Khalid Al-Hinai', 'Salem Al-Mahrouqi', 'Youssef Al-Kindi'];
    const assignedByNames = ['Manager Smith', 'Supervisor Johnson', 'Director Wilson'];
    
    for (let i = 1; i <= 50; i++) {
      const assignedDate = new Date();
      assignedDate.setDate(assignedDate.getDate() - Math.floor(Math.random() * 365));
      
      const expectedReturnDate = new Date(assignedDate);
      expectedReturnDate.setDate(expectedReturnDate.getDate() + Math.floor(Math.random() * 180) + 30);
      
      const isActive = Math.random() > 0.7;
      const actualReturnDate = isActive ? undefined : new Date(expectedReturnDate.getTime() + (Math.random() - 0.5) * 30 * 24 * 60 * 60 * 1000);
      
      const status = isActive ? AssignmentStatus.ACTIVE : 
                    (actualReturnDate && actualReturnDate > expectedReturnDate) ? AssignmentStatus.OVERDUE : 
                    AssignmentStatus.COMPLETED;
      
      mockHistory.push({
        id: `ASG-${String(i).padStart(4, '0')}`,
        machineId: `M${String(i).padStart(3, '0')}`,
        machineName: machineNames[Math.floor(Math.random() * machineNames.length)],
        machineType: Object.values(MachineType)[Math.floor(Math.random() * Object.values(MachineType).length)],
        machineSerialNumber: `SN${String(Math.floor(Math.random() * 100000)).padStart(5, '0')}`,
        projectId: `PRJ-${String(Math.floor(Math.random() * 10) + 1).padStart(3, '0')}`,
        projectName: projectNames[Math.floor(Math.random() * projectNames.length)],
        projectLocation: ['Al Hajar Mountains', 'Muscat Governorate', 'Dhofar Governorate'][Math.floor(Math.random() * 3)],
        operatorId: `OP${String(i).padStart(3, '0')}`,
        operatorName: operatorNames[Math.floor(Math.random() * operatorNames.length)],
        operatorRole: 'Machine Operator',
        assignedBy: assignedByNames[Math.floor(Math.random() * assignedByNames.length)],
        assignedDate,
        expectedReturnDate: Math.random() > 0.2 ? expectedReturnDate : undefined,
        actualReturnDate,
        status,
        location: ['Site A', 'Site B', 'Site C', 'Main Quarry', 'Processing Plant'][Math.floor(Math.random() * 5)],
        notes: Math.random() > 0.7 ? 'Regular assignment for project operations.' : undefined,
        createdAt: assignedDate,
        updatedAt: actualReturnDate || assignedDate
      });
    }
    
    return mockHistory.sort((a, b) => b.assignedDate.getTime() - a.assignedDate.getTime());
  }

  private calculateStatistics(): void {
    this.statistics.totalAssignments = this.assignmentHistory.length;
    this.statistics.activeAssignments = this.assignmentHistory.filter(h => h.status === AssignmentStatus.ACTIVE).length;
    this.statistics.completedAssignments = this.assignmentHistory.filter(h => h.status === AssignmentStatus.COMPLETED).length;
    this.statistics.overdueAssignments = this.assignmentHistory.filter(h => 
      h.status === AssignmentStatus.ACTIVE && 
      h.expectedReturnDate && 
      new Date() > h.expectedReturnDate
    ).length;
  }

  applyFilters(): void {
    let filtered = [...this.assignmentHistory];
    
    // Search filter
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(assignment => 
        assignment.machineName.toLowerCase().includes(term) ||
        assignment.projectName.toLowerCase().includes(term) ||
        assignment.operatorName.toLowerCase().includes(term) ||
        assignment.id.toLowerCase().includes(term) ||
        assignment.machineSerialNumber.toLowerCase().includes(term)
      );
    }
    
    // Status filter
    if (this.selectedStatus !== 'ALL') {
      filtered = filtered.filter(assignment => assignment.status === this.selectedStatus);
    }
    
    // Machine type filter
    if (this.selectedMachineType !== 'ALL') {
      filtered = filtered.filter(assignment => assignment.machineType === this.selectedMachineType);
    }
    
    // Date range filter
    if (this.selectedDateRange !== 'ALL') {
      const now = new Date();
      let startDate = new Date();
      
      switch (this.selectedDateRange) {
        case 'LAST_7_DAYS':
          startDate.setDate(now.getDate() - 7);
          break;
        case 'LAST_30_DAYS':
          startDate.setDate(now.getDate() - 30);
          break;
        case 'LAST_3_MONTHS':
          startDate.setMonth(now.getMonth() - 3);
          break;
        case 'LAST_6_MONTHS':
          startDate.setMonth(now.getMonth() - 6);
          break;
        case 'LAST_YEAR':
          startDate.setFullYear(now.getFullYear() - 1);
          break;
      }
      
      filtered = filtered.filter(assignment => assignment.assignedDate >= startDate);
    }
    
    this.filteredHistory = filtered;
    this.updatePagination();
  }

  private updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredHistory.length / this.pageSize);
    if (this.currentPage > this.totalPages) {
      this.currentPage = 1;
    }
  }

  onSearchChange(): void {
    this.currentPage = 1;
    this.applyFilters();
  }

  onStatusFilterChange(): void {
    this.currentPage = 1;
    this.applyFilters();
  }

  onMachineTypeFilterChange(): void {
    this.currentPage = 1;
    this.applyFilters();
  }

  onDateRangeFilterChange(): void {
    this.currentPage = 1;
    this.applyFilters();
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  refreshHistory(): void {
    this.loadAssignmentHistory();
  }

  exportHistory(): void {
    // Implementation for exporting history to CSV/Excel
    const csvContent = this.generateCSVContent();
    const blob = new Blob([csvContent], { type: 'text/csv' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `machine-assignment-history-${new Date().toISOString().split('T')[0]}.csv`;
    link.click();
    window.URL.revokeObjectURL(url);
    
    this.notificationService.showSuccess('Assignment history exported successfully');
  }

  private generateCSVContent(): string {
    const headers = [
      'Assignment ID', 'Machine Name', 'Machine Type', 'Serial Number',
      'Project Name', 'Project Location', 'Operator Name', 'Operator Role',
      'Assigned By', 'Assigned Date', 'Expected Return', 'Actual Return',
      'Status', 'Duration (Days)', 'Location', 'Notes'
    ];
    
    const rows = this.filteredHistory.map(assignment => [
      assignment.id,
      assignment.machineName,
      assignment.machineType,
      assignment.machineSerialNumber,
      assignment.projectName,
      assignment.projectLocation,
      assignment.operatorName,
      assignment.operatorRole,
      assignment.assignedBy,
      this.formatDate(assignment.assignedDate),
      assignment.expectedReturnDate ? this.formatDate(assignment.expectedReturnDate) : '',
      assignment.actualReturnDate ? this.formatDate(assignment.actualReturnDate) : '',
      assignment.status,
      this.calculateDurationInDays(assignment),
      assignment.location || '',
      assignment.notes || ''
    ]);
    
    return [headers, ...rows].map(row => 
      row.map(cell => `"${String(cell).replace(/"/g, '""')}"`).join(',')
    ).join('\n');
  }

  viewAssignmentDetails(assignment: MachineAssignmentHistory): void {
    this.selectedAssignment = assignment;
    this.showDetailsModal = true;
  }

  viewMachineHistory(machineId: string): void {
    // Navigate to machine-specific history or filter current view
    this.searchTerm = machineId;
    this.applyFilters();
  }

  returnMachine(assignment: MachineAssignmentHistory): void {
    this.machineToReturn = assignment;
    this.returnNotes = '';
    this.showReturnModal = true;
  }

  confirmReturn(): void {
    if (!this.machineToReturn) return;
    
    this.isReturning = true;
    
    // Simulate API call
    setTimeout(() => {
      if (this.machineToReturn) {
        // Update the assignment
        const index = this.assignmentHistory.findIndex(a => a.id === this.machineToReturn!.id);
        if (index !== -1) {
          this.assignmentHistory[index] = {
            ...this.assignmentHistory[index],
            actualReturnDate: new Date(),
            status: AssignmentStatus.COMPLETED,
            notes: this.returnNotes || this.assignmentHistory[index].notes,
            updatedAt: new Date()
          };
        }
        
        this.applyFilters();
        this.calculateStatistics();
        this.notificationService.showSuccess(`Machine ${this.machineToReturn.machineName} returned successfully`);
      }
      
      this.isReturning = false;
      this.closeModals();
    }, 1500);
  }

  closeModals(): void {
    this.showDetailsModal = false;
    this.showReturnModal = false;
    this.selectedAssignment = null;
    this.machineToReturn = null;
    this.returnNotes = '';
  }

  // Utility methods
  formatDate(date: Date): string {
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'short',
      day: '2-digit'
    }).format(date);
  }

  formatTime(date: Date): string {
    return new Intl.DateTimeFormat('en-US', {
      hour: '2-digit',
      minute: '2-digit',
      hour12: true
    }).format(date);
  }

  formatDateTime(date: Date): string {
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'short',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      hour12: true
    }).format(date);
  }

  calculateDuration(assignment: MachineAssignmentHistory): string {
    const endDate = assignment.actualReturnDate || new Date();
    const startDate = assignment.assignedDate;
    const diffTime = Math.abs(endDate.getTime() - startDate.getTime());
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    
    if (diffDays === 1) return '1 day';
    if (diffDays < 30) return `${diffDays} days`;
    if (diffDays < 365) {
      const months = Math.floor(diffDays / 30);
      const remainingDays = diffDays % 30;
      return remainingDays > 0 ? `${months}m ${remainingDays}d` : `${months} months`;
    }
    
    const years = Math.floor(diffDays / 365);
    const remainingDays = diffDays % 365;
    return remainingDays > 0 ? `${years}y ${Math.floor(remainingDays / 30)}m` : `${years} years`;
  }

  private calculateDurationInDays(assignment: MachineAssignmentHistory): number {
    const endDate = assignment.actualReturnDate || new Date();
    const startDate = assignment.assignedDate;
    const diffTime = Math.abs(endDate.getTime() - startDate.getTime());
    return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  }

  getStatusClass(status: AssignmentStatus): string {
    switch (status) {
      case AssignmentStatus.ACTIVE:
        return 'status-active';
      case AssignmentStatus.COMPLETED:
        return 'status-completed';
      case AssignmentStatus.OVERDUE:
        return 'status-overdue';
      case AssignmentStatus.CANCELLED:
        return 'status-cancelled';
      default:
        return 'status-unknown';
    }
  }

  getDurationClass(assignment: MachineAssignmentHistory): string {
    if (assignment.status === AssignmentStatus.ACTIVE && assignment.expectedReturnDate) {
      const now = new Date();
      const daysOverdue = Math.ceil((now.getTime() - assignment.expectedReturnDate.getTime()) / (1000 * 60 * 60 * 24));
      
      if (daysOverdue > 7) return 'duration-critical';
      if (daysOverdue > 0) return 'duration-overdue';
      if (daysOverdue > -7) return 'duration-warning';
    }
    
    return 'duration-normal';
  }

  get assignmentStatusOptions() {
    return Object.values(AssignmentStatus);
  }

  get machineTypeOptions() {
    return Object.values(MachineType);
  }
}