# FUNCTIONAL REQUIREMENTS SPECIFICATION
# Drilling & Blasting Management System (DBMS)

## 1. AUTHENTICATION & AUTHORIZATION

### 1.1 User Authentication

**FR-1:** The system shall display a login page with input fields for email address and password.

**FR-2:** The system shall authenticate user credentials using JWT token-based authentication.

**FR-3:** The system shall hash all passwords using BCrypt before storing them in the database.

**FR-4:** The system shall generate a JWT access token upon successful authentication.

**FR-5:** The system shall attach the JWT token to all authenticated API requests via HTTP Authorization header.

**FR-6:** The system shall validate the JWT token for each protected API endpoint.

**FR-7:** The system shall provide a "Forgot Password" feature accessible from the login page.

**FR-8:** The system shall generate a unique 6-digit verification code when a user requests password reset.

**FR-9:** The system shall send the verification code to the user's registered email address.

**FR-10:** The system shall expire the verification code after 10 minutes.

**FR-11:** The system shall allow the user to reset their password using the valid verification code.

**FR-12:** The system shall validate that the email address is in correct format using a value object pattern.

**FR-13:** The system shall prevent login if the user account status is Inactive or Suspended.

**FR-14:** The system shall display appropriate error messages for invalid credentials.

### 1.2 Role-Based Access Control

**FR-15:** The system shall support exactly seven user roles: Admin, Blasting Engineer, Mechanical Engineer, Machine Manager, Operator, Explosive Manager, and Store Manager.

**FR-16:** The system shall enforce role-based access control for all modules and features.

**FR-17:** The system shall enforce permission-based authorization within each role.

**FR-18:** The system shall define the following base permissions: Create, Read, Update, Delete, Approve, Export, Manage, and View.

**FR-19:** The system shall assign specific module permissions to each role via role-permission mappings.

**FR-20:** The system shall restrict access to API endpoints based on user role and permissions.

**FR-21:** The system shall restrict access to frontend routes based on user role using route guards.

**FR-22:** Upon successful login, the system shall redirect the user to a role-specific dashboard.

**FR-23:** The system shall enforce resource ownership validation for sensitive operations.

**FR-24:** The system shall restrict regional users to access only data within their assigned region.

### 1.3 User Roles & Capabilities

**FR-25:** Admin users shall have full system access including user management, project management, machine inventory, and store management.

**FR-26:** Blasting Engineer users shall have access to drilling pattern creation, blast sequence design, explosive calculations, and explosive approval requests.

**FR-27:** Mechanical Engineer users shall have access to view maintenance reports, create maintenance jobs, complete maintenance work, and track maintenance analytics.

**FR-28:** Machine Manager users shall have access to machine inventory management, assignment request approval, accessory inventory, and maintenance oversight.

**FR-29:** Operator users shall have access to view assigned machines, view assigned project sites, submit maintenance reports, and mark drill points as completed.

**FR-30:** Explosive Manager users shall have access to central warehouse inventory management, transfer request approval, quality control, and batch tracking.

**FR-31:** Store Manager users shall have access to regional store inventory management, stock transactions, transfer request creation, and explosive approval requests from blasting engineers.

---

## 2. USER MANAGEMENT

### 2.1 User Account Creation

**FR-32:** The system shall provide a User Management interface accessible only to Admin users.

**FR-33:** Admin users shall be able to create new user accounts.

**FR-34:** The system shall require the following mandatory fields for user creation: Email, Password, First Name, Last Name, Phone Number, Region, and Role.

**FR-35:** The system shall reject user creation if the provided email address already exists in the system.

**FR-36:** The system shall validate email format using the Email value object before account creation.

**FR-37:** The system shall assign a default status of Active to newly created user accounts.

**FR-38:** The system shall record the creation timestamp for each new user account.

### 2.2 User Account Management

**FR-39:** Admin users shall be able to view a list of all users in the system.

**FR-40:** The system shall display user information including Email, First Name, Last Name, Role, Status, Region, and Created Date.

**FR-41:** The system shall exclude password information from all user list displays.

**FR-42:** Admin users shall be able to filter the user list by role, status, region, and creation date.

**FR-43:** Admin users shall be able to search for users by email, first name, or last name.

**FR-44:** Admin users shall be able to edit existing user account details.

**FR-45:** Admin users shall be able to change a user's assigned role.

**FR-46:** Admin users shall be able to update a user's status to Active, Inactive, or Suspended.

**FR-47:** Admin users shall be able to delete user accounts from the system.

**FR-48:** The system shall record the last updated timestamp whenever user information is modified.

### 2.3 User-Project Assignment

**FR-49:** Admin users shall be able to assign users to specific projects.

**FR-50:** The system shall notify users when they are assigned to a project.

**FR-51:** The system shall restrict users to access only projects they are assigned to.

**FR-52:** Admin users shall be able to remove user assignments from projects.

### 2.4 User Profile Management

**FR-53:** The system shall provide a User Profile page accessible to all logged-in users.

**FR-54:** Users shall be able to view their personal details on the User Profile page.

**FR-55:** Users shall be able to edit the following profile fields: First Name, Last Name, Phone Number, Region, and Address.

**FR-56:** Users shall not be able to edit their email address or role.

**FR-57:** The system shall require the user to enter their current password before changing to a new password.

**FR-58:** The system shall enforce password complexity requirements of minimum 8 characters.

**FR-59:** The system shall validate that new passwords contain at least one uppercase letter, one lowercase letter, one number, and one special character.

**FR-60:** The system shall save updated profile information when the user clicks "Save Changes".

**FR-61:** The system shall display a success message after successfully updating profile information.

---

## 3. PROJECT MANAGEMENT

### 3.1 Project Creation & Configuration

**FR-62:** The system shall provide a Project Management interface accessible to Admin users.

**FR-63:** Admin users shall be able to create new mining projects.

**FR-64:** The system shall require the following mandatory fields for project creation: Project Name, Description, Region, Start Date, and End Date.

**FR-65:** The system shall generate a unique Project ID automatically upon project creation.

**FR-66:** The system shall assign one of the 11 predefined regions in Oman to each project: Muscat, Dhofar, Musandam, Al Buraimi, Ad Dakhiliyah, Al Batinah North, Al Batinah South, Ash Sharqiyah North, Ash Sharqiyah South, Ad Dhahirah, or Al Wusta.

**FR-67:** The system shall assign a default status of Planned to newly created projects.

**FR-68:** The system shall allow Admin users to assign a project coordinator to each project.

**FR-69:** The system shall record creation and last updated timestamps for each project.

### 3.2 Project Status Management

**FR-70:** The system shall support the following project statuses: Planned, Active, Completed, OnHold, and Cancelled.

**FR-71:** Admin users shall be able to update the status of a project.

**FR-72:** The system shall track the status change history for each project with timestamps.

**FR-73:** The system shall prevent deletion of projects with Active status.

### 3.3 Project Viewing & Filtering

**FR-74:** The system shall display a list of all projects accessible to the user based on their role and region.

**FR-75:** The system shall allow filtering of projects by Region, Status, Coordinator, and Date Range.

**FR-76:** The system shall allow searching for projects by name or description.

**FR-77:** Admin users shall be able to view detailed information for each project.

**FR-78:** Admin users shall be able to edit project details including name, description, dates, and coordinator.

**FR-79:** The system shall display a Project Dashboard showing total projects, projects by status, and projects by region.

### 3.4 Project Site Management

**FR-80:** The system shall allow users to create multiple project sites under a single project.

**FR-81:** Blasting Engineer users shall be able to create new sites within their assigned projects.

**FR-82:** The system shall require the following mandatory fields for site creation: Site Name, Description, and Location.

**FR-83:** The system shall generate a unique Site ID automatically upon site creation.

**FR-84:** The system shall enforce unique site names within each project.

**FR-85:** The system shall assign a default status of Planned to newly created sites.

**FR-86:** The system shall support the following site statuses: Planned, Active, InProgress, Completed, and OnHold.

**FR-87:** Blasting Engineer users shall be able to update site status.

**FR-88:** Blasting Engineer users shall be able to edit site details.

**FR-89:** The system shall display all sites associated with a project in a list view.

**FR-90:** The system shall track creation and last updated timestamps for each site.

---

## 4. MACHINE MANAGEMENT

### 4.1 Machine Inventory

**FR-91:** The system shall provide a Machine Inventory interface accessible to Machine Manager and Admin users.

**FR-92:** Machine Manager users shall be able to add new machines to the inventory.

**FR-93:** The system shall require the following mandatory fields for machine creation: Machine Name, Type, Model, Manufacturer, Serial Number, and Manufacturing Year.

**FR-94:** The system shall generate a unique Machine ID automatically upon machine creation.

**FR-95:** The system shall assign a default status of Available to newly added machines.

**FR-96:** The system shall support the following machine statuses: Available, InUse, Maintenance, and OutOfService.

**FR-97:** Machine Manager users shall be able to edit machine details.

**FR-98:** Machine Manager users shall be able to delete machines with Available status.

**FR-99:** The system shall prevent deletion of machines with InUse or Maintenance status.

**FR-100:** The system shall track the current location and region assignment for each machine.

**FR-101:** The system shall display machine inventory with filtering options by Type, Status, Region, and Project Assignment.

**FR-102:** The system shall allow searching for machines by name, model, serial number, or manufacturer.

### 4.2 Machine Assignment Workflow

**FR-103:** The system shall allow users to create machine assignment requests.

**FR-104:** Operator and Blasting Engineer users shall be able to submit machine assignment requests for their projects.

**FR-105:** The system shall require the following fields for assignment requests: Project ID, Machine Type, Quantity, Expected Start Date, Expected End Date, and Justification.

**FR-106:** The system shall route all machine assignment requests to the Machine Manager for approval.

**FR-107:** Machine Manager users shall be able to view all pending assignment requests.

**FR-108:** Machine Manager users shall be able to approve assignment requests by selecting specific machines.

**FR-109:** Machine Manager users shall be able to reject assignment requests with a reason.

**FR-110:** The system shall send notifications to the requester when their assignment request is approved or rejected.

**FR-111:** Upon approval, the system shall create a Machine Assignment record linking the machine to the project and operator.

**FR-112:** The system shall update the machine status to InUse when assigned.

**FR-113:** The system shall record the expected return date for each assignment.

**FR-114:** The system shall track the actual return date when the assignment is completed.

**FR-115:** The system shall flag assignments as Overdue if not returned by the expected return date.

**FR-116:** The system shall allow Machine Manager users to mark assignments as Completed.

**FR-117:** The system shall update machine status to Available when assignment is completed.

**FR-118:** The system shall maintain a complete history of all machine assignments with timestamps.

**FR-119:** Machine Manager users shall be able to view assignment history for each machine.

**FR-120:** The system shall allow cancellation of Active assignments with a cancellation reason.

### 4.3 Accessory Inventory Management

**FR-121:** The system shall provide an Accessories Inventory interface accessible to Machine Manager users.

**FR-122:** Machine Manager users shall be able to add new accessories to the inventory.

**FR-123:** The system shall require the following fields for accessory creation: Accessory Name, Category, Part Number, Quantity, Unit, Supplier, and Location.

**FR-124:** The system shall support the following accessory categories: DrillBit, HydraulicHose, Filter, Bearing, Seal, Belt, Lubricant, CoolingFluid, and Other.

**FR-125:** The system shall generate a unique Accessory ID automatically upon creation.

**FR-126:** Machine Manager users shall be able to edit accessory details.

**FR-127:** Machine Manager users shall be able to delete accessories from the inventory.

**FR-128:** The system shall track current stock quantity for each accessory.

**FR-129:** Machine Manager users shall be able to perform stock adjustments for accessories.

**FR-130:** The system shall support the following stock adjustment types: Addition, Reduction, and Transfer.

**FR-131:** The system shall require a reason for each stock adjustment.

**FR-132:** The system shall record the adjusted quantity, adjustment reason, and timestamp for each stock adjustment.

**FR-133:** The system shall update accessory quantity automatically after stock adjustments.

**FR-134:** The system shall allow Machine Manager users to set minimum and maximum stock levels for each accessory.

**FR-135:** The system shall trigger low stock alerts when accessory quantity falls below the minimum threshold.

**FR-136:** The system shall send in-app notifications to Machine Manager users for low stock alerts.

**FR-137:** The system shall display accessories inventory with filtering options by Category, Location, and Stock Level.

**FR-138:** The system shall allow searching for accessories by name, part number, or supplier.

---

## 5. MAINTENANCE MANAGEMENT

### 5.1 Maintenance Report Submission (Operator)

**FR-139:** The system shall provide a Maintenance Report interface accessible to Operator users.

**FR-140:** Operator users shall be able to submit maintenance reports for their assigned machines.

**FR-141:** The system shall require the following fields for maintenance reports: Machine ID, Affected Part, Problem Category, Symptom Description, and Severity Level.

**FR-142:** The system shall support the following machine parts: Engine, Hydraulics, Electrical, DrillHead, Transmission, Brakes, Cooling, Fuel, Chassis, and Other.

**FR-143:** The system shall support the following severity levels: Critical, High, Medium, and Low.

**FR-144:** The system shall allow Operator users to enter error codes if available.

**FR-145:** The system shall generate a unique ticket ID in the format MR-YYYYMMDD-#### upon report submission.

**FR-146:** The system shall assign a default status of Reported to newly submitted maintenance reports.

**FR-147:** The system shall record the timestamp when the maintenance report is submitted.

**FR-148:** The system shall send notifications to Mechanical Engineer users when a new maintenance report is submitted.

**FR-149:** Operator users shall be able to view the status of their submitted maintenance reports.

### 5.2 Maintenance Report Processing (Mechanical Engineer)

**FR-150:** The system shall provide a Maintenance Management interface accessible to Mechanical Engineer users.

**FR-151:** Mechanical Engineer users shall be able to view all pending maintenance reports.

**FR-152:** The system shall support the following maintenance report statuses: Reported, Acknowledged, InProgress, Resolved, and Closed.

**FR-153:** Mechanical Engineer users shall be able to acknowledge received maintenance reports.

**FR-154:** The system shall update report status to Acknowledged when the engineer acknowledges it.

**FR-155:** Mechanical Engineer users shall be able to create maintenance jobs from maintenance reports.

**FR-156:** The system shall allow Mechanical Engineer users to view maintenance report details including machine information, problem description, and severity.

**FR-157:** The system shall allow filtering of maintenance reports by Status, Severity, Machine, and Date Range.

### 5.3 Maintenance Job Management

**FR-158:** Mechanical Engineer users shall be able to create maintenance jobs manually or from maintenance reports.

**FR-159:** The system shall require the following fields for job creation: Machine ID, Maintenance Type, Description, Scheduled Date, and Estimated Hours.

**FR-160:** The system shall support the following maintenance types: Preventive, Corrective, Emergency, and Inspection.

**FR-161:** The system shall generate a unique Job ID automatically upon job creation.

**FR-162:** The system shall assign a default status of Scheduled to newly created maintenance jobs.

**FR-163:** The system shall support the following job statuses: Scheduled, InProgress, Completed, and Cancelled.

**FR-164:** Mechanical Engineer users shall be able to assign maintenance jobs to themselves or other engineers.

**FR-165:** The system shall allow Mechanical Engineer users to start a maintenance job by updating status to InProgress.

**FR-166:** The system shall update the linked maintenance report status to InProgress when the job starts.

**FR-167:** Mechanical Engineer users shall be able to complete maintenance jobs by providing work observations and parts replaced.

**FR-168:** The system shall require the following fields when completing a job: Actual Hours, Work Observations, and Parts Replaced.

**FR-169:** The system shall update job status to Completed when the engineer marks it complete.

**FR-170:** The system shall update the linked maintenance report status to Resolved when the job is completed.

**FR-171:** The system shall update machine status based on maintenance job completion.

**FR-172:** The system shall flag jobs as Overdue if not completed by the scheduled date.

**FR-173:** The system shall send notifications to assigned engineers for overdue jobs.

**FR-174:** Mechanical Engineer users shall be able to cancel maintenance jobs with a cancellation reason.

**FR-175:** The system shall maintain a complete history of all maintenance jobs with timestamps.

**FR-176:** Mechanical Engineer users shall be able to view job history for each machine.

### 5.4 Status Synchronization

**FR-177:** The system shall automatically synchronize statuses between maintenance reports, maintenance jobs, and machines.

**FR-178:** The system shall update machine status to Maintenance when a maintenance job is in progress.

**FR-179:** The system shall update machine status to Available when a maintenance job is completed and no other jobs are pending.

**FR-180:** The system shall ensure consistency between report status and job status at all times.

### 5.5 Maintenance Analytics

**FR-181:** The system shall provide a Maintenance Analytics interface accessible to Mechanical Engineer users.

**FR-182:** The system shall calculate and display average response time from report submission to job start.

**FR-183:** The system shall calculate and display average resolution time from report submission to job completion.

**FR-184:** The system shall display the most common maintenance issues by problem category.

**FR-185:** The system shall calculate machine downtime based on maintenance job duration.

**FR-186:** The system shall display maintenance job counts by status, type, and severity.

**FR-187:** The system shall allow filtering of analytics by date range, machine, and engineer.

---

## 6. DRILLING OPERATIONS

### 6.1 CSV Drill Hole Import

**FR-188:** The system shall provide a CSV Import interface accessible to Blasting Engineer users.

**FR-189:** Blasting Engineer users shall be able to upload CSV files containing drill hole data.

**FR-190:** The system shall support CSV files with the following columns: Hole ID, X Coordinate, Y Coordinate, Z Coordinate, Depth, Azimuth, and Dip.

**FR-191:** The system shall validate all CSV data before importing.

**FR-192:** The system shall support both 2D coordinates (X, Y) and 3D coordinates (X, Y, Z).

**FR-193:** The system shall default Z coordinate to 0 if not provided in CSV file.

**FR-194:** The system shall validate that Depth values are greater than 0.

**FR-195:** The system shall validate that Azimuth values are between 0 and 360 degrees.

**FR-196:** The system shall validate that Dip values are between -90 and 90 degrees.

**FR-197:** The system shall display validation errors with line numbers if CSV data is invalid.

**FR-198:** The system shall allow Blasting Engineer users to review imported drill hole data before saving.

**FR-199:** The system shall save all valid drill holes to the database when the engineer confirms import.

**FR-200:** The system shall associate imported drill holes with the selected project and site.

**FR-201:** The system shall record the import timestamp for each drill hole batch.

### 6.2 Interactive Drilling Pattern Creator

**FR-202:** The system shall provide a Drilling Pattern Creator interface accessible to Blasting Engineer users.

**FR-203:** The system shall render an interactive 2D canvas using Konva.js for pattern design.

**FR-204:** Blasting Engineer users shall be able to add drill points by clicking on the canvas.

**FR-205:** Blasting Engineer users shall be able to drag and reposition drill points on the canvas.

**FR-206:** Blasting Engineer users shall be able to delete drill points from the canvas.

**FR-207:** The system shall display a grid overlay on the canvas for alignment reference.

**FR-208:** The system shall provide grid snapping functionality for precise drill point placement.

**FR-209:** The system shall display ruler measurements on the canvas for distance reference.

**FR-210:** The system shall provide zoom in and zoom out controls for the canvas.

**FR-211:** The system shall provide pan functionality to navigate large patterns.

**FR-212:** Blasting Engineer users shall be able to configure pattern settings for all drill points.

**FR-213:** The system shall allow configuration of the following pattern parameters: Spacing, Burden, Depth, Diameter, and Stemming.

**FR-214:** The system shall validate that Spacing values are greater than 0.

**FR-215:** The system shall validate that Burden values are greater than 0.

**FR-216:** The system shall validate that Depth values are greater than 0.

**FR-217:** The system shall validate that Diameter values are greater than 0.

**FR-218:** The system shall validate that Stemming values are between 0 and Depth.

**FR-219:** The system shall display real-time calculations of pattern metrics as points are added.

**FR-220:** Blasting Engineer users shall be able to save the drilling pattern with a descriptive name.

**FR-221:** The system shall associate saved patterns with the selected project and site.

**FR-222:** Blasting Engineer users shall be able to load previously saved patterns for editing.

**FR-223:** The system shall allow Blasting Engineer users to create pattern templates for reuse.

**FR-224:** Blasting Engineer users shall be able to duplicate existing patterns to create variations.

### 6.3 Drill Hole 3D Visualization

**FR-225:** The system shall provide a Drill Visualization interface accessible to Blasting Engineer users.

**FR-226:** The system shall render drill holes in 3D space using Three.js library.

**FR-227:** The system shall display drill holes with their correct X, Y, and Z coordinates.

**FR-228:** The system shall visualize drill hole depth as vertical cylinders in 3D space.

**FR-229:** The system shall provide orbit camera controls for rotating the 3D view.

**FR-230:** The system shall provide zoom controls for the 3D visualization.

**FR-231:** The system shall provide pan controls for navigating the 3D space.

**FR-232:** The system shall display coordinate axes (X, Y, Z) in the 3D visualization.

**FR-233:** The system shall highlight selected drill holes in the 3D view.

**FR-234:** The system shall display drill hole properties when hovering over holes in 3D view.

### 6.4 Explosive Calculations

**FR-235:** The system shall provide an Explosive Calculation interface accessible to Blasting Engineer users.

**FR-236:** Blasting Engineer users shall be able to calculate explosive requirements for a project site.

**FR-237:** The system shall calculate ANFO requirements based on drill hole parameters.

**FR-238:** The system shall calculate Emulsion requirements based on drill hole parameters.

**FR-239:** The system shall calculate explosive requirements per drill hole.

**FR-240:** The system shall calculate total explosive requirements for the entire site.

**FR-241:** The system shall use hole volume formulas based on depth and diameter.

**FR-242:** The system shall account for stemming length when calculating charge length.

**FR-243:** The system shall apply explosive density values when calculating mass requirements.

**FR-244:** The system shall save calculation results with a descriptive name.

**FR-245:** Blasting Engineer users shall be able to view previously saved calculation results.

**FR-246:** Blasting Engineer users shall be able to duplicate calculation results to create variations.

**FR-247:** The system shall associate calculation results with the selected project and site.

**FR-248:** The system shall record the calculation timestamp for each result.

---

## 7. BLASTING OPERATIONS

### 7.1 Blast Sequence Designer

**FR-249:** The system shall provide a Blast Sequence Designer interface accessible to Blasting Engineer users.

**FR-250:** Blasting Engineer users shall be able to create connections between drill points.

**FR-251:** The system shall support the following connection types: DetonatingCord and Connectors.

**FR-252:** The system shall support the following detonator types: Electric, NonElectric, and Electronic.

**FR-253:** Blasting Engineer users shall be able to assign delay times to each connection in milliseconds.

**FR-254:** Blasting Engineer users shall be able to assign sequence numbers to drill points.

**FR-255:** Blasting Engineer users shall be able to designate starting holes for the blast sequence.

**FR-256:** The system shall visually render connections between drill points as lines on the canvas.

**FR-257:** Blasting Engineer users shall be able to edit connection properties including delay time and detonator type.

**FR-258:** Blasting Engineer users shall be able to delete connections from the sequence.

**FR-259:** The system shall save blast sequence data in JSON format.

**FR-260:** The system shall associate blast sequences with the selected project and site.

**FR-261:** Blasting Engineer users shall be able to load previously saved blast sequences.

### 7.2 Blast Sequence Simulator

**FR-262:** The system shall provide a Blast Sequence Simulator interface accessible to Blasting Engineer users.

**FR-263:** The system shall render blast sequences in 3D space using Three.js library.

**FR-264:** The system shall animate the blast sequence based on assigned delay timings.

**FR-265:** The system shall highlight drill holes as they fire in chronological order during simulation.

**FR-266:** The system shall provide a timeline display showing the current simulation time.

**FR-267:** The system shall provide Play, Pause, and Reset controls for simulation playback.

**FR-268:** The system shall allow adjustment of simulation playback speed.

**FR-269:** The system shall calculate total blast duration based on delay timings.

**FR-270:** The system shall display the firing order of holes during simulation.

**FR-271:** The system shall visualize connections between holes during simulation.

**FR-272:** Blasting Engineer users shall be able to view simulation for previously saved blast sequences.

---

## 8. EXPLOSIVE INVENTORY MANAGEMENT

### 8.1 Central Warehouse Inventory (Explosive Manager)

**FR-273:** The system shall provide a Central Inventory interface accessible to Explosive Manager users.

**FR-274:** Explosive Manager users shall be able to add ANFO inventory batches to the central warehouse.

**FR-275:** The system shall require the following fields for ANFO batches: Batch Number, Quantity, Manufacturing Date, Expiry Date, Supplier, and Storage Location.

**FR-276:** The system shall capture the following technical properties for ANFO: Density, Velocity of Detonation (VOD), Energy, Fume Class, Ammonium Nitrate Percentage, Fuel Oil Percentage, Sensitivity Type, and Grade.

**FR-277:** Explosive Manager users shall be able to add Emulsion inventory batches to the central warehouse.

**FR-278:** The system shall require the following fields for Emulsion batches: Batch Number, Quantity, Manufacturing Date, Expiry Date, Supplier, and Storage Location.

**FR-279:** The system shall capture the following technical properties for Emulsion: Density, Velocity of Detonation (VOD), Energy, Fume Class, Emulsion Percentage, Sensitizer Percentage, Sensitization Type, and Grade.

**FR-280:** The system shall generate a unique Inventory ID automatically for each batch.

**FR-281:** The system shall assign a default status of Available to newly added inventory batches.

**FR-282:** The system shall support the following inventory statuses: Available, Allocated, Depleted, Expired, and Quarantined.

**FR-283:** Explosive Manager users shall be able to edit inventory batch details.

**FR-284:** Explosive Manager users shall be able to update inventory batch status.

**FR-285:** The system shall track the total quantity, allocated quantity, and available quantity for each batch.

**FR-286:** The system shall calculate available quantity as total quantity minus allocated quantity.

**FR-287:** The system shall prevent allocation of quantities exceeding available quantity.

**FR-288:** Explosive Manager users shall be able to quarantine inventory batches for quality issues.

**FR-289:** The system shall allow Explosive Manager users to create quality check records for inventory batches.

**FR-290:** The system shall trigger expiry alerts for batches within 30 days of expiry date.

**FR-291:** The system shall send in-app notifications to Explosive Manager users for expiry alerts.

**FR-292:** The system shall automatically update batch status to Expired after expiry date.

**FR-293:** The system shall display central inventory with filtering options by Explosive Type, Status, Supplier, and Expiry Date Range.

**FR-294:** The system shall allow searching for inventory batches by batch number or supplier.

### 8.2 Inventory Transfer Workflow (Central to Stores)

**FR-295:** Store Manager users shall be able to create inventory transfer requests from central warehouse to their store.

**FR-296:** The system shall require the following fields for transfer requests: Explosive Type, Quantity, Destination Store, Required By Date, and Request Notes.

**FR-297:** The system shall generate a unique Transfer Request ID automatically upon creation.

**FR-298:** The system shall assign a default status of Pending to newly created transfer requests.

**FR-299:** The system shall support the following transfer request statuses: Pending, Approved, Rejected, InProgress, Completed, and Cancelled.

**FR-300:** The system shall route all transfer requests to the Explosive Manager for approval.

**FR-301:** Explosive Manager users shall be able to view all pending transfer requests.

**FR-302:** Explosive Manager users shall be able to approve transfer requests with original or adjusted quantity.

**FR-303:** Explosive Manager users shall be able to reject transfer requests with a rejection reason.

**FR-304:** The system shall send notifications to Store Manager users when their transfer request is approved or rejected.

**FR-305:** The system shall update request status to Approved when the Explosive Manager approves it.

**FR-306:** The system shall update request status to Rejected when the Explosive Manager rejects it.

**FR-307:** Explosive Manager users shall be able to dispatch approved transfer requests.

**FR-308:** The system shall require the following fields for dispatching: Truck Number, Driver Name, Driver Contact, and Dispatch Date/Time.

**FR-309:** The system shall update request status to InProgress upon dispatch.

**FR-310:** The system shall allocate the requested quantity in central warehouse inventory upon dispatch.

**FR-311:** The system shall send notifications to Store Manager users when their request is dispatched.

**FR-312:** Store Manager users shall be able to confirm delivery of transferred inventory.

**FR-313:** The system shall update request status to Completed upon delivery confirmation.

**FR-314:** The system shall create a stock-in transaction in the destination store upon delivery confirmation.

**FR-315:** The system shall update store inventory quantity automatically upon delivery confirmation.

**FR-316:** The system shall reduce central warehouse inventory quantity upon completed transfer.

**FR-317:** The system shall maintain complete transfer history with timestamps and actor information.

**FR-318:** Explosive Manager users shall be able to cancel transfer requests with a cancellation reason.

### 8.3 Store Management

**FR-319:** The system shall provide a Store Management interface accessible to Explosive Manager users.

**FR-320:** Explosive Manager users shall be able to create new stores.

**FR-321:** The system shall require the following fields for store creation: Store Name, Location, Region, and Store Manager.

**FR-322:** The system shall generate a unique Store ID automatically upon store creation.

**FR-323:** The system shall assign a default status of Operational to newly created stores.

**FR-324:** The system shall support the following store statuses: Operational, Maintenance, and Decommissioned.

**FR-325:** Explosive Manager users shall be able to edit store details.

**FR-326:** Explosive Manager users shall be able to update store status.

**FR-327:** The system shall send notifications to assigned Store Manager users when a store is created or modified.

**FR-328:** The system shall display all stores with filtering options by Status, Region, and Store Manager.

### 8.4 Store Inventory Management (Store Manager)

**FR-329:** The system shall provide a Store Inventory interface accessible to Store Manager users.

**FR-330:** Store Manager users shall be able to view inventory for their assigned stores.

**FR-331:** The system shall track current quantity, reserved quantity, and available quantity for each explosive type in each store.

**FR-332:** The system shall calculate available quantity as current quantity minus reserved quantity.

**FR-333:** Store Manager users shall be able to set minimum and maximum stock levels for each explosive type.

**FR-334:** The system shall trigger low stock alerts when quantity falls below the minimum threshold.

**FR-335:** The system shall send in-app notifications to Store Manager users for low stock alerts.

**FR-336:** The system shall record stock transactions for each inventory change.

**FR-337:** The system shall support the following transaction types: StockIn, StockOut, Transfer, Adjustment, and Reservation.

**FR-338:** The system shall record the following fields for each transaction: Transaction Type, Quantity, Transaction Date, Reference Number, and Notes.

**FR-339:** Store Manager users shall be able to view complete transaction history for their stores.

**FR-340:** The system shall allow filtering of transactions by Type, Date Range, and Explosive Type.

**FR-341:** The system shall track batch numbers for inventory received from central warehouse.

**FR-342:** The system shall track manufacturing and expiry dates for store inventory.

**FR-343:** The system shall trigger expiry alerts for store inventory within 30 days of expiry.

---

## 9. EXPLOSIVE APPROVAL REQUESTS

### 9.1 Explosive Usage Request (Blasting Engineer)

**FR-344:** The system shall provide an Explosive Approval Request interface accessible to Blasting Engineer users.

**FR-345:** Blasting Engineer users shall be able to submit requests for explosive usage for their project sites.

**FR-346:** The system shall require the following fields for explosive requests: Project ID, Site ID, Explosive Type, Quantity, Expected Usage Date, Blasting Date/Time, and Justification.

**FR-347:** The system shall allow Blasting Engineer users to add comments and safety notes to requests.

**FR-348:** The system shall require completion of a safety checklist for each request.

**FR-349:** The system shall require environmental assessment information for each request.

**FR-350:** The system shall generate a unique Request ID automatically upon request creation.

**FR-351:** The system shall assign a default status of Pending to newly created explosive requests.

**FR-352:** The system shall support the following request statuses: Pending, Approved, Rejected, Cancelled, and Expired.

**FR-353:** The system shall support the following request priorities: Low, Normal, High, and Critical.

**FR-354:** The system shall route explosive requests to the appropriate Store Manager based on project region.

### 9.2 Explosive Request Approval (Store Manager)

**FR-355:** Store Manager users shall be able to view all explosive requests for their region.

**FR-356:** Store Manager users shall be able to filter requests by Status, Priority, Explosive Type, and Date Range.

**FR-357:** Store Manager users shall be able to approve explosive requests.

**FR-358:** Store Manager users shall be able to reject explosive requests with a rejection reason.

**FR-359:** The system shall send notifications to Blasting Engineer users when their request is approved or rejected.

**FR-360:** The system shall update request status to Approved when the Store Manager approves it.

**FR-361:** The system shall update request status to Rejected when the Store Manager rejects it.

**FR-362:** The system shall reserve the approved quantity in store inventory upon approval.

**FR-363:** The system shall create a reservation transaction in store inventory upon approval.

**FR-364:** The system shall automatically expire requests if not approved within a configurable time period.

**FR-365:** The system shall maintain complete request history with timestamps and approver information.

---

## 10. OPERATOR WORKFLOWS

### 10.1 Operator Dashboard & Assignments

**FR-366:** The system shall provide an Operator Dashboard accessible to Operator users.

**FR-367:** Operator users shall be able to view their assigned project.

**FR-368:** Operator users shall be able to view all sites within their assigned project.

**FR-369:** Operator users shall be able to view their assigned machines.

**FR-370:** The system shall display machine details including Type, Model, and Assignment Date for operator's machines.

**FR-371:** Operator users shall be able to view drilling patterns for their assigned sites.

**FR-372:** The system shall provide read-only access to drilling patterns for Operator users.

**FR-373:** Operator users shall be able to view site details including location and status.

### 10.2 Drill Point Completion Tracking

**FR-374:** Operator users shall be able to mark individual drill points as completed.

**FR-375:** The system shall update drill point status to Completed when marked by operator.

**FR-376:** The system shall record the completion timestamp for each drill point.

**FR-377:** The system shall display completion progress for each site based on completed drill points.

**FR-378:** The system shall calculate completion percentage as completed points divided by total points.

---

## 11. DASHBOARDS & NOTIFICATIONS

### 11.1 Role-Specific Dashboards

**FR-379:** The system shall provide a unique dashboard for each user role upon login.

**FR-380:** Admin dashboard shall display total users, total projects, total machines, and system-wide statistics.

**FR-381:** Blasting Engineer dashboard shall display active sites, pending calculations, recent patterns, and explosive requests.

**FR-382:** Operator dashboard shall display assigned machines, active sites, pending tasks, and recent notifications.

**FR-383:** Mechanical Engineer dashboard shall display pending reports, active jobs, overdue jobs, and maintenance statistics.

**FR-384:** Machine Manager dashboard shall display machine availability, pending assignment requests, low stock accessories, and assignment statistics.

**FR-385:** Explosive Manager dashboard shall display inventory levels, pending transfer requests, expiring batches, and low stock alerts.

**FR-386:** Store Manager dashboard shall display store inventory levels, pending requests, low stock alerts, and recent transactions.

**FR-387:** All dashboards shall display the total count of unread notifications for the user.

**FR-388:** All dashboards shall provide quick access links to primary features for that role.

### 11.2 Notification System

**FR-389:** The system shall provide an in-app notification system accessible to all users.

**FR-390:** The system shall send notifications for machine assignment approvals and rejections.

**FR-391:** The system shall send notifications when new maintenance reports are submitted.

**FR-392:** The system shall send notifications when maintenance jobs are assigned to engineers.

**FR-393:** The system shall send notifications when maintenance jobs are overdue.

**FR-394:** The system shall send notifications for inventory transfer request approvals and rejections.

**FR-395:** The system shall send notifications when transfer requests are dispatched.

**FR-396:** The system shall send notifications for explosive request approvals and rejections.

**FR-397:** The system shall send notifications for low stock alerts in accessories and store inventory.

**FR-398:** The system shall send notifications for expiry alerts in central and store inventory.

**FR-399:** The system shall send notifications when users are assigned to projects.

**FR-400:** The system shall send notifications when stores are created or assigned to store managers.

**FR-401:** The system shall display notification count badge on the notification icon.

**FR-402:** Users shall be able to view all their notifications in a notification panel.

**FR-403:** Users shall be able to mark individual notifications as read.

**FR-404:** Users shall be able to mark all notifications as read.

**FR-405:** The system shall display notification timestamp and sender information.

**FR-406:** The system shall provide navigation links within notifications to relevant resources.

---

## 12. REGION MANAGEMENT

**FR-407:** The system shall support exactly 11 regions based on Oman's governorates: Muscat, Dhofar, Musandam, Al Buraimi, Ad Dakhiliyah, Al Batinah North, Al Batinah South, Ash Sharqiyah North, Ash Sharqiyah South, Ad Dhahirah, and Al Wusta.

**FR-408:** The system shall assign each user to exactly one region.

**FR-409:** The system shall assign each project to exactly one region.

**FR-410:** The system shall assign each store to exactly one region.

**FR-411:** The system shall assign each machine to exactly one region.

**FR-412:** The system shall restrict regional users to view and manage data only within their assigned region.

**FR-413:** Admin users shall have access to data across all regions.

**FR-414:** The system shall allow filtering of data by region in all list views.

---

## 13. DATA INTEGRITY & AUDIT

**FR-415:** The system shall record creation timestamp for all entities upon creation.

**FR-416:** The system shall record last updated timestamp for all entities upon modification.

**FR-417:** The system shall track the user who created each entity.

**FR-418:** The system shall track the user who last modified each entity.

**FR-419:** The system shall support soft deletion using an IsActive flag for critical entities.

**FR-420:** The system shall prevent hard deletion of entities with dependent relationships.

**FR-421:** The system shall cascade delete related entities when a parent entity is deleted where appropriate.

**FR-422:** The system shall maintain referential integrity for all foreign key relationships.

**FR-423:** The system shall use optimistic concurrency control with UpdatedAt timestamps.

**FR-424:** The system shall validate all required fields before saving entities to the database.

**FR-425:** The system shall enforce unique constraints for unique identifiers (email, batch number, etc.).

---

## 14. SECURITY & VALIDATION

**FR-426:** The system shall hash all passwords using BCrypt with a minimum work factor of 10.

**FR-427:** The system shall never store passwords in plain text.

**FR-428:** The system shall validate all API request payloads using FluentValidation library.

**FR-429:** The system shall prevent SQL injection by using parameterized queries through Entity Framework.

**FR-430:** The system shall sanitize all user inputs before processing.

**FR-431:** The system shall enforce CORS policies to restrict unauthorized domain access.

**FR-432:** The system shall validate JWT tokens for all protected API endpoints.

**FR-433:** The system shall reject expired JWT tokens.

**FR-434:** The system shall reject requests with invalid or missing authentication tokens.

**FR-435:** The system shall enforce role-based authorization for all API endpoints using authorization policies.

**FR-436:** The system shall log all authentication failures for security monitoring.

**FR-437:** The system shall log all authorization failures for security monitoring.

**FR-438:** The system shall validate email format using the Email value object pattern.

**FR-439:** The system shall validate that numeric values are within acceptable ranges.

**FR-440:** The system shall validate that date values are logical (e.g., start date before end date).

---

## 15. PERFORMANCE & CACHING

**FR-441:** The system shall implement in-memory caching for frequently accessed data.

**FR-442:** The system shall cache user session data to reduce database queries.

**FR-443:** The system shall cache role and permission data for authorization checks.

**FR-444:** The system shall invalidate cached data when underlying data is modified.

**FR-445:** The system shall implement performance monitoring for all service operations.

**FR-446:** The system shall log performance metrics for operations exceeding threshold duration.

**FR-447:** The system shall use projection queries to retrieve only required fields for list views.

**FR-448:** The system shall implement pagination for all list views to limit data transfer.

**FR-449:** The system shall use lazy loading for navigation properties to optimize query performance.

---

## 16. LOGGING & ERROR HANDLING

**FR-450:** The system shall implement structured logging with correlation IDs for request tracking.

**FR-451:** The system shall log all API requests with method, endpoint, user, and timestamp.

**FR-452:** The system shall log all business rule violations as warnings.

**FR-453:** The system shall log all validation failures with details of failed fields.

**FR-454:** The system shall log all database errors with query context.

**FR-455:** The system shall log all unhandled exceptions with stack traces.

**FR-456:** The system shall implement global exception handling middleware to catch all errors.

**FR-457:** The system shall return appropriate HTTP status codes for different error types.

**FR-458:** The system shall return user-friendly error messages for client-facing errors.

**FR-459:** The system shall hide sensitive implementation details in error messages.

**FR-460:** The system shall implement the Result pattern to handle operation outcomes without exceptions.

---

## TOTAL FUNCTIONAL REQUIREMENTS: 460