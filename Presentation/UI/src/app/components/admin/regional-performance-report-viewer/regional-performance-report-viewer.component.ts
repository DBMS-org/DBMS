import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportService, ReportFilter } from '../../../services/report.service';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';

@Component({
  selector: 'app-regional-performance-report-viewer',
  standalone: true,
  imports: [CommonModule, TableModule, TabViewModule, ButtonModule, TagModule],
  templateUrl: './regional-performance-report-viewer.component.html',
  styleUrls: ['./regional-performance-report-viewer.component.scss']
})
export class RegionalPerformanceReportViewerComponent implements OnInit {
  reportData: any = null;
  loading = false;
  error: string | null = null;

  constructor(
    private reportService: ReportService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.loadReport();
  }

  loadReport() {
    this.loading = true;
    this.error = null;

    // Get filter parameters from query params
    const queryParams = this.route.snapshot.queryParams;
    const filter: ReportFilter = {
      startDate: queryParams['startDate'] ? new Date(queryParams['startDate']) : undefined,
      endDate: queryParams['endDate'] ? new Date(queryParams['endDate']) : undefined,
      regionId: queryParams['regionId'] || undefined
    };

    this.reportService.getRegionalPerformanceReport(filter).subscribe({
      next: (response) => {
        // Handle both wrapped ApiResponse and direct object response
        if (response.data) {
          this.reportData = response.data;
        } else {
          this.reportData = response;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading regional performance report:', err);
        this.error = 'Failed to load regional performance report. Please try again.';
        this.loading = false;
      }
    });
  }

  goBack() {
    this.router.navigate(['/admin/executive-dashboard']);
  }

  printReport() {
    window.print();
  }

  exportToPdf() {
    // TODO: Implement PDF export
    alert('PDF export functionality will be implemented soon');
  }

  exportToExcel() {
    // TODO: Implement Excel export
    alert('Excel export functionality will be implemented soon');
  }

  getPerformanceColor(score: number): string {
    if (score >= 90) return '#22c55e'; // Green
    if (score >= 75) return '#3b82f6'; // Blue
    if (score >= 60) return '#f59e0b'; // Orange
    return '#ef4444'; // Red
  }

  getEfficiencyColor(rate: number): string {
    if (rate >= 85) return '#22c55e';
    if (rate >= 70) return '#3b82f6';
    if (rate >= 50) return '#f59e0b';
    return '#ef4444';
  }

  getUtilizationColor(percentage: number): string {
    if (percentage >= 80) return '#22c55e';
    if (percentage >= 60) return '#f59e0b';
    return '#ef4444';
  }

  getStatusColor(status: string): string {
    const statusColorMap: { [key: string]: string } = {
      'excellent': '#22c55e',
      'good': '#3b82f6',
      'fair': '#f59e0b',
      'poor': '#ef4444',
      'active': '#22c55e',
      'completed': '#3b82f6',
      'delayed': '#f59e0b',
      'critical': '#ef4444'
    };
    return statusColorMap[status.toLowerCase()] || '#6b7280';
  }
}
