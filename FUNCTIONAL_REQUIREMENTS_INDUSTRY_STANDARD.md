# FUNCTIONAL REQUIREMENTS SPECIFICATION
## Drilling & Blasting Management System (DBMS)

**Document Version:** 3.0
**Date:** October 23, 2025
**Project:** DBMS - Enterprise Drilling & Blasting Management System
**Based on:** Comprehensive Implementation Analysis and Code Review

---

## TABLE OF CONTENTS

1. [Introduction](#1-introduction)
2. [Scope](#2-scope)
3. [Functional Requirements](#3-functional-requirements)
   - 3.1 [Authentication & Authorization](#31-authentication--authorization)
   - 3.2 [User Management](#32-user-management)
   - 3.3 [User Profile Management](#33-user-profile-management)
   - 3.4 [Regional Management](#34-regional-management)
   - 3.5 [Project Management](#35-project-management)
   - 3.6 [Project Site Management](#36-project-site-management)
   - 3.7 [Drilling Operations Management](#37-drilling-operations-management)
   - 3.8 [Blasting Operations Management](#38-blasting-operations-management)
   - 3.9 [Machine Management](#39-machine-management)
   - 3.10 [Store Management](#310-store-management)
   - 3.11 [Central Explosive Inventory Management](#311-central-explosive-inventory-management)
   - 3.12 [Inventory Transfer Management](#312-inventory-transfer-management)
   - 3.13 [Explosive Approval Requests](#313-explosive-approval-requests)
   - 3.14 [Explosive Calculation & Reporting](#314-explosive-calculation--reporting)
   - 3.15 [Maintenance Management](#315-maintenance-management)
   - 3.16 [Accessories Inventory Management](#316-accessories-inventory-management)
   - 3.17 [Analytics & Reporting](#317-analytics--reporting)
   - 3.18 [System Administration](#318-system-administration)

---

## 1. INTRODUCTION

### 1.1 Purpose
This document specifies the functional requirements for the Drilling & Blasting Management System (DBMS), an enterprise-level application designed to manage drilling operations, blasting sequences, explosive materials inventory, machinery, and multi-site project coordination.

### 1.2 Document Conventions
- The keyword "shall" indicates a mandatory requirement
- The keyword "should" indicates a recommended requirement
- The keyword "may" indicates an optional requirement
- All requirements are uniquely identified with the format [REQ-XXX-YYY]

### 1.3 Intended Audience
- System Administrators
- Project Managers
- Blasting Engineers
- Machine Managers
- Explosive Managers
- Store Managers
- Quality Assurance Teams
- Development Teams

---

## 2. SCOPE

### 2.1 System Overview
The DBMS is a web-based enterprise application implementing Clean Architecture principles with:
- Backend: ASP.NET Core 8 REST API
- Frontend: Angular 19 Single Page Application
- Database: SQL Server with Entity Framework Core 8
- Authentication: JWT-based with role-based access control

### 2.2 Major Features
- Multi-role authentication and authorization
- Regional project and site management
- Drilling pattern design and data management
- Blast sequence design and simulation
- Comprehensive explosive inventory lifecycle
- Machine and equipment tracking
- Multi-level approval workflows
- Reporting and analytics

---

## 3. FUNCTIONAL REQUIREMENTS

---

## 3.1 Authentication & Authorization

### 3.1.1 User Authentication

**[REQ-AUTH-001]** The system shall provide a secure login interface requiring Username (Email) and Password credentials.

**[REQ-AUTH-002]** The system shall authenticate user credentials against the database using BCrypt password hashing with cryptographic salt.

**[REQ-AUTH-003]** The system shall issue a JSON Web Token (JWT) upon successful authentication, containing the following claims:
- User ID
- Full Name
- Email Address
- Primary Role
- Assigned Region

**[REQ-AUTH-004]** The system shall use HS256 algorithm for JWT signature generation and validation.

**[REQ-AUTH-005]** The system shall validate JWT tokens on each authenticated API request, verifying signature, issuer, audience, and expiration.

**[REQ-AUTH-006]** The system shall track and record the last login timestamp for each user upon successful authentication.

**[REQ-AUTH-007]** The system shall provide a logout endpoint that invalidates the current session.

**[REQ-AUTH-008]** The system shall provide a token validation endpoint to verify token validity and extract user claims.

**[REQ-AUTH-009]** The system should implement automatic session timeout after 30 minutes of inactivity on the client side.

**[REQ-AUTH-010]** Upon session timeout, the system shall prompt the user for re-authentication to continue their session.

### 3.1.2 Account Security

**[REQ-AUTH-020]** The system should implement account lockout after a configurable number of consecutive failed login attempts (recommended: 5 attempts).

**[REQ-AUTH-021]** Upon account lockout, the system shall display the message: "Account locked – please reset your password".

**[REQ-AUTH-022]** The system shall prevent locked accounts from authenticating until password reset is completed.

### 3.1.3 Password Reset Workflow

**[REQ-AUTH-030]** The system shall provide a "Forgot Password" feature accessible from the login interface.

**[REQ-AUTH-031]** Upon password reset initiation, the system shall:
1. Validate the provided email address exists in the system
2. Generate a unique, time-limited password reset code
3. Store the reset code with user association and expiration timestamp
4. Send the reset code to the registered email address

**[REQ-AUTH-032]** The password reset code shall expire after 10 minutes from generation.

**[REQ-AUTH-033]** The system shall provide an endpoint to verify reset code validity before allowing password change.

**[REQ-AUTH-034]** The system shall require the following for password reset completion:
- Valid reset code
- User email address
- New password meeting complexity requirements

**[REQ-AUTH-035]** Upon successful password reset, the system shall:
1. Hash the new password using BCrypt
2. Update the user's password in the database
3. Invalidate the reset code
4. Send confirmation email to the user

**[REQ-AUTH-036]** The system shall enforce password complexity requirements for new passwords:
- Minimum 8 characters in length
- At least one uppercase letter (A-Z)
- At least one lowercase letter (a-z)
- At least one numeric digit (0-9)
- At least one special character (!@#$%^&*, etc.)

### 3.1.4 Role-Based Access Control (RBAC)

**[REQ-AUTH-040]** The system shall implement Role-Based Access Control for all protected resources.

**[REQ-AUTH-041]** The system shall support the following predefined roles:
1. **Admin** - Full system access and administrative privileges
2. **Blasting Engineer** - Manage drilling patterns, blast sequences, and site operations
3. **Mechanical Engineer** - Manage mechanical and maintenance operations
4. **Machine Manager** - Manage machine inventory and assignments
5. **Explosive Manager** - Manage explosive inventory and approve transfers
6. **Store Manager** - Manage store operations and inventory
7. **Operator** - Execute drilling operations and confirm completions

**[REQ-AUTH-042]** The system shall support many-to-many relationship between users and roles (a user may have multiple roles).

**[REQ-AUTH-043]** The system shall implement granular permissions for each module with Create, Read, Update, and Delete operations.

**[REQ-AUTH-044]** The system shall enforce the following authorization policies:

| Policy Name | Authorized Roles |
|------------|------------------|
| RequireAdminRole | Admin, Administrator |
| ReadDrillData | Admin, Blasting Engineer, Operator |
| ManageDrillData | Blasting Engineer, Operator |
| ManageProjectSites | Admin, Blasting Engineer, Operator |
| ManageMachines | Admin, Machine Manager |
| ReadProjectData | Admin, Blasting Engineer, Operator, Machine Manager |
| ManageExplosiveRequests | Admin, Store Manager, Blasting Engineer |
| ReadInventoryData | Admin, Store Manager, Explosive Manager, Blasting Engineer, Operator |
| ManageInventory | Admin, Store Manager, Explosive Manager |
| ApproveTransfers | Admin, Store Manager, Explosive Manager |

**[REQ-AUTH-045]** The system shall implement resource ownership-based authorization, verifying that users can only modify resources they own or are authorized to access.

**[REQ-AUTH-046]** Upon successful authentication, the system shall redirect users to a role-specific dashboard based on their primary role.

---

## 3.2 User Management

### 3.2.1 User Registration & Creation

**[REQ-USER-001]** The system shall provide a User Management interface accessible to users with Admin role after authentication.

**[REQ-USER-002]** The system shall allow Admins to register new user accounts with the following mandatory fields:
- Email Address (unique identifier)
- Password (meeting complexity requirements)
- First Name
- Last Name
- Home-Country Phone Number
- Resident-Country Phone Number
- Country of Origin
- Residential Address
- Primary Role assignment
- Regional assignment (optional)

**[REQ-USER-003]** The system shall validate that the Email Address is unique across all user accounts during registration.

**[REQ-USER-004]** The system shall reject user account creation if the provided Email Address already exists, displaying an appropriate error message.

**[REQ-USER-005]** The system shall hash passwords using BCrypt algorithm with cryptographic salt before storage.

**[REQ-USER-006]** The system shall never store or transmit passwords in plain text.

**[REQ-USER-007]** Upon successful user account creation, the system should send an email notification to the newly created user containing:
- Welcome message
- Temporary password (if applicable)
- Instructions for first login
- Password change requirement notice

**[REQ-USER-008]** The system should require users with temporary passwords to change their password upon first login.

### 3.2.2 User Account Management

**[REQ-USER-020]** The system shall provide functionality for Admins to retrieve user account details by User ID.

**[REQ-USER-021]** The system shall provide functionality to list all user accounts with the following information:
- User ID
- Email Address
- First Name
- Last Name
- Assigned Roles
- Account Status (Active, Inactive, Locked, Suspended)
- Region Assignment
- Creation Date
- Last Login Timestamp

**[REQ-USER-022]** The system shall exclude password hash from all user listing and detail endpoints.

**[REQ-USER-023]** The system shall allow Admins to update existing user account information, including:
- Personal information (name, phone numbers, address, country)
- Role assignments
- Regional assignment
- Account status

**[REQ-USER-024]** The system shall allow Admins to deactivate user accounts without deleting historical data.

**[REQ-USER-025]** The system shall allow Admins to permanently delete user accounts.

**[REQ-USER-026]** The system should send email notifications to users when their account information is modified by an Admin.

**[REQ-USER-027]** The email notification shall include:
- Description of changes made
- Timestamp of modification
- Identity of the Admin who made the changes

### 3.2.3 User Search & Filtering

**[REQ-USER-030]** The User Management interface shall provide search functionality for user accounts.

**[REQ-USER-031]** The system shall support filtering user lists by:
- Role
- Account Status (Active, Inactive, Locked, Suspended)
- Creation Date range
- Region
- Email domain

**[REQ-USER-032]** The system shall support pagination for user lists to optimize performance with large datasets.

**[REQ-USER-033]** The system shall support sorting user lists by Email, First Name, Last Name, Role, Status, and Creation Date.

---

## 3.3 User Profile Management

### 3.3.1 Profile Access & Display

**[REQ-PROFILE-001]** The system shall provide a User Profile interface accessible to each authenticated user.

**[REQ-PROFILE-002]** The system shall display the current user's profile information, including:
- Email Address (read-only)
- First Name
- Last Name
- Home-Country Phone Number
- Resident-Country Phone Number
- Country of Origin
- Residential Address
- Assigned Roles (read-only)
- Region Assignment (read-only)
- Last Login timestamp (read-only)

### 3.3.2 Profile Updates

**[REQ-PROFILE-010]** Users shall be able to edit the following personal information on their profile:
- First Name
- Last Name
- Home-Country Phone Number
- Resident-Country Phone Number
- Country of Origin
- Residential Address

**[REQ-PROFILE-011]** The system shall validate the format of Phone Numbers upon saving profile changes.

**[REQ-PROFILE-012]** The system shall validate the format of Address fields upon saving profile changes.

**[REQ-PROFILE-013]** The User Profile interface shall include a "Save Changes" button.

**[REQ-PROFILE-014]** The "Save Changes" button shall remain disabled until the user makes valid modifications to editable fields.

**[REQ-PROFILE-015]** Upon successful profile update, the system shall display a confirmation message to the user.

### 3.3.3 Password Management

**[REQ-PROFILE-020]** Users shall be able to change their password from the User Profile interface.

**[REQ-PROFILE-021]** The system shall require the user to enter their current password before allowing password change.

**[REQ-PROFILE-022]** The system shall verify the current password against the stored password hash before proceeding with password update.

**[REQ-PROFILE-023]** The system shall enforce password complexity requirements for new passwords (as defined in REQ-AUTH-036).

**[REQ-PROFILE-024]** Upon successful password change, the system shall:
1. Hash the new password using BCrypt
2. Update the password in the database
3. Send email confirmation to the user
4. Display a success message

**[REQ-PROFILE-025]** The system shall send an email notification to the user whenever their profile details are changed, including:
- Description of fields modified
- Timestamp of change
- Source of change (user self-service or admin modification)

---

## 3.4 Regional Management

### 3.4.1 Region Configuration

**[REQ-REGION-001]** The system shall maintain a database of operational regions.

**[REQ-REGION-002]** Each region shall have the following attributes:
- Unique Region ID
- Region Name
- Geographic coordinates (optional)

**[REQ-REGION-003]** The system shall pre-configure regions for Oman governorates:
- Muscat
- Dhofar
- Musandam
- Al Buraimi
- Ad Dakhiliyah
- Al Batinah North
- Al Batinah South
- Ash Sharqiyah North
- Ash Sharqiyah South
- Ad Dhahirah
- Al Wusta

**[REQ-REGION-004]** The system shall allow Admins to add, edit, or remove regions.

**[REQ-REGION-005]** The system shall provide an API endpoint to retrieve all active regions for use in dropdowns and filters.

### 3.4.2 Regional Associations

**[REQ-REGION-010]** The system shall support regional assignment for the following entities:
- Users
- Projects
- Machines
- Stores

**[REQ-REGION-011]** The system shall use regional filters to optimize data access and reporting.

---

## 3.5 Project Management

### 3.5.1 Project Creation

**[REQ-PROJECT-001]** The system shall provide a Project Management interface accessible to users with Admin and Blasting Engineer roles.

**[REQ-PROJECT-002]** General Managers (Admins) shall be able to create new projects with the following attributes:
- Project ID (unique, auto-generated)
- Project Name (unique, mandatory)
- Description (mandatory)
- Region (mandatory, from predefined regions)
- Area of Operation (mandatory)
- Start Date (optional)
- Expected End Date (optional)
- Assigned User/Manager (optional)

**[REQ-PROJECT-003]** The system shall validate that the Project Name is unique across all projects.

**[REQ-PROJECT-004]** The system shall reject project creation if the provided Project Name already exists, displaying an error message.

**[REQ-PROJECT-005]** The system shall prompt the user to re-enter project details if validation fails.

**[REQ-PROJECT-006]** The system shall initialize new projects with a status of "Planned".

### 3.5.2 Project Status Management

**[REQ-PROJECT-020]** The system shall support the following project statuses:
1. **Planned** - Project is in planning phase
2. **In Progress** - Active project with ongoing operations
3. **On Hold** - Temporarily suspended project
4. **Completed** - Project has been successfully completed
5. **Cancelled** - Project has been terminated

**[REQ-PROJECT-021]** Authorized users shall be able to update project status through the Project Management interface.

**[REQ-PROJECT-022]** The system shall record the timestamp of each status change.

**[REQ-PROJECT-023]** The system should notify assigned users when project status changes.

### 3.5.3 Project Updates & Deletion

**[REQ-PROJECT-030]** Admins and Blasting Engineers shall be able to edit the following project attributes:
- Project Name
- Description
- Region
- Area of Operation
- Start Date
- Expected End Date
- Assigned User
- Status

**[REQ-PROJECT-031]** The system shall maintain unique constraint validation on Project Name during updates.

**[REQ-PROJECT-032]** Admins shall be able to delete projects that have no associated sites or data.

**[REQ-PROJECT-033]** The system shall prevent deletion of projects that have associated sites, drill data, or blast data.

**[REQ-PROJECT-034]** Before deleting a project, the system shall display a confirmation dialog listing all dependent data that will be affected.

### 3.5.4 Project Dashboard & Listing

**[REQ-PROJECT-040]** The system shall display a Project Dashboard showing:
- Total number of projects
- Number of projects in Planned status
- Number of projects in In Progress status
- Number of projects in On Hold status
- Number of projects in Completed status
- Number of projects in Cancelled status

**[REQ-PROJECT-041]** The Project Management interface shall display a list of all projects accessible to the current user based on role and region.

**[REQ-PROJECT-042]** The project list shall support filtering by:
- Region
- Status
- Date range (start date, expected end date)

**[REQ-PROJECT-043]** The system shall provide search functionality for projects by Name or Description.

**[REQ-PROJECT-044]** The system shall support pagination and sorting for project lists.

### 3.5.5 Project Archival

**[REQ-PROJECT-050]** The system shall allow Admins to archive completed projects.

**[REQ-PROJECT-051]** Archived projects shall remain in the database but be excluded from active project lists by default.

**[REQ-PROJECT-052]** The system shall provide an option to view archived projects separately.

### 3.5.6 Resource Assignment

**[REQ-PROJECT-060]** The system shall allow assignment of machines to projects.

**[REQ-PROJECT-061]** The system shall allow assignment of operators to projects.

**[REQ-PROJECT-062]** The system shall notify assigned users when they are added to a project.

**[REQ-PROJECT-063]** The system shall track resource assignments with timestamps.

### 3.5.7 Operator-Specific Access

**[REQ-PROJECT-070]** The system shall provide an endpoint for operators to retrieve their assigned project.

**[REQ-PROJECT-071]** Operators shall only be able to view and interact with projects to which they are explicitly assigned.

---

## 3.6 Project Site Management

### 3.6.1 Site Creation

**[REQ-SITE-001]** The system shall display a list of all projects and their associated sites to authorized users.

**[REQ-SITE-002]** Blasting Engineers and Operators shall be able to select an assigned project and create a new site under that project.

**[REQ-SITE-003]** Site creation shall require the following mandatory attributes:
- Site ID (unique within project, auto-generated)
- Site Name (unique within project)
- Site Address (mandatory)
- Project ID (parent project reference)

**[REQ-SITE-004]** The system shall validate that the Site Name is unique within the selected project.

**[REQ-SITE-005]** The system shall reject site creation if a Site Name with the same name already exists within the selected project, displaying an error message.

**[REQ-SITE-006]** The system shall prompt the user to re-enter site details if validation fails.

**[REQ-SITE-007]** The system shall initialize new sites with a status of "Planned".

### 3.6.2 Site Status Management

**[REQ-SITE-020]** The system shall support the following site statuses:
1. **Planned** - Site is in planning phase
2. **Setup** - Site setup and preparation phase
3. **InProgress** - Active site with ongoing operations
4. **Blast** - Site ready for blasting operations
5. **OnHold** - Temporarily suspended site
6. **Completed** - Site operations have been successfully completed
7. **Cancelled** - Site has been terminated
8. **Archived** - Site is archived for historical reference

**[REQ-SITE-021]** Authorized users shall be able to update site status.

**[REQ-SITE-022]** The system shall record the timestamp of each status change.

### 3.6.3 Site Workflow Tracking

**[REQ-SITE-030]** The system shall track the following workflow milestones for each site:
- **IsPatternApproved** - Indicates drill pattern has been reviewed and approved
- **IsSimulationConfirmed** - Indicates blast simulation has been reviewed and confirmed
- **IsOperatorCompleted** - Indicates operator has completed drilling operations
- **IsCompleted** - Indicates all site operations are complete

**[REQ-SITE-031]** The system shall record timestamps for each workflow milestone completion.

**[REQ-SITE-032]** The system shall track which user completed each workflow milestone.

**[REQ-SITE-033]** When a site is marked as completed, the system shall:
1. Set IsCompleted flag to true
2. Record CompletedAt timestamp
3. Record CompletedByUserId

**[REQ-SITE-034]** The system shall allow authorized users to approve drill patterns for a site, setting IsPatternApproved to true.

**[REQ-SITE-035]** The system shall allow authorized users to confirm blast simulations for a site, setting IsSimulationConfirmed to true.

**[REQ-SITE-036]** The system shall allow operators to mark drilling operations as complete, setting IsOperatorCompleted to true.

### 3.6.4 Site Updates & Deletion

**[REQ-SITE-040]** Blasting Engineers shall be able to edit the following site attributes:
- Site Name
- Site Address
- Status
- Workflow milestone flags

**[REQ-SITE-041]** The system shall maintain unique constraint validation on Site Name within project during updates.

**[REQ-SITE-042]** Authorized users shall be able to delete sites.

**[REQ-SITE-043]** The system shall cascade delete all associated drill data, blast data, and calculations when a site is deleted.

**[REQ-SITE-044]** Before deleting a site, the system shall display a confirmation dialog listing all dependent data that will be removed.

### 3.6.5 Site Listing & Filtering

**[REQ-SITE-050]** The system shall provide an endpoint to retrieve all sites for a specific project.

**[REQ-SITE-051]** The system shall support filtering sites by status.

**[REQ-SITE-052]** The system shall display site completion percentage based on workflow milestones.

---

## 3.7 Drilling Operations Management

### 3.7.1 Drill Hole Creation

**[REQ-DRILL-001]** The system shall allow Blasting Engineers and Operators to create drill holes for project sites.

**[REQ-DRILL-002]** Each drill hole shall have the following attributes:
- Drill Hole ID (unique, auto-generated)
- Project ID (mandatory)
- Project Site ID (mandatory)
- Hole Number / Name (mandatory)
- Serial Number (optional)
- 3D Positioning Data (optional):
  - Easting coordinate
  - Northing coordinate
  - Elevation
  - Length
  - Depth
  - Azimuth (angle)
  - Dip (angle)
- 2D Positioning Data (used when 3D data unavailable):
  - X coordinate
  - Y coordinate
- ActualDepth, Stemming
- Drill Point collection (one or more drill points)

**[REQ-DRILL-003]** The system shall validate that drill holes are associated with valid projects and sites.

**[REQ-DRILL-004]** The system shall support batch creation of multiple drill holes in a single operation.

### 3.7.2 Drill Point Management

**[REQ-DRILL-010]** Each drill hole shall contain one or more drill points with the following attributes:
- Drill Point ID (unique, auto-generated)
- X Coordinate (ground position, mandatory)
- Y Coordinate (ground position, mandatory)
- Depth (vertical depth in meters, mandatory)
- Spacing (horizontal distance to adjacent holes in meters)
- Burden (distance to free face in meters)
- Diameter (hole diameter in millimeters)
- Stemming (length of stemming material in meters)
- Subdrill (depth below bench floor in meters)
- Volume (calculated rock volume to be fragmented)
- ANFO (calculated ANFO quantity in kg)
- Emulsion (calculated Emulsion quantity in kg)

**[REQ-DRILL-011]** The system shall allow Blasting Engineers to manually enter drill point parameters.

**[REQ-DRILL-012]** The system shall validate drill point parameters against acceptable ranges:
- Depth: 0-50 meters
- Diameter: 50-500 millimeters
- Spacing: 1-20 meters
- Burden: 1-20 meters
- Stemming: 0-10 meters

**[REQ-DRILL-013]** The system shall reject drill point creation if parameters are outside acceptable ranges, displaying appropriate error messages.

### 3.7.3 CSV Bulk Import

**[REQ-DRILL-020]** The system shall provide functionality to import drill data from CSV files.

**[REQ-DRILL-021]** The CSV file shall support the following columns:
- Hole ID
- X Coordinate
- Y Coordinate
- Depth
- Spacing
- Burden
- Diameter
- Stemming
- Subdrill
- Volume (optional, can be calculated)
- ANFO (optional, can be calculated)
- Emulsion (optional, can be calculated)

**[REQ-DRILL-022]** The system shall validate the CSV file format and structure before processing.

**[REQ-DRILL-023]** The system shall display validation errors with row numbers for invalid data entries.

**[REQ-DRILL-024]** The system shall provide a preview of imported data before final save, allowing users to review and confirm.

**[REQ-DRILL-025]** Blasting Engineers shall be able to save or reject imported drill data after review.

**[REQ-DRILL-026]** Upon successful import, the system shall create drill holes and drill points from the CSV data.

**[REQ-DRILL-027]** The system shall support bulk updates to existing drill data through CSV re-import.

### 3.7.4 Drill Pattern Configuration

**[REQ-DRILL-030]** The system shall maintain pattern settings for drill hole generation with the following configurable parameters:
- Default Spacing (meters)
- Default Burden (meters)
- Default Depth (meters)
- Default Diameter (millimeters)
- Default Stemming (meters)
- Grid layout parameters

**[REQ-DRILL-031]** The system shall allow Blasting Engineers to save drill patterns as named templates.

**[REQ-DRILL-032]** Users shall be able to manage saved drill pattern templates with options for:
- Editing template parameters
- Duplicating templates
- Deleting templates
- Applying templates to new sites

### 3.7.5 Drill Data Updates & Deletion

**[REQ-DRILL-040]** Blasting Engineers and Operators shall be able to update drill point parameters after creation.

**[REQ-DRILL-041]** The system shall validate updated parameters against acceptable ranges (as defined in REQ-DRILL-012).

**[REQ-DRILL-042]** The system shall recalculate explosive requirements when drill point parameters are modified.

**[REQ-DRILL-043]** Authorized users shall be able to delete individual drill holes or drill points.

**[REQ-DRILL-044]** The system shall provide functionality to delete all drill data for a specific project site.

**[REQ-DRILL-045]** Before deleting drill data, the system shall display a confirmation dialog indicating the number of drill holes and points that will be removed.

### 3.7.6 Drill Point Completion Tracking

**[REQ-DRILL-050]** The system shall track completion status for each drill point.

**[REQ-DRILL-051]** Each drill point shall have the following completion attributes:
- IsCompleted (boolean flag)
- CompletedAt (timestamp)
- CompletedByUserId (user who confirmed completion)

**[REQ-DRILL-052]** Operators shall be able to mark drill points as completed after drilling operations.

**[REQ-DRILL-053]** The system shall record the timestamp and operator identity when a drill point is marked complete.

**[REQ-DRILL-054]** The system shall calculate and display the completion percentage for each site based on completed drill points.

### 3.7.7 Drill Data Access & Listing

**[REQ-DRILL-060]** The system shall provide endpoints to retrieve drill data with the following access patterns:
- All drill holes (with pagination)
- Drill holes by Project ID
- Drill holes by Project Site ID
- Individual drill hole by ID with all drill points

**[REQ-DRILL-061]** The system shall enforce read authorization based on user role (ReadDrillData policy).

**[REQ-DRILL-062]** The system shall support filtering drill data by:
- Project
- Site
- Completion status
- Date range

---

## 3.8 Blasting Operations Management

### 3.8.1 Blast Connection Design

**[REQ-BLAST-001]** The system shall provide functionality for designing blast connections between drill points.

**[REQ-BLAST-002]** Each blast connection shall link two drill points with the following attributes:
- Connection ID (unique, auto-generated)
- Project ID (mandatory)
- Project Site ID (mandatory)
- Point 1 ID (source drill point, mandatory)
- Point 2 ID (target drill point, mandatory)
- Connector Type (mandatory)
- Delay (milliseconds, mandatory)
- Sequence (blast order number, mandatory)
- IsStartingHole (boolean flag)

**[REQ-BLAST-003]** The system shall support the following connector types:
1. **Detonating Cord** - Explosive cord connecting holes
2. **Connectors** - Non-explosive connectors (surface or downline)

**[REQ-BLAST-004]** Blasting Engineers shall be able to create blast connections through a visual interface.

**[REQ-BLAST-005]** The system shall validate that both drill points exist before creating a connection.

**[REQ-BLAST-006]** The system shall prevent creation of duplicate connections between the same two drill points.

**[REQ-BLAST-007]** The system shall allow designation of starting holes (initiation points) for the blast sequence.

### 3.8.2 Detonator Assignment

**[REQ-BLAST-020]** The system shall support assignment of detonators to drill points.

**[REQ-BLAST-021]** The system shall support the following detonator types:
1. **Electric** - Electrically initiated detonators
2. **Non-Electric** - Shock tube detonators
3. **Electronic** - Programmable electronic detonators

**[REQ-BLAST-022]** Each detonator assignment shall include:
- Detonator Type
- Delay time (milliseconds)
- Sequence number

**[REQ-BLAST-023]** The system shall validate that delay times are within acceptable ranges (0-10000 milliseconds).

### 3.8.3 Blast Sequence Simulation

**[REQ-BLAST-030]** The system shall provide a blast sequence simulation feature with visual timeline representation.

**[REQ-BLAST-031]** The simulation shall display:
- Firing order of drill holes based on delay times
- Timeline visualization of detonation sequence
- Visual highlighting of holes as they detonate in sequence
- Total blast duration

**[REQ-BLAST-032]** The simulation shall support interactive controls:
- Play/Pause
- Speed adjustment
- Step-by-step advancement
- Reset to beginning

**[REQ-BLAST-033]** The simulation shall visually update the 2D pattern view to reflect the blast sequence animation.

**[REQ-BLAST-034]** Users shall be able to review and confirm blast simulations for approval.

**[REQ-BLAST-035]** Upon simulation confirmation, the system shall set the IsSimulationConfirmed flag for the site.

### 3.8.4 Blasting Data Storage

**[REQ-BLAST-040]** The system shall store blasting workflow data for each project site.

**[REQ-BLAST-041]** Blasting data shall be stored in JSON format to support flexible schema evolution.

**[REQ-BLAST-042]** The stored blasting data shall include:
- Drill pattern configuration
- Blast connections
- Detonator assignments
- Simulation settings
- Workflow state

**[REQ-BLAST-043]** The system shall provide endpoints to retrieve and update blasting data for a specific project site.

### 3.8.5 Blast Connection Management

**[REQ-BLAST-050]** Blasting Engineers shall be able to update existing blast connections, including:
- Connector type
- Delay timing
- Sequence order

**[REQ-BLAST-051]** Blasting Engineers shall be able to delete individual blast connections.

**[REQ-BLAST-052]** The system shall provide functionality to delete all blasting workflow data for a specific project site.

**[REQ-BLAST-053]** Before deleting blasting data, the system shall display a confirmation dialog.

### 3.8.6 Blast Data Access

**[REQ-BLAST-060]** The system shall provide endpoints to retrieve blast connections with the following access patterns:
- All connections for a project site
- Individual connection by ID
- Connections by sequence order

**[REQ-BLAST-061]** The system shall enforce authorization based on user role for blasting operations.

---

## 3.9 Machine Management

### 3.9.1 Machine Inventory

**[REQ-MACHINE-001]** The system shall provide a Machine Inventory interface accessible to Machine Managers and Admins.

**[REQ-MACHINE-002]** Machine Managers shall be able to add machines to the inventory with the following attributes:
- Machine ID (unique, auto-generated)
- Machine Name (mandatory)
- Type (mandatory)
- Model (mandatory)
- Manufacturer (mandatory)
- Serial Number (unique, mandatory)
- Rig Number (optional)
- Plate Number (optional)
- Chassis Details (optional)
- Manufacturing Year (mandatory)
- Current Location (optional)
- Specifications (flexible JSON field for additional technical details)

**[REQ-MACHINE-003]** The system shall validate that Serial Number is unique across all machines.

**[REQ-MACHINE-004]** The system shall reject machine creation if the provided Serial Number already exists.

### 3.9.2 Machine Status Management

**[REQ-MACHINE-020]** The system shall support the following machine statuses:
1. **Available** - Machine is available for assignment
2. **In Use** - Machine is currently assigned and operational
3. **Under Maintenance** - Machine is undergoing maintenance
4. **Out of Service** - Machine is not operational

**[REQ-MACHINE-021]** The system shall display the real-time availability or assignment status of each machine.

**[REQ-MACHINE-022]** Authorized users shall be able to update machine status.

**[REQ-MACHINE-023]** The system shall automatically update machine status to "In Use" when assigned to a project or operator.

**[REQ-MACHINE-024]** The system shall automatically update machine status to "Available" when assignments are removed.

### 3.9.3 Machine Assignment

**[REQ-MACHINE-030]** The system shall allow assignment of machines to projects.

**[REQ-MACHINE-031]** The system shall allow assignment of machines to operators.

**[REQ-MACHINE-032]** Each machine assignment shall record:
- Assigned Project ID
- Assigned Operator User ID
- Assignment Date (timestamp)

**[REQ-MACHINE-033]** The system shall validate that operators exist before assigning machines.

**[REQ-MACHINE-034]** The system shall notify assigned machine operators of new assignments via in-app notification.

**[REQ-MACHINE-035]** The system shall notify requesting General Managers when machine assignments are completed.

**[REQ-MACHINE-036]** An operator shall be assigned to only one machine at a time.

**[REQ-MACHINE-037]** A machine shall be assigned to only one operator at a time.

**[REQ-MACHINE-038]** The system shall maintain a history of all machine assignments with timestamps.

### 3.9.4 Machine Assignment Requests (Optional Enhancement)

**[REQ-MACHINE-040]** The system should allow General Managers to submit machine assignment requests specifying:
- Project ID
- Machine Type required
- Quantity needed
- Request justification or details

**[REQ-MACHINE-041]** The system should route all machine assignment requests to Machine Managers for review.

**[REQ-MACHINE-042]** Machine Managers should be able to review requests and:
- Approve and assign specific machines
- Reject requests with reason

**[REQ-MACHINE-043]** The system should notify requesters of assignment request status changes.

### 3.9.5 Maintenance Tracking

**[REQ-MACHINE-050]** The system shall track maintenance dates for each machine with:
- Last Maintenance Date
- Next Maintenance Date

**[REQ-MACHINE-051]** Machine Managers shall be able to schedule machines for maintenance.

**[REQ-MACHINE-052]** The system should generate alerts when machines approach scheduled maintenance dates.

**[REQ-MACHINE-053]** The system shall maintain a history of all maintenance events with timestamps.

**[REQ-MACHINE-054]** Machine Operators should be able to log daily usage data for machines, including:
- Engine hours
- Idle hours
- Service hours

**[REQ-MACHINE-055]** The system should calculate remaining service hours based on logged usage and configured service intervals.

**[REQ-MACHINE-056]** The system should trigger alerts when a machine approaches its defined service threshold based on remaining service hours.

### 3.9.6 Machine Updates & Deletion

**[REQ-MACHINE-060]** Machine Managers shall be able to edit existing machine details.

**[REQ-MACHINE-061]** Machine Managers shall be able to delete machines from the inventory.

**[REQ-MACHINE-062]** The system shall prevent deletion of machines that are currently assigned to projects or operators.

**[REQ-MACHINE-063]** Before deleting a machine, the system shall display a confirmation dialog.

### 3.9.7 Machine Listing & Filtering

**[REQ-MACHINE-070]** The Machine Inventory interface shall support filtering by:
- Model
- Type
- Status (Available, In Use, Under Maintenance, Out of Service)
- Project assignment
- Operator assignment
- Region

**[REQ-MACHINE-071]** The system shall provide an endpoint to retrieve a machine by assigned operator ID.

**[REQ-MACHINE-072]** The system shall support search functionality for machines by Name, Serial Number, Rig Number, or Plate Number.

**[REQ-MACHINE-073]** The system shall support pagination and sorting for machine lists.

---

## 3.10 Store Management

### 3.10.1 Store Creation & Configuration

**[REQ-STORE-001]** The system shall provide a Store Management interface accessible to Explosive Managers and Admins.

**[REQ-STORE-002]** Explosive Managers shall be able to create new stores with the following attributes:
- Store ID (unique, auto-generated)
- Store Name (mandatory, unique)
- Store Address (mandatory)
- City (mandatory)
- Store Manager User ID (mandatory, must be a user with Store Manager role)
- Region ID (mandatory)
- Storage Capacity (total capacity in kg, mandatory)
- Allowed Explosive Types (comma-separated list, mandatory)

**[REQ-STORE-003]** The system shall validate that Store Name is unique across all stores.

**[REQ-STORE-004]** The system shall validate that the assigned Store Manager has the Store Manager role.

**[REQ-STORE-005]** The system shall support the following explosive types for stores:
- ANFO (Ammonium Nitrate Fuel Oil)
- Emulsion

**[REQ-STORE-006]** Upon successful store creation, the system shall:
1. Generate and assign a unique Store ID
2. Initialize store with Operational status
3. Create an empty inventory record
4. Notify the designated Store Manager via email

**[REQ-STORE-007]** The email notification to the Store Manager shall include:
- Store name and location
- Assigned manager information
- Storage capacity
- Allowed explosive types

### 3.10.2 Store Status Management

**[REQ-STORE-020]** The system shall support the following store statuses:
1. **Operational** - Store is active and functional
2. **Under Maintenance** - Store is temporarily unavailable
3. **Decommissioned** - Store is permanently closed

**[REQ-STORE-021]** Explosive Managers shall be able to update store status.

**[REQ-STORE-022]** The system shall provide a dedicated endpoint to update store status.

**[REQ-STORE-023]** When a store status is changed to "Under Maintenance" or "Decommissioned", the system should notify:
- Assigned Store Manager
- Explosive Manager
- Users with pending transfer requests to that store

### 3.10.3 Store Updates & Deletion

**[REQ-STORE-030]** Explosive Managers shall be able to edit store details, including:
- Store Name
- Store Address
- City
- Store Manager assignment
- Storage Capacity
- Allowed Explosive Types
- Status

**[REQ-STORE-031]** The system shall maintain unique constraint validation on Store Name during updates.

**[REQ-STORE-032]** The system shall require confirmation before deleting a store.

**[REQ-STORE-033]** Upon deletion confirmation, the system shall:
1. Remove the store from the database
2. Delete all associated inventory records
3. Cancel all pending transfer requests to/from that store
4. Notify the affected Store Manager via email

**[REQ-STORE-034]** The system shall allow Explosive Managers to deactivate a store (set status to Decommissioned) while retaining all historical data.

**[REQ-STORE-035]** Decommissioned stores shall be excluded from active store lists but remain accessible for historical reporting.

### 3.10.4 Store Inventory Tracking

**[REQ-STORE-040]** The system shall maintain inventory records for each store with the following attributes per explosive type:
- Store ID (foreign key)
- Explosive Type (ANFO, Emulsion)
- Quantity (current available quantity in kg)
- Reserved Quantity (allocated but not yet dispatched, in kg)
- Unit (kg, lbs, tons)
- Minimum Stock Level (threshold for low-stock alerts, in kg)
- Maximum Stock Level (capacity limit, in kg)
- Last Restocked Date (timestamp)
- Expiry Date (optional)
- Batch Number (optional)
- Supplier (optional)

**[REQ-STORE-041]** The system shall calculate available quantity as: Total Quantity - Reserved Quantity.

**[REQ-STORE-042]** The system shall calculate current occupancy for each store based on sum of all inventory quantities.

**[REQ-STORE-043]** The system shall prevent inventory additions that would exceed the store's storage capacity.

**[REQ-STORE-044]** The system shall display a validation error if an inventory transaction would exceed storage capacity.

### 3.10.5 Store Transaction Management

**[REQ-STORE-050]** The system shall log all inventory transactions for each store.

**[REQ-STORE-051]** Each transaction record shall include:
- Transaction ID (unique, auto-generated)
- Store ID
- Transaction Type (Addition, Consumption, Reservation, Release, Transfer In, Transfer Out)
- Explosive Type
- Quantity (with positive/negative values based on transaction type)
- Transaction Date (timestamp)
- User ID (who performed the transaction)
- Reference Number (optional, for linking to transfer requests or approvals)
- Notes (optional)

**[REQ-STORE-052]** The system shall automatically create transaction records when:
- Inventory is received from central warehouse
- Inventory is consumed for blasting operations
- Inventory is reserved for approved requests
- Reservations are released or cancelled

### 3.10.6 Store Capacity & Utilization

**[REQ-STORE-060]** The system shall calculate and display current occupancy for each store.

**[REQ-STORE-061]** The system shall calculate utilization rate as: (Current Occupancy / Storage Capacity) × 100.

**[REQ-STORE-062]** The system shall provide an endpoint to retrieve utilization percentage for a specific store.

**[REQ-STORE-063]** The system shall display visual indicators (progress bars, color coding) for store capacity:
- Green: < 70% utilization
- Yellow: 70-90% utilization
- Red: > 90% utilization

### 3.10.7 Store Search & Filtering

**[REQ-STORE-070]** The system shall allow Explosive Managers to view, search, and filter all stores.

**[REQ-STORE-071]** The system shall support filtering stores by:
- Status (Operational, Under Maintenance, Decommissioned)
- Region
- Store Manager
- Allowed Explosive Types
- Utilization percentage range

**[REQ-STORE-072]** The system shall provide a dedicated search endpoint with query parameters for filtering.

**[REQ-STORE-073]** The system shall support sorting stores by Name, City, Region, Storage Capacity, and Utilization.

**[REQ-STORE-074]** The system shall provide an endpoint to retrieve stores by Region ID.

**[REQ-STORE-075]** The system shall provide an endpoint to retrieve store(s) managed by a specific user.

### 3.10.8 Store Statistics

**[REQ-STORE-080]** The system shall provide aggregate statistics for the store network:
- Total number of stores
- Number of operational stores
- Total storage capacity across all stores
- Total current occupancy across all stores
- Average utilization rate
- Number of stores by region

**[REQ-STORE-081]** The system shall provide a dedicated endpoint to retrieve store statistics.

---

## 3.11 Central Explosive Inventory Management

### 3.11.1 Central Warehouse Inventory

**[REQ-CENTRAL-001]** The system shall maintain a central warehouse inventory for explosive materials.

**[REQ-CENTRAL-002]** Each inventory batch shall have the following attributes:
- Inventory ID (unique, auto-generated)
- Batch ID (unique identifier, mandatory)
- Explosive Type (ANFO or Emulsion, mandatory)
- Quantity (total quantity in kg, mandatory)
- Allocated Quantity (quantity reserved for transfers, in kg)
- Manufacturing Date (mandatory)
- Expiry Date (mandatory)
- Supplier (mandatory)
- Manufacturer Batch Number (optional)
- Storage Location within warehouse (mandatory)
- Status (mandatory)

**[REQ-CENTRAL-003]** The system shall support the following inventory statuses:
1. **Available** - Ready for allocation and transfer
2. **Allocated** - Reserved for approved transfer requests
3. **Depleted** - Fully consumed, zero quantity remaining
4. **Expired** - Past expiry date, must not be used
5. **Quarantined** - Under quality review, cannot be used

**[REQ-CENTRAL-004]** The system shall calculate Available Quantity as: Total Quantity - Allocated Quantity.

**[REQ-CENTRAL-005]** The system shall calculate Days Until Expiry based on current date and Expiry Date.

**[REQ-CENTRAL-006]** The system shall automatically mark inventory as Expired when current date exceeds Expiry Date.

**[REQ-CENTRAL-007]** The system shall flag inventory as "Expiring Soon" when Days Until Expiry is less than 30 days.

### 3.11.2 ANFO Technical Properties

**[REQ-CENTRAL-020]** For ANFO inventory items, the system shall store the following technical properties:
- Fume Class (classification of toxic fumes produced)
- Grade (quality grade of ANFO)
- Sensitization Type (method of sensitization)
- Density (g/cm³)
- Velocity of Detonation (m/s)
- Weight Strength (% relative to ANFO standard)
- Bulk Strength (relative explosive energy)

**[REQ-CENTRAL-021]** ANFO technical properties shall be linked to the central warehouse inventory record via foreign key relationship.

### 3.11.3 Emulsion Technical Properties

**[REQ-CENTRAL-030]** For Emulsion inventory items, the system shall store the following technical properties:
- Density (g/cm³)
- Velocity of Detonation (m/s)
- Water Resistance rating
- Sensitization method
- Critical Diameter (minimum diameter for stable detonation, mm)
- Cartridge Diameter (for cartridged emulsions, mm)

**[REQ-CENTRAL-031]** Emulsion technical properties shall be linked to the central warehouse inventory record via foreign key relationship.

### 3.11.4 Quality Control

**[REQ-CENTRAL-040]** The system shall support quality check records for inventory batches.

**[REQ-CENTRAL-041]** Each quality check record shall include:
- Quality Check ID (unique, auto-generated)
- Central Warehouse Inventory ID (foreign key)
- Check Date (timestamp)
- Performed By User ID
- Quality Status (Pass, Fail, Pending)
- Test Results (flexible JSON field for detailed test data)
- Notes (optional)

**[REQ-CENTRAL-042]** Quality Managers shall be able to create quality check records for inventory batches.

**[REQ-CENTRAL-043]** If a quality check fails, the system shall automatically update the inventory status to "Quarantined".

**[REQ-CENTRAL-044]** Quarantined inventory shall not be available for allocation or transfer until status is changed to "Available" by authorized personnel.

### 3.11.5 Inventory Creation & Updates

**[REQ-CENTRAL-050]** Explosive Managers shall be able to add new inventory batches to the central warehouse.

**[REQ-CENTRAL-051]** The system shall validate that Batch ID is unique across all inventory records.

**[REQ-CENTRAL-052]** The system shall validate that Expiry Date is later than Manufacturing Date.

**[REQ-CENTRAL-053]** The system shall require technical properties (ANFO or Emulsion) to be provided based on Explosive Type.

**[REQ-CENTRAL-054]** Explosive Managers shall be able to update existing inventory details, including:
- Quantity (additions to stock)
- Storage Location
- Status
- Technical properties

**[REQ-CENTRAL-055]** The system shall prevent updates that would result in negative Available Quantity.

**[REQ-CENTRAL-056]** The system shall log all inventory quantity changes in an audit trail.

### 3.11.6 Inventory Deletion

**[REQ-CENTRAL-060]** Explosive Managers shall be able to delete inventory batches.

**[REQ-CENTRAL-061]** The system shall prevent deletion of inventory batches that:
- Have Allocated Quantity > 0
- Are referenced in active transfer requests
- Have status "Allocated" or "Quarantined"

**[REQ-CENTRAL-062]** Before deleting an inventory batch, the system shall display a confirmation dialog.

**[REQ-CENTRAL-063]** Upon deletion, the system shall cascade delete associated quality check records and technical properties.

### 3.11.7 Inventory Search & Filtering

**[REQ-CENTRAL-070]** The system shall provide advanced search functionality for central warehouse inventory.

**[REQ-CENTRAL-071]** The system shall support filtering inventory by:
- Explosive Type (ANFO, Emulsion)
- Status (Available, Allocated, Depleted, Expired, Quarantined)
- Supplier
- Manufacturing Date range
- Expiry Date range
- Batch ID (partial match)

**[REQ-CENTRAL-072]** The system shall support pagination for inventory lists to optimize performance with large datasets.

**[REQ-CENTRAL-073]** The system shall support sorting inventory by Batch ID, Manufacturing Date, Expiry Date, Quantity, and Status.

**[REQ-CENTRAL-074]** The system shall provide a search endpoint with query parameters for filtering.

### 3.11.8 Low Stock & Expiry Monitoring

**[REQ-CENTRAL-080]** Explosive Managers shall be able to set and modify low-stock thresholds for each explosive type.

**[REQ-CENTRAL-081]** The system shall monitor total available quantity across all batches for each explosive type.

**[REQ-CENTRAL-082]** The system shall trigger a low-stock alert when total available quantity falls below the defined threshold.

**[REQ-CENTRAL-083]** Low-stock alerts shall be delivered via in-app notifications to Explosive Managers.

**[REQ-CENTRAL-084]** The system shall generate daily reports listing all inventory batches expiring within 30 days.

**[REQ-CENTRAL-085]** The system shall send email notifications to Explosive Managers for batches expiring within 7 days.

---

## 3.12 Inventory Transfer Management

### 3.12.1 Transfer Request Creation

**[REQ-TRANSFER-001]** Store Managers shall be able to submit inventory transfer requests from central warehouse to their assigned store.

**[REQ-TRANSFER-002]** Each transfer request shall have the following attributes:
- Transfer Request ID (unique, auto-generated)
- Request Number (unique, formatted identifier)
- Central Warehouse Inventory ID (source batch, mandatory)
- Destination Store ID (mandatory)
- Requested Quantity (in kg, mandatory)
- Approved Quantity (in kg, set during approval)
- Required By Date (target delivery date, optional)
- Status (mandatory)
- Priority (optional)
- Request Notes (optional)

**[REQ-TRANSFER-003]** The system shall validate that the requested quantity does not exceed the available quantity in the source inventory batch.

**[REQ-TRANSFER-004]** The system shall validate that the destination store allows the explosive type being requested.

**[REQ-TRANSFER-005]** The system shall validate that the requested quantity + current store occupancy does not exceed the destination store's storage capacity.

**[REQ-TRANSFER-006]** Upon successful transfer request creation, the system shall:
1. Generate and assign a unique Request Number
2. Set status to "Pending"
3. Record the requesting user ID
4. Record the request date timestamp
5. Route the request to Explosive Manager for approval

**[REQ-TRANSFER-007]** The system shall notify the Explosive Manager of new pending transfer requests via in-app notification.

### 3.12.2 Transfer Request Status Workflow

**[REQ-TRANSFER-020]** The system shall support the following transfer request statuses:
1. **Pending** - Awaiting approval from Explosive Manager
2. **Approved** - Approved by Explosive Manager, ready for dispatch
3. **Rejected** - Rejected by Explosive Manager
4. **Dispatched** - Explosives have been sent from central warehouse
5. **Delivered** - Explosives received at destination store
6. **Cancelled** - Request cancelled by Store Manager or Admin

**[REQ-TRANSFER-021]** The transfer request lifecycle shall follow this workflow:

```
Create Request → Pending
    ↓
Approve/Reject → Approved/Rejected
    ↓ (if Approved)
Mark Dispatched → Dispatched
    ↓
Confirm Delivery → Delivered
```

**[REQ-TRANSFER-022]** Each status transition shall be recorded with timestamp and user ID.

### 3.12.3 Transfer Request Approval

**[REQ-TRANSFER-030]** The system shall route all transfer requests from Store Managers to Explosive Managers for approval.

**[REQ-TRANSFER-031]** Explosive Managers shall be able to review all pending transfer requests with details:
- Source inventory batch information
- Destination store information
- Requested quantity
- Available quantity in source batch
- Destination store capacity and current occupancy
- Required by date
- Request notes

**[REQ-TRANSFER-032]** Explosive Managers shall be able to approve transfer requests.

**[REQ-TRANSFER-033]** Upon approval, the system shall:
1. Update transfer request status to "Approved"
2. Record approved quantity (may differ from requested quantity)
3. Record approving user ID
4. Record approval date timestamp
5. Allocate the approved quantity in the source inventory (update Allocated Quantity)
6. Record approval notes (optional)
7. Notify the requesting Store Manager via in-app notification

**[REQ-TRANSFER-034]** Explosive Managers shall be able to approve a quantity different from (less than or equal to) the requested quantity.

**[REQ-TRANSFER-035]** The system shall validate that approved quantity does not exceed available quantity in source batch.

### 3.12.4 Transfer Request Rejection

**[REQ-TRANSFER-040]** Explosive Managers shall be able to reject transfer requests.

**[REQ-TRANSFER-041]** Upon rejection, the system shall require a rejection reason.

**[REQ-TRANSFER-042]** Upon rejection, the system shall:
1. Update transfer request status to "Rejected"
2. Record rejecting user ID
3. Record rejection date timestamp
4. Store rejection reason
5. Notify the requesting Store Manager with justification via in-app notification

**[REQ-TRANSFER-043]** Rejected transfer requests shall not allocate any inventory quantity.

### 3.12.5 Dispatch Management

**[REQ-TRANSFER-050]** Warehouse personnel shall be able to mark approved transfer requests as dispatched.

**[REQ-TRANSFER-051]** When marking a request as dispatched, the system shall require:
- Truck Number (vehicle identification)
- Driver Name
- Driver Contact Number
- Dispatch Date (timestamp)

**[REQ-TRANSFER-052]** Upon dispatch, the system shall:
1. Update transfer request status to "Dispatched"
2. Record dispatch date timestamp
3. Deduct approved quantity from source inventory total quantity
4. Maintain allocated quantity until delivery confirmation
5. Notify the destination Store Manager with dispatch details

**[REQ-TRANSFER-053]** The dispatch notification to Store Manager shall include:
- Expected delivery information
- Truck number
- Driver contact details
- Quantity being delivered
- Source batch details

### 3.12.6 Delivery Confirmation

**[REQ-TRANSFER-060]** Store Managers shall be able to confirm delivery of dispatched transfer requests.

**[REQ-TRANSFER-061]** Upon delivery confirmation, the system shall:
1. Update transfer request status to "Delivered"
2. Record delivery confirmation date timestamp
3. Record confirming user ID (Store Manager)
4. Release allocated quantity from source inventory
5. Add delivered quantity to destination store inventory
6. Create store transaction record for "Transfer In"
7. Notify Explosive Manager of successful delivery

**[REQ-TRANSFER-062]** If the destination store does not have an existing inventory record for the explosive type, the system shall create one.

**[REQ-TRANSFER-063]** The system shall update the destination store inventory:
- Increase total quantity by delivered amount
- Update last restocked date
- Copy batch number and expiry date from source

**[REQ-TRANSFER-064]** Store Managers should be able to report discrepancies between dispatched and delivered quantities.

**[REQ-TRANSFER-065]** If a discrepancy is reported, the system should flag the transfer for investigation and notify the Explosive Manager.

### 3.12.7 Transfer Request Cancellation

**[REQ-TRANSFER-070]** Store Managers shall be able to cancel transfer requests in "Pending" status.

**[REQ-TRANSFER-071]** Admins and Explosive Managers shall be able to cancel transfer requests in "Pending" or "Approved" status.

**[REQ-TRANSFER-072]** Upon cancellation, the system shall:
1. Update transfer request status to "Cancelled"
2. Release any allocated quantity in source inventory
3. Record cancellation date and user
4. Notify relevant parties (Store Manager, Explosive Manager)

**[REQ-TRANSFER-073]** The system shall prevent cancellation of transfer requests in "Dispatched" or "Delivered" status.

### 3.12.8 Transfer Request Updates

**[REQ-TRANSFER-080]** Store Managers shall be able to update transfer requests in "Pending" status, modifying:
- Requested quantity
- Required by date
- Request notes

**[REQ-TRANSFER-081]** Explosive Managers shall be able to update transfer requests in any status except "Delivered" or "Cancelled".

**[REQ-TRANSFER-082]** The system shall re-validate quantity constraints when a transfer request is updated.

**[REQ-TRANSFER-083]** The system shall log all modifications to transfer requests in an audit trail.

### 3.12.9 Transfer Request Listing & Filtering

**[REQ-TRANSFER-090]** The system shall provide endpoints to retrieve transfer requests with appropriate filtering.

**[REQ-TRANSFER-091]** The system shall support filtering transfer requests by:
- Status (Pending, Approved, Rejected, Dispatched, Delivered, Cancelled)
- Destination Store ID
- Source Inventory Batch ID
- Date range (request date, required by date, approval date, dispatch date, delivery date)
- Requesting user
- Approving user

**[REQ-TRANSFER-092]** Store Managers shall only see transfer requests for stores they manage.

**[REQ-TRANSFER-093]** Explosive Managers shall see all transfer requests across all stores.

**[REQ-TRANSFER-094]** The system shall support pagination and sorting for transfer request lists.

---

## 3.13 Explosive Approval Requests

### 3.13.1 Explosive Usage Request Creation

**[REQ-APPROVAL-001]** Blasting Engineers shall be able to submit explosive usage approval requests for project sites.

**[REQ-APPROVAL-002]** Each approval request shall have the following attributes:
- Approval Request ID (unique, auto-generated)
- Project Site ID (mandatory)
- Requested By User ID (auto-populated)
- Approval Type (mandatory)
- Priority (mandatory)
- Expected Usage Date (optional)
- Blasting Date (optional)
- Blast Timing (time of day, optional)
- Safety Checklist Completed (boolean, mandatory)
- Environmental Assessment Completed (boolean, mandatory)
- Request Notes (justification, optional)
- Additional Data (flexible JSON field for extensibility)
- Status (mandatory)

**[REQ-APPROVAL-003]** The system shall support the following approval types:
1. **Standard** - Regular blasting operations
2. **Emergency** - Urgent safety-related blasting
3. **Maintenance** - Maintenance-related explosive use
4. **Testing** - Testing or trial blasts
5. **Research** - Research and development purposes

**[REQ-APPROVAL-004]** The system shall support the following priority levels:
1. **Low** - Routine operations
2. **Normal** - Standard priority
3. **High** - Important, time-sensitive
4. **Critical** - Urgent, safety-critical

**[REQ-APPROVAL-005]** The system shall require that Safety Checklist is completed (set to true) before allowing request submission.

**[REQ-APPROVAL-006]** The system shall require that Environmental Assessment is completed (set to true) before allowing request submission.

**[REQ-APPROVAL-007]** Upon successful request creation, the system shall:
1. Set status to "Pending"
2. Record requesting user ID
3. Record request date timestamp
4. Route to appropriate Store Manager based on project site location

**[REQ-APPROVAL-008]** The system shall notify the Store Manager of new pending approval requests via in-app notification.

### 3.13.2 Explosive Approval Workflow

**[REQ-APPROVAL-020]** The system shall support the following approval request statuses:
1. **Pending** - Awaiting Store Manager review
2. **Approved** - Approved by Store Manager
3. **Rejected** - Rejected by Store Manager
4. **Cancelled** - Cancelled by requester
5. **Expired** - Request expired due to passage of time

**[REQ-APPROVAL-021]** Store Managers shall be able to review pending approval requests with details:
- Project and site information
- Requested explosive quantities (from drill plan calculations)
- Approval type and priority
- Expected usage date
- Safety and environmental compliance status
- Requester information and notes

**[REQ-APPROVAL-022]** Store Managers shall be able to approve explosive usage requests.

**[REQ-APPROVAL-023]** Upon approval, the system shall:
1. Update request status to "Approved"
2. Record approving user ID (Store Manager)
3. Record approval date timestamp
4. Mark explosive quantities as "allocated" in store inventory
5. Notify the requesting Blasting Engineer via in-app notification

**[REQ-APPROVAL-024]** Store Managers shall be able to reject explosive usage requests.

**[REQ-APPROVAL-025]** Upon rejection, the system shall require a rejection reason.

**[REQ-APPROVAL-026]** Upon rejection, the system shall:
1. Update request status to "Rejected"
2. Record rejecting user ID
3. Record rejection date timestamp
4. Store rejection reason
5. Notify the requesting Blasting Engineer with justification

**[REQ-APPROVAL-027]** Blasting Engineers shall be able to cancel their pending approval requests.

**[REQ-APPROVAL-028]** The system should automatically expire approval requests if the expected usage date has passed and status is still "Pending".

### 3.13.3 Safety & Environmental Compliance

**[REQ-APPROVAL-040]** The system shall enforce that Safety Checklist must be completed before explosive usage request can be created.

**[REQ-APPROVAL-041]** The system shall enforce that Environmental Assessment must be completed before explosive usage request can be created.

**[REQ-APPROVAL-042]** The system should provide guidance or links to safety checklist requirements.

**[REQ-APPROVAL-043]** The system should provide guidance or links to environmental assessment requirements.

**[REQ-APPROVAL-044]** The system shall log all safety and environmental compliance data for audit purposes.

### 3.13.4 Approval Request Listing & Filtering

**[REQ-APPROVAL-050]** The system shall provide endpoints to retrieve approval requests with appropriate filtering.

**[REQ-APPROVAL-051]** The system shall support filtering approval requests by:
- Status (Pending, Approved, Rejected, Cancelled, Expired)
- Priority (Low, Normal, High, Critical)
- Approval Type
- Project Site ID
- Requesting user
- Date range

**[REQ-APPROVAL-052]** Blasting Engineers shall see approval requests they created.

**[REQ-APPROVAL-053]** Store Managers shall see approval requests routed to them based on site location.

**[REQ-APPROVAL-054]** Explosive Managers and Admins shall see all approval requests across all sites.

---

## 3.14 Explosive Calculation & Reporting

### 3.14.1 Explosive Quantity Calculation

**[REQ-CALC-001]** The system shall automatically calculate explosive requirements for each drill point based on:
- Hole depth
- Diameter
- Spacing
- Burden
- Rock volume to be fragmented
- Validated explosive formulas

**[REQ-CALC-002]** The system shall calculate ANFO (Ammonium Nitrate Fuel Oil) quantity required per drill point in kilograms.

**[REQ-CALC-003]** The system shall calculate Emulsion explosive quantity required per drill point in kilograms.

**[REQ-CALC-004]** The system shall store calculated explosive quantities with each drill point record.

**[REQ-CALC-005]** The system shall provide aggregated explosive calculations for entire project sites, including:
- Total ANFO required
- Total Emulsion required
- Total explosive weight
- Number of holes
- Total volume to be blasted

**[REQ-CALC-006]** Blasting Engineers shall be able to manually override automatically calculated explosive requirements.

**[REQ-CALC-007]** The system shall validate manually overridden quantities against acceptable ranges to prevent unsafe conditions.

**[REQ-CALC-008]** The system shall log all manual overrides with user ID and timestamp for audit purposes.

**[REQ-CALC-009]** The system shall automatically recalculate explosive requirements whenever relevant drill point parameters are changed.

### 3.14.2 Explosive Calculation Results Storage

**[REQ-CALC-020]** The system shall maintain explosive calculation result records with the following attributes:
- Calculation Result ID (unique, auto-generated)
- Project ID
- Project Site ID
- Total ANFO (kg)
- Total Emulsion (kg)
- Total Explosive Weight (kg)
- Number of Holes
- Total Volume (m³)
- Calculation Date (timestamp)
- Calculated By User ID
- Is Approved (boolean)

**[REQ-CALC-021]** The system shall provide endpoints to retrieve explosive calculation results for specific project sites.

**[REQ-CALC-022]** The system shall allow authorized users to approve explosive calculations, setting IsApproved flag to true.

### 3.14.3 Comparison: Planned vs. Actual

**[REQ-CALC-030]** The system shall provide a side-by-side comparison display of:
- Planned drilling pattern parameters
- Actual survey data parameters
- Planned explosive quantities
- Calculated explosive quantities based on actual data

**[REQ-CALC-031]** The comparison shall highlight variances between planned and actual values exceeding configurable thresholds.

**[REQ-CALC-032]** The system shall allow users to accept or adjust calculations based on the variance analysis.

---

## 3.15 Maintenance Management

### 3.15.1 Maintenance Record Creation

**[REQ-MAINT-001]** The system shall provide a Maintenance Management interface accessible to Mechanical Engineers and Machine Managers.

**[REQ-MAINT-002]** Mechanical Engineers and Machine Managers shall be able to create maintenance records with the following attributes:
- Maintenance Record ID (unique, auto-generated)
- Machine ID (mandatory, reference to machine being maintained)
- Machine Name, Model, Rig Number (auto-populated from machine)
- Type (mandatory): Preventive, Corrective, Emergency, Inspection
- Priority (mandatory): Low, Medium, High, Critical
- Status (mandatory): Scheduled, In-Progress, Completed, Overdue, Cancelled
- Description (mandatory, maintenance work description)
- Scheduled Date and Time (mandatory)
- Assigned Technician (mandatory, name or user reference)
- Estimated Duration (hours, mandatory)
- Actual Duration (hours, set upon completion)
- Estimated Cost (optional)
- Actual Cost (optional, set upon completion)
- Notes (optional, additional maintenance notes)
- Completion Notes (optional, recorded upon completion)
- Completed Date (timestamp, set when status changed to Completed)
- Created Date (timestamp, auto-generated)
- Created By User ID (auto-populated)

**[REQ-MAINT-003]** The system shall validate that the Machine ID exists and is valid before creating a maintenance record.

**[REQ-MAINT-004]** The system shall auto-populate machine details (name, model, rig number) from the selected machine.

**[REQ-MAINT-005]** The system shall initialize new maintenance records with status "Scheduled" by default.

### 3.15.2 Maintenance Status Management

**[REQ-MAINT-020]** The system shall support the following maintenance record statuses:
1. **Scheduled** - Maintenance is scheduled for future date
2. **In-Progress** - Maintenance work is currently being performed
3. **Completed** - Maintenance work has been successfully completed
4. **Overdue** - Scheduled maintenance date has passed without completion
5. **Cancelled** - Maintenance has been cancelled

**[REQ-MAINT-021]** Authorized users shall be able to update maintenance status.

**[REQ-MAINT-022]** The system shall automatically mark maintenance records as "Overdue" when scheduled date/time has passed and status is still "Scheduled".

**[REQ-MAINT-023]** When maintenance status is changed to "Completed", the system shall:
1. Record the completion date timestamp
2. Require completion notes to be entered
3. Require actual duration to be recorded
4. Optionally record actual cost

**[REQ-MAINT-024]** When maintenance status is changed to "In-Progress", the system shall:
1. Record the start timestamp
2. Update the machine status to "UnderMaintenance" if not already set

**[REQ-MAINT-025]** When maintenance status is changed to "Completed", the system should:
1. Update the machine's LastMaintenanceDate to current date
2. Calculate and update NextMaintenanceDate based on maintenance schedule
3. Update machine status back to "Available" if no other maintenance is in progress

### 3.15.3 Maintenance Tracking & Reporting

**[REQ-MAINT-030]** The system shall provide maintenance statistics dashboard showing:
- Total scheduled maintenance count
- In-progress maintenance count
- Overdue maintenance count
- Completed maintenance count

**[REQ-MAINT-031]** Operators shall be able to view maintenance records for their assigned machine.

**[REQ-MAINT-032]** Mechanical Engineers shall be able to view all maintenance records across all machines.

**[REQ-MAINT-033]** Machine Managers shall be able to view maintenance records for machines under their management.

**[REQ-MAINT-034]** The system shall provide an endpoint for operators to retrieve their assigned machine information.

**[REQ-MAINT-035]** The system shall provide an endpoint for operators to retrieve maintenance reports for their assigned machine.

**[REQ-MAINT-036]** Operators shall be able to submit maintenance reports for their assigned machine through a dedicated form.

### 3.15.4 Maintenance Record Updates & Deletion

**[REQ-MAINT-040]** Authorized users shall be able to edit maintenance records, including:
- Type, Priority, Status
- Description and Notes
- Scheduled Date and Time
- Assigned Technician
- Estimated/Actual Duration
- Estimated/Actual Cost
- Completion Notes

**[REQ-MAINT-041]** The system shall maintain audit trail of all modifications to maintenance records.

**[REQ-MAINT-042]** Authorized users shall be able to delete maintenance records.

**[REQ-MAINT-043]** The system shall display a confirmation dialog before deleting maintenance records.

### 3.15.5 Maintenance Search & Filtering

**[REQ-MAINT-050]** The Maintenance Management interface shall support advanced filtering by:
- Machine ID or Name
- Maintenance Type (Preventive, Corrective, Emergency, Inspection)
- Status (Scheduled, In-Progress, Completed, Overdue, Cancelled)
- Priority (Low, Medium, High, Critical)
- Date range (scheduled date, completed date)
- Assigned Technician

**[REQ-MAINT-051]** The system shall support search functionality for maintenance records by:
- Machine name
- Rig number
- Description
- Technician name

**[REQ-MAINT-052]** The system shall support pagination for maintenance record lists.

**[REQ-MAINT-053]** The system shall support sorting maintenance records by date, priority, status, and machine.

### 3.15.6 Maintenance Cost Tracking

**[REQ-MAINT-060]** The system shall track estimated and actual costs for maintenance activities.

**[REQ-MAINT-061]** The system shall calculate variance between estimated and actual costs.

**[REQ-MAINT-062]** The system shall provide cost summary reports showing:
- Total estimated costs
- Total actual costs
- Cost variance
- Cost breakdown by maintenance type
- Cost breakdown by machine

### 3.15.7 Maintenance Notifications & Alerts

**[REQ-MAINT-070]** The system should send notifications to assigned technicians when maintenance is scheduled.

**[REQ-MAINT-071]** The system should send alerts when maintenance becomes overdue.

**[REQ-MAINT-072]** The system should notify Machine Managers when maintenance is completed.

**[REQ-MAINT-073]** The system should alert when maintenance actual cost exceeds estimated cost by a configurable threshold.

---

## 3.16 Accessories Inventory Management

### 3.16.1 Accessory Creation & Configuration

**[REQ-ACCESS-001]** The system shall provide an Accessories Inventory Management interface accessible to Machine Managers, Mechanical Engineers, and Admins.

**[REQ-ACCESS-002]** Authorized users shall be able to add accessories to the inventory with the following attributes:
- Accessory ID (unique, auto-generated)
- Name (mandatory)
- Category (mandatory): Engine Parts, Hydraulic, Electrical, Filters, Belts & Hoses, Lubricants, Safety Equipment, Tools
- Part Number (optional, unique identifier)
- Description (optional)
- Quantity (mandatory, current stock quantity)
- Unit (mandatory): pcs (pieces), kg (kilograms), ltr (liters), m (meters), box, set
- Minimum Stock Level (mandatory, reorder threshold)
- Supplier (optional)
- Location (optional, storage location)
- Created At (timestamp, auto-generated)
- Last Updated (timestamp, auto-updated)

**[REQ-ACCESS-003]** The system shall validate that Part Number is unique across all accessories if provided.

**[REQ-ACCESS-004]** The system shall support the following accessory categories:
1. **Engine Parts** - Engine components and spare parts
2. **Hydraulic** - Hydraulic system components
3. **Electrical** - Electrical components and wiring
4. **Filters** - Oil filters, air filters, fuel filters
5. **Belts & Hoses** - Drive belts, hydraulic hoses
6. **Lubricants** - Oils, greases, lubricating fluids
7. **Safety Equipment** - Personal protective equipment and safety devices
8. **Tools** - Hand tools, specialized equipment

**[REQ-ACCESS-005]** The system shall support the following units of measurement:
- pcs (pieces) - for countable items
- kg (kilograms) - for weight-based items
- ltr (liters) - for liquid volumes
- m (meters) - for length-based items
- box - for boxed items
- set - for sets of items

### 3.16.2 Stock Level Management

**[REQ-ACCESS-020]** The system shall track current stock quantity for each accessory.

**[REQ-ACCESS-021]** The system shall calculate stock status based on current quantity:
- **Available** - Quantity > Minimum Stock Level
- **Low Stock** - 0 < Quantity <= Minimum Stock Level
- **Out of Stock** - Quantity = 0
- **Discontinued** - Item marked as discontinued (special flag)

**[REQ-ACCESS-022]** The system shall provide visual indicators for stock status:
- Green badge for Available
- Yellow/Orange badge for Low Stock
- Red badge for Out of Stock
- Gray badge for Discontinued

**[REQ-ACCESS-023]** The system shall display inventory statistics showing:
- Total Available items (quantity > minimum stock level)
- Low Stock items count (quantity <= minimum stock level && quantity > 0)
- Out of Stock items count (quantity = 0)

### 3.16.3 Stock Adjustment Operations

**[REQ-ACCESS-030]** The system shall provide stock adjustment functionality with the following operations:
1. **Add** - Increase stock quantity (receiving new items)
2. **Remove** - Decrease stock quantity (issuing/consuming items)
3. **Set** - Set absolute stock quantity (inventory correction)

**[REQ-ACCESS-031]** For "Add" operation, the system shall:
1. Display current quantity
2. Accept adjustment quantity (positive number)
3. Calculate new quantity = current quantity + adjustment quantity
4. Update stock quantity
5. Update Last Updated timestamp

**[REQ-ACCESS-032]** For "Remove" operation, the system shall:
1. Display current quantity
2. Accept adjustment quantity (positive number)
3. Validate that adjustment quantity <= current quantity
4. Calculate new quantity = current quantity - adjustment quantity
5. Update stock quantity
6. Update Last Updated timestamp

**[REQ-ACCESS-033]** For "Set" operation, the system shall:
1. Display current quantity
2. Accept new absolute quantity
3. Set stock quantity to new value
4. Update Last Updated timestamp

**[REQ-ACCESS-034]** The system shall prevent stock removal if the operation would result in negative quantity.

**[REQ-ACCESS-035]** The system shall display validation error if insufficient stock for removal operation.

**[REQ-ACCESS-036]** The system shall record all stock adjustments in an audit trail with:
- Adjustment type (Add, Remove, Set)
- Previous quantity
- New quantity
- Adjustment amount
- User who performed adjustment
- Timestamp
- Notes (optional)

### 3.16.4 Accessory Updates & Deletion

**[REQ-ACCESS-040]** Authorized users shall be able to edit existing accessory details, including:
- Name
- Category
- Part Number
- Description
- Unit
- Minimum Stock Level
- Supplier
- Location

**[REQ-ACCESS-041]** The system shall maintain unique constraint validation on Part Number during updates.

**[REQ-ACCESS-042]** Authorized users shall be able to delete accessories from inventory.

**[REQ-ACCESS-043]** The system shall display a confirmation dialog before deleting accessories.

**[REQ-ACCESS-044]** The system shall provide option to mark accessories as "Discontinued" instead of deletion to preserve historical data.

### 3.16.5 Accessories Search & Filtering

**[REQ-ACCESS-050]** The Accessories Inventory interface shall provide comprehensive search functionality.

**[REQ-ACCESS-051]** The system shall support search by:
- Name (partial match, case-insensitive)
- Part Number (partial match)
- Category
- Supplier
- Description (partial match)

**[REQ-ACCESS-052]** The system shall support filtering accessories by:
- Category (Engine Parts, Hydraulic, Electrical, Filters, Belts & Hoses, Lubricants, Safety Equipment, Tools)
- Status (Available, Low Stock, Out of Stock, Discontinued)
- Supplier
- Stock level range

**[REQ-ACCESS-053]** The system shall support combined filters (multiple filters applied simultaneously).

**[REQ-ACCESS-054]** The system shall support pagination for accessory lists with configurable page size (default: 25 items per page).

**[REQ-ACCESS-055]** The system shall support sorting accessories by:
- Name (alphabetical)
- Part Number
- Category
- Quantity
- Minimum Stock Level
- Last Updated date

### 3.16.6 Low Stock Alerts & Monitoring

**[REQ-ACCESS-060]** The system shall automatically identify accessories with low stock (quantity <= minimum stock level).

**[REQ-ACCESS-061]** The system shall display low stock warnings on the accessories list with visual indicators.

**[REQ-ACCESS-062]** The system should send notifications to Machine Managers when accessories reach low stock level.

**[REQ-ACCESS-063]** The system shall provide a dedicated "Low Stock Items" view showing all accessories requiring reorder.

**[REQ-ACCESS-064]** The system should send notifications when accessories become out of stock.

### 3.16.7 Accessories Reporting & Export

**[REQ-ACCESS-070]** The system shall provide accessories inventory reports showing:
- Complete inventory list with quantities
- Low stock items
- Out of stock items
- Stock value by category
- Items by supplier

**[REQ-ACCESS-071]** The system shall support exporting accessories data to CSV format.

**[REQ-ACCESS-072]** The CSV export shall include all accessory fields and current stock status.

**[REQ-ACCESS-073]** The system shall allow users to filter data before export.

### 3.16.8 Accessories Usage Tracking (Optional Enhancement)

**[REQ-ACCESS-080]** The system should track which accessories are used for which machines.

**[REQ-ACCESS-081]** The system should maintain usage history showing:
- Date of usage
- Machine used on
- Quantity used
- User who recorded usage
- Maintenance record reference (if applicable)

**[REQ-ACCESS-082]** The system should provide usage analytics showing:
- Most frequently used accessories
- Usage trends over time
- Cost analysis by accessory type

---

## 3.17 Analytics & Reporting

### 3.15.1 Blast Reports

**[REQ-REPORT-001]** The system shall generate comprehensive blast reports with the following sections:
- Project Information (name, region, area)
- Site Information (name, address, status)
- Drill Pattern Image (2D visualization)
- Survey Data Summary (tabular format with all drill points)
- Explosive Usage Summary (ANFO, Emulsion quantities)
- Volume Calculations (total rock volume)
- Blast Sequence Information (if available)
- Safety and Compliance Data

**[REQ-REPORT-002]** The system shall provide a preview interface for blast reports before export.

**[REQ-REPORT-003]** Blast reports shall be exportable in PDF format.

**[REQ-REPORT-004]** The system shall include timestamp and generated-by user information in all reports.

**[REQ-REPORT-005]** The system shall allow customization of report templates (optional enhancement).

### 3.15.2 Inventory Reports

**[REQ-REPORT-020]** The system shall generate store-wise inventory reports showing:
- Store details (name, location, manager, capacity)
- Current inventory levels by explosive type
- Reserved quantities
- Available quantities
- Utilization percentage
- Last restocked dates
- Expiry dates

**[REQ-REPORT-021]** The system shall generate central warehouse inventory reports showing:
- Total inventory by explosive type
- Inventory by status (Available, Allocated, Expired, etc.)
- Batches expiring within configurable time periods
- Low-stock alerts
- Quality check summaries

**[REQ-REPORT-022]** The system shall support exporting inventory reports to PDF format.

**[REQ-REPORT-023]** The system should support exporting inventory data to Excel format for further analysis.

### 3.15.3 Transfer Activity Reports

**[REQ-REPORT-030]** The system shall generate transfer activity reports showing:
- All transfer requests within a date range
- Transfer request status distribution
- Average approval time
- Average delivery time
- Transfer volumes by explosive type
- Store-wise transfer statistics

**[REQ-REPORT-031]** The system shall provide filtering options for transfer reports by status, date range, store, and explosive type.

### 3.15.4 Project Progress Reports

**[REQ-REPORT-040]** The system shall generate project progress reports showing:
- Project overview (name, region, status, dates)
- Number of sites and their statuses
- Completion percentages for each site
- Total drill points and completion status
- Machine assignments
- Operator assignments

**[REQ-REPORT-041]** The system shall provide visual indicators (charts, graphs) for project progress.

### 3.15.5 Machine Utilization Reports

**[REQ-REPORT-050]** The system should generate machine utilization reports showing:
- Machine inventory summary
- Status distribution (Available, In Use, Under Maintenance, Out of Service)
- Assignment history
- Maintenance schedules and completion
- Usage hours (if tracked)

### 3.15.6 Dashboard Analytics

**[REQ-REPORT-060]** The system shall provide role-specific dashboards with key performance indicators:

**Admin Dashboard:**
- Total projects (by status)
- Total sites (by status)
- Total users (by role)
- Total machines (by status)
- Total stores (by status)
- Pending approval requests

**Blasting Engineer Dashboard:**
- Assigned projects
- Sites in progress
- Pending explosive approvals
- Recent blast calculations

**Machine Manager Dashboard:**
- Total machines (by status)
- Pending maintenance schedules
- Machine assignment requests
- Utilization statistics

**Explosive Manager Dashboard:**
- Total inventory (central warehouse)
- Pending transfer requests
- Low-stock alerts
- Expiring inventory warnings
- Store utilization overview

**Store Manager Dashboard:**
- Store inventory levels
- Pending incoming transfers
- Pending explosive approval requests
- Utilization status

**[REQ-REPORT-061]** Dashboard data shall refresh in near real-time (configurable refresh interval).

---

## 3.16 System Administration

### 3.16.1 Role & Permission Management

**[REQ-ADMIN-001]** The system shall provide an administrative interface for managing roles and permissions.

**[REQ-ADMIN-002]** Admins shall be able to create custom roles (optional enhancement beyond predefined roles).

**[REQ-ADMIN-003]** Admins shall be able to assign permissions to roles at the module and operation level (Create, Read, Update, Delete).

**[REQ-ADMIN-004]** The system shall maintain a many-to-many relationship between roles and permissions.

**[REQ-ADMIN-005]** Changes to role permissions shall take effect immediately for all users with that role.

### 3.16.2 System Configuration

**[REQ-ADMIN-020]** The system shall provide configuration settings for:
- JWT token expiration time
- Password complexity requirements
- Account lockout threshold and duration
- Password reset code expiration time
- Session timeout duration
- Low-stock thresholds for explosive types
- Maintenance alert thresholds for machines
- Expiry warning periods for inventory

**[REQ-ADMIN-021]** Admins shall be able to update system configuration settings through a secure interface.

**[REQ-ADMIN-022]** Configuration changes shall be logged in an audit trail with timestamp and admin identity.

### 3.16.3 Audit Trail & Logging

**[REQ-ADMIN-030]** The system shall maintain comprehensive audit logs for:
- User authentication events (login, logout, failed attempts)
- User account modifications
- Role and permission changes
- Project and site creation, updates, deletions
- Drill data modifications
- Blast sequence modifications
- Machine assignments and status changes
- Inventory transactions
- Transfer request lifecycle events
- Approval request lifecycle events
- Configuration changes

**[REQ-ADMIN-031]** Each audit log entry shall include:
- Event type
- Timestamp
- User ID (who performed the action)
- Resource type and ID (what was affected)
- Action performed (Create, Read, Update, Delete)
- Before and after values (for updates)
- IP address (if applicable)

**[REQ-ADMIN-032]** Audit logs shall be retained for a configurable period (recommended: minimum 2 years).

**[REQ-ADMIN-033]** Admins shall be able to search and filter audit logs by event type, user, date range, and resource.

**[REQ-ADMIN-034]** The system shall provide the ability to export audit logs for compliance and investigation purposes.

### 3.16.4 System Health Monitoring

**[REQ-ADMIN-040]** The system should provide health monitoring dashboards showing:
- API response times
- Database query performance
- Error rates
- Active user sessions
- Resource utilization (CPU, memory, disk)

**[REQ-ADMIN-041]** The system should trigger alerts when performance metrics exceed configurable thresholds.

**[REQ-ADMIN-042]** The system shall log all unhandled exceptions with full stack traces for debugging.

### 3.16.5 Data Backup & Recovery

**[REQ-ADMIN-050]** The system should support automated database backups on a configurable schedule.

**[REQ-ADMIN-051]** Admins shall be able to initiate manual database backups.

**[REQ-ADMIN-052]** The system should provide functionality to restore from backup.

**[REQ-ADMIN-053]** Backup and restore operations shall be logged in the audit trail.

### 3.16.6 Data Export & Import

**[REQ-ADMIN-060]** Admins shall be able to export data in standard formats (CSV, JSON, Excel) for:
- User lists
- Project and site data
- Drill data
- Inventory data
- Transfer history

**[REQ-ADMIN-061]** The system should support bulk data import for initial system setup or migration.

**[REQ-ADMIN-062]** Data imports shall be validated before application to prevent data corruption.

---

## 4. NON-FUNCTIONAL REQUIREMENTS (SUMMARY)

### 4.1 Performance

**[REQ-PERF-001]** The system shall respond to API requests within 2 seconds for 95% of requests under normal load.

**[REQ-PERF-002]** The system shall support at least 100 concurrent users without performance degradation.

**[REQ-PERF-003]** The system shall use pagination for large data sets to optimize response times.

**[REQ-PERF-004]** The system shall implement caching for frequently accessed data (user lists, regions, etc.).

### 4.2 Security

**[REQ-SEC-001]** The system shall use HTTPS for all communications between client and server.

**[REQ-SEC-002]** The system shall hash all passwords using BCrypt with cryptographic salt before storage.

**[REQ-SEC-003]** The system shall never expose password hashes in API responses.

**[REQ-SEC-004]** The system shall implement JWT-based authentication with signature verification.

**[REQ-SEC-005]** The system shall enforce role-based access control on all protected endpoints.

**[REQ-SEC-006]** The system shall implement CORS (Cross-Origin Resource Sharing) with restricted allowed origins.

**[REQ-SEC-007]** The system shall validate and sanitize all user inputs to prevent injection attacks.

### 4.3 Reliability

**[REQ-REL-001]** The system shall have 99.5% uptime during business hours (8 AM - 8 PM local time).

**[REQ-REL-002]** The system shall implement global exception handling to prevent unhandled errors from crashing the application.

**[REQ-REL-003]** The system shall log all errors with sufficient context for debugging and resolution.

### 4.4 Scalability

**[REQ-SCALE-001]** The system architecture shall support horizontal scaling of the API tier.

**[REQ-SCALE-002]** The database schema shall be optimized with proper indexing for query performance.

**[REQ-SCALE-003]** The system shall support regional data partitioning for improved performance in multi-region deployments.

### 4.5 Usability

**[REQ-USE-001]** The user interface shall be responsive and accessible on desktop browsers (minimum resolution 1366x768).

**[REQ-USE-002]** The system shall provide clear error messages for validation failures.

**[REQ-USE-003]** The system shall display loading indicators during asynchronous operations.

**[REQ-USE-004]** The system shall provide tooltips and help text for complex features.

### 4.6 Compatibility

**[REQ-COMPAT-001]** The system shall be compatible with the following browsers:
- Google Chrome (latest 2 versions)
- Mozilla Firefox (latest 2 versions)
- Microsoft Edge (latest 2 versions)

**[REQ-COMPAT-002]** The backend API shall follow RESTful design principles.

**[REQ-COMPAT-003]** The system shall use JSON for all API request and response payloads.

---

## 5. GLOSSARY

| Term | Definition |
|------|------------|
| ANFO | Ammonium Nitrate Fuel Oil - A widely used bulk blasting explosive |
| Blast Sequence | The timed order in which drill holes are detonated |
| Burden | The distance from a drill hole to the nearest free face |
| Clean Architecture | Software architecture pattern with separation of concerns across layers |
| Detonator | A device used to trigger the detonation of explosives |
| Drill Point | A specific location where a hole is to be drilled |
| Emulsion | A water-in-oil emulsion explosive |
| JWT | JSON Web Token - A token-based authentication mechanism |
| RBAC | Role-Based Access Control |
| Spacing | The horizontal distance between adjacent drill holes |
| Stemming | Non-explosive material placed at the top of a charged drill hole |
| Subdrill | The depth of drilling below the designed floor elevation |

---

## 6. REVISION HISTORY

| Version | Date | Description | Author |
|---------|------|-------------|--------|
| 1.0 | Oct 13, 2025 | Initial draft based on requirements | Project Team |
| 2.0 | Oct 22, 2025 | Comprehensive rewrite based on actual implementation analysis | System Analyst |

---

## 7. APPROVAL

This Functional Requirements Specification is submitted for review and approval.

| Role | Name | Signature | Date |
|------|------|-----------|------|
| Product Owner | _______________ | _______________ | ________ |
| Technical Lead | _______________ | _______________ | ________ |
| Quality Assurance | _______________ | _______________ | ________ |
| Project Manager | _______________ | _______________ | ________ |

---

**END OF DOCUMENT**
