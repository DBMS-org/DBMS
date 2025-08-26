export const CANVAS_CONSTANTS = {
  GRID_SIZE: 10, // Base grid size in pixels (reduced from 50 to fix scaling issues)
  RULER_WIDTH: 50,
  RULER_HEIGHT: 50,
  POINT_RADIUS: 5,
  MAX_GRID_LINES: 1000,
  MAX_DRILL_POINTS: 500,
  MAX_CACHE_SIZE: 10,
  DEFAULT_SETTINGS: {
    spacing: 3,
    burden: 2.5,
    depth: 10
  },
  ARIA_LABELS: {
    patternCreator: 'Drilling pattern creator canvas',
    gridContainer: 'Grid container for drill pattern',
    pointsContainer: 'Container for drill points',
    placeHole: 'Place drill hole',
    selectPoint: 'Select drill point',
    movePoint: 'Move drill point',
    deletePoint: 'Delete drill point',
    clearPoints: 'Clear all drill points',
    savePattern: 'Save drilling pattern',
    toBlastDesigner: 'Send pattern to blast sequence designer',
    showHelp: 'Show help instructions'
  },
  // Development/Production settings
  DEBUG: {
    ENABLED: false, // Set to false for production
    LOG_LEVELS: {
      INFO: 'info',
      WARN: 'warn', 
      ERROR: 'error'
    }
  },
  // Performance thresholds
  PERFORMANCE: {
    GRID_PITCH_MIN_THRESHOLD: 0.5, // meters
    GRID_PITCH_SUPPORT_THRESHOLD: 0.10, // 10%
    COORDINATE_PRECISION: 2, // decimal places
    DUPLICATE_TOLERANCE: 0.01 // meters
  },
  // Timeout values
  TIMEOUTS: {
    CANVAS_INIT_RETRY: 50, // ms
    DUPLICATE_MESSAGE: 3000, // ms
    SAVE_FEEDBACK: 2000, // ms
    RESIZE_DEBOUNCE: 150 // ms
  }
} as const; 