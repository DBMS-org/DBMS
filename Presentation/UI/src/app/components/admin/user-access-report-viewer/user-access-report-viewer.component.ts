import { Component, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ReportService } from '../../../services/report.service';
import { FormsModule } from '@angular/forms';

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
import pdfMake from 'pdfmake/build/pdfmake';
import pdfFonts from 'pdfmake/build/vfs_fonts';

@Component({
  selector: 'app-user-access-report-viewer',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
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
  originalReport: any = null;
  loading = false;
  error: string | null = null;

  // Filter properties
  isFilterExpanded: boolean = false;
  filterStartDate: string = '';
  filterEndDate: string = '';
  filterUserStatuses: string[] = [];
  filterActivityStatuses: string[] = [];
  filterRoles: string[] = [];
  filterRegions: string[] = [];

  // Filter options
  userStatusOptions = [
    { label: 'Active', value: 'Active' },
    { label: 'Inactive', value: 'Inactive' }
  ];
  activityStatusOptions: any[] = [];
  roleOptions: any[] = [];
  regionOptions: any[] = [];

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
            this.originalReport = JSON.parse(JSON.stringify(response.data));
          } else {
            this.report = response;
            this.originalReport = JSON.parse(JSON.stringify(response));
          }
          this.populateFilterOptions();
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

  populateFilterOptions() {
    if (!this.originalReport) return;

    // Extract unique activity statuses
    if (this.originalReport.userActivity && this.originalReport.userActivity.length > 0) {
      const uniqueStatuses = new Set<string>();
      this.originalReport.userActivity.forEach((activity: any) => {
        if (activity.activityStatus) {
          uniqueStatuses.add(activity.activityStatus);
        }
      });
      this.activityStatusOptions = Array.from(uniqueStatuses).map(status => ({
        label: status,
        value: status
      })).sort((a: any, b: any) => a.label.localeCompare(b.label));
    }

    // Extract unique roles
    if (this.originalReport.roleDistribution && this.originalReport.roleDistribution.length > 0) {
      this.roleOptions = this.originalReport.roleDistribution.map((role: any) => ({
        label: role.roleName,
        value: role.roleName
      })).sort((a: any, b: any) => a.label.localeCompare(b.label));
    }

    // Extract unique regions
    if (this.originalReport.users && this.originalReport.users.length > 0) {
      const uniqueRegions = new Set<string>();
      this.originalReport.users.forEach((user: any) => {
        if (user.region) {
          uniqueRegions.add(user.region);
        }
      });
      this.regionOptions = Array.from(uniqueRegions).map(region => ({
        label: region,
        value: region
      })).sort((a: any, b: any) => a.label.localeCompare(b.label));
    }
  }

  applyFilters() {
    if (!this.originalReport) return;

    this.report = JSON.parse(JSON.stringify(this.originalReport));

    // Apply date range filter
    if (this.filterStartDate || this.filterEndDate) {
      if (this.report.users) {
        this.report.users = this.report.users.filter((user: any) => {
          const userDate = new Date(user.createdAt);
          if (this.filterStartDate && this.filterEndDate) {
            const startDate = new Date(this.filterStartDate);
            const endDate = new Date(this.filterEndDate);
            return userDate >= startDate && userDate <= endDate;
          } else if (this.filterStartDate) {
            const startDate = new Date(this.filterStartDate);
            return userDate >= startDate;
          } else if (this.filterEndDate) {
            const endDate = new Date(this.filterEndDate);
            return userDate <= endDate;
          }
          return true;
        });
      }
    }

    // Apply user status filter
    if (this.filterUserStatuses.length > 0) {
      if (this.report.users) {
        this.report.users = this.report.users.filter((user: any) => {
          const status = user.isActive ? 'Active' : 'Inactive';
          return this.filterUserStatuses.includes(status);
        });
      }
      if (this.report.userActivity) {
        this.report.userActivity = this.report.userActivity.filter((activity: any) => {
          const status = activity.isActive ? 'Active' : 'Inactive';
          return this.filterUserStatuses.includes(status);
        });
      }
    }

    // Apply activity status filter
    if (this.filterActivityStatuses.length > 0) {
      if (this.report.userActivity) {
        this.report.userActivity = this.report.userActivity.filter((activity: any) =>
          this.filterActivityStatuses.includes(activity.activityStatus)
        );
      }
    }

    // Apply roles filter
    if (this.filterRoles.length > 0) {
      if (this.report.users) {
        this.report.users = this.report.users.filter((user: any) =>
          user.roles.some((role: string) => this.filterRoles.includes(role))
        );
      }
      if (this.report.userActivity) {
        this.report.userActivity = this.report.userActivity.filter((activity: any) =>
          this.filterRoles.includes(activity.primaryRole)
        );
      }
    }

    // Apply region filter
    if (this.filterRegions.length > 0) {
      if (this.report.users) {
        this.report.users = this.report.users.filter((user: any) =>
          this.filterRegions.includes(user.region)
        );
      }
    }

    // Recalculate statistics
    if (this.report.users) {
      this.report.statistics.totalUsers = this.report.users.length;
      this.report.statistics.activeUsers = this.report.users.filter((u: any) => u.isActive).length;
      this.report.statistics.inactiveUsers = this.report.users.filter((u: any) => !u.isActive).length;
    }

    this.prepareChartData();
  }

  clearFilters() {
    this.filterStartDate = '';
    this.filterEndDate = '';
    this.filterUserStatuses = [];
    this.filterActivityStatuses = [];
    this.filterRoles = [];
    this.filterRegions = [];

    if (this.originalReport) {
      this.report = JSON.parse(JSON.stringify(this.originalReport));
      this.prepareChartData();
    }
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
                  { text: 'User & Access Management Report', style: 'reportName' }
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
          }
        },
        defaultStyle: {
          font: 'Roboto'
        }
      };

      // Create and download PDF
      const pdfDocGenerator = pdfMake.createPdf(docDefinition);

      // Download the PDF
      const fileName = `user-access-report-${new Date().toISOString().split('T')[0]}.pdf`;
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
        // Capture user status chart (first chart - doughnut)
        if (chartElements[0] && this.userStatusChart) {
          chartImages.userStatusChart = chartElements[0].toDataURL('image/png');
        }

        // Capture activity status chart (second chart - pie)
        if (chartElements[1] && this.activityStatusChart) {
          chartImages.activityStatusChart = chartElements[1].toDataURL('image/png');
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
          text: 'USER & ACCESS MANAGEMENT REPORT',
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
                { text: 'User Access Control & Permission Analysis', style: 'coverDetailValue', border: [false, false, false, false] }
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
                    { text: (this.report.statistics?.totalUsers || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Total Users', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#eff6ff',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.report.statistics?.activeUsers || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Active Users', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#f0fdf4',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.report.statistics?.totalRoles || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Total Roles', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#fef3c7',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                }
              ],
              [
                {
                  stack: [
                    { text: (this.report.statistics?.adminUsers || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Admin Users', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#f3f4f6',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.report.statistics?.regularUsers || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Regular Users', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#fce7f3',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.report.statistics?.usersWithMultipleRoles || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Multi-Role Users', style: 'coverMetricLabel' }
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
                      text: 'This report contains sensitive user access and permission information. It is intended solely for authorized system administrators and security personnel. Unauthorized distribution or disclosure of user data is strictly prohibited and may violate privacy regulations.',
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
            { text: 'Total Users', style: 'tableCell' },
            { text: stats.totalUsers?.toString() || '0', style: 'tableCell', alignment: 'right', bold: true }
          ],
          [
            { text: 'Active Users', style: 'tableCell' },
            { text: stats.activeUsers?.toString() || '0', style: 'tableCell', alignment: 'right' }
          ],
          [
            { text: 'Inactive Users', style: 'tableCell' },
            { text: stats.inactiveUsers?.toString() || '0', style: 'tableCell', alignment: 'right' }
          ],
          [
            { text: 'Total Roles', style: 'tableCell' },
            { text: stats.totalRoles?.toString() || '0', style: 'tableCell', alignment: 'right', bold: true }
          ],
          [
            { text: 'Users with Multiple Roles', style: 'tableCell' },
            { text: stats.usersWithMultipleRoles?.toString() || '0', style: 'tableCell', alignment: 'right' }
          ],
          [
            { text: 'Admin Users', style: 'tableCell' },
            { text: stats.adminUsers?.toString() || '0', style: 'tableCell', alignment: 'right' }
          ],
          [
            { text: 'Regular Users', style: 'tableCell' },
            { text: stats.regularUsers?.toString() || '0', style: 'tableCell', alignment: 'right' }
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

    // User Overview Section
    if (this.report.users && this.report.users.length > 0) {
      content.push({
        text: 'User Overview',
        style: 'sectionHeader',
        pageBreak: 'before'
      });

      // Add charts if available
      const chartsRow: any[] = [];
      if (chartImages.userStatusChart) {
        chartsRow.push({
          image: chartImages.userStatusChart,
          width: 200,
          alignment: 'center'
        });
      }
      if (chartImages.activityStatusChart) {
        chartsRow.push({
          image: chartImages.activityStatusChart,
          width: 200,
          alignment: 'center'
        });
      }

      if (chartsRow.length > 0) {
        content.push({
          columns: chartsRow,
          columnGap: 20,
          margin: [0, 10, 0, 20]
        });
      }

      // Users Table (limited to first 10)
      const usersTableBody: any[] = [
        [
          { text: 'ID', style: 'tableHeader' },
          { text: 'Username', style: 'tableHeader' },
          { text: 'Email', style: 'tableHeader' },
          { text: 'Region', style: 'tableHeader' },
          { text: 'Roles', style: 'tableHeader' },
          { text: 'Status', style: 'tableHeader' }
        ]
      ];

      const visibleUsers = this.report.users.slice(0, 10);
      visibleUsers.forEach((user: any) => {
        usersTableBody.push([
          { text: user.userId?.toString() || '', style: 'tableCell', fontSize: 7 },
          { text: user.username || '', style: 'tableCell', fontSize: 7, bold: true },
          { text: user.email || '', style: 'tableCell', fontSize: 7 },
          { text: user.region || '', style: 'tableCell', fontSize: 7 },
          { text: (user.roles || []).join(', '), style: 'tableCell', fontSize: 7 },
          { text: user.isActive ? 'Active' : 'Inactive', style: 'tableCell', fontSize: 7, color: user.isActive ? '#10b981' : '#ef4444' }
        ]);
      });

      content.push({
        table: {
          headerRows: 1,
          widths: ['auto', '*', '*', 'auto', '*', 'auto'],
          body: usersTableBody
        },
        layout: {
          hLineWidth: () => 0.5,
          vLineWidth: () => 0.5,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      });
    }

    // User Activity Section
    if (this.report.userActivity && this.report.userActivity.length > 0) {
      content.push({
        text: 'User Activity Summary',
        style: 'sectionHeader',
        pageBreak: 'before'
      });

      // User Activity Table (limited to first 10)
      const activityTableBody: any[] = [
        [
          { text: 'ID', style: 'tableHeader' },
          { text: 'Username', style: 'tableHeader' },
          { text: 'Primary Role', style: 'tableHeader' },
          { text: 'Last Login', style: 'tableHeader' },
          { text: 'Activity Status', style: 'tableHeader' },
          { text: 'Account Status', style: 'tableHeader' }
        ]
      ];

      const visibleActivity = this.report.userActivity.slice(0, 10);
      visibleActivity.forEach((activity: any) => {
        activityTableBody.push([
          { text: activity.userId?.toString() || '', style: 'tableCell', fontSize: 7 },
          { text: activity.username || '', style: 'tableCell', fontSize: 7, bold: true },
          { text: activity.primaryRole || '', style: 'tableCell', fontSize: 7 },
          { text: activity.lastLoginDate ? new Date(activity.lastLoginDate).toLocaleDateString() : 'Never', style: 'tableCell', fontSize: 7 },
          { text: activity.activityStatus || '', style: 'tableCell', fontSize: 7 },
          { text: activity.isActive ? 'Active' : 'Inactive', style: 'tableCell', fontSize: 7, color: activity.isActive ? '#10b981' : '#ef4444' }
        ]);
      });

      content.push({
        table: {
          headerRows: 1,
          widths: ['auto', '*', '*', 'auto', 'auto', 'auto'],
          body: activityTableBody
        },
        layout: {
          hLineWidth: () => 0.5,
          vLineWidth: () => 0.5,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      });
    }

    // Role Distribution Section
    if (this.report.roleDistribution && this.report.roleDistribution.length > 0) {
      content.push({
        text: 'Role Distribution',
        style: 'sectionHeader',
        pageBreak: 'before'
      });

      // Role Distribution Table
      const roleTableBody: any[] = [
        [
          { text: 'Role Name', style: 'tableHeader' },
          { text: 'User Count', style: 'tableHeader', alignment: 'right' },
          { text: 'Percentage', style: 'tableHeader', alignment: 'right' }
        ]
      ];

      const totalUsers = this.report.statistics.totalUsers || 1;
      this.report.roleDistribution.forEach((role: any) => {
        const percentage = ((role.userCount / totalUsers) * 100).toFixed(1);
        roleTableBody.push([
          { text: role.roleName || '', style: 'tableCell', bold: true },
          { text: role.userCount?.toString() || '0', style: 'tableCell', alignment: 'right' },
          { text: percentage + '%', style: 'tableCell', alignment: 'right' }
        ]);
      });

      content.push({
        table: {
          headerRows: 1,
          widths: ['*', 'auto', 'auto'],
          body: roleTableBody
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
