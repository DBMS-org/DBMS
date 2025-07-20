import { InjectionToken } from '@angular/core';
import { 
  IPatternStateService,
  IPatternDataStore,
  ICanvasManagerService,
  IPatternValidationService,
  IErrorHandlingService,
  IPatternEventBusService,
  IPerformanceMonitoringService,
  IGridRenderingService,
  IRulerRenderingService,
  IDrillPointRenderingService,
  IPatternCreatorServices
} from '../interfaces/service.interfaces';

/**
 * Injection tokens for pattern creator services
 */
export const PATTERN_STATE_SERVICE = new InjectionToken<IPatternStateService>(
  'PatternStateService',
  {
    providedIn: 'root',
    factory: () => {
      throw new Error('PATTERN_STATE_SERVICE must be provided');
    }
  }
);

export const PATTERN_DATA_STORE = new InjectionToken<IPatternDataStore>(
  'PatternDataStore',
  {
    providedIn: 'root',
    factory: () => {
      throw new Error('PATTERN_DATA_STORE must be provided');
    }
  }
);

export const CANVAS_MANAGER_SERVICE = new InjectionToken<ICanvasManagerService>(
  'CanvasManagerService',
  {
    providedIn: 'root',
    factory: () => {
      throw new Error('CANVAS_MANAGER_SERVICE must be provided');
    }
  }
);

export const PATTERN_VALIDATION_SERVICE = new InjectionToken<IPatternValidationService>(
  'PatternValidationService',
  {
    providedIn: 'root',
    factory: () => {
      throw new Error('PATTERN_VALIDATION_SERVICE must be provided');
    }
  }
);

export const ERROR_HANDLING_SERVICE = new InjectionToken<IErrorHandlingService>(
  'ErrorHandlingService',
  {
    providedIn: 'root',
    factory: () => {
      throw new Error('ERROR_HANDLING_SERVICE must be provided');
    }
  }
);

export const PATTERN_EVENT_BUS_SERVICE = new InjectionToken<IPatternEventBusService>(
  'PatternEventBusService',
  {
    providedIn: 'root',
    factory: () => {
      throw new Error('PATTERN_EVENT_BUS_SERVICE must be provided');
    }
  }
);

export const PERFORMANCE_MONITORING_SERVICE = new InjectionToken<IPerformanceMonitoringService>(
  'PerformanceMonitoringService',
  {
    providedIn: 'root',
    factory: () => {
      throw new Error('PERFORMANCE_MONITORING_SERVICE must be provided');
    }
  }
);

export const GRID_RENDERING_SERVICE = new InjectionToken<IGridRenderingService>(
  'GridRenderingService',
  {
    providedIn: 'root',
    factory: () => {
      throw new Error('GRID_RENDERING_SERVICE must be provided');
    }
  }
);

export const RULER_RENDERING_SERVICE = new InjectionToken<IRulerRenderingService>(
  'RulerRenderingService',
  {
    providedIn: 'root',
    factory: () => {
      throw new Error('RULER_RENDERING_SERVICE must be provided');
    }
  }
);

export const DRILL_POINT_RENDERING_SERVICE = new InjectionToken<IDrillPointRenderingService>(
  'DrillPointRenderingService',
  {
    providedIn: 'root',
    factory: () => {
      throw new Error('DRILL_POINT_RENDERING_SERVICE must be provided');
    }
  }
);

export const PATTERN_CREATOR_SERVICES = new InjectionToken<IPatternCreatorServices>(
  'PatternCreatorServices',
  {
    providedIn: 'root',
    factory: () => {
      throw new Error('PATTERN_CREATOR_SERVICES must be provided');
    }
  }
);

/**
 * Configuration tokens
 */
export const PATTERN_CREATOR_CONFIG = new InjectionToken<PatternCreatorConfig>(
  'PatternCreatorConfig',
  {
    providedIn: 'root',
    factory: () => ({
      enablePerformanceMonitoring: true,
      enableEventLogging: false,
      cacheEnabled: true,
      maxCacheSize: 100,
      autoSaveInterval: 30000, // 30 seconds
      validationEnabled: true,
      debugMode: false
    })
  }
);

/**
 * Configuration interface
 */
export interface PatternCreatorConfig {
  enablePerformanceMonitoring: boolean;
  enableEventLogging: boolean;
  cacheEnabled: boolean;
  maxCacheSize: number;
  autoSaveInterval: number;
  validationEnabled: boolean;
  debugMode: boolean;
}

/**
 * Canvas configuration token
 */
export const CANVAS_CONFIG = new InjectionToken<CanvasConfig>(
  'CanvasConfig',
  {
    providedIn: 'root',
    factory: () => ({
      defaultWidth: 800,
      defaultHeight: 600,
      minZoom: 0.1,
      maxZoom: 10,
      zoomStep: 0.1,
      panSensitivity: 1,
      gridSpacing: 1,
      gridColor: '#e0e0e0',
      backgroundColor: '#ffffff',
      enableAntiAliasing: true,
      pixelRatio: window.devicePixelRatio || 1
    })
  }
);

/**
 * Canvas configuration interface
 */
export interface CanvasConfig {
  defaultWidth: number;
  defaultHeight: number;
  minZoom: number;
  maxZoom: number;
  zoomStep: number;
  panSensitivity: number;
  gridSpacing: number;
  gridColor: string;
  backgroundColor: string;
  enableAntiAliasing: boolean;
  pixelRatio: number;
}

/**
 * Validation configuration token
 */
export const VALIDATION_CONFIG = new InjectionToken<ValidationConfig>(
  'ValidationConfig',
  {
    providedIn: 'root',
    factory: () => ({
      minSpacing: 0.5,
      maxDepth: 50,
      duplicateTolerance: 0.1,
      maxPoints: 10000,
      enableRealTimeValidation: true,
      showValidationErrors: true
    })
  }
);

/**
 * Validation configuration interface
 */
export interface ValidationConfig {
  minSpacing: number;
  maxDepth: number;
  duplicateTolerance: number;
  maxPoints: number;
  enableRealTimeValidation: boolean;
  showValidationErrors: boolean;
}