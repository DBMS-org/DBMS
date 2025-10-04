import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { StoreStatisticsComponent } from './store-statistics/store-statistics.component';
import { StoreFiltersComponent } from './store-filters/store-filters.component';
import { StoreListComponent } from './store-list/store-list.component';
import { StoreFormComponent } from './store-form/store-form.component';
import { Subject, takeUntil } from 'rxjs';
import { 
  Store, 
  StoreStatistics, 
  StoreFilters, 
  CreateStoreRequest, 
  UpdateStoreRequest,
  ExplosiveType,
  StoreStatus
} from '../../../core/models/store.model';
import { StoreService } from '../../../core/services/store.service';

@Component({
  selector: 'app-stores',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    ReactiveFormsModule,
    StoreStatisticsComponent,
    StoreFiltersComponent,
    StoreListComponent,
    StoreFormComponent
  ],
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
    regionId: 'ALL',
    city: 'ALL',
    storeManager: 'ALL',
    searchTerm: ''
  };

  // Enums for templates
  ExplosiveType = ExplosiveType;
  StoreStatus = StoreStatus;

  // Enum arrays for dropdowns
  explosiveTypes = Object.values(ExplosiveType).filter(value => typeof value === 'string') as string[];
  storeStatuses = Object.values(StoreStatus).filter(value => typeof value === 'string') as string[];

  // Unique values for filters
  uniqueLocations: string[] = [];
  uniqueManagers: string[] = [];

  // Success/Error messages
  successMessage = '';
  errorMessage = '';

  // Additional properties for project consistency
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private storeService: StoreService
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

   private initializeForms(): void {
    this.addStoreForm = this.fb.group({
      storeName: ['', [Validators.required, Validators.minLength(3)]],
      storeAddress: ['', [Validators.required, Validators.minLength(10)]],
      storageCapacity: ['', [Validators.required, Validators.min(1)]],
      city: ['', Validators.required],
      regionId: ['', Validators.required],
      managerUserId: ['']
    });

    this.editStoreForm = this.fb.group({
      storeName: ['', [Validators.required, Validators.minLength(3)]],
      storeAddress: ['', [Validators.required, Validators.minLength(10)]],
      storageCapacity: ['', [Validators.required, Validators.min(1)]],
      city: ['', Validators.required],
      status: ['', Validators.required],
      managerUserId: ['']
    });
  }

  // Data loading methods
  private loadStores(): void {
    this.isLoading = true;
    this.error = null;
    this.clearMessages();

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
          console.error('Error loading stores:', error);
          const errorMsg = this.getErrorMessage(error);
          this.error = errorMsg;
          this.showError(errorMsg);
          this.stores = [];
          this.filteredStores = [];
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
          console.error('Error loading statistics:', error);
          // Use default statistics if API fails - silent fallback
          this.statistics = {
            totalStores: 0,
            activeStores: 0,
            inactiveStores: 0,
            operationalStores: 0,
            maintenanceStores: 0,
            totalCapacity: 0,
            totalOccupancy: 0,
            utilizationRate: 0,
            storesByRegion: {}
          };
        }
      });
  }

  private updateUniqueValues(): void {
    this.uniqueLocations = Array.from(new Set(this.stores.map(s => s.city)));
    this.uniqueManagers = Array.from(new Set(this.stores.map(s => s.managerUserName).filter((name): name is string => !!name)));
  }

  // Filter and search methods
  applyFilters(): void {
    this.filteredStores = this.stores.filter(store => {
      // Status filter
      if (this.filters.status && this.filters.status !== 'ALL' && store.status !== this.filters.status) {
        return false;
      }

      // Location filter (using regionId and city)
      if (this.filters.regionId && this.filters.regionId !== 'ALL' && store.regionId !== this.filters.regionId) {
        return false;
      }

      if (this.filters.city && this.filters.city !== 'ALL' && store.city !== this.filters.city) {
        return false;
      }

      // Store manager filter
      if (this.filters.storeManager && this.filters.storeManager !== 'ALL' && store.managerUserName !== this.filters.storeManager) {
        return false;
      }

      // Search term filter
      if (this.filters.searchTerm && this.filters.searchTerm.trim()) {
        const searchTerm = this.filters.searchTerm.toLowerCase();
        const searchableText = [
          store.storeName,
          store.managerUserName,
          store.city,
          store.storeAddress,
          store.status?.toString()
        ].join(' ').toLowerCase();

        if (!searchableText.includes(searchTerm)) {
          return false;
        }
      }

      return true;
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
      regionId: 'ALL',
      city: 'ALL',
      storeManager: 'ALL',
      searchTerm: ''
    };
    this.applyFilters();
  }

  // Modal methods
  openAddStoreModal(): void {
    this.addStoreForm.reset();
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
      storeName: store.storeName,
      storeAddress: store.storeAddress,
      storageCapacity: store.storageCapacity,
      city: store.city,
      status: store.status,
      managerUserId: store.managerUserId
    });
  }

  // CRUD operations
  onAddStore(formData?: any): void {
    const request: CreateStoreRequest = formData || this.addStoreForm.value;

    this.isLoading = true;
    this.clearMessages();

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
          console.error('Error adding store:', error);
          const errorMsg = this.getErrorMessage(error, 'Failed to add store');
          this.showError(errorMsg);
          this.isLoading = false;
        },
        complete: () => {
          this.isLoading = false;
        }
      });
  }

  onEditStore(formData?: any): void {
    if (!this.selectedStore) {
      this.showError('No store selected for editing');
      return;
    }

    const request: UpdateStoreRequest = formData || this.editStoreForm.value;

    this.isLoading = true;
    this.clearMessages();

    this.storeService.updateStore(this.selectedStore.id, request)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (store) => {
          this.showSuccess('Store updated successfully');
          this.loadStores();
          this.loadStatistics();
          setTimeout(() => this.closeAllModals(), 2000);
        },
        error: (error) => {
          console.error('Error updating store:', error);
          const errorMsg = this.getErrorMessage(error, 'Failed to update store');
          this.showError(errorMsg);
          this.isLoading = false;
        },
        complete: () => {
          this.isLoading = false;
        }
      });
  }

  onDeleteStore(): void {
    if (!this.selectedStore) {
      this.showError('No store selected for deletion');
      return;
    }

    this.isLoading = true;
    this.clearMessages();

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
          console.error('Error deleting store:', error);
          const errorMsg = this.getErrorMessage(error, 'Failed to delete store');
          this.showError(errorMsg);
          this.isLoading = false;
        },
        complete: () => {
          this.isLoading = false;
        }
      });
  }

  onDeactivateStore(): void {
    if (!this.selectedStore) {
      this.showError('No store selected for deactivation');
      return;
    }

    const request: UpdateStoreRequest = {
      storeName: this.selectedStore.storeName,
      storeAddress: this.selectedStore.storeAddress,
      storageCapacity: this.selectedStore.storageCapacity,
      city: this.selectedStore.city,
      status: StoreStatus.TemporarilyClosed,
      managerUserId: this.selectedStore.managerUserId
    };

    this.isLoading = true;
    this.clearMessages();

    this.storeService.updateStore(this.selectedStore.id, request)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (store) => {
          this.showSuccess('Store deactivated successfully');
          this.loadStores();
          this.loadStatistics();
          this.closeAllModals();
        },
        error: (error) => {
          console.error('Error deactivating store:', error);
          const errorMsg = this.getErrorMessage(error, 'Failed to deactivate store');
          this.showError(errorMsg);
          this.isLoading = false;
        },
        complete: () => {
          this.isLoading = false;
        }
      });
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
      storageCapacity: 'Storage Capacity',
      city: 'City',
      regionId: 'Region',
      managerUserId: 'Manager',
      status: 'Status'
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

  private getErrorMessage(error: any, defaultMessage: string = 'An error occurred'): string {
    if (!error) return defaultMessage;

    // Handle HTTP error response
    if (error.error) {
      // If error.error is a string
      if (typeof error.error === 'string') {
        return error.error;
      }

      // If error.error has a message property
      if (error.error.message) {
        return error.error.message;
      }

      // If error.error has errors property (validation errors)
      if (error.error.errors) {
        const errors = error.error.errors;
        const firstErrorKey = Object.keys(errors)[0];
        if (firstErrorKey && errors[firstErrorKey]?.length > 0) {
          return errors[firstErrorKey][0];
        }
      }

      // If error.error has title property
      if (error.error.title) {
        return error.error.title;
      }
    }

    // Handle error message directly
    if (error.message) {
      return error.message;
    }

    // Handle status codes
    if (error.status) {
      switch (error.status) {
        case 400:
          return 'Invalid request. Please check your input.';
        case 401:
          return 'Unauthorized. Please log in again.';
        case 403:
          return 'You do not have permission to perform this action.';
        case 404:
          return 'Store not found.';
        case 409:
          return 'A conflict occurred. The store may already exist.';
        case 500:
          return 'Server error. Please try again later.';
        case 0:
          return 'Network error. Please check your connection.';
        default:
          return `${defaultMessage} (Error ${error.status})`;
      }
    }

    return defaultMessage;
  }

  // Helper methods for template
  getStatusBadgeClass(status: StoreStatus): string {
    switch (status) {
      case StoreStatus.Operational:
        return 'badge-success';
      case StoreStatus.UnderMaintenance:
        return 'badge-warning';
      case StoreStatus.TemporarilyClosed:
        return 'badge-secondary';
      case StoreStatus.InspectionRequired:
        return 'badge-info';
      case StoreStatus.Decommissioned:
        return 'badge-danger';
      default:
        return 'badge-secondary';
    }
  }

  getStatusClass(status: StoreStatus): string {
    switch (status) {
      case StoreStatus.Operational:
        return 'status-active';
      case StoreStatus.UnderMaintenance:
        return 'status-maintenance';
      case StoreStatus.TemporarilyClosed:
        return 'status-inactive';
      case StoreStatus.InspectionRequired:
        return 'status-pending';
      case StoreStatus.Decommissioned:
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

  private generateId(): string {
    return 'store_' + Math.random().toString(36).substr(2, 9);
  }
}
