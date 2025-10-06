import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { InventoryTransferService } from '../../../core/services/inventory-transfer.service';
import {
  InventoryTransferRequest,
  TransferRequestStatus,
  TransferRequestFilter
} from '../../../core/models/inventory-transfer.model';
import { DispatchModalComponent } from './dispatch-modal/dispatch-modal.component';

@Component({
  selector: 'app-requests',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatChipsModule,
    MatSnackBarModule,
    DispatchModalComponent
  ],
  templateUrl: './requests.component.html',
  styleUrls: ['./requests.component.scss']
})
export class RequestsComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  requests: InventoryTransferRequest[] = [];
  filteredRequests: InventoryTransferRequest[] = [];
  loading = false;
  errorMessage: string | null = null;

  // Filter form
  filterForm: FormGroup;
  searchTerm = '';
  filtersExpanded = false;

  // Enums for template
  TransferRequestStatus = TransferRequestStatus;

  // View options
  currentView: 'all' | 'anfo' | 'emulsion' = 'all';
  sortBy: string = 'requestDate';
  sortOrder: boolean = true; // true = descending

  // Pagination
  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;

  // Modal state
  showDetailsModal = false;
  showDispatchModal = false;
  selectedRequest: InventoryTransferRequest | null = null;

  constructor(
    private transferService: InventoryTransferService,
    private fb: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.filterForm = this.fb.group({
      status: [''],
      isOverdue: [null],
      isUrgent: [null]
    });
  }

  ngOnInit(): void {
    this.loadRequests();
    this.setupFilterSubscription();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private setupFilterSubscription(): void {
    this.filterForm.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.currentPage = 1;
        this.loadRequests();
      });
  }

  loadRequests(): void {
    this.loading = true;
    this.errorMessage = null;

    const filter: TransferRequestFilter = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
      ...this.filterForm.value,
      sortBy: this.sortBy,
      sortDescending: this.sortOrder
    };

    this.transferService.getTransferRequests(filter)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (pagedData) => {
          this.requests = pagedData.items;
          this.totalCount = pagedData.totalCount;
          this.totalPages = pagedData.totalPages;
          this.applyViewFilter();
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading requests:', error);
          this.errorMessage = error.message || 'Failed to load transfer requests';
          this.loading = false;
          this.snackBar.open('Error loading requests', 'Close', { duration: 3000 });
        }
      });
  }

  applyViewFilter(): void {
    let filtered = [...this.requests];

    if (this.currentView === 'anfo') {
      filtered = filtered.filter(req => req.explosiveTypeName === 'ANFO');
    } else if (this.currentView === 'emulsion') {
      filtered = filtered.filter(req => req.explosiveTypeName === 'Emulsion');
    }

    this.filteredRequests = filtered;
  }

  onSortChange(field: string): void {
    if (this.sortBy === field) {
      this.sortOrder = !this.sortOrder;
    } else {
      this.sortBy = field;
      this.sortOrder = true;
    }
    this.loadRequests();
  }

  onViewChange(view: 'all' | 'anfo' | 'emulsion'): void {
    this.currentView = view;
    this.applyViewFilter();
  }

  clearFilters(): void {
    this.filterForm.reset();
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadRequests();
  }

  toggleFilters(): void {
    this.filtersExpanded = !this.filtersExpanded;
  }

  getStatusClass(status: TransferRequestStatus): string {
    switch (status) {
      case TransferRequestStatus.Approved: return 'status-approved';
      case TransferRequestStatus.Pending: return 'status-pending';
      case TransferRequestStatus.Rejected: return 'status-rejected';
      case TransferRequestStatus.InProgress: return 'status-in-progress';
      case TransferRequestStatus.Completed: return 'status-completed';
      case TransferRequestStatus.Cancelled: return 'status-cancelled';
      default: return '';
    }
  }

  onPageChange(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadRequests();
    }
  }

  get pageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  viewRequestDetails(request: InventoryTransferRequest): void {
    this.selectedRequest = request;
    this.showDetailsModal = true;
  }

  closeDetailsModal(): void {
    this.showDetailsModal = false;
    this.selectedRequest = null;
  }

  openApprovalForm(request: InventoryTransferRequest): void {
    this.router.navigate(['/explosive-manager/requests/approval', request.id]);
  }

  openDispatchForm(request: InventoryTransferRequest): void {
    this.selectedRequest = request;
    this.showDispatchModal = true;
  }

  closeDispatchModal(): void {
    this.showDispatchModal = false;
    this.selectedRequest = null;
  }

  onDispatchComplete(): void {
    this.closeDispatchModal();
    this.loadRequests();
    this.snackBar.open('Request dispatched successfully', 'Close', { duration: 3000 });
  }

  confirmDelivery(request: InventoryTransferRequest): void {
    if (confirm(`Confirm delivery for request ${request.requestNumber}?`)) {
      this.transferService.confirmDelivery(request.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.snackBar.open('Delivery confirmed successfully', 'Close', { duration: 3000 });
            this.loadRequests();
          },
          error: (error) => {
            this.snackBar.open(error.message || 'Failed to confirm delivery', 'Close', { duration: 3000 });
          }
        });
    }
  }

  canApprove(request: InventoryTransferRequest): boolean {
    return request.status === TransferRequestStatus.Pending;
  }

  canDispatch(request: InventoryTransferRequest): boolean {
    return request.status === TransferRequestStatus.Approved && !request.dispatchDate;
  }

  canConfirmDelivery(request: InventoryTransferRequest): boolean {
    return request.dispatchDate != null && !request.deliveryConfirmedDate;
  }

  getDispatchStatusText(request: InventoryTransferRequest): string {
    if (request.deliveryConfirmedDate) return 'Delivered';
    if (request.dispatchDate) return 'Dispatched';
    if (request.status === TransferRequestStatus.Approved) return 'Ready to Dispatch';
    return 'Not Dispatched';
  }

  getDispatchStatusClass(request: InventoryTransferRequest): string {
    if (request.deliveryConfirmedDate) return 'status-completed';
    if (request.dispatchDate) return 'status-in-progress';
    return 'status-pending';
  }

  // For template compatibility
  get paginatedRequests(): InventoryTransferRequest[] {
    return this.filteredRequests;
  }

  onSearchChange(): void {
    this.loadRequests();
  }
}
