import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from '../../../core/services/auth.service';
import { User } from '../../../core/models/user.model';
import { StockRequestService } from '../../../core/services/stock-request.service';
import { InventoryTransferService } from '../../../core/services/inventory-transfer.service';
import { StockRequestStatistics } from '../../../core/models/stock-request.model';
import { InventoryTransferRequest } from '../../../core/models/inventory-transfer.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit, OnDestroy {
  currentUser: User | null = null;
  private userSubscription: Subscription = new Subscription();

  // Real statistics from APIs
  stockRequestStats: StockRequestStatistics | null = null;
  pendingTransfers: InventoryTransferRequest[] = [];
  recentTransfers: InventoryTransferRequest[] = [];

  // Computed dashboard stats
  dashboardStats = {
    pendingInventoryRequests: 0,
    readyToDispatch: 0,
    completedThisMonth: 0,
    totalTransfers: 0
  };

  isLoading = false;
  lastRefreshed: Date = new Date();

  constructor(
    private authService: AuthService,
    private router: Router,
    private stockRequestService: StockRequestService,
    private inventoryTransferService: InventoryTransferService
  ) {}

  ngOnInit() {
    this.subscribeToCurrentUser();
    this.loadDashboardData();
  }

  ngOnDestroy() {
    this.userSubscription.unsubscribe();
  }

  private subscribeToCurrentUser() {
    this.userSubscription = this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  private loadDashboardData() {
    this.isLoading = true;

    // Load all data in parallel
    forkJoin({
      stockStats: this.stockRequestService.getStockRequestStatistics(),
      transfers: this.inventoryTransferService.getTransferRequests({ pageSize: 100 }).pipe(
        map(pagedList => pagedList.items)
      )
    }).subscribe({
      next: (data) => {
        this.stockRequestStats = data.stockStats;

        // Filter pending transfers
        this.pendingTransfers = data.transfers
          .filter((t: InventoryTransferRequest) => t.status === 'Pending')
          .slice(0, 5);

        // Filter ready to dispatch
        const readyToDispatch = data.transfers
          .filter((t: InventoryTransferRequest) => t.status === 'Approved' && !t.dispatchDate);

        // Get recent transfers (last 5 completed)
        this.recentTransfers = data.transfers
          .filter((t: InventoryTransferRequest) => t.status === 'Completed')
          .sort((a: InventoryTransferRequest, b: InventoryTransferRequest) =>
            new Date(b.requestDate).getTime() - new Date(a.requestDate).getTime())
          .slice(0, 5);

        // Calculate completed this month
        const now = new Date();
        const firstDayOfMonth = new Date(now.getFullYear(), now.getMonth(), 1);
        const completedThisMonth = data.transfers.filter((t: InventoryTransferRequest) =>
          t.status === 'Completed' &&
          t.requestDate &&
          new Date(t.requestDate) >= firstDayOfMonth
        ).length;

        // Update dashboard stats
        this.dashboardStats = {
          pendingInventoryRequests: this.pendingTransfers.length,
          readyToDispatch: readyToDispatch.length,
          completedThisMonth: completedThisMonth,
          totalTransfers: data.transfers.length
        };

        this.lastRefreshed = new Date();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading dashboard data:', error);
        this.isLoading = false;
      }
    });
  }

  getUserWelcomeMessage(): string {
    if (!this.currentUser) return 'Welcome, Store Manager';

    const timeOfDay = this.getTimeOfDayGreeting();
    return `${timeOfDay}, ${this.currentUser.name}`;
  }

  private getTimeOfDayGreeting(): string {
    const hour = new Date().getHours();
    if (hour < 12) return 'Good morning';
    if (hour < 17) return 'Good afternoon';
    return 'Good evening';
  }

  getStatusSeverity(status: string): string {
    switch (status?.toLowerCase()) {
      case 'pending':
        return 'bg-yellow-100 text-yellow-800 border-yellow-200';
      case 'approved':
        return 'bg-green-100 text-green-800 border-green-200';
      case 'inprogress':
      case 'in progress':
        return 'bg-blue-100 text-blue-800 border-blue-200';
      case 'completed':
        return 'bg-teal-100 text-teal-800 border-teal-200';
      case 'rejected':
      case 'cancelled':
        return 'bg-red-100 text-red-800 border-red-200';
      default:
        return 'bg-gray-100 text-gray-800 border-gray-200';
    }
  }

  formatDate(date: Date | string | undefined): string {
    if (!date) return 'N/A';
    const d = new Date(date);
    return d.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
  }

  getTimeAgo(date: Date | string | undefined): string {
    if (!date) return 'N/A';
    const now = new Date();
    const past = new Date(date);
    const diffMs = now.getTime() - past.getTime();
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 60) return `${diffMins} min${diffMins !== 1 ? 's' : ''} ago`;
    if (diffHours < 24) return `${diffHours} hour${diffHours !== 1 ? 's' : ''} ago`;
    if (diffDays < 7) return `${diffDays} day${diffDays !== 1 ? 's' : ''} ago`;
    return this.formatDate(date);
  }

  navigateToAddStock(): void {
    this.router.navigate(['/store-manager/add-stock']);
  }

  navigateToRequestHistory(): void {
    this.router.navigate(['/store-manager/request-history']);
  }

  navigateToBlastingRequests(): void {
    this.router.navigate(['/store-manager/blasting-engineer-requests']);
  }

  viewTransferDetails(transfer: InventoryTransferRequest): void {
    this.router.navigate(['/store-manager/request-history'], {
      queryParams: { requestId: transfer.requestNumber }
    });
  }

  refreshDashboard(): void {
    this.loadDashboardData();
  }

  trackByRequestNumber(index: number, item: InventoryTransferRequest): string {
    return item.requestNumber;
  }
}
