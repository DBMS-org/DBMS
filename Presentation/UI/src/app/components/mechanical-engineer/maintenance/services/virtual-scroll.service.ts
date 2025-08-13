import { Injectable, signal, computed } from '@angular/core';

export interface VirtualScrollConfig {
  itemHeight: number;
  containerHeight: number;
  buffer: number; // Number of items to render outside visible area
  totalItems: number;
}

export interface VirtualScrollState {
  startIndex: number;
  endIndex: number;
  visibleItems: number;
  scrollTop: number;
  totalHeight: number;
  offsetY: number;
}

@Injectable({
  providedIn: 'root'
})
export class VirtualScrollService {
  private configs = new Map<string, VirtualScrollConfig>();
  private scrollStates = new Map<string, ReturnType<typeof signal<VirtualScrollState>>>();

  /**
   * Initialize virtual scrolling for a specific list
   */
  initializeVirtualScroll(listId: string, config: VirtualScrollConfig) {
    this.configs.set(listId, config);
    
    const initialState: VirtualScrollState = {
      startIndex: 0,
      endIndex: Math.min(config.totalItems - 1, this.calculateVisibleItems(config) + config.buffer),
      visibleItems: this.calculateVisibleItems(config),
      scrollTop: 0,
      totalHeight: config.totalItems * config.itemHeight,
      offsetY: 0
    };

    this.scrollStates.set(listId, signal(initialState));
  }

  /**
   * Update scroll position and recalculate visible items
   */
  updateScrollPosition(listId: string, scrollTop: number) {
    const config = this.configs.get(listId);
    const stateSignal = this.scrollStates.get(listId);
    
    if (!config || !stateSignal) {
      return;
    }

    const visibleItems = this.calculateVisibleItems(config);
    const startIndex = Math.floor(scrollTop / config.itemHeight);
    const bufferedStartIndex = Math.max(0, startIndex - config.buffer);
    const bufferedEndIndex = Math.min(
      config.totalItems - 1,
      startIndex + visibleItems + config.buffer
    );

    const newState: VirtualScrollState = {
      startIndex: bufferedStartIndex,
      endIndex: bufferedEndIndex,
      visibleItems,
      scrollTop,
      totalHeight: config.totalItems * config.itemHeight,
      offsetY: bufferedStartIndex * config.itemHeight
    };

    stateSignal.set(newState);
  }

  /**
   * Update total items count
   */
  updateTotalItems(listId: string, totalItems: number) {
    const config = this.configs.get(listId);
    const stateSignal = this.scrollStates.get(listId);
    
    if (!config || !stateSignal) {
      return;
    }

    // Update config
    const updatedConfig = { ...config, totalItems };
    this.configs.set(listId, updatedConfig);

    // Recalculate state
    const currentState = stateSignal();
    const visibleItems = this.calculateVisibleItems(updatedConfig);
    const endIndex = Math.min(
      totalItems - 1,
      currentState.startIndex + visibleItems + config.buffer
    );

    const newState: VirtualScrollState = {
      ...currentState,
      endIndex,
      totalHeight: totalItems * config.itemHeight
    };

    stateSignal.set(newState);
  }

  /**
   * Get virtual scroll state as signal
   */
  getVirtualScrollState(listId: string) {
    return this.scrollStates.get(listId);
  }

  /**
   * Get computed visible items for a list
   */
  getVisibleItems<T>(listId: string, allItems: T[]) {
    const stateSignal = this.scrollStates.get(listId);
    
    if (!stateSignal) {
      return signal<T[]>([]);
    }

    return computed(() => {
      const state = stateSignal();
      return allItems.slice(state.startIndex, state.endIndex + 1);
    });
  }

  /**
   * Scroll to specific item index
   */
  scrollToIndex(listId: string, index: number) {
    const config = this.configs.get(listId);
    
    if (!config) {
      return;
    }

    const scrollTop = index * config.itemHeight;
    this.updateScrollPosition(listId, scrollTop);
    
    return scrollTop;
  }

  /**
   * Get item position by index
   */
  getItemPosition(listId: string, index: number): number {
    const config = this.configs.get(listId);
    return config ? index * config.itemHeight : 0;
  }

  /**
   * Clean up virtual scroll instance
   */
  cleanup(listId: string) {
    this.configs.delete(listId);
    this.scrollStates.delete(listId);
  }

  private calculateVisibleItems(config: VirtualScrollConfig): number {
    return Math.ceil(config.containerHeight / config.itemHeight);
  }
}