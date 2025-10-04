# Explosive Materials Technical Specifications

## Overview

This document defines the technical specifications and quality control parameters for explosive materials managed in the DBMS inventory system.

---

## 1. ANFO (Ammonium Nitrate Fuel Oil)

### Composition
- **Primary Components:**
  - 94-95% Porous Prilled Ammonium Nitrate (NH₄NO₃)
  - 5-6% Fuel Oil (Number 2 diesel/road diesel)

### Physical Properties

#### Density Specifications
- **Bulk Density:** 800-900 kg/m³ (0.8-0.9 g/cm³)
  - Standard reference: 810 kg/m³
  - Loose pour: 800-850 kg/m³
  - Pneumatic loading: ~1,100 kg/m³ (at 4 bar pressure)
- **Ammonium Nitrate Prill Density:**
  - Individual prill: ~1,300 kg/m³
  - Crystal density: 1,700 kg/m³
  - Explosive-grade prill: 800-900 kg/m³

#### Detonation Properties
- **Velocity of Detonation (VOD):**
  - Standard: 3,200 m/s (in 130mm diameter, unconfined)
  - Range: 2,700-4,600 m/s (9,000-15,000 ft/s)
  - Dependent on: density, diameter, confinement, temperature

#### Critical Dimensions
- **Critical Diameter:** ≥50mm (minimum), ≥75mm (recommended)
- **Recommended Borehole Diameter:** ≥100mm

### Quality Control Parameters

| Parameter | Specification | Unit | Tolerance |
|-----------|--------------|------|-----------|
| Fuel Oil Content | 5.5-6.0 | % by weight | ±0.3% |
| Bulk Density | 800-900 | kg/m³ | ±50 kg/m³ |
| Moisture Content | <0.2 | % | Max |
| Prill Size | 1-3 | mm | Standard |
| Detonation Velocity | 3,000-3,500 | m/s | Test required |

### Manufacturing Grades
- **FGAN (Fertilizer Grade):** Higher density, lower porosity - NOT for explosives
- **TGAN (Technical Grade):** Lower density, higher porosity - FOR explosives

### Storage Requirements
- **Environment:** Dry, well-ventilated
- **Water Resistance:** Zero (must be kept completely dry)
- **Temperature Range:** 5-35°C
- **Humidity:** <50% RH
- **Shelf Life:**
  - Dry conditions: 6-12 months
  - Mixed ANFO: 1-2 months

### Safety Parameters
- **Fume Class:** Class 1 (when properly balanced)
- **Sensitivity:** Low (requires booster)
- **Minimum Primer:** 150-200g Pentolite or equivalent

### Chemical Balance
- **Fuel/Oxidizer Ratio:** Critical for safety
  - **Fuel rich:** Produces carbon monoxide (CO) - hazardous
  - **Oxidizer rich:** Produces nitrogen dioxide (NO₂) - toxic

---

## 2. Emulsion Explosives

### Composition
- **Oxidizer Phase (70-85%):**
  - Ammonium Nitrate
  - Sodium Nitrate
  - Water (8-20%)
- **Fuel Phase (5-10%):**
  - Mineral oil
  - Wax
  - Emulsifiers
- **Sensitizers (5-15%):**
  - Glass microspheres (chemical sensitization)
  - Perlite
  - Sodium nitrite solutions

### Physical Properties

#### Density Specifications
- **Unsensitized Density:** 1,300-1,450 kg/m³
- **Sensitized Density:** 1,100-1,300 kg/m³
- **Target Working Density:** 1,150-1,250 kg/m³
- **Low Density Grades:** 800-1,000 kg/m³

#### Rheological Properties
- **Viscosity:**
  - At 20°C: 50,000-200,000 cP
  - At 40°C: 20,000-80,000 cP
  - Pumpable range: <100,000 cP
- **Flow Behavior:** Non-Newtonian, thixotropic

#### Water Content
- **Total Water Content:** 8-20% by weight
  - Minimum: 8% (structural integrity)
  - Maximum: 20% (detonation performance)
  - Optimal: 12-16%

### Quality Control Parameters

| Parameter | Specification | Unit | Measurement Method |
|-----------|--------------|------|-------------------|
| Density (unsensitized) | 1,300-1,450 | kg/m³ | Pycnometer |
| Density (sensitized) | 1,100-1,300 | kg/m³ | Direct measurement |
| Viscosity @ 20°C | 50,000-200,000 | cP | Brookfield viscometer |
| Water Content | 12-16 | % | Karl Fischer titration |
| pH Value | 4.5-6.5 | - | pH meter |
| Detonation Velocity | 4,500-6,000 | m/s | VOD measurement |
| Bubble Size | 10-100 | μm | Microscopy |

### Temperature Specifications
- **Storage Temperature:** -20°C to 50°C
- **Application Temperature:** 0-45°C
- **Optimal Performance:** 15-30°C
- **Critical Temperature:** >60°C (degradation risk)

### Performance Characteristics
- **Water Resistance:** Excellent (100% water-resistant)
- **Detonation Velocity:** 4,500-6,000 m/s
- **Critical Diameter:** 25-50mm
- **Fume Class:** Class 1
- **Sensitivity:** Medium (requires booster)

### Shelf Life & Stability
- **Shelf Life:**
  - Bulk: 3-6 months
  - Packaged: 6-12 months
- **Stability Indicators:**
  - No phase separation
  - Viscosity within range
  - No crystallization
  - Consistent color

### Color Coding
- **Standard:** White to off-white
- **Dense grades:** Pink or red
- **Water-resistant:** Blue tint
- **Deviations:** Indicates quality issues

---

## 3. Data Model Specifications

### ANFO Inventory Model

```typescript
interface ANFOInventory {
  // Core Identification
  batchId: string;                    // Format: ANFO-YYYY-XXX
  manufacturingDate: Date;
  expiryDate: Date;                   // Manufacturing date + 12 months

  // Quantity
  quantity: number;                   // kg
  unit: 'kg' | 'tons';

  // Quality Parameters
  density: number;                    // 0.8 - 0.9 g/cm³
  fuelOilContent: number;            // 5.5 - 6.0 %
  moistureContent: number;           // < 0.2 %
  prillSize: number;                 // 1-3 mm
  detonationVelocity?: number;       // 3000-3500 m/s (if tested)

  // Manufacturing
  grade: 'TGAN' | 'Standard';
  supplier: string;
  manufacturerBatchNumber?: string;

  // Storage
  storageLocation: string;
  storageConditions: {
    temperature: number;             // °C
    humidity: number;                // % RH
  };

  // Quality Control
  fumeClass: 1 | 2 | 3;
  qualityCheckDate?: Date;
  qualityStatus: 'Approved' | 'Pending' | 'Rejected';

  // Additional
  notes?: string;
  waterResistance: 'None';           // Always 'None' for ANFO
}
```

### Emulsion Inventory Model

```typescript
interface EmulsionInventory {
  // Core Identification
  batchId: string;                    // Format: EMU-YYYY-XXX
  manufacturingDate: Date;
  expiryDate: Date;                   // Manufacturing date + 6 months

  // Quantity
  quantity: number;                   // kg
  unit: 'kg' | 'tons';

  // Quality Parameters
  densityUnsensitized: number;       // 1.30 - 1.45 g/cm³
  densitySensitized: number;         // 1.10 - 1.30 g/cm³
  viscosity: number;                 // 50,000 - 200,000 cP
  waterContent: number;              // 12 - 16 %
  pH: number;                        // 4.5 - 6.5
  detonationVelocity?: number;       // 4500-6000 m/s (if tested)
  bubbleSize?: number;               // 10-100 μm

  // Temperature
  storageTemperature: number;        // -20 to 50 °C
  applicationTemperature?: number;   // 0 to 45 °C

  // Manufacturing
  grade: 'Standard' | 'High-Density' | 'Low-Density' | 'Water-Resistant';
  supplier: string;
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
  stabilityCheck: {
    phaseSeparation: boolean;
    crystallization: boolean;
    colorConsistency: boolean;
  };

  // Additional
  notes?: string;
  waterResistance: 'Excellent' | 'Good';  // Always water-resistant
}
```

---

## 4. Validation Rules

### ANFO Validation
```typescript
const ANFOValidation = {
  density: { min: 0.8, max: 0.9, unit: 'g/cm³' },
  fuelOilContent: { min: 5.5, max: 6.0, unit: '%' },
  moistureContent: { max: 0.2, unit: '%' },
  prillSize: { min: 1, max: 3, unit: 'mm' },
  detonationVelocity: { min: 3000, max: 3500, unit: 'm/s' },
  shelfLife: { value: 12, unit: 'months' },
  storageTemp: { min: 5, max: 35, unit: '°C' },
  storageHumidity: { max: 50, unit: '%' }
};
```

### Emulsion Validation
```typescript
const EmulsionValidation = {
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
```

---

## 5. Quality Checks & Alerts

### Mandatory Quality Checks
1. **Upon Receipt:**
   - Visual inspection
   - Density measurement
   - Documentation verification

2. **Monthly:**
   - Storage condition verification
   - Expiry date monitoring
   - Physical property checks

3. **Before Transfer:**
   - Quality status confirmation
   - Quantity verification
   - Condition assessment

### Alert Thresholds

| Alert Type | ANFO | Emulsion |
|-----------|------|----------|
| Expiry Warning | 30 days | 30 days |
| Expiry Critical | 7 days | 7 days |
| Humidity Warning | >40% RH | N/A |
| Temperature Warning | >30°C | >40°C |
| Density Out of Range | ±10% | ±5% |
| Viscosity High | N/A | >150,000 cP |

---

## References

- ANFO explosive technical specifications from Dyno Nobel
- Scientific research on ANFO properties and quality control
- Industrial standards for water gel and emulsion explosives
- Mining safety regulations for explosive materials

---

*Document Version: 1.0*
*Last Updated: 2025-10-04*
*Classification: Technical Reference*
