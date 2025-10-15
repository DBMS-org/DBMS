using Application.Interfaces.ProjectManagement;
using Application.Interfaces.Infrastructure;
using Application.DTOs.ProjectManagement;
using Domain.Entities.ProjectManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.API.Controllers
{
    [ApiController]
    [Route("api/explosive-approval-requests")]
    public class ExplosiveApprovalRequestController : ControllerBase
    {
        private readonly IExplosiveApprovalRequestService _explosiveApprovalRequestService;
        private readonly IMappingService _mappingService;
        private readonly ILogger<ExplosiveApprovalRequestController> _logger;

        public ExplosiveApprovalRequestController(
            IExplosiveApprovalRequestService explosiveApprovalRequestService,
            IMappingService mappingService,
            ILogger<ExplosiveApprovalRequestController> logger)
        {
            _explosiveApprovalRequestService = explosiveApprovalRequestService;
            _mappingService = mappingService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> GetExplosiveApprovalRequest(int id)
        {
            try
            {
                var request = await _explosiveApprovalRequestService.GetExplosiveApprovalRequestByIdAsync(id);
                if (request == null)
                {
                    return NotFound($"Explosive approval request with ID {id} not found");
                }
                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving explosive approval request {RequestId}", id);
                return StatusCode(500, "An error occurred while retrieving the explosive approval request");
            }
        }

        [HttpGet("project-site/{projectSiteId}")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> GetExplosiveApprovalRequestsByProjectSite(int projectSiteId)
        {
            try
            {
                var requests = await _explosiveApprovalRequestService.GetExplosiveApprovalRequestsByProjectSiteIdAsync(projectSiteId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving explosive approval requests for project site {ProjectSiteId}", projectSiteId);
                return StatusCode(500, "An error occurred while retrieving the explosive approval requests");
            }
        }

        [HttpGet("pending")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> GetPendingExplosiveApprovalRequests()
        {
            try
            {
                var requests = await _explosiveApprovalRequestService.GetPendingExplosiveApprovalRequestsAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending explosive approval requests");
                return StatusCode(500, "An error occurred while retrieving pending explosive approval requests");
            }
        }

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
                    dto.BlastingDate,
                    dto.BlastTiming);

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

        [HttpPut("{id}")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> UpdateExplosiveApprovalRequest(int id, [FromBody] UpdateExplosiveApprovalRequestDto dto)
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

                // Update properties
                existingRequest.ExpectedUsageDate = dto.ExpectedUsageDate;
                existingRequest.Comments = dto.Comments;
                existingRequest.Priority = dto.Priority;
                existingRequest.ApprovalType = dto.ApprovalType;
                existingRequest.UpdatedAt = DateTime.UtcNow;

                var success = await _explosiveApprovalRequestService.UpdateExplosiveApprovalRequestAsync(existingRequest);
                if (!success)
                {
                    return StatusCode(500, "Failed to update explosive approval request");
                }

                return Ok(existingRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating explosive approval request {RequestId}", id);
                return StatusCode(500, "An error occurred while updating the explosive approval request");
            }
        }

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

                // Only allow timing updates for pending requests
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

                var success = await _explosiveApprovalRequestService.ApproveExplosiveApprovalRequestAsync(id, userId.Value, dto.Comments);
                if (!success)
                {
                    return NotFound($"Explosive approval request with ID {id} not found or cannot be approved");
                }

                return Ok(new { message = "Explosive approval request approved successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving explosive approval request {RequestId}", id);
                return StatusCode(500, "An error occurred while approving the explosive approval request");
            }
        }

        [HttpPost("{id}/reject")]
        [Authorize(Policy = "ManageExplosiveRequests")]
        public async Task<IActionResult> RejectExplosiveApprovalRequest(int id, [FromBody] RejectionActionDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.RejectionReason))
                {
                    return BadRequest("Rejection reason is required");
                }

                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized("User ID not found in token");
                }

                var success = await _explosiveApprovalRequestService.RejectExplosiveApprovalRequestAsync(id, userId.Value, dto.RejectionReason);
                if (!success)
                {
                    return NotFound($"Explosive approval request with ID {id} not found or cannot be rejected");
                }

                return Ok(new { message = "Explosive approval request rejected successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting explosive approval request {RequestId}", id);
                return StatusCode(500, "An error occurred while rejecting the explosive approval request");
            }
        }

        [HttpPost("{id}/cancel")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> CancelExplosiveApprovalRequest(int id)
        {
            try
            {
                var success = await _explosiveApprovalRequestService.CancelExplosiveApprovalRequestAsync(id);
                if (!success)
                {
                    return NotFound($"Explosive approval request with ID {id} not found or cannot be cancelled");
                }

                return Ok(new { message = "Explosive approval request cancelled successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling explosive approval request {RequestId}", id);
                return StatusCode(500, "An error occurred while cancelling the explosive approval request");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> DeleteExplosiveApprovalRequest(int id)
        {
            try
            {
                var success = await _explosiveApprovalRequestService.DeleteExplosiveApprovalRequestAsync(id);
                if (!success)
                {
                    return NotFound($"Explosive approval request with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting explosive approval request {RequestId}", id);
                return StatusCode(500, "An error occurred while deleting the explosive approval request");
            }
        }

        [HttpGet("project-site/{projectSiteId}/has-pending")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> HasPendingExplosiveApprovalRequest(int projectSiteId)
        {
            try
            {
                var hasPending = await _explosiveApprovalRequestService.HasPendingExplosiveApprovalRequestAsync(projectSiteId);
                return Ok(new { hasPendingRequest = hasPending });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking for pending explosive approval requests for project site {ProjectSiteId}", projectSiteId);
                return StatusCode(500, "An error occurred while checking for pending explosive approval requests");
            }
        }

        [HttpGet("project-site/{projectSiteId}/latest")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> GetLatestExplosiveApprovalRequest(int projectSiteId)
        {
            try
            {
                var request = await _explosiveApprovalRequestService.GetLatestExplosiveApprovalRequestAsync(projectSiteId);
                if (request == null)
                {
                    return NotFound($"No explosive approval requests found for project site {projectSiteId}");
                }
                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest explosive approval request for project site {ProjectSiteId}", projectSiteId);
                return StatusCode(500, "An error occurred while retrieving the latest explosive approval request");
            }
        }

        [HttpGet("my-requests")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> GetMyExplosiveApprovalRequests()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized("User ID not found in token");
                }

                var requests = await _explosiveApprovalRequestService.GetExplosiveApprovalRequestsByUserIdAsync(userId.Value);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving explosive approval requests for current user");
                return StatusCode(500, "An error occurred while retrieving the explosive approval requests");
            }
        }

        [HttpGet("store-manager/region/{region}")]
        [Authorize(Policy = "ManageExplosiveRequests")]
        public async Task<IActionResult> GetExplosiveApprovalRequestsByRegion(string region)
        {
            try
            {
                // Verify that the current user is a store manager and can only access their own region
                var currentUserRegion = User.FindFirst("region")?.Value;
                if (!User.IsInRole("Admin") && !User.IsInRole("Administrator") && currentUserRegion != region)
                {
                    return StatusCode(403, new { message = "You can only access explosive approval requests from your assigned region." });
                }

                var requests = await _explosiveApprovalRequestService.GetExplosiveApprovalRequestsByRegionAsync(region);
                var requestDtos = _mappingService.Map<IEnumerable<ExplosiveApprovalRequestDto>>(requests);
                return Ok(requestDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving explosive approval requests for region {Region}", region);
                return StatusCode(500, "An error occurred while retrieving the explosive approval requests");
            }
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }

    // DTOs for the controller
    public class CreateExplosiveApprovalRequestDto
    {
        public int ProjectSiteId { get; set; }
        public DateTime ExpectedUsageDate { get; set; }
        public string? Comments { get; set; }
        public RequestPriority Priority { get; set; } = RequestPriority.Normal;
        public ExplosiveApprovalType ApprovalType { get; set; } = ExplosiveApprovalType.Standard;
        public DateTime? BlastingDate { get; set; }
        public string? BlastTiming { get; set; }
    }

    public class UpdateExplosiveApprovalRequestDto
    {
        public DateTime ExpectedUsageDate { get; set; }
        public string? Comments { get; set; }
        public RequestPriority Priority { get; set; }
        public ExplosiveApprovalType ApprovalType { get; set; }
        public DateTime? BlastingDate { get; set; }
        public string? BlastTiming { get; set; }
    }

    public class UpdateBlastingTimingDto
    {
        public DateTime? BlastingDate { get; set; }
        public string? BlastTiming { get; set; }
    }

    public class ApprovalActionDto
    {
        public string? Comments { get; set; }
    }

    public class RejectionActionDto
    {
        public string RejectionReason { get; set; } = string.Empty;
    }
}