import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { 
  Machine, 
  CreateMachineRequest,
  UpdateMachineRequest,
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
  private apiUrl = `${environment.apiUrl}/api/machines`;

  constructor(private http: HttpClient) {}

  // Machine Inventory Operations
  getAllMachines(): Observable<Machine[]> {
    return this.http.get<Machine[]>(this.apiUrl).pipe(
      map(machines => machines.map(machine => this.mapMachine(machine)))
    );
  }

  getMachineById(id: number): Observable<Machine> {
    return this.http.get<Machine>(`${this.apiUrl}/${id}`).pipe(
      map(machine => this.mapMachine(machine))
    );
  }

  addMachine(request: CreateMachineRequest): Observable<Machine> {
    return this.http.post<Machine>(this.apiUrl, request).pipe(
      map(machine => this.mapMachine(machine))
    );
  }

  updateMachine(id: number, request: UpdateMachineRequest): Observable<Machine> {
    return this.http.put<Machine>(`${this.apiUrl}/${id}`, request).pipe(
      map(machine => this.mapMachine(machine))
    );
  }

  deleteMachine(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  updateMachineStatus(id: number, status: MachineStatus): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/status`, { status });
  }

  // Search and Statistics
  searchMachines(params: {
    name?: string;
    type?: string;
    status?: string;
    manufacturer?: string;
    serialNumber?: string;
  }): Observable<Machine[]> {
    const queryParams = new URLSearchParams();
    
    if (params.name) queryParams.append('name', params.name);
    if (params.type) queryParams.append('type', params.type);
    if (params.status) queryParams.append('status', params.status);
    if (params.manufacturer) queryParams.append('manufacturer', params.manufacturer);
    if (params.serialNumber) queryParams.append('serialNumber', params.serialNumber);

    return this.http.get<Machine[]>(`${this.apiUrl}/search?${queryParams.toString()}`).pipe(
      map(machines => machines.map(machine => this.mapMachine(machine)))
    );
  }

  getMachineStatistics(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/statistics`);
  }

  // Machine Assignment Operations (Mock for now)
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

  // Helper method to map backend response to frontend model
  private mapMachine(machine: any): Machine {
    return {
      ...machine,
      createdAt: new Date(machine.createdAt),
      updatedAt: new Date(machine.updatedAt),
      lastMaintenanceDate: machine.lastMaintenanceDate ? new Date(machine.lastMaintenanceDate) : undefined,
      nextMaintenanceDate: machine.nextMaintenanceDate ? new Date(machine.nextMaintenanceDate) : undefined
    };
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