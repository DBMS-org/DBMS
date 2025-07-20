import { Injectable } from '@angular/core';
import Konva from 'konva';
import { IGridRenderingService } from '../interfaces/service.interfaces';
import { PatternSettings } from '../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../constants/canvas.constants';
import { Logger } from '../utils/logger.util';

/**
 * GridRenderingService - Handles all grid rendering operations
 * 
 * This service implements the IGridRenderingService interface and provides
 * optimized grid rendering with caching, intersection indicators, and
 * performance monitoring.
 */
@Injectable({
  providedIn: 'root'
})
export class GridRenderingService implements IGridRenderingService {
  
  private layer: Konva.Layer | null = null;
  private gridGroup: Konva.Group | null = null;
  private intersectionGroup: Konva.Group | null = null;
  
  // Grid properties
  private gridSpacing: number = CANVAS_CONSTANTS.DEFAULT_SETTINGS.spacing;
  private gridColor: string = '#e0e0e0';
  private isVisible: boolean = true;
  private cachingEnabled: boolean = true;
  private showPreciseModeIndicators: boolean = false;
  
  // Cache management
  private gridCache: Map<string, Konva.Group> = new Map();
  private intersectionCache: Map<string, Konva.Group> = new Map();
  private lastCacheKey: string = '';
  
  // Performance tracking
  private renderCount: number = 0;
  private cacheHits: number = 0;
  private lastRenderTime: number = 0;

  /**
   * Initialize the rendering service with a Konva layer
   */
  public initialize(layer: Konva.Layer): void {
    if (!layer) {
      throw new Error('Layer is required for grid rendering service initialization');
    }

    this.layer = layer;
    this.gridGroup = new Konva.Group({ name: 'grid-main' });
    this.intersectionGroup = new Konva.Group({ name: 'grid-intersections' });
    
    // Add groups to layer
    this.layer.add(this.gridGroup);
    this.layer.add(this.intersectionGroup);
    
    Logger.info('Grid rendering service initialized');
  }

  /**
   * Render the grid
   */
  public render(): void {
    if (!this.layer || !this.gridGroup) {
      Logger.warn('Grid rendering service not properly initialized');
      return;
    }

    const startTime = Date.now();
    this.renderCount++;

    try {
      // Generate cache key based on current state
      const cacheKey = this.generateCacheKey();
      
      // Try to use cached grid if available and caching is enabled
      if (this.cachingEnabled && this.tryUseCachedGrid(cacheKey)) {
        this.cacheHits++;
        this.lastRenderTime = Date.now() - startTime;
        return;
      }

      // Clear existing grid
      this.clear();

      // Render new grid
      this.renderGridLines();
      
      // Render intersections if in precise mode
      if (this.showPreciseModeIndicators) {
        this.renderIntersections();
      }

      // Cache the rendered grid
      if (this.cachingEnabled) {
        this.cacheGrid(cacheKey);
      }

      // Update layer
      this.layer.batchDraw();
      
      this.lastRenderTime = Date.now() - startTime;
      
      Logger.debug('Grid rendered', { 
        renderTime: this.lastRenderTime,
        cacheKey,
        cached: false
      });

    } catch (error) {
      Logger.error('Grid rendering failed', error);
      throw error;
    }
  }

  /**
   * Update the grid (alias for render)
   */
  public update(): void {
    this.render();
  }

  /**
   * Clear the grid
   */
  public clear(): void {
    if (this.gridGroup) {
      this.gridGroup.destroyChildren();
    }
    if (this.intersectionGroup) {
      this.intersectionGroup.destroyChildren();
    }
  }

  /**
   * Destroy the rendering service and clean up resources
   */
  public destroy(): void {
    this.clear();
    this.clearCache();
    
    if (this.gridGroup) {
      this.gridGroup.destroy();
      this.gridGroup = null;
    }
    
    if (this.intersectionGroup) {
      this.intersectionGroup.destroy();
      this.intersectionGroup = null;
    }
    
    this.layer = null;
    
    Logger.info('Grid rendering service destroyed');
  }

  /**
   * Enable or disable caching
   */
  public enableCaching(enabled: boolean): void {
    this.cachingEnabled = enabled;
    
    if (!enabled) {
      this.clearCache();
    }
    
    Logger.debug('Grid caching', { enabled });
  }

  /**
   * Invalidate cache to force re-render
   */
  public invalidateCache(): void {
    this.clearCache();
    Logger.debug('Grid cache invalidated');
  }

  /**
   * Set visibility of the grid
   */
  public setVisible(visible: boolean): void {
    this.isVisible = visible;
    
    if (this.gridGroup) {
      this.gridGroup.visible(visible);
    }
    
    if (this.intersectionGroup) {
      this.intersectionGroup.visible(visible);
    }
    
    if (this.layer) {
      this.layer.batchDraw();
    }
  }

  /**
   * Check if the grid is visible
   */
  public isVisible(): boolean {
    return this.isVisible;
  }

  /**
   * Set grid spacing
   */
  public setGridSpacing(spacing: number): void {
    if (spacing <= 0) {
      throw new Error('Grid spacing must be positive');
    }
    
    this.gridSpacing = spacing;
    this.invalidateCache();
    
    Logger.debug('Grid spacing updated', { spacing });
  }

  /**
   * Set grid color
   */
  public setGridColor(color: string): void {
    if (!color || typeof color !== 'string') {
      throw new Error('Invalid grid color');
    }
    
    this.gridColor = color;
    this.invalidateCache();
    
    Logger.debug('Grid color updated', { color });
  }

  /**
   * Show or hide precise mode indicators
   */
  public showPreciseMode(enabled: boolean): void {
    this.showPreciseModeIndicators = enabled;
    
    if (this.intersectionGroup) {
      this.intersectionGroup.visible(enabled);
      
      if (this.layer) {
        this.layer.batchDraw();
      }
    }
    
    Logger.debug('Precise mode indicators', { enabled });
  }

  /**
   * Highlight specific intersection points
   */
  public highlightIntersections(points: Array<{ x: number; y: number }>): void {
    if (!this.intersectionGroup || !points.length) return;

    try {
      // Clear existing highlights
      const highlights = this.intersectionGroup.find('.highlight');
      highlights.forEach(highlight => highlight.destroy());

      // Add new highlights
      points.forEach((point, index) => {
        const highlight = new Konva.Circle({
          x: point.x,
          y: point.y,
          radius: 4,
          fill: 'rgba(255, 193, 7, 0.8)', // Amber highlight
          stroke: 'rgba(255, 152, 0, 1)',
          strokeWidth: 2,
          name: 'highlight',
          listening: false
        });
        
        this.intersectionGroup!.add(highlight);
      });

      if (this.layer) {
        this.layer.batchDraw();
      }

      Logger.debug('Intersection highlights updated', { count: points.length });

    } catch (error) {
      Logger.error('Failed to highlight intersections', error);
    }
  }

  /**
   * Get performance statistics
   */
  public getPerformanceStats(): {
    renderCount: number;
    cacheHits: number;
    cacheHitRate: number;
    lastRenderTime: number;
    cacheSize: number;
  } {
    return {
      renderCount: this.renderCount,
      cacheHits: this.cacheHits,
      cacheHitRate: this.renderCount > 0 ? this.cacheHits / this.renderCount : 0,
      lastRenderTime: this.lastRenderTime,
      cacheSize: this.gridCache.size + this.intersectionCache.size
    };
  }

  /**
   * Render grid lines
   */
  private renderGridLines(): void {
    if (!this.layer || !this.gridGroup) return;

    const stage = this.layer.getStage();
    if (!stage) return;

    const width = stage.width();
    const height = stage.height();
    const scale = stage.scaleX();
    const position = stage.position();

    // Calculate grid parameters
    const spacing = this.gridSpacing * CANVAS_CONSTANTS.GRID_SIZE * scale;
    const burden = this.gridSpacing * CANVAS_CONSTANTS.GRID_SIZE * scale; // Using same value for burden
    
    // Calculate visible area
    const startX = CANVAS_CONSTANTS.RULER_WIDTH;
    const startY = CANVAS_CONSTANTS.RULER_HEIGHT;
    const endX = width;
    const endY = height;

    // Calculate grid starting positions with pan offsets
    let alignedGridX = startX + (position.x % spacing);
    if (alignedGridX < startX) alignedGridX += spacing;
    
    let alignedGridY = startY + (position.y % burden);
    if (alignedGridY < startY) alignedGridY += burden;

    let totalLinesDrawn = 0;
    const maxLines = CANVAS_CONSTANTS.MAX_GRID_LINES;

    // Draw minor grid lines (lighter)
    const minorSpacing = spacing / 2;
    const minorBurden = burden / 2;
    const minorColor = this.adjustColorOpacity(this.gridColor, 0.3);

    // Minor vertical lines
    for (let x = alignedGridX - spacing; x < endX && totalLinesDrawn < maxLines; x += minorSpacing) {
      if (x >= startX) {
        this.gridGroup.add(new Konva.Line({
          points: [x, startY, x, endY],
          stroke: minorColor,
          strokeWidth: 0.5,
          listening: false,
          perfectDrawEnabled: false
        }));
        totalLinesDrawn++;
      }
    }

    // Minor horizontal lines
    for (let y = alignedGridY - burden; y < endY && totalLinesDrawn < maxLines; y += minorBurden) {
      if (y >= startY) {
        this.gridGroup.add(new Konva.Line({
          points: [startX, y, endX, y],
          stroke: minorColor,
          strokeWidth: 0.5,
          listening: false,
          perfectDrawEnabled: false
        }));
        totalLinesDrawn++;
      }
    }

    // Draw major grid lines (darker)
    const majorColor = this.adjustColorOpacity(this.gridColor, 0.6);

    // Major vertical lines
    for (let x = alignedGridX; x < endX && totalLinesDrawn < maxLines; x += spacing) {
      if (x >= startX) {
        this.gridGroup.add(new Konva.Line({
          points: [x, startY, x, endY],
          stroke: majorColor,
          strokeWidth: 1,
          listening: false,
          perfectDrawEnabled: false
        }));
        totalLinesDrawn++;
      }
    }

    // Major horizontal lines
    for (let y = alignedGridY; y < endY && totalLinesDrawn < maxLines; y += burden) {
      if (y >= startY) {
        this.gridGroup.add(new Konva.Line({
          points: [startX, y, endX, y],
          stroke: majorColor,
          strokeWidth: 1,
          listening: false,
          perfectDrawEnabled: false
        }));
        totalLinesDrawn++;
      }
    }

    // Log warning if we hit the limit
    if (totalLinesDrawn >= maxLines) {
      Logger.warn(`Grid line limit of ${maxLines} reached`);
    }
  }

  /**
   * Render intersection indicators for precise mode
   */
  private renderIntersections(): void {
    if (!this.layer || !this.intersectionGroup) return;

    const stage = this.layer.getStage();
    if (!stage) return;

    const width = stage.width();
    const height = stage.height();
    const scale = stage.scaleX();
    const position = stage.position();

    // Calculate grid parameters
    const spacing = this.gridSpacing * CANVAS_CONSTANTS.GRID_SIZE * scale;
    const burden = this.gridSpacing * CANVAS_CONSTANTS.GRID_SIZE * scale;
    
    // Calculate visible area
    const startX = CANVAS_CONSTANTS.RULER_WIDTH;
    const startY = CANVAS_CONSTANTS.RULER_HEIGHT;
    const endX = width;
    const endY = height;

    // Calculate grid starting positions
    let alignedGridX = startX + (position.x % spacing);
    if (alignedGridX < startX) alignedGridX += spacing;
    
    let alignedGridY = startY + (position.y % burden);
    if (alignedGridY < startY) alignedGridY += burden;

    let totalIntersections = 0;
    const maxIntersections = Math.floor(CANVAS_CONSTANTS.MAX_GRID_LINES / 4);

    // Draw intersection points
    for (let x = alignedGridX; x < endX && totalIntersections < maxIntersections; x += spacing) {
      for (let y = alignedGridY; y < endY && totalIntersections < maxIntersections; y += burden) {
        if (x >= startX && y >= startY) {
          const intersectionPoint = new Konva.Circle({
            x: x,
            y: y,
            radius: 2,
            fill: 'rgba(255, 165, 0, 0.8)', // Orange color
            stroke: 'rgba(255, 140, 0, 1)',
            strokeWidth: 1,
            listening: false,
            perfectDrawEnabled: false
          });
          
          this.intersectionGroup.add(intersectionPoint);
          totalIntersections++;
        }
      }
    }

    // Log warning if we hit the intersection limit
    if (totalIntersections >= maxIntersections) {
      Logger.warn(`Grid intersection limit of ${maxIntersections} reached`);
    }
  }

  /**
   * Generate cache key based on current rendering state
   */
  private generateCacheKey(): string {
    if (!this.layer) return '';

    const stage = this.layer.getStage();
    if (!stage) return '';

    const width = stage.width();
    const height = stage.height();
    const scale = stage.scaleX();
    const position = stage.position();

    return `grid_${this.gridSpacing}_${this.gridColor}_${scale.toFixed(2)}_${position.x.toFixed(0)}_${position.y.toFixed(0)}_${width}x${height}_${this.showPreciseModeIndicators}`;
  }

  /**
   * Try to use cached grid
   */
  private tryUseCachedGrid(cacheKey: string): boolean {
    const cachedGrid = this.gridCache.get(cacheKey);
    const cachedIntersections = this.intersectionCache.get(cacheKey);

    if (cachedGrid && this.gridGroup) {
      // Clone cached grid
      const clonedGrid = cachedGrid.clone();
      this.gridGroup.add(...clonedGrid.getChildren());
      
      // Add cached intersections if available and needed
      if (cachedIntersections && this.intersectionGroup && this.showPreciseModeIndicators) {
        const clonedIntersections = cachedIntersections.clone();
        this.intersectionGroup.add(...clonedIntersections.getChildren());
      }

      this.layer!.batchDraw();
      
      Logger.debug('Used cached grid', { cacheKey });
      return true;
    }

    return false;
  }

  /**
   * Cache the current grid
   */
  private cacheGrid(cacheKey: string): void {
    if (!this.gridGroup || !this.intersectionGroup) return;

    try {
      // Cache main grid
      const gridClone = this.gridGroup.clone();
      this.gridCache.set(cacheKey, gridClone);

      // Cache intersections if they exist
      if (this.showPreciseModeIndicators && this.intersectionGroup.hasChildren()) {
        const intersectionClone = this.intersectionGroup.clone();
        this.intersectionCache.set(cacheKey, intersectionClone);
      }

      // Limit cache size
      this.limitCacheSize();
      
      this.lastCacheKey = cacheKey;

    } catch (error) {
      Logger.error('Failed to cache grid', error);
    }
  }

  /**
   * Clear all caches
   */
  private clearCache(): void {
    // Destroy cached objects
    this.gridCache.forEach(group => group.destroy());
    this.intersectionCache.forEach(group => group.destroy());
    
    // Clear maps
    this.gridCache.clear();
    this.intersectionCache.clear();
    
    this.lastCacheKey = '';
  }

  /**
   * Limit cache size to prevent memory issues
   */
  private limitCacheSize(): void {
    const maxCacheSize = CANVAS_CONSTANTS.MAX_CACHE_SIZE;
    
    // Limit grid cache
    if (this.gridCache.size > maxCacheSize) {
      const keys = Array.from(this.gridCache.keys());
      const oldestKey = keys[0];
      const oldGroup = this.gridCache.get(oldestKey);
      if (oldGroup) {
        oldGroup.destroy();
      }
      this.gridCache.delete(oldestKey);
    }
    
    // Limit intersection cache
    if (this.intersectionCache.size > maxCacheSize) {
      const keys = Array.from(this.intersectionCache.keys());
      const oldestKey = keys[0];
      const oldGroup = this.intersectionCache.get(oldestKey);
      if (oldGroup) {
        oldGroup.destroy();
      }
      this.intersectionCache.delete(oldestKey);
    }
  }

  /**
   * Adjust color opacity
   */
  private adjustColorOpacity(color: string, opacity: number): string {
    // Simple implementation - in production, you might want a more robust color parser
    if (color.startsWith('#')) {
      // Convert hex to rgba
      const hex = color.slice(1);
      const r = parseInt(hex.slice(0, 2), 16);
      const g = parseInt(hex.slice(2, 4), 16);
      const b = parseInt(hex.slice(4, 6), 16);
      return `rgba(${r}, ${g}, ${b}, ${opacity})`;
    }
    
    // If already rgba, modify opacity
    if (color.startsWith('rgba')) {
      return color.replace(/[\d\.]+\)$/g, `${opacity})`);
    }
    
    // If rgb, convert to rgba
    if (color.startsWith('rgb')) {
      return color.replace('rgb', 'rgba').replace(')', `, ${opacity})`);
    }
    
    // Fallback
    return `rgba(224, 224, 224, ${opacity})`;
  }
}