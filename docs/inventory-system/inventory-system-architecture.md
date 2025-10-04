# Inventory System Architecture

## System Overview

The Detonator-Based Mining System (DBMS) implements a three-tier inventory management system for explosive materials with **ONE central warehouse** and multiple local stores.

---

## System Flow

```
┌─────────────────────────────────────┐
│   CENTRAL WAREHOUSE (SINGLE)         │
│   (explosive-manager/inventory)      │
│   - Main inventory location          │
│   - ANFO batches                     │
│   - Emulsion batches                 │
│   - Manufacturing records            │
│   - Quality control data             │
└────────────────┬────────────────────┘
                 │
                 │ Transfer Requests
                 ↓
┌─────────────────────────────────────┐
│   Transfer Request System            │
│   (explosive-manager/requests)       │
│   - Request creation by stores       │
│   - Approval workflow                │
│   - Transfer tracking                │
│   - Warehouse → Store transfers      │
└────────────────┬────────────────────┘
                 │
                 │ Approved Transfers
                 ↓
┌─────────────────────────────────────┐
│   Local Stores (MULTIPLE)            │
│   (explosive-manager/stores)         │
│   - Store locations across regions   │
│   - Store-level inventory            │
│   - Local stock management           │
│   - Receive from central warehouse   │
└────────────────┬────────────────────┘
                 │
                 │ Consumption
                 ↓
┌─────────────────────────────────────┐
│   Project Sites                      │
│   - Actual usage                     │
│   - Detonation operations            │
└─────────────────────────────────────┘
```

---

## Three Core Components

### 1. Central Warehouse Inventory (explosive-manager/inventory)

**Purpose:** **SINGLE** central warehouse where manufactured explosives are received and stored before distribution to local stores.

**Important:** There is only ONE central warehouse in the system.

**Key Features:**
- Batch tracking (ANFO, Emulsion)
- Manufacturing date tracking
- Expiry date monitoring
- Quality control data
- Type-specific properties (density, viscosity, etc.)
- Supplier information

**Backend Entity:** `CentralInventory` or `WarehouseInventory` (To be created)

**Note:** Since there's only one warehouse, we don't need a warehouse ID or location field. The storage location field refers to sections/bays within the single central warehouse.

**Operations:**
- Receive new batches
- Track quality parameters
- Monitor expiry
- Allocate to transfer requests
- Generate inventory reports

---

### 2. Small Local Stores (explosive-manager/stores)

**Purpose:** Multiple local storage facilities that hold explosives closer to project sites.

**Key Features:**
- Store location management
- Store capacity tracking
- Manager assignment
- Regional distribution
- Safety compliance

**Backend Entity:** `Store` (Existing) + `StoreInventory` (Existing)

**Operations:**
- Create/manage stores
- Track store capacity
- Monitor stock levels
- Manage store personnel
- Track store status

---

### 3. Transfer Request System (explosive-manager/requests)

**Purpose:** Manage transfer of explosives from main warehouse to local stores.

**Key Features:**
- Request creation by store managers
- Approval workflow
- Quantity validation
- Transfer tracking
- Audit trail

**Backend Entity:** `InventoryTransferRequest` (To be created)

**Operations:**
- Create transfer request
- Approve/Reject requests
- Execute transfers
- Update both inventories
- Track transfer history

---

## Data Model Architecture

### Current Backend Entities (Existing)

```csharp
// 1. Store - Represents local storage facilities
public class Store
{
    public int Id { get; set; }
    public string StoreName { get; set; }
    public string StoreAddress { get; set; }
    public decimal StorageCapacity { get; set; }
    public decimal CurrentOccupancy { get; set; }
    public string City { get; set; }
    public StoreStatus Status { get; set; }
    public int RegionId { get; set; }
    public int? ManagerUserId { get; set; }
}

// 2. StoreInventory - Local store inventory items
public class StoreInventory
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public ExplosiveType ExplosiveType { get; set; }
    public decimal Quantity { get; set; }
    public decimal ReservedQuantity { get; set; }
    public string Unit { get; set; }
    public decimal MinimumStockLevel { get; set; }
    public decimal MaximumStockLevel { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? BatchNumber { get; set; }
}
```

### Required New Entities

```csharp
// 3. CentralInventory - Single central warehouse batches
public class CentralInventory : BaseAuditableEntity
{
    public string BatchId { get; private set; }
    public ExplosiveType ExplosiveType { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal AvailableQuantity { get; private set; }
    public decimal AllocatedQuantity { get; private set; }
    public DateTime ManufacturingDate { get; private set; }
    public DateTime ExpiryDate { get; private set; }
    public string Supplier { get; private set; }
    public string? Grade { get; private set; }
    public string Unit { get; private set; }
    public string StorageLocation { get; private set; } // Bay/Section within central warehouse
    public string? TechnicalProperties { get; private set; } // JSON for type-specific data

    // Note: No warehouse ID needed - only one central warehouse exists
}

// 4. InventoryTransferRequest - Transfer from central warehouse to stores
public class InventoryTransferRequest : BaseAuditableEntity
{
    public int CentralInventoryId { get; private set; }  // Source: Always from central warehouse
    public int DestinationStoreId { get; private set; }  // To: Specific local store
    public ExplosiveType ExplosiveType { get; private set; }
    public decimal RequestedQuantity { get; private set; }
    public decimal? ApprovedQuantity { get; private set; }
    public TransferStatus Status { get; private set; }
    public int RequestedByUserId { get; private set; }  // Store manager requests
    public int? ApprovedByUserId { get; private set; }   // Warehouse manager approves
    public DateTime? ApprovedAt { get; private set; }
    public string? RejectionReason { get; private set; }
}

public enum TransferStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    InTransit = 4,
    Completed = 5,
    Cancelled = 6
}
```

---

## API Architecture

### 1. Warehouse Inventory API

**Base URL:** `/api/warehouse-inventory`

**Endpoints:**
- `GET /` - Get all warehouse inventory
- `GET /{id}` - Get specific batch
- `GET /type/{explosiveType}` - Get by explosive type
- `GET /expiring?days=30` - Get expiring batches
- `GET /low-stock` - Get low stock items
- `GET /statistics` - Get inventory statistics
- `POST /` - Add new batch
- `PUT /{id}` - Update batch
- `DELETE /{id}` - Remove batch

### 2. Transfer Requests API

**Base URL:** `/api/transfer-requests`

**Endpoints:**
- `GET /` - Get all transfer requests
- `GET /{id}` - Get specific request
- `GET /store/{storeId}` - Get requests for store
- `GET /pending` - Get pending requests
- `GET /history` - Get transfer history
- `POST /` - Create transfer request
- `PUT /{id}/approve` - Approve request
- `PUT /{id}/reject` - Reject request
- `PUT /{id}/complete` - Complete transfer
- `PUT /{id}/cancel` - Cancel request

### 3. Store Inventory API (Existing)

**Base URL:** `/api/store-inventory`

**Endpoints:**
- Similar to warehouse but for local stores

---

## Business Rules

### Warehouse Inventory Rules

1. **Batch Tracking**
   - Each batch must have unique BatchId
   - Manufacturing date cannot be in future
   - Expiry date must be after manufacturing date
   - Cannot add stock to expired batches

2. **Quantity Management**
   - Available Quantity = Total Quantity - Allocated Quantity
   - Cannot allocate more than available
   - Cannot delete batch with active allocations

3. **Quality Control**
   - Type-specific properties must meet standards
   - Expired batches should be flagged
   - Low stock alerts for critical items

### Transfer Request Rules

1. **Request Creation**
   - Store must exist and be active
   - Requested quantity must be > 0
   - Warehouse must have sufficient available quantity
   - User must have permission

2. **Approval Process**
   - Only authorized users can approve
   - Approved quantity ≤ Requested quantity
   - Approved quantity ≤ Available warehouse quantity
   - Approved quantity + Store current ≤ Store capacity

3. **Transfer Execution**
   - Deduct from warehouse available quantity
   - Add to store inventory
   - Update transfer status to Completed
   - Create audit trail

### Store Inventory Rules

1. **Capacity Management**
   - Total inventory cannot exceed store capacity
   - Must maintain minimum stock levels
   - Alert when approaching maximum capacity

2. **Stock Levels**
   - Reserved quantity cannot exceed total quantity
   - Available = Total - Reserved
   - Low stock alert when below minimum

---

## Frontend-Backend Mapping

### Frontend Components → Backend APIs

| Frontend Component | Backend API | Entity |
|-------------------|-------------|--------|
| `explosive-manager/inventory` | `/api/warehouse-inventory` | `WarehouseInventory` |
| `explosive-manager/stores` | `/api/storemanagement` | `Store` |
| `explosive-manager/requests` | `/api/transfer-requests` | `InventoryTransferRequest` |
| Store inventory view | `/api/store-inventory` | `StoreInventory` |

---

## Database Schema

### Tables

1. **WarehouseInventory**
   - Primary key: Id
   - Unique: BatchId
   - Indexes: ExplosiveType, ExpiryDate

2. **InventoryTransferRequest**
   - Primary key: Id
   - Foreign keys: WarehouseInventoryId, DestinationStoreId, RequestedByUserId, ApprovedByUserId
   - Indexes: Status, DestinationStoreId, CreatedAt

3. **Store** (Existing)
   - Primary key: Id
   - Indexes: RegionId, ManagerUserId

4. **StoreInventory** (Existing)
   - Primary key: Id
   - Foreign keys: StoreId
   - Indexes: StoreId, ExplosiveType

### Relationships

```
WarehouseInventory 1 ──── * InventoryTransferRequest
Store 1 ──── * InventoryTransferRequest
Store 1 ──── * StoreInventory
User 1 ──── * InventoryTransferRequest (RequestedBy)
User 1 ──── * InventoryTransferRequest (ApprovedBy)
```

---

## Security & Permissions

### Role-Based Access

| Role | Warehouse Inventory | Transfer Requests | Store Inventory |
|------|-------------------|-------------------|-----------------|
| Admin | Full access | Full access | Full access |
| Explosive Manager | Full access | Approve/Reject | View all |
| Store Manager | View | Create for own store | Manage own store |
| Mechanical Engineer | View | View | View |

### Audit Requirements

- All inventory changes logged
- Transfer requests tracked with timestamps
- Approval/rejection reasons recorded
- User actions auditable

---

## Implementation Phases

### Phase 1: Warehouse Inventory (Current Priority)
1. Create `WarehouseInventory` entity
2. Create DTOs and validators
3. Implement repository and service
4. Create API controller
5. Add frontend service and models
6. Connect UI components

### Phase 2: Transfer Request System
1. Create `InventoryTransferRequest` entity
2. Implement approval workflow
3. Create transfer logic
4. Build request UI
5. Add notifications

### Phase 3: Integration & Reports
1. Link all systems
2. Create dashboards
3. Generate reports
4. Add analytics

---

## Notes

- All quantities in kilograms (kg) unless specified
- Dates stored in UTC
- Batch numbers follow format: `{Type}-{Year}-{Sequence}` (e.g., ANFO-2025-001)
- Expiry alerts sent 30 days before expiration
- Low stock alerts when below minimum level

---

*Document Version: 1.0*
*Last Updated: 2025-10-04*
*Author: System Architecture Team*
