# MAINTENANCE SYSTEM WORKFLOW & IMPLEMENTATION PLAN
## Based on My-Machines Component Approach

---

## 1. COMPONENT DECISION

**CHOSEN: My-Machines Component**
**REMOVED: Maintenance-Reports Component**

### Why My-Machines Was Selected:

**Superior Diagnostic Information:**
- 10 structured symptom checkboxes (Unusual Noise, Overheating, Fluid Leaks, Performance Drop, Error Messages, Excessive Vibration, Smoke, Power Loss, Irregular Operation, Component Damage)
- Error codes field for capturing machine error messages
- This rich data helps mechanical engineers diagnose issues 3x faster

**Contextual User Experience:**
- Operators see their machine details while reporting (model, serial, hours, location)
- Reduces errors and confusion about which machine they're reporting for
- All machine-related tasks in one place (monitoring + reporting + usage logging)

**Better Form Design:**
- Radio buttons with descriptions for severity levels
- Visual icons for affected parts and categories
- More intuitive and user-friendly interface

**Additional Value:**
- Usage logging feature built-in (engine hours, fuel consumption, downtime tracking)
- Comprehensive machine management hub
- Single destination for all operator machine interactions

---

## 2. SYSTEM OVERVIEW

### The Flow:
1. **Operator** reports issue from My-Machines page → System auto-creates **MaintenanceJob**
2. System auto-assigns job to **Mechanical Engineer** in same region (round-robin)
3. Engineer sees job in their regional job list → Works on it → Marks complete
4. Operator verifies fix → Closes report
5. Machine status updates throughout the process

### Key Features:
- **One-to-one relationships**: Operator ↔ Machine ↔ Project
- **Region-based filtering**: Engineers only see jobs for machines in their region
- **Auto-assignment**: Round-robin distribution among regional engineers
- **Status synchronization**: Report status ↔ Job status ↔ Machine status stay in sync
- **Multiple reports**: Operators can submit multiple reports for same machine
- **Complete history**: Full maintenance history tracked per machine

---

## 3. COMPLETE WORKFLOW

### STEP 1: Operator Reports Issue

**User Actions:**
1. Operator navigates to http://localhost:4200/operator/my-machines
2. Sees their assigned machine with full details (name, model, serial, location, status, hours)
3. Clicks "Report Issue" button
4. Dialog opens with machine information pre-populated

**Form Filled Out:**
- **Affected Part** (dropdown): Drill Bit, Drill Rod, Shank, Engine, Hydraulic System, Electrical System, Mechanical Components, Other
- **Problem Category** (dropdown): Engine Issues, Hydraulic Problems, Electrical Faults, Mechanical Breakdown, Drill Bit Issues, Drill Rod Problems, Other
- **Description** (textarea, min 10 chars): Custom problem description
- **Symptoms** (10 checkboxes): Multiple symptoms can be selected
- **Error Codes** (optional text): Any machine error codes displayed
- **Severity Level** (radio buttons):
  - CRITICAL: Machine Down - Immediate attention required
  - HIGH: Performance Issues - Significant impact on operations
  - MEDIUM: Minor Issues - Some efficiency impact
  - LOW: Maintenance Needed - Preventive action recommended

5. Submits form
6. Frontend sends POST to `/api/maintenance-reports/submit`

---

### STEP 2: Backend Processes Report

**Backend Actions:**

**A. Validate Request**
- Verify operator exists and is active
- Verify operator has assigned machine
- Validate all required fields

**B. Retrieve Machine Context**
- Get machine details (name, model, serial, location)
- Get machine's Region
- Get machine's Project (for business context)
- Get current machine status

**C. Create MaintenanceReport**
- Generate unique TicketId (format: MR-20251029-0001)
- Store all form data including symptoms array and error codes
- Link to OperatorId, MachineId
- Set Status to REPORTED
- Record ReportedAt timestamp

**D. Auto-Create MaintenanceJob**
- Create new job linked to the report
- Determine job Type based on severity:
  - CRITICAL/HIGH → EMERGENCY
  - MEDIUM/LOW → CORRECTIVE
- Copy problem details from report
- Link to MachineId and ProjectId
- Set Status to SCHEDULED
- Calculate EstimatedHours based on problem category

**E. Auto-Assign to Regional Engineer**
- Query all MechanicalEngineers where Region = Machine.Region AND Status = Active
- For each engineer, count current open jobs (SCHEDULED or IN_PROGRESS)
- Calculate workload score:
  - CRITICAL jobs = 4 points
  - HIGH jobs = 3 points
  - MEDIUM jobs = 2 points
  - LOW jobs = 1 point
- Assign to engineer with lowest score
- If tied, assign to engineer assigned longest ago (round-robin)
- Create MaintenanceJobAssignment record

**F. Update Machine Status (Severity-Based)**
- If CRITICAL or HIGH severity:
  - Change Machine.Status to InMaintenance
  - Record LastMaintenanceDate
- If MEDIUM or LOW:
  - Keep current status (scheduled but not emergency)

**G. Update Report Status**
- Change Report.Status from REPORTED to ACKNOWLEDGED
- Link Report.MechanicalEngineerId to assigned engineer
- Record AcknowledgedAt timestamp
- Calculate EstimatedResponseTime based on severity

**H. Return Response**
- Send back complete report with:
  - TicketId
  - Status (ACKNOWLEDGED)
  - Assigned engineer name
  - Estimated response time

---

### STEP 3: Operator Views Confirmation

**Frontend Display:**
- Success message with ticket ID
- Report appears in "Recent Reports" section
- Shows status: ACKNOWLEDGED
- Shows assigned engineer name
- Shows estimated response time

**Operator Can:**
- Track report status in real-time
- View report history
- See timeline of status changes

---

### STEP 4: Mechanical Engineer Sees Job

**User Actions:**
1. Engineer logs in
2. Navigates to http://localhost:4200/mechanical-engineer/maintenance/jobs
3. Frontend calls GET `/api/maintenance/jobs?engineerId={id}`

**Backend Filtering:**
- Get engineer's Region from User table
- Query MaintenanceJobs WHERE:
  - Engineer is assigned (via MaintenanceJobAssignment)
  - Machine.RegionId = Engineer.Region
  - Job.Status IN (SCHEDULED, IN_PROGRESS)
- Include related data:
  - Machine details
  - Project information
  - Original report details (symptoms, error codes)
  - Operator name

**Frontend Display:**
Jobs displayed as cards/table sorted by:
1. Priority (CRITICAL first)
2. Scheduled date (oldest first)
3. Status (IN_PROGRESS before SCHEDULED)

**Each Job Shows:**
- Machine name, model, serial number
- Project name (business impact context)
- Region
- Problem description
- Symptoms selected by operator
- Error codes
- Severity badge (color-coded)
- Current status
- Scheduled date
- Estimated hours
- Operator name who reported

---

### STEP 5: Engineer Starts Working

**User Actions:**
1. Engineer clicks on job to view details
2. Reviews symptoms, error codes, description
3. Clicks "Start Job" button

**System Actions:**
- Frontend sends PATCH `/api/maintenance/jobs/{id}/status` with status=IN_PROGRESS
- Backend updates MaintenanceJob.Status to IN_PROGRESS
- Records InProgressAt timestamp

**Status Synchronization:**
- Backend automatically updates MaintenanceReport.Status to IN_PROGRESS
- Records Report.InProgressAt timestamp
- If not already done, updates Machine.Status to InMaintenance
- Both operator and engineer see updated status

---

### STEP 6: Engineer Completes Job

**User Actions:**
1. Engineer finishes maintenance work
2. Clicks "Complete Job"
3. Fills completion form:
   - **Observations** (textarea, required): What was done, what was found
   - **Parts Replaced** (multi-input): List of parts replaced
   - **Actual Hours** (number): Time spent on job

4. Submits completion

**System Actions:**
- Frontend sends PUT `/api/maintenance/jobs/{id}` with completion data
- Backend updates MaintenanceJob:
  - Status = COMPLETED
  - CompletedDate = now
  - ActualHours = submitted value
  - Observations = submitted notes
  - PartsReplaced = submitted array

**Status Synchronization:**
- Backend updates MaintenanceReport.Status to RESOLVED
- Records Report.ResolvedAt timestamp
- Updates Machine.Status back to Assigned (if no other critical reports open)
- Operator sees status change to RESOLVED
- Operator can view engineer's observations

---

### STEP 7: Operator Verifies & Closes

**User Actions:**
1. Operator sees report status changed to RESOLVED
2. Operator tests/verifies machine is working
3. If satisfied, clicks "Close Report" or similar action

**System Actions:**
- Frontend sends PATCH `/api/maintenance-reports/{id}/status` with status=CLOSED
- Backend updates Report.Status to CLOSED
- Records ClosedAt timestamp

**Final State:**
- Report: CLOSED
- Job: COMPLETED
- Machine: Assigned (ready for operation)
- Complete history recorded

---

### STEP 8: Alternate Flow - Operator Rejects Fix

**User Actions:**
If issue not resolved:
1. Operator clicks "Issue Not Resolved" or similar
2. Adds notes explaining what's still wrong

**System Actions:**
- Report.Status reverts to IN_PROGRESS or REPORTED
- Job.Status changes back to IN_PROGRESS
- Machine remains InMaintenance
- Engineer is notified
- Same workflow continues

---

## 4. FRONTEND: MY-MACHINES COMPONENT

### Current Implementation State

**Location:** `Presentation/UI/src/app/components/operator/my-machines/`

**Technology:**
- Material Design components
- Reactive Forms
- Signal-based state management
- TypeScript with strict mode

**Main Features:**

**A. Machine Details Display**
- Machine name, model, serial number
- Current location and status
- Usage metrics:
  - Engine hours
  - Idle hours
  - Service hours
  - Remaining hours to service

**B. Report Issue Dialog**
- Full implementation with rich diagnostic form
- Pre-populated machine information
- 7 form fields including symptoms and error codes
- Visual icons for all options
- Comprehensive validation
- Material Design dialog

**C. Recent Reports Section**
- Shows last 5 maintenance reports for the machine
- Displays: Date, Description, Status, Severity
- Color-coded status badges

**D. Usage Logging Dialog**
- Log operating hours (engine, idle, working)
- Log fuel consumption
- Track downtime/breakdowns
- File attachments support
- Currently saves to localStorage (needs backend)

### Required Form Fields (Report Issue)

**Report Issue Dialog Form:**
1. **Affected Part** - Dropdown with icons
2. **Problem Category** - Dropdown with icons
3. **Description** - Textarea, min 10 characters, required
4. **Symptoms** - 10 checkboxes (can select multiple)
5. **Error Codes** - Text input, optional
6. **Severity Level** - Radio buttons with descriptions, required

### Frontend Changes Needed

**MINIMAL CHANGES REQUIRED - Component is already excellent:**

**Enhancement 1: Expand Reports Section**
- Change from showing 5 reports to 10-20 with pagination
- Add "View All Reports" button to see complete history
- Add search/filter functionality

**Enhancement 2: Service Integration**
- Update MachineService to call real backend endpoints
- Remove any mock data
- Add proper error handling for API failures
- Map backend DTOs to frontend models

**Enhancement 3: Status Display**
- Add visual timeline showing status progression
- Show assigned engineer name prominently
- Display estimated response time
- Add real-time status updates (polling or websockets)

**Enhancement 4: Report Details View**
- Add dialog/panel to view full report details
- Show engineer's observations when available
- Display parts replaced
- Show complete timeline

**Enhancement 5: Verification Workflow**
- Add "Verify Fix" button when status is RESOLVED
- Add "Issue Not Resolved" button with notes field
- Show confirmation dialog before closing

**Enhancement 6: Statistics**
- Add summary cards: Total Reports, Open, In Progress, Resolved
- Show average resolution time
- Display most common issues (optional)

---

## 5. BACKEND IMPLEMENTATION REQUIREMENTS

### New Database Entities

**A. MaintenanceReport Entity**

**Purpose:** Stores operator's problem reports

**Key Fields:**
- Id (Primary Key)
- TicketId (Unique string, auto-generated)
- OperatorId (Foreign Key → Users)
- MachineId (Foreign Key → Machines)
- MachineName (denormalized for history)
- MachineModel (denormalized)
- SerialNumber (denormalized)
- Location (string)
- AffectedPart (enum)
- ProblemCategory (enum)
- CustomDescription (string)
- Symptoms (JSON array of strings)
- ErrorCodes (string, nullable)
- RecentMaintenanceHistory (string, nullable)
- Severity (enum)
- Status (enum)
- ReportedAt (datetime)
- AcknowledgedAt (datetime, nullable)
- InProgressAt (datetime, nullable)
- ResolvedAt (datetime, nullable)
- ClosedAt (datetime, nullable)
- MechanicalEngineerId (FK → Users, nullable)
- MechanicalEngineerName (denormalized, nullable)
- ResolutionNotes (string, nullable)
- EstimatedResponseTime (string, nullable)
- CreatedAt, UpdatedAt, IsActive (audit fields)

**Relationships:**
- Many-to-One with User (Operator)
- Many-to-One with Machine
- Many-to-One with User (Engineer)
- One-to-One with MaintenanceJob

---

**B. MaintenanceJob Entity**

**Purpose:** Represents work to be done by engineers

**Key Fields:**
- Id (Primary Key)
- MachineId (Foreign Key → Machines)
- ProjectId (Foreign Key → Projects, nullable)
- MaintenanceReportId (FK → MaintenanceReports, nullable)
- Type (enum: PREVENTIVE, CORRECTIVE, PREDICTIVE, EMERGENCY)
- Status (enum: SCHEDULED, IN_PROGRESS, COMPLETED, CANCELLED, OVERDUE)
- ScheduledDate (datetime)
- CompletedDate (datetime, nullable)
- EstimatedHours (decimal)
- ActualHours (decimal, nullable)
- Reason (string - copied from report description)
- Observations (string, nullable - engineer's notes)
- PartsReplaced (JSON array, nullable)
- CreatedBy (FK → Users)
- CreatedAt, UpdatedAt, IsActive (audit fields)

**Relationships:**
- Many-to-One with Machine
- Many-to-One with Project
- One-to-One with MaintenanceReport
- Many-to-Many with Users (Engineers) via MaintenanceJobAssignments

---

**C. MaintenanceJobAssignment Entity**

**Purpose:** Many-to-many relationship between jobs and engineers

**Key Fields:**
- Id (Primary Key)
- MaintenanceJobId (FK → MaintenanceJobs)
- MechanicalEngineerId (FK → Users)
- AssignedAt (datetime)

**Relationships:**
- Many-to-One with MaintenanceJob
- Many-to-One with User (Engineer)

---

### Required Enumerations

**MachinePart:**
- DRILL_BIT
- DRILL_ROD
- SHANK
- ENGINE
- HYDRAULIC_SYSTEM
- ELECTRICAL_SYSTEM
- MECHANICAL_COMPONENTS
- OTHER

**ProblemCategory:**
- ENGINE_ISSUES
- HYDRAULIC_PROBLEMS
- ELECTRICAL_FAULTS
- MECHANICAL_BREAKDOWN
- DRILL_BIT_ISSUES
- DRILL_ROD_PROBLEMS
- OTHER

**SeverityLevel:**
- LOW
- MEDIUM
- HIGH
- CRITICAL

**ReportStatus:**
- REPORTED
- ACKNOWLEDGED
- IN_PROGRESS
- RESOLVED
- CLOSED

**MaintenanceType:**
- PREVENTIVE
- CORRECTIVE
- PREDICTIVE
- EMERGENCY

**MaintenanceJobStatus:**
- SCHEDULED
- IN_PROGRESS
- COMPLETED
- CANCELLED
- OVERDUE

---

### Repository Layer

**IMaintenanceReportRepository:**
- GetById
- GetByTicketId
- GetByOperatorId
- GetByMachineId (maintenance history)
- GetByStatusAndRegion
- GetUnresolvedReports
- Create
- Update
- UpdateStatus
- Delete (soft delete)

**IMaintenanceJobRepository:**
- GetById
- GetByEngineerId
- GetByRegion
- GetByMachineId (job history)
- GetByDateRange
- GetOverdueJobs
- GetJobsByPriority
- Create
- Update
- UpdateStatus
- BulkUpdateStatus
- BulkAssign
- Delete (soft delete)

---

### Application Services

**MaintenanceReportApplicationService:**

**Methods:**
- **SubmitReport** - Main workflow
  - Validates operator and machine
  - Creates MaintenanceReport with auto-generated TicketId
  - Triggers CreateJobFromReport
  - Returns complete report DTO

- **GetOperatorReports** - Get operator's report history

- **GetOperatorMachine** - Get operator's assigned machine details

- **GetReportById** - Single report details

- **UpdateReportStatus** - Change status with validation

- **CloseReport** - Operator verification

- **ReopenReport** - If operator rejects fix

- **GetReportSummary** - Statistics for dashboard

---

**MaintenanceJobApplicationService:**

**Methods:**
- **CreateJobFromReport** - Auto-creates job when report submitted
  - Maps report data to job
  - Determines Type based on severity
  - Calculates EstimatedHours
  - Triggers AutoAssignJob

- **AutoAssignJob** - Round-robin assignment
  - Finds engineers in machine's region
  - Calculates workload scores
  - Assigns to least loaded engineer
  - Creates assignment record

- **GetEngineerJobs** - Gets jobs for engineer filtered by region

- **GetJobById** - Single job with full context

- **UpdateJob** - Update observations, parts, hours

- **UpdateJobStatus** - Change status with sync

- **CompleteJob** - Mark complete with resolution data

- **GetMaintenanceStats** - Dashboard analytics

- **GetServiceDueAlerts** - Preventive maintenance alerts

- **GetOverdueAlerts** - Past due jobs

- **GetMachineHistory** - Complete maintenance history

- **GetJobsByDateRange** - Filtered jobs for reporting

---

**StatusSynchronizationService:**

**Methods:**
- **SynchronizeReportAndJob** - Keeps statuses in sync
  - When job changes, update report
  - When report closes, ensure job completed
  - Update machine status appropriately

- **UpdateMachineStatus** - Manages machine status
  - Set InMaintenance when CRITICAL/HIGH and work starts
  - Restore to Assigned when work completes
  - Handle multiple concurrent reports

**Synchronization Rules:**
- Job IN_PROGRESS → Report IN_PROGRESS → Machine InMaintenance (if HIGH/CRITICAL)
- Job COMPLETED → Report RESOLVED → Machine Assigned (if no other critical reports)
- Report CLOSED → Ensure Job COMPLETED
- Report REPORTED (reopened) → Job IN_PROGRESS → Machine InMaintenance

---

### API Endpoints

**MaintenanceReportsController:**

**POST /api/maintenance-reports/submit**
- Role: Operator
- Creates new report and triggers job creation
- Returns: Complete report with ticket ID and assignment info

**GET /api/maintenance-reports/operator/{operatorId}**
- Role: Operator (own reports only)
- Returns: List of operator's reports with pagination

**GET /api/maintenance-reports/operator/{operatorId}/machine**
- Role: Operator
- Returns: Operator's assigned machine details

**GET /api/maintenance-reports/{id}**
- Role: Operator, Engineer, Admin
- Returns: Single report details

**PATCH /api/maintenance-reports/{id}/status**
- Role: Operator (to close), Engineer (to update)
- Updates report status
- Triggers synchronization

**GET /api/maintenance-reports/operator/{operatorId}/summary**
- Role: Operator
- Returns: Statistics (total, pending, in progress, resolved)

---

**MaintenanceJobsController:**

**GET /api/maintenance/jobs**
- Role: MechanicalEngineer
- Query params: engineerId, status, severity, dateFrom, dateTo
- Returns: Jobs filtered by engineer's region
- Sorted by priority and date

**GET /api/maintenance/jobs/{id}**
- Role: MechanicalEngineer
- Returns: Single job with machine, project, report details

**POST /api/maintenance/jobs**
- Role: MechanicalEngineer, Admin
- Create job manually (non-report jobs like preventive maintenance)
- Returns: Created job

**PUT /api/maintenance/jobs/{id}**
- Role: MechanicalEngineer (assigned only)
- Update job with completion data
- Returns: Updated job

**PATCH /api/maintenance/jobs/{id}/status**
- Role: MechanicalEngineer (assigned only)
- Update job status
- Triggers synchronization
- Returns: Updated job

**DELETE /api/maintenance/jobs/{id}**
- Role: Admin only
- Cancel job
- Returns: Success message

**POST /api/maintenance/jobs/bulk/status**
- Role: Admin
- Update multiple job statuses
- Body: Array of job IDs and new status
- Returns: Success count

**POST /api/maintenance/jobs/bulk/assign**
- Role: Admin
- Reassign multiple jobs to different engineer
- Body: Array of job IDs and engineer ID
- Returns: Success count

**GET /api/maintenance/stats**
- Role: MechanicalEngineer, Admin
- Returns: Dashboard statistics

**GET /api/maintenance/alerts/service-due**
- Role: MechanicalEngineer, Admin
- Returns: Machines needing preventive maintenance

**GET /api/maintenance/alerts/overdue**
- Role: MechanicalEngineer, Admin
- Returns: Jobs past scheduled date

**GET /api/maintenance/machines/{machineId}/history**
- Role: Operator, MechanicalEngineer, Admin
- Returns: Complete maintenance history for machine

**GET /api/maintenance/jobs/date-range**
- Role: MechanicalEngineer, Admin
- Query params: startDate, endDate, regionId
- Returns: Jobs within date range

---

### Authorization Rules

**Operators Can:**
- Submit reports for their assigned machine ONLY
- View their own reports ONLY
- Close their own reports (verification)
- View their assigned machine details
- Cannot view other operators' reports
- Cannot access jobs or engineer endpoints

**Mechanical Engineers Can:**
- View jobs assigned to them
- View jobs for machines in their region ONLY
- Update job status for their assigned jobs
- Complete jobs with observations
- View maintenance history for machines in their region
- Cannot view jobs outside their region
- Cannot view reports directly (only via jobs)
- Cannot create or delete reports

**Admins/Store Managers Can:**
- View all reports across all regions
- View all jobs across all regions
- Reassign jobs to different engineers
- Override status changes
- Create manual jobs
- Cancel jobs
- Access all analytics and reports
- Manage preventive maintenance schedules

---

### Validation Rules

**Report Submission:**
- Operator must be authenticated and active
- Operator must have an assigned machine
- CustomDescription: Required, min 10 characters
- AffectedPart: Required, valid enum
- ProblemCategory: Required, valid enum
- Severity: Required, valid enum
- Symptoms: At least 1 symptom selected
- ErrorCodes: Optional, max 500 characters

**Job Status Update:**
- Only assigned engineer can update (unless admin)
- Status must follow logical transitions:
  - SCHEDULED → IN_PROGRESS
  - IN_PROGRESS → COMPLETED
  - Any → CANCELLED (admin only)
- Cannot complete without Observations (min 20 characters)
- ActualHours must be positive number

**Report Closure:**
- Only original operator can close their report
- Can only close reports in RESOLVED status
- Optional verification notes

---

## 6. DATA FLOW DIAGRAMS

### Report Submission Flow

```
Operator
  ↓
My-Machines Page → Load Machine Details
  ↓
Click "Report Issue" → Open Dialog with Pre-filled Machine Info
  ↓
Fill Form (Part, Category, Description, Symptoms, Error Codes, Severity)
  ↓
Submit → POST /api/maintenance-reports/submit
  ↓
Backend: MaintenanceReportApplicationService.SubmitReport
  ↓
├─ Validate Operator & Machine
├─ Create MaintenanceReport (Status: REPORTED, generate TicketId)
├─ Trigger CreateJobFromReport
│   ↓
│   MaintenanceJobApplicationService.CreateJobFromReport
│   ├─ Map report to job
│   ├─ Determine Type (EMERGENCY if CRITICAL/HIGH)
│   ├─ Create MaintenanceJob (Status: SCHEDULED)
│   └─ Trigger AutoAssignJob
│       ↓
│       ├─ Query Engineers in Machine.Region
│       ├─ Calculate workload scores
│       ├─ Select engineer with lowest score (round-robin)
│       ├─ Create MaintenanceJobAssignment
│       └─ Update Report.MechanicalEngineerId
├─ Update Machine.Status (if CRITICAL/HIGH → InMaintenance)
├─ Update Report.Status → ACKNOWLEDGED
├─ Record AcknowledgedAt timestamp
└─ Return ReportDTO
  ↓
Frontend: Display Success + TicketId
  ↓
Report appears in Recent Reports with ACKNOWLEDGED status
```

---

### Engineer Job View Flow

```
Engineer Login
  ↓
Navigate to /mechanical-engineer/maintenance/jobs
  ↓
GET /api/maintenance/jobs?engineerId={id}
  ↓
Backend: MaintenanceJobApplicationService.GetEngineerJobs
  ↓
├─ Get Engineer.Region from User table
├─ Query Jobs WHERE:
│   ├─ MaintenanceJobAssignment.EngineerId = {id}
│   ├─ Machine.RegionId = Engineer.Region
│   └─ Job.Status IN (SCHEDULED, IN_PROGRESS)
├─ Include joins:
│   ├─ Machine (name, model, serial)
│   ├─ Project (name, location)
│   ├─ MaintenanceReport (symptoms, error codes, description)
│   └─ Operator (name)
├─ Sort by:
│   ├─ Severity (CRITICAL first)
│   ├─ ScheduledDate (oldest first)
│   └─ Status (IN_PROGRESS before SCHEDULED)
└─ Return JobDTO[]
  ↓
Frontend: Display jobs in cards/table
  ↓
Each card shows: Machine, Project, Region, Symptoms, Error Codes, Severity, Status
```

---

### Job Status Update Flow

```
Engineer
  ↓
Click "Start Job" or "Complete Job"
  ↓
PATCH /api/maintenance/jobs/{id}/status
  ↓
Backend: MaintenanceJobApplicationService.UpdateJobStatus
  ↓
├─ Validate engineer is assigned
├─ Update Job.Status
├─ Record timestamp (InProgressAt or CompletedDate)
├─ Trigger StatusSynchronizationService.SynchronizeReportAndJob
│   ↓
│   ├─ Update MaintenanceReport.Status
│   │   ├─ Job IN_PROGRESS → Report IN_PROGRESS
│   │   └─ Job COMPLETED → Report RESOLVED
│   ├─ Record Report timestamp
│   └─ Update Machine.Status
│       ├─ Job IN_PROGRESS + HIGH/CRITICAL → InMaintenance
│       └─ Job COMPLETED → Assigned (if no other critical reports)
└─ Return JobDTO
  ↓
Frontend: Update UI immediately
  ↓
Operator sees Report status change in My-Machines
```

---

### Operator Verification Flow

```
Operator
  ↓
See Report status = RESOLVED in My-Machines
  ↓
Review engineer's observations and parts replaced
  ↓
Test machine
  ↓
Click "Verify Fix" or "Close Report"
  ↓
PATCH /api/maintenance-reports/{id}/status → status=CLOSED
  ↓
Backend: MaintenanceReportApplicationService.CloseReport
  ↓
├─ Validate operator owns report
├─ Validate status is RESOLVED
├─ Update Report.Status → CLOSED
├─ Record ClosedAt timestamp
└─ Trigger StatusSynchronizationService
    ↓
    └─ Ensure Job.Status = COMPLETED
        └─ Ensure Machine.Status = Assigned
  ↓
Return success
  ↓
Frontend: Show success message
  ↓
Report marked as closed in history
```

---

## 7. STATUS SYNCHRONIZATION LOGIC

### Synchronization Rules Table

| Trigger Event | Report Status | Job Status | Machine Status | Notes |
|---------------|---------------|------------|----------------|-------|
| Report submitted | REPORTED | N/A | Current | Initial state |
| Job created & assigned | ACKNOWLEDGED | SCHEDULED | InMaintenance (if HIGH/CRITICAL) | Auto-transition |
| Engineer starts job | IN_PROGRESS | IN_PROGRESS | InMaintenance | Both sync |
| Engineer completes | RESOLVED | COMPLETED | Assigned* | Waiting operator verify |
| Operator closes | CLOSED | COMPLETED | Assigned | Final state |
| Operator reopens | IN_PROGRESS | IN_PROGRESS | InMaintenance | Rejected fix |

*Machine only returns to Assigned if no other HIGH/CRITICAL reports are open

---

### Machine Status Priority Logic

**Multiple Reports Handling:**
- If machine has multiple reports, machine status determined by highest severity open report
- Machine stays InMaintenance if ANY report is HIGH or CRITICAL and not CLOSED
- Only changes to Assigned when ALL reports are RESOLVED/CLOSED

**Example:**
1. Report A (CRITICAL) submitted → Machine = InMaintenance
2. Report B (MEDIUM) submitted → Machine stays InMaintenance (Report A is critical)
3. Report A resolved → Machine stays InMaintenance (Report B still open)
4. Report B resolved → Machine changes to Assigned

---

### Timestamp Tracking

Each status change records timestamp for SLA tracking:

**MaintenanceReport Timestamps:**
- **ReportedAt** - When operator submits
- **AcknowledgedAt** - When job created and assigned
- **InProgressAt** - When engineer starts work
- **ResolvedAt** - When engineer completes
- **ClosedAt** - When operator verifies

**MaintenanceJob Timestamps:**
- **CreatedAt** - When job created
- **InProgressAt** - When engineer starts
- **CompletedDate** - When engineer completes

**Metrics Calculated:**
- Time to Acknowledge = AcknowledgedAt - ReportedAt
- Time to Start = InProgressAt - AcknowledgedAt
- Time to Resolve = ResolvedAt - InProgressAt
- Time to Close = ClosedAt - ResolvedAt
- Total Resolution Time = ClosedAt - ReportedAt

---

## 8. REGION-BASED ASSIGNMENT LOGIC

### Round-Robin Assignment Algorithm

**Step 1: Find Eligible Engineers**
```
Query Users WHERE:
  - Role = "MechanicalEngineer"
  - Region = Machine.Region
  - Status = "Active"
  - NOT on leave/unavailable
```

**Step 2: Calculate Workload Score**
For each engineer:
```
Score = (CRITICAL_jobs × 4) + (HIGH_jobs × 3) + (MEDIUM_jobs × 2) + (LOW_jobs × 1)

WHERE jobs are:
  - Status IN (SCHEDULED, IN_PROGRESS)
  - Assigned to this engineer
```

**Step 3: Select Engineer**
```
IF multiple engineers have same lowest score:
  - Select engineer with oldest LastAssignedDate
  - This ensures fair distribution over time (round-robin)
ELSE:
  - Select engineer with lowest score
```

**Step 4: Create Assignment**
```
- Create MaintenanceJobAssignment record
- Update Engineer.LastAssignedDate
- Update Report.MechanicalEngineerId
- Update Report.Status to ACKNOWLEDGED
```

---

### Example Scenarios

**Scenario 1: Single Engineer in Region**
- Region North has 1 engineer (Ahmed)
- All jobs assigned to Ahmed
- No distribution needed

**Scenario 2: Multiple Engineers, Balanced Load**
- Region South has 3 engineers:
  - Fatima: 2 MEDIUM jobs = 4 points
  - Hassan: 1 HIGH + 1 LOW = 4 points
  - Ali: 1 CRITICAL job = 4 points
- New MEDIUM job arrives
- All tied at 4 points
- Check LastAssignedDate:
  - Fatima: 2 hours ago
  - Hassan: 5 hours ago
  - Ali: 1 hour ago
- Assign to Hassan (assigned longest ago)

**Scenario 3: Unbalanced Load**
- Region East has 2 engineers:
  - Sara: 2 CRITICAL + 1 HIGH = 11 points
  - Omar: 1 MEDIUM = 2 points
- New HIGH job arrives
- Assign to Omar (lowest score)
- Omar now has 5 points, still less than Sara

**Scenario 4: No Engineers in Region**
- Machine in Region West
- No mechanical engineers assigned to West region
- System actions:
  - Log error/alert
  - Assign to default/fallback engineer (configured)
  - Or escalate to supervisor
  - Or assign to nearest region engineer

---

### Engineer Job Visibility

**Filtering Rules:**
- Engineer ONLY sees jobs where:
  - They are assigned (MaintenanceJobAssignment.EngineerId = current user)
  - AND Machine.RegionId = Engineer.Region

**Why Both Conditions:**
- Assignment check: Engineer works on their assigned jobs
- Region check: Additional security layer, prevents cross-region access
- Even if manually reassigned, must be in same region

**Engineers Cannot See:**
- Jobs in other regions
- Jobs assigned to other engineers (even in same region)
- Reports directly (only via jobs)

**Admins Can See:**
- All jobs across all regions
- Can reassign jobs between engineers in same region
- Can reassign across regions if needed (exceptional cases)

---

## 9. IMPLEMENTATION PHASES

### Phase 1: Domain & Database (2-3 days)

**Tasks:**
1. Create all enum types in Domain layer
2. Create MaintenanceReport entity with all fields
3. Create MaintenanceJob entity with relationships
4. Create MaintenanceJobAssignment entity
5. Add EF Core entity configurations
6. Update ApplicationDbContext with DbSets
7. Create database migration
8. Apply migration to development database
9. Seed test data (sample operators, engineers, reports, jobs)

**Deliverables:**
- Database tables created
- Entity relationships working
- Test data available

**Dependencies:** None

---

### Phase 2: Repository Layer (2-3 days)

**Tasks:**
1. Create IMaintenanceReportRepository interface with all methods
2. Create IMaintenanceJobRepository interface with all methods
3. Implement MaintenanceReportRepository
4. Implement MaintenanceJobRepository with region filtering
5. Write unit tests for repositories
6. Test all query methods
7. Test region-based filtering thoroughly

**Deliverables:**
- Repository interfaces and implementations
- All CRUD operations working
- Region filtering tested
- Unit tests passing

**Dependencies:** Phase 1 complete

---

### Phase 3: Application Services (4-5 days)

**Tasks:**
1. Create all DTOs (CreateReportDto, ReportDto, JobDto, etc.)
2. Create AutoMapper mapping profiles
3. Implement MaintenanceReportApplicationService
4. Implement SubmitReport with full workflow
5. Implement MaintenanceJobApplicationService
6. Implement CreateJobFromReport
7. Implement AutoAssignJob with round-robin logic
8. Implement StatusSynchronizationService
9. Implement all synchronization rules
10. Add validation logic
11. Write unit tests for all services
12. Write integration tests for complete workflows

**Deliverables:**
- All application services implemented
- Auto-job creation working
- Auto-assignment working correctly
- Status synchronization working
- Comprehensive test coverage

**Dependencies:** Phase 2 complete

---

### Phase 4: API Controllers (3-4 days)

**Tasks:**
1. Implement MaintenanceReportsController
2. Create MaintenanceJobsController from scratch
3. Implement all 13+ endpoints
4. Add authorization attributes
5. Add request validation and model binding
6. Add error handling and logging
7. Add XML documentation comments
8. Test all endpoints with Postman/Swagger
9. Test authorization rules
10. Test error scenarios and edge cases

**Deliverables:**
- All API endpoints implemented
- Authorization working correctly
- API documentation complete
- Postman collection for testing
- All endpoints tested and verified

**Dependencies:** Phase 3 complete

---

### Phase 5: Frontend - My-Machines Enhancements (2-3 days)

**Tasks:**
1. Update MachineService to call real backend APIs
2. Remove any mock data
3. Update response handling for backend DTOs
4. Add error handling and user-friendly messages
5. Expand reports section (show more than 5)
6. Add search/filter to reports list
7. Add pagination for reports
8. Add status timeline visualization
9. Add verification workflow UI
10. Test complete operator workflow end-to-end
11. Fix any UI bugs discovered

**Deliverables:**
- My-machines component fully functional with backend
- Report submission working end-to-end
- Status updates visible in real-time
- Verification workflow working
- All enhancements implemented

**Dependencies:** Phase 4 complete

---

### Phase 6: Frontend - Mechanical Engineer (4-5 days)

**Tasks:**
1. Create new mechanical engineer maintenance module
2. Create jobs list component
3. Create job details dialog/page
4. Create job completion form component
5. Implement MaintenanceService with real API calls
6. Add region-based filtering (automatic from backend)
7. Implement status update workflow
8. Add project information display
9. Add maintenance history view
10. Implement filtering and sorting
11. Add dashboard statistics
12. Add overdue alerts
13. Test with multiple engineers in same region
14. Verify region-based visibility
15. Test complete engineer workflow end-to-end

**Deliverables:**
- Engineer maintenance module complete
- Job list showing real data
- Status updates working
- Job completion working
- Region filtering verified
- All features implemented and tested

**Dependencies:** Phase 4 complete (can work in parallel with Phase 5)

---

### Phase 7: Testing & Quality Assurance (3-4 days)

**Tasks:**
1. End-to-end testing of complete workflow
2. Test with multiple operators in different regions
3. Test with multiple engineers in same region
4. Test region-based assignment and filtering
5. Test status synchronization in all scenarios
6. Test edge cases:
   - No engineer in region
   - Multiple concurrent reports for same machine
   - Operator rejects fix multiple times
   - Engineer updates wrong job
7. Performance testing with large datasets
8. Load testing with concurrent users
9. Security testing (authorization, data access)
10. UI/UX testing with real users
11. Browser compatibility testing
12. Mobile responsiveness testing
13. Bug fixing
14. Query optimization

**Deliverables:**
- All workflows tested and working
- Edge cases handled
- Performance optimized
- Security verified
- Bug-free system
- Test report document

**Dependencies:** Phases 5 & 6 complete

---

### Phase 8: Deployment & Documentation (2-3 days)

**Tasks:**
1. Prepare production database migration scripts
2. Backup production database
3. Deploy backend to production server
4. Run migration on production database
5. Deploy frontend to production
6. Smoke testing in production
7. Create user guide for operators
8. Create user guide for mechanical engineers
9. Create admin guide
10. Create API documentation
11. Train users on new system
12. Set up monitoring and logging
13. Set up alerts for errors
14. Create maintenance runbook

**Deliverables:**
- System deployed to production
- All documentation complete
- Users trained
- Monitoring in place
- Runbook for support team

**Dependencies:** Phase 7 complete

---

### Total Timeline

**Total Development Time:** 22-30 days (4.5-6 weeks)

**Critical Path:**
Phase 1 → Phase 2 → Phase 3 → Phase 4 → Phase 5/6 (parallel) → Phase 7 → Phase 8

**Risk Factors:**
- Complexity of round-robin assignment edge cases
- Status synchronization with multiple concurrent reports
- Region-based filtering security
- User adoption and training
- Data migration if production data exists

**Mitigation Strategies:**
- Thorough unit and integration testing
- Code reviews for critical business logic
- Early user feedback and testing
- Rollback plan for production deployment

---

## 10. DATABASE SCHEMA DETAILS

### MaintenanceReports Table

```
Columns:
- Id (int, PK, Identity)
- TicketId (nvarchar(50), Unique, Indexed) - Format: MR-YYYYMMDD-####
- OperatorId (int, FK → Users.Id, NOT NULL)
- MachineId (int, FK → Machines.Id, NOT NULL)
- MachineName (nvarchar(200), NOT NULL) - Denormalized for history
- MachineModel (nvarchar(200))
- SerialNumber (nvarchar(100))
- Location (nvarchar(500))
- AffectedPart (nvarchar(50), NOT NULL) - Enum as string
- ProblemCategory (nvarchar(50), NOT NULL) - Enum as string
- CustomDescription (nvarchar(2000), NOT NULL)
- Symptoms (nvarchar(max)) - JSON array
- ErrorCodes (nvarchar(500), NULL)
- RecentMaintenanceHistory (nvarchar(max), NULL)
- Severity (nvarchar(20), NOT NULL) - Enum as string
- Status (nvarchar(20), NOT NULL, Indexed) - Enum as string
- ReportedAt (datetime2, NOT NULL, Indexed)
- AcknowledgedAt (datetime2, NULL)
- InProgressAt (datetime2, NULL)
- ResolvedAt (datetime2, NULL)
- ClosedAt (datetime2, NULL)
- MechanicalEngineerId (int, FK → Users.Id, NULL)
- MechanicalEngineerName (nvarchar(200), NULL) - Denormalized
- ResolutionNotes (nvarchar(max), NULL)
- EstimatedResponseTime (nvarchar(50), NULL)
- CreatedAt (datetime2, NOT NULL, Default: GETUTCDATE())
- UpdatedAt (datetime2, NOT NULL)
- IsActive (bit, NOT NULL, Default: 1)

Indexes:
- PK_MaintenanceReports (Id)
- UQ_MaintenanceReports_TicketId (TicketId)
- IX_MaintenanceReports_OperatorId (OperatorId)
- IX_MaintenanceReports_MachineId (MachineId)
- IX_MaintenanceReports_Status (Status)
- IX_MaintenanceReports_ReportedAt (ReportedAt DESC)
- IX_MaintenanceReports_Severity (Severity)
```

---

### MaintenanceJobs Table

```
Columns:
- Id (int, PK, Identity)
- MachineId (int, FK → Machines.Id, NOT NULL)
- ProjectId (int, FK → Projects.Id, NULL)
- MaintenanceReportId (int, FK → MaintenanceReports.Id, NULL, Unique)
- Type (nvarchar(20), NOT NULL) - Enum as string
- Status (nvarchar(20), NOT NULL, Indexed) - Enum as string
- ScheduledDate (datetime2, NOT NULL, Indexed)
- CompletedDate (datetime2, NULL)
- EstimatedHours (decimal(5,2), NOT NULL)
- ActualHours (decimal(5,2), NULL)
- Reason (nvarchar(2000), NOT NULL)
- Observations (nvarchar(max), NULL)
- PartsReplaced (nvarchar(max), NULL) - JSON array
- CreatedBy (int, FK → Users.Id, NOT NULL)
- CreatedAt (datetime2, NOT NULL, Default: GETUTCDATE())
- UpdatedAt (datetime2, NOT NULL)
- IsActive (bit, NOT NULL, Default: 1)

Indexes:
- PK_MaintenanceJobs (Id)
- UQ_MaintenanceJobs_ReportId (MaintenanceReportId) - One-to-one
- IX_MaintenanceJobs_MachineId (MachineId)
- IX_MaintenanceJobs_ProjectId (ProjectId)
- IX_MaintenanceJobs_Status (Status)
- IX_MaintenanceJobs_ScheduledDate (ScheduledDate)
- IX_MaintenanceJobs_Type (Type)
```

---

### MaintenanceJobAssignments Table

```
Columns:
- Id (int, PK, Identity)
- MaintenanceJobId (int, FK → MaintenanceJobs.Id, NOT NULL)
- MechanicalEngineerId (int, FK → Users.Id, NOT NULL)
- AssignedAt (datetime2, NOT NULL, Default: GETUTCDATE())

Indexes:
- PK_MaintenanceJobAssignments (Id)
- UQ_MaintenanceJobAssignments_Job_Engineer (MaintenanceJobId, MechanicalEngineerId)
- IX_MaintenanceJobAssignments_EngineerId (MechanicalEngineerId)
- IX_MaintenanceJobAssignments_JobId (MaintenanceJobId)
```

---

### Relationships

**MaintenanceReports:**
- OperatorId → Users.Id (Many-to-One)
- MachineId → Machines.Id (Many-to-One)
- MechanicalEngineerId → Users.Id (Many-to-One, Optional)

**MaintenanceJobs:**
- MachineId → Machines.Id (Many-to-One)
- ProjectId → Projects.Id (Many-to-One, Optional)
- MaintenanceReportId → MaintenanceReports.Id (One-to-One, Optional)
- CreatedBy → Users.Id (Many-to-One)

**MaintenanceJobAssignments:**
- MaintenanceJobId → MaintenanceJobs.Id (Many-to-One, Cascade Delete)
- MechanicalEngineerId → Users.Id (Many-to-One)

**Existing Tables Used:**
- Users (Operators and Engineers)
- Machines (with OperatorId, ProjectId, RegionId)
- Projects (with RegionId)
- Regions

---

## 11. SUCCESS CRITERIA

### Technical Success

- All database migrations run successfully
- All API endpoints return correct data
- Region-based filtering works correctly
- Round-robin assignment distributes fairly
- Status synchronization is accurate
- No security vulnerabilities
- Page load time < 2 seconds
- API response time < 500ms
- Zero data loss
- 95%+ uptime

### Functional Success

- Operators can submit reports easily
- Jobs are created automatically
- Engineers see only their regional jobs
- Status updates are reflected immediately
- Operators can verify and close reports
- Complete audit trail maintained
- Maintenance history accessible
- Reports can be searched and filtered

### User Satisfaction

- Operators find the form intuitive
- Engineers have all information needed to diagnose
- Average time to resolution decreases
- Number of "returned" jobs decreases
- Users prefer new system over old method
- Training time < 1 hour per user

---

## APPENDIX: KEY DECISIONS

### Decision 1: My-Machines vs Maintenance-Reports
**Chosen:** My-Machines
**Reason:** Richer diagnostic information (symptoms + error codes) leads to faster issue resolution

### Decision 2: Auto-Create Jobs
**Chosen:** Automatic job creation
**Reason:** Reduces manual work, ensures no reports are missed

### Decision 3: Auto-Assignment
**Chosen:** Round-robin with workload balancing
**Reason:** Fair distribution, prevents overload, automatic process

### Decision 4: Region-Based Filtering
**Chosen:** Strict region filtering at database level
**Reason:** Security, scalability, localization

### Decision 5: Status Synchronization
**Chosen:** Automatic bi-directional sync
**Reason:** Single source of truth, prevents inconsistencies

### Decision 6: Operator Verification
**Chosen:** Required operator approval before closure
**Reason:** Ensures quality, confirms fix works

### Decision 7: Multiple Reports per Machine
**Chosen:** Allow multiple concurrent reports
**Reason:** Realistic - machines can have multiple issues

### Decision 8: Machine Status Management
**Chosen:** Automatic based on highest severity open report
**Reason:** Accurate operational status, prevents premature assignments

---

## DOCUMENT END

**Version:** 1.0
**Date:** October 29, 2025
**Status:** Ready for Implementation
**Next Step:** Begin Phase 1 - Domain & Database Implementation
