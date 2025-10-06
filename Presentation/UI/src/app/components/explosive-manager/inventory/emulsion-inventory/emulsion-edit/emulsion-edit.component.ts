import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CentralInventoryService } from '../../../../../core/services/central-inventory.service';
import { UpdateEmulsionInventoryRequest, EmulsionGrade, SensitizationType, FumeClass, QualityStatus } from '../../../../../core/models/central-inventory.model';

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
  isLoading = false;
  errorMessage: string | null = null;
  itemId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private inventoryService: CentralInventoryService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.itemId = id ? parseInt(id, 10) : null;
    this.initializeForm();
    this.loadEmulsionData();
  }

  private initializeForm(): void {
    this.emulsionForm = this.fb.group({
      quantity: ['', [Validators.required, Validators.min(0.001)]],
      densityUnsensitized: ['', [Validators.required, Validators.min(1.30), Validators.max(1.45)]],
      densitySensitized: ['', [Validators.required, Validators.min(1.10), Validators.max(1.30)]],
      viscosity: ['', [Validators.required, Validators.min(50000), Validators.max(200000)]],
      waterContent: ['', [Validators.required, Validators.min(12), Validators.max(16)]],
      pH: ['', [Validators.required, Validators.min(4.5), Validators.max(6.5)]],
      detonationVelocity: ['', [Validators.min(4500), Validators.max(6000)]],
      bubbleSize: ['', [Validators.min(10), Validators.max(100)]],
      storageTemperature: ['', [Validators.required, Validators.min(-20), Validators.max(50)]],
      applicationTemperature: ['', [Validators.min(0), Validators.max(45)]],
      grade: ['Standard', [Validators.required]],
      manufacturerBatchNumber: [''],
      color: ['White', [Validators.required]],
      fumeClass: ['Class1', [Validators.required]],
      qualityStatus: ['Approved', [Validators.required]],
      waterResistance: ['Excellent'],
      notes: [''],
      sensitizationType: ['Chemical', [Validators.required]],
      sensitizerContent: ['', [Validators.min(0), Validators.max(100)]],
      storageLocation: ['', [Validators.required]]
    });
  }

  private loadEmulsionData(): void {
    if (this.itemId) {
      this.isLoading = true;
      this.inventoryService.getInventoryById(this.itemId).subscribe({
        next: (inventory) => {
          const properties = inventory.emulsionProperties;

          this.emulsionForm.patchValue({
            quantity: inventory.quantity,
            densityUnsensitized: properties?.densityUnsensitized || '',
            densitySensitized: properties?.densitySensitized || '',
            viscosity: properties?.viscosity || '',
            waterContent: properties?.waterContent || '',
            pH: properties?.pH || '',
            detonationVelocity: properties?.detonationVelocity || '',
            bubbleSize: properties?.bubbleSize || '',
            storageTemperature: properties?.storageTemperature || '',
            applicationTemperature: properties?.applicationTemperature || '',
            grade: properties?.grade || 'Standard',
            manufacturerBatchNumber: inventory.manufacturerBatchNumber || '',
            color: properties?.color || 'White',
            fumeClass: properties?.fumeClass || 'Class1',
            qualityStatus: properties?.qualityStatus || 'Approved',
            waterResistance: properties?.waterResistance || 'Excellent',
            notes: properties?.notes || '',
            sensitizationType: properties?.sensitizationType || 'Chemical',
            sensitizerContent: properties?.sensitizerContent || '',
            storageLocation: inventory.storageLocation || ''
          });

          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading Emulsion data:', error);
          this.errorMessage = error.message || 'Failed to load Emulsion data';
          this.isLoading = false;
        }
      });
    }
  }

  onSubmit(): void {
    if (this.emulsionForm.valid && this.itemId) {
      this.isSubmitting = true;
      this.errorMessage = null;

      const formData = this.emulsionForm.value;
      const request: UpdateEmulsionInventoryRequest = {
        quantity: formData.quantity,
        densityUnsensitized: formData.densityUnsensitized,
        densitySensitized: formData.densitySensitized,
        viscosity: formData.viscosity,
        waterContent: formData.waterContent,
        pH: formData.pH,
        storageTemperature: formData.storageTemperature,
        applicationTemperature: formData.applicationTemperature,
        qualityStatus: formData.qualityStatus as QualityStatus,
        notes: formData.notes,
        storageLocation: formData.storageLocation
      };

      this.inventoryService.updateEmulsionBatch(this.itemId, request).subscribe({
        next: () => {
          this.isSubmitting = false;
          this.router.navigate(['/explosive-manager/inventory/emulsion']);
        },
        error: (error) => {
          console.error('Error updating Emulsion:', error);
          this.errorMessage = error.message || 'Failed to update Emulsion';
          this.isSubmitting = false;
        }
      });
    } else {
      Object.keys(this.emulsionForm.controls).forEach(key => {
        this.emulsionForm.get(key)?.markAsTouched();
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/explosive-manager/inventory/emulsion']);
  }
}
