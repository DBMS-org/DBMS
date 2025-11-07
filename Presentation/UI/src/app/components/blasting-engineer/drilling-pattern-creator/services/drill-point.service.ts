import { Injectable } from '@angular/core';
import { DrillPoint, PatternData, PatternSettings, BlastSequenceData } from '../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../constants/canvas.constants';

@Injectable({
  providedIn: 'root'
})
export class DrillPointService {
  private currentId = 1;

  constructor() {}

  validateDrillPointCount(currentCount: number, maxPoints: number): boolean {
    if (currentCount >= maxPoints) {
      console.warn(`Maximum number of drill points (${maxPoints}) reached`);
      return false;
    }
    return true;
  }

  validateCoordinates(x: number, y: number): boolean {
    if (isNaN(x) || isNaN(y)) {
      console.warn('Invalid coordinates');
      return false;
    }
    return true;
  }

  validateUniqueCoordinates(x: number, y: number, existingPoints: DrillPoint[]): boolean {
    // Check if a drill point already exists at these coordinates
    const duplicate = existingPoints.find(point => 
      Math.abs(point.x - x) < 0.01 && Math.abs(point.y - y) < 0.01
    );
    
    if (duplicate) {
      console.warn(`A drill point already exists at coordinates (${x.toFixed(2)}, ${y.toFixed(2)})`);
      return false;
    }
    return true;
  }

  createDrillPoint(x: number, y: number, settings: PatternSettings, isPreciseMode: boolean = true): DrillPoint {
    // Safety check: ensure settings have valid values
    const safeSpacing = settings.spacing || CANVAS_CONSTANTS.DEFAULT_SETTINGS.spacing;
    const safeBurden = settings.burden || CANVAS_CONSTANTS.DEFAULT_SETTINGS.burden;
    const safeDepth = settings.depth || CANVAS_CONSTANTS.DEFAULT_SETTINGS.depth;
    
    console.log('createDrillPoint safety check:', {
      originalSettings: settings,
      safeValues: { spacing: safeSpacing, burden: safeBurden, depth: safeDepth },
      isPreciseMode: isPreciseMode
    });
    
    // Only align coordinates to grid if in precise mode
    let finalX = x;
    let finalY = y;
    
    if (isPreciseMode) {
      const alignedCoords = this.alignCoordinatesToGrid(x, y, safeSpacing, safeBurden);
      finalX = alignedCoords.x;
      finalY = alignedCoords.y;
      console.log(`Precise mode: aligned (${x.toFixed(2)}, ${y.toFixed(2)}) -> (${finalX}, ${finalY})`);
    } else {
      console.log(`Free mode: using exact coordinates (${x.toFixed(2)}, ${y.toFixed(2)})`);
    }
    
    return {
      x: Number(finalX.toFixed(2)),
      y: Number(finalY.toFixed(2)),
      id: `DH${this.currentId++}`, // Use DH1, DH2 format for ID instead of GUID
      displayName: `DH${this.currentId - 1}`, // Keep consistent with ID
      depth: safeDepth,
      spacing: safeSpacing,
      burden: safeBurden,
      stemming: settings.stemming,
      subDrill: settings.subDrill
    };
  }

  private alignCoordinatesToGrid(x: number, y: number, spacing: number, burden: number): { x: number, y: number } {
    if (!spacing || spacing <= 0 || isNaN(spacing)) {
      console.error('Invalid spacing value:', spacing, 'using default:', CANVAS_CONSTANTS.DEFAULT_SETTINGS.spacing);
      spacing = CANVAS_CONSTANTS.DEFAULT_SETTINGS.spacing;
    }
    if (!burden || burden <= 0 || isNaN(burden)) {
      console.error('Invalid burden value:', burden, 'using default:', CANVAS_CONSTANTS.DEFAULT_SETTINGS.burden);
      burden = CANVAS_CONSTANTS.DEFAULT_SETTINGS.burden;
    }
    
    const alignedX = Math.round(x / spacing) * spacing;
    const alignedY = Math.round(y / burden) * burden;
    
    console.log(`Aligning coordinates: (${x.toFixed(2)}, ${y.toFixed(2)}) -> (${alignedX}, ${alignedY}) with spacing=${spacing}, burden=${burden}`);
    
    return { x: alignedX, y: alignedY };
  }

  alignExistingPointsToGrid(drillPoints: DrillPoint[]): DrillPoint[] {
    return drillPoints.map(point => {
      const safeSpacing = point.spacing || CANVAS_CONSTANTS.DEFAULT_SETTINGS.spacing;
      const safeBurden = point.burden || CANVAS_CONSTANTS.DEFAULT_SETTINGS.burden;
      
      const alignedCoords = this.alignCoordinatesToGrid(point.x, point.y, safeSpacing, safeBurden);
      
      return {
        ...point,
        x: Number(alignedCoords.x.toFixed(2)),
        y: Number(alignedCoords.y.toFixed(2)),
        spacing: safeSpacing,
        burden: safeBurden
      };
    });
  }

  validatePointAlignment(point: DrillPoint): boolean {
    const tolerance = 0.01; // Small tolerance for floating point precision
    const expectedX = Math.round(point.x / point.spacing) * point.spacing;
    const expectedY = Math.round(point.y / point.burden) * point.burden;
    
    return Math.abs(point.x - expectedX) < tolerance && Math.abs(point.y - expectedY) < tolerance;
  }

  generateGridPattern(
    rows: number, 
    columns: number, 
    spacing: number, 
    burden: number, 
    depth: number,
    startX: number = 0,
    startY: number = 0,
    startAtSpacingBurden: boolean = false
  ): DrillPoint[] {
    const points: DrillPoint[] = [];
    const actualStartX = startAtSpacingBurden ? spacing : startX;
    const actualStartY = startAtSpacingBurden ? burden : startY;
    
    for (let row = 0; row < rows; row++) {
      for (let col = 0; col < columns; col++) {
        const x = actualStartX + (col * spacing);
        const y = actualStartY + (row * burden);
        
        points.push({
          x: Number(x.toFixed(2)),
          y: Number(y.toFixed(2)),
          id: `DH${this.currentId++}`,
          displayName: `DH${this.currentId - 1}`,
          depth: depth,
          spacing: spacing,
          burden: burden,
          stemming: CANVAS_CONSTANTS.DEFAULT_SETTINGS.stemming,
          subDrill: CANVAS_CONSTANTS.DEFAULT_SETTINGS.subDrill
        });
      }
    }
    
    console.log(`Generated ${points.length} drill points in ${rows}x${columns} grid`);
    console.log(`Starting at (${actualStartX}, ${actualStartY}) with spacing=${spacing}, burden=${burden}`);
    return points;
  }

  selectPoint(point: DrillPoint | null, points: DrillPoint[]): DrillPoint | null {
    if (!point) {
      return null;
    }
    return points.find(p => p.id === point.id) || null;
  }

  removePoint(point: DrillPoint, points: DrillPoint[]): DrillPoint[] {
    return points.filter(p => p.id !== point.id);
  }

  clearPoints(): DrillPoint[] {
    return [];
  }

  resetHoleNumbering(): void {
    this.currentId = 1;
  }

  setCurrentId(id: number): void {
    this.currentId = id;
  }

  getCurrentId(): number {
    return this.currentId;
  }

  exportPatternForBlastDesign(drillPoints: DrillPoint[], settings: PatternSettings): PatternData {
    return {
      drillPoints: drillPoints.map(p => ({ ...p })),
      settings: { ...settings }
    };
  }

  // New method to get pattern data for sharing between components
  getPatternData(drillPoints: DrillPoint[], settings: PatternSettings): PatternData {
    return {
      drillPoints: [...drillPoints],
      settings: { ...settings }
    };
  }

  /**
   * Robustly estimates grid pitch (spacing & burden).
   * Steps:
   *   1.  Collect all successive deltas for sorted coords.
   *   2.  Discard anything < 0.5 m (assumed measurement noise).
   *   3.  Round remaining deltas to nearest 0.1 m, build a frequency table.
   *   4.  If the most common bucket accounts for ≥10 % of samples, use it (mode).
   *      Otherwise return the median of the cleaned delta list.
   *   5.  Fallback to 1 m when no usable deltas exist.
   */
  calculateGridPitch(drillPoints: DrillPoint[]): { spacing: number; burden: number } {
    const estimate = (coords: number[]): number => {
      if (coords.length < 2) return 1;

      // Sort coordinates ascending
      const sorted = [...coords].sort((a, b) => a - b);

      // Build list of deltas, filter out sub-0.5 m jitter
      const deltas: number[] = [];
      for (let i = 1; i < sorted.length; i++) {
        const delta = +(sorted[i] - sorted[i - 1]).toFixed(3); // keep high precision initially
        if (delta >= 0.5) deltas.push(delta);
      }

      if (deltas.length === 0) return 1;

      // Bucket to 0.1 m resolution and build histogram
      const freq: Record<string, number> = {};
      deltas.forEach(d => {
        const bucket = (Math.round(d * 10) / 10).toFixed(1); // string key like "1.8"
        freq[bucket] = (freq[bucket] || 0) + 1;
      });

      // Determine mode and its support percentage
      const entries = Object.entries(freq).sort((a, b) => b[1] - a[1]);
      const [modeBucket, modeCount] = entries[0];
      const support = modeCount / deltas.length;

      if (support >= 0.10) {
        return parseFloat(modeBucket);
      }

      // Fallback: median of deltas rounded to 0.1 m
      deltas.sort((a, b) => a - b);
      const median = deltas[Math.floor(deltas.length / 2)];
      return +(Math.round(median * 10) / 10).toFixed(1);
    };

    return {
      spacing: estimate(drillPoints.map(p => p.x)),
      burden: estimate(drillPoints.map(p => p.y))
    };
  }

  /**
   * Euclidean distance between two drill points (unit: same as X,Y coordinates).
   */
  getDistance(a: DrillPoint, b: DrillPoint): number {
    const dx = a.x - b.x;
    const dy = a.y - b.y;
    return Math.sqrt(dx * dx + dy * dy);
  }

  /**
   * Generate a proper GUID/UUID v4
   */
  private generateGuid(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      const r = Math.random() * 16 | 0;
      const v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
}