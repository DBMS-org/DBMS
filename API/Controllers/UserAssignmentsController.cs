using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAssignmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserAssignmentsController> _logger;

        public UserAssignmentsController(
            ApplicationDbContext context, 
            ILogger<UserAssignmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/userassignments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAssignmentDto>>> GetUserAssignments()
        {
            try
            {
                var userAssignments = await _context.Users
                    .Select(u => new UserAssignmentDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        Role = u.Role,
                        Region = u.Region,
                        Status = u.Status,
                        AssignedProjects = _context.Projects
                                                    .Where(p => p.AssignedUserId == u.Id)
                        .Include(p => p.Region)
                        .Select(p => new UserProjectAssignmentDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Status = p.Status,
                            RegionId = p.RegionId,
                            RegionName = p.Region.Name
                        })
                            .ToList()
                    })
                    .ToListAsync();

                return Ok(userAssignments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user assignments");
                return StatusCode(500, "Internal server error occurred while fetching user assignments");
            }
        }

        // GET: api/userassignments/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserAssignmentDto>> GetUserAssignment(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound($"User with ID {userId} not found");
                }

                var userAssignment = new UserAssignmentDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    Region = user.Region,
                    Status = user.Status,
                    AssignedProjects = await _context.Projects
                        .Where(p => p.AssignedUserId == userId)
                        .Include(p => p.Region)
                        .Select(p => new UserProjectAssignmentDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Status = p.Status,
                            RegionId = p.RegionId,
                            RegionName = p.Region.Name
                        })
                        .ToListAsync()
                };

                return Ok(userAssignment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user assignment for user {UserId}", userId);
                return StatusCode(500, "Internal server error occurred while fetching user assignment");
            }
        }

        // GET: api/userassignments/{userId}/projects
        [HttpGet("{userId}/projects")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetUserProjects(int userId)
        {
            try
            {
                var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
                if (!userExists)
                {
                    return NotFound($"User with ID {userId} not found");
                }

                var projects = await _context.Projects
                    .Include(p => p.AssignedUser)
                    .Include(p => p.ProjectSites)
                    .Include(p => p.Region)
                    .Where(p => p.AssignedUserId == userId)
                    .Select(p => new ProjectDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        RegionId = p.RegionId,
                        RegionName = p.Region.Name,
                        Status = p.Status,
                        Description = p.Description,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        AssignedUserId = p.AssignedUserId,
                        AssignedUserName = p.AssignedUser != null ? p.AssignedUser.Name : null,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,
                        ProjectSites = p.ProjectSites.Select(ps => new ProjectSiteDto
                        {
                            Id = ps.Id,
                            ProjectId = ps.ProjectId,
                            Name = ps.Name,
                            Location = ps.Location,
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
                _logger.LogError(ex, "Error occurred while fetching projects for user {UserId}", userId);
                return StatusCode(500, "Internal server error occurred while fetching user projects");
            }
        }

        // POST: api/userassignments/{userId}/assign-project
        [HttpPost("{userId}/assign-project")]
        public async Task<IActionResult> AssignProjectToUser(int userId, [FromBody] AssignProjectRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound($"User with ID {userId} not found");
                }

                var project = await _context.Projects.FindAsync(request.ProjectId);
                if (project == null)
                {
                    return NotFound($"Project with ID {request.ProjectId} not found");
                }

                // Check if project is already assigned to another user
                if (project.AssignedUserId.HasValue && project.AssignedUserId != userId)
                {
                    return BadRequest($"Project is already assigned to another user");
                }

                // Assign project to user
                project.AssignedUserId = userId;
                project.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = $"Project '{project.Name}' assigned to user '{user.Name}' successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while assigning project {ProjectId} to user {UserId}", request.ProjectId, userId);
                return StatusCode(500, "Internal server error occurred while assigning project");
            }
        }

        // DELETE: api/userassignments/{userId}/unassign-project/{projectId}
        [HttpDelete("{userId}/unassign-project/{projectId}")]
        public async Task<IActionResult> UnassignProjectFromUser(int userId, int projectId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound($"User with ID {userId} not found");
                }

                var project = await _context.Projects.FindAsync(projectId);
                if (project == null)
                {
                    return NotFound($"Project with ID {projectId} not found");
                }

                if (project.AssignedUserId != userId)
                {
                    return BadRequest($"Project is not assigned to this user");
                }

                // Unassign project from user
                project.AssignedUserId = null;
                project.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = $"Project '{project.Name}' unassigned from user '{user.Name}' successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while unassigning project {ProjectId} from user {UserId}", projectId, userId);
                return StatusCode(500, "Internal server error occurred while unassigning project");
            }
        }

        // GET: api/userassignments/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<UserAssignmentStatisticsDto>> GetUserAssignmentStatistics()
        {
            try
            {
                var totalUsers = await _context.Users.CountAsync();
                var activeUsers = await _context.Users.CountAsync(u => u.Status == "Active");
                var usersWithProjects = await _context.Users
                    .CountAsync(u => _context.Projects.Any(p => p.AssignedUserId == u.Id));
                var totalProjects = await _context.Projects.CountAsync();
                var assignedProjects = await _context.Projects.CountAsync(p => p.AssignedUserId.HasValue);

                var statistics = new UserAssignmentStatisticsDto
                {
                    TotalUsers = totalUsers,
                    ActiveUsers = activeUsers,
                    UsersWithProjects = usersWithProjects,
                    TotalProjects = totalProjects,
                    AssignedProjects = assignedProjects,
                    UnassignedProjects = totalProjects - assignedProjects
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user assignment statistics");
                return StatusCode(500, "Internal server error occurred while fetching statistics");
            }
        }
    }
} 