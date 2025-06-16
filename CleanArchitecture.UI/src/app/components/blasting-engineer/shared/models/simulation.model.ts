export interface SimulationState {
  isPlaying: boolean;
  isPaused: boolean;
  currentTime: number; // milliseconds
  totalDuration: number; // milliseconds
  playbackSpeed: number; // 0.25x to 4x
  currentStep: number;
  totalSteps: number;
}

export interface AnimationFrame {
  time: number;
  holeStates: Map<string, HoleAnimationState>;
  connectionStates: Map<string, ConnectionAnimationState>;
  activeEffects: BlastEffect[];
}

export enum HoleAnimationState {
  READY = 'ready',
  DETONATING = 'detonating', 
  BLASTED = 'blasted'
}

export enum ConnectionAnimationState {
  INACTIVE = 'inactive',
  SIGNAL_PROPAGATING = 'signal_propagating',
  SIGNAL_TRANSMITTED = 'signal_transmitted'
}

export interface BlastEffect {
  id: string;
  type: BlastEffectType;
  holeId: string;
  startTime: number;
  duration: number;
  intensity: number;
  radius: number;
  position: { x: number; y: number };
}

export enum BlastEffectType {
  EXPLOSION = 'explosion',
  SHOCKWAVE = 'shockwave',
  DEBRIS = 'debris',
  SMOKE = 'smoke'
}

export interface SimulationEvent {
  time: number;
  type: SimulationEventType;
  targetId: string;
  data: {
    connectionId?: string;
    toHoleId?: string;
    wireSequence?: string;
    triggeredByWire?: string;
    [key: string]: any;
  };
}

export enum SimulationEventType {
  SIGNAL_START = 'signal_start',
  SIGNAL_ARRIVE = 'signal_arrive',
  HOLE_DETONATE = 'hole_detonate',
  EFFECT_START = 'effect_start'
}

export interface SimulationSettings {
  showTiming: boolean;
  showConnections: boolean;
  showEffects: boolean;
  showSequenceNumbers: boolean;
  effectIntensity: number; // 0-100%
  animationQuality: 'low' | 'medium' | 'high';
}

export interface ViewSettings {
  showGrid: boolean;
  colorTheme: 'default' | 'dark' | 'high-contrast' | 'colorblind';
  showHoleDetails: boolean;
  showConnectionLabels: boolean;
  highlightActiveConnections: boolean;
  frameRate: number;
}

export interface SimulationValidation {
  isValid: boolean;
  warnings: ValidationWarning[];
  errors: ValidationError[];
  suggestions: ValidationSuggestion[];
}

export interface ValidationWarning {
  type: string;
  message: string;
  affectedHoles: string[];
  severity: 'low' | 'medium' | 'high';
}

export interface ValidationError {
  type: string;
  message: string;
  affectedElements: string[];
  fixSuggestion: string;
}

export interface ValidationSuggestion {
  type: string;
  message: string;
  priority: 'low' | 'medium' | 'high';
}

export interface OptimizationSuggestion {
  type: string;
  message: string;
  priority: 'low' | 'medium' | 'high';
  potentialImprovement: number; // 0-100%
  affectedElements: string[];
}

export interface SimulationMetrics {
  totalBlastTime: number;
  averageDelayBetweenHoles: number;
  maxSimultaneousDetonations: number;
  efficiencyScore: number; // 0-100
  safetyScore: number; // 0-100
  connectionUtilization: number; // 0-100%
}

export interface TimelineMarker {
  time: number;
  type: 'hole_blast' | 'milestone' | 'sequence_start' | 'sequence_end';
  label: string;
  color: string;
} 