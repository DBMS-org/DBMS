using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;

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
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            try
            {
                var projects = await _projectService.GetAllProjectsAsync();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching projects");
                return StatusCode(500, "Internal server error occurred while fetching projects");
            }
        }

        // GET: api/projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(int id)
        {
            try
            {
                var project = await _projectService.GetProjectByIdAsync(id);
                if (project == null)
                {
                    return NotFound($"Project with ID {id} not found");
                }

                return Ok(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching project with ID {ProjectId}", id);
                return StatusCode(500, "Internal server error occurred while fetching project");
            }
        }

        // POST: api/projects
        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var project = await _projectService.CreateProjectAsync(request);
                return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating project");
                return StatusCode(500, "Internal server error occurred while creating project");
            }
        }

        // PUT: api/projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, UpdateProjectRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var success = await _projectService.UpdateProjectAsync(id, request);
                if (!success)
                {
                    return NotFound($"Project with ID {id} not found");
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating project with ID {ProjectId}", id);
                return StatusCode(500, "Internal server error occurred while updating project");
            }
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                var success = await _projectService.DeleteProjectAsync(id);
                if (!success)
                {
                    return NotFound($"Project with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting project with ID {ProjectId}", id);
                return StatusCode(500, "Internal server error occurred while deleting project");
            }
        }

        // GET: api/projects/5/sites
        [HttpGet("{id}/sites")]
        public async Task<ActionResult<IEnumerable<ProjectSiteDto>>> GetProjectSites(int id)
        {
            try
            {
                var projectSites = await _projectService.GetProjectSitesAsync(id);
                return Ok(projectSites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching project sites for project {ProjectId}", id);
                return StatusCode(500, "Internal server error occurred while fetching project sites");
            }
        }

        // GET: api/projects/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> SearchProjects(
            [FromQuery] string? name = null,
            [FromQuery] string? region = null,
            [FromQuery] string? status = null)
        {
            try
            {
                var projects = await _projectService.SearchProjectsAsync(name, region, status);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching projects");
                return StatusCode(500, "Internal server error occurred while searching projects");
            }
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
        public async Task<ActionResult<ProjectDto?>> GetProjectByOperator(int operatorId)
        {
            try
            {
                var project = await _projectService.GetProjectByOperatorAsync(operatorId);
                return Ok(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching project for operator {OperatorId}", operatorId);
                return StatusCode(500, "Internal server error occurred while fetching project");
            }
        }
    }
} 