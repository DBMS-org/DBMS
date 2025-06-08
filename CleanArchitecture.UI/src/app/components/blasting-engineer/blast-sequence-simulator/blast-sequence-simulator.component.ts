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
  private canvasConfig = {
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
    this.initializeSubscriptions();
    this.loadSimulationData();
    this.dataService.setCurrentWorkflowStep('simulate');
    console.log('Blast Simulator - ngOnInit completed');
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

    // First render a background grid for better visualization
    this.renderBackgroundGrid();

    // Render drill holes with enhanced styling
    this.patternData.drillPoints.forEach(hole => {
      this.renderAdvancedDrillHole(hole);
    });

    // Render connections with enhanced styling
    this.connections.forEach(conn => {
      this.renderAdvancedConnection(conn);
    });

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
    
    // Create grid lines
    const gridGroup = new Konva.Group({
      opacity: 0.3
    });

    // Vertical lines
    for (let x = 0; x <= stageWidth; x += gridSize) {
      const line = new Konva.Line({
        points: [x, 0, x, stageHeight],
        stroke: '#e0e0e0',
        strokeWidth: 0.5
      });
      gridGroup.add(line);
    }

    // Horizontal lines
    for (let y = 0; y <= stageHeight; y += gridSize) {
      const line = new Konva.Line({
        points: [0, y, stageWidth, y],
        stroke: '#e0e0e0',
        strokeWidth: 0.5
      });
      gridGroup.add(line);
    }

    this.mainLayer.add(gridGroup);
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

    // Hole ID label with background
    const labelBg = new Konva.Rect({
      x: -15,
      y: -30,
      width: 30,
      height: 16,
      fill: 'rgba(255,255,255,0.9)',
      stroke: '#ddd',
      strokeWidth: 1,
      cornerRadius: 3
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
      width: 30
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
      visible: false // Initially hidden, show on hover
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
      specsText.visible(true);
      document.body.style.cursor = 'pointer';
      this.mainLayer.batchDraw();
    });

    group.on('mouseleave', () => {
      mainCircle.fill(this.getHoleColor(state).fill);
      specsText.visible(false);
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

    // Main connection line with enhanced styling
    const line = new Konva.Line({
      points: [fromX, fromY, toX, toY],
      stroke: this.getConnectionColor(state),
      strokeWidth: this.getConnectionWidth(state),
      dash: state === 'SIGNAL_PROPAGATING' ? [5, 5] : [],
      shadowColor: 'rgba(0,0,0,0.2)',
      shadowBlur: 2,
      shadowOffset: { x: 1, y: 1 }
    });

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

    // Timing label with background
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
      cornerRadius: 3
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
      width: 40
    });

    connectionGroup.add(line);
    connectionGroup.add(arrow);
    connectionGroup.add(timingBg);
    connectionGroup.add(timingLabel);

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
    
    // Create events for each connection
    this.connections.forEach(connection => {
      // Signal start event
      this.simulationEvents.push({
        time: connection.delay,
        type: SimulationEventType.SIGNAL_START,
        targetId: connection.id,
        data: { fromHoleId: connection.fromHoleId, toHoleId: connection.toHoleId }
      });

      // Detonation event
      this.simulationEvents.push({
        time: connection.delay + 50, // Small delay for signal propagation
        type: SimulationEventType.HOLE_DETONATE,
        targetId: connection.toHoleId,
        data: { delay: connection.delay, type: connection.connectorType }
      });

      // Effect events
      this.simulationEvents.push({
        time: connection.delay + 50,
        type: SimulationEventType.EFFECT_START,
        targetId: connection.toHoleId,
        data: { effectType: BlastEffectType.EXPLOSION, duration: 1000 }
      });
    });

    // Sort events by time
    this.simulationEvents.sort((a, b) => a.time - b.time);
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
        this.currentFrame.connectionStates.set(event.targetId, ConnectionAnimationState.SIGNAL_PROPAGATING);
        break;

      case SimulationEventType.HOLE_DETONATE:
        this.currentFrame.holeStates.set(event.targetId, HoleAnimationState.DETONATING);
        this.createBlastEffect(event.targetId, currentTime);
        break;

      case SimulationEventType.EFFECT_START:
        // Effects are created by detonation events
        break;
    }
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
    
    // Background
    this.timelineCtx.fillStyle = '#f8f9fa';
    this.timelineCtx.fillRect(0, 0, 800, 100);

    // Render timeline markers
    this.timelineMarkers.forEach(marker => {
      this.renderTimelineMarker(marker);
    });
  }

  private renderTimelineMarker(marker: TimelineMarker): void {
    if (this.simulationState.totalDuration === 0) return;

    const x = (marker.time / this.simulationState.totalDuration) * 780 + 10;
    const y = 50;

    // Marker line
    this.timelineCtx.beginPath();
    this.timelineCtx.moveTo(x, 20);
    this.timelineCtx.lineTo(x, 80);
    this.timelineCtx.strokeStyle = marker.color;
    this.timelineCtx.lineWidth = 2;
    this.timelineCtx.stroke();

    // Marker dot
    this.timelineCtx.beginPath();
    this.timelineCtx.arc(x, y, 4, 0, 2 * Math.PI);
    this.timelineCtx.fillStyle = marker.color;
    this.timelineCtx.fill();

    // Label
    this.timelineCtx.fillStyle = '#495057';
    this.timelineCtx.font = '10px Arial';
    this.timelineCtx.textAlign = 'center';
    this.timelineCtx.fillText(marker.label, x, y + 20);
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
} 