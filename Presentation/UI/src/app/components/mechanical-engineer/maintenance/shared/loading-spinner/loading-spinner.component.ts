import { Component, input, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';

export type SpinnerSize = 'small' | 'medium' | 'large';
export type SpinnerType = 'circular' | 'linear';

@Component({
  selector: 'app-loading-spinner',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, MatProgressBarModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="loading-container" [class]="'loading-' + size()">
      @if (type() === 'circular') {
        <mat-spinner 
          [diameter]="getDiameter()"
          [strokeWidth]="getStrokeWidth()"
          [value]="progress() !== undefined ? progress() : undefined"
          [mode]="progress() !== undefined ? 'determinate' : 'indeterminate'">
        </mat-spinner>
      } @else {
        <mat-progress-bar
          [value]="progress() !== undefined ? progress() : undefined"
          [mode]="progress() !== undefined ? 'determinate' : 'indeterminate'">
        </mat-progress-bar>
      }
      
      @if (message()) {
        <div class="loading-message">{{ message() }}</div>
      }
      
      @if (progress() !== undefined && showProgress()) {
        <div class="loading-progress">{{ progress() }}%</div>
      }
    </div>
  `,
  styles: [`
    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      gap: 12px;
      padding: 16px;
    }

    .loading-small {
      gap: 8px;
      padding: 8px;
    }

    .loading-large {
      gap: 16px;
      padding: 24px;
    }

    .loading-message {
      font-size: 14px;
      color: #666;
      text-align: center;
      max-width: 300px;
    }

    .loading-small .loading-message {
      font-size: 12px;
    }

    .loading-large .loading-message {
      font-size: 16px;
    }

    .loading-progress {
      font-size: 12px;
      color: #1976d2;
      font-weight: 500;
    }

    .loading-small .loading-progress {
      font-size: 10px;
    }

    .loading-large .loading-progress {
      font-size: 14px;
    }

    mat-progress-bar {
      width: 200px;
    }

    .loading-small mat-progress-bar {
      width: 150px;
    }

    .loading-large mat-progress-bar {
      width: 300px;
    }
  `]
})
export class LoadingSpinnerComponent {
  size = input<SpinnerSize>('medium');
  type = input<SpinnerType>('circular');
  message = input<string>('');
  progress = input<number | undefined>(undefined);
  showProgress = input<boolean>(true);

  getDiameter(): number {
    switch (this.size()) {
      case 'small': return 24;
      case 'medium': return 40;
      case 'large': return 60;
      default: return 40;
    }
  }

  getStrokeWidth(): number {
    switch (this.size()) {
      case 'small': return 2;
      case 'medium': return 3;
      case 'large': return 4;
      default: return 3;
    }
  }
}