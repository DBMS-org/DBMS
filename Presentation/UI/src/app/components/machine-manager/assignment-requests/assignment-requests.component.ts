import { Component, OnInit, OnDestroy, ElementRef, Renderer2 } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { NotificationService } from '../../../core/services/notification.service';
import { MachineService } from '../../../core/services/machine.service';
import { MachineAssignmentRequest, Machine as GlobalMachine } from '../../../core/models/machine.model';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { TagModule } from 'primeng/tag';
import { PanelModule } from 'primeng/panel';
import { TooltipModule } from 'primeng/tooltip';
import { MessageModule } from 'primeng/message';
import { MatIconModule } from '@angular/material/icon';

export interface RequestStatistics {
  pending: number;
  approved: number;
  rejected: number;
  total: number;
}

@Component({
  selector: 'app-assignment-requests',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    DropdownModule,
    TagModule,
    PanelModule,
    TooltipModule,
    MessageModule,
    MatIconModule
  ],
  templateUrl: './assignment-requests.component.html',
  styleUrl: './assignment-requests.component.scss'
})
export class AssignmentRequestsComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  // Data properties
  assignmentRequests: MachineAssignmentRequest[] = [];
  filteredRequests: MachineAssignmentRequest[] = [];
  // Add displayed slice for pagination
  displayedRequests: MachineAssignmentRequest[] = [];
  availableMachines: GlobalMachine[] = [];
  statistics: RequestStatistics = {
    pending: 0,
    approved: 0,
    rejected: 0,
    total: 0
  };

  // Filter properties
  selectedStatus: string = 'ALL';
  selectedUrgency: string = 'ALL';
  selectedMachineType: string = 'ALL';
  // Free-text search
  searchQuery: string = '';
  // Filter expansion state
  isFilterExpanded: boolean = true;

  // Sorting and pagination
  sortBy: 'requestedDate' | 'urgency' | 'status' | 'machineType' | 'projectName' | 'requestedBy' | 'id' = 'requestedDate';
  sortDirection: 'asc' | 'desc' = 'desc';
  pageSize: number = 10;
  currentPage: number = 1;
  pageNumbers: number[] = [];

  // Modal properties
  // selectedRequest property removed as view details functionality is no longer used
  requestToApprove: MachineAssignmentRequest | null = null;
  requestToReject: MachineAssignmentRequest | null = null;

  // Form properties
  approvalNotes: string = '';
  rejectionReason: string = '';
  selectedMachinesForAssignment: number[] = [];

  // State properties
  isLoading: boolean = false;
  isProcessing: boolean = false;
  error: string | null = null;

  constructor(
    private notificationService: NotificationService,
    private elementRef: ElementRef,
    private renderer: Renderer2,
    private machineService: MachineService
  ) {}

  ngOnInit(): void {
    this.loadAssignmentRequests();
    this.loadAvailableMachines();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // Data loading methods
  loadAssignmentRequests(): void {
    this.isLoading = true;
    this.error = null;

    this.machineService.getAllAssignmentRequests()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (requests) => {
          this.assignmentRequests = requests;
          this.calculateStatistics();
          this.applyAll();
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading assignment requests:', error);
          this.error = 'Failed to load assignment requests';
          this.isLoading = false;
          this.notificationService.showError('Failed to load assignment requests');
        }
      });
  }

  loadAvailableMachines(): void {
    this.machineService.getAllMachines()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (machines) => {
          this.availableMachines = machines.filter(m => m.status === 'Available');
        },
        error: (error) => {
          console.error('Error loading machines:', error);
        }
      });
  }

  refreshRequests(): void {
    this.loadAssignmentRequests();
    this.notificationService.showSuccess('Assignment requests refreshed successfully');
  }

  exportRequests(): void {
    // In a real application, this would generate and download a CSV/Excel file
    const csvContent = this.generateCSVContent();
    const blob = new Blob([csvContent], { type: 'text/csv' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `assignment-requests-${new Date().toISOString().split('T')[0]}.csv`;
    link.click();
    window.URL.revokeObjectURL(url);
    this.notificationService.showSuccess('Assignment requests exported successfully');
  }

  private generateCSVContent(): string {
    const headers = ['ID', 'Date', 'Requester', 'Project', 'Machine Type', 'Quantity', 'Urgency', 'Status'];
    const rows = this.filteredRequests.map(request => [
      request.id,
      this.formatDate(request.requestedDate),
      request.requestedBy,
      request.projectName || '',
      request.machineType,
      request.quantity.toString(),
      request.urgency,
      request.status
    ]);

    return [headers, ...rows].map(row => row.map(cell => `"${cell}"`).join(',')).join('\n');
  }

  // Filter methods
  applyFilters(): void {
    this.filteredRequests = this.assignmentRequests.filter(request => {
      const statusMatch = this.selectedStatus === 'ALL' || request.status === this.selectedStatus;
      const urgencyMatch = this.selectedUrgency === 'ALL' || request.urgency === this.selectedUrgency;
      const typeMatch = this.selectedMachineType === 'ALL' || request.machineType === this.selectedMachineType;
      const search = this.searchQuery.trim().toLowerCase();
      const searchMatch = !search || (
        request.id.toString().includes(search) ||
        request.requestedBy.toLowerCase().includes(search) ||
        (request.projectName || '').toLowerCase().includes(search) ||
        request.projectId.toString().includes(search) ||
        (typeof request.machineType === 'string' ? request.machineType : '').toLowerCase().includes(search)
      );
      return statusMatch && urgencyMatch && typeMatch && searchMatch;
    });
  }

  applyAll(): void {
    this.applyFilters();
    this.sortRequests();
    this.updatePagination();
    this.updateDisplayedRequests();
  }

  sortRequests(): void {
    const dir = this.sortDirection === 'asc' ? 1 : -1;
    this.filteredRequests.sort((a, b) => {
      let av: any;
      let bv: any;
      switch (this.sortBy) {
        case 'requestedDate':
          av = new Date(a.requestedDate).getTime();
          bv = new Date(b.requestedDate).getTime();
          break;
        case 'urgency':
          // Handle string urgency values
          const urgencyStr = (val: string | any) => typeof val === 'string' ? val.toUpperCase() : '';
          const order: Record<string, number> = { LOW: 1, MEDIUM: 2, HIGH: 3, URGENT: 4, CRITICAL: 5 };
          av = order[urgencyStr(a.urgency)] || 0;
          bv = order[urgencyStr(b.urgency)] || 0;
          break;
        case 'status':
          // Handle string status values
          const statusStr = (val: string | any) => typeof val === 'string' ? val.toUpperCase() : '';
          const sOrder: Record<string, number> = { PENDING: 1, APPROVED: 2, REJECTED: 3, PARTIALLYFULFILLED: 4, COMPLETED: 5, CANCELLED: 6 };
          av = sOrder[statusStr(a.status)] || 0;
          bv = sOrder[statusStr(b.status)] || 0;
          break;
        case 'machineType':
          av = (typeof a.machineType === 'string' ? a.machineType : '').toLowerCase();
          bv = (typeof b.machineType === 'string' ? b.machineType : '').toLowerCase();
          break;
        case 'projectName':
          av = (a.projectName || '').toLowerCase();
          bv = (b.projectName || '').toLowerCase();
          break;
        case 'requestedBy':
          av = a.requestedBy.toLowerCase();
          bv = b.requestedBy.toLowerCase();
          break;
        case 'id':
          av = a.id;
          bv = b.id;
          break;
        default:
          av = 0; bv = 0;
      }
      if (av < bv) return -1 * dir;
      if (av > bv) return 1 * dir;
      return 0;
    });
  }

  // Computed total pages for pagination controls
  get totalPages(): number {
    return Math.max(1, Math.ceil(this.filteredRequests.length / this.pageSize));
  }

  updatePagination(): void {
    const totalPages = this.totalPages;
    if (this.currentPage > totalPages) {
      this.currentPage = totalPages;
    }
    if (this.currentPage < 1) {
      this.currentPage = 1;
    }
    // Build page numbers array for template ngFor
    this.pageNumbers = Array.from({ length: totalPages }, (_, i) => i + 1);
  }

  updateDisplayedRequests(): void {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.displayedRequests = this.filteredRequests.slice(start, end);
  }

  setSort(field: typeof this.sortBy): void {
    if (this.sortBy === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortBy = field;
      this.sortDirection = 'asc';
    }
    this.sortRequests();
    this.updateDisplayedRequests();
  }

  toggleFilterExpansion(): void {
    this.isFilterExpanded = !this.isFilterExpanded;
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.updateDisplayedRequests();
  }

  setPageSize(size: number): void {
    this.pageSize = size;
    this.currentPage = 1;
    this.updatePagination();
    this.updateDisplayedRequests();
  }

  clearFilters(): void {
    this.selectedStatus = 'ALL';
    this.selectedUrgency = 'ALL';
    this.selectedMachineType = 'ALL';
    this.searchQuery = '';
    this.currentPage = 1;
    this.applyAll();
  }

  // Removed exportCSV functionality as per request
  getRowClass(request: MachineAssignmentRequest): string {
    switch (request.urgency) {
      case 'URGENT': return 'urgent-row';
      case 'HIGH': return 'high-row';
      case 'MEDIUM': return 'medium-row';
      default: return 'low-row';
    }
  }

  getRequestAgeLabel(request: MachineAssignmentRequest): string {
    const now = new Date().getTime();
    const then = new Date(request.requestedDate).getTime();
    let diff = Math.max(0, Math.floor((now - then) / 1000)); // seconds
    const days = Math.floor(diff / 86400); diff %= 86400;
    const hours = Math.floor(diff / 3600); diff %= 3600;
    const minutes = Math.floor(diff / 60);
    if (days > 0) return `${days}d ${hours}h`;
    if (hours > 0) return `${hours}h ${minutes}m`;
    return `${minutes}m`;
  }

  // Statistics calculation
  calculateStatistics(): void {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const statusStr = (val: string | any) => typeof val === 'string' ? val.toUpperCase() : val.toString().toUpperCase();

    this.statistics = {
      pending: this.assignmentRequests.filter(r => statusStr(r.status) === 'PENDING').length,
      approved: this.assignmentRequests.filter(r =>
        statusStr(r.status) === 'APPROVED' || statusStr(r.status) === 'PARTIALLYFULFILLED'
      ).length,
      rejected: this.assignmentRequests.filter(r =>
        statusStr(r.status) === 'REJECTED'
      ).length,
      total: this.assignmentRequests.length
    };
  }

  // Modal methods
  // Removed viewRequestDetails method as the eye/view details action has been removed
  approveRequest(request: MachineAssignmentRequest): void {
    // Get machineType as string
    const machineType = typeof request.machineType === 'string' ? request.machineType : '';
    // Auto-assign the first N available machines matching the request type
    const available = this.getAvailableMachinesForType(machineType);

    if (available.length < request.quantity) {
      this.notificationService.showWarning('Not enough available machines to fulfill this request.');
      return;
    }

    this.isProcessing = true;

    const assignedIds = available.slice(0, request.quantity).map(m => m.id);

    // Call backend to approve request
    this.machineService.approveAssignmentRequest(request.id, assignedIds, this.approvalNotes || '')
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (resp) => {
          // Reload data from backend
          this.loadAssignmentRequests();
          this.loadAvailableMachines();

          this.notificationService.showSuccess('Assignment request approved successfully');
          this.isProcessing = false;
        },
        error: (err) => {
          console.error('Failed to approve assignment request', err);
          this.notificationService.showError('Failed to approve assignment request');
          this.isProcessing = false;
        }
      });
  }

  rejectRequest(request: MachineAssignmentRequest): void {
    this.isProcessing = true;
    const comments = 'Rejected by manager';

    // Call backend to reject request
    this.machineService.rejectAssignmentRequest(request.id, comments)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (resp) => {
          // Reload data from backend
          this.loadAssignmentRequests();

          this.notificationService.showSuccess('Assignment request rejected');
          this.isProcessing = false;
        },
        error: (err) => {
          console.error('Failed to reject assignment request', err);
          this.notificationService.showError('Failed to reject assignment request');
          this.isProcessing = false;
        }
      });
  }

  closeModals(): void {
    // Removed requestDetailsModal hide since details modal was removed
    this.triggerModal('approveRequestModal', 'hide');
    this.triggerModal('rejectRequestModal', 'hide');

    // Reset properties after a short delay to allow modals to close gracefully
    setTimeout(() => {
      // Removed selectedRequest reset since it no longer exists
      this.requestToApprove = null;
      this.requestToReject = null;
      this.approvalNotes = '';
      this.rejectionReason = ''
      this.selectedMachinesForAssignment = [];
    }, 300);
  }

  private triggerModal(modalId: string, action: 'show' | 'hide'): void {
    const modalElement = this.elementRef.nativeElement.querySelector(`#${modalId}`);
    if (modalElement) {
      // Using Bootstrap's native JavaScript API
      const bootstrap = (window as any).bootstrap;
      if (bootstrap) {
        const modal = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
        if (action === 'show') {
          modal.show();
        } else {
          modal.hide();
        }
      }
    }
  }

  // Machine selection methods
  getAvailableMachinesForType(machineType: string): GlobalMachine[] {
    return this.availableMachines.filter(machine =>
      machine.type === machineType && machine.status === 'Available'
    );
  }

  isMachineSelected(machineId: number): boolean {
    return this.selectedMachinesForAssignment.includes(machineId);
  }

  toggleMachineSelection(machineId: number): void {
    const index = this.selectedMachinesForAssignment.indexOf(machineId);
    if (index > -1) {
      this.selectedMachinesForAssignment.splice(index, 1);
    } else if (this.requestToApprove && this.selectedMachinesForAssignment.length < this.requestToApprove.quantity) {
      this.selectedMachinesForAssignment.push(machineId);
    }
  }

  // Request processing methods
  confirmApproval(): void {
    if (!this.requestToApprove || this.selectedMachinesForAssignment.length === 0) {
      return;
    }

    this.isProcessing = true;

    // Call backend to approve request with selected machines
    this.machineService.approveAssignmentRequest(this.requestToApprove.id, [...this.selectedMachinesForAssignment], this.approvalNotes || '')
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          // Reload data from backend
          this.loadAssignmentRequests();
          this.loadAvailableMachines();

          this.notificationService.showSuccess('Assignment request approved successfully');
          this.isProcessing = false;
          this.closeModals();
        },
        error: (err) => {
          console.error('Failed to approve assignment request', err);
          this.notificationService.showError('Failed to approve assignment request');
          this.isProcessing = false;
        }
      });
  }

  confirmRejection(): void {
    if (!this.requestToReject || !this.rejectionReason.trim()) {
      return;
    }

    this.isProcessing = true;

    // Call backend to reject request with reason
    this.machineService.rejectAssignmentRequest(this.requestToReject.id, this.rejectionReason)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          // Reload data from backend
          this.loadAssignmentRequests();

          this.notificationService.showSuccess('Assignment request rejected');
          this.isProcessing = false;
          this.closeModals();
        },
        error: (err) => {
          console.error('Failed to reject assignment request', err);
          this.notificationService.showError('Failed to reject assignment request');
          this.isProcessing = false;
        }
      });
  }

  // Notification methods
  private sendAssignmentNotifications(request: MachineAssignmentRequest, machineIds: number[]): void {
    // In real implementation, this would call a notification service
    console.log('Sending assignment notifications:', {
      requester: request.requestedBy,
      machines: machineIds,
      project: request.projectName
    });
  }

  private sendRejectionNotification(request: MachineAssignmentRequest, reason: string): void {
    // In real implementation, this would call a notification service
    console.log('Sending rejection notification:', {
      requester: request.requestedBy,
      reason: reason,
      project: request.projectName
    });
  }

  // Utility methods
  getStatusClass(status: string): string {
    switch (status) {
      case 'PENDING': return 'status-pending';
      case 'APPROVED': return 'status-approved';
      case 'REJECTED': return 'status-rejected';
      default: return '';
    }
  }

  getUrgencyClass(urgency: string): string {
    switch (urgency) {
      case 'LOW': return 'urgency-low';
      case 'MEDIUM': return 'urgency-medium';
      case 'HIGH': return 'urgency-high';
      case 'URGENT': return 'urgency-urgent';
      default: return '';
    }
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  // Mock data generation methods (deprecated - using real API now)
  private generateMockRequests(): MachineAssignmentRequest[] {
    // No longer used - data comes from API
    return [];
    /* const mockRequests: MachineAssignmentRequest[] = [
      {
        id: 'REQ-001',
        requesterName: 'John Smith',
        requesterEmail: 'john.smith@company.com',
        projectId: 'PROJ-2024-001',
        projectName: 'Highway Construction Phase 1',
        machineType: 'EXCAVATOR',
        quantity: 2,
        urgency: 'HIGH',
        status: 'PENDING',
        description: 'Need 2 excavators for foundation work starting next week',
        requestedAt: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000)
      },
      {
        id: 'REQ-002',
        requesterName: 'Sarah Johnson',
        requesterEmail: 'sarah.johnson@company.com',
        projectId: 'PROJ-2024-002',
        projectName: 'Building Complex Development',
        machineType: 'CRANE',
        quantity: 1,
        urgency: 'URGENT',
        status: 'PENDING',
        description: 'Urgent need for crane for high-rise construction',
        requestedAt: new Date(Date.now() - 1 * 24 * 60 * 60 * 1000)
      },
      {
        id: 'REQ-003',
        requesterName: 'Mike Wilson',
        requesterEmail: 'mike.wilson@company.com',
        projectId: 'PROJ-2024-003',
        projectName: 'Road Maintenance Project',
        machineType: 'BULLDOZER',
        quantity: 1,
        urgency: 'MEDIUM',
        status: 'APPROVED',
        description: 'Road clearing and leveling work',
        requestedAt: new Date(Date.now() - 3 * 24 * 60 * 60 * 1000),
        processedAt: new Date(Date.now() - 1 * 24 * 60 * 60 * 1000),
        processedBy: 'Machine Manager',
        assignedMachines: ['MACH-001'],
        approvalNotes: 'Approved for immediate deployment'
      },
      {
        id: 'REQ-004',
        requesterName: 'Lisa Brown',
        requesterEmail: 'lisa.brown@company.com',
        projectId: 'PROJ-2024-004',
        projectName: 'Mining Operation Expansion',
        machineType: 'DUMP_TRUCK',
        quantity: 3,
        urgency: 'LOW',
        status: 'REJECTED',
        description: 'Need dump trucks for material transport',
        requestedAt: new Date(Date.now() - 5 * 24 * 60 * 60 * 1000),
        processedAt: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000),
        processedBy: 'Machine Manager',
        rejectionReason: 'All dump trucks currently assigned to higher priority projects'
      }
    ];

    return mockRequests; */
  }

  private generateMockMachines(): GlobalMachine[] {
    // No longer used - data comes from API
    return [];
    /* const mockMachines: GlobalMachine[] = [
      {
        id: 'MACH-001',
        name: 'CAT 320D Excavator',
        model: '320D',
        serialNumber: 'CAT320D001',
        type: 'EXCAVATOR',
        status: 'AVAILABLE',
        currentLocation: 'Warehouse A',
        rigNo: 'RIG-001',
        plateNo: 'ABC-123',
        company: 'Caterpillar'
      },
      {
        id: 'MACH-002',
        name: 'CAT 330D Excavator',
        model: '330D',
        serialNumber: 'CAT330D001',
        type: 'EXCAVATOR',
        status: 'AVAILABLE',
        currentLocation: 'Warehouse B',
        rigNo: 'RIG-002',
        plateNo: 'DEF-456',
        company: 'Caterpillar'
      },
      {
        id: 'MACH-003',
        name: 'Liebherr LTM 1050',
        model: 'LTM 1050',
        serialNumber: 'LIE1050001',
        type: 'CRANE',
        status: 'AVAILABLE',
        currentLocation: 'Site C',
        rigNo: 'RIG-003',
        plateNo: 'GHI-789',
        company: 'Liebherr'
      },
      {
        id: 'MACH-004',
        name: 'CAT D6T Bulldozer',
        model: 'D6T',
        serialNumber: 'CATD6T001',
        type: 'BULLDOZER',
        status: 'ASSIGNED',
        currentLocation: 'Project Site 1',
        rigNo: 'RIG-004',
        plateNo: 'JKL-012',
        company: 'Caterpillar'
      }
    ];

    return mockMachines; */
  }

}
