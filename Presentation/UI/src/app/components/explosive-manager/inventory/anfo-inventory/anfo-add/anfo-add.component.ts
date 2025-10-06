import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CentralInventoryService } from '../../../../../core/services/central-inventory.service';
import { CreateANFOInventoryRequest, ANFOGrade, FumeClass, QualityStatus } from '../../../../../core/models/central-inventory.model';

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
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private inventoryService: CentralInventoryService
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
      centralWarehouseStoreId: [1, [Validators.required]], // Default to central warehouse

      // Quality Parameters
      density: ['', [Validators.required, Validators.min(0.8), Validators.max(0.9)]],
      fuelOilContent: ['', [Validators.required, Validators.min(5.5), Validators.max(6.0)]],
      moistureContent: ['', [Validators.max(0.2)]],
      prillSize: ['', [Validators.min(1), Validators.max(3)]],
      detonationVelocity: ['', [Validators.min(3000), Validators.max(3500)]],

      // Manufacturing
      grade: ['Standard', [Validators.required]],
      manufacturerBatchNumber: [''],
      fumeClass: ['Class1', [Validators.required]],
      qualityStatus: ['Approved', [Validators.required]],
      waterResistance: ['None'],
      notes: [''],

      // Storage
      storageLocation: ['', [Validators.required]],
      storageTemperature: ['', [Validators.required, Validators.min(5), Validators.max(35)]],
      storageHumidity: ['', [Validators.required, Validators.max(50)]]
    });
  }

  onSubmit(): void {
    if (this.anfoForm.valid) {
      this.isSubmitting = true;
      this.errorMessage = null;

      const formData = this.anfoForm.value;

      // Convert dates to proper format
      const request: CreateANFOInventoryRequest = {
        ...formData,
        manufacturingDate: new Date(formData.manufacturingDate),
        expiryDate: new Date(formData.expiryDate),
        grade: formData.grade as ANFOGrade,
        fumeClass: formData.fumeClass as FumeClass,
        qualityStatus: formData.qualityStatus as QualityStatus
      };

      console.log('Creating ANFO batch:', request);

      this.inventoryService.createANFOBatch(request).subscribe({
        next: (response) => {
          console.log('ANFO batch created successfully:', response);
          this.isSubmitting = false;
          // Navigate back to inventory list
          this.router.navigate(['/explosive-manager/inventory/anfo']);
        },
        error: (error) => {
          console.error('Error creating ANFO batch:', error);
          this.errorMessage = error.message || 'Failed to create ANFO batch';
          this.isSubmitting = false;
        }
      });
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
