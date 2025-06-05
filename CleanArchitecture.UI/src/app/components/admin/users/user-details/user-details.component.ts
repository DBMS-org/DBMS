import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../../core/services/user.service';
import { ProjectService } from '../../../../core/services/project.service';
import { User } from '../../../../core/models/user.model';
import { Project } from '../../../../core/models/project.model';
import { forkJoin, Subscription } from 'rxjs';

interface UserActivity {
  id: number;
  action: string;
  description: string;
  timestamp: Date;
  icon: string;
  type: 'login' | 'update' | 'create' | 'delete' | 'view' | 'export' | 'assignment';
}

@Component({
  selector: 'app-user-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.scss']
})
export class UserDetailsComponent implements OnInit, OnDestroy {
  user: User | null = null;
  activities: UserActivity[] = [];
  loading = false;
  loadingActivities = false;
  error: string | null = null;
  private subscription: Subscription | undefined;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private projectService: ProjectService
  ) {}

  ngOnInit() {
    // Get user ID from route parameters
    this.route.params.subscribe(params => {
      const userId = +params['id'];
      if (userId) {
        // Force refresh data every time component loads
        this.loadUser(userId);
      }
    });
  }

  ngOnDestroy() {
    this.subscription?.unsubscribe();
  }

  loadUser(userId: number) {
    this.loading = true;
    this.error = null;

    // Add timestamp to avoid caching
    const timestamp = new Date().getTime();
    const userObservable = this.userService.getUser(userId);

    userObservable.subscribe({
      next: (user) => {
        this.user = user;
        this.loading = false;
        this.loadUserActivitiesFromRealData(userId);
      },
      error: (error) => {
        this.error = error.message;
        this.loading = false;
        console.error('Error loading user:', error);
        
        // Fallback to mock data if API fails
        this.user = {
          id: userId,
          name: 'John Doe',
          email: 'john@example.com',
          role: 'Admin',
          region: 'Muscat',
          country: 'Oman',
          omanPhone: '+968 9876 5432',
          countryPhone: '+1 234 567 8900',
          status: 'Active',
          createdAt: new Date(),
          updatedAt: new Date()
        };
        this.loadUserActivitiesFromRealData(userId);
      }
    });
  }

  loadUserActivitiesFromRealData(userId: number) {
    this.loadingActivities = true;
    
    // Load both user data and projects to build comprehensive activity timeline
    const subscription = forkJoin({
      user: this.userService.getUser(userId),
      allProjects: this.projectService.getProjects()
    }).subscribe({
      next: (data) => {
        this.activities = this.buildActivitiesFromRealData(data.user, data.allProjects);
        this.loadingActivities = false;
      },
      error: (error) => {
        console.error('Error loading activity data:', error);
        // Fall back to basic activities from user data only
        if (this.user) {
          this.activities = this.buildBasicActivitiesFromUser(this.user);
        }
        this.loadingActivities = false;
      }
    });

    this.subscription = subscription;
  }

  private buildActivitiesFromRealData(user: User, allProjects: Project[]): UserActivity[] {
    const activities: UserActivity[] = [];
    let activityId = 1;

    // Only show activities that are logically realistic and not from seeded data
    
    // 1. Last Login Activity (only if it's recent and realistic)
    if (user.lastLoginAt) {
      const loginDate = new Date(user.lastLoginAt);
      const now = new Date();
      const daysDiff = Math.floor((now.getTime() - loginDate.getTime()) / (1000 * 60 * 60 * 24));
      
      // Only show if login was within last 30 days (realistic)
      if (daysDiff <= 30) {
        activities.push({
          id: activityId++,
          action: 'Last Login',
          description: 'Signed in to the system',
          timestamp: loginDate,
          icon: 'login',
          type: 'login'
        });
      }
    }

    // 2. Profile Update Activity (only if it's a meaningful update)
    if (user.updatedAt && user.createdAt) {
      const created = new Date(user.createdAt);
      const updated = new Date(user.updatedAt);
      const updateDiff = updated.getTime() - created.getTime();
      
      // Only show if update was at least 1 hour after creation (realistic profile update)
      if (updateDiff > (60 * 60 * 1000)) {
        activities.push({
          id: activityId++,
          action: 'Profile Updated',
          description: 'Updated profile information',
          timestamp: updated,
          icon: 'edit',
          type: 'update'
        });
      }
    }

    // 3. Project Assignments (only show the most recent one to avoid clutter)
    const assignedProjects = allProjects.filter(project => project.assignedUserId === user.id);
    if (assignedProjects.length > 0) {
      // Sort by creation date and only show the most recent assignment
      const mostRecentProject = assignedProjects
        .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())[0];
      
      activities.push({
        id: activityId++,
        action: 'Project Assignment',
        description: `Assigned to project: ${mostRecentProject.name}`,
        timestamp: new Date(mostRecentProject.createdAt),
        icon: 'assignment',
        type: 'assignment'
      });
    }

    // 4. Account Creation Activity (always show this as it's fundamental)
    activities.push({
      id: activityId++,
      action: 'Account Created',
      description: `User account was created with role: ${user.role}`,
      timestamp: new Date(user.createdAt),
      icon: 'person_add',
      type: 'create'
    });

    // Sort activities by timestamp (most recent first)
    return activities.sort((a, b) => b.timestamp.getTime() - a.timestamp.getTime());
  }

  private buildBasicActivitiesFromUser(user: User): UserActivity[] {
    const activities: UserActivity[] = [];
    let activityId = 1;

    // Only show realistic activities based on actual user data
    
    // 1. Last Login (only if recent and realistic)
    if (user.lastLoginAt) {
      const loginDate = new Date(user.lastLoginAt);
      const now = new Date();
      const daysDiff = Math.floor((now.getTime() - loginDate.getTime()) / (1000 * 60 * 60 * 24));
      
      // Only show if login was within last 30 days
      if (daysDiff <= 30) {
        activities.push({
          id: activityId++,
          action: 'Last Login',
          description: 'Signed in to the system',
          timestamp: loginDate,
          icon: 'login',
          type: 'login'
        });
      }
    }

    // 2. Profile Update (only if it's a meaningful update)
    if (user.updatedAt && user.createdAt) {
      const created = new Date(user.createdAt);
      const updated = new Date(user.updatedAt);
      const updateDiff = updated.getTime() - created.getTime();
      
      // Only show if update was at least 1 hour after creation
      if (updateDiff > (60 * 60 * 1000)) {
        activities.push({
          id: activityId++,
          action: 'Profile Updated',
          description: 'Updated profile information',
          timestamp: updated,
          icon: 'edit',
          type: 'update'
        });
      }
    }

    // 3. Account Creation (always show this)
    activities.push({
      id: activityId++,
      action: 'Account Created',
      description: `User account was created with role: ${user.role}`,
      timestamp: new Date(user.createdAt),
      icon: 'person_add',
      type: 'create'
    });

    // Sort by most recent first
    return activities.sort((a, b) => b.timestamp.getTime() - a.timestamp.getTime());
  }

  getFormattedTime(timestamp: Date): string {
    const now = new Date();
    // Use UTC time for calculations to avoid timezone issues
    const nowUtc = now.getTime();
    const timestampUtc = timestamp.getTime();
    const diffInMinutes = Math.floor((nowUtc - timestampUtc) / (1000 * 60));
    
    if (diffInMinutes < 60) {
      return `${diffInMinutes} minutes ago`;
    } else if (diffInMinutes < 1440) { // 24 hours
      const hours = Math.floor(diffInMinutes / 60);
      return `${hours} ${hours === 1 ? 'hour' : 'hours'} ago`;
    } else if (diffInMinutes < 10080) { // 7 days
      const days = Math.floor(diffInMinutes / 1440);
      return `${days} ${days === 1 ? 'day' : 'days'} ago`;
    } else if (diffInMinutes < 43200) { // 30 days
      const weeks = Math.floor(diffInMinutes / 10080);
      return `${weeks} ${weeks === 1 ? 'week' : 'weeks'} ago`;
    } else {
      const months = Math.floor(diffInMinutes / 43200);
      return `${months} ${months === 1 ? 'month' : 'months'} ago`;
    }
  }

  getActivityTypeClass(type: string): string {
    const classMap: { [key: string]: string } = {
      'login': 'activity-login',
      'update': 'activity-update',
      'create': 'activity-create',
      'delete': 'activity-delete',
      'view': 'activity-view',
      'export': 'activity-export',
      'assignment': 'activity-assignment'
    };
    return classMap[type] || 'activity-default';
  }

  getUserJoinedDate(): string {
    if (this.user?.createdAt) {
      return new Intl.DateTimeFormat('en-US', { 
        month: 'short', 
        year: 'numeric' 
      }).format(new Date(this.user.createdAt));
    }
    return 'Jan 2024'; // fallback
  }

  goBack() {
    this.router.navigate(['/admin/users']);
  }

  editUser() {
    if (this.user) {
      this.router.navigate(['/admin/users', this.user.id, 'edit']);
    }
  }

  deleteUser() {
    if (this.user && confirm('Are you sure you want to delete this user?')) {
      this.userService.deleteUser(this.user.id).subscribe({
        next: () => {
          console.log('User deleted successfully');
          this.router.navigate(['/admin/users']);
        },
        error: (error) => {
          this.error = error.message;
          console.error('Error deleting user:', error);
        }
      });
    }
  }
}
