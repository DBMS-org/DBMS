import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MachineService } from '../../../core/services/machine.service';
import { AuthService } from '../../../core/services/auth.service';
import { 
  Machine, 
  MachineAssignmentRequest, 
  AssignmentRequestStatus, 
  RequestUrgency,
  MachineType,
  MachineStatus
} from '../../../core/models/machine.model';

@Component({
  selector: 'app-assignment-requests',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './assignment-requests.component.html',
  styleUrl: './assignment-requests.component.scss'
})
export class AssignmentRequestsComponent implements OnInit, OnDestroy {
  assignmentRequests: MachineAssignmentRequest[] = [];
  filteredRequests: MachineAssignmentRequest[] = [];
  availableMachines: Machine[] = [];
  
  isLoading = false;
  error: string | null = null;
  
  // Filter properties
  searchTerm = '';
  selectedStatus: AssignmentRequestStatus | 'ALL' = 'ALL';
  selectedUrgency: RequestUrgency | 'ALL' = 'ALL';
  selectedMachineType: MachineType | 'ALL' = 'ALL';
  
  // Modal states
  showRequestDetailsModal = false;
  showAssignMachinesModal = false;
  showRejectRequestModal = false;
  selectedRequest: MachineAssignmentRequest | null = null;
  
  // Assignment modal data
  selectedMachinesForAssignment: string[] = [];
  assignmentComments = '';
  rejectionReason = '';
  
  // Enums for template
  AssignmentRequestStatus = AssignmentRequestStatus;
  RequestUrgency = RequestUrgency;
  MachineType = MachineType;
  
  private subscriptions: Subscription[] = [];

  constructor(
    private machineService: MachineService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadAssignmentRequests();
    this.loadAvailableMachines();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  private loadAssignmentRequests(): void {
    this.isLoading = true;
    const sub = this.machineService.getAllAssignmentRequests().subscribe({
      next: (requests) => {
        this.assignmentRequests = requests.sort((a, b) => 
          new Date(b.requestedDate).getTime() - new Date(a.requestedDate).getTime()
        );
        this.applyFilters();
        this.isLoading = false;
      },
      error: (error) => {
        this.error = 'Failed to load assignment requests';
        this.isLoading = false;
        console.error('Error loading assignment requests:', error);
      }
    });
    this.subscriptions.push(sub);
  }

  private loadAvailableMachines(): void {
    const sub = this.machineService.getAllMachines().subscribe({
      next: (machines: Machine[]) => {
        this.availableMachines = machines.filter(m => m.status === MachineStatus.AVAILABLE);
      },
      error: (error: any) => {
        console.error('Error loading available machines:', error);
      }
    });
    this.subscriptions.push(sub);
  }

  applyFilters(): void {
    this.filteredRequests = this.assignmentRequests.filter(request => {
      const matchesSearch = !this.searchTerm || 
        request.projectId.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        request.machineType.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        request.requestedBy.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        (request.detailsOrExplanation && request.detailsOrExplanation.toLowerCase().includes(this.searchTerm.toLowerCase()));
      
      const matchesStatus = this.selectedStatus === 'ALL' || request.status === this.selectedStatus;
      const matchesUrgency = this.selectedUrgency === 'ALL' || request.urgency === this.selectedUrgency;
      const matchesMachineType = this.selectedMachineType === 'ALL' || request.machineType === this.selectedMachineType;
      
      return matchesSearch && matchesStatus && matchesUrgency && matchesMachineType;
    });
  }

  onSearchChange(): void {
    this.applyFilters();
  }

  onStatusFilterChange(): void {
    this.applyFilters();
  }

  onUrgencyFilterChange(): void {
    this.applyFilters();
  }

  onMachineTypeFilterChange(): void {
    this.applyFilters();
  }

  openRequestDetails(request: MachineAssignmentRequest): void {
    this.selectedRequest = request;
    this.showRequestDetailsModal = true;
  }

  openAssignMachinesModal(request: MachineAssignmentRequest): void {
    this.selectedRequest = request;
    this.selectedMachinesForAssignment = [];
    this.assignmentComments = '';
    this.showAssignMachinesModal = true;
  }

  openRejectRequestModal(request: MachineAssignmentRequest): void {
    this.selectedRequest = request;
    this.rejectionReason = '';
    this.showRejectRequestModal = true;
  }

  closeModals(): void {
    this.showRequestDetailsModal = false;
    this.showAssignMachinesModal = false;
    this.showRejectRequestModal = false;
    this.selectedRequest = null;
    this.selectedMachinesForAssignment = [];
    this.assignmentComments = '';
    this.rejectionReason = '';
  }

  toggleMachineSelection(machineId: string): void {
    const index = this.selectedMachinesForAssignment.indexOf(machineId);
    if (index > -1) {
      this.selectedMachinesForAssignment.splice(index, 1);
    } else {
      this.selectedMachinesForAssignment.push(machineId);
    }
  }

  isMachineSelected(machineId: string): boolean {
    return this.selectedMachinesForAssignment.includes(machineId);
  }

  confirmAssignment(): void {
    if (this.selectedRequest && this.selectedMachinesForAssignment.length > 0) {
      const sub = this.machineService.approveAssignmentRequest(
        this.selectedRequest.id,
        this.selectedMachinesForAssignment,
        this.assignmentComments
      ).subscribe({
        next: () => {
          this.loadAssignmentRequests();
          this.loadAvailableMachines();
          this.closeModals();
        },
        error: (error: any) => {
          this.error = 'Failed to assign machines. Please try again.';
          console.error('Error assigning machines:', error);
        }
      });
      this.subscriptions.push(sub);
    }
  }

  confirmRejection(): void {
    if (this.selectedRequest && this.rejectionReason.trim()) {
      const sub = this.machineService.rejectAssignmentRequest(
        this.selectedRequest.id,
        this.rejectionReason
      ).subscribe({
        next: () => {
          this.loadAssignmentRequests();
          this.closeModals();
        },
        error: (error: any) => {
          this.error = 'Failed to reject request. Please try again.';
          console.error('Error rejecting request:', error);
        }
      });
      this.subscriptions.push(sub);
    }
  }

  getStatusClass(status: AssignmentRequestStatus): string {
    switch (status) {
      case AssignmentRequestStatus.PENDING:
        return 'bg-warning text-dark';
      case AssignmentRequestStatus.APPROVED:
        return 'bg-success';
      case AssignmentRequestStatus.REJECTED:
        return 'bg-danger';
      case AssignmentRequestStatus.PARTIALLY_FULFILLED:
        return 'bg-info';
      case AssignmentRequestStatus.COMPLETED:
        return 'bg-primary';
      case AssignmentRequestStatus.CANCELLED:
        return 'bg-secondary';
      default:
        return 'bg-light text-dark';
    }
  }

  getUrgencyClass(urgency: RequestUrgency): string {
    switch (urgency) {
      case RequestUrgency.LOW:
        return 'bg-success';
      case RequestUrgency.MEDIUM:
        return 'bg-warning text-dark';
      case RequestUrgency.HIGH:
        return 'bg-danger';
      case RequestUrgency.CRITICAL:
        return 'bg-dark';
      default:
        return 'bg-light text-dark';
    }
  }

  getAvailableMachinesForType(machineType: MachineType): Machine[] {
    return this.availableMachines.filter(machine => machine.type === machineType);
  }

  formatDate(date: Date | string): string {
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  navigateToInventory(): void {
    this.router.navigate(['/machine-manager/machine-inventory']);
  }

  get assignmentRequestStatusOptions() {
    return Object.values(AssignmentRequestStatus);
  }

  get requestUrgencyOptions() {
    return Object.values(RequestUrgency);
  }

  get machineTypeOptions() {
    return Object.values(MachineType);
  }
}
