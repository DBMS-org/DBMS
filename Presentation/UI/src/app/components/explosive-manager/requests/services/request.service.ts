import { Injectable } from '@angular/core';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { map, delay } from 'rxjs/operators';
import { ExplosiveRequest, ExplosiveType, RequestStatus, RequestSearchCriteria } from '../models/request.model';

@Injectable({
  providedIn: 'root'
})
export class RequestService {
  private requestsSubject = new BehaviorSubject<ExplosiveRequest[]>(this.getMockRequests());
  public requests$ = this.requestsSubject.asObservable();

  constructor() { }

  getRequests(criteria?: RequestSearchCriteria): Observable<ExplosiveRequest[]> {
    return this.requests$.pipe(
      map(requests => this.filterRequests(requests, criteria)),
      delay(300) // Simulate API delay
    );
  }

  getRequestById(id: string): Observable<ExplosiveRequest | undefined> {
    return this.requests$.pipe(
      map(requests => requests.find(req => req.id === id))
    );
  }

  updateRequestStatus(id: string, status: RequestStatus, notes?: string): Observable<boolean> {
    const requests = this.requestsSubject.value;
    const requestIndex = requests.findIndex(req => req.id === id);
    
    if (requestIndex !== -1) {
      requests[requestIndex] = {
        ...requests[requestIndex],
        status,
        notes: notes || requests[requestIndex].notes,
        approvalDate: status === RequestStatus.APPROVED ? new Date() : requests[requestIndex].approvalDate
      };
      this.requestsSubject.next([...requests]);
      return of(true).pipe(delay(200));
    }
    
    return of(false);
  }

  private filterRequests(requests: ExplosiveRequest[], criteria?: RequestSearchCriteria): ExplosiveRequest[] {
    if (!criteria) return requests;

    let filtered = [...requests];

    // Apply search term
    if (criteria.searchTerm) {
      const term = criteria.searchTerm.toLowerCase();
      filtered = filtered.filter(req => 
        req.requesterName.toLowerCase().includes(term) ||
        req.purpose.toLowerCase().includes(term) ||
        req.storeLocation.toLowerCase().includes(term)
      );
    }

    // Apply filters
    if (criteria.filters) {
      const { explosiveType, status, requesterName, dateFrom, dateTo, storeLocation } = criteria.filters;
      
      if (explosiveType) {
        filtered = filtered.filter(req => req.explosiveType === explosiveType);
      }
      
      if (status) {
        filtered = filtered.filter(req => req.status === status);
      }
      

      if (requesterName) {
        filtered = filtered.filter(req => req.requesterName.toLowerCase().includes(requesterName.toLowerCase()));
      }
      
      if (storeLocation) {
        filtered = filtered.filter(req => req.storeLocation.toLowerCase().includes(storeLocation.toLowerCase()));
      }
      
      if (dateFrom) {
        filtered = filtered.filter(req => new Date(req.requestDate) >= dateFrom);
      }
      
      if (dateTo) {
        filtered = filtered.filter(req => new Date(req.requestDate) <= dateTo);
      }
    }

    // Apply sorting
    if (criteria.sortBy) {
      filtered.sort((a, b) => {
        const aValue = a[criteria.sortBy!];
        const bValue = b[criteria.sortBy!];
        
        if (criteria.sortOrder === 'desc') {
          return aValue > bValue ? -1 : aValue < bValue ? 1 : 0;
        }
        return aValue < bValue ? -1 : aValue > bValue ? 1 : 0;
      });
    }

    return filtered;
  }

  private getMockRequests(): ExplosiveRequest[] {
    return [
      {
        id: '1',
        requesterId: 'SM001',
        requesterName: 'John Smith',
        requesterRole: 'Store Manager',
        explosiveType: ExplosiveType.ANFO,
        quantity: 0.5,
        unit: 'tons',
        requestDate: new Date('2024-01-15'),
        requiredDate: new Date('2024-01-20'),
        status: RequestStatus.APPROVED,
        approvalDate: new Date('2024-01-16'),
        approvedBy: 'Safety Manager',

        storeLocation: 'Warehouse A',
        purpose: 'Mining operation - Sector 7',
        notes: 'Urgent requirement for scheduled blasting'
      },
      {
        id: '2',
        requesterId: 'SM002',
        requesterName: 'Sarah Johnson',
        requesterRole: 'Store Manager',
        explosiveType: ExplosiveType.EMULSION,
        quantity: 0.3,
        unit: 'tons',
        requestDate: new Date('2024-01-14'),
        requiredDate: new Date('2024-01-25'),
        status: RequestStatus.PENDING,

        storeLocation: 'Warehouse B',
        purpose: 'Quarry expansion project',
        notes: 'Weather dependent operation'
      },
      {
        id: '3',
        requesterId: 'SM001',
        requesterName: 'John Smith',
        requesterRole: 'Store Manager',
        explosiveType: ExplosiveType.ANFO,
        quantity: 0.75,
        unit: 'tons',
        requestDate: new Date('2024-01-12'),
        requiredDate: new Date('2024-01-18'),
        status: RequestStatus.COMPLETED,
        approvalDate: new Date('2024-01-13'),
        approvedBy: 'Operations Manager',

        storeLocation: 'Warehouse A',
        purpose: 'Emergency road construction',
        notes: 'Completed successfully'
      },
      {
        id: '4',
        requesterId: 'SM003',
        requesterName: 'Mike Wilson',
        requesterRole: 'Store Manager',
        explosiveType: ExplosiveType.EMULSION,
        quantity: 0.2,
        unit: 'tons',
        requestDate: new Date('2024-01-10'),
        requiredDate: new Date('2024-01-22'),
        status: RequestStatus.REJECTED,
        rejectionReason: 'Insufficient safety clearance',

        storeLocation: 'Warehouse C',
        purpose: 'Demolition project',
        notes: 'Resubmit with proper safety documentation'
      },
      {
        id: '5',
        requesterId: 'SM002',
        requesterName: 'Sarah Johnson',
        requesterRole: 'Store Manager',
        explosiveType: ExplosiveType.ANFO,
        quantity: 0.4,
        unit: 'tons',
        requestDate: new Date('2024-01-08'),
        requiredDate: new Date('2024-01-30'),
        status: RequestStatus.IN_PROGRESS,
        approvalDate: new Date('2024-01-09'),
        approvedBy: 'Safety Manager',

        storeLocation: 'Warehouse B',
        purpose: 'Tunnel excavation',
        notes: 'Preparation in progress'
      }
    ];
  }
}