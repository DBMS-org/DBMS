import { Component, Input, Output, EventEmitter, OnInit, OnChanges, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { DrillPoint } from '../../models/drill-point.model';

export interface DepthTableRow {
  id: string;
  x: number;
  y: number;
  depth: number;
  isEditing?: boolean;
  originalDepth?: number;
  selected?: boolean;
}

export interface DepthChangeEvent {
  pointId: string;
  newDepth: number;
}

export interface BulkDepthChangeEvent {
  pointIds: string[];
  newDepth: number;
}

export interface ApplyGlobalDepthEvent {
  pointIds?: string[]; // If undefined, apply to all holes
}

export interface SortEvent {
  column: keyof DepthTableRow;
  direction: 'asc' | 'desc';
}

export interface FilterCriteria {
  searchText: string;
  depthFilter: 'all' | 'custom' | 'global';
}

export interface FilterEvent {
  criteria: FilterCriteria;
  filteredCount: number;
  totalCount: number;
}

@Component({
  selector: 'app-depth-editor-table',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatSortModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatFormFieldModule,
    MatSelectModule,
    MatChipsModule,
    MatCheckboxModule,
    MatMenuModule,
    MatDividerModule
  ],
  templateUrl: './depth-editor-table.component.html',
  styleUrls: ['./depth-editor-table.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DepthEditorTableComponent implements OnInit, OnChanges {
  @Input() drillPoints: DrillPoint[] = [];
  @Input() globalDepth: number = 10;
  @Input() readonly: boolean = false;
  
  @Output() depthChange = new EventEmitter<DepthChangeEvent>();
  @Output() bulkDepthChange = new EventEmitter<BulkDepthChangeEvent>();
  @Output() applyGlobalDepth = new EventEmitter<ApplyGlobalDepthEvent>();
  @Output() sortChange = new EventEmitter<SortEvent>();
  @Output() filterChange = new EventEmitter<FilterEvent>();

  displayedColumns: string[] = [];
  tableData: DepthTableRow[] = [];
  filteredData: DepthTableRow[] = [];
  sortColumn: keyof DepthTableRow = 'id';
  sortDirection: 'asc' | 'desc' = 'asc';

  // Selection properties
  selectedRows: Set<string> = new Set();
  bulkDepthValue: number = 10;
  isAllSelected: boolean = false;
  isIndeterminate: boolean = false;

  // Filter properties
  searchText: string = '';
  depthFilter: 'all' | 'custom' | 'global' = 'all';
  
  // Filter options
  depthFilterOptions = [
    { value: 'all', label: 'All Holes' },
    { value: 'custom', label: 'Custom Depths Only' },
    { value: 'global', label: 'Global Depth Only' }
  ];

  ngOnInit(): void {
    this.updateDisplayedColumns();
    this.updateTableData();
  }

  ngOnChanges(): void {
    this.updateDisplayedColumns();
    this.updateTableData();
  }

  private updateDisplayedColumns(): void {
    this.displayedColumns = [];
    
    if (!this.readonly) {
      this.displayedColumns.push('select');
    }
    
    this.displayedColumns.push('id', 'x', 'y', 'depth', 'actions');
  }

  private updateTableData(): void {
    this.tableData = this.drillPoints.map(point => ({
      id: point.id,
      x: Math.round(point.x * 100) / 100, // Round to 2 decimal places
      y: Math.round(point.y * 100) / 100,
      depth: Math.round(point.depth * 100) / 100,
      isEditing: false,
      originalDepth: point.depth,
      selected: this.selectedRows.has(point.id)
    }));
    
    this.applyFiltering();
    this.applySorting();
    this.updateSelectionState();
  }

  onSort(sort: Sort): void {
    if (!sort.active || sort.direction === '') {
      this.sortColumn = 'id';
      this.sortDirection = 'asc';
    } else {
      this.sortColumn = sort.active as keyof DepthTableRow;
      this.sortDirection = sort.direction as 'asc' | 'desc';
    }
    
    this.applyFiltering();
    this.applySorting();
    this.sortChange.emit({
      column: this.sortColumn,
      direction: this.sortDirection
    });
  }

  private applySorting(): void {
    this.filteredData.sort((a, b) => {
      const aValue = a[this.sortColumn];
      const bValue = b[this.sortColumn];
      
      let comparison = 0;
      if (typeof aValue === 'string' && typeof bValue === 'string') {
        comparison = aValue.localeCompare(bValue);
      } else if (typeof aValue === 'number' && typeof bValue === 'number') {
        comparison = aValue - bValue;
      }
      
      return this.sortDirection === 'asc' ? comparison : -comparison;
    });
  }

  private applyFiltering(): void {
    let filtered = [...this.tableData];

    // Apply search text filter
    if (this.searchText.trim()) {
      const searchLower = this.searchText.toLowerCase().trim();
      filtered = filtered.filter(row => 
        row.id.toLowerCase().includes(searchLower)
      );
    }

    // Apply depth filter
    if (this.depthFilter === 'custom') {
      filtered = filtered.filter(row => this.hasCustomDepth(row));
    } else if (this.depthFilter === 'global') {
      filtered = filtered.filter(row => !this.hasCustomDepth(row));
    }

    this.filteredData = filtered;

    // Emit filter change event
    this.filterChange.emit({
      criteria: {
        searchText: this.searchText,
        depthFilter: this.depthFilter
      },
      filteredCount: this.filteredData.length,
      totalCount: this.tableData.length
    });
  }

  startEdit(row: DepthTableRow): void {
    if (this.readonly) return;
    
    // Cancel any other editing rows
    this.filteredData.forEach(r => {
      if (r.isEditing && r.id !== row.id) {
        this.cancelEdit(r);
      }
    });
    
    row.isEditing = true;
    row.originalDepth = row.depth;
  }

  saveEdit(row: DepthTableRow): void {
    if (this.readonly) return;
    
    // Validate depth value
    if (!this.isValidDepth(row.depth)) {
      // Reset to original value if invalid
      row.depth = row.originalDepth || this.globalDepth;
      return;
    }
    
    // Round to 2 decimal places
    row.depth = Math.round(row.depth * 100) / 100;
    
    row.isEditing = false;
    
    // Emit the change event
    this.depthChange.emit({
      pointId: row.id,
      newDepth: row.depth
    });
  }

  cancelEdit(row: DepthTableRow): void {
    row.depth = row.originalDepth || this.globalDepth;
    row.isEditing = false;
  }

  isValidDepth(depth: number): boolean {
    return !isNaN(depth) && depth >= 1.0 && depth <= 50.0;
  }

  hasCustomDepth(row: DepthTableRow): boolean {
    return Math.abs(row.depth - this.globalDepth) > 0.01;
  }

  getDepthDisplayClass(row: DepthTableRow): string {
    if (this.hasCustomDepth(row)) {
      return 'custom-depth';
    }
    return 'global-depth';
  }

  onDepthInputKeydown(event: KeyboardEvent, row: DepthTableRow): void {
    if (event.key === 'Enter') {
      this.saveEdit(row);
    } else if (event.key === 'Escape') {
      this.cancelEdit(row);
    }
  }

  onDepthInputBlur(row: DepthTableRow): void {
    this.saveEdit(row);
  }

  // Filter methods
  onSearchTextChange(): void {
    this.applyFiltering();
    this.applySorting();
  }

  onDepthFilterChange(): void {
    this.applyFiltering();
    this.applySorting();
  }

  clearSearch(): void {
    this.searchText = '';
    this.onSearchTextChange();
  }

  clearFilters(): void {
    this.searchText = '';
    this.depthFilter = 'all';
    this.applyFiltering();
    this.applySorting();
  }

  getFilterSummary(): string {
    const total = this.tableData.length;
    const filtered = this.filteredData.length;
    
    if (total === filtered) {
      return `Showing all ${total} holes`;
    } else {
      return `Showing ${filtered} of ${total} holes`;
    }
  }

  hasActiveFilters(): boolean {
    return this.searchText.trim() !== '' || this.depthFilter !== 'all';
  }

  clearDepthFilter(): void {
    this.depthFilter = 'all';
    this.onDepthFilterChange();
  }

  getDepthFilterLabel(): string {
    return this.depthFilterOptions.find(opt => opt.value === this.depthFilter)?.label || '';
  }

  // Selection methods
  toggleRowSelection(row: DepthTableRow): void {
    if (this.selectedRows.has(row.id)) {
      this.selectedRows.delete(row.id);
      row.selected = false;
    } else {
      this.selectedRows.add(row.id);
      row.selected = true;
    }
    this.updateSelectionState();
  }

  toggleAllSelection(): void {
    if (this.isAllSelected) {
      // Deselect all
      this.selectedRows.clear();
      this.filteredData.forEach(row => row.selected = false);
    } else {
      // Select all filtered rows
      this.filteredData.forEach(row => {
        this.selectedRows.add(row.id);
        row.selected = true;
      });
    }
    this.updateSelectionState();
  }

  private updateSelectionState(): void {
    const selectedCount = this.filteredData.filter(row => row.selected).length;
    const totalCount = this.filteredData.length;
    
    this.isAllSelected = selectedCount === totalCount && totalCount > 0;
    this.isIndeterminate = selectedCount > 0 && selectedCount < totalCount;
  }

  getSelectedCount(): number {
    return this.selectedRows.size;
  }

  hasSelectedRows(): boolean {
    return this.selectedRows.size > 0;
  }

  clearSelection(): void {
    this.selectedRows.clear();
    this.filteredData.forEach(row => row.selected = false);
    this.updateSelectionState();
  }

  // Bulk operations
  applyBulkDepthChange(): void {
    if (this.readonly || this.selectedRows.size === 0) return;
    
    if (!this.isValidDepth(this.bulkDepthValue)) {
      return;
    }

    const selectedIds = Array.from(this.selectedRows);
    this.bulkDepthChange.emit({
      pointIds: selectedIds,
      newDepth: Math.round(this.bulkDepthValue * 100) / 100
    });

    // Update local data for immediate UI feedback
    this.tableData.forEach(row => {
      if (this.selectedRows.has(row.id)) {
        row.depth = Math.round(this.bulkDepthValue * 100) / 100;
      }
    });

    this.clearSelection();
  }

  applyGlobalDepthToSelected(): void {
    if (this.readonly || this.selectedRows.size === 0) return;

    const selectedIds = Array.from(this.selectedRows);
    this.applyGlobalDepth.emit({
      pointIds: selectedIds
    });

    // Update local data for immediate UI feedback
    this.tableData.forEach(row => {
      if (this.selectedRows.has(row.id)) {
        row.depth = this.globalDepth;
      }
    });

    this.clearSelection();
  }

  applyGlobalDepthToAll(): void {
    if (this.readonly) return;

    this.applyGlobalDepth.emit({
      pointIds: undefined // Apply to all holes
    });

    // Update local data for immediate UI feedback
    this.tableData.forEach(row => {
      row.depth = this.globalDepth;
    });

    this.clearSelection();
  }

  resetSelectedToGlobalDepth(): void {
    this.applyGlobalDepthToSelected();
  }

  getSelectedRowsWithCustomDepth(): DepthTableRow[] {
    return this.filteredData.filter(row => 
      row.selected && this.hasCustomDepth(row)
    );
  }

  hasSelectedCustomDepthRows(): boolean {
    return this.getSelectedRowsWithCustomDepth().length > 0;
  }

  getAllRowsWithCustomDepth(): DepthTableRow[] {
    return this.tableData.filter(row => this.hasCustomDepth(row));
  }

  hasAnyCustomDepthRows(): boolean {
    return this.getAllRowsWithCustomDepth().length > 0;
  }

  getBulkOperationSummary(): string {
    const selectedCount = this.selectedRows.size;
    if (selectedCount === 0) {
      return 'No holes selected';
    } else if (selectedCount === 1) {
      return '1 hole selected';
    } else {
      return `${selectedCount} holes selected`;
    }
  }
}