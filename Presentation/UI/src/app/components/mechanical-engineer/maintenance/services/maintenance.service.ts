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

  // Dashboard and Stats - Connected to backend API
  getMaintenanceStats(regionId?: number): Observable<MaintenanceStats> {
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

    // Get region ID from user if not provided
    const effectiveRegionId = regionId || this.getUserRegionId();
    if (!effectiveRegionId) {
      console.warn('No region ID found, falling back to mock data');
      return this.mockService.getMaintenanceStats().pipe(
        finalize(() => this.loadingService.stopLoading(operationId))
      );
    }

    // Call real backend API
    return this.http.get<MaintenanceStats>(`${this.jobsApiUrl}/stats/region/${effectiveRegionId}`).pipe(
      tap(stats => this.offlineStorage.storeOfflineData({ maintenanceStats: stats })),
      catchError(error => {
        console.error('Get maintenance stats API failed:', error);
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

  // Maintenance Jobs - Connected to real backend API
  getMaintenanceJobs(filters?: JobFilters, forceRefresh: boolean = false): Observable<MaintenanceJob[]> {
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

    // Get current user ID from auth service
    const userId = this.getCurrentUserId();
    if (!userId) {
      // Fallback to mock if no user context
      console.warn('No user ID found, falling back to mock data');
      return this.mockService.getMaintenanceJobs(filters).pipe(
        tap(jobs => {
          this.offlineStorage.storeOfflineData({ maintenanceJobs: jobs });
          this.performanceService.createSearchIndex('maintenance-jobs', jobs);
        }),
        finalize(() => this.loadingService.stopLoading(operationId))
      );
    }

    // Build query params
    let params = this.buildFilterParams(filters);

    // Add cache busting parameter when force refresh is requested
    if (forceRefresh) {
      params = params.set('_t', Date.now().toString());
    }

    // Configure request options with cache control headers for force refresh
    const options = {
      params,
      headers: forceRefresh ? {
        'Cache-Control': 'no-cache, no-store, must-revalidate',
        'Pragma': 'no-cache',
        'Expires': '0'
      } : undefined
    };

    // Call real backend API
    return this.http.get<MaintenanceJob[]>(`${this.jobsApiUrl}/engineer/${userId}`, options).pipe(
      map(jobs => this.transformJobDates(jobs)),
      tap(jobs => {
        this.offlineStorage.storeOfflineData({ maintenanceJobs: jobs });
        this.performanceService.createSearchIndex('maintenance-jobs', jobs);
      }),
      catchError(error => {
        console.error('Get jobs API failed:', error);
        // Fallback to mock data in development
        return this.mockService.getMaintenanceJobs(filters);
      }),
      finalize(() => this.loadingService.stopLoading(operationId))
    );
  }

  getMaintenanceJob(jobId: number): Observable<MaintenanceJob> {
    return this.http.get<MaintenanceJob>(`${this.jobsApiUrl}/${jobId}`).pipe(
      map(job => this.transformJobDates([job])[0]),
      catchError(error => {
        console.error('Get job by ID API failed:', error);
        return this.mockService.getMaintenanceJob(jobId.toString());
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

  updateMaintenanceJob(jobId: number, job: Partial<MaintenanceJob>): Observable<MaintenanceJob> {
    // If offline, queue the operation for sync
    if (this.offlineStorage.isOffline()) {
      this.syncService.queueJobUpdate(jobId.toString(), job);
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

  updateJobStatus(jobId: number, status: MaintenanceStatus): Observable<void> {
    const operationId = `update-job-${jobId}`;
    this.loadingService.startLoading(operationId, 'Updating job status...');

    // If offline, queue the operation for sync
    if (this.offlineStorage.isOffline()) {
      this.syncService.queueJobStatusUpdate(jobId.toString(), status);
      this.loadingService.stopLoading(operationId);
      return of(void 0);
    }

    // Call real backend API
    return this.http.patch<void>(`${this.jobsApiUrl}/${jobId}/status`, { status }).pipe(
      catchError(error => {
        console.error('Update job status API failed:', error);
        return this.errorHandler.handleError(error);
      }),
      finalize(() => this.loadingService.stopLoading(operationId))
    );
  }

  completeMaintenanceJob(jobId: number, observations: string, actualHours: number, partsReplaced?: string[]): Observable<MaintenanceJob> {
    const operationId = `complete-job-${jobId}`;
    this.loadingService.startLoading(operationId, 'Completing job...');

    // If offline, queue the operation for sync
    if (this.offlineStorage.isOffline()) {
      this.syncService.queueJobUpdate(jobId.toString(), {
        status: MaintenanceStatus.COMPLETED,
        observations,
        actualHours,
        partsReplaced,
        completedDate: new Date()
      });
      this.loadingService.stopLoading(operationId);
      return of({ id: jobId, observations, actualHours, partsReplaced } as MaintenanceJob);
    }

    // Call real backend API
    return this.http.post<MaintenanceJob>(`${this.jobsApiUrl}/${jobId}/complete`, {
      observations,
      actualHours,
      partsReplaced
    }).pipe(
      map(job => this.transformJobDates([job])[0]),
      catchError(error => {
        console.error('Complete job API failed:', error);
        return this.errorHandler.handleError(error);
      }),
      finalize(() => this.loadingService.stopLoading(operationId))
    );
  }

  deleteMaintenanceJob(jobId: number): Observable<void> {
    // If offline, queue the operation for sync
    if (this.offlineStorage.isOffline()) {
      this.syncService.queueJobDeletion(jobId.toString());
      return of(void 0);
    }

    return this.http.delete<void>(`${this.apiUrl}/jobs/${jobId}`)
      .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  // Bulk operations - Connected to backend API
  bulkUpdateJobStatus(jobIds: number[], status: MaintenanceStatus): Observable<void> {
    // Call real backend API
    return this.http.post<void>(`${this.jobsApiUrl}/bulk-update-status`, { jobIds, status }).pipe(
      catchError(error => {
        console.error('Bulk update status API failed:', error);
        return this.errorHandler.handleError(error);
      })
    );
  }

  bulkAssignJobs(jobIds: number[], engineerId: number): Observable<void> {
    // Call real backend API
    return this.http.post<void>(`${this.jobsApiUrl}/bulk-assign`, { jobIds, engineerId }).pipe(
      catchError(error => {
        console.error('Bulk assign API failed:', error);
        return this.errorHandler.handleError(error);
      })
    );
  }

  // Machine History
  getMachineMaintenanceHistory(machineId: number): Observable<MachineMaintenanceHistory> {
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
  uploadMaintenanceFile(jobId: number, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);

    // TODO: Replace with actual API call when backend is ready
    return of({ success: true });  // Return success until backend is ready
    // return this.http.post(`${this.apiUrl}/jobs/${jobId}/files`, formData)
    //   .pipe(catchError(this.errorHandler.handleError.bind(this.errorHandler)));
  }

  deleteMaintenanceFile(jobId: number, fileId: string): Observable<void> {
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

  /**
   * Get user's region ID from local storage or auth service
   */
  private getUserRegionId(): number | null {
    try {
      const userStr = localStorage.getItem('user');
      if (userStr) {
        const user = JSON.parse(userStr);
        return user.regionId || null;
      }
      return null;
    } catch {
      return null;
    }
  }

  /**
   * Transform date strings from API to Date objects and map backend fields to frontend
   */
  private transformJobDates(jobs: MaintenanceJob[]): MaintenanceJob[] {
    return jobs.map(job => ({
      ...job,
      // Map projectName to project for backward compatibility with existing components
      project: job.projectName || '',
      // Ensure serialNumber is defined
      serialNumber: job.serialNumber || '',
      // Ensure assignedTo array is populated from assignments
      assignedTo: job.assignments?.map(a => a.mechanicalEngineerName || `Engineer ${a.mechanicalEngineerId}`) || [],
      // Transform dates
      scheduledDate: new Date(job.scheduledDate),
      completedDate: job.completedDate ? new Date(job.completedDate) : undefined,
      createdAt: job.createdAt ? new Date(job.createdAt) : undefined,
      updatedAt: job.updatedAt ? new Date(job.updatedAt) : undefined,
      assignments: job.assignments?.map(a => ({
        ...a,
        assignedAt: new Date(a.assignedAt)
      }))
    }));
  }
}