import { Component, Input, Output, EventEmitter, inject } from '@angular/core';
import { BasePatternComponentClass } from './base-pattern.component';
import { ICanvasManagerService, ICanvasRenderingService } from '../interfaces/service.interfaces';
import { PatternSettings } from '../models/drill-point.model';
import { CanvasState } from '../models/pattern-state.model';
import { CANVAS_MANAGER_SERVICE } from '../tokens/injection.tokens';

/**
 * Base abstract component for all canvas-related components
 * Provides common canvas functionality and lifecycle management
 */
@Component({
    template: ''
})
export abstract class BaseCanvasComponentClass extends BasePatternComponentClass {
    protected readonly canvasManager = inject(CANVAS_MANAGER_SERVICE);

    @Input() settings!: PatternSettings;
    @Input() canvasState!: CanvasState;
    @Output() canvasStateChange = new EventEmitter<CanvasState>();

    protected layer: any = null; // Konva.Layer
    protected renderingService: ICanvasRenderingService | null = null;

    /**
     * Layer name for this canvas component
     */
    protected abstract readonly layerName: string;

    /**
     * Whether this component should create its own layer
     */
    protected readonly createOwnLayer: boolean = true;

    /**
     * Whether rendering should be cached for performance
     */
    protected readonly enableCaching: boolean = true;

    protected override onComponentInit(): void {
        super.onComponentInit();
        this.initializeCanvas();
    }

    protected override onComponentDestroy(): void {
        this.destroyCanvas();
        super.onComponentDestroy();
    }

    /**
     * Initialize canvas layer and rendering service
     */
    protected initializeCanvas(): void {
        try {
            if (this.createOwnLayer) {
                this.layer = this.canvasManager.createLayer(this.layerName);
            } else {
                this.layer = this.canvasManager.getLayer(this.layerName);
            }

            if (!this.layer) {
                throw new Error(`Failed to create or get layer: ${this.layerName}`);
            }

            this.initializeRenderingService();
            this.onCanvasInitialized();
        } catch (error) {
            this.handleError(error as Error, 'initializeCanvas');
        }
    }

    /**
     * Initialize the rendering service for this component
     */
    protected abstract initializeRenderingService(): void;

    /**
     * Called after canvas is successfully initialized
     */
    protected onCanvasInitialized(): void {
        // Default implementation - override in derived classes
    }

    /**
     * Destroy canvas resources
     */
    protected destroyCanvas(): void {
        try {
            if (this.renderingService) {
                this.renderingService.destroy();
                this.renderingService = null;
            }

            if (this.layer && this.createOwnLayer) {
                this.canvasManager.removeLayer(this.layerName);
            }

            this.layer = null;
        } catch (error) {
            this.handleError(error as Error, 'destroyCanvas');
        }
    }

    /**
     * Render the canvas content
     */
    public render(): void {
        try {
            if (!this.renderingService) {
                this.handleError(new Error('Rendering service not initialized'), 'render');
                return;
            }

            this.renderingService.render();
            this.onRenderComplete();
        } catch (error) {
            this.handleError(error as Error, 'render');
        }
    }

    /**
     * Update the canvas content
     */
    public update(): void {
        try {
            if (!this.renderingService) {
                this.handleError(new Error('Rendering service not initialized'), 'update');
                return;
            }

            this.renderingService.update();
            this.onUpdateComplete();
        } catch (error) {
            this.handleError(error as Error, 'update');
        }
    }

    /**
     * Clear the canvas content
     */
    public clear(): void {
        try {
            if (!this.renderingService) {
                this.handleError(new Error('Rendering service not initialized'), 'clear');
                return;
            }

            this.renderingService.clear();
            this.onClearComplete();
        } catch (error) {
            this.handleError(error as Error, 'clear');
        }
    }

    /**
     * Called after render is complete
     */
    protected onRenderComplete(): void {
        // Default implementation - override in derived classes
    }

    /**
     * Called after update is complete
     */
    protected onUpdateComplete(): void {
        // Default implementation - override in derived classes
    }

    /**
     * Called after clear is complete
     */
    protected onClearComplete(): void {
        // Default implementation - override in derived classes
    }

    /**
     * Handle settings changes
     */
    protected onSettingsChange(newSettings: PatternSettings): void {
        this.settings = newSettings;
        this.update();
    }

    /**
     * Handle canvas state changes
     */
    protected onCanvasStateChange(newState: CanvasState): void {
        this.canvasState = newState;
        this.canvasStateChange.emit(newState);
        this.update();
    }

    /**
     * Enable or disable caching for performance
     */
    protected setCachingEnabled(enabled: boolean): void {
        if (this.renderingService) {
            this.renderingService.enableCaching(enabled);
        }
    }

    /**
     * Invalidate cache to force re-render
     */
    protected invalidateCache(): void {
        if (this.renderingService) {
            this.renderingService.invalidateCache();
        }
    }

    /**
     * Set visibility of this canvas component
     */
    protected setVisible(visible: boolean): void {
        if (this.renderingService) {
            this.renderingService.setVisible(visible);
        }
    }

    /**
     * Check if this canvas component is visible
     */
    protected isVisible(): boolean {
        return this.renderingService ? this.renderingService.isVisible() : false;
    }

    /**
     * Get layer bounds for optimization
     */
    protected getLayerBounds(): { x: number; y: number; width: number; height: number } | null {
        if (!this.layer) {
            return null;
        }

        try {
            const clientRect = this.layer.getClientRect();
            return {
                x: clientRect.x,
                y: clientRect.y,
                width: clientRect.width,
                height: clientRect.height
            };
        } catch (error) {
            this.handleError(error as Error, 'getLayerBounds');
            return null;
        }
    }

    /**
     * Check if a point is within the visible canvas area
     */
    protected isPointVisible(x: number, y: number): boolean {
        const bounds = this.getLayerBounds();
        if (!bounds) {
            return true; // Assume visible if we can't determine bounds
        }

        return x >= bounds.x &&
            x <= bounds.x + bounds.width &&
            y >= bounds.y &&
            y <= bounds.y + bounds.height;
    }

    /**
     * Override to provide canvas-specific debug information
     */
    protected override getDebugInfo(): any {
        return {
            ...super.getDebugInfo(),
            layerName: this.layerName,
            hasLayer: !!this.layer,
            hasRenderingService: !!this.renderingService,
            isVisible: this.isVisible(),
            bounds: this.getLayerBounds()
        };
    }
}