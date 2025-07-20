import { Observable } from 'rxjs';
import {
  DrillPoint,
  PatternSettings
} from '../models/drill-point.model';
import {
  PatternState,
  CanvasState,
  UIState,
  PatternStateAction
} from '../models/pattern-state.model';

// Define missing types locally
export type DrillPointId = string;
export type PatternId = string;
export type ISOTimestamp = string;

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

export interface PatternCreatorEvent {
  type: string;
  timestamp: string;
  source?: string;
  [key: string]: any; // Allow additional properties
}

export interface BaseService {
  initialize(): void;
  destroy(): void;
}

// PatternStateAction is imported from pattern-state.model.ts

/**
 * Pattern data interface for persistence
 */
export interface PatternData {
  readonly drillPoints: readonly DrillPoint[];
  readonly settings: PatternSettings;
  readonly metadata: {
    readonly projectId: number;
    readonly siteId: number;
    readonly patternId: PatternId;
    readonly version: string;
    readonly createdAt: ISOTimestamp;
    readonly lastModified: ISOTimestamp;
  };
}

/**
 * Interface for the main pattern state service with enhanced type safety
 */
export interface IPatternStateService {
  // State observables with enhanced typing
  readonly state$: Observable<PatternState>;
  readonly drillPoints$: Observable<readonly DrillPoint[]>;
  readonly settings$: Observable<PatternSettings>;
  readonly selectedPoint$: Observable<DrillPoint | null>;
  readonly canvasState$: Observable<CanvasState>;
  readonly uiState$: Observable<UIState>;
  readonly isLoading$: Observable<boolean>;
  readonly error$: Observable<Error | null>;
  readonly isDirty$: Observable<boolean>;
  readonly validationResult$: Observable<ValidationResult<PatternState>>;

  // State queries
  getCurrentState(): PatternState;
  getDrillPoints(): readonly DrillPoint[];
  getSelectedPoint(): DrillPoint | null;
  getSettings(): PatternSettings;
  getCanvasState(): CanvasState;
  getUIState(): UIState;
  isStateDirty(): boolean;

  // State actions with validation
  dispatch(action: PatternStateAction): Promise<ValidationResult<void>>;
  updateSettings(settings: Partial<PatternSettings>): Promise<ValidationResult<PatternSettings>>;
  addDrillPoint(point: DrillPoint): Promise<ValidationResult<DrillPoint>>;
  updateDrillPoint(id: DrillPointId, updates: Partial<DrillPoint>): Promise<ValidationResult<DrillPoint>>;
  deleteDrillPoint(id: DrillPointId): Promise<ValidationResult<void>>;
  selectPoint(point: DrillPoint | null): Promise<void>;
  updateCanvasState(state: Partial<CanvasState>): Promise<void>;
  updateUIState(state: Partial<UIState>): Promise<void>;
  clearAllPoints(): Promise<ValidationResult<void>>;
  resetState(): Promise<void>;

  // Persistence with enhanced error handling
  savePattern(): Promise<ValidationResult<void>>;
  loadPattern(projectId: number, siteId: number): Promise<ValidationResult<PatternData>>;
  exportPattern(format: 'json' | 'csv' | 'blast_designer'): Promise<ValidationResult<string>>;
  importPattern(data: string, format: 'json' | 'csv'): Promise<ValidationResult<PatternData>>;

  // Validation
  validateState(): ValidationResult<PatternState>;
  validateDrillPoint(point: DrillPoint): ValidationResult<DrillPoint>;
  validateSettings(settings: PatternSettings): ValidationResult<PatternSettings>;

  // History management
  canUndo(): boolean;
  canRedo(): boolean;
  undo(): Promise<ValidationResult<void>>;
  redo(): Promise<ValidationResult<void>>;
  getHistory(): readonly StateHistoryEntry[];
  clearHistory(): void;

  // Performance monitoring
  getPerformanceMetrics(): PerformanceMetrics;
  enablePerformanceMonitoring(enabled: boolean): void;
}

/**
 * State history entry for undo/redo functionality
 */
export interface StateHistoryEntry {
  readonly id: string;
  readonly state: PatternState;
  readonly action: PatternStateAction;
  readonly timestamp: ISOTimestamp;
  readonly description?: string;
}

/**
 * Interface for pattern data store
 */
export interface IPatternDataStore {
  // Data operations
  getPatternData(projectId: number, siteId: number): Observable<PatternData>;
  savePatternData(data: PatternData): Observable<void>;
  deletePatternData(projectId: number, siteId: number): Observable<void>;
  
  // Validation
  validatePatternData(data: PatternData): Observable<boolean>;
  
  // Cache management
  clearCache(): void;
  getCacheStatus(): { size: number; lastUpdated: Date | null };
}

/**
 * Interface for canvas management service
 */
export interface ICanvasManagerService {
  // Canvas lifecycle
  initializeCanvas(containerId: string): Observable<any>; // Returns Konva.Stage
  destroyCanvas(): void;
  resizeCanvas(width: number, height: number): void;
  
  // Layer management
  createLayer(name: string): any; // Returns Konva.Layer
  getLayer(name: string): any | null;
  removeLayer(name: string): void;
  clearLayer(name: string): void;
  
  // Canvas state
  getCanvasState(): CanvasState;
  updateCanvasState(state: Partial<CanvasState>): void;
  
  // Transformations
  zoom(scale: number, centerX?: number, centerY?: number): void;
  pan(deltaX: number, deltaY: number): void;
  resetView(): void;
  
  // Coordinate conversion
  screenToCanvas(screenX: number, screenY: number): { x: number; y: number };
  canvasToScreen(canvasX: number, canvasY: number): { x: number; y: number };
}

/**
 * Interface for pattern validation service
 */
export interface IPatternValidationService {
  // Point validation
  validateDrillPoint(point: DrillPoint): { isValid: boolean; errors: string[] };
  validatePointPosition(x: number, y: number, existingPoints: DrillPoint[]): { isValid: boolean; errors: string[] };
  
  // Settings validation
  validatePatternSettings(settings: PatternSettings): { isValid: boolean; errors: string[] };
  
  // Pattern validation
  validatePattern(data: PatternData): { isValid: boolean; errors: string[] };
  
  // Business rules
  checkMinimumSpacing(points: DrillPoint[], minSpacing: number): boolean;
  checkMaximumDepth(points: DrillPoint[], maxDepth: number): boolean;
  detectDuplicatePoints(points: DrillPoint[], tolerance: number): DrillPoint[];
}

/**
 * Interface for error handling service
 */
export interface IErrorHandlingService {
  // Error handling
  handleError(error: Error, context: string): void;
  handleComponentError(component: string, error: Error, context?: any): void;
  handleServiceError(service: string, method: string, error: Error, parameters?: any): void;

  // User notifications
  showUserError(message: string, type: 'warning' | 'error' | 'info'): void;
  showValidationError(field: string, message: string): void;
  clearErrors(): void;

  // Error logging
  logError(error: Error, context: string, metadata?: any): void;
  getErrorHistory(): Array<{ error: Error; context: string; timestamp: Date }>;

  // Recovery
  attemptRecovery(error: Error, context: string): boolean;
}

/**
 * Interface for event bus service
 */
export interface IPatternEventBusService {
  // Event emission
  emit(event: PatternCreatorEvent): void;
  
  // Event subscription
  subscribe(
    eventType: string,
    handler: (event: PatternCreatorEvent) => void
  ): () => void;
  
  // Event filtering
  filter(
    predicate: (event: PatternCreatorEvent) => boolean
  ): Observable<PatternCreatorEvent>;
  
  // Event history
  getEventHistory(): PatternCreatorEvent[];
  clearHistory(): void;
  
  // Debugging
  enableLogging(enabled: boolean): void;
  getSubscriptionCount(): number;
}

/**
 * Interface for performance monitoring service
 */
export interface IPerformanceMonitoringService {
  // Performance tracking
  startTimer(operation: string): string; // Returns timer ID
  endTimer(timerId: string): number; // Returns duration in ms
  recordMetric(name: string, value: number, metadata?: any): void;
  
  // Memory monitoring
  getMemoryUsage(): { used: number; total: number };
  trackMemoryLeak(component: string): void;
  
  // Performance reporting
  getPerformanceReport(): {
    operations: Array<{ name: string; averageTime: number; count: number }>;
    memory: { current: number; peak: number };
    errors: number;
  };
  
  // Optimization suggestions
  getOptimizationSuggestions(): string[];
}

/**
 * Interface for canvas rendering services
 */
export interface ICanvasRenderingService {
  // Rendering lifecycle
  initialize(layer: any): void; // Konva.Layer
  render(): void;
  update(): void;
  clear(): void;
  destroy(): void;
  
  // Performance optimization
  enableCaching(enabled: boolean): void;
  invalidateCache(): void;
  
  // Visibility
  setVisible(visible: boolean): void;
  isVisible(): boolean;
}

/**
 * Interface for grid rendering service
 */
export interface IGridRenderingService extends ICanvasRenderingService {
  // Grid-specific methods
  setGridSpacing(spacing: number): void;
  setGridColor(color: string): void;
  showPreciseMode(enabled: boolean): void;
  highlightIntersections(points: Array<{ x: number; y: number }>): void;
}

/**
 * Interface for ruler rendering service
 */
export interface IRulerRenderingService extends ICanvasRenderingService {
  // Ruler-specific methods
  setUnit(unit: 'meters' | 'feet'): void;
  setScale(scale: number): void;
  showHorizontalRuler(enabled: boolean): void;
  showVerticalRuler(enabled: boolean): void;
}

/**
 * Interface for drill point rendering service
 */
export interface IDrillPointRenderingService extends ICanvasRenderingService {
  // Point-specific methods
  renderPoints(points: DrillPoint[]): void;
  highlightPoint(pointId: string): void;
  selectPoint(pointId: string | null): void;
  updatePointPosition(pointId: string, x: number, y: number): void;
  setPointStyle(style: { color: string; size: number; strokeWidth: number }): void;
}

/**
 * Interface for dependency injection token
 */
export interface IPatternCreatorServices {
  stateService: IPatternStateService;
  dataStore: IPatternDataStore;
  canvasManager: ICanvasManagerService;
  validationService: IPatternValidationService;
  errorHandler: IErrorHandlingService;
  eventBus: IPatternEventBusService;
  performanceMonitor: IPerformanceMonitoringService;
  gridRenderer: IGridRenderingService;
  rulerRenderer: IRulerRenderingService;
  pointRenderer: IDrillPointRenderingService;
}