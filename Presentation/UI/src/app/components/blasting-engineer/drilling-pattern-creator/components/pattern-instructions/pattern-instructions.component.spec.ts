import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BehaviorSubject } from 'rxjs';
import { PatternInstructionsComponent } from './pattern-instructions.component';
import { PatternStateService } from '../../services/pattern-state.service';
import { PatternState } from '../../models/pattern-state.model';

describe('PatternInstructionsComponent', () => {
    let component: PatternInstructionsComponent;
    let fixture: ComponentFixture<PatternInstructionsComponent>;
    let mockPatternStateService: jasmine.SpyObj<PatternStateService>;
    let mockState$: BehaviorSubject<any>;

    const mockPatternState: PatternState = {
        drillPoints: [],
        settings: {
            spacing: 3.0,
            burden: 2.5,
            depth: 10.0
        },
        selectedPoint: null,
        canvasState: {
            scale: 1.0,
            panOffsetX: 0,
            panOffsetY: 0,
            isInitialized: true,
            isDragging: false,
            isPanning: false
        },
        uiState: {
            isHolePlacementMode: false,
            isPreciseMode: false,
            isFullscreen: false,
            showInstructions: false,
            isSaved: true,
            duplicateMessage: null,
            cursorPosition: null
        },
        metadata: {
            projectId: 1,
            siteId: 1,
            lastModified: new Date(),
            version: '1.0.0'
        }
    };

    beforeEach(async () => {
        // Create mock observables
        mockState$ = new BehaviorSubject<PatternState>(mockPatternState);

        // Create mock service
        mockPatternStateService = jasmine.createSpyObj('PatternStateService',
            ['setShowInstructions'],
            {
                state$: mockState$
            }
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

    afterEach(() => {
        fixture.destroy();
    });

    describe('Component Initialization', () => {
        it('should create', () => {
            expect(component).toBeTruthy();
        });

        it('should initialize with default values', () => {
            expect(component.showInstructions).toBeFalse();
        });

        it('should implement IPatternInstructionsComponent interface', () => {
            expect(component.showInstructions).toBeDefined();
            expect(component.instructionsToggle).toBeDefined();
            expect(typeof component.toggleInstructions).toBe('function');
            expect(typeof component.onInstructionsToggle).toBe('function');
        });

        it('should subscribe to state changes on init', () => {
            component.ngOnInit();

            // Verify subscription is active by emitting new values
            const newState = {
                ...mockPatternState,
                uiState: { ...mockPatternState.uiState, showInstructions: true }
            };
            mockState$.next(newState);

            expect(component.showInstructions).toBeTrue();
        });
    });

    describe('Input Properties', () => {
        beforeEach(() => {
            component.ngOnInit();
        });

        it('should accept showInstructions input', () => {
            component.showInstructions = true;
            expect(component.showInstructions).toBeTrue();

            component.showInstructions = false;
            expect(component.showInstructions).toBeFalse();
        });

        it('should update from state service', () => {
            const newState = {
                ...mockPatternState,
                uiState: { ...mockPatternState.uiState, showInstructions: true }
            };

            mockState$.next(newState);

            expect(component.showInstructions).toBeTrue();
        });
    });

    describe('Instructions Toggle Functionality', () => {
        beforeEach(() => {
            component.ngOnInit();
            spyOn(component.instructionsToggle, 'emit');
        });

        it('should toggle instructions visibility', () => {
            component.showInstructions = false;

            component.toggleInstructions();

            expect(component.showInstructions).toBeTrue();
            expect(component.instructionsToggle.emit).toHaveBeenCalledWith(true);
            expect(mockPatternStateService.setShowInstructions).toHaveBeenCalledWith(true);
        });

        it('should handle onInstructionsToggle correctly', () => {
            component.onInstructionsToggle(true);

            expect(component.showInstructions).toBeTrue();
            expect(component.instructionsToggle.emit).toHaveBeenCalledWith(true);
            expect(mockPatternStateService.setShowInstructions).toHaveBeenCalledWith(true);
        });

        it('should emit false when toggling off', () => {
            component.showInstructions = true;

            component.toggleInstructions();

            expect(component.showInstructions).toBeFalse();
            expect(component.instructionsToggle.emit).toHaveBeenCalledWith(false);
            expect(mockPatternStateService.setShowInstructions).toHaveBeenCalledWith(true);
        });
    });

    describe('Event Handlers', () => {
        beforeEach(() => {
            component.ngOnInit();
            spyOn(component, 'onInstructionsToggle');
        });

        it('should handle overlay click', () => {
            const mockEvent = new Event('click');

            component.onOverlayClick(mockEvent);

            expect(component.onInstructionsToggle).toHaveBeenCalledWith(false);
        });

        it('should handle escape key', () => {
            component.showInstructions = true;
            const mockEvent = new KeyboardEvent('keydown', { key: 'Escape' });
            spyOn(mockEvent, 'preventDefault');

            component.onKeyDown(mockEvent);

            expect(mockEvent.preventDefault).toHaveBeenCalled();
            expect(component.onInstructionsToggle).toHaveBeenCalledWith(false);
        });

        it('should handle H key', () => {
            component.showInstructions = true;
            const mockEvent = new KeyboardEvent('keydown', { key: 'h' });
            spyOn(mockEvent, 'preventDefault');

            component.onKeyDown(mockEvent);

            expect(mockEvent.preventDefault).toHaveBeenCalled();
            expect(component.onInstructionsToggle).toHaveBeenCalledWith(false);
        });

        it('should handle uppercase H key', () => {
            component.showInstructions = true;
            const mockEvent = new KeyboardEvent('keydown', { key: 'H' });
            spyOn(mockEvent, 'preventDefault');

            component.onKeyDown(mockEvent);

            expect(mockEvent.preventDefault).toHaveBeenCalled();
            expect(component.onInstructionsToggle).toHaveBeenCalledWith(false);
        });

        it('should ignore keyboard events when instructions are hidden', () => {
            component.showInstructions = false;
            const mockEvent = new KeyboardEvent('keydown', { key: 'Escape' });
            spyOn(mockEvent, 'preventDefault');

            component.onKeyDown(mockEvent);

            expect(mockEvent.preventDefault).not.toHaveBeenCalled();
            expect(component.onInstructionsToggle).not.toHaveBeenCalled();
        });

        it('should ignore H key with modifier keys', () => {
            component.showInstructions = true;
            const mockEvent = new KeyboardEvent('keydown', { key: 'h', ctrlKey: true });
            spyOn(mockEvent, 'preventDefault');

            component.onKeyDown(mockEvent);

            expect(mockEvent.preventDefault).not.toHaveBeenCalled();
            expect(component.onInstructionsToggle).not.toHaveBeenCalled();
        });

        it('should ignore other keys', () => {
            component.showInstructions = true;
            const mockEvent = new KeyboardEvent('keydown', { key: 'a' });
            spyOn(mockEvent, 'preventDefault');

            component.onKeyDown(mockEvent);

            expect(mockEvent.preventDefault).not.toHaveBeenCalled();
            expect(component.onInstructionsToggle).not.toHaveBeenCalled();
        });
    });

    describe('Component Information', () => {
        it('should return correct instructions info', () => {
            component.showInstructions = true;

            const info = component.getInstructionsInfo();

            expect(info).toEqual({
                isVisible: true,
                hasKeyboardSupport: true,
                isAccessible: true
            });
        });

        it('should return false for visibility when hidden', () => {
            component.showInstructions = false;

            const info = component.getInstructionsInfo();

            expect(info.isVisible).toBeFalse();
            expect(info.hasKeyboardSupport).toBeTrue();
            expect(info.isAccessible).toBeTrue();
        });
    });

    describe('Template Rendering', () => {
        beforeEach(() => {
            component.ngOnInit();
        });

        it('should render instructions overlay when showInstructions is true', () => {
            component.showInstructions = true;
            fixture.detectChanges();

            const overlay = fixture.nativeElement.querySelector('.instructions-overlay');
            expect(overlay).toBeTruthy();
        });

        it('should not render instructions overlay when showInstructions is false', () => {
            component.showInstructions = false;
            fixture.detectChanges();

            const overlay = fixture.nativeElement.querySelector('.instructions-overlay');
            expect(overlay).toBeFalsy();
        });

        it('should render instructions title', () => {
            component.showInstructions = true;
            fixture.detectChanges();

            const title = fixture.nativeElement.querySelector('#instructions-title');
            expect(title?.textContent?.trim()).toBe('Drilling Pattern Creator - Help & Instructions');
        });

        it('should render close button', () => {
            component.showInstructions = true;
            fixture.detectChanges();

            const closeButton = fixture.nativeElement.querySelector('.close-button');
            expect(closeButton).toBeTruthy();
            expect(closeButton.getAttribute('aria-label')).toBe('Close instructions');
        });

        it('should render all instruction sections', () => {
            component.showInstructions = true;
            fixture.detectChanges();

            const sections = fixture.nativeElement.querySelectorAll('.instruction-section');
            expect(sections.length).toBe(5); // Quick Start, Mouse Controls, Keyboard Shortcuts, Pattern Settings, Tips

            const sectionTitles = Array.from(sections).map((section: any) =>
                section.querySelector('.section-title')?.textContent?.trim()
            );

            expect(sectionTitles).toContain('Quick Start');
            expect(sectionTitles).toContain('Mouse Controls');
            expect(sectionTitles).toContain('Keyboard Shortcuts');
            expect(sectionTitles).toContain('Pattern Settings');
            expect(sectionTitles).toContain('Tips & Best Practices');
        });

        it('should render quick start instructions', () => {
            component.showInstructions = true;
            fixture.detectChanges();

            const instructionItems = fixture.nativeElement.querySelectorAll('.instruction-item');
            expect(instructionItems.length).toBeGreaterThan(0);

            const firstStep = instructionItems[0].querySelector('.instruction-step');
            expect(firstStep?.textContent?.trim()).toBe('1.');
        });

        it('should render mouse controls', () => {
            component.showInstructions = true;
            fixture.detectChanges();

            const controlItems = fixture.nativeElement.querySelectorAll('.control-item');
            expect(controlItems.length).toBeGreaterThan(0);

            const controlActions = Array.from(controlItems).map((item: any) =>
                item.querySelector('.control-action')?.textContent?.trim()
            );

            expect(controlActions).toContain('Left Click');
            expect(controlActions).toContain('Right Click');
            expect(controlActions).toContain('Drag');
        });

        it('should render keyboard shortcuts', () => {
            component.showInstructions = true;
            fixture.detectChanges();

            const shortcutItems = fixture.nativeElement.querySelectorAll('.shortcut-item');
            expect(shortcutItems.length).toBeGreaterThan(0);

            const shortcutKeys = Array.from(shortcutItems).map((item: any) =>
                item.querySelector('.shortcut-key')?.textContent?.trim()
            );

            expect(shortcutKeys).toContain('P');
            expect(shortcutKeys).toContain('G');
            expect(shortcutKeys).toContain('F');
            expect(shortcutKeys).toContain('H');
        });

        it('should render pattern settings', () => {
            component.showInstructions = true;
            fixture.detectChanges();

            const settingItems = fixture.nativeElement.querySelectorAll('.setting-item');
            expect(settingItems.length).toBe(3);

            const settingNames = Array.from(settingItems).map((item: any) =>
                item.querySelector('.setting-name')?.textContent?.trim()
            );

            expect(settingNames).toContain('Spacing');
            expect(settingNames).toContain('Burden');
            expect(settingNames).toContain('Depth');
        });

        it('should render tips and best practices', () => {
            component.showInstructions = true;
            fixture.detectChanges();

            const tipItems = fixture.nativeElement.querySelectorAll('.tip-item');
            expect(tipItems.length).toBeGreaterThan(0);

            const tipIcons = fixture.nativeElement.querySelectorAll('.tip-icon');
            expect(tipIcons.length).toBe(tipItems.length);
        });

        it('should render footer with primary button', () => {
            component.showInstructions = true;
            fixture.detectChanges();

            const footer = fixture.nativeElement.querySelector('.instructions-footer');
            expect(footer).toBeTruthy();

            const primaryButton = footer.querySelector('.primary-button');
            expect(primaryButton).toBeTruthy();
            expect(primaryButton.textContent?.trim()).toBe("Got it, let's start!");
        });
    });

    describe('Accessibility', () => {
        beforeEach(() => {
            component.ngOnInit();
            component.showInstructions = true;
            fixture.detectChanges();
        });

        it('should have proper ARIA attributes on overlay', () => {
            const overlay = fixture.nativeElement.querySelector('.instructions-overlay');
            expect(overlay.getAttribute('role')).toBe('dialog');
            expect(overlay.getAttribute('aria-labelledby')).toBe('instructions-title');
            expect(overlay.getAttribute('aria-describedby')).toBe('instructions-content');
            expect(overlay.getAttribute('aria-hidden')).toBe('false');
        });

        it('should have proper ARIA attributes when hidden', () => {
            component.showInstructions = false;
            fixture.detectChanges();

            const overlay = fixture.nativeElement.querySelector('.instructions-overlay');
            expect(overlay).toBeFalsy(); // Component uses *ngIf, so element won't exist
        });

        it('should have focusable instructions panel', () => {
            const panel = fixture.nativeElement.querySelector('.instructions-panel');
            expect(panel.getAttribute('tabindex')).toBe('-1');
        });

        it('should have proper section headings with IDs', () => {
            const sectionTitles = fixture.nativeElement.querySelectorAll('.section-title');

            sectionTitles.forEach((title: any) => {
                expect(title.id).toBeTruthy();
            });

            const sections = fixture.nativeElement.querySelectorAll('.instruction-section');
            sections.forEach((section: any) => {
                const titleId = section.querySelector('.section-title')?.id;
                expect(section.getAttribute('aria-labelledby')).toBe(titleId);
            });
        });

        it('should have accessible close button', () => {
            const closeButton = fixture.nativeElement.querySelector('.close-button');
            expect(closeButton.getAttribute('aria-label')).toBe('Close instructions');
            expect(closeButton.getAttribute('type')).toBe('button');
        });

        it('should have accessible primary button', () => {
            const primaryButton = fixture.nativeElement.querySelector('.primary-button');
            expect(primaryButton.getAttribute('type')).toBe('button');
        });
    });

    describe('User Interactions', () => {
        beforeEach(() => {
            component.ngOnInit();
            component.showInstructions = true;
            fixture.detectChanges();
            spyOn(component, 'toggleInstructions');
        });

        it('should close instructions when close button is clicked', () => {
            const closeButton = fixture.nativeElement.querySelector('.close-button');

            closeButton.click();

            expect(component.toggleInstructions).toHaveBeenCalled();
        });

        it('should close instructions when primary button is clicked', () => {
            const primaryButton = fixture.nativeElement.querySelector('.primary-button');

            primaryButton.click();

            expect(component.toggleInstructions).toHaveBeenCalled();
        });

        it('should close instructions when overlay is clicked', () => {
            const overlay = fixture.nativeElement.querySelector('.instructions-overlay');
            spyOn(component, 'onOverlayClick');

            overlay.click();

            expect(component.onOverlayClick).toHaveBeenCalled();
        });

        it('should not close when clicking inside panel', () => {
            const panel = fixture.nativeElement.querySelector('.instructions-panel');
            const clickEvent = new Event('click');
            spyOn(clickEvent, 'stopPropagation');

            panel.dispatchEvent(clickEvent);

            expect(clickEvent.stopPropagation).toHaveBeenCalled();
        });

        it('should handle keyboard navigation on close button', () => {
            const closeButton = fixture.nativeElement.querySelector('.close-button');

            // Test Enter key
            const enterEvent = new KeyboardEvent('keydown', { key: 'Enter' });
            closeButton.dispatchEvent(enterEvent);
            expect(component.toggleInstructions).toHaveBeenCalled();

            // Reset spy
            (component.toggleInstructions as jasmine.Spy).calls.reset();

            // Test Space key
            const spaceEvent = new KeyboardEvent('keydown', { key: ' ' });
            closeButton.dispatchEvent(spaceEvent);
            expect(component.toggleInstructions).toHaveBeenCalled();
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