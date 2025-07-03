using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.DTOs.BlastingOperations;
using Application.DTOs.DrillingOperations;
using Application.DTOs.Shared;
using Application.Interfaces.BlastingOperations;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SiteBlastingController : ControllerBase
    {
        private readonly ISiteBlastingDataService _dataService;
        private readonly IDrillPatternService _patternService;
        private readonly IBlastSequenceService _sequenceService;
        private readonly IWorkflowProgressService _workflowService;
        private readonly ILogger<SiteBlastingController> _logger;

        public SiteBlastingController(
            ISiteBlastingDataService dataService,
            IDrillPatternService patternService,
            IBlastSequenceService sequenceService,
            IWorkflowProgressService workflowService,
            ILogger<SiteBlastingController> logger)
        {
            _dataService = dataService;
            _patternService = patternService;
            _sequenceService = sequenceService;
            _workflowService = workflowService;
            _logger = logger;
        }

        #region Generic Site Data Operations

        /// <summary>
        /// Get specific site data by type (pattern, connections, simulation_settings, simulation_state)
        /// </summary>
        [HttpGet("projects/{projectId}/sites/{siteId}/data/{dataType}")]
        public async Task<ActionResult<SiteBlastingDataDto>> GetSiteData(int projectId, int siteId, string dataType)
        {
            try
            {
                var data = await _dataService.GetSiteDataAsync(projectId, siteId, dataType);
                
                if (data == null)
                {
                    return NotFound($"No {dataType} data found for project {projectId}, site {siteId}");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting site data for project {ProjectId}, site {SiteId}, type {DataType}", 
                    projectId, siteId, dataType);
                return StatusCode(500, "Internal server error occurred while fetching site data");
            }
        }

        /// <summary>
        /// Get all site data for a specific project/site combination
        /// </summary>
        [HttpGet("projects/{projectId}/sites/{siteId}/data")]
        public async Task<ActionResult<List<SiteBlastingDataDto>>> GetAllSiteData(int projectId, int siteId)
        {
            try
            {
                var data = await _dataService.GetAllSiteDataAsync(projectId, siteId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all site data for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                return StatusCode(500, "Internal server error occurred while fetching site data");
            }
        }

        /// <summary>
        /// Save site data (pattern, connections, simulation settings, etc.)
        /// </summary>
        [HttpPost("projects/{projectId}/sites/{siteId}/data")]
        public async Task<ActionResult<SiteBlastingDataDto>> SaveSiteData(int projectId, int siteId, [FromBody] CreateSiteBlastingDataRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Ensure the route parameters match the request
                request.ProjectId = projectId;
                request.SiteId = siteId;

                var userId = GetCurrentUserId();
                var savedData = await _dataService.SaveSiteDataAsync(request, userId);

                return Ok(savedData);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving site data for project {ProjectId}, site {SiteId}, type {DataType}", 
                    projectId, siteId, request.DataType);
                return StatusCode(500, "Internal server error occurred while saving site data");
            }
        }

        /// <summary>
        /// Delete specific site data by type
        /// </summary>
        [HttpDelete("projects/{projectId}/sites/{siteId}/data/{dataType}")]
        public async Task<IActionResult> DeleteSiteData(int projectId, int siteId, string dataType)
        {
            try
            {
                var deleted = await _dataService.DeleteSiteDataAsync(projectId, siteId, dataType);
                
                if (!deleted)
                {
                    return NotFound($"No {dataType} data found for project {projectId}, site {siteId}");
                }

                return Ok(new { message = $"Successfully deleted {dataType} data" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting site data for project {ProjectId}, site {SiteId}, type {DataType}", 
                    projectId, siteId, dataType);
                return StatusCode(500, "Internal server error occurred while deleting site data");
            }
        }

        /// <summary>
        /// Delete all site data for a project/site combination
        /// </summary>
        [HttpDelete("projects/{projectId}/sites/{siteId}/data")]
        public async Task<IActionResult> DeleteAllSiteData(int projectId, int siteId)
        {
            try
            {
                var deleted = await _dataService.DeleteAllSiteDataAsync(projectId, siteId);
                
                if (!deleted)
                {
                    return NotFound($"No data found for project {projectId}, site {siteId}");
                }

                return Ok(new { message = "Successfully deleted all site data" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting all site data for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                return StatusCode(500, "Internal server error occurred while deleting site data");
            }
        }

        /// <summary>
        /// Cleanup site data (individual steps or complete cleanup)
        /// </summary>
        [HttpPost("projects/{projectId}/sites/{siteId}/cleanup")]
        public async Task<IActionResult> CleanupSiteData(int projectId, int siteId, [FromBody] CleanupSiteDataRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Ensure the route parameters match the request
                request.ProjectId = projectId;
                request.SiteId = siteId;

                var success = await _dataService.CleanupSiteDataAsync(request);
                
                if (!success)
                {
                    return BadRequest("Failed to cleanup site data");
                }

                var message = request.CleanAll 
                    ? "Successfully cleaned up all site data" 
                    : $"Successfully cleaned up {string.Join(", ", request.DataTypesToClean)} data";

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up site data for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                return StatusCode(500, "Internal server error occurred while cleaning up site data");
            }
        }

        #endregion

        #region Bulk Operations

        /// <summary>
        /// Save multiple data types in a single request
        /// </summary>
        [HttpPost("projects/{projectId}/sites/{siteId}/bulk-save")]
        public async Task<IActionResult> SaveBulkSiteData(int projectId, int siteId, [FromBody] BulkSiteDataRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Ensure the route parameters match the request
                request.ProjectId = projectId;
                request.SiteId = siteId;

                var userId = GetCurrentUserId();
                var success = await _dataService.SaveBulkSiteDataAsync(request, userId);
                
                if (!success)
                {
                    return BadRequest("Failed to save bulk site data");
                }

                return Ok(new { message = "Successfully saved bulk site data" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving bulk site data for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                return StatusCode(500, "Internal server error occurred while saving bulk site data");
            }
        }

        /// <summary>
        /// Get all site data in bulk format
        /// </summary>
        [HttpGet("projects/{projectId}/sites/{siteId}/bulk")]
        public async Task<ActionResult<Dictionary<string, SiteBlastingDataDto>>> GetBulkSiteData(int projectId, int siteId)
        {
            try
            {
                var data = await _dataService.GetBulkSiteDataAsync(projectId, siteId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bulk site data for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                return StatusCode(500, "Internal server error occurred while fetching bulk site data");
            }
        }

        #endregion

        #region Drill Pattern Operations

        /// <summary>
        /// Get all drill patterns for a site
        /// </summary>
        [HttpGet("projects/{projectId}/sites/{siteId}/patterns")]
        public async Task<ActionResult<List<DrillPatternDto>>> GetDrillPatterns(int projectId, int siteId)
        {
            try
            {
                var patterns = await _patternService.GetDrillPatternsAsync(projectId, siteId);
                return Ok(patterns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill patterns for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                return StatusCode(500, "Internal server error occurred while fetching drill patterns");
            }
        }

        /// <summary>
        /// Get a specific drill pattern
        /// </summary>
        [HttpGet("patterns/{id}")]
        public async Task<ActionResult<DrillPatternDto>> GetDrillPattern(int id)
        {
            try
            {
                var pattern = await _patternService.GetDrillPatternAsync(id);
                
                if (pattern == null)
                {
                    return NotFound($"Drill pattern {id} not found");
                }

                return Ok(pattern);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill pattern {Id}", id);
                return StatusCode(500, "Internal server error occurred while fetching drill pattern");
            }
        }

        /// <summary>
        /// Create a new drill pattern
        /// </summary>
        [HttpPost("projects/{projectId}/sites/{siteId}/patterns")]
        public async Task<ActionResult<DrillPatternDto>> CreateDrillPattern(int projectId, int siteId, [FromBody] CreateDrillPatternRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Ensure the route parameters match the request
                request.ProjectId = projectId;
                request.SiteId = siteId;

                var userId = GetCurrentUserId();
                var pattern = await _patternService.CreateDrillPatternAsync(request, userId);

                return CreatedAtAction(nameof(GetDrillPattern), new { id = pattern.Id }, pattern);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating drill pattern for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                return StatusCode(500, "Internal server error occurred while creating drill pattern");
            }
        }

        /// <summary>
        /// Update an existing drill pattern
        /// </summary>
        [HttpPut("patterns/{id}")]
        public async Task<ActionResult<DrillPatternDto>> UpdateDrillPattern(int id, [FromBody] CreateDrillPatternRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();
                var pattern = await _patternService.UpdateDrillPatternAsync(id, request, userId);

                return Ok(pattern);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating drill pattern {Id}", id);
                return StatusCode(500, "Internal server error occurred while updating drill pattern");
            }
        }

        /// <summary>
        /// Delete a drill pattern (soft delete)
        /// </summary>
        [HttpDelete("patterns/{id}")]
        public async Task<IActionResult> DeleteDrillPattern(int id)
        {
            try
            {
                var deleted = await _patternService.DeleteDrillPatternAsync(id);
                
                if (!deleted)
                {
                    return NotFound($"Drill pattern {id} not found");
                }

                return Ok(new { message = "Successfully deleted drill pattern" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting drill pattern {Id}", id);
                return StatusCode(500, "Internal server error occurred while deleting drill pattern");
            }
        }

        #endregion

        #region Blast Sequence Operations

        /// <summary>
        /// Get all blast sequences for a site
        /// </summary>
        [HttpGet("projects/{projectId}/sites/{siteId}/sequences")]
        public async Task<ActionResult<List<BlastSequenceDto>>> GetBlastSequences(int projectId, int siteId)
        {
            try
            {
                var sequences = await _sequenceService.GetBlastSequencesAsync(projectId, siteId);
                return Ok(sequences);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blast sequences for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                return StatusCode(500, "Internal server error occurred while fetching blast sequences");
            }
        }

        /// <summary>
        /// Get a specific blast sequence
        /// </summary>
        [HttpGet("sequences/{id}")]
        public async Task<ActionResult<BlastSequenceDto>> GetBlastSequence(int id)
        {
            try
            {
                var sequence = await _sequenceService.GetBlastSequenceAsync(id);
                
                if (sequence == null)
                {
                    return NotFound($"Blast sequence {id} not found");
                }

                return Ok(sequence);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blast sequence {Id}", id);
                return StatusCode(500, "Internal server error occurred while fetching blast sequence");
            }
        }

        /// <summary>
        /// Create a new blast sequence
        /// </summary>
        [HttpPost("projects/{projectId}/sites/{siteId}/sequences")]
        public async Task<ActionResult<BlastSequenceDto>> CreateBlastSequence(int projectId, int siteId, [FromBody] CreateBlastSequenceRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Ensure the route parameters match the request
                request.ProjectId = projectId;
                request.SiteId = siteId;

                var userId = GetCurrentUserId();
                var sequence = await _sequenceService.CreateBlastSequenceAsync(request, userId);

                return CreatedAtAction(nameof(GetBlastSequence), new { id = sequence.Id }, sequence);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating blast sequence for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                return StatusCode(500, "Internal server error occurred while creating blast sequence");
            }
        }

        /// <summary>
        /// Update an existing blast sequence
        /// </summary>
        [HttpPut("sequences/{id}")]
        public async Task<ActionResult<BlastSequenceDto>> UpdateBlastSequence(int id, [FromBody] CreateBlastSequenceRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();
                var sequence = await _sequenceService.UpdateBlastSequenceAsync(id, request, userId);

                return Ok(sequence);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blast sequence {Id}", id);
                return StatusCode(500, "Internal server error occurred while updating blast sequence");
            }
        }

        /// <summary>
        /// Delete a blast sequence (soft delete)
        /// </summary>
        [HttpDelete("sequences/{id}")]
        public async Task<IActionResult> DeleteBlastSequence(int id)
        {
            try
            {
                var deleted = await _sequenceService.DeleteBlastSequenceAsync(id);
                
                if (!deleted)
                {
                    return NotFound($"Blast sequence {id} not found");
                }

                return Ok(new { message = "Successfully deleted blast sequence" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blast sequence {Id}", id);
                return StatusCode(500, "Internal server error occurred while deleting blast sequence");
            }
        }

        #endregion

        #region Workflow Progress Tracking

        /// <summary>
        /// Get workflow progress for a site
        /// </summary>
        [HttpGet("projects/{projectId}/sites/{siteId}/progress")]
        public async Task<ActionResult<SiteWorkflowProgressDto>> GetWorkflowProgress(int projectId, int siteId)
        {
            try
            {
                var progress = await _workflowService.GetWorkflowProgressAsync(projectId, siteId);
                return Ok(progress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workflow progress for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                return StatusCode(500, "Internal server error occurred while fetching workflow progress");
            }
        }

        /// <summary>
        /// Update workflow progress for a specific step
        /// </summary>
        [HttpPut("projects/{projectId}/sites/{siteId}/progress/{stepId}")]
        public async Task<ActionResult<SiteWorkflowProgressDto>> UpdateWorkflowProgress(int projectId, int siteId, string stepId, [FromBody] bool completed)
        {
            try
            {
                var progress = await _workflowService.UpdateWorkflowProgressAsync(projectId, siteId, stepId, completed);
                return Ok(progress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workflow progress for project {ProjectId}, site {SiteId}, step {StepId}", 
                    projectId, siteId, stepId);
                return StatusCode(500, "Internal server error occurred while updating workflow progress");
            }
        }

        #endregion

        #region Validation Endpoints

        /// <summary>
        /// Validate if a project/site combination exists
        /// </summary>
        [HttpGet("projects/{projectId}/sites/{siteId}/validate")]
        public async Task<ActionResult<bool>> ValidateProjectSite(int projectId, int siteId)
        {
            try
            {
                var isValid = await _dataService.ValidateProjectSiteExistsAsync(projectId, siteId);
                return Ok(new { isValid, projectId, siteId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating project site: project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                return StatusCode(500, "Internal server error occurred while validating project site");
            }
        }

        #endregion

        #region Operator Completion Operations

        /// <summary>
        /// Mark operator completion for a site
        /// </summary>
        [HttpPost("projects/{projectId}/sites/{siteId}/operator-completion")]
        public async Task<IActionResult> SetOperatorCompletion(int projectId, int siteId)
        {
            var success = await _workflowService.SetOperatorCompletionAsync(projectId, siteId, true);
            if (!success)
                return NotFound("Project site not found");
            return Ok(new { message = "Operator completion marked." });
        }

        /// <summary>
        /// Revoke operator completion for a site
        /// </summary>
        [HttpDelete("projects/{projectId}/sites/{siteId}/operator-completion")]
        public async Task<IActionResult> RevokeOperatorCompletion(int projectId, int siteId)
        {
            var success = await _workflowService.SetOperatorCompletionAsync(projectId, siteId, false);
            if (!success)
                return NotFound("Project site not found");
            return Ok(new { message = "Operator completion revoked." });
        }

        #endregion

        #region Helper Methods

        private int GetCurrentUserId()
        {
            // For now, return a default user ID since authentication is disabled
            // In production, this should be properly implemented with authentication
            return 1; // Default to admin user
        }

        #endregion
    }
} 