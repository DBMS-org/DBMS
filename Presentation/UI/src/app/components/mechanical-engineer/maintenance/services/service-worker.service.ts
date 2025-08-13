import { Injectable, signal } from '@angular/core';

// Extend ServiceWorkerRegistration to include sync property
declare global {
    interface ServiceWorkerRegistration {
        sync?: {
            register(tag: string): Promise<void>;
        };
    }
}

export interface ServiceWorkerStatus {
    isEnabled: boolean;
    isRegistered: boolean;
    hasUpdate: boolean;
    isOnline: boolean;
}

@Injectable({
    providedIn: 'root'
})
export class ServiceWorkerService {
    // Service worker status signals
    isEnabled = signal(false);
    isRegistered = signal(false);
    hasUpdate = signal(false);
    isOnline = signal(navigator.onLine);

    private registration: ServiceWorkerRegistration | null = null;

    constructor() {
        this.initializeServiceWorker();
        this.setupOnlineOfflineDetection();
    }

    /**
     * Initialize service worker
     */
    private async initializeServiceWorker(): Promise<void> {
        // Check if service workers are supported
        if (!('serviceWorker' in navigator)) {
            console.log('Service workers not supported');
            return;
        }

        this.isEnabled.set(true);

        try {
            // Register the maintenance-specific service worker
            this.registration = await navigator.serviceWorker.register('/sw-maintenance.js', {
                scope: '/maintenance'
            });

            this.isRegistered.set(true);
            console.log('Maintenance service worker registered:', this.registration);

            // Listen for updates
            this.setupUpdateListener();

            // Listen for messages from service worker
            this.setupMessageListener();

            // Check for updates
            this.checkForUpdates();

        } catch (error) {
            console.error('Failed to register maintenance service worker:', error);
        }
    }



    /**
     * Setup update listener for custom service worker
     */
    private setupUpdateListener(): void {
        if (!this.registration) return;

        this.registration.addEventListener('updatefound', () => {
            const newWorker = this.registration!.installing;
            if (!newWorker) return;

            newWorker.addEventListener('statechange', () => {
                if (newWorker.state === 'installed' && navigator.serviceWorker.controller) {
                    this.hasUpdate.set(true);
                    console.log('New maintenance service worker available');
                }
            });
        });
    }

    /**
     * Setup message listener for service worker communication
     */
    private setupMessageListener(): void {
        navigator.serviceWorker.addEventListener('message', (event) => {
            const { type, payload } = event.data;

            switch (type) {
                case 'BACKGROUND_SYNC':
                    this.handleBackgroundSync(payload);
                    break;

                case 'CACHE_UPDATED':
                    console.log('Service worker cache updated:', payload);
                    break;

                default:
                    console.log('Unknown service worker message:', type, payload);
            }
        });
    }

    /**
     * Setup online/offline detection
     */
    private setupOnlineOfflineDetection(): void {
        window.addEventListener('online', () => {
            this.isOnline.set(true);
            console.log('Application is online');
        });

        window.addEventListener('offline', () => {
            this.isOnline.set(false);
            console.log('Application is offline');
        });
    }

    /**
     * Handle background sync request from service worker
     */
    private handleBackgroundSync(payload: any): void {
        console.log('Background sync requested:', payload);

        // Emit custom event that can be listened to by other services
        window.dispatchEvent(new CustomEvent('maintenance-background-sync', {
            detail: payload
        }));
    }

    /**
     * Check for service worker updates
     */
    async checkForUpdates(): Promise<void> {
        // Check custom service worker
        if (this.registration) {
            try {
                await this.registration.update();
            } catch (error) {
                console.error('Failed to update custom service worker:', error);
            }
        }
    }

    /**
     * Apply available updates
     */
    async applyUpdate(): Promise<void> {
        // Handle custom service worker update
        if (this.registration && this.registration.waiting) {
            this.registration.waiting.postMessage({ type: 'SKIP_WAITING' });
            window.location.reload();
        }
    }

    /**
     * Send message to service worker
     */
    sendMessage(type: string, payload?: any): void {
        if (!this.registration || !this.registration.active) {
            console.warn('Service worker not available for messaging');
            return;
        }

        this.registration.active.postMessage({ type, payload });
    }

    /**
     * Cache maintenance data in service worker
     */
    cacheMaintenanceData(data: any): void {
        this.sendMessage('CACHE_MAINTENANCE_DATA', data);
    }

    /**
     * Clear service worker cache
     */
    clearCache(): void {
        this.sendMessage('CLEAR_CACHE');
    }

    /**
     * Request background sync
     */
    requestBackgroundSync(tag: string = 'maintenance-sync'): void {
        if ('serviceWorker' in navigator && 'sync' in window.ServiceWorkerRegistration.prototype) {
            navigator.serviceWorker.ready.then((registration) => {
                if (registration.sync) {
                    return registration.sync.register(tag);
                } else {
                    console.warn('Background sync not supported');
                }
            }).catch((error) => {
                console.error('Background sync registration failed:', error);
            });
        } else {
            console.warn('Background sync not supported in this browser');
        }
    }

    /**
     * Get service worker status
     */
    getStatus(): ServiceWorkerStatus {
        return {
            isEnabled: this.isEnabled(),
            isRegistered: this.isRegistered(),
            hasUpdate: this.hasUpdate(),
            isOnline: this.isOnline()
        };
    }

    /**
     * Unregister service worker (for cleanup)
     */
    async unregister(): Promise<void> {
        if (this.registration) {
            try {
                await this.registration.unregister();
                this.isRegistered.set(false);
                console.log('Maintenance service worker unregistered');
            } catch (error) {
                console.error('Failed to unregister service worker:', error);
            }
        }
    }
}