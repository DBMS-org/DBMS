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
      // Core Information
      batchId: ['', [Validators.required, Validators.pattern(/^EMU-\d{4}-\d{3}$/)]],
      manufacturingDate: ['', [Validators.required]],
      expiryDate: ['', [Validators.required]],
      supplier: ['', [Validators.required]],
      quantity: ['', [Validators.required, Validators.min(0.001)]],
      unit: ['kg', [Validators.required]],

      // Quality Parameters
      densityUnsensitized: ['', [Validators.required, Validators.min(1.30), Validators.max(1.45)]],
      densitySensitized: ['', [Validators.required, Validators.min(1.10), Validators.max(1.30)]],
      viscosity: ['', [Validators.required, Validators.min(50000), Validators.max(200000)]],
      waterContent: ['', [Validators.required, Validators.min(12), Validators.max(16)]],
      pH: ['', [Validators.required, Validators.min(4.5), Validators.max(6.5)]],
      detonationVelocity: ['', [Validators.min(4500), Validators.max(6000)]],
      bubbleSize: ['', [Validators.min(10), Validators.max(100)]],

      // Temperature
      storageTemperature: ['', [Validators.required, Validators.min(-20), Validators.max(50)]],
      applicationTemperature: ['', [Validators.min(0), Validators.max(45)]],

      // Manufacturing
      grade: ['Standard', [Validators.required]],
      manufacturerBatchNumber: [''],
      color: ['White', [Validators.required]],

      // Sensitization
      sensitizationType: ['Chemical', [Validators.required]],
      sensitizerContent: ['', [Validators.min(0), Validators.max(100)]],

      // Storage
      storageLocation: ['', [Validators.required]],

      // Quality Control
      fumeClass: [1, [Validators.required]],
      qualityStatus: ['Pending', [Validators.required]],

      // Additional
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
