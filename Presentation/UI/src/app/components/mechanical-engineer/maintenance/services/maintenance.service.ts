import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, finalize, tap, switchMap, map } from 'rxjs/operators';
import { MaintenanceErrorHandlerService } from './maintenance-error-handler.service';
import { MaintenanceMockService } from './maintenance-mock.service';
import { MaintenanceLoadingService } from './maintenance-loading.service';
import { PerformanceOptimizationService } from './performance-optimization.service';
import { OfflineStorageService } from './offline-storage.service';
import { SyncService } from './sync.service';
import { environment } from '../../../../../environments/environment';
import {
  MaintenanceJob,
  MaintenanceAlert,
  MaintenanceStats,
  MachineMaintenanceHistory,
  ServiceIntervalConfig,
  NotificationPreferences,
  JobFilters,
  MaintenanceStatus,
  ServiceComplianceData,
  MTBFMetrics,
  PartsUsageData,
  UsageMetrics
} from '../models/maintenance.models';

@Injectable({
  providedIn: 'root'
})
export class MaintenanceService {
  private http = inject(HttpClient);
  private errorHandler = inject(MaintenanceErrorHandlerService);
  private mockService = inject(MaintenanceMockService);
  private loadingService = inject(MaintenanceLoadingService);
  private performanceService = inject(PerformanceOptimizationService);
  private offlineStorage = inject(OfflineStorageService);
  private syncService = inject(SyncService);
  private readonly apiUrl = `${environment.apiUrl}/api/maintenance`;
  private readonly jobsApiUrl = `${environment.apiUrl}/api/maintenance-jobs`;
  private readonly reportsApiUrl = `${environment.apiUrl}/api/maintenance-reports`;

  // Dashboard and Stats - Using mock data for development with offline support
  getMaintenanceStats(): Observable<MaintenanceStats> {
    const operationId = 'maintenance-stats';
    this.loadingService.startLoading(operationId, 'Loading maintenance statistics...');

    // Check if offline and return cached data
    if (this.offlineStorage.isOffline()) {
      const cachedStats = this.offlineStorage.getCachedMaintenanceStats();
      this.loadingService.stopLoading(operationId);

      if (cachedStats) {
        return of(cachedStats);
      } else {
        return throwError(() => new Error('No cached maintenance statistics available offline'));
      }
    }

    // Use real API with fallback to mock in dev mode
    return this.http.get<MaintenanceStats>(`${this.jobsApiUrl}/stats/region`).pipe(
      tap(stats => this.offlineStorage.storeOfflineData({ maintenanceStats: stats })),
      catchError(error => {
        console.warn('API call failed, falling back to mock data:', error);
        return this.mockService.getMaintenanceStats();
      }),
      finalize(() => this.loadingService.stopLoading(operationId))
    );
  }

  getServiceDueAlerts(): Observable<MaintenanceAlert[]> {
    // Check if offline and return cached data
    if (this.offlineStorage.isOffline()) {
      const cachedAlerts = this.offlineStorage.getCachedAlerts();
      return of(cachedAlerts.serviceDue);
    }

    // Use real API with fallback to mock in dev mode
    return this.http.get<MaintenanceAlert[]>(`${this.apiUrl}/alerts/service-due`).pipe(
      tap(alerts => this.offlineStorage.storeOfflineData({ serviceDueAlerts: alerts })),
      catchError(error => {
        console.warn('Service due alerts API failed, falling back to mock:', error);
        return this.mockService.getServiceDueAlerts();
      })
    );
  }

  getOverdueAlerts(): Observable<MaintenanceAlert[]> {
    // Check if offline and return cached data
    if (this.offlineStorage.isOffline()) {
      const cachedAlerts = this.offlineStorage.getCachedAlerts();
      return of(cachedAlerts.overdue);
    }

    // Use real API with fallback to mock in dev mode
    return this.http.get<MaintenanceAlert[]>(`${this.jobsApiUrl}/overdue`).pipe(
      tap(alerts => this.offlineStorage.storeOfflineData({ overdueAlerts: alerts })),
      catchError(error => {
        console.warn('Overdue alerts API failed, falling back to mock:', error);
        return this.mockService.getOverdueAlerts();
      })
    );
  }

  // Maintenance Jobs - Using mock data for development with offline support
  getMaintenanceJobs(filters?: JobFilters): Observable<MaintenanceJob[]> {
    const operationId = 'maintenance-jobs';
    this.loadingService.startLoading(operationId, 'Loading maintenance jobs...');

    // Check if offline and return cached data
    if (this.offlineStorage.isOffline()) {
      const cachedJobs = this.offlineStorage.getCachedMaintenanceJobs();
      if (cachedJobs.length > 0) {
        this.loadingService.stopLoading(operationId);
        // Create search index for performance optimization
        this.performanceService.createSearchIndex('maintenance-jobs', cachedJobs);
        return of(cachedJobs);
      }
      // Fallback to mock data even when offline (dev mode)
    }

    // Use real API - fetch jobs for current engineer
    // Note: Frontend should get current user ID from auth service
    const userId = this.getCurrentUserId();
    if (!userId) {
      // Fallback to mock if no user context
      return this.mockService.getMaintenanceJobs(filters).pipe(
        tap(jobs => {
          this.offlineStorage.storeOfflineData({ maintenanceJobs: jobs });
          this.performanceService.createSearchIndex('maintenance-jobs', jobs);
        }),
        finalize(() => this.loadingService.stopLoading(operationId))
      );
    }

    const params = this.buildFilterParams(filters);
    return this.http.get<MaintenanceJob[]>(`${this.jobsApiUrl}/engineer/${userId}`, { params }).pipe(
      tap(jobs => {
        this.offlineStorage.storeOfflineData({ maintenanceJobs: jobs });
        this.performanceService.createSearchIndex('maintenance-jobs', jobs);
      }),
      catchError(error => {
        console.warn('Get jobs API failed, falling back to mock:', error);
        return this.mockService.getMaintenanceJobs(filters);
      }),
      finalize(() => this.loadingService.stopLoading(operationId))
    );
  }

  getMaintenanceJob(jobId: string): Observable<MaintenanceJob> {
    return this.http.get<MaintenanceJob>(`${this.jobsApiUrl}/${jobId}`).pipe(
      catchError(error => {
        console.warn('Get job by ID API failed, falling back to mock:', error);
        return this.mockService.getMaintenanceJob(jobId);
      })
    );
  }

  createMaintenanceJob(job: Partial<MaintenanceJob>): Observable<MaintenanceJob> {
    // If offline, queue the operation for sync
    if (this.offlineStorage.isOffline()) {
      this.syncService.queueJobCreation(job);
      // Return a temporary job with generated ID for optimistic updates
      const tempJob: MaintenanceJob = {
        id: `temp-${Date.now()}`,
        ...job
      } as MaintenanceJob;
      return of(tempJob);
    }

    return this.http.post<MaintenanceJob>(`${this.jobsApiUrl}/create`, job)
      .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  updateMaintenanceJob(jobId: string, job: Partial<MaintenanceJob>): Observable<MaintenanceJob> {
    // If offline, queue the operation for sync
    if (this.offlineStorage.isOffline()) {
      this.syncService.queueJobUpdate(jobId, job);
      // Return updated job for optimistic updates
      const updatedJob: MaintenanceJob = {
        id: jobId,
        ...job
      } as MaintenanceJob;
      return of(updatedJob);
    }

    return this.http.patch<MaintenanceJob>(`${this.jobsApiUrl}/${jobId}/status`, job)
      .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  updateJobStatus(jobId: string, status: MaintenanceStatus): Observable<void> {
    const operationId = `update-job-${jobId}`;
    this.loadingService.startLoading(operationId, 'Updating job status...');

    // If offline, queue the operation for sync
    if (this.offlineStorage.isOffline()) {
      this.syncService.queueJobStatusUpdate(jobId, status);
      this.loadingService.stopLoading(operationId);
      return of(void 0);
    }

    // TODO: Replace with actual API call when backend is ready
    return this.mockService.updateJobStatus(jobId, status).pipe(
      finalize(() => this.loadingService.stopLoading(operationId))
    );
    // return this.http.patch<void>(`${this.apiUrl}/jobs/${jobId}/status`, { status })
    //   .pipe(
    //     catchError(this.errorHandler.handleError.bind(this.errorHandler)),
    //     finalize(() => this.loadingService.stopLoading(operationId))
    //   );
  }

  deleteMaintenanceJob(jobId: string): Observable<void> {
    // If offline, queue the operation for sync
    if (this.offlineStorage.isOffline()) {
      this.syncService.queueJobDeletion(jobId);
      return of(void 0);
    }

    return this.http.delete<void>(`${this.apiUrl}/jobs/${jobId}`)
      .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  // Bulk operations - Using mock data for development
  bulkUpdateJobStatus(jobIds: string[], status: MaintenanceStatus): Observable<void> {
    // TODO: Replace with actual API call when backend is ready
    // For now, update each job individually using mock service
    return new Observable(observer => {
      Promise.all(jobIds.map(id => this.mockService.updateJobStatus(id, status).toPromise()))
        .then(() => {
          observer.next();
          observer.complete();
        })
        .catch(error => observer.error(error));
    });
    // return this.http.patch<void>(`${this.apiUrl}/jobs/bulk/status`, { jobIds, status })
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  bulkAssignJobs(jobIds: string[], assignedTo: string[]): Observable<void> {
    // TODO: Replace with actual API call when backend is ready
    return of(void 0);  // Return void until backend is ready
    // return this.http.patch<void>(`${this.apiUrl}/jobs/bulk/assign`, { jobIds, assignedTo })
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  // Machine History
  getMachineMaintenanceHistory(machineId: string): Observable<MachineMaintenanceHistory> {
    // TODO: Replace with actual API call when backend is ready
    return of({} as MachineMaintenanceHistory);  // Return empty object until backend is ready
    // return this.http.get<MachineMaintenanceHistory>(`${this.apiUrl}/machines/${machineId}/history`)
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  // Calendar and Timeline - Using mock data for development
  getMaintenanceJobsByDateRange(startDate: Date, endDate: Date): Observable<MaintenanceJob[]> {
    // TODO: Replace with actual API call when backend is ready
    return this.mockService.getMaintenanceJobsByDateRange(startDate, endDate);
    // const params = new HttpParams()
    //   .set('startDate', startDate.toISOString())
    //   .set('endDate', endDate.toISOString());
    // 
    // return this.http.get<MaintenanceJob[]>(`${this.apiUrl}/jobs/date-range`, { params })
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  // Analytics - Using mock data for development
  getServiceComplianceData(): Observable<ServiceComplianceData> {
    // TODO: Replace with actual API call when backend is ready
    return this.mockService.getServiceComplianceData();
    // return this.http.get<ServiceComplianceData>(`${this.apiUrl}/analytics/service-compliance`)
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  getMTBFMetrics(): Observable<MTBFMetrics[]> {
    // TODO: Replace with actual API call when backend is ready
    return this.mockService.getMTBFMetrics();
    // return this.http.get<MTBFMetrics[]>(`${this.apiUrl}/analytics/mtbf`)
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  getPartsUsageData(): Observable<PartsUsageData[]> {
    // TODO: Replace with actual API call when backend is ready
    return this.mockService.getPartsUsageData();
    // return this.http.get<PartsUsageData[]>(`${this.apiUrl}/analytics/parts-usage`)
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  getUsageMetrics(machineId?: string): Observable<UsageMetrics[]> {
    // TODO: Replace with actual API call when backend is ready
    return this.mockService.getUsageMetrics(machineId);
    // const params = machineId ? new HttpParams().set('machineId', machineId) : new HttpParams();
    // return this.http.get<UsageMetrics[]>(`${this.apiUrl}/analytics/usage-metrics`, { params })
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  // Configuration
  getServiceIntervalConfigs(): Observable<ServiceIntervalConfig[]> {
    // TODO: Replace with actual API call when backend is ready
    return of([]);  // Return empty array until backend is ready
    // return this.http.get<ServiceIntervalConfig[]>(`${this.apiUrl}/config/service-intervals`)
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  updateServiceIntervalConfig(config: ServiceIntervalConfig): Observable<ServiceIntervalConfig> {
    // TODO: Replace with actual API call when backend is ready
    return of(config);  // Return config until backend is ready
    // return this.http.put<ServiceIntervalConfig>(`${this.apiUrl}/config/service-intervals`, config)
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  getNotificationPreferences(): Observable<NotificationPreferences> {
    // TODO: Replace with actual API call when backend is ready
    return this.mockService.getNotificationPreferences();
    // return this.http.get<NotificationPreferences>(`${this.apiUrl}/config/notifications`)
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  updateNotificationPreferences(preferences: NotificationPreferences): Observable<NotificationPreferences> {
    // TODO: Replace with actual API call when backend is ready
    return this.mockService.updateNotificationPreferences(preferences);
    // return this.http.put<NotificationPreferences>(`${this.apiUrl}/config/notifications`, preferences)
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  // File Upload
  uploadMaintenanceFile(jobId: string, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);

    // TODO: Replace with actual API call when backend is ready
    return of({ success: true });  // Return success until backend is ready
    // return this.http.post(`${this.apiUrl}/jobs/${jobId}/files`, formData)
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  deleteMaintenanceFile(jobId: string, fileId: string): Observable<void> {
    // TODO: Replace with actual API call when backend is ready
    return of(void 0);  // Return void until backend is ready
    // return this.http.delete<void>(`${this.apiUrl}/jobs/${jobId}/files/${fileId}`)
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  // Search and Filtering - Using mock data for development
  searchMaintenanceJobs(searchTerm: string): Observable<MaintenanceJob[]> {
    // TODO: Replace with actual API call when backend is ready
    return this.mockService.searchMaintenanceJobs(searchTerm);
    // const params = new HttpParams().set('search', searchTerm);
    // return this.http.get<MaintenanceJob[]>(`${this.apiUrl}/jobs/search`, { params })
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  // Helper Methods
  private buildFilterParams(filters?: JobFilters): HttpParams {
    let params = new HttpParams();

    if (!filters) return params;

    if (filters.dateRange) {
      params = params.set('startDate', filters.dateRange.start.toISOString());
      params = params.set('endDate', filters.dateRange.end.toISOString());
    }

    if (filters.status && filters.status.length > 0) {
      params = params.set('status', filters.status.join(','));
    }

    if (filters.machineType && filters.machineType.length > 0) {
      params = params.set('machineType', filters.machineType.join(','));
    }

    if (filters.project && filters.project.length > 0) {
      params = params.set('project', filters.project.join(','));
    }

    if (filters.assignedTo && filters.assignedTo.length > 0) {
      params = params.set('assignedTo', filters.assignedTo.join(','));
    }

    if (filters.searchTerm) {
      params = params.set('search', filters.searchTerm);
    }

    return params;
  }

  // Performance Optimization Methods

  /**
   * Get optimized search results with debouncing
   * This method should be used by components that want debounced search functionality
   */
  getOptimizedSearchResults(): Observable<string> {
    // Return the debounced search observable
    return this.performanceService.createDebouncedSearch('maintenance-jobs');
  }

  /**
   * Trigger a debounced search
   */
  triggerOptimizedSearch(searchTerm: string): void {
    this.performanceService.triggerSearch('maintenance-jobs', searchTerm);
  }

  /**
   * Search maintenance jobs with performance optimization
   */
  searchMaintenanceJobsOptimized(searchTerm: string): MaintenanceJob[] {
    return this.performanceService.searchItems('maintenance-jobs', searchTerm);
  }

  /**
   * Filter maintenance jobs with caching
   */
  filterMaintenanceJobsOptimized(jobs: MaintenanceJob[], filters: JobFilters): MaintenanceJob[] {
    return this.performanceService.filterItems('maintenance-jobs', jobs, filters);
  }

  /**
   * Combined search and filter with optimization
   */
  searchAndFilterJobs(jobs: MaintenanceJob[], searchTerm: string, filters: JobFilters): MaintenanceJob[] {
    return this.performanceService.searchAndFilter('maintenance-jobs', jobs, searchTerm, filters);
  }

  /**
   * Get loading state for specific operation
   */
  getLoadingState(operationId: string) {
    return this.loadingService.getLoadingState(operationId);
  }

  /**
   * Get loading signal for reactive components
   */
  getLoadingSignal(operationId: string) {
    return this.loadingService.getLoadingSignal(operationId);
  }

  /**
   * Get performance metrics
   */
  getPerformanceMetrics() {
    return this.performanceService.getPerformanceMetrics();
  }

  /**
   * Clear performance caches
   */
  clearPerformanceCaches(): void {
    this.performanceService.clearCaches();
  }

  // Offline and Sync Methods

  /**
   * Get offline status and sync information
   */
  getOfflineStatus() {
    return {
      ...this.offlineStorage.getOfflineStatus(),
      ...this.syncService.getSyncStatus()
    };
  }

  /**
   * Force sync when online
   */
  forceSyncIfOnline(): Observable<any> {
    if (this.offlineStorage.isOffline()) {
      return throwError(() => new Error('Cannot sync while offline'));
    }

    return this.syncService.performFullSync();
  }

  /**
   * Enable/disable auto-sync
   */
  setAutoSyncEnabled(enabled: boolean): void {
    this.syncService.setAutoSyncEnabled(enabled);
  }

  /**
   * Sync fresh data from server
   */
  syncMaintenanceData(): Observable<void> {
    return this.syncService.syncMaintenanceData();
  }

  /**
   * Check if data is available offline
   */
  hasOfflineData(): boolean {
    return this.offlineStorage.hasOfflineData();
  }

  /**
   * Clear all offline data and sync queue
   */
  clearOfflineData(): void {
    this.offlineStorage.clearOfflineData();
    this.offlineStorage.clearSyncQueue();
  }

  /**
   * Get current user ID from local storage or auth service
   * This should ideally come from an AuthService
   */
  private getCurrentUserId(): number | null {
    try {
      const userStr = localStorage.getItem('user');
      if (userStr) {
        const user = JSON.parse(userStr);
        return user.id || null;
      }
      return null;
    } catch {
      return null;
    }
  }
}