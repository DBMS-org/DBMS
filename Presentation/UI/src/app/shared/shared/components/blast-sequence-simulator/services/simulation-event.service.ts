import { Injectable } from '@angular/core';
import { BlastConnection } from '../../../../core/models/site-blasting.model';
import { SimulationEvent, SimulationEventType, BlastEffectType } from '../../shared/models/simulation.model';

@Injectable({
  providedIn: 'root'
})
export class SimulationEventService {
  generateSimulationEvents(connections: BlastConnection[]): SimulationEvent[] {
    const events: SimulationEvent[] = [];
    const processedConnections = new Set<string>();
    const connectionsByFromHole = new Map<string, BlastConnection[]>();

    // Build connections map
    connections.forEach(conn => {
      const fromConnections = connectionsByFromHole.get(conn.fromHoleId) || [];
      fromConnections.push(conn);
      connectionsByFromHole.set(conn.fromHoleId, fromConnections);
    });

    // Find initial connections
    const targetHoles = new Set(connections.map(c => c.toHoleId));
    const initialConnections = connections.filter(c => !targetHoles.has(c.fromHoleId));

    // Process waves
    const processWave = (connectionsToProcess: BlastConnection[], waveStartTime: number) => {
      if (connectionsToProcess.length === 0) return;

      // Mark connections as processed
      connectionsToProcess.forEach(conn => {
        processedConnections.add(conn.id);
      });

      // Process each connection in the wave
      connectionsToProcess.forEach(connection => {
        // Signal propagation (25ms)
        const signalPropagationTime = waveStartTime + 25;
        events.push({
          time: signalPropagationTime,
          type: SimulationEventType.SIGNAL_ARRIVE,
          targetId: connection.id,
          data: { 
            fromHoleId: connection.fromHoleId, 
            toHoleId: connection.toHoleId,
            wireSequence: connection.sequence.toString()
          }
        });

        // Hidden point 2 (end point) activation (35ms total)
        const endPointActivationTime = signalPropagationTime + 10;
        events.push({
          time: endPointActivationTime,
          type: SimulationEventType.SIGNAL_ARRIVE,
          targetId: `${connection.id}_end`,
          data: { 
            connectionId: connection.id,
            pointType: 'endPoint',
            pointLabel: '2',
            position: connection.endPoint,
            toHoleId: connection.toHoleId
          }
        });

        // Hole detonation event (50ms total)
        const detonationTime = endPointActivationTime + 15;
        events.push({
          time: detonationTime,
          type: SimulationEventType.HOLE_DETONATE,
          targetId: connection.toHoleId,
          data: { 
            delay: connection.delay, 
            type: connection.connectorType,
            wireSequence: connection.sequence.toString(),
            triggeredByWire: connection.id
          }
        });

        // Blast effect
        events.push({
          time: detonationTime,
          type: SimulationEventType.EFFECT_START,
          targetId: connection.toHoleId,
          data: { 
            effectType: BlastEffectType.EXPLOSION, 
            duration: 1000,
            triggeredByWire: connection.id
          }
        });
      });

      // Find next wave
      const nextWaveConnections: BlastConnection[] = [];
      connectionsToProcess.forEach(connection => {
        const potentialNextConnections = connectionsByFromHole.get(connection.toHoleId) || [];
        potentialNextConnections.forEach(nextConn => {
          if (!processedConnections.has(nextConn.id) && !nextWaveConnections.includes(nextConn)) {
            nextWaveConnections.push(nextConn);
          }
        });
      });

      // Process next wave with 500ms delay
      if (nextWaveConnections.length > 0) {
        const nextWaveTime = waveStartTime + 500;
        processWave(nextWaveConnections, nextWaveTime);
      }
    };

    // Start processing waves
    const initialStartTime = initialConnections.length > 0 ? 
      Math.min(...initialConnections.map(conn => conn.delay)) : 0;
    
    processWave(initialConnections, initialStartTime);

    // Sort events by time
    return events.sort((a, b) => a.time - b.time);
  }

  detectTimingConflicts(events: SimulationEvent[]): Array<{time: number; severity: 'high' | 'medium'}> {
    const conflicts: Array<{time: number; severity: 'high' | 'medium'}> = [];
    const detonationTimes = new Map<number, number>();

    // Count detonations at each time
    events.forEach(event => {
      if (event.type === SimulationEventType.HOLE_DETONATE) {
        const count = detonationTimes.get(event.time) || 0;
        detonationTimes.set(event.time, count + 1);
      }
    });

    // Check for conflicts
    detonationTimes.forEach((count, time) => {
      if (count > 3) {
        conflicts.push({ time, severity: 'high' });
      } else if (count > 1) {
        conflicts.push({ time, severity: 'medium' });
      }
    });

    return conflicts;
  }
} 