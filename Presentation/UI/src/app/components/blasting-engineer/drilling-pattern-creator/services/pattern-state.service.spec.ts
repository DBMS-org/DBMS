import { TestBed } from '@angular/core/testing';
import { PatternStateService } from './pattern-state.service';
import { PatternState, createInitialPatternState } from '../models/pattern-state.model';
import { DrillPoint, PatternSettings } from '../models/drill-point.model';
import { take } from 'rxjs/operators';

describe('PatternStateService', () => {
  let service: PatternStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PatternStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('State Initialization', () => {
    it('should initialize with default state', (done) => {
      service.state$.pipe(take(1)).subscribe(state => {
        expect(state.drillPoints).toEqual([]);
        expect(state.settings.spacing).toBe(3);
        expect(state.settings.burden).toBe(2.5);
        expect(state.settings.depth).toBe(10);
        expect(state.selectedPoint).toBeNull();
        expect(state.canvasState.isInitialized).toBeFalse();
        expect(state.uiState.isSaved).toBeTrue();
        done();
      });
    });

    it('should initialize state with project and site IDs', (done) => {
      service.initializeState(123, 456);
      
      service.metadata$.pipe(take(1)).subscribe(metadata => {
        expect(metadata.projectId).toBe(123);
        expect(metadata.siteId).toBe(456);
        expect(metadata.version).toBe('1.0.0');
        done();
      });
    });
  });

  describe('Settings Management', () => {
    it('should update settings and mark as modified', (done) => {
      const newSettings: Partial<PatternSettings> = {
        spacing: 4,
        burden: 3
      };

      service.updateSettings(newSettings);

      service.settings$.pipe(take(1)).subscribe(settings => {
        expect(settings.spacing).toBe(4);
        expect(settings.burden).toBe(3);
        expect(settings.depth).toBe(10); // unchanged
        done();
      });
    });

    it('should mark pattern as modified when settings change', (done) => {
      service.updateSettings({ spacing: 5 });

      service.isPatternModified$.pipe(take(1)).subscribe(isModified => {
        expect(isModified).toBeTrue();
        done();
      });
    });
  });

  describe('Drill Point Management', () => {
    const mockDrillPoint: DrillPoint = {
      id: 'DH1',
      x: 10,
      y: 20,
      depth: 15,
      spacing: 3,
      burden: 2.5,
      stemming: 3,
      subDrill: 0.5
    };

    it('should add drill point', (done) => {
      service.addDrillPoint(mockDrillPoint);

      service.drillPoints$.pipe(take(1)).subscribe(points => {
        expect(points.length).toBe(1);
        expect(points[0]).toEqual(mockDrillPoint);
        done();
      });
    });

    it('should update drill point count observable', (done) => {
      service.addDrillPoint(mockDrillPoint);

      service.drillPointsCount$.pipe(take(1)).subscribe(count => {
        expect(count).toBe(1);
        done();
      });
    });

    it('should update existing drill point', (done) => {
      service.addDrillPoint(mockDrillPoint);
      service.updateDrillPoint('DH1', { depth: 25 });

      service.drillPoints$.pipe(take(1)).subscribe(points => {
        expect(points[0].depth).toBe(25);
        expect(points[0].x).toBe(10); // unchanged
        done();
      });
    });

    it('should delete drill point', (done) => {
      service.addDrillPoint(mockDrillPoint);
      service.deleteDrillPoint('DH1');

      service.drillPoints$.pipe(take(1)).subscribe(points => {
        expect(points.length).toBe(0);
        done();
      });
    });

    it('should clear selected point when deleted point is selected', (done) => {
      service.addDrillPoint(mockDrillPoint);
      service.selectPoint(mockDrillPoint);
      service.deleteDrillPoint('DH1');

      service.selectedPoint$.pipe(take(1)).subscribe(selectedPoint => {
        expect(selectedPoint).toBeNull();
        done();
      });
    });

    it('should clear all drill points', (done) => {
      service.addDrillPoint(mockDrillPoint);
      service.addDrillPoint({ ...mockDrillPoint, id: 'DH2' });
      service.clearAllPoints();

      service.drillPoints$.pipe(take(1)).subscribe(points => {
        expect(points.length).toBe(0);
        done();
      });
    });
  });

  describe('Point Selection', () => {
    const mockDrillPoint: DrillPoint = {
      id: 'DH1',
      x: 10,
      y: 20,
      depth: 15,
      spacing: 3,
      burden: 2.5,
      stemming: 3,
      subDrill: 0.5
    };

    it('should select drill point', (done) => {
      service.selectPoint(mockDrillPoint);

      service.selectedPoint$.pipe(take(1)).subscribe(selectedPoint => {
        expect(selectedPoint).toEqual(mockDrillPoint);
        done();
      });
    });

    it('should deselect drill point', (done) => {
      service.selectPoint(mockDrillPoint);
      service.selectPoint(null);

      service.selectedPoint$.pipe(take(1)).subscribe(selectedPoint => {
        expect(selectedPoint).toBeNull();
        done();
      });
    });
  });

  describe('Canvas State Management', () => {
    it('should update canvas state', (done) => {
      service.updateCanvasState({ scale: 2, isInitialized: true });

      service.canvasState$.pipe(take(1)).subscribe(canvasState => {
        expect(canvasState.scale).toBe(2);
        expect(canvasState.isInitialized).toBeTrue();
        expect(canvasState.panOffsetX).toBe(0); // unchanged
        done();
      });
    });

    it('should track canvas initialization', (done) => {
      service.updateCanvasState({ isInitialized: true });

      service.canvasInitialized$.pipe(take(1)).subscribe(isInitialized => {
        expect(isInitialized).toBeTrue();
        done();
      });
    });

    it('should track current scale', (done) => {
      service.updateCanvasState({ scale: 1.5 });

      service.currentScale$.pipe(take(1)).subscribe(scale => {
        expect(scale).toBe(1.5);
        done();
      });
    });

    it('should track interaction state', (done) => {
      service.updateCanvasState({ isDragging: true });

      service.isInteracting$.pipe(take(1)).subscribe(isInteracting => {
        expect(isInteracting).toBeTrue();
        done();
      });
    });
  });

  describe('UI State Management', () => {
    it('should update UI state', (done) => {
      service.updateUIState({ isHolePlacementMode: true, isPreciseMode: true });

      service.uiState$.pipe(take(1)).subscribe(uiState => {
        expect(uiState.isHolePlacementMode).toBeTrue();
        expect(uiState.isPreciseMode).toBeTrue();
        expect(uiState.isFullscreen).toBeFalse(); // unchanged
        done();
      });
    });

    it('should mark pattern as saved', (done) => {
      service.updateUIState({ isSaved: false }); // first mark as unsaved
      service.markAsSaved();

      service.uiState$.pipe(take(1)).subscribe(uiState => {
        expect(uiState.isSaved).toBeTrue();
        done();
      });
    });

    it('should mark pattern as modified', (done) => {
      service.markAsModified();

      service.isPatternModified$.pipe(take(1)).subscribe(isModified => {
        expect(isModified).toBeTrue();
        done();
      });
    });

    it('should set and auto-clear duplicate message', (done) => {
      service.setDuplicateMessage('Duplicate point detected');

      service.uiState$.pipe(take(1)).subscribe(uiState => {
        expect(uiState.duplicateMessage).toBe('Duplicate point detected');
        
        // Test auto-clear after timeout
        setTimeout(() => {
          service.uiState$.pipe(take(1)).subscribe(updatedUiState => {
            expect(updatedUiState.duplicateMessage).toBeNull();
            done();
          });
        }, 3100); // slightly longer than the 3000ms timeout
      });
    });
  });

  describe('State Persistence', () => {
    it('should load complete pattern state', (done) => {
      const mockState = createInitialPatternState(100, 200);
      mockState.drillPoints = [{
        id: 'DH1',
        x: 5,
        y: 10,
        depth: 12,
        spacing: 3,
        burden: 2.5
      }];
      mockState.settings.spacing = 4;

      service.loadPattern(mockState);

      service.state$.pipe(take(1)).subscribe(state => {
        expect(state.drillPoints.length).toBe(1);
        expect(state.settings.spacing).toBe(4);
        expect(state.metadata.projectId).toBe(100);
        expect(state.uiState.isSaved).toBeTrue(); // should be marked as saved after load
        done();
      });
    });

    it('should reset state to initial values', (done) => {
      // First modify the state
      service.initializeState(123, 456);
      service.addDrillPoint({
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 15,
        spacing: 3,
        burden: 2.5,
        stemming: 3,
        subDrill: 0.5
      });
      service.updateSettings({ spacing: 5 });

      // Then reset
      service.resetState();

      service.state$.pipe(take(1)).subscribe(state => {
        expect(state.drillPoints.length).toBe(0);
        expect(state.settings.spacing).toBe(3); // back to default
        expect(state.metadata.projectId).toBe(123); // preserved
        expect(state.metadata.siteId).toBe(456); // preserved
        done();
      });
    });
  });

  describe('Derived Observables', () => {
    it('should get specific drill point observable', (done) => {
      const mockPoint: DrillPoint = {
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 15,
        spacing: 3,
        burden: 2.5
      };

      service.addDrillPoint(mockPoint);

      service.getDrillPoint$('DH1').pipe(take(1)).subscribe(point => {
        expect(point).toEqual(mockPoint);
        done();
      });
    });

    it('should return undefined for non-existent drill point', (done) => {
      service.getDrillPoint$('NON_EXISTENT').pipe(take(1)).subscribe(point => {
        expect(point).toBeUndefined();
        done();
      });
    });

    it('should get points with custom depths', (done) => {
      service.updateSettings({ depth: 10 });
      service.addDrillPoint({
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 10, // same as global
        spacing: 3,
        burden: 2.5
      });
      service.addDrillPoint({
        id: 'DH2',
        x: 15,
        y: 25,
        depth: 15, // different from global
        spacing: 3,
        burden: 2.5
      });

      service.getPointsWithCustomDepths$().pipe(take(1)).subscribe(customPoints => {
        expect(customPoints.length).toBe(1);
        expect(customPoints[0].id).toBe('DH2');
        done();
      });
    });

    it('should validate pattern status', (done) => {
      service.getValidationStatus$().pipe(take(1)).subscribe(validation => {
        expect(validation.isValid).toBeFalse();
        expect(validation.errors).toContain('No drill points defined');
        done();
      });
    });

    it('should validate pattern with valid data', (done) => {
      service.addDrillPoint({
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 15,
        spacing: 3,
        burden: 2.5,
        stemming: 3,
        subDrill: 0.5
      });

      service.getValidationStatus$().pipe(take(1)).subscribe(validation => {
        expect(validation.isValid).toBeTrue();
        expect(validation.errors.length).toBe(0);
        done();
      });
    });
  });

  describe('State Immutability', () => {
    it('should not mutate original state when updating', (done) => {
      const originalState = service.getCurrentState();
      const originalDrillPoints = originalState.drillPoints;

      service.addDrillPoint({
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 15,
        spacing: 3,
        burden: 2.5,
        stemming: 3,
        subDrill: 0.5
      });

      // Original state should remain unchanged
      expect(originalDrillPoints.length).toBe(0);
      
      service.drillPoints$.pipe(take(1)).subscribe(newPoints => {
        expect(newPoints.length).toBe(1);
        expect(newPoints).not.toBe(originalDrillPoints); // different reference
        done();
      });
    });
  });
});

