import { Injectable } from '@angular/core';
import { DrillPoint, PatternData, PatternSettings, BlastSequenceData } from '../models/drill-point.model';

@Injectable({
  providedIn: 'root'
})
export class DrillPointService {
  private currentId = 1;

  constructor() {}

  validateDrillPointCount(currentCount: number, maxCount: number): boolean {
    if (currentCount >= maxCount) {
      console.warn('Maximum number of drill points reached');
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
    // Don't reset currentId to preserve hole numbering sequence
    // this.currentId = 1; // Commented out to maintain sequential numbering
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

  exportPattern(drillPoints: DrillPoint[], settings: PatternSettings): void {
    const pattern = {
      drillPoints,
      settings: { ...settings }
    };
    
    const blob = new Blob([JSON.stringify(pattern, null, 2)], { type: 'application/json' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'drilling-pattern.json';
    a.click();
    window.URL.revokeObjectURL(url);
  }

  // New method for exporting to blast sequence designer
  exportPatternForBlastDesign(drillPoints: DrillPoint[], settings: PatternSettings): PatternData {
    return {
      drillPoints: [...drillPoints], // Create a copy
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
} 