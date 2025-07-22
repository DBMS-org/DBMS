import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  OnDestroy,
  OnChanges,
  ChangeDetectionStrategy,
  inject,
  signal,
  computed
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Subject, takeUntil, debounceTime, distinctUntilChanged } from 'rxjs';

import { PatternToolbarContract } from '../../contracts/component.contracts';
import { DrillPoint, PatternSettings } from '../../models/drill-point.model';
import {
  ModeToggleEvent,
  PointActionEvent,
  PatternActionEvent
} from '../../models/pattern-state.model';
import { PatternEventBusService } from '../../services/pattern-event-bus.service';
import { UnifiedDrillDataService } from '../../../../../core/services/unified-drill-data.service';
import { NotificationService } from '../../../../../core/services/notification.service';

/**
 * Standalone toolbar component for drilling pattern creator
 * Handles all toolbar functionality including settings, modes, and actions
 */
@Component({
  selector: 'app-pattern-toolbar',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatInputModule,
    MatFormFieldModule
  ],
  templateUrl: './pattern-toolbar.component.html',
  styleUrls: ['./pattern-toolbar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PatternToolbarComponent implements OnInit, OnDestroy, OnChanges, PatternToolbarContract {
  private readonly eventBus = inject(PatternEventBusService);
  private readonly fb = inject(FormBuilder);
  private readonly unifiedDrillDataService = inject(UnifiedDrillDataService);
  private readonly notificationService = inject(NotificationService);
  private readonly destroy$ = new Subject<void>();

  // Current context
  private currentProjectId: number = 0;
  private currentSiteId: number = 0;

  // Inputs
  @Input() settings: PatternSettings = { spacing: 3, burden: 2.5, depth: 10 };
  @Input() selectedPoint: DrillPoint | null = null;
  @Input() drillPointsCount: number = 0;
  @Input() isHolePlacementMode: boolean = false;
  @Input() isPreciseMode: boolean = false;
  @Input() isSaved: boolean = true;
  @Input() disabled?: boolean = false;

  // Outputs
  @Output() settingsChange = new EventEmitter<PatternSettings>();
  @Output() modeToggle = new EventEmitter<ModeToggleEvent>();
  @Output() pointAction = new EventEmitter<PointActionEvent>();
  @Output() patternAction = new EventEmitter<PatternActionEvent>();
  
  // Reactive state
  private _settings = signal<PatternSettings>({ spacing: 3, burden: 2.5, depth: 10 });
  private _selectedPoint = signal<DrillPoint | null>(null);
  private _drillPointsCount = signal<number>(0);
  private _isHolePlacementMode = signal<boolean>(false);
  private _isPreciseMode = signal<boolean>(false);
  private _isSaved = signal<boolean>(true);
  private _disabled = signal<boolean>(false);
  validationErrors = signal<string[]>([]);

  // Form controls
  public settingsForm!: FormGroup;
  public selectedHoleDepthForm!: FormGroup;

  // Computed properties
  hasValidationErrors = computed(() => this.validationErrors().length > 0);
  
  // Signal getters for template use
  getSettings = () => this._settings();
  getSelectedPoint = () => this._selectedPoint();
  getDrillPointsCount = () => this._drillPointsCount();
  getIsHolePlacementMode = () => this._isHolePlacementMode();
  getIsPreciseMode = () => this._isPreciseMode();
  getIsSaved = () => this._isSaved();
  getDisabled = () => this._disabled();

  ngOnInit(): void {
    this.initializeForms();
    this.setupFormSubscriptions();
    this.getCurrentContext();
  }

  /**
   * Get current project and site context
   */
  private getCurrentContext(): void {
    const context = this.unifiedDrillDataService.getCurrentContext();
    this.currentProjectId = context.projectId;
    this.currentSiteId = context.siteId;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Initialize reactive forms for pattern settings
   */
  private initializeForms(): void {
    // Main pattern settings form
    this.settingsForm = this.fb.group({
      spacing: [
        this.settings.spacing,
        [
          Validators.required,
          Validators.min(0.5),
          Validators.max(10),
          this.decimalPlacesValidator(2)
        ]
      ],
      burden: [
        this.settings.burden,
        [
          Validators.required,
          Validators.min(0.5),
          Validators.max(10),
          this.decimalPlacesValidator(2)
        ]
      ],
      depth: [
        this.settings.depth,
        [
          Validators.required,
          Validators.min(1),
          Validators.max(50),
          this.decimalPlacesValidator(2)
        ]
      ]
    });

    // Selected hole depth form
    this.selectedHoleDepthForm = this.fb.group({
      selectedHoleDepth: [
        this.selectedPoint?.depth || this.settings.depth,
        [
          Validators.required,
          Validators.min(1),
          Validators.max(50),
          this.decimalPlacesValidator(2)
        ]
      ]
    });
    
    // Initialize signals with input values
    this._settings.set(this.settings);
    this._selectedPoint.set(this.selectedPoint);
    this._drillPointsCount.set(this.drillPointsCount);
    this._isHolePlacementMode.set(this.isHolePlacementMode);
    this._isPreciseMode.set(this.isPreciseMode);
    this._isSaved.set(this.isSaved);
    this._disabled.set(this.disabled || false);
  }

  /**
   * Setup form subscriptions for reactive updates
   */
  private setupFormSubscriptions(): void {
    // Settings form changes
    this.settingsForm.valueChanges
      .pipe(
        takeUntil(this.destroy$),
        debounceTime(300),
        distinctUntilChanged((prev, curr) =>
          JSON.stringify(prev) === JSON.stringify(curr)
        )
      )
      .subscribe(value => {
        if (this.settingsForm.valid) {
          this.emitSettingsChange(value);
        }
      });

    // Selected hole depth changes
    this.selectedHoleDepthForm.get('selectedHoleDepth')?.valueChanges
      .pipe(
        takeUntil(this.destroy$),
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(depth => {
        if (this._selectedPoint() && this.selectedHoleDepthForm.valid) {
          this.onSelectedHoleDepthChange(depth);
        }
      });
  }

  /**
   * Custom validator for decimal places
   */
  private decimalPlacesValidator(maxDecimalPlaces: number) {
    return (control: any) => {
      if (control.value === null || control.value === undefined || control.value === '') {
        return null;
      }

      const value = control.value.toString();
      const decimalIndex = value.indexOf('.');

      if (decimalIndex !== -1) {
        const decimalPlaces = value.length - decimalIndex - 1;
        if (decimalPlaces > maxDecimalPlaces) {
          return { maxDecimalPlaces: { actual: decimalPlaces, max: maxDecimalPlaces } };
        }
      }

      return null;
    };
  }

  /**
   * Update forms when inputs change
   */
  ngOnChanges(): void {
    // Update signals with the latest input values
    this._settings.set(this.settings);
    this._selectedPoint.set(this.selectedPoint);
    this._drillPointsCount.set(this.drillPointsCount);
    this._isHolePlacementMode.set(this.isHolePlacementMode);
    this._isPreciseMode.set(this.isPreciseMode);
    this._isSaved.set(this.isSaved);
    this._disabled.set(this.disabled || false);

    if (this.settingsForm) {
      this.settingsForm.patchValue(this.settings, { emitEvent: false });

      // Handle disabled state through FormControl instead of HTML attribute
      if (this.disabled) {
        this.settingsForm.disable({ emitEvent: false });
      } else {
        this.settingsForm.enable({ emitEvent: false });
      }
    }

    if (this.selectedHoleDepthForm && this.selectedPoint) {
      this.selectedHoleDepthForm.patchValue({
        selectedHoleDepth: this.selectedPoint?.depth
      }, { emitEvent: false });

      // Handle disabled state for selected hole form
      if (this.disabled) {
        this.selectedHoleDepthForm.disable({ emitEvent: false });
      } else {
        this.selectedHoleDepthForm.enable({ emitEvent: false });
      }
    }
  }

  /**
   * Emit settings change event
   */
  private emitSettingsChange(settings: PatternSettings): void {
    this.eventBus.emit({ type: 'SETTINGS_CHANGE', payload: settings });
    this.settingsChange.emit(settings);
  }

  public toggleHolePlacementMode(): void {
    if (this._disabled()) return;
    const payload: ModeToggleEvent = { 
      mode: 'HOLE_PLACEMENT', 
      enabled: !this._isHolePlacementMode() 
    };
    this.eventBus.emit({ type: 'MODE_TOGGLE', payload });
    this.modeToggle.emit(payload);
  }

  public togglePreciseMode(): void {
    if (this._disabled()) return;
    const payload: ModeToggleEvent = { 
      mode: 'PRECISE', 
      enabled: !this._isPreciseMode() 
    };
    this.eventBus.emit({ type: 'MODE_TOGGLE', payload });
    this.modeToggle.emit(payload);
  }

  public toggleFullscreen(): void {
    if (this._disabled()) return;
    const payload: ModeToggleEvent = { 
      mode: 'FULLSCREEN', 
      enabled: true 
    };
    this.eventBus.emit({ type: 'MODE_TOGGLE', payload });
    this.modeToggle.emit(payload);
  }
  // Update other action methods similarly to use eventBus.emit instead of this.xxx.emit
  /**
   * Handle point actions
   */
  public onDeletePoint(): void {
    if (this._disabled() || !this._selectedPoint()) return;

    this.pointAction.emit({
      action: 'DELETE',
      pointId: this._selectedPoint()!.id
    });
  }

  /**
   * Clear all drill points from both UI and database
   */
  public onClearAll(): void {
    if (this._disabled()) return;

    // First check if we have a valid context
    if (!this.currentProjectId || !this.currentSiteId) {
      this.getCurrentContext();

      if (!this.currentProjectId || !this.currentSiteId) {
        this.notificationService.showError('Cannot clear points: Missing project or site context');
        return;
      }
    }

    // Confirm with the user before proceeding
    if (confirm('Are you sure you want to clear all drill points? This will delete them from the database.')) {
      // Clear points from database
      this.unifiedDrillDataService.clearAllDrillPoints(this.currentProjectId, this.currentSiteId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (success) => {
            if (success) {
              // Clear points from UI state
              this.pointAction.emit({
                action: 'CLEAR_ALL'
              });

              this.notificationService.showSuccess('All drill points have been cleared from the database');
            } else {
              this.notificationService.showError('Failed to clear drill points from the database');
            }
          },
          error: (error) => {
            console.error('Error clearing drill points:', error);
            this.notificationService.showError('An error occurred while clearing drill points');
          }
        });
    }
  }

  public openDepthEditor(): void {
    if (this._disabled() || this._drillPointsCount() === 0) return;

    this.pointAction.emit({
      action: 'OPEN_DEPTH_EDITOR'
    });
  }

  /**
   * Handle pattern actions
   */
  public onSavePattern(): void {
    if (this._disabled() || this._drillPointsCount() === 0) return;

    this.patternAction.emit({
      action: 'SAVE'
    });
  }

  public onExportToBlastDesigner(): void {
    if (this._disabled() || this._drillPointsCount() === 0) return;

    this.patternAction.emit({
      action: 'EXPORT_TO_BLAST_DESIGNER'
    });
  }

  /**
   * Handle selected hole depth change
   */
  private onSelectedHoleDepthChange(depth: number): void {
    if (!this._selectedPoint()) return;

    // Emit a point update through settings change
    // This maintains the existing pattern of updating through settings
    const updatedSettings = { ...this._settings() };
    this.settingsChange.emit(updatedSettings);

    // Also emit a specific point action for individual depth change
    this.pointAction.emit({
      action: 'UPDATE_POINT_DEPTH',
      pointId: this._selectedPoint()!.id,
      depth
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

  /**
   * Contract method: Reset to default values
   */
  public resetToDefaults(): void {
    const defaultSettings: PatternSettings = {
      spacing: 3,
      burden: 2.5,
      depth: 10
    };

    this.settingsForm.patchValue(defaultSettings);
    this.emitSettingsChange(defaultSettings);
  }

  /**
   * Contract method: Validate current settings
   */
  public validateSettings(): { isValid: boolean; errors: string[] } {
    const errors: string[] = [];

    if (this.settingsForm.invalid) {
      const controls = this.settingsForm.controls;

      Object.keys(controls).forEach(key => {
        const control = controls[key];
        if (control.invalid) {
          const fieldName = key.charAt(0).toUpperCase() + key.slice(1);

          if (control.errors?.['required']) {
            errors.push(`${fieldName} is required`);
          }
          if (control.errors?.['min']) {
            errors.push(`${fieldName} must be at least ${control.errors['min'].min}`);
          }
          if (control.errors?.['max']) {
            errors.push(`${fieldName} must not exceed ${control.errors['max'].max}`);
          }
          if (control.errors?.['maxDecimalPlaces']) {
            errors.push(`${fieldName} can have at most ${control.errors['maxDecimalPlaces'].max} decimal places`);
          }
        }
      });
    }

    this.validationErrors.set(errors);
    return {
      isValid: errors.length === 0,
      errors
    };
  }

  /**
   * Get form control for template access
   */
  public getFormControl(controlName: string): FormControl<any> {
    const control = this.settingsForm.get(controlName);
    if (!control) {
      throw new Error(`Form control ${controlName} not found`);
    }
    return control as FormControl<any>;
  }

  /**
   * Get selected hole form control
   */
  public getSelectedHoleControl(): FormControl<any> {
    const control = this.selectedHoleDepthForm.get('selectedHoleDepth');
    if (!control) {
      throw new Error('Selected hole depth control not found');
    }
    return control as FormControl<any>;
  }

  /**
   * Check if a form control has a specific error
   */
  public hasError(controlName: string, errorType: string): boolean {
    const control = this.settingsForm.get(controlName);
    return control ? control.hasError(errorType) && control.touched : false;
  }

  /**
   * Get error message for a form control
   */
  public getErrorMessage(controlName: string): string {
    const control = this.settingsForm.get(controlName);
    if (!control || !control.errors || !control.touched) {
      return '';
    }

    const errors = control.errors;
    const fieldName = controlName.charAt(0).toUpperCase() + controlName.slice(1);

    if (errors['required']) {
      return `${fieldName} is required`;
    }
    if (errors['min']) {
      return `${fieldName} must be at least ${errors['min'].min}`;
    }
    if (errors['max']) {
      return `${fieldName} must not exceed ${errors['max'].max}`;
    }
    if (errors['maxDecimalPlaces']) {
      return `${fieldName} can have at most ${errors['maxDecimalPlaces'].max} decimal places`;
    }

    return '';
  }
}