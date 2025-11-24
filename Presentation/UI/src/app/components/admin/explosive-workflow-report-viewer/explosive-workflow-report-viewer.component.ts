import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ReportService } from '../../../services/report.service';
import { TableModule } from 'primeng/table';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { ChartModule } from 'primeng/chart';
import { TabViewModule } from 'primeng/tabview';
import { TagModule } from 'primeng/tag';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { MessageModule } from 'primeng/message';

@Component({
  selector: 'app-explosive-workflow-report-viewer',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    CardModule,
    ButtonModule,
    ChartModule,
    TabViewModule,
    TagModule,
    ProgressSpinnerModule,
    MessageModule
  ],
  templateUrl: './explosive-workflow-report-viewer.component.html',
  styleUrls: ['./explosive-workflow-report-viewer.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ExplosiveWorkflowReportViewerComponent implements OnInit {
  report: any = null;
  loading = false;
  error: string | null = null;

  // Chart data
  approvalStatusChart: any;
  transferStatusChart: any;
  turnaroundChart: any;
  requesterChart: any;

  constructor(
    private reportService: ReportService,
    private route: ActivatedRoute,
    private location: Location
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.loadReport({
        startDate: params['startDate'] ? new Date(params['startDate']) : undefined,
        endDate: params['endDate'] ? new Date(params['endDate']) : undefined,
        regionId: params['regionId']
      });
    });
  }

  loadReport(filter?: any) {
    this.loading = true;
    this.error = null;

    this.reportService.getExplosiveWorkflowReport(filter).subscribe({
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
        this.error = 'Failed to load explosive workflow report. Please try again.';
        console.error('Error loading report:', err);
      }
    });
  }

  prepareChartData() {
    if (!this.report) return;

    // Approval Status Chart
    const approvalAnalysis = this.report.approvalAnalysis;
    if (approvalAnalysis?.byStatus) {
      this.approvalStatusChart = {
        labels: Object.keys(approvalAnalysis.byStatus),
        datasets: [{
          label: 'Approval Requests by Status',
          data: Object.values(approvalAnalysis.byStatus),
          backgroundColor: [
            '#FFA726', // Pending
            '#66BB6A', // Approved
            '#EF5350', // Rejected
            '#9E9E9E', // Cancelled
            '#AB47BC'  // Expired
          ]
        }]
      };
    }

    // Transfer Status Chart
    const transferAnalysis = this.report.transferAnalysis;
    if (transferAnalysis?.byStatus) {
      this.transferStatusChart = {
        labels: Object.keys(transferAnalysis.byStatus),
        datasets: [{
          label: 'Transfer Requests by Status',
          data: Object.values(transferAnalysis.byStatus),
          backgroundColor: [
            '#FFA726', // Pending
            '#66BB6A', // Approved
            '#EF5350', // Rejected
            '#42A5F5', // InProgress
            '#26A69A', // Completed
            '#9E9E9E'  // Cancelled
          ]
        }]
      };
    }

    // Turnaround Time Comparison
    const turnaround = this.report.turnaroundAnalysis;
    this.turnaroundChart = {
      labels: ['Approval Requests', 'Transfer Requests'],
      datasets: [{
        label: 'Average Turnaround (Days)',
        data: [
          turnaround?.averageApprovalTurnaroundDays || 0,
          turnaround?.averageTransferTurnaroundDays || 0
        ],
        backgroundColor: ['#42A5F5', '#66BB6A']
      }]
    };

    // Top Requesters Chart
    const topRequesters = this.report.requesterStatistics?.slice(0, 5) || [];
    this.requesterChart = {
      labels: topRequesters.map((r: any) => r.userName),
      datasets: [{
        label: 'Total Requests',
        data: topRequesters.map((r: any) => r.totalRequests),
        backgroundColor: '#5C6BC0'
      }]
    };
  }

  getStatusSeverity(status: string): string {
    const statusLower = status?.toLowerCase() || '';
    if (statusLower.includes('pending')) return 'warning';
    if (statusLower.includes('approved') || statusLower.includes('completed')) return 'success';
    if (statusLower.includes('rejected')) return 'danger';
    if (statusLower.includes('progress')) return 'info';
    return 'secondary';
  }

  getPrioritySeverity(priority: string): string {
    const priorityLower = priority?.toLowerCase() || '';
    if (priorityLower === 'critical') return 'danger';
    if (priorityLower === 'high') return 'warning';
    if (priorityLower === 'normal') return 'info';
    return 'secondary';
  }

  goBack() {
    this.location.back();
  }

  printReport() {
    window.print();
  }

  exportToPdf() {
    console.log('PDF export not implemented yet');
    // Placeholder for future PDF export functionality
  }

  exportToExcel() {
    console.log('Excel export not implemented yet');
    // Placeholder for future Excel export functionality
  }
}
