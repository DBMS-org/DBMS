import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MachineService } from '../../../core/services/machine.service';
import { AuthService } from '../../../core/services/auth.service';

import { MachineDetailsComponent } from '../machine-details/machine-details.component';
import { AddMachineComponent } from './add-machine/add-machine.component';
import { EditMachineComponent } from './edit-machine/edit-machine.component';
import { 
  Machine, 
  MachineType, 
  MachineStatus,
  MachineAssignmentRequest,
  AssignmentRequestStatus 
} from '../../../core/models/machine.model';

/**
 * Machine Inventory Component
 * 
 * Comprehensive management interface for drilling machine inventory including:
 * - Machine listing with advanced filtering and search capabilities
 * - Real-time statistics dashboard for machine status overview
 * - CRUD operations for machine records (add, edit, delete, view details)
 * - Assignment request monitoring and management
 * - Project-based machine allocation tracking
 */
@Component({
  selector: 'app-machine-inventory',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    ReactiveFormsModule,
    MachineDetailsComponent,
    AddMachineComponent,
    EditMachineComponent
  ],
  templateUrl: './machine-inventory.component.html',
  styleUrl: './machine-inventory.component.scss'
})
export class MachineInventoryComponent implements OnInit, OnDestroy {
  // Core data collections
  machines: Machine[] = [];
  filteredMachines: Machine[] = [];
  assignmentRequests: MachineAssignmentRequest[] = [];
  isLoading = false;
  error: string | null = null;
  
  // Advanced filtering and search properties
  searchTerm = '';
  selectedStatus: MachineStatus | 'ALL' = 'ALL';
  selectedType: MachineType | 'ALL' = 'ALL';
  selectedProject: string | 'ALL' | 'UNASSIGNED' = 'ALL';
  
  // Dynamic filter options extracted from current machine data
  projectOptions: string[] = [];
  
  // Modal dialog states for various operations
  showDeleteConfirmModal = false;
  showMachineDetailsModal = false;
  showAddMachineModal = false;
  showEditMachineModal = false;
  selectedMachine: Machine | null = null;
  machineToDelete: Machine | null = null;
  
  // Enum references for template usage
  MachineStatus = MachineStatus;
  MachineType = MachineType;
  AssignmentRequestStatus = AssignmentRequestStatus;
  
  // Real-time dashboard statistics
  statistics = {
    total: 0,
    available: 0,
    assigned: 0,
    maintenance: 0,
    outOfService: 0,
    pendingRequests: 0
  };
  
  // Subscription management for memory leak prevention
  private subscriptions: Subscription[] = [];

  constructor(
    private machineService: MachineService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadMachines();
    this.loadAssignmentRequests();
  }

  ngOnDestroy(): void {
    // Clean up all subscriptions to prevent memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  /**
   * Loads all machines from the backend and initializes filtering options
   * Updates project options and applies current filters after loading
   */
  private loadMachines(): void {
    this.isLoading = true;
    const sub = this.machineService.getAllMachines().subscribe({
      next: (machines) => {
        this.machines = machines;
        this.extractProjectOptions();
        this.applyFilters();
        this.calculateStatistics();
        this.isLoading = false;
      },
      error: (error) => {
        this.error = 'Failed to load machines';
        this.isLoading = false;
        console.error('Error loading machines:', error);
      }
    });
    this.subscriptions.push(sub);
  }

  /**
   * Loads pending assignment requests for dashboard statistics
   * Filters to show only pending requests that require attention
   */
  private loadAssignmentRequests(): void {
    const sub = this.machineService.getAllAssignmentRequests().subscribe({
      next: (requests) => {
        this.assignmentRequests = requests.filter(req => 
          req.status === AssignmentRequestStatus.PENDING
        );
        this.calculateStatistics();
      },
      error: (error) => {
        console.error('Error loading assignment requests:', error);
      }
    });
    this.subscriptions.push(sub);
  }

  /**
   * Calculates and updates dashboard statistics
   * Attempts to fetch from backend, falls back to local calculation if needed
   */
  private calculateStatistics(): void {
    const sub = this.machineService.getMachineStatistics().subscribe({
      next: (stats) => {
        this.statistics = {
          total: stats.totalMachines || 0,
          available: stats.availableMachines || 0,
          assigned: stats.assignedMachines || 0,
          maintenance: stats.maintenanceMachines || 0,
          outOfService: stats.outOfServiceMachines || 0,
          pendingRequests: this.assignmentRequests.length
        };
      },
      error: (error) => {
        console.error('Error loading statistics:', error);
        // Fallback to local calculation when backend statistics are unavailable
        this.statistics = {
          total: this.machines.length,
          available: this.machines.filter(m => m.status === MachineStatus.AVAILABLE).length,
          assigned: this.machines.filter(m => m.status === MachineStatus.ASSIGNED).length,
          maintenance: this.machines.filter(m => m.status === MachineStatus.IN_MAINTENANCE).length,
          outOfService: this.machines.filter(m => 
            m.status === MachineStatus.OUT_OF_SERVICE || m.status === MachineStatus.UNDER_REPAIR
          ).length,
          pendingRequests: this.assignmentRequests.length
        };
      }
    });
    this.subscriptions.push(sub);
  }

  /**
   * Applies all active filters to the machine list
   * Supports multi-field search and multiple filter criteria simultaneously
   */
  applyFilters(): void {
    this.filteredMachines = this.machines.filter(machine => {
      // Multi-field text search across machine properties
      const matchesSearch = !this.searchTerm || 
        machine.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        machine.manufacturer.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        machine.model.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        machine.serialNumber.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        (machine.rigNo && machine.rigNo.toLowerCase().includes(this.searchTerm.toLowerCase())) ||
        (machine.plateNo && machine.plateNo.toLowerCase().includes(this.searchTerm.toLowerCase()));
      
      const matchesStatus = this.selectedStatus === 'ALL' || machine.status === this.selectedStatus;
      const matchesType = this.selectedType === 'ALL' || machine.type === this.selectedType;
      
      // Project filter with special handling for unassigned machines
      const matchesProject = this.selectedProject === 'ALL' || 
        (this.selectedProject === 'UNASSIGNED' && (!machine.assignedToProject && !machine.projectName)) ||
        (machine.assignedToProject && machine.assignedToProject === this.selectedProject) ||
        (machine.projectName && machine.projectName === this.selectedProject);
      
      return matchesSearch && matchesStatus && matchesType && matchesProject;
    });
  }

  // Filter event handlers that trigger re-filtering
  onSearchChange(): void {
    this.applyFilters();
  }

  onStatusFilterChange(): void {
    this.applyFilters();
  }

  onTypeFilterChange(): void {
    this.applyFilters();
  }

  onProjectFilterChange(): void {
    this.applyFilters();
  }

  /**
   * Extracts unique project names from machine assignments
   * Creates dynamic filter options based on current machine data
   */
  private extractProjectOptions(): void {
    const projects = new Set<string>();
    this.machines.forEach(machine => {
      if (machine.assignedToProject) {
        projects.add(machine.assignedToProject);
      }
      if (machine.projectName) {
        projects.add(machine.projectName);
      }
    });
    this.projectOptions = Array.from(projects).sort();
  }

  // Modal dialog management methods
  openAddMachineModal(): void {
    this.selectedMachine = null;
    this.showAddMachineModal = true;
  }

  openEditMachineModal(machine: Machine): void {
    this.selectedMachine = machine;
    this.showEditMachineModal = true;
  }

  openMachineDetailsModal(machine: Machine): void {
    this.selectedMachine = machine;
    this.showMachineDetailsModal = true;
  }

  openDeleteConfirmModal(machine: Machine): void {
    this.machineToDelete = machine;
    this.showDeleteConfirmModal = true;
  }

  /**
   * Closes all modal dialogs and resets selected machine references
   */
  closeModals(): void {
    this.showDeleteConfirmModal = false;
    this.showMachineDetailsModal = false;
    this.showAddMachineModal = false;
    this.showEditMachineModal = false;
    this.selectedMachine = null;
    this.machineToDelete = null;
  }

  /**
   * Handles successful machine save operations
   * Refreshes the machine list and closes modal dialogs
   */
  onMachineSaved(machine: Machine): void {
    this.loadMachines();
    this.closeModals();
  }

  /**
   * Confirms and executes machine deletion
   * Refreshes the machine list after successful deletion
   */
  confirmDelete(): void {
    if (this.machineToDelete) {
      const sub = this.machineService.deleteMachine(this.machineToDelete.id).subscribe({
        next: () => {
          this.loadMachines();
          this.closeModals();
        },
        error: (error) => {
          this.error = 'Failed to delete machine';
          this.isLoading = false;
          console.error('Error deleting machine:', error);
        }
      });
      this.subscriptions.push(sub);
    }
  }

  /**
   * Returns appropriate Bootstrap CSS class for machine status badges
   * Provides visual status indicators in the machine list
   */
  getStatusClass(status: MachineStatus): string {
    switch (status) {
      case MachineStatus.AVAILABLE:
        return 'bg-success';
      case MachineStatus.ASSIGNED:
        return 'bg-warning text-dark';
      case MachineStatus.IN_MAINTENANCE:
        return 'bg-info';
      case MachineStatus.UNDER_REPAIR:
        return 'bg-danger';
      case MachineStatus.OUT_OF_SERVICE:
        return 'bg-dark';
      case MachineStatus.RETIRED:
        return 'bg-secondary';
      default:
        return 'bg-light text-dark';
    }
  }

  /**
   * Navigates to the assignment requests management page
   */
  navigateToAssignmentRequests(): void {
    this.router.navigate(['/machine-manager/assignment-requests']);
  }

  // Getter methods for template dropdown options
  get machineTypeOptions() {
    return Object.values(MachineType);
  }

  get machineStatusOptions() {
    return Object.values(MachineStatus);
  }
}
