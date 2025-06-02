# DBMS Project - Simplified Clean Architecture

A simplified DBMS project with Angular frontend and .NET backend following Clean Architecture principles.

## 📁 Project Structure

```
DBMS/
├── Core/                          # Domain + Application Layer (Combined)
│   ├── Entities/                  # Database entities and domain models
│   ├── Interfaces/                # Contracts and repository interfaces
│   ├── Services/                  # Business logic services
│   ├── DTOs/                      # Data Transfer Objects
│   └── Validators/                # Input validation logic
├── Infrastructure/                # Data Access + External Services
│   ├── Data/                      # DbContext, configurations, migrations
│   ├── Repositories/              # Repository implementations
│   └── Services/                  # External service implementations
├── API/                          # Web API Layer
│   ├── Controllers/               # API controllers
│   ├── Middleware/                # Custom middleware
│   └── Properties/                # Project properties
├── CleanArchitecture.UI/          # Angular Frontend
└── docs/                         # Documentation
```

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js (for Angular)
- SQL Server or SQLite
- Visual Studio 2022 or VS Code

### Backend Setup (To be implemented)
1. Create .NET projects for each layer:
   ```bash
   # From project root
   dotnet new classlib -n Core
   dotnet new classlib -n Infrastructure  
   dotnet new webapi -n API
   dotnet new sln -n DBMS
   dotnet sln add Core Infrastructure API
   ```

2. Add project references:
   ```bash
   dotnet add API reference Core Infrastructure
   dotnet add Infrastructure reference Core
   ```

### Frontend Setup
```bash
cd CleanArchitecture.UI
npm install
ng serve
```

## 🏗️ Architecture Benefits

**Simplified from original 5-layer to 3-layer architecture:**

1. **Core** (Domain + Application combined)
   - Contains all business logic and entities
   - No external dependencies
   - Easy to test

2. **Infrastructure** (Data + External Services)
   - Database implementations
   - External API calls
   - File system operations

3. **API** (Presentation Layer)
   - Controllers and endpoints
   - Authentication/Authorization
   - Request/Response handling

## 📋 Next Steps for Backend Implementation

1. **Core Layer:**
   - Define your database entities in `Entities/`
   - Create repository interfaces in `Interfaces/`
   - Implement business services in `Services/`

2. **Infrastructure Layer:**
   - Set up DbContext in `Data/`
   - Implement repositories in `Repositories/`
   - Configure database connections

3. **API Layer:**
   - Create controllers in `Controllers/`
   - Set up dependency injection
   - Configure Swagger/OpenAPI

## 🔧 Recommended Tech Stack

- **Backend:** .NET 8, Entity Framework Core, SQL Server/SQLite
- **Frontend:** Angular 18, TypeScript, Bootstrap/Material
- **Testing:** xUnit, Moq, Angular Testing Utilities
- **Documentation:** Swagger/OpenAPI

## 📚 Learning Resources

- [Clean Architecture by Robert Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [.NET Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)

## Clean Architecture Principles

This simplified project follows Clean Architecture principles:

1. **Dependency Rule**: Dependencies point inward toward the core
2. **Separation of Concerns**: Each layer has a specific responsibility
3. **Independence of Frameworks**: Core business logic is independent of frameworks
4. **Testability**: Each layer can be tested independently

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE.txt) file for details.

---
**Note:** This is a simplified structure designed for learning DBMS concepts without getting overwhelmed by complex architecture patterns. 