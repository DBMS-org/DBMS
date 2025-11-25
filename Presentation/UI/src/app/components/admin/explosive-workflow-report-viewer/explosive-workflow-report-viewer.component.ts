import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { FormsModule } from '@angular/forms';
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
import pdfMake from 'pdfmake/build/pdfmake';
import pdfFonts from 'pdfmake/build/vfs_fonts';

@Component({
  selector: 'app-explosive-workflow-report-viewer',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
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
  originalReport: any = null;
  loading = false;
  error: string | null = null;
  isFilterExpanded: boolean = false;

  // Client-side filters
  filterStartDate: string = '';
  filterEndDate: string = '';
  filterStatuses: string[] = [];
  filterPriorities: string[] = [];
  filterProjectSites: string[] = [];
  filterRegions: string[] = [];
  filterApprovalTypes: string[] = [];

  // Filter dropdown options
  statusOptions: any[] = [
    { label: 'Pending', value: 'Pending' },
    { label: 'Approved', value: 'Approved' },
    { label: 'Rejected', value: 'Rejected' },
    { label: 'Cancelled', value: 'Cancelled' },
    { label: 'In Progress', value: 'InProgress' },
    { label: 'Completed', value: 'Completed' }
  ];

  priorityOptions: any[] = [
    { label: 'Critical', value: 'Critical' },
    { label: 'High', value: 'High' },
    { label: 'Medium', value: 'Medium' },
    { label: 'Low', value: 'Low' }
  ];

  approvalTypeOptions: any[] = [
    { label: 'Routine', value: 'Routine' },
    { label: 'Emergency', value: 'Emergency' },
    { label: 'Scheduled', value: 'Scheduled' }
  ];

  projectSiteOptions: any[] = [];
  regionOptions: any[] = [];

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
          this.originalReport = JSON.parse(JSON.stringify(response.data));
          this.report = response.data;
        } else {
          this.originalReport = JSON.parse(JSON.stringify(response));
          this.report = response;
        }
        this.populateFilterOptions();
        this.prepareChartData();
      },
      error: (err) => {
        this.loading = false;
        this.error = 'Failed to load explosive workflow report. Please try again.';
        console.error('Error loading report:', err);
      }
    });
  }

  populateFilterOptions() {
    if (!this.originalReport) return;

    // Extract unique project sites from approval requests
    if (this.originalReport.approvalRequests && this.originalReport.approvalRequests.length > 0) {
      const uniqueSites = new Set<string>();
      this.originalReport.approvalRequests.forEach((req: any) => {
        if (req.projectSiteName) {
          uniqueSites.add(req.projectSiteName);
        }
      });
      this.projectSiteOptions = Array.from(uniqueSites).map(siteName => ({
        label: siteName,
        value: siteName
      })).sort((a: any, b: any) => a.label.localeCompare(b.label));
    }

    // Extract unique regions from approval requests
    if (this.originalReport.approvalRequests && this.originalReport.approvalRequests.length > 0) {
      const uniqueRegions = new Set<string>();
      this.originalReport.approvalRequests.forEach((req: any) => {
        if (req.regionName) {
          uniqueRegions.add(req.regionName);
        }
      });
      this.regionOptions = Array.from(uniqueRegions).map(regionName => ({
        label: regionName,
        value: regionName
      })).sort((a: any, b: any) => a.label.localeCompare(b.label));
    }
  }

  applyFilters() {
    if (!this.originalReport) return;

    this.report = JSON.parse(JSON.stringify(this.originalReport));

    // Apply filters to approval requests
    if (this.report.approvalRequests) {
      let filteredApprovals = this.report.approvalRequests;

      // Date filter
      if (this.filterStartDate || this.filterEndDate) {
        filteredApprovals = filteredApprovals.filter((req: any) => {
          const reqDate = new Date(req.createdDate);
          if (this.filterStartDate && this.filterEndDate) {
            return reqDate >= new Date(this.filterStartDate) && reqDate <= new Date(this.filterEndDate);
          } else if (this.filterStartDate) {
            return reqDate >= new Date(this.filterStartDate);
          } else if (this.filterEndDate) {
            return reqDate <= new Date(this.filterEndDate);
          }
          return true;
        });
      }

      // Status filter
      if (this.filterStatuses.length > 0) {
        filteredApprovals = filteredApprovals.filter((req: any) =>
          this.filterStatuses.includes(req.status)
        );
      }

      // Priority filter
      if (this.filterPriorities.length > 0) {
        filteredApprovals = filteredApprovals.filter((req: any) =>
          this.filterPriorities.includes(req.priority)
        );
      }

      // Project site filter
      if (this.filterProjectSites.length > 0) {
        filteredApprovals = filteredApprovals.filter((req: any) =>
          this.filterProjectSites.includes(req.projectSiteName)
        );
      }

      // Region filter
      if (this.filterRegions.length > 0) {
        filteredApprovals = filteredApprovals.filter((req: any) =>
          this.filterRegions.includes(req.regionName)
        );
      }

      // Approval type filter
      if (this.filterApprovalTypes.length > 0) {
        filteredApprovals = filteredApprovals.filter((req: any) =>
          this.filterApprovalTypes.includes(req.approvalType)
        );
      }

      this.report.approvalRequests = filteredApprovals;
    }

    // Apply similar filters to transfer requests
    if (this.report.transferRequests) {
      let filteredTransfers = this.report.transferRequests;

      if (this.filterStatuses.length > 0) {
        filteredTransfers = filteredTransfers.filter((req: any) =>
          this.filterStatuses.includes(req.status)
        );
      }

      if (this.filterRegions.length > 0) {
        filteredTransfers = filteredTransfers.filter((req: any) =>
          this.filterRegions.includes(req.destinationRegion)
        );
      }

      this.report.transferRequests = filteredTransfers;
    }

    // Recalculate charts
    this.prepareChartData();
  }

  clearFilters() {
    this.filterStartDate = '';
    this.filterEndDate = '';
    this.filterStatuses = [];
    this.filterPriorities = [];
    this.filterProjectSites = [];
    this.filterRegions = [];
    this.filterApprovalTypes = [];

    if (this.originalReport) {
      this.report = JSON.parse(JSON.stringify(this.originalReport));
      this.prepareChartData();
    }
  }

  removeFilter(filterType: string, value: string) {
    switch (filterType) {
      case 'status':
        this.filterStatuses = this.filterStatuses.filter(s => s !== value);
        break;
      case 'priority':
        this.filterPriorities = this.filterPriorities.filter(p => p !== value);
        break;
      case 'region':
        this.filterRegions = this.filterRegions.filter(r => r !== value);
        break;
      case 'site':
        this.filterProjectSites = this.filterProjectSites.filter(s => s !== value);
        break;
    }
    this.applyFilters();
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

  async exportToPdf() {
    if (!this.report) {
      return;
    }

    try {
      // Configure pdfMake fonts
      (pdfMake as any).vfs = pdfFonts;

      // Capture charts as images
      const chartImages = await this.captureCharts();

      // Generate PDF document definition
      const docDefinition: any = {
        pageOrientation: 'portrait',
        pageSize: 'A4',
        pageMargins: [40, 80, 40, 60],
        header: () => {
          return {
            columns: [
              {
                stack: [
                  { text: 'DBMS - Detonation and Blasting Management System', style: 'systemName' },
                  { text: 'Explosive Workflow Report', style: 'reportName' }
                ],
                margin: [40, 20, 0, 0]
              }
            ]
          };
        },
        footer: (currentPage: number, pageCount: number) => {
          return {
            columns: [
              { text: 'Generated by DBMS © 2024', alignment: 'left', margin: [40, 0, 0, 0], fontSize: 8, color: '#666' },
              { text: `Page ${currentPage} of ${pageCount}`, alignment: 'right', margin: [0, 0, 40, 0], fontSize: 8, color: '#666' }
            ],
            margin: [0, 10, 0, 10]
          };
        },
        content: this.generatePdfContent(chartImages),
        styles: {
          // Cover Page Styles
          coverLogo: {
            fontSize: 36,
            bold: true,
            color: '#1e40af',
            alignment: 'center',
            letterSpacing: 2
          },
          coverSubtitle: {
            fontSize: 14,
            color: '#4b5563',
            alignment: 'center',
            italics: true
          },
          coverTitle: {
            fontSize: 28,
            bold: true,
            color: '#1f2937',
            alignment: 'center',
            letterSpacing: 1
          },
          coverSectionTitle: {
            fontSize: 14,
            bold: true,
            color: '#1e40af',
            alignment: 'center',
            decoration: 'underline'
          },
          coverDetailLabel: {
            fontSize: 11,
            bold: true,
            color: '#4b5563'
          },
          coverDetailValue: {
            fontSize: 11,
            color: '#1f2937'
          },
          coverMetricValue: {
            fontSize: 20,
            bold: true,
            color: '#1e40af',
            alignment: 'center',
            margin: [0, 0, 0, 5]
          },
          coverMetricLabel: {
            fontSize: 9,
            color: '#6b7280',
            alignment: 'center'
          },
          disclaimerTitle: {
            fontSize: 10,
            bold: true,
            color: '#991b1b',
            alignment: 'center',
            margin: [0, 0, 0, 5]
          },
          disclaimerText: {
            fontSize: 8,
            color: '#7f1d1d',
            alignment: 'justify',
            lineHeight: 1.3
          },
          // Existing Styles
          systemName: {
            fontSize: 12,
            bold: true,
            color: '#1e40af',
            margin: [0, 0, 0, 2]
          },
          reportName: {
            fontSize: 10,
            color: '#4b5563'
          },
          sectionHeader: {
            fontSize: 14,
            bold: true,
            margin: [0, 15, 0, 10],
            color: '#1f2937'
          },
          subHeader: {
            fontSize: 11,
            bold: true,
            margin: [0, 10, 0, 5],
            color: '#374151'
          },
          tableHeader: {
            bold: true,
            fontSize: 9,
            color: '#374151',
            fillColor: '#f3f4f6'
          },
          tableCell: {
            fontSize: 8,
            color: '#1f2937'
          },
          statCard: {
            fontSize: 9,
            margin: [0, 5, 0, 5]
          },
          workflowStep: {
            fontSize: 8,
            margin: [0, 3, 0, 3]
          }
        },
        defaultStyle: {
          font: 'Roboto'
        }
      };

      // Create and download PDF
      const pdfDocGenerator = pdfMake.createPdf(docDefinition);

      // Download the PDF
      const fileName = `explosive-workflow-report-${new Date().toISOString().split('T')[0]}.pdf`;
      pdfDocGenerator.download(fileName);

      // Open the PDF in a new tab
      pdfDocGenerator.open();

    } catch (error) {
      console.error('Error generating PDF:', error);
      alert('Failed to generate PDF. Please try again.');
    }
  }

  private async captureCharts(): Promise<any> {
    const chartImages: any = {};

    try {
      // Find all chart canvas elements
      const chartElements = document.querySelectorAll('canvas');

      if (chartElements.length > 0) {
        // Capture approval status chart (first chart - pie)
        if (chartElements[0] && this.approvalStatusChart) {
          chartImages.approvalStatusChart = chartElements[0].toDataURL('image/png');
        }

        // Capture transfer status chart (second chart - doughnut)
        if (chartElements[1] && this.transferStatusChart) {
          chartImages.transferStatusChart = chartElements[1].toDataURL('image/png');
        }

        // Capture requester chart (third chart - bar)
        if (chartElements[2] && this.requesterChart) {
          chartImages.requesterChart = chartElements[2].toDataURL('image/png');
        }
      }
    } catch (error) {
      console.warn('Could not capture charts:', error);
    }

    return chartImages;
  }

  private generatePdfContent(chartImages: any): any[] {
    const content: any[] = [];

    // ========== ENHANCED COVER PAGE ==========
    content.push({
      stack: [
        // Company Logo/Name Section
        {
          text: 'DBMS',
          style: 'coverLogo',
          margin: [0, 60, 0, 10]
        },
        {
          text: 'Detonation and Blasting Management System',
          style: 'coverSubtitle',
          margin: [0, 0, 0, 40]
        },

        // Decorative Line
        {
          canvas: [
            {
              type: 'line',
              x1: 100, y1: 0,
              x2: 415, y2: 0,
              lineWidth: 3,
              lineColor: '#1e40af'
            }
          ],
          margin: [0, 0, 0, 40]
        },

        // Main Report Title
        {
          text: 'EXPLOSIVE WORKFLOW REPORT',
          style: 'coverTitle',
          margin: [0, 0, 0, 50]
        },

        // Report Details Box
        {
          table: {
            widths: [120, '*'],
            body: [
              [
                { text: 'Report Type:', style: 'coverDetailLabel', border: [false, false, false, false] },
                { text: 'Explosive Approval & Transfer Workflow Analysis', style: 'coverDetailValue', border: [false, false, false, false] }
              ],
              [
                { text: 'Generated On:', style: 'coverDetailLabel', border: [false, false, false, false] },
                { text: new Date(this.report.generatedAt).toLocaleString('en-US', {
                  dateStyle: 'full',
                  timeStyle: 'short'
                }), style: 'coverDetailValue', border: [false, false, false, false] }
              ],
              [
                { text: 'Report Period:', style: 'coverDetailLabel', border: [false, false, false, false] },
                { text: this.report.startDate && this.report.endDate
                  ? `${new Date(this.report.startDate).toLocaleDateString('en-US', { dateStyle: 'medium' })} - ${new Date(this.report.endDate).toLocaleDateString('en-US', { dateStyle: 'medium' })}`
                  : 'All Time',
                  style: 'coverDetailValue', border: [false, false, false, false]
                }
              ],
              [
                { text: 'Region Filter:', style: 'coverDetailLabel', border: [false, false, false, false] },
                { text: this.report.regionFilter || 'All Regions', style: 'coverDetailValue', border: [false, false, false, false] }
              ],
              [
                { text: 'Generated By:', style: 'coverDetailLabel', border: [false, false, false, false] },
                { text: 'System Administrator', style: 'coverDetailValue', border: [false, false, false, false] }
              ]
            ]
          },
          margin: [40, 0, 40, 40]
        },

        // Key Metrics Summary
        {
          text: 'EXECUTIVE SUMMARY',
          style: 'coverSectionTitle',
          margin: [0, 20, 0, 15]
        },
        {
          table: {
            widths: ['*', '*', '*'],
            body: [
              [
                {
                  stack: [
                    { text: (this.report.statistics?.totalApprovalRequests || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Approval Requests', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#eff6ff',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.report.statistics?.totalTransferRequests || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Transfer Requests', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#f0fdf4',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.report.statistics?.approvedRequests || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Approved', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#fef3c7',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                }
              ],
              [
                {
                  stack: [
                    { text: ((this.report.statistics?.approvalRate || 0).toFixed(1)) + '%', style: 'coverMetricValue' },
                    { text: 'Approval Rate', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#f3f4f6',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.report.statistics?.totalExplosiveQuantity || 0).toFixed(2) + ' kg', style: 'coverMetricValue' },
                    { text: 'Total Explosives', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#fce7f3',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.report.turnaroundAnalysis?.averageApprovalTurnaroundDays || 0).toFixed(1) + ' days', style: 'coverMetricValue' },
                    { text: 'Avg Turnaround', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#ede9fe',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                }
              ]
            ]
          },
          margin: [0, 0, 0, 40]
        },

        // Confidentiality Notice
        {
          table: {
            widths: ['*'],
            body: [
              [
                {
                  stack: [
                    { text: 'CONFIDENTIALITY NOTICE', style: 'disclaimerTitle' },
                    {
                      text: 'This report contains highly sensitive information about explosive materials and their management. Access is restricted to authorized personnel only. Any unauthorized access, use, distribution, or disclosure is strictly prohibited and may be subject to legal consequences.',
                      style: 'disclaimerText'
                    }
                  ],
                  fillColor: '#fef2f2',
                  margin: [15, 10, 15, 10],
                  border: [false, false, false, false]
                }
              ]
            ]
          },
          margin: [0, 0, 0, 20]
        }
      ],
      pageBreak: 'after'
    });

    // Workflow Overview
    content.push({
      text: 'Workflow Overview',
      style: 'sectionHeader',
      pageBreak: 'before'
    });

    const workflowSteps = [
      { step: '1', title: 'Blasting Engineer Request', desc: 'Blasting Engineer creates an Explosive Approval Request for a project site' },
      { step: '2', title: 'Store Manager Approval', desc: 'Store Manager reviews and approves/rejects the approval request' },
      { step: '3', title: 'Store Manager Transfer Request', desc: 'Store Manager creates a Transfer Request to get explosives from central warehouse' },
      { step: '4', title: 'Explosive Manager Approval', desc: 'Explosive Manager approves the transfer and marks dispatch/completion status' }
    ];

    const workflowTable: any[] = [];
    workflowSteps.forEach((step) => {
      workflowTable.push([
        { text: step.step, style: 'tableHeader', alignment: 'center', bold: true },
        { text: step.title, style: 'tableCell', bold: true },
        { text: step.desc, style: 'tableCell' }
      ]);
    });

    content.push({
      table: {
        headerRows: 0,
        widths: ['auto', '*', '*'],
        body: workflowTable
      },
      layout: {
        hLineWidth: () => 0.5,
        vLineWidth: () => 0.5,
        hLineColor: () => '#e5e7eb',
        vLineColor: () => '#e5e7eb'
      },
      margin: [0, 0, 0, 10]
    });

    // Summary Statistics
    content.push({
      text: 'Summary Statistics',
      style: 'sectionHeader',
      pageBreak: 'before'
    });

    const stats = this.report.statistics || {};
    const statsTable = {
      table: {
        widths: ['*', 'auto'],
        body: [
          [
            { text: 'Total Approval Requests', style: 'tableCell' },
            { text: stats.totalApprovalRequests?.toString() || '0', style: 'tableCell', alignment: 'right', bold: true }
          ],
          [
            { text: 'Approval Rate', style: 'tableCell' },
            { text: (stats.approvalRate?.toFixed(1) || '0') + '%', style: 'tableCell', alignment: 'right' }
          ],
          [
            { text: 'Pending Approvals', style: 'tableCell' },
            { text: stats.pendingApprovals?.toString() || '0', style: 'tableCell', alignment: 'right' }
          ],
          [
            { text: 'Approved Requests', style: 'tableCell' },
            { text: stats.approvedRequests?.toString() || '0', style: 'tableCell', alignment: 'right' }
          ],
          [
            { text: 'Rejected Requests', style: 'tableCell' },
            { text: stats.rejectedRequests?.toString() || '0', style: 'tableCell', alignment: 'right' }
          ],
          [
            { text: 'Total Transfer Requests', style: 'tableCell' },
            { text: stats.totalTransferRequests?.toString() || '0', style: 'tableCell', alignment: 'right', bold: true }
          ],
          [
            { text: 'Transfer Completion Rate', style: 'tableCell' },
            { text: (stats.transferCompletionRate?.toFixed(1) || '0') + '%', style: 'tableCell', alignment: 'right' }
          ],
          [
            { text: 'In Progress Transfers', style: 'tableCell' },
            { text: stats.inProgressTransfers?.toString() || '0', style: 'tableCell', alignment: 'right' }
          ],
          [
            { text: 'Completed Transfers', style: 'tableCell' },
            { text: stats.completedTransfers?.toString() || '0', style: 'tableCell', alignment: 'right' }
          ],
          [
            { text: 'Total Workflow Items', style: 'tableCell' },
            { text: stats.totalWorkflowItems?.toString() || '0', style: 'tableCell', alignment: 'right', bold: true }
          ],
          [
            { text: 'Overall Completion Rate', style: 'tableCell' },
            { text: (stats.overallCompletionRate?.toFixed(1) || '0') + '%', style: 'tableCell', alignment: 'right' }
          ],
          [
            { text: 'Average Turnaround Time', style: 'tableCell' },
            { text: (this.report.turnaroundAnalysis?.overallAverageTurnaroundDays?.toFixed(1) || '0') + ' days', style: 'tableCell', alignment: 'right', bold: true }
          ]
        ]
      },
      layout: {
        hLineWidth: () => 0.5,
        vLineWidth: () => 0.5,
        hLineColor: () => '#e5e7eb',
        vLineColor: () => '#e5e7eb'
      }
    };

    content.push(statsTable);

    // Approval Requests Section
    if (this.report.approvalRequests && this.report.approvalRequests.length > 0) {
      content.push({
        text: 'Stage 1: Blasting Engineer Approval Requests',
        style: 'sectionHeader',
        pageBreak: 'before'
      });

      // Add approval status chart if available
      if (chartImages.approvalStatusChart) {
        content.push({
          image: chartImages.approvalStatusChart,
          width: 400,
          alignment: 'center',
          margin: [0, 10, 0, 20]
        });
      }

      // Approval Requests Table (limited to first 10)
      const approvalTableBody: any[] = [
        [
          { text: 'ID', style: 'tableHeader' },
          { text: 'Project Site', style: 'tableHeader' },
          { text: 'Requested By', style: 'tableHeader' },
          { text: 'Status', style: 'tableHeader' },
          { text: 'Priority', style: 'tableHeader' },
          { text: 'Turnaround', style: 'tableHeader', alignment: 'right' }
        ]
      ];

      const visibleApprovals = this.report.approvalRequests.slice(0, 10);
      visibleApprovals.forEach((request: any) => {
        approvalTableBody.push([
          { text: request.id?.toString() || '', style: 'tableCell', fontSize: 7 },
          { text: request.projectSiteName || '', style: 'tableCell', fontSize: 7 },
          { text: request.requestedByUserName || '', style: 'tableCell', fontSize: 7 },
          { text: request.status || '', style: 'tableCell', fontSize: 7 },
          { text: request.priority || '', style: 'tableCell', fontSize: 7 },
          { text: request.turnaroundDays ? (request.turnaroundDays + ' days') : 'N/A', style: 'tableCell', alignment: 'right', fontSize: 7 }
        ]);
      });

      content.push({
        table: {
          headerRows: 1,
          widths: ['auto', '*', '*', 'auto', 'auto', 'auto'],
          body: approvalTableBody
        },
        layout: {
          hLineWidth: () => 0.5,
          vLineWidth: () => 0.5,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      });
    }

    // Transfer Requests Section
    if (this.report.transferRequests && this.report.transferRequests.length > 0) {
      content.push({
        text: 'Stage 2: Store Manager Transfer Requests',
        style: 'sectionHeader',
        pageBreak: 'before'
      });

      // Add transfer status chart if available
      if (chartImages.transferStatusChart) {
        content.push({
          image: chartImages.transferStatusChart,
          width: 400,
          alignment: 'center',
          margin: [0, 10, 0, 20]
        });
      }

      // Quantity Analysis
      const transferAnalysis = this.report.transferAnalysis || {};
      content.push({
        text: 'Quantity Analysis',
        style: 'subHeader'
      });

      const quantityTable = {
        table: {
          widths: ['*', 'auto'],
          body: [
            [
              { text: 'Total Requested Quantity', style: 'tableCell' },
              { text: (transferAnalysis.totalRequestedQuantity?.toFixed(2) || '0.00') + ' kg', style: 'tableCell', alignment: 'right' }
            ],
            [
              { text: 'Total Approved Quantity', style: 'tableCell' },
              { text: (transferAnalysis.totalApprovedQuantity?.toFixed(2) || '0.00') + ' kg', style: 'tableCell', alignment: 'right' }
            ],
            [
              { text: 'Total Transferred Quantity', style: 'tableCell' },
              { text: (transferAnalysis.totalTransferredQuantity?.toFixed(2) || '0.00') + ' kg', style: 'tableCell', alignment: 'right' }
            ],
            [
              { text: 'Fulfillment Rate', style: 'tableCell', bold: true },
              { text: (transferAnalysis.quantityFulfillmentRate?.toFixed(1) || '0.0') + '%', style: 'tableCell', alignment: 'right', bold: true }
            ],
            [
              { text: 'Overdue Requests', style: 'tableCell' },
              { text: transferAnalysis.overdueCount?.toString() || '0', style: 'tableCell', alignment: 'right', color: '#ef4444' }
            ],
            [
              { text: 'Urgent Requests', style: 'tableCell' },
              { text: transferAnalysis.urgentCount?.toString() || '0', style: 'tableCell', alignment: 'right', color: '#f59e0b' }
            ]
          ]
        },
        layout: {
          hLineWidth: () => 0.5,
          vLineWidth: () => 0.5,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        },
        margin: [0, 0, 0, 15]
      };

      content.push(quantityTable);

      // Transfer Requests Table (limited to first 10)
      const transferTableBody: any[] = [
        [
          { text: 'Request #', style: 'tableHeader' },
          { text: 'Explosive Type', style: 'tableHeader' },
          { text: 'Destination', style: 'tableHeader' },
          { text: 'Quantity (kg)', style: 'tableHeader', alignment: 'right' },
          { text: 'Status', style: 'tableHeader' },
          { text: 'Requested By', style: 'tableHeader' }
        ]
      ];

      const visibleTransfers = this.report.transferRequests.slice(0, 10);
      visibleTransfers.forEach((request: any) => {
        transferTableBody.push([
          { text: request.requestNumber || '', style: 'tableCell', fontSize: 7, bold: true },
          { text: request.explosiveTypeName || '', style: 'tableCell', fontSize: 7 },
          { text: request.destinationStoreName || '', style: 'tableCell', fontSize: 7 },
          { text: request.requestedQuantity?.toFixed(2) || '0.00', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: request.status || '', style: 'tableCell', fontSize: 7 },
          { text: request.requestedByUserName || '', style: 'tableCell', fontSize: 7 }
        ]);
      });

      content.push({
        table: {
          headerRows: 1,
          widths: ['auto', '*', '*', 'auto', 'auto', '*'],
          body: transferTableBody
        },
        layout: {
          hLineWidth: () => 0.5,
          vLineWidth: () => 0.5,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      });
    }

    // Requester Statistics Section
    if (this.report.requesterStatistics && this.report.requesterStatistics.length > 0) {
      content.push({
        text: 'Requester Performance Analysis',
        style: 'sectionHeader',
        pageBreak: 'before'
      });

      // Add requester chart if available
      if (chartImages.requesterChart) {
        content.push({
          image: chartImages.requesterChart,
          width: 500,
          alignment: 'center',
          margin: [0, 10, 0, 20]
        });
      }

      // Requester Statistics Table (limited to first 10)
      const requesterTableBody: any[] = [
        [
          { text: 'User Name', style: 'tableHeader' },
          { text: 'Role', style: 'tableHeader' },
          { text: 'Total', style: 'tableHeader', alignment: 'right' },
          { text: 'Approved', style: 'tableHeader', alignment: 'right' },
          { text: 'Rejected', style: 'tableHeader', alignment: 'right' },
          { text: 'Rate', style: 'tableHeader', alignment: 'right' }
        ]
      ];

      const visibleRequesters = this.report.requesterStatistics.slice(0, 10);
      visibleRequesters.forEach((requester: any) => {
        requesterTableBody.push([
          { text: requester.userName || '', style: 'tableCell', fontSize: 7, bold: true },
          { text: requester.userRole || '', style: 'tableCell', fontSize: 7 },
          { text: requester.totalRequests?.toString() || '0', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: requester.approvedCount?.toString() || '0', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: requester.rejectedCount?.toString() || '0', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: (requester.approvalRate?.toFixed(1) || '0.0') + '%', style: 'tableCell', alignment: 'right', fontSize: 7 }
        ]);
      });

      content.push({
        table: {
          headerRows: 1,
          widths: ['*', '*', 'auto', 'auto', 'auto', 'auto'],
          body: requesterTableBody
        },
        layout: {
          hLineWidth: () => 0.5,
          vLineWidth: () => 0.5,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      });
    }

    return content;
  }

}
