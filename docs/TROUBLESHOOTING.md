# Troubleshooting Guide

Common issues and solutions for DBMS (Drilling & Blasting Management System).

---

## Table of Contents

1. [Database Issues](#database-issues)
2. [Authentication & Authorization](#authentication--authorization)
3. [API Issues](#api-issues)
4. [Frontend Issues](#frontend-issues)
5. [Build & Compilation](#build--compilation)
6. [Performance Issues](#performance-issues)
7. [File Upload Issues](#file-upload-issues)
8. [Environment & Configuration](#environment--configuration)

---

## Database Issues

### Error: "Cannot open database requested by login"

**Symptoms**: Application crashes on startup with SQL Server connection error.

**Causes**:
- Database doesn't exist
- Connection string is incorrect
- SQL Server is not running

**Solutions**:

1. **Check SQL Server is running**:
   ```bash
   # Windows - Check SQL Server status
   Get-Service | Where-Object {$_.Name -like "*SQL*"}
   ```

2. **Verify connection string** in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DBMS;Trusted_Connection=true;"
   }
   ```

3. **Create database and apply migrations**:
   ```bash
   cd Presentation/API
   dotnet ef database update
   ```

4. **If migrations are missing**:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

---

### Error: "Pending model changes detected"

**Symptoms**: EF Core detects schema changes but no migration exists.

**Solution**:
```bash
cd Presentation/API
dotnet ef migrations add DescribeYourChanges
dotnet ef database update
```

---

### Error: Duplicate key constraint violation

**Symptoms**: SQL error when inserting records with duplicate IDs.

**Causes**:
- Seeded data conflicts with existing records
- Manual ID assignment conflicts

**Solutions**:

1. **Clear and recreate database**:
   ```bash
   dotnet ef database drop --force
   dotnet ef database update
   ```

2. **Check seed data** in `ApplicationDbContext.cs:60-120`

3. **Run cleanup script** (if exists):
   ```bash
   sqlcmd -S (localdb)\mssqllocaldb -d DBMS -i cleanup_duplicates.sql
   ```

---

### Error: "The entity type 'X' requires a primary key"

**Symptoms**: Missing primary key configuration.

**Solution**: Add configuration in `Infrastructure/Configurations/`:
```csharp
public class MyEntityConfiguration : IEntityTypeConfiguration<MyEntity>
{
    public void Configure(EntityTypeBuilder<MyEntity> builder)
    {
        builder.HasKey(e => e.Id);
        // ... other configurations
    }
}
```

---

## Authentication & Authorization

### Error: 401 Unauthorized on valid token

**Symptoms**: API returns 401 even with valid JWT token.

**Causes**:
- Token expired
- JWT settings mismatch between client and server
- Clock skew between systems

**Solutions**:

1. **Check token expiration**:
   - Decode token at [jwt.io](https://jwt.io)
   - Verify `exp` claim is in the future

2. **Verify JWT settings match** in `appsettings.json`:
   ```json
   "JwtSettings": {
     "SecretKey": "your-secret-key-must-be-at-least-32-characters",
     "Issuer": "DBMS-API",
     "Audience": "DBMS-UI",
     "ExpirationMinutes": 60
   }
   ```

3. **Check Authorization header format**:
   ```
   Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
   ```
   (Must have "Bearer " prefix)

4. **Debug current user claims**:
   ```http
   GET /api/auth/debug/current-user
   Authorization: Bearer {your-token}
   ```

---

### Error: 403 Forbidden on authorized user

**Symptoms**: User is authenticated but receives 403 on endpoint.

**Causes**:
- User role doesn't match policy requirements
- Authorization policy misconfigured

**Solutions**:

1. **Check user role** via debug endpoint:
   ```http
   GET /api/auth/debug/current-user
   ```

2. **Verify authorization policy** in `Program.cs:121-132`:
   ```csharp
   options.AddPolicy("PolicyName", policy =>
       policy.RequireRole("Admin", "AllowedRole"));
   ```

3. **Check controller policy**:
   ```csharp
   [Authorize(Policy = "RequireAdminRole")] // Must have Admin role
   ```

4. **Verify role exists in database**:
   ```sql
   SELECT * FROM Roles WHERE Name = 'Admin';
   SELECT * FROM UserRoles WHERE UserId = 1;
   ```

---

### Error: "Cannot read properties of null (reading 'token')"

**Symptoms**: Frontend cannot access auth token after login.

**Causes**:
- Token not saved to localStorage/sessionStorage
- Login response format incorrect

**Solutions**:

1. **Verify login response** includes token:
   ```json
   {
     "token": "eyJhbGciOiJIUzI1NiIs...",
     "user": { /* UserDto */ }
   }
   ```

2. **Check auth service** stores token:
   ```typescript
   localStorage.setItem('authToken', response.token);
   ```

3. **Clear and re-login**:
   ```javascript
   localStorage.clear();
   // Re-login through UI
   ```

---

## API Issues

### Error: CORS blocked request

**Symptoms**: Browser console shows "blocked by CORS policy".

**Causes**:
- Frontend origin not in CORS whitelist
- Credentials not allowed

**Solutions**:

1. **Verify frontend URL** in `Program.cs:84-93`:
   ```csharp
   policy.WithOrigins("http://localhost:4200", "http://localhost:4201")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials();
   ```

2. **Check Angular proxy** in `proxy.conf.json`:
   ```json
   {
     "/api": {
       "target": "https://localhost:5019",
       "secure": false
     }
   }
   ```

3. **Ensure CORS middleware order** in `Program.cs:216`:
   ```csharp
   app.UseCors("AllowAngularApp"); // Must be before UseAuthentication
   app.UseAuthentication();
   app.UseAuthorization();
   ```

---

### Error: 500 Internal Server Error

**Symptoms**: Generic 500 error with no details.

**Causes**:
- Unhandled exception in service layer
- Database connection lost
- Null reference exception

**Solutions**:

1. **Check API logs** in console output or logs folder

2. **Enable detailed errors** in `appsettings.Development.json`:
   ```json
   "Logging": {
     "LogLevel": {
       "Default": "Debug",
       "Microsoft.AspNetCore": "Debug"
     }
   }
   ```

3. **Test endpoint** with debugger:
   - Set breakpoint in controller
   - Run API in debug mode (F5 in Visual Studio)

4. **Check GlobalExceptionMiddleware** logs:
   - Review `Middleware/GlobalExceptionMiddleware.cs:219`
   - Exception details logged automatically

---

### Error: Circular reference detected in JSON

**Symptoms**: JSON serialization fails with circular reference error.

**Solution**: Already configured in `Program.cs:50`:
```csharp
options.JsonSerializerOptions.ReferenceHandler =
    System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
```

If still occurring:
1. **Use DTOs** instead of returning entities directly
2. **Add `[JsonIgnore]`** to navigation properties causing cycles

---

## Frontend Issues

### Error: "Cannot find module '@angular/material'"

**Symptoms**: Angular build fails with missing module.

**Solution**:
```bash
cd Presentation/UI
npm install
# If still failing:
rm -rf node_modules package-lock.json
npm install
```

---

### Error: Angular dev server won't start

**Symptoms**: `npm start` fails or hangs.

**Solutions**:

1. **Check port availability**:
   ```bash
   # Windows
   netstat -ano | findstr :4200
   # Kill process if needed
   taskkill /PID <PID> /F
   ```

2. **Clear Angular cache**:
   ```bash
   ng cache clean
   npm start
   ```

3. **Verify Node version**:
   ```bash
   node --version  # Should be >= 20
   ```

---

### Error: HTTP call returns 404 but API is running

**Symptoms**: Frontend makes request to wrong URL.

**Causes**:
- API base URL misconfigured
- Proxy not working

**Solutions**:

1. **Check environment file** (`environment.ts`):
   ```typescript
   export const environment = {
     apiUrl: 'https://localhost:5019/api'
   };
   ```

2. **Verify proxy config** (`proxy.conf.json`):
   ```json
   {
     "/api": {
       "target": "https://localhost:5019",
       "secure": false,
       "logLevel": "debug"
     }
   }
   ```

3. **Start Angular with proxy**:
   ```bash
   ng serve --proxy-config proxy.conf.json
   ```

---

### Error: Chart.js not rendering

**Symptoms**: Charts don't display or throw errors.

**Solutions**:

1. **Verify Chart.js installed**:
   ```bash
   npm list chart.js ng2-charts
   ```

2. **Import Chart.js** in component:
   ```typescript
   import { Chart } from 'chart.js';
   ```

3. **Check canvas element** exists in template:
   ```html
   <canvas #myChart></canvas>
   ```

---

## Build & Compilation

### Error: "Type 'X' does not satisfy constraint 'Y'"

**Symptoms**: TypeScript compilation error in Angular.

**Solution**:
1. **Check TypeScript version** matches Angular version
2. **Update dependencies**:
   ```bash
   npm update
   ```

---

### Error: FluentValidation errors not showing

**Symptoms**: Validation passes when it should fail.

**Causes**:
- Validator not registered
- Validation not triggered

**Solutions**:

1. **Verify validator registration** in `Program.cs:60-61`:
   ```csharp
   builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
   builder.Services.AddFluentValidationAutoValidation();
   ```

2. **Check validator class** exists for DTO:
   ```csharp
   public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
   {
       public CreateUserRequestValidator()
       {
           RuleFor(x => x.Username).NotEmpty();
           // ... rules
       }
   }
   ```

3. **Ensure DTO is used** in controller:
   ```csharp
   public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
   ```

---

## Performance Issues

### Issue: Slow API response times

**Symptoms**: Requests take > 2 seconds.

**Causes**:
- N+1 query problem
- Missing indexes
- Large result sets without pagination

**Solutions**:

1. **Check SQL queries** with EF Core logging:
   ```json
   "Logging": {
     "LogLevel": {
       "Microsoft.EntityFrameworkCore.Database.Command": "Information"
     }
   }
   ```

2. **Add `.Include()` for eager loading**:
   ```csharp
   var projects = await _context.Projects
       .Include(p => p.Sites)
       .Include(p => p.Region)
       .ToListAsync();
   ```

3. **Review performance logs** via `AuditPerformanceMiddleware.cs:225`

4. **Add database indexes** in entity configuration:
   ```csharp
   builder.HasIndex(e => e.Email).IsUnique();
   builder.HasIndex(e => e.CreatedDate);
   ```

---

### Issue: High memory usage

**Symptoms**: Application uses excessive RAM.

**Solutions**:

1. **Use pagination** for large datasets:
   ```csharp
   var results = await query
       .Skip((page - 1) * pageSize)
       .Take(pageSize)
       .ToListAsync();
   ```

2. **Use `AsNoTracking()`** for read-only queries:
   ```csharp
   var users = await _context.Users.AsNoTracking().ToListAsync();
   ```

3. **Clear cache** if using IMemoryCache:
   ```csharp
   _cache.Remove("cacheKey");
   ```

---

## File Upload Issues

### Error: CSV upload fails with "Invalid file format"

**Symptoms**: CSV upload endpoint returns 400 error.

**Causes**:
- CSV format doesn't match expected structure
- Wrong delimiter or encoding

**Solutions**:

1. **Verify CSV format** expected by `CsvImportService`:
   - Check column headers match expected names
   - Ensure UTF-8 encoding
   - Use comma (,) as delimiter

2. **Check file extension** validation:
   ```csharp
   var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
   if (fileExtension != ".csv") { /* error */ }
   ```

3. **Test with sample CSV**:
   ```csv
   HoleId,X,Y,Z,Depth
   H001,100.5,200.3,50.0,15.5
   H002,101.0,201.5,50.2,16.0
   ```

---

### Error: File upload returns 413 Request Entity Too Large

**Symptoms**: Large files fail to upload.

**Solution**: Increase request size limit in `Program.cs`:
```csharp
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 52428800; // 50 MB
});
```

---

## Environment & Configuration

### Error: "Unable to load configuration from 'appsettings.json'"

**Symptoms**: Application can't find configuration file.

**Solutions**:

1. **Verify file exists** at `Presentation/API/appsettings.json`

2. **Check file properties**:
   - "Copy to Output Directory" = "Copy if newer"
   - Build Action = "Content"

3. **Run from correct directory**:
   ```bash
   cd Presentation/API
   dotnet run
   ```

---

### Error: Connection string not found

**Symptoms**: Null reference when accessing configuration.

**Solution**: Ensure connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DBMS;Trusted_Connection=true;"
  }
}
```

Access in code:
```csharp
builder.Configuration.GetConnectionString("DefaultConnection")
```

---

## Common Error Patterns

### Pattern: "Object reference not set to instance"

**Likely Causes**:
1. Missing dependency injection registration
2. Entity not loaded from database
3. Navigation property not included

**Debug Steps**:
1. Check DI registration in `Program.cs`
2. Add null checks: `if (entity == null) return NotFound();`
3. Use `.Include()` for related entities

---

### Pattern: "Cannot insert duplicate key"

**Likely Causes**:
1. Seeded data conflicts
2. Attempting to create existing entity
3. Primary key collision

**Debug Steps**:
1. Check if entity exists before creating
2. Use `CreateOrUpdate` pattern
3. Clear and reseed database

---

### Pattern: Validation errors on update

**Likely Causes**:
1. Missing required fields in DTO
2. Business rule violations
3. Concurrency conflicts

**Debug Steps**:
1. Log validation errors: `_logger.LogWarning($"Validation failed: {result.Error}")`
2. Check business rules in `*ApplicationService`
3. Review entity constraints in `*Configuration.cs`

---

## Getting Help

If issues persist:

1. **Check logs** in API console output
2. **Review Git history** for recent breaking changes
3. **Check documentation**:
   - [ARCHITECTURE.md](ARCHITECTURE.md) - System design
   - [API.md](API.md) - API reference
   - [DATABASE.md](DATABASE.md) - Database schema
4. **Debug with breakpoints** in Visual Studio/VS Code
5. **File GitHub issue** with:
   - Error message
   - Steps to reproduce
   - Environment details (OS, .NET version, Node version)

---

**Last Updated**: October 2, 2025
