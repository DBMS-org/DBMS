import { Component, Input, OnInit, OnDestroy, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { PatternSettings, DrillPoint } from '../../models/drill-point.model';
import { PatternStateService } from '../../services/pattern-state.service';
import { Logger } from '../../utils/logger.util';

/**
 * PatternStatusBarComponent displays real-time status information for the drilling pattern creator.
 * This component is responsible for:
 * - Displaying cursor position tracking in real-time
 * - Showing current scale and zoom level
 * - Indicating active modes (hole placement, precise mode, fullscreen)
 * - Displaying drill point count and pattern statistics
 * - Providing reactive status updates based on state changes
 */
@Component({
  selector: 'app-pattern-status-bar',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="status-bar" [class.fullscreen]="isFullscreen" role="status" aria-live="polite">
      <!-- Cursor Position Section -->
      <div class="status-section cursor-position" *ngIf="cursorPosition">
        <span class="status-label">Position:</span>
        <span class="status-value">
          X: {{ formatCoordinate(cursorPosition.x) }}m, 
          Y: {{ formatCoordinate(cursorPosition.y) }}m
        </span>
      </div>

      <!-- Scale and Zoom Section -->
      <div class="status-section scale-info">
        <span class="status-label">Scale:</span>
        <span class="status-value">{{ formatScale(scale) }}%</span>
        <span class="status-separator">|</span>
        <span class="status-label">Zoom:</span>
        <span class="status-value">{{ formatZoomLevel(scale) }}</span>
      </div>

      <!-- Mode Indicators Section -->
      <div class="status-section mode-indicators">
        <span class="mode-indicator" 
              [class.active]="isHolePlacementMode"
              [attr.aria-label]="isHolePlacementMode ? 'Hole placement mode active' : 'Hole placement mode inactive'">
          <span class="mode-icon">⊕</span>
          <span class="mode-text">Place</span>
        </span>
        <span class="mode-indicator" 
              [class.active]="isPreciseMode"
              [attr.aria-label]="isPreciseMode ? 'Precise mode active' : 'Precise mode inactive'">
          <span class="mode-icon">⊞</span>
          <span class="mode-text">Precise</span>
        </span>
        <span class="mode-indicator" 
              [class.active]="isFullscreen"
              [attr.aria-label]="isFullscreen ? 'Fullscreen mode active' : 'Fullscreen mode inactive'">
          <span class="mode-icon">⛶</span>
          <span class="mode-text">Full</span>
        </span>
      </div>

      <!-- Pattern Statistics Section -->
      <div class="status-section pattern-stats">
        <span class="status-label">Points:</span>
        <span class="status-value">{{ drillPointCount }}</span>
        <span class="status-separator">|</span>
        <span class="status-label">Selected:</span>
        <span class="status-value">{{ selectedPointInfo }}</span>
      </div>

      <!-- Pattern Settings Section -->
      <div class="status-section pattern-settings" *ngIf="settings">
        <span class="status-label">Spacing:</span>
        <span class="status-value">{{ formatValue(settings.spacing) }}m</span>
        <span class="status-separator">|</span>
        <span class="status-label">Burden:</span>
        <span class="status-value">{{ formatValue(settings.burden) }}m</span>
        <span class="status-separator">|</span>
        <span class="status-label">Depth:</span>
        <span class="status-value">{{ formatValue(settings.depth) }}m</span>
      </div>

      <!-- Save Status Section -->
      <div class="status-section save-status">
        <span class="save-indicator" 
              [class.saved]="isSaved"
              [class.unsaved]="!isSaved"
              [attr.aria-label]="isSaved ? 'Pattern saved' : 'Pattern has unsaved changes'">
          <span class="save-icon">{{ isSaved ? '✓' : '●' }}</span>
          <span class="save-text">{{ isSaved ? 'Saved' : 'Modified' }}</span>
        </span>
      </div>
    </div>
  `,
  styleUrls: ['./pattern-status-bar.component.scss']
})
export class PatternStatusBarComponent implements OnInit, OnDestroy, OnChanges {
  @Input() cursorPosition: { x: number; y: number } | null = null;
  @Input() scale: number = 1;
  @Input() isHolePlacementMode: boolean = false;
  @Input() isPreciseMode: boolean = false;
  @Input() isFullscreen: boolean = false;
  @Input() drillPoints: DrillPoint[] = [];
  @Input() selectedPoint: DrillPoint | null = null;
  @Input() settings: PatternSettings | null = null;
  @Input() isSaved: boolean = true;

  // Computed properties for display
  drillPointCount: number = 0;
  selectedPointInfo: string = 'None';

  // Component lifecycle
  private destroy$ = new Subject<void>();

  constructor(private patternStateService: PatternStateService) { }

  ngOnInit(): void {
    Logger.info('PatternStatusBarComponent initialized');
    this.subscribeToStateChanges();
    this.updateComputedProperties();
  }

  ngOnDestroy(): void {
    Logger.info('PatternStatusBarComponent destroying');
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Update computed properties when inputs change
    if (changes['drillPoints'] || changes['selectedPoint']) {
      this.updateComputedProperties();
    }

    // Log significant state changes for debugging
    if (changes['isHolePlacementMode']) {
      Logger.debug(`Hole placement mode: ${this.isHolePlacementMode}`);
    }
    if (changes['isPreciseMode']) {
      Logger.debug(`Precise mode: ${this.isPreciseMode}`);
    }
    if (changes['scale']) {
      Logger.debug(`Scale changed to: ${this.scale}`);
    }
  }

  /**
   * Subscribe to reactive state changes from PatternStateService
   */
  private subscribeToStateChanges(): void {
    // Subscribe to cursor position updates
    this.patternStateService.state$
      .pipe(takeUntil(this.destroy$))
      .subscribe(state => {
        // Update cursor position if it's different from input
        if (state.cursorPosition && !this.cursorPosition) {
          this.cursorPosition = state.cursorPosition;
        }

        // Update save status
        if (state.isSaved !== this.isSaved) {
          this.isSaved = state.isSaved;
        }
      });

    // Subscribe to drill points changes
    this.patternStateService.drillPoints$
      .pipe(takeUntil(this.destroy$))
      .subscribe(points => {
        if (points.length !== this.drillPoints.length) {
          this.drillPoints = points;
          this.updateComputedProperties();
        }
      });

    // Subscribe to selected point changes
    this.patternStateService.selectedPoint$
      .pipe(takeUntil(this.destroy$))
      .subscribe(point => {
        if (point !== this.selectedPoint) {
          this.selectedPoint = point;
          this.updateComputedProperties();
        }
      });

    // Subscribe to settings changes
    this.patternStateService.settings$
      .pipe(takeUntil(this.destroy$))
      .subscribe(settings => {
        if (settings !== this.settings) {
          this.settings = settings;
        }
      });
  }

  /**
   * Update computed properties for display
   */
  private updateComputedProperties(): void {
    // Update drill point count
    this.drillPointCount = this.drillPoints.length;

    // Update selected point info
    if (this.selectedPoint) {
      this.selectedPointInfo = `ID: ${this.selectedPoint.id.substring(0, 8)}...`;
    } else {
      this.selectedPointInfo = 'None';
    }
  }

  /**
   * Format coordinate values for display
   */
  formatCoordinate(value: number): string {
    return value.toFixed(2);
  }

  /**
   * Format scale as percentage
   */
  formatScale(scale: number): string {
    return Math.round(scale * 100).toString();
  }

  /**
   * Format zoom level description
   */
  formatZoomLevel(scale: number): string {
    if (scale >= 2) return 'High';
    if (scale >= 1.5) return 'Medium';
    if (scale >= 1) return 'Normal';
    if (scale >= 0.5) return 'Low';
    return 'Very Low';
  }

  /**
   * Format numeric values for display
   */
  formatValue(value: number): string {
    return value.toFixed(1);
  }

  /**
   * Get performance statistics for debugging
   */
  getStatusInfo(): {
    drillPointCount: number;
    selectedPointId: string | null;
    scale: number;
    modes: {
      holePlacement: boolean;
      precise: boolean;
      fullscreen: boolean;
    };
    isSaved: boolean;
  } {
    return {
      drillPointCount: this.drillPointCount,
      selectedPointId: this.selectedPoint?.id || null,
      scale: this.scale,
      modes: {
        holePlacement: this.isHolePlacementMode,
        precise: this.isPreciseMode,
        fullscreen: this.isFullscreen
      },
      isSaved: this.isSaved
    };
  }
}