import { Component, Input, ViewChild, ElementRef, OnChanges, SimpleChanges, AfterViewInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import Konva from 'konva';
import { PatternData, DrillPoint, BlastConnection } from '../../../drilling-pattern-creator/models/drill-point.model';
import { SimulationSettings, ViewSettings, HoleAnimationState, ConnectionAnimationState } from '../../../shared/models/simulation.model';

@Component({
  selector: 'app-pattern-renderer',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div #patternContainer class="pattern-container"></div>
  `,
  styles: [`
    .pattern-container {
      width: 100%;
      height: 100%;
      min-height: 500px;
      background: #f8f9fa;
    }
  `]
})
export class PatternRendererComponent implements OnChanges, AfterViewInit, OnDestroy {
  @ViewChild('patternContainer') containerRef!: ElementRef<HTMLDivElement>;
  @Input() patternData: PatternData | null = null;
  @Input() connections: BlastConnection[] = [];
  @Input() simulationSettings!: SimulationSettings;
  @Input() viewSettings!: ViewSettings;
  @Input() holeStates: Map<string, HoleAnimationState> = new Map();
  @Input() connectionStates: Map<string, ConnectionAnimationState> = new Map();

  // Expose stage for video capture
  public stage!: Konva.Stage;
  private mainLayer!: Konva.Layer;
  private effectsLayer!: Konva.Layer;
  private gridLayer!: Konva.Layer;
  private holeShapes: Map<string, Konva.Circle> = new Map();
  private connectionShapes: Map<string, Konva.Group> = new Map();
  private labelShapes: Map<string, Konva.Text> = new Map();
  private isDragging = false;
  private lastPointerPosition = { x: 0, y: 0 };

  private canvasConfig = {
    width: 800,
    height: 600,
    padding: 50,
    scale: 1,
    offsetX: 0,
    offsetY: 0
  };

  ngOnChanges(changes: SimpleChanges): void {
    if (this.containerRef) {
      if (!this.stage) {
        this.initializeCanvas();
      }
      this.render();
    }
  }

  ngAfterViewInit(): void {
    if (!this.stage) {
      this.initializeCanvas();
    }

    // Add resize listener for responsive canvas
    window.addEventListener('resize', this.handleResize.bind(this));
  }

  ngOnDestroy(): void {
    // Clean up resize listener
    window.removeEventListener('resize', this.handleResize.bind(this));

    // Clean up Konva stage
    if (this.stage) {
      this.stage.destroy();
    }
  }

  private handleResize(): void {
    if (!this.stage || !this.containerRef) return;

    const container = this.containerRef.nativeElement;
    const newWidth = container.clientWidth;
    const newHeight = container.clientHeight;

    if (newWidth > 0 && newHeight > 0) {
      this.stage.width(newWidth);
      this.stage.height(newHeight);
      this.canvasConfig.width = newWidth;
      this.canvasConfig.height = newHeight;

      // Re-render grid with new dimensions
      this.gridLayer.destroyChildren();
      this.renderGrid();

      // Redraw all layers
      this.stage.getLayers().forEach(layer => layer.batchDraw());
    }
  }

  private initializeCanvas(): void {
    const container = this.containerRef.nativeElement;

    // Get actual container dimensions
    const containerWidth = container.clientWidth || 800;
    const containerHeight = container.clientHeight || 600;

    // Update canvasConfig with actual dimensions
    this.canvasConfig.width = containerWidth;
    this.canvasConfig.height = containerHeight;

    this.stage = new Konva.Stage({
      container: container,
      width: containerWidth,
      height: containerHeight
    });

    // Create separate layers for different types of content
    this.gridLayer = new Konva.Layer();
    this.mainLayer = new Konva.Layer();
    this.effectsLayer = new Konva.Layer();
    
    this.stage.add(this.gridLayer);
    this.stage.add(this.mainLayer);
    this.stage.add(this.effectsLayer);

    // Enable hardware acceleration
    this.stage.getLayers().forEach(layer => {
      layer.getCanvas()._canvas.style.transform = 'translateZ(0)';
    });

    this.setupKonvaInteractions();
    this.renderGrid(); // Render grid once
  }

  private setupKonvaInteractions(): void {
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
      const clampedScale = Math.max(0.1, Math.min(5, newScale));
      
      this.stage.scale({ x: clampedScale, y: clampedScale });
      
      const newPos = {
        x: pointer.x - mousePointTo.x * clampedScale,
        y: pointer.y - mousePointTo.y * clampedScale,
      };
      
      this.stage.position(newPos);
      this.gridLayer.batchDraw();
      this.mainLayer.batchDraw();
    });

    // Optimize drag handling
    this.stage.on('dragstart', () => {
      this.isDragging = true;
      this.lastPointerPosition = this.stage.getPointerPosition() || { x: 0, y: 0 };
    });

    this.stage.on('dragmove', () => {
      if (!this.isDragging) return;
      
      const currentPos = this.stage.getPointerPosition() || { x: 0, y: 0 };
      const dx = currentPos.x - this.lastPointerPosition.x;
      const dy = currentPos.y - this.lastPointerPosition.y;
      
      this.stage.position({
        x: this.stage.x() + dx,
        y: this.stage.y() + dy
      });
      
      this.lastPointerPosition = currentPos;
      
      // Only redraw grid and main layer during drag
      this.gridLayer.batchDraw();
      this.mainLayer.batchDraw();
    });

    this.stage.on('dragend', () => {
      this.isDragging = false;
    });

    this.stage.draggable(true);
  }

  private render(): void {
    if (!this.patternData || !this.mainLayer) return;

    // Toggle grid visibility according to the current view settings
    if (this.gridLayer) {
      this.gridLayer.visible(this.viewSettings.showGrid);
      this.gridLayer.batchDraw();
    }

    // Clear previous content and caches
    this.mainLayer.destroyChildren();
    this.holeShapes.clear();
    this.connectionShapes.clear();
    this.labelShapes.clear();

    // Render holes and connections
    this.patternData.drillPoints.forEach(hole => {
      this.renderDrillHole(hole);
    });

    if (this.simulationSettings.showConnections) {
      this.connections.forEach(conn => {
        this.renderConnection(conn);
      });
    }

    // Cache the main layer for better performance
    this.mainLayer.cache();
    this.mainLayer.batchDraw();
  }

  private renderGrid(): void {
    const gridSize = 50;
    const stageWidth = this.stage.width();
    const stageHeight = this.stage.height();
    
    const gridGroup = new Konva.Group({
      opacity: 0.3
    });

    // Optimize grid rendering by using a single path for all lines
    const points: number[] = [];
    
    // Vertical lines
    for (let x = 0; x <= stageWidth; x += gridSize) {
      points.push(x, 0, x, stageHeight);
    }
    
    // Horizontal lines
    for (let y = 0; y <= stageHeight; y += gridSize) {
      points.push(0, y, stageWidth, y);
    }

    const gridLines = new Konva.Line({
      points: points,
      stroke: this.getGridColorByTheme(),
      strokeWidth: 0.5,
      listening: false // Disable event listening for grid lines
    });

    gridGroup.add(gridLines);
    this.gridLayer.add(gridGroup);
    this.gridLayer.cache(); // Cache the grid layer
    this.gridLayer.batchDraw();
  }

  private renderDrillHole(hole: DrillPoint): void {
    const visualX = hole.x * 60;
    const visualY = hole.y * 60;
    
    const group = new Konva.Group({
      x: visualX,
      y: visualY,
      listening: false // Disable event listening for better performance
    });

    const state = this.holeStates.get(hole.id) || HoleAnimationState.READY;
    
    const depthRing = new Konva.Circle({
      radius: 12,
      stroke: this.getHoleColor(state).ring,
      strokeWidth: 2,
      opacity: 0.5,
      listening: false
    });

    const mainCircle = new Konva.Circle({
      radius: 8,
      fill: this.getHoleColor(state).fill,
      stroke: this.getHoleColor(state).stroke,
      strokeWidth: 2,
      listening: false
    });

    group.add(depthRing);
    group.add(mainCircle);

    if (this.simulationSettings.showSequenceNumbers) {
      const labelBg = new Konva.Rect({
        x: -15,
        y: -30,
        width: 30,
        height: 16,
        fill: 'rgba(255,255,255,0.9)',
        stroke: '#ddd',
        strokeWidth: 1,
        cornerRadius: 3,
        listening: false
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
        listening: false
      });

      group.add(labelBg);
      group.add(label);
      this.labelShapes.set(hole.id, label);
    }

    if (this.viewSettings.showHoleDetails) {
      const specsText = new Konva.Text({
        x: -20,
        y: 15,
        text: `D:${hole.depth || 10}m\nS:${hole.spacing || 3}m\nB:${hole.burden || 2.5}m`,
        fontSize: 8,
        fontFamily: 'Arial, sans-serif',
        fill: '#666',
        align: 'center',
        width: 40,
        listening: false
      });
      group.add(specsText);
    }

    this.mainLayer.add(group);
    this.holeShapes.set(hole.id, mainCircle);

    if (state === HoleAnimationState.DETONATING) {
      mainCircle.to({ scaleX: 1.5, scaleY: 1.5, duration: 0.2, easing: Konva.Easings.EaseInOut });
    } else {
      mainCircle.to({ scaleX: 1, scaleY: 1, duration: 0.2, easing: Konva.Easings.EaseInOut });
    }
  }

  private renderConnection(conn: BlastConnection): void {
    const fromHole = this.patternData!.drillPoints.find(h => h.id === conn.point1DrillPointId);
    const toHole = this.patternData!.drillPoints.find(h => h.id === conn.point2DrillPointId);

    if (!fromHole || !toHole) return;

    const fromX = fromHole.x * 60;
    const fromY = fromHole.y * 60;
    const toX = toHole.x * 60;
    const toY = toHole.y * 60;

    const connectionGroup = new Konva.Group({
      listening: false // Disable event listening for better performance
    });

    const state = this.connectionStates.get(conn.id) || ConnectionAnimationState.INACTIVE;
    const isActive = state === ConnectionAnimationState.SIGNAL_PROPAGATING || 
                    state === ConnectionAnimationState.SIGNAL_TRANSMITTED;
    const shouldHighlight = this.viewSettings.highlightActiveConnections && isActive;

    const line = new Konva.Line({
      points: [fromX, fromY, toX, toY],
      stroke: shouldHighlight ? '#ff9800' : this.getConnectionColor(state),
      strokeWidth: shouldHighlight ? this.getConnectionWidth(state) + 2 : this.getConnectionWidth(state),
      dash: state === ConnectionAnimationState.SIGNAL_PROPAGATING ? [5, 5] : [],
      listening: false
    });

    const arrowPoints = this.calculateArrowPoints(
      { x: fromX, y: fromY }, 
      { x: toX, y: toY }
    );
    const arrow = new Konva.Line({
      points: arrowPoints,
      stroke: this.getConnectionColor(state),
      strokeWidth: 2,
      fill: this.getConnectionColor(state),
      closed: false,
      listening: false
    });

    connectionGroup.add(line);
    connectionGroup.add(arrow);

    if (this.simulationSettings.showTiming) {
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
        listening: false
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
        listening: false
      });

      connectionGroup.add(timingBg);
      connectionGroup.add(timingLabel);
    }

    this.mainLayer.add(connectionGroup);
    this.connectionShapes.set(conn.id, connectionGroup);
  }

  private getHoleColor(state: HoleAnimationState): { fill: string; stroke: string; ring: string; hover: string } {
    const theme = this.viewSettings.colorTheme;
    
    if (theme === 'high-contrast') {
      switch (state) {
        case HoleAnimationState.READY: return { fill: '#0000FF', stroke: '#000080', ring: '#E0E0FF', hover: '#4040FF' };
        case HoleAnimationState.DETONATING: return { fill: '#FF0000', stroke: '#CC0000', ring: '#FFE0E0', hover: '#FF4040' };
        case HoleAnimationState.BLASTED: return { fill: '#00FF00', stroke: '#00CC00', ring: '#E0FFE0', hover: '#40FF40' };
        default: return { fill: '#808080', stroke: '#404040', ring: '#F0F0F0', hover: '#A0A0A0' };
      }
    } else if (theme === 'colorblind') {
      switch (state) {
        case HoleAnimationState.READY: return { fill: '#0088FF', stroke: '#0066CC', ring: '#CCE8FF', hover: '#4499FF' };
        case HoleAnimationState.DETONATING: return { fill: '#FF8800', stroke: '#CC6600', ring: '#FFE4CC', hover: '#FF9944' };
        case HoleAnimationState.BLASTED: return { fill: '#8800FF', stroke: '#6600CC', ring: '#E4CCFF', hover: '#9944FF' };
        default: return { fill: '#808080', stroke: '#606060', ring: '#F0F0F0', hover: '#A0A0A0' };
      }
    } else if (theme === 'dark') {
      switch (state) {
        case HoleAnimationState.READY: return { fill: '#64B5F6', stroke: '#1976D2', ring: '#2196F3', hover: '#90CAF9' };
        case HoleAnimationState.DETONATING: return { fill: '#FF8A65', stroke: '#F57C00', ring: '#FF9800', hover: '#FFAB91' };
        case HoleAnimationState.BLASTED: return { fill: '#81C784', stroke: '#388E3C', ring: '#4CAF50', hover: '#A5D6A7' };
        default: return { fill: '#BDBDBD', stroke: '#757575', ring: '#9E9E9E', hover: '#E0E0E0' };
      }
    } else {
      switch (state) {
        case HoleAnimationState.READY:
          return {
            fill: '#e3f2fd',
            stroke: '#1976d2',
            ring: '#bbdefb',
            hover: '#bbdefb'
          };
        case HoleAnimationState.DETONATING:
          return {
            fill: '#fff3e0',
            stroke: '#f57f17',
            ring: '#ffcc02',
            hover: '#ffe082'
          };
        case HoleAnimationState.BLASTED:
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

  private getConnectionColor(state: ConnectionAnimationState): string {
    switch (state) {
      case ConnectionAnimationState.INACTIVE:
        return '#dee2e6';
      case ConnectionAnimationState.SIGNAL_PROPAGATING:
        return '#ff9800';
      case ConnectionAnimationState.SIGNAL_TRANSMITTED:
        return '#4caf50';
      default:
        return '#dee2e6';
    }
  }

  private getConnectionWidth(state: ConnectionAnimationState): number {
    switch (state) {
      case ConnectionAnimationState.INACTIVE:
        return 2;
      case ConnectionAnimationState.SIGNAL_PROPAGATING:
        return 4;
      case ConnectionAnimationState.SIGNAL_TRANSMITTED:
        return 3;
      default:
        return 2;
    }
  }

  private calculateArrowPoints(from: { x: number; y: number }, to: { x: number; y: number }): number[] {
    const angle = Math.atan2(to.y - from.y, to.x - from.x);
    const arrowLength = 10;
    const arrowAngle = Math.PI / 6;

    const x1 = to.x - arrowLength * Math.cos(angle - arrowAngle);
    const y1 = to.y - arrowLength * Math.sin(angle - arrowAngle);
    const x2 = to.x - arrowLength * Math.cos(angle + arrowAngle);
    const y2 = to.y - arrowLength * Math.sin(angle + arrowAngle);

    return [to.x, to.y, x1, y1, to.x, to.y, x2, y2];
  }

  private getGridColorByTheme(): string {
    switch (this.viewSettings.colorTheme) {
      case 'dark': return '#333333';
      case 'high-contrast': return '#000000';
      case 'colorblind': return '#cccccc';
      default: return '#e0e0e0';
    }
  }
} 