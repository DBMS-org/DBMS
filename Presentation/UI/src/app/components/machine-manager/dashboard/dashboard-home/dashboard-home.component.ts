import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription, forkJoin } from 'rxjs';
import { AuthService } from '../../../../core/services/auth.service';
import { MachineService } from '../../../../core/services/machine.service';
import { User } from '../../../../core/models/user.model';
import { Machine, MachineStatus, MachineType } from '../../../../core/models/machine.model';
import { MachineServiceConfigService, ServiceDueAlertDto } from '../../../../services/machine-service-config.service';
import { UsageLogService } from '../../../../services/usage-log.service';
import { UsageLogDto } from '../../../operator/my-machines/models/usage-log.models';

interface MachineStats {
  totalMachines: number;
  activeMachines: number;
  availableMachines: number;
  maintenanceMachines: number;
  pendingAssignments: number;
  scheduledMaintenance: number;
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
    scheduledMaintenance: 8
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
    private usageLogService: UsageLogService,
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
          this.loadRecentActivities(); // Load activities after stats
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading machine statistics:', error);
          this.loadMachinesDirectly(); // Fallback to direct machine loading
        }
      })
    );

    // Load assignment requests count
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

    // Load recent activities from real backend data
    this.loadRecentActivities();
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

  /**
   * Load recent activities from real backend data
   * Combines usage logs, assignment requests, and service alerts
   */
  private loadRecentActivities() {
    const activities: Activity[] = [];
    const now = new Date();
    const sevenDaysAgo = new Date(now.getTime() - 7 * 24 * 60 * 60 * 1000);

    // Parallel load all activity sources
    this.machineSubscription.add(
      forkJoin({
        machines: this.machineService.getAllMachines(),
        assignmentRequests: this.machineService.getAllAssignmentRequests(),
        activeAssignments: this.machineService.getActiveAssignments()
      }).subscribe({
        next: (data) => {
          // 1. Recent machine additions (last 7 days)
          const recentMachines = data.machines
            .filter(m => new Date(m.createdAt).getTime() > sevenDaysAgo.getTime())
            .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
            .slice(0, 3);

          recentMachines.forEach(machine => {
            activities.push({
              icon: 'inventory_2',
              title: 'Machine Added',
              description: `${machine.type} ${machine.name} added to inventory`,
              timestamp: this.formatTimestamp(machine.createdAt),
              type: 'inventory'
            });
          });

          // 2. Recent assignment requests (last 7 days)
          const recentRequests = data.assignmentRequests
            .filter(r => new Date(r.requestedDate).getTime() > sevenDaysAgo.getTime())
            .sort((a, b) => new Date(b.requestedDate).getTime() - new Date(a.requestedDate).getTime())
            .slice(0, 3);

          recentRequests.forEach(request => {
            let activityTitle = '';
            let activityType: 'assignment' | 'inventory' | 'accessories' | 'maintenance' | 'alert' = 'assignment';

            if (request.status === 'Approved') {
              activityTitle = 'Assignment Approved';
              activityType = 'inventory';
            } else if (request.status === 'Rejected') {
              activityTitle = 'Assignment Rejected';
              activityType = 'alert';
            } else {
              activityTitle = 'Assignment Request';
            }

            activities.push({
              icon: request.status === 'Approved' ? 'check_circle' :
                    request.status === 'Rejected' ? 'cancel' : 'assignment',
              title: activityTitle,
              description: `${request.quantity} ${request.machineType} for ${request.requestedBy}`,
              timestamp: this.formatTimestamp(request.requestedDate),
              type: activityType
            });
          });

          // 3. Recent active assignments (last 7 days)
          const recentAssignments = data.activeAssignments
            .filter(a => new Date(a.assignedDate).getTime() > sevenDaysAgo.getTime())
            .sort((a, b) => new Date(b.assignedDate).getTime() - new Date(a.assignedDate).getTime())
            .slice(0, 2);

          recentAssignments.forEach(assignment => {
            activities.push({
              icon: 'assignment_turned_in',
              title: 'Machine Assigned',
              description: `${assignment.machineName} assigned to ${assignment.operatorName}`,
              timestamp: this.formatTimestamp(assignment.assignedDate),
              type: 'assignment'
            });
          });

          // 4. Load usage logs for assigned machines (recent 5)
          this.loadUsageLogsActivities(data.machines, sevenDaysAgo, activities);

          // 5. Add service alerts as activities (critical ones)
          const criticalAlerts = this.serviceDueAlerts
            .filter(alert => alert.urgency === 'overdue' || alert.urgency === 'critical')
            .slice(0, 2);

          criticalAlerts.forEach(alert => {
            activities.push({
              icon: 'warning',
              title: alert.urgency === 'overdue' ? 'Service Overdue' : 'Service Due Soon',
              description: `${alert.machineName} - ${alert.serviceType} service ${alert.urgency === 'overdue' ? 'overdue' : 'critical'}`,
              timestamp: alert.urgency === 'overdue' ? 'Overdue' : 'Due soon',
              type: 'alert'
            });
          });

          // Sort all activities by timestamp and limit to 10
          this.recentActivities = activities
            .sort((a, b) => this.getTimestampValue(b.timestamp) - this.getTimestampValue(a.timestamp))
            .slice(0, 10);
        },
        error: (error) => {
          console.error('Error loading recent activities:', error);
          // Fallback to empty activities
          this.recentActivities = [];
        }
      })
    );
  }

  /**
   * Load usage log activities for assigned machines
   */
  private loadUsageLogsActivities(machines: Machine[], sevenDaysAgo: Date, activities: Activity[]) {
    // Get assigned machines to fetch their usage logs
    const assignedMachines = machines
      .filter(m => m.status === MachineStatus.ASSIGNED)
      .slice(0, 5); // Limit to 5 machines to avoid too many API calls

    if (assignedMachines.length === 0) {
      return;
    }

    // Fetch usage logs for each assigned machine
    const usageLogRequests = assignedMachines.map(machine =>
      this.usageLogService.getUsageLogsByMachine(machine.id, sevenDaysAgo, new Date())
    );

    this.machineSubscription.add(
      forkJoin(usageLogRequests).subscribe({
        next: (usageLogsArrays) => {
          // Flatten all usage logs and filter recent ones
          const allUsageLogs: UsageLogDto[] = usageLogsArrays.flat();

          const recentUsageLogs = allUsageLogs
            .filter(log => new Date(log.createdAt).getTime() > sevenDaysAgo.getTime())
            .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
            .slice(0, 5);

          recentUsageLogs.forEach(log => {
            const workingHoursText = log.workingHours > 0 ? `${log.workingHours.toFixed(1)}h worked` : '';
            const downtimeText = log.hasDowntime && log.downtimeHours ? `, ${log.downtimeHours.toFixed(1)}h downtime` : '';

            activities.push({
              icon: log.hasDowntime ? 'report_problem' : 'schedule',
              title: log.hasDowntime ? 'Usage Log (Downtime)' : 'Usage Log Submitted',
              description: `${log.machineName}: ${workingHoursText}${downtimeText}`,
              timestamp: this.formatTimestamp(log.createdAt),
              type: log.hasDowntime ? 'alert' : 'maintenance'
            });
          });

          // Re-sort all activities after adding usage logs
          this.recentActivities = activities
            .sort((a, b) => this.getTimestampValue(b.timestamp) - this.getTimestampValue(a.timestamp))
            .slice(0, 10);
        },
        error: (error) => {
          console.error('Error loading usage logs for activities:', error);
          // Continue without usage logs
        }
      })
    );
  }

  /**
   * Format timestamp to human-readable format
   */
  private formatTimestamp(date: Date | string): string {
    const dateObj = typeof date === 'string' ? new Date(date) : date;
    const timeDiff = Date.now() - dateObj.getTime();
    const hoursAgo = Math.floor(timeDiff / (1000 * 60 * 60));
    const daysAgo = Math.floor(hoursAgo / 24);

    if (daysAgo > 7) {
      return dateObj.toLocaleDateString();
    } else if (daysAgo > 0) {
      return `${daysAgo} day${daysAgo > 1 ? 's' : ''} ago`;
    } else if (hoursAgo > 0) {
      return `${hoursAgo} hour${hoursAgo > 1 ? 's' : ''} ago`;
    } else {
      return 'Just now';
    }
  }

  /**
   * Get numeric value for timestamp sorting
   */
  private getTimestampValue(timestamp: string): number {
    if (timestamp === 'Just now') return Date.now();
    if (timestamp === 'Overdue' || timestamp === 'Due soon') return Date.now();

    const match = timestamp.match(/(\d+)\s+(hour|day)/);
    if (match) {
      const value = parseInt(match[1]);
      const unit = match[2];
      const milliseconds = unit === 'hour' ? value * 60 * 60 * 1000 : value * 24 * 60 * 60 * 1000;
      return Date.now() - milliseconds;
    }

    return 0;
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
