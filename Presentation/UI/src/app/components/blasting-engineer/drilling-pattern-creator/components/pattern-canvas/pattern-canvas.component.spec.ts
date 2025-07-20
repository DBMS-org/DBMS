import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Component, DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { PatternCanvasComponent } from './pattern-canvas.component';
import { 
  PatternSettings, 
  DrillPoint 
} from '../../models/drill-point.model';
import { 
  CanvasState, 
  UIState, 
  PlacePointEvent, 
  MovePointEvent 
} from '../../models/pattern-state.model';
import { 
  ICanvasManagerService,
  IErrorHandlingService,
  IPerformanceMonitoringService
} from '../../interfaces/service.interfaces';
import { 
  CANVAS_MANAGER_SERVICE,
  ERROR_HANDLING_SERVICE,
  PERFORMANCE_MONITORING_SERVICE,
  CANVAS_CONFIG,
  CanvasConfig
} from '../../tokens/injection.tokens';

// Mock services
class MockCanvasManagerService implements Partial<ICanvasManagerService> {
  initializeCanvas = jasmine.createSpy('initializeCanvas').and.returnValue(Promise.resolve({}));
  destroyCanvas = jasmine.createSpy('destroyCanvas');
  resizeCanvas = jasmine.createSpy('resizeCanvas');
  resetView = jasmine.createSpy('resetView');
  zoom = jasmine.createSpy('zoom');
  screenToCanvas = jasmine.createSpy('screenToCanvas').and.returnValue({ x: 100, y: 100 });
  canvasToScreen = jasmine.createSpy('canvasToScreen').and.returnValue({ x: 100, y: 100 });
}

class MockErrorHandlingService implements Partial<IErrorHandlingService> {
  handleError = jasmine.createSpy('handleError');
  handleComponentError = jasmine.createSpy('handleComponentError');
  showUserError = jasmine.createSpy('showUserError');
  logError = jasmine.createSpy('logError');
}

class MockPerformanceMonitoringService implements Partial<IPerformanceMonitoringService> {
  startTimer = jasmine.createSpy('startTimer').and.returnValue('timer-id');
  endTimer = jasmine.createSpy('endTimer').and.returnValue(100);
  recordMetric = jasmine.createSpy('recordMetric');
  trackMemoryLeak = jasmine.createSpy('trackMemoryLeak');
}

// Test host component
@Component({
  template: `
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
  `
})
class TestHostComponent {
  settings: PatternSettings = {
    spacing: 3,
    burden: 2.5,
    depth: 10
  };

  drillPoints: DrillPoint[] = [
    { id: '1', x: 10, y: 10, depth: 10, spacing: 3, burden: 2.5 },
    { id: '2', x: 20, y: 20, depth: 12, spacing: 3, burden: 2.5 }
  ];

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

  onPointPlaced = jasmine.createSpy('onPointPlaced');
  onPointSelected = jasmine.createSpy('onPointSelected');
  onPointMoved = jasmine.createSpy('onPointMoved');
  onCanvasStateChange = jasmine.createSpy('onCanvasStateChange');
}

describe('PatternCanvasComponent', () => {
  let component: PatternCanvasComponent;
  let hostComponent: TestHostComponent;
  let fixture: ComponentFixture<TestHostComponent>;
  let canvasElement: DebugElement;
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
    mockCanvasManager = new MockCanvasManagerService();
    mockErrorHandler = new MockErrorHandlingService();
    mockPerformanceMonitor = new MockPerformanceMonitoringService();

    await TestBed.configureTestingModule({
      imports: [
        PatternCanvasComponent,
        NoopAnimationsModule
      ],
      declarations: [TestHostComponent],
      providers: [
        { provide: CANVAS_MANAGER_SERVICE, useValue: mockCanvasManager },
        { provide: ERROR_HANDLING_SERVICE, useValue: mockErrorHandler },
        { provide: PERFORMANCE_MONITORING_SERVICE, useValue: mockPerformanceMonitor },
        { provide: CANVAS_CONFIG, useValue: mockCanvasConfig }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TestHostComponent);
    hostComponent = fixture.componentInstance;
    canvasElement = fixture.debugElement.query(By.directive(PatternCanvasComponent));
    component = canvasElement.componentInstance;

    fixture.detectChanges();
  });

  describe('Component Initialization', () => {
    it('should create', () => {
      expect(component).toBeTruthy();
    });

    it('should have correct component name', () => {
      expect(component['componentName']).toBe('PatternCanvasComponent');
    });

    it('should initialize with default values', () => {
      expect(component.isInitializing).toBe(false);
      expect(component.hasError).toBe(false);
      expect(component.errorMessage).toBe('');
    });

    it('should set up performance monitoring on init', () => {
      component.ngOnInit();
      expect(mockPerformanceMonitor.trackMemoryLeak).toHaveBeenCalledWith('PatternCanvasComponent');
      expect(mockPerformanceMonitor.recordMetric).toHaveBeenCalledWith(
        'component-init',
        jasmine.any(Number),
        { component: 'PatternCanvasComponent' }
      );
    });
  });

  describe('Canvas Initialization', () => {
    it('should initialize canvas after view init', fakeAsync(() => {
      spyOn(component, 'initializeCanvas');
      component.ngAfterViewInit();
      tick();
      expect(component.initializeCanvas).toHaveBeenCalled();
    }));

    it('should call canvas manager to initialize canvas', async () => {
      await component.initializeCanvas();
      expect(mockCanvasManager.initializeCanvas).toHaveBeenCalled();
    });

    it('should set isInitializing to true during initialization', async () => {
      const initPromise = component.initializeCanvas();
      expect(component.isInitializing).toBe(true);
      await initPromise;
      expect(component.isInitializing).toBe(false);
    });

    it('should emit canvas state change when initialized', async () => {
      spyOn(component.canvasStateChange, 'emit');
      await component.initializeCanvas();
      expect(component.canvasStateChange.emit).toHaveBeenCalledWith(
        jasmine.objectContaining({ isInitialized: true })
      );
    });

    it('should handle initialization errors', async () => {
      const error = new Error('Initialization failed');
      mockCanvasManager.initializeCanvas.and.returnValue(Promise.reject(error));

      await component.initializeCanvas();

      expect(component.hasError).toBe(true);
      expect(component.errorMessage).toBe('Initialization failed');
      expect(mockErrorHandler.handleComponentError).toHaveBeenCalledWith(
        'PatternCanvasComponent',
        error,
        { phase: 'initialization' }
      );
    });

    it('should skip initialization if already initialized', async () => {
      hostComponent.canvasState = { ...hostComponent.canvasState, isInitialized: true };
      fixture.detectChanges();

      await component.initializeCanvas();
      expect(mockCanvasManager.initializeCanvas).not.toHaveBeenCalled();
    });
  });

  describe('Canvas Operations', () => {
    beforeEach(async () => {
      await component.initializeCanvas();
    });

    it('should resize canvas', () => {
      component.resizeCanvas(1000, 800);
      expect(mockCanvasManager.resizeCanvas).toHaveBeenCalledWith(1000, 800);
    });

    it('should not resize if not initialized', () => {
      hostComponent.canvasState = { ...hostComponent.canvasState, isInitialized: false };
      fixture.detectChanges();

      component.resizeCanvas(1000, 800);
      expect(mockCanvasManager.resizeCanvas).not.toHaveBeenCalled();
    });

    it('should reset view', () => {
      component.resetView();
      expect(mockCanvasManager.resetView).toHaveBeenCalled();
    });

    it('should emit canvas state change when resetting view', () => {
      spyOn(component.canvasStateChange, 'emit');
      component.resetView();
      expect(component.canvasStateChange.emit).toHaveBeenCalledWith(
        jasmine.objectContaining({
          scale: 1,
          panOffsetX: 0,
          panOffsetY: 0
        })
      );
    });

    it('should fit content to screen', () => {
      component.fitToScreen();
      expect(mockCanvasManager.zoom).toHaveBeenCalled();
    });

    it('should reset view when no drill points exist', () => {
      hostComponent.drillPoints = [];
      fixture.detectChanges();
      spyOn(component, 'resetView');

      component.fitToScreen();
      expect(component.resetView).toHaveBeenCalled();
    });
  });

  describe('Event Handling', () => {
    it('should prevent context menu', () => {
      const event = new MouseEvent('contextmenu');
      spyOn(event, 'preventDefault');

      component.onContextMenu(event);
      expect(event.preventDefault).toHaveBeenCalled();
    });

    it('should retry initialization on retry button click', () => {
      component.hasError = true;
      component.errorMessage = 'Test error';
      fixture.detectChanges();

      spyOn(component, 'initializeCanvas');
      const retryButton = fixture.debugElement.query(By.css('.retry-button'));
      retryButton.nativeElement.click();

      expect(component.hasError).toBe(false);
      expect(component.errorMessage).toBe('');
      expect(component.initializeCanvas).toHaveBeenCalled();
    });
  });

  describe('Input Changes', () => {
    it('should handle settings changes', () => {
      const newSettings: PatternSettings = {
        spacing: 4,
        burden: 3,
        depth: 15
      };

      hostComponent.settings = newSettings;
      fixture.detectChanges();

      expect(component.settings).toEqual(newSettings);
    });

    it('should handle drill points changes', () => {
      const newDrillPoints: DrillPoint[] = [
        { id: '3', x: 30, y: 30, depth: 8, spacing: 3, burden: 2.5 }
      ];

      hostComponent.drillPoints = newDrillPoints;
      fixture.detectChanges();

      expect(component.drillPoints).toEqual(newDrillPoints);
    });

    it('should handle canvas state changes', () => {
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

      expect(component.canvasState).toEqual(newCanvasState);
    });

    it('should handle UI state changes', () => {
      const newUIState: UIState = {
        isHolePlacementMode: true,
        isPreciseMode: true,
        isFullscreen: false,
        showInstructions: true,
        isSaved: false,
        duplicateMessage: 'Duplicate point detected',
        cursorPosition: { x: 50, y: 75 }
      };

      hostComponent.uiState = newUIState;
      fixture.detectChanges();

      expect(component.uiState).toEqual(newUIState);
    });
  });

  describe('CSS Classes', () => {
    it('should apply fullscreen class when in fullscreen mode', () => {
      hostComponent.uiState = { ...hostComponent.uiState, isFullscreen: true };
      fixture.detectChanges();

      const containerElement = fixture.debugElement.query(By.css('.pattern-canvas-container'));
      expect(containerElement.nativeElement).toHaveClass('fullscreen');
    });

    it('should apply hole-placement-mode class when in hole placement mode', () => {
      hostComponent.uiState = { ...hostComponent.uiState, isHolePlacementMode: true };
      fixture.detectChanges();

      const containerElement = fixture.debugElement.query(By.css('.pattern-canvas-container'));
      expect(containerElement.nativeElement).toHaveClass('hole-placement-mode');
    });

    it('should apply precise-mode class when in precise mode', () => {
      hostComponent.uiState = { ...hostComponent.uiState, isPreciseMode: true };
      fixture.detectChanges();

      const containerElement = fixture.debugElement.query(By.css('.pattern-canvas-container'));
      expect(containerElement.nativeElement).toHaveClass('precise-mode');
    });
  });

  describe('Loading and Error States', () => {
    it('should show loading overlay when initializing', () => {
      component.isInitializing = true;
      fixture.detectChanges();

      const loadingOverlay = fixture.debugElement.query(By.css('.canvas-loading-overlay'));
      expect(loadingOverlay).toBeTruthy();
    });

    it('should hide loading overlay when not initializing', () => {
      component.isInitializing = false;
      fixture.detectChanges();

      const loadingOverlay = fixture.debugElement.query(By.css('.canvas-loading-overlay'));
      expect(loadingOverlay).toBeFalsy();
    });

    it('should show error overlay when there is an error', () => {
      component.hasError = true;
      component.errorMessage = 'Test error message';
      fixture.detectChanges();

      const errorOverlay = fixture.debugElement.query(By.css('.canvas-error-overlay'));
      expect(errorOverlay).toBeTruthy();

      const errorMessage = fixture.debugElement.query(By.css('.error-message p'));
      expect(errorMessage.nativeElement.textContent).toBe('Test error message');
    });

    it('should hide error overlay when there is no error', () => {
      component.hasError = false;
      fixture.detectChanges();

      const errorOverlay = fixture.debugElement.query(By.css('.canvas-error-overlay'));
      expect(errorOverlay).toBeFalsy();
    });
  });

  describe('Accessibility', () => {
    it('should have proper ARIA attributes', () => {
      const containerElement = fixture.debugElement.query(By.css('.pattern-canvas-container'));
      
      expect(containerElement.nativeElement.getAttribute('role')).toBe('application');
      expect(containerElement.nativeElement.getAttribute('tabindex')).toBe('0');
      expect(containerElement.nativeElement.getAttribute('aria-label')).toContain('Drilling pattern canvas');
    });

    it('should update aria-label with drill points count', () => {
      hostComponent.drillPoints = [
        { id: '1', x: 10, y: 10, depth: 10, spacing: 3, burden: 2.5 },
        { id: '2', x: 20, y: 20, depth: 12, spacing: 3, burden: 2.5 },
        { id: '3', x: 30, y: 30, depth: 8, spacing: 3, burden: 2.5 }
      ];
      fixture.detectChanges();

      const containerElement = fixture.debugElement.query(By.css('.pattern-canvas-container'));
      expect(containerElement.nativeElement.getAttribute('aria-label')).toContain('3 drill points');
    });

    it('should have screen reader announcements', () => {
      const srElements = fixture.debugElement.queryAll(By.css('.sr-only'));
      expect(srElements.length).toBeGreaterThan(0);
    });
  });

  describe('Component Cleanup', () => {
    it('should destroy canvas on component destroy', () => {
      component.ngOnDestroy();
      expect(mockCanvasManager.destroyCanvas).toHaveBeenCalled();
    });

    it('should clean up resize observer on destroy', () => {
      const mockResizeObserver = {
        disconnect: jasmine.createSpy('disconnect'),
        observe: jasmine.createSpy('observe')
      };
      
      // Mock ResizeObserver
      (window as any).ResizeObserver = jasmine.createSpy('ResizeObserver').and.returnValue(mockResizeObserver);
      
      component.ngAfterViewInit();
      component.ngOnDestroy();
      
      expect(mockResizeObserver.disconnect).toHaveBeenCalled();
    });
  });

  describe('Performance Monitoring', () => {
    it('should track performance metrics', async () => {
      await component.initializeCanvas();
      expect(mockPerformanceMonitor.startTimer).toHaveBeenCalledWith('canvas-initialization');
      expect(mockPerformanceMonitor.endTimer).toHaveBeenCalled();
    });

    it('should track memory leaks', () => {
      component.ngOnInit();
      expect(mockPerformanceMonitor.trackMemoryLeak).toHaveBeenCalledWith('PatternCanvasComponent');
    });
  });

  describe('Error Handling', () => {
    it('should handle canvas operation errors', () => {
      const error = new Error('Canvas operation failed');
      mockCanvasManager.resizeCanvas.and.throwError(error);

      component.resizeCanvas(800, 600);

      expect(mockErrorHandler.handleError).toHaveBeenCalled();
    });

    it('should show user-friendly error messages', () => {
      const error = new Error('Canvas error');
      component['handleError'](error, 'canvas-operation');

      expect(mockErrorHandler.showUserError).toHaveBeenCalledWith(
        'Canvas operation failed. Please try again.',
        'error'
      );
    });
  });
});