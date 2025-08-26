# Operator Maintenance Reporting Module

This module provides functionality for operators to report drilling machine problems and track resolution status.

## Structure

```
maintenance-reports/
├── maintenance-reports.component.ts    # Main container component
├── models/
│   └── maintenance-report.models.ts    # All interfaces, enums, and constants
├── services/
│   └── maintenance-report.service.ts   # HTTP service for API communication
├── shared/
│   ├── status-badge/                   # Status indicator component
│   └── problem-category-icons/         # Problem category and machine part icons
├── index.ts                           # Barrel exports
└── README.md                          # This file
```

## Key Features

- **Problem Reporting**: Submit detailed problem reports with machine part, category, severity, and symptoms
- **Status Tracking**: View and track the status of submitted reports
- **Offline Support**: Save drafts and pending reports locally when offline
- **Responsive Design**: Mobile-optimized interface for field use
- **Real-time Updates**: Automatic status updates and notifications

## Models

### Core Interfaces
- `ProblemReport`: Complete problem report with all details and status tracking
- `OperatorMachine`: Machine assigned to the operator
- `ReportFilters`: Filtering criteria for report lists
- `CreateProblemReportRequest`: DTO for submitting new reports

### Enums
- `MachinePart`: Drill bit, drill rod, shank, engine, hydraulic, electrical, mechanical, other
- `ProblemCategory`: Engine issues, hydraulic problems, electrical faults, etc.
- `SeverityLevel`: Critical, high, medium, low
- `ReportStatus`: Reported, acknowledged, in progress, resolved, closed
- `MachineStatus`: Operational, under maintenance, down for repair, offline

## Services

### MaintenanceReportService
- Submit problem reports
- Fetch operator reports with filtering
- Get operator machine details
- Handle offline scenarios with local storage
- Sync pending reports when connection restored

## Usage

The module integrates with the existing operator layout and follows Angular Material design patterns. It's accessible via the `/operator/maintenance-reports` route.

## API Endpoints

- `POST /api/maintenance-reports/submit` - Submit new problem report
- `GET /api/maintenance-reports/operator/{id}` - Get operator's reports
- `GET /api/maintenance-reports/operator/{id}/machine` - Get operator's machine
- `GET /api/maintenance-reports/operator/{id}/summary` - Get report summary
- `PATCH /api/maintenance-reports/{id}/status` - Update report status