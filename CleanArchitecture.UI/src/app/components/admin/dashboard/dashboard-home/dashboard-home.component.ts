import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription, forkJoin } from 'rxjs';
import { AuthService } from '../../../../core/services/auth.service';
import { UserService } from '../../../../core/services/user.service';
import { ProjectService } from '../../../../core/services/project.service';
import { SiteService } from '../../../../core/services/site.service';
import { DrillHoleService, DrillHole } from '../../../../core/services/drill-hole.service';
import { User } from '../../../../core/models/user.model';

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
    totalProjects: 0,
    activeProjects: 0,
    totalSites: 0,
    activeSites: 0,
    totalDrillHoles: 0,
    activeDrillSites: 0,
    totalEngineers: 0,
    totalOperators: 0
  };

  recentActivities: any[] = [];
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
    // Load general dashboard statistics
    this.loadStats();
    this.loadRecentActivities();
  }

  private loadUserSpecificData() {
    if (!this.currentUser) return;

    // Customize activities based on user region/country
    this.recentActivities = this.filterActivitiesByUserRegion();
  }

  private loadStats() {
    // Load comprehensive admin statistics from database
    forkJoin({
      users: this.userService.getUsers(),
      projects: this.projectService.getProjects(),
      sites: this.siteService.getAllSites(),
      drillHoles: this.drillHoleService.getAllDrillHoles()
    }).subscribe({
      next: ({ users, projects, sites, drillHoles }) => {
        // User statistics
        this.stats.totalUsers = users.length;
        this.stats.activeUsers = users.filter((u: any) => u.status === 'Active').length;
        this.stats.totalEngineers = users.filter((u: any) => u.role === 'BlastingEngineer').length;
        this.stats.totalOperators = users.filter((u: any) => u.role === 'Operator').length;
        
        // Project statistics
        this.stats.totalProjects = projects.length;
        this.stats.activeProjects = projects.filter((p: any) => p.status === 'Active').length;
        
        // Site statistics
        this.stats.totalSites = sites.length;
        this.stats.activeSites = sites.filter((s: any) => s.status === 'Active').length;
        
        // Drill hole statistics
        this.stats.totalDrillHoles = drillHoles.length;
        const sitesWithDrillHoles = new Set(drillHoles.map((h: any) => h.siteId).filter((id: any) => id));
        this.stats.activeDrillSites = sitesWithDrillHoles.size;
        
        // Calculate drill analytics
        this.calculateDrillAnalytics(drillHoles);
        
        // Update system metrics
        this.systemMetrics.totalDataUploads = sitesWithDrillHoles.size;
        this.systemMetrics.averageDrillDepth = this.quickStats.averageDepth;
        this.systemMetrics.dataQuality = drillHoles.length > 100 ? 'Excellent' : drillHoles.length > 50 ? 'Good' : 'Limited';
        
        this.isLoading = false;
        
        console.log('📊 Admin Dashboard Statistics:', {
          totalUsers: this.stats.totalUsers,
          activeUsers: this.stats.activeUsers,
          totalEngineers: this.stats.totalEngineers,
          totalOperators: this.stats.totalOperators,
          totalProjects: this.stats.totalProjects,
          activeProjects: this.stats.activeProjects,
          totalSites: this.stats.totalSites,
          activeSites: this.stats.activeSites,
          totalDrillHoles: this.stats.totalDrillHoles,
          activeDrillSites: this.stats.activeDrillSites
        });
      },
      error: (error) => {
        console.error('Error loading admin dashboard data:', error);
        // Set default values on error
        this.stats = {
          totalUsers: 150,
          activeUsers: 120,
          totalProjects: 25,
          activeProjects: 18,
          totalSites: 0,
          activeSites: 0,
          totalDrillHoles: 0,
          activeDrillSites: 0,
          totalEngineers: 0,
          totalOperators: 0
        };
        this.isLoading = false;
      }
    });
  }

  private loadRecentActivities() {
    // Sample activities - in a real app, you'd fetch from API
    const allActivities = [
      { id: 1, user: 'John Doe', action: 'created a new project', time: '5 minutes ago', region: 'Muscat' },
      { id: 2, user: 'Jane Smith', action: 'updated user profile', time: '1 hour ago', region: 'Salalah' },
      { id: 3, user: 'Bob Johnson', action: 'completed drill plan review', time: '2 hours ago', region: 'Muscat' },
      { id: 4, user: 'Alice Brown', action: 'uploaded CSV data', time: '3 hours ago', region: 'Sohar' },
      { id: 5, user: 'Mike Wilson', action: 'generated blast report', time: '4 hours ago', region: 'Muscat' }
    ];

    this.recentActivities = allActivities;
  }

  private filterActivitiesByUserRegion(): any[] {
    if (!this.currentUser?.region) {
      return this.recentActivities;
    }

    // Show activities from user's region first, then others
    const userRegionActivities = this.recentActivities.filter(activity => 
      activity.region === this.currentUser?.region
    );
    const otherActivities = this.recentActivities.filter(activity => 
      activity.region !== this.currentUser?.region
    );

    return [...userRegionActivities, ...otherActivities];
  }

  logout() {
    this.authService.logout();
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
    
    // In a real app, you'd get this from the user's last login data
    return 'Last login: Today at 9:30 AM';
  }

  trackActivity(index: number, activity: any): number {
    return activity.id;
  }

  getActivityIcon(action: string): string {
    if (action.includes('project')) return 'work';
    if (action.includes('profile') || action.includes('user')) return 'person';
    if (action.includes('upload')) return 'cloud_upload';
    if (action.includes('review') || action.includes('report')) return 'assessment';
    if (action.includes('completed')) return 'check_circle';
    return 'notification_important';
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

  // Method to refresh dashboard data manually
  refreshDashboard(): void {
    this.isLoading = true;
    this.loadDashboardData();
  }

  // Navigation methods for admin actions
  navigateToUsers(): void {
    this.router.navigate(['/admin/users']);
  }

  navigateToProjects(): void {
    this.router.navigate(['/admin/project-management']);
  }
}
