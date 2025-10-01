import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ExplosiveType } from '../../models/store.model';
import { StockRequest, StockRequestStatus } from '../../models/stock-request.model';
import { ExplosiveRequest, RequestStatus } from '../../../components/explosive-manager/requests/models/explosive-request.model';
import { ExplosiveType as ExplosiveManagerExplosiveType } from '../../../components/explosive-manager/requests/models/explosive-request.model';

/**
 * Centralized Mock Data Service
 * Provides consistent mock data across all components
 */
@Injectable({
  providedIn: 'root'
})
export class MockDataService {
  
  // Centralized user data
  private readonly mockUsers = {
    storeManagers: [
      { id: 'SM001', name: 'Ahmed Al-Rashid', role: 'Store Manager', storeId: 'store1', storeName: 'Muscat Field Storage' },
      { id: 'SM002', name: 'Fatima Al-Zahra', role: 'Store Manager', storeId: 'store2', storeName: 'Sohar Industrial Storage' },
      { id: 'SM003', name: 'John Smith', role: 'Store Manager', storeId: 'store3', storeName: 'Nizwa Distribution Center' },
      { id: 'SM004', name: 'Sarah Johnson', role: 'Store Manager', storeId: 'store4', storeName: 'Salalah Regional Storage' },
      { id: 'SM005', name: 'Mike Wilson', role: 'Store Manager', storeId: 'store5', storeName: 'Buraimi Field Storage' }
    ],
    explosiveManagers: [
      { id: 'EM001', name: 'Omar Al-Balushi', role: 'Explosive Manager' },
      { id: 'EM002', name: 'Khalid Al-Hinai', role: 'Explosive Manager' }
    ]
  };

  // Centralized request items data
  private readonly mockRequestItems = [
    // ANFO items
    { explosiveType: ExplosiveType.ANFO, requestedQuantity: 0.5, unit: 'tons', purpose: 'Mining operations - Phase 2', specifications: 'Standard grade ANFO for surface mining' },
    { explosiveType: ExplosiveType.ANFO, requestedQuantity: 0.3, unit: 'tons', purpose: 'Primary blasting', specifications: 'Bulk ANFO' },
    { explosiveType: ExplosiveType.ANFO, requestedQuantity: 0.75, unit: 'tons', purpose: 'Emergency road construction', specifications: 'High-grade ANFO' },
    { explosiveType: ExplosiveType.ANFO, requestedQuantity: 20, unit: 'kg', purpose: 'Stemming tests', specifications: 'Test grade ANFO' },
    
    // Emulsion items
    { explosiveType: ExplosiveType.Emulsion, requestedQuantity: 0.3, unit: 'tons', purpose: 'Underground blasting operations', specifications: 'Water-resistant emulsion for wet conditions' },
    { explosiveType: ExplosiveType.Emulsion, requestedQuantity: 60, unit: 'kg', purpose: 'Initiation charges', specifications: 'Cartridges 32mm' },
    { explosiveType: ExplosiveType.Emulsion, requestedQuantity: 0.25, unit: 'tons', purpose: 'Bulk loading', specifications: 'Pumpable emulsion' },
    { explosiveType: ExplosiveType.Emulsion, requestedQuantity: 0.2, unit: 'tons', purpose: 'Structure demolition', specifications: 'Cartridges 40mm' },
    
    // Other explosive types
    { explosiveType: ExplosiveType.DetonatingCord, requestedQuantity: 250, unit: 'meters', purpose: 'Surface blast connections', specifications: '10 g/m detonating cord' },
    { explosiveType: ExplosiveType.BlastingCaps, requestedQuantity: 60, unit: 'pieces', purpose: 'Detonation sequence setup', specifications: 'Electric blasting caps, delay 0-9' },
    { explosiveType: ExplosiveType.BlastingCaps, requestedQuantity: 100, unit: 'pieces', purpose: 'Detonation sequence', specifications: 'Electric blasting caps, delay 0-9' },
    { explosiveType: ExplosiveType.Primer, requestedQuantity: 40, unit: 'pieces', purpose: 'Primer cartridges for emulsion shots', specifications: 'Suitable for 32-40mm boreholes' },
    { explosiveType: ExplosiveType.Booster, requestedQuantity: 20, unit: 'pieces', purpose: 'Boosters for large diameter holes', specifications: '400g boosters for 76-89mm holes' }
  ];

  constructor() {}

  /**
   * Get unified mock data for stock requests (Store Manager perspective)
   */
  getStockRequestMockData(): Observable<StockRequest[]> {
    const mockData: StockRequest[] = [
      {
        id: '1',
        requesterId: 'SM001',
        requesterName: 'Ahmed Al-Rashid',
        requesterStoreId: 'store1',
        requesterStoreName: 'Muscat Field Storage',
        explosiveManagerId: 'EM001',
        explosiveManagerName: 'Omar Al-Balushi',
        requestedItems: [
          this.mockRequestItems[0], // ANFO 0.5 tons
          this.mockRequestItems[9], // Detonating cord
          this.mockRequestItems[10] // Blasting caps 60 pieces
        ],
        requestDate: new Date('2024-01-15'),
        requiredDate: new Date('2024-01-25'),
        status: StockRequestStatus.APPROVED,
        dispatched: true,
        dispatchedDate: new Date('2024-01-17'),
        fulfillmentDate: new Date('2024-01-18'),
        justification: 'Urgent requirement for upcoming mining phase',
        notes: 'Please ensure delivery before 25th Jan',
        approvalDate: new Date('2024-01-16'),
        createdAt: new Date('2024-01-15'),
        updatedAt: new Date('2024-01-16')
      },
      {
        id: '2',
        requesterId: 'SM002',
        requesterName: 'Fatima Al-Zahra',
        requesterStoreId: 'store2',
        requesterStoreName: 'Sohar Industrial Storage',
        requestedItems: [
          this.mockRequestItems[4], // Emulsion 0.3 tons
          this.mockRequestItems[12] // Primer 40 pieces
        ],
        requestDate: new Date('2024-01-20'),
        requiredDate: new Date('2024-02-05'),
        status: StockRequestStatus.PENDING,
        dispatched: false,
        justification: 'Routine stock replenishment',
        notes: 'Standard delivery schedule',
        createdAt: new Date('2024-01-20'),
        updatedAt: new Date('2024-01-20')
      },
      {
        id: '3',
        requesterId: 'SM001',
        requesterName: 'Ahmed Al-Rashid',
        requesterStoreId: 'store1',
        requesterStoreName: 'Muscat Field Storage',
        requestedItems: [
          this.mockRequestItems[10], // Blasting caps 100 pieces
          this.mockRequestItems[12]  // Booster 20 pieces
        ],
        requestDate: new Date('2024-01-18'),
        requiredDate: new Date('2024-01-30'),
        status: StockRequestStatus.FULFILLED,
        dispatched: true,
        dispatchedDate: new Date('2024-01-19'),
        fulfillmentDate: new Date('2024-01-19'),
        justification: 'Critical component for scheduled blast',
        notes: 'Handle with extreme care',
        createdAt: new Date('2024-01-18'),
        updatedAt: new Date('2024-01-19')
      },
      {
        id: '4',
        requesterId: 'SM003',
        requesterName: 'John Smith',
        requesterStoreId: 'store3',
        requesterStoreName: 'Nizwa Distribution Center',
        requestedItems: [
          { explosiveType: this.mockRequestItems[2].explosiveType, requestedQuantity: this.mockRequestItems[2].requestedQuantity, unit: this.mockRequestItems[2].unit, purpose: this.mockRequestItems[2].purpose, specifications: this.mockRequestItems[2].specifications }
        ],
        requestDate: new Date('2024-01-12'),
        requiredDate: new Date('2024-01-22'),
        status: StockRequestStatus.IN_PROGRESS,
        dispatched: true,
        dispatchedDate: new Date('2024-01-14'),
        justification: 'Emergency road construction project',
        notes: 'High priority - infrastructure development',
        approvalDate: new Date('2024-01-13'),
        createdAt: new Date('2024-01-12'),
        updatedAt: new Date('2024-01-14')
      },
      {
        id: '5',
        requesterId: 'SM004',
        requesterName: 'Sarah Johnson',
        requesterStoreId: 'store4',
        requesterStoreName: 'Salalah Regional Storage',
        requestedItems: [
          { explosiveType: this.mockRequestItems[6].explosiveType, requestedQuantity: this.mockRequestItems[6].requestedQuantity, unit: this.mockRequestItems[6].unit, purpose: this.mockRequestItems[6].purpose, specifications: this.mockRequestItems[6].specifications },
          { explosiveType: this.mockRequestItems[3].explosiveType, requestedQuantity: this.mockRequestItems[3].requestedQuantity, unit: this.mockRequestItems[3].unit, purpose: this.mockRequestItems[3].purpose, specifications: this.mockRequestItems[3].specifications }
        ],
        requestDate: new Date('2024-01-10'),
        requiredDate: new Date('2024-01-28'),
        status: StockRequestStatus.REJECTED,
        dispatched: false,
        justification: 'Quarry expansion project - Phase 1',
        notes: 'Insufficient safety documentation provided',
        createdAt: new Date('2024-01-10'),
        updatedAt: new Date('2024-01-11')
      }
    ];

    return of(mockData);
  }

  /**
   * Convert store model ExplosiveType to explosive manager ExplosiveType
   */
  private mapExplosiveType(storeType: ExplosiveType): ExplosiveManagerExplosiveType {
    switch (storeType) {
      case ExplosiveType.ANFO:
        return ExplosiveManagerExplosiveType.ANFO;
      case ExplosiveType.Emulsion:
        return ExplosiveManagerExplosiveType.EMULSION;
      default:
        return ExplosiveManagerExplosiveType.ANFO; // Default fallback
    }
  }

  /**
   * Get unified mock data for explosive requests (Explosive Manager perspective)
   */
  getExplosiveRequestMockData(): Observable<ExplosiveRequest[]> {
    const mockData: ExplosiveRequest[] = [
      {
        id: '1',
        requesterId: 'SM001',
        requesterName: 'Ahmed Al-Rashid',
        requesterRole: 'Store Manager',
        explosiveType: this.mapExplosiveType(ExplosiveType.ANFO),
        quantity: 0.5,
        unit: 'tons',
        requestDate: new Date('2024-01-15'),
        requiredDate: new Date('2024-01-25'),
        status: RequestStatus.APPROVED,
        approvalDate: new Date('2024-01-16'),
        approvedBy: 'Omar Al-Balushi',
        storeLocation: 'Muscat Field Storage',
        purpose: 'Mining operations - Phase 2',
        notes: 'Urgent requirement for scheduled blasting',
        requestedItems: [
          { explosiveType: this.mapExplosiveType(this.mockRequestItems[0].explosiveType), quantity: this.mockRequestItems[0].requestedQuantity, unit: this.mockRequestItems[0].unit, purpose: this.mockRequestItems[0].purpose, specifications: this.mockRequestItems[0].specifications },
          { explosiveType: this.mapExplosiveType(this.mockRequestItems[9].explosiveType), quantity: this.mockRequestItems[9].requestedQuantity, unit: this.mockRequestItems[9].unit, purpose: this.mockRequestItems[9].purpose, specifications: this.mockRequestItems[9].specifications },
          { explosiveType: this.mapExplosiveType(this.mockRequestItems[10].explosiveType), quantity: this.mockRequestItems[10].requestedQuantity, unit: this.mockRequestItems[10].unit, purpose: this.mockRequestItems[10].purpose, specifications: this.mockRequestItems[10].specifications }
        ]
      },
      {
        id: '2',
        requesterId: 'SM002',
        requesterName: 'Fatima Al-Zahra',
        requesterRole: 'Store Manager',
        explosiveType: this.mapExplosiveType(ExplosiveType.Emulsion),
        quantity: 0.3,
        unit: 'tons',
        requestDate: new Date('2024-01-20'),
        requiredDate: new Date('2024-02-05'),
        status: RequestStatus.PENDING,
        storeLocation: 'Sohar Industrial Storage',
        purpose: 'Underground blasting operations',
        notes: 'Weather dependent operation',
        requestedItems: [
          { explosiveType: this.mapExplosiveType(this.mockRequestItems[4].explosiveType), quantity: this.mockRequestItems[4].requestedQuantity, unit: this.mockRequestItems[4].unit, purpose: this.mockRequestItems[4].purpose, specifications: this.mockRequestItems[4].specifications },
          { explosiveType: this.mapExplosiveType(this.mockRequestItems[12].explosiveType), quantity: this.mockRequestItems[12].requestedQuantity, unit: this.mockRequestItems[12].unit, purpose: this.mockRequestItems[12].purpose, specifications: this.mockRequestItems[12].specifications }
        ]
      },
      {
        id: '3',
        requesterId: 'SM001',
        requesterName: 'Ahmed Al-Rashid',
        requesterRole: 'Store Manager',
        explosiveType: this.mapExplosiveType(ExplosiveType.ANFO),
        quantity: 0.75,
        unit: 'tons',
        requestDate: new Date('2024-01-18'),
        requiredDate: new Date('2024-01-30'),
        status: RequestStatus.COMPLETED,
        approvalDate: new Date('2024-01-19'),
        approvedBy: 'Omar Al-Balushi',
        storeLocation: 'Muscat Field Storage',
        purpose: 'Critical component for scheduled blast',
        notes: 'Completed successfully',
        requestedItems: [
          { explosiveType: this.mapExplosiveType(this.mockRequestItems[10].explosiveType), quantity: this.mockRequestItems[10].requestedQuantity, unit: this.mockRequestItems[10].unit, purpose: this.mockRequestItems[10].purpose, specifications: this.mockRequestItems[10].specifications },
          { explosiveType: this.mapExplosiveType(this.mockRequestItems[12].explosiveType), quantity: this.mockRequestItems[12].requestedQuantity, unit: this.mockRequestItems[12].unit, purpose: this.mockRequestItems[12].purpose, specifications: this.mockRequestItems[12].specifications }
        ]
      },
      {
        id: '4',
        requesterId: 'SM003',
        requesterName: 'John Smith',
        requesterRole: 'Store Manager',
        explosiveType: this.mapExplosiveType(ExplosiveType.ANFO),
        quantity: 0.75,
        unit: 'tons',
        requestDate: new Date('2024-01-12'),
        requiredDate: new Date('2024-01-22'),
        status: RequestStatus.DISPATCHED,
        approvalDate: new Date('2024-01-13'),
        approvedBy: 'Khalid Al-Hinai',
        dispatchDate: new Date('2024-01-14'),
        truckNumber: 'TRK-001',
        driverName: 'Ali Hassan',
        storeLocation: 'Nizwa Distribution Center',
        purpose: 'Emergency road construction',
        notes: 'High priority dispatch completed',
        requestedItems: [
          { explosiveType: this.mapExplosiveType(this.mockRequestItems[2].explosiveType), quantity: this.mockRequestItems[2].requestedQuantity, unit: this.mockRequestItems[2].unit, purpose: this.mockRequestItems[2].purpose, specifications: this.mockRequestItems[2].specifications }
        ]
      },
      {
        id: '5',
        requesterId: 'SM004',
        requesterName: 'Sarah Johnson',
        requesterRole: 'Store Manager',
        explosiveType: this.mapExplosiveType(ExplosiveType.Emulsion),
        quantity: 0.25,
        unit: 'tons',
        requestDate: new Date('2024-01-10'),
        requiredDate: new Date('2024-01-28'),
        status: RequestStatus.REJECTED,
        rejectionReason: 'Insufficient safety clearance documentation',
        storeLocation: 'Salalah Regional Storage',
        purpose: 'Quarry expansion project',
        notes: 'Resubmit with proper safety documentation',
        requestedItems: [
          { explosiveType: this.mapExplosiveType(this.mockRequestItems[6].explosiveType), quantity: this.mockRequestItems[6].requestedQuantity, unit: this.mockRequestItems[6].unit, purpose: this.mockRequestItems[6].purpose, specifications: this.mockRequestItems[6].specifications },
          { explosiveType: this.mapExplosiveType(this.mockRequestItems[3].explosiveType), quantity: this.mockRequestItems[3].requestedQuantity, unit: this.mockRequestItems[3].unit, purpose: this.mockRequestItems[3].purpose, specifications: this.mockRequestItems[3].specifications }
        ]
      }
    ];

    return of(mockData);
  }

  /**
   * Get mock user data
   */
  getMockUsers() {
    return this.mockUsers;
  }



  /**
   * Get mock request items
   */
  getMockRequestItems() {
    return this.mockRequestItems;
  }
}