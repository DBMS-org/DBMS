import { Component, ElementRef, ViewChild, AfterViewInit, ChangeDetectorRef, ChangeDetectionStrategy, HostListener, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import Konva from 'konva';
import { CanvasService } from './services/canvas.service';
import { DrillPointService } from './services/drill-point.service';
import { DrillPoint, PatternSettings, PatternData, DrillingPatternError } from './models/drill-point.model';
import { CANVAS_CONSTANTS } from './constants/canvas.constants';

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
  private stage!: Konva.Stage;
  private gridLayer!: Konva.Layer;
  private rulerLayer!: Konva.Layer;
  private pointsLayer!: Konva.Layer;
  private gridGroup!: Konva.Group;
  private rulerGroup!: Konva.Group;
  private drillPoints: DrillPoint[] = [];
  public selectedPoint: DrillPoint | null = null;
  public isHolePlacementMode = false;
  public isPreciseMode = false;
  public showInstructions = false;
  public scale = 1;
  private offsetX = 0;
  private offsetY = 0;
  private gridAnimationFrame = 0;
  private gridAnimationTime = 0;
  public cursorPosition: { x: number; y: number } | null = null;
  private lastPointerX = 0;
  private lastPointerY = 0;
  private isDragging = false;
  private draggedPoint: DrillPoint | null = null;
  private drillPointObjects: Map<string, Konva.Group> = new Map();
  private resizeTimeout: any;

  public readonly ARIA_LABELS = CANVAS_CONSTANTS.ARIA_LABELS;
  readonly settings: PatternSettings = CANVAS_CONSTANTS.DEFAULT_SETTINGS;

  constructor(
    private cdr: ChangeDetectorRef,
    private canvasService: CanvasService,
    private drillPointService: DrillPointService
  ) {}

  ngAfterViewInit(): void {
    this.initializeStage();
    this.setupEventListeners();
    this.setupZoomEvents();
    this.drawGrid();
    this.drawRulers();
    this.drawDrillPoints();
  }

  ngOnDestroy(): void {
    if (this.stage) {
      this.stage.destroy();
    }
    if (this.resizeTimeout) {
      clearTimeout(this.resizeTimeout);
    }
    if (this.gridAnimationFrame) {
      cancelAnimationFrame(this.gridAnimationFrame);
    }
  }

  private initializeStage(): void {
    const container = this.containerRef.nativeElement;
    const width = container.offsetWidth;
    const height = container.offsetHeight;

    this.stage = new Konva.Stage({
      container: container,
      width: width,
      height: height
    });

    // Create layers
    this.gridLayer = new Konva.Layer();
    this.rulerLayer = new Konva.Layer();
    this.pointsLayer = new Konva.Layer();

    // Add layers to stage
    this.stage.add(this.rulerLayer);
    this.stage.add(this.gridLayer);
    this.stage.add(this.pointsLayer);

    // Set layer order
    this.rulerLayer.moveToBottom();
    this.gridLayer.moveToBottom();
    this.pointsLayer.moveToTop();
  }

  private setupEventListeners(): void {
    this.stage.on('mousemove', (e) => {
      const pointer = this.stage.getPointerPosition();
      if (pointer) {
        this.cursorPosition = this.canvasService.calculateGridCoordinates(
          pointer,
          this.scale,
          this.offsetX,
          this.offsetY
        );
        this.cdr.detectChanges();
      }
    });

    this.stage.on('mousedown', (e) => {
      if (this.isHolePlacementMode) {
        const pointer = this.stage.getPointerPosition();
        if (pointer) {
          const coords = this.canvasService.calculateGridCoordinates(
            pointer,
            this.scale,
            this.offsetX,
            this.offsetY
          );
          this.addDrillPoint(coords.x, coords.y);
        }
      }
    });

    this.stage.on('dragstart', (e) => {
      const group = e.target as Konva.Group;
      const point = (group as any).data as DrillPoint;
      if (point) {
        this.draggedPoint = point;
        this.isDragging = true;
      }
    });

    this.stage.on('dragmove', (e) => {
      if (this.isDragging && this.draggedPoint) {
        const group = e.target as Konva.Group;
        const pointer = this.stage.getPointerPosition();
        if (pointer) {
          const coords = this.canvasService.calculateGridCoordinates(
            pointer,
            this.scale,
            this.offsetX,
            this.offsetY
          );
          this.updateDrillPointPosition(this.draggedPoint, coords.x, coords.y);
        }
      }
    });

    this.stage.on('dragend', () => {
      this.isDragging = false;
      this.draggedPoint = null;
    });

    this.stage.on('click', (e) => {
      if (!this.isHolePlacementMode) {
        const group = e.target.getParent() as Konva.Group;
        const point = (group as any).data as DrillPoint;
        if (point) {
          this.selectPoint(point);
        } else {
          this.selectPoint(null);
        }
      }
    });
  }

  private setupZoomEvents(): void {
    // Listen for wheel events on the stage container
    const container = this.stage.container();
    container.addEventListener('wheel', (e: WheelEvent) => {
      e.preventDefault();
      
      // Simple zoom factor calculation
      const delta = e.deltaY > 0 ? 0.9 : 1.1;
      this.scale *= delta;
      
      // Redraw everything
      this.drawGrid();
      this.drawRulers();
      this.drawDrillPoints();
    }, { passive: false });
  }

  private drawGrid(): void {
    if (this.gridGroup) {
      this.gridGroup.destroy();
    }

    const cacheKey = `${this.scale}-${this.offsetX}-${this.offsetY}`;
    if (!this.canvasService.handleGridCache(cacheKey, this.gridGroup, this.gridLayer)) {
      this.gridGroup = this.canvasService.drawGrid(
        this.gridLayer,
        this.settings,
        this.scale,
        this.offsetX,
        this.offsetY,
        this.stage.width(),
        this.stage.height()
      );
      this.gridLayer.add(this.gridGroup);
      this.canvasService.updateGridCache(cacheKey, this.gridGroup);
    }
    this.gridLayer.batchDraw();
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
      this.stage.height()
    );
    this.rulerLayer.add(this.rulerGroup);
    this.rulerLayer.batchDraw();
  }

  private drawDrillPoints(): void {
    this.drillPointObjects.forEach(group => {
      group.destroy();
    });
    this.drillPointObjects.clear();

    this.drillPoints.forEach(point => {
      const group = this.canvasService.createDrillPointObject(
        point,
        this.scale,
        this.offsetX,
        this.offsetY,
        this.isHolePlacementMode,
        point === this.selectedPoint
      );
      this.drillPointObjects.set(point.id, group);
      this.pointsLayer.add(group);
    });
    this.pointsLayer.batchDraw();
  }

  private addDrillPoint(x: number, y: number): void {
    if (!this.drillPointService.validateCoordinates(x, y)) {
      return;
    }

    if (!this.drillPointService.validateDrillPointCount(
      this.drillPoints.length,
      CANVAS_CONSTANTS.MAX_DRILL_POINTS
    )) {
      return;
    }

    const point = this.drillPointService.createDrillPoint(x, y, this.settings);
    this.drillPoints.push(point);
    this.drawDrillPoints();
  }

  private updateDrillPointPosition(point: DrillPoint, x: number, y: number): void {
    if (!this.drillPointService.validateCoordinates(x, y)) {
      return;
    }

    point.x = x;
    point.y = y;
    this.drawDrillPoints();
  }

  private selectPoint(point: DrillPoint | null): void {
    this.selectedPoint = this.drillPointService.selectPoint(point, this.drillPoints);
    this.drawDrillPoints();
  }

  toggleHolePlacementMode(): void {
    this.toggleMode('holePlacement');
  }

  togglePreciseMode(): void {
    this.toggleMode('precise');
  }

  private toggleMode(mode: 'holePlacement' | 'precise'): void {
    const modeProperty = mode === 'holePlacement' ? 'isHolePlacementMode' : 'isPreciseMode';
    this[modeProperty] = !this[modeProperty];
    this.canvasService.setCanvasCursor(this.stage, this[modeProperty]);
    if (mode === 'holePlacement') {
      this.canvasService.updatePointSelectability(this.drillPointObjects, this.isHolePlacementMode);
    }
    this.drawDrillPoints();
  }

  onClearAll(): void {
    this.drillPoints = this.drillPointService.clearPoints();
    this.selectPoint(null);
  }

  onExportPattern(): void {
    this.drillPointService.exportPattern(this.drillPoints, this.settings);
  }

  onDeletePoint(): void {
    if (this.selectedPoint) {
      this.drillPoints = this.drillPointService.removePoint(this.selectedPoint, this.drillPoints);
      this.selectPoint(null);
    }
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
}