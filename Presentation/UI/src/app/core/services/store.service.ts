import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { 
  Store, 
  StoreStatistics, 
  StoreFilters, 
  CreateStoreRequest, 
  UpdateStoreRequest,
  StoreType,
  ExplosiveType,
  StoreStatus,
  StorageCapacityUnit,
  SecurityLevel
} from '../models/store.model';

@Injectable({
  providedIn: 'root'
})
export class StoreService {
  private readonly apiUrl = `${environment.apiUrl}/stores`;
  private storesSubject = new BehaviorSubject<Store[]>([]);
  public stores$ = this.storesSubject.asObservable();

  constructor(private http: HttpClient) {}

  // Get all stores
  getAllStores(): Observable<Store[]> {
    // For now, return mock data. Replace with actual HTTP call when backend is ready
    return of(this.getMockStores());
    // return this.http.get<Store[]>(this.apiUrl);
  }

  // Get store by ID
  getStoreById(id: string): Observable<Store> {
    const stores = this.getMockStores();
    const store = stores.find(s => s.id === id);
    if (!store) {
      throw new Error(`Store with ID ${id} not found`);
    }
    return of(store);
    // return this.http.get<Store>(`${this.apiUrl}/${id}`);
  }

  // Create new store
  createStore(request: CreateStoreRequest): Observable<Store> {
    const newStore: Store = {
      id: this.generateId(),
      ...request,
      currentOccupancy: 0,
      status: StoreStatus.OPERATIONAL,
      isActive: true,
      lastInspectionDate: new Date(),
      nextInspectionDate: new Date(Date.now() + 90 * 24 * 60 * 60 * 1000), // 90 days from now
      createdAt: new Date(),
      updatedAt: new Date(),
      createdBy: 'current-user-id',
      updatedBy: 'current-user-id'
    };
    
    return of(newStore);
    // return this.http.post<Store>(this.apiUrl, request);
  }

  // Update store
  updateStore(request: UpdateStoreRequest): Observable<Store> {
    const stores = this.getMockStores();
    const existingStore = stores.find(s => s.id === request.id);
    if (!existingStore) {
      throw new Error(`Store with ID ${request.id} not found`);
    }

    const updatedStore: Store = {
      ...existingStore,
      ...request,
      updatedAt: new Date(),
      updatedBy: 'current-user-id'
    };

    return of(updatedStore);
    // return this.http.put<Store>(`${this.apiUrl}/${request.id}`, request);
  }

  // Delete store
  deleteStore(id: string): Observable<void> {
    return of(void 0);
    // return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Deactivate store
  deactivateStore(id: string): Observable<Store> {
    const stores = this.getMockStores();
    const store = stores.find(s => s.id === id);
    if (!store) {
      throw new Error(`Store with ID ${id} not found`);
    }

    const deactivatedStore: Store = {
      ...store,
      isActive: false,
      status: StoreStatus.DECOMMISSIONED,
      updatedAt: new Date(),
      updatedBy: 'current-user-id'
    };

    return of(deactivatedStore);
    // return this.http.patch<Store>(`${this.apiUrl}/${id}/deactivate`, {});
  }

  // Get store statistics
  getStoreStatistics(): Observable<StoreStatistics> {
    const stores = this.getMockStores();
    
    const statistics: StoreStatistics = {
      totalStores: stores.length,
      activeStores: stores.filter(s => s.isActive).length,
      inactiveStores: stores.filter(s => !s.isActive).length,
      operationalStores: stores.filter(s => s.status === StoreStatus.OPERATIONAL).length,
      maintenanceStores: stores.filter(s => s.status === StoreStatus.UNDER_MAINTENANCE).length,
      totalCapacity: stores.reduce((sum, s) => sum + s.storageCapacity, 0),
      totalOccupancy: stores.reduce((sum, s) => sum + (s.currentOccupancy || 0), 0),
      utilizationRate: 0, // Calculate based on total capacity and occupancy
      storesByType: this.groupStoresByType(stores),
      storesByRegion: this.groupStoresByRegion(stores)
    };

    statistics.utilizationRate = statistics.totalCapacity > 0 
      ? (statistics.totalOccupancy / statistics.totalCapacity) * 100 
      : 0;

    return of(statistics);
    // return this.http.get<StoreStatistics>(`${this.apiUrl}/statistics`);
  }

  // Filter stores
  filterStores(filters: StoreFilters): Observable<Store[]> {
    let stores = this.getMockStores();

    if (filters.status && filters.status !== 'ALL') {
      stores = stores.filter(s => s.status === filters.status);
    }

    if (filters.storeType && filters.storeType !== 'ALL') {
      stores = stores.filter(s => s.storeType === filters.storeType);
    }

    if (filters.location && filters.location !== 'ALL') {
      stores = stores.filter(s => 
        s.location.city.toLowerCase().includes(filters.location!.toLowerCase()) ||
        s.location.region.toLowerCase().includes(filters.location!.toLowerCase())
      );
    }

    if (filters.storeManager && filters.storeManager !== 'ALL') {
      stores = stores.filter(s => 
        s.storeManagerName.toLowerCase().includes(filters.storeManager!.toLowerCase())
      );
    }

    if (filters.isActive !== null && filters.isActive !== undefined) {
      stores = stores.filter(s => s.isActive === filters.isActive);
    }

    if (filters.searchTerm) {
      const searchTerm = filters.searchTerm.toLowerCase();
      stores = stores.filter(s => 
        s.storeName.toLowerCase().includes(searchTerm) ||
        s.storeAddress.toLowerCase().includes(searchTerm) ||
        s.storeManagerName.toLowerCase().includes(searchTerm) ||
        s.location.city.toLowerCase().includes(searchTerm) ||
        s.location.region.toLowerCase().includes(searchTerm)
      );
    }

    return of(stores);
  }

  // Helper methods
  private generateId(): string {
    return 'store_' + Math.random().toString(36).substr(2, 9);
  }

  private groupStoresByType(stores: Store[]): { [key: string]: number } {
    return stores.reduce((acc, store) => {
      acc[store.storeType] = (acc[store.storeType] || 0) + 1;
      return acc;
    }, {} as { [key: string]: number });
  }

  private groupStoresByRegion(stores: Store[]): { [key: string]: number } {
    return stores.reduce((acc, store) => {
      acc[store.location.region] = (acc[store.location.region] || 0) + 1;
      return acc;
    }, {} as { [key: string]: number });
  }

  // Mock data for development
  private getMockStores(): Store[] {
    return [
      {
        id: 'store_001',
        storeName: 'Main Explosive Warehouse - Muscat',
        storeAddress: '123 Industrial Area, Muscat, Oman',
        storeManagerName: 'Ahmed Al-Rashid',
        storeManagerContact: '+968-9876-5432',
        storeManagerEmail: 'ahmed.rashid@company.com',
        storeType: StoreType.MAIN_WAREHOUSE,
        explosiveTypesAvailable: [ExplosiveType.ANFO, ExplosiveType.EMULSION, ExplosiveType.DYNAMITE],
        storageCapacity: 500,
        storageCapacityUnit: StorageCapacityUnit.TONS,
        currentOccupancy: 350,
        status: StoreStatus.OPERATIONAL,
        isActive: true,
        location: {
          city: 'Muscat',
          region: 'Muscat Governorate',
          country: 'Oman',
          coordinates: { latitude: 23.6345, longitude: 58.5974 }
        },
        securityLevel: SecurityLevel.LEVEL_4,
        lastInspectionDate: new Date('2024-01-15'),
        nextInspectionDate: new Date('2024-04-15'),
        createdAt: new Date('2023-01-01'),
        updatedAt: new Date('2024-01-15'),
        createdBy: 'admin_001',
        updatedBy: 'admin_001'
      },
      {
        id: 'store_002',
        storeName: 'Field Storage Unit - Sohar',
        storeAddress: '456 Mining District, Sohar, Oman',
        storeManagerName: 'Fatima Al-Zahra',
        storeManagerContact: '+968-9876-5433',
        storeManagerEmail: 'fatima.zahra@company.com',
        storeType: StoreType.FIELD_STORAGE,
        explosiveTypesAvailable: [ExplosiveType.BLASTING_CAPS, ExplosiveType.DETONATING_CORD],
        storageCapacity: 100,
        storageCapacityUnit: StorageCapacityUnit.TONS,
        currentOccupancy: 75,
        status: StoreStatus.OPERATIONAL,
        isActive: true,
        location: {
          city: 'Sohar',
          region: 'North Al Batinah',
          country: 'Oman',
          coordinates: { latitude: 24.3473, longitude: 56.7536 }
        },
        securityLevel: SecurityLevel.LEVEL_3,
        lastInspectionDate: new Date('2024-01-20'),
        nextInspectionDate: new Date('2024-04-20'),
        createdAt: new Date('2023-03-15'),
        updatedAt: new Date('2024-01-20'),
        createdBy: 'admin_001',
        updatedBy: 'manager_002'
      },
      {
        id: 'store_003',
        storeName: 'Distribution Center - Nizwa',
        storeAddress: '789 Commercial Zone, Nizwa, Oman',
        storeManagerName: 'Mohammed Al-Busaidi',
        storeManagerContact: '+968-9876-5434',
        storeManagerEmail: 'mohammed.busaidi@company.com',
        storeType: StoreType.DISTRIBUTION_CENTER,
        explosiveTypesAvailable: [ExplosiveType.PRIMER, ExplosiveType.BOOSTER],
        storageCapacity: 200,
        storageCapacityUnit: StorageCapacityUnit.TONS,
        currentOccupancy: 120,
        status: StoreStatus.UNDER_MAINTENANCE,
        isActive: true,
        location: {
          city: 'Nizwa',
          region: 'Ad Dakhiliyah',
          country: 'Oman',
          coordinates: { latitude: 22.9333, longitude: 57.5333 }
        },
        securityLevel: SecurityLevel.LEVEL_2,
        lastInspectionDate: new Date('2024-01-10'),
        nextInspectionDate: new Date('2024-04-10'),
        createdAt: new Date('2023-06-01'),
        updatedAt: new Date('2024-01-25'),
        createdBy: 'admin_001',
        updatedBy: 'manager_003'
      }
    ];
  }
} 