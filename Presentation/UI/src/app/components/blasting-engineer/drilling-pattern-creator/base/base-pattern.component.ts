import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { BasePatternComponent } from '../interfaces/component.interfaces';
import { PatternCreatorEvent } from '../interfaces/service.interfaces';
import { PatternStateService } from '../services/pattern-state.service';
import { ErrorHandlingService } from '../services/error-handling.service';

/**
 * Base abstract component for all pattern creator components
 * Provides common functionality like lifecycle management, error handling, and event subscription
 */
@Component({
  template: ''
})
export abstract class BasePatternComponentClass implements BasePatternComponent, OnInit, OnDestroy {
  protected readonly destroy$ = new Subject<void>();
  protected readonly stateService = inject(PatternStateService);
  protected readonly errorHandler = inject(ErrorHandlingService);
  // Note: eventBus is optional since we don't have a concrete implementation yet
  protected readonly eventBus: any = null;
  
  /**
   * Component name for logging and debugging
   */
  protected abstract readonly componentName: string;
  
  /**
   * Whether the component should automatically subscribe to state changes
   */
  protected readonly autoSubscribeToState: boolean = true;
  
  /**
   * Whether the component should emit lifecycle events
   */
  protected readonly emitLifecycleEvents: boolean = true;

  ngOnInit(): void {
    try {
      this.onComponentInit();
      
      if (this.autoSubscribeToState) {
        this.subscribeToStateChanges();
      }
      
      this.subscribeToEvents();
      
      if (this.emitLifecycleEvents && this.eventBus) {
        this.eventBus.emit({
          type: 'COMPONENT_INITIALIZED',
          timestamp: new Date().toISOString(),
          source: this.componentName,
          payload: { component: this.componentName }
        });
      }
    } catch (error) {
      this.handleError(error as Error, 'ngOnInit');
    }
  }

  ngOnDestroy(): void {
    try {
      this.onComponentDestroy();
      
      if (this.emitLifecycleEvents && this.eventBus) {
        this.eventBus.emit({
          type: 'COMPONENT_DESTROYED',
          timestamp: new Date().toISOString(),
          source: this.componentName,
          payload: { component: this.componentName }
        });
      }
      
      this.destroy$.next();
      this.destroy$.complete();
    } catch (error) {
      this.handleError(error as Error, 'ngOnDestroy');
    }
  }

  /**
   * Override this method to implement component-specific initialization
   */
  protected onComponentInit(): void {
    // Default implementation - override in derived classes
  }

  /**
   * Override this method to implement component-specific cleanup
   */
  protected onComponentDestroy(): void {
    // Default implementation - override in derived classes
  }

  /**
   * Subscribe to state changes - override to customize state subscriptions
   */
  protected subscribeToStateChanges(): void {
    // Default implementation - override in derived classes if needed
    this.stateService.state$
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (state) => this.onStateChange(state),
        error: (error) => this.handleError(error, 'state subscription')
      });
  }

  /**
   * Subscribe to events - override to customize event subscriptions
   */
  protected subscribeToEvents(): void {
    // Default implementation - override in derived classes
  }

  /**
   * Handle state changes - override to implement component-specific state handling
   */
  protected onStateChange(state: any): void {
    // Default implementation - override in derived classes
  }

  /**
   * Handle errors consistently across all components
   */
  protected handleError(error: Error, context: string): void {
    this.errorHandler.handleComponentError(this.componentName, error, context);
  }

  /**
   * Emit events through the event bus
   */
  protected emitEvent(event: Partial<PatternCreatorEvent> & { type: string }): void {
    if (!this.eventBus) {
      return; // No event bus available, skip event emission
    }
    
    const fullEvent: PatternCreatorEvent = {
      timestamp: new Date().toISOString(),
      source: this.componentName,
      ...event
    };
    
    this.eventBus.emit(fullEvent);
  }

  /**
   * Subscribe to specific event types
   */
  protected subscribeToEvent(
    eventType: string,
    handler: (event: PatternCreatorEvent) => void
  ): void {
    if (this.eventBus) {
      this.eventBus.subscribe(eventType, handler);
    }
  }

  /**
   * Validate component state - override to implement validation
   */
  protected validateState(): { isValid: boolean; errors: string[] } {
    return { isValid: true, errors: [] };
  }

  /**
   * Reset component to initial state - override to implement reset logic
   */
  protected resetComponent(): void {
    // Default implementation - override in derived classes
  }

  /**
   * Get component debug information
   */
  protected getDebugInfo(): any {
    return {
      componentName: this.componentName,
      isDestroyed: this.destroy$.closed,
      timestamp: new Date()
    };
  }
}