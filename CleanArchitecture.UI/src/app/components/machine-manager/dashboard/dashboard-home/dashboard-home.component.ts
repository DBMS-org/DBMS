import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../../../core/services/auth.service';
import { User } from '../../../../core/models/user.model';

interface MachineStats {
  activeMachines: number;
  maintenanceTasks: number;
  efficiencyRate: number;
  downtimeHours: number;
  equipmentHealth: number;
  totalEquipment: number;
  reportsGenerated: number;
}

interface SystemMetrics {
  systemStatus: string;
  productionRate: string;
  nextMaintenance: string;
  maintenanceStatus: string;
}

interface Activity {
  icon: string;
  title: string;
  description: string;
  timestamp: string;
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
    activeMachines: 24,
    maintenanceTasks: 8,
    efficiencyRate: 92,
    downtimeHours: 3.5,
    equipmentHealth: 89,
    totalEquipment: 156,
    reportsGenerated: 47
  };

  systemMetrics: SystemMetrics = {
    systemStatus: 'Operational',
    productionRate: '95% Capacity',
    nextMaintenance: '3 days',
    maintenanceStatus: 'scheduled'
  };

  recentActivities: Activity[] = [
    {
      icon: 'precision_manufacturing',
      title: 'Machine Status Updated',
      description: 'Updated status for Excavator Unit #5',
      timestamp: '1 hour ago'
    },
    {
      icon: 'build_circle',
      title: 'Maintenance Completed',
      description: 'Routine maintenance completed for Drill Unit #3',
      timestamp: '3 hours ago'
    },
    {
      icon: 'warning',
      title: 'Equipment Alert',
      description: 'Performance alert for Crusher Unit #2',
      timestamp: '5 hours ago'
    },
    {
      icon: 'assessment',
      title: 'Weekly Report Generated',
      description: 'Machine efficiency report for Week 25',
      timestamp: '1 day ago'
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
    const name = this.currentUser?.name || 'Machine Manager';
    const hour = new Date().getHours();
    let greeting = 'Good morning';
    
    if (hour >= 12 && hour < 17) {
      greeting = 'Good afternoon';
    } else if (hour >= 17) {
      greeting = 'Good evening';
    }
    
    return `${greeting}, ${name}!`;
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

  navigateToOperations() {
    this.router.navigate(['/machine-manager/operations']);
  }

  navigateToMaintenance() {
    this.router.navigate(['/machine-manager/maintenance']);
  }

  navigateToEquipment() {
    this.router.navigate(['/machine-manager/equipment']);
  }

  navigateToReports() {
    this.router.navigate(['/machine-manager/reports']);
  }
}
