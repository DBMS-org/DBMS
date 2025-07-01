import { Injectable } from '@angular/core';
import Konva from 'konva';
import { Subject } from 'rxjs';

import { DrillPoint, CanvasPosition } from '../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../constants/canvas.constants';
import { Logger } from '../utils/logger.util';
import { KonvaHelpers } from '../utils/konva-helpers.util';

export interface CanvasEvent {
  type: 'click' | 'contextmenu' | 'dragstart' | 'dragmove' | 'dragend' | 'mousemove' | 'wheel';
  position?: CanvasPosition;
  pointId?: string;
  originalEvent?: any;
}

export interface PanState {
  isPanning: boolean;
  startX: number;
  startY: number;
  offsetX: number;
  offsetY: number;
}

@Injectable({
  providedIn: 'root'
})
export class EventHandlerManager {
  private canvasEvents$ = new Subject<CanvasEvent>();
  private panState: PanState = {
    isPanning: false,
    startX: 0,
    startY: 0,
    offsetX: 0,
    offsetY: 0
  };

  private isDragging = false;
  private draggedPointId?: string;

  // Timeout handles for cleanup
  private timeouts: Map<string, number> = new Map();

  getCanvasEvents() {
    return this.canvasEvents$.asObservable();
  }

  getPanState(): PanState {
    return { ...this.panState };
  }

  setupStageEvents(stage: Konva.Stage): void {
    // Click events
    stage.on('click tap', (e) => this.handleStageClick(e));
    stage.on('contextmenu', (e) => this.handleContextMenu(e));
    
    // Pan events
    stage.on('mousedown', (e) => this.handlePanStart(e));
    stage.on('mousemove', (e) => this.handlePanMove(e));
    stage.on('mouseup', () => this.handlePanEnd());
    stage.on('mouseleave', () => this.handlePanEnd());
    
    // Zoom events
    stage.on('wheel', (e) => this.handleWheel(e));

    Logger.info('Stage events configured');
  }

  setupPointEvents(pointGroup: Konva.Group, pointId: string): void {
    // Click events
    pointGroup.on('click tap', (e) => {
      e.cancelBubble = true;
      this.canvasEvents$.next({
        type: 'click',
        pointId,
        position: this.getEventPosition(e)
      });
    });

    // Context menu
    pointGroup.on('contextmenu', (e) => {
      e.cancelBubble = true;
      this.canvasEvents$.next({
        type: 'contextmenu',
        pointId,
        position: this.getEventPosition(e)
      });
    });

    // Drag events
    pointGroup.draggable(true);
    pointGroup.on('dragstart', (e) => {
      this.isDragging = true;
      this.draggedPointId = pointId;
      Logger.debug('Drag started for point', pointId);
      
      this.canvasEvents$.next({
        type: 'dragstart',
        pointId,
        position: this.getEventPosition(e)
      });
    });

    pointGroup.on('dragmove', (e) => {
      if (this.isDragging && this.draggedPointId === pointId) {
        this.canvasEvents$.next({
          type: 'dragmove',
          pointId,
          position: this.getEventPosition(e)
        });
      }
    });

    pointGroup.on('dragend', (e) => {
      if (this.isDragging && this.draggedPointId === pointId) {
        this.isDragging = false;
        this.draggedPointId = undefined;
        
        this.canvasEvents$.next({
          type: 'dragend',
          pointId,
          position: this.getEventPosition(e)
        });
        
        Logger.debug('Drag ended for point', pointId);
      }
    });
  }

  private handleStageClick(e: Konva.KonvaEventObject<MouseEvent>): void {
    // Only handle clicks on empty space (not on drill points)
    if (e.target === e.target.getStage()) {
      this.canvasEvents$.next({
        type: 'click',
        position: this.getEventPosition(e),
        originalEvent: e
      });
    }
  }

  private handleContextMenu(e: Konva.KonvaEventObject<MouseEvent>): void {
    e.evt.preventDefault();
    
    const pointId = KonvaHelpers.getPointIdFromTarget(e.target);
    this.canvasEvents$.next({
      type: 'contextmenu',
      pointId: pointId || undefined,
      position: this.getEventPosition(e),
      originalEvent: e
    });
  }

  private handlePanStart(e: Konva.KonvaEventObject<MouseEvent>): void {
    // Only start panning if clicking on empty space and not dragging a point
    if (e.target === e.target.getStage() && !this.isDragging) {
      const pos = this.getEventPosition(e);
      this.panState.isPanning = true;
      this.panState.startX = pos.x - this.panState.offsetX;
      this.panState.startY = pos.y - this.panState.offsetY;
    }
  }

  private handlePanMove(e: Konva.KonvaEventObject<MouseEvent>): void {
    if (this.panState.isPanning && !this.isDragging) {
      const pos = this.getEventPosition(e);
      const newOffsetX = pos.x - this.panState.startX;
      const newOffsetY = pos.y - this.panState.startY;

      // Only update if there's significant movement
      if (Math.abs(newOffsetX - this.panState.offsetX) > 1 || 
          Math.abs(newOffsetY - this.panState.offsetY) > 1) {
        this.panState.offsetX = newOffsetX;
        this.panState.offsetY = newOffsetY;

        this.canvasEvents$.next({
          type: 'mousemove',
          position: pos,
          originalEvent: e
        });
      }
    }
  }

  private handlePanEnd(): void {
    this.panState.isPanning = false;
  }

  private handleWheel(e: Konva.KonvaEventObject<WheelEvent>): void {
    e.evt.preventDefault();
    
    this.canvasEvents$.next({
      type: 'wheel',
      position: this.getEventPosition(e),
      originalEvent: e
    });
  }

  private getEventPosition(e: Konva.KonvaEventObject<any>): CanvasPosition {
    const stage = e.target.getStage();
    const pointer = stage?.getPointerPosition();
    return pointer || { x: 0, y: 0 };
  }

  // Utility methods for timeouts
  setTimeout(key: string, callback: () => void, delay: number): void {
    this.clearTimeout(key);
    const timeoutId = window.setTimeout(callback, delay);
    this.timeouts.set(key, timeoutId);
  }

  clearTimeout(key: string): void {
    const timeoutId = this.timeouts.get(key);
    if (timeoutId) {
      window.clearTimeout(timeoutId);
      this.timeouts.delete(key);
    }
  }

  clearAllTimeouts(): void {
    this.timeouts.forEach(timeoutId => window.clearTimeout(timeoutId));
    this.timeouts.clear();
  }

  // Cleanup
  destroy(): void {
    this.clearAllTimeouts();
    this.canvasEvents$.complete();
    Logger.info('Event handler manager destroyed');
  }
} 