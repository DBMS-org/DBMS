import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MachineService } from '../../../../core/services/machine.service';
import { 
  Machine, 
  CreateMachineRequest,
  MachineType, 
  MachineStatus
} from '../../../../core/models/machine.model';

@Component({
  selector: 'app-add-machine',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './add-machine.component.html',
  styleUrl: './add-machine.component.scss'
})
export class AddMachineComponent {
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

  onSubmit(): void {
    if (this.machineForm.valid) {
      this.isLoading = true;
      this.error = null;

      const formValue = this.machineForm.value;
      const request: CreateMachineRequest = {
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
        status: formValue.status
      };
      
      this.machineService.addMachine(request).subscribe({
        next: (machine: Machine) => {
          this.machineSaved.emit(machine);
          this.isLoading = false;
        },
        error: (error: any) => {
          this.error = 'Failed to create machine. Please try again.';
          this.isLoading = false;
          console.error('Error creating machine:', error);
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