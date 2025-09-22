import { Component, ElementRef, ViewChild, AfterViewInit, ChangeDetectorRef, ChangeDetectionStrategy, HostListener, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import Konva from 'konva';
import { UnifiedDrillDataService, PatternData } from '../../../core/services/unified-drill-data.service';
import { SiteBlastingService } from '../../../core/services/site-blasting.service';
import { NotificationService } from '../../../core/services/notification.service';
import { 
  DrillPoint, 
  DetonatorInfo, 
  DetonatorType
} from '../drilling-pattern-creator/models/drill-point.model';
import { ConnectorType, BlastConnection, UpdateBlastConnectionRequest } from '../../../core/models/site-blasting.model';
import { DrillLocation } from '../../../core/models/drilling.model';

// Local interface for export functionality
interface BlastSequenceData {
  patternData: {
    drillPoints: DrillPoint[];
    settings: {
      spacing: number;
      burden: number;
      depth: number;
    };
  };
  connections: BlastConnection[];
  detonators: DetonatorInfo[];
  metadata: {
    exportedAt: string;
    version: string;
    totalSequenceTime: number;
  };
}

interface SmartHint {
  id: string;
  title: string;
  message: string;
  icon: string;
  actionText?: string;
  actionCallback?: () => void;
  priority: 'high' | 'medium' | 'low';
  dismissible: boolean;
}

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
  selectedConnectorType: ConnectorType = ConnectorType.DetonatingCord;
  currentDelay: number | null = null;
  currentSequence = 1;
  showHelp = false;

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

  // Site context
  private currentProjectId!: number;
  private currentSiteId!: number;

  // Continuous connection throttling
  private lastHoverTime = 0;
  private hoverThrottleDelay = 100; // milliseconds
  private lastHoveredHoleId: string | null = null;

  // Context menu and starting hole functionality
  public showContextMenu = false;
  public contextMenuX = 0;
  public contextMenuY = 0;
  public contextMenuHole: any = null;
  public startingHoleId: string | null = null;

  // Smart Hints System
  public showSmartHint = true;
  public currentHint: SmartHint | null = null;
  public dismissedHints: Set<string> = new Set();

  // Progressive Workflow
  public showWizard = false;
  public wizardStep = 1;
  public isFirstTimeUser = true;
  public wizardSteps = [
    { 
      id: 1, 
      title: 'Select Starting Hole', 
      description: 'Right-click on a hole to set as starting point',
      icon: 'play_arrow',
      target: '.canvas-container'
    },
    { 
      id: 2, 
      title: 'Choose Connector Type', 
      description: 'Select detonating cord or connectors',
      icon: 'cable',
      target: '.connector-type-selector'
    },
    { 
      id: 3, 
      title: 'Set Delay Time', 
      description: 'Choose the delay timing for connections',
      icon: 'schedule',
      target: '.delay-selector'
    },
    { 
      id: 4, 
      title: 'Create Connections', 
      description: 'Click holes to create blast sequence',
      icon: 'link',
      target: '.mode-button'
    }
  ];

  // Enhanced Status
  public completionPercentage = 0;
  public nextActionText = '';
  public nextActionIcon = '';

  // Connection Preview
  public previewConnection: { from: DrillPoint, to: DrillPoint } | null = null;
  public hoveredHole: DrillPoint | null = null;

  constructor(
    private cdr: ChangeDetectorRef,
    private unifiedDrillDataService: UnifiedDrillDataService,
    private siteBlastingService: SiteBlastingService,
    private router: Router,
    private notification: NotificationService
  ) {}

  ngAfterViewInit(): void {
    // Check if user has completed wizard before
    const wizardCompleted = localStorage.getItem('blast-sequence-wizard-completed');
    if (!wizardCompleted) {
      this.isFirstTimeUser = true;
      setTimeout(() => this.startWizard(), 1000);
    }

    // Apply intelligent defaults
    this.applyIntelligentDefaults();

    // Initialize site context from route parameters
    this.initializeSiteContext();
    
    // Load pattern data first, then initialize canvas when data is available
    this.loadPatternData();
    
    // Initialize smart hints
    this.updateSmartHints();
  }

  private initializeSiteContext(): void {
    // Simplified: extract project and site from route
    const routeMatch = this.router.url.match(/project-management\/(\d+)\/sites\/(\d+)/);
    const projectId = routeMatch ? +routeMatch[1] : 4; // Default to project 4 for testing
    const siteId = routeMatch ? +routeMatch[2] : 3;    // Default to site 3 for testing

    console.log('🔍 Sequence Designer - Setting site context', { projectId, siteId });
    this.currentProjectId = projectId;
    this.currentSiteId = siteId;
    
    this.loadBackendSequenceData();
  }

  ngOnDestroy(): void {
    if (this.resizeTimeout) {
      clearTimeout(this.resizeTimeout);
    }
    this.cleanup();
  }

  private loadPatternData(): void {
    if (!this.patternData && this.currentProjectId && this.currentSiteId) {
      // If no pattern data in memory, try to load from backend
      this.unifiedDrillDataService.loadPatternData(this.currentProjectId, this.currentSiteId)
        .subscribe({
          next: (patternData) => {
            if (patternData && patternData.drillLocations && patternData.drillLocations.length > 0) {
              
              // Use the pattern data directly - it's already in the correct format
              this.patternData = patternData;
              
              console.log('✅ Loaded pattern from backend with', patternData.drillLocations.length, 'points');
              
              // Initialize canvas NOW that we have data
              this.initializeCanvasWithData();
              
              this.cdr.detectChanges();
            } else {
              console.warn('⚠️ No pattern data available in backend. Please create a pattern first.');
              this.notification.showError('No drill pattern found. Please create a pattern first.');
              this.backToPatternCreator();
            }
          },
          error: (error) => {
            console.error('❌ Error loading pattern from backend:', error);
            this.notification.showError('Failed to load drill pattern data');
            this.backToPatternCreator();
          }
        });
    } else if (!this.patternData) {
      console.warn('⚠️ No pattern data available. Please create a pattern first.');
      this.notification.showError('No drill pattern found. Please create a pattern first.');
      this.backToPatternCreator();
    } else {
      // Pattern data already exists, initialize canvas
      this.initializeCanvasWithData();
    }
    
    // Initialize empty connections - they will be loaded separately
    this.connections = [];
    this.updateFilteredConnections();
    
    this.cdr.detectChanges();
  }

  private initializeCanvasWithData(): void {
    // Wait a bit for the DOM to be ready
    setTimeout(() => {
      this.initializeCanvas();
    }, 100);
  }

  private loadBackendSequenceData(): void {
    if (!this.currentProjectId || !this.currentSiteId) {
      return;
    }

    console.log('🔍 Loading sequence data for project', this.currentProjectId, 'site', this.currentSiteId);

    // Try to load blast connections from the API
    this.siteBlastingService.getBlastConnections(this.currentProjectId, this.currentSiteId)
      .subscribe({
        next: (connections) => {
          if (connections && connections.length > 0) {
            console.log('✅ Found', connections.length, 'blast connections in backend - using structured data');
            
            // Map API connections to frontend format
            this.connections = connections.map(conn => ({
              id: conn.id,
              point1DrillPointId: conn.point1DrillPointId,
              point2DrillPointId: conn.point2DrillPointId,
              fromHoleId: conn.point1DrillPointId,
              toHoleId: conn.point2DrillPointId,
              connectorType: this.mapConnectorTypeFromApi(conn.connectorType),
              delay: conn.delay,
              sequence: conn.sequence,
              projectId: conn.projectId,
              siteId: conn.siteId,
              createdAt: conn.createdAt,
              updatedAt: conn.updatedAt,
              // Default values for required properties
              startPoint: {
                id: `SP${conn.id}`,
                label: "1",
                x: 0,
                y: 0,
                isHidden: true
              },
              endPoint: {
                id: `EP${conn.id}`,
                label: "2",
                x: 0,
                y: 0,
                isHidden: true
              },
              // Navigation properties for UI
              point1DrillPoint: this.patternData?.drillLocations.find((p: DrillLocation) => p.id === conn.point1DrillPointId),
              point2DrillPoint: this.patternData?.drillLocations.find((p: DrillLocation) => p.id === conn.point2DrillPointId)
            }));
            
            // Ensure connections have proper structure for UI
            this.connections = this.connections.map(conn => 
              this.ensureConnectionHasHiddenPoints(conn)
            );
            
            // Update currentSequence to continue from the highest existing sequence number
            this.updateCurrentSequenceFromExistingConnections();
            
            this.updateFilteredConnections();
            
            // Redraw if canvas is ready
            setTimeout(() => {
              if (this.isInitialized) {
                this.redrawConnections();
              }
            }, 200);
            
            this.cdr.markForCheck();
          } else {
            console.log('⚠️ No blast connections found - ready for new connections');
            this.handleNoConnectionsAvailable();
          }
        },
        error: (error) => {
          console.error('❌ Error loading blast connections:', error);
          this.handleNoConnectionsAvailable();
        }
      });
  }

  private handleNoConnectionsAvailable(): void {
    console.log('🔍 No connections available - initializing empty state');
    this.connections = [];
    this.updateFilteredConnections();
    
    // Enable connection mode automatically when no connections exist
    if (this.patternData && this.patternData.drillLocations && this.patternData.drillLocations.length > 0) {
      console.log('⚡ Auto-enabling connection mode for new sequence');
      this.isConnectionMode = true;
      this.notification.showSuccess('Ready to create blast connections. Click drill points to connect them.');
    }
    
    this.cdr.markForCheck();
  }

  // Context Menu Methods
  private showDrillPointContextMenu(e: Konva.KonvaEventObject<MouseEvent>, drillPoint: any): void {
    // Hide any existing context menu
    this.hideContextMenu();
    
    // Get mouse position relative to the page
    const stage = e.target.getStage();
    const container = stage?.container();
    if (!container) return;
    
    const containerRect = container.getBoundingClientRect();
    this.contextMenuX = e.evt.clientX;
    this.contextMenuY = e.evt.clientY;
    this.contextMenuHole = drillPoint;
    this.showContextMenu = true;
    
    this.cdr.detectChanges();
  }

  public hideContextMenu(): void {
    this.showContextMenu = false;
    this.contextMenuHole = null;
    this.cdr.detectChanges();
  }

  public setAsStartingHole(hole: any): void {
    if (!hole) return;
    
    // Clear previous starting hole
    this.startingHoleId = hole.id;
    
    // Redraw drill points to update visual indicators
    this.drawDrillPoints();
    
    // Save to backend
    this.saveStartingHoleToBackend(hole.id);
    
    // Update smart hints
    this.updateSmartHints();
    
    this.notification.showSuccess(`Hole ${hole.id} set as starting hole for blast simulation`);
    this.hideContextMenu();
  }

  private saveStartingHoleToBackend(holeId: string): void {
    // Update all connections to mark the starting hole
    this.connections.forEach(connection => {
      const wasStartingHole = connection.isStartingHole;
      connection.isStartingHole = (connection.point1DrillPointId === holeId || connection.point2DrillPointId === holeId);
      
      // Only save if the status changed
      if (wasStartingHole !== connection.isStartingHole) {
        const updateRequest: UpdateBlastConnectionRequest = {
          id: connection.id,
          point1DrillPointId: connection.point1DrillPointId,
          point2DrillPointId: connection.point2DrillPointId,
          fromHoleId: connection.fromHoleId,
          toHoleId: connection.toHoleId,
          connectorType: connection.connectorType,
          delay: connection.delay,
          sequence: connection.sequence,
          isStartingHole: connection.isStartingHole,
          projectId: this.currentProjectId,
          siteId: this.currentSiteId
        };
        
        this.siteBlastingService.updateBlastConnection(connection.id, this.currentProjectId, this.currentSiteId, updateRequest).subscribe({
          next: (response) => {
            console.log('✅ Starting hole status updated for connection:', connection.id);
          },
          error: (error) => {
            console.error('❌ Failed to update starting hole status:', error);
            this.notification.showError('Failed to save starting hole to database');
          }
        });
      }
    });
  }

  public isStartingHole(holeId: string): boolean {
    return this.startingHoleId === holeId;
  }

  // Click outside to hide context menu
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (this.showContextMenu) {
      const target = event.target as HTMLElement;
      if (!target.closest('.context-menu')) {
        this.hideContextMenu();
      }
    }
  }

  // Helper method to map connector type from API numeric value to UI string
  private mapConnectorTypeFromApi(connectorType: number): ConnectorType {
    switch (connectorType) {
      case 0:
        return ConnectorType.DetonatingCord;
      case 1:
        return ConnectorType.Connectors;
      default:
        return ConnectorType.DetonatingCord;
    }
  }

  // Helper method to map connector type from UI string to API numeric value
  private mapConnectorTypeToApi(connectorType: ConnectorType): number {
    switch (connectorType) {
      case ConnectorType.DetonatingCord:
        return 0;
      case ConnectorType.Connectors:
        return 1;
      default:
        return 0;
    }
  }

  private initializeCanvas(): void {
    if (this.isInitialized || !this.containerRef) {
      return;
    }

    const container = this.containerRef.nativeElement;
    
    // Ensure container has proper dimensions
    if (container.offsetWidth === 0 || container.offsetHeight === 0) {
      console.log('⏳ Container not ready, retrying canvas initialization...');
      setTimeout(() => this.initializeCanvas(), 100);
      return;
    }
    
    console.log('🎨 Initializing canvas with container dimensions:', container.offsetWidth, 'x', container.offsetHeight);
    
    // Calculate optimal canvas size based on container and content
    const canvasWidth = Math.max(container.offsetWidth, 600); // Increased minimum width
    const canvasHeight = Math.max(container.offsetHeight, 400); // Increased minimum height
    
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
    
    // Initialize with reasonable defaults
    this.scale = 1.0;
    this.panX = 0;
    this.panY = 0;
    
    this.drawGrid();
    
    // Draw drill points - this should now work since we have pattern data
    console.log('🎯 Drawing drill points with pattern data:', this.patternData?.drillLocations?.length || 0, 'points');
    this.drawDrillPoints();
    
    // Center view immediately after drawing points
    setTimeout(() => {
      this.centerView();
      this.cdr.detectChanges();
    }, 50);
    
    // Draw existing connections if any
    if (this.connections.length > 0) {
      console.log('🔗 Drawing existing connections:', this.connections.length);
      setTimeout(() => {
        this.redrawConnections();
      }, 100);
    }
    
    this.isInitialized = true;
    console.log('✅ Canvas initialization complete');
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
    if (!this.patternData || this.patternData.drillLocations.length === 0) return;
    
    // Calculate the center of the pattern
    const points = this.patternData.drillLocations;
    let minX = points[0].x, maxX = points[0].x;
    let minY = points[0].y, maxY = points[0].y;

    points.forEach((point: DrillLocation) => {
      minX = Math.min(minX, point.x);
      maxX = Math.max(maxX, point.x);
      minY = Math.min(minY, point.y);
      maxY = Math.max(maxY, point.y);
    });

    const centerX = (minX + maxX) / 2;
    const centerY = (minY + maxY) / 2;

    // Center the view
    this.panX = (this.stage.width() / 2) - (centerX * this.scale);
    this.panY = (this.stage.height() / 2) - (centerY * this.scale);

    this.applyTransform();
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
        // Handle hover-based connection creation
        this.handleConnectionHover(e);
      }
    });

    // Right click for context menu or panning
    this.stage.on('mousedown', (e) => {
      if (e.evt.button === 2) { // Right mouse button
        const clickedPoint = this.getClickedDrillPoint(e);
        if (clickedPoint) {
          // Show context menu for drill point
          this.showDrillPointContextMenu(e, clickedPoint);
        } else {
          // Start panning if not clicking on a drill point
          this.startPan(e);
        }
        e.cancelBubble = true;
      }
    });

    this.stage.on('mouseup', (e) => {
      if (e.evt.button === 2) { // Right mouse button
        if (this.isPanning) {
          this.endPan();
        }
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
      // First selection - start chain connection mode
      this.selectedFromHole = clickedPoint;
      this.startTemporaryLine(clickedPoint);
      this.highlightPoint(clickedPoint, '#4CAF50');
      this.notification.showSuccess('Chain connection started! Hover over holes to create connections. Click any hole to stop.');
    } else {
      // Any click after initial selection stops the chain connection process
      this.cancelConnection();
      this.notification.showSuccess('Chain connection stopped.');
    }
  }

  private handleConnectionHover(e: Konva.KonvaEventObject<MouseEvent>): void {
    if (!this.selectedFromHole) return;
    
    const hoveredPoint = this.getClickedDrillPoint(e);
    
    if (!hoveredPoint || hoveredPoint.id === this.selectedFromHole.id) {
      return;
    }

    // Throttling to prevent excessive connection creation
    const currentTime = Date.now();
    if (currentTime - this.lastHoverTime < this.hoverThrottleDelay) {
      return;
    }

    // Skip if we're hovering over the same hole as before
    if (this.lastHoveredHoleId === hoveredPoint.id) {
      return;
    }

    this.lastHoverTime = currentTime;
    this.lastHoveredHoleId = hoveredPoint.id;

    // Check if connection already exists
    const existingConnection = this.connections.find(conn =>
      (conn.point1DrillPointId === this.selectedFromHole!.id && conn.point2DrillPointId === hoveredPoint.id) ||
      (conn.point1DrillPointId === hoveredPoint.id && conn.point2DrillPointId === this.selectedFromHole!.id)
    );

    if (existingConnection) {
      return; // Don't create duplicate connections
    }

    // Create connection on hover
    this.selectedToHole = hoveredPoint;
    this.createConnection(false); // Don't keep from hole selected
    
    // Make the hovered point the new starting point for chain connections
    this.selectedFromHole = hoveredPoint;
    this.selectedToHole = null;
    
    // Update visual feedback - highlight the new starting point
    this.clearHighlights();
    this.highlightPoint(hoveredPoint, '#4CAF50');
    
    // Start new temporary line from the new starting point
    this.startTemporaryLine(hoveredPoint);
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
    const target = e.target;
    const pointId = target.attrs.pointId;
    
    if (!pointId || !this.patternData) return null;
    
    // Convert DrillLocation to DrillPoint for internal use
    const drillLocation = this.patternData.drillLocations.find((point: DrillLocation) => {
      return point.id === pointId;
    });
    
    if (!drillLocation) return null;
    
    return {
      id: drillLocation.id,
      x: drillLocation.x,
      y: drillLocation.y,
      depth: drillLocation.depth,
      spacing: drillLocation.spacing,
      burden: drillLocation.burden,
      stemming: drillLocation.stemming || 2.0,
      subDrill: drillLocation.subDrill || 0.5
    };
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
    console.log('🔄 Resetting zoom');
    this.scale = 1.0; // Reset to 100%
    this.panX = 0;
    this.panY = 0;
    this.applyTransform();
    
    // After resetting, center the view properly
    setTimeout(() => {
      this.centerView();
    }, 50);
    
    this.cdr.detectChanges();
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
    if (!this.patternData || !this.patternData.drillLocations || this.patternData.drillLocations.length === 0) {
      console.warn('⚠️ No pattern data for centering view');
      return;
    }

    const points = this.patternData.drillLocations;
    console.log('🎯 Centering view with', points.length, 'points');

    // Calculate bounding box of all drill points in canvas coordinates
    let minX = Infinity, maxX = -Infinity;
    let minY = Infinity, maxY = -Infinity;

    points.forEach((point: DrillLocation) => {
      const canvasPos = this.convertToCanvasCoords(point.x, point.y);
      minX = Math.min(minX, canvasPos.x);
      maxX = Math.max(maxX, canvasPos.x);
      minY = Math.min(minY, canvasPos.y);
      maxY = Math.max(maxY, canvasPos.y);
    });

    // Add some padding around the pattern
    const padding = 50;
    minX -= padding;
    maxX += padding;
    minY -= padding;
    maxY += padding;

    const patternWidth = maxX - minX;
    const patternHeight = maxY - minY;
    const patternCenterX = (minX + maxX) / 2;
    const patternCenterY = (minY + maxY) / 2;

    console.log('📐 Pattern bounds:', { minX, maxX, minY, maxY, width: patternWidth, height: patternHeight });

    // Calculate scale to fit pattern in canvas with some margin
    const canvasWidth = this.stage.width();
    const canvasHeight = this.stage.height();
    const margin = 0.8; // Use 80% of canvas size to leave some margin

    // Prevent division by zero and ensure reasonable scaling
    const scaleX = patternWidth > 0 ? (canvasWidth * margin) / patternWidth : 1.0;
    const scaleY = patternHeight > 0 ? (canvasHeight * margin) / patternHeight : 1.0;
    
    // Choose the smaller scale to ensure everything fits, but cap it at reasonable values
    let optimalScale = Math.min(scaleX, scaleY);
    
    // Apply strict zoom limits
    optimalScale = Math.max(0.1, Math.min(2.0, optimalScale)); // Between 10% and 200%

    // Ensure scale is within our defined limits
    this.scale = Math.max(this.minScale, Math.min(this.maxScale, optimalScale));

    // Center the pattern in the canvas
    this.panX = (canvasWidth / 2) - (patternCenterX * this.scale);
    this.panY = (canvasHeight / 2) - (patternCenterY * this.scale);

    console.log('🔍 Applied zoom and pan:', { 
      scale: this.scale, 
      scalePercent: Math.round(this.scale * 100) + '%',
      panX: this.panX, 
      panY: this.panY,
      canvasSize: { width: canvasWidth, height: canvasHeight },
      patternSize: { width: patternWidth, height: patternHeight }
    });

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
    this.saveUserPreferences();
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
    if (!this.patternData) return 'No pattern data';
    
    const holes = this.patternData.drillLocations.length;
    const connections = this.connections.length;
    
    if (holes === 0) return 'No drill holes';
    if (connections === 0) return 'No connections';
    
    return `${holes} holes, ${connections} connections`;
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

  private createConnection(keepFromHoleSelected: boolean = false): void {
    if (!this.selectedFromHole || !this.selectedToHole) {
      console.warn('Both from and to holes must be selected');
      this.notification.showError('Please select both drill points to create a connection');
      return;
    }

    if (!this.selectedFromHole.id || !this.selectedToHole.id) {
      console.warn('Selected holes do not have valid IDs');
      this.notification.showError('Selected drill points are invalid');
      return;
    }

    if (this.selectedFromHole.id === this.selectedToHole.id) {
      console.warn('Cannot create connection to the same hole');
      this.notification.showError('Cannot create connection to the same drill point');
      return;
    }

    // Check if connection already exists
    const existingConnection = this.connections.find(conn =>
      (conn.point1DrillPointId === this.selectedFromHole!.id && conn.point2DrillPointId === this.selectedToHole!.id) ||
      (conn.point1DrillPointId === this.selectedToHole!.id && conn.point2DrillPointId === this.selectedFromHole!.id)
    );

    if (existingConnection) {
      console.warn('Connection already exists between these points');
      this.notification.showError('Connection already exists between these drill points');
      this.cancelConnection();
      return;
    }

    // Check if this is the first connection and ensure it starts from the starting hole
    if (this.connections.length === 0 && this.selectedFromHole.id !== this.startingHoleId) {
      console.warn('First connection must start from the starting hole');
      this.notification.showError('The first connection must start from the selected starting hole. Please select the starting hole first, then connect to another hole.');
      this.cancelConnection();
      return;
    }

    // Create the connection
    const newConnection: BlastConnection = {
      id: `conn-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`,
      point1DrillPointId: this.selectedFromHole.id,
      point2DrillPointId: this.selectedToHole.id,
      // Add fromHoleId and toHoleId for compatibility
      fromHoleId: this.selectedFromHole.id,
      toHoleId: this.selectedToHole.id,
      connectorType: this.selectedConnectorType,
      delay: this.currentDelay || this.getDefaultDelay(),
      sequence: this.currentSequence,
      projectId: this.currentProjectId,
      siteId: this.currentSiteId,
      // Add required startPoint and endPoint properties
      startPoint: {
        id: `SP-${this.selectedFromHole.id}`,
        label: "1",
        x: this.selectedFromHole.x,
        y: this.selectedFromHole.y,
        isHidden: true
      },
      endPoint: {
        id: `EP-${this.selectedToHole.id}`,
        label: "2",
        x: this.selectedToHole.x,
        y: this.selectedToHole.y,
        isHidden: true
      },
      // Navigation properties for UI
      point1DrillPoint: this.selectedFromHole,
      point2DrillPoint: this.selectedToHole
    };

    // Add to connections array
    this.connections.push(newConnection);
    this.updateFilteredConnections();

    // Save to backend immediately
    this.saveConnectionToBackend(newConnection);

    // Draw the connection
    this.drawConnection(newConnection);

    // Update sequence counter
    this.currentSequence++;

    // Reset selection conditionally
    if (keepFromHoleSelected) {
      // Only reset the target hole, keep the from hole selected for multiple connections
      this.selectedToHole = null;
      // Remove temporary line
      if (this.temporaryLine) {
        this.temporaryLine.destroy();
        this.temporaryLine = null;
      }
    } else {
      // Reset both selections (traditional behavior)
      this.cancelConnection();
    }

    // Update smart hints
    this.updateSmartHints();

    console.log('✅ Created connection:', newConnection);
    this.notification.showSuccess(`Connection created between points ${this.selectedFromHole?.id} and ${this.selectedToHole?.id}`);
  }

  private saveConnectionToBackend(connection: BlastConnection): void {
    const createRequest = {
      id: connection.id,
      projectId: connection.projectId,
      siteId: connection.siteId,
      point1DrillPointId: connection.point1DrillPointId,
      point2DrillPointId: connection.point2DrillPointId,
      connectorType: this.mapConnectorTypeToApi(connection.connectorType),
      delay: connection.delay,
      sequence: connection.sequence
    };

    console.log('🔗 Sending connection request to backend:', createRequest);

    this.siteBlastingService.createBlastConnection(createRequest)
      .subscribe({
        next: (savedConnection) => {
          console.log('✅ Connection saved to backend:', savedConnection);
          // Update the connection with the backend-generated ID
          connection.id = savedConnection.id.toString();
        },
        error: (error) => {
          console.error('❌ Failed to save connection to backend:', error);
          console.error('❌ Error details:', {
            status: error.status,
            message: error.message,
            error: error.error
          });
          this.notification.showError('Failed to save connection to database');
          // Remove from local connections on failure
          this.connections = this.connections.filter(c => c.id !== connection.id);
          this.updateFilteredConnections();
          this.redrawConnections();
        }
      });
  }

  private getDefaultDelay(): number {
    return this.selectedConnectorType === ConnectorType.DetonatingCord ? 
      this.detonatingCordDelays[0] : this.connectorsDelays[0];
  }

  private drawConnection(connection: BlastConnection): void {
    if (!this.patternData) return;

    // Find the actual drill points
    const fromPoint = this.patternData.drillLocations.find((p: DrillLocation) => p.id === connection.point1DrillPointId);
    const toPoint = this.patternData.drillLocations.find((p: DrillLocation) => p.id === connection.point2DrillPointId);

    if (!fromPoint || !toPoint) {
      console.warn('Could not find drill points for connection:', connection);
      return;
    }

    // Convert to canvas coordinates
    const fromCanvas = this.convertToCanvasCoords(fromPoint.x, fromPoint.y);
    const toCanvas = this.convertToCanvasCoords(toPoint.x, toPoint.y);

    // Create connection group
    const connectionGroup = new Konva.Group({
      connectionId: connection.id
    });

    // Draw the main connection line
    const connectionLine = new Konva.Line({
      points: [fromCanvas.x, fromCanvas.y, toCanvas.x, toCanvas.y],
      stroke: this.getConnectorColor(connection.connectorType),
      strokeWidth: 3,
      opacity: 0.8,
      connectionId: connection.id
    });

    connectionGroup.add(connectionLine);

    // Add sequence number in the middle
    const midX = (fromCanvas.x + toCanvas.x) / 2;
    const midY = (fromCanvas.y + toCanvas.y) / 2;

    const sequenceText = new Konva.Text({
      x: midX - 10,
      y: midY - 10,
      text: connection.sequence.toString(),
      fontSize: 12,
      fill: 'white',
      align: 'center',
      verticalAlign: 'middle',
      width: 20,
      height: 20,
      connectionId: connection.id
    });

    const sequenceBackground = new Konva.Rect({
      x: midX - 10,
      y: midY - 10,
      width: 20,
      height: 20,
      fill: this.getConnectorColor(connection.connectorType),
      cornerRadius: 10,
      connectionId: connection.id
    });

    connectionGroup.add(sequenceBackground);
    connectionGroup.add(sequenceText);

    // Add to layer and store reference
    this.connectionsLayer.add(connectionGroup);
    this.connectionObjects.set(connection.id, connectionGroup);

    this.connectionsLayer.draw();
  }

  public getConnectorColor(type: ConnectorType): string {
    switch (type) {
      case ConnectorType.DetonatingCord: return '#FF5722'; // Red for Non-Electric-detonation-wire
      case ConnectorType.Connectors: return '#FF9800'; // Orange for Non-Electric-connectors-wire
      default: return '#666666'; // Gray for unknown
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
    
    // Reset hover tracking for continuous connections
    this.lastHoveredHoleId = null;
    this.lastHoverTime = 0;
    
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
    if (!this.patternData || this.patternData.drillLocations.length === 0) {
      return;
    }

    this.gridLayer.destroyChildren();
    const points = this.patternData.drillLocations;

    points.forEach((point: DrillLocation) => {
      const canvasPos = this.convertToCanvasCoords(point.x, point.y);
      
      const gridPoint = new Konva.Circle({
        x: canvasPos.x,
        y: canvasPos.y,
        radius: 2,
        fill: this.getGridColor(),
        opacity: this.getGridOpacity()
      });

      this.gridLayer.add(gridPoint);
    });

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
    // Set reasonable fixed zoom limits
    this.minScale = 0.1;  // 10% minimum zoom
    this.maxScale = 3.0;  // 300% maximum zoom
    
    console.log('🔍 Zoom limits set:', { min: this.minScale, max: this.maxScale });
  }

  private drawDrillPoints(): void {
    if (!this.patternData || !this.patternData.drillLocations || this.patternData.drillLocations.length === 0) {
      console.warn('⚠️ No pattern data available for drawing drill points');
      return;
    }

    console.log('🎯 Drawing drill points:', this.patternData.drillLocations.length, 'points');
    
    this.pointsLayer.destroyChildren();
    this.drillPointObjects.clear();
    const points = this.patternData.drillLocations;

    points.forEach((point: DrillLocation, index: number) => {
      const canvasPos = this.convertToCanvasCoords(point.x, point.y);
      
      console.log(`Drawing point ${index + 1}/${points.length}:`, {
        id: point.id,
        original: { x: point.x, y: point.y },
        canvas: canvasPos
      });
      
      const pointGroup = new Konva.Group({
        x: canvasPos.x,
        y: canvasPos.y,
        pointId: point.id
      });

      // Check if this is the starting hole
      const isStarting = this.startingHoleId === point.id;
      
      // Make circles larger and more visible
      const circle = new Konva.Circle({
        radius: 12, // Increased from 8 to 12
        fill: isStarting ? '#4CAF50' : '#2196F3', // Green for starting hole
        stroke: isStarting ? '#2E7D32' : '#1976D2', // Darker green border for starting hole
        strokeWidth: isStarting ? 4 : 3, // Thicker border for starting hole
        pointId: point.id
      });

      const text = new Konva.Text({
        text: point.id,
        fontSize: 10, // Reduced to fit better in circle
        fontStyle: 'bold',
        fill: 'white',
        align: 'center',
        verticalAlign: 'middle',
        pointId: point.id
      });

      // Center the text properly within the circle
      text.offsetX(text.width() / 2);
      text.offsetY(text.height() / 2);

      pointGroup.add(circle);
      pointGroup.add(text);

      // Add starting hole indicator (play icon)
      if (isStarting) {
        const startIcon = new Konva.Path({
          data: 'M8 5v14l11-7z', // Play icon path
          fill: 'white',
          scale: { x: 0.8, y: 0.8 },
          offsetX: 12,
          offsetY: 12,
          x: 18,
          y: 12,
          pointId: point.id
        });
        pointGroup.add(startIcon);
      }
      this.pointsLayer.add(pointGroup);
      
      this.drillPointObjects.set(point.id, pointGroup);
    });

    console.log('✅ Added', points.length, 'drill points to layer');
    this.pointsLayer.draw();
    console.log('✅ Drill points layer drawn');
  }

  private convertToCanvasCoords(x: number, y: number): { x: number, y: number } {
    // Convert drill pattern coordinates to canvas coordinates
    // Use a reasonable scale that works well with typical drill pattern data
    const scale = 30; // 30px per unit - good balance for visibility
    const offsetX = 50; // Small offset from edges
    const offsetY = 50; // Small offset from edges
    
    const result = {
      x: offsetX + (x * scale),
      y: offsetY + (y * scale)
    };
    
    return result;
  }

  // Clean up Konva stage resources
  private cleanup(): void {
    if (this.stage) {
      this.stage.destroy();
    }
  }

  // Public methods for UI interaction
  toggleConnectionMode(): void {
    // If trying to enable connection mode, check if starting hole is selected
    if (!this.isConnectionMode && this.startingHoleId === null) {
      this.notification.showError('Please select a starting hole before creating connections. Right-click on a hole and select "Set as Starting Hole".');
      return;
    }
    
    this.isConnectionMode = !this.isConnectionMode;
    if (!this.isConnectionMode) {
      this.cancelConnection();
    }
    this.clearHighlights();
    
    // Update smart hints
    this.updateSmartHints();
    
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
    if (this.patternData && this.patternData.drillLocations.length > 0) {
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
    if (this.connections.length === 0) {
      console.log('No connections to clear');
      return;
    }

    console.log('🧹 Clearing all connections');
    
    // Clear from backend (delete all connections for this site)
    if (this.currentProjectId && this.currentSiteId) {
      // Delete each connection individually since we don't have a bulk delete API
      const deletePromises = this.connections.map(connection => 
        this.siteBlastingService.deleteBlastConnection(connection.id, this.currentProjectId, this.currentSiteId).toPromise()
      );
      
      Promise.all(deletePromises).then(() => {
        console.log('✅ All connections cleared from backend');
        this.notification.showSuccess('All connections cleared successfully');
      }).catch(error => {
        console.error('❌ Error clearing connections from backend:', error);
        this.notification.showError('Failed to clear some connections from database');
      });
    }
    
    // Clear from local state
    this.connections = [];
    this.updateFilteredConnections();
    
    // Clear visual representations
    this.connectionObjects.forEach(obj => obj.destroy());
    this.connectionObjects.clear();
    this.connectionsLayer.draw();
    
    // Reset sequence counter
    this.currentSequence = 1;
    
    this.cdr.markForCheck();
  }

  toggleConnectionsVisibility(): void {
    this.showConnections = !this.showConnections;
    this.connectionsLayer.visible(this.showConnections);
    this.connectionsLayer.draw();
    this.cdr.detectChanges();
  }

  exportBlastSequence(): void {
    if (!this.patternData) return;
    
    // Convert UnifiedDrillDataService PatternData to local PatternData format for export
    const localPatternData = {
      drillPoints: this.patternData.drillLocations.map((location: DrillLocation) => ({
        id: location.id,
        x: location.x,
        y: location.y,
        depth: location.depth,
        spacing: location.spacing,
        burden: location.burden,
        stemming: location.stemming || 2.0,
        subDrill: location.subDrill || 0.5
      })),
      settings: {
        spacing: this.patternData.settings.spacing,
        burden: this.patternData.settings.burden,
        depth: this.patternData.settings.depth
      }
    };
    
    const blastSequenceData: BlastSequenceData = {
      patternData: localPatternData,
      connections: [...this.connections],
      detonators: [...this.detonators],
      metadata: {
        exportedAt: new Date().toISOString(),
        version: '1.0',
        totalSequenceTime: Math.max(...this.connections.map(c => c.delay), 0)
      }
    };
    
    // Export functionality - currently not implemented
    console.log('📤 Blast sequence data ready for export:', blastSequenceData);
    this.notification.showSuccess('Blast sequence data prepared for export');
  }

  backToPatternCreator(): void {
    console.log('🔄 Navigating back to pattern creator');
    this.router.navigate(['/blasting-engineer/project-management', this.currentProjectId, 'sites', this.currentSiteId, 'pattern-creator']);
  }

  onSaveSequence(): void {
    if (!this.connections || this.connections.length === 0) {
      this.notification.showError('No connections to save. Create some blast connections first.');
      return;
    }

    console.log('💾 Saving sequence with', this.connections.length, 'connections');
    
    // Show loading state
    this.isSaved = false;
    
    // All connections are already saved to backend individually
    // Just show success message
    this.isSaved = true;
    this.notification.showSuccess(`Blast sequence saved successfully with ${this.connections.length} connections`);
    
    // Clear the saved flag after a delay
    if (this.saveTimeout) {
      clearTimeout(this.saveTimeout);
    }
    
    this.saveTimeout = setTimeout(() => {
      this.isSaved = false;
      this.cdr.markForCheck();
    }, 2000);
    
    this.cdr.markForCheck();
  }

  goToSimulator(): void {
    // Check if we have any connections first
    if (!this.connections || this.connections.length === 0) {
      this.notification.showError('No blast sequence available for simulation. Create at least one connection first.');
      return;
    }

    // Check for disconnected drill points
    const disconnectedPoints = this.getDisconnectedDrillPoints();
    
    if (disconnectedPoints.length > 0) {
      // Use a more user-friendly confirmation approach
      this.showDisconnectedPointsDialog(disconnectedPoints);
      return;
    }

    console.log('🚀 Navigating to simulator with', this.connections.length, 'connections');
    
    // Navigate to simulator
    this.router.navigate(['/blasting-engineer/project-management', this.currentProjectId, 'sites', this.currentSiteId, 'simulator']);
  }

  private showDisconnectedPointsDialog(disconnectedPoints: DrillLocation[]): void {
    const message = `⚠️ Warning: ${disconnectedPoints.length} drill point(s) are not connected to any blast sequence:\n\n` +
      `${disconnectedPoints.slice(0, 5).map((p: DrillLocation) => `• ${p.id}`).join('\n')}` +
      `${disconnectedPoints.length > 5 ? `\n• ... and ${disconnectedPoints.length - 5} more` : ''}\n\n` +
      `These points will not participate in the blast simulation.\n\n` +
      `Do you really want to proceed to the simulator?`;
    
    if (confirm(message)) {
      console.log('🚀 User confirmed navigation with disconnected points. Navigating to simulator with', this.connections.length, 'connections');
      this.router.navigate(['/blasting-engineer/project-management', this.currentProjectId, 'sites', this.currentSiteId, 'simulator']);
    } else {
              this.notification.showError('Simulation cancelled. You can add connections to include all drill points.');
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
    if (!this.isInitialized || !this.stage) {
      console.log('⚠️ Canvas not initialized, skipping redraw');
      return;
    }
    
    console.log('🔄 Redrawing all canvas layers');
    
    // Clear and redraw grid
    this.drawGrid();
    
    // Clear and redraw drill points
    this.drawDrillPoints();
    
    // Clear and redraw connections
    this.redrawConnections();
    
    // Ensure all layers are visible and drawn
    this.gridLayer.visible(true);
    this.pointsLayer.visible(true);
    this.connectionsLayer.visible(this.showConnections);
    
    this.gridLayer.draw();
    this.pointsLayer.draw();
    this.connectionsLayer.draw();
    
    // Re-center the view
    setTimeout(() => {
      this.centerView();
    }, 100);
    
    console.log('✅ All layers redrawn');
  }

  // Template helper methods
  trackConnection(index: number, connection: BlastConnection): string {
    return connection.id;
  }

  getConnectorTypeName(type: ConnectorType): string {
    switch (type) {
      case ConnectorType.DetonatingCord: return 'Detonating Cord';
      case ConnectorType.Connectors: return 'Connectors';
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
      case ConnectorType.DetonatingCord:
        return this.detonatingCordDelays;
      case ConnectorType.Connectors:
        return this.connectorsDelays;
      default:
        return this.detonatingCordDelays;
    }
  }

  // Set delay from predefined options
  setDelayFromOption(delay: number): void {
    this.currentDelay = delay;
    this.updateSmartHints();
    this.saveUserPreferences();
  }

  // Check if user can create connections
  canCreateConnections(): boolean {
    return this.currentDelay !== null && this.startingHoleId !== null;
  }

  // Get drill points that are not connected to any blast sequence
  private getDisconnectedDrillPoints(): DrillLocation[] {
    if (!this.patternData || !this.patternData.drillLocations) {
      return [];
    }

    const connectedPointIds = new Set<string>();
    
    // Add all connected drill point IDs to the set
    this.connections.forEach(connection => {
      connectedPointIds.add(connection.point1DrillPointId);
      connectedPointIds.add(connection.point2DrillPointId);
    });

    // Return drill points that are not in the connected set
    return this.patternData.drillLocations.filter(point => !connectedPointIds.has(point.id));
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
    if (!connection.id) {
      console.warn('Cannot delete connection without ID');
      return;
    }

    // Delete from backend first
    this.siteBlastingService.deleteBlastConnection(connection.id, this.currentProjectId, this.currentSiteId)
      .subscribe({
        next: () => {
          console.log('✅ Connection deleted from backend');
          
          // Remove from local connections
          this.connections = this.connections.filter(c => c.id !== connection.id);
          
          // Renumber sequences
          this.renumberSequences();
          
          // Update filtered connections
          this.updateFilteredConnections();
          
          // Redraw connections
          this.redrawConnections();
          
          this.notification.showSuccess('Connection deleted successfully');
          this.cdr.markForCheck();
        },
        error: (error) => {
          console.error('❌ Failed to delete connection from backend:', error);
          this.notification.showError('Failed to delete connection from database');
        }
      });
  }

  private renumberSequences(): void {
    this.connections.forEach((connection, index) => {
      connection.sequence = index + 1;
    });
    this.currentSequence = this.connections.length + 1;
    
    // Redraw connections with new sequence numbers
    this.redrawConnections();
  }

  private updateCurrentSequenceFromExistingConnections(): void {
    if (this.connections && this.connections.length > 0) {
      // Find the highest sequence number from existing connections
      const maxSequence = Math.max(...this.connections.map(conn => conn.sequence || 0));
      // Set currentSequence to be the next number after the highest existing sequence
      this.currentSequence = maxSequence + 1;
      console.log(`🔢 Updated currentSequence to ${this.currentSequence} based on ${this.connections.length} existing connections (max sequence: ${maxSequence})`);
    } else {
      // No existing connections, start from 1
      this.currentSequence = 1;
      console.log('🔢 No existing connections, currentSequence set to 1');
    }
  }

  private redrawConnections(): void {
    this.connectionObjects.forEach(obj => obj.destroy());
    this.connectionObjects.clear();
    
    this.connections.forEach(connection => {
      this.drawConnection(connection);
    });
    
    this.connectionsLayer.draw();
  }

  // Method to ensure connection has proper hidden points structure
  private ensureConnectionHasHiddenPoints(connection: BlastConnection): BlastConnection {
    // Since we're using the site-blasting.model.ts BlastConnection interface,
    // we don't need to add startPoint/endPoint properties
    // The connection visualization will use the drill point coordinates directly
    return connection;
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

  // Filter and sort connections based on search term and sort criteria
  private updateFilteredConnections(): void {
    let filtered = [...this.connections];

    // Apply search filter
    if (this.searchTerm) {
      const searchLower = this.searchTerm.toLowerCase();
      filtered = filtered.filter(connection => 
        connection.point1DrillPointId.toLowerCase().includes(searchLower) ||
        connection.point2DrillPointId.toLowerCase().includes(searchLower) ||
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
      from: connection.point1DrillPointId,
      to: connection.point2DrillPointId,
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

  debugRefreshCanvas(): void {
    console.log('🔧 Debug: Refreshing canvas...');
    
    if (!this.isInitialized || !this.stage) {
      console.log('❌ Canvas not initialized, reinitializing...');
      this.initializeCanvas();
      return;
    }
    
    console.log('📊 Canvas state:', {
      stageSize: { width: this.stage.width(), height: this.stage.height() },
      scale: this.scale,
      scalePercent: Math.round(this.scale * 100) + '%',
      pan: { x: this.panX, y: this.panY },
      patternData: this.patternData?.drillLocations?.length || 0,
      drillPointObjects: this.drillPointObjects.size,
      layers: {
        grid: this.gridLayer.visible(),
        points: this.pointsLayer.visible(),
        connections: this.connectionsLayer.visible()
      }
    });
    
    // Fix zoom if it's stuck at extreme values
    if (this.scale > 10 || this.scale < 0.05) {
      console.log('🔧 Fixing extreme zoom value:', this.scale);
      this.scale = 1.0;
      this.panX = 0;
      this.panY = 0;
    }
    
    // Force redraw everything
    this.redrawAll();
    
    this.notification.showSuccess('Canvas refreshed - zoom: ' + Math.round(this.scale * 100) + '%');
  }

  // Smart Hints System
  updateSmartHints(): void {
    const hint = this.getCurrentSmartHint();
    if (hint && !this.dismissedHints.has(hint.id)) {
      this.currentHint = hint;
    } else {
      this.currentHint = null;
    }
    this.updateProgressStatus();
    this.cdr.detectChanges();
  }

  getCurrentSmartHint(): SmartHint | null {
    // Priority-based hint system
    if (!this.startingHoleId) {
      return {
        id: 'select-starting-hole',
        title: 'Select Starting Hole',
        message: 'Right-click on any hole to set it as your starting point for the blast sequence.',
        icon: 'play_arrow',
        actionText: 'Show Me How',
        actionCallback: () => this.highlightCanvasArea(),
        priority: 'high',
        dismissible: true
      };
    }

    if (!this.currentDelay) {
      return {
        id: 'select-delay',
        title: 'Choose Delay Time',
        message: 'Select a delay time for your connections. This determines the timing between blasts.',
        icon: 'schedule',
        actionText: 'Set Default',
        actionCallback: () => this.setDefaultDelay(),
        priority: 'high',
        dismissible: true
      };
    }

    if (!this.isConnectionMode && this.canCreateConnections()) {
      return {
        id: 'enter-connection-mode',
        title: 'Ready to Connect',
        message: 'You\'re all set! Click "Connect" to start creating blast connections.',
        icon: 'link',
        actionText: 'Start Connecting',
        actionCallback: () => this.toggleConnectionMode(),
        priority: 'medium',
        dismissible: true
      };
    }

    if (this.isConnectionMode && this.connections.length === 0) {
      return {
        id: 'create-first-connection',
        title: 'Create Your First Connection',
        message: 'Click on the starting hole, then click on another hole to create your first connection.',
        icon: 'touch_app',
        priority: 'medium',
        dismissible: true
      };
    }

    if (this.connections.length > 0 && this.connections.length < 3) {
      return {
        id: 'continue-sequence',
        title: 'Build Your Sequence',
        message: 'Great start! Continue connecting holes to build your blast sequence.',
        icon: 'timeline',
        priority: 'low',
        dismissible: true
      };
    }

    return null;
  }

  dismissHint(hintId: string): void {
    this.dismissedHints.add(hintId);
    this.currentHint = null;
    this.updateSmartHints();
  }

  executeHintAction(): void {
    if (this.currentHint?.actionCallback) {
      this.currentHint.actionCallback();
    }
  }

  // Enhanced Status Methods
  updateProgressStatus(): void {
    this.completionPercentage = this.calculateCompletionPercentage();
    const nextAction = this.getNextAction();
    this.nextActionText = nextAction.text;
    this.nextActionIcon = nextAction.icon;
  }

  calculateCompletionPercentage(): number {
    let progress = 0;
    
    // Starting hole selected (25%)
    if (this.startingHoleId) progress += 25;
    
    // Delay selected (25%)
    if (this.currentDelay) progress += 25;
    
    // At least one connection (25%)
    if (this.connections.length > 0) progress += 25;
    
    // Multiple connections (25%)
    if (this.connections.length >= 3) progress += 25;
    
    return Math.min(progress, 100);
  }

  getNextAction(): { text: string, icon: string } {
    if (!this.startingHoleId) {
      return { text: 'Select a starting hole', icon: 'play_arrow' };
    }
    if (!this.currentDelay) {
      return { text: 'Choose delay time', icon: 'schedule' };
    }
    if (!this.isConnectionMode) {
      return { text: 'Enter connection mode', icon: 'link' };
    }
    if (this.connections.length === 0) {
      return { text: 'Create first connection', icon: 'touch_app' };
    }
    return { text: 'Continue building sequence', icon: 'timeline' };
  }

  // Helper Methods
  highlightCanvasArea(): void {
    // Add visual highlight to canvas
    const canvas = document.querySelector('.canvas-container');
    if (canvas) {
      canvas.classList.add('highlight-hint');
      setTimeout(() => canvas.classList.remove('highlight-hint'), 3000);
    }
  }

  setDefaultDelay(): void {
    const defaultDelays = this.getAvailableDelays();
    if (defaultDelays.length > 0) {
      this.currentDelay = defaultDelays[0];
      this.updateSmartHints();
    }
  }

  // Intelligent Defaults and Automation
  applyIntelligentDefaults(): void {
    // Load user preferences
    this.loadUserPreferences();
    
    // Set default connector type based on most common usage
    this.selectedConnectorType = ConnectorType.DetonatingCord;
    
    // Auto-set default delay if none selected
    if (!this.currentDelay) {
      const savedDelay = localStorage.getItem('preferred-delay');
      if (savedDelay) {
        this.currentDelay = parseInt(savedDelay);
      } else {
        // Use most common delay (25ms)
        this.currentDelay = 25;
      }
    }
    
    // Auto-enable helpful features for new users
    if (this.isFirstTimeUser) {
      this.showSmartHint = true;
      this.showConnectionsPanel = true;
    }
  }

  loadUserPreferences(): void {
    try {
      const preferences = localStorage.getItem('blast-sequence-preferences');
      if (preferences) {
        const prefs = JSON.parse(preferences);
        
        // Apply saved preferences
        if (prefs.connectorType) {
          this.selectedConnectorType = prefs.connectorType;
        }
        if (prefs.defaultDelay) {
          this.currentDelay = prefs.defaultDelay;
        }
        if (prefs.showHints !== undefined) {
          this.showSmartHint = prefs.showHints;
        }
        if (prefs.showConnectionsPanel !== undefined) {
          this.showConnectionsPanel = prefs.showConnectionsPanel;
        }
      }
    } catch (error) {
      console.warn('Failed to load user preferences:', error);
    }
  }

  saveUserPreferences(): void {
    try {
      const preferences = {
        connectorType: this.selectedConnectorType,
        defaultDelay: this.currentDelay,
        showHints: this.showSmartHint,
        showConnectionsPanel: this.showConnectionsPanel,
        lastUsed: Date.now()
      };
      
      localStorage.setItem('blast-sequence-preferences', JSON.stringify(preferences));
      localStorage.setItem('preferred-delay', this.currentDelay?.toString() || '25');
    } catch (error) {
      console.warn('Failed to save user preferences:', error);
    }
  }

  // Auto-optimization features
  optimizeSequence(): void {
    if (this.connections.length < 2) return;
    
    // Auto-suggest optimal delays based on connection pattern
    const suggestedDelay = this.calculateOptimalDelay();
    if (suggestedDelay && suggestedDelay !== this.currentDelay) {
      this.notification.showSuccess(`Suggested delay: ${suggestedDelay}ms for better timing`);
    }
  }

  calculateOptimalDelay(): number | null {
    if (this.connections.length === 0) return null;
    
    // Simple optimization: suggest shorter delays for dense patterns
    const avgDistance = this.calculateAverageConnectionDistance();
    
    if (avgDistance < 5) {
      return 17; // Short delay for close connections
    } else if (avgDistance < 10) {
      return 25; // Medium delay
    } else {
      return 42; // Longer delay for spread out connections
    }
  }

  calculateAverageConnectionDistance(): number {
    if (this.connections.length === 0) return 0;
    
    let totalDistance = 0;
    for (const connection of this.connections) {
      const point1 = this.patternData?.drillLocations.find(p => p.id === connection.point1DrillPointId);
      const point2 = this.patternData?.drillLocations.find(p => p.id === connection.point2DrillPointId);
      
      if (point1 && point2) {
        const distance = Math.sqrt(
          Math.pow(point2.x - point1.x, 2) + Math.pow(point2.y - point1.y, 2)
        );
        totalDistance += distance;
      }
    }
    
    return totalDistance / this.connections.length;
  }

  // Smart suggestions
  suggestNextConnection(): DrillPoint | null {
    if (!this.selectedFromHole || this.connections.length === 0) return null;
    
    // Find the closest unconnected hole
    const connectedHoleIds = new Set();
    this.connections.forEach(conn => {
      connectedHoleIds.add(conn.point1DrillPointId);
      connectedHoleIds.add(conn.point2DrillPointId);
    });
    
    const unconnectedHoles = this.patternData?.drillLocations.filter(
      hole => !connectedHoleIds.has(hole.id)
    ) || [];
    
    if (unconnectedHoles.length === 0) return null;
    
    // Find closest hole to current selection
    let closestHole: DrillPoint | null = null;
    let minDistance = Infinity;
    
    for (const hole of unconnectedHoles) {
      const distance = Math.sqrt(
        Math.pow(hole.x - this.selectedFromHole.x, 2) + 
        Math.pow(hole.y - this.selectedFromHole.y, 2)
      );
      
      if (distance < minDistance) {
        minDistance = distance;
        closestHole = {
          id: hole.id,
          x: hole.x,
          y: hole.y,
          depth: hole.depth,
          spacing: hole.spacing,
          burden: hole.burden,
          stemming: hole.stemming ?? 2.0, // Provide default value for required stemming property
          subDrill: hole.subDrill ?? 0.5
        };
      }
    }
    
    return closestHole;
  }

  // Wizard Methods
  startWizard(): void {
    this.showWizard = true;
    this.wizardStep = 1;
  }

  nextWizardStep(): void {
    if (this.wizardStep < this.wizardSteps.length) {
      this.wizardStep++;
    } else {
      this.completeWizard();
    }
  }

  previousWizardStep(): void {
    if (this.wizardStep > 1) {
      this.wizardStep--;
    }
  }

  completeWizard(): void {
    this.showWizard = false;
    this.isFirstTimeUser = false;
    localStorage.setItem('blast-sequence-wizard-completed', 'true');
  }

  skipWizard(): void {
    this.showWizard = false;
    this.isFirstTimeUser = false;
    localStorage.setItem('blast-sequence-wizard-completed', 'true');
  }

  getCurrentWizardStep() {
    return this.wizardSteps.find(step => step.id === this.wizardStep);
  }

  // Connection Preview Methods
  showConnectionPreview(from: DrillPoint, to: DrillPoint): void {
    this.previewConnection = { from, to };
    this.drawConnectionPreview();
  }

  hideConnectionPreview(): void {
    this.previewConnection = null;
    this.clearConnectionPreview();
  }

  private drawConnectionPreview(): void {
    if (!this.previewConnection || !this.connectionsLayer) return;
    
    this.clearConnectionPreview();
    
    const fromCoords = this.convertToCanvasCoords(
      this.previewConnection.from.x, 
      this.previewConnection.from.y
    );
    const toCoords = this.convertToCanvasCoords(
      this.previewConnection.to.x, 
      this.previewConnection.to.y
    );

    const previewLine = new Konva.Line({
      points: [fromCoords.x, fromCoords.y, toCoords.x, toCoords.y],
      stroke: '#667eea',
      strokeWidth: 3,
      dash: [10, 5],
      opacity: 0.7,
      name: 'connection-preview'
    });

    this.connectionsLayer.add(previewLine);
    this.connectionsLayer.batchDraw();
  }

  private clearConnectionPreview(): void {
    if (!this.connectionsLayer) return;
    
    const previewLines = this.connectionsLayer.find('.connection-preview');
    previewLines.forEach(line => line.destroy());
    this.connectionsLayer.batchDraw();
  }
}
