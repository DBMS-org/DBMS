# 🔔 Automatic Notification Implementation Plan

## Executive Summary

This plan outlines the implementation of automatic notification creation across all backend workflows. The frontend notification system is **already complete** and ready to receive notifications. This plan focuses on adding notification creation in backend repositories so users are automatically notified when important events occur.

---

## Current Status Analysis

### ✅ Already Implemented
1. **Frontend Notification System** - 100% Complete
   - All 7 role-based notification pages working
   - Connected to backend API with 30-second polling
   - Role-specific filtering operational
   - CRUD operations fully functional

2. **Backend Infrastructure** - 100% Complete
   - Notification database table with 5 indexes
   - NotificationRepository with 15+ methods
   - NotificationsController with 11 REST endpoints
   - Domain entities (Notification, NotificationType, NotificationPriority)

3. **Explosive Approval Request Workflow** - 100% Complete ✅
   - ✅ Create notification → Store Managers (lines 113-169)
   - ✅ Approve notification → Blasting Engineer (lines 235-262)
   - ✅ Reject notification → Blasting Engineer (lines 300-327)

### ❌ NOT Implemented (Needs Work)
1. **Inventory Transfer Request Workflow** - 0% Complete
2. **Machine Assignment Request Workflow** - 0% Complete
3. **Machine Assignment Workflow** - 0% Complete
4. **Maintenance Report Workflow** - 0% Complete
5. **Maintenance Job Workflow** - 0% Complete

---

## Implementation Plan

### Phase 1: Inventory Transfer Request Notifications
**Repository**: `InventoryTransferRequestRepository.cs`
**Estimated Time**: 2-3 hours

#### Workflow Events & Notifications

| Event | Trigger | Recipient | Type | Priority | Action URL |
|-------|---------|-----------|------|----------|------------|
| **Request Created** | Store Manager creates transfer request | Explosive Manager | `TransferRequestCreated` (200) | Normal/High | `/explosive-manager/requests/{id}` |
| **Request Approved** | Explosive Manager approves request | Store Manager (requester) | `TransferRequestApproved` (202) | High | `/store-manager/request-history/{id}` |
| **Request Rejected** | Explosive Manager rejects request | Store Manager (requester) | `TransferRequestRejected` (203) | High | `/store-manager/request-history/{id}` |
| **Transfer Dispatched** | Explosive Manager dispatches transfer | Store Manager (destination) | `TransferDispatched` (204) | High | `/store-manager/request-history/{id}` |
| **Transfer Completed** | Transfer marked as completed | Store Manager (destination) | `TransferCompleted` (205) | Normal | `/store-manager/request-history/{id}` |

#### Implementation Steps

1. **Add Dependencies** (Constructor Injection)
```csharp
private readonly INotificationRepository _notificationRepository;
private readonly IUserRepository _userRepository;

public InventoryTransferRequestRepository(
    ApplicationDbContext context,
    INotificationRepository notificationRepository,
    IUserRepository userRepository)
{
    _context = context;
    _notificationRepository = notificationRepository;
    _userRepository = userRepository;
}
```

2. **Add Notification in `CreateAsync()` Method**
   - Find all Explosive Managers
   - Create notification with transfer details
   - Handle errors gracefully (don't fail creation if notification fails)

3. **Add Notification in `ApproveAsync()` Method**
   - Notify the Store Manager who created the request
   - Include approved quantity

4. **Add Notification in `RejectAsync()` Method**
   - Notify the Store Manager who created the request
   - Include rejection reason

5. **Add Notification in `DispatchAsync()` Method**
   - Notify the destination Store Manager
   - Include dispatch details (truck number, driver, etc.)

6. **Add Notification in `CompleteAsync()` Method**
   - Notify the destination Store Manager
   - Confirm completion

---

### Phase 2: Machine Assignment Request Notifications
**Repository**: Need to identify (likely `MachineAssignmentRequestRepository.cs`)
**Estimated Time**: 2-3 hours

#### Workflow Events & Notifications

| Event | Trigger | Recipient | Type | Priority | Action URL |
|-------|---------|-----------|------|----------|------------|
| **Request Created** | User creates machine request | Machine Manager | `MachineRequestCreated` (300) | Normal/High/Urgent | `/machine-manager/requests/{id}` |
| **Request Approved** | Machine Manager approves | Requester | `MachineRequestApproved` (301) | High | `/admin/projects` or requester dashboard |
| **Request Rejected** | Machine Manager rejects | Requester | `MachineRequestRejected` (302) | High | `/admin/projects` or requester dashboard |
| **Request Partially Fulfilled** | Some machines assigned | Requester | `MachineRequestPartial` (303) | Medium | `/admin/projects` |
| **Request Completed** | All machines assigned | Machine Manager + Requester | `MachineRequestCompleted` (304) | Normal | Both dashboards |

#### Implementation Steps

1. **Identify Repository File**
   - Search for machine assignment request CRUD operations
   - Likely in `Infrastructure/Repositories/MachineManagement/`

2. **Add Dependencies**
   - Inject `INotificationRepository` and `IUserRepository`

3. **Add Notifications in Key Methods**
   - `CreateAsync()` → Notify Machine Manager
   - `ApproveAsync()` → Notify requester
   - `RejectAsync()` → Notify requester
   - `AssignPartialAsync()` → Notify requester
   - `CompleteAsync()` → Notify both

---

### Phase 3: Machine Assignment Notifications
**Repository**: `MachineAssignmentRepository.cs` (if exists, or service layer)
**Estimated Time**: 1-2 hours

#### Workflow Events & Notifications

| Event | Trigger | Recipient | Type | Priority | Action URL |
|-------|---------|-----------|------|----------|------------|
| **Machine Assigned** | Machine assigned to operator | Operator | `MachineAssigned` (400) | High | `/operator/my-machines` |
| **Machine Returned** | Operator returns machine | Machine Manager | `MachineReturned` (402) | Normal | `/machine-manager/assignments` |
| **Assignment Cancelled** | Assignment cancelled | Operator (if assigned) | `MachineAssignmentCancelled` (403) | Normal | `/operator/my-machines` |

#### Implementation Steps

1. **Locate Assignment Logic**
   - Could be in repository or service layer
   - Find where machine is actually assigned to operator

2. **Add Notification Creation**
   - After successful assignment → Notify operator
   - After return → Notify Machine Manager
   - After cancellation → Notify operator

---

### Phase 4: Maintenance Report Notifications
**Repository**: `MaintenanceReportRepository.cs`
**Estimated Time**: 2-3 hours

#### Workflow Events & Notifications

| Event | Trigger | Recipient | Type | Priority | Action URL |
|-------|---------|-----------|------|----------|------------|
| **Report Created** | Operator submits report | Mechanical Engineer (available) | `MaintenanceReportCreated` (500) | High/Urgent | `/mechanical-engineer/reports/{id}` |
| **Report Acknowledged** | Engineer acknowledges | Operator (reporter) | `MaintenanceReportAcknowledged` (501) | Normal | `/operator/maintenance-reports/{id}` |
| **Report In Progress** | Work started | Operator (reporter) | `MaintenanceReportInProgress` (502) | Normal | `/operator/maintenance-reports/{id}` |
| **Report Resolved** | Problem fixed | Operator (reporter) | `MaintenanceReportResolved` (503) | High | `/operator/maintenance-reports/{id}` |
| **Report Closed** | Report marked closed | Mechanical Engineer (assigned) | `MaintenanceReportClosed` (504) | Normal | `/mechanical-engineer/reports/{id}` |
| **Report Reopened** | Report reopened | Mechanical Engineer (assigned) | `MaintenanceReportReopened` (505) | High | `/mechanical-engineer/reports/{id}` |

#### Implementation Steps

1. **Add Dependencies**
   - Inject `INotificationRepository` and `IUserRepository`

2. **Find All Mechanical Engineers**
   - Use `_userRepository.GetByRoleAsync("MechanicalEngineer")`
   - Or assign to specific engineer if assignment logic exists

3. **Add Notifications**
   - `CreateAsync()` → Notify Mechanical Engineers
   - `AcknowledgeAsync()` → Notify operator
   - `StartWorkAsync()` → Notify operator
   - `ResolveAsync()` → Notify operator
   - `CloseAsync()` → Notify engineer
   - `ReopenAsync()` → Notify engineer

---

### Phase 5: Maintenance Job Notifications
**Repository**: `MaintenanceJobRepository.cs`
**Estimated Time**: 2-3 hours

#### Workflow Events & Notifications

| Event | Trigger | Recipient | Type | Priority | Action URL |
|-------|---------|-----------|------|----------|------------|
| **Job Created** | Scheduled maintenance created | Mechanical Engineer (assigned) | `MaintenanceJobCreated` (600) | Normal/High | `/mechanical-engineer/maintenance/jobs/{id}` |
| **Job Started** | Job work started | Creator (if different) | `MaintenanceJobStarted` (603) | Normal | `/mechanical-engineer/maintenance/jobs/{id}` |
| **Job Completed** | Job finished | Creator + Machine Manager | `MaintenanceJobCompleted` (604) | Normal | Both dashboards |
| **Job Cancelled** | Job cancelled | Mechanical Engineer (assigned) | `MaintenanceJobCancelled` (605) | Normal | `/mechanical-engineer/maintenance/jobs/{id}` |

#### Implementation Steps

1. **Add Dependencies**
   - Inject `INotificationRepository` and `IUserRepository`

2. **Add Notifications**
   - `CreateAsync()` → Notify assigned engineer
   - `StartAsync()` → Notify creator
   - `CompleteAsync()` → Notify creator and Machine Manager
   - `CancelAsync()` → Notify assigned engineer

---

## Code Pattern Template

### Standard Notification Pattern

```csharp
// At the end of the method, after SaveChangesAsync() but before return

if (result > 0)  // or after successful save
{
    try
    {
        // Load related data if needed
        var recipient = await _userRepository.GetByIdAsync(recipientUserId);

        // Create notification
        var notification = Notification.Create(
            userId: recipientUserId,
            type: NotificationType.YourTypeHere,
            title: "Notification Title",
            message: "Detailed message with {contextInfo}",
            priority: NotificationPriority.High,
            relatedEntityType: "EntityName",
            relatedEntityId: entityId,
            actionUrl: $"/role-name/path/{entityId}"
        );

        await _notificationRepository.CreateAsync(notification);

        _logger.LogInformation(
            "Created notification for user {UserId} for {EntityType} {EntityId}",
            recipientUserId, "EntityName", entityId);
    }
    catch (Exception notifEx)
    {
        // Log error but don't fail the main operation
        _logger.LogError(notifEx,
            "Error creating notification for {EntityType} {EntityId}",
            "EntityName", entityId);
    }
}
```

### Bulk Notification Pattern (Multiple Recipients)

```csharp
try
{
    // Find recipients
    var recipients = await _userRepository.GetByRoleAsync("RoleName");

    if (recipients.Any())
    {
        var notifications = recipients.Select(user =>
            Notification.Create(
                userId: user.Id,
                type: NotificationType.YourTypeHere,
                title: "Notification Title",
                message: "Message for all recipients",
                priority: NotificationPriority.Normal,
                relatedEntityType: "EntityName",
                relatedEntityId: entityId,
                actionUrl: $"/role-name/path/{entityId}"
            )
        ).ToList();

        await _notificationRepository.CreateBulkAsync(notifications);

        _logger.LogInformation(
            "Created {Count} notifications for {Role} for {EntityType} {EntityId}",
            notifications.Count, "RoleName", "EntityName", entityId);
    }
}
catch (Exception notifEx)
{
    _logger.LogError(notifEx,
        "Error creating notifications for {EntityType} {EntityId}",
        "EntityName", entityId);
}
```

---

## Testing Strategy

### Unit Testing (Optional but Recommended)

For each repository method with notifications:

```csharp
[Fact]
public async Task ApproveRequest_Should_CreateNotificationForRequester()
{
    // Arrange
    var mockNotificationRepo = new Mock<INotificationRepository>();
    var repository = new YourRepository(
        _context,
        mockNotificationRepo.Object,
        _mockUserRepo.Object);

    // Act
    await repository.ApproveAsync(requestId, approverId);

    // Assert
    mockNotificationRepo.Verify(
        x => x.CreateAsync(It.IsAny<Notification>()),
        Times.Once);
}
```

### Manual Testing Checklist

For each workflow:

1. **Create Trigger Event**
   - E.g., Create transfer request in frontend
   - Check database: `SELECT * FROM Notifications WHERE Type = 200`
   - Verify notification created with correct data

2. **Frontend Verification**
   - Login as recipient role
   - Navigate to notifications page
   - Wait 30 seconds (for polling)
   - Verify notification appears

3. **Interaction Testing**
   - Click notification
   - Verify navigation to correct page
   - Mark as read
   - Verify read status updates

4. **Error Handling**
   - Stop notification service
   - Perform action
   - Verify main operation still succeeds
   - Check error logs

---

## Implementation Order (Recommended)

### Week 1: High Priority
1. ✅ **Explosive Approval Requests** - Already complete
2. **Inventory Transfer Requests** - Store-to-warehouse transfers (critical business flow)
3. **Maintenance Reports** - Safety-critical notifications

### Week 2: Medium Priority
4. **Machine Assignments** - Operational efficiency
5. **Machine Assignment Requests** - Resource allocation

### Week 3: Lower Priority
6. **Maintenance Jobs** - Scheduled maintenance
7. **Additional Events** - System notifications, project updates

---

## Required Changes Summary

### Files to Modify

| File | Purpose | Changes Required |
|------|---------|------------------|
| `InventoryTransferRequestRepository.cs` | Transfer workflow | Add 5 notification points |
| `MachineAssignmentRequestRepository.cs` | Machine requests | Add 5 notification points |
| `MachineAssignmentRepository.cs` | Machine assignments | Add 3 notification points |
| `MaintenanceReportRepository.cs` | Maintenance reports | Add 6 notification points |
| `MaintenanceJobRepository.cs` | Maintenance jobs | Add 4 notification points |

**Total**: 5 files, ~23 notification integration points

### Dependencies to Add

Each repository needs:
```csharp
private readonly INotificationRepository _notificationRepository;
private readonly IUserRepository _userRepository;
```

### Service Registration (Verify in Program.cs)

Ensure these are registered:
```csharp
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
```

---

## Error Handling Strategy

### Principle: **Never Fail the Main Operation**

All notification creation must be wrapped in try-catch:

```csharp
try
{
    // Notification creation code
}
catch (Exception notifEx)
{
    // Log but don't throw
    _logger.LogError(notifEx, "Notification failed but operation succeeded");
}
```

### Rationale
- User actions (approve, reject, create) are critical
- Notifications are "nice to have" but not essential for data integrity
- System should degrade gracefully

---

## Monitoring & Observability

### Logging Standards

```csharp
// Success
_logger.LogInformation(
    "Created {NotificationType} notification for user {UserId}",
    notification.Type, userId);

// Failure
_logger.LogError(notifEx,
    "Failed to create {NotificationType} notification for {EntityType} {EntityId}",
    NotificationType.YourType, "EntityName", entityId);
```

### Metrics to Track (Future Enhancement)

1. **Notification Creation Rate**
   - Count of notifications created per hour
   - Success rate

2. **Notification Delivery**
   - Time from creation to user viewing
   - Read rate by notification type

3. **Notification Engagement**
   - Click-through rate on action URLs
   - Time to mark as read

---

## Database Considerations

### Current Schema (No Changes Needed)

The Notifications table already has:
- ✅ Proper indexes for performance
- ✅ Foreign key to Users table
- ✅ Cascade delete on user deletion
- ✅ All necessary fields

### Performance Notes

- Bulk inserts use `CreateBulkAsync()` for efficiency
- Indexes on `UserId`, `IsRead`, `CreatedAt`, `Type`
- Notifications auto-deleted when user is deleted (CASCADE)

---

## Migration & Rollout Strategy

### Phase 1: Development (This Plan)
1. Implement notifications in DEV environment
2. Test each workflow thoroughly
3. Verify frontend displays correctly

### Phase 2: Staging
1. Deploy to staging
2. End-to-end testing with real data
3. Performance testing (load testing if needed)

### Phase 3: Production
1. Deploy backend changes first
2. Monitor error logs for 24 hours
3. Verify notifications are being created
4. Gather user feedback

### Rollback Plan

If issues arise:
1. **Option A**: Disable notification creation
   - Wrap all notification code in feature flag
   - `if (_config.NotificationsEnabled) { ... }`

2. **Option B**: Roll back to previous version
   - Notifications won't break existing functionality
   - System degrades gracefully

---

## Success Metrics

### Technical Metrics
- ✅ All 23 notification points implemented
- ✅ Zero main operation failures due to notifications
- ✅ <100ms overhead for notification creation
- ✅ 100% of notifications delivered within 30 seconds

### User Metrics
- Users receive notifications for all workflow events
- <5% of notifications go unread for >24 hours
- Users report increased awareness of workflow status
- Reduced "where is my request" inquiries

---

## Estimated Timeline

| Phase | Tasks | Time | Total |
|-------|-------|------|-------|
| **Phase 1** | Inventory Transfer (5 points) | 3 hours | 3h |
| **Phase 2** | Machine Requests (5 points) | 3 hours | 6h |
| **Phase 3** | Machine Assignments (3 points) | 2 hours | 8h |
| **Phase 4** | Maintenance Reports (6 points) | 3 hours | 11h |
| **Phase 5** | Maintenance Jobs (4 points) | 2 hours | 13h |
| **Testing** | Manual testing all workflows | 3 hours | 16h |
| **Bug Fixes** | Address any issues found | 2 hours | 18h |

**Total Estimated Time: 18-20 hours (2-3 days of focused development)**

---

## Next Steps

1. **Review this plan** - Confirm approach and priorities
2. **Start with Phase 1** - Inventory Transfer Requests
3. **Implement incrementally** - One repository at a time
4. **Test thoroughly** - After each phase
5. **Deploy to staging** - Before production

---

## Questions to Resolve

1. **Machine Assignment Logic**
   - Where is the actual machine-to-operator assignment handled?
   - Is it in a repository or service layer?

2. **User Role Assignments**
   - How to find "available" Mechanical Engineer?
   - Round-robin? First available? Specific assignment?

3. **Notification Cleanup**
   - Should old notifications be auto-deleted?
   - Recommended: Delete notifications older than 90 days

4. **Priority Rules**
   - Should certain request types always be Urgent/Critical?
   - Current: Based on request priority field

---

## Conclusion

This plan provides a clear, actionable roadmap to complete the automatic notification system. The frontend is ready and waiting. With 18-20 hours of focused backend development, you'll have a fully automatic, end-to-end notification system that keeps all users informed of important workflow events.

**Current Status**: 🟡 20% Complete (1 of 5 workflows)
**After Implementation**: 🟢 100% Complete (5 of 5 workflows)

Ready to start? Let me know which phase you'd like to tackle first!

---

**Document Version**: 1.0
**Created**: November 14, 2025
**Status**: Ready for Implementation
**Estimated Completion**: 2-3 days
