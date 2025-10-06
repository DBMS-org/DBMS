import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import {
  CentralInventory,
  CreateANFOInventoryRequest,
  CreateEmulsionInventoryRequest,
  UpdateANFOInventoryRequest,
  UpdateEmulsionInventoryRequest,
  InventoryDashboard,
  InventoryFilter,
  PagedList,
  ExplosiveType
} from '../models/central-inventory.model';

@Injectable({
  providedIn: 'root'
})
export class CentralInventoryService {
  private readonly apiUrl = `${environment.apiUrl}/api/CentralInventory`;

  constructor(private http: HttpClient) {}

  /**
   * Get paginated inventory with filtering
   */
  getInventory(filter: InventoryFilter = {}): Observable<PagedList<CentralInventory>> {
    let params = new HttpParams();

    if (filter.pageNumber) params = params.set('pageNumber', filter.pageNumber.toString());
    if (filter.pageSize) params = params.set('pageSize', filter.pageSize.toString());
    if (filter.explosiveType) params = params.set('explosiveType', filter.explosiveType);
    if (filter.status) params = params.set('status', filter.status);
    if (filter.supplier) params = params.set('supplier', filter.supplier);
    if (filter.batchId) params = params.set('batchId', filter.batchId);
    if (filter.isExpiringSoon !== undefined) params = params.set('isExpiringSoon', filter.isExpiringSoon.toString());
    if (filter.sortBy) params = params.set('sortBy', filter.sortBy);
    if (filter.sortDescending !== undefined) params = params.set('sortDescending', filter.sortDescending.toString());

    return this.http.get<any>(this.apiUrl, { params }).pipe(
      map(response => this.extractPagedData<CentralInventory>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Get inventory by ID
   */
  getInventoryById(id: number): Observable<CentralInventory> {
    return this.http.get<any>(`${this.apiUrl}/${id}`).pipe(
      map(response => this.extractData<CentralInventory>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Get inventory by batch ID
   */
  getInventoryByBatchId(batchId: string): Observable<CentralInventory> {
    return this.http.get<any>(`${this.apiUrl}/batch/${batchId}`).pipe(
      map(response => this.extractData<CentralInventory>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Get inventory by explosive type
   */
  getInventoryByType(type: ExplosiveType): Observable<CentralInventory[]> {
    return this.http.get<any>(`${this.apiUrl}/type/${type}`).pipe(
      map(response => this.extractArrayData<CentralInventory>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Get dashboard statistics
   */
  getDashboard(): Observable<InventoryDashboard> {
    return this.http.get<any>(`${this.apiUrl}/dashboard`).pipe(
      map(response => this.extractData<InventoryDashboard>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Get expiring batches
   */
  getExpiringBatches(daysThreshold: number = 30): Observable<CentralInventory[]> {
    const params = new HttpParams().set('daysThreshold', daysThreshold.toString());
    return this.http.get<any>(`${this.apiUrl}/expiring`, { params }).pipe(
      map(response => this.extractArrayData<CentralInventory>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Get expired batches
   */
  getExpiredBatches(): Observable<CentralInventory[]> {
    return this.http.get<any>(`${this.apiUrl}/expired`).pipe(
      map(response => this.extractArrayData<CentralInventory>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Create ANFO batch
   */
  createANFOBatch(request: CreateANFOInventoryRequest): Observable<CentralInventory> {
    return this.http.post<any>(`${this.apiUrl}/anfo`, request).pipe(
      map(response => this.extractData<CentralInventory>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Create Emulsion batch
   */
  createEmulsionBatch(request: CreateEmulsionInventoryRequest): Observable<CentralInventory> {
    return this.http.post<any>(`${this.apiUrl}/emulsion`, request).pipe(
      map(response => this.extractData<CentralInventory>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Update ANFO batch
   */
  updateANFOBatch(id: number, request: UpdateANFOInventoryRequest): Observable<CentralInventory> {
    return this.http.put<any>(`${this.apiUrl}/anfo/${id}`, request).pipe(
      map(response => this.extractData<CentralInventory>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Update Emulsion batch
   */
  updateEmulsionBatch(id: number, request: UpdateEmulsionInventoryRequest): Observable<CentralInventory> {
    return this.http.put<any>(`${this.apiUrl}/emulsion/${id}`, request).pipe(
      map(response => this.extractData<CentralInventory>(response)),
      catchError(this.handleError)
    );
  }

  /**
   * Quarantine batch
   */
  quarantineBatch(id: number, reason: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/quarantine`, { reason }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Release batch from quarantine
   */
  releaseFromQuarantine(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/release`, {}).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Mark batch as expired
   */
  markAsExpired(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/expire`, {}).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Update storage location
   */
  updateStorageLocation(id: number, newLocation: string): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/storage-location`, { newLocation }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Delete inventory batch
   */
  deleteInventory(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
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

    console.error('CentralInventoryService Error:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
