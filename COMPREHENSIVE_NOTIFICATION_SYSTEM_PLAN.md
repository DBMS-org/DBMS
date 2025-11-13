# 🔔 COMPREHENSIVE NOTIFICATION SYSTEM IMPLEMENTATION PLAN

## 📋 Table of Contents
1. [System Overview](#system-overview)
2. [Role Analysis](#role-analysis)
3. [Notification Triggers & Types](#notification-triggers--types)
4. [Architecture Design](#architecture-design)
5. [Backend Implementation](#backend-implementation)
6. [Frontend Implementation](#frontend-implementation)
7. [Implementation Phases](#implementation-phases)
8. [Testing Strategy](#testing-strategy)

---

## 🎯 System Overview

### Current State
- **Frontend**: Local notification system using localStorage (no backend persistence)
- **Backend**: No notification infrastructure exists
- **Gap**: When workflows complete (approvals, assignments, etc.), users are not notified

### Target State
- **Full-stack notification system** with database persistence
- **Real-time updates** via API polling (SignalR optional for future)
- **Role-based notifications** for all user types
- **Read/unread tracking** and notification history
- **Comprehensive coverage** of all workflow state changes

---

## 👥 Role Analysis

### Identified Roles (7 Total)

#### 1. **Admin**
- **Responsibilities**: User management, system configuration, project creation, region management
- **Main Components**: Dashboard, users, machine inventory, stores, project management
- **Authorization Policies**: `RequireAdminRole`

#### 2. **Blasting Engineer**
- **Responsibilities**: Create explosive approval requests, drill planning, blast calculations, site management
- **Main Components**: Project management, site dashboard, drill visualization, explosive calculations, blast simulator
- **Authorization Policies**: `ManageProjectSites`, `ManageDrillData`, `ReadDrillData`

#### 3. **Store Manager**
- **Responsibilities**: Approve/reject explosive approval requests, manage store inventory, request explosives from central warehouse
- **Main Components**: Blasting engineer requests, request history, inventory management
- **Authorization Policies**: `ManageExplosiveRequests`

#### 4. **Explosive Manager**
- **Responsibilities**: Manage central warehouse inventory, approve/reject transfer requests, dispatch explosives to stores
- **Main Components**: Inventory management, transfer requests, stores, dispatch tracking
- **Authorization Policies**: `ManageInventory`, `ApproveTransfers`, `ReadInventoryData`

#### 5. **Machine Manager**
- **Responsibilities**: Manage machine inventory, approve/reject machine assignment requests, assign machines to operators
- **Main Components**: Machine inventory, assignment requests, assignment history
- **Authorization Policies**: `ManageMachines`

#### 6. **Mechanical Engineer**
- **Responsibilities**: Handle maintenance jobs, respond to maintenance reports, perform maintenance work
- **Main Components**: Maintenance dashboard, maintenance jobs, maintenance reports, maintenance analytics
- **Authorization Policies**: None specific (implied from role)

#### 7. **Operator**
- **Responsibilities**: Submit maintenance reports, use assigned machines, work on project sites
- **Main Components**: Dashboard, my machines, maintenance reports, project sites
- **Authorization Policies**: None specific (implied from role)

---

## 🔔 Notification Triggers & Types

### 1. **Explosive Approval Request Workflow**

#### Entity: `ExplosiveApprovalRequest`
#### Status Flow: `Pending → Approved/Rejected/Cancelled/Expired`

| Trigger Event | Notified Role | Notification Type | Priority | Content |
|--------------|---------------|-------------------|----------|---------|
| **Request Created** | Store Manager (in same region) | `explosive_request_created` | Normal/High/Critical | "New explosive approval request #{id} for {siteName}" |
| **Request Approved** | Blasting Engineer (creator) | `explosive_request_approved` | High | "Your explosive approval request #{id} has been approved by {storeManagerName}" |
| **Request Rejected** | Blasting Engineer (creator) | `explosive_request_rejected` | High | "Your explosive approval request #{id} has been rejected. Reason: {reason}" |
| **Request Cancelled** | Store Manager (who was reviewing) | `explosive_request_cancelled` | Normal | "Explosive approval request #{id} has been cancelled by {engineerName}" |
| **Request Expired** | Blasting Engineer (creator) | `explosive_request_expired` | Medium | "Your explosive approval request #{id} has expired" |
| **Blasting Date Updated** | Store Manager (in same region) | `explosive_request_updated` | Normal | "Blasting date/timing updated for request #{id}" |

---

### 2. **Inventory Transfer Request Workflow**

#### Entity: `InventoryTransferRequest`
#### Status Flow: `Pending → Approved/Rejected → InProgress → Completed/Cancelled`

| Trigger Event | Notified Role | Notification Type | Priority | Content |
|--------------|---------------|-------------------|----------|---------|
| **Transfer Request Created** | Explosive Manager | `transfer_request_created` | Normal/High | "New transfer request #{requestNumber} from {storeName}" |
| **Transfer Request Urgent** | Explosive Manager | `transfer_request_urgent` | Urgent | "Urgent: Transfer request #{requestNumber} required within 7 days" |
| **Transfer Request Approved** | Store Manager (requester) | `transfer_request_approved` | High | "Transfer request #{requestNumber} approved. Quantity: {quantity}" |
| **Transfer Request Rejected** | Store Manager (requester) | `transfer_request_rejected` | High | "Transfer request #{requestNumber} rejected. Reason: {reason}" |
| **Transfer Dispatched** | Store Manager (destination) | `transfer_dispatched` | High | "Transfer #{requestNumber} dispatched. Truck: {truckNumber}, Driver: {driverName}" |
| **Transfer Completed** | Store Manager (destination) | `transfer_completed` | Normal | "Transfer #{requestNumber} completed successfully" |
| **Transfer Cancelled** | Store Manager (requester) | `transfer_cancelled` | Normal | "Transfer request #{requestNumber} cancelled" |

---

### 3. **Machine Assignment Request Workflow**

#### Entity: `MachineAssignmentRequest`
#### Status Flow: `Pending → Approved/Rejected/PartiallyFulfilled → Completed/Cancelled`

| Trigger Event | Notified Role | Notification Type | Priority | Content |
|--------------|---------------|-------------------|----------|---------|
| **Assignment Request Created** | Machine Manager | `machine_request_created` | Normal/High/Urgent | "New machine assignment request: {quantity}x {machineType} for {projectName}" |
| **Assignment Request Approved** | Requester (likely Admin/PM) | `machine_request_approved` | High | "Machine assignment request approved. {quantity} machines assigned" |
| **Assignment Request Rejected** | Requester | `machine_request_rejected` | High | "Machine assignment request rejected. Reason: {reason}" |
| **Assignment Partially Fulfilled** | Requester | `machine_request_partial` | Medium | "Machine assignment partially fulfilled: {assigned}/{requested} machines" |
| **Assignment Request Completed** | Machine Manager | `machine_request_completed` | Normal | "Machine assignment request #{id} completed" |

---

### 4. **Machine Assignment Workflow**

#### Entity: `MachineAssignment`
#### Status Flow: `Active → Completed/Overdue/Cancelled`

| Trigger Event | Notified Role | Notification Type | Priority | Content |
|--------------|---------------|-------------------|----------|---------|
| **Machine Assigned** | Operator (assigned) | `machine_assigned` | High | "Machine {machineName} ({serialNumber}) assigned to you for {projectName}" |
| **Machine Assignment Overdue** | Operator + Machine Manager | `machine_assignment_overdue` | High | "Machine {machineName} return overdue. Expected: {expectedDate}" |
| **Machine Returned** | Machine Manager | `machine_returned` | Normal | "{operatorName} returned machine {machineName} from {projectName}" |
| **Assignment Cancelled** | Operator (if assigned) | `machine_assignment_cancelled` | Normal | "Your machine assignment for {machineName} has been cancelled" |

---

### 5. **Maintenance Report Workflow**

#### Entity: `MaintenanceReport`
#### Status Flow: `Reported → Acknowledged → InProgress → Resolved → Closed`

| Trigger Event | Notified Role | Notification Type | Priority | Content |
|--------------|---------------|-------------------|----------|---------|
| **Report Submitted** | Mechanical Engineer (available) | `maintenance_report_created` | High/Urgent | "New maintenance report #{ticketId}: {machineName} - {problemCategory} ({severity})" |
| **Report Acknowledged** | Operator (reporter) | `maintenance_report_acknowledged` | Normal | "Your maintenance report #{ticketId} acknowledged by {engineerName}. ETA: {eta}" |
| **Report In Progress** | Operator (reporter) | `maintenance_report_in_progress` | Normal | "Maintenance work started on your report #{ticketId}" |
| **Report Resolved** | Operator (reporter) | `maintenance_report_resolved` | High | "Maintenance report #{ticketId} resolved. Please verify the fix" |
| **Report Closed** | Mechanical Engineer (assigned) | `maintenance_report_closed` | Normal | "Maintenance report #{ticketId} closed by operator" |
| **Report Reopened** | Mechanical Engineer (assigned) | `maintenance_report_reopened` | High | "Maintenance report #{ticketId} reopened by operator. Reason: {reason}" |

---

### 6. **Maintenance Job Workflow**

#### Entity: `MaintenanceJob`
#### Status Flow: `Scheduled → InProgress → Completed/Cancelled/Overdue`

| Trigger Event | Notified Role | Notification Type | Priority | Content |
|--------------|---------------|-------------------|----------|---------|
| **Job Created** | Mechanical Engineer (assigned) | `maintenance_job_created` | Normal/High | "New maintenance job assigned: {machineName} - {type} ({scheduledDate})" |
| **Job Due Soon** | Mechanical Engineer (assigned) | `maintenance_job_due_soon` | High | "Maintenance job #{id} due in 24 hours" |
| **Job Overdue** | Mechanical Engineer (assigned) | `maintenance_job_overdue` | Urgent | "Maintenance job #{id} is overdue" |
| **Job Started** | Creator (if different) | `maintenance_job_started` | Normal | "Maintenance job #{id} started by {engineerName}" |
| **Job Completed** | Creator + Machine Manager | `maintenance_job_completed` | Normal | "Maintenance job #{id} completed. Duration: {hours}h" |
| **Job Cancelled** | Mechanical Engineer (assigned) | `maintenance_job_cancelled` | Normal | "Maintenance job #{id} cancelled. Reason: {reason}" |

---

### 7. **System & Administrative Notifications**

| Trigger Event | Notified Role | Notification Type | Priority | Content |
|--------------|---------------|-------------------|----------|---------|
| **User Created** | New User | `user_account_created` | Normal | "Welcome! Your account has been created. Role: {role}" |
| **User Role Changed** | User | `user_role_changed` | High | "Your role has been changed from {oldRole} to {newRole}" |
| **User Assigned to Region** | User | `user_region_assigned` | Normal | "You have been assigned to {regionName} region" |
| **Project Created** | Blasting Engineers (in region) | `project_created` | Normal | "New project created: {projectName} in {regionName}" |
| **Project Site Created** | Blasting Engineer (assigned) | `project_site_created` | Normal | "New project site created: {siteName} in {projectName}" |

---

## 🏗️ Architecture Design

### Database Schema

#### **Notification Entity**

```csharp
public class Notification : BaseAuditableEntity
{
    public int Id { get; set; }

    // User who receives this notification
    public int UserId { get; set; }

    // Type of notification (for filtering/grouping)
    public NotificationType Type { get; set; }

    // Title and message
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    // Priority level
    public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;

    // Read/unread tracking
    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }

    // Related entity information (optional)
    public string? RelatedEntityType { get; set; }  // e.g., "ExplosiveApprovalRequest"
    public int? RelatedEntityId { get; set; }       // e.g., 123

    // Action URL for navigation (optional)
    public string? ActionUrl { get; set; }  // e.g., "/store-manager/requests/123"

    // Additional metadata (JSON)
    public string? AdditionalData { get; set; }

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual User User { get; set; } = null!;
}
```

#### **Notification Enums**

```csharp
public enum NotificationType
{
    // Explosive Approval Requests
    ExplosiveRequestCreated,
    ExplosiveRequestApproved,
    ExplosiveRequestRejected,
    ExplosiveRequestCancelled,
    ExplosiveRequestExpired,
    ExplosiveRequestUpdated,

    // Inventory Transfer Requests
    TransferRequestCreated,
    TransferRequestUrgent,
    TransferRequestApproved,
    TransferRequestRejected,
    TransferDispatched,
    TransferCompleted,
    TransferCancelled,

    // Machine Assignment Requests
    MachineRequestCreated,
    MachineRequestApproved,
    MachineRequestRejected,
    MachineRequestPartial,
    MachineRequestCompleted,

    // Machine Assignments
    MachineAssigned,
    MachineAssignmentOverdue,
    MachineReturned,
    MachineAssignmentCancelled,

    // Maintenance Reports
    MaintenanceReportCreated,
    MaintenanceReportAcknowledged,
    MaintenanceReportInProgress,
    MaintenanceReportResolved,
    MaintenanceReportClosed,
    MaintenanceReportReopened,

    // Maintenance Jobs
    MaintenanceJobCreated,
    MaintenanceJobDueSoon,
    MaintenanceJobOverdue,
    MaintenanceJobStarted,
    MaintenanceJobCompleted,
    MaintenanceJobCancelled,

    // System/Admin
    UserAccountCreated,
    UserRoleChanged,
    UserRegionAssigned,
    ProjectCreated,
    ProjectSiteCreated,

    // Generic
    System,
    Info,
    Warning,
    Error
}

public enum NotificationPriority
{
    Low = 0,
    Normal = 1,
    High = 2,
    Urgent = 3,
    Critical = 4
}
```

---

## 🔨 Backend Implementation

### Phase 1: Core Infrastructure

#### 1.1 Create Domain Entity
**File**: `Domain/Entities/Notifications/Notification.cs`

```csharp
using Domain.Common;
using Domain.Entities.UserManagement;

namespace Domain.Entities.Notifications
{
    public class Notification : BaseAuditableEntity
    {
        public int UserId { get; private set; }
        public NotificationType Type { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Message { get; private set; } = string.Empty;
        public NotificationPriority Priority { get; private set; } = NotificationPriority.Normal;
        public bool IsRead { get; private set; } = false;
        public DateTime? ReadAt { get; private set; }
        public string? RelatedEntityType { get; private set; }
        public int? RelatedEntityId { get; private set; }
        public string? ActionUrl { get; private set; }
        public string? AdditionalData { get; private set; }

        // Navigation property
        public virtual User User { get; private set; } = null!;

        private Notification() { }

        public static Notification Create(
            int userId,
            NotificationType type,
            string title,
            string message,
            NotificationPriority priority = NotificationPriority.Normal,
            string? relatedEntityType = null,
            int? relatedEntityId = null,
            string? actionUrl = null,
            string? additionalData = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required", nameof(title));

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message is required", nameof(message));

            return new Notification
            {
                UserId = userId,
                Type = type,
                Title = title,
                Message = message,
                Priority = priority,
                RelatedEntityType = relatedEntityType,
                RelatedEntityId = relatedEntityId,
                ActionUrl = actionUrl,
                AdditionalData = additionalData
            };
        }

        public void MarkAsRead()
        {
            if (!IsRead)
            {
                IsRead = true;
                ReadAt = DateTime.UtcNow;
                MarkUpdated();
            }
        }

        public void MarkAsUnread()
        {
            if (IsRead)
            {
                IsRead = false;
                ReadAt = null;
                MarkUpdated();
            }
        }
    }
}
```

#### 1.2 Create Enums
**File**: `Domain/Entities/Notifications/NotificationType.cs`

```csharp
namespace Domain.Entities.Notifications
{
    public enum NotificationType
    {
        // Explosive Approval Requests
        ExplosiveRequestCreated = 100,
        ExplosiveRequestApproved = 101,
        ExplosiveRequestRejected = 102,
        ExplosiveRequestCancelled = 103,
        ExplosiveRequestExpired = 104,
        ExplosiveRequestUpdated = 105,

        // Inventory Transfer Requests
        TransferRequestCreated = 200,
        TransferRequestUrgent = 201,
        TransferRequestApproved = 202,
        TransferRequestRejected = 203,
        TransferDispatched = 204,
        TransferCompleted = 205,
        TransferCancelled = 206,

        // Machine Assignment Requests
        MachineRequestCreated = 300,
        MachineRequestApproved = 301,
        MachineRequestRejected = 302,
        MachineRequestPartial = 303,
        MachineRequestCompleted = 304,

        // Machine Assignments
        MachineAssigned = 400,
        MachineAssignmentOverdue = 401,
        MachineReturned = 402,
        MachineAssignmentCancelled = 403,

        // Maintenance Reports
        MaintenanceReportCreated = 500,
        MaintenanceReportAcknowledged = 501,
        MaintenanceReportInProgress = 502,
        MaintenanceReportResolved = 503,
        MaintenanceReportClosed = 504,
        MaintenanceReportReopened = 505,

        // Maintenance Jobs
        MaintenanceJobCreated = 600,
        MaintenanceJobDueSoon = 601,
        MaintenanceJobOverdue = 602,
        MaintenanceJobStarted = 603,
        MaintenanceJobCompleted = 604,
        MaintenanceJobCancelled = 605,

        // System/Admin
        UserAccountCreated = 700,
        UserRoleChanged = 701,
        UserRegionAssigned = 702,
        ProjectCreated = 703,
        ProjectSiteCreated = 704,

        // Generic
        System = 1000,
        Info = 1001,
        Warning = 1002,
        Error = 1003
    }
}
```

**File**: `Domain/Entities/Notifications/NotificationPriority.cs`

```csharp
namespace Domain.Entities.Notifications
{
    public enum NotificationPriority
    {
        Low = 0,
        Normal = 1,
        High = 2,
        Urgent = 3,
        Critical = 4
    }
}
```

#### 1.3 Update DbContext
**File**: `Infrastructure/Data/ApplicationDbContext.cs`

```csharp
// Add this DbSet
public DbSet<Notification> Notifications { get; set; }

// In OnModelCreating, add:
modelBuilder.Entity<Notification>(entity =>
{
    entity.HasKey(n => n.Id);

    entity.HasOne(n => n.User)
        .WithMany()
        .HasForeignKey(n => n.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    entity.HasIndex(n => new { n.UserId, n.IsRead });
    entity.HasIndex(n => n.CreatedAt);
    entity.HasIndex(n => n.Type);

    entity.Property(n => n.Title)
        .IsRequired()
        .HasMaxLength(200);

    entity.Property(n => n.Message)
        .IsRequired()
        .HasMaxLength(1000);

    entity.Property(n => n.RelatedEntityType)
        .HasMaxLength(100);
});
```

#### 1.4 Create Migration

```bash
dotnet ef migrations add AddNotificationSystem
dotnet ef database update
```

---

### Phase 2: Repository Layer

#### 2.1 Repository Interface
**File**: `Application/Interfaces/INotificationRepository.cs`

```csharp
using Domain.Entities.Notifications;

namespace Application.Interfaces
{
    public interface INotificationRepository
    {
        // Create
        Task<Notification> CreateAsync(Notification notification);
        Task<IEnumerable<Notification>> CreateBulkAsync(IEnumerable<Notification> notifications);

        // Read
        Task<Notification?> GetByIdAsync(int id);
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, int skip = 0, int take = 50);
        Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task<IEnumerable<Notification>> GetByTypeAsync(int userId, NotificationType type);

        // Update
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> MarkAsUnreadAsync(int notificationId);
        Task<bool> MarkAllAsReadAsync(int userId);

        // Delete
        Task<bool> DeleteAsync(int notificationId);
        Task<bool> DeleteOldNotificationsAsync(int userId, int daysOld = 30);
        Task<bool> DeleteAllAsync(int userId);
    }
}
```

#### 2.2 Repository Implementation
**File**: `Infrastructure/Repositories/NotificationRepository.cs`

```csharp
using Application.Interfaces;
using Domain.Entities.Notifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> CreateAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<IEnumerable<Notification>> CreateBulkAsync(IEnumerable<Notification> notifications)
        {
            _context.Notifications.AddRange(notifications);
            await _context.SaveChangesAsync();
            return notifications;
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, int skip = 0, int take = 50)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task<IEnumerable<Notification>> GetByTypeAsync(int userId, NotificationType type)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && n.Type == type)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return false;

            notification.MarkAsRead();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsUnreadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return false;

            notification.MarkAsUnread();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var unreadNotifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.MarkAsRead();
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOldNotificationsAsync(int userId, int daysOld = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysOld);
            var oldNotifications = await _context.Notifications
                .Where(n => n.UserId == userId && n.CreatedAt < cutoffDate)
                .ToListAsync();

            _context.Notifications.RemoveRange(oldNotifications);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();

            _context.Notifications.RemoveRange(notifications);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
```

---

### Phase 3: Service Layer

#### 3.1 Notification Service Interface
**File**: `Application/Interfaces/INotificationService.cs`

```csharp
using Domain.Entities.Notifications;

namespace Application.Interfaces
{
    public interface INotificationService
    {
        // Core notification creation
        Task<Notification> CreateNotificationAsync(
            int userId,
            NotificationType type,
            string title,
            string message,
            NotificationPriority priority = NotificationPriority.Normal,
            string? relatedEntityType = null,
            int? relatedEntityId = null,
            string? actionUrl = null,
            Dictionary<string, object>? additionalData = null);

        Task CreateNotificationsForUsersAsync(
            IEnumerable<int> userIds,
            NotificationType type,
            string title,
            string message,
            NotificationPriority priority = NotificationPriority.Normal,
            string? relatedEntityType = null,
            int? relatedEntityId = null,
            string? actionUrl = null);

        // Explosive Approval Request notifications
        Task NotifyExplosiveRequestCreatedAsync(int requestId, int requestedByUserId, int projectSiteId);
        Task NotifyExplosiveRequestApprovedAsync(int requestId, int approvedByUserId);
        Task NotifyExplosiveRequestRejectedAsync(int requestId, int rejectedByUserId, string reason);
        Task NotifyExplosiveRequestCancelledAsync(int requestId, int cancelledByUserId);
        Task NotifyExplosiveRequestExpiredAsync(int requestId);

        // Transfer Request notifications
        Task NotifyTransferRequestCreatedAsync(int requestId);
        Task NotifyTransferRequestApprovedAsync(int requestId);
        Task NotifyTransferRequestRejectedAsync(int requestId);
        Task NotifyTransferDispatchedAsync(int requestId);
        Task NotifyTransferCompletedAsync(int requestId);

        // Machine Assignment notifications
        Task NotifyMachineRequestCreatedAsync(int requestId);
        Task NotifyMachineRequestApprovedAsync(int requestId);
        Task NotifyMachineRequestRejectedAsync(int requestId);
        Task NotifyMachineAssignedAsync(int assignmentId);

        // Maintenance notifications
        Task NotifyMaintenanceReportCreatedAsync(int reportId);
        Task NotifyMaintenanceReportAcknowledgedAsync(int reportId);
        Task NotifyMaintenanceReportResolvedAsync(int reportId);
        Task NotifyMaintenanceJobCreatedAsync(int jobId);
        Task NotifyMaintenanceJobCompletedAsync(int jobId);
    }
}
```

#### 3.2 Notification Service Implementation
**File**: `Application/Services/NotificationService.cs`

```csharp
using System.Text.Json;
using Application.Interfaces;
using Domain.Entities.Notifications;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IExplosiveApprovalRequestRepository _explosiveRequestRepo;
        private readonly IInventoryTransferRepository _transferRepo;
        private readonly IMachineAssignmentRequestRepository _machineRequestRepo;
        private readonly IMachineAssignmentRepository _machineAssignmentRepo;
        private readonly IMaintenanceReportRepository _maintenanceReportRepo;
        private readonly IMaintenanceJobRepository _maintenanceJobRepo;
        private readonly IUserRepository _userRepository;

        public NotificationService(
            INotificationRepository notificationRepository,
            IExplosiveApprovalRequestRepository explosiveRequestRepo,
            IInventoryTransferRepository transferRepo,
            IMachineAssignmentRequestRepository machineRequestRepo,
            IMachineAssignmentRepository machineAssignmentRepo,
            IMaintenanceReportRepository maintenanceReportRepo,
            IMaintenanceJobRepository maintenanceJobRepo,
            IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _explosiveRequestRepo = explosiveRequestRepo;
            _transferRepo = transferRepo;
            _machineRequestRepo = machineRequestRepo;
            _machineAssignmentRepo = machineAssignmentRepo;
            _maintenanceReportRepo = maintenanceReportRepo;
            _maintenanceJobRepo = maintenanceJobRepo;
            _userRepository = userRepository;
        }

        public async Task<Notification> CreateNotificationAsync(
            int userId,
            NotificationType type,
            string title,
            string message,
            NotificationPriority priority = NotificationPriority.Normal,
            string? relatedEntityType = null,
            int? relatedEntityId = null,
            string? actionUrl = null,
            Dictionary<string, object>? additionalData = null)
        {
            var notification = Notification.Create(
                userId,
                type,
                title,
                message,
                priority,
                relatedEntityType,
                relatedEntityId,
                actionUrl,
                additionalData != null ? JsonSerializer.Serialize(additionalData) : null
            );

            return await _notificationRepository.CreateAsync(notification);
        }

        public async Task CreateNotificationsForUsersAsync(
            IEnumerable<int> userIds,
            NotificationType type,
            string title,
            string message,
            NotificationPriority priority = NotificationPriority.Normal,
            string? relatedEntityType = null,
            int? relatedEntityId = null,
            string? actionUrl = null)
        {
            var notifications = userIds.Select(userId =>
                Notification.Create(
                    userId,
                    type,
                    title,
                    message,
                    priority,
                    relatedEntityType,
                    relatedEntityId,
                    actionUrl
                )
            ).ToList();

            await _notificationRepository.CreateBulkAsync(notifications);
        }

        // ===== EXPLOSIVE APPROVAL REQUEST NOTIFICATIONS =====

        public async Task NotifyExplosiveRequestCreatedAsync(int requestId, int requestedByUserId, int projectSiteId)
        {
            var request = await _explosiveRequestRepo.GetByIdAsync(requestId);
            if (request == null) return;

            // Get site and project details for context
            var site = request.ProjectSite;
            var project = site?.Project;
            var region = project?.Region;
            var requester = request.RequestedByUser;

            // Get all Store Managers in the same region
            var storeManagers = await _userRepository.GetUsersByRoleAndRegionAsync("StoreManager", region?.Name);

            var title = $"New Explosive Approval Request #{requestId}";
            var message = $"{requester.Name} requested explosive approval for {site?.Name} ({project?.Name}). Expected usage: {request.ExpectedUsageDate:MMM dd, yyyy}";
            var priority = request.Priority switch
            {
                Domain.Entities.ProjectManagement.RequestPriority.Critical => NotificationPriority.Critical,
                Domain.Entities.ProjectManagement.RequestPriority.High => NotificationPriority.High,
                _ => NotificationPriority.Normal
            };

            await CreateNotificationsForUsersAsync(
                storeManagers.Select(sm => sm.Id),
                NotificationType.ExplosiveRequestCreated,
                title,
                message,
                priority,
                "ExplosiveApprovalRequest",
                requestId,
                $"/store-manager/requests/{requestId}"
            );
        }

        public async Task NotifyExplosiveRequestApprovedAsync(int requestId, int approvedByUserId)
        {
            var request = await _explosiveRequestRepo.GetByIdAsync(requestId);
            if (request == null) return;

            var approver = await _userRepository.GetByIdAsync(approvedByUserId);
            var site = request.ProjectSite;

            var title = "Explosive Request Approved";
            var message = $"Your explosive approval request for {site?.Name} has been approved by {approver?.Name}. Blasting scheduled for {request.BlastingDate:MMM dd, yyyy} at {request.BlastTiming}.";

            await CreateNotificationAsync(
                request.RequestedByUserId,
                NotificationType.ExplosiveRequestApproved,
                title,
                message,
                NotificationPriority.High,
                "ExplosiveApprovalRequest",
                requestId,
                $"/blasting-engineer/site-dashboard/{site?.Id}"
            );
        }

        public async Task NotifyExplosiveRequestRejectedAsync(int requestId, int rejectedByUserId, string reason)
        {
            var request = await _explosiveRequestRepo.GetByIdAsync(requestId);
            if (request == null) return;

            var rejector = await _userRepository.GetByIdAsync(rejectedByUserId);
            var site = request.ProjectSite;

            var title = "Explosive Request Rejected";
            var message = $"Your explosive approval request for {site?.Name} was rejected by {rejector?.Name}. Reason: {reason}";

            await CreateNotificationAsync(
                request.RequestedByUserId,
                NotificationType.ExplosiveRequestRejected,
                title,
                message,
                NotificationPriority.High,
                "ExplosiveApprovalRequest",
                requestId,
                $"/blasting-engineer/site-dashboard/{site?.Id}"
            );
        }

        public async Task NotifyExplosiveRequestCancelledAsync(int requestId, int cancelledByUserId)
        {
            var request = await _explosiveRequestRepo.GetByIdAsync(requestId);
            if (request == null) return;

            var canceller = await _userRepository.GetByIdAsync(cancelledByUserId);
            var site = request.ProjectSite;
            var project = site?.Project;
            var region = project?.Region;

            // Notify Store Managers in the region
            var storeManagers = await _userRepository.GetUsersByRoleAndRegionAsync("StoreManager", region?.Name);

            var title = "Explosive Request Cancelled";
            var message = $"Explosive approval request #{requestId} for {site?.Name} has been cancelled by {canceller?.Name}.";

            await CreateNotificationsForUsersAsync(
                storeManagers.Select(sm => sm.Id),
                NotificationType.ExplosiveRequestCancelled,
                title,
                message,
                NotificationPriority.Normal,
                "ExplosiveApprovalRequest",
                requestId,
                $"/store-manager/requests"
            );
        }

        public async Task NotifyExplosiveRequestExpiredAsync(int requestId)
        {
            var request = await _explosiveRequestRepo.GetByIdAsync(requestId);
            if (request == null) return;

            var site = request.ProjectSite;

            var title = "Explosive Request Expired";
            var message = $"Your explosive approval request for {site?.Name} has expired.";

            await CreateNotificationAsync(
                request.RequestedByUserId,
                NotificationType.ExplosiveRequestExpired,
                title,
                message,
                NotificationPriority.Normal,
                "ExplosiveApprovalRequest",
                requestId,
                $"/blasting-engineer/site-dashboard/{site?.Id}"
            );
        }

        // ===== TRANSFER REQUEST NOTIFICATIONS =====
        // (Similar implementations for other notification types)

        // ... Additional methods following the same pattern ...
    }
}
```

---

### Phase 4: Controller Layer

#### 4.1 Notifications Controller
**File**: `Presentation/API/Controllers/NotificationsController.cs`

```csharp
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationsController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("id");
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var userId = GetCurrentUserId();
            var notifications = await _notificationRepository.GetByUserIdAsync(userId, skip, take);
            return Ok(notifications);
        }

        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            var userId = GetCurrentUserId();
            var notifications = await _notificationRepository.GetUnreadByUserIdAsync(userId);
            return Ok(notifications);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = GetCurrentUserId();
            var count = await _notificationRepository.GetUnreadCountAsync(userId);
            return Ok(new { count });
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var success = await _notificationRepository.MarkAsReadAsync(id);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpPut("{id}/unread")]
        public async Task<IActionResult> MarkAsUnread(int id)
        {
            var success = await _notificationRepository.MarkAsUnreadAsync(id);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpPut("mark-all-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = GetCurrentUserId();
            await _notificationRepository.MarkAllAsReadAsync(userId);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var success = await _notificationRepository.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpDelete("delete-all")]
        public async Task<IActionResult> DeleteAllNotifications()
        {
            var userId = GetCurrentUserId();
            await _notificationRepository.DeleteAllAsync(userId);
            return Ok();
        }
    }
}
```

---

### Phase 5: Integration with Existing Workflows

#### 5.1 Explosive Approval Request Integration
**File**: `Infrastructure/Repositories/ProjectManagement/ExplosiveApprovalRequestRepository.cs`

**Add at line 165 (after approval):**
```csharp
// After: await _context.SaveChangesAsync();

// Trigger notification
await _notificationService.NotifyExplosiveRequestApprovedAsync(requestId, approvedByUserId);
```

**Similar integrations needed in:**
- `RejectRequestAsync()` - Line 167-191
- `CancelRequestAsync()` - (if exists)
- `CreateExplosiveApprovalRequest()` in service layer

#### 5.2 Transfer Request Integration
**File**: `Infrastructure/Repositories/ExplosiveInventory/InventoryTransferRequestRepository.cs`

Add notification calls after:
- Approve (line 99-122)
- Reject (line 127-142)
- Dispatch (line 161-186)
- Complete (line 207-218)

#### 5.3 Machine Assignment Integration
Similar patterns for:
- `MachineAssignmentRequestRepository.cs`
- `MachineAssignmentRepository.cs`

#### 5.4 Maintenance Integration
- `MaintenanceReportRepository.cs`
- `MaintenanceJobRepository.cs`

---

## 🎨 Frontend Implementation

### Phase 1: TypeScript Models

#### 1.1 Notification Model
**File**: `Presentation/UI/src/app/core/models/notification.model.ts`

```typescript
export enum NotificationType {
  // Explosive Approval Requests
  ExplosiveRequestCreated = 100,
  ExplosiveRequestApproved = 101,
  ExplosiveRequestRejected = 102,
  ExplosiveRequestCancelled = 103,
  ExplosiveRequestExpired = 104,
  ExplosiveRequestUpdated = 105,

  // ... (all other types)
}

export enum NotificationPriority {
  Low = 0,
  Normal = 1,
  High = 2,
  Urgent = 3,
  Critical = 4
}

export interface Notification {
  id: number;
  userId: number;
  type: NotificationType;
  title: string;
  message: string;
  priority: NotificationPriority;
  isRead: boolean;
  readAt?: Date;
  relatedEntityType?: string;
  relatedEntityId?: number;
  actionUrl?: string;
  additionalData?: string;
  createdAt: Date;
  updatedAt: Date;
}
```

---

### Phase 2: Notification Service

#### 2.1 Update Existing Service
**File**: `Presentation/UI/src/app/core/services/notification.service.ts`

**Replace entire file with backend-connected version:**

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, interval } from 'rxjs';
import { map, tap, switchMap } from 'rxjs/operators';
import { Notification } from '../models/notification.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiUrl = `${environment.apiUrl}/notifications`;
  private notificationsSubject = new BehaviorSubject<Notification[]>([]);
  private unreadCountSubject = new BehaviorSubject<number>(0);

  public notifications$ = this.notificationsSubject.asObservable();
  public unreadCount$ = this.unreadCountSubject.asObservable();

  constructor(private http: HttpClient) {
    // Poll for new notifications every 30 seconds
    this.startPolling();
  }

  private startPolling(): void {
    interval(30000) // 30 seconds
      .pipe(switchMap(() => this.fetchNotifications()))
      .subscribe();
  }

  // Fetch all notifications
  fetchNotifications(skip: number = 0, take: number = 50): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.apiUrl}?skip=${skip}&take=${take}`)
      .pipe(
        tap(notifications => {
          this.notificationsSubject.next(notifications);
          this.updateUnreadCount();
        })
      );
  }

  // Fetch unread notifications
  fetchUnreadNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.apiUrl}/unread`);
  }

  // Get unread count
  fetchUnreadCount(): Observable<number> {
    return this.http.get<{ count: number }>(`${this.apiUrl}/unread-count`)
      .pipe(
        map(response => response.count),
        tap(count => this.unreadCountSubject.next(count))
      );
  }

  // Mark as read
  markAsRead(notificationId: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${notificationId}/read`, {})
      .pipe(
        tap(() => {
          this.updateNotificationReadStatus(notificationId, true);
          this.updateUnreadCount();
        })
      );
  }

  // Mark as unread
  markAsUnread(notificationId: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${notificationId}/unread`, {})
      .pipe(
        tap(() => {
          this.updateNotificationReadStatus(notificationId, false);
          this.updateUnreadCount();
        })
      );
  }

  // Mark all as read
  markAllAsRead(): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/mark-all-read`, {})
      .pipe(
        tap(() => {
          const notifications = this.notificationsSubject.value;
          notifications.forEach(n => n.isRead = true);
          this.notificationsSubject.next([...notifications]);
          this.unreadCountSubject.next(0);
        })
      );
  }

  // Delete notification
  deleteNotification(notificationId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${notificationId}`)
      .pipe(
        tap(() => {
          const notifications = this.notificationsSubject.value.filter(n => n.id !== notificationId);
          this.notificationsSubject.next(notifications);
          this.updateUnreadCount();
        })
      );
  }

  // Delete all notifications
  deleteAllNotifications(): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/delete-all`)
      .pipe(
        tap(() => {
          this.notificationsSubject.next([]);
          this.unreadCountSubject.next(0);
        })
      );
  }

  // Helper methods
  private updateNotificationReadStatus(notificationId: number, isRead: boolean): void {
    const notifications = this.notificationsSubject.value;
    const notification = notifications.find(n => n.id === notificationId);
    if (notification) {
      notification.isRead = isRead;
      notification.readAt = isRead ? new Date() : undefined;
      this.notificationsSubject.next([...notifications]);
    }
  }

  private updateUnreadCount(): void {
    const count = this.notificationsSubject.value.filter(n => !n.isRead).length;
    this.unreadCountSubject.next(count);
  }
}
```

---

### Phase 3: Update Notification Components

#### 3.1 Update Shared Notification Panel
**File**: `Presentation/UI/src/app/shared/components/notification-panel/notification-panel.component.ts`

**Update to use backend service:**

```typescript
import { Component, OnInit, OnDestroy } from '@angular/core';
import { NotificationService } from '../../../core/services/notification.service';
import { Notification } from '../../../core/models/notification.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-notification-panel',
  templateUrl: './notification-panel.component.html',
  styleUrls: ['./notification-panel.component.scss']
})
export class NotificationPanelComponent implements OnInit, OnDestroy {
  notifications: Notification[] = [];
  unreadCount: number = 0;
  loading: boolean = false;

  private subscriptions = new Subscription();

  constructor(private notificationService: NotificationService) {}

  ngOnInit(): void {
    this.loadNotifications();

    // Subscribe to notifications observable
    this.subscriptions.add(
      this.notificationService.notifications$.subscribe(
        notifications => this.notifications = notifications
      )
    );

    // Subscribe to unread count
    this.subscriptions.add(
      this.notificationService.unreadCount$.subscribe(
        count => this.unreadCount = count
      )
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  loadNotifications(): void {
    this.loading = true;
    this.notificationService.fetchNotifications().subscribe({
      next: () => this.loading = false,
      error: (err) => {
        console.error('Error loading notifications:', err);
        this.loading = false;
      }
    });
  }

  markAsRead(notification: Notification): void {
    if (!notification.isRead) {
      this.notificationService.markAsRead(notification.id).subscribe();
    }
  }

  markAllAsRead(): void {
    this.notificationService.markAllAsRead().subscribe();
  }

  deleteNotification(notificationId: number): void {
    this.notificationService.deleteNotification(notificationId).subscribe();
  }

  navigateToAction(notification: Notification): void {
    if (notification.actionUrl) {
      // Navigate to action URL
      // Mark as read
      this.markAsRead(notification);
    }
  }
}
```

---

### Phase 4: Update Role-Specific Notification Pages

Update notification components for each role to use the backend service:

- `components/blasting-engineer/notifications/notifications.component.ts`
- `components/store-manager/notifications/*` (if exists, create if not)
- `components/explosive-manager/notifications/*` (if exists, create if not)
- `components/machine-manager/notifications/notifications.component.ts`
- `components/mechanical-engineer/maintenance/maintenance-notifications/maintenance-notifications.component.ts`
- `components/operator/notifications/notifications.component.ts`
- `components/admin/notifications/*` (if exists, create if not)

---

## 📅 Implementation Phases

### **Phase 1: Core Infrastructure** (Estimated: 2-3 days)
- [ ] Create Notification entity and enums
- [ ] Update DbContext and create migration
- [ ] Create repository interface and implementation
- [ ] Test database operations

### **Phase 2: Service Layer** (Estimated: 3-4 days)
- [ ] Create NotificationService interface
- [ ] Implement core notification methods
- [ ] Implement explosive approval notification methods
- [ ] Implement transfer request notification methods
- [ ] Implement machine assignment notification methods
- [ ] Implement maintenance notification methods
- [ ] Create unit tests

### **Phase 3: API Layer** (Estimated: 1-2 days)
- [ ] Create NotificationsController
- [ ] Test API endpoints with Postman
- [ ] Add authorization checks
- [ ] Test with different user roles

### **Phase 4: Workflow Integration** (Estimated: 3-4 days)
- [ ] Integrate with ExplosiveApprovalRequest workflow
- [ ] Integrate with InventoryTransferRequest workflow
- [ ] Integrate with MachineAssignment workflow
- [ ] Integrate with MaintenanceReport workflow
- [ ] Integrate with MaintenanceJob workflow
- [ ] Test end-to-end workflows

### **Phase 5: Frontend Models & Services** (Estimated: 2 days)
- [ ] Create TypeScript notification models
- [ ] Update NotificationService to use backend
- [ ] Add polling mechanism
- [ ] Test service methods

### **Phase 6: Frontend Components** (Estimated: 3-4 days)
- [ ] Update shared notification panel
- [ ] Update/create role-specific notification pages
- [ ] Add navigation from notifications
- [ ] Add notification badges in navbars
- [ ] Test UI/UX

### **Phase 7: Testing & Polish** (Estimated: 2-3 days)
- [ ] Integration testing
- [ ] User acceptance testing
- [ ] Performance testing
- [ ] Bug fixes
- [ ] Documentation

---

## 🧪 Testing Strategy

### Backend Testing

#### Unit Tests
```csharp
// NotificationRepositoryTests.cs
[Fact]
public async Task CreateNotification_ShouldAddToDatabase()
{
    // Arrange
    var notification = Notification.Create(...);

    // Act
    var result = await _repository.CreateAsync(notification);

    // Assert
    Assert.NotNull(result);
    Assert.True(result.Id > 0);
}
```

#### Integration Tests
- Test complete workflows (create request → approve → notification sent)
- Test notification creation for all scenarios
- Test notification retrieval and filtering
- Test mark as read/unread operations

### Frontend Testing
- Test notification service API calls
- Test notification display and filtering
- Test navigation from notifications
- Test notification badge updates
- Test polling mechanism

---

## 📊 Database Migration Script

```sql
-- Create Notifications table
CREATE TABLE Notifications (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    Type INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Message NVARCHAR(1000) NOT NULL,
    Priority INT NOT NULL DEFAULT 1,
    IsRead BIT NOT NULL DEFAULT 0,
    ReadAt DATETIME2 NULL,
    RelatedEntityType NVARCHAR(100) NULL,
    RelatedEntityId INT NULL,
    ActionUrl NVARCHAR(500) NULL,
    AdditionalData NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Create indexes for performance
CREATE INDEX IX_Notifications_UserId_IsRead ON Notifications(UserId, IsRead);
CREATE INDEX IX_Notifications_CreatedAt ON Notifications(CreatedAt DESC);
CREATE INDEX IX_Notifications_Type ON Notifications(Type);
CREATE INDEX IX_Notifications_UserId_CreatedAt ON Notifications(UserId, CreatedAt DESC);
```

---

## 🚀 Deployment Checklist

- [ ] Run database migration
- [ ] Update DI container to register NotificationService and NotificationRepository
- [ ] Deploy backend API
- [ ] Deploy frontend application
- [ ] Test in production environment
- [ ] Monitor notification creation and delivery
- [ ] Set up notification cleanup job (delete old notifications)

---

## 📈 Future Enhancements (Optional)

1. **Real-time Notifications with SignalR**
   - Push notifications instantly instead of polling
   - Better user experience

2. **Email Notifications**
   - Send email for critical notifications
   - Configurable per user

3. **Push Notifications (Web Push API)**
   - Browser push notifications
   - Works even when app is closed

4. **Notification Preferences**
   - Allow users to configure which notifications they want
   - Frequency settings

5. **Notification Grouping**
   - Group similar notifications
   - "3 new explosive requests" instead of 3 separate notifications

6. **Notification Templates**
   - Use templates for consistent messaging
   - Support multiple languages

---

## 📝 Summary

This comprehensive plan covers:

✅ **7 user roles** with complete notification coverage
✅ **50+ notification types** across all workflows
✅ **Full-stack implementation** (Backend + Frontend)
✅ **Database design** with proper indexing
✅ **Clean architecture** with repository and service patterns
✅ **Frontend integration** with existing Angular components
✅ **Testing strategy** for quality assurance
✅ **Phased implementation** for manageable development

**Estimated Total Time**: 16-22 days of development work

---

## 🎯 Next Steps

1. **Review this plan** and confirm approach
2. **Start with Phase 1** - Core Infrastructure
3. **Proceed sequentially** through phases
4. **Test thoroughly** at each phase
5. **Deploy incrementally** if possible

Would you like me to start implementing any specific phase?
