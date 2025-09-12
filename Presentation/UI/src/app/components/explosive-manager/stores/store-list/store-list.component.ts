import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store, StoreStatus } from '../../../../core/models/store.model';

@Component({
  selector: 'app-store-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './store-list.component.html',
  styleUrl: './store-list.component.scss'
})
export class StoreListComponent {
  @Input() stores: Store[] = [];
  @Input() isLoading = false;

  @Output() viewStore = new EventEmitter<Store>();
  @Output() editStore = new EventEmitter<Store>();
  @Output() deleteStore = new EventEmitter<Store>();
  @Output() deactivateStore = new EventEmitter<Store>();
  @Output() addNewStore = new EventEmitter<void>();
  @Output() clearFilters = new EventEmitter<void>();

  onViewStore(store: Store): void {
    this.viewStore.emit(store);
  }

  onEditStore(store: Store): void {
    this.editStore.emit(store);
  }

  onDeleteStore(store: Store): void {
    this.deleteStore.emit(store);
  }

  onDeactivateStore(store: Store): void {
    this.deactivateStore.emit(store);
  }

  onAddNewStore(): void {
    this.addNewStore.emit();
  }

  onClearFilters(): void {
    this.clearFilters.emit();
  }

  getStatusBadgeClass(status: StoreStatus): string {
    switch (status) {
      case StoreStatus.OPERATIONAL:
        return 'badge-success';
      case StoreStatus.UNDER_MAINTENANCE:
        return 'badge-warning';
      case StoreStatus.TEMPORARILY_CLOSED:
        return 'badge-danger';
      case StoreStatus.INSPECTION_REQUIRED:
        return 'badge-info';
      case StoreStatus.DECOMMISSIONED:
        return 'badge-secondary';
      default:
        return 'badge-secondary';
    }
  }

  getStatusClass(status: StoreStatus): string {
    switch (status) {
      case StoreStatus.OPERATIONAL:
        return 'text-success';
      case StoreStatus.UNDER_MAINTENANCE:
        return 'text-warning';
      case StoreStatus.TEMPORARILY_CLOSED:
        return 'text-danger';
      case StoreStatus.INSPECTION_REQUIRED:
        return 'text-info';
      case StoreStatus.DECOMMISSIONED:
        return 'text-secondary';
      default:
        return 'text-secondary';
    }
  }

  getUtilizationPercentage(store: Store): number {
    return store.storageCapacity > 0 ? ((store.currentOccupancy || 0) / store.storageCapacity) * 100 : 0;
  }

  getUtilizationClass(percentage: number): string {
    if (percentage >= 90) return 'high-utilization';
    if (percentage >= 70) return 'medium-utilization';
    return 'low-utilization';
  }
}
