# Application Layer – Design & Guidelines

## Purpose
The **Application** layer is the _use-case orchestration_ layer. It coordinates domain objects, applies business workflows, and handles cross-cutting concerns like security, validation, and logging. It exposes **interfaces** that the Infrastructure & Presentation layers rely on. It must remain free of infrastructure details such as Entity Framework Core or ASP.NET.

```
Presentation ─┬─> Application ─┬─> Domain
             │               └─> (only abstractions pointing outward)
Infrastructure ┘
```

---

## 1. Dependency Rules
1. **May reference:** `Domain` and `Microsoft.Extensions.*` abstractions (logging, caching, options).
2. **Must not reference:** `Infrastructure`, EF-Core, ASP.NET packages, or any concrete database/email/IO classes.
3. **DTOs are for external boundaries only** – do not use them internally within application services or pass them to the Domain layer.

> _Note_: The project file currently honours rule #1 – keep it that way.

---

## 2. Building Blocks
| Concept | Guidelines |
|---------|------------|
| **Interface** | Declares behaviour needed by the Application layer (`IProjectRepository`, `IPasswordService`). These are implemented in the `Infrastructure` project. |
| **Application Service** | Orchestrates a complete _use-case_ (`ProjectApplicationService`). It should be thin (~ ≤ 400 LOC) and focused on coordinating domain entities and core services. |
| **DTO / Request** | Shapes data for controllers/UI; hides sensitive fields; aggregates info from multiple entities. All incoming requests must have a dedicated request DTO. |
| **Core Service** | A cross-cutting concern implemented as a service (`IValidationService`, `IStructuredLogger`, `ICacheService`). These are defined in `Application` and implemented in `Infrastructure`. |
| **Validator** | A `FluentValidation` class for each request DTO that performs input validation. |

---

## 3. Core Services & Patterns

The application layer relies on a set of standardized services to handle cross-cutting concerns. All application services **must** use these services where appropriate.

| Service | Interface | Purpose & Usage |
|---------|-----------|-----------------|
| **Validation** | `IValidationService` | Centralized, synchronous/asynchronous validation of request DTOs. **Must** be the first step in any command-based application service method. Throws `ValidationException`. |
| **Logging** | `IStructuredLogger<T>` | Standardized structured logging with correlation IDs and operation scopes. Replaces `ILogger<T>` for all application services. **Must** be used for all logging. |
| **Caching** | `ICacheService` | In-memory caching for frequently accessed, slow-changing data (e.g., lists of projects, users). Use for read-heavy operations. |
| **Error Handling** | `Result<T>` | All methods that can fail **must** return a `Result<T>` object. This provides a consistent success/failure structure with detailed error messages. Do not use custom response objects. |
| **Exception Handling** | Custom Exceptions | Catch specific exceptions (`DbException`, `ValidationException`) and wrap them in a `Result.Failure`. Avoid catching generic `Exception` unless absolutely necessary. |
| **Performance** | `IPerformanceMonitor` | Used via middleware to automatically track and log the performance of all HTTP requests. Can be injected directly to measure specific long-running operations. |
| **Resource Management**| `ResourceManager` | Provides helpers for timeouts, `CancellationToken` management, and batch processing for long-running or bulk operations (e.g., CSV imports, bulk deletes). |

---

## 4. Modelling Principles
1. **Single Responsibility:** 1 service == 1 cohesive set of use-cases. If it grows beyond ~400 LOC or 10 methods, split it.
2. **Open/Closed via Interfaces:** The application depends _only_ on abstractions (`IUserRepository`, `IEmailService`).
3. **Asynchronous All The Way:** All repository and service methods must be `Task`-based.
4. **Centralized Validation:** Every public, command-executing method in an application service must begin with a call to `_validationService.ValidateAsync(request)`.
5. **Structured Logging Only:** All logging must go through `IStructuredLogger<T>`. Wrap method logic in `using var operation = _logger.BeginOperation(...)` to create a performance and contextual scope. Use specific logging methods like `LogDataAccessError` or `LogValidationFailure`.
6. **Consistent Error Handling:** Never throw exceptions directly from a service method. Instead, catch specific exceptions, log them, and return `Result.Failure()`.
7. **Strategic Caching:** Apply caching via `ICacheService` on read-heavy, high-latency methods that return data that does not change frequently. Invalidate the cache on CUD (Create/Update/Delete) operations.
8. **Unit-Testable:** Services must be testable with mocked dependencies. Avoid static calls.

---

## 5. Implementation Templates
### 5.1 Application Service Method
```csharp
public async Task<Result<UserDto>> CreateUserAsync(CreateUserRequest request)
{
    // 1. Use structured logger to create an operation scope for context and performance tracking.
    using var operation = _logger.BeginOperation("CreateUser", new { Email = request.Email, Role = request.Role });
    try
    {
        // 2. Validate the incoming request immediately.
        var validationResult = await _validationService.ValidateAsync(request);
        if (validationResult.IsFailure)
        {
            _logger.LogValidationFailure("User", validationResult.Errors, new { Email = request.Email });
            return Result.Failure<UserDto>(validationResult.Errors);
        }

        // 3. Coordinate domain entities and repositories.
        var existingUser = await _userRepository.ExistsByNameOrEmailAsync(request.Name, request.Email);
        if (existingUser)
        {
            _logger.LogBusinessWarning("User creation failed - user already exists", new { Email = request.Email });
            return Result.Failure<UserDto>(ErrorCodes.Messages.UserAlreadyExists(request.Email));
        }
        
        // ... business logic ...

        var createdUser = await _userRepository.CreateAsync(user);

        // 4. Invalidate cache if necessary.
        _cacheService.Remove(AllUsersCacheKey);
        
        _logger.LogOperationSuccess("CreateUser", new { UserId = createdUser.Id });
        return Result.Success(MapToDto(createdUser));
    }
    // 5. Catch specific exceptions and return a Failure Result.
    catch (DbException ex)
    {
        _logger.LogDataAccessError("CreateUser", ex, new { Email = request.Email });
        return Result.Failure<UserDto>("Database error occurred while creating user");
    }
    catch (Exception ex)
    {
        _logger.LogUnexpectedError("CreateUser", ex, new { Email = request.Email });
        return Result.Failure<UserDto>(ErrorCodes.Messages.InternalError);
    }
}
```

---

## 6. Improvement Back-log (Status as of recent refactoring)
| 📝 Item | Status | Notes |
|---------|--------|-------|
| Standardize utility usage | ✅ **Completed** | `SafeDataConverter` and other utilities are now used consistently. |
| Unify error handling | ✅ **Completed** | `Result<T>` pattern is now used across all application services. |
| Implement input validation layer | ✅ **Completed** | Centralized `IValidationService` is in place and used by services. |
| Improve exception handling | ✅ **Completed** | Specific exceptions are caught and handled gracefully. |
| Standardize logging patterns | ✅ **Completed** | `IStructuredLogger` provides consistent, structured, and correlated logs. |
| Implement proper resource management | ✅ **Completed** | `ResourceManager` helps manage long-running operations. |
| Optimize performance-critical ops | ✅ **Completed** | Caching (`ICacheService`) has been added for key services. |
| Handle cross-cutting concerns | ✅ **Completed** | Auditing and performance monitoring are now handled by middleware. |
| Split over-sized services | Not started | `SiteBlastingApplicationService` and `DrillHoleApplicationService` still need refactoring. |
| Audit DTO list and delete duplicates | In progress | Some redundant DTOs may still exist. |

---

## 7. Checklist for a New Use-Case
- [ ] Do we already have a service that covers this use-case? If yes, extend carefully; else create a new `*ApplicationService`.
- [ ] Add or reuse repository/interface abstractions. **Never call EF-Core directly**.
- [ ] **Inject and use core services**: `IValidationService`, `IStructuredLogger`, and `ICacheService` where applicable.
- [ ] Create a `FluentValidation` validator for the request DTO.
- [ ] Wrap all method logic in a `try-catch` block and an `IStructuredLogger` operation scope.
- [ ] Return a `Result<T>` for all outcomes.
- [ ] Invalidate the appropriate cache keys on any CUD operation.
- [ ] Write/extend unit tests with mocked dependencies.

---

### Authors & Maintenance
Created July 2025 by the DBMS team. This document was significantly updated after the major application layer refactoring. Update this file whenever you add patterns, rules, or major refactors in the Application layer. 