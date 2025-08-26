import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { DrillPoint, PatternSettings, PatternData, DrillHoleData } from '../models/drill-point.model';
import { CANVAS_CONSTANTS } from '../constants/canvas.constants';
import { Logger } from '../utils/logger.util';
import { DrillPointService } from '../services/drill-point.service';
import { SiteBlastingService } from '../../../../core/services/site-blasting.service';
import { BlastSequenceDataService } from '../../shared/services/blast-sequence-data.service';
import { NavigationController } from '../../shared/services/navigation-controller.service';
import { DrillDataService } from '../../csv-upload/drill-data.service';
// Using DrillHoleData from local model instead of core model
// import { DrillHole } from '../../../../core/models/drill-hole.model';

export interface SaveResult {
  success: boolean;
  message: string;
  patternId?: number;
}

@Injectable({
  providedIn: 'root'
})
export class PatternDataManager {
  private currentProjectId?: number;
  private currentSiteId?: number;
  private drillPoints: DrillPoint[] = [];
  private settings: PatternSettings = { ...CANVAS_CONSTANTS.DEFAULT_SETTINGS };

  constructor(
    private drillPointService: DrillPointService,
    private siteBlastingService: SiteBlastingService,
    private blastSequenceDataService: BlastSequenceDataService,
    private navigationController: NavigationController,
    private drillDataService: DrillDataService
  ) {}

  initializeContext(projectId: number, siteId: number): void {
    this.currentProjectId = projectId;
    this.currentSiteId = siteId;
    this.blastSequenceDataService.setSiteContext(projectId, siteId);
    Logger.info('Pattern data manager context set', { projectId, siteId });
  }

  getDrillPoints(): DrillPoint[] {
    return [...this.drillPoints];
  }

  getSettings(): PatternSettings {
    return { ...this.settings };
  }

  updateSettings(newSettings: Partial<PatternSettings>): void {
    this.settings = { ...this.settings, ...newSettings };
    Logger.info('Pattern settings updated', this.settings);
  }

  addDrillPoint(x: number, y: number): DrillPoint | null {
    // Validate coordinates
    if (!this.drillPointService.validateCoordinates(x, y)) {
      return null;
    }

    // Check max points limit
    if (!this.drillPointService.validateDrillPointCount(
      this.drillPoints.length, 
      CANVAS_CONSTANTS.MAX_DRILL_POINTS
    )) {
      return null;
    }

    // Check for duplicates
    if (!this.drillPointService.validateUniqueCoordinates(x, y, this.drillPoints)) {
      return null;
    }

    const newPoint = this.drillPointService.createDrillPoint(x, y, this.settings);
    this.drillPoints.push(newPoint);
    
    Logger.info('Drill point added', newPoint);
    return newPoint;
  }

  removeDrillPoint(pointId: string): boolean {
    const pointToRemove = this.drillPoints.find(p => p.id === pointId);
    if (!pointToRemove) {
      Logger.warn('Point not found for removal', pointId);
      return false;
    }

    this.drillPoints = this.drillPointService.removePoint(pointToRemove, this.drillPoints);
    Logger.info('Drill point removed', pointId);
    return true;
  }

  updateDrillPointPosition(pointId: string, x: number, y: number): boolean {
    const point = this.drillPoints.find(p => p.id === pointId);
    if (!point) {
      Logger.warn('Point not found for position update', pointId);
      return false;
    }

    // Validate new coordinates
    if (!this.drillPointService.validateCoordinates(x, y)) {
      return false;
    }

    // Check for duplicates (excluding current point)
    const otherPoints = this.drillPoints.filter(p => p.id !== pointId);
    if (!this.drillPointService.validateUniqueCoordinates(x, y, otherPoints)) {
      return false;
    }

    point.x = Number(x.toFixed(CANVAS_CONSTANTS.PERFORMANCE.COORDINATE_PRECISION));
    point.y = Number(y.toFixed(CANVAS_CONSTANTS.PERFORMANCE.COORDINATE_PRECISION));
    
    Logger.info('Drill point position updated', { pointId, x, y });
    return true;
  }

  clearAllPoints(): void {
    this.drillPoints = [];
    this.drillPointService.resetHoleNumbering();
    Logger.info('All drill points cleared');
  }

  loadExistingPatternData(): void {
    const existingPatternData = this.blastSequenceDataService.getPatternData();
    if (existingPatternData?.drillPoints) {
      this.drillPoints = [...existingPatternData.drillPoints];
      
      if (existingPatternData.settings) {
        this.settings = { ...existingPatternData.settings };
      }

      // Update the currentId to continue from the highest existing ID
      if (this.drillPoints.length > 0) {
        const highestId = Math.max(...this.drillPoints.map(point => {
          const numericPart = parseInt(point.id.replace('DH', ''));
          return isNaN(numericPart) ? 0 : numericPart;
        }));
        this.drillPointService.setCurrentId(highestId + 1);
      }

      Logger.info('Existing pattern data loaded', {
        pointsCount: this.drillPoints.length,
        nextId: this.drillPointService.getCurrentId()
      });
    }
  }

  loadBackendPatternData(): Observable<boolean> {
    if (!this.currentProjectId || !this.currentSiteId) {
      return of(false);
    }

    return this.siteBlastingService.getDrillPatterns(this.currentProjectId, this.currentSiteId).pipe(
      map(patterns => {
        if (patterns && patterns.length > 0) {
          // Use the most recent pattern
          const latestPattern = patterns[patterns.length - 1];
          
          try {
            if (latestPattern.drillPointsJson) {
              const parsedPoints = JSON.parse(latestPattern.drillPointsJson);
              
              // Align loaded points to ensure coordinates match spacing/burden values
              this.drillPoints = this.drillPointService.alignExistingPointsToGrid(parsedPoints);
              
              Logger.info('Pattern loaded from backend and aligned', {
                patternName: latestPattern.name,
                pointsCount: this.drillPoints.length,
                alignmentApplied: true
              });
              return true;
            }
          } catch (error) {
            Logger.error('Failed to parse drill points JSON', error);
          }
        }
        return false;
      }),
      catchError(error => {
        Logger.error('Error loading drill patterns', error);
        return of(false);
      })
    );
  }

  processUploadedCSVData(): Observable<boolean> {
    const uploadedDrillData = this.drillDataService.getDrillData();
    if (!uploadedDrillData || uploadedDrillData.length === 0) {
      return of(false);
    }

    return of(true).pipe(
      map(() => {
        // Check if pattern already exists
        if (this.drillPoints.length > 0) {
          const userConfirmed = confirm(
            `This will replace the existing pattern with ${this.drillPoints.length} drill points. Continue?`
          );
          if (!userConfirmed) {
            return false;
          }
        }

        // Clear existing pattern
        this.clearAllPoints();

        // Convert uploaded data to drill points
        const convertedPoints = this.convertCSVDataToPoints(uploadedDrillData);
        
        // Auto-detect spacing and burden
        const { spacing: autoSpacing, burden: autoBurden } = 
          this.drillPointService.calculateGridPitch(convertedPoints);

        // Update settings
        this.updateSettings({
          spacing: autoSpacing,
          burden: autoBurden
        });

        // Anchor points to origin and apply settings
        const anchoredPoints = this.anchorPointsToOrigin(convertedPoints);
        this.drillPoints = anchoredPoints.map(p => ({
          ...p,
          spacing: autoSpacing,
          burden: autoBurden
        }));

        Logger.info('CSV data processed', {
          originalCount: uploadedDrillData.length,
          convertedCount: this.drillPoints.length,
          autoSpacing,
          autoBurden
        });

        return true;
      })
    );
  }

  private convertCSVDataToPoints(csvData: DrillHoleData[]): DrillPoint[] {
    const defaultDepthSetting = this.settings.depth ?? CANVAS_CONSTANTS.DEFAULT_SETTINGS.depth;

    return csvData.map((hole, index) => {
      const depthValue = hole.depth ?? hole.length ?? defaultDepthSetting;
      return {
        x: Number((hole.easting?.toFixed(2) ?? 0)),
        y: Number((hole.northing?.toFixed(2) ?? 0)),
        id: hole.id ? hole.id.toString() : `DH${index + 1}`,
        depth: depthValue,
        spacing: this.settings.spacing,
        burden: this.settings.burden
      };
    });
  }

  private anchorPointsToOrigin(points: DrillPoint[]): DrillPoint[] {
    if (points.length === 0) return points;

    const minX = Math.min(...points.map(p => p.x));
    const minY = Math.min(...points.map(p => p.y));

    return points.map(p => ({
      ...p,
      x: Number((p.x - minX).toFixed(CANVAS_CONSTANTS.PERFORMANCE.COORDINATE_PRECISION)),
      y: Number((p.y - minY).toFixed(CANVAS_CONSTANTS.PERFORMANCE.COORDINATE_PRECISION))
    }));
  }

  savePattern(): Observable<SaveResult> {
    if (this.drillPoints.length === 0) {
      return of({ success: false, message: 'No drill points to save' });
    }

    if (!this.currentProjectId || !this.currentSiteId) {
      return of({ success: false, message: 'Missing project or site context' });
    }

    const patternData = this.getPatternData();
    const patternName = `Pattern_${new Date().toISOString().slice(0, 16).replace(/:/g, '-')}`;

    return this.siteBlastingService.saveDrillPattern({
      projectId: this.currentProjectId,
      siteId: this.currentSiteId,
      name: patternName,
      drillPointsJson: JSON.stringify(this.drillPoints),
      createdAt: new Date().toISOString()
    }).pipe(
      map(savedPattern => {
            // Save to local storage and workflow state
    this.saveToLocalStorage(patternData);
    
    // Update pattern data in blast sequence data service
    this.blastSequenceDataService.setPatternData(patternData, true);
    
    // Save workflow state if context exists
    if (this.currentProjectId && this.currentSiteId) {
      this.navigationController.updateWorkflowStep('pattern-creator', {
        drillPoints: this.drillPoints,
        settings: this.settings,
        completedAt: new Date().toISOString()
      }).subscribe({
        next: () => Logger.info('Workflow state saved successfully'),
        error: (error) => Logger.error('Error saving workflow state', error)
      });
    }

    Logger.info('Pattern saved successfully', savedPattern);
        return {
          success: true,
          message: 'Pattern saved successfully',
          patternId: savedPattern.id
        };
      }),
      catchError(error => {
        Logger.error('Error saving pattern to backend', error);
        
        // Fallback to local storage
        this.saveToLocalStorage(patternData);
        
        return of({
          success: false,
          message: 'Pattern saved locally (backend unavailable)'
        });
      })
    );
  }

  private saveToLocalStorage(patternData: PatternData): void {
    try {
      localStorage.setItem('currentDrillingPattern', JSON.stringify(patternData));
      Logger.info('Pattern saved to local storage');
    } catch (error) {
      Logger.error('Failed to save to local storage', error);
    }
  }

  // Method has been inlined in savePattern() method

  getPatternData(): PatternData {
    return this.drillPointService.getPatternData(this.drillPoints, this.settings);
  }

  exportForBlastDesigner(): PatternData {
    return this.drillPointService.exportPatternForBlastDesign(this.drillPoints, this.settings);
  }
} 