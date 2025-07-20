import { TestBed } from '@angular/core/testing';
import { Observable, of } from 'rxjs';

import { 
  IPatternStateService,
  IPatternDataStore,
  ICanvasManagerService,
  IPatternValidationService,
  IErrorHandlingService
} from './service.interfaces';

import { 
  PatternState, 
  PatternSettings, 
  DrillPoint, 
  CanvasState,
  UIState,
  PatternData,
  PatternStateAction
} from '../models';

describe('Service Interfaces', () => {
  
  describe('IPatternStateService', () => {
    it('should define required observables and methods', () => {
      const mockState: PatternState = {
        drillPoints: [],
        settings: { spacing: 3, burden: 2.5, depth: 10 },
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
          projectId: 1,
          siteId: 1,
          lastModified: new Date(),
          version: '1.0.0'
        }
      };

      const service: IPatternStateService = {
        state$: of(mockState),
        drillPoints$: of([]),
        settings$: of({ spacing: 3, burden: 2.5, depth: 10 }),
        selectedPoint$: of(null),
        canvasState$: of(mockState.canvasState),
        uiState$: of(mockState.uiState),
        isLoading$: of(false),
        error$: of(null),
        dispatch: jasmine.createSpy('dispatch'),
        updateSettings: jasmine.createSpy('updateSettings'),
        addDrillPoint: jasmine.createSpy('addDrillPoint'),
        updateDrillPoint: jasmine.createSpy('updateDrillPoint'),
        deleteDrillPoint: jasmine.createSpy('deleteDrillPoint'),
        selectPoint: jasmine.createSpy('selectPoint'),
        updateCanvasState: jasmine.createSpy('updateCanvasState'),
        updateUIState: jasmine.createSpy('updateUIState'),
        clearAllPoints: jasmine.createSpy('clearAllPoints'),
        resetState: jasmine.createSpy('resetState'),
        savePattern: jasmine.createSpy('savePattern').and.returnValue(of(undefined)),
        loadPattern: jasmine.createSpy('loadPattern').and.returnValue(of({} as PatternData)),
        getCurrentState: jasmine.createSpy('getCurrentState').and.returnValue(mockState),
        isStateDirty: jasmine.createSpy('isStateDirty').and.returnValue(false)
      };

      expect(service.state$).toBeDefined();
      expect(service.drillPoints$).toBeDefined();
      expect(service.settings$).toBeDefined();
      expect(service.updateSettings).toBeDefined();
      expect(service.addDrillPoint).toBeDefined();
      expect(service.savePattern).toBeDefined();
    });
  });

  describe('IPatternDataStore', () => {
    it('should define required data operations', () => {
      const mockPatternData: PatternData = {
        drillPoints: [],
        settings: { spacing: 3, burden: 2.5, depth: 10 }
      };

      const store: IPatternDataStore = {
        getPatternData: jasmine.createSpy('getPatternData').and.returnValue(of(mockPatternData)),
        savePatternData: jasmine.createSpy('savePatternData').and.returnValue(of(undefined)),
        deletePatternData: jasmine.createSpy('deletePatternData').and.returnValue(of(undefined)),
        validatePatternData: jasmine.createSpy('validatePatternData').and.returnValue(of(true)),
        clearCache: jasmine.createSpy('clearCache'),
        getCacheStatus: jasmine.createSpy('getCacheStatus').and.returnValue({ 
          size: 0, 
          lastUpdated: null 
        })
      };

      expect(store.getPatternData).toBeDefined();
      expect(store.savePatternData).toBeDefined();
      expect(store.validatePatternData).toBeDefined();
      expect(store.clearCache).toBeDefined();
    });
  });

  describe('ICanvasManagerService', () => {
    it('should define required canvas operations', () => {
      const mockCanvasState: CanvasState = {
        scale: 1,
        panOffsetX: 0,
        panOffsetY: 0,
        isInitialized: false,
        isDragging: false,
        isPanning: false
      };

      const service: ICanvasManagerService = {
        initializeCanvas: jasmine.createSpy('initializeCanvas').and.returnValue(of({})),
        destroyCanvas: jasmine.createSpy('destroyCanvas'),
        resizeCanvas: jasmine.createSpy('resizeCanvas'),
        createLayer: jasmine.createSpy('createLayer').and.returnValue({}),
        getLayer: jasmine.createSpy('getLayer').and.returnValue({}),
        removeLayer: jasmine.createSpy('removeLayer'),
        clearLayer: jasmine.createSpy('clearLayer'),
        getCanvasState: jasmine.createSpy('getCanvasState').and.returnValue(mockCanvasState),
        updateCanvasState: jasmine.createSpy('updateCanvasState'),
        zoom: jasmine.createSpy('zoom'),
        pan: jasmine.createSpy('pan'),
        resetView: jasmine.createSpy('resetView'),
        screenToCanvas: jasmine.createSpy('screenToCanvas').and.returnValue({ x: 0, y: 0 }),
        canvasToScreen: jasmine.createSpy('canvasToScreen').and.returnValue({ x: 0, y: 0 })
      };

      expect(service.initializeCanvas).toBeDefined();
      expect(service.createLayer).toBeDefined();
      expect(service.zoom).toBeDefined();
      expect(service.screenToCanvas).toBeDefined();
    });
  });

  describe('IPatternValidationService', () => {
    it('should define required validation methods', () => {
      const mockPoint: DrillPoint = { x: 0, y: 0, id: '1', depth: 10, spacing: 3, burden: 2.5 };
      const mockSettings: PatternSettings = { spacing: 3, burden: 2.5, depth: 10 };
      const mockPatternData: PatternData = { drillPoints: [mockPoint], settings: mockSettings };

      const service: IPatternValidationService = {
        validateDrillPoint: jasmine.createSpy('validateDrillPoint').and.returnValue({ 
          isValid: true, 
          errors: [] 
        }),
        validatePointPosition: jasmine.createSpy('validatePointPosition').and.returnValue({ 
          isValid: true, 
          errors: [] 
        }),
        validatePatternSettings: jasmine.createSpy('validatePatternSettings').and.returnValue({ 
          isValid: true, 
          errors: [] 
        }),
        validatePattern: jasmine.createSpy('validatePattern').and.returnValue({ 
          isValid: true, 
          errors: [] 
        }),
        checkMinimumSpacing: jasmine.createSpy('checkMinimumSpacing').and.returnValue(true),
        checkMaximumDepth: jasmine.createSpy('checkMaximumDepth').and.returnValue(true),
        detectDuplicatePoints: jasmine.createSpy('detectDuplicatePoints').and.returnValue([])
      };

      expect(service.validateDrillPoint).toBeDefined();
      expect(service.validatePointPosition).toBeDefined();
      expect(service.validatePatternSettings).toBeDefined();
      expect(service.checkMinimumSpacing).toBeDefined();
    });
  });

  describe('IErrorHandlingService', () => {
    it('should define required error handling methods', () => {
      const service: IErrorHandlingService = {
        handleError: jasmine.createSpy('handleError'),
        handleComponentError: jasmine.createSpy('handleComponentError'),
        handleServiceError: jasmine.createSpy('handleServiceError'),
        showUserError: jasmine.createSpy('showUserError'),
        showValidationError: jasmine.createSpy('showValidationError'),
        clearErrors: jasmine.createSpy('clearErrors'),
        logError: jasmine.createSpy('logError'),
        getErrorHistory: jasmine.createSpy('getErrorHistory').and.returnValue([]),
        attemptRecovery: jasmine.createSpy('attemptRecovery').and.returnValue(false)
      };

      expect(service.handleError).toBeDefined();
      expect(service.handleComponentError).toBeDefined();
      expect(service.showUserError).toBeDefined();
      expect(service.logError).toBeDefined();
    });
  });

  describe('Interface Type Safety', () => {
    it('should enforce correct action types', () => {
      const updateSettingsAction: PatternStateAction = {
        type: 'UPDATE_SETTINGS',
        payload: { spacing: 4 }
      };

      const addPointAction: PatternStateAction = {
        type: 'ADD_DRILL_POINT',
        payload: { x: 10, y: 20, id: '1', depth: 10, spacing: 3, burden: 2.5 }
      };

      expect(updateSettingsAction.type).toBe('UPDATE_SETTINGS');
      expect(addPointAction.type).toBe('ADD_DRILL_POINT');
    });

    it('should enforce correct validation result types', () => {
      const validationResult: { isValid: boolean; errors: string[] } = {
        isValid: false,
        errors: ['Spacing too small', 'Depth exceeds maximum']
      };

      expect(validationResult.isValid).toBe(false);
      expect(validationResult.errors).toEqual(['Spacing too small', 'Depth exceeds maximum']);
    });
  });
});