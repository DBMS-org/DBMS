# DrillPointCanvasComponent

The `DrillPointCanvasComponent` is a specialized Angular component responsible for rendering and managing drill points in the drilling pattern creator. It handles all drill point operations including rendering, selection, dragging, validation, and visual feedback.

## Features

### Core Functionality
- **Point Rendering**: Renders drill points with different visual styles based on state (selected, custom depth, etc.)
- **Interactive Selection**: Click-to-select points with visual feedback
- **Drag and Drop**: Drag points to new positions with real-time validation
- **Point Placement**: Add new points in hole placement mode with duplicate detection
- **Visual Indicators**: Custom depth indicators and hover effects
- **Performance Optimized**: Efficient rendering for large datasets (tested with 1000+ points)

### Visual Features
- **Color Coding**: Different colors for selected, custom depth, and standard points
- **Depth Indicators**: Triangle indicators for points with custom depths
- **Hover Effects**: Highlight rings when hovering over points
- **Selection Feedback**: Visual emphasis for selected points

### Validation & Safety
- **Duplicate Detection**: Prevents placing points too close together
- **Boundary Validation**: Ensures points stay within canvas bounds
- **Distance Validation**: Enforces minimum distance between points
- **Real-time Feedback**: Immediate validation during drag operations

## Usage

### Basic Setup

```typescript
import { DrillPointCanvasComponent } from './drill-point-canvas.component';

@Component({
  imports: [DrillPointCanvasComponent],
  template: `
    <app-drill-point-canvas
      [layer]="konvaLayer"
      [drillPoints]="points"
      [selectedPoint]="selectedPoint"
      [settings]="patternSettings"
      [canvasState]="canvasState"
      [isHolePlacementMode]="placementMode"
      [isPreciseMode]="preciseMode"
      (pointSelected)="onPointSelected($event)"
      (pointMoved)="onPointMoved($event)"
      (pointPlaced)="onPointPlaced($event)"
      (duplicateDetected)="onDuplicateDetected($event)">
    </app-drill-point-canvas>
  `
})
export class MyComponent {
  konvaLayer: Konva.Layer;
  points: DrillPoint[] = [];
  selectedPoint: DrillPoint | null = null;
  // ... other properties
}
```

### Input Properties

| Property | Type | Description |
|----------|------|-------------|
| `layer` | `Konva.Layer` | **Required.** The Konva layer to render points on |
| `drillPoints` | `DrillPoint[]` | Array of drill points to render |
| `selectedPoint` | `DrillPoint \| null` | Currently selected point |
| `settings` | `PatternSettings` | **Required.** Pattern settings (spacing, burden, depth) |
| `canvasState` | `CanvasState` | **Required.** Canvas state (scale, offsets, dimensions) |
| `isHolePlacementMode` | `boolean` | Enable/disable point placement mode |
| `isPreciseMode` | `boolean` | Enable/disable precise mode features |

### Output Events

| Event | Type | Description |
|-------|------|-------------|
| `pointSelected` | `DrillPoint \| null` | Emitted when a point is selected/deselected |
| `pointMoved` | `{point: DrillPoint, newPosition: CanvasPosition}` | Emitted when a point is moved |
| `pointPlaced` | `CanvasPosition` | Emitted when a new point is placed |
| `duplicateDetected` | `{message: string, existingPoint: DrillPoint}` | Emitted when duplicate placement is attempted |

### Data Models

```typescript
interface DrillPoint {
  id: string;
  x: number;        // World coordinates
  y: number;        // World coordinates
  depth: number;    // Depth in meters
  spacing: number;  // Spacing setting
  burden: number;   // Burden setting
}

interface PatternSettings {
  spacing: number;  // Default spacing
  burden: number;   // Default burden
  depth: number;    // Default depth
}

interface CanvasState {
  scale: number;      // Zoom level
  offsetX: number;    // Base offset X
  offsetY: number;    // Base offset Y
  panOffsetX: number; // Pan offset X
  panOffsetY: number; // Pan offset Y
  width: number;      // Canvas width
  height: number;     // Canvas height
}
```

## Methods

### Public Methods

| Method | Parameters | Returns | Description |
|--------|------------|---------|-------------|
| `renderPoints()` | - | `void` | Manually trigger point rendering |
| `updatePoints()` | - | `void` | Update points when data changes |
| `clearPoints()` | - | `void` | Clear all points from canvas |
| `addPointAtPosition()` | `position: CanvasPosition` | `DrillPoint \| null` | Add point at position (with validation) |
| `selectPoint()` | `pointId: string \| null` | `void` | Select/deselect point by ID |
| `getPerformanceStats()` | - | `PerformanceStats` | Get rendering performance statistics |

### Performance Statistics

```typescript
interface PerformanceStats {
  lastRenderTime: number;  // Last render time in milliseconds
  renderCount: number;     // Total number of renders
  pointCount: number;      // Current number of points
}
```

## Visual Styling

### Point Colors

The component uses different colors to indicate point states:

- **Standard Points**: Blue fill (`#2196f3`) with white stroke
- **Custom Depth Points**: Orange fill (`#ff9800`) with darker orange stroke (`#f57c00`)
- **Selected Points**: Red fill (`#ff4444`) with yellow stroke (`#ffff00`)

### Depth Indicators

Points with custom depths (different from pattern settings) display a small orange triangle indicator in the top-right corner.

### Hover Effects

When hovering over points, a yellow highlight ring appears around the point.

## Validation Rules

### Point Placement Validation

1. **Duplicate Detection**: Points cannot be placed within `DUPLICATE_TOLERANCE` (0.01m) of existing points
2. **Minimum Distance**: Points must be at least `MIN_POINT_DISTANCE` (0.1m) apart
3. **Boundary Check**: Points must be placed within canvas boundaries (excluding ruler areas)

### Drag Validation

During drag operations, the same validation rules apply. Invalid positions cause the point to snap back to its last valid position.

## Performance Considerations

### Optimization Features

- **Efficient Rendering**: Uses Konva groups for organized rendering
- **Event Batching**: Batches canvas updates for better performance
- **Memory Management**: Proper cleanup of Konva objects
- **Coordinate Caching**: Efficient coordinate conversion

### Performance Limits

- **Recommended**: Up to 500 points for optimal performance
- **Maximum Tested**: 1000+ points (may experience slower interactions)
- **Render Time**: Typically < 50ms for 100 points

## Integration Examples

### With Grid and Ruler Components

```typescript
// Complete canvas setup with multiple components
@Component({
  template: `
    <app-grid-canvas [layer]="gridLayer" [settings]="settings" [canvasState]="state"></app-grid-canvas>
    <app-ruler-canvas [layer]="rulerLayer" [settings]="settings" [canvasState]="state"></app-ruler-canvas>
    <app-drill-point-canvas 
      [layer]="pointsLayer" 
      [drillPoints]="points" 
      [settings]="settings" 
      [canvasState]="state"
      (pointSelected)="onPointSelected($event)">
    </app-drill-point-canvas>
  `
})
export class IntegratedCanvasComponent {
  // Layer setup in correct order (bottom to top)
  gridLayer = new Konva.Layer();
  rulerLayer = new Konva.Layer();
  pointsLayer = new Konva.Layer();
}
```

### Event Handling Pattern

```typescript
export class PatternCreatorComponent {
  onPointSelected(point: DrillPoint | null): void {
    this.selectedPoint = point;
    // Update UI, enable/disable buttons, etc.
  }

  onPointMoved(event: {point: DrillPoint, newPosition: CanvasPosition}): void {
    // Update point in data array
    const index = this.points.findIndex(p => p.id === event.point.id);
    if (index >= 0) {
      this.points[index] = { ...this.points[index], ...event.newPosition };
    }
  }

  onDuplicateDetected(event: {message: string, existingPoint: DrillPoint}): void {
    // Show user feedback
    this.showWarning(event.message);
    // Optionally highlight existing point
    this.selectedPoint = event.existingPoint;
  }
}
```

## Testing

### Unit Tests

The component includes comprehensive unit tests covering:

- Component initialization and cleanup
- Point rendering and updates
- Event handling (click, drag, hover)
- Validation logic
- Coordinate conversion
- Performance tracking
- Error handling

### Demo Tests

Demo tests demonstrate real-world usage scenarios:

- Creating grid patterns
- Interactive point operations
- Performance with large datasets
- Error handling and edge cases
- Integration with canvas state

### Running Tests

```bash
# Run unit tests
ng test --include="**/drill-point-canvas.component.spec.ts"

# Run demo tests
ng test --include="**/drill-point-canvas.demo.spec.ts"
```

## Troubleshooting

### Common Issues

1. **Points Not Rendering**
   - Ensure `layer` is properly initialized and added to stage
   - Check that `settings` and `canvasState` are provided
   - Verify points have valid coordinates

2. **Click Events Not Working**
   - Ensure layer is added to stage and stage is added to DOM
   - Check that points are within canvas boundaries
   - Verify event handlers are properly set up

3. **Performance Issues**
   - Reduce number of points if > 500
   - Check for memory leaks (call `ngOnDestroy`)
   - Monitor render times with `getPerformanceStats()`

4. **Validation Errors**
   - Check duplicate tolerance settings
   - Ensure canvas boundaries are correctly calculated
   - Verify coordinate conversion functions

### Debug Information

Enable debug logging by setting `CANVAS_CONSTANTS.DEBUG.ENABLED = true` to see detailed rendering information.

## Dependencies

- **Angular**: ^19.0.0
- **Konva**: ^9.0.0
- **TypeScript**: ^5.7.0

## Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

## Contributing

When contributing to this component:

1. Follow the established patterns for event handling
2. Add comprehensive tests for new features
3. Update this README for any API changes
4. Ensure performance doesn't degrade with changes
5. Test with large datasets (500+ points)

## License

This component is part of the DBMS (Drilling & Blasting Management System) and follows the project's licensing terms.