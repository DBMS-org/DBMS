import { Component, OnInit, OnDestroy, ElementRef, Renderer2 } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { NotificationService } from '../../../core/services/notification.service';
import { MachineService } from '../../../core/services/machine.service';

export interface AssignmentRequest {
  id: string;
  requesterName: string;
  requesterEmail: string;
  projectId: string;
  projectName: string;
  machineType: string;
  quantity: number;
  urgency: 'LOW' | 'MEDIUM' | 'HIGH' | 'URGENT';
  status: 'PENDING' | 'APPROVED' | 'REJECTED';
  description?: string;
  requestedAt: Date;
  processedAt?: Date;
  processedBy?: string;
  rejectionReason?: string;
  assignedMachines?: string[];
  approvalNotes?: string;
}

export interface Machine {
  id: string;
  name: string;
  model: string;
  serialNumber: string;
  type: string;
  status: 'AVAILABLE' | 'ASSIGNED' | 'UNDER_MAINTENANCE';
  currentLocation?: string;
  rigNo?: string;
  plateNo?: string;
  company?: string;
}

export interface RequestStatistics {
  pending: number;
  approved: number;
  rejected: number;
  total: number;
}

@Component({
  selector: 'app-assignment-requests',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './assignment-requests.component.html',
  styleUrl: './assignment-requests.component.scss'
})
export class AssignmentRequestsComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  // Data properties
  assignmentRequests: AssignmentRequest[] = [];
  filteredRequests: AssignmentRequest[] = [];
  // Add displayed slice for pagination
  displayedRequests: AssignmentRequest[] = [];
  availableMachines: Machine[] = [];
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

  // Sorting and pagination
  sortBy: 'requestedAt' | 'urgency' | 'status' | 'machineType' | 'projectName' | 'requesterName' | 'id' = 'requestedAt';
  sortDirection: 'asc' | 'desc' = 'desc';
  pageSize: number = 10;
  currentPage: number = 1;
  pageNumbers: number[] = [];

  // Modal properties
  // selectedRequest property removed as view details functionality is no longer used
  requestToApprove: AssignmentRequest | null = null;
  requestToReject: AssignmentRequest | null = null;

  // Form properties
  approvalNotes: string = '';
  rejectionReason: string = '';
  selectedMachinesForAssignment: string[] = [];

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

    // Simulate API call with mock data
    setTimeout(() => {
      this.assignmentRequests = this.generateMockRequests();
      this.calculateStatistics();
      this.applyAll();
      this.isLoading = false;
    }, 1000);
  }

  loadAvailableMachines(): void {
    // Simulate API call with mock data
    this.availableMachines = this.generateMockMachines();
  }

  refreshRequests(): void {
    this.loadAssignmentRequests();
    this.notificationService.showSuccess('Assignment requests refreshed successfully');
  }

  // Filter methods
  applyFilters(): void {
    this.filteredRequests = this.assignmentRequests.filter(request => {
      const statusMatch = this.selectedStatus === 'ALL' || request.status === this.selectedStatus;
      const urgencyMatch = this.selectedUrgency === 'ALL' || request.urgency === this.selectedUrgency;
      const typeMatch = this.selectedMachineType === 'ALL' || request.machineType === this.selectedMachineType;
      const search = this.searchQuery.trim().toLowerCase();
      const searchMatch = !search || (
        request.id.toLowerCase().includes(search) ||
        request.requesterName.toLowerCase().includes(search) ||
        (request.requesterEmail || '').toLowerCase().includes(search) ||
        request.projectName.toLowerCase().includes(search) ||
        request.projectId.toLowerCase().includes(search) ||
        request.machineType.toLowerCase().includes(search)
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
        case 'requestedAt':
          av = new Date(a.requestedAt).getTime();
          bv = new Date(b.requestedAt).getTime();
          break;
        case 'urgency':
          const order = { LOW: 1, MEDIUM: 2, HIGH: 3, URGENT: 4 } as const;
          av = order[a.urgency];
          bv = order[b.urgency];
          break;
        case 'status':
          const sOrder = { PENDING: 1, APPROVED: 2, REJECTED: 3 } as const;
          av = sOrder[a.status];
          bv = sOrder[b.status];
          break;
        case 'machineType':
          av = a.machineType.toLowerCase();
          bv = b.machineType.toLowerCase();
          break;
        case 'projectName':
          av = a.projectName.toLowerCase();
          bv = b.projectName.toLowerCase();
          break;
        case 'requesterName':
          av = a.requesterName.toLowerCase();
          bv = b.requesterName.toLowerCase();
          break;
        case 'id':
          av = a.id.toLowerCase();
          bv = b.id.toLowerCase();
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
  getRowClass(request: AssignmentRequest): string {
    switch (request.urgency) {
      case 'URGENT': return 'urgent-row';
      case 'HIGH': return 'high-row';
      case 'MEDIUM': return 'medium-row';
      default: return 'low-row';
    }
  }

  getRequestAgeLabel(request: AssignmentRequest): string {
    const now = new Date().getTime();
    const then = new Date(request.requestedAt).getTime();
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

    this.statistics = {
      pending: this.assignmentRequests.filter(r => r.status === 'PENDING').length,
      approved: this.assignmentRequests.filter(r => 
        r.status === 'APPROVED' && 
        r.processedAt && 
        new Date(r.processedAt) >= today
      ).length,
      rejected: this.assignmentRequests.filter(r => 
        r.status === 'REJECTED' && 
        r.processedAt && 
        new Date(r.processedAt) >= today
      ).length,
      total: this.assignmentRequests.length
    };
  }

  // Modal methods
  // Removed viewRequestDetails method as the eye/view details action has been removed
  approveRequest(request: AssignmentRequest): void {
    // Auto-assign the first N available machines matching the request type
    const available = this.getAvailableMachinesForType(request.machineType);

    if (available.length < request.quantity) {
      this.notificationService.showWarning('Not enough available machines to fulfill this request.');
      return;
    }

    this.isProcessing = true;

    const assignedIds = available.slice(0, request.quantity).map(m => m.id);

    // Call backend to approve request
    this.machineService.approveAssignmentRequest(request.id, assignedIds, this.approvalNotes || '')
      .subscribe({
        next: (resp) => {
          const requestIndex = this.assignmentRequests.findIndex(r => r.id === request.id);
          if (requestIndex > -1) {
            this.assignmentRequests[requestIndex] = {
              ...this.assignmentRequests[requestIndex],
              status: 'APPROVED',
              processedAt: new Date(),
              processedBy: 'Machine Manager',
              assignedMachines: assignedIds,
              approvalNotes: this.approvalNotes || ''
            };
          }

          // Update machine statuses in local list
          assignedIds.forEach(machineId => {
            const machine = this.availableMachines.find(m => m.id === machineId);
            if (machine) {
              machine.status = 'ASSIGNED';
            }
          });

          this.calculateStatistics();
          this.applyAll();
          this.notificationService.showSuccess('Assignment request approved successfully');
          this.sendAssignmentNotifications(request, assignedIds);
          this.isProcessing = false;
        },
        error: (err) => {
          console.error('Failed to approve assignment request', err);
          this.notificationService.showError('Failed to approve assignment request');
          this.isProcessing = false;
        }
      });
  }

  rejectRequest(request: AssignmentRequest): void {
    this.isProcessing = true;
    const comments = 'Rejected by manager';

    // Call backend to reject request
    this.machineService.rejectAssignmentRequest(request.id, comments)
      .subscribe({
        next: (resp) => {
          const requestIndex = this.assignmentRequests.findIndex(r => r.id === request.id);
          if (requestIndex > -1) {
            this.assignmentRequests[requestIndex] = {
              ...this.assignmentRequests[requestIndex],
              status: 'REJECTED',
              processedAt: new Date(),
              processedBy: 'Machine Manager',
              rejectionReason: comments
            };
          }

          this.calculateStatistics();
          this.applyAll();
          this.notificationService.showSuccess('Assignment request rejected');
          this.sendRejectionNotification(request, comments);
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
  getAvailableMachinesForType(machineType: string): Machine[] {
    return this.availableMachines.filter(machine => 
      machine.type === machineType && machine.status === 'AVAILABLE'
    );
  }

  isMachineSelected(machineId: string): boolean {
    return this.selectedMachinesForAssignment.includes(machineId);
  }

  toggleMachineSelection(machineId: string): void {
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
      .subscribe({
        next: () => {
          const requestIndex = this.assignmentRequests.findIndex(r => r.id === this.requestToApprove!.id);
          if (requestIndex > -1) {
            this.assignmentRequests[requestIndex] = {
              ...this.assignmentRequests[requestIndex],
              status: 'APPROVED',
              processedAt: new Date(),
              processedBy: 'Machine Manager',
              assignedMachines: [...this.selectedMachinesForAssignment],
              approvalNotes: this.approvalNotes
            };

            // Update machine statuses
            this.selectedMachinesForAssignment.forEach(machineId => {
              const machine = this.availableMachines.find(m => m.id === machineId);
              if (machine) {
                machine.status = 'ASSIGNED';
              }
            });

            this.calculateStatistics();
            this.applyAll();
            this.notificationService.showSuccess('Assignment request approved successfully');
            if (this.requestToApprove) {
              this.sendAssignmentNotifications(this.requestToApprove, this.selectedMachinesForAssignment);
            }
          }

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
      .subscribe({
        next: () => {
          const requestIndex = this.assignmentRequests.findIndex(r => r.id === this.requestToReject!.id);
          if (requestIndex > -1) {
            this.assignmentRequests[requestIndex] = {
              ...this.assignmentRequests[requestIndex],
              status: 'REJECTED',
              processedAt: new Date(),
              processedBy: 'Machine Manager',
              rejectionReason: this.rejectionReason
            };

            this.calculateStatistics();
            this.applyAll();
            this.notificationService.showSuccess('Assignment request rejected');
            if (this.requestToReject) {
              this.sendRejectionNotification(this.requestToReject, this.rejectionReason);
            }
          }

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
  private sendAssignmentNotifications(request: AssignmentRequest, machineIds: string[]): void {
    // In real implementation, this would call a notification service
    console.log('Sending assignment notifications:', {
      requester: request.requesterEmail,
      machines: machineIds,
      project: request.projectName
    });
  }

  private sendRejectionNotification(request: AssignmentRequest, reason: string): void {
    // In real implementation, this would call a notification service
    console.log('Sending rejection notification:', {
      requester: request.requesterEmail,
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

  // Mock data generation methods
  private generateMockRequests(): AssignmentRequest[] {
    const mockRequests: AssignmentRequest[] = [
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

    return mockRequests;
  }

  private generateMockMachines(): Machine[] {
    const mockMachines: Machine[] = [
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

    return mockMachines;
  }

}
