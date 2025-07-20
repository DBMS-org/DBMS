import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BehaviorSubject } from 'rxjs';
import { PatternStatusBarComponent } from './pattern-status-bar.component';
import { PatternStateService } from '../../services/pattern-state.service';
import { DrillPoint, PatternSettings } from '../../models/drill-point.model';

/**
 * Demo test suite for PatternStatusBarComponent
 * These tests demonstrate the component's capabilities and serve as living documentation
 */
describe('PatternStatusBarComponent - Demo Tests', () => {
  let component: PatternStatusBarComponent;
  let fixture: ComponentFixture<PatternStatusBarComponent>;
  let mockPatternStateService: jasmine.SpyObj<PatternStateService>;

  // Demo data
  const demoSettings: PatternSettings = {
    spacing: 3.5,
    burden: 2.8,
    depth: 12.5
  };

  const demoDrillPoints: DrillPoint[] = [
    {
      id: 'demo-point-001',
      x: 10.5,
      y: 15.2,
      depth: 12.5,
      spacing: 3.5,
      burden: 2.8
    },
    {
      id: 'demo-point-002',
      x: 14.0,
      y: 15.2,
      depth: 10.0, // Custom depth
      spacing: 3.5,
      burden: 2.8
    },
    {
      id: 'demo-point-003',
      x: 17.5,
      y: 15.2,
      depth: 12.5,
      spacing: 3.5,
      burden: 2.8
    }
  ];

  beforeEach(async () => {
    const mockState$ = new BehaviorSubject({
      cursorPosition: { x: 25.75, y: 18.33 },
      isSaved: false
    });
    const mockDrillPoints$ = new BehaviorSubject(demoDrillPoints);
    const mockSelectedPoint$ = new BehaviorSubject(demoDrillPoints[1]);
    const mockSettings$ = new BehaviorSubject(demoSettings);

    mockPatternStateService = jasmine.createSpyObj('PatternStateService', [], {
      state$: mockState$,
      drillPoints$: mockDrillPoints$,
      selectedPoint$: mockSelectedPoint$,
      settings$: mockSettings$
    });

    await TestBed.configureTestingModule({
      imports: [PatternStatusBarComponent],
      providers: [
        { provide: PatternStateService, useValue: mockPatternStateService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PatternStatusBarComponent);
    component = fixture.componentInstance;
  });

  describe('Demo Scenario 1: Active Drilling Session', () => {
    it('should display comprehensive status for an active drilling session', () => {
      // Setup active session state
      component.cursorPosition = { x: 25.75, y: 18.33 };
      component.scale = 1.5;
      component.isHolePlacementMode = true;
      component.isPreciseMode = true;
      component.isFullscreen = false;
      component.drillPoints = demoDrillPoints;
      component.selectedPoint = demoDrillPoints[1];
      component.settings = demoSettings;
      component.isSaved = false;

      component.ngOnInit();
      fixture.detectChanges();

      // Verify all status sections are displayed correctly
      const statusBar = fixture.nativeElement.querySelector('.status-bar');
      expect(statusBar).toBeTruthy();

      // Check cursor position display
      const cursorSection = fixture.nativeElement.querySelector('.cursor-position');
      expect(cursorSection.textContent).toContain('X: 25.75m, Y: 18.33m');

      // Check scale display
      const scaleSection = fixture.nativeElement.querySelector('.scale-info');
      expect(scaleSection.textContent).toContain('150%');
      expect(scaleSection.textContent).toContain('Medium');

      // Check active modes
      const modeIndicators = fixture.nativeElement.querySelectorAll('.mode-indicator.active');
      expect(modeIndicators.length).toBe(2); // Place and Precise modes active

      // Check pattern statistics
      const statsSection = fixture.nativeElement.querySelector('.pattern-stats');
      expect(statsSection.textContent).toContain('Points: 3');
      expect(statsSection.textContent).toContain('Selected: ID: demo-poi...');

      // Check pattern settings
      const settingsSection = fixture.nativeElement.querySelector('.pattern-settings');
      expect(settingsSection.textContent).toContain('Spacing: 3.5m');
      expect(settingsSection.textContent).toContain('Burden: 2.8m');
      expect(settingsSection.textContent).toContain('Depth: 12.5m');

      // Check unsaved status
      const saveIndicator = fixture.nativeElement.querySelector('.save-indicator.unsaved');
      expect(saveIndicator).toBeTruthy();
      expect(saveIndicator.textContent).toContain('Modified');
    });
  });

  describe('Demo Scenario 2: Fullscreen Presentation Mode', () => {
    it('should adapt display for fullscreen presentation mode', () => {
      // Setup fullscreen presentation state
      component.cursorPosition = null; // No cursor tracking in presentation
      component.scale = 0.75; // Zoomed out to show full pattern
      component.isHolePlacementMode = false;
      component.isPreciseMode = false;
      component.isFullscreen = true;
      component.drillPoints = demoDrillPoints;
      component.selectedPoint = null;
      component.settings = demoSettings;
      component.isSaved = true;

      component.ngOnInit();
      fixture.detectChanges();

      // Verify fullscreen styling
      const statusBar = fixture.nativeElement.querySelector('.status-bar.fullscreen');
      expect(statusBar).toBeTruthy();

      // Check that cursor position is not displayed
      const cursorSection = fixture.nativeElement.querySelector('.cursor-position');
      expect(cursorSection).toBeFalsy();

      // Check scale display for zoomed out view
      const scaleSection = fixture.nativeElement.querySelector('.scale-info');
      expect(scaleSection.textContent).toContain('75%');
      expect(scaleSection.textContent).toContain('Low');

      // Check only fullscreen mode is active
      const activeModes = fixture.nativeElement.querySelectorAll('.mode-indicator.active');
      expect(activeModes.length).toBe(1);
      expect(activeModes[0].textContent).toContain('Full');

      // Check no selection
      const statsSection = fixture.nativeElement.querySelector('.pattern-stats');
      expect(statsSection.textContent).toContain('Selected: None');

      // Check saved status
      const saveIndicator = fixture.nativeElement.querySelector('.save-indicator.saved');
      expect(saveIndicator).toBeTruthy();
      expect(saveIndicator.textContent).toContain('Saved');
    });
  });

  describe('Demo Scenario 3: Empty Pattern State', () => {
    it('should handle empty pattern gracefully', () => {
      // Setup empty pattern state
      component.cursorPosition = null;
      component.scale = 1.0;
      component.isHolePlacementMode = false;
      component.isPreciseMode = false;
      component.isFullscreen = false;
      component.drillPoints = [];
      component.selectedPoint = null;
      component.settings = demoSettings;
      component.isSaved = true;

      component.ngOnInit();
      fixture.detectChanges();

      // Check empty pattern statistics
      const statsSection = fixture.nativeElement.querySelector('.pattern-stats');
      expect(statsSection.textContent).toContain('Points: 0');
      expect(statsSection.textContent).toContain('Selected: None');

      // Check no active modes
      const activeModes = fixture.nativeElement.querySelectorAll('.mode-indicator.active');
      expect(activeModes.length).toBe(0);

      // Check normal scale
      const scaleSection = fixture.nativeElement.querySelector('.scale-info');
      expect(scaleSection.textContent).toContain('100%');
      expect(scaleSection.textContent).toContain('Normal');
    });
  });

  describe('Demo Scenario 4: High Zoom Detail Work', () => {
    it('should display appropriate information for detailed work at high zoom', () => {
      // Setup high zoom detail work state
      component.cursorPosition = { x: 12.345, y: 16.789 };
      component.scale = 2.5; // High zoom for detail work
      component.isHolePlacementMode = true;
      component.isPreciseMode = true;
      component.isFullscreen = false;
      component.drillPoints = demoDrillPoints;
      component.selectedPoint = demoDrillPoints[0];
      component.settings = demoSettings;
      component.isSaved = false;

      component.ngOnInit();
      fixture.detectChanges();

      // Check high precision cursor display
      const cursorSection = fixture.nativeElement.querySelector('.cursor-position');
      expect(cursorSection.textContent).toContain('X: 12.35m, Y: 16.79m');

      // Check high zoom level
      const scaleSection = fixture.nativeElement.querySelector('.scale-info');
      expect(scaleSection.textContent).toContain('250%');
      expect(scaleSection.textContent).toContain('High');

      // Check both precision modes active
      const activeModes = fixture.nativeElement.querySelectorAll('.mode-indicator.active');
      expect(activeModes.length).toBe(2);

      // Verify precise mode is indicated
      const preciseModeIndicator = Array.from(activeModes as NodeListOf<HTMLElement>).find(
        (el: HTMLElement) => el.textContent?.includes('Precise')
      );
      expect(preciseModeIndicator).toBeTruthy();
    });
  });

  describe('Demo Scenario 5: Mobile Responsive Display', () => {
    it('should adapt for mobile display constraints', () => {
      // Setup mobile-like state
      component.cursorPosition = { x: 15.0, y: 20.0 };
      component.scale = 1.2;
      component.isHolePlacementMode = true;
      component.isPreciseMode = false;
      component.isFullscreen = false;
      component.drillPoints = demoDrillPoints.slice(0, 2); // Fewer points
      component.selectedPoint = demoDrillPoints[0];
      component.settings = demoSettings;
      component.isSaved = true;

      component.ngOnInit();
      fixture.detectChanges();

      // Simulate mobile viewport
      Object.defineProperty(window, 'innerWidth', {
        writable: true,
        configurable: true,
        value: 480
      });

      // The component should still display all essential information
      const statusBar = fixture.nativeElement.querySelector('.status-bar');
      expect(statusBar).toBeTruthy();

      // Essential sections should be present
      expect(fixture.nativeElement.querySelector('.cursor-position')).toBeTruthy();
      expect(fixture.nativeElement.querySelector('.scale-info')).toBeTruthy();
      expect(fixture.nativeElement.querySelector('.mode-indicators')).toBeTruthy();
      expect(fixture.nativeElement.querySelector('.pattern-stats')).toBeTruthy();
      expect(fixture.nativeElement.querySelector('.save-status')).toBeTruthy();
    });
  });

  describe('Demo Scenario 6: Real-time State Updates', () => {
    it('should demonstrate reactive state updates', (done) => {
      component.ngOnInit();
      fixture.detectChanges();

      // Initial state
      expect(component.drillPointCount).toBe(3);
      expect(component.selectedPointInfo).toContain('demo-poi');

      // Simulate state service updates
      const newPoints = [...demoDrillPoints, {
        id: 'demo-point-004',
        x: 21.0,
        y: 15.2,
        depth: 12.5,
        spacing: 3.5,
        burden: 2.8
      }];

      // Update through state service mock
      (mockPatternStateService.drillPoints$ as BehaviorSubject<DrillPoint[]>).next(newPoints);

      // Allow for async updates
      setTimeout(() => {
        expect(component.drillPoints.length).toBe(4);
        expect(component.drillPointCount).toBe(4);
        done();
      }, 10);
    });
  });

  describe('Demo Scenario 7: Accessibility Features', () => {
    it('should demonstrate accessibility features', () => {
      component.isHolePlacementMode = true;
      component.isPreciseMode = false;
      component.isFullscreen = true;
      component.isSaved = false;

      component.ngOnInit();
      fixture.detectChanges();

      // Check ARIA attributes
      const statusBar = fixture.nativeElement.querySelector('.status-bar');
      expect(statusBar.getAttribute('role')).toBe('status');
      expect(statusBar.getAttribute('aria-live')).toBe('polite');

      // Check mode indicator labels
      const modeIndicators = fixture.nativeElement.querySelectorAll('.mode-indicator');
      expect(modeIndicators[0].getAttribute('aria-label')).toContain('Hole placement mode active');
      expect(modeIndicators[1].getAttribute('aria-label')).toContain('Precise mode inactive');
      expect(modeIndicators[2].getAttribute('aria-label')).toContain('Fullscreen mode active');

      // Check save status label
      const saveIndicator = fixture.nativeElement.querySelector('.save-indicator');
      expect(saveIndicator.getAttribute('aria-label')).toContain('Pattern has unsaved changes');
    });
  });

  describe('Demo Scenario 8: Performance Monitoring', () => {
    it('should provide performance monitoring capabilities', () => {
      component.ngOnInit();

      // Get initial status info
      const statusInfo = component.getStatusInfo();

      expect(statusInfo).toEqual({
        drillPointCount: 3,
        selectedPointId: 'demo-point-002',
        scale: 1,
        modes: {
          holePlacement: false,
          precise: false,
          fullscreen: false
        },
        isSaved: true
      });

      // Verify formatting methods work correctly
      expect(component.formatCoordinate(12.3456789)).toBe('12.35');
      expect(component.formatScale(1.75)).toBe('175');
      expect(component.formatZoomLevel(1.75)).toBe('Medium');
      expect(component.formatValue(3.14159)).toBe('3.1');
    });
  });
});