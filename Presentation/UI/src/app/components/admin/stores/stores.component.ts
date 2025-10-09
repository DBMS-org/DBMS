import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
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
import { UserService } from '../../../core/services/user.service';
import { User } from '../../../core/models/user.model';

@Component({
  selector: 'app-stores',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
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
  showViewStoreModal = false;
  showAssignManagerModal = false;

  // Forms
  assignManagerForm!: FormGroup;

  // Users list for manager assignment
  availableManagers: User[] = [];

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
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private storeService: StoreService,
    private userService: UserService
  ) {
    this.initializeForms();
  }

  ngOnInit(): void {
    this.loadStores();
    this.loadStatistics();
    this.loadUsers();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initializeForms(): void {
    this.assignManagerForm = this.fb.group({
      managerUserId: ['', Validators.required]
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

  private loadUsers(): void {
    this.userService.getUsers()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (users) => {
          // Filter for Store Manager role
          this.availableManagers = users.filter(user => user.role === 'StoreManager');
        },
        error: (error) => {
          console.error('Error loading users:', error);
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
      if (this.filters.status && this.filters.status !== 'ALL' && store.status !== this.filters.status) {
        return false;
      }

      if (this.filters.regionId && this.filters.regionId !== 'ALL' && store.regionId !== this.filters.regionId) {
        return false;
      }

      if (this.filters.city && this.filters.city !== 'ALL' && store.city !== this.filters.city) {
        return false;
      }

      if (this.filters.storeManager && this.filters.storeManager !== 'ALL' && store.managerUserName !== this.filters.storeManager) {
        return false;
      }

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

  onFilterChange(filters: StoreFilters): void {
    this.filters = filters;
    this.applyFilters();
  }

  onSearchChange(searchTerm: string): void {
    this.filters.searchTerm = searchTerm;
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
  openAssignManagerModal(store: Store): void {
    this.selectedStore = store;
    this.assignManagerForm.patchValue({
      managerUserId: store.managerUserId || ''
    });
    this.showAssignManagerModal = true;
  }

  openViewStoreModal(store: Store): void {
    this.selectedStore = store;
    this.showViewStoreModal = true;
  }

  closeAllModals(): void {
    this.showAssignManagerModal = false;
    this.showViewStoreModal = false;
    this.selectedStore = null;
    this.clearMessages();
  }

  // CRUD operations
  onAssignManager(): void {
    if (!this.selectedStore) {
      this.showError('No store selected');
      return;
    }

    if (this.assignManagerForm.invalid) {
      this.markFormGroupTouched(this.assignManagerForm);
      return;
    }

    const managerUserId = this.assignManagerForm.value.managerUserId;
    this.isLoading = true;
    this.clearMessages();

    this.storeService.assignManager(this.selectedStore.id, managerUserId, this.selectedStore)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (store) => {
          this.showSuccess('Manager assigned successfully');
          this.loadStores();
          this.loadStatistics();
          setTimeout(() => this.closeAllModals(), 2000);
        },
        error: (error) => {
          console.error('Error assigning manager:', error);
          const errorMsg = this.getErrorMessage(error, 'Failed to assign manager');
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

    if (error.error) {
      if (typeof error.error === 'string') {
        return error.error;
      }

      if (error.error.message) {
        return error.error.message;
      }

      if (error.error.errors) {
        const errors = error.error.errors;
        const firstErrorKey = Object.keys(errors)[0];
        if (firstErrorKey && errors[firstErrorKey]?.length > 0) {
          return errors[firstErrorKey][0];
        }
      }

      if (error.error.title) {
        return error.error.title;
      }
    }

    if (error.message) {
      return error.message;
    }

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
}
