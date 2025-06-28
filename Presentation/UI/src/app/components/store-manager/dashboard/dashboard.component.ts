import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { User } from '../../../core/models/user.model';

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
  
  stats = {
    pendingAddRequests: 5,
    pendingEditRequests: 3,
    pendingDeleteRequests: 2,
    totalNotifications: 8,
    dispatchOrders: 4,
    rejectedRequests: 1,
    approvedRequests: 12,
    explosiveStockEntries: 156
  };

  recentActivities: any[] = [
    { id: 1, action: 'Add Stock Request Submitted', item: 'C4 Explosives - Batch #C4-2024-001', time: '1 hour ago', type: 'add-request', status: 'pending' },
    { id: 2, action: 'Edit Stock Request Approved', item: 'TNT - Batch #TNT-2024-045', time: '2 hours ago', type: 'edit-request', status: 'approved' },
    { id: 3, action: 'Dispatch Notification Received', item: 'PETN - 25kg for Project Alpha', time: '3 hours ago', type: 'dispatch', status: 'prepare' },
    { id: 4, action: 'Delete Request Rejected', item: 'Detonators - Batch #DET-2024-012', time: '4 hours ago', type: 'delete-request', status: 'rejected' },
    { id: 5, action: 'Store Deletion Notification', item: 'Store Beta - Closure Notice', time: '6 hours ago', type: 'store-deletion', status: 'notification' }
  ];

  systemMetrics = {
    requestApprovalRate: '85%',
    averageResponseTime: '2.5 hours',
    dispatchEfficiency: '94%',
    notificationStatus: 'All Read',
    lastStockUpdate: '1 day ago',
    totalBatches: 89
  };

  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router
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
    // Load store manager specific data
    this.isLoading = true;
    
    // Simulate API call
    setTimeout(() => {
      this.isLoading = false;
    }, 1000);
  }

  getUserWelcomeMessage(): string {
    if (!this.currentUser) return 'Welcome, Store Manager';
    
    const timeOfDay = this.getTimeOfDayGreeting();
    return `${timeOfDay}, ${this.currentUser.name}`;
  }

  getInitials(): string {
    if (!this.currentUser?.name) return 'SM';
    
    const names = this.currentUser.name.split(' ');
    if (names.length >= 2) {
      return (names[0][0] + names[1][0]).toUpperCase();
    }
    return names[0].substring(0, 2).toUpperCase();
  }

  private getTimeOfDayGreeting(): string {
    const hour = new Date().getHours();
    if (hour < 12) return 'Good morning';
    if (hour < 17) return 'Good afternoon';
    return 'Good evening';
  }

  getActivityIcon(type: string): string {
    switch (type) {
      case 'add-request': return 'add_box';
      case 'edit-request': return 'edit';
      case 'delete-request': return 'delete';
      case 'dispatch': return 'local_shipping';
      case 'store-deletion': return 'store';
      default: return 'notifications';
    }
  }

  getActivityStatusColor(status: string): string {
    switch (status) {
      case 'pending': return 'status-pending';
      case 'approved': return 'status-approved';
      case 'rejected': return 'status-rejected';
      case 'prepare': return 'status-prepare';
      case 'notification': return 'status-notification';
      default: return '';
    }
  }

  navigateToAddStock(): void {
    this.router.navigate(['/store-manager/add-stock']);
  }

  navigateToEditStock(): void {
    this.router.navigate(['/store-manager/edit-stock']);
  }

  navigateToDeleteStock(): void {
    this.router.navigate(['/store-manager/delete-stock']);
  }

  navigateToNotifications(): void {
    this.router.navigate(['/store-manager/notifications']);
  }

  navigateToDispatch(): void {
    this.router.navigate(['/store-manager/dispatch']);
  }

  refreshDashboard(): void {
    this.loadDashboardData();
  }

  trackActivity(index: number, activity: any): number {
    return activity.id;
  }
}
