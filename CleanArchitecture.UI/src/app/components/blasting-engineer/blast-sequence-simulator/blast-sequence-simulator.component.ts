import { Component, OnInit, OnDestroy, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, takeUntil, combineLatest, interval, animationFrameScheduler } from 'rxjs';
import { map, filter, throttleTime } from 'rxjs/operators';
import Konva from 'konva';
import { BlastSequenceDataService } from '../shared/services/blast-sequence-data.service';
import { 
  SimulationState, 
  SimulationSettings, 
  ViewSettings,
  SimulationValidation,
  SimulationMetrics,
  AnimationFrame,
  BlastEffect,
  BlastEffectType,
  HoleAnimationState,
  ConnectionAnimationState,
  TimelineMarker,
  SimulationEvent,
  SimulationEventType
} from '../shared/models/simulation.model';
import { PatternData, DrillPoint, BlastConnection } from '../drilling-pattern-creator/models/drill-point.model';

interface ViewMode {
  id: string;
  name: string;
  description: string;
  icon: string;
}

@Component({
  selector: 'app-blast-sequence-simulator',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './blast-sequence-simulator.component.html',
  styleUrls: ['./blast-sequence-simulator.component.scss']
})
export class BlastSequenceSimulatorComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('simulationCanvas', { static: true }) canvasRef!: ElementRef<HTMLDivElement>;
  @ViewChild('timelineCanvas', { static: true }) timelineCanvasRef!: ElementRef<HTMLCanvasElement>;

  private destroy$ = new Subject<void>();
  private stage!: Konva.Stage;
  private mainLayer!: Konva.Layer;
  private effectsLayer!: Konva.Layer;
  private timelineCtx!: CanvasRenderingContext2D;
  private animationFrameId: number | null = null;
  private animationStartTime: number = 0;
  private holeShapes: Map<string, Konva.Circle> = new Map();
  private connectionShapes: Map<string, Konva.Group> = new Map();
  private labelShapes: Map<string, Konva.Text> = new Map();

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
    safetyScore: 100,
    connectionUtilization: 0
  };

  // UI State
  selectedViewMode: ViewMode = { id: '2d', name: '2D View', description: 'Top-down view', icon: 'view_quilt' };
  showControlPanel = true;
  showMetricsPanel = true;
  showValidationPanel = true;
  isFullscreen = false;

  // Save functionality
  public isSaved = false;
  private saveTimeout: any;

  // Animation State
  private currentFrame: AnimationFrame = {
    time: 0,
    holeStates: new Map(),
    connectionStates: new Map(),
    activeEffects: []
  };
  private timelineMarkers: TimelineMarker[] = [];
  private simulationEvents: SimulationEvent[] = [];
  private activeEffects: BlastEffect[] = [];

  // View Configuration
  viewModes: ViewMode[] = [
    { id: '2d', name: '2D View', description: 'Top-down view', icon: 'view_quilt' },
    { id: 'timeline', name: 'Timeline View', description: 'Sequence timeline', icon: 'timeline' }
  ];

  // Canvas Configuration
  canvasConfig = {
    width: 800,
    height: 600,
    padding: 50,
    scale: 1,
    offsetX: 0,
    offsetY: 0
  };

  constructor(
    private dataService: BlastSequenceDataService,
    private router: Router
  ) {}

  ngOnInit(): void {
    console.log('Blast Simulator - ngOnInit called');
    
    // Initialize site context from route parameters
    this.initializeSiteContext();
    
    this.initializeSubscriptions();
    this.loadSimulationData();
    this.dataService.setCurrentWorkflowStep('simulate');
    console.log('Blast Simulator - ngOnInit completed');
  }

  private initializeSiteContext(): void {
    // Get projectId and siteId from route URL pattern: /blasting-engineer/project-management/:projectId/sites/:siteId/simulator
    const routeMatch = this.router.url.match(/project-management\/(\d+)\/sites\/(\d+)/);
    const projectId = routeMatch ? +routeMatch[1] : null;
    const siteId = routeMatch ? +routeMatch[2] : null;
    
    if (projectId && siteId) {
      console.log('Simulator - Setting site context:', { projectId, siteId });
      this.dataService.setSiteContext(projectId, siteId);
    } else {
      console.warn('Simulator - Could not extract site context from route:', this.router.url);
      console.warn('Route match result:', routeMatch);
      
      // For testing purposes, use the known valid site combination
      console.log('Falling back to test site context: Project 1, Site 3');
      this.dataService.setSiteContext(1, 3);
    }
  }

  ngAfterViewInit(): void {
    console.log('Blast Simulator - ngAfterViewInit called');
    try {
      this.initializeCanvas();
      this.initializeTimelineCanvas();
      this.setupCanvasEventListeners();
      this.resizeCanvases();
      this.generateSimulationEvents();
      this.renderInitialState();
      console.log('Blast Simulator - ngAfterViewInit completed successfully');
    } catch (error) {
      console.error('Error in ngAfterViewInit:', error);
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.stopAnimation();
    this.removeCanvasEventListeners();
  }

  private removeCanvasEventListeners(): void {
    if (this.stage) {
      this.stage.destroy();
    }
  }

  // Initialization
  private initializeSubscriptions(): void {
    // Subscribe to data changes
    this.dataService.blastSequenceData$
      .pipe(takeUntil(this.destroy$))
      .subscribe(data => {
        const hadPatternData = !!this.patternData;
        const hadConnections = this.connections.length > 0;
        
        this.patternData = data.patternData;
        this.connections = data.connections;
        this.simulationState = data.simulationState;
        this.simulationSettings = data.simulationSettings;
        
        // Only reset the view if this is new pattern data, not just updates
        const isNewPattern = !hadPatternData && !!data.patternData;
        const isNewConnections = !hadConnections && data.connections.length > 0;
        
        if (isNewPattern || isNewConnections) {
          this.onDataChanged();
        } else {
          // Just re-render without resetting the view
          this.renderKonvaPattern();
        }
      });

    // Subscribe to validation results
    this.dataService.validation$
      .pipe(takeUntil(this.destroy$))
      .subscribe(validation => {
        this.validation = validation;
      });

    // Calculate metrics when data changes
    combineLatest([
      this.dataService.patternData$,
      this.dataService.connections$
    ]).pipe(
      takeUntil(this.destroy$),
      filter(([pattern, connections]) => pattern !== null && connections.length > 0)
    ).subscribe(() => {
      this.metrics = this.dataService.calculateSimulationMetrics();
      this.timelineMarkers = this.dataService.generateTimelineMarkers();
    });
  }

  private loadSimulationData(): void {
    const patternData = this.dataService.getPatternData();
    const connections = this.dataService.getConnections();

    console.log('Loading simulation data:', {
      patternData: !!patternData,
      drillPoints: patternData?.drillPoints?.length || 0,
      connections: connections.length
    });

    // Assign the data to component properties
    this.patternData = patternData;
    this.connections = connections;

    if (!patternData || connections.length === 0) {
      console.warn('Missing simulation data:', {
        hasPatternData: !!patternData,
        connectionCount: connections.length
      });
      // Don't redirect automatically, let user decide
    } else {
      // If we have data, trigger initial rendering
      setTimeout(() => {
        this.onDataChanged();
      }, 0);
    }
  }

  private initializeCanvas(): void {
    const container = this.canvasRef.nativeElement;
    
    // Create Konva stage
    this.stage = new Konva.Stage({
      container: container,
      width: this.canvasConfig.width,
      height: this.canvasConfig.height
    });

    // Create layers
    this.mainLayer = new Konva.Layer();
    this.effectsLayer = new Konva.Layer();
    
    this.stage.add(this.mainLayer);
    this.stage.add(this.effectsLayer);

    // Enable built-in zoom and pan
    this.setupKonvaInteractions();
  }

  private initializeTimelineCanvas(): void {
    const canvas = this.timelineCanvasRef.nativeElement;
    this.timelineCtx = canvas.getContext('2d')!;
    
    // Set timeline canvas size
    canvas.width = 800;
    canvas.height = 100;
  }

  private setupKonvaInteractions(): void {
    // Enable zoom with mouse wheel
    const scaleBy = 1.05;
    
    this.stage.on('wheel', (e) => {
      e.evt.preventDefault();
      
      const oldScale = this.stage.scaleX();
      const pointer = this.stage.getPointerPosition();
      
      if (!pointer) return;
      
      const mousePointTo = {
        x: (pointer.x - this.stage.x()) / oldScale,
        y: (pointer.y - this.stage.y()) / oldScale,
      };
      
      const newScale = e.evt.deltaY > 0 ? oldScale * scaleBy : oldScale / scaleBy;
      
      // Limit zoom range
      const clampedScale = Math.max(0.1, Math.min(5, newScale));
      
      this.stage.scale({ x: clampedScale, y: clampedScale });
      
      const newPos = {
        x: pointer.x - mousePointTo.x * clampedScale,
        y: pointer.y - mousePointTo.y * clampedScale,
      };
      
      this.stage.position(newPos);
    });

    // Enable panning by dragging the stage
    this.stage.draggable(true);
  }

  private setupCanvasEventListeners(): void {
    // Konva handles events internally, no need for manual event listeners
  }

  private resizeCanvases(): void {
    // Handle responsive canvas sizing
    if (this.patternData && this.patternData.drillPoints.length > 0) {
      this.calculateOptimalCanvasSize();
      this.renderInitialState();
    }
  }

  resetZoom(): void {
    if (this.patternData && this.patternData.drillPoints.length > 0) {
      this.resetKonvaView();
      this.renderKonvaPattern();
    }
  }

  private resetKonvaView(): void {
    if (!this.stage || !this.patternData) return;

    // Calculate bounds with the same scaling factor used in rendering (60x)
    const scaleFactor = 60;
    const holes = this.patternData.drillPoints;
    const scaledHoles = holes.map(h => ({ x: h.x * scaleFactor, y: h.y * scaleFactor }));
    
    const minX = Math.min(...scaledHoles.map(h => h.x));
    const maxX = Math.max(...scaledHoles.map(h => h.x));
    const minY = Math.min(...scaledHoles.map(h => h.y));
    const maxY = Math.max(...scaledHoles.map(h => h.y));

    const patternWidth = Math.max(maxX - minX, 100);
    const patternHeight = Math.max(maxY - minY, 100);
    const centerX = (minX + maxX) / 2;
    const centerY = (minY + maxY) / 2;

    // Calculate scale to fit in stage with padding
    const padding = 100;
    const availableWidth = this.canvasConfig.width - padding * 2;
    const availableHeight = this.canvasConfig.height - padding * 2;
    
    const scaleX = availableWidth / patternWidth;
    const scaleY = availableHeight / patternHeight;
    const scale = Math.min(scaleX, scaleY, 1.5); // Reasonable max scale

    // Reset stage position and scale
    this.stage.position({
      x: this.canvasConfig.width / 2 - centerX * scale,
      y: this.canvasConfig.height / 2 - centerY * scale
    });
    this.stage.scale({ x: scale, y: scale });
  }

  private renderKonvaPattern(): void {
    if (!this.patternData || !this.mainLayer) return;

    // Clear existing shapes
    this.mainLayer.destroyChildren();
    this.holeShapes.clear();
    this.connectionShapes.clear();
    this.labelShapes.clear();

    // First render a background grid for better visualization (only if enabled)
    if (this.viewSettings.showGrid) {
    this.renderBackgroundGrid();
    }

    // Render drill holes with enhanced styling
    this.patternData.drillPoints.forEach(hole => {
      this.renderAdvancedDrillHole(hole);
    });

    // Render connections with enhanced styling (only if enabled)
    if (this.simulationSettings.showConnections) {
    this.connections.forEach(conn => {
      this.renderAdvancedConnection(conn);
    });
    }

    // Add scale reference
    this.renderScaleReference();

    // Add pattern information
    this.renderPatternInfo();

    this.mainLayer.batchDraw();
  }

  private renderBackgroundGrid(): void {
    const gridSize = 50;
    const stageWidth = this.stage.width();
    const stageHeight = this.stage.height();
    
    // Get grid color based on theme
    const gridColor = this.getGridColorByTheme();
    
    // Create grid lines
    const gridGroup = new Konva.Group({
      opacity: 0.3
    });

    // Vertical lines
    for (let x = 0; x <= stageWidth; x += gridSize) {
      const line = new Konva.Line({
        points: [x, 0, x, stageHeight],
        stroke: gridColor,
        strokeWidth: 0.5
      });
      gridGroup.add(line);
    }

    // Horizontal lines
    for (let y = 0; y <= stageHeight; y += gridSize) {
      const line = new Konva.Line({
        points: [0, y, stageWidth, y],
        stroke: gridColor,
        strokeWidth: 0.5
      });
      gridGroup.add(line);
    }

    this.mainLayer.add(gridGroup);
  }

  private getGridColorByTheme(): string {
    switch (this.viewSettings.colorTheme) {
      case 'dark': return '#333333';
      case 'high-contrast': return '#000000';
      case 'colorblind': return '#cccccc';
      default: return '#e0e0e0';
    }
  }

  private renderAdvancedDrillHole(hole: any): void {
    // Apply proper spacing and scaling for visualization
    const visualX = hole.x * 60; // Scale up for better spacing
    const visualY = hole.y * 60; // Scale up for better spacing
    
    const group = new Konva.Group({
      x: visualX,
      y: visualY
    });

    // Get current state for color coding
    const rawState = this.currentFrame.holeStates.get(hole.id) || HoleAnimationState.READY;
    const state = rawState === HoleAnimationState.READY ? 'READY' :
                 rawState === HoleAnimationState.DETONATING ? 'DETONATING' :
                 rawState === HoleAnimationState.BLASTED ? 'BLASTED' : 'READY';
    
    // Outer ring for depth indicator
    const depthRing = new Konva.Circle({
      radius: 12,
      stroke: this.getHoleColor(state).ring,
      strokeWidth: 2,
      opacity: 0.5
    });

    // Main drill hole circle
    const mainCircle = new Konva.Circle({
      radius: 8,
      fill: this.getHoleColor(state).fill,
      stroke: this.getHoleColor(state).stroke,
      strokeWidth: 2,
      shadowColor: 'rgba(0,0,0,0.3)',
      shadowBlur: 4,
      shadowOffset: { x: 1, y: 1 }
    });

    // Hole ID label with background (controlled by showSequenceNumbers)
    const labelBg = new Konva.Rect({
      x: -15,
      y: -30,
      width: 30,
      height: 16,
      fill: 'rgba(255,255,255,0.9)',
      stroke: '#ddd',
      strokeWidth: 1,
      cornerRadius: 3,
      visible: this.simulationSettings.showSequenceNumbers
    });

    const label = new Konva.Text({
      x: -15,
      y: -27,
      text: hole.id,
      fontSize: 10,
      fontFamily: 'Arial, sans-serif',
      fontStyle: 'bold',
      fill: '#333',
      align: 'center',
      width: 30,
      visible: this.simulationSettings.showSequenceNumbers
    });

    // Drill specifications text
    const specsText = new Konva.Text({
      x: -20,
      y: 15,
      text: `D:${hole.depth || 10}m\nS:${hole.spacing || 3}m\nB:${hole.burden || 2.5}m`,
      fontSize: 8,
      fontFamily: 'Arial, sans-serif',
      fill: '#666',
      align: 'center',
      width: 40,
      visible: this.viewSettings.showHoleDetails // Show/hide based on setting
    });

    group.add(depthRing);
    group.add(mainCircle);
    group.add(labelBg);
    group.add(label);
    group.add(specsText);

    // Enhanced interactions
    group.on('click', () => {
      console.log('Clicked drill hole:', {
        id: hole.id,
        depth: hole.depth,
        spacing: hole.spacing,
        burden: hole.burden,
        coordinates: { x: hole.x, y: hole.y }
      });
      
      // Pulse effect on click
      const tween = new Konva.Tween({
        node: mainCircle,
        duration: 0.3,
        scaleX: 1.3,
        scaleY: 1.3,
        onFinish: () => {
          new Konva.Tween({
            node: mainCircle,
            duration: 0.2,
            scaleX: 1,
            scaleY: 1
          }).play();
        }
      });
      tween.play();
    });

    group.on('mouseenter', () => {
      // Hover effects
      mainCircle.fill(this.getHoleColor(state).hover);
      // Only show specs on hover if not permanently visible
      if (!this.viewSettings.showHoleDetails) {
      specsText.visible(true);
      }
      document.body.style.cursor = 'pointer';
      this.mainLayer.batchDraw();
    });

    group.on('mouseleave', () => {
      mainCircle.fill(this.getHoleColor(state).fill);
      // Only hide specs on leave if not permanently visible
      if (!this.viewSettings.showHoleDetails) {
      specsText.visible(false);
      }
      document.body.style.cursor = 'default';
      this.mainLayer.batchDraw();
    });

    this.mainLayer.add(group);
    this.holeShapes.set(hole.id, mainCircle);
    this.labelShapes.set(hole.id, label);
  }

  private renderAdvancedConnection(conn: any): void {
    const fromHole = this.patternData!.drillPoints.find(h => h.id === conn.fromHoleId);
    const toHole = this.patternData!.drillPoints.find(h => h.id === conn.toHoleId);

    if (!fromHole || !toHole) return;

    // Apply same scaling as holes for consistent positioning
    const fromX = fromHole.x * 60;
    const fromY = fromHole.y * 60;
    const toX = toHole.x * 60;
    const toY = toHole.y * 60;

    const connectionGroup = new Konva.Group();
    const rawState = this.currentFrame.connectionStates.get(conn.id) || ConnectionAnimationState.INACTIVE;
    const state = rawState === ConnectionAnimationState.SIGNAL_PROPAGATING ? 'SIGNAL_PROPAGATING' : 
                 rawState === ConnectionAnimationState.SIGNAL_TRANSMITTED ? 'SIGNAL_TRANSMITTED' : 'INACTIVE';

    // Determine if this connection should be highlighted
    const isActive = state === 'SIGNAL_PROPAGATING' || state === 'SIGNAL_TRANSMITTED';
    const shouldHighlight = this.viewSettings.highlightActiveConnections && isActive;

    // Main connection line with enhanced styling
    const line = new Konva.Line({
      points: [fromX, fromY, toX, toY],
      stroke: shouldHighlight ? '#ff9800' : this.getConnectionColor(state),
      strokeWidth: shouldHighlight ? this.getConnectionWidth(state) + 2 : this.getConnectionWidth(state),
      dash: state === 'SIGNAL_PROPAGATING' ? [5, 5] : [],
      shadowColor: shouldHighlight ? 'rgba(255, 152, 0, 0.5)' : 'rgba(0,0,0,0.2)',
      shadowBlur: shouldHighlight ? 6 : 2,
      shadowOffset: { x: 1, y: 1 }
    });

    // Hidden points rendering if they exist
    if (conn.startPoint && conn.endPoint) {
      // Hidden start point (1) - positioned based on connection data
      const startX = conn.startPoint.x * 60;
      const startY = conn.startPoint.y * 60;
      
      const startPointCircle = new Konva.Circle({
        x: startX,
        y: startY,
        radius: 6,
        fill: state === 'SIGNAL_PROPAGATING' ? '#4CAF50' : 'rgba(255, 255, 255, 0.8)',
        stroke: this.getConnectionColor(state),
        strokeWidth: 2,
        opacity: state === 'INACTIVE' ? 0.5 : 0.9,
        visible: false // Hide wire points as requested
      });
      
      const startPointLabel = new Konva.Text({
        x: startX - 3,
        y: startY - 4,
        text: conn.startPoint.label,
        fontSize: 8,
        fill: 'black',
        fontStyle: 'bold',
        visible: false // Hide wire points as requested
      });

      // Hidden end point (2) - positioned based on connection data  
      const endX = conn.endPoint.x * 60;
      const endY = conn.endPoint.y * 60;
      
      const endPointCircle = new Konva.Circle({
        x: endX,
        y: endY,
        radius: 6,
        fill: state === 'SIGNAL_TRANSMITTED' ? '#FF5722' : 'rgba(255, 255, 255, 0.8)',
        stroke: this.getConnectionColor(state),
        strokeWidth: 2,
        opacity: state === 'INACTIVE' ? 0.5 : 0.9,
        visible: false // Hide wire points as requested
      });
      
      const endPointLabel = new Konva.Text({
        x: endX - 3,
        y: endY - 4,
        text: conn.endPoint.label,
        fontSize: 8,
        fill: 'black',
        fontStyle: 'bold',
        visible: false // Hide wire points as requested
      });
      
      // Add hidden points to the connection group
      connectionGroup.add(startPointCircle);
      connectionGroup.add(startPointLabel);
      connectionGroup.add(endPointCircle);
      connectionGroup.add(endPointLabel);
    }

    // Enhanced arrow
    const arrowPoints = this.calculateArrowPoints(
      { x: fromX, y: fromY }, 
      { x: toX, y: toY }
    );
    const arrow = new Konva.Line({
      points: arrowPoints,
      stroke: this.getConnectionColor(state),
      strokeWidth: 2,
      fill: this.getConnectionColor(state),
      closed: false
    });

    // Timing label with background (controlled by showTiming)
    const midX = (fromX + toX) / 2;
    const midY = (fromY + toY) / 2;
    
    const timingBg = new Konva.Rect({
      x: midX - 20,
      y: midY - 8,
      width: 40,
      height: 16,
      fill: 'rgba(255,255,255,0.9)',
      stroke: '#ddd',
      strokeWidth: 1,
      cornerRadius: 3,
      visible: this.simulationSettings.showTiming
    });

    const timingLabel = new Konva.Text({
      x: midX - 20,
      y: midY - 5,
      text: `${conn.delay}ms`,
      fontSize: 10,
      fontFamily: 'Arial, sans-serif',
      fontStyle: 'bold',
      fill: '#333',
      align: 'center',
      width: 40,
      visible: this.simulationSettings.showTiming
    });

    // Wire sequence label for debugging
    const sequenceLabel = new Konva.Text({
      x: midX - 10,
      y: midY - 25,
      text: `${this.getConnectorTypeName(conn.connectorType)} ${conn.delay}ms`,
      fontSize: 8,
      fontFamily: 'Arial, sans-serif',
      fontStyle: 'bold',
      fill: this.getConnectionColor(state),
      align: 'center',
      visible: this.viewSettings.showConnectionLabels
    });

    connectionGroup.add(line);
    connectionGroup.add(arrow);
    connectionGroup.add(timingBg);
    connectionGroup.add(timingLabel);
    connectionGroup.add(sequenceLabel);

    // Connection hover effects
    connectionGroup.on('mouseenter', () => {
      line.strokeWidth(this.getConnectionWidth(state) + 1);
      line.stroke('#ff9800');
      document.body.style.cursor = 'pointer';
      this.mainLayer.batchDraw();
    });

    connectionGroup.on('mouseleave', () => {
      line.strokeWidth(this.getConnectionWidth(state));
      line.stroke(this.getConnectionColor(state));
      document.body.style.cursor = 'default';
      this.mainLayer.batchDraw();
    });

    this.mainLayer.add(connectionGroup);
    this.connectionShapes.set(conn.id, connectionGroup);
  }

  private renderScaleReference(): void {
    const scaleGroup = new Konva.Group({
      x: 20,
      y: this.stage.height() - 60
    });

    // Scale bar background
    const scaleBg = new Konva.Rect({
      width: 120,
      height: 40,
      fill: 'rgba(255,255,255,0.9)',
      stroke: '#ddd',
      strokeWidth: 1,
      cornerRadius: 5
    });

    // Scale reference line (representing 10 meters)
    const scaleLine = new Konva.Line({
      points: [10, 25, 60, 25],
      stroke: '#333',
      strokeWidth: 2
    });

    // Scale ticks
    const tick1 = new Konva.Line({
      points: [10, 20, 10, 30],
      stroke: '#333',
      strokeWidth: 2
    });

    const tick2 = new Konva.Line({
      points: [60, 20, 60, 30],
      stroke: '#333',
      strokeWidth: 2
    });

    // Scale label
    const scaleLabel = new Konva.Text({
      x: 10,
      y: 5,
      text: '10 meters',
      fontSize: 10,
      fontFamily: 'Arial, sans-serif',
      fill: '#333',
      width: 100,
      align: 'center'
    });

    scaleGroup.add(scaleBg);
    scaleGroup.add(scaleLine);
    scaleGroup.add(tick1);
    scaleGroup.add(tick2);
    scaleGroup.add(scaleLabel);

    this.mainLayer.add(scaleGroup);
  }

  private renderPatternInfo(): void {
    if (!this.patternData) return;

    const infoGroup = new Konva.Group({
      x: this.stage.width() - 180,
      y: 20
    });

    // Info panel background
    const infoBg = new Konva.Rect({
      width: 160,
      height: 120,
      fill: 'rgba(255,255,255,0.95)',
      stroke: '#ddd',
      strokeWidth: 1,
      cornerRadius: 5,
      shadowColor: 'rgba(0,0,0,0.1)',
      shadowBlur: 4,
      shadowOffset: { x: 2, y: 2 }
    });

    // Title
    const title = new Konva.Text({
      x: 10,
      y: 10,
      text: 'Pattern Information',
      fontSize: 12,
      fontFamily: 'Arial, sans-serif',
      fontStyle: 'bold',
      fill: '#333',
      width: 140
    });

    // Pattern details
    const totalHoles = this.patternData.drillPoints.length;
    const totalConnections = this.connections.length;
    const avgSpacing = this.patternData.drillPoints.length > 0 ? 
      (this.patternData.drillPoints.reduce((sum, hole) => sum + (hole.spacing || 3), 0) / this.patternData.drillPoints.length).toFixed(1) : '0';
    const avgBurden = this.patternData.drillPoints.length > 0 ? 
      (this.patternData.drillPoints.reduce((sum, hole) => sum + (hole.burden || 2.5), 0) / this.patternData.drillPoints.length).toFixed(1) : '0';
    const avgDepth = this.patternData.drillPoints.length > 0 ? 
      (this.patternData.drillPoints.reduce((sum, hole) => sum + (hole.depth || 10), 0) / this.patternData.drillPoints.length).toFixed(1) : '0';

    const details = new Konva.Text({
      x: 10,
      y: 30,
      text: `Drill Holes: ${totalHoles}\nConnections: ${totalConnections}\nAvg. Spacing: ${avgSpacing}m\nAvg. Burden: ${avgBurden}m\nAvg. Depth: ${avgDepth}m`,
      fontSize: 10,
      fontFamily: 'Arial, sans-serif',
      fill: '#666',
      lineHeight: 1.4,
      width: 140
    });

    infoGroup.add(infoBg);
    infoGroup.add(title);
    infoGroup.add(details);

    this.mainLayer.add(infoGroup);
  }

  private getHoleColor(state: string): { fill: string; stroke: string; ring: string; hover: string } {
    const theme = this.viewSettings.colorTheme;
    
    if (theme === 'high-contrast') {
      switch (state) {
        case 'READY': return { fill: '#0000FF', stroke: '#000080', ring: '#E0E0FF', hover: '#4040FF' };
        case 'DETONATING': return { fill: '#FF0000', stroke: '#CC0000', ring: '#FFE0E0', hover: '#FF4040' };
        case 'BLASTED': return { fill: '#00FF00', stroke: '#00CC00', ring: '#E0FFE0', hover: '#40FF40' };
        default: return { fill: '#808080', stroke: '#404040', ring: '#F0F0F0', hover: '#A0A0A0' };
      }
    } else if (theme === 'colorblind') {
      switch (state) {
        case 'READY': return { fill: '#0088FF', stroke: '#0066CC', ring: '#CCE8FF', hover: '#4499FF' };
        case 'DETONATING': return { fill: '#FF8800', stroke: '#CC6600', ring: '#FFE4CC', hover: '#FF9944' };
        case 'BLASTED': return { fill: '#8800FF', stroke: '#6600CC', ring: '#E4CCFF', hover: '#9944FF' };
        default: return { fill: '#808080', stroke: '#606060', ring: '#F0F0F0', hover: '#A0A0A0' };
      }
    } else if (theme === 'dark') {
      switch (state) {
        case 'READY': return { fill: '#64B5F6', stroke: '#1976D2', ring: '#2196F3', hover: '#90CAF9' };
        case 'DETONATING': return { fill: '#FF8A65', stroke: '#F57C00', ring: '#FF9800', hover: '#FFAB91' };
        case 'BLASTED': return { fill: '#81C784', stroke: '#388E3C', ring: '#4CAF50', hover: '#A5D6A7' };
        default: return { fill: '#BDBDBD', stroke: '#757575', ring: '#9E9E9E', hover: '#E0E0E0' };
      }
    } else {
      // Default theme
    switch (state) {
      case 'READY':
        return {
          fill: '#e3f2fd',
          stroke: '#1976d2',
          ring: '#bbdefb',
          hover: '#bbdefb'
        };
      case 'DETONATING':
        return {
          fill: '#fff3e0',
          stroke: '#f57f17',
          ring: '#ffcc02',
          hover: '#ffe082'
        };
      case 'BLASTED':
        return {
          fill: '#ffebee',
          stroke: '#d32f2f',
          ring: '#ef5350',
          hover: '#ffcdd2'
        };
      default:
        return {
          fill: '#f5f5f5',
          stroke: '#9e9e9e',
          ring: '#e0e0e0',
          hover: '#eeeeee'
        };
      }
    }
  }

  private getConnectionColor(state: string): string {
    switch (state) {
      case 'INACTIVE':
        return '#dee2e6';
      case 'SIGNAL_PROPAGATING':
        return '#ff9800';
      case 'SIGNAL_TRANSMITTED':
        return '#4caf50';
      default:
        return '#dee2e6';
    }
  }

  private getConnectionWidth(state: string): number {
    switch (state) {
      case 'INACTIVE':
        return 2;
      case 'SIGNAL_PROPAGATING':
        return 4;
      case 'SIGNAL_TRANSMITTED':
        return 3;
      default:
        return 2;
    }
  }

  private calculateArrowPoints(fromHole: any, toHole: any): number[] {
    const angle = Math.atan2(toHole.y - fromHole.y, toHole.x - fromHole.x);
    const arrowLength = 10;
    const arrowAngle = Math.PI / 6;

    const x1 = toHole.x - arrowLength * Math.cos(angle - arrowAngle);
    const y1 = toHole.y - arrowLength * Math.sin(angle - arrowAngle);
    const x2 = toHole.x - arrowLength * Math.cos(angle + arrowAngle);
    const y2 = toHole.y - arrowLength * Math.sin(angle + arrowAngle);

    return [toHole.x, toHole.y, x1, y1, toHole.x, toHole.y, x2, y2];
  }

  private calculateOptimalCanvasSize(): void {
    if (!this.patternData || this.patternData.drillPoints.length === 0) return;

    // Calculate pattern bounds
    const holes = this.patternData.drillPoints;
    const minX = Math.min(...holes.map(h => h.x));
    const maxX = Math.max(...holes.map(h => h.x));
    const minY = Math.min(...holes.map(h => h.y));
    const maxY = Math.max(...holes.map(h => h.y));

    // Calculate pattern size
    const patternWidth = maxX - minX;
    const patternHeight = maxY - minY;
    const availableWidth = this.canvasConfig.width - (this.canvasConfig.padding * 2);
    const availableHeight = this.canvasConfig.height - (this.canvasConfig.padding * 2);

    let scale: number;

    // Handle different cases for pattern size
    if (patternWidth === 0 && patternHeight === 0) {
      // Single point - use a reasonable scale
      scale = 2.0;
    } else if (patternWidth === 0 || patternHeight === 0) {
      // Line pattern - use the non-zero dimension
      const dimension = Math.max(patternWidth, patternHeight);
      scale = Math.min(availableWidth, availableHeight) / dimension * 0.6;
    } else {
      // Regular pattern with width and height
      const scaleX = availableWidth / patternWidth;
      const scaleY = availableHeight / patternHeight;
      scale = Math.min(scaleX, scaleY) * 0.6;
    }

    // Ensure minimum scale for visibility
    this.canvasConfig.scale = Math.max(scale, 0.5);

    // Calculate center point of pattern
    const patternCenterX = (minX + maxX) / 2;
    const patternCenterY = (minY + maxY) / 2;

    // Calculate offset to center pattern in canvas
    this.canvasConfig.offsetX = (this.canvasConfig.width / 2) - (patternCenterX * this.canvasConfig.scale);
    this.canvasConfig.offsetY = (this.canvasConfig.height / 2) - (patternCenterY * this.canvasConfig.scale);

    console.log('Canvas size calculated:', {
      patternBounds: { minX, maxX, minY, maxY },
      patternSize: { width: patternWidth, height: patternHeight },
      patternCenter: { x: patternCenterX, y: patternCenterY },
      availableSize: { width: availableWidth, height: availableHeight },
      scale: this.canvasConfig.scale,
      offset: { x: this.canvasConfig.offsetX, y: this.canvasConfig.offsetY }
    });
  }

  // Data Management
  private onDataChanged(): void {
    this.generateSimulationEvents();
    this.resetAnimation();
    this.resetKonvaView();
    this.renderKonvaPattern();
  }

  private generateSimulationEvents(): void {
    if (!this.connections.length) return;

    this.simulationEvents = [];
    
    // Group connections by their starting holes (for simultaneous starts)
    const connectionsByFromHole = new Map<string, BlastConnection[]>();
    this.connections.forEach(conn => {
      if (!connectionsByFromHole.has(conn.fromHoleId)) {
        connectionsByFromHole.set(conn.fromHoleId, []);
      }
      connectionsByFromHole.get(conn.fromHoleId)!.push(conn);
    });

    // Group connections by their ending holes (for finding what gets triggered)
    const connectionsByToHole = new Map<string, BlastConnection[]>();
    this.connections.forEach(conn => {
      if (!connectionsByToHole.has(conn.toHoleId)) {
        connectionsByToHole.set(conn.toHoleId, []);
      }
      connectionsByToHole.get(conn.toHoleId)!.push(conn);
    });

    // Track which connections have been processed
    const processedConnections = new Set<string>();
    const triggerTimes = new Map<string, number>(); // connectionId -> trigger time

    // Find initial connections - these are connections that start from holes 
    // that are NOT the target of any other connection's Point 2
    const allToHoleIds = new Set(this.connections.map(conn => conn.toHoleId));
    const initialFromHoles = new Set<string>();
    
    this.connections.forEach(conn => {
      // If this connection starts from a hole that is not a target of any other connection,
      // it's an initial connection
      if (!allToHoleIds.has(conn.fromHoleId)) {
        initialFromHoles.add(conn.fromHoleId);
      }
    });
    
    // Get ALL connections that start from initial holes (multiple Point 1s on same hole)
    const initialConnections: BlastConnection[] = [];
    initialFromHoles.forEach(holeId => {
      const connectionsFromHole = connectionsByFromHole.get(holeId) || [];
      initialConnections.push(...connectionsFromHole);
    });
    
    console.log('Initial holes with Point 1s:', Array.from(initialFromHoles));
    console.log('Initial connections found:', initialConnections.map(c => `${c.id} (${this.getConnectorTypeName(c.connectorType)})`));

    // Process connections wave by wave
    const processWave = (connectionsToProcess: BlastConnection[], waveStartTime: number) => {
      if (connectionsToProcess.length === 0) return;

      console.log(`Processing wave at time ${waveStartTime}ms with connections:`, 
        connectionsToProcess.map(c => `${c.id} (${this.getConnectorTypeName(c.connectorType)} ${c.delay}ms from ${c.fromHoleId})`));

      // All connections in this wave start simultaneously
      connectionsToProcess.forEach(connection => {
        if (processedConnections.has(connection.id)) {
          console.log(`Skipping already processed connection: ${connection.id}`);
          return;
        }
        
        processedConnections.add(connection.id);
        triggerTimes.set(connection.id, waveStartTime);

        console.log(`Processing connection: ${connection.id} (${this.getConnectorTypeName(connection.connectorType)} ${connection.delay}ms) from hole ${connection.fromHoleId} to ${connection.toHoleId}`);

        // Hidden point 1 (start point) activation
      this.simulationEvents.push({
          time: waveStartTime,
        type: SimulationEventType.SIGNAL_START,
          targetId: `${connection.id}_start`,
          data: { 
            connectionId: connection.id,
            pointType: 'startPoint',
            pointLabel: '1',
            position: connection.startPoint
          }
        });
        
        // Signal propagation along the wire (from point 1 to point 2)
        const propagationDelay = 25;
        const signalPropagationTime = waveStartTime + propagationDelay;
        
        this.simulationEvents.push({
          time: signalPropagationTime,
          type: SimulationEventType.SIGNAL_ARRIVE,
        targetId: connection.id,
          data: { 
            fromHoleId: connection.fromHoleId, 
            toHoleId: connection.toHoleId,
            wireSequence: connection.sequence
          }
      });

        // Hidden point 2 (end point) activation
        const endPointActivationTime = signalPropagationTime + 10;
      this.simulationEvents.push({
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

        // Hole detonation event
        const detonationTime = endPointActivationTime + 15;
        this.simulationEvents.push({
          time: detonationTime,
        type: SimulationEventType.HOLE_DETONATE,
        targetId: connection.toHoleId,
          data: { 
            delay: connection.delay, 
            type: connection.connectorType,
            wireSequence: connection.sequence,
            triggeredByWire: connection.id
          }
      });

        // Blast effect
      this.simulationEvents.push({
          time: detonationTime,
        type: SimulationEventType.EFFECT_START,
        targetId: connection.toHoleId,
          data: { 
            effectType: BlastEffectType.EXPLOSION, 
            duration: 1000,
            triggeredByWire: connection.id
          }
        });

        console.log(`${this.getConnectorTypeName(connection.connectorType)} ${connection.delay}ms: Start=${waveStartTime}ms, EndPoint=${endPointActivationTime}ms, Detonation=${detonationTime}ms`);
      });

      // Find next wave of connections to trigger
      // Look for connections that start from holes where current wave ends
      const nextWaveConnections: BlastConnection[] = [];
      const nextWaveTime = Math.max(...connectionsToProcess.map(conn => {
        const endTime = triggerTimes.get(conn.id)! + 25 + 10; // propagation + end point activation
        return endTime;
      })) + 500; // 500ms delay between waves

      connectionsToProcess.forEach(connection => {
        // Find connections that should be triggered by this connection's point 2
        // Point 2 can only trigger Point 1 (not another Point 2)
        const potentialNextConnections = connectionsByFromHole.get(connection.toHoleId) || [];
        
        potentialNextConnections.forEach(nextConn => {
          if (!processedConnections.has(nextConn.id)) {
            // Check if this connection's point 2 should trigger the next connection's point 1
            // For now, we'll trigger all connections starting from the same hole
            if (!nextWaveConnections.includes(nextConn)) {
              nextWaveConnections.push(nextConn);
            }
          }
      });
    });

      // Process next wave if there are connections to process
      if (nextWaveConnections.length > 0) {
        processWave(nextWaveConnections, nextWaveTime);
      }
    };

    // Start with initial connections at the earliest delay time among them
    const initialStartTime = initialConnections.length > 0 ? 
      Math.min(...initialConnections.map(conn => conn.delay)) : 0;
    
    console.log(`Starting initial wave at time: ${initialStartTime}ms with ${initialConnections.length} connections`);
    processWave(initialConnections, initialStartTime);

    // Sort all events by time to ensure proper execution order
    this.simulationEvents.sort((a, b) => a.time - b.time);
    
    // Detect timing conflicts and overlaps
    const timingConflicts = this.detectTimingConflicts();
    
    // Update validation with timing conflicts
    this.updateValidationWithConflicts(timingConflicts);
    
    // Calculate total duration based on the last event + effect duration
    const lastEventTime = this.simulationEvents.length > 0 ? 
      Math.max(...this.simulationEvents.map(e => e.time)) : 0;
    const totalDuration = lastEventTime + 1500; // Add buffer for effects to complete
    
    // Update simulation state with new duration and total steps
    this.dataService.updateSimulationState({
      totalDuration: totalDuration,
      totalSteps: this.simulationEvents.length,
      currentTime: 0,
      currentStep: 0
    });
    
    console.log('Generated wire sequence events:', this.simulationEvents);
    console.log(`Total simulation duration: ${totalDuration}ms with ${this.simulationEvents.length} events`);
    if (timingConflicts.length > 0) {
      console.warn('Timing conflicts detected:', timingConflicts);
    }
  }

  // Animation Controls
  play(): void {
    console.log('Play button clicked!', {
      isPlaying: this.simulationState.isPlaying,
      patternData: !!this.patternData,
      connections: this.connections.length
    });
    
    if (this.simulationState.isPlaying) {
      console.log('Already playing, ignoring');
      return;
    }

    this.dataService.updateSimulationState({ 
      isPlaying: true, 
      isPaused: false 
    });

    this.animationStartTime = performance.now() - this.simulationState.currentTime;
    this.startAnimationLoop();
    console.log('Animation started');
  }

  pause(): void {
    console.log('Pause button clicked!');
    this.dataService.updateSimulationState({ 
      isPlaying: false, 
      isPaused: true 
    });
    this.stopAnimation();
  }

  stop(): void {
    this.dataService.updateSimulationState({ 
      isPlaying: false, 
      isPaused: false,
      currentTime: 0,
      currentStep: 0
    });
    this.stopAnimation();
    this.resetAnimation();
    this.renderInitialState();
  }

  restart(): void {
    this.stop();
    setTimeout(() => this.play(), 100);
  }

  setPlaybackSpeed(speed: number): void {
    this.dataService.updateSimulationState({ playbackSpeed: speed });
    
    if (this.simulationState.isPlaying) {
      // Restart animation with new speed
      this.animationStartTime = performance.now() - (this.simulationState.currentTime / speed);
    }
  }

  seekToTime(time: number): void {
    const clampedTime = Math.max(0, Math.min(time, this.simulationState.totalDuration));
    this.dataService.updateSimulationState({ currentTime: clampedTime });
    
    if (this.simulationState.isPlaying) {
      this.animationStartTime = performance.now() - clampedTime;
    }
    
    this.updateAnimationFrame(clampedTime);
    this.render();
  }

  seekToStep(stepIndex: number): void {
    if (stepIndex < 0 || stepIndex >= this.simulationEvents.length) return;
    
    const event = this.simulationEvents[stepIndex];
    this.seekToTime(event.time);
    this.dataService.updateSimulationState({ currentStep: stepIndex });
  }

  // Animation Loop
  private startAnimationLoop(): void {
    this.animationFrameId = requestAnimationFrame(() => this.animationLoop());
  }

  private animationLoop(): void {
    if (!this.simulationState.isPlaying) return;

    const now = performance.now();
    const elapsed = (now - this.animationStartTime) * this.simulationState.playbackSpeed;
    
    // Update current time
    this.dataService.updateSimulationState({ currentTime: elapsed });

    // Check if animation is complete
    if (elapsed >= this.simulationState.totalDuration) {
      this.pause();
      return;
    }

    // Update animation frame
    this.updateAnimationFrame(elapsed);
    
    // Render frame
    this.render();

    // Continue animation
    this.animationFrameId = requestAnimationFrame(() => this.animationLoop());
  }

  private stopAnimation(): void {
    if (this.animationFrameId) {
      cancelAnimationFrame(this.animationFrameId);
      this.animationFrameId = null;
    }
  }

  private resetAnimation(): void {
    this.currentFrame = {
      time: 0,
      holeStates: new Map(),
      connectionStates: new Map(),
      activeEffects: []
    };
    this.activeEffects = [];
  }

  private updateAnimationFrame(currentTime: number): void {
    if (!this.patternData) return;

    this.currentFrame.time = currentTime;

    // Initialize hole states
    this.patternData.drillPoints.forEach(hole => {
      if (!this.currentFrame.holeStates.has(hole.id)) {
        this.currentFrame.holeStates.set(hole.id, HoleAnimationState.READY);
      }
    });

    // Initialize connection states
    this.connections.forEach(conn => {
      if (!this.currentFrame.connectionStates.has(conn.id)) {
        this.currentFrame.connectionStates.set(conn.id, ConnectionAnimationState.INACTIVE);
      }
    });

    // Process events up to current time
    const activeEvents = this.simulationEvents.filter(event => 
      event.time <= currentTime && event.time > currentTime - 100 // 100ms window
    );

    activeEvents.forEach(event => {
      this.processEvent(event, currentTime);
    });

    // Update active effects
    this.updateActiveEffects(currentTime);
  }

  private processEvent(event: SimulationEvent, currentTime: number): void {
    switch (event.type) {
      case SimulationEventType.SIGNAL_START:
        // Handle both regular signals and hidden point signals
        if (event.targetId.includes('_start')) {
          // Hidden start point (1) activation
          console.log(`Hidden point 1 activated for connection: ${event.data.connectionId}`);
          this.currentFrame.connectionStates.set(event.data.connectionId, ConnectionAnimationState.SIGNAL_PROPAGATING);
        } else {
          // Regular connection signal start
        this.currentFrame.connectionStates.set(event.targetId, ConnectionAnimationState.SIGNAL_PROPAGATING);
        }
        break;

      case SimulationEventType.SIGNAL_ARRIVE:
        // Handle signal arrival at different points
        if (event.targetId.includes('_end')) {
          // Hidden end point (2) reached - mark connection as transmitted
          const connectionId = event.data.connectionId;
          console.log(`Hidden point 2 reached for connection: ${connectionId}, toHole: ${event.data.toHoleId}`);
          this.currentFrame.connectionStates.set(connectionId, ConnectionAnimationState.SIGNAL_TRANSMITTED);
          
          // Trigger visual effect for hidden point 2
          this.createHiddenPointEffect(event.data.connectionId, 'endPoint', currentTime);
          
          // Note: The wave-based logic in generateSimulationEvents now handles triggering next connections
          // No need to manually trigger here as all timing is pre-calculated
        } else {
          // Regular signal arrival at hole
          console.log(`Signal arrived at hole: ${event.targetId} from wire ${event.data.wireSequence}`);
          this.currentFrame.connectionStates.set(event.targetId, ConnectionAnimationState.SIGNAL_TRANSMITTED);
        }
        break;

      case SimulationEventType.HOLE_DETONATE:
        console.log(`Hole detonating: ${event.targetId} triggered by wire ${event.data.triggeredByWire}`);
        this.currentFrame.holeStates.set(event.targetId, HoleAnimationState.DETONATING);
        this.createBlastEffect(event.targetId, currentTime);
        break;

      case SimulationEventType.EFFECT_START:
        // Effects are created by detonation events
        console.log(`Effect started for: ${event.targetId} by wire ${event.data.triggeredByWire}`);
        break;
    }
  }

  // New method to create visual effects for hidden points
  private createHiddenPointEffect(connectionId: string, pointType: 'startPoint' | 'endPoint', startTime: number): void {
    const connection = this.connections.find(c => c.id === connectionId);
    if (!connection) return;

    const point = pointType === 'startPoint' ? connection.startPoint : connection.endPoint;
    const effectId = `hidden_effect_${connectionId}_${pointType}_${startTime}`;

    const effect: BlastEffect = {
      id: effectId,
      type: BlastEffectType.SHOCKWAVE,
      holeId: `${connectionId}_${pointType}`,
      startTime: startTime,
      duration: 300, // Shorter duration for hidden point effects
      intensity: 50, // Lower intensity
      radius: 10, // Smaller radius
      position: { x: point.x, y: point.y }
    };

    this.activeEffects.push(effect);
    this.currentFrame.activeEffects = [...this.activeEffects];
    
    console.log(`Created hidden point effect for ${pointType} of connection ${connectionId}`);
  }

  private createBlastEffect(holeId: string, startTime: number): void {
    if (!this.patternData) return;

    const hole = this.patternData.drillPoints.find(h => h.id === holeId);
    if (!hole) return;

    const effect: BlastEffect = {
      id: `effect_${holeId}_${startTime}`,
      type: BlastEffectType.EXPLOSION,
      holeId: holeId,
      startTime: startTime,
      duration: 1000,
      intensity: this.simulationSettings.effectIntensity,
      radius: 20,
      position: { x: hole.x, y: hole.y }
    };

    this.activeEffects.push(effect);
    this.currentFrame.activeEffects = [...this.activeEffects];
  }

  private updateActiveEffects(currentTime: number): void {
    // Remove expired effects
    this.activeEffects = this.activeEffects.filter(effect => 
      currentTime < effect.startTime + effect.duration
    );

    this.currentFrame.activeEffects = [...this.activeEffects];
  }

  // Rendering
  private renderInitialState(): void {
    // Now using Konva - this method is replaced by renderKonvaPattern
    this.renderKonvaPattern();
    this.renderTimeline();
  }

  private render(): void {
    // Now using Konva - this method is replaced by renderKonvaPattern  
    this.renderKonvaPattern();
    this.renderTimeline();
    this.renderCurrentTimeIndicator();
  }

  private clearCanvas(): void {
    // Konva handles clearing automatically when we destroy children
    if (this.mainLayer) {
      this.mainLayer.destroyChildren();
    }
    if (this.effectsLayer) {
      this.effectsLayer.destroyChildren();
    }
  }

  // Legacy canvas methods - now handled by Konva
  private renderPattern(): void {
    // This is now handled by renderKonvaPattern()
  }

  private renderHole(x: number, y: number, hole: DrillPoint, state: HoleAnimationState): void {
    // This is now handled by renderKonvaPattern()
  }

  private renderHoleLabel(x: number, y: number, label: string): void {
    // This is now handled by renderKonvaPattern()
  }

  private renderConnections(): void {
    // This is now handled by renderKonvaPattern()
  }

  private renderConnection(
    fromPos: { x: number; y: number },
    toPos: { x: number; y: number },
    connection: BlastConnection,
    state: ConnectionAnimationState
  ): void {
    // This is now handled by renderKonvaPattern()
  }

  private drawArrow(from: { x: number; y: number }, to: { x: number; y: number }, color: string): void {
    // This is now handled by renderKonvaPattern()
  }

  private renderConnectionTiming(x: number, y: number, delay: number): void {
    // This is now handled by renderKonvaPattern()
  }

  private renderActiveEffects(): void {
    // This is now handled by renderKonvaPattern()
  }

  private renderBlastEffect(x: number, y: number, effect: BlastEffect): void {
    const elapsed = this.currentFrame.time - effect.startTime;
    const progress = Math.min(elapsed / effect.duration, 1);
    
    switch (effect.type) {
      case BlastEffectType.EXPLOSION:
        this.renderExplosionEffect(x, y, effect, progress);
        break;
      case BlastEffectType.SHOCKWAVE:
        this.renderShockwaveEffect(x, y, effect, progress);
        break;
    }
  }

  private renderExplosionEffect(x: number, y: number, effect: BlastEffect, progress: number): void {
    // This method is now replaced by Konva-based effects
    // Effect rendering is handled in renderKonvaPattern for Konva integration
    console.log('Explosion effect triggered at:', x, y, 'progress:', progress);
  }

  private renderShockwaveEffect(x: number, y: number, effect: BlastEffect, progress: number): void {
    // This method is now replaced by Konva-based effects  
    // Effect rendering is handled in renderKonvaPattern for Konva integration
    console.log('Shockwave effect triggered at:', x, y, 'progress:', progress);
  }

  // Timeline Rendering
  private renderTimeline(): void {
    if (!this.timelineCtx) return;

    // Clear timeline
    this.timelineCtx.clearRect(0, 0, 800, 100);
    
    // Background with grid lines
    this.timelineCtx.fillStyle = '#f8f9fa';
    this.timelineCtx.fillRect(0, 0, 800, 100);

    // Add subtle grid lines for time intervals
    this.drawTimeGrid();

    // Group markers by type for better rendering
    const waveMarkers = this.timelineMarkers.filter(m => m.type === 'hole_blast' && m.label.includes('Wave'));
    const milestoneMarkers = this.timelineMarkers.filter(m => m.type === 'milestone');
    const sequenceMarkers = this.timelineMarkers.filter(m => m.type === 'sequence_start' || m.type === 'sequence_end');
    const detonationMarkers = this.timelineMarkers.filter(m => m.type === 'hole_blast' && m.label.includes('Detonation'));

    // Render in order: grid -> milestones -> waves -> detonations -> sequence markers
    milestoneMarkers.forEach(marker => this.renderTimelineMarker(marker, 'small'));
    waveMarkers.forEach(marker => this.renderTimelineMarker(marker, 'medium'));
    detonationMarkers.forEach(marker => this.renderTimelineMarker(marker, 'medium'));
    sequenceMarkers.forEach(marker => this.renderTimelineMarker(marker, 'large'));
  }

  private drawTimeGrid(): void {
    if (this.simulationState.totalDuration === 0) return;

    this.timelineCtx.strokeStyle = '#e9ecef';
    this.timelineCtx.lineWidth = 1;
    
    // Draw vertical grid lines every 500ms
    const interval = 500; // 500ms intervals
    for (let time = 0; time <= this.simulationState.totalDuration; time += interval) {
      const x = (time / this.simulationState.totalDuration) * 780 + 10;
      this.timelineCtx.beginPath();
      this.timelineCtx.moveTo(x, 85);
      this.timelineCtx.lineTo(x, 90);
      this.timelineCtx.stroke();
      
      // Add time labels at major intervals
      if (time % 1000 === 0) { // Every 1000ms (1s)
        this.timelineCtx.fillStyle = '#6c757d';
        this.timelineCtx.font = '9px Arial';
        this.timelineCtx.textAlign = 'center';
        this.timelineCtx.fillText(`${time/1000}s`, x, 98);
      }
    }
  }

  private renderTimelineMarker(marker: TimelineMarker, size: 'small' | 'medium' | 'large' = 'medium'): void {
    if (this.simulationState.totalDuration === 0) return;

    const x = (marker.time / this.simulationState.totalDuration) * 780 + 10;
    
    // Different sizes and positions for different marker types
    const config = this.getMarkerConfig(size, marker.type);

    // Marker line
    this.timelineCtx.beginPath();
    this.timelineCtx.moveTo(x, config.lineStart);
    this.timelineCtx.lineTo(x, config.lineEnd);
    this.timelineCtx.strokeStyle = marker.color;
    this.timelineCtx.lineWidth = config.lineWidth;
    this.timelineCtx.stroke();

    // Marker dot/shape
    this.timelineCtx.beginPath();
    if (marker.type === 'milestone') {
      // Small diamond for milestones
      const size = config.dotSize;
      this.timelineCtx.moveTo(x, config.dotY - size);
      this.timelineCtx.lineTo(x + size, config.dotY);
      this.timelineCtx.lineTo(x, config.dotY + size);
      this.timelineCtx.lineTo(x - size, config.dotY);
      this.timelineCtx.closePath();
    } else {
      // Circle for other markers
      this.timelineCtx.arc(x, config.dotY, config.dotSize, 0, 2 * Math.PI);
    }
    this.timelineCtx.fillStyle = marker.color;
    this.timelineCtx.fill();

    // Add border for larger markers
    if (size === 'large') {
      this.timelineCtx.strokeStyle = '#fff';
      this.timelineCtx.lineWidth = 2;
      this.timelineCtx.stroke();
    }

    // Label with smart positioning to avoid overlaps
    this.renderMarkerLabel(x, marker.label, marker.type, config);
  }

  private getMarkerConfig(size: 'small' | 'medium' | 'large', type: string) {
    const configs = {
      small: {
        dotSize: 3,
        dotY: 70,
        lineStart: 65,
        lineEnd: 75,
        lineWidth: 1,
        labelY: 85
      },
      medium: {
        dotSize: 4,
        dotY: 50,
        lineStart: 40,
        lineEnd: 60,
        lineWidth: 2,
        labelY: 35
      },
      large: {
        dotSize: 6,
        dotY: 50,
        lineStart: 20,
        lineEnd: 80,
        lineWidth: 3,
        labelY: 15
      }
    };
    
    return configs[size];
  }

  private renderMarkerLabel(x: number, label: string, type: string, config: any): void {
    this.timelineCtx.fillStyle = '#495057';
    
    // Different font sizes for different marker types
    if (type === 'milestone') {
      this.timelineCtx.font = '8px Arial';
    } else if (type === 'sequence_start' || type === 'sequence_end') {
      this.timelineCtx.font = 'bold 10px Arial';
    } else {
      this.timelineCtx.font = '9px Arial';
    }
    
    this.timelineCtx.textAlign = 'center';
    
    // Truncate long labels for better readability
    const maxLength = type === 'milestone' ? 15 : 25;
    const displayLabel = label.length > maxLength ? label.substring(0, maxLength) + '...' : label;
    
    // Add background for better readability on wave markers
    if (type === 'hole_blast' && label.includes('Wave')) {
      const textWidth = this.timelineCtx.measureText(displayLabel).width;
      this.timelineCtx.fillStyle = 'rgba(255, 255, 255, 0.8)';
      this.timelineCtx.fillRect(x - textWidth/2 - 2, config.labelY - 8, textWidth + 4, 10);
      this.timelineCtx.fillStyle = '#495057';
    }
    
    this.timelineCtx.fillText(displayLabel, x, config.labelY);
  }

  private renderCurrentTimeIndicator(): void {
    if (!this.timelineCtx || this.simulationState.totalDuration === 0) return;

    const x = (this.simulationState.currentTime / this.simulationState.totalDuration) * 780 + 10;

    // Current time line
    this.timelineCtx.beginPath();
    this.timelineCtx.moveTo(x, 10);
    this.timelineCtx.lineTo(x, 90);
    this.timelineCtx.strokeStyle = '#dc3545';
    this.timelineCtx.lineWidth = 3;
    this.timelineCtx.stroke();

    // Current time indicator
    this.timelineCtx.beginPath();
    this.timelineCtx.arc(x, 50, 6, 0, 2 * Math.PI);
    this.timelineCtx.fillStyle = '#dc3545';
    this.timelineCtx.fill();
  }

  // Utility Methods
  private worldToScreen(worldX: number, worldY: number): { x: number; y: number } {
    return {
      x: (worldX * this.canvasConfig.scale) + this.canvasConfig.offsetX,
      y: (worldY * this.canvasConfig.scale) + this.canvasConfig.offsetY
    };
  }

  private screenToWorld(screenX: number, screenY: number): { x: number; y: number } {
    return {
      x: (screenX - this.canvasConfig.offsetX) / this.canvasConfig.scale,
      y: (screenY - this.canvasConfig.offsetY) / this.canvasConfig.scale
    };
  }

  // UI Event Handlers
  onViewModeChange(mode: ViewMode): void {
    this.selectedViewMode = mode;
    this.renderKonvaPattern();
  }

  onSettingsChange(settings: Partial<SimulationSettings>): void {
    this.dataService.updateSimulationSettings(settings);
  }

  onTimelineClick(event: MouseEvent): void {
    const canvas = this.timelineCanvasRef.nativeElement;
    const rect = canvas.getBoundingClientRect();
    const x = event.clientX - rect.left;
    
    // Convert to time
    const normalizedX = Math.max(0, Math.min(1, (x - 10) / 780));
    const targetTime = normalizedX * this.simulationState.totalDuration;
    
    this.seekToTime(targetTime);
  }

  toggleFullscreen(): void {
    console.log('Toggle fullscreen clicked!', 'Current:', this.isFullscreen);
    this.isFullscreen = !this.isFullscreen;
    console.log('New fullscreen state:', this.isFullscreen);
  }

  // Navigation
  goToSequenceDesigner(): void {
    console.log('Navigate to sequence designer clicked!');
    this.router.navigate(['/blasting-engineer/blast-sequence-designer']);
  }

  goToPatternCreator(): void {
    console.log('Navigate to pattern creator clicked!');
    this.router.navigate(['/blasting-engineer/drilling-pattern']);
  }

  // Export functionality
  onSaveSimulation(): void {
    if (!this.patternData || this.connections.length === 0) {
      console.warn('No simulation data to save');
      return;
    }

    // Update data service (in memory)
    this.dataService.updateSimulationState(this.simulationState, false);
    this.dataService.updateSimulationSettings(this.simulationSettings, false);
    
    // Explicitly save to storage
    this.dataService.saveSimulationState();
    this.dataService.saveSimulationSettings();

    // Update save state
    this.isSaved = true;

    // Clear save timeout if it exists
    if (this.saveTimeout) {
      clearTimeout(this.saveTimeout);
    }

    // Reset save state after 3 seconds
    this.saveTimeout = setTimeout(() => {
      this.isSaved = false;
    }, 3000);

    console.log('Simulation saved successfully');
  }

  exportVideo(): void {
    // Placeholder for video export functionality
    console.log('Video export functionality would be implemented here');
  }

  exportReport(): void {
    // Placeholder for report export functionality
    console.log('Report export functionality would be implemented here');
  }

  // Validation helpers
  hasValidationErrors(): boolean {
    return this.validation.errors.length > 0;
  }

  hasValidationWarnings(): boolean {
    return this.validation.warnings.length > 0;
  }

  getValidationSummary(): string {
    const errorCount = this.validation.errors.length;
    const warningCount = this.validation.warnings.length;
    
    if (errorCount > 0) {
      return `${errorCount} error${errorCount > 1 ? 's' : ''} found`;
    } else if (warningCount > 0) {
      return `${warningCount} warning${warningCount > 1 ? 's' : ''} found`;
    } else {
      return 'Validation passed';
    }
  }

  private getConnectorTypeName(connectorType: string): string {
    switch (connectorType) {
      case 'Non-Electric-detonation-wire':
        return 'Det Cord';
      case 'Non-Electric-connectors-wire':
        return 'Connector';
      default:
        return 'Unknown';
    }
  }

  private detectTimingConflicts(): Array<{type: string, details: string, affectedHoles: string[], time: number}> {
    const conflicts: Array<{type: string, details: string, affectedHoles: string[], time: number}> = [];
    const holeEvents = new Map<string, Array<{time: number, eventType: string, connectionId: string}>>();
    
    // Group events by target hole
    this.simulationEvents.forEach(event => {
      let targetHole = '';
      let eventType = '';
      let connectionId = '';
      
      if (event.type === SimulationEventType.HOLE_DETONATE) {
        targetHole = event.targetId;
        eventType = 'DETONATION';
        connectionId = event.data?.triggeredByWire || 'unknown';
      } else if (event.type === SimulationEventType.SIGNAL_ARRIVE && !event.targetId.includes('_')) {
        targetHole = event.targetId;
        eventType = 'SIGNAL_ARRIVAL';
        connectionId = event.targetId;
      }
      
      if (targetHole) {
        if (!holeEvents.has(targetHole)) {
          holeEvents.set(targetHole, []);
        }
        holeEvents.get(targetHole)!.push({
          time: event.time,
          eventType: eventType,
          connectionId: connectionId
        });
      }
    });
    
    // Check for conflicts within each hole
    holeEvents.forEach((events, holeId) => {
      // Check for simultaneous detonations (within 50ms)
      const detonationEvents = events.filter(e => e.eventType === 'DETONATION');
      for (let i = 0; i < detonationEvents.length; i++) {
        for (let j = i + 1; j < detonationEvents.length; j++) {
          const timeDiff = Math.abs(detonationEvents[i].time - detonationEvents[j].time);
          if (timeDiff < 50) { // 50ms overlap threshold
            conflicts.push({
              type: 'SIMULTANEOUS_DETONATION',
              details: `Hole ${holeId} has overlapping detonations at ${detonationEvents[i].time}ms and ${detonationEvents[j].time}ms (${timeDiff}ms apart)`,
              affectedHoles: [holeId],
              time: Math.min(detonationEvents[i].time, detonationEvents[j].time)
            });
          }
        }
      }
      
      // Check for rapid signal arrivals (within 25ms)
      const signalEvents = events.filter(e => e.eventType === 'SIGNAL_ARRIVAL');
      for (let i = 0; i < signalEvents.length; i++) {
        for (let j = i + 1; j < signalEvents.length; j++) {
          const timeDiff = Math.abs(signalEvents[i].time - signalEvents[j].time);
          if (timeDiff < 25) { // 25ms overlap threshold for signals
            conflicts.push({
              type: 'SIGNAL_INTERFERENCE',
              details: `Hole ${holeId} receives overlapping signals at ${signalEvents[i].time}ms and ${signalEvents[j].time}ms (${timeDiff}ms apart)`,
              affectedHoles: [holeId],
              time: Math.min(signalEvents[i].time, signalEvents[j].time)
            });
          }
        }
      }
    });
    
    return conflicts;
  }

  private updateValidationWithConflicts(conflicts: Array<{type: string, details: string, affectedHoles: string[], time: number}>): void {
    // Clear previous timing-related validation issues
    this.validation.warnings = this.validation.warnings.filter(w => w.type !== 'timing_overlap');
    this.validation.errors = this.validation.errors.filter(e => e.type !== 'timing_conflict');
    
    conflicts.forEach(conflict => {
      if (conflict.type === 'SIMULTANEOUS_DETONATION') {
        // Simultaneous detonations are critical errors
        this.validation.errors.push({
          type: 'timing_conflict',
          message: conflict.details,
          affectedElements: conflict.affectedHoles,
          fixSuggestion: 'Adjust delay times to ensure minimum 50ms separation between detonations'
        });
        this.validation.isValid = false;
      } else if (conflict.type === 'SIGNAL_INTERFERENCE') {
        // Signal interference is a warning
        this.validation.warnings.push({
          type: 'timing_overlap',
          message: conflict.details,
          affectedHoles: conflict.affectedHoles,
          severity: 'medium'
        });
      }
    });
    
    // Update validation status
    if (this.validation.errors.length === 0 && this.validation.warnings.length === 0) {
      this.validation.isValid = true;
    }
    
    // Note: Validation is updated locally and will be displayed in the UI
    // The data service will be notified through the normal validation flow
  }

  // View Control Methods
  onViewSettingsChange(settings: Partial<ViewSettings>): void {
    this.viewSettings = { ...this.viewSettings, ...settings };
    this.renderKonvaPattern();
    
    // Apply theme changes
    if (settings.colorTheme) {
      this.applyColorTheme(settings.colorTheme);
    }
  }

  // Zoom Controls
  zoomIn(): void {
    this.canvasConfig.scale = Math.min(this.canvasConfig.scale * 1.2, 5);
    this.stage.scale({ x: this.canvasConfig.scale, y: this.canvasConfig.scale });
    this.stage.draw();
  }

  zoomOut(): void {
    this.canvasConfig.scale = Math.max(this.canvasConfig.scale * 0.8, 0.1);
    this.stage.scale({ x: this.canvasConfig.scale, y: this.canvasConfig.scale });
    this.stage.draw();
  }

  // Camera/Pan Controls
  panUp(): void {
    this.canvasConfig.offsetY += 50;
    this.stage.position({ x: this.canvasConfig.offsetX, y: this.canvasConfig.offsetY });
    this.stage.draw();
  }

  panDown(): void {
    this.canvasConfig.offsetY -= 50;
    this.stage.position({ x: this.canvasConfig.offsetX, y: this.canvasConfig.offsetY });
    this.stage.draw();
  }

  panLeft(): void {
    this.canvasConfig.offsetX += 50;
    this.stage.position({ x: this.canvasConfig.offsetX, y: this.canvasConfig.offsetY });
    this.stage.draw();
  }

  panRight(): void {
    this.canvasConfig.offsetX -= 50;
    this.stage.position({ x: this.canvasConfig.offsetX, y: this.canvasConfig.offsetY });
    this.stage.draw();
  }

  centerView(): void {
    if (!this.patternData) return;
    
    // Calculate center of all holes
    const holes = this.patternData.drillPoints;
    if (holes.length === 0) return;
    
    const bounds = holes.reduce(
      (acc, hole) => ({
        minX: Math.min(acc.minX, hole.x),
        maxX: Math.max(acc.maxX, hole.x),
        minY: Math.min(acc.minY, hole.y),
        maxY: Math.max(acc.maxY, hole.y)
      }),
      { minX: Infinity, maxX: -Infinity, minY: Infinity, maxY: -Infinity }
    );
    
    const centerX = (bounds.minX + bounds.maxX) / 2;
    const centerY = (bounds.minY + bounds.maxY) / 2;
    
    // Center the view on the pattern
    this.canvasConfig.offsetX = this.canvasConfig.width / 2 - centerX * this.canvasConfig.scale;
    this.canvasConfig.offsetY = this.canvasConfig.height / 2 - centerY * this.canvasConfig.scale;
    
    this.stage.position({ x: this.canvasConfig.offsetX, y: this.canvasConfig.offsetY });
    this.stage.draw();
  }

  cycleColorTheme(): void {
    const themes: Array<'default' | 'dark' | 'high-contrast' | 'colorblind'> = ['default', 'dark', 'high-contrast', 'colorblind'];
    const currentIndex = themes.indexOf(this.viewSettings.colorTheme);
    const nextIndex = (currentIndex + 1) % themes.length;
    const nextTheme = themes[nextIndex];
    
    this.onViewSettingsChange({ colorTheme: nextTheme });
  }

  private applyColorTheme(theme: string): void {
    const themes = {
      default: {
        background: '#f5f5f5',
        grid: '#e0e0e0',
        hole: '#2196f3',
        connection: '#4caf50'
      },
      dark: {
        background: '#1a1a1a',
        grid: '#333333',
        hole: '#64b5f6',
        connection: '#81c784'
      },
      'high-contrast': {
        background: '#ffffff',
        grid: '#000000',
        hole: '#ff0000',
        connection: '#0000ff'
      },
      colorblind: {
        background: '#f5f5f5',
        grid: '#cccccc',
        hole: '#ff8800',
        connection: '#0088ff'
      }
    };
    
    const selectedTheme = themes[theme as keyof typeof themes] || themes.default;
    
    // Apply theme to canvas background
    if (this.stage) {
      // This would update the stage background and re-render
      this.renderKonvaPattern();
    }
  }
} 