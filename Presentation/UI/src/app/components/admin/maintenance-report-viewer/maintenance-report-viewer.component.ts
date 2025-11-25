import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportService, ReportFilter } from '../../../services/report.service';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { MultiSelectModule } from 'primeng/multiselect';
import { CardModule } from 'primeng/card';
import pdfMake from 'pdfmake/build/pdfmake';
import pdfFonts from 'pdfmake/build/vfs_fonts';

@Component({
  selector: 'app-maintenance-report-viewer',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    TabViewModule,
    ButtonModule,
    TagModule,
    CalendarModule,
    DropdownModule,
    MultiSelectModule,
    CardModule
  ],
  templateUrl: './maintenance-report-viewer.component.html',
  styleUrls: ['./maintenance-report-viewer.component.scss']
})
export class MaintenanceReportViewerComponent implements OnInit {
  reportData: any = null;
  originalReportData: any = null; // Store original unfiltered data
  loading = false;
  error: string | null = null;
  activeTab: string = 'allMaintenanceJobs';
  isFilterExpanded: boolean = false;

  // Filter parameters
  startDate: string | null = null;
  endDate: string | null = null;
  regionId: string | null = null;

  // Client-side filters
  filterStartDate: string = '';
  filterEndDate: string = '';
  filterStatuses: string[] = [];
  filterMaintenanceTypes: string[] = [];
  filterPriorities: string[] = [];
  filterMachines: string[] = [];
  filterEngineers: string[] = [];

  // Filter dropdown options
  statusOptions = [
    { label: 'Completed', value: 'Completed' },
    { label: 'In Progress', value: 'InProgress' },
    { label: 'Scheduled', value: 'Scheduled' },
    { label: 'Overdue', value: 'Overdue' },
    { label: 'Cancelled', value: 'Cancelled' }
  ];

  maintenanceTypeOptions = [
    { label: 'Emergency', value: 'Emergency' },
    { label: 'Corrective', value: 'Corrective' },
    { label: 'Preventive', value: 'Preventive' },
    { label: 'Inspection', value: 'Inspection' }
  ];

  priorityOptions = [
    { label: 'Critical', value: 'Critical' },
    { label: 'High', value: 'High' },
    { label: 'Medium', value: 'Medium' },
    { label: 'Low', value: 'Low' }
  ];

  machineOptions: any[] = [];
  engineerOptions: any[] = [];

  constructor(
    private reportService: ReportService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    // Configure pdfMake fonts
    (pdfMake as any).vfs = pdfFonts;
  }

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
          this.originalReportData = JSON.parse(JSON.stringify(response.data)); // Deep copy
          this.reportData = response.data;
        } else {
          this.originalReportData = JSON.parse(JSON.stringify(response)); // Deep copy
          this.reportData = response;
        }

        // Populate filter dropdown options from data
        this.populateFilterOptions();
      },
      error: (err) => {
        this.loading = false;
        this.error = 'Failed to load maintenance performance report. Please try again.';
        console.error('Error loading report:', err);
      }
    });
  }

  populateFilterOptions() {
    if (!this.originalReportData) return;

    // Extract unique machines
    if (this.originalReportData.allMaintenanceJobs) {
      const machines = new Set<string>();
      this.originalReportData.allMaintenanceJobs.forEach((job: any) => {
        if (job.machineName) {
          machines.add(job.machineName);
        }
      });
      this.machineOptions = Array.from(machines).map(machine => ({
        label: machine,
        value: machine
      })).sort((a, b) => a.label.localeCompare(b.label));
    }

    // Extract unique engineers
    if (this.originalReportData.allEngineers) {
      this.engineerOptions = this.originalReportData.allEngineers.map((eng: any) => ({
        label: eng.engineerName,
        value: eng.engineerName
      })).sort((a: any, b: any) => a.label.localeCompare(b.label));
    }
  }

  applyFilters() {
    if (!this.originalReportData) return;

    // Deep copy original data
    this.reportData = JSON.parse(JSON.stringify(this.originalReportData));

    // Apply date range filter
    if (this.filterStartDate || this.filterEndDate) {
      if (this.reportData.allMaintenanceJobs) {
        this.reportData.allMaintenanceJobs = this.reportData.allMaintenanceJobs.filter((job: any) => {
          const jobDate = new Date(job.scheduledDate);

          if (this.filterStartDate && this.filterEndDate) {
            const startDate = new Date(this.filterStartDate);
            const endDate = new Date(this.filterEndDate);
            return jobDate >= startDate && jobDate <= endDate;
          } else if (this.filterStartDate) {
            const startDate = new Date(this.filterStartDate);
            return jobDate >= startDate;
          } else if (this.filterEndDate) {
            const endDate = new Date(this.filterEndDate);
            return jobDate <= endDate;
          }

          return true;
        });
      }
    }

    // Apply status filter
    if (this.filterStatuses.length > 0) {
      if (this.reportData.allMaintenanceJobs) {
        this.reportData.allMaintenanceJobs = this.reportData.allMaintenanceJobs.filter((job: any) =>
          this.filterStatuses.includes(job.status)
        );
      }
      if (this.reportData.criticalIssues) {
        this.reportData.criticalIssues = this.reportData.criticalIssues.filter((issue: any) =>
          this.filterStatuses.includes(issue.status)
        );
      }
    }

    // Apply maintenance type filter
    if (this.filterMaintenanceTypes.length > 0) {
      if (this.reportData.allMaintenanceJobs) {
        this.reportData.allMaintenanceJobs = this.reportData.allMaintenanceJobs.filter((job: any) =>
          this.filterMaintenanceTypes.includes(job.maintenanceType)
        );
      }
    }

    // Apply priority filter
    if (this.filterPriorities.length > 0) {
      if (this.reportData.allMaintenanceJobs) {
        this.reportData.allMaintenanceJobs = this.reportData.allMaintenanceJobs.filter((job: any) =>
          this.filterPriorities.includes(job.priority)
        );
      }
      if (this.reportData.criticalIssues) {
        this.reportData.criticalIssues = this.reportData.criticalIssues.filter((issue: any) =>
          this.filterPriorities.includes(issue.priority)
        );
      }
    }

    // Apply machine filter
    if (this.filterMachines.length > 0) {
      if (this.reportData.allMaintenanceJobs) {
        this.reportData.allMaintenanceJobs = this.reportData.allMaintenanceJobs.filter((job: any) =>
          this.filterMachines.includes(job.machineName)
        );
      }
      if (this.reportData.machineServiceHistory) {
        this.reportData.machineServiceHistory = this.reportData.machineServiceHistory.filter((machine: any) =>
          this.filterMachines.includes(machine.machineName)
        );
      }
    }

    // Apply engineer filter
    if (this.filterEngineers.length > 0) {
      if (this.reportData.allEngineers) {
        this.reportData.allEngineers = this.reportData.allEngineers.filter((eng: any) =>
          this.filterEngineers.includes(eng.engineerName)
        );
      }
      if (this.reportData.topPerformingMechanics) {
        this.reportData.topPerformingMechanics = this.reportData.topPerformingMechanics.filter((mechanic: any) =>
          this.filterEngineers.includes(mechanic.mechanicName)
        );
      }
    }

    // Recalculate statistics based on filtered data
    this.recalculateStatistics();
  }

  recalculateStatistics() {
    if (!this.reportData || !this.reportData.allMaintenanceJobs) return;

    const jobs = this.reportData.allMaintenanceJobs;

    // Recalculate main statistics
    if (this.reportData.statistics) {
      this.reportData.statistics.totalMaintenanceRecords = jobs.length;
      this.reportData.statistics.completedMaintenance = jobs.filter((j: any) => j.status === 'Completed').length;
      this.reportData.statistics.inProgressMaintenance = jobs.filter((j: any) => j.status === 'InProgress').length;
      this.reportData.statistics.pendingMaintenance = jobs.filter((j: any) => j.status === 'Scheduled').length;

      const jobsWithHours = jobs.filter((j: any) => j.actualHours);
      this.reportData.statistics.averageCompletionTimeHours = jobsWithHours.length > 0
        ? jobsWithHours.reduce((sum: number, j: any) => sum + j.actualHours, 0) / jobsWithHours.length
        : 0;

      this.reportData.statistics.maintenanceCompletionRate = jobs.length > 0
        ? (this.reportData.statistics.completedMaintenance / jobs.length) * 100
        : 0;
    }

    // Recalculate maintenance type breakdown
    if (this.reportData.maintenanceTypeBreakdown) {
      const typeGroups = jobs.reduce((acc: any, job: any) => {
        if (!acc[job.maintenanceType]) {
          acc[job.maintenanceType] = [];
        }
        acc[job.maintenanceType].push(job);
        return acc;
      }, {});

      this.reportData.maintenanceTypeBreakdown = Object.keys(typeGroups).map(type => ({
        maintenanceType: type,
        count: typeGroups[type].length,
        percentage: jobs.length > 0 ? (typeGroups[type].length / jobs.length) * 100 : 0,
        averageCompletionTimeHours: typeGroups[type].filter((j: any) => j.actualHours).length > 0
          ? typeGroups[type].filter((j: any) => j.actualHours).reduce((sum: number, j: any) => sum + j.actualHours, 0) / typeGroups[type].filter((j: any) => j.actualHours).length
          : 0
      }));
    }
  }

  clearFilters() {
    this.filterStartDate = '';
    this.filterEndDate = '';
    this.filterStatuses = [];
    this.filterMaintenanceTypes = [];
    this.filterPriorities = [];
    this.filterMachines = [];
    this.filterEngineers = [];

    // Reset to original data
    if (this.originalReportData) {
      this.reportData = JSON.parse(JSON.stringify(this.originalReportData));
    }
  }

  goBack() {
    this.router.navigate(['/admin/executive-dashboard']);
  }

  printReport() {
    window.print();
  }

  exportToPdf() {
    if (!this.reportData) return;

    const docDefinition: any = {
      pageOrientation: 'portrait',
      pageSize: 'A4',
      pageMargins: [40, 80, 40, 60],

      // Header for every page
      header: (currentPage: number, pageCount: number) => {
        return {
          margin: [40, 30, 40, 20],
          columns: [
            {
              stack: [
                { text: 'DBMS - Detonation and Blasting Management System', style: 'headerSystem', alignment: 'left' },
                { text: 'Maintenance Performance Report', style: 'headerReport', alignment: 'left' }
              ]
            },
            {
              text: `Page ${currentPage} of ${pageCount}`,
              alignment: 'right',
              fontSize: 9,
              color: '#6b7280',
              margin: [0, 10, 0, 0]
            }
          ]
        };
      },

      // Footer for every page
      footer: (currentPage: number, pageCount: number) => {
        return {
          margin: [40, 20, 40, 20],
          columns: [
            { text: 'Generated by DBMS', fontSize: 8, color: '#6b7280' },
            { text: `© ${new Date().getFullYear()} All Rights Reserved`, fontSize: 8, color: '#6b7280', alignment: 'right' }
          ]
        };
      },

      content: [],

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
        headerSystem: {
          fontSize: 11,
          bold: true,
          color: '#667eea',
          margin: [0, 0, 0, 4]
        },
        headerReport: {
          fontSize: 9,
          color: '#6b7280'
        },
        reportTitle: {
          fontSize: 20,
          bold: true,
          color: '#1f2937',
          margin: [0, 0, 0, 8]
        },
        reportMeta: {
          fontSize: 10,
          color: '#6b7280',
          margin: [0, 0, 0, 4]
        },
        sectionTitle: {
          fontSize: 14,
          bold: true,
          color: '#1f2937',
          margin: [0, 20, 0, 12]
        },
        tableHeader: {
          bold: true,
          fontSize: 10,
          color: 'white',
          fillColor: '#667eea'
        },
        tableCell: {
          fontSize: 9,
          color: '#374151'
        },
        statCard: {
          fontSize: 9,
          margin: [0, 4, 0, 4]
        },
        statValue: {
          fontSize: 16,
          bold: true,
          color: '#1f2937'
        },
        statLabel: {
          fontSize: 8,
          color: '#6b7280'
        }
      }
    };

    // ========== ENHANCED COVER PAGE ==========
    docDefinition.content.push({
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
          text: 'MAINTENANCE PERFORMANCE REPORT',
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
                { text: 'Comprehensive Maintenance Operations Analysis', style: 'coverDetailValue', border: [false, false, false, false] }
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
                    { text: (this.reportData.statistics?.totalMaintenanceRecords || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Total Records', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#eff6ff',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.reportData.statistics?.completedMaintenance || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Completed', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#f0fdf4',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.reportData.statistics?.inProgressMaintenance || 0).toString(), style: 'coverMetricValue' },
                    { text: 'In Progress', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#fef3c7',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                }
              ],
              [
                {
                  stack: [
                    { text: ((this.reportData.statistics?.averageCompletionTimeHours || 0).toFixed(1)) + ' hrs', style: 'coverMetricValue' },
                    { text: 'Avg Completion Time', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#f3f4f6',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: ((this.reportData.statistics?.maintenanceCompletionRate || 0).toFixed(1)) + '%', style: 'coverMetricValue' },
                    { text: 'Completion Rate', style: 'coverMetricLabel' }
                  ],
                  fillColor: '#fce7f3',
                  margin: [8, 10, 8, 10],
                  border: [false, false, false, false]
                },
                {
                  stack: [
                    { text: (this.reportData.statistics?.pendingMaintenance || 0).toString(), style: 'coverMetricValue' },
                    { text: 'Pending Tasks', style: 'coverMetricLabel' }
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
                      text: 'This report contains confidential information about maintenance operations and equipment performance. It is intended solely for authorized maintenance personnel and management. Unauthorized distribution, copying, or disclosure of this information is strictly prohibited.',
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

    // Statistics Cards
    if (this.reportData.statistics) {
      docDefinition.content.push({
        text: 'Key Statistics',
        style: 'sectionTitle'
      });

      const statsTable = {
        table: {
          widths: ['*', '*', '*'],
          body: [
            [
              { stack: [
                { text: this.reportData.statistics.totalMaintenanceRecords || 0, style: 'statValue', alignment: 'center' },
                { text: 'Total Maintenance Records', style: 'statLabel', alignment: 'center' }
              ], margin: [0, 8, 0, 8] },
              { stack: [
                { text: this.reportData.statistics.completedMaintenance || 0, style: 'statValue', alignment: 'center' },
                { text: 'Completed Maintenance', style: 'statLabel', alignment: 'center' }
              ], margin: [0, 8, 0, 8] },
              { stack: [
                { text: this.reportData.statistics.inProgressMaintenance || 0, style: 'statValue', alignment: 'center' },
                { text: 'In Progress', style: 'statLabel', alignment: 'center' }
              ], margin: [0, 8, 0, 8] }
            ],
            [
              { stack: [
                { text: this.reportData.statistics.pendingMaintenance || 0, style: 'statValue', alignment: 'center' },
                { text: 'Pending Maintenance', style: 'statLabel', alignment: 'center' }
              ], margin: [0, 8, 0, 8] },
              { stack: [
                { text: `${this.reportData.statistics.averageCompletionTimeHours?.toFixed(1) || 0}h`, style: 'statValue', alignment: 'center' },
                { text: 'Avg Completion Time', style: 'statLabel', alignment: 'center' }
              ], margin: [0, 8, 0, 8] },
              { stack: [
                { text: `${this.reportData.statistics.maintenanceCompletionRate?.toFixed(1) || 0}%`, style: 'statValue', alignment: 'center' },
                { text: 'Completion Rate', style: 'statLabel', alignment: 'center' }
              ], margin: [0, 8, 0, 8] }
            ]
          ]
        },
        layout: {
          fillColor: (rowIndex: number) => rowIndex % 2 === 0 ? '#f9fafb' : null,
          hLineWidth: () => 1,
          vLineWidth: () => 1,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      };

      docDefinition.content.push(statsTable);
    }

    // Maintenance Type Breakdown
    if (this.reportData.maintenanceTypeBreakdown?.length > 0) {
      docDefinition.content.push({
        text: 'Maintenance Type Breakdown',
        style: 'sectionTitle'
      });

      const typeTableBody: any[] = [[
        { text: 'Type', style: 'tableHeader' },
        { text: 'Count', style: 'tableHeader', alignment: 'center' },
        { text: 'Percentage', style: 'tableHeader', alignment: 'center' },
        { text: 'Avg Time (hrs)', style: 'tableHeader', alignment: 'center' }
      ]];

      this.reportData.maintenanceTypeBreakdown.forEach((type: any) => {
        typeTableBody.push([
          { text: type.maintenanceType, style: 'tableCell' },
          { text: type.count.toString(), style: 'tableCell', alignment: 'center' },
          { text: `${type.percentage?.toFixed(1)}%`, style: 'tableCell', alignment: 'center' },
          { text: type.averageCompletionTimeHours?.toFixed(1), style: 'tableCell', alignment: 'center' }
        ]);
      });

      docDefinition.content.push({
        table: {
          headerRows: 1,
          widths: ['*', 'auto', 'auto', 'auto'],
          body: typeTableBody
        },
        layout: {
          fillColor: (rowIndex: number) => rowIndex === 0 ? '#667eea' : (rowIndex % 2 === 0 ? '#f9fafb' : null),
          hLineWidth: () => 1,
          vLineWidth: () => 1,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      });
    }

    // Top Performing Mechanics
    if (this.reportData.topPerformingMechanics?.length > 0) {
      docDefinition.content.push({
        text: 'Top Performing Mechanics',
        style: 'sectionTitle',
        pageBreak: 'before'
      });

      const mechanicsTableBody: any[] = [[
        { text: 'Rank', style: 'tableHeader', alignment: 'center' },
        { text: 'Mechanic Name', style: 'tableHeader' },
        { text: 'Tasks', style: 'tableHeader', alignment: 'center' },
        { text: 'Avg Time', style: 'tableHeader', alignment: 'center' },
        { text: 'Rate', style: 'tableHeader', alignment: 'center' },
        { text: 'Machines', style: 'tableHeader', alignment: 'center' }
      ]];

      this.reportData.topPerformingMechanics.forEach((mechanic: any, index: number) => {
        mechanicsTableBody.push([
          { text: `#${index + 1}`, style: 'tableCell', alignment: 'center', bold: index < 3 },
          { text: mechanic.mechanicName, style: 'tableCell' },
          { text: mechanic.tasksCompleted.toString(), style: 'tableCell', alignment: 'center' },
          { text: `${mechanic.averageCompletionTimeHours?.toFixed(1)}h`, style: 'tableCell', alignment: 'center' },
          { text: `${mechanic.completionRate?.toFixed(1)}%`, style: 'tableCell', alignment: 'center' },
          { text: mechanic.machinesServiced.toString(), style: 'tableCell', alignment: 'center' }
        ]);
      });

      docDefinition.content.push({
        table: {
          headerRows: 1,
          widths: ['auto', '*', 'auto', 'auto', 'auto', 'auto'],
          body: mechanicsTableBody
        },
        layout: {
          fillColor: (rowIndex: number) => rowIndex === 0 ? '#667eea' : (rowIndex % 2 === 0 ? '#f9fafb' : null),
          hLineWidth: () => 1,
          vLineWidth: () => 1,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      });
    }

    // Critical Issues
    if (this.reportData.criticalIssues?.length > 0) {
      docDefinition.content.push({
        text: 'Critical Issues & Overdue Maintenance',
        style: 'sectionTitle',
        pageBreak: 'before'
      });

      const issuesTableBody: any[] = [[
        { text: 'Machine', style: 'tableHeader' },
        { text: 'Issue', style: 'tableHeader' },
        { text: 'Priority', style: 'tableHeader' },
        { text: 'Reported', style: 'tableHeader' },
        { text: 'Status', style: 'tableHeader' },
        { text: 'Days', style: 'tableHeader', alignment: 'center' }
      ]];

      this.reportData.criticalIssues.forEach((issue: any) => {
        issuesTableBody.push([
          { text: issue.machineIdentifier, style: 'tableCell', fontSize: 8 },
          { text: issue.issueDescription.substring(0, 50) + (issue.issueDescription.length > 50 ? '...' : ''), style: 'tableCell', fontSize: 8 },
          { text: issue.priority, style: 'tableCell', fontSize: 8 },
          { text: new Date(issue.reportedDate).toLocaleDateString(), style: 'tableCell', fontSize: 8 },
          { text: issue.status, style: 'tableCell', fontSize: 8 },
          { text: `${issue.daysOpen} days`, style: 'tableCell', alignment: 'center', fontSize: 8 }
        ]);
      });

      docDefinition.content.push({
        table: {
          headerRows: 1,
          widths: ['auto', '*', 'auto', 'auto', 'auto', 'auto'],
          body: issuesTableBody
        },
        layout: {
          fillColor: (rowIndex: number) => rowIndex === 0 ? '#667eea' : (rowIndex % 2 === 0 ? '#f9fafb' : null),
          hLineWidth: () => 1,
          vLineWidth: () => 1,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      });
    }

    // Regional Breakdown
    if (this.reportData.regionalBreakdown?.length > 0) {
      docDefinition.content.push({
        text: 'Regional Maintenance Performance',
        style: 'sectionTitle'
      });

      const regionTableBody: any[] = [[
        { text: 'Region', style: 'tableHeader' },
        { text: 'Total', style: 'tableHeader', alignment: 'center' },
        { text: 'Completed', style: 'tableHeader', alignment: 'center' },
        { text: 'Completion Rate', style: 'tableHeader', alignment: 'center' }
      ]];

      this.reportData.regionalBreakdown.forEach((region: any) => {
        regionTableBody.push([
          { text: region.region, style: 'tableCell', bold: true },
          { text: region.totalMaintenance.toString(), style: 'tableCell', alignment: 'center' },
          { text: region.completedMaintenance.toString(), style: 'tableCell', alignment: 'center' },
          { text: `${region.completionRate?.toFixed(1)}%`, style: 'tableCell', alignment: 'center' }
        ]);
      });

      docDefinition.content.push({
        table: {
          headerRows: 1,
          widths: ['*', 'auto', 'auto', 'auto'],
          body: regionTableBody
        },
        layout: {
          fillColor: (rowIndex: number) => rowIndex === 0 ? '#667eea' : (rowIndex % 2 === 0 ? '#f9fafb' : null),
          hLineWidth: () => 1,
          vLineWidth: () => 1,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      });
    }

    // All Maintenance Jobs (only visible data from current tab)
    if (this.activeTab === 'allMaintenanceJobs' && this.reportData.allMaintenanceJobs?.length > 0) {
      docDefinition.content.push({
        text: 'All Maintenance Jobs (Current Page Data)',
        style: 'sectionTitle',
        pageBreak: 'before'
      });

      const jobsTableBody: any[] = [[
        { text: 'ID', style: 'tableHeader' },
        { text: 'Title', style: 'tableHeader' },
        { text: 'Machine', style: 'tableHeader' },
        { text: 'Type', style: 'tableHeader' },
        { text: 'Status', style: 'tableHeader' }
      ]];

      // Only include first 10 jobs (what's visible on current page)
      const visibleJobs = this.reportData.allMaintenanceJobs.slice(0, 10);
      visibleJobs.forEach((job: any) => {
        jobsTableBody.push([
          { text: job.maintenanceJobId.toString(), style: 'tableCell', fontSize: 8 },
          { text: job.jobTitle.substring(0, 30), style: 'tableCell', fontSize: 8 },
          { text: job.machineName, style: 'tableCell', fontSize: 8 },
          { text: job.maintenanceType, style: 'tableCell', fontSize: 8 },
          { text: job.status, style: 'tableCell', fontSize: 8 }
        ]);
      });

      docDefinition.content.push({
        table: {
          headerRows: 1,
          widths: ['auto', '*', 'auto', 'auto', 'auto'],
          body: jobsTableBody
        },
        layout: {
          fillColor: (rowIndex: number) => rowIndex === 0 ? '#667eea' : (rowIndex % 2 === 0 ? '#f9fafb' : null),
          hLineWidth: () => 1,
          vLineWidth: () => 1,
          hLineColor: () => '#e5e7eb',
          vLineColor: () => '#e5e7eb'
        }
      });
    }

    // Generate PDF
    const pdfDocGenerator = pdfMake.createPdf(docDefinition);

    // Download the PDF
    const fileName = `Maintenance_Performance_Report_${new Date().toISOString().split('T')[0]}.pdf`;
    pdfDocGenerator.download(fileName);

    // Also open the PDF after downloading
    setTimeout(() => {
      pdfDocGenerator.open();
    }, 100);
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
