import { Injectable } from '@angular/core';
import Konva from 'konva';
import { CANVAS_CONSTANTS } from '../constants/canvas.constants';
import { Logger } from '../utils/logger.util';
import { KonvaHelpers } from '../utils/konva-helpers.util';
import { PatternSettings, ViewportSettings } from '../models/drill-point.model';

@Injectable({
  providedIn: 'root'
})
export class CanvasManager {
  private stage!: Konva.Stage;
  private gridLayer!: Konva.Layer;
  private rulerLayer!: Konva.Layer;
  private pointsLayer!: Konva.Layer;
  private gridGroup!: Konva.Group;
  private rulerGroup!: Konva.Group;
  private intersectionGroup!: Konva.Group;
  
  private isInitialized = false;
  private containerElement!: HTMLDivElement;

  // Layer order constants
  private readonly LAYER_ORDER = {
    GRID: 0,
    POINTS: 1,
    RULERS: 2
  };

  initialize(container: HTMLDivElement): void {
    if (this.isInitialized) {
      Logger.warn('Canvas already initialized, skipping');
      return;
    }

    this.containerElement = container;
    this.createStage();
    this.createLayers();
    this.setupLayerOrder();
    this.isInitialized = true;
    
    Logger.info('Canvas manager initialized successfully');
  }

  private createStage(): void {
    const rect = this.containerElement.getBoundingClientRect();
    const width = rect.width || 800;
    const height = rect.height || 600;

    this.stage = new Konva.Stage({
      container: this.containerElement,
      width,
      height,
      draggable: false
    });

    Logger.info('Konva stage created', { width, height });
  }

  private createLayers(): void {
    // Create layers in order
    this.gridLayer = new Konva.Layer();
    this.pointsLayer = new Konva.Layer();
    this.rulerLayer = new Konva.Layer();

    // Add to stage
    this.stage.add(this.gridLayer);
    this.stage.add(this.pointsLayer);
    this.stage.add(this.rulerLayer);

    Logger.info('Canvas layers created');
  }

  private setupLayerOrder(): void {
    // Set layer z-index: grid at bottom, points in middle, rulers on top
    this.gridLayer.moveToBottom();
    this.pointsLayer.moveToTop();
    this.rulerLayer.moveToTop();
  }

  getStage(): Konva.Stage {
    return this.stage;
  }

  getLayers() {
    return {
      grid: this.gridLayer,
      points: this.pointsLayer,
      rulers: this.rulerLayer
    };
  }

  getGroups() {
    return {
      grid: this.gridGroup,
      rulers: this.rulerGroup,
      intersections: this.intersectionGroup
    };
  }

  setGroups(groups: { grid?: Konva.Group; rulers?: Konva.Group; intersections?: Konva.Group }) {
    if (groups.grid) this.gridGroup = groups.grid;
    if (groups.rulers) this.rulerGroup = groups.rulers;
    if (groups.intersections) this.intersectionGroup = groups.intersections;
  }

  resize(onResizeCallback?: () => void): void {
    if (!this.isInitialized || !this.stage) return;

    const rect = this.containerElement.getBoundingClientRect();
    const width = rect.width || 800;
    const height = rect.height || 600;

    const oldDimensions = {
      width: this.stage.width(),
      height: this.stage.height()
    };

    this.stage.width(width);
    this.stage.height(height);
    
    Logger.info('Canvas resized', { 
      from: oldDimensions,
      to: { width, height }
    });

    // Execute callback after resize (e.g., to clear cache and redraw)
    if (onResizeCallback) {
      onResizeCallback();
    }
  }

  setCursor(cursor: string): void {
    if (this.stage) {
      KonvaHelpers.setCursor(this.stage, cursor);
    }
  }

  getPointerPosition(): { x: number; y: number } | null {
    return this.stage?.getPointerPosition() || null;
  }

  batchDraw(): void {
    if (this.gridLayer) this.gridLayer.batchDraw();
    if (this.pointsLayer) this.pointsLayer.batchDraw();
    if (this.rulerLayer) this.rulerLayer.batchDraw();
  }

  clearAll(): void {
    if (this.gridLayer) this.gridLayer.destroyChildren();
    if (this.pointsLayer) this.pointsLayer.destroyChildren();
    if (this.rulerLayer) this.rulerLayer.destroyChildren();
    this.batchDraw();
  }

  destroy(): void {
    if (this.stage) {
      this.stage.destroy();
      this.isInitialized = false;
      Logger.info('Canvas manager destroyed');
    }
  }

  isReady(): boolean {
    return this.isInitialized && !!this.stage;
  }
} 