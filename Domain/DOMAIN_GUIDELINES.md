# Domain Layer – Design & Guidelines

## Purpose
The **Domain** project is the _heart_ of the application.  It contains the
core business model expressed as _entities_, _value objects_ and (optionally)
_domain events_.  Nothing here should know about databases, HTTP, UI
frameworks or any infrastructure-specific concepts.

> Rule of thumb: **If a class still makes sense in a console app with no
> database, it probably belongs here.**

---

## 1. Dependency Rules
1. **Zero outward dependencies** – the project must _not_ reference
   Application, Infrastructure, or Presentation.  Only `System.*` and
   community **utility** packages (e.g. _SmartEnum_) that do not leak
   infrastructure concerns are allowed.
2. No references to EF-Core attributes (`[Key]`, `[Table]`, `DbContext`).
   Mapping happens in Infrastructure.
3. The only acceptable attributes are _purely in-process_ helpers such as
   `System.ComponentModel.DataAnnotations` for basic validation.  Even these
   can be removed if you adopt external validators elsewhere.

---

## 2. Modelling Principles (OOP Best Practices)
1. **Encapsulation first**  
   • Fields that must obey invariants should expose _read-only_ properties and
     change state through intention-revealing methods.
2. **Invariants live in the Entity**  
   • Example: a `Project` cannot be set to **Closed** if it still has active
     `ProjectSite`s.  The guard sits inside `Project.Close()` rather than in
     an Application service.
3. **Invalid states are unrepresentable**  
   • Prefer `enum`/_smart-enum_ over free-text (`Status = "Active"`).  
   • Wrap primitives in **value objects** (`Email`, `PhoneNumber`, `Coordinate`).
4. **Prefer immutability**  
   • Value objects and domain events should be immutable record types.
5. **Domain events**  
   • Emit events (`OperatorAssigned`, `MachineSentForMaintenance`) from within
     entities to signal significant changes without pulling in dependencies.
6. **Explicit relationships**  
   • Navigation collections should be private-set and modified only via
     methods (`AddSite`, `RemoveSite`).

---

## 3. Architectural Concepts
| Concept | Guidelines |
|---------|------------|
| **Entity** | Has an identity (`Id`) and lifecycle. Contains business behaviour and invariants. |
| **Aggregate Root** | Cluster of entities treated as a unit. Other code holds only the root's Id. |
| **Value Object** | Equality by _value_, immutable, no identity. Use for money, coordinates, etc. |
| **Domain Event** | Record raised by entities to describe something that _happened_ in the past. |

---

## 4. Current State & Improvement Back-log (2025-07-01)
Below are concrete improvements identified during the June-30 audit.

| 📝 Item | Status | Priority |
|---------|--------|----------|
| Convert string `Status`, `Role`, `Type` fields to enums/value objects | Not started | High |
| Move operator-assignment rule from `ProjectApplicationService` into `Project` entity | Not started | High |
| Introduce `Email` & `PhoneNumber` value objects | Not started | Medium |
| Replace public setters with private setters plus behaviour methods in `Project`, `Machine`, `User` | In progress | Medium |
| Emit domain events (`OperatorAssigned`, `MachineMaintenanceScheduled`) | Not started | Medium |
| Mark aggregate boundaries (`Project`, `User`, `Machine`) clearly with comments or interfaces | Not started | Low |

---

## 5. Example – Refactoring `Project`
```csharp
public class Project : IAggregateRoot   // marker interface
{
    private readonly List<ProjectSite> _sites = new();

    public int Id { get; private set; }
    public string Name { get; private set; }
    public Region Region { get; private set; }
    public ProjectStatus Status { get; private set; } = ProjectStatus.Planned;
    public IReadOnlyCollection<ProjectSite> Sites => _sites.AsReadOnly();

    // ctor enforces minimum valid state
    private Project() { }
    public Project(string name, Region region)
    {
        Name = !string.IsNullOrWhiteSpace(name)
            ? name
            : throw new ArgumentException("Name cannot be empty", nameof(name));
        Region = region ?? throw new ArgumentNullException(nameof(region));
    }

    public void AssignOperator(User user)
    {
        if (Status == ProjectStatus.Closed)
            throw new DomainException("Cannot assign operator to a closed project.");
        // … business rules …
    }

    public void Close()
    {
        if (_sites.Any(s => !s.IsOperatorCompleted))
            throw new DomainException("All sites must be completed before closing project.");
        Status = ProjectStatus.Closed;
        DomainEvents.Raise(new ProjectClosed(Id));
    }
}
```

---

## 6. Testing Guidelines
1. **Unit tests** hit entities directly – no mocks required.
2. Arrange small scenarios and assert invariants:
   ```csharp
   var project = new Project("Mine-Alpha", region);
   project.AssignOperator(operator);
   Assert.Equal(operator.Id, project.AssignedUserId);
   ```

---

## 7. Checklist for adding a new entity
- [ ] Does it belong in the Domain? (framework-free?)
- [ ] Does it have at least one invariant enforced in code?
- [ ] Are all string literals converted to enums/value objects where practical?
- [ ] Are navigation collections private and modified through methods?
- [ ] Have you added unit tests for the new behaviour?

> Follow this checklist before committing a new entity.

---

### Authors & Maintenance
Created July 2025 by the DBMS team.  Update this document whenever you
introduce a new pattern or rule in the Domain layer. 