/**
 * Core type definitions for the drilling pattern creator component
 */

// Base types
export type DrillPointId = string;
export type PatternId = string;
export type ISOTimestamp = string;

/**
 * Validation result type for type-safe error handling
 */
export interface ValidationResult<T = void> {
  readonly isValid: boolean;
  readonly data?: T;
  readonly errors: readonly string[];
  readonly warnings?: readonly string[];
}

/**
 * Performance metrics interface
 */
export interface PerformanceMetrics {
  readonly operationTimes: Record<string, number>;
  readonly memoryUsage: {
    readonly current: number;
    readonly peak: number;
  };
  readonly renderingStats: {
    readonly fps: number;
    readonly frameTime: number;
  };
  readonly errorCount: number;
}

/**
 * Base service interface with common functionality
 */
export interface BaseService {
  readonly isInitialized: boolean;
  initialize(): Promise<void>;
  destroy(): void;
}

/**
 * Pattern state interface
 */
export interface PatternState {
  readonly drillPoints: readonly import('../models/drill-point.model').DrillPoint[];
  readonly settings: import('../models/drill-point.model').PatternSettings;
  readonly selectedPoint: import('../models/drill-point.model').DrillPoint | null;
  readonly canvasState: CanvasState;
  readonly uiState: UIState;
  readonly isLoading: boolean;
  readonly error: Error | null;
  readonly isDirty: boolean;
}

/**
 * Canvas state interface
 */
export interface CanvasState {
  readonly zoom: number;
  readonly panX: number;
  readonly panY: number;
  readonly width: number;
  readonly height: number;
  readonly gridVisible: boolean;
  readonly rulerVisible: boolean;
  readonly snapToGrid: boolean;
  readonly gridSpacing: number;
}

/**
 * UI state interface
 */
export interface UIState {
  readonly sidebarOpen: boolean;
  readonly toolbarVisible: boolean;
  readonly statusBarVisible: boolean;
  readonly activetool: string | null;
  readonly showValidationErrors: boolean;
  readonly theme: 'light' | 'dark';
}

/**
 * Error context for enhanced error handling
 */
export interface ErrorContext {
  readonly component: string;
  readonly method: string;
  readonly parameters?: Record<string, any>;
  readonly timestamp: ISOTimestamp;
  readonly userAgent?: string;
  readonly stackTrace?: string;
}

/**
 * Event system types
 */
export interface PatternCreatorEvent {
  readonly type: string;
  readonly timestamp: ISOTimestamp;
  readonly source: string;
  readonly data?: any;
}

export interface DrillPointAddedEvent extends PatternCreatorEvent {
  readonly type: 'drill-point-added';
  readonly data: {
    readonly point: import('../models/drill-point.model').DrillPoint;
  };
}

export interface DrillPointUpdatedEvent extends PatternCreatorEvent {
  readonly type: 'drill-point-updated';
  readonly data: {
    readonly pointId: DrillPointId;
    readonly updates: Partial<import('../models/drill-point.model').DrillPoint>;
  };
}

export interface DrillPointDeletedEvent extends PatternCreatorEvent {
  readonly type: 'drill-point-deleted';
  readonly data: {
    readonly pointId: DrillPointId;
  };
}

export interface PatternSettingsUpdatedEvent extends PatternCreatorEvent {
  readonly type: 'pattern-settings-updated';
  readonly data: {
    readonly settings: import('../models/drill-point.model').PatternSettings;
  };
}

export interface CanvasStateUpdatedEvent extends PatternCreatorEvent {
  readonly type: 'canvas-state-updated';
  readonly data: {
    readonly canvasState: CanvasState;
  };
}

/**
 * Event handler types
 */
export type EventHandler<T extends PatternCreatorEvent = PatternCreatorEvent> = (event: T) => void;
export type AsyncEventHandler<T extends PatternCreatorEvent = PatternCreatorEvent> = (event: T) => Promise<void>;

/**
 * Event subscription interface
 */
export interface EventSubscription {
  readonly eventType: string;
  readonly handler: EventHandler;
  unsubscribe(): void;
}

/**
 * Utility types for type safety
 */
export type ReadonlyDeep<T> = {
  readonly [P in keyof T]: T[P] extends object ? ReadonlyDeep<T[P]> : T[P];
};

export type Optional<T, K extends keyof T> = Omit<T, K> & Partial<Pick<T, K>>;

export type RequiredFields<T, K extends keyof T> = T & Required<Pick<T, K>>;

/**
 * API response types
 */
export interface ApiResponse<T = any> {
  readonly success: boolean;
  readonly data?: T;
  readonly error?: string;
  readonly timestamp: ISOTimestamp;
}

export interface PaginatedResponse<T> extends ApiResponse<T[]> {
  readonly pagination: {
    readonly page: number;
    readonly pageSize: number;
    readonly total: number;
    readonly totalPages: number;
  };
}

/**
 * File operation types
 */
export interface FileImportResult {
  readonly success: boolean;
  readonly data?: import('../models/drill-point.model').PatternData;
  readonly errors: readonly string[];
  readonly warnings: readonly string[];
}

export interface FileExportOptions {
  readonly format: 'json' | 'csv' | 'blast_designer';
  readonly includeMetadata: boolean;
  readonly compression?: 'none' | 'gzip';
}

/**
 * Validation helper functions
 */
export function createValidationResult<T>(
  isValid: boolean,
  data?: T,
  errors: string[] = [],
  warnings: string[] = []
): ValidationResult<T> {
  return {
    isValid,
    data,
    errors: Object.freeze(errors),
    warnings: Object.freeze(warnings)
  };
}

export function createSuccessResult<T>(data: T, warnings: string[] = []): ValidationResult<T> {
  return createValidationResult(true, data, [], warnings);
}

export function createErrorResult<T = void>(errors: string[], warnings: string[] = []): ValidationResult<T> {
  return createValidationResult<T>(false, undefined as T, errors, warnings);
}