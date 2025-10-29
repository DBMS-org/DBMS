using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities.MachineManagement;
using Domain.Entities.UserManagement;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace API.Controllers
{
    [ApiController]
    [Route("api/maintenance-reports")]
    [Authorize]
    public class MaintenanceReportsController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MaintenanceReportsController> _logger;

        public MaintenanceReportsController(ApplicationDbContext context, ILogger<MaintenanceReportsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/maintenance-reports/operator/{operatorId}/machine
        [HttpGet("operator/{operatorId}/machine")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOperatorMachine(int operatorId)
        {
            // Authorization check temporarily disabled for testing
            // var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            // if (userId != operatorId && !User.IsInRole("Admin") && !User.IsInRole("MechanicalEngineer"))
            // {
            //     return Forbid();
            // }

            var machine = await _context.Machines
                .Include(m => m.Region)
                .FirstOrDefaultAsync(m => m.OperatorId == operatorId);

            if (machine == null)
            {
                _logger.LogInformation("No machine found assigned to operator {OperatorId}", operatorId);
                return NotFound($"No machine assigned to operator {operatorId}");
            }

            var operatorMachine = new
            {
                id = machine.Id.ToString(),
                name = machine.Name,
                model = machine.Model,
                serialNumber = machine.SerialNumber,
                currentLocation = machine.CurrentLocation ?? machine.Region?.Name ?? "Unknown",
                status = machine.Status.ToString(),
                assignedOperatorId = machine.OperatorId.ToString()
            };

            return Ok(operatorMachine);
        }

        // GET: api/maintenance-reports/operator/{operatorId}
        [HttpGet("operator/{operatorId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOperatorReports(int operatorId)
        {
            // Authorization check temporarily disabled for testing
            // var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            // if (userId != operatorId && !User.IsInRole("Admin") && !User.IsInRole("MechanicalEngineer"))
            // {
            //     return Forbid();
            // }

            // In a real implementation, this would retrieve maintenance reports from a database
            // For now, we'll return a mock empty list
            var reports = new List<object>();

            return Ok(reports);
        }

        // POST: api/maintenance-reports/submit
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitReport([FromBody] object reportData)
        {
            // In a real implementation, this would validate and save the report to a database
            // For now, we'll just return a mock response
            var mockReport = new
            {
                id = Guid.NewGuid().ToString(),
                ticketId = $"MR-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(1000, 9999)}",
                reportedAt = DateTime.UtcNow,
                status = "REPORTED"
                // Other fields would be populated from the request
            };

            return Ok(mockReport);
        }

        // GET: api/maintenance-reports/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(string id)
        {
            // In a real implementation, this would retrieve a specific report from a database
            return NotFound($"Report with ID {id} not found");
        }

        // PATCH: api/maintenance-reports/{id}/status
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,MechanicalEngineer")]
        public async Task<IActionResult> UpdateReportStatus(string id, [FromBody] object statusUpdate)
        {
            // In a real implementation, this would update a report's status in a database
            return NotFound($"Report with ID {id} not found");
        }

        // GET: api/maintenance-reports/operator/{operatorId}/summary
        [HttpGet("operator/{operatorId}/summary")]
        public async Task<IActionResult> GetReportSummary(int operatorId)
        {
            // Verify the user has permission to access this operator's data
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (userId != operatorId && !User.IsInRole("Admin") && !User.IsInRole("MechanicalEngineer"))
            {
                return Forbid();
            }

            // In a real implementation, this would calculate summary statistics from a database
            var summary = new
            {
                totalReports = 0,
                reportedToday = 0,
                inProgressReports = 0,
                resolvedThisWeek = 0,
                averageResponseTime = "N/A"
            };

            return Ok(summary);
        }
        
        // POST: api/maintenance-reports/test/operator/{operatorId}
        [HttpPost("test/operator/{operatorId}")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateTestReports(int operatorId)
        {
            // Verify operator exists
            var operatorExists = await _context.Users.AnyAsync(u => u.Id == operatorId);
            if (!operatorExists)
            {
                return BadRequest($"Operator with ID {operatorId} not found");
            }

            // Find assigned machine
            var machine = await _context.Machines
                .FirstOrDefaultAsync(m => m.OperatorId == operatorId);

            if (machine == null)
            {
                return BadRequest($"No machine found for operator {operatorId}. Please assign a machine first.");
            }

            // Create dummy reports
            var reports = new List<object>();
            var statuses = new[] { "REPORTED", "ACKNOWLEDGED", "IN_PROGRESS", "RESOLVED", "CLOSED" };
            var categories = new[] { "ENGINE_ISSUES", "HYDRAULIC_PROBLEMS", "ELECTRICAL_FAULTS", "MECHANICAL_BREAKDOWN", "OTHER" };
            var severity = new[] { "LOW", "MEDIUM", "HIGH", "CRITICAL" };
            
            for (int i = 0; i < 5; i++)
            {
                var randomStatus = statuses[new Random().Next(statuses.Length)];
                var randomCategory = categories[new Random().Next(categories.Length)];
                var randomSeverity = severity[new Random().Next(severity.Length)];
                
                var report = new
                {
                    id = Guid.NewGuid().ToString(),
                    ticketId = $"MR-{DateTime.UtcNow:yyyyMMdd}-{1000 + i}",
                    machineId = machine.Id.ToString(),
                    operatorId = operatorId.ToString(),
                    machineName = machine.Name,
                    problemCategory = randomCategory,
                    customDescription = $"Test report {i+1} for {randomCategory.ToLower().Replace('_', ' ')}",
                    severity = randomSeverity,
                    reportedAt = DateTime.UtcNow.AddDays(-i),
                    status = randomStatus,
                    lastUpdatedAt = DateTime.UtcNow.AddHours(-i),
                    location = machine.CurrentLocation
                };
                
                reports.Add(report);
            }
            
            return Ok(reports);
        }
    }
}
