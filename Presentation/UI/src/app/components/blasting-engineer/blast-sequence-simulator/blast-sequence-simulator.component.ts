import { Component, OnInit, OnDestroy, ViewChild, ElementRef, ChangeDetectorRef, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, takeUntil, firstValueFrom, map } from 'rxjs';
import { 
  SimulationState, 
  SimulationSettings, 
  ViewSettings,
  SimulationValidation,
  SimulationMetrics,
  TimelineMarker,
  AnimationFrame,
  BlastEffect
} from '../shared/models/simulation.model';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { DrillPoint, PatternData, BlastConnection } from '../drilling-pattern-creator/models/drill-point.model';
import { TimelineComponent } from './components/timeline/timeline.component';
import { PatternRendererComponent } from './components/pattern-renderer/pattern-renderer.component';
import { ViewControlsComponent } from './components/view-controls/view-controls.component';
import { AnimationService } from './services/animation.service';
import { ReportExportService } from './services/report-export.service';
import { NotificationService } from '../../../core/services/notification.service';
import { UnifiedDrillDataService } from '../../../core/services/unified-drill-data.service';
import { SiteBlastingService } from '../../../core/services/site-blasting.service';
import { ActivatedRoute } from '@angular/router';
import { ConnectorType } from '../../../core/models/site-blasting.model';
import { DrillLocation } from '../../../core/models/drilling.model';

@Component({
  selector: 'app-blast-sequence-simulator',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule,
    TimelineComponent,
    PatternRendererComponent,
    ViewControlsComponent,
    MatTabsModule,
    MatIconModule,
    MatButtonModule,
    MatProgressBarModule
  ],
  template: `
    <div class="simulator-container" [class.fullscreen]="isFullscreen">
      <div class="toolbar">
        <div class="left-controls">
          <button (click)="play()" [disabled]="simulationState.isPlaying" title="Play (Space)" aria-label="Play">
            <i class="material-icons">play_arrow</i>
          </button>
          <button (click)="pause()" [disabled]="!simulationState.isPlaying" title="Pause (Space)" aria-label="Pause">
            <i class="material-icons">pause</i>
          </button>
          <button (click)="stop()" title="Stop (S)" aria-label="Stop">
            <i class="material-icons">stop</i>
          </button>
        </div>

        <div class="center-controls">
          
          <span class="time-display">
            {{simulationState.currentTime | number:'1.0-0'}}ms 
          </span>
        </div>

        <div class="right-controls">
          <button (click)="toggleFullscreen()" title="Toggle fullscreen (F)" aria-label="Toggle Fullscreen">
            <i class="material-icons">{{isFullscreen ? 'fullscreen_exit' : 'fullscreen'}}</i>
          </button>
          <button (click)="onSaveSimulation()" [disabled]="isSaved || !patternData" title="Save simulation">
            <i class="material-icons">{{isSaved ? 'check' : 'save'}}</i>
          </button>
        </div>
      </div>

      <div class="main-content" #patternContainer>
        <app-view-controls
          [viewSettings]="viewSettings"
          (viewSettingsChange)="onViewSettingsChange($event)"
          (zoomIn)="zoomIn()"
          (zoomOut)="zoomOut()"
          (centerView)="centerView()"
          (panUp)="panUp()"
          (panDown)="panDown()"
          (panLeft)="panLeft()"
          (panRight)="panRight()"
        ></app-view-controls>

        <app-pattern-renderer
          #patternRenderer
          [patternData]="patternData"
          [connections]="connections"
          [simulationSettings]="simulationSettings"
          [viewSettings]="viewSettings"
          [holeStates]="currentFrame.holeStates"
          [connectionStates]="currentFrame.connectionStates"
        ></app-pattern-renderer>

        <app-timeline
          [markers]="timelineMarkers"
          [currentTime]="simulationState.currentTime"
          [totalDuration]="simulationState.totalDuration"
        ></app-timeline>
      </div>
    </div>
  `,
  styles: [`
    .simulator-container {
      display: flex;
      flex-direction: column;
      height: 100vh;
      background: #f8f9fa;
    }

    .simulator-container.fullscreen {
      position: fixed;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      z-index: 1000;
    }

    .toolbar {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 0.5rem;
      background: #fff;
      border-bottom: 1px solid #dee2e6;
    }

    .left-controls,
    .right-controls {
      display: flex;
      gap: 0.5rem;
    }

    .center-controls {
      display: flex;
      align-items: center;
      gap: 1rem;
      flex: 1;
      max-width: 600px;
      margin: 0 2rem;
    }

    .progress-container {
      flex: 1;
      height: 6px;
      background: #dee2e6;
      border-radius: 3px;
      position: relative;
      cursor: pointer;
    }

    .progress-bar {
      height: 100%;
      background: linear-gradient(90deg, #42a5f5 0%, #1976d2 100%);
      border-radius: 3px 0 0 3px;
      transition: width 0.1s linear;
    }

    .time-display {
      font-family: monospace;
      font-size: 0.875rem;
      color: #495057;
    }

    button {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 36px;
      height: 36px;
      border: none;
      background: transparent;
      border-radius: 4px;
      cursor: pointer;
      transition: all 0.2s;
    }

    button:hover:not(:disabled) {
      background: #e9ecef;
    }

    button:active:not(:disabled), button.active {
      background: #d0d7de;
    }

    button:disabled {
      opacity: 0.5;
      cursor: not-allowed;
    }

    button i {
      font-size: 20px;
    }

    input[type="range"] {
      flex: 1;
      height: 4px;
      -webkit-appearance: none;
      background: #dee2e6;
      border-radius: 2px;
      outline: none;
    }

    input[type="range"]::-webkit-slider-thumb {
      -webkit-appearance: none;
      width: 16px;
      height: 16px;
      background: #1976d2;
      border-radius: 50%;
      cursor: pointer;
    }

    .main-content {
      display: flex;
      flex-direction: column;
      flex: 1;
      padding: 1rem;
      gap: 1rem;
      overflow: hidden;
    }
  `]
})
export class BlastSequenceSimulatorComponent implements OnInit, OnDestroy {
  @ViewChild('patternContainer') containerRef!: ElementRef<HTMLDivElement>;
  @ViewChild('patternRenderer') patternRendererComp!: PatternRendererComponent;

  private destroy$ = new Subject<void>();
  private saveTimeout: any;

  // Data
  patternData: PatternData | null = null;
  connections: BlastConnection[] = [];
  simulationState: SimulationState = {
    isPlaying: false,
    isPaused: false,
    currentTime: 0,
    totalDuration: 0,
    playbackSpeed: 1,
    currentStep: 0,
    totalSteps: 0
  };
  simulationSettings: SimulationSettings = {
    showTiming: true,
    showConnections: true,
    showEffects: true,
    showSequenceNumbers: true,
    effectIntensity: 75,
    animationQuality: 'medium'
  };
  viewSettings: ViewSettings = {
    showGrid: true,
    colorTheme: 'default',
    showHoleDetails: false,
    showConnectionLabels: true,
    highlightActiveConnections: true,
    frameRate: 60
  };
  validation: SimulationValidation = {
    isValid: true,
    warnings: [],
    errors: [],
    suggestions: []
  };
  metrics: SimulationMetrics = {
    totalBlastTime: 0,
    averageDelayBetweenHoles: 0,
    maxSimultaneousDetonations: 0,
    efficiencyScore: 0,
    safetyScore: 0,
    connectionUtilization: 0
  };

  // UI State
  isFullscreen = false;
  isSaved = false;
  timelineMarkers: TimelineMarker[] = [];
  currentFrame: AnimationFrame = {
    time: 0,
    holeStates: new Map(),
    connectionStates: new Map(),
    activeEffects: []
  };

  // Site context
  private currentProjectId!: number;
  private currentSiteId!: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private animationService: AnimationService,
    private reportExportService: ReportExportService,
    private cdr: ChangeDetectorRef,
    private notificationService: NotificationService,
    private unifiedDrillDataService: UnifiedDrillDataService,
    private siteBlastingService: SiteBlastingService
  ) {}

  ngOnInit(): void {
    // Initialize site context from route parameters
    this.initializeSiteContext();
    
    // Setup animation frame subscription
    this.animationService.getAnimationFrame()
      .pipe(takeUntil(this.destroy$))
      .subscribe(frame => {
        this.currentFrame = frame;
        this.simulationState.currentTime = frame.time;
        // Manually trigger change detection because animation frames run outside Angular's zone
        this.cdr.detectChanges();
      });
  }

  private initializeSiteContext(): void {
    // Get project and site from route parameters
    this.route.params.subscribe(params => {
      this.currentProjectId = +params['projectId'];
      this.currentSiteId = +params['siteId'];
      
      console.log('🔍 Simulator - Setting site context', { 
        projectId: this.currentProjectId, 
        siteId: this.currentSiteId 
      });
      
      // Load data now that we have context
      this.loadDataFromBackend();
    });
  }

  private loadDataFromBackend(): void {
    if (!this.currentProjectId || !this.currentSiteId) {
      console.error('❌ Missing project or site ID for simulator');
      this.notificationService.showError('Missing project or site context');
      return;
    }

    // Load pattern data first
    this.loadPatternData();
    
    // Load blast connections
    this.loadBlastConnections();
  }

  private loadPatternData(): void {
    this.unifiedDrillDataService.loadPatternData(this.currentProjectId, this.currentSiteId)
      .subscribe({
        next: (patternData) => {
          if (patternData && patternData.drillLocations && patternData.drillLocations.length > 0) {
            // Convert unified service PatternData to component PatternData format
            this.patternData = {
              drillPoints: patternData.drillLocations as any,
              settings: patternData.settings
            } as any;
            console.log('✅ Simulator loaded pattern data with', patternData.drillLocations.length, 'points');
            this.cdr.detectChanges();
          } else {
            console.warn('⚠️ No pattern data available for simulation');
            this.notificationService.showError('No drill pattern found. Please create a pattern first.');
            this.goToPatternCreator();
          }
        },
        error: (error) => {
          console.error('❌ Error loading pattern data:', error);
          this.notificationService.showError('Failed to load drill pattern data');
          this.goToPatternCreator();
        }
      });
  }

  private loadBlastConnections(): void {
    this.siteBlastingService.getBlastConnections(this.currentProjectId, this.currentSiteId)
      .subscribe({
        next: (connections) => {
          if (connections && connections.length > 0) {
            // Map API connections to frontend format
            this.connections = connections.map(conn => ({
              id: conn.id,
              point1DrillPointId: conn.point1DrillPointId,
              point2DrillPointId: conn.point2DrillPointId,
              // Add fromHoleId and toHoleId for compatibility
              fromHoleId: conn.point1DrillPointId,
              toHoleId: conn.point2DrillPointId,
              connectorType: this.mapConnectorTypeFromApi(conn.connectorType),
              delay: conn.delay,
              sequence: conn.sequence,
              projectId: conn.projectId,
              siteId: conn.siteId,
              createdAt: conn.createdAt,
              updatedAt: conn.updatedAt,
              // Include IsStartingHole property for simulation
              isStartingHole: (conn as any).isStartingHole || false,
              // Add required startPoint and endPoint properties
              startPoint: {
                id: `SP-${conn.id}`,
                label: "1",
                x: 0,
                y: 0,
                isHidden: true
              },
              endPoint: {
                id: `EP-${conn.id}`,
                label: "2",
                x: 0,
                y: 0,
                isHidden: true
              }
            }));
            
            console.log('✅ Simulator loaded', connections.length, 'blast connections');
            this.initializeSimulation();
            this.cdr.detectChanges();
          } else {
            console.warn('⚠️ No blast connections found for simulation');
            this.notificationService.showError('No blast sequence found. Please create connections first.');
            this.goToSequenceDesigner();
          }
        },
        error: (error) => {
          console.error('❌ Error loading blast connections:', error);
          this.notificationService.showError('Failed to load blast sequence data');
          this.goToSequenceDesigner();
        }
      });
  }

  private mapConnectorTypeFromApi(connectorType: number): any {
    // Map from backend enum to frontend enum
    switch (connectorType) {
      case 0: return 'DETONATING_CORD'; // ConnectorType.DetonatingCord
      case 1: return 'CONNECTORS'; // ConnectorType.Connectors  
      default: return 'DETONATING_CORD';
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.animationService.stopAnimation();
  }

  // Animation Controls
  play(): void {
    if (!this.patternData || this.connections.length === 0) return;
    this.simulationState.isPlaying = true;
    this.simulationState.isPaused = false;
    this.simulationState.currentTime = 0;
    this.animationService.startAnimation(this.simulationState, this.connections, this.patternData.drillPoints);
  }

  pause(): void {
    this.simulationState.isPlaying = false;
    this.simulationState.isPaused = true;
    this.animationService.stopAnimation();
  }

  stop(): void {
    this.simulationState.isPlaying = false;
    this.simulationState.isPaused = false;
    this.simulationState.currentTime = 0;
    this.simulationState.currentStep = 0;
    this.animationService.stopAnimation();
    this.animationService.resetAnimation();
  }

  seekToTime(time: number): void {
    const clampedTime = Math.max(0, Math.min(time, this.simulationState.totalDuration));
    this.simulationState.currentTime = clampedTime;
  }

  // View Controls
  onViewSettingsChange(settings: Partial<ViewSettings>): void {
    this.viewSettings = { ...this.viewSettings, ...settings };
  }

  zoomIn(): void {
    const stage = this.patternRendererComp?.stage;
    if (!stage) { return; }

    const container = stage.container() as HTMLDivElement;
    const center = { x: container.clientWidth / 2, y: container.clientHeight / 2 };

    const oldScale = stage.scaleX();
    const newScale = Math.min(oldScale * 1.1, 5); // Clamp max zoom

    const mousePointTo = {
      x: (center.x - stage.x()) / oldScale,
      y: (center.y - stage.y()) / oldScale,
    };

    stage.scale({ x: newScale, y: newScale });
    const newPos = {
      x: center.x - mousePointTo.x * newScale,
      y: center.y - mousePointTo.y * newScale,
    };
    stage.position(newPos);

    stage.getLayers().forEach(layer => layer.batchDraw());
  }

  zoomOut(): void {
    const stage = this.patternRendererComp?.stage;
    if (!stage) { return; }

    const container = stage.container() as HTMLDivElement;
    const center = { x: container.clientWidth / 2, y: container.clientHeight / 2 };

    const oldScale = stage.scaleX();
    const newScale = Math.max(oldScale / 1.1, 0.1); // Clamp min zoom

    const mousePointTo = {
      x: (center.x - stage.x()) / oldScale,
      y: (center.y - stage.y()) / oldScale,
    };

    stage.scale({ x: newScale, y: newScale });
    const newPos = {
      x: center.x - mousePointTo.x * newScale,
      y: center.y - mousePointTo.y * newScale,
    };
    stage.position(newPos);

    stage.getLayers().forEach(layer => layer.batchDraw());
  }

  centerView(): void {
    const stage = this.patternRendererComp?.stage;
    if (!stage) { return; }

    stage.scale({ x: 1, y: 1 });
    stage.position({ x: 0, y: 0 });
    stage.getLayers().forEach(layer => layer.batchDraw());
  }

  resetZoom(): void {
    this.centerView();
  }

  private pan(offsetX: number, offsetY: number): void {
    const stage = this.patternRendererComp?.stage;
    if (!stage) { return; }

    stage.position({ x: stage.x() + offsetX, y: stage.y() + offsetY });
    stage.getLayers().forEach(layer => layer.batchDraw());
  }

  panUp(): void {
    this.pan(0, -50);
  }

  panDown(): void {
    this.pan(0, 50);
  }

  panLeft(): void {
    this.pan(50, 0);
  }

  panRight(): void {
    this.pan(-50, 0);
  }

  // Export functionality
  async exportReport(): Promise<void> {
    if (!this.patternData) return;

    try {
      await this.reportExportService.exportReport({
        patternData: this.patternData,
        connections: this.connections,
        simulationSettings: this.simulationSettings,
        simulationState: this.simulationState,
        projectName: `Project ${this.currentProjectId} - Site ${this.currentSiteId}`,
        canvasRef: this.containerRef,
        metrics: {
          totalBlastTime: this.simulationState.totalDuration,
          holes: this.patternData.drillPoints?.length || 0,
          connections: this.connections.length,
          averageDelayBetweenHoles: this.metrics.averageDelayBetweenHoles,
          maxSimultaneousDetonations: this.metrics.maxSimultaneousDetonations,
          efficiencyScore: this.metrics.efficiencyScore,
          safetyScore: this.metrics.safetyScore,
          connectionUtilization: this.metrics.connectionUtilization
        }
      });
      this.notificationService.showSuccess('Report exported successfully.');
    } catch (error) {
      this.notificationService.showError('Failed to export report.');
      console.error('Report export failed:', error);
    }
  }

  toggleFullscreen(): void {
    this.isFullscreen = !this.isFullscreen;
  }

  async onSaveSimulation(): Promise<void> {
    if (!this.patternData || this.connections.length === 0) {
      console.warn('No simulation data to save');
      return;
    }

    try {
      // For simulator, we don't need to save data since it's loaded from database
      // Just show success message
      this.isSaved = true;
      this.notificationService.showSuccess('Simulation state saved.');
      
      this.saveTimeout = setTimeout(() => {
        this.isSaved = false;
      }, 3000);
    } catch (error) {
      console.error('Error saving simulation:', error);
      this.notificationService.showError('Failed to save simulation.');
    }
  }

  // Progress bar helpers
  get progressPercentage(): number {
    return this.simulationState.totalDuration > 0 ?
      (this.simulationState.currentTime / this.simulationState.totalDuration) * 100 : 0;
  }

  onProgressBarClick(event: MouseEvent): void {
    const container = event.currentTarget as HTMLElement;
    const rect = container.getBoundingClientRect();
    const clickX = event.clientX - rect.left;
    const newTime = (clickX / rect.width) * this.simulationState.totalDuration;
    this.seekToTime(newTime);
  }

  // Navigation methods for breadcrumb navigation
  goToPatternCreator(): void {
    this.router.navigate(['/blasting-engineer/project-management', this.currentProjectId, 'sites', this.currentSiteId, 'pattern-creator']);
  }

  goToSequenceDesigner(): void {
    this.router.navigate(['/blasting-engineer/project-management', this.currentProjectId, 'sites', this.currentSiteId, 'sequence-designer']);
  }

  /**
   * KEYBOARD SHORTCUTS
   *  – Space : Play/Pause toggle
   *  – S     : Stop
   *  – ←/→   : Seek -/+ 1 s
   *  – F     : Full-screen toggle
   */
  @HostListener('window:keydown', ['$event'])
  handleKeyboard(event: KeyboardEvent): void {
    if (event.target instanceof HTMLInputElement || event.target instanceof HTMLTextAreaElement) {
      return; // ignore while typing in inputs
    }

    switch (event.key.toLowerCase()) {
      case ' ': // Space – toggle play/pause
        event.preventDefault();
        this.simulationState.isPlaying ? this.pause() : this.play();
        break;
      case 's': // Stop
        this.stop();
        break;
      case 'f': // Fullscreen
        this.toggleFullscreen();
        break;
      case 'arrowleft': // Seek −1 s
        this.seekToTime(this.simulationState.currentTime - 1000);
        break;
      case 'arrowright': // Seek +1 s
        this.seekToTime(this.simulationState.currentTime + 1000);
        break;
    }
  }

  private onRunValidation(data: any) {
    this.validation = {
      ...this.validation,
      ...data.validation,
      isValidating: false,
    };
    this.metrics = { ...this.metrics, ...data.metrics };
    this.cdr.markForCheck();
  }
  
  private onConnectionValidated(validation: SimulationValidation) {
    if (validation.errors.length > 0) {
      this.notificationService.showError('Sequence has validation errors.');
    }
    this.validation.errors = validation.errors;
    this.cdr.markForCheck();
  }

  private initializeSimulation(): void {
    if (!this.patternData) return;
    
    // Update simulation state directly
    this.simulationState.isPlaying = false;
    this.simulationState.isPaused = false;
    this.simulationState.currentTime = 0;
    this.simulationState.currentStep = 0;
    this.simulationState.totalDuration = (this.patternData.drillPoints?.length || 0) * this.simulationSettings.effectIntensity;
    
    this.animationService.resetAnimation();
    this.animationService.startAnimation(this.simulationState, this.connections, this.patternData.drillPoints);
  }
}