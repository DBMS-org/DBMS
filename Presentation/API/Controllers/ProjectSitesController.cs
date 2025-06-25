using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities;
using Application.DTOs;
using System.Text.Json;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectSitesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectSitesController> _logger;

        public ProjectSitesController(
            ApplicationDbContext context, 
            ILogger<ProjectSitesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/projectsites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectSiteDto>>> GetProjectSites()
        {
            try
            {
                var projectSites = await _context.ProjectSites
                    .Select(ps => new ProjectSiteDto
                    {
                        Id = ps.Id,
                        ProjectId = ps.ProjectId,
                        Name = ps.Name,
                        Location = ps.Location,
                        Coordinates = ParseCoordinates(ps.Coordinates),
                        Status = ps.Status,
                        Description = ps.Description,
                        CreatedAt = ps.CreatedAt,
                        UpdatedAt = ps.UpdatedAt,
                        IsPatternApproved = ps.IsPatternApproved,
                        IsSimulationConfirmed = ps.IsSimulationConfirmed,
                        IsOperatorCompleted = ps.IsOperatorCompleted
                    })
                    .ToListAsync();

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
        public async Task<ActionResult<ProjectSiteDto>> GetProjectSite(int id)
        {
            try
            {
                var projectSite = await _context.ProjectSites.FindAsync(id);

                if (projectSite == null)
                {
                    return NotFound($"Project site with ID {id} not found");
                }

                var projectSiteDto = new ProjectSiteDto
                {
                    Id = projectSite.Id,
                    ProjectId = projectSite.ProjectId,
                    Name = projectSite.Name,
                    Location = projectSite.Location,
                    Coordinates = ParseCoordinates(projectSite.Coordinates),
                    Status = projectSite.Status,
                    Description = projectSite.Description,
                    CreatedAt = projectSite.CreatedAt,
                    UpdatedAt = projectSite.UpdatedAt,
                    IsPatternApproved = projectSite.IsPatternApproved,
                    IsSimulationConfirmed = projectSite.IsSimulationConfirmed,
                    IsOperatorCompleted = projectSite.IsOperatorCompleted
                };

                return Ok(projectSiteDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching project site with ID {ProjectSiteId}", id);
                return StatusCode(500, "Internal server error occurred while fetching project site");
            }
        }

        // GET: api/projectsites/project/5
        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<ProjectSiteDto>>> GetProjectSitesByProject(int projectId)
        {
            try
            {
                var projectExists = await _context.Projects.AnyAsync(p => p.Id == projectId);
                if (!projectExists)
                {
                    return NotFound($"Project with ID {projectId} not found");
                }

                var projectSites = await _context.ProjectSites
                    .Where(ps => ps.ProjectId == projectId)
                    .Select(ps => new ProjectSiteDto
                    {
                        Id = ps.Id,
                        ProjectId = ps.ProjectId,
                        Name = ps.Name,
                        Location = ps.Location,
                        Coordinates = ParseCoordinates(ps.Coordinates),
                        Status = ps.Status,
                        Description = ps.Description,
                        CreatedAt = ps.CreatedAt,
                        UpdatedAt = ps.UpdatedAt,
                        IsPatternApproved = ps.IsPatternApproved,
                        IsSimulationConfirmed = ps.IsSimulationConfirmed,
                        IsOperatorCompleted = ps.IsOperatorCompleted
                    })
                    .ToListAsync();

                return Ok(projectSites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching project sites for project {ProjectId}", projectId);
                return StatusCode(500, "Internal server error occurred while fetching project sites");
            }
        }

        // POST: api/projectsites
        [HttpPost]
        public async Task<ActionResult<ProjectSiteDto>> CreateProjectSite(CreateProjectSiteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Validate that the project exists
                var projectExists = await _context.Projects.AnyAsync(p => p.Id == request.ProjectId);
                if (!projectExists)
                {
                    return BadRequest($"Project with ID {request.ProjectId} not found");
                }

                var projectSite = new ProjectSite
                {
                    ProjectId = request.ProjectId,
                    Name = request.Name,
                    Location = request.Location,
                    Coordinates = SerializeCoordinates(request.Coordinates),
                    Status = request.Status,
                    Description = request.Description
                };

                _context.ProjectSites.Add(projectSite);
                await _context.SaveChangesAsync();

                var projectSiteDto = new ProjectSiteDto
                {
                    Id = projectSite.Id,
                    ProjectId = projectSite.ProjectId,
                    Name = projectSite.Name,
                    Location = projectSite.Location,
                    Coordinates = ParseCoordinates(projectSite.Coordinates),
                    Status = projectSite.Status,
                    Description = projectSite.Description,
                    CreatedAt = projectSite.CreatedAt,
                    UpdatedAt = projectSite.UpdatedAt,
                    IsPatternApproved = projectSite.IsPatternApproved,
                    IsSimulationConfirmed = projectSite.IsSimulationConfirmed,
                    IsOperatorCompleted = projectSite.IsOperatorCompleted
                };

                return CreatedAtAction(nameof(GetProjectSite), new { id = projectSite.Id }, projectSiteDto);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while creating project site");
                return StatusCode(500, "Database error occurred while creating project site");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating project site");
                return StatusCode(500, "Internal server error occurred while creating project site");
            }
        }

        // PUT: api/projectsites/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectSite(int id, ProjectSiteDto request)
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

                var projectSite = await _context.ProjectSites.FindAsync(id);
                if (projectSite == null)
                {
                    return NotFound($"Project site with ID {id} not found");
                }

                // Validate that the project exists
                var projectExists = await _context.Projects.AnyAsync(p => p.Id == request.ProjectId);
                if (!projectExists)
                {
                    return BadRequest($"Project with ID {request.ProjectId} not found");
                }

                // Update project site properties
                projectSite.ProjectId = request.ProjectId;
                projectSite.Name = request.Name;
                projectSite.Location = request.Location;
                projectSite.Coordinates = SerializeCoordinates(request.Coordinates);
                projectSite.Status = request.Status;
                projectSite.Description = request.Description;
                projectSite.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectSiteExists(id))
                {
                    return NotFound($"Project site with ID {id} not found");
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while updating project site {ProjectSiteId}", id);
                return StatusCode(500, "Database error occurred while updating project site");
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
                var projectSite = await _context.ProjectSites.FindAsync(id);
                if (projectSite == null)
                {
                    return NotFound($"Project site with ID {id} not found");
                }

                _context.ProjectSites.Remove(projectSite);
                await _context.SaveChangesAsync();

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
            var site = await _context.ProjectSites.FindAsync(id);
            if (site == null) return NotFound();
            site.IsPatternApproved = true;
            site.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/projectsites/{id}/revoke
        [HttpPost("{id}/revoke")]
        public async Task<IActionResult> RevokePattern(int id)
        {
            var site = await _context.ProjectSites.FindAsync(id);
            if (site == null) return NotFound();
            site.IsPatternApproved = false;
            site.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/projectsites/{id}/confirm-simulation
        [HttpPost("{id}/confirm-simulation")]
        public async Task<IActionResult> ConfirmSimulation(int id)
        {
            var site = await _context.ProjectSites.FindAsync(id);
            if (site == null) return NotFound();
            site.IsSimulationConfirmed = true;
            site.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/projectsites/{id}/revoke-simulation
        [HttpPost("{id}/revoke-simulation")]
        public async Task<IActionResult> RevokeSimulation(int id)
        {
            var site = await _context.ProjectSites.FindAsync(id);
            if (site == null) return NotFound();
            site.IsSimulationConfirmed = false;
            site.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ProjectSiteExists(int id)
        {
            return _context.ProjectSites.Any(e => e.Id == id);
        }

        private static CoordinatesDto? ParseCoordinates(string coordinates)
        {
            if (string.IsNullOrEmpty(coordinates))
                return null;

            try
            {
                // Try to parse as JSON first (for structured coordinates)
                if (coordinates.StartsWith("{") && coordinates.EndsWith("}"))
                {
                    return JsonSerializer.Deserialize<CoordinatesDto>(coordinates);
                }

                // Try to parse as comma-separated lat,lng
                var parts = coordinates.Split(',');
                if (parts.Length == 2 &&
                    double.TryParse(parts[0].Trim(), out var lat) &&
                    double.TryParse(parts[1].Trim(), out var lng))
                {
                    return new CoordinatesDto { Latitude = lat, Longitude = lng };
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private static string SerializeCoordinates(CoordinatesDto? coordinates)
        {
            if (coordinates == null)
                return string.Empty;

            try
            {
                return JsonSerializer.Serialize(coordinates);
            }
            catch
            {
                return $"{coordinates.Latitude},{coordinates.Longitude}";
            }
        }
    }
} 
