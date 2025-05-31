import { Injectable } from '@angular/core';
import Konva from 'konva';

export interface ZoomConfig {
  minScale?: number;
  maxScale?: number;
  zoomInFactor?: number;
  zoomOutFactor?: number;
}

export interface ZoomEventCallbacks {
  onZoomChange?: (scale: number) => void;
  onRedraw?: () => void;
}

@Injectable({
  providedIn: 'root'
})
export class ZoomService {
  private currentScale = 1;
  private minScale = 0.1;
  private maxScale = 10;
  private zoomInFactor = 1.1;
  private zoomOutFactor = 0.9;
  private callbacks: ZoomEventCallbacks = {};

  constructor() {}

  // ==================== ZOOM CONFIGURATION ====================

  configure(config: ZoomConfig): void {
    if (config.minScale !== undefined) this.minScale = config.minScale;
    if (config.maxScale !== undefined) this.maxScale = config.maxScale;
    if (config.zoomInFactor !== undefined) this.zoomInFactor = config.zoomInFactor;
    if (config.zoomOutFactor !== undefined) this.zoomOutFactor = config.zoomOutFactor;
  }

  setCallbacks(callbacks: ZoomEventCallbacks): void {
    this.callbacks = callbacks;
  }

  // ==================== ZOOM STATE MANAGEMENT ====================

  getCurrentScale(): number {
    return this.currentScale;
  }

  setScale(scale: number): void {
    const constrainedScale = this.constrainScale(scale);
    if (constrainedScale !== this.currentScale) {
      this.currentScale = constrainedScale;
      this.notifyZoomChange();
    }
  }

  // ==================== EVENT HANDLING ====================

  setupZoomEvents(stage: Konva.Stage): void {
    const container = stage.container();
    container.addEventListener('wheel', this.handleWheelEvent.bind(this), { passive: false });
  }

  private handleWheelEvent(e: WheelEvent): void {
    e.preventDefault();
    
    // Calculate zoom delta
    const delta = e.deltaY > 0 ? this.zoomOutFactor : this.zoomInFactor;
    this.setScale(this.currentScale * delta);
  }

  removeZoomEvents(stage: Konva.Stage): void {
    const container = stage.container();
    container.removeEventListener('wheel', this.handleWheelEvent.bind(this));
  }

  // ==================== UTILITY METHODS ====================

  private constrainScale(scale: number): number {
    return Math.max(this.minScale, Math.min(this.maxScale, scale));
  }

  private notifyZoomChange(): void {
    if (this.callbacks.onZoomChange) {
      this.callbacks.onZoomChange(this.currentScale);
    }
    if (this.callbacks.onRedraw) {
      this.callbacks.onRedraw();
    }
  }
}