import { Component, OnInit, signal, inject, ChangeDetectionStrategy, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaintenanceMockService } from '../services/maintenance-mock.service';
import { MaintenanceStats, MaintenanceAlert, AlertType, Priority } from '../models/maintenance.models';
import { MaintenanceError } from '../services/maintenance-error-handler.service';
import { MaintenanceAlertsComponent } from './maintenance-alerts/maintenance-alerts.component';
import { MaintenanceQuickActionsComponent } from './maintenance-quick-actions/maintenance-quick-actions.component';
import { LoadingSpinnerComponent } from '../shared/loading-spinner/loading-spinner.component';
import { SkeletonLoaderComponent } from '../shared/skeleton-loader/skeleton-loader.component';
import { OfflineIndicatorComponent } from '../shared/offline-indicator/offline-indicator.component';

@Component({
  selector: 'app-maintenance-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MaintenanceAlertsComponent,
    MaintenanceQuickActionsComponent,
    LoadingSpinnerComponent,
    SkeletonLoaderComponent,
    OfflineIndicatorComponent
  ],
  templateUrl: './maintenance-dashboard.component.html',
  styleUrl: './maintenance-dashboard.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MaintenanceDashboardComponent implements OnInit {
  private maintenanceService = inject(MaintenanceMockService);

  // Loading and error states
  isLoading = signal(true);
  error = signal<MaintenanceError | null>(null);
  
  // Core data signals
  maintenanceStats = signal<MaintenanceStats | null>(null);
  serviceDueAlerts = signal<MaintenanceAlert[]>([]);
  overdueAlerts = signal<MaintenanceAlert[]>([]);

  // Computed values for enhanced stats display
  totalAlerts = computed(() => 
    this.serviceDueAlerts().length + this.overdueAlerts().length
  );

  criticalAlertsCount = computed(() => 
    [...this.serviceDueAlerts(), ...this.overdueAlerts()]
      .filter(alert => alert.priority === Priority.HIGH).length
  );

  hasAlerts = computed(() => this.totalAlerts() > 0);

  ngOnInit() {
    this.loadDashboardData();
  }

  async loadDashboardData() {
    this.isLoading.set(true);
    this.error.set(null);

    try {
      // For now, we'll use mock data since the backend isn't implemented yet
      // In a real implementation, these would be actual service calls:
      // const [stats, serviceDue, overdue] = await Promise.all([
      //   this.maintenanceService.getMaintenanceStats().toPromise(),
      //   this.maintenanceService.getServiceDueAlerts().toPromise(),
      //   this.maintenanceService.getOverdueAlerts().toPromise()
      // ]);

      const mockStats: MaintenanceStats = {
        totalMachines: 42,
        scheduledJobs: 15,
        inProgressJobs: 8,
        completedJobs: 23,
        overdueJobs: 3,
        serviceDueAlerts: 6
      };

      const mockServiceDueAlerts: MaintenanceAlert[] = [
        {
          id: '1',
          machineId: 'EX-001',
          machineName: 'Excavator EX-001',
          alertType: AlertType.SERVICE_DUE,
          message: 'Routine maintenance due soon - 250 engine hours reached',
          dueDate: new Date(Date.now() + 5 * 24 * 60 * 60 * 1000), // 5 days from now
          priority: Priority.MEDIUM,
          daysUntilDue: 5
        },
        {
          id: '2',
          machineId: 'DR-205',
          machineName: 'Drill Rig DR-205',
          alertType: AlertType.SERVICE_DUE,
          message: 'Hydraulic system service required',
          dueDate: new Date(Date.now() + 2 * 24 * 60 * 60 * 1000), // 2 days from now
          priority: Priority.HIGH,
          daysUntilDue: 2
        },
        {
          id: '4',
          machineId: 'BH-301',
          machineName: 'Bulldozer BH-301',
          alertType: AlertType.SERVICE_DUE,
          message: 'Track tension adjustment needed',
          dueDate: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000), // 7 days from now
          priority: Priority.LOW,
          daysUntilDue: 7
        }
      ];

      const mockOverdueAlerts: MaintenanceAlert[] = [
        {
          id: '3',
          machineId: 'LD-103',
          machineName: 'Loader LD-103',
          alertType: AlertType.OVERDUE,
          message: 'Oil change overdue - immediate attention required',
          dueDate: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000), // 2 days ago
          priority: Priority.HIGH,
          daysPastDue: 2
        },
        {
          id: '5',
          machineId: 'CR-150',
          machineName: 'Crane CR-150',
          alertType: AlertType.OVERDUE,
          message: 'Safety inspection overdue',
          dueDate: new Date(Date.now() - 5 * 24 * 60 * 60 * 1000), // 5 days ago
          priority: Priority.HIGH,
          daysPastDue: 5
        }
      ];

      this.maintenanceStats.set(mockStats);
      this.serviceDueAlerts.set(mockServiceDueAlerts);
      this.overdueAlerts.set(mockOverdueAlerts);

    } catch (err) {
      console.error('Error loading dashboard data:', err);
      this.error.set(err as MaintenanceError);
    } finally {
      this.isLoading.set(false);
    }
  }

  refreshDashboard(): void {
    this.loadDashboardData();
  }
}