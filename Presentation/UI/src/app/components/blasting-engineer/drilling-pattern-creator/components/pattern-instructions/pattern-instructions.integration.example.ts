import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { PatternInstructionsComponent } from './pattern-instructions.component';
import { PatternStateService } from '../../services/pattern-state.service';

/**
 * Integration examples showing how to use PatternInstructionsComponent
 * in different scenarios and with various parent components.
 */

// Example 1: Basic Integration with Toolbar
@Component({
  selector: 'app-toolbar-with-instructions-example',
  template: `
    <div class="toolbar-container">
      <!-- Toolbar with help button -->
      <div class="toolbar">
        <button class="help-button" 
                (click)="toggleInstructions()"
                [attr.aria-pressed]="showInstructions"
                title="Show help instructions (H)">
          <span class="help-icon">?</span>
          Help
        </button>
      </div>

      <!-- Instructions component -->
      <app-pattern-instructions
        [showInstructions]="showInstructions"
        (instructionsToggle)="onInstructionsToggle($event)">
      </app-pattern-instructions>
    </div>
  `,
  styles: [`
    .toolbar {
      display: flex;
      padding: 8px;
      background: #f5f5f5;
      border-bottom: 1px solid #ddd;
    }
    
    .help-button {
      display: flex;
      align-items: center;
      gap: 4px;
      padding: 8px 12px;
      background: #2196f3;
      color: white;
      border: none;
      border-radius: 4px;
      cursor: pointer;
    }
    
    .help-button:hover {
      background: #1976d2;
    }
    
    .help-icon {
      font-weight: bold;
      font-size: 14px;
    }
  `],
  imports: [PatternInstructionsComponent]
})
export class ToolbarWithInstructionsExample implements OnInit, OnDestroy {
  showInstructions = false;
  private destroy$ = new Subject<void>();

  constructor(private patternStateService: PatternStateService) {}

  ngOnInit(): void {
    // Subscribe to state changes
    this.patternStateService.state$
      .pipe(takeUntil(this.destroy$))
      .subscribe(state => {
        this.showInstructions = state.showInstructions;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  toggleInstructions(): void {
    this.onInstructionsToggle(!this.showInstructions);
  }

  onInstructionsToggle(show: boolean): void {
    this.showInstructions = show;
    this.patternStateService.setShowInstructions(show);
  }
}

// Example 2: Integration with Keyboard Service
@Component({
  selector: 'app-keyboard-instructions-example',
  template: `
    <div class="canvas-container" 
         tabindex="0" 
         (keydown)="onKeyDown($event)">
      
      <!-- Canvas content -->
      <div class="canvas-content">
        <p>Press H for help, F1 for instructions, or Escape to close</p>
        <p>Instructions visible: {{ showInstructions ? 'Yes' : 'No' }}</p>
      </div>

      <!-- Instructions component -->
      <app-pattern-instructions
        [showInstructions]="showInstructions"
        (instructionsToggle)="onInstructionsToggle($event)">
      </app-pattern-instructions>
    </div>
  `,
  styles: [`
    .canvas-container {
      position: relative;
      width: 100%;
      height: 400px;
      border: 2px solid #ddd;
      outline: none;
    }
    
    .canvas-container:focus {
      border-color: #2196f3;
    }
    
    .canvas-content {
      padding: 20px;
      text-align: center;
    }
  `],
  imports: [PatternInstructionsComponent]
})
export class KeyboardInstructionsExample {
  showInstructions = false;

  onKeyDown(event: KeyboardEvent): void {
    switch (event.key) {
      case 'h':
      case 'H':
        if (!event.ctrlKey && !event.altKey && !event.metaKey) {
          event.preventDefault();
          this.toggleInstructions();
        }
        break;
      case 'F1':
        event.preventDefault();
        this.onInstructionsToggle(true);
        break;
      case 'Escape':
        if (this.showInstructions) {
          event.preventDefault();
          this.onInstructionsToggle(false);
        }
        break;
    }
  }

  toggleInstructions(): void {
    this.onInstructionsToggle(!this.showInstructions);
  }

  onInstructionsToggle(show: boolean): void {
    this.showInstructions = show;
  }
}

// Example 3: Integration with First-Time User Experience
@Component({
  selector: 'app-first-time-user-example',
  template: `
    <div class="app-container">
      <!-- Welcome banner for new users -->
      <div class="welcome-banner" *ngIf="isFirstTimeUser && !showInstructions">
        <div class="welcome-content">
          <h3>Welcome to the Drilling Pattern Creator!</h3>
          <p>New to this tool? Let us show you around.</p>
          <button class="welcome-button" (click)="showWelcomeInstructions()">
            Show Instructions
          </button>
          <button class="skip-button" (click)="skipInstructions()">
            Skip for now
          </button>
        </div>
      </div>

      <!-- Main application content -->
      <div class="main-content">
        <p>Main application content goes here...</p>
        <button (click)="toggleInstructions()">Toggle Help</button>
      </div>

      <!-- Instructions component -->
      <app-pattern-instructions
        [showInstructions]="showInstructions"
        (instructionsToggle)="onInstructionsToggle($event)">
      </app-pattern-instructions>
    </div>
  `,
  styles: [`
    .welcome-banner {
      background: linear-gradient(135deg, #e3f2fd 0%, #bbdefb 100%);
      padding: 20px;
      text-align: center;
      border-bottom: 1px solid #90caf9;
    }
    
    .welcome-content h3 {
      margin: 0 0 10px 0;
      color: #1976d2;
    }
    
    .welcome-content p {
      margin: 0 0 15px 0;
      color: #666;
    }
    
    .welcome-button {
      background: #2196f3;
      color: white;
      border: none;
      padding: 10px 20px;
      border-radius: 4px;
      margin-right: 10px;
      cursor: pointer;
    }
    
    .skip-button {
      background: transparent;
      color: #666;
      border: 1px solid #ddd;
      padding: 10px 20px;
      border-radius: 4px;
      cursor: pointer;
    }
    
    .main-content {
      padding: 20px;
    }
  `],
  imports: [PatternInstructionsComponent]
})
export class FirstTimeUserExample implements OnInit {
  showInstructions = false;
  isFirstTimeUser = true;

  ngOnInit(): void {
    // Check if user has seen instructions before
    this.isFirstTimeUser = !localStorage.getItem('drilling-pattern-instructions-seen');
    
    // Auto-show instructions for first-time users after a delay
    if (this.isFirstTimeUser) {
      setTimeout(() => {
        if (this.isFirstTimeUser && !this.showInstructions) {
          this.showWelcomeInstructions();
        }
      }, 2000);
    }
  }

  showWelcomeInstructions(): void {
    this.onInstructionsToggle(true);
    this.markInstructionsAsSeen();
  }

  skipInstructions(): void {
    this.isFirstTimeUser = false;
    this.markInstructionsAsSeen();
  }

  toggleInstructions(): void {
    this.onInstructionsToggle(!this.showInstructions);
  }

  onInstructionsToggle(show: boolean): void {
    this.showInstructions = show;
    if (show) {
      this.markInstructionsAsSeen();
    }
  }

  private markInstructionsAsSeen(): void {
    localStorage.setItem('drilling-pattern-instructions-seen', 'true');
    this.isFirstTimeUser = false;
  }
}

// Example 4: Integration with Context-Sensitive Help
@Component({
  selector: 'app-context-sensitive-help-example',
  template: `
    <div class="context-container">
      <!-- Different modes with context-sensitive help -->
      <div class="mode-selector">
        <button 
          *ngFor="let mode of modes" 
          [class.active]="currentMode === mode.id"
          (click)="setMode(mode.id)">
          {{ mode.name }}
        </button>
        <button class="help-button" (click)="showContextHelp()">
          Help for {{ getCurrentModeName() }}
        </button>
      </div>

      <!-- Mode-specific content -->
      <div class="mode-content">
        <div [ngSwitch]="currentMode">
          <div *ngSwitchCase="'placement'">
            <h3>Hole Placement Mode</h3>
            <p>Click on the canvas to place drill holes.</p>
          </div>
          <div *ngSwitchCase="'editing'">
            <h3>Hole Editing Mode</h3>
            <p>Select and modify existing drill holes.</p>
          </div>
          <div *ngSwitchCase="'settings'">
            <h3>Pattern Settings Mode</h3>
            <p>Configure pattern parameters.</p>
          </div>
        </div>
      </div>

      <!-- Instructions component with context -->
      <app-pattern-instructions
        [showInstructions]="showInstructions"
        (instructionsToggle)="onInstructionsToggle($event)">
      </app-pattern-instructions>
    </div>
  `,
  styles: [`
    .mode-selector {
      display: flex;
      gap: 10px;
      padding: 10px;
      background: #f5f5f5;
      border-bottom: 1px solid #ddd;
    }
    
    .mode-selector button {
      padding: 8px 16px;
      border: 1px solid #ddd;
      background: white;
      cursor: pointer;
      border-radius: 4px;
    }
    
    .mode-selector button.active {
      background: #2196f3;
      color: white;
      border-color: #2196f3;
    }
    
    .help-button {
      margin-left: auto;
      background: #ff9800 !important;
      color: white !important;
      border-color: #ff9800 !important;
    }
    
    .mode-content {
      padding: 20px;
    }
  `],
  imports: [PatternInstructionsComponent]
})
export class ContextSensitiveHelpExample {
  showInstructions = false;
  currentMode = 'placement';
  
  modes = [
    { id: 'placement', name: 'Place Holes' },
    { id: 'editing', name: 'Edit Holes' },
    { id: 'settings', name: 'Settings' }
  ];

  setMode(modeId: string): void {
    this.currentMode = modeId;
  }

  getCurrentModeName(): string {
    return this.modes.find(m => m.id === this.currentMode)?.name || 'Unknown';
  }

  showContextHelp(): void {
    // Could customize instructions based on current mode
    this.onInstructionsToggle(true);
  }

  onInstructionsToggle(show: boolean): void {
    this.showInstructions = show;
  }
}

// Example 5: Integration with State Management and Analytics
@Component({
  selector: 'app-analytics-instructions-example',
  template: `
    <div class="analytics-container">
      <!-- Analytics dashboard -->
      <div class="analytics-panel">
        <h3>Help Usage Analytics</h3>
        <div class="stats">
          <div class="stat">
            <span class="stat-label">Times Opened:</span>
            <span class="stat-value">{{ analytics.timesOpened }}</span>
          </div>
          <div class="stat">
            <span class="stat-label">Total Time Viewed:</span>
            <span class="stat-value">{{ analytics.totalTimeViewed }}s</span>
          </div>
          <div class="stat">
            <span class="stat-label">Last Opened:</span>
            <span class="stat-value">{{ analytics.lastOpened | date:'short' }}</span>
          </div>
        </div>
        <button (click)="toggleInstructions()">Open Instructions</button>
      </div>

      <!-- Instructions component with analytics -->
      <app-pattern-instructions
        [showInstructions]="showInstructions"
        (instructionsToggle)="onInstructionsToggle($event)">
      </app-pattern-instructions>
    </div>
  `,
  styles: [`
    .analytics-panel {
      padding: 20px;
      background: #f9f9f9;
      border: 1px solid #ddd;
      border-radius: 4px;
      margin: 20px;
    }
    
    .stats {
      display: flex;
      flex-direction: column;
      gap: 10px;
      margin: 15px 0;
    }
    
    .stat {
      display: flex;
      justify-content: space-between;
    }
    
    .stat-label {
      font-weight: 500;
    }
    
    .stat-value {
      color: #2196f3;
      font-weight: 600;
    }
  `],
  imports: [PatternInstructionsComponent]
})
export class AnalyticsInstructionsExample implements OnInit, OnDestroy {
  showInstructions = false;
  private destroy$ = new Subject<void>();
  private instructionsOpenTime: number | null = null;

  analytics = {
    timesOpened: 0,
    totalTimeViewed: 0,
    lastOpened: null as Date | null
  };

  ngOnInit(): void {
    // Load analytics from storage
    this.loadAnalytics();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    
    // Save analytics on component destroy
    this.saveAnalytics();
  }

  toggleInstructions(): void {
    this.onInstructionsToggle(!this.showInstructions);
  }

  onInstructionsToggle(show: boolean): void {
    this.showInstructions = show;
    
    if (show) {
      // Track opening
      this.analytics.timesOpened++;
      this.analytics.lastOpened = new Date();
      this.instructionsOpenTime = Date.now();
    } else if (this.instructionsOpenTime) {
      // Track viewing time
      const viewTime = Math.round((Date.now() - this.instructionsOpenTime) / 1000);
      this.analytics.totalTimeViewed += viewTime;
      this.instructionsOpenTime = null;
    }
    
    this.saveAnalytics();
  }

  private loadAnalytics(): void {
    const stored = localStorage.getItem('instructions-analytics');
    if (stored) {
      const parsed = JSON.parse(stored);
      this.analytics = {
        ...this.analytics,
        ...parsed,
        lastOpened: parsed.lastOpened ? new Date(parsed.lastOpened) : null
      };
    }
  }

  private saveAnalytics(): void {
    localStorage.setItem('instructions-analytics', JSON.stringify(this.analytics));
  }
}

// Export all examples for easy importing
export const PATTERN_INSTRUCTIONS_EXAMPLES = {
  ToolbarWithInstructionsExample,
  KeyboardInstructionsExample,
  FirstTimeUserExample,
  ContextSensitiveHelpExample,
  AnalyticsInstructionsExample
};

/**
 * Usage in a module:
 * 
 * import { PATTERN_INSTRUCTIONS_EXAMPLES } from './pattern-instructions.integration.example';
 * 
 * @NgModule({
 *   declarations: [
 *     ...Object.values(PATTERN_INSTRUCTIONS_EXAMPLES)
 *   ],
 *   // ...
 * })
 * export class ExamplesModule { }
 */