import { Component, Input, OnInit, OnDestroy, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import Konva from 'konva';
import { PatternSettings } from '../../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../../constants/canvas.constants';
import { Logger } from '../../utils/logger.util';

/**
 * RulerCanvasComponent handles all ruler rendering operations for the drilling pattern creator.
 * This component is responsible for:
 * - Rendering horizontal and vertical rulers with measurement marks
 * - Managing ruler scaling and positioning logic
 * - Handling ruler updates for pan/zoom operations
 * - Optimizing ruler rendering for performance
 */
@Component({
  selector: 'app-ruler-canvas',
  standalone: true,
  imports: [CommonModule],
  template: '', // No template needed - this is a service component
  styleUrls: []
})
export class RulerCanvasComponent implements OnInit, OnDestroy, OnChanges {
  @Input() layer!: Konva.Layer;
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

  // Ruler objects
  private horizontalRulerGroup: Konva.Group | null = null;
  private verticalRulerGroup: Konva.Group | null = null;
  private rulerBackgroundGroup: Konva.Group | null = null;

  // Ruler caching mechanism
  private rulerCache: Map<string, { horizontal: Konva.Group; vertical: Konva.Group; background: Konva.Group }> = new Map();

  // Performance tracking
  private lastRenderTime: number = 0;
  private renderCount: number = 0;

  // Ruler configuration
  private readonly RULER_CONFIG = {
    MAJOR_TICK_HEIGHT: 15,
    MINOR_TICK_HEIGHT: 8,
    MICRO_TICK_HEIGHT: 4,
    TEXT_OFFSET: 20,
    BACKGROUND_COLOR: '#f5f5f5',
    BORDER_COLOR: '#cccccc',
    TICK_COLOR: '#666666',
    TEXT_COLOR: '#333333',
    FONT_SIZE: 10,
    FONT_FAMILY: 'Arial, sans-serif'
  };

  constructor() {}

  ngOnInit(): void {
    Logger.info('RulerCanvasComponent initialized');
  }

  ngOnDestroy(): void {
    Logger.info('RulerCanvasComponent destroying');
    this.clearAllCaches();
    this.destroyRulerObjects();
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Check if any inputs that affect ruler rendering have changed
    const shouldUpdateRuler = 
      changes['settings'] || 
      changes['canvasState'];

    if (shouldUpdateRuler && this.layer) {
      this.updateRuler();
    }
  }

  /**
   * Main method to render the rulers
   */
  public renderRuler(): void {
    if (!this.layer || !this.settings || !this.canvasState) {
      Logger.warn('RulerCanvasComponent: Missing required inputs for ruler rendering');
      return;
    }

    const startTime = performance.now();
    
    try {
      this.renderRulerBackground();
      this.renderHorizontalRuler();
      this.renderVerticalRuler();
      this.layer.batchDraw();
      
      this.lastRenderTime = performance.now() - startTime;
      this.renderCount++;
      
      Logger.debug(`Ruler rendered in ${this.lastRenderTime.toFixed(2)}ms (render #${this.renderCount})`);
    } catch (error) {
      Logger.error('Error rendering ruler:', error);
    }
  }

  /**
   * Update ruler when settings or state changes
   */
  public updateRuler(): void {
    this.renderRuler();
  }

  /**
   * Clear the ruler and remove from layer
   */
  public clearRuler(): void {
    this.destroyRulerObjects();
    if (this.layer) {
      this.layer.batchDraw();
    }
  }

  /**
   * Get ruler cache statistics for debugging
   */
  public getCacheStats(): { cacheSize: number; lastRenderTime: number; renderCount: number } {
    return {
      cacheSize: this.rulerCache.size,
      lastRenderTime: this.lastRenderTime,
      renderCount: this.renderCount
    };
  }

  /**
   * Force clear all caches (useful for testing or memory management)
   */
  public clearAllCaches(): void {
    this.clearRulerCache();
  }

  // Private methods

  /**
   * Render ruler background
   */
  private renderRulerBackground(): void {
    if (this.rulerBackgroundGroup) {
      this.rulerBackgroundGroup.destroy();
    }

    this.rulerBackgroundGroup = new Konva.Group();

    // Horizontal ruler background
    const horizontalBackground = new Konva.Rect({
      x: 0,
      y: 0,
      width: this.canvasState.width,
      height: CANVAS_CONSTANTS.RULER_HEIGHT,
      fill: this.RULER_CONFIG.BACKGROUND_COLOR,
      stroke: this.RULER_CONFIG.BORDER_COLOR,
      strokeWidth: 1,
      listening: false
    });

    // Vertical ruler background
    const verticalBackground = new Konva.Rect({
      x: 0,
      y: 0,
      width: CANVAS_CONSTANTS.RULER_WIDTH,
      height: this.canvasState.height,
      fill: this.RULER_CONFIG.BACKGROUND_COLOR,
      stroke: this.RULER_CONFIG.BORDER_COLOR,
      strokeWidth: 1,
      listening: false
    });

    // Corner square
    const cornerSquare = new Konva.Rect({
      x: 0,
      y: 0,
      width: CANVAS_CONSTANTS.RULER_WIDTH,
      height: CANVAS_CONSTANTS.RULER_HEIGHT,
      fill: this.RULER_CONFIG.BACKGROUND_COLOR,
      stroke: this.RULER_CONFIG.BORDER_COLOR,
      strokeWidth: 1,
      listening: false
    });

    this.rulerBackgroundGroup.add(horizontalBackground);
    this.rulerBackgroundGroup.add(verticalBackground);
    this.rulerBackgroundGroup.add(cornerSquare);
    
    this.layer.add(this.rulerBackgroundGroup);
  }

  /**
   * Render horizontal ruler with caching
   */
  private renderHorizontalRuler(): void {
    const cacheKey = this.generateRulerCacheKey();
    
    // Check cache first
    const cachedRuler = this.rulerCache.get(cacheKey);
    if (cachedRuler) {
      this.replaceHorizontalRulerGroup(cachedRuler.horizontal.clone());
      Logger.debug('Using cached horizontal ruler');
      return;
    }

    // Create new horizontal ruler
    const newHorizontalRulerGroup = this.createHorizontalRuler();
    this.replaceHorizontalRulerGroup(newHorizontalRulerGroup);
    
    Logger.debug('Created new horizontal ruler');
  }

  /**
   * Render vertical ruler with caching
   */
  private renderVerticalRuler(): void {
    const cacheKey = this.generateRulerCacheKey();
    
    // Check cache first
    const cachedRuler = this.rulerCache.get(cacheKey);
    if (cachedRuler) {
      this.replaceVerticalRulerGroup(cachedRuler.vertical.clone());
      Logger.debug('Using cached vertical ruler');
      return;
    }

    // Create new vertical ruler
    const newVerticalRulerGroup = this.createVerticalRuler();
    this.replaceVerticalRulerGroup(newVerticalRulerGroup);
    
    // Cache both rulers together
    this.updateRulerCache(cacheKey);
    
    Logger.debug('Created new vertical ruler and cached both rulers');
  }

  /**
   * Create horizontal ruler
   */
  private createHorizontalRuler(): Konva.Group {
    const horizontalRulerGroup = new Konva.Group();
    
    const { scale, offsetX, panOffsetX, width } = this.canvasState;
    const totalOffsetX = offsetX + panOffsetX;
    
    // Calculate the measurement unit based on scale
    const baseUnit = this.calculateOptimalUnit(scale);
    const pixelsPerUnit = baseUnit * CANVAS_CONSTANTS.GRID_SIZE * scale;
    
    // Calculate starting position
    const startX = CANVAS_CONSTANTS.RULER_WIDTH;
    const endX = width;
    
    // Calculate the first measurement position
    const firstMeasurement = Math.floor(-totalOffsetX / pixelsPerUnit) * baseUnit;
    let currentMeasurement = firstMeasurement;
    let currentX = startX + (firstMeasurement * pixelsPerUnit) + totalOffsetX;
    
    let tickCount = 0;
    const maxTicks = 200; // Prevent too many ticks
    
    while (currentX < endX && tickCount < maxTicks) {
      if (currentX >= startX) {
        this.drawHorizontalTick(horizontalRulerGroup, currentX, currentMeasurement, baseUnit);
      }
      
      currentMeasurement += baseUnit;
      currentX += pixelsPerUnit;
      tickCount++;
    }
    
    return horizontalRulerGroup;
  }

  /**
   * Create vertical ruler
   */
  private createVerticalRuler(): Konva.Group {
    const verticalRulerGroup = new Konva.Group();
    
    const { scale, offsetY, panOffsetY, height } = this.canvasState;
    const totalOffsetY = offsetY + panOffsetY;
    
    // Calculate the measurement unit based on scale
    const baseUnit = this.calculateOptimalUnit(scale);
    const pixelsPerUnit = baseUnit * CANVAS_CONSTANTS.GRID_SIZE * scale;
    
    // Calculate starting position
    const startY = CANVAS_CONSTANTS.RULER_HEIGHT;
    const endY = height;
    
    // Calculate the first measurement position
    const firstMeasurement = Math.floor(-totalOffsetY / pixelsPerUnit) * baseUnit;
    let currentMeasurement = firstMeasurement;
    let currentY = startY + (firstMeasurement * pixelsPerUnit) + totalOffsetY;
    
    let tickCount = 0;
    const maxTicks = 200; // Prevent too many ticks
    
    while (currentY < endY && tickCount < maxTicks) {
      if (currentY >= startY) {
        this.drawVerticalTick(verticalRulerGroup, currentY, currentMeasurement, baseUnit);
      }
      
      currentMeasurement += baseUnit;
      currentY += pixelsPerUnit;
      tickCount++;
    }
    
    return verticalRulerGroup;
  }

  /**
   * Draw horizontal tick mark and label
   */
  private drawHorizontalTick(group: Konva.Group, x: number, measurement: number, baseUnit: number): void {
    const isMainTick = measurement % (baseUnit * 5) === 0;
    const isMajorTick = measurement % baseUnit === 0;
    
    let tickHeight: number;
    if (isMainTick) {
      tickHeight = this.RULER_CONFIG.MAJOR_TICK_HEIGHT;
    } else if (isMajorTick) {
      tickHeight = this.RULER_CONFIG.MINOR_TICK_HEIGHT;
    } else {
      tickHeight = this.RULER_CONFIG.MICRO_TICK_HEIGHT;
    }
    
    // Draw tick line
    const tick = new Konva.Line({
      points: [x, CANVAS_CONSTANTS.RULER_HEIGHT - tickHeight, x, CANVAS_CONSTANTS.RULER_HEIGHT],
      stroke: this.RULER_CONFIG.TICK_COLOR,
      strokeWidth: 1,
      listening: false
    });
    group.add(tick);
    
    // Add label for main ticks
    if (isMainTick) {
      const label = new Konva.Text({
        x: x - 15,
        y: 5,
        text: measurement.toFixed(1),
        fontSize: this.RULER_CONFIG.FONT_SIZE,
        fontFamily: this.RULER_CONFIG.FONT_FAMILY,
        fill: this.RULER_CONFIG.TEXT_COLOR,
        align: 'center',
        width: 30,
        listening: false
      });
      group.add(label);
    }
  }

  /**
   * Draw vertical tick mark and label
   */
  private drawVerticalTick(group: Konva.Group, y: number, measurement: number, baseUnit: number): void {
    const isMainTick = measurement % (baseUnit * 5) === 0;
    const isMajorTick = measurement % baseUnit === 0;
    
    let tickHeight: number;
    if (isMainTick) {
      tickHeight = this.RULER_CONFIG.MAJOR_TICK_HEIGHT;
    } else if (isMajorTick) {
      tickHeight = this.RULER_CONFIG.MINOR_TICK_HEIGHT;
    } else {
      tickHeight = this.RULER_CONFIG.MICRO_TICK_HEIGHT;
    }
    
    // Draw tick line
    const tick = new Konva.Line({
      points: [CANVAS_CONSTANTS.RULER_WIDTH - tickHeight, y, CANVAS_CONSTANTS.RULER_WIDTH, y],
      stroke: this.RULER_CONFIG.TICK_COLOR,
      strokeWidth: 1,
      listening: false
    });
    group.add(tick);
    
    // Add label for main ticks
    if (isMainTick) {
      const label = new Konva.Text({
        x: 5,
        y: y - 6,
        text: measurement.toFixed(1),
        fontSize: this.RULER_CONFIG.FONT_SIZE,
        fontFamily: this.RULER_CONFIG.FONT_FAMILY,
        fill: this.RULER_CONFIG.TEXT_COLOR,
        listening: false
      });
      group.add(label);
    }
  }

  /**
   * Calculate optimal measurement unit based on scale
   */
  private calculateOptimalUnit(scale: number): number {
    // Base units in meters
    const units = [0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 50, 100];
    
    // Target pixel distance between major ticks (50-100 pixels is optimal)
    const targetPixelDistance = 75;
    const currentPixelDistance = CANVAS_CONSTANTS.GRID_SIZE * scale;
    
    // Find the unit that gives us the closest to target pixel distance
    let bestUnit = units[0];
    let bestDistance = Math.abs(currentPixelDistance * bestUnit - targetPixelDistance);
    
    for (const unit of units) {
      const distance = Math.abs(currentPixelDistance * unit - targetPixelDistance);
      if (distance < bestDistance) {
        bestDistance = distance;
        bestUnit = unit;
      }
    }
    
    return bestUnit;
  }

  /**
   * Replace the current horizontal ruler group with a new one
   */
  private replaceHorizontalRulerGroup(newHorizontalRulerGroup: Konva.Group): void {
    if (this.horizontalRulerGroup) {
      this.horizontalRulerGroup.destroy();
    }
    this.horizontalRulerGroup = newHorizontalRulerGroup;
    this.layer.add(this.horizontalRulerGroup);
  }

  /**
   * Replace the current vertical ruler group with a new one
   */
  private replaceVerticalRulerGroup(newVerticalRulerGroup: Konva.Group): void {
    if (this.verticalRulerGroup) {
      this.verticalRulerGroup.destroy();
    }
    this.verticalRulerGroup = newVerticalRulerGroup;
    this.layer.add(this.verticalRulerGroup);
  }

  /**
   * Destroy all ruler objects
   */
  private destroyRulerObjects(): void {
    if (this.horizontalRulerGroup) {
      this.horizontalRulerGroup.destroy();
      this.horizontalRulerGroup = null;
    }
    if (this.verticalRulerGroup) {
      this.verticalRulerGroup.destroy();
      this.verticalRulerGroup = null;
    }
    if (this.rulerBackgroundGroup) {
      this.rulerBackgroundGroup.destroy();
      this.rulerBackgroundGroup = null;
    }
  }

  /**
   * Generate cache key for rulers
   */
  private generateRulerCacheKey(): string {
    const { scale, offsetX, offsetY, panOffsetX, panOffsetY, width, height } = this.canvasState;
    return `ruler-${scale}-${offsetX + panOffsetX}-${offsetY + panOffsetY}-${width}-${height}`;
  }

  /**
   * Update ruler cache with size limit
   */
  private updateRulerCache(cacheKey: string): void {
    if (this.horizontalRulerGroup && this.verticalRulerGroup && this.rulerBackgroundGroup) {
      this.rulerCache.set(cacheKey, {
        horizontal: this.horizontalRulerGroup.clone(),
        vertical: this.verticalRulerGroup.clone(),
        background: this.rulerBackgroundGroup.clone()
      });
      
      if (this.rulerCache.size > CANVAS_CONSTANTS.MAX_CACHE_SIZE) {
        const keys = Array.from(this.rulerCache.keys());
        if (keys.length > 0) {
          const oldRuler = this.rulerCache.get(keys[0]);
          if (oldRuler) {
            oldRuler.horizontal.destroy();
            oldRuler.vertical.destroy();
            oldRuler.background.destroy();
          }
          this.rulerCache.delete(keys[0]);
        }
      }
    }
  }

  /**
   * Clear ruler cache
   */
  private clearRulerCache(): void {
    this.rulerCache.forEach(ruler => {
      ruler.horizontal.destroy();
      ruler.vertical.destroy();
      ruler.background.destroy();
    });
    this.rulerCache.clear();
  }
}