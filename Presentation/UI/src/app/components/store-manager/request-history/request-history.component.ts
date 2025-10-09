import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { Subject, takeUntil } from 'rxjs';
import { InventoryTransferService } from '../../../core/services/inventory-transfer.service';
import {
  InventoryTransferRequest,
  TransferRequestStatus
} from '../../../core/models/inventory-transfer.model';
import { ViewDetailsComponent } from './view-details/view-details.component';

@Component({
  selector: 'app-request-history',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, ViewDetailsComponent, MatIconModule, MatButtonModule],
  templateUrl: './request-history.component.html',
  styleUrls: ['./request-history.component.scss']
})
export class RequestHistoryComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  requests: InventoryTransferRequest[] = [];
  filteredRequests: InventoryTransferRequest[] = [];

  isLoading = false;

  successMessage = '';
  errorMessage = '';

  TransferRequestStatus = TransferRequestStatus;

  // Filter properties
  filterStatus = '';
  filterDateFrom = '';
  filterDateTo = '';
  searchTerm = '';

  // UI state
  isFiltersCollapsed = true;

  // Row expansion state
  expandedRows: Set<number> = new Set<number>();

  // View Details Modal
  showViewDetails = false;
  selectedRequest: InventoryTransferRequest | null = null;

  // Dispatch Info Modal
  showDispatchInfo = false;
  selectedDispatchRequest: InventoryTransferRequest | null = null;

  constructor(
    private transferService: InventoryTransferService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadRequests();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadRequests(): void {
    this.isLoading = true;
    this.transferService.getMyRequests()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (requests) => {
          this.requests = requests;
          this.applyFilters();
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading requests:', error);
          this.errorMessage = 'Failed to load request history';
          this.isLoading = false;
        }
      });
  }

  applyFilters(): void {
    this.filteredRequests = this.requests.filter(request => {
      const matchesStatus = !this.filterStatus || request.status === this.filterStatus;
      const matchesSearch = !this.searchTerm ||
        request.requestNumber.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        request.requestedByUserName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        request.explosiveTypeName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        (request.requestNotes && request.requestNotes.toLowerCase().includes(this.searchTerm.toLowerCase()));

      let matchesDateRange = true;
      if (this.filterDateFrom) {
        matchesDateRange = matchesDateRange && new Date(request.requestDate) >= new Date(this.filterDateFrom);
      }
      if (this.filterDateTo) {
        matchesDateRange = matchesDateRange && new Date(request.requestDate) <= new Date(this.filterDateTo);
      }

      return matchesStatus && matchesSearch && matchesDateRange;
    });
  }

  clearFilters(): void {
    this.filterStatus = '';
    this.filterDateFrom = '';
    this.filterDateTo = '';
    this.searchTerm = '';
    this.filteredRequests = [...this.requests];
  }

  hasActiveFilters(): boolean {
    return !!(this.searchTerm || this.filterStatus || this.filterDateFrom || this.filterDateTo);
  }

  getStatusClass(status: TransferRequestStatus): string {
    switch (status) {
      case TransferRequestStatus.Approved:
        return 'bg-success';
      case TransferRequestStatus.Pending:
        return 'bg-warning';
      case TransferRequestStatus.Rejected:
        return 'bg-danger';
      case TransferRequestStatus.Completed:
        return 'bg-info';
      case TransferRequestStatus.InProgress:
        return 'bg-primary';
      default:
        return 'bg-secondary';
    }
  }



  formatDate(date: Date | undefined): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  clearMessages(): void {
    this.successMessage = '';
    this.errorMessage = '';
  }

  toggleFilters(): void {
    this.isFiltersCollapsed = !this.isFiltersCollapsed;
  }

  // Expand/collapse handlers for parent table rows
  isExpanded(id: number): boolean {
    return this.expandedRows.has(id);
  }

  toggleExpanded(id: number): void {
    if (this.expandedRows.has(id)) {
      this.expandedRows.delete(id);
    } else {
      this.expandedRows.add(id);
    }
  }

  // View Details Modal Methods
  openViewDetails(request: InventoryTransferRequest): void {
    this.selectedRequest = request;
    this.showViewDetails = true;
  }

  closeViewDetails(): void {
    this.showViewDetails = false;
    this.selectedRequest = null;
  }

  openDispatchInfo(request: InventoryTransferRequest): void {
    this.router.navigate(['/store-manager/dispatch-info', request.id]);
  }

  closeDispatchInfo(): void {
    this.showDispatchInfo = false;
    this.selectedDispatchRequest = null;
  }

  // Helper method to determine received status
  isReceived(request: InventoryTransferRequest): boolean {
    return !!request.deliveryConfirmedDate;
  }

  getReceivedStatusClass(request: InventoryTransferRequest): string {
    return this.isReceived(request) ? 'badge-success' : 'badge-secondary';
  }

  getReceivedStatusText(request: InventoryTransferRequest): string {
    return this.isReceived(request) ? 'Received' : 'Not Received';
  }
}