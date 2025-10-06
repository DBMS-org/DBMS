import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CentralInventoryService } from '../../../../core/services/central-inventory.service';
import { CentralInventory, ExplosiveType } from '../../../../core/models/central-inventory.model';

@Component({
  selector: 'app-emulsion-inventory',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './emulsion-inventory.component.html',
  styleUrl: './emulsion-inventory.component.scss'
})
export class EmulsionInventoryComponent implements OnInit {
  inventory: CentralInventory[] = [];
  isLoading = false;
  errorMessage: string | null = null;
  selectedItem: CentralInventory | null = null;
  showDetailsModal = false;

  constructor(
    private router: Router,
    private inventoryService: CentralInventoryService
  ) {}

  ngOnInit(): void {
    this.loadInventory();
  }

  loadInventory(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.inventoryService.getInventoryByType(ExplosiveType.Emulsion).subscribe({
      next: (data) => {
        this.inventory = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading Emulsion inventory:', error);
        this.errorMessage = error.message || 'Failed to load inventory';
        this.isLoading = false;
      }
    });
  }

  get totalStock(): number {
    return this.inventory.reduce((sum, item) => sum + item.availableQuantity, 0);
  }

  get lowStockCount(): number {
    return this.inventory.filter(item => item.availableQuantity < 200).length; // Less than 200kg
  }

  navigateToAdd(): void {
    this.router.navigate(['/explosive-manager/inventory/emulsion/add']);
  }

  navigateToEdit(id: number): void {
    this.router.navigate(['/explosive-manager/inventory/emulsion/edit', id]);
  }

  getStatusClass(item: CentralInventory): string {
    const daysUntilExpiry = this.getDaysUntilExpiry(item.expiryDate);

    if (daysUntilExpiry < 30) return 'warning';
    if (item.availableQuantity < 200) return 'warning';
    return 'good';
  }

  getStatusText(item: CentralInventory): string {
    const daysUntilExpiry = this.getDaysUntilExpiry(item.expiryDate);

    if (daysUntilExpiry < 30) return 'Expiring Soon';
    if (item.availableQuantity < 200) return 'Low Stock';
    return 'Good';
  }

  private getDaysUntilExpiry(expiryDate: Date): number {
    const today = new Date();
    const expiry = new Date(expiryDate);
    const diffTime = expiry.getTime() - today.getTime();
    return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  }

  viewDetails(item: CentralInventory): void {
    this.selectedItem = item;
    this.showDetailsModal = true;
  }

  closeDetailsModal(): void {
    this.showDetailsModal = false;
    this.selectedItem = null;
  }
}
