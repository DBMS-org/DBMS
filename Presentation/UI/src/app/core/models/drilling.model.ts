// Unified Drilling Models - Consolidates DrillPoint, DrillHole, and related interfaces
export interface DrillLocation {
  id: string;
  
  // 2D Coordinates (required)
  x: number;
  y: number;
  
  // 3D Coordinates (optional for legacy CSV data)
  easting?: number;
  northing?: number;
  elevation?: number;
  
  // Drilling Parameters
  depth: number;
  length?: number;
  actualDepth?: number;
  
  // Pattern Parameters
  spacing: number;
  burden: number;
  
  // 3D Orientation (optional)
  azimuth?: number | null;
  dip?: number | null;
  
  // Additional Properties
  name?: string;
  serialNumber?: number;
  stemming?: number;
  
  // Context
  projectId: number;
  siteId: number;
  
  // Metadata
  createdAt: Date;
  updatedAt: Date;
  
  // Computed Properties
  has3DData: boolean;
  requiresFallbackTo2D: boolean;
}

export interface PatternSettings {
  id?: number;
  name: string;
  projectId: number;
  siteId: number;
  spacing: number;
  burden: number;
  depth: number;
  
  // Additional settings
  gridSize?: number;
  orientation?: number;
  
  createdAt?: Date;
  updatedAt?: Date;
}

// Unified Blast Connection Models
export enum ConnectorType {
  DETONATING_CORD = 'detonating_cord',
  CONNECTORS = 'connectors'
}

export enum DetonatorType {
  ELECTRIC = 'electric',
  NON_ELECTRIC = 'non_electric',
  ELECTRONIC = 'electronic'
}

export interface BlastConnection {
  id: string;
  fromHoleId: string;
  toHoleId: string;
  connectorType: ConnectorType;
  delay: number; // milliseconds
  sequence: number;
  
  // Connection points
  startPoint: ConnectionPoint;
  endPoint: ConnectionPoint;
  
  // Context
  projectId: number;
  siteId: number;
  
  createdAt: Date;
  updatedAt: Date;
}

export interface ConnectionPoint {
  id: string;
  label: string;
  x: number;
  y: number;
}

export interface DetonatorInfo {
  id: string;
  holeId: string;
  type: DetonatorType;
  delay: number; // milliseconds
  sequence: number;
  
  // Context
  projectId: number;
  siteId: number;
  
  createdAt: Date;
  updatedAt: Date;
}

export interface BlastSequence {
  id: number;
  projectId: number;
  siteId: number;
  name: string;
  description: string;
  
  // Sequence data
  connections: BlastConnection[];
  detonators: DetonatorInfo[];
  simulationSettings: SimulationSettings;
  
  // Metadata
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
  createdByUserId: number;
}

export interface SimulationSettings {
  timeStep: number;
  duration: number;
  playbackSpeed: number;
  
  visualEffects: {
    showWaveEffects: boolean;
    showParticleEffects: boolean;
    showTimingLabels: boolean;
  };
  
  analysis: {
    calculatePeakPressure: boolean;
    calculateVibration: boolean;
    generateReport: boolean;
  };
}

// DTOs for API communication
export interface CreateDrillLocationRequest {
  x: number;
  y: number;
  depth: number;
  spacing: number;
  burden: number;
  projectId: number;
  siteId: number;
  
  // Optional 3D data
  easting?: number;
  northing?: number;
  elevation?: number;
  azimuth?: number;
  dip?: number;
  name?: string;
}

export interface UpdateDrillLocationRequest {
  id: string;
  x: number;
  y: number;
  depth?: number;
  spacing?: number;
  burden?: number;
  projectId: number;
  siteId: number;
}

export interface SavePatternRequest {
  projectId: number;
  siteId: number;
  drillLocations: DrillLocation[];
  settings: PatternSettings;
}

export interface SavePatternResult {
  success: boolean;
  message: string;
  patternId?: number;
}

// Utility functions for model conversion
export class DrillModelConverter {
  static drillHoleToDrillLocation(drillHole: any): DrillLocation {
    return {
      id: drillHole.id || drillHole.serialNumber?.toString() || '',
      x: drillHole.easting || drillHole.x || 0,
      y: drillHole.northing || drillHole.y || 0,
      easting: drillHole.easting,
      northing: drillHole.northing,
      elevation: drillHole.elevation,
      depth: drillHole.depth || drillHole.actualDepth || 0,
      length: drillHole.length,
      actualDepth: drillHole.actualDepth,
      spacing: drillHole.spacing || 3.0,
      burden: drillHole.burden || 2.5,
      azimuth: drillHole.azimuth,
      dip: drillHole.dip,
      name: drillHole.name,
      serialNumber: drillHole.serialNumber,
      stemming: drillHole.stemming,
      projectId: drillHole.projectId || 0,
      siteId: drillHole.siteId || 0,
      createdAt: drillHole.createdAt || new Date(),
      updatedAt: drillHole.updatedAt || new Date(),
      has3DData: drillHole.azimuth != null && drillHole.dip != null,
      requiresFallbackTo2D: drillHole.azimuth == null || drillHole.dip == null
    };
  }
  
  static drillPointToDrillLocation(drillPoint: any): DrillLocation {
    return {
      id: drillPoint.id || '',
      x: drillPoint.x || 0,
      y: drillPoint.y || 0,
      depth: drillPoint.depth || 0,
      spacing: drillPoint.spacing || 3.0,
      burden: drillPoint.burden || 2.5,
      projectId: drillPoint.projectId || 0,
      siteId: drillPoint.siteId || 0,
      createdAt: drillPoint.createdAt || new Date(),
      updatedAt: drillPoint.updatedAt || new Date(),
      has3DData: false,
      requiresFallbackTo2D: true
    };
  }
  
  static drillLocationToBackendDto(location: DrillLocation): any {
    return {
      id: location.id,
      x: location.x,
      y: location.y,
      depth: location.depth,
      spacing: location.spacing,
      burden: location.burden,
      projectId: location.projectId,
      siteId: location.siteId,
      createdAt: location.createdAt.toISOString(),
      updatedAt: location.updatedAt.toISOString()
    };
  }
} 