import { TestBed } from '@angular/core/testing';
import { PatternDataStore, ValidationResult } from './pattern-data.store';
import { PatternState, createInitialPatternState } from '../models/pattern-state.model';
import { DrillPoint } from '../models/drill-point.model';
import { take } from 'rxjs/operators';

describe('PatternDataStore', () => {
  let store: PatternDataStore;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    store = TestBed.inject(PatternDataStore);
    
    // Clear localStorage before each test
    localStorage.clear();
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should be created', () => {
    expect(store).toBeTruthy();
  });

  describe('Pattern Validation', () => {
    it('should validate empty pattern as invalid', (done) => {
      const emptyState = createInitialPatternState(1, 1);

      store.validatePattern(emptyState).subscribe(result => {
        expect(result.isValid).toBeFalse();
        expect(result.errors.length).toBeGreaterThan(0);
        expect(result.errors.some(e => e.code === 'EMPTY_PATTERN')).toBeTrue();
        done();
      });
    });

    it('should validate pattern with valid data', (done) => {
      const validState = createInitialPatternState(1, 1);
      validState.drillPoints = [{
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 15,
        spacing: 3,
        burden: 2.5
      }];

      store.validatePattern(validState).subscribe(result => {
        expect(result.isValid).toBeTrue();
        expect(result.errors.length).toBe(0);
        done();
      });
    });

    it('should detect invalid settings', (done) => {
      const invalidState = createInitialPatternState(1, 1);
      invalidState.settings.spacing = -1;
      invalidState.settings.burden = 0;
      invalidState.settings.depth = 100;

      store.validatePattern(invalidState).subscribe(result => {
        expect(result.isValid).toBeFalse();
        expect(result.errors.some(e => e.code === 'INVALID_SPACING')).toBeTrue();
        expect(result.errors.some(e => e.code === 'INVALID_BURDEN')).toBeTrue();
        expect(result.errors.some(e => e.code === 'INVALID_DEPTH')).toBeTrue();
        done();
      });
    });

    it('should detect duplicate points', (done) => {
      const stateWithDuplicates = createInitialPatternState(1, 1);
      stateWithDuplicates.drillPoints = [
        { id: 'DH1', x: 10, y: 20, depth: 15, spacing: 3, burden: 2.5 },
        { id: 'DH2', x: 10, y: 20, depth: 15, spacing: 3, burden: 2.5 } // duplicate coordinates
      ];

      store.validatePattern(stateWithDuplicates).subscribe(result => {
        expect(result.isValid).toBeFalse();
        expect(result.errors.some(e => e.code === 'DUPLICATE_POINTS')).toBeTrue();
        done();
      });
    });

    it('should detect duplicate IDs', (done) => {
      const stateWithDuplicateIds = createInitialPatternState(1, 1);
      stateWithDuplicateIds.drillPoints = [
        { id: 'DH1', x: 10, y: 20, depth: 15, spacing: 3, burden: 2.5 },
        { id: 'DH1', x: 15, y: 25, depth: 15, spacing: 3, burden: 2.5 } // duplicate ID
      ];

      store.validatePattern(stateWithDuplicateIds).subscribe(result => {
        expect(result.isValid).toBeFalse();
        expect(result.errors.some(e => e.code === 'DUPLICATE_IDS')).toBeTrue();
        done();
      });
    });

    it('should generate warnings for unusual values', (done) => {
      const stateWithWarnings = createInitialPatternState(1, 1);
      stateWithWarnings.settings.spacing = 15; // large spacing
      stateWithWarnings.settings.burden = 12; // large burden
      stateWithWarnings.drillPoints = [
        { id: 'DH1', x: 10, y: 20, depth: 25, spacing: 3, burden: 2.5 } // custom depth
      ];

      store.validatePattern(stateWithWarnings).subscribe(result => {
        expect(result.warnings.length).toBeGreaterThan(0);
        expect(result.warnings.some(w => w.code === 'LARGE_SPACING')).toBeTrue();
        expect(result.warnings.some(w => w.code === 'LARGE_BURDEN')).toBeTrue();
        expect(result.warnings.some(w => w.code === 'CUSTOM_DEPTHS')).toBeTrue();
        done();
      });
    });

    it('should validate maximum drill points limit', (done) => {
      const stateWithTooManyPoints = createInitialPatternState(1, 1);
      stateWithTooManyPoints.drillPoints = Array.from({ length: 501 }, (_, i) => ({
        id: `DH${i + 1}`,
        x: i,
        y: i,
        depth: 15,
        spacing: 3,
        burden: 2.5
      }));

      store.validatePattern(stateWithTooManyPoints).subscribe(result => {
        expect(result.isValid).toBeFalse();
        expect(result.errors.some(e => e.code === 'TOO_MANY_POINTS')).toBeTrue();
        done();
      });
    });
  });

  describe('Pattern Persistence', () => {
    it('should save valid pattern to localStorage', (done) => {
      const validState = createInitialPatternState(123, 456);
      validState.drillPoints = [{
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 15,
        spacing: 3,
        burden: 2.5
      }];

      store.savePattern(validState).subscribe({
        next: () => {
          const savedData = localStorage.getItem('pattern_123_456');
          expect(savedData).toBeTruthy();
          
          const parsedData = JSON.parse(savedData!);
          expect(parsedData.drillPoints.length).toBe(1);
          expect(parsedData.metadata.projectId).toBe(123);
          expect(parsedData.metadata.siteId).toBe(456);
          done();
        },
        error: (error) => {
          fail(`Save should not fail: ${error.message}`);
        }
      });
    });

    it('should reject saving invalid pattern', (done) => {
      const invalidState = createInitialPatternState(123, 456);
      invalidState.settings.spacing = -1; // invalid

      store.savePattern(invalidState).subscribe({
        next: () => {
          fail('Save should fail for invalid pattern');
        },
        error: (error) => {
          expect(error.message).toContain('Pattern validation failed');
          done();
        }
      });
    });

    it('should load pattern from localStorage', (done) => {
      const originalState = createInitialPatternState(123, 456);
      originalState.drillPoints = [{
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 15,
        spacing: 3,
        burden: 2.5
      }];

      // First save the pattern
      store.savePattern(originalState).subscribe(() => {
        // Then load it
        store.loadPattern(123, 456).subscribe(loadedState => {
          expect(loadedState.drillPoints.length).toBe(1);
          expect(loadedState.drillPoints[0].id).toBe('DH1');
          expect(loadedState.metadata.projectId).toBe(123);
          expect(loadedState.metadata.siteId).toBe(456);
          done();
        });
      });
    });

    it('should handle loading non-existent pattern', (done) => {
      store.loadPattern(999, 999).subscribe({
        next: () => {
          fail('Load should fail for non-existent pattern');
        },
        error: (error) => {
          expect(error.message).toContain('No pattern found');
          done();
        }
      });
    });
  });

  describe('Pattern Export', () => {
    it('should export valid pattern data', (done) => {
      const validState = createInitialPatternState(123, 456);
      validState.drillPoints = [{
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 15,
        spacing: 3,
        burden: 2.5
      }];

      store.exportPattern(validState).subscribe(exportedData => {
        expect(exportedData.drillPoints.length).toBe(1);
        expect(exportedData.drillPoints[0].id).toBe('DH1');
        expect(exportedData.settings.spacing).toBe(3);
        expect(exportedData.settings.burden).toBe(2.5);
        expect(exportedData.settings.depth).toBe(10);
        done();
      });
    });

    it('should reject exporting invalid pattern', (done) => {
      const invalidState = createInitialPatternState(123, 456);
      invalidState.settings.spacing = -1; // invalid

      store.exportPattern(invalidState).subscribe({
        next: () => {
          fail('Export should fail for invalid pattern');
        },
        error: (error) => {
          expect(error.message).toContain('Cannot export invalid pattern');
          done();
        }
      });
    });

    it('should create deep copy of data on export', (done) => {
      const validState = createInitialPatternState(123, 456);
      const originalPoint: DrillPoint = {
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 15,
        spacing: 3,
        burden: 2.5
      };
      validState.drillPoints = [originalPoint];

      store.exportPattern(validState).subscribe(exportedData => {
        // Modify exported data
        exportedData.drillPoints[0].x = 999;
        exportedData.settings.spacing = 999;

        // Original should remain unchanged
        expect(originalPoint.x).toBe(10);
        expect(validState.settings.spacing).toBe(3);
        done();
      });
    });
  });

  describe('Loading State Management', () => {
    it('should track loading state during save operation', (done) => {
      const validState = createInitialPatternState(123, 456);
      validState.drillPoints = [{
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 15,
        spacing: 3,
        burden: 2.5
      }];

      let loadingStates: boolean[] = [];

      store.isLoading$.subscribe(isLoading => {
        loadingStates.push(isLoading);
      });

      store.savePattern(validState).subscribe(() => {
        // Should have recorded: false (initial), true (during save), false (after save)
        expect(loadingStates.length).toBeGreaterThanOrEqual(2);
        expect(loadingStates[loadingStates.length - 1]).toBeFalse(); // final state should be false
        done();
      });
    });

    it('should track loading state during load operation', (done) => {
      let loadingStates: boolean[] = [];

      store.isLoading$.subscribe(isLoading => {
        loadingStates.push(isLoading);
      });

      store.loadPattern(999, 999).subscribe({
        next: () => {},
        error: () => {
          // Should have recorded loading states even on error
          expect(loadingStates.length).toBeGreaterThanOrEqual(2);
          expect(loadingStates[loadingStates.length - 1]).toBeFalse(); // final state should be false
          done();
        }
      });
    });
  });

  describe('Error Handling', () => {
    it('should track last error', (done) => {
      const invalidState = createInitialPatternState(123, 456);
      invalidState.settings.spacing = -1; // invalid

      store.savePattern(invalidState).subscribe({
        next: () => {},
        error: () => {
          store.lastError$.pipe(take(1)).subscribe(error => {
            expect(error).toBeTruthy();
            expect(error!.message).toContain('Pattern validation failed');
            done();
          });
        }
      });
    });

    it('should clear error on successful operation', (done) => {
      const validState = createInitialPatternState(123, 456);
      validState.drillPoints = [{
        id: 'DH1',
        x: 10,
        y: 20,
        depth: 15,
        spacing: 3,
        burden: 2.5
      }];

      store.savePattern(validState).subscribe(() => {
        store.lastError$.pipe(take(1)).subscribe(error => {
          expect(error).toBeNull();
          done();
        });
      });
    });
  });

  describe('Cache Management', () => {
    it('should clear all cached patterns', () => {
      // Save some test patterns
      localStorage.setItem('pattern_1_1', '{"test": "data1"}');
      localStorage.setItem('pattern_2_2', '{"test": "data2"}');
      localStorage.setItem('other_data', '{"test": "other"}');

      store.clearCache();

      expect(localStorage.getItem('pattern_1_1')).toBeNull();
      expect(localStorage.getItem('pattern_2_2')).toBeNull();
      expect(localStorage.getItem('other_data')).toBeTruthy(); // should not be cleared
    });

    it('should get available patterns for project', (done) => {
      // Save test patterns
      const pattern1 = createInitialPatternState(100, 1);
      const pattern2 = createInitialPatternState(100, 2);
      const pattern3 = createInitialPatternState(200, 1); // different project

      localStorage.setItem('pattern_100_1', JSON.stringify({
        ...pattern1,
        metadata: { ...pattern1.metadata, lastModified: pattern1.metadata.lastModified.toISOString() }
      }));
      localStorage.setItem('pattern_100_2', JSON.stringify({
        ...pattern2,
        metadata: { ...pattern2.metadata, lastModified: pattern2.metadata.lastModified.toISOString() }
      }));
      localStorage.setItem('pattern_200_1', JSON.stringify({
        ...pattern3,
        metadata: { ...pattern3.metadata, lastModified: pattern3.metadata.lastModified.toISOString() }
      }));

      store.getAvailablePatterns(100).subscribe(patterns => {
        expect(patterns.length).toBe(2);
        expect(patterns.some(p => p.siteId === 1)).toBeTrue();
        expect(patterns.some(p => p.siteId === 2)).toBeTrue();
        expect(patterns.every(p => p.lastModified instanceof Date)).toBeTrue();
        done();
      });
    });

    it('should handle corrupted pattern data gracefully', (done) => {
      // Save corrupted data
      localStorage.setItem('pattern_100_1', 'invalid json');
      localStorage.setItem('pattern_100_2', '{"invalid": "structure"}');

      store.getAvailablePatterns(100).subscribe(patterns => {
        expect(patterns.length).toBe(0); // should skip corrupted entries
        done();
      });
    });
  });
});