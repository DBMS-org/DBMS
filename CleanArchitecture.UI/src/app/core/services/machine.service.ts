import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { 
  Machine, 
  MachineAssignmentRequest, 
  MachineAssignment, 
  MachineType, 
  MachineStatus, 
  AssignmentRequestStatus,
  RequestUrgency 
} from '../models/machine.model';

@Injectable({
  providedIn: 'root'
})
export class MachineService {
  private apiUrl = `${environment.apiUrl}/machines`;

  constructor(private http: HttpClient) {}

  // Machine Inventory Operations
  getAllMachines(): Observable<Machine[]> {
    // For now, return mock data until backend is implemented
    return of(this.getMockMachines());
  }

  getMachineById(id: string): Observable<Machine> {
    return this.http.get<Machine>(`${this.apiUrl}/${id}`);
  }

  addMachine(machine: Machine): Observable<Machine> {
    return this.http.post<Machine>(this.apiUrl, machine);
  }

  updateMachine(id: string, machine: Machine): Observable<Machine> {
    return this.http.put<Machine>(`${this.apiUrl}/${id}`, machine);
  }

  deleteMachine(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  updateMachineStatus(id: string, status: MachineStatus): Observable<Machine> {
    return this.http.patch<Machine>(`${this.apiUrl}/${id}/status`, { status });
  }

  // Machine Assignment Operations
  getAllAssignmentRequests(): Observable<MachineAssignmentRequest[]> {
    return of(this.getMockAssignmentRequests());
  }

  submitAssignmentRequest(request: MachineAssignmentRequest): Observable<MachineAssignmentRequest> {
    return this.http.post<MachineAssignmentRequest>(`${this.apiUrl}/assignment-requests`, request);
  }

  approveAssignmentRequest(requestId: string, assignedMachines: string[], comments?: string): Observable<MachineAssignmentRequest> {
    return this.http.patch<MachineAssignmentRequest>(`${this.apiUrl}/assignment-requests/${requestId}/approve`, {
      assignedMachines,
      comments
    });
  }

  rejectAssignmentRequest(requestId: string, comments: string): Observable<MachineAssignmentRequest> {
    return this.http.patch<MachineAssignmentRequest>(`${this.apiUrl}/assignment-requests/${requestId}/reject`, {
      comments
    });
  }

  getActiveAssignments(): Observable<MachineAssignment[]> {
    return this.http.get<MachineAssignment[]>(`${this.apiUrl}/assignments/active`);
  }

  assignMachine(assignment: MachineAssignment): Observable<MachineAssignment> {
    return this.http.post<MachineAssignment>(`${this.apiUrl}/assignments`, assignment);
  }

  returnMachine(assignmentId: string): Observable<MachineAssignment> {
    return this.http.patch<MachineAssignment>(`${this.apiUrl}/assignments/${assignmentId}/return`, {});
  }

  // Statistics
  getMachineStatistics(): Observable<any> {
    return of({
      totalMachines: 45,
      availableMachines: 32,
      assignedMachines: 8,
      maintenanceMachines: 3,
      outOfServiceMachines: 2,
      pendingRequests: 7,
      activeAssignments: 8
    });
  }

  // Mock data for development
  private getMockMachines(): Machine[] {
    return [
      {
        id: '1',
        name: 'Atlas Copco ROC L8',
        type: MachineType.DRILL_RIG,
        model: 'ROC L8',
        manufacturer: 'Atlas Copco',
        serialNumber: 'AC-2023-001',
        status: MachineStatus.AVAILABLE,
        currentLocation: 'Muscat Depot',
        specifications: {
          power: '350 HP',
          weight: '42,000 kg',
          maxOperatingDepth: '54 m',
          drillingDiameter: '115-152 mm',
          fuelType: 'Diesel'
        },
        createdAt: new Date('2023-01-15'),
        updatedAt: new Date('2024-01-10')
      },
      {
        id: '2',
        name: 'Caterpillar 320D',
        type: MachineType.EXCAVATOR,
        model: '320D',
        manufacturer: 'Caterpillar',
        serialNumber: 'CAT-2023-002',
        status: MachineStatus.ASSIGNED,
        assignedToProject: 'Al Hajar Mining Project',
        currentLocation: 'Site A - Al Hajar',
        specifications: {
          power: '122 HP',
          weight: '20,300 kg',
          capacity: '1.2 m³ bucket',
          fuelType: 'Diesel'
        },
        createdAt: new Date('2023-02-20'),
        updatedAt: new Date('2024-01-08')
      },
      {
        id: '3',
        name: 'Komatsu PC400',
        type: MachineType.EXCAVATOR,
        model: 'PC400',
        manufacturer: 'Komatsu',
        serialNumber: 'KOM-2023-003',
        status: MachineStatus.IN_MAINTENANCE,
        currentLocation: 'Maintenance Bay 2',
        lastMaintenanceDate: new Date('2024-01-05'),
        nextMaintenanceDate: new Date('2024-01-15'),
        specifications: {
          power: '257 HP',
          weight: '40,200 kg',
          capacity: '1.9 m³ bucket',
          fuelType: 'Diesel'
        },
        createdAt: new Date('2023-03-10'),
        updatedAt: new Date('2024-01-05')
      }
    ];
  }

  private getMockAssignmentRequests(): MachineAssignmentRequest[] {
    return [
      {
        id: '1',
        projectId: 'proj-001',
        machineType: MachineType.DRILL_RIG,
        quantity: 2,
        requestedBy: 'John Smith',
        requestedDate: new Date('2024-01-10'),
        status: AssignmentRequestStatus.PENDING,
        urgency: RequestUrgency.HIGH,
        detailsOrExplanation: 'Urgent requirement for Phase 2 drilling operations at Al Hajar site.',
        expectedUsageDuration: '6 months',
        expectedReturnDate: new Date('2024-07-10')
      },
      {
        id: '2',
        projectId: 'proj-002',
        machineType: MachineType.EXCAVATOR,
        quantity: 1,
        requestedBy: 'Sarah Johnson',
        requestedDate: new Date('2024-01-08'),
        status: AssignmentRequestStatus.APPROVED,
        urgency: RequestUrgency.MEDIUM,
        detailsOrExplanation: 'Required for site preparation and material handling.',
        approvedBy: 'Mike Wilson',
        approvedDate: new Date('2024-01-09'),
        assignedMachines: ['2'],
        expectedUsageDuration: '3 months'
      }
    ];
  }
} 