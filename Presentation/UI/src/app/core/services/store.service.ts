import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { 
  Store, 
  StoreStatistics, 
  StoreFilters, 
  CreateStoreRequest, 
  UpdateStoreRequest,
  ExplosiveType,
  StoreStatus
} from '../models/store.model';

@Injectable({
  providedIn: 'root'
})
export class StoreService {
  private readonly apiUrl = `${environment.apiUrl}/api/stores`;

  constructor(private http: HttpClient) {}

  // Get all stores
  getAllStores(): Observable<Store[]> {
    return this.http.get<Store[]>(this.apiUrl).pipe(
      map(stores => stores.map(store => this.mapStore(store))),
      catchError(this.handleError)
    );
  }

  // Get store by ID
  getStoreById(id: string): Observable<Store> {
    return this.http.get<Store>(`${this.apiUrl}/${id}`).pipe(
      map(store => this.mapStore(store)),
      catchError(this.handleError)
    );
  }

  // Create new store
  createStore(request: CreateStoreRequest): Observable<Store> {
    return this.http.post<Store>(this.apiUrl, request).pipe(
      map(store => this.mapStore(store)),
      catchError(this.handleError)
    );
  }

  // Update store
  updateStore(id: string, request: UpdateStoreRequest): Observable<Store> {
    return this.http.put<Store>(`${this.apiUrl}/${id}`, request).pipe(
      map(store => this.mapStore(store)),
      catchError(this.handleError)
    );
  }

  // Delete store
  deleteStore(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  // Deactivate store
  deactivateStore(id: string): Observable<Store> {
    return this.http.patch<Store>(`${this.apiUrl}/${id}/deactivate`, {}).pipe(
      map(store => this.mapStore(store)),
      catchError(this.handleError)
    );
  }

  // Get store statistics
  getStoreStatistics(): Observable<StoreStatistics> {
    return this.http.get<StoreStatistics>(`${this.apiUrl}/statistics`).pipe(
      catchError(this.handleError)
    );
  }

  // Filter stores
  filterStores(filters: StoreFilters): Observable<Store[]> {
    return this.getAllStores().pipe(
      map(stores => this.applyFilters(stores, filters))
    );
  }

  // Private helper methods
  private mapStore(store: any): Store {
    return {
      ...store,
      createdAt: store.createdAt ? new Date(store.createdAt) : new Date(),
      updatedAt: store.updatedAt ? new Date(store.updatedAt) : new Date()
    };
  }

  private applyFilters(stores: Store[], filters: StoreFilters): Store[] {
    return stores.filter(store => {
      // Status filter
      if (filters.status && filters.status !== 'ALL' && store.status !== filters.status) {
        return false;
      }

      // Location filter
      if (filters.location && filters.location !== 'ALL' && store.location.city !== filters.location) {
        return false;
      }

      // Store manager filter
      if (filters.storeManager && filters.storeManager !== 'ALL' && store.storeManagerName !== filters.storeManager) {
        return false;
      }

      // Active status filter
      if (filters.isActive !== null && store.isActive !== filters.isActive) {
        return false;
      }

      // Search term filter
      if (filters.searchTerm && filters.searchTerm.trim()) {
        const searchTerm = filters.searchTerm.toLowerCase();
        const searchableText = [
          store.storeName,
          store.storeManagerName,
          store.location.city,
          store.storeAddress,
          store.status
        ].join(' ').toLowerCase();
        
        if (!searchableText.includes(searchTerm)) {
          return false;
        }
      }

      return true;
    });
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    
    console.error('StoreService Error:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}