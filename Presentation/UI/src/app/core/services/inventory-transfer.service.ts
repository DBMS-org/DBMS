import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import {
  InventoryTransferRequest,
  CreateTransferRequest,
  ApproveTransferRequest,
  RejectTransferRequest,
  DispatchTransferRequest,
  TransferRequestFilter,
  PagedList
} from '../models/inventory-transfer.model';

@Injectable({
  providedIn: 'root'
})
export class InventoryTransferService {
  private readonly apiUrl = `${environment.apiUrl}/api/InventoryTransfer`;

  constructor(private http: HttpClient) {}

  /**
   * Get paginated transfer requests with filtering
   */
  getTransferRequests(filter: TransferRequestFilter = {}): Observable<PagedList<InventoryTransferRequest>> {
    let params = new HttpParams();

    if (filter.pageNumber) params = params.set('pageNumber', filter.pageNumber.toString());
    if (filter.pageSize) params = params.set('pageSize', filter.pageSize.toString());
    if (filter.status) params = params.set('status', filter.status);
    if (filter.destinationStoreId) params = params.set('destinationStoreId', filter.destinationStoreId.toString());
    if (filter.requestedByUserId) params = params.set('requestedByUserId', filter.requestedByUserId.toString());
    if (filter.isOverdue !== undefined) params = params.set('isOverdue', filter.isOverdue.toString());
    if (filter.isUrgent !== undefined) params = params.set('isUrgent', filter.isUrgent.toString());
    if (filter.sortBy) params = params.set('sortBy', filter.sortBy);
    if (filter.sortDescending !== undefined) params = params.set('sortDescending', filter.sortDescending.toString());

    return this.http.get<any>(this.apiUrl, { params }).pipe(
      map(response => this.extractPagedData<InventoryTransferRequest>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Get transfer request by ID
   */
  getTransferRequestById(id: number): Observable<InventoryTransferRequest> {
    return this.http.get<any>(`${this.apiUrl}/${id}`).pipe(
      map(response => this.extractData<InventoryTransferRequest>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Get pending transfer requests
   */
  getPendingRequests(): Observable<InventoryTransferRequest[]> {
    return this.http.get<any>(`${this.apiUrl}/pending`).pipe(
      map(response => this.extractArrayData<InventoryTransferRequest>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Get urgent transfer requests
   */
  getUrgentRequests(): Observable<InventoryTransferRequest[]> {
    return this.http.get<any>(`${this.apiUrl}/urgent`).pipe(
      map(response => this.extractArrayData<InventoryTransferRequest>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Get overdue transfer requests
   */
  getOverdueRequests(): Observable<InventoryTransferRequest[]> {
    return this.http.get<any>(`${this.apiUrl}/overdue`).pipe(
      map(response => this.extractArrayData<InventoryTransferRequest>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Get transfer requests by user
   */
  getRequestsByUser(userId: number): Observable<InventoryTransferRequest[]> {
    return this.http.get<any>(`${this.apiUrl}/user/${userId}`).pipe(
      map(response => this.extractArrayData<InventoryTransferRequest>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Get current user's transfer requests
   */
  getMyRequests(): Observable<InventoryTransferRequest[]> {
    return this.http.get<any>(`${this.apiUrl}/my-requests`).pipe(
      map(response => this.extractArrayData<InventoryTransferRequest>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Get transfer requests by store
   */
  getRequestsByStore(storeId: number): Observable<InventoryTransferRequest[]> {
    return this.http.get<any>(`${this.apiUrl}/store/${storeId}`).pipe(
      map(response => this.extractArrayData<InventoryTransferRequest>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Create new transfer request
   */
  createTransferRequest(request: CreateTransferRequest): Observable<InventoryTransferRequest> {
    return this.http.post<any>(this.apiUrl, request).pipe(
      map(response => this.extractData<InventoryTransferRequest>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Approve transfer request
   */
  approveTransferRequest(id: number, request: ApproveTransferRequest): Observable<InventoryTransferRequest> {
    return this.http.post<any>(`${this.apiUrl}/${id}/approve`, request).pipe(
      map(response => this.extractData<InventoryTransferRequest>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Reject transfer request
   */
  rejectTransferRequest(id: number, request: RejectTransferRequest): Observable<InventoryTransferRequest> {
    return this.http.post<any>(`${this.apiUrl}/${id}/reject`, request).pipe(
      map(response => this.extractData<InventoryTransferRequest>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Dispatch transfer request
   */
  dispatchTransferRequest(id: number, request: DispatchTransferRequest): Observable<InventoryTransferRequest> {
    return this.http.post<any>(`${this.apiUrl}/${id}/dispatch`, request).pipe(
      map(response => this.extractData<InventoryTransferRequest>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Confirm delivery of transfer request
   */
  confirmDelivery(id: number): Observable<InventoryTransferRequest> {
    return this.http.post<any>(`${this.apiUrl}/${id}/confirm-delivery`, {}).pipe(
      map(response => this.extractData<InventoryTransferRequest>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Complete transfer request
   */
  completeTransferRequest(id: number): Observable<InventoryTransferRequest> {
    return this.http.post<any>(`${this.apiUrl}/${id}/complete`, {}).pipe(
      map(response => this.extractData<InventoryTransferRequest>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Cancel transfer request
   */
  cancelTransferRequest(id: number, reason: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/cancel`, { reason }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Extract data from API response wrapper
   */
  private extractData<T>(response: any): T {
    return response.data || response;
  }

  /**
   * Extract array data from API response wrapper
   */
  private extractArrayData<T>(response: any): T[] {
    return response.data || response;
  }

  /**
   * Extract paged data from API response wrapper
   */
  private extractPagedData<T>(response: any): PagedList<T> {
    const data = response.data || response;
    return {
      items: data.items || [],
      pageNumber: data.pageNumber || 1,
      pageSize: data.pageSize || 10,
      totalCount: data.totalCount || 0,
      totalPages: data.totalPages || 0,
      hasPreviousPage: data.hasPreviousPage || false,
      hasNextPage: data.hasNextPage || false
    };
  }

  /**
   * Handle HTTP errors
   */
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      if (error.error && error.error.message) {
        errorMessage = error.error.message;
      } else {
        errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      }
    }

    console.error('InventoryTransferService Error:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
