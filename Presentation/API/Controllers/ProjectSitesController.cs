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

        public ProjectSitesController(IProjectSiteService projectSiteService, IAuthorizationService authorizationService)
        {
            _projectSiteService = projectSiteService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> GetProjectSites()
        {
            var projectSites = await _projectSiteService.GetAllProjectSitesAsync();
            return Ok(projectSites);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectSite(int id)
        {
            var projectSite = await _projectSiteService.GetProjectSiteByIdAsync(id);
            if (projectSite == null)
            {
                return NotFound($"Project site with ID {id} not found");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, projectSite, "RequireOwnership");
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            return Ok(projectSite);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetProjectSitesByProject(int projectId)
        {
            var projectSites = await _projectSiteService.GetProjectSitesByProjectIdAsync(projectId);
            return Ok(projectSites);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CreateProjectSite(CreateProjectSiteRequest request)
        {
            var projectSite = await _projectSiteService.CreateProjectSiteAsync(request);
            return Created(projectSite, nameof(GetProjectSite));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
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
        [Authorize(Policy = "RequireAdminRole")]
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
        [Authorize(Policy = "RequireAdminRole")]
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
        [Authorize(Policy = "RequireAdminRole")]
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
        [Authorize(Policy = "RequireAdminRole")]
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
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> RevokeSimulation(int id)
        {
            var success = await _projectSiteService.RevokeSimulationAsync(id);
            if (!success)
            {
                return NotFound($"Project site with ID {id} not found");
            }
            return Ok();
        }
    }
} 