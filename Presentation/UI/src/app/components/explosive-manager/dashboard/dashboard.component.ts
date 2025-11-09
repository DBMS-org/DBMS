import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription, forkJoin } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { User } from '../../../core/models/user.model';
import { CentralInventoryService } from '../../../core/services/central-inventory.service';
import { StoreService } from '../../../core/services/store.service';
import { ExplosiveApprovalRequestService } from '../../../core/services/explosive-approval-request.service';
import { InventoryDashboard } from '../../../core/models/central-inventory.model';
import { Store, StoreStatus } from '../../../core/models/store.model';

// Main dashboard component for Explosive Manager
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

  // Real-time dashboard data from API
  inventoryDashboard: InventoryDashboard | null = null;
  stores: Store[] = [];
  pendingRequests: any[] = [];
  expiringBatches: any[] = [];
  storeStatistics: any = null;

  isLoading = false;
  loadError: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private centralInventoryService: CentralInventoryService,
    private storeService: StoreService,
    private approvalRequestService: ExplosiveApprovalRequestService
  ) {}

  ngOnInit() {
    this.subscribeToCurrentUser();
    this.loadDashboardData();
  }

  ngOnDestroy() {
    this.userSubscription.unsubscribe();
  }

  // Subscribe to user authentication state
  private subscribeToCurrentUser() {
    this.userSubscription = this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  // Load all dashboard data from backend APIs
  private loadDashboardData() {
    this.isLoading = true;
    this.loadError = null;

    forkJoin({
      inventoryDashboard: this.centralInventoryService.getDashboard(),
      stores: this.storeService.getAllStores(),
      storeStatistics: this.storeService.getStoreStatistics(),
      pendingRequests: this.approvalRequestService.getPendingExplosiveApprovalRequests(),
      expiringBatches: this.centralInventoryService.getExpiringBatches(30)
    }).subscribe({
      next: (data) => {
        this.inventoryDashboard = data.inventoryDashboard;
        this.stores = data.stores;
        this.storeStatistics = data.storeStatistics;
        this.pendingRequests = data.pendingRequests;
        this.expiringBatches = data.expiringBatches;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading dashboard data:', error);
        this.loadError = 'Failed to load dashboard data. Please try again.';
        this.isLoading = false;
      }
    });
  }

  // Check if there are any alerts to display
  hasAlerts(): boolean {
    return !!(
      (this.expiringBatches && this.expiringBatches.length > 0) ||
      (this.inventoryDashboard && this.inventoryDashboard.quarantinedBatches > 0) ||
      (this.inventoryDashboard && this.inventoryDashboard.overdueTransferRequests > 0)
    );
  }

  // Calculate percentage for progress bars
  getInventoryUtilizationPercentage(): number {
    if (!this.inventoryDashboard || this.inventoryDashboard.totalQuantity === 0) return 0;
    return Math.round((this.inventoryDashboard.allocatedQuantity / this.inventoryDashboard.totalQuantity) * 100);
  }

  // Get average store utilization
  getAverageStoreUtilization(): number {
    if (!this.stores || this.stores.length === 0) return 0;
    const totalUtilization = this.stores.reduce((sum, store) => sum + (store.utilizationPercentage || 0), 0);
    return Math.round(totalUtilization / this.stores.length);
  }

  // Get operational stores count
  getOperationalStoresCount(): number {
    if (!this.stores) return 0;
    return this.stores.filter(store => store.status === StoreStatus.Operational).length;
  }

  // Navigate to inventory management page
  navigateToInventory(): void {
    this.router.navigate(['/explosive-manager/inventory']);
  }

  // Navigate to requests page
  navigateToRequests(): void {
    this.router.navigate(['/explosive-manager/requests']);
  }

  // Navigate to stores page
  navigateToStores(): void {
    this.router.navigate(['/explosive-manager/stores']);
  }

  // Navigate to specific request
  navigateToRequest(requestId: number): void {
    this.router.navigate(['/explosive-manager/requests/approval', requestId]);
  }

  // Reload dashboard data
  refreshDashboard(): void {
    this.loadDashboardData();
  }

  // Format date for display
  formatDate(date: string | Date): string {
    if (!date) return 'N/A';
    const d = new Date(date);
    return d.toLocaleDateString();
  }

  // Get status badge class
  getStatusClass(status: StoreStatus): string {
    switch (status) {
      case StoreStatus.Operational:
        return 'status-operational';
      case StoreStatus.UnderMaintenance:
        return 'status-maintenance';
      case StoreStatus.TemporarilyClosed:
        return 'status-closed';
      case StoreStatus.InspectionRequired:
        return 'status-maintenance';
      case StoreStatus.Decommissioned:
        return 'status-closed';
      default:
        return 'status-default';
    }
  }

  // Get status display name
  getStatusDisplayName(status: StoreStatus): string {
    switch (status) {
      case StoreStatus.Operational:
        return 'Operational';
      case StoreStatus.UnderMaintenance:
        return 'Under Maintenance';
      case StoreStatus.TemporarilyClosed:
        return 'Temporarily Closed';
      case StoreStatus.InspectionRequired:
        return 'Inspection Required';
      case StoreStatus.Decommissioned:
        return 'Decommissioned';
      default:
        return 'Unknown';
    }
  }

  // Track items for performance optimization
  trackById(index: number, item: any): any {
    return item.id;
  }
}
