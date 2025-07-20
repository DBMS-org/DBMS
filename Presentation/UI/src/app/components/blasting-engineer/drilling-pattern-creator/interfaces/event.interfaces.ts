import { DrillPoint, PatternSettings, CanvasState } from '../models';

/**
 * Base interface for all events in the pattern creator
 */
export interface BasePatternEvent {
  type: string;
  timestamp: Date;
  source?: string;
}

/**
 * Toolbar-related events
 */
export interface ToolbarSettingsChangeEvent extends BasePatternEvent {
  type: 'TOOLBAR_SETTINGS_CHANGE';
  settings: Partial<PatternSettings>;
}

export interface ToolbarModeToggleEvent extends BasePatternEvent {
  type: 'TOOLBAR_MODE_TOGGLE';
  mode: 'HOLE_PLACEMENT' | 'PRECISE' | 'FULLSCREEN';
  enabled: boolean;
}

export interface ToolbarPointActionEvent extends BasePatternEvent {
  type: 'TOOLBAR_POINT_ACTION';
  action: 'DELETE' | 'CLEAR_ALL' | 'OPEN_DEPTH_EDITOR';
  pointId?: string;
}

export interface ToolbarPatternActionEvent extends BasePatternEvent {
  type: 'TOOLBAR_PATTERN_ACTION';
  action: 'SAVE' | 'EXPORT_TO_BLAST_DESIGNER';
}

/**
 * Canvas-related events
 */
export interface CanvasPointPlacedEvent extends BasePatternEvent {
  type: 'CANVAS_POINT_PLACED';
  x: number;
  y: number;
  settings: PatternSettings;
}

export interface CanvasPointSelectedEvent extends BasePatternEvent {
  type: 'CANVAS_POINT_SELECTED';
  point: DrillPoint | null;
}

export interface CanvasPointMovedEvent extends BasePatternEvent {
  type: 'CANVAS_POINT_MOVED';
  pointId: string;
  oldPosition: { x: number; y: number };
  newPosition: { x: number; y: number };
}

export interface CanvasStateChangedEvent extends BasePatternEvent {
  type: 'CANVAS_STATE_CHANGED';
  oldState: Partial<CanvasState>;
  newState: Partial<CanvasState>;
}

export interface CanvasZoomEvent extends BasePatternEvent {
  type: 'CANVAS_ZOOM';
  scale: number;
  centerX: number;
  centerY: number;
}

export interface CanvasPanEvent extends BasePatternEvent {
  type: 'CANVAS_PAN';
  deltaX: number;
  deltaY: number;
}

/**
 * Validation events
 */
export interface ValidationErrorEvent extends BasePatternEvent {
  type: 'VALIDATION_ERROR';
  field: string;
  message: string;
  value?: any;
}

export interface ValidationSuccessEvent extends BasePatternEvent {
  type: 'VALIDATION_SUCCESS';
  field: string;
}

/**
 * State management events
 */
export interface StateUpdateEvent extends BasePatternEvent {
  type: 'STATE_UPDATE';
  property: string;
  oldValue: any;
  newValue: any;
}

export interface StateResetEvent extends BasePatternEvent {
  type: 'STATE_RESET';
  reason: string;
}

/**
 * Error events
 */
export interface ComponentErrorEvent extends BasePatternEvent {
  type: 'COMPONENT_ERROR';
  component: string;
  error: Error;
  context?: any;
}

export interface ServiceErrorEvent extends BasePatternEvent {
  type: 'SERVICE_ERROR';
  service: string;
  method: string;
  error: Error;
  parameters?: any;
}

/**
 * Lifecycle events
 */
export interface ComponentInitializedEvent extends BasePatternEvent {
  type: 'COMPONENT_INITIALIZED';
  component: string;
}

export interface ComponentDestroyedEvent extends BasePatternEvent {
  type: 'COMPONENT_DESTROYED';
  component: string;
}

/**
 * User interaction events
 */
export interface UserInteractionEvent extends BasePatternEvent {
  type: 'USER_INTERACTION';
  action: string;
  target: string;
  data?: any;
}

export interface KeyboardShortcutEvent extends BasePatternEvent {
  type: 'KEYBOARD_SHORTCUT';
  key: string;
  modifiers: string[];
  action: string;
}

/**
 * Performance events
 */
export interface PerformanceEvent extends BasePatternEvent {
  type: 'PERFORMANCE';
  operation: string;
  duration: number;
  metadata?: any;
}

/**
 * Union type for all pattern creator events
 */
export type PatternCreatorEvent =
  | ToolbarSettingsChangeEvent
  | ToolbarModeToggleEvent
  | ToolbarPointActionEvent
  | ToolbarPatternActionEvent
  | CanvasPointPlacedEvent
  | CanvasPointSelectedEvent
  | CanvasPointMovedEvent
  | CanvasStateChangedEvent
  | CanvasZoomEvent
  | CanvasPanEvent
  | ValidationErrorEvent
  | ValidationSuccessEvent
  | StateUpdateEvent
  | StateResetEvent
  | ComponentErrorEvent
  | ServiceErrorEvent
  | ComponentInitializedEvent
  | ComponentDestroyedEvent
  | UserInteractionEvent
  | KeyboardShortcutEvent
  | PerformanceEvent;

/**
 * Event handler type definitions
 */
export type PatternEventHandler<T extends PatternCreatorEvent = PatternCreatorEvent> = (event: T) => void;

/**
 * Event emitter interface for components
 */
export interface IPatternEventEmitter {
  emit<T extends PatternCreatorEvent>(event: T): void;
  subscribe<T extends PatternCreatorEvent>(
    eventType: T['type'],
    handler: PatternEventHandler<T>
  ): () => void;
}

/**
 * Event bus interface for cross-component communication
 */
export interface IPatternEventBus extends IPatternEventEmitter {
  clear(): void;
  getEventHistory(): PatternCreatorEvent[];
  enableLogging(enabled: boolean): void;
}