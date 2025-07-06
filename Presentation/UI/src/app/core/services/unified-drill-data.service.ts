import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { 
  DrillLocation, 
  PatternSettings, 
  SavePatternRequest,
  SavePatternResult,
  CreateDrillLocationRequest,
  UpdateDrillLocationRequest,
  DrillModelConverter
} from '../models/drilling.model';

export interface PatternData {
  drillLocations: DrillLocation[];
  settings: PatternSettings;
  projectId: number;
  siteId: number;
}

@Injectable({
  providedIn: 'root'
})
export class UnifiedDrillDataService {
  private apiUrl = `${environment.apiUrl}/api/DrillPointPattern`;
  
  // Data state management
  private currentPatternSubject = new BehaviorSubject<PatternData | null>(null);
  public currentPattern$ = this.currentPatternSubject.asObservable();
  
  // Current context
  private currentProjectId: number = 0;
  private currentSiteId: number = 0;
  
  constructor(private http: HttpClient) {}
  
  // Context Management
  setSiteContext(projectId: number, siteId: number): void {
    this.currentProjectId = projectId;
    this.currentSiteId = siteId;
    this.loadPatternData(projectId, siteId);
  }
  
  getCurrentContext(): { projectId: number, siteId: number } {
    return { projectId: this.currentProjectId, siteId: this.currentSiteId };
  }
  
  /**
   * Load pattern data from the backend
   */
  loadPatternData(projectId: number, siteId: number): Observable<PatternData | null> {
    console.log('🔄 Loading pattern data from backend...', { projectId, siteId });
    
    return this.http.get<any>(`${this.apiUrl}/pattern-data?projectId=${projectId}&siteId=${siteId}`).pipe(
      map(response => {
        console.log('✅ Backend pattern data loaded:', response);
        
        if (!response || (!response.drillPoints?.length && !response.settings)) {
          console.log('ℹ️ No pattern data found');
          return null;
        }

        const patternData: PatternData = {
          drillLocations: response.drillPoints?.map((point: any) => 
            DrillModelConverter.drillPointToDrillLocation(point)
          ) || [],
          settings: response.settings || {
            name: 'Default Pattern',
            projectId,
            siteId,
            spacing: 3.0,
            burden: 2.5,
            depth: 10.0
          },
          projectId,
          siteId
        };

        this.setCurrentPattern(patternData);
        return patternData;
      }),
      catchError(error => {
        console.error('❌ Error loading pattern data:', error);
        return of(null);
      })
    );
  }
  
  /**
   * Save pattern data to the backend
   */
  savePattern(projectId: number, siteId: number, drillLocations: DrillLocation[], settings: PatternSettings): Observable<SavePatternResult> {
    console.log('💾 Saving pattern data...', { 
      projectId, 
      siteId, 
      drillPointCount: drillLocations.length,
      settings 
    });

    const drillPoints = drillLocations.map(location => ({
      id: location.id,
      x: location.x,
      y: location.y,
      depth: location.depth,
      spacing: location.spacing,
      burden: location.burden,
      projectId: location.projectId,
      siteId: location.siteId,
      createdAt: new Date(),
      updatedAt: new Date()
    }));

    const request = {
      projectId,
      siteId,
      drillPoints,
      settings
    };

    console.log('📤 Sending request to backend:', JSON.stringify(request, null, 2));

    return this.http.post<any>(`${this.apiUrl}/save-pattern`, request).pipe(
      map(response => {
        console.log('✅ Pattern saved successfully:', response);
        
        // Update current pattern
        const patternData: PatternData = {
          drillLocations,
          settings,
          projectId,
          siteId
        };
        this.setCurrentPattern(patternData);
        
        return {
          success: response.success || true,
          message: 'Pattern saved successfully'
        };
      }),
      catchError(error => {
        console.error('❌ Error saving pattern:', error);
        console.error('❌ Error details:', error.error);
        return of({
          success: false,
          message: error.error?.message || 'Failed to save pattern'
        });
      })
    );
  }
  
  /**
   * Get drill points for a project/site
   */
  getDrillPoints(projectId: number, siteId: number): Observable<DrillLocation[]> {
    return this.http.get<any[]>(`${this.apiUrl}/drill-points?projectId=${projectId}&siteId=${siteId}`).pipe(
      map(points => points.map(point => DrillModelConverter.drillPointToDrillLocation(point))),
      catchError(error => {
        console.error('❌ Error loading drill points:', error);
        return of([]);
      })
    );
  }
  
  /**
   * Get pattern settings for a project/site
   */
  getPatternSettings(projectId: number, siteId: number): Observable<PatternSettings> {
    return this.http.get<PatternSettings>(`${this.apiUrl}/pattern-settings?projectId=${projectId}&siteId=${siteId}`).pipe(
      catchError(error => {
        console.error('❌ Error loading pattern settings:', error);
        return of({
          name: 'Default Pattern',
          projectId,
          siteId,
          spacing: 3.0,
          burden: 2.5,
          depth: 10.0
        });
      })
    );
  }
  
  /**
   * Clear all drill points for a project/site
   */
  clearAllDrillPoints(projectId: number, siteId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/drill-points?projectId=${projectId}&siteId=${siteId}`).pipe(
      tap(success => {
        if (success) {
          console.log('✅ All drill points cleared');
          // Clear current pattern if it matches
          const current = this.getCurrentPattern();
          if (current && current.projectId === projectId && current.siteId === siteId) {
            this.setCurrentPattern(null);
          }
        }
      }),
      catchError(error => {
        console.error('❌ Error clearing drill points:', error);
        return of(false);
      })
    );
  }
  
  /**
   * Set the current pattern data
   */
  setCurrentPattern(patternData: PatternData | null): void {
    console.log('📝 Setting current pattern:', patternData);
    this.currentPatternSubject.next(patternData);
  }
  
  /**
   * Get the current pattern data
   */
  getCurrentPattern(): PatternData | null {
    return this.currentPatternSubject.value;
  }
  
  // Drill Location Management
  addDrillLocation(request: CreateDrillLocationRequest): Observable<DrillLocation> {
    return this.http.post<any>(`${this.apiUrl}/drill-points`, request).pipe(
      map(response => DrillModelConverter.drillPointToDrillLocation(response))
    );
  }
  
  updateDrillLocation(request: UpdateDrillLocationRequest): Observable<boolean> {
    return this.http.put<boolean>(`${this.apiUrl}/drill-points/${request.id}/position`, request);
  }
  
  removeDrillLocation(locationId: string, projectId: number, siteId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/drill-points/${locationId}`, {
      params: { projectId: projectId.toString(), siteId: siteId.toString() }
    });
  }
  
  clearAllDrillLocations(projectId: number, siteId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/drill-points`, {
      params: { projectId: projectId.toString(), siteId: siteId.toString() }
    });
  }
  
  /**
   * Get current drill locations (for backward compatibility)
   */
  getDrillLocations(): DrillLocation[] {
    const current = this.getCurrentPattern();
    return current ? current.drillLocations : [];
  }

  /**
   * Set drill locations (for backward compatibility)
   */
  setDrillLocations(locations: DrillLocation[]): void {
    const current = this.getCurrentPattern();
    if (current) {
      const updatedPattern: PatternData = {
        ...current,
        drillLocations: locations
      };
      this.setCurrentPattern(updatedPattern);
    }
  }

  /**
   * Clear local drill locations (for backward compatibility)
   */
  clearLocalDrillLocations(): void {
    const current = this.getCurrentPattern();
    if (current) {
      const updatedPattern: PatternData = {
        ...current,
        drillLocations: []
      };
      this.setCurrentPattern(updatedPattern);
    }
  }
  
  // Process CSV data
  processUploadedCsvData(request: any): Observable<PatternData> {
    return this.http.post<any>(`${this.apiUrl}/process-csv`, request).pipe(
      map(response => ({
        drillLocations: response.drillPoints?.map((point: any) => 
          DrillModelConverter.drillPointToDrillLocation(point)
        ) || [],
        settings: response.settings || {
          name: 'CSV Import',
          projectId: request.projectId,
          siteId: request.siteId,
          spacing: 3.0,
          burden: 2.5,
          depth: 10.0
        },
        projectId: request.projectId,
        siteId: request.siteId
      })),
      catchError(error => {
        console.error('❌ Error processing CSV data:', error);
        return throwError(() => error);
      })
    );
  }
  
  // Persistence
  saveToLocalStorage(key: string, data: any): void {
    try {
      localStorage.setItem(key, JSON.stringify(data));
    } catch (error) {
      console.error('Error saving to localStorage:', error);
    }
  }
  
  loadFromLocalStorage<T>(key: string): T | null {
    try {
      const data = localStorage.getItem(key);
      return data ? JSON.parse(data) : null;
    } catch (error) {
      console.error('Error loading from localStorage:', error);
      return null;
    }
  }
  
  clearLocalStorage(key: string): void {
    localStorage.removeItem(key);
  }
  
  // Cleanup
  clear(): void {
    this.currentPatternSubject.next(null);
    this.currentProjectId = 0;
    this.currentSiteId = 0;
  }
} 