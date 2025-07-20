/**
 * Pattern Canvas Component Exports
 * 
 * This file provides a centralized export point for the PatternCanvasComponent
 * and related types/interfaces.
 */

export { PatternCanvasComponent } from './pattern-canvas.component';

// Re-export related types for convenience
export type { PatternCanvasContract } from '../../contracts/component.contracts';
export type { 
  CanvasState, 
  UIState, 
  PlacePointEvent, 
  MovePointEvent 
} from '../../models/pattern-state.model';
export type { 
  PatternSettings, 
  DrillPoint 
} from '../../models/drill-point.model';