import { 
  Component, 
  OnInit, 
  OnDestroy, 
  ChangeDetectionStrategy, 
  ChangeDetectorRef,
  inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subject, takeUntil, combineLatest } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';

// Child Components
import { PatternToolbarComponent } from './components/pattern-toolbar/pattern-toolbar.component';
import { PatternCanvasComponent } from './components/pattern-canvas/pattern-canvas.component';
import { PatternStatusBarComponent } from './components/pattern-status-bar/pattern-status-bar.component';
import { PatternInstructionsComponent } from './components/pattern-instructions/pattern-instructions.component';

// Models and Events
import { DrillPoint, PatternSettings } from './models/drill-point.model';
import { 
  ModeToggleEvent, 
  PointActionEvent, 
  PatternActionEvent,
  PlacePointEvent,
  MovePointEvent,
  CanvasState,
  UIState
} from './models/pattern-state.model';
import { 
  DrillLocation, 
  PatternSettings as UnifiedPatternSettings 
} from '../../../core/models/drilling.model';

// Services
import { PatternStateService } from './services/pattern-state.service';
import { DrillPointService } from './services/drill-point.service';
import { UnifiedDrillDataService } from '../../../core/services/unified-drill-data.service';
import { NotificationService } from '../../../core/services/notification.service';
import { ErrorHandlingService } from './services/error-handling.service';

// Components for dialogs
import { 
  DepthEditorTableComponent, 
  DepthChangeEvent, 
  BulkDepthChangeEvent, 
  ApplyGlobalDepthEvent 
} from './components/depth-editor-table/depth-editor-table.component';

// Utils
import { Logger } from './utils/logger.util';

/**
 * Main container component for the drilling pattern creator
 * 
 * This component serves as a coordinator/container that:
 * - Manages overall component state through PatternStateService
 * - Coordinates child components (toolbar, canvas, status bar, instructions)
 * - Handles route parameters and site context
 * - Manages component lifecycle and cleanup
 * - Delegates business logic to appropriate services
 * 
 * The component is kept under 300 lines by delegating all business logic
 * to services and presentation logic to child components.
 */
@Component({
  selector: 'app-drilling-pattern-creator',
  standalone: true,
  imports: [
    CommonModule,
    PatternToolbarComponent,
    PatternCanvasComponent,
    PatternStatusBarComponent,
    PatternInstructionsComponent
  ],
  templateUrl: './drilling-pattern-creator.component.html',
  styleUrls: ['./drilling-pattern-creator.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DrillingPatternCreatorComponent implements OnInit, OnDestroy {
  
  // Injected services
  private readonly patternState = inject(PatternStateService);
  private readonly drillPointService = inject(DrillPointService);
  private readonly unifiedDrillDataService = inject(UnifiedDrillDataService);
  private readonly notificationService = inject(NotificationService);
  private readonly errorHandler = inject(ErrorHandlingService);
  private readonly router = inject(Router);
  private readonly dialog = inject(MatDialog);
  private readonly cdr = inject(ChangeDetectorRef);

  // Component lifecycle
  private readonly destroy$ = new Subject<void>();

  // Site context
  private currentProjectId!: number;
  private currentSiteId!: number;

  // State observables for template
  public readonly state$ = this.patternState.state$;
  public readonly drillPoints$ = this.patternState.drillPoints$;
  public readonly selectedPoint$ = this.patternState.selectedPoint$;
  public readonly settings$ = this.patternState.settings$;
  public readonly isHolePlacementMode$ = this.patternState.isHolePlacementMode$;
  public readonly isPreciseMode$ = this.patternState.isPreciseMode$;
  public readonly isSaved$ = this.patternState.isSaved$;
  public readonly isFullscreen$ = this.patternState.isFullscreen$;

  // Current state snapshots for child components
  public currentState: any = {};
  public canvasState: CanvasState = {
    scale: 1,
    panOffsetX: 0,
    panOffsetY: 0,
    isInitialized: false,
    isDragging: false,
    isPanning: false
  };
  public uiState: UIState = {
    isHolePlacementMode: false,
    isPreciseMode: false,
    isFullscreen: false,
    showInstructions: false,
    isSaved: false,
    duplicateMessage: null,
    cursorPosition: null
  };

  ngOnInit(): void {
    this.initializeSiteContext();
    this.setupStateSubscriptions();
    this.loadPatternData();
  }

  ngOnDestroy(): void {
    Logger.info('DrillingPatternCreatorComponent destroying');
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Initialize site context from route parameters
   */
  private initializeSiteContext(): void {
    const routeMatch = this.router.url.match(/project-management\/(\d+)\/sites\/(\d+)/);
    this.currentProjectId = routeMatch ? +routeMatch[1] : 4; // Default for testing
    this.currentSiteId = routeMatch ? +routeMatch[2] : 3;    // Default for testing
    
    Logger.info('Site context initialized', { 
      projectId: this.currentProjectId, 
      siteId: this.currentSiteId 
    });
  }

  /**
   * Setup reactive subscriptions to state changes
   */
  private setupStateSubscriptions(): void {
    // Subscribe to combined state for child components
    combineLatest([
      this.patternState.state$,
      this.patternState.drillPoints$,
      this.patternState.settings$
    ]).pipe(
      takeUntil(this.destroy$)
    ).subscribe(([state, drillPoints, settings]) => {
      this.currentState = state;
      
      // Update UI state for child components
      this.uiState = {
        isHolePlacementMode: state.isHolePlacementMode,
        isPreciseMode: state.isPreciseMode,
        isFullscreen: state.isFullscreen,
        showInstructions: state.showInstructions,
        isSaved: state.isSaved,
        duplicateMessage: state.duplicateAttemptMessage,
        cursorPosition: state.cursorPosition
      };
      
      this.cdr.markForCheck();
    });
  }

  /**
   * Load pattern data from backend
   */
  private loadPatternData(): void {
    if (!this.currentProjectId || !this.currentSiteId) {
      Logger.warn('Missing project or site ID, skipping pattern load');
      return;
    }

    this.unifiedDrillDataService.loadPatternData(this.currentProjectId, this.currentSiteId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (pattern: any) => this.handlePatternDataLoaded(pattern),
        error: (error: any) => this.errorHandler.handleError(error, 'loadPatternData')
      });
  }

  /**
   * Handle loaded pattern data
   */
  private handlePatternDataLoaded(pattern: any): void {
    const drillData = pattern?.drillLocations || pattern?.drillPoints || [];
    
    if (pattern && drillData && drillData.length > 0) {
      // Convert backend data to local format
      const drillPoints = drillData.map((location: any) => ({
        id: location.id,
        x: location.x,
        y: location.y,
        depth: location.depth,
        spacing: location.spacing,
        burden: location.burden
      }));
      
      const settings = {
        spacing: pattern.settings?.spacing || 3.0,
        burden: pattern.settings?.burden || 2.5,
        depth: pattern.settings?.depth || 10.0
      };

      // Update state through service
      this.patternState.updateSettings(settings);
      drillPoints.forEach((point: DrillPoint) => this.patternState.addDrillPoint(point));
      
      // Update drill point service ID counter
      if (drillPoints.length > 0) {
        const highestId = Math.max(...drillPoints.map((point: DrillPoint) => {
          const numericPart = parseInt(point.id.replace('DH', ''));
          return isNaN(numericPart) ? 0 : numericPart;
        }));
        this.drillPointService.setCurrentId(highestId + 1);
      }
      
      Logger.info('Pattern data loaded successfully', { 
        pointCount: drillPoints.length, 
        settings 
      });
    }
  }

  // =============================================================================
  // EVENT HANDLERS FOR CHILD COMPONENTS
  // =============================================================================

  /**
   * Handle toolbar events
   */
  public onToolbarSettingsChange(settings: PatternSettings): void {
    this.patternState.updateSettings(settings);
  }

  public onToolbarModeToggle(event: ModeToggleEvent): void {
    Logger.info('Mode toggle event', event);
    
    switch (event.mode) {
      case 'HOLE_PLACEMENT':
        this.patternState.toggleHolePlacementMode();
        Logger.info('Hole placement mode toggled');
        break;
      case 'PRECISE':
        this.patternState.togglePreciseMode();
        Logger.info('Precise mode toggled');
        break;
      case 'FULLSCREEN':
        this.patternState.toggleFullscreen();
        Logger.info('Fullscreen mode toggled');
        break;
    }
  }

  public onToolbarPointAction(event: PointActionEvent): void {
    switch (event.action) {
      case 'DELETE':
        if (event.pointId) {
          this.patternState.removeDrillPoint(event.pointId);
        }
        break;
      case 'CLEAR_ALL':
        this.patternState.clearAllPoints();
        break;
      case 'OPEN_DEPTH_EDITOR':
        this.openDepthEditorDialog();
        break;
      case 'UPDATE_POINT_DEPTH':
        if (event.pointId && event.depth !== undefined) {
          this.updatePointDepth(event.pointId, event.depth);
        }
        break;
    }
  }

  public onToolbarPatternAction(event: PatternActionEvent): void {
    switch (event.action) {
      case 'SAVE':
        this.savePattern();
        break;
      case 'EXPORT_TO_BLAST_DESIGNER':
        this.exportToBlastDesigner();
        break;
    }
  }

  /**
   * Handle canvas events
   */
  public onCanvasPointPlaced(event: PlacePointEvent): void {
    Logger.info('Point placement event received', event);
    
    const point = this.drillPointService.createDrillPoint(
      event.x, 
      event.y, 
      event.settings
    );
    
    Logger.info('Created drill point', point);
    
    if (this.validatePointPlacement(point)) {
      this.patternState.addDrillPoint(point);
      Logger.info('Point added to state');
    } else {
      Logger.warn('Point placement validation failed');
    }
  }

  public onCanvasPointSelected(point: DrillPoint): void {
    Logger.info('Point selected', point);
    this.patternState.selectPoint(point);
  }

  public onCanvasPointMoved(event: MovePointEvent): void {
    Logger.info('Point moved', event);
    
    // Create a temporary point for validation
    const tempPoint: DrillPoint = {
      id: event.point.id,
      x: event.newX,
      y: event.newY,
      depth: event.point.depth,
      spacing: event.point.spacing,
      burden: event.point.burden
    };
    
    if (this.validatePointPlacement(tempPoint)) {
      // Update the point with new coordinates
      const updatedPoint = { ...event.point, x: event.newX, y: event.newY };
      this.patternState.updateDrillPoint(updatedPoint);
      Logger.info('Point position updated');
    }
  }

  public onCanvasStateChange(canvasState: CanvasState): void {
    this.canvasState = canvasState;
  }

  // =============================================================================
  // PRIVATE HELPER METHODS
  // =============================================================================

  /**
   * Validate point placement (coordinates and uniqueness)
   */
  private validatePointPlacement(point: DrillPoint): boolean {
    const currentPoints = this.patternState.drillPoints;
    
    if (!this.drillPointService.validateCoordinates(point.x, point.y)) {
      return false;
    }

    const otherPoints = currentPoints.filter(p => p.id !== point.id);
    if (!this.drillPointService.validateUniqueCoordinates(point.x, point.y, otherPoints)) {
      this.showDuplicateMessage(point.x, point.y);
      return false;
    }

    return true;
  }

  /**
   * Show duplicate placement message
   */
  private showDuplicateMessage(x: number, y: number): void {
    const message = `Hole already exists at (${this.formatValue(x)}, ${this.formatValue(y)})`;
    this.patternState.setDuplicateMessage(message);
    
    // Clear message after 3 seconds
    setTimeout(() => {
      this.patternState.setDuplicateMessage(null);
    }, 3000);
  }

  /**
   * Update individual point depth
   */
  private updatePointDepth(pointId: string, depth: number): void {
    const currentPoints = this.patternState.drillPoints;
    const point = currentPoints.find(p => p.id === pointId);
    
    if (point && this.drillPointService.validateDepthRange(depth)) {
      const updatedPoint = { ...point, depth: this.drillPointService.formatDepth(depth) };
      this.patternState.updateDrillPoint(updatedPoint);
    }
  }

  /**
   * Save pattern to backend
   */
  private savePattern(): void {
    const currentState = this.patternState.currentState;
    const drillPoints = currentState.drillPoints;
    const settings = currentState.settings;

    if (drillPoints.length === 0) {
      this.notificationService.showError('No drill points to save');
      return;
    }

    // Convert to backend format
    const drillLocations: DrillLocation[] = drillPoints.map((point: DrillPoint) => ({
      id: point.id,
      x: point.x,
      y: point.y,
      depth: point.depth,
      spacing: point.spacing,
      burden: point.burden,
      projectId: this.currentProjectId,
      siteId: this.currentSiteId,
      easting: point.x,
      northing: point.y,
      elevation: 0,
      length: point.depth,
      azimuth: 0,
      dip: 0,
      actualDepth: point.depth,
      stemming: 0,
      createdAt: new Date(),
      updatedAt: new Date(),
      has3DData: false,
      requiresFallbackTo2D: false
    }));

    const unifiedSettings: UnifiedPatternSettings = {
      name: 'Default Pattern',
      projectId: this.currentProjectId,
      siteId: this.currentSiteId,
      spacing: settings.spacing,
      burden: settings.burden,
      depth: settings.depth
    };

    this.unifiedDrillDataService.savePattern(
      this.currentProjectId,
      this.currentSiteId,
      drillLocations,
      unifiedSettings
    ).pipe(
      takeUntil(this.destroy$)
    ).subscribe({
      next: (result) => {
        if (result.success) {
          this.patternState.setSaved(true);
          this.notificationService.showSuccess('Pattern saved successfully');
          
          // Reset save state after 3 seconds
          setTimeout(() => {
            this.patternState.setSaved(false);
          }, 3000);
        } else {
          this.notificationService.showError('Failed to save pattern');
        }
      },
      error: (error) => {
        this.errorHandler.handleError(error as Error, 'savePattern');
        this.notificationService.showError('Failed to save pattern');
      }
    });
  }

  /**
   * Export pattern to blast designer
   */
  private exportToBlastDesigner(): void {
    const currentState = this.patternState.currentState;
    
    if (currentState.drillPoints.length === 0) {
      this.notificationService.showError('No drill points to export');
      return;
    }
    
    const targetRoute = `/blasting-engineer/project-management/${this.currentProjectId}/sites/${this.currentSiteId}/sequence-designer`;
    
    this.router.navigate([targetRoute]).then(success => {
      if (success) {
        Logger.info('Successfully navigated to blast designer');
      } else {
        this.notificationService.showError('Failed to navigate to blast designer');
      }
    }).catch(error => {
      this.errorHandler.handleError(error as Error, 'exportToBlastDesigner');
    });
  }

  /**
   * Open depth editor dialog
   */
  private openDepthEditorDialog(): void {
    const currentState = this.patternState.currentState;
    
    const dialogRef = this.dialog.open(DepthEditorTableComponent, {
      width: '90vw',
      maxWidth: '1200px',
      height: '80vh',
      maxHeight: '800px',
      data: {
        drillPoints: [...currentState.drillPoints],
        globalDepth: currentState.settings.depth,
        readonly: currentState.isReadOnly
      },
      disableClose: false,
      autoFocus: true,
      restoreFocus: true
    });

    // Handle depth change events
    const componentInstance = dialogRef.componentInstance;
    
    componentInstance.depthChange.pipe(
      takeUntil(this.destroy$)
    ).subscribe((event: DepthChangeEvent) => {
      this.updatePointDepth(event.pointId, event.newDepth);
    });

    componentInstance.bulkDepthChange.pipe(
      takeUntil(this.destroy$)
    ).subscribe((event: BulkDepthChangeEvent) => {
      event.pointIds.forEach(pointId => {
        this.updatePointDepth(pointId, event.newDepth);
      });
    });

    componentInstance.applyGlobalDepth.pipe(
      takeUntil(this.destroy$)
    ).subscribe((event: ApplyGlobalDepthEvent) => {
      const currentSettings = this.patternState.currentState.settings;
      
      if (event.pointIds) {
        event.pointIds.forEach(pointId => {
          this.updatePointDepth(pointId, currentSettings.depth);
        });
      } else {
        // Apply to all points
        const currentPoints = this.patternState.currentState.drillPoints;
        currentPoints.forEach(point => {
          this.updatePointDepth(point.id, currentSettings.depth);
        });
      }
    });
  }

  /**
   * Format numeric values for display
   */
  public formatValue(value: number | undefined | null): string {
    if (value === undefined || value === null || isNaN(value)) {
      return '0.00';
    }
    return value % 1 === 0 ? value.toString() : value.toFixed(2);
  }
}