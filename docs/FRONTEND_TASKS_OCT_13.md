# Frontend Development Tasks - October 13, 2025

## Overview
This document outlines the frontend implementation tasks for enhancing the blasting engineer request workflow, store manager approval flow, and site management features.

---

## 1. Add Complete Button in Sites List (Blasting Engineer)

### Purpose
Allow blasting engineers to mark sites as complete from the sites list view.

### Implementation Location
- **Component**: `blasting-engineer/project-management/project-sites/project-sites.component.ts`
- **Template**: `blasting-engineer/project-management/project-sites/project-sites.component.html`
- **Route**: `/blasting-engineer/project-management/{projectId}/sites`

### Requirements
- Add "Complete" button for each site in the sites list
- Button should be disabled until all required criteria are met:
  - All workflow steps completed
  - Pattern approved for operator
  - Simulation confirmed
  - Explosive approval request submitted
- Button should show appropriate tooltip when disabled
- Confirmation dialog before marking site as complete
- Success/error notification after action

### Business Rules
- Blasting date and timing are NOT required for site completion
- Site can be completed even if timing hasn't been specified yet
- Once completed, site status should update and be visible across the system

---

## 2. Add Blasting Date & Timing in Multiple Locations

### Purpose
Allow blasting engineers to specify or update blasting date and timing from TWO different locations for flexibility.

---

### 2A. Option 1: Site Dashboard (During Explosive Approval Request)

### Implementation Location
- **Component**: `blasting-engineer/project-management/site-dashboard/site-dashboard.component.ts`
- **Template**: `blasting-engineer/project-management/site-dashboard/site-dashboard.component.html`
- **Location in Template**: Lines 351-418 (Explosive Approval Modal)

### Requirements
- **Modify existing explosive approval modal** to include blasting date and timing fields
- Add two new optional fields:
  - **Blasting Date**: Date picker input
  - **Blast Timing**: Time input (24-hour format)
- These fields are OPTIONAL when creating the request
- BE can submit request without date/timing
- NO validation on timing - BE can set any time

### Modal Fields
1. Expected Usage Date (existing - required)
2. **Blasting Date (NEW - optional)**
3. **Blast Timing (NEW - optional)**
4. Comments (existing - optional)
5. Explosive totals summary (existing)

### Form Structure Update
Current explosive approval form needs new fields:
- `expectedUsageDate: string` (existing)
- `blastingDate: string` (NEW)
- `blastTiming: string` (NEW)
- `comments: string` (existing)

### User Flow - Site Dashboard
1. BE completes all workflow steps
2. BE clicks "Request Explosive Approval"
3. Modal opens with fields:
   - Expected usage date (required)
   - Blasting date (optional - can set now or later)
   - Blast timing (optional - can set now or later)
   - Comments (optional)
4. BE can choose to:
   - Fill all fields including timing → Submit with complete info
   - Fill only expected usage date → Submit, set timing later
5. Request created successfully either way

---

### 2B. Option 2: Proposal History (After Request Creation)

### Implementation Location
- **Component**: `blasting-engineer/proposal-history/proposal-history.component.ts`
- **Template**: `blasting-engineer/proposal-history/proposal-history.component.html`

### Requirements
- Display list of all explosive approval requests created by the user
- For each request, show:
  - Project site name
  - Request status (Pending/Approved/Rejected)
  - Expected usage date
  - Current blasting date (if set, otherwise "Not Set")
  - Current blast timing (if set, otherwise "Not Set")
- Provide "Set Timing" or "Update Timing" button for each request
- Modal dialog for entering/editing blasting date and timing
- NO validation on timing - BE can set any time they want (24-hour format)
- Can update timing even if already set
- Save button disabled until both date and timing are filled

### User Flow - Proposal History
1. BE goes to proposal history page
2. Sees list of all their explosive requests
3. For requests with "Not Set" timing, clicks "Set Timing"
4. For requests with existing timing, clicks "Update Timing"
5. Modal opens with:
   - Date picker (pre-filled if already set)
   - Time input (pre-filled if already set)
6. BE enters or modifies date and timing
7. BE clicks Save
8. Request updates with new timing
9. List refreshes showing updated values

### Display States
- **Not Set**: Show "Not Set" label with warning icon
- **Set**: Show actual date/time values with edit icon
- **After Update**: Show success message and refreshed data

---

### Key Difference Between Two Options
- **Site Dashboard**: Set timing when initially creating request (optional)
- **Proposal History**: Set or update timing anytime after request exists (can do multiple times)
- Both locations update the same request timing data
- BE has flexibility to choose when to specify timing

---

## 3. Store Manager Request Approval Logic

### Purpose
Store Manager can view all requests, but can only approve if blasting date and timing are specified. Can reject anytime.

### Implementation Location
- **Component**: `store-manager/blasting-engineer-requests/request-details/request-details.component.ts`
- **Template**: `store-manager/blasting-engineer-requests/request-details/request-details.component.html`

### Requirements

#### View Mode (Always Available)
- SM can always open and view complete request details
- Display all request information including:
  - Request metadata (ID, date, requester)
  - Project and site information
  - Expected usage date
  - **Blasting date** (with "Not Specified" warning if missing)
  - **Blast timing** (with "Not Specified" warning if missing)
  - Comments
  - Explosive calculations (existing)
  - Processing history (if applicable)

#### Approval Mode (Conditional)
SM can only approve if ALL of the following are true:
- Request status is "Pending"
- Blasting date IS specified
- Blast timing IS specified

If any condition is not met:
- Disable the Approve button
- Show clear message explaining why approval is blocked
- Example messages:
  - "Cannot approve: Blasting Date not specified by Blasting Engineer"
  - "Cannot approve: Blast Timing not specified by Blasting Engineer"
  - "Cannot approve: Blasting Date and Blast Timing not specified by Blasting Engineer"

#### Rejection Mode (Always Available for Pending)
- SM can reject any pending request at any time
- Rejection does NOT require date/timing to be specified
- Rejection modal should ask for rejection reason
- Rejection reason is stored and visible to all users

### UI Changes Required

#### Add Blasting Schedule Section
Insert new section after Explosive Calculations Section (around line 146):
- Section header with icon
- Two info items:
  - Blasting Date (show value or "Not Specified")
  - Blast Timing (show value or "Not Specified")
- Information note when missing: "Blasting Engineer needs to specify date and timing before approval"

#### Update Modal Footer
Update footer section (lines 214-233):
- Add approval requirements message box
- Show message when approval conditions not met
- Update approve button to have proper disabled state and tooltip
- Keep reject button always enabled for pending requests
- Style buttons appropriately

### Visual Indicators
- Use warning color (#f59e0b) for missing date/timing
- Use success color (#10b981) when date/timing specified
- Warning icon for missing information
- Check icon for specified information

---

## Component Relationships & Data Flow

### Blasting Engineer Flow

**Path A: Set Timing During Request Creation (Site Dashboard)**
1. Complete all workflow steps
2. Click "Request Explosive Approval"
3. Fill expected usage date (required)
4. **Optionally fill blasting date and timing**
5. Submit request
6. Request created with timing (if provided)

**Path B: Set Timing After Request Creation (Proposal History)**
1. Request already exists (created with or without timing)
2. Navigate to Proposal History
3. Find request needing timing
4. Click "Set Timing" or "Update Timing"
5. Enter/modify date and timing
6. Save changes
7. Request updated with timing

**Final Step: Complete Site**
1. Go to Sites List
2. Verify completion criteria met
3. Click "Complete" button
4. Confirm action
5. Site marked as complete

### Store Manager Flow

1. View Requests List
2. Click request to view details
3. Review all information including blasting schedule
4. Check timing status:
   - **If timing specified**: Can APPROVE or REJECT
   - **If timing missing**: Can only VIEW or REJECT
5. Make decision and process request
6. Approved requests go to Explosive Manager

---

## Models & Interfaces Updates

### ExplosiveApprovalRequest
**New/Updated Fields:**
- `blastingDate?: string` - Optional, ISO date format
- `blastTiming?: string` - Optional, 24-hour time format (HH:mm)

### ProjectSite
**New Fields for Completion:**
- `isCompleted: boolean` - Indicates if site workflow is complete
- `completedAt?: string` - Timestamp when site was completed
- `completedByUserId?: number` - User who marked site as complete

---

## API Endpoints Required

### 1. Create/Update Explosive Approval Request (Modified)
**Endpoint**: `POST /api/projectsites/{siteId}/request-explosive-approval`

**Description**: Creates explosive approval request with optional timing

**Request Body**:
- `expectedUsageDate: string` (required)
- `blastingDate?: string` (optional - NEW)
- `blastTiming?: string` (optional - NEW)
- `comments?: string` (optional)

**Response**: Created ExplosiveApprovalRequest object

---

### 2. Update Blasting Date & Timing
**Endpoint**: `PUT /api/explosive-approval-requests/{requestId}/timing`

**Description**: Updates blasting date and timing for existing request

**Request Body**:
- `blastingDate: string` (required)
- `blastTiming: string` (required)

**Response**: Updated ExplosiveApprovalRequest object

---

### 3. Get Explosive Requests for User
**Endpoint**: `GET /api/explosive-approval-requests/by-user/{userId}`

**Description**: Retrieves all explosive approval requests for proposal history

**Response**: Array of ExplosiveApprovalRequest objects

---

### 4. Complete Site
**Endpoint**: `POST /api/projectsites/{siteId}/complete`

**Description**: Marks site as complete after criteria met

**Response**: Updated ProjectSite object

---

### 5. Approve Request (Update Validation)
**Endpoint**: `POST /api/explosive-approval-requests/{requestId}/approve`

**Description**: Approves request (requires timing specified)

**Backend Validation**:
- Request status must be Pending
- Blasting date must be specified
- Blast timing must be specified
- User must have Store Manager role

**Response**: Updated ExplosiveApprovalRequest with approval details

---

## Styling Guidelines

### Complete Button (Sites List)
- Primary success color with gradient
- Icon + text label
- Hover effects with elevation
- Clear disabled state
- Tooltip explaining requirements

### Timing Inputs (Site Dashboard Modal)
- Date picker for blasting date
- Time input (24-hour) for blast timing
- Optional field indicators
- Form hints below inputs
- Consistent with existing modal styling

### Timing Modal (Proposal History)
- Clean modern modal design
- Date picker and time input
- Pre-fill existing values
- Save button disabled until both filled
- Cancel button secondary style

### Blasting Schedule Section (Store Manager)
- Warning style for missing info (orange/amber)
- Success style for specified info (green)
- Clear visual hierarchy
- Message box for requirements
- Icons for each state

### Status Indicators
- Not Specified: Orange with warning icon
- Specified: Green with check icon
- Pending: Blue with clock icon
- Approved: Green with check circle
- Rejected: Red with cancel icon

---

## Testing Checklist

### BE - Site Dashboard (Explosive Approval Modal)
- [ ] Modal opens correctly
- [ ] Expected usage date required
- [ ] Blasting date field appears (optional)
- [ ] Blast timing field appears (optional)
- [ ] Can submit without date/timing
- [ ] Can submit with date/timing
- [ ] Explosive totals display correctly
- [ ] Success message after submission
- [ ] Error handling works

### BE - Proposal History
- [ ] All user requests display
- [ ] Shows current timing status (set or not set)
- [ ] "Set Timing" appears when not set
- [ ] "Update Timing" appears when set
- [ ] Modal opens with correct pre-filled data
- [ ] Can set new timing
- [ ] Can update existing timing
- [ ] Save disabled until both fields filled
- [ ] Success message displays
- [ ] List refreshes after save

### BE - Sites List
- [ ] Complete button appears
- [ ] Button disabled when criteria not met
- [ ] Button enabled when criteria met
- [ ] Tooltip shows requirements
- [ ] Confirmation dialog works
- [ ] Success notification displays
- [ ] Site status updates

### SM - Request Details
- [ ] Can view all requests
- [ ] Blasting schedule section appears
- [ ] Missing timing shows warnings
- [ ] Specified timing shows correctly
- [ ] Approve disabled when timing missing
- [ ] Clear message explains blocking
- [ ] Approve enabled when timing present
- [ ] Reject always available for pending
- [ ] Success/error messages display

### Integration
- [ ] Timing set in dashboard persists
- [ ] Timing updated in history persists
- [ ] SM sees BE's timing updates
- [ ] Full workflow works end-to-end
- [ ] Multiple timing updates work
- [ ] Data consistency maintained

---

## Implementation Order

1. **Update Models/Interfaces**
   - Add new optional fields to ExplosiveApprovalRequest
   - Add completion fields to ProjectSite

2. **Modify Site Dashboard Explosive Modal**
   - Add blasting date input field
   - Add blast timing input field
   - Update form submission to include new fields
   - Update API call

3. **Implement Proposal History**
   - Create timing modal component
   - Add timing display to requests list
   - Implement set/update timing functionality
   - Add API service methods

4. **Update Store Manager Request Details**
   - Add blasting schedule section
   - Update approval button logic
   - Add requirement messages
   - Update styling

5. **Add Complete Button to Sites List**
   - Add button to sites list UI
   - Implement completion logic
   - Add confirmation dialog
   - Add API integration

6. **End-to-End Testing**
   - Test all user paths
   - Verify data persistence
   - Test error scenarios

---

## Business Rules Summary

### Timing Specification
- Timing is optional when creating request (Site Dashboard)
- Timing can be set/updated anytime in Proposal History
- BE can update timing multiple times
- No validation on what time BE chooses
- Use 24-hour format for consistency

### Store Manager Approval
- Can view all requests anytime
- Can only approve if BOTH date AND timing specified
- Can reject anytime (with or without timing)
- Must provide rejection reason
- Acts as gatekeeper to ensure timing is set

### Site Completion
- All workflow steps must be completed
- Pattern must be approved for operator
- Simulation must be confirmed
- Explosive approval request must be submitted
- Blasting date/timing NOT required for completion

---

## Notes & Considerations

- **Flexibility**: BE can choose when to specify timing (during request or later)
- **Data Consistency**: Both locations update same request data
- **Timezone**: Consider timezone handling for date/time
- **Audit Trail**: Log all timing updates and approvals
- **Notifications**: Consider notifications for status changes
- **Permissions**: Enforce proper role-based access
- **Validation**: Backend must validate all rules
- **Error Handling**: Graceful error recovery
- **Loading States**: Show loading indicators
- **Optimistic Updates**: Consider for better UX

---

## Future Enhancements

1. **Notification System**
   - Alert SM when timing is added after initial review
   - Notify BE of approval/rejection

2. **Timing History**
   - Track all timing changes
   - Show who changed and when

3. **Calendar View**
   - Visual calendar of scheduled blasts
   - Conflict detection

4. **Bulk Operations**
   - Update timing for multiple requests
   - Batch approvals

5. **Advanced Features**
   - Weather integration for timing recommendations
   - Safety checklist before site completion
   - Mobile-responsive views
   - PDF export of proposals
   - Analytics and reporting
