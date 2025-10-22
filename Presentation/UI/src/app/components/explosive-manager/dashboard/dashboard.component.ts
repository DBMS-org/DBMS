import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { User } from '../../../core/models/user.model';

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

  // Dashboard statistics for inventory and safety
  stats = {
    totalInventory: 24,
    safetyProtocols: 12,
    monthlyUsage: 156,
    safetyIncidents: 0
  };

  // Recent activity log for tracking changes
  recentActivities: any[] = [
    { id: 1, action: 'Inventory updated', item: 'C4 Explosives', time: '2 hours ago', type: 'inventory' },
    { id: 2, action: 'Safety protocol reviewed', item: 'Blast Zone Safety', time: '4 hours ago', type: 'safety' },
    { id: 3, action: 'Operation completed', item: 'Mine Blast #45', time: '6 hours ago', type: 'operation' },
    { id: 4, action: 'Alert resolved', item: 'Storage Temperature', time: '8 hours ago', type: 'alert' }
  ];

  // Environmental and security monitoring data
  systemMetrics = {
    storageTemperature: '18°C',
    humidityLevel: '45%',
    securityStatus: 'Secure',
    lastInspection: '2 days ago'
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

  // Subscribe to user authentication state
  private subscribeToCurrentUser() {
    this.userSubscription = this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  // Load all dashboard data from backend
  private loadDashboardData() {
    this.isLoading = true;

    // Simulate API call
    setTimeout(() => {
      this.isLoading = false;
    }, 1000);
  }

  // Generate personalized greeting message
  getUserWelcomeMessage(): string {
    if (!this.currentUser) return 'Welcome, Explosive Manager';

    const timeOfDay = this.getTimeOfDayGreeting();
    return `${timeOfDay}, ${this.currentUser.name}`;
  }

  // Generate user initials for avatar
  getInitials(): string {
    if (!this.currentUser?.name) return 'EM';

    const names = this.currentUser.name.split(' ');
    if (names.length >= 2) {
      return (names[0][0] + names[1][0]).toUpperCase();
    }
    return names[0].substring(0, 2).toUpperCase();
  }

  // Get time-based greeting
  private getTimeOfDayGreeting(): string {
    const hour = new Date().getHours();
    if (hour < 12) return 'Good morning';
    if (hour < 17) return 'Good afternoon';
    return 'Good evening';
  }

  // Map activity type to corresponding icon
  getActivityIcon(type: string): string {
    switch (type) {
      case 'inventory': return 'inventory';
      case 'safety': return 'security';
      case 'operation': return 'work';
      case 'alert': return 'warning';
      default: return 'info';
    }
  }

  // Navigate to inventory management page
  navigateToInventory(): void {
    this.router.navigate(['/explosive-manager/inventory']);
  }

  // Navigate to safety protocols page
  navigateToSafety(): void {
    this.router.navigate(['/explosive-manager/safety']);
  }

  // Reload dashboard data
  refreshDashboard(): void {
    this.loadDashboardData();
  }

  // Track activities for performance optimization
  trackActivity(index: number, activity: any): number {
    return activity.id;
  }
}
