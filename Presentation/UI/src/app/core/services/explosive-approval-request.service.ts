import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface ExplosiveApprovalRequest {
  id: number;
  projectSiteId: number;
  requestedByUserId: number;
  expectedUsageDate: string;
  blastingDate?: string;
  blastTiming?: string;
  comments?: string;
  status: 'Pending' | 'Approved' | 'Rejected' | 'Cancelled' | 'Expired';
  priority: 'Low' | 'Normal' | 'High' | 'Critical';
  approvalType: 'Standard' | 'Emergency' | 'Maintenance' | 'Testing' | 'Research';
  processedByUserId?: number;
  processedAt?: string;
  rejectionReason?: string;
  additionalData?: string;
  estimatedDurationHours?: number;
  safetyChecklistCompleted: boolean;
  environmentalAssessmentCompleted: boolean;
  createdAt: string;
  updatedAt: string;
  isActive: boolean;
  
  // Navigation properties
  projectSite?: {
    id: number;
    name: string;
    project: {
      id: number;
      name: string;
      region: string;
      regionId: number;
    };
  };
  requestedByUser?: {
    id: number;
    name: string;
    email: string;
    region: string;
  };
  processedByUser?: {
    id: number;
    name: string;
    email: string;
  };
}

export interface CreateExplosiveApprovalRequestDto {
  projectSiteId: number;
  expectedUsageDate: string;
  comments?: string;
  priority: 'Low' | 'Normal' | 'High' | 'Critical';
  approvalType: 'Standard' | 'Emergency' | 'Maintenance' | 'Testing' | 'Research';
}

@Injectable({
  providedIn: 'root'
})
export class ExplosiveApprovalRequestService {
  private readonly apiUrl = `${environment.apiUrl}/api/explosive-approval-requests`;

  constructor(private http: HttpClient) {}

  /**
   * Get explosive approval request by ID
   */
  getExplosiveApprovalRequest(id: number): Observable<ExplosiveApprovalRequest> {
    const headers = {
      'Cache-Control': 'no-cache, no-store, must-revalidate',
      'Pragma': 'no-cache',
      'Expires': '0'
    };
    
    return this.http.get<ExplosiveApprovalRequest>(`${this.apiUrl}/${id}`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Get explosive approval requests by project site ID
   */
  getExplosiveApprovalRequestsByProjectSite(projectSiteId: number): Observable<ExplosiveApprovalRequest[]> {
    const headers = {
      'Cache-Control': 'no-cache, no-store, must-revalidate',
      'Pragma': 'no-cache',
      'Expires': '0'
    };
    
    return this.http.get<ExplosiveApprovalRequest[]>(`${this.apiUrl}/project-site/${projectSiteId}`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Get explosive approval requests for store manager by region
   */
  getExplosiveApprovalRequestsByRegion(region: string): Observable<ExplosiveApprovalRequest[]> {
    const headers = {
      'Cache-Control': 'no-cache, no-store, must-revalidate',
      'Pragma': 'no-cache',
      'Expires': '0'
    };
    
    return this.http.get<ExplosiveApprovalRequest[]>(`${this.apiUrl}/store-manager/region/${encodeURIComponent(region)}`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Get current user's explosive approval requests
   */
  getMyExplosiveApprovalRequests(): Observable<ExplosiveApprovalRequest[]> {
    const headers = {
      'Cache-Control': 'no-cache, no-store, must-revalidate',
      'Pragma': 'no-cache',
      'Expires': '0'
    };
    
    return this.http.get<ExplosiveApprovalRequest[]>(`${this.apiUrl}/my-requests`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Get all pending explosive approval requests
   */
  getPendingExplosiveApprovalRequests(): Observable<ExplosiveApprovalRequest[]> {
    const headers = {
      'Cache-Control': 'no-cache, no-store, must-revalidate',
      'Pragma': 'no-cache',
      'Expires': '0'
    };
    
    return this.http.get<ExplosiveApprovalRequest[]>(`${this.apiUrl}/pending`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Create a new explosive approval request
   */
  createExplosiveApprovalRequest(request: CreateExplosiveApprovalRequestDto): Observable<ExplosiveApprovalRequest> {
    return this.http.post<ExplosiveApprovalRequest>(this.apiUrl, request).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Approve an explosive approval request
   */
  approveExplosiveApprovalRequest(requestId: number, comments?: string): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/${requestId}/approve`, { comments }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Reject an explosive approval request
   */
  rejectExplosiveApprovalRequest(requestId: number, rejectionReason: string): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/${requestId}/reject`, { rejectionReason }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Cancel an explosive approval request
   */
  cancelExplosiveApprovalRequest(requestId: number): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/${requestId}/cancel`, {}).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Delete an explosive approval request
   */
  deleteExplosiveApprovalRequest(requestId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/${requestId}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = error.error?.message || `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    
    console.error('ExplosiveApprovalRequestService Error:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}