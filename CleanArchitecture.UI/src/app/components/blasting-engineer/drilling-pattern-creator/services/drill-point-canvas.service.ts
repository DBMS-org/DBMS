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
} 