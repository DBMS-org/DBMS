import { DrillPoint, PatternSettings } from './drill-point.model';

/**
 * Core state interface for the drilling pattern creator
 */
export interface PatternState {
  drillPoints: DrillPoint[];
  settings: PatternSettings;
  selectedPoint: DrillPoint | null;
  canvasState: CanvasState;
  uiState: UIState;
  metadata: PatternMetadata;
}

/**
 * Canvas-specific state interface
 */
export interface CanvasState {
  scale: number;
  panOffsetX: number;
  panOffsetY: number;
  isInitialized: boolean;
  isDragging: boolean;
  isPanning: boolean;
}

/**
 * UI-specific state interface
 */
export interface UIState {
  isHolePlacementMode: boolean;
  isPreciseMode: boolean;
  isFullscreen: boolean;
  showInstructions: boolean;
  isSaved: boolean;
  duplicateMessage: string | null;
  cursorPosition: { x: number; y: number } | null;
}

/**
 * Pattern metadata interface
 */
export interface PatternMetadata {
  projectId: number;
  siteId: number;
  lastModified: Date;
  version: string;
}

/**
 * Event interfaces for component communication
 */
export interface ToolbarAction {
  type: 'TOGGLE_MODE' | 'UPDATE_SETTINGS' | 'POINT_ACTION' | 'PATTERN_ACTION';
  payload: any;
}

export interface CanvasEvent {
  type: 'POINT_PLACED' | 'POINT_SELECTED' | 'POINT_MOVED' | 'STATE_CHANGED';
  payload: any;
}

export interface ModeToggleEvent {
  mode: 'HOLE_PLACEMENT' | 'PRECISE' | 'FULLSCREEN';
  enabled: boolean;
}

export interface PointActionEvent {
  action: 'DELETE' | 'CLEAR_ALL' | 'OPEN_DEPTH_EDITOR' | 'UPDATE_POINT_DEPTH';
  pointId?: string;
  depth?: number;
}

export interface PatternActionEvent {
  action: 'SAVE' | 'EXPORT_TO_BLAST_DESIGNER';
}

export interface PlacePointEvent {
  x: number;
  y: number;
  settings: PatternSettings;
}

export interface MovePointEvent {
  point: DrillPoint;
  newX: number;
  newY: number;
}

/**
 * State action types for reactive state management
 */
export type PatternStateAction =
  | { type: 'UPDATE_SETTINGS'; payload: Partial<PatternSettings> }
  | { type: 'ADD_DRILL_POINT'; payload: DrillPoint }
  | { type: 'UPDATE_DRILL_POINT'; payload: { id: string; updates: Partial<DrillPoint> } }
  | { type: 'DELETE_DRILL_POINT'; payload: string }
  | { type: 'SELECT_POINT'; payload: DrillPoint | null }
  | { type: 'UPDATE_CANVAS_STATE'; payload: Partial<CanvasState> }
  | { type: 'UPDATE_UI_STATE'; payload: Partial<UIState> }
  | { type: 'CLEAR_ALL_POINTS' }
  | { type: 'LOAD_PATTERN'; payload: PatternState }
  | { type: 'RESET_STATE' };

/**
 * Initial state factory
 */
export function createInitialPatternState(projectId: number, siteId: number): PatternState {
  return {
    drillPoints: [],
    settings: {
      spacing: 3,
      burden: 2.5,
      depth: 10
    },
    selectedPoint: null,
    canvasState: {
      scale: 1,
      panOffsetX: 0,
      panOffsetY: 0,
      isInitialized: false,
      isDragging: false,
      isPanning: false
    },
    uiState: {
      isHolePlacementMode: false,
      isPreciseMode: false,
      isFullscreen: false,
      showInstructions: false,
      isSaved: true,
      duplicateMessage: null,
      cursorPosition: null
    },
    metadata: {
      projectId,
      siteId,
      lastModified: new Date(),
      version: '1.0.0'
    }
  };
}