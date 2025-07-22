import {
  Component,
  Input,
  Output,
  EventEmitter,
  ViewChild,
  ElementRef,
  OnInit,
  OnDestroy,
  AfterViewInit,
  OnChanges,
  SimpleChanges,
  ChangeDetectionStrategy,
  inject,
  NgZone
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { takeUntil, debounceTime } from 'rxjs/operators';
import { fromEvent, merge } from 'rxjs';
import Konva from 'konva';

// Import specialized canvas components
// import { GridCanvasComponent } from '../grid-canvas/grid-canvas.component';
// import { RulerCanvasComponent } from '../ruler-canvas/ruler-canvas.component';
// import { DrillPointCanvasComponent } from '../drill-point-canvas/drill-point-canvas.component';

import { BasePatternComponentClass } from '../../base/base-pattern.component';
import { PatternCanvasContract } from '../../contracts/component.contracts';
import {
  PatternSettings,
  DrillPoint
} from '../../models/drill-point.model';
import {
  CanvasState,
  UIState,
  PlacePointEvent,
  MovePointEvent
} from '../../models/pattern-state.model';
import { CanvasConfig } from '../../tokens/injection.tokens';
import { CanvasManager } from '../../managers/canvas.manager';
import { Logger } from '../../utils/logger.util';
import { PatternEventBusService } from '../../services/pattern-event-bus.service';

/**
 * PatternCanvasComponent - Main canvas coordinator component
 * 
 * This component serves as the main canvas container and coordinator for all canvas operations.
 * It manages the Konva stage, handles canvas-level events (zoom, pan, resize), and coordinates
 * child canvas components (grid, ruler, drill points).
 * 
 * Responsibilities:
 * - Initialize and manage Konva stage and layers
 * - Handle canvas lifecycle (init, update, destroy)
 * - Coordinate canvas-level events (zoom, pan, resize)
 * - Manage canvas state and transformations
 * - Provide proper cleanup and memory management
 */
@Component({
  selector: 'app-pattern-canvas',
  standalone: true,
  imports: [
    CommonModule
    // Sub-components will be added when they're ready
    // RulerCanvasComponent,
    // GridCanvasComponent,
    // DrillPointCanvasComponent
  ],
  templateUrl: './pattern-canvas.component.html',
  styleUrls: ['./pattern-canvas.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PatternCanvasComponent
  extends BasePatternComponentClass
  implements PatternCanvasContract, OnInit, AfterViewInit, OnDestroy, OnChanges {

  protected override readonly componentName = 'PatternCanvasComponent';

  // Injected services
  private readonly ngZone = inject(NgZone);
  private readonly canvasManager = inject(CanvasManager);

  // Optional services (not implemented yet) - using fallback implementations
  private readonly performanceMonitor: any = this.createPerformanceMonitorFallback();
  private readonly canvasConfig: CanvasConfig = {
    defaultWidth: 800,
    defaultHeight: 600,
    minZoom: 0.1,
    maxZoom: 10,
    zoomStep: 0.1,
    panSensitivity: 1,
    gridSpacing: 1,
    gridColor: '#e0e0e0',
    backgroundColor: '#ffffff',
    enableAntiAliasing: true,
    pixelRatio: window.devicePixelRatio || 1
  };

  // Template references
  @ViewChild('canvasContainer', { static: true })
  canvasContainer!: ElementRef<HTMLDivElement>;

  // Sub-component references
  // @ViewChild(GridCanvasComponent) gridCanvas!: GridCanvasComponent;
  // @ViewChild(RulerCanvasComponent) rulerCanvas!: RulerCanvasComponent;
  // @ViewChild(DrillPointCanvasComponent) drillPointCanvas!: DrillPointCanvasComponent;

  // Inputs
  @Input() settings!: PatternSettings;
  @Input() drillPoints: DrillPoint[] = [];
  @Input() selectedPoint: DrillPoint | null = null;
  @Input() canvasState!: CanvasState;
  @Input() uiState!: UIState;

  // Outputs
  @Output() pointPlaced = new EventEmitter<PlacePointEvent>();
  @Output() pointSelected = new EventEmitter<DrillPoint>();
  @Output() pointMoved = new EventEmitter<MovePointEvent>();
  @Output() canvasStateChange = new EventEmitter<CanvasState>();

  // Component state
  public isInitializing = false;
  public hasError = false;
  public errorMessage = '';
  public hoveredPoint: DrillPoint | null = null;
  public tooltipPosition = { x: 0, y: 0 };

  // Konva layers for sub-components
  public gridLayer: Konva.Layer | null = null;
  public rulerLayer: Konva.Layer | null = null;
  public pointsLayer: Konva.Layer | null = null;

  // Private properties
  private stage: Konva.Stage | null = null;
  private resizeObserver: ResizeObserver | null = null;
  private initializationTimerId: string | null = null;

  override ngOnInit(): void {
    super.ngOnInit();
    this.setupPerformanceMonitoring();
    this.setupReactiveUpdates();
  }

  /**
   * Setup reactive updates for drill points and settings changes
   */
  private setupReactiveUpdates(): void {
    // Sub-components will handle their own reactive updates
    // This component coordinates the overall canvas state
  }

  /**
   * Initialize sub-components after canvas is ready
   */
  private initializeSubComponents(): void {
    // Sub-components will initialize themselves when their inputs are available
    // This method can be used for any coordination logic needed
    Logger.info('Sub-components ready for initialization');
  }

  ngAfterViewInit(): void {
    // Initialize canvas after view is ready
    this.ngZone.runOutsideAngular(() => {
      this.initializeCanvas();
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    Logger.info('Canvas input changes', changes);

    // Re-render when drill points change
    if (changes['drillPoints'] && this.stage) {
      Logger.info('Drill points changed, re-rendering', {
        newCount: this.drillPoints?.length || 0
      });
      this.renderDrillPoints();
    }

    // Re-render grid when settings change
    if (changes['settings'] && this.stage) {
      Logger.info('Settings changed, re-rendering grid');
      this.renderBasicGrid();
    }

    // Update selected point highlighting
    if (changes['selectedPoint'] && this.stage) {
      Logger.info('Selected point changed', this.selectedPoint);
      this.renderDrillPoints();
    }

    // Update UI state
    if (changes['uiState']) {
      Logger.info('UI state changed', this.uiState);
      this.updateCanvasCursor();
    }
  }

  /**
   * Update canvas cursor based on UI state
   */
  private updateCanvasCursor(): void {
    if (!this.canvasContainer?.nativeElement) return;

    const cursor = this.uiState?.isHolePlacementMode ? 'crosshair' : 'default';
    this.canvasContainer.nativeElement.style.cursor = cursor;

    if (this.stage) {
      this.canvasManager.setCursor(cursor);
    }
  }

  /**
   * Handle point selection from drill point canvas
   */
  public onPointSelected(point: DrillPoint): void {
    Logger.info('Point selected in canvas', point);
    this.eventBus.emit({ type: 'POINT_SELECTED', payload: point });
  }

  public onPointMoved(event: MovePointEvent): void {
    Logger.info('Point moved in canvas', event);
    this.eventBus.emit({ type: 'POINT_MOVED', payload: event });
  }

  public onPointPlaced(event: PlacePointEvent): void {
    Logger.info('Point placed in canvas', event);
    this.eventBus.emit({ type: 'POINT_PLACED', payload: event });
  }

  // Add similar for canvasStateChange if needed
  override ngOnDestroy(): void {
    this.destroyCanvas();
    super.ngOnDestroy();
  }

  /**
   * Initialize the canvas and set up event handlers
   */
  public async initializeCanvas(): Promise<void> {
    if (this.stage && this.canvasManager.isReady()) {
      Logger.warn('Canvas already initialized, skipping');
      return;
    }

    this.initializationTimerId = this.performanceMonitor?.startTimer('canvas-initialization') || null;
    this.isInitializing = true;
    this.hasError = false;

    try {
      Logger.info('Initializing canvas...', {
        containerElement: !!this.canvasContainer?.nativeElement,
        containerDimensions: this.canvasContainer?.nativeElement ? {
          width: this.canvasContainer.nativeElement.clientWidth,
          height: this.canvasContainer.nativeElement.clientHeight
        } : null
      });

      if (!this.canvasContainer?.nativeElement) {
        throw new Error('Canvas container element not found');
      }

      // Initialize canvas manager with the container element
      this.canvasManager.initialize(this.canvasContainer.nativeElement);
      this.stage = this.canvasManager.getStage();

      if (!this.stage) {
        throw new Error('Failed to create Konva stage');
      }

      // Get layers from canvas manager
      const layers = this.canvasManager.getLayers();
      this.gridLayer = layers.grid;
      this.rulerLayer = layers.rulers;
      this.pointsLayer = layers.points;

      Logger.info('Konva stage and layers created successfully', {
        stageWidth: this.stage.width(),
        stageHeight: this.stage.height(),
        hasGridLayer: !!this.gridLayer,
        hasRulerLayer: !!this.rulerLayer,
        hasPointsLayer: !!this.pointsLayer
      });

      // Set up event handlers
      this.setupCanvasEventHandlers();

      // Set up resize observer
      this.setupResizeObserver();
      
      // Position the stage so that (0,0) is at the top-left corner
      // This ensures the grid grows from left and bottom
      const containerWidth = this.canvasContainer.nativeElement.clientWidth;
      const containerHeight = this.canvasContainer.nativeElement.clientHeight;
      
      // Set initial position to place origin at top-left with a small margin
      this.stage.position({ x: 50, y: 50 });
      this.stage.batchDraw();

      // Render initial content (will be replaced by sub-components later)
      this.renderBasicGrid();
      this.renderDrillPoints();

      // Update canvas state
      this.updateCanvasState({
        isInitialized: true,
        scale: 1,
        panOffsetX: 50,
        panOffsetY: 50
      });

      Logger.info('Canvas initialized successfully');

    } catch (error) {
      this.handleInitializationError(error as Error);
    } finally {
      this.isInitializing = false;

      if (this.initializationTimerId && this.performanceMonitor) {
        this.performanceMonitor.endTimer(this.initializationTimerId);
        this.initializationTimerId = null;
      }
    }
  }

  /**
   * Resize canvas to fit container
   */
  public resizeCanvas(width: number, height: number): void {
    try {
      if (!this.stage) {
        Logger.warn('Cannot resize canvas - no stage');
        return;
      }

      const timerId = this.performanceMonitor?.startTimer('canvas-resize');

      Logger.info('Resizing canvas', {
        requestedWidth: width,
        requestedHeight: height,
        currentWidth: this.stage.width(),
        currentHeight: this.stage.height()
      });

      this.canvasManager.resize();

      // Re-render grid and drill points after resize
      this.renderBasicGrid();
      this.renderDrillPoints();

      Logger.info('Canvas resized successfully', {
        newWidth: this.stage.width(),
        newHeight: this.stage.height()
      });

      if (timerId && this.performanceMonitor) {
        this.performanceMonitor.endTimer(timerId);
      }

    } catch (error) {
      this.handleError(error as Error, 'resizeCanvas');
    }
  }

  /**
   * Export canvas as image
   */
  public async exportAsImage(): Promise<Blob> {
    if (!this.stage) {
      throw new Error('Canvas not initialized');
    }

    try {
      const timerId = this.performanceMonitor?.startTimer('canvas-export');

      const dataURL = this.stage.toDataURL({
        mimeType: 'image/png',
        quality: 1,
        pixelRatio: 2
      });

      // Convert data URL to blob
      const response = await fetch(dataURL);
      const blob = await response.blob();

      if (timerId && this.performanceMonitor) {
        this.performanceMonitor.endTimer(timerId);
      }

      return blob;

    } catch (error) {
      this.handleError(error as Error, 'exportAsImage');
      throw error;
    }
  }

  /**
   * Fit canvas content to screen while maintaining origin at top-left
   */
  public fitToScreen(): void {
    try {
      if (!this.canvasState?.isInitialized) {
        Logger.warn('Cannot fit to screen - canvas not initialized');
        return;
      }

      const timerId = this.performanceMonitor?.startTimer('fit-to-screen');

      // Calculate bounds of all drill points
      if (this.drillPoints.length === 0) {
        this.resetView();
        return;
      }

      const bounds = this.calculateContentBounds();
      const containerRect = this.canvasContainer.nativeElement.getBoundingClientRect();

      // Calculate scale to fit content with padding
      const padding = 50;
      const scaleX = (containerRect.width - padding * 2) / bounds.width;
      const scaleY = (containerRect.height - padding * 2) / bounds.height;
      const scale = Math.min(scaleX, scaleY, this.canvasConfig.maxZoom);

      // Apply transformation to stage - position to keep origin visible
      if (this.stage) {
        this.stage.scale({ x: scale, y: scale });
        
        // Position the stage to keep the origin (0,0) at the top-left with padding
        // and ensure all content is visible
        const stagePos = {
          x: Math.min(padding, padding - bounds.x * scale),
          y: Math.min(padding, padding - bounds.y * scale)
        };
        
        this.stage.position(stagePos);
        this.stage.batchDraw();
      }

      this.updateCanvasState({
        scale,
        panOffsetX: this.stage?.x() || 0,
        panOffsetY: this.stage?.y() || 0
      });
      
      // Re-render grid with new view
      this.renderBasicGrid();

      if (timerId && this.performanceMonitor) {
        this.performanceMonitor.endTimer(timerId);
      }

      Logger.info('Canvas fitted to screen', { 
        scale, 
        position: { x: this.stage?.x(), y: this.stage?.y() } 
      });

    } catch (error) {
      this.handleError(error as Error, 'fitToScreen');
    }
  }

  /**
   * Reset canvas view to default
   */
  public resetView(): void {
    try {
      if (!this.canvasState?.isInitialized) {
        Logger.warn('Cannot reset view - canvas not initialized');
        return;
      }

      if (this.stage) {
        this.stage.scale({ x: 1, y: 1 });
        // Position the stage so that (0,0) is at the top-left corner with a small margin
        this.stage.position({ x: 50, y: 50 });
        this.stage.batchDraw();
      }

      this.updateCanvasState({
        scale: 1,
        panOffsetX: 50,
        panOffsetY: 50
      });
      
      // Re-render grid with default view
      this.renderBasicGrid();

      Logger.info('Canvas view reset');

    } catch (error) {
      this.handleError(error as Error, 'resetView');
    }
  }

  /**
   * Handle context menu (right-click)
   */
  public onContextMenu(event: MouseEvent): void {
    event.preventDefault();
    // Context menu functionality can be added here
  }

  /**
   * Retry canvas initialization after error
   */
  public retryInitialization(): void {
    this.hasError = false;
    this.errorMessage = '';
    this.initializeCanvas();
  }

  /**
   * Destroy canvas and clean up resources
   */
  private destroyCanvas(): void {
    try {
      Logger.info('Destroying canvas...');

      // Clean up resize observer
      if (this.resizeObserver) {
        this.resizeObserver.disconnect();
        this.resizeObserver = null;
      }

      // Destroy canvas manager
      this.canvasManager.destroy();

      // Clear stage reference
      this.stage = null;

      // Update canvas state
      this.updateCanvasState({
        isInitialized: false,
        isDragging: false,
        isPanning: false
      });

      Logger.info('Canvas destroyed successfully');

    } catch (error) {
      this.handleError(error as Error, 'destroyCanvas');
    }
  }

  /**
   * Set up canvas event handlers
   */
  private setupCanvasEventHandlers(): void {
    if (!this.stage) return;

    // Mouse/touch events
    this.stage.on('click tap', this.onCanvasClick.bind(this));
    this.stage.on('dragstart', this.onDragStart.bind(this));
    this.stage.on('dragmove', this.onDragMove.bind(this));
    this.stage.on('dragend', this.onDragEnd.bind(this));
    this.stage.on('wheel', this.onWheel.bind(this));

    // Keyboard events (handled at document level)
    this.ngZone.runOutsideAngular(() => {
      const keyboardEvents$ = merge(
        fromEvent<KeyboardEvent>(document, 'keydown'),
        fromEvent<KeyboardEvent>(document, 'keyup')
      ).pipe(
        takeUntil(this.destroy$),
        debounceTime(10)
      );

      keyboardEvents$.subscribe(event => {
        this.ngZone.run(() => this.onKeyboardEvent(event));
      });
    });
  }

  /**
   * Set up resize observer for responsive canvas
   */
  private setupResizeObserver(): void {
    if (!this.canvasContainer?.nativeElement) return;

    this.resizeObserver = new ResizeObserver(entries => {
      for (const entry of entries) {
        const { width, height } = entry.contentRect;
        this.ngZone.run(() => {
          this.resizeCanvas(width, height);
        });
      }
    });

    this.resizeObserver.observe(this.canvasContainer.nativeElement);
  }

  /**
   * Set up performance monitoring
   */
  private setupPerformanceMonitoring(): void {
    // Track memory usage
    this.performanceMonitor?.trackMemoryLeak(this.componentName);

    // Record component initialization
    this.performanceMonitor?.recordMetric('component-init', Date.now(), {
      component: this.componentName
    });
  }

  /**
   * Handle canvas click events
   */
  private onCanvasClick(event: any): void {
    try {
      const pos = this.canvasManager.getPointerPosition();
      if (!pos) return;

      Logger.info('Canvas clicked', { pos, isHolePlacementMode: this.uiState?.isHolePlacementMode });

      if (this.uiState?.isHolePlacementMode) {
        // Transform coordinates if needed (account for canvas scaling/panning)
        const transformedPos = this.transformScreenToCanvas(pos);

        // Place new drill point
        this.pointPlaced.emit({
          x: transformedPos.x,
          y: transformedPos.y,
          settings: this.settings
        });
      }

    } catch (error) {
      this.handleError(error as Error, 'onCanvasClick');
    }
  }

  /**
   * Transform screen coordinates to canvas coordinates
   */
  private transformScreenToCanvas(screenPos: { x: number; y: number }): { x: number; y: number } {
    if (!this.stage) return screenPos;

    // Account for stage transformations (scale, pan)
    const transform = this.stage.getAbsoluteTransform().copy();
    transform.invert();

    return transform.point(screenPos);
  }

  /**
   * Handle drag start events
   */
  private onDragStart(_event: any): void {
    this.updateCanvasState({ isDragging: true });
  }

  /**
   * Handle drag move events
   */
  private onDragMove(_event: any): void {
    if (!this.canvasState?.isDragging) return;

    const pos = this.canvasManager.getPointerPosition();
    if (pos) {
      this.updateCanvasState({
        panOffsetX: pos.x,
        panOffsetY: pos.y
      });
    }
  }

  /**
   * Handle drag end events
   */
  private onDragEnd(_event: any): void {
    this.updateCanvasState({ isDragging: false });
    
    // Re-render grid after panning to ensure it covers the visible area
    this.renderBasicGrid();
  }

  /**
   * Handle mouse wheel events for zooming while maintaining origin at top-left
   */
  private onWheel(event: any): void {
    event.evt.preventDefault();

    const scaleBy = 1.1;
    const stage = event.target.getStage();
    const pointer = stage.getPointerPosition();
    
    // Get the point under the mouse in world coordinates
    const mousePointTo = {
      x: (pointer.x - stage.x()) / stage.scaleX(),
      y: (pointer.y - stage.y()) / stage.scaleY()
    };

    const direction = event.evt.deltaY > 0 ? -1 : 1;
    const newScale = Math.max(
      this.canvasConfig.minZoom,
      Math.min(this.canvasConfig.maxZoom, stage.scaleX() * Math.pow(scaleBy, direction))
    );

    // Apply new scale
    stage.scale({ x: newScale, y: newScale });

    // Calculate new position to zoom toward mouse point
    const newPos = {
      x: pointer.x - mousePointTo.x * newScale,
      y: pointer.y - mousePointTo.y * newScale
    };
    
    // Ensure the origin stays visible - don't let it go too far off screen
    // This ensures the grid grows from left and bottom
    const minX = -newScale * 1000; // Allow some negative space
    const minY = -newScale * 1000; // Allow some negative space
    
    newPos.x = Math.max(newPos.x, 50); // Keep origin with some padding
    newPos.y = Math.max(newPos.y, 50); // Keep origin with some padding

    // Apply new position
    stage.position(newPos);
    stage.batchDraw();

    // Update canvas state
    this.updateCanvasState({
      scale: newScale,
      panOffsetX: newPos.x,
      panOffsetY: newPos.y
    });
    
    // Re-render grid when zoom level changes significantly
    const scaleChange = Math.abs(this.canvasState.scale - newScale) / this.canvasState.scale;
    if (scaleChange > 0.1) {
      this.renderBasicGrid();
    }
  }

  /**
   * Handle keyboard events
   */
  private onKeyboardEvent(_event: KeyboardEvent): void {
    // Keyboard shortcuts can be implemented here
    // For example: Ctrl+Z for undo, Ctrl+Y for redo, etc.
  }

  /**
   * Calculate bounds of all content
   */
  private calculateContentBounds(): { x: number; y: number; width: number; height: number } {
    if (this.drillPoints.length === 0) {
      return { x: 0, y: 0, width: 100, height: 100 };
    }

    const xs = this.drillPoints.map(p => p.x);
    const ys = this.drillPoints.map(p => p.y);

    const minX = Math.min(...xs);
    const maxX = Math.max(...xs);
    const minY = Math.min(...ys);
    const maxY = Math.max(...ys);

    return {
      x: minX,
      y: minY,
      width: maxX - minX || 100,
      height: maxY - minY || 100
    };
  }

  /**
   * Update canvas state and emit changes
   */
  private updateCanvasState(updates: Partial<CanvasState>): void {
    const newState = { ...this.canvasState, ...updates };
    this.canvasStateChange.emit(newState);
  }

  /**
   * Handle initialization errors
   */
  private handleInitializationError(error: Error): void {
    this.hasError = true;
    this.errorMessage = error.message || 'Failed to initialize canvas';

    this.errorHandler.handleComponentError(
      this.componentName,
      error,
      { phase: 'initialization' }
    );

    Logger.error('Canvas initialization failed', error);
  }

  /**
   * Override base error handling for canvas-specific errors
   */
  protected override handleError(error: Error, context: string): void {
    super.handleError(error, context);

    // Canvas-specific error handling
    if (context.includes('canvas') || context.includes('Canvas')) {
      this.errorHandler.showUserError(
        'Canvas operation failed. Please try again.',
        'error'
      );
    }
  }

  /**
   * Handle keyboard events on canvas
   */
  public onKeyDown(event: KeyboardEvent): void {
    // Handle keyboard shortcuts
    switch (event.key) {
      case 'Escape':
        if (this.uiState.isFullscreen) {
          // Exit fullscreen mode - emit keyboard shortcut event
          this.emitEvent({
            type: 'KEYBOARD_SHORTCUT',
            key: 'Escape',
            modifiers: [],
            action: 'exit_fullscreen'
          });
        }
        break;
      case 'Delete':
      case 'Backspace':
        if (this.selectedPoint) {
          // Delete selected point - emit keyboard shortcut event
          this.emitEvent({
            type: 'KEYBOARD_SHORTCUT',
            key: event.key,
            modifiers: [],
            action: 'delete_selected_point'
          });
        }
        break;
      default:
        // Let other keyboard events bubble up
        break;
    }
  }

  /**
   * Handle canvas focus
   */
  public onCanvasFocus(): void {
    // Canvas gained focus - can be used for accessibility or state management
    Logger.debug('Canvas focused');
  }

  /**
   * Handle canvas blur
   */
  public onCanvasBlur(): void {
    // Canvas lost focus - can be used for accessibility or state management
    Logger.debug('Canvas blurred');
  }

  /**
   * Get debug information for this component
   */
  protected override getDebugInfo(): any {
    return {
      ...super.getDebugInfo(),
      canvasState: this.canvasState,
      isInitialized: this.canvasState?.isInitialized || false,
      hasStage: !!this.stage,
      drillPointsCount: this.drillPoints?.length || 0,
      containerSize: this.canvasContainer?.nativeElement ? {
        width: this.canvasContainer.nativeElement.clientWidth,
        height: this.canvasContainer.nativeElement.clientHeight
      } : null
    };
  }
  
  /**
   * Format numeric values for display
   */
  public formatValue(value: number | undefined | null): string {
    if (value === undefined || value === null || isNaN(value)) {
      return '0.00';
    }
    return value % 1 === 0 ? value.toString() : value.toFixed(2);
  }
  
  /**
   * Zoom in on the canvas
   */
  public zoomIn(): void {
    if (!this.stage) return;
    
    const currentScale = this.stage.scaleX();
    const newScale = Math.min(currentScale * 1.2, this.canvasConfig.maxZoom);
    
    // Get center of stage
    const centerX = this.stage.width() / 2;
    const centerY = this.stage.height() / 2;
    
    // Zoom toward center
    this.zoomToPoint(centerX, centerY, newScale);
  }
  
  /**
   * Zoom out on the canvas
   */
  public zoomOut(): void {
    if (!this.stage) return;
    
    const currentScale = this.stage.scaleX();
    const newScale = Math.max(currentScale / 1.2, this.canvasConfig.minZoom);
    
    // Get center of stage
    const centerX = this.stage.width() / 2;
    const centerY = this.stage.height() / 2;
    
    // Zoom toward center
    this.zoomToPoint(centerX, centerY, newScale);
  }
  
  /**
   * Zoom to a specific point with a given scale
   */
  private zoomToPoint(x: number, y: number, scale: number): void {
    if (!this.stage) return;
    
    const oldScale = this.stage.scaleX();
    
    // Calculate new position
    const mousePointTo = {
      x: (x - this.stage.x()) / oldScale,
      y: (y - this.stage.y()) / oldScale
    };
    
    const newPos = {
      x: x - mousePointTo.x * scale,
      y: y - mousePointTo.y * scale
    };
    
    // Apply new scale and position
    this.stage.scale({ x: scale, y: scale });
    this.stage.position(newPos);
    this.stage.batchDraw();
    
    // Update canvas state
    this.updateCanvasState({
      scale,
      panOffsetX: newPos.x,
      panOffsetY: newPos.y
    });
    
    // Re-render grid with new scale
    this.renderBasicGrid();
  }







  /**
   * Render a professional grid with axes and measurements that adjusts with zoom
   * Ensures the origin (0,0) is at the top-left corner
   */
  private renderBasicGrid(): void {
    if (!this.stage) {
      Logger.warn('Cannot render grid - no stage');
      return;
    }

    try {
      const layers = this.canvasManager.getLayers();
      const gridLayer = layers.grid;

      if (!gridLayer) {
        Logger.warn('No grid layer available');
        return;
      }

      // Clear existing grid
      gridLayer.destroyChildren();

      // Get canvas dimensions and scale
      const stageWidth = this.stage.width();
      const stageHeight = this.stage.height();
      const scale = this.stage.scaleX();
      
      // Calculate the visible area in world coordinates
      const viewportTopLeft = this.transformScreenToCanvas({ x: 0, y: 0 });
      const viewportBottomRight = this.transformScreenToCanvas({ 
        x: stageWidth, 
        y: stageHeight 
      });
      
      // Ensure we always include the origin (0,0) in our grid
      const gridLeft = Math.min(0, Math.floor(viewportTopLeft.x / 100) * 100);
      const gridTop = Math.min(0, Math.floor(viewportTopLeft.y / 100) * 100);
      
      // Calculate grid dimensions to cover the entire viewport plus buffer
      const gridRight = Math.max(2000, Math.ceil(viewportBottomRight.x / 100) * 100 + 500);
      const gridBottom = Math.max(2000, Math.ceil(viewportBottomRight.y / 100) * 100 + 500);
      const gridWidth = gridRight - gridLeft;
      const gridHeight = gridBottom - gridTop;

      if (gridWidth <= 0 || gridHeight <= 0) {
        Logger.warn('Invalid grid dimensions', { gridWidth, gridHeight });
        return;
      }

      // Adjust grid spacing based on zoom level
      const baseSpacing = this.settings?.spacing || 1;
      let gridSpacing = Math.max(baseSpacing * 20, 20);
      let majorGridSpacing = gridSpacing * 5;
      
      // Adjust grid density based on zoom level
      if (scale < 0.5) {
        gridSpacing = majorGridSpacing;
        majorGridSpacing = gridSpacing * 5;
      } else if (scale > 2) {
        gridSpacing = Math.max(baseSpacing * 10, 10);
        majorGridSpacing = gridSpacing * 5;
      }

      // Create grid container positioned at the grid origin
      const gridGroup = new Konva.Group({
        name: 'grid-group',
        x: gridLeft,
        y: gridTop
      });

      // Create minor grid lines
      const minorGridGroup = new Konva.Group({
        name: 'minor-grid-lines'
      });

      // Vertical minor lines - cover the entire grid area
      for (let x = 0; x <= gridWidth; x += gridSpacing) {
        const worldX = gridLeft + x;
        const isMajor = worldX % majorGridSpacing === 0;
        if (!isMajor) {
          const line = new Konva.Line({
            points: [x, 0, x, gridHeight],
            stroke: '#e2e8f0',
            strokeWidth: 1,
            opacity: 0.4,
            name: `grid-line-v-${worldX}`,
            listening: false
          });
          minorGridGroup.add(line);
        }
      }

      // Horizontal minor lines - cover the entire grid area
      for (let y = 0; y <= gridHeight; y += gridSpacing) {
        const worldY = gridTop + y;
        const isMajor = worldY % majorGridSpacing === 0;
        if (!isMajor) {
          const line = new Konva.Line({
            points: [0, y, gridWidth, y],
            stroke: '#e2e8f0',
            strokeWidth: 1,
            opacity: 0.4,
            name: `grid-line-h-${worldY}`,
            listening: false
          });
          minorGridGroup.add(line);
        }
      }

      // Create major grid lines
      const majorGridGroup = new Konva.Group({
        name: 'major-grid-lines'
      });

      // Calculate starting points for major grid lines to ensure they align with world coordinates
      const startX = Math.ceil(gridLeft / majorGridSpacing) * majorGridSpacing - gridLeft;
      const startY = Math.ceil(gridTop / majorGridSpacing) * majorGridSpacing - gridTop;

      // Vertical major lines
      for (let x = startX; x <= gridWidth; x += majorGridSpacing) {
        const worldX = gridLeft + x;
        const line = new Konva.Line({
          points: [x, 0, x, gridHeight],
          stroke: '#cbd5e1',
          strokeWidth: 1,
          opacity: 0.6,
          name: `grid-line-major-v-${worldX}`,
          listening: false
        });
        majorGridGroup.add(line);
        
        // Add measurement labels - only if they would be visible
        if (scale > 0.3) {
          const label = new Konva.Text({
            x: x - 10,
            y: 10,
            text: `${Math.round(worldX / 20)}m`,
            fontSize: 10,
            fontFamily: 'Arial, sans-serif',
            fill: '#64748b',
            align: 'center',
            width: 20,
            listening: false
          });
          majorGridGroup.add(label);
        }
      }

      // Horizontal major lines
      for (let y = startY; y <= gridHeight; y += majorGridSpacing) {
        const worldY = gridTop + y;
        const line = new Konva.Line({
          points: [0, y, gridWidth, y],
          stroke: '#cbd5e1',
          strokeWidth: 1,
          opacity: 0.6,
          name: `grid-line-major-h-${worldY}`,
          listening: false
        });
        majorGridGroup.add(line);
        
        // Add measurement labels - only if they would be visible
        if (scale > 0.3) {
          const label = new Konva.Text({
            x: 10,
            y: y - 8,
            text: `${Math.round(worldY / 20)}m`,
            fontSize: 10,
            fontFamily: 'Arial, sans-serif',
            fill: '#64748b',
            listening: false
          });
          majorGridGroup.add(label);
        }
      }

      // Add axes
      const axesGroup = new Konva.Group({
        name: 'axes'
      });
      
      // Calculate axes positions - ensure they're at 0,0 if visible
      const xAxisY = -gridTop;
      const yAxisX = -gridLeft;
      
      // Only draw axes if they're in the visible area
      if (xAxisY >= 0 && xAxisY <= gridHeight) {
        // X-axis
        const xAxis = new Konva.Line({
          points: [0, xAxisY, gridWidth, xAxisY],
          stroke: '#94a3b8',
          strokeWidth: 2,
          name: 'x-axis',
          listening: false
        });
        axesGroup.add(xAxis);
      }
      
      if (yAxisX >= 0 && yAxisX <= gridWidth) {
        // Y-axis
        const yAxis = new Konva.Line({
          points: [yAxisX, 0, yAxisX, gridHeight],
          stroke: '#94a3b8',
          strokeWidth: 2,
          name: 'y-axis',
          listening: false
        });
        axesGroup.add(yAxis);
      }
      
      // Origin label - only show if origin is visible
      if (yAxisX >= 0 && xAxisY >= 0 && yAxisX <= gridWidth && xAxisY <= gridHeight) {
        const originLabel = new Konva.Text({
          x: yAxisX + 5,
          y: xAxisY + 5,
          text: '0,0',
          fontSize: 10,
          fontFamily: 'Arial, sans-serif',
          fill: '#475569',
          padding: 2,
          listening: false
        });
        axesGroup.add(originLabel);
      }

      // Add all groups to the main grid group
      gridGroup.add(minorGridGroup);
      gridGroup.add(majorGridGroup);
      gridGroup.add(axesGroup);
      
      // Add the grid to the layer
      gridLayer.add(gridGroup);
      gridLayer.batchDraw();

      Logger.info('Enhanced grid rendered successfully', {
        gridDimensions: { left: gridLeft, top: gridTop, width: gridWidth, height: gridHeight },
        scale,
        spacing: { grid: gridSpacing, major: majorGridSpacing }
      });

    } catch (error) {
      Logger.error('Failed to render grid', error);
    }
  }

  /**
   * Render drill points on the canvas
   */
  private renderDrillPoints(): void {
    if (!this.stage || !this.drillPoints) return;

    try {
      const layers = this.canvasManager.getLayers();
      const pointsLayer = layers.points;

      if (!pointsLayer) return;

      // Clear existing drill points
      pointsLayer.destroyChildren();

      Logger.info('Rendering drill points', {
        count: this.drillPoints.length
      });

      // Render each drill point
      this.drillPoints.forEach((point) => {
        // Create a group for the point and its label
        const pointGroup = new Konva.Group({
          name: `drill-point-group-${point.id}`,
          draggable: true,
          x: point.x,
          y: point.y
        });
        
        // Create the point circle
        const circle = new Konva.Circle({
          x: 0,
          y: 0,
          radius: 8,
          fill: point.id === this.selectedPoint?.id ? '#ef4444' : '#3b82f6',
          stroke: point.id === this.selectedPoint?.id ? '#b91c1c' : '#1d4ed8',
          strokeWidth: 2,
          name: `drill-point-${point.id}`,
          shadowColor: 'rgba(0,0,0,0.3)',
          shadowBlur: 4,
          shadowOffset: { x: 0, y: 2 },
          shadowOpacity: 0.3
        });

        // Add point label
        const label = new Konva.Text({
          x: -12,
          y: -30,
          text: point.id,
          fontSize: 12,
          fontFamily: 'Arial, sans-serif',
          fill: '#1e293b',
          align: 'center',
          width: 24,
          padding: 2,
          name: `drill-point-label-${point.id}`
        });
        
        // Add depth indicator
        const depthIndicator = new Konva.Text({
          x: 10,
          y: -8,
          text: `${this.formatValue(point.depth)}m`,
          fontSize: 10,
          fontFamily: 'Arial, sans-serif',
          fill: '#64748b',
          padding: 2,
          name: `drill-point-depth-${point.id}`
        });

        // Add all elements to the group
        pointGroup.add(circle);
        pointGroup.add(label);
        pointGroup.add(depthIndicator);
        
        // Add event handlers
        pointGroup.on('click', () => {
          this.pointSelected.emit(point);
        });

        pointGroup.on('dragmove', () => {
          const pos = pointGroup.position();
          this.pointMoved.emit({
            point: point,
            newX: pos.x,
            newY: pos.y
          });
          
          // Update label position
          label.position({
            x: -12,
            y: -30
          });
        });
        
        // Add hover effects and tooltip
        pointGroup.on('mouseenter', () => {
          document.body.style.cursor = 'pointer';
          circle.to({
            duration: 0.2,
            scaleX: 1.2,
            scaleY: 1.2,
            shadowBlur: 8
          });
          
          // Show tooltip
          this.hoveredPoint = point;
          const stagePos = this.stage!.getPointerPosition();
          if (stagePos) {
            this.tooltipPosition = {
              x: stagePos.x,
              y: stagePos.y - 10
            };
          }
        });
        
        pointGroup.on('mouseleave', () => {
          document.body.style.cursor = 'default';
          circle.to({
            duration: 0.2,
            scaleX: 1,
            scaleY: 1,
            shadowBlur: 4
          });
          
          // Hide tooltip
          this.hoveredPoint = null;
        });

        pointsLayer.add(pointGroup);
      });

      pointsLayer.batchDraw();
      Logger.info('Drill points rendered successfully');

    } catch (error) {
      Logger.error('Failed to render drill points', error);
    }
  }

  /**
   * Create fallback performance monitor when service is not available
   */
  private createPerformanceMonitorFallback(): any {
    return {
      startTimer: (name: string): string => {
        const timerId = `${name}-${Date.now()}`;
        Logger.debug(`Performance timer started: ${timerId}`);
        return timerId;
      },
      endTimer: (timerId: string) => {
        Logger.debug(`Performance timer ended: ${timerId}`);
      },
      trackMemoryLeak: (componentName: string) => {
        Logger.debug(`Memory leak tracking for: ${componentName}`);
      },
      recordMetric: (name: string, value: number, metadata?: any) => {
        Logger.debug(`Metric recorded: ${name} = ${value}`, metadata);
      }
    };
  }
}