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

  private subscriptions = new Subscription();

  // Dashboard statistics
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

  // Recent activities
  recentActivities: any[] = [];

  // System metrics
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
    private unifiedDrillDataService: UnifiedDrillDataService
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

    // Get the project assigned to this operator
    const projectSub = this.projectService.getProjectByOperator(this.currentUser.id).subscribe({
      next: (project) => {
        this.assignedProject = project;
        if (project) {
          this.loadProjectDetails(project.id);
        } else {
          console.log('No project assigned to this operator');
          this.isLoading = false;
          // Show helpful message when no project is assigned
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
  }

  private loadProjectDetails(projectId: number): void {
    // Get all sites for this project
    const sitesSub = this.siteService.getProjectSites(projectId).subscribe({
      next: (sites) => {
        this.projectSites = sites.map(s => ({
          ...s,
          createdAt: new Date(s.createdAt),
          updatedAt: new Date(s.updatedAt)
        }));

        // Load drill holes for all sites
        this.loadDrillHolesForSites(projectId, sites);
        
        // Calculate dashboard statistics
        this.updateStatistics(sites, []);
        
        // Load site-specific drilling data
        this.loadSiteSpecificData();
        
        // Populate recent activity feed
        this.updateRecentActivities();
        
        // Update system status metrics
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

  private loadDrillHolesForSites(projectId: number, sites: ProjectSite[]): void {
    let totalDrillHoles = 0;
    
    sites.forEach(site => {
      const drillHolesSub = this.drillHoleService.getDrillHolesBySite(projectId, site.id).subscribe({
        next: (drillHoles) => {
          totalDrillHoles += drillHoles.length;
          this.stats.totalDrillHoles = totalDrillHoles;
        },
        error: (err) => {
          console.warn('Failed to load drill holes for site', site.id, err);
        }
      });
      this.subscriptions.add(drillHolesSub);
    });
  }

  private loadSiteSpecificData(): void {
    if (!this.assignedProject || this.projectSites.length === 0) return;

    // Get drilling patterns and blast data for each site
    this.projectSites.forEach(site => {
      // Get drill points for pattern statistics
      const drillPointsSub = this.unifiedDrillDataService.getDrillPoints(this.assignedProject!.id, site.id).subscribe({
        next: (drillPoints) => {
          if (drillPoints.length > 0) {
            this.stats.sitesWithPatterns++;
          }
          
          // Note: BlastSequence entity was removed as it's not used in the application
          // Statistics for sitesWithSequences will be calculated differently if needed in the future
        },
        error: (err) => console.warn('Failed to load drill points for site', site.id, err)
      });
      this.subscriptions.add(drillPointsSub);
    });
  }

  private updateStatistics(sites: ProjectSite[], drillHoles: any[]): void {
    this.stats.totalSites = sites.length;
    this.stats.totalDrillHoles = drillHoles.length;
    this.stats.approvedPatterns = sites.filter(s => s.isPatternApproved).length;
    this.stats.confirmedSimulations = sites.filter(s => s.isSimulationConfirmed).length;
    
    // Calculate project progress
    if (sites.length > 0) {
      const completedSites = sites.filter(s => s.isPatternApproved && s.isSimulationConfirmed).length;
      this.stats.projectProgress = Math.round((completedSites / sites.length) * 100);
    }
    
    this.stats.activeWorkflows = sites.filter(s => !s.isPatternApproved || !s.isSimulationConfirmed).length;
  }

  private updateRecentActivities(): void {
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

    // Add site-related activities
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

    // Limit to 5 most recent activities
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

  getActivityIcon(type: string): string {
    switch (type) {
      case 'project': return 'assignment';
      case 'pattern': return 'grid_on';
      case 'sequence': return 'timeline';
      case 'drill': return 'construction';
      default: return 'info';
    }
  }

  getActivityStatusColor(status: string): string {
    switch (status) {
      case 'active': return 'status-active';
      case 'approved': return 'status-approved';
      case 'pending': return 'status-pending';
      case 'completed': return 'status-completed';
      default: return '';
    }
  }

  // Navigation methods
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

  refreshDashboard(): void {
    this.loadOperatorData();
  }

  trackActivity(index: number, activity: any): number {
    return activity.id;
  }
}