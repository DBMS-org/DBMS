import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { By } from '@angular/platform-browser';

import { DepthEditorTableComponent, FilterEvent } from './depth-editor-table.component';
import { DrillPoint } from '../models/drill-point.model';

describe('DepthEditorTableComponent', () => {
  let component: DepthEditorTableComponent;
  let fixture: ComponentFixture<DepthEditorTableComponent>;

  const mockDrillPoints: DrillPoint[] = [
    { id: 'DH001', x: 10.5, y: 20.3, depth: 10.0, spacing: 5.0, burden: 4.0 },
    { id: 'DH002', x: 15.5, y: 20.3, depth: 12.5, spacing: 5.0, burden: 4.0 },
    { id: 'DH003', x: 20.5, y: 20.3, depth: 10.0, spacing: 5.0, burden: 4.0 }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DepthEditorTableComponent, NoopAnimationsModule]
    }).compileComponents();

    fixture = TestBed.createComponent(DepthEditorTableComponent);
    component = fixture.componentInstance;
    
    // Set up default inputs
    component.drillPoints = mockDrillPoints;
    component.globalDepth = 10.0;
    component.readonly = false;
    
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display drill points in table', () => {
    const rows = fixture.debugElement.queryAll(By.css('tr[mat-row]'));
    expect(rows.length).toBe(3);
    expect(component.filteredData.length).toBe(3);
  });

  it('should show correct column headers', () => {
    const headers = fixture.debugElement.queryAll(By.css('th[mat-header-cell]'));
    const headerTexts = headers.map(h => h.nativeElement.textContent.trim());
    
    expect(headerTexts).toContain('Hole ID');
    expect(headerTexts).toContain('X (m)');
    expect(headerTexts).toContain('Y (m)');
    expect(headerTexts).toContain('Depth (m)');
    expect(headerTexts).toContain('Actions');
  });

  it('should identify custom depths correctly', () => {
    expect(component.hasCustomDepth({ id: 'DH001', x: 10, y: 20, depth: 10.0 })).toBeFalse();
    expect(component.hasCustomDepth({ id: 'DH002', x: 15, y: 20, depth: 12.5 })).toBeTrue();
  });

  it('should apply correct CSS classes for custom depths', () => {
    const customDepthRow = { id: 'DH002', x: 15, y: 20, depth: 12.5 };
    const globalDepthRow = { id: 'DH001', x: 10, y: 20, depth: 10.0 };
    
    expect(component.getDepthDisplayClass(customDepthRow)).toBe('custom-depth');
    expect(component.getDepthDisplayClass(globalDepthRow)).toBe('global-depth');
  });

  it('should start editing when edit button is clicked', () => {
    const editButton = fixture.debugElement.query(By.css('button[aria-label="Edit hole depth"]'));
    editButton.nativeElement.click();
    fixture.detectChanges();
    
    expect(component.tableData[0].isEditing).toBeTrue();
  });

  it('should emit depth change event when saving edit', () => {
    spyOn(component.depthChange, 'emit');
    
    const row = component.tableData[0];
    row.isEditing = true;
    row.depth = 15.0;
    
    component.saveEdit(row);
    
    expect(component.depthChange.emit).toHaveBeenCalledWith({
      pointId: 'DH001',
      newDepth: 15.0
    });
  });

  it('should validate depth values', () => {
    expect(component['isValidDepth'](5.0)).toBeTrue();
    expect(component['isValidDepth'](0.5)).toBeFalse(); // Below minimum
    expect(component['isValidDepth'](55.0)).toBeFalse(); // Above maximum
    expect(component['isValidDepth'](NaN)).toBeFalse(); // Invalid number
  });

  it('should cancel edit and restore original value', () => {
    const row = component.tableData[0];
    const originalDepth = row.depth;
    
    row.isEditing = true;
    row.depth = 99.0; // Invalid value
    
    component.cancelEdit(row);
    
    expect(row.isEditing).toBeFalse();
    expect(row.depth).toBe(originalDepth);
  });

  it('should sort table data correctly', () => {
    component.onSort({ active: 'depth', direction: 'desc' });
    
    expect(component.filteredData[0].depth).toBe(12.5); // Highest depth first
    expect(component.sortColumn).toBe('depth');
    expect(component.sortDirection).toBe('desc');
  });

  it('should emit sort change event', () => {
    spyOn(component.sortChange, 'emit');
    
    component.onSort({ active: 'x', direction: 'asc' });
    
    expect(component.sortChange.emit).toHaveBeenCalledWith({
      column: 'x',
      direction: 'asc'
    });
  });

  it('should handle keyboard events in edit mode', () => {
    const row = component.tableData[0];
    row.isEditing = true;
    
    spyOn(component, 'saveEdit');
    spyOn(component, 'cancelEdit');
    
    // Test Enter key
    const enterEvent = new KeyboardEvent('keydown', { key: 'Enter' });
    component.onDepthInputKeydown(enterEvent, row);
    expect(component.saveEdit).toHaveBeenCalledWith(row);
    
    // Test Escape key
    const escapeEvent = new KeyboardEvent('keydown', { key: 'Escape' });
    component.onDepthInputKeydown(escapeEvent, row);
    expect(component.cancelEdit).toHaveBeenCalledWith(row);
  });

  it('should disable editing in readonly mode', () => {
    component.readonly = true;
    fixture.detectChanges();
    
    const actionButtons = fixture.debugElement.queryAll(By.css('.action-buttons'));
    expect(actionButtons.length).toBe(0);
    
    const readonlyIndicators = fixture.debugElement.queryAll(By.css('.readonly-indicator'));
    expect(readonlyIndicators.length).toBe(mockDrillPoints.length);
  });

  it('should show no data message when table is empty', () => {
    component.drillPoints = [];
    component.ngOnInit();
    fixture.detectChanges();
    
    const noDataElement = fixture.debugElement.query(By.css('.no-data'));
    expect(noDataElement).toBeTruthy();
    expect(noDataElement.nativeElement.textContent).toContain('No drill holes to display');
  });

  it('should round coordinates and depths to 2 decimal places', () => {
    const pointWithPrecision: DrillPoint = {
      id: 'DH004',
      x: 10.123456,
      y: 20.987654,
      depth: 15.123456,
      spacing: 5.0,
      burden: 4.0
    };
    
    component.drillPoints = [pointWithPrecision];
    component.ngOnChanges();
    
    const tableRow = component.tableData[0];
    expect(tableRow.x).toBe(10.12);
    expect(tableRow.y).toBe(20.99);
    expect(tableRow.depth).toBe(15.12);
  });

  it('should show custom depth indicator icon for custom depths', () => {
    fixture.detectChanges();
    
    // Find the row with custom depth (DH002 has depth 12.5, global is 10.0)
    const customDepthRows = fixture.debugElement.queryAll(By.css('.custom-depth-row'));
    expect(customDepthRows.length).toBeGreaterThan(0);
    
    const customIndicator = fixture.debugElement.query(By.css('.custom-indicator'));
    expect(customIndicator).toBeTruthy();
  });

  // Filtering functionality tests
  describe('Filtering functionality', () => {
    beforeEach(() => {
      fixture.detectChanges();
    });

    it('should filter by search text', () => {
      component.searchText = 'DH001';
      component.onSearchTextChange();
      
      expect(component.filteredData.length).toBe(1);
      expect(component.filteredData[0].id).toBe('DH001');
    });

    it('should filter by search text case insensitively', () => {
      component.searchText = 'dh001';
      component.onSearchTextChange();
      
      expect(component.filteredData.length).toBe(1);
      expect(component.filteredData[0].id).toBe('DH001');
    });

    it('should filter by partial search text', () => {
      component.searchText = '00';
      component.onSearchTextChange();
      
      expect(component.filteredData.length).toBe(3); // All contain '00'
    });

    it('should filter by custom depths only', () => {
      component.depthFilter = 'custom';
      component.onDepthFilterChange();
      
      expect(component.filteredData.length).toBe(1);
      expect(component.filteredData[0].id).toBe('DH002'); // Only DH002 has custom depth
    });

    it('should filter by global depths only', () => {
      component.depthFilter = 'global';
      component.onDepthFilterChange();
      
      expect(component.filteredData.length).toBe(2);
      expect(component.filteredData.map(d => d.id)).toEqual(['DH001', 'DH003']);
    });

    it('should combine search and depth filters', () => {
      component.searchText = 'DH00';
      component.depthFilter = 'global';
      component.onSearchTextChange();
      
      expect(component.filteredData.length).toBe(2); // DH001 and DH003 match both filters
    });

    it('should clear search filter', () => {
      component.searchText = 'DH001';
      component.onSearchTextChange();
      expect(component.filteredData.length).toBe(1);
      
      component.clearSearch();
      expect(component.searchText).toBe('');
      expect(component.filteredData.length).toBe(3);
    });

    it('should clear all filters', () => {
      component.searchText = 'DH001';
      component.depthFilter = 'custom';
      component.onSearchTextChange();
      expect(component.filteredData.length).toBe(0); // No match for both filters
      
      component.clearFilters();
      expect(component.searchText).toBe('');
      expect(component.depthFilter).toBe('all');
      expect(component.filteredData.length).toBe(3);
    });

    it('should emit filter change events', () => {
      spyOn(component.filterChange, 'emit');
      
      component.searchText = 'DH001';
      component.onSearchTextChange();
      
      expect(component.filterChange.emit).toHaveBeenCalledWith({
        criteria: {
          searchText: 'DH001',
          depthFilter: 'all'
        },
        filteredCount: 1,
        totalCount: 3
      });
    });

    it('should detect active filters correctly', () => {
      expect(component.hasActiveFilters()).toBeFalse();
      
      component.searchText = 'test';
      expect(component.hasActiveFilters()).toBeTrue();
      
      component.searchText = '';
      component.depthFilter = 'custom';
      expect(component.hasActiveFilters()).toBeTrue();
      
      component.depthFilter = 'all';
      expect(component.hasActiveFilters()).toBeFalse();
    });

    it('should generate correct filter summary', () => {
      expect(component.getFilterSummary()).toBe('Showing all 3 holes');
      
      component.searchText = 'DH001';
      component.onSearchTextChange();
      expect(component.getFilterSummary()).toBe('Showing 1 of 3 holes');
    });

    it('should show no filtered data when filters match nothing', () => {
      component.searchText = 'NONEXISTENT';
      component.onSearchTextChange();
      
      expect(component.filteredData.length).toBe(0);
      expect(component.tableData.length).toBeGreaterThan(0);
      
      // Check if the condition for showing no-filtered-data is met
      const shouldShowNoFilteredData = component.tableData.length > 0 && component.filteredData.length === 0;
      expect(shouldShowNoFilteredData).toBeTrue();
    });

    it('should maintain sorting after filtering', () => {
      component.onSort({ active: 'depth', direction: 'desc' });
      component.searchText = 'DH00';
      component.onSearchTextChange();
      
      // Should still be sorted by depth descending
      expect(component.filteredData[0].depth).toBeGreaterThanOrEqual(component.filteredData[1].depth);
    });

    it('should clear depth filter correctly', () => {
      component.depthFilter = 'custom';
      component.clearDepthFilter();
      
      expect(component.depthFilter).toBe('all');
      expect(component.filteredData.length).toBe(3);
    });

    it('should get depth filter label correctly', () => {
      component.depthFilter = 'custom';
      expect(component.getDepthFilterLabel()).toBe('Custom Depths Only');
      
      component.depthFilter = 'global';
      expect(component.getDepthFilterLabel()).toBe('Global Depth Only');
      
      component.depthFilter = 'all';
      expect(component.getDepthFilterLabel()).toBe('All Holes');
    });
  });

  // Bulk operations tests
  describe('Bulk operations functionality', () => {
    beforeEach(() => {
      fixture.detectChanges();
    });

    it('should toggle row selection', () => {
      const row = component.tableData[0];
      expect(row.selected).toBeFalsy();
      expect(component.selectedRows.has(row.id)).toBeFalse();
      
      component.toggleRowSelection(row);
      
      expect(row.selected).toBeTrue();
      expect(component.selectedRows.has(row.id)).toBeTrue();
      expect(component.getSelectedCount()).toBe(1);
    });

    it('should toggle all selection', () => {
      expect(component.isAllSelected).toBeFalse();
      expect(component.getSelectedCount()).toBe(0);
      
      component.toggleAllSelection();
      
      expect(component.isAllSelected).toBeTrue();
      expect(component.getSelectedCount()).toBe(3);
      expect(component.filteredData.every(row => row.selected)).toBeTrue();
    });

    it('should deselect all when toggling all selection twice', () => {
      component.toggleAllSelection(); // Select all
      expect(component.getSelectedCount()).toBe(3);
      
      component.toggleAllSelection(); // Deselect all
      expect(component.getSelectedCount()).toBe(0);
      expect(component.isAllSelected).toBeFalse();
      expect(component.filteredData.every(row => !row.selected)).toBeTrue();
    });

    it('should update selection state correctly', () => {
      // No selection
      expect(component.isAllSelected).toBeFalse();
      expect(component.isIndeterminate).toBeFalse();
      
      // Partial selection
      component.toggleRowSelection(component.tableData[0]);
      expect(component.isAllSelected).toBeFalse();
      expect(component.isIndeterminate).toBeTrue();
      
      // Full selection
      component.toggleAllSelection();
      expect(component.isAllSelected).toBeTrue();
      expect(component.isIndeterminate).toBeFalse();
    });

    it('should clear selection', () => {
      component.toggleAllSelection(); // Select all
      expect(component.getSelectedCount()).toBe(3);
      
      component.clearSelection();
      
      expect(component.getSelectedCount()).toBe(0);
      expect(component.selectedRows.size).toBe(0);
      expect(component.filteredData.every(row => !row.selected)).toBeTrue();
    });

    it('should check if has selected rows', () => {
      expect(component.hasSelectedRows()).toBeFalse();
      
      component.toggleRowSelection(component.tableData[0]);
      expect(component.hasSelectedRows()).toBeTrue();
    });

    it('should emit bulk depth change event', () => {
      spyOn(component.bulkDepthChange, 'emit');
      
      // Select some rows
      component.toggleRowSelection(component.tableData[0]);
      component.toggleRowSelection(component.tableData[1]);
      component.bulkDepthValue = 15.0;
      
      component.applyBulkDepthChange();
      
      expect(component.bulkDepthChange.emit).toHaveBeenCalledWith({
        pointIds: ['DH001', 'DH002'],
        newDepth: 15.0
      });
    });

    it('should not apply bulk depth change with invalid depth', () => {
      spyOn(component.bulkDepthChange, 'emit');
      
      component.toggleRowSelection(component.tableData[0]);
      component.bulkDepthValue = 0.5; // Invalid depth
      
      component.applyBulkDepthChange();
      
      expect(component.bulkDepthChange.emit).not.toHaveBeenCalled();
    });

    it('should not apply bulk depth change with no selection', () => {
      spyOn(component.bulkDepthChange, 'emit');
      
      component.bulkDepthValue = 15.0;
      component.applyBulkDepthChange();
      
      expect(component.bulkDepthChange.emit).not.toHaveBeenCalled();
    });

    it('should not apply bulk depth change in readonly mode', () => {
      spyOn(component.bulkDepthChange, 'emit');
      
      component.readonly = true;
      component.toggleRowSelection(component.tableData[0]);
      component.bulkDepthValue = 15.0;
      
      component.applyBulkDepthChange();
      
      expect(component.bulkDepthChange.emit).not.toHaveBeenCalled();
    });

    it('should emit apply global depth to selected event', () => {
      spyOn(component.applyGlobalDepth, 'emit');
      
      component.toggleRowSelection(component.tableData[0]);
      component.toggleRowSelection(component.tableData[1]);
      
      component.applyGlobalDepthToSelected();
      
      expect(component.applyGlobalDepth.emit).toHaveBeenCalledWith({
        pointIds: ['DH001', 'DH002']
      });
    });

    it('should emit apply global depth to all event', () => {
      spyOn(component.applyGlobalDepth, 'emit');
      
      component.applyGlobalDepthToAll();
      
      expect(component.applyGlobalDepth.emit).toHaveBeenCalledWith({
        pointIds: undefined
      });
    });

    it('should reset selected to global depth', () => {
      spyOn(component, 'applyGlobalDepthToSelected');
      
      component.resetSelectedToGlobalDepth();
      
      expect(component.applyGlobalDepthToSelected).toHaveBeenCalled();
    });

    it('should get selected rows with custom depth', () => {
      // DH002 has custom depth (12.5 vs global 10.0)
      component.toggleRowSelection(component.tableData[0]); // DH001 - global depth
      component.toggleRowSelection(component.tableData[1]); // DH002 - custom depth
      
      const customDepthRows = component.getSelectedRowsWithCustomDepth();
      
      expect(customDepthRows.length).toBe(1);
      expect(customDepthRows[0].id).toBe('DH002');
    });

    it('should check if has selected custom depth rows', () => {
      expect(component.hasSelectedCustomDepthRows()).toBeFalse();
      
      component.toggleRowSelection(component.tableData[1]); // DH002 - custom depth
      expect(component.hasSelectedCustomDepthRows()).toBeTrue();
      
      component.clearSelection();
      component.toggleRowSelection(component.tableData[0]); // DH001 - global depth
      expect(component.hasSelectedCustomDepthRows()).toBeFalse();
    });

    it('should get all rows with custom depth', () => {
      const customDepthRows = component.getAllRowsWithCustomDepth();
      
      expect(customDepthRows.length).toBe(1);
      expect(customDepthRows[0].id).toBe('DH002');
    });

    it('should check if has any custom depth rows', () => {
      expect(component.hasAnyCustomDepthRows()).toBeTrue(); // DH002 has custom depth
      
      // Change all to global depth
      component.tableData.forEach(row => row.depth = component.globalDepth);
      expect(component.hasAnyCustomDepthRows()).toBeFalse();
    });

    it('should generate bulk operation summary', () => {
      expect(component.getBulkOperationSummary()).toBe('No holes selected');
      
      component.toggleRowSelection(component.tableData[0]);
      expect(component.getBulkOperationSummary()).toBe('1 hole selected');
      
      component.toggleRowSelection(component.tableData[1]);
      expect(component.getBulkOperationSummary()).toBe('2 holes selected');
    });

    it('should update local data after bulk depth change', () => {
      component.toggleRowSelection(component.tableData[0]);
      component.toggleRowSelection(component.tableData[1]);
      component.bulkDepthValue = 15.5;
      
      const originalDepths = [component.tableData[0].depth, component.tableData[1].depth];
      
      component.applyBulkDepthChange();
      
      expect(component.tableData[0].depth).toBe(15.5);
      expect(component.tableData[1].depth).toBe(15.5);
      expect(component.tableData[2].depth).toBe(originalDepths[2] || 10.0); // Unchanged
      expect(component.getSelectedCount()).toBe(0); // Selection cleared
    });

    it('should update local data after applying global depth to selected', () => {
      component.toggleRowSelection(component.tableData[1]); // DH002 with custom depth
      const originalDepth = component.tableData[1].depth;
      
      expect(originalDepth).not.toBe(component.globalDepth);
      
      component.applyGlobalDepthToSelected();
      
      expect(component.tableData[1].depth).toBe(component.globalDepth);
      expect(component.getSelectedCount()).toBe(0); // Selection cleared
    });

    it('should update local data after applying global depth to all', () => {
      const originalDepths = component.tableData.map(row => row.depth);
      
      component.applyGlobalDepthToAll();
      
      component.tableData.forEach(row => {
        expect(row.depth).toBe(component.globalDepth);
      });
      expect(component.getSelectedCount()).toBe(0); // Selection cleared
    });

    it('should round bulk depth value to 2 decimal places', () => {
      spyOn(component.bulkDepthChange, 'emit');
      
      component.toggleRowSelection(component.tableData[0]);
      component.bulkDepthValue = 15.123456;
      
      component.applyBulkDepthChange();
      
      expect(component.bulkDepthChange.emit).toHaveBeenCalledWith({
        pointIds: ['DH001'],
        newDepth: 15.12
      });
    });

    it('should maintain selection state after filtering', () => {
      component.toggleRowSelection(component.tableData[0]); // Select DH001
      component.toggleRowSelection(component.tableData[1]); // Select DH002
      expect(component.getSelectedCount()).toBe(2);
      
      // Apply filter that shows only DH001
      component.searchText = 'DH001';
      component.onSearchTextChange();
      
      expect(component.filteredData.length).toBe(1);
      expect(component.getSelectedCount()).toBe(2); // Selection count unchanged
      expect(component.selectedRows.has('DH001')).toBeTrue();
      expect(component.selectedRows.has('DH002')).toBeTrue();
    });

    it('should update selection state correctly after filtering', () => {
      // Select all rows
      component.toggleAllSelection();
      expect(component.isAllSelected).toBeTrue();
      
      // Filter to show only one row
      component.searchText = 'DH001';
      component.onSearchTextChange();
      
      // Selection state should update based on filtered data
      expect(component.filteredData.length).toBe(1);
      expect(component.isAllSelected).toBeTrue(); // All visible rows are selected
      expect(component.isIndeterminate).toBeFalse();
    });
  });
});