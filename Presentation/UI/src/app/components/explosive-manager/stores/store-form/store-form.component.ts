import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store, StoreStatus } from '../../../../core/models/store.model';
import { StoreService } from '../../../../core/services/store.service';
import { REGIONS } from '../../../../core/constants/regions';

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
  regions = REGIONS;
  explosiveTypesList = ['ANFO', 'Emulsion'];
  selectedExplosiveTypes: string[] = [];

  private fb = inject(FormBuilder);
  private storeService = inject(StoreService);
  private router = inject(Router);

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
      managerUserId: ['']
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
        managerUserId: this.store.managerUserId
      });

      // Populate explosive types checkboxes
      if (this.store.allowedExplosiveTypes) {
        this.selectedExplosiveTypes = this.store.allowedExplosiveTypes.split(',').map(t => t.trim());
      }
    }
  }

  onSubmit() {
    if (!this.storeForm) {
      console.error('Form not initialized');
      return;
    }

    if (this.storeForm.valid) {
      try {
        const formValue = {
          ...this.storeForm.value,
          regionId: Number(this.storeForm.value.regionId),
          storageCapacity: Number(this.storeForm.value.storageCapacity),
          status: this.storeForm.value.status ? Number(this.storeForm.value.status) : undefined,
          managerUserId: this.storeForm.value.managerUserId ? Number(this.storeForm.value.managerUserId) : undefined,
          allowedExplosiveTypes: this.selectedExplosiveTypes.join(',')
        };

        // Validate converted values
        if (isNaN(formValue.regionId) || formValue.regionId <= 0) {
          console.error('Invalid region ID');
          return;
        }

        if (isNaN(formValue.storageCapacity) || formValue.storageCapacity <= 0) {
          console.error('Invalid storage capacity');
          return;
        }

        this.formSubmit.emit(formValue);
      } catch (error) {
        console.error('Error preparing form data:', error);
      }
    } else {
      this.markFormGroupTouched();
    }
  }

  onExplosiveTypeChange(event: Event, type: string): void {
    const target = event.target as HTMLInputElement;

    if (target.checked) {
      if (!this.selectedExplosiveTypes.includes(type)) {
        this.selectedExplosiveTypes.push(type);
      }
    } else {
      const index = this.selectedExplosiveTypes.indexOf(type);
      if (index !== -1) {
        this.selectedExplosiveTypes.splice(index, 1);
      }
    }
  }

  isExplosiveTypeSelected(type: string): boolean {
    return this.selectedExplosiveTypes.includes(type);
  }

  onClose() {
    this.closeModal.emit();
  }

  onModalClick(event: Event) {
    event.stopPropagation();
  }

  isFieldInvalid(fieldName: string): boolean {
    if (!this.storeForm) {
      return false;
    }
    const field = this.storeForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  private markFormGroupTouched() {
    if (!this.storeForm) {
      return;
    }
    Object.keys(this.storeForm.controls).forEach(key => {
      const control = this.storeForm.get(key);
      if (control) {
        control.markAsTouched();
      }
    });
  }

}
