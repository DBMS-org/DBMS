# Database Schema & Migration Guide

Complete database documentation for DBMS (Drilling & Blasting Management System).

**Database Provider**: SQL Server / SQL Server LocalDB
**ORM**: Entity Framework Core 8
**Migration Strategy**: Code-First with EF Core Migrations

---

## Table of Contents

1. [Database Overview](#database-overview)
2. [Entity Relationships](#entity-relationships)
3. [Core Tables](#core-tables)
4. [Module-Specific Tables](#module-specific-tables)
5. [Indexes & Constraints](#indexes--constraints)
6. [Migration Management](#migration-management)
7. [Seeded Data](#seeded-data)
8. [Common Queries](#common-queries)

---

## Database Overview

### Connection String

**Development** (`appsettings.Development.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DBMS;Trusted_Connection=true;"
  }
}
```

**Production** (configure in `appsettings.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=DBMS;User Id=your-user;Password=your-password;TrustServerCertificate=True;"
  }
}
```

### Database Context

**Location**: `Infrastructure/Data/ApplicationDbContext.cs`

**Key Features**:
- Automatic configuration discovery via `ApplyConfigurationsFromAssembly`
- Domain event dispatcher integration
- Audit tracking via `BaseAuditableEntity`
- Seeded master data (Roles, Permissions, Regions)

---

## Entity Relationships

### High-Level ER Diagram

```
┌─────────┐         ┌──────────────┐         ┌─────────────┐
│  User   │────────>│   Project    │<────────│   Region    │
└─────────┘         └──────────────┘         └─────────────┘
     │                      │                        │
     │                      │                        │
     ▼                      ▼                        ▼
┌─────────┐         ┌──────────────┐         ┌─────────────┐
│  Store  │         │ ProjectSite  │         │   Machine   │
└─────────┘         └──────────────┘         └─────────────┘
     │                      │
     │                      │
     ▼                      ▼
┌──────────────┐    ┌──────────────┐
│StoreInventory│    │  DrillHole   │
└──────────────┘    └──────────────┘
                            │
                            ▼
                    ┌──────────────────────┐
                    │ ExplosiveCalculation │
                    └──────────────────────┘
```

---

## Core Tables

### Users

**Table**: `Users`
**Entity**: `Domain.Entities.UserManagement.User`
**Configuration**: `Infrastructure/Configurations/UserManagement/UserConfiguration.cs`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| Name | nvarchar(256) | NOT NULL | User full name |
| Email_Value | nvarchar(256) | UNIQUE, NOT NULL | Email (Value Object) |
| PasswordHash | nvarchar(MAX) | NOT NULL | BCrypt hashed password |
| Role | nvarchar(100) | NOT NULL | User role (e.g., Admin, BlastingEngineer) |
| Status | int | NOT NULL | UserStatus enum (0=Active, 1=Inactive) |
| Region | nvarchar(100) | NULL | Geographic region |
| Country | nvarchar(100) | NULL | Country |
| OmanPhone | nvarchar(20) | NULL | Oman phone number |
| CountryPhone | nvarchar(20) | NULL | Country phone number |
| LastLoginAt | datetime2 | NULL | Last login timestamp |
| PasswordResetCode | nvarchar(6) | NULL | 6-digit reset code |
| PasswordResetCodeExpiry | datetime2 | NULL | Reset code expiration |
| CreatedDate | datetime2 | NOT NULL | Creation timestamp (UTC) |
| UpdatedDate | datetime2 | NULL | Last update timestamp (UTC) |

**Indexes**:
- `IX_Users_Email` (UNIQUE) on `Email_Value`

**Navigation Properties**:
- `UserRoles` → Many-to-Many with Roles
- `ManagedStores` → One-to-Many with Stores

---

### Roles

**Table**: `Roles`
**Entity**: `Domain.Entities.UserManagement.Role`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| Name | nvarchar(100) | NOT NULL | Role name (e.g., "Admin") |
| NormalizedName | nvarchar(100) | NOT NULL | Uppercase name for lookups |
| Description | nvarchar(500) | NULL | Role description |

**Seeded Roles** (see [Seeded Data](#seeded-data)):
- Admin (ID: 1)
- Blasting Engineer (ID: 2)
- Mechanical Engineer (ID: 3)
- Machine Manager (ID: 4)
- Explosive Manager (ID: 5)
- Store Manager (ID: 6)
- Operator (ID: 7)

---

### Permissions

**Table**: `Permissions`
**Entity**: `Domain.Entities.UserManagement.Permission`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| Module | nvarchar(100) | NOT NULL | Module name (e.g., "UserManagement") |
| Action | nvarchar(50) | NOT NULL | Action type (Create, Read, Update, Delete) |
| Name | nvarchar(100) | NOT NULL | Display name |
| Description | nvarchar(500) | NULL | Permission description |

**Seeded Permissions**: UserManagement (Create, Read, Update, Delete), ProjectManagement (Create, Read, Update, Delete)

---

### Regions

**Table**: `Regions`
**Entity**: `Domain.Entities.ProjectManagement.Region`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| Name | nvarchar(100) | NOT NULL | Region name |
| Code | nvarchar(10) | NULL | Region code |
| Description | nvarchar(500) | NULL | Region description |

**Seeded Regions**:
- North (ID: 1)
- South (ID: 2)
- East (ID: 3)
- West (ID: 4)

---

## Module-Specific Tables

### Project Management

#### Projects

**Table**: `Projects`
**Entity**: `Domain.Entities.ProjectManagement.Project`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| Name | nvarchar(200) | NOT NULL | Project name |
| Region | nvarchar(100) | NOT NULL | Project region |
| Status | int | NOT NULL | ProjectStatus enum (0=Planned, 1=InProgress, 2=Completed, 3=OnHold) |
| Description | nvarchar(MAX) | NULL | Project description |
| StartDate | datetime2 | NULL | Project start date |
| EndDate | datetime2 | NULL | Project end date |
| AssignedUserId | int | FK → Users | Assigned user (nullable) |
| RegionId | int | FK → Regions | Region foreign key |
| CreatedDate | datetime2 | NOT NULL | Audit: Created date |
| UpdatedDate | datetime2 | NULL | Audit: Updated date |

**Relationships**:
- Many-to-One with User (AssignedUser)
- Many-to-One with Region
- One-to-Many with ProjectSites
- One-to-Many with Machines
- One-to-Many with Stores

---

#### ProjectSites

**Table**: `ProjectSites`
**Entity**: `Domain.Entities.ProjectManagement.ProjectSite`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| Name | nvarchar(200) | NOT NULL | Site name |
| ProjectId | int | FK → Projects, NOT NULL | Parent project |
| Latitude | decimal(9,6) | NULL | GPS latitude |
| Longitude | decimal(9,6) | NULL | GPS longitude |
| Status | int | NOT NULL | ProjectSiteStatus enum |
| Area | decimal(18,2) | NULL | Site area (sq meters) |
| CreatedDate | datetime2 | NOT NULL | Audit: Created date |
| UpdatedDate | datetime2 | NULL | Audit: Updated date |

**Cascade**: Delete Project → Delete ProjectSites

---

### Drilling Operations

#### DrillHoles

**Table**: `DrillHoles`
**Entity**: `Domain.Entities.DrillingOperations.DrillHole`
**Configuration**: `Infrastructure/Configurations/DrillingOperations/DrillHoleConfiguration.cs`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | nvarchar(50) | PK | Drill hole ID (e.g., "DH-001") |
| SerialNumber | int | NULL | Sequential number |
| Name | nvarchar(100) | NOT NULL | Hole name |
| Easting | float | NOT NULL | X coordinate |
| Northing | float | NOT NULL | Y coordinate |
| Elevation | float | NOT NULL | Z coordinate / elevation |
| Length | float | NOT NULL | Drill hole length |
| Depth | float | NOT NULL | Designed depth |
| Azimuth | float | NULL | Drill azimuth (3D) |
| Dip | float | NULL | Drill dip angle (3D) |
| ActualDepth | float | NOT NULL | Actual drilled depth |
| Stemming | float | NOT NULL | Stemming length |
| ProjectId | int | FK → Projects, NOT NULL | Parent project |
| SiteId | int | FK → ProjectSites, NOT NULL | Parent site |
| CreatedAt | datetime2 | NOT NULL | Creation timestamp |
| UpdatedAt | datetime2 | NOT NULL | Update timestamp |

**Computed Properties**:
- `Has3DData`: `Azimuth IS NOT NULL AND Dip IS NOT NULL`
- `RequiresFallbackTo2D`: `Azimuth IS NULL OR Dip IS NULL`

**Indexes**:
- `IX_DrillHoles_ProjectId_SiteId` on (ProjectId, SiteId)

---

#### DrillPoints

**Table**: `DrillPoints`
**Entity**: `Domain.Entities.DrillingOperations.DrillPoint`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| X | float | NOT NULL | X coordinate |
| Y | float | NOT NULL | Y coordinate |
| Z | float | NULL | Z coordinate (3D) |
| DrillHoleId | nvarchar(50) | FK → DrillHoles | Associated drill hole |
| PatternSettingsId | int | FK → PatternSettings | Pattern configuration |
| CreatedAt | datetime2 | NOT NULL | Creation timestamp |

---

### Blasting Operations

#### SiteBlastingData

**Table**: `SiteBlastingData`
**Entity**: `Domain.Entities.BlastingOperations.SiteBlastingData`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| ProjectSiteId | int | FK → ProjectSites, NOT NULL | Associated site |
| BlastingDate | datetime2 | NULL | Scheduled blast date |
| Status | nvarchar(50) | NULL | Blasting status |
| CreatedDate | datetime2 | NOT NULL | Creation timestamp |
| UpdatedDate | datetime2 | NULL | Update timestamp |

---

#### ExplosiveCalculationResults

**Table**: `ExplosiveCalculationResults`
**Entity**: `Domain.Entities.DrillingOperations.ExplosiveCalculationResult`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| DrillHoleId | nvarchar(50) | FK → DrillHoles | Associated drill hole |
| ExplosiveType | nvarchar(100) | NULL | Type of explosive |
| Quantity | decimal(18,2) | NOT NULL | Explosive quantity (kg) |
| CalculationDate | datetime2 | NOT NULL | Calculation timestamp |
| Notes | nvarchar(MAX) | NULL | Additional notes |

---

#### BlastConnections

**Table**: `BlastConnections`
**Entity**: `Domain.Entities.BlastingOperations.BlastConnection`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| SourceDrillHoleId | nvarchar(50) | FK → DrillHoles | Source hole |
| TargetDrillHoleId | nvarchar(50) | FK → DrillHoles | Target hole |
| ConnectionType | nvarchar(50) | NULL | Connection type |
| Delay | int | NULL | Delay time (ms) |
| SiteBlastingId | int | FK → SiteBlastingData | Parent blasting record |

---

#### ExplosiveApprovalRequests

**Table**: `ExplosiveApprovalRequests`
**Entity**: `Domain.Entities.ProjectManagement.ExplosiveApprovalRequest`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| ProjectId | int | FK → Projects, NOT NULL | Associated project |
| RequestedBy | int | FK → Users | Requester user ID |
| ApprovedBy | int | FK → Users | Approver user ID |
| RequestDate | datetime2 | NOT NULL | Request timestamp |
| ApprovalDate | datetime2 | NULL | Approval timestamp |
| Status | int | NOT NULL | Approval status (0=Pending, 1=Approved, 2=Rejected) |
| Quantity | decimal(18,2) | NOT NULL | Requested quantity |
| ExplosiveType | nvarchar(100) | NULL | Type of explosive |
| Justification | nvarchar(MAX) | NULL | Request justification |

---

### Store Management

#### Stores

**Table**: `Stores`
**Entity**: `Domain.Entities.StoreManagement.Store`
**Configuration**: `Infrastructure/Configurations/StoreManagement/StoreConfiguration.cs`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| StoreName | nvarchar(200) | NOT NULL | Store name |
| StoreAddress | nvarchar(500) | NOT NULL | Physical address |
| StoreManagerName | nvarchar(200) | NOT NULL | Manager name |
| StoreManagerContact | nvarchar(20) | NOT NULL | Manager phone |
| StoreManagerEmail | nvarchar(256) | NOT NULL | Manager email |
| StorageCapacity | decimal(18,2) | NOT NULL | Max capacity (kg) |
| CurrentOccupancy | decimal(18,2) | NOT NULL | Current stock (kg) |
| City | nvarchar(100) | NOT NULL | City location |
| Status | int | NOT NULL | StoreStatus enum (0=Operational, 1=Maintenance, 2=Decommissioned) |
| RegionId | int | FK → Regions, NOT NULL | Associated region |
| ManagerUserId | int | FK → Users | Assigned manager |
| CreatedDate | datetime2 | NOT NULL | Audit: Created date |
| UpdatedDate | datetime2 | NULL | Audit: Updated date |

**Business Rules** (enforced in entity):
- `CurrentOccupancy` ≤ `StorageCapacity`
- Cannot change status of decommissioned store
- Utilization rate = (CurrentOccupancy / StorageCapacity) × 100

**Indexes**:
- `IX_Stores_RegionId` on RegionId
- `IX_Stores_ManagerUserId` on ManagerUserId

---

#### StoreInventories

**Table**: `StoreInventories`
**Entity**: `Domain.Entities.StoreManagement.StoreInventory`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| StoreId | int | FK → Stores, NOT NULL | Parent store |
| ExplosiveType | int | NOT NULL | ExplosiveType enum |
| Quantity | decimal(18,2) | NOT NULL | Current quantity (kg) |
| MinimumStockLevel | decimal(18,2) | NOT NULL | Reorder threshold |
| MaximumStockLevel | decimal(18,2) | NOT NULL | Maximum allowed |
| UnitOfMeasure | nvarchar(20) | NOT NULL | Unit (e.g., "kg") |
| LastRestockDate | datetime2 | NULL | Last restock timestamp |
| CreatedDate | datetime2 | NOT NULL | Audit: Created date |
| UpdatedDate | datetime2 | NULL | Audit: Updated date |

**Cascade**: Delete Store → Delete StoreInventories

---

#### StoreTransactions

**Table**: `StoreTransactions`
**Entity**: `Domain.Entities.StoreManagement.StoreTransaction`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| StoreId | int | FK → Stores, NOT NULL | Associated store |
| TransactionType | int | NOT NULL | TransactionType enum (0=In, 1=Out, 2=Transfer, 3=Adjustment) |
| ExplosiveType | int | NOT NULL | ExplosiveType enum |
| Quantity | decimal(18,2) | NOT NULL | Transaction quantity |
| TransactionDate | datetime2 | NOT NULL | Transaction timestamp |
| PerformedByUserId | int | FK → Users | User who performed transaction |
| Notes | nvarchar(MAX) | NULL | Transaction notes |
| ReferenceNumber | nvarchar(50) | NULL | External reference |
| CreatedDate | datetime2 | NOT NULL | Audit: Created date |

**Indexes**:
- `IX_StoreTransactions_StoreId` on StoreId
- `IX_StoreTransactions_TransactionDate` on TransactionDate

---

### Machine Management

#### Machines

**Table**: `Machines`
**Entity**: `Domain.Entities.MachineManagement.Machine`

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | PK, Identity | Primary key |
| Name | nvarchar(200) | NOT NULL | Machine name |
| Type | nvarchar(100) | NULL | Machine type |
| Status | int | NOT NULL | MachineStatus enum (0=Available, 1=InUse, 2=Maintenance, 3=Retired) |
| ProjectId | int | FK → Projects | Assigned project |
| Location | nvarchar(200) | NULL | Current location |
| SerialNumber | nvarchar(100) | NULL | Serial number |
| CreatedDate | datetime2 | NOT NULL | Audit: Created date |
| UpdatedDate | datetime2 | NULL | Audit: Updated date |

---

## Indexes & Constraints

### Primary Keys
All entities use `Id` as primary key (int, IDENTITY) except:
- **DrillHoles**: Uses `Id` (nvarchar) as natural key

### Foreign Keys with Cascade Delete
- Projects → ProjectSites (CASCADE)
- Stores → StoreInventories (CASCADE)
- ProjectSites → DrillHoles (RESTRICT by default)

### Unique Constraints
- `Users.Email_Value` (Unique index)
- Various composite unique indexes for business rules

### Check Constraints
Applied via Fluent API validations:
- Store: `CurrentOccupancy <= StorageCapacity`
- StoreInventory: `Quantity >= 0`
- DrillHole: `ActualDepth >= 0`

---

## Migration Management

### Creating Migrations

```bash
cd Presentation/API

# Create new migration
dotnet ef migrations add MigrationName

# Example: Add new column
dotnet ef migrations add AddPhoneNumberToUsers
```

### Applying Migrations

```bash
# Update database to latest migration
dotnet ef database update

# Update to specific migration
dotnet ef database update MigrationName

# Rollback to previous migration
dotnet ef database update PreviousMigrationName
```

### Removing Migrations

```bash
# Remove last migration (if not applied to database)
dotnet ef migrations remove

# Force remove (if already applied - DANGEROUS)
dotnet ef database update PreviousMigration
dotnet ef migrations remove
```

### Migration History

**Location**: `Infrastructure/Migrations/`

**Key Migrations**:
1. `20250602042904_InitialCreate` - Initial database schema
2. `20250602051352_AddAuthenticationFields` - Added JWT authentication
3. `20251001062013_AddStoreManagementTables` - Store management module

**View Applied Migrations**:
```sql
SELECT * FROM __EFMigrationsHistory ORDER BY MigrationId DESC;
```

---

## Seeded Data

### Default Roles
Seeded in `ApplicationDbContext.cs:63-71`:

| ID | Name | NormalizedName | Description |
|----|------|----------------|-------------|
| 1 | Admin | ADMIN | Administrator with full access |
| 2 | Blasting Engineer | BLASTING_ENGINEER | Manages blasting operations |
| 3 | Mechanical Engineer | MECHANICAL_ENGINEER | Manages mechanical tasks |
| 4 | Machine Manager | MACHINE_MANAGER | Manages machine inventory |
| 5 | Explosive Manager | EXPLOSIVE_MANAGER | Manages explosive materials |
| 6 | Store Manager | STORE_MANAGER | Manages store inventory |
| 7 | Operator | OPERATOR | Operates machinery |

### Default Regions
Seeded in `ApplicationDbContext.cs:100+`:

| ID | Name | Code |
|----|------|------|
| 1 | North | N |
| 2 | South | S |
| 3 | East | E |
| 4 | West | W |

### Default Permissions
Basic CRUD permissions for UserManagement and ProjectManagement modules.

---

## Common Queries

### Get All Users with Roles
```sql
SELECT u.Id, u.Name, u.Email_Value, r.Name AS RoleName
FROM Users u
LEFT JOIN UserRoles ur ON u.Id = ur.UserId
LEFT JOIN Roles r ON ur.RoleId = r.Id;
```

### Get Projects with Sites
```sql
SELECT p.Id, p.Name AS ProjectName, ps.Name AS SiteName, ps.Status
FROM Projects p
LEFT JOIN ProjectSites ps ON p.Id = ps.ProjectId
WHERE p.Status = 1  -- InProgress
ORDER BY p.StartDate DESC;
```

### Get Drill Holes for Site
```sql
SELECT dh.Id, dh.Name, dh.Easting, dh.Northing, dh.ActualDepth
FROM DrillHoles dh
WHERE dh.ProjectId = @projectId AND dh.SiteId = @siteId
ORDER BY dh.SerialNumber;
```

### Get Store Utilization
```sql
SELECT
    s.Id,
    s.StoreName,
    s.StorageCapacity,
    s.CurrentOccupancy,
    CAST((s.CurrentOccupancy / s.StorageCapacity * 100) AS DECIMAL(5,2)) AS UtilizationPercent
FROM Stores s
WHERE s.Status = 0  -- Operational
ORDER BY UtilizationPercent DESC;
```

### Get Low Stock Items
```sql
SELECT
    s.StoreName,
    si.ExplosiveType,
    si.Quantity,
    si.MinimumStockLevel
FROM StoreInventories si
INNER JOIN Stores s ON si.StoreId = s.Id
WHERE si.Quantity < si.MinimumStockLevel
ORDER BY s.StoreName, si.ExplosiveType;
```

### Transaction History
```sql
SELECT
    st.TransactionDate,
    s.StoreName,
    st.TransactionType,
    st.ExplosiveType,
    st.Quantity,
    u.Name AS PerformedBy
FROM StoreTransactions st
INNER JOIN Stores s ON st.StoreId = s.Id
LEFT JOIN Users u ON st.PerformedByUserId = u.Id
WHERE st.TransactionDate >= DATEADD(day, -30, GETDATE())
ORDER BY st.TransactionDate DESC;
```

---

## Database Backup & Restore

### Backup
```sql
BACKUP DATABASE [DBMS]
TO DISK = 'C:\Backups\DBMS_backup.bak'
WITH FORMAT, MEDIANAME = 'DBMSBackup', NAME = 'Full Backup of DBMS';
```

### Restore
```sql
RESTORE DATABASE [DBMS]
FROM DISK = 'C:\Backups\DBMS_backup.bak'
WITH REPLACE;
```

### Using EF Core
```bash
# Script all migrations to SQL file
dotnet ef migrations script -o migrations.sql

# Apply to production database
sqlcmd -S production-server -d DBMS -i migrations.sql
```

---

## Performance Optimization

### Recommended Indexes
```sql
-- User lookups
CREATE NONCLUSTERED INDEX IX_Users_Role ON Users(Role);
CREATE NONCLUSTERED INDEX IX_Users_Region ON Users(Region);

-- Drill hole queries
CREATE NONCLUSTERED INDEX IX_DrillHoles_ProjectId_SiteId ON DrillHoles(ProjectId, SiteId);

-- Transaction queries
CREATE NONCLUSTERED INDEX IX_StoreTransactions_Date_Type ON StoreTransactions(TransactionDate, TransactionType);

-- Inventory lookups
CREATE NONCLUSTERED INDEX IX_StoreInventories_ExplosiveType ON StoreInventories(ExplosiveType);
```

### Query Optimization
- Use `.AsNoTracking()` for read-only queries
- Implement pagination for large result sets
- Use `.Include()` for eager loading related entities
- Avoid SELECT * - specify only needed columns

---

## Troubleshooting

### Common Issues

1. **Migration Pending**: Run `dotnet ef database update`
2. **Duplicate Key**: Check seeded data and existing records
3. **FK Constraint**: Ensure parent records exist before insert
4. **Connection Timeout**: Increase timeout in connection string

See [TROUBLESHOOTING.md](TROUBLESHOOTING.md#database-issues) for detailed solutions.

---

**Last Updated**: October 2, 2025
