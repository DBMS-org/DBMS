import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../../../core/services/auth.service';
import { UserService } from '../../../../core/services/user.service';
import { ProjectService } from '../../../../core/services/project.service';
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
    activeProjects: 0
  };

  recentActivities: any[] = [];
  isLoading = true;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private projectService: ProjectService,
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
    // Load users and projects statistics
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.stats.totalUsers = users.length;
        this.stats.activeUsers = users.filter(u => u.status === 'Active').length;
        
        // Load projects using role-based filtering
        this.projectService.getProjectsForCurrentUser().subscribe({
          next: (projects) => {
            this.stats.totalProjects = projects.length;
            this.stats.activeProjects = projects.filter(p => p.status === 'Active').length;
            this.isLoading = false;
          },
          error: (error) => {
            console.error('Error loading projects:', error);
            // Set default project values on error
            this.stats.totalProjects = 25;
            this.stats.activeProjects = 18;
            this.isLoading = false;
          }
        });
      },
      error: (error) => {
        console.error('Error loading users:', error);
        // Set default values on error
        this.stats = {
          totalUsers: 150,
          activeUsers: 120,
          totalProjects: 25,
          activeProjects: 18
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
}
