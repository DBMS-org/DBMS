import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';

// Interfaces
interface Accessory {
  id: string;
  name: string;
  category: string;
  partNumber: string;
  description?: string;
  quantity: number;
  unit: string;
  minStockLevel: number;
  unitPrice: number;
  supplier: string;
  location?: string;
  createdAt: Date;
  lastUpdated: Date;
}

interface AccessoryForm {
  name: string;
  category: string;
  partNumber: string;
  description: string;
  quantity: number;
  unit: string;
  minStockLevel: number;
  unitPrice: number;
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
  totalValue: number;
}

@Component({
  selector: 'app-accessories-inventory',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './accessories-inventory.component.html',
  styleUrl: './accessories-inventory.component.scss'
})
export class AccessoriesInventoryComponent implements OnInit, OnDestroy {
  // Data properties
  accessories: Accessory[] = [];
  filteredAccessories: Accessory[] = [];
  paginatedAccessories: Accessory[] = [];
  
  // Filter properties
  searchTerm = '';
  selectedCategory = '';
  selectedStatus = '';
  selectedSupplier = '';
  categoryOptions: string[] = [];
  supplierOptions: string[] = [];
  
  // Sorting properties
  sortField = 'name';
  sortDirection: 'asc' | 'desc' = 'asc';
  
  // Pagination properties
  currentPage = 1;
  pageSize = 25;
  totalPages = 0;
  
  // UI state properties
  isLoading = false;
  isRefreshing = false;
  errorMessage = '';
  lastUpdated = new Date();
  
  // Statistics
  statistics: InventoryStatistics = {
    totalAvailable: 0,
    lowStock: 0,
    outOfStock: 0,
    totalValue: 0
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
    unitPrice: 0,
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

  constructor() {}

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
    
    // Simulate API call with mock data
    setTimeout(() => {
      try {
        this.accessories = this.generateMockAccessories();
        this.extractFilterOptions();
        this.applyFilters();
        this.calculateStatistics();
        this.lastUpdated = new Date();
        this.isLoading = false;
      } catch (error) {
        this.errorMessage = 'Failed to load accessories inventory. Please try again.';
        this.isLoading = false;
      }
    }, 1000);
  }

  private generateMockAccessories(): Accessory[] {
    const categories = ['Engine Parts', 'Hydraulic Parts', 'Electrical Components', 'Filters', 'Belts & Hoses', 'Lubricants', 'Safety Equipment', 'Tools'];
    const suppliers = ['CAT Parts Inc.', 'Hydraulic Solutions', 'ElectroTech', 'FilterMax', 'Industrial Supply Co.', 'SafetyFirst', 'ToolMaster'];
    const units = ['pcs', 'kg', 'ltr', 'm', 'box', 'set'];
    
    const accessories: Accessory[] = [];
    
    for (let i = 1; i <= 50; i++) {
      const category = categories[Math.floor(Math.random() * categories.length)];
      const supplier = suppliers[Math.floor(Math.random() * suppliers.length)];
      const unit = units[Math.floor(Math.random() * units.length)];
      const quantity = Math.floor(Math.random() * 100) + 1;
      const minStock = Math.floor(Math.random() * 20) + 5;
      
      accessories.push({
        id: `ACC-${i.toString().padStart(3, '0')}`,
        name: this.generateAccessoryName(category),
        category,
        partNumber: `PN-${Math.random().toString(36).substr(2, 8).toUpperCase()}`,
        description: `High-quality ${category.toLowerCase()} for industrial machinery`,
        quantity,
        unit,
        minStockLevel: minStock,
        unitPrice: Math.round((Math.random() * 500 + 10) * 100) / 100,
        supplier,
        location: `Warehouse ${String.fromCharCode(65 + Math.floor(Math.random() * 5))}-${Math.floor(Math.random() * 20) + 1}`,
        createdAt: new Date(Date.now() - Math.random() * 365 * 24 * 60 * 60 * 1000),
        lastUpdated: new Date(Date.now() - Math.random() * 30 * 24 * 60 * 60 * 1000)
      });
    }
    
    return accessories;
  }

  private generateAccessoryName(category: string): string {
    const names: { [key: string]: string[] } = {
      'Engine Parts': ['Oil Filter', 'Air Filter', 'Fuel Filter', 'Spark Plug', 'Piston Ring', 'Gasket Set', 'Valve Cover'],
      'Hydraulic Parts': ['Hydraulic Pump', 'Cylinder Seal', 'Hydraulic Hose', 'Pressure Valve', 'Flow Control Valve'],
      'Electrical Components': ['Alternator', 'Starter Motor', 'Wiring Harness', 'Relay Switch', 'Fuse Set', 'Battery'],
      'Filters': ['Oil Filter', 'Air Filter', 'Fuel Filter', 'Hydraulic Filter', 'Cabin Filter'],
      'Belts & Hoses': ['Drive Belt', 'Timing Belt', 'Radiator Hose', 'Fuel Hose', 'Hydraulic Hose'],
      'Lubricants': ['Engine Oil', 'Hydraulic Oil', 'Gear Oil', 'Grease', 'Coolant'],
      'Safety Equipment': ['Safety Helmet', 'Safety Vest', 'Work Gloves', 'Safety Glasses', 'First Aid Kit'],
      'Tools': ['Wrench Set', 'Socket Set', 'Screwdriver Set', 'Multimeter', 'Torque Wrench']
    };
    
    const categoryNames = names[category] || ['Generic Part'];
    return categoryNames[Math.floor(Math.random() * categoryNames.length)];
  }

  private extractFilterOptions(): void {
    this.categoryOptions = [...new Set(this.accessories.map(a => a.category))].sort();
    this.supplierOptions = [...new Set(this.accessories.map(a => a.supplier))].sort();
  }

  private calculateStatistics(): void {
    this.statistics = {
      totalAvailable: this.accessories.filter(a => a.quantity > a.minStockLevel).length,
      lowStock: this.accessories.filter(a => a.quantity <= a.minStockLevel && a.quantity > 0).length,
      outOfStock: this.accessories.filter(a => a.quantity === 0).length,
      totalValue: this.accessories.reduce((sum, a) => sum + (a.quantity * a.unitPrice), 0)
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
    this.applySorting();
    this.updatePagination();
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.applyFilters();
  }

  clearAllFilters(): void {
    this.searchTerm = '';
    this.selectedCategory = '';
    this.selectedStatus = '';
    this.selectedSupplier = '';
    this.applyFilters();
  }

  hasActiveFilters(): boolean {
    return !!(this.searchTerm || this.selectedCategory || this.selectedStatus || this.selectedSupplier);
  }

  // Sorting methods
  sortBy(field: keyof Accessory): void {
    if (this.sortField === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortField = field;
      this.sortDirection = 'asc';
    }
    this.applySorting();
    this.updatePagination();
  }

  private applySorting(): void {
    this.filteredAccessories.sort((a, b) => {
      let aValue = a[this.sortField as keyof Accessory];
      let bValue = b[this.sortField as keyof Accessory];
      
      // Handle different data types
      if (aValue instanceof Date && bValue instanceof Date) {
        aValue = aValue.getTime();
        bValue = bValue.getTime();
      } else if (typeof aValue === 'string' && typeof bValue === 'string') {
        aValue = aValue.toLowerCase();
        bValue = bValue.toLowerCase();
      }
      
      if (aValue < bValue) {
        return this.sortDirection === 'asc' ? -1 : 1;
      }
      if (aValue > bValue) {
        return this.sortDirection === 'asc' ? 1 : -1;
      }
      return 0;
    });
  }

  // Pagination methods
  private updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredAccessories.length / this.pageSize);
    if (this.currentPage > this.totalPages) {
      this.currentPage = Math.max(1, this.totalPages);
    }
    this.updatePaginatedAccessories();
  }

  private updatePaginatedAccessories(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedAccessories = this.filteredAccessories.slice(startIndex, endIndex);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePaginatedAccessories();
    }
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.updatePagination();
  }

  getStartIndex(): number {
    return (this.currentPage - 1) * this.pageSize;
  }

  getEndIndex(): number {
    return Math.min(this.getStartIndex() + this.pageSize, this.filteredAccessories.length);
  }

  getVisiblePages(): number[] {
    const pages: number[] = [];
    const maxVisible = 5;
    let start = Math.max(1, this.currentPage - Math.floor(maxVisible / 2));
    let end = Math.min(this.totalPages, start + maxVisible - 1);
    
    if (end - start + 1 < maxVisible) {
      start = Math.max(1, end - maxVisible + 1);
    }
    
    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    
    return pages;
  }

  // Accessory management methods
  openAddAccessoryModal(): void {
    this.isEditMode = false;
    this.selectedAccessory = null;
    this.resetAccessoryForm();
    this.showAccessoryModal = true;
  }

  editAccessory(accessory: Accessory): void {
    this.isEditMode = true;
    this.selectedAccessory = accessory;
    this.accessoryForm = {
      name: accessory.name,
      category: accessory.category,
      partNumber: accessory.partNumber,
      description: accessory.description || '',
      quantity: accessory.quantity,
      unit: accessory.unit,
      minStockLevel: accessory.minStockLevel,
      unitPrice: accessory.unitPrice,
      supplier: accessory.supplier,
      location: accessory.location || ''
    };
    this.showAccessoryModal = true;
    this.closeDetailsModal();
  }

  saveAccessory(): void {
    if (!this.isAccessoryFormValid()) return;
    
    if (this.isEditMode && this.selectedAccessory) {
      // Update existing accessory
      const index = this.accessories.findIndex(a => a.id === this.selectedAccessory!.id);
      if (index !== -1) {
        this.accessories[index] = {
          ...this.accessories[index],
          ...this.accessoryForm,
          lastUpdated: new Date()
        };
      }
    } else {
      // Add new accessory
      const newAccessory: Accessory = {
        id: `ACC-${(this.accessories.length + 1).toString().padStart(3, '0')}`,
        ...this.accessoryForm,
        createdAt: new Date(),
        lastUpdated: new Date()
      };
      this.accessories.push(newAccessory);
    }
    
    this.extractFilterOptions();
    this.applyFilters();
    this.calculateStatistics();
    this.closeAccessoryModal();
  }

  deleteAccessory(accessory: Accessory): void {
    if (confirm(`Are you sure you want to delete "${accessory.name}"? This action cannot be undone.`)) {
      const index = this.accessories.findIndex(a => a.id === accessory.id);
      if (index !== -1) {
        this.accessories.splice(index, 1);
        this.extractFilterOptions();
        this.applyFilters();
        this.calculateStatistics();
      }
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
      unitPrice: 0,
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
      this.accessoryForm.minStockLevel >= 0 &&
      this.accessoryForm.unitPrice >= 0
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
    
    const newQuantity = this.calculateNewStock();
    const index = this.accessories.findIndex(a => a.id === this.selectedAccessory!.id);
    
    if (index !== -1) {
      this.accessories[index].quantity = newQuantity;
      this.accessories[index].lastUpdated = new Date();
      
      this.applyFilters();
      this.calculateStatistics();
      this.closeStockModal();
    }
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
    if (accessory.quantity === 0) {
      return 'Out of Stock';
    } else if (accessory.quantity <= accessory.minStockLevel) {
      return 'Low Stock';
    } else {
      return 'Available';
    }
  }

  trackByAccessoryId(index: number, accessory: Accessory): string {
    return accessory.id;
  }

  // Export and refresh methods
  exportInventory(): void {
    // In a real application, this would generate and download a CSV/Excel file
    const csvContent = this.generateCSVContent();
    const blob = new Blob([csvContent], { type: 'text/csv' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `accessories-inventory-${new Date().toISOString().split('T')[0]}.csv`;
    link.click();
    window.URL.revokeObjectURL(url);
  }

  private generateCSVContent(): string {
    const headers = ['Name', 'Category', 'Part Number', 'Quantity', 'Unit', 'Min Stock', 'Unit Price', 'Total Value', 'Supplier', 'Status', 'Location'];
    const rows = this.filteredAccessories.map(accessory => [
      accessory.name,
      accessory.category,
      accessory.partNumber,
      accessory.quantity.toString(),
      accessory.unit,
      accessory.minStockLevel.toString(),
      accessory.unitPrice.toFixed(2),
      (accessory.quantity * accessory.unitPrice).toFixed(2),
      accessory.supplier,
      this.getStatusDisplay(accessory),
      accessory.location || ''
    ]);
    
    return [headers, ...rows].map(row => row.map(cell => `"${cell}"`).join(',')).join('\n');
  }

  refreshInventory(): void {
    this.isRefreshing = true;
    setTimeout(() => {
      this.loadAccessories();
      this.isRefreshing = false;
    }, 1000);
  }
}