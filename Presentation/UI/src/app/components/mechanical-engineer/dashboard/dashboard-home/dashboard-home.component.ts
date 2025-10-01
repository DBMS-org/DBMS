import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';

interface MaintenanceStats {
  totalMachines: number;
  operational: number;
  underMaintenance: number;
  breakdown: number;
  pendingTasks: number;
  scheduledToday: number;
}

interface OperationalStatus {
  running: number;
  idle: number;
  underMaintenance: number;
  breakdown: number;
  available: number;
}

interface MaintenanceAlert {
  id: string;
  machine: string;
  message: string;
  priority: 'HIGH' | 'MEDIUM' | 'LOW';
  timestamp: string;
  icon: string;
}

interface Activity {
  title: string;
  description: string;
  timestamp: string;
  icon: string;
}

@Component({
  selector: 'app-dashboard-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard-home.component.html',
  styleUrl: './dashboard-home.component.scss'
})
export class DashboardHomeComponent implements OnInit {
  currentUser: any = null;
  isLoading = true;

  maintenanceStats: MaintenanceStats = {
    totalMachines: 42,
    operational: 28,
    underMaintenance: 8,
    breakdown: 3,
    pendingTasks: 15,
    scheduledToday: 6
  };

  operationalStatus: OperationalStatus = {
    running: 18,
    idle: 10,
    underMaintenance: 8,
    breakdown: 3,
    available: 3
  };

  maintenanceAlerts: MaintenanceAlert[] = [
    {
      id: '1',
      machine: 'Excavator EX-001',
      message: 'Service threshold reached - 8 hours remaining',
      priority: 'HIGH',
      timestamp: '2 hours ago',
      icon: 'error'
    },
    {
      id: '2',
      machine: 'Drill Rig DR-205',
      message: 'Scheduled maintenance due in 24 hours',
      priority: 'MEDIUM',
      timestamp: '4 hours ago',
      icon: 'schedule'
    },
    {
      id: '3',
      machine: 'Loader LD-103',
      message: 'Oil change required - overdue by 2 days',
      priority: 'HIGH',
      timestamp: '6 hours ago',
      icon: 'warning'
    }
  ];

  recentActivities: Activity[] = [
    {
      title: 'Scheduled Maintenance',
      description: 'Scheduled preventive maintenance for Excavator EX-005',
      timestamp: '30 minutes ago',
      icon: 'schedule'
    },
    {
      title: 'Maintenance Alert Resolved',
      description: 'Resolved hydraulic system alert for Drill Rig DR-102',
      timestamp: '2 hours ago',
      icon: 'check_circle'
    },
    {
      title: 'Machine Status Update',
      description: 'Updated operational status for 5 machines',
      timestamp: '4 hours ago',
      icon: 'update'
    },
    {
      title: 'Service Completed',
      description: 'Completed routine service for Loader LD-201',
      timestamp: '1 day ago',
      icon: 'build'
    },
    {
      title: 'Inventory Check',
      description: 'Performed inventory check on spare parts',
      timestamp: '2 days ago',
      icon: 'inventory'
    }
  ];

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    this.isLoading = true;
    this.currentUser = this.authService.getCurrentUser();
    
    // Simulate loading time
    setTimeout(() => {
      this.isLoading = false;
    }, 1000);
  }

  refreshDashboard() {
    this.loadDashboardData();
  }

  getUserWelcomeMessage(): string {
    if (!this.currentUser) return 'Welcome, Mechanical Engineer';
    
    const hour = new Date().getHours();
    let greeting = 'Good morning';
    
    if (hour >= 12 && hour < 17) {
      greeting = 'Good afternoon';
    } else if (hour >= 17) {
      greeting = 'Good evening';
    }
    
    return `${greeting}, ${this.currentUser.name}!`;
  }

  getInitials(): string {
    if (!this.currentUser?.name) return 'ME';
    
    const names = this.currentUser.name.split(' ');
    if (names.length >= 2) {
      return (names[0][0] + names[1][0]).toUpperCase();
    }
    return names[0].substring(0, 2).toUpperCase();
  }

  getUserLocationInfo(): string {
    return this.currentUser?.region || '';
  }

  getLastLoginInfo(): string {
    return 'Last login: Today at 9:30 AM';
  }

  // Navigation methods
  navigateToMaintenanceManagement() {
    this.router.navigate(['/mechanical-engineer/maintenance']);
  }

  navigateToSchedule() {
    this.router.navigate(['/mechanical-engineer/schedule']);
  }

  navigateToInventory() {
    this.router.navigate(['/mechanical-engineer/inventory']);
  }

  navigateToAlerts() {
    this.router.navigate(['/mechanical-engineer/alerts']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }
}
