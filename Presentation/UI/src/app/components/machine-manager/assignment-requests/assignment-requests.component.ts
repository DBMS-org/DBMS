import { Component, OnInit, OnDestroy, ElementRef, Renderer2 } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { NotificationService } from '../../../core/services/notification.service';

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

  // Modal properties
  selectedRequest: AssignmentRequest | null = null;
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
    private renderer: Renderer2
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
      this.applyFilters();
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
      
      return statusMatch && urgencyMatch && typeMatch;
    });
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
  viewRequestDetails(request: AssignmentRequest): void {
    this.selectedRequest = request;
    this.triggerModal('requestDetailsModal', 'show');
  }

  approveRequest(request: AssignmentRequest): void {
    this.requestToApprove = request;
    this.selectedMachinesForAssignment = [];
    this.approvalNotes = '';
    this.triggerModal('approveRequestModal', 'show');
  }

  rejectRequest(request: AssignmentRequest): void {
    this.requestToReject = request;
    this.rejectionReason = '';
    this.triggerModal('rejectRequestModal', 'show');
  }

  closeModals(): void {
    this.triggerModal('requestDetailsModal', 'hide');
    this.triggerModal('approveRequestModal', 'hide');
    this.triggerModal('rejectRequestModal', 'hide');

    // Reset properties after a short delay to allow modals to close gracefully
    setTimeout(() => {
      this.selectedRequest = null;
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

    // Simulate API call
    setTimeout(() => {
      const requestIndex = this.assignmentRequests.findIndex(r => r.id === this.requestToApprove!.id);
      if (requestIndex > -1) {
        this.assignmentRequests[requestIndex] = {
          ...this.assignmentRequests[requestIndex],
          status: 'APPROVED',
          processedAt: new Date(),
          processedBy: 'Current Machine Manager', // In real app, get from auth service
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
        this.applyFilters();
        this.notificationService.showSuccess('Assignment request approved successfully');
        
        // In real app, send notification to requester and operators
        if (this.requestToApprove) {
          this.sendAssignmentNotifications(this.requestToApprove, this.selectedMachinesForAssignment);
        }
      }

      this.isProcessing = false;
      this.closeModals();
    }, 1500);
  }

  confirmRejection(): void {
    if (!this.requestToReject || !this.rejectionReason.trim()) {
      return;
    }

    this.isProcessing = true;

    // Simulate API call
    setTimeout(() => {
      const requestIndex = this.assignmentRequests.findIndex(r => r.id === this.requestToReject!.id);
      if (requestIndex > -1) {
        this.assignmentRequests[requestIndex] = {
          ...this.assignmentRequests[requestIndex],
          status: 'REJECTED',
          processedAt: new Date(),
          processedBy: 'Current Machine Manager', // In real app, get from auth service
          rejectionReason: this.rejectionReason
        };

        this.calculateStatistics();
        this.applyFilters();
        this.notificationService.showSuccess('Assignment request rejected');
        
        // In real app, send notification to requester
        if (this.requestToReject) {
          this.sendRejectionNotification(this.requestToReject, this.rejectionReason);
        }
      }

      this.isProcessing = false;
      this.closeModals();
    }, 1000);
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
