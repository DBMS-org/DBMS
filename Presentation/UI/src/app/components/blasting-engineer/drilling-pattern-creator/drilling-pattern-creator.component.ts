import { Component, ElementRef, ViewChild, AfterViewInit, ChangeDetectorRef, ChangeDetectionStrategy, HostListener, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of, throwError, Subject } from 'rxjs';
import { catchError, takeUntil } from 'rxjs/operators';
import Konva from 'konva';
import { MatDialog } from '@angular/material/dialog';

// Models and Constants
import { DrillPoint, PatternSettings } from './models/drill-point.model';
import { CANVAS_CONSTANTS } from './constants/canvas.constants';
import { Logger } from './utils/logger.util';
import { KonvaHelpers } from './utils/konva-helpers.util';

// Services
import { CanvasService } from './services/canvas.service';
import { GridService } from './services/grid.service';
import { RulerService } from './services/ruler.service';
import { DrillPointCanvasService } from './services/drill-point-canvas.service';
import { DrillPointService } from './services/drill-point.service';
import { ZoomService } from './services/zoom.service';
import { PatternDataService } from '../shared/pattern-data.service';
import { BlastSequenceDataService } from '../shared/services/blast-sequence-data.service';
import { SiteBlastingService } from '../../../core/services/site-blasting.service';
import { AuthService } from '../../../core/services/auth.service';
import { SiteService } from '../../../core/services/site.service';
import { NavigationController, WorkflowStepId } from '../shared/services/navigation-controller.service';
import { DrillDataService } from '../csv-upload/csv-upload.component';
import { DrillPointPatternService } from '../../../core/services/drill-point-pattern.service';
import { StateService } from '../../../core/services/state.service';
import { ConfirmDialogComponent, ConfirmDialogData } from '../../../shared/shared/components/confirm-dialog/confirm-dialog.component';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-drilling-pattern-creator',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './drilling-pattern-creator.component.html',
  styleUrls: ['./drilling-pattern-creator.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DrillingPatternCreatorComponent implements AfterViewInit, OnDestroy {
  @ViewChild('container') containerRef!: ElementRef<HTMLDivElement>;
  
  // Canvas objects
  private stage!: Konva.Stage;
  private gridLayer!: Konva.Layer;
  private rulerLayer!: Konva.Layer;
  private pointsLayer!: Konva.Layer;
  private gridGroup!: Konva.Group;
  private rulerGroup!: Konva.Group;
  private intersectionGroup!: Konva.Group;
  
  // Component state
  public drillPoints: DrillPoint[] = [];
  public selectedPoint: DrillPoint | null = null;
  public isHolePlacementMode = false;
  public isPreciseMode = false;
  public showInstructions = false;
  public cursorPosition: { x: number; y: number } | null = null;
  public duplicateAttemptMessage: string | null = null;
  public isSaved = false;
  
  // Internal state
  private offsetX = 0;
  private offsetY = 0;
  private gridAnimationFrame = 0;
  private isDragging = false;
  private draggedPoint: DrillPoint | null = null;
  private drillPointObjects: Map<string, Konva.Group> = new Map();
  private isInitialized = false;
  private isReadOnly = false;
  
  // Panning functionality
  private isPanning = false;
  private panStartX = 0;
  private panStartY = 0;
  private panOffsetX = 0;
  private panOffsetY = 0;

  // Timeouts - properly typed
  private resizeTimeout: number | null = null;
  private duplicateMessageTimeout: number | null = null;
  private saveTimeout: number | null = null;

  // Site context
  private currentProjectId!: number;
  private currentSiteId!: number;

  // Component lifecycle
  private destroy$ = new Subject<void>();

  // Constants for template
  public readonly ARIA_LABELS = CANVAS_CONSTANTS.ARIA_LABELS;
  public settings: PatternSettings = { ...CANVAS_CONSTANTS.DEFAULT_SETTINGS };

  constructor(
    private cdr: ChangeDetectorRef,
    private canvasService: CanvasService,
    private gridService: GridService,
    private rulerService: RulerService,
    private drillPointCanvasService: DrillPointCanvasService,
    private drillPointService: DrillPointService,
    private zoomService: ZoomService,
    private patternDataService: PatternDataService,
    private blastSequenceDataService: BlastSequenceDataService,
    private siteBlastingService: SiteBlastingService,
    private router: Router,
    private authService: AuthService,
    private siteService: SiteService,
    private navigationController: NavigationController,
    private drillDataService: DrillDataService,
    private drillPointPatternService: DrillPointPatternService,
    private stateService: StateService,
    private dialog: MatDialog,
    private notification: NotificationService
  ) {}

  formatValue(value: number): string {
    // Show decimal places only if they're not .00
    return value % 1 === 0 ? value.toString() : value.toFixed(2);
  }

  // Getter for accessing current scale through zoom service
  get scale(): number {
    return this.zoomService.getCurrentScale();
  }

  private getApprovalKey(): string {
    return `patternApproved_${this.currentProjectId}_${this.currentSiteId}`;
  }

  ngAfterViewInit(): void {
    // Initialize site context from route parameters
    this.initializeSiteContext();
    
    // Determine read-only mode for operator
    if (this.authService.isOperator()) {
      this.siteService.getSite(this.currentSiteId).subscribe({
        next: site => {
          if (!site.isPatternApproved) {
            this.notification.showError('Pattern not yet approved by engineer.');
            this.router.navigate(['/operator/dashboard']);
            return;
          }
          this.isReadOnly = true;
        },
        error: () => {
          this.notification.showError('Unable to verify pattern approval.');
          this.router.navigate(['/operator/dashboard']);
        }
      });
    }
    
    // Add a small delay to ensure the container is fully rendered
    // This fixes the issue where grid doesn't show on initial navigation
    setTimeout(() => {
      this.initializeCanvas();
    }, 0);
  }

  private initializeSiteContext(): void {
    // 1) Try to use global state first
    const { activeProjectId, activeSiteId } = this.stateService.currentState;
    if (activeProjectId && activeSiteId) {
      Logger.info('Pattern Creator - Using StateService context', { projectId: activeProjectId, siteId: activeSiteId });
      this.currentProjectId = activeProjectId;
      this.currentSiteId = activeSiteId;
      this.blastSequenceDataService.setSiteContext(activeProjectId, activeSiteId);
      this.loadExistingPatternData();
      this.loadBackendPatternData();
      return;
    }

    // 2) Fallback: extract from route and push into StateService (direct link / page refresh)
    const routeMatch = this.router.url.match(/project-management\/(\d+)\/sites\/(\d+)/);
    const projectId = routeMatch ? +routeMatch[1] : null;
    const siteId = routeMatch ? +routeMatch[2] : null;

    if (projectId && siteId) {
      Logger.info('Pattern Creator - Setting site context from route', { projectId, siteId });
      this.currentProjectId = projectId;
      this.currentSiteId = siteId;
      // Persist to global state for later screens
      this.stateService.setProjectId(projectId);
      this.stateService.setSiteId(siteId);
      this.blastSequenceDataService.setSiteContext(projectId, siteId);
      this.loadExistingPatternData();
      this.loadBackendPatternData();
    } else {
      Logger.warn('Pattern Creator - Could not determine site context; falling back to defaults');
      this.currentProjectId = 1;
      this.currentSiteId = 3;
      this.stateService.setProjectId(1);
      this.stateService.setSiteId(3);
      this.blastSequenceDataService.setSiteContext(1, 3);
      this.loadExistingPatternData();
      this.loadBackendPatternData();
    }
  }

  private loadExistingPatternData(): void {
    const existingPatternData = this.blastSequenceDataService.getPatternData();
    if (existingPatternData && existingPatternData.drillPoints) {
      // Load existing drill points
      this.drillPoints = [...existingPatternData.drillPoints];
      
      // Update the currentId to continue from the highest existing ID
      if (this.drillPoints.length > 0) {
        const highestId = Math.max(...this.drillPoints.map(point => {
          const numericPart = parseInt(point.id.replace('DH', ''));
          return isNaN(numericPart) ? 0 : numericPart;
        }));
        this.drillPointService.setCurrentId(highestId + 1);
      }
      
      // Load existing settings
      if (existingPatternData.settings) {
        this.settings = { ...existingPatternData.settings };
      }
      
      Logger.info('Loaded existing pattern data', {
        pointsCount: this.drillPoints.length,
        nextId: this.drillPointService.getCurrentId()
      });
      
      // Redraw points after loading (need to wait for canvas to be ready)
      setTimeout(() => {
        if (this.isInitialized) {
          this.drawDrillPoints();
        }
      }, 100);
      
      // Trigger UI update
      this.cdr.markForCheck();
    }
  }

  private loadBackendPatternData(): void {
    if (!this.currentProjectId || !this.currentSiteId) {
      return;
    }

    // Try to load saved drill patterns from backend first
    this.drillPointPatternService.getPattern(this.currentProjectId, this.currentSiteId)
      .subscribe({
        next: (pattern) => {
          if (pattern && pattern.drillPoints) {
            // Load the most recent pattern
            this.drillPoints = [...pattern.drillPoints];
            
            this.settings = {
              spacing: pattern.spacing,
              burden: pattern.burden,
              depth: pattern.depth
            };
            
            console.log('Loaded pattern from backend:', pattern.name, 'with', this.drillPoints.length, 'points');
            
            // Redraw if canvas is ready
            setTimeout(() => {
              if (this.isInitialized) {
                this.drawDrillPoints();
              }
            }, 100);
            
            this.cdr.markForCheck();
          } else {
            // No explicit drill-pattern record – fall back to generic workflow state
            this.loadWorkflowStateFromBackend();
          }
          // After attempting to load backend pattern, process any uploaded CSV data
          this.processIncomingDrillData();
        },
        error: (error) => {
          console.log('Error loading drill patterns:', error.message);
          // Attempt workflow state as secondary source if drill patterns failed/404
          this.loadWorkflowStateFromBackend();
          // Even if error, still process incoming CSV data
          this.processIncomingDrillData();
        }
      });
  }

  /**
   * Attempts to fetch previously saved workflow-state (pattern) data.
   * This is only called when no explicit DrillPattern exists, which
   * prevents the noisy 404 you were seeing on every navigation.
   */
  private loadWorkflowStateFromBackend(): void {
    this.siteBlastingService.getWorkflowState(this.currentProjectId, this.currentSiteId, 'pattern')
      .pipe(
        catchError(err => {
          if (err.message === 'Resource not found') {
            // Silently ignore missing workflow-state instead of logging errors.
            return of(null);
          }
          return throwError(() => err);
        })
      )
      .subscribe(workflowData => {
        if (!workflowData || !workflowData.jsonData) {
          return;
        }
        const patternData = workflowData.jsonData;
        if (patternData.drillPoints) {
          this.drillPoints = patternData.drillPoints;
          this.settings = patternData.settings || this.settings;

          console.log('Loaded workflow pattern state from backend with', this.drillPoints.length, 'points');

          // Redraw
          setTimeout(() => {
            if (this.isInitialized) {
              this.drawDrillPoints();
            }
          }, 100);
          this.cdr.markForCheck();
        }
      });
  }

  ngOnDestroy(): void {
    Logger.info('Component destroying');
    
    // Signal component destruction
    this.destroy$.next();
    this.destroy$.complete();
    
    // Mark as not initialized
    this.isInitialized = false;
    
    // Clean up timeouts
    this.clearAllTimeouts();
    
    // Clean up animation frame
    if (this.gridAnimationFrame) {
      cancelAnimationFrame(this.gridAnimationFrame);
    }
    
    // Clean up zoom events and stage
    if (this.stage) {
      this.zoomService.removeZoomEvents(this.stage);
      this.stage.destroy();
    }
    
    // Clean up grid cache
    this.gridService.clearGridCache();
  }

  private clearAllTimeouts(): void {
    const timeouts = [this.resizeTimeout, this.duplicateMessageTimeout, this.saveTimeout];
    timeouts.forEach(timeout => {
      if (timeout) clearTimeout(timeout);
    });
    this.resizeTimeout = null;
    this.duplicateMessageTimeout = null;
    this.saveTimeout = null;
  }

  private initializeCanvas(): void {
    if (this.isReadOnly) {
      // Initialize minimal canvas just for view
      this.initializeStage();
      // Disable pointer events to make read-only
      if (this.stage) {
        this.stage.listening(false);
      }
      return;
    }

    if (this.isInitialized) {
      Logger.warn('Canvas already initialized, skipping');
      return;
    }

    const container = this.containerRef.nativeElement;
    
    // Check if container has proper dimensions
    if (container.offsetWidth === 0 || container.offsetHeight === 0) {
      Logger.warn('Container not ready, retrying...');
      // Retry after a short delay if container isn't ready
              setTimeout(() => {
          this.initializeCanvas();
        }, CANVAS_CONSTANTS.TIMEOUTS.CANVAS_INIT_RETRY);
      return;
    }

    console.log('Initializing canvas with container size:', {
      width: container.offsetWidth,
      height: container.offsetHeight
    });

    // Clear grid cache to ensure fresh rendering after navigation
    this.gridService.clearGridCache();

    this.initializeStage();
    this.setupEventListeners();
    this.setupZoomService();
    
    // Mark as initialized before drawing
    this.isInitialized = true;
    
    this.drawGrid();
    this.drawRulers();
    this.drawDrillPoints();

    console.log('Initial canvas setup completed');
  }

  private initializeStage(): void {
    const container = this.containerRef.nativeElement;
    const width = container.offsetWidth;
    const height = container.offsetHeight;

    console.log('Creating Konva stage with dimensions:', { width, height });

    // Ensure minimum dimensions
    const finalWidth = Math.max(width, 300);
    const finalHeight = Math.max(height, 200);

    this.stage = new Konva.Stage({
      container: container,
      width: finalWidth,
      height: finalHeight
    });

    // Create layers
    this.gridLayer = new Konva.Layer();
    this.rulerLayer = new Konva.Layer();
    this.pointsLayer = new Konva.Layer();

    // Add layers to stage
    this.stage.add(this.rulerLayer);
    this.stage.add(this.gridLayer);
    this.stage.add(this.pointsLayer);

    // Set layer order: grid at bottom, points in middle, rulers on top
    this.gridLayer.moveToBottom();
    this.pointsLayer.moveToTop();
    this.rulerLayer.moveToTop();

    console.log('Stage initialized successfully');
  }

  private setupZoomService(): void {
    // Configure zoom service
    this.zoomService.configure({
      minScale: 0.1,
      maxScale: 10,
      zoomInFactor: 1.1,
      zoomOutFactor: 0.9
    });

    // Set up callbacks
    this.zoomService.setCallbacks({
      onZoomChange: (scale: number) => {
        // Trigger change detection when zoom changes
        this.cdr.detectChanges();
      },
      onRedraw: () => {
        // Only redraw if fully initialized
        if (this.isInitialized) {
          console.log('Zoom service requesting redraw');
          this.drawGrid();
          this.drawRulers();
          this.drawDrillPoints();
        }
      }
    });

    // Setup zoom events on the stage
    this.zoomService.setupZoomEvents(this.stage);
    console.log('Zoom service setup completed');
  }

  private setupEventListeners(): void {
    this.stage.on('mousemove', (e: Konva.KonvaEventObject<MouseEvent>) => {
      const pointer = this.stage.getPointerPosition();
      if (pointer) {
        // Handle panning
        if (this.isPanning && !this.isHolePlacementMode) {
          const deltaX = pointer.x - this.panStartX;
          const deltaY = pointer.y - this.panStartY;
          
          // Calculate new potential pan offsets
          const newPanOffsetX = this.panOffsetX + deltaX;
          const newPanOffsetY = this.panOffsetY + deltaY;
          
          // Store previous values to check if boundary was hit
          const prevPanOffsetX = this.panOffsetX;
          const prevPanOffsetY = this.panOffsetY;
          
          // Apply boundary constraints - don't allow panning beyond origin (0,0)
          // Prevent negative ruler values by limiting pan offsets
          this.panOffsetX = Math.min(0, newPanOffsetX);
          this.panOffsetY = Math.min(0, newPanOffsetY);
          
          // Check if we hit a boundary and provide visual feedback
          const hitBoundaryX = newPanOffsetX > 0 && this.panOffsetX === 0;
          const hitBoundaryY = newPanOffsetY > 0 && this.panOffsetY === 0;
          
          if (hitBoundaryX || hitBoundaryY) {
            // Briefly change cursor to indicate boundary hit
            this.updateCursor('not-allowed');
            setTimeout(() => {
              if (this.isPanning) {
                this.updateCursor('panning');
              }
            }, 100);
          }
          
          this.panStartX = pointer.x;
          this.panStartY = pointer.y;
          
          // Only redraw if pan offsets actually changed
          if (this.panOffsetX !== prevPanOffsetX || this.panOffsetY !== prevPanOffsetY) {
            this.drawGrid();
            this.drawRulers();
            this.drawDrillPoints();
          }
          
          return;
        }

        // Handle drill point dragging
        if (this.isDragging && this.draggedPoint) {
          const scale = this.zoomService.getCurrentScale();
          const worldX = (pointer.x - CANVAS_CONSTANTS.RULER_WIDTH - this.offsetX - this.panOffsetX) / (CANVAS_CONSTANTS.GRID_SIZE * scale);
          const worldY = (pointer.y - CANVAS_CONSTANTS.RULER_HEIGHT - this.offsetY - this.panOffsetY) / (CANVAS_CONSTANTS.GRID_SIZE * scale);
          
          if (this.isPreciseMode) {
            const snapX = Math.round(worldX / this.settings.spacing) * this.settings.spacing;
            const snapY = Math.round(worldY / this.settings.burden) * this.settings.burden;
            this.updateDrillPointPosition(this.draggedPoint, snapX, snapY);
          } else {
            this.updateDrillPointPosition(this.draggedPoint, worldX, worldY);
          }
          return;
        }

        // Update cursor position if in hole placement mode
        if (this.isHolePlacementMode) {
          const scale = this.zoomService.getCurrentScale();
          const worldX = (pointer.x - CANVAS_CONSTANTS.RULER_WIDTH - this.offsetX - this.panOffsetX) / (CANVAS_CONSTANTS.GRID_SIZE * scale);
          const worldY = (pointer.y - CANVAS_CONSTANTS.RULER_HEIGHT - this.offsetY - this.panOffsetY) / (CANVAS_CONSTANTS.GRID_SIZE * scale);
          
          this.cursorPosition = { x: worldX, y: worldY };
          this.cdr.detectChanges();
        }
      }
    });

    this.stage.on('mousedown', (e: Konva.KonvaEventObject<MouseEvent>) => {
      if (e.evt.button === 0) { // Left click
        if (this.isHolePlacementMode) {
          this.placeDrillHole(e);
        } else {
          // Start panning
          const pointer = this.stage.getPointerPosition();
          if (pointer) {
            this.isPanning = true;
            this.panStartX = pointer.x;
            this.panStartY = pointer.y;
            this.updateCursor('panning');
          }
        }
      }
    });

    this.stage.on('mouseup', (e: Konva.KonvaEventObject<MouseEvent>) => {
      this.isPanning = false;
      this.isDragging = false;
      this.draggedPoint = null;
      
      if (!this.isHolePlacementMode) {
        this.updateCursor('default');
      }
    });

    // Handle right-click context menu on stage
    this.stage.container().addEventListener('contextmenu', (e: Event) => {
      e.preventDefault();
    });

    // Handle right-click on drill points for context menu
    this.stage.on('contextmenu', (e: Konva.KonvaEventObject<MouseEvent>) => {
      e.evt.preventDefault();
      
      if (e.target !== this.stage) {
        // Check for pointId using type-safe helper
        const pointId = KonvaHelpers.getPointIdFromTarget(e.target);
        
        if (pointId) {
          const point = this.drillPoints.find(p => p.id === pointId);
          if (point) {
            // Select the point and show context action
            this.selectPoint(point);
            Logger.debug('Right-clicked on point for context actions', point.id);
          }
        }
      }
    });

    this.stage.on('dragstart', (e: Konva.KonvaEventObject<DragEvent>) => {
      if (e.target !== this.stage) {
        this.isDragging = true;
        this.isPanning = false;
        
        // Look for pointId using type-safe helper
        const pointId = KonvaHelpers.getPointIdFromTarget(e.target);
        
        this.draggedPoint = this.drillPoints.find(p => p.id === pointId) || null;
        Logger.debug('Drag started for point', { pointId, draggedPointId: this.draggedPoint?.id });
        
        // Auto-select the dragged point
        if (this.draggedPoint) {
          this.selectPoint(this.draggedPoint);
        }
      }
    });

    this.stage.on('dragmove', (e: Konva.KonvaEventObject<DragEvent>) => {
      if (this.draggedPoint) {
        const scale = this.zoomService.getCurrentScale();
        const position = e.target.position();
        const worldX = (position.x - CANVAS_CONSTANTS.RULER_WIDTH - this.offsetX - this.panOffsetX) / (CANVAS_CONSTANTS.GRID_SIZE * scale);
        const worldY = (position.y - CANVAS_CONSTANTS.RULER_HEIGHT - this.offsetY - this.panOffsetY) / (CANVAS_CONSTANTS.GRID_SIZE * scale);
        
        if (this.isPreciseMode) {
          const snapX = Math.round(worldX / this.settings.spacing) * this.settings.spacing;
          const snapY = Math.round(worldY / this.settings.burden) * this.settings.burden;
          
          // Update both the object and the data
          const scaledX = snapX * CANVAS_CONSTANTS.GRID_SIZE * scale + this.offsetX + this.panOffsetX + CANVAS_CONSTANTS.RULER_WIDTH;
          const scaledY = snapY * CANVAS_CONSTANTS.GRID_SIZE * scale + this.offsetY + this.panOffsetY + CANVAS_CONSTANTS.RULER_HEIGHT;
          e.target.position({ x: scaledX, y: scaledY });
          
          this.draggedPoint.x = snapX;
          this.draggedPoint.y = snapY;
        } else {
          this.draggedPoint.x = worldX;
          this.draggedPoint.y = worldY;
        }
      }
    });

    this.stage.on('click', (e: Konva.KonvaEventObject<MouseEvent>) => {
      // Handle click on drill points
      if (e.target !== this.stage) {
        // Check for pointId in target or parent group
        let pointId = e.target.attrs.pointId || (e.target.parent && (e.target.parent as any).pointId);
        
        // If target is a group, check its pointId
        if (!pointId && e.target.hasName && e.target.hasName('drill-point-group')) {
          pointId = (e.target as any).pointId;
        }
        
        // If target is a child of a drill point group, look for parent's pointId
        if (!pointId && e.target.parent) {
          pointId = (e.target.parent as any).pointId;
        }

        console.log('Click detected on:', e.target.getClassName(), 'pointId:', pointId);
        
        if (pointId) {
          const point = this.drillPoints.find(p => p.id === pointId);
          if (point) {
            console.log('Selecting point:', point.id);
            this.selectPoint(point);
            this.cdr.detectChanges();
            return;
          }
        }
      }
      
      // Clicked on empty space
      console.log('Clicked on empty space, deselecting');
      this.selectPoint(null);
      this.cdr.detectChanges();
    });
  }

  private drawGrid(): void {
    if (!this.isInitialized || !this.stage) {
      console.log('Skipping grid draw - not initialized or no stage');
      return;
    }

    console.log('drawGrid called - Stage dimensions:', {
      width: this.stage?.width(),
      height: this.stage?.height(),
      scale: this.scale,
      offsetX: this.offsetX,
      offsetY: this.offsetY,
      panOffsetX: this.panOffsetX,
      panOffsetY: this.panOffsetY,
      isPreciseMode: this.isPreciseMode
    });

    const cacheKey = `${this.scale}-${this.offsetX + this.panOffsetX}-${this.offsetY + this.panOffsetY}-${this.isPreciseMode}`;
    console.log('Grid cache key:', cacheKey);
    
    // Always destroy intersection group to ensure proper precise mode handling
    if (this.intersectionGroup) {
      console.log('Destroying old intersection group');
      this.intersectionGroup.destroy();
      this.intersectionGroup = null as any;
    }
    
    const isCached = this.canvasService.handleGridCache(cacheKey, this.gridGroup, this.gridLayer);
    
    if (!isCached) {
      console.log('Drawing new grid with cache key:', cacheKey);
      
      // Destroy old grid group only when creating new ones
      if (this.gridGroup) {
        console.log('Destroying old grid group');
        this.gridGroup.destroy();
      }
      
      this.gridGroup = this.canvasService.drawGrid(
        this.gridLayer,
        this.settings,
        this.scale,
        this.offsetX + this.panOffsetX,
        this.offsetY + this.panOffsetY,
        this.stage.width(),
        this.stage.height()
      );
      
      this.gridLayer.add(this.gridGroup);
      this.canvasService.updateGridCache(cacheKey, this.gridGroup);
      
      console.log('Grid created and added to layer');
    } else {
      console.log('Using cached grid');
    }
    
    // Always create intersection group (whether cached or not) based on current precise mode
    this.intersectionGroup = this.canvasService.drawGridIntersections(
      this.gridLayer,
      this.settings,
      this.scale,
      this.offsetX + this.panOffsetX,
      this.offsetY + this.panOffsetY,
      this.stage.width(),
      this.stage.height(),
      this.isPreciseMode
    );
    
    this.gridLayer.add(this.intersectionGroup);
    
    console.log('Grid layer children count:', this.gridLayer.children.length);
    console.log('Precise mode intersection points visible:', this.isPreciseMode);
    this.gridLayer.batchDraw();
    console.log('Grid layer batch draw completed');
  }

  private drawRulers(): void {
    if (this.rulerGroup) {
      this.rulerGroup.destroy();
    }

    this.rulerGroup = this.canvasService.drawRulers(
      this.rulerLayer,
      this.settings,
      this.scale,
      this.stage.width(),
      this.stage.height(),
      this.panOffsetX,
      this.panOffsetY
    );
    this.rulerLayer.add(this.rulerGroup);
    this.rulerLayer.batchDraw();
  }

  private drawDrillPoints(): void {
    console.log('drawDrillPoints called, points count:', this.drillPoints.length);
    
    this.drillPointObjects.forEach(group => {
      group.destroy();
    });
    this.drillPointObjects.clear();

    this.drillPoints.forEach((point, index) => {
      console.log(`Creating visual for point ${index}:`, point);
      
      const group = this.canvasService.createDrillPointObject(
        point,
        this.scale,
        this.offsetX + this.panOffsetX,
        this.offsetY + this.panOffsetY,
        this.isHolePlacementMode,
        point === this.selectedPoint
      );
      
      console.log(`Created group for point ${point.id} at position:`, { x: group.x(), y: group.y() });
      
      this.drillPointObjects.set(point.id, group);
      this.pointsLayer.add(group);
    });
    
    console.log('About to batch draw points layer');
    this.pointsLayer.batchDraw();
    console.log('Batch draw completed');
  }

  private placeDrillHole(e: Konva.KonvaEventObject<MouseEvent>): void {
    const pointer = this.stage.getPointerPosition();
    if (!pointer) return;

    const scale = this.zoomService.getCurrentScale();
    
    // Fix coordinate calculation to account for rulers and grid size
    const worldX = (pointer.x - CANVAS_CONSTANTS.RULER_WIDTH - this.offsetX - this.panOffsetX) / (CANVAS_CONSTANTS.GRID_SIZE * scale);
    const worldY = (pointer.y - CANVAS_CONSTANTS.RULER_HEIGHT - this.offsetY - this.panOffsetY) / (CANVAS_CONSTANTS.GRID_SIZE * scale);

    console.log('Click position:', { 
      pointerX: pointer.x, 
      pointerY: pointer.y, 
      scale, 
      panOffsetX: this.panOffsetX, 
      panOffsetY: this.panOffsetY,
      offsetX: this.offsetX,
      offsetY: this.offsetY,
      worldX, 
      worldY 
    });

    if (this.isPreciseMode) {
      const snapX = Math.round(worldX / this.settings.spacing) * this.settings.spacing;
      const snapY = Math.round(worldY / this.settings.burden) * this.settings.burden;
      console.log('Precise mode - snapped to:', { snapX, snapY });
      this.addDrillPoint(snapX, snapY);
    } else {
      console.log('Free mode - adding at:', { worldX, worldY });
      this.addDrillPoint(worldX, worldY);
    }
  }

  private addDrillPoint(x: number, y: number): void {
    console.log('addDrillPoint called with:', { x, y });

    if (!this.drillPointService.validateCoordinates(x, y)) {
      console.log('Invalid coordinates');
      return;
    }

    if (!this.drillPointService.validateUniqueCoordinates(x, y, this.drillPoints)) {
      // Show duplicate placement message
      this.duplicateAttemptMessage = `Hole already exists at (${this.formatValue(x)}, ${this.formatValue(y)})`;
      
      // Find the existing point and briefly highlight it
      const existingPoint = this.drillPoints.find(point => 
        Math.abs(point.x - x) < 0.01 && Math.abs(point.y - y) < 0.01
      );
      
      if (existingPoint) {
        this.highlightExistingPoint(existingPoint);
      }
      
      // Clear message after 3 seconds
      if (this.duplicateMessageTimeout) {
        clearTimeout(this.duplicateMessageTimeout);
      }
      this.duplicateMessageTimeout = setTimeout(() => {
        this.duplicateAttemptMessage = null;
        this.cdr.detectChanges();
      }, 3000);
      
      this.cdr.detectChanges();
      return;
    }

    if (!this.drillPointService.validateDrillPointCount(
      this.drillPoints.length,
      CANVAS_CONSTANTS.MAX_DRILL_POINTS
    )) {
      console.log('Max drill points reached');
      return;
    }

    const point = this.drillPointService.createDrillPoint(x, y, this.settings);
    console.log('Created drill point:', point);
    this.drillPoints.push(point);
    console.log('Total drill points:', this.drillPoints.length);
    this.drawDrillPoints();
  }

  private highlightExistingPoint(point: DrillPoint): void {
    // Temporarily select the existing point to highlight it
    const previousSelection = this.selectedPoint;
    this.selectedPoint = point;
    this.drawDrillPoints();
    
    // Flash the point by changing selection
    setTimeout(() => {
      this.selectedPoint = null;
      this.drawDrillPoints();
      setTimeout(() => {
        this.selectedPoint = point;
        this.drawDrillPoints();
        setTimeout(() => {
          this.selectedPoint = previousSelection;
          this.drawDrillPoints();
        }, 200);
      }, 200);
    }, 200);
  }

  private updateDrillPointPosition(point: DrillPoint, x: number, y: number): void {
    if (!this.drillPointService.validateCoordinates(x, y)) {
      return;
    }

    // Check for duplicates excluding the current point being moved
    const otherPoints = this.drillPoints.filter(p => p.id !== point.id);
    if (!this.drillPointService.validateUniqueCoordinates(x, y, otherPoints)) {
      // Show duplicate placement message
      this.duplicateAttemptMessage = `Cannot move hole to (${this.formatValue(x)}, ${this.formatValue(y)}) - position occupied`;
      
      // Find the existing point and briefly highlight it
      const existingPoint = otherPoints.find(p => 
        Math.abs(p.x - x) < 0.01 && Math.abs(p.y - y) < 0.01
      );
      
      if (existingPoint) {
        this.highlightExistingPoint(existingPoint);
      }
      
      // Clear message after 3 seconds
      if (this.duplicateMessageTimeout) {
        clearTimeout(this.duplicateMessageTimeout);
      }
      this.duplicateMessageTimeout = setTimeout(() => {
        this.duplicateAttemptMessage = null;
        this.cdr.detectChanges();
      }, 3000);
      
      this.cdr.detectChanges();
      return;
    }

    point.x = x;
    point.y = y;
    this.drawDrillPoints();
  }

  private selectPoint(point: DrillPoint | null): void {
    const previousSelection = this.selectedPoint;
    this.selectedPoint = this.drillPointService.selectPoint(point, this.drillPoints);
    
    // Only redraw if selection actually changed
    if (previousSelection !== this.selectedPoint) {
      this.drawDrillPoints();
      this.cdr.markForCheck();
    }
  }

  toggleHolePlacementMode(): void {
    if (this.isReadOnly) return;
    this.toggleMode('holePlacement');
  }

  togglePreciseMode(): void {
    if (this.isReadOnly) return;
    this.toggleMode('precise');
  }

  private toggleMode(mode: 'holePlacement' | 'precise'): void {
    const modeProperty = mode === 'holePlacement' ? 'isHolePlacementMode' : 'isPreciseMode';
    const previousValue = this[modeProperty];
    this[modeProperty] = !this[modeProperty];
    
    console.log(`Toggle ${mode} mode: ${previousValue} -> ${this[modeProperty]}`);
    
    // Set appropriate cursor based on mode
    if (mode === 'holePlacement') {
      this.canvasService.setCanvasCursor(this.stage, this.isHolePlacementMode ? 'crosshair' : 'default');
      this.canvasService.updatePointSelectability(this.drillPointObjects, this.isHolePlacementMode);
    } else if (mode === 'precise') {
      // Clear grid cache when precise mode changes to force redraw
      console.log('Clearing grid cache due to precise mode change');
      this.gridService.clearGridCache();
      
      // Redraw grid to show/hide intersection points
      this.drawGrid();
    }
    
    this.drawDrillPoints();
  }

  onClearAll(): void {
    if (this.isReadOnly) return;
    this.drillPoints = this.drillPointService.clearPoints();
    this.selectPoint(null);
    this.drawDrillPoints();
  }

  onSavePattern(): void {
    if (this.isReadOnly) return;
    if (this.drillPoints.length === 0) {
      console.warn('No drill points to save');
      return;
    }

    if (!this.currentProjectId || !this.currentSiteId) {
      console.error('Missing project or site context for saving');
      return;
    }

    const patternData = this.drillPointService.getPatternData(this.drillPoints, this.settings);
    const currentDate = new Date();
    const patternName = `Drill Pattern ${currentDate.toLocaleDateString()} ${currentDate.toLocaleTimeString()}`;

    // Save to backend as a drill pattern
    const saveRequest = {
      projectId: this.currentProjectId,
      siteId: this.currentSiteId,
      name: patternName,
      description: `Drill pattern created with ${this.drillPoints.length} holes`,
      spacing: this.settings.spacing,
      burden: this.settings.burden,
      depth: this.settings.depth,
      drillPoints: this.drillPoints
    };

    this.drillPointPatternService.savePattern(saveRequest).subscribe({
      next: (savedPattern) => {
        console.log('Pattern saved to backend successfully:', savedPattern);
        
        // Update local state
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
      },
      error: (error) => {
        console.error('Error saving pattern to backend:', error);
        // Fallback to local storage
        this.saveToLocalStorage(patternData);
      }
    });

    // Also update local data service for immediate use
    this.blastSequenceDataService.setPatternData(patternData, false);
    this.blastSequenceDataService.savePatternData();
  }

  private saveToLocalStorage(patternData: any): void {
    // Fallback method for local storage when backend is unavailable
    this.blastSequenceDataService.setPatternData(patternData, false);
    this.blastSequenceDataService.savePatternData();

    this.isSaved = true;
    this.cdr.markForCheck();

    if (this.saveTimeout) {
      clearTimeout(this.saveTimeout);
    }

    this.saveTimeout = setTimeout(() => {
      this.isSaved = false;
      this.cdr.markForCheck();
    }, 3000);

    console.log('Pattern saved to local storage (backend unavailable)');
  }


  onExportToBlastDesigner(): void {
    console.log('Export to Blast Designer button clicked');
    console.log('Current project ID:', this.currentProjectId);
    console.log('Current site ID:', this.currentSiteId);
    console.log('Drill points count:', this.drillPoints.length);
    
    if (this.drillPoints.length === 0) {
      console.warn('No drill points to export');
      return;
    }
    
    const patternData = this.drillPointService.getPatternData(this.drillPoints, this.settings);
    
    // Update data service (in memory)
    this.blastSequenceDataService.setPatternData(patternData, false);
    
    // Save pattern data when navigating to next step
    this.blastSequenceDataService.savePatternData();
    
    // Export to both services for backward compatibility and new workflow
    this.patternDataService.setCurrentPattern(patternData);
    this.blastSequenceDataService.setPatternData(patternData, true); // Auto-save on navigation
    
    // Navigate to blast sequence designer with context (force navigation to bypass workflow checks)
    const targetRoute = `/blasting-engineer/project-management/${this.currentProjectId}/sites/${this.currentSiteId}/sequence-designer`;
    console.log('Attempting navigation to:', targetRoute);
    
    // Try direct router navigation first
    this.router.navigate([targetRoute]).then(success => {
      console.log('Direct navigation success:', success);
    }).catch(error => {
      console.error('Direct navigation error:', error);
      // Fallback to navigation controller
      this.navigationController.navigateToStepWithContext(WorkflowStepId.SEQUENCE, this.currentProjectId, this.currentSiteId, true);
    });
  }

  onDeletePoint(): void {
    if (this.isReadOnly) return;
    if (this.selectedPoint) {
      this.drillPoints = this.drillPointService.removePoint(this.selectedPoint, this.drillPoints);
      this.selectPoint(null);
    }
  }

  onSpacingChange(value: number): void {
    if (this.isReadOnly) return;
    this.settings.spacing = value;
    this.updatePattern();
  }

  onBurdenChange(value: number): void {
    if (this.isReadOnly) return;
    this.settings.burden = value;
    this.updatePattern();
  }

  onDepthChange(value: number): void {
    if (this.isReadOnly) return;
    this.settings.depth = value;
    // Depth doesn't affect visual display, just update existing points
    this.drillPoints.forEach(point => {
      point.depth = value;
    });
  }

  private updatePattern(): void {
    // Clear grid cache since grid spacing has changed
    this.gridService.clearGridCache();
    
    // Update existing drill points with new settings
    this.drillPoints.forEach(point => {
      point.spacing = this.settings.spacing;
      point.burden = this.settings.burden;
    });
    
    // Redraw everything
    this.drawGrid();
    this.drawRulers();
    this.drawDrillPoints();
    this.cdr.detectChanges();
  }

  @HostListener('window:resize')
  onResize(): void {
    if (this.resizeTimeout) {
      clearTimeout(this.resizeTimeout);
    }
    this.resizeTimeout = setTimeout(() => {
      const container = this.containerRef.nativeElement;
      this.stage.width(container.offsetWidth);
      this.stage.height(container.offsetHeight);
      this.drawGrid();
      this.drawRulers();
      this.drawDrillPoints();
    }, 250);
  }

  @HostListener('window:keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): void {
    // Only handle keyboard events when component is active
    if (!this.containerRef?.nativeElement?.contains(document.activeElement) && 
        document.activeElement !== document.body) {
      return;
    }

    switch(event.key) {
      case 'Delete':
      case 'Backspace':
        if (this.selectedPoint) {
          event.preventDefault();
          this.onDeletePoint();
        }
        break;
      case 'Escape':
        event.preventDefault();
        this.selectPoint(null);
        if (this.isHolePlacementMode) {
          this.toggleHolePlacementMode();
        }
        break;
      case ' ':
        if (event.target === document.body) {
          event.preventDefault();
          this.toggleHolePlacementMode();
        }
        break;
      case 'p':
      case 'P':
        if (event.ctrlKey || event.metaKey) {
          event.preventDefault();
          this.togglePreciseMode();
        }
        break;
      case 'ArrowUp':
        if (this.selectedPoint) {
          event.preventDefault();
          this.moveSelectedPoint(0, this.isPreciseMode ? -this.settings.burden : -0.1);
        }
        break;
      case 'ArrowDown':
        if (this.selectedPoint) {
          event.preventDefault();
          this.moveSelectedPoint(0, this.isPreciseMode ? this.settings.burden : 0.1);
        }
        break;
      case 'ArrowLeft':
        if (this.selectedPoint) {
          event.preventDefault();
          this.moveSelectedPoint(this.isPreciseMode ? -this.settings.spacing : -0.1, 0);
        }
        break;
      case 'ArrowRight':
        if (this.selectedPoint) {
          event.preventDefault();
          this.moveSelectedPoint(this.isPreciseMode ? this.settings.spacing : 0.1, 0);
        }
        break;
    }
  }

  private updateCursor(cursor: string): void {
    this.canvasService.setCanvasCursor(this.stage, cursor);
  }

  private moveSelectedPoint(deltaX: number, deltaY: number): void {
    if (!this.selectedPoint) return;
    
    const newX = this.selectedPoint.x + deltaX;
    const newY = this.selectedPoint.y + deltaY;
    
    this.updateDrillPointPosition(this.selectedPoint, newX, newY);
  }

  private async processIncomingDrillData(): Promise<void> {
    // Retrieve any drill data uploaded in CSV Upload component
    const uploadedDrillData = this.drillDataService.getDrillData();
    if (!uploadedDrillData || uploadedDrillData.length === 0) {
      return; // No new data to process
    }

    const hasExistingPattern = this.drillPoints && this.drillPoints.length > 0;
    let proceedWithImport = true;

    // If there is already a pattern, confirm with user
    if (hasExistingPattern) {
      const dialogResult = await this.dialog.open<ConfirmDialogComponent, ConfirmDialogData, boolean>(ConfirmDialogComponent, {
        width: '340px',
        data: {
          title: 'Overwrite Pattern?',
          message: 'Existing pattern data detected. Importing new CSV will overwrite current data. Continue?',
          confirmText: 'Import',
          cancelText: 'Cancel'
        }
      }).afterClosed().toPromise();

      proceedWithImport = dialogResult === true;
    }

    if (!proceedWithImport) {
      // User chose to keep existing pattern – clear the staged drill data to avoid repeated prompts
      this.drillDataService.clearDrillData();
      return;
    }

    // User confirmed (or no existing pattern). Clean up old pattern data if any
    if (hasExistingPattern && this.currentProjectId && this.currentSiteId) {
      this.blastSequenceDataService.cleanupPatternData(this.currentProjectId, this.currentSiteId);
    }

    const defaultDepthSetting = this.settings.depth ?? CANVAS_CONSTANTS.DEFAULT_SETTINGS.depth;

    // First map raw drill holes to coordinates + depth (spacing/burden will be filled after grid calculation)
    const initialPoints: DrillPoint[] = uploadedDrillData.map((hole, index) => {
      const depthValue = (hole as any).depth ?? (hole as any).length ?? defaultDepthSetting;
      return {
        x: Number((hole as any).easting?.toFixed(2) ?? 0),
        y: Number((hole as any).northing?.toFixed(2) ?? 0),
        id: hole.id ? hole.id.toString() : `DH${index + 1}`,
        depth: depthValue,
        spacing: 0,
        burden: 0
      } as DrillPoint;
    });

    // Auto-detect pitch
    const { spacing: autoSpacing, burden: autoBurden } = this.drillPointService.calculateGridPitch(initialPoints);

    // Apply pitch to settings and to each point
    this.settings.spacing = autoSpacing;
    this.settings.burden = autoBurden;

    // Anchor the pattern so the smallest easting/northing starts at 0,0 – keeps
    // canvas coordinates compact instead of hundreds of thousands of pixels.
    const minX = Math.min(...initialPoints.map(p => p.x));
    const minY = Math.min(...initialPoints.map(p => p.y));

    const convertedPoints: DrillPoint[] = initialPoints.map(p => ({
      ...p,
      x: +(p.x - minX).toFixed(2),
      y: +(p.y - minY).toFixed(2),
      spacing: autoSpacing,
      burden: autoBurden
    }));

    // Replace current drill points and refresh canvas
    this.drillPoints = convertedPoints;
    // Reset hole numbering in drillPointService so it continues after imported IDs
    const highestId = Math.max(
      ...this.drillPoints.map(dp => {
        const numericPart = parseInt(dp.id.replace(/\D/g, ''));
        return isNaN(numericPart) ? 0 : numericPart;
      })
    );
    this.drillPointService.setCurrentId(highestId + 1);

    // Update settings depth to average of holes if available
    if (convertedPoints.length > 0) {
      const avgDepth = convertedPoints.reduce((sum, p) => sum + (p.depth || defaultDepthSetting), 0) / convertedPoints.length;
      this.settings.depth = Number(avgDepth.toFixed(2));
    }

    // Persist pattern data in shared service
    const patternData = this.drillPointService.getPatternData(this.drillPoints, this.settings);
    this.blastSequenceDataService.setPatternData(patternData, /*autoSave*/ true);

    // Redraw if canvas initialized
    setTimeout(() => {
      if (this.isInitialized) {
        this.drawDrillPoints();
      }
      // Trigger change detection
      this.cdr.detectChanges();
    }, 100);

    // Clear the staged drill data to prevent re-import on navigation history
    this.drillDataService.clearDrillData();

    this.notification.showSuccess('Pattern imported successfully!');
  }
}