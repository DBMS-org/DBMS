import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { Subscription, forkJoin } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { ProjectService } from '../../../core/services/project.service';
import { SiteService, ProjectSite } from '../../../core/services/site.service';
import { DrillHoleService } from '../../../core/services/drill-hole.service';
import { SiteBlastingService } from '../../../core/services/site-blasting.service';
import { Project } from '../../../core/models/project.model';
import { User } from '../../../core/models/user.model';
import { UnifiedDrillDataService } from '../../../core/services/unified-drill-data.service';
import { MaintenanceReportService } from '../maintenance-reports/services/maintenance-report.service';
import { OperatorMachine, ProblemReport } from '../maintenance-reports/models/maintenance-report.models';
import { MaintenanceService } from '../../mechanical-engineer/maintenance/services/maintenance.service';
import { UsageMetrics } from '../../mechanical-engineer/maintenance/models/maintenance.models';
import { NotificationService } from '../../../core/services/notification.service';
import { Notification } from '../../../core/models/notification.model';
import { UsageLogService } from '../../../services/usage-log.service';
import { UsageStatisticsDto } from '../my-machines/models/usage-log.models';

@Component({
  selector: 'app-operator-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class OperatorDashboardComponent implements OnInit, OnDestroy {
  currentUser: User | null = null;
  assignedProject: Project | null = null;
  projectSites: ProjectSite[] = [];
  isLoading = true;
  error: string | null = null;

  // Machine & Maintenance data
  assignedMachine: OperatorMachine | null = null;
  machineUsageMetrics: UsageMetrics | null = null;
  machineUsageStatistics: UsageStatisticsDto | null = null;
  maintenanceReports: ProblemReport[] = [];
  serviceIntervalHours = 500;

  // Notifications
  recentNotifications: Notification[] = [];
  unreadNotificationsCount = 0;

  private subscriptions = new Subscription();

  stats = {
    totalSites: 0,
    sitesWithPatterns: 0,
    sitesWithSequences: 0,
    totalDrillHoles: 0,
    approvedPatterns: 0,
    confirmedSimulations: 0,
    projectProgress: 0,
    activeWorkflows: 0
  };

  recentActivities: any[] = [];

  systemMetrics = {
    projectStatus: 'Unknown',
    lastPatternUpdate: 'N/A',
    workflowCompletion: '0%',
    totalProgress: '0%'
  };

  constructor(
    private authService: AuthService,
    private projectService: ProjectService,
    private siteService: SiteService,
    private drillHoleService: DrillHoleService,
    private siteBlastingService: SiteBlastingService,
    private router: Router,
    private unifiedDrillDataService: UnifiedDrillDataService,
    private maintenanceReportService: MaintenanceReportService,
    private maintenanceService: MaintenanceService,
    private notificationService: NotificationService,
    private usageLogService: UsageLogService
  ) {}

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    if (this.currentUser) {
      this.loadOperatorData();
    } else {
      this.error = 'User not authenticated';
      this.isLoading = false;
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  private loadOperatorData(): void {
    if (!this.currentUser) return;

    this.isLoading = true;
    this.error = null;

    const projectSub = this.projectService.getProjectByOperator(this.currentUser.id).subscribe({
      next: (project) => {
        this.assignedProject = project;
        if (project) {
          this.loadProjectDetails(project.id);
        } else {
          console.log('No project assigned to this operator');
          this.isLoading = false;
          this.error = 'No project is currently assigned to you. Please contact your administrator.';
        }
      },
      error: (err) => {
        console.error('Failed to load project', err);
        this.error = 'Failed to load assigned project';
        this.isLoading = false;
      }
    });

    this.subscriptions.add(projectSub);

    // Load machine and maintenance data
    this.loadMachineData();

    // Load notifications
    this.loadNotifications();
  }

  private loadProjectDetails(projectId: number): void {
    const sitesSub = this.siteService.getProjectSites(projectId).subscribe({
      next: (sites) => {
        this.projectSites = sites.map(s => ({
          ...s,
          createdAt: new Date(s.createdAt),
          updatedAt: new Date(s.updatedAt)
        }));

        // Update statistics without drill holes first
        this.updateStatistics(sites);
        this.loadDrillHolesForSites(projectId, sites);
        this.loadSiteSpecificData();
        this.updateRecentActivities();
        this.updateSystemMetrics();
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load project sites', err);
        this.error = 'Failed to load project sites';
        this.isLoading = false;
      }
    });

    this.subscriptions.add(sitesSub);
  }

  private loadDrillHolesForSites(projectId: number, sites: ProjectSite[]): void{
    let totalCompletedDrillHoles = 0;
    let sitesProcessed = 0;

    sites.forEach(site => {
      const drillPointsSub = this.unifiedDrillDataService.getDrillPoints(projectId, site.id).subscribe({
        next: (drillPoints) => {
          // Count completed drill points for this site
          const completedCount = drillPoints.filter(dp => dp.isCompleted).length;
          totalCompletedDrillHoles += completedCount;
          sitesProcessed++;

          console.log(`Site ${site.id}: ${completedCount} completed drill holes out of ${drillPoints.length}`);

          // Update the total count
          this.stats.totalDrillHoles = totalCompletedDrillHoles;

          // Log the running total
          if (sitesProcessed === sites.length) {
            console.log(`Total completed drill holes across all sites: ${totalCompletedDrillHoles}`);
          }
        },
        error: (err) => {
          console.warn('Failed to load drill points for site', site.id, err);
          sitesProcessed++;
        }
      });
      this.subscriptions.add(drillPointsSub);
    });
  }

  private loadSiteSpecificData(): void {
    if (!this.assignedProject || this.projectSites.length === 0) return;

    this.projectSites.forEach(site => {
      const drillPointsSub = this.unifiedDrillDataService.getDrillPoints(this.assignedProject!.id, site.id).subscribe({
        next: (drillPoints) => {
          if (drillPoints.length > 0) {
            this.stats.sitesWithPatterns++;
          }
        },
        error: (err) => console.warn('Failed to load drill points for site', site.id, err)
      });
      this.subscriptions.add(drillPointsSub);
    });
  }

  private updateStatistics(sites: ProjectSite[]): void {
    this.stats.totalSites = sites.length;
    // Don't reset totalDrillHoles - it's updated by loadDrillHolesForSites
    this.stats.approvedPatterns = sites.filter(s => s.isPatternApproved).length;
    this.stats.confirmedSimulations = sites.filter(s => s.isSimulationConfirmed).length;

    if (sites.length > 0) {
      const completedSites = sites.filter(s => s.isPatternApproved && s.isSimulationConfirmed).length;
      this.stats.projectProgress = Math.round((completedSites / sites.length) * 100);
    }

    this.stats.activeWorkflows = sites.filter(s => !s.isPatternApproved || !s.isSimulationConfirmed).length;
  }

  private updateRecentActivities(): void{
    this.recentActivities = [];
    
    if (this.assignedProject) {
      this.recentActivities.push({
        id: 1,
        action: 'Project Assigned',
        item: this.assignedProject.name,
        time: this.formatRelativeTime(this.assignedProject.startDate),
        type: 'project',
        status: 'active'
      });
    }

    this.projectSites.slice(0, 3).forEach((site, index) => {
      this.recentActivities.push({
        id: index + 2,
        action: site.isPatternApproved ? 'Pattern Approved' : 'Pattern Pending',
        item: site.name,
        time: this.formatRelativeTime(site.updatedAt),
        type: 'pattern',
        status: site.isPatternApproved ? 'approved' : 'pending'
      });
    });

    this.recentActivities = this.recentActivities.slice(0, 5);
  }

  private updateSystemMetrics(): void {
    if (this.assignedProject) {
      this.systemMetrics.projectStatus = this.assignedProject.status;
      this.systemMetrics.totalProgress = `${this.stats.projectProgress}%`;
    }

    if (this.projectSites.length > 0) {
      const latestUpdate = Math.max(...this.projectSites.map(s => s.updatedAt.getTime()));
      this.systemMetrics.lastPatternUpdate = this.formatRelativeTime(new Date(latestUpdate));
      
      const completionRate = this.stats.totalSites > 0 
        ? Math.round(((this.stats.approvedPatterns + this.stats.confirmedSimulations) / (this.stats.totalSites * 2)) * 100)
        : 0;
      this.systemMetrics.workflowCompletion = `${completionRate}%`;
    }
  }

  private formatRelativeTime(date?: Date): string {
    if (!date) return 'N/A';
    
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffHours = Math.floor(diffMs / (1000 * 60 * 60));
    const diffDays = Math.floor(diffHours / 24);
    
    if (diffDays > 0) {
      return `${diffDays} day${diffDays > 1 ? 's' : ''} ago`;
    } else if (diffHours > 0) {
      return `${diffHours} hour${diffHours > 1 ? 's' : ''} ago`;
    } else {
      return 'Recently';
    }
  }

  formatDate(date?: Date): string {
    return date ? new Date(date).toLocaleDateString() : '-';
  }

  getUserWelcomeMessage(): string {
    if (!this.currentUser) return 'Welcome, Operator';
    
    const timeOfDay = this.getTimeOfDayGreeting();
    return `${timeOfDay}, ${this.currentUser.name}`;
  }

  getInitials(): string {
    if (!this.currentUser?.name) return 'OP';
    
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

  // Map activity type to Material icon name
  getActivityIcon(type: string): string {
    switch (type) {
      case 'project': return 'assignment';
      case 'pattern': return 'grid_on';
      case 'sequence': return 'timeline';
      case 'drill': return 'construction';
      default: return 'info';
    }
  }

  // Get CSS class for activity status styling
  getActivityStatusColor(status: string): string {
    switch (status) {
      case 'active': return 'status-active';
      case 'approved': return 'status-approved';
      case 'pending': return 'status-pending';
      case 'completed': return 'status-completed';
      default: return '';
    }
  }

  // Navigate to project overview page
  navigateToProject(): void {
    this.router.navigate(['/operator/my-project']);
  }

  navigateToSites(): void {
-    this.router.navigate(['/operator/my-project/sites']);
+    this.router.navigate(['/operator/my-project'], { queryParams: { showSites: true } });
   }

  navigateToSite(siteId: number): void {
    this.router.navigate(['/operator/my-project/sites', siteId, 'pattern-view']);
  }

  // Reload all dashboard data
  refreshDashboard(): void {
    this.loadOperatorData();
  }

  // Track function for Angular's ngFor optimization
  trackActivity(index: number, activity: any): number {
    return activity.id;
  }

  // Load machine and maintenance data
  private loadMachineData(): void {
    if (!this.currentUser?.id) return;

    const machineSub = this.maintenanceReportService.getOperatorMachine(this.currentUser.id).subscribe({
      next: (machine) => {
        this.assignedMachine = machine;
        if (machine?.id) {
          this.loadMachineUsageMetrics(machine.id);
          this.loadMachineUsageStatistics(machine.id);
        }
      },
      error: (err) => {
        console.error('Failed to load operator machine:', err);
        // Don't set error state - machine might not be assigned yet
      }
    });

    const reportsSub = this.maintenanceReportService.getOperatorReports(this.currentUser.id).subscribe({
      next: (reports) => {
        console.log('🔧 DASHBOARD: Raw maintenance reports received:', reports);
        console.log('🔧 DASHBOARD: Number of reports:', reports?.length || 0);

        this.maintenanceReports = reports.sort((a, b) =>
          new Date(b.reportedAt).getTime() - new Date(a.reportedAt).getTime()
        );

        console.log('🔧 DASHBOARD: Sorted maintenance reports:', this.maintenanceReports);
        console.log('🔧 DASHBOARD: Critical count:', this.getCriticalReportsCount());
        console.log('🔧 DASHBOARD: Active count:', this.getActiveReportsCount());
        console.log('🔧 DASHBOARD: Completed count:', this.getCompletedReportsCount());
      },
      error: (err) => {
        console.error('❌ DASHBOARD: Failed to load maintenance reports:', err);
      }
    });

    this.subscriptions.add(machineSub);
    this.subscriptions.add(reportsSub);
  }

  private loadMachineUsageMetrics(machineId: number): void {
    const metricsSub = this.maintenanceService.getUsageMetrics(machineId).subscribe({
      next: (metricsArr) => {
        this.machineUsageMetrics = metricsArr && metricsArr.length ? metricsArr[0] : null;
      },
      error: (err) => {
        console.error('Failed to load usage metrics:', err);
      }
    });

    this.subscriptions.add(metricsSub);
  }

  private loadMachineUsageStatistics(machineId: number): void {
    const statsSub = this.usageLogService.getUsageStatistics(machineId, 7).subscribe({
      next: (stats) => {
        this.machineUsageStatistics = stats;
      },
      error: (err) => {
        console.error('Failed to load usage statistics:', err);
      }
    });

    this.subscriptions.add(statsSub);
  }

  private loadNotifications(): void {
    const notificationsSub = this.notificationService.fetchNotifications(0, 5).subscribe({
      next: (notifications) => {
        console.log('🔔 DASHBOARD: Raw notifications received:', notifications);
        console.log('🔔 DASHBOARD: Number of notifications:', notifications?.length || 0);

        // Filter notifications relevant to Operator role
        const operatorNotifications = notifications.filter(n => this.isOperatorRelevantNotification(n.type));
        console.log('🔔 DASHBOARD: Filtered operator notifications:', operatorNotifications);
        console.log('🔔 DASHBOARD: Operator notifications count:', operatorNotifications.length);

        this.recentNotifications = operatorNotifications.slice(0, 5);
        this.unreadNotificationsCount = operatorNotifications.filter(n => !n.isRead).length;

        console.log('🔔 DASHBOARD: Final recentNotifications:', this.recentNotifications);
        console.log('🔔 DASHBOARD: Unread count:', this.unreadNotificationsCount);
      },
      error: (err) => {
        console.error('❌ DASHBOARD: Failed to load notifications:', err);
      }
    });

    this.subscriptions.add(notificationsSub);
  }

  private isOperatorRelevantNotification(type: any): boolean {
    // Handle both numeric enum values and string enum names
    let typeValue: number;

    if (typeof type === 'number') {
      typeValue = type;
    } else if (typeof type === 'string') {
      // Try to parse as number first
      typeValue = parseInt(type, 10);

      // If that didn't work, the backend might be sending the enum name as string
      if (isNaN(typeValue)) {
        console.log('⚠️ DASHBOARD: Non-numeric notification type:', type);
        // Include by default if we can't parse it - better to show than hide
        return true;
      }
    } else {
      console.log('⚠️ DASHBOARD: Unknown notification type format:', type);
      return true; // Include by default
    }

    console.log('🔍 DASHBOARD: Checking notification type value:', typeValue);

    // Handle unknown/undefined types (type 0 or undefined)
    if (typeValue === 0 || typeValue === -1 || typeValue === undefined || typeValue === null) {
      console.log('⚠️ DASHBOARD: Unknown notification type detected, including by default');
      return true;
    }

    // Machine Assignments (400-499), Maintenance Reports (500-599), Maintenance Jobs (600-699)
    if (typeValue >= 400 && typeValue < 700) return true;

    // Generic system notifications (1000+)
    if (typeValue >= 1000) return true;

    // For now, include ALL notifications to debug what types we're getting
    console.log('⚠️ DASHBOARD: Notification type', typeValue, 'not in expected ranges, but including anyway');
    return true;
  }

  // Machine status helpers
  getMachineStatusClass(): string {
    if (!this.assignedMachine) return '';

    switch (this.assignedMachine.status) {
      case 'OPERATIONAL': return 'status-operational';
      case 'UNDER_MAINTENANCE': return 'status-under-maintenance';
      case 'DOWN_FOR_REPAIR': return 'status-down-for-repair';
      case 'OFFLINE': return 'status-offline';
      default: return '';
    }
  }

  getMachineStatusLabel(): string {
    if (!this.assignedMachine) return 'N/A';

    switch (this.assignedMachine.status) {
      case 'OPERATIONAL': return 'Operational';
      case 'UNDER_MAINTENANCE': return 'Under Maintenance';
      case 'DOWN_FOR_REPAIR': return 'Down for Repair';
      case 'OFFLINE': return 'Offline';
      default: return this.assignedMachine.status;
    }
  }

  getServiceProgress(): number {
    if (!this.machineUsageMetrics) return 0;
    const current = this.machineUsageMetrics.engineHours % this.serviceIntervalHours;
    return Math.round((current / this.serviceIntervalHours) * 100);
  }

  getServiceRemaining(): number {
    if (!this.machineUsageMetrics) return this.serviceIntervalHours;
    const current = this.machineUsageMetrics.engineHours % this.serviceIntervalHours;
    return Math.max(0, this.serviceIntervalHours - current);
  }

  getServiceUrgencyClass(): string {
    const progress = this.getServiceProgress();
    if (progress >= 90) return 'urgent';
    if (progress >= 75) return 'warning';
    return 'normal';
  }

  // Maintenance report helpers
  getActiveReportsCount(): number {
    const count = this.maintenanceReports.filter(r => {
      const status = typeof r.status === 'string' ? r.status.toUpperCase() : r.status;
      const isActive = status === 'REPORTED' || status === 'ACKNOWLEDGED' || status === 'IN_PROGRESS' ||
                       status === 'SUBMITTED' || status === 'APPROVED' || status === 'PENDING';
      console.log(`📊 Report ${r.id} status: ${r.status} -> ${status}, isActive: ${isActive}`);
      return isActive;
    }).length;
    console.log('📊 Total active reports:', count);
    return count;
  }

  getCriticalReportsCount(): number {
    const count = this.maintenanceReports.filter(r => {
      const status = typeof r.status === 'string' ? r.status.toUpperCase() : r.status;
      const isActive = status === 'REPORTED' || status === 'ACKNOWLEDGED' || status === 'IN_PROGRESS' ||
                       status === 'SUBMITTED' || status === 'APPROVED' || status === 'PENDING';
      // SeverityLevel enum: CRITICAL = 'Critical', HIGH = 'High', MEDIUM = 'Medium', LOW = 'Low'
      const isCritical = r.severity === 'Critical' || r.severity === 'High';
      console.log(`🚨 Report ${r.id} severity: ${r.severity}, status: ${r.status}, isCritical: ${isCritical}, isActive: ${isActive}`);
      return isCritical && isActive;
    }).length;
    console.log('🚨 Total critical reports:', count);
    return count;
  }

  getCompletedReportsCount(): number {
    const count = this.maintenanceReports.filter(r => {
      const status = typeof r.status === 'string' ? r.status.toUpperCase() : r.status;
      const isCompleted = status === 'RESOLVED' || status === 'CLOSED' || status === 'COMPLETED';
      console.log(`✅ Report ${r.id} status: ${r.status} -> ${status}, isCompleted: ${isCompleted}`);
      return isCompleted;
    }).length;
    console.log('✅ Total completed reports:', count);
    return count;
  }

  // Utilization metrics
  getUtilizationRate(): number {
    if (!this.machineUsageStatistics || this.machineUsageStatistics.totalEngineHours === 0) return 0;
    return Math.round((this.machineUsageStatistics.totalWorkingHours / this.machineUsageStatistics.totalEngineHours) * 100);
  }

  // Navigation helpers
  navigateToMachines(): void {
    this.router.navigate(['/operator/my-machines']);
  }

  navigateToMaintenance(): void {
    this.router.navigate(['/operator/maintenance-reports']);
  }

  navigateToNotifications(): void {
    this.router.navigate(['/operator/notifications']);
  }

  navigateToMachineUsage(): void {
    if (this.assignedMachine?.id) {
      this.router.navigate(['/operator/machine-usage', this.assignedMachine.id]);
    }
  }

  markNotificationAsRead(notification: Notification): void {
    this.notificationService.markAsRead(notification.id).subscribe({
      next: () => {
        this.loadNotifications();
      },
      error: (err) => {
        console.error('Failed to mark notification as read:', err);
      }
    });
  }

  getTimeAgo(date: Date | string): string {
    const now = new Date();
    const then = new Date(date);
    const diffMs = now.getTime() - then.getTime();
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMins / 60);
    const diffDays = Math.floor(diffHours / 24);

    if (diffDays > 0) return `${diffDays}d ago`;
    if (diffHours > 0) return `${diffHours}h ago`;
    if (diffMins > 0) return `${diffMins}m ago`;
    return 'Just now';
  }

  getNotificationIcon(notification: Notification): string {
    // You can expand this based on notification types
    if (!notification.isRead) return 'notifications_active';
    return 'notifications';
  }
}