import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { 
  StockRequest, 
  CreateStockRequestRequest, 
  StockRequestStatistics, 
  StockRequestFilters,
  StockRequestStatus,
  StockRequestItem
} from '../models/stock-request.model';
import { ExplosiveType } from '../models/store.model';
import { MockDataService } from './mock-data/mock-data.service';

@Injectable({
  providedIn: 'root'
})
export class StockRequestService {
  private apiUrl = `${environment.apiUrl}/stock-requests`;

  constructor(
    private http: HttpClient,
    private mockDataService: MockDataService
  ) {}

  // Get all stock requests for current user's store
  getStockRequests(): Observable<StockRequest[]> {
    // Use centralized mock data service
    return this.mockDataService.getStockRequestMockData();
    // return this.http.get<StockRequest[]>(this.apiUrl);
  }

  // Create new stock request
  createStockRequest(request: CreateStockRequestRequest): Observable<StockRequest> {
    // Mock response for development
    const mockResponse: StockRequest = {
      id: Date.now().toString(),
      requesterId: 'current-user',
      requesterName: 'Current User',
      requesterStoreId: request.requesterStoreId,
      requesterStoreName: 'Current Store',
      requestedItems: request.requestedItems,
      requestDate: new Date(),
      requiredDate: request.requiredDate,
      status: StockRequestStatus.PENDING,
      dispatched: false,

      justification: request.justification,
      notes: request.notes,
      createdAt: new Date(),
      updatedAt: new Date()
    };

    return of(mockResponse);
    // return this.http.post<StockRequest>(this.apiUrl, request);
  }

  // Get stock request statistics
  getStockRequestStatistics(): Observable<StockRequestStatistics> {
    // Mock statistics for development
    const mockStats: StockRequestStatistics = {
      totalRequests: 15,
      pendingRequests: 3,
      approvedRequests: 8,
      rejectedRequests: 2,
      fulfilledRequests: 2,
      averageProcessingTime: 2.5,

      requestsByStatus: {
        'Pending': 3,
        'Under Review': 1,
        'Approved': 8,
        'Rejected': 2,
        'Fulfilled': 1
      }
    };

    return of(mockStats);
    // return this.http.get<StockRequestStatistics>(`${this.apiUrl}/statistics`);
  }

  // Filter stock requests
  filterStockRequests(filters: StockRequestFilters): Observable<StockRequest[]> {
    return this.getStockRequests(); // For now, return all requests
    // return this.http.post<StockRequest[]>(`${this.apiUrl}/filter`, filters);
  }

  // Cancel stock request
  cancelStockRequest(requestId: string): Observable<void> {
    return of(void 0);
    // return this.http.patch<void>(`${this.apiUrl}/${requestId}/cancel`, {});
  }

  // Get available explosive types from explosive manager's inventory
  getAvailableExplosiveTypes(): Observable<ExplosiveType[]> {
    return of(Object.values(ExplosiveType).filter(value => typeof value === 'number') as ExplosiveType[]);
  }

  // Get common units for different explosive types
  getUnitsForExplosiveType(explosiveType: ExplosiveType): string[] {
    const unitMap: { [key in ExplosiveType]: string[] } = {
      [ExplosiveType.ANFO]: ['tons', 'kg'],
      [ExplosiveType.Emulsion]: ['tons', 'kg'],
      [ExplosiveType.Dynamite]: ['tons', 'boxes'],
      [ExplosiveType.BlastingCaps]: ['pieces', 'boxes'],
      [ExplosiveType.DetonatingCord]: ['meters', 'rolls'],
      [ExplosiveType.Primer]: ['pieces', 'boxes'],
      [ExplosiveType.Booster]: ['pieces', 'tons'],
      [ExplosiveType.ShapedCharges]: ['pieces', 'sets']
    };

    return unitMap[explosiveType] || ['tons', 'pieces'];
  }
}