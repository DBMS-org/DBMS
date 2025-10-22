# DBMS – Clean Architecture Overview

> **Document Type:** Architecture Guide & Design Patterns
> **Version:** 2.0 | **Last Updated:** 2025-10-20
> **Audience:** Developers, Architects, Technical Leads
> **Status:** 🟢 Current & Maintained

---

## 📋 Quick Reference

| Topic | Go To | Use When |
|-------|-------|----------|
| Layer Responsibilities | [Section 3](#3-layer-responsibilities) | Understanding where code belongs |
| Dependency Rules | [Section 4](#4-dependency-rules--project-references) | Adding new dependencies |
| Service Design | [Section 8](#8-service-design-guidelines) | Creating new services |
| DTO Guidelines | [Section 7](#7-dto-strategy--guidelines) | Deciding when to use DTOs |
| Testing Strategy | [Section 9](#9-testing-strategy) | Writing tests |

---

## 📚 Table of Contents

1. [High-Level Overview](#1-high-level-overview)
2. [Project Structure](#2-project-structure)
3. [Layer Responsibilities](#3-layer-responsibilities)
   1. [Domain Layer](#31-domain)
   2. [Application Layer](#32-application)
   3. [Infrastructure Layer](#33-infrastructure)
   4. [Presentation Layer](#34-presentation)
4. [Dependency Rules & Project References](#4-dependency-rules--project-references)
5. [Cross-Cutting Concerns](#5-cross-cutting-concerns)
   1. [Dependency Injection](#51-dependency-injection)
   2. [Configuration & Secrets](#52-configuration--secrets)
   3. [Validation Strategy](#53-validation-strategy)
   4. [Logging](#54-logging)
6. [Data-Access Strategy (Repository Pattern)](#6-data-access-strategy-repository-pattern)
7. [DTO Strategy & Guidelines](#7-dto-strategy--guidelines)
8. [Service Design Guidelines](#8-service-design-guidelines)
9. [Testing Strategy](#9-testing-strategy)
10. [Future Improvement Back-log](#10-future-improvement-back-log)

---

## 1. High-Level Overview
This solution follows **Clean Architecture** (inspired by Robert C. Martin).  Business rules are completely isolated from
framework concerns such as EF Core, ASP.NET Core or Angular.

```
          +----------------------+
          |     Presentation     |
          |  (API / Angular UI)  |
          +-----------▲----------+
                      |
          +-----------|----------+
          |      Application     |
          |  (Services, DTOs)    |
          +-----------▲----------+
                      |
          +-----------|----------+
          |      Domain (Core)   |
          |   (Entities ONLY)    |
          +----------------------+
                      ▲
                      |
          +-----------|----------+
          |     Infrastructure   |
          |(EF Core, Repos, etc) |
          +----------------------+
```

*  **Dependencies flow inwards** – outer layers may depend on inner layers, never vice-versa.
*  All I/O concerns live in *Infrastructure*; business rules and orchestration live in *Application*.
*  Presentation knows only *Application* abstractions – never EF Core or Domain internals.

---

## 2. Project Structure
```
DBMS/
├─ Domain/                   # Pure business entities
│  └─ Entities/
├─ Application/              # DTOs, Interfaces, Business Services
│  ├─ Interfaces/
│  ├─ Services/
│  └─ DTOs/
├─ Infrastructure/           # EF Core, Repositories, External Services
│  ├─ Data/ (ApplicationDbContext)
│  └─ Repositories/
├─ Presentation/
│  ├─ API/                   # ASP.NET Core controllers
│  └─ UI/                    # Angular 17 client (in /src)
└─ ARCHITECTURE.md           # ← this document
```

---

## 3. Layer Responsibilities
### 3.1 Domain
* **What:** Plain C# classes (POCOs) – e.g. `User`, `Project`, `DrillPattern`.
* **Rules:**
  * No references to ASP.NET, EF Core, Automapper, etc.
  * Only basic `System.*` packages are allowed.
  * Navigation properties allowed but keep business invariants here when possible.

### 3.2 Application
* **What:**
  * **Interfaces** – e.g. `ISiteBlastingService`, `IDrillHoleRepository`.
  * **Business Services** – e.g. `SiteBlastingApplicationService`, `DrillHoleService`.
  * **DTOs / Requests** – used where entities expose sensitive data or require transformation.
* **Rules:**
  * Depends **only** on `Domain` and `Microsoft.Extensions.*` abstractions.
  * No EF Core; only repositories & interfaces.

### 3.3 Infrastructure
* **What:**
  * `ApplicationDbContext` (EF Core SQL Server).
  * Repository implementations e.g. `DrillHoleRepository`, `SiteBlastingRepository`.
  * External concerns – e.g. Email, JWT, Password hashing.
* **Rules:**
  * May reference *Application* to implement its interfaces.
  * Do **not** contain business rules; pure data access / external integration.

### 3.4 Presentation
* **API** – ASP.NET Core minimal MVC controllers; uses DI to reach `Application` services.
* **UI** – Angular 17 SPA (under `Presentation/UI`).

---

## 4. Dependency Rules & Project References
| Project            | References               |
|--------------------|--------------------------|
| `Domain`           | – none –                |
| `Application`      | `Domain`                |
| `Infrastructure`   | `Domain`, `Application` |
| `Presentation/API` | `Application`, `Infrastructure` (via DI) |
| `Presentation/UI`  | REST API only           |

> **TIP:** if you need a new package in *Domain*—stop. It probably belongs elsewhere.

---

## 5. Cross-Cutting Concerns
### 5.1 Dependency Injection
* Registered in `Presentation/API/Program.cs`.
* Follow the rule: **Register interfaces, not concrete classes.**
* Example:
  ```csharp
  builder.Services.AddScoped<ISiteBlastingRepository, SiteBlastingRepository>();
  builder.Services.AddScoped<ISiteBlastingService, SiteBlastingApplicationService>();
  ```

### 5.2 Configuration & Secrets
* Connection strings in `appsettings*.json`.
* JWT secrets in `appsettings.Development.json` – **replace in Production!**

### 5.3 Validation Strategy
* Basic property validation via DataAnnotations in Domain.
* Complex/business validation lives in *Application* services.

### 5.4 Logging
* `Microsoft.Extensions.Logging` used in all layers.
* Infrastructure repositories log query failures; Application services log business events/errors.

---

## 6. Data-Access Strategy (Repository Pattern)
* **Interface in Application** (e.g. `ISiteBlastingRepository`).
* **Implementation in Infrastructure** (e.g. `SiteBlastingRepository`).
* **Unit of Work** – EF Core `SaveChangesAsync` inside each repository method; atomic by method.
* **Testing:** Interfaces allow mocking repositories for unit tests.

---

## 7. DTO Strategy & Guidelines
### Current Counts
* **Entities Used Directly:** Region, Permission, Role (simple CRUD)
* **DTOs (23 total):**
  * **Security-Sensitive** – `UserDto`, `LoginRequest`, `LoginResponse`, `ForgotPasswordRequest`, `ResetPasswordRequest`.
  * **Complex Mapping** – `ProjectDto`, `MachineDto`, `ProjectSiteDto`, `DrillPatternDto`, `WorkflowProgressDto`, `SiteBlastingDto`, `UserAssignmentDto`.
  * **Command/Request Objects** – `Create*Request`, `Update*Request`, `CsvUploadRequest`.
  * **Utility** – `ApiResponse<T>`.

### When to create a new DTO
1. **Expose only needed fields** to clients.
2. **Hide sensitive data** (e.g. password hash).
3. **Aggregate or transform** data from multiple entities.
4. **Request/Command** objects for POST/PUT endpoints.

> If none of the above apply, **return domain entities directly** to avoid duplication.

---

## 8. Service Design Guidelines
* **Application Services** contain orchestration & business rules; *never* talk to EF Core.
* **Naming:** `*ApplicationService` for Application layer, `*Repository` for data access.
* **Single Responsibility:** 
  * Large services (e.g. `SiteBlastingApplicationService`) should be split when > ~400 lines or > ~10 methods.
* **Return Types:**
  * Prefer DTOs for API boundaries; entities for internal calls.

---

## 9. Testing Strategy
* **Unit Tests (xUnit / NUnit)**
  * Mock repositories using Moq.
  * Test Application services in isolation.
* **Integration Tests**
  * In-memory SQL Server or SQLite.
  * Verify repository implementations.
* **E2E** – Playwright / Cypress for Angular UI.

---

## 10. Future Improvement Back-log
| 📝 Item | Priority |
|---------|----------|
| Split `ISiteBlastingService` into smaller interfaces (`IDrillPatternService`, `IWorkflowService`). | High |
| Refactor `DrillHoleService` to return DTOs instead of entities (security). | Med |
| Introduce FluentValidation for request objects. | Med |
| Add global exception-handling middleware with Problem-Details output. | Med |
| Migrate to `Minimal APIs` for simple endpoints. | Low |
| Add caching (Redis) for read-heavy lookups (Regions, Roles). | Low |
| CI pipeline – dotnet test, eslint, ng test. | Med |

---

---

## 📖 Document Information

| Attribute | Value |
|-----------|-------|
| **Document Version** | 2.0 |
| **Last Updated** | 2025-10-20 |
| **Original Design** | 2025-06-29 |
| **Review Status** | ✅ Current |
| **Next Review** | 2025-11-20 |

---

## 📚 References & Inspiration

### Pattern Sources
- **Clean Architecture** – Robert C. Martin ("Uncle Bob")
- **Onion Architecture** – Jeffrey Palermo
- **Hexagonal Architecture** – Alistair Cockburn
- **Microsoft eShopOnWeb** – Reference implementation

### Further Reading
- [Clean Architecture Book](https://www.amazon.com/Clean-Architecture-Craftsmans-Software-Structure/dp/0134494164)
- [Microsoft Architecture Patterns](https://learn.microsoft.com/en-us/azure/architecture/patterns/)
- [Domain-Driven Design](https://www.amazon.com/Domain-Driven-Design-Tackling-Complexity-Software/dp/0321125215)

---

## 🔗 Related Documentation

- [PROJECT_ANALYSIS.md](PROJECT_ANALYSIS.md) - Comprehensive code analysis and quality metrics
- [API.md](API.md) - REST API reference and endpoint documentation
- [DATABASE.md](DATABASE.md) - Database schema and migration guide
- [TROUBLESHOOTING.md](TROUBLESHOOTING.md) - Common issues and solutions

---

> **Note:** Keep this document up-to-date whenever significant architectural decisions are made. All changes should be reviewed and approved by the technical lead. 