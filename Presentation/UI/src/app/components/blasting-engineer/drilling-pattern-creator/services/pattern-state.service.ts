import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { DrillPoint, PatternSettings } from '../models/drill-point.model';
import { map } from 'rxjs/operators';

export interface PatternState {
  drillPoints: DrillPoint[];
  selectedPoint: DrillPoint | null;
  settings: PatternSettings;
  isHolePlacementMode: boolean;
  isPreciseMode: boolean;
  showInstructions: boolean;
  cursorPosition: { x: number; y: number } | null;
  duplicateAttemptMessage: string | null;
  isSaved: boolean;
  isFullscreen: boolean;
  selectedHoleDepth: number | null;
  isReadOnly: boolean;
}

const initialState: PatternState = {
  drillPoints: [],
  selectedPoint: null,
  settings: { spacing: 3.0, burden: 2.5, depth: 10.0, diameter: 115, stemming: 3, subDrill: 0.5 },
  isHolePlacementMode: false,
  isPreciseMode: false,
  showInstructions: false,
  cursorPosition: null,
  duplicateAttemptMessage: null,
  isSaved: false,
  isFullscreen: false,
  selectedHoleDepth: null,
  isReadOnly: false
};

@Injectable({
  providedIn: 'root'
})
export class PatternStateService {
  private _state$ = new BehaviorSubject<PatternState>(initialState);

  // State observables
  readonly state$: Observable<PatternState> = this._state$.asObservable();
  readonly drillPoints$: Observable<DrillPoint[]> = this._state$.pipe(
    map(state => state.drillPoints)
  );
  readonly selectedPoint$: Observable<DrillPoint | null> = this._state$.pipe(
    map(state => state.selectedPoint)
  );
  readonly settings$: Observable<PatternSettings> = this._state$.pipe(
    map(state => state.settings)
  );
  readonly isHolePlacementMode$: Observable<boolean> = this._state$.pipe(
    map(state => state.isHolePlacementMode)
  );
  readonly isPreciseMode$: Observable<boolean> = this._state$.pipe(
    map(state => state.isPreciseMode)
  );
  readonly isSaved$: Observable<boolean> = this._state$.pipe(
    map(state => state.isSaved)
  );
  readonly isFullscreen$: Observable<boolean> = this._state$.pipe(
    map(state => state.isFullscreen)
  );

  // State getters
  get currentState(): PatternState {
    return this._state$.value;
  }

  get drillPoints(): DrillPoint[] {
    return this.currentState.drillPoints;
  }

  get selectedPoint(): DrillPoint | null {
    return this.currentState.selectedPoint;
  }

  get settings(): PatternSettings {
    return this.currentState.settings;
  }

  // State actions
  updateSettings(settings: Partial<PatternSettings>): void {
    this.updateState({
      settings: { ...this.currentState.settings, ...settings },
      isSaved: false
    });
  }

  addDrillPoint(point: DrillPoint): void {
    console.log('Adding drill point to state:', point);
    const updatedPoints = [...this.currentState.drillPoints, point];
    console.log('Updated points array:', updatedPoints);
    this.updateState({
      drillPoints: updatedPoints,
      isSaved: false
    });
  }

  updateDrillPoint(updatedPoint: DrillPoint): void {
    const updatedPoints = this.currentState.drillPoints.map(point =>
      point.id === updatedPoint.id ? updatedPoint : point
    );
    this.updateState({
      drillPoints: updatedPoints,
      isSaved: false
    });
  }

  removeDrillPoint(pointId: string): void {
    const updatedPoints = this.currentState.drillPoints.filter(point => point.id !== pointId);
    this.updateState({
      drillPoints: updatedPoints,
      selectedPoint: this.currentState.selectedPoint?.id === pointId ? null : this.currentState.selectedPoint,
      isSaved: false
    });
  }

  selectPoint(point: DrillPoint | null): void {
    this.updateState({
      selectedPoint: point,
      selectedHoleDepth: point?.depth || null
    });
  }

  toggleHolePlacementMode(): void {
    this.updateState({
      isHolePlacementMode: !this.currentState.isHolePlacementMode
    });
  }

  togglePreciseMode(): void {
    this.updateState({
      isPreciseMode: !this.currentState.isPreciseMode
    });
  }

  toggleFullscreen(): void {
    this.updateState({
      isFullscreen: !this.currentState.isFullscreen
    });
  }

  setSaved(saved: boolean): void {
    this.updateState({ isSaved: saved });
  }

  setReadOnly(readonly: boolean): void {
    this.updateState({ isReadOnly: readonly });
  }

  setCursorPosition(position: { x: number; y: number } | null): void {
    this.updateState({ cursorPosition: position });
  }

  setDuplicateMessage(message: string | null): void {
    this.updateState({ duplicateAttemptMessage: message });
  }

  clearAllPoints(): void {
    this.updateState({
      drillPoints: [],
      selectedPoint: null,
      selectedHoleDepth: null,
      isSaved: false
    });
  }

  updateSelectedHoleDepth(depth: number): void {
    if (this.currentState.selectedPoint) {
      const updatedPoint = { ...this.currentState.selectedPoint, depth };
      this.updateDrillPoint(updatedPoint);
      this.updateState({ selectedHoleDepth: depth });
    }
  }

  toggleInstructions(): void {
    this.updateState({
      showInstructions: !this.currentState.showInstructions
    });
  }

  setShowInstructions(show: boolean): void {
    this.updateState({ showInstructions: show });
  }

  private updateState(updates: Partial<PatternState>): void {
    this._state$.next({ ...this.currentState, ...updates });
  }
}

