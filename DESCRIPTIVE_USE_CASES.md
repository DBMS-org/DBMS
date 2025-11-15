# DESCRIPTIVE USE CASES SPECIFICATION
## Drilling & Blasting Management System (DBMS)

**Document Version:** 2.0 (Corrected)
**Date:** November 15, 2025
**Project:** DBMS - Enterprise Drilling & Blasting Management System
**Based on:** Functional Requirements v2.0 and Implementation Analysis

---

## TABLE OF CONTENTS

1. [Introduction](#1-introduction)
2. [Use Case Diagram Overview](#2-use-case-diagram-overview)
3. [Authentication & Authorization Use Cases](#3-authentication--authorization-use-cases)
4. [User Management Use Cases](#4-user-management-use-cases)
5. [Project Management Use Cases](#5-project-management-use-cases)
6. [Drilling Operations Use Cases](#6-drilling-operations-use-cases)
7. [Blasting Operations Use Cases](#7-blasting-operations-use-cases)
8. [Machine Management Use Cases](#8-machine-management-use-cases)
9. [Explosive Inventory Use Cases](#9-explosive-inventory-use-cases)
10. [Store Management Use Cases](#10-store-management-use-cases)
11. [Approval Workflow Use Cases](#11-approval-workflow-use-cases)
12. [Reporting & Analytics Use Cases](#12-reporting--analytics-use-cases)

---

## 1. INTRODUCTION

### 1.1 Purpose
This document provides detailed descriptive use cases for the Drilling & Blasting Management System (DBMS). Each use case includes actors, preconditions, postconditions, basic flow, exceptional flow, and alternate flow.

### 1.2 Use Case Template
Each use case follows this standard format:
- **Use Case ID**: Unique identifier
- **Use Case Name**: Descriptive name
- **Actors**: Users or systems involved
- **Preconditions**: Conditions that must be true before execution
- **Postconditions**: System state after successful completion
- **Basic Flow**: Normal sequence of steps
- **Exceptional Flow**: Error handling scenarios
- **Alternate Flow**: Alternative paths to achieve the goal

### 1.3 Actor Definitions

| Actor | Description |
|-------|-------------|
| Admin | System administrator with full access privileges |
| Blasting Engineer | Engineer responsible for designing drilling patterns and blast sequences |
| Machine Manager | Manager responsible for machine inventory and assignments |
| Explosive Manager | Manager responsible for explosive inventory and approvals |
| Store Manager | Manager of a specific explosive storage facility |
| Mechanical Engineer | Engineer responsible for machine maintenance |
| Operator | Field operator who executes drilling operations |
| System | Automated system processes |

---

## 3. AUTHENTICATION & AUTHORIZATION USE CASES

---

### UC-AUTH-001: User Login

**Use Case ID**: UC-AUTH-001

**Use Case Name**: User Login

**Actors**:
- Primary: Any User (Admin, Blasting Engineer, Machine Manager, Explosive Manager, Store Manager, Mechanical Engineer, Operator)
- Secondary: System

**Preconditions**:
- User must have a valid registered account in the system
- User must not be currently logged in
- User account must not be locked or deactivated

**Postconditions**:
- User is successfully authenticated
- User session is established
- User login time is recorded
- User is redirected to role-specific dashboard
- User can access authorized system functions

**Basic Flow**:
1. User navigates to the login page
2. System displays login form with Username (Email) and Password fields
3. User enters their email address in the Username field
4. User enters their password in the Password field
5. User clicks the "Login" button
6. System validates that both fields are not empty
7. System verifies user credentials
8. System confirms user account is active
9. System authenticates user and creates session
10. System records login time for audit purposes
11. System redirects user to role-specific dashboard
12. Use case ends successfully

**Exceptional Flow**:

**E1: Empty Credentials**
- **Trigger**: User submits form with empty email or password (Step 6)
- **Actions**:
  1. System displays validation error: "Email and Password are required"
  2. System keeps user on login page
  3. Return to Step 3

**E2: User Not Found**
- **Trigger**: No user exists with provided email address (Step 7)
- **Actions**:
  1. System displays error message: "Invalid email or password"
  2. System logs failed login attempt
  3. System keeps user on login page
  4. Return to Step 3

**E3: Invalid Password**
- **Trigger**: Password is incorrect (Step 7)
- **Actions**:
  1. System increments failed login attempt counter for user
  2. System displays error message: "Invalid email or password"
  3. System logs failed login attempt with timestamp and IP address
  4. If failed attempts >= 5, go to E4
  5. Otherwise, return to Step 3

**E4: Account Locked**
- **Trigger**: User has 5 consecutive failed login attempts (Step 9 or E3)
- **Actions**:
  1. System sets user account status to "Locked"
  2. System displays error message: "Account locked – please reset your password"
  3. System sends email notification to user about account lockout
  4. System logs lockout event
  5. Use case ends

**E5: Account Deactivated**
- **Trigger**: User account status is "Deactivated" (Step 9)
- **Actions**:
  1. System displays error message: "Your account has been deactivated. Please contact administrator"
  2. System logs attempted login of deactivated account
  3. Use case ends

**E6: Network Error**
- **Trigger**: Network connection fails during authentication (any step)
- **Actions**:
  1. System displays error message: "Network error. Please check your connection and try again"
  2. System keeps user on login page
  3. Return to Step 3

**Alternate Flow**:

**A1: First-Time Login with Temporary Password**
- **Trigger**: User has temporary password set (after Step 9)
- **Actions**:
  1. System authenticates user
  2. System redirects user to Change Password page instead of dashboard
  3. System displays message: "You must change your temporary password before continuing"
  4. User must complete password change process (see UC-AUTH-003)
  5. After successful password change, user proceeds to dashboard
  6. Use case ends successfully

**A2: Remember Me Option**
- **Trigger**: User selects "Remember Me" checkbox (after Step 4)
- **Actions**:
  1. Continue Steps 5-9 from basic flow
  2. System extends session duration
  3. User remains logged in for extended period
  4. Continue from Step 10
  5. Use case ends successfully

---

### UC-AUTH-002: Password Reset Request

**Use Case ID**: UC-AUTH-002

**Use Case Name**: Password Reset Request (Forgot Password)

**Actors**:
- Primary: Any User
- Secondary: System, Email Service

**Preconditions**:
- User must have a registered account with valid email address
- User is not currently logged in
- Email service must be operational

**Postconditions**:
- Password reset code is generated
- Reset code is sent to user's registered email
- Reset code will expire after 10 minutes
- Reset request is logged for security audit

**Basic Flow**:
1. User navigates to the login page
2. User clicks "Forgot Password?" link
3. System displays password reset request page
4. System displays email input field
5. User enters their registered email address
6. User clicks "Send Reset Code" button
7. System validates email format
8. System verifies user account exists
9. System generates unique 6-digit reset code
10. System sets code expiration time (10 minutes from now)
11. System sends email to user with reset code and instructions
12. System displays success message: "Reset code has been sent to your email. Please check your inbox"
13. System redirects user to reset code verification page
14. Use case ends successfully

**Exceptional Flow**:

**E1: Invalid Email Format**
- **Trigger**: Email format validation fails (Step 7)
- **Actions**:
  1. System displays error: "Please enter a valid email address"
  2. System keeps focus on email input field
  3. Return to Step 5

**E2: Email Not Found**
- **Trigger**: No user exists with provided email (Step 8)
- **Actions**:
  1. For security reasons, system displays same success message as basic flow
  2. System logs attempted reset for non-existent email
  3. System does not send any email
  4. Use case ends (user sees success message but receives no email)

**E3: Email Service Failure**
- **Trigger**: Email service fails to send email (Step 11)
- **Actions**:
  1. System logs email delivery failure
  2. System displays error: "Failed to send reset code. Please try again later"
  3. System cancels reset request
  4. Return to Step 5

**E4: Multiple Requests**
- **Trigger**: User already has active reset code (after Step 8)
- **Actions**:
  1. System invalidates previous reset code
  2. Continue with Steps 9-14 (generate new code)
  3. Use case ends successfully

**E5: Database Error**
- **Trigger**: Database operation fails (Step 11)
- **Actions**:
  1. System logs database error with details
  2. System displays error: "System error occurred. Please try again"
  3. Use case ends

**Alternate Flow**:

**A1: User Cancels Reset Request**
- **Trigger**: User clicks "Cancel" or "Back to Login" (any step before Step 6)
- **Actions**:
  1. System navigates back to login page
  2. No reset code is generated
  3. Use case ends

---

### UC-AUTH-003: Complete Password Reset

**Use Case ID**: UC-AUTH-003

**Use Case Name**: Complete Password Reset

**Actors**:
- Primary: Any User
- Secondary: System, Email Service

**Preconditions**:
- User has requested password reset (UC-AUTH-002 completed)
- User has received reset code via email
- Reset code has not expired (within 10 minutes)
- Reset code has not been used

**Postconditions**:
- User's password is securely updated
- Reset code is marked as used and invalidated
- User receives email confirmation of password change
- User can login with new password
- Password change event is logged

**Basic Flow**:
1. User receives reset code via email
2. User navigates to reset code verification page (or is already there from UC-AUTH-002)
3. System displays form with fields:
   - Email Address
   - Reset Code (6-digit)
   - New Password
   - Confirm New Password
4. User enters their email address
5. User enters the 6-digit reset code from email
6. User enters new password
7. User enters new password again in confirm field
8. User clicks "Reset Password" button
9. System validates all fields are not empty
10. System validates email format
11. System validates reset code format (6 digits)
12. System validates new password matches confirm password
13. System validates new password meets complexity requirements:
    - Minimum 8 characters
    - At least one uppercase letter
    - At least one lowercase letter
    - At least one digit
    - At least one special character
14. System verifies reset code matches the one sent to user's email
15. System verifies reset code exists
16. System checks reset code has not expired
17. System verifies reset code has not been used
18. System retrieves user account
19. System securely processes new password
20. System updates user record with new password
21. System marks reset code as used
22. System records when reset code was used
23. System logs password reset event with user ID and timestamp
24. System composes confirmation email:
    - Subject: "Password Changed Successfully - DBMS"
    - Body confirming password change
    - Timestamp of change
    - Security notice to contact admin if not authorized
25. System sends confirmation email to user
26. System displays success message: "Password has been reset successfully. You can now login with your new password"
27. System redirects to login page after 3 seconds
28. Use case ends successfully

**Exceptional Flow**:

**E1: Empty Fields**
- **Trigger**: Any required field is empty (Step 9)
- **Actions**:
  1. System displays validation error: "All fields are required"
  2. System highlights empty fields in red
  3. Return to Step 4

**E2: Invalid Email Format**
- **Trigger**: Email format validation fails (Step 10)
- **Actions**:
  1. System displays error: "Invalid email format"
  2. System highlights email field
  3. Return to Step 4

**E3: Invalid Reset Code Format**
- **Trigger**: Reset code is not 6 digits (Step 11)
- **Actions**:
  1. System displays error: "Reset code must be 6 digits"
  2. System highlights reset code field
  3. Return to Step 5

**E4: Passwords Do Not Match**
- **Trigger**: New password ≠ confirm password (Step 12)
- **Actions**:
  1. System displays error: "Passwords do not match"
  2. System clears confirm password field
  3. System highlights both password fields
  4. Return to Step 6

**E5: Weak Password**
- **Trigger**: Password does not meet complexity requirements (Step 13)
- **Actions**:
  1. System displays error message with specific requirements not met:
     - "Password must be at least 8 characters"
     - "Password must contain at least one uppercase letter"
     - "Password must contain at least one lowercase letter"
     - "Password must contain at least one digit"
     - "Password must contain at least one special character"
  2. System highlights password fields
  3. Return to Step 6

**E6: Reset Code Not Found**
- **Trigger**: No matching reset code found in database (Step 15)
- **Actions**:
  1. System displays error: "Invalid reset code or email address"
  2. System logs failed reset attempt
  3. Return to Step 4

**E7: Reset Code Expired**
- **Trigger**: Current time > expiration timestamp (Step 16)
- **Actions**:
  1. System displays error: "Reset code has expired. Please request a new one"
  2. System provides "Request New Code" button
  3. If user clicks button, redirect to UC-AUTH-002
  4. Otherwise, use case ends

**E8: Reset Code Already Used**
- **Trigger**: Reset code IsUsed = true (Step 17)
- **Actions**:
  1. System displays error: "This reset code has already been used. Please request a new one"
  2. System provides "Request New Code" button
  3. If user clicks button, redirect to UC-AUTH-002
  4. Otherwise, use case ends

**E9: System Update Failure**
- **Trigger**: Password update fails (Step 20 or 21)
- **Actions**:
  1. System cancels the operation
  2. System logs error
  3. System displays error: "Failed to reset password. Please try again"
  4. Return to Step 4

**E10: Email Notification Failure**
- **Trigger**: Confirmation email fails to send (Step 25)
- **Actions**:
  1. System logs email failure (but password change is already complete)
  2. Continue with Steps 26-28
  3. Use case ends successfully (password is changed despite email failure)

**Alternate Flow**:

**A1: User Cancels Reset**
- **Trigger**: User clicks "Cancel" button (any step before Step 8)
- **Actions**:
  1. System displays confirmation dialog: "Are you sure you want to cancel password reset?"
  2. If user confirms:
     - System redirects to login page
     - Use case ends
  3. If user cancels the cancellation:
     - Return to current step

**A2: Request New Reset Code**
- **Trigger**: User clicks "Didn't receive code?" link (any step)
- **Actions**:
  1. System redirects to UC-AUTH-002 (Password Reset Request)
  2. Email field is pre-populated if already entered
  3. Use case ends (new use case begins)

---

### UC-AUTH-004: User Logout

**Use Case ID**: UC-AUTH-004

**Use Case Name**: User Logout

**Actors**:
- Primary: Any Authenticated User
- Secondary: System

**Preconditions**:
- User must be logged in with active session
- User must be authenticated

**Postconditions**:
- User session is terminated
- User authentication is invalidated
- User is redirected to login page
- Logout event is logged
- User cannot access protected resources without re-authentication

**Basic Flow**:
1. User is on any authenticated page in the system
2. User clicks "Logout" button in navigation menu or user profile dropdown
3. System displays confirmation dialog: "Are you sure you want to logout?"
4. User clicks "Yes" to confirm logout
5. System processes logout request
6. System validates user session
7. System logs logout event with:
   - User ID
   - Logout timestamp
   - Session duration
   - IP address
8. System confirms logout successful
9. System clears user session information
10. System clears all cached user data
11. System resets application to logged-out state
12. System displays notification: "You have been logged out successfully"
13. System redirects user to login page
14. Use case ends successfully

**Exceptional Flow**:

**E1: User Cancels Logout**
- **Trigger**: User clicks "Cancel" or closes confirmation dialog (Step 4)
- **Actions**:
  1. System dismisses confirmation dialog
  2. User remains on current page
  3. Session remains active
  4. Use case ends

**E2: Service Unavailable**
- **Trigger**: Logout service unavailable (Step 5-8)
- **Actions**:
  1. System proceeds with local cleanup (Steps 9-11)
  2. System logs logout event locally
  3. Continue with Steps 12-14
  4. Use case ends successfully (graceful degradation)

**E3: Invalid Session**
- **Trigger**: Session validation fails (Step 6)
- **Actions**:
  1. System returns unauthorized response
  2. System proceeds with local cleanup (Steps 9-11)
  3. Continue with Steps 12-14
  4. Use case ends successfully

**Alternate Flow**:

**A1: Automatic Session Timeout Logout**
- **Trigger**: User idle for 30 minutes (system timeout)
- **Actions**:
  1. System detects session timeout
  2. System automatically performs Steps 5-11 (no confirmation needed)
  3. System displays notification: "Your session has expired due to inactivity. Please login again"
  4. System redirects to login page
  5. Use case ends

**A2: Quick Logout (No Confirmation)**
- **Trigger**: User presses Ctrl+Shift+Q keyboard shortcut
- **Actions**:
  1. Skip confirmation dialog (Step 3-4)
  2. Continue directly with Steps 5-14
  3. Use case ends successfully

---

### UC-AUTH-005: Token Validation

**Use Case ID**: UC-AUTH-005

**Use Case Name**: Token Validation

**Actors**:
- Primary: System
- Secondary: Any Authenticated User

**Preconditions**:
- User has previously logged in and has active session
- User attempts to access protected resource
- Session information is maintained by system

**Postconditions**:
- Session validity is determined
- Access to protected resource is granted or denied
- Invalid sessions trigger re-authentication
- Session validation is logged for security audit

**Basic Flow**:
1. User navigates to protected page or initiates action requiring authentication
2. System retrieves user session information
3. System includes session credentials with request
4. System sends request to verify access
5. System receives request and extracts session information
6. System validates session is present
7. System verifies session authenticity
8. System validates session integrity
9. System validates session source
10. System validates session authorization level
11. System checks session expiration time
12. System compares expiration time with current time
13. System verifies session has not expired
14. System extracts user information from session:
    - User ID
    - Full Name
    - Email
    - Primary Role
    - Assigned Region
15. System attaches user identity to request
16. System logs successful session validation
17. System proceeds with processing the requested operation
18. System returns successful response with requested data
19. System renders response data to user
20. Use case ends successfully

**Exceptional Flow**:

**E1: Session Not Found**
- **Trigger**: No active session found (Step 2)
- **Actions**:
  1. System displays notification: "Session expired. Please login"
  2. System redirects user to login page
  3. Use case ends

**E2: Missing Session Information**
- **Trigger**: Session information not present in request (Step 6)
- **Actions**:
  1. System returns unauthorized response
  2. System includes error message: "Session information is missing"
  3. System receives unauthorized response
  4. System redirects to login page
  5. Use case ends

**E3: Invalid Session Format**
- **Trigger**: Session cannot be verified (Step 7)
- **Actions**:
  1. System logs security warning: "Malformed session"
  2. System returns unauthorized response
  3. System includes error message: "Invalid session format"
  4. System removes invalid session
  5. System redirects to login page
  6. Use case ends

**E4: Invalid Verification**
- **Trigger**: Session verification fails (Step 8)
- **Actions**:
  1. System logs security alert: "Session verification failed"
  2. System returns unauthorized response
  3. System includes error message: "Invalid session"
  4. System removes invalid session
  5. System redirects to login page
  6. Use case ends

**E5: Invalid Source**
- **Trigger**: Session source doesn't match expected source (Step 9)
- **Actions**:
  1. System logs security alert: "Session source mismatch"
  2. System returns unauthorized response
  3. System includes error message: "Invalid session source"
  4. System removes invalid session
  5. System redirects to login page
  6. Use case ends

**E6: Invalid Authorization Level**
- **Trigger**: Session authorization doesn't match required level (Step 10)
- **Actions**:
  1. System logs security alert: "Authorization level mismatch"
  2. System returns unauthorized response
  3. System includes error message: "Invalid authorization"
  4. System removes invalid session
  5. System redirects to login page
  6. Use case ends

**E7: Session Expired**
- **Trigger**: Current time exceeds session expiration time (Step 13)
- **Actions**:
  1. System returns unauthorized response
  2. System includes error message: "Session has expired"
  3. System receives unauthorized response
  4. System removes expired session
  5. System displays notification: "Your session has expired. Please login again"
  6. System redirects to login page
  7. Use case ends

**E8: User Account Deactivated**
- **Trigger**: User ID in session corresponds to deactivated account (after Step 14)
- **Actions**:
  1. System checks user status
  2. System detects account is deactivated
  3. System returns forbidden response
  4. System includes error message: "Account has been deactivated"
  5. System removes session
  6. System displays error message
  7. System redirects to login page
  8. Use case ends

**Alternate Flow**:

**A1: Session Refresh (Optional Enhancement)**
- **Trigger**: Session is close to expiration (within 5 minutes) but still valid (after Step 13)
- **Actions**:
  1. Continue Steps 14-18 to complete current request
  2. System generates new session with extended expiration
  3. System includes new session information in response
  4. System receives response and extracts new session
  5. System replaces old session with new session
  6. System displays subtle notification: "Session extended"
  7. Use case ends successfully

---

## 4. USER MANAGEMENT USE CASES

---

### UC-USER-001: Create New User Account

**Use Case ID**: UC-USER-001

**Use Case Name**: Create New User Account

**Actors**:
- Primary: Admin
- Secondary: System, Email Service

**Preconditions**:
- Admin must be authenticated and authorized
- Admin must have "RequireAdminRole" authorization
- User Management interface must be accessible
- Email service must be operational

**Postconditions**:
- New user account is created
- User password is securely stored
- User is assigned specified role
- New user receives welcome email with credentials
- User creation event is logged
- Admin receives confirmation of successful creation

**Basic Flow**:
1. Admin navigates to User Management section
2. System displays list of existing users
3. Admin clicks "Add New User" button
4. System displays user creation form with fields:
   - Email Address (required)
   - Password (required)
   - First Name (required)
   - Last Name (required)
   - Home-Country Phone Number (required)
   - Resident-Country Phone Number (required)
   - Country of Origin (required)
   - Residential Address (required)
   - Primary Role (required, dropdown)
   - Region Assignment (optional, dropdown)
5. Admin enters email address
6. Admin enters temporary password
7. Admin enters first name
8. Admin enters last name
9. Admin enters home-country phone number
10. Admin enters resident-country phone number
11. Admin selects country of origin from dropdown
12. Admin enters residential address
13. Admin selects primary role from dropdown (Admin, Blasting Engineer, Machine Manager, Explosive Manager, Store Manager, Mechanical Engineer, Operator)
14. Admin optionally selects region assignment
15. Admin clicks "Create User" button
16. System validates all required fields are not empty
17. System validates email format using regex
18. System validates phone number formats
19. System validates password meets complexity requirements
20. System checks if email already exists
21. System validates email is unique
22. System securely stores password
23. System creates user record with:
    - All provided information
    - Secure password storage
    - Account status: Active
    - Created timestamp
    - Created By: Current Admin's User ID
    - Is First Login: true
24. System creates user-role association
25. System saves changes
26. System composes welcome email:
    - Subject: "Welcome to DBMS - Your Account Details"
    - Body with:
      * Welcome message
      * Login URL
      * Username (email address)
      * Temporary password
      * Instructions to change password on first login
      * System support contact information
27. System sends email to new user's email address
28. System logs user creation event with Admin ID and timestamp
29. System displays success notification: "User account created successfully"
30. System refreshes user list showing new user
31. Use case ends successfully

**Exceptional Flow**:

**E1: Empty Required Fields**
- **Trigger**: One or more required fields are empty (Step 16)
- **Actions**:
  1. System displays validation errors next to each empty field
  2. System displays summary message: "Please fill in all required fields"
  3. System highlights empty fields in red
  4. Return to Step 5

**E2: Invalid Email Format**
- **Trigger**: Email format validation fails (Step 17)
- **Actions**:
  1. System displays error: "Please enter a valid email address"
  2. System highlights email field
  3. Return to Step 5

**E3: Invalid Phone Number Format**
- **Trigger**: Phone number format validation fails (Step 18)
- **Actions**:
  1. System displays error: "Please enter valid phone numbers in format: +XXX-XXXXXXXXXX"
  2. System highlights phone number fields
  3. Return to Step 9

**E4: Weak Password**
- **Trigger**: Password doesn't meet complexity requirements (Step 19)
- **Actions**:
  1. System displays error message with requirements:
     - "Password must be at least 8 characters"
     - "Must contain uppercase, lowercase, digit, and special character"
  2. System highlights password field
  3. Return to Step 6

**E5: Email Already Exists**
- **Trigger**: Email address already registered (Step 21)
- **Actions**:
  1. System displays error: "A user with this email address already exists"
  2. System suggests: "Please use a different email address or check existing users"
  3. System highlights email field
  4. Return to Step 5

**E6: System Error**
- **Trigger**: User creation fails (Step 25)
- **Actions**:
  1. System cancels the operation
  2. System logs error details
  3. System displays error: "Failed to create user account. Please try again"
  4. Return to Step 15

**E7: Email Service Failure**
- **Trigger**: Email fails to send (Step 27)
- **Actions**:
  1. System logs email delivery failure
  2. User account is still created (email is not critical)
  3. System displays warning: "User account created but welcome email could not be sent. Please manually provide credentials to user"
  4. System displays created credentials on screen for Admin to copy
  5. Continue with Steps 29-31
  6. Use case ends successfully

**Alternate Flow**:

**A1: Admin Cancels User Creation**
- **Trigger**: Admin clicks "Cancel" button (any step before Step 15)
- **Actions**:
  1. System displays confirmation: "Discard changes?"
  2. If Admin confirms:
     - System clears form
     - System returns to user list
     - No user is created
     - Use case ends
  3. If Admin cancels:
     - Return to current step

**A2: Create Multiple Users**
- **Trigger**: Admin clicks "Save and Add Another" instead of "Create User" (Step 15)
- **Actions**:
  1. Execute Steps 16-28 from basic flow
  2. System displays success notification
  3. System clears form for new entry
  4. System keeps user on creation form instead of returning to list
  5. Return to Step 5 for next user creation
  6. Use case continues until Admin clicks "Done" or "Back to List"

**A3: Assign Multiple Roles**
- **Trigger**: Admin selects "Assign Additional Roles" checkbox (after Step 13)
- **Actions**:
  1. System displays multi-select role dropdown
  2. Admin selects multiple roles for user
  3. System validates at least one role is selected
  4. Continue with Step 14
  5. In Step 24, system creates multiple user-role associations
  6. Use case continues normally

---

### UC-USER-002: Update User Account

**Use Case ID**: UC-USER-002

**Use Case Name**: Update User Account

**Actors**:
- Primary: Admin
- Secondary: System, Email Service

**Preconditions**:
- Admin must be authenticated and authorized
- Admin must have "RequireAdminRole" authorization
- Target user account must exist in database
- User Management interface must be accessible

**Postconditions**:
- User account information is updated
- User receives email notification of changes
- Update event is logged with change history
- If role changed, user permissions are updated immediately
- Admin receives confirmation of successful update

**Basic Flow**:
1. Admin navigates to User Management section
2. System displays list of all users with columns:
   - Email, First Name, Last Name, Role(s), Status, Region, Creation Date, Last Login
3. Admin searches or filters to find target user
4. Admin clicks on user row or "Edit" button for specific user
5. System retrieves user details
6. System displays user edit form pre-populated with current data:
   - Email Address (read-only, display only)
   - First Name (editable)
   - Last Name (editable)
   - Home-Country Phone Number (editable)
   - Resident-Country Phone Number (editable)
   - Country of Origin (editable)
   - Residential Address (editable)
   - Primary Role (editable, dropdown)
   - Region Assignment (editable, dropdown)
   - Account Status (editable, dropdown: Active, Inactive)
7. Admin modifies one or more fields as needed
8. Admin clicks "Save Changes" button
9. System validates modified fields
10. System validates required fields are not empty
11. System validates phone number formats if changed
12. System captures "before" state of user record for audit trail
13. System updates user record with modified fields
14. System updates timestamp to current time
15. System records modifier as Current Admin's User ID
16. If role was changed:
    - System updates user-role associations
    - System refreshes user permissions
17. System saves changes
18. System captures "after" state of user record for audit trail
19. System creates audit log entry with:
    - User ID
    - Admin ID (who made changes)
    - Timestamp
    - Changed fields with before/after values
20. System composes notification email:
    - Subject: "Your DBMS Account Has Been Updated"
    - Body with:
      * Notification of account modification
      * List of fields that were changed (not showing actual values for security)
      * Timestamp of change
      * Who made the change (Admin name)
      * Contact information if user did not authorize changes
21. System sends email to user's email address
22. System displays success notification: "User account updated successfully"
23. System refreshes user list showing updated information
24. System returns Admin to user list view
25. Use case ends successfully

**Exceptional Flow**:

**E1: Empty Required Fields**
- **Trigger**: Required field is cleared/emptied (Step 10)
- **Actions**:
  1. System displays error: "Required fields cannot be empty"
  2. System highlights empty required fields
  3. Return to Step 7

**E2: Invalid Phone Number Format**
- **Trigger**: Phone number format validation fails (Step 11)
- **Actions**:
  1. System displays error: "Invalid phone number format"
  2. System highlights phone number fields
  3. Return to Step 7

**E3: User Not Found**
- **Trigger**: User record doesn't exist (Step 5)
- **Actions**:
  1. System displays error: "User account not found. It may have been deleted"
  2. System returns Admin to user list
  3. System refreshes user list
  4. Use case ends

**E4: Update Failure**
- **Trigger**: User update fails (Step 17)
- **Actions**:
  1. System cancels the operation
  2. System logs error details
  3. System displays error: "Failed to update user account. Please try again"
  4. Return to Step 7

**E5: Concurrent Modification Detected**
- **Trigger**: User was modified by another admin between Step 5 and Step 13
- **Actions**:
  1. System detects modified timestamp changed
  2. System displays error: "This user was modified by another administrator. Please review current data"
  3. System reloads user data with latest values
  4. System highlights fields that were changed by other admin
  5. Return to Step 6

**E6: Email Notification Failure**
- **Trigger**: Email fails to send (Step 21)
- **Actions**:
  1. System logs email delivery failure
  2. User update is still successful
  3. System displays warning: "User updated but notification email could not be sent"
  4. Continue with Steps 22-25
  5. Use case ends successfully

**Alternate Flow**:

**A1: Admin Cancels Edit**
- **Trigger**: Admin clicks "Cancel" button (any step before Step 8)
- **Actions**:
  1. If changes were made, system displays confirmation: "Discard changes?"
  2. If Admin confirms:
     - System discards changes
     - System returns to user list
     - No update occurs
     - Use case ends
  3. If Admin cancels the cancel:
     - Return to Step 7

**A2: Quick Status Toggle**
- **Trigger**: Admin clicks status toggle switch directly in user list (at Step 3)
- **Actions**:
  1. System displays confirmation: "Change user status to [Active/Inactive]?"
  2. Admin confirms
  3. System updates only status field
  4. System skips to Steps 17-25
  5. Use case ends successfully

**A3: Reset Password for User**
- **Trigger**: Admin clicks "Reset Password" button in edit form (after Step 6)
- **Actions**:
  1. System displays password reset dialog
  2. Admin enters new temporary password
  3. System validates password complexity
  4. System securely stores new password
  5. System updates password
  6. System sets first login flag to true
  7. System sends email to user with new temporary password
  8. System displays success message
  9. Return to Step 7 (form remains open for other edits)

**A4: Bulk Update Multiple Users**
- **Trigger**: Admin selects multiple users using checkboxes (at Step 3)
- **Actions**:
  1. Admin clicks "Bulk Actions" dropdown
  2. Admin selects action (e.g., "Change Region", "Change Status")
  3. System displays bulk update dialog
  4. Admin specifies new value(s)
  5. Admin confirms bulk update
  6. System updates all selected users
  7. System sends notification emails to all affected users
  8. System displays summary: "X users updated successfully"
  9. Use case ends

---

### UC-USER-003: Delete User Account

**Use Case ID**: UC-USER-003

**Use Case Name**: Delete User Account

**Actors**:
- Primary: Admin
- Secondary: System, Email Service

**Preconditions**:
- Admin must be authenticated and authorized
- Admin must have "RequireAdminRole" authorization
- Target user account must exist in database
- User Management interface must be accessible

**Postconditions**:
- User account is permanently removed (or deactivated based on policy)
- All user-role associations are removed
- User receives email notification of account deletion
- Deletion event is logged
- Historical data (projects, drill logs, etc.) retains user reference for audit trail
- User can no longer login to system

**Basic Flow**:
1. Admin navigates to User Management section
2. System displays list of all users
3. Admin searches or filters to find target user
4. Admin clicks "Delete" button (trash icon) for specific user
5. System displays confirmation dialog with warning:
   - "Are you sure you want to delete this user account?"
   - User details: Email, Name, Role
   - Warning: "This action cannot be undone"
   - Dropdown: "Reason for deletion" (required)
   - Checkbox: "I understand this user will no longer be able to access the system"
6. Admin selects deletion reason from dropdown
7. Admin checks confirmation checkbox
8. Admin clicks "Confirm Delete" button
9. System validates reason is selected and checkbox is checked
10. System checks if user has critical active assignments:
    - Active projects as assigned user
    - Stores as assigned manager
    - Machines as assigned operator
    - Pending approval requests
11. System retrieves user details for logging
12. System begins deletion process
13. System removes user-role associations
14. System removes user's password reset codes (if any)
15. System updates historical records to retain user reference but mark as deleted:
    - Audit logs retain User ID
    - Created By / Modified By fields retain User ID
    - Project assignments are marked with "[Deleted User]" annotation
16. System removes user record
17. System confirms deletion complete
18. System logs deletion event with:
    - Deleted User ID and Email
    - Admin who performed deletion
    - Timestamp
    - Reason for deletion
19. System composes notification email:
    - Subject: "Your DBMS Account Has Been Deleted"
    - Body with:
      * Notification of account deletion
      * Effective date
      * Reason (if appropriate to share)
      * Contact information for questions
20. System sends email to deleted user's email address
21. System displays success notification: "User account deleted successfully"
22. System refreshes user list (deleted user no longer appears)
23. Use case ends successfully

**Exceptional Flow**:

**E1: Confirmation Not Completed**
- **Trigger**: Admin doesn't select reason or check checkbox (Step 9)
- **Actions**:
  1. System displays error: "Please select a reason and confirm deletion"
  2. System prevents deletion
  3. Return to Step 6

**E2: User Has Critical Active Assignments**
- **Trigger**: User has active assignments that would be orphaned (Step 10)
- **Actions**:
  1. System displays detailed warning with list of assignments:
     - "This user has active assignments:"
     - List of projects (with links)
     - List of stores managed
     - List of machines assigned
     - Count of pending requests
  2. System displays error: "Cannot delete user with active assignments. Please reassign or complete these items first"
  3. System provides "Reassign Resources" button
  4. If Admin clicks "Reassign Resources", open assignment transfer dialog
  5. Otherwise, use case ends

**E3: User Not Found**
- **Trigger**: User was already deleted by another admin (Step 11)
- **Actions**:
  1. System displays error: "User not found. May have been already deleted"
  2. System refreshes user list
  3. Use case ends

**E4: Deletion Failure**
- **Trigger**: Deletion process fails (Step 17)
- **Actions**:
  1. System cancels entire operation
  2. System logs error details
  3. System displays error: "Failed to delete user account. Please try again or contact support"
  4. User account remains unchanged
  5. Use case ends

**E5: Cannot Delete Self**
- **Trigger**: Admin attempts to delete their own account (detected at Step 11)
- **Actions**:
  1. System displays error: "You cannot delete your own account. Please ask another administrator"
  2. System prevents deletion
  3. Use case ends

**E6: Cannot Delete Last Admin**
- **Trigger**: User being deleted is the only active Admin user (Step 11)
- **Actions**:
  1. System displays error: "Cannot delete the last administrator account. Please create another admin user first"
  2. System prevents deletion
  3. Use case ends

**E7: Email Notification Failure**
- **Trigger**: Email fails to send (Step 20)
- **Actions**:
  1. System logs email failure
  2. Deletion is still successful
  3. System displays warning: "User deleted but notification email could not be sent"
  4. Continue with Steps 21-23
  5. Use case ends successfully

**Alternate Flow**:

**A1: Admin Cancels Deletion**
- **Trigger**: Admin clicks "Cancel" in confirmation dialog (after Step 5)
- **Actions**:
  1. System closes confirmation dialog
  2. System returns to user list
  3. No deletion occurs
  4. Use case ends

**A2: Soft Delete (Deactivate Instead)**
- **Trigger**: Admin selects "Deactivate instead of delete" option (at Step 5)
- **Actions**:
  1. System changes dialog to deactivation confirmation
  2. Admin confirms deactivation
  3. Instead of deleting, system updates user status to "Deactivated"
  4. User-role associations remain but user cannot login
  5. System logs deactivation event
  6. System sends "Account Deactivated" email to user
  7. User appears in list with "Deactivated" status
  8. Use case ends successfully

**A3: Bulk Delete Multiple Users**
- **Trigger**: Admin selects multiple users with checkboxes (at Step 3)
- **Actions**:
  1. Admin clicks "Bulk Actions" > "Delete Selected"
  2. System displays bulk deletion confirmation
  3. System lists all selected users
  4. System checks each user for critical assignments
  5. System displays warnings for any users that cannot be deleted
  6. Admin can proceed with deleting only valid users
  7. Admin confirms bulk deletion
  8. System deletes all valid users in single transaction
  9. System displays summary: "X of Y users deleted successfully"
  10. System lists any users that could not be deleted with reasons
  11. Use case ends

---

### UC-USER-004: Search and Filter Users

**Use Case ID**: UC-USER-004

**Use Case Name**: Search and Filter Users

**Actors**:
- Primary: Admin
- Secondary: System

**Preconditions**:
- Admin must be authenticated and authorized
- Admin must have "RequireAdminRole" authorization
- User Management interface must be accessible
- At least one user exists in system

**Postconditions**:
- Filtered/searched user list is displayed
- Filter criteria are retained during session
- Results are paginated for performance
- Search history may be saved for quick access

**Basic Flow**:
1. Admin navigates to User Management section
2. System displays complete list of users (default: first 20 users, sorted by creation date descending)
3. System displays filter panel with options:
   - Role (multi-select dropdown)
   - Account Status (multi-select: Active, Inactive, Locked)
   - Region (multi-select dropdown)
   - Creation Date Range (date picker)
   - Last Login Date Range (date picker)
4. System displays search bar at top with placeholder: "Search by name or email..."
5. Admin enters search text in search bar (e.g., "john" or "john@example.com")
6. System performs real-time search
7. System searches for users where:
   - Email contains search text (case-insensitive), OR
   - First Name contains search text (case-insensitive), OR
   - Last Name contains search text (case-insensitive), OR
   - Full Name (First + Last) contains search text
8. System retrieves matching user records
9. System displays filtered results in user list table
10. System displays result count: "Showing X results"
11. Admin optionally applies additional filters from filter panel
12. Admin selects one or more roles from Role filter
13. System updates query to include role filter
14. System re-executes search with combined criteria (search text AND selected roles)
15. System displays updated filtered results
16. System updates result count
17. Admin can view results paginated (20 per page)
18. Admin can click page numbers or next/previous to navigate pages
19. Admin can export filtered results by clicking "Export" button
20. System generates CSV file with filtered users
21. Use case ends successfully

**Exceptional Flow**:

**E1: No Results Found**
- **Trigger**: No users match search criteria (Step 8)
- **Actions**:
  1. System displays message: "No users found matching your criteria"
  2. System displays suggestions:
     - "Try different search terms"
     - "Clear some filters"
  3. System provides "Clear All Filters" button
  4. If Admin clicks "Clear All", return to Step 2 with all users
  5. Otherwise, use case ends

**E2: Search Error**
- **Trigger**: Search operation fails (Step 8 or 14)
- **Actions**:
  1. System logs error details
  2. System displays error: "Unable to retrieve users. Please try again"
  3. System keeps previous results displayed (if any)
  4. Use case ends

**E3: Invalid Date Range**
- **Trigger**: Admin selects end date before start date (Step 11)
- **Actions**:
  1. System displays validation error: "End date must be after start date"
  2. System highlights date fields in red
  3. System prevents filter application
  4. Return to Step 11

**Alternate Flow**:

**A1: Quick Filter by Role**
- **Trigger**: Admin clicks role badge/chip in interface (at Step 3)
- **Actions**:
  1. System immediately filters to show only users with that role
  2. System updates role filter selection to reflect chosen role
  3. Continue from Step 14
  4. Use case continues

**A2: Save Filter Preset**
- **Trigger**: Admin clicks "Save Filter" button after applying filters (after Step 16)
- **Actions**:
  1. System displays dialog: "Save this filter preset"
  2. Admin enters preset name (e.g., "Active Blasting Engineers")
  3. Admin clicks "Save"
  4. System stores filter criteria with name in user preferences
  5. System displays saved preset in "Saved Filters" dropdown
  6. Admin can load this preset in future sessions
  7. Use case continues

**A3: Load Saved Filter Preset**
- **Trigger**: Admin selects saved filter from dropdown (at Step 3)
- **Actions**:
  1. System retrieves saved filter criteria
  2. System populates all filter fields with saved values
  3. System executes search with loaded criteria
  4. Continue from Step 15
  5. Use case continues

**A4: Sort Results**
- **Trigger**: Admin clicks column header to sort (any time after Step 9)
- **Actions**:
  1. System re-retrieves users sorted by selected column
  2. System toggles sort direction (ascending/descending) on repeated clicks
  3. System displays sort indicator (up/down arrow) in column header
  4. System updates displayed results
  5. Use case continues

**A5: Clear All Filters**
- **Trigger**: Admin clicks "Clear All Filters" button (any time after Step 11)
- **Actions**:
  1. System resets all filter selections to default
  2. System clears search text
  3. System removes all filter chips/tags
  4. System re-queries database for all users
  5. Return to Step 2
  6. Use case continues

**A6: Export Filtered Results**
- **Trigger**: Admin clicks "Export to CSV" or "Export to Excel" (Step 19)
- **Actions**:
  1. System retrieves all users matching current filter (not just current page)
  2. System generates file with columns: Email, First Name, Last Name, Role(s), Status, Region, Creation Date, Last Login
  3. System initiates file download
  4. System displays notification: "Export completed - X users exported"
  5. Use case continues (Admin remains on filtered view)

---

## 5. PROJECT MANAGEMENT USE CASES

---

### UC-PROJECT-001: Create New Project

**Use Case ID**: UC-PROJECT-001

**Use Case Name**: Create New Project

**Actors**:
- Primary: Admin, Blasting Engineer (in General Manager capacity)
- Secondary: System

**Preconditions**:
- User must be authenticated and authorized
- User must have "ReadProjectData" or "RequireAdminRole" authorization
- Project Management interface must be accessible
- At least one region must exist in database

**Postconditions**:
- New project is created in database with status "Planned"
- Project ID is auto-generated
- Project is assigned to specified user (optional)
- Project creation event is logged
- Assigned user receives notification (if specified)
- Project appears in project list

**Basic Flow**:
1. User navigates to Project Management section
2. System displays project dashboard showing:
   - Total projects count
   - Projects by status (Planned, In Progress, On Hold, Completed, Cancelled)
   - Recent projects list
3. User clicks "Create New Project" button
4. System displays project creation form with fields:
   - Project Name (required, text input)
   - Description (required, textarea)
   - Region (required, dropdown)
   - Area of Operation (required, text input)
   - Start Date (optional, date picker)
   - Expected End Date (optional, date picker)
   - Assigned User/Manager (optional, user dropdown with search)
5. User enters project name
6. User enters detailed description of project
7. User selects region from dropdown (populated with all active regions)
8. User enters area of operation (specific location/site area)
9. User optionally selects start date
10. User optionally selects expected end date
11. User optionally searches and selects assigned user/manager
12. User clicks "Create Project" button
13. System validates all required fields are not empty
14. System validates project name format (alphanumeric, spaces, hyphens allowed)
15. System queries database to check if project name already exists
16. System validates project name is unique
17. If end date provided, system validates end date is after start date
18. System begins database transaction
19. System generates unique Project ID (auto-increment or GUID)
20. System creates project record in database with:
    - Generated Project ID
    - Project Name
    - Description
    - Region ID
    - Area of Operation
    - Start Date (if provided)
    - Expected End Date (if provided)
    - Assigned User ID (if provided)
    - Status: "Planned"
    - Created At: current timestamp
    - Created By: current user's ID
21. System commits transaction to database
22. System logs project creation event with:
    - Project ID
    - Project Name
    - Created By User ID
    - Timestamp
23. If assigned user was specified:
    - System creates in-app notification for assigned user
    - Notification message: "You have been assigned to project: [Project Name]"
    - System sends email notification to assigned user (optional)
24. System displays success notification: "Project '[Project Name]' created successfully"
25. System redirects user to project details page for newly created project
26. Use case ends successfully

**Exceptional Flow**:

**E1: Empty Required Fields**
- **Trigger**: One or more required fields are empty (Step 13)
- **Actions**:
  1. System displays validation errors next to empty fields:
     - "Project Name is required"
     - "Description is required"
     - "Region is required"
     - "Area of Operation is required"
  2. System highlights empty fields in red
  3. System displays summary: "Please fill in all required fields"
  4. Return to Step 5

**E2: Invalid Project Name Format**
- **Trigger**: Project name contains invalid characters (Step 14)
- **Actions**:
  1. System displays error: "Project name can only contain letters, numbers, spaces, and hyphens"
  2. System highlights project name field
  3. Return to Step 5

**E3: Duplicate Project Name**
- **Trigger**: Project name already exists in database (Step 16)
- **Actions**:
  1. System displays error: "A project with this name already exists. Please choose a different name"
  2. System highlights project name field
  3. System optionally displays link to existing project: "View existing project"
  4. Return to Step 5

**E4: Invalid Date Range**
- **Trigger**: Expected end date is before start date (Step 17)
- **Actions**:
  1. System displays error: "Expected end date must be after start date"
  2. System highlights both date fields
  3. Return to Step 9

**E5: Database Error**
- **Trigger**: Database transaction fails (Step 21)
- **Actions**:
  1. System rolls back transaction
  2. System logs error details with stack trace
  3. System displays error: "Failed to create project. Please try again or contact support"
  4. Return to Step 12

**E6: Assigned User Not Found**
- **Trigger**: Selected assigned user no longer exists or is deactivated (after Step 11)
- **Actions**:
  1. System displays error: "Selected user is not available. Please choose another user"
  2. System clears assigned user field
  3. Return to Step 11

**Alternate Flow**:

**A1: Cancel Project Creation**
- **Trigger**: User clicks "Cancel" button (any step before Step 12)
- **Actions**:
  1. If form has unsaved changes, system displays confirmation: "Discard changes?"
  2. If user confirms:
     - System clears form
     - System returns to project dashboard
     - No project is created
     - Use case ends
  3. If user cancels the cancel:
     - Return to current step

**A2: Quick Create (Minimal Information)**
- **Trigger**: User clicks "Quick Create" instead of "Create New Project" (at Step 3)
- **Actions**:
  1. System displays simplified form with only required fields:
     - Project Name
     - Region
  2. System auto-fills:
     - Description: "[Project Name] - Drilling and Blasting Project"
     - Area of Operation: "[Selected Region] - TBD"
     - Assigned User: Current user
  3. User enters project name and selects region
  4. User clicks "Create"
  5. Continue from Step 13 with auto-filled values
  6. Use case ends successfully

**A3: Duplicate from Existing Project**
- **Trigger**: User clicks "Duplicate" button on existing project (at Step 3)
- **Actions**:
  1. System displays creation form pre-populated with data from source project:
     - Project Name: "[Original Name] - Copy"
     - Description: copied
     - Region: copied
     - Area of Operation: copied
     - Assigned User: current user (not copied)
  2. System displays info message: "Creating new project based on [Original Project Name]"
  3. User modifies fields as needed (especially Project Name)
  4. Continue from Step 12
  5. Use case ends successfully

**A4: Save as Draft**
- **Trigger**: User clicks "Save as Draft" instead of "Create Project" (at Step 12)
- **Actions**:
  1. System validates only Project Name is required for draft
  2. System creates project with status "Draft" instead of "Planned"
  3. Draft projects are not visible to operators
  4. User can later complete and publish the draft
  5. System displays: "Project saved as draft"
  6. Use case ends successfully

---

### UC-PROJECT-002: Update Project Details

**Use Case ID**: UC-PROJECT-002

**Use Case Name**: Update Project Details

**Actors**:
- Primary: Admin, Blasting Engineer
- Secondary: System

**Preconditions**:
- User must be authenticated and authorized
- User must have "ReadProjectData" authorization
- Target project must exist in database
- User must have permission to edit the project (ownership or admin role)

**Postconditions**:
- Project information is updated in database
- Modified timestamp is updated
- Modifier user is recorded
- Update event is logged with change history
- If assigned user changed, notifications are sent
- Updated project appears in project list

**Basic Flow**:
1. User navigates to Project Management section
2. System displays list of projects
3. User searches or filters to find target project
4. User clicks on project row or "Edit" button for specific project
5. System retrieves project details from database including:
   - All project fields
   - Current status
   - Associated sites count
   - Assigned user information
6. System displays project edit form pre-populated with current data:
   - Project Name (editable)
   - Description (editable)
   - Region (editable, dropdown)
   - Area of Operation (editable)
   - Start Date (editable, date picker)
   - Expected End Date (editable, date picker)
   - Assigned User/Manager (editable, user dropdown)
   - Status (editable, dropdown: Planned, In Progress, On Hold, Completed, Cancelled)
7. User modifies one or more fields as needed
8. User clicks "Save Changes" button
9. System validates all required fields are not empty
10. System validates project name format
11. If project name was changed:
    - System checks if new name already exists for another project
    - System validates new name is unique
12. If end date was changed:
    - System validates end date is after start date
13. System captures "before" state of project for audit trail:
    - Previous values of all fields
14. System begins database transaction
15. System updates project record with modified fields
16. System updates Modified At timestamp to current time
17. System updates Modified By field to current user's ID
18. If assigned user was changed:
    - System retrieves old assigned user ID
    - System retrieves new assigned user ID
    - System prepares notifications for both users
19. System commits transaction to database
20. System captures "after" state of project for audit trail
21. System creates audit log entry with:
    - Project ID
    - User ID (who made changes)
    - Timestamp
    - Changed fields with before/after values
22. If assigned user was changed:
    - System sends notification to new assigned user: "You have been assigned to project: [Project Name]"
    - If old assigned user exists, send notification: "You have been unassigned from project: [Project Name]"
23. System displays success notification: "Project updated successfully"
24. System refreshes project details display with updated information
25. Use case ends successfully

**Exceptional Flow**:

**E1: Empty Required Fields**
- **Trigger**: Required field is cleared/emptied (Step 9)
- **Actions**:
  1. System displays validation errors: "Required fields cannot be empty"
  2. System highlights empty required fields (Project Name, Description, Region, Area)
  3. Return to Step 7

**E2: Invalid Project Name Format**
- **Trigger**: Project name contains invalid characters (Step 10)
- **Actions**:
  1. System displays error: "Project name can only contain letters, numbers, spaces, and hyphens"
  2. System highlights project name field
  3. Return to Step 7

**E3: Duplicate Project Name**
- **Trigger**: New project name conflicts with existing project (Step 11)
- **Actions**:
  1. System displays error: "A project with this name already exists"
  2. System highlights project name field
  3. System shows link to conflicting project
  4. Return to Step 7

**E4: Invalid Date Range**
- **Trigger**: Expected end date is before start date (Step 12)
- **Actions**:
  1. System displays error: "Expected end date must be after start date"
  2. System highlights date fields
  3. Return to Step 7

**E5: Project Not Found**
- **Trigger**: Project was deleted by another user (Step 5)
- **Actions**:
  1. System displays error: "Project not found. It may have been deleted"
  2. System returns user to project list
  3. System refreshes project list
  4. Use case ends

**E6: Database Update Failure**
- **Trigger**: Database transaction fails (Step 19)
- **Actions**:
  1. System rolls back transaction
  2. System logs error details
  3. System displays error: "Failed to update project. Please try again"
  4. Return to Step 7

**E7: Concurrent Modification Detected**
- **Trigger**: Project was modified by another user between Step 5 and Step 15
- **Actions**:
  1. System detects Modified At timestamp changed
  2. System displays warning: "This project was modified by another user. Please review current data"
  3. System reloads project with latest data
  4. System highlights fields changed by other user
  5. User must decide to overwrite or merge changes
  6. Return to Step 6

**E8: Cannot Change Status with Dependencies**
- **Trigger**: User tries to set status to "Completed" but sites are still in progress (Step 9)
- **Actions**:
  1. System validates status change
  2. System displays error: "Cannot mark project as Completed. X sites are still in progress"
  3. System shows list of incomplete sites
  4. System keeps status at current value
  5. Return to Step 7

**Alternate Flow**:

**A1: Cancel Update**
- **Trigger**: User clicks "Cancel" button (any step before Step 8)
- **Actions**:
  1. If changes were made, system displays confirmation: "Discard changes?"
  2. If user confirms:
     - System reverts to original data
     - System returns to project details view (read-only)
     - No update occurs
     - Use case ends
  3. If user cancels the cancel:
     - Return to Step 7

**A2: Quick Status Update**
- **Trigger**: User uses status dropdown directly in project list (at Step 3)
- **Actions**:
  1. System displays status dropdown inline
  2. User selects new status
  3. System performs validation (check dependencies)
  4. System updates only status field
  5. System executes Steps 14-19 for status field only
  6. System displays toast notification: "Status updated"
  7. Use case ends successfully

**A3: Save and Continue Editing**
- **Trigger**: User clicks "Apply" instead of "Save Changes" (at Step 8)
- **Actions**:
  1. Execute Steps 9-21 from basic flow
  2. Display success notification
  3. Keep edit form open with updated data
  4. User can continue making additional changes
  5. Return to Step 7

**A4: View Change History**
- **Trigger**: User clicks "View History" button in edit form (after Step 6)
- **Actions**:
  1. System retrieves all audit log entries for this project
  2. System displays change history in modal/side panel:
     - Date/time of change
     - User who made change
     - Fields that were changed
     - Before and after values
  3. User reviews history
  4. User closes history modal
  5. Return to Step 7

---

### UC-PROJECT-003: Delete Project

**Use Case ID**: UC-PROJECT-003

**Use Case Name**: Delete Project

**Actors**:
- Primary: Admin
- Secondary: System

**Preconditions**:
- User must be authenticated with Admin role
- User must have "RequireAdminRole" authorization
- Target project must exist in database
- Project Management interface must be accessible

**Postconditions**:
- Project is removed from database (if no dependencies)
- All associated sites are also deleted (cascade delete)
- All drill data, blast data for those sites are deleted
- Deletion event is logged
- Assigned users are notified
- Project no longer appears in project list

**Basic Flow**:
1. Admin navigates to Project Management section
2. System displays list of projects
3. Admin searches or filters to find target project
4. Admin clicks "Delete" button (trash icon) for specific project
5. System checks for project dependencies:
   - Number of associated sites
   - Number of drill holes across all sites
   - Number of blast connections
   - Number of assigned machines
   - Number of assigned operators
6. System retrieves dependency counts
7. System displays detailed confirmation dialog:
   - "Are you sure you want to delete this project?"
   - Project details: Project Name, Region, Status
   - Warning section showing:
     * "This project has X site(s)"
     * "This project has X drill hole(s)"
     * "This project has X blast connection(s)"
     * "This project has X assigned machine(s)"
     * "This project has X assigned operator(s)"
   - Warning: "All associated data will be permanently deleted. This action cannot be undone"
   - Dropdown: "Reason for deletion" (required)
   - Text input: "Type project name to confirm" (required)
   - Checkbox: "I understand all data will be permanently deleted"
8. Admin selects deletion reason from dropdown
9. Admin types exact project name in confirmation field
10. Admin checks confirmation checkbox
11. Admin clicks "Confirm Delete" button
12. System validates reason is selected
13. System validates typed project name matches exactly
14. System validates checkbox is checked
15. System retrieves project details and all associated data for logging
16. System begins database transaction
17. System deletes in order (cascade):
    a. Blast connections for all project sites
    b. Detonator info records
    c. Site blasting data
    d. Drill points for all drill holes
    e. Drill holes for all project sites
    f. Explosive calculation results
    g. Explosive approval requests for project sites
    h. Project sites
    i. Machine assignments to project
    j. Project record itself
18. System commits transaction to database
19. System logs project deletion event with:
    - Deleted Project ID, Project Name, Region
    - Count of cascaded deletions (sites, drill holes, etc.)
    - Admin who performed deletion
    - Timestamp
    - Reason for deletion
20. System creates notifications for affected users:
    - Assigned user (if any): "Project [Name] has been deleted"
    - Operators assigned to project machines: "Project [Name] has been deleted. Your machine assignment has been updated"
21. System displays success notification: "Project '[Project Name]' and all associated data deleted successfully"
22. System displays deletion summary:
    - "Deleted X site(s)"
    - "Deleted X drill hole(s)"
    - "Deleted X blast connection(s)"
23. System refreshes project list (deleted project no longer appears)
24. Use case ends successfully

**Exceptional Flow**:

**E1: Confirmation Not Completed**
- **Trigger**: Admin doesn't complete all confirmation requirements (Step 12, 13, or 14)
- **Actions**:
  1. System displays specific errors:
     - "Please select a reason for deletion"
     - "Please type the project name exactly to confirm"
     - "Please check the confirmation checkbox"
  2. System highlights incomplete fields
  3. System prevents deletion
  4. Return to Step 8

**E2: Project Name Mismatch**
- **Trigger**: Typed project name doesn't match exactly (Step 13)
- **Actions**:
  1. System displays error: "Project name doesn't match. Please type '[Exact Project Name]' to confirm"
  2. System highlights text input field
  3. System keeps dialog open
  4. Return to Step 9

**E3: Project Not Found**
- **Trigger**: Project was already deleted by another admin (Step 15)
- **Actions**:
  1. System displays error: "Project not found. It may have been already deleted"
  2. System closes confirmation dialog
  3. System refreshes project list
  4. Use case ends

**E4: Database Deletion Failure**
- **Trigger**: Database transaction fails during cascade delete (Step 18)
- **Actions**:
  1. System rolls back entire transaction
  2. System logs error details with specific point of failure
  3. System displays error: "Failed to delete project. Database error occurred. Please try again or contact support"
  4. Project and all associated data remain unchanged
  5. Use case ends

**E5: Foreign Key Constraint Violation**
- **Trigger**: Unexpected foreign key constraint prevents deletion (Step 17)
- **Actions**:
  1. System catches foreign key violation exception
  2. System rolls back transaction
  3. System logs error with constraint details
  4. System displays error: "Cannot delete project due to external references. Please contact support"
  5. Use case ends

**Alternate Flow**:

**A1: Admin Cancels Deletion**
- **Trigger**: Admin clicks "Cancel" in confirmation dialog (after Step 7)
- **Actions**:
  1. System closes confirmation dialog
  2. System returns to project list
  3. No deletion occurs
  4. Use case ends

**A2: Archive Instead of Delete**
- **Trigger**: Admin selects "Archive this project instead" option (at Step 7)
- **Actions**:
  1. System changes dialog to archive confirmation
  2. System explains: "Archiving will hide the project but preserve all data"
  3. Admin confirms archiving
  4. System updates project status to "Archived"
  5. System sets IsArchived flag to true
  6. Project disappears from active project list but remains in database
  7. System displays: "Project archived successfully. View in Archives section"
  8. Use case ends successfully

**A3: Export Before Delete**
- **Trigger**: Admin clicks "Export Project Data" in confirmation dialog (after Step 7)
- **Actions**:
  1. System generates comprehensive export of:
     - Project details
     - All sites
     - All drill data
     - All blast connections
     - All calculations
  2. System creates ZIP file with CSV/JSON files
  3. System initiates download
  4. System displays: "Export completed. You can now proceed with deletion"
  5. Return to Step 8 (admin can continue with deletion)

**A4: Soft Delete with Recovery Option**
- **Trigger**: System is configured for soft-delete policy (Step 16)
- **Actions**:
  1. Instead of physical deletion, system marks project as deleted:
     - Sets IsDeleted flag to true
     - Sets DeletedAt timestamp
     - Sets DeletedBy to current admin's ID
  2. System marks all associated records as deleted (cascade soft-delete)
  3. Project disappears from normal views
  4. Admin can recover from "Deleted Projects" section within 30 days
  5. After 30 days, automated process permanently deletes
  6. System displays: "Project moved to Deleted Projects. Can be recovered within 30 days"
  7. Use case ends successfully

---

## 6. DRILLING OPERATIONS USE CASES

---

### UC-DRILL-001: Import Drill Data from CSV

**Use Case ID**: UC-DRILL-001

**Use Case Name**: Import Drill Data from CSV File

**Actors**:
- Primary: Blasting Engineer, Operator
- Secondary: System

**Preconditions**:
- User must be authenticated and authorized
- User must have "ManageDrillData" authorization
- Target project and site must exist and be accessible to user
- CSV file must be prepared with required columns
- Drilling Operations interface must be accessible

**Postconditions**:
- Drill holes and drill points are created in bulk from CSV data
- All imported data is validated and stored in database
- Invalid rows are reported to user with specific error messages
- Import event is logged with file details and row counts
- Explosive quantities are automatically calculated for each drill point
- Imported drill data is visible in drill plan view

**Basic Flow**:
1. User navigates to Drilling Operations section
2. System displays drill plan management interface
3. User selects target Project from dropdown
4. System loads sites for selected project
5. User selects target Site from dropdown
6. System displays drill data table for selected site (currently empty or with existing drill holes)
7. User clicks "Import CSV" button
8. System displays CSV import dialog with:
   - File upload area (drag-and-drop or browse)
   - Download CSV template button
   - Expected column format display:
     * Required columns: Hole ID, X, Y, Depth
     * Optional columns: Spacing, Burden, Diameter, Stemming, Subdrill, Volume, ANFO, Emulsion
   - File size limit notice (e.g., "Max 10MB")
9. User clicks "Download Template" to see sample format (optional)
10. System downloads CSV template file with headers and sample row
11. User prepares CSV file with drill data following template format
12. User drags and drops CSV file onto upload area (or clicks browse to select file)
13. System validates file:
    - File format is CSV (.csv extension)
    - File size is within limit (10MB)
    - File is not empty
14. System reads file contents
15. System displays file preview showing:
    - Filename
    - File size
    - Number of rows detected
    - First 5 rows preview in table format
16. User reviews preview
17. User clicks "Validate and Import" button
18. System begins validation process:
    - Displays progress indicator: "Validating X of Y rows..."
19. System parses CSV file line by line (starting from row 2, row 1 is headers)
20. For each row, system validates:
    a. Hole ID is present and not empty
    b. X coordinate is present and numeric
    c. Y coordinate is present and numeric
    d. Depth is present and numeric, within range (0-50 meters)
    e. If Spacing provided: numeric, within range (1-20 meters)
    f. If Burden provided: numeric, within range (1-20 meters)
    g. If Diameter provided: numeric, within range (50-500 mm)
    h. If Stemming provided: numeric, within range (0-10 meters)
    i. If Subdrill provided: numeric
21. System collects validation errors with row numbers
22. If validation errors found:
    - Jump to Exceptional Flow E2
23. If all rows valid:
    - System displays validation success summary:
      * "All X rows validated successfully"
      * Summary statistics: Min/Max/Avg for Depth, Spacing, Burden
    - System displays "Proceed with Import" button
24. User clicks "Proceed with Import"
25. System begins database transaction
26. System displays progress: "Importing X of Y rows..."
27. For each validated row:
    a. System creates or retrieves DrillHole record by Hole ID
    b. System creates DrillPoint record with:
       - Project ID (from selection)
       - Project Site ID (from selection)
       - X, Y coordinates
       - Depth, Spacing, Burden, Diameter, Stemming, Subdrill
       - IsCompleted: false
    c. If Volume not provided in CSV, system calculates:
       - Volume = π × (Diameter/2)² × Depth
    d. If ANFO not provided in CSV, system calculates using formula based on Volume, Burden, Spacing
    e. If Emulsion not provided in CSV, system calculates similarly
    f. System saves DrillPoint with calculated explosive quantities
28. System commits transaction to database
29. System logs import event with:
    - User ID
    - Project ID, Site ID
    - Filename
    - Number of rows imported
    - Timestamp
30. System displays success notification:
    - "Successfully imported X drill points from CSV"
    - Shows breakdown: "X new drill holes created"
31. System closes import dialog
32. System refreshes drill data table showing newly imported drill points
33. System displays drill points on 2D pattern canvas
34. Use case ends successfully

**Exceptional Flow**:

**E1: Invalid File Format or Size**
- **Trigger**: File validation fails (Step 13)
- **Actions**:
  1. System displays error based on issue:
     - "Please upload a CSV file (.csv format only)"
     - "File size exceeds 10MB limit. Please reduce data or split into multiple files"
     - "File appears to be empty"
  2. System rejects file upload
  3. Return to Step 12

**E2: Validation Errors Found**
- **Trigger**: One or more rows have validation errors (Step 22)
- **Actions**:
  1. System displays validation error report in scrollable panel:
     - "Validation failed. X errors found in Y rows"
     - Table with columns: Row Number | Field | Error Message
     - Example errors:
       * "Row 5: Depth - Value '75' exceeds maximum of 50 meters"
       * "Row 8: X Coordinate - Missing required value"
       * "Row 12: Diameter - Value 'abc' is not a valid number"
  2. System provides "Export Error Report" button
  3. System provides "Edit and Re-upload" button
  4. System cancels import (no data is saved)
  5. If user clicks "Export Error Report":
     - System generates CSV file with error details
     - User can fix issues in spreadsheet application
  6. If user clicks "Edit and Re-upload":
     - Return to Step 12
  7. Use case ends

**E3: Missing Required Columns**
- **Trigger**: CSV file doesn't have required column headers (Step 20)
- **Actions**:
  1. System displays error: "CSV file is missing required columns: [list of missing columns]"
  2. System shows expected format: "Required columns are: Hole ID, X, Y, Depth"
  3. System suggests downloading template
  4. System cancels import
  5. Return to Step 12

**E4: Database Import Failure**
- **Trigger**: Database transaction fails during import (Step 28)
- **Actions**:
  1. System rolls back entire transaction (no partial import)
  2. System logs error details with row number where failure occurred
  3. System displays error: "Import failed at row X. No data was imported. Please try again or contact support"
  4. System provides "View Error Details" button for technical users
  5. Use case ends

**E5: Duplicate Hole IDs**
- **Trigger**: CSV contains duplicate Hole IDs (detected during validation)
- **Actions**:
  1. System identifies all duplicate Hole IDs
  2. System displays error: "Duplicate Hole IDs found in CSV"
  3. System lists duplicate IDs with row numbers:
     - "Hole ID 'DH-001' appears in rows: 5, 12, 23"
  4. System asks user: "Allow duplicate Hole IDs? (Multiple drill points will be created for each hole)"
  5. If user confirms:
     - Continue with import (multiple DrillPoints per DrillHole)
  6. If user cancels:
     - Return to Step 12 for user to fix duplicates
  7. Jump to Step 24

**E6: Project or Site Not Selected**
- **Trigger**: User attempts import without selecting project/site (Step 7)
- **Actions**:
  1. System disables "Import CSV" button
  2. System displays tooltip: "Please select a project and site first"
  3. Return to Step 3

**Alternate Flow**:

**A1: User Cancels Import**
- **Trigger**: User clicks "Cancel" in import dialog (any step before Step 24)
- **Actions**:
  1. If file was uploaded, system displays confirmation: "Cancel import? Uploaded file will be discarded"
  2. If user confirms cancellation:
     - System closes import dialog
     - No data is imported
     - Use case ends
  3. If user cancels the cancel:
     - Return to current step

**A2: Update Existing Drill Points**
- **Trigger**: System detects some Hole IDs in CSV already exist in database (Step 27)
- **Actions**:
  1. Before starting import, system displays warning:
     - "X Hole IDs in CSV already exist in this site"
     - "Do you want to: [Update existing] or [Skip existing] or [Cancel import]"
  2. If user selects "Update existing":
     - System updates existing DrillPoint records instead of creating new
     - System recalculates explosive quantities based on new parameters
  3. If user selects "Skip existing":
     - System only imports new Hole IDs, skips existing ones
     - System displays summary: "Imported X new points, skipped Y existing"
  4. If user selects "Cancel":
     - Return to Step 12
  5. Continue with import based on user choice

**A3: Import with Auto-Generation of Missing Values**
- **Trigger**: CSV has some optional columns empty (Step 20)
- **Actions**:
  1. For rows with missing optional values, system applies defaults:
     - Spacing: Use default from site pattern settings or 3.0 meters
     - Burden: Use default from site pattern settings or 3.0 meters
     - Diameter: Use default from site pattern settings or 115 mm
     - Stemming: Use default from site pattern settings or 3.0 meters
     - Subdrill: 0.5 meters (default)
  2. System displays info message: "X rows had missing optional values. Defaults were applied"
  3. System provides "Review Defaults" button to see applied values
  4. Continue with Step 26
  5. Use case ends successfully

**A4: Large File Batch Processing**
- **Trigger**: CSV file has more than 1000 rows (Step 19)
- **Actions**:
  1. System displays message: "Large file detected (X rows). Import will be processed in batches"
  2. System processes in batches of 500 rows at a time
  3. System shows progress bar: "Processing batch X of Y..."
  4. System commits each batch to database separately
  5. If any batch fails, previous batches are retained (partial import allowed)
  6. System displays summary: "Imported X of Y rows successfully. Z rows failed"
  7. System provides error report for failed rows
  8. Use case ends (partial success)

---

### UC-DRILL-002: Create Drill Point Manually

**Use Case ID**: UC-DRILL-002

**Use Case Name**: Create Drill Point Manually

**Actors**:
- Primary: Blasting Engineer, Operator
- Secondary: System

**Preconditions**:
- User must be authenticated and authorized
- User must have "ManageDrillData" authorization
- Target project and site must exist and be accessible
- Drilling Operations interface must be accessible

**Postconditions**:
- New drill hole and drill point are created in database
- Explosive quantities are automatically calculated
- Drill point appears in drill data table and 2D canvas
- Creation event is logged
- Drill point is available for blast sequence design

**Basic Flow**:
1. User navigates to Drilling Operations section
2. User selects target Project from dropdown
3. System loads sites for selected project
4. User selects target Site from dropdown
5. System displays drill plan interface with:
   - Drill data table listing existing drill points
   - 2D pattern canvas showing drill point positions
   - "Add Drill Point" button
6. User clicks "Add Drill Point" button
7. System displays drill point creation form with fields:
   - Hole Number (required, text input)
   - X Coordinate (required, number input, meters)
   - Y Coordinate (required, number input, meters)
   - Depth (required, number input, meters, range: 0-50)
   - Spacing (optional, number input, meters, range: 1-20, default: 3.0)
   - Burden (optional, number input, meters, range: 1-20, default: 3.0)
   - Diameter (optional, number input, millimeters, range: 50-500, default: 115)
   - Stemming (optional, number input, meters, range: 0-10, default: 3.0)
   - Subdrill (optional, number input, meters, default: 0.5)
8. User enters hole number (e.g., "DH-001")
9. User enters X coordinate (e.g., 100.5)
10. User enters Y coordinate (e.g., 250.3)
11. User enters depth (e.g., 12.0 meters)
12. User optionally enters spacing (or accepts default)
13. User optionally enters burden (or accepts default)
14. User optionally enters diameter (or accepts default)
15. User optionally enters stemming (or accepts default)
16. User optionally enters subdrill (or accepts default)
17. User clicks "Calculate and Preview" button
18. System validates all required fields are not empty
19. System validates all numeric values are within acceptable ranges
20. System calculates volume based on formula:
    - Volume = π × (Diameter/2)² × Depth
21. System calculates ANFO quantity based on:
    - ANFO (kg) = Volume × Burden × Spacing × ANFO_density × factor
    - (using configured ANFO density and calculation factor)
22. System calculates Emulsion quantity similarly
23. System displays calculation preview in form:
    - "Calculated Volume: X.XX m³"
    - "Calculated ANFO: X.XX kg"
    - "Calculated Emulsion: X.XX kg"
24. System displays drill point position preview on 2D canvas (temporary marker)
25. User reviews calculated values and position
26. User clicks "Create Drill Point" button
27. System begins database transaction
28. System checks if DrillHole with this Hole Number already exists for this site
29. If DrillHole doesn't exist:
    - System creates DrillHole record with:
      * Hole Number
      * Project ID
      * Project Site ID
      * Created At timestamp
      * Created By user ID
30. System creates DrillPoint record with:
    - Drill Hole ID (from step 29 or existing)
    - Project ID
    - Project Site ID
    - X, Y coordinates
    - Depth, Spacing, Burden, Diameter, Stemming, Subdrill
    - Volume (calculated)
    - ANFO (calculated)
    - Emulsion (calculated)
    - IsCompleted: false
    - Created At timestamp
    - Created By user ID
31. System commits transaction to database
32. System logs drill point creation event with user ID and timestamp
33. System displays success notification: "Drill point created successfully"
34. System closes creation form
35. System refreshes drill data table showing new drill point
36. System renders new drill point on 2D canvas as circle marker at (X, Y) position
37. System updates drill plan statistics:
    - Total drill points count
    - Total depth sum
    - Total explosives required
38. Use case ends successfully

**Exceptional Flow**:

**E1: Empty Required Fields**
- **Trigger**: One or more required fields are empty (Step 18)
- **Actions**:
  1. System displays validation errors next to empty fields:
     - "Hole Number is required"
     - "X Coordinate is required"
     - "Y Coordinate is required"
     - "Depth is required"
  2. System highlights empty fields in red
  3. Return to Step 8

**E2: Invalid Numeric Values**
- **Trigger**: Numeric values are outside acceptable ranges (Step 19)
- **Actions**:
  1. System displays specific validation errors:
     - "Depth must be between 0 and 50 meters" (if Depth > 50 or < 0)
     - "Diameter must be between 50 and 500 millimeters"
     - "Spacing must be between 1 and 20 meters"
     - "Burden must be between 1 and 20 meters"
     - "Stemming must be between 0 and 10 meters"
  2. System highlights invalid fields
  3. Return to relevant step (11, 12, 13, 14, or 15)

**E3: Non-Numeric Input**
- **Trigger**: User enters non-numeric characters in number fields (any step 9-16)
- **Actions**:
  1. System displays error: "Please enter a valid number"
  2. System highlights invalid field
  3. System focuses on invalid field
  4. Return to relevant step

**E4: Database Insert Failure**
- **Trigger**: Database transaction fails (Step 31)
- **Actions**:
  1. System rolls back transaction
  2. System logs error details
  3. System displays error: "Failed to create drill point. Please try again"
  4. System keeps form open with entered data
  5. Return to Step 26 (user can retry)

**E5: Duplicate Hole Number**
- **Trigger**: User wants to add another point to existing hole (Step 28)
- **Actions**:
  1. System detects existing DrillHole with same Hole Number
  2. System displays info message: "Adding drill point to existing hole: [Hole Number]"
  3. System skips DrillHole creation (Step 29)
  4. System continues with DrillPoint creation (Step 30)
  5. Use case continues successfully
  - Note: This is actually normal behavior, not an error

**E6: Project or Site Not Selected**
- **Trigger**: User clicks "Add Drill Point" without selecting project/site (Step 6)
- **Actions**:
  1. System disables "Add Drill Point" button
  2. System displays tooltip: "Please select a project and site first"
  3. Return to Step 2

**Alternate Flow**:

**A1: Cancel Drill Point Creation**
- **Trigger**: User clicks "Cancel" button (any step before Step 26)
- **Actions**:
  1. If form has entered data, system displays confirmation: "Discard drill point?"
  2. If user confirms:
     - System closes creation form
     - System removes temporary preview marker from canvas
     - No drill point is created
     - Use case ends
  3. If user cancels the cancel:
     - Return to current step

**A2: Create Multiple Drill Points (Quick Add)**
- **Trigger**: User clicks "Save and Add Another" instead of "Create Drill Point" (Step 26)
- **Actions**:
  1. Execute Steps 27-32 to create drill point
  2. System displays success notification
  3. System clears form except for Spacing, Burden, Diameter, Stemming (keeps as defaults for next point)
  4. System increments Hole Number automatically (e.g., DH-001 → DH-002)
  5. System keeps form open for next drill point
  6. Return to Step 9
  7. User can continue adding multiple drill points quickly

**A3: Click on Canvas to Set Coordinates**
- **Trigger**: User clicks "Pick on Canvas" button next to X/Y fields (after Step 7)
- **Actions**:
  1. System enables canvas click mode
  2. System displays instruction: "Click on canvas to set X, Y coordinates"
  3. System changes cursor to crosshair
  4. User clicks desired position on canvas
  5. System calculates clicked position coordinates
  6. System populates X and Y fields with clicked coordinates
  7. System disables canvas click mode
  8. System displays temporary marker at clicked position
  9. Continue from Step 11 (X and Y are now filled)

**A4: Manual Override of Calculated Values**
- **Trigger**: User clicks "Override Calculations" checkbox (after Step 23)
- **Actions**:
  1. System enables manual editing of Volume, ANFO, and Emulsion fields
  2. System displays warning icon: "Manual values will override automatic calculations"
  3. User enters custom values for explosives
  4. System validates manual values are non-negative
  5. System uses manual values instead of calculated values
  6. Continue from Step 26
  7. System logs that manual override was used

**A5: Use Pattern Template**
- **Trigger**: User clicks "Apply Pattern Template" button (at Step 6)
- **Actions**:
  1. System displays saved pattern templates
  2. User selects a template (e.g., "Standard 3x3 Pattern")
  3. System loads template default values into form:
     - Spacing, Burden, Diameter, Stemming, Subdrill
  4. User only needs to enter Hole Number, X, Y, and Depth
  5. Continue from Step 8
  6. Use case continues with template defaults applied

---

(Continuing with more use cases...)

## 7. BLASTING OPERATIONS USE CASES

---

### UC-BLAST-001: Design Blast Sequence

**Use Case ID**: UC-BLAST-001

**Use Case Name**: Design Blast Sequence (Create Blast Connections)

**Actors**:
- Primary: Blasting Engineer
- Secondary: System

**Preconditions**:
- User must be authenticated and authorized
- User must have "ManageDrillData" authorization
- Target project and site must exist with drill points already created
- At least 2 drill points must exist in the site for creating connections
- Blasting Operations interface must be accessible

**Postconditions**:
- Blast connections between drill points are created and stored
- Detonation sequence and delays are defined
- Starting hole(s) are designated
- Blast sequence is ready for simulation
- Connection creation event is logged
- Blast plan is saved for execution

**Basic Flow**:
1. User navigates to Blasting Operations section
2. User selects target Project from dropdown
3. System loads sites for selected project
4. User selects target Site from dropdown
5. System retrieves all drill points for selected site from database
6. System displays Blast Sequence Designer interface with:
   - 2D canvas showing all drill points as circles at their (X, Y) positions
   - Drill point list panel showing all points with coordinates
   - Sequence design toolbar with:
     * "Add Connection" mode button
     * "Set Starting Hole" button
     * "Delete Connection" button
     * Connector Type selector (Detonating Cord / Connectors)
     * Delay input field (milliseconds)
     * Sequence number display
   - Connection list panel (initially empty)
7. User clicks "Add Connection" mode button
8. System enters connection creation mode
9. System changes cursor to crosshair
10. System displays instruction: "Click two drill points to create connection"
11. User clicks first drill point on canvas (Point 1)
12. System highlights selected Point 1 with distinct color
13. System displays temporary line following mouse cursor from Point 1
14. User clicks second drill point on canvas (Point 2)
15. System validates Points 1 and 2 are different
16. System displays connection creation dialog with fields:
    - Point 1: [Hole Number, X, Y] (read-only, auto-filled)
    - Point 2: [Hole Number, X, Y] (read-only, auto-filled)
    - Connector Type (dropdown: Detonating Cord, Connectors) (required)
    - Delay (milliseconds) (required, number input, range: 0-10000)
    - Sequence Number (auto-calculated based on existing connections)
    - Mark as Starting Hole (checkbox for Point 1 or Point 2)
17. User selects Connector Type from dropdown (e.g., "Detonating Cord")
18. User enters Delay in milliseconds (e.g., 25 ms)
19. System auto-calculates Sequence Number:
    - Finds max existing sequence number
    - New sequence = max + 1
    - Displays: "Sequence Number: X"
20. User optionally checks "Mark Point 1 as Starting Hole" if this is initiation point
21. User clicks "Create Connection" button
22. System validates Connector Type is selected
23. System validates Delay is within range (0-10000 ms)
24. System begins database transaction
25. System creates BlastConnection record with:
    - Project ID
    - Project Site ID
    - Point1 ID (from step 11)
    - Point2 ID (from step 14)
    - Connector Type (from step 17)
    - Delay (milliseconds, from step 18)
    - Sequence Number (from step 19)
    - IsStartingHole flag (from step 20)
    - Created At timestamp
    - Created By user ID
26. System commits transaction to database
27. System logs connection creation event
28. System closes connection dialog
29. System renders connection on canvas as line between Point 1 and Point 2:
    - Line color based on connector type
    - Detonating Cord: red solid line
    - Connectors: blue dashed line
    - Line annotated with delay time and sequence number
30. System adds connection to Connection List panel showing:
    - Sequence Number
    - Point 1 Hole Number → Point 2 Hole Number
    - Connector Type
    - Delay (ms)
    - Edit and Delete buttons
31. System exits connection creation mode
32. System displays success notification: "Blast connection created"
33. User can repeat steps 7-32 to create additional connections
34. When all connections are created, user clicks "Save Blast Plan" button
35. System validates blast plan:
    - At least one starting hole is designated
    - All drill points are connected in sequence (no orphaned points)
    - Sequence numbers form logical progression
36. System updates site record:
    - Sets IsPatternApproved = true (if not already set)
    - Updates Modified At timestamp
37. System displays success notification: "Blast plan saved successfully"
38. Use case ends successfully

**Exceptional Flow**:

**E1: Same Drill Point Selected Twice**
- **Trigger**: User clicks same drill point for both Point 1 and Point 2 (Step 15)
- **Actions**:
  1. System detects Point 1 ID == Point 2 ID
  2. System displays error: "Cannot create connection to the same drill point"
  3. System clears selection
  4. Return to Step 11

**E2: Connection Already Exists**
- **Trigger**: Connection between these two points already exists (Step 24)
- **Actions**:
  1. System queries existing connections
  2. System detects duplicate (Point1-Point2 or Point2-Point1)
  3. System displays error: "Connection already exists between these drill points"
  4. System highlights existing connection on canvas
  5. User can choose to:
     - Edit existing connection (go to UC-BLAST-002)
     - Cancel and select different points
  6. Return to Step 11

**E3: Empty Required Fields**
- **Trigger**: Connector Type not selected or Delay not entered (Step 22-23)
- **Actions**:
  1. System displays validation errors:
     - "Connector Type is required"
     - "Delay is required"
  2. System highlights empty fields
  3. Return to Step 17

**E4: Invalid Delay Value**
- **Trigger**: Delay is outside acceptable range (Step 23)
- **Actions**:
  1. System displays error: "Delay must be between 0 and 10,000 milliseconds"
  2. System highlights delay field
  3. Return to Step 18

**E5: No Drill Points in Site**
- **Trigger**: Site has no drill points (Step 5)
- **Actions**:
  1. System displays message: "No drill points found for this site. Please create drill points first"
  2. System provides "Go to Drill Plan" button
  3. System disables Blast Sequence Designer
  4. Use case ends

**E6: Database Insert Failure**
- **Trigger**: Database transaction fails (Step 26)
- **Actions**:
  1. System rolls back transaction
  2. System logs error details
  3. System displays error: "Failed to create blast connection. Please try again"
  4. Return to Step 21 (user can retry)

**E7: Invalid Blast Plan**
- **Trigger**: Blast plan validation fails (Step 35)
- **Actions**:
  1. System displays validation errors with details:
     - "No starting hole designated. Please mark at least one hole as starting point"
     - "X drill points are not connected to any sequence"
     - "Sequence numbers have gaps or duplicates"
  2. System highlights problematic drill points and connections on canvas
  3. System prevents saving until issues resolved
  4. User must fix issues and try saving again
  5. Return to Step 7 (user continues editing)

**Alternate Flow**:

**A1: Cancel Connection Creation**
- **Trigger**: User presses ESC key or clicks "Cancel" during connection creation (any step 11-20)
- **Actions**:
  1. System exits connection creation mode
  2. System clears selected Point 1 (if any)
  3. System removes temporary line
  4. System returns to normal mode
  5. Return to Step 7

**A2: Quick Connection with Default Values**
- **Trigger**: User holds SHIFT key while clicking Point 2 (Step 14)
- **Actions**:
  1. System creates connection immediately without showing dialog
  2. System uses default values:
     - Connector Type: Detonating Cord (default)
     - Delay: 25 ms (default)
     - Sequence: auto-calculated
     - IsStartingHole: false
  3. Execute Steps 24-32 directly
  4. System immediately returns to connection creation mode for next connection
  5. Return to Step 11

**A3: Bidirectional Connection**
- **Trigger**: User checks "Create Bidirectional Connection" (in Step 16 dialog)
- **Actions**:
  1. System creates two connections:
     - Point1 → Point2 with entered delay
     - Point2 → Point1 with same delay + offset (e.g., +5 ms)
  2. Both connections are saved in database
  3. Both connections are rendered on canvas
  4. Use case continues successfully

**A4: Auto-Sequence All Drill Points**
- **Trigger**: User clicks "Auto-Generate Sequence" button (at Step 6)
- **Actions**:
  1. System displays auto-sequence dialog with options:
     - Pattern Type: Row-by-row, Column-by-column, Spiral, Custom
     - Starting Point: dropdown to select drill point
     - Default Delay: milliseconds between each detonation
     - Connector Type: Detonating Cord or Connectors
  2. User selects options and clicks "Generate"
  3. System calculates optimal connection path based on pattern
  4. System creates all connections automatically:
     - Connects each point to next in sequence
     - Assigns sequential numbers
     - Applies specified delay
  5. System displays preview of generated sequence
  6. User can accept or reject:
     - If accepted: all connections are saved to database
     - If rejected: connections are discarded, return to manual mode
  7. If accepted, continue from Step 34

**A5: Edit Connection Inline**
- **Trigger**: User double-clicks existing connection line on canvas (after Step 29)
- **Actions**:
  1. System displays connection details in edit mode
  2. User can modify:
     - Connector Type
     - Delay
     - Sequence Number (with validation)
  3. User clicks "Update"
  4. System validates and updates connection in database
  5. System re-renders connection with updated values
  6. Return to Step 7

**A6: Set Multiple Starting Holes**
- **Trigger**: User needs multiple initiation points (Step 20)
- **Actions**:
  1. User creates first connection and marks Point 1 as starting hole
  2. User creates another connection and marks its Point 1 as starting hole
  3. System allows multiple starting holes (for complex blast patterns)
  4. During simulation, all starting holes detonate simultaneously at t=0
  5. Use case continues normally

---

### UC-BLAST-002: Simulate Blast Sequence

**Use Case ID**: UC-BLAST-002

**Use Case Name**: Simulate Blast Sequence

**Actors**:
- Primary: Blasting Engineer
- Secondary: System

**Preconditions**:
- User must be authenticated and authorized
- User must have "ManageDrillData" authorization
- Target project and site must have blast sequence designed (connections created)
- At least one starting hole must be designated
- Blast Simulation interface must be accessible

**Postconditions**:
- Blast simulation is executed and visualized
- User can view detonation sequence animation
- Simulation can be confirmed/approved
- Site IsSimulationConfirmed flag is updated
- Simulation review event is logged

**Basic Flow**:
1. User navigates to Blasting Operations section
2. User selects target Project and Site
3. System retrieves all drill points and blast connections for site
4. System displays Blast Sequence Simulator interface with:
   - 2D canvas showing drill points and connections
   - Timeline panel showing sequence of detonations
   - Playback controls (Play, Pause, Reset, Speed)
   - Time display showing current time in milliseconds
   - Legend showing color coding for detonation states
5. System calculates blast timeline:
   - Identifies all starting holes (IsStartingHole = true)
   - Builds detonation tree from starting holes
   - Calculates detonation time for each drill point based on delays
   - Orders drill points by detonation time
6. System displays timeline with all drill points positioned by detonation time
7. User clicks "Play Simulation" button
8. System begins animation:
   - Current time counter starts at 0ms
   - System highlights starting holes in orange (at t=0)
9. System animates detonation sequence:
   - For each drill point at its calculated time:
     * Point changes color to red (detonating)
     * Point pulses/expands briefly
     * Connection lines to next points highlight
     * After 100ms, point changes to gray (detonated)
10. System displays current detonation information panel:
    - Hole Number currently detonating
    - Current time (ms)
    - Next holes to detonate with countdown
11. System progresses through entire sequence
12. When simulation completes:
    - All drill points are gray (detonated)
    - Total blast time is displayed
    - "Simulation Complete" message appears
13. User reviews simulation results
14. User can replay by clicking "Reset" then "Play" again
15. User clicks "Confirm Simulation" button
16. System displays confirmation dialog: "Approve this blast sequence for execution?"
17. User clicks "Confirm"
18. System updates site record:
    - Sets IsSimulationConfirmed = true
    - Updates Modified At timestamp
    - Records user ID who confirmed
19. System logs simulation confirmation event
20. System displays success: "Blast simulation confirmed"
21. Use case ends successfully

**Exceptional Flow**:

**E1: No Blast Connections Found**
- **Trigger**: Site has no blast connections defined (Step 3)
- **Actions**:
  1. System displays message: "No blast sequence found for this site. Please design blast sequence first"
  2. System provides "Go to Blast Sequence Designer" button
  3. Use case ends

**E2: No Starting Holes Designated**
- **Trigger**: No drill points marked as starting holes (Step 5)
- **Actions**:
  1. System displays error: "No starting holes found. Cannot simulate blast sequence"
  2. System provides "Go to Blast Sequence Designer" button to fix
  3. Use case ends

**E3: Orphaned Drill Points**
- **Trigger**: Some drill points are not connected to sequence (Step 5)
- **Actions**:
  1. System displays warning: "X drill points are not connected to blast sequence"
  2. System lists orphaned drill points
  3. System highlights orphaned points in red on canvas
  4. User can choose to:
     - "Continue Simulation" (simulate connected points only)
     - "Go to Designer" (fix connections)
  5. If continue, execute Steps 6-21 for connected points only

**E4: Circular Dependencies in Sequence**
- **Trigger**: Blast connections form circular loop (Step 5)
- **Actions**:
  1. System detects circular dependency using graph traversal
  2. System displays error: "Circular dependency detected in blast sequence"
  3. System highlights connections forming the loop
  4. System provides "Go to Blast Sequence Designer" to fix
  5. Use case ends

**Alternate Flow**:

**A1: Adjust Playback Speed**
- **Trigger**: User adjusts speed slider (any time during Steps 8-11)
- **Actions**:
  1. User moves speed slider (0.25x, 0.5x, 1x, 2x, 4x)
  2. System adjusts animation speed accordingly
  3. Simulation continues at new speed
  4. Use case continues

**A2: Pause and Step Through**
- **Trigger**: User clicks "Pause" button (during Step 9)
- **Actions**:
  1. System pauses animation at current time
  2. System displays "Step Forward" and "Step Backward" buttons
  3. User clicks "Step Forward" to advance to next detonation
  4. System highlights next drill point and updates time
  5. User can step through entire sequence manually
  6. User clicks "Resume" to continue automatic playback
  7. Use case continues

**A3: Export Simulation Report**
- **Trigger**: User clicks "Export Report" button (after Step 12)
- **Actions**:
  1. System generates blast simulation report PDF containing:
     - Project and site details
     - 2D pattern visualization with connections
     - Timeline diagram
     - Detonation sequence table (time, hole, delay)
     - Total blast duration
     - Safety notes
  2. System initiates PDF download
  3. Use case continues

**A4: Modify Sequence During Review**
- **Trigger**: User clicks "Edit Sequence" during simulation (any step)
- **Actions**:
  1. System redirects to Blast Sequence Designer
  2. User makes modifications to connections/delays
  3. User saves changes
  4. System returns to Blast Simulator
  5. System recalculates timeline with new parameters
  6. Return to Step 6 with updated sequence

---

### UC-BLAST-003: Generate Blast Report

**Use Case ID**: UC-BLAST-003

**Use Case Name**: Generate Blast Report

**Actors**:
- Primary: Blasting Engineer
- Secondary: System

**Preconditions**:
- User must be authenticated and authorized
- Target project and site must exist
- Site must have drill data and blast sequence
- Explosive calculations must be completed

**Postconditions**:
- Comprehensive blast report is generated
- Report includes all technical details
- Report is exported as PDF
- Report generation event is logged

**Basic Flow**:
1. User navigates to Reporting section
2. User selects "Blast Report" from report types
3. System displays blast report configuration form:
   - Project selection (dropdown)
   - Site selection (dropdown)
   - Report sections (checkboxes):
     * Project Information
     * Site Details
     * Drill Pattern Visualization
     * Survey Data Table
     * Explosive Usage Summary
     * Blast Sequence Information
     * Safety Compliance Data
4. User selects Project
5. System loads sites for selected project
6. User selects Site
7. System retrieves all data for selected site:
   - Project and site metadata
   - All drill points
   - Blast connections
   - Explosive calculations
   - Pattern approval status
8. User selects desired report sections (or keeps all selected)
9. User clicks "Generate Preview" button
10. System composes report with sections:
    - **Header**: Report title, generation date, generated by user
    - **Project Information**: Name, region, area, status
    - **Site Information**: Name, address, status, completion dates
    - **Drill Pattern Visualization**: 2D canvas rendering as image
    - **Survey Data Table**: All drill points with coordinates, depth, explosives
    - **Explosive Usage Summary**:
      * Total ANFO required (kg)
      * Total Emulsion required (kg)
      * Total explosive weight (kg)
      * Number of drill holes
      * Total depth drilled (m)
      * Total volume (m³)
    - **Blast Sequence Information**: Timeline diagram, connection table
    - **Safety Compliance**: Safety checklist status, environmental assessment
11. System displays report preview in modal dialog
12. User reviews report content
13. User clicks "Export as PDF" button
14. System converts report to PDF format:
    - Applies professional styling
    - Includes page numbers
    - Adds header/footer with project name and date
    - Embeds images at appropriate resolution
15. System generates PDF file
16. System initiates download with filename: "BlastReport_[ProjectName]_[SiteName]_[Date].pdf"
17. System logs report generation event
18. System displays success: "Blast report generated successfully"
19. Use case ends successfully

**Exceptional Flow**:

**E1: No Data Available**
- **Trigger**: Selected site has no drill data (Step 7)
- **Actions**:
  1. System displays error: "Insufficient data to generate report. Please complete drill plan first"
  2. Use case ends

**E2: PDF Generation Failure**
- **Trigger**: PDF conversion fails (Step 15)
- **Actions**:
  1. System logs error details
  2. System displays error: "Failed to generate PDF. Please try again"
  3. System keeps report preview open
  4. User can try again or export as different format
  5. Return to Step 13

**Alternate Flow**:

**A1: Export as Excel**
- **Trigger**: User clicks "Export as Excel" (Step 13)
- **Actions**:
  1. System generates Excel workbook with sheets:
     - Project Info
     - Drill Data (tabular)
     - Explosive Calculations
     - Blast Sequence
  2. System initiates download
  3. Use case ends successfully

**A2: Email Report**
- **Trigger**: User clicks "Email Report" (Step 13)
- **Actions**:
  1. System displays email dialog
  2. User enters recipient email addresses
  3. User optionally adds message
  4. System generates PDF
  5. System sends email with PDF attachment
  6. System displays success
  7. Use case ends

---

## 8. MACHINE MANAGEMENT USE CASES

---

### UC-MACHINE-001: Add Machine to Inventory

**Use Case ID**: UC-MACHINE-001

**Use Case Name**: Add Machine to Inventory

**Actors**:
- Primary: Machine Manager, Admin
- Secondary: System

**Preconditions**:
- User must be authenticated and authorized
- User must have "ManageMachines" authorization
- Machine Management interface must be accessible

**Postconditions**:
- New machine is added to inventory database
- Machine has unique Serial Number
- Machine status is set to "Available"
- Machine creation event is logged
- Machine appears in inventory list

**Basic Flow**:
1. User navigates to Machine Management section
2. System displays machine inventory list with filters and search
3. User clicks "Add New Machine" button
4. System displays machine creation form with fields:
   - Machine Name (required)
   - Type (required, text input)
   - Model (required, text input)
   - Manufacturer (required, text input)
   - Serial Number (required, unique)
   - Rig Number (optional)
   - Plate Number (optional)
   - Chassis Details (optional, textarea)
   - Manufacturing Year (required, year picker)
   - Current Location (optional, text input)
   - Region (optional, dropdown)
   - Specifications (optional, JSON editor for technical details)
5. User enters machine name (e.g., "Drilling Rig DR-001")
6. User enters type (e.g., "Drilling Rig")
7. User enters model (e.g., "DR-5000X")
8. User enters manufacturer (e.g., "Atlas Copco")
9. User enters serial number (e.g., "SN-2024-001234")
10. User optionally enters rig number
11. User optionally enters plate number
12. User optionally enters chassis details
13. User selects manufacturing year
14. User optionally enters current location
15. User optionally selects region
16. User optionally enters technical specifications in JSON format or form
17. User clicks "Add Machine" button
18. System validates all required fields are not empty
19. System validates serial number format
20. System queries database to check if serial number already exists
21. System validates serial number is unique
22. System validates manufacturing year is reasonable (e.g., 1950-current year)
23. System begins database transaction
24. System creates Machine record with:
    - All entered information
    - Status: "Available"
    - Created At: current timestamp
    - Created By: current user ID
    - Last Maintenance Date: null
    - Next Maintenance Date: null (can be set later)
25. System commits transaction to database
26. System logs machine creation event
27. System displays success notification: "Machine '[Name]' added to inventory successfully"
28. System refreshes machine inventory list showing new machine
29. System redirects to machine details page for newly created machine
30. Use case ends successfully

**Exceptional Flow**:

**E1: Empty Required Fields**
- **Trigger**: One or more required fields are empty (Step 18)
- **Actions**:
  1. System displays validation errors next to empty fields
  2. System highlights required fields
  3. Return to Step 5

**E2: Duplicate Serial Number**
- **Trigger**: Serial number already exists in database (Step 21)
- **Actions**:
  1. System displays error: "A machine with this serial number already exists"
  2. System provides link to existing machine
  3. System highlights serial number field
  4. Return to Step 9

**E3: Invalid Manufacturing Year**
- **Trigger**: Manufacturing year is unrealistic (Step 22)
- **Actions**:
  1. System displays error: "Manufacturing year must be between 1950 and [current year]"
  2. System highlights year field
  3. Return to Step 13

**E4: Invalid JSON in Specifications**
- **Trigger**: User enters invalid JSON in specifications field (Step 16)
- **Actions**:
  1. System displays error: "Invalid JSON format in specifications"
  2. System highlights syntax error location
  3. Return to Step 16

**E5: Database Insert Failure**
- **Trigger**: Database transaction fails (Step 25)
- **Actions**:
  1. System rolls back transaction
  2. System logs error details
  3. System displays error: "Failed to add machine. Please try again"
  4. Return to Step 17

**Alternate Flow**:

**A1: Cancel Machine Creation**
- **Trigger**: User clicks "Cancel" (any step before Step 17)
- **Actions**:
  1. If form has data, system displays confirmation: "Discard machine data?"
  2. If user confirms:
     - System clears form
     - System returns to inventory list
     - No machine is created
  3. Use case ends

**A2: Quick Add (Minimal Info)**
- **Trigger**: User clicks "Quick Add" instead of "Add New Machine" (Step 3)
- **Actions**:
  1. System displays simplified form with only:
     - Machine Name
     - Serial Number
     - Type
  2. System auto-fills:
     - Model: "TBD"
     - Manufacturer: "TBD"
     - Status: "Available"
  3. Continue from Step 17
  4. Use case ends successfully

---

### UC-MACHINE-002: Assign Machine to Project and Operator

**Use Case ID**: UC-MACHINE-002

**Use Case Name**: Assign Machine to Project and Operator

**Actors**:
- Primary: Machine Manager, Admin
- Secondary: System, Operator (notified)

**Preconditions**:
- User must be authenticated and authorized
- User must have "ManageMachines" authorization
- Target machine must exist with status "Available"
- Target project must exist
- Target operator must exist with Operator role

**Postconditions**:
- Machine is assigned to project and operator
- Machine status changes to "In Use"
- Operator is linked to machine
- Assignment timestamp is recorded
- Operator receives notification
- Assignment event is logged

**Basic Flow**:
1. User navigates to Machine Management section
2. System displays machine inventory list
3. User searches or filters to find target machine
4. User verifies machine status is "Available"
5. User clicks "Assign Machine" button for target machine
6. System displays machine assignment form:
   - Machine details (read-only): Name, Serial Number, Type, Model
   - Current Status (read-only)
   - Assign to Project (dropdown with search)
   - Assign to Operator (dropdown with search, filtered by Operator role)
   - Assignment Notes (optional, textarea)
7. User searches and selects target project from dropdown
8. System validates project exists and is not in "Completed" or "Cancelled" status
9. User searches and selects target operator from dropdown
10. System validates operator:
    - Has Operator role
    - Is not currently assigned to another machine
    - Is active (not deactivated)
11. User optionally enters assignment notes
12. User clicks "Assign Machine" button
13. System validates project is selected
14. System validates operator is selected
15. System checks if operator is already assigned to another machine
16. System validates operator is available
17. System begins database transaction
18. System updates Machine record:
    - AssignedToProject: selected project ID
    - AssignedToOperator: selected operator ID
    - Status: "In Use"
    - Modified At: current timestamp
    - Modified By: current user ID
19. System creates machine assignment history record:
    - Machine ID
    - Project ID
    - Operator ID
    - Assigned At: current timestamp
    - Assigned By: current user ID
    - Notes: assignment notes
20. System commits transaction to database
21. System creates notification for operator:
    - Message: "You have been assigned to machine: [Machine Name] for project: [Project Name]"
    - Include machine details and project details
22. System sends email notification to operator (optional)
23. System logs assignment event
24. System displays success: "Machine '[Name]' assigned to operator '[Operator Name]' for project '[Project Name]'"
25. System refreshes machine inventory list
26. System displays updated machine status
27. Use case ends successfully

**Exceptional Flow**:

**E1: Machine Not Available**
- **Trigger**: Machine status is not "Available" (Step 4 or 17)
- **Actions**:
  1. System displays error: "Machine is not available for assignment. Current status: [Status]"
  2. System provides information about current assignment (if any)
  3. Use case ends

**E2: Project Not Selected**
- **Trigger**: User doesn't select project (Step 13)
- **Actions**:
  1. System displays error: "Please select a project"
  2. System highlights project dropdown
  3. Return to Step 7

**E3: Operator Not Selected**
- **Trigger**: User doesn't select operator (Step 14)
- **Actions**:
  1. System displays error: "Please select an operator"
  2. System highlights operator dropdown
  3. Return to Step 9

**E4: Operator Already Assigned**
- **Trigger**: Selected operator is already assigned to another machine (Step 16)
- **Actions**:
  1. System displays error: "Operator '[Name]' is already assigned to machine '[Other Machine]'"
  2. System provides options:
     - "View current assignment"
     - "Reassign operator" (unassign from current, assign to new)
     - "Select different operator"
  3. If user selects "Reassign":
     - System unassigns operator from current machine
     - System sets old machine status to "Available"
     - Continue with Step 17
  4. Otherwise, return to Step 9

**E5: Database Update Failure**
- **Trigger**: Database transaction fails (Step 20)
- **Actions**:
  1. System rolls back transaction
  2. System logs error
  3. System displays error: "Failed to assign machine. Please try again"
  4. Return to Step 12

**E6: Invalid Project Status**
- **Trigger**: Project is completed or cancelled (Step 8)
- **Actions**:
  1. System displays error: "Cannot assign machine to completed or cancelled project"
  2. System highlights project dropdown
  3. Return to Step 7

**Alternate Flow**:

**A1: Cancel Assignment**
- **Trigger**: User clicks "Cancel" (any step before Step 12)
- **Actions**:
  1. System closes assignment form
  2. No assignment is made
  3. Machine remains in current status
  4. Use case ends

**A2: Assign Without Operator**
- **Trigger**: User checks "Assign to project only (no operator yet)" (Step 9)
- **Actions**:
  1. System disables operator dropdown
  2. User proceeds without selecting operator
  3. In Step 18, system updates:
     - AssignedToProject: project ID
     - AssignedToOperator: null
     - Status: "Assigned (No Operator)"
  4. Machine is assigned to project but not to specific operator
  5. Use case continues

**A3: Assign Multiple Machines**
- **Trigger**: User selects multiple machines using checkboxes (Step 3)
- **Actions**:
  1. User clicks "Bulk Assign"
  2. System displays bulk assignment form:
     - List of selected machines
     - Single project dropdown
     - Operator assignments (one per machine)
  3. User selects project
  4. User assigns operator to each machine (or leaves unassigned)
  5. System validates all assignments
  6. System performs assignments in transaction
  7. System displays summary: "X machines assigned successfully"
  8. Use case ends

---

### UC-MACHINE-003: Unassign Machine

**Use Case ID**: UC-MACHINE-003

**Use Case Name**: Unassign Machine (Return to Available)

**Actors**:
- Primary: Machine Manager, Admin
- Secondary: System, Operator (notified)

**Preconditions**:
- User must be authenticated and authorized
- User must have "ManageMachines" authorization
- Target machine must be currently assigned (status "In Use")

**Postconditions**:
- Machine assignment is removed
- Machine status changes to "Available"
- Operator is unlinked from machine
- Unassignment timestamp is recorded
- Operator receives notification
- Unassignment event is logged

**Basic Flow**:
1. User navigates to Machine Management section
2. System displays machine inventory list
3. User filters by status "In Use" to see assigned machines
4. User clicks on target machine to view details
5. System displays machine details showing:
   - Current assignment information
   - Assigned project name
   - Assigned operator name
   - Assignment date
6. User clicks "Unassign Machine" button
7. System displays unassignment confirmation dialog:
   - "Unassign this machine from project and operator?"
   - Machine details
   - Current assignment details
   - Reason for unassignment (dropdown: required)
     * Project Completed
     * Machine Required Elsewhere
     * Maintenance Required
     * Other
   - Additional notes (optional, textarea)
8. User selects reason from dropdown
9. User optionally enters additional notes
10. User clicks "Confirm Unassignment"
11. System validates reason is selected
12. System begins database transaction
13. System records current assignment details for history
14. System updates Machine record:
    - AssignedToProject: null
    - AssignedToOperator: null
    - Status: "Available"
    - Modified At: current timestamp
    - Modified By: current user ID
15. System updates machine assignment history record:
    - Sets Unassigned At: current timestamp
    - Sets Unassigned By: current user ID
    - Sets Unassignment Reason: selected reason
    - Sets Unassignment Notes: additional notes
16. System commits transaction to database
17. System creates notification for operator:
    - Message: "You have been unassigned from machine: [Machine Name]. Reason: [Reason]"
18. System logs unassignment event
19. System displays success: "Machine '[Name]' unassigned successfully. Status set to Available"
20. System refreshes machine details showing updated status
21. Use case ends successfully

**Exceptional Flow**:

**E1: Machine Not Assigned**
- **Trigger**: Machine status is "Available" or "Under Maintenance" (Step 4)
- **Actions**:
  1. System disables "Unassign Machine" button
  2. System displays message: "Machine is not currently assigned"
  3. Use case ends

**E2: Reason Not Selected**
- **Trigger**: User doesn't select reason (Step 11)
- **Actions**:
  1. System displays error: "Please select a reason for unassignment"
  2. System highlights reason dropdown
  3. Return to Step 8

**E3: Database Update Failure**
- **Trigger**: Database transaction fails (Step 16)
- **Actions**:
  1. System rolls back transaction
  2. System logs error
  3. System displays error: "Failed to unassign machine. Please try again"
  4. Machine remains assigned
  5. Return to Step 10

**Alternate Flow**:

**A1: Cancel Unassignment**
- **Trigger**: User clicks "Cancel" in confirmation dialog (Step 10)
- **Actions**:
  1. System closes dialog
  2. No changes are made
  3. Machine remains assigned
  4. Use case ends

**A2: Unassign and Schedule Maintenance**
- **Trigger**: User selects "Maintenance Required" as reason (Step 8)
- **Actions**:
  1. After unassignment (Step 16), system prompts:
     "Schedule maintenance for this machine?"
  2. If user confirms:
     - System updates machine status to "Under Maintenance"
     - System opens maintenance scheduling dialog (UC-MACHINE-004)
  3. Use case continues to UC-MACHINE-004

---

## 9. EXPLOSIVE INVENTORY USE CASES

---

### UC-INVENTORY-001: Add Explosive Batch to Central Warehouse

**Use Case ID**: UC-INVENTORY-001

**Use Case Name**: Add Explosive Batch to Central Warehouse

**Actors**:
- Primary: Explosive Manager
- Secondary: System

**Preconditions**:
- User must be authenticated and authorized
- User must have "ManageInventory" authorization
- Central Warehouse Inventory interface must be accessible

**Postconditions**:
- New explosive batch is added to central warehouse inventory
- Batch has unique Batch ID
- Technical properties (ANFO or Emulsion) are stored
- Batch status is set to "Available"
- Inventory creation event is logged
- Batch appears in inventory list

**Basic Flow**:
1. User navigates to Central Warehouse Inventory section
2. System displays inventory dashboard showing:
   - Total inventory by explosive type
   - Available, Allocated, Expired inventory counts
   - Low stock alerts
   - Recent additions
3. User clicks "Add New Batch" button
4. System displays explosive type selection:
   - ANFO (Ammonium Nitrate Fuel Oil)
   - Emulsion
5. User selects explosive type (e.g., ANFO)
6. System displays batch creation form with fields based on type:
   - **Common Fields:**
     * Batch ID (required, unique, auto-suggested)
     * Quantity (required, kg)
     * Manufacturing Date (required, date picker)
     * Expiry Date (required, date picker)
     * Supplier (required, text input)
     * Manufacturer Batch Number (optional)
     * Storage Location (required, text input)
   - **ANFO-Specific Fields** (if ANFO selected):
     * Fume Class (dropdown)
     * Grade (text input)
     * Sensitization Type (dropdown)
     * Density (g/cm³)
     * Velocity of Detonation (m/s)
     * Weight Strength (%)
     * Bulk Strength
   - **Emulsion-Specific Fields** (if Emulsion selected):
     * Density (g/cm³)
     * Velocity of Detonation (m/s)
     * Water Resistance (dropdown: Excellent, Good, Fair)
     * Sensitization Method (text input)
     * Critical Diameter (mm)
     * Cartridge Diameter (mm, optional)
7. User enters or accepts auto-generated Batch ID (e.g., "ANFO-2025-001")
8. User enters quantity in kilograms (e.g., 5000 kg)
9. User selects manufacturing date
10. User selects expiry date
11. User enters supplier name
12. User optionally enters manufacturer batch number
13. User enters storage location in warehouse (e.g., "Bay A-12")
14. User enters technical properties specific to explosive type:
    - For ANFO: Fume class, grade, density, velocity, strength values
    - For Emulsion: Density, velocity, water resistance, critical diameter
15. User clicks "Add Batch" button
16. System validates all required fields are not empty
17. System validates Batch ID format and uniqueness
18. System validates quantity is positive number
19. System validates expiry date is after manufacturing date
20. System validates expiry date is in future (not expired)
21. System validates technical property values are within acceptable ranges
22. System begins database transaction
23. System creates CentralWarehouseInventory record with:
    - Batch ID
    - Explosive Type
    - Quantity
    - Allocated Quantity: 0
    - Manufacturing Date
    - Expiry Date
    - Supplier
    - Manufacturer Batch Number
    - Storage Location
    - Status: "Available"
    - Created At: current timestamp
    - Created By: current user ID
24. System creates technical properties record based on type:
    - ANFOTechnicalProperties table (if ANFO)
    - EmulsionTechnicalProperties table (if Emulsion)
    - Links to CentralWarehouseInventory record
25. System commits transaction to database
26. System calculates days until expiry
27. If days until expiry < 30, system creates "Expiring Soon" alert
28. System checks total available quantity against low-stock threshold
29. If below threshold, system creates low-stock alert
30. System logs inventory addition event
31. System displays success: "Explosive batch '[Batch ID]' added successfully. Quantity: [X] kg"
32. System refreshes inventory dashboard showing new batch
33. Use case ends successfully

**Exceptional Flow**:

**E1: Empty Required Fields**
- **Trigger**: One or more required fields are empty (Step 16)
- **Actions**:
  1. System displays validation errors next to empty fields
  2. System highlights required fields
  3. Return to Step 7

**E2: Duplicate Batch ID**
- **Trigger**: Batch ID already exists (Step 17)
- **Actions**:
  1. System displays error: "Batch ID already exists. Please use a unique ID"
  2. System suggests alternative: "[BatchID]-2"
  3. System highlights Batch ID field
  4. Return to Step 7

**E3: Invalid Quantity**
- **Trigger**: Quantity is zero, negative, or non-numeric (Step 18)
- **Actions**:
  1. System displays error: "Quantity must be a positive number"
  2. System highlights quantity field
  3. Return to Step 8

**E4: Invalid Date Range**
- **Trigger**: Expiry date is before or equal to manufacturing date (Step 19)
- **Actions**:
  1. System displays error: "Expiry date must be after manufacturing date"
  2. System highlights both date fields
  3. Return to Step 9

**E5: Expired Batch**
- **Trigger**: Expiry date is in the past (Step 20)
- **Actions**:
  1. System displays error: "Cannot add expired batch. Expiry date must be in the future"
  2. System highlights expiry date field
  3. User can either:
     - Correct expiry date
     - Cancel (expired material should not be added)
  4. Return to Step 10

**E6: Invalid Technical Properties**
- **Trigger**: Technical property values are outside acceptable ranges (Step 21)
- **Actions**:
  1. System displays specific errors:
     - "Density must be between 0.5 and 2.0 g/cm³"
     - "Velocity of Detonation must be between 1000 and 7000 m/s"
     - "Weight Strength must be between 0 and 200%"
  2. System highlights invalid fields
  3. Return to Step 14

**E7: Database Insert Failure**
- **Trigger**: Database transaction fails (Step 25)
- **Actions**:
  1. System rolls back entire transaction
  2. System logs error details
  3. System displays error: "Failed to add batch. Please try again"
  4. Return to Step 15

**Alternate Flow**:

**A1: Cancel Batch Creation**
- **Trigger**: User clicks "Cancel" (any step before Step 15)
- **Actions**:
  1. If form has data, system displays confirmation: "Discard batch data?"
  2. If user confirms:
     - System clears form
     - System returns to inventory dashboard
     - No batch is created
  3. Use case ends

**A2: Import Batch from Supplier Document**
- **Trigger**: User clicks "Import from Document" (Step 3)
- **Actions**:
  1. System displays file upload dialog
  2. User uploads supplier delivery document (PDF/CSV)
  3. System extracts batch information using OCR or parsing
  4. System pre-fills form with extracted data
  5. User reviews and corrects if needed
  6. Continue from Step 15
  7. Use case continues

**A3: Copy from Previous Batch**
- **Trigger**: User clicks "Copy from Existing Batch" (Step 6)
- **Actions**:
  1. System displays batch selection dialog
  2. User searches and selects previous batch
  3. System copies all fields except:
     - Batch ID (generates new)
     - Quantity (user must enter new)
     - Manufacturing Date (user must enter new)
     - Expiry Date (user must enter new)
  4. Technical properties are copied (assuming same supplier/type)
  5. User modifies as needed
  6. Continue from Step 15
  7. Use case continues

---

## 10. STORE MANAGEMENT USE CASES

---

### UC-STORE-001: Create New Store

**Use Case ID**: UC-STORE-001

**Use Case Name**: Create New Store

**Actors**:
- Primary: Explosive Manager, Admin
- Secondary: System, Store Manager (assigned), Email Service

**Preconditions**:
- User must be authenticated and authorized
- User must have "ManageInventory" authorization
- Store Management interface must be accessible
- At least one region must exist
- At least one user with Store Manager role must exist

**Postconditions**:
- New store is created in database
- Store ID is auto-generated
- Store Manager is assigned
- Store status is set to "Operational"
- Store Manager receives notification
- Store creation event is logged
- Store appears in store list

**Basic Flow**:
1. User navigates to Store Management section
2. System displays store dashboard showing:
   - Total stores count
   - Stores by status (Operational, Under Maintenance, Decommissioned)
   - Total storage capacity across all stores
   - Average utilization rate
   - Map view of store locations
3. User clicks "Create New Store" button
4. System displays store creation form with fields:
   - Store Name (required, text input)
   - Store Address (required, textarea)
   - City (required, text input)
   - Region (required, dropdown)
   - Store Manager (required, user dropdown with Store Manager role filter)
   - Storage Capacity (required, number input, kg)
   - Allowed Explosive Types (required, multi-select: ANFO, Emulsion)
5. User enters store name (e.g., "Muscat Central Explosive Store")
6. User enters full store address
7. User enters city name
8. User selects region from dropdown
9. User searches and selects Store Manager from user dropdown
10. System validates selected user has Store Manager role
11. User enters total storage capacity in kilograms (e.g., 50000 kg)
12. User selects allowed explosive types (can select both)
13. User clicks "Create Store" button
14. System validates all required fields are not empty
15. System validates Store Name is unique
16. System validates Storage Capacity is positive number
17. System validates at least one explosive type is selected
18. System validates selected Store Manager has appropriate role
19. System checks if Store Manager is already managing another store
20. System begins database transaction
21. System generates unique Store ID
22. System creates Store record with:
    - Store ID (generated)
    - Store Name
    - Store Address
    - City
    - Region ID
    - Manager User ID
    - Storage Capacity
    - Allowed Explosive Types (comma-separated string)
    - Status: "Operational"
    - Created At: current timestamp
    - Created By: current user ID
23. System creates initial empty inventory records for allowed explosive types:
    - StoreInventory record for each allowed type
    - Quantity: 0
    - Reserved Quantity: 0
    - Minimum Stock Level: 10% of capacity (calculated)
    - Maximum Stock Level: capacity
24. System commits transaction to database
25. System creates notification for assigned Store Manager:
    - Message: "You have been assigned as manager of store: [Store Name]"
    - Include store details: address, capacity, allowed explosives
26. System sends email to Store Manager:
    - Subject: "Store Assignment - [Store Name]"
    - Body with store details and responsibilities
    - Login link
27. System logs store creation event
28. System displays success: "Store '[Store Name]' created successfully. Store ID: [ID]"
29. System refreshes store list showing new store
30. System redirects to store details page
31. Use case ends successfully

**Exceptional Flow**:

**E1: Empty Required Fields**
- **Trigger**: One or more required fields empty (Step 14)
- **Actions**:
  1. System displays validation errors
  2. System highlights empty required fields
  3. Return to Step 5

**E2: Duplicate Store Name**
- **Trigger**: Store Name already exists (Step 15)
- **Actions**:
  1. System displays error: "A store with this name already exists"
  2. System provides link to existing store
  3. System highlights Store Name field
  4. Return to Step 5

**E3: Invalid Storage Capacity**
- **Trigger**: Capacity is zero, negative, or non-numeric (Step 16)
- **Actions**:
  1. System displays error: "Storage capacity must be a positive number (in kg)"
  2. System highlights capacity field
  3. Return to Step 11

**E4: No Explosive Types Selected**
- **Trigger**: User doesn't select any explosive types (Step 17)
- **Actions**:
  1. System displays error: "Please select at least one allowed explosive type"
  2. System highlights explosive types selector
  3. Return to Step 12

**E5: Invalid Store Manager Role**
- **Trigger**: Selected user doesn't have Store Manager role (Step 18)
- **Actions**:
  1. System displays error: "Selected user does not have Store Manager role"
  2. System highlights Store Manager dropdown
  3. Return to Step 9

**E6: Store Manager Already Assigned**
- **Trigger**: Selected Store Manager is already managing another store (Step 19)
- **Actions**:
  1. System displays warning: "[Manager Name] is already managing store '[Other Store Name]'"
  2. System asks: "Assign this manager to multiple stores?"
  3. If user confirms:
     - Continue with Step 20 (allow multiple assignments)
  4. If user cancels:
     - Return to Step 9 to select different manager

**E7: Database Insert Failure**
- **Trigger**: Database transaction fails (Step 24)
- **Actions**:
  1. System rolls back entire transaction
  2. System logs error details
  3. System displays error: "Failed to create store. Please try again"
  4. Return to Step 13

**E8: Email Notification Failure**
- **Trigger**: Email to Store Manager fails (Step 26)
- **Actions**:
  1. Store is still created successfully
  2. System logs email failure
  3. System displays warning: "Store created but email notification failed. Please manually inform Store Manager"
  4. System displays Store Manager contact information
  5. Continue from Step 28
  6. Use case ends successfully

**Alternate Flow**:

**A1: Cancel Store Creation**
- **Trigger**: User clicks "Cancel" (any step before Step 13)
- **Actions**:
  1. If form has data, system displays confirmation: "Discard store data?"
  2. If user confirms:
     - System clears form
     - System returns to store dashboard
     - No store is created
  3. Use case ends

**A2: Create Store from Template**
- **Trigger**: User clicks "Use Template" (Step 4)
- **Actions**:
  1. System displays store templates:
     - "Small Store (10,000 kg capacity)"
     - "Medium Store (50,000 kg capacity)"
     - "Large Store (100,000 kg capacity)"
  2. User selects template
  3. System pre-fills Storage Capacity and default Allowed Explosive Types
  4. User enters remaining required fields
  5. Continue from Step 13
  6. Use case continues

---

### UC-STORE-002: Create Inventory Transfer Request

**Use Case ID**: UC-STORE-002

**Use Case Name**: Create Inventory Transfer Request (Store Manager Requests from Central Warehouse)

**Actors**:
- Primary: Store Manager
- Secondary: System, Explosive Manager (notified)

**Preconditions**:
- User must be authenticated and authorized
- User must have Store Manager role
- User must be assigned to at least one store
- Central warehouse must have available inventory
- Transfer Management interface must be accessible

**Postconditions**:
- Transfer request is created with status "Pending"
- Request is routed to Explosive Manager for approval
- Central warehouse inventory is NOT yet allocated (pending approval)
- Explosive Manager receives notification
- Transfer request event is logged
- Request appears in transfer request list

**Basic Flow**:
1. Store Manager navigates to Transfer Management section
2. System displays transfer request dashboard showing:
   - Pending requests
   - Approved requests awaiting delivery
   - Completed transfers history
   - Store's current inventory levels
3. Store Manager clicks "Request Transfer" button
4. System retrieves Store Manager's assigned store(s)
5. If multiple stores, system displays store selection dropdown
6. Store Manager selects destination store
7. System displays current inventory for selected store:
   - ANFO: Current quantity, Reserved quantity, Available quantity
   - Emulsion: Current quantity, Reserved quantity, Available quantity
   - Utilization: X% of capacity
8. System displays transfer request form with fields:
   - Destination Store (auto-filled, read-only)
   - Explosive Type (required, dropdown: ANFO or Emulsion)
   - Source Batch (required, dropdown populated from available central inventory)
   - Requested Quantity (required, kg)
   - Required By Date (optional, date picker)
   - Request Notes (optional, textarea)
9. Store Manager selects explosive type (e.g., ANFO)
10. System queries central warehouse for available batches of selected type
11. System populates Source Batch dropdown with batches where:
    - Explosive Type matches selection
    - Status is "Available"
    - Available Quantity > 0
    - Not expired
    - Ordered by expiry date (FIFO - First In, First Out)
12. Store Manager selects source batch from dropdown
13. System displays batch details:
    - Batch ID
    - Available Quantity
    - Expiry Date
    - Days until expiry
14. Store Manager enters requested quantity in kg
15. System validates quantity in real-time:
    - Must be positive
    - Must not exceed available quantity in source batch
    - Destination store must have capacity remaining
16. System calculates and displays:
    - "Available in batch: X kg"
    - "Destination store capacity remaining: Y kg"
    - "After transfer, utilization will be: Z%"
17. Store Manager optionally selects required-by date
18. Store Manager optionally enters request notes/justification
19. Store Manager clicks "Submit Request" button
20. System validates all required fields are not empty
21. System validates requested quantity is within limits
22. System validates destination store has capacity for requested quantity
23. System validates source batch has sufficient available quantity
24. System begins database transaction
25. System generates unique Request Number (e.g., "TR-2025-001234")
26. System creates InventoryTransferRequest record with:
    - Request Number (generated)
    - Central Warehouse Inventory ID (source batch)
    - Destination Store ID
    - Requested Quantity
    - Approved Quantity: null (pending approval)
    - Required By Date
    - Status: "Pending"
    - Request Date: current timestamp
    - Requested By User ID: Store Manager's ID
    - Request Notes
27. System commits transaction to database
28. System creates notification for Explosive Manager:
    - Message: "New transfer request from [Store Name]: [Quantity] kg of [Type]"
    - Include request details and link to approval page
29. System logs transfer request creation event
30. System displays success: "Transfer request submitted successfully. Request Number: [Number]"
31. System displays message: "Your request is pending approval from Explosive Manager"
32. System refreshes transfer request dashboard showing new request in "Pending" status
33. Use case ends successfully

**Exceptional Flow**:

**E1: No Assigned Store**
- **Trigger**: Store Manager is not assigned to any store (Step 4)
- **Actions**:
  1. System displays error: "You are not assigned to any store. Please contact administrator"
  2. System disables "Request Transfer" button
  3. Use case ends

**E2: No Available Batches**
- **Trigger**: No available batches of selected type in central warehouse (Step 11)
- **Actions**:
  1. System displays message: "No available [Type] batches in central warehouse"
  2. System disables Source Batch dropdown
  3. System displays "Contact Explosive Manager for new inventory"
  4. User can select different explosive type or cancel
  5. If user selects different type, return to Step 10
  6. Otherwise, use case ends

**E3: Quantity Exceeds Available**
- **Trigger**: Requested quantity > available in batch (Step 21)
- **Actions**:
  1. System displays error: "Requested quantity (X kg) exceeds available quantity in batch (Y kg)"
  2. System highlights quantity field
  3. System suggests: "Maximum available: Y kg"
  4. Return to Step 14

**E4: Destination Store Capacity Exceeded**
- **Trigger**: Requested quantity would exceed destination store capacity (Step 22)
- **Actions**:
  1. System calculates remaining capacity: Capacity - Current Occupancy
  2. System displays error: "Transfer would exceed store capacity. Remaining capacity: X kg"
  3. System highlights quantity field
  4. Return to Step 14

**E5: Source Batch Expired or Unavailable**
- **Trigger**: Selected batch status changed to "Expired" or "Allocated" before submission (Step 23)
- **Actions**:
  1. System displays error: "Selected batch is no longer available. Please select another batch"
  2. System refreshes batch dropdown
  3. Return to Step 12

**E6: Database Insert Failure**
- **Trigger**: Database transaction fails (Step 27)
- **Actions**:
  1. System rolls back transaction
  2. System logs error
  3. System displays error: "Failed to submit transfer request. Please try again"
  4. Return to Step 19

**Alternate Flow**:

**A1: Cancel Request Creation**
- **Trigger**: Store Manager clicks "Cancel" (any step before Step 19)
- **Actions**:
  1. If form has data, system displays confirmation: "Discard transfer request?"
  2. If user confirms:
     - System clears form
     - System returns to dashboard
     - No request is created
  3. Use case ends

**A2: Quick Request (Low Stock Alert)**
- **Trigger**: Store Manager clicks "Request Restock" from low-stock alert (Step 2)
- **Actions**:
  1. System pre-fills form with:
     - Destination Store: current store
     - Explosive Type: type with low stock
     - Requested Quantity: (Minimum Stock Level - Current Quantity)
  2. System suggests most suitable batch (nearest expiry, FIFO)
  3. Store Manager reviews and adjusts if needed
  4. Continue from Step 19
  5. Use case continues

**A3: Split Request Across Multiple Batches**
- **Trigger**: Single batch doesn't have enough quantity (Step 13)
- **Actions**:
  1. System displays message: "Requested quantity exceeds single batch. Split across multiple batches?"
  2. If Store Manager confirms:
     - System creates multiple transfer requests automatically
     - Each request references different source batch
     - Quantities are distributed optimally
     - All requests submitted together
  3. System displays: "Created X transfer requests totaling Y kg"
  4. Use case ends successfully

---

## 11. APPROVAL WORKFLOW USE CASES

---

### UC-APPROVAL-001: Approve Transfer Request

**Use Case ID**: UC-APPROVAL-001

**Use Case Name**: Approve Inventory Transfer Request

**Actors**:
- Primary: Explosive Manager
- Secondary: System, Store Manager (notified)

**Preconditions**:
- User must be authenticated and authorized
- User must have "ApproveTransfers" authorization
- Transfer request must exist with status "Pending"
- Source batch must have sufficient available quantity
- Transfer Management interface must be accessible

**Postconditions**:
- Transfer request status changes to "Approved"
- Approved quantity is recorded
- Source batch allocated quantity is increased
- Store Manager receives approval notification
- Approval event is logged
- Request moves to "Ready for Dispatch" queue

**Basic Flow**:
1. Explosive Manager navigates to Transfer Management section
2. System displays approval dashboard showing:
   - Pending transfer requests (count)
   - Requests by priority
   - Requests by store
   - Requests by explosive type
3. System displays list of pending transfer requests with columns:
   - Request Number
   - Store Name
   - Explosive Type
   - Requested Quantity
   - Request Date
   - Required By Date
   - Priority indicator
   - "Review" button
4. Explosive Manager filters or searches for specific request
5. Explosive Manager clicks "Review" button for target request
6. System retrieves full request details from database
7. System displays transfer request review page showing:
   - **Request Information:**
     * Request Number
     * Request Date
     * Requested By (Store Manager name)
     * Request Notes
   - **Destination Store Information:**
     * Store Name
     * Current Occupancy
     * Storage Capacity
     * Remaining Capacity
     * Current utilization %
   - **Source Batch Information:**
     * Batch ID
     * Explosive Type
     * Total Quantity in batch
     * Allocated Quantity
     * Available Quantity
     * Expiry Date
     * Days until expiry
     * Storage Location
   - **Transfer Details:**
     * Requested Quantity: X kg
     * Required By Date
   - **Validation Checks** (color-coded):
     ✓ Source batch has sufficient quantity
     ✓ Destination store has capacity
     ✓ Source batch not expired
     ⚠ Source batch expires in < 30 days (warning)
8. Explosive Manager reviews all information
9. System displays approval form with fields:
   - Approved Quantity (pre-filled with requested quantity, editable)
   - Approval Notes (optional, textarea)
   - Priority (dropdown: Normal, High, Urgent)
10. Explosive Manager can modify approved quantity if needed
11. System validates approved quantity in real-time:
    - Must be ≤ requested quantity
    - Must be ≤ available quantity in source batch
    - Must not exceed destination store remaining capacity
12. Explosive Manager optionally enters approval notes
13. Explosive Manager optionally sets priority level
14. Explosive Manager clicks "Approve Transfer" button
15. System validates approved quantity is valid
16. System begins database transaction
17. System updates InventoryTransferRequest record:
    - Status: "Approved"
    - Approved Quantity: entered value
    - Approved Date: current timestamp
    - Approved By User ID: Explosive Manager's ID
    - Approval Notes: entered notes
    - Priority: selected priority
18. System updates source CentralWarehouseInventory record:
    - Allocated Quantity += Approved Quantity
    - (This reserves the quantity for this transfer)
19. System commits transaction to database
20. System creates notification for requesting Store Manager:
    - Message: "Your transfer request [Request Number] has been approved"
    - Details: "Approved quantity: X kg. Priority: Y. Ready for dispatch"
    - Link to request details
21. System sends email to Store Manager (optional)
22. System logs approval event with details
23. System displays success: "Transfer request approved successfully. Approved quantity: X kg"
24. System moves request to "Approved - Awaiting Dispatch" section
25. System refreshes approval dashboard (pending count decreases)
26. Use case ends successfully

**Exceptional Flow**:

**E1: Invalid Approved Quantity**
- **Trigger**: Approved quantity exceeds limits (Step 15)
- **Actions**:
  1. System displays error: "Approved quantity must be between 0 and [max] kg"
  2. System highlights approved quantity field
  3. Return to Step 10

**E2: Source Batch Insufficient Quantity**
- **Trigger**: Source batch available quantity < approved quantity (Step 15)
- **Actions**:
  1. System displays error: "Source batch no longer has sufficient quantity. Available: X kg"
  2. System suggests:
     - "Approve partial quantity (X kg)"
     - "Select different source batch"
  3. If partial approval:
     - Explosive Manager adjusts quantity to available amount
     - Continue from Step 14
  4. If different batch:
     - Return to request review, change source batch
     - Return to Step 10

**E3: Destination Store Capacity Exceeded**
- **Trigger**: Store capacity would be exceeded (Step 15)
- **Actions**:
  1. System displays error: "Transfer would exceed store capacity. Remaining capacity: X kg"
  2. Explosive Manager must reduce approved quantity
  3. Return to Step 10

**E4: Request Already Processed**
- **Trigger**: Request status changed by another user (Step 17)
- **Actions**:
  1. System detects status is no longer "Pending"
  2. System displays error: "This request has already been processed by another user"
  3. System shows current status
  4. Use case ends

**E5: Database Update Failure**
- **Trigger**: Database transaction fails (Step 19)
- **Actions**:
  1. System rolls back entire transaction
  2. System logs error
  3. System displays error: "Failed to approve transfer. Please try again"
  4. Return to Step 14

**E6: Source Batch Expired**
- **Trigger**: Source batch expiry date has passed (Step 7)
- **Actions**:
  1. System displays error: "Source batch has expired. Cannot approve transfer"
  2. System suggests: "Reject this request and ask Store Manager to create new request with valid batch"
  3. Explosive Manager should reject request (go to UC-APPROVAL-002)
  4. Use case ends

**Alternate Flow**:

**A1: Cancel Approval**
- **Trigger**: Explosive Manager clicks "Cancel" (any step before Step 14)
- **Actions**:
  1. System closes review page
  2. No changes are made
  3. Request remains "Pending"
  4. System returns to approval dashboard
  5. Use case ends

**A2: Approve Partial Quantity**
- **Trigger**: Explosive Manager intentionally approves less than requested (Step 10)
- **Actions**:
  1. Explosive Manager enters approved quantity < requested quantity
  2. System displays warning: "Approving partial quantity (X kg of Y kg requested)"
  3. System requires approval notes explaining partial approval
  4. Explosive Manager enters explanation in notes
  5. Continue from Step 14
  6. Store Manager is notified of partial approval with explanation
  7. Use case continues successfully

**A3: Bulk Approve Multiple Requests**
- **Trigger**: Explosive Manager selects multiple pending requests (Step 4)
- **Actions**:
  1. Explosive Manager checks checkboxes next to multiple requests
  2. Explosive Manager clicks "Bulk Approve"
  3. System displays bulk approval dialog:
     - List of selected requests
     - Total quantity across all requests
     - Validation checks for each request
  4. System validates all requests can be approved
  5. If any validation fails:
     - System shows which requests have issues
     - Explosive Manager can deselect problematic ones
  6. Explosive Manager confirms bulk approval
  7. System approves all valid requests in single transaction
  8. System displays summary: "Approved X of Y requests"
  9. Use case ends successfully

**A4: Request More Information**
- **Trigger**: Explosive Manager needs clarification (Step 8)
- **Actions**:
  1. Explosive Manager clicks "Request Information" button
  2. System displays message dialog
  3. Explosive Manager enters question/request for Store Manager
  4. System sends notification to Store Manager
  5. Request remains in "Pending" status
  6. Store Manager can reply with additional information
  7. Explosive Manager reviews again later
  8. Use case ends (to be resumed after clarification)

---

### UC-APPROVAL-002: Reject Transfer Request

**Use Case ID**: UC-APPROVAL-002

**Use Case Name**: Reject Inventory Transfer Request

**Actors**:
- Primary: Explosive Manager
- Secondary: System, Store Manager (notified)

**Preconditions**:
- User must be authenticated and authorized
- User must have "ApproveTransfers" authorization
- Transfer request must exist with status "Pending"

**Postconditions**:
- Transfer request status changes to "Rejected"
- Rejection reason is recorded
- No inventory is allocated
- Store Manager receives rejection notification with reason
- Rejection event is logged

**Basic Flow**:
1. Explosive Manager navigates to Transfer Management section
2. System displays pending transfer requests list
3. Explosive Manager clicks "Review" for target request
4. System displays transfer request review page (same as UC-APPROVAL-001 Step 7)
5. Explosive Manager reviews request details
6. Explosive Manager identifies reason for rejection (e.g., insufficient stock, expired batch, store capacity issue, policy violation)
7. Explosive Manager clicks "Reject Transfer" button
8. System displays rejection confirmation dialog with:
   - Request details summary
   - "Reason for Rejection" dropdown (required):
     * Insufficient Inventory
     * Source Batch Expired
     * Store Capacity Issue
     * Incorrect Request
     * Policy Violation
     * Other
   - Detailed Explanation (required, textarea)
   - Suggestions for Store Manager (optional, textarea)
9. Explosive Manager selects rejection reason from dropdown
10. Explosive Manager enters detailed explanation
11. Explosive Manager optionally enters suggestions (e.g., "Request from Batch ANFO-2025-002 instead")
12. Explosive Manager clicks "Confirm Rejection"
13. System validates reason is selected
14. System validates detailed explanation is not empty
15. System begins database transaction
16. System updates InventoryTransferRequest record:
    - Status: "Rejected"
    - Rejection Reason: selected reason
    - Rejection Notes: detailed explanation
    - Rejected Date: current timestamp
    - Rejected By User ID: Explosive Manager's ID
17. System commits transaction to database
18. System creates notification for Store Manager:
    - Message: "Your transfer request [Request Number] has been rejected"
    - Reason: rejection reason
    - Detailed explanation
    - Suggestions (if provided)
    - Link to create new request
19. System sends email to Store Manager with rejection details
20. System logs rejection event
21. System displays success: "Transfer request rejected. Store Manager has been notified"
22. System refreshes approval dashboard (pending count decreases)
23. Use case ends successfully

**Exceptional Flow**:

**E1: Empty Required Fields**
- **Trigger**: Reason not selected or explanation empty (Step 13-14)
- **Actions**:
  1. System displays error: "Please select rejection reason and provide explanation"
  2. System highlights empty fields
  3. Return to Step 9

**E2: Request Already Processed**
- **Trigger**: Request status changed by another user (Step 16)
- **Actions**:
  1. System detects status is no longer "Pending"
  2. System displays error: "This request has already been processed"
  3. Use case ends

**E3: Database Update Failure**
- **Trigger**: Database transaction fails (Step 17)
- **Actions**:
  1. System rolls back transaction
  2. System logs error
  3. System displays error: "Failed to reject request. Please try again"
  4. Return to Step 12

**Alternate Flow**:

**A1: Cancel Rejection**
- **Trigger**: Explosive Manager clicks "Cancel" in rejection dialog (Step 12)
- **Actions**:
  1. System closes rejection dialog
  2. No changes are made
  3. Request remains "Pending"
  4. System returns to review page
  5. Use case ends

**A2: Reject with Alternative Suggestion**
- **Trigger**: Explosive Manager selects "Other" as reason (Step 9)
- **Actions**:
  1. System displays additional field: "Alternative Action"
  2. Explosive Manager provides specific alternative:
     - "Request different batch: [Batch ID]"
     - "Reduce quantity to: [X] kg"
     - "Wait for new inventory expected: [Date]"
  3. Continue from Step 12
  4. Store Manager receives detailed alternative action in notification
  5. Use case continues

---

## 12. REPORTING & ANALYTICS USE CASES

---

### UC-REPORT-001: View Dashboard Analytics

**Use Case ID**: UC-REPORT-001

**Use Case Name**: View Role-Specific Dashboard Analytics

**Actors**:
- Primary: Any Authenticated User
- Secondary: System

**Preconditions**:
- User must be authenticated
- User must have valid role
- Dashboard interface must be accessible

**Postconditions**:
- Role-specific dashboard is displayed
- Real-time data is retrieved and visualized
- User can view key performance indicators
- Dashboard view event is logged

**Basic Flow**:
1. User logs in to system (UC-AUTH-001)
2. System identifies user's primary role
3. System redirects user to role-specific dashboard
4. **If Admin:**
   - System retrieves:
     * Total projects count by status
     * Total sites count by status
     * Total users count by role
     * Total machines count by status
     * Total stores count by status
     * Pending approval requests count
   - System displays Admin Dashboard with widgets:
     * Project Statistics (pie chart)
     * Site Completion Progress (bar chart)
     * User Distribution by Role (pie chart)
     * Machine Utilization (gauge)
     * Store Capacity Overview (bar chart)
     * Recent Activity Feed
     * Pending Actions List
5. **If Blasting Engineer:**
   - System retrieves:
     * Assigned projects list
     * Sites in progress for assigned projects
     * Pending explosive approval requests
     * Recent drill plans
     * Recent blast calculations
   - System displays Blasting Engineer Dashboard with widgets:
     * My Projects (card list)
     * Sites Status Overview (status cards)
     * Pending Approvals (table)
     * Recent Drill Plans (list)
     * Quick Actions (Create Project, Upload CSV, Design Blast)
6. **If Machine Manager:**
   - System retrieves:
     * Total machines by status
     * Pending machine assignment requests
     * Machines due for maintenance
     * Machine utilization statistics
   - System displays Machine Manager Dashboard with widgets:
     * Machine Inventory Summary (cards)
     * Status Distribution (donut chart)
     * Maintenance Schedule (calendar)
     * Pending Requests (table)
     * Quick Actions (Add Machine, Assign Machine)
7. **If Explosive Manager:**
   - System retrieves:
     * Total inventory (central warehouse) by type
     * Pending transfer requests count
     * Low-stock alerts
     * Expiring inventory warnings (< 30 days)
     * Store utilization overview
   - System displays Explosive Manager Dashboard with widgets:
     * Central Inventory Summary (cards with ANFO/Emulsion)
     * Pending Transfers (table with priority)
     * Low Stock Alerts (alert panel)
     * Expiring Batches (warning panel)
     * Store Network Status (map or list)
     * Quick Actions (Add Batch, Approve Transfers)
8. **If Store Manager:**
   - System retrieves:
     * Assigned store(s) details
     * Current inventory levels
     * Pending incoming transfers
     * Pending explosive approval requests
     * Utilization status
   - System displays Store Manager Dashboard with widgets:
     * My Store(s) Overview (cards)
     * Inventory Levels (bar chart per explosive type)
     * Pending Transfers (table)
     * Pending Approvals (table)
     * Low Stock Warnings
     * Quick Actions (Request Transfer, Approve Usage)
9. **If Operator:**
   - System retrieves:
     * Assigned project details
     * Assigned machine details
     * Assigned drill points for completion
     * Pending tasks
   - System displays Operator Dashboard with widgets:
     * My Project (card)
     * My Machine (card with details)
     * Drill Points to Complete (map view)
     * Today's Tasks (checklist)
     * Quick Actions (Mark Drill Complete, View Blast Plan)
10. System displays all widgets with real-time data
11. System displays last updated timestamp
12. System provides refresh button for manual refresh
13. System auto-refreshes dashboard every 60 seconds (configurable)
14. User can click on any widget to drill down to detailed view
15. Use case ends successfully

**Exceptional Flow**:

**E1: Data Retrieval Failure**
- **Trigger**: Database query fails for any widget (Step 4-9)
- **Actions**:
  1. System logs error for failed query
  2. System displays error message in affected widget: "Unable to load data"
  3. System displays "Retry" button in widget
  4. Other widgets continue to load successfully
  5. User can click Retry to reload failed widget
  6. Use case continues (partial success)

**E2: No Data Available**
- **Trigger**: User has no assigned resources (e.g., Operator not assigned to project)
- **Actions**:
  1. System displays empty state in dashboard:
     - "You are not currently assigned to any project"
     - Suggestion: "Contact your manager for assignment"
  2. System disables action buttons
  3. Use case ends

**Alternate Flow**:

**A1: Filter Dashboard by Date Range**
- **Trigger**: User selects date range filter (Step 14)
- **Actions**:
  1. System displays date range picker
  2. User selects start and end dates
  3. System re-queries data filtered by date range
  4. System updates all widgets with filtered data
  5. System displays applied filter indicator
  6. Use case continues

**A2: Export Dashboard as Report**
- **Trigger**: User clicks "Export Dashboard" button (Step 14)
- **Actions**:
  1. System displays export options: PDF, Excel, PNG (image)
  2. User selects format
  3. System generates report with all widget data and charts
  4. System includes timestamp and user info
  5. System initiates download
  6. Use case continues

**A3: Customize Dashboard Layout**
- **Trigger**: User clicks "Customize Dashboard" (Step 14)
- **Actions**:
  1. System enables edit mode
  2. User can:
     - Drag and drop widgets to reorder
     - Show/hide specific widgets
     - Resize widgets
  3. User clicks "Save Layout"
  4. System saves user preferences
  5. System displays success: "Dashboard layout saved"
  6. Use case continues

---

## 13. USE CASE SUMMARY AND TRACEABILITY MATRIX

---

### 13.1 Complete Use Case List

| Use Case ID | Use Case Name | Primary Actor(s) | Module |
|------------|---------------|------------------|---------|
| UC-AUTH-001 | User Login | Any User | Authentication |
| UC-AUTH-002 | Password Reset Request | Any User | Authentication |
| UC-AUTH-003 | Complete Password Reset | Any User | Authentication |
| UC-AUTH-004 | User Logout | Authenticated User | Authentication |
| UC-AUTH-005 | Token Validation | System | Authentication |
| UC-USER-001 | Create New User Account | Admin | User Management |
| UC-USER-002 | Update User Account | Admin | User Management |
| UC-USER-003 | Delete User Account | Admin | User Management |
| UC-USER-004 | Search and Filter Users | Admin | User Management |
| UC-PROJECT-001 | Create New Project | Admin, Blasting Engineer | Project Management |
| UC-PROJECT-002 | Update Project Details | Admin, Blasting Engineer | Project Management |
| UC-PROJECT-003 | Delete Project | Admin | Project Management |
| UC-DRILL-001 | Import Drill Data from CSV | Blasting Engineer, Operator | Drilling Operations |
| UC-DRILL-002 | Create Drill Point Manually | Blasting Engineer, Operator | Drilling Operations |
| UC-BLAST-001 | Design Blast Sequence | Blasting Engineer | Blasting Operations |
| UC-BLAST-002 | Simulate Blast Sequence | Blasting Engineer | Blasting Operations |
| UC-BLAST-003 | Generate Blast Report | Blasting Engineer | Reporting |
| UC-MACHINE-001 | Add Machine to Inventory | Machine Manager, Admin | Machine Management |
| UC-MACHINE-002 | Assign Machine to Project | Machine Manager, Admin | Machine Management |
| UC-MACHINE-003 | Unassign Machine | Machine Manager, Admin | Machine Management |
| UC-INVENTORY-001 | Add Explosive Batch | Explosive Manager | Explosive Inventory |
| UC-STORE-001 | Create New Store | Explosive Manager, Admin | Store Management |
| UC-STORE-002 | Create Transfer Request | Store Manager | Inventory Transfer |
| UC-APPROVAL-001 | Approve Transfer Request | Explosive Manager | Approval Workflow |
| UC-APPROVAL-002 | Reject Transfer Request | Explosive Manager | Approval Workflow |
| UC-REPORT-001 | View Dashboard Analytics | All Authenticated Users | Reporting & Analytics |

### 13.2 Use Case to Functional Requirements Traceability

| Use Case | Related Functional Requirements |
|----------|--------------------------------|
| UC-AUTH-001 | REQ-AUTH-001 to REQ-AUTH-010, REQ-AUTH-020 to REQ-AUTH-022, REQ-AUTH-046 |
| UC-AUTH-002 | REQ-AUTH-030 to REQ-AUTH-032 |
| UC-AUTH-003 | REQ-AUTH-033 to REQ-AUTH-036 |
| UC-AUTH-004 | REQ-AUTH-010 |
| UC-AUTH-005 | REQ-AUTH-004, REQ-AUTH-005 |
| UC-USER-001 | REQ-USER-001 to REQ-USER-008 |
| UC-USER-002 | REQ-USER-020 to REQ-USER-027 |
| UC-USER-003 | REQ-USER-024 to REQ-USER-026 |
| UC-USER-004 | REQ-USER-030 to REQ-USER-033 |
| UC-PROJECT-001 | REQ-PROJECT-001 to REQ-PROJECT-006 |
| UC-PROJECT-002 | REQ-PROJECT-020 to REQ-PROJECT-031 |
| UC-PROJECT-003 | REQ-PROJECT-032 to REQ-PROJECT-034 |
| UC-DRILL-001 | REQ-DRILL-020 to REQ-DRILL-027 |
| UC-DRILL-002 | REQ-DRILL-010 to REQ-DRILL-013, REQ-DRILL-030 to REQ-DRILL-032 |
| UC-BLAST-001 | REQ-BLAST-001 to REQ-BLAST-023 |
| UC-BLAST-002 | REQ-BLAST-030 to REQ-BLAST-036 |
| UC-BLAST-003 | REQ-REPORT-001 to REQ-REPORT-005 |
| UC-MACHINE-001 | REQ-MACHINE-001 to REQ-MACHINE-004 |
| UC-MACHINE-002 | REQ-MACHINE-030 to REQ-MACHINE-038 |
| UC-MACHINE-003 | REQ-MACHINE-030 to REQ-MACHINE-038 |
| UC-INVENTORY-001 | REQ-CENTRAL-001 to REQ-CENTRAL-006, REQ-CENTRAL-050 to REQ-CENTRAL-054 |
| UC-STORE-001 | REQ-STORE-001 to REQ-STORE-007 |
| UC-STORE-002 | REQ-TRANSFER-001 to REQ-TRANSFER-007 |
| UC-APPROVAL-001 | REQ-TRANSFER-030 to REQ-TRANSFER-035 |
| UC-APPROVAL-002 | REQ-TRANSFER-040 to REQ-TRANSFER-043 |
| UC-REPORT-001 | REQ-REPORT-060 to REQ-REPORT-061 |

---

## 14. CONCLUSION

This document provides comprehensive descriptive use cases for the Drilling & Blasting Management System (DBMS). Each use case is detailed with:

- **Clear identification** with unique IDs
- **Actor definitions** specifying who interacts with the system
- **Preconditions** ensuring the system state is ready
- **Postconditions** defining the expected outcome
- **Basic flow** describing the normal sequence of steps
- **Exceptional flows** handling error conditions and invalid inputs
- **Alternate flows** providing alternative paths to achieve goals

### Coverage Summary:
- **26 detailed use cases** covering all major system functions
- **8 primary actor roles** with specific responsibilities
- **Complete traceability** to functional requirements
- **Real-world scenarios** based on actual implementation
- **Comprehensive error handling** for robust system behavior

### Next Steps:
1. Review use cases with stakeholders for accuracy
2. Validate against system implementation
3. Create UI/UX mockups aligned with use case flows
4. Develop test cases based on use case scenarios
5. Update use cases as system evolves

---

**Document Version:** 1.0
**Last Updated:** October 22, 2025
**Status:** Complete - Ready for Review

---

**END OF DOCUMENT**
