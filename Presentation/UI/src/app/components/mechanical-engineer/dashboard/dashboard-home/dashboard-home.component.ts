import { Component, OnInit, signal, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../../core/services/auth.service';
import { MaintenanceService } from '../../maintenance/services/maintenance.service';
import {
  MaintenanceStats,
  MaintenanceAlert as MaintAlert,
  MaintenanceJob,
  AlertType,
  MaintenanceStatus,
  Priority
} from '../../maintenance/models/maintenance.models';

@Component({
  selector: 'app-dashboard-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard-home.component.html',
  styleUrl: './dashboard-home.component.scss'
})
export class DashboardHomeComponent implements OnInit {
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);
  private authService = inject(AuthService);
  private router = inject(Router);
  private maintenanceService = inject(MaintenanceService);

  // Component state
  currentUser: any = null;
  isLoading = signal(true);
  error = signal<string | null>(null);

  // Dashboard data signals
  stats = signal<MaintenanceStats>({
    totalMachines: 0,
    scheduledJobs: 0,
    inProgressJobs: 0,
    completedJobs: 0,
    overdueJobs: 0,
    serviceDueAlerts: 0
  });

  maintenanceAlerts = signal<MaintAlert[]>([]);
  maintenanceJobs = signal<MaintenanceJob[]>([]);

  // Computed properties
  criticalAlerts = computed(() =>
    this.maintenanceAlerts().filter(alert => alert.priority === Priority.HIGH)
  );

  scheduledJobs = computed(() =>
    this.maintenanceJobs().filter(job => job.status === MaintenanceStatus.SCHEDULED)
  );

  inProgressJobs = computed(() =>
    this.maintenanceJobs().filter(job => job.status === MaintenanceStatus.IN_PROGRESS)
  );

  recentCompletedJobs = computed(() =>
    this.maintenanceJobs()
      .filter(job => job.status === MaintenanceStatus.COMPLETED)
      .sort((a, b) => {
        const dateA = a.completedDate?.getTime() || 0;
        const dateB = b.completedDate?.getTime() || 0;
        return dateB - dateA;
      })
      .slice(0, 5)
  );

  overdueJobs = computed(() =>
    this.maintenanceJobs().filter(job => job.status === MaintenanceStatus.OVERDUE)
  );

  // Job statistics by type
  preventiveJobs = computed(() =>
    this.maintenanceJobs().filter(job => job.type === 'Preventive')
  );

  correctiveJobs = computed(() =>
    this.maintenanceJobs().filter(job => job.type === 'Corrective')
  );

  // Overall statistics
  totalJobs = computed(() => this.maintenanceJobs().length);

  ngOnInit(): void {
    this.loadDashboardData();
  }

  private async loadDashboardData(): Promise<void> {
    try {
      this.isLoading.set(true);
      this.error.set(null);
      this.currentUser = this.authService.getCurrentUser();

      // Load maintenance stats
      this.maintenanceService.getMaintenanceStats().subscribe({
        next: (stats) => {
          this.stats.set(stats);
        },
        error: (error) => {
          console.error('Error loading maintenance stats:', error);
        }
      });

      // Load maintenance jobs
      this.maintenanceService.getMaintenanceJobs().subscribe({
        next: (jobs) => {
          this.maintenanceJobs.set(jobs);
        },
        error: (error) => {
          console.error('Error loading maintenance jobs:', error);
        }
      });

      // Load service due alerts
      this.maintenanceService.getServiceDueAlerts().subscribe({
        next: (alerts) => {
          this.maintenanceAlerts.set(alerts);
        },
        error: (error) => {
          console.error('Error loading service alerts:', error);
        }
      });

      // Load overdue alerts
      this.maintenanceService.getOverdueAlerts().subscribe({
        next: (overdueAlerts) => {
          // Combine with existing alerts
          const currentAlerts = this.maintenanceAlerts();
          const combined = Array.isArray(currentAlerts)
            ? [...currentAlerts, ...(Array.isArray(overdueAlerts) ? overdueAlerts : [])]
            : (Array.isArray(overdueAlerts) ? overdueAlerts : []);
          this.maintenanceAlerts.set(combined);
        },
        error: (error) => {
          console.error('Error loading overdue alerts:', error);
        }
      });

    } catch (error) {
      console.error('Error loading dashboard data:', error);
      this.error.set('Failed to load dashboard data');
    } finally {
      this.isLoading.set(false);
    }
  }


  refreshDashboard(): void {
    this.loadDashboardData();
  }

  getUserWelcomeMessage(): string {
    if (!this.currentUser) return 'Welcome, Mechanical Engineer';

    const hour = new Date().getHours();
    let greeting = 'Good morning';

    if (hour >= 12 && hour < 17) {
      greeting = 'Good afternoon';
    } else if (hour >= 17) {
      greeting = 'Good evening';
    }

    return `${greeting}, ${this.currentUser.name}!`;
  }

  getUserLocationInfo(): string {
    return this.currentUser?.region || 'Mining Site';
  }

  getLastLoginInfo(): string {
    return 'Last login: Today at 9:30 AM';
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }

  // Navigation methods
  navigateToMaintenanceJobs(): void {
    this.router.navigate(['/mechanical-engineer/maintenance/jobs']);
  }

  navigateToMaintenanceAnalytics(): void {
    this.router.navigate(['/mechanical-engineer/maintenance/analytics']);
  }

  viewJobDetails(job: MaintenanceJob): void {
    this.router.navigate(['/mechanical-engineer/maintenance/jobs']);
  }

  viewAlertDetails(alert: MaintAlert): void {
    this.router.navigate(['/mechanical-engineer/maintenance/jobs']);
  }

  // Helper methods
  getAlertIcon(alertType: AlertType): string {
    return alertType === AlertType.OVERDUE ? 'warning' : 'schedule';
  }

  getPriorityClass(priority: Priority): string {
    switch (priority) {
      case Priority.HIGH:
        return 'priority-high';
      case Priority.MEDIUM:
        return 'priority-medium';
      case Priority.LOW:
        return 'priority-low';
      default:
        return '';
    }
  }

  getStatusClass(status: MaintenanceStatus): string {
    switch (status) {
      case MaintenanceStatus.SCHEDULED:
        return 'status-scheduled';
      case MaintenanceStatus.IN_PROGRESS:
        return 'status-in-progress';
      case MaintenanceStatus.COMPLETED:
        return 'status-completed';
      case MaintenanceStatus.OVERDUE:
        return 'status-overdue';
      case MaintenanceStatus.CANCELLED:
        return 'status-cancelled';
      default:
        return '';
    }
  }

  formatDate(date: Date | undefined): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString();
  }

  getTimeAgo(date: Date): string {
    const now = new Date();
    const diffMs = now.getTime() - new Date(date).getTime();
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 60) return `${diffMins} minutes ago`;
    if (diffHours < 24) return `${diffHours} hours ago`;
    return `${diffDays} days ago`;
  }
}
