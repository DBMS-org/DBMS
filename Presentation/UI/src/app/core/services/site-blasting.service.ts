import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  SiteBlastingData,
  CreateSiteBlastingDataRequest,
  UpdateSiteBlastingDataRequest,
  BlastSequence,
  CreateBlastSequenceRequest,
  UpdateBlastSequenceRequest,
  BlastConnection,
  CreateBlastConnectionRequest,
  UpdateBlastConnectionRequest,
  WorkflowState
} from '../models/site-blasting.model';

@Injectable({
  providedIn: 'root'
})
export class SiteBlastingService {
  private apiUrl = `${environment.apiUrl}/api/siteblasting`;

  constructor(private http: HttpClient) {}

  // Site Blasting Data Methods
  getSiteData(projectId: number, siteId: number, dataType: string): Observable<SiteBlastingData> {
    return this.http.get<SiteBlastingData>(`${this.apiUrl}/projects/${projectId}/sites/${siteId}/data/${dataType}`);
  }

  getAllSiteData(projectId: number, siteId: number): Observable<SiteBlastingData[]> {
    return this.http.get<SiteBlastingData[]>(`${this.apiUrl}/projects/${projectId}/sites/${siteId}/data`);
  }

  saveSiteData(request: CreateSiteBlastingDataRequest): Observable<SiteBlastingData> {
    return this.http.post<SiteBlastingData>(`${this.apiUrl}/data`, request);
  }

  updateSiteData(projectId: number, siteId: number, dataType: string, request: UpdateSiteBlastingDataRequest): Observable<SiteBlastingData> {
    return this.http.put<SiteBlastingData>(`${this.apiUrl}/projects/${projectId}/sites/${siteId}/data/${dataType}`, request);
  }

  deleteSiteData(projectId: number, siteId: number, dataType: string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/projects/${projectId}/sites/${siteId}/data/${dataType}`);
  }

  deleteAllSiteData(projectId: number, siteId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/projects/${projectId}/sites/${siteId}/data`);
  }

  // Blast Sequence Methods
  getBlastSequences(projectId: number, siteId: number): Observable<BlastSequence[]> {
    return this.http.get<BlastSequence[]>(`${this.apiUrl}/projects/${projectId}/sites/${siteId}/sequences`);
  }

  getBlastSequence(id: number): Observable<BlastSequence> {
    return this.http.get<BlastSequence>(`${this.apiUrl}/sequences/${id}`);
  }

  createBlastSequence(request: CreateBlastSequenceRequest): Observable<BlastSequence> {
    return this.http.post<BlastSequence>(`${this.apiUrl}/projects/${request.projectId}/sites/${request.siteId}/sequences`, request);
  }

  updateBlastSequence(id: number, request: UpdateBlastSequenceRequest): Observable<BlastSequence> {
    return this.http.put<BlastSequence>(`${this.apiUrl}/sequences/${id}`, request);
  }

  deleteBlastSequence(id: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/sequences/${id}`);
  }

  // Blast Connection Methods
  getBlastConnections(projectId: number, siteId: number): Observable<BlastConnection[]> {
    return this.http.get<BlastConnection[]>(`${this.apiUrl}/projects/${projectId}/sites/${siteId}/connections`);
  }

  getBlastConnection(id: string, projectId: number, siteId: number): Observable<BlastConnection> {
    return this.http.get<BlastConnection>(`${this.apiUrl}/projects/${projectId}/sites/${siteId}/connections/${id}`);
  }

  getBlastConnectionsBySequence(projectId: number, siteId: number, sequence: number): Observable<BlastConnection[]> {
    return this.http.get<BlastConnection[]>(`${this.apiUrl}/projects/${projectId}/sites/${siteId}/connections/sequence/${sequence}`);
  }

  createBlastConnection(request: CreateBlastConnectionRequest): Observable<BlastConnection> {
    return this.http.post<BlastConnection>(`${this.apiUrl}/projects/${request.projectId}/sites/${request.siteId}/connections`, request);
  }

  updateBlastConnection(id: string, projectId: number, siteId: number, request: UpdateBlastConnectionRequest): Observable<BlastConnection> {
    return this.http.put<BlastConnection>(`${this.apiUrl}/projects/${projectId}/sites/${siteId}/connections/${id}`, request);
  }

  deleteBlastConnection(id: string, projectId: number, siteId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/projects/${projectId}/sites/${siteId}/connections/${id}`);
  }

  // Workflow State Methods
  getWorkflowState(projectId: number, siteId: number): Observable<WorkflowState> {
    return this.http.get<WorkflowState>(`${this.apiUrl}/projects/${projectId}/sites/${siteId}/data/pattern`);
  }

  updateWorkflowState(projectId: number, siteId: number, workflowState: WorkflowState): Observable<WorkflowState> {
    const request: CreateSiteBlastingDataRequest = {
      projectId,
      siteId,
      dataType: 'pattern',
      jsonData: workflowState
    };
    return this.http.post<WorkflowState>(`${this.apiUrl}/data`, request);
  }

  // Utility Methods
  deleteAllWorkflowData(projectId: number, siteId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/projects/${projectId}/sites/${siteId}/data`);
  }

  hasWorkflowData(projectId: number, siteId: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/projects/${projectId}/sites/${siteId}/has-data`);
  }
} 