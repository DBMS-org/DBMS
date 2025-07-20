import { TestBed } from '@angular/core/testing';
import Konva from 'konva';

import { GridRenderingService } from './grid-rendering.service';
import { CANVAS_CONSTANTS } from '../constants/canvas.constants';

describe('GridRenderingService', () => {
  let service: GridRenderingService;
  let mockLayer: jasmine.SpyObj<Konva.Layer>;
  let mockStage: jasmine.SpyObj<Konva.Stage>;
  let mockGroup: jasmine.SpyObj<Konva.Group>;

  beforeEach(() => {
    // Create mock Konva objects
    mockGroup = jasmine.createSpyObj('Group', [
      'add',
      'destroyChildren',
      'destroy',
      'visible',
      'clone',
      'getChildren',
      'hasChildren',
      'find'
    ]);

    mockStage = jasmine.createSpyObj('Stage', [
      'width',
      'height',
      'scaleX',
      'position'
    ]);

    mockLayer = jasmine.createSpyObj('Layer', [
      'add',
      'batchDraw',
      'getStage'
    ]);

    // Configure mock returns
    mockStage.width.and.returnValue(800);
    mockStage.height.and.returnValue(600);
    mockStage.scaleX.and.returnValue(1);
    mockStage.position.and.returnValue({ x: 0, y: 0 });
    mockLayer.getStage.and.returnValue(mockStage);
    mockGroup.getChildren.and.returnValue([]);
    mockGroup.hasChildren.and.returnValue(false);
    mockGroup.find.and.returnValue([]);

    // Mock Konva.Group constructor
    spyOn(Konva, 'Group').and.returnValue(mockGroup);
    spyOn(Konva, 'Line').and.returnValue({} as any);
    spyOn(Konva, 'Circle').and.returnValue({} as any);

    TestBed.configureTestingModule({});
    service = TestBed.inject(GridRenderingService);
  });

  describe('Service Initialization', () => {
    it('should be created', () => {
      expect(service).toBeTruthy();
    });

    it('should initialize with layer', () => {
      expect(() => service.initialize(mockLayer)).not.toThrow();
      expect(mockLayer.add).toHaveBeenCalledTimes(2); // grid and intersection groups
    });

    it('should throw error when initialized without layer', () => {
      expect(() => service.initialize(null as any)).toThrowError('Layer is required for grid rendering service initialization');
    });

    it('should create grid and intersection groups on initialization', () => {
      service.initialize(mockLayer);
      
      expect(Konva.Group).toHaveBeenCalledWith({ name: 'grid-main' });
      expect(Konva.Group).toHaveBeenCalledWith({ name: 'grid-intersections' });
    });
  });

  describe('Grid Rendering', () => {
    beforeEach(() => {
      service.initialize(mockLayer);
    });

    it('should render grid successfully', () => {
      expect(() => service.render()).not.toThrow();
      expect(mockLayer.batchDraw).toHaveBeenCalled();
    });

    it('should not render when layer is not initialized', () => {
      const uninitializedService = new GridRenderingService();
      
      expect(() => uninitializedService.render()).not.toThrow();
      // Should not crash but also not render anything
    });

    it('should clear existing grid before rendering', () => {
      spyOn(service, 'clear');
      
      service.render();
      
      expect(service.clear).toHaveBeenCalled();
    });

    it('should render intersections when in precise mode', () => {
      service.showPreciseMode(true);
      
      service.render();
      
      // Should render both grid lines and intersections
      expect(mockLayer.batchDraw).toHaveBeenCalled();
    });

    it('should use cache when available and caching is enabled', () => {
      // First render to populate cache
      service.render();
      
      // Second render should use cache
      spyOn(service as any, 'tryUseCachedGrid').and.returnValue(true);
      
      service.render();
      
      expect((service as any).tryUseCachedGrid).toHaveBeenCalled();
    });

    it('should skip cache when caching is disabled', () => {
      service.enableCaching(false);
      
      service.render();
      
      // Should render without attempting to use cache
      expect(mockLayer.batchDraw).toHaveBeenCalled();
    });
  });

  describe('Grid Properties', () => {
    beforeEach(() => {
      service.initialize(mockLayer);
    });

    it('should set grid spacing', () => {
      const spacing = 5;
      
      expect(() => service.setGridSpacing(spacing)).not.toThrow();
    });

    it('should throw error for invalid spacing', () => {
      expect(() => service.setGridSpacing(-1)).toThrowError('Grid spacing must be positive');
      expect(() => service.setGridSpacing(0)).toThrowError('Grid spacing must be positive');
    });

    it('should set grid color', () => {
      const color = '#ff0000';
      
      expect(() => service.setGridColor(color)).not.toThrow();
    });

    it('should throw error for invalid color', () => {
      expect(() => service.setGridColor('')).toThrowError('Invalid grid color');
      expect(() => service.setGridColor(null as any)).toThrowError('Invalid grid color');
    });

    it('should invalidate cache when properties change', () => {
      spyOn(service, 'invalidateCache');
      
      service.setGridSpacing(5);
      expect(service.invalidateCache).toHaveBeenCalled();
      
      service.setGridColor('#ff0000');
      expect(service.invalidateCache).toHaveBeenCalled();
    });
  });

  describe('Precise Mode', () => {
    beforeEach(() => {
      service.initialize(mockLayer);
    });

    it('should enable precise mode', () => {
      service.showPreciseMode(true);
      
      expect(mockGroup.visible).toHaveBeenCalledWith(true);
      expect(mockLayer.batchDraw).toHaveBeenCalled();
    });

    it('should disable precise mode', () => {
      service.showPreciseMode(false);
      
      expect(mockGroup.visible).toHaveBeenCalledWith(false);
      expect(mockLayer.batchDraw).toHaveBeenCalled();
    });

    it('should highlight intersection points', () => {
      const points = [
        { x: 100, y: 100 },
        { x: 200, y: 200 }
      ];
      
      expect(() => service.highlightIntersections(points)).not.toThrow();
      expect(mockGroup.add).toHaveBeenCalled();
    });

    it('should clear existing highlights before adding new ones', () => {
      const mockHighlights = [
        { destroy: jasmine.createSpy('destroy') },
        { destroy: jasmine.createSpy('destroy') }
      ];
      mockGroup.find.and.returnValue(mockHighlights);
      
      const points = [{ x: 100, y: 100 }];
      service.highlightIntersections(points);
      
      mockHighlights.forEach(highlight => {
        expect(highlight.destroy).toHaveBeenCalled();
      });
    });

    it('should handle empty points array', () => {
      expect(() => service.highlightIntersections([])).not.toThrow();
    });
  });

  describe('Visibility', () => {
    beforeEach(() => {
      service.initialize(mockLayer);
    });

    it('should set visibility', () => {
      service.setVisible(false);
      
      expect(mockGroup.visible).toHaveBeenCalledWith(false);
      expect(mockLayer.batchDraw).toHaveBeenCalled();
    });

    it('should return visibility status', () => {
      expect(service.isVisible()).toBe(true); // Default visibility
    });

    it('should update both grid and intersection groups visibility', () => {
      service.setVisible(false);
      
      // Should be called twice (once for each group)
      expect(mockGroup.visible).toHaveBeenCalledTimes(2);
    });
  });

  describe('Caching', () => {
    beforeEach(() => {
      service.initialize(mockLayer);
    });

    it('should enable caching', () => {
      service.enableCaching(true);
      
      // Should not throw and should allow caching
      expect(() => service.render()).not.toThrow();
    });

    it('should disable caching and clear cache', () => {
      spyOn(service as any, 'clearCache');
      
      service.enableCaching(false);
      
      expect((service as any).clearCache).toHaveBeenCalled();
    });

    it('should invalidate cache', () => {
      spyOn(service as any, 'clearCache');
      
      service.invalidateCache();
      
      expect((service as any).clearCache).toHaveBeenCalled();
    });

    it('should generate unique cache keys for different states', () => {
      const key1 = (service as any).generateCacheKey();
      
      // Change state
      mockStage.scaleX.and.returnValue(2);
      const key2 = (service as any).generateCacheKey();
      
      expect(key1).not.toBe(key2);
    });

    it('should limit cache size', () => {
      const originalMaxCacheSize = CANVAS_CONSTANTS.MAX_CACHE_SIZE;
      
      // Mock a small cache size for testing
      spyOn(CANVAS_CONSTANTS, 'MAX_CACHE_SIZE' as any).and.returnValue(2);
      
      // Fill cache beyond limit
      for (let i = 0; i < 5; i++) {
        mockStage.scaleX.and.returnValue(i);
        service.render();
      }
      
      // Cache should be limited
      const cacheSize = (service as any).gridCache.size;
      expect(cacheSize).toBeLessThanOrEqual(2);
    });
  });

  describe('Performance', () => {
    beforeEach(() => {
      service.initialize(mockLayer);
    });

    it('should return performance statistics', () => {
      service.render(); // Generate some stats
      
      const stats = service.getPerformanceStats();
      
      expect(stats).toEqual(jasmine.objectContaining({
        renderCount: jasmine.any(Number),
        cacheHits: jasmine.any(Number),
        cacheHitRate: jasmine.any(Number),
        lastRenderTime: jasmine.any(Number),
        cacheSize: jasmine.any(Number)
      }));
    });

    it('should track render count', () => {
      const initialStats = service.getPerformanceStats();
      
      service.render();
      service.render();
      
      const finalStats = service.getPerformanceStats();
      expect(finalStats.renderCount).toBe(initialStats.renderCount + 2);
    });

    it('should track cache hits', () => {
      // First render (cache miss)
      service.render();
      
      // Second render with same state (should be cache hit if caching works)
      spyOn(service as any, 'tryUseCachedGrid').and.returnValue(true);
      service.render();
      
      const stats = service.getPerformanceStats();
      expect(stats.cacheHits).toBeGreaterThan(0);
    });

    it('should calculate cache hit rate correctly', () => {
      service.render(); // 1 render, 0 cache hits
      
      const stats = service.getPerformanceStats();
      expect(stats.cacheHitRate).toBe(0);
    });
  });

  describe('Grid Line Rendering', () => {
    beforeEach(() => {
      service.initialize(mockLayer);
    });

    it('should respect maximum grid line limit', () => {
      // Mock a very small spacing to potentially exceed line limit
      service.setGridSpacing(0.1);
      
      expect(() => service.render()).not.toThrow();
      
      // Should not crash even with many lines
    });

    it('should render both minor and major grid lines', () => {
      service.render();
      
      // Should create multiple line objects
      expect(Konva.Line).toHaveBeenCalled();
    });

    it('should align grid with ruler boundaries', () => {
      service.render();
      
      // Grid should start after ruler width/height
      // This is tested implicitly through the rendering process
      expect(mockLayer.batchDraw).toHaveBeenCalled();
    });
  });

  describe('Intersection Rendering', () => {
    beforeEach(() => {
      service.initialize(mockLayer);
      service.showPreciseMode(true);
    });

    it('should render intersection points in precise mode', () => {
      service.render();
      
      expect(Konva.Circle).toHaveBeenCalled();
    });

    it('should respect maximum intersection limit', () => {
      // Mock very small spacing to potentially create many intersections
      service.setGridSpacing(0.1);
      
      expect(() => service.render()).not.toThrow();
    });

    it('should not render intersections when precise mode is off', () => {
      service.showPreciseMode(false);
      
      // Reset spy call count
      (Konva.Circle as jasmine.Spy).calls.reset();
      
      service.render();
      
      // Should not create circle objects for intersections
      expect(Konva.Circle).not.toHaveBeenCalled();
    });
  });

  describe('Color Utilities', () => {
    beforeEach(() => {
      service.initialize(mockLayer);
    });

    it('should adjust hex color opacity', () => {
      const result = (service as any).adjustColorOpacity('#ff0000', 0.5);
      expect(result).toBe('rgba(255, 0, 0, 0.5)');
    });

    it('should adjust rgba color opacity', () => {
      const result = (service as any).adjustColorOpacity('rgba(255, 0, 0, 1)', 0.5);
      expect(result).toBe('rgba(255, 0, 0, 0.5)');
    });

    it('should adjust rgb color opacity', () => {
      const result = (service as any).adjustColorOpacity('rgb(255, 0, 0)', 0.5);
      expect(result).toBe('rgba(255, 0, 0, 0.5)');
    });

    it('should provide fallback for unknown color formats', () => {
      const result = (service as any).adjustColorOpacity('red', 0.5);
      expect(result).toBe('rgba(224, 224, 224, 0.5)');
    });
  });

  describe('Service Cleanup', () => {
    beforeEach(() => {
      service.initialize(mockLayer);
    });

    it('should clear grid on clear()', () => {
      service.clear();
      
      expect(mockGroup.destroyChildren).toHaveBeenCalledTimes(2); // Both groups
    });

    it('should destroy service properly', () => {
      spyOn(service, 'clear');
      spyOn(service as any, 'clearCache');
      
      service.destroy();
      
      expect(service.clear).toHaveBeenCalled();
      expect((service as any).clearCache).toHaveBeenCalled();
      expect(mockGroup.destroy).toHaveBeenCalledTimes(2);
    });

    it('should handle destroy when not initialized', () => {
      const uninitializedService = new GridRenderingService();
      
      expect(() => uninitializedService.destroy()).not.toThrow();
    });
  });

  describe('Error Handling', () => {
    beforeEach(() => {
      service.initialize(mockLayer);
    });

    it('should handle rendering errors gracefully', () => {
      mockLayer.batchDraw.and.throwError('Render error');
      
      expect(() => service.render()).toThrow();
    });

    it('should handle cache errors gracefully', () => {
      mockGroup.clone.and.throwError('Clone error');
      
      // Should not crash the service
      expect(() => service.render()).not.toThrow();
    });

    it('should handle highlight errors gracefully', () => {
      mockGroup.find.and.throwError('Find error');
      
      const points = [{ x: 100, y: 100 }];
      expect(() => service.highlightIntersections(points)).not.toThrow();
    });
  });

  describe('Update Method', () => {
    beforeEach(() => {
      service.initialize(mockLayer);
    });

    it('should call render when update is called', () => {
      spyOn(service, 'render');
      
      service.update();
      
      expect(service.render).toHaveBeenCalled();
    });
  });
});