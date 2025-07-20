import { Component, Input, Output, EventEmitter, OnInit, OnDestroy, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { PatternStateService } from '../../services/pattern-state.service';
import { IPatternInstructionsComponent } from '../../interfaces/component.interfaces';
import { Logger } from '../../utils/logger.util';

/**
 * PatternInstructionsComponent provides contextual help and instructions for the drilling pattern creator.
 * This component is responsible for:
 * - Displaying comprehensive help instructions with keyboard shortcuts
 * - Implementing show/hide functionality with proper accessibility
 * - Supporting keyboard navigation and focus management
 * - Providing responsive instruction layout for different screen sizes
 * - Managing instruction state through reactive patterns
 */
@Component({
  selector: 'app-pattern-instructions',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="instructions-overlay" 
         *ngIf="showInstructions"
         [attr.aria-hidden]="!showInstructions"
         role="dialog"
         aria-labelledby="instructions-title"
         aria-describedby="instructions-content"
         (click)="onOverlayClick($event)">
      
      <div class="instructions-panel" 
           #instructionsPanel
           tabindex="-1"
           (click)="$event.stopPropagation()">
        
        <!-- Header -->
        <div class="instructions-header">
          <h2 id="instructions-title" class="instructions-title">
            Drilling Pattern Creator - Help & Instructions
          </h2>
          <button class="close-button" 
                  type="button"
                  aria-label="Close instructions"
                  (click)="toggleInstructions()"
                  (keydown.enter)="toggleInstructions()"
                  (keydown.space)="toggleInstructions()">
            <span class="close-icon" aria-hidden="true">&times;</span>
          </button>
        </div>

        <!-- Content -->
        <div id="instructions-content" class="instructions-content">
          
          <!-- Quick Start Section -->
          <section class="instruction-section" aria-labelledby="quick-start-title">
            <h3 id="quick-start-title" class="section-title">Quick Start</h3>
            <div class="instruction-list">
              <div class="instruction-item">
                <span class="instruction-step">1.</span>
                <span class="instruction-text">Set your pattern parameters (spacing, burden, depth) in the toolbar</span>
              </div>
              <div class="instruction-item">
                <span class="instruction-step">2.</span>
                <span class="instruction-text">Click "Place Holes" to enter hole placement mode</span>
              </div>
              <div class="instruction-item">
                <span class="instruction-step">3.</span>
                <span class="instruction-text">Click on the canvas to place drill holes at desired locations</span>
              </div>
              <div class="instruction-item">
                <span class="instruction-step">4.</span>
                <span class="instruction-text">Use "Precise Mode" for grid-aligned placement with visual indicators</span>
              </div>
            </div>
          </section>

          <!-- Mouse Controls Section -->
          <section class="instruction-section" aria-labelledby="mouse-controls-title">
            <h3 id="mouse-controls-title" class="section-title">Mouse Controls</h3>
            <div class="controls-grid">
              <div class="control-item">
                <span class="control-action">Left Click</span>
                <span class="control-description">Place drill hole (in placement mode) or select hole</span>
              </div>
              <div class="control-item">
                <span class="control-action">Right Click</span>
                <span class="control-description">Context menu for selected hole options</span>
              </div>
              <div class="control-item">
                <span class="control-action">Drag</span>
                <span class="control-description">Move selected drill hole to new position</span>
              </div>
              <div class="control-item">
                <span class="control-action">Mouse Wheel</span>
                <span class="control-description">Zoom in/out on canvas</span>
              </div>
              <div class="control-item">
                <span class="control-action">Middle Click + Drag</span>
                <span class="control-description">Pan around the canvas</span>
              </div>
            </div>
          </section>

          <!-- Keyboard Shortcuts Section -->
          <section class="instruction-section" aria-labelledby="keyboard-shortcuts-title">
            <h3 id="keyboard-shortcuts-title" class="section-title">Keyboard Shortcuts</h3>
            <div class="shortcuts-grid">
              <div class="shortcut-item">
                <kbd class="shortcut-key">P</kbd>
                <span class="shortcut-description">Toggle hole placement mode</span>
              </div>
              <div class="shortcut-item">
                <kbd class="shortcut-key">G</kbd>
                <span class="shortcut-description">Toggle precise/grid mode</span>
              </div>
              <div class="shortcut-item">
                <kbd class="shortcut-key">F</kbd>
                <span class="shortcut-description">Toggle fullscreen mode</span>
              </div>
              <div class="shortcut-item">
                <kbd class="shortcut-key">H</kbd>
                <span class="shortcut-description">Show/hide help instructions</span>
              </div>
              <div class="shortcut-item">
                <kbd class="shortcut-key">Delete</kbd>
                <span class="shortcut-description">Delete selected drill hole</span>
              </div>
              <div class="shortcut-item">
                <kbd class="shortcut-key">Ctrl + S</kbd>
                <span class="shortcut-description">Save current pattern</span>
              </div>
              <div class="shortcut-item">
                <kbd class="shortcut-key">Ctrl + A</kbd>
                <span class="shortcut-description">Select all drill holes</span>
              </div>
              <div class="shortcut-item">
                <kbd class="shortcut-key">Escape</kbd>
                <span class="shortcut-description">Exit current mode or close dialogs</span>
              </div>
            </div>
          </section>

          <!-- Pattern Settings Section -->
          <section class="instruction-section" aria-labelledby="pattern-settings-title">
            <h3 id="pattern-settings-title" class="section-title">Pattern Settings</h3>
            <div class="settings-list">
              <div class="setting-item">
                <span class="setting-name">Spacing</span>
                <span class="setting-description">Distance between drill holes in the same row (meters)</span>
              </div>
              <div class="setting-item">
                <span class="setting-name">Burden</span>
                <span class="setting-description">Distance between rows of drill holes (meters)</span>
              </div>
              <div class="setting-item">
                <span class="setting-name">Depth</span>
                <span class="setting-description">Default drilling depth for new holes (meters)</span>
              </div>
            </div>
          </section>

          <!-- Tips and Best Practices Section -->
          <section class="instruction-section" aria-labelledby="tips-title">
            <h3 id="tips-title" class="section-title">Tips & Best Practices</h3>
            <div class="tips-list">
              <div class="tip-item">
                <span class="tip-icon" aria-hidden="true">💡</span>
                <span class="tip-text">Use precise mode for accurate grid-aligned hole placement</span>
              </div>
              <div class="tip-item">
                <span class="tip-icon" aria-hidden="true">💡</span>
                <span class="tip-text">Right-click on holes to access individual depth settings</span>
              </div>
              <div class="tip-item">
                <span class="tip-icon" aria-hidden="true">💡</span>
                <span class="tip-text">Save your work frequently using Ctrl+S</span>
              </div>
              <div class="tip-item">
                <span class="tip-icon" aria-hidden="true">💡</span>
                <span class="tip-text">Use the status bar to monitor cursor position and pattern statistics</span>
              </div>
              <div class="tip-item">
                <span class="tip-icon" aria-hidden="true">💡</span>
                <span class="tip-text">Zoom out to see the full pattern before finalizing</span>
              </div>
            </div>
          </section>

        </div>

        <!-- Footer -->
        <div class="instructions-footer">
          <button class="primary-button" 
                  type="button"
                  (click)="toggleInstructions()">
            Got it, let's start!
          </button>
        </div>

      </div>
    </div>
  `,
  styleUrls: ['./pattern-instructions.component.scss']
})
export class PatternInstructionsComponent implements OnInit, OnDestroy, IPatternInstructionsComponent {
  @Input() showInstructions: boolean = false;
  
  @Output() instructionsToggle = new EventEmitter<boolean>();

  // Component lifecycle
  private destroy$ = new Subject<void>();

  constructor(private patternStateService: PatternStateService) { }

  ngOnInit(): void {
    Logger.info('PatternInstructionsComponent initialized');
    this.subscribeToStateChanges();
  }

  ngOnDestroy(): void {
    Logger.info('PatternInstructionsComponent destroying');
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Subscribe to reactive state changes from PatternStateService
   */
  private subscribeToStateChanges(): void {
    this.patternStateService.state$
      .pipe(takeUntil(this.destroy$))
      .subscribe(state => {
        if (state.showInstructions !== this.showInstructions) {
          this.showInstructions = state.showInstructions;
          this.focusInstructionsPanel();
        }
      });
  }

  /**
   * Toggle instructions visibility
   */
  toggleInstructions(): void {
    const newState = !this.showInstructions;
    this.onInstructionsToggle(newState);
  }

  /**
   * Handle instructions toggle event
   */
  onInstructionsToggle(show: boolean): void {
    this.showInstructions = show;
    this.instructionsToggle.emit(show);
    
    // Update state service
    this.patternStateService.setShowInstructions(show);
    
    Logger.debug(`Instructions ${show ? 'shown' : 'hidden'}`);
  }

  /**
   * Handle overlay click to close instructions
   */
  onOverlayClick(_event: Event): void {
    // Close instructions when clicking on overlay background
    this.onInstructionsToggle(false);
  }

  /**
   * Handle keyboard navigation
   */
  @HostListener('document:keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): void {
    if (!this.showInstructions) return;

    switch (event.key) {
      case 'Escape':
        event.preventDefault();
        this.onInstructionsToggle(false);
        break;
      case 'h':
      case 'H':
        if (!event.ctrlKey && !event.altKey && !event.metaKey) {
          event.preventDefault();
          this.onInstructionsToggle(false);
        }
        break;
    }
  }

  /**
   * Focus management for accessibility
   */
  private focusInstructionsPanel(): void {
    // Focus the instructions panel when shown for screen readers
    if (this.showInstructions) {
      setTimeout(() => {
        const panel = document.querySelector('.instructions-panel') as HTMLElement;
        if (panel) {
          panel.focus();
        }
      }, 100);
    }
  }

  /**
   * Get component status for debugging
   */
  getInstructionsInfo(): {
    isVisible: boolean;
    hasKeyboardSupport: boolean;
    isAccessible: boolean;
  } {
    return {
      isVisible: this.showInstructions,
      hasKeyboardSupport: true,
      isAccessible: true
    };
  }
}