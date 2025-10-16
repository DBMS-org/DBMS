import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, OnDestroy, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { SiteBlastingService } from '../../../core/services/site-blasting.service';
import { SiteService } from '../../../core/services/site.service';
import { DrillPoint, PatternSettings } from '../../blasting-engineer/drilling-pattern-creator/models/drill-point.model';
import Konva from 'konva';
import { CanvasService } from '../../blasting-engineer/drilling-pattern-creator/services/canvas.service';
import { ZoomService } from '../../blasting-engineer/drilling-pattern-creator/services/zoom.service';
import { CANVAS_CONSTANTS } from '../../blasting-engineer/drilling-pattern-creator/constants/canvas.constants';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog, MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { GridService } from '../../blasting-engineer/drilling-pattern-creator/services/grid.service';
import { RulerService } from '../../blasting-engineer/drilling-pattern-creator/services/ruler.service';
import { UnifiedDrillDataService } from '../../../core/services/unified-drill-data.service';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-confirmation-dialog',
  template: `
    <h2 mat-dialog-title>
      <span class="material-icons">{{ data.icon }}</span>
      {{ data.title }}
    </h2>
    <mat-dialog-content>
      <p>{{ data.message }}</p>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button [color]="data.confirmColor" (click)="confirm()">
        {{ data.confirmText }}
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    :host {
      display: block;
      padding: 1rem;
    }
    h2 {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      margin: 0;
      color: #212529;
    }
    .material-icons {
      color: #1971c2;
    }
    mat-dialog-content {
      margin: 1rem 0;
    }
    mat-dialog-actions {
      gap: 0.5rem;
    }
  `],
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule]
})
export class ConfirmationDialogComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: {
      title: string;
      message: string;
      icon: string;
      confirmText: string;
      confirmColor: 'primary' | 'warn' | 'accent';
    },
    private dialogRef: MatDialogRef<ConfirmationDialogComponent>
  ) {}

  confirm(): void {
    this.dialogRef.close(true);
  }
}

@Component({
  selector: 'app-points-table-dialog',
  template: `
    <h2 mat-dialog-title>
      <span class="material-icons">grid_on</span>
      Drill Points List ({{ sortedPoints.length }} points)
    </h2>
    <mat-dialog-content>
      <table class="points-table" *ngIf="sortedPoints.length > 0; else emptyState">
        <thead>
          <tr>
            <th>Point ID</th>
            <th>Depth (m)</th>
            <th>Spacing (m)</th>
            <th>Burden (m)</th>
          </tr>
        </thead>
        <tbody>
          @for (p of sortedPoints; track p.id) {
            <tr>
              <td class="point-id">{{ p.id }}</td>
              <td>{{ p.depth.toFixed(2) }}</td>
              <td>{{ p.spacing.toFixed(2) }}</td>
              <td>{{ p.burden.toFixed(2) }}</td>
            </tr>
          }
        </tbody>
      </table>
      <ng-template #emptyState>
        <div class="alert alert-info">No pattern data found.</div>
      </ng-template>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Close</button>
    </mat-dialog-actions>
  `,
  styles: [`
    :host {
      display: block;
      padding: 1rem;
    }
    mat-dialog-content {
      min-height: 200px;
      max-height: 500px;
    }
    .points-table {
      width: 100%;
      border-collapse: collapse;
    }
    .points-table th, .points-table td {
      padding: 0.75rem;
      text-align: left;
      border-bottom: 1px solid #e9ecef;
      font-size: 0.875rem;
    }
    .points-table th {
      background-color: #f8f9fa;
      color: #495057;
      font-weight: 600;
      position: sticky;
      top: 0;
      z-index: 1;
    }
    .points-table td {
      color: #212529;
    }
    .points-table tbody tr:hover {
      background-color: #f8f9fa;
    }
    h2 {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      margin: 0;
      color: #212529;
    }
    .material-icons {
      color: #1971c2;
    }
  `],
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule]
})
export class PointsTableDialogComponent {
  sortedPoints: any[] = [];

  constructor(@Inject(MAT_DIALOG_DATA) public data: { points: any[] }) {
    // Sort drill points by ID for organized display
    this.sortedPoints = [...data.points].sort((a, b) => a.id - b.id);
  }
}

@Component({
  selector: 'app-operator-pattern-view',
  standalone: true,
  imports: [CommonModule, RouterModule, MatButtonModule, MatTooltipModule, MatDialogModule, MatIconModule],
  templateUrl: './pattern-view.component.html',
  styleUrl: './pattern-view.component.scss'
})
export class OperatorPatternViewComponent implements OnInit, AfterViewInit, OnDestroy {
  projectId!: number;
  siteId!: number;
  drillPoints: DrillPoint[] = [];
  loading = true;
  error: string | null = null;
  @ViewChild('canvasContainer') containerRef!: ElementRef<HTMLDivElement>;
  private stage!: Konva.Stage;
  private gridLayer!: Konva.Layer;
  private rulerLayer!: Konva.Layer;
  private pointsLayer!: Konva.Layer;

  private gridGroup!: Konva.Group;
  private rulerGroup!: Konva.Group;

  // Canvas pan and zoom state
  private offsetX = 0;
  private offsetY = 0;
  private panStartX = 0;
  private panStartY = 0;
  private panOffsetX = 0;
  private panOffsetY = 0;
  private isPanning = false;

  // Drilling pattern configuration
  settings: PatternSettings = { ...CANVAS_CONSTANTS.DEFAULT_SETTINGS };

  // Component UI state
  showInstructions = false;
  isCompleted = false;

  constructor(
    private route: ActivatedRoute,
    private siteBlastingService: SiteBlastingService,
    private siteService: SiteService,
    private canvasService: CanvasService,
    private zoomService: ZoomService,
    private dialog: MatDialog,
    private gridService: GridService,
    private rulerService: RulerService,
    private unifiedDrillDataService: UnifiedDrillDataService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.siteId = +this.route.snapshot.paramMap.get('siteId')!;
    if (!this.siteId) {
      this.error = 'Invalid route parameters';
      this.loading = false;
      return;
    }

    // fetch site to get projectId and completion status
    this.siteService.getSite(this.siteId).subscribe({
      next: site => {
        this.projectId = site.projectId;
        this.isCompleted = site.isOperatorCompleted;
        this.loadPatternData();
      },
      error: err => {
        this.error = err.message;
        this.loading = false;
      }
    });
  }

  ngAfterViewInit() {
    // Wait for DOM to fully render before creating canvas
    setTimeout(() => {
      this.initializeCanvas();
    }, 100);
  }

  private initializeCanvas() {
    if (!this.containerRef?.nativeElement) {
      console.warn('Canvas container not available');
      return;
    }

    const containerElement = this.containerRef.nativeElement;
    const containerWidth = containerElement.clientWidth;
    const containerHeight = containerElement.clientHeight;

    console.log('Initializing canvas:', { containerWidth, containerHeight });

    if (containerWidth === 0 || containerHeight === 0) {
      console.warn('Container has invalid dimensions, retrying...', { containerWidth, containerHeight });
      setTimeout(() => this.initializeCanvas(), 200);
      return;
    }

    this.stage = new Konva.Stage({
      container: containerElement,
      width: containerWidth,
      height: containerHeight
    });

    // Create layers with proper interaction settings
    this.gridLayer = new Konva.Layer({ listening: false });
    this.rulerLayer = new Konva.Layer({ listening: false });
    this.pointsLayer = new Konva.Layer({ listening: false }); // Disable point interactions

    this.stage.add(this.gridLayer);
    this.stage.add(this.rulerLayer);
    this.stage.add(this.pointsLayer);

    // Setup zoom events and callbacks
    this.setupZoomService();

    // Setup panning
    this.setupPanEvents();

    // Handle window resize
    window.addEventListener('resize', this.handleResize);

    // Initial draw
    this.redrawCanvas();
  }

  private handleResize = () => {
    if (!this.containerRef || !this.stage) return;
    
    this.stage.width(this.containerRef.nativeElement.clientWidth);
    this.stage.height(this.containerRef.nativeElement.clientHeight);
    this.redrawCanvas();
  };

  private setupZoomService() {
    // Configure zoom service with view-only settings
    this.zoomService.setCallbacks({
      onRedraw: () => this.redrawCanvas()
    });
    this.zoomService.setupZoomEvents(this.stage);
  }

  private setupPanEvents() {
    // Start panning on mouse down (left button)
    this.stage.on('mousedown', (e: Konva.KonvaEventObject<MouseEvent>) => {
      if (e.evt.button !== 0) return; // Only left button
      const pointer = this.stage.getPointerPosition();
      if (!pointer) return;
      
      this.isPanning = true;
      this.panStartX = pointer.x;
      this.panStartY = pointer.y;
      this.canvasService.setCanvasCursor(this.stage, 'grabbing');
      
      // Prevent text selection during panning
      e.evt.preventDefault();
    });

    // Handle panning movement
    this.stage.on('mousemove', (e: Konva.KonvaEventObject<MouseEvent>) => {
      if (!this.isPanning) return;
      const pointer = this.stage.getPointerPosition();
      if (!pointer) return;
      
      const deltaX = pointer.x - this.panStartX;
      const deltaY = pointer.y - this.panStartY;
      
      // Calculate new potential pan offsets
      const newPanOffsetX = this.panOffsetX + deltaX;
      const newPanOffsetY = this.panOffsetY + deltaY;
      
      // Store previous values to check if boundary was hit
      const prevPanOffsetX = this.panOffsetX;
      const prevPanOffsetY = this.panOffsetY;
      
      // Apply boundary constraints - don't allow panning beyond origin (0,0)
      this.panOffsetX = Math.min(0, newPanOffsetX);
      this.panOffsetY = Math.min(0, newPanOffsetY);
      
      // Check if we hit a boundary and provide visual feedback
      const hitBoundaryX = newPanOffsetX > 0 && this.panOffsetX === 0;
      const hitBoundaryY = newPanOffsetY > 0 && this.panOffsetY === 0;
      
      if (hitBoundaryX || hitBoundaryY) {
        // Briefly change cursor to indicate boundary hit
        this.canvasService.setCanvasCursor(this.stage, 'not-allowed');
        setTimeout(() => {
          if (this.isPanning) {
            this.canvasService.setCanvasCursor(this.stage, 'grabbing');
          }
        }, 100);
      }
      
      this.panStartX = pointer.x;
      this.panStartY = pointer.y;
      
      // Only redraw if pan offsets actually changed
      if (this.panOffsetX !== prevPanOffsetX || this.panOffsetY !== prevPanOffsetY) {
        this.drawGrid();
        this.drawRulers();
        this.drawPoints(); // Redraw points to move with grid
      }
      
      e.evt.preventDefault();
    });

    // End panning on mouse up or mouse leave
    const endPanning = () => {
      if (!this.isPanning) return;
      this.isPanning = false;
      this.canvasService.setCanvasCursor(this.stage, 'grab');
    };

    this.stage.on('mouseup', endPanning);
    this.stage.on('mouseleave', endPanning);

    // Set default cursor
    this.canvasService.setCanvasCursor(this.stage, 'grab');
  }

  private redrawCanvas() {
    // Draw grid, rulers, points in order
    this.drawGrid();
    this.drawRulers();
    this.drawPoints();
  }

  private drawGrid() {
    if (!this.gridLayer) return;

    if (this.gridGroup) {
      this.gridGroup.destroy();
    }

    this.gridGroup = this.gridService.drawGrid(
      this.gridLayer,
      this.settings,
      this.zoomService.getCurrentScale(),
      this.offsetX + this.panOffsetX,
      this.offsetY + this.panOffsetY,
      this.stage.width(),
      this.stage.height()
    );

    this.gridLayer.add(this.gridGroup);
    this.gridLayer.batchDraw();
  }

  private drawRulers() {
    if (!this.rulerLayer) return;

    if (this.rulerGroup) {
      this.rulerGroup.destroy();
    }

    this.rulerGroup = this.rulerService.drawRulers(
      this.rulerLayer,
      this.settings,
      this.zoomService.getCurrentScale(),
      this.stage.width(),
      this.stage.height(),
      this.panOffsetX,
      this.panOffsetY
    );

    this.rulerLayer.add(this.rulerGroup);
    this.rulerLayer.batchDraw();
  }

  private drawPoints() {
    if (!this.pointsLayer) return;

    // Clear existing visuals
    this.pointsLayer.destroyChildren();

    const scale = this.zoomService.getCurrentScale();

    this.drillPoints.forEach(p => {
      const pointGroup = this.canvasService.createDrillPointObject(
        p,
        scale,
        this.offsetX + this.panOffsetX,
        this.offsetY + this.panOffsetY,
        true, // disable dragging
        false // not selected
      );
      this.pointsLayer.add(pointGroup);
    });

    this.pointsLayer.batchDraw();
  }

  private loadPatternData(): void {
    if (!this.projectId || !this.siteId) {
      console.error('Project ID or Site ID not available');
      this.error = 'Project ID or Site ID not available';
      this.loading = false;
      return;
    }

    // Load drill points instead of drill patterns
    this.unifiedDrillDataService.getDrillPoints(this.projectId, this.siteId).subscribe({
      next: (drillPoints) => {
        console.log('Drill points loaded:', drillPoints);
        this.drillPoints = drillPoints.map(point => ({
          id: point.id,
          x: point.x,
          y: point.y,
          depth: point.depth,
          spacing: point.spacing,
          burden: point.burden,
          stemming: point.stemming || this.settings.stemming,
          subDrill: point.subDrill || this.settings.subDrill
        }));
        this.loading = false;
        this.error = null;
        this.cdr.detectChanges();
        
        // Render canvas after data is loaded
        setTimeout(() => {
          this.renderCanvas();
        }, 100);
      },
      error: (error) => {
        console.error('Error loading drill points:', error);
        this.error = 'Failed to load drill points: ' + (error.message || 'Unknown error');
        this.drillPoints = [];
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  private renderCanvas() {
    if (!this.stage) {
      console.log('Stage not initialized, initializing canvas...');
      this.initializeCanvas();
      // Delay redraw to ensure stage is ready
      setTimeout(() => {
        if (this.stage) {
          console.log('Canvas ready, drawing content...');
          this.redrawCanvas();
        }
      }, 100);
    } else {
      console.log('Stage available, redrawing canvas...');
      this.redrawCanvas();
    }
  }

  openPointsTable(): void {
    this.dialog.open(PointsTableDialogComponent, {
      data: { points: this.drillPoints },
      width: '600px',
      maxWidth: '90vw',
      maxHeight: '90vh'
    });
  }

  goBack(): void {
    history.back();
  }

  resetView(): void {
    this.offsetX = 0;
    this.offsetY = 0;
    this.panOffsetX = 0;
    this.panOffsetY = 0;
    this.redrawCanvas();
  }

  retryLoad(): void {
    this.error = null;
    this.loading = true;
    this.loadPatternData();
  }

  toggleInstructions(): void {
    this.showInstructions = !this.showInstructions;
  }

  markAsCompleted(): void {
    if (this.isCompleted || this.drillPoints.length === 0) {
      return;
    }

    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Mark Pattern as Completed',
        message: `Are you sure you want to mark the drilling pattern for Site ${this.siteId} as completed? This action will be recorded in the database.`,
        icon: 'task_alt',
        confirmText: 'Mark as Completed',
        confirmColor: 'primary'
      },
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // TODO: Implement operator completion tracking in backend
        this.isCompleted = true;
        console.log(`Drilling pattern for Site ${this.siteId} marked as completed`);
      }
    });
  }

  revokeCompletion(): void {
    if (!this.isCompleted) {
      return;
    }

    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Revoke Pattern Completion',
        message: `Are you sure you want to revoke the completion status for Site ${this.siteId}? This will reset the completion status in the database.`,
        icon: 'undo',
        confirmText: 'Revoke Completion',
        confirmColor: 'warn'
      },
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // TODO: Implement operator completion revocation in backend
        this.isCompleted = false;
        console.log(`Drilling pattern completion for Site ${this.siteId} has been revoked`);
      }
    });
  }

  ngOnDestroy(): void {
    // Clean up event listeners
    if (this.stage) {
      this.zoomService.removeZoomEvents(this.stage);
      this.stage.destroy();
    }
    window.removeEventListener('resize', this.handleResize);
  }
}