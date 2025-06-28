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
  machines: Machine[] = [];
  filteredMachines: Machine[] = [];
  assignmentRequests: MachineAssignmentRequest[] = [];
  isLoading = false;
  error: string | null = null;
  
  // Filter and search properties
  searchTerm = '';
  selectedStatus: MachineStatus | 'ALL' = 'ALL';
  selectedType: MachineType | 'ALL' = 'ALL';
  
  // Modal states
  showDeleteConfirmModal = false;
  showMachineDetailsModal = false;
  showAddMachineModal = false;
  showEditMachineModal = false;
  selectedMachine: Machine | null = null;
  machineToDelete: Machine | null = null;
  
  // Enums for template
  MachineStatus = MachineStatus;
  MachineType = MachineType;
  AssignmentRequestStatus = AssignmentRequestStatus;
  
  // Statistics
  statistics = {
    total: 0,
    available: 0,
    assigned: 0,
    maintenance: 0,
    outOfService: 0,
    pendingRequests: 0
  };
  
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
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  private loadMachines(): void {
    this.isLoading = true;
    const sub = this.machineService.getAllMachines().subscribe({
      next: (machines) => {
        this.machines = machines;
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
        // Fallback to local calculation
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

  applyFilters(): void {
    this.filteredMachines = this.machines.filter(machine => {
      const matchesSearch = !this.searchTerm || 
        machine.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        machine.manufacturer.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        machine.model.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        machine.serialNumber.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        (machine.rigNo && machine.rigNo.toLowerCase().includes(this.searchTerm.toLowerCase())) ||
        (machine.plateNo && machine.plateNo.toLowerCase().includes(this.searchTerm.toLowerCase()));
      
      const matchesStatus = this.selectedStatus === 'ALL' || machine.status === this.selectedStatus;
      const matchesType = this.selectedType === 'ALL' || machine.type === this.selectedType;
      
      return matchesSearch && matchesStatus && matchesType;
    });
  }

  onSearchChange(): void {
    this.applyFilters();
  }

  onStatusFilterChange(): void {
    this.applyFilters();
  }

  onTypeFilterChange(): void {
    this.applyFilters();
  }

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

  closeModals(): void {
    this.showDeleteConfirmModal = false;
    this.showMachineDetailsModal = false;
    this.showAddMachineModal = false;
    this.showEditMachineModal = false;
    this.selectedMachine = null;
    this.machineToDelete = null;
  }

  onMachineSaved(machine: Machine): void {
    this.loadMachines();
    this.closeModals();
  }

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

  navigateToAssignmentRequests(): void {
    this.router.navigate(['/machine-manager/assignment-requests']);
  }

  get machineTypeOptions() {
    return Object.values(MachineType);
  }

  get machineStatusOptions() {
    return Object.values(MachineStatus);
  }
}
