# GridCanvasComponent

A specialized Angular component for rendering grid lines and intersection indicators in the drilling pattern creator. This component extracts grid rendering logic from the monolithic main component and provides optimized, cached grid operations.

## Features

- **Grid Line Rendering**: Draws major and minor grid lines based on spacing and burden settings
- **Precise Mode Support**: Shows intersection indicators when precise mode is enabled
- **Performance Optimization**: Built-in caching mechanism to prevent unnecessary redraws
- **Automatic Updates**: Responds to input changes via Angular's change detection
- **Memory Management**: Proper cleanup of Konva objects and cache management
- **Error Handling**: Graceful handling of edge cases and invalid inputs

## Usage

### Basic Usage

```typescript
import { GridCanvasComponent } from './grid-canvas.component';

@Component({
  template: `
    <app-grid-canvas
      [layer]="gridLayer"
      [settings]="settings"
      [canvasState]="canvasState"
      [isPreciseMode]="isPreciseMode">
    </app-grid-canvas>
  `,
  imports: [GridCanvasComponent]
})
export class MyComponent {
  gridLayer: Konva.Layer;
  settings: PatternSettings = { spacing: 3, burden: 2.5, depth: 10 };
  canvasState = {
    scale: 1,
    offsetX: 0,
    offsetY: 0,
    panOffsetX: 0,
    panOffsetY: 0,
    width: 800,
    height: 600
  };
  isPreciseMode = false;
}
```

### With Reactive State Management

```typescript
@Component({
  template: `
    <app-grid-canvas
      [layer]="gridLayer"
      [settings]="settings$ | async"
      [canvasState]="canvasState$ | async"
      [isPreciseMode]="isPreciseMode$ | async">
    </app-grid-canvas>
  `
})
export class MyComponent {
  settings$ = this.stateService.settings$;
  canvasState$ = this.stateService.canvasState$;
  isPreciseMode$ = this.stateService.isPreciseMode$;
}
```

## API Reference

### Inputs

| Input | Type | Description |
|-------|------|-------------|
| `layer` | `Konva.Layer` | The Konva layer where grid will be rendered |
| `settings` | `PatternSettings` | Grid settings (spacing, burden, depth) |
| `canvasState` | `CanvasState` | Canvas state (scale, offsets, dimensions) |
| `isPreciseMode` | `boolean` | Whether to show intersection indicators |

### Public Methods

| Method | Description | Returns |
|--------|-------------|---------|
| `renderGrid()` | Render the grid with current settings | `void` |
| `updateGrid()` | Update grid when settings change | `void` |
| `clearGrid()` | Clear the grid from the layer | `void` |
| `getCacheStats()` | Get cache performance statistics | `CacheStats` |
| `clearAllCaches()` | Clear all cached grid objects | `void` |

### Types

```typescript
interface PatternSettings {
  spacing: number;  // Grid spacing in meters
  burden: number;   // Grid burden in meters
  depth: number;    // Drill depth in meters
}

interface CanvasState {
  scale: number;      // Zoom scale factor
  offsetX: number;    // X offset for grid alignment
  offsetY: number;    // Y offset for grid alignment
  panOffsetX: number; // Pan offset X
  panOffsetY: number; // Pan offset Y
  width: number;      // Canvas width
  height: number;     // Canvas height
}

interface CacheStats {
  gridCacheSize: number;         // Number of cached grid objects
  intersectionCacheSize: number; // Number of cached intersection objects
  lastRenderTime: number;        // Last render time in milliseconds
  renderCount: number;           // Total number of renders
}
```

## Performance Considerations

### Caching Strategy

The component implements a two-level caching system:

1. **Grid Cache**: Caches grid line objects based on settings and canvas state
2. **Intersection Cache**: Caches intersection indicators separately

Cache keys are generated from:
- Scale factor
- Pan and offset positions
- Canvas dimensions
- Grid settings (spacing, burden)
- Precise mode state

### Cache Limits

- Maximum cache size: `CANVAS_CONSTANTS.MAX_CACHE_SIZE` (default: 10)
- Maximum grid lines: `CANVAS_CONSTANTS.MAX_GRID_LINES` (default: 1000)
- Automatic cleanup when limits are exceeded

### Performance Tips

1. **Avoid Frequent Setting Changes**: Batch setting updates to minimize cache misses
2. **Monitor Cache Stats**: Use `getCacheStats()` to monitor performance
3. **Clear Cache When Needed**: Call `clearAllCaches()` during major state changes
4. **Optimize Grid Density**: Use appropriate spacing/burden values for zoom level

## Integration with Existing System

### Before (Monolithic Approach)

```typescript
// In main component
private drawGrid(): void {
  const cacheKey = `${this.scale}-${this.offsetX + this.panOffsetX}...`;
  const isCached = this.canvasService.handleGridCache(cacheKey, this.gridGroup, this.gridLayer);
  if (!isCached) {
    this.gridGroup = this.canvasService.drawGrid(
      this.gridLayer, this.settings, this.scale, 
      this.offsetX + this.panOffsetX, this.offsetY + this.panOffsetY,
      this.stage.width(), this.stage.height()
    );
    this.canvasService.updateGridCache(cacheKey, this.gridGroup);
  }
  this.intersectionGroup = this.canvasService.drawGridIntersections(...);
  this.gridLayer.batchDraw();
}
```

### After (Component-Based Approach)

```typescript
// In template
<app-grid-canvas
  [layer]="gridLayer"
  [settings]="settings"
  [canvasState]="canvasState"
  [isPreciseMode]="isPreciseMode">
</app-grid-canvas>

// No manual grid drawing code needed in component
```

## Testing

The component includes comprehensive unit tests covering:

- Component lifecycle
- Input change handling
- Grid rendering functionality
- Caching behavior
- Performance optimization
- Error handling
- Memory management

Run tests with:
```bash
ng test --include="**/grid-canvas.component.spec.ts"
```

## Architecture Benefits

1. **Separation of Concerns**: Grid logic is isolated from main component
2. **Testability**: Component can be tested in isolation
3. **Reusability**: Can be used in other canvas-based components
4. **Maintainability**: Clear interfaces and single responsibility
5. **Performance**: Optimized caching with automatic cleanup
6. **Type Safety**: Full TypeScript support with proper interfaces

## Migration Guide

To migrate from the current monolithic approach:

1. Replace direct `gridService` calls with `GridCanvasComponent`
2. Move grid-related state to reactive observables
3. Remove manual cache management code
4. Update templates to use the component
5. Update tests to use component interface

See `grid-canvas.integration.example.ts` for detailed migration examples.