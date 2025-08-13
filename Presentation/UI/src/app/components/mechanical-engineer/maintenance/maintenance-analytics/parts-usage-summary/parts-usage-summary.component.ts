import { Component, Input, OnInit, OnDestroy, OnChanges, ElementRef, ViewChild, ChangeDetectionStrategy, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Chart, ChartConfiguration, ChartType, registerables } from 'chart.js';
import { PartsUsageData, UsageMetrics } from '../../models/maintenance.models';
import { MaintenanceService } from '../../services/maintenance.service';

Chart.register(...registerables);

@Component({
  selector: 'app-parts-usage-summary',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    MatChipsModule,
    MatTooltipModule
  ],
  templateUrl: './parts-usage-summary.component.html',
  styleUrl: './parts-usage-summary.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PartsUsageSummaryComponent implements OnInit, OnDestroy, OnChanges {
  @Input() data: PartsUsageData[] | null = null;
  @ViewChild('chartCanvas', { static: true }) chartCanvas!: ElementRef<HTMLCanvasElement>;
  
  private chart: Chart | null = null;
  private maintenanceService = inject(MaintenanceService);

  // Make Math available in template
  protected readonly Math = Math;

  // Signals for filtering and sorting
  sortBy = signal<'usage' | 'cost' | 'name'>('usage');
  sortOrder = signal<'asc' | 'desc'>('desc');
  searchTerm = signal<string>('');
  selectedMachineTypes = signal<string[]>([]);
  usageMetrics = signal<UsageMetrics[] | null>(null);

  // Computed values for filtering and sorting
  availableMachineTypes = computed(() => {
    if (!this.data) return [];
    
    const allTypes = new Set<string>();
    this.data.forEach(part => {
      part.machineTypes.forEach(type => {
        if (type !== 'All Types') {
          allTypes.add(type);
        }
      });
    });
    
    return Array.from(allTypes).sort();
  });

  filteredAndSortedData = computed(() => {
    if (!this.data) return [];

    let filtered = [...this.data];

    // Apply search filter
    const search = this.searchTerm().toLowerCase();
    if (search) {
      filtered = filtered.filter(part => 
        part.partName.toLowerCase().includes(search) ||
        part.machineTypes.some(type => type.toLowerCase().includes(search))
      );
    }

    // Apply machine type filter
    const selectedTypes = this.selectedMachineTypes();
    if (selectedTypes.length > 0) {
      filtered = filtered.filter(part =>
        part.machineTypes.some(type => 
          selectedTypes.includes(type) || part.machineTypes.includes('All Types')
        )
      );
    }

    // Apply sorting
    const sorted = filtered.sort((a, b) => {
      let comparison = 0;
      
      switch (this.sortBy()) {
        case 'usage':
          comparison = a.usageCount - b.usageCount;
          break;
        case 'cost':
          comparison = a.totalCost - b.totalCost;
          break;
        case 'name':
          comparison = a.partName.localeCompare(b.partName);
          break;
      }

      return this.sortOrder() === 'asc' ? comparison : -comparison;
    });

    return sorted;
  });

  // Legacy computed for backward compatibility
  sortedData = computed(() => this.filteredAndSortedData());

  // Usage metrics computed values
  totalEngineHours = computed(() => {
    const metrics = this.usageMetrics();
    return metrics ? metrics.reduce((sum, m) => sum + m.engineHours, 0) : 0;
  });

  totalServiceHours = computed(() => {
    const metrics = this.usageMetrics();
    return metrics ? metrics.reduce((sum, m) => sum + m.serviceHours, 0) : 0;
  });

  totalIdleHours = computed(() => {
    const metrics = this.usageMetrics();
    return metrics ? metrics.reduce((sum, m) => sum + m.idleHours, 0) : 0;
  });

  utilizationRate = computed(() => {
    const totalEngine = this.totalEngineHours();
    const totalIdle = this.totalIdleHours();
    const totalTime = totalEngine + totalIdle;
    
    return totalTime > 0 ? (totalEngine / totalTime) * 100 : 0;
  });

  ngOnInit() {
    if (this.data && this.data.length > 0) {
      this.createChart();
    }
    this.loadUsageMetrics();
  }

  ngOnDestroy() {
    if (this.chart) {
      this.chart.destroy();
    }
  }

  ngOnChanges() {
    if (this.data && this.chartCanvas) {
      this.updateChart();
    }
  }

  setSortBy(value: 'usage' | 'cost' | 'name') {
    this.sortBy.set(value);
    this.updateChart();
  }

  toggleSortOrder() {
    this.sortOrder.set(this.sortOrder() === 'asc' ? 'desc' : 'asc');
    this.updateChart();
  }

  setSearchTerm(term: string) {
    this.searchTerm.set(term);
    this.updateChart();
  }

  toggleMachineTypeFilter(machineType: string) {
    const current = this.selectedMachineTypes();
    if (current.includes(machineType)) {
      this.selectedMachineTypes.set(current.filter(type => type !== machineType));
    } else {
      this.selectedMachineTypes.set([...current, machineType]);
    }
    this.updateChart();
  }

  clearMachineTypeFilters() {
    this.selectedMachineTypes.set([]);
    this.updateChart();
  }

  clearAllFilters() {
    this.searchTerm.set('');
    this.selectedMachineTypes.set([]);
    this.updateChart();
  }

  private async loadUsageMetrics() {
    try {
      const metrics = await this.maintenanceService.getUsageMetrics().toPromise();
      this.usageMetrics.set(metrics || []);
    } catch (error) {
      console.error('Error loading usage metrics:', error);
      this.usageMetrics.set([]);
    }
  }

  private createChart() {
    if (!this.filteredAndSortedData() || this.filteredAndSortedData().length === 0 || !this.chartCanvas) return;

    const ctx = this.chartCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    // Show top 10 parts for the chart
    const topParts = this.filteredAndSortedData().slice(0, 10);
    const labels = topParts.map(item => item.partName);
    const usageData = topParts.map(item => item.usageCount);
    const costData = topParts.map(item => item.totalCost);

    const config: ChartConfiguration = {
      type: 'bar' as ChartType,
      data: {
        labels: labels,
        datasets: [
          {
            label: 'Usage Count',
            data: usageData,
            backgroundColor: '#2196f3',
            borderColor: '#1976d2',
            borderWidth: 1,
            yAxisID: 'y'
          },
          {
            label: 'Total Cost ($)',
            data: costData,
            backgroundColor: '#4caf50',
            borderColor: '#388e3c',
            borderWidth: 1,
            yAxisID: 'y'
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        indexAxis: 'y',
        interaction: {
          mode: 'index',
          intersect: false,
        },
        plugins: {
          legend: {
            display: true,
            position: 'top'
          },
          tooltip: {
            callbacks: {
              label: (context) => {
                const partData = topParts[context.dataIndex];
                const avgCost = partData.totalCost / partData.usageCount;
                
                if (context.datasetIndex === 0) {
                  return `Usage: ${context.parsed.x} times`;
                } else {
                  return [
                    `Total Cost: $${partData.totalCost.toFixed(2)}`,
                    `Avg Cost: $${avgCost.toFixed(2)}`
                  ];
                }
              }
            }
          }
        },
        scales: {
          x: {
            beginAtZero: true,
            title: {
              display: true,
              text: 'Count / Cost ($)'
            }
          },
          y: {
            title: {
              display: true,
              text: 'Parts'
            }
          }
        }
      }
    };

    this.chart = new Chart(ctx, config);
  }

  private updateChart() {
    if (!this.chart || !this.filteredAndSortedData() || this.filteredAndSortedData().length === 0) return;

    const topParts = this.filteredAndSortedData().slice(0, 10);
    const labels = topParts.map(item => item.partName);
    const usageData = topParts.map(item => item.usageCount);
    const costData = topParts.map(item => item.totalCost);

    this.chart.data.labels = labels;
    this.chart.data.datasets[0].data = usageData;
    this.chart.data.datasets[1].data = costData;
    this.chart.update();
  }
}