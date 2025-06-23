import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MachineService } from '../../../core/services/machine.service';
import { AuthService } from '../../../core/services/auth.service';
import { 
  Machine, 
  MachineType, 
  MachineStatus
} from '../../../core/models/machine.model';

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
  showDeleteConfirmModal = false;
  selectedMachine: Machine | null = null;
  machineToDelete: Machine | null = null;
  
  // Enums for template
  MachineStatus = MachineStatus;
  MachineType = MachineType;
  
  // Add the missing statistics property
  statistics = {
    total: 0,
    available: 0,
    assigned: 0,
    maintenance: 0,
    outOfService: 0
  };
  
  private subscriptions: Subscription[] = [];

  constructor(
    private machineService: MachineService,
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadMachines();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  private loadMachines(): void {
    this.isLoading = true;
    const sub = this.machineService.getAllMachines().subscribe({
      next: (machines) => {
        this.machines = machines;
        this.calculateStatistics();
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

  private calculateStatistics(): void {
    this.statistics.total = this.machines.length;
    this.statistics.available = this.machines.filter(m => m.status === MachineStatus.AVAILABLE).length;
    this.statistics.assigned = this.machines.filter(m => m.status === MachineStatus.ASSIGNED).length;
    this.statistics.maintenance = this.machines.filter(m => 
      m.status === MachineStatus.IN_MAINTENANCE || m.status === MachineStatus.UNDER_REPAIR
    ).length;
    this.statistics.outOfService = this.machines.filter(m => m.status === MachineStatus.OUT_OF_SERVICE).length;
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

  deleteMachine(machine: Machine): void {
    this.machineToDelete = machine;
    this.showDeleteConfirmModal = true;
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
          console.error('Error deleting machine:', error);
        }
      });
      this.subscriptions.push(sub);
    }
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
    this.showDeleteConfirmModal = false;
    this.selectedMachine = null;
    this.machineToDelete = null;
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
