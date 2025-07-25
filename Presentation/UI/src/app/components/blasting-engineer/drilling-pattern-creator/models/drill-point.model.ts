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

// New interfaces for blast sequence design
export interface BlastConnection {
  id: string;
  point1DrillPointId: string;
  point2DrillPointId: string;
  connectorType: ConnectorType;
  delay: number; // milliseconds
  sequence: number;
  projectId: number;
  siteId: number;
  // UI helper properties for visualization
  startPoint?: {
    id: string;
    label: string; // "1" for starting point
    x: number;
    y: number;
  };
  endPoint?: {
    id: string;
    label: string; // "2" for ending point
    x: number;
    y: number;
  };
  // Navigation properties for UI
  point1DrillPoint?: DrillPoint;
  point2DrillPoint?: DrillPoint;
}

export interface DetonatorInfo {
  id: string;
  drillPointId: string;
  type: DetonatorType;
  delay: number; // milliseconds
  sequence: number;
  projectId: number;
  siteId: number;
}

export interface BlastSequenceData {
  patternData: PatternData;
  connections: BlastConnection[];
  detonators: DetonatorInfo[];
  metadata: {
    exportedAt: string;
    version: string;
    totalSequenceTime: number;
  };
}

export enum ConnectorType {
  DETONATING_CORD = 'Non-Electric-detonation-wire',
  CONNECTORS = 'Non-Electric-connectors-wire'
}

export enum DetonatorType {
  ELECTRIC = 'electric',
  NON_ELECTRIC = 'non_electric',
  ELECTRONIC = 'electronic'
}

export class DrillingPatternError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'DrillingPatternError';
  }
}

// Type-safe interfaces for Konva event handling
export interface KonvaTargetWithPointId {
  pointId?: string;
  attrs: { pointId?: string; [key: string]: any };
  parent?: KonvaTargetWithPointId;
  getClassName?: () => string;
}

export interface DrillHoleData {
  id?: number;
  easting?: number;
  northing?: number;
  depth?: number;
  length?: number;
  [key: string]: any;
}

export interface CanvasPosition {
  x: number;
  y: number;
}

export interface ViewportSettings {
  scale: number;
  panOffsetX: number;
  panOffsetY: number;
} 