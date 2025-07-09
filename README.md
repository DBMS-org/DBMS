# DBMS – Clean Architecture Monorepo  

A production-ready DBMS (Drilling & Blasting Management System) built with **ASP.NET Core 8** + **Angular 17** following Clean Architecture.  
This repository contains **four isolated layers** (Domain, Application, Infrastructure, Presentation) and an Angular SPA – all wired together with Dependency Injection.

👉 For the full technical deep-dive see **[ARCHITECTURE.md](ARCHITECTURE.md)**.

---

## 🌳 Project Layout
```
DBMS/
├─ Domain/                 # Pure business entities (no deps)
│  └─ Entities/
├─ Application/            # Interfaces, Business services, DTOs
│  ├─ Interfaces/
│  ├─ Services/
│  └─ DTOs/
├─ Infrastructure/         # EF Core, Repositories, External services
│  ├─ Data/                # ApplicationDbContext + Migrations
│  └─ Repositories/
├─ Presentation/
│  ├─ API/                 # ASP.NET Core Web API
│  └─ UI/                  # Angular 17 client (src/…)
├─ ARCHITECTURE.md         # Detailed design document
└─ README.md               # This file
```

### Layer Dependency Diagram
```
Presentation  →  Application  →  Domain
      ▲                │
      └──── Infrastructure
```
*Arrows = compile-time references (dependencies flow inwards only).*  
Infrastructure implements Application interfaces; Presentation consumes Application services through DI.

---

## 🚀 Getting Started
### Prerequisites
* .NET 8 SDK
* Node.js >= 20 (Angular)
* SQL Server or SQL Express (default connection string)

### Clone & Build
```bash
# clone
git clone https://github.com/your-org/dbms.git && cd dbms

# restore & build
dotnet build
```

### Run API
```bash
cd Presentation/API
# assumes `dotnet-ef` tools installed for migrations
# dotnet ef database update   # first time only
dotnet run
```
API will listen on **https://localhost:5019** (see launchSettings.json).

### Run Angular UI
```bash
cd Presentation/UI
npm install
npm start     # alias for ng serve --open
```
UI will be served from **http://localhost:4200** and proxy API calls to `5019`.

---

## 🏗️ Layer Highlights
| Layer | Responsibilities | Project | Key Refs |
|-------|------------------|---------|----------|
| Domain | Entities & invariants | `Domain` | `User`, `Project`, `DrillPattern` |
| Application | Business logic, DTOs, Interfaces | `Application` | `SiteBlastingApplicationService`, `IDrillHoleService` |
| Infrastructure | Data access & external services | `Infrastructure` | `ApplicationDbContext`, `SiteBlastingRepository` |
| Presentation | HTTP / UI | `Presentation/API`, `Presentation/UI` | Controllers, Angular components |

---

## 🔒 Security
* JWT authentication (HS256) via `JwtService`.
* Password hashing with **BCrypt.Net**.
* Sensitive fields (e.g. `PasswordHash`) never leave the backend – see `UserDto`.

---

## 🧪 Testing
* **Unit** – target Application services with mocked repositories (Moq).
* **Integration** – EF Core InMemory provider to test repositories.
* **E2E** – Playwright (UI) planned.

Run all .NET tests:
```bash
dotnet test
```

---

## ✨ Contributing
1. Fork → create branch `feature/<name>`  
2. `dotnet format` & `ng lint` before committing  
3. PR with clear description – CI must pass.

---

## 📄 License
Released under the MIT License – see [LICENSE.txt](LICENSE.txt). 
