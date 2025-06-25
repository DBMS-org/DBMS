import { Injectable } from '@angular/core';
import Konva from 'konva';
import { PatternSettings } from '../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../constants/canvas.constants';

@Injectable({
  providedIn: 'root'
})
export class RulerService {

  constructor() {}

  private formatValue(value: number): string {
    // Show decimal places only if they're not .00
    return value % 1 === 0 ? value.toString() : value.toFixed(2);
  }

  drawRulers(
    layer: Konva.Layer,
    settings: PatternSettings,
    scale: number,
    width: number,
    height: number,
    panOffsetX: number = 0,
    panOffsetY: number = 0
  ): Konva.Group {
    const rulerGroup = new Konva.Group();

    // Draw ruler backgrounds
    rulerGroup.add(new Konva.Rect({
      x: 0,
      y: 0,
      width: CANVAS_CONSTANTS.RULER_WIDTH,
      height: height,
      fill: '#f8f9fa',
      listening: false
    }));

    rulerGroup.add(new Konva.Rect({
      x: 0,
      y: 0,
      width: width,
      height: CANVAS_CONSTANTS.RULER_HEIGHT,
      fill: '#f8f9fa',
      listening: false
    }));

    // Calculate grid dimensions
    const burden = settings.burden * CANVAS_CONSTANTS.GRID_SIZE * scale;
    const spacing = settings.spacing * CANVAS_CONSTANTS.GRID_SIZE * scale;

    // Draw burden measurements (vertical ruler)
    const gridStartY = CANVAS_CONSTANTS.RULER_HEIGHT + panOffsetY;
    // Calculate the burden value at the first visible grid line
    // When panOffsetY is 0 (at boundary), start from 0
    let burdenValue = panOffsetY === 0 ? 0 : Math.floor(-panOffsetY / burden) * settings.burden;
    
    // Adjust starting position to align with first grid line
    let firstGridY = CANVAS_CONSTANTS.RULER_HEIGHT + (panOffsetY % burden);
    if (firstGridY < CANVAS_CONSTANTS.RULER_HEIGHT) {
      firstGridY += burden;
      burdenValue += settings.burden;
    }
    
    for (let y = firstGridY; y < height; y += burden) {
      if (y >= CANVAS_CONSTANTS.RULER_HEIGHT) {
        rulerGroup.add(new Konva.Text({
          x: CANVAS_CONSTANTS.RULER_WIDTH / 2,
          y: y,
          text: `${this.formatValue(burdenValue)}m`,
          fontSize: 10,
          fill: '#495057',
          align: 'center',
          listening: false
        }));
      }
      burdenValue += settings.burden;
    }

    // Draw spacing measurements (horizontal ruler)
    const gridStartX = CANVAS_CONSTANTS.RULER_WIDTH + panOffsetX;
    // Calculate the spacing value at the first visible grid line
    // When panOffsetX is 0 (at boundary), start from 0
    let spacingValue = panOffsetX === 0 ? 0 : Math.floor(-panOffsetX / spacing) * settings.spacing;
    
    // Adjust starting position to align with first grid line
    let firstGridX = CANVAS_CONSTANTS.RULER_WIDTH + (panOffsetX % spacing);
    if (firstGridX < CANVAS_CONSTANTS.RULER_WIDTH) {
      firstGridX += spacing;
      spacingValue += settings.spacing;
    }
    
    for (let x = firstGridX; x < width; x += spacing) {
      if (x >= CANVAS_CONSTANTS.RULER_WIDTH) {
        rulerGroup.add(new Konva.Text({
          x: x,
          y: CANVAS_CONSTANTS.RULER_HEIGHT / 2,
          text: `${this.formatValue(spacingValue)}m`,
          fontSize: 10,
          fill: '#495057',
          align: 'center',
          listening: false
        }));
      }
      spacingValue += settings.spacing;
    }

    // Add origin (0,0) marker if visible
    const originX = CANVAS_CONSTANTS.RULER_WIDTH + panOffsetX;
    const originY = CANVAS_CONSTANTS.RULER_HEIGHT + panOffsetY;
    
    if (originX >= CANVAS_CONSTANTS.RULER_WIDTH && originX <= width && 
        originY >= CANVAS_CONSTANTS.RULER_HEIGHT && originY <= height) {
      rulerGroup.add(new Konva.Text({
        x: originX,
        y: originY,
        text: '0',
        fontSize: 12,
        fill: '#dc3545',
        fontWeight: 'bold',
        align: 'center',
        listening: false
      }));
      
      // Add visual marker at origin
      rulerGroup.add(new Konva.Circle({
        x: originX,
        y: originY,
        radius: 3,
        fill: '#dc3545',
        listening: false
      }));
    }

    return rulerGroup;
  }
} 