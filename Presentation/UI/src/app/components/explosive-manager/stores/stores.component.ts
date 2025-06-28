import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { StoreService } from '../../../core/services/store.service';
import { 
  Store, 
  StoreStatistics, 
  StoreFilters, 
  CreateStoreRequest, 
  UpdateStoreRequest,
  StoreType,
  ExplosiveType,
  StoreStatus,
  StorageCapacityUnit,
  SecurityLevel
} from '../../../core/models/store.model';

@Component({
  selector: 'app-stores',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './stores.component.html',
  styleUrls: ['./stores.component.scss']
})
export class StoresComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  // Data properties
  stores: Store[] = [];
  filteredStores: Store[] = [];
  statistics: StoreStatistics | null = null;
  selectedStore: Store | null = null;

  // UI state properties
  isLoading = false;
  showAddStoreModal = false;
  showEditStoreModal = false;
  showViewStoreModal = false;
  showDeleteConfirmModal = false;
  showDeactivateConfirmModal = false;

  // Forms - properly initialized
  addStoreForm!: FormGroup;
  editStoreForm!: FormGroup;

  // Filters and search
  filters: StoreFilters = {
    status: 'ALL',
    storeType: 'ALL',
    location: 'ALL',
    storeManager: 'ALL',
    isActive: null,
    searchTerm: ''
  };

  // Enums for templates
  StoreType = StoreType;
  ExplosiveType = ExplosiveType;
  StoreStatus = StoreStatus;
  StorageCapacityUnit = StorageCapacityUnit;
  SecurityLevel = SecurityLevel;

  // Enum arrays for dropdowns
  storeTypes = Object.values(StoreType);
  explosiveTypes = Object.values(ExplosiveType);
  storeStatuses = Object.values(StoreStatus);
  storageCapacityUnits = Object.values(StorageCapacityUnit);
  securityLevels = Object.values(SecurityLevel);

  // Unique values for filters
  uniqueLocations: string[] = [];
  uniqueManagers: string[] = [];

  // Success/Error messages
  successMessage = '';
  errorMessage = '';

  // Additional properties for project consistency
  error: string | null = null;

  constructor(
    private storeService: StoreService,
    private fb: FormBuilder
  ) {
    this.initializeForms();
  }

  ngOnInit(): void {
    this.loadStores();
    this.loadStatistics();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // Initialization methods
  private initializeForms(): void {
    this.addStoreForm = this.fb.group({
      storeName: ['', [Validators.required, Validators.minLength(3)]],
      storeAddress: ['', [Validators.required, Validators.minLength(10)]],
      storeManagerName: ['', [Validators.required, Validators.minLength(2)]],
      storeManagerContact: ['', [Validators.required, Validators.pattern(/^\+?[\d\s\-\(\)]{10,}$/)]],
      storeManagerEmail: ['', [Validators.email]],
      storeType: ['', Validators.required],
      explosiveTypesAvailable: this.fb.array([], Validators.required),
      storageCapacity: ['', [Validators.required, Validators.min(1)]],
      storageCapacityUnit: ['', Validators.required],
      location: this.fb.group({
        city: ['', Validators.required],
        region: ['', Validators.required],
        country: ['Oman', Validators.required]
      }),
      securityLevel: ['', Validators.required]
    });

    this.editStoreForm = this.fb.group({
      id: ['', Validators.required],
      storeName: ['', [Validators.required, Validators.minLength(3)]],
      storeAddress: ['', [Validators.required, Validators.minLength(10)]],
      storeManagerName: ['', [Validators.required, Validators.minLength(2)]],
      storeManagerContact: ['', [Validators.required, Validators.pattern(/^\+?[\d\s\-\(\)]{10,}$/)]],
      storeManagerEmail: ['', [Validators.email]],
      storeType: ['', Validators.required],
      explosiveTypesAvailable: this.fb.array([], Validators.required),
      storageCapacity: ['', [Validators.required, Validators.min(1)]],
      storageCapacityUnit: ['', Validators.required],
      location: this.fb.group({
        city: ['', Validators.required],
        region: ['', Validators.required],
        country: ['Oman', Validators.required]
      }),
      securityLevel: ['', Validators.required]
    });
  }

  // Data loading methods
  private loadStores(): void {
    this.isLoading = true;
    this.storeService.getAllStores()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (stores) => {
          this.stores = stores;
          this.applyFilters();
          this.updateUniqueValues();
          this.isLoading = false;
        },
        error: (error) => {
          this.showError('Failed to load stores');
          this.isLoading = false;
        }
      });
  }

  private loadStatistics(): void {
    this.storeService.getStoreStatistics()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (statistics) => {
          this.statistics = statistics;
        },
        error: (error) => {
          console.error('Failed to load statistics', error);
        }
      });
  }

  private updateUniqueValues(): void {
    this.uniqueLocations = [...new Set(this.stores.map(s => s.location.city))];
    this.uniqueManagers = [...new Set(this.stores.map(s => s.storeManagerName))];
  }

  // Filter and search methods
  applyFilters(): void {
    this.storeService.filterStores(this.filters)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (filteredStores) => {
          this.filteredStores = filteredStores;
        },
        error: (error) => {
          this.showError('Failed to apply filters');
        }
      });
  }

  onFilterChange(): void {
    this.applyFilters();
  }

  onSearchChange(): void {
    this.applyFilters();
  }

  clearFilters(): void {
    this.filters = {
      status: 'ALL',
      storeType: 'ALL',
      location: 'ALL',
      storeManager: 'ALL',
      isActive: null,
      searchTerm: ''
    };
    this.applyFilters();
  }

  // Modal methods
  openAddStoreModal(): void {
    this.addStoreForm.reset();
    this.clearExplosiveTypesArray('add');
    this.showAddStoreModal = true;
  }

  openEditStoreModal(store: Store): void {
    this.selectedStore = store;
    this.populateEditForm(store);
    this.showEditStoreModal = true;
  }

  openViewStoreModal(store: Store): void {
    this.selectedStore = store;
    this.showViewStoreModal = true;
  }

  openDeleteConfirmModal(store: Store): void {
    this.selectedStore = store;
    this.showDeleteConfirmModal = true;
  }

  openDeactivateConfirmModal(store: Store): void {
    this.selectedStore = store;
    this.showDeactivateConfirmModal = true;
  }

  closeAllModals(): void {
    this.showAddStoreModal = false;
    this.showEditStoreModal = false;
    this.showViewStoreModal = false;
    this.showDeleteConfirmModal = false;
    this.showDeactivateConfirmModal = false;
    this.selectedStore = null;
    this.clearMessages();
  }

  // Form methods
  private populateEditForm(store: Store): void {
    this.editStoreForm.patchValue({
      id: store.id,
      storeName: store.storeName,
      storeAddress: store.storeAddress,
      storeManagerName: store.storeManagerName,
      storeManagerContact: store.storeManagerContact,
      storeManagerEmail: store.storeManagerEmail,
      storeType: store.storeType,
      storageCapacity: store.storageCapacity,
      storageCapacityUnit: store.storageCapacityUnit,
      location: {
        city: store.location.city,
        region: store.location.region,
        country: store.location.country
      },
      securityLevel: store.securityLevel
    });

    this.setExplosiveTypesArray('edit', store.explosiveTypesAvailable);
  }

  // Explosive types array management
  get addExplosiveTypesArray(): FormArray {
    return this.addStoreForm.get('explosiveTypesAvailable') as FormArray;
  }

  get editExplosiveTypesArray(): FormArray {
    return this.editStoreForm.get('explosiveTypesAvailable') as FormArray;
  }

  private clearExplosiveTypesArray(formType: 'add' | 'edit'): void {
    const array = formType === 'add' ? this.addExplosiveTypesArray : this.editExplosiveTypesArray;
    while (array.length !== 0) {
      array.removeAt(0);
    }
  }

  private setExplosiveTypesArray(formType: 'add' | 'edit', explosiveTypes: ExplosiveType[]): void {
    this.clearExplosiveTypesArray(formType);
    const array = formType === 'add' ? this.addExplosiveTypesArray : this.editExplosiveTypesArray;
    explosiveTypes.forEach(type => {
      array.push(this.fb.control(type));
    });
  }

  onExplosiveTypeChange(type: ExplosiveType, checked: boolean, formType: 'add' | 'edit'): void {
    const array = formType === 'add' ? this.addExplosiveTypesArray : this.editExplosiveTypesArray;
    
    if (checked) {
      array.push(this.fb.control(type));
    } else {
      const index = array.controls.findIndex(control => control.value === type);
      if (index >= 0) {
        array.removeAt(index);
      }
    }
  }

  // Helper method for checkbox change events
  onCheckboxChange(event: Event, type: ExplosiveType, formType: 'add' | 'edit'): void {
    const target = event.target as HTMLInputElement;
    this.onExplosiveTypeChange(type, target.checked, formType);
  }

  isExplosiveTypeSelected(type: ExplosiveType, formType: 'add' | 'edit'): boolean {
    const array = formType === 'add' ? this.addExplosiveTypesArray : this.editExplosiveTypesArray;
    return array.controls.some(control => control.value === type);
  }

  // CRUD operations
  onAddStore(): void {
    if (this.addStoreForm.valid) {
      const request: CreateStoreRequest = this.addStoreForm.value;
      
      this.storeService.createStore(request)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (store) => {
            this.showSuccess('Store added successfully');
            this.loadStores();
            this.loadStatistics();
            setTimeout(() => this.closeAllModals(), 2000);
          },
          error: (error) => {
            this.showError('Failed to add store');
          }
        });
    } else {
      this.markFormGroupTouched(this.addStoreForm);
    }
  }

  onEditStore(): void {
    if (this.editStoreForm.valid) {
      const request: UpdateStoreRequest = this.editStoreForm.value;
      
      this.storeService.updateStore(request)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (store) => {
            this.showSuccess('Store updated successfully');
            this.loadStores();
            this.loadStatistics();
            setTimeout(() => this.closeAllModals(), 2000);
          },
          error: (error) => {
            this.showError('Failed to update store');
          }
        });
    } else {
      this.markFormGroupTouched(this.editStoreForm);
    }
  }

  onDeleteStore(): void {
    if (this.selectedStore) {
      this.storeService.deleteStore(this.selectedStore.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.showSuccess('Store deleted successfully');
            this.loadStores();
            this.loadStatistics();
            this.closeAllModals();
          },
          error: (error) => {
            this.showError('Failed to delete store');
          }
        });
    }
  }

  onDeactivateStore(): void {
    if (this.selectedStore) {
      this.storeService.deactivateStore(this.selectedStore.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.showSuccess('Store deactivated successfully');
            this.loadStores();
            this.loadStatistics();
            this.closeAllModals();
          },
          error: (error) => {
            this.showError('Failed to deactivate store');
          }
        });
    }
  }

  // Utility methods
  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  isFieldInvalid(form: FormGroup, fieldName: string): boolean {
    const field = form.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  // Updated method to handle nested form groups
  isNestedFieldInvalid(parentForm: FormGroup, parentFieldName: string, fieldName: string): boolean {
    const parentField = parentForm.get(parentFieldName) as FormGroup;
    if (!parentField) return false;
    const field = parentField.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(form: FormGroup, fieldName: string): string {
    const field = form.get(fieldName);
    if (field?.errors) {
      if (field.errors['required']) return `${this.getFieldDisplayName(fieldName)} is required`;
      if (field.errors['minlength']) return `${this.getFieldDisplayName(fieldName)} is too short`;
      if (field.errors['email']) return 'Please enter a valid email address';
      if (field.errors['pattern']) return 'Please enter a valid phone number';
      if (field.errors['min']) return 'Value must be greater than 0';
    }
    return '';
  }

  private getFieldDisplayName(fieldName: string): string {
    const displayNames: { [key: string]: string } = {
      storeName: 'Store Name',
      storeAddress: 'Store Address',
      storeManagerName: 'Store Manager Name',
      storeManagerContact: 'Store Manager Contact',
      storeManagerEmail: 'Store Manager Email',
      storeType: 'Store Type',
      explosiveTypesAvailable: 'Explosive Types',
      storageCapacity: 'Storage Capacity',
      storageCapacityUnit: 'Storage Capacity Unit',
      securityLevel: 'Security Level',
      city: 'City',
      region: 'Region',
      country: 'Country'
    };
    return displayNames[fieldName] || fieldName;
  }

  // Message methods
  private showSuccess(message: string): void {
    this.successMessage = message;
    this.errorMessage = '';
    setTimeout(() => this.clearMessages(), 5000);
  }

  private showError(message: string): void {
    this.errorMessage = message;
    this.error = message;
    this.successMessage = '';
    setTimeout(() => this.clearMessages(), 5000);
  }

  private clearMessages(): void {
    this.successMessage = '';
    this.errorMessage = '';
    this.error = null;
  }

  // Helper methods for template
  getStatusBadgeClass(status: StoreStatus): string {
    switch (status) {
      case StoreStatus.OPERATIONAL:
        return 'badge-success';
      case StoreStatus.UNDER_MAINTENANCE:
        return 'badge-warning';
      case StoreStatus.TEMPORARILY_CLOSED:
        return 'badge-secondary';
      case StoreStatus.INSPECTION_REQUIRED:
        return 'badge-info';
      case StoreStatus.DECOMMISSIONED:
        return 'badge-danger';
      default:
        return 'badge-secondary';
    }
  }

  getStatusClass(status: StoreStatus): string {
    switch (status) {
      case StoreStatus.OPERATIONAL:
        return 'status-active';
      case StoreStatus.UNDER_MAINTENANCE:
        return 'status-maintenance';
      case StoreStatus.TEMPORARILY_CLOSED:
        return 'status-inactive';
      case StoreStatus.INSPECTION_REQUIRED:
        return 'status-pending';
      case StoreStatus.DECOMMISSIONED:
        return 'status-cancelled';
      default:
        return 'status-inactive';
    }
  }

  getUtilizationPercentage(store: Store): number {
    return store.storageCapacity > 0 ? Math.round((store.currentOccupancy || 0) / store.storageCapacity * 100) : 0;
  }

  getUtilizationClass(percentage: number): string {
    if (percentage >= 90) return 'text-danger';
    if (percentage >= 75) return 'text-warning';
    return 'text-success';
  }

  get currentYear(): number {
    return new Date().getFullYear();
  }
}
