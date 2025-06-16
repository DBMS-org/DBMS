export const CANVAS_CONSTANTS = {
  GRID_SIZE: 50, // Base grid size in pixels
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
  }
} as const; 