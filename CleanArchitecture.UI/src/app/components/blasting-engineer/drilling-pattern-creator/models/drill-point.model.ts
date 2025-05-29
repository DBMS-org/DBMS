export interface DrillPoint {
  x: number;
  y: number;
  id: string;
  depth: number;
  spacing: number;
  burden: number;
}

export interface PatternSettings {
  spacing: number;
  burden: number;
  depth: number;
}

export interface PatternData {
  drillPoints: DrillPoint[];
  settings: PatternSettings;
}

export class DrillingPatternError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'DrillingPatternError';
  }
} 