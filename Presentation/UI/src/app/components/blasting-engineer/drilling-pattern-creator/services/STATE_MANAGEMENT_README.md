# Drilling Pattern Creator - State Management Infrastructure

This document describes the new reactive state management infrastructure implemented for the drilling pattern creator refactoring.

## Overview

The state management system follows reactive patterns using RxJS and provides a centralized, predictable way to manage application state. It consists of three main components:

1. **PatternStateService** - Central state management with reactive streams
2. **PatternDataStore** - Data persistence and validation operations  
3. **Pattern State Models** - TypeScript interfaces and types

## Architecture

### PatternStateService

The `PatternStateService` is the core of the state management system. It provides:

- **Centralized State**: Single source of truth for all pattern data
- **Reactive Streams**: RxJS observables for state changes
- **Immutable Updates**: State changes create new state objects
- **Derived Selectors**: Computed observables for common queries

#### Key Features:

```typescript
// State observables
readonly state$: Observable<PatternState>
readonly drillPoints$: Observable<DrillPoint[]>
readonly settings$: Observable<PatternSettings>
readonly selectedPoint$: Observable<DrillPoint | null>
readonly canvasState$: Observable<CanvasState>
readonly uiState$: Observable<UIState>

// Computed selectors
readonly drillPointsCount$: Observable<number>
readonly isPatternModified$: Observable<boolean>
readonly canvasInitialized$: Observable<boolean>
```

#### Usage Example:

```typescript
// Inject the service
constructor(private patternState: PatternStateService) {}

// Subscribe to state changes
this.patternState.drillPoints$.subscribe(points => {
  console.log('Drill points updated:', points);
});

// Update state
this.patternState.addDrillPoint(newPoint);
this.patternState.updateSettings({ spacing: 4 });
this.patternState.selectPoint(selectedPoint);
```

### PatternDataStore

The `PatternDataStore` handles data persistence, validation, and export operations:

- **Pattern Validation**: Comprehensive validation with errors and warnings
- **Data Persistence**: Save/load patterns to/from localStorage (can be extended for API)
- **Export Operations**: Export pattern data for external use
- **Loading States**: Track async operation states

#### Key Features:

```typescript
// Core operations
savePattern(state: PatternState): Observable<void>
loadPattern(projectId: number, siteId: number): Observable<PatternState>
exportPattern(state: PatternState): Observable<PatternData>
validatePattern(state: PatternState): Observable<ValidationResult>

// State tracking
readonly isLoading$: Observable<boolean>
readonly lastError$: Observable<Error | null>
```

#### Usage Example:

```typescript
// Save current pattern
this.dataStore.savePattern(currentState).subscribe({
  next: () => console.log('Pattern saved successfully'),
  error: (error) => console.error('Save failed:', error)
});

// Load pattern
this.dataStore.loadPattern(projectId, siteId).subscribe({
  next: (state) => this.patternState.loadPattern(state),
  error: (error) => console.error('Load failed:', error)
});
```

## State Structure

### PatternState Interface

The main state interface contains all application state:

```typescript
interface PatternState {
  drillPoints: DrillPoint[];           // Array of drill points
  settings: PatternSettings;           // Pattern settings (spacing, burden, depth)
  selectedPoint: DrillPoint | null;    // Currently selected point
  canvasState: CanvasState;           // Canvas-specific state
  uiState: UIState;                   // UI-specific state
  metadata: PatternMetadata;          // Pattern metadata
}
```

### CanvasState Interface

Canvas-specific state for rendering and interactions:

```typescript
interface CanvasState {
  scale: number;                      // Zoom level
  panOffsetX: number;                 // Pan offset X
  panOffsetY: number;                 // Pan offset Y
  isInitialized: boolean;             // Canvas initialization status
  isDragging: boolean;                // Drag operation in progress
  isPanning: boolean;                 // Pan operation in progress
}
```

### UIState Interface

UI-specific state for modes and user interactions:

```typescript
interface UIState {
  isHolePlacementMode: boolean;       // Hole placement mode active
  isPreciseMode: boolean;             // Precise mode active
  isFullscreen: boolean;              // Fullscreen mode active
  showInstructions: boolean;          // Instructions visible
  isSaved: boolean;                   // Pattern saved status
  duplicateMessage: string | null;    // Duplicate point message
  cursorPosition: { x: number; y: number } | null; // Cursor position
}
```

## Event System

The system uses typed events for component communication:

### Event Types

```typescript
// Toolbar actions
interface ToolbarAction {
  type: 'TOGGLE_MODE' | 'UPDATE_SETTINGS' | 'POINT_ACTION' | 'PATTERN_ACTION';
  payload: any;
}

// Canvas events
interface CanvasEvent {
  type: 'POINT_PLACED' | 'POINT_SELECTED' | 'POINT_MOVED' | 'STATE_CHANGED';
  payload: any;
}

// Specific event types
interface PlacePointEvent {
  x: number;
  y: number;
  settings: PatternSettings;
}

interface MovePointEvent {
  pointId: string;
  newX: number;
  newY: number;
}
```

## Validation System

The data store includes comprehensive validation:

### Validation Rules

- **Drill Points**: Must have at least one point, maximum 500 points
- **Settings**: Spacing, burden, and depth must be positive numbers
- **Coordinates**: No duplicate drill point coordinates
- **IDs**: No duplicate drill point IDs
- **Depth Range**: Depth must be between 0 and 50 meters

### Validation Result

```typescript
interface ValidationResult {
  isValid: boolean;
  errors: ValidationError[];      // Blocking errors
  warnings: ValidationWarning[];  // Non-blocking warnings
}
```

## Testing

The state management system includes comprehensive unit tests:

- **PatternStateService Tests**: State management, reactive streams, immutability
- **PatternDataStore Tests**: Validation, persistence, error handling
- **Integration Tests**: End-to-end state management workflows
- **Model Tests**: Interface validation and type safety

### Running Tests

```bash
# Run specific test files
ng test --include="**/pattern-state.service.spec.ts"
ng test --include="**/pattern-data.store.spec.ts"
ng test --include="**/pattern-state-integration.spec.ts"
```

## Usage Guidelines

### Best Practices

1. **Subscribe in Components**: Use async pipe or subscribe in ngOnInit
2. **Unsubscribe**: Always unsubscribe in ngOnDestroy to prevent memory leaks
3. **Immutable Updates**: Never mutate state directly, use service methods
4. **Error Handling**: Always handle errors in subscriptions
5. **Loading States**: Use loading observables for UI feedback

### Example Component Integration

```typescript
@Component({
  selector: 'app-pattern-toolbar',
  template: `
    <div *ngIf="isLoading$ | async">Loading...</div>
    <div>Points: {{ drillPointsCount$ | async }}</div>
    <button (click)="addPoint()" [disabled]="isLoading$ | async">
      Add Point
    </button>
  `
})
export class PatternToolbarComponent implements OnInit, OnDestroy {
  drillPointsCount$ = this.patternState.drillPointsCount$;
  isLoading$ = this.dataStore.isLoading$;
  
  private destroy$ = new Subject<void>();

  constructor(
    private patternState: PatternStateService,
    private dataStore: PatternDataStore
  ) {}

  ngOnInit() {
    // Subscribe to state changes
    this.patternState.selectedPoint$
      .pipe(takeUntil(this.destroy$))
      .subscribe(point => {
        console.log('Selected point changed:', point);
      });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  addPoint() {
    const newPoint: DrillPoint = {
      id: 'DH' + Date.now(),
      x: 10,
      y: 20,
      depth: 15,
      spacing: 3,
      burden: 2.5
    };
    
    this.patternState.addDrillPoint(newPoint);
  }
}
```

## Migration Guide

When integrating with existing components:

1. **Inject Services**: Add PatternStateService and PatternDataStore to constructors
2. **Replace Direct State**: Replace component properties with service observables
3. **Update Methods**: Use service methods instead of direct state manipulation
4. **Add Subscriptions**: Subscribe to relevant state observables
5. **Handle Async**: Use loading states and error handling

## Performance Considerations

- **Change Detection**: Use OnPush strategy with observables
- **Selective Subscriptions**: Only subscribe to needed state slices
- **Unsubscribe**: Always clean up subscriptions
- **Debouncing**: Use debouncing for frequent updates
- **Memoization**: Leverage distinctUntilChanged for expensive operations

## Future Enhancements

Potential improvements for the state management system:

1. **Undo/Redo**: Implement command pattern for state history
2. **Persistence**: Replace localStorage with API integration
3. **Offline Support**: Add offline-first capabilities
4. **State Snapshots**: Implement state snapshots for debugging
5. **Performance Monitoring**: Add state change performance metrics