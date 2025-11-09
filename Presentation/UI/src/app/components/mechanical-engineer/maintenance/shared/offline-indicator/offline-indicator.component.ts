import { Component, inject, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OfflineStorageService } from '../../services/offline-storage.service';
import { SyncService } from '../../services/sync.service';

@Component({
  selector: 'app-offline-indicator',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatProgressBarModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="offline-indicator" [class]="getIndicatorClass()">
      @if (offlineStorage.isOffline()) {
        <!-- Offline State -->
        <div class="offline-status">
          <mat-icon class="status-icon offline">cloud_off</mat-icon>
          <div class="status-content">
            <div class="status-title">You're offline</div>
            <div class="status-subtitle">
              @if (offlineStorage.hasPendingSync()) {
                {{ offlineStorage.getSyncQueue().length }} changes will sync when online
              } @else {
                Using cached data
              }
            </div>
          </div>
        </div>
      } @else {
        <!-- Online State -->
        @if (syncService.isSyncing()) {
          <!-- Syncing State -->
          <div class="sync-status">
            <mat-icon class="status-icon syncing">sync</mat-icon>
            <div class="status-content">
              <div class="status-title">Syncing...</div>
              <div class="status-subtitle">{{ syncService.syncProgress().current }}</div>
              <mat-progress-bar 
                mode="determinate" 
                [value]="syncService.syncProgress().percentage"
                class="sync-progress">
              </mat-progress-bar>
            </div>
          </div>
        } @else if (offlineStorage.hasPendingSync()) {
          <!-- Pending Sync State -->
          <div class="pending-sync-status">
            <mat-icon class="status-icon pending">cloud_sync</mat-icon>
            <div class="status-content">
              <div class="status-title">{{ offlineStorage.getSyncQueue().length }} changes pending</div>
              <div class="status-subtitle">
                <button 
                  mat-button 
                  class="sync-now-btn"
                  (click)="syncNow()"
                  [disabled]="syncService.isSyncing()">
                  Sync now
                </button>
              </div>
            </div>
          </div>
        } @else {
          <!-- Online and Synced State -->
          <div class="online-status">
            <mat-icon class="status-icon online">cloud_done</mat-icon>
            <div class="status-content">
              <div class="status-title">All synced</div>
              @if (offlineStorage.lastSyncTime()) {
                <div class="status-subtitle">
                  Last sync: {{ formatLastSync(normalizeDate(offlineStorage.lastSyncTime()!)) }}
                </div>
              }
            </div>
          </div>
        }
      }

      <!-- Storage Info (Development/Debug) -->
      @if (showStorageInfo()) {
        <div class="storage-info" [matTooltip]="getStorageTooltip()">
          <mat-icon class="storage-icon">storage</mat-icon>
          <span class="storage-text">{{ getStorageUsage() }}</span>
        </div>
      }
    </div>
  `,
  styles: [`
    .offline-indicator {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 8px 16px;
      border-radius: 8px;
      font-size: 14px;
      transition: all 0.3s ease;
      min-height: 48px;
    }

    .offline-indicator.offline {
      background: linear-gradient(135deg, #fef3c7 0%, #fde68a 100%);
      border: 1px solid #f59e0b;
      color: #92400e;
    }

    .offline-indicator.syncing {
      background: linear-gradient(135deg, #dbeafe 0%, #bfdbfe 100%);
      border: 1px solid #3b82f6;
      color: #1e40af;
    }

    .offline-indicator.pending {
      background: linear-gradient(135deg, #fef3c7 0%, #fde68a 100%);
      border: 1px solid #f59e0b;
      color: #92400e;
    }

    .offline-indicator.online {
      background: linear-gradient(135deg, #d1fae5 0%, #a7f3d0 100%);
      border: 1px solid #10b981;
      color: #065f46;
    }

    .offline-status,
    .sync-status,
    .pending-sync-status,
    .online-status {
      display: flex;
      align-items: center;
      gap: 12px;
      flex: 1;
    }

    .status-icon {
      font-size: 20px;
      width: 20px;
      height: 20px;
    }

    .status-icon.offline {
      color: #f59e0b;
    }

    .status-icon.syncing {
      color: #3b82f6;
      animation: spin 1s linear infinite;
    }

    .status-icon.pending {
      color: #f59e0b;
    }

    .status-icon.online {
      color: #10b981;
    }

    .status-content {
      flex: 1;
      min-width: 0;
    }

    .status-title {
      font-weight: 600;
      line-height: 1.2;
    }

    .status-subtitle {
      font-size: 12px;
      opacity: 0.8;
      margin-top: 2px;
      line-height: 1.2;
    }

    .sync-progress {
      margin-top: 4px;
      height: 3px;
    }

    .sync-now-btn {
      font-size: 12px;
      min-height: 24px;
      line-height: 24px;
      padding: 0 8px;
      color: inherit;
      text-decoration: underline;
    }

    .storage-info {
      display: flex;
      align-items: center;
      gap: 4px;
      padding: 4px 8px;
      background: rgba(0, 0, 0, 0.1);
      border-radius: 4px;
      font-size: 11px;
      cursor: help;
    }

    .storage-icon {
      font-size: 14px;
      width: 14px;
      height: 14px;
    }

    .storage-text {
      font-family: monospace;
    }

    @keyframes spin {
      0% { transform: rotate(0deg); }
      100% { transform: rotate(360deg); }
    }

    /* Responsive design */
    @media (max-width: 768px) {
      .offline-indicator {
        padding: 6px 12px;
        font-size: 13px;
        min-height: 40px;
      }

      .status-icon {
        font-size: 18px;
        width: 18px;
        height: 18px;
      }

      .status-subtitle {
        font-size: 11px;
      }

      .storage-info {
        display: none; /* Hide on mobile */
      }
    }

    /* High contrast mode */
    @media (prefers-contrast: high) {
      .offline-indicator {
        border-width: 2px;
      }
    }

    /* Reduced motion */
    @media (prefers-reduced-motion: reduce) {
      .status-icon.syncing {
        animation: none;
      }
      
      .offline-indicator {
        transition: none;
      }
    }
  `]
})
export class OfflineIndicatorComponent {
  offlineStorage = inject(OfflineStorageService);
  syncService = inject(SyncService);
  private snackBar = inject(MatSnackBar);

  // Configuration
  showStorageInfo = () => false; // Set to true for development/debugging

  getIndicatorClass(): string {
    if (this.offlineStorage.isOffline()) {
      return 'offline';
    }
    
    if (this.syncService.isSyncing()) {
      return 'syncing';
    }
    
    if (this.offlineStorage.hasPendingSync()) {
      return 'pending';
    }
    
    return 'online';
  }

  syncNow(): void {
    this.syncService.performFullSync().subscribe({
      next: (result) => {
        if (result.success) {
          this.snackBar.open(
            `Sync completed: ${result.syncedItems} items synced`,
            'Close',
            { duration: 3000, panelClass: ['success-snackbar'] }
          );
        } else {
          this.snackBar.open(
            `Sync completed with errors: ${result.failedItems} items failed`,
            'Close',
            { duration: 5000, panelClass: ['warning-snackbar'] }
          );
        }
      },
      error: (error) => {
        this.snackBar.open(
          'Sync failed. Please try again.',
          'Close',
          { duration: 5000, panelClass: ['error-snackbar'] }
        );
      }
    });
  }

  formatLastSync(date: Date): string {
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffMins = Math.floor(diffMs / (1000 * 60));
    const diffHours = Math.floor(diffMs / (1000 * 60 * 60));

    if (diffMins < 1) {
      return 'just now';
    } else if (diffMins < 60) {
      return `${diffMins}m ago`;
    } else if (diffHours < 24) {
      return `${diffHours}h ago`;
    } else {
      return date.toLocaleDateString();
    }
  }

  // Ensure date is a Date object
  normalizeDate(value: any): Date {
    return value instanceof Date ? value : new Date(value);
  }

  getStorageUsage(): string {
    const info = this.offlineStorage.getStorageInfo();
    const usedMB = (info.used / (1024 * 1024)).toFixed(1);
    return `${usedMB}MB`;
  }

  getStorageTooltip(): string {
    const info = this.offlineStorage.getStorageInfo();
    const usedMB = (info.used / (1024 * 1024)).toFixed(1);
    const availableMB = (info.available / (1024 * 1024)).toFixed(1);
    return `Storage: ${usedMB}MB used of ${availableMB}MB available (${info.percentage.toFixed(1)}%)`;
  }
}