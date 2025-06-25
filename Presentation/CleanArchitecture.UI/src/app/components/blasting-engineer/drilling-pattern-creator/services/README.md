# Canvas Services Architecture

The canvas functionality has been divided into three specialized services for better modularity and maintainability:

## Service Overview

### 1. GridService (`grid.service.ts`)
**Responsibility**: Grid drawing, intersections, and caching

**Key Methods**:
- `drawGrid()` - Renders major and minor grid lines
- `drawGridIntersections()` - Shows orange dots at grid intersections in precise mode
- `handleGridCache()` - Manages grid caching for performance
- `updateGridCache()` - Updates the grid cache
- `clearGridCache()` - Clears all cached grid objects

**Features**:
- Grid line alignment with ruler measurements
- Minor and major grid lines
- Precise mode intersection indicators
- Performance optimization through caching

### 2. RulerService (`ruler.service.ts`)
**Responsibility**: Ruler drawing and measurements

**Key Methods**:
- `drawRulers()` - Renders horizontal and vertical rulers with measurements
- `formatValue()` - Smart decimal formatting (hides .00 for whole numbers)

**Features**:
- Dynamic ruler values based on spacing/burden settings
- Boundary-aware ruler rendering
- Origin (0,0) marker when visible
- Pan-aware measurement calculation

### 3. DrillPointCanvasService (`drill-point-canvas.service.ts`)
**Responsibility**: Drill point rendering, coordinate calculations, and cursor management

**Key Methods**:
- `createDrillPointObject()` - Creates visual drill point representations
- `calculateGridCoordinates()` - Converts canvas coordinates to grid coordinates
- `snapToGridIntersection()` - Handles precise mode snapping
- `setCanvasCursor()` - Manages cursor states
- `updatePointSelectability()` - Controls point dragging behavior

**Features**:
- Precise grid snapping in precise mode
- Multiple cursor states (crosshair, panning, dragging, etc.)
- Grid-to-canvas coordinate conversion
- Point selection and dragging management

### 4. CanvasService (`canvas.service.ts`) - Facade
**Responsibility**: Coordinates between the three specialized services

**Purpose**:
- Maintains backward compatibility
- Provides a single entry point for the component
- Delegates method calls to appropriate specialized services
- Simplifies dependency management

## Usage Pattern

The component injects all four services:
```typescript
constructor(
  private canvasService: CanvasService,        // Facade for backward compatibility
  private gridService: GridService,           // Direct access for specific operations
  private rulerService: RulerService,         // Direct access for specific operations
  private drillPointCanvasService: DrillPointCanvasService // Direct access for specific operations
) {}
```

### When to Use Each Service:

1. **Use `canvasService`** for general operations that work across multiple areas
2. **Use `gridService`** directly when you need specific grid operations (like cache clearing)
3. **Use `rulerService`** directly for ruler-specific functionality
4. **Use `drillPointCanvasService`** directly for drill point rendering or coordinate calculations

## Benefits of This Architecture

1. **Single Responsibility Principle**: Each service has a clear, focused purpose
2. **Easier Testing**: Services can be unit tested independently
3. **Better Maintainability**: Changes to grid logic don't affect ruler or drill point logic
4. **Improved Code Organization**: Related functionality is grouped together
5. **Backward Compatibility**: Existing code continues to work through the facade pattern

## Migration Notes

- Existing code using `canvasService` continues to work without changes
- New functionality can use specialized services directly for better performance
- The facade pattern allows gradual migration to specialized services
- All services are dependency-injected and can be mocked for testing 