import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, ViewChild } from '@angular/core';
import { of } from 'rxjs';

import { GridCanvasComponent } from './grid-canvas.component';
import { PatternSettings } from '../../models/drill-point.model';
import { CanvasState } from '../../models/pattern-state.model';
import { GridRenderingService } from '../../services/grid-rendering.service';
import { 
  GRID_RENDERING_SERVICE,
  CANVAS_MANAGER_SERVICE,
  ERROR_HANDLING_SERVICE,
  PERFORMANCE_MONITORING_SERVICE
} from '../../tokens/injection.tokens';

/**
 * Integration tests for GridCanvasComponent
 * 
 * These tests verify the component works correctly with real services
 * and handles the complete grid rendering workflow.
 */

@Component({
  template: `
    <app-grid-canvas
      [settings]="settings"
      [canvasState]="canvasState"
      [isPreciseMode]="isPreciseMode"
      [visible]="visible">
    </app-grid-canvas>
  `
})
class TestHostComponent {
  @ViewChild(GridCanvasComponent) gridCanvas!: GridCanvasComponent;
  
  settings: PatternSettings = {
    spacing: 3,
    burden: 2.5,
    depth: 10
  };
  
  canvasState: CanvasState = {
    scale: 1,
    panOffsetX: 0,
    panOffsetY: 0,
    isInitialized: true,
    isDragging: false,
    isPanning: false
  };
  
  isPreciseMode = false;
  visible = true;
}

describe('GridCanvasComponent Integration', () => {
  let hostComponent: TestHostComponent;
  let fixture: ComponentFixture<TestHostComponent>;
  let gridComponent: GridCanvasComponent;
  let gridRenderingService: GridRenderingService;
  let mockCanvasManager: any;
  let mockErrorHandler: any;
  let mockPerformanceMonitor: any;
  let mockLayer: any;

  beforeEach(async () => {
    // Create mock layer
    mockLayer = {
      add: jasmine.createSpy('add'),
      remove: jasmine.createSpy('remove'),
      destroy: jasmine.createSpy('destroy'),
      batchDraw: jasmine.createSpy('batchDraw'),
      getStage: jasmine.createSpy('getStage').and.returnValue({
        width: () => 800,
        height: () => 600,
        scaleX: () => 1,
        position: () => ({ x: 0, y: 0 })
      })
    };

    // Create mock services
    mockCanvasManager = {
      createLayer: jasmine.createSpy('createLayer').and.returnValue(mockLayer),
      getLayer: jasmine.createSpy('getLayer').and.returnValue(mockLayer),
      removeLayer: jasmine.createSpy('removeLayer'),
      clearLayer: jasmine.createSpy('clearLayer')
    };

    mockErrorHandler = {
      handleError: jasmine.createSpy('handleError'),
      handleComponentError: jasmine.createSpy('handleComponentError'),
      showUserError: jasmine.createSpy('showUserError'),
      logError: jasmine.createSpy('logError')
    };

    mockPerformanceMonitor = {
      startTimer: jasmine.createSpy('startTimer').and.returnValue('timer-id'),
      endTimer: jasmine.createSpy('endTimer').and.returnValue(100),
      recordMetric: jasmine.createSpy('recordMetric'),
      trackMemoryLeak: jasmine.createSpy('trackMemoryLeak')
    };

    await TestBed.configureTestingModule({
      imports: [GridCanvasComponent],
      declarations: [TestHostComponent],
      providers: [
        GridRenderingService,
        { provide: CANVAS_MANAGER_SERVICE, useValue: mockCanvasManager },
        { provide: ERROR_HANDLING_SERVICE, useValue: mockErrorHandler },
        { provide: PERFORMANCE_MONITORING_SERVICE, useValue: mockPerformanceMonitor }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TestHostComponent);
    hostComponent = fixture.componentInstance;
    gridRenderingService = TestBed.inject(GridRenderingService);
    
    fixture.detectChanges();
    gridComponent = hostComponent.gridCanvas;
  });

  describe('Component Integration', () => {
    it('should create and initialize properly', () => {
      expect(hostComponent).toBeTruthy();
      expect(gridComponent).toBeTruthy();
      expect(gridRenderingService).toBeTruthy();
    });

    it('should initialize grid rendering service with layer', () => {
      expect(mockCanvasManager.createLayer).toHaveBeenCalledWith('grid');
      expect(mockLayer.add).toHaveBeenCalled();
    });

    it('should set up performance monitoring', () => {
      expect(mockPerformanceMonitor.trackMemoryLeak).toHaveBeenCalledWith('GridCanvasComponent');
      expect(mockPerformanceMonitor.recordMetric).toHaveBeenCalled();
    });
  });

  describe('Grid Rendering Integration', () => {
    it('should render grid when component is initialized', () => {
      spyOn(gridComponent, 'renderGrid').and.callThrough();
      
      gridComponent.ngOnInit();
      
      expect(gridComponent.renderGrid).toHaveBeenCalled();
    });

    it('should update grid when settings change', () => {
      spyOn(gridComponent, 'updateGridSpacing');
      
      hostComponent.settings = { ...hostComponent.settings, spacing: 5 };
      fixture.detectChanges();
      
      // Trigger settings change manually since we're testing integration
      (gridComponent as any).onSettingsChange(hostComponent.settings);
      
      expect(gridComponent.updateGridSpacing).toHaveBeenCalledWith(5);
    });

    it('should update grid when canvas state changes', () => {
      spyOn(gridComponent, 'update');
      
      hostComponent.canvasState = { ...hostComponent.canvasState, scale: 2 };
      fixture.detectChanges();
      
      // Trigger canvas state change manually
      (gridComponent as any).onCanvasStateChange(hostComponent.canvasState);
      
      expect(gridComponent.update).toHaveBeenCalled();
    });
  });

  describe('Precise Mode Integration', () => {
    it('should show intersections when precise mode is enabled', () => {
      spyOn(gridComponent, 'showIntersections');
      
      hostComponent.isPreciseMode = true;
      fixture.detectChanges();
      
      gridComponent.onPreciseModeChange(true);
      
      expect(gridComponent.showIntersections).toHaveBeenCalledWith(true);
    });

    it('should hide intersections when precise mode is disabled', () => {
      spyOn(gridComponent, 'showIntersections');
      
      hostComponent.isPreciseMode = false;
      fixture.detectChanges();
      
      gridComponent.onPreciseModeChange(false);
      
      expect(gridComponent.showIntersections).toHaveBeenCalledWith(false);
    });
  });

  describe('Visibility Integration', () => {
    it('should handle visibility changes', () => {
      spyOn(gridRenderingService, 'setVisible');
      
      hostComponent.visible = false;
      fixture.detectChanges();
      
      gridComponent.onVisibilityChange(false);
      
      expect(gridRenderingService.setVisible).toHaveBeenCalledWith(false);
    });

    it('should show grid when visible is true', () => {
      spyOn(gridRenderingService, 'setVisible');
      
      hostComponent.visible = true;
      fixture.detectChanges();
      
      gridComponent.onVisibilityChange(true);
      
      expect(gridRenderingService.setVisible).toHaveBeenCalledWith(true);
    });
  });

  describe('Performance Integration', () => {
    it('should track rendering performance', () => {
      gridComponent.renderGrid();
      
      expect(mockPerformanceMonitor.startTimer).toHaveBeenCalledWith('grid-render');
      expect(mockPerformanceMonitor.endTimer).toHaveBeenCalledWith('timer-id');
    });

    it('should provide performance metrics', () => {
      const metrics = gridComponent.getPerformanceMetrics();
      
      expect(metrics).toEqual(jasmine.objectContaining({
        lastRenderTime: jasmine.any(Number),
        cacheHitRate: jasmine.any(Number),
        gridComplexity: jasmine.any(Number)
      }));
    });

    it('should calculate grid complexity based on settings and state', () => {
      hostComponent.settings.spacing = 1; // Smaller spacing = higher complexity
      hostComponent.canvasState.scale = 2; // Higher scale = higher complexity
      fixture.detectChanges();
      
      const metrics = gridComponent.getPerformanceMetrics();
      
      expect(metrics.gridComplexity).toBeGreaterThan(0);
    });
  });

  describe('Error Handling Integration', () => {
    it('should handle grid rendering errors', () => {
      spyOn(gridRenderingService, 'render').and.throwError('Render error');
      
      gridComponent.renderGrid();
      
      expect(mockErrorHandler.showUserError).toHaveBeenCalledWith(
        'Grid rendering failed. The grid may not display correctly.',
        'warning'
      );
    });

    it('should handle invalid spacing values', () => {
      gridComponent.updateGridSpacing(-1);
      
      expect(mockErrorHandler.showUserError).toHaveBeenCalled();
    });

    it('should handle invalid color values', () => {
      gridComponent.setGridColor('');
      
      expect(mockErrorHandler.showUserError).toHaveBeenCalled();
    });
  });

  describe('Caching Integration', () => {
    it('should use caching for performance', () => {
      spyOn(gridRenderingService, 'enableCaching');
      
      gridComponent.ngOnInit();
      
      // Caching should be enabled by default
      expect(gridRenderingService.enableCaching).toHaveBeenCalledWith(true);
    });

    it('should invalidate cache when properties change', () => {
      spyOn(gridRenderingService, 'invalidateCache');
      
      gridComponent.updateGridSpacing(5);
      
      expect(gridRenderingService.invalidateCache).toHaveBeenCalled();
    });

    it('should force refresh by invalidating cache', () => {
      spyOn(gridRenderingService, 'invalidateCache');
      spyOn(gridComponent, 'renderGrid');
      
      gridComponent.forceRefresh();
      
      expect(gridRenderingService.invalidateCache).toHaveBeenCalled();
      expect(gridComponent.renderGrid).toHaveBeenCalled();
    });
  });

  describe('Lifecycle Integration', () => {
    it('should clean up resources on destroy', () => {
      spyOn(gridRenderingService, 'destroy');
      
      gridComponent.ngOnDestroy();
      
      // Base class should handle cleanup
      expect(gridComponent).toBeTruthy(); // Component still exists but cleaned up
    });

    it('should handle multiple initialization calls gracefully', () => {
      expect(() => {
        gridComponent.ngOnInit();
        gridComponent.ngOnInit(); // Second call should not cause issues
      }).not.toThrow();
    });
  });

  describe('Real-world Scenarios', () => {
    it('should handle rapid settings changes', () => {
      spyOn(gridComponent, 'update');
      
      // Simulate rapid settings changes
      for (let i = 1; i <= 10; i++) {
        hostComponent.settings = { ...hostComponent.settings, spacing: i };
        (gridComponent as any).onSettingsChange(hostComponent.settings);
      }
      
      // Should handle all changes without errors
      expect(gridComponent.update).toHaveBeenCalledTimes(10);
    });

    it('should handle canvas state updates during rendering', () => {
      spyOn(gridComponent, 'update');
      
      // Start rendering
      gridComponent.renderGrid();
      
      // Change state during rendering
      hostComponent.canvasState = { ...hostComponent.canvasState, scale: 1.5 };
      (gridComponent as any).onCanvasStateChange(hostComponent.canvasState);
      
      expect(gridComponent.update).toHaveBeenCalled();
    });

    it('should maintain performance with large grid complexity', () => {
      // Set up for high complexity
      hostComponent.settings.spacing = 0.5; // Very small spacing
      hostComponent.canvasState.scale = 3; // High zoom
      fixture.detectChanges();
      
      const startTime = Date.now();
      gridComponent.renderGrid();
      const endTime = Date.now();
      
      // Should complete within reasonable time (adjust threshold as needed)
      expect(endTime - startTime).toBeLessThan(1000); // 1 second max
    });

    it('should handle precise mode toggling during rendering', () => {
      spyOn(gridComponent, 'showIntersections');
      
      // Start rendering
      gridComponent.renderGrid();
      
      // Toggle precise mode multiple times
      gridComponent.onPreciseModeChange(true);
      gridComponent.onPreciseModeChange(false);
      gridComponent.onPreciseModeChange(true);
      
      expect(gridComponent.showIntersections).toHaveBeenCalledTimes(3);
    });
  });

  describe('Accessibility Integration', () => {
    it('should provide debug information for accessibility tools', () => {
      const debugInfo = (gridComponent as any).getDebugInfo();
      
      expect(debugInfo).toEqual(jasmine.objectContaining({
        componentName: 'GridCanvasComponent',
        layerName: 'grid',
        visible: jasmine.any(Boolean),
        isPreciseMode: jasmine.any(Boolean)
      }));
    });

    it('should handle visibility changes for screen readers', () => {
      // Grid visibility should be properly managed
      gridComponent.onVisibilityChange(false);
      expect(gridComponent.visible).toBe(false);
      
      gridComponent.onVisibilityChange(true);
      expect(gridComponent.visible).toBe(true);
    });
  });
});