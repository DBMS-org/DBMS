// Site Blasting Data Models
export interface SiteBlastingData {
  id: number;
  projectId: number;
  siteId: number;
  dataType: string;
  jsonData: any;
  createdAt: Date;
  updatedAt: Date;
  createdByUserId: number;
}

export interface CreateSiteBlastingDataRequest {
  projectId: number;
  siteId: number;
  dataType: string;
  jsonData: any;
}

export interface UpdateSiteBlastingDataRequest {
  jsonData: any;
}

// Drill Pattern Models
export interface DrillPattern {
  id: number;
  projectId: number;
  siteId: number;
  name: string;
  description: string;
  spacing: number;
  burden: number;
  depth: number;
  drillPointsJson: DrillPoint[] | string; // Can be array (frontend) or JSON string (backend)
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
  createdByUserId: number;
}

export interface CreateDrillPatternRequest {
  projectId: number;
  siteId: number;
  name: string;
  description: string;
  spacing: number;
  burden: number;
  depth: number;
  drillPointsJson: string; // Backend expects JSON string
}

export interface UpdateDrillPatternRequest {
  name: string;
  description: string;
  spacing: number;
  burden: number;
  depth: number;
  drillPointsJson: string; // Backend expects JSON string
  isActive: boolean;
}

// Blast Sequence Models
export interface BlastSequence {
  id: number;
  projectId: number;
  siteId: number;
  drillPatternId: number;
  name: string;
  description: string;
  connectionsJson: BlastConnection[] | string; // Can be array (frontend) or JSON string (backend)
  simulationSettingsJson: SimulationSettings | string; // Can be object (frontend) or JSON string (backend)
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
  createdByUserId: number;
}

export interface CreateBlastSequenceRequest {
  projectId: number;
  siteId: number;
  drillPatternId: number;
  name: string;
  description: string;
  connectionsJson: string; // Backend expects JSON string
  simulationSettingsJson: string; // Backend expects JSON string
}

export interface UpdateBlastSequenceRequest {
  drillPatternId: number;
  name: string;
  description: string;
  connectionsJson: string; // Backend expects JSON string
  simulationSettingsJson: string; // Backend expects JSON string
  isActive: boolean;
}

// Related Models (from existing drill-point.model.ts)
export interface DrillPoint {
  x: number;
  y: number;
  id: string;
  depth: number;
  spacing: number;
  burden: number;
}

export interface BlastConnection {
  id: string;
  fromHoleId: string;
  toHoleId: string;
  connectorType: ConnectorType;
  delay: number;
  sequence: number;
  startPoint: {
    id: string;
    label: string;
    x: number;
    y: number;
    isHidden: boolean;
  };
  endPoint: {
    id: string;
    label: string;
    x: number;
    y: number;
    isHidden: boolean;
  };
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

export enum ConnectorType {
  DETONATING_CORD = 'Non-Electric-detonation-wire',
  CONNECTORS = 'Non-Electric-connectors-wire'
}

export enum DetonatorType {
  ELECTRIC = 'electric',
  NON_ELECTRIC = 'non_electric',
  ELECTRONIC = 'electronic'
}

// Response DTOs
export interface DrillPatternResponse {
  success: boolean;
  data: DrillPattern;
  message: string;
}

export interface BlastSequenceResponse {
  success: boolean;
  data: BlastSequence;
  message: string;
}

export interface SiteBlastingDataResponse {
  success: boolean;
  data: SiteBlastingData;
  message: string;
} 