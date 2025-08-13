import { Component, Input, OnInit, OnDestroy, OnChanges, ElementRef, ViewChild, ChangeDetectionStrategy, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { Chart, ChartConfiguration, ChartType, registerables } from 'chart.js';
import { ServiceComplianceData } from '../../models/maintenance.models';

Chart.register(...registerables);

@Component({
  selector: 'app-service-compliance-chart',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  template: `
    <div class="service-compliance-chart">
      @if (hasInsufficientData()) {
        <div class="insufficient-data">
          <mat-icon>info_outline</mat-icon>
          <h4>Insufficient Data</h4>
          <p>{{ insufficientDataMessage() }}</p>
          <div class="data-requirements">
            <h5>To display service compliance:</h5>
            <ul>
              <li>At least 5 completed or overdue maintenance jobs</li>
              <li>Data from the last 30 days</li>
              <li>Mix of completed and overdue jobs for meaningful analysis</li>
            </ul>
          </div>
        </div>
      } @else {
        <div class="chart-container">
          <div class="compliance-summary">
            <div class="compliance-percentage">
              <span class="percentage-value" [class.good]="isGoodCompliance()" [class.poor]="isPoorCompliance()">
                {{ data?.percentage }}%
              </span>
              <span class="percentage-label">Service Compliance Rate</span>
            </div>
            <div class="compliance-breakdown">
              <div class="breakdown-item on-time">
                <div class="count">{{ data?.onTime }}</div>
                <div class="label">On Time</div>
              </div>
              <div class="breakdown-item overdue">
                <div class="count">{{ data?.overdue }}</div>
                <div class="label">Overdue</div>
              </div>
            </div>
          </div>
          <div class="chart-wrapper">
            <canvas #chartCanvas></canvas>
          </div>
          <div class="compliance-insights">
            <div class="insight" [ngClass]="getComplianceLevel()">
              <mat-icon>{{ getComplianceIcon() }}</mat-icon>
              <span>{{ getComplianceMessage() }}</span>
            </div>
          </div>
        </div>
      }
    </div>
  `,
  styles: [`
    .service-compliance-chart {
      height: 100%;
      display: flex;
      flex-direction: column;
    }

    .insufficient-data {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      height: 100%;
      text-align: center;
      padding: 24px;
      color: #666;
    }

    .insufficient-data mat-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
      margin-bottom: 16px;
      color: #ff9800;
    }

    .insufficient-data h4 {
      margin: 0 0 8px 0;
      color: #333;
      font-weight: 500;
    }

    .insufficient-data p {
      margin: 0 0 16px 0;
      font-size: 14px;
    }

    .data-requirements {
      background: #f5f5f5;
      padding: 16px;
      border-radius: 8px;
      text-align: left;
      max-width: 300px;
    }

    .data-requirements h5 {
      margin: 0 0 8px 0;
      font-size: 14px;
      font-weight: 500;
      color: #333;
    }

    .data-requirements ul {
      margin: 0;
      padding-left: 16px;
      font-size: 12px;
      line-height: 1.4;
    }

    .data-requirements li {
      margin-bottom: 4px;
    }

    .chart-container {
      height: 100%;
      display: flex;
      flex-direction: column;
    }

    .compliance-summary {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 16px;
      padding: 16px;
      background: #f8f9fa;
      border-radius: 8px;
    }

    .compliance-percentage {
      display: flex;
      flex-direction: column;
      align-items: center;
    }

    .percentage-value {
      font-size: 32px;
      font-weight: bold;
      color: #666;
      transition: color 0.3s ease;
    }

    .percentage-value.good {
      color: #4caf50;
    }

    .percentage-value.poor {
      color: #f44336;
    }

    .percentage-label {
      font-size: 12px;
      color: #666;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }

    .compliance-breakdown {
      display: flex;
      gap: 16px;
    }

    .breakdown-item {
      display: flex;
      flex-direction: column;
      align-items: center;
      padding: 12px 16px;
      border-radius: 8px;
      min-width: 60px;
    }

    .breakdown-item.on-time {
      background: #e8f5e8;
      color: #2e7d32;
    }

    .breakdown-item.overdue {
      background: #ffebee;
      color: #c62828;
    }

    .breakdown-item .count {
      font-size: 20px;
      font-weight: bold;
      margin-bottom: 4px;
    }

    .breakdown-item .label {
      font-size: 11px;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }

    .chart-wrapper {
      flex: 1;
      position: relative;
      min-height: 200px;
    }

    canvas {
      max-height: 100%;
      max-width: 100%;
    }

    .compliance-insights {
      margin-top: 16px;
      padding: 12px 16px;
      border-radius: 8px;
      background: #f5f5f5;
    }

    .insight {
      display: flex;
      align-items: center;
      gap: 8px;
      font-size: 14px;
      font-weight: 500;
    }

    .insight.excellent {
      color: #2e7d32;
    }

    .insight.good {
      color: #388e3c;
    }

    .insight.fair {
      color: #f57c00;
    }

    .insight.poor {
      color: #d32f2f;
    }

    .insight mat-icon {
      font-size: 18px;
      width: 18px;
      height: 18px;
    }

    /* Responsive Design */
    @media (max-width: 768px) {
      .compliance-summary {
        flex-direction: column;
        gap: 16px;
      }

      .compliance-breakdown {
        justify-content: center;
      }

      .percentage-value {
        font-size: 28px;
      }
    }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ServiceComplianceChartComponent implements OnInit, OnDestroy, OnChanges {
  @Input() data: ServiceComplianceData | null = null;
  @ViewChild('chartCanvas', { static: true }) chartCanvas!: ElementRef<HTMLCanvasElement>;
  
  private chart: Chart | null = null;

  // Computed properties for data analysis
  hasInsufficientData = computed(() => {
    if (!this.data) return true;
    const total = this.data.onTime + this.data.overdue;
    return total < 5; // Require at least 5 data points for meaningful analysis
  });

  insufficientDataMessage = computed(() => {
    if (!this.data) {
      return 'No service compliance data available for analysis.';
    }
    const total = this.data.onTime + this.data.overdue;
    if (total === 0) {
      return 'No completed or overdue maintenance jobs found in the last 30 days.';
    }
    if (total < 5) {
      return `Only ${total} maintenance job${total === 1 ? '' : 's'} found. Need at least 5 for reliable analysis.`;
    }
    return 'Insufficient data for compliance analysis.';
  });

  isGoodCompliance = computed(() => {
    return this.data && this.data.percentage >= 80;
  });

  isPoorCompliance = computed(() => {
    return this.data && this.data.percentage < 60;
  });

  ngOnInit() {
    if (this.data && !this.hasInsufficientData()) {
      this.createChart();
    }
  }

  ngOnDestroy() {
    if (this.chart) {
      this.chart.destroy();
    }
  }

  ngOnChanges() {
    if (this.data && this.chartCanvas && !this.hasInsufficientData()) {
      if (this.chart) {
        this.updateChart();
      } else {
        this.createChart();
      }
    } else if (this.chart) {
      // Destroy chart if data becomes insufficient
      this.chart.destroy();
      this.chart = null;
    }
  }

  getComplianceLevel(): string {
    if (!this.data) return 'poor';
    const percentage = this.data.percentage;
    if (percentage >= 95) return 'excellent';
    if (percentage >= 80) return 'good';
    if (percentage >= 60) return 'fair';
    return 'poor';
  }

  getComplianceIcon(): string {
    const level = this.getComplianceLevel();
    switch (level) {
      case 'excellent': return 'sentiment_very_satisfied';
      case 'good': return 'sentiment_satisfied';
      case 'fair': return 'sentiment_neutral';
      case 'poor': return 'sentiment_dissatisfied';
      default: return 'info';
    }
  }

  getComplianceMessage(): string {
    if (!this.data) return 'No data available';
    
    const percentage = this.data.percentage;
    const level = this.getComplianceLevel();
    
    switch (level) {
      case 'excellent':
        return `Excellent compliance rate! ${this.data.onTime} of ${this.data.onTime + this.data.overdue} services completed on time.`;
      case 'good':
        return `Good compliance rate. Consider reviewing the ${this.data.overdue} overdue maintenance${this.data.overdue === 1 ? '' : 's'}.`;
      case 'fair':
        return `Fair compliance rate. ${this.data.overdue} overdue maintenance${this.data.overdue === 1 ? '' : 's'} need${this.data.overdue === 1 ? 's' : ''} attention.`;
      case 'poor':
        return `Poor compliance rate. Immediate action needed for ${this.data.overdue} overdue maintenance${this.data.overdue === 1 ? '' : 's'}.`;
      default:
        return 'Unable to determine compliance level';
    }
  }

  private createChart() {
    if (!this.data || !this.chartCanvas || this.hasInsufficientData()) return;

    const ctx = this.chartCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    const total = this.data.onTime + this.data.overdue;
    const onTimePercentage = total > 0 ? (this.data.onTime / total * 100).toFixed(1) : '0';
    const overduePercentage = total > 0 ? (this.data.overdue / total * 100).toFixed(1) : '0';

    const config: ChartConfiguration = {
      type: 'doughnut' as ChartType,
      data: {
        labels: ['On Time', 'Overdue'],
        datasets: [{
          data: [this.data.onTime, this.data.overdue],
          backgroundColor: [
            '#4caf50', // Green for on-time
            '#f44336'  // Red for overdue
          ],
          borderWidth: 3,
          borderColor: '#ffffff',
          hoverBorderWidth: 4,
          hoverBorderColor: '#ffffff'
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
              usePointStyle: true,
              pointStyle: 'circle',
              font: {
                size: 12,
                weight: 500
              }
            }
          },
          tooltip: {
            backgroundColor: 'rgba(0, 0, 0, 0.8)',
            titleColor: '#ffffff',
            bodyColor: '#ffffff',
            borderColor: '#ffffff',
            borderWidth: 1,
            cornerRadius: 8,
            displayColors: true,
            callbacks: {
              label: (context) => {
                const label = context.label || '';
                const value = context.parsed as number;
                const percentage = total > 0 ? (value / total * 100).toFixed(1) : '0';
                return `${label}: ${value} jobs (${percentage}%)`;
              },
              afterLabel: (context) => {
                if (context.label === 'On Time') {
                  return 'Services completed within scheduled timeframe';
                } else {
                  return 'Services that exceeded their due date';
                }
              }
            }
          }
        },
        animation: {
          duration: 1000,
          easing: 'easeOutQuart'
        },
        interaction: {
          intersect: false,
          mode: 'index'
        }
      }
    };

    this.chart = new Chart(ctx, config);
  }

  private updateChart() {
    if (!this.chart || !this.data || this.hasInsufficientData()) return;

    this.chart.data.datasets[0].data = [this.data.onTime, this.data.overdue];
    this.chart.update('active');
  }
}