import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ViewSettings } from '../../../shared/models/simulation.model';

@Component({
  selector: 'app-view-controls',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="view-controls">
      <div class="zoom-controls">
        <button (click)="onZoomIn()" class="control-btn">
          <i class="material-icons">zoom_in</i>
        </button>
        <button (click)="onZoomOut()" class="control-btn">
          <i class="material-icons">zoom_out</i>
        </button>
        <button (click)="onCenterView()" class="control-btn">
          <i class="material-icons">center_focus_strong</i>
        </button>
      </div>
      
      <div class="pan-controls">
        <button (click)="onPanUp()" class="control-btn">
          <i class="material-icons">arrow_upward</i>
        </button>
        <button (click)="onPanDown()" class="control-btn">
          <i class="material-icons">arrow_downward</i>
        </button>
        <button (click)="onPanLeft()" class="control-btn">
          <i class="material-icons">arrow_back</i>
        </button>
        <button (click)="onPanRight()" class="control-btn">
          <i class="material-icons">arrow_forward</i>
        </button>
      </div>

      <div class="view-settings">
        <button (click)="toggleGrid()" class="control-btn">
          <i class="material-icons">{{viewSettings.showGrid ? 'grid_on' : 'grid_off'}}</i>
        </button>
        <button (click)="toggleHoleDetails()" class="control-btn">
          <i class="material-icons">{{viewSettings.showHoleDetails ? 'info' : 'info_outline'}}</i>
        </button>
        <button (click)="cycleColorTheme()" class="control-btn">
          <i class="material-icons">palette</i>
        </button>
      </div>
    </div>
  `,
  styles: [`
    .view-controls {
      display: flex;
      gap: 1rem;
      padding: 0.5rem;
      background: #fff;
      border-radius: 4px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .zoom-controls,
    .pan-controls,
    .view-settings {
      display: flex;
      gap: 0.5rem;
    }

    .control-btn {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 36px;
      height: 36px;
      border: none;
      background: transparent;
      border-radius: 4px;
      cursor: pointer;
      transition: all 0.2s;
    }

    .control-btn:hover {
      background: #e9ecef;
    }

    .control-btn i {
      font-size: 20px;
    }
  `]
})
export class ViewControlsComponent {
  @Input() viewSettings!: ViewSettings;
  @Output() viewSettingsChange = new EventEmitter<Partial<ViewSettings>>();
  @Output() zoomIn = new EventEmitter<void>();
  @Output() zoomOut = new EventEmitter<void>();
  @Output() centerView = new EventEmitter<void>();
  @Output() panUp = new EventEmitter<void>();
  @Output() panDown = new EventEmitter<void>();
  @Output() panLeft = new EventEmitter<void>();
  @Output() panRight = new EventEmitter<void>();

  onZoomIn(): void {
    this.zoomIn.emit();
  }

  onZoomOut(): void {
    this.zoomOut.emit();
  }

  onCenterView(): void {
    this.centerView.emit();
  }

  onPanUp(): void {
    this.panUp.emit();
  }

  onPanDown(): void {
    this.panDown.emit();
  }

  onPanLeft(): void {
    this.panLeft.emit();
  }

  onPanRight(): void {
    this.panRight.emit();
  }

  toggleGrid(): void {
    this.viewSettingsChange.emit({
      ...this.viewSettings,
      showGrid: !this.viewSettings.showGrid
    });
  }

  toggleHoleDetails(): void {
    this.viewSettingsChange.emit({
      ...this.viewSettings,
      showHoleDetails: !this.viewSettings.showHoleDetails
    });
  }

  cycleColorTheme(): void {
    const themes: Array<'default' | 'dark' | 'high-contrast' | 'colorblind'> = 
      ['default', 'dark', 'high-contrast', 'colorblind'];
    const currentIndex = themes.indexOf(this.viewSettings.colorTheme);
    const nextIndex = (currentIndex + 1) % themes.length;
    
    this.viewSettingsChange.emit({
      ...this.viewSettings,
      colorTheme: themes[nextIndex] as 'default' | 'dark' | 'high-contrast' | 'colorblind'
    });
  }
} 