import { Component, OnInit, ViewChildren, QueryList, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportService, ReportFilter } from '../../../services/report.service';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { CardModule } from 'primeng/card';
import { ChartModule } from 'primeng/chart';
import pdfMake from 'pdfmake/build/pdfmake';
import pdfFonts from 'pdfmake/build/vfs_fonts';

@Component({
  selector: 'app-drilling-operations-report-viewer',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, TabViewModule, ButtonModule, TagModule, CardModule, ChartModule],
  templateUrl: './drilling-operations-report-viewer.component.html',
  styleUrls: ['./drilling-operations-report-viewer.component.scss']
})
export class DrillingOperationsReportViewerComponent implements OnInit {
  reportData: any = null;
  originalReportData: any = null;
  loading = false;
  error: string | null = null;
  activeTab: string = 'projectSites';
  isFilterExpanded: boolean = false;

  // Filter parameters
  startDate: string | null = null;
  endDate: string | null = null;
  regionId: string | null = null;

  // Client-side filters
  filterStartDate: string = '';
  filterEndDate: string = '';
  filterProjectSites: string[] = [];
  filterRegions: string[] = [];

  // Filter dropdown options
  projectSiteOptions: any[] = [];
  regionOptions: any[] = [];

  // Chart data
  projectSiteChartData: any = null;
  regionalChartData: any = null;

  @ViewChildren('chartCanvas') chartCanvases!: QueryList<ElementRef>;

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
          this.originalReportData = JSON.parse(JSON.stringify(response.data));
          this.reportData = response.data;
          this.prepareChartData();
          this.populateFilterOptions();
        } else {
          this.originalReportData = JSON.parse(JSON.stringify(response));
          this.reportData = response;
          this.prepareChartData();
          this.populateFilterOptions();
        }
      },
      error: (err) => {
        this.loading = false;
        this.error = 'Failed to load drilling operations report. Please try again.';
        console.error('Error loading report:', err);
      }
    });
  }

  populateFilterOptions() {
    if (!this.originalReportData) return;

    // Extract unique project sites from DrillingByProjectSite
    if (this.originalReportData.drillingByProjectSite && this.originalReportData.drillingByProjectSite.length > 0) {
      const uniqueSites = new Set<string>();
      this.originalReportData.drillingByProjectSite.forEach((site: any) => {
        if (site.projectSiteName) {
          uniqueSites.add(site.projectSiteName);
        }
      });
      this.projectSiteOptions = Array.from(uniqueSites).map(siteName => ({
        label: siteName,
        value: siteName
      })).sort((a: any, b: any) => a.label.localeCompare(b.label));
    }

    // Extract unique regions from DrillingByRegion
    if (this.originalReportData.drillingByRegion && this.originalReportData.drillingByRegion.length > 0) {
      this.regionOptions = this.originalReportData.drillingByRegion.map((region: any) => ({
        label: region.regionName,
        value: region.regionName
      })).sort((a: any, b: any) => a.label.localeCompare(b.label));
    }
  }

  applyFilters() {
    if (!this.originalReportData) return;

    this.reportData = JSON.parse(JSON.stringify(this.originalReportData));

    // Apply date filters if needed
    // Add your date filtering logic here based on your data structure

    // Apply project site filter
    if (this.filterProjectSites.length > 0) {
      if (this.reportData.drillingByProjectSite) {
        this.reportData.drillingByProjectSite = this.reportData.drillingByProjectSite.filter((site: any) =>
          this.filterProjectSites.includes(site.projectSiteName)
        );
      }

      // Also filter allDrillHoles by project site
      if (this.reportData.allDrillHoles) {
        this.reportData.allDrillHoles = this.reportData.allDrillHoles.filter((hole: any) =>
          this.filterProjectSites.includes(hole.projectSiteName)
        );
      }

      // Also filter allDrillPoints by project site
      if (this.reportData.allDrillPoints) {
        this.reportData.allDrillPoints = this.reportData.allDrillPoints.filter((point: any) =>
          this.filterProjectSites.includes(point.projectSiteName)
        );
      }
    }

    // Apply region filter
    if (this.filterRegions.length > 0) {
      if (this.reportData.drillingByRegion) {
        this.reportData.drillingByRegion = this.reportData.drillingByRegion.filter((region: any) =>
          this.filterRegions.includes(region.regionName)
        );
      }

      // Also filter allDrillHoles by region
      if (this.reportData.allDrillHoles) {
        this.reportData.allDrillHoles = this.reportData.allDrillHoles.filter((hole: any) =>
          this.filterRegions.includes(hole.regionName)
        );
      }

      // Also filter allDrillPoints by region
      if (this.reportData.allDrillPoints) {
        this.reportData.allDrillPoints = this.reportData.allDrillPoints.filter((point: any) =>
          this.filterRegions.includes(point.regionName)
        );
      }
    }

    // Recalculate statistics
    this.prepareChartData();
  }

  clearFilters() {
    this.filterStartDate = '';
    this.filterEndDate = '';
    this.filterProjectSites = [];
    this.filterRegions = [];

    if (this.originalReportData) {
      this.reportData = JSON.parse(JSON.stringify(this.originalReportData));
      this.prepareChartData();
    }
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

  async exportToPdf() {
    if (!this.reportData) {
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
                  { text: 'Drilling Operations Report', style: 'reportName' }
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
      const fileName = `drilling-operations-report-${new Date().toISOString().split('T')[0]}.pdf`;
      pdfDocGenerator.download(fileName);

      // Open the PDF in a new tab
      pdfDocGenerator.open();

    } catch (error) {
      console.error('Error generating PDF:', error);
      alert('Failed to generate PDF. Please try again.');
    }
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

  private async captureCharts(): Promise<{ projectSiteChart?: string, regionalChart?: string }> {
    const chartImages: any = {};

    try {
      // Find all chart canvas elements
      const chartElements = document.querySelectorAll('canvas');

      if (chartElements.length > 0) {
        // Capture project site chart (first chart)
        if (chartElements[0] && this.projectSiteChartData) {
          chartImages.projectSiteChart = chartElements[0].toDataURL('image/png');
        }

        // Capture regional chart (second chart if exists)
        if (chartElements[1] && this.regionalChartData) {
          chartImages.regionalChart = chartElements[1].toDataURL('image/png');
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
          text: 'DRILLING OPERATIONS REPORT',
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
                { text: 'Comprehensive Drilling Operations Analysis', style: 'coverDetailValue', border: [false, false, false, false] }
              ],
              [
                { text: 'Generated On:', style: 'coverDetailLabel', border: [false, false, false, false] },
                { text: new Date(this.reportData.generatedAt).toLocaleString('en-US', {
                  dateStyle: 'full',
                  timeStyle: 'short'
                }), style: 'coverDetailValue', border: [false, false, false, false] }
              ],
              [
                { text: 'Report Period:', style: 'coverDetailLabel', border: [false, false, false, false] },
                { text: this.reportData.startDate && this.reportData.endDate
                  ? `${new Date(this.reportData.startDate).toLocaleDateString('en-US', { dateStyle: 'medium' })} - ${new Date(this.reportData.endDate).toLocaleDateString('en-US', { dateStyle: 'medium' })}`
                  : 'All Time',
                  style: 'coverDetailValue', border: [false, false, false, false]
                }
              ],
              [
                { text: 'Region Filter:', style: 'coverDetailLabel', border: [false, false, false, false] },
                { text: this.reportData.regionFilter || 'All Regions', style: 'coverDetailValue', border: [false, false, false, false] }
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
                    { text: (this.reportData.statistics?.totalDrillHoles || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Total Drill Holes', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#eff6ff',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.reportData.statistics?.totalDrillPoints || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Total Drill Points', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#f0fdf4',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.reportData.statistics?.totalProjectSites || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Project Sites', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#fef3c7',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                }
              ],
              [
                {
                  stack: [
                    { text: (this.reportData.statistics?.totalDrillingMeters || 0).toFixed(2) + ' m', style: 'coverMetricValue' },
                    { text: 'Total Drilling', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#f3f4f6',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.reportData.statistics?.averageDrillDepth || 0).toFixed(2) + ' m', style: 'coverMetricValue' },
                    { text: 'Average Depth', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#fce7f3',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.reportData.statistics?.totalBlastConnections || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Blast Connections', style: 'coverMetricLabel' }
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
                      text: 'This report contains confidential information about drilling operations and explosive management. It is intended solely for authorized personnel. Unauthorized distribution, copying, or disclosure of this information is strictly prohibited.',
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

    const stats = this.reportData.statistics || {};
    const statsTable = {
      table: {
        widths: ['*', '*', '*'],
        body: [
          [
            { text: 'Metric', style: 'tableHeader', alignment: 'left' },
            { text: 'Value', style: 'tableHeader', alignment: 'right' },
            { text: 'Unit', style: 'tableHeader', alignment: 'left' }
          ],
          [
            { text: 'Total Drill Holes', style: 'tableCell' },
            { text: (stats.totalDrillHoles || 0).toString(), style: 'tableCell', alignment: 'right' },
            { text: 'count', style: 'tableCell' }
          ],
          [
            { text: 'Total Drill Points', style: 'tableCell' },
            { text: (stats.totalDrillPoints || 0).toString(), style: 'tableCell', alignment: 'right' },
            { text: 'count', style: 'tableCell' }
          ],
          [
            { text: 'Blast Connections', style: 'tableCell' },
            { text: (stats.totalBlastConnections || 0).toString(), style: 'tableCell', alignment: 'right' },
            { text: 'count', style: 'tableCell' }
          ],
          [
            { text: 'Project Sites', style: 'tableCell' },
            { text: (stats.totalProjectSites || 0).toString(), style: 'tableCell', alignment: 'right' },
            { text: 'count', style: 'tableCell' }
          ],
          [
            { text: 'Total Drilling', style: 'tableCell' },
            { text: (stats.totalDrillingMeters || 0).toFixed(2), style: 'tableCell', alignment: 'right' },
            { text: 'meters', style: 'tableCell' }
          ],
          [
            { text: 'Average Depth', style: 'tableCell' },
            { text: (stats.averageDrillDepth || 0).toFixed(2), style: 'tableCell', alignment: 'right' },
            { text: 'meters', style: 'tableCell' }
          ],
          [
            { text: 'Active Patterns', style: 'tableCell' },
            { text: (stats.activePatterns || 0).toString(), style: 'tableCell', alignment: 'right' },
            { text: 'count', style: 'tableCell' }
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

    // Project Sites Section
    if (this.reportData.drillingByProjectSite && this.reportData.drillingByProjectSite.length > 0) {
      content.push({
        text: 'Drilling by Project Site',
        style: 'sectionHeader',
        pageBreak: 'before'
      });

      // Add chart if available
      if (chartImages.projectSiteChart) {
        content.push({
          image: chartImages.projectSiteChart,
          width: 500,
          alignment: 'center',
          margin: [0, 10, 0, 20]
        });
      }

      // Project Sites Table (limited to visible data)
      const projectSitesTableBody: any[] = [
        [
          { text: 'Site Name', style: 'tableHeader' },
          { text: 'Project', style: 'tableHeader' },
          { text: 'Region', style: 'tableHeader' },
          { text: 'Holes', style: 'tableHeader', alignment: 'right' },
          { text: 'Points', style: 'tableHeader', alignment: 'right' },
          { text: 'Total Depth (m)', style: 'tableHeader', alignment: 'right' },
          { text: 'Avg Depth (m)', style: 'tableHeader', alignment: 'right' }
        ]
      ];

      // Include all data (filtered if filters are applied)
      this.reportData.drillingByProjectSite.forEach((site: any) => {
        projectSitesTableBody.push([
          { text: site.projectSiteName || '', style: 'tableCell', fontSize: 7 },
          { text: site.projectName || '', style: 'tableCell', fontSize: 7 },
          { text: site.regionName || '', style: 'tableCell', fontSize: 7 },
          { text: site.drillHoleCount?.toString() || '0', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: site.drillPointCount?.toString() || '0', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: site.totalDepth?.toFixed(2) || '0.00', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: site.averageDepth?.toFixed(2) || '0.00', style: 'tableCell', alignment: 'right', fontSize: 7 }
        ]);
      });

      content.push({
        table: {
          headerRows: 1,
          widths: ['*', '*', 'auto', 'auto', 'auto', 'auto', 'auto'],
          body: projectSitesTableBody
        },
        layout: {
          hLineWidth: () => 0.5,
          vLineWidth: () => 0.5,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      });
    }

    // Regional Distribution Section
    if (this.reportData.drillingByRegion && this.reportData.drillingByRegion.length > 0) {
      content.push({
        text: 'Drilling by Region',
        style: 'sectionHeader',
        pageBreak: 'before'
      });

      // Add chart if available
      if (chartImages.regionalChart) {
        content.push({
          image: chartImages.regionalChart,
          width: 500,
          alignment: 'center',
          margin: [0, 10, 0, 20]
        });
      }

      // Regional Table
      const regionalTableBody: any[] = [
        [
          { text: 'Region', style: 'tableHeader' },
          { text: 'Drill Holes', style: 'tableHeader', alignment: 'right' },
          { text: 'Drill Points', style: 'tableHeader', alignment: 'right' },
          { text: 'Sites', style: 'tableHeader', alignment: 'right' },
          { text: 'Total Depth (m)', style: 'tableHeader', alignment: 'right' },
          { text: 'Avg Depth (m)', style: 'tableHeader', alignment: 'right' }
        ]
      ];

      // Include all regions (filtered if filters are applied)
      this.reportData.drillingByRegion.forEach((region: any) => {
        regionalTableBody.push([
          { text: region.regionName || '', style: 'tableCell', bold: true },
          { text: region.totalDrillHoles?.toString() || '0', style: 'tableCell', alignment: 'right' },
          { text: region.totalDrillPoints?.toString() || '0', style: 'tableCell', alignment: 'right' },
          { text: region.projectSiteCount?.toString() || '0', style: 'tableCell', alignment: 'right' },
          { text: region.totalDepth?.toFixed(2) || '0.00', style: 'tableCell', alignment: 'right' },
          { text: region.averageDepth?.toFixed(2) || '0.00', style: 'tableCell', alignment: 'right' }
        ]);
      });

      content.push({
        table: {
          headerRows: 1,
          widths: ['*', 'auto', 'auto', 'auto', 'auto', 'auto'],
          body: regionalTableBody
        },
        layout: {
          hLineWidth: () => 0.5,
          vLineWidth: () => 0.5,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      });
    }

    // Blast Connections Section
    if (this.reportData.blastConnections && this.reportData.blastConnections.length > 0) {
      content.push({
        text: 'Blast Connection Details',
        style: 'sectionHeader',
        pageBreak: 'before'
      });

      const connectionsTableBody: any[] = [
        [
          { text: 'Type', style: 'tableHeader' },
          { text: 'Point 1 ID', style: 'tableHeader' },
          { text: 'Point 2 ID', style: 'tableHeader' },
          { text: 'Delay (ms)', style: 'tableHeader', alignment: 'right' },
          { text: 'Sequence', style: 'tableHeader', alignment: 'right' },
          { text: 'Project Site', style: 'tableHeader' }
        ]
      ];

      // Include all blast connections (filtered if filters are applied)
      this.reportData.blastConnections.forEach((conn: any) => {
        connectionsTableBody.push([
          { text: conn.connectorType || '', style: 'tableCell', fontSize: 7 },
          { text: conn.point1DrillPointId?.toString() || '', style: 'tableCell', fontSize: 7 },
          { text: conn.point2DrillPointId?.toString() || '', style: 'tableCell', fontSize: 7 },
          { text: conn.delay?.toString() || '0', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: conn.sequence?.toString() || '0', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: conn.projectSiteName || '', style: 'tableCell', fontSize: 7 }
        ]);
      });

      content.push({
        table: {
          headerRows: 1,
          widths: ['auto', 'auto', 'auto', 'auto', 'auto', '*'],
          body: connectionsTableBody
        },
        layout: {
          hLineWidth: () => 0.5,
          vLineWidth: () => 0.5,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      });
    }

    // Explosive Calculations Section
    if (this.reportData.explosiveCalculations && this.reportData.explosiveCalculations.length > 0) {
      content.push({
        text: 'Explosive Calculation Results',
        style: 'sectionHeader',
        pageBreak: 'before'
      });

      const explosivesTableBody: any[] = [
        [
          { text: 'Project Site', style: 'tableHeader' },
          { text: 'ANFO (kg)', style: 'tableHeader', alignment: 'right' },
          { text: 'Emulsion (kg)', style: 'tableHeader', alignment: 'right' },
          { text: 'Total (kg)', style: 'tableHeader', alignment: 'right' },
          { text: 'Filled Holes', style: 'tableHeader', alignment: 'right' },
          { text: 'Date', style: 'tableHeader' }
        ]
      ];

      // Include all explosive calculations (filtered if filters are applied)
      this.reportData.explosiveCalculations.forEach((calc: any) => {
        explosivesTableBody.push([
          { text: calc.projectSiteName || '', style: 'tableCell', fontSize: 7 },
          { text: calc.totalAnfo?.toFixed(2) || '0.00', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: calc.totalEmulsion?.toFixed(2) || '0.00', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: calc.totalExplosive?.toFixed(2) || '0.00', style: 'tableCell', alignment: 'right', fontSize: 7, bold: true },
          { text: calc.numberOfFilledHoles?.toString() || '0', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: calc.calculationDate ? new Date(calc.calculationDate).toLocaleDateString() : '', style: 'tableCell', fontSize: 7 }
        ]);
      });

      content.push({
        table: {
          headerRows: 1,
          widths: ['*', 'auto', 'auto', 'auto', 'auto', 'auto'],
          body: explosivesTableBody
        },
        layout: {
          hLineWidth: () => 0.5,
          vLineWidth: () => 0.5,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      });
    }

    // All Drill Points Section
    if (this.reportData.allDrillPoints && this.reportData.allDrillPoints.length > 0) {
      content.push({
        text: 'All Drill Points - Detailed Data',
        style: 'sectionHeader',
        pageBreak: 'before'
      });

      const drillPointsTableBody: any[] = [
        [
          { text: 'Point ID', style: 'tableHeader' },
          { text: 'X', style: 'tableHeader', alignment: 'right' },
          { text: 'Y', style: 'tableHeader', alignment: 'right' },
          { text: 'Depth (m)', style: 'tableHeader', alignment: 'right' },
          { text: 'Diameter (mm)', style: 'tableHeader', alignment: 'right' },
          { text: 'Project Site', style: 'tableHeader' },
          { text: 'Created', style: 'tableHeader' }
        ]
      ];

      // Include all drill points (filtered if filters are applied)
      this.reportData.allDrillPoints.forEach((point: any) => {
        drillPointsTableBody.push([
          { text: point.pointId?.toString() || '', style: 'tableCell', fontSize: 7 },
          { text: point.x?.toFixed(2) || '0.00', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: point.y?.toFixed(2) || '0.00', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: point.depth?.toFixed(2) || '0.00', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: point.diameter?.toFixed(2) || '0.00', style: 'tableCell', alignment: 'right', fontSize: 7 },
          { text: point.projectSiteName || '', style: 'tableCell', fontSize: 7 },
          { text: point.createdDate ? new Date(point.createdDate).toLocaleDateString() : '', style: 'tableCell', fontSize: 7 }
        ]);
      });

      content.push({
        table: {
          headerRows: 1,
          widths: ['auto', 'auto', 'auto', 'auto', 'auto', '*', 'auto'],
          body: drillPointsTableBody
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
