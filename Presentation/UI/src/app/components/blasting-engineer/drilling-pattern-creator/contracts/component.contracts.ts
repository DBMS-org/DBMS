import { EventEmitter, Type } from '@angular/core';
import { Observable } from 'rxjs';
import {
    DrillPoint,
    PatternSettings
} from '../models/drill-point.model';
import {
    CanvasState,
    UIState,
    PatternState,
    ModeToggleEvent,
    PointActionEvent,
    PatternActionEvent,
    PlacePointEvent,
    MovePointEvent
} from '../models/pattern-state.model';

/**
 * Input/Output contract for the main drilling pattern creator component
 */
export interface DrillingPatternCreatorContract {
    // Inputs
    projectId: number;
    siteId: number;
    initialData?: any;

    // Outputs
    patternSaved: EventEmitter<void>;
    patternExported: EventEmitter<any>;
    stateChanged: EventEmitter<PatternState>;
    error: EventEmitter<Error>;
}

/**
 * Input/Output contract for the pattern toolbar component
 */
export interface PatternToolbarContract {
    // Inputs
    settings: PatternSettings;
    selectedPoint: DrillPoint | null;
    drillPointsCount: number;
    isHolePlacementMode: boolean;
    isPreciseMode: boolean;
    isSaved: boolean;
    disabled?: boolean;

    // Outputs
    settingsChange: EventEmitter<PatternSettings>;
    modeToggle: EventEmitter<ModeToggleEvent>;
    pointAction: EventEmitter<PointActionEvent>;
    patternAction: EventEmitter<PatternActionEvent>;

    // Methods
    resetToDefaults(): void;
    validateSettings(): { isValid: boolean; errors: string[] };
}

/**
 * Input/Output contract for the pattern canvas component
 */
export interface PatternCanvasContract {
    // Inputs
    settings: PatternSettings;
    drillPoints: DrillPoint[];
    selectedPoint: DrillPoint | null;
    canvasState: CanvasState;
    uiState: UIState;

    // Outputs
    pointPlaced: EventEmitter<PlacePointEvent>;
    pointSelected: EventEmitter<DrillPoint | null>;
    pointMoved: EventEmitter<MovePointEvent>;
    canvasStateChange: EventEmitter<CanvasState>;

    // Methods
    initializeCanvas(): Promise<void>;
    resizeCanvas(width: number, height: number): void;
    exportAsImage(): Promise<Blob>;
    fitToScreen(): void;
    resetView(): void;
}

/**
 * Input/Output contract for grid canvas component
 */
export interface GridCanvasContract {
    // Inputs
    layer: any; // Konva.Layer
    settings: PatternSettings;
    canvasState: CanvasState;
    isPreciseMode: boolean;
    visible: boolean;

    // Methods
    renderGrid(): void;
    updateGridSpacing(spacing: number): void;
    setGridColor(color: string): void;
    showIntersections(show: boolean): void;
}

/**
 * Input/Output contract for ruler canvas component
 */
export interface RulerCanvasContract {
    // Inputs
    layer: any; // Konva.Layer
    settings: PatternSettings;
    canvasState: CanvasState;
    unit: 'meters' | 'feet';
    visible: boolean;

    // Methods
    renderRuler(): void;
    setUnit(unit: 'meters' | 'feet'): void;
    showHorizontalRuler(show: boolean): void;
    showVerticalRuler(show: boolean): void;
}

/**
 * Input/Output contract for drill point canvas component
 */
export interface DrillPointCanvasContract {
    // Inputs
    layer: any; // Konva.Layer
    drillPoints: DrillPoint[];
    selectedPoint: DrillPoint | null;
    settings: PatternSettings;
    highlightedPointId?: string;

    // Outputs
    pointSelected: EventEmitter<DrillPoint>;
    pointMoved: EventEmitter<MovePointEvent>;
    pointDoubleClicked: EventEmitter<DrillPoint>;

    // Methods
    renderPoints(): void;
    selectPoint(pointId: string | null): void;
    highlightPoint(pointId: string): void;
    updatePointPosition(pointId: string, x: number, y: number): void;
    setPointStyle(style: PointStyle): void;
}

/**
 * Input/Output contract for status bar component
 */
export interface PatternStatusBarContract {
    // Inputs
    canvasState: CanvasState;
    uiState: UIState;
    drillPointsCount: number;
    selectedPoint: DrillPoint | null;

    // Methods
    updateCursorPosition(x: number, y: number): void;
    showMessage(message: string, type: 'info' | 'warning' | 'error'): void;
    clearMessage(): void;
}

/**
 * Input/Output contract for instructions component
 */
export interface PatternInstructionsContract {
    // Inputs
    showInstructions: boolean;
    currentMode: 'normal' | 'hole_placement' | 'precise';

    // Outputs
    instructionsToggle: EventEmitter<boolean>;

    // Methods
    showModeInstructions(mode: string): void;
    showKeyboardShortcuts(): void;
}

/**
 * Communication contract between parent and child components
 */
export interface ComponentCommunicationContract<TInput = any, TOutput = any> {
    // Data flow down (parent to child)
    inputs: TInput;

    // Events flow up (child to parent)
    outputs: TOutput;

    // Bidirectional communication methods
    sendCommand?(command: string, data?: any): void;
    receiveCommand?(command: string, data?: any): void;
}

/**
 * Contract for components that support validation
 */
export interface ValidationContract {
    // Validation state
    readonly isValid$: Observable<boolean>;
    readonly validationErrors$: Observable<string[]>;

    // Validation methods
    validate(): Promise<{ isValid: boolean; errors: string[] }>;
    clearValidationErrors(): void;

    // Validation events
    validationStateChanged: EventEmitter<{ isValid: boolean; errors: string[] }>;
}

/**
 * Contract for components that support undo/redo
 */
export interface UndoRedoContract {
    // State
    readonly canUndo$: Observable<boolean>;
    readonly canRedo$: Observable<boolean>;

    // Methods
    undo(): void;
    redo(): void;
    clearHistory(): void;

    // Events
    undoExecuted: EventEmitter<any>;
    redoExecuted: EventEmitter<any>;
}

/**
 * Contract for components that support keyboard shortcuts
 */
export interface KeyboardShortcutContract {
    // Shortcut definitions
    readonly shortcuts: KeyboardShortcut[];

    // Methods
    registerShortcut(shortcut: KeyboardShortcut): void;
    unregisterShortcut(key: string): void;
    enableShortcuts(enabled: boolean): void;

    // Events
    shortcutExecuted: EventEmitter<{ key: string; action: string }>;
}

/**
 * Supporting interfaces
 */
export interface PointStyle {
    color: string;
    size: number;
    strokeWidth: number;
    strokeColor?: string;
    opacity?: number;
}

export interface KeyboardShortcut {
    key: string;
    modifiers?: ('ctrl' | 'alt' | 'shift')[];
    action: string;
    description: string;
    handler: () => void;
}

/**
 * Component factory contract for dynamic component creation
 */
export interface ComponentFactoryContract<T = any> {
    create(type: Type<T>, inputs?: any): T;
    destroy(component: T): void;
    getInstances(): T[];
}

/**
 * Component registry contract for managing component instances
 */
export interface ComponentRegistryContract {
    register<T>(name: string, component: T): void;
    unregister(name: string): void;
    get<T>(name: string): T | null;
    getAll(): Map<string, any>;
    clear(): void;
}

/**
 * Component lifecycle contract
 */
export interface ComponentLifecycleContract {
    // Lifecycle events
    initialized: EventEmitter<void>;
    destroyed: EventEmitter<void>;
    stateChanged: EventEmitter<any>;
    errorOccurred: EventEmitter<Error>;

    // Lifecycle methods
    initialize(): Promise<void>;
    destroy(): Promise<void>;
    reset(): void;

    // State
    readonly isInitialized: boolean;
    readonly isDestroyed: boolean;
}