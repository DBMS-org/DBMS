import { TestBed } from '@angular/core/testing';
import { Component, EventEmitter } from '@angular/core';
import { Observable, of } from 'rxjs';

import { 
  BasePatternComponent,
  IDrillingPatternCreatorComponent,
  IPatternToolbarComponent,
  IPatternCanvasComponent
} from './component.interfaces';

import { 
  PatternState, 
  PatternSettings, 
  DrillPoint, 
  CanvasState,
  ToolbarAction,
  CanvasEvent,
  ModeToggleEvent,
  PointActionEvent,
  PatternActionEvent,
  PlacePointEvent,
  MovePointEvent
} from '../models';

describe('Component Interfaces', () => {
  
  describe('BasePatternComponent', () => {
    it('should define required lifecycle methods', () => {
      const component: BasePatternComponent = {
        ngOnInit: jasmine.createSpy('ngOnInit'),
        ngOnDestroy: jasmine.createSpy('ngOnDestroy')
      };

      expect(component.ngOnInit).toBeDefined();
      expect(component.ngOnDestroy).toBeDefined();
    });
  });

  describe('IDrillingPatternCreatorComponent', () => {
    it('should define required properties and methods', () => {
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

      const component: IDrillingPatternCreatorComponent = {
        ngOnInit: jasmine.createSpy('ngOnInit'),
        ngOnDestroy: jasmine.createSpy('ngOnDestroy'),
        state$: of(mockState),
        isLoading$: of(false),
        onToolbarAction: jasmine.createSpy('onToolbarAction'),
        onCanvasEvent: jasmine.createSpy('onCanvasEvent')
      };

      expect(component.state$).toBeDefined();
      expect(component.isLoading$).toBeDefined();
      expect(component.onToolbarAction).toBeDefined();
      expect(component.onCanvasEvent).toBeDefined();
    });
  });

  describe('IPatternToolbarComponent', () => {
    it('should define required inputs and outputs', () => {
      const mockSettings: PatternSettings = { spacing: 3, burden: 2.5, depth: 10 };
      const mockPoint: DrillPoint = { x: 0, y: 0, id: '1', depth: 10, spacing: 3, burden: 2.5 };

      const component: IPatternToolbarComponent = {
        ngOnInit: jasmine.createSpy('ngOnInit'),
        ngOnDestroy: jasmine.createSpy('ngOnDestroy'),
        settings: mockSettings,
        selectedPoint: mockPoint,
        drillPointsCount: 1,
        isHolePlacementMode: false,
        isPreciseMode: false,
        isSaved: true,
        settingsChange: new EventEmitter<PatternSettings>(),
        modeToggle: new EventEmitter<ModeToggleEvent>(),
        pointAction: new EventEmitter<PointActionEvent>(),
        patternAction: new EventEmitter<PatternActionEvent>(),
        onSettingsChange: jasmine.createSpy('onSettingsChange'),
        onModeToggle: jasmine.createSpy('onModeToggle'),
        onPointAction: jasmine.createSpy('onPointAction'),
        onPatternAction: jasmine.createSpy('onPatternAction')
      };

      expect(component.settings).toEqual(mockSettings);
      expect(component.selectedPoint).toEqual(mockPoint);
      expect(component.settingsChange).toBeInstanceOf(EventEmitter);
      expect(component.modeToggle).toBeInstanceOf(EventEmitter);
      expect(component.pointAction).toBeInstanceOf(EventEmitter);
      expect(component.patternAction).toBeInstanceOf(EventEmitter);
    });
  });

  describe('IPatternCanvasComponent', () => {
    it('should define required canvas methods', () => {
      const mockSettings: PatternSettings = { spacing: 3, burden: 2.5, depth: 10 };
      const mockCanvasState: CanvasState = {
        scale: 1,
        panOffsetX: 0,
        panOffsetY: 0,
        isInitialized: false,
        isDragging: false,
        isPanning: false
      };

      const component: IPatternCanvasComponent = {
        ngOnInit: jasmine.createSpy('ngOnInit'),
        ngOnDestroy: jasmine.createSpy('ngOnDestroy'),
        settings: mockSettings,
        drillPoints: [],
        selectedPoint: null,
        canvasState: mockCanvasState,
        pointPlaced: new EventEmitter<PlacePointEvent>(),
        pointSelected: new EventEmitter<DrillPoint>(),
        pointMoved: new EventEmitter<MovePointEvent>(),
        canvasStateChange: new EventEmitter<CanvasState>(),
        initializeCanvas: jasmine.createSpy('initializeCanvas'),
        updateCanvas: jasmine.createSpy('updateCanvas'),
        destroyCanvas: jasmine.createSpy('destroyCanvas'),
        onPointPlaced: jasmine.createSpy('onPointPlaced'),
        onPointSelected: jasmine.createSpy('onPointSelected'),
        onPointMoved: jasmine.createSpy('onPointMoved'),
        onCanvasStateChange: jasmine.createSpy('onCanvasStateChange')
      };

      expect(component.initializeCanvas).toBeDefined();
      expect(component.updateCanvas).toBeDefined();
      expect(component.destroyCanvas).toBeDefined();
      expect(component.pointPlaced).toBeInstanceOf(EventEmitter);
      expect(component.pointSelected).toBeInstanceOf(EventEmitter);
      expect(component.pointMoved).toBeInstanceOf(EventEmitter);
      expect(component.canvasStateChange).toBeInstanceOf(EventEmitter);
    });
  });

  describe('Interface Type Safety', () => {
    it('should enforce correct event types', () => {
      const toolbarAction: ToolbarAction = {
        type: 'TOGGLE_MODE',
        payload: { mode: 'HOLE_PLACEMENT', enabled: true }
      };

      const canvasEvent: CanvasEvent = {
        type: 'POINT_PLACED',
        payload: { x: 10, y: 20, settings: { spacing: 3, burden: 2.5, depth: 10 } }
      };

      expect(toolbarAction.type).toBe('TOGGLE_MODE');
      expect(canvasEvent.type).toBe('POINT_PLACED');
    });

    it('should enforce correct event payload types', () => {
      const modeToggleEvent: ModeToggleEvent = {
        mode: 'PRECISE',
        enabled: true
      };

      const pointActionEvent: PointActionEvent = {
        action: 'DELETE',
        pointId: 'point-1'
      };

      const patternActionEvent: PatternActionEvent = {
        action: 'SAVE'
      };

      expect(modeToggleEvent.mode).toBe('PRECISE');
      expect(pointActionEvent.action).toBe('DELETE');
      expect(patternActionEvent.action).toBe('SAVE');
    });
  });
});