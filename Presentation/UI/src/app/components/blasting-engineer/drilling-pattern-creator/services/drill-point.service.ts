import { Injectable } from '@angular/core';
import { DrillPoint, PatternData, PatternSettings, BlastSequenceData } from '../models/drill-point.model';

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

  createDrillPoint(x: number, y: number, settings: PatternSettings): DrillPoint {
    return {
      x: Number(x.toFixed(2)),
      y: Number(y.toFixed(2)),
      id: `DH${this.currentId++}`,
      depth: settings.depth,
      spacing: settings.spacing,
      burden: settings.burden
    };
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

  // Method to reset hole numbering (if explicitly needed)
  resetHoleNumbering(): void {
    this.currentId = 1;
  }

  // Methods to manage currentId for continuous numbering
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
} 