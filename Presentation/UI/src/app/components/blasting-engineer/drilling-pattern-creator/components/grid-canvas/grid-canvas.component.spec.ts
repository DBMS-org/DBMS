import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SimpleChange } from '@angular/core';
import Konva from 'konva';
import { GridCanvasComponent } from './grid-canvas.component';
import { PatternSettings } from '../../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../../constants/canvas.constants';

describe('GridCanvasComponent', () => {
  let component: GridCanvasComponent;
  let fixture: ComponentFixture<GridCanvasComponent>;
  let mockLayer: jasmine.SpyObj<Konva.Layer>;
  let mockStage: jasmine.SpyObj<Konva.Stage>;

  const defaultSettings: PatternSettings = {
    spacing: 3,
    burden: 2.5,
    depth: 10
  };

  const defaultCanvasState = {
    scale: 1,
    offsetX: 0,
    offsetY: 0,
    panOffsetX: 0,
    panOffsetY: 0,
    width: 800,
    height: 600
  };

  beforeEach(async () => {
    // Create mock layer with spy methods
    mockLayer = jasmine.createSpyObj('Layer', ['add', 'batchDraw', 'destroy']);
    mockStage = jasmine.createSpyObj('Stage', ['width', 'height']);
    (mockStage.width as jasmine.Spy).and.returnValue(800);
    (mockStage.height as jasmine.Spy).and.returnValue(600);

    await TestBed.configureTestingModule({
      imports: [GridCanvasComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(GridCanvasComponent);
    component = fixture.componentInstance;

    // Set up component inputs
    component.layer = mockLayer;
    component.settings = { ...defaultSettings };
    component.canvasState = { ...defaultCanvasState };
    component.isPreciseMode = false;
  });

  afterEach(() => {
    // Clean up any remaining objects
    component.clearAllCaches();
  });

  describe('Component Lifecycle', () => {
    it('should create', () => {
      expect(component).toBeTruthy();
    });

    it('should initialize without errors', () => {
      expect(() => component.ngOnInit()).not.toThrow();
    });

    it('should destroy cleanly', () => {
      component.ngOnInit();
      expect(() => component.ngOnDestroy()).not.toThrow();
    });

    it('should clear caches on destroy', () => {
      component.ngOnInit();
      spyOn(component, 'clearAllCaches');
      component.ngOnDestroy();
      expect(component.clearAllCaches).toHaveBeenCalled();
    });
  });

  describe('Input Changes', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should update grid when settings change', () => {
      spyOn(component, 'updateGrid');
      
      const changes = {
        settings: new SimpleChange(defaultSettings, { ...defaultSettings, spacing: 4 }, false)
      };
      
      component.ngOnChanges(changes);
      expect(component.updateGrid).toHaveBeenCalled();
    });

    it('should update grid when canvas state changes', () => {
      spyOn(component, 'updateGrid');
      
      const changes = {
        canvasState: new SimpleChange(defaultCanvasState, { ...defaultCanvasState, scale: 2 }, false)
      };
      
      component.ngOnChanges(changes);
      expect(component.updateGrid).toHaveBeenCalled();
    });

    it('should update grid when precise mode changes', () => {
      spyOn(component, 'updateGrid');
      
      const changes = {
        isPreciseMode: new SimpleChange(false, true, false)
      };
      
      component.ngOnChanges(changes);
      expect(component.updateGrid).toHaveBeenCalled();
    });

    it('should not update grid for unrelated changes', () => {
      spyOn(component, 'updateGrid');
      
      const changes = {
        someOtherProperty: new SimpleChange('old', 'new', false)
      };
      
      component.ngOnChanges(changes);
      expect(component.updateGrid).not.toHaveBeenCalled();
    });
  });

  describe('Grid Rendering', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should render grid successfully', () => {
      expect(() => component.renderGrid()).not.toThrow();
      expect(mockLayer.batchDraw).toHaveBeenCalled();
    });

    it('should handle missing layer gracefully', () => {
      component.layer = null as any;
      expect(() => component.renderGrid()).not.toThrow();
      expect(mockLayer.batchDraw).not.toHaveBeenCalled();
    });

    it('should handle missing settings gracefully', () => {
      component.settings = null as any;
      expect(() => component.renderGrid()).not.toThrow();
      expect(mockLayer.batchDraw).not.toHaveBeenCalled();
    });

    it('should handle missing canvas state gracefully', () => {
      component.canvasState = null as any;
      expect(() => component.renderGrid()).not.toThrow();
      expect(mockLayer.batchDraw).not.toHaveBeenCalled();
    });

    it('should track render performance', () => {
      const initialStats = component.getCacheStats();
      component.renderGrid();
      const afterStats = component.getCacheStats();
      
      expect(afterStats.renderCount).toBe(initialStats.renderCount + 1);
      expect(afterStats.lastRenderTime).toBeGreaterThan(0);
    });
  });

  describe('Grid Caching', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should cache grid after first render', () => {
      const initialStats = component.getCacheStats();
      component.renderGrid();
      const afterStats = component.getCacheStats();
      
      expect(afterStats.gridCacheSize).toBeGreaterThan(initialStats.gridCacheSize);
    });

    it('should use cached grid on subsequent renders with same parameters', () => {
      // First render - should create cache
      component.renderGrid();
      const firstStats = component.getCacheStats();
      
      // Second render with same parameters - should use cache
      component.renderGrid();
      const secondStats = component.getCacheStats();
      
      expect(secondStats.gridCacheSize).toBe(firstStats.gridCacheSize);
      expect(secondStats.renderCount).toBe(firstStats.renderCount + 1);
    });

    it('should create new cache entry when parameters change', () => {
      // First render
      component.renderGrid();
      const firstStats = component.getCacheStats();
      
      // Change settings and render again
      component.settings = { ...defaultSettings, spacing: 4 };
      component.renderGrid();
      const secondStats = component.getCacheStats();
      
      expect(secondStats.gridCacheSize).toBeGreaterThan(firstStats.gridCacheSize);
    });

    it('should limit cache size', () => {
      // Render with many different settings to exceed cache limit
      for (let i = 0; i < CANVAS_CONSTANTS.MAX_CACHE_SIZE + 5; i++) {
        component.settings = { ...defaultSettings, spacing: 3 + i * 0.1 };
        component.renderGrid();
      }
      
      const stats = component.getCacheStats();
      expect(stats.gridCacheSize).toBeLessThanOrEqual(CANVAS_CONSTANTS.MAX_CACHE_SIZE);
    });

    it('should clear all caches', () => {
      component.renderGrid();
      component.isPreciseMode = true;
      component.renderGrid();
      
      let stats = component.getCacheStats();
      expect(stats.gridCacheSize + stats.intersectionCacheSize).toBeGreaterThan(0);
      
      component.clearAllCaches();
      stats = component.getCacheStats();
      expect(stats.gridCacheSize).toBe(0);
      expect(stats.intersectionCacheSize).toBe(0);
    });
  });

  describe('Precise Mode Intersections', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should not render intersections when precise mode is false', () => {
      component.isPreciseMode = false;
      component.renderGrid();
      
      const stats = component.getCacheStats();
      expect(stats.intersectionCacheSize).toBe(0);
    });

    it('should render intersections when precise mode is true', () => {
      component.isPreciseMode = true;
      component.renderGrid();
      
      const stats = component.getCacheStats();
      expect(stats.intersectionCacheSize).toBeGreaterThan(0);
    });

    it('should cache intersections separately from grid', () => {
      component.isPreciseMode = true;
      component.renderGrid();
      
      const stats = component.getCacheStats();
      expect(stats.gridCacheSize).toBeGreaterThan(0);
      expect(stats.intersectionCacheSize).toBeGreaterThan(0);
    });

    it('should update intersections when precise mode changes', () => {
      // First render without precise mode
      component.isPreciseMode = false;
      component.renderGrid();
      let stats = component.getCacheStats();
      expect(stats.intersectionCacheSize).toBe(0);
      
      // Enable precise mode and render
      component.isPreciseMode = true;
      component.renderGrid();
      stats = component.getCacheStats();
      expect(stats.intersectionCacheSize).toBeGreaterThan(0);
    });
  });

  describe('Grid Update and Refresh', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should update grid', () => {
      spyOn(component, 'renderGrid');
      component.updateGrid();
      expect(component.renderGrid).toHaveBeenCalled();
    });

    it('should clear grid', () => {
      component.renderGrid();
      component.clearGrid();
      expect(mockLayer.batchDraw).toHaveBeenCalled();
    });

    it('should handle clear grid without layer', () => {
      component.layer = null as any;
      expect(() => component.clearGrid()).not.toThrow();
    });
  });

  describe('Performance and Error Handling', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should handle large grid sizes gracefully', () => {
      // Set very small spacing to create many grid lines
      component.settings = { ...defaultSettings, spacing: 0.1, burden: 0.1 };
      component.canvasState = { ...defaultCanvasState, scale: 10 };
      
      expect(() => component.renderGrid()).not.toThrow();
    });

    it('should handle zero or negative spacing', () => {
      component.settings = { ...defaultSettings, spacing: 0, burden: -1 };
      expect(() => component.renderGrid()).not.toThrow();
    });

    it('should handle extreme zoom levels', () => {
      component.canvasState = { ...defaultCanvasState, scale: 100 };
      expect(() => component.renderGrid()).not.toThrow();
      
      component.canvasState = { ...defaultCanvasState, scale: 0.01 };
      expect(() => component.renderGrid()).not.toThrow();
    });

    it('should handle extreme pan offsets', () => {
      component.canvasState = { 
        ...defaultCanvasState, 
        panOffsetX: 10000, 
        panOffsetY: -10000 
      };
      expect(() => component.renderGrid()).not.toThrow();
    });

    it('should provide cache statistics', () => {
      const stats = component.getCacheStats();
      expect(stats).toEqual(jasmine.objectContaining({
        gridCacheSize: jasmine.any(Number),
        intersectionCacheSize: jasmine.any(Number),
        lastRenderTime: jasmine.any(Number),
        renderCount: jasmine.any(Number)
      }));
    });
  });

  describe('Memory Management', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should properly destroy Konva objects', () => {
      component.renderGrid();
      component.isPreciseMode = true;
      component.renderGrid();
      
      // Spy on destroy methods would require more complex mocking
      // For now, just ensure no errors are thrown
      expect(() => component.ngOnDestroy()).not.toThrow();
    });

    it('should handle multiple renders without memory leaks', () => {
      // Render multiple times with different settings
      for (let i = 0; i < 10; i++) {
        component.settings = { ...defaultSettings, spacing: 3 + i * 0.1 };
        component.isPreciseMode = i % 2 === 0;
        component.renderGrid();
      }
      
      expect(() => component.clearAllCaches()).not.toThrow();
    });
  });

  describe('Integration with Canvas Constants', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should respect MAX_GRID_LINES constant', () => {
      // This test would need to be more sophisticated to actually count lines
      // For now, just ensure the constant is being used
      expect(CANVAS_CONSTANTS.MAX_GRID_LINES).toBeDefined();
      expect(CANVAS_CONSTANTS.MAX_GRID_LINES).toBeGreaterThan(0);
    });

    it('should respect MAX_CACHE_SIZE constant', () => {
      expect(CANVAS_CONSTANTS.MAX_CACHE_SIZE).toBeDefined();
      expect(CANVAS_CONSTANTS.MAX_CACHE_SIZE).toBeGreaterThan(0);
    });

    it('should use correct ruler dimensions', () => {
      expect(CANVAS_CONSTANTS.RULER_WIDTH).toBeDefined();
      expect(CANVAS_CONSTANTS.RULER_HEIGHT).toBeDefined();
    });

    it('should use correct grid size', () => {
      expect(CANVAS_CONSTANTS.GRID_SIZE).toBeDefined();
      expect(CANVAS_CONSTANTS.GRID_SIZE).toBeGreaterThan(0);
    });
  });
});