import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DrillPointCanvasComponent } from './drill-point-canvas.component';
import { DrillPoint, PatternSettings, CanvasPosition } from '../../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../../constants/canvas.constants';
import Konva from 'konva';

describe('DrillPointCanvasComponent', () => {
  let component: DrillPointCanvasComponent;
  let fixture: ComponentFixture<DrillPointCanvasComponent>;
  let mockLayer: jasmine.SpyObj<Konva.Layer>;
  let mockStage: jasmine.SpyObj<Konva.Stage>;

  const mockSettings: PatternSettings = { spacing: 3,
    burden: 2.5,
    depth: 10, diameter: 115, stemming: 3, subDrill: 0.5 };

  const mockCanvasState = {
    scale: 1,
    offsetX: 0,
    offsetY: 0,
    panOffsetX: 0,
    panOffsetY: 0,
    width: 800,
    height: 600
  };

  const mockDrillPoints: DrillPoint[] = [
    { id: 'point-1', x: 5, y: 5, depth: 10, spacing: 3, burden: 2.5, stemming: 3, subDrill: 0.5 },
    { id: 'point-2', x: 8, y: 5, depth: 12, spacing: 3, burden: 2.5, stemming: 3, subDrill: 0.5 }, // Custom depth
    { id: 'point-3', x: 5, y: 7.5, depth: 10, spacing: 3, burden: 2.5, stemming: 3, subDrill: 0.5 }
  ];

  beforeEach(async () => {
    // Create mock Konva objects
    mockLayer = jasmine.createSpyObj('Layer', [
      'add', 'batchDraw', 'getRelativePointerPosition', 'on', 'off'
    ]);
    
    mockStage = jasmine.createSpyObj('Stage', ['container']);

    await TestBed.configureTestingModule({
      imports: [DrillPointCanvasComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(DrillPointCanvasComponent);
    component = fixture.componentInstance;

    // Set up component inputs
    component.layer = mockLayer;
    component.settings = mockSettings;
    component.canvasState = mockCanvasState;
    component.drillPoints = [];
    component.selectedPoint = null;
    component.isHolePlacementMode = false;
    component.isPreciseMode = false;
  });

  afterEach(() => {
    // Clean up any remaining Konva objects
    if (component) {
      component.ngOnDestroy();
    }
  });

  describe('Component Initialization', () => {
    it('should create', () => {
      expect(component).toBeTruthy();
    });

    it('should initialize with default values', () => {
      expect(component.drillPoints).toEqual([]);
      expect(component.selectedPoint).toBeNull();
      expect(component.isHolePlacementMode).toBeFalse();
      expect(component.isPreciseMode).toBeFalse();
    });

    it('should initialize points group on ngOnInit', () => {
      spyOn(component as any, 'initializePointsGroup');
      
      component.ngOnInit();
      
      expect((component as any).initializePointsGroup).toHaveBeenCalled();
    });

    it('should destroy point objects on ngOnDestroy', () => {
      spyOn(component as any, 'destroyPointObjects');
      
      component.ngOnDestroy();
      
      expect((component as any).destroyPointObjects).toHaveBeenCalled();
    });
  });

  describe('Point Rendering', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should render points when renderPoints is called', () => {
      component.drillPoints = mockDrillPoints;
      spyOn(component as any, 'renderAllPoints');
      spyOn(component as any, 'renderPointHighlights');
      spyOn(component as any, 'renderDepthIndicators');
      
      component.renderPoints();
      
      expect((component as any).renderAllPoints).toHaveBeenCalled();
      expect((component as any).renderPointHighlights).toHaveBeenCalled();
      expect((component as any).renderDepthIndicators).toHaveBeenCalled();
      expect(mockLayer.batchDraw).toHaveBeenCalled();
    });

    it('should warn when missing required inputs', () => {
      spyOn(console, 'warn');
      component.layer = null as any;
      
      component.renderPoints();
      
      expect(console.warn).toHaveBeenCalledWith(
        jasmine.stringContaining('Missing required inputs for point rendering')
      );
    });

    it('should update points when input changes', () => {
      spyOn(component, 'updatePoints');
      
      component.drillPoints = mockDrillPoints;
      component.ngOnChanges({
        drillPoints: {
          currentValue: mockDrillPoints,
          previousValue: [],
          firstChange: false,
          isFirstChange: () => false
        }
      });
      
      expect(component.updatePoints).toHaveBeenCalled();
    });

    it('should clear points when clearPoints is called', () => {
      spyOn(component as any, 'clearPointObjects');
      
      component.clearPoints();
      
      expect((component as any).clearPointObjects).toHaveBeenCalled();
      expect(mockLayer.batchDraw).toHaveBeenCalled();
    });
  });

  describe('Point Placement', () => {
    beforeEach(() => {
      component.ngOnInit();
      component.isHolePlacementMode = true;
    });

    it('should add point at valid position', () => {
      const position: CanvasPosition = { x: 100, y: 100 };
      spyOn(component as any, 'validatePointPlacement').and.returnValue({ isValid: true });
      spyOn(component.pointPlaced, 'emit');
      
      const result = component.addPointAtPosition(position);
      
      expect(result).toBeTruthy();
      expect(result?.depth).toBe(mockSettings.depth);
      expect(component.pointPlaced.emit).toHaveBeenCalledWith(position);
    });

    it('should not add point at invalid position', () => {
      const position: CanvasPosition = { x: 100, y: 100 };
      spyOn(component as any, 'validatePointPlacement').and.returnValue({ 
        isValid: false, 
        message: 'Invalid position' 
      });
      spyOn(component.pointPlaced, 'emit');
      
      const result = component.addPointAtPosition(position);
      
      expect(result).toBeNull();
      expect(component.pointPlaced.emit).not.toHaveBeenCalled();
    });

    it('should emit duplicate detected when point is too close', () => {
      const position: CanvasPosition = { x: 100, y: 100 };
      const existingPoint = mockDrillPoints[0];
      spyOn(component as any, 'validatePointPlacement').and.returnValue({ 
        isValid: false, 
        isDuplicate: true,
        existingPoint: existingPoint,
        message: 'Duplicate detected'
      });
      spyOn(component.duplicateDetected, 'emit');
      
      component.addPointAtPosition(position);
      
      expect(component.duplicateDetected.emit).toHaveBeenCalledWith({
        message: 'Duplicate detected',
        existingPoint: existingPoint
      });
    });
  });

  describe('Point Selection', () => {
    beforeEach(() => {
      component.ngOnInit();
      component.drillPoints = mockDrillPoints;
    });

    it('should select point by ID', () => {
      spyOn(component.pointSelected, 'emit');
      
      component.selectPoint('point-1');
      
      expect(component.pointSelected.emit).toHaveBeenCalledWith(mockDrillPoints[0]);
    });

    it('should deselect when null ID provided', () => {
      spyOn(component.pointSelected, 'emit');
      
      component.selectPoint(null);
      
      expect(component.pointSelected.emit).toHaveBeenCalledWith(null);
    });

    it('should emit null when point ID not found', () => {
      spyOn(component.pointSelected, 'emit');
      
      component.selectPoint('non-existent');
      
      expect(component.pointSelected.emit).toHaveBeenCalledWith(null);
    });
  });

  describe('Point Validation', () => {
    beforeEach(() => {
      component.ngOnInit();
      component.drillPoints = mockDrillPoints;
    });

    it('should validate point placement successfully for valid position', () => {
      const position: CanvasPosition = { x: 15, y: 15 }; // Far from existing points
      
      const result = (component as any).validatePointPlacement(position);
      
      expect(result.isValid).toBeTruthy();
    });

    it('should detect duplicate points', () => {
      const position: CanvasPosition = { x: 5, y: 5 }; // Same as existing point
      
      const result = (component as any).validatePointPlacement(position);
      
      expect(result.isValid).toBeFalsy();
      expect(result.isDuplicate).toBeTruthy();
      expect(result.existingPoint).toBeTruthy();
    });

    it('should detect points too close together', () => {
      const position: CanvasPosition = { x: 5.05, y: 5.05 }; // Very close to existing point
      
      const result = (component as any).validatePointPlacement(position);
      
      expect(result.isValid).toBeFalsy();
    });

    it('should exclude specified point from validation', () => {
      const position: CanvasPosition = { x: 5, y: 5 }; // Same as point-1
      
      const result = (component as any).validatePointPlacement(position, 'point-1');
      
      expect(result.isValid).toBeTruthy(); // Should be valid since we exclude point-1
    });
  });

  describe('Coordinate Conversion', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should convert world coordinates to canvas coordinates', () => {
      const worldPos: CanvasPosition = { x: 5, y: 5 };
      
      const canvasPos = (component as any).worldToCanvasCoordinates(worldPos);
      
      const expectedX = CANVAS_CONSTANTS.RULER_WIDTH + (5 * CANVAS_CONSTANTS.GRID_SIZE * 1);
      const expectedY = CANVAS_CONSTANTS.RULER_HEIGHT + (5 * CANVAS_CONSTANTS.GRID_SIZE * 1);
      
      expect(canvasPos.x).toBe(expectedX);
      expect(canvasPos.y).toBe(expectedY);
    });

    it('should convert canvas coordinates to world coordinates', () => {
      const canvasPos: CanvasPosition = { 
        x: CANVAS_CONSTANTS.RULER_WIDTH + (5 * CANVAS_CONSTANTS.GRID_SIZE), 
        y: CANVAS_CONSTANTS.RULER_HEIGHT + (5 * CANVAS_CONSTANTS.GRID_SIZE) 
      };
      
      const worldPos = (component as any).canvasToWorldCoordinates(canvasPos);
      
      expect(worldPos.x).toBe(5);
      expect(worldPos.y).toBe(5);
    });

    it('should handle scale in coordinate conversion', () => {
      component.canvasState = { ...mockCanvasState, scale: 2 };
      const worldPos: CanvasPosition = { x: 5, y: 5 };
      
      const canvasPos = (component as any).worldToCanvasCoordinates(worldPos);
      const backToWorld = (component as any).canvasToWorldCoordinates(canvasPos);
      
      expect(backToWorld.x).toBeCloseTo(5, 2);
      expect(backToWorld.y).toBeCloseTo(5, 2);
    });
  });

  describe('Performance Tracking', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should track performance statistics', () => {
      component.drillPoints = mockDrillPoints;
      
      component.renderPoints();
      
      const stats = component.getPerformanceStats();
      expect(stats.lastRenderTime).toBeGreaterThan(0);
      expect(stats.renderCount).toBe(1);
      expect(stats.pointCount).toBe(mockDrillPoints.length);
    });

    it('should increment render count on multiple renders', () => {
      component.renderPoints();
      component.renderPoints();
      
      const stats = component.getPerformanceStats();
      expect(stats.renderCount).toBe(2);
    });
  });

  describe('Event Handling', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should handle point click events', () => {
      const point = mockDrillPoints[0];
      spyOn(component, 'selectPoint');
      
      (component as any).handlePointClick(point);
      
      expect(component.selectPoint).toHaveBeenCalledWith(point.id);
    });

    it('should handle point mouse enter events', () => {
      const point = mockDrillPoints[0];
      spyOn(component as any, 'renderPointHighlights');
      
      (component as any).handlePointMouseEnter(point);
      
      expect((component as any).hoveredPoint).toBe(point);
      expect(document.body.style.cursor).toBe('pointer');
      expect((component as any).renderPointHighlights).toHaveBeenCalled();
    });

    it('should handle point mouse leave events', () => {
      (component as any).hoveredPoint = mockDrillPoints[0];
      
      (component as any).handlePointMouseLeave();
      
      expect((component as any).hoveredPoint).toBeNull();
      expect(document.body.style.cursor).toBe('default');
    });

    it('should handle drag start events', () => {
      const point = mockDrillPoints[0];
      const mockEvent = {
        target: { x: () => 100, y: () => 100 }
      };
      spyOn(component, 'selectPoint');
      
      (component as any).handlePointDragStart(point, mockEvent);
      
      expect((component as any).isDragging).toBeTruthy();
      expect((component as any).dragStartPosition).toEqual({ x: 100, y: 100 });
      expect(component.selectPoint).toHaveBeenCalledWith(point.id);
      expect(document.body.style.cursor).toBe('grabbing');
    });

    it('should handle drag end events', () => {
      const point = mockDrillPoints[0];
      const mockEvent = {
        target: { x: () => 150, y: () => 150 }
      };
      spyOn(component.pointMoved, 'emit');
      spyOn(component as any, 'canvasToWorldCoordinates').and.returnValue({ x: 7, y: 7 });
      
      (component as any).handlePointDragEnd(point, mockEvent);
      
      expect((component as any).isDragging).toBeFalsy();
      expect(document.body.style.cursor).toBe('pointer');
      expect(component.pointMoved.emit).toHaveBeenCalledWith({
        point: point,
        newPosition: { x: 7, y: 7 }
      });
    });
  });

  describe('Depth Indicators', () => {
    beforeEach(() => {
      component.ngOnInit();
      component.drillPoints = mockDrillPoints;
    });

    it('should render depth indicators for custom depth points', () => {
      spyOn(component as any, 'renderDepthIndicator');
      
      (component as any).renderDepthIndicators();
      
      // Should only render indicator for point-2 which has custom depth (12 vs default 10)
      expect((component as any).renderDepthIndicator).toHaveBeenCalledTimes(1);
      expect((component as any).renderDepthIndicator).toHaveBeenCalledWith(mockDrillPoints[1]);
    });

    it('should not render depth indicators for global depth points', () => {
      component.drillPoints = [mockDrillPoints[0], mockDrillPoints[2]]; // Only global depth points
      spyOn(component as any, 'renderDepthIndicator');
      
      (component as any).renderDepthIndicators();
      
      expect((component as any).renderDepthIndicator).not.toHaveBeenCalled();
    });
  });

  describe('Error Handling', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should handle rendering errors gracefully', () => {
      spyOn(console, 'error');
      spyOn(component as any, 'renderAllPoints').and.throwError('Render error');
      
      component.renderPoints();
      
      expect(console.error).toHaveBeenCalledWith(
        jasmine.stringContaining('Error rendering drill points:'),
        jasmine.any(Error)
      );
    });

    it('should generate unique point IDs', () => {
      const id1 = (component as any).generatePointId();
      const id2 = (component as any).generatePointId();
      
      expect(id1).not.toBe(id2);
      expect(id1).toContain('point-');
      expect(id2).toContain('point-');
    });
  });

  describe('Memory Management', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should clean up objects when clearing points', () => {
      const mockPointsGroup = jasmine.createSpyObj('Group', ['destroyChildren']);
      const mockHighlightGroup = jasmine.createSpyObj('Group', ['destroyChildren']);
      const mockDepthGroup = jasmine.createSpyObj('Group', ['destroyChildren']);
      
      (component as any).pointsGroup = mockPointsGroup;
      (component as any).highlightGroup = mockHighlightGroup;
      (component as any).depthIndicatorGroup = mockDepthGroup;
      
      (component as any).clearPointObjects();
      
      expect(mockPointsGroup.destroyChildren).toHaveBeenCalled();
      expect(mockHighlightGroup.destroyChildren).toHaveBeenCalled();
      expect(mockDepthGroup.destroyChildren).toHaveBeenCalled();
    });

    it('should destroy all objects on component destroy', () => {
      const mockPointsGroup = jasmine.createSpyObj('Group', ['destroy']);
      const mockHighlightGroup = jasmine.createSpyObj('Group', ['destroy']);
      const mockDepthGroup = jasmine.createSpyObj('Group', ['destroy']);
      
      (component as any).pointsGroup = mockPointsGroup;
      (component as any).highlightGroup = mockHighlightGroup;
      (component as any).depthIndicatorGroup = mockDepthGroup;
      
      (component as any).destroyPointObjects();
      
      expect(mockPointsGroup.destroy).toHaveBeenCalled();
      expect(mockHighlightGroup.destroy).toHaveBeenCalled();
      expect(mockDepthGroup.destroy).toHaveBeenCalled();
      expect((component as any).pointsGroup).toBeNull();
      expect((component as any).highlightGroup).toBeNull();
      expect((component as any).depthIndicatorGroup).toBeNull();
    });
  });
});