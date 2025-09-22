import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { 
  SimulationState, 
  SimulationEvent, 
  SimulationEventType,
  HoleAnimationState,
  ConnectionAnimationState,
  AnimationFrame,
  BlastEffect,
  BlastEffectType
} from '../../shared/models/simulation.model';
import { BlastConnection } from '../../drilling-pattern-creator/models/drill-point.model';

@Injectable({
  providedIn: 'root'
})
export class AnimationService {
  private animationFrame$ = new BehaviorSubject<AnimationFrame>({
    time: 0,
    holeStates: new Map(),
    connectionStates: new Map(),
    activeEffects: []
  });
  private animationFrameId: number | null = null;
  private animationStartTime: number = 0;
  private activeEffects: BlastEffect[] = [];
  private lastFrameTime: number = 0;
  private holeStartTimes: Map<string, number> = new Map();
  private connectionStartTimes: Map<string, number> = new Map();
  private totalDurationMs: number = 0;

  constructor() {}

  getAnimationFrame(): Observable<AnimationFrame> {
    return this.animationFrame$.asObservable();
  }

  startAnimation(state: SimulationState, connections: BlastConnection[], drillPoints?: any[]): void {
    if (this.animationFrameId) {
      cancelAnimationFrame(this.animationFrameId);
    }

    // Build propagation schedule based on connection topology and starting hole location
    this.buildPropagationSchedule(connections, drillPoints);

    // Maximum duration for stopping criteria
    this.totalDurationMs = Math.max(
      ...Array.from(this.holeStartTimes.values()).map(t => t),
      ...connections.map(c => (this.connectionStartTimes.get(c.id) || 0) + (c.delay || 0))
    );

    this.animationStartTime = performance.now();
    this.lastFrameTime = 0;
    this.animate(state, connections);
  }

  stopAnimation(): void {
    if (this.animationFrameId) {
      cancelAnimationFrame(this.animationFrameId);
      this.animationFrameId = null;
    }
  }

  resetAnimation(): void {
    this.animationFrame$.next({
      time: 0,
      holeStates: new Map(),
      connectionStates: new Map(),
      activeEffects: []
    });
    this.activeEffects = [];
  }

  private animate(state: SimulationState, connections: BlastConnection[]): void {
    const currentTime = performance.now() - this.animationStartTime;
    const deltaTime = currentTime - this.lastFrameTime;
    this.lastFrameTime = currentTime;

    const frame: AnimationFrame = {
      time: currentTime,
      holeStates: new Map(),
      connectionStates: new Map(),
      activeEffects: this.activeEffects
    };

    // Update connection states
    connections.forEach(connection => {
      if (connection.id) {
        const state = this.calculateConnectionState(connection, currentTime);
        frame.connectionStates.set(connection.id, state);
      }
    });

    // Update hole states
    connections.forEach(connection => {
      if (connection.point1DrillPointId && connection.point2DrillPointId) {
        const fromState = this.calculateHoleState(connection.point1DrillPointId, currentTime, connections);
        const toState = this.calculateHoleState(connection.point2DrillPointId, currentTime, connections);
        frame.holeStates.set(connection.point1DrillPointId, fromState);
        frame.holeStates.set(connection.point2DrillPointId, toState);
      }
    });

    // Update effects
    this.updateEffects(currentTime);

    this.animationFrame$.next(frame);

    if (currentTime < this.totalDurationMs + 500) { // add small buffer
      this.animationFrameId = requestAnimationFrame(() => this.animate(state, connections));
    } else {
      this.stopAnimation();
    }
  }

  private buildPropagationSchedule(connections: BlastConnection[], drillPoints?: any[]): void {
    this.holeStartTimes.clear();
    this.connectionStartTimes.clear();

    const queue: Array<{holeId: string, start: number}> = [];

    // First priority: Find connections marked as starting holes (IsStartingHole = true)
    const startingConnections = connections.filter(conn => (conn as any).isStartingHole === true);
    
    if (startingConnections.length > 0) {
      // Use holes marked as starting holes - these represent the actual starting hole location
      startingConnections.forEach(conn => {
        // Get the actual starting hole ID from the connection
        const startingHoleId = conn.point1DrillPointId;
        
        if (!this.holeStartTimes.has(startingHoleId)) {
          this.holeStartTimes.set(startingHoleId, 0);
          queue.push({ holeId: startingHoleId, start: 0 });
          
          // Log for debugging - starting from actual hole location
          if (drillPoints) {
            const startingPoint = drillPoints.find(p => p.id === startingHoleId);
            if (startingPoint) {
              console.log(`Starting simulation from hole ${startingHoleId} at coordinates (${startingPoint.x}, ${startingPoint.y})`);
            }
          }
        }
      });
    } else {
      // Fallback: Determine holes that are never a toHoleId (no incoming connections)
      const allToHoles = new Set(connections.map(c => c.point2DrillPointId));

      connections.forEach(conn => {
        if (!allToHoles.has(conn.point1DrillPointId) && !this.holeStartTimes.has(conn.point1DrillPointId)) {
          this.holeStartTimes.set(conn.point1DrillPointId, 0);
          queue.push({ holeId: conn.point1DrillPointId, start: 0 });
        }
      });

      // Edge case: if every hole has incoming, choose the first connection's fromHole as root
      if (queue.length === 0 && connections.length > 0) {
        const root = connections[0].point1DrillPointId;
        this.holeStartTimes.set(root, 0);
        queue.push({ holeId: root, start: 0 });
      }
    }

    // BFS/DFS propagation from the actual starting hole location
    while (queue.length > 0) {
      const { holeId, start } = queue.shift()!;

      // For every outgoing connection from this hole
      connections.filter(c => c.point1DrillPointId === holeId).forEach(conn => {
        const connStart = start;
        const connEnd = connStart + (conn.delay || 0);
        const nextHoleBlast = connEnd + 500; // extra 500ms after arrival
        this.connectionStartTimes.set(conn.id, connStart);

        const existingHoleTime = this.holeStartTimes.get(conn.point2DrillPointId);
        if (existingHoleTime === undefined || nextHoleBlast < existingHoleTime) {
          this.holeStartTimes.set(conn.point2DrillPointId, nextHoleBlast);
          queue.push({ holeId: conn.point2DrillPointId, start: nextHoleBlast });
        }
      });
    }
  }

  private calculateConnectionState(connection: BlastConnection, currentTime: number): ConnectionAnimationState {
    const startTime = this.holeStartTimes.get(connection.point1DrillPointId) || 0;
    const endTime = startTime + (connection.delay || 0);
    const activationWindow = 100; // ms window for activation

    if (currentTime < startTime) {
      return ConnectionAnimationState.INACTIVE;
    } else if (currentTime >= startTime && currentTime < endTime) {
      return ConnectionAnimationState.SIGNAL_PROPAGATING;
    } else {
      return ConnectionAnimationState.SIGNAL_TRANSMITTED;
    }
  }

  private calculateHoleState(holeId: string, currentTime: number, connections: BlastConnection[]): HoleAnimationState {
    const blastTime = this.holeStartTimes.get(holeId);
    if (blastTime === undefined) {
      return HoleAnimationState.READY;
    }

    const activationWindow = 100; // ms window for activation

    if (currentTime < blastTime) {
      return HoleAnimationState.READY;
    } else if (currentTime >= blastTime && currentTime < blastTime + activationWindow) {
      return HoleAnimationState.DETONATING;
    } else {
      return HoleAnimationState.BLASTED;
    }
  }

  private updateEffects(currentTime: number): void {
    // Remove expired effects
    this.activeEffects = this.activeEffects.filter(effect => 
      currentTime < effect.startTime + effect.duration
    );
  }
}