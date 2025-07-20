import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Component, DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { BehaviorSubject, of } from 'rxjs';
import { map } from 'rxjs/operators';

import { PatternCanvasComponent } from './pattern-canvas.component';
import { PatternToolbarComponent } from '../pattern-toolbar/pattern-toolbar.component';
import { 
  PatternSettings, 
  DrillPoint 
} from '../../models/drill-point.model';
import { 
  CanvasState, 
  UIState, 
  PatternState,
  PlacePointEvent, 
  MovePointEvent 
} from '../../models/pattern-state.model';
import { 
  IPatternStateService,
  ICanvasManagerService,
  IErrorHandlingService,
  IPerformanceMonitoringService
} from '../../interfaces/service.interfaces';
import { 
  PATTERN_STATE_SERVICE,
  CANVAS_MANAGER_SERVICE,
  ERROR_HANDLING_SERVICE,
  PERFORMANCE_MONITORING_SERVICE,
  CANVAS_CONFIG,
  CanvasConfig
} from '../../tokens/injection.tokens';

// Mock services for integration testing
class MockPatternStateService implements Partial<IPatternStateService> {
  private stateSubject = new BehaviorSubject<PatternState>({
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
  });

  state$ = this.stateSubject.asObservable();
  drillPoints$ = this.state$.pipe(map(state => state.drillPoints));
  settings$ = this.state$.pipe(map(state => state.settings));
  selectedPoint$ = this.state$.pipe(map(state => state.selectedPoint));
  canvasState$ = this.state$.pipe(map(state => state.canvasState));
  uiState$ = this.state$.pipe(map(state => state.uiState));
  isLoading$ = of(false);
  error$ = of(null);

  updateSettings = jasmine.createSpy('updateSettings');
  addDrillPoint = jasmine.createSpy('addDrillPoint');
  updateDrillPoint = jasmine.createSpy('updateDrillPoint');
  deleteDrillPoint = jasmine.createSpy('deleteDrillPoint');
  selectPoint = jasmine.createSpy('selectPoint');
  updateCanvasState = jasmine.createSpy('updateCanvasState');
  updateUIState = jasmine.createSpy('updateUIState');
  clearAllPoints = jasmine.createSpy('clearAllPoints');
  resetState = jasmine.createSpy('resetState');
  dispatch = jasmine.createSpy('dispatch');
  savePattern = jasmine.createSpy('savePattern').and.returnValue(of(void 0));
  loadPattern = jasmine.createSpy('loadPattern').and.returnValue(of({}));
  getCurrentState = jasmine.createSpy('getCurrentState').and.returnValue(this.stateSubject.value);
  isStateDirty = jasmine.createSpy('isStateDirty').and.returnValue(false);

  // Helper method to update state for testing
  updateState(updates: Partial<PatternState>): void {
    const currentState = this.stateSubject.value;
    this.stateSubject.next({ ...currentState, ...updates });
  }
}

class MockCanvasManagerService implements Partial<ICanvasManagerService> {
  private mockStage = {
    getPointerPosition: () => ({ x: 100, y: 100 }),
    on: jasmine.createSpy('on'),
    off: jasmine.createSpy('off'),
    toDataURL: jasmine.createSpy('toDataURL').and.returnValue('data:image/png;base64,test'),
    scale: jasmine.createSpy('scale'),
    position: jasmine.createSpy('position'),
    batchDraw: jasmine.createSpy('batchDraw')
  };

  initializeCanvas = jasmine.createSpy('initializeCanvas').and.returnValue(Promise.resolve(this.mockStage));
  destroyCanvas = jasmine.createSpy('destroyCanvas');
  resizeCanvas = jasmine.createSpy('resizeCanvas');
  resetView = jasmine.createSpy('resetView');
  zoom = jasmine.createSpy('zoom');
  pan = jasmine.createSpy('pan');
  screenToCanvas = jasmine.createSpy('screenToCanvas').and.returnValue({ x: 100, y: 100 });
  canvasToScreen = jasmine.createSpy('canvasToScreen').and.returnValue({ x: 100, y: 100 });
  createLayer = jasmine.createSpy('createLayer').and.returnValue({});
  getLayer = jasmine.createSpy('getLayer').and.returnValue({});
  removeLayer = jasmine.createSpy('removeLayer');
  clearLayer = jasmine.createSpy('clearLayer');
  getCanvasState = jasmine.createSpy('getCanvasState').and.returnValue({
    scale: 1,
    panOffsetX: 0,
    panOffsetY: 0,
    isInitialized: true,
    isDragging: false,
    isPanning: false
  });
  updateCanvasState = jasmine.createSpy('updateCanvasState');
}

class MockErrorHandlingService implements Partial<IErrorHandlingService> {
  handleError = jasmine.createSpy('handleError');
  handleComponentError = jasmine.createSpy('handleComponentError');
  showUserError = jasmine.createSpy('showUserError');
  logError = jasmine.createSpy('logError');
  clearErrors = jasmine.createSpy('clearErrors');
}

class MockPerformanceMonitoringService implements Partial<IPerformanceMonitoringService> {
  startTimer = jasmine.createSpy('startTimer').and.returnValue('timer-id');
  endTimer = jasmine.createSpy('endTimer').and.returnValue(100);
  recordMetric = jasmine.createSpy('recordMetric');
  trackMemoryLeak = jasmine.createSpy('trackMemoryLeak');
  getMemoryUsage = jasmine.createSpy('getMemoryUsage').and.returnValue({ used: 1000, total: 2000 });
  getPerformanceReport = jasmine.createSpy('getPerformanceReport').and.returnValue({
    operations: [],
    memory: { current: 1000, peak: 1500 },
    errors: 0
  });
}

// Integration test host component
@Component({
  template: `
    <div class="pattern-creator-container">
      <app-pattern-toolbar
        [settings]="settings"
        [selectedPoint]="selectedPoint"
        [drillPointsCount]="drillPoints.length"
        [isHolePlacementMode]="uiState.isHolePlacementMode"
        [isPreciseMode]="uiState.isPreciseMode"
        [isSaved]="uiState.isSaved"
        (settingsChange)="onSettingsChange($event)"
        (modeToggle)="onModeToggle($event)"
        (pointAction)="onPointAction($event)"
        (patternAction)="onPatternAction($event)">
      </app-pattern-toolbar>

      <app-pattern-canvas
        [settings]="settings"
        [drillPoints]="drillPoints"
        [selectedPoint]="selectedPoint"
        [canvasState]="canvasState"
        [uiState]="uiState"
        (pointPlaced)="onPointPlaced($event)"
        (pointSelected)="onPointSelected($event)"
        (pointMoved)="onPointMoved($event)"
        (canvasStateChange)="onCanvasStateChange($event)">
      </app-pattern-canvas>
    </div>
  `
})
class IntegrationTestHostComponent {
  settings: PatternSettings = {
    spacing: 3,
    burden: 2.5,
    depth: 10
  };

  drillPoints: DrillPoint[] = [];
  selectedPoint: DrillPoint | null = null;

  canvasState: CanvasState = {
    scale: 1,
    panOffsetX: 0,
    panOffsetY: 0,
    isInitialized: false,
    isDragging: false,
    isPanning: false
  };

  uiState: UIState = {
    isHolePlacementMode: false,
    isPreciseMode: false,
    isFullscreen: false,
    showInstructions: false,
    isSaved: true,
    duplicateMessage: null,
    cursorPosition: null
  };

  // Event handlers
  onSettingsChange = jasmine.createSpy('onSettingsChange');
  onModeToggle = jasmine.createSpy('onModeToggle');
  onPointAction = jasmine.createSpy('onPointAction');
  onPatternAction = jasmine.createSpy('onPatternAction');
  onPointPlaced = jasmine.createSpy('onPointPlaced');
  onPointSelected = jasmine.createSpy('onPointSelected');
  onPointMoved = jasmine.createSpy('onPointMoved');
  onCanvasStateChange = jasmine.createSpy('onCanvasStateChange');
}

describe('PatternCanvasComponent Integration Tests', () => {
  let hostComponent: IntegrationTestHostComponent;
  let fixture: ComponentFixture<IntegrationTestHostComponent>;
  let canvasComponent: PatternCanvasComponent;
  let toolbarComponent: PatternToolbarComponent;
  let mockStateService: MockPatternStateService;
  let mockCanvasManager: MockCanvasManagerService;
  let mockErrorHandler: MockErrorHandlingService;
  let mockPerformanceMonitor: MockPerformanceMonitoringService;

  const mockCanvasConfig: CanvasConfig = {
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
    pixelRatio: 1
  };

  beforeEach(async () => {
    mockStateService = new MockPatternStateService();
    mockCanvasManager = new MockCanvasManagerService();
    mockErrorHandler = new MockErrorHandlingService();
    mockPerformanceMonitor = new MockPerformanceMonitoringService();

    await TestBed.configureTestingModule({
      imports: [
        PatternCanvasComponent,
        PatternToolbarComponent,
        NoopAnimationsModule
      ],
      declarations: [IntegrationTestHostComponent],
      providers: [
        { provide: PATTERN_STATE_SERVICE, useValue: mockStateService },
        { provide: CANVAS_MANAGER_SERVICE, useValue: mockCanvasManager },
        { provide: ERROR_HANDLING_SERVICE, useValue: mockErrorHandler },
        { provide: PERFORMANCE_MONITORING_SERVICE, useValue: mockPerformanceMonitor },
        { provide: CANVAS_CONFIG, useValue: mockCanvasConfig }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(IntegrationTestHostComponent);
    hostComponent = fixture.componentInstance;

    const canvasDebugElement = fixture.debugElement.query(By.directive(PatternCanvasComponent));
    const toolbarDebugElement = fixture.debugElement.query(By.directive(PatternToolbarComponent));
    
    canvasComponent = canvasDebugElement.componentInstance;
    toolbarComponent = toolbarDebugElement.componentInstance;

    fixture.detectChanges();
  });

  describe('Component Integration', () => {
    it('should create both components', () => {
      expect(canvasComponent).toBeTruthy();
      expect(toolbarComponent).toBeTruthy();
    });

    it('should initialize canvas after view init', fakeAsync(() => {
      tick();
      expect(mockCanvasManager.initializeCanvas).toHaveBeenCalled();
    }));

    it('should pass correct inputs to canvas component', () => {
      expect(canvasComponent.settings).toEqual(hostComponent.settings);
      expect(canvasComponent.drillPoints).toEqual(hostComponent.drillPoints);
      expect(canvasComponent.canvasState).toEqual(hostComponent.canvasState);
      expect(canvasComponent.uiState).toEqual(hostComponent.uiState);
    });

    it('should pass correct inputs to toolbar component', () => {
      expect(toolbarComponent.settings).toEqual(hostComponent.settings);
      expect(toolbarComponent.drillPointsCount).toBe(hostComponent.drillPoints.length);
      expect(toolbarComponent.isHolePlacementMode).toBe(hostComponent.uiState.isHolePlacementMode);
      expect(toolbarComponent.isPreciseMode).toBe(hostComponent.uiState.isPreciseMode);
    });
  });

  describe('State Synchronization', () => {
    it('should update canvas when drill points change', () => {
      const newDrillPoints: DrillPoint[] = [
        { id: '1', x: 10, y: 10, depth: 10, spacing: 3, burden: 2.5 },
        { id: '2', x: 20, y: 20, depth: 12, spacing: 3, burden: 2.5 }
      ];

      hostComponent.drillPoints = newDrillPoints;
      fixture.detectChanges();

      expect(canvasComponent.drillPoints).toEqual(newDrillPoints);
      expect(toolbarComponent.drillPointsCount).toBe(2);
    });

    it('should update both components when settings change', () => {
      const newSettings: PatternSettings = {
        spacing: 4,
        burden: 3,
        depth: 15
      };

      hostComponent.settings = newSettings;
      fixture.detectChanges();

      expect(canvasComponent.settings).toEqual(newSettings);
      expect(toolbarComponent.settings).toEqual(newSettings);
    });

    it('should update canvas when UI state changes', () => {
      const newUIState: UIState = {
        ...hostComponent.uiState,
        isHolePlacementMode: true,
        isPreciseMode: true
      };

      hostComponent.uiState = newUIState;
      fixture.detectChanges();

      expect(canvasComponent.uiState).toEqual(newUIState);
      expect(toolbarComponent.isHolePlacementMode).toBe(true);
      expect(toolbarComponent.isPreciseMode).toBe(true);
    });

    it('should update canvas when canvas state changes', () => {
      const newCanvasState: CanvasState = {
        scale: 2,
        panOffsetX: 100,
        panOffsetY: 200,
        isInitialized: true,
        isDragging: false,
        isPanning: true
      };

      hostComponent.canvasState = newCanvasState;
      fixture.detectChanges();

      expect(canvasComponent.canvasState).toEqual(newCanvasState);
    });
  });

  describe('Event Communication', () => {
    it('should emit point placed event from canvas', () => {
      const placePointEvent: PlacePointEvent = {
        x: 100,
        y: 200,
        settings: hostComponent.settings
      };

      canvasComponent.pointPlaced.emit(placePointEvent);
      expect(hostComponent.onPointPlaced).toHaveBeenCalledWith(placePointEvent);
    });

    it('should emit point selected event from canvas', () => {
      const drillPoint: DrillPoint = { id: '1', x: 10, y: 10, depth: 10, spacing: 3, burden: 2.5 };

      canvasComponent.pointSelected.emit(drillPoint);
      expect(hostComponent.onPointSelected).toHaveBeenCalledWith(drillPoint);
    });

    it('should emit point moved event from canvas', () => {
      const movePointEvent: MovePointEvent = {
        pointId: '1',
        newX: 15,
        newY: 25
      };

      canvasComponent.pointMoved.emit(movePointEvent);
      expect(hostComponent.onPointMoved).toHaveBeenCalledWith(movePointEvent);
    });

    it('should emit canvas state change from canvas', () => {
      const newCanvasState: CanvasState = {
        scale: 1.5,
        panOffsetX: 50,
        panOffsetY: 75,
        isInitialized: true,
        isDragging: false,
        isPanning: false
      };

      canvasComponent.canvasStateChange.emit(newCanvasState);
      expect(hostComponent.onCanvasStateChange).toHaveBeenCalledWith(newCanvasState);
    });

    it('should emit settings change from toolbar', () => {
      const newSettings: PatternSettings = {
        spacing: 5,
        burden: 4,
        depth: 20
      };

      toolbarComponent.settingsChange.emit(newSettings);
      expect(hostComponent.onSettingsChange).toHaveBeenCalledWith(newSettings);
    });
  });

  describe('Mode Changes', () => {
    it('should handle hole placement mode toggle', () => {
      const modeToggleEvent = {
        mode: 'HOLE_PLACEMENT' as const,
        enabled: true
      };

      toolbarComponent.modeToggle.emit(modeToggleEvent);
      expect(hostComponent.onModeToggle).toHaveBeenCalledWith(modeToggleEvent);
    });

    it('should handle precise mode toggle', () => {
      const modeToggleEvent = {
        mode: 'PRECISE' as const,
        enabled: true
      };

      toolbarComponent.modeToggle.emit(modeToggleEvent);
      expect(hostComponent.onModeToggle).toHaveBeenCalledWith(modeToggleEvent);
    });

    it('should apply correct CSS classes based on mode', () => {
      hostComponent.uiState = {
        ...hostComponent.uiState,
        isHolePlacementMode: true,
        isPreciseMode: true
      };
      fixture.detectChanges();

      const canvasContainer = fixture.debugElement.query(By.css('.pattern-canvas-container'));
      expect(canvasContainer.nativeElement).toHaveClass('hole-placement-mode');
      expect(canvasContainer.nativeElement).toHaveClass('precise-mode');
    });
  });

  describe('Error Handling Integration', () => {
    it('should handle canvas initialization errors gracefully', async () => {
      const error = new Error('Canvas initialization failed');
      mockCanvasManager.initializeCanvas.and.returnValue(Promise.reject(error));

      await canvasComponent.initializeCanvas();

      expect(canvasComponent.hasError).toBe(true);
      expect(mockErrorHandler.handleComponentError).toHaveBeenCalledWith(
        'PatternCanvasComponent',
        error,
        { phase: 'initialization' }
      );
    });

    it('should show error overlay when canvas fails to initialize', () => {
      canvasComponent.hasError = true;
      canvasComponent.errorMessage = 'Initialization failed';
      fixture.detectChanges();

      const errorOverlay = fixture.debugElement.query(By.css('.canvas-error-overlay'));
      expect(errorOverlay).toBeTruthy();
    });

    it('should allow retry after canvas error', () => {
      canvasComponent.hasError = true;
      canvasComponent.errorMessage = 'Test error';
      fixture.detectChanges();

      spyOn(canvasComponent, 'initializeCanvas');
      const retryButton = fixture.debugElement.query(By.css('.retry-button'));
      retryButton.nativeElement.click();

      expect(canvasComponent.hasError).toBe(false);
      expect(canvasComponent.initializeCanvas).toHaveBeenCalled();
    });
  });

  describe('Performance Integration', () => {
    it('should track performance metrics during initialization', async () => {
      await canvasComponent.initializeCanvas();

      expect(mockPerformanceMonitor.startTimer).toHaveBeenCalledWith('canvas-initialization');
      expect(mockPerformanceMonitor.endTimer).toHaveBeenCalled();
    });

    it('should track memory usage for both components', () => {
      expect(mockPerformanceMonitor.trackMemoryLeak).toHaveBeenCalledWith('PatternCanvasComponent');
    });

    it('should record component initialization metrics', () => {
      expect(mockPerformanceMonitor.recordMetric).toHaveBeenCalledWith(
        'component-init',
        jasmine.any(Number),
        { component: 'PatternCanvasComponent' }
      );
    });
  });

  describe('Accessibility Integration', () => {
    it('should maintain accessibility attributes across components', () => {
      const canvasContainer = fixture.debugElement.query(By.css('.pattern-canvas-container'));
      
      expect(canvasContainer.nativeElement.getAttribute('role')).toBe('application');
      expect(canvasContainer.nativeElement.getAttribute('tabindex')).toBe('0');
    });

    it('should update accessibility labels when drill points change', () => {
      hostComponent.drillPoints = [
        { id: '1', x: 10, y: 10, depth: 10, spacing: 3, burden: 2.5 },
        { id: '2', x: 20, y: 20, depth: 12, spacing: 3, burden: 2.5 }
      ];
      fixture.detectChanges();

      const canvasContainer = fixture.debugElement.query(By.css('.pattern-canvas-container'));
      expect(canvasContainer.nativeElement.getAttribute('aria-label')).toContain('2 drill points');
    });
  });

  describe('Cleanup Integration', () => {
    it('should clean up both components on destroy', () => {
      fixture.destroy();

      expect(mockCanvasManager.destroyCanvas).toHaveBeenCalled();
    });
  });
});