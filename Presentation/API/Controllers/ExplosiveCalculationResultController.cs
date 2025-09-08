using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces.DrillingOperations;
using Application.DTOs.DrillingOperations;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/explosive-calculation-results")]
    [Authorize]
    public class ExplosiveCalculationResultController : BaseApiController
    {
        private readonly IExplosiveCalculationResultService _explosiveCalculationResultService;
        private readonly ILogger<ExplosiveCalculationResultController> _logger;

        public ExplosiveCalculationResultController(
            IExplosiveCalculationResultService explosiveCalculationResultService,
            ILogger<ExplosiveCalculationResultController> logger)
        {
            _explosiveCalculationResultService = explosiveCalculationResultService;
            _logger = logger;
        }

        /// <summary>
        /// Get all explosive calculation results for a specific project and site
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="siteId">Site ID</param>
        /// <returns>List of explosive calculation results</returns>
        [HttpGet("project/{projectId}/site/{siteId}")]
        public async Task<IActionResult> GetByProjectAndSite(int projectId, int siteId)
        {
            try
            {
                _logger.LogInformation("Getting explosive calculation results for project {ProjectId} and site {SiteId}", projectId, siteId);
                
                var result = await _explosiveCalculationResultService.GetExplosiveCalculationResultsBySiteIdAsync(projectId, siteId);
                
                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation results for project {ProjectId} and site {SiteId}", projectId, siteId);
                return StatusCode(500, "An error occurred while retrieving explosive calculation results");
            }
        }

        /// <summary>
        /// Get explosive calculation result by ID
        /// </summary>
        /// <param name="id">Calculation result ID</param>
        /// <returns>Explosive calculation result</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Getting explosive calculation result with ID {Id}", id);
                
                var result = await _explosiveCalculationResultService.GetExplosiveCalculationResultByIdAsync(id);
                
                if (result.IsSuccess)
                {
                    if (result.Value == null)
                    {
                        return NotFound($"Explosive calculation result with ID {id} not found");
                    }
                    return Ok(result.Value);
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation result with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the explosive calculation result");
            }
        }

        /// <summary>
        /// Get explosive calculation result by calculation ID
        /// </summary>
        /// <param name="calculationId">Calculation ID</param>
        /// <returns>Explosive calculation result</returns>
        [HttpGet("calculation/{calculationId}")]
        public async Task<IActionResult> GetByCalculationId(string calculationId)
        {
            try
            {
                _logger.LogInformation("Getting explosive calculation result with calculation ID {CalculationId}", calculationId);
                
                var result = await _explosiveCalculationResultService.GetExplosiveCalculationResultByCalculationIdAsync(calculationId);
                
                if (result.IsSuccess)
                {
                    if (result.Value == null)
                    {
                        return NotFound($"Explosive calculation result with calculation ID {calculationId} not found");
                    }
                    return Ok(result.Value);
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation result with calculation ID {CalculationId}", calculationId);
                return StatusCode(500, "An error occurred while retrieving the explosive calculation result");
            }
        }

        /// <summary>
        /// Check if existing calculations exist for a site
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="siteId">Site ID</param>
        /// <returns>Information about existing calculations</returns>
        [HttpGet("project/{projectId}/site/{siteId}/check-existing")]
        public async Task<IActionResult> CheckExistingCalculations(int projectId, int siteId)
        {
            try
            {
                _logger.LogInformation("Checking for existing calculations for project {ProjectId} and site {SiteId}", projectId, siteId);
                
                var result = await _explosiveCalculationResultService.GetExplosiveCalculationResultsDtoBySiteIdAsync(projectId, siteId);
                
                if (result.IsSuccess)
                {
                    var hasExisting = result.Value.Any();
                    return Ok(new { 
                        hasExisting = hasExisting,
                        count = result.Value.Count(),
                        calculations = hasExisting ? result.Value : null
                    });
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existing calculations for project {ProjectId} and site {SiteId}", projectId, siteId);
                return StatusCode(500, "An error occurred while checking existing calculations");
            }
        }

        /// <summary>
        /// Create a new explosive calculation result
        /// </summary>
        /// <param name="request">Create explosive calculation result request</param>
        /// <returns>Created explosive calculation result</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExplosiveCalculationResultRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Creating explosive calculation result for project {ProjectId} and site {SiteId}", request.ProjectId, request.SiteId);
                
                var result = await _explosiveCalculationResultService.CreateExplosiveCalculationResultFromDtoAsync(request);
                
                if (result.IsSuccess)
                {
                    return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
                }
                
                // If existing calculation found, return specific error with 409 Conflict status
                if (result.Error == "EXISTING_CALCULATION_FOUND")
                {
                    return Conflict(new { 
                        error = "EXISTING_CALCULATION_FOUND",
                        message = "A calculation already exists for this site. Please confirm to overwrite."
                    });
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating explosive calculation result");
                return StatusCode(500, "An error occurred while creating the explosive calculation result");
            }
        }

        /// <summary>
        /// Create a new explosive calculation result with confirmation (overwrites existing)
        /// </summary>
        /// <param name="request">Create explosive calculation result request</param>
        /// <returns>Created explosive calculation result</returns>
        [HttpPost("create-with-confirmation")]
        public async Task<IActionResult> CreateWithConfirmation([FromBody] CreateExplosiveCalculationResultRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Creating explosive calculation result with confirmation for project {ProjectId} and site {SiteId}", request.ProjectId, request.SiteId);
                
                var result = await _explosiveCalculationResultService.CreateExplosiveCalculationResultWithConfirmationAsync(request);
                
                if (result.IsSuccess)
                {
                    return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating explosive calculation result with confirmation");
                return StatusCode(500, "An error occurred while creating the explosive calculation result");
            }
        }

        /// <summary>
        /// Update an existing explosive calculation result
        /// </summary>
        /// <param name="id">Calculation result ID</param>
        /// <param name="request">Update explosive calculation result request</param>
        /// <returns>Updated explosive calculation result</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExplosiveCalculationResultRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Updating explosive calculation result with ID {Id}", id);
                
                var result = await _explosiveCalculationResultService.UpdateExplosiveCalculationResultFromDtoAsync(id, request);
                
                if (result.IsSuccess)
                {
                    return Ok();
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating explosive calculation result with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the explosive calculation result");
            }
        }

        /// <summary>
        /// Delete an explosive calculation result
        /// </summary>
        /// <param name="id">Calculation result ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting explosive calculation result with ID {Id}", id);
                
                var result = await _explosiveCalculationResultService.DeleteExplosiveCalculationResultAsync(id);
                
                if (result.IsSuccess)
                {
                    return NoContent();
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting explosive calculation result with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the explosive calculation result");
            }
        }

        /// <summary>
        /// Delete all explosive calculation results for a project
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("project/{projectId}")]
        [Authorize(Roles = "Admin,BlastingEngineer")]
        public async Task<IActionResult> DeleteByProject(int projectId)
        {
            try
            {
                _logger.LogInformation("Deleting all explosive calculation results for project {ProjectId}", projectId);
                
                var result = await _explosiveCalculationResultService.DeleteExplosiveCalculationResultsByProjectIdAsync(projectId);
                
                if (result.IsSuccess)
                {
                    return NoContent();
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting explosive calculation results for project {ProjectId}", projectId);
                return StatusCode(500, "An error occurred while deleting explosive calculation results");
            }
        }

        /// <summary>
        /// Delete all explosive calculation results for a site
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="siteId">Site ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("project/{projectId}/site/{siteId}")]
        [Authorize(Roles = "Admin,BlastingEngineer")]
        public async Task<IActionResult> DeleteBySite(int projectId, int siteId)
        {
            try
            {
                _logger.LogInformation("Deleting all explosive calculation results for project {ProjectId} and site {SiteId}", projectId, siteId);
                
                var result = await _explosiveCalculationResultService.DeleteExplosiveCalculationResultsBySiteIdAsync(projectId, siteId);
                
                if (result.IsSuccess)
                {
                    return NoContent();
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting explosive calculation results for project {ProjectId} and site {SiteId}", projectId, siteId);
                return StatusCode(500, "An error occurred while deleting explosive calculation results");
            }
        }

        /// <summary>
        /// Get count of explosive calculation results for a project and site
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="siteId">Site ID</param>
        /// <returns>Count of explosive calculation results</returns>
        [HttpGet("project/{projectId}/site/{siteId}/count")]
        public async Task<IActionResult> GetCount(int projectId, int siteId)
        {
            try
            {
                _logger.LogInformation("Getting count of explosive calculation results for project {ProjectId} and site {SiteId}", projectId, siteId);
                
                var result = await _explosiveCalculationResultService.GetExplosiveCalculationResultCountBySiteIdAsync(projectId, siteId);
                
                if (result.IsSuccess)
                {
                    return Ok(new { count = result.Value });
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting count of explosive calculation results for project {ProjectId} and site {SiteId}", projectId, siteId);
                return StatusCode(500, "An error occurred while getting the count");
            }
        }
    }
}