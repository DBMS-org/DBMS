# Backend Implementation Plan - October 15, 2025

## Overview
This document outlines the complete backend implementation plan to support the frontend features documented in [FRONTEND_TASKS_OCT_13.md](FRONTEND_TASKS_OCT_13.md).

---

## 1. Database Schema Updates

### 1.1 ExplosiveApprovalRequest Entity Updates
**File**: `Domain/Entities/ProjectManagement/ExplosiveApprovalRequest.cs`

**Add the following properties:**
```csharp
/// <summary>
/// Specific date when the blasting operation will occur
/// Optional when creating request, can be updated later
/// Required before approval by Store Manager
/// </summary>
public DateTime? BlastingDate { get; set; }

/// <summary>
/// Specific time when the blasting operation will occur
/// Format: "HH:mm" (24-hour format)
/// Optional when creating request, can be updated later
/// Required before approval by Store Manager
/// </summary>
public string? BlastTiming { get; set; }
```

**Business Rules:**
- Both fields are nullable (optional during creation)
- Can be updated at any time after request creation
- Store Manager MUST ensure both are populated before approval
- Store Manager can reject without timing requirements

---

### 1.2 ProjectSite Entity Updates
**File**: `Domain/Entities/ProjectManagement/ProjectSite.cs`

**Add the following properties:**
```csharp
/// <summary>
/// Indicates whether this project site has been marked as completed
/// </summary>
public bool IsCompleted { get; set; } = false;

/// <summary>
/// Date and time when the site was marked as completed
/// </summary>
public DateTime? CompletedAt { get; set; }

/// <summary>
/// User ID of the person who marked the site as completed
/// </summary>
public int? CompletedByUserId { get; set; }

// Navigation property
public virtual User? CompletedByUser { get; set; }
```

**Business Rules:**
- A site can only be completed if:
  - `IsPatternApproved == true`
  - `IsSimulationConfirmed == true`
  - `IsOperatorCompleted == true`
- Timing fields (BlastingDate/BlastTiming) are NOT required for site completion
- Once completed, `CompletedAt` is set to current UTC time
- `CompletedByUserId` stores the user who marked it complete

---

## 2. Database Migration

### 2.1 Create EF Core Migration
**Command:**
```bash
cd Infrastructure
dotnet ef migrations add AddBlastingTimingAndSiteCompletionFields --startup-project ../Presentation/API
```

**Migration Details:**
- Add `BlastingDate` (DateTime, nullable) to `ExplosiveApprovalRequests` table
- Add `BlastTiming` (nvarchar(10), nullable) to `ExplosiveApprovalRequests` table
- Add `IsCompleted` (bit, NOT NULL, default 0) to `ProjectSites` table
- Add `CompletedAt` (DateTime, nullable) to `ProjectSites` table
- Add `CompletedByUserId` (int, nullable) to `ProjectSites` table
- Add foreign key constraint from `ProjectSites.CompletedByUserId` to `Users.Id`

**Apply Migration:**
```bash
dotnet ef database update --startup-project ../Presentation/API
```

---

## 3. DTOs Updates

### 3.1 ExplosiveApprovalRequestDto
**File**: `Application/DTOs/ProjectManagement/ExplosiveApprovalRequestDto.cs`

**Add properties:**
```csharp
public DateTime? BlastingDate { get; set; }
public string? BlastTiming { get; set; }
```

---

### 3.2 CreateExplosiveApprovalRequestDto
**File**: `Presentation/API/Controllers/ExplosiveApprovalRequestController.cs`

**Update the inline DTO (lines 353-360):**
```csharp
public class CreateExplosiveApprovalRequestDto
{
    public int ProjectSiteId { get; set; }
    public DateTime ExpectedUsageDate { get; set; }
    public string? Comments { get; set; }
    public RequestPriority Priority { get; set; } = RequestPriority.Normal;
    public ExplosiveApprovalType ApprovalType { get; set; } = ExplosiveApprovalType.Standard;

    // NEW FIELDS
    public DateTime? BlastingDate { get; set; }
    public string? BlastTiming { get; set; }
}
```

---

### 3.3 UpdateExplosiveApprovalRequestDto
**File**: `Presentation/API/Controllers/ExplosiveApprovalRequestController.cs`

**Update the inline DTO (lines 362-368):**
```csharp
public class UpdateExplosiveApprovalRequestDto
{
    public DateTime ExpectedUsageDate { get; set; }
    public string? Comments { get; set; }
    public RequestPriority Priority { get; set; }
    public ExplosiveApprovalType ApprovalType { get; set; }

    // NEW FIELDS
    public DateTime? BlastingDate { get; set; }
    public string? BlastTiming { get; set; }
}
```

---

### 3.4 Create New UpdateBlastingTimingDto
**File**: `Presentation/API/Controllers/ExplosiveApprovalRequestController.cs`

**Add after other DTOs (~line 378):**
```csharp
public class UpdateBlastingTimingDto
{
    public DateTime? BlastingDate { get; set; }
    public string? BlastTiming { get; set; }
}
```

---

### 3.5 ProjectSiteDto
**File**: `Application/DTOs/ProjectManagement/ProjectSiteDto.cs`

**Add properties:**
```csharp
public bool IsCompleted { get; set; }
public DateTime? CompletedAt { get; set; }
public int? CompletedByUserId { get; set; }
public string? CompletedByUserName { get; set; }
```

---

## 4. Repository Layer Updates

### 4.1 IExplosiveApprovalRequestRepository
**File**: `Application/Interfaces/Infrastructure/Repositories/IExplosiveApprovalRequestRepository.cs`

**Add method signature after `DeleteAsync` (~line 55):**
```csharp
/// <summary>
/// Updates the blasting date and timing for an explosive approval request
/// </summary>
Task<bool> UpdateBlastingTimingAsync(int requestId, DateTime? blastingDate, string? blastTiming);
```

---

### 4.2 ExplosiveApprovalRequestRepository
**File**: `Infrastructure/Repositories/ProjectManagement/ExplosiveApprovalRequestRepository.cs`

**Add implementation before `GetByRegionAsync` method (~line 230):**
```csharp
public async Task<bool> UpdateBlastingTimingAsync(int requestId, DateTime? blastingDate, string? blastTiming)
{
    try
    {
        var request = await _context.ExplosiveApprovalRequests.FindAsync(requestId);
        if (request == null)
        {
            return false;
        }

        request.BlastingDate = blastingDate;
        request.BlastTiming = blastTiming;
        request.UpdatedAt = DateTime.UtcNow;

        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error updating blasting timing for request {RequestId}", requestId);
        throw;
    }
}
```

**Update `ApproveRequestAsync` method (line 130-158) to add validation:**
```csharp
public async Task<bool> ApproveRequestAsync(int requestId, int approvedByUserId, string? approvalComments = null)
{
    try
    {
        var request = await _context.ExplosiveApprovalRequests.FindAsync(requestId);
        if (request == null || request.Status != ExplosiveApprovalStatus.Pending)
        {
            return false;
        }

        // NEW VALIDATION: Ensure blasting date and timing are set before approval
        if (!request.BlastingDate.HasValue || string.IsNullOrWhiteSpace(request.BlastTiming))
        {
            throw new InvalidOperationException(
                "Cannot approve request: Blasting date and timing must be specified before approval.");
        }

        request.Status = ExplosiveApprovalStatus.Approved;
        request.ProcessedByUserId = approvedByUserId;
        request.ProcessedAt = DateTime.UtcNow;
        request.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrEmpty(approvalComments))
        {
            request.Comments = approvalComments;
        }

        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error approving explosive approval request {RequestId}", requestId);
        throw;
    }
}
```

---

### 4.3 IProjectSiteRepository
**File**: `Application/Interfaces/ProjectManagement/IProjectSiteRepository.cs`

**Add method signature:**
```csharp
/// <summary>
/// Checks if a project site exists by ID
/// </summary>
Task<bool> ExistsAsync(int id);

/// <summary>
/// Marks a project site as completed
/// </summary>
Task<bool> CompleteSiteAsync(int id, int completedByUserId);
```

---

### 4.4 ProjectSiteRepository
**File**: `Infrastructure/Repositories/ProjectManagement/ProjectSiteRepository.cs`

**Add implementations:**
```csharp
public async Task<bool> ExistsAsync(int id)
{
    return await _context.ProjectSites.AnyAsync(ps => ps.Id == id);
}

public async Task<bool> CompleteSiteAsync(int id, int completedByUserId)
{
    try
    {
        var site = await _context.ProjectSites.FindAsync(id);
        if (site == null)
        {
            return false;
        }

        // Validate that all required steps are completed
        if (!site.IsPatternApproved || !site.IsSimulationConfirmed || !site.IsOperatorCompleted)
        {
            throw new InvalidOperationException(
                "Cannot complete site: Pattern approval, simulation confirmation, and operator completion are all required.");
        }

        site.IsCompleted = true;
        site.CompletedAt = DateTime.UtcNow;
        site.CompletedByUserId = completedByUserId;
        site.UpdatedAt = DateTime.UtcNow;

        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error completing project site {SiteId}", id);
        throw;
    }
}
```

---

## 5. Service Layer Updates

### 5.1 IExplosiveApprovalRequestService
**File**: `Application/Interfaces/ProjectManagement/IExplosiveApprovalRequestService.cs`

**Add method signature after `DeleteExplosiveApprovalRequestAsync` (~line 61):**
```csharp
/// <summary>
/// Updates the blasting date and timing for an explosive approval request
/// </summary>
Task<bool> UpdateBlastingTimingAsync(int requestId, DateTime? blastingDate, string? blastTiming);
```

---

### 5.2 ExplosiveApprovalRequestApplicationService
**File**: `Application/Services/ProjectManagement/ExplosiveApprovalRequestApplicationService.cs`

**Update `CreateExplosiveApprovalRequestAsync` method signature (line 76-82):**
```csharp
public async Task<ExplosiveApprovalRequest> CreateExplosiveApprovalRequestAsync(
    int projectSiteId,
    int requestedByUserId,
    DateTime expectedUsageDate,
    string? comments = null,
    RequestPriority priority = RequestPriority.Normal,
    ExplosiveApprovalType approvalType = ExplosiveApprovalType.Standard,
    DateTime? blastingDate = null,
    string? blastTiming = null)
```

**Update the implementation (line 102-111):**
```csharp
var request = new ExplosiveApprovalRequest
{
    ProjectSiteId = projectSiteId,
    RequestedByUserId = requestedByUserId,
    ExpectedUsageDate = expectedUsageDate,
    Comments = comments,
    Priority = priority,
    ApprovalType = approvalType,
    Status = ExplosiveApprovalStatus.Pending,
    BlastingDate = blastingDate,  // NEW
    BlastTiming = blastTiming      // NEW
};
```

**Add new method implementation before `GetExplosiveApprovalRequestsByRegionAsync` (~line 220):**
```csharp
public async Task<bool> UpdateBlastingTimingAsync(int requestId, DateTime? blastingDate, string? blastTiming)
{
    try
    {
        // Validate timing format if provided
        if (!string.IsNullOrWhiteSpace(blastTiming))
        {
            if (!TimeSpan.TryParse(blastTiming, out _))
            {
                throw new ArgumentException("Invalid timing format. Expected format: HH:mm (e.g., 14:30)");
            }
        }

        return await _explosiveApprovalRequestRepository.UpdateBlastingTimingAsync(
            requestId, blastingDate, blastTiming);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error updating blasting timing for request {RequestId}", requestId);
        throw;
    }
}
```

---

### 5.3 IProjectSiteService
**File**: `Application/Interfaces/ProjectManagement/IProjectSiteService.cs`

**Add method signature after `RevokeSimulationAsync` (~line 17):**
```csharp
/// <summary>
/// Marks a project site as completed
/// </summary>
Task<bool> CompleteSiteAsync(int id, int completedByUserId);
```

---

### 5.4 ProjectSiteApplicationService
**File**: `Application/Services/ProjectManagement/ProjectSiteApplicationService.cs`

**Add implementation:**
```csharp
public async Task<bool> CompleteSiteAsync(int id, int completedByUserId)
{
    try
    {
        return await _projectSiteRepository.CompleteSiteAsync(id, completedByUserId);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error completing project site {SiteId}", id);
        throw;
    }
}
```

---

## 6. Controller Layer Updates

### 6.1 ExplosiveApprovalRequestController
**File**: `Presentation/API/Controllers/ExplosiveApprovalRequestController.cs`

#### 6.1.1 Update CreateExplosiveApprovalRequest Method (lines 81-121)
```csharp
[HttpPost]
[Authorize(Policy = "ManageProjectSites")]
public async Task<IActionResult> CreateExplosiveApprovalRequest([FromBody] CreateExplosiveApprovalRequestDto dto)
{
    try
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized("User ID not found in token");
        }

        var request = await _explosiveApprovalRequestService.CreateExplosiveApprovalRequestAsync(
            dto.ProjectSiteId,
            userId.Value,
            dto.ExpectedUsageDate,
            dto.Comments,
            dto.Priority,
            dto.ApprovalType,
            dto.BlastingDate,    // NEW
            dto.BlastTiming);     // NEW

        return CreatedAtAction(nameof(GetExplosiveApprovalRequest), new { id = request.Id }, request);
    }
    catch (ArgumentException ex)
    {
        return BadRequest(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        return Conflict(ex.Message);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error creating explosive approval request");
        return StatusCode(500, "An error occurred while creating the explosive approval request");
    }
}
```

#### 6.1.2 Add New UpdateBlastingTiming Endpoint (after UpdateExplosiveApprovalRequest, ~line 160)
```csharp
[HttpPut("{id}/timing")]
[Authorize(Policy = "ManageProjectSites")]
public async Task<IActionResult> UpdateBlastingTiming(int id, [FromBody] UpdateBlastingTimingDto dto)
{
    try
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingRequest = await _explosiveApprovalRequestService.GetExplosiveApprovalRequestByIdAsync(id);
        if (existingRequest == null)
        {
            return NotFound($"Explosive approval request with ID {id} not found");
        }

        // Only allow timing updates for pending or approved requests
        if (existingRequest.Status != ExplosiveApprovalStatus.Pending)
        {
            return BadRequest("Blasting timing can only be updated for pending requests");
        }

        var success = await _explosiveApprovalRequestService.UpdateBlastingTimingAsync(
            id, dto.BlastingDate, dto.BlastTiming);

        if (!success)
        {
            return StatusCode(500, "Failed to update blasting timing");
        }

        // Fetch updated request to return
        var updatedRequest = await _explosiveApprovalRequestService.GetExplosiveApprovalRequestByIdAsync(id);
        return Ok(updatedRequest);
    }
    catch (ArgumentException ex)
    {
        return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error updating blasting timing for request {RequestId}", id);
        return StatusCode(500, "An error occurred while updating the blasting timing");
    }
}
```

#### 6.1.3 Update ApproveExplosiveApprovalRequest Method (lines 162-187)
Add try-catch for InvalidOperationException to handle timing validation:

```csharp
[HttpPost("{id}/approve")]
[Authorize(Policy = "ManageExplosiveRequests")]
public async Task<IActionResult> ApproveExplosiveApprovalRequest(int id, [FromBody] ApprovalActionDto dto)
{
    try
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized("User ID not found in token");
        }

        var success = await _explosiveApprovalRequestService.ApproveExplosiveApprovalRequestAsync(
            id, userId.Value, dto.Comments);

        if (!success)
        {
            return NotFound($"Explosive approval request with ID {id} not found or cannot be approved");
        }

        return Ok(new { message = "Explosive approval request approved successfully" });
    }
    catch (InvalidOperationException ex)  // NEW: Catch timing validation error
    {
        return BadRequest(new { message = ex.Message });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error approving explosive approval request {RequestId}", id);
        return StatusCode(500, "An error occurred while approving the explosive approval request");
    }
}
```

---

### 6.2 ProjectSitesController
**File**: `Presentation/API/Controllers/ProjectSitesController.cs`

**Add new endpoint after RevokeSimulation (~line 136):**
```csharp
[HttpPost("{id}/complete")]
[Authorize(Policy = "ManageProjectSites")]
public async Task<IActionResult> CompleteSite(int id)
{
    try
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized("User ID not found in token");
        }

        var success = await _projectSiteService.CompleteSiteAsync(id, userId.Value);
        if (!success)
        {
            return NotFound($"Project site with ID {id} not found");
        }

        return Ok(new { message = "Project site marked as completed successfully" });
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error completing project site {SiteId}", id);
        return StatusCode(500, "An error occurred while completing the project site");
    }
}

private int? GetCurrentUserId()
{
    var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    return int.TryParse(userIdClaim, out var userId) ? userId : null;
}
```

---

## 7. API Endpoints Summary

### 7.1 New/Updated Endpoints

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| **NEW** | `PUT /api/explosive-approval-requests/{id}/timing` | Update blasting date and timing | ManageProjectSites |
| **UPDATED** | `POST /api/explosive-approval-requests` | Create request with optional timing | ManageProjectSites |
| **UPDATED** | `POST /api/explosive-approval-requests/{id}/approve` | Approve (validates timing exists) | ManageExplosiveRequests |
| **NEW** | `POST /api/projectsites/{id}/complete` | Mark project site as completed | ManageProjectSites |

---

## 8. Validation Rules Summary

### 8.1 ExplosiveApprovalRequest Rules
1. **Creation**: BlastingDate and BlastTiming are optional
2. **Timing Update**: Can be done anytime for pending requests
3. **Approval**:
   - MUST have both BlastingDate AND BlastTiming set
   - Returns 400 BadRequest if timing is missing
4. **Rejection**: No timing requirements (can reject anytime)
5. **Timing Format**: "HH:mm" (e.g., "14:30")

### 8.2 ProjectSite Completion Rules
1. **Prerequisites**:
   - `IsPatternApproved == true`
   - `IsSimulationConfirmed == true`
   - `IsOperatorCompleted == true`
2. **NOT Required**: BlastingDate/BlastTiming (independent workflows)
3. **Result**: Sets `IsCompleted = true`, `CompletedAt`, `CompletedByUserId`

---

## 9. Testing Checklist

### 9.1 Unit Tests Needed
- [ ] ExplosiveApprovalRequestRepository.UpdateBlastingTimingAsync
- [ ] ExplosiveApprovalRequestRepository.ApproveRequestAsync (with/without timing)
- [ ] ProjectSiteRepository.CompleteSiteAsync (with/without prerequisites)
- [ ] ExplosiveApprovalRequestApplicationService.UpdateBlastingTimingAsync (timing validation)
- [ ] ProjectSiteApplicationService.CompleteSiteAsync

### 9.2 Integration Tests Needed
- [ ] Create request without timing → Success
- [ ] Create request with timing → Success
- [ ] Update timing for pending request → Success
- [ ] Update timing for approved request → Failure
- [ ] Approve request without timing → Failure (400 BadRequest)
- [ ] Approve request with timing → Success
- [ ] Reject request without timing → Success
- [ ] Complete site without prerequisites → Failure
- [ ] Complete site with prerequisites → Success

### 9.3 Manual Testing Scenarios
1. **Blasting Engineer Workflow**:
   - Create request without timing
   - Update timing later
   - Verify Store Manager can see timing in request details

2. **Store Manager Workflow**:
   - Try to approve request without timing (should fail)
   - Update timing from Store Manager side (if allowed by business rules)
   - Approve request with timing (should succeed)

3. **Site Completion Workflow**:
   - Try to complete site without all prerequisites (should fail)
   - Complete all steps (pattern, simulation, operator)
   - Mark site as complete (should succeed)

---

## 10. Migration & Deployment Steps

1. **Create Migration**:
   ```bash
   cd Infrastructure
   dotnet ef migrations add AddBlastingTimingAndSiteCompletionFields --startup-project ../Presentation/API
   ```

2. **Review Migration**:
   - Check generated migration file
   - Ensure all columns have correct types and constraints

3. **Apply to Development Database**:
   ```bash
   dotnet ef database update --startup-project ../Presentation/API
   ```

4. **Test Endpoints** using Swagger/Postman

5. **Apply to Production**:
   - Backup database
   - Run migration
   - Verify data integrity

---

## 11. Future Enhancements

### 11.1 Potential Additions
- [ ] Add notification when timing is updated
- [ ] Add audit log for site completion events
- [ ] Add bulk timing update endpoint
- [ ] Add validation for BlastingDate (cannot be in past)
- [ ] Add conflict detection (multiple blasts at same site/time)

### 11.2 Performance Optimizations
- [ ] Add indexes on `BlastingDate` for date-range queries
- [ ] Add index on `IsCompleted` for filtering completed sites
- [ ] Cache project site completion status

---

## 12. Related Documentation

- [Frontend Tasks](FRONTEND_TASKS_OCT_13.md) - Frontend implementation requirements
- Entity Framework Core Migrations - Official documentation
- ASP.NET Core Web API - Best practices

---

**Document Created**: October 15, 2025
**Last Updated**: October 15, 2025
**Status**: Ready for Implementation
