import { TestBed } from '@angular/core/testing';
import { PatternStateService } from './pattern-state.service';
import { PatternDataStore } from './pattern-data.store';
import { DrillPoint } from '../models/drill-point.model';
import { take, combineLatest } from 'rxjs';

describe('Pattern State Management Integration', () => {
  let stateService: PatternStateService;
  let dataStore: PatternDataStore;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    stateService = TestBed.inject(PatternStateService);
    dataStore = TestBed.inject(PatternDataStore);
    
    // Clear localStorage
    localStorage.clear();
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should integrate state service with data store for save/load operations', (done) => {
    // Initialize state
    stateService.initializeState(100, 200);

    // Add some drill points
    const point1: DrillPoint = {
      id: 'DH1',
      x: 10,
      y: 20,
      depth: 15,
      spacing: 3,
      burden: 2.5
    };

    const point2: DrillPoint = {
      id: 'DH2',
      x: 15,
      y: 25,
      depth: 12,
      spacing: 3,
      burden: 2.5
    };

    stateService.addDrillPoint(point1);
    stateService.addDrillPoint(point2);
    stateService.updateSettings({ spacing: 4, burden: 3 });

    // Get current state and save it
    const currentState = stateService.getCurrentState();
    
    dataStore.savePattern(currentState).subscribe({
      next: () => {
        // Load the pattern back
        dataStore.loadPattern(100, 200).subscribe({
          next: (loadedState) => {
            // Verify the loaded state matches what we saved
            expect(loadedState.drillPoints.length).toBe(2);
            expect(loadedState.drillPoints[0].id).toBe('DH1');
            expect(loadedState.drillPoints[1].id).toBe('DH2');
            expect(loadedState.settings.spacing).toBe(4);
            expect(loadedState.settings.burden).toBe(3);
            expect(loadedState.metadata.projectId).toBe(100);
            expect(loadedState.metadata.siteId).toBe(200);
            done();
          },
          error: (error) => fail(`Load failed: ${error.message}`)
        });
      },
      error: (error) => fail(`Save failed: ${error.message}`)
    });
  });

  it('should validate state before saving', (done) => {
    // Initialize with invalid state
    stateService.initializeState(100, 200);
    stateService.updateSettings({ spacing: -1, burden: 0 }); // invalid settings

    const currentState = stateService.getCurrentState();
    
    dataStore.savePattern(currentState).subscribe({
      next: () => {
        fail('Save should fail for invalid state');
      },
      error: (error) => {
        expect(error.message).toContain('Pattern validation failed');
        done();
      }
    });
  });

  it('should export pattern data correctly', (done) => {
    // Initialize and populate state
    stateService.initializeState(100, 200);
    
    const point: DrillPoint = {
      id: 'DH1',
      x: 10,
      y: 20,
      depth: 15,
      spacing: 3,
      burden: 2.5
    };

    stateService.addDrillPoint(point);
    stateService.updateSettings({ spacing: 5, burden: 4, depth: 12 });

    const currentState = stateService.getCurrentState();
    
    dataStore.exportPattern(currentState).subscribe({
      next: (exportedData) => {
        expect(exportedData.drillPoints.length).toBe(1);
        expect(exportedData.drillPoints[0].id).toBe('DH1');
        expect(exportedData.settings.spacing).toBe(5);
        expect(exportedData.settings.burden).toBe(4);
        expect(exportedData.settings.depth).toBe(12);
        done();
      },
      error: (error) => fail(`Export failed: ${error.message}`)
    });
  });

  it('should handle reactive state updates correctly', (done) => {
    stateService.initializeState(100, 200);

    // Track state changes
    const stateChanges: any[] = [];
    
    combineLatest([
      stateService.drillPointsCount$,
      stateService.settings$,
      stateService.isPatternModified$
    ]).subscribe(([count, settings, isModified]) => {
      stateChanges.push({ count, settings, isModified });
    });

    // Make changes
    stateService.addDrillPoint({
      id: 'DH1',
      x: 10,
      y: 20,
      depth: 15,
      spacing: 3,
      burden: 2.5
    });

    stateService.updateSettings({ spacing: 6 });

    // Wait a bit for all reactive updates
    setTimeout(() => {
      expect(stateChanges.length).toBeGreaterThan(1);
      
      const lastState = stateChanges[stateChanges.length - 1];
      expect(lastState.count).toBe(1);
      expect(lastState.settings.spacing).toBe(6);
      expect(lastState.isModified).toBeTrue();
      done();
    }, 100);
  });

  it('should handle validation status updates', (done) => {
    stateService.initializeState(100, 200);

    // Initially should be invalid (no drill points)
    stateService.getValidationStatus$().pipe(take(1)).subscribe(validation => {
      expect(validation.isValid).toBeFalse();
      expect(validation.errors.length).toBeGreaterThan(0);

      // Add a drill point to make it valid
      stateService.addDrillPoint({
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 15,
        spacing: 3,
        burden: 2.5
      });

      // Check validation again
      stateService.getValidationStatus$().pipe(take(1)).subscribe(updatedValidation => {
        expect(updatedValidation.isValid).toBeTrue();
        expect(updatedValidation.errors.length).toBe(0);
        done();
      });
    });
  });

  it('should handle custom depth tracking', (done) => {
    stateService.initializeState(100, 200);
    stateService.updateSettings({ depth: 10 });

    // Add points with different depths
    stateService.addDrillPoint({
      id: 'DH1',
      x: 10,
      y: 20,
      depth: 10, // same as global
      spacing: 3,
      burden: 2.5
    });

    stateService.addDrillPoint({
      id: 'DH2',
      x: 15,
      y: 25,
      depth: 15, // different from global
      spacing: 3,
      burden: 2.5
    });

    stateService.getPointsWithCustomDepths$().pipe(take(1)).subscribe(customPoints => {
      expect(customPoints.length).toBe(1);
      expect(customPoints[0].id).toBe('DH2');
      expect(customPoints[0].depth).toBe(15);
      done();
    });
  });

  it('should maintain state immutability', (done) => {
    stateService.initializeState(100, 200);

    const originalState = stateService.getCurrentState();
    const originalDrillPoints = originalState.drillPoints;
    const originalSettings = originalState.settings;

    // Make changes
    stateService.addDrillPoint({
      id: 'DH1',
      x: 10,
      y: 20,
      depth: 15,
      spacing: 3,
      burden: 2.5
    });

    stateService.updateSettings({ spacing: 7 });

    // Original state should remain unchanged
    expect(originalDrillPoints.length).toBe(0);
    expect(originalSettings.spacing).toBe(3);

    // New state should have changes
    stateService.state$.pipe(take(1)).subscribe(newState => {
      expect(newState.drillPoints.length).toBe(1);
      expect(newState.settings.spacing).toBe(7);
      
      // References should be different
      expect(newState.drillPoints).not.toBe(originalDrillPoints);
      expect(newState.settings).not.toBe(originalSettings);
      done();
    });
  });
});