import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { Store, StoreStatus } from '../../../../core/models/store.model';

@Component({
  selector: 'app-store-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './store-form.component.html',
  styleUrl: './store-form.component.scss'
})
export class StoreFormComponent implements OnInit, OnChanges {
  @Input() store: Store | null = null;
  @Input() isVisible: boolean = false;
  @Input() isEditMode: boolean = false;
  @Input() successMessage: string = '';
  @Input() errorMessage: string = '';
  @Input() explosiveTypes: string[] = [];
  @Input() storeStatuses: string[] = [];
  @Input() uniqueLocations: string[] = [];
  @Input() uniqueManagers: string[] = [];

  @Output() formSubmit = new EventEmitter<any>();
  @Output() closeModal = new EventEmitter<void>();

  storeForm!: FormGroup;
  isSubmitting: boolean = false;

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.initializeForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['store'] && this.storeForm) {
      this.populateForm();
    }
  }

  initializeForm() {
    this.storeForm = this.fb.group({
      storeName: ['', [Validators.required, Validators.minLength(2)]],
      storeAddress: ['', [Validators.required]],
      storageCapacity: ['', [Validators.required, Validators.min(1)]],
      city: ['', [Validators.required]],
      status: [StoreStatus.Operational, [Validators.required]],
      regionId: ['', [Validators.required]],
      projectId: ['', [Validators.required]],
      managerUserId: ['', [Validators.required]]
    });

    if (this.store) {
      this.populateForm();
    }
  }

  populateForm() {
    if (this.store && this.storeForm) {
      this.storeForm.patchValue({
        storeName: this.store.storeName,
        storeAddress: this.store.storeAddress,
        storageCapacity: this.store.storageCapacity,
        city: this.store.city,
        status: this.store.status,
        regionId: this.store.regionId,
        projectId: this.store.projectId,
        managerUserId: this.store.managerUserId
      });
    }
  }

  onSubmit() {
    if (this.storeForm.valid) {
      this.isSubmitting = true;
      const formData = this.storeForm.value;
      const storeData = {
        ...formData,
        location: {
          city: formData.locationCity,
          region: formData.locationRegion
        }
      };
      
      // Remove the separate location fields
      delete storeData.locationCity;
      delete storeData.locationRegion;
      
      this.formSubmit.emit(storeData);
      
      // Reset submitting state after a delay
      setTimeout(() => {
        this.isSubmitting = false;
      }, 1000);
    } else {
      this.markFormGroupTouched();
    }
  }

  onClose() {
    this.closeModal.emit();
  }

  onExplosiveTypeChange(event: Event, type: string): void {
    const target = event.target as HTMLInputElement;
    const explosiveTypesArray = this.storeForm.get('explosiveTypes') as FormArray;
    
    if (target.checked) {
      explosiveTypesArray.push(this.fb.control(type));
    } else {
      const index = explosiveTypesArray.controls.findIndex(control => control.value === type);
      if (index !== -1) {
        explosiveTypesArray.removeAt(index);
      }
    }
  }

  isExplosiveTypeSelected(type: string): boolean {
    const explosiveTypesArray = this.storeForm.get('explosiveTypes') as FormArray;
    return explosiveTypesArray.controls.some(control => control.value === type);
  }

  onModalClick(event: Event) {
    event.stopPropagation();
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.storeForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  private markFormGroupTouched() {
    Object.keys(this.storeForm.controls).forEach(key => {
      const control = this.storeForm.get(key);
      if (control) {
        control.markAsTouched();
      }
    });
  }
}
