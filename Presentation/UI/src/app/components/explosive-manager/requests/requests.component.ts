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
import { RequestService } from './services/request.service';
import { ExplosiveRequest, ExplosiveType, RequestStatus, RequestSearchCriteria } from './models/explosive-request.model';

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
    MatSnackBarModule
  ],
  templateUrl: './requests.component.html',
  styleUrls: ['./requests.component.scss']
})
export class RequestsComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  
  requests: ExplosiveRequest[] = [];
  filteredRequests: ExplosiveRequest[] = [];
  loading = false;
  
  // Filter form
  filterForm: FormGroup;
  searchTerm = '';
  filtersExpanded = false;
  
  // Enums for template
  ExplosiveType = ExplosiveType;
  RequestStatus = RequestStatus;

  
  // View options
  currentView: 'all' | 'anfo' | 'emulsion' = 'all';
  sortBy: 'requestDate' | 'requiredDate' | 'status' = 'requestDate';
  sortOrder: 'asc' | 'desc' = 'desc';
  
  // Pagination
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;
  
  // Modal state
  showDetailsModal = false;
  selectedRequest: ExplosiveRequest | null = null;

  // Row expansion state
  expandedRows = new Set<string>();
  constructor(
    private requestService: RequestService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.filterForm = this.fb.group({
      explosiveType: [''],
      status: [''],
      /* priority removed */
      requesterName: [''],
      storeLocation: [''],
      dateFrom: [''],
      dateTo: ['']
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
        this.applyFilters();
      });
  }
  
  loadRequests(): void {
    this.loading = true;
    const criteria: RequestSearchCriteria = {
      searchTerm: this.searchTerm,
      filters: this.filterForm.value,
      sortBy: this.sortBy,
      sortOrder: this.sortOrder
    };
    
    this.requestService.getRequests(criteria)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (requests) => {
          this.requests = requests;
          this.applyViewFilter();
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading requests:', error);
          this.loading = false;
        }
      });
  }
  
  applyFilters(): void {
    this.currentPage = 1;
    this.loadRequests();
  }
  
  applyViewFilter(): void {
    let filtered = [...this.requests];
    
    if (this.currentView === 'anfo') {
      filtered = filtered.filter(req => (req.requestedItems?.some(i => i.explosiveType === ExplosiveType.ANFO)) || req.explosiveType === ExplosiveType.ANFO);
    } else if (this.currentView === 'emulsion') {
      filtered = filtered.filter(req => (req.requestedItems?.some(i => i.explosiveType === ExplosiveType.EMULSION)) || req.explosiveType === ExplosiveType.EMULSION);
    }
    
    this.filteredRequests = filtered;
    this.calculatePagination();
  }
  
  onSearchChange(): void {
    this.applyFilters();
  }
  
  onSortChange(field: 'requestDate' | 'requiredDate' | 'status'): void {
    if (this.sortBy === field) {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortBy = field;
      this.sortOrder = 'desc';
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
  
  getStatusClass(status: RequestStatus): string {
    switch (status) {
      case RequestStatus.APPROVED: return 'status-approved';
      case RequestStatus.PENDING: return 'status-pending';
      case RequestStatus.REJECTED: return 'status-rejected';
      case RequestStatus.IN_PROGRESS: return 'status-in-progress';
      case RequestStatus.COMPLETED: return 'status-completed';
      case RequestStatus.CANCELLED: return 'status-cancelled';
      default: return '';
    }
  }
  

  private calculatePagination(): void {
    this.totalPages = Math.ceil(this.filteredRequests.length / this.pageSize);
  }
  
  get paginatedRequests(): ExplosiveRequest[] {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    return this.filteredRequests.slice(startIndex, endIndex);
  }
  
  onPageChange(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }
  
  get pageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }
  
  viewRequestDetails(request: ExplosiveRequest): void {
    this.selectedRequest = request;
    this.showDetailsModal = true;
  }
  
  closeDetailsModal(): void {
    this.showDetailsModal = false;
    this.selectedRequest = null;
  }

  openApprovalForm(request: ExplosiveRequest): void {
    this.router.navigate(['/explosive-manager/requests/approval', request.id]);
  }

  goToDispatch(request: ExplosiveRequest): void {
    this.router.navigate(['/explosive-manager/requests/dispatch', request.id]);
  }

  getDispatchStatusText(request: ExplosiveRequest): string {
    if (request.status === RequestStatus.DISPATCHED) return 'Dispatched';
    if (request.dispatchDate) return 'Scheduled';
    return 'Not Dispatched';
  }

  getDispatchStatusClass(request: ExplosiveRequest): string {
    const text = this.getDispatchStatusText(request);
    switch (text) {
      case 'Dispatched':
        return 'status-completed';
      case 'Scheduled':
        return 'status-in-progress';
      default:
        return 'status-pending';
    }
  }

  // Helpers for multi-item display
  isExpanded(request: ExplosiveRequest): boolean {
    return this.expandedRows.has(request.id);
  }

  toggleExpanded(requestId: string): void {
    if (this.expandedRows.has(requestId)) {
      this.expandedRows.delete(requestId);
    } else {
      this.expandedRows.add(requestId);
    }
  }

  getItemsForRequest(request: ExplosiveRequest) {
    if (request.requestedItems && request.requestedItems.length > 0) {
      return request.requestedItems;
    }
    // Fallback to single-item fields if multi-item array is not present
    if (request.explosiveType && request.quantity != null && request.unit) {
      return [{
        explosiveType: request.explosiveType,
        quantity: request.quantity,
        unit: request.unit,
        purpose: request.purpose,
        specifications: undefined
      }];
    }
    return [];
  }

  getItemsCount(request: ExplosiveRequest): number {
    return this.getItemsForRequest(request).length;
  }
}
