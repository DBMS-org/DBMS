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
4. **AutoMapper Integration:** Use `IMappingService` for all entity-DTO conversions. Never manually map between entities and DTOs.

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
| **Mapping Profile** | AutoMapper profiles for entity-DTO conversions. Keep mappings simple and avoid complex expressions in expression trees. |

---

## 3. Core Services & Patterns

The application layer relies on a set of standardized services to handle cross-cutting concerns. All application services **must** use these services where appropriate.

| Service | Interface | Purpose & Usage |
|---------|-----------|-----------------|
| **Validation** | `IValidationService` | Centralized, synchronous/asynchronous validation of request DTOs. **Must** be the first step in any command-based application service method. Returns `Result<T>`. |
| **Logging** | `IStructuredLogger<T>` | Standardized structured logging with correlation IDs and operation scopes. Replaces `ILogger<T>` for all application services. **Must** be used for all logging. |
| **Caching** | `ICacheService` | In-memory caching for frequently accessed, slow-changing data (e.g., lists of projects, users). Use for read-heavy operations. |
| **Mapping** | `IMappingService` | AutoMapper-based entity-DTO conversions. Use for all entity-to-DTO and DTO-to-entity mappings. |
| **Error Handling** | `Result<T>` | All methods that can fail **must** return a `Result<T>` object. This provides a consistent success/failure structure with detailed error messages. Do not use custom response objects. |
| **Exception Handling** | Custom Exceptions | Catch specific exceptions (`DbException`, `ValidationException`) and wrap them in a `Result.Failure`. Avoid catching generic `Exception` unless absolutely necessary. |
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
8. **AutoMapper Usage:** Always use `IMappingService` for entity-DTO conversions. Never manually map properties.
9. **Unit-Testable:** Services must be testable with mocked dependencies. Avoid static calls.

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
        
        // 4. Use AutoMapper for entity creation
        var user = _mappingService.Map<User>(request);
        user.PasswordHash = _passwordService.HashPassword(request.Password);

        var createdUser = await _userRepository.CreateAsync(user);

        // 5. Invalidate cache if necessary.
        _cacheService.Remove(AllUsersCacheKey);
        
        // 6. Use AutoMapper for DTO conversion
        var userDto = _mappingService.Map<UserDto>(createdUser);
        
        _logger.LogOperationSuccess("CreateUser", new { UserId = createdUser.Id });
        return Result.Success(userDto);
    }
    // 7. Catch specific exceptions and return a Failure Result.
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

### 5.2 AutoMapper Profile Template
```csharp
public class UserManagementMappingProfile : Profile
{
    public UserManagementMappingProfile()
    {
        // Entity to DTO mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)));

        // DTO to Entity mappings (for updates)
        CreateMap<UpdateUserRequest, User>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<UserStatus>(src.Status)))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Don't update creation time
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()); // Will be set by service
    }
}
```

---

## 6. Current State Analysis (Updated: January 2025)

### ✅ **Strengths & Completed Items**
| Item | Status | Notes |
|------|--------|-------|
| AutoMapper Integration | ✅ **Completed** | `IMappingService` implemented and used consistently across services. |
| Result Pattern | ✅ **Completed** | `Result<T>` pattern used across all application services. |
| Structured Logging | ✅ **Completed** | `IStructuredLogger<T>` provides consistent, structured, and correlated logs. |
| Validation Layer | ✅ **Completed** | Centralized `IValidationService` with FluentValidation integration. |
| Caching Strategy | ✅ **Completed** | `ICacheService` implemented for read-heavy operations. |
| Exception Handling | ✅ **Completed** | Specific exceptions caught and wrapped in `Result.Failure()`. |
| Resource Management | ✅ **Completed** | `ResourceManager` for long-running operations. |
| Interface Segregation | ✅ **Completed** | Well-defined interfaces for all domains. |

### ⚠️ **Areas Needing Attention**
| Item | Status | Priority | Notes |
|------|--------|----------|-------|
| Service Size Management | ⚠️ **Needs Review** | High | `DrillPointPatternApplicationService` (649 LOC) exceeds 400 LOC guideline. |
| Exception Cleanup | ⚠️ **In Progress** | Medium | Many unused exception classes in `ApplicationExceptions.cs`. |
| Performance Monitoring | ⚠️ **Partially Implemented** | Low | `IPerformanceMonitor` registered but middleware not fully integrated. |
| Validation Consistency | ⚠️ **Needs Review** | Medium | Some services use `ILogger<T>` instead of `IStructuredLogger<T>`. |
| DTO Organization | ⚠️ **Needs Audit** | Medium | Potential for DTO consolidation and cleanup. |

### 🚨 **Immediate Actions Required**
1. **Split Large Services:** `DrillPointPatternApplicationService` needs refactoring into smaller, focused services.
2. **Clean Up Exceptions:** Remove unused exception classes from `ApplicationExceptions.cs`.
3. **Standardize Logging:** Ensure all services use `IStructuredLogger<T>` consistently.
4. **Audit DTOs:** Review and consolidate redundant DTOs across domains.

---

## 7. Dead Code Identified
| Item | Location | Status | Action |
|------|----------|--------|--------|
| `AutoMapperDemoService` | `Services/Infrastructure/` | ❌ **Dead Code** | Delete - demonstration only |
| `PerformanceMonitor` | `Services/Infrastructure/` | ❌ **Dead Code** | Remove from DI if not needed |
| Unused Exceptions | `Exceptions/ApplicationExceptions.cs` | ⚠️ **Unused** | Remove: `EntityNotFoundException`, `EntityAlreadyExistsException`, `BusinessRuleViolationException`, `ExternalServiceException`, `DataAccessException`, `NotFoundException`, `UnauthorizedException`, `ForbiddenException`, `ConflictException`, `InternalServerException` |

---

## 8. Checklist for a New Use-Case
- [ ] Do we already have a service that covers this use-case? If yes, extend carefully; else create a new `*ApplicationService`.
- [ ] Add or reuse repository/interface abstractions. **Never call EF-Core directly**.
- [ ] **Inject and use core services**: `IValidationService`, `IStructuredLogger`, `ICacheService`, and `IMappingService`.
- [ ] Create a `FluentValidation` validator for the request DTO.
- [ ] Create AutoMapper profile mappings for entity-DTO conversions.
- [ ] Wrap all method logic in a `try-catch` block and an `IStructuredLogger` operation scope.
- [ ] Return a `Result<T>` for all outcomes.
- [ ] Use `IMappingService` for all entity-DTO conversions.
- [ ] Invalidate the appropriate cache keys on any CUD operation.
- [ ] Write/extend unit tests with mocked dependencies.

---

## 9. Code Quality Metrics
- **Total Application Services:** 12
- **Services Following Result Pattern:** 11/12 (91.7%)
- **Services Using Structured Logging:** 8/12 (66.7%)
- **Services Using AutoMapper:** 3/12 (25.0%)
- **Services Exceeding 400 LOC:** 1/12 (8.3%)
- **Services Using Validation Service:** 4/12 (33.3%)

---

### Authors & Maintenance
Created July 2025 by the DBMS team. This document was significantly updated after the major application layer refactoring and AutoMapper integration. Update this file whenever you add patterns, rules, or major refactors in the Application layer.

**Last Updated:** January 2025
**Next Review:** February 2025 