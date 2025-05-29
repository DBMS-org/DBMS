import { Injectable } from '@angular/core';
import Konva from 'konva';
import { DrillPoint, PatternSettings } from '../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../constants/canvas.constants';

@Injectable({
  providedIn: 'root'
})
export class CanvasService {
  private gridCache: Map<string, Konva.Group> = new Map();

  constructor() {}

  handleGridCache(cacheKey: string, gridGroup: Konva.Group, layer: Konva.Layer): boolean {
    const cachedGrid = this.gridCache.get(cacheKey);
    if (cachedGrid) {
      if (gridGroup) {
        gridGroup.destroy();
      }
      layer.add(cachedGrid);
      return true;
    }
    return false;
  }

  updateGridCache(cacheKey: string, gridGroup: Konva.Group): void {
    this.gridCache.set(cacheKey, gridGroup);
    if (this.gridCache.size > CANVAS_CONSTANTS.MAX_CACHE_SIZE) {
      const keys = Array.from(this.gridCache.keys());
      if (keys.length > 0) {
        const oldGroup = this.gridCache.get(keys[0]);
        if (oldGroup) {
          oldGroup.destroy();
        }
        this.gridCache.delete(keys[0]);
      }
    }
  }

  drawGrid(
    layer: Konva.Layer,
    settings: PatternSettings,
    scale: number,
    offsetX: number,
    offsetY: number,
    width: number,
    height: number
  ): Konva.Group {
    const gridGroup = new Konva.Group();

    // Calculate grid dimensions
    const spacing = settings.spacing * CANVAS_CONSTANTS.GRID_SIZE * scale;
    const burden = settings.burden * CANVAS_CONSTANTS.GRID_SIZE * scale;

    // Calculate grid boundaries
    const startX = CANVAS_CONSTANTS.RULER_WIDTH;
    const startY = CANVAS_CONSTANTS.RULER_HEIGHT;
    const endX = width;
    const endY = height;

    // Draw minor grid lines
    const minorSpacing = spacing / 2;
    const minorBurden = burden / 2;

    // Draw minor vertical lines
    for (let x = startX; x < endX; x += minorSpacing) {
      gridGroup.add(new Konva.Line({
        points: [x, startY, x, endY],
        stroke: 'rgba(200, 200, 200, 0.3)',
        listening: false
      }));
    }

    // Draw minor horizontal lines
    for (let y = startY; y < endY; y += minorBurden) {
      gridGroup.add(new Konva.Line({
        points: [startX, y, endX, y],
        stroke: 'rgba(200, 200, 200, 0.3)',
        listening: false
      }));
    }

    // Draw major grid lines
    // Draw major vertical lines
    for (let x = startX; x < endX; x += spacing) {
      gridGroup.add(new Konva.Line({
        points: [x, startY, x, endY],
        stroke: 'rgba(150, 150, 150, 0.5)',
        listening: false
      }));
    }

    // Draw major horizontal lines
    for (let y = startY; y < endY; y += burden) {
      gridGroup.add(new Konva.Line({
        points: [startX, y, endX, y],
        stroke: 'rgba(150, 150, 150, 0.5)',
        listening: false
      }));
    }

    return gridGroup;
  }

  drawRulers(
    layer: Konva.Layer,
    settings: PatternSettings,
    scale: number,
    width: number,
    height: number
  ): Konva.Group {
    const rulerGroup = new Konva.Group();

    // Draw ruler backgrounds
    rulerGroup.add(new Konva.Rect({
      x: 0,
      y: 0,
      width: CANVAS_CONSTANTS.RULER_WIDTH,
      height: height,
      fill: '#f8f9fa',
      listening: false
    }));

    rulerGroup.add(new Konva.Rect({
      x: 0,
      y: 0,
      width: width,
      height: CANVAS_CONSTANTS.RULER_HEIGHT,
      fill: '#f8f9fa',
      listening: false
    }));

    // Draw measurements
    const burden = settings.burden * CANVAS_CONSTANTS.GRID_SIZE * scale;
    let burdenValue = 0;
    for (let y = CANVAS_CONSTANTS.RULER_HEIGHT; y < height; y += burden) {
      rulerGroup.add(new Konva.Text({
        x: CANVAS_CONSTANTS.RULER_WIDTH / 2,
        y: y,
        text: `${burdenValue}m`,
        fontSize: 10,
        fill: '#495057',
        align: 'center',
        listening: false
      }));
      burdenValue += settings.burden;
    }

    const spacing = settings.spacing * CANVAS_CONSTANTS.GRID_SIZE * scale;
    let spacingValue = 0;
    for (let x = CANVAS_CONSTANTS.RULER_WIDTH; x < width; x += spacing) {
      rulerGroup.add(new Konva.Text({
        x: x,
        y: CANVAS_CONSTANTS.RULER_HEIGHT / 2,
        text: `${spacingValue}m`,
        fontSize: 10,
        fill: '#495057',
        align: 'center',
        listening: false
      }));
      spacingValue += settings.spacing;
    }

    // Add 0 at the origin
    rulerGroup.add(new Konva.Text({
      x: CANVAS_CONSTANTS.RULER_WIDTH / 2,
      y: CANVAS_CONSTANTS.RULER_HEIGHT / 2,
      text: '0',
      fontSize: 10,
      fill: '#495057',
      align: 'center',
      listening: false
    }));

    return rulerGroup;
  }

  createDrillPointObject(
    point: DrillPoint,
    scale: number,
    offsetX: number,
    offsetY: number,
    isHolePlacementMode: boolean,
    isSelected: boolean
  ): Konva.Group {
    // Convert grid coordinates to canvas coordinates
    const gridToCanvas = (gridCoord: number, gridSize: number, scale: number, offset: number, rulerSize: number) => {
      return gridCoord * gridSize * scale + offset + rulerSize;
    };

    const x = gridToCanvas(point.x, CANVAS_CONSTANTS.GRID_SIZE, scale, offsetX, CANVAS_CONSTANTS.RULER_WIDTH);
    const y = gridToCanvas(point.y, CANVAS_CONSTANTS.GRID_SIZE, scale, offsetY, CANVAS_CONSTANTS.RULER_HEIGHT);

    const group = new Konva.Group({
      x,
      y,
      draggable: !isHolePlacementMode,
      listening: true
    });

    const circle = new Konva.Circle({
      radius: CANVAS_CONSTANTS.POINT_RADIUS,
      fill: isSelected ? '#ff0000' : '#2196f3',
      stroke: '#ffffff',
      strokeWidth: 2,
      listening: true
    });

    const text = new Konva.Text({
      text: point.id,
      fontSize: 12,
      fill: '#000000',
      offsetX: CANVAS_CONSTANTS.POINT_RADIUS,
      offsetY: CANVAS_CONSTANTS.POINT_RADIUS,
      listening: false
    });

    group.add(circle);
    group.add(text);

    // Store the drill point data and original grid coordinates
    (group as any).data = {
      ...point,
      originalX: point.x,
      originalY: point.y
    };

    return group;
  }

  calculateGridCoordinates(pointer: { x: number; y: number }, scale: number, offsetX: number, offsetY: number): { x: number; y: number } {
    return {
      x: (pointer.x - CANVAS_CONSTANTS.RULER_WIDTH - offsetX) / (CANVAS_CONSTANTS.GRID_SIZE * scale),
      y: (pointer.y - CANVAS_CONSTANTS.RULER_HEIGHT - offsetY) / (CANVAS_CONSTANTS.GRID_SIZE * scale)
    };
  }

  setCanvasCursor(stage: Konva.Stage, mode: boolean): void {
    stage.container().style.cursor = mode ? 'crosshair' : 'default';
  }

  updatePointSelectability(drillPointObjects: Map<string, Konva.Group>, isHolePlacementMode: boolean): void {
    drillPointObjects.forEach(group => {
      group.draggable(!isHolePlacementMode);
    });
  }
} 