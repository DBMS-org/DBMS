import { Component, OnInit, OnDestroy, ViewChild, ElementRef, ChangeDetectorRef, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, takeUntil, firstValueFrom } from 'rxjs';
import { BlastSequenceDataService } from '../shared/services/blast-sequence-data.service';
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
import { PatternData, BlastConnection } from '../drilling-pattern-creator/models/drill-point.model';
import { TimelineComponent } from './components/timeline/timeline.component';
import { PatternRendererComponent } from './components/pattern-renderer/pattern-renderer.component';
import { ViewControlsComponent } from './components/view-controls/view-controls.component';
import { AnimationService } from './services/animation.service';
import { ReportExportService } from './services/report-export.service';
import { NavigationController } from '../shared/services/navigation-controller.service';
import { StateService } from '../../../core/services/state.service';

@Component({
  selector: 'app-blast-sequence-simulator',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule,
    TimelineComponent,
    PatternRendererComponent,
    ViewControlsComponent
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
    private dataService: BlastSequenceDataService,
    private router: Router,
    private animationService: AnimationService,
    private reportExportService: ReportExportService,
    private cdr: ChangeDetectorRef,
    private navigationController: NavigationController,
    private stateService: StateService
  ) {}

  ngOnInit(): void {
    // Resolve site context via StateService, fallback to URL parsing
    const { activeProjectId, activeSiteId } = this.stateService.currentState;
    let projectId = activeProjectId;
    let siteId = activeSiteId;

    if (!projectId || !siteId) {
      const routeMatch = this.router.url.match(/project-management\/(\d+)\/sites\/(\d+)/);
      projectId = routeMatch ? +routeMatch[1] : 1;
      siteId = routeMatch ? +routeMatch[2] : 3;

      // Store back to global state so later components have it
      this.stateService.setProjectId(projectId);
      this.stateService.setSiteId(siteId);
    }

    this.currentProjectId = projectId;
    this.currentSiteId = siteId;
    this.dataService.setSiteContext(projectId, siteId);

    // Wait for data to load before enabling simulation
    this.dataService.blastSequenceData$
      .pipe(takeUntil(this.destroy$))
      .subscribe(data => {
        this.patternData = data.patternData;
        this.connections = data.connections;
        this.simulationState = {
          ...data.simulationState,
          isPlaying: false,
          isPaused: false,
          currentTime: 0,
          currentStep: 0,
          totalDuration: data.simulationState.totalDuration
        };
        this.simulationSettings = data.simulationSettings;
        this.animationService.resetAnimation();
        // If data is missing, show a warning
        if (!this.patternData || this.connections.length === 0) {
          // Optionally, show a message or redirect
        }
      });

    this.dataService.validation$
      .pipe(takeUntil(this.destroy$))
      .subscribe(validation => {
        this.validation = validation;
      });

    this.animationService.getAnimationFrame()
      .pipe(takeUntil(this.destroy$))
      .subscribe(frame => {
        this.currentFrame = frame;
        this.simulationState.currentTime = frame.time;
        // Manually trigger change detection because animation frames run outside Angular's zone
        this.cdr.detectChanges();
      });

    this.dataService.setCurrentWorkflowStep('simulate');
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.animationService.stopAnimation();
  }

  // Animation Controls
  play(): void {
    if (!this.patternData || this.connections.length === 0) return;
    this.dataService.updateSimulationState({ 
      isPlaying: true, 
      isPaused: false,
      currentTime: 0
    });
    this.animationService.startAnimation(this.simulationState, this.connections);
  }

  pause(): void {
    this.dataService.updateSimulationState({ 
      isPlaying: false, 
      isPaused: true 
    });
    this.animationService.stopAnimation();
  }

  stop(): void {
    this.dataService.updateSimulationState({ 
      isPlaying: false, 
      isPaused: false,
      currentTime: 0,
      currentStep: 0
    });
    this.animationService.stopAnimation();
    this.animationService.resetAnimation();
  }

  seekToTime(time: number): void {
    const clampedTime = Math.max(0, Math.min(time, this.simulationState.totalDuration));
    this.dataService.updateSimulationState({ currentTime: clampedTime });
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
    if (!this.patternData) {
      console.warn('No pattern data available for report export');
      return;
    }

    try {
      const currentProject = await firstValueFrom(this.dataService.currentProject$);
      await this.reportExportService.exportReport({
        patternData: this.patternData,
        connections: this.connections,
        simulationSettings: this.simulationSettings,
        simulationState: this.simulationState,
        projectName: currentProject?.name || 'Unnamed Project',
        canvasRef: this.containerRef,
        metrics: {
          totalBlastTime: this.simulationState.totalDuration,
          holes: this.patternData.drillPoints.length,
          connections: this.connections.length,
          averageDelayBetweenHoles: this.metrics.averageDelayBetweenHoles,
          maxSimultaneousDetonations: this.metrics.maxSimultaneousDetonations,
          efficiencyScore: this.metrics.efficiencyScore,
          safetyScore: this.metrics.safetyScore,
          connectionUtilization: this.metrics.connectionUtilization
        }
      });
    } catch (error) {
      console.error('Error exporting report:', error);
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
      // Save pattern data and connections
      this.dataService.setPatternData(this.patternData, true);
      this.dataService.setConnections(this.connections, true);
      this.dataService.savePatternData();
      this.dataService.saveConnections();

      this.isSaved = true;
      this.saveTimeout = setTimeout(() => {
        this.isSaved = false;
      }, 3000);
    } catch (error) {
      console.error('Error saving simulation:', error);
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
    this.navigationController.navigateToPatternCreator(this.currentProjectId, this.currentSiteId);
  }

  goToSequenceDesigner(): void {
    this.navigationController.navigateToSequenceDesigner(this.currentProjectId, this.currentSiteId);
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
} 