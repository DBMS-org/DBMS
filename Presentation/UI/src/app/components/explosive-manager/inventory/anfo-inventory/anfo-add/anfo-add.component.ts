import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-anfo-add',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './anfo-add.component.html',
  styleUrl: './anfo-add.component.scss'
})
export class AnfoAddComponent implements OnInit {
  anfoForm!: FormGroup;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  private initializeForm(): void {
    this.anfoForm = this.fb.group({
      // Core Information
      batchId: ['', [Validators.required, Validators.pattern(/^ANFO-\d{4}-\d{3}$/)]],
      manufacturingDate: ['', [Validators.required]],
      expiryDate: ['', [Validators.required]],
      supplier: ['', [Validators.required]],
      quantity: ['', [Validators.required, Validators.min(0.001)]],
      unit: ['kg', [Validators.required]],

      // Quality Parameters
      density: ['', [Validators.required, Validators.min(0.8), Validators.max(0.9)]],
      fuelOilContent: ['', [Validators.required, Validators.min(5.5), Validators.max(6.0)]],
      moistureContent: ['', [Validators.max(0.2)]],
      prillSize: ['', [Validators.min(1), Validators.max(3)]],
      detonationVelocity: ['', [Validators.min(3000), Validators.max(3500)]],

      // Manufacturing
      grade: ['TGAN', [Validators.required]],
      manufacturerBatchNumber: [''],

      // Storage
      storageLocation: ['', [Validators.required]],
      storageTemperature: ['', [Validators.required, Validators.min(5), Validators.max(35)]],
      storageHumidity: ['', [Validators.required, Validators.max(50)]],

      // Quality Control
      fumeClass: [1, [Validators.required]],
      qualityStatus: ['Pending', [Validators.required]],

      // Additional
      notes: ['']
    });
  }

  onSubmit(): void {
    if (this.anfoForm.valid) {
      this.isSubmitting = true;
      
      // Simulate API call
      const formData = this.anfoForm.value;
      console.log('Adding ANFO stock:', formData);
      
      // Simulate async operation
      setTimeout(() => {
        this.isSubmitting = false;
        // Navigate back to inventory list
        this.router.navigate(['/explosive-manager/inventory/anfo']);
      }, 1500);
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.anfoForm.controls).forEach(key => {
        this.anfoForm.get(key)?.markAsTouched();
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/explosive-manager/inventory/anfo']);
  }
}
