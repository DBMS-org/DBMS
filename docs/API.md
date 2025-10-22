# API Documentation

Complete REST API reference for the DBMS (Drilling & Blasting Management System).

**Base URL**: `https://localhost:5019/api`
**Authentication**: JWT Bearer tokens (except where marked `[AllowAnonymous]`)

---

## Table of Contents

1. [Authentication](#authentication)
2. [User Management](#user-management)
3. [Project Management](#project-management)
4. [Project Sites](#project-sites)
5. [Drilling Operations](#drilling-operations)
6. [Drill Point Patterns](#drill-point-patterns)
7. [Blasting Operations](#blasting-operations)
8. [Explosive Calculations](#explosive-calculations)
9. [Explosive Approval Requests](#explosive-approval-requests)
10. [Store Management](#store-management)
11. [Machines](#machines)
12. [Regions](#regions)
13. [User Assignments](#user-assignments)
14. [Common Response Formats](#common-response-formats)

---

## Authentication

### POST `/api/auth/login`
Login with username and password.

**Authorization**: `[AllowAnonymous]`

**Request Body**:
```json
{
  "username": "string",
  "password": "string"
}
```

**Response** (200 OK):
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "john.doe",
    "email": "john@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Admin",
    "region": "North"
  },
  "message": "Login successful"
}
```

**Errors**:
- `401 Unauthorized`: Invalid credentials

---

### POST `/api/auth/register`
Register a new user.

**Authorization**: `[AllowAnonymous]`

**Request Body**:
```json
{
  "username": "string",
  "email": "string",
  "password": "string",
  "firstName": "string",
  "lastName": "string",
  "role": "string",
  "regionId": 1
}
```

**Response** (200 OK):
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": { /* UserDto */ },
  "message": "Registration successful"
}
```

**Errors**:
- `400 BadRequest`: Validation errors or user already exists

---

### POST `/api/auth/forgot-password`
Request password reset code.

**Authorization**: `[AllowAnonymous]`

**Request Body**:
```json
{
  "email": "user@example.com"
}
```

**Response** (200 OK):
```json
{
  "message": "Reset code sent to email"
}
```

---

### POST `/api/auth/verify-reset-code`
Verify password reset code.

**Authorization**: `[AllowAnonymous]`

**Request Body**:
```json
{
  "email": "user@example.com",
  "resetCode": "123456"
}
```

---

### POST `/api/auth/reset-password`
Reset password with verified code.

**Authorization**: `[AllowAnonymous]`

**Request Body**:
```json
{
  "email": "user@example.com",
  "resetCode": "123456",
  "newPassword": "newSecurePassword123"
}
```

---

### POST `/api/auth/validate-token`
Validate current JWT token.

**Authorization**: `[Authorize]`

**Headers**: `Authorization: Bearer {token}`

**Response** (200 OK):
```json
{
  "isValid": true,
  "userId": 1,
  "username": "john.doe"
}
```

---

### POST `/api/auth/logout`
Logout user (invalidate token).

**Authorization**: `[Authorize]`

**Response** (200 OK):
```json
{
  "message": "Logout successful"
}
```

---

### GET `/api/auth/debug/current-user`
Debug endpoint to view current user claims.

**Response** (200 OK):
```json
{
  "userIdClaim": "1",
  "userIdParsed": 1,
  "userName": "john.doe",
  "userEmail": "john@example.com",
  "userRole": "Admin",
  "userRegion": "North",
  "isAuthenticated": true,
  "allClaims": [
    { "type": "nameid", "value": "1" },
    { "type": "name", "value": "john.doe" }
  ]
}
```

---

## User Management

### GET `/api/users`
Get all users.

**Authorization**: `[Authorize]`

**Response** (200 OK):
```json
[
  {
    "id": 1,
    "username": "john.doe",
    "email": "john@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Admin",
    "region": "North"
  }
]
```

---

### GET `/api/users/{id}`
Get user by ID.

**Authorization**: `[Authorize]`

**Response** (200 OK): `UserDto`

**Errors**:
- `404 NotFound`: User not found

---

### POST `/api/users`
Create a new user.

**Authorization**: `[Authorize(Policy = "RequireAdminRole")]` (Admin only)

**Request Body**: `CreateUserRequest`

**Response** (201 Created):
```json
{
  "id": 5,
  "username": "new.user",
  // ... other UserDto fields
}
```

**Errors**:
- `409 Conflict`: User already exists

---

### PUT `/api/users/{id}`
Update user.

**Authorization**: `[Authorize(Policy = "RequireAdminRole")]` (Admin only)

**Request Body**: `UpdateUserRequest`

**Response** (200 OK): No content

**Errors**:
- `400 BadRequest`: ID mismatch
- `404 NotFound`: User not found

---

### DELETE `/api/users/{id}`
Delete user.

**Authorization**: `[Authorize(Policy = "RequireAdminRole")]` (Admin only)

**Response** (200 OK): No content

**Errors**:
- `404 NotFound`: User not found

---

### GET `/api/users/test-connection`
Test database connection.

**Authorization**: `[AllowAnonymous]`

**Response** (200 OK): `{ "status": "connected" }`

---

## Project Management

### GET `/api/projects`
Get all projects.

**Authorization**: `[Authorize(Policy = "ReadProjectData")]`

**Response** (200 OK): Array of `ProjectDto`

---

### GET `/api/projects/{id}`
Get project by ID.

**Authorization**: `[Authorize(Policy = "ReadProjectData")]`

**Response** (200 OK): `ProjectDto`

---

### POST `/api/projects`
Create new project.

**Authorization**: `[Authorize(Policy = "RequireAdminRole")]`

**Request Body**:
```json
{
  "projectName": "New Mining Project",
  "description": "Description text",
  "startDate": "2025-01-01T00:00:00Z",
  "endDate": "2025-12-31T00:00:00Z",
  "status": "Active",
  "regionId": 1,
  "owningUserId": 1
}
```

**Response** (201 Created): `ProjectDto`

---

### PUT `/api/projects/{id}`
Update project.

**Authorization**: `[Authorize]` (Owner or Admin)

**Request Body**: `UpdateProjectRequest`

**Response** (200 OK): No content

---

### DELETE `/api/projects/{id}`
Delete project.

**Authorization**: `[Authorize]` (Owner or Admin)

**Response** (200 OK): No content

---

### GET `/api/projects/{id}/sites`
Get all sites for a project.

**Response** (200 OK): Array of `ProjectSiteDto`

---

### GET `/api/projects/search`
Search projects with filters.

**Query Parameters**:
- `name` (optional): Filter by project name
- `region` (optional): Filter by region
- `status` (optional): Filter by status

**Response** (200 OK): Array of `ProjectDto`

---

### GET `/api/projects/by-operator/{operatorId}`
Get project assigned to operator.

**Authorization**: `[Authorize(Policy = "ReadProjectData")]` (Self or Admin)

**Response** (200 OK): `ProjectDto`

**Errors**:
- `403 Forbid`: Not authorized to access this operator's data
- `404 NotFound`: No project found for operator

---

## Project Sites

### GET `/api/projectsites`
Get all project sites.

**Response** (200 OK): Array of `ProjectSiteDto`

---

### GET `/api/projectsites/{id}`
Get site by ID.

**Response** (200 OK): `ProjectSiteDto`

---

### POST `/api/projectsites`
Create new site.

**Request Body**:
```json
{
  "siteName": "Site A",
  "projectId": 1,
  "latitude": 40.7128,
  "longitude": -74.0060,
  "status": "Active"
}
```

**Response** (201 Created): `ProjectSiteDto`

---

### PUT `/api/projectsites/{id}`
Update site.

**Response** (200 OK): No content

---

### DELETE `/api/projectsites/{id}`
Delete site.

**Response** (200 OK): No content

---

## Drilling Operations

### GET `/api/drillplan`
Get all drill holes.

**Authorization**: `[Authorize(Policy = "ReadDrillData")]`

**Response** (200 OK): Array of `DrillHoleDto`

---

### GET `/api/drillplan/{id}`
Get drill hole by ID.

**Authorization**: `[Authorize(Policy = "ReadDrillData")]`

**Response** (200 OK): `DrillHoleDto`

---

### POST `/api/drillplan/upload-csv`
Upload CSV file to create drill holes.

**Authorization**: `[Authorize(Policy = "ManageDrillData")]`

**Request**: `multipart/form-data` with CSV file

**Response** (200 OK): Array of created `DrillHoleDto`

**Errors**:
- `400 BadRequest`: Invalid file format or no valid drill holes

---

### POST `/api/drillplan`
Create single drill hole.

**Authorization**: `[Authorize(Policy = "ManageDrillData")]`

**Request Body**: `CreateDrillHoleRequest`

**Response** (201 Created): `DrillHoleDto`

---

### POST `/api/drillplan/projects/{projectId}/sites/{siteId}`
Create drill hole for specific site.

**Authorization**: `[Authorize(Policy = "ManageDrillData")]`

**Request Body**: `CreateDrillHoleRequest` (projectId and siteId set from route)

---

### PUT `/api/drillplan/{id}`
Update drill hole.

**Authorization**: `[Authorize(Policy = "ManageDrillData")]`

**Response** (200 OK): No content

---

### DELETE `/api/drillplan/{id}`
Delete drill hole.

**Authorization**: `[Authorize(Policy = "ManageDrillData")]`

**Response** (200 OK): No content

---

### GET `/api/drillplan/projects/{projectId}/sites/{siteId}`
Get drill holes for specific site.

**Authorization**: `[Authorize(Policy = "ReadDrillData")]`

**Response** (200 OK): Array of `DrillHoleDto`

---

## Drill Point Patterns

### GET `/api/drillpointpattern`
Get all drill patterns.

**Response** (200 OK): Array of drill patterns

---

### GET `/api/drillpointpattern/{id}`
Get pattern by ID.

**Response** (200 OK): Drill pattern

---

### POST `/api/drillpointpattern`
Create new drill pattern.

**Request Body**: Pattern configuration

**Response** (201 Created): Created pattern

---

### PUT `/api/drillpointpattern/{id}`
Update pattern.

**Response** (200 OK): No content

---

### DELETE `/api/drillpointpattern/{id}`
Delete pattern.

**Response** (200 OK): No content

---

## Blasting Operations

### GET `/api/siteblasting`
Get all site blasting records.

**Response** (200 OK): Array of `SiteBlastingDto`

---

### GET `/api/siteblasting/{id}`
Get blasting record by ID.

**Response** (200 OK): `SiteBlastingDto`

---

### POST `/api/siteblasting`
Create blasting record.

**Request Body**: `CreateSiteBlastingRequest`

**Response** (201 Created): `SiteBlastingDto`

---

### PUT `/api/siteblasting/{id}`
Update blasting record.

**Response** (200 OK): No content

---

### DELETE `/api/siteblasting/{id}`
Delete blasting record.

**Response** (200 OK): No content

---

## Explosive Calculations

### GET `/api/explosivecalculationresult`
Get all explosive calculations.

**Response** (200 OK): Array of calculation results

---

### GET `/api/explosivecalculationresult/{id}`
Get calculation by ID.

**Response** (200 OK): Calculation result

---

### POST `/api/explosivecalculationresult`
Create new calculation.

**Request Body**: Calculation parameters

**Response** (201 Created): Calculation result

---

## Explosive Approval Requests

### GET `/api/explosiveapprovalrequest`
Get all approval requests.

**Authorization**: `[Authorize(Policy = "ManageExplosiveRequests")]`

**Response** (200 OK): Array of approval requests

---

### GET `/api/explosiveapprovalrequest/{id}`
Get approval request by ID.

**Response** (200 OK): Approval request details

---

### POST `/api/explosiveapprovalrequest`
Create new approval request.

**Request Body**: Approval request data

**Response** (201 Created): Created request

---

### PUT `/api/explosiveapprovalrequest/{id}/approve`
Approve explosive request.

**Authorization**: `[Authorize(Policy = "ManageExplosiveRequests")]`

**Response** (200 OK): No content

---

### PUT `/api/explosiveapprovalrequest/{id}/reject`
Reject explosive request.

**Authorization**: `[Authorize(Policy = "ManageExplosiveRequests")]`

**Response** (200 OK): No content

---

## Store Management

**Base Route**: `/api/storemanagement`

**Authorization**: `[Authorize(Roles = "Admin,ExplosiveManager,StoreManager")]`

### GET `/api/storemanagement`
Get all stores.

**Response** (200 OK): Array of `StoreDto`

---

### GET `/api/storemanagement/{id}`
Get store by ID.

**Response** (200 OK): `StoreDto`

---

### POST `/api/storemanagement`
Create new store.

**Authorization**: `[Authorize(Roles = "Admin,Administrator,Explosive Manager")]`

**Request Body**:
```json
{
  "storeName": "Main Explosive Store",
  "location": "North Sector",
  "capacity": 5000.0,
  "currentStock": 0.0,
  "managerId": 5,
  "regionId": 1,
  "status": "Active"
}
```

**Response** (201 Created): `StoreDto`

---

### PUT `/api/storemanagement/{id}`
Update store.

**Authorization**: `[Authorize(Roles = "Admin,Administrator,Explosive Manager")]`

**Request Body**: `UpdateStoreRequest`

**Response** (200 OK): No content

---

### DELETE `/api/storemanagement/{id}`
Delete store.

**Authorization**: `[Authorize(Policy = "RequireAdminRole")]`

**Response** (200 OK): No content

---

### GET `/api/storemanagement/statistics`
Get store statistics.

**Response** (200 OK):
```json
{
  "totalStores": 5,
  "activeStores": 4,
  "totalCapacity": 25000.0,
  "totalStock": 18500.0,
  "utilizationPercentage": 74.0
}
```

---

### GET `/api/storemanagement/search`
Search stores with filters.

**Query Parameters**:
- `storeName` (optional): Filter by store name
- `city` (optional): Filter by city/location
- `status` (optional): Filter by status

**Response** (200 OK): Array of `StoreDto`

---

### GET `/api/storemanagement/region/{regionId}`
Get stores by region.

**Response** (200 OK): Array of `StoreDto`

---

### GET `/api/storemanagement/manager/{userId}`
Get store managed by user.

**Response** (200 OK): `StoreDto`

**Errors**:
- `404 NotFound`: No store found for manager

---

### PUT `/api/storemanagement/{id}/status`
Update store status.

**Authorization**: `[Authorize(Roles = "Admin,Administrator,Explosive Manager")]`

**Request Body**:
```json
{
  "status": "Active" | "Inactive" | "Maintenance"
}
```

**Response** (200 OK): No content

---

### GET `/api/storemanagement/{id}/utilization`
Get store utilization percentage.

**Response** (200 OK):
```json
{
  "utilizationPercentage": 85.5
}
```

---

## Machines

### GET `/api/machines`
Get all machines.

**Response** (200 OK): Array of machine records

---

### GET `/api/machines/{id}`
Get machine by ID.

**Response** (200 OK): Machine details

---

### POST `/api/machines`
Create new machine.

**Authorization**: `[Authorize(Policy = "ManageMachines")]`

**Response** (201 Created): Created machine

---

### PUT `/api/machines/{id}`
Update machine.

**Authorization**: `[Authorize(Policy = "ManageMachines")]`

**Response** (200 OK): No content

---

### DELETE `/api/machines/{id}`
Delete machine.

**Authorization**: `[Authorize(Policy = "ManageMachines")]`

**Response** (200 OK): No content

---

## Regions

### GET `/api/regions`
Get all regions.

**Response** (200 OK): Array of regions

---

### GET `/api/regions/{id}`
Get region by ID.

**Response** (200 OK): Region details

---

### POST `/api/regions`
Create new region.

**Response** (201 Created): Created region

---

### PUT `/api/regions/{id}`
Update region.

**Response** (200 OK): No content

---

### DELETE `/api/regions/{id}`
Delete region.

**Response** (200 OK): No content

---

## User Assignments

### GET `/api/userassignments`
Get all user assignments.

**Response** (200 OK): Array of assignments

---

### GET `/api/userassignments/{id}`
Get assignment by ID.

**Response** (200 OK): Assignment details

---

### POST `/api/userassignments`
Create new assignment.

**Response** (201 Created): Created assignment

---

### DELETE `/api/userassignments/{id}`
Delete assignment.

**Response** (200 OK): No content

---

## Common Response Formats

### Success Response
```json
{
  "isSuccess": true,
  "value": { /* Data object */ },
  "error": null
}
```

### Error Response
```json
{
  "isSuccess": false,
  "value": null,
  "error": "Error message description"
}
```

### Validation Error Response
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "fieldName": ["Error message"]
  }
}
```

---

## Authorization Policies

| Policy | Allowed Roles |
|--------|---------------|
| `RequireAdminRole` | Admin, Administrator |
| `RequireUserRole` | User, StandardUser |
| `ReadDrillData` | Admin, BlastingEngineer, Operator |
| `ManageDrillData` | BlastingEngineer |
| `ManageProjectSites` | BlastingEngineer |
| `ManageMachines` | Admin, MachineManager |
| `ReadProjectData` | Admin, BlastingEngineer, Operator, MachineManager |
| `ManageExplosiveRequests` | Admin, StoreManager, BlastingEngineer |
| `RequireOwnership` | Resource owner or Admin |

---

## HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | OK - Request succeeded |
| 201 | Created - Resource created successfully |
| 400 | Bad Request - Invalid request data |
| 401 | Unauthorized - Missing/invalid authentication |
| 403 | Forbidden - Insufficient permissions |
| 404 | Not Found - Resource not found |
| 409 | Conflict - Resource already exists |
| 500 | Internal Server Error - Server error occurred |

---

## Notes

1. **All timestamps** are in UTC ISO 8601 format: `2025-01-01T12:00:00Z`
2. **JWT tokens** expire after configured duration (see `appsettings.json`)
3. **CORS** is enabled for `http://localhost:4200` and `http://localhost:4201`
4. **File uploads** use `multipart/form-data` encoding
5. **Circular references** in JSON are handled with `ReferenceHandler.IgnoreCycles`

---

**Last Updated**: October 2, 2025
