import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, combineLatest, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { 
  PatternData, 
  DrillPoint, 
  BlastConnection, 
  BlastSequenceData,
  ConnectorType,
  DetonatorType 
} from '../../drilling-pattern-creator/models/drill-point.model';
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
} from '../models/simulation.model';
import { SiteBlastingService } from '../../../../core/services/site-blasting.service';
import { BlastTimeCalculatorService } from '../../blast-sequence-simulator/services/blast-time-calculator.service';
import { ProjectService } from '../../../../core/services/project.service';
import { Project } from '../../../../core/models/project.model';

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

  // Site Context
  private currentSiteContext: { projectId: number; siteId: number } | null = null;

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

  constructor(
    private siteBlastingService: SiteBlastingService,
    private blastTimeCalculator: BlastTimeCalculatorService,
    private projectService: ProjectService
  ) {}

  // Site Context Methods
  setSiteContext(projectId: number, siteId: number): void {
    console.log('Setting site context from', this.currentSiteContext, 'to', { projectId, siteId });
    this.currentSiteContext = { projectId, siteId };
    // Fetch and emit project data
    this.projectService.getProject(projectId).subscribe({
      next: (project: Project) => {
        this.currentProjectSubject.next({
          id: project.id.toString(),
          name: project.name ?? '',
          description: project.description ?? '',
          createdAt: project.createdAt,
          updatedAt: project.updatedAt,
          patternData: null,
          connections: [],
          simulationSettings: this.simulationSettingsSubject.value,
          metadata: { version: '', author: '', notes: '' }
        });
      },
      error: (err) => {
        console.warn('Failed to load project data:', err);
        this.currentProjectSubject.next(null);
      }
    });
    // Load site-specific data here
    this.loadSiteData(projectId, siteId);
  }

  public getCurrentSiteContext(): { projectId: number; siteId: number } | null {
    return this.currentSiteContext;
  }

  // Utility method to clean up inconsistent data
  cleanupSiteData(projectId: number, siteId: number): void {
    const siteKey = `site_${projectId}_${siteId}`;
    
    console.log('🧹 DELETING ALL SITE DATA from backend and frontend for:', siteKey);
    
    // Delete all data from backend first
    this.siteBlastingService.deleteAllWorkflowData(projectId, siteId).subscribe({
      next: () => {
        console.log('✅ Successfully deleted all site data from backend');
      },
      error: (error) => {
        console.warn('⚠️ Failed to delete backend data, continuing with frontend cleanup:', error.message);
      }
    });

    // Also delete individual drill patterns and blast sequences
    this.siteBlastingService.getDrillPatterns(projectId, siteId).subscribe({
      next: (patterns) => {
        patterns.forEach(pattern => {
          this.siteBlastingService.deleteDrillPattern(projectId, siteId, pattern.id).subscribe({
            next: () => console.log(`✅ Deleted drill pattern ${pattern.id} from backend`),
            error: (error) => console.warn(`⚠️ Failed to delete drill pattern ${pattern.id}:`, error.message)
          });
        });
      },
      error: (error) => console.warn('⚠️ Failed to fetch drill patterns for deletion:', error.message)
    });

    this.siteBlastingService.getBlastSequences(projectId, siteId).subscribe({
      next: (sequences) => {
        sequences.forEach(sequence => {
          this.siteBlastingService.deleteBlastSequence(projectId, siteId, sequence.id).subscribe({
            next: () => console.log(`✅ Deleted blast sequence ${sequence.id} from backend`),
            error: (error) => console.warn(`⚠️ Failed to delete blast sequence ${sequence.id}:`, error.message)
          });
        });
      },
      error: (error) => console.warn('⚠️ Failed to fetch blast sequences for deletion:', error.message)
    });
    
    // Remove ALL site-specific localStorage data
    const keysToRemove = [
      `${siteKey}_pattern`,
      `${siteKey}_connections`,
      `${siteKey}_simulation_settings`,
      `${siteKey}_simulation_state`
    ];
    
    let itemsRemoved = 0;
    keysToRemove.forEach(key => {
      if (localStorage.getItem(key)) {
        localStorage.removeItem(key);
        itemsRemoved++;
        console.log('❌ Removed localStorage key:', key);
      }
    });
    
    console.log(`🗑️ Removed ${itemsRemoved} localStorage items`);
    
    // Clear current in-memory data if this is the active site
    if (this.currentSiteContext && 
        this.currentSiteContext.projectId === projectId && 
        this.currentSiteContext.siteId === siteId) {
      
             // Reset all data to initial state
       this.patternDataSubject.next(null);
       this.connectionsSubject.next([]);
       this.simulationStateSubject.next({
         isPlaying: false,
         isPaused: false,
         currentTime: 0,
         totalDuration: 0,
         playbackSpeed: 1,
         currentStep: 0,
         totalSteps: 0
       });
       this.simulationSettingsSubject.next({
         showTiming: true,
         showConnections: true,
         showEffects: true,
         showSequenceNumbers: true,
         effectIntensity: 75,
         animationQuality: 'medium'
       });
      
      // Reset workflow to initial state
      this.resetWorkflow();
      
      console.log('🔄 Reset all in-memory data for active site');
    }
    
    console.log('✅ Complete site data cleanup finished - ALL progress reset to 0%');
  }

  // Individual step cleanup methods
  cleanupPatternData(projectId: number, siteId: number): void {
    const siteKey = `site_${projectId}_${siteId}`;
    console.log('🧹 DELETING PATTERN data from backend and frontend for:', siteKey);
    
    // Delete drill patterns from backend
    this.siteBlastingService.getDrillPatterns(projectId, siteId).subscribe({
      next: (patterns) => {
        patterns.forEach(pattern => {
          this.siteBlastingService.deleteDrillPattern(projectId, siteId, pattern.id).subscribe({
            next: () => console.log(`✅ Deleted drill pattern ${pattern.id} from backend`),
            error: (error) => console.warn(`⚠️ Failed to delete drill pattern ${pattern.id}:`, error.message)
          });
        });
      },
      error: (error) => console.warn('⚠️ Failed to fetch drill patterns for deletion:', error.message)
    });

    // Delete workflow data for pattern
    this.siteBlastingService.deleteWorkflowData(projectId, siteId, 'pattern').subscribe({
      next: () => console.log('✅ Deleted pattern workflow data from backend'),
      error: (error) => console.warn('⚠️ Failed to delete pattern workflow data:', error.message)
    });
    
    // Remove pattern data from localStorage
    if (localStorage.getItem(`${siteKey}_pattern`)) {
      localStorage.removeItem(`${siteKey}_pattern`);
      console.log('❌ Removed pattern data from localStorage');
    }
    
    // Clear in-memory pattern data if this is the active site
    if (this.currentSiteContext && 
        this.currentSiteContext.projectId === projectId && 
        this.currentSiteContext.siteId === siteId) {
      this.patternDataSubject.next(null);
      this.updateWorkflowStep('pattern', false);
      this.disableWorkflowStep('sequence');
      this.disableWorkflowStep('simulate');
      console.log('🔄 Reset pattern data in memory');
    }
    
    console.log('✅ Pattern data deletion completed');
  }

  cleanupSequenceData(projectId: number, siteId: number): void {
    const siteKey = `site_${projectId}_${siteId}`;
    console.log('🧹 DELETING SEQUENCE data from backend and frontend for:', siteKey);
    
    // Delete blast sequences from backend
    this.siteBlastingService.getBlastSequences(projectId, siteId).subscribe({
      next: (sequences) => {
        sequences.forEach(sequence => {
          this.siteBlastingService.deleteBlastSequence(projectId, siteId, sequence.id).subscribe({
            next: () => console.log(`✅ Deleted blast sequence ${sequence.id} from backend`),
            error: (error) => console.warn(`⚠️ Failed to delete blast sequence ${sequence.id}:`, error.message)
          });
        });
      },
      error: (error) => console.warn('⚠️ Failed to fetch blast sequences for deletion:', error.message)
    });

    // Delete workflow data for connections
    this.siteBlastingService.deleteWorkflowData(projectId, siteId, 'connections').subscribe({
      next: () => console.log('✅ Deleted connections workflow data from backend'),
      error: (error) => console.warn('⚠️ Failed to delete connections workflow data:', error.message)
    });
    
    // Remove connections data from localStorage
    if (localStorage.getItem(`${siteKey}_connections`)) {
      localStorage.removeItem(`${siteKey}_connections`);
      console.log('❌ Removed connections data from localStorage');
    }
    
    // Clear in-memory connections data if this is the active site
    if (this.currentSiteContext && 
        this.currentSiteContext.projectId === projectId && 
        this.currentSiteContext.siteId === siteId) {
      this.connectionsSubject.next([]);
      this.updateWorkflowStep('sequence', false);
      this.disableWorkflowStep('simulate');
      console.log('🔄 Reset connections data in memory');
    }
    
    console.log('✅ Sequence data deletion completed');
  }

  cleanupSimulationData(projectId: number, siteId: number): void {
    const siteKey = `site_${projectId}_${siteId}`;
    console.log('🧹 DELETING SIMULATION data from backend and frontend for:', siteKey);
    
    // Delete simulation workflow data from backend
    this.siteBlastingService.deleteWorkflowData(projectId, siteId, 'simulation_settings').subscribe({
      next: () => console.log('✅ Deleted simulation settings from backend'),
      error: (error) => console.warn('⚠️ Failed to delete simulation settings:', error.message)
    });

    this.siteBlastingService.deleteWorkflowData(projectId, siteId, 'simulation_state').subscribe({
      next: () => console.log('✅ Deleted simulation state from backend'),
      error: (error) => console.warn('⚠️ Failed to delete simulation state:', error.message)
    });
    
    // Remove simulation data from localStorage
    const settingsKey = `${siteKey}_simulation_settings`;
    const stateKey = `${siteKey}_simulation_state`;
    
    if (localStorage.getItem(settingsKey)) {
      localStorage.removeItem(settingsKey);
      console.log('❌ Removed simulation settings from localStorage');
    }
    
    if (localStorage.getItem(stateKey)) {
      localStorage.removeItem(stateKey);
      console.log('❌ Removed simulation state from localStorage');
    }
    
    // Reset in-memory simulation data if this is the active site
    if (this.currentSiteContext && 
        this.currentSiteContext.projectId === projectId && 
        this.currentSiteContext.siteId === siteId) {
      this.simulationStateSubject.next({
        isPlaying: false,
        isPaused: false,
        currentTime: 0,
        totalDuration: 0,
        playbackSpeed: 1,
        currentStep: 0,
        totalSteps: 0
      });
      this.simulationSettingsSubject.next({
        showTiming: true,
        showConnections: true,
        showEffects: true,
        showSequenceNumbers: true,
        effectIntensity: 75,
        animationQuality: 'medium'
      });
      this.updateWorkflowStep('simulate', false);
      console.log('🔄 Reset simulation data in memory');
    }
    
    console.log('✅ Simulation data deletion completed');
  }

  getSiteWorkflowProgress(siteId: number): Observable<any> {
    if (!this.currentSiteContext) {
      return of({
        'pattern-creator': { completed: false, progress: 0 },
        'sequence-designer': { completed: false, progress: 0 },
        'simulator': { completed: false, progress: 0 }
      });
    }

    // Return an observable that checks both current data and backend data
    return new Observable(observer => {
      // Check current loaded data first
      const currentPattern = this.patternDataSubject.value;
      const currentConnections = this.connectionsSubject.value;
      const currentSimulationSettings = this.simulationSettingsSubject.value;
      
      // Pattern is complete if there are drill points
      const hasPattern = !!(currentPattern && currentPattern.drillPoints && currentPattern.drillPoints.length > 0);
      const patternProgress = hasPattern ? 100 : 0;
      
      // Sequence is complete if there are connections and pattern exists
      const hasConnections = hasPattern && currentConnections && currentConnections.length > 0;
      const sequenceProgress = hasConnections ? 100 : 0;
      
      // Simulator is complete if settings exist and sequence exists
      const hasSimulationData = hasConnections && !!currentSimulationSettings;
      const simulatorProgress = hasSimulationData ? 100 : 0;

      console.log('Site workflow progress (from loaded data):', {
        pattern: hasPattern,
        sequence: hasConnections,  
        simulator: hasSimulationData,
        progressValues: { patternProgress, sequenceProgress, simulatorProgress }
      });

      const progressData = {
        'pattern-creator': { 
          completed: hasPattern, 
          progress: patternProgress 
        },
        'sequence-designer': { 
          completed: hasConnections, 
          progress: sequenceProgress 
        },
        'simulator': { 
          completed: hasSimulationData, 
          progress: simulatorProgress 
        }
      };

      observer.next(progressData);
      observer.complete();
    });
  }

  private loadSiteData(projectId: number, siteId: number): void {
    // Clear current data first to avoid mixing sites
    this.clearAllData();
    
    console.log('Loading site data for:', { projectId, siteId });
    
    // Load from backend first, then fallback to localStorage
    this.loadFromBackend(projectId, siteId);
  }

  private loadFromBackend(projectId: number, siteId: number): void {
    console.log('Loading data from backend for site:', { projectId, siteId });
    
    // Load drill patterns
    this.siteBlastingService.getDrillPatterns(projectId, siteId).subscribe({
      next: (patterns) => {
        if (patterns && patterns.length > 0) {
          const latestPattern = patterns[0]; // Get most recent pattern
          
          // Handle drillPointsJson which can be either string (from backend) or array (already parsed)
          let drillPoints: DrillPoint[] = [];
          if (latestPattern.drillPointsJson) {
            if (typeof latestPattern.drillPointsJson === 'string') {
              try {
                drillPoints = JSON.parse(latestPattern.drillPointsJson);
              } catch (error) {
                console.warn('Failed to parse drill points JSON:', error);
                drillPoints = [];
              }
            } else {
              drillPoints = latestPattern.drillPointsJson;
            }
          }
          
          const patternData: PatternData = {
            drillPoints: drillPoints,
            settings: {
              spacing: latestPattern.spacing,
              burden: latestPattern.burden,
              depth: latestPattern.depth
            }
          };
          
          this.patternDataSubject.next(patternData);
          console.log('Loaded pattern from backend:', latestPattern.name, 'with', patternData.drillPoints.length, 'points');
          
          // After loading pattern, load blast sequences
          this.loadBlastSequencesFromBackend(projectId, siteId);
        } else {
          console.log('No patterns found in backend, trying localStorage');
          this.loadFromLocalStorage(projectId, siteId);
        }
      },
      error: (error) => {
        console.log('Error loading patterns from backend, trying localStorage:', error.message);
        this.loadFromLocalStorage(projectId, siteId);
      }
    });
  }

  private loadBlastSequencesFromBackend(projectId: number, siteId: number): void {
    this.siteBlastingService.getBlastSequences(projectId, siteId).subscribe({
      next: (sequences) => {
        if (sequences && sequences.length > 0) {
          const latestSequence = sequences[0]; // Get most recent sequence
          
          // Handle connectionsJson which can be either string (from backend) or array (already parsed)
          let connections: BlastConnection[] = [];
          if (latestSequence.connectionsJson) {
            if (typeof latestSequence.connectionsJson === 'string') {
              try {
                connections = JSON.parse(latestSequence.connectionsJson);
              } catch (error) {
                console.warn('Failed to parse connections JSON:', error);
                connections = [];
              }
            } else {
              connections = latestSequence.connectionsJson;
            }
          }
          
          // Validate connections against current pattern
          if (this.patternDataSubject.value) {
            const validConnections = this.validateConnections(connections, this.patternDataSubject.value.drillPoints);
            this.connectionsSubject.next(validConnections);
            console.log('Loaded blast sequence from backend:', latestSequence.name, 'with', validConnections.length, 'connections');
          } else {
            this.connectionsSubject.next(connections);
            console.log('Loaded blast sequence from backend:', latestSequence.name, 'with', connections.length, 'connections');
          }
        } else {
          console.log('No blast sequences found in backend');
        }
        
        // After loading backend data, load workflow state
        this.loadWorkflowStateFromBackend(projectId, siteId);
      },
      error: (error) => {
        console.log('Error loading blast sequences from backend:', error.message);
        this.loadWorkflowStateFromBackend(projectId, siteId);
      }
    });
  }

  private loadWorkflowStateFromBackend(projectId: number, siteId: number): void {
    // Load simulation settings and state
    this.siteBlastingService.getWorkflowState(projectId, siteId, 'simulation_settings').subscribe({
      next: (data) => {
        if (data && data.jsonData) {
          const settings = data.jsonData;
          this.simulationSettingsSubject.next(settings);
          console.log('Loaded simulation settings from backend');
        }
      },
      error: (error) => {
        console.log('No simulation settings found in backend:', error.message);
      }
    });

    this.siteBlastingService.getWorkflowState(projectId, siteId, 'simulation_state').subscribe({
      next: (data) => {
        if (data && data.jsonData) {
          const state = data.jsonData;
          this.simulationStateSubject.next(state);
          console.log('Loaded simulation state from backend');
        }
      },
      error: (error) => {
        console.log('No simulation state found in backend:', error.message);
      }
    });
  }

  private loadFromLocalStorage(projectId: number, siteId: number): void {
    const siteKey = `site_${projectId}_${siteId}`;
    console.log('Loading from localStorage for:', siteKey);
    
    // Try to load existing data for this site from localStorage
    const savedPatternData = localStorage.getItem(`${siteKey}_pattern`);
    const savedConnections = localStorage.getItem(`${siteKey}_connections`);
    const savedSimulationSettings = localStorage.getItem(`${siteKey}_simulation_settings`);
    const savedSimulationState = localStorage.getItem(`${siteKey}_simulation_state`);
    
    if (savedPatternData) {
      try {
        const patternData = JSON.parse(savedPatternData);
        this.patternDataSubject.next(patternData);
        console.log('Loaded pattern data from localStorage:', patternData.drillPoints?.length || 0, 'points');
      } catch (error) {
        console.warn('Failed to load pattern data from localStorage:', error);
        localStorage.removeItem(`${siteKey}_pattern`);
      }
    }
    
    if (savedConnections) {
      try {
        const connections = JSON.parse(savedConnections);
        
        // Validate connections match current pattern
        if (this.patternDataSubject.value) {
          const validConnections = this.validateConnections(connections, this.patternDataSubject.value.drillPoints);
          this.connectionsSubject.next(validConnections);
          console.log('Loaded connections from localStorage:', validConnections.length, 'valid connections');
          
          // Save cleaned connections back if any were removed
          if (validConnections.length !== connections.length) {
            localStorage.setItem(`${siteKey}_connections`, JSON.stringify(validConnections));
          }
        } else {
          this.connectionsSubject.next(connections);
          console.log('Loaded connections from localStorage:', connections.length, 'connections');
        }
      } catch (error) {
        console.warn('Failed to load connections from localStorage:', error);
        localStorage.removeItem(`${siteKey}_connections`);
      }
    }
    
    if (savedSimulationSettings) {
      try {
        const settings = JSON.parse(savedSimulationSettings);
        this.simulationSettingsSubject.next(settings);
        console.log('Loaded simulation settings from localStorage');
      } catch (error) {
        console.warn('Failed to load simulation settings from localStorage:', error);
        localStorage.removeItem(`${siteKey}_simulation_settings`);
      }
    }
    
    if (savedSimulationState) {
      try {
        const state = JSON.parse(savedSimulationState);
        this.simulationStateSubject.next(state);
        console.log('Loaded simulation state from localStorage');
      } catch (error) {
        console.warn('Failed to load simulation state from localStorage:', error);
        localStorage.removeItem(`${siteKey}_simulation_state`);
      }
    }
  }

  private clearAllData(): void {
    this.patternDataSubject.next(null);
    this.connectionsSubject.next([]);
    // Don't reset simulation settings/state as they are global preferences
  }

  private validateConnections(connections: BlastConnection[], drillPoints: DrillPoint[]): BlastConnection[] {
    if (!drillPoints || drillPoints.length === 0) {
      return [];
    }
    
    const validHoleIds = new Set(drillPoints.map(point => point.id));
    
    return connections.filter(connection => {
      const isValid = validHoleIds.has(connection.fromHoleId) && validHoleIds.has(connection.toHoleId);
      if (!isValid) {
        console.warn('Removing invalid connection:', connection.fromHoleId, '->', connection.toHoleId);
      }
      return isValid;
    });
  }



  // Pattern Data Methods
  setPatternData(patternData: PatternData, autoSave: boolean = false): void {
    this.patternDataSubject.next(patternData);
    this.updateWorkflowStep('pattern', true);
    this.enableWorkflowStep('sequence');
    this.validateSequence();
    this.calculateSimulationDuration();
    
    // Only save to storage if explicitly requested
    if (autoSave && this.currentSiteContext) {
      const siteKey = `site_${this.currentSiteContext.projectId}_${this.currentSiteContext.siteId}`;
      localStorage.setItem(`${siteKey}_pattern`, JSON.stringify(patternData));
    }
  }

  // Explicit save method for pattern data
  savePatternData(): void {
    const currentPattern = this.patternDataSubject.value;
    if (currentPattern && this.currentSiteContext) {
      const siteKey = `site_${this.currentSiteContext.projectId}_${this.currentSiteContext.siteId}`;
      localStorage.setItem(`${siteKey}_pattern`, JSON.stringify(currentPattern));
      console.log('Pattern data saved to localStorage');
    }
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
  setConnections(connections: BlastConnection[], autoSave: boolean = false): void {
    this.connectionsSubject.next([...connections]);
    this.updateWorkflowStep('sequence', connections.length > 0);
    this.enableWorkflowStep('simulate');
    this.validateSequence();
    this.calculateSimulationDuration();
    
    // Only save to storage if explicitly requested
    if (autoSave && this.currentSiteContext) {
      const siteKey = `site_${this.currentSiteContext.projectId}_${this.currentSiteContext.siteId}`;
      localStorage.setItem(`${siteKey}_connections`, JSON.stringify(connections));
    }
  }

  // Explicit save method for connections
  saveConnections(): void {
    const currentConnections = this.connectionsSubject.value;
    if (this.currentSiteContext) {
      const siteKey = `site_${this.currentSiteContext.projectId}_${this.currentSiteContext.siteId}`;
      localStorage.setItem(`${siteKey}_connections`, JSON.stringify(currentConnections));
      console.log('Connections saved to localStorage');
    }
  }

  addConnection(connection: BlastConnection): void {
    const current = this.connectionsSubject.value;
    this.setConnections([...current, connection], false); // Don't auto-save
  }

  removeConnection(connectionId: string): void {
    const current = this.connectionsSubject.value;
    const filtered = current.filter(c => c.id !== connectionId);
    this.setConnections(filtered, false); // Don't auto-save
  }

  updateConnection(updatedConnection: BlastConnection): void {
    const current = this.connectionsSubject.value;
    const index = current.findIndex(c => c.id === updatedConnection.id);
    if (index !== -1) {
      const updated = [...current];
      updated[index] = updatedConnection;
      this.setConnections(updated, false); // Don't auto-save
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
  updateSimulationState(updates: Partial<SimulationState>, autoSave: boolean = false): void {
    const current = this.simulationStateSubject.value;
    const newState = { ...current, ...updates };
    this.simulationStateSubject.next(newState);
    
    // Only save to storage if explicitly requested
    if (autoSave && this.currentSiteContext) {
      const siteKey = `site_${this.currentSiteContext.projectId}_${this.currentSiteContext.siteId}`;
      localStorage.setItem(`${siteKey}_simulation_state`, JSON.stringify(newState));
    }
  }

  // Explicit save method for simulation state
  saveSimulationState(): void {
    const currentState = this.simulationStateSubject.value;
    if (this.currentSiteContext) {
      const siteKey = `site_${this.currentSiteContext.projectId}_${this.currentSiteContext.siteId}`;
      localStorage.setItem(`${siteKey}_simulation_state`, JSON.stringify(currentState));
      console.log('Simulation state saved to localStorage');
    }
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
  updateSimulationSettings(updates: Partial<SimulationSettings>, autoSave: boolean = false): void {
    const current = this.simulationSettingsSubject.value;
    const newSettings = { ...current, ...updates };
    this.simulationSettingsSubject.next(newSettings);
    
    // Only save to storage if explicitly requested
    if (autoSave && this.currentSiteContext) {
      const siteKey = `site_${this.currentSiteContext.projectId}_${this.currentSiteContext.siteId}`;
      localStorage.setItem(`${siteKey}_simulation_settings`, JSON.stringify(newSettings));
    }
  }

  // Explicit save method for simulation settings
  saveSimulationSettings(): void {
    const currentSettings = this.simulationSettingsSubject.value;
    if (this.currentSiteContext) {
      const siteKey = `site_${this.currentSiteContext.projectId}_${this.currentSiteContext.siteId}`;
      localStorage.setItem(`${siteKey}_simulation_settings`, JSON.stringify(currentSettings));
      console.log('Simulation settings saved to localStorage');
    }
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
          potentialImprovement: Math.round((totalTime - 3000) / totalTime * 100),
          priority: 'high',
          affectedElements: connections.map(c => c.id)
        });
      }
    }

    return suggestions;
  }

  private calculateSimulationDuration(): void {
    const connections = this.connectionsSubject.value;
    const pattern = this.patternDataSubject.value;

    // If there is no pattern or no holes, nothing to simulate
    if (!pattern || pattern.drillPoints.length === 0) {
      this.updateSimulationState({ totalDuration: 0, totalSteps: 0 });
      return;
    }

    // Fast-path for patterns without any wired connections
    if (connections.length === 0) {
      const totalDuration = pattern.drillPoints.length * 500; // 500 ms per hole for the single-wave blast
      this.updateSimulationState({ totalDuration, totalSteps: 0 });
      return;
    }

    /*
     * Calculate the moment of the last detonation using the same rules that the
     * AnimationService uses to drive the visualisation:
     *   • Signal travels along a connection for <delay> ms.
     *   • The destination hole then detonates 500 ms after the signal arrives.
     * We perform a breadth-first propagation starting with every "root" hole – a
     * hole that has no incoming connection – and keep track of the earliest
     * known blast time for every hole.  The longest of those blast times is the
     * overall duration of the sequence.
     */
    const holeStartTimes = new Map<string, number>();
    const toHoleIds = new Set(connections.map(c => c.toHoleId));

    // Identify root holes (no incoming signal)
    const queue: Array<{ holeId: string; start: number }> = [];
    pattern.drillPoints.forEach(point => {
      if (!toHoleIds.has(point.id)) {
        holeStartTimes.set(point.id, 0);
        queue.push({ holeId: point.id, start: 0 });
      }
    });

    // Edge case: every hole has an incoming connection – fall back to the first connection's origin
    if (queue.length === 0 && connections.length > 0) {
      const rootId = connections[0].fromHoleId;
      holeStartTimes.set(rootId, 0);
      queue.push({ holeId: rootId, start: 0 });
    }

    // Propagate signals through the network
    while (queue.length > 0) {
      const { holeId, start } = queue.shift()!;
      const outgoing = connections.filter(c => c.fromHoleId === holeId);

      outgoing.forEach(conn => {
        const connStart = start;
        const connEnd = connStart + (conn.delay || 0);
        const nextHoleBlast = connEnd + 500; // 500 ms after signal arrival

        const existingTime = holeStartTimes.get(conn.toHoleId);
        if (existingTime === undefined || nextHoleBlast > existingTime) {
          holeStartTimes.set(conn.toHoleId, nextHoleBlast);
          queue.push({ holeId: conn.toHoleId, start: nextHoleBlast });
        }
      });
    }

    const totalDuration = Math.max(...holeStartTimes.values());
    const totalSteps = connections.length;

    this.updateSimulationState({ totalDuration, totalSteps });
  }

  // Project Management Methods
  createNewProject(name: string, description: string = ''): ProjectData {
    const project: ProjectData = {
      id: this.generateId(),
      name,
      description,
      createdAt: new Date(),
      updatedAt: new Date(),
      patternData: null,
      connections: [],
      simulationSettings: this.simulationSettingsSubject.value,
      metadata: {
        version: '1.0',
        author: 'User',
        notes: ''
      }
    };

    this.currentProjectSubject.next(project);
    this.resetWorkflow();
    return project;
  }

  saveProject(): void {
    const current = this.currentProjectSubject.value;
    if (current) {
      const updated: ProjectData = {
        ...current,
        patternData: this.patternDataSubject.value,
        connections: this.connectionsSubject.value,
        simulationSettings: this.simulationSettingsSubject.value,
        updatedAt: new Date()
      };
      this.currentProjectSubject.next(updated);
      // Here you would typically save to backend/storage
    }
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

  // Utility Methods
  private generateId(): string {
    return Date.now().toString(36) + Math.random().toString(36).substr(2);
  }

  // Timeline Methods for Simulator
  generateTimelineMarkers(): TimelineMarker[] {
    const connections = this.connectionsSubject.value;
    const markers: TimelineMarker[] = [];

    if (connections.length === 0) {
      // Add start marker even with no connections
      markers.push({
        time: 0,
        label: 'Blast Start',
        type: 'sequence_start',
        color: '#4CAF50'
      });
      return markers;
    }

    // Add start marker
    markers.push({
      time: 0,
      label: 'Blast Start',
      type: 'sequence_start',
      color: '#4CAF50'
    });

    // Group connections by their starting holes (for wave analysis)
    const connectionsByFromHole = new Map<string, BlastConnection[]>();
    connections.forEach(conn => {
      if (!connectionsByFromHole.has(conn.fromHoleId)) {
        connectionsByFromHole.set(conn.fromHoleId, []);
      }
      connectionsByFromHole.get(conn.fromHoleId)!.push(conn);
    });

    // Find initial connections (not targeted by other connections' point 2)
    const allToHoleIds = new Set(connections.map(conn => conn.toHoleId));
    const initialFromHoles = new Set<string>();
    
    connections.forEach(conn => {
      if (!allToHoleIds.has(conn.fromHoleId)) {
        initialFromHoles.add(conn.fromHoleId);
      }
    });
    
    const initialConnections: BlastConnection[] = [];
    initialFromHoles.forEach(holeId => {
      const connectionsFromHole = connectionsByFromHole.get(holeId) || [];
      initialConnections.push(...connectionsFromHole);
    });

    // Track processed connections and wave timing
    const processedConnections = new Set<string>();
    const waveMarkers: Array<{time: number, connections: BlastConnection[], waveNumber: number}> = [];
    
    // Process waves to build timeline markers
    const processWave = (connectionsToProcess: BlastConnection[], waveStartTime: number, waveNumber: number) => {
      if (connectionsToProcess.length === 0) return;

      waveMarkers.push({
        time: waveStartTime,
        connections: [...connectionsToProcess],
        waveNumber
      });

      // Mark connections as processed
      connectionsToProcess.forEach(conn => {
        processedConnections.add(conn.id);
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
        const nextWaveTime = waveStartTime + 500; // 500ms between waves
        processWave(nextWaveConnections, nextWaveTime, waveNumber + 1);
      }
    };

    // Start processing waves
    const initialStartTime = initialConnections.length > 0 ? 
      Math.min(...initialConnections.map(conn => conn.delay)) : 0;
    
    processWave(initialConnections, initialStartTime, 1);

    // Create timeline markers from wave data
    waveMarkers.forEach(wave => {
      // Wave start marker
      const waveConnections = wave.connections;
      const wireNames = waveConnections.map(conn => {
        const typeName = this.getConnectorTypeName(conn.connectorType);
        return `${typeName} ${conn.delay}ms`;
      }).join(', ');

      markers.push({
        time: wave.time,
        label: `Wave ${wave.waveNumber}: ${wireNames}`,
        type: 'hole_blast',
        color: wave.waveNumber === 1 ? '#FF5722' : '#FF8A65'
      });

      // Add individual wire events within the wave
      waveConnections.forEach((conn, index) => {
        const baseTime = wave.time;
        
        // Hidden Point 1 activation
        markers.push({
          time: baseTime,
          label: `${this.getConnectorTypeName(conn.connectorType)} Point 1`,
          type: 'milestone',
          color: '#4CAF50'
        });

        // Signal propagation (25ms delay)
        markers.push({
          time: baseTime + 25,
          label: `${this.getConnectorTypeName(conn.connectorType)} Signal`,
          type: 'milestone',
          color: '#2196F3'
        });

        // Hidden Point 2 activation (35ms total)
        markers.push({
          time: baseTime + 35,
          label: `${this.getConnectorTypeName(conn.connectorType)} Point 2`,
          type: 'milestone',
          color: '#FF9800'
        });

        // Detonation (50ms total)
        const detonationTime = baseTime + 50;
        markers.push({
          time: detonationTime,
          label: `${conn.toHoleId} Detonation`,
          type: 'hole_blast',
          color: '#F44336'
        });
      });
    });

    // Add end marker
    const lastWave = waveMarkers[waveMarkers.length - 1];
    const endTime = lastWave ? lastWave.time + 50 + 1500 : 2000; // Detonation + effects duration
    
    markers.push({
      time: endTime,
      label: 'Blast Complete',
      type: 'sequence_end',
      color: '#2196F3'
    });

    return markers.sort((a, b) => a.time - b.time);
  }

  private getConnectorTypeName(connectorType: string): string {
    switch (connectorType?.toLowerCase()) {
      case 'det_cord': return 'Det Cord';
      case 'shock_tube': return 'Shock Tube';
      case 'electronic': return 'Electronic';
      case 'pyrotechnic': return 'Pyrotechnic';
      case 'connector': return 'Connector';
      default: return connectorType || 'Unknown';
    }
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

    // Use the new path-based calculation for totalBlastTime
    const totalBlastTime = this.blastTimeCalculator.calculateTotalBlastTime(connections);
    // Use the new path-based calculation for average delay between detonations
    const averageDelayBetweenHoles = this.blastTimeCalculator.getAverageDelayBetweenDetonations(connections);
    // Use wave-based metrics for the rest
    const waveMetrics = this.calculateWaveBasedMetrics(connections);
    const totalHoles = patternData.drillPoints.length;
    const connectedHoles = new Set([...connections.map(c => c.fromHoleId), ...connections.map(c => c.toHoleId)]);
    const connectionUtilization = (connectedHoles.size / totalHoles) * 100;
    const timingConflicts = this.detectTimingConflicts(connections);
    const criticalConflicts = timingConflicts.filter(w => w.severity === 'high').length;
    const mediumConflicts = timingConflicts.filter(w => w.severity === 'medium').length;
    const efficiencyScore = this.calculateEfficiencyScore(waveMetrics, connections.length, patternData.drillPoints.length);
    const safetyScore = this.calculateSafetyScore(waveMetrics.maxSimultaneousDetonations, criticalConflicts, mediumConflicts);

    return {
      totalBlastTime,
      averageDelayBetweenHoles,
      maxSimultaneousDetonations: waveMetrics.maxSimultaneousDetonations,
      efficiencyScore: Math.round(efficiencyScore),
      safetyScore: Math.round(safetyScore),
      connectionUtilization: Math.round(connectionUtilization)
    };
  }

  private calculateWaveBasedMetrics(connections: BlastConnection[]) {
    // Group connections by their starting holes (for wave analysis)
    const connectionsByFromHole = new Map<string, BlastConnection[]>();
    connections.forEach(conn => {
      if (!connectionsByFromHole.has(conn.fromHoleId)) {
        connectionsByFromHole.set(conn.fromHoleId, []);
      }
      connectionsByFromHole.get(conn.fromHoleId)!.push(conn);
    });

    // Find initial connections (not targeted by other connections' point 2)
    const allToHoleIds = new Set(connections.map(conn => conn.toHoleId));
    const initialFromHoles = new Set<string>();
    
    connections.forEach(conn => {
      if (!allToHoleIds.has(conn.fromHoleId)) {
        initialFromHoles.add(conn.fromHoleId);
      }
    });
    
    const initialConnections: BlastConnection[] = [];
    initialFromHoles.forEach(holeId => {
      const connectionsFromHole = connectionsByFromHole.get(holeId) || [];
      initialConnections.push(...connectionsFromHole);
    });

    // Track wave timing and detonations
    const processedConnections = new Set<string>();
    const waveTimings: Array<{waveNumber: number, startTime: number, connections: BlastConnection[], detonationTimes: number[]}> = [];
    
    // Process waves to calculate actual timing
    const processWave = (connectionsToProcess: BlastConnection[], waveStartTime: number, waveNumber: number) => {
      if (connectionsToProcess.length === 0) return;

      const detonationTimes: number[] = [];
      
      connectionsToProcess.forEach(conn => {
        processedConnections.add(conn.id);
        // Each connection detonates 50ms after wave start (25ms propagation + 35ms point 2 + detonation)
        const detonationTime = waveStartTime + 50;
        detonationTimes.push(detonationTime);
      });

      waveTimings.push({
        waveNumber,
        startTime: waveStartTime,
        connections: [...connectionsToProcess],
        detonationTimes
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
        const nextWaveTime = waveStartTime + 500; // 500ms between waves
        processWave(nextWaveConnections, nextWaveTime, waveNumber + 1);
      }
    };

    // Start processing waves
    const initialStartTime = initialConnections.length > 0 ? 
      Math.min(...initialConnections.map(conn => conn.delay)) : 0;
    
    processWave(initialConnections, initialStartTime, 1);

    // Calculate metrics from wave data
    let totalBlastTime = 0;
    let maxSimultaneousDetonations = 0;
    const allDetonationTimes: number[] = [];

    if (waveTimings.length > 0) {
      // Total blast time is the last detonation + effect duration
      const lastWave = waveTimings[waveTimings.length - 1];
      const lastDetonationTime = Math.max(...lastWave.detonationTimes);
      totalBlastTime = lastDetonationTime + 1500; // 1.5s for blast effects to complete

      // Max simultaneous detonations in any wave
      maxSimultaneousDetonations = Math.max(...waveTimings.map(wave => wave.connections.length));

      // Collect all detonation times
      waveTimings.forEach(wave => {
        allDetonationTimes.push(...wave.detonationTimes);
      });
    }

    // Calculate average delay between waves (not individual holes)
    const averageDelayBetweenWaves = waveTimings.length > 1 ? 
      (waveTimings[waveTimings.length - 1].startTime - waveTimings[0].startTime) / (waveTimings.length - 1) : 0;

    return {
      totalBlastTime,
      averageDelayBetweenWaves,
      maxSimultaneousDetonations,
      waveCount: waveTimings.length,
      allDetonationTimes
    };
  }

  private calculateEfficiencyScore(waveMetrics: any, connectionCount: number, totalHoles: number): number {
    let score = 100;

    // Penalty for long total blast time (more than 5 seconds is inefficient)
    if (waveMetrics.totalBlastTime > 5000) {
      score -= Math.min(30, (waveMetrics.totalBlastTime - 5000) / 1000 * 5);
    }

    // Penalty for too many waves (indicates poor sequencing)
    const idealWaves = Math.ceil(connectionCount / 4); // Ideal 4 connections per wave
    if (waveMetrics.waveCount > idealWaves) {
      score -= (waveMetrics.waveCount - idealWaves) * 10;
    }

    // Bonus for good connection utilization
    const utilizationRate = connectionCount / totalHoles;
    if (utilizationRate > 0.8) {
      score += 10; // Bonus for high utilization
    } else if (utilizationRate < 0.5) {
      score -= 15; // Penalty for low utilization
    }

    // Penalty for poor wave distribution
    if (waveMetrics.maxSimultaneousDetonations > 6) {
      score -= (waveMetrics.maxSimultaneousDetonations - 6) * 5; // Penalty for too many simultaneous
    }

    return Math.max(0, Math.min(100, score));
  }

  private calculateSafetyScore(maxSimultaneous: number, criticalConflicts: number, mediumConflicts: number): number {
    let score = 100;

    // Major penalties for timing conflicts
    score -= criticalConflicts * 25; // -25 points per critical conflict
    score -= mediumConflicts * 10;   // -10 points per medium conflict

    // Penalty for too many simultaneous detonations (safety risk)
    if (maxSimultaneous > 4) {
      score -= (maxSimultaneous - 4) * 8; // -8 points per extra simultaneous detonation
    }

    // Bonus for well-distributed timing
    if (maxSimultaneous <= 2) {
      score += 10; // Bonus for conservative simultaneous detonations
    }

    return Math.max(0, Math.min(100, score));
  }

  updateValidation(validation: SimulationValidation): void {
    this.validationSubject.next(validation);
  }
} 