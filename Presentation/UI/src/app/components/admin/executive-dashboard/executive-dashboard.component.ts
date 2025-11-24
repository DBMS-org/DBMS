import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ReportService, ReportMetadata, ReportFilter } from '../../../services/report.service';

@Component({
  selector: 'app-executive-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './executive-dashboard.component.html',
  styleUrls: ['./executive-dashboard.component.scss']
})
export class ExecutiveDashboardComponent implements OnInit {
  reports: ReportMetadata[] = [];
  filteredReports: ReportMetadata[] = [];
  loading = false;
  error: string | null = null;

  // Filter states
  searchQuery = '';
  selectedCategory = '';
  categories: string[] = [];

  // Date range filter
  showDateFilter = false;
  startDate: string = '';
  endDate: string = '';
  selectedRegion = '';

  constructor(
    private reportService: ReportService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadAvailableReports();
    this.setDefaultDateRange();
  }

  setDefaultDateRange() {
    const today = new Date();
    const thirtyDaysAgo = new Date();
    thirtyDaysAgo.setDate(today.getDate() - 30);

    this.endDate = today.toISOString().split('T')[0];
    this.startDate = thirtyDaysAgo.toISOString().split('T')[0];
  }

  loadAvailableReports() {
    this.loading = true;
    this.error = null;

    this.reportService.getAvailableReports().subscribe({
      next: (response) => {
        console.log('API Response:', response);

        // Handle both wrapped ApiResponse and direct array response
        let reportsData: ReportMetadata[] = [];

        if (Array.isArray(response)) {
          // Response is a direct array
          reportsData = response as any;
        } else if (response.success && response.data) {
          // Response is wrapped in ApiResponse
          reportsData = response.data;
        } else if ((response as any).length !== undefined) {
          // Response might be an array-like object
          reportsData = response as any;
        }

        if (reportsData.length > 0) {
          this.reports = reportsData;
          this.filteredReports = [...this.reports];
          this.extractCategories();
        } else {
          this.error = 'No reports available';
          console.warn('Response received but no data:', response);
        }
        this.loading = false;
      },
      error: (error) => {
        if (error.status === 401) {
          this.error = 'Authentication required. Please log in.';
        } else if (error.status === 403) {
          this.error = 'Access denied. Admin or General Manager role required.';
        } else if (error.status === 0) {
          this.error = 'Cannot connect to server. Please ensure the backend is running.';
        } else {
          this.error = `Failed to load reports: ${error.message || error.statusText}`;
        }
        console.error('Error loading reports:', error);
        this.loading = false;
      }
    });
  }

  extractCategories() {
    const categorySet = new Set(this.reports.map(r => r.category));
    this.categories = Array.from(categorySet);
  }

  onSearch() {
    this.applyFilters();
  }

  onCategoryFilter() {
    this.applyFilters();
  }

  applyFilters() {
    let filtered = [...this.reports];

    if (this.searchQuery.trim()) {
      const query = this.searchQuery.toLowerCase();
      filtered = filtered.filter(r =>
        r.name.toLowerCase().includes(query) ||
        r.description.toLowerCase().includes(query) ||
        r.category.toLowerCase().includes(query)
      );
    }

    if (this.selectedCategory) {
      filtered = filtered.filter(r => r.category === this.selectedCategory);
    }

    this.filteredReports = filtered;
  }

  generateReport(report: ReportMetadata) {
    // Navigate to the appropriate report viewer based on report type
    switch (report.id) {
      // REMOVED IN PHASE 1: case 'fleet-management'
      // REMOVED IN PHASE 1: case 'inventory-status'
      // REMOVED IN PHASE 1: case 'operational-efficiency'

      case 'maintenance-performance':
        this.router.navigate(['/admin/maintenance-report'], {
          queryParams: {
            startDate: this.startDate,
            endDate: this.endDate,
            regionId: this.selectedRegion || ''
          }
        });
        break;
      // REMOVED: Regional Performance Report
      // case 'regional-performance':
      //   this.router.navigate(['/admin/regional-performance-report'], {
      //     queryParams: {
      //       startDate: this.startDate,
      //       endDate: this.endDate,
      //       regionId: this.selectedRegion || ''
      //     }
      //   });
      //   break;
      case 'drilling-operations':
        this.router.navigate(['/admin/drilling-operations-report'], {
          queryParams: {
            startDate: this.startDate,
            endDate: this.endDate,
            regionId: this.selectedRegion || ''
          }
        });
        break;
      case 'explosive-workflow':
        this.router.navigate(['/admin/explosive-workflow-report'], {
          queryParams: {
            startDate: this.startDate,
            endDate: this.endDate,
            regionId: this.selectedRegion || ''
          }
        });
        break;
      case 'user-access':
        this.router.navigate(['/admin/user-access-report'], {
          queryParams: {
            startDate: this.startDate,
            endDate: this.endDate,
            regionId: this.selectedRegion || ''
          }
        });
        break;
      default:
        this.error = 'Unknown report type';
    }
  }

  getCategoryIcon(category: string): string {
    const iconMap: { [key: string]: string } = {
      'Operations': 'pi-cog',
      'Maintenance': 'pi-wrench',
      'Inventory': 'pi-box',
      'Analytics': 'pi-chart-bar',
      'Financial': 'pi-dollar'
    };
    return iconMap[category] || 'pi-file';
  }

  getCategoryColor(category: string): string {
    const colorMap: { [key: string]: string } = {
      'Operations': '#3B82F6', // Blue
      'Maintenance': '#F59E0B', // Orange
      'Inventory': '#10B981', // Green
      'Analytics': '#8B5CF6', // Purple
      'Financial': '#EF4444'  // Red
    };
    return colorMap[category] || '#6B7280';
  }

  toggleDateFilter() {
    this.showDateFilter = !this.showDateFilter;
  }

  resetFilters() {
    this.searchQuery = '';
    this.selectedCategory = '';
    this.selectedRegion = '';
    this.setDefaultDateRange();
    this.applyFilters();
  }
}
