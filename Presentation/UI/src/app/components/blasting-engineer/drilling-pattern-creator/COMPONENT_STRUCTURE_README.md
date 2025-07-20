# Drilling Pattern Creator - Component Structure & Interfaces

This document describes the component structure, interfaces, and communication contracts for the refactored drilling pattern creator.

## Overview

The drilling pattern creator has been refactored from a monolithic component into a modular, well-structured architecture with clear separation of concerns. This document outlines the component interfaces, base classes, and communication contracts that form the foundation of the refactored system.

## Architecture

### Component Hierarchy

```
DrillingPatternCreatorComponent (Container)
├── PatternToolbarComponent (Presentation)
├── PatternCanvasComponent (Canvas Operations)
│   ├── GridCanvasComponent (Grid Rendering)
│   ├── RulerCanvasComponent (Ruler Rendering)
│   └── DrillPointCanvasComponent (Point Rendering)
├── PatternStatusBarComponent (Status Display)
└── PatternInstructionsComponent (Help Content)
```

### Service Layer

```
Core Services:
├── PatternStateService (State Management)
├── PatternDataStore (Data Operations)
├── CanvasManagerService (Canvas Coordination)
└── PatternValidationService (Business Rules)

Canvas Services:
├── GridRenderingService (Grid Operations)
├── RulerRenderingService (Ruler Operations)
├── DrillPointRenderingService (Point Operations)
└── CanvasInteractionService (User Interactions)

Support Services:
├── ErrorHandlingService (Error Management)
├── PatternEventBusService (Event Communication)
└── PerformanceMonitoringService (Performance Tracking)
```

## Component Interfaces

### Base Interfaces

#### `BasePatternComponent`
All pattern creator components implement this base interface:
- `ngOnInit()`: Component initialization
- `ngOnDestroy()`: Component cleanup

#### `BasePatternComponentClass`
Abstract base class providing:
- Automatic lifecycle management
- Error handling integration
- Event bus subscription
- State management integration
- Performance monitoring

#### `BaseCanvasComponentClass`
Extends `BasePatternComponentClass` for canvas components:
- Canvas layer management
- Rendering service integration
- Performance optimization (caching)
- Coordinate transformations

### Component-Specific Interfaces

#### `IDrillingPatternCreatorComponent`
Main container component interface:
```typescript
interface IDrillingPatternCreatorComponent {
  readonly state$: Observable<PatternState>;
  readonly isLoading$: Observable<boolean>;
  onToolbarAction(action: ToolbarAction): void;
  onCanvasEvent(event: CanvasEvent): void;
}
```

#### `IPatternToolbarComponent`
Toolbar component interface:
```typescript
interface IPatternToolbarComponent {
  // Inputs
  settings: PatternSettings;
  selectedPoint: DrillPoint | null;
  drillPointsCount: number;
  // ... other inputs
  
  // Outputs
  settingsChange: EventEmitter<PatternSettings>;
  modeToggle: EventEmitter<ModeToggleEvent>;
  // ... other outputs
}
```

#### `IPatternCanvasComponent`
Main canvas component interface:
```typescript
interface IPatternCanvasComponent {
  // Canvas lifecycle
  initializeCanvas(): void;
  updateCanvas(): void;
  destroyCanvas(): void;
  
  // Event handling
  onPointPlaced(event: PlacePointEvent): void;
  onPointSelected(point: DrillPoint): void;
  // ... other methods
}
```

## Service Interfaces

### Core Services

#### `IPatternStateService`
Central state management:
```typescript
interface IPatternStateService {
  // State observables
  readonly state$: Observable<PatternState>;
  readonly drillPoints$: Observable<DrillPoint[]>;
  // ... other observables
  
  // State actions
  updateSettings(settings: Partial<PatternSettings>): void;
  addDrillPoint(point: DrillPoint): void;
  // ... other actions
}
```

#### `ICanvasManagerService`
Canvas coordination:
```typescript
interface ICanvasManagerService {
  initializeCanvas(containerId: string): Observable<any>;
  createLayer(name: string): any;
  zoom(scale: number, centerX?: number, centerY?: number): void;
  // ... other methods
}
```

### Rendering Services

#### `ICanvasRenderingService`
Base rendering interface:
```typescript
interface ICanvasRenderingService {
  initialize(layer: any): void;
  render(): void;
  update(): void;
  clear(): void;
  destroy(): void;
}
```

Specialized rendering services extend this interface:
- `IGridRenderingService`: Grid-specific rendering
- `IRulerRenderingService`: Ruler-specific rendering
- `IDrillPointRenderingService`: Point-specific rendering

## Event System

### Event Types

The system uses a comprehensive event system with strongly-typed events:

#### Toolbar Events
- `ToolbarSettingsChangeEvent`: Settings modifications
- `ToolbarModeToggleEvent`: Mode changes
- `ToolbarPointActionEvent`: Point-related actions
- `ToolbarPatternActionEvent`: Pattern-level actions

#### Canvas Events
- `CanvasPointPlacedEvent`: Point placement
- `CanvasPointSelectedEvent`: Point selection
- `CanvasPointMovedEvent`: Point movement
- `CanvasStateChangedEvent`: Canvas state changes

#### System Events
- `ComponentInitializedEvent`: Component lifecycle
- `ComponentErrorEvent`: Error handling
- `PerformanceEvent`: Performance monitoring

### Event Bus

The `IPatternEventBusService` provides centralized event communication:
```typescript
interface IPatternEventBusService {
  emit<T extends PatternCreatorEvent>(event: T): void;
  subscribe<T extends PatternCreatorEvent>(
    eventType: T['type'],
    handler: (event: T) => void
  ): () => void;
}
```

## Communication Contracts

### Component Contracts

Each component has a well-defined contract specifying:
- Input properties and their types
- Output events and their payloads
- Public methods and their signatures
- Validation requirements

Example:
```typescript
interface PatternToolbarContract {
  // Inputs
  settings: PatternSettings;
  selectedPoint: DrillPoint | null;
  
  // Outputs
  settingsChange: EventEmitter<PatternSettings>;
  modeToggle: EventEmitter<ModeToggleEvent>;
  
  // Methods
  resetToDefaults(): void;
  validateSettings(): { isValid: boolean; errors: string[] };
}
```

### Validation Contracts

Components that support validation implement:
```typescript
interface ValidationContract {
  readonly isValid$: Observable<boolean>;
  readonly validationErrors$: Observable<string[]>;
  validate(): Promise<{ isValid: boolean; errors: string[] }>;
}
```

### Undo/Redo Contracts

Components supporting undo/redo implement:
```typescript
interface UndoRedoContract {
  readonly canUndo$: Observable<boolean>;
  readonly canRedo$: Observable<boolean>;
  undo(): void;
  redo(): void;
}
```

## Dependency Injection

### Service Tokens

All services are provided through injection tokens:
```typescript
export const PATTERN_STATE_SERVICE = new InjectionToken<IPatternStateService>('PatternStateService');
export const CANVAS_MANAGER_SERVICE = new InjectionToken<ICanvasManagerService>('CanvasManagerService');
// ... other tokens
```

### Provider Configuration

Services are configured through provider functions:
```typescript
export function providePatternCreatorServices(
  config?: Partial<PatternCreatorConfig>
): Provider[] {
  return [
    { provide: PATTERN_STATE_SERVICE, useClass: PatternStateService },
    { provide: CANVAS_MANAGER_SERVICE, useClass: CanvasManagerService },
    // ... other providers
  ];
}
```

### Configuration

The system supports configuration through tokens:
- `PATTERN_CREATOR_CONFIG`: General configuration
- `CANVAS_CONFIG`: Canvas-specific settings
- `VALIDATION_CONFIG`: Validation rules

## Usage Examples

### Component Implementation

```typescript
@Component({
  selector: 'app-pattern-toolbar',
  template: '...'
})
export class PatternToolbarComponent 
  extends BasePatternComponentClass 
  implements IPatternToolbarComponent {
  
  protected readonly componentName = 'PatternToolbarComponent';
  
  @Input() settings!: PatternSettings;
  @Output() settingsChange = new EventEmitter<PatternSettings>();
  
  onSettingsChange(newSettings: PatternSettings): void {
    this.settingsChange.emit(newSettings);
    this.emitEvent({
      type: 'TOOLBAR_SETTINGS_CHANGE',
      settings: newSettings
    });
  }
}
```

### Service Implementation

```typescript
@Injectable()
export class PatternStateService implements IPatternStateService {
  private readonly stateSubject = new BehaviorSubject<PatternState>(initialState);
  
  readonly state$ = this.stateSubject.asObservable();
  readonly drillPoints$ = this.state$.pipe(map(state => state.drillPoints));
  
  updateSettings(settings: Partial<PatternSettings>): void {
    const currentState = this.stateSubject.value;
    this.stateSubject.next({
      ...currentState,
      settings: { ...currentState.settings, ...settings }
    });
  }
}
```

### Testing

```typescript
describe('PatternToolbarComponent', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [PatternToolbarComponent],
      providers: [
        ...providePatternCreatorTestingServices()
      ]
    });
  });
  
  it('should emit settings change', () => {
    const component = TestBed.createComponent(PatternToolbarComponent);
    spyOn(component.componentInstance.settingsChange, 'emit');
    
    component.componentInstance.onSettingsChange(mockSettings);
    
    expect(component.componentInstance.settingsChange.emit).toHaveBeenCalledWith(mockSettings);
  });
});
```

## Benefits

This structure provides:

1. **Type Safety**: All interfaces are strongly typed
2. **Testability**: Clear contracts enable easy mocking
3. **Maintainability**: Single responsibility principle
4. **Extensibility**: Well-defined extension points
5. **Performance**: Optimized rendering and state management
6. **Error Handling**: Comprehensive error management
7. **Documentation**: Self-documenting interfaces

## Next Steps

1. Implement concrete service classes
2. Create component implementations
3. Add comprehensive unit tests
4. Implement integration tests
5. Add performance optimizations
6. Create documentation and examples

This foundation enables the systematic implementation of the remaining refactoring tasks while maintaining code quality and architectural integrity.