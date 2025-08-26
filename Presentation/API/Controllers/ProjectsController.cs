using Microsoft.AspNetCore.Mvc;
using Application.DTOs.ProjectManagement;
using Application.DTOs.Shared;
using Domain.Entities.ProjectManagement;
using Application.Interfaces.ProjectManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : BaseApiController
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "ReadProjectData")]
        public async Task<IActionResult> GetProjects()
        {
            var result = await _projectService.GetAllProjectsAsync();
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ReadProjectData")]
        public async Task<IActionResult> GetProject(int id)
        {
            var result = await _projectService.GetProjectByIdAsync(id);
            if (result.IsFailure)
            {
                return NotFound();
            }
            return Ok(result.Value);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CreateProject(CreateProjectRequest request)
        {
            var result = await _projectService.CreateProjectAsync(request);
            if (result.IsFailure)
                {
                    return BadRequest(result.Error);
            }
            return CreatedAtAction(nameof(GetProject), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, UpdateProjectRequest request)
        {
            var projectResult = await _projectService.GetProjectByIdAsync(id);
            if (projectResult.IsFailure)
            {
                return NotFound();
            }

            var project = projectResult.Value;
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (project.OwningUserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var result = await _projectService.UpdateProjectAsync(id, request);
            if (result.IsFailure)
            {
                return NotFound();
                }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var projectResult = await _projectService.GetProjectByIdAsync(id);
            if (projectResult.IsFailure)
            {
                return NotFound();
            }

            var project = projectResult.Value;
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (project.OwningUserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var result = await _projectService.DeleteProjectAsync(id);
            if (result.IsFailure)
            {
                return NotFound();
                }
            return Ok();
        }

        [HttpGet("{id}/sites")]
        public async Task<IActionResult> GetProjectSites(int id)
        {
            var result = await _projectService.GetProjectSitesAsync(id);
            return Ok(result.Value);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProjects(
            [FromQuery] string? name = null,
            [FromQuery] string? region = null,
            [FromQuery] string? status = null)
        {
            var result = await _projectService.SearchProjectsAsync(name, region, status);
            return Ok(result.Value);
        }

        [HttpGet("by-operator/{operatorId}")]
        [Authorize(Policy = "ReadProjectData")]
        public async Task<IActionResult> GetProjectByOperator(int operatorId)
        {
            // Verify the user has permission to access this operator's data
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (userId != operatorId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var result = await _projectService.GetProjectByOperatorAsync(operatorId);
            if (result.IsFailure)
            {
                _logger.LogWarning("No project found for operator {OperatorId}", operatorId);
                return NotFound($"No project found for operator {operatorId}");
            }
            return Ok(result.Value);
        }
    }
} 