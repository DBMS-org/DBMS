using Microsoft.AspNetCore.Mvc;
using Application.DTOs.ProjectManagement;
using Application.Interfaces.ProjectManagement;
using Domain.Entities.ProjectManagement;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectSitesController : ControllerBase
    {
        private readonly IProjectSiteService _projectSiteService;
        private readonly ILogger<ProjectSitesController> _logger;

        public ProjectSitesController(
            IProjectSiteService projectSiteService, 
            ILogger<ProjectSitesController> logger)
        {
            _projectSiteService = projectSiteService;
            _logger = logger;
        }

        // GET: api/projectsites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectSite>>> GetProjectSites()
        {
            try
            {
                var projectSites = await _projectSiteService.GetAllProjectSitesAsync();
                return Ok(projectSites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching project sites");
                return StatusCode(500, "Internal server error occurred while fetching project sites");
            }
        }

        // GET: api/projectsites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectSite?>> GetProjectSite(int id)
        {
            try
            {
                var projectSite = await _projectSiteService.GetProjectSiteByIdAsync(id);

                if (projectSite == null)
                {
                    return NotFound($"Project site with ID {id} not found");
                }

                return Ok(projectSite);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching project site with ID {ProjectSiteId}", id);
                return StatusCode(500, "Internal server error occurred while fetching project site");
            }
        }

        // GET: api/projectsites/project/5
        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<ProjectSite>>> GetProjectSitesByProject(int projectId)
        {
            try
            {
                var projectSites = await _projectSiteService.GetProjectSitesByProjectIdAsync(projectId);
                return Ok(projectSites);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching project sites for project {ProjectId}", projectId);
                return StatusCode(500, "Internal server error occurred while fetching project sites");
            }
        }

        // POST: api/projectsites
        [HttpPost]
        public async Task<ActionResult<ProjectSite>> CreateProjectSite(CreateProjectSiteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var projectSite = await _projectSiteService.CreateProjectSiteAsync(request);
                return CreatedAtAction(nameof(GetProjectSite), new { id = projectSite.Id }, projectSite);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating project site");
                return StatusCode(500, "Internal server error occurred while creating project site");
            }
        }

        // PUT: api/projectsites/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectSite(int id, ProjectSite request)
        {
            try
            {
                if (id != request.Id)
                {
                    return BadRequest("Project site ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var success = await _projectSiteService.UpdateProjectSiteAsync(id, request);
                if (!success)
                {
                    return NotFound($"Project site with ID {id} not found");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating project site {ProjectSiteId}", id);
                return StatusCode(500, "Internal server error occurred while updating project site");
            }
        }

        // DELETE: api/projectsites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectSite(int id)
        {
            try
            {
                var success = await _projectSiteService.DeleteProjectSiteAsync(id);
                if (!success)
                {
                    return NotFound($"Project site with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting project site {ProjectSiteId}", id);
                return StatusCode(500, "Internal server error occurred while deleting project site");
            }
        }

        // POST: api/projectsites/{id}/approve
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApprovePattern(int id)
        {
            try
            {
                var success = await _projectSiteService.ApprovePatternAsync(id);
                if (!success)
                {
                    return NotFound($"Project site with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while approving pattern for project site {ProjectSiteId}", id);
                return StatusCode(500, "Internal server error occurred while approving pattern");
            }
        }

        // POST: api/projectsites/{id}/revoke
        [HttpPost("{id}/revoke")]
        public async Task<IActionResult> RevokePattern(int id)
        {
            try
            {
                var success = await _projectSiteService.RevokePatternAsync(id);
                if (!success)
                {
                    return NotFound($"Project site with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while revoking pattern for project site {ProjectSiteId}", id);
                return StatusCode(500, "Internal server error occurred while revoking pattern");
            }
        }

        // POST: api/projectsites/{id}/confirm-simulation
        [HttpPost("{id}/confirm-simulation")]
        public async Task<IActionResult> ConfirmSimulation(int id)
        {
            try
            {
                var success = await _projectSiteService.ConfirmSimulationAsync(id);
                if (!success)
                {
                    return NotFound($"Project site with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while confirming simulation for project site {ProjectSiteId}", id);
                return StatusCode(500, "Internal server error occurred while confirming simulation");
            }
        }

        // POST: api/projectsites/{id}/revoke-simulation
        [HttpPost("{id}/revoke-simulation")]
        public async Task<IActionResult> RevokeSimulation(int id)
        {
            try
            {
                var success = await _projectSiteService.RevokeSimulationAsync(id);
                if (!success)
                {
                    return NotFound($"Project site with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while revoking simulation for project site {ProjectSiteId}", id);
                return StatusCode(500, "Internal server error occurred while revoking simulation");
            }
        }
    }
} 