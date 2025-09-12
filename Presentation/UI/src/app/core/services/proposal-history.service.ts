import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface ProposalHistoryItem {
  id: number;
  projectSiteId: number;
  projectSiteName?: string;
  requestedByUserId: number;
  requestedByUserName?: string;
  expectedUsageDate: Date;
  comments?: string;

  approvalType: ExplosiveApprovalType;
  status: ExplosiveApprovalStatus;
  createdAt: Date;
  updatedAt: Date;
  approvedAt?: Date;
  approvedByUserId?: number;
  approvedByUserName?: string;
  rejectedAt?: Date;
  rejectedByUserId?: number;
  rejectedByUserName?: string;
  rejectionReason?: string;
  approvalComments?: string;
}



export enum ExplosiveApprovalType {
  Standard = 0,
  Emergency = 1,
  Bulk = 2
}

export enum ExplosiveApprovalStatus {
  Pending = 0,
  Approved = 1,
  Rejected = 2,
  Cancelled = 3
}

export interface CreateProposalRequest {
  projectSiteId: number;
  expectedUsageDate: Date;
  comments?: string;

  approvalType: ExplosiveApprovalType;
}

export interface UpdateProposalRequest {
  expectedUsageDate: Date;
  comments?: string;

  approvalType: ExplosiveApprovalType;
}

@Injectable({
  providedIn: 'root'
})
export class ProposalHistoryService {
  private readonly apiUrl = `${environment.apiUrl}/api/explosive-approval-requests`;

  constructor(private http: HttpClient) {}

  private getHttpOptions(): { headers: HttpHeaders } {
    const token = localStorage.getItem('authToken');
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': token ? `Bearer ${token}` : ''
      })
    };
  }

  /**
   * Get all explosive approval requests for the current user's project sites
   */
  getUserProposals(): Observable<ProposalHistoryItem[]> {
    return this.http.get<ProposalHistoryItem[]>(
      `${this.apiUrl}/user-requests`,
      this.getHttpOptions()
    ).pipe(
      map(proposals => proposals.map(proposal => ({
        ...proposal,
        expectedUsageDate: new Date(proposal.expectedUsageDate),
        createdAt: new Date(proposal.createdAt),
        updatedAt: new Date(proposal.updatedAt),
        approvedAt: proposal.approvedAt ? new Date(proposal.approvedAt) : undefined,
        rejectedAt: proposal.rejectedAt ? new Date(proposal.rejectedAt) : undefined
      }))),
      catchError(this.handleError)
    );
  }

  /**
   * Get explosive approval requests by project site ID
   */
  getProposalsByProjectSite(projectSiteId: number): Observable<ProposalHistoryItem[]> {
    return this.http.get<ProposalHistoryItem[]>(
      `${this.apiUrl}/project-site/${projectSiteId}`,
      this.getHttpOptions()
    ).pipe(
      map(proposals => proposals.map(proposal => ({
        ...proposal,
        expectedUsageDate: new Date(proposal.expectedUsageDate),
        createdAt: new Date(proposal.createdAt),
        updatedAt: new Date(proposal.updatedAt),
        approvedAt: proposal.approvedAt ? new Date(proposal.approvedAt) : undefined,
        rejectedAt: proposal.rejectedAt ? new Date(proposal.rejectedAt) : undefined
      }))),
      catchError(this.handleError)
    );
  }

  /**
   * Get all pending explosive approval requests
   */
  getPendingProposals(): Observable<ProposalHistoryItem[]> {
    return this.http.get<ProposalHistoryItem[]>(
      `${this.apiUrl}/pending`,
      this.getHttpOptions()
    ).pipe(
      map(proposals => proposals.map(proposal => ({
        ...proposal,
        expectedUsageDate: new Date(proposal.expectedUsageDate),
        createdAt: new Date(proposal.createdAt),
        updatedAt: new Date(proposal.updatedAt),
        approvedAt: proposal.approvedAt ? new Date(proposal.approvedAt) : undefined,
        rejectedAt: proposal.rejectedAt ? new Date(proposal.rejectedAt) : undefined
      }))),
      catchError(this.handleError)
    );
  }

  /**
   * Get a specific explosive approval request by ID
   */
  getProposalById(id: number): Observable<ProposalHistoryItem> {
    return this.http.get<ProposalHistoryItem>(
      `${this.apiUrl}/${id}`,
      this.getHttpOptions()
    ).pipe(
      map(proposal => ({
        ...proposal,
        expectedUsageDate: new Date(proposal.expectedUsageDate),
        createdAt: new Date(proposal.createdAt),
        updatedAt: new Date(proposal.updatedAt),
        approvedAt: proposal.approvedAt ? new Date(proposal.approvedAt) : undefined,
        rejectedAt: proposal.rejectedAt ? new Date(proposal.rejectedAt) : undefined
      })),
      catchError(this.handleError)
    );
  }

  /**
   * Create a new explosive approval request
   */
  createProposal(request: CreateProposalRequest): Observable<ProposalHistoryItem> {
    return this.http.post<ProposalHistoryItem>(
      this.apiUrl,
      request,
      this.getHttpOptions()
    ).pipe(
      map(proposal => ({
        ...proposal,
        expectedUsageDate: new Date(proposal.expectedUsageDate),
        createdAt: new Date(proposal.createdAt),
        updatedAt: new Date(proposal.updatedAt),
        approvedAt: proposal.approvedAt ? new Date(proposal.approvedAt) : undefined,
        rejectedAt: proposal.rejectedAt ? new Date(proposal.rejectedAt) : undefined
      })),
      catchError(this.handleError)
    );
  }

  /**
   * Update an existing explosive approval request
   */
  updateProposal(id: number, request: UpdateProposalRequest): Observable<ProposalHistoryItem> {
    return this.http.put<ProposalHistoryItem>(
      `${this.apiUrl}/${id}`,
      request,
      this.getHttpOptions()
    ).pipe(
      map(proposal => ({
        ...proposal,
        expectedUsageDate: new Date(proposal.expectedUsageDate),
        createdAt: new Date(proposal.createdAt),
        updatedAt: new Date(proposal.updatedAt),
        approvedAt: proposal.approvedAt ? new Date(proposal.approvedAt) : undefined,
        rejectedAt: proposal.rejectedAt ? new Date(proposal.rejectedAt) : undefined
      })),
      catchError(this.handleError)
    );
  }

  /**
   * Cancel an explosive approval request
   */
  cancelProposal(id: number): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/${id}/cancel`,
      {},
      this.getHttpOptions()
    ).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Delete an explosive approval request
   */
  deleteProposal(id: number): Observable<any> {
    return this.http.delete(
      `${this.apiUrl}/${id}`,
      this.getHttpOptions()
    ).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Get the latest explosive approval request for a project site
   */
  getLatestProposalForSite(projectSiteId: number): Observable<ProposalHistoryItem> {
    return this.http.get<ProposalHistoryItem>(
      `${this.apiUrl}/project-site/${projectSiteId}/latest`,
      this.getHttpOptions()
    ).pipe(
      map(proposal => ({
        ...proposal,
        expectedUsageDate: new Date(proposal.expectedUsageDate),
        createdAt: new Date(proposal.createdAt),
        updatedAt: new Date(proposal.updatedAt),
        approvedAt: proposal.approvedAt ? new Date(proposal.approvedAt) : undefined,
        rejectedAt: proposal.rejectedAt ? new Date(proposal.rejectedAt) : undefined
      })),
      catchError(this.handleError)
    );
  }

  /**
   * Check if a project site has pending explosive approval requests
   */
  hasPendingProposalForSite(projectSiteId: number): Observable<{ hasPendingRequest: boolean }> {
    return this.http.get<{ hasPendingRequest: boolean }>(
      `${this.apiUrl}/project-site/${projectSiteId}/has-pending`,
      this.getHttpOptions()
    ).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Get status display text
   */
  getStatusText(status: ExplosiveApprovalStatus): string {
    switch (status) {
      case ExplosiveApprovalStatus.Pending:
        return 'Pending';
      case ExplosiveApprovalStatus.Approved:
        return 'Approved';
      case ExplosiveApprovalStatus.Rejected:
        return 'Rejected';
      case ExplosiveApprovalStatus.Cancelled:
        return 'Cancelled';
      default:
        return 'Unknown';
    }
  }



  /**
   * Get approval type display text
   */
  getApprovalTypeText(type: ExplosiveApprovalType): string {
    switch (type) {
      case ExplosiveApprovalType.Standard:
        return 'Standard';
      case ExplosiveApprovalType.Emergency:
        return 'Emergency';
      case ExplosiveApprovalType.Bulk:
        return 'Bulk';
      default:
        return 'Standard';
    }
  }

  private handleError(error: any): Observable<never> {
    console.error('ProposalHistoryService error:', error);
    let errorMessage = 'An error occurred while processing your request.';
    
    if (error.error?.message) {
      errorMessage = error.error.message;
    } else if (error.message) {
      errorMessage = error.message;
    }
    
    return throwError(() => new Error(errorMessage));
  }
}