# Backend Setup Guide

This guide will help you implement the backend for your DBMS project using the simplified Clean Architecture.

## Step 1: Create .NET Projects

Run these commands from the project root directory:

```bash
# Create the Core project (Domain + Application)
dotnet new classlib -n Core

# Create the Infrastructure project (Data Access)
dotnet new classlib -n Infrastructure

# Create the API project (Web API)
dotnet new webapi -n API

# Create a solution file
dotnet new sln -n DBMS

# Add projects to solution
dotnet sln add Core/Core.csproj
dotnet sln add Infrastructure/Infrastructure.csproj  
dotnet sln add API/API.csproj
```

## Step 2: Set Up Project References

```bash
# API depends on Core and Infrastructure
dotnet add API/API.csproj reference Core/Core.csproj
dotnet add API/API.csproj reference Infrastructure/Infrastructure.csproj

# Infrastructure depends on Core
dotnet add Infrastructure/Infrastructure.csproj reference Core/Core.csproj
```

## Step 3: Add NuGet Packages

### Core Project (No external dependencies - keep it clean!)
```bash
# No packages needed - this is the domain layer
```

### Infrastructure Project
```bash
cd Infrastructure
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
# OR for SQLite: dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### API Project
```bash
cd ../API  
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Swashbuckle.AspNetCore
```

## Step 4: Basic Project Structure

### Core Project Structure
```
Core/
├── Entities/
│   ├── User.cs
│   ├── Product.cs
│   └── BaseEntity.cs
├── Interfaces/
│   ├── IRepository.cs
│   ├── IUserRepository.cs
│   └── IUnitOfWork.cs
├── Services/
│   ├── IUserService.cs
│   └── UserService.cs
├── DTOs/
│   ├── UserDto.cs
│   └── CreateUserDto.cs
└── Validators/
    └── UserValidator.cs
```

### Infrastructure Project Structure
```
Infrastructure/
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── DbInitializer.cs
│   └── Configurations/
│       ├── UserConfiguration.cs
│       └── ProductConfiguration.cs
├── Repositories/
│   ├── Repository.cs
│   ├── UserRepository.cs
│   └── UnitOfWork.cs
└── Services/
    ├── EmailService.cs
    └── FileService.cs
```

### API Project Structure
```
API/
├── Controllers/
│   ├── UsersController.cs
│   └── ProductsController.cs
├── Middleware/
│   ├── ExceptionMiddleware.cs
│   └── LoggingMiddleware.cs
└── Extensions/
    ├── ServiceCollectionExtensions.cs
    └── ApplicationBuilderExtensions.cs
```

## Step 5: Sample Implementation

### Core/Entities/BaseEntity.cs
```csharp
namespace Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
```

### Core/Entities/User.cs
```csharp
namespace Core.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
```

### Core/Interfaces/IRepository.cs
```csharp
namespace Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
```

## Step 6: Database Setup

### Infrastructure/Data/ApplicationDbContext.cs
```csharp
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Apply configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
```

## Step 7: Dependency Injection Setup

### API/Program.cs
```csharp
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository pattern
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

## Step 8: Create Your First Migration

```bash
# From the project root
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API
dotnet ef database update --project Infrastructure --startup-project API
```

## Next Steps

1. Implement your specific DBMS entities (tables you want to manage)
2. Create repositories for each entity
3. Build controllers for your API endpoints
4. Add validation and error handling
5. Connect your Angular frontend to the API

## Tips for DBMS Learning

- Start with simple entities (User, Product, Order)
- Focus on CRUD operations first
- Add relationships between entities gradually
- Practice writing SQL queries and see how EF Core translates them
- Use SQL Server Management Studio or similar tools to view your database

This simplified structure will help you focus on learning database concepts without getting lost in complex architecture! 