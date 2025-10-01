export interface Store {
  id: number;
  storeName: string;
  storeAddress: string;
  storeManagerName: string;
  storeManagerContact: string;
  storeManagerEmail: string;
  storageCapacity: number;
  currentOccupancy: number;
  city: string;
  status: StoreStatus;
  regionId: number;
  projectId?: number;
  managerUserId?: number;
  createdAt: Date;
  updatedAt: Date;
  
  // Navigation properties (for detailed views)
  regionName?: string;
  projectName?: string;
  managerUserName?: string;
  inventoryItemsCount?: number;
  utilizationPercentage?: number;
}

export enum ExplosiveType {
  ANFO = 1,
  Emulsion = 2,
  Dynamite = 3,
  BlastingCaps = 4,
  DetonatingCord = 5,
  Primer = 6,
  Booster = 7,
  ShapedCharges = 8
}

export enum StoreStatus {
  Operational = 1,
  UnderMaintenance = 2,
  TemporarilyClosed = 3,
  InspectionRequired = 4,
  Decommissioned = 5
}

export interface StoreStatistics {
  totalStores: number;
  activeStores: number;
  inactiveStores: number;
  operationalStores: number;
  maintenanceStores: number;
  totalCapacity: number;
  totalOccupancy: number;
  utilizationRate: number;
  storesByRegion: { [key: string]: number };
}

export interface StoreFilters {
  status?: StoreStatus | 'ALL';
  regionId?: number | 'ALL';
  city?: string | 'ALL';
  storeManager?: string | 'ALL';
  searchTerm?: string;
}

export interface CreateStoreRequest {
  storeName: string;
  storeAddress: string;
  storeManagerName: string;
  storeManagerContact: string;
  storeManagerEmail: string;
  storageCapacity: number;
  city: string;
  regionId: number;
  projectId?: number;
  managerUserId?: number;
}

export interface UpdateStoreRequest {
  storeName: string;
  storeAddress: string;
  storeManagerName: string;
  storeManagerContact: string;
  storeManagerEmail: string;
  storageCapacity: number;
  city: string;
  status: StoreStatus;
  projectId?: number;
  managerUserId?: number;
}