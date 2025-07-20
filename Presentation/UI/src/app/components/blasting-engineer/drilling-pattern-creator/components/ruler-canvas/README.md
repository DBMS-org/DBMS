# RulerCanvasComponent

## Overview

The `RulerCanvasComponent` is a specialized Angular component responsible for rendering horizontal and vertical rulers in the drilling pattern creator. It provides precise measurement indicators that help users accurately position drill points and understand the scale of their drilling patterns.

## Features

- **Dual Ruler System**: Renders both horizontal and vertical rulers with measurement marks
- **Dynamic Scaling**: Automatically adjusts measurement units based on zoom level for optimal readability
- **Performance Optimized**: Implements caching mechanisms to prevent unnecessary re-renders
- **Pan/Zoom Support**: Updates ruler positions and scales in real-time during canvas navigation
- **Precise Measurements**: Displays accurate measurements in meters with appropriate precision

## Architecture

### Component Structure
```
RulerCanvasComponent
├── Horizontal Ruler Group (top ruler)
├── Vertical Ruler Group (left ruler)
└── Background Group (ruler backgrounds and borders)
```

### Caching Strategy
- **Cache Key Generation**: Based on scale, offsets, and canvas dimensions
- **Cache Size Limit**: Respects `CANVAS_CONSTANTS.MAX_CACHE_SIZE` to prevent memory leaks
- **Automatic Cleanup**: Destroys oldest cache entries when limit is exceeded

## Usage

### Basic Implementation
```typescript
<app-ruler-canvas
  [layer]="konvaLayer"
  [settings]="patternSettings"
  [canvasState]="currentCanvasState">
</app-ruler-canvas>
```

### Input Properties

| Property | Type | Description |
|----------|------|-------------|
| `layer` | `Konva.Layer` | The Konva layer where rulers will be rendered |
| `settings` | `PatternSettings` | Pattern settings (spacing, burden, depth) |
| `canvasState` | `CanvasState` | Current canvas state (scale, offsets, dimensions) |

### Canvas State Interface
```typescript
interface CanvasState {
  scale: number;        // Current zoom level
  offsetX: number;      // Base X offset
  offsetY: number;      // Base Y offset
  panOffsetX: number;   // Pan X offset
  panOffsetY: number;   // Pan Y offset
  width: number;        // Canvas width
  height: number;       // Canvas height
}
```

## Public Methods

### Core Methods
- `renderRuler()`: Main rendering method
- `updateRuler()`: Updates rulers when state changes
- `clearRuler()`: Removes rulers from the layer

### Utility Methods
- `getCacheStats()`: Returns cache performance statistics
- `clearAllCaches()`: Forces cache cleanup (useful for testing)

## Ruler Configuration

### Visual Settings
```typescript
private readonly RULER_CONFIG = {
  MAJOR_TICK_HEIGHT: 15,      // Height of major tick marks
  MINOR_TICK_HEIGHT: 8,       // Height of minor tick marks
  MICRO_TICK_HEIGHT: 4,       // Height of micro tick marks
  TEXT_OFFSET: 20,            // Offset for measurement labels
  BACKGROUND_COLOR: '#f5f5f5', // Ruler background color
  BORDER_COLOR: '#cccccc',    // Ruler border color
  TICK_COLOR: '#666666',      // Tick mark color
  TEXT_COLOR: '#333333',      // Label text color
  FONT_SIZE: 10,              // Label font size
  FONT_FAMILY: 'Arial, sans-serif' // Label font family
};
```

### Measurement Units
The component automatically selects optimal measurement units based on zoom level:
- Available units: 0.1m, 0.2m, 0.5m, 1m, 2m, 5m, 10m, 20m, 50m, 100m
- Target: 50-100 pixels between major tick marks for optimal readability

## Performance Considerations

### Optimization Features
- **Tick Limiting**: Maximum 200 ticks per ruler to prevent performance issues
- **Caching System**: Reuses rendered rulers when possible
- **Batch Drawing**: Uses Konva's batch drawing for efficient rendering
- **Memory Management**: Automatic cleanup of old cache entries

### Performance Monitoring
```typescript
const stats = component.getCacheStats();
console.log(`Cache size: ${stats.cacheSize}`);
console.log(`Last render time: ${stats.lastRenderTime}ms`);
console.log(`Total renders: ${stats.renderCount}`);
```

## Integration with Canvas System

### Layer Management
- Rulers are rendered on the same layer as other canvas elements
- Background elements are rendered first, then tick marks and labels
- Proper z-index ordering ensures rulers appear correctly

### Coordinate System
- Uses the same coordinate system as the main canvas
- Respects `CANVAS_CONSTANTS.RULER_WIDTH` and `CANVAS_CONSTANTS.RULER_HEIGHT`
- Aligns with grid system for consistent measurements

## Testing

### Unit Test Coverage
- Component lifecycle (initialization, destruction)
- Input change handling
- Rendering with various configurations
- Cache management and performance
- Error handling and edge cases
- Memory management

### Test Utilities
```typescript
// Mock layer for testing
const mockLayer = jasmine.createSpyObj('Konva.Layer', ['add', 'batchDraw', 'destroy']);

// Test different scales
const scaleValues = [0.5, 1, 2, 5, 10];
scaleValues.forEach(scale => {
  component.canvasState = { ...mockCanvasState, scale };
  component.renderRuler();
});
```

## Error Handling

### Graceful Degradation
- Handles missing inputs with warning messages
- Continues operation even if rendering errors occur
- Provides meaningful error messages for debugging

### Common Error Scenarios
- Missing layer, settings, or canvas state
- Konva rendering errors
- Memory allocation issues
- Invalid scale or offset values

## Dependencies

### Required Imports
- `@angular/core`: Component lifecycle and change detection
- `@angular/common`: Common Angular utilities
- `konva`: Canvas rendering library

### Internal Dependencies
- `PatternSettings` from drill-point.model
- `CANVAS_CONSTANTS` from canvas.constants
- `Logger` utility for debugging

## Best Practices

### Usage Guidelines
1. Always provide valid layer, settings, and canvas state
2. Call `clearAllCaches()` when component is no longer needed
3. Monitor cache statistics in development for performance tuning
4. Use appropriate canvas dimensions for optimal ruler rendering

### Performance Tips
1. Avoid frequent unnecessary re-renders by checking input changes
2. Use caching effectively by maintaining consistent canvas states
3. Monitor memory usage in applications with long-running sessions
4. Consider ruler visibility when canvas is very small or very large

## Future Enhancements

### Potential Improvements
- Customizable ruler themes and colors
- Support for different measurement units (feet, inches)
- Interactive ruler features (click to set measurements)
- Ruler-specific zoom controls
- Export ruler measurements to external formats