# Structured Logging Guide

This guide explains how to implement and use the structured logging system in the DBMS application.

## Overview

The structured logging system provides:
- **Correlation IDs** for request tracing
- **Operation timing** and performance metrics
- **Standardized log levels** and messages
- **Structured data** for better observability
- **Centralized logging patterns** across services

## Setup

### 1. Dependency Injection Registration

Add the structured logger to your DI container:

```csharp
// In Program.cs or ServiceCollectionExtensions.cs
services.AddScoped(typeof(IStructuredLogger<>), typeof(StructuredLogger<>));
```

### 2. Service Constructor Injection

Update your service constructors to use `IStructuredLogger<T>`:

```csharp
public class UserApplicationService : IUserService
{
    private readonly IStructuredLogger<UserApplicationService> _logger;
    
    public UserApplicationService(IStructuredLogger<UserApplicationService> logger)
    {
        _logger = logger;
    }
}
```

## Usage Patterns

### 1. Operation Tracking

Use `BeginOperation` for automatic timing and context tracking:

```csharp
public async Task<Result<UserDto>> CreateUserAsync(CreateUserRequest request)
{
    using var operation = _logger.BeginOperation("CreateUser", new { Email = request.Email });
    
    try
    {
        // Your operation logic here
        _logger.LogOperationSuccess("CreateUser", new { UserId = user.Id });
        return Result.Success(userDto);
    }
    catch (Exception ex)
    {
        _logger.LogUnexpectedError("CreateUser", ex, new { Email = request.Email });
        return Result.Failure<UserDto>(ErrorCodes.Messages.InternalError);
    }
}
```

### 2. Validation Logging

Log validation failures with detailed context:

```csharp
var validationResult = await _validationService.ValidateAsync(request);
if (validationResult.IsFailure)
{
    _logger.LogValidationFailure("User", validationResult.Errors, 
        new { Email = request.Email });
    return Result.Failure<UserDto>(validationResult.Errors);
}
```

### 3. Business Logic Warnings

Log business rule violations and edge cases:

```csharp
if (existingUser)
{
    _logger.LogBusinessWarning("User already exists", 
        new { Email = request.Email });
    return Result.Failure<UserDto>("User already exists");
}
```

### 4. Exception Handling

Use specific exception logging methods:

```csharp
catch (ValidationException ex)
{
    _logger.LogValidationFailure("User", ex.ValidationErrors, 
        new { Email = request.Email });
    return Result.Failure<UserDto>(ex.ValidationErrors);
}
catch (DbException ex)
{
    _logger.LogDataAccessError("CreateUser", ex, 
        new { Email = request.Email });
    return Result.Failure<UserDto>("Database error occurred");
}
catch (Exception ex)
{
    _logger.LogUnexpectedError("CreateUser", ex, 
        new { Email = request.Email });
    return Result.Failure<UserDto>(ErrorCodes.Messages.InternalError);
}
```

## Log Levels and When to Use

### Debug
- Operation start/end (automatic with BeginOperation)
- Detailed flow information
- Fast operations (<1s)

### Information
- Operation success messages
- Important state changes
- Operations taking 1-5 seconds

### Warning
- Business rule violations
- Validation failures
- Operations taking >5 seconds
- External service issues

### Error
- Database errors
- External service failures
- Unexpected exceptions

## Structured Data Best Practices

### 1. Include Relevant Context

Always include identifying information:

```csharp
_logger.LogOperationSuccess("CreateUser", new { 
    UserId = user.Id, 
    Email = user.Email, 
    Role = user.Role 
});
```

### 2. Use Consistent Property Names

- `UserId` for user identifiers
- `Email` for email addresses
- `OperationName` for operation names
- `DurationMs` for timing

### 3. Avoid Sensitive Information

Never log passwords, tokens, or personal data:

```csharp
// ❌ Bad
_logger.LogDebug("User login", new { Password = request.Password });

// ✅ Good
_logger.LogDebug("User login", new { Username = request.Username });
```

## Correlation IDs

### Setting Correlation IDs

```csharp
// From HTTP headers or generate new
var correlationId = HttpContext.Items["CorrelationId"]?.ToString() 
                   ?? Guid.NewGuid().ToString("N")[..8];
_logger.SetCorrelationId(correlationId);
```

### Automatic Correlation

All log messages automatically include the correlation ID:

```
[a1b2c3d4] Operation CreateUser completed successfully in 245ms
```

## Performance Monitoring

### Automatic Performance Tracking

Operations are automatically timed:

```csharp
using var operation = _logger.BeginOperation("GetAllUsers");
// Operation timing is logged automatically when disposed
```

### Manual Performance Metrics

```csharp
var stopwatch = Stopwatch.StartNew();
// ... your operation
stopwatch.Stop();

_logger.LogPerformanceMetric("BulkImport", stopwatch.ElapsedMilliseconds, 
    new { RecordCount = records.Count });
```

## Migration from Standard ILogger

### Before
```csharp
_logger.LogError(ex, "Error creating user {Email}", request.Email);
```

### After
```csharp
_logger.LogDataAccessError("CreateUser", ex, new { Email = request.Email });
```

## Example: Complete Service Method

```csharp
public async Task<Result<UserDto>> UpdateUserAsync(int id, UpdateUserRequest request)
{
    using var operation = _logger.BeginOperation("UpdateUser", 
        new { UserId = id, Email = request.Email });
    
    try
    {
        // Validate input
        var validationResult = await _validationService.ValidateAsync(request);
        if (validationResult.IsFailure)
        {
            _logger.LogValidationFailure("User", validationResult.Errors, 
                new { UserId = id });
            return Result.Failure<UserDto>(validationResult.Errors);
        }

        // Check if user exists
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            _logger.LogBusinessWarning("User not found for update", 
                new { UserId = id });
            return Result.Failure<UserDto>("User not found");
        }

        // Update user
        user.Email = request.Email;
        user.UpdatedAt = DateTime.UtcNow;
        
        await _userRepository.UpdateAsync(user);
        
        _logger.LogOperationSuccess("UpdateUser", 
            new { UserId = id, Email = request.Email });
        
        return Result.Success(MapToDto(user));
    }
    catch (ValidationException ex)
    {
        _logger.LogValidationFailure("User", ex.ValidationErrors, 
            new { UserId = id });
        return Result.Failure<UserDto>(ex.ValidationErrors);
    }
    catch (DbException ex)
    {
        _logger.LogDataAccessError("UpdateUser", ex, 
            new { UserId = id });
        return Result.Failure<UserDto>("Database error occurred");
    }
    catch (Exception ex)
    {
        _logger.LogUnexpectedError("UpdateUser", ex, 
            new { UserId = id });
        return Result.Failure<UserDto>(ErrorCodes.Messages.InternalError);
    }
}
```

## Benefits

1. **Improved Observability**: Correlation IDs and structured data make debugging easier
2. **Performance Monitoring**: Automatic timing and performance metrics
3. **Consistent Patterns**: Standardized logging across all services
4. **Better Alerting**: Structured data enables better monitoring and alerting
5. **Reduced Noise**: Appropriate log levels reduce log volume

## Next Steps

1. Update all application services to use `IStructuredLogger<T>`
2. Configure correlation ID middleware for HTTP requests
3. Set up log aggregation and monitoring dashboards
4. Create alerts based on performance thresholds and error patterns 