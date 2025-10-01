import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { StoreStatisticsComponent } from './store-statistics/store-statistics.component';
import { StoreFiltersComponent } from './store-filters/store-filters.component';
import { StoreListComponent } from './store-list/store-list.component';
import { StoreFormComponent } from './store-form/store-form.component';
import { Subject } from 'rxjs';
import { 
  Store, 
  StoreStatistics, 
  StoreFilters, 
  CreateStoreRequest, 
  UpdateStoreRequest,
  ExplosiveType,
  StoreStatus
} from '../../../core/models/store.model';

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
    location: 'ALL',
    storeManager: 'ALL',
    isActive: null,
    searchTerm: ''
  };

  // Enums for templates
  ExplosiveType = ExplosiveType;
  StoreStatus = StoreStatus;

  // Enum arrays for dropdowns
  explosiveTypes = Object.values(ExplosiveType);
  storeStatuses = Object.values(StoreStatus);

  // Unique values for filters
  uniqueLocations: string[] = [];
  uniqueManagers: string[] = [];

  // Success/Error messages
  successMessage = '';
  errorMessage = '';

  // Additional properties for project consistency
  error: string | null = null;

  // Mock data
  private mockStores: Store[] = [];
  private mockStatistics: StoreStatistics = {
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

  constructor(
    private fb: FormBuilder
  ) {
    this.initializeForms();
    this.initializeMockData();
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
  private initializeMockData(): void {
    this.mockStores = [
      {
        id: '1',
        storeName: 'Central Explosives Depot',
        storeAddress: '123 Industrial Ave, Mining District',
        storeManagerName: 'John Smith',
        storeManagerContact: '+1-555-0101',
        storeManagerEmail: 'john.smith@company.com',
        explosiveTypesAvailable: [ExplosiveType.ANFO, ExplosiveType.EMULSION, ExplosiveType.BLASTING_CAPS],
        storageCapacity: 500,
        currentOccupancy: 350,
        location: {
          city: 'Denver',
          region: 'Colorado'
        },
        status: StoreStatus.OPERATIONAL,
        isActive: true
      },
      {
        id: '2',
        storeName: 'North Site Storage',
        storeAddress: '456 Mining Road, North Sector',
        storeManagerName: 'Sarah Johnson',
        storeManagerContact: '+1-555-0102',
        storeManagerEmail: 'sarah.johnson@company.com',
        explosiveTypesAvailable: [ExplosiveType.ANFO, ExplosiveType.BOOSTER],
        storageCapacity: 200,
        currentOccupancy: 120,
        location: {
          city: 'Boulder',
          region: 'Colorado'
        },
        status: StoreStatus.OPERATIONAL,
        isActive: true
      },
      {
        id: '3',
        storeName: 'Emergency Storage Unit',
        storeAddress: '789 Safety Blvd, Emergency Zone',
        storeManagerName: 'Mike Wilson',
        storeManagerContact: '+1-555-0103',
        storeManagerEmail: 'mike.wilson@company.com',
        explosiveTypesAvailable: [ExplosiveType.BLASTING_CAPS, ExplosiveType.SHAPED_CHARGES],
        storageCapacity: 50,
        currentOccupancy: 25,
        location: {
          city: 'Aurora',
          region: 'Colorado'
        },
        status: StoreStatus.OPERATIONAL,
        isActive: true
      },
      {
        id: '4',
        storeName: 'West Wing Depot',
        storeAddress: '321 West Mining St, Industrial Park',
        storeManagerName: 'Lisa Brown',
        storeManagerContact: '+1-555-0104',
        storeManagerEmail: 'lisa.brown@company.com',
        explosiveTypesAvailable: [ExplosiveType.EMULSION, ExplosiveType.ANFO],
        storageCapacity: 300,
        currentOccupancy: 180,
        location: {
          city: 'Lakewood',
          region: 'Colorado'
        },
        status: StoreStatus.UNDER_MAINTENANCE,
        isActive: true
      },
      {
        id: '5',
        storeName: 'Temporary Field Store',
        storeAddress: '654 Temporary Access Road, Field Site',
        storeManagerName: 'David Garcia',
        storeManagerContact: '+1-555-0105',
        storeManagerEmail: 'david.garcia@company.com',
        explosiveTypesAvailable: [ExplosiveType.ANFO],
        storageCapacity: 100,
        currentOccupancy: 45,
        location: {
          city: 'Westminster',
          region: 'Colorado'
        },
        status: StoreStatus.TEMPORARILY_CLOSED,
        isActive: false
      }
    ];

    // Calculate mock statistics
    this.mockStatistics = {
      totalStores: this.mockStores.length,
      activeStores: this.mockStores.filter(s => s.isActive).length,
      inactiveStores: this.mockStores.filter(s => !s.isActive).length,
      operationalStores: this.mockStores.filter(s => s.status === StoreStatus.OPERATIONAL).length,
      maintenanceStores: this.mockStores.filter(s => s.status === StoreStatus.UNDER_MAINTENANCE).length,
      totalCapacity: this.mockStores.reduce((sum, s) => sum + s.storageCapacity, 0),
      totalOccupancy: this.mockStores.reduce((sum, s) => sum + (s.currentOccupancy || 0), 0),
      utilizationRate: 0,
      storesByRegion: {}
    };

    // Calculate utilization rate
    this.mockStatistics.utilizationRate = this.mockStatistics.totalCapacity > 0 
      ? (this.mockStatistics.totalOccupancy / this.mockStatistics.totalCapacity) * 100 
      : 0;

    // Store type statistics removed

    // Calculate stores by region
    this.mockStores.forEach(store => {
      this.mockStatistics.storesByRegion[store.location.city] = 
        (this.mockStatistics.storesByRegion[store.location.city] || 0) + 1;
    });
   }

   private updateMockStatistics(): void {
     // Recalculate mock statistics
     this.mockStatistics = {
       totalStores: this.mockStores.length,
       activeStores: this.mockStores.filter(s => s.isActive).length,
       inactiveStores: this.mockStores.filter(s => !s.isActive).length,
       operationalStores: this.mockStores.filter(s => s.status === StoreStatus.OPERATIONAL).length,
       maintenanceStores: this.mockStores.filter(s => s.status === StoreStatus.UNDER_MAINTENANCE).length,
       totalCapacity: this.mockStores.reduce((sum, s) => sum + s.storageCapacity, 0),
       totalOccupancy: this.mockStores.reduce((sum, s) => sum + (s.currentOccupancy || 0), 0),
       utilizationRate: 0,
       storesByRegion: {}
     };

     // Calculate utilization rate
     this.mockStatistics.utilizationRate = this.mockStatistics.totalCapacity > 0 
       ? (this.mockStatistics.totalOccupancy / this.mockStatistics.totalCapacity) * 100 
       : 0;

     // Store type statistics removed

     // Calculate stores by region
     this.mockStores.forEach(store => {
       this.mockStatistics.storesByRegion[store.location.city] = 
         (this.mockStatistics.storesByRegion[store.location.city] || 0) + 1;
     });
   }

   private initializeForms(): void {
    this.addStoreForm = this.fb.group({
      storeName: ['', [Validators.required, Validators.minLength(3)]],
      storeAddress: ['', [Validators.required, Validators.minLength(10)]],
      storeManagerName: ['', [Validators.required, Validators.minLength(2)]],
      storeManagerContact: ['', [Validators.required, Validators.pattern(/^\+?[\d\s\-\(\)]{10,}$/)]],
      storeManagerEmail: ['', [Validators.email]],
      explosiveTypesAvailable: this.fb.array([], Validators.required),
      storageCapacity: ['', [Validators.required, Validators.min(1)]],
      location: this.fb.group({
        city: ['', Validators.required],
        region: ['', Validators.required]
      })
    });

    this.editStoreForm = this.fb.group({
      id: ['', Validators.required],
      storeName: ['', [Validators.required, Validators.minLength(3)]],
      storeAddress: ['', [Validators.required, Validators.minLength(10)]],
      storeManagerName: ['', [Validators.required, Validators.minLength(2)]],
      storeManagerContact: ['', [Validators.required, Validators.pattern(/^\+?[\d\s\-\(\)]{10,}$/)]],
      storeManagerEmail: ['', [Validators.email]],
      explosiveTypesAvailable: this.fb.array([], Validators.required),
      storageCapacity: ['', [Validators.required, Validators.min(1)]],
      location: this.fb.group({
        city: ['', Validators.required],
        region: ['', Validators.required]
      })
    });
  }

  // Data loading methods
  private loadStores(): void {
    this.isLoading = true;
    // Simulate loading delay
    setTimeout(() => {
      this.stores = [...this.mockStores];
      this.applyFilters();
      this.updateUniqueValues();
      this.isLoading = false;
    }, 500);
  }

  private loadStatistics(): void {
    // Use mock statistics
    this.statistics = { ...this.mockStatistics };
  }

  private updateUniqueValues(): void {
    this.uniqueLocations = Array.from(new Set(this.stores.map(s => s.location.city)));
    this.uniqueManagers = Array.from(new Set(this.stores.map(s => s.storeManagerName)));
  }

  // Filter and search methods
  applyFilters(): void {
    this.filteredStores = this.stores.filter(store => {
      // Status filter
      if (this.filters.status && this.filters.status !== 'ALL' && store.status !== this.filters.status) {
        return false;
      }

      // Store type filter removed

      // Location filter
      if (this.filters.location && this.filters.location !== 'ALL' && store.location.city !== this.filters.location) {
        return false;
      }

      // Store manager filter
      if (this.filters.storeManager && this.filters.storeManager !== 'ALL' && store.storeManagerName !== this.filters.storeManager) {
        return false;
      }

      // Active status filter
      if (this.filters.isActive !== null && store.isActive !== this.filters.isActive) {
        return false;
      }

      // Search term filter
      if (this.filters.searchTerm && this.filters.searchTerm.trim()) {
        const searchTerm = this.filters.searchTerm.toLowerCase();
        const searchableText = [
          store.storeName,
          store.storeManagerName,
          store.location.city,
          store.storeAddress,
          store.status
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
      storeName: store.storeName,
      storeAddress: store.storeAddress,
      storeManagerName: store.storeManagerName,
      storeManagerContact: store.storeManagerContact,
      storeManagerEmail: store.storeManagerEmail,
      storageCapacity: store.storageCapacity,
      location: {
        city: store.location.city,
        region: store.location.region
      },
      status: store.status
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
  onAddStore(formData?: any): void {
    const request: CreateStoreRequest = formData || this.addStoreForm.value;
    
    // Create new store with mock data
    const newStore: Store = {
      id: this.generateId(),
      ...request,
      currentOccupancy: 0,
      isActive: true
    };
    
    // Add to mock stores
    this.mockStores.push(newStore);
    
    // Update statistics
    this.updateMockStatistics();
    
    this.showSuccess('Store added successfully');
    this.loadStores();
    this.loadStatistics();
    setTimeout(() => this.closeAllModals(), 2000);
  }

  onEditStore(formData?: any): void {
    if (this.selectedStore) {
      const request: UpdateStoreRequest = formData || this.editStoreForm.value;
      
      // Find and update store in mock data
      const storeIndex = this.mockStores.findIndex(s => s.id === this.selectedStore!.id);
      if (storeIndex !== -1) {
        this.mockStores[storeIndex] = {
          ...this.mockStores[storeIndex],
          ...request
        };
        
        // Update statistics
        this.updateMockStatistics();
        
        this.showSuccess('Store updated successfully');
        this.loadStores();
        this.loadStatistics();
        setTimeout(() => this.closeAllModals(), 2000);
      } else {
        this.showError('Store not found');
      }
    }
  }

  onDeleteStore(): void {
    if (this.selectedStore) {
      // Remove store from mock data
      const storeIndex = this.mockStores.findIndex(s => s.id === this.selectedStore!.id);
      if (storeIndex !== -1) {
        this.mockStores.splice(storeIndex, 1);
        
        // Update statistics
        this.updateMockStatistics();
        
        this.showSuccess('Store deleted successfully');
        this.loadStores();
        this.loadStatistics();
        this.closeAllModals();
      } else {
        this.showError('Store not found');
      }
    }
  }

  onDeactivateStore(): void {
    if (this.selectedStore) {
      // Deactivate store in mock data
      const storeIndex = this.mockStores.findIndex(s => s.id === this.selectedStore!.id);
      if (storeIndex !== -1) {
        this.mockStores[storeIndex] = {
          ...this.mockStores[storeIndex],
          isActive: false,
          status: StoreStatus.TEMPORARILY_CLOSED
        };
        
        // Update statistics
        this.updateMockStatistics();
        
        this.showSuccess('Store deactivated successfully');
        this.loadStores();
        this.loadStatistics();
        this.closeAllModals();
      } else {
        this.showError('Store not found');
      }
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
      explosiveTypesAvailable: 'Explosive Types',
      storageCapacity: 'Storage Capacity',
      city: 'City',
      region: 'Region'
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

  private generateId(): string {
    return 'store_' + Math.random().toString(36).substr(2, 9);
  }
}
