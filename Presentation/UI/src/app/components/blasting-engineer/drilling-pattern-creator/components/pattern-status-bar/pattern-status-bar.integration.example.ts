import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, interval } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { PatternStatusBarComponent } from './pattern-status-bar.component';
import { PatternStateService } from '../../services/pattern-state.service';
import { DrillPoint, PatternSettings } from '../../models/drill-point.model';

/**
 * Integration example demonstrating how to use PatternStatusBarComponent
 * with real-time updates and state management.
 */
@Component({
  selector: 'app-status-bar-integration-example',
  standalone: true,
  imports: [CommonModule, PatternStatusBarComponent],
  template: `
    <div class="integration-example">
      <h2>Pattern Status Bar Integration Example</h2>
      
      <!-- Control Panel -->
      <div class="control-panel">
        <h3>Controls</h3>
        <div class="controls">
          <button (click)="toggleHolePlacementMode()" 
                  [class.active]="isHolePlacementMode">
            Toggle Hole Placement Mode
          </button>
          <button (click)="togglePreciseMode()" 
                  [class.active]="isPreciseMode">
            Toggle Precise Mode
          </button>
          <button (click)="toggleFullscreen()" 
                  [class.active]="isFullscreen">
            Toggle Fullscreen
          </button>
          <button (click)="addRandomPoint()">
            Add Random Point
          </button>
          <button (click)="selectRandomPoint()">
            Select Random Point
          </button>
          <button (click)="clearAllPoints()">
            Clear All Points
          </button>
          <button (click)="toggleSaveStatus()">
            Toggle Save Status
          </button>
        </div>
        
        <div class="scale-controls">
          <label>Scale: {{ scale }}x</label>
          <input type="range" 
                 min="0.25" 
                 max="3" 
                 step="0.25" 
                 [(ngModel)]="scale"
                 (input)="onScaleChange()">
        </div>
        
        <div class="settings-controls">
          <h4>Pattern Settings</h4>
          <div class="setting-group">
            <label>Spacing: {{ settings.spacing }}m</label>
            <input type="range" 
                   min="1" 
                   max="10" 
                   step="0.5" 
                   [(ngModel)]="settings.spacing"
                   (input)="onSettingsChange()">
          </div>
          <div class="setting-group">
            <label>Burden: {{ settings.burden }}m</label>
            <input type="range" 
                   min="1" 
                   max="8" 
                   step="0.5" 
                   [(ngModel)]="settings.burden"
                   (input)="onSettingsChange()">
          </div>
          <div class="setting-group">
            <label>Depth: {{ settings.depth }}m</label>
            <input type="range" 
                   min="5" 
                   max="25" 
                   step="1" 
                   [(ngModel)]="settings.depth"
                   (input)="onSettingsChange()">
          </div>
        </div>
      </div>

      <!-- Status Bar Display -->
      <div class="status-bar-container" [class.fullscreen-demo]="isFullscreen">
        <h3>Status Bar Component</h3>
        <app-pattern-status-bar
          [cursorPosition]="cursorPosition"
          [scale]="scale"
          [isHolePlacementMode]="isHolePlacementMode"
          [isPreciseMode]="isPreciseMode"
          [isFullscreen]="isFullscreen"
          [drillPoints]="drillPoints"
          [selectedPoint]="selectedPoint"
          [settings]="settings"
          [isSaved]="isSaved">
        </app-pattern-status-bar>
      </div>

      <!-- State Information Display -->
      <div class="state-info">
        <h3>Current State</h3>
        <div class="state-grid">
          <div class="state-item">
            <strong>Drill Points:</strong> {{ drillPoints.length }}
          </div>
          <div class="state-item">
            <strong>Selected Point:</strong> 
            {{ selectedPoint ? selectedPoint.id.substring(0, 8) + '...' : 'None' }}
          </div>
          <div class="state-item">
            <strong>Cursor Position:</strong> 
            {{ cursorPosition ? 'X: ' + cursorPosition.x.toFixed(2) + ', Y: ' + cursorPosition.y.toFixed(2) : 'None' }}
          </div>
          <div class="state-item">
            <strong>Scale:</strong> {{ (scale * 100).toFixed(0) }}%
          </div>
          <div class="state-item">
            <strong>Modes:</strong>
            <span [class.active-mode]="isHolePlacementMode">Place</span>,
            <span [class.active-mode]="isPreciseMode">Precise</span>,
            <span [class.active-mode]="isFullscreen">Fullscreen</span>
          </div>
          <div class="state-item">
            <strong>Save Status:</strong> 
            <span [class.saved]="isSaved" [class.unsaved]="!isSaved">
              {{ isSaved ? 'Saved' : 'Modified' }}
            </span>
          </div>
        </div>
      </div>

      <!-- Performance Monitoring -->
      <div class="performance-info">
        <h3>Performance Monitoring</h3>
        <div class="perf-grid">
          <div class="perf-item">
            <strong>Update Frequency:</strong> {{ updateCount }} updates
          </div>
          <div class="perf-item">
            <strong>Last Update:</strong> {{ lastUpdateTime | date:'HH:mm:ss.SSS' }}
          </div>
          <div class="perf-item">
            <strong>Cursor Updates:</strong> {{ cursorUpdateCount }}
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .integration-example {
      padding: 20px;
      font-family: 'Roboto', sans-serif;
    }

    .control-panel {
      background: #f5f5f5;
      padding: 20px;
      border-radius: 8px;
      margin-bottom: 20px;
    }

    .controls {
      display: flex;
      flex-wrap: wrap;
      gap: 10px;
      margin-bottom: 20px;
    }

    .controls button {
      padding: 8px 16px;
      border: 1px solid #ccc;
      background: white;
      border-radius: 4px;
      cursor: pointer;
      transition: all 0.2s;
    }

    .controls button:hover {
      background: #e3f2fd;
    }

    .controls button.active {
      background: #2196f3;
      color: white;
      border-color: #1976d2;
    }

    .scale-controls, .settings-controls {
      margin-top: 20px;
    }

    .setting-group {
      margin: 10px 0;
    }

    .setting-group label {
      display: block;
      margin-bottom: 5px;
      font-weight: 500;
    }

    .setting-group input[type="range"] {
      width: 200px;
    }

    .status-bar-container {
      background: white;
      border: 1px solid #e0e0e0;
      border-radius: 8px;
      margin-bottom: 20px;
      overflow: hidden;
    }

    .status-bar-container h3 {
      background: #f5f5f5;
      margin: 0;
      padding: 10px 20px;
      border-bottom: 1px solid #e0e0e0;
    }

    .status-bar-container.fullscreen-demo {
      position: fixed;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      z-index: 1000;
      background: white;
      border-radius: 0;
    }

    .state-info, .performance-info {
      background: #f9f9f9;
      padding: 20px;
      border-radius: 8px;
      margin-bottom: 20px;
    }

    .state-grid, .perf-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 15px;
      margin-top: 15px;
    }

    .state-item, .perf-item {
      background: white;
      padding: 10px;
      border-radius: 4px;
      border-left: 4px solid #2196f3;
    }

    .active-mode {
      color: #2196f3;
      font-weight: bold;
    }

    .saved {
      color: #4caf50;
      font-weight: bold;
    }

    .unsaved {
      color: #ff9800;
      font-weight: bold;
    }

    @media (max-width: 768px) {
      .controls {
        flex-direction: column;
      }
      
      .state-grid, .perf-grid {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class PatternStatusBarIntegrationExample implements OnInit, OnDestroy {
  // Component state
  cursorPosition: { x: number; y: number } | null = null;
  scale: number = 1;
  isHolePlacementMode: boolean = false;
  isPreciseMode: boolean = false;
  isFullscreen: boolean = false;
  drillPoints: DrillPoint[] = [];
  selectedPoint: DrillPoint | null = null;
  isSaved: boolean = true;

  settings: PatternSettings = {
    spacing: 3.0,
    burden: 2.5,
    depth: 10.0
  };

  // Performance monitoring
  updateCount: number = 0;
  lastUpdateTime: Date = new Date();
  cursorUpdateCount: number = 0;

  private destroy$ = new Subject<void>();
  private pointIdCounter = 1;

  constructor(private patternStateService: PatternStateService) {}

  ngOnInit(): void {
    this.setupCursorSimulation();
    this.setupStateMonitoring();
    this.initializeWithSampleData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Setup cursor position simulation
   */
  private setupCursorSimulation(): void {
    // Simulate cursor movement every 100ms
    interval(100)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        if (this.isHolePlacementMode) {
          this.cursorPosition = {
            x: Math.random() * 50,
            y: Math.random() * 30
          };
          this.cursorUpdateCount++;
          this.patternStateService.setCursorPosition(this.cursorPosition);
        }
      });
  }

  /**
   * Setup state change monitoring
   */
  private setupStateMonitoring(): void {
    this.patternStateService.state$
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.updateCount++;
        this.lastUpdateTime = new Date();
      });
  }

  /**
   * Initialize with sample data
   */
  private initializeWithSampleData(): void {
    // Add some initial drill points
    this.addRandomPoint();
    this.addRandomPoint();
    this.addRandomPoint();
  }

  /**
   * Toggle hole placement mode
   */
  toggleHolePlacementMode(): void {
    this.isHolePlacementMode = !this.isHolePlacementMode;
    this.patternStateService.toggleHolePlacementMode();
    this.markAsModified();
  }

  /**
   * Toggle precise mode
   */
  togglePreciseMode(): void {
    this.isPreciseMode = !this.isPreciseMode;
    this.patternStateService.togglePreciseMode();
    this.markAsModified();
  }

  /**
   * Toggle fullscreen mode
   */
  toggleFullscreen(): void {
    this.isFullscreen = !this.isFullscreen;
    this.patternStateService.toggleFullscreen();
    this.markAsModified();
  }

  /**
   * Add a random drill point
   */
  addRandomPoint(): void {
    const newPoint: DrillPoint = {
      id: `example-point-${this.pointIdCounter++}`,
      x: Math.random() * 40 + 5,
      y: Math.random() * 25 + 5,
      depth: this.settings.depth + (Math.random() - 0.5) * 4,
      spacing: this.settings.spacing,
      burden: this.settings.burden
    };

    this.drillPoints = [...this.drillPoints, newPoint];
    this.patternStateService.addDrillPoint(newPoint);
    this.markAsModified();
  }

  /**
   * Select a random drill point
   */
  selectRandomPoint(): void {
    if (this.drillPoints.length > 0) {
      const randomIndex = Math.floor(Math.random() * this.drillPoints.length);
      this.selectedPoint = this.drillPoints[randomIndex];
      this.patternStateService.selectPoint(this.selectedPoint);
    }
  }

  /**
   * Clear all drill points
   */
  clearAllPoints(): void {
    this.drillPoints = [];
    this.selectedPoint = null;
    this.patternStateService.clearAllPoints();
    this.markAsModified();
  }

  /**
   * Toggle save status
   */
  toggleSaveStatus(): void {
    this.isSaved = !this.isSaved;
    this.patternStateService.setSaved(this.isSaved);
  }

  /**
   * Handle scale changes
   */
  onScaleChange(): void {
    this.markAsModified();
  }

  /**
   * Handle settings changes
   */
  onSettingsChange(): void {
    this.patternStateService.updateSettings(this.settings);
    this.markAsModified();
  }

  /**
   * Mark pattern as modified
   */
  private markAsModified(): void {
    if (this.isSaved) {
      this.isSaved = false;
      this.patternStateService.setSaved(false);
    }
  }
}