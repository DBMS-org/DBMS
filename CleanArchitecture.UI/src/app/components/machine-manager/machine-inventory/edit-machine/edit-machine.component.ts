import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MachineService } from '../../../../core/services/machine.service';
import { 
  Machine, 
  UpdateMachineRequest,
  MachineType, 
  MachineStatus
} from '../../../../core/models/machine.model';

@Component({
  selector: 'app-edit-machine',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './edit-machine.component.html',
  styleUrl: './edit-machine.component.scss'
})
export class EditMachineComponent implements OnInit {
  @Input() machine!: Machine;
  @Output() machineSaved = new EventEmitter<Machine>();
  @Output() close = new EventEmitter<void>();

  machineForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  // Enums for template
  MachineType = MachineType;
  MachineStatus = MachineStatus;

  constructor(
    private formBuilder: FormBuilder,
    private machineService: MachineService
  ) {
    this.machineForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      type: [MachineType.DRILL_RIG, Validators.required],
      manufacturer: ['', [Validators.required, Validators.minLength(2)]],
      model: ['', [Validators.required, Validators.minLength(2)]],
      serialNumber: ['', [Validators.required, Validators.minLength(3)]],
      rigNo: [''],
      plateNo: [''],
      manufacturingYear: ['', [Validators.pattern(/^\d{4}$/)]],
      chassisDetails: [''],
      currentLocation: [''],
      status: [MachineStatus.AVAILABLE, Validators.required]
    });
  }

  ngOnInit(): void {
    if (this.machine) {
      this.machineForm.patchValue({
        name: this.machine.name,
        type: this.machine.type,
        manufacturer: this.machine.manufacturer,
        model: this.machine.model,
        serialNumber: this.machine.serialNumber,
        rigNo: this.machine.rigNo || '',
        plateNo: this.machine.plateNo || '',
        manufacturingYear: this.machine.manufacturingYear?.toString() || '',
        chassisDetails: this.machine.chassisDetails || '',
        currentLocation: this.machine.currentLocation || '',
        status: this.machine.status
      });
    }
  }

  onSubmit(): void {
    if (this.machineForm.valid) {
      this.isLoading = true;
      this.error = null;

      const formValue = this.machineForm.value;
      const request: UpdateMachineRequest = {
        name: formValue.name,
        type: formValue.type,
        manufacturer: formValue.manufacturer,
        model: formValue.model,
        serialNumber: formValue.serialNumber,
        rigNo: formValue.rigNo || undefined,
        plateNo: formValue.plateNo || undefined,
        manufacturingYear: formValue.manufacturingYear ? parseInt(formValue.manufacturingYear) : undefined,
        chassisDetails: formValue.chassisDetails || undefined,
        currentLocation: formValue.currentLocation || undefined,
        status: formValue.status,
        lastMaintenanceDate: this.machine.lastMaintenanceDate,
        nextMaintenanceDate: this.machine.nextMaintenanceDate
      };
      
      this.machineService.updateMachine(this.machine.id, request).subscribe({
        next: (machine: Machine) => {
          this.machineSaved.emit(machine);
          this.isLoading = false;
        },
        error: (error: any) => {
          this.error = 'Failed to update machine. Please try again.';
          this.isLoading = false;
          console.error('Error updating machine:', error);
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  onCancel(): void {
    this.close.emit();
  }

  private markFormGroupTouched(): void {
    Object.keys(this.machineForm.controls).forEach(key => {
      const control = this.machineForm.get(key);
      control?.markAsTouched();
    });
  }

  get machineTypeOptions() {
    return Object.values(MachineType);
  }

  get machineStatusOptions() {
    return Object.values(MachineStatus);
  }
} 