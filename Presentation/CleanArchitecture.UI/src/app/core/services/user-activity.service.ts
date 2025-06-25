import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { AuthService } from './auth.service';

export interface UserActivity {
  id: string;
  userId: number;
  userName: string;
  action: string;
  details?: string;
  timestamp: Date;
  region?: string;
  category: 'auth' | 'project' | 'upload' | 'visualization' | 'admin' | 'other';
}

@Injectable({
  providedIn: 'root'
})
export class UserActivityService {
  private activitiesSubject = new BehaviorSubject<UserActivity[]>([]);
  public activities$ = this.activitiesSubject.asObservable();

  constructor(private authService: AuthService) {}

  /**
   * Track a user activity
   */
  trackActivity(action: string, details?: string, category: UserActivity['category'] = 'other'): void {
    const currentUser = this.authService.getCurrentUser();
    if (!currentUser) return;

    const activity: UserActivity = {
      id: this.generateId(),
      userId: currentUser.id,
      userName: currentUser.name,
      action,
      details,
      timestamp: new Date(),
      region: currentUser.region,
      category
    };

    const currentActivities = this.activitiesSubject.value;
    const updatedActivities = [activity, ...currentActivities].slice(0, 100); // Keep last 100 activities
    this.activitiesSubject.next(updatedActivities);

    // In a real application, you would also send this to a backend API
    console.log('User activity tracked:', activity);
  }

  /**
   * Get activities for the current user
   */
  getCurrentUserActivities(): Observable<UserActivity[]> {
    const currentUser = this.authService.getCurrentUser();
    if (!currentUser) return new BehaviorSubject<UserActivity[]>([]).asObservable();

    return new BehaviorSubject(
      this.activitiesSubject.value.filter(activity => activity.userId === currentUser.id)
    ).asObservable();
  }

  /**
   * Get activities by region
   */
  getActivitiesByRegion(region: string): UserActivity[] {
    return this.activitiesSubject.value.filter(activity => activity.region === region);
  }

  /**
   * Get recent activities (for dashboard display)
   */
  getRecentActivities(limit: number = 10): UserActivity[] {
    return this.activitiesSubject.value
      .sort((a, b) => b.timestamp.getTime() - a.timestamp.getTime())
      .slice(0, limit);
  }

  /**
   * Get activities filtered by user region preference
   */
  getActivitiesPrioritizedByUserRegion(): UserActivity[] {
    const currentUser = this.authService.getCurrentUser();
    if (!currentUser?.region) {
      return this.getRecentActivities();
    }

    const allActivities = this.activitiesSubject.value;
    const userRegionActivities = allActivities.filter(activity => activity.region === currentUser.region);
    const otherActivities = allActivities.filter(activity => activity.region !== currentUser.region);

    return [...userRegionActivities, ...otherActivities].slice(0, 10);
  }

  /**
   * Clear all activities
   */
  clearActivities(): void {
    this.activitiesSubject.next([]);
  }

  /**
   * Generate a unique ID for activities
   */
  private generateId(): string {
    return Date.now().toString(36) + Math.random().toString(36).substr(2);
  }

  /**
   * Initialize with empty activities (will be populated by real API data)
   */
  initializeSampleActivities(): void {
    this.activitiesSubject.next([]);
  }
} 