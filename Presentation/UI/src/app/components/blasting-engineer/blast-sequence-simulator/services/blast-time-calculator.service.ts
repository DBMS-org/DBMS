import { Injectable } from '@angular/core';
import { BlastConnection } from '../../drilling-pattern-creator/models/drill-point.model';

@Injectable({
  providedIn: 'root'
})
export class BlastTimeCalculatorService {

  calculateBlastSequenceTime(connections: BlastConnection[]): number {
    if (connections.length === 0) return 0;

    // Group connections by their starting hole
    const connectionsByFromHole = new Map<string, BlastConnection[]>();
    connections.forEach(conn => {
      if (!connectionsByFromHole.has(conn.point1DrillPointId)) {
        connectionsByFromHole.set(conn.point1DrillPointId, []);
      }
      connectionsByFromHole.get(conn.point1DrillPointId)!.push(conn);
    });

    // Find starting holes (holes that are not the destination of any connection)
    const allToHoleIds = new Set(connections.map(c => c.point2DrillPointId));
    const startingHoles = Array.from(new Set(connections.map(c => c.point1DrillPointId))).filter(holeId => !allToHoleIds.has(holeId));

    let maxTime = 0;
    const visited = new Set<string>();

    // Traverse from each starting hole
    const traverse = (holeId: string, accumulatedDelay: number, path: Set<string>) => {
      if (path.has(holeId)) return; // Prevent cycles

      const newPath = new Set(path);
      newPath.add(holeId);
      
      maxTime = Math.max(maxTime, accumulatedDelay);
      
      const outgoingConnections = connectionsByFromHole.get(holeId) || [];
      outgoingConnections.forEach(conn => {
        traverse(conn.point2DrillPointId, accumulatedDelay + conn.delay, newPath);
      });
    };

    startingHoles.forEach(startingHole => {
      if (!visited.has(startingHole)) {
        traverse(startingHole, 0, new Set());
      }
    });

    return maxTime;
  }

  getTotalHoles(connections: BlastConnection[]): number {
    const uniqueHoles = new Set<string>();
    connections.forEach(conn => {
      uniqueHoles.add(conn.point1DrillPointId);
      uniqueHoles.add(conn.point2DrillPointId);
    });
    return uniqueHoles.size;
  }

  getAverageDelayBetweenDetonations(connections: BlastConnection[]): number {
    if (connections.length === 0) return 0;
    
    const totalDelay = connections.reduce((sum, conn) => sum + conn.delay, 0);
    return totalDelay / connections.length;
  }

  getConnectionsPerHole(connections: BlastConnection[]): number {
    if (connections.length === 0) return 0;
    
    const uniqueHoles = new Set<string>();
    connections.forEach(conn => {
      uniqueHoles.add(conn.point1DrillPointId);
      uniqueHoles.add(conn.point2DrillPointId);
    });
    
    return connections.length / uniqueHoles.size;
  }

  analyzeSequenceTiming(connections: BlastConnection[]): any {
    if (connections.length === 0) return { waves: [], totalTime: 0 };

    // Group connections by their starting hole
    const connectionsByFromHole = new Map<string, BlastConnection[]>();
    connections.forEach(conn => {
      const fromConnections = connectionsByFromHole.get(conn.point1DrillPointId) || [];
      fromConnections.push(conn);
      connectionsByFromHole.set(conn.point1DrillPointId, fromConnections);
    });

    // Find initial connections (not targeted by other connections)
    const targetHoles = new Set(connections.map(c => c.point2DrillPointId));
    const initialConnections = connections.filter(c => !targetHoles.has(c.point1DrillPointId));

    const waves: any[] = [];
    let currentTime = 0;
    let currentWave = initialConnections.slice();
    let waveNumber = 1;

    while (currentWave.length > 0) {
      const nextWave: BlastConnection[] = [];
      
      currentWave.forEach(connection => {
        const nextConnections = connectionsByFromHole.get(connection.point2DrillPointId) || [];
        nextWave.push(...nextConnections);
      });

      waves.push({
        wave: waveNumber,
        connections: currentWave.length,
        time: currentTime,
        holes: currentWave.map(c => ({ from: c.point1DrillPointId, to: c.point2DrillPointId }))
      });

      // Calculate time for next wave
      if (nextWave.length > 0) {
        const avgDelay = currentWave.reduce((sum, conn) => sum + conn.delay, 0) / currentWave.length;
        currentTime += avgDelay;
      }

      currentWave = nextWave;
      waveNumber++;
    }

    return {
      waves,
      totalTime: currentTime,
      totalWaves: waves.length
    };
  }

  calculateWaveBasedTiming(connections: BlastConnection[]): any {
    if (connections.length === 0) return { waves: [], totalTime: 0 };

    // Group connections by their starting hole
    const connectionsByFromHole = new Map<string, BlastConnection[]>();
    connections.forEach(conn => {
      if (!connectionsByFromHole.has(conn.point1DrillPointId)) {
        connectionsByFromHole.set(conn.point1DrillPointId, []);
      }
      connectionsByFromHole.get(conn.point1DrillPointId)!.push(conn);
    });

    // Find starting holes (holes that are not the destination of any connection)
    const allToHoleIds = new Set(connections.map(c => c.point2DrillPointId));
    const initialFromHoles = Array.from(new Set(connections.map(c => c.point1DrillPointId))).filter(holeId => !allToHoleIds.has(holeId));

    const waves: any[] = [];
    let currentTime = 0;
    let currentFromHoles = initialFromHoles.slice();
    let waveNumber = 1;
    const processedConnections = new Set<string>();

    while (currentFromHoles.length > 0) {
      const currentWaveConnections: BlastConnection[] = [];
      const nextFromHoles: string[] = [];

      currentFromHoles.forEach(fromHole => {
        const connections = connectionsByFromHole.get(fromHole) || [];
        connections.forEach(conn => {
          if (!processedConnections.has(conn.id)) {
            currentWaveConnections.push(conn);
            processedConnections.add(conn.id);
            nextFromHoles.push(conn.point2DrillPointId);
          }
        });
      });

      if (currentWaveConnections.length > 0) {
        waves.push({
          wave: waveNumber,
          connections: currentWaveConnections.length,
          time: currentTime,
          holes: currentWaveConnections.map(c => ({ from: c.point1DrillPointId, to: c.point2DrillPointId }))
        });

        // Calculate time for next wave (average delay of current wave)
        const avgDelay = currentWaveConnections.reduce((sum, conn) => sum + conn.delay, 0) / currentWaveConnections.length;
        currentTime += avgDelay;
      }

      currentFromHoles = Array.from(new Set(nextFromHoles));
      waveNumber++;
    }

    return {
      waves,
      totalTime: currentTime,
      totalWaves: waves.length
    };
  }
} 