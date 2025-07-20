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

// Import specialized canvas components (will be added gradually)
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
    // GridCanvasComponent,
    // RulerCanvasComponent,
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

  // Sub-component references (will be added when components are ready)
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

      // Render initial content (will be replaced by sub-components later)
      this.renderBasicGrid();
      this.renderDrillPoints();

      // Update canvas state
      this.updateCanvasState({
        isInitialized: true,
        scale: 1
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
   * Fit canvas content to screen
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

      // Calculate center position
      const centerX = bounds.x + bounds.width / 2;
      const centerY = bounds.y + bounds.height / 2;

      // Apply transformation to stage
      if (this.stage) {
        this.stage.scale({ x: scale, y: scale });
        this.stage.position({ x: centerX, y: centerY });
        this.stage.batchDraw();
      }

      this.updateCanvasState({
        scale,
        panOffsetX: centerX,
        panOffsetY: centerY
      });

      if (timerId && this.performanceMonitor) {
        this.performanceMonitor.endTimer(timerId);
      }

      Logger.info('Canvas fitted to screen', { scale, centerX, centerY });

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
        this.stage.position({ x: 0, y: 0 });
        this.stage.batchDraw();
      }

      this.updateCanvasState({
        scale: 1,
        panOffsetX: 0,
        panOffsetY: 0
      });

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
  }

  /**
   * Handle mouse wheel events for zooming
   */
  private onWheel(event: any): void {
    event.evt.preventDefault();

    const scaleBy = 1.1;
    const stage = event.target.getStage();
    const pointer = stage.getPointerPosition();
    const mousePointTo = {
      x: (pointer.x - stage.x()) / stage.scaleX(),
      y: (pointer.y - stage.y()) / stage.scaleY()
    };

    const direction = event.evt.deltaY > 0 ? -1 : 1;
    const newScale = Math.max(
      this.canvasConfig.minZoom,
      Math.min(this.canvasConfig.maxZoom, stage.scaleX() * Math.pow(scaleBy, direction))
    );

    stage.scale({ x: newScale, y: newScale });

    const newPos = {
      x: pointer.x - mousePointTo.x * newScale,
      y: pointer.y - mousePointTo.y * newScale
    };

    stage.position(newPos);
    stage.batchDraw();

    this.updateCanvasState({
      scale: newScale,
      panOffsetX: newPos.x,
      panOffsetY: newPos.y
    });
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
   * Render a basic grid to make the canvas visible
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

      // Get canvas dimensions
      const width = this.stage.width();
      const height = this.stage.height();

      if (width === 0 || height === 0) {
        Logger.warn('Canvas has zero dimensions', { width, height });
        return;
      }

      // Grid spacing based on settings or default
      const spacing = this.settings?.spacing || 1;
      const gridSpacing = Math.max(spacing * 50, 50);

      // Create grid lines
      const gridGroup = new Konva.Group();

      // Add a background rectangle
      const background = new Konva.Rect({
        x: 0,
        y: 0,
        width: width,
        height: height,
        fill: 'transparent',
        stroke: '#e0e0e0',
        strokeWidth: 1
      });
      gridGroup.add(background);

      // Vertical lines
      for (let x = 0; x <= width; x += gridSpacing) {
        const line = new Konva.Line({
          points: [x, 0, x, height],
          stroke: '#e0e0e0',
          strokeWidth: 1,
          opacity: 0.5
        });
        gridGroup.add(line);
      }

      // Horizontal lines
      for (let y = 0; y <= height; y += gridSpacing) {
        const line = new Konva.Line({
          points: [0, y, width, y],
          stroke: '#e0e0e0',
          strokeWidth: 1,
          opacity: 0.5
        });
        gridGroup.add(line);
      }

      gridLayer.add(gridGroup);
      gridLayer.batchDraw();

      Logger.info('Basic grid rendered successfully');

    } catch (error) {
      Logger.error('Failed to render basic grid', error);
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
        const circle = new Konva.Circle({
          x: point.x,
          y: point.y,
          radius: 8,
          fill: point.id === this.selectedPoint?.id ? '#ff6b35' : '#4a90e2',
          stroke: '#2c3e50',
          strokeWidth: 2,
          draggable: true,
          name: `drill-point-${point.id}`
        });

        // Add point label
        const label = new Konva.Text({
          x: point.x - 10,
          y: point.y - 25,
          text: point.id,
          fontSize: 12,
          fontFamily: 'Arial',
          fill: '#2c3e50',
          align: 'center'
        });

        // Add event handlers
        circle.on('click', () => {
          this.pointSelected.emit(point);
        });

        circle.on('dragmove', () => {
          const pos = circle.position();
          this.pointMoved.emit({
            point: point,
            newX: pos.x,
            newY: pos.y
          });
        });

        pointsLayer.add(circle);
        pointsLayer.add(label);
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