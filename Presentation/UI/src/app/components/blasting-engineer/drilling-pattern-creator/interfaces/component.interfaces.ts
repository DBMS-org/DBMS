import { EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { 
  DrillPoint, 
  PatternSettings
} from '../models/drill-point.model';
import {
  CanvasState, 
  UIState, 
  PatternState,
  ModeToggleEvent,
  PointActionEvent,
  PatternActionEvent,
  PlacePointEvent,
  MovePointEvent
} from '../models/pattern-state.model';

// Define missing types locally

export interface ValidationResult<T> {
  isValid: boolean;
  data?: T;
  errors: string[];
}

export interface PerformanceMetrics {
  operationCount: number;
  averageTime: number;
  memoryUsage: number;
}

/**
 * Base interface for all pattern creator components with enhanced type safety
 * @template TConfig - Component configuration type
 */
export interface BasePatternComponent<TConfig = Record<string, unknown>> {
  /**
   * Component initialization with optional configuration
   * @param config - Optional component configuration
   */
  ngOnInit(config?: TConfig): void;
  
  /**
   * Component cleanup with resource management
   */
  ngOnDestroy(): void;
  
  /**
   * Get component health status for debugging
   */
  getHealthStatus?(): ComponentHealthStatus;
  
  /**
   * Get component performance metrics
   */
  getPerformanceMetrics?(): PerformanceMetrics;
}

/**
 * Component health status for monitoring and debugging
 */
export interface ComponentHealthStatus {
  /** Whether component is healthy */
  readonly isHealthy: boolean;
  /** Component initialization status */
  readonly isInitialized: boolean;
  /** Any health issues */
  readonly issues: readonly string[];
  /** Last health check timestamp */
  readonly lastChecked: string;
}

/**
 * Configuration for the main drilling pattern creator component
 */
export interface DrillingPatternCreatorConfig {
  /** Project ID */
  readonly projectId: number;
  /** Site ID */
  readonly siteId: number;
  /** Whether component is read-only */
  readonly readOnly?: boolean;
  /** Initial pattern settings */
  readonly initialSettings?: Partial<PatternSettings>;
  /** Performance monitoring enabled */
  readonly enablePerformanceMonitoring?: boolean;
}

/**
 * Interface for the main drilling pattern creator container component
 */
export interface IDrillingPatternCreatorComponent extends BasePatternComponent<DrillingPatternCreatorConfig> {
  // State observables with enhanced typing
  readonly state$: Observable<PatternState>;
  readonly isLoading$: Observable<boolean>;
  readonly error$: Observable<Error | null>;
  readonly isDirty$: Observable<boolean>;
  
  // Event handlers with specific event types
  onToolbarSettingsChange(event: PatternSettings): Promise<void>;
  onToolbarModeToggle(event: ModeToggleEvent): Promise<void>;
  onToolbarPointAction(event: PointActionEvent): Promise<void>;
  onToolbarPatternAction(event: PatternActionEvent): Promise<void>;
  onCanvasPointPlaced(event: PlacePointEvent): Promise<void>;
  onCanvasPointSelected(event: DrillPoint): Promise<void>;
  onCanvasPointMoved(event: MovePointEvent): Promise<void>;
  onCanvasStateChanged(event: Partial<CanvasState>): Promise<void>;
  
  // Component-specific methods
  savePattern(): Promise<ValidationResult<void>>;
  loadPattern(projectId: number, siteId: number): Promise<ValidationResult<PatternState>>;
  exportPattern(format: 'json' | 'csv' | 'blast_designer'): Promise<ValidationResult<string>>;
  resetPattern(): Promise<void>;
}

/**
 * Configuration for the pattern toolbar component
 */
export interface PatternToolbarConfig {
  /** Whether to show advanced settings */
  readonly showAdvancedSettings?: boolean;
  /** Available export formats */
  readonly availableExportFormats?: readonly string[];
  /** Whether to enable keyboard shortcuts */
  readonly enableKeyboardShortcuts?: boolean;
}

/**
 * Interface for the pattern toolbar component with enhanced type safety
 */
export interface IPatternToolbarComponent extends BasePatternComponent<PatternToolbarConfig> {
  // Input properties with strict typing
  readonly settings: PatternSettings;
  readonly selectedPoint: DrillPoint | null;
  readonly drillPointsCount: number;
  readonly activeMode: string;
  readonly isSaved: boolean;
  readonly isReadOnly: boolean;
  readonly validationErrors: readonly string[];
  
  // Output events with specific event types
  readonly settingsChange: EventEmitter<PatternSettings>;
  readonly modeToggle: EventEmitter<ModeToggleEvent>;
  readonly pointAction: EventEmitter<PointActionEvent>;
  readonly patternAction: EventEmitter<PatternActionEvent>;
  
  // Methods with enhanced signatures
  onSettingsChange(settings: Partial<PatternSettings>): Promise<ValidationResult<PatternSettings>>;
  onModeToggle(mode: string, enabled: boolean): Promise<void>;
  onPointAction(action: string, pointId?: string): Promise<void>;
  onPatternAction(action: string, parameters?: Record<string, unknown>): Promise<void>;
  
  // Validation methods
  validateSettings(settings: Partial<PatternSettings>): ValidationResult<PatternSettings>;
  getAvailableActions(): readonly string[];
}

/**
 * Interface for the main canvas component
 */
export interface IPatternCanvasComponent extends BasePatternComponent {
  // Input properties
  settings: PatternSettings;
  drillPoints: DrillPoint[];
  selectedPoint: DrillPoint | null;
  canvasState: CanvasState;
  
  // Output events
  pointPlaced: EventEmitter<PlacePointEvent>;
  pointSelected: EventEmitter<DrillPoint>;
  pointMoved: EventEmitter<MovePointEvent>;
  canvasStateChange: EventEmitter<CanvasState>;
  
  // Methods
  initializeCanvas(): void;
  updateCanvas(): void;
  destroyCanvas(): void;
  onPointPlaced(event: PlacePointEvent): void;
  onPointSelected(point: DrillPoint): void;
  onPointMoved(event: MovePointEvent): void;
  onCanvasStateChange(state: Partial<CanvasState>): void;
}

/**
 * Interface for grid canvas component
 */
export interface IGridCanvasComponent extends BasePatternComponent {
  // Input properties
  layer: any; // Konva.Layer
  settings: PatternSettings;
  canvasState: CanvasState;
  isPreciseMode: boolean;
  
  // Methods
  renderGrid(): void;
  updateGrid(): void;
  clearGrid(): void;
}

/**
 * Interface for ruler canvas component
 */
export interface IRulerCanvasComponent extends BasePatternComponent {
  // Input properties
  layer: any; // Konva.Layer
  settings: PatternSettings;
  canvasState: CanvasState;
  
  // Methods
  renderRuler(): void;
  updateRuler(): void;
  clearRuler(): void;
}

/**
 * Interface for drill point canvas component
 */
export interface IDrillPointCanvasComponent extends BasePatternComponent {
  // Input properties
  layer: any; // Konva.Layer
  drillPoints: DrillPoint[];
  selectedPoint: DrillPoint | null;
  settings: PatternSettings;
  
  // Output events
  pointSelected: EventEmitter<DrillPoint>;
  pointMoved: EventEmitter<MovePointEvent>;
  
  // Methods
  renderPoints(): void;
  updatePoints(): void;
  clearPoints(): void;
  selectPoint(point: DrillPoint | null): void;
  highlightPoint(pointId: string): void;
  onPointSelected(point: DrillPoint): void;
  onPointMoved(event: MovePointEvent): void;
}

/**
 * Interface for status bar component
 */
export interface IPatternStatusBarComponent extends BasePatternComponent {
  // Input properties
  canvasState: CanvasState;
  uiState: UIState;
  drillPointsCount: number;
  
  // Methods
  updateCursorPosition(x: number, y: number): void;
  updateStatus(message: string): void;
}

/**
 * Interface for instructions component
 */
export interface IPatternInstructionsComponent extends BasePatternComponent {
  // Input properties
  showInstructions: boolean;
  
  // Output events
  instructionsToggle: EventEmitter<boolean>;
  
  // Methods
  toggleInstructions(): void;
  onInstructionsToggle(show: boolean): void;
}

/**
 * Interface for components that can be validated
 */
export interface IValidatable {
  isValid(): boolean;
  getValidationErrors(): string[];
}

/**
 * Interface for components that can be reset
 */
export interface IResettable {
  reset(): void;
}

/**
 * Interface for components that support undo/redo
 */
export interface IUndoable {
  canUndo(): boolean;
  canRedo(): boolean;
  undo(): void;
  redo(): void;
}