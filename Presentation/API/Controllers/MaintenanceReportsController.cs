using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces.MaintenanceOperations;
using Application.DTOs.MaintenanceOperations;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/maintenance-reports")]
    [Authorize]
    public class MaintenanceReportsController : BaseApiController
    {
        private readonly IMaintenanceReportService _reportService;
        private readonly ILogger<MaintenanceReportsController> _logger;

        public MaintenanceReportsController(
            IMaintenanceReportService reportService,
            ILogger<MaintenanceReportsController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        // GET: api/maintenance-reports/operator/{operatorId}/machine
        [HttpGet("operator/{operatorId}/machine")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<IActionResult> GetOperatorMachine(int operatorId)
        {
            var userId = GetCurrentUserId();
            if (userId != operatorId && !User.IsInRole("Admin") && !User.IsInRole("MechanicalEngineer"))
            {
                return Forbid();
            }

            var result = await _reportService.GetOperatorMachineAsync(operatorId);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to get machine for operator {OperatorId}: {Error}", operatorId, result.Error);
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        // GET: api/maintenance-reports/operator/{operatorId}
        [HttpGet("operator/{operatorId}")]
        [Authorize(Roles = "Operator,Admin,MechanicalEngineer")]
        public async Task<IActionResult> GetOperatorReports(int operatorId)
        {
            var userId = GetCurrentUserId();
            if (userId != operatorId && !User.IsInRole("Admin") && !User.IsInRole("MechanicalEngineer"))
            {
                return Forbid();
            }

            var result = await _reportService.GetOperatorReportsAsync(operatorId);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get reports for operator {OperatorId}: {Error}", operatorId, result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        // POST: api/maintenance-reports/submit
        [HttpPost("submit")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<IActionResult> SubmitReport([FromBody] SubmitMaintenanceReportRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            if (request.OperatorId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var result = await _reportService.SubmitReportAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to submit maintenance report: {Error}", result.Error);
                return BadRequest(result.Error);
            }

            _logger.LogInformation("Maintenance report submitted successfully: {TicketId}", result.Value?.TicketId);
            return CreatedAtAction(nameof(GetReportById), new { id = result.Value?.Id }, result.Value);
        }

        // GET: api/maintenance-reports/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Operator,Admin,MechanicalEngineer")]
        public async Task<IActionResult> GetReportById(int id)
        {
            var result = await _reportService.GetReportByIdAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to get report {ReportId}: {Error}", id, result.Error);
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        // GET: api/maintenance-reports/ticket/{ticketId}
        [HttpGet("ticket/{ticketId}")]
        [Authorize(Roles = "Operator,Admin,MechanicalEngineer")]
        public async Task<IActionResult> GetReportByTicketId(string ticketId)
        {
            var result = await _reportService.GetReportByTicketIdAsync(ticketId);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to get report by ticket {TicketId}: {Error}", ticketId, result.Error);
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        // PATCH: api/maintenance-reports/{id}/status
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,MechanicalEngineer")]
        public async Task<IActionResult> UpdateReportStatus(int id, [FromBody] UpdateReportStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _reportService.UpdateReportStatusAsync(id, request);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to update report {ReportId} status: {Error}", id, result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        // POST: api/maintenance-reports/{id}/close
        [HttpPost("{id}/close")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<IActionResult> CloseReport(int id)
        {
            var userId = GetCurrentUserId();

            var result = await _reportService.CloseReportAsync(id, userId);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to close report {ReportId}: {Error}", id, result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        // POST: api/maintenance-reports/{id}/reopen
        [HttpPost("{id}/reopen")]
        [Authorize(Roles = "Operator,Admin")]
        public async Task<IActionResult> ReopenReport(int id, [FromBody] ReopenReportRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _reportService.ReopenReportAsync(id, request.Reason);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to reopen report {ReportId}: {Error}", id, result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        // GET: api/maintenance-reports/operator/{operatorId}/summary
        [HttpGet("operator/{operatorId}/summary")]
        [Authorize(Roles = "Operator,Admin,MechanicalEngineer")]
        public async Task<IActionResult> GetReportSummary(int operatorId)
        {
            var userId = GetCurrentUserId();
            if (userId != operatorId && !User.IsInRole("Admin") && !User.IsInRole("MechanicalEngineer"))
            {
                return Forbid();
            }

            var result = await _reportService.GetOperatorSummaryAsync(operatorId);
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get summary for operator {OperatorId}: {Error}", operatorId, result.Error);
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

    public class ReopenReportRequest
    {
        public string Reason { get; set; } = string.Empty;
    }
}
