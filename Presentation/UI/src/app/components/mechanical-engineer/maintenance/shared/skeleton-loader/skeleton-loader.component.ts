import { Component, input, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';

export type SkeletonType = 'text' | 'card' | 'table' | 'chart' | 'list' | 'avatar' | 'button';

@Component({
  selector: 'app-skeleton-loader',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="skeleton-container" [class]="'skeleton-' + type()">
      @switch (type()) {
        @case ('text') {
          <div class="skeleton-text" [style.width]="width()" [style.height]="height()"></div>
        }
        @case ('card') {
          <div class="skeleton-card" [style.width]="width()" [style.height]="height()">
            <div class="skeleton-card-header"></div>
            <div class="skeleton-card-content">
              <div class="skeleton-text-line"></div>
              <div class="skeleton-text-line short"></div>
              <div class="skeleton-text-line medium"></div>
            </div>
          </div>
        }
        @case ('table') {
          <div class="skeleton-table" [style.width]="width()">
            <div class="skeleton-table-header">
              @for (col of getTableColumns(); track col) {
                <div class="skeleton-table-header-cell"></div>
              }
            </div>
            @for (row of getTableRows(); track row) {
              <div class="skeleton-table-row">
                @for (col of getTableColumns(); track col) {
                  <div class="skeleton-table-cell"></div>
                }
              </div>
            }
          </div>
        }
        @case ('chart') {
          <div class="skeleton-chart" [style.width]="width()" [style.height]="height()">
            <div class="skeleton-chart-title"></div>
            <div class="skeleton-chart-content">
              <div class="skeleton-chart-bars">
                @for (bar of getChartBars(); track bar) {
                  <div class="skeleton-chart-bar" [style.height]="bar + '%'"></div>
                }
              </div>
            </div>
            <div class="skeleton-chart-legend">
              @for (item of getChartLegend(); track item) {
                <div class="skeleton-legend-item"></div>
              }
            </div>
          </div>
        }
        @case ('list') {
          <div class="skeleton-list" [style.width]="width()">
            @for (item of getListItems(); track item) {
              <div class="skeleton-list-item">
                <div class="skeleton-avatar"></div>
                <div class="skeleton-list-content">
                  <div class="skeleton-text-line"></div>
                  <div class="skeleton-text-line short"></div>
                </div>
              </div>
            }
          </div>
        }
        @case ('avatar') {
          <div class="skeleton-avatar" [style.width]="width()" [style.height]="height()"></div>
        }
        @case ('button') {
          <div class="skeleton-button" [style.width]="width()" [style.height]="height()"></div>
        }
      }
    </div>
  `,
  styles: [`
    .skeleton-container {
      animation: pulse 1.5s ease-in-out infinite;
    }

    @keyframes pulse {
      0% {
        opacity: 1;
      }
      50% {
        opacity: 0.5;
      }
      100% {
        opacity: 1;
      }
    }

    .skeleton-text,
    .skeleton-text-line,
    .skeleton-card-header,
    .skeleton-table-header-cell,
    .skeleton-table-cell,
    .skeleton-chart-title,
    .skeleton-chart-bar,
    .skeleton-legend-item,
    .skeleton-avatar,
    .skeleton-button {
      background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
      background-size: 200% 100%;
      animation: loading 1.5s infinite;
      border-radius: 4px;
    }

    @keyframes loading {
      0% {
        background-position: 200% 0;
      }
      100% {
        background-position: -200% 0;
      }
    }

    /* Text Skeleton */
    .skeleton-text {
      height: 16px;
      width: 100%;
    }

    /* Card Skeleton */
    .skeleton-card {
      border: 1px solid #e0e0e0;
      border-radius: 8px;
      padding: 16px;
      background: white;
    }

    .skeleton-card-header {
      height: 24px;
      width: 60%;
      margin-bottom: 16px;
    }

    .skeleton-card-content {
      display: flex;
      flex-direction: column;
      gap: 8px;
    }

    .skeleton-text-line {
      height: 14px;
      width: 100%;
    }

    .skeleton-text-line.short {
      width: 40%;
    }

    .skeleton-text-line.medium {
      width: 70%;
    }

    /* Table Skeleton */
    .skeleton-table {
      border: 1px solid #e0e0e0;
      border-radius: 4px;
      overflow: hidden;
    }

    .skeleton-table-header {
      display: flex;
      background: #f5f5f5;
      padding: 12px 0;
    }

    .skeleton-table-header-cell {
      flex: 1;
      height: 16px;
      margin: 0 12px;
    }

    .skeleton-table-row {
      display: flex;
      padding: 16px 0;
      border-bottom: 1px solid #e0e0e0;
    }

    .skeleton-table-row:last-child {
      border-bottom: none;
    }

    .skeleton-table-cell {
      flex: 1;
      height: 14px;
      margin: 0 12px;
    }

    /* Chart Skeleton */
    .skeleton-chart {
      border: 1px solid #e0e0e0;
      border-radius: 8px;
      padding: 16px;
      background: white;
    }

    .skeleton-chart-title {
      height: 20px;
      width: 50%;
      margin-bottom: 16px;
    }

    .skeleton-chart-content {
      height: 200px;
      display: flex;
      align-items: end;
      margin-bottom: 16px;
    }

    .skeleton-chart-bars {
      display: flex;
      align-items: end;
      gap: 8px;
      width: 100%;
      height: 100%;
    }

    .skeleton-chart-bar {
      flex: 1;
      min-height: 20px;
    }

    .skeleton-chart-legend {
      display: flex;
      gap: 16px;
      flex-wrap: wrap;
    }

    .skeleton-legend-item {
      height: 12px;
      width: 80px;
    }

    /* List Skeleton */
    .skeleton-list-item {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 12px 0;
      border-bottom: 1px solid #e0e0e0;
    }

    .skeleton-list-item:last-child {
      border-bottom: none;
    }

    .skeleton-avatar {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      flex-shrink: 0;
    }

    .skeleton-list-content {
      flex: 1;
      display: flex;
      flex-direction: column;
      gap: 4px;
    }

    /* Button Skeleton */
    .skeleton-button {
      height: 36px;
      width: 120px;
    }
  `]
})
export class SkeletonLoaderComponent {
  type = input<SkeletonType>('text');
  width = input<string>('100%');
  height = input<string>('auto');
  rows = input<number>(5);
  columns = input<number>(4);

  getTableRows(): number[] {
    return Array.from({ length: this.rows() }, (_, i) => i);
  }

  getTableColumns(): number[] {
    return Array.from({ length: this.columns() }, (_, i) => i);
  }

  getChartBars(): number[] {
    return [60, 80, 45, 90, 70, 55, 85];
  }

  getChartLegend(): number[] {
    return Array.from({ length: 4 }, (_, i) => i);
  }

  getListItems(): number[] {
    return Array.from({ length: this.rows() }, (_, i) => i);
  }
}