import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  private readonly SESSION_KEYS = [
    'token',
    'user',
    'currentProjectId',
    'currentSiteId',
    'lastActivity',
    'userPreferences',
    'dashboardSettings',
    'recentActivities',
    'cachedProjects',
    'cachedSites'
  ];

  constructor() {}

  /**
   * Clear all session data from localStorage and sessionStorage
   */
  clearAllSessionData(): void {
    // Clear specific keys from localStorage
    this.SESSION_KEYS.forEach(key => {
      localStorage.removeItem(key);
    });

    // Clear any pattern-based keys
    this.clearPatternKeys();

    // Clear all sessionStorage
    sessionStorage.clear();

    // Clear any IndexedDB data if needed
    this.clearIndexedDBData();
  }

  /**
   * Clear keys that match certain patterns
   */
  private clearPatternKeys(): void {
    const patterns = [
      'pattern_',
      'drill_',
      'blast_',
      'cache_',
      'temp_',
      'workflow_'
    ];

    // Get all localStorage keys
    const localStorageKeys = Object.keys(localStorage);
    
    localStorageKeys.forEach(key => {
      if (patterns.some(pattern => key.startsWith(pattern))) {
        localStorage.removeItem(key);
      }
    });
  }

  /**
   * Clear IndexedDB data (if any)
   */
  private clearIndexedDBData(): void {
    if ('indexedDB' in window) {
      try {
        // Clear any app-specific IndexedDB databases
        indexedDB.deleteDatabase('DBMS_Cache');
        indexedDB.deleteDatabase('DBMS_Offline');
      } catch (error) {
        console.warn('Failed to clear IndexedDB:', error);
      }
    }
  }

  /**
   * Set last activity timestamp
   */
  updateLastActivity(): void {
    localStorage.setItem('lastActivity', new Date().toISOString());
  }

  /**
   * Get last activity timestamp
   */
  getLastActivity(): Date | null {
    const lastActivity = localStorage.getItem('lastActivity');
    return lastActivity ? new Date(lastActivity) : null;
  }

  /**
   * Check if session has expired based on inactivity
   */
  isSessionExpired(maxInactiveMinutes: number = 60): boolean {
    const lastActivity = this.getLastActivity();
    if (!lastActivity) return false;

    const now = new Date();
    const diffMinutes = (now.getTime() - lastActivity.getTime()) / (1000 * 60);
    return diffMinutes > maxInactiveMinutes;
  }

  /**
   * Backup session data before logout (for debugging or analytics)
   */
  backupSessionData(): any {
    const backup: any = {};
    
    this.SESSION_KEYS.forEach(key => {
      const value = localStorage.getItem(key);
      if (value) {
        try {
          backup[key] = JSON.parse(value);
        } catch {
          backup[key] = value;
        }
      }
    });

    backup.timestamp = new Date().toISOString();
    backup.userAgent = navigator.userAgent;
    
    return backup;
  }

  /**
   * Clear specific cached data while keeping authentication
   */
  clearCacheData(): void {
    const cacheKeys = [
      'cachedProjects',
      'cachedSites',
      'recentActivities',
      'dashboardSettings'
    ];

    cacheKeys.forEach(key => {
      localStorage.removeItem(key);
    });
  }
} 