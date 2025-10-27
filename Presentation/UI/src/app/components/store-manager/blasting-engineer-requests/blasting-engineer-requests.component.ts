import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ExplosiveApprovalRequestService, ExplosiveApprovalRequest } from '../../../core/services/explosive-approval-request.service';
import { AuthService } from '../../../core/services/auth.service';
import { RequestDetailsComponent } from './request-details/request-details.component';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { TagModule } from 'primeng/tag';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { TooltipModule } from 'primeng/tooltip';
import { DialogModule } from 'primeng/dialog';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { PanelModule } from 'primeng/panel';
import { MessageModule } from 'primeng/message';

@Component({
  selector: 'app-blasting-engineer-requests',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RequestDetailsComponent,
    TableModule,
    ButtonModule,
    InputTextModule,
    DropdownModule,
    TagModule,
    IconFieldModule,
    InputIconModule,
    TooltipModule,
    DialogModule,
    InputTextareaModule,
    PanelModule,
    MessageModule
  ],
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
  currentUserCountry: string | null = null;
  currentUserName: string | null = null;
  currentUserRole: string | null = null;
  lastRefreshTime: Date | null = null;
  errorMessage: string = '';
  
  // Modal properties
  selectedRequest: ExplosiveApprovalRequest | null = null;
  isDetailsModalVisible: boolean = false;

  // Filter panel toggle
  isFiltersPanelCollapsed: boolean = false;

  // Rejection dialog properties
  showRejectDialog = false;
  rejectionReason = '';
  rejectionError = '';
  isRejectingRequest = false;

  // Dropdown options
  statusOptions: Array<{label: string, value: string}> = [
    { label: 'All Statuses', value: 'ALL' },
    { label: 'Pending', value: 'Pending' },
    { label: 'Approved', value: 'Approved' },
    { label: 'Rejected', value: 'Rejected' },
    { label: 'Cancelled', value: 'Cancelled' },
    { label: 'Expired', value: 'Expired' }
  ];

  constructor(
    private explosiveApprovalService: ExplosiveApprovalRequestService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadUserInfo();
    this.loadRequests();
  }

  private loadUserInfo(): void {
    const currentUser = this.authService.getCurrentUser();
    this.currentUserRegion = this.authService.getUserRegion();
    this.currentUserCountry = this.authService.getUserCountry();
    this.currentUserName = currentUser?.name || null;
    this.currentUserRole = this.authService.getUserRole();
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
          console.log('=== EXPLOSIVE APPROVAL REQUESTS DATA FROM DATABASE ===');
          console.log('Total requests received:', requests.length);
          console.log('Full requests array:', requests);
          
          // Log each request in detail
          requests.forEach((request, index) => {
            console.log(`\n--- Request ${index + 1} (ID: ${request.id}) ---`);
            console.log('Full request object:', request);
            console.log('Request ID:', request.id);
            console.log('Status:', request.status);
            console.log('Priority:', request.priority);
            console.log('Approval Type:', request.approvalType);
            console.log('Comments:', request.comments);
            console.log('Expected Usage Date:', request.expectedUsageDate);
            console.log('Created At:', request.createdAt);
            console.log('Processed At:', request.processedAt);
            
            // Log user information
            console.log('Requested By User:', request.requestedByUser);
            if (request.requestedByUser) {
              console.log('  - User ID:', request.requestedByUser.id);
              console.log('  - User Name:', request.requestedByUser.name);
              console.log('  - User Email:', request.requestedByUser.email);
              console.log('  - User Region:', request.requestedByUser.region);
            }
            
            console.log('Processed By User:', request.processedByUser);
            if (request.processedByUser) {
              console.log('  - Processed User ID:', request.processedByUser.id);
              console.log('  - Processed User Name:', request.processedByUser.name);
              console.log('  - Processed User Email:', request.processedByUser.email);
            }
            
            // Log project site information
            console.log('Project Site:', request.projectSite);
            if (request.projectSite) {
              console.log('  - Site ID:', request.projectSite.id);
              console.log('  - Site Name:', request.projectSite.name);
              
              // Log project information within project site
              console.log('  - Project:', request.projectSite.project);
              if (request.projectSite.project) {
                console.log('    - Project ID:', request.projectSite.project.id);
                console.log('    - Project Name:', request.projectSite.project.name);
                console.log('    - Project Region:', request.projectSite.project.region);
                console.log('    - Project Region ID:', request.projectSite.project.regionId);
              } else {
                console.log('    - Project: NULL or UNDEFINED');
              }
            } else {
              console.log('  - Project Site: NULL or UNDEFINED');
            }
            
            console.log('--- End Request Details ---\n');
          });
          
          console.log('=== END DATABASE DATA LOGGING ===\n');
          
          this.requests = requests;
          this.filteredRequests = [...this.requests];
          this.lastRefreshTime = new Date();
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

  refreshRequests(): void {
    this.loadUserInfo(); // Refresh user info as well
    this.loadRequests();
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
    const currentUser = this.authService.getCurrentUser();
    const approvalComment = `Approved by ${currentUser?.name || 'Store Manager'} from ${this.currentUserRegion || 'Unknown Region'}`;
    
    this.explosiveApprovalService.approveExplosiveApprovalRequest(request.id, approvalComment)
      .subscribe({
        next: (success) => {
          if (success) {
            console.log('Approved request:', request.id);
            this.errorMessage = ''; // Clear any previous errors
            // Refresh the data from server to get the updated status
            this.loadRequests();
          }
        },
        error: (error) => {
          console.error('Error approving request:', error);
          this.errorMessage = 'Failed to approve request. Please try again.';
        }
      });
  }

  onReject(request: ExplosiveApprovalRequest): void {
    this.selectedRequest = request;
    this.rejectionReason = '';
    this.rejectionError = '';
    this.showRejectDialog = true;
  }

  confirmReject(): void {
    if (!this.rejectionReason || this.rejectionReason.trim().length === 0) {
      this.rejectionError = 'Rejection reason is required';
      return;
    }

    if (this.rejectionReason.trim().length < 10) {
      this.rejectionError = 'Please provide a more detailed reason (at least 10 characters)';
      return;
    }

    this.isRejectingRequest = true;
    this.rejectionError = '';

    const currentUser = this.authService.getCurrentUser();
    const detailedReason = `Rejected by ${currentUser?.name || 'Store Manager'} from ${this.currentUserRegion || 'Unknown Region'}: ${this.rejectionReason}`;

    this.explosiveApprovalService.rejectExplosiveApprovalRequest(this.selectedRequest!.id, detailedReason)
      .subscribe({
        next: (success) => {
          if (success) {
            console.log('Rejected request:', this.selectedRequest!.id);
            this.errorMessage = ''; // Clear any previous errors
            this.isRejectingRequest = false;
            this.closeRejectDialog();
            // Refresh the data from server to get the updated status
            this.loadRequests();
          }
        },
        error: (error) => {
          console.error('Error rejecting request:', error);
          this.rejectionError = 'Failed to reject request. Please try again.';
          this.isRejectingRequest = false;
        }
      });
  }

  closeRejectDialog(): void {
    this.showRejectDialog = false;
    this.rejectionReason = '';
    this.rejectionError = '';
    this.selectedRequest = null;
  }

  onViewDetails(request: ExplosiveApprovalRequest): void {
    console.log('View details for request:', request.id);
    this.selectedRequest = request;
    this.isDetailsModalVisible = true;
  }

  onCloseDetailsModal(): void {
    this.isDetailsModalVisible = false;
    this.selectedRequest = null;
  }

  onApproveFromModal(request: ExplosiveApprovalRequest): void {
    this.onApprove(request);
    this.onCloseDetailsModal();
  }

  onRejectFromModal(request: ExplosiveApprovalRequest): void {
    this.onReject(request);
    this.onCloseDetailsModal();
  }

  getPendingCount(): number {
    return this.filteredRequests.filter(request => request.status === 'Pending').length;
  }

  getApprovedCount(): number {
    return this.filteredRequests.filter(request => request.status === 'Approved').length;
  }

  getRejectedCount(): number {
    return this.filteredRequests.filter(request => request.status === 'Rejected').length;
  }

  getTotalRequestsCount(): number {
    return this.filteredRequests.length;
  }

  getFormattedRefreshTime(): string {
    if (!this.lastRefreshTime) return 'Never';
    return this.lastRefreshTime.toLocaleString();
  }

  formatDate(dateString: string): string {
    if (!dateString) return 'N/A';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  formatDateTime(dateString: string): string {
    if (!dateString) return 'N/A';
    const date = new Date(dateString);
    return date.toLocaleString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  getProcessedByInfo(request: ExplosiveApprovalRequest): string {
    if (request.processedAt && request.processedByUserId) {
      return `Processed on ${this.formatDateTime(request.processedAt)}`;
    }
    return '';
  }

  getUserDisplayInfo(): string {
    const parts = [];
    if (this.currentUserName) parts.push(this.currentUserName);
    if (this.currentUserRole) parts.push(`(${this.currentUserRole})`);
    return parts.join(' ');
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.statusFilter = 'ALL';
    this.filteredRequests = [...this.requests];
  }

  getStatusSeverity(status: string): 'success' | 'info' | 'warning' | 'danger' | 'secondary' | 'contrast' | undefined {
    switch (status) {
      case 'Pending': return 'warning';
      case 'Approved': return 'success';
      case 'Rejected': return 'danger';
      case 'Cancelled': return 'secondary';
      case 'Expired': return 'contrast';
      default: return 'info';
    }
  }

  getPrioritySeverity(priority: string): 'success' | 'info' | 'warning' | 'danger' | 'secondary' | 'contrast' | undefined {
    switch (priority.toLowerCase()) {
      case 'high': return 'danger';
      case 'medium': return 'warning';
      case 'low': return 'info';
      default: return 'secondary';
    }
  }

  getApprovalTypeSeverity(approvalType: string): 'success' | 'info' | 'warning' | 'danger' | 'secondary' | 'contrast' | undefined {
    switch (approvalType.toLowerCase()) {
      case 'normal': return 'info';
      case 'urgent': return 'danger';
      case 'emergency': return 'contrast';
      default: return 'secondary';
    }
  }

  toggleFiltersPanel(): void {
    this.isFiltersPanelCollapsed = !this.isFiltersPanelCollapsed;
  }
}