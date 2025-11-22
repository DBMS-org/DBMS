using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces.MaintenanceOperations;
using Application.DTOs.MaintenanceOperations;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/maintenance-jobs")]
    [Authorize]
    public class MaintenanceJobsController : BaseApiController
    {
        private readonly IMaintenanceJobService _jobService;
        private readonly ILogger<MaintenanceJobsController> _logger;

        public MaintenanceJobsController(
            IMaintenanceJobService jobService,
            ILogger<MaintenanceJobsController> logger)
        {
            _jobService = jobService;
            _logger = logger;
        }

        // GET: api/maintenance-jobs/all
        [HttpGet("all")]
        [Authorize(Roles = "Admin,MachineManager")]
        public async Task<IActionResult> GetAllJobs(
            [FromQuery] string? status = null,
            [FromQuery] string? type = null,
            [FromQuery] int? machineId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _jobService.GetAllJobsAsync(status, type, machineId, startDate, endDate);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get all jobs: {Error}", result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        // GET: api/maintenance-jobs/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,MechanicalEngineer,MachineManager")]
        public async Task<IActionResult> GetJobById(int id)
        {
            var result = await _jobService.GetJobByIdAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to get job {JobId}: {Error}", id, result.Error);
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        // GET: api/maintenance-jobs/engineer/{engineerId}
        [HttpGet("engineer/{engineerId}")]
        [Authorize(Roles = "Admin,MechanicalEngineer")]
        public async Task<IActionResult> GetEngineerJobs(int engineerId, [FromQuery] int? regionId = null)
        {
            var userId = GetCurrentUserId();
            if (userId != engineerId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var result = await _jobService.GetEngineerJobsAsync(engineerId, regionId);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get jobs for engineer {EngineerId}: {Error}", engineerId, result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        // GET: api/maintenance-jobs/overdue
        [HttpGet("overdue")]
        [Authorize(Roles = "Admin,MechanicalEngineer")]
        public async Task<IActionResult> GetOverdueJobs([FromQuery] int? regionId = null)
        {
            var result = await _jobService.GetOverdueJobsAsync(regionId);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get overdue jobs: {Error}", result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        // GET: api/maintenance-jobs/machine/{machineId}
        [HttpGet("machine/{machineId}")]
        [Authorize(Roles = "Admin,MechanicalEngineer,Operator")]
        public async Task<IActionResult> GetJobsByMachine(int machineId)
        {
            var result = await _jobService.GetJobsByMachineAsync(machineId);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get jobs for machine {MachineId}: {Error}", machineId, result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        // POST: api/maintenance-jobs/create
        [HttpPost("create")]
        [Authorize(Roles = "Admin,MechanicalEngineer")]
        public async Task<IActionResult> CreateManualJob([FromBody] CreateManualJobRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();

            var result = await _jobService.CreateManualJobAsync(request, userId);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to create manual job: {Error}", result.Error);
                return BadRequest(result.Error);
            }

            _logger.LogInformation("Manual maintenance job created: {JobId}", result.Value?.Id);
            return CreatedAtAction(nameof(GetJobById), new { id = result.Value?.Id }, result.Value);
        }

        // POST: api/maintenance-jobs/bulk-assign
        [HttpPost("bulk-assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BulkAssignEngineer([FromBody] BulkAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _jobService.BulkAssignEngineerAsync(request.JobIds, request.EngineerId);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to bulk assign engineer: {Error}", result.Error);
                return BadRequest(result.Error);
            }

            return Ok(new { success = result.Value, message = "Jobs assigned successfully" });
        }

        // POST: api/maintenance-jobs/{id}/complete
        [HttpPost("{id}/complete")]
        [Authorize(Roles = "MechanicalEngineer,Admin")]
        public async Task<IActionResult> CompleteJob(int id, [FromBody] CompleteMaintenanceJobRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _jobService.CompleteJobAsync(id, request);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to complete job {JobId}: {Error}", id, result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        // POST: api/maintenance-jobs/bulk-update-status
        [HttpPost("bulk-update-status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BulkUpdateStatus([FromBody] BulkUpdateStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _jobService.BulkUpdateStatusAsync(request.JobIds, request.Status);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to bulk update status: {Error}", result.Error);
                return BadRequest(result.Error);
            }

            return Ok(new { success = result.Value, message = "Job statuses updated successfully" });
        }

        // PATCH: api/maintenance-jobs/{id}/status
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,MechanicalEngineer")]
        public async Task<IActionResult> UpdateJobStatus(int id, [FromBody] UpdateJobStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _jobService.UpdateJobStatusAsync(id, request);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to update job {JobId} status: {Error}", id, result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        // GET: api/maintenance-jobs/stats/region/{regionId}
        [HttpGet("stats/region/{regionId}")]
        [Authorize(Roles = "Admin,MechanicalEngineer")]
        public async Task<IActionResult> GetMaintenanceStats(int regionId)
        {
            var result = await _jobService.GetMaintenanceStatsAsync(regionId);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get maintenance stats for region {RegionId}: {Error}", regionId, result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedAccessException("User ID not found in claims");
            }
            return userId;
        }
    }

    // Request DTOs
    public class AssignEngineerRequest
    {
        public int EngineerId { get; set; }
    }

    public class BulkAssignRequest
    {
        public List<int> JobIds { get; set; } = new();
        public int EngineerId { get; set; }
    }

    public class BulkUpdateStatusRequest
    {
        public List<int> JobIds { get; set; } = new();
        public string Status { get; set; } = string.Empty;
    }
}
