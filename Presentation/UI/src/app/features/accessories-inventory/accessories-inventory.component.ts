import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { TagModule } from 'primeng/tag';
import { DialogModule } from 'primeng/dialog';
import { Textarea } from 'primeng/inputtextarea';
import { InputNumberModule } from 'primeng/inputnumber';
import { TooltipModule } from 'primeng/tooltip';
import { AccessoryService, Accessory as AccessoryDto } from '../../core/services/accessory.service';

// Interfaces
interface Accessory {
  id: number;
  name: string;
  category: string;
  partNumber: string;
  description?: string;
  quantity: number;
  unit: string;
  minStockLevel: number;
  supplier: string;
  location?: string;
  status?: string;
  createdAt: Date;
  updatedAt?: Date;
  lastUpdated?: Date; // For backward compatibility with template
}

interface AccessoryForm {
  name: string;
  category: string;
  partNumber: string;
  description: string;
  quantity: number;
  unit: string;
  minStockLevel: number;
  supplier: string;
  location: string;
}

interface StockAdjustment {
  type: 'add' | 'remove' | 'set';
  quantity: number;
  reason: string;
  notes: string;
}

interface InventoryStatistics {
  totalAvailable: number;
  lowStock: number;
  outOfStock: number;
}

@Component({
  selector: 'app-accessories-inventory',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    DropdownModule,
    TagModule,
    DialogModule,
    Textarea,
    InputNumberModule,
    TooltipModule
  ],
  templateUrl: './accessories-inventory.component.html',
  styleUrl: './accessories-inventory.component.scss'
})
export class AccessoriesInventoryComponent implements OnInit, OnDestroy {
  // Data properties
  accessories: Accessory[] = [];
  filteredAccessories: Accessory[] = [];

  // Filter properties
  searchTerm = '';
  selectedCategory: any = null;
  selectedStatus: any = null;
  selectedSupplier: any = null;
  // Filter expansion state
  isFilterExpanded: boolean = true;
  categoryOptions: any[] = [];
  supplierOptions: any[] = [];
  statusOptions: any[] = [
    { label: 'Available', value: 'available' },
    { label: 'Low Stock', value: 'low-stock' },
    { label: 'Out of Stock', value: 'out-of-stock' },
    { label: 'Discontinued', value: 'discontinued' }
  ];

  // PrimeNG Table properties
  first = 0;
  rows = 25;
  
  // UI state properties
  isLoading = false;
  isRefreshing = false;
  errorMessage = '';
  lastUpdated = new Date();
  
  // Statistics
  statistics: InventoryStatistics = {
    totalAvailable: 0,
    lowStock: 0,
    outOfStock: 0
  };
  
  // Modal properties
  showAccessoryModal = false;
  showStockModal = false;
  showDetailsModal = false;
  isEditMode = false;
  selectedAccessory: Accessory | null = null;
  
  // Form properties
  accessoryForm: AccessoryForm = {
    name: '',
    category: '',
    partNumber: '',
    description: '',
    quantity: 0,
    unit: '',
    minStockLevel: 0,
    supplier: '',
    location: ''
  };
  
  stockAdjustment: StockAdjustment = {
    type: 'add',
    quantity: 0,
    reason: 'purchase',
    notes: ''
  };
  
  private subscriptions: Subscription[] = [];

  constructor(private accessoryService: AccessoryService) {}

  ngOnInit(): void {
    this.loadAccessories();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  // Data loading methods
  private loadAccessories(): void {
    this.isLoading = true;
    this.errorMessage = '';

    console.log('Loading accessories from API...');

    const sub = this.accessoryService.getAccessories(
      this.searchTerm || undefined,
      this.selectedCategory || undefined,
      this.selectedSupplier || undefined,
      this.selectedStatus || undefined
    ).subscribe({
      next: (data) => {
        console.log('Accessories loaded successfully:', data);
        this.accessories = (data || [])
          .map(dto => this.mapDtoToAccessory(dto))
          .filter((acc): acc is Accessory => acc !== null);
        console.log('Mapped accessories:', this.accessories);
        this.extractFilterOptions();
        this.applyFilters();
        this.loadStatistics();
        this.lastUpdated = new Date();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading accessories:', error);
        console.error('Error details:', {
          status: error.status,
          statusText: error.statusText,
          message: error.message,
          error: error.error
        });
        this.errorMessage = 'Failed to load accessories inventory. Please try again.';
        this.isLoading = false;
      }
    });

    this.subscriptions.push(sub);
  }

  private loadStatistics(): void {
    const sub = this.accessoryService.getStatistics().subscribe({
      next: (stats) => {
        this.statistics = {
          totalAvailable: stats.totalAvailable,
          lowStock: stats.lowStock,
          outOfStock: stats.outOfStock
        };
      },
      error: (error) => {
        console.error('Error loading statistics:', error);
        // Use calculated statistics as fallback
        this.calculateStatistics();
      }
    });

    this.subscriptions.push(sub);
  }

  private mapDtoToAccessory(dto: AccessoryDto): Accessory | null {
    // Return null if dto is invalid
    if (!dto || !dto.id) {
      console.warn('Invalid DTO received:', dto);
      return null;
    }

    // Safely handle updatedAt - use createdAt as fallback
    const updatedDate = dto?.updatedAt ? new Date(dto.updatedAt) :
                       dto?.createdAt ? new Date(dto.createdAt) : new Date();
    return {
      id: dto.id,
      name: dto.name,
      category: dto.category,
      partNumber: dto.partNumber,
      description: dto.description,
      quantity: dto.quantity,
      unit: dto.unit,
      minStockLevel: dto.minStockLevel,
      supplier: dto.supplier,
      location: dto.location,
      status: dto.status,
      createdAt: new Date(dto.createdAt),
      updatedAt: updatedDate,
      lastUpdated: updatedDate // For backward compatibility with template
    };
  }

  // Mock data methods removed - now using real API

  private extractFilterOptions(): void {
    const categories = [...new Set(this.accessories.map(a => a.category))].sort();
    this.categoryOptions = categories.map(cat => ({ label: cat, value: cat }));

    const suppliers = [...new Set(this.accessories.map(a => a.supplier))].sort();
    this.supplierOptions = suppliers.map(sup => ({ label: sup, value: sup }));
  }

  private calculateStatistics(): void {
    this.statistics = {
      totalAvailable: this.accessories.filter(a => a.quantity > a.minStockLevel).length,
      lowStock: this.accessories.filter(a => a.quantity <= a.minStockLevel && a.quantity > 0).length,
      outOfStock: this.accessories.filter(a => a.quantity === 0).length
    };
  }

  // Filter methods
  onSearchChange(): void {
    this.applyFilters();
  }

  onCategoryFilterChange(): void {
    this.applyFilters();
  }

  onStatusFilterChange(): void {
    this.applyFilters();
  }

  onSupplierFilterChange(): void {
    this.applyFilters();
  }

  private applyFilters(): void {
    let filtered = [...this.accessories];

    // Apply search filter
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(accessory =>
        accessory.name.toLowerCase().includes(term) ||
        accessory.partNumber.toLowerCase().includes(term) ||
        accessory.category.toLowerCase().includes(term) ||
        accessory.supplier.toLowerCase().includes(term) ||
        (accessory.description && accessory.description.toLowerCase().includes(term))
      );
    }

    // Apply category filter
    if (this.selectedCategory) {
      filtered = filtered.filter(accessory => accessory.category === this.selectedCategory);
    }

    // Apply status filter
    if (this.selectedStatus) {
      filtered = filtered.filter(accessory => {
        switch (this.selectedStatus) {
          case 'available':
            return accessory.quantity > accessory.minStockLevel;
          case 'low-stock':
            return accessory.quantity <= accessory.minStockLevel && accessory.quantity > 0;
          case 'out-of-stock':
            return accessory.quantity === 0;
          case 'discontinued':
            return false; // Would need a discontinued flag in real implementation
          default:
            return true;
        }
      });
    }

    // Apply supplier filter
    if (this.selectedSupplier) {
      filtered = filtered.filter(accessory => accessory.supplier === this.selectedSupplier);
    }

    this.filteredAccessories = filtered;
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.applyFilters();
  }

  clearAllFilters(): void {
    this.searchTerm = '';
    this.selectedCategory = null;
    this.selectedStatus = null;
    this.selectedSupplier = null;
    this.applyFilters();
  }

  toggleFilterExpansion(): void {
    this.isFilterExpanded = !this.isFilterExpanded;
  }

  hasActiveFilters(): boolean {
    return !!(this.searchTerm || this.selectedCategory || this.selectedStatus || this.selectedSupplier);
  }

  // Accessory management methods
  openAddAccessoryModal(): void {
    this.isEditMode = false;
    this.selectedAccessory = null;
    this.resetAccessoryForm();
    this.showAccessoryModal = true;
  }

  editAccessory(accessory: Accessory): void {
    console.log('editAccessory called with:', accessory);

    // Close details modal first (before setting selectedAccessory)
    this.showDetailsModal = false;

    // Now set edit mode and selected accessory
    this.isEditMode = true;
    this.selectedAccessory = accessory;
    console.log('isEditMode set to:', this.isEditMode);
    console.log('selectedAccessory set to:', this.selectedAccessory);

    this.accessoryForm = {
      name: accessory.name,
      category: accessory.category,
      partNumber: accessory.partNumber,
      description: accessory.description || '',
      quantity: accessory.quantity,
      unit: accessory.unit,
      minStockLevel: accessory.minStockLevel,
      supplier: accessory.supplier,
      location: accessory.location || ''
    };
    this.showAccessoryModal = true;
  }

  saveAccessory(): void {
    console.log('saveAccessory called');
    console.log('isEditMode:', this.isEditMode);
    console.log('selectedAccessory:', this.selectedAccessory);

    if (!this.isAccessoryFormValid()) return;

    if (this.isEditMode && this.selectedAccessory) {
      // Update existing accessory
      const updateRequest = {
        name: this.accessoryForm.name,
        category: this.accessoryForm.category,
        partNumber: this.accessoryForm.partNumber,
        description: this.accessoryForm.description,
        unit: this.accessoryForm.unit,
        minStockLevel: this.accessoryForm.minStockLevel,
        supplier: this.accessoryForm.supplier,
        location: this.accessoryForm.location
      };

      console.log('Updating accessory ID:', this.selectedAccessory.id);
      console.log('Update request:', updateRequest);

      const sub = this.accessoryService.updateAccessory(this.selectedAccessory.id, updateRequest).subscribe({
        next: (updated) => {
          const index = this.accessories.findIndex(a => a.id === this.selectedAccessory!.id);
          if (index !== -1) {
            const updatedAccessory = this.mapDtoToAccessory(updated);
            if (updatedAccessory) {
              this.accessories[index] = updatedAccessory;
            }
          }
          this.extractFilterOptions();
          this.applyFilters();
          this.loadStatistics();
          this.closeAccessoryModal();
        },
        error: (error) => {
          console.error('Error updating accessory:', error);
          this.errorMessage = error.error?.message || 'Failed to update accessory. Please try again.';
        }
      });

      this.subscriptions.push(sub);
    } else {
      // Create new accessory
      const createRequest = {
        name: this.accessoryForm.name,
        category: this.accessoryForm.category,
        partNumber: this.accessoryForm.partNumber,
        description: this.accessoryForm.description,
        quantity: this.accessoryForm.quantity,
        unit: this.accessoryForm.unit,
        minStockLevel: this.accessoryForm.minStockLevel,
        supplier: this.accessoryForm.supplier,
        location: this.accessoryForm.location
      };

      const sub = this.accessoryService.createAccessory(createRequest).subscribe({
        next: (created) => {
          const newAccessory = this.mapDtoToAccessory(created);
          if (newAccessory) {
            this.accessories.push(newAccessory);
            this.extractFilterOptions();
            this.applyFilters();
            this.loadStatistics();
            this.closeAccessoryModal();
          }
        },
        error: (error) => {
          console.error('Error creating accessory:', error);
          // Handle different error types
          if (error.status === 409) {
            this.errorMessage = error.error?.message || 'An accessory with this part number already exists.';
          } else if (error.status === 400) {
            this.errorMessage = error.error?.message || 'Invalid accessory data. Please check your inputs.';
          } else if (error.status === 401 || error.status === 403) {
            this.errorMessage = 'You do not have permission to create accessories.';
          } else {
            this.errorMessage = error.error?.message || 'Failed to create accessory. Please try again.';
          }
        }
      });

      this.subscriptions.push(sub);
    }
  }

  deleteAccessory(accessory: Accessory): void {
    if (confirm(`Are you sure you want to delete "${accessory.name}"? This action cannot be undone.`)) {
      const sub = this.accessoryService.deleteAccessory(accessory.id).subscribe({
        next: () => {
          const index = this.accessories.findIndex(a => a.id === accessory.id);
          if (index !== -1) {
            this.accessories.splice(index, 1);
            this.extractFilterOptions();
            this.applyFilters();
            this.loadStatistics();
          }
        },
        error: (error) => {
          console.error('Error deleting accessory:', error);
          this.errorMessage = error.error?.message || 'Failed to delete accessory. Please try again.';
        }
      });

      this.subscriptions.push(sub);
    }
  }

  closeAccessoryModal(): void {
    this.showAccessoryModal = false;
    this.resetAccessoryForm();
  }

  private resetAccessoryForm(): void {
    this.accessoryForm = {
      name: '',
      category: '',
      partNumber: '',
      description: '',
      quantity: 0,
      unit: '',
      minStockLevel: 0,
      supplier: '',
      location: ''
    };
  }

  isAccessoryFormValid(): boolean {
    return !!(
      this.accessoryForm.name &&
      this.accessoryForm.category &&
      this.accessoryForm.partNumber &&
      this.accessoryForm.unit &&
      this.accessoryForm.supplier &&
      this.accessoryForm.quantity >= 0 &&
      this.accessoryForm.minStockLevel >= 0
    );
  }

  // Stock adjustment methods
  adjustStock(accessory: Accessory): void {
    this.selectedAccessory = accessory;
    this.stockAdjustment = {
      type: 'add',
      quantity: 0,
      reason: 'purchase',
      notes: ''
    };
    this.showStockModal = true;
  }

  calculateNewStock(): number {
    if (!this.selectedAccessory) return 0;
    
    switch (this.stockAdjustment.type) {
      case 'add':
        return this.selectedAccessory.quantity + this.stockAdjustment.quantity;
      case 'remove':
        return Math.max(0, this.selectedAccessory.quantity - this.stockAdjustment.quantity);
      case 'set':
        return this.stockAdjustment.quantity;
      default:
        return this.selectedAccessory.quantity;
    }
  }

  applyStockAdjustment(): void {
    if (!this.selectedAccessory || !this.stockAdjustment.quantity) return;

    const adjustmentRequest = {
      type: this.stockAdjustment.type.charAt(0).toUpperCase() + this.stockAdjustment.type.slice(1), // Capitalize first letter
      quantity: this.stockAdjustment.quantity,
      reason: this.stockAdjustment.reason.charAt(0).toUpperCase() + this.stockAdjustment.reason.slice(1),
      notes: this.stockAdjustment.notes
    };

    const sub = this.accessoryService.adjustStock(this.selectedAccessory.id, adjustmentRequest).subscribe({
      next: (updated) => {
        const index = this.accessories.findIndex(a => a.id === this.selectedAccessory!.id);
        if (index !== -1) {
          const updatedAccessory = this.mapDtoToAccessory(updated);
          if (updatedAccessory) {
            this.accessories[index] = updatedAccessory;
          }
        }
        this.applyFilters();
        this.loadStatistics();
        this.closeStockModal();
      },
      error: (error) => {
        console.error('Error adjusting stock:', error);
        this.errorMessage = error.error?.message || 'Failed to adjust stock. Please try again.';
      }
    });

    this.subscriptions.push(sub);
  }

  closeStockModal(): void {
    this.showStockModal = false;
    this.selectedAccessory = null;
  }

  // Details modal methods
  viewAccessoryDetails(accessory: Accessory): void {
    this.selectedAccessory = accessory;
    this.showDetailsModal = true;
  }

  closeDetailsModal(): void {
    this.showDetailsModal = false;
    this.selectedAccessory = null;
  }

  // Utility methods
  getStatusClass(accessory: Accessory): string {
    if (accessory.quantity === 0) {
      return 'status-out-of-stock';
    } else if (accessory.quantity <= accessory.minStockLevel) {
      return 'status-low-stock';
    } else {
      return 'status-available';
    }
  }

  getStatusDisplay(accessory: Accessory): string {
    return accessory.status || (accessory.quantity === 0 ? 'Out of Stock' :
           accessory.quantity <= accessory.minStockLevel ? 'Low Stock' : 'Available');
  }

  getStatusSeverity(accessory: Accessory): 'success' | 'warning' | 'danger' | 'info' {
    if (accessory.quantity === 0) {
      return 'danger';
    } else if (accessory.quantity <= accessory.minStockLevel) {
      return 'warning';
    } else {
      return 'success';
    }
  }

  trackByAccessoryId(index: number, accessory: Accessory): number {
    return accessory.id;
  }

  // Export and refresh methods
  exportInventory(): void {
    const sub = this.accessoryService.exportToCsv(
      this.searchTerm || undefined,
      this.selectedCategory || undefined,
      this.selectedSupplier || undefined,
      this.selectedStatus || undefined
    ).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `accessories-inventory-${new Date().toISOString().split('T')[0]}.csv`;
        link.click();
        window.URL.revokeObjectURL(url);
      },
      error: (error) => {
        console.error('Error exporting inventory:', error);
        this.errorMessage = 'Failed to export inventory. Please try again.';
      }
    });

    this.subscriptions.push(sub);
  }

  refreshInventory(): void {
    this.isRefreshing = true;
    this.loadAccessories();
    // Set isRefreshing to false after a short delay for visual feedback
    setTimeout(() => {
      this.isRefreshing = false;
    }, 500);
  }
}