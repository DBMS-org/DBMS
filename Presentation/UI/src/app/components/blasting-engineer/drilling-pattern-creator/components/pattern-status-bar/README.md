# PatternStatusBarComponent

The `PatternStatusBarComponent` is a specialized Angular component that provides real-time status information for the drilling pattern creator. It displays essential information about the current state of the pattern editor, including cursor position, scale, active modes, and pattern statistics.

## Features

- **Real-time cursor position tracking** - Shows precise X,Y coordinates as the user moves the mouse
- **Scale and zoom level display** - Indicates current zoom percentage and descriptive zoom level
- **Mode indicators** - Visual indicators for active modes (hole placement, precise mode, fullscreen)
- **Pattern statistics** - Shows drill point count and selected point information
- **Pattern settings display** - Current spacing, burden, and depth values
- **Save status indicator** - Visual indication of whether the pattern has unsaved changes
- **Reactive state management** - Automatically updates based on state changes from PatternStateService
- **Responsive design** - Adapts to different screen sizes and orientations
- **Accessibility support** - Proper ARIA labels and screen reader support

## Usage

### Basic Usage

```typescript
<app-pattern-status-bar
  [cursorPosition]="cursorPosition"
  [scale]="canvasScale"
  [isHolePlacementMode]="isHolePlacementMode"
  [isPreciseMode]="isPreciseMode"
  [isFullscreen]="isFullscreen"
  [drillPoints]="drillPoints"
  [selectedPoint]="selectedPoint"
  [settings]="patternSettings"
  [isSaved]="isSaved">
</app-pattern-status-bar>
```

### With Reactive State Management

```typescript
// The component automatically subscribes to PatternStateService
// No additional setup required - just include the component
<app-pattern-status-bar></app-pattern-status-bar>
```

## Inputs

| Input | Type | Default | Description |
|-------|------|---------|-------------|
| `cursorPosition` | `{ x: number; y: number } \| null` | `null` | Current cursor position in world coordinates |
| `scale` | `number` | `1` | Current canvas scale/zoom level |
| `isHolePlacementMode` | `boolean` | `false` | Whether hole placement mode is active |
| `isPreciseMode` | `boolean` | `false` | Whether precise mode is active |
| `isFullscreen` | `boolean` | `false` | Whether fullscreen mode is active |
| `drillPoints` | `DrillPoint[]` | `[]` | Array of current drill points |
| `selectedPoint` | `DrillPoint \| null` | `null` | Currently selected drill point |
| `settings` | `PatternSettings \| null` | `null` | Current pattern settings |
| `isSaved` | `boolean` | `true` | Whether the pattern has been saved |

## Public Methods

### `getStatusInfo()`

Returns comprehensive status information for debugging and monitoring:

```typescript
const statusInfo = statusBarComponent.getStatusInfo();
// Returns:
// {
//   drillPointCount: number;
//   selectedPointId: string | null;
//   scale: number;
//   modes: {
//     holePlacement: boolean;
//     precise: boolean;
//     fullscreen: boolean;
//   };
//   isSaved: boolean;
// }
```

### Formatting Methods

- `formatCoordinate(value: number): string` - Formats coordinates to 2 decimal places
- `formatScale(scale: number): string` - Formats scale as percentage
- `formatZoomLevel(scale: number): string` - Returns descriptive zoom level
- `formatValue(value: number): string` - Formats numeric values to 1 decimal place

## State Management Integration

The component automatically integrates with `PatternStateService` and subscribes to:

- **State changes** - Cursor position and save status updates
- **Drill points changes** - Point count and selection updates
- **Settings changes** - Pattern configuration updates
- **Selected point changes** - Selection state updates

## Styling

The component uses SCSS with the following key classes:

- `.status-bar` - Main container
- `.status-section` - Individual status sections
- `.mode-indicator` - Mode indicator buttons
- `.save-indicator` - Save status indicator
- `.fullscreen` - Fullscreen mode styling

### CSS Custom Properties

The component supports theming through CSS custom properties:

```css
.status-bar {
  --status-bg-color: #f5f5f5;
  --status-text-color: #333;
  --status-border-color: #e0e0e0;
  --active-mode-color: #2196f3;
  --saved-color: #4caf50;
  --unsaved-color: #ff9800;
}
```

## Responsive Design

The component adapts to different screen sizes:

- **Desktop** - Full status information displayed
- **Tablet** - Pattern settings hidden to save space
- **Mobile** - Mode indicators show icons only, vertical layout on very small screens

## Accessibility

The component follows accessibility best practices:

- **ARIA attributes** - `role="status"` and `aria-live="polite"`
- **Screen reader support** - Descriptive labels for all interactive elements
- **High contrast support** - Adapts to high contrast mode preferences
- **Reduced motion support** - Respects user motion preferences

## Performance

The component is optimized for performance:

- **Reactive subscriptions** - Only updates when state actually changes
- **OnPush change detection** - Efficient change detection strategy
- **Computed properties** - Cached calculations for display values
- **Memory management** - Proper subscription cleanup on destroy

## Testing

The component includes comprehensive unit tests covering:

- Component initialization and lifecycle
- Input changes and computed property updates
- Reactive state management integration
- Template rendering and DOM updates
- Accessibility features
- Formatting methods
- Error handling and edge cases

### Running Tests

```bash
ng test --include="**/pattern-status-bar.component.spec.ts"
```

## Integration Example

```typescript
import { Component } from '@angular/core';
import { PatternStatusBarComponent } from './components/pattern-status-bar/pattern-status-bar.component';
import { PatternStateService } from './services/pattern-state.service';

@Component({
  selector: 'app-drilling-pattern-creator',
  template: `
    <div class="pattern-creator">
      <!-- Canvas and toolbar components -->
      <app-pattern-canvas></app-pattern-canvas>
      
      <!-- Status bar at bottom -->
      <app-pattern-status-bar></app-pattern-status-bar>
    </div>
  `,
  imports: [PatternStatusBarComponent]
})
export class DrillingPatternCreatorComponent {
  constructor(private patternStateService: PatternStateService) {}
}
```

## Browser Support

The component supports all modern browsers:

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

## Dependencies

- Angular 19+
- RxJS 7+
- TypeScript 5.7+

## Related Components

- `PatternCanvasComponent` - Main canvas container
- `PatternToolbarComponent` - Pattern editing toolbar
- `DrillPointCanvasComponent` - Drill point rendering
- `GridCanvasComponent` - Grid rendering
- `RulerCanvasComponent` - Ruler rendering