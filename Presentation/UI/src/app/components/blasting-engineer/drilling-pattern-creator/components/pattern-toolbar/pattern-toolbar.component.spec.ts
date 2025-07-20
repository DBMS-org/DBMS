import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';

import { PatternToolbarComponent } from './pattern-toolbar.component';
import { DrillPoint, PatternSettings } from '../../models/drill-point.model';
import { ModeToggleEvent, PointActionEvent, PatternActionEvent } from '../../models/pattern-state.model';

describe('PatternToolbarComponent', () => {
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

  describe('Component Initialization', () => {
    it('should create', () => {
      expect(component).toBeTruthy();
    });

    it('should initialize forms with default settings', () => {
      expect(component.settingsForm).toBeDefined();
      expect(component.selectedHoleDepthForm).toBeDefined();
      expect(component.settingsForm.get('spacing')?.value).toBe(mockSettings.spacing);
      expect(component.settingsForm.get('burden')?.value).toBe(mockSettings.burden);
      expect(component.settingsForm.get('depth')?.value).toBe(mockSettings.depth);
    });

    it('should setup form subscriptions', fakeAsync(() => {
      spyOn(component.settingsChange, 'emit');
      
      component.settingsForm.patchValue({
        spacing: 4,
        burden: 3,
        depth: 15
      });
      
      tick(300); // Wait for debounce
      
      expect(component.settingsChange.emit).toHaveBeenCalledWith({
        spacing: 4,
        burden: 3,
        depth: 15
      });
    }));
  });

  describe('Form Validation', () => {
    it('should validate spacing within range', () => {
      const spacingControl = component.settingsForm.get('spacing');
      
      spacingControl?.setValue(0.3);
      expect(spacingControl?.hasError('min')).toBeTruthy();
      
      spacingControl?.setValue(15);
      expect(spacingControl?.hasError('max')).toBeTruthy();
      
      spacingControl?.setValue(5);
      expect(spacingControl?.valid).toBeTruthy();
    });

    it('should validate burden within range', () => {
      const burdenControl = component.settingsForm.get('burden');
      
      burdenControl?.setValue(0.3);
      expect(burdenControl?.hasError('min')).toBeTruthy();
      
      burdenControl?.setValue(15);
      expect(burdenControl?.hasError('max')).toBeTruthy();
      
      burdenControl?.setValue(3);
      expect(burdenControl?.valid).toBeTruthy();
    });

    it('should validate depth within range', () => {
      const depthControl = component.settingsForm.get('depth');
      
      depthControl?.setValue(0.5);
      expect(depthControl?.hasError('min')).toBeTruthy();
      
      depthControl?.setValue(60);
      expect(depthControl?.hasError('max')).toBeTruthy();
      
      depthControl?.setValue(20);
      expect(depthControl?.valid).toBeTruthy();
    });

    it('should validate decimal places', () => {
      const spacingControl = component.settingsForm.get('spacing');
      
      spacingControl?.setValue(3.123);
      expect(spacingControl?.hasError('maxDecimalPlaces')).toBeTruthy();
      
      spacingControl?.setValue(3.12);
      expect(spacingControl?.valid).toBeTruthy();
    });

    it('should require all fields', () => {
      const spacingControl = component.settingsForm.get('spacing');
      const burdenControl = component.settingsForm.get('burden');
      const depthControl = component.settingsForm.get('depth');
      
      spacingControl?.setValue('');
      burdenControl?.setValue('');
      depthControl?.setValue('');
      
      expect(spacingControl?.hasError('required')).toBeTruthy();
      expect(burdenControl?.hasError('required')).toBeTruthy();
      expect(depthControl?.hasError('required')).toBeTruthy();
    });
  });

  describe('Mode Toggle Events', () => {
    it('should emit hole placement mode toggle', () => {
      spyOn(component.modeToggle, 'emit');
      
      component.toggleHolePlacementMode();
      
      expect(component.modeToggle.emit).toHaveBeenCalledWith({
        mode: 'HOLE_PLACEMENT',
        enabled: true
      });
    });

    it('should emit precise mode toggle', () => {
      spyOn(component.modeToggle, 'emit');
      
      component.togglePreciseMode();
      
      expect(component.modeToggle.emit).toHaveBeenCalledWith({
        mode: 'PRECISE',
        enabled: true
      });
    });

    it('should emit fullscreen mode toggle', () => {
      spyOn(component.modeToggle, 'emit');
      
      component.toggleFullscreen();
      
      expect(component.modeToggle.emit).toHaveBeenCalledWith({
        mode: 'FULLSCREEN',
        enabled: true
      });
    });

    it('should not emit events when disabled', () => {
      component.disabled = true;
      spyOn(component.modeToggle, 'emit');
      
      component.toggleHolePlacementMode();
      component.togglePreciseMode();
      component.toggleFullscreen();
      
      expect(component.modeToggle.emit).not.toHaveBeenCalled();
    });
  });

  describe('Point Action Events', () => {
    beforeEach(() => {
      component.selectedPoint = mockDrillPoint;
      component.drillPointsCount = 5;
      fixture.detectChanges();
    });

    it('should emit delete point action', () => {
      spyOn(component.pointAction, 'emit');
      
      component.onDeletePoint();
      
      expect(component.pointAction.emit).toHaveBeenCalledWith({
        action: 'DELETE',
        pointId: mockDrillPoint.id
      });
    });

    it('should emit clear all action', () => {
      spyOn(component.pointAction, 'emit');
      
      component.onClearAll();
      
      expect(component.pointAction.emit).toHaveBeenCalledWith({
        action: 'CLEAR_ALL'
      });
    });

    it('should emit open depth editor action', () => {
      spyOn(component.pointAction, 'emit');
      
      component.openDepthEditor();
      
      expect(component.pointAction.emit).toHaveBeenCalledWith({
        action: 'OPEN_DEPTH_EDITOR'
      });
    });

    it('should not emit delete when no point selected', () => {
      component.selectedPoint = null;
      spyOn(component.pointAction, 'emit');
      
      component.onDeletePoint();
      
      expect(component.pointAction.emit).not.toHaveBeenCalled();
    });

    it('should not emit depth editor when no points exist', () => {
      component.drillPointsCount = 0;
      spyOn(component.pointAction, 'emit');
      
      component.openDepthEditor();
      
      expect(component.pointAction.emit).not.toHaveBeenCalled();
    });
  });

  describe('Pattern Action Events', () => {
    beforeEach(() => {
      component.drillPointsCount = 5;
      fixture.detectChanges();
    });

    it('should emit save pattern action', () => {
      spyOn(component.patternAction, 'emit');
      
      component.onSavePattern();
      
      expect(component.patternAction.emit).toHaveBeenCalledWith({
        action: 'SAVE'
      });
    });

    it('should emit export to blast designer action', () => {
      spyOn(component.patternAction, 'emit');
      
      component.onExportToBlastDesigner();
      
      expect(component.patternAction.emit).toHaveBeenCalledWith({
        action: 'EXPORT_TO_BLAST_DESIGNER'
      });
    });

    it('should not emit actions when no points exist', () => {
      component.drillPointsCount = 0;
      spyOn(component.patternAction, 'emit');
      
      component.onSavePattern();
      component.onExportToBlastDesigner();
      
      expect(component.patternAction.emit).not.toHaveBeenCalled();
    });
  });

  describe('Selected Hole Depth Handling', () => {
    beforeEach(() => {
      component.selectedPoint = mockDrillPoint;
      fixture.detectChanges();
    });

    it('should update selected hole depth form when point changes', () => {
      component.ngOnChanges();
      
      expect(component.selectedHoleDepthForm.get('selectedHoleDepth')?.value)
        .toBe(mockDrillPoint.depth);
    });

    it('should emit point action on depth change', fakeAsync(() => {
      spyOn(component.pointAction, 'emit');
      
      component.selectedHoleDepthForm.get('selectedHoleDepth')?.setValue(15);
      tick(300); // Wait for debounce
      
      expect(component.pointAction.emit).toHaveBeenCalledWith({
        action: 'UPDATE_POINT_DEPTH',
        pointId: mockDrillPoint.id,
        depth: 15
      });
    }));
  });

  describe('Contract Methods', () => {
    it('should reset to defaults', () => {
      spyOn(component.settingsChange, 'emit');
      
      component.resetToDefaults();
      
      expect(component.settingsForm.get('spacing')?.value).toBe(3);
      expect(component.settingsForm.get('burden')?.value).toBe(2.5);
      expect(component.settingsForm.get('depth')?.value).toBe(10);
      expect(component.settingsChange.emit).toHaveBeenCalled();
    });

    it('should validate settings and return errors', () => {
      component.settingsForm.get('spacing')?.setValue(0.3);
      component.settingsForm.get('burden')?.setValue(15);
      
      const result = component.validateSettings();
      
      expect(result.isValid).toBeFalsy();
      expect(result.errors.length).toBeGreaterThan(0);
      expect(result.errors).toContain('Spacing must be at least 0.5');
      expect(result.errors).toContain('Burden must not exceed 10');
    });

    it('should validate settings and return success', () => {
      const result = component.validateSettings();
      
      expect(result.isValid).toBeTruthy();
      expect(result.errors.length).toBe(0);
    });
  });

  describe('Utility Methods', () => {
    it('should format values correctly', () => {
      expect(component.formatValue(5)).toBe('5');
      expect(component.formatValue(5.5)).toBe('5.50');
      expect(component.formatValue(null)).toBe('0.00');
      expect(component.formatValue(undefined)).toBe('0.00');
      expect(component.formatValue(NaN)).toBe('0.00');
    });

    it('should check form control errors', () => {
      const spacingControl = component.settingsForm.get('spacing');
      spacingControl?.setValue(0.3);
      spacingControl?.markAsTouched();
      
      expect(component.hasError('spacing', 'min')).toBeTruthy();
      expect(component.hasError('spacing', 'max')).toBeFalsy();
    });

    it('should get error messages', () => {
      const spacingControl = component.settingsForm.get('spacing');
      spacingControl?.setValue(0.3);
      spacingControl?.markAsTouched();
      
      const errorMessage = component.getErrorMessage('spacing');
      expect(errorMessage).toBe('Spacing must be at least 0.5');
    });
  });

  describe('Template Integration', () => {
    it('should render all toolbar sections', () => {
      const sections = fixture.debugElement.queryAll(By.css('.toolbar-section'));
      expect(sections.length).toBeGreaterThanOrEqual(4);
    });

    it('should show selected hole section when point is selected', () => {
      component.selectedPoint = mockDrillPoint;
      fixture.detectChanges();
      
      const selectedHoleSection = fixture.debugElement.query(By.css('.selected-hole-section'));
      expect(selectedHoleSection).toBeTruthy();
    });

    it('should hide selected hole section when no point is selected', () => {
      component.selectedPoint = null;
      fixture.detectChanges();
      
      const selectedHoleSection = fixture.debugElement.query(By.css('.selected-hole-section'));
      expect(selectedHoleSection).toBeFalsy();
    });

    it('should disable buttons when disabled flag is set', () => {
      component.disabled = true;
      fixture.detectChanges();
      
      const buttons = fixture.debugElement.queryAll(By.css('button'));
      buttons.forEach(button => {
        expect(button.nativeElement.disabled).toBeTruthy();
      });
    });

    it('should show active state for mode buttons', () => {
      component.isHolePlacementMode = true;
      component.isPreciseMode = true;
      fixture.detectChanges();
      
      const buttons = fixture.debugElement.queryAll(By.css('button'));
      const holePlacementButton = buttons.find(btn => 
        btn.nativeElement.getAttribute('aria-label')?.includes('Exit hole placement mode')
      );
      const preciseModeButton = buttons.find(btn => 
        btn.nativeElement.getAttribute('aria-label')?.includes('Exit precise mode')
      );
      
      expect(holePlacementButton).toBeTruthy();
      expect(preciseModeButton).toBeTruthy();
      
      if (holePlacementButton) {
        expect(holePlacementButton.nativeElement.classList.contains('active')).toBeTruthy();
      }
      if (preciseModeButton) {
        expect(preciseModeButton.nativeElement.classList.contains('precise-mode-active')).toBeTruthy();
      }
    });

    it('should show validation errors when form is invalid', () => {
      component.settingsForm.get('spacing')?.setValue(0.3);
      component.settingsForm.get('spacing')?.markAsTouched();
      component.validateSettings();
      fixture.detectChanges();
      
      const errorSection = fixture.debugElement.query(By.css('.validation-errors'));
      expect(errorSection).toBeTruthy();
    });
  });

  describe('Accessibility', () => {
    it('should have proper ARIA labels', () => {
      const toolbar = fixture.debugElement.query(By.css('.pattern-toolbar'));
      expect(toolbar.nativeElement.getAttribute('role')).toBe('toolbar');
      expect(toolbar.nativeElement.getAttribute('aria-label')).toBe('Pattern Creator Tools');
    });

    it('should have proper button states', () => {
      component.isHolePlacementMode = true;
      component.isPreciseMode = true;
      fixture.detectChanges();
      
      const buttons = fixture.debugElement.queryAll(By.css('button'));
      const holePlacementButton = buttons.find(btn => 
        btn.nativeElement.getAttribute('aria-label')?.includes('Exit hole placement mode')
      );
      const preciseModeButton = buttons.find(btn => 
        btn.nativeElement.getAttribute('aria-label')?.includes('Exit precise mode')
      );
      
      expect(holePlacementButton).toBeTruthy();
      expect(preciseModeButton).toBeTruthy();
      
      if (holePlacementButton) {
        expect(holePlacementButton.nativeElement.getAttribute('aria-pressed')).toBe('true');
      }
      if (preciseModeButton) {
        expect(preciseModeButton.nativeElement.classList.contains('precise-mode-active')).toBeTruthy();
      }
    });

    it('should support keyboard navigation', () => {
      const button = fixture.debugElement.query(By.css('button'));
      spyOn(component, 'toggleHolePlacementMode');
      
      button.triggerEventHandler('keydown.enter', {});
      expect(component.toggleHolePlacementMode).toHaveBeenCalled();
    });
  });

  describe('Error Handling', () => {
    it('should handle form errors gracefully', () => {
      const spacingControl = component.settingsForm.get('spacing');
      spacingControl?.setValue('invalid');
      
      expect(() => component.validateSettings()).not.toThrow();
    });

    it('should handle missing form controls', () => {
      expect(component.getFormControl('nonexistent')).toBeNull();
      expect(component.hasError('nonexistent', 'required')).toBeFalsy();
      expect(component.getErrorMessage('nonexistent')).toBe('');
    });
  });
});