import { Injectable, signal } from '@angular/core';
import { MaintenanceJob, MaintenanceStats, MaintenanceAlert } from '../models/maintenance.models';

export interface OfflineData {
  maintenanceJobs: MaintenanceJob[];
  maintenanceStats: MaintenanceStats | null;
  serviceDueAlerts: MaintenanceAlert[];
  overdueAlerts: MaintenanceAlert[];
  lastSync: Date;
  version: number;
}

export interface SyncQueueItem {
  id: string;
  type: 'job-status-update' | 'job-create' | 'job-update' | 'job-delete';
  data: any;
  timestamp: Date;
  retryCount: number;
}

@Injectable({
  providedIn: 'root'
})
export class OfflineStorageService {
  private readonly STORAGE_KEY = 'maintenance-offline-data';
  private readonly SYNC_QUEUE_KEY = 'maintenance-sync-queue';
  private readonly MAX_RETRY_COUNT = 3;
  private readonly CACHE_EXPIRY_HOURS = 24;

  // Signals for reactive state
  isOffline = signal(false);
  hasPendingSync = signal(false);
  lastSyncTime = signal<Date | null>(null);

  constructor() {
    this.initializeOfflineDetection();
    this.checkPendingSync();
  }

  /**
   * Store maintenance data for offline access
   */
  storeOfflineData(data: Partial<OfflineData>): void {
    try {
      const existingData = this.getOfflineData();
      const updatedData: OfflineData = {
        maintenanceJobs: existingData?.maintenanceJobs || [],
        maintenanceStats: existingData?.maintenanceStats || null,
        serviceDueAlerts: existingData?.serviceDueAlerts || [],
        overdueAlerts: existingData?.overdueAlerts || [],
        ...existingData,
        ...data,
        lastSync: new Date(),
        version: (existingData?.version || 0) + 1
      };

      localStorage.setItem(this.STORAGE_KEY, JSON.stringify(updatedData));
      this.lastSyncTime.set(updatedData.lastSync);
    } catch (error) {
      console.error('Failed to store offline data:', error);
    }
  }

  /**
   * Retrieve offline data
   */
  getOfflineData(): OfflineData | null {
    try {
      const data = localStorage.getItem(this.STORAGE_KEY);
      if (!data) return null;

      const parsedData: OfflineData = JSON.parse(data);
      // Normalize date fields
      if (parsedData.lastSync && typeof parsedData.lastSync === 'string') {
        parsedData.lastSync = new Date(parsedData.lastSync);
      }
      
      // Check if data is expired
      if (this.isDataExpired(parsedData.lastSync)) {
        this.clearOfflineData();
        return null;
      }

      return parsedData;
    } catch (error) {
      console.error('Failed to retrieve offline data:', error);
      return null;
    }
  }

  /**
   * Get cached maintenance jobs
   */
  getCachedMaintenanceJobs(): MaintenanceJob[] {
    const data = this.getOfflineData();
    return data?.maintenanceJobs || [];
  }

  /**
   * Get cached maintenance stats
   */
  getCachedMaintenanceStats(): MaintenanceStats | null {
    const data = this.getOfflineData();
    return data?.maintenanceStats || null;
  }

  /**
   * Get cached alerts
   */
  getCachedAlerts(): { serviceDue: MaintenanceAlert[]; overdue: MaintenanceAlert[] } {
    const data = this.getOfflineData();
    return {
      serviceDue: data?.serviceDueAlerts || [],
      overdue: data?.overdueAlerts || []
    };
  }

  /**
   * Add operation to sync queue for when online
   */
  addToSyncQueue(item: Omit<SyncQueueItem, 'id' | 'timestamp' | 'retryCount'>): void {
    try {
      const queue = this.getSyncQueue();
      const newItem: SyncQueueItem = {
        ...item,
        id: this.generateId(),
        timestamp: new Date(),
        retryCount: 0
      };

      queue.push(newItem);
      localStorage.setItem(this.SYNC_QUEUE_KEY, JSON.stringify(queue));
      this.hasPendingSync.set(true);
    } catch (error) {
      console.error('Failed to add to sync queue:', error);
    }
  }

  /**
   * Get sync queue
   */
  getSyncQueue(): SyncQueueItem[] {
    try {
      const data = localStorage.getItem(this.SYNC_QUEUE_KEY);
      return data ? JSON.parse(data) : [];
    } catch (error) {
      console.error('Failed to retrieve sync queue:', error);
      return [];
    }
  }

  /**
   * Remove item from sync queue
   */
  removeFromSyncQueue(itemId: string): void {
    try {
      const queue = this.getSyncQueue();
      const updatedQueue = queue.filter(item => item.id !== itemId);
      localStorage.setItem(this.SYNC_QUEUE_KEY, JSON.stringify(updatedQueue));
      this.hasPendingSync.set(updatedQueue.length > 0);
    } catch (error) {
      console.error('Failed to remove from sync queue:', error);
    }
  }

  /**
   * Update retry count for sync queue item
   */
  updateSyncItemRetryCount(itemId: string): void {
    try {
      const queue = this.getSyncQueue();
      const item = queue.find(item => item.id === itemId);
      
      if (item) {
        item.retryCount++;
        
        // Remove item if max retries exceeded
        if (item.retryCount > this.MAX_RETRY_COUNT) {
          this.removeFromSyncQueue(itemId);
          console.warn(`Sync item ${itemId} removed after ${this.MAX_RETRY_COUNT} failed attempts`);
        } else {
          localStorage.setItem(this.SYNC_QUEUE_KEY, JSON.stringify(queue));
        }
      }
    } catch (error) {
      console.error('Failed to update sync item retry count:', error);
    }
  }

  /**
   * Clear all offline data
   */
  clearOfflineData(): void {
    try {
      localStorage.removeItem(this.STORAGE_KEY);
      this.lastSyncTime.set(null);
    } catch (error) {
      console.error('Failed to clear offline data:', error);
    }
  }

  /**
   * Clear sync queue
   */
  clearSyncQueue(): void {
    try {
      localStorage.removeItem(this.SYNC_QUEUE_KEY);
      this.hasPendingSync.set(false);
    } catch (error) {
      console.error('Failed to clear sync queue:', error);
    }
  }

  /**
   * Get storage usage information
   */
  getStorageInfo(): { used: number; available: number; percentage: number } {
    try {
      let used = 0;
      let available = 5 * 1024 * 1024; // Assume 5MB available (conservative estimate)

      // Calculate used storage
      for (let key in localStorage) {
        if (localStorage.hasOwnProperty(key)) {
          used += localStorage[key].length + key.length;
        }
      }

      // Try to get more accurate storage info if available
      if ('storage' in navigator && 'estimate' in navigator.storage) {
        navigator.storage.estimate().then(estimate => {
          if (estimate.quota && estimate.usage) {
            available = estimate.quota;
            used = estimate.usage;
          }
        });
      }

      const percentage = (used / available) * 100;

      return { used, available, percentage };
    } catch (error) {
      console.error('Failed to get storage info:', error);
      return { used: 0, available: 0, percentage: 0 };
    }
  }

  /**
   * Check if offline data is available
   */
  hasOfflineData(): boolean {
    const data = this.getOfflineData();
    return data !== null && !this.isDataExpired(data.lastSync);
  }

  /**
   * Get offline status
   */
  getOfflineStatus() {
    return {
      isOffline: this.isOffline(),
      hasPendingSync: this.hasPendingSync(),
      lastSyncTime: this.lastSyncTime(),
      hasOfflineData: this.hasOfflineData(),
      pendingSyncCount: this.getSyncQueue().length
    };
  }

  private initializeOfflineDetection(): void {
    // Initial state
    this.isOffline.set(!navigator.onLine);

    // Listen for online/offline events
    window.addEventListener('online', () => {
      this.isOffline.set(false);
      console.log('Application is now online');
    });

    window.addEventListener('offline', () => {
      this.isOffline.set(true);
      console.log('Application is now offline');
    });
  }

  private checkPendingSync(): void {
    const queue = this.getSyncQueue();
    this.hasPendingSync.set(queue.length > 0);
    
    const data = this.getOfflineData();
    if (data) {
      const lastSyncValue = data.lastSync instanceof Date ? data.lastSync : new Date(data.lastSync as any);
      this.lastSyncTime.set(lastSyncValue);
    }
  }

  private isDataExpired(lastSync: Date): boolean {
    const now = new Date();
    const syncTime = new Date(lastSync);
    const hoursDiff = (now.getTime() - syncTime.getTime()) / (1000 * 60 * 60);
    return hoursDiff > this.CACHE_EXPIRY_HOURS;
  }

  private generateId(): string {
    return Date.now().toString(36) + Math.random().toString(36).substr(2);
  }
}