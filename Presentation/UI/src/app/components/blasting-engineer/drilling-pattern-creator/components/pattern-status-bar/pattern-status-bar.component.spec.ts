import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BehaviorSubject } from 'rxjs';
import { PatternStatusBarComponent } from './pattern-status-bar.component';
import { PatternStateService } from '../../services/pattern-state.service';
import { DrillPoint, PatternSettings } from '../../models/drill-point.model';

describe('PatternStatusBarComponent', () => {
  let component: PatternStatusBarComponent;
  let fixture: ComponentFixture<PatternStatusBarComponent>;
  let mockPatternStateService: jasmine.SpyObj<PatternStateService>;
  let mockState$: BehaviorSubject<any>;
  let mockDrillPoints$: BehaviorSubject<DrillPoint[]>;
  let mockSelectedPoint$: BehaviorSubject<DrillPoint | null>;
  let mockSettings$: BehaviorSubject<PatternSettings>;

  const mockDrillPoint: DrillPoint = {
    id: 'test-point-123',
    x: 10.5,
    y: 15.2,
    depth: 12.0,
    spacing: 3.0,
    burden: 2.5
  };

  const mockSettings: PatternSettings = {
    spacing: 3.0,
    burden: 2.5,
    depth: 10.0
  };

  beforeEach(async () => {
    // Create mock observables
    mockState$ = new BehaviorSubject({
      cursorPosition: { x: 5.5, y: 8.2 },
      isSaved: true
    });
    mockDrillPoints$ = new BehaviorSubject<DrillPoint[]>([]);
    mockSelectedPoint$ = new BehaviorSubject<DrillPoint | null>(null);
    mockSettings$ = new BehaviorSubject<PatternSettings>(mockSettings);

    // Create mock service
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

  afterEach(() => {
    fixture.destroy();
  });

  describe('Component Initialization', () => {
    it('should create', () => {
      expect(component).toBeTruthy();
    });

    it('should initialize with default values', () => {
      expect(component.cursorPosition).toBeNull();
      expect(component.scale).toBe(1);
      expect(component.isHolePlacementMode).toBeFalse();
      expect(component.isPreciseMode).toBeFalse();
      expect(component.isFullscreen).toBeFalse();
      expect(component.drillPoints).toEqual([]);
      expect(component.selectedPoint).toBeNull();
      expect(component.settings).toBeNull();
      expect(component.isSaved).toBeTrue();
    });

    it('should subscribe to state changes on init', () => {
      component.ngOnInit();
      
      // Verify subscriptions are active by emitting new values
      mockState$.next({
        cursorPosition: { x: 10.0, y: 20.0 },
        isSaved: false
      });

      expect(component.cursorPosition).toEqual({ x: 10.0, y: 20.0 });
      expect(component.isSaved).toBeFalse();
    });

    it('should update computed properties on init', () => {
      component.drillPoints = [mockDrillPoint];
      component.selectedPoint = mockDrillPoint;
      
      component.ngOnInit();
      
      expect(component.drillPointCount).toBe(1);
      expect(component.selectedPointInfo).toBe('ID: test-poi...');
    });
  });

  describe('Input Changes', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should update computed properties when drill points change', () => {
      const changes = {
        drillPoints: {
          currentValue: [mockDrillPoint],
          previousValue: [],
          firstChange: false,
          isFirstChange: () => false
        }
      };

      component.drillPoints = [mockDrillPoint];
      component.ngOnChanges(changes);

      expect(component.drillPointCount).toBe(1);
    });

    it('should update selected point info when selected point changes', () => {
      const changes = {
        selectedPoint: {
          currentValue: mockDrillPoint,
          previousValue: null,
          firstChange: false,
          isFirstChange: () => false
        }
      };

      component.selectedPoint = mockDrillPoint;
      component.ngOnChanges(changes);

      expect(component.selectedPointInfo).toBe('ID: test-poi...');
    });

    it('should handle null selected point', () => {
      const changes = {
        selectedPoint: {
          currentValue: null,
          previousValue: mockDrillPoint,
          firstChange: false,
          isFirstChange: () => false
        }
      };

      component.selectedPoint = null;
      component.ngOnChanges(changes);

      expect(component.selectedPointInfo).toBe('None');
    });
  });

  describe('Reactive State Updates', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should update cursor position from state service', () => {
      const newPosition = { x: 25.5, y: 30.8 };
      
      mockState$.next({
        cursorPosition: newPosition,
        isSaved: true
      });

      expect(component.cursorPosition).toEqual(newPosition);
    });

    it('should update save status from state service', () => {
      mockState$.next({
        cursorPosition: null,
        isSaved: false
      });

      expect(component.isSaved).toBeFalse();
    });

    it('should update drill points from state service', () => {
      const newPoints = [mockDrillPoint];
      
      mockDrillPoints$.next(newPoints);

      expect(component.drillPoints).toEqual(newPoints);
      expect(component.drillPointCount).toBe(1);
    });

    it('should update selected point from state service', () => {
      mockSelectedPoint$.next(mockDrillPoint);

      expect(component.selectedPoint).toEqual(mockDrillPoint);
      expect(component.selectedPointInfo).toBe('ID: test-poi...');
    });

    it('should update settings from state service', () => {
      const newSettings: PatternSettings = {
        spacing: 4.0,
        burden: 3.0,
        depth: 15.0
      };
      
      mockSettings$.next(newSettings);

      expect(component.settings).toEqual(newSettings);
    });
  });

  describe('Formatting Methods', () => {
    it('should format coordinates correctly', () => {
      expect(component.formatCoordinate(10.123)).toBe('10.12');
      expect(component.formatCoordinate(5.999)).toBe('6.00');
      expect(component.formatCoordinate(0)).toBe('0.00');
    });

    it('should format scale as percentage', () => {
      expect(component.formatScale(1.0)).toBe('100');
      expect(component.formatScale(1.5)).toBe('150');
      expect(component.formatScale(0.75)).toBe('75');
      expect(component.formatScale(2.25)).toBe('225');
    });

    it('should format zoom level descriptions', () => {
      expect(component.formatZoomLevel(2.5)).toBe('High');
      expect(component.formatZoomLevel(1.8)).toBe('Medium');
      expect(component.formatZoomLevel(1.2)).toBe('Normal');
      expect(component.formatZoomLevel(0.8)).toBe('Low');
      expect(component.formatZoomLevel(0.3)).toBe('Very Low');
    });

    it('should format numeric values', () => {
      expect(component.formatValue(3.14159)).toBe('3.1');
      expect(component.formatValue(10)).toBe('10.0');
      expect(component.formatValue(2.99)).toBe('3.0');
    });
  });

  describe('Status Information', () => {
    beforeEach(() => {
      component.ngOnInit();
      component.drillPoints = [mockDrillPoint];
      component.selectedPoint = mockDrillPoint;
      component.scale = 1.5;
      component.isHolePlacementMode = true;
      component.isPreciseMode = false;
      component.isFullscreen = true;
      component.isSaved = false;
    });

    it('should return correct status info', () => {
      const statusInfo = component.getStatusInfo();

      expect(statusInfo).toEqual({
        drillPointCount: 1,
        selectedPointId: 'test-point-123',
        scale: 1.5,
        modes: {
          holePlacement: true,
          precise: false,
          fullscreen: true
        },
        isSaved: false
      });
    });

    it('should handle null selected point in status info', () => {
      component.selectedPoint = null;
      
      const statusInfo = component.getStatusInfo();

      expect(statusInfo.selectedPointId).toBeNull();
    });
  });

  describe('Template Rendering', () => {
    beforeEach(() => {
      component.ngOnInit();
      fixture.detectChanges();
    });

    it('should render cursor position when available', () => {
      component.cursorPosition = { x: 10.5, y: 15.2 };
      fixture.detectChanges();

      const positionElement = fixture.nativeElement.querySelector('.cursor-position .status-value');
      expect(positionElement?.textContent?.trim()).toContain('X: 10.50m, Y: 15.20m');
    });

    it('should not render cursor position when null', () => {
      component.cursorPosition = null;
      fixture.detectChanges();

      const positionElement = fixture.nativeElement.querySelector('.cursor-position');
      expect(positionElement).toBeNull();
    });

    it('should render scale and zoom information', () => {
      component.scale = 1.5;
      fixture.detectChanges();

      const scaleElement = fixture.nativeElement.querySelector('.scale-info');
      expect(scaleElement?.textContent).toContain('150%');
      expect(scaleElement?.textContent).toContain('Medium');
    });

    it('should render active mode indicators', () => {
      component.isHolePlacementMode = true;
      component.isPreciseMode = false;
      component.isFullscreen = true;
      fixture.detectChanges();

      const modeIndicators = fixture.nativeElement.querySelectorAll('.mode-indicator');
      expect(modeIndicators[0]).toHaveClass('active'); // Place mode
      expect(modeIndicators[1]).not.toHaveClass('active'); // Precise mode
      expect(modeIndicators[2]).toHaveClass('active'); // Fullscreen mode
    });

    it('should render pattern statistics', () => {
      component.drillPoints = [mockDrillPoint];
      component.selectedPoint = mockDrillPoint;
      fixture.detectChanges();

      const statsElement = fixture.nativeElement.querySelector('.pattern-stats');
      expect(statsElement?.textContent).toContain('Points: 1');
      expect(statsElement?.textContent).toContain('Selected: ID: test-poi...');
    });

    it('should render pattern settings when available', () => {
      component.settings = mockSettings;
      fixture.detectChanges();

      const settingsElement = fixture.nativeElement.querySelector('.pattern-settings');
      expect(settingsElement?.textContent).toContain('Spacing: 3.0m');
      expect(settingsElement?.textContent).toContain('Burden: 2.5m');
      expect(settingsElement?.textContent).toContain('Depth: 10.0m');
    });

    it('should not render pattern settings when null', () => {
      component.settings = null;
      fixture.detectChanges();

      const settingsElement = fixture.nativeElement.querySelector('.pattern-settings');
      expect(settingsElement).toBeNull();
    });

    it('should render save status correctly', () => {
      component.isSaved = true;
      fixture.detectChanges();

      const saveIndicator = fixture.nativeElement.querySelector('.save-indicator');
      expect(saveIndicator).toHaveClass('saved');
      expect(saveIndicator?.textContent).toContain('Saved');

      component.isSaved = false;
      fixture.detectChanges();

      expect(saveIndicator).toHaveClass('unsaved');
      expect(saveIndicator?.textContent).toContain('Modified');
    });

    it('should apply fullscreen class when in fullscreen mode', () => {
      component.isFullscreen = true;
      fixture.detectChanges();

      const statusBar = fixture.nativeElement.querySelector('.status-bar');
      expect(statusBar).toHaveClass('fullscreen');
    });
  });

  describe('Accessibility', () => {
    beforeEach(() => {
      component.ngOnInit();
      fixture.detectChanges();
    });

    it('should have proper ARIA attributes', () => {
      const statusBar = fixture.nativeElement.querySelector('.status-bar');
      expect(statusBar.getAttribute('role')).toBe('status');
      expect(statusBar.getAttribute('aria-live')).toBe('polite');
    });

    it('should have ARIA labels for mode indicators', () => {
      component.isHolePlacementMode = true;
      fixture.detectChanges();

      const placeModeIndicator = fixture.nativeElement.querySelector('.mode-indicator');
      expect(placeModeIndicator.getAttribute('aria-label')).toBe('Hole placement mode active');
    });

    it('should have ARIA labels for save indicator', () => {
      component.isSaved = false;
      fixture.detectChanges();

      const saveIndicator = fixture.nativeElement.querySelector('.save-indicator');
      expect(saveIndicator.getAttribute('aria-label')).toBe('Pattern has unsaved changes');
    });
  });

  describe('Component Cleanup', () => {
    it('should complete destroy subject on destroy', () => {
      component.ngOnInit();
      spyOn(component['destroy$'], 'next');
      spyOn(component['destroy$'], 'complete');

      component.ngOnDestroy();

      expect(component['destroy$'].next).toHaveBeenCalled();
      expect(component['destroy$'].complete).toHaveBeenCalled();
    });
  });
});