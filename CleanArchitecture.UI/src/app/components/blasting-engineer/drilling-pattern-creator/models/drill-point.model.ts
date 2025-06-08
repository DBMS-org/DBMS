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
  fromHoleId: string;
  toHoleId: string;
  connectorType: ConnectorType;
  delay: number; // milliseconds
  sequence: number;
}

export interface DetonatorInfo {
  id: string;
  holeId: string;
  type: DetonatorType;
  delay: number; // milliseconds
  sequence: number;
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