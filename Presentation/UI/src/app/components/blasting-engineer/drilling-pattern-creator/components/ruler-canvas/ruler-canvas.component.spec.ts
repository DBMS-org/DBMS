import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SimpleChange } from '@angular/core';
import Konva from 'konva';
import { RulerCanvasComponent } from './ruler-canvas.component';
import { PatternSettings } from '../../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../../constants/canvas.constants';

describe('RulerCanvasComponent', () => {
  let component: RulerCanvasComponent;
  let fixture: ComponentFixture<RulerCanvasComponent>;
  let mockLayer: jasmine.SpyObj<Konva.Layer>;

  const mockSettings: PatternSettings = {
    spacing: 3,
    burden: 2.5,
    depth: 10
  };

  const mockCanvasState = {
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
    mockLayer = jasmine.createSpyObj('Konva.Layer', ['add', 'batchDraw', 'destroy']);

    await TestBed.configureTestingModule({
      imports: [RulerCanvasComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(RulerCanvasComponent);
    component = fixture.componentInstance;

    // Set up component inputs
    component.layer = mockLayer;
    component.settings = mockSettings;
    component.canvasState = mockCanvasState;
  });

  afterEach(() => {
    // Clean up any remaining objects
    component.clearAllCaches();
  });

  describe('Component Lifecycle', () => {
    it('should create', () => {
      expect(component).toBeTruthy();
    });

    it('should initialize correctly', () => {
      spyOn(console, 'log'); // Spy on console.log to check logging
      component.ngOnInit();
      // Component should initialize without errors
      expect(component).toBeTruthy();
    });

    it('should destroy correctly and clean up resources', () => {
      component.ngOnInit();
      spyOn(component, 'clearAllCaches');
      spyOn(console, 'log');
      
      component.ngOnDestroy();
      
      expect(component.clearAllCaches).toHaveBeenCalled();
    });
  });

  describe('Input Changes', () => {
    it('should update ruler when settings change', () => {
      spyOn(component, 'updateRuler');
      
      const changes = {
        settings: new SimpleChange(null, mockSettings, false)
      };
      
      component.ngOnChanges(changes);
      
      expect(component.updateRuler).toHaveBeenCalled();
    });

    it('should update ruler when canvas state changes', () => {
      spyOn(component, 'updateRuler');
      
      const changes = {
        canvasState: new SimpleChange(null, mockCanvasState, false)
      };
      
      component.ngOnChanges(changes);
      
      expect(component.updateRuler).toHaveBeenCalled();
    });

    it('should not update ruler when unrelated inputs change', () => {
      spyOn(component, 'updateRuler');
      
      const changes = {
        someOtherProperty: new SimpleChange(null, 'value', false)
      };
      
      component.ngOnChanges(changes);
      
      expect(component.updateRuler).not.toHaveBeenCalled();
    });
  });

  describe('Ruler Rendering', () => {
    it('should render ruler successfully with valid inputs', () => {
      spyOn(console, 'log');
      spyOn(console, 'debug');
      
      component.renderRuler();
      
      expect(mockLayer.add).toHaveBeenCalled();
      expect(mockLayer.batchDraw).toHaveBeenCalled();
    });

    it('should handle missing layer gracefully', () => {
      component.layer = null as any;
      spyOn(console, 'warn');
      
      component.renderRuler();
      
      expect(console.warn).toHaveBeenCalledWith(
        '[DrillingPattern] RulerCanvasComponent: Missing required inputs for ruler rendering'
      );
    });

    it('should handle missing settings gracefully', () => {
      component.settings = null as any;
      spyOn(console, 'warn');
      
      component.renderRuler();
      
      expect(console.warn).toHaveBeenCalledWith(
        '[DrillingPattern] RulerCanvasComponent: Missing required inputs for ruler rendering'
      );
    });

    it('should handle missing canvas state gracefully', () => {
      component.canvasState = null as any;
      spyOn(console, 'warn');
      
      component.renderRuler();
      
      expect(console.warn).toHaveBeenCalledWith(
        '[DrillingPattern] RulerCanvasComponent: Missing required inputs for ruler rendering'
      );
    });

    it('should handle rendering errors gracefully', () => {
      mockLayer.batchDraw.and.throwError('Mock rendering error');
      spyOn(console, 'error');
      
      component.renderRuler();
      
      expect(console.error).toHaveBeenCalledWith(
        '[DrillingPattern] Error rendering ruler:',
        jasmine.any(Error)
      );
    });
  });

  describe('Ruler Updates', () => {
    it('should call renderRuler when updateRuler is called', () => {
      spyOn(component, 'renderRuler');
      
      component.updateRuler();
      
      expect(component.renderRuler).toHaveBeenCalled();
    });
  });

  describe('Ruler Clearing', () => {
    it('should clear ruler and redraw layer', () => {
      component.renderRuler(); // First render something
      
      component.clearRuler();
      
      expect(mockLayer.batchDraw).toHaveBeenCalled();
    });

    it('should handle clearing when layer is null', () => {
      component.layer = null as any;
      
      expect(() => component.clearRuler()).not.toThrow();
    });
  });

  describe('Cache Management', () => {
    it('should return cache statistics', () => {
      const stats = component.getCacheStats();
      
      expect(stats).toEqual({
        cacheSize: jasmine.any(Number),
        lastRenderTime: jasmine.any(Number),
        renderCount: jasmine.any(Number)
      });
    });

    it('should clear all caches', () => {
      // Render to create cache entries
      component.renderRuler();
      
      const initialStats = component.getCacheStats();
      component.clearAllCaches();
      const finalStats = component.getCacheStats();
      
      expect(finalStats.cacheSize).toBe(0);
    });

    it('should limit cache size to prevent memory leaks', () => {
      // Create multiple different canvas states to fill cache
      for (let i = 0; i < CANVAS_CONSTANTS.MAX_CACHE_SIZE + 5; i++) {
        component.canvasState = {
          ...mockCanvasState,
          scale: 1 + i * 0.1, // Different scale to create different cache keys
          offsetX: i * 10
        };
        component.renderRuler();
      }
      
      const stats = component.getCacheStats();
      expect(stats.cacheSize).toBeLessThanOrEqual(CANVAS_CONSTANTS.MAX_CACHE_SIZE);
    });
  });

  describe('Performance Tracking', () => {
    it('should track render performance', () => {
      const initialStats = component.getCacheStats();
      
      component.renderRuler();
      
      const finalStats = component.getCacheStats();
      expect(finalStats.renderCount).toBe(initialStats.renderCount + 1);
      expect(finalStats.lastRenderTime).toBeGreaterThan(0);
    });
  });

  describe('Ruler Scaling and Positioning', () => {
    it('should handle different scale values', () => {
      const scaleValues = [0.5, 1, 2, 5, 10];
      
      scaleValues.forEach(scale => {
        component.canvasState = { ...mockCanvasState, scale };
        expect(() => component.renderRuler()).not.toThrow();
      });
    });

    it('should handle different pan offset values', () => {
      component.canvasState = {
        ...mockCanvasState,
        panOffsetX: 100,
        panOffsetY: 50
      };
      
      expect(() => component.renderRuler()).not.toThrow();
      expect(mockLayer.add).toHaveBeenCalled();
    });

    it('should handle different canvas dimensions', () => {
      component.canvasState = {
        ...mockCanvasState,
        width: 1200,
        height: 800
      };
      
      expect(() => component.renderRuler()).not.toThrow();
      expect(mockLayer.add).toHaveBeenCalled();
    });
  });

  describe('Edge Cases', () => {
    it('should handle zero scale gracefully', () => {
      component.canvasState = { ...mockCanvasState, scale: 0 };
      
      expect(() => component.renderRuler()).not.toThrow();
    });

    it('should handle negative offsets', () => {
      component.canvasState = {
        ...mockCanvasState,
        offsetX: -100,
        offsetY: -50,
        panOffsetX: -200,
        panOffsetY: -150
      };
      
      expect(() => component.renderRuler()).not.toThrow();
    });

    it('should handle very small canvas dimensions', () => {
      component.canvasState = {
        ...mockCanvasState,
        width: 100,
        height: 100
      };
      
      expect(() => component.renderRuler()).not.toThrow();
    });

    it('should handle very large canvas dimensions', () => {
      component.canvasState = {
        ...mockCanvasState,
        width: 5000,
        height: 3000
      };
      
      expect(() => component.renderRuler()).not.toThrow();
    });
  });

  describe('Memory Management', () => {
    it('should properly destroy Konva objects on component destroy', () => {
      component.renderRuler();
      
      // Spy on destroy methods would require access to private properties
      // Instead, we test that clearAllCaches is called which handles cleanup
      spyOn(component, 'clearAllCaches');
      
      component.ngOnDestroy();
      
      expect(component.clearAllCaches).toHaveBeenCalled();
    });

    it('should handle multiple render cycles without memory leaks', () => {
      // Render multiple times with different states
      for (let i = 0; i < 10; i++) {
        component.canvasState = {
          ...mockCanvasState,
          scale: 1 + i * 0.1
        };
        component.renderRuler();
      }
      
      // Should not throw and cache should be managed
      const stats = component.getCacheStats();
      expect(stats.cacheSize).toBeLessThanOrEqual(CANVAS_CONSTANTS.MAX_CACHE_SIZE);
    });
  });
});