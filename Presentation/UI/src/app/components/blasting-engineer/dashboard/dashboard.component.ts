import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription, forkJoin } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { ProjectService } from '../../../core/services/project.service';
import { SiteService, ProjectSite } from '../../../core/services/site.service';
import { UserActivityService } from '../../../core/services/user-activity.service';
import { UnifiedDrillDataService } from '../../../core/services/unified-drill-data.service';
import { DrillHoleService, DrillHole } from '../../../core/services/drill-hole.service';
import { DrillLocation } from '../../../core/models/drilling.model';
import { User } from '../../../core/models/user.model';
import { Project } from '../../../core/models/project.model';

// Dashboard component for blasting engineer overview and statistics
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

  // Dashboard statistics for projects, sites, and drill holes
  stats = {
    totalProjects: 0,
    activeProjects: 0,
    totalSites: 0,
    activeSites: 0,
    totalDrillHoles: 0,
    activeDrillSites: 0,
    uploadedDataSets: 0,
    completedPatterns: 0,
    pendingReviews: 0
  };

  recentActivities: any[] = [];
  userProjects: Project[] = [];
  drillData: DrillHole[] = [];
  drillLocations: DrillLocation[] = [];
  recentUploads: any[] = [];

  // Quick statistics for drill hole data
  quickStats = {
    averageDepth: 0,
    maxElevation: 0,
    minElevation: 0,
    totalDrillLength: 0
  };
  isLoading = true;

  constructor(
    private authService: AuthService,
    private projectService: ProjectService,
    private siteService: SiteService,
    private userActivityService: UserActivityService,
    private unifiedDrillDataService: UnifiedDrillDataService,
    private drillHoleService: DrillHoleService,
    private router: Router
  ) {}

  ngOnInit() {
    this.subscribeToCurrentUser();
    this.loadDrillData();
  }

  ngOnDestroy() {
    this.userSubscription.unsubscribe();
  }

  // Subscribe to current user authentication state
  private subscribeToCurrentUser() {
    this.userSubscription = this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
      if (user) {
        console.log('Current logged-in blasting engineer:', user);
        this.loadUserSpecificData();
      }
    });
  }

  private loadUserSpecificData() {
    this.loadDashboardData();
  }

  // Load all dashboard data from database
  private loadDashboardData() {
    forkJoin({
      projects: this.projectService.getProjectsForCurrentUser(),
      allSites: this.siteService.getAllSites(),
      allDrillHoles: this.drillHoleService.getAllDrillHoles()
    }).subscribe({
      next: ({ projects, allSites, allDrillHoles }) => {
        this.userProjects = projects;

        // Filter sites and drill holes for current user's projects
        const userProjectIds = projects.map(p => p.id);
        const userSites = allSites.filter(site => userProjectIds.includes(site.projectId));
        const userSiteIds = userSites.map(s => s.id);
        const userDrillHoles = allDrillHoles.filter(hole =>
          hole.projectId && hole.siteId &&
          userProjectIds.includes(hole.projectId) &&
          userSiteIds.includes(hole.siteId)
        );

        // Calculate sites that have drill hole data
        const sitesWithDrillHoles = new Set(userDrillHoles.map(h => h.siteId));

        // Update all statistics with real data
        this.stats.totalProjects = projects.length;
        this.stats.activeProjects = projects.filter(p => p.status === 'Active').length;
        this.stats.totalSites = userSites.length;
        this.stats.activeSites = userSites.filter(s => s.status === 'Active').length;
        this.stats.totalDrillHoles = userDrillHoles.length;
        this.stats.activeDrillSites = sitesWithDrillHoles.size;
        this.stats.uploadedDataSets = sitesWithDrillHoles.size;
        this.stats.completedPatterns = sitesWithDrillHoles.size;
        this.stats.pendingReviews = Math.max(0, this.stats.activeSites - sitesWithDrillHoles.size);

        this.drillData = userDrillHoles;
        this.calculateQuickStats();
        this.loadRecentUploadsFromDatabase(userDrillHoles);
        this.generateSiteActivities(userSites);
        this.loadRecentActivities();
        this.isLoading = false;

        console.log('📊 Dashboard Statistics:', {
          totalProjects: this.stats.totalProjects,
          activeProjects: this.stats.activeProjects,
          totalSites: this.stats.totalSites,
          activeSites: this.stats.activeSites,
          totalDrillHoles: this.stats.totalDrillHoles,
          activeDrillSites: this.stats.activeDrillSites,
          userDrillHoles: userDrillHoles.length
        });
      },
      error: (error) => {
        console.error('Error loading dashboard data:', error);
        // Use fallback mock data if database load fails
        this.stats = {
          totalProjects: 12,
          activeProjects: 8,
          totalSites: 8,
          activeSites: 6,
          totalDrillHoles: 0,
          activeDrillSites: 0,
          uploadedDataSets: this.drillData.length > 0 ? 5 : 0,
          completedPatterns: 15,
          pendingReviews: 3
        };
        this.loadRecentActivities();
        this.isLoading = false;
      }
    });
  }

  // Load drill data from unified service for immediate display
  private loadDrillData() {
    this.drillLocations = this.unifiedDrillDataService.getDrillLocations();

    // Convert drill locations to drill holes format
    this.drillData = this.drillLocations.map(loc => ({
      id: loc.id,
      easting: loc.easting || loc.x,
      northing: loc.northing || loc.y,
      elevation: loc.elevation || 0,
      depth: loc.depth || 0,
      length: loc.length || loc.depth || 0,
      azimuth: loc.azimuth || 0,
      dip: loc.dip || 0,
      actualDepth: loc.actualDepth || loc.depth || 0,
      stemming: loc.stemming || 0,
      projectId: loc.projectId,
      siteId: loc.siteId,
      createdAt: (loc.createdAt instanceof Date ? loc.createdAt : new Date(loc.createdAt)).toISOString(),
      updatedAt: (loc.updatedAt instanceof Date ? loc.updatedAt : new Date(loc.updatedAt)).toISOString()
    } as DrillHole));
    this.calculateQuickStats();
    this.loadRecentUploads();
  }

  // Calculate statistics for drill hole data
  private calculateQuickStats() {
    if (this.drillData.length === 0) {
      this.quickStats = {
        averageDepth: 0,
        maxElevation: 0,
        minElevation: 0,
        totalDrillLength: 0
      };
      return;
    }

    const depths = this.drillData.map(hole => hole.depth);
    const elevations = this.drillData.map(hole => hole.elevation);
    const lengths = this.drillData.map(hole => hole.length);

    this.quickStats = {
      averageDepth: Math.round(depths.reduce((a, b) => a + b, 0) / depths.length * 100) / 100,
      maxElevation: Math.max(...elevations),
      minElevation: Math.min(...elevations),
      totalDrillLength: Math.round(lengths.reduce((a, b) => a + b, 0) * 100) / 100
    };
  }

  // Load sample recent uploads for display
  private loadRecentUploads() {
    this.recentUploads = [
      {
        filename: 'drill_data_batch_1.csv',
        uploadDate: new Date(Date.now() - 2 * 60 * 60 * 1000),
        recordCount: 150,
        status: 'processed'
      },
      {
        filename: 'mining_site_alpha.csv',
        uploadDate: new Date(Date.now() - 24 * 60 * 60 * 1000),
        recordCount: 89,
        status: 'processed'
      },
      {
        filename: 'quarry_beta_holes.csv',
        uploadDate: new Date(Date.now() - 3 * 24 * 60 * 60 * 1000),
        recordCount: 234,
        status: 'processed'
      }
    ];
  }

  // Load recent uploads from database drill holes
  private loadRecentUploadsFromDatabase(drillHoles: DrillHole[]) {
    const siteGroups = drillHoles.reduce((groups: any, hole) => {
      const siteKey = `${hole.projectId}-${hole.siteId}`;
      if (!groups[siteKey]) {
        groups[siteKey] = {
          siteId: hole.siteId,
          projectId: hole.projectId,
          holes: [],
          latestUpdate: hole.createdAt || hole.updatedAt
        };
      }
      groups[siteKey].holes.push(hole);

      // Track latest update timestamp
      const holeDate = hole.createdAt || hole.updatedAt;
      if (holeDate && (!groups[siteKey].latestUpdate || holeDate > groups[siteKey].latestUpdate)) {
        groups[siteKey].latestUpdate = holeDate;
      }

      return groups;
    }, {});

    // Convert to upload format and sort by date
    this.recentUploads = Object.values(siteGroups)
      .map((group: any) => ({
        filename: `Project_${group.projectId}_Site_${group.siteId}_drill_data.csv`,
        uploadDate: new Date(group.latestUpdate || Date.now()),
        recordCount: group.holes.length,
        status: 'processed',
        projectId: group.projectId,
        siteId: group.siteId
      }))
      .sort((a: any, b: any) => new Date(b.uploadDate).getTime() - new Date(a.uploadDate).getTime())
      .slice(0, 5);
  }

  // Load and filter recent user activities
  private loadRecentActivities() {
    const allActivities = this.userActivityService.getActivitiesPrioritizedByUserRegion();

    // Filter out navigation activities, keep only important ones
    this.recentActivities = allActivities.filter(activity => {
      const action = activity.action.toLowerCase();

      if (action.includes('dashboard access') ||
          action.includes('accessed dashboard') ||
          action.includes('navigated to')) {
        return false;
      }

      return action.includes('created') ||
             action.includes('uploaded') ||
             action.includes('completed') ||
             action.includes('updated') ||
             action.includes('logged out') ||
             action.includes('assigned') ||
             action.includes('reviewed') ||
             action.includes('exported') ||
             action.includes('generated');
    });
  }

  // Handle user logout
  logout() {
    this.userActivityService.trackActivity('logged out', 'User session ended', 'auth');
    this.authService.logoutWithConfirmation();
  }

  // Generate personalized welcome message
  getUserWelcomeMessage(): string {
    if (!this.currentUser) return 'Welcome, Blasting Engineer';

    const timeOfDay = this.getTimeOfDayGreeting();
    return `${timeOfDay}, ${this.currentUser.name}`;
  }

  // Get user initials for avatar display
  getInitials(): string {
    if (!this.currentUser?.name) return 'BE';

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
    return 'Last login: Today at 8:45 AM';
  }

  trackActivity(index: number, activity: any): number {
    return activity.id;
  }

  // Get appropriate icon for activity type
  getActivityIcon(action: string): string {
    if (action.includes('upload')) return 'cloud_upload';
    if (action.includes('design') || action.includes('pattern')) return 'grid_on';
    if (action.includes('review')) return 'assessment';
    if (action.includes('export') || action.includes('report')) return 'description';
    if (action.includes('created') && action.includes('site')) return 'place';
    if (action.includes('created')) return 'add_circle';
    if (action.includes('visualization') || action.includes('3D')) return '3d_rotation';
    if (action.includes('dashboard') || action.includes('accessed')) return 'dashboard';
    return 'work';
  }

  // Navigation methods
  navigateToUpload() {
    this.router.navigate(['/blasting-engineer/csv-upload']);
  }

  navigateToProjects() {
    this.router.navigate(['/blasting-engineer/project-management']);
  }

  navigateToSites() {
    this.router.navigate(['/blasting-engineer/project-management']);
  }

  // Determine data quality based on drill hole count
  getDataQualityStatus(): string {
    if (this.drillData.length === 0) return 'No Data';
    if (this.drillData.length < 50) return 'Limited';
    if (this.drillData.length < 200) return 'Good';
    return 'Excellent';
  }

  getDataQualityClass(): string {
    const status = this.getDataQualityStatus();
    return `quality-${status.toLowerCase().replace(' ', '-')}`;
  }

  // Format date as relative time
  formatDate(date: Date | string): string {
    const now = new Date();
    const dateObj = date instanceof Date ? date : new Date(date);

    if (isNaN(dateObj.getTime())) {
      return 'Invalid date';
    }

    const diffInHours = (now.getTime() - dateObj.getTime()) / (1000 * 60 * 60);

    if (diffInHours < 1) return 'Just now';
    if (diffInHours < 24) return `${Math.floor(diffInHours)} hours ago`;
    if (diffInHours < 48) return 'Yesterday';
    return `${Math.floor(diffInHours / 24)} days ago`;
  }

  getTimeFromNow(timestamp: Date | string): string {
    return this.formatDate(timestamp);
  }

  // Generate activities from recent site creations
  private generateSiteActivities(sites: ProjectSite[]) {
    const recentSites = sites
      .filter(site => {
        const siteDate = new Date(site.createdAt);
        const weekAgo = new Date(Date.now() - 7 * 24 * 60 * 60 * 1000);
        return siteDate > weekAgo;
      })
      .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
      .slice(0, 3);

    recentSites.forEach(site => {
      this.userActivityService.trackActivity(
        `created site "${site.name}"`,
        `New drilling site created at ${site.location}`,
        'project'
      );
    });
  }

  // Navigate to drill visualization for specific site or general view
  navigateToDrillVisualization(projectId?: number, siteId?: number): void {
    if (projectId && siteId) {
      this.router.navigate(['/blasting-engineer/project-management', projectId, 'sites', siteId, 'drill-visualization']);
    } else {
      this.router.navigate(['/blasting-engineer/drill-visualization']);
    }
  }

  // Refresh all dashboard data
  refreshDashboard(): void {
    this.isLoading = true;
    this.loadUserSpecificData();
  }

  // Navigate to pattern designer
  navigateToPatternDesigner(): void {
    this.router.navigate(['/blasting-engineer/drilling-pattern']);
  }
}
