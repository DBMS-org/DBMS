import { Component, OnInit, signal, inject, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MaintenanceMockService } from './services/maintenance-mock.service';
import { MaintenanceErrorHandlerService, MaintenanceError } from './services/maintenance-error-handler.service';

@Component({
  selector: 'app-maintenance',
  standalone: true,
  imports: [
    CommonModule, 
    RouterModule
  ],
  templateUrl: './maintenance.component.html',
  styleUrl: './maintenance.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MaintenanceComponent implements OnInit {
  private maintenanceService = inject(MaintenanceMockService);

  protected errorHandler = inject(MaintenanceErrorHandlerService);

  isLoading = signal(true);
  error = signal<MaintenanceError | null>(null);
  retryCount = signal(0);

  ngOnInit() {
    this.loadMaintenanceData();
  }

  private async loadMaintenanceData() {
    this.isLoading.set(true);
    this.error.set(null);

    try {
      // Initialize maintenance data - this will trigger loading of dashboard data
      // The actual data loading will be handled by child components
      
      this.isLoading.set(false);
    } catch (err) {
      console.error('Error loading maintenance data:', err);
      this.error.set(err as MaintenanceError);
      this.isLoading.set(false);
    }
  }

  retryLoadData() {
    const currentRetryCount = this.retryCount();
    if (currentRetryCount < 3) { // Limit retry attempts
      this.retryCount.set(currentRetryCount + 1);
      this.loadMaintenanceData();
    }
  }

  // Navigation controls removed; this component is nested inside the
  // mechanical engineer layout and uses its global navigation.
}