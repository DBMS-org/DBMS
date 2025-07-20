import { 
  PatternState, 
  CanvasState, 
  UIState, 
  PatternMetadata,
  createInitialPatternState,
  ToolbarAction,
  CanvasEvent,
  ModeToggleEvent,
  PointActionEvent,
  PatternActionEvent,
  PlacePointEvent,
  MovePointEvent
} from './pattern-state.model';
import { DrillPoint, PatternSettings } from './drill-point.model';

describe('PatternState Models', () => {
  describe('createInitialPatternState', () => {
    it('should create initial state with correct default values', () => {
      const projectId = 123;
      const siteId = 456;
      const initialState = createInitialPatternState(projectId, siteId);

      expect(initialState.drillPoints).toEqual([]);
      expect(initialState.selectedPoint).toBeNull();
      
      expect(initialState.settings.spacing).toBe(3);
      expect(initialState.settings.burden).toBe(2.5);
      expect(initialState.settings.depth).toBe(10);

      expect(initialState.canvasState.scale).toBe(1);
      expect(initialState.canvasState.panOffsetX).toBe(0);
      expect(initialState.canvasState.panOffsetY).toBe(0);
      expect(initialState.canvasState.isInitialized).toBeFalse();
      expect(initialState.canvasState.isDragging).toBeFalse();
      expect(initialState.canvasState.isPanning).toBeFalse();

      expect(initialState.uiState.isHolePlacementMode).toBeFalse();
      expect(initialState.uiState.isPreciseMode).toBeFalse();
      expect(initialState.uiState.isFullscreen).toBeFalse();
      expect(initialState.uiState.showInstructions).toBeFalse();
      expect(initialState.uiState.isSaved).toBeTrue();
      expect(initialState.uiState.duplicateMessage).toBeNull();
      expect(initialState.uiState.cursorPosition).toBeNull();

      expect(initialState.metadata.projectId).toBe(projectId);
      expect(initialState.metadata.siteId).toBe(siteId);
      expect(initialState.metadata.version).toBe('1.0.0');
      expect(initialState.metadata.lastModified).toBeInstanceOf(Date);
    });

    it('should create state with different project and site IDs', () => {
      const state1 = createInitialPatternState(100, 200);
      const state2 = createInitialPatternState(300, 400);

      expect(state1.metadata.projectId).toBe(100);
      expect(state1.metadata.siteId).toBe(200);
      expect(state2.metadata.projectId).toBe(300);
      expect(state2.metadata.siteId).toBe(400);
    });

    it('should create state with current timestamp', () => {
      const beforeCreation = new Date();
      const state = createInitialPatternState(1, 1);
      const afterCreation = new Date();

      expect(state.metadata.lastModified.getTime()).toBeGreaterThanOrEqual(beforeCreation.getTime());
      expect(state.metadata.lastModified.getTime()).toBeLessThanOrEqual(afterCreation.getTime());
    });
  });

  describe('PatternState Interface', () => {
    it('should have all required properties', () => {
      const state = createInitialPatternState(1, 1);

      expect(state.drillPoints).toBeDefined();
      expect(state.settings).toBeDefined();
      expect(state.selectedPoint).toBeDefined();
      expect(state.canvasState).toBeDefined();
      expect(state.uiState).toBeDefined();
      expect(state.metadata).toBeDefined();

      expect(Array.isArray(state.drillPoints)).toBeTrue();
      expect(typeof state.settings).toBe('object');
      expect(typeof state.canvasState).toBe('object');
      expect(typeof state.uiState).toBe('object');
      expect(typeof state.metadata).toBe('object');
    });

    it('should allow drill points array manipulation', () => {
      const state = createInitialPatternState(1, 1);
      const drillPoint: DrillPoint = {
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 15,
        spacing: 3,
        burden: 2.5
      };

      state.drillPoints.push(drillPoint);
      expect(state.drillPoints.length).toBe(1);
      expect(state.drillPoints[0]).toEqual(drillPoint);
    });

    it('should allow selected point assignment', () => {
      const state = createInitialPatternState(1, 1);
      const drillPoint: DrillPoint = {
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 15,
        spacing: 3,
        burden: 2.5
      };

      state.selectedPoint = drillPoint;
      expect(state.selectedPoint).toEqual(drillPoint);

      state.selectedPoint = null;
      expect(state.selectedPoint).toBeNull();
    });
  });

  describe('CanvasState Interface', () => {
    it('should have all required canvas properties', () => {
      const state = createInitialPatternState(1, 1);
      const canvasState = state.canvasState;

      expect(canvasState.scale).toBeDefined();
      expect(canvasState.panOffsetX).toBeDefined();
      expect(canvasState.panOffsetY).toBeDefined();
      expect(canvasState.isInitialized).toBeDefined();
      expect(canvasState.isDragging).toBeDefined();
      expect(canvasState.isPanning).toBeDefined();

      expect(typeof canvasState.scale).toBe('number');
      expect(typeof canvasState.panOffsetX).toBe('number');
      expect(typeof canvasState.panOffsetY).toBe('number');
      expect(typeof canvasState.isInitialized).toBe('boolean');
      expect(typeof canvasState.isDragging).toBe('boolean');
      expect(typeof canvasState.isPanning).toBe('boolean');
    });

    it('should allow canvas state modifications', () => {
      const state = createInitialPatternState(1, 1);
      
      state.canvasState.scale = 2.5;
      state.canvasState.panOffsetX = 100;
      state.canvasState.panOffsetY = -50;
      state.canvasState.isInitialized = true;
      state.canvasState.isDragging = true;
      state.canvasState.isPanning = false;

      expect(state.canvasState.scale).toBe(2.5);
      expect(state.canvasState.panOffsetX).toBe(100);
      expect(state.canvasState.panOffsetY).toBe(-50);
      expect(state.canvasState.isInitialized).toBeTrue();
      expect(state.canvasState.isDragging).toBeTrue();
      expect(state.canvasState.isPanning).toBeFalse();
    });
  });

  describe('UIState Interface', () => {
    it('should have all required UI properties', () => {
      const state = createInitialPatternState(1, 1);
      const uiState = state.uiState;

      expect(uiState.isHolePlacementMode).toBeDefined();
      expect(uiState.isPreciseMode).toBeDefined();
      expect(uiState.isFullscreen).toBeDefined();
      expect(uiState.showInstructions).toBeDefined();
      expect(uiState.isSaved).toBeDefined();
      expect(uiState.duplicateMessage).toBeDefined();
      expect(uiState.cursorPosition).toBeDefined();

      expect(typeof uiState.isHolePlacementMode).toBe('boolean');
      expect(typeof uiState.isPreciseMode).toBe('boolean');
      expect(typeof uiState.isFullscreen).toBe('boolean');
      expect(typeof uiState.showInstructions).toBe('boolean');
      expect(typeof uiState.isSaved).toBe('boolean');
    });

    it('should allow UI state modifications', () => {
      const state = createInitialPatternState(1, 1);
      
      state.uiState.isHolePlacementMode = true;
      state.uiState.isPreciseMode = true;
      state.uiState.isFullscreen = true;
      state.uiState.showInstructions = true;
      state.uiState.isSaved = false;
      state.uiState.duplicateMessage = 'Duplicate detected';
      state.uiState.cursorPosition = { x: 100, y: 200 };

      expect(state.uiState.isHolePlacementMode).toBeTrue();
      expect(state.uiState.isPreciseMode).toBeTrue();
      expect(state.uiState.isFullscreen).toBeTrue();
      expect(state.uiState.showInstructions).toBeTrue();
      expect(state.uiState.isSaved).toBeFalse();
      expect(state.uiState.duplicateMessage).toBe('Duplicate detected');
      expect(state.uiState.cursorPosition).toEqual({ x: 100, y: 200 });
    });

    it('should handle null cursor position', () => {
      const state = createInitialPatternState(1, 1);
      
      state.uiState.cursorPosition = { x: 50, y: 75 };
      expect(state.uiState.cursorPosition).toEqual({ x: 50, y: 75 });

      state.uiState.cursorPosition = null;
      expect(state.uiState.cursorPosition).toBeNull();
    });
  });

  describe('PatternMetadata Interface', () => {
    it('should have all required metadata properties', () => {
      const state = createInitialPatternState(123, 456);
      const metadata = state.metadata;

      expect(metadata.projectId).toBeDefined();
      expect(metadata.siteId).toBeDefined();
      expect(metadata.lastModified).toBeDefined();
      expect(metadata.version).toBeDefined();

      expect(typeof metadata.projectId).toBe('number');
      expect(typeof metadata.siteId).toBe('number');
      expect(metadata.lastModified).toBeInstanceOf(Date);
      expect(typeof metadata.version).toBe('string');
    });

    it('should allow metadata modifications', () => {
      const state = createInitialPatternState(1, 1);
      const newDate = new Date('2024-01-01');
      
      state.metadata.projectId = 999;
      state.metadata.siteId = 888;
      state.metadata.lastModified = newDate;
      state.metadata.version = '2.0.0';

      expect(state.metadata.projectId).toBe(999);
      expect(state.metadata.siteId).toBe(888);
      expect(state.metadata.lastModified).toEqual(newDate);
      expect(state.metadata.version).toBe('2.0.0');
    });
  });

  describe('Event Interfaces', () => {
    it('should create valid ToolbarAction', () => {
      const action: ToolbarAction = {
        type: 'UPDATE_SETTINGS',
        payload: { spacing: 4 }
      };

      expect(action.type).toBe('UPDATE_SETTINGS');
      expect(action.payload).toEqual({ spacing: 4 });
    });

    it('should create valid CanvasEvent', () => {
      const event: CanvasEvent = {
        type: 'POINT_PLACED',
        payload: { x: 10, y: 20 }
      };

      expect(event.type).toBe('POINT_PLACED');
      expect(event.payload).toEqual({ x: 10, y: 20 });
    });

    it('should create valid ModeToggleEvent', () => {
      const event: ModeToggleEvent = {
        mode: 'HOLE_PLACEMENT',
        enabled: true
      };

      expect(event.mode).toBe('HOLE_PLACEMENT');
      expect(event.enabled).toBeTrue();
    });

    it('should create valid PointActionEvent', () => {
      const event: PointActionEvent = {
        action: 'DELETE',
        pointId: 'DH1'
      };

      expect(event.action).toBe('DELETE');
      expect(event.pointId).toBe('DH1');
    });

    it('should create PointActionEvent without pointId', () => {
      const event: PointActionEvent = {
        action: 'CLEAR_ALL'
      };

      expect(event.action).toBe('CLEAR_ALL');
      expect(event.pointId).toBeUndefined();
    });

    it('should create valid PatternActionEvent', () => {
      const event: PatternActionEvent = {
        action: 'SAVE'
      };

      expect(event.action).toBe('SAVE');
    });

    it('should create valid PlacePointEvent', () => {
      const settings: PatternSettings = {
        spacing: 3,
        burden: 2.5,
        depth: 10
      };

      const event: PlacePointEvent = {
        x: 15,
        y: 25,
        settings
      };

      expect(event.x).toBe(15);
      expect(event.y).toBe(25);
      expect(event.settings).toEqual(settings);
    });

    it('should create valid MovePointEvent', () => {
      const event: MovePointEvent = {
        pointId: 'DH1',
        newX: 30,
        newY: 40
      };

      expect(event.pointId).toBe('DH1');
      expect(event.newX).toBe(30);
      expect(event.newY).toBe(40);
    });
  });

  describe('Type Safety', () => {
    it('should enforce toolbar action types', () => {
      const validTypes: ToolbarAction['type'][] = [
        'TOGGLE_MODE',
        'UPDATE_SETTINGS',
        'POINT_ACTION',
        'PATTERN_ACTION'
      ];

      validTypes.forEach(type => {
        const action: ToolbarAction = { type, payload: {} };
        expect(action.type).toBe(type);
      });
    });

    it('should enforce canvas event types', () => {
      const validTypes: CanvasEvent['type'][] = [
        'POINT_PLACED',
        'POINT_SELECTED',
        'POINT_MOVED',
        'STATE_CHANGED'
      ];

      validTypes.forEach(type => {
        const event: CanvasEvent = { type, payload: {} };
        expect(event.type).toBe(type);
      });
    });

    it('should enforce mode toggle types', () => {
      const validModes: ModeToggleEvent['mode'][] = [
        'HOLE_PLACEMENT',
        'PRECISE',
        'FULLSCREEN'
      ];

      validModes.forEach(mode => {
        const event: ModeToggleEvent = { mode, enabled: true };
        expect(event.mode).toBe(mode);
      });
    });

    it('should enforce point action types', () => {
      const validActions: PointActionEvent['action'][] = [
        'DELETE',
        'CLEAR_ALL',
        'OPEN_DEPTH_EDITOR'
      ];

      validActions.forEach(action => {
        const event: PointActionEvent = { action };
        expect(event.action).toBe(action);
      });
    });

    it('should enforce pattern action types', () => {
      const validActions: PatternActionEvent['action'][] = [
        'SAVE',
        'EXPORT_TO_BLAST_DESIGNER'
      ];

      validActions.forEach(action => {
        const event: PatternActionEvent = { action };
        expect(event.action).toBe(action);
      });
    });
  });
});