# Application Layer – Design & Guidelines

## Purpose
The **Application** project is the _use-case orchestration_ layer. It
coordinates domain objects, applies business workflows, handles security &
validation, and exposes **interfaces** that Infrastructure & Presentation rely
on.  It must remain free of infrastructure details such as EF-Core or
ASP.NET.

```
Presentation ─┬─> Application ─┬─> Domain
             │               └─> (only abstractions pointing outward)
Infrastructure ┘
```

---

## 1. Dependency Rules
1. **May reference:** `Domain` and `Microsoft.Extensions.*` abstractions
   (logging, options).  Nothing else.
2. **Must not reference:** Infrastructure, EF-Core, ASP.NET packages, or any
   concrete database/email/IO classes.
3. **DTOs are for external boundaries only** – do not leak them into Domain.

> _Note_: The project file currently honours rule #1 – keep it that way.

---

## 2. Building Blocks
| Concept | Guidelines |
|---------|------------|
| **Interface** | Declares behaviour needed by Application (`IProjectRepository`, `IPasswordService`).  Implemented in Infrastructure. |
| **Application Service** | Orchestrates a _use-case_ (`ProjectApplicationService`).  Should be thin (~ ≤ 400 LOC) and focused. |
| **DTO / Request** | Shapes data for controllers/UI; hides sensitive fields; aggregates info from multiple entities. |
| **Validator** | (Optional) Class per request/command – use FluentValidation for complex rules. |

---

## 3. Modelling Principles
1. **Single Responsibility**  
   • 1 service == 1 cohesive set of use-cases. If it grows beyond ~400 LOC or
   10 methods, split it.
2. **Open/Closed via interfaces**  
   • Application depends _only_ on abstractions; any external system must
   satisfy an interface.
3. **No direct I/O**  
   • All file, network, DB operations sit behind repository or service
   interfaces.
4. **Error handling & Logging**  
   • Catch _only_ where you can add context, then re-throw or return proper
   result. Always inject `ILogger<T>`.
5. **Validation strategy**  
   • Quick property checks = DataAnnotations (DTO), complex rules = separate
   validator classes.
6. **DTO hygiene**  
   • Create DTO _only_ when you hide data or change shape. Otherwise return
   entities to avoid duplication.
7. **Asynchronous all the way**  
   • All repository methods are `Task`-based; services should mirror that.
8. **Unit-test first**  
   • Services must be testable with mocked repositories – no static calls.

---

## 4. Current Design – Strengths ✅
1. Pure dependencies (`Domain`, `Microsoft.Extensions.Logging.Abstractions`).
2. Clear folder layout: `Interfaces / Services / DTOs`.
3. Extensive logging in most services (`ProjectApplicationService`, `AuthApplicationService`).
4. Repository pattern used consistently.

## 4.1 Areas to Improve 🔍
| Finding | Impact | Suggested fix |
|---------|--------|---------------|
| **Over-sized services** – `SiteBlastingApplicationService` (~680 LOC) & `DrillHoleApplicationService` (~420 LOC). | Maintainability, testability | Split by use-case (CSV-import vs CRUD; Workflow vs Analytics). |
| **Naming consistency** – we renamed `DrillHoleService` → `DrillHoleApplicationService`, ensure all future services follow `*ApplicationService`. | Clarity | Review remaining classes. |
| **Missing logger injection** – (fixed for DrillHole) ensure new smaller services also inject ILogger. | Observability | Template snippet below. |
| **Redundant DTOs** – some DTOs mirror entities 1:1 (e.g. RegionDto). | Code bloat | Return entity directly if no sensitive data. |
| **Validation scattered** – CSV parsing embeds heavy validation logic in service. | Complexity | Extract validators / helper classes. |
| **Lack of CQRS separation** – Queries & Commands mixed in same class. | Flexibility | Optional: adopt CQRS (one handler per use-case). |

---

## 5. Implementation Templates
### 5.1 Service skeleton
```csharp
public class ProjectApplicationService : IProjectService
{
    private readonly IProjectRepository _repo;
    private readonly ILogger<ProjectApplicationService> _log;

    public ProjectApplicationService(IProjectRepository repo, ILogger<ProjectApplicationService> log)
    {
        _repo = repo;
        _log = log;
    }

    public async Task<Result<ProjectDto>> CreateAsync(CreateProjectRequest cmd, CancellationToken ct)
    {
        // validation (FluentValidation or early guards)
        var project = new Project(cmd.Name, cmd.Region);
        // orchestrate domain behaviour…
        await _repo.AddAsync(project, ct);
        _log.LogInformation("Project {Id} created", project.Id);
        return Result.Success(Map(project));
    }
}
```

### 5.2 Validator example
```csharp
public class CreateProjectValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Region).NotEmpty();
    }
}
```

---

## 6. Improvement Back-log (2025-07-01)
| 📝 Item | Status | Priority |
|---------|--------|----------|
| Split `SiteBlastingApplicationService` into `SiteBlastingWorkflowService`, `CsvImportService`, etc. | Not started | High |
| Introduce FluentValidation for all *Request DTOs* | Not started | High |
| Audit DTO list and delete duplicates | In progress | Medium |
| Adopt SmartEnum for roles/status values (share with Domain) | Not started | Medium |
| Add unit tests for each new service created from splits | Not started | Medium |
| Consider CQRS/mediator (e.g., MediatR) if service count keeps growing | Backlog | Low |

---

## 7. Checklist for a New Use-Case
- [ ] Do we already have a service that covers this use-case? If yes, extend
      carefully; else create a new `*ApplicationService`.
- [ ] Add or reuse repository/interface abstractions; **never call EF-Core**.
- [ ] Inject `ILogger<T>` and log at 
  *Information* (happy path) and *Error* (exceptions).
- [ ] Validate input via DataAnnotations or FluentValidation.
- [ ] Return DTO only if needed; otherwise return Domain entity.
- [ ] Write/extend unit tests with mocked dependencies.

---

### Authors & Maintenance
Created July 2025 by the DBMS team.  Update this file whenever you add
patterns, rules, or major refactors in the Application layer. 