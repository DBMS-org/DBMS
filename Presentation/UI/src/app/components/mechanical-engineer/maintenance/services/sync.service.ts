import { Injectable, inject, signal, Injector } from '@angular/core';
import { Observable, from, of, throwError, timer } from 'rxjs';
import { switchMap, catchError, retry, tap } from 'rxjs/operators';
import { MaintenanceService } from './maintenance.service';
import { OfflineStorageService, SyncQueueItem } from './offline-storage.service';
import { MaintenanceJob, MaintenanceStatus } from '../models/maintenance.models';

export interface SyncResult {
  success: boolean;
  syncedItems: number;
  failedItems: number;
  errors: string[];
}

export interface SyncProgress {
  total: number;
  completed: number;
  current: string;
  percentage: number;
}

@Injectable({
  providedIn: 'root'
})
export class SyncService {
  private injector = inject(Injector);
  private offlineStorage = inject(OfflineStorageService);

  // Sync state signals
  isSyncing = signal(false);
  syncProgress = signal<SyncProgress>({ total: 0, completed: 0, current: '', percentage: 0 });
  lastSyncResult = signal<SyncResult | null>(null);

  // Auto-sync configuration
  private readonly AUTO_SYNC_INTERVAL = 5 * 60 * 1000; // 5 minutes
  private readonly RETRY_DELAY = 2000; // 2 seconds
  private readonly MAX_RETRIES = 3;

  private autoSyncTimer: any;

  constructor() {
    this.initializeAutoSync();
  }

  private getMaintenanceService(): MaintenanceService {
    return this.injector.get(MaintenanceService);
  }

  /**
   * Perform full synchronization
   */
  performFullSync(): Observable<SyncResult> {
    if (this.isSyncing()) {
      return throwError(() => new Error('Sync already in progress'));
    }

    if (this.offlineStorage.isOffline()) {
      return throwError(() => new Error('Cannot sync while offline'));
    }

    this.isSyncing.set(true);
    const syncQueue = this.offlineStorage.getSyncQueue();
    
    if (syncQueue.length === 0) {
      this.isSyncing.set(false);
      const result: SyncResult = {
        success: true,
        syncedItems: 0,
        failedItems: 0,
        errors: []
      };
      this.lastSyncResult.set(result);
      return of(result);
    }

    // Initialize progress
    this.syncProgress.set({
      total: syncQueue.length,
      completed: 0,
      current: 'Starting sync...',
      percentage: 0
    });

    return from(this.processSyncQueue(syncQueue)).pipe(
      tap(result => {
        this.lastSyncResult.set(result);
        this.isSyncing.set(false);
        
        // Reset progress
        this.syncProgress.set({
          total: 0,
          completed: 0,
          current: '',
          percentage: 0
        });
      }),
      catchError(error => {
        this.isSyncing.set(false);
        this.syncProgress.set({
          total: 0,
          completed: 0,
          current: '',
          percentage: 0
        });
        return throwError(() => error);
      })
    );
  }

  /**
   * Sync specific data types
   */
  syncMaintenanceData(): Observable<void> {
    if (this.offlineStorage.isOffline()) {
      return throwError(() => new Error('Cannot sync while offline'));
    }

    return this.getMaintenanceService().getMaintenanceJobs().pipe(
      switchMap(jobs => {
        // Store fresh data
        this.offlineStorage.storeOfflineData({ maintenanceJobs: jobs });
        
        // Also sync stats and alerts
        return this.getMaintenanceService().getMaintenanceStats().pipe(
          switchMap(stats => {
            this.offlineStorage.storeOfflineData({ maintenanceStats: stats });
            
            return this.getMaintenanceService().getServiceDueAlerts().pipe(
              switchMap(serviceDue => {
                return this.getMaintenanceService().getOverdueAlerts().pipe(
                  tap(overdue => {
                    this.offlineStorage.storeOfflineData({
                      serviceDueAlerts: serviceDue,
                      overdueAlerts: overdue
                    });
                  })
                );
              })
            );
          })
        );
      }),
      switchMap(() => of(void 0))
    );
  }

  /**
   * Queue job status update for sync
   */
  queueJobStatusUpdate(jobId: string, status: MaintenanceStatus): void {
    this.offlineStorage.addToSyncQueue({
      type: 'job-status-update',
      data: { jobId, status }
    });
  }

  /**
   * Queue job creation for sync
   */
  queueJobCreation(job: Partial<MaintenanceJob>): void {
    this.offlineStorage.addToSyncQueue({
      type: 'job-create',
      data: job
    });
  }

  /**
   * Queue job update for sync
   */
  queueJobUpdate(jobId: string, job: Partial<MaintenanceJob>): void {
    this.offlineStorage.addToSyncQueue({
      type: 'job-update',
      data: { jobId, job }
    });
  }

  /**
   * Queue job deletion for sync
   */
  queueJobDeletion(jobId: string): void {
    this.offlineStorage.addToSyncQueue({
      type: 'job-delete',
      data: { jobId }
    });
  }

  /**
   * Get sync status
   */
  getSyncStatus() {
    return {
      isSyncing: this.isSyncing(),
      progress: this.syncProgress(),
      lastResult: this.lastSyncResult(),
      offlineStatus: this.offlineStorage.getOfflineStatus()
    };
  }

  /**
   * Enable/disable auto-sync
   */
  setAutoSyncEnabled(enabled: boolean): void {
    if (enabled) {
      this.startAutoSync();
    } else {
      this.stopAutoSync();
    }
  }

  /**
   * Force immediate sync if online
   */
  forceSyncIfOnline(): void {
    if (!this.offlineStorage.isOffline() && this.offlineStorage.hasPendingSync()) {
      this.performFullSync().subscribe({
        next: (result) => {
          console.log('Force sync completed:', result);
        },
        error: (error) => {
          console.error('Force sync failed:', error);
        }
      });
    }
  }

  private async processSyncQueue(queue: SyncQueueItem[]): Promise<SyncResult> {
    const result: SyncResult = {
      success: true,
      syncedItems: 0,
      failedItems: 0,
      errors: []
    };

    for (let i = 0; i < queue.length; i++) {
      const item = queue[i];
      
      // Update progress
      this.syncProgress.set({
        total: queue.length,
        completed: i,
        current: `Syncing ${item.type}...`,
        percentage: (i / queue.length) * 100
      });

      try {
        await this.processSyncItem(item);
        this.offlineStorage.removeFromSyncQueue(item.id);
        result.syncedItems++;
      } catch (error) {
        console.error(`Failed to sync item ${item.id}:`, error);
        this.offlineStorage.updateSyncItemRetryCount(item.id);
        result.failedItems++;
        result.errors.push(`${item.type}: ${error}`);
        result.success = false;
      }
    }

    // Final progress update
    this.syncProgress.set({
      total: queue.length,
      completed: queue.length,
      current: 'Sync completed',
      percentage: 100
    });

    return result;
  }

  private async processSyncItem(item: SyncQueueItem): Promise<void> {
    switch (item.type) {
      case 'job-status-update':
        await this.getMaintenanceService().updateJobStatus(
          item.data.jobId,
          item.data.status
        ).toPromise();
        break;

      case 'job-create':
        await this.getMaintenanceService().createMaintenanceJob(item.data).toPromise();
        break;

      case 'job-update':
        await this.getMaintenanceService().updateMaintenanceJob(
          item.data.jobId,
          item.data.job
        ).toPromise();
        break;

      case 'job-delete':
        await this.getMaintenanceService().deleteMaintenanceJob(item.data.jobId).toPromise();
        break;

      default:
        throw new Error(`Unknown sync item type: ${item.type}`);
    }
  }

  private initializeAutoSync(): void {
    // Listen for online events to trigger sync
    window.addEventListener('online', () => {
      setTimeout(() => {
        this.forceSyncIfOnline();
      }, 1000); // Wait a second for connection to stabilize
    });

    this.startAutoSync();
  }

  private startAutoSync(): void {
    this.stopAutoSync(); // Clear any existing timer
    
    this.autoSyncTimer = setInterval(() => {
      if (!this.offlineStorage.isOffline() && 
          this.offlineStorage.hasPendingSync() && 
          !this.isSyncing()) {
        
        this.performFullSync().subscribe({
          next: (result) => {
            console.log('Auto-sync completed:', result);
          },
          error: (error) => {
            console.error('Auto-sync failed:', error);
          }
        });
      }
    }, this.AUTO_SYNC_INTERVAL);
  }

  private stopAutoSync(): void {
    if (this.autoSyncTimer) {
      clearInterval(this.autoSyncTimer);
      this.autoSyncTimer = null;
    }
  }
}