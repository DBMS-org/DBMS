import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import {
  SiteBlastingData,
  CreateSiteBlastingDataRequest,
  UpdateSiteBlastingDataRequest,
  DrillPattern,
  CreateDrillPatternRequest,
  UpdateDrillPatternRequest,
  BlastSequence,
  CreateBlastSequenceRequest,
  UpdateBlastSequenceRequest,
  DrillPatternResponse,
  BlastSequenceResponse,
  SiteBlastingDataResponse
} from '../models/site-blasting.model';

@Injectable({
  providedIn: 'root'
})
export class SiteBlastingService {
  private readonly apiUrl = `${environment.apiUrl}/api/siteblasting`;

  constructor(private http: HttpClient) {}

  // ========== Site Blasting Data Operations ==========

  // Get workflow data by type for a specific site
  getWorkflowData(projectId: number, siteId: number, dataType: string): Observable<SiteBlastingData> {
    const url = `${this.apiUrl}/projects/${projectId}/sites/${siteId}/data/${dataType}`;
    return this.http.get<SiteBlastingData>(url).pipe(
      map(response => ({
        ...response,
        createdAt: new Date(response.createdAt),
        updatedAt: new Date(response.updatedAt)
      })),
      catchError(this.handleError)
    );
  }

  // Save workflow data for a specific site
  saveWorkflowData(request: CreateSiteBlastingDataRequest): Observable<SiteBlastingData> {
    const url = `${this.apiUrl}/projects/${request.projectId}/sites/${request.siteId}/data`;
    return this.http.post<SiteBlastingData>(url, request).pipe(
      map(response => ({
        ...response,
        createdAt: new Date(response.createdAt),
        updatedAt: new Date(response.updatedAt)
      })),
      catchError(this.handleError)
    );
  }

  // Update workflow data
  updateWorkflowData(projectId: number, siteId: number, dataType: string, request: UpdateSiteBlastingDataRequest): Observable<void> {
    const url = `${this.apiUrl}/projects/${projectId}/sites/${siteId}/data/${dataType}`;
    return this.http.put<void>(url, request).pipe(
      catchError(this.handleError)
    );
  }

  // Get all workflow data for a site
  getAllWorkflowData(projectId: number, siteId: number): Observable<SiteBlastingData[]> {
    const url = `${this.apiUrl}/projects/${projectId}/sites/${siteId}/data`;
    return this.http.get<SiteBlastingData[]>(url).pipe(
      map(data => data.map(item => ({
        ...item,
        createdAt: new Date(item.createdAt),
        updatedAt: new Date(item.updatedAt)
      }))),
      catchError(this.handleError)
    );
  }

  // Delete workflow data by type for a specific site
  deleteWorkflowData(projectId: number, siteId: number, dataType: string): Observable<void> {
    const url = `${this.apiUrl}/projects/${projectId}/sites/${siteId}/data/${dataType}`;
    return this.http.delete<void>(url).pipe(
      catchError(this.handleError)
    );
  }

  // Delete all workflow data for a site
  deleteAllWorkflowData(projectId: number, siteId: number): Observable<void> {
    const url = `${this.apiUrl}/projects/${projectId}/sites/${siteId}/data`;
    return this.http.delete<void>(url).pipe(
      catchError(this.handleError)
    );
  }

  // ========== Drill Pattern Operations ==========

  // Get all drill patterns for a site
  getDrillPatterns(projectId: number, siteId: number): Observable<DrillPattern[]> {
    const url = `${this.apiUrl}/projects/${projectId}/sites/${siteId}/patterns`;
    return this.http.get<DrillPattern[]>(url).pipe(
      map(patterns => patterns.map(pattern => ({
        ...pattern,
        drillPointsJson: typeof pattern.drillPointsJson === 'string' 
          ? JSON.parse(pattern.drillPointsJson) 
          : pattern.drillPointsJson,
        createdAt: new Date(pattern.createdAt),
        updatedAt: new Date(pattern.updatedAt)
      }))),
      catchError(this.handleError)
    );
  }

  // Get specific drill pattern
  getDrillPattern(projectId: number, siteId: number, patternId: number): Observable<DrillPattern> {
    const url = `${this.apiUrl}/patterns/${patternId}`;
    return this.http.get<DrillPattern>(url).pipe(
      map(response => ({
        ...response,
        createdAt: new Date(response.createdAt),
        updatedAt: new Date(response.updatedAt)
      })),
      catchError(this.handleError)
    );
  }

  // Create drill pattern
  createDrillPattern(request: CreateDrillPatternRequest): Observable<DrillPattern> {
    const url = `${this.apiUrl}/projects/${request.projectId}/sites/${request.siteId}/patterns`;
    return this.http.post<DrillPattern>(url, request).pipe(
      map(response => ({
        ...response,
        createdAt: new Date(response.createdAt),
        updatedAt: new Date(response.updatedAt)
      })),
      catchError(this.handleError)
    );
  }

  // Update drill pattern
  updateDrillPattern(projectId: number, siteId: number, patternId: number, request: UpdateDrillPatternRequest): Observable<void> {
    const url = `${this.apiUrl}/patterns/${patternId}`;
    return this.http.put<void>(url, request).pipe(
      catchError(this.handleError)
    );
  }

  // Delete drill pattern
  deleteDrillPattern(projectId: number, siteId: number, patternId: number): Observable<void> {
    const url = `${this.apiUrl}/patterns/${patternId}`;
    return this.http.delete<void>(url).pipe(
      catchError(this.handleError)
    );
  }

  // ========== Blast Sequence Operations ==========

  // Get all blast sequences for a site
  getBlastSequences(projectId: number, siteId: number): Observable<BlastSequence[]> {
    const url = `${this.apiUrl}/projects/${projectId}/sites/${siteId}/sequences`;
    return this.http.get<BlastSequence[]>(url).pipe(
      map(sequences => sequences.map(sequence => ({
        ...sequence,
        connectionsJson: typeof sequence.connectionsJson === 'string' 
          ? JSON.parse(sequence.connectionsJson) 
          : sequence.connectionsJson,
        simulationSettingsJson: typeof sequence.simulationSettingsJson === 'string' 
          ? JSON.parse(sequence.simulationSettingsJson) 
          : sequence.simulationSettingsJson,
        createdAt: new Date(sequence.createdAt),
        updatedAt: new Date(sequence.updatedAt)
      }))),
      catchError(this.handleError)
    );
  }

  // Get specific blast sequence
  getBlastSequence(projectId: number, siteId: number, sequenceId: number): Observable<BlastSequence> {
    const url = `${this.apiUrl}/sequences/${sequenceId}`;
    return this.http.get<BlastSequence>(url).pipe(
      map(response => ({
        ...response,
        createdAt: new Date(response.createdAt),
        updatedAt: new Date(response.updatedAt)
      })),
      catchError(this.handleError)
    );
  }

  // Create blast sequence
  createBlastSequence(request: CreateBlastSequenceRequest): Observable<BlastSequence> {
    const url = `${this.apiUrl}/projects/${request.projectId}/sites/${request.siteId}/sequences`;
    return this.http.post<BlastSequence>(url, request).pipe(
      map(response => ({
        ...response,
        createdAt: new Date(response.createdAt),
        updatedAt: new Date(response.updatedAt)
      })),
      catchError(this.handleError)
    );
  }

  // Update blast sequence
  updateBlastSequence(projectId: number, siteId: number, sequenceId: number, request: UpdateBlastSequenceRequest): Observable<void> {
    const url = `${this.apiUrl}/sequences/${sequenceId}`;
    return this.http.put<void>(url, request).pipe(
      catchError(this.handleError)
    );
  }

  // Delete blast sequence
  deleteBlastSequence(projectId: number, siteId: number, sequenceId: number): Observable<void> {
    const url = `${this.apiUrl}/sequences/${sequenceId}`;
    return this.http.delete<void>(url).pipe(
      catchError(this.handleError)
    );
  }

  // Get blast sequences for a specific drill pattern
  getBlastSequencesByPattern(projectId: number, siteId: number, patternId: number): Observable<BlastSequence[]> {
    // Filter sequences by drill pattern ID client-side since there's no specific endpoint
    return this.getBlastSequences(projectId, siteId).pipe(
      map(sequences => sequences.filter(seq => seq.drillPatternId === patternId))
    );
  }

  // ========== Convenience Methods ==========

  // Save drill pattern from drilling pattern creator
  saveDrillPatternFromCreator(
    projectId: number, 
    siteId: number, 
    name: string, 
    description: string,
    drillPoints: any[], 
    spacing: number, 
    burden: number, 
    depth: number
  ): Observable<DrillPattern> {
    const request: CreateDrillPatternRequest = {
      projectId,
      siteId,
      name,
      description,
      spacing,
      burden,
      depth,
      drillPointsJson: JSON.stringify(drillPoints) // Ensure it's a JSON string
    };
    console.log('Creating drill pattern with request:', request);
    return this.createDrillPattern(request);
  }

  // Save blast sequence from blast sequence designer
  saveBlastSequenceFromDesigner(
    projectId: number,
    siteId: number,
    drillPatternId: number,
    name: string,
    description: string,
    connections: any[],
    simulationSettings: any
  ): Observable<BlastSequence> {
    const request: CreateBlastSequenceRequest = {
      projectId,
      siteId,
      drillPatternId,
      name,
      description,
      connectionsJson: JSON.stringify(connections), // Ensure it's a JSON string
      simulationSettingsJson: JSON.stringify(simulationSettings) // Ensure it's a JSON string
    };
    console.log('Creating blast sequence with request:', request);
    return this.createBlastSequence(request);
  }

  // Save generic workflow state (pattern, connections, simulation_settings, etc.)
  saveWorkflowState(
    projectId: number,
    siteId: number,
    dataType: 'pattern' | 'connections' | 'simulation_settings' | 'simulation_state',
    data: any
  ): Observable<SiteBlastingData> {
    const request: CreateSiteBlastingDataRequest = {
      projectId,
      siteId,
      dataType,
      jsonData: typeof data === 'string' ? data : JSON.stringify(data) // Ensure it's a JSON string
    };
    console.log('Saving workflow state:', { dataType, request });
    return this.saveWorkflowData(request);
  }

  // Get workflow state
  getWorkflowState(
    projectId: number,
    siteId: number,
    dataType: 'pattern' | 'connections' | 'simulation_settings' | 'simulation_state'
  ): Observable<SiteBlastingData> {
    return this.getWorkflowData(projectId, siteId, dataType);
  }

  // Error handling
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred!';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Client Error: ${error.error.message}`;
    } else {
      // Server-side error
      console.error('Full error response:', error);
      
      if (error.status === 404) {
        errorMessage = 'Resource not found';
      } else if (error.status === 400) {
        if (typeof error.error === 'string') {
          errorMessage = `Validation Error: ${error.error}`;
        } else if (error.error && error.error.errors) {
          const validationErrors = Object.values(error.error.errors).flat();
          errorMessage = `Validation Errors: ${validationErrors.join(', ')}`;
        } else if (error.error && error.error.message) {
          errorMessage = `Validation Error: ${error.error.message}`;
        } else {
          errorMessage = 'Invalid request data - please check all required fields';
        }
      } else if (error.status === 500) {
        errorMessage = 'Server error occurred';
      } else {
        errorMessage = `Server Error Code: ${error.status}\nMessage: ${error.message}`;
      }
    }
    
    console.error('SiteBlastingService Error:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
} 