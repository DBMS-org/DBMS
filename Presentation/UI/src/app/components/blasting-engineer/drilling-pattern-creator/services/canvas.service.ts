import { Injectable } from '@angular/core';
import Konva from 'konva';
import { DrillPoint, PatternSettings } from '../models/drill-point.model';
import { GridService } from './grid.service';
import { RulerService } from './ruler.service';
import { DrillPointCanvasService } from './drill-point-canvas.service';

@Injectable({
  providedIn: 'root'
})
export class CanvasService {

  constructor(
    private gridService: GridService,
    private rulerService: RulerService,
    private drillPointCanvasService: DrillPointCanvasService
  ) {}

  // ==================== GRID/RULER/DRILL POINT DELEGATION ====================

  // Grid-related methods (delegated to GridService)
  handleGridCache(cacheKey: string, gridGroup: Konva.Group, layer: Konva.Layer): boolean {
    return this.gridService.handleGridCache(cacheKey, gridGroup, layer);
  }

  updateGridCache(cacheKey: string, gridGroup: Konva.Group): void {
    this.gridService.updateGridCache(cacheKey, gridGroup);
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
    return this.gridService.drawGrid(layer, settings, scale, offsetX, offsetY, width, height);
  }

  drawGridIntersections(
    layer: Konva.Layer,
    settings: PatternSettings,
    scale: number,
    offsetX: number,
    offsetY: number,
    width: number,
    height: number,
    isPreciseMode: boolean
  ): Konva.Group {
    return this.gridService.drawGridIntersections(layer, settings, scale, offsetX, offsetY, width, height, isPreciseMode);
  }

  // Ruler-related methods (delegated to RulerService)
  drawRulers(
    layer: Konva.Layer,
    settings: PatternSettings,
    scale: number,
    width: number,
    height: number,
    panOffsetX: number = 0,
    panOffsetY: number = 0
  ): Konva.Group {
    return this.rulerService.drawRulers(layer, settings, scale, width, height, panOffsetX, panOffsetY);
  }

  // Drill point-related methods (delegated to DrillPointCanvasService)
  createDrillPointObject(
    point: DrillPoint,
    scale: number,
    offsetX: number,
    offsetY: number,
    isHolePlacementMode: boolean,
    isSelected: boolean,
    globalDepth?: number
  ): Konva.Group {
    return this.drillPointCanvasService.createDrillPointObject(point, scale, offsetX, offsetY, isHolePlacementMode, isSelected, globalDepth);
  }

  calculateGridCoordinates(
    pointer: { x: number; y: number }, 
    scale: number, 
    offsetX: number, 
    offsetY: number, 
    isPreciseMode?: boolean, 
    settings?: PatternSettings
  ): { x: number; y: number } {
    return this.drillPointCanvasService.calculateGridCoordinates(pointer, scale, offsetX, offsetY, isPreciseMode, settings);
  }

  setCanvasCursor(stage: Konva.Stage, cursorType: string | boolean): void {
    this.drillPointCanvasService.setCanvasCursor(stage, cursorType);
  }

  updatePointSelectability(drillPointObjects: Map<string, Konva.Group>, isHolePlacementMode: boolean): void {
    this.drillPointCanvasService.updatePointSelectability(drillPointObjects, isHolePlacementMode);
  }

  // Custom depth detection methods (delegated to DrillPointCanvasService)
  hasCustomDepth(point: DrillPoint, globalDepth: number): boolean {
    return this.drillPointCanvasService.hasCustomDepth(point, globalDepth);
  }

  getPointsWithCustomDepths(points: DrillPoint[], globalDepth: number): DrillPoint[] {
    return this.drillPointCanvasService.getPointsWithCustomDepths(points, globalDepth);
  }
} 