import { Component, OnInit, inject, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { MaintenanceMockService } from '../services/maintenance-mock.service';
import { ServiceComplianceData, MTBFMetrics, PartsUsageData, UsageMetrics } from '../models/maintenance.models';
import { ServiceComplianceChartComponent } from './service-compliance-chart/service-compliance-chart.component';
import { MTBFMetricsComponent } from './mtbf-metrics/mtbf-metrics.component';
import { PartsUsageSummaryComponent } from './parts-usage-summary/parts-usage-summary.component';
import { UsageMetricsChartComponent } from './usage-metrics-chart/usage-metrics-chart.component';

@Component({
  selector: 'app-maintenance-analytics',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatButtonModule,
    MatGridListModule,
    ServiceComplianceChartComponent,
    MTBFMetricsComponent,
    PartsUsageSummaryComponent,
    UsageMetricsChartComponent
  ],
  templateUrl: './maintenance-analytics.component.html',
  styleUrl: './maintenance-analytics.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MaintenanceAnalyticsComponent implements OnInit {
  private maintenanceService = inject(MaintenanceMockService);

  // Loading and error states
  isLoading = signal(false);
  hasError = signal(false);
  errorMessage = signal('');
  lastUpdated = signal(new Date());

  // Analytics data from the service
  serviceComplianceData = signal<ServiceComplianceData | null>(null);
  mtbfMetrics = signal<MTBFMetrics[] | null>(null);
  partsUsageData = signal<PartsUsageData[] | null>(null);
  usageMetrics = signal<UsageMetrics[] | null>(null);

  // Quick calculations for dashboard cards
  topPartsUsage = computed(() => {
    const data = this.partsUsageData();
    return data ? data.slice(0, 5) : [];
  });

  totalEngineHours = computed(() => {
    const metrics = this.usageMetrics();
    return metrics ? metrics.reduce((sum, m) => sum + m.engineHours, 0) : 0;
  });

  totalServiceHours = computed(() => {
    const metrics = this.usageMetrics();
    return metrics ? metrics.reduce((sum, m) => sum + m.serviceHours, 0) : 0;
  });

  totalIdleHours = computed(() => {
    const metrics = this.usageMetrics();
    return metrics ? metrics.reduce((sum, m) => sum + m.idleHours, 0) : 0;
  });

  // Controls which analytics section to show
  viewFilter = signal<'all' | 'compliance' | 'mtbf' | 'parts' | 'usage'>('compliance');

  setView(view: 'all' | 'compliance' | 'mtbf' | 'parts' | 'usage') {
    this.viewFilter.set(view);
  }

  ngOnInit() {
    this.loadAnalyticsData();
  }

  refreshData() {
    this.loadAnalyticsData();
  }

  private async loadAnalyticsData() {
    this.isLoading.set(true);
    this.hasError.set(false);
    this.errorMessage.set('');

    try {
      // Load all analytics data in parallel
      const [serviceCompliance, mtbf, partsUsage, usage] = await Promise.allSettled([
        this.maintenanceService.getServiceComplianceData().toPromise(),
        this.maintenanceService.getMTBFMetrics().toPromise(),
        this.maintenanceService.getPartsUsageData().toPromise(),
        this.maintenanceService.getUsageMetrics().toPromise()
      ]);

      // Handle service compliance data
      if (serviceCompliance.status === 'fulfilled' && serviceCompliance.value) {
        this.serviceComplianceData.set(serviceCompliance.value);
      }

      // Handle MTBF metrics
      if (mtbf.status === 'fulfilled' && mtbf.value) {
        this.mtbfMetrics.set(mtbf.value);
      }

      // Handle parts usage data
      if (partsUsage.status === 'fulfilled' && partsUsage.value) {
        this.partsUsageData.set(partsUsage.value);
      }

      // Handle usage metrics
      if (usage.status === 'fulfilled' && usage.value) {
        this.usageMetrics.set(usage.value);
      }

      // Check if all requests failed
      const allFailed = [serviceCompliance, mtbf, partsUsage, usage]
        .every(result => result.status === 'rejected');

      if (allFailed) {
        this.hasError.set(true);
        this.errorMessage.set('Failed to load analytics data. Please try again.');
      }

    } catch (error) {
      console.error('Error loading analytics data:', error);
      this.hasError.set(true);
      this.errorMessage.set('An unexpected error occurred while loading analytics data.');
    } finally {
      this.isLoading.set(false);
      this.lastUpdated.set(new Date());
    }
  }
}