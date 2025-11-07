import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MachineService } from '../../../core/services/machine.service';
import { Machine } from '../../../core/models/machine.model';

@Component({
  selector: 'app-edit-machine',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit-machine.component.html',
  styleUrls: ['./edit-machine.component.scss']
})
export class EditMachineComponent implements OnInit {
  @Input() machine!: Machine;
  @Output() machineSaved = new EventEmitter<Machine>();
  @Output() close = new EventEmitter<void>();

  machineForm: FormGroup;
  isSubmitting = false;
  error: string | null = null;
  successMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private machineService: MachineService
  ) {
    this.machineForm = this.createForm();
  }

  ngOnInit(): void {
    if (this.machine) {
      this.populateForm();
    }
  }

  get currentYear(): number {
    return new Date().getFullYear();
  }

  private createForm(): FormGroup {
    return this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      operatorName: ['', [Validators.required, Validators.minLength(2)]],
      model: ['', [Validators.required]],
      serialNumber: ['', [Validators.required]],
      rigNo: ['', [Validators.required]],
      plateNo: ['', [Validators.required]],

      chassisDetails: ['', [Validators.required, Validators.minLength(10)]],
      manufacturingYear: ['', [
        Validators.required,
        Validators.min(1900),
        Validators.max(this.currentYear)
      ]]
    });
  }

  private populateForm(): void {
    this.machineForm.patchValue({
      name: this.machine.name || '',
      operatorName: this.machine.operatorName || '',
      model: this.machine.model || '',
      serialNumber: this.machine.serialNumber || '',
      rigNo: this.machine.rigNo || '',
      plateNo: this.machine.plateNo || '',

      chassisDetails: this.machine.chassisDetails || '',
      manufacturingYear: this.machine.manufacturingYear || ''
    });
  }

  isFieldValid(fieldName: string): boolean {
    const field = this.machineForm.get(fieldName);
    return field ? field.valid && field.touched : false;
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.machineForm.get(fieldName);
    return field ? field.invalid && field.touched : false;
  }

  getFieldError(fieldName: string): string {
    const field = this.machineForm.get(fieldName);
    if (!field || !field.errors) return '';

    const errors = field.errors;
    const fieldDisplayNames: { [key: string]: string } = {
      name: 'Machine Name',
      operatorName: 'Operator Name',
      model: 'Model',
      serialNumber: 'Serial Number',
      rigNo: 'Rig No',
      plateNo: 'Plate No',

      chassisDetails: 'Chassis Details',
      manufacturingYear: 'Manufacturing Year'
    };

    const displayName = fieldDisplayNames[fieldName] || fieldName;

    if (errors['required']) return `${displayName} is required`;
    if (errors['minlength']) return `${displayName} must be at least ${errors['minlength'].requiredLength} characters`;
    if (errors['min']) return `${displayName} must be at least ${errors['min'].min}`;
    if (errors['max']) return `${displayName} cannot exceed ${errors['max'].max}`;

    return `${displayName} is invalid`;
  }

  isFormComplete(): boolean {
    return this.machineForm.valid;
  }

  getCompletionPercentage(): number {
    const totalFields = Object.keys(this.machineForm.controls).length;
    const validFields = Object.keys(this.machineForm.controls)
      .filter(key => this.machineForm.get(key)?.valid).length;
    
    return Math.round((validFields / totalFields) * 100);
  }

  getTotalErrors(): number {
    let errorCount = 0;
    Object.keys(this.machineForm.controls).forEach(key => {
      const control = this.machineForm.get(key);
      if (control && control.invalid && control.touched) {
        errorCount++;
      }
    });
    return errorCount;
  }

  onSubmit(): void {
    if (this.machineForm.valid) {
      this.isSubmitting = true;
      this.error = null;

      const updatedMachine: Machine = {
        ...this.machine,
        ...this.machineForm.value,
        updatedAt: new Date()
      };

      this.machineService.updateMachine(this.machine.id, updatedMachine).subscribe({
        next: (savedMachine) => {
          this.successMessage = 'Machine updated successfully!';
          
          // Auto-close after success
          setTimeout(() => {
            this.machineSaved.emit(savedMachine);
          }, 1500);
        },
        error: (error: any) => {
          console.error('Error updating machine:', error);
          this.error = error.error?.message || 'Failed to update machine. Please try again.';
          this.isSubmitting = false;
        },
        complete: () => {
          this.isSubmitting = false;
        }
      });
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.machineForm.controls).forEach(key => {
        this.machineForm.get(key)?.markAsTouched();
      });
      this.error = 'Please fill in all required fields correctly.';
    }
  }

  onCancel(): void {
    this.close.emit();
  }
}
