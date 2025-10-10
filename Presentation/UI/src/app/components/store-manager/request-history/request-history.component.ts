import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { Subject, takeUntil } from 'rxjs';
import { StockRequestService } from '../../../core/services/stock-request.service';
import { 
  StockRequest, 
  StockRequestStatus
} from '../../../core/models/stock-request.model';
import { ExplosiveType } from '../../../core/models/store.model';
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

  requests: StockRequest[] = [];
  filteredRequests: StockRequest[] = [];
  
  isLoading = false;
  
  successMessage = '';
  errorMessage = '';
  
  ExplosiveType = ExplosiveType;
  StockRequestStatus = StockRequestStatus;
  
  // Filter properties
  filterStatus = '';
  filterType: ExplosiveType | '' = '';
  filterDateFrom = '';
  filterDateTo = '';
  searchTerm = '';
  
  // UI state
  isFiltersCollapsed = true;
  
  // Row expansion state
  expandedRows: Set<string> = new Set<string>();
  
  // View Details Modal
  showViewDetails = false;
  selectedRequest: StockRequest | null = null;
  
  // Dispatch Info Modal
  showDispatchInfo = false;
  selectedDispatchRequest: StockRequest | null = null;

  // User and store information (would typically come from auth service)
  currentUser = {
    name: 'John Smith',
    role: 'Store Manager'
  };
  
  currentStore = {
    id: 'store1',
    name: 'Muscat Field Storage',
    manager: 'Ahmed Al-Rashid'
  };

  // Sample data for demonstration
  sampleRequests: StockRequest[] = [
    {
      id: '1',
      requesterId: 'user1',
      requesterName: 'Ahmed Al-Rashid',
      requesterStoreId: 'store1',
      requesterStoreName: 'Muscat Field Storage',
      requestedItems: [
        {
          explosiveType: ExplosiveType.ANFO,
          requestedQuantity: 0.5,
          unit: 'tons',
          purpose: 'Mining operations - Phase 2',
          specifications: 'Standard grade ANFO for surface mining'
        },
        {
          explosiveType: ExplosiveType.DetonatingCord,
          requestedQuantity: 250,
          unit: 'meters',
          purpose: 'Surface blast connections',
          specifications: '10 g/m detonating cord'
        },
        {
          explosiveType: ExplosiveType.BlastingCaps,
          requestedQuantity: 60,
          unit: 'pieces',
          purpose: 'Detonation sequence setup',
          specifications: 'Electric blasting caps, delay 0-9'
        }
      ],
      requestDate: new Date('2024-01-15'),
      requiredDate: new Date('2024-01-25'),
      status: StockRequestStatus.APPROVED,
      dispatched: true,
      dispatchedDate: new Date('2024-01-17'),
      fulfillmentDate: new Date('2024-01-18'),
      justification: 'Urgent requirement for upcoming mining phase',
      notes: 'Please ensure delivery before 25th Jan',
      approvalDate: new Date('2024-01-16'),
      createdAt: new Date('2024-01-15'),
      updatedAt: new Date('2024-01-16')
    },
    {
      id: '2',
      requesterId: 'user2',
      requesterName: 'Fatima Al-Zahra',
      requesterStoreId: 'store2',
      requesterStoreName: 'Sohar Industrial Storage',
      requestedItems: [
        {
          explosiveType: ExplosiveType.Emulsion,
          requestedQuantity: 0.3,
          unit: 'tons',
          purpose: 'Underground blasting operations',
          specifications: 'Water-resistant emulsion for wet conditions'
        },
        {
          explosiveType: ExplosiveType.Primer,
          requestedQuantity: 40,
          unit: 'pieces',
          purpose: 'Primer cartridges for emulsion shots',
          specifications: 'Suitable for 32-40mm boreholes'
        }
      ],
      requestDate: new Date('2024-01-20'),
      requiredDate: new Date('2024-02-05'),
      status: StockRequestStatus.PENDING,
      dispatched: false,
      justification: 'Routine stock replenishment',
      notes: 'Standard delivery schedule',
      createdAt: new Date('2024-01-20'),
      updatedAt: new Date('2024-01-20')
    },
    {
      id: '3',
      requesterId: 'user1',
      requesterName: 'Ahmed Al-Rashid',
      requesterStoreId: 'store1',
      requesterStoreName: 'Muscat Field Storage',
      requestedItems: [
        {
          explosiveType: ExplosiveType.BlastingCaps,
          requestedQuantity: 100,
          unit: 'pieces',
          purpose: 'Detonation sequence setup',
          specifications: 'Electric blasting caps, delay 0-9'
        },
        {
          explosiveType: ExplosiveType.Booster,
          requestedQuantity: 20,
          unit: 'pieces',
          purpose: 'Boosters for large diameter holes',
          specifications: '400g boosters for 76-89mm holes'
        }
      ],
      requestDate: new Date('2024-01-18'),
      requiredDate: new Date('2024-01-30'),
      status: StockRequestStatus.FULFILLED,
      dispatched: true,
      dispatchedDate: new Date('2024-01-19'),
      justification: 'Critical component for scheduled blast',
      notes: 'Handle with extreme care',
      fulfillmentDate: new Date('2024-01-19'),
      createdAt: new Date('2024-01-18'),
      updatedAt: new Date('2024-01-19')
    }
  ];

  constructor(
    private stockRequestService: StockRequestService,
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
    this.stockRequestService.getStockRequests()
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
      const matchesType = !this.filterType || request.requestedItems.some(item => item.explosiveType === this.filterType);
      const matchesSearch = !this.searchTerm || 
        request.id.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        request.requesterName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        request.justification.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        request.requestedItems.some(item => item.purpose.toLowerCase().includes(this.searchTerm.toLowerCase()));
      
      let matchesDateRange = true;
      if (this.filterDateFrom) {
        matchesDateRange = matchesDateRange && request.requestDate >= new Date(this.filterDateFrom);
      }
      if (this.filterDateTo) {
        matchesDateRange = matchesDateRange && request.requestDate <= new Date(this.filterDateTo);
      }
      
      return matchesStatus && matchesType && matchesSearch && matchesDateRange;
    });
  }

  clearFilters(): void {
    this.filterStatus = '';
    this.filterType = '';
    this.filterDateFrom = '';
    this.filterDateTo = '';
    this.searchTerm = '';
    this.filteredRequests = [...this.requests];
  }

  hasActiveFilters(): boolean {
    return !!(this.searchTerm || this.filterStatus || this.filterType || this.filterDateFrom || this.filterDateTo);
  }

  getStatusClass(status: StockRequestStatus): string {
    switch (status) {
      case StockRequestStatus.APPROVED:
        return 'bg-success';
      case StockRequestStatus.PENDING:
        return 'bg-warning';
      case StockRequestStatus.REJECTED:
        return 'bg-danger';
      case StockRequestStatus.FULFILLED:
        return 'bg-info';
      case StockRequestStatus.IN_PROGRESS:
        return 'bg-primary';
      default:
        return 'bg-secondary';
    }
  }



  formatDate(date: Date): string {
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
  isExpanded(id: string): boolean {
    return this.expandedRows.has(id);
  }

  toggleExpanded(id: string): void {
    if (this.expandedRows.has(id)) {
      this.expandedRows.delete(id);
    } else {
      this.expandedRows.add(id);
    }
  }

  // View Details Modal Methods
  openViewDetails(request: StockRequest): void {
    this.selectedRequest = request;
    this.showViewDetails = true;
  }

  closeViewDetails(): void {
    this.showViewDetails = false;
    this.selectedRequest = null;
  }
  
  openDispatchInfo(request: StockRequest): void {
    this.router.navigate(['/store-manager/dispatch-info', request.id]);
  }
  
  closeDispatchInfo(): void {
    this.showDispatchInfo = false;
    this.selectedDispatchRequest = null;
  }

  // Helper method to determine received status
  isReceived(request: StockRequest): boolean {
    return !!request.fulfillmentDate;
  }

  getReceivedStatusClass(request: StockRequest): string {
    return this.isReceived(request) ? 'badge-success' : 'badge-secondary';
  }

  getReceivedStatusText(request: StockRequest): string {
    return this.isReceived(request) ? 'Received' : 'Not Received';
  }
}