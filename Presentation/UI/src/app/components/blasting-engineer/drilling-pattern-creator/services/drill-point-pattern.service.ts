import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../../../../environments/environment';

// DTOs that match the backend
interface DrillPointDto {
  id: string;
  x: number;
  y: number;
  depth: number;
  spacing: number;
  burden: number;
  projectId: number;
  siteId: number;
  createdAt: Date;
  updatedAt: Date;
}

interface PatternSettingsDto {
  spacing: number;
  burden: number;
  depth: number;
}

interface PatternDataDto {
  drillPoints: DrillPointDto[];
  settings: PatternSettingsDto;
}

interface CreateDrillPointRequest {
  x: number;
  y: number;
  depth: number;
  spacing: number;
  burden: number;
  projectId: number;
  siteId: number;
}

interface SavePatternRequest {
  projectId: number;
  siteId: number;
  drillPoints: DrillPointDto[];
  settings: PatternSettingsDto;
}

interface SavePatternResult {
  success: boolean;
  message: string;
  patternId?: number;
}

@Injectable({
  providedIn: 'root'
})
export class DrillPointPatternService {
  private readonly apiUrl = `${environment.apiUrl}/api/drill-point-pattern`;

  constructor(private http: HttpClient) {}

  // Create drill point
  createDrillPoint(request: CreateDrillPointRequest): Observable<DrillPointDto> {
    return this.http.post<DrillPointDto>(`${this.apiUrl}/drill-points`, request);
  }

  // Update drill point position
  updateDrillPointPosition(pointId: string, x: number, y: number): Observable<boolean> {
    return this.http.put<boolean>(`${this.apiUrl}/drill-points/${pointId}/position`, { x, y });
  }

  // Remove drill point
  removeDrillPoint(pointId: string, projectId: number, siteId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/drill-points/${pointId}?projectId=${projectId}&siteId=${siteId}`);
  }

  // Get all drill points for site
  getDrillPoints(projectId: number, siteId: number): Observable<DrillPointDto[]> {
    return this.http.get<DrillPointDto[]>(`${this.apiUrl}/drill-points?projectId=${projectId}&siteId=${siteId}`);
  }

  // Clear all drill points
  clearAllDrillPoints(projectId: number, siteId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/drill-points?projectId=${projectId}&siteId=${siteId}`);
  }

  // Get pattern data
  getPatternData(projectId: number, siteId: number): Observable<PatternDataDto> {
    return this.http.get<PatternDataDto>(`${this.apiUrl}/pattern-data?projectId=${projectId}&siteId=${siteId}`);
  }

  // Save pattern
  savePattern(request: SavePatternRequest): Observable<SavePatternResult> {
    return this.http.post<SavePatternResult>(`${this.apiUrl}/save-pattern`, request);
  }

  // Get pattern settings
  getPatternSettings(projectId: number, siteId: number): Observable<PatternSettingsDto> {
    return this.http.get<PatternSettingsDto>(`${this.apiUrl}/pattern-settings?projectId=${projectId}&siteId=${siteId}`);
  }

  // Update pattern settings
  updatePatternSettings(projectId: number, siteId: number, settings: PatternSettingsDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/pattern-settings?projectId=${projectId}&siteId=${siteId}`, settings);
  }

  // Process CSV data
  processUploadedCsvData(projectId: number, siteId: number, csvData: any[]): Observable<PatternDataDto> {
    return this.http.post<PatternDataDto>(`${this.apiUrl}/process-csv`, {
      projectId,
      siteId,
      csvData
    });
  }

  // Calculate grid pitch
  calculateGridPitch(projectId: number, siteId: number): Observable<{spacing: number, burden: number}> {
    return this.http.get<{spacing: number, burden: number}>(`${this.apiUrl}/calculate-grid-pitch?projectId=${projectId}&siteId=${siteId}`);
  }

  // Export for blast designer
  exportForBlastDesigner(projectId: number, siteId: number): Observable<PatternDataDto> {
    return this.http.get<PatternDataDto>(`${this.apiUrl}/export-for-blast-designer?projectId=${projectId}&siteId=${siteId}`);
  }

  // Validation methods
  validateCoordinates(x: number, y: number): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/validate-coordinates`, { x, y });
  }

  validateUniqueCoordinates(x: number, y: number, projectId: number, siteId: number, excludePointId?: string): Observable<boolean> {
    const params = new URLSearchParams();
    params.append('x', x.toString());
    params.append('y', y.toString());
    params.append('projectId', projectId.toString());
    params.append('siteId', siteId.toString());
    if (excludePointId) {
      params.append('excludePointId', excludePointId);
    }
    
    return this.http.get<boolean>(`${this.apiUrl}/validate-unique-coordinates?${params.toString()}`);
  }

  validateDrillPointCount(projectId: number, siteId: number, maxPoints: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/validate-drill-point-count?projectId=${projectId}&siteId=${siteId}&maxPoints=${maxPoints}`);
  }
} 