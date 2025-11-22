using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces.MachineManagement;
using Application.DTOs.MachineManagement;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/usage-logs")]
    [AllowAnonymous] // Temporarily allow anonymous access for testing
    public class UsageLogsController : BaseApiController
    {
        private readonly IUsageLogService _usageLogService;
        private readonly ILogger<UsageLogsController> _logger;

        public UsageLogsController(
            IUsageLogService usageLogService,
            ILogger<UsageLogsController> logger)
        {
            _usageLogService = usageLogService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new usage log (Operator)
        /// </summary>
        [HttpPost]
        // [Authorize(Roles = "Operator")] // Uncomment when authentication is ready
        public async Task<IActionResult> CreateUsageLog([FromBody] CreateUsageLogRequest request)
        {
            try
            {
                // Get operator ID from claims (or use default for testing)
                var operatorId = User.FindFirstValue(ClaimTypes.NameIdentifier) != null
                    ? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                    : 1; // Default for testing

                var result = await _usageLogService.CreateUsageLogAsync(request, operatorId);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }

                return Created($"/api/usage-logs/{result.Value.Id}", result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating usage log");
                return InternalServerError("An error occurred while creating the usage log");
            }
        }

        /// <summary>
        /// Get usage log by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsageLog(int id)
        {
            try
            {
                var result = await _usageLogService.GetUsageLogByIdAsync(id);

                if (!result.IsSuccess)
                {
                    return NotFound(result.Error);
                }

                return base.Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving usage log {LogId}", id);
                return InternalServerError("An error occurred while retrieving the usage log");
            }
        }

        /// <summary>
        /// Get usage logs for a specific machine
        /// </summary>
        [HttpGet("machine/{machineId}")]
        public async Task<IActionResult> GetUsageLogsByMachine(
            int machineId,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var result = await _usageLogService.GetUsageLogsByMachineAsync(machineId, startDate, endDate);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }

                return base.Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving usage logs for machine {MachineId}", machineId);
                return InternalServerError("An error occurred while retrieving usage logs");
            }
        }

        /// <summary>
        /// Get latest usage log for a machine
        /// </summary>
        [HttpGet("machine/{machineId}/latest")]
        public async Task<IActionResult> GetLatestUsageLog(int machineId)
        {
            try
            {
                var result = await _usageLogService.GetLatestUsageLogByMachineAsync(machineId);

                if (!result.IsSuccess)
                {
                    return NotFound(result.Error);
                }

                return base.Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest usage log for machine {MachineId}", machineId);
                return InternalServerError("An error occurred while retrieving the latest usage log");
            }
        }

        /// <summary>
        /// Get usage statistics for a machine
        /// </summary>
        [HttpGet("machine/{machineId}/statistics")]
        public async Task<IActionResult> GetUsageStatistics(
            int machineId,
            [FromQuery] int days = 30)
        {
            try
            {
                var result = await _usageLogService.GetUsageStatisticsAsync(machineId, days);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }

                return base.Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating usage statistics for machine {MachineId}", machineId);
                return InternalServerError("An error occurred while calculating usage statistics");
            }
        }

        /// <summary>
        /// Get usage logs by operator
        /// </summary>
        [HttpGet("operator/{operatorId}")]
        public async Task<IActionResult> GetUsageLogsByOperator(
            int operatorId,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var result = await _usageLogService.GetUsageLogsByOperatorAsync(operatorId, startDate, endDate);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }

                return base.Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving usage logs for operator {OperatorId}", operatorId);
                return InternalServerError("An error occurred while retrieving usage logs");
            }
        }

        /// <summary>
        /// Approve a usage log (Machine Manager)
        /// </summary>
        [HttpPatch("{id}/approve")]
        // [Authorize(Roles = "MachineManager")] // Uncomment when authentication is ready
        public async Task<IActionResult> ApproveUsageLog(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) != null
                    ? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                    : 1; // Default for testing

                var result = await _usageLogService.ApproveUsageLogAsync(id, userId);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }

                return base.Ok(new { message = "Usage log approved successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving usage log {LogId}", id);
                return InternalServerError("An error occurred while approving the usage log");
            }
        }

        /// <summary>
        /// Reject a usage log (Machine Manager)
        /// </summary>
        [HttpPatch("{id}/reject")]
        // [Authorize(Roles = "MachineManager")] // Uncomment when authentication is ready
        public async Task<IActionResult> RejectUsageLog(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) != null
                    ? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                    : 1; // Default for testing

                var result = await _usageLogService.RejectUsageLogAsync(id, userId);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }

                return base.Ok(new { message = "Usage log rejected successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting usage log {LogId}", id);
                return InternalServerError("An error occurred while rejecting the usage log");
            }
        }
    }
}
