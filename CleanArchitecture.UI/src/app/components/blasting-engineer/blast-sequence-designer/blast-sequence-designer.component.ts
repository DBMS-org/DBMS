import { Component, ElementRef, ViewChild, AfterViewInit, ChangeDetectorRef, ChangeDetectionStrategy, HostListener, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import Konva from 'konva';
import { PatternDataService } from '../shared/pattern-data.service';
import { BlastSequenceDataService } from '../shared/services/blast-sequence-data.service';
import { 
  PatternData, 
  DrillPoint, 
  BlastConnection, 
  DetonatorInfo, 
  ConnectorType, 
  DetonatorType, 
  BlastSequenceData 
} from '../drilling-pattern-creator/models/drill-point.model';

@Component({
  selector: 'app-blast-sequence-designer',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './blast-sequence-designer.component.html',
  styleUrls: ['./blast-sequence-designer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BlastSequenceDesignerComponent implements AfterViewInit, OnDestroy {
  @ViewChild('container') containerRef!: ElementRef<HTMLDivElement>;

  private stage!: Konva.Stage;
  private gridLayer!: Konva.Layer;
  private pointsLayer!: Konva.Layer;
  private connectionsLayer!: Konva.Layer;
  
  patternData: PatternData | null = null;
  connections: BlastConnection[] = [];
  detonators: DetonatorInfo[] = [];
  
  // UI State
  isConnectionMode = false;
  showConnections = true;
  showConnectionsPanel = true;
  showSearch = false;
  searchTerm = '';
  sortBy: 'sequence' | 'type' | 'delay' = 'sequence';
  sortDirection: 'asc' | 'desc' = 'asc';
  filteredConnections: BlastConnection[] = [];
  selectedFromHole: DrillPoint | null = null;
  selectedToHole: DrillPoint | null = null;
  selectedConnectorType: ConnectorType = ConnectorType.DETONATING_CORD;
  currentDelay: number | null = null;
  currentSequence = 1;
  showHelp = false;
  showHiddenPoints = false;

  // Predefined delay options for non-electric systems
  detonatingCordDelays = [17, 25, 42]; // milliseconds - no 67ms for detonating cord
  connectorsDelays = [17, 25, 42, 67]; // milliseconds - includes 67ms for connectors
  
  // Visual elements
  private drillPointObjects: Map<string, Konva.Group> = new Map();
  private connectionObjects: Map<string, Konva.Group> = new Map();
  private temporaryLine: Konva.Line | null = null;
  
  // Zoom and Pan properties
  private scale = 1;
  private panX = 0;
  private panY = 0;
  private isPanning = false;
  private lastPanPoint = { x: 0, y: 0 };
  private minScale = 0.1;
  private maxScale = 5;
  
  // Enums for template
  ConnectorType = ConnectorType;
  DetonatorType = DetonatorType;
  
  private resizeTimeout: any;
  private isInitialized = false;

  // Save functionality
  public isSaved = false;
  private saveTimeout: any;

  constructor(
    private cdr: ChangeDetectorRef,
    private patternDataService: PatternDataService,
    private blastSequenceDataService: BlastSequenceDataService,
    private router: Router
  ) {}

  ngAfterViewInit(): void {
    // Initialize site context from route parameters
    this.initializeSiteContext();
    
    this.loadPatternData();
    this.initializeCanvas();
    
    // Set current workflow step to sequence
    this.blastSequenceDataService.setCurrentWorkflowStep('sequence');
  }

  private initializeSiteContext(): void {
    // Get projectId and siteId from route
    const projectId = +(this.router.url.match(/project-management\/(\d+)\/sites\/(\d+)/) || [])[1];
    const siteId = +(this.router.url.match(/project-management\/(\d+)\/sites\/(\d+)/) || [])[2];
    
    if (projectId && siteId) {
      console.log('Setting site context:', { projectId, siteId });
      this.blastSequenceDataService.setSiteContext(projectId, siteId);
    } else {
      console.warn('Could not extract site context from route:', this.router.url);
    }
  }

  ngOnDestroy(): void {
    if (this.resizeTimeout) {
      clearTimeout(this.resizeTimeout);
    }
    this.cleanup();
  }

  private loadPatternData(): void {
    // Try to get pattern data from shared service first, then fallback to old service
    this.patternData = this.blastSequenceDataService.getPatternData() || 
                     this.patternDataService.getCurrentPatternValue();
    
    if (!this.patternData) {
      console.warn('No pattern data available. Redirecting to pattern creator.');
      // Optionally redirect back to pattern creator
      // this.router.navigate(['/blasting-engineer/drilling-pattern-creator']);
    }
    
    // Load existing connections if any and ensure they have hidden points
    const existingConnections = this.blastSequenceDataService.getConnections();
    if (existingConnections.length > 0) {
      // Ensure all connections have proper hidden points structure
      const migratedConnections = existingConnections.map(conn => 
        this.ensureConnectionHasHiddenPoints(conn)
      );
      
      // Update the service with migrated connections
      this.blastSequenceDataService.setConnections(migratedConnections);
      this.connections = migratedConnections;
      
      console.log('Loaded and migrated connections with hidden points:', this.connections);
    } else {
      this.connections = [];
    }
    
    this.updateFilteredConnections();
    
    this.cdr.detectChanges();
  }

  private initializeCanvas(): void {
    if (this.isInitialized || !this.containerRef) {
      return;
    }

    const container = this.containerRef.nativeElement;
    
    // Ensure container has proper dimensions
    if (container.offsetWidth === 0 || container.offsetHeight === 0) {
      setTimeout(() => this.initializeCanvas(), 100);
      return;
    }
    
    // Calculate optimal canvas size based on container and content
    const canvasWidth = Math.max(container.offsetWidth, 400);
    const canvasHeight = Math.max(container.offsetHeight, 300);
    
    this.stage = new Konva.Stage({
      container: container,
      width: canvasWidth,
      height: canvasHeight
    });

    this.gridLayer = new Konva.Layer();
    this.pointsLayer = new Konva.Layer();
    this.connectionsLayer = new Konva.Layer();

    this.stage.add(this.gridLayer);
    this.stage.add(this.connectionsLayer);
    this.stage.add(this.pointsLayer);

    this.setupEventListeners();
    this.calculateZoomLimits();
    this.ensureCanvasVisibility();
    this.drawGrid();
    this.drawDrillPoints();
    
    // Draw existing connections if any
    if (this.connections.length > 0) {
      this.redrawConnections();
    }
    
    // Initialize zoom and center view
    this.applyTransform();
    setTimeout(() => {
      this.centerView();
    }, 100);
    
    this.isInitialized = true;
  }

  private ensureCanvasVisibility(): void {
    // Ensure canvas is visible regardless of mode
    const canvas = this.stage.container().querySelector('canvas');
    if (canvas) {
      canvas.style.display = 'block';
      canvas.style.visibility = 'visible';
      canvas.style.opacity = '1';
    }
    
    // Make sure stage is properly sized
    this.stage.visible(true);
    this.gridLayer.visible(true);
    this.pointsLayer.visible(true);
    this.connectionsLayer.visible(this.showConnections);
  }

  private resizeCanvas(): void {
    if (!this.stage || !this.containerRef) return;
    
    const container = this.containerRef.nativeElement;
    const newWidth = container.offsetWidth;
    const newHeight = container.offsetHeight;
    
    // Ensure minimum dimensions - prioritize 2D canvas usability
    const minWidth = Math.max(newWidth, 400);
    const minHeight = Math.max(newHeight, this.isConnectionMode ? 350 : 300);
    
    // Only resize if dimensions have actually changed
    if (this.stage.width() !== minWidth || this.stage.height() !== minHeight) {
      this.stage.width(minWidth);
      this.stage.height(minHeight);
      
      // Recalculate zoom limits based on new canvas size
      this.calculateZoomLimits();
      
      // Maintain center position but adjust for new canvas size
      this.maintainViewportCenter();
      
      // Redraw everything with new dimensions
      this.applyTransform();
    }
  }

  private maintainViewportCenter(): void {
    if (!this.patternData || this.patternData.drillPoints.length === 0) return;
    
    // Calculate the center of the pattern
    const points = this.patternData.drillPoints;
    let minX = points[0].x, maxX = points[0].x;
    let minY = points[0].y, maxY = points[0].y;

    points.forEach(point => {
      minX = Math.min(minX, point.x);
      maxX = Math.max(maxX, point.x);
      minY = Math.min(minY, point.y);
      maxY = Math.max(maxY, point.y);
    });

    const topLeft = this.convertToCanvasCoords(minX, minY);
    const bottomRight = this.convertToCanvasCoords(maxX, maxY);
    const centerX = (topLeft.x + bottomRight.x) / 2;
    const centerY = (topLeft.y + bottomRight.y) / 2;

    // Adjust pan to keep pattern centered in new canvas size
    const stageWidth = this.stage.width();
    const stageHeight = this.stage.height();
    
    this.panX = (stageWidth / 2) - centerX * this.scale;
    this.panY = (stageHeight / 2) - centerY * this.scale;
  }

  private setupEventListeners(): void {
    // Left click events
    this.stage.on('click', (e) => {
      if (this.isPanning) return; // Don't handle clicks during panning
      
      if (this.isConnectionMode) {
        this.handleConnectionClick(e);
      } else {
        this.handleSelectionClick(e);
      }
    });

    // Mouse move events
    this.stage.on('mousemove', (e) => {
      if (this.isPanning) {
        this.handlePan(e);
      } else if (this.isConnectionMode && this.selectedFromHole && this.temporaryLine) {
        this.updateTemporaryLine(e);
      }
    });

    // Right click for panning
    this.stage.on('mousedown', (e) => {
      if (e.evt.button === 2) { // Right mouse button
        this.startPan(e);
        e.cancelBubble = true;
      }
    });

    this.stage.on('mouseup', (e) => {
      if (e.evt.button === 2) { // Right mouse button
        this.endPan();
      }
    });

    // Zoom with mouse wheel
    this.stage.on('wheel', (e) => {
      e.evt.preventDefault();
      this.handleZoom(e);
    });

    // Context menu prevention (right click)
    this.stage.container().addEventListener('contextmenu', (e) => {
      e.preventDefault();
    });

    // Handle mouse leave to end panning
    this.stage.on('mouseleave', () => {
      this.endPan();
    });
  }

  private handleConnectionClick(e: Konva.KonvaEventObject<MouseEvent>): void {
    const clickedPoint = this.getClickedDrillPoint(e);
    
    if (!clickedPoint) {
      this.cancelConnection();
      return;
    }

    if (!this.selectedFromHole) {
      // First selection
      this.selectedFromHole = clickedPoint;
      this.startTemporaryLine(clickedPoint);
      this.highlightPoint(clickedPoint, '#4CAF50');
    } else if (clickedPoint.id !== this.selectedFromHole.id) {
      // Second selection - create connection
      this.selectedToHole = clickedPoint;
      this.createConnection();
      this.cancelConnection();
    } else {
      // Same point clicked - cancel
      this.cancelConnection();
    }
  }

  private handleSelectionClick(e: Konva.KonvaEventObject<MouseEvent>): void {
    const clickedPoint = this.getClickedDrillPoint(e);
    // Handle point selection for editing/info display
    this.clearHighlights();
    if (clickedPoint) {
      this.highlightPoint(clickedPoint, '#2196F3');
    }
  }

  private getClickedDrillPoint(e: Konva.KonvaEventObject<MouseEvent>): DrillPoint | null {
    if (!this.patternData) return null;
    
    const pos = this.stage.getPointerPosition();
    if (!pos) return null;

    // Convert screen coordinates to world coordinates
    const worldPos = {
      x: (pos.x - this.panX) / this.scale,
      y: (pos.y - this.panY) / this.scale
    };

    return this.patternData.drillPoints.find(point => {
      const pointPos = this.convertToCanvasCoords(point.x, point.y);
      const distance = Math.sqrt(
        Math.pow(worldPos.x - pointPos.x, 2) + Math.pow(worldPos.y - pointPos.y, 2)
      );
      return distance <= (15 / this.scale); // Adjust click radius for zoom level
    }) || null;
  }

  private startTemporaryLine(fromPoint: DrillPoint): void {
    const fromPos = this.convertToCanvasCoords(fromPoint.x, fromPoint.y);
    
    this.temporaryLine = new Konva.Line({
      points: [fromPos.x, fromPos.y, fromPos.x, fromPos.y],
      stroke: '#4CAF50',
      strokeWidth: 2,
      dash: [5, 5]
    });
    
    this.connectionsLayer.add(this.temporaryLine);
    this.connectionsLayer.draw();
  }

  private updateTemporaryLine(e: Konva.KonvaEventObject<MouseEvent>): void {
    if (!this.temporaryLine || !this.selectedFromHole) return;
    
    const pos = this.stage.getPointerPosition();
    if (!pos) return;
    
    // Convert screen coordinates to world coordinates
    const worldPos = {
      x: (pos.x - this.panX) / this.scale,
      y: (pos.y - this.panY) / this.scale
    };
    
    const fromPos = this.convertToCanvasCoords(this.selectedFromHole.x, this.selectedFromHole.y);
    this.temporaryLine.points([fromPos.x, fromPos.y, worldPos.x, worldPos.y]);
    this.connectionsLayer.draw();
  }

  // Zoom and Pan Methods
  private handleZoom(e: Konva.KonvaEventObject<WheelEvent>): void {
    const scaleBy = 1.1;
    const stage = this.stage;
    const pointer = stage.getPointerPosition();
    
    if (!pointer) return;

    const mousePointTo = {
      x: (pointer.x - stage.x()) / this.scale,
      y: (pointer.y - stage.y()) / this.scale,
    };

    let direction = e.evt.deltaY > 0 ? -1 : 1;
    
    // Reverse scroll direction on macOS (natural scrolling)
    if (navigator.platform.indexOf('Mac') > -1) {
      direction = -direction;
    }

    const newScale = direction > 0 ? this.scale * scaleBy : this.scale / scaleBy;
    
    // Clamp scale to min/max values
    this.scale = Math.max(this.minScale, Math.min(this.maxScale, newScale));

    const newPos = {
      x: pointer.x - mousePointTo.x * this.scale,
      y: pointer.y - mousePointTo.y * this.scale,
    };

    this.panX = newPos.x;
    this.panY = newPos.y;

    this.applyTransform();
    this.cdr.detectChanges();
  }

  private startPan(e: Konva.KonvaEventObject<MouseEvent>): void {
    this.isPanning = true;
    const pointer = this.stage.getPointerPosition();
    if (pointer) {
      this.lastPanPoint = { x: pointer.x, y: pointer.y };
    }
    this.stage.container().style.cursor = 'grab';
  }

  private handlePan(e: Konva.KonvaEventObject<MouseEvent>): void {
    if (!this.isPanning) return;
    
    const pointer = this.stage.getPointerPosition();
    if (!pointer) return;

    const dx = pointer.x - this.lastPanPoint.x;
    const dy = pointer.y - this.lastPanPoint.y;

    this.panX += dx;
    this.panY += dy;

    this.lastPanPoint = { x: pointer.x, y: pointer.y };
    
    this.applyTransform();
  }

  private endPan(): void {
    this.isPanning = false;
    this.stage.container().style.cursor = this.isConnectionMode ? 'crosshair' : 'default';
  }

  private applyTransform(): void {
    this.stage.scale({ x: this.scale, y: this.scale });
    this.stage.position({ x: this.panX, y: this.panY });
    this.stage.batchDraw();
    
    // Redraw grid to adapt to new zoom/pan
    this.drawGrid();
  }

  // Public zoom methods for UI controls
  zoomIn(): void {
    const scaleBy = 1.2;
    const newScale = Math.min(this.maxScale, this.scale * scaleBy);
    this.setZoom(newScale);
  }

  zoomOut(): void {
    const scaleBy = 1.2;
    const newScale = Math.max(this.minScale, this.scale / scaleBy);
    this.setZoom(newScale);
  }

  resetZoom(): void {
    this.setZoom(1);
    this.centerView();
  }

  private setZoom(newScale: number): void {
    const stage = this.stage;
    const center = {
      x: stage.width() / 2,
      y: stage.height() / 2,
    };

    const mousePointTo = {
      x: (center.x - this.panX) / this.scale,
      y: (center.y - this.panY) / this.scale,
    };

    this.scale = newScale;

    const newPos = {
      x: center.x - mousePointTo.x * this.scale,
      y: center.y - mousePointTo.y * this.scale,
    };

    this.panX = newPos.x;
    this.panY = newPos.y;

    this.applyTransform();
  }

  private centerView(): void {
    if (!this.patternData || this.patternData.drillPoints.length === 0) {
      this.panX = 0;
      this.panY = 0;
      this.applyTransform();
      return;
    }

    // Calculate bounding box of all drill points
    const points = this.patternData.drillPoints;
    let minX = points[0].x, maxX = points[0].x;
    let minY = points[0].y, maxY = points[0].y;

    points.forEach(point => {
      minX = Math.min(minX, point.x);
      maxX = Math.max(maxX, point.x);
      minY = Math.min(minY, point.y);
      maxY = Math.max(maxY, point.y);
    });

    // Convert to canvas coordinates
    const topLeft = this.convertToCanvasCoords(minX, minY);
    const bottomRight = this.convertToCanvasCoords(maxX, maxY);

    // Calculate pattern dimensions
    const patternWidth = Math.abs(bottomRight.x - topLeft.x);
    const patternHeight = Math.abs(bottomRight.y - topLeft.y);

    // Calculate optimal zoom to fit pattern with adaptive padding
    const stageWidth = this.stage.width();
    const stageHeight = this.stage.height();
    
    // Adaptive padding based on canvas size and connection mode
    const basePadding = Math.min(stageWidth, stageHeight) * 0.06; // Reduced to 6% for more space
    const minPadding = this.isConnectionMode ? 40 : 60; // Even less padding in connection mode
    const padding = Math.max(basePadding, minPadding);
    
    const optimalScaleX = (stageWidth - padding * 2) / patternWidth;
    const optimalScaleY = (stageHeight - padding * 2) / patternHeight;
    const optimalScale = Math.min(optimalScaleX, optimalScaleY);
    
    // Clamp to zoom limits
    this.scale = Math.max(this.minScale, Math.min(this.maxScale, optimalScale));

    // Calculate center point
    const centerX = (topLeft.x + bottomRight.x) / 2;
    const centerY = (topLeft.y + bottomRight.y) / 2;

    // Center the view
    this.panX = (stageWidth / 2) - centerX * this.scale;
    this.panY = (stageHeight / 2) - centerY * this.scale;

    this.applyTransform();
  }

  getCurrentZoom(): number {
    return Math.round(this.scale * 100);
  }

  // New UI Methods
  exitConnectionMode(): void {
    this.isConnectionMode = false;
    this.cancelConnection();
    
    // Ensure canvas remains visible and handle layout change
    if (this.stage) {
      this.ensureCanvasVisibility();
      setTimeout(() => {
        this.handleLayoutModeChange();
      }, 150);
    }
    
    this.cdr.detectChanges();
  }

  setConnectorType(type: ConnectorType): void {
    this.selectedConnectorType = type;
    this.onConnectorTypeChange();
    this.cdr.detectChanges();
  }

  confirmClearConnections(): void {
    if (this.connections.length === 0) return;
    
    const confirmed = confirm(`Are you sure you want to clear all ${this.connections.length} connections? This action cannot be undone.`);
    if (confirmed) {
      this.clearAllConnections();
    }
  }

  getPerformanceStatus(): string {
    if (!this.patternData) return 'No data';
    
    const holes = this.patternData.drillPoints.length;
    const connections = this.connections.length;
    
    if (holes < 50) return 'Optimal';
    if (holes < 200) return 'Good';
    if (holes < 500) return 'Fair';
    return 'Heavy';
  }

  // Keyboard shortcuts
  @HostListener('window:keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): void {
    // Don't interfere with typing in inputs
    if (event.target instanceof HTMLInputElement || event.target instanceof HTMLSelectElement) {
      return;
    }

    switch (event.key.toLowerCase()) {
      case 'escape':
        if (this.isConnectionMode) {
          this.exitConnectionMode();
        }
        if (this.showHelp) {
          this.showHelp = false;
        }
        break;
      
      case 'h':
        this.showHelp = !this.showHelp;
        break;
      
      case ' ':
        event.preventDefault();
        this.resetZoom();
        break;
      
      case 'c':
        if (event.ctrlKey || event.metaKey) {
          event.preventDefault();
          if (!this.isConnectionMode && this.canCreateConnections()) {
            this.toggleConnectionMode();
          }
        }
        break;
      
      case 'z':
        if (event.ctrlKey || event.metaKey) {
          event.preventDefault();
          this.undoLastConnection();
        }
        break;
    }
    this.cdr.detectChanges();
  }

  private undoLastConnection(): void {
    if (this.connections.length > 0) {
      const lastConnection = this.connections[this.connections.length - 1];
      this.deleteConnection(lastConnection);
    }
  }

  private createConnection(): void {
    if (!this.selectedFromHole || !this.selectedToHole) return;

    // Check if delay is selected
    if (this.currentDelay === null) {
      alert('Please select a delay before creating connections.');
      return;
    }

    // Calculate positions for hidden starting and ending points
    const fromPos = this.convertToCanvasCoords(this.selectedFromHole.x, this.selectedFromHole.y);
    const toPos = this.convertToCanvasCoords(this.selectedToHole.x, this.selectedToHole.y);
    
    // Calculate offset positions for hidden points (slightly offset from the main holes)
    const offsetDistance = 15; // pixels
    const angle = Math.atan2(toPos.y - fromPos.y, toPos.x - fromPos.x);
    
    // Starting point (1) - offset from the from-hole
    const startPointOffset = {
      x: -Math.cos(angle) * offsetDistance,
      y: -Math.sin(angle) * offsetDistance
    };
    
    // Ending point (2) - offset from the to-hole  
    const endPointOffset = {
      x: Math.cos(angle) * offsetDistance,
      y: Math.sin(angle) * offsetDistance
    };

    const connection: BlastConnection = {
      id: `conn_${Date.now()}`,
      fromHoleId: this.selectedFromHole.id,
      toHoleId: this.selectedToHole.id,
      connectorType: this.selectedConnectorType,
      delay: this.currentDelay,
      sequence: this.currentSequence,
      // Hidden starting point with label "1"
      startPoint: {
        id: `start_${Date.now()}`,
        label: "1",
        x: this.selectedFromHole.x + (startPointOffset.x / this.scale),
        y: this.selectedFromHole.y + (startPointOffset.y / this.scale),
        isHidden: true
      },
      // Hidden ending point with label "2"
      endPoint: {
        id: `end_${Date.now()}`,
        label: "2", 
        x: this.selectedToHole.x + (endPointOffset.x / this.scale),
        y: this.selectedToHole.y + (endPointOffset.y / this.scale),
        isHidden: true
      }
    };

    this.connections.push(connection);
    
    // Update shared data service
    this.blastSequenceDataService.addConnection(connection);
    
    this.drawConnection(connection);
    this.currentSequence++;
    this.updateFilteredConnections();
    
    console.log('Connection created with hidden points:', connection);
    this.cdr.detectChanges();
  }

  private drawConnection(connection: BlastConnection): void {
    if (!this.patternData) return;
    
    const fromPoint = this.patternData.drillPoints.find(p => p.id === connection.fromHoleId);
    const toPoint = this.patternData.drillPoints.find(p => p.id === connection.toHoleId);
    
    if (!fromPoint || !toPoint) return;
    
    const fromPos = this.convertToCanvasCoords(fromPoint.x, fromPoint.y);
    const toPos = this.convertToCanvasCoords(toPoint.x, toPoint.y);
    
    // Convert hidden points to canvas coordinates
    const startPointPos = this.convertToCanvasCoords(connection.startPoint.x, connection.startPoint.y);
    const endPointPos = this.convertToCanvasCoords(connection.endPoint.x, connection.endPoint.y);
    
    const connectionGroup = new Konva.Group();
    
    // Main connection line (from drill hole to drill hole)
    const line = new Konva.Line({
      points: [fromPos.x, fromPos.y, toPos.x, toPos.y],
      stroke: this.getConnectorColor(connection.connectorType),
      strokeWidth: 3,
      lineCap: 'round'
    });
    
    // Hidden starting point (1) - smaller circle
    const startPointCircle = new Konva.Circle({
      x: startPointPos.x,
      y: startPointPos.y,
      radius: 8,
      fill: connection.startPoint.isHidden ? 'rgba(255, 255, 255, 0.8)' : 'white',
      stroke: this.getConnectorColor(connection.connectorType),
      strokeWidth: 1.5,
      opacity: connection.startPoint.isHidden ? 0.7 : 1,
      visible: this.showHiddenPoints
    });
    
    const startPointLabel = new Konva.Text({
      x: startPointPos.x - 3,
      y: startPointPos.y - 4,
      text: connection.startPoint.label,
      fontSize: 8,
      fill: 'black',
      fontStyle: 'bold',
      visible: this.showHiddenPoints
    });
    
    // Hidden ending point (2) - smaller circle
    const endPointCircle = new Konva.Circle({
      x: endPointPos.x,
      y: endPointPos.y,
      radius: 8,
      fill: connection.endPoint.isHidden ? 'rgba(255, 255, 255, 0.8)' : 'white',
      stroke: this.getConnectorColor(connection.connectorType),
      strokeWidth: 1.5,
      opacity: connection.endPoint.isHidden ? 0.7 : 1,
      visible: this.showHiddenPoints
    });
    
    const endPointLabel = new Konva.Text({
      x: endPointPos.x - 3,
      y: endPointPos.y - 4,
      text: connection.endPoint.label,
      fontSize: 8,
      fill: 'black',
      fontStyle: 'bold',
      visible: this.showHiddenPoints
    });
    
    // Sequence label (main connection identifier)
    const midX = (fromPos.x + toPos.x) / 2;
    const midY = (fromPos.y + toPos.y) / 2;
    
    const sequenceCircle = new Konva.Circle({
      x: midX,
      y: midY,
      radius: 12,
      fill: 'white',
      stroke: this.getConnectorColor(connection.connectorType),
      strokeWidth: 2
    });
    
    const sequenceLabel = new Konva.Text({
      x: midX - 5,
      y: midY - 6,
      text: connection.sequence.toString(),
      fontSize: 10,
      fill: 'black',
      fontStyle: 'bold'
    });
    
    // Add all elements to the connection group
    connectionGroup.add(
      line, 
      startPointCircle, 
      startPointLabel, 
      endPointCircle, 
      endPointLabel,
      sequenceCircle, 
      sequenceLabel
    );
    
    this.connectionsLayer.add(connectionGroup);
    this.connectionObjects.set(connection.id, connectionGroup);
    this.connectionsLayer.draw();
  }

  public getConnectorColor(type: ConnectorType): string {
    switch (type) {
      case ConnectorType.DETONATING_CORD: return '#FF5722'; // Red for Non-Electric-detonation-wire
      case ConnectorType.CONNECTORS: return '#FF9800'; // Orange for Non-Electric-connectors-wire
      default: return '#666666';
    }
  }

  private cancelConnection(): void {
    this.selectedFromHole = null;
    this.selectedToHole = null;
    
    if (this.temporaryLine) {
      this.temporaryLine.destroy();
      this.temporaryLine = null;
      this.connectionsLayer.draw();
    }
    
    this.clearHighlights();
  }

  private highlightPoint(point: DrillPoint, color: string): void {
    const pointGroup = this.drillPointObjects.get(point.id);
    if (pointGroup) {
      const circle = pointGroup.findOne('.drill-hole-circle') as Konva.Circle;
      if (circle) {
        circle.stroke(color);
        circle.strokeWidth(3);
        this.pointsLayer.draw();
      }
    }
  }

  private clearHighlights(): void {
    this.drillPointObjects.forEach(pointGroup => {
      const circle = pointGroup.findOne('.drill-hole-circle') as Konva.Circle;
      if (circle) {
        circle.stroke('#1971c2');
        circle.strokeWidth(2);
      }
    });
    this.pointsLayer.draw();
  }

  private drawGrid(): void {
    if (!this.patternData) return;
    
    this.gridLayer.destroyChildren();
    
    // Calculate adaptive grid size based on zoom level
    const baseGridSize = 50; // Base grid size in pixels
    const zoomAdjustedGridSize = this.calculateAdaptiveGridSize(baseGridSize);
    
    // Get the visible area in world coordinates
    const visibleBounds = this.getVisibleWorldBounds();
    
    // Extend bounds to ensure full coverage
    const padding = zoomAdjustedGridSize * 2;
    const startX = Math.floor((visibleBounds.left - padding) / zoomAdjustedGridSize) * zoomAdjustedGridSize;
    const endX = Math.ceil((visibleBounds.right + padding) / zoomAdjustedGridSize) * zoomAdjustedGridSize;
    const startY = Math.floor((visibleBounds.top - padding) / zoomAdjustedGridSize) * zoomAdjustedGridSize;
    const endY = Math.ceil((visibleBounds.bottom + padding) / zoomAdjustedGridSize) * zoomAdjustedGridSize;
    
    // Draw vertical lines
    for (let x = startX; x <= endX; x += zoomAdjustedGridSize) {
      const line = new Konva.Line({
        points: [x, startY, x, endY],
        stroke: this.getGridColor(),
        strokeWidth: this.getGridStrokeWidth(),
        opacity: this.getGridOpacity()
      });
      this.gridLayer.add(line);
    }
    
    // Draw horizontal lines
    for (let y = startY; y <= endY; y += zoomAdjustedGridSize) {
      const line = new Konva.Line({
        points: [startX, y, endX, y],
        stroke: this.getGridColor(),
        strokeWidth: this.getGridStrokeWidth(),
        opacity: this.getGridOpacity()
      });
      this.gridLayer.add(line);
    }
    
    this.gridLayer.draw();
  }

  private calculateAdaptiveGridSize(baseSize: number): number {
    // Adjust grid size based on zoom level for optimal visibility
    if (this.scale >= 2.0) return baseSize / 2;      // Smaller grid when zoomed in
    if (this.scale >= 1.0) return baseSize;          // Normal grid
    if (this.scale >= 0.5) return baseSize * 2;      // Larger grid when zoomed out
    return baseSize * 4;                             // Much larger grid when very zoomed out
  }

  private getVisibleWorldBounds(): { left: number, top: number, right: number, bottom: number } {
    const stageWidth = this.stage.width();
    const stageHeight = this.stage.height();
    
    // Convert screen bounds to world coordinates
    const topLeft = this.screenToWorld(0, 0);
    const bottomRight = this.screenToWorld(stageWidth, stageHeight);
    
    return {
      left: topLeft.x,
      top: topLeft.y,
      right: bottomRight.x,
      bottom: bottomRight.y
    };
  }

  private screenToWorld(screenX: number, screenY: number): { x: number, y: number } {
    return {
      x: (screenX - this.panX) / this.scale,
      y: (screenY - this.panY) / this.scale
    };
  }

  private getGridColor(): string {
    // Adjust grid color based on zoom level for better visibility
    if (this.scale >= 1.5) return '#d0d0d0';      // Lighter when zoomed in
    if (this.scale >= 0.8) return '#e0e0e0';      // Normal
    return '#f0f0f0';                              // Even lighter when zoomed out
  }

  private getGridStrokeWidth(): number {
    // Adjust stroke width to maintain visibility at different zoom levels
    return Math.max(0.5, 1 / this.scale);
  }

  private getGridOpacity(): number {
    // Fade grid at extreme zoom levels
    if (this.scale < 0.3) return 0.3;
    if (this.scale > 3.0) return 0.6;
    return 0.8;
  }

  private calculateZoomLimits(): void {
    if (!this.patternData || this.patternData.drillPoints.length === 0 || !this.stage) {
      this.minScale = 0.1;
      this.maxScale = 5;
      return;
    }

    // Calculate bounding box of all drill points
    const points = this.patternData.drillPoints;
    let minX = points[0].x, maxX = points[0].x;
    let minY = points[0].y, maxY = points[0].y;

    points.forEach(point => {
      minX = Math.min(minX, point.x);
      maxX = Math.max(maxX, point.x);
      minY = Math.min(minY, point.y);
      maxY = Math.max(maxY, point.y);
    });

    // Convert to canvas coordinates
    const topLeft = this.convertToCanvasCoords(minX, minY);
    const bottomRight = this.convertToCanvasCoords(maxX, maxY);

    // Calculate pattern dimensions
    const patternWidth = Math.abs(bottomRight.x - topLeft.x);
    const patternHeight = Math.abs(bottomRight.y - topLeft.y);
    const patternSize = Math.max(patternWidth, patternHeight);

    // Get current stage dimensions
    const stageWidth = this.stage.width();
    const stageHeight = this.stage.height();
    
    // Adaptive padding based on canvas size and pattern size
    const basePadding = Math.min(stageWidth, stageHeight) * 0.1; // 10% of smaller dimension
    const patternPadding = patternSize * 0.15; // 15% of pattern size
    const padding = Math.max(basePadding, patternPadding, 50); // Minimum 50px
    
    const paddedWidth = patternWidth + (padding * 2);
    const paddedHeight = patternHeight + (padding * 2);

    // Calculate minimum scale to fit pattern in view with padding
    const scaleX = stageWidth / paddedWidth;
    const scaleY = stageHeight / paddedHeight;
    
    // Set minimum scale to ensure pattern is always visible (with 15% extra margin)
    this.minScale = Math.max(0.05, Math.min(scaleX, scaleY) * 0.85);
    
    // Set maximum scale based on pattern density and canvas size
    const pointDensity = points.length / (patternSize / 1000); // points per 1000 canvas units
    const canvasArea = stageWidth * stageHeight;
    const baseMaxScale = canvasArea > 500000 ? 10 : canvasArea > 200000 ? 8 : 6; // Scale based on canvas size
    
    if (pointDensity > 5) {
      this.maxScale = baseMaxScale;  // High density patterns
    } else if (pointDensity > 2) {
      this.maxScale = baseMaxScale * 0.8;  // Medium density
    } else {
      this.maxScale = baseMaxScale * 0.6;  // Low density patterns
    }
    
    // Ensure current scale is within new limits
    this.scale = Math.max(this.minScale, Math.min(this.maxScale, this.scale));
  }

  private drawDrillPoints(): void {
    if (!this.patternData) return;
    
    this.patternData.drillPoints.forEach(point => {
      const canvasPos = this.convertToCanvasCoords(point.x, point.y);
      
      const pointGroup = new Konva.Group({
        x: canvasPos.x,
        y: canvasPos.y
      });
      
      // Drill hole circle
      const circle = new Konva.Circle({
        radius: 8,
        fill: '#ffffff',
        stroke: '#1971c2',
        strokeWidth: 2,
        name: 'drill-hole-circle'
      });
      
      // Hole ID label
      const label = new Konva.Text({
        x: -15,
        y: -25,
        text: point.id,
        fontSize: 10,
        fill: '#333333',
        fontStyle: 'bold'
      });
      
      pointGroup.add(circle, label);
      this.pointsLayer.add(pointGroup);
      this.drillPointObjects.set(point.id, pointGroup);
    });
    
    this.pointsLayer.draw();
  }

  private convertToCanvasCoords(x: number, y: number): { x: number, y: number } {
    // Convert drill pattern coordinates to canvas coordinates
    // This is a simple conversion - you might need to adjust based on your coordinate system
    const scale = 20; // 20px per meter
    const offsetX = 100; // Offset from left edge
    const offsetY = 100; // Offset from top edge
    
    return {
      x: offsetX + (x * scale),
      y: offsetY + (y * scale)
    };
  }

  private cleanup(): void {
    if (this.stage) {
      this.stage.destroy();
    }
  }

  // Public methods for UI interaction
  toggleConnectionMode(): void {
    this.isConnectionMode = !this.isConnectionMode;
    if (!this.isConnectionMode) {
      this.cancelConnection();
    }
    this.clearHighlights();
    
    // Ensure canvas remains visible in all modes and resize if needed
    if (this.stage) {
      this.ensureCanvasVisibility();
      // Delay resize to allow DOM to update after layout changes
      setTimeout(() => {
        this.handleLayoutModeChange();
      }, 150);
    }
    
    this.cdr.detectChanges();
  }

  private handleLayoutModeChange(): void {
    if (!this.stage) return;
    
    // Resize canvas to fit new layout
    this.resizeCanvas();
    
    // Adjust zoom if pattern is no longer well-positioned
    if (this.patternData && this.patternData.drillPoints.length > 0) {
      const currentZoom = this.scale;
      
      // If zoom is too close to limits, recenter the view
      if (currentZoom <= this.minScale * 1.1 || currentZoom >= this.maxScale * 0.9) {
        this.centerView();
      } else {
        // Just maintain current center
        this.maintainViewportCenter();
        this.applyTransform();
      }
    }
  }

  clearAllConnections(): void {
    this.connections = [];
    
    // Update shared data service
    this.blastSequenceDataService.clearConnections();
    
    this.connectionObjects.forEach(obj => obj.destroy());
    this.connectionObjects.clear();
    this.connectionsLayer.draw();
    this.currentSequence = 1;
    this.updateFilteredConnections();
    this.cdr.detectChanges();
  }

  toggleConnectionsVisibility(): void {
    this.showConnections = !this.showConnections;
    this.connectionsLayer.visible(this.showConnections);
    this.connectionsLayer.draw();
    this.cdr.detectChanges();
  }

  toggleHiddenPointsVisibility(): void {
    this.showHiddenPoints = !this.showHiddenPoints;
    this.redrawConnections();
  }

  exportBlastSequence(): void {
    if (!this.patternData) return;
    
    const blastSequenceData: BlastSequenceData = {
      patternData: this.patternData,
      connections: [...this.connections],
      detonators: [...this.detonators],
      metadata: {
        exportedAt: new Date().toISOString(),
        version: '1.0',
        totalSequenceTime: Math.max(...this.connections.map(c => c.delay), 0)
      }
    };
    
    this.patternDataService.exportBlastSequence(blastSequenceData);
  }

  backToPatternCreator(): void {
    this.router.navigate(['/blasting-engineer/drilling-pattern']);
  }

  onSaveSequence(): void {
    if (this.connections.length === 0) {
      console.warn('No connections to save');
      return;
    }

    // Update data service (in memory)
    this.blastSequenceDataService.setConnections(this.connections, false);
    
    // Explicitly save to storage
    this.blastSequenceDataService.saveConnections();

    // Update save state
    this.isSaved = true;
    this.cdr.markForCheck();

    // Clear save timeout if it exists
    if (this.saveTimeout) {
      clearTimeout(this.saveTimeout);
    }

    // Reset save state after 3 seconds
    this.saveTimeout = setTimeout(() => {
      this.isSaved = false;
      this.cdr.markForCheck();
    }, 3000);

    console.log('Blast sequence saved successfully');
  }

  goToSimulator(): void {
    if (this.connections.length > 0) {
      // Update data service (in memory)
      this.blastSequenceDataService.setConnections(this.connections, false);
      
      // Save connections when navigating to next step
      this.blastSequenceDataService.saveConnections();
      
      this.router.navigate(['/blasting-engineer/blast-sequence-simulator']);
    }
  }

  @HostListener('window:resize')
  onResize(): void {
    if (this.resizeTimeout) {
      clearTimeout(this.resizeTimeout);
    }
    this.resizeTimeout = setTimeout(() => {
      if (this.stage && this.containerRef) {
        this.resizeCanvas();
        this.redrawAll();
      }
    }, 250);
  }

  private redrawAll(): void {
    this.calculateZoomLimits();
    this.drawGrid();
    // Points and connections don't need to be redrawn as they maintain their positions
  }

  // Template helper methods
  trackConnection(index: number, connection: BlastConnection): string {
    return connection.id;
  }

  getConnectorTypeName(type: ConnectorType): string {
    switch (type) {
      case ConnectorType.DETONATING_CORD: return 'Detonating Cord';
      case ConnectorType.CONNECTORS: return 'Connectors';
      default: return 'Unknown';
    }
  }

  // Check if the selected connector type is non-electric (all types are non-electric now)
  isNonElectricConnector(): boolean {
    return true; // All connector types are now non-electric
  }

  // Get available delay options for the current connector type
  getAvailableDelays(): number[] {
    switch (this.selectedConnectorType) {
      case ConnectorType.DETONATING_CORD:
        return this.detonatingCordDelays;
      case ConnectorType.CONNECTORS:
        return this.connectorsDelays;
      default:
        return [];
    }
  }

  // Set delay from predefined options
  setDelayFromOption(delay: number): void {
    this.currentDelay = delay;
  }

  // Check if user can create connections
  canCreateConnections(): boolean {
    return this.currentDelay !== null;
  }

  // Get current delay display text
  getCurrentDelayText(): string {
    return this.currentDelay !== null ? `${this.currentDelay}ms` : 'No delay selected';
  }

  // Handle connector type change to reset delay appropriately
  onConnectorTypeChange(): void {
    // Reset delay selection when connector type changes
    this.currentDelay = null;
  }

  deleteConnection(connection: BlastConnection): void {
    const index = this.connections.findIndex(c => c.id === connection.id);
    if (index !== -1) {
      this.connections.splice(index, 1);
      
      // Update shared data service
      this.blastSequenceDataService.removeConnection(connection.id);
      
      // Remove visual representation
      const connectionObj = this.connectionObjects.get(connection.id);
      if (connectionObj) {
        connectionObj.destroy();
        this.connectionObjects.delete(connection.id);
        this.connectionsLayer.draw();
      }
      
      // Renumber sequences
      this.renumberSequences();
      this.updateFilteredConnections();
      this.cdr.detectChanges();
    }
  }

  private renumberSequences(): void {
    this.connections.forEach((connection, index) => {
      connection.sequence = index + 1;
    });
    this.currentSequence = this.connections.length + 1;
    
    // Redraw connections with new sequence numbers
    this.redrawConnections();
  }

  private redrawConnections(): void {
    // Clear existing connection visuals
    this.connectionObjects.forEach(obj => obj.destroy());
    this.connectionObjects.clear();
    
    // Redraw all connections with current settings
    this.connections.forEach(connection => {
      this.drawConnection(connection);
    });
    
    this.connectionsLayer.draw();
  }

  // Method to ensure connection has proper hidden points structure
  private ensureConnectionHasHiddenPoints(connection: BlastConnection): BlastConnection {
    if (connection.startPoint && connection.endPoint) {
      return connection;
    }

    // If missing hidden points, create them
    const fromHole = this.patternData?.drillPoints.find(p => p.id === connection.fromHoleId);
    const toHole = this.patternData?.drillPoints.find(p => p.id === connection.toHoleId);
    
    if (!fromHole || !toHole) {
      return connection;
    }
    
    // Calculate offset positions for hidden points
    const offsetDistance = 0.5; // world units
    const angle = Math.atan2(toHole.y - fromHole.y, toHole.x - fromHole.x);
    
    return {
      ...connection,
      startPoint: {
        id: `start_${connection.id}`,
        label: "1",
        x: fromHole.x - Math.cos(angle) * offsetDistance,
        y: fromHole.y - Math.sin(angle) * offsetDistance,
        isHidden: true
      },
      endPoint: {
        id: `end_${connection.id}`,
        label: "2",
        x: toHole.x + Math.cos(angle) * offsetDistance,
        y: toHole.y + Math.sin(angle) * offsetDistance,
        isHidden: true
      }
    };
  }

  // Enhanced Panel Methods
  toggleConnectionsPanel(): void {
    this.showConnectionsPanel = !this.showConnectionsPanel;
    if (this.showConnectionsPanel) {
      this.updateFilteredConnections();
    }
    this.cdr.detectChanges();
  }

  toggleSearch(): void {
    this.showSearch = !this.showSearch;
    if (!this.showSearch) {
      this.searchTerm = '';
      this.updateFilteredConnections();
    }
    this.cdr.detectChanges();
  }

  toggleSort(): void {
    if (this.sortBy === 'sequence') {
      this.sortBy = 'type';
    } else if (this.sortBy === 'type') {
      this.sortBy = 'delay';
    } else {
      this.sortBy = 'sequence';
    }
    this.sortConnections();
  }

  filterConnections(): void {
    this.updateFilteredConnections();
    this.cdr.detectChanges();
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.updateFilteredConnections();
    this.cdr.detectChanges();
  }

  sortConnections(): void {
    this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    this.updateFilteredConnections();
    this.cdr.detectChanges();
  }

  getFilteredConnections(): BlastConnection[] {
    return this.filteredConnections;
  }

  private updateFilteredConnections(): void {
    let filtered = [...this.connections];

    // Apply search filter
    if (this.searchTerm) {
      const searchLower = this.searchTerm.toLowerCase();
      filtered = filtered.filter(connection => 
        connection.fromHoleId.toLowerCase().includes(searchLower) ||
        connection.toHoleId.toLowerCase().includes(searchLower) ||
        connection.sequence.toString().includes(searchLower) ||
        this.getConnectorTypeName(connection.connectorType).toLowerCase().includes(searchLower) ||
        connection.delay.toString().includes(searchLower)
      );
    }

    // Apply sorting
    filtered.sort((a, b) => {
      let aValue: string | number;
      let bValue: string | number;

      switch (this.sortBy) {
        case 'sequence':
          aValue = a.sequence;
          bValue = b.sequence;
          break;
        case 'type':
          aValue = this.getConnectorTypeName(a.connectorType);
          bValue = this.getConnectorTypeName(b.connectorType);
          break;
        case 'delay':
          aValue = a.delay;
          bValue = b.delay;
          break;
        default:
          aValue = a.sequence;
          bValue = b.sequence;
      }

      if (typeof aValue === 'string' && typeof bValue === 'string') {
        return this.sortDirection === 'asc' 
          ? aValue.localeCompare(bValue)
          : bValue.localeCompare(aValue);
      } else {
        return this.sortDirection === 'asc' 
          ? (aValue as number) - (bValue as number)
          : (bValue as number) - (aValue as number);
      }
    });

    this.filteredConnections = filtered;
  }

  isConnectionHighlighted(connection: BlastConnection): boolean {
    // Add logic to highlight connections based on selection or hover
    return false;
  }

  exportConnections(): void {
    // Export filtered connections as CSV or JSON
    const connectionsData = this.getFilteredConnections().map(connection => ({
      sequence: connection.sequence,
      from: connection.fromHoleId,
      to: connection.toHoleId,
      type: this.getConnectorTypeName(connection.connectorType),
      delay: connection.delay
    }));

    const dataStr = JSON.stringify(connectionsData, null, 2);
    const dataBlob = new Blob([dataStr], { type: 'application/json' });
    const url = URL.createObjectURL(dataBlob);
    const link = document.createElement('a');
    link.href = url;
    link.download = 'blast-connections.json';
    link.click();
    URL.revokeObjectURL(url);
  }
}
