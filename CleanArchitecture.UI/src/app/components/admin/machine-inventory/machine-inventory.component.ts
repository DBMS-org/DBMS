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
  showEditMachineModal = false;
  selectedMachine: Machine | null = null;
  
  // Forms
  machineForm!: FormGroup;
  
  // Enums for template
  MachineStatus = MachineStatus;
  MachineType = MachineType;
  
  // Statistics
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
  ) {
    this.initializeForms();
  }

  ngOnInit(): void {
    this.loadMachines();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  private initializeForms(): void {
    this.machineForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      type: ['', Validators.required],
      model: ['', Validators.required],
      manufacturer: ['', Validators.required],
      serialNumber: ['', Validators.required],
      currentLocation: [''],
      power: [''],
      weight: [''],
      dimensions: [''],
      capacity: [''],
      fuelType: [''],
      maxOperatingDepth: [''],
      drillingDiameter: ['']
    });
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

  private calculateStatistics(): void {
    this.statistics = {
      total: this.machines.length,
      available: this.machines.filter(m => m.status === MachineStatus.AVAILABLE).length,
      assigned: this.machines.filter(m => m.status === MachineStatus.ASSIGNED).length,
      maintenance: this.machines.filter(m => m.status === MachineStatus.IN_MAINTENANCE).length,
      outOfService: this.machines.filter(m => 
        m.status === MachineStatus.OUT_OF_SERVICE || m.status === MachineStatus.UNDER_REPAIR
      ).length
    };
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

  openEditMachineModal(machine: Machine): void {
    this.selectedMachine = machine;
    this.machineForm.patchValue({
      name: machine.name,
      type: machine.type,
      model: machine.model,
      manufacturer: machine.manufacturer,
      serialNumber: machine.serialNumber,
      currentLocation: machine.currentLocation,
      power: machine.specifications?.power,
      weight: machine.specifications?.weight,
      dimensions: machine.specifications?.dimensions,
      capacity: machine.specifications?.capacity,
      fuelType: machine.specifications?.fuelType,
      maxOperatingDepth: machine.specifications?.maxOperatingDepth,
      drillingDiameter: machine.specifications?.drillingDiameter
    });
    this.showEditMachineModal = true;
  }

  closeMachineModal(): void {
    this.showEditMachineModal = false;
    this.selectedMachine = null;
  }

  saveMachine(): void {
    if (this.machineForm.invalid || !this.selectedMachine) return;

    const formValue = this.machineForm.value;
    
    const updatedMachine: Machine = {
      ...this.selectedMachine,
      name: formValue.name,
      type: formValue.type,
      model: formValue.model,
      manufacturer: formValue.manufacturer,
      serialNumber: formValue.serialNumber,
      currentLocation: formValue.currentLocation,
      specifications: {
        power: formValue.power,
        weight: formValue.weight,
        dimensions: formValue.dimensions,
        capacity: formValue.capacity,
        fuelType: formValue.fuelType,
        maxOperatingDepth: formValue.maxOperatingDepth,
        drillingDiameter: formValue.drillingDiameter
      }
    };

    const sub = this.machineService.updateMachine(this.selectedMachine.id, updatedMachine).subscribe({
      next: () => {
        this.loadMachines();
        this.closeMachineModal();
        console.log('Machine updated successfully');
      },
      error: (error) => {
        this.error = 'Failed to update machine';
        console.error('Error updating machine:', error);
      }
    });
    this.subscriptions.push(sub);
  }

  updateMachineStatus(machine: Machine, newStatus: MachineStatus): void {
    const sub = this.machineService.updateMachineStatus(machine.id, newStatus).subscribe({
      next: () => {
        machine.status = newStatus;
        this.calculateStatistics();
        this.applyFilters();
      },
      error: (error) => {
        this.error = 'Failed to update machine status';
        console.error('Error updating machine status:', error);
      }
    });
    this.subscriptions.push(sub);
  }

  viewMachine(machine: Machine): void {
    console.log('Viewing machine:', machine);
    // TODO: Implement machine view details logic
  }

  deleteMachine(machine: Machine): void {
    if (confirm(`Are you sure you want to delete ${machine.name}?`)) {
      const sub = this.machineService.deleteMachine(machine.id).subscribe({
        next: () => {
          this.loadMachines();
        },
        error: (error) => {
          this.error = 'Failed to delete machine';
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
