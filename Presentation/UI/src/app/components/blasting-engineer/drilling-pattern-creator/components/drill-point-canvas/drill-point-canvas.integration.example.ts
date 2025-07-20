import { Component, ViewChild, ElementRef, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import Konva from 'konva';
import { DrillPointCanvasComponent } from './drill-point-canvas.component';
import { GridCanvasComponent } from '../grid-canvas/grid-canvas.component';
import { RulerCanvasComponent } from '../ruler-canvas/ruler-canvas.component';
import { DrillPoint, PatternSettings, CanvasPosition } from '../../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../../constants/canvas.constants';

/**
 * Integration example showing how to use DrillPointCanvasComponent
 * together with other canvas components in a complete drilling pattern creator.
 * 
 * This example demonstrates:
 * - Setting up multiple canvas components together
 * - Coordinating state between components
 * - Handling user interactions across components
 * - Managing canvas lifecycle and cleanup
 */
@Component({
  selector: 'app-drill-point-integration-example',
  standalone: true,
  imports: [
    CommonModule,
    DrillPointCanvasComponent,
    GridCanvasComponent,
    RulerCanvasComponent
  ],
  template: `
    <div class="canvas-container">
      <div #canvasContainer class="konva-container"></div>
      
      <div class="controls">
        <div class="mode-controls">
          <button 
            [class.active]="isHolePlacementMode"
            (click)="toggleHolePlacementMode()">
            {{ isHolePlacementMode ? 'Exit' : 'Enter' }} Hole Placement Mode
          </button>
          
          <button 
            [class.active]="isPreciseMode"
            (click)="togglePreciseMode()">
            {{ isPreciseMode ? 'Disable' : 'Enable' }} Precise Mode
          </button>
        </div>
        
        <div class="pattern-controls">
          <label>
            Spacing: 
            <input 
              type="number" 
              [(ngModel)]="settings.spacing" 
              (change)="onSettingsChange()"
              min="1" 
              max="10" 
              step="0.1">
          </label>
          
          <label>
            Burden: 
            <input 
              type="number" 
              [(ngModel)]="settings.burden" 
              (change)="onSettingsChange()"
              min="1" 
              max="10" 
              step="0.1">
          </label>
          
          <label>
            Depth: 
            <input 
              type="number" 
              [(ngModel)]="settings.depth" 
              (change)="onSettingsChange()"
              min="1" 
              max="50" 
              step="0.5">
          </label>
        </div>
        
        <div class="action-controls">
          <button (click)="clearAllPoints()" [disabled]="drillPoints.length === 0">
            Clear All Points ({{ drillPoints.length }})
          </button>
          
          <button (click)="deleteSelectedPoint()" [disabled]="!selectedPoint">
            Delete Selected Point
          </button>
          
          <button (click)="generateGridPattern()">
            Generate 5x5 Grid Pattern
          </button>
        </div>
      </div>
      
      <div class="status-bar">
        <div class="point-info">
          <span>Points: {{ drillPoints.length }}</span>
          <span *ngIf="selectedPoint">
            | Selected: ({{ selectedPoint.x.toFixed(2) }}, {{ selectedPoint.y.toFixed(2) }}) 
            Depth: {{ selectedPoint.depth }}m
          </span>
        </div>
        
        <div class="performance-info">
          <span>Render Time: {{ performanceStats.lastRenderTime.toFixed(2) }}ms</span>
          <span>| Renders: {{ performanceStats.renderCount }}</span>
        </div>
        
        <div class="canvas-info">
          <span>Scale: {{ canvasState.scale.toFixed(2) }}x</span>
          <span>| Pan: ({{ canvasState.panOffsetX }}, {{ canvasState.panOffsetY }})</span>
        </div>
      </div>
      
      <div class="messages" *ngIf="statusMessage">
        <div class="message" [class]="messageType">
          {{ statusMessage }}
        </div>
      </div>
    </div>

    <!-- Hidden canvas components - they render directly to Konva layers -->
    <app-grid-canvas
      [layer]="gridLayer"
      [settings]="settings"
      [canvasState]="canvasState"
      [isPreciseMode]="isPreciseMode">
    </app-grid-canvas>

    <app-ruler-canvas
      [layer]="rulerLayer"
      [settings]="settings"
      [canvasState]="canvasState">
    </app-ruler-canvas>

    <app-drill-point-canvas
      [layer]="pointsLayer"
      [drillPoints]="drillPoints"
      [selectedPoint]="selectedPoint"
      [settings]="settings"
      [canvasState]="canvasState"
      [isHolePlacementMode]="isHolePlacementMode"
      [isPreciseMode]="isPreciseMode"
      (pointSelected)="onPointSelected($event)"
      (pointMoved)="onPointMoved($event)"
      (pointPlaced)="onPointPlaced($event)"
      (duplicateDetected)="onDuplicateDetected($event)">
    </app-drill-point-canvas>
  `,
  styles: [`
    .canvas-container {
      display: flex;
      flex-direction: column;
      height: 100vh;
      font-family: Arial, sans-serif;
    }

    .konva-container {
      flex: 1;
      border: 1px solid #ccc;
      position: relative;
    }

    .controls {
      padding: 10px;
      background: #f5f5f5;
      border-bottom: 1px solid #ccc;
      display: flex;
      gap: 20px;
      flex-wrap: wrap;
    }

    .mode-controls, .pattern-controls, .action-controls {
      display: flex;
      gap: 10px;
      align-items: center;
    }

    button {
      padding: 8px 16px;
      border: 1px solid #ccc;
      background: white;
      cursor: pointer;
      border-radius: 4px;
    }

    button:hover {
      background: #f0f0f0;
    }

    button.active {
      background: #007bff;
      color: white;
    }

    button:disabled {
      opacity: 0.5;
      cursor: not-allowed;
    }

    label {
      display: flex;
      align-items: center;
      gap: 5px;
    }

    input {
      padding: 4px 8px;
      border: 1px solid #ccc;
      border-radius: 4px;
      width: 80px;
    }

    .status-bar {
      padding: 8px 10px;
      background: #e9ecef;
      border-top: 1px solid #ccc;
      display: flex;
      justify-content: space-between;
      font-size: 12px;
      color: #666;
    }

    .messages {
      position: absolute;
      top: 10px;
      right: 10px;
      z-index: 1000;
    }

    .message {
      padding: 10px 15px;
      border-radius: 4px;
      margin-bottom: 5px;
      font-weight: bold;
    }

    .message.success {
      background: #d4edda;
      color: #155724;
      border: 1px solid #c3e6cb;
    }

    .message.error {
      background: #f8d7da;
      color: #721c24;
      border: 1px solid #f5c6cb;
    }

    .message.warning {
      background: #fff3cd;
      color: #856404;
      border: 1px solid #ffeaa7;
    }
  `]
})
export class DrillPointIntegrationExample implements OnInit, OnDestroy {
  @ViewChild('canvasContainer', { static: true }) canvasContainer!: ElementRef<HTMLDivElement>;
  @ViewChild(DrillPointCanvasComponent) drillPointCanvas!: DrillPointCanvasComponent;
  @ViewChild(GridCanvasComponent) gridCanvas!: GridCanvasComponent;
  @ViewChild(RulerCanvasComponent) rulerCanvas!: RulerCanvasComponent;

  // Konva objects
  stage!: Konva.Stage;
  gridLayer!: Konva.Layer;
  rulerLayer!: Konva.Layer;
  pointsLayer!: Konva.Layer;

  // Component state
  drillPoints: DrillPoint[] = [];
  selectedPoint: DrillPoint | null = null;
  settings: PatternSettings = {
    spacing: 4,
    burden: 3,
    depth: 12
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

  // UI state
  isHolePlacementMode = false;
  isPreciseMode = false;
  statusMessage = '';
  messageType: 'success' | 'error' | 'warning' = 'success';
  performanceStats = { lastRenderTime: 0, renderCount: 0 };

  // Interaction state
  private isDragging = false;
  private lastPointerPosition: { x: number; y: number } | null = null;

  ngOnInit(): void {
    this.initializeCanvas();
    this.setupCanvasInteractions();
    this.renderAllLayers();
    
    // Show welcome message
    this.showMessage('Canvas initialized. Click "Enter Hole Placement Mode" to start adding points.', 'success');
  }

  ngOnDestroy(): void {
    this.stage?.destroy();
  }

  /**
   * Initialize Konva stage and layers
   */
  private initializeCanvas(): void {
    const container = this.canvasContainer.nativeElement;
    const rect = container.getBoundingClientRect();
    
    this.canvasState.width = rect.width;
    this.canvasState.height = rect.height;

    this.stage = new Konva.Stage({
      container: container,
      width: this.canvasState.width,
      height: this.canvasState.height
    });

    // Create layers in correct order (bottom to top)
    this.gridLayer = new Konva.Layer({ name: 'grid-layer' });
    this.rulerLayer = new Konva.Layer({ name: 'ruler-layer' });
    this.pointsLayer = new Konva.Layer({ name: 'points-layer' });

    this.stage.add(this.gridLayer);
    this.stage.add(this.rulerLayer);
    this.stage.add(this.pointsLayer);
  }

  /**
   * Set up canvas interactions (pan, zoom, etc.)
   */
  private setupCanvasInteractions(): void {
    // Mouse wheel zoom
    this.stage.on('wheel', (e) => {
      e.evt.preventDefault();
      
      const oldScale = this.canvasState.scale;
      const pointer = this.stage.getPointerPosition();
      
      if (!pointer) return;

      const mousePointTo = {
        x: (pointer.x - this.canvasState.panOffsetX) / oldScale,
        y: (pointer.y - this.canvasState.panOffsetY) / oldScale
      };

      const direction = e.evt.deltaY > 0 ? -1 : 1;
      const scaleBy = 1.1;
      const newScale = direction > 0 ? oldScale * scaleBy : oldScale / scaleBy;

      // Limit zoom range
      this.canvasState.scale = Math.max(0.1, Math.min(5, newScale));

      const newPos = {
        x: pointer.x - mousePointTo.x * this.canvasState.scale,
        y: pointer.y - mousePointTo.y * this.canvasState.scale
      };

      this.canvasState.panOffsetX = newPos.x;
      this.canvasState.panOffsetY = newPos.y;

      this.renderAllLayers();
    });

    // Pan with middle mouse or right mouse
    this.stage.on('mousedown', (e) => {
      if (e.evt.button === 1 || e.evt.button === 2) { // Middle or right mouse
        this.isDragging = true;
        this.lastPointerPosition = this.stage.getPointerPosition();
        this.stage.container().style.cursor = 'grabbing';
      }
    });

    this.stage.on('mousemove', (e) => {
      if (this.isDragging && this.lastPointerPosition) {
        const currentPosition = this.stage.getPointerPosition();
        if (currentPosition) {
          const dx = currentPosition.x - this.lastPointerPosition.x;
          const dy = currentPosition.y - this.lastPointerPosition.y;

          this.canvasState.panOffsetX += dx;
          this.canvasState.panOffsetY += dy;

          this.lastPointerPosition = currentPosition;
          this.renderAllLayers();
        }
      }
    });

    this.stage.on('mouseup', () => {
      this.isDragging = false;
      this.lastPointerPosition = null;
      this.stage.container().style.cursor = 'default';
    });

    // Prevent context menu on right click
    this.stage.container().addEventListener('contextmenu', (e) => {
      e.preventDefault();
    });

    // Handle window resize
    window.addEventListener('resize', () => {
      this.handleResize();
    });
  }

  /**
   * Handle window resize
   */
  private handleResize(): void {
    const container = this.canvasContainer.nativeElement;
    const rect = container.getBoundingClientRect();
    
    this.canvasState.width = rect.width;
    this.canvasState.height = rect.height;
    
    this.stage.width(this.canvasState.width);
    this.stage.height(this.canvasState.height);
    
    this.renderAllLayers();
  }

  /**
   * Render all canvas layers
   */
  private renderAllLayers(): void {
    // Update performance stats from drill point canvas
    if (this.drillPointCanvas) {
      this.performanceStats = this.drillPointCanvas.getPerformanceStats();
    }

    // Trigger re-render of all components
    // The components will automatically re-render due to input changes
  }

  /**
   * Toggle hole placement mode
   */
  toggleHolePlacementMode(): void {
    this.isHolePlacementMode = !this.isHolePlacementMode;
    const mode = this.isHolePlacementMode ? 'enabled' : 'disabled';
    this.showMessage(`Hole placement mode ${mode}. ${this.isHolePlacementMode ? 'Click on canvas to place points.' : ''}`, 'success');
  }

  /**
   * Toggle precise mode
   */
  togglePreciseMode(): void {
    this.isPreciseMode = !this.isPreciseMode;
    const mode = this.isPreciseMode ? 'enabled' : 'disabled';
    this.showMessage(`Precise mode ${mode}. ${this.isPreciseMode ? 'Grid intersections are now visible.' : ''}`, 'success');
  }

  /**
   * Handle settings change
   */
  onSettingsChange(): void {
    this.showMessage('Pattern settings updated. Existing points maintain their individual settings.', 'success');
  }

  /**
   * Handle point selection
   */
  onPointSelected(point: DrillPoint | null): void {
    this.selectedPoint = point;
    if (point) {
      this.showMessage(`Point selected at (${point.x.toFixed(2)}, ${point.y.toFixed(2)}) with depth ${point.depth}m`, 'success');
    }
  }

  /**
   * Handle point movement
   */
  onPointMoved(event: { point: DrillPoint; newPosition: CanvasPosition }): void {
    // Update the point in our array
    const pointIndex = this.drillPoints.findIndex(p => p.id === event.point.id);
    if (pointIndex >= 0) {
      this.drillPoints[pointIndex] = {
        ...this.drillPoints[pointIndex],
        x: event.newPosition.x,
        y: event.newPosition.y
      };
      
      this.showMessage(`Point moved to (${event.newPosition.x.toFixed(2)}, ${event.newPosition.y.toFixed(2)})`, 'success');
    }
  }

  /**
   * Handle point placement
   */
  onPointPlaced(position: CanvasPosition): void {
    // The component handles validation, we just need to add the point
    const newPoint = this.drillPointCanvas.addPointAtPosition(position);
    if (newPoint) {
      this.drillPoints = [...this.drillPoints, newPoint];
      this.showMessage(`Point placed at (${newPoint.x.toFixed(2)}, ${newPoint.y.toFixed(2)})`, 'success');
    }
  }

  /**
   * Handle duplicate detection
   */
  onDuplicateDetected(event: { message: string; existingPoint: DrillPoint }): void {
    this.showMessage(event.message, 'warning');
    
    // Briefly highlight the existing point
    this.selectedPoint = event.existingPoint;
    setTimeout(() => {
      if (this.selectedPoint === event.existingPoint) {
        this.selectedPoint = null;
      }
    }, 2000);
  }

  /**
   * Clear all points
   */
  clearAllPoints(): void {
    this.drillPoints = [];
    this.selectedPoint = null;
    this.showMessage('All points cleared', 'success');
  }

  /**
   * Delete selected point
   */
  deleteSelectedPoint(): void {
    if (this.selectedPoint) {
      this.drillPoints = this.drillPoints.filter(p => p.id !== this.selectedPoint!.id);
      const deletedPoint = this.selectedPoint;
      this.selectedPoint = null;
      this.showMessage(`Point at (${deletedPoint.x.toFixed(2)}, ${deletedPoint.y.toFixed(2)}) deleted`, 'success');
    }
  }

  /**
   * Generate a 5x5 grid pattern
   */
  generateGridPattern(): void {
    const gridPoints: DrillPoint[] = [];
    let pointId = 1;

    // Clear existing points
    this.drillPoints = [];
    this.selectedPoint = null;

    // Generate 5x5 grid
    for (let row = 0; row < 5; row++) {
      for (let col = 0; col < 5; col++) {
        const point: DrillPoint = {
          id: `grid-${pointId++}`,
          x: 5 + (col * this.settings.spacing),
          y: 5 + (row * this.settings.burden),
          depth: this.settings.depth + (Math.random() > 0.7 ? Math.random() * 4 - 2 : 0), // Some random depth variation
          spacing: this.settings.spacing,
          burden: this.settings.burden
        };
        gridPoints.push(point);
      }
    }

    this.drillPoints = gridPoints;
    this.showMessage(`Generated 5x5 grid pattern with ${gridPoints.length} points`, 'success');
  }

  /**
   * Show status message
   */
  private showMessage(message: string, type: 'success' | 'error' | 'warning'): void {
    this.statusMessage = message;
    this.messageType = type;

    // Auto-hide message after 3 seconds
    setTimeout(() => {
      if (this.statusMessage === message) {
        this.statusMessage = '';
      }
    }, 3000);
  }
}

/**
 * Usage Instructions:
 * 
 * 1. Import this component in your module or use it standalone
 * 2. Add it to your template: <app-drill-point-integration-example></app-drill-point-integration-example>
 * 3. The component demonstrates:
 *    - Canvas setup with multiple layers
 *    - Coordinated rendering of grid, ruler, and points
 *    - Interactive point placement and selection
 *    - Pan and zoom functionality
 *    - Settings management
 *    - Error handling and user feedback
 * 
 * Key Integration Points:
 * - All canvas components share the same canvasState for consistency
 * - Event handling is coordinated through the parent component
 * - Performance monitoring shows render times and statistics
 * - Memory management through proper cleanup in ngOnDestroy
 * 
 * Customization Options:
 * - Modify the template to change UI layout
 * - Add more controls for advanced features
 * - Integrate with your application's state management
 * - Add persistence/loading functionality
 * - Customize styling and themes
 */