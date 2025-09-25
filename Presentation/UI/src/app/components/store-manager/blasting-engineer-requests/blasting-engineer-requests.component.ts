import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ExplosiveApprovalRequestService, ExplosiveApprovalRequest } from '../../../core/services/explosive-approval-request.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-blasting-engineer-requests',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './blasting-engineer-requests.component.html',
  styleUrl: './blasting-engineer-requests.component.scss'
})
export class BlastingEngineerRequestsComponent implements OnInit {
  requests: ExplosiveApprovalRequest[] = [];
  filteredRequests: ExplosiveApprovalRequest[] = [];
  searchTerm: string = '';
  statusFilter: string = 'ALL';
  isLoading: boolean = false;
  currentUserRegion: string | null = null;
  errorMessage: string = '';

  constructor(
    private explosiveApprovalService: ExplosiveApprovalRequestService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.currentUserRegion = this.authService.getUserRegion();
    this.loadRequests();
  }

  loadRequests(): void {
    this.isLoading = true;
    this.errorMessage = '';

    if (!this.currentUserRegion) {
      this.errorMessage = 'Unable to determine your region. Please contact your administrator.';
      this.isLoading = false;
      return;
    }

    this.explosiveApprovalService.getExplosiveApprovalRequestsByRegion(this.currentUserRegion)
      .subscribe({
        next: (requests) => {
          this.requests = requests;
          this.filteredRequests = [...this.requests];
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading explosive approval requests:', error);
          this.errorMessage = 'Failed to load blasting engineer requests. Please try again later.';
          this.isLoading = false;
          this.requests = [];
          this.filteredRequests = [];
        }
      });
  }

  onSearch(): void {
    this.applyFilters();
  }

  onStatusFilterChange(): void {
    this.applyFilters();
  }

  private applyFilters(): void {
    this.filteredRequests = this.requests.filter(request => {
      const matchesSearch = !this.searchTerm || 
        request.id.toString().includes(this.searchTerm) ||
        request.requestedByUser?.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        request.projectSite?.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        request.comments?.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      const matchesStatus = this.statusFilter === 'ALL' || request.status === this.statusFilter;
      
      return matchesSearch && matchesStatus;
    });
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Pending': return 'status-pending';
      case 'Approved': return 'status-approved';
      case 'Rejected': return 'status-rejected';
      case 'Cancelled': return 'status-cancelled';
      case 'Expired': return 'status-expired';
      default: return '';
    }
  }

  onApprove(request: ExplosiveApprovalRequest): void {
    this.explosiveApprovalService.approveExplosiveApprovalRequest(request.id, 'Approved by store manager')
      .subscribe({
        next: (success) => {
          if (success) {
            request.status = 'Approved';
            console.log('Approved request:', request.id);
          }
        },
        error: (error) => {
          console.error('Error approving request:', error);
          this.errorMessage = 'Failed to approve request. Please try again.';
        }
      });
  }

  onReject(request: ExplosiveApprovalRequest): void {
    const rejectionReason = prompt('Please provide a reason for rejection:');
    if (rejectionReason) {
      this.explosiveApprovalService.rejectExplosiveApprovalRequest(request.id, rejectionReason)
        .subscribe({
          next: (success) => {
            if (success) {
              request.status = 'Rejected';
              console.log('Rejected request:', request.id);
            }
          },
          error: (error) => {
            console.error('Error rejecting request:', error);
            this.errorMessage = 'Failed to reject request. Please try again.';
          }
        });
    }
  }

  onViewDetails(request: ExplosiveApprovalRequest): void {
    console.log('View details for request:', request.id);
    // TODO: Implement modal or navigation to detailed view
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.statusFilter = 'ALL';
    this.filteredRequests = [...this.requests];
  }

  getPendingCount(): number {
    return this.filteredRequests.filter(request => request.status === 'Pending').length;
  }

  getApprovedCount(): number {
    return this.filteredRequests.filter(request => request.status === 'Approved').length;
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  refreshRequests(): void {
    this.loadRequests();
  }
}