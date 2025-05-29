export const CANVAS_CONSTANTS = {
  RULER_WIDTH: 30,
  RULER_HEIGHT: 30,
  POINT_RADIUS: 5,
  GRID_SIZE: 50,
  DEBOUNCE_TIME: 100,
  MAX_DRILL_POINTS: 1000,
  MAX_CACHE_SIZE: 10,
  POINT_SELECTION_THRESHOLD: 2,
  DEFAULT_SETTINGS: {
    spacing: 3,
    burden: 2.5,
    depth: 10
  },
  ARIA_LABELS: {
    canvas: 'Drilling pattern canvas',
    holePlacement: 'Toggle hole placement mode',
    preciseMode: 'Toggle precise mode',
    zoomIn: 'Zoom in',
    zoomOut: 'Zoom out',
    resetZoom: 'Reset zoom',
    clearAll: 'Clear all drill points',
    exportPattern: 'Export drilling pattern'
  }
} as const; 