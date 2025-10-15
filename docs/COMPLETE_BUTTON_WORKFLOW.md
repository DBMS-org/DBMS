# Complete Button Workflow - Site Completion Process

## 📍 Location
**URL**: `blasting-engineer/project-management/4/sites`
**Component**: `project-sites.component.ts`
**Button**: Complete button in site cards

---

## 🔒 When is the Complete Button DISABLED?

The Complete button is **disabled** when the `canCompleteSite()` method returns `false`.

### Code Logic ([project-sites.component.ts:171-179](Presentation/UI/src/app/components/blasting-engineer/project-management/project-sites/project-sites.component.ts#L171-L179)):

```typescript
canCompleteSite(site: ProjectSite): boolean {
  // A site can be completed if:
  // 1. Pattern is approved (isPatternApproved = true)
  // 2. Explosive approval has been requested (isExplosiveApprovalRequested = true)
  // 3. Site is not already completed by operator
  return site.isPatternApproved &&
         site.isExplosiveApprovalRequested &&
         !site.isOperatorCompleted;
}
```

### ❌ Button is DISABLED if ANY of these conditions are FALSE:
1. ❌ `isPatternApproved = false` - Pattern not approved for operator
2. ❌ `isExplosiveApprovalRequested = false` - No explosive approval request sent
3. ❌ `isOperatorCompleted = true` - Site already completed

---

## ✅ When is the Complete Button ENABLED (Clickable)?

The button becomes **ENABLED** when **ALL** three requirements are met:

| Requirement | Field | Value Required | How to Achieve |
|------------|-------|----------------|----------------|
| ✅ **Pattern Approved** | `isPatternApproved` | `true` | Blasting Engineer approves pattern in Site Dashboard |
| ✅ **Explosive Approval Requested** | `isExplosiveApprovalRequested` | `true` | Blasting Engineer requests explosive approval in Site Dashboard |
| ✅ **Not Already Completed** | `isOperatorCompleted` | `false` | Site hasn't been completed yet |

---

## 🔄 Process: From DISABLED to CLICKABLE

### Step-by-Step Workflow

```
┌─────────────────────────────────────────────────────────────┐
│                    INITIAL STATE                             │
│                 Complete Button: DISABLED                    │
│                                                              │
│  Missing Requirements:                                       │
│  ❌ Pattern approval                                        │
│  ❌ Explosive approval request                              │
└─────────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────────┐
│              STEP 1: CREATE DRILLING PATTERN                 │
│                                                              │
│  Location: Site Dashboard > Pattern Creator                 │
│  Action: Blasting Engineer creates drilling pattern         │
│  Result: Pattern data saved                                 │
│                                                              │
│  Complete Button: STILL DISABLED                            │
│  Missing Requirements:                                       │
│  ❌ Pattern approval                                        │
│  ❌ Explosive approval request                              │
└─────────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────────┐
│              STEP 2: APPROVE PATTERN                         │
│                                                              │
│  Location: Site Dashboard                                   │
│  Button: "Approve Pattern for Operator"                     │
│  Action: Click approve button                               │
│  Backend: POST /api/projectsites/{siteId}/approve           │
│  Result: isPatternApproved = true ✅                        │
│                                                              │
│  Complete Button: STILL DISABLED                            │
│  Missing Requirements:                                       │
│  ✅ Pattern approval (DONE)                                 │
│  ❌ Explosive approval request                              │
└─────────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────────┐
│        STEP 3: REQUEST EXPLOSIVE APPROVAL                    │
│                                                              │
│  Location: Site Dashboard                                   │
│  Button: "Request Explosive Approval"                       │
│  Form Fields:                                               │
│    - Expected Usage Date (required)                         │
│    - Blasting Date (optional)                               │
│    - Blast Timing (optional)                                │
│    - Comments (optional)                                    │
│  Action: Fill form and submit                               │
│  Backend: POST /api/explosive-approval-requests             │
│  Result: isExplosiveApprovalRequested = true ✅             │
│                                                              │
│  ──────────────────────────────────────────────────────     │
│  🎉 Complete Button: NOW ENABLED! 🎉                        │
│  ──────────────────────────────────────────────────────     │
│  All Requirements Met:                                       │
│  ✅ Pattern approval (DONE)                                 │
│  ✅ Explosive approval request (DONE)                       │
│  ✅ Not already completed (DONE)                            │
└─────────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────────┐
│          STEP 4: CLICK COMPLETE BUTTON                       │
│                                                              │
│  Location: Project Sites List                               │
│  URL: blasting-engineer/project-management/4/sites          │
│  Button: Green "Complete" button                            │
│  Action: Click the button                                   │
│                                                              │
│  1. Confirmation Dialog Appears:                            │
│     "Are you sure you want to mark '{siteName}' as          │
│      complete? This action cannot be undone."               │
│                                                              │
│  2. User clicks "Yes, Complete"                             │
│                                                              │
│  3. Backend Call:                                           │
│     POST /api/projectsites/{siteId}/complete                │
│                                                              │
│  4. Backend Updates:                                        │
│     - isOperatorCompleted = true                            │
│     - completedAt = current UTC time                        │
│     - completedByUserId = current user ID                   │
│                                                              │
│  5. Frontend Updates:                                       │
│     - Site marked as completed in UI                        │
│     - Success toast notification shown                      │
│     - Complete button becomes DISABLED again                │
│                                                              │
└─────────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────────┐
│                  FINAL STATE                                 │
│              Complete Button: DISABLED                       │
│                                                              │
│  Site Status: COMPLETED ✅                                  │
│  Reason: isOperatorCompleted = true                         │
│  Tooltip: "Site is already completed by operator"           │
└─────────────────────────────────────────────────────────────┘
```

---

## 🎯 Summary Table

| Step | Action | Location | Backend Endpoint | Field Updated | Button Status |
|------|--------|----------|------------------|---------------|---------------|
| 1 | Create Pattern | Site Dashboard > Pattern Creator | Various drill point APIs | N/A | ❌ Disabled |
| 2 | Approve Pattern | Site Dashboard | `POST /api/projectsites/{id}/approve` | `isPatternApproved = true` | ❌ Disabled |
| 3 | Request Explosives | Site Dashboard | `POST /api/explosive-approval-requests` | `isExplosiveApprovalRequested = true` | ✅ **ENABLED** |
| 4 | Click Complete | Project Sites List | `POST /api/projectsites/{id}/complete` | `isOperatorCompleted = true` | ❌ Disabled (completed) |

---

## 💡 Key Points

### What Makes the Button Enabled?
- **BOTH** flags must be `true`:
  1. `isPatternApproved = true`
  2. `isExplosiveApprovalRequested = true`
- **AND** site must NOT be completed yet (`isOperatorCompleted = false`)

### What Does NOT Affect the Button?
- ❌ Explosive approval being **approved** by Store Manager (not required)
- ❌ Blasting date/timing being set (not required)
- ❌ Store Manager's response status (not checked)

### Important Notes:
1. **Explosive Approval Request vs Approval**:
   - Only the **REQUEST** is required (`isExplosiveApprovalRequested`)
   - Store Manager's **APPROVAL** is NOT required for site completion
   - This allows Blasting Engineers to complete sites before Store Manager responds

2. **Timing Not Required**:
   - Blasting date and timing are optional when requesting explosive approval
   - Store Manager can update timing later in Proposal History
   - Store Manager CANNOT approve until timing is set (but this doesn't block site completion)

3. **One-Way Action**:
   - Once completed, the button stays disabled permanently
   - The action cannot be undone
   - This is enforced by the `isOperatorCompleted` check

---

## 🔍 Tooltip Messages

The button shows helpful tooltips based on the current state:

```typescript
getCompleteButtonTooltip(site: ProjectSite): string {
  if (site.isOperatorCompleted) {
    return 'Site is already completed by operator';
  }

  const missingRequirements: string[] = [];

  if (!site.isPatternApproved) {
    missingRequirements.push('Pattern approval');
  }

  if (!site.isExplosiveApprovalRequested) {
    missingRequirements.push('Explosive approval request');
  }

  if (missingRequirements.length > 0) {
    return `Missing requirements: ${missingRequirements.join(', ')}`;
  }

  return 'Mark site as complete';
}
```

### Example Tooltips:
- ✅ All requirements met: **"Mark site as complete"**
- ❌ Missing all: **"Missing requirements: Pattern approval, Explosive approval request"**
- ❌ Already completed: **"Site is already completed by operator"**

---

## 🧪 Testing the Button State

### Test Case 1: Fresh Site (All Requirements Missing)
```typescript
site = {
  isPatternApproved: false,
  isExplosiveApprovalRequested: false,
  isOperatorCompleted: false
}
// Result: Button DISABLED
// Tooltip: "Missing requirements: Pattern approval, Explosive approval request"
```

### Test Case 2: Pattern Approved Only
```typescript
site = {
  isPatternApproved: true,
  isExplosiveApprovalRequested: false,
  isOperatorCompleted: false
}
// Result: Button DISABLED
// Tooltip: "Missing requirements: Explosive approval request"
```

### Test Case 3: All Requirements Met (Button Enabled!)
```typescript
site = {
  isPatternApproved: true,
  isExplosiveApprovalRequested: true,
  isOperatorCompleted: false
}
// Result: Button ENABLED ✅
// Tooltip: "Mark site as complete"
```

### Test Case 4: Site Already Completed
```typescript
site = {
  isPatternApproved: true,
  isExplosiveApprovalRequested: true,
  isOperatorCompleted: true
}
// Result: Button DISABLED
// Tooltip: "Site is already completed by operator"
```

---

## 📊 Visual State Diagram

```
                    Initial State
                         │
                         ▼
            ┌────────────────────────┐
            │  All Requirements = ❌  │
            │  Button: DISABLED      │
            └────────────────────────┘
                         │
                         │ Approve Pattern
                         ▼
            ┌────────────────────────┐
            │  Pattern Approved = ✅  │
            │  Explosive Req = ❌     │
            │  Button: DISABLED      │
            └────────────────────────┘
                         │
                         │ Request Explosives
                         ▼
            ┌────────────────────────┐
            │  Pattern Approved = ✅  │
            │  Explosive Req = ✅     │
            │  Button: ENABLED ✅    │
            └────────────────────────┘
                         │
                         │ Click Complete
                         ▼
            ┌────────────────────────┐
            │  Completed = ✅         │
            │  Button: DISABLED      │
            │  (Final State)         │
            └────────────────────────┘
```

---

**Last Updated**: October 15, 2025
**Component**: `project-sites.component.ts`
**Button Location**: `project-sites.component.html:133-142`
