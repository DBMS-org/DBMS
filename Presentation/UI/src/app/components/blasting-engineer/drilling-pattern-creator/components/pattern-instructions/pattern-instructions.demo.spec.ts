import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BehaviorSubject } from 'rxjs';
import { PatternInstructionsComponent } from './pattern-instructions.component';
import { PatternStateService } from '../../services/pattern-state.service';

/**
 * Demo tests showcasing PatternInstructionsComponent usage patterns and integration examples.
 * These tests serve as living documentation and usage examples for developers.
 */
describe('PatternInstructionsComponent - Demo & Usage Examples', () => {
  let component: PatternInstructionsComponent;
  let fixture: ComponentFixture<PatternInstructionsComponent>;
  let mockPatternStateService: jasmine.SpyObj<PatternStateService>;
  let mockState$: BehaviorSubject<any>;

  const createMockPatternState = (overrides: any = {}) => ({
    drillPoints: [],
    selectedPoint: null,
    settings: { spacing: 3.0, burden: 2.5, depth: 10.0 },
    isHolePlacementMode: false,
    isPreciseMode: false,
    showInstructions: false,
    cursorPosition: null,
    duplicateAttemptMessage: null,
    isSaved: true,
    isFullscreen: false,
    selectedHoleDepth: null,
    isReadOnly: false,
    ...overrides
  });

  beforeEach(async () => {
    mockState$ = new BehaviorSubject<any>(createMockPatternState());
    mockPatternStateService = jasmine.createSpyObj('PatternStateService', 
      ['setShowInstructions'], 
      { state$: mockState$ }
    );

    await TestBed.configureTestingModule({
      imports: [PatternInstructionsComponent],
      providers: [
        { provide: PatternStateService, useValue: mockPatternStateService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PatternInstructionsComponent);
    component = fixture.componentInstance;
  });

  describe('Usage Example 1: Basic Implementation', () => {
    it('should demonstrate basic component setup and usage', () => {
      // DEMO: Basic component initialization
      component.ngOnInit();
      fixture.detectChanges();

      // DEMO: Component should be created and ready
      expect(component).toBeTruthy();
      expect(component.showInstructions).toBeFalse();

      // DEMO: Show instructions programmatically
      component.showInstructions = true;
      fixture.detectChanges();

      // DEMO: Verify instructions are visible
      const overlay = fixture.nativeElement.querySelector('.instructions-overlay');
      expect(overlay).toBeTruthy();

      // DEMO: Instructions should contain all expected sections
      const sections = fixture.nativeElement.querySelectorAll('.instruction-section');
      expect(sections.length).toBe(5);

      console.log('✓ Demo 1: Basic implementation works correctly');
    });
  });

  describe('Usage Example 2: State Service Integration', () => {
    it('should demonstrate reactive state management integration', () => {
      component.ngOnInit();

      // DEMO: Component responds to state service changes
      const newState = createMockPatternState({
        showInstructions: true 
      });

      mockState$.next(newState);

      // DEMO: Component automatically updates from state service
      expect(component.showInstructions).toBeTrue();

      // DEMO: Component updates state service when toggled
      component.onInstructionsToggle(false);

      expect(mockPatternStateService.setShowInstructions).toHaveBeenCalledWith(false);

      console.log('✓ Demo 2: State service integration works correctly');
    });
  });

  describe('Usage Example 3: Keyboard Navigation', () => {
    it('should demonstrate keyboard accessibility features', () => {
      component.ngOnInit();
      component.showInstructions = true;
      fixture.detectChanges();

      spyOn(component, 'onInstructionsToggle');

      // DEMO: Escape key closes instructions
      const escapeEvent = new KeyboardEvent('keydown', { key: 'Escape' });
      component.onKeyDown(escapeEvent);

      expect(component.onInstructionsToggle).toHaveBeenCalledWith(false);

      // DEMO: H key toggles instructions
      const hKeyEvent = new KeyboardEvent('keydown', { key: 'h' });
      component.onKeyDown(hKeyEvent);

      expect(component.onInstructionsToggle).toHaveBeenCalledWith(false);

      // DEMO: Modifier keys are ignored
      const ctrlHEvent = new KeyboardEvent('keydown', { key: 'h', ctrlKey: true });
      (component.onInstructionsToggle as jasmine.Spy).calls.reset();
      component.onKeyDown(ctrlHEvent);

      expect(component.onInstructionsToggle).not.toHaveBeenCalled();

      console.log('✓ Demo 3: Keyboard navigation works correctly');
    });
  });

  describe('Usage Example 4: Accessibility Features', () => {
    it('should demonstrate accessibility compliance', () => {
      component.showInstructions = true;
      fixture.detectChanges();

      // DEMO: Proper ARIA attributes
      const overlay = fixture.nativeElement.querySelector('.instructions-overlay');
      expect(overlay.getAttribute('role')).toBe('dialog');
      expect(overlay.getAttribute('aria-labelledby')).toBe('instructions-title');
      expect(overlay.getAttribute('aria-describedby')).toBe('instructions-content');

      // DEMO: Focusable elements have proper attributes
      const closeButton = fixture.nativeElement.querySelector('.close-button');
      expect(closeButton.getAttribute('aria-label')).toBe('Close instructions');

      // DEMO: Section headings are properly structured
      const sections = fixture.nativeElement.querySelectorAll('.instruction-section');
      sections.forEach((section: any) => {
        const title = section.querySelector('.section-title');
        expect(title.id).toBeTruthy();
        expect(section.getAttribute('aria-labelledby')).toBe(title.id);
      });

      console.log('✓ Demo 4: Accessibility features work correctly');
    });
  });

  describe('Usage Example 5: Responsive Design', () => {
    it('should demonstrate responsive behavior', () => {
      component.showInstructions = true;
      fixture.detectChanges();

      // DEMO: Component renders with responsive classes
      const overlay = fixture.nativeElement.querySelector('.instructions-overlay');
      const panel = fixture.nativeElement.querySelector('.instructions-panel');

      expect(overlay).toHaveClass('instructions-overlay');
      expect(panel).toHaveClass('instructions-panel');

      // DEMO: Content sections are properly structured for responsive layout
      const controlsGrid = fixture.nativeElement.querySelector('.controls-grid');
      const shortcutsGrid = fixture.nativeElement.querySelector('.shortcuts-grid');

      expect(controlsGrid).toBeTruthy();
      expect(shortcutsGrid).toBeTruthy();

      // DEMO: Mobile-friendly elements are present
      const closeButton = fixture.nativeElement.querySelector('.close-button');
      expect(closeButton).toBeTruthy();

      console.log('✓ Demo 5: Responsive design works correctly');
    });
  });

  describe('Usage Example 6: Content Verification', () => {
    it('should demonstrate comprehensive help content', () => {
      component.showInstructions = true;
      fixture.detectChanges();

      // DEMO: Quick Start section
      const quickStartSection = Array.from(fixture.nativeElement.querySelectorAll('.section-title'))
        .find((title: any) => title.textContent?.includes('Quick Start'));
      expect(quickStartSection).toBeTruthy();

      const instructionSteps = fixture.nativeElement.querySelectorAll('.instruction-step');
      expect(instructionSteps.length).toBeGreaterThan(0);

      // DEMO: Keyboard shortcuts section
      const shortcutKeys = fixture.nativeElement.querySelectorAll('.shortcut-key');
      const shortcutTexts = Array.from(shortcutKeys).map((key: any) => key.textContent?.trim());
      
      expect(shortcutTexts).toContain('P');
      expect(shortcutTexts).toContain('G');
      expect(shortcutTexts).toContain('F');
      expect(shortcutTexts).toContain('H');

      // DEMO: Mouse controls section
      const controlActions = fixture.nativeElement.querySelectorAll('.control-action');
      const controlTexts = Array.from(controlActions).map((action: any) => action.textContent?.trim());
      
      expect(controlTexts).toContain('Left Click');
      expect(controlTexts).toContain('Right Click');
      expect(controlTexts).toContain('Drag');

      // DEMO: Pattern settings section
      const settingNames = fixture.nativeElement.querySelectorAll('.setting-name');
      const settingTexts = Array.from(settingNames).map((name: any) => name.textContent?.trim());
      
      expect(settingTexts).toContain('Spacing');
      expect(settingTexts).toContain('Burden');
      expect(settingTexts).toContain('Depth');

      // DEMO: Tips section
      const tipItems = fixture.nativeElement.querySelectorAll('.tip-item');
      expect(tipItems.length).toBeGreaterThan(0);

      console.log('✓ Demo 6: Content verification works correctly');
    });
  });

  describe('Usage Example 7: Event Handling', () => {
    it('should demonstrate comprehensive event handling', () => {
      component.ngOnInit();
      spyOn(component.instructionsToggle, 'emit');

      // DEMO: Toggle functionality
      component.toggleInstructions();
      expect(component.showInstructions).toBeTrue();
      expect(component.instructionsToggle.emit).toHaveBeenCalledWith(true);

      // DEMO: Direct event handling
      component.onInstructionsToggle(false);
      expect(component.showInstructions).toBeFalse();
      expect(component.instructionsToggle.emit).toHaveBeenCalledWith(false);

      // DEMO: Overlay click handling
      component.showInstructions = true;
      const mockEvent = new Event('click');
      spyOn(component, 'onInstructionsToggle');
      
      component.onOverlayClick(mockEvent);
      expect(component.onInstructionsToggle).toHaveBeenCalledWith(false);

      console.log('✓ Demo 7: Event handling works correctly');
    });
  });

  describe('Usage Example 8: Component Information API', () => {
    it('should demonstrate component status API', () => {
      // DEMO: Get component information when hidden
      component.showInstructions = false;
      let info = component.getInstructionsInfo();
      
      expect(info).toEqual({
        isVisible: false,
        hasKeyboardSupport: true,
        isAccessible: true
      });

      // DEMO: Get component information when visible
      component.showInstructions = true;
      info = component.getInstructionsInfo();
      
      expect(info).toEqual({
        isVisible: true,
        hasKeyboardSupport: true,
        isAccessible: true
      });

      console.log('✓ Demo 8: Component information API works correctly');
      console.log('Component Info:', info);
    });
  });

  describe('Usage Example 9: Integration with Parent Components', () => {
    it('should demonstrate parent component integration patterns', () => {
      // DEMO: Simulate parent component interaction
      const parentComponentState = {
        showInstructions: false,
        onInstructionsToggle: jasmine.createSpy('onInstructionsToggle')
      };

      // DEMO: Parent component receives events
      component.instructionsToggle.subscribe(parentComponentState.onInstructionsToggle);

      // DEMO: Parent component controls visibility
      component.showInstructions = parentComponentState.showInstructions;
      expect(component.showInstructions).toBeFalse();

      // DEMO: Component notifies parent of changes
      component.onInstructionsToggle(true);
      expect(parentComponentState.onInstructionsToggle).toHaveBeenCalledWith(true);

      console.log('✓ Demo 9: Parent component integration works correctly');
    });
  });

  describe('Usage Example 10: Performance and Cleanup', () => {
    it('should demonstrate proper lifecycle management', () => {
      // DEMO: Component initialization
      component.ngOnInit();
      expect(component['destroy$']).toBeTruthy();

      // DEMO: Subscription management
      const destroySubject = component['destroy$'];
      spyOn(destroySubject, 'next');
      spyOn(destroySubject, 'complete');

      // DEMO: Proper cleanup on destroy
      component.ngOnDestroy();
      expect(destroySubject.next).toHaveBeenCalled();
      expect(destroySubject.complete).toHaveBeenCalled();

      console.log('✓ Demo 10: Lifecycle management works correctly');
    });
  });

  afterEach(() => {
    fixture.destroy();
  });
});