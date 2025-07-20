import { Injectable } from '@angular/core';
import Konva from 'konva';
import { DrillPoint, PatternSettings } from '../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../constants/canvas.constants';

@Injectable({
  providedIn: 'root'
})
export class DrillPointCanvasService {

  constructor() {}

  createDrillPointObject(
    point: DrillPoint,
    scale: number,
    offsetX: number,
    offsetY: number,
    isHolePlacementMode: boolean,
    isSelected: boolean,
    globalDepth?: number
  ): Konva.Group {
    console.log('createDrillPointObject called with:', {
      point,
      scale,
      offsetX,
      offsetY,
      isHolePlacementMode,
      isSelected
    });

    // Convert grid coordinates to canvas coordinates
    const gridToCanvas = (gridCoord: number, gridSize: number, scale: number, offset: number, rulerSize: number) => {
      return gridCoord * gridSize * scale + offset + rulerSize;
    };

    const x = gridToCanvas(point.x, CANVAS_CONSTANTS.GRID_SIZE, scale, offsetX, CANVAS_CONSTANTS.RULER_WIDTH);
    const y = gridToCanvas(point.y, CANVAS_CONSTANTS.GRID_SIZE, scale, offsetY, CANVAS_CONSTANTS.RULER_HEIGHT);

    console.log('Calculated canvas position:', { x, y });
    console.log('CANVAS_CONSTANTS:', {
      GRID_SIZE: CANVAS_CONSTANTS.GRID_SIZE,
      RULER_WIDTH: CANVAS_CONSTANTS.RULER_WIDTH,
      RULER_HEIGHT: CANVAS_CONSTANTS.RULER_HEIGHT,
      POINT_RADIUS: CANVAS_CONSTANTS.POINT_RADIUS
    });

    const group = new Konva.Group({
      x,
      y,
      draggable: !isHolePlacementMode,
      listening: true,
      name: 'drill-point-group' // Add name for click detection
    });

    // Determine if this hole has a custom depth
    const hasCustomDepth = globalDepth !== undefined && this.hasCustomDepth(point, globalDepth);
    
    // Choose colors based on selection and custom depth status
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

    const circle = new Konva.Circle({
      radius: CANVAS_CONSTANTS.POINT_RADIUS,
      fill: fillColor,
      stroke: strokeColor,
      strokeWidth: isSelected ? 3 : 2,
      listening: true,
      pointId: point.id // Add pointId for click detection
    });

    const text = new Konva.Text({
      text: point.id,
      fontSize: 12,
      fill: '#000000',
      offsetX: CANVAS_CONSTANTS.POINT_RADIUS,
      offsetY: CANVAS_CONSTANTS.POINT_RADIUS,
      listening: false
    });

    // Add custom depth indicator if hole has custom depth
    let depthIndicator: Konva.Shape | null = null;
    if (hasCustomDepth) {
      // Add a small diamond indicator for custom depths
      depthIndicator = new Konva.RegularPolygon({
        x: CANVAS_CONSTANTS.POINT_RADIUS + 3,
        y: -CANVAS_CONSTANTS.POINT_RADIUS - 3,
        sides: 4,
        radius: 3,
        fill: CANVAS_CONSTANTS.CUSTOM_DEPTH_COLORS.INDICATOR,
        stroke: '#ffffff',
        strokeWidth: 1,
        rotation: 45,
        listening: false
      });
    }

    // Add pointId to group for click detection
    (group as any).pointId = point.id;
    (circle as any).pointId = point.id;

    group.add(circle);
    group.add(text);
    if (depthIndicator) {
      group.add(depthIndicator);
    }

    // Add hover tooltip functionality
    this.addTooltipEvents(group, point, hasCustomDepth, globalDepth);

    // Store the drill point data and original grid coordinates
    (group as any).data = {
      ...point,
      originalX: point.x,
      originalY: point.y
    };

    console.log('Created group with children:', group.children.length);

    return group;
  }

  calculateGridCoordinates(
    pointer: { x: number; y: number }, 
    scale: number, 
    offsetX: number, 
    offsetY: number, 
    isPreciseMode?: boolean, 
    settings?: PatternSettings
  ): { x: number; y: number } {
    const rawX = (pointer.x - CANVAS_CONSTANTS.RULER_WIDTH - offsetX) / (CANVAS_CONSTANTS.GRID_SIZE * scale);
    const rawY = (pointer.y - CANVAS_CONSTANTS.RULER_HEIGHT - offsetY) / (CANVAS_CONSTANTS.GRID_SIZE * scale);
    
    if (isPreciseMode && settings) {
      return this.snapToGridIntersection(rawX, rawY, settings);
    }
    
    return { x: rawX, y: rawY };
  }

  private snapToGridIntersection(x: number, y: number, settings: PatternSettings): { x: number; y: number } {
    // Snap to grid intersection points based on spacing and burden
    const snappedX = Math.round(x / settings.spacing) * settings.spacing;
    const snappedY = Math.round(y / settings.burden) * settings.burden;
    
    return { x: snappedX, y: snappedY };
  }

  setCanvasCursor(stage: Konva.Stage, cursorType: string | boolean): void {
    const container = stage.container();
    
    if (typeof cursorType === 'boolean') {
      // Legacy support for boolean mode
      container.style.cursor = cursorType ? 'crosshair' : 'default';
      return;
    }
    
    // Handle different cursor types
    switch (cursorType) {
      case 'panning':
        container.style.cursor = 'grabbing';
        break;
      case 'dragging':
        container.style.cursor = 'move';
        break;
      case 'crosshair':
        container.style.cursor = 'crosshair';
        break;
      case 'pointer':
        container.style.cursor = 'pointer';
        break;
      case 'grab':
        container.style.cursor = 'grab';
        break;
      case 'not-allowed':
        container.style.cursor = 'not-allowed';
        break;
      case 'default':
      default:
        container.style.cursor = 'default';
        break;
    }
  }

  updatePointSelectability(drillPointObjects: Map<string, Konva.Group>, isHolePlacementMode: boolean): void {
    drillPointObjects.forEach(group => {
      group.draggable(!isHolePlacementMode);
    });
  }

  /**
   * Determines if a drill point has a custom depth different from the global depth
   */
  hasCustomDepth(point: DrillPoint, globalDepth: number): boolean {
    return Math.abs(point.depth - globalDepth) > 0.01;
  }

  /**
   * Gets all drill points that have custom depths
   */
  getPointsWithCustomDepths(points: DrillPoint[], globalDepth: number): DrillPoint[] {
    return points.filter(point => this.hasCustomDepth(point, globalDepth));
  }

  /**
   * Adds hover tooltip events to a drill point group
   */
  private addTooltipEvents(group: Konva.Group, point: DrillPoint, hasCustomDepth: boolean, globalDepth?: number): void {
    let tooltip: Konva.Group | null = null;

    group.on('mouseenter', () => {
      // Create tooltip
      tooltip = this.createTooltip(point, hasCustomDepth, globalDepth);
      if (tooltip) {
        group.add(tooltip);
        group.getLayer()?.batchDraw();
      }
    });

    group.on('mouseleave', () => {
      // Remove tooltip
      if (tooltip) {
        tooltip.destroy();
        tooltip = null;
        group.getLayer()?.batchDraw();
      }
    });
  }

  /**
   * Creates a tooltip showing drill point information
   */
  private createTooltip(point: DrillPoint, hasCustomDepth: boolean, globalDepth?: number): Konva.Group {
    const tooltip = new Konva.Group({
      x: CANVAS_CONSTANTS.POINT_RADIUS + 10,
      y: -CANVAS_CONSTANTS.POINT_RADIUS - 10
    });

    // Prepare tooltip text
    const lines = [
      `ID: ${point.id}`,
      `Position: (${point.x.toFixed(2)}, ${point.y.toFixed(2)})`,
      `Depth: ${point.depth.toFixed(1)}m`
    ];

    if (hasCustomDepth && globalDepth !== undefined) {
      lines.push(`Global: ${globalDepth.toFixed(1)}m`);
      lines.push('Custom depth');
    }

    // Calculate tooltip dimensions
    const fontSize = 10;
    const lineHeight = fontSize + 2;
    const padding = 6;
    const maxWidth = Math.max(...lines.map(line => line.length * (fontSize * 0.6)));
    const tooltipWidth = maxWidth + padding * 2;
    const tooltipHeight = lines.length * lineHeight + padding * 2;

    // Create background
    const background = new Konva.Rect({
      width: tooltipWidth,
      height: tooltipHeight,
      fill: 'rgba(0, 0, 0, 0.8)',
      cornerRadius: 4,
      stroke: '#ffffff',
      strokeWidth: 1
    });

    tooltip.add(background);

    // Add text lines
    lines.forEach((line, index) => {
      const text = new Konva.Text({
        x: padding,
        y: padding + index * lineHeight,
        text: line,
        fontSize: fontSize,
        fill: '#ffffff',
        fontFamily: 'Arial, sans-serif'
      });
      tooltip.add(text);
    });

    return tooltip;
  }
} 