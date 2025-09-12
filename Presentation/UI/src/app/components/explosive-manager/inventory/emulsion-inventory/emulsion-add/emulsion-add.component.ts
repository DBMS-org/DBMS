import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-emulsion-add',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './emulsion-add.component.html',
  styleUrl: './emulsion-add.component.scss'
})
export class EmulsionAddComponent implements OnInit {
  emulsionForm!: FormGroup;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  private initializeForm(): void {
    this.emulsionForm = this.fb.group({
      batchId: ['', [Validators.required]],
      manufacturingDate: ['', [Validators.required]],
      expiryDate: ['', [Validators.required]],
      location: ['', [Validators.required]],
      quantity: ['', [Validators.required, Validators.min(0.001)]],
      storageTemperature: [''],
      density: ['', [Validators.required, Validators.min(0.8), Validators.max(1.5)]],
      viscosity: [''],
      waterContent: [''],
      grade: [''],
      notes: ['']
    });
  }

  onSubmit(): void {
    if (this.emulsionForm.valid) {
      this.isSubmitting = true;
      
      // Simulate API call
      const formData = this.emulsionForm.value;
      console.log('Adding Emulsion stock:', formData);
      
      // Simulate async operation
      setTimeout(() => {
        this.isSubmitting = false;
        // Navigate back to inventory list
        this.router.navigate(['/explosive-manager/inventory/emulsion']);
      }, 1500);
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.emulsionForm.controls).forEach(key => {
        this.emulsionForm.get(key)?.markAsTouched();
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/explosive-manager/inventory/emulsion']);
  }
}
