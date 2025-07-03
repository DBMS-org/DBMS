using Microsoft.AspNetCore.Mvc;
using Application.DTOs.ProjectManagement;
using Application.DTOs.Shared;
using Domain.Entities.ProjectManagement;
using Application.Interfaces.ProjectManagement;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(
            IProjectService projectService, 
            ILogger<ProjectsController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        // GET: api/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var result = await _projectService.GetAllProjectsAsync();
            
            if (result.IsFailure)
            {
                _logger.LogError("Error occurred while fetching projects: {Error}", result.Error);
                return StatusCode(500, result.Error);
            }

            return Ok(result.Value);
        }

        // GET: api/projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project?>> GetProject(int id)
        {
            var result = await _projectService.GetProjectByIdAsync(id);
            
            if (result.IsFailure)
            {
                if (result.Error.Contains("not found"))
                {
                    return NotFound(result.Error);
                }
                
                _logger.LogError("Error occurred while fetching project with ID {ProjectId}: {Error}", id, result.Error);
                return StatusCode(500, result.Error);
            }

            return Ok(result.Value);
        }

        // POST: api/projects
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(CreateProjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _projectService.CreateProjectAsync(request);
            
            if (result.IsFailure)
            {
                if (result.Error.Contains("not found"))
                {
                    return BadRequest(result.Error);
                }
                
                _logger.LogError("Error occurred while creating project: {Error}", result.Error);
                return StatusCode(500, result.Error);
            }

            return CreatedAtAction(nameof(GetProject), new { id = result.Value.Id }, result.Value);
        }

        // PUT: api/projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, UpdateProjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _projectService.UpdateProjectAsync(id, request);
            
            if (result.IsFailure)
            {
                if (result.Error.Contains("not found"))
                {
                    return NotFound(result.Error);
                }
                
                _logger.LogError("Error occurred while updating project with ID {ProjectId}: {Error}", id, result.Error);
                return StatusCode(500, result.Error);
            }

            return NoContent();
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var result = await _projectService.DeleteProjectAsync(id);
            
            if (result.IsFailure)
            {
                if (result.Error.Contains("not found"))
                {
                    return NotFound(result.Error);
                }
                
                _logger.LogError("Error occurred while deleting project with ID {ProjectId}: {Error}", id, result.Error);
                return StatusCode(500, result.Error);
            }

            return NoContent();
        }

        // GET: api/projects/5/sites
        [HttpGet("{id}/sites")]
        public async Task<ActionResult<IEnumerable<ProjectSite>>> GetProjectSites(int id)
        {
            var result = await _projectService.GetProjectSitesAsync(id);
            
            if (result.IsFailure)
            {
                _logger.LogError("Error occurred while fetching project sites for project {ProjectId}: {Error}", id, result.Error);
                return StatusCode(500, result.Error);
            }

            return Ok(result.Value);
        }

        // GET: api/projects/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Project>>> SearchProjects(
            [FromQuery] string? name = null,
            [FromQuery] string? region = null,
            [FromQuery] string? status = null)
        {
            var result = await _projectService.SearchProjectsAsync(name, region, status);
            
            if (result.IsFailure)
            {
                _logger.LogError("Error occurred while searching projects: {Error}", result.Error);
                return StatusCode(500, result.Error);
            }

            return Ok(result.Value);
        }

        // GET: api/projects/test-connection
        [HttpGet("test-connection")]
        public ActionResult TestConnection()
        {
            try
            {
                return Ok(new { message = "Connection successful", timestamp = DateTime.UtcNow });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while testing connection");
                return StatusCode(500, "Internal server error occurred while testing connection");
            }
        }

        // GET: api/projects/by-operator/5
        [HttpGet("by-operator/{operatorId}")]
        public async Task<ActionResult<Project?>> GetProjectByOperator(int operatorId)
        {
            var result = await _projectService.GetProjectByOperatorAsync(operatorId);
            
            if (result.IsFailure)
            {
                if (result.Error.Contains("not found"))
                {
                    return NotFound(result.Error);
                }
                
                _logger.LogError("Error occurred while fetching project for operator {OperatorId}: {Error}", operatorId, result.Error);
                return StatusCode(500, result.Error);
            }

            return Ok(result.Value);
        }
    }
} 