import { Injectable } from '@angular/core';
import { Observable, of, throwError, BehaviorSubject } from 'rxjs';
import { map, catchError, tap, delay } from 'rxjs/operators';
import { PatternState } from '../models/pattern-state.model';
import { DrillPoint, PatternSettings, PatternData } from '../models/drill-point.model';

/**
 * Data store interface for pattern persistence operations
 */
export interface PatternDataOperations {
  savePattern(state: PatternState): Observable<void>;
  loadPattern(projectId: number, siteId: number): Observable<PatternState>;
  exportPattern(state: PatternState): Observable<PatternData>;
  validatePattern(state: PatternState): Observable<ValidationResult>;
}

/**
 * Validation result interface
 */
export interface ValidationResult {
  isValid: boolean;
  errors: ValidationError[];
  warnings: ValidationWarning[];
}

export interface ValidationError {
  field: string;
  message: string;
  code: string;
}

export interface ValidationWarning {
  field: string;
  message: string;
  code: string;
}

/**
 * Pattern data store service for centralized data operations
 * Handles persistence, validation, and data transformation
 */
@Injectable({
  providedIn: 'root'
})
export class PatternDataStore implements PatternDataOperations {
  private readonly _isLoading$ = new BehaviorSubject<boolean>(false);
  private readonly _lastError$ = new BehaviorSubject<Error | null>(null);

  // Public observables
  readonly isLoading$ = this._isLoading$.asObservable();
  readonly lastError$ = this._lastError$.asObservable();

  constructor() {}

  /**
   * Save pattern to persistent storage
   */
  savePattern(state: PatternState): Observable<void> {
    this._isLoading$.next(true);
    this._lastError$.next(null);

    // Simulate API call with validation
    return this.validatePattern(state).pipe(
      map(validation => {
        if (!validation.isValid) {
          throw new Error(`Pattern validation failed: ${validation.errors.map(e => e.message).join(', ')}`);
        }
        return validation;
      }),
      delay(500), // Simulate network delay
      map(() => {
        // Simulate successful save
        const savedData = this.serializePatternState(state);
        this.saveToLocalStorage(state.metadata.projectId, state.metadata.siteId, savedData);
        return void 0;
      }),
      tap(() => this._isLoading$.next(false)),
      catchError(error => {
        this._isLoading$.next(false);
        this._lastError$.next(error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Load pattern from persistent storage
   */
  loadPattern(projectId: number, siteId: number): Observable<PatternState> {
    this._isLoading$.next(true);
    this._lastError$.next(null);

    return of(null).pipe(
      delay(300), // Simulate network delay
      map(() => {
        const savedData = this.loadFromLocalStorage(projectId, siteId);
        if (savedData) {
          return this.deserializePatternState(savedData);
        }
        throw new Error(`No pattern found for project ${projectId}, site ${siteId}`);
      }),
      tap(() => this._isLoading$.next(false)),
      catchError(error => {
        this._isLoading$.next(false);
        this._lastError$.next(error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Export pattern data for external use
   */
  exportPattern(state: PatternState): Observable<PatternData> {
    return this.validatePattern(state).pipe(
      map(validation => {
        if (!validation.isValid) {
          throw new Error(`Cannot export invalid pattern: ${validation.errors.map(e => e.message).join(', ')}`);
        }

        return {
          drillPoints: state.drillPoints.map(point => ({ ...point })),
          settings: { ...state.settings }
        };
      }),
      catchError(error => {
        this._lastError$.next(error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Validate pattern state
   */
  validatePattern(state: PatternState): Observable<ValidationResult> {
    const errors: ValidationError[] = [];
    const warnings: ValidationWarning[] = [];

    // Validate drill points
    if (state.drillPoints.length === 0) {
      errors.push({
        field: 'drillPoints',
        message: 'At least one drill point is required',
        code: 'EMPTY_PATTERN'
      });
    }

    if (state.drillPoints.length > 500) {
      errors.push({
        field: 'drillPoints',
        message: 'Maximum 500 drill points allowed',
        code: 'TOO_MANY_POINTS'
      });
    }

    // Validate settings
    if (state.settings.spacing <= 0) {
      errors.push({
        field: 'settings.spacing',
        message: 'Spacing must be greater than 0',
        code: 'INVALID_SPACING'
      });
    }

    if (state.settings.burden <= 0) {
      errors.push({
        field: 'settings.burden',
        message: 'Burden must be greater than 0',
        code: 'INVALID_BURDEN'
      });
    }

    if (state.settings.depth <= 0 || state.settings.depth > 50) {
      errors.push({
        field: 'settings.depth',
        message: 'Depth must be between 0 and 50 meters',
        code: 'INVALID_DEPTH'
      });
    }

    // Validate drill point coordinates
    const duplicatePoints = this.findDuplicatePoints(state.drillPoints);
    if (duplicatePoints.length > 0) {
      errors.push({
        field: 'drillPoints',
        message: `Duplicate drill points found at coordinates: ${duplicatePoints.map(p => `(${p.x}, ${p.y})`).join(', ')}`,
        code: 'DUPLICATE_POINTS'
      });
    }

    // Validate drill point IDs
    const duplicateIds = this.findDuplicateIds(state.drillPoints);
    if (duplicateIds.length > 0) {
      errors.push({
        field: 'drillPoints',
        message: `Duplicate drill point IDs found: ${duplicateIds.join(', ')}`,
        code: 'DUPLICATE_IDS'
      });
    }

    // Add warnings
    if (state.settings.spacing > 10) {
      warnings.push({
        field: 'settings.spacing',
        message: 'Large spacing value may not be typical for drilling patterns',
        code: 'LARGE_SPACING'
      });
    }

    if (state.settings.burden > 10) {
      warnings.push({
        field: 'settings.burden',
        message: 'Large burden value may not be typical for drilling patterns',
        code: 'LARGE_BURDEN'
      });
    }

    const customDepthPoints = state.drillPoints.filter(p => 
      Math.abs(p.depth - state.settings.depth) > 0.01
    );
    if (customDepthPoints.length > 0) {
      warnings.push({
        field: 'drillPoints',
        message: `${customDepthPoints.length} drill points have custom depths`,
        code: 'CUSTOM_DEPTHS'
      });
    }

    return of({
      isValid: errors.length === 0,
      errors,
      warnings
    });
  }

  /**
   * Clear all cached data
   */
  clearCache(): void {
    // Clear localStorage patterns
    const keys = Object.keys(localStorage).filter(key => key.startsWith('pattern_'));
    keys.forEach(key => localStorage.removeItem(key));
  }

  /**
   * Get available patterns for a project
   */
  getAvailablePatterns(projectId: number): Observable<Array<{ siteId: number; lastModified: Date }>> {
    const patterns: Array<{ siteId: number; lastModified: Date }> = [];
    
    const keys = Object.keys(localStorage).filter(key => 
      key.startsWith(`pattern_${projectId}_`)
    );

    keys.forEach(key => {
      try {
        const data = JSON.parse(localStorage.getItem(key) || '{}');
        if (data.metadata) {
          patterns.push({
            siteId: data.metadata.siteId,
            lastModified: new Date(data.metadata.lastModified)
          });
        }
      } catch (error) {
        console.warn(`Failed to parse pattern data for key ${key}:`, error);
      }
    });

    return of(patterns);
  }

  /**
   * Private helper methods
   */
  private serializePatternState(state: PatternState): string {
    return JSON.stringify({
      ...state,
      metadata: {
        ...state.metadata,
        lastModified: state.metadata.lastModified.toISOString()
      }
    });
  }

  private deserializePatternState(data: string): PatternState {
    const parsed = JSON.parse(data);
    return {
      ...parsed,
      metadata: {
        ...parsed.metadata,
        lastModified: new Date(parsed.metadata.lastModified)
      }
    };
  }

  private saveToLocalStorage(projectId: number, siteId: number, data: string): void {
    const key = `pattern_${projectId}_${siteId}`;
    localStorage.setItem(key, data);
  }

  private loadFromLocalStorage(projectId: number, siteId: number): string | null {
    const key = `pattern_${projectId}_${siteId}`;
    return localStorage.getItem(key);
  }

  private findDuplicatePoints(points: DrillPoint[]): DrillPoint[] {
    const duplicates: DrillPoint[] = [];
    const seen = new Set<string>();

    points.forEach(point => {
      const key = `${point.x.toFixed(2)}_${point.y.toFixed(2)}`;
      if (seen.has(key)) {
        duplicates.push(point);
      } else {
        seen.add(key);
      }
    });

    return duplicates;
  }

  private findDuplicateIds(points: DrillPoint[]): string[] {
    const duplicates: string[] = [];
    const seen = new Set<string>();

    points.forEach(point => {
      if (seen.has(point.id)) {
        duplicates.push(point.id);
      } else {
        seen.add(point.id);
      }
    });

    return duplicates;
  }
}