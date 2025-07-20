/**
 * Integration examples for RulerCanvasComponent
 * These examples demonstrate how to integrate the RulerCanvasComponent
 * with other canvas components and the main drilling pattern creator.
 */

import { Component, ViewChild, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import Konva from 'konva';
import { RulerCanvasComponent } from './ruler-canvas.component';
import { GridCanvasComponent } from '../grid-canvas/grid-canvas.component';
import { PatternSettings } from '../../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../../constants/canvas.constants';

/**
 * Example 1: Basic Integration with Pattern Canvas
 * Shows how to integrate RulerCanvasComponent with a main canvas container
 */
@Component({
  selector: 'app-ruler-integration-basic',
  template: `
    <div class="canvas-container">
      <div #canvasContainer class="konva-container"></div>
      <app-ruler-canvas
        [layer]="rulerLayer"
        [settings]="patternSettings"
        [canvasState]="canvasState">
      </app-ruler-canvas>
    </div>
  `,
  styles: [`
    .canvas-container {
      position: relative;
      width: 100%;
      height: 600px;
      border: 1px solid #ccc;
    }
    .konva-container {
      width: 100%;
      height: 100%;
    }
  `]
})
export class RulerIntegrationBasicExample implements AfterViewInit, OnDestroy {
  @ViewChild('canvasContainer', { static: true }) canvasContainer!: ElementRef<HTMLDivElement>;
  @ViewChild(RulerCanvasComponent) rulerComponent!: RulerCanvasComponent;

  // Konva objects
  private stage!: Konva.Stage;
  rulerLayer!: Konva.Layer;

  // Component state
  patternSettings: PatternSettings = {
    spacing: 3,
    burden: 2.5,
    depth: 10
  };

  canvasState = {
    scale: 1,
    offsetX: 0,
    offsetY: 0,
    panOffsetX: 0,
    panOffsetY: 0,
    width: 800,
    height: 600
  };

  ngAfterViewInit(): void {
    this.initializeCanvas();
    this.setupEventHandlers();
  }

  ngOnDestroy(): void {
    this.cleanup();
  }

  private initializeCanvas(): void {
    const container = this.canvasContainer.nativeElement;
    
    this.stage = new Konva.Stage({
      container: container,
      width: container.offsetWidth,
      height: container.offsetHeight
    });

    this.rulerLayer = new Konva.Layer();
    this.stage.add(this.rulerLayer);

    // Update canvas state with actual dimensions
    this.canvasState = {
      ...this.canvasState,
      width: container.offsetWidth,
      height: container.offsetHeight
    };
  }

  private setupEventHandlers(): void {
    // Handle zoom events
    this.stage.on('wheel', (e) => {
      e.evt.preventDefault();
      
      const scaleBy = 1.1;
      const stage = this.stage;
      const oldScale = stage.scaleX();
      const pointer = stage.getPointerPosition();
      
      if (!pointer) return;

      const mousePointTo = {
        x: (pointer.x - stage.x()) / oldScale,
        y: (pointer.y - stage.y()) / oldScale,
      };

      const newScale = e.evt.deltaY > 0 ? oldScale * scaleBy : oldScale / scaleBy;
      
      // Update canvas state
      this.canvasState = {
        ...this.canvasState,
        scale: newScale,
        panOffsetX: pointer.x - mousePointTo.x * newScale,
        panOffsetY: pointer.y - mousePointTo.y * newScale
      };

      // Update stage
      stage.scale({ x: newScale, y: newScale });
      stage.position({
        x: this.canvasState.panOffsetX,
        y: this.canvasState.panOffsetY
      });
      
      stage.batchDraw();
    });

    // Handle pan events
    this.stage.on('dragmove', () => {
      const position = this.stage.position();
      this.canvasState = {
        ...this.canvasState,
        panOffsetX: position.x,
        panOffsetY: position.y
      };
    });
  }

  private cleanup(): void {
    if (this.stage) {
      this.stage.destroy();
    }
  }

  // Public methods for external control
  public zoomIn(): void {
    const newScale = this.canvasState.scale * 1.2;
    this.updateScale(newScale);
  }

  public zoomOut(): void {
    const newScale = this.canvasState.scale / 1.2;
    this.updateScale(newScale);
  }

  public resetZoom(): void {
    this.updateScale(1);
    this.canvasState = {
      ...this.canvasState,
      panOffsetX: 0,
      panOffsetY: 0
    };
    this.stage.position({ x: 0, y: 0 });
    this.stage.batchDraw();
  }

  private updateScale(newScale: number): void {
    this.canvasState = { ...this.canvasState, scale: newScale };
    this.stage.scale({ x: newScale, y: newScale });
    this.stage.batchDraw();
  }
}

/**
 * Example 2: Integration with Grid and Ruler Components
 * Shows how to coordinate multiple canvas components
 */
@Component({
  selector: 'app-ruler-grid-integration',
  template: `
    <div class="canvas-container">
      <div #canvasContainer class="konva-container"></div>
      <app-ruler-canvas
        [layer]="backgroundLayer"
        [settings]="patternSettings"
        [canvasState]="canvasState">
      </app-ruler-canvas>
      <app-grid-canvas
        [layer]="gridLayer"
        [settings]="patternSettings"
        [canvasState]="canvasState"
        [isPreciseMode]="isPreciseMode">
      </app-grid-canvas>
      <div class="controls">
        <button (click)="togglePreciseMode()">
          {{ isPreciseMode ? 'Disable' : 'Enable' }} Precise Mode
        </button>
        <button (click)="updateSpacing()">Update Spacing</button>
        <button (click)="resetView()">Reset View</button>
      </div>
    </div>
  `,
  styles: [`
    .canvas-container {
      position: relative;
      width: 100%;
      height: 600px;
      border: 1px solid #ccc;
    }
    .konva-container {
      width: 100%;
      height: 100%;
    }
    .controls {
      position: absolute;
      top: 10px;
      right: 10px;
      display: flex;
      gap: 10px;
    }
    button {
      padding: 8px 16px;
      background: #007bff;
      color: white;
      border: none;
      border-radius: 4px;
      cursor: pointer;
    }
    button:hover {
      background: #0056b3;
    }
  `]
})
export class RulerGridIntegrationExample implements AfterViewInit, OnDestroy {
  @ViewChild('canvasContainer', { static: true }) canvasContainer!: ElementRef<HTMLDivElement>;
  @ViewChild(RulerCanvasComponent) rulerComponent!: RulerCanvasComponent;
  @ViewChild(GridCanvasComponent) gridComponent!: GridCanvasComponent;

  // Konva objects
  private stage!: Konva.Stage;
  backgroundLayer!: Konva.Layer;
  gridLayer!: Konva.Layer;

  // Component state
  patternSettings: PatternSettings = {
    spacing: 3,
    burden: 2.5,
    depth: 10
  };

  canvasState = {
    scale: 1,
    offsetX: 0,
    offsetY: 0,
    panOffsetX: 0,
    panOffsetY: 0,
    width: 800,
    height: 600
  };

  isPreciseMode = false;

  ngAfterViewInit(): void {
    this.initializeCanvas();
    this.setupEventHandlers();
  }

  ngOnDestroy(): void {
    this.cleanup();
  }

  private initializeCanvas(): void {
    const container = this.canvasContainer.nativeElement;
    
    this.stage = new Konva.Stage({
      container: container,
      width: container.offsetWidth,
      height: container.offsetHeight,
      draggable: true
    });

    // Create layers in proper order
    this.backgroundLayer = new Konva.Layer();
    this.gridLayer = new Konva.Layer();
    
    this.stage.add(this.backgroundLayer);
    this.stage.add(this.gridLayer);

    // Update canvas state
    this.canvasState = {
      ...this.canvasState,
      width: container.offsetWidth,
      height: container.offsetHeight
    };
  }

  private setupEventHandlers(): void {
    // Synchronized zoom handling
    this.stage.on('wheel', (e) => {
      e.evt.preventDefault();
      
      const scaleBy = 1.1;
      const stage = this.stage;
      const oldScale = stage.scaleX();
      const pointer = stage.getPointerPosition();
      
      if (!pointer) return;

      const mousePointTo = {
        x: (pointer.x - stage.x()) / oldScale,
        y: (pointer.y - stage.y()) / oldScale,
      };

      const newScale = e.evt.deltaY > 0 ? oldScale * scaleBy : oldScale / scaleBy;
      
      this.updateCanvasState({
        scale: newScale,
        panOffsetX: pointer.x - mousePointTo.x * newScale,
        panOffsetY: pointer.y - mousePointTo.y * newScale
      });

      stage.scale({ x: newScale, y: newScale });
      stage.position({
        x: this.canvasState.panOffsetX,
        y: this.canvasState.panOffsetY
      });
      
      stage.batchDraw();
    });

    // Synchronized pan handling
    this.stage.on('dragmove', () => {
      const position = this.stage.position();
      this.updateCanvasState({
        panOffsetX: position.x,
        panOffsetY: position.y
      });
    });

    // Handle window resize
    window.addEventListener('resize', () => {
      this.handleResize();
    });
  }

  private updateCanvasState(updates: Partial<typeof this.canvasState>): void {
    this.canvasState = { ...this.canvasState, ...updates };
  }

  private handleResize(): void {
    const container = this.canvasContainer.nativeElement;
    const newWidth = container.offsetWidth;
    const newHeight = container.offsetHeight;
    
    this.stage.size({ width: newWidth, height: newHeight });
    this.updateCanvasState({ width: newWidth, height: newHeight });
  }

  private cleanup(): void {
    window.removeEventListener('resize', this.handleResize);
    if (this.stage) {
      this.stage.destroy();
    }
  }

  // Public control methods
  public togglePreciseMode(): void {
    this.isPreciseMode = !this.isPreciseMode;
  }

  public updateSpacing(): void {
    this.patternSettings = {
      ...this.patternSettings,
      spacing: this.patternSettings.spacing + 0.5
    };
  }

  public resetView(): void {
    this.canvasState = {
      ...this.canvasState,
      scale: 1,
      panOffsetX: 0,
      panOffsetY: 0
    };
    this.stage.scale({ x: 1, y: 1 });
    this.stage.position({ x: 0, y: 0 });
    this.stage.batchDraw();
  }
}

/**
 * Example 3: Performance Monitoring Integration
 * Shows how to monitor and optimize ruler performance
 */
@Component({
  selector: 'app-ruler-performance-monitor',
  template: `
    <div class="canvas-container">
      <div #canvasContainer class="konva-container"></div>
      <app-ruler-canvas
        [layer]="rulerLayer"
        [settings]="patternSettings"
        [canvasState]="canvasState">
      </app-ruler-canvas>
      <div class="performance-panel">
        <h3>Performance Metrics</h3>
        <div class="metric">
          <label>Cache Size:</label>
          <span>{{ performanceStats.cacheSize }}</span>
        </div>
        <div class="metric">
          <label>Last Render Time:</label>
          <span>{{ performanceStats.lastRenderTime.toFixed(2) }}ms</span>
        </div>
        <div class="metric">
          <label>Total Renders:</label>
          <span>{{ performanceStats.renderCount }}</span>
        </div>
        <div class="metric">
          <label>Average Render Time:</label>
          <span>{{ averageRenderTime.toFixed(2) }}ms</span>
        </div>
        <button (click)="clearCaches()">Clear Caches</button>
        <button (click)="resetStats()">Reset Stats</button>
      </div>
    </div>
  `,
  styles: [`
    .canvas-container {
      position: relative;
      width: 100%;
      height: 600px;
      border: 1px solid #ccc;
    }
    .konva-container {
      width: 70%;
      height: 100%;
      float: left;
    }
    .performance-panel {
      width: 30%;
      height: 100%;
      float: right;
      padding: 20px;
      background: #f8f9fa;
      border-left: 1px solid #ccc;
    }
    .metric {
      display: flex;
      justify-content: space-between;
      margin-bottom: 10px;
      padding: 5px 0;
      border-bottom: 1px solid #eee;
    }
    button {
      margin: 5px 0;
      padding: 8px 16px;
      background: #007bff;
      color: white;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      width: 100%;
    }
    button:hover {
      background: #0056b3;
    }
  `]
})
export class RulerPerformanceMonitorExample implements AfterViewInit, OnDestroy {
  @ViewChild('canvasContainer', { static: true }) canvasContainer!: ElementRef<HTMLDivElement>;
  @ViewChild(RulerCanvasComponent) rulerComponent!: RulerCanvasComponent;

  // Konva objects
  private stage!: Konva.Stage;
  rulerLayer!: Konva.Layer;

  // Component state
  patternSettings: PatternSettings = CANVAS_CONSTANTS.DEFAULT_SETTINGS;
  canvasState = {
    scale: 1,
    offsetX: 0,
    offsetY: 0,
    panOffsetX: 0,
    panOffsetY: 0,
    width: 800,
    height: 600
  };

  // Performance tracking
  performanceStats = {
    cacheSize: 0,
    lastRenderTime: 0,
    renderCount: 0
  };

  private totalRenderTime = 0;
  private monitoringInterval?: number;

  get averageRenderTime(): number {
    return this.performanceStats.renderCount > 0 
      ? this.totalRenderTime / this.performanceStats.renderCount 
      : 0;
  }

  ngAfterViewInit(): void {
    this.initializeCanvas();
    this.startPerformanceMonitoring();
  }

  ngOnDestroy(): void {
    this.stopPerformanceMonitoring();
    this.cleanup();
  }

  private initializeCanvas(): void {
    const container = this.canvasContainer.nativeElement;
    
    this.stage = new Konva.Stage({
      container: container,
      width: container.offsetWidth * 0.7, // 70% width for canvas
      height: container.offsetHeight,
      draggable: true
    });

    this.rulerLayer = new Konva.Layer();
    this.stage.add(this.rulerLayer);

    this.canvasState = {
      ...this.canvasState,
      width: container.offsetWidth * 0.7,
      height: container.offsetHeight
    };
  }

  private startPerformanceMonitoring(): void {
    this.monitoringInterval = window.setInterval(() => {
      if (this.rulerComponent) {
        const newStats = this.rulerComponent.getCacheStats();
        
        // Track total render time for average calculation
        if (newStats.renderCount > this.performanceStats.renderCount) {
          this.totalRenderTime += newStats.lastRenderTime;
        }
        
        this.performanceStats = newStats;
      }
    }, 100); // Update every 100ms
  }

  private stopPerformanceMonitoring(): void {
    if (this.monitoringInterval) {
      clearInterval(this.monitoringInterval);
    }
  }

  private cleanup(): void {
    if (this.stage) {
      this.stage.destroy();
    }
  }

  // Public control methods
  public clearCaches(): void {
    if (this.rulerComponent) {
      this.rulerComponent.clearAllCaches();
    }
  }

  public resetStats(): void {
    this.totalRenderTime = 0;
    this.performanceStats = {
      cacheSize: 0,
      lastRenderTime: 0,
      renderCount: 0
    };
  }
}

/**
 * Utility functions for ruler integration
 */
export class RulerIntegrationUtils {
  /**
   * Calculate optimal canvas dimensions based on container
   */
  static calculateCanvasDimensions(container: HTMLElement): { width: number; height: number } {
    return {
      width: container.offsetWidth,
      height: container.offsetHeight
    };
  }

  /**
   * Create synchronized event handlers for multiple canvas components
   */
  static createSynchronizedEventHandlers(
    stage: Konva.Stage,
    canvasStateUpdater: (updates: any) => void
  ) {
    return {
      onWheel: (e: Konva.KonvaEventObject<WheelEvent>) => {
        e.evt.preventDefault();
        
        const scaleBy = 1.1;
        const oldScale = stage.scaleX();
        const pointer = stage.getPointerPosition();
        
        if (!pointer) return;

        const mousePointTo = {
          x: (pointer.x - stage.x()) / oldScale,
          y: (pointer.y - stage.y()) / oldScale,
        };

        const newScale = e.evt.deltaY > 0 ? oldScale * scaleBy : oldScale / scaleBy;
        
        canvasStateUpdater({
          scale: newScale,
          panOffsetX: pointer.x - mousePointTo.x * newScale,
          panOffsetY: pointer.y - mousePointTo.y * newScale
        });

        stage.scale({ x: newScale, y: newScale });
        stage.position({
          x: pointer.x - mousePointTo.x * newScale,
          y: pointer.y - mousePointTo.y * newScale
        });
        
        stage.batchDraw();
      },

      onDragMove: () => {
        const position = stage.position();
        canvasStateUpdater({
          panOffsetX: position.x,
          panOffsetY: position.y
        });
      }
    };
  }

  /**
   * Validate canvas state for ruler rendering
   */
  static validateCanvasState(canvasState: any): boolean {
    return (
      canvasState &&
      typeof canvasState.scale === 'number' &&
      typeof canvasState.width === 'number' &&
      typeof canvasState.height === 'number' &&
      canvasState.scale > 0 &&
      canvasState.width > 0 &&
      canvasState.height > 0
    );
  }
}