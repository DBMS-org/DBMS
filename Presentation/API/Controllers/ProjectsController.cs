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
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(
            ApplicationDbContext context, 
            ILogger<ProjectsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            try
            {
                var projects = await _context.Projects
                    .Include(p => p.AssignedUser)
                    .Include(p => p.Region)
                    .Include(p => p.ProjectSites)
                    .Select(p => new ProjectDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Status = p.Status,
                        Description = p.Description,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        AssignedUserId = p.AssignedUserId,
                        RegionId = p.RegionId,
                        AssignedUserName = p.AssignedUser != null ? p.AssignedUser.Name : null,
                        RegionName = p.Region != null ? p.Region.Name : null,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,
                        ProjectSites = p.ProjectSites.Select(ps => new ProjectSiteDto
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
                        }).ToList()
                    })
                    .ToListAsync();

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
                var project = await _context.Projects
                    .Include(p => p.AssignedUser)
                    .Include(p => p.Region)
                    .Include(p => p.ProjectSites)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (project == null)
                {
                    return NotFound($"Project with ID {id} not found");
                }

                var projectDto = new ProjectDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Status = project.Status,
                    Description = project.Description,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    AssignedUserId = project.AssignedUserId,
                    RegionId = project.RegionId,
                    AssignedUserName = project.AssignedUser?.Name,
                    RegionName = project.Region?.Name,
                    CreatedAt = project.CreatedAt,
                    UpdatedAt = project.UpdatedAt,
                    ProjectSites = project.ProjectSites.Select(ps => new ProjectSiteDto
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
                    }).ToList()
                };

                return Ok(projectDto);
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

                // Validate assigned user exists if provided
                if (request.AssignedUserId.HasValue)
                {
                    var userExists = await _context.Users.AnyAsync(u => u.Id == request.AssignedUserId.Value);
                    if (!userExists)
                    {
                        return BadRequest($"User with ID {request.AssignedUserId.Value} not found");
                    }
                }

                // If the operator is already assigned to a different project, unassign first
                if (request.AssignedUserId.HasValue)
                {
                    var existing = await _context.Projects.FirstOrDefaultAsync(p => p.AssignedUserId == request.AssignedUserId);
                    if (existing != null)
                    {
                        existing.AssignedUserId = null;
                    }
                }

                var project = new Project
                {
                    Name = request.Name,
                    RegionId = request.RegionId,
                    Status = request.Status,
                    Description = request.Description,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    AssignedUserId = request.AssignedUserId
                };

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                // Load the project with related data for response
                var createdProject = await _context.Projects
                    .Include(p => p.AssignedUser)
                    .Include(p => p.Region)
                    .Include(p => p.ProjectSites)
                    .FirstOrDefaultAsync(p => p.Id == project.Id);

                var projectDto = new ProjectDto
                {
                    Id = createdProject!.Id,
                    Name = createdProject.Name,
                    Status = createdProject.Status,
                    Description = createdProject.Description,
                    StartDate = createdProject.StartDate,
                    EndDate = createdProject.EndDate,
                    AssignedUserId = createdProject.AssignedUserId,
                    RegionId = createdProject.RegionId,
                    AssignedUserName = createdProject.AssignedUser?.Name,
                    RegionName = createdProject.Region?.Name,
                    CreatedAt = createdProject.CreatedAt,
                    UpdatedAt = createdProject.UpdatedAt,
                    ProjectSites = new List<ProjectSiteDto>()
                };

                return CreatedAtAction(nameof(GetProject), new { id = project.Id }, projectDto);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while creating project");
                return StatusCode(500, "Database error occurred while creating project");
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
                if (id != request.Id)
                {
                    return BadRequest("Project ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    return NotFound($"Project with ID {id} not found");
                }

                // Validate assigned user exists if provided
                if (request.AssignedUserId.HasValue)
                {
                    var userExists = await _context.Users.AnyAsync(u => u.Id == request.AssignedUserId.Value);
                    if (!userExists)
                    {
                        return BadRequest($"User with ID {request.AssignedUserId.Value} not found");
                    }
                }

                // If operator reassigned, unassign from previous project
                if (request.AssignedUserId.HasValue)
                {
                    var previous = await _context.Projects.FirstOrDefaultAsync(p => p.AssignedUserId == request.AssignedUserId && p.Id != id);
                    if (previous != null)
                    {
                        previous.AssignedUserId = null;
                    }
                }

                // Update project properties
                project.Name = request.Name;
                project.RegionId = request.RegionId;
                project.Status = request.Status;
                project.Description = request.Description;
                project.StartDate = request.StartDate;
                project.EndDate = request.EndDate;
                project.AssignedUserId = request.AssignedUserId;
                project.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound($"Project with ID {id} not found");
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error occurred while updating project {ProjectId}", id);
                return StatusCode(500, "Database error occurred while updating project");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating project {ProjectId}", id);
                return StatusCode(500, "Internal server error occurred while updating project");
            }
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    return NotFound($"Project with ID {id} not found");
                }

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting project {ProjectId}", id);
                return StatusCode(500, "Internal server error occurred while deleting project");
            }
        }

        // GET: api/projects/{id}/sites
        [HttpGet("{id}/sites")]
        public async Task<ActionResult<IEnumerable<ProjectSiteDto>>> GetProjectSites(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    return NotFound($"Project with ID {id} not found");
                }

                var sites = await _context.ProjectSites
                    .Where(ps => ps.ProjectId == id)
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

                return Ok(sites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching sites for project {ProjectId}", id);
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
                var query = _context.Projects
                    .Include(p => p.AssignedUser)
                    .Include(p => p.Region)
                    .Include(p => p.ProjectSites)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Name.Contains(name));
                }

                if (!string.IsNullOrEmpty(region))
                {
                    query = query.Where(p => p.Region.Name.Contains(region));
                }

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(p => p.Status == status);
                }

                var projects = await query
                    .Select(p => new ProjectDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Status = p.Status,
                        Description = p.Description,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        AssignedUserId = p.AssignedUserId,
                        RegionId = p.RegionId,
                        AssignedUserName = p.AssignedUser != null ? p.AssignedUser.Name : null,
                        RegionName = p.Region != null ? p.Region.Name : null,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,
                        ProjectSites = p.ProjectSites.Select(ps => new ProjectSiteDto
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
                        }).ToList()
                    })
                    .ToListAsync();

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
        public async Task<ActionResult> TestConnection()
        {
            try
            {
                var count = await _context.Projects.CountAsync();
                return Ok(new { message = "Database connection successful", projectCount = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database connection test failed");
                return StatusCode(500, "Database connection failed");
            }
        }

        // GET: api/projects/by-operator/5
        [HttpGet("by-operator/{operatorId}")]
        public async Task<ActionResult<ProjectDto?>> GetProjectByOperator(int operatorId)
        {
            var project = await _context.Projects
                .Include(p => p.AssignedUser)
                .Include(p => p.Region)
                .FirstOrDefaultAsync(p => p.AssignedUserId == operatorId);

            if (project == null)
            {
                return Ok(null); // No project assigned
            }

            var dto = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Status = project.Status,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                AssignedUserId = project.AssignedUserId,
                RegionId = project.RegionId,
                AssignedUserName = project.AssignedUser?.Name,
                RegionName = project.Region?.Name,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
                ProjectSites = new List<ProjectSiteDto>()
            };

            return Ok(dto);
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
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
    }
} 
