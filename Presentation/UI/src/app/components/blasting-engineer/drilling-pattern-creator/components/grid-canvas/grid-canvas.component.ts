import { Component, Input, OnInit, OnDestroy, OnChanges, SimpleChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import Konva from 'konva';
import { 
  PatternSettings, 
  CanvasState, 
  ValidationResult, 
  PerformanceMetrics,
  KonvaLayer,
  CoordinateValue
} from '../../types';
import { IGridCanvasComponent, GridCanvasConfig, ComponentHealthStatus } from '../../interfaces/component.interfaces';
import { CANVAS_CONSTANTS } from '../../constants/canvas.constants';
import { Logger } from '../../utils/logger.util';

/**
 * GridCanvasComponent handles all grid rendering operations for the drilling pattern creator.
 * This component is responsible for:
 * - Rendering grid lines (major and minor) with type safety
 * - Managing grid caching for performance optimization
 * - Displaying precise mode grid intersection indicators
 * - Handling grid updates and refresh functionality
 * - Providing comprehensive error handling and validation
 * 
 * @implements {IGridCanvasComponent}
 */
@Component({
  selector: 'app-grid-canvas',
  standalone: true,
  imports: [CommonModule],
  template: '', // No template needed - this is a service component
  styleUrls: []
})
export class GridCanvasComponent implements IGridCanvasComponent, OnInit, OnDestroy, OnChanges {
  // Input properties with enhanced type safety
  @Input({ required: true }) layer!: KonvaLayer;
  @Input({ required: true }) settings!: PatternSettings;
  @Input({ required: true }) canvasState!: CanvasState;
  @Input() isPreciseMode: boolean = false;
  @Input() isVisible: boolean = true;

  // Component configuration
  private config: GridCanvasConfig = {
    renderQuality: 'medium',
    maxGridLines: CANVAS_CONSTANTS.MAX_GRID_LINES,
    enableCaching: true,
    colorScheme: {
      majorLines: 'rgba(150, 150, 150, 0.5)',
      minorLines: 'rgba(200, 200, 200, 0.3)',
      intersections: 'rgba(255, 165, 0, 0.8)'
    }
  };

  // Grid objects with type safety
  private gridGroup: Konva.Group | null = null;
  private intersectionGroup: Konva.Group | null = null;

  // Grid caching mechanism with enhanced typing
  private readonly gridCache = new Map<string, Konva.Group>();
  private readonly intersectionCache = new Map<string, Konva.Group>();

  // Performance tracking with detailed metrics
  private performanceMetrics: PerformanceMetrics = {
    operation: 'grid-rendering',
    duration: 0,
    memoryUsage: 0,
    timestamp: new Date().toISOString() as import('../../types').ISOTimestamp,
    metadata: {
      renderCount: 0,
      cacheHitRate: 0,
      gridLinesRendered: 0
    }
  };

  // Component health status
  private healthStatus: ComponentHealthStatus = {
    isHealthy: true,
    isInitialized: false,
    issues: [],
    lastChecked: new Date().toISOString()
  };

  constructor() {}

  ngOnInit(): void {
    Logger.info('GridCanvasComponent initialized');
  }

  ngOnDestroy(): void {
    Logger.info('GridCanvasComponent destroying');
    this.clearAllCaches();
    this.destroyGridObjects();
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Check if any inputs that affect grid rendering have changed
    const shouldUpdateGrid = 
      changes['settings'] || 
      changes['canvasState'] || 
      changes['isPreciseMode'];

    if (shouldUpdateGrid && this.layer) {
      this.updateGrid();
    }
  }

  /**
   * Main method to render the grid
   */
  public renderGrid(): void {
    if (!this.layer || !this.settings || !this.canvasState) {
      Logger.warn('GridCanvasComponent: Missing required inputs for grid rendering');
      return;
    }

    const startTime = performance.now();
    
    try {
      this.renderGridLines();
      this.renderGridIntersections();
      this.layer.batchDraw();
      
      this.lastRenderTime = performance.now() - startTime;
      this.renderCount++;
      
      Logger.debug(`Grid rendered in ${this.lastRenderTime.toFixed(2)}ms (render #${this.renderCount})`);
    } catch (error) {
      Logger.error('Error rendering grid:', error);
    }
  }

  /**
   * Update grid when settings or state changes
   */
  public updateGrid(): void {
    this.renderGrid();
  }

  /**
   * Clear the grid and remove from layer
   */
  public clearGrid(): void {
    this.destroyGridObjects();
    if (this.layer) {
      this.layer.batchDraw();
    }
  }

  /**
   * Get grid cache statistics for debugging
   */
  public getCacheStats(): { gridCacheSize: number; intersectionCacheSize: number; lastRenderTime: number; renderCount: number } {
    return {
      gridCacheSize: this.gridCache.size,
      intersectionCacheSize: this.intersectionCache.size,
      lastRenderTime: this.lastRenderTime,
      renderCount: this.renderCount
    };
  }

  /**
   * Force clear all caches (useful for testing or memory management)
   */
  public clearAllCaches(): void {
    this.clearGridCache();
    this.clearIntersectionCache();
  }

  // Private methods

  /**
   * Render grid lines with caching
   */
  private renderGridLines(): void {
    const cacheKey = this.generateGridCacheKey();
    
    // Check cache first
    const cachedGrid = this.gridCache.get(cacheKey);
    if (cachedGrid) {
      this.replaceGridGroup(cachedGrid.clone());
      Logger.debug('Using cached grid');
      return;
    }

    // Create new grid
    const newGridGroup = this.createGridLines();
    this.replaceGridGroup(newGridGroup);
    
    // Cache the grid
    this.updateGridCache(cacheKey, newGridGroup.clone());
    
    Logger.debug('Created new grid and cached it');
  }

  /**
   * Render grid intersection indicators for precise mode
   */
  private renderGridIntersections(): void {
    // Always destroy old intersection group since precise mode can change
    if (this.intersectionGroup) {
      this.intersectionGroup.destroy();
      this.intersectionGroup = null;
    }

    if (!this.isPreciseMode) {
      return;
    }

    const cacheKey = this.generateIntersectionCacheKey();
    
    // Check cache first
    const cachedIntersections = this.intersectionCache.get(cacheKey);
    if (cachedIntersections) {
      this.intersectionGroup = cachedIntersections.clone();
      this.layer.add(this.intersectionGroup);
      Logger.debug('Using cached intersections');
      return;
    }

    // Create new intersections
    this.intersectionGroup = this.createGridIntersections();
    this.layer.add(this.intersectionGroup);
    
    // Cache the intersections
    this.updateIntersectionCache(cacheKey, this.intersectionGroup.clone());
    
    Logger.debug('Created new intersections and cached them');
  }

  /**
   * Create grid lines
   */
  private createGridLines(): Konva.Group {
    const gridGroup = new Konva.Group();
    let totalLinesDrawn = 0;

    const { scale, offsetX, offsetY, panOffsetX, panOffsetY, width, height } = this.canvasState;
    const totalOffsetX = offsetX + panOffsetX;
    const totalOffsetY = offsetY + panOffsetY;

    // Calculate grid dimensions
    const spacing = this.settings.spacing * CANVAS_CONSTANTS.GRID_SIZE * scale;
    const burden = this.settings.burden * CANVAS_CONSTANTS.GRID_SIZE * scale;

    // Calculate grid boundaries
    const startX = CANVAS_CONSTANTS.RULER_WIDTH;
    const startY = CANVAS_CONSTANTS.RULER_HEIGHT;
    const endX = width;
    const endY = height;

    // Align grid starting positions with ruler measurements
    let alignedGridX = startX + (totalOffsetX % spacing);
    if (alignedGridX < startX) alignedGridX += spacing;
    
    let alignedGridY = startY + (totalOffsetY % burden);
    if (alignedGridY < startY) alignedGridY += burden;

    // Draw minor grid lines first
    totalLinesDrawn += this.drawMinorGridLines(gridGroup, alignedGridX, alignedGridY, spacing, burden, startX, startY, endX, endY);
    
    // Draw major grid lines
    totalLinesDrawn += this.drawMajorGridLines(gridGroup, alignedGridX, alignedGridY, spacing, burden, startX, startY, endX, endY);

    // Log warning if we hit the limit
    if (totalLinesDrawn >= CANVAS_CONSTANTS.MAX_GRID_LINES) {
      Logger.warn(`Grid line limit of ${CANVAS_CONSTANTS.MAX_GRID_LINES} reached. Consider zooming in or increasing spacing/burden values for better performance.`);
    }

    return gridGroup;
  }

  /**
   * Draw minor grid lines
   */
  private drawMinorGridLines(
    gridGroup: Konva.Group,
    alignedGridX: number,
    alignedGridY: number,
    spacing: number,
    burden: number,
    startX: number,
    startY: number,
    endX: number,
    endY: number
  ): number {
    let linesDrawn = 0;
    const minorSpacing = spacing / 2;
    const minorBurden = burden / 2;

    // Draw minor vertical lines
    for (let x = alignedGridX - spacing; x < endX && linesDrawn < CANVAS_CONSTANTS.MAX_GRID_LINES; x += minorSpacing) {
      if (x >= startX) {
        gridGroup.add(new Konva.Line({
          points: [x, startY, x, endY],
          stroke: 'rgba(200, 200, 200, 0.3)',
          listening: false
        }));
        linesDrawn++;
      }
    }

    // Draw minor horizontal lines
    for (let y = alignedGridY - burden; y < endY && linesDrawn < CANVAS_CONSTANTS.MAX_GRID_LINES; y += minorBurden) {
      if (y >= startY) {
        gridGroup.add(new Konva.Line({
          points: [startX, y, endX, y],
          stroke: 'rgba(200, 200, 200, 0.3)',
          listening: false
        }));
        linesDrawn++;
      }
    }

    return linesDrawn;
  }

  /**
   * Draw major grid lines
   */
  private drawMajorGridLines(
    gridGroup: Konva.Group,
    alignedGridX: number,
    alignedGridY: number,
    spacing: number,
    burden: number,
    startX: number,
    startY: number,
    endX: number,
    endY: number
  ): number {
    let linesDrawn = 0;

    // Draw major vertical lines
    for (let x = alignedGridX; x < endX && linesDrawn < CANVAS_CONSTANTS.MAX_GRID_LINES; x += spacing) {
      if (x >= startX) {
        gridGroup.add(new Konva.Line({
          points: [x, startY, x, endY],
          stroke: 'rgba(150, 150, 150, 0.5)',
          listening: false
        }));
        linesDrawn++;
      }
    }

    // Draw major horizontal lines
    for (let y = alignedGridY; y < endY && linesDrawn < CANVAS_CONSTANTS.MAX_GRID_LINES; y += burden) {
      if (y >= startY) {
        gridGroup.add(new Konva.Line({
          points: [startX, y, endX, y],
          stroke: 'rgba(150, 150, 150, 0.5)',
          listening: false
        }));
        linesDrawn++;
      }
    }

    return linesDrawn;
  }

  /**
   * Create grid intersection indicators for precise mode
   */
  private createGridIntersections(): Konva.Group {
    const intersectionGroup = new Konva.Group();
    
    if (!this.isPreciseMode) {
      return intersectionGroup;
    }

    let totalIntersections = 0;
    const maxIntersections = Math.floor(CANVAS_CONSTANTS.MAX_GRID_LINES / 4);

    const { scale, offsetX, offsetY, panOffsetX, panOffsetY, width, height } = this.canvasState;
    const totalOffsetX = offsetX + panOffsetX;
    const totalOffsetY = offsetY + panOffsetY;

    // Calculate grid dimensions
    const spacing = this.settings.spacing * CANVAS_CONSTANTS.GRID_SIZE * scale;
    const burden = this.settings.burden * CANVAS_CONSTANTS.GRID_SIZE * scale;

    // Calculate grid boundaries
    const startX = CANVAS_CONSTANTS.RULER_WIDTH;
    const startY = CANVAS_CONSTANTS.RULER_HEIGHT;
    const endX = width;
    const endY = height;

    // Calculate grid starting positions
    let alignedGridX = startX + (totalOffsetX % spacing);
    if (alignedGridX < startX) alignedGridX += spacing;
    
    let alignedGridY = startY + (totalOffsetY % burden);
    if (alignedGridY < startY) alignedGridY += burden;

    // Draw intersection points where major grid lines meet
    for (let x = alignedGridX; x < endX && totalIntersections < maxIntersections; x += spacing) {
      for (let y = alignedGridY; y < endY && totalIntersections < maxIntersections; y += burden) {
        if (x >= startX && y >= startY) {
          const intersectionPoint = new Konva.Circle({
            x: x,
            y: y,
            radius: 2,
            fill: 'rgba(255, 165, 0, 0.8)', // Orange color for visibility
            stroke: 'rgba(255, 140, 0, 1)',
            strokeWidth: 1,
            listening: false
          });
          intersectionGroup.add(intersectionPoint);
          totalIntersections++;
        }
      }
    }

    // Log warning if we hit the intersection limit
    if (totalIntersections >= maxIntersections) {
      Logger.warn(`Grid intersection limit of ${maxIntersections} reached. Consider zooming in or increasing spacing/burden values.`);
    }

    return intersectionGroup;
  }

  /**
   * Replace the current grid group with a new one
   */
  private replaceGridGroup(newGridGroup: Konva.Group): void {
    if (this.gridGroup) {
      this.gridGroup.destroy();
    }
    this.gridGroup = newGridGroup;
    this.layer.add(this.gridGroup);
  }

  /**
   * Destroy all grid objects
   */
  private destroyGridObjects(): void {
    if (this.gridGroup) {
      this.gridGroup.destroy();
      this.gridGroup = null;
    }
    if (this.intersectionGroup) {
      this.intersectionGroup.destroy();
      this.intersectionGroup = null;
    }
  }

  /**
   * Generate cache key for grid lines
   */
  private generateGridCacheKey(): string {
    const { scale, offsetX, offsetY, panOffsetX, panOffsetY, width, height } = this.canvasState;
    return `grid-${scale}-${offsetX + panOffsetX}-${offsetY + panOffsetY}-${width}-${height}-${this.settings.spacing}-${this.settings.burden}`;
  }

  /**
   * Generate cache key for intersection indicators
   */
  private generateIntersectionCacheKey(): string {
    const { scale, offsetX, offsetY, panOffsetX, panOffsetY, width, height } = this.canvasState;
    return `intersections-${scale}-${offsetX + panOffsetX}-${offsetY + panOffsetY}-${width}-${height}-${this.settings.spacing}-${this.settings.burden}-${this.isPreciseMode}`;
  }

  /**
   * Update grid cache with size limit
   */
  private updateGridCache(cacheKey: string, gridGroup: Konva.Group): void {
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

  /**
   * Update intersection cache with size limit
   */
  private updateIntersectionCache(cacheKey: string, intersectionGroup: Konva.Group): void {
    this.intersectionCache.set(cacheKey, intersectionGroup);
    if (this.intersectionCache.size > CANVAS_CONSTANTS.MAX_CACHE_SIZE) {
      const keys = Array.from(this.intersectionCache.keys());
      if (keys.length > 0) {
        const oldGroup = this.intersectionCache.get(keys[0]);
        if (oldGroup) {
          oldGroup.destroy();
        }
        this.intersectionCache.delete(keys[0]);
      }
    }
  }

  /**
   * Clear grid cache
   */
  private clearGridCache(): void {
    this.gridCache.forEach(group => {
      group.destroy();
    });
    this.gridCache.clear();
  }

  /**
   * Clear intersection cache
   */
  private clearIntersectionCache(): void {
    this.intersectionCache.forEach(group => {
      group.destroy();
    });
    this.intersectionCache.clear();
  }
}