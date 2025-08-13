import { Injectable, signal } from '@angular/core';
import { Observable, BehaviorSubject, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';

export interface LoadingState {
  isLoading: boolean;
  operation: string;
  progress?: number;
}

@Injectable({
  providedIn: 'root'
})
export class MaintenanceLoadingService {
  private loadingStates = new Map<string, BehaviorSubject<LoadingState>>();
  
  // Global loading state
  private globalLoadingSubject = new BehaviorSubject<boolean>(false);
  public globalLoading$ = this.globalLoadingSubject.asObservable();
  
  // Performance tracking
  private performanceMetrics = signal<Map<string, number>>(new Map());

  /**
   * Start loading for a specific operation
   */
  startLoading(operationId: string, operation: string, progress?: number): void {
    const startTime = performance.now();
    
    if (!this.loadingStates.has(operationId)) {
      this.loadingStates.set(operationId, new BehaviorSubject<LoadingState>({
        isLoading: false,
        operation: ''
      }));
    }

    const state: LoadingState = {
      isLoading: true,
      operation,
      progress
    };

    this.loadingStates.get(operationId)!.next(state);
    this.updateGlobalLoading();
    
    // Store start time for performance tracking
    this.performanceMetrics.update(metrics => {
      const newMetrics = new Map(metrics);
      newMetrics.set(`${operationId}_start`, startTime);
      return newMetrics;
    });
  }

  /**
   * Update loading progress for an operation
   */
  updateProgress(operationId: string, progress: number): void {
    const currentState = this.loadingStates.get(operationId);
    if (currentState) {
      const state = currentState.value;
      currentState.next({
        ...state,
        progress
      });
    }
  }

  /**
   * Stop loading for a specific operation
   */
  stopLoading(operationId: string): void {
    const endTime = performance.now();
    const currentState = this.loadingStates.get(operationId);
    
    if (currentState) {
      currentState.next({
        isLoading: false,
        operation: ''
      });
      this.updateGlobalLoading();
      
      // Calculate and store performance metrics
      this.performanceMetrics.update(metrics => {
        const newMetrics = new Map(metrics);
        const startTime = newMetrics.get(`${operationId}_start`);
        if (startTime) {
          const duration = endTime - startTime;
          newMetrics.set(operationId, duration);
          newMetrics.delete(`${operationId}_start`);
        }
        return newMetrics;
      });
    }
  }

  /**
   * Get loading state for a specific operation
   */
  getLoadingState(operationId: string): Observable<LoadingState> {
    if (!this.loadingStates.has(operationId)) {
      this.loadingStates.set(operationId, new BehaviorSubject<LoadingState>({
        isLoading: false,
        operation: ''
      }));
    }
    return this.loadingStates.get(operationId)!.asObservable();
  }

  /**
   * Check if any operation is loading
   */
  isAnyLoading(): Observable<boolean> {
    if (this.loadingStates.size === 0) {
      return new BehaviorSubject<boolean>(false).asObservable();
    }

    const loadingObservables = Array.from(this.loadingStates.values());
    return combineLatest(loadingObservables).pipe(
      map(states => states.some(state => state.isLoading))
    );
  }

  /**
   * Get performance metrics for operations
   */
  getPerformanceMetrics(): Map<string, number> {
    return this.performanceMetrics();
  }

  /**
   * Clear all loading states
   */
  clearAll(): void {
    this.loadingStates.forEach(subject => {
      subject.next({ isLoading: false, operation: '' });
    });
    this.updateGlobalLoading();
  }

  /**
   * Get loading state as signal for reactive components
   */
  getLoadingSignal(operationId: string) {
    const loadingSignal = signal<LoadingState>({ isLoading: false, operation: '' });
    
    this.getLoadingState(operationId).subscribe(state => {
      loadingSignal.set(state);
    });
    
    return loadingSignal;
  }

  private updateGlobalLoading(): void {
    const isAnyLoading = Array.from(this.loadingStates.values())
      .some(subject => subject.value.isLoading);
    this.globalLoadingSubject.next(isAnyLoading);
  }
}