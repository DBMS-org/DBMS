import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CentralInventoryService } from '../../../../../core/services/central-inventory.service';
import { UpdateANFOInventoryRequest, ANFOGrade, FumeClass, QualityStatus } from '../../../../../core/models/central-inventory.model';

@Component({
  selector: 'app-anfo-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './anfo-edit.component.html',
  styleUrl: './anfo-edit.component.scss'
})
export class AnfoEditComponent implements OnInit {
  anfoForm!: FormGroup;
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
    this.loadItemData();
  }

  private initializeForm(): void {
    this.anfoForm = this.fb.group({
      quantity: ['', [Validators.required, Validators.min(0.001)]],
      density: ['', [Validators.required, Validators.min(0.8), Validators.max(0.9)]],
      fuelOilContent: ['', [Validators.required, Validators.min(5.5), Validators.max(6.0)]],
      moistureContent: ['', [Validators.max(0.2)]],
      prillSize: ['', [Validators.min(1), Validators.max(3)]],
      detonationVelocity: ['', [Validators.min(3000), Validators.max(3500)]],
      grade: ['Standard', [Validators.required]],
      manufacturerBatchNumber: [''],
      fumeClass: ['Class1', [Validators.required]],
      qualityStatus: ['Approved', [Validators.required]],
      waterResistance: ['None'],
      notes: [''],
      storageLocation: ['', [Validators.required]],
      storageTemperature: ['', [Validators.required, Validators.min(5), Validators.max(35)]],
      storageHumidity: ['', [Validators.required, Validators.max(50)]]
    });
  }

  private loadItemData(): void {
    if (this.itemId) {
      this.isLoading = true;
      this.inventoryService.getInventoryById(this.itemId).subscribe({
        next: (inventory) => {
          const properties = inventory.anfoProperties;

          this.anfoForm.patchValue({
            quantity: inventory.quantity,
            density: properties?.density || '',
            fuelOilContent: properties?.fuelOilContent || '',
            moistureContent: properties?.moistureContent || '',
            prillSize: properties?.prillSize || '',
            detonationVelocity: properties?.detonationVelocity || '',
            grade: properties?.grade || 'Standard',
            manufacturerBatchNumber: inventory.manufacturerBatchNumber || '',
            fumeClass: properties?.fumeClass || 'Class1',
            qualityStatus: properties?.qualityStatus || 'Approved',
            waterResistance: properties?.waterResistance || 'None',
            notes: properties?.notes || '',
            storageLocation: inventory.storageLocation || '',
            storageTemperature: properties?.storageTemperature || '',
            storageHumidity: properties?.storageHumidity || ''
          });

          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading ANFO data:', error);
          this.errorMessage = error.message || 'Failed to load ANFO data';
          this.isLoading = false;
        }
      });
    }
  }

  onSubmit(): void {
    if (this.anfoForm.valid && this.itemId) {
      this.isSubmitting = true;
      this.errorMessage = null;

      const formData = this.anfoForm.value;
      const request: UpdateANFOInventoryRequest = {
        quantity: formData.quantity,
        density: formData.density,
        fuelOilContent: formData.fuelOilContent,
        moistureContent: formData.moistureContent,
        storageTemperature: formData.storageTemperature,
        storageHumidity: formData.storageHumidity,
        qualityStatus: formData.qualityStatus as QualityStatus,
        notes: formData.notes,
        storageLocation: formData.storageLocation
      };

      this.inventoryService.updateANFOBatch(this.itemId, request).subscribe({
        next: () => {
          this.isSubmitting = false;
          this.router.navigate(['/explosive-manager/inventory/anfo']);
        },
        error: (error) => {
          console.error('Error updating ANFO:', error);
          this.errorMessage = error.message || 'Failed to update ANFO';
          this.isSubmitting = false;
        }
      });
    } else {
      Object.keys(this.anfoForm.controls).forEach(key => {
        this.anfoForm.get(key)?.markAsTouched();
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/explosive-manager/inventory/anfo']);
  }

  getCurrentDateTime(): string {
    return new Date().toLocaleString();
  }
}
