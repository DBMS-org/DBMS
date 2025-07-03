using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.DTOs.UserManagement;
using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireAdminRole")]
    public class UserAssignmentsController : BaseApiController
    {
        private readonly ApplicationDbContext _context;

        public UserAssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAssignments()
        {
            var userAssignments = await _context.Users
                .Select(u => new UserAssignmentDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    Region = u.Region,
                    Status = u.Status.ToString(),
                    AssignedProjects = _context.Projects
                        .Where(p => p.AssignedUserId == u.Id)
                        .Select(p => new UserProjectAssignmentDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Status = p.Status.ToString(),
                            Region = p.Region
                        })
                        .ToList()
                })
                .ToListAsync();

            return Ok(userAssignments);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserAssignment(int userId)
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
                Status = user.Status.ToString(),
                AssignedProjects = await _context.Projects
                    .Where(p => p.AssignedUserId == userId)
                    .Select(p => new UserProjectAssignmentDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Status = p.Status.ToString(),
                        Region = p.Region
                    })
                    .ToListAsync()
            };

            return Ok(userAssignment);
        }

        [HttpGet("{userId}/projects")]
        public async Task<IActionResult> GetUserProjects(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return NotFound($"User with ID {userId} not found");
            }

            var projects = await _context.Projects
                .Include(p => p.AssignedUser)
                .Include(p => p.ProjectSites)
                .Where(p => p.AssignedUserId == userId)
                .ToListAsync();

            return Ok(projects);
        }

        [HttpPost("{userId}/assign-project")]
        public async Task<IActionResult> AssignProjectToUser(int userId, [FromBody] AssignProjectRequest request)
        {
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
            
            if (project.AssignedUserId.HasValue && project.AssignedUserId != userId)
            {
                return Conflict($"Project is already assigned to another user");
            }
            
            project.AssignedUserId = userId;
            project.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{userId}/unassign-project/{projectId}")]
        public async Task<IActionResult> UnassignProjectFromUser(int userId, int projectId)
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
            
            project.AssignedUserId = null;
            project.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetUserAssignmentStatistics()
        {
            var totalUsers = await _context.Users.CountAsync();
            var activeUsers = await _context.Users.CountAsync(u => u.Status == UserStatus.Active);
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
    }
} 