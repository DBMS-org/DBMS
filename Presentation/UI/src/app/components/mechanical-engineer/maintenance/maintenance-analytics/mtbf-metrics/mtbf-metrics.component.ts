import { Component, input, OnInit, OnDestroy, ElementRef, ViewChild, ChangeDetectionStrategy, effect, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Chart, ChartConfiguration, ChartType, registerables } from 'chart.js';
import { MTBFMetrics } from '../../models/maintenance.models';

Chart.register(...registerables);

@Component({
  selector: 'app-mtbf-metrics',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatProgressSpinnerModule],
  templateUrl: './mtbf-metrics.component.html',
  styleUrl: './mtbf-metrics.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MTBFMetricsComponent implements OnInit, OnDestroy {
  data = input<MTBFMetrics[] | null>(null);
  @ViewChild('chartCanvas', { static: false }) chartCanvas!: ElementRef<HTMLCanvasElement>;

  private chart: Chart | null = null;
  private isChartInitialized = signal(false);

  // Computed values for summary statistics
  sortedData = signal<MTBFMetrics[]>([]);
  averageMTBF = signal(0);
  bestPerformer = signal<MTBFMetrics | null>(null);
  totalFailures = signal(0);
  maxMTBF = signal(0);

  constructor() {
    // Effect to update computed values when data changes
    effect(() => {
      const currentData = this.data();
      if (currentData && currentData.length > 0) {
        this.updateComputedValues(currentData);
        if (this.chartCanvas && !this.isChartInitialized()) {
          setTimeout(() => this.createChart(), 0);
        } else if (this.chart) {
          this.updateChart();
        }
      }
    });
  }

  ngOnInit() {
    // Initial setup is handled by the effect
  }

  ngOnDestroy() {
    if (this.chart) {
      this.chart.destroy();
    }
  }

  private updateComputedValues(data: MTBFMetrics[]) {
    // Sort data by MTBF hours descending
    const sorted = [...data].sort((a, b) => b.mtbfHours - a.mtbfHours);
    this.sortedData.set(sorted);

    // Calculate average MTBF
    const avgMTBF = Math.round(data.reduce((sum, item) => sum + item.mtbfHours, 0) / data.length);
    this.averageMTBF.set(avgMTBF);

    // Find best performer
    this.bestPerformer.set(sorted[0] || null);

    // Calculate total failures
    const totalFail = data.reduce((sum, item) => sum + item.failureCount, 0);
    this.totalFailures.set(totalFail);

    // Find max MTBF for bar width calculation
    const maxMTBF = Math.max(...data.map(item => item.mtbfHours));
    this.maxMTBF.set(maxMTBF);
  }

  getBarWidth(mtbfHours: number): number {
    const max = this.maxMTBF();
    return max > 0 ? (mtbfHours / max) * 100 : 0;
  }

  private createChart() {
    const currentData = this.data();
    if (!currentData || currentData.length === 0 || !this.chartCanvas) return;

    const ctx = this.chartCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    const labels = currentData.map(item => item.machineType);
    const mtbfData = currentData.map(item => item.mtbfHours);
    const failureData = currentData.map(item => item.failureCount);

    // Create color gradients based on MTBF performance
    const avgMTBF = this.averageMTBF();
    const backgroundColors = mtbfData.map(mtbf =>
      mtbf > avgMTBF ? '#4caf50' : mtbf > avgMTBF * 0.8 ? '#2196f3' : '#ff9800'
    );
    const borderColors = mtbfData.map(mtbf =>
      mtbf > avgMTBF ? '#388e3c' : mtbf > avgMTBF * 0.8 ? '#1976d2' : '#f57c00'
    );

    const config: ChartConfiguration = {
      type: 'bar' as ChartType,
      data: {
        labels: labels,
        datasets: [
          {
            label: 'MTBF (Hours)',
            data: mtbfData,
            backgroundColor: backgroundColors,
            borderColor: borderColors,
            borderWidth: 2,
            yAxisID: 'y',
            borderRadius: 4,
            borderSkipped: false
          },
          {
            label: 'Failure Count',
            data: failureData,
            backgroundColor: 'rgba(255, 152, 0, 0.7)',
            borderColor: '#f57c00',
            borderWidth: 2,
            yAxisID: 'y1',
            type: 'line' as ChartType,
            pointBackgroundColor: '#f57c00',
            pointBorderColor: '#fff',
            pointBorderWidth: 2,
            pointRadius: 5,
            tension: 0.3
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        interaction: {
          mode: 'index',
          intersect: false,
        },
        plugins: {
          legend: {
            position: 'top',
            labels: {
              usePointStyle: true,
              padding: 20,
              font: {
                size: 12
              }
            }
          },
          tooltip: {
            backgroundColor: 'rgba(0, 0, 0, 0.8)',
            titleColor: '#fff',
            bodyColor: '#fff',
            borderColor: '#ddd',
            borderWidth: 1,
            callbacks: {
              title: (context) => {
                return `${context[0].label}`;
              },
              label: (context) => {
                if (context.datasetIndex === 0) {
                  const avgMTBF = this.averageMTBF();
                  const performance = context.parsed.y > avgMTBF ? 'Above Average' : 'Below Average';
                  return `MTBF: ${context.parsed.y} hours (${performance})`;
                } else {
                  return `Failures: ${context.parsed.y} in 6 months`;
                }
              },
              afterBody: (context) => {
                const dataIndex = context[0].dataIndex;
                const currentData = this.data();
                if (currentData && currentData[dataIndex]) {
                  const metric = currentData[dataIndex];
                  return `Period: ${metric.periodMonths} months`;
                }
                return '';
              }
            }
          }
        },
        scales: {
          x: {
            display: true,
            title: {
              display: true,
              text: 'Machine Type',
              font: {
                size: 12,
                weight: 'bold'
              }
            },
            ticks: {
              maxRotation: 45,
              minRotation: 0
            }
          },
          y: {
            type: 'linear',
            display: true,
            position: 'left',
            title: {
              display: true,
              text: 'MTBF (Hours)',
              font: {
                size: 12,
                weight: 'bold'
              }
            },
            beginAtZero: true,
            grid: {
              color: 'rgba(0, 0, 0, 0.1)'
            }
          },
          y1: {
            type: 'linear',
            display: true,
            position: 'right',
            title: {
              display: true,
              text: 'Failure Count',
              font: {
                size: 12,
                weight: 'bold'
              }
            },
            beginAtZero: true,
            grid: {
              drawOnChartArea: false,
            },
            ticks: {
              stepSize: 1
            }
          }
        }
      }
    };

    this.chart = new Chart(ctx, config);
    this.isChartInitialized.set(true);
  }

  private updateChart() {
    const currentData = this.data();
    if (!this.chart || !currentData || currentData.length === 0) return;

    const labels = currentData.map(item => item.machineType);
    const mtbfData = currentData.map(item => item.mtbfHours);
    const failureData = currentData.map(item => item.failureCount);

    // Update colors based on new data
    const avgMTBF = this.averageMTBF();
    const backgroundColors = mtbfData.map(mtbf =>
      mtbf > avgMTBF ? '#4caf50' : mtbf > avgMTBF * 0.8 ? '#2196f3' : '#ff9800'
    );
    const borderColors = mtbfData.map(mtbf =>
      mtbf > avgMTBF ? '#388e3c' : mtbf > avgMTBF * 0.8 ? '#1976d2' : '#f57c00'
    );

    this.chart.data.labels = labels;
    this.chart.data.datasets[0].data = mtbfData;
    this.chart.data.datasets[0].backgroundColor = backgroundColors;
    this.chart.data.datasets[0].borderColor = borderColors;
    this.chart.data.datasets[1].data = failureData;
    this.chart.update('none');
  }
}