# Explosive Materials Technical Specifications - Reference Guide

## Document Information
**Extracted from:** explosive-materials-specifications.md  
**Purpose:** Quick reference for technical specifications and validation rules  
**Last Updated:** 2025-10-04  

---

## ANFO (Ammonium Nitrate Fuel Oil) Specifications

### Composition Standards
- **Primary Components:**
  - 94-95% Porous Prilled Ammonium Nitrate (NH₄NO₃)
  - 5-6% Fuel Oil (Number 2 diesel/road diesel)

### Physical Properties Reference

#### Density Specifications
| Property | Value | Unit | Notes |
|----------|-------|------|-------|
| Bulk Density | 800-900 | kg/m³ | Standard reference: 810 kg/m³ |
| Loose Pour Density | 800-850 | kg/m³ | Natural settling |
| Pneumatic Loading | ~1,100 | kg/m³ | At 4 bar pressure |
| Prill Density | ~1,300 | kg/m³ | Individual prill |
| Crystal Density | 1,700 | kg/m³ | Pure crystal |

#### Performance Characteristics
| Property | Value | Unit | Conditions |
|----------|-------|------|------------|
| Velocity of Detonation | 3,200 | m/s | 130mm diameter, unconfined |
| VOD Range | 2,700-4,600 | m/s | Varies with conditions |
| Critical Diameter | ≥50 | mm | Minimum |
| Recommended Diameter | ≥75 | mm | Optimal |
| Borehole Diameter | ≥100 | mm | Recommended |

### Quality Control Parameters

| Parameter | Specification | Unit | Tolerance | Validation |
|-----------|--------------|------|-----------|------------|
| Fuel Oil Content | 5.5-6.0 | % by weight | ±0.3% | Required |
| Bulk Density | 800-900 | kg/m³ | ±50 kg/m³ | Required |
| Moisture Content | <0.2 | % | Max | Optional |
| Prill Size | 1-3 | mm | Standard | Optional |
| Detonation Velocity | 3,000-3,500 | m/s | Test required | Optional |

### Storage Requirements
| Parameter | Specification | Unit | Critical |
|-----------|--------------|------|----------|
| Temperature Range | 5-35 | °C | Yes |
| Humidity | <50 | % RH | Yes |
| Water Resistance | Zero | - | Must stay dry |
| Shelf Life (Dry) | 6-12 | months | - |
| Shelf Life (Mixed) | 1-2 | months | - |

### Manufacturing Grades
- **FGAN (Fertilizer Grade):** Higher density, lower porosity - NOT for explosives
- **TGAN (Technical Grade):** Lower density, higher porosity - FOR explosives

### Safety Parameters
- **Fume Class:** Class 1 (when properly balanced)
- **Sensitivity:** Low (requires booster)
- **Minimum Primer:** 150-200g Pentolite or equivalent

---

## Emulsion Explosives Specifications

### Composition Standards
- **Oxidizer Phase (70-85%):** Ammonium Nitrate, Sodium Nitrate, Water (8-20%)
- **Fuel Phase (5-10%):** Mineral oil, Wax, Emulsifiers
- **Sensitizers (5-15%):** Glass microspheres, Perlite, Sodium nitrite solutions

### Physical Properties Reference

#### Density Specifications
| Property | Value | Unit | Application |
|----------|-------|------|-------------|
| Unsensitized Density | 1,300-1,450 | kg/m³ | Manufacturing |
| Sensitized Density | 1,100-1,300 | kg/m³ | Field use |
| Target Working Density | 1,150-1,250 | kg/m³ | Optimal performance |
| Low Density Grades | 800-1,000 | kg/m³ | Special applications |

#### Rheological Properties
| Property | Value | Unit | Temperature |
|----------|-------|------|-------------|
| Viscosity @ 20°C | 50,000-200,000 | cP | Standard |
| Viscosity @ 40°C | 20,000-80,000 | cP | Elevated |
| Pumpable Range | <100,000 | cP | Field limit |
| Flow Behavior | Non-Newtonian, thixotropic | - | - |

#### Water Content Standards
| Property | Value | Unit | Purpose |
|----------|-------|------|---------|
| Minimum Water | 8 | % | Structural integrity |
| Maximum Water | 20 | % | Performance limit |
| Optimal Range | 12-16 | % | Best performance |

### Quality Control Parameters

| Parameter | Specification | Unit | Method | Validation |
|-----------|--------------|------|--------|------------|
| Density (unsensitized) | 1,300-1,450 | kg/m³ | Pycnometer | Required |
| Density (sensitized) | 1,100-1,300 | kg/m³ | Direct measurement | Required |
| Viscosity @ 20°C | 50,000-200,000 | cP | Brookfield viscometer | Required |
| Water Content | 12-16 | % | Karl Fischer titration | Required |
| pH Value | 4.5-6.5 | - | pH meter | Required |
| Detonation Velocity | 4,500-6,000 | m/s | VOD measurement | Optional |
| Bubble Size | 10-100 | μm | Microscopy | Optional |

### Temperature Specifications
| Parameter | Range | Unit | Application |
|-----------|-------|------|-------------|
| Storage Temperature | -20 to 50 | °C | Long-term storage |
| Application Temperature | 0-45 | °C | Field use |
| Optimal Performance | 15-30 | °C | Best conditions |
| Critical Temperature | >60 | °C | Degradation risk |

### Performance Characteristics
| Property | Value | Unit | Notes |
|----------|-------|------|-------|
| Water Resistance | Excellent | - | 100% water-resistant |
| Detonation Velocity | 4,500-6,000 | m/s | Performance range |
| Critical Diameter | 25-50 | mm | Minimum |
| Fume Class | Class 1 | - | Safe when balanced |
| Sensitivity | Medium | - | Requires booster |

### Shelf Life & Stability
| Parameter | Duration | Unit | Conditions |
|-----------|----------|------|------------|
| Bulk Storage | 3-6 | months | Proper conditions |
| Packaged Storage | 6-12 | months | Sealed containers |

### Stability Indicators
- ✅ No phase separation
- ✅ Viscosity within range  
- ✅ No crystallization
- ✅ Consistent color

### Color Coding Standards
| Grade | Color | Purpose |
|-------|-------|---------|
| Standard | White to off-white | General use |
| Dense grades | Pink or red | High density |
| Water-resistant | Blue tint | Marine/wet conditions |
| Deviations | Any other color | Quality issues |

---

## Validation Rules for Implementation

### ANFO Validation Rules
```typescript
const ANFOValidation = {
  density: { min: 0.8, max: 0.9, unit: 'g/cm³', required: true },
  fuelOilContent: { min: 5.5, max: 6.0, unit: '%', required: true },
  moistureContent: { max: 0.2, unit: '%', required: false },
  prillSize: { min: 1, max: 3, unit: 'mm', required: false },
  detonationVelocity: { min: 3000, max: 3500, unit: 'm/s', required: false },
  shelfLife: { value: 12, unit: 'months' },
  storageTemp: { min: 5, max: 35, unit: '°C', required: true },
  storageHumidity: { max: 50, unit: '%', required: true }
};
```

### Emulsion Validation Rules
```typescript
const EmulsionValidation = {
  densityUnsensitized: { min: 1.30, max: 1.45, unit: 'g/cm³', required: true },
  densitySensitized: { min: 1.10, max: 1.30, unit: 'g/cm³', required: true },
  viscosity: { min: 50000, max: 200000, unit: 'cP', required: true },
  waterContent: { min: 12, max: 16, unit: '%', required: true },
  pH: { min: 4.5, max: 6.5, required: true },
  detonationVelocity: { min: 4500, max: 6000, unit: 'm/s', required: false },
  bubbleSize: { min: 10, max: 100, unit: 'μm', required: false },
  shelfLife: { value: 6, unit: 'months' },
  storageTemp: { min: -20, max: 50, unit: '°C', required: true },
  applicationTemp: { min: 0, max: 45, unit: '°C', required: false }
};
```

---

## Quality Check Alert Thresholds

### Alert Configuration
| Alert Type | ANFO Threshold | Emulsion Threshold | Action Required |
|-----------|----------------|-------------------|-----------------|
| Expiry Warning | 30 days | 30 days | Plan usage/disposal |
| Expiry Critical | 7 days | 7 days | Immediate action |
| Humidity Warning | >40% RH | N/A | Check storage |
| Temperature Warning | >30°C | >40°C | Cooling required |
| Density Out of Range | ±10% | ±5% | Quality check |
| Viscosity High | N/A | >150,000 cP | Heating/mixing |

### Mandatory Quality Checks

#### Upon Receipt
- ✅ Visual inspection
- ✅ Density measurement  
- ✅ Documentation verification

#### Monthly Monitoring
- ✅ Storage condition verification
- ✅ Expiry date monitoring
- ✅ Physical property checks

#### Before Transfer
- ✅ Quality status confirmation
- ✅ Quantity verification
- ✅ Condition assessment

---

## Data Model Specifications

### ANFO Inventory Model
```typescript
interface ANFOInventory {
  // Core Identification
  batchId: string;                    // Format: ANFO-YYYY-XXX
  manufacturingDate: Date;
  expiryDate: Date;                   // Manufacturing date + 12 months

  // Quality Parameters (Required)
  density: number;                    // 0.8 - 0.9 g/cm³
  fuelOilContent: number;            // 5.5 - 6.0 %
  storageTemperature: number;        // 5-35°C
  storageHumidity: number;           // % RH (max 50%)

  // Quality Parameters (Optional)
  moistureContent?: number;          // < 0.2 %
  prillSize?: number;                // 1-3 mm
  detonationVelocity?: number;       // 3000-3500 m/s

  // Manufacturing
  grade: 'TGAN' | 'Standard';
  supplier: string;
  manufacturerBatchNumber?: string;

  // Constants
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

  // Quality Parameters (Required)
  densityUnsensitized: number;       // 1.30 - 1.45 g/cm³
  densitySensitized: number;         // 1.10 - 1.30 g/cm³
  viscosity: number;                 // 50,000 - 200,000 cP
  waterContent: number;              // 12 - 16 %
  pH: number;                        // 4.5 - 6.5
  storageTemperature: number;        // -20 to 50 °C

  // Quality Parameters (Optional)
  detonationVelocity?: number;       // 4500-6000 m/s
  bubbleSize?: number;               // 10-100 μm
  applicationTemperature?: number;   // 0 to 45 °C
  sensitizerContent?: number;        // % by weight

  // Manufacturing
  grade: 'Standard' | 'High-Density' | 'Low-Density' | 'Water-Resistant';
  supplier: string;
  manufacturerBatchNumber?: string;
  color: string;                     // White, Pink, Blue, etc.
  sensitizationType: 'Chemical' | 'Physical' | 'Hybrid';

  // Constants
  waterResistance: 'Excellent';      // Always water-resistant
}
```

---

## References & Standards

### Industry Standards
- ANFO explosive technical specifications from Dyno Nobel
- Scientific research on ANFO properties and quality control
- Industrial standards for water gel and emulsion explosives
- Mining safety regulations for explosive materials

### Compliance Requirements
- All specifications must meet industry safety standards
- Quality control parameters are mandatory for safety certification
- Storage conditions must be monitored continuously
- Expiry dates are strictly enforced for safety

---

*Technical Reference Guide*  
*Extracted from explosive-materials-specifications.md*  
*For implementation in DBMS Inventory System*