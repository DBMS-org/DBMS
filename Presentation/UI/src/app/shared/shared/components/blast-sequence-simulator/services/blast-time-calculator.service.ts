import { Injectable } from '@angular/core';
import { BlastConnection } from '../../../../core/models/site-blasting.model';

@Injectable({
  providedIn: 'root'
})
export class BlastTimeCalculatorService {
  calculateTotalBlastTime(connections: BlastConnection[]): number {
    if (connections.length === 0) {
      return 0;
    }

    // Build a map from fromHoleId to connections
    const connectionsByFromHole = new Map<string, BlastConnection[]>();
    connections.forEach(conn => {
      if (!connectionsByFromHole.has(conn.fromHoleId)) {
        connectionsByFromHole.set(conn.fromHoleId, []);
      }
      connectionsByFromHole.get(conn.fromHoleId)!.push(conn);
    });

    // Find initial holes (those that are not a toHoleId)
    const allToHoleIds = new Set(connections.map(c => c.toHoleId));
    const initialHoles = Array.from(new Set(connections.map(c => c.fromHoleId))).filter(holeId => !allToHoleIds.has(holeId));

    // Track the maximum detonation time
    let maxDetonationTime = 0;

    // Recursive function to traverse all paths
    function traverse(holeId: string, accumulatedDelay: number, visited: Set<string>) {
      // Prevent cycles
      if (visited.has(holeId)) return;
      visited.add(holeId);

      const outgoing = connectionsByFromHole.get(holeId) || [];
      if (outgoing.length === 0) {
        // Leaf node (no outgoing connections), update max detonation time
        if (accumulatedDelay > maxDetonationTime) {
          maxDetonationTime = accumulatedDelay;
        }
        return;
      }
      for (const conn of outgoing) {
        // For each outgoing connection, add its delay and traverse to its toHoleId
        traverse(conn.toHoleId, accumulatedDelay + conn.delay, new Set(visited));
      }
    }

    // Start traversal from each initial hole
    for (const holeId of initialHoles) {
      traverse(holeId, 0, new Set());
    }

    // Count unique holes (fromHoleId and toHoleId)
    const uniqueHoles = new Set<string>();
    connections.forEach(conn => {
      uniqueHoles.add(conn.fromHoleId);
      uniqueHoles.add(conn.toHoleId);
    });
    const holeCount = uniqueHoles.size;

    // Add 500ms per hole
    return maxDetonationTime + holeCount * 500;
  }

  calculateWaveMetrics(connections: BlastConnection[]): {
    totalBlastTime: number;
    averageDelayBetweenWaves: number;
    maxSimultaneousDetonations: number;
    waveCount: number;
  } {
    if (connections.length === 0) {
      return {
        totalBlastTime: 0,
        averageDelayBetweenWaves: 0,
        maxSimultaneousDetonations: 0,
        waveCount: 0
      };
    }

    const waves: BlastConnection[][] = [];
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
    const processWave = (connectionsToProcess: BlastConnection[], waveStartTime: number, waveNumber: number) => {
      if (connectionsToProcess.length === 0) return;

      waves.push(connectionsToProcess);
      connectionsToProcess.forEach(conn => processedConnections.add(conn.id));

      const nextWaveConnections: BlastConnection[] = [];
      connectionsToProcess.forEach(connection => {
        const potentialNextConnections = connectionsByFromHole.get(connection.toHoleId) || [];
        potentialNextConnections.forEach(nextConn => {
          if (!processedConnections.has(nextConn.id) && !nextWaveConnections.includes(nextConn)) {
            nextWaveConnections.push(nextConn);
          }
        });
      });

      if (nextWaveConnections.length > 0) {
        processWave(nextWaveConnections, waveStartTime + 500, waveNumber + 1);
      }
    };

    processWave(initialConnections, 0, 1);

    const totalBlastTime = this.calculateTotalBlastTime(connections);
    const maxSimultaneousDetonations = Math.max(...waves.map(wave => wave.length));
    const averageDelayBetweenWaves = waves.length > 1 ? 
      (totalBlastTime - 1500) / (waves.length - 1) : 0;

    return {
      totalBlastTime,
      averageDelayBetweenWaves,
      maxSimultaneousDetonations,
      waveCount: waves.length
    };
  }

  /**
   * Returns an array of detonation times for all leaf holes (holes with no outgoing connections)
   */
  getAllDetonationTimes(connections: BlastConnection[]): number[] {
    if (connections.length === 0) return [];

    const connectionsByFromHole = new Map<string, BlastConnection[]>();
    connections.forEach(conn => {
      if (!connectionsByFromHole.has(conn.fromHoleId)) {
        connectionsByFromHole.set(conn.fromHoleId, []);
      }
      connectionsByFromHole.get(conn.fromHoleId)!.push(conn);
    });

    const allToHoleIds = new Set(connections.map(c => c.toHoleId));
    const initialHoles = Array.from(new Set(connections.map(c => c.fromHoleId))).filter(holeId => !allToHoleIds.has(holeId));

    const detonationTimes: number[] = [];

    function traverse(holeId: string, accumulatedDelay: number, visited: Set<string>) {
      if (visited.has(holeId)) return;
      visited.add(holeId);
      const outgoing = connectionsByFromHole.get(holeId) || [];
      if (outgoing.length === 0) {
        detonationTimes.push(accumulatedDelay);
        return;
      }
      for (const conn of outgoing) {
        traverse(conn.toHoleId, accumulatedDelay + conn.delay, new Set(visited));
      }
    }
    for (const holeId of initialHoles) {
      traverse(holeId, 0, new Set());
    }
    return detonationTimes;
  }

  /**
   * Returns the average delay between detonations (sorted detonation times)
   */
  getAverageDelayBetweenDetonations(connections: BlastConnection[]): number {
    const times = this.getAllDetonationTimes(connections).sort((a, b) => a - b);
    if (times.length <= 1) return 0;
    let total = 0;
    for (let i = 1; i < times.length; i++) {
      total += times[i] - times[i - 1];
    }
    return total / (times.length - 1);
  }
} 