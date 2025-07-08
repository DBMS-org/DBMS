// Site Blasting Data Models
export interface SiteBlastingData {
  id?: number;
  projectId: number;
  siteId: number;
  dataType: string;
  jsonData: string;
  createdAt?: Date;
  updatedAt?: Date;
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
  point1DrillPointId: string;
  point2DrillPointId: string;
  connectorType: ConnectorType;
  delay: number;
  sequence: number;
  projectId: number;
  siteId: number;
  createdAt?: Date;
  updatedAt?: Date;
  // Navigation properties for UI
  point1DrillPoint?: DrillPoint;
  point2DrillPoint?: DrillPoint;
}

export interface CreateBlastConnectionRequest {
  id: string;
  point1DrillPointId: string;
  point2DrillPointId: string;
  connectorType: ConnectorType;
  delay: number;
  sequence: number;
  projectId: number;
  siteId: number;
}

export interface UpdateBlastConnectionRequest {
  id: string;
  point1DrillPointId: string;
  point2DrillPointId: string;
  connectorType: ConnectorType;
  delay: number;
  sequence: number;
  projectId: number;
  siteId: number;
}

export enum ConnectorType {
  DetonatingCord = 0,
  Connectors = 1
}

export interface SimulationSettings {
  animationSpeed: number;
  showDelays: boolean;
  showConnectors: boolean;
  highlightSequence: boolean;
}

export interface WorkflowState {
  currentStep: string;
  completedSteps: string[];
  isPatternApproved: boolean;
  isSequenceApproved: boolean;
  operatorCompleted: boolean;
}

export interface SiteBlastingResponse {
  success: boolean;
  data: SiteBlastingData;
  message?: string;
}

 