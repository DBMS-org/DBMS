import { Component, Input, Output, EventEmitter, OnInit, OnDestroy, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import Konva from 'konva';
import { DrillPoint, PatternSettings, CanvasPosition } from '../../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../../constants/canvas.constants';
import { Logger } from '../../utils/logger.util';

/**
 * DrillPointCanvasComponent handles all drill point rendering and interaction operations.
 * This component is responsible for:
 * - Rendering drill points with visual feedback and depth indicators
 * - Managing point selection, dragging, and highlighting
 * - Implementing point validation and duplicate detection
 * - Handling point-specific user interactions (click, drag, hover)
 * - Providing visual indicators for custom depth points
 */
@Component({
  selector: 'app-drill-point-canvas',
  standalone: true,
  imports: [CommonModule],
  template: '', // No template needed - this is a service component
  styleUrls: []
})
export class DrillPointCanvasComponent implements OnInit, OnDestroy, OnChanges {
  @Input() layer!: Konva.Layer;
  @Input() drillPoints: DrillPoint[] = [];
  @Input() selectedPoint: DrillPoint | null = null;
  @Input() settings!: PatternSettings;
  @Input() canvasState!: {
    scale: number;
    offsetX: number;
    offsetY: number;
    panOffsetX: number;
    panOffsetY: number;
    width: number;
    height: number;
  };
  @Input() isHolePlacementMode: boolean = false;
  @Input() isPreciseMode: boolean = false;

  // Event outputs
  @Output() pointSelected = new EventEmitter<DrillPoint | null>();
  @Output() pointMoved = new EventEmitter<{ point: DrillPoint; newPosition: CanvasPosition }>();
  @Output() pointPlaced = new EventEmitter<CanvasPosition>();
  @Output() duplicateDetected = new EventEmitter<{ message: string; existingPoint: DrillPoint }>();

  // Point rendering objects
  private pointsGroup: Konva.Group | null = null;
  private highlightGroup: Konva.Group | null = null;
  private depthIndicatorGroup: Konva.Group | null = null;

  // Point interaction state
  private isDragging: boolean = false;
  private dragStartPosition: CanvasPosition | null = null;
  private hoveredPoint: DrillPoint | null = null;

  // Point validation
  private readonly DUPLICATE_TOLERANCE = CANVAS_CONSTANTS.PERFORMANCE.DUPLICATE_TOLERANCE;
  private readonly MIN_POINT_DISTANCE = 0.1; // meters

  // Performance tracking
  private lastRenderTime: number = 0;
  private renderCount: number = 0;

  constructor() {}

  ngOnInit(): void {
    Logger.info('DrillPointCanvasComponent initialized');
    this.initializePointsGroup();
  }

  ngOnDestroy(): void {
    Logger.info('DrillPointCanvasComponent destroying');
    this.destroyPointObjects();
  }

  ngOnChanges(changes: SimpleChanges): void {
    const shouldUpdatePoints = 
      changes['drillPoints'] || 
      changes['selectedPoint'] || 
      changes['settings'] ||
      changes['canvasState'];

    if (shouldUpdatePoints && this.layer) {
      this.updatePoints();
    }
  }

  /**
   * Main method to render all drill points
   */
  public renderPoints(): void {
    if (!this.layer || !this.settings || !this.canvasState) {
      Logger.warn('DrillPointCanvasComponent: Missing required inputs for point rendering');
      return;
    }

    const startTime = performance.now();
    
    try {
      this.clearPointObjects();
      this.initializePointsGroup();
      this.renderAllPoints();
      this.renderPointHighlights();
      this.renderDepthIndicators();
      this.layer.batchDraw();
      
      this.lastRenderTime = performance.now() - startTime;
      this.renderCount++;
      
      Logger.debug(`Points rendered in ${this.lastRenderTime.toFixed(2)}ms (render #${this.renderCount})`);
    } catch (error) {
      Logger.error('Error rendering drill points:', error);
    }
  }

  /**
   * Update points when data changes
   */
  public updatePoints(): void {
    this.renderPoints();
  }

  /**
   * Clear all points from the canvas
   */
  public clearPoints(): void {
    this.clearPointObjects();
    if (this.layer) {
      this.layer.batchDraw();
    }
  }

  /**
   * Add a new point at the specified position
   */
  public addPointAtPosition(position: CanvasPosition): DrillPoint | null {
    const worldPosition = this.canvasToWorldCoordinates(position);
    
    // Validate point placement
    const validationResult = this.validatePointPlacement(worldPosition);
    if (!validationResult.isValid) {
      if (validationResult.isDuplicate && validationResult.existingPoint) {
        this.duplicateDetected.emit({
          message: validationResult.message || 'Duplicate point detected',
          existingPoint: validationResult.existingPoint
        });
      }
      return null;
    }

    // Create new drill point
    const newPoint: DrillPoint = {
      id: this.generatePointId(),
      x: worldPosition.x,
      y: worldPosition.y,
      depth: this.settings.depth,
      spacing: this.settings.spacing,
      burden: this.settings.burden
    };

    this.pointPlaced.emit(position);
    return newPoint;
  }

  /**
   * Select a point by ID
   */
  public selectPoint(pointId: string | null): void {
    const point = pointId ? this.drillPoints.find(p => p.id === pointId) : null;
    this.pointSelected.emit(point);
  }

  /**
   * Get performance statistics
   */
  public getPerformanceStats(): { lastRenderTime: number; renderCount: number; pointCount: number } {
    return {
      lastRenderTime: this.lastRenderTime,
      renderCount: this.renderCount,
      pointCount: this.drillPoints.length
    };
  }

  // Private methods

  /**
   * Initialize the points group and set up event handlers
   */
  private initializePointsGroup(): void {
    if (this.pointsGroup) {
      this.pointsGroup.destroy();
    }

    this.pointsGroup = new Konva.Group({
      name: 'drill-points-group'
    });

    this.highlightGroup = new Konva.Group({
      name: 'highlight-group'
    });

    this.depthIndicatorGroup = new Konva.Group({
      name: 'depth-indicator-group'
    });

    this.layer.add(this.pointsGroup);
    this.layer.add(this.highlightGroup);
    this.layer.add(this.depthIndicatorGroup);

    // Set up canvas click handler for point placement
    this.setupCanvasClickHandler();
  }

  /**
   * Render all drill points
   */
  private renderAllPoints(): void {
    if (!this.pointsGroup) return;

    this.drillPoints.forEach(point => {
      this.renderSinglePoint(point);
    });
  }

  /**
   * Render a single drill point
   */
  private renderSinglePoint(point: DrillPoint): void {
    if (!this.pointsGroup) return;

    const canvasPosition = this.worldToCanvasCoordinates({ x: point.x, y: point.y });
    const isSelected = this.selectedPoint?.id === point.id;
    const hasCustomDepth = point.depth !== this.settings.depth;

    // Determine colors based on state
    let fillColor: string;
    let strokeColor: string;

    if (isSelected) {
      fillColor = CANVAS_CONSTANTS.SELECTED_COLORS.FILL;
      strokeColor = CANVAS_CONSTANTS.SELECTED_COLORS.STROKE;
    } else if (hasCustomDepth) {
      fillColor = CANVAS_CONSTANTS.CUSTOM_DEPTH_COLORS.FILL;
      strokeColor = CANVAS_CONSTANTS.CUSTOM_DEPTH_COLORS.STROKE;
    } else {
      fillColor = CANVAS_CONSTANTS.GLOBAL_DEPTH_COLORS.FILL;
      strokeColor = CANVAS_CONSTANTS.GLOBAL_DEPTH_COLORS.STROKE;
    }

    // Create point circle
    const pointCircle = new Konva.Circle({
      x: canvasPosition.x,
      y: canvasPosition.y,
      radius: CANVAS_CONSTANTS.POINT_RADIUS,
      fill: fillColor,
      stroke: strokeColor,
      strokeWidth: isSelected ? 3 : 2,
      pointId: point.id,
      draggable: true,
      name: 'drill-point'
    });

    // Add event handlers
    this.setupPointEventHandlers(pointCircle, point);

    this.pointsGroup.add(pointCircle);
  }

  /**
   * Render point highlights for hover effects
   */
  private renderPointHighlights(): void {
    if (!this.highlightGroup || !this.hoveredPoint) return;

    const canvasPosition = this.worldToCanvasCoordinates({ 
      x: this.hoveredPoint.x, 
      y: this.hoveredPoint.y 
    });

    const highlight = new Konva.Circle({
      x: canvasPosition.x,
      y: canvasPosition.y,
      radius: CANVAS_CONSTANTS.POINT_RADIUS + 3,
      stroke: '#ffeb3b',
      strokeWidth: 2,
      fill: 'transparent',
      listening: false,
      name: 'point-highlight'
    });

    this.highlightGroup.add(highlight);
  }

  /**
   * Render depth indicators for custom depth points
   */
  private renderDepthIndicators(): void {
    if (!this.depthIndicatorGroup) return;

    this.drillPoints
      .filter(point => point.depth !== this.settings.depth)
      .forEach(point => {
        this.renderDepthIndicator(point);
      });
  }

  /**
   * Render depth indicator for a specific point
   */
  private renderDepthIndicator(point: DrillPoint): void {
    if (!this.depthIndicatorGroup) return;

    const canvasPosition = this.worldToCanvasCoordinates({ x: point.x, y: point.y });
    
    // Create depth indicator symbol (small triangle)
    const indicator = new Konva.RegularPolygon({
      x: canvasPosition.x + CANVAS_CONSTANTS.POINT_RADIUS + 3,
      y: canvasPosition.y - CANVAS_CONSTANTS.POINT_RADIUS - 3,
      sides: 3,
      radius: 4,
      fill: CANVAS_CONSTANTS.CUSTOM_DEPTH_COLORS.INDICATOR,
      stroke: '#ffffff',
      strokeWidth: 1,
      listening: false,
      name: 'depth-indicator'
    });

    this.depthIndicatorGroup.add(indicator);
  }

  /**
   * Set up event handlers for a point
   */
  private setupPointEventHandlers(pointShape: Konva.Circle, point: DrillPoint): void {
    // Click handler
    pointShape.on('click', (e) => {
      e.cancelBubble = true;
      this.handlePointClick(point);
    });

    // Mouse enter handler
    pointShape.on('mouseenter', () => {
      this.handlePointMouseEnter(point);
    });

    // Mouse leave handler
    pointShape.on('mouseleave', () => {
      this.handlePointMouseLeave();
    });

    // Drag start handler
    pointShape.on('dragstart', (e) => {
      this.handlePointDragStart(point, e);
    });

    // Drag move handler
    pointShape.on('dragmove', (e) => {
      this.handlePointDragMove(point, e);
    });

    // Drag end handler
    pointShape.on('dragend', (e) => {
      this.handlePointDragEnd(point, e);
    });
  }

  /**
   * Set up canvas click handler for point placement
   */
  private setupCanvasClickHandler(): void {
    if (!this.layer) return;

    this.layer.on('click', (e) => {
      // Only handle clicks in hole placement mode and when not clicking on existing points
      if (!this.isHolePlacementMode || e.target.getClassName() !== 'Layer') {
        return;
      }

      const stage = this.layer.getStage();
      if (stage) {
        const position = stage.getPointerPosition();
        if (position) {
          this.addPointAtPosition(position);
        }
      }
    });
  }

  /**
   * Handle point click events
   */
  private handlePointClick(point: DrillPoint): void {
    this.selectPoint(point.id);
  }

  /**
   * Handle point mouse enter events
   */
  private handlePointMouseEnter(point: DrillPoint): void {
    this.hoveredPoint = point;
    document.body.style.cursor = 'pointer';
    this.renderPointHighlights();
    this.layer?.batchDraw();
  }

  /**
   * Handle point mouse leave events
   */
  private handlePointMouseLeave(): void {
    this.hoveredPoint = null;
    document.body.style.cursor = 'default';
    this.highlightGroup?.destroyChildren();
    this.layer?.batchDraw();
  }

  /**
   * Handle point drag start events
   */
  private handlePointDragStart(point: DrillPoint, e: Konva.KonvaEventObject<DragEvent>): void {
    this.isDragging = true;
    this.dragStartPosition = { x: e.target.x(), y: e.target.y() };
    this.selectPoint(point.id);
    document.body.style.cursor = 'grabbing';
  }

  /**
   * Handle point drag move events
   */
  private handlePointDragMove(point: DrillPoint, e: Konva.KonvaEventObject<DragEvent>): void {
    // Validate drag position
    const canvasPosition = { x: e.target.x(), y: e.target.y() };
    const worldPosition = this.canvasToWorldCoordinates(canvasPosition);
    
    const validationResult = this.validatePointPlacement(worldPosition, point.id);
    if (!validationResult.isValid) {
      // Revert to last valid position
      if (this.dragStartPosition) {
        e.target.x(this.dragStartPosition.x);
        e.target.y(this.dragStartPosition.y);
      }
      return;
    }

    // Update drag start position for next validation
    this.dragStartPosition = canvasPosition;
  }

  /**
   * Handle point drag end events
   */
  private handlePointDragEnd(point: DrillPoint, e: Konva.KonvaEventObject<DragEvent>): void {
    this.isDragging = false;
    document.body.style.cursor = 'pointer';

    const canvasPosition = { x: e.target.x(), y: e.target.y() };
    const worldPosition = this.canvasToWorldCoordinates(canvasPosition);

    // Emit point moved event
    this.pointMoved.emit({
      point: point,
      newPosition: worldPosition
    });

    this.dragStartPosition = null;
  }

  /**
   * Validate point placement
   */
  private validatePointPlacement(position: CanvasPosition, excludePointId?: string): {
    isValid: boolean;
    message?: string;
    isDuplicate?: boolean;
    existingPoint?: DrillPoint;
  } {
    // Check for duplicates
    const existingPoint = this.drillPoints.find(p => {
      if (excludePointId && p.id === excludePointId) return false;
      
      const distance = Math.sqrt(
        Math.pow(p.x - position.x, 2) + Math.pow(p.y - position.y, 2)
      );
      return distance < this.DUPLICATE_TOLERANCE;
    });

    if (existingPoint) {
      return {
        isValid: false,
        message: `Point too close to existing point at (${existingPoint.x.toFixed(2)}, ${existingPoint.y.toFixed(2)})`,
        isDuplicate: true,
        existingPoint: existingPoint
      };
    }

    // Check minimum distance between points
    const tooClose = this.drillPoints.find(p => {
      if (excludePointId && p.id === excludePointId) return false;
      
      const distance = Math.sqrt(
        Math.pow(p.x - position.x, 2) + Math.pow(p.y - position.y, 2)
      );
      return distance < this.MIN_POINT_DISTANCE;
    });

    if (tooClose) {
      return {
        isValid: false,
        message: `Points must be at least ${this.MIN_POINT_DISTANCE}m apart`
      };
    }

    // Check canvas boundaries
    const canvasPos = this.worldToCanvasCoordinates(position);
    if (canvasPos.x < CANVAS_CONSTANTS.RULER_WIDTH || 
        canvasPos.y < CANVAS_CONSTANTS.RULER_HEIGHT ||
        canvasPos.x > this.canvasState.width ||
        canvasPos.y > this.canvasState.height) {
      return {
        isValid: false,
        message: 'Point must be placed within canvas boundaries'
      };
    }

    return { isValid: true };
  }

  /**
   * Convert world coordinates to canvas coordinates
   */
  private worldToCanvasCoordinates(worldPos: CanvasPosition): CanvasPosition {
    const { scale, offsetX, offsetY, panOffsetX, panOffsetY } = this.canvasState;
    const totalOffsetX = offsetX + panOffsetX;
    const totalOffsetY = offsetY + panOffsetY;

    return {
      x: CANVAS_CONSTANTS.RULER_WIDTH + (worldPos.x * CANVAS_CONSTANTS.GRID_SIZE * scale) + totalOffsetX,
      y: CANVAS_CONSTANTS.RULER_HEIGHT + (worldPos.y * CANVAS_CONSTANTS.GRID_SIZE * scale) + totalOffsetY
    };
  }

  /**
   * Convert canvas coordinates to world coordinates
   */
  private canvasToWorldCoordinates(canvasPos: CanvasPosition): CanvasPosition {
    const { scale, offsetX, offsetY, panOffsetX, panOffsetY } = this.canvasState;
    const totalOffsetX = offsetX + panOffsetX;
    const totalOffsetY = offsetY + panOffsetY;

    return {
      x: (canvasPos.x - CANVAS_CONSTANTS.RULER_WIDTH - totalOffsetX) / (CANVAS_CONSTANTS.GRID_SIZE * scale),
      y: (canvasPos.y - CANVAS_CONSTANTS.RULER_HEIGHT - totalOffsetY) / (CANVAS_CONSTANTS.GRID_SIZE * scale)
    };
  }

  /**
   * Generate unique point ID
   */
  private generatePointId(): string {
    return `point-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
  }

  /**
   * Clear all point objects
   */
  private clearPointObjects(): void {
    this.pointsGroup?.destroyChildren();
    this.highlightGroup?.destroyChildren();
    this.depthIndicatorGroup?.destroyChildren();
  }

  /**
   * Destroy all point objects
   */
  private destroyPointObjects(): void {
    if (this.pointsGroup) {
      this.pointsGroup.destroy();
      this.pointsGroup = null;
    }
    if (this.highlightGroup) {
      this.highlightGroup.destroy();
      this.highlightGroup = null;
    }
    if (this.depthIndicatorGroup) {
      this.depthIndicatorGroup.destroy();
      this.depthIndicatorGroup = null;
    }
  }
}