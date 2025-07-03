using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.DTOs.BlastingOperations;
using Application.DTOs.DrillingOperations;
using Application.Interfaces.BlastingOperations;
using System.Security.Claims;
using Application.DTOs.Shared;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SiteBlastingController : BaseApiController
    {
        private readonly ISiteBlastingDataService _dataService;
        private readonly IDrillPatternService _patternService;
        private readonly IBlastSequenceService _sequenceService;
        private readonly IWorkflowProgressService _workflowService;

        public SiteBlastingController(
            ISiteBlastingDataService dataService,
            IDrillPatternService patternService,
            IBlastSequenceService sequenceService,
            IWorkflowProgressService workflowService)
        {
            _dataService = dataService;
            _patternService = patternService;
            _sequenceService = sequenceService;
            _workflowService = workflowService;
        }

        #region Generic Site Data Operations

        /// <summary>
        /// Get specific site data by type (pattern, connections, simulation_settings, simulation_state)
        /// </summary>
        [HttpGet("projects/{projectId}/sites/{siteId}/data/{dataType}")]
        public async Task<IActionResult> GetSiteData(int projectId, int siteId, string dataType)
        {
            var data = await _dataService.GetSiteDataAsync(projectId, siteId, dataType);
            if (data == null)
            {
                return NotFound($"No {dataType} data found for project {projectId}, site {siteId}");
            }
            return Ok(data);
        }

        /// <summary>
        /// Get all site data for a specific project/site combination
        /// </summary>
        [HttpGet("projects/{projectId}/sites/{siteId}/data")]
        public async Task<IActionResult> GetAllSiteData(int projectId, int siteId)
        {
            var data = await _dataService.GetAllSiteDataAsync(projectId, siteId);
            return Ok(data);
        }

        /// <summary>
        /// Save site data (pattern, connections, simulation settings, etc.)
        /// </summary>
        [HttpPost("projects/{projectId}/sites/{siteId}/data")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> SaveSiteData(int projectId, int siteId, [FromBody] CreateSiteBlastingDataRequest request)
        {
            request.ProjectId = projectId;
            request.SiteId = siteId;

            var userId = GetCurrentUserId();
            var savedData = await _dataService.SaveSiteDataAsync(request, userId);

            return Ok(savedData);
        }

        /// <summary>
        /// Delete specific site data by type
        /// </summary>
        [HttpDelete("projects/{projectId}/sites/{siteId}/data/{dataType}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteSiteData(int projectId, int siteId, string dataType)
        {
            var deleted = await _dataService.DeleteSiteDataAsync(projectId, siteId, dataType);
            if (!deleted)
            {
                return NotFound($"No {dataType} data found for project {projectId}, site {siteId}");
            }
            return Ok();
        }

        /// <summary>
        /// Delete all site data for a project/site combination
        /// </summary>
        [HttpDelete("projects/{projectId}/sites/{siteId}/data")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteAllSiteData(int projectId, int siteId)
        {
            var deleted = await _dataService.DeleteAllSiteDataAsync(projectId, siteId);
            if (!deleted)
            {
                return NotFound($"No data found for project {projectId}, site {siteId}");
            }
            return Ok();
        }

        /// <summary>
        /// Cleanup site data (individual steps or complete cleanup)
        /// </summary>
        [HttpPost("projects/{projectId}/sites/{siteId}/cleanup")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CleanupSiteData(int projectId, int siteId, [FromBody] CleanupSiteDataRequest request)
        {
            request.ProjectId = projectId;
            request.SiteId = siteId;

            var success = await _dataService.CleanupSiteDataAsync(request);
            if (!success)
            {
                return BadRequest("Failed to cleanup site data");
            }

            return Ok();
        }

        #endregion

        #region Bulk Operations

        /// <summary>
        /// Save multiple data types in a single request
        /// </summary>
        [HttpPost("projects/{projectId}/sites/{siteId}/bulk-save")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> SaveBulkSiteData(int projectId, int siteId, [FromBody] BulkSiteDataRequest request)
        {
            request.ProjectId = projectId;
            request.SiteId = siteId;

            var userId = GetCurrentUserId();
            var success = await _dataService.SaveBulkSiteDataAsync(request, userId);
            if (!success)
            {
                return BadRequest("Failed to save bulk site data");
            }
            return Ok();
        }

        /// <summary>
        /// Get all site data in bulk format
        /// </summary>
        [HttpGet("projects/{projectId}/sites/{siteId}/bulk")]
        public async Task<IActionResult> GetBulkSiteData(int projectId, int siteId)
        {
            var result = await _dataService.GetBulkSiteDataAsync(projectId, siteId);
            return Ok(result);
        }

        #endregion

        #region Drill Pattern Operations

        /// <summary>
        /// Get all drill patterns for a site
        /// </summary>
        [HttpGet("projects/{projectId}/sites/{siteId}/patterns")]
        public async Task<IActionResult> GetDrillPatterns(int projectId, int siteId)
        {
            var result = await _patternService.GetDrillPatternsAsync(projectId, siteId);
            return Ok(result);
        }

        /// <summary>
        /// Get a specific drill pattern
        /// </summary>
        [HttpGet("patterns/{id}")]
        public async Task<IActionResult> GetDrillPattern(int id)
        {
            var result = await _patternService.GetDrillPatternAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// Create a new drill pattern
        /// </summary>
        [HttpPost("projects/{projectId}/sites/{siteId}/patterns")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CreateDrillPattern(int projectId, int siteId, [FromBody] CreateDrillPatternRequest request)
        {
            request.ProjectId = projectId;
            request.SiteId = siteId;
            var userId = GetCurrentUserId();

            var result = await _patternService.CreateDrillPatternAsync(request, userId);
            return CreatedAtAction(nameof(GetDrillPattern), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing drill pattern
        /// </summary>
        [HttpPut("patterns/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> UpdateDrillPattern(int id, [FromBody] CreateDrillPatternRequest request)
        {
            var userId = GetCurrentUserId();
            var result = await _patternService.UpdateDrillPatternAsync(id, request, userId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// Delete a drill pattern (soft delete)
        /// </summary>
        [HttpDelete("patterns/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteDrillPattern(int id)
        {
            var success = await _patternService.DeleteDrillPatternAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return Ok();
        }

        #endregion

        #region Blast Sequence Operations

        /// <summary>
        /// Get all blast sequences for a site
        /// </summary>
        [HttpGet("projects/{projectId}/sites/{siteId}/sequences")]
        public async Task<IActionResult> GetBlastSequences(int projectId, int siteId)
        {
            var result = await _sequenceService.GetBlastSequencesAsync(projectId, siteId);
            return Ok(result);
        }

        /// <summary>
        /// Get a specific blast sequence
        /// </summary>
        [HttpGet("sequences/{id}")]
        public async Task<IActionResult> GetBlastSequence(int id)
        {
            var result = await _sequenceService.GetBlastSequenceAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// Create a new blast sequence
        /// </summary>
        [HttpPost("projects/{projectId}/sites/{siteId}/sequences")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CreateBlastSequence(int projectId, int siteId, [FromBody] CreateBlastSequenceRequest request)
        {
            request.ProjectId = projectId;
            request.SiteId = siteId;
            var userId = GetCurrentUserId();

            var result = await _sequenceService.CreateBlastSequenceAsync(request, userId);
            return CreatedAtAction(nameof(GetBlastSequence), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing blast sequence
        /// </summary>
        [HttpPut("sequences/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> UpdateBlastSequence(int id, [FromBody] CreateBlastSequenceRequest request)
        {
            var userId = GetCurrentUserId();
            var result = await _sequenceService.UpdateBlastSequenceAsync(id, request, userId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// Delete a blast sequence (soft delete)
        /// </summary>
        [HttpDelete("sequences/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteBlastSequence(int id)
        {
            var success = await _sequenceService.DeleteBlastSequenceAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return Ok();
        }

        #endregion

        #region Workflow Progress Tracking

        /// <summary>
        /// Get workflow progress for a site
        /// </summary>
        [HttpGet("projects/{projectId}/sites/{siteId}/progress")]
        public async Task<IActionResult> GetWorkflowProgress(int projectId, int siteId)
        {
            var result = await _workflowService.GetWorkflowProgressAsync(projectId, siteId);
            return Ok(result);
        }

        /// <summary>
        /// Update workflow progress for a specific step
        /// </summary>
        [HttpPut("projects/{projectId}/sites/{siteId}/progress/{stepId}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> UpdateWorkflowProgress(int projectId, int siteId, string stepId, [FromBody] bool completed)
        {
            var result = await _workflowService.UpdateWorkflowProgressAsync(projectId, siteId, stepId, completed);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        #endregion

        #region Validation Endpoints

        /// <summary>
        /// Validate if a project/site combination exists
        /// </summary>
        [HttpGet("projects/{projectId}/sites/{siteId}/validate")]
        public async Task<IActionResult> ValidateProjectSite(int projectId, int siteId)
        {
            var result = await _dataService.ValidateProjectSiteExistsAsync(projectId, siteId);
            return Ok(result);
        }

        #endregion

        #region Operator Completion Operations

        /// <summary>
        /// Mark operator completion for a site
        /// </summary>
        [HttpPost("projects/{projectId}/sites/{siteId}/operator-completion")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> SetOperatorCompletion(int projectId, int siteId)
        {
            await _workflowService.SetOperatorCompletionAsync(projectId, siteId, true);
            return Ok();
        }

        /// <summary>
        /// Revoke operator completion for a site
        /// </summary>
        [HttpDelete("projects/{projectId}/sites/{siteId}/operator-completion")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> RevokeOperatorCompletion(int projectId, int siteId)
        {
            await _workflowService.SetOperatorCompletionAsync(projectId, siteId, false);
            return Ok();
        }

        #endregion

        #region Helper Methods

        private int GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }
            return userId;
        }

        #endregion
    }
} 