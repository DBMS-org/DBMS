import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MachineService } from '../../../core/services/machine.service';
import { ProjectService } from '../../../core/services/project.service';
import { AuthService } from '../../../core/services/auth.service';
import { 
  MachineAssignmentRequest, 
  AssignmentRequestStatus,
  RequestUrgency,
  MachineType
} from '../../../core/models/machine.model';
import { Project } from '../../../core/models/project.model';

@Component({
  selector: 'app-machine-assignments',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './machine-assignments.component.html',
  styleUrl: './machine-assignments.component.scss'
})
export class MachineAssignmentsComponent implements OnInit, OnDestroy {
  assignmentRequests: MachineAssignmentRequest[] = [];
  projects: Project[] = [];
  filteredRequests: MachineAssignmentRequest[] = [];
  isLoading = false;
  error: string | null = null;
  
  // Filter properties
  selectedStatus: AssignmentRequestStatus | 'ALL' = 'ALL';
  selectedUrgency: RequestUrgency | 'ALL' = 'ALL';
  searchTerm = '';
  
  // Modal states
  showApprovalModal = false;
  showRejectionModal = false;
  showAssignmentRequestModal = false;
  selectedRequest: MachineAssignmentRequest | null = null;
  
  // Forms
  approvalForm!: FormGroup;
  rejectionForm!: FormGroup;
  assignmentRequestForm!: FormGroup;
  
  // Enums for template
  AssignmentRequestStatus = AssignmentRequestStatus;
  RequestUrgency = RequestUrgency;
  MachineType = MachineType;
  
  // Statistics
  statistics = {
    total: 0,
    pending: 0,
    approved: 0,
    rejected: 0,
    completed: 0
  };
  
  private subscriptions: Subscription[] = [];

  constructor(
    private machineService: MachineService,
    private projectService: ProjectService,
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {
    this.initializeForms();
  }

  ngOnInit(): void {
    this.loadAssignmentRequests();
    this.loadProjects();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  private initializeForms(): void {
    this.approvalForm = this.formBuilder.group({
      assignedMachines: ['', Validators.required],
      comments: ['']
    });

    this.rejectionForm = this.formBuilder.group({
      comments: ['', Validators.required]
    });

    this.assignmentRequestForm = this.formBuilder.group({
      projectId: ['', Validators.required],
      machineType: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      urgency: [RequestUrgency.MEDIUM, Validators.required],
      detailsOrExplanation: [''],
      expectedUsageDuration: [''],
      expectedReturnDate: ['']
    });
  }

  private loadAssignmentRequests(): void {
    this.isLoading = true;
    const sub = this.machineService.getAllAssignmentRequests().subscribe({
      next: (requests) => {
        this.assignmentRequests = requests;
        this.applyFilters();
        this.calculateStatistics();
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

  private loadProjects(): void {
    const sub = this.projectService.getProjects().subscribe({
      next: (projects: Project[]) => {
        this.projects = projects;
      },
      error: (error: any) => {
        console.error('Error loading projects:', error);
      }
    });
    this.subscriptions.push(sub);
  }

  private calculateStatistics(): void {
    this.statistics = {
      total: this.assignmentRequests.length,
      pending: this.assignmentRequests.filter(r => r.status === AssignmentRequestStatus.PENDING).length,
      approved: this.assignmentRequests.filter(r => r.status === AssignmentRequestStatus.APPROVED).length,
      rejected: this.assignmentRequests.filter(r => r.status === AssignmentRequestStatus.REJECTED).length,
      completed: this.assignmentRequests.filter(r => r.status === AssignmentRequestStatus.COMPLETED).length
    };
  }

  applyFilters(): void {
    this.filteredRequests = this.assignmentRequests.filter(request => {
      const matchesSearch = !this.searchTerm || 
        request.requestedBy.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        request.projectId.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        request.machineType.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      const matchesStatus = this.selectedStatus === 'ALL' || request.status === this.selectedStatus;
      const matchesUrgency = this.selectedUrgency === 'ALL' || request.urgency === this.selectedUrgency;
      
      return matchesSearch && matchesStatus && matchesUrgency;
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

  openApprovalModal(request: MachineAssignmentRequest): void {
    this.selectedRequest = request;
    this.approvalForm.reset();
    this.showApprovalModal = true;
  }

  openRejectionModal(request: MachineAssignmentRequest): void {
    this.selectedRequest = request;
    this.rejectionForm.reset();
    this.showRejectionModal = true;
  }

  closeApprovalModal(): void {
    this.showApprovalModal = false;
    this.selectedRequest = null;
  }

  closeRejectionModal(): void {
    this.showRejectionModal = false;
    this.selectedRequest = null;
  }

  openAssignmentRequestModal(): void {
    this.assignmentRequestForm.reset({
      urgency: RequestUrgency.MEDIUM,
      quantity: 1
    });
    this.showAssignmentRequestModal = true;
  }

  closeAssignmentRequestModal(): void {
    this.showAssignmentRequestModal = false;
  }

  submitAssignmentRequest(): void {
    if (this.assignmentRequestForm.invalid) return;

    const currentUser = this.authService.getCurrentUser();
    const formValue = this.assignmentRequestForm.value;
    
    const request: MachineAssignmentRequest = {
      id: Date.now().toString(),
      projectId: formValue.projectId,
      machineType: formValue.machineType,
      quantity: formValue.quantity,
      requestedBy: currentUser?.name || 'Admin User',
      requestedDate: new Date(),
      status: AssignmentRequestStatus.PENDING,
      urgency: formValue.urgency,
      detailsOrExplanation: formValue.detailsOrExplanation,
      expectedUsageDuration: formValue.expectedUsageDuration,
      expectedReturnDate: formValue.expectedReturnDate ? new Date(formValue.expectedReturnDate) : undefined
    };

    const sub = this.machineService.submitAssignmentRequest(request).subscribe({
      next: () => {
        this.loadAssignmentRequests();
        this.closeAssignmentRequestModal();
        this.showNotification('Assignment request submitted successfully!');
      },
      error: (error) => {
        this.error = 'Failed to submit assignment request';
        console.error('Error submitting assignment request:', error);
      }
    });
    this.subscriptions.push(sub);
  }

  approveRequest(): void {
    if (!this.selectedRequest || this.approvalForm.invalid) return;

    const formValue = this.approvalForm.value;
    
    const sub = this.machineService.approveAssignmentRequest(
      this.selectedRequest.id,
      formValue.assignedMachines.split(',').map((id: string) => id.trim()),
      formValue.comments
    ).subscribe({
      next: () => {
        this.loadAssignmentRequests();
        this.closeApprovalModal();
        this.showNotification('Assignment request approved successfully!');
      },
      error: (error) => {
        this.error = 'Failed to approve assignment request';
        console.error('Error approving request:', error);
      }
    });
    this.subscriptions.push(sub);
  }

  rejectRequest(): void {
    if (!this.selectedRequest || this.rejectionForm.invalid) return;

    const formValue = this.rejectionForm.value;
    
    const sub = this.machineService.rejectAssignmentRequest(
      this.selectedRequest.id,
      formValue.comments
    ).subscribe({
      next: () => {
        this.loadAssignmentRequests();
        this.closeRejectionModal();
        this.showNotification('Assignment request rejected.');
      },
      error: (error) => {
        this.error = 'Failed to reject assignment request';
        console.error('Error rejecting request:', error);
      }
    });
    this.subscriptions.push(sub);
  }

  getStatusClass(status: AssignmentRequestStatus): string {
    switch (status) {
      case AssignmentRequestStatus.PENDING:
        return 'bg-warning text-dark';
      case AssignmentRequestStatus.APPROVED:
        return 'bg-success';
      case AssignmentRequestStatus.REJECTED:
        return 'bg-danger';
      case AssignmentRequestStatus.COMPLETED:
        return 'bg-primary';
      default:
        return 'bg-secondary';
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
        return 'bg-secondary';
    }
  }

  navigateToInventory(): void {
    this.router.navigate(['/admin/machine-inventory']);
  }

  private showNotification(message: string): void {
    // In a real application, you would use a proper notification service
    alert(message);
  }

  get statusOptions() {
    return Object.values(AssignmentRequestStatus);
  }

  get urgencyOptions() {
    return Object.values(RequestUrgency);
  }

  get machineTypeOptions() {
    return Object.values(MachineType);
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString();
  }
}
