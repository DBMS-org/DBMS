# PatternInstructionsComponent

A comprehensive help and instructions component for the drilling pattern creator that provides contextual guidance, keyboard shortcuts, and best practices for users.

## Overview

The `PatternInstructionsComponent` is a modal dialog component that displays detailed instructions and help content for the drilling pattern creator. It features a responsive design, full accessibility support, and keyboard navigation capabilities.

## Features

- **Comprehensive Help Content**: Covers quick start guide, mouse controls, keyboard shortcuts, pattern settings, and best practices
- **Accessibility Support**: Full ARIA compliance, keyboard navigation, and screen reader support
- **Responsive Design**: Adapts to different screen sizes with mobile-optimized layouts
- **Keyboard Navigation**: Supports Escape and H keys for quick access
- **State Management**: Integrates with PatternStateService for reactive state updates
- **Focus Management**: Proper focus handling for accessibility

## Usage

### Basic Implementation

```typescript
import { PatternInstructionsComponent } from './pattern-instructions.component';

@Component({
  template: `
    <app-pattern-instructions
      [showInstructions]="showInstructions"
      (instructionsToggle)="onInstructionsToggle($event)">
    </app-pattern-instructions>
  `
})
export class ParentComponent {
  showInstructions = false;

  onInstructionsToggle(show: boolean): void {
    this.showInstructions = show;
  }
}
```

### With State Service Integration

```typescript
import { PatternStateService } from '../../services/pattern-state.service';

@Component({
  template: `
    <app-pattern-instructions
      [showInstructions]="(patternStateService.state$ | async)?.uiState.showInstructions || false"
      (instructionsToggle)="onInstructionsToggle($event)">
    </app-pattern-instructions>
  `
})
export class ParentComponent {
  constructor(public patternStateService: PatternStateService) {}

  onInstructionsToggle(show: boolean): void {
    this.patternStateService.updateUIState({ showInstructions: show });
  }
}
```

## API Reference

### Inputs

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `showInstructions` | `boolean` | `false` | Controls the visibility of the instructions modal |

### Outputs

| Event | Type | Description |
|-------|------|-------------|
| `instructionsToggle` | `EventEmitter<boolean>` | Emitted when instructions visibility changes |

### Methods

| Method | Parameters | Return Type | Description |
|--------|------------|-------------|-------------|
| `toggleInstructions()` | none | `void` | Toggles the instructions visibility |
| `onInstructionsToggle(show: boolean)` | `show: boolean` | `void` | Handles instructions toggle events |
| `getInstructionsInfo()` | none | `object` | Returns component status information |

## Content Sections

### 1. Quick Start
Step-by-step guide for new users:
- Setting pattern parameters
- Entering hole placement mode
- Placing drill holes
- Using precise mode

### 2. Mouse Controls
Comprehensive mouse interaction guide:
- Left click for placement/selection
- Right click for context menus
- Drag for moving holes
- Mouse wheel for zooming
- Middle click + drag for panning

### 3. Keyboard Shortcuts
All available keyboard shortcuts:
- `P` - Toggle hole placement mode
- `G` - Toggle precise/grid mode
- `F` - Toggle fullscreen mode
- `H` - Show/hide help instructions
- `Delete` - Delete selected hole
- `Ctrl + S` - Save pattern
- `Ctrl + A` - Select all holes
- `Escape` - Exit mode or close dialogs

### 4. Pattern Settings
Explanation of pattern parameters:
- **Spacing**: Distance between holes in same row
- **Burden**: Distance between rows
- **Depth**: Default drilling depth

### 5. Tips & Best Practices
Helpful tips for efficient usage:
- Using precise mode for accuracy
- Right-clicking for individual settings
- Saving work frequently
- Monitoring status bar
- Zooming out for full pattern view

## Accessibility Features

### ARIA Support
- `role="dialog"` for modal semantics
- `aria-labelledby` and `aria-describedby` for proper labeling
- `aria-hidden` for visibility state
- `aria-live` regions for dynamic content

### Keyboard Navigation
- **Escape**: Close instructions
- **H**: Toggle instructions (when focused)
- **Tab**: Navigate through interactive elements
- **Enter/Space**: Activate buttons

### Screen Reader Support
- Semantic HTML structure
- Proper heading hierarchy
- Descriptive labels and instructions
- Focus management

## Responsive Design

### Desktop (768px+)
- Full-width modal with sidebar layout
- Grid-based content organization
- Hover effects and animations

### Tablet (768px - 480px)
- Stacked content layout
- Reduced padding and margins
- Simplified grid structures

### Mobile (< 480px)
- Single-column layout
- Compressed content sections
- Touch-optimized buttons
- Minimal animations

## Styling

### CSS Classes

| Class | Description |
|-------|-------------|
| `.instructions-overlay` | Modal overlay background |
| `.instructions-panel` | Main content panel |
| `.instructions-header` | Header with title and close button |
| `.instructions-content` | Scrollable content area |
| `.instruction-section` | Individual content sections |
| `.shortcuts-grid` | Keyboard shortcuts grid layout |
| `.controls-grid` | Mouse controls grid layout |

### Customization

The component uses CSS custom properties for easy theming:

```scss
.instructions-overlay {
  --primary-color: #2196f3;
  --background-color: #ffffff;
  --text-color: #333333;
  --border-color: #e0e0e0;
}
```

## Testing

### Unit Tests
Comprehensive test coverage including:
- Component initialization
- Input/output handling
- Event handling
- Keyboard navigation
- Accessibility features
- Template rendering
- State management integration

### Test Utilities
```typescript
// Test helper for component setup
function setupComponent(showInstructions = false) {
  const fixture = TestBed.createComponent(PatternInstructionsComponent);
  const component = fixture.componentInstance;
  component.showInstructions = showInstructions;
  fixture.detectChanges();
  return { fixture, component };
}
```

## Performance Considerations

### Optimization Features
- **Lazy Loading**: Component only renders when visible
- **Event Delegation**: Efficient event handling
- **CSS Animations**: Hardware-accelerated transitions
- **Memory Management**: Proper subscription cleanup

### Best Practices
- Use `OnPush` change detection when possible
- Implement proper cleanup in `ngOnDestroy`
- Minimize DOM manipulations
- Use CSS transforms for animations

## Browser Support

- **Modern Browsers**: Full feature support
- **IE11**: Basic functionality with polyfills
- **Mobile Browsers**: Touch-optimized interactions
- **Screen Readers**: Full accessibility support

## Integration Examples

### With Toolbar Component
```typescript
// In toolbar component
toggleHelp(): void {
  this.patternStateService.updateUIState({ 
    showInstructions: !this.currentState.showInstructions 
  });
}
```

### With Keyboard Service
```typescript
// Global keyboard handler
@HostListener('document:keydown', ['$event'])
onKeyDown(event: KeyboardEvent): void {
  if (event.key === 'F1' || (event.key === 'h' && !event.ctrlKey)) {
    event.preventDefault();
    this.toggleInstructions();
  }
}
```

## Troubleshooting

### Common Issues

1. **Instructions not showing**
   - Check `showInstructions` input binding
   - Verify state service integration
   - Check for CSS z-index conflicts

2. **Keyboard navigation not working**
   - Ensure component is properly focused
   - Check for event propagation issues
   - Verify keyboard event handlers

3. **Accessibility issues**
   - Validate ARIA attributes
   - Test with screen readers
   - Check focus management

### Debug Information
Use the `getInstructionsInfo()` method to get component status:

```typescript
const info = component.getInstructionsInfo();
console.log('Instructions Info:', info);
// Output: { isVisible: true, hasKeyboardSupport: true, isAccessible: true }
```

## Contributing

When contributing to this component:

1. Maintain accessibility standards
2. Test across different screen sizes
3. Verify keyboard navigation
4. Update documentation for new features
5. Add appropriate unit tests
6. Follow the established code style

## Related Components

- `PatternToolbarComponent` - Contains help button trigger
- `PatternStateService` - Manages instruction visibility state
- `PatternCanvasComponent` - Main canvas that instructions describe