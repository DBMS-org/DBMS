import { Component, Input, OnInit, OnDestroy, ElementRef, ViewChild, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, ChartConfiguration, ChartType, registerables } from 'chart.js';
import { UsageMetrics } from '../../models/maintenance.models';

Chart.register(...registerables);

@Component({
    selector: 'app-usage-metrics-chart',
    standalone: true,
    imports: [CommonModule],
    template: `
    <div class="chart-container">
      <canvas #chartCanvas></canvas>
    </div>
  `,
    styles: [`
    .chart-container {
      position: relative;
      height: 250px;
      width: 100%;
    }
    
    canvas {
      max-height: 100%;
      max-width: 100%;
    }
  `],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class UsageMetricsChartComponent implements OnInit, OnDestroy {
    @Input() data: UsageMetrics[] | null = null;
    @ViewChild('chartCanvas', { static: true }) chartCanvas!: ElementRef<HTMLCanvasElement>;

    private chart: Chart | null = null;

    ngOnInit() {
        if (this.data && this.data.length > 0) {
            this.createChart();
        }
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

    private createChart() {
        if (!this.data || this.data.length === 0 || !this.chartCanvas) return;

        const ctx = this.chartCanvas.nativeElement.getContext('2d');
        if (!ctx) return;

        // Aggregate data by totals
        const totalEngineHours = this.data.reduce((sum, m) => sum + m.engineHours, 0);
        const totalServiceHours = this.data.reduce((sum, m) => sum + m.serviceHours, 0);
        const totalIdleHours = this.data.reduce((sum, m) => sum + m.idleHours, 0);

        const config: ChartConfiguration = {
            type: 'doughnut' as ChartType,
            data: {
                labels: ['Engine Hours', 'Service Hours', 'Idle Hours'],
                datasets: [{
                    data: [totalEngineHours, totalServiceHours, totalIdleHours],
                    backgroundColor: [
                        '#2196f3', // Blue for engine hours
                        '#4caf50', // Green for service hours
                        '#ff9800'  // Orange for idle hours
                    ],
                    borderWidth: 2,
                    borderColor: '#ffffff'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            padding: 20,
                            usePointStyle: true
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: (context) => {
                                const total = totalEngineHours + totalServiceHours + totalIdleHours;
                                const percentage = total > 0 ? ((context.parsed as number) / total * 100).toFixed(1) : '0';
                                return `${context.label}: ${context.parsed} hours (${percentage}%)`;
                            }
                        }
                    }
                }
            }
        };

        this.chart = new Chart(ctx, config);
    }

    private updateChart() {
        if (!this.chart || !this.data || this.data.length === 0) return;

        const totalEngineHours = this.data.reduce((sum, m) => sum + m.engineHours, 0);
        const totalServiceHours = this.data.reduce((sum, m) => sum + m.serviceHours, 0);
        const totalIdleHours = this.data.reduce((sum, m) => sum + m.idleHours, 0);

        this.chart.data.datasets[0].data = [totalEngineHours, totalServiceHours, totalIdleHours];
        this.chart.update();
    }
}