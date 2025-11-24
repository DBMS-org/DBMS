import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportService, ReportFilter } from '../../../services/report.service';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';

@Component({
  selector: 'app-maintenance-report-viewer',
  standalone: true,
  imports: [CommonModule, TableModule, TabViewModule, ButtonModule, TagModule],
  templateUrl: './maintenance-report-viewer.component.html',
  styleUrls: ['./maintenance-report-viewer.component.scss']
})
export class MaintenanceReportViewerComponent implements OnInit {
  reportData: any = null;
  loading = false;
  error: string | null = null;
  activeTab: string = 'allMaintenanceJobs';

  // Filter parameters
  startDate: string | null = null;
  endDate: string | null = null;
  regionId: string | null = null;

  constructor(
    private reportService: ReportService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    // Get query parameters
    this.route.queryParams.subscribe(params => {
      this.startDate = params['startDate'] || null;
      this.endDate = params['endDate'] || null;
      this.regionId = params['regionId'] || null;

      this.loadReport();
    });
  }

  loadReport() {
    this.loading = true;
    this.error = null;

    const filter: ReportFilter = {
      startDate: this.startDate ? new Date(this.startDate) : undefined,
      endDate: this.endDate ? new Date(this.endDate) : undefined,
      regionId: this.regionId || undefined
    };

    this.reportService.getMaintenancePerformanceReport(filter).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.data) {
          this.reportData = response.data;
        } else {
          this.reportData = response;
        }
      },
      error: (err) => {
        this.loading = false;
        this.error = 'Failed to load maintenance performance report. Please try again.';
        console.error('Error loading report:', err);
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
    alert('PDF export functionality will be implemented soon');
  }

  exportToExcel() {
    alert('Excel export functionality will be implemented soon');
  }

  getPercentageWidth(percentage: number): string {
    return `${Math.min(percentage, 100)}%`;
  }

  getStatusColor(status: string): string {
    const statusMap: { [key: string]: string } = {
      'Completed': '#22c55e',
      'InProgress': '#3b82f6',
      'Scheduled': '#f59e0b',
      'Overdue': '#ef4444',
      'Cancelled': '#6b7280'
    };
    return statusMap[status] || '#6b7280';
  }

  getTypeColor(type: string): string {
    const typeMap: { [key: string]: string } = {
      'Emergency': '#ef4444',
      'Corrective': '#f59e0b',
      'Preventive': '#3b82f6',
      'Inspection': '#8b5cf6'
    };
    return typeMap[type] || '#6b7280';
  }

  getPriorityColor(priority: string): string {
    const priorityMap: { [key: string]: string } = {
      'Critical': '#dc2626',
      'High': '#f59e0b',
      'Medium': '#3b82f6',
      'Low': '#22c55e'
    };
    return priorityMap[priority] || '#6b7280';
  }
}
