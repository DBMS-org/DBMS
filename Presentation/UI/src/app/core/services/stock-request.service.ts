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

@Injectable({
  providedIn: 'root'
})
export class StockRequestService {
  private apiUrl = `${environment.apiUrl}/stock-requests`;

  constructor(private http: HttpClient) {}

  // Get all stock requests for current user's store
  getStockRequests(): Observable<StockRequest[]> {
    // Mock data for development
    const mockRequests: StockRequest[] = [
      {
        id: '1',
        requesterId: 'user1',
        requesterName: 'Ahmed Al-Rashid',
        requesterStoreId: 'store1',
        requesterStoreName: 'Muscat Field Storage',
        explosiveManagerId: 'mgr1',
        explosiveManagerName: 'Omar Al-Balushi',
        requestedItems: [
          {
            explosiveType: ExplosiveType.ANFO,
            requestedQuantity: 0.5,
            unit: 'tons',
            purpose: 'Mining operations - Phase 2',
            specifications: 'Standard grade ANFO for surface mining'
          },
          {
            explosiveType: ExplosiveType.BLASTING_CAPS,
            requestedQuantity: 100,
            unit: 'pieces',
            purpose: 'Detonation sequence',
            specifications: 'Electric blasting caps, delay 0-9'
          }
        ],
        requestDate: new Date('2024-01-15'),
        requiredDate: new Date('2024-01-25'),
        status: StockRequestStatus.APPROVED,

        justification: 'Urgent requirement for upcoming mining phase. Current stock levels are critically low.',
        notes: 'Please ensure delivery before 25th Jan as operations are scheduled to begin.',
        approvalDate: new Date('2024-01-16'),
        createdAt: new Date('2024-01-15'),
        updatedAt: new Date('2024-01-16')
      },
      {
        id: '2',
        requesterId: 'user1',
        requesterName: 'Ahmed Al-Rashid',
        requesterStoreId: 'store1',
        requesterStoreName: 'Muscat Field Storage',
        requestedItems: [
          {
            explosiveType: ExplosiveType.EMULSION,
            requestedQuantity: 0.2,
            unit: 'tons',
            purpose: 'Underground blasting',
            specifications: 'Water-resistant emulsion for wet conditions'
          }
        ],
        requestDate: new Date('2024-01-20'),
        requiredDate: new Date('2024-02-05'),
        status: StockRequestStatus.PENDING,

        justification: 'Routine stock replenishment for underground operations.',
        createdAt: new Date('2024-01-20'),
        updatedAt: new Date('2024-01-20')
      }
    ];

    return of(mockRequests);
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
    return of(Object.values(ExplosiveType));
  }

  // Get common units for different explosive types
  getUnitsForExplosiveType(explosiveType: ExplosiveType): string[] {
    const unitMap: { [key in ExplosiveType]: string[] } = {
      [ExplosiveType.ANFO]: ['tons', 'kg'],
      [ExplosiveType.EMULSION]: ['tons', 'kg'],
      [ExplosiveType.DYNAMITE]: ['tons', 'boxes'],
      [ExplosiveType.BLASTING_CAPS]: ['pieces', 'boxes'],
      [ExplosiveType.DETONATING_CORD]: ['meters', 'rolls'],
      [ExplosiveType.PRIMER]: ['pieces', 'boxes'],
      [ExplosiveType.BOOSTER]: ['pieces', 'tons'],
      [ExplosiveType.SHAPED_CHARGES]: ['pieces', 'sets']
    };

    return unitMap[explosiveType] || ['tons', 'pieces'];
  }
}