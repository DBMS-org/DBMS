using Microsoft.AspNetCore.Mvc;
using Application.DTOs.ProjectManagement;
using Application.Interfaces.ProjectManagement;
using Domain.Entities.ProjectManagement;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectSitesController : BaseApiController
    {
        private readonly IProjectSiteService _projectSiteService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<ProjectSitesController> _logger;

        public ProjectSitesController(
            IProjectSiteService projectSiteService,
            IAuthorizationService authorizationService,
            ILogger<ProjectSitesController> logger)
        {
            _projectSiteService = projectSiteService;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "ReadDrillData")]
        public async Task<IActionResult> GetProjectSites()
            {
                var projectSites = await _projectSiteService.GetAllProjectSitesAsync();
                return Ok(projectSites);
            }

        [HttpGet("{id}")]
        [Authorize(Policy = "ReadDrillData")]
        public async Task<IActionResult> GetProjectSite(int id)
            {
                var projectSite = await _projectSiteService.GetProjectSiteByIdAsync(id);
                if (projectSite == null)
                {
                    return NotFound($"Project site with ID {id} not found");
            }

            return Ok(projectSite);
        }

        [HttpGet("project/{projectId}")]
        [Authorize(Policy = "ReadDrillData")]
        public async Task<IActionResult> GetProjectSitesByProject(int projectId)
            {
                var projectSites = await _projectSiteService.GetProjectSitesByProjectIdAsync(projectId);
                return Ok(projectSites);
            }

        [HttpPost]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> CreateProjectSite(CreateProjectSiteRequest request)
        {
                var projectSite = await _projectSiteService.CreateProjectSiteAsync(request);
            return Created(projectSite, nameof(GetProjectSite));
            }

        [HttpPut("{id}")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> UpdateProjectSite(int id, ProjectSite request)
        {
                if (id != request.Id)
                {
                    return BadRequest("Project site ID mismatch");
                }

                var success = await _projectSiteService.UpdateProjectSiteAsync(id, request);
                if (!success)
                {
                    return NotFound($"Project site with ID {id} not found");
                }

            return Ok();
            }

        [HttpDelete("{id}")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> DeleteProjectSite(int id)
            {
                var success = await _projectSiteService.DeleteProjectSiteAsync(id);
                if (!success)
                {
                    return NotFound($"Project site with ID {id} not found");
                }
            return Ok();
            }

        [HttpPost("{id}/approve")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> ApprovePattern(int id)
            {
                var success = await _projectSiteService.ApprovePatternAsync(id);
                if (!success)
                {
                    return NotFound($"Project site with ID {id} not found");
                }
            return Ok();
            }

        [HttpPost("{id}/revoke")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> RevokePattern(int id)
            {
                var success = await _projectSiteService.RevokePatternAsync(id);
                if (!success)
                {
                    return NotFound($"Project site with ID {id} not found");
                }
            return Ok();
            }

        [HttpPost("{id}/confirm-simulation")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> ConfirmSimulation(int id)
            {
                var success = await _projectSiteService.ConfirmSimulationAsync(id);
                if (!success)
                {
                    return NotFound($"Project site with ID {id} not found");
                }
            return Ok();
            }

        [HttpPost("{id}/revoke-simulation")]
        [Authorize(Policy = "ManageProjectSites")]
        public async Task<IActionResult> RevokeSimulation(int id)
            {
                var success = await _projectSiteService.RevokeSimulationAsync(id);
                if (!success)
                {
                    return NotFound($"Project site with ID {id} not found");
                }
            return Ok();
        }

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

        // Explosive approval endpoints have been moved to ExplosiveApprovalRequestController
    }

    public class RequestExplosiveApprovalRequest
    {
        public DateTime ExpectedUsageDate { get; set; }
        public string? Comments { get; set; }
    }
}