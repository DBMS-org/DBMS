import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CentralInventoryService } from '../../../../core/services/central-inventory.service';
import { InventoryDashboard } from '../../../../core/models/central-inventory.model';

@Component({
  selector: 'app-inventory-overview',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './inventory-overview.component.html',
  styleUrl: './inventory-overview.component.scss'
})
export class InventoryOverviewComponent implements OnInit {
  dashboard: InventoryDashboard | null = null;
  isLoading = false;
  errorMessage: string | null = null;
  showAddStockMenu = false;

  constructor(
    private inventoryService: CentralInventoryService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.inventoryService.getDashboard().subscribe({
      next: (data) => {
        this.dashboard = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading dashboard:', error);
        this.errorMessage = error.message || 'Failed to load dashboard';
        this.isLoading = false;
      }
    });
  }

  // Helper methods for template
  get totalANFO(): number {
    return this.dashboard?.quantityByType?.ANFO || 0;
  }

  get totalEmulsion(): number {
    return this.dashboard?.quantityByType?.Emulsion || 0;
  }

  get totalInventory(): number {
    return this.dashboard?.totalQuantity || 0;
  }

  get lowStockCount(): number {
    return this.dashboard?.depletedBatches || 0;
  }

  get expiringBatchesCount(): number {
    return this.dashboard?.expiringBatches || 0;
  }

  get pendingTransfersCount(): number {
    return this.dashboard?.pendingTransferRequests || 0;
  }

  toggleAddStockMenu(): void {
    this.showAddStockMenu = !this.showAddStockMenu;
  }

  addANFOStock(): void {
    this.showAddStockMenu = false;
    this.router.navigate(['/explosive-manager/inventory/anfo/add']);
  }

  addEmulsionStock(): void {
    this.showAddStockMenu = false;
    this.router.navigate(['/explosive-manager/inventory/emulsion/add']);
  }

  recordUsage(): void {
    // Navigate to usage recording page
    this.router.navigate(['/explosive-manager/inventory']);
  }

  generateReport(): void {
    // Navigate to reports page
    this.router.navigate(['/explosive-manager/inventory']);
  }

  syncInventory(): void {
    this.loadDashboard();
  }
}
