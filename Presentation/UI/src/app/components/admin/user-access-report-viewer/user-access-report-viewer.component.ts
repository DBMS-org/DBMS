import { Component, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ReportService } from '../../../services/report.service';

// PrimeNG Imports
import { TableModule } from 'primeng/table';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { MessageModule } from 'primeng/message';
import { ChartModule } from 'primeng/chart';
import { TabViewModule } from 'primeng/tabview';
import { ChipModule } from 'primeng/chip';
import { DividerModule } from 'primeng/divider';
import { AvatarModule } from 'primeng/avatar';

@Component({
  selector: 'app-user-access-report-viewer',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    CardModule,
    ButtonModule,
    TagModule,
    ProgressSpinnerModule,
    MessageModule,
    ChartModule,
    TabViewModule,
    ChipModule,
    DividerModule,
    AvatarModule
  ],
  templateUrl: './user-access-report-viewer.component.html',
  styleUrls: ['./user-access-report-viewer.component.scss']
})
export class UserAccessReportViewerComponent implements OnInit {
  report: any = null;
  loading = false;
  error: string | null = null;

  // Chart data
  roleDistributionChart: any;
  permissionCoverageChart: any;
  userStatusChart: any;
  activityStatusChart: any;

  constructor(
    private reportService: ReportService,
    private route: ActivatedRoute,
    private location: Location
  ) {}

  ngOnInit() {
    this.loadReport();
  }

  loadReport() {
    this.loading = true;
    this.error = null;

    // Get query params for filtering
    this.route.queryParams.subscribe(params => {
      const filter = {
        startDate: params['startDate'] ? new Date(params['startDate']) : undefined,
        endDate: params['endDate'] ? new Date(params['endDate']) : undefined,
        regionId: params['regionId'] || undefined
      };

      this.reportService.getUserAccessReport(filter).subscribe({
        next: (response) => {
          this.loading = false;
          if (response.data) {
            this.report = response.data;
          } else {
            this.report = response;
          }
          this.prepareChartData();
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Failed to load user access report. Please try again.';
          console.error('Error loading report:', err);
        }
      });
    });
  }

  prepareChartData() {
    if (!this.report) return;

    // Role Distribution Pie Chart
    const roleLabels = this.report.roleDistribution.map((r: any) => r.roleName);
    const roleValues = this.report.roleDistribution.map((r: any) => r.userCount);
    const roleColors = this.generateColors(roleLabels.length);

    this.roleDistributionChart = {
      labels: roleLabels,
      datasets: [{
        data: roleValues,
        backgroundColor: roleColors,
        hoverBackgroundColor: roleColors.map((c: string) => this.adjustBrightness(c, -20))
      }]
    };

    // Permission Coverage Bar Chart
    const permissionLabels = this.report.permissionMatrix
      .slice(0, 10) // Top 10 permissions
      .map((p: any) => p.permissionName);
    const permissionUserCounts = this.report.permissionMatrix
      .slice(0, 10)
      .map((p: any) => p.userCount);

    this.permissionCoverageChart = {
      labels: permissionLabels,
      datasets: [{
        label: 'Users with Permission',
        data: permissionUserCounts,
        backgroundColor: '#42A5F5',
        borderColor: '#1E88E5',
        borderWidth: 1
      }]
    };

    // User Status Doughnut Chart
    this.userStatusChart = {
      labels: ['Active Users', 'Inactive Users'],
      datasets: [{
        data: [
          this.report.statistics.activeUsers,
          this.report.statistics.inactiveUsers
        ],
        backgroundColor: ['#66BB6A', '#EF5350'],
        hoverBackgroundColor: ['#4CAF50', '#E53935']
      }]
    };

    // Activity Status Chart
    const activityStatusMap: { [key: string]: number } = {};
    this.report.userActivity.forEach((u: any) => {
      activityStatusMap[u.activityStatus] = (activityStatusMap[u.activityStatus] || 0) + 1;
    });

    this.activityStatusChart = {
      labels: Object.keys(activityStatusMap),
      datasets: [{
        data: Object.values(activityStatusMap),
        backgroundColor: ['#66BB6A', '#42A5F5', '#FFA726', '#EF5350', '#AB47BC'],
        hoverBackgroundColor: ['#4CAF50', '#1E88E5', '#FB8C00', '#E53935', '#8E24AA']
      }]
    };
  }

  generateColors(count: number): string[] {
    const baseColors = [
      '#42A5F5', '#66BB6A', '#FFA726', '#AB47BC', '#26A69A',
      '#EC407A', '#5C6BC0', '#FFCA28', '#EF5350', '#8D6E63'
    ];
    const colors = [];
    for (let i = 0; i < count; i++) {
      colors.push(baseColors[i % baseColors.length]);
    }
    return colors;
  }

  adjustBrightness(color: string, amount: number): string {
    const num = parseInt(color.replace('#', ''), 16);
    const r = Math.min(255, Math.max(0, (num >> 16) + amount));
    const g = Math.min(255, Math.max(0, ((num >> 8) & 0x00FF) + amount));
    const b = Math.min(255, Math.max(0, (num & 0x0000FF) + amount));
    return `#${((r << 16) | (g << 8) | b).toString(16).padStart(6, '0')}`;
  }

  getUserStatusSeverity(isActive: boolean): string {
    return isActive ? 'success' : 'danger';
  }

  getActivityStatusSeverity(status: string): string {
    const severityMap: { [key: string]: string } = {
      'Active': 'success',
      'Recently Active': 'info',
      'New': 'warning',
      'Dormant': 'warning',
      'Inactive': 'danger'
    };
    return severityMap[status] || 'secondary';
  }

  getRoleChipColor(index: number): string {
    const colors = ['primary', 'success', 'info', 'warning', 'danger', 'secondary'];
    return colors[index % colors.length];
  }

  goBack() {
    this.location.back();
  }

  printReport() {
    window.print();
  }

  exportToPdf() {
    console.log('Export to PDF functionality - To be implemented');
    // TODO: Implement PDF export
  }

  exportToExcel() {
    console.log('Export to Excel functionality - To be implemented');
    // TODO: Implement Excel export
  }
}
