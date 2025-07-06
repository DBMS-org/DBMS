using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces.BlastingOperations;
using Application.DTOs.BlastingOperations;
using Domain.Entities.BlastingOperations;
using System.Security.Claims;

namespace Presentation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SiteBlastingController : ControllerBase
    {
        private readonly ISiteBlastingService _siteBlastingService;
        private readonly IBlastConnectionService _blastConnectionService;

        public SiteBlastingController(
            ISiteBlastingService siteBlastingService,
            IBlastConnectionService blastConnectionService)
        {
            _siteBlastingService = siteBlastingService;
            _blastConnectionService = blastConnectionService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        [HttpDelete("projects/{projectId}/sites/{siteId}/data")]
        public async Task<IActionResult> DeleteAllWorkflowData(int projectId, int siteId)
        {
            var hasData = await _siteBlastingService.DeleteAllWorkflowDataAsync(projectId, siteId);
            return Ok(new { success = true, hadData = hasData });
        }

        #region Blast Sequences
        
        [HttpGet("projects/{projectId}/sites/{siteId}/sequences")]
        public async Task<IActionResult> GetBlastSequences(int projectId, int siteId)
            {
            var result = await _siteBlastingService.GetBlastSequencesAsync(projectId, siteId);
            return Ok(result);
        }

        [HttpGet("sequences/{id}")]
        public async Task<IActionResult> GetBlastSequence(int id)
            {
            var result = await _siteBlastingService.GetBlastSequenceAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost("projects/{projectId}/sites/{siteId}/sequences")]
        public async Task<IActionResult> CreateBlastSequence(int projectId, int siteId, [FromBody] CreateBlastSequenceRequest request)
        {
                request.ProjectId = projectId;
                request.SiteId = siteId;
            
                var userId = GetCurrentUserId();
            var result = await _siteBlastingService.CreateBlastSequenceAsync(request, userId);
            return CreatedAtAction(nameof(GetBlastSequence), new { id = result.Id }, result);
        }

        [HttpPut("sequences/{id}")]
        public async Task<IActionResult> UpdateBlastSequence(int id, [FromBody] UpdateBlastSequenceRequest request)
        {
                var userId = GetCurrentUserId();
            var result = await _siteBlastingService.UpdateBlastSequenceAsync(id, request, userId);
            return Ok(result);
        }

        [HttpDelete("sequences/{id}")]
        public async Task<IActionResult> DeleteBlastSequence(int id)
        {
            var success = await _siteBlastingService.DeleteBlastSequenceAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }

        #endregion

        #region Blast Connections
        
        [HttpGet("projects/{projectId}/sites/{siteId}/connections")]
        public async Task<IActionResult> GetBlastConnections(int projectId, int siteId)
        {
            var result = await _blastConnectionService.GetConnectionsByProjectAndSiteAsync(projectId, siteId);
            
            if (!result.IsSuccess)
                return BadRequest(result.Error);
            
            // Map entities to DTOs
            var connectionDtos = result.Value.Select(MapToDto).ToList();
            return Ok(connectionDtos);
        }

        [HttpGet("projects/{projectId}/sites/{siteId}/connections/{id}")]
        public async Task<IActionResult> GetBlastConnection(string id, int projectId, int siteId)
        {
            var result = await _blastConnectionService.GetConnectionByIdAsync(id, projectId, siteId);
            
            if (!result.IsSuccess)
                return NotFound(result.Error);
            
            var connectionDto = MapToDto(result.Value);
            return Ok(connectionDto);
        }

        [HttpGet("projects/{projectId}/sites/{siteId}/connections/sequence/{sequence}")]
        public async Task<IActionResult> GetBlastConnectionsBySequence(int projectId, int siteId, int sequence)
        {
            var result = await _blastConnectionService.GetConnectionsBySequenceAsync(projectId, siteId, sequence);
            
            if (!result.IsSuccess)
                return BadRequest(result.Error);
            
            var connectionDtos = result.Value.Select(MapToDto).ToList();
            return Ok(connectionDtos);
        }

        [HttpPost("projects/{projectId}/sites/{siteId}/connections")]
        public async Task<IActionResult> CreateBlastConnection(int projectId, int siteId, [FromBody] CreateBlastConnectionRequest request)
        {
            request.ProjectId = projectId;
            request.SiteId = siteId;
            
            var blastConnection = new BlastConnection
            {
                Id = request.Id,
                Point1DrillPointId = request.Point1DrillPointId,
                Point2DrillPointId = request.Point2DrillPointId,
                ConnectorType = request.ConnectorType,
                Delay = request.Delay,
                Sequence = request.Sequence,
                ProjectId = request.ProjectId,
                SiteId = request.SiteId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            var result = await _blastConnectionService.CreateConnectionAsync(blastConnection);
            
            if (!result.IsSuccess)
                return BadRequest(result.Error);
            
            var connectionDto = MapToDto(result.Value);
            return CreatedAtAction(nameof(GetBlastConnection), 
                new { id = result.Value.Id, projectId = projectId, siteId = siteId }, 
                connectionDto);
        }

        [HttpPut("projects/{projectId}/sites/{siteId}/connections/{id}")]
        public async Task<IActionResult> UpdateBlastConnection(string id, int projectId, int siteId, [FromBody] UpdateBlastConnectionRequest request)
        {
            request.Id = id;
            request.ProjectId = projectId;
            request.SiteId = siteId;
            
            var result = await _blastConnectionService.GetConnectionByIdAsync(id, projectId, siteId);
            
            if (!result.IsSuccess)
                return NotFound(result.Error);
            
            var entity = result.Value;

            // Update entity properties
            entity.Point1DrillPointId = request.Point1DrillPointId;
            entity.Point2DrillPointId = request.Point2DrillPointId;
            entity.ConnectorType = request.ConnectorType;
            entity.Delay = request.Delay;
            entity.Sequence = request.Sequence;
            entity.ProjectId = request.ProjectId;
            entity.SiteId = request.SiteId;
            entity.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _blastConnectionService.UpdateConnectionAsync(entity);
            
            if (!updateResult.IsSuccess)
                return BadRequest(updateResult.Error);
            
            var connectionDto = MapToDto(entity);
            return Ok(connectionDto);
        }

        [HttpDelete("projects/{projectId}/sites/{siteId}/connections/{id}")]
        public async Task<IActionResult> DeleteBlastConnection(string id, int projectId, int siteId)
        {
            var result = await _blastConnectionService.DeleteConnectionAsync(id, projectId, siteId);
            
            if (!result.IsSuccess)
                return NotFound(result.Error);
                
            return NoContent();
        }

        #endregion

        #region Helper Methods

        private static BlastConnectionDto MapToDto(BlastConnection entity)
        {
            return new BlastConnectionDto
            {
                Id = entity.Id,
                Point1DrillPointId = entity.Point1DrillPointId,
                Point2DrillPointId = entity.Point2DrillPointId,
                ConnectorType = entity.ConnectorType,
                Delay = entity.Delay,
                Sequence = entity.Sequence,
                ProjectId = entity.ProjectId,
                SiteId = entity.SiteId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Point1DrillPoint = entity.Point1DrillPoint != null ? new DrillPointDto
                {
                    Id = entity.Point1DrillPoint.Id,
                    X = entity.Point1DrillPoint.X,
                    Y = entity.Point1DrillPoint.Y,
                    Depth = entity.Point1DrillPoint.Depth,
                    Spacing = entity.Point1DrillPoint.Spacing,
                    Burden = entity.Point1DrillPoint.Burden,
                    ProjectId = entity.Point1DrillPoint.ProjectId,
                    SiteId = entity.Point1DrillPoint.SiteId
                } : null,
                Point2DrillPoint = entity.Point2DrillPoint != null ? new DrillPointDto
                {
                    Id = entity.Point2DrillPoint.Id,
                    X = entity.Point2DrillPoint.X,
                    Y = entity.Point2DrillPoint.Y,
                    Depth = entity.Point2DrillPoint.Depth,
                    Spacing = entity.Point2DrillPoint.Spacing,
                    Burden = entity.Point2DrillPoint.Burden,
                    ProjectId = entity.Point2DrillPoint.ProjectId,
                    SiteId = entity.Point2DrillPoint.SiteId
                } : null
            };
        }

        #endregion
    }
} 