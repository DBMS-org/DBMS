import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-emulsion-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './emulsion-edit.component.html',
  styleUrl: './emulsion-edit.component.scss'
})
export class EmulsionEditComponent implements OnInit {
  emulsionForm!: FormGroup;
  isSubmitting = false;
  emulsionId: string | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.emulsionId = this.route.snapshot.paramMap.get('id');
    this.initializeForm();
    this.loadEmulsionData();
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

  private loadEmulsionData(): void {
    // Simulate loading data from API
    // In a real application, this would be a service call
    const mockData = {
      batchId: 'EM-2024-001',
      manufacturingDate: '2024-01-15',
      expiryDate: '2025-01-15',
      location: 'warehouse-a',
      quantity: 0.5,
      storageTemperature: 20,
      density: 1.2,
      viscosity: 15000,
      waterContent: 18,
      grade: 'standard',
      notes: 'High-quality emulsion explosive for mining operations'
    };

    // Populate the form with existing data
    this.emulsionForm.patchValue(mockData);
  }

  onSubmit(): void {
    if (this.emulsionForm.valid) {
      this.isSubmitting = true;
      
      // Simulate API call
      const formData = this.emulsionForm.value;
      console.log('Updating Emulsion stock:', formData);
      
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
