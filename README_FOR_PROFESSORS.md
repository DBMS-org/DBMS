# Database Management System - DBMS Project

## Quick Start for Evaluation

This project has two branches:
- **`presentation-ready`** - Clean, 1-commit version for easy evaluation (RECOMMENDED)
- **`master`** - Full development history with all commits

### Recommended: View the Clean Version

```bash
git checkout presentation-ready
git log --stat
```

---

## Project Overview

Complete drilling and blasting management system built with modern full-stack architecture.

### Core Features

1. **Role-Based Access Control**
   - Admin - System configuration and user management
   - Blasting Engineer - Blast planning and execution
   - Mechanical Engineer - Equipment maintenance
   - Explosive Manager - Inventory and safety compliance
   - Store Manager - Supply chain management
   - Operator - Machine operation and reporting

2. **Machine Management**
   - Real-time monitoring of drilling machines
   - Maintenance scheduling and tracking
   - Assignment management
   - Performance analytics

3. **Explosive Management**
   - Inventory tracking with safety compliance
   - Usage reporting
   - Regulatory compliance monitoring

4. **Blasting Operations**
   - Blast sequence design
   - Safety protocols
   - Pattern creation
   - Execution tracking

5. **Reporting & Analytics**
   - Comprehensive dashboards
   - Performance metrics
   - Maintenance reports
   - Safety compliance reports

### Technical Stack

**Frontend**
- Angular 18 with TypeScript
- PrimeNG component library
- TailwindCSS for styling
- RxJS for reactive programming
- JWT authentication

**Backend**
- .NET 8 (C#)
- Entity Framework Core
- SQL Server database
- RESTful API architecture
- Clean Architecture principles

**Architecture**
- Domain-Driven Design
- Repository pattern
- Dependency injection
- DTOs for data transfer
- AutoMapper for object mapping

### Project Structure

```
DBMS/
├── Presentation/
│   ├── UI/                    # Angular frontend
│   │   ├── src/
│   │   │   ├── app/
│   │   │   │   ├── components/  # Feature components
│   │   │   │   ├── services/    # Business logic
│   │   │   │   ├── guards/      # Route protection
│   │   │   │   └── models/      # Data models
│   └── API/                   # .NET Core API
├── Application/               # Application logic layer
├── Domain/                    # Domain entities
├── Infrastructure/            # Data access & external services
└── Documentation/             # Project documentation
```

## How to Run

### Prerequisites
- Node.js 18+
- .NET 8 SDK
- SQL Server
- Git

### Backend Setup

```bash
# Navigate to API directory
cd Presentation/API

# Restore packages
dotnet restore

# Update database
dotnet ef database update

# Run API
dotnet run
```

API will be available at: `https://localhost:7000`

### Frontend Setup

```bash
# Navigate to UI directory
cd Presentation/UI

# Install dependencies
npm install

# Start development server
npm start
```

Application will be available at: `http://localhost:4200`

### Default Login Credentials

```
Admin:
Email: admin@dbms.com
Password: Admin@123

Engineer:
Email: engineer@dbms.com
Password: Engineer@123
```

## About the Commit History

### The Clean Branch (presentation-ready)

We created a clean branch for easier evaluation:
- **1 consolidated commit** containing all project work
- All features, code, and documentation preserved
- Professional commit message
- Easy to review and understand

### Why We Did This

During development, our repository accumulated duplicate commits:
- Original: 594 commits
- Actual unique work: 341 commits
- Duplicates: 258 commits (43.1%)

Duplicates occurred due to:
- Merge conflict resolutions
- Multiple team members working simultaneously
- Branch management during development

The `presentation-ready` branch consolidates all our work into a single, clean commit for evaluation purposes.

### Viewing Full History

If you want to see detailed development history:

```bash
git checkout master
git log --oneline --graph
```

## Key Features to Evaluate

### 1. Authentication & Authorization
- JWT-based authentication
- Role-based access control
- Protected routes and components

### 2. Responsive UI/UX
- Modern, intuitive interface
- Mobile-responsive design
- Real-time updates
- Interactive dashboards

### 3. Database Design
- Normalized schema
- Proper relationships
- Efficient queries
- Migration management

### 4. API Design
- RESTful endpoints
- Proper HTTP methods
- Error handling
- DTOs for data transfer

### 5. Code Quality
- Clean Architecture principles
- SOLID principles
- Dependency injection
- Separation of concerns

## Testing the Application

### Test User Flows

1. **Admin Flow**
   - Login as admin
   - Create new users
   - Assign roles
   - View system analytics

2. **Blasting Engineer Flow**
   - View blast designs
   - Create blast patterns
   - Submit execution reports

3. **Mechanical Engineer Flow**
   - View assigned machines
   - Create maintenance reports
   - Track machine status

4. **Operator Flow**
   - View assigned machines
   - Log machine operations
   - Report issues

5. **Manager Flows**
   - View inventory (Store Manager)
   - Track explosives (Explosive Manager)
   - View dashboards

## Database Schema

Key entities:
- Users & Roles
- Machines & MachineTypes
- MaintenanceJobs & MaintenanceReports
- Projects & Sites
- BlastDesigns & BlastPatterns
- Explosives & Inventory
- AccessoryTypes & Accessories

## API Endpoints

Main endpoint categories:
- `/api/auth` - Authentication
- `/api/users` - User management
- `/api/machines` - Machine operations
- `/api/maintenance` - Maintenance tracking
- `/api/projects` - Project management
- `/api/blast` - Blasting operations
- `/api/inventory` - Inventory management

## Evaluation Checklist

- [ ] Application runs successfully
- [ ] Login/Authentication works
- [ ] Role-based access is enforced
- [ ] CRUD operations function correctly
- [ ] Database relationships are proper
- [ ] UI is responsive and intuitive
- [ ] API follows REST conventions
- [ ] Code is well-organized
- [ ] Documentation is clear
- [ ] Features match requirements

## Team Members

**Zuhran Yousaf**
- Student ID: 35305
- Email: 35305@students.riphah.edu.pk
- Role: Full-stack developer

**Saad**
- Email: saad.mr27698@gmail.com
- Role: Full-stack developer

## Project Timeline

- Development: ~3 months
- Original commits: 594
- Unique work commits: 341
- Team size: 2 developers
- Technologies: 10+ (Angular, .NET, SQL Server, etc.)

## Questions?

For questions about the project:
1. Check the code comments
2. Review the commit message in `presentation-ready`
3. Explore the master branch for detailed history

---

**Branch for Evaluation**: `presentation-ready`
**Last Updated**: November 11, 2025
**Status**: Complete and ready for evaluation
