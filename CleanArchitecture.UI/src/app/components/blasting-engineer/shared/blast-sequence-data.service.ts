import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { 
  PatternData, 
  DrillPoint, 
  BlastConnection, 
  BlastSequenceData,
  ConnectorType,
  DetonatorType 
} from '../drilling-pattern-creator/models/drill-point.model';
import { 
  SimulationState, 
  SimulationSettings, 
  SimulationValidation,
  SimulationMetrics,
  SimulationEvent,
  TimelineMarker,
  ValidationWarning,
  ValidationError,
  OptimizationSuggestion
} from './models/simulation.model';

export interface WorkflowStep {
  id: string;
  name: string;
  completed: boolean;
  current: boolean;
  enabled: boolean;
}

export interface ProjectData {
  id: string;
  name: string;
  description: string;
  createdAt: Date;
  updatedAt: Date;
  patternData: PatternData | null;
  connections: BlastConnection[];
  simulationSettings: SimulationSettings;
  metadata: {
    version: string;
    author: string;
    notes: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class BlastSequenceDataService {
  
  // Pattern Data
  private patternDataSubject = new BehaviorSubject<PatternData | null>(null);
  public patternData$ = this.patternDataSubject.asObservable();

  // Connections Data
  private connectionsSubject = new BehaviorSubject<BlastConnection[]>([]);
  public connections$ = this.connectionsSubject.asObservable();

  // Simulation State
  private simulationStateSubject = new BehaviorSubject<SimulationState>({
    isPlaying: false,
    isPaused: false,
    currentTime: 0,
    totalDuration: 0,
    playbackSpeed: 1,
    currentStep: 0,
    totalSteps: 0
  });
  public simulationState$ = this.simulationStateSubject.asObservable();

  // Simulation Settings
  private simulationSettingsSubject = new BehaviorSubject<SimulationSettings>({
    showTiming: true,
    showConnections: true,
    showEffects: true,
    showSequenceNumbers: true,
    effectIntensity: 75,
    animationQuality: 'medium'
  });
  public simulationSettings$ = this.simulationSettingsSubject.asObservable();

  // Workflow State
  private workflowStepsSubject = new BehaviorSubject<WorkflowStep[]>([
    { id: 'pattern', name: 'Create Pattern', completed: false, current: true, enabled: true },
    { id: 'sequence', name: 'Design Sequence', completed: false, current: false, enabled: false },
    { id: 'simulate', name: 'Simulate & Validate', completed: false, current: false, enabled: false }
  ]);
  public workflowSteps$ = this.workflowStepsSubject.asObservable();

  // Current Project
  private currentProjectSubject = new BehaviorSubject<ProjectData | null>(null);
  public currentProject$ = this.currentProjectSubject.asObservable();

  // Validation Results
  private validationSubject = new BehaviorSubject<SimulationValidation>({
    isValid: true,
    warnings: [],
    errors: [],
    suggestions: []
  });
  public validation$ = this.validationSubject.asObservable();

  // Combined data observable for components that need everything
  public blastSequenceData$ = combineLatest([
    this.patternData$,
    this.connections$,
    this.simulationState$,
    this.simulationSettings$
  ]).pipe(
    map(([patternData, connections, simulationState, simulationSettings]) => ({
      patternData,
      connections,
      simulationState,
      simulationSettings
    }))
  );

  constructor() {}

  // Pattern Data Methods
  setPatternData(patternData: PatternData): void {
    this.patternDataSubject.next(patternData);
    this.updateWorkflowStep('pattern', true);
    this.enableWorkflowStep('sequence');
    this.validateSequence();
  }

  getPatternData(): PatternData | null {
    return this.patternDataSubject.value;
  }

  clearPatternData(): void {
    this.patternDataSubject.next(null);
    this.clearConnections();
    this.resetWorkflow();
  }

  // Connections Methods
  setConnections(connections: BlastConnection[]): void {
    // Migrate connections to ensure they have hidden points
    const migratedConnections = this.migrateConnectionsToNewFormat(connections);
    this.connectionsSubject.next([...migratedConnections]);
    this.updateWorkflowStep('sequence', migratedConnections.length > 0);
    this.enableWorkflowStep('simulate');
    this.validateSequence();
    this.calculateSimulationDuration();
  }

  private migrateConnectionsToNewFormat(connections: BlastConnection[]): BlastConnection[] {
    return connections.map(connection => {
      // Check if connection already has hidden points
      if (connection.startPoint && connection.endPoint) {
        return connection;
      }
      
      // Create default hidden points if they don't exist
      const fromHole = this.getPatternData()?.drillPoints.find(p => p.id === connection.fromHoleId);
      const toHole = this.getPatternData()?.drillPoints.find(p => p.id === connection.toHoleId);
      
      if (!fromHole || !toHole) {
        return connection;
      }
      
      // Calculate offset positions for hidden points
      const offsetDistance = 0.5; // world units
      const angle = Math.atan2(toHole.y - fromHole.y, toHole.x - fromHole.x);
      
      return {
        ...connection,
        startPoint: {
          id: `start_${connection.id}`,
          label: "1",
          x: fromHole.x - Math.cos(angle) * offsetDistance,
          y: fromHole.y - Math.sin(angle) * offsetDistance,
          isHidden: true
        },
        endPoint: {
          id: `end_${connection.id}`,
          label: "2",
          x: toHole.x + Math.cos(angle) * offsetDistance,
          y: toHole.y + Math.sin(angle) * offsetDistance,
          isHidden: true
        }
      };
    });
  }

  addConnection(connection: BlastConnection): void {
    const current = this.connectionsSubject.value;
    this.setConnections([...current, connection]);
  }

  removeConnection(connectionId: string): void {
    const current = this.connectionsSubject.value;
    const filtered = current.filter(c => c.id !== connectionId);
    this.setConnections(filtered);
  }

  updateConnection(updatedConnection: BlastConnection): void {
    const current = this.connectionsSubject.value;
    const index = current.findIndex(c => c.id === updatedConnection.id);
    if (index !== -1) {
      const updated = [...current];
      updated[index] = updatedConnection;
      this.setConnections(updated);
    }
  }

  getConnections(): BlastConnection[] {
    return this.connectionsSubject.value;
  }

  clearConnections(): void {
    this.connectionsSubject.next([]);
    this.updateWorkflowStep('sequence', false);
    this.disableWorkflowStep('simulate');
  }

  // Simulation State Methods
  updateSimulationState(updates: Partial<SimulationState>): void {
    const current = this.simulationStateSubject.value;
    this.simulationStateSubject.next({ ...current, ...updates });
  }

  resetSimulation(): void {
    const current = this.simulationStateSubject.value;
    this.simulationStateSubject.next({
      ...current,
      isPlaying: false,
      isPaused: false,
      currentTime: 0,
      currentStep: 0
    });
  }

  // Simulation Settings Methods
  updateSimulationSettings(updates: Partial<SimulationSettings>): void {
    const current = this.simulationSettingsSubject.value;
    this.simulationSettingsSubject.next({ ...current, ...updates });
  }

  // Workflow Methods
  private updateWorkflowStep(stepId: string, completed: boolean): void {
    const steps = this.workflowStepsSubject.value.map(step => 
      step.id === stepId ? { ...step, completed } : step
    );
    this.workflowStepsSubject.next(steps);
  }

  private enableWorkflowStep(stepId: string): void {
    const steps = this.workflowStepsSubject.value.map(step => 
      step.id === stepId ? { ...step, enabled: true } : step
    );
    this.workflowStepsSubject.next(steps);
  }

  private disableWorkflowStep(stepId: string): void {
    const steps = this.workflowStepsSubject.value.map(step => 
      step.id === stepId ? { ...step, enabled: false, completed: false } : step
    );
    this.workflowStepsSubject.next(steps);
  }

  setCurrentWorkflowStep(stepId: string): void {
    const steps = this.workflowStepsSubject.value.map(step => ({
      ...step,
      current: step.id === stepId
    }));
    this.workflowStepsSubject.next(steps);
  }

  getWorkflowSteps(): WorkflowStep[] {
    return this.workflowStepsSubject.value;
  }

  updateValidation(validation: SimulationValidation): void {
    this.validationSubject.next(validation);
  }

  private resetWorkflow(): void {
    const steps: WorkflowStep[] = [
      { id: 'pattern', name: 'Create Pattern', completed: false, current: true, enabled: true },
      { id: 'sequence', name: 'Design Sequence', completed: false, current: false, enabled: false },
      { id: 'simulate', name: 'Simulate & Validate', completed: false, current: false, enabled: false }
    ];
    this.workflowStepsSubject.next(steps);
  }

  // Validation Methods
  private validateSequence(): void {
    const patternData = this.patternDataSubject.value;
    const connections = this.connectionsSubject.value;

    if (!patternData || connections.length === 0) {
      this.validationSubject.next({
        isValid: true,
        warnings: [],
        errors: [],
        suggestions: []
      });
      return;
    }

    const warnings: ValidationWarning[] = [];
    const errors: ValidationError[] = [];
    const suggestions: OptimizationSuggestion[] = [];

    // Check for timing overlaps
    const timingConflicts = this.detectTimingConflicts(connections);
    warnings.push(...timingConflicts);

    // Check for orphaned holes
    const orphanedHoles = this.detectOrphanedHoles(patternData.drillPoints, connections);
    if (orphanedHoles.length > 0) {
      warnings.push({
        type: 'connection_missing',
        message: `${orphanedHoles.length} holes are not connected to the blast sequence`,
        affectedHoles: orphanedHoles,
        severity: 'medium'
      });
    }

    // Check for optimization opportunities
    const optimizations = this.generateOptimizationSuggestions(connections);
    suggestions.push(...optimizations);

    this.validationSubject.next({
      isValid: errors.length === 0,
      warnings,
      errors,
      suggestions
    });
  }

  private detectTimingConflicts(connections: BlastConnection[]): ValidationWarning[] {
    const warnings: ValidationWarning[] = [];
    const timeGroups = new Map<number, BlastConnection[]>();

    // Group connections by delay time
    connections.forEach(conn => {
      if (!timeGroups.has(conn.delay)) {
        timeGroups.set(conn.delay, []);
      }
      timeGroups.get(conn.delay)!.push(conn);
    });

    // Check for suspicious simultaneous detonations
    timeGroups.forEach((conns, delay) => {
      if (conns.length > 5) { // More than 5 simultaneous detonations might be unsafe
        warnings.push({
          type: 'timing_overlap',
          message: `${conns.length} holes are set to detonate simultaneously at ${delay}ms`,
          affectedHoles: conns.flatMap(c => [c.fromHoleId, c.toHoleId]),
          severity: 'high'
        });
      }
    });

    return warnings;
  }

  private detectOrphanedHoles(holes: DrillPoint[], connections: BlastConnection[]): string[] {
    const connectedHoles = new Set<string>();
    connections.forEach(conn => {
      connectedHoles.add(conn.fromHoleId);
      connectedHoles.add(conn.toHoleId);
    });

    return holes
      .filter(hole => !connectedHoles.has(hole.id))
      .map(hole => hole.id);
  }

  private generateOptimizationSuggestions(connections: BlastConnection[]): OptimizationSuggestion[] {
    const suggestions: OptimizationSuggestion[] = [];
    
    if (connections.length > 0) {
      const maxDelay = Math.max(...connections.map(c => c.delay));
      const totalTime = maxDelay + 100; // Add some buffer
      
      if (totalTime > 5000) { // More than 5 seconds
        suggestions.push({
          type: 'reduce_total_time',
          message: 'Consider reducing delay times to improve blast efficiency',
          potentialImprovement: `Could reduce total blast time by ${Math.round((totalTime - 3000) / 1000 * 10) / 10}s`,
          implementationHint: 'Review delay values and optimize sequence timing'
        });
      }
    }

    return suggestions;
  }

  private calculateSimulationDuration(): void {
    const connections = this.connectionsSubject.value;
    if (connections.length === 0) {
      this.updateSimulationState({ totalDuration: 0, totalSteps: 0 });
      return;
    }

    const maxDelay = Math.max(...connections.map(c => c.delay));
    const totalDuration = maxDelay + 2000; // Add 2 seconds for final effects
    const totalSteps = connections.length;

    this.updateSimulationState({ totalDuration, totalSteps });
  }

  // Export Methods
  exportBlastSequenceData(): BlastSequenceData {
    const patternData = this.patternDataSubject.value;
    const connections = this.connectionsSubject.value;

    if (!patternData) {
      throw new Error('No pattern data available for export');
    }

    return {
      patternData,
      connections,
      detonators: [], // You might want to populate this based on connections
      metadata: {
        exportedAt: new Date().toISOString(),
        version: '1.0',
        totalSequenceTime: Math.max(...connections.map(c => c.delay), 0)
      }
    };
  }

  // Timeline Methods for Simulator
  generateTimelineMarkers(): TimelineMarker[] {
    const connections = this.connectionsSubject.value;
    const markers: TimelineMarker[] = [];

    // Add start marker
    markers.push({
      time: 0,
      label: 'Blast Start',
      type: 'sequence_start',
      color: '#4CAF50'
    });

    // Add markers for each unique delay time
    const uniqueDelays = [...new Set(connections.map(c => c.delay))].sort((a, b) => a - b);
    
    uniqueDelays.forEach((delay, index) => {
      const connectionsAtTime = connections.filter(c => c.delay === delay);
      markers.push({
        time: delay,
        label: `Detonation ${index + 1} (${connectionsAtTime.length} holes)`,
        type: 'hole_blast',
        color: '#FF5722'
      });
    });

    // Add end marker
    const maxDelay = Math.max(...connections.map(c => c.delay), 0);
    markers.push({
      time: maxDelay + 2000,
      label: 'Blast Complete',
      type: 'sequence_end',
      color: '#2196F3'
    });

    return markers;
  }

  // Metrics Calculation
  calculateSimulationMetrics(): SimulationMetrics {
    const connections = this.connectionsSubject.value;
    const patternData = this.patternDataSubject.value;

    if (connections.length === 0 || !patternData) {
      return {
        totalBlastTime: 0,
        averageDelayBetweenHoles: 0,
        maxSimultaneousDetonations: 0,
        efficiencyScore: 0,
        safetyScore: 100,
        connectionUtilization: 0
      };
    }

    const delays = connections.map(c => c.delay).sort((a, b) => a - b);
    const totalBlastTime = Math.max(...delays) + 1000; // Add 1s for final effects
    const averageDelayBetweenHoles = delays.length > 1 ? 
      (delays[delays.length - 1] - delays[0]) / (delays.length - 1) : 0;

    // Calculate simultaneous detonations
    const timeGroups = new Map<number, number>();
    connections.forEach(conn => {
      timeGroups.set(conn.delay, (timeGroups.get(conn.delay) || 0) + 1);
    });
    const maxSimultaneousDetonations = Math.max(...timeGroups.values());

    // Calculate efficiency (lower total time = higher efficiency)
    const efficiencyScore = Math.max(0, 100 - (totalBlastTime / 100)); // Simplified formula

    // Calculate safety score (fewer simultaneous detonations = safer)
    const safetyScore = Math.max(0, 100 - (maxSimultaneousDetonations * 10));

    // Calculate connection utilization
    const totalHoles = patternData.drillPoints.length;
    const connectedHoles = new Set([...connections.map(c => c.fromHoleId), ...connections.map(c => c.toHoleId)]);
    const connectionUtilization = (connectedHoles.size / totalHoles) * 100;

    return {
      totalBlastTime,
      averageDelayBetweenHoles,
      maxSimultaneousDetonations,
      efficiencyScore: Math.round(efficiencyScore),
      safetyScore: Math.round(safetyScore),
      connectionUtilization: Math.round(connectionUtilization)
    };
  }

  // Utility Methods
  private generateId(): string {
    return Date.now().toString(36) + Math.random().toString(36).substr(2);
  }
} 