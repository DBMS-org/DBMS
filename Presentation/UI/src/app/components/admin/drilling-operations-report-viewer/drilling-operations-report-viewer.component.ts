import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportService, ReportFilter } from '../../../services/report.service';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { CardModule } from 'primeng/card';
import { ChartModule } from 'primeng/chart';

@Component({
  selector: 'app-drilling-operations-report-viewer',
  standalone: true,
  imports: [CommonModule, TableModule, TabViewModule, ButtonModule, TagModule, CardModule, ChartModule],
  templateUrl: './drilling-operations-report-viewer.component.html',
  styleUrls: ['./drilling-operations-report-viewer.component.scss']
})
export class DrillingOperationsReportViewerComponent implements OnInit {
  reportData: any = null;
  loading = false;
  error: string | null = null;
  activeTab: string = 'projectSites';

  // Filter parameters
  startDate: string | null = null;
  endDate: string | null = null;
  regionId: string | null = null;

  // Chart data
  projectSiteChartData: any = null;
  regionalChartData: any = null;

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

    this.reportService.getDrillingOperationsReport(filter).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.data) {
          this.reportData = response.data;
          this.prepareChartData();
        } else {
          this.reportData = response;
          this.prepareChartData();
        }
      },
      error: (err) => {
        this.loading = false;
        this.error = 'Failed to load drilling operations report. Please try again.';
        console.error('Error loading report:', err);
      }
    });
  }

  prepareChartData() {
    if (!this.reportData) return;

    // Project Site Chart
    if (this.reportData.drillingByProjectSite && this.reportData.drillingByProjectSite.length > 0) {
      this.projectSiteChartData = {
        labels: this.reportData.drillingByProjectSite.map((site: any) => site.projectSiteName),
        datasets: [
          {
            label: 'Drill Holes',
            data: this.reportData.drillingByProjectSite.map((site: any) => site.drillHoleCount),
            backgroundColor: '#42A5F5',
            borderColor: '#1E88E5',
            borderWidth: 1
          },
          {
            label: 'Drill Points',
            data: this.reportData.drillingByProjectSite.map((site: any) => site.drillPointCount),
            backgroundColor: '#66BB6A',
            borderColor: '#43A047',
            borderWidth: 1
          }
        ]
      };
    }

    // Regional Chart
    if (this.reportData.drillingByRegion && this.reportData.drillingByRegion.length > 0) {
      this.regionalChartData = {
        labels: this.reportData.drillingByRegion.map((region: any) => region.regionName),
        datasets: [
          {
            label: 'Total Depth (m)',
            data: this.reportData.drillingByRegion.map((region: any) => region.totalDepth),
            backgroundColor: '#FFA726',
            borderColor: '#FB8C00',
            borderWidth: 1
          }
        ]
      };
    }
  }

  goBack() {
    this.router.navigate(['/admin/executive-dashboard']);
  }

  printReport() {
    window.print();
  }

  exportToPdf() {
    const filter: ReportFilter = {
      startDate: this.startDate ? new Date(this.startDate) : undefined,
      endDate: this.endDate ? new Date(this.endDate) : undefined,
      regionId: this.regionId || undefined
    };

    this.reportService.exportReportToPdf('drilling-operations', filter).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `drilling-operations-report-${new Date().toISOString().split('T')[0]}.pdf`;
        link.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        alert('PDF export is not yet implemented');
        console.error('Error exporting to PDF:', err);
      }
    });
  }

  exportToExcel() {
    const filter: ReportFilter = {
      startDate: this.startDate ? new Date(this.startDate) : undefined,
      endDate: this.endDate ? new Date(this.endDate) : undefined,
      regionId: this.regionId || undefined
    };

    this.reportService.exportReportToExcel('drilling-operations', filter).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `drilling-operations-report-${new Date().toISOString().split('T')[0]}.xlsx`;
        link.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        alert('Excel export is not yet implemented');
        console.error('Error exporting to Excel:', err);
      }
    });
  }

  refreshReport() {
    this.loadReport();
  }

  getStatusSeverity(status: string): string {
    switch (status?.toLowerCase()) {
      case 'active':
      case 'completed':
        return 'success';
      case 'pending':
      case 'inprogress':
        return 'warning';
      case 'onhold':
        return 'info';
      default:
        return 'secondary';
    }
  }
}
