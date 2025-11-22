import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../../../core/services/auth.service';
import { MachineService } from '../../../../core/services/machine.service';
import { User } from '../../../../core/models/user.model';
import { Machine, MachineStatus, MachineType } from '../../../../core/models/machine.model';
import { MachineServiceConfigService, ServiceDueAlertDto } from '../../../../services/machine-service-config.service';

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

interface ServiceAlert {
  machineId: number;
  machineName: string;
  serviceType: 'Engine' | 'Drifter';
  hoursRemaining: number;
  urgency: 'good' | 'caution' | 'warning' | 'critical' | 'overdue';
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

  recentActivities: Activity[] = [];
  serviceDueAlerts: ServiceAlert[] = [];
  backendServiceAlerts: ServiceDueAlertDto[] = [];  // New: Backend alerts

  private userSubscription: Subscription = new Subscription();
  private machineSubscription: Subscription = new Subscription();

  constructor(
    private authService: AuthService,
    private machineService: MachineService,
    private machineServiceConfigService: MachineServiceConfigService,
    private router: Router
  ) {}

  ngOnInit() {
    this.userSubscription = this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });

    this.loadMachineData();
    this.loadServiceDueAlerts();  // New: Load from backend
  }

  ngOnDestroy() {
    this.userSubscription.unsubscribe();
    this.machineSubscription.unsubscribe();
  }

  private loadMachineData() {
    this.isLoading = true;
    
    // Load machine statistics
    this.machineSubscription.add(
      this.machineService.getMachineStatistics().subscribe({
        next: (data) => {
          this.updateMachineStats(data);
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading machine statistics:', error);
          this.loadMachinesDirectly(); // Fallback to direct machine loading
        }
      })
    );

    // Load assignment requests count (using mock data for now)
    this.machineSubscription.add(
      this.machineService.getAllAssignmentRequests().subscribe({
        next: (requests) => {
          this.stats.pendingAssignments = requests.filter(r => r.status === 'Pending').length;
          this.systemMetrics.pendingRequests = this.stats.pendingAssignments;
        },
        error: (error) => {
          console.error('Error loading assignment requests:', error);
        }
      })
    );
  }

  private loadMachinesDirectly() {
    // Fallback: Load machines directly and calculate stats
    this.machineSubscription.add(
      this.machineService.getAllMachines().subscribe({
        next: (machines) => {
          this.calculateStatsFromMachines(machines);
          this.calculateServiceAlerts(machines);
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading machines:', error);
          this.isLoading = false;
        }
      })
    );
  }

  private updateMachineStats(apiStats: any) {
    this.stats = {
      ...this.stats,
      totalMachines: apiStats.totalMachines || 0,
      activeMachines: apiStats.assignedMachines || 0,
      availableMachines: apiStats.availableMachines || 0,
      maintenanceMachines: apiStats.maintenanceMachines || 0
    };
  }

  private calculateStatsFromMachines(machines: Machine[]) {
    this.stats = {
      ...this.stats,
      totalMachines: machines.length,
      activeMachines: machines.filter(m => m.status === MachineStatus.ASSIGNED).length,
      availableMachines: machines.filter(m => m.status === MachineStatus.AVAILABLE).length,
      maintenanceMachines: machines.filter(m => m.status === MachineStatus.IN_MAINTENANCE).length
    };
    
    // Generate recent activities based on real machine data
    this.generateRecentActivities(machines);
  }

  private calculateServiceAlerts(machines: Machine[]) {
    const alerts: ServiceAlert[] = [];

    machines.forEach(machine => {
      // Check engine service
      if (machine.engineServiceInterval && machine.engineServiceInterval > 0) {
        const currentHours = machine.currentEngineServiceHours || 0;
        const interval = machine.engineServiceInterval;
        const hoursRemaining = interval - currentHours;

        // Only show alerts if service is due within 100 hours or overdue
        if (hoursRemaining <= 100) {
          alerts.push({
            machineId: machine.id,
            machineName: `${machine.type} ${machine.name}`,
            serviceType: 'Engine',
            hoursRemaining: hoursRemaining,
            urgency: this.getServiceUrgency(hoursRemaining)
          });
        }
      }

      // Check drifter service (only for drill rigs)
      if (machine.type === MachineType.DRILL_RIG && machine.drifterServiceInterval && machine.drifterServiceInterval > 0) {
        const currentHours = machine.currentDrifterServiceHours || 0;
        const interval = machine.drifterServiceInterval;
        const hoursRemaining = interval - currentHours;

        // Only show alerts if service is due within 100 hours or overdue
        if (hoursRemaining <= 100) {
          alerts.push({
            machineId: machine.id,
            machineName: `${machine.type} ${machine.name}`,
            serviceType: 'Drifter',
            hoursRemaining: hoursRemaining,
            urgency: this.getServiceUrgency(hoursRemaining)
          });
        }
      }
    });

    // Sort by urgency (overdue first, then by hours remaining)
    this.serviceDueAlerts = alerts.sort((a, b) => {
      // Overdue items first
      if (a.hoursRemaining < 0 && b.hoursRemaining >= 0) return -1;
      if (a.hoursRemaining >= 0 && b.hoursRemaining < 0) return 1;
      // Then sort by hours remaining (ascending)
      return a.hoursRemaining - b.hoursRemaining;
    });
  }

  /**
   * Load service due alerts from backend API
   * NEW: Uses real backend API instead of calculating from machines
   */
  private loadServiceDueAlerts() {
    this.machineSubscription.add(
      this.machineServiceConfigService.getServiceDueAlerts().subscribe({
        next: (alerts: ServiceDueAlertDto[]) => {
          this.backendServiceAlerts = alerts;

          // Convert backend alerts to frontend format
          this.serviceDueAlerts = alerts.map(alert => ({
            machineId: alert.machineId,
            machineName: alert.machineName,
            serviceType: alert.serviceType as 'Engine' | 'Drifter',
            hoursRemaining: alert.hoursRemaining,
            urgency: this.mapUrgencyLevelToFrontend(alert.urgencyLevel)
          }));

          // Update critical alerts count
          this.systemMetrics.criticalAlerts = alerts.filter(a =>
            a.urgencyLevel === 'RED' || a.urgencyLevel === 'ORANGE'
          ).length;
        },
        error: (error) => {
          console.error('Error loading service due alerts:', error);
          // Fallback to calculating from machines if API fails
          this.loadMachinesDirectly();
        }
      })
    );
  }

  /**
   * Map backend urgency level to frontend urgency
   */
  private mapUrgencyLevelToFrontend(urgencyLevel: string): 'good' | 'caution' | 'warning' | 'critical' | 'overdue' {
    switch (urgencyLevel) {
      case 'RED':
        return 'overdue';  // < 0 hours (overdue) or critically low
      case 'ORANGE':
        return 'critical'; // < 20 hours
      case 'YELLOW':
        return 'warning';  // < 50 hours
      case 'GREEN':
        return 'caution';  // < 100 hours
      default:
        return 'good';
    }
  }

  private getServiceUrgency(hoursRemaining: number): 'good' | 'caution' | 'warning' | 'critical' | 'overdue' {
    if (hoursRemaining < 0) return 'overdue';
    if (hoursRemaining < 20) return 'critical';
    if (hoursRemaining < 50) return 'warning';
    if (hoursRemaining < 100) return 'caution';
    return 'good';
  }

  private generateRecentActivities(machines: Machine[]) {
    const activities: Activity[] = [];

    // Add activities based on recent machines (sorted by creation date)
    const recentMachines = machines
      .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
      .slice(0, 2);

    recentMachines.forEach((machine, index) => {
      const timeDiff = Date.now() - new Date(machine.createdAt).getTime();
      const hoursAgo = Math.floor(timeDiff / (1000 * 60 * 60));
      const daysAgo = Math.floor(hoursAgo / 24);

      let timeText = '';
      if (daysAgo > 0) {
        timeText = `${daysAgo} day${daysAgo > 1 ? 's' : ''} ago`;
      } else if (hoursAgo > 0) {
        timeText = `${hoursAgo} hour${hoursAgo > 1 ? 's' : ''} ago`;
      } else {
        timeText = 'Recently';
      }

      activities.push({
        icon: 'inventory_2',
        title: 'Machine Added',
        description: `${machine.type} ${machine.name} added to inventory`,
        timestamp: timeText,
        type: 'inventory'
      });
    });

    // Add some mock activities for features we don't have APIs for yet
    activities.push(
      {
        icon: 'assignment',
        title: 'Assignment Request',
        description: 'Machine assignment request pending review',
        timestamp: '2 hours ago',
        type: 'assignment'
      },
      {
        icon: 'warning',
        title: 'Low Stock Alert',
        description: 'Some accessories below threshold (mock data)',
        timestamp: '4 hours ago',
        type: 'accessories'
      },
      {
        icon: 'engineering',
        title: 'Maintenance Scheduled',
        description: 'Preventive maintenance scheduled (mock data)',
        timestamp: '6 hours ago',
        type: 'maintenance'
      }
    );

    this.recentActivities = activities.slice(0, 5); // Keep only top 5 activities
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
    this.loadMachineData();
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
    this.router.navigate(['/machine-manager/drilling-components']);
  }

  navigateToMaintenanceManagement() {
    this.router.navigate(['/machine-manager/maintenance-management']);
  }

  navigateToMachineDetails(machineId: number) {
    this.router.navigate(['/machine-manager/machine-inventory'], {
      queryParams: { selectedMachineId: machineId }
    });
  }

  getActivityTypeClass(type: string): string {
    return `activity-${type}`;
  }

  getUrgencyClass(urgency: string): string {
    return `urgency-${urgency}`;
  }
}
