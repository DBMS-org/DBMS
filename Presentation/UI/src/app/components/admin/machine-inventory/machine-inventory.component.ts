import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MachineService } from '../../../core/services/machine.service';
import { AuthService } from '../../../core/services/auth.service';
import { UserService } from '../../../core/services/user.service';
import {
  Machine,
  MachineType,
  MachineStatus
} from '../../../core/models/machine.model';
import { User } from '../../../core/models/user.model';

@Component({
  selector: 'app-machine-inventory',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './machine-inventory.component.html',
  styleUrl: './machine-inventory.component.scss'
})
export class MachineInventoryComponent implements OnInit, OnDestroy {
  machines: Machine[] = [];
  filteredMachines: Machine[] = [];
  isLoading = false;
  error: string | null = null;
  
  // Filter and search properties
  searchTerm = '';
  selectedStatus: MachineStatus | 'ALL' = 'ALL';
  selectedType: MachineType | 'ALL' = 'ALL';
  
  // Modal states
  showMachineDetailsModal = false;
  showAssignOperatorModal = false;
  showReassignConfirmModal = false;
  selectedMachine: Machine | null = null;
  machineToAssignOperator: Machine | null = null;

  // Operators list
  operators: User[] = [];
  selectedOperatorId: number | null = null;
  operatorCurrentMachine: Machine | null = null;

  // Enums for template
  MachineStatus = MachineStatus;
  MachineType = MachineType;



  private subscriptions: Subscription[] = [];

  constructor(
    private machineService: MachineService,
    private authService: AuthService,
    private userService: UserService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadMachines();
    this.loadOperators();
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


  applyFilters(): void {
    this.filteredMachines = this.machines.filter(machine => {
      const matchesSearch = !this.searchTerm || 
        machine.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        machine.manufacturer.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        machine.model.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        machine.serialNumber.toLowerCase().includes(this.searchTerm.toLowerCase());
      
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

  viewMachine(machine: Machine): void {
    this.selectedMachine = machine;
    this.showMachineDetailsModal = true;
  }

  closeMachineDetailsModal(): void {
    this.showMachineDetailsModal = false;
    this.selectedMachine = null;
  }

  closeModals(): void {
    this.showMachineDetailsModal = false;
    this.showAssignOperatorModal = false;
    this.showReassignConfirmModal = false;
    this.selectedMachine = null;
    this.machineToAssignOperator = null;
    this.selectedOperatorId = null;
    this.operatorCurrentMachine = null;
  }

  private loadOperators(): void {
    const sub = this.userService.getUsers().subscribe({
      next: (users) => {
        // Filter only operators
        this.operators = users.filter(user => user.role === 'Operator');
      },
      error: (error) => {
        console.error('Error loading operators:', error);
      }
    });
    this.subscriptions.push(sub);
  }

  openAssignOperatorModal(machine: Machine): void {
    this.machineToAssignOperator = machine;
    this.selectedOperatorId = machine.operatorId || null;
    this.showAssignOperatorModal = true;
  }

  confirmAssignOperator(): void {
    if (!this.machineToAssignOperator) {
      return;
    }

    // If selectedOperatorId is null, it means unassign - allow this
    if (this.selectedOperatorId === null) {
      console.log('Unassigning operator from machine:', this.machineToAssignOperator.name);
      this.performOperatorAssignment();
      return;
    }

    // Get the selected operator's name for comparison
    const selectedOperator = this.operators.find(op => op.id === this.selectedOperatorId);

    // Debug logging
    console.log('Selected Operator ID:', this.selectedOperatorId);
    console.log('Selected Operator Name:', selectedOperator?.name);
    console.log('Current Machine ID:', this.machineToAssignOperator.id);
    console.log('All Machines:', this.machines.map(m => ({
      id: m.id,
      name: m.name,
      operatorId: m.operatorId,
      assignedToOperator: m.assignedToOperator
    })));

    // Check if operator is already assigned to another machine
    // Check both by operatorId (new assignments) and assignedToOperator name (legacy assignments)
    const operatorAlreadyAssigned = this.machines.find(m => {
      if (m.id === this.machineToAssignOperator!.id) return false;

      // Check by operatorId if available
      if (m.operatorId && m.operatorId === this.selectedOperatorId) return true;

      // Also check by operator name for legacy data
      if (m.assignedToOperator && selectedOperator && m.assignedToOperator === selectedOperator.name) return true;

      return false;
    });

    console.log('Operator Already Assigned:', operatorAlreadyAssigned);

    if (operatorAlreadyAssigned) {
      // Store the current machine and show confirmation dialog
      this.operatorCurrentMachine = operatorAlreadyAssigned;
      this.showAssignOperatorModal = false;
      this.showReassignConfirmModal = true;
      return;
    }

    // If operator is not assigned elsewhere, proceed directly
    this.performOperatorAssignment();
  }

  confirmReassignment(): void {
    this.performOperatorAssignment();
  }

  private performOperatorAssignment(): void {
    if (!this.machineToAssignOperator || this.selectedOperatorId === null) {
      return;
    }

    const updateData = {
      name: this.machineToAssignOperator.name,
      type: this.machineToAssignOperator.type,
      model: this.machineToAssignOperator.model,
      manufacturer: this.machineToAssignOperator.manufacturer,
      serialNumber: this.machineToAssignOperator.serialNumber,
      rigNo: this.machineToAssignOperator.rigNo,
      plateNo: this.machineToAssignOperator.plateNo,
      chassisDetails: this.machineToAssignOperator.chassisDetails,
      manufacturingYear: this.machineToAssignOperator.manufacturingYear,
      currentLocation: this.machineToAssignOperator.currentLocation,
      status: this.machineToAssignOperator.status,
      operatorId: this.selectedOperatorId,
      projectId: this.machineToAssignOperator.projectId,
      regionId: this.machineToAssignOperator.regionId
    };

    const sub = this.machineService.updateMachine(this.machineToAssignOperator.id, updateData).subscribe({
      next: () => {
        // If there was a previous machine, unassign the operator from it
        if (this.operatorCurrentMachine) {
          this.unassignOperatorFromMachine(this.operatorCurrentMachine.id);
        } else {
          this.loadMachines();
          this.closeModals();
        }
      },
      error: (error) => {
        this.error = 'Failed to assign operator';
        console.error('Error assigning operator:', error);
        this.closeModals();
      }
    });
    this.subscriptions.push(sub);
  }

  private unassignOperatorFromMachine(machineId: number): void {
    const machineToUnassign = this.machines.find(m => m.id === machineId);
    if (!machineToUnassign) {
      this.loadMachines();
      this.closeModals();
      return;
    }

    const updateData = {
      name: machineToUnassign.name,
      type: machineToUnassign.type,
      model: machineToUnassign.model,
      manufacturer: machineToUnassign.manufacturer,
      serialNumber: machineToUnassign.serialNumber,
      rigNo: machineToUnassign.rigNo,
      plateNo: machineToUnassign.plateNo,
      chassisDetails: machineToUnassign.chassisDetails,
      manufacturingYear: machineToUnassign.manufacturingYear,
      currentLocation: machineToUnassign.currentLocation,
      status: machineToUnassign.status,
      operatorId: undefined,
      projectId: machineToUnassign.projectId,
      regionId: machineToUnassign.regionId
    };

    const sub = this.machineService.updateMachine(machineId, updateData).subscribe({
      next: () => {
        this.loadMachines();
        this.closeModals();
      },
      error: (error) => {
        console.error('Error unassigning operator from previous machine:', error);
        this.loadMachines();
        this.closeModals();
      }
    });
    this.subscriptions.push(sub);
  }

  getStatusClass(status: MachineStatus): string {
    switch (status) {
      case MachineStatus.AVAILABLE:
        return 'bg-success';
      case MachineStatus.ASSIGNED:
        return 'bg-warning';
      case MachineStatus.IN_MAINTENANCE:
        return 'bg-info';
      case MachineStatus.UNDER_REPAIR:
        return 'bg-danger';
      case MachineStatus.OUT_OF_SERVICE:
        return 'bg-secondary';
      default:
        return 'bg-light';
    }
  }

  getStatusIcon(status: string): string {
    switch (status) {
      case 'Available':
        return 'check_circle';
      case 'Assigned':
        return 'assignment';
      case 'In Maintenance':
        return 'build';
      case 'Under Repair':
        return 'warning';
      case 'Out of Service':
        return 'block';
      case 'Retired':
        return 'archive';
      default:
        return 'help';
    }
  }

  formatDate(date: Date | string | undefined): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  get machineTypeOptions() {
    return Object.values(MachineType);
  }

  get machineStatusOptions() {
    return Object.values(MachineStatus);
  }

  navigateToAssignments(): void {
    this.router.navigate(['/admin/machine-assignments']);
  }
}
