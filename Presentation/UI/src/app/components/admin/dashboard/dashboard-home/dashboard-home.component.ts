import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription, forkJoin } from 'rxjs';
import { AuthService } from '../../../../core/services/auth.service';
import { UserService } from '../../../../core/services/user.service';
import { ProjectService } from '../../../../core/services/project.service';
import { SiteService } from '../../../../core/services/site.service';
import { DrillHoleService, DrillHole } from '../../../../core/services/drill-hole.service';
import { MachineService } from '../../../../core/services/machine.service';
import { User } from '../../../../core/models/user.model';
import { Machine, MachineStatus } from '../../../../core/models/machine.model';

@Component({
  selector: 'app-dashboard-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.scss']
})
export class DashboardHomeComponent implements OnInit, OnDestroy {
  currentUser: User | null = null;
  private userSubscription: Subscription = new Subscription();
  
  stats = {
    totalUsers: 0,
    activeUsers: 0,
    deactivatedUsers: 0,
    totalProjects: 0,
    activeProjects: 0,
    pendingProjects: 0,
    completedProjects: 0,
    archivedProjects: 0,
    totalMachines: 0,
    assignedMachines: 0,
    pendingAssignments: 0,
    machineRequests: 0,
    totalSites: 0,
    activeSites: 0,
    totalDrillHoles: 0,
    activeDrillSites: 0
  };

  systemMetrics = {
    databaseStatus: 'Connected',
    totalDataUploads: 0,
    averageDrillDepth: 0,
    dataQuality: 'Good'
  };
  quickStats = {
    averageDepth: 0,
    maxElevation: 0,
    minElevation: 0,
    totalDrillLength: 0
  };
  isLoading = true;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private projectService: ProjectService,
    private siteService: SiteService,
    private drillHoleService: DrillHoleService,
    private machineService: MachineService,
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
      if (user) {
        console.log('Current logged-in user:', user);
        this.loadUserSpecificData();
      }
    });
  }

  private loadDashboardData() {
    this.loadStats();
  }

  private loadUserSpecificData() {
    if (!this.currentUser) return;

    console.log('Loading user-specific data for:', this.currentUser.name);
  }

  private loadStats() {
    forkJoin({
      users: this.userService.getUsers(),
      projects: this.projectService.getProjects(),
      sites: this.siteService.getAllSites(),
      drillHoles: this.drillHoleService.getAllDrillHoles(),
      machines: this.machineService.getAllMachines(),
      machineStats: this.machineService.getMachineStatistics(),
      assignmentRequests: this.machineService.getAllAssignmentRequests()
    }).subscribe({
      next: ({ users, projects, sites, drillHoles, machines, machineStats, assignmentRequests }) => {
        this.stats.totalUsers = users.length;
        this.stats.activeUsers = users.filter((u: any) => u.status === 'Active').length;
        this.stats.deactivatedUsers = users.filter((u: any) => u.status === 'Inactive' || u.status === 'Deactivated').length;

        this.stats.totalProjects = projects.length;
        this.stats.activeProjects = projects.filter((p: any) => p.status === 'Active').length;
        this.stats.pendingProjects = projects.filter((p: any) => p.status === 'Pending').length;
        this.stats.completedProjects = projects.filter((p: any) => p.status === 'Completed').length;
        this.stats.archivedProjects = projects.filter((p: any) => p.status === 'Archived').length;

        this.stats.totalSites = sites.length;
        this.stats.activeSites = sites.filter((s: any) => s.status === 'Active').length;

        this.stats.totalDrillHoles = drillHoles.length;
        const sitesWithDrillHoles = new Set(drillHoles.map((h: any) => h.siteId).filter((id: any) => id));
        this.stats.activeDrillSites = sitesWithDrillHoles.size;

        this.stats.totalMachines = machineStats.totalMachines || machines.length || 0;
        this.stats.assignedMachines = machineStats.assignedMachines || machines.filter((m: Machine) => m.status === MachineStatus.ASSIGNED).length || 0;
        this.stats.pendingAssignments = assignmentRequests.filter((r: any) => r.status === 'Pending').length || 0;
        this.stats.machineRequests = assignmentRequests.length || 0;

        this.calculateDrillAnalytics(drillHoles);

        this.systemMetrics.totalDataUploads = sitesWithDrillHoles.size;
        this.systemMetrics.averageDrillDepth = this.quickStats.averageDepth;
        this.systemMetrics.dataQuality = drillHoles.length > 100 ? 'Excellent' : drillHoles.length > 50 ? 'Good' : 'Limited';
        
        this.isLoading = false;
        
        console.log('📊 Admin Dashboard Statistics (Database):', {
          totalUsers: this.stats.totalUsers,
          activeUsers: this.stats.activeUsers,
          deactivatedUsers: this.stats.deactivatedUsers,
          totalProjects: this.stats.totalProjects,
          activeProjects: this.stats.activeProjects,
          pendingProjects: this.stats.pendingProjects,
          completedProjects: this.stats.completedProjects,
          archivedProjects: this.stats.archivedProjects,
          totalSites: this.stats.totalSites,
          activeSites: this.stats.activeSites,
          totalDrillHoles: this.stats.totalDrillHoles,
          activeDrillSites: this.stats.activeDrillSites,
          totalMachines: this.stats.totalMachines,
          assignedMachines: this.stats.assignedMachines,
          pendingAssignments: this.stats.pendingAssignments,
          machineRequests: this.stats.machineRequests
        });
      },
      error: (error) => {
        console.error('Error loading admin dashboard data:', error);
        this.stats = {
          totalUsers: 0,
          activeUsers: 0,
          deactivatedUsers: 0,
          totalProjects: 0,
          activeProjects: 0,
          pendingProjects: 0,
          completedProjects: 0,
          archivedProjects: 0,
          totalMachines: 0,
          assignedMachines: 0,
          pendingAssignments: 0,
          machineRequests: 0,
          totalSites: 0,
          activeSites: 0,
          totalDrillHoles: 0,
          activeDrillSites: 0
        };
        this.isLoading = false;
      }
    });
  }

  logout() {
    this.authService.logoutWithConfirmation();
  }

  getUserWelcomeMessage(): string {
    if (!this.currentUser) return 'Welcome, Admin';
    
    const timeOfDay = this.getTimeOfDayGreeting();
    return `${timeOfDay}, ${this.currentUser.name}`;
  }

  private getTimeOfDayGreeting(): string {
    const hour = new Date().getHours();
    if (hour < 12) return 'Good morning';
    if (hour < 18) return 'Good afternoon';
    return 'Good evening';
  }

  getUserLocationInfo(): string {
    if (!this.currentUser) return '';
    
    const parts = [];
    if (this.currentUser.region) parts.push(this.currentUser.region);
    if (this.currentUser.country) parts.push(this.currentUser.country);
    
    return parts.length > 0 ? `Location: ${parts.join(', ')}` : '';
  }

  getLastLoginInfo(): string {
    if (!this.currentUser) return '';

    return 'Last login: Today at 9:30 AM';
  }

  getInitials(): string {
    if (!this.currentUser?.name) return 'GM';
    
    const names = this.currentUser.name.split(' ');
    if (names.length >= 2) {
      return (names[0][0] + names[1][0]).toUpperCase();
    }
    return names[0].substring(0, 2).toUpperCase();
  }

  private calculateDrillAnalytics(drillHoles: DrillHole[]): void {
    if (drillHoles.length === 0) {
      this.quickStats = {
        averageDepth: 0,
        maxElevation: 0,
        minElevation: 0,
        totalDrillLength: 0
      };
      return;
    }

    const depths = drillHoles.map((hole: any) => hole.depth || 0);
    const elevations = drillHoles.map((hole: any) => hole.elevation || 0);
    const lengths = drillHoles.map((hole: any) => hole.length || 0);

    this.quickStats = {
      averageDepth: Math.round(depths.reduce((a, b) => a + b, 0) / depths.length * 100) / 100,
      maxElevation: Math.max(...elevations),
      minElevation: Math.min(...elevations),
      totalDrillLength: Math.round(lengths.reduce((a, b) => a + b, 0) * 100) / 100
    };
  }

  refreshDashboard(): void {
    this.isLoading = true;
    this.loadDashboardData();
  }

  navigateToUsers(): void {
    this.router.navigate(['/admin/users']);
  }

  navigateToProjects(): void {
    this.router.navigate(['/admin/project-management']);
  }

  navigateToMachineInventory(): void {
    this.router.navigate(['/admin/machine-inventory']);
  }

  navigateToMachineAssignments(): void {
    this.router.navigate(['/admin/machine-assignments']);
  }

  navigateToCreateUser(): void {
    this.router.navigate(['/admin/users/add-user']);
  }

  navigateToCreateProject(): void {
    this.router.navigate(['/admin/project-management/add-project']);
  }

  submitMachineAssignmentRequest(): void {
    console.log('Opening machine assignment request form...');
    this.router.navigate(['/admin/machine-assignments'], { queryParams: { action: 'request' } });
  }
}
