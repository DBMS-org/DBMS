// Maintenance Module Service Worker
// Provides offline capabilities for the maintenance management system

const CACHE_NAME = 'maintenance-cache-v1';
const RUNTIME_CACHE = 'maintenance-runtime-v1';

// Resources to cache on install
const STATIC_RESOURCES = [
  // Add static resources here when needed
  // '/assets/icons/maintenance-icon.png',
  // '/assets/fonts/roboto.woff2'
];

// API endpoints to cache
const API_CACHE_PATTERNS = [
  /\/api\/maintenance\/stats/,
  /\/api\/maintenance\/jobs/,
  /\/api\/maintenance\/alerts/
];

// Install event - cache static resources
self.addEventListener('install', event => {
  console.log('Maintenance SW: Installing...');
  
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(cache => {
        console.log('Maintenance SW: Caching static resources');
        return cache.addAll(STATIC_RESOURCES);
      })
      .then(() => {
        console.log('Maintenance SW: Installation complete');
        return self.skipWaiting();
      })
  );
});

// Activate event - clean up old caches
self.addEventListener('activate', event => {
  console.log('Maintenance SW: Activating...');
  
  event.waitUntil(
    caches.keys()
      .then(cacheNames => {
        return Promise.all(
          cacheNames
            .filter(cacheName => {
              return cacheName.startsWith('maintenance-') && 
                     cacheName !== CACHE_NAME && 
                     cacheName !== RUNTIME_CACHE;
            })
            .map(cacheName => {
              console.log('Maintenance SW: Deleting old cache:', cacheName);
              return caches.delete(cacheName);
            })
        );
      })
      .then(() => {
        console.log('Maintenance SW: Activation complete');
        return self.clients.claim();
      })
  );
});

// Fetch event - implement caching strategies
self.addEventListener('fetch', event => {
  const { request } = event;
  const url = new URL(request.url);

  // Skip non-GET requests
  if (request.method !== 'GET') {
    return;
  }

  // Skip chrome-extension and other non-http requests
  if (!url.protocol.startsWith('http')) {
    return;
  }

  // Handle API requests
  if (isApiRequest(url)) {
    event.respondWith(handleApiRequest(request));
    return;
  }

  // Handle static resources
  if (isStaticResource(url)) {
    event.respondWith(handleStaticResource(request));
    return;
  }

  // Default: network first for everything else
  event.respondWith(
    fetch(request)
      .catch(() => {
        // If network fails, try cache
        return caches.match(request);
      })
  );
});

// Check if request is for API
function isApiRequest(url) {
  return url.pathname.startsWith('/api/maintenance');
}

// Check if request is for static resource
function isStaticResource(url) {
  return url.pathname.startsWith('/assets/') || 
         url.pathname.endsWith('.js') || 
         url.pathname.endsWith('.css') ||
         url.pathname.endsWith('.woff2') ||
         url.pathname.endsWith('.png') ||
         url.pathname.endsWith('.svg');
}

// Handle API requests with cache-first strategy for GET requests
async function handleApiRequest(request) {
  const cache = await caches.open(RUNTIME_CACHE);
  
  try {
    // Try network first
    const networkResponse = await fetch(request);
    
    if (networkResponse.ok) {
      // Cache successful responses
      const responseClone = networkResponse.clone();
      await cache.put(request, responseClone);
      
      console.log('Maintenance SW: Cached API response:', request.url);
    }
    
    return networkResponse;
  } catch (error) {
    console.log('Maintenance SW: Network failed, trying cache:', request.url);
    
    // Network failed, try cache
    const cachedResponse = await cache.match(request);
    
    if (cachedResponse) {
      console.log('Maintenance SW: Serving from cache:', request.url);
      
      // Add a header to indicate this is from cache
      const response = cachedResponse.clone();
      response.headers.set('X-Served-From', 'cache');
      
      return response;
    }
    
    // No cache available, return error
    throw error;
  }
}

// Handle static resources with cache-first strategy
async function handleStaticResource(request) {
  const cache = await caches.open(CACHE_NAME);
  
  // Try cache first
  const cachedResponse = await cache.match(request);
  if (cachedResponse) {
    return cachedResponse;
  }
  
  // Not in cache, fetch from network
  try {
    const networkResponse = await fetch(request);
    
    if (networkResponse.ok) {
      // Cache the response
      await cache.put(request, networkResponse.clone());
    }
    
    return networkResponse;
  } catch (error) {
    console.error('Maintenance SW: Failed to fetch static resource:', request.url);
    throw error;
  }
}

// Handle background sync (if supported)
if ('sync' in self.registration) {
  self.addEventListener('sync', event => {
    console.log('Maintenance SW: Background sync triggered:', event.tag);
    
    if (event.tag === 'maintenance-sync') {
      event.waitUntil(performBackgroundSync());
    }
  });
}

// Perform background sync
async function performBackgroundSync() {
  try {
    // Notify the main thread to perform sync
    const clients = await self.clients.matchAll();
    
    clients.forEach(client => {
      client.postMessage({
        type: 'BACKGROUND_SYNC',
        payload: { action: 'sync-maintenance-data' }
      });
    });
    
    console.log('Maintenance SW: Background sync notification sent');
  } catch (error) {
    console.error('Maintenance SW: Background sync failed:', error);
  }
}

// Handle messages from main thread
self.addEventListener('message', event => {
  const { type, payload } = event.data;
  
  switch (type) {
    case 'SKIP_WAITING':
      self.skipWaiting();
      break;
      
    case 'CACHE_MAINTENANCE_DATA':
      handleCacheMaintenanceData(payload);
      break;
      
    case 'CLEAR_CACHE':
      handleClearCache();
      break;
      
    default:
      console.log('Maintenance SW: Unknown message type:', type);
  }
});

// Cache maintenance data from main thread
async function handleCacheMaintenanceData(data) {
  try {
    const cache = await caches.open(RUNTIME_CACHE);
    
    // Create mock responses for the data
    if (data.jobs) {
      const jobsResponse = new Response(JSON.stringify(data.jobs), {
        headers: { 'Content-Type': 'application/json' }
      });
      await cache.put('/api/maintenance/jobs', jobsResponse);
    }
    
    if (data.stats) {
      const statsResponse = new Response(JSON.stringify(data.stats), {
        headers: { 'Content-Type': 'application/json' }
      });
      await cache.put('/api/maintenance/stats', statsResponse);
    }
    
    console.log('Maintenance SW: Cached maintenance data');
  } catch (error) {
    console.error('Maintenance SW: Failed to cache maintenance data:', error);
  }
}

// Clear all caches
async function handleClearCache() {
  try {
    const cacheNames = await caches.keys();
    const maintenanceCaches = cacheNames.filter(name => name.startsWith('maintenance-'));
    
    await Promise.all(
      maintenanceCaches.map(cacheName => caches.delete(cacheName))
    );
    
    console.log('Maintenance SW: All caches cleared');
  } catch (error) {
    console.error('Maintenance SW: Failed to clear caches:', error);
  }
}

console.log('Maintenance SW: Service Worker loaded');