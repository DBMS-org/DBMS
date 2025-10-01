import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { StoreFilters, StoreStatus } from '../../../../core/models/store.model';

@Component({
  selector: 'app-store-filters',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './store-filters.component.html',
  styleUrl: './store-filters.component.scss'
})
export class StoreFiltersComponent {
  @Input() filters: StoreFilters = {
    status: 'ALL',
    regionId: 'ALL',
    city: 'ALL',
    storeManager: 'ALL',
    searchTerm: ''
  };
  @Input() storeStatuses: string[] = [];
  @Input() uniqueLocations: string[] = [];
  @Input() uniqueManagers: string[] = [];

  @Output() filtersChange = new EventEmitter<StoreFilters>();
  @Output() searchChange = new EventEmitter<string>();
  @Output() clearFilters = new EventEmitter<void>();

  onFilterChange(): void {
    this.filtersChange.emit(this.filters);
  }

  onSearchChange(): void {
    this.searchChange.emit(this.filters.searchTerm);
  }

  onClearFilters(): void {
    this.clearFilters.emit();
  }
}
