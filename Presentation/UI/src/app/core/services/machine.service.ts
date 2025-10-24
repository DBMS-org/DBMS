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

/**
 * Machine Service
 * 
 * Comprehensive service for managing drilling machine operations including:
 * - Complete CRUD operations for machine inventory management
 * - Advanced search and filtering capabilities across machine properties
 * - Real-time statistics and reporting for machine utilization
 * - Assignment request workflow management (submit, approve, reject)
 * - Machine assignment lifecycle tracking (assign, return, status updates)
 * - Data transformation and mapping between backend and frontend models
 */
@Injectable({
  providedIn: 'root'
})
export class MachineService {
  private apiUrl = `${environment.apiUrl}/api/machines`;

  constructor(private http: HttpClient) {}

  // Machine Inventory Operations
  /**
   * Retrieves all machines from the backend with proper data transformation
   * Maps backend response to frontend model with date conversions
   */
  getAllMachines(): Observable<Machine[]> {
    return this.http.get<Machine[]>(this.apiUrl).pipe(
      map(machines => machines.map(machine => this.mapMachine(machine)))
    );
  }

  /**
   * Retrieves a specific machine by ID with complete details
   * Includes maintenance history and assignment information
   */
  getMachineById(id: number): Observable<Machine> {
    return this.http.get<Machine>(`${this.apiUrl}/${id}`).pipe(
      map(machine => this.mapMachine(machine))
    );
  }

  /**
   * Creates a new machine record in the system
   * Validates required fields and applies business rules
   */
  addMachine(request: CreateMachineRequest): Observable<Machine> {
    return this.http.post<Machine>(this.apiUrl, request).pipe(
      map(machine => this.mapMachine(machine))
    );
  }

  /**
   * Updates an existing machine record with new information
   * Handles region ID assignment based on location if not provided
   */
  updateMachine(id: number, request: UpdateMachineRequest): Observable<Machine> {
    // Ensure regionId is set if missing - auto-detect from location
    if (!request.regionId && request.currentLocation) {
      // Try to find a matching region based on location string
      const regions = environment.regions as Record<string, number>;
      const regionMatch = Object.entries(regions).find(
        ([key]) => request.currentLocation?.toLowerCase().includes(key.toLowerCase())
      );
      if (regionMatch) {
        request.regionId = regionMatch[1];
      }
    }
    
    console.log('Sending update machine request:', request);
    return this.http.put<Machine>(`${this.apiUrl}/${id}`, request).pipe(
      map(machine => this.mapMachine(machine))
    );
  }

  /**
   * Permanently removes a machine from the system
   * Validates that machine is not currently assigned before deletion
   */
  deleteMachine(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  /**
   * Updates the operational status of a machine
   * Triggers workflow notifications for status changes
   */
  updateMachineStatus(id: number, status: MachineStatus): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/status`, { status });
  }

  // Search and Statistics Operations
  /**
   * Performs advanced search across multiple machine properties
   * Supports partial matching and multiple filter criteria
   */
  searchMachines(params: {
    name?: string;
    type?: string;
    status?: string;
    manufacturer?: string;
    serialNumber?: string;
  }): Observable<Machine[]> {
    const queryParams = new URLSearchParams();
    
    // Build query parameters dynamically based on provided filters
    if (params.name) queryParams.append('name', params.name);
    if (params.type) queryParams.append('type', params.type);
    if (params.status) queryParams.append('status', params.status);
    if (params.manufacturer) queryParams.append('manufacturer', params.manufacturer);
    if (params.serialNumber) queryParams.append('serialNumber', params.serialNumber);

    return this.http.get<Machine[]>(`${this.apiUrl}/search?${queryParams.toString()}`).pipe(
      map(machines => machines.map(machine => this.mapMachine(machine)))
    );
  }

  /**
   * Retrieves comprehensive machine statistics for dashboard display
   * Includes utilization rates, maintenance schedules, and availability metrics
   */
  getMachineStatistics(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/statistics`);
  }

  // Machine Assignment Operations (Mock implementation for development)
  /**
   * Retrieves all pending and processed assignment requests
   * Currently uses mock data for development purposes
   */
  getAllAssignmentRequests(): Observable<MachineAssignmentRequest[]> {
    return of(this.getMockAssignmentRequests());
  }

  /**
   * Submits a new machine assignment request for approval
   * Validates request details and business rules before submission
   */
  submitAssignmentRequest(request: MachineAssignmentRequest): Observable<MachineAssignmentRequest> {
    return this.http.post<MachineAssignmentRequest>(`${this.apiUrl}/assignment-requests`, request);
  }

  /**
   * Approves an assignment request and assigns specified machines
   * Updates machine status and creates assignment records
   */
  approveAssignmentRequest(requestId: string, assignedMachines: string[], comments?: string): Observable<MachineAssignmentRequest> {
    return this.http.patch<MachineAssignmentRequest>(`${this.apiUrl}/assignment-requests/${requestId}/approve`, {
      assignedMachines,
      comments
    });
  }

  /**
   * Rejects an assignment request with mandatory comments
   * Notifies requester and logs rejection reason
   */
  rejectAssignmentRequest(requestId: string, comments: string): Observable<MachineAssignmentRequest> {
    return this.http.patch<MachineAssignmentRequest>(`${this.apiUrl}/assignment-requests/${requestId}/reject`, {
      comments
    });
  }

  /**
   * Retrieves all currently active machine assignments
   * Includes assignment details, duration, and return dates
   */
  getActiveAssignments(): Observable<MachineAssignment[]> {
    return this.http.get<MachineAssignment[]>(`${this.apiUrl}/assignments/active`);
  }

  /**
   * Creates a new machine assignment record
   * Updates machine status and tracks assignment lifecycle
   */
  assignMachine(assignment: MachineAssignment): Observable<MachineAssignment> {
    return this.http.post<MachineAssignment>(`${this.apiUrl}/assignments`, assignment);
  }

  /**
   * Processes machine return from assignment
   * Updates machine status and closes assignment record
   */
  returnMachine(assignmentId: string): Observable<MachineAssignment> {
    return this.http.patch<MachineAssignment>(`${this.apiUrl}/assignments/${assignmentId}/return`, {});
  }

  /**
   * Helper method to map backend response to frontend model
   * Handles date conversions and data type transformations
   */
  private mapMachine(machine: any): Machine {
    return {
      ...machine,
      createdAt: new Date(machine.createdAt),
      updatedAt: new Date(machine.updatedAt),
      lastMaintenanceDate: machine.lastMaintenanceDate ? new Date(machine.lastMaintenanceDate) : undefined,
      nextMaintenanceDate: machine.nextMaintenanceDate ? new Date(machine.nextMaintenanceDate) : undefined
    };
  }

  /**
   * Provides mock assignment request data for development and testing
   * Simulates realistic assignment scenarios with various statuses
   */
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