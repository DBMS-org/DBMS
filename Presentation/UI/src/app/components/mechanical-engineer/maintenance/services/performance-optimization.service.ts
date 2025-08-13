import { Injectable, signal } from '@angular/core';
import { debounceTime, distinctUntilChanged, Subject, Observable } from 'rxjs';
import { MaintenanceJob, JobFilters } from '../models/maintenance.models';

export interface SearchIndex {
  id: string;
  searchableText: string;
  originalItem: MaintenanceJob;
}

export interface FilterPerformanceMetrics {
  filterTime: number;
  searchTime: number;
  totalItems: number;
  filteredItems: number;
  lastUpdate: Date;
}

@Injectable({
  providedIn: 'root'
})
export class PerformanceOptimizationService {
  private searchIndexes = new Map<string, SearchIndex[]>();
  private filterCache = new Map<string, MaintenanceJob[]>();
  private searchSubjects = new Map<string, Subject<string>>();
  private performanceMetrics = signal<Map<string, FilterPerformanceMetrics>>(new Map());

  // Debounce configuration
  private readonly SEARCH_DEBOUNCE_TIME = 300; // ms
  private readonly FILTER_DEBOUNCE_TIME = 150; // ms

  /**
   * Create search index for fast text searching
   */
  createSearchIndex(datasetId: string, items: MaintenanceJob[]): void {
    const startTime = performance.now();
    
    const searchIndex: SearchIndex[] = items.map(item => ({
      id: item.id,
      searchableText: this.createSearchableText(item),
      originalItem: item
    }));

    this.searchIndexes.set(datasetId, searchIndex);
    
    const endTime = performance.now();
    console.log(`Search index created for ${datasetId}: ${endTime - startTime}ms for ${items.length} items`);
  }

  /**
   * Update search index when items change
   */
  updateSearchIndex(datasetId: string, items: MaintenanceJob[]): void {
    // Only recreate if items have changed significantly
    const existingIndex = this.searchIndexes.get(datasetId);
    if (!existingIndex || existingIndex.length !== items.length) {
      this.createSearchIndex(datasetId, items);
      return;
    }

    // Update existing index
    const startTime = performance.now();
    const updatedIndex = existingIndex.map((indexItem, i) => {
      const currentItem = items[i];
      if (currentItem && indexItem.id === currentItem.id) {
        return {
          ...indexItem,
          searchableText: this.createSearchableText(currentItem),
          originalItem: currentItem
        };
      }
      return indexItem;
    });

    this.searchIndexes.set(datasetId, updatedIndex);
    
    const endTime = performance.now();
    console.log(`Search index updated for ${datasetId}: ${endTime - startTime}ms`);
  }

  /**
   * Perform optimized search with debouncing
   */
  createDebouncedSearch(datasetId: string): Observable<string> {
    if (!this.searchSubjects.has(datasetId)) {
      const subject = new Subject<string>();
      this.searchSubjects.set(datasetId, subject);
    }

    return this.searchSubjects.get(datasetId)!.pipe(
      debounceTime(this.SEARCH_DEBOUNCE_TIME),
      distinctUntilChanged()
    );
  }

  /**
   * Trigger search with debouncing
   */
  triggerSearch(datasetId: string, searchTerm: string): void {
    const subject = this.searchSubjects.get(datasetId);
    if (subject) {
      subject.next(searchTerm);
    }
  }

  /**
   * Fast search using pre-built index
   */
  searchItems(datasetId: string, searchTerm: string): MaintenanceJob[] {
    const startTime = performance.now();
    
    if (!searchTerm.trim()) {
      return [];
    }

    const searchIndex = this.searchIndexes.get(datasetId);
    if (!searchIndex) {
      return [];
    }

    const searchTermLower = searchTerm.toLowerCase();
    const results = searchIndex
      .filter(indexItem => indexItem.searchableText.includes(searchTermLower))
      .map(indexItem => indexItem.originalItem);

    const endTime = performance.now();
    this.updatePerformanceMetrics(datasetId, 'search', endTime - startTime, searchIndex.length, results.length);

    return results;
  }

  /**
   * Optimized filtering with caching
   */
  filterItems(datasetId: string, items: MaintenanceJob[], filters: JobFilters): MaintenanceJob[] {
    const startTime = performance.now();
    
    // Create cache key from filters
    const cacheKey = this.createFilterCacheKey(datasetId, filters);
    
    // Check cache first
    const cachedResult = this.filterCache.get(cacheKey);
    if (cachedResult) {
      return cachedResult;
    }

    // Apply filters
    let filteredItems = [...items];

    // Date range filter
    if (filters.dateRange) {
      filteredItems = filteredItems.filter(job => {
        const jobDate = new Date(job.scheduledDate);
        return jobDate >= filters.dateRange!.start && jobDate <= filters.dateRange!.end;
      });
    }

    // Status filter
    if (filters.status && filters.status.length > 0) {
      const statusSet = new Set(filters.status);
      filteredItems = filteredItems.filter(job => statusSet.has(job.status));
    }

    // Machine type filter
    if (filters.machineType && filters.machineType.length > 0) {
      const machineTypeSet = new Set(filters.machineType);
      filteredItems = filteredItems.filter(job => {
        const machineType = this.extractMachineType(job.machineName);
        return machineTypeSet.has(machineType);
      });
    }

    // Project filter
    if (filters.project && filters.project.length > 0) {
      const projectSet = new Set(filters.project);
      filteredItems = filteredItems.filter(job => projectSet.has(job.project));
    }

    // Assigned to filter
    if (filters.assignedTo && filters.assignedTo.length > 0) {
      const assignedToSet = new Set(filters.assignedTo);
      filteredItems = filteredItems.filter(job => 
        job.assignedTo.some(tech => assignedToSet.has(tech))
      );
    }

    // Cache result
    this.filterCache.set(cacheKey, filteredItems);
    
    // Clean cache if it gets too large
    if (this.filterCache.size > 100) {
      this.clearOldCacheEntries();
    }

    const endTime = performance.now();
    this.updatePerformanceMetrics(datasetId, 'filter', endTime - startTime, items.length, filteredItems.length);

    return filteredItems;
  }

  /**
   * Combined search and filter operation
   */
  searchAndFilter(datasetId: string, items: MaintenanceJob[], searchTerm: string, filters: JobFilters): MaintenanceJob[] {
    const startTime = performance.now();
    
    let result: MaintenanceJob[];

    if (searchTerm.trim()) {
      // Search first, then filter
      const searchResults = this.searchItems(datasetId, searchTerm);
      result = this.filterItems(datasetId, searchResults, filters);
    } else {
      // Just filter
      result = this.filterItems(datasetId, items, filters);
    }

    const endTime = performance.now();
    console.log(`Combined search and filter for ${datasetId}: ${endTime - startTime}ms, ${result.length} results`);

    return result;
  }

  /**
   * Get performance metrics
   */
  getPerformanceMetrics(): Map<string, FilterPerformanceMetrics> {
    return this.performanceMetrics();
  }

  /**
   * Clear all caches
   */
  clearCaches(): void {
    this.filterCache.clear();
    this.searchIndexes.clear();
  }

  /**
   * Clear caches for specific dataset
   */
  clearDatasetCaches(datasetId: string): void {
    // Clear filter cache entries for this dataset
    const keysToDelete = Array.from(this.filterCache.keys())
      .filter(key => key.startsWith(datasetId));
    keysToDelete.forEach(key => this.filterCache.delete(key));

    // Clear search index
    this.searchIndexes.delete(datasetId);
  }

  private createSearchableText(item: MaintenanceJob): string {
    return [
      item.machineName,
      item.serialNumber,
      item.project,
      item.reason,
      item.type,
      item.status,
      ...item.assignedTo
    ].join(' ').toLowerCase();
  }

  private createFilterCacheKey(datasetId: string, filters: JobFilters): string {
    const filterParts = [
      datasetId,
      filters.dateRange ? `${filters.dateRange.start.getTime()}-${filters.dateRange.end.getTime()}` : '',
      filters.status ? filters.status.sort().join(',') : '',
      filters.machineType ? filters.machineType.sort().join(',') : '',
      filters.project ? filters.project.sort().join(',') : '',
      filters.assignedTo ? filters.assignedTo.sort().join(',') : ''
    ];
    return filterParts.join('|');
  }

  private extractMachineType(machineName: string): string {
    const match = machineName.match(/^([A-Za-z\s]+)/);
    return match ? match[1].trim() : machineName;
  }

  private clearOldCacheEntries(): void {
    // Keep only the 50 most recent cache entries
    const entries = Array.from(this.filterCache.entries());
    const entriesToKeep = entries.slice(-50);
    
    this.filterCache.clear();
    entriesToKeep.forEach(([key, value]) => {
      this.filterCache.set(key, value);
    });
  }

  private updatePerformanceMetrics(
    datasetId: string, 
    operation: 'search' | 'filter', 
    time: number, 
    totalItems: number, 
    resultItems: number
  ): void {
    // Schedule metrics update outside of any computed contexts to avoid NG0600
    queueMicrotask(() => {
      this.performanceMetrics.update(metrics => {
        const newMetrics = new Map(metrics);
        const existing = newMetrics.get(datasetId) || {
          filterTime: 0,
          searchTime: 0,
          totalItems: 0,
          filteredItems: 0,
          lastUpdate: new Date()
        };

        const updated = {
          ...existing,
          [operation === 'search' ? 'searchTime' : 'filterTime']: time,
          totalItems,
          filteredItems: resultItems,
          lastUpdate: new Date()
        };

        newMetrics.set(datasetId, updated);
        return newMetrics;
      });
    });
  }
}