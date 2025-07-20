import { Provider, inject } from '@angular/core';
import { 
  PATTERN_STATE_SERVICE,
  PATTERN_DATA_STORE,
  CANVAS_MANAGER_SERVICE,
  PATTERN_VALIDATION_SERVICE,
  ERROR_HANDLING_SERVICE,
  PATTERN_EVENT_BUS_SERVICE,
  PERFORMANCE_MONITORING_SERVICE,
  GRID_RENDERING_SERVICE,
  RULER_RENDERING_SERVICE,
  DRILL_POINT_RENDERING_SERVICE,
  PATTERN_CREATOR_SERVICES,
  PATTERN_CREATOR_CONFIG,
  CANVAS_CONFIG,
  VALIDATION_CONFIG,
  PatternCreatorConfig,
  CanvasConfig,
  ValidationConfig
} from '../tokens/injection.tokens';

import { GridRenderingService } from '../services/grid-rendering.service';

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

// Import existing services (these will be implemented in later tasks)
import { PatternStateService } from '../services/pattern-state.service';
import { PatternDataStore } from '../services/pattern-data.store';

/**
 * Provides all services needed for the pattern creator components
 */
export function providePatternCreatorServices(
  config?: Partial<PatternCreatorConfig>
): Provider[] {
  return [
    // Configuration providers
    {
      provide: PATTERN_CREATOR_CONFIG,
      useValue: {
        enablePerformanceMonitoring: true,
        enableEventLogging: false,
        cacheEnabled: true,
        maxCacheSize: 100,
        autoSaveInterval: 30000,
        validationEnabled: true,
        debugMode: false,
        ...config
      } as PatternCreatorConfig
    },
    
    {
      provide: CANVAS_CONFIG,
      useValue: {
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
      } as CanvasConfig
    },
    
    {
      provide: VALIDATION_CONFIG,
      useValue: {
        minSpacing: 0.5,
        maxDepth: 50,
        duplicateTolerance: 0.1,
        maxPoints: 10000,
        enableRealTimeValidation: true,
        showValidationErrors: true
      } as ValidationConfig
    },

    // Core service providers
    {
      provide: PATTERN_STATE_SERVICE,
      useClass: PatternStateService
    },
    
    {
      provide: PATTERN_DATA_STORE,
      useClass: PatternDataStore
    },

    // Canvas manager service (to be implemented)
    {
      provide: CANVAS_MANAGER_SERVICE,
      useFactory: () => {
        // This will be implemented in a later task
        throw new Error('CanvasManagerService not yet implemented');
      }
    },

    // Validation service (to be implemented)
    {
      provide: PATTERN_VALIDATION_SERVICE,
      useFactory: () => {
        // This will be implemented in a later task
        throw new Error('PatternValidationService not yet implemented');
      }
    },

    // Error handling service (to be implemented)
    {
      provide: ERROR_HANDLING_SERVICE,
      useFactory: () => {
        // This will be implemented in a later task
        throw new Error('ErrorHandlingService not yet implemented');
      }
    },

    // Event bus service (to be implemented)
    {
      provide: PATTERN_EVENT_BUS_SERVICE,
      useFactory: () => {
        // This will be implemented in a later task
        throw new Error('PatternEventBusService not yet implemented');
      }
    },

    // Performance monitoring service (to be implemented)
    {
      provide: PERFORMANCE_MONITORING_SERVICE,
      useFactory: () => {
        // This will be implemented in a later task
        throw new Error('PerformanceMonitoringService not yet implemented');
      }
    },

    // Rendering services
    {
      provide: GRID_RENDERING_SERVICE,
      useClass: GridRenderingService
    },

    {
      provide: RULER_RENDERING_SERVICE,
      useFactory: () => {
        // This will be implemented in a later task
        throw new Error('RulerRenderingService not yet implemented');
      }
    },

    {
      provide: DRILL_POINT_RENDERING_SERVICE,
      useFactory: () => {
        // This will be implemented in a later task
        throw new Error('DrillPointRenderingService not yet implemented');
      }
    },

    // Aggregate services provider
    {
      provide: PATTERN_CREATOR_SERVICES,
      useFactory: (
        stateService: IPatternStateService,
        dataStore: IPatternDataStore,
        canvasManager: ICanvasManagerService,
        validationService: IPatternValidationService,
        errorHandler: IErrorHandlingService,
        eventBus: IPatternEventBusService,
        performanceMonitor: IPerformanceMonitoringService,
        gridRenderer: IGridRenderingService,
        rulerRenderer: IRulerRenderingService,
        pointRenderer: IDrillPointRenderingService
      ): IPatternCreatorServices => ({
        stateService,
        dataStore,
        canvasManager,
        validationService,
        errorHandler,
        eventBus,
        performanceMonitor,
        gridRenderer,
        rulerRenderer,
        pointRenderer
      }),
      deps: [
        PATTERN_STATE_SERVICE,
        PATTERN_DATA_STORE,
        CANVAS_MANAGER_SERVICE,
        PATTERN_VALIDATION_SERVICE,
        ERROR_HANDLING_SERVICE,
        PATTERN_EVENT_BUS_SERVICE,
        PERFORMANCE_MONITORING_SERVICE,
        GRID_RENDERING_SERVICE,
        RULER_RENDERING_SERVICE,
        DRILL_POINT_RENDERING_SERVICE
      ]
    }
  ];
}

/**
 * Provides minimal services for testing
 */
export function providePatternCreatorTestingServices(): Provider[] {
  return [
    // Mock implementations for testing
    {
      provide: PATTERN_STATE_SERVICE,
      useValue: createMockPatternStateService()
    },
    
    {
      provide: PATTERN_DATA_STORE,
      useValue: createMockPatternDataStore()
    },
    
    {
      provide: ERROR_HANDLING_SERVICE,
      useValue: createMockErrorHandlingService()
    },
    
    {
      provide: PATTERN_EVENT_BUS_SERVICE,
      useValue: createMockEventBusService()
    }
  ];
}

/**
 * Mock service factories for testing
 */
function createMockPatternStateService(): Partial<IPatternStateService> {
  return {
    // Mock implementation - to be expanded in tests
  };
}

function createMockPatternDataStore(): Partial<IPatternDataStore> {
  return {
    // Mock implementation - to be expanded in tests
  };
}

function createMockErrorHandlingService(): Partial<IErrorHandlingService> {
  return {
    handleError: () => {},
    handleComponentError: () => {},
    handleServiceError: () => {},
    showUserError: () => {},
    showValidationError: () => {},
    clearErrors: () => {},
    logError: () => {},
    getErrorHistory: () => [],
    attemptRecovery: () => false
  };
}

function createMockEventBusService(): Partial<IPatternEventBusService> {
  return {
    emit: () => {},
    subscribe: () => () => {},
    filter: () => new (require('rxjs')).Observable(),
    getEventHistory: () => [],
    clearHistory: () => {},
    enableLogging: () => {},
    getSubscriptionCount: () => 0
  };
}