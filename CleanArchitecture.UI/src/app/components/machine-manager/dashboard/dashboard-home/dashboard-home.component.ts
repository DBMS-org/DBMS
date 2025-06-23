import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../../../core/services/auth.service';
import { User } from '../../../../core/models/user.model';

interface MachineStats {
  totalMachines: number;
  activeMachines: number;
  availableMachines: number;
  maintenanceMachines: number;
  pendingAssignments: number;
  lowStockAccessories: number;
  scheduledMaintenance: number;
  accessoriesTotal: number;
}

interface SystemMetrics {
  systemStatus: string;
  pendingRequests: number;
  criticalAlerts: number;
}

interface Activity {
  icon: string;
  title: string;
  description: string;
  timestamp: string;
  type: 'inventory' | 'assignment' | 'accessories' | 'maintenance' | 'alert';
}

@Component({
  selector: 'app-dashboard-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.scss']
})
export class DashboardHomeComponent implements OnInit, OnDestroy {
  currentUser: User | null = null;
  isLoading = true;
  
  stats: MachineStats = {
    totalMachines: 42,
    activeMachines: 24,
    availableMachines: 15,
    maintenanceMachines: 3,
    pendingAssignments: 6,
    lowStockAccessories: 4,
    scheduledMaintenance: 8,
    accessoriesTotal: 156
  };

  systemMetrics: SystemMetrics = {
    systemStatus: 'Operational',
    pendingRequests: 6,
    criticalAlerts: 2
  };

  recentActivities: Activity[] = [
    {
      icon: 'inventory_2',
      title: 'New Machine Added',
      description: 'Excavator CAT 320D added to inventory',
      timestamp: '30 minutes ago',
      type: 'inventory'
    },
    {
      icon: 'assignment',
      title: 'Assignment Request',
      description: 'Machine assignment request for Project #2024-05',
      timestamp: '1 hour ago',
      type: 'assignment'
    },
    {
      icon: 'warning',
      title: 'Low Stock Alert',
      description: 'Hydraulic filters below threshold (5 remaining)',
      timestamp: '2 hours ago',
      type: 'accessories'
    },
    {
      icon: 'engineering',
      title: 'Maintenance Scheduled',
      description: 'Preventive maintenance scheduled for Drill Rig #DR-003',
      timestamp: '3 hours ago',
      type: 'maintenance'
    },
    {
      icon: 'check_circle',
      title: 'Assignment Completed',
      description: 'Machine assignment approved for Project #2024-04',
      timestamp: '4 hours ago',
      type: 'assignment'
    }
  ];

  private userSubscription: Subscription = new Subscription();

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.userSubscription = this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
    
    // Simulate loading
    setTimeout(() => {
      this.isLoading = false;
    }, 1000);
  }

  ngOnDestroy() {
    this.userSubscription.unsubscribe();
  }

  getUserWelcomeMessage(): string {
    if (!this.currentUser) return 'Welcome, Machine Manager';
    
    const timeOfDay = this.getTimeOfDayGreeting();
    return `${timeOfDay}, ${this.currentUser.name}`;
  }

  getInitials(): string {
    if (!this.currentUser?.name) return 'MM';
    
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

  getUserLocationInfo(): string {
    return this.currentUser?.region || '';
  }

  getLastLoginInfo(): string {
    return 'Last login: Today at 8:15 AM';
  }

  refreshDashboard() {
    this.isLoading = true;
    // Simulate refresh
    setTimeout(() => {
      this.isLoading = false;
    }, 1000);
  }

  logout() {
    this.authService.logout();
  }

  // Navigation methods for new features
  navigateToMachineInventory() {
    this.router.navigate(['/machine-manager/machine-inventory']);
  }

  navigateToAssignmentRequests() {
    this.router.navigate(['/machine-manager/assignment-requests']);
  }

  navigateToAccessoriesInventory() {
    this.router.navigate(['/machine-manager/accessories-inventory']);
  }

  navigateToMaintenanceManagement() {
    this.router.navigate(['/machine-manager/maintenance-management']);
  }

  getActivityTypeClass(type: string): string {
    return `activity-${type}`;
  }
}
