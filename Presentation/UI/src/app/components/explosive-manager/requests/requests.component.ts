import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { RequestService } from './services/request.service';
import { ExplosiveRequest, ExplosiveType, RequestStatus, RequestSearchCriteria } from './models/request.model';
import { AnfoRequestsComponent } from './anfo-requests/anfo-requests.component';
import { EmulsionRequestsComponent } from './emulsion-requests/emulsion-requests.component';

@Component({
  selector: 'app-requests',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    ReactiveFormsModule,
    AnfoRequestsComponent,
    EmulsionRequestsComponent
  ],
  templateUrl: './requests.component.html',
  styleUrl: './requests.component.scss'
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
  sortBy: 'requestDate' | 'requiredDate' | 'status' | 'quantity' = 'requestDate';
  sortOrder: 'asc' | 'desc' = 'desc';
  
  // Pagination
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;
  
  // Modal state
  showDetailsModal = false;
  selectedRequest: ExplosiveRequest | null = null;
  
  constructor(
    private requestService: RequestService,
    private fb: FormBuilder
  ) {
    this.filterForm = this.fb.group({
      explosiveType: [''],
      status: [''],
      priority: [''],
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
      filtered = filtered.filter(req => req.explosiveType === ExplosiveType.ANFO);
    } else if (this.currentView === 'emulsion') {
      filtered = filtered.filter(req => req.explosiveType === ExplosiveType.EMULSION);
    }
    
    this.filteredRequests = filtered;
    this.calculatePagination();
  }
  
  onSearchChange(): void {
    this.applyFilters();
  }
  
  onSortChange(field: 'requestDate' | 'requiredDate' | 'status' | 'quantity'): void {
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
  
  updateRequestStatus(requestId: string, status: RequestStatus): void {
    this.requestService.updateRequestStatus(requestId, status)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (success) => {
          if (success) {
            this.loadRequests();
          }
        },
        error: (error) => {
          console.error('Error updating request status:', error);
        }
      });
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
}
