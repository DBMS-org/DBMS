import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { UserActivityService } from '../../../core/services/user-activity.service';
import { DrillDataService } from '../csv-upload/csv-upload.component';
import { User } from '../../../core/models/user.model';

interface DrillHole {
  serialNumber: number;
  id: string;
  name?: string;
  easting: number;
  northing: number;
  elevation: number;
  length: number;
  depth: number;
  azimuth: number;
  dip: number;
  actualDepth: number;
  stemming: number;
  createdAt?: string;
  updatedAt?: string;
}

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
  
  stats = {
    totalProjects: 0,
    activeProjects: 0,
    uploadedDataSets: 0,
    completedPatterns: 0,
    pendingReviews: 0
  };

  recentActivities: any[] = [];
  drillData: DrillHole[] = [];
  recentUploads: any[] = [];
  quickStats = {
    averageDepth: 0,
    maxElevation: 0,
    minElevation: 0,
    totalDrillLength: 0
  };
  isLoading = true;

  constructor(
    private authService: AuthService,
    private userActivityService: UserActivityService,
    private drillDataService: DrillDataService,
    private router: Router
  ) {}

  ngOnInit() {
    this.subscribeToCurrentUser();
    this.loadDashboardData();
    this.loadDrillData();
    this.userActivityService.initializeSampleActivities();
  }

  ngOnDestroy() {
    this.userSubscription.unsubscribe();
  }

  private subscribeToCurrentUser() {
    this.userSubscription = this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
      if (user) {
        console.log('Current logged-in blasting engineer:', user);
        this.loadUserSpecificData();
        this.trackDashboardAccess();
      }
    });
  }

  private loadDashboardData() {
    // Simulate loading data
    setTimeout(() => {
      this.stats = {
        totalProjects: 12,
        activeProjects: 8,
        uploadedDataSets: this.drillData.length > 0 ? 5 : 0,
        completedPatterns: 15,
        pendingReviews: 3
      };
      this.loadRecentActivities();
      this.isLoading = false;
    }, 1000);
  }

  private loadDrillData() {
    this.drillData = this.drillDataService.getDrillData();
    this.calculateQuickStats();
    this.loadRecentUploads();
  }

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

  private loadRecentUploads() {
    // Sample recent uploads data
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

  private loadUserSpecificData() {
    if (!this.currentUser) return;

    // Filter activities based on user region
    this.recentActivities = this.userActivityService.getActivitiesPrioritizedByUserRegion();
  }

  private loadRecentActivities() {
    // Get activities from the activity service
    this.recentActivities = this.userActivityService.getActivitiesPrioritizedByUserRegion();
  }

  private trackDashboardAccess() {
    this.userActivityService.trackActivity(
      'accessed dashboard',
      'Blasting Engineer Dashboard accessed',
      'other'
    );
  }

  logout() {
    this.userActivityService.trackActivity('logged out', 'User session ended', 'auth');
    this.authService.logout();
  }

  getUserWelcomeMessage(): string {
    if (!this.currentUser) return 'Welcome, Blasting Engineer';
    
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
    return 'Last login: Today at 8:45 AM';
  }

  trackActivity(index: number, activity: any): number {
    return activity.id;
  }

  getActivityIcon(action: string): string {
    if (action.includes('upload')) return 'cloud_upload';
    if (action.includes('design') || action.includes('pattern')) return 'grid_on';
    if (action.includes('review')) return 'assessment';
    if (action.includes('export') || action.includes('report')) return 'description';
    if (action.includes('created')) return 'add_circle';
    if (action.includes('visualization') || action.includes('3D')) return '3d_rotation';
    if (action.includes('dashboard') || action.includes('accessed')) return 'dashboard';
    return 'work';
  }

  navigateToUpload() {
    this.userActivityService.trackActivity('navigated to CSV upload', 'Accessed CSV upload page', 'upload');
    this.router.navigate(['/blasting-engineer/csv-upload']);
  }

  navigateToProjects() {
    this.userActivityService.trackActivity('navigated to project management', 'Accessed project management', 'project');
    this.router.navigate(['/blasting-engineer/project-management']);
  }

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

  formatDate(date: Date): string {
    const now = new Date();
    const diffInHours = (now.getTime() - date.getTime()) / (1000 * 60 * 60);
    
    if (diffInHours < 1) return 'Just now';
    if (diffInHours < 24) return `${Math.floor(diffInHours)} hours ago`;
    if (diffInHours < 48) return 'Yesterday';
    return `${Math.floor(diffInHours / 24)} days ago`;
  }

  getTimeFromNow(timestamp: Date): string {
    return this.formatDate(timestamp);
  }
}
