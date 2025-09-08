import { Injectable } from '@angular/core';
import Konva from 'konva';
import { PatternSettings } from '../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../constants/canvas.constants';

@Injectable({
  providedIn: 'root'
})
export class GridService {
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

  clearGridCache(): void {
    console.log(`🗑️ Clearing grid cache (${this.gridCache.size} entries)`);
    this.gridCache.forEach((group, key) => {
      console.log(`  Destroying cached grid: ${key}`);
      group.destroy();
    });
    this.gridCache.clear();
    console.log('✅ Grid cache cleared');
  }

  /**
   * Clear cache entries that match a specific pattern (e.g., all entries for a specific canvas size)
   */
  clearCacheByPattern(pattern: RegExp): void {
    const keysToDelete: string[] = [];
    
    this.gridCache.forEach((group, key) => {
      if (pattern.test(key)) {
        keysToDelete.push(key);
        group.destroy();
      }
    });
    
    keysToDelete.forEach(key => {
      this.gridCache.delete(key);
    });
    
    console.log(`🗑️ Cleared ${keysToDelete.length} cache entries matching pattern: ${pattern}`);
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
    let totalLinesDrawn = 0;

    // Calculate grid dimensions
    const spacing = settings.spacing * CANVAS_CONSTANTS.GRID_SIZE * scale;
    const burden = settings.burden * CANVAS_CONSTANTS.GRID_SIZE * scale;

    // Calculate grid boundaries
    const startX = CANVAS_CONSTANTS.RULER_WIDTH;
    const startY = CANVAS_CONSTANTS.RULER_HEIGHT;
    const endX = width;
    const endY = height;

    // Align grid starting positions with ruler measurements
    let alignedGridX = startX + (offsetX % spacing);
    if (alignedGridX < startX) alignedGridX += spacing;
    
    let alignedGridY = startY + (offsetY % burden);
    if (alignedGridY < startY) alignedGridY += burden;

    // Draw minor grid lines
    const minorSpacing = spacing / 2;
    const minorBurden = burden / 2;

    // Draw minor vertical lines (aligned with ruler) - with limit check
    for (let x = alignedGridX - spacing; x < endX && totalLinesDrawn < CANVAS_CONSTANTS.MAX_GRID_LINES; x += minorSpacing) {
      if (x >= startX) {
        gridGroup.add(new Konva.Line({
          points: [x, startY, x, endY],
          stroke: 'rgba(200, 200, 200, 0.3)',
          listening: false
        }));
        totalLinesDrawn++;
      }
    }

    // Draw minor horizontal lines (aligned with ruler) - with limit check
    for (let y = alignedGridY - burden; y < endY && totalLinesDrawn < CANVAS_CONSTANTS.MAX_GRID_LINES; y += minorBurden) {
      if (y >= startY) {
        gridGroup.add(new Konva.Line({
          points: [startX, y, endX, y],
          stroke: 'rgba(200, 200, 200, 0.3)',
          listening: false
        }));
        totalLinesDrawn++;
      }
    }

    // Draw major grid lines
    // Draw major vertical lines (aligned with ruler measurements) - with limit check
    for (let x = alignedGridX; x < endX && totalLinesDrawn < CANVAS_CONSTANTS.MAX_GRID_LINES; x += spacing) {
      if (x >= startX) {
        gridGroup.add(new Konva.Line({
          points: [x, startY, x, endY],
          stroke: 'rgba(150, 150, 150, 0.5)',
          listening: false
        }));
        totalLinesDrawn++;
      }
    }

    // Draw major horizontal lines (aligned with ruler measurements) - with limit check
    for (let y = alignedGridY; y < endY && totalLinesDrawn < CANVAS_CONSTANTS.MAX_GRID_LINES; y += burden) {
      if (y >= startY) {
        gridGroup.add(new Konva.Line({
          points: [startX, y, endX, y],
          stroke: 'rgba(150, 150, 150, 0.5)',
          listening: false
        }));
        totalLinesDrawn++;
      }
    }

    // Log warning if we hit the limit
    if (totalLinesDrawn >= CANVAS_CONSTANTS.MAX_GRID_LINES) {
      console.warn(`Grid line limit of ${CANVAS_CONSTANTS.MAX_GRID_LINES} reached. Consider zooming in or increasing spacing/burden values for better performance.`);
    }

    return gridGroup;
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
    const intersectionGroup = new Konva.Group();
    
    if (!isPreciseMode) {
      return intersectionGroup;
    }

    let totalIntersections = 0;
    const maxIntersections = Math.floor(CANVAS_CONSTANTS.MAX_GRID_LINES / 4); // Limit intersections to 1/4 of max grid lines

    // Calculate grid dimensions
    const spacing = settings.spacing * CANVAS_CONSTANTS.GRID_SIZE * scale;
    const burden = settings.burden * CANVAS_CONSTANTS.GRID_SIZE * scale;

    // Calculate grid boundaries
    const startX = CANVAS_CONSTANTS.RULER_WIDTH;
    const startY = CANVAS_CONSTANTS.RULER_HEIGHT;
    const endX = width;
    const endY = height;

    // Calculate grid starting positions with pan offsets (aligned with rulers)
    let alignedGridX = startX + (offsetX % spacing);
    if (alignedGridX < startX) alignedGridX += spacing;
    
    let alignedGridY = startY + (offsetY % burden);
    if (alignedGridY < startY) alignedGridY += burden;

    // Draw intersection points where major grid lines meet (aligned with ruler measurements)
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
      console.warn(`Grid intersection limit of ${maxIntersections} reached. Consider zooming in or increasing spacing/burden values.`);
    }

    return intersectionGroup;
  }
} 