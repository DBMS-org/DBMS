import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { PatternToolbarComponent } from './pattern-toolbar.component';
import { DrillPoint, PatternSettings } from '../../models/drill-point.model';

/**
 * Integration tests for PatternToolbarComponent
 * These tests verify the component works correctly in a real environment
 */
describe('PatternToolbarComponent Integration', () => {
  let component: PatternToolbarComponent;
  let fixture: ComponentFixture<PatternToolbarComponent>;

  const mockSettings: PatternSettings = {
    spacing: 3,
    burden: 2.5,
    depth: 10
  };

  const mockDrillPoint: DrillPoint = {
    id: 'DH001',
    x: 5,
    y: 5,
    depth: 12,
    spacing: 3,
    burden: 2.5
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        PatternToolbarComponent,
        ReactiveFormsModule,
        MatButtonModule,
        MatIconModule,
        MatTooltipModule,
        MatInputModule,
        MatFormFieldModule,
        BrowserAnimationsModule
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PatternToolbarComponent);
    component = fixture.componentInstance;
    
    // Set default inputs
    component.settings = mockSettings;
    component.selectedPoint = null;
    component.drillPointsCount = 0;
    component.isHolePlacementMode = false;
    component.isPreciseMode = false;
    component.isSaved = true;
    component.disabled = false;

    fixture.detectChanges();
  });

  describe('Component Integration', () => {
    it('should create and initialize successfully', () => {
      expect(component).toBeTruthy();
      expect(component.settingsForm).toBeDefined();
      expect(component.selectedHoleDepthForm).toBeDefined();
    });

    it('should render toolbar sections', () => {
      const compiled = fixture.nativeElement;
      const toolbarSections = compiled.querySelectorAll('.toolbar-section');
      
      expect(toolbarSections.length).toBeGreaterThanOrEqual(4);
    });

    it('should handle settings changes', (done) => {
      component.settingsChange.subscribe((settings: PatternSettings) => {
        expect(settings.spacing).toBe(4);
        expect(settings.burden).toBe(3);
        expect(settings.depth).toBe(15);
        done();
      });

      // Simulate form changes
      component.settingsForm.patchValue({
        spacing: 4,
        burden: 3,
        depth: 15
      });
    });

    it('should emit mode toggle events', () => {
      spyOn(component.modeToggle, 'emit');

      component.toggleHolePlacementMode();
      component.togglePreciseMode();
      component.toggleFullscreen();

      expect(component.modeToggle.emit).toHaveBeenCalledTimes(3);
    });

    it('should emit point action events', () => {
      component.selectedPoint = mockDrillPoint;
      component.drillPointsCount = 5;
      spyOn(component.pointAction, 'emit');

      component.onDeletePoint();
      component.onClearAll();
      component.openDepthEditor();

      expect(component.pointAction.emit).toHaveBeenCalledTimes(3);
    });

    it('should emit pattern action events', () => {
      component.drillPointsCount = 5;
      spyOn(component.patternAction, 'emit');

      component.onSavePattern();
      component.onExportToBlastDesigner();

      expect(component.patternAction.emit).toHaveBeenCalledTimes(2);
    });

    it('should validate form inputs correctly', () => {
      const spacingControl = component.settingsForm.get('spacing');
      const burdenControl = component.settingsForm.get('burden');
      const depthControl = component.settingsForm.get('depth');

      // Test invalid values
      spacingControl?.setValue(0.3);
      burdenControl?.setValue(15);
      depthControl?.setValue(60);

      expect(spacingControl?.hasError('min')).toBeTruthy();
      expect(burdenControl?.hasError('max')).toBeTruthy();
      expect(depthControl?.hasError('max')).toBeTruthy();

      // Test valid values
      spacingControl?.setValue(3);
      burdenControl?.setValue(2.5);
      depthControl?.setValue(10);

      expect(spacingControl?.valid).toBeTruthy();
      expect(burdenControl?.valid).toBeTruthy();
      expect(depthControl?.valid).toBeTruthy();
    });

    it('should handle selected point changes', () => {
      component.selectedPoint = mockDrillPoint;
      component.ngOnChanges();

      const selectedHoleDepthControl = component.selectedHoleDepthForm.get('selectedHoleDepth');
      expect(selectedHoleDepthControl?.value).toBe(mockDrillPoint.depth);
    });

    it('should reset to defaults correctly', () => {
      spyOn(component.settingsChange, 'emit');

      component.resetToDefaults();

      expect(component.settingsForm.get('spacing')?.value).toBe(3);
      expect(component.settingsForm.get('burden')?.value).toBe(2.5);
      expect(component.settingsForm.get('depth')?.value).toBe(10);
      expect(component.settingsChange.emit).toHaveBeenCalled();
    });

    it('should validate settings and return results', () => {
      // Test valid settings
      let result = component.validateSettings();
      expect(result.isValid).toBeTruthy();
      expect(result.errors.length).toBe(0);

      // Test invalid settings
      component.settingsForm.get('spacing')?.setValue(0.3);
      component.settingsForm.get('burden')?.setValue(15);

      result = component.validateSettings();
      expect(result.isValid).toBeFalsy();
      expect(result.errors.length).toBeGreaterThan(0);
    });

    it('should format values correctly', () => {
      expect(component.formatValue(5)).toBe('5');
      expect(component.formatValue(5.5)).toBe('5.50');
      expect(component.formatValue(null)).toBe('0.00');
      expect(component.formatValue(undefined)).toBe('0.00');
    });

    it('should handle disabled state correctly', () => {
      component.disabled = true;
      fixture.detectChanges();

      const compiled = fixture.nativeElement;
      const buttons = compiled.querySelectorAll('button');
      const inputs = compiled.querySelectorAll('input');

      // Check that buttons and inputs are disabled
      buttons.forEach((button: HTMLButtonElement) => {
        expect(button.disabled).toBeTruthy();
      });

      inputs.forEach((input: HTMLInputElement) => {
        expect(input.disabled).toBeTruthy();
      });
    });
  });

  describe('Accessibility Integration', () => {
    it('should have proper ARIA attributes', () => {
      const compiled = fixture.nativeElement;
      const toolbar = compiled.querySelector('.pattern-toolbar');
      
      expect(toolbar.getAttribute('role')).toBe('toolbar');
      expect(toolbar.getAttribute('aria-label')).toBe('Pattern Creator Tools');
    });

    it('should have proper form labels and inputs', () => {
      const compiled = fixture.nativeElement;
      const inputs = compiled.querySelectorAll('input[matInput]');
      
      inputs.forEach((input: HTMLInputElement) => {
        expect(input.id).toBeTruthy();
        expect(input.getAttribute('aria-label')).toBeTruthy();
        expect(input.title).toBeTruthy();
        expect(input.placeholder).toBeTruthy();
      });
    });
  });
});