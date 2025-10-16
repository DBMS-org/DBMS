import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface ProjectSite {
  id: number;
  projectId: number;
  name: string;
  location: string;
  coordinates?: {
    latitude: number;
    longitude: number;
  };
  status: string;
  description: string;
  isPatternApproved: boolean;
  isSimulationConfirmed: boolean;
  isOperatorCompleted: boolean;
  isExplosiveApprovalRequested: boolean;
  explosiveApprovalRequestDate?: Date;
  expectedExplosiveUsageDate?: Date;
  explosiveApprovalComments?: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateSiteRequest {
  projectId: number;
  name: string;
  location: string;
  coordinates?: {
    latitude: number;
    longitude: number;
  };
  status: string;
  description: string;
}

@Injectable({
  providedIn: 'root'
})
export class SiteService {
  private apiUrl = `${environment.apiUrl}/api/projectsites`;

  constructor(private http: HttpClient) {}

  private getHttpOptions() {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Cache-Control': 'no-cache, no-store, must-revalidate',
        'Pragma': 'no-cache',
        'Expires': '0'
      })
    };
  }

  // Get all sites for a specific project
  getProjectSites(projectId: number): Observable<ProjectSite[]> {
    const url = `${environment.apiUrl}/api/projectsites/project/${projectId}`;
    return this.http.get<ProjectSite[]>(url, this.getHttpOptions())
      .pipe(
        catchError(this.handleError)
      );
  }

  // Get all sites
  getAllSites(): Observable<ProjectSite[]> {
    return this.http.get<ProjectSite[]>(this.apiUrl, this.getHttpOptions())
      .pipe(
        catchError(this.handleError)
      );
  }

  // Get a specific site by ID
  getSite(id: number): Observable<ProjectSite> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<ProjectSite>(url, this.getHttpOptions())
      .pipe(
        catchError(this.handleError)
      );
  }

  // Create a new site
  createSite(site: CreateSiteRequest): Observable<ProjectSite> {
    return this.http.post<ProjectSite>(this.apiUrl, site, this.getHttpOptions())
      .pipe(
        catchError(this.handleError)
      );
  }

  // Update an existing site
  updateSite(id: number, site: ProjectSite): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.put(url, site, this.getHttpOptions())
      .pipe(
        catchError(this.handleError)
      );
  }

  // Delete a site
  deleteSite(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete(url, this.getHttpOptions())
      .pipe(
        catchError(this.handleError)
      );
  }

  // Approve pattern for operator
  approvePattern(siteId: number) {
    const url = `${this.apiUrl}/${siteId}/approve`;
    return this.http.post(url, {}, this.getHttpOptions());
  }

  // Revoke pattern
  revokePattern(siteId: number) {
    const url = `${this.apiUrl}/${siteId}/revoke`;
    return this.http.post(url, {}, this.getHttpOptions());
  }

  // Confirm simulation for admin
  confirmSimulation(siteId: number) {
    const url = `${this.apiUrl}/${siteId}/confirm-simulation`;
    return this.http.post(url, {}, this.getHttpOptions());
  }

  revokeSimulation(siteId: number) {
    const url = `${this.apiUrl}/${siteId}/revoke-simulation`;
    return this.http.post(url, {}, this.getHttpOptions());
  }

  // Request explosive approval from store manager
  requestExplosiveApproval(
    siteId: number,
    expectedUsageDate: string,
    comments?: string,
    blastingDate?: string,
    blastTiming?: string
  ) {
    const url = `${environment.apiUrl}/api/explosive-approval-requests`;
    return this.http.post(url, {
      ProjectSiteId: siteId,
      ExpectedUsageDate: expectedUsageDate,
      Comments: comments,
      BlastingDate: blastingDate,
      BlastTiming: blastTiming
    }, this.getHttpOptions());
  }

  // Check if there's a pending explosive approval request for a site
  hasPendingExplosiveApprovalRequest(siteId: number) {
    const url = `${environment.apiUrl}/api/explosive-approval-requests/project-site/${siteId}/has-pending`;
    return this.http.get<{hasPendingRequest: boolean}>(url, this.getHttpOptions());
  }

  // Revoke explosive approval request
  revokeExplosiveApprovalRequest(siteId: number) {
    // First get the latest request for this site, then cancel it
    const getLatestUrl = `${environment.apiUrl}/api/explosive-approval-requests/project-site/${siteId}/latest`;
    return this.http.get<any>(getLatestUrl, this.getHttpOptions())
      .pipe(
        catchError(() => throwError(() => new Error('No explosive approval request found'))),
        // If we get a request, cancel it
        switchMap((request: any) => {
          const cancelUrl = `${environment.apiUrl}/api/explosive-approval-requests/${request.id}/cancel`;
          return this.http.post(cancelUrl, {}, this.getHttpOptions());
        })
      );
  }

  // Complete site (mark as operator completed)
  completeSite(siteId: number) {
    const url = `${this.apiUrl}/${siteId}/complete`;
    return this.http.post(url, {}, this.getHttpOptions())
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(error: any) {
    console.error('An error occurred:', error);
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      console.error('Client-side error:', error.error.message);
    } else {
      // Server-side error
      console.error(`Backend returned code ${error.status}, body was:`, error.error);
    }
    
    return throwError(() => error);
  }
}