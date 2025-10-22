# CLAUDE.md – AI Assistant Context

This document provides essential context for AI assistants working on the DBMS codebase.

## Project Overview

**DBMS (Drilling & Blasting Management System)** is a production-ready enterprise application for managing drilling operations, explosive calculations, and blasting workflows in mining/construction contexts.

- **Architecture**: Clean Architecture (Onion)
- **Backend**: ASP.NET Core 8 Web API (.NET 8)
- **Frontend**: Angular 19 SPA
- **Database**: SQL Server / SQL Express
- **Authentication**: JWT (HS256) with BCrypt password hashing

## Key Architecture Principles

### Layer Structure
```
Domain ← Application ← Infrastructure
          ↑
     Presentation
```

**Dependency Rule**: Dependencies point INWARD only. Outer layers depend on inner layers, never vice-versa.

### Layer Responsibilities

| Layer | Purpose | Key Artifacts |
|-------|---------|---------------|
| **Domain** | Pure business entities (POCOs) | `User`, `Store`, `DrillHole`, `Project`, `SiteBlasting` |
| **Application** | Business logic, DTOs, Service interfaces | `I*Service`, `I*Repository`, `*Dto`, `*Request` |
| **Infrastructure** | Data access, EF Core, external services | `ApplicationDbContext`, `*Repository`, `JwtService`, `PasswordService` |
| **Presentation** | HTTP API + Angular UI | Controllers, Components, Services |

### Critical Rules
1. **Domain layer** has NO external dependencies (no EF Core, ASP.NET, etc.)
2. **Application layer** references ONLY Domain (+ Microsoft.Extensions.*)
3. **Infrastructure** implements Application interfaces but contains NO business logic
4. **Presentation** depends on Application abstractions, never directly on Infrastructure or Domain

## Project Structure

```
DBMS/
├─ Domain/                          # Pure C# entities
│  └─ Entities/
│     ├─ UserManagement/            # User, Role, Permission
│     ├─ ProjectManagement/         # Project, ProjectSite, Region
│     ├─ DrillingOperations/        # DrillHole, DrillPoint, DrillPattern
│     ├─ BlastingOperations/        # SiteBlasting, ExplosiveCalculation, BlastConnection
│     └─ StoreManagement/           # Store, StoreInventory, StoreTransaction
│
├─ Application/                     # Business layer
│  ├─ Interfaces/                   # Service & Repository contracts
│  ├─ Services/                     # Business logic implementation
│  ├─ DTOs/                         # Data Transfer Objects
│  ├─ Validators/                   # FluentValidation rules
│  └─ Mapping/                      # AutoMapper profiles
│
├─ Infrastructure/                  # Data & external concerns
│  ├─ Data/                         # ApplicationDbContext
│  ├─ Migrations/                   # EF Core migrations
│  ├─ Repositories/                 # Repository implementations
│  ├─ Configurations/               # EF entity configurations
│  └─ Services/                     # JWT, Email, Password hashing
│
└─ Presentation/
   ├─ API/                          # ASP.NET Core Web API
   │  ├─ Controllers/               # REST endpoints
   │  ├─ Middleware/                # GlobalExceptionMiddleware, AuditPerformanceMiddleware
   │  └─ Program.cs                 # DI container & pipeline setup
   └─ UI/                           # Angular 19 SPA
      └─ src/
         ├─ app/
         │  ├─ core/                # Guards, Interceptors, Services
         │  ├─ shared/              # Reusable components
         │  └─ components/          # Feature components by module
         └─ environments/
```

## Key Modules & Features

### 1. User Management
- **Entities**: `User`, `Role`, `Permission`
- **Auth**: JWT tokens, BCrypt passwords, role-based authorization
- **Services**: `IUserService`, `IAuthService`
- **Policies**: `RequireAdminRole`, `RequireUserRole`, `RequireOwnership`

### 2. Project Management
- **Entities**: `Project`, `ProjectSite`, `Region`
- **Features**: Multi-site projects, regional organization
- **Services**: `IProjectService`, `IProjectSiteService`, `IRegionService`

### 3. Drilling Operations
- **Entities**: `DrillHole`, `DrillPoint`, `DrillPattern`
- **Features**: CSV import, pattern generation, drill data validation
- **Services**: `IDrillHoleService`, `ICsvImportService`, `IDrillPointPatternService`

### 4. Blasting Operations
- **Entities**: `SiteBlasting`, `ExplosiveCalculationResult`, `BlastConnection`, `ExplosiveApprovalRequest`
- **Features**: Explosive calculations, blast sequencing, approval workflows
- **Services**: `ISiteBlastingService`, `IExplosiveCalculationResultService`, `IBlastConnectionService`

### 5. Store Management (Recently Added)
- **Entities**: `Store`, `StoreInventory`, `StoreTransaction`
- **Features**: Inventory tracking, transaction history, explosive storage
- **Services**: `IStoreService`, `IStoreInventoryService`, `IStoreTransactionService`

## Technology Stack Details

### Backend (.NET 8)
- **ORM**: Entity Framework Core 8 (SQL Server provider)
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Security**: JWT Bearer tokens, BCrypt.Net-Next
- **Logging**: Microsoft.Extensions.Logging
- **Patterns**: Repository Pattern, Service Layer Pattern

### Frontend (Angular 19)
- **UI Library**: Angular Material 19
- **State Management**: RxJS observables
- **HTTP Client**: Angular HttpClient with interceptors
- **Charts**: Chart.js + ng2-charts
- **3D Visualization**: Three.js
- **Canvas**: Konva.js
- **PDF Generation**: jsPDF + jsPDF-AutoTable

### Database
- **Provider**: SQL Server (LocalDB for dev)
- **Migrations**: EF Core Code-First migrations
- **Connection String**: `appsettings.json` → `DefaultConnection`

## Development Patterns

### Service Registration Pattern
All services registered in `Presentation/API/Program.cs`:
```csharp
// Pattern: Interface → Implementation
builder.Services.AddScoped<I{Module}Repository, {Module}Repository>();
builder.Services.AddScoped<I{Module}Service, {Module}ApplicationService>();
```

### DTO Strategy
- **Use DTOs when**: Security-sensitive fields, complex aggregations, request/response shaping
- **Use Entities when**: Simple CRUD, no sensitive data, internal operations
- **Current DTOs**: `UserDto`, `ProjectDto`, `StoreDto`, `DrillPatternDto`, `SiteBlastingDto`, etc.

### Validation Strategy
- **FluentValidation**: Request validation (`Create*RequestValidator`, `Update*RequestValidator`)
- **Domain validation**: Business invariants in Domain entities
- **Application validation**: Complex rules in `*ApplicationService` classes

### Error Handling
- **Middleware**: `GlobalExceptionMiddleware` catches all exceptions
- **Logging**: Structured logging via `IStructuredLogger<T>`
- **Performance**: `AuditPerformanceMiddleware` tracks request timing

## Common Tasks

### Adding a New Entity Module
1. **Domain**: Create entity in `Domain/Entities/{Module}/`
2. **Application**: Create DTOs, interfaces, validators, mapping profile
3. **Infrastructure**: Create repository, EF configuration, add migration
4. **Presentation**: Create controller, register services in `Program.cs`
5. **UI**: Create Angular service, model, components

### Running the Application
```bash
# API (from Presentation/API)
dotnet run                    # Runs on https://localhost:5019

# UI (from Presentation/UI)
npm install
npm start                     # Runs on http://localhost:4200

# Database migrations
dotnet ef database update
dotnet ef migrations add {Name}
```

### Testing
```bash
# Backend tests
dotnet test

# Frontend tests
cd Presentation/UI
npm test
```

## Authorization Policies

| Policy | Roles |
|--------|-------|
| `RequireAdminRole` | Admin, Administrator |
| `RequireUserRole` | User, StandardUser |
| `ReadDrillData` | Admin, BlastingEngineer, Operator |
| `ManageDrillData` | BlastingEngineer |
| `ManageProjectSites` | BlastingEngineer |
| `ManageMachines` | Admin, MachineManager |
| `ReadProjectData` | Admin, BlastingEngineer, Operator, MachineManager |
| `ManageExplosiveRequests` | Admin, StoreManager, BlastingEngineer |

## Important Configuration

### CORS
- **Allowed Origins**: `http://localhost:4200`, `http://localhost:4201`
- **Credentials**: Enabled
- **Policy**: `AllowAngularApp`

### JWT Settings
- **Algorithm**: HS256
- **Location**: `appsettings.json` → `JwtSettings`
- **Fields**: `SecretKey`, `Issuer`, `Audience`

### API Endpoints
- **Base URL**: `https://localhost:5019`
- **Swagger**: Available in Development mode
- **API Routes**: `/api/{module}/{action}`

## Code Style & Conventions

### Naming Conventions
- **Interfaces**: `I{Name}Service`, `I{Name}Repository`
- **Services**: `{Name}ApplicationService` (Application), `{Name}DomainService` (Domain)
- **DTOs**: `{Name}Dto`, `Create{Name}Request`, `Update{Name}Request`
- **Entities**: PascalCase, singular (e.g., `Store`, not `Stores`)

### File Organization
- **Group by feature**: `{Module}/{Entity}/` structure
- **Separate by concern**: Interfaces vs Implementations
- **Mirror structure**: Application interfaces → Infrastructure implementations

## Known Patterns & Quirks

1. **JSON Serialization**: Uses camelCase, handles circular references with `ReferenceHandler.IgnoreCycles`
2. **DbContext**: `EnsureCreated()` runs on startup (dev only)
3. **Migrations**: Located in `Infrastructure/Migrations/`
4. **AutoMapper**: Profiles in `Application/Mapping/`
5. **Cache Service**: Uses `IMemoryCache` for performance-critical lookups

## Recent Changes (As of Oct 2025)

- **Store Management Module**: Fully implemented with entities, services, repositories, UI
- **Security Levels**: Removed from Store entity for simplification
- **Angular 19**: Upgraded from Angular 17
- **FluentValidation**: Now used throughout Application layer
- **Performance Monitoring**: Added `IPerformanceMonitor` service

## Quick Reference Documentation

When working on specific tasks, consult these reference documents:

### 📚 Primary Documentation
- **[CLAUDE.md](CLAUDE.md)** (this file) - AI assistant context and codebase overview
- **[README.md](../README.md)** - Project setup and getting started guide
- **[ARCHITECTURE.md](ARCHITECTURE.md)** - Clean Architecture principles and design patterns

### 🔧 Technical References
- **[API.md](API.md)** - Complete REST API documentation
  - **Use when**: Working with controllers, endpoints, request/response formats
  - **Contains**: All API endpoints, auth requirements, payload schemas, status codes

- **[DATABASE.md](DATABASE.md)** - Database schema and migration guide
  - **Use when**: Working with entities, repositories, migrations, or queries
  - **Contains**: Table structures, relationships, indexes, seeded data, SQL queries

- **[TROUBLESHOOTING.md](TROUBLESHOOTING.md)** - Common issues and solutions
  - **Use when**: Debugging errors, fixing build issues, or resolving runtime problems
  - **Contains**: Error patterns, solutions, debugging steps, configuration fixes

- **[STORE_MANAGEMENT_REVIEW.md](STORE_MANAGEMENT_REVIEW.md)** - Store Management code review
  - **Use when**: Working on Store Management module fixes
  - **Contains**: Detailed analysis of issues, redundancies, and recommended fixes

### 📝 Other Resources
- **[LICENSE.txt](../LICENSE.txt)** - MIT License
- **Repository**: https://github.com/DBMS-org/DBMS

---

## AI Assistant Guidelines

### When to Reference Documentation

1. **Before adding/modifying API endpoints** → Read [API.md](API.md)
2. **Before database changes** → Read [DATABASE.md](DATABASE.md)
3. **When encountering errors** → Check [TROUBLESHOOTING.md](TROUBLESHOOTING.md)
4. **For architecture decisions** → Consult [ARCHITECTURE.md](ARCHITECTURE.md)
5. **For general context** → Start with this file (CLAUDE.md)

### Workflow for Common Tasks

**Adding a new feature:**
1. Read CLAUDE.md for project structure
2. Check ARCHITECTURE.md for layer responsibilities
3. Review API.md for existing similar endpoints
4. Check DATABASE.md for related entities

**Fixing a bug:**
1. Check TROUBLESHOOTING.md for known issues
2. Review relevant documentation (API.md or DATABASE.md)
3. Consult ARCHITECTURE.md for design patterns

**Database work:**
1. Always consult DATABASE.md first
2. Check migration history and seeded data
3. Review entity relationships before changes

---

**Last Updated**: October 2, 2025
**For**: Claude Code Assistant
**Maintained By**: Development Team
