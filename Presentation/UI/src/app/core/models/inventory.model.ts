// Warehouse Inventory Models

export interface WarehouseInventory {
  id: number;
  batchId: string;
  explosiveType: ExplosiveType;
  quantity: number;
  availableQuantity: number;
  allocatedQuantity: number;
  unit: string;
  manufacturingDate: Date;
  expiryDate: Date;
  supplier: string;
  grade?: string;
  technicalProperties?: any; // Type-specific properties
  createdAt: Date;
  updatedAt: Date;

  // Computed properties
  daysUntilExpiry?: number;
  isExpired?: boolean;
  isExpiringSoon?: boolean;
}

export interface ANFOInventory extends WarehouseInventory {
  explosiveType: ExplosiveType.ANFO;
  technicalProperties: ANFOTechnicalProperties;
}

export interface EmulsionInventory extends WarehouseInventory {
  explosiveType: ExplosiveType.Emulsion;
  technicalProperties: EmulsionTechnicalProperties;
}

// ANFO Technical Properties
export interface ANFOTechnicalProperties {
  // Quality Parameters
  density: number;                    // 0.8-0.9 g/cm³
  fuelOilContent: number;            // 5.5-6.0 %
  moistureContent?: number;          // < 0.2 %
  prillSize?: number;                // 1-3 mm
  detonationVelocity?: number;       // 3000-3500 m/s

  // Manufacturing
  grade: 'TGAN' | 'Standard';
  manufacturerBatchNumber?: string;

  // Storage
  storageLocation: string;
  storageConditions: {
    temperature: number;             // 5-35°C
    humidity: number;                // < 50% RH
  };

  // Quality Control
  fumeClass: 1 | 2 | 3;
  qualityCheckDate?: Date;
  qualityStatus: 'Approved' | 'Pending' | 'Rejected';
  waterResistance: 'None';

  // Additional
  notes?: string;
}

// Emulsion Technical Properties
export interface EmulsionTechnicalProperties {
  // Quality Parameters
  densityUnsensitized: number;       // 1.30-1.45 g/cm³
  densitySensitized: number;         // 1.10-1.30 g/cm³
  viscosity: number;                 // 50,000-200,000 cP
  waterContent: number;              // 12-16 %
  pH: number;                        // 4.5-6.5
  detonationVelocity?: number;       // 4500-6000 m/s
  bubbleSize?: number;               // 10-100 μm

  // Temperature
  storageTemperature: number;        // -20 to 50°C
  applicationTemperature?: number;   // 0-45°C

  // Manufacturing
  grade: 'Standard' | 'High-Density' | 'Low-Density' | 'Water-Resistant';
  manufacturerBatchNumber?: string;
  color: string;                     // White, Pink, Blue, etc.

  // Sensitization
  sensitizationType: 'Chemical' | 'Physical' | 'Hybrid';
  sensitizerContent?: number;        // % by weight

  // Storage
  storageLocation: string;

  // Quality Control
  fumeClass: 1 | 2 | 3;
  qualityCheckDate?: Date;
  qualityStatus: 'Approved' | 'Pending' | 'Rejected';
  stabilityCheck?: {
    phaseSeparation: boolean;
    crystallization: boolean;
    colorConsistency: boolean;
  };
  waterResistance: 'Excellent' | 'Good';

  // Additional
  notes?: string;
}

// Request DTOs
export interface CreateANFOInventoryRequest {
  batchId: string;
  quantity: number;
  unit: string;
  manufacturingDate: Date;
  expiryDate: Date;
  supplier: string;

  // ANFO Specific
  density: number;
  fuelOilContent: number;
  moistureContent?: number;
  prillSize?: number;
  grade: 'TGAN' | 'Standard';
  storageLocation: string;
  storageTemperature: number;
  storageHumidity: number;
  fumeClass: 1 | 2 | 3;
  notes?: string;
}

export interface CreateEmulsionInventoryRequest {
  batchId: string;
  quantity: number;
  unit: string;
  manufacturingDate: Date;
  expiryDate: Date;
  supplier: string;

  // Emulsion Specific
  densityUnsensitized: number;
  densitySensitized: number;
  viscosity: number;
  waterContent: number;
  pH: number;
  storageTemperature: number;
  applicationTemperature?: number;
  grade: 'Standard' | 'High-Density' | 'Low-Density' | 'Water-Resistant';
  color: string;
  sensitizationType: 'Chemical' | 'Physical' | 'Hybrid';
  storageLocation: string;
  fumeClass: 1 | 2 | 3;
  notes?: string;
}

export interface UpdateInventoryRequest {
  quantity?: number;
  availableQuantity?: number;
  expiryDate?: Date;
  technicalProperties?: any;
  notes?: string;
}

// Inventory Statistics
export interface InventoryStatistics {
  totalBatches: number;
  totalQuantity: number;
  availableQuantity: number;
  allocatedQuantity: number;
  expiringBatches: number;
  expiredBatches: number;
  quantityByType: {
    [key in ExplosiveType]?: number;
  };
  batchesBySupplier: {
    [supplier: string]: number;
  };
}

// Enums
export enum ExplosiveType {
  ANFO = 'ANFO',
  Emulsion = 'Emulsion',
  Dynamite = 'Dynamite',
  BlastingCaps = 'BlastingCaps',
  DetonatingCord = 'DetonatingCord',
  Primer = 'Primer',
  Booster = 'Booster',
  ShapedCharges = 'ShapedCharges'
}

// Validation Constants
export const ANFOValidation = {
  density: { min: 0.8, max: 0.9, unit: 'g/cm³' },
  fuelOilContent: { min: 5.5, max: 6.0, unit: '%' },
  moistureContent: { max: 0.2, unit: '%' },
  prillSize: { min: 1, max: 3, unit: 'mm' },
  detonationVelocity: { min: 3000, max: 3500, unit: 'm/s' },
  shelfLife: { value: 12, unit: 'months' },
  storageTemp: { min: 5, max: 35, unit: '°C' },
  storageHumidity: { max: 50, unit: '%' }
};

export const EmulsionValidation = {
  densityUnsensitized: { min: 1.30, max: 1.45, unit: 'g/cm³' },
  densitySensitized: { min: 1.10, max: 1.30, unit: 'g/cm³' },
  viscosity: { min: 50000, max: 200000, unit: 'cP' },
  waterContent: { min: 12, max: 16, unit: '%' },
  pH: { min: 4.5, max: 6.5 },
  detonationVelocity: { min: 4500, max: 6000, unit: 'm/s' },
  shelfLife: { value: 6, unit: 'months' },
  storageTemp: { min: -20, max: 50, unit: '°C' },
  applicationTemp: { min: 0, max: 45, unit: '°C' }
};
