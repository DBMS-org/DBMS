import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DrillPointCanvasComponent } from './drill-point-canvas.component';
import { DrillPoint, PatternSettings } from '../../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../../constants/canvas.constants';
import Konva from 'konva';

/**
 * Demo tests showing how to use DrillPointCanvasComponent in real scenarios.
 * These tests demonstrate the component's capabilities and serve as usage examples.
 */
describe('DrillPointCanvasComponent - Demo Usage', () => {
  let component: DrillPointCanvasComponent;
  let fixture: ComponentFixture<DrillPointCanvasComponent>;
  let mockLayer: Konva.Layer;
  let mockStage: Konva.Stage;

  const demoSettings: PatternSettings = { spacing: 4,
    burden: 3,
    depth: 12, diameter: 115, stemming: 3, subDrill: 0.5 };

  const demoCanvasState = {
    scale: 1.5,
    offsetX: 50,
    offsetY: 50,
    panOffsetX: 0,
    panOffsetY: 0,
    width: 1000,
    height: 800
  };

  beforeEach(async () => {
    // Create real Konva objects for demo
    const container = document.createElement('div');
    document.body.appendChild(container);
    
    mockStage = new Konva.Stage({
      container: container,
      width: 1000,
      height: 800
    });
    
    mockLayer = new Konva.Layer();
    mockStage.add(mockLayer);

    await TestBed.configureTestingModule({
      imports: [DrillPointCanvasComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(DrillPointCanvasComponent);
    component = fixture.componentInstance;

    // Set up component with demo data
    component.layer = mockLayer;
    component.settings = demoSettings;
    component.canvasState = demoCanvasState;
    component.drillPoints = [];
    component.selectedPoint = null;
    component.isHolePlacementMode = true;
    component.isPreciseMode = false;
  });

  afterEach(() => {
    mockStage.destroy();
    document.body.removeChild(mockStage.container());
  });

  describe('Demo: Creating a Basic Drilling Pattern', () => {
    it('should create a 3x3 grid pattern', async () => {
      component.ngOnInit();
      
      const gridPoints: DrillPoint[] = [];
      let pointId = 1;

      // Create 3x3 grid pattern
      for (let row = 0; row < 3; row++) {
        for (let col = 0; col < 3; col++) {
          const point: DrillPoint = {
            id: `demo-point-${pointId++}`,
            x: 5 + (col * demoSettings.spacing),
            y: 5 + (row * demoSettings.burden),
            depth: demoSettings.depth,
            spacing: demoSettings.spacing,
            burden: demoSettings.burden
          };
          gridPoints.push(point);
        }
      }

      component.drillPoints = gridPoints;
      component.renderPoints();

      // Verify all points are rendered
      expect(component.drillPoints.length).toBe(9);
      
      const stats = component.getPerformanceStats();
      expect(stats.pointCount).toBe(9);
      expect(stats.lastRenderTime).toBeGreaterThan(0);
      
      console.log('Demo: Created 3x3 grid pattern with performance:', stats);
    });

    it('should demonstrate custom depth points with indicators', async () => {
      component.ngOnInit();
      
      const mixedDepthPoints: DrillPoint[] = [
        { id: 'standard-1', x: 5, y: 5, depth: 12, spacing: 4, burden: 3, stemming: 3, subDrill: 0.5 },
        { id: 'shallow-1', x: 9, y: 5, depth: 8, spacing: 4, burden: 3, stemming: 3, subDrill: 0.5 }, // Custom depth
        { id: 'deep-1', x: 13, y: 5, depth: 15, spacing: 4, burden: 3, stemming: 3, subDrill: 0.5 }, // Custom depth
        { id: 'standard-2', x: 5, y: 8, depth: 12, spacing: 4, burden: 3, stemming: 3, subDrill: 0.5 },
        { id: 'custom-1', x: 9, y: 8, depth: 10, spacing: 4, burden: 3, stemming: 3, subDrill: 0.5 } // Custom depth
      ];

      component.drillPoints = mixedDepthPoints;
      component.renderPoints();

      // Count points with custom depths (different from settings.depth)
      const customDepthPoints = mixedDepthPoints.filter(p => p.depth !== demoSettings.depth);
      expect(customDepthPoints.length).toBe(3);

      console.log('Demo: Created pattern with mixed depths:', {
        totalPoints: mixedDepthPoints.length,
        customDepthPoints: customDepthPoints.length,
        standardDepth: demoSettings.depth
      });
    });
  });

  describe('Demo: Interactive Point Operations', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should demonstrate point placement workflow', async () => {
      let placedPoints: DrillPoint[] = [];
      let duplicateMessages: string[] = [];

      // Subscribe to events
      component.pointPlaced.subscribe(() => {
        console.log('Demo: Point placed successfully');
      });

      component.duplicateDetected.subscribe((event) => {
        duplicateMessages.push(event.message);
        console.log('Demo: Duplicate detected:', event.message);
      });

      // Simulate placing points
      const positions = [
        { x: 100, y: 100 },
        { x: 200, y: 100 },
        { x: 100, y: 100 }, // Duplicate - should be rejected
        { x: 300, y: 100 }
      ];

      for (const pos of positions) {
        const newPoint = component.addPointAtPosition(pos);
        if (newPoint) {
          placedPoints.push(newPoint);
          component.drillPoints = [...component.drillPoints, newPoint];
        }
      }

      expect(placedPoints.length).toBe(3); // 4 attempts, 1 duplicate rejected
      expect(duplicateMessages.length).toBe(1);
      
      console.log('Demo: Point placement results:', {
        attempted: positions.length,
        successful: placedPoints.length,
        duplicates: duplicateMessages.length
      });
    });

    it('should demonstrate point selection and highlighting', async () => {
      const testPoints: DrillPoint[] = [
        { id: 'select-1', x: 5, y: 5, depth: 12, spacing: 4, burden: 3, stemming: 3, subDrill: 0.5 },
        { id: 'select-2', x: 9, y: 5, depth: 12, spacing: 4, burden: 3, stemming: 3, subDrill: 0.5 },
        { id: 'select-3', x: 13, y: 5, depth: 12, spacing: 4, burden: 3, stemming: 3, subDrill: 0.5 }
      ];

      component.drillPoints = testPoints;
      component.renderPoints();

      let selectedPoints: (DrillPoint | null)[] = [];
      component.pointSelected.subscribe(point => {
        selectedPoints.push(point);
        console.log('Demo: Point selected:', point?.id || 'none');
      });

      // Test selection workflow
      component.selectPoint('select-1');
      component.selectPoint('select-3');
      component.selectPoint(null); // Deselect
      component.selectPoint('non-existent'); // Should select null

      expect(selectedPoints.length).toBe(4);
      expect(selectedPoints[0]?.id).toBe('select-1');
      expect(selectedPoints[1]?.id).toBe('select-3');
      expect(selectedPoints[2]).toBeNull();
      expect(selectedPoints[3]).toBeNull();

      console.log('Demo: Selection workflow completed');
    });
  });

  describe('Demo: Performance with Large Datasets', () => {
    it('should handle large drilling patterns efficiently', async () => {
      component.ngOnInit();
      
      // Create a large pattern (20x20 = 400 points)
      const largePattern: DrillPoint[] = [];
      let pointId = 1;

      const startTime = performance.now();

      for (let row = 0; row < 20; row++) {
        for (let col = 0; col < 20; col++) {
          const point: DrillPoint = {
            id: `large-${pointId++}`,
            x: col * 2,
            y: row * 2,
            depth: Math.random() > 0.8 ? 15 : 12, // 20% custom depth
            spacing: 2,
            burden: 2
          };
          largePattern.push(point);
        }
      }

      const creationTime = performance.now() - startTime;

      component.drillPoints = largePattern;
      component.renderPoints();

      const stats = component.getPerformanceStats();
      
      expect(largePattern.length).toBe(400);
      expect(stats.pointCount).toBe(400);
      expect(stats.lastRenderTime).toBeLessThan(100); // Should render in under 100ms

      console.log('Demo: Large pattern performance:', {
        pointCount: largePattern.length,
        creationTime: `${creationTime.toFixed(2)}ms`,
        renderTime: `${stats.lastRenderTime.toFixed(2)}ms`,
        customDepthPoints: largePattern.filter(p => p.depth !== 12).length
      });
    });

    it('should demonstrate memory management with pattern updates', async () => {
      component.ngOnInit();
      
      const iterations = 10;
      const pointsPerIteration = 50;
      
      for (let i = 0; i < iterations; i++) {
        const pattern: DrillPoint[] = [];
        
        for (let j = 0; j < pointsPerIteration; j++) {
          pattern.push({
            id: `iter-${i}-point-${j}`,
            x: Math.random() * 50,
            y: Math.random() * 50,
            depth: 12,
            spacing: 3,
            burden: 2.5
          });
        }
        
        component.drillPoints = pattern;
        component.renderPoints();
        
        // Clear and re-render to test memory management
        component.clearPoints();
      }

      const finalStats = component.getPerformanceStats();
      expect(finalStats.renderCount).toBe(iterations);
      
      console.log('Demo: Memory management test completed:', {
        iterations,
        pointsPerIteration,
        totalRenders: finalStats.renderCount
      });
    });
  });

  describe('Demo: Error Handling and Edge Cases', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should demonstrate validation error handling', async () => {
      const existingPoints: DrillPoint[] = [
        { id: 'existing-1', x: 10, y: 10, depth: 12, spacing: 4, burden: 3, stemming: 3, subDrill: 0.5 }
      ];

      component.drillPoints = existingPoints;
      
      let duplicateEvents: any[] = [];
      component.duplicateDetected.subscribe(event => {
        duplicateEvents.push(event);
      });

      // Try to place points too close to existing ones
      const invalidPositions = [
        { x: 10.005, y: 10.005 }, // Too close (within duplicate tolerance)
        { x: 10.05, y: 10.05 },   // Too close (within minimum distance)
        { x: -50, y: -50 },       // Outside canvas bounds
      ];

      const results = invalidPositions.map(pos => component.addPointAtPosition(pos));
      
      expect(results.every(r => r === null)).toBeTruthy();
      expect(duplicateEvents.length).toBeGreaterThan(0);

      console.log('Demo: Validation error handling:', {
        invalidAttempts: invalidPositions.length,
        rejectedPoints: results.filter(r => r === null).length,
        duplicateEvents: duplicateEvents.length
      });
    });

    it('should demonstrate coordinate conversion accuracy', async () => {
      const testCases = [
        { world: { x: 0, y: 0 } },
        { world: { x: 10, y: 10 } },
        { world: { x: -5, y: -5 } },
        { world: { x: 25.5, y: 15.7 } }
      ];

      const results = testCases.map(testCase => {
        const canvas = (component as any).worldToCanvasCoordinates(testCase.world);
        const backToWorld = (component as any).canvasToWorldCoordinates(canvas);
        
        return {
          original: testCase.world,
          canvas,
          converted: backToWorld,
          accuracy: {
            x: Math.abs(testCase.world.x - backToWorld.x),
            y: Math.abs(testCase.world.y - backToWorld.y)
          }
        };
      });

      // Verify conversion accuracy (should be within 0.001 units)
      results.forEach(result => {
        expect(result.accuracy.x).toBeLessThan(0.001);
        expect(result.accuracy.y).toBeLessThan(0.001);
      });

      console.log('Demo: Coordinate conversion accuracy test:', results);
    });
  });

  describe('Demo: Integration with Canvas State', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should demonstrate zoom and pan effects on point rendering', async () => {
      const basePoints: DrillPoint[] = [
        { id: 'zoom-1', x: 5, y: 5, depth: 12, spacing: 4, burden: 3, stemming: 3, subDrill: 0.5 },
        { id: 'zoom-2', x: 10, y: 5, depth: 12, spacing: 4, burden: 3, stemming: 3, subDrill: 0.5 }
      ];

      component.drillPoints = basePoints;

      // Test different zoom levels
      const zoomLevels = [0.5, 1.0, 2.0, 4.0];
      const renderTimes: number[] = [];

      for (const scale of zoomLevels) {
        component.canvasState = { ...demoCanvasState, scale };
        
        const startTime = performance.now();
        component.renderPoints();
        const renderTime = performance.now() - startTime;
        
        renderTimes.push(renderTime);
      }

      // Verify rendering works at all zoom levels
      expect(renderTimes.every(time => time > 0)).toBeTruthy();

      console.log('Demo: Zoom level performance:', {
        zoomLevels,
        renderTimes: renderTimes.map(t => `${t.toFixed(2)}ms`),
        averageTime: `${(renderTimes.reduce((a, b) => a + b) / renderTimes.length).toFixed(2)}ms`
      });
    });

    it('should demonstrate pan offset effects', async () => {
      const testPoint: DrillPoint = {
        id: 'pan-test',
        x: 10,
        y: 10,
        depth: 12,
        spacing: 4,
        burden: 3, stemming: 3, subDrill: 0.5
      };

      component.drillPoints = [testPoint];

      // Test different pan offsets
      const panOffsets = [
        { panOffsetX: 0, panOffsetY: 0 },
        { panOffsetX: 100, panOffsetY: 50 },
        { panOffsetX: -50, panOffsetY: -100 },
        { panOffsetX: 200, panOffsetY: 200 }
      ];

      const canvasPositions = panOffsets.map(offset => {
        component.canvasState = { ...demoCanvasState, ...offset };
        return (component as any).worldToCanvasCoordinates({ x: testPoint.x, y: testPoint.y });
      });

      // Verify that pan offsets affect canvas positions correctly
      expect(canvasPositions[1].x).toBe(canvasPositions[0].x + 100);
      expect(canvasPositions[1].y).toBe(canvasPositions[0].y + 50);

      console.log('Demo: Pan offset effects:', {
        worldPosition: { x: testPoint.x, y: testPoint.y },
        canvasPositions: canvasPositions.map((pos, i) => ({
          offset: panOffsets[i],
          canvas: pos
        }))
      });
    });
  });
});