import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DrillPointPatternService {
  private readonly apiUrl = `${environment.apiUrl}/api/DrillPointPattern`;

  constructor(private http: HttpClient) {}

  // Fetch pattern data
  getPattern(projectId: number, siteId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/pattern-data`, { params: { projectId, siteId } });
  }

  // Save (create or update) a drill pattern
  savePattern(request: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/save-pattern`, request);
  }

  // Add a single drill point
  addPoint(request: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/drill-points`, request);
  }

  // Update a drill point position
  updatePoint(request: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/drill-points/${request.id}/position`, request);
  }

  // Delete a drill point by ID
  deletePoint(projectId: number, siteId: number, pointId: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/drill-points/${pointId}`, { params: { projectId, siteId } });
  }

  // Clear all points for a pattern
  clearAll(projectId: number, siteId: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/drill-points`, { params: { projectId, siteId } });
  }

  // Process uploaded CSV data on the server
  processCsvUpload(request: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/process-csv`, request);
  }
} 