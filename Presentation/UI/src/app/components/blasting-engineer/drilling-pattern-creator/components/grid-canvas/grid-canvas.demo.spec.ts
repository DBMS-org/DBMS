/**
 * Demonstration test for GridCanvasComponent
 * This test shows the component working with real Konva objects
 */

import { ComponentFixture, TestBed } from '@angular/core/testing';
import Konva from 'konva';
import { GridCanvasComponent } from './grid-canvas.component';
import { PatternSettings } from '../../models/drill-point.model';

describe('GridCanvasComponent - Demo', () => {
  let component: GridCanvasComponent;
  let fixture: ComponentFixture<GridCanvasComponent>;
  let stage: Konva.Stage;
  let layer: Konva.Layer;

  const settings: PatternSettings = {
    spacing: 3,
    burden: 2.5,
    depth: 10
  };

  const canvasState = {
    scale: 1,
    offsetX: 0,
    offsetY: 0,
    panOffsetX: 0,
    panOffsetY: 0,
    width: 800,
    height: 600
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GridCanvasComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(GridCanvasComponent);
    component = fixture.componentInstance;

    // Create real Konva objects for demonstration
    const container = document.createElement('div');
    container.style.width = '800px';
    container.style.height = '600px';
    document.body.appendChild(container);

    stage = new Konva.Stage({
      container: container,
      width: 800,
      height: 600
    });

    layer = new Konva.Layer();
    stage.add(layer);

    // Set up component inputs
    component.layer = layer;
    component.settings = settings;
    component.canvasState = canvasState;
    component.isPreciseMode = false;

    component.ngOnInit();
  });

  afterEach(() => {
    if (stage) {
      stage.destroy();
    }
    // Clean up DOM
    const containers = document.querySelectorAll('div[style*="width: 800px"]');
    containers.forEach(container => container.remove());
  });

  it('should create and render grid successfully', () => {
    expect(component).toBeTruthy();
    
    // Render the grid
    expect(() => component.renderGrid()).not.toThrow();
    
    // Check that layer has children (grid lines)
    expect(layer.children.length).toBeGreaterThan(0);
  });

  it('should handle precise mode toggle', () => {
    // Initial render without precise mode
    component.renderGrid();
    const initialChildCount = layer.children.length;
    
    // Enable precise mode
    component.isPreciseMode = true;
    component.renderGrid();
    
    // Should have more children (intersection points added)
    expect(layer.children.length).toBeGreaterThanOrEqual(initialChildCount);
  });

  it('should cache grid for performance', () => {
    // First render
    component.renderGrid();
    const stats1 = component.getCacheStats();
    
    // Second render with same parameters
    component.renderGrid();
    const stats2 = component.getCacheStats();
    
    // Cache should be used (render count increases but cache size stays same)
    expect(stats2.renderCount).toBe(stats1.renderCount + 1);
    expect(stats2.gridCacheSize).toBeGreaterThan(0);
  });

  it('should handle settings changes', () => {
    // Initial render
    component.renderGrid();
    const initialStats = component.getCacheStats();
    
    // Change settings
    component.settings = { ...settings, spacing: 4 };
    component.renderGrid();
    const newStats = component.getCacheStats();
    
    // Should create new cache entry
    expect(newStats.gridCacheSize).toBeGreaterThan(initialStats.gridCacheSize);
  });

  it('should clear grid properly', () => {
    // Render grid
    component.renderGrid();
    expect(layer.children.length).toBeGreaterThan(0);
    
    // Clear grid
    component.clearGrid();
    
    // Layer should be empty or have minimal children
    expect(layer.children.length).toBeLessThanOrEqual(1);
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

  it('should handle extreme zoom levels', () => {
    // Test very high zoom
    component.canvasState = { ...canvasState, scale: 10 };
    expect(() => component.renderGrid()).not.toThrow();
    
    // Test very low zoom
    component.canvasState = { ...canvasState, scale: 0.1 };
    expect(() => component.renderGrid()).not.toThrow();
  });

  it('should handle large pan offsets', () => {
    component.canvasState = { 
      ...canvasState, 
      panOffsetX: 5000, 
      panOffsetY: -3000 
    };
    
    expect(() => component.renderGrid()).not.toThrow();
    expect(layer.children.length).toBeGreaterThan(0);
  });

  it('should demonstrate performance with multiple renders', () => {
    const startTime = performance.now();
    
    // Render multiple times with different settings
    for (let i = 0; i < 10; i++) {
      component.settings = { ...settings, spacing: 3 + i * 0.1 };
      component.renderGrid();
    }
    
    const endTime = performance.now();
    const totalTime = endTime - startTime;
    
    // Should complete reasonably quickly (less than 1 second)
    expect(totalTime).toBeLessThan(1000);
    
    const stats = component.getCacheStats();
    expect(stats.renderCount).toBe(10);
    expect(stats.gridCacheSize).toBeGreaterThan(0);
  });
});