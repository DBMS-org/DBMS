using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.DrillingOperations;
using Application.DTOs.DrillingOperations;
using Application.DTOs.Shared;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrillPointPatternController : ControllerBase
    {
        private readonly IDrillPointPatternService _drillPointPatternService;
        private readonly ILogger<DrillPointPatternController> _logger;

        public DrillPointPatternController(
            IDrillPointPatternService drillPointPatternService, 
            ILogger<DrillPointPatternController> logger)
        {
            _drillPointPatternService = drillPointPatternService;
            _logger = logger;
        }

        [HttpPost("drill-points")]
        public async Task<ActionResult<DrillPointDto>> CreateDrillPoint([FromBody] CreateDrillPointRequest request)
        {
            try
            {
                var result = await _drillPointPatternService.CreateDrillPointAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating drill point");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("drill-points/{pointId}/position")]
        public async Task<ActionResult<bool>> UpdateDrillPointPosition(string pointId, [FromBody] UpdateDrillPointPositionRequest request)
        {
            try
            {
                request.PointId = pointId;
                var result = await _drillPointPatternService.UpdateDrillPointPositionAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating drill point position");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("drill-points/{pointId}")]
        public async Task<ActionResult<bool>> RemoveDrillPoint(string pointId, [FromQuery] int projectId, [FromQuery] int siteId)
        {
            try
            {
                var result = await _drillPointPatternService.RemoveDrillPointAsync(pointId, projectId, siteId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing drill point");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("drill-points")]
        public async Task<ActionResult<List<DrillPointDto>>> GetDrillPoints([FromQuery] int projectId, [FromQuery] int siteId)
        {
            try
            {
                var result = await _drillPointPatternService.GetDrillPointsAsync(projectId, siteId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill points");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("drill-points")]
        public async Task<ActionResult<bool>> ClearAllDrillPoints([FromQuery] int projectId, [FromQuery] int siteId)
        {
            try
            {
                var result = await _drillPointPatternService.ClearAllDrillPointsAsync(projectId, siteId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing drill points");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("pattern-data")]
        public async Task<ActionResult<PatternDataDto>> GetPatternData([FromQuery] int projectId, [FromQuery] int siteId)
        {
            try
            {
                var result = await _drillPointPatternService.GetPatternDataAsync(projectId, siteId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pattern data");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("save-pattern")]
        public async Task<ActionResult<SavePatternResult>> SavePattern([FromBody] SavePatternRequest request)
        {
            try
            {
                var result = await _drillPointPatternService.SavePatternAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving pattern");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("pattern-settings")]
        public async Task<ActionResult<PatternSettingsDto>> GetPatternSettings([FromQuery] int projectId, [FromQuery] int siteId)
        {
            try
            {
                var result = await _drillPointPatternService.GetPatternSettingsAsync(projectId, siteId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pattern settings");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("pattern-settings")]
        public async Task<ActionResult> UpdatePatternSettings([FromQuery] int projectId, [FromQuery] int siteId, [FromBody] PatternSettingsDto settings)
        {
            try
            {
                await _drillPointPatternService.UpdatePatternSettingsAsync(projectId, siteId, settings);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating pattern settings");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("process-csv")]
        public async Task<ActionResult<PatternDataDto>> ProcessUploadedCsvData([FromBody] ProcessCsvDataRequest request)
        {
            try
            {
                var result = await _drillPointPatternService.ProcessUploadedCsvDataAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing CSV data");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("calculate-grid-pitch")]
        public async Task<ActionResult<object>> CalculateGridPitch([FromQuery] int projectId, [FromQuery] int siteId)
        {
            try
            {
                var result = await _drillPointPatternService.CalculateGridPitchAsync(projectId, siteId);
                return Ok(new { spacing = result.spacing, burden = result.burden });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating grid pitch");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("export-for-blast-designer")]
        public async Task<ActionResult<PatternDataDto>> ExportForBlastDesigner([FromQuery] int projectId, [FromQuery] int siteId)
        {
            try
            {
                var result = await _drillPointPatternService.ExportPatternForBlastDesignerAsync(projectId, siteId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting for blast designer");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("validate-coordinates")]
        public async Task<ActionResult<bool>> ValidateCoordinates([FromBody] ValidateCoordinatesRequest request)
        {
            try
            {
                var result = await _drillPointPatternService.ValidateCoordinatesAsync(request.X, request.Y);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating coordinates");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("validate-unique-coordinates")]
        public async Task<ActionResult<bool>> ValidateUniqueCoordinates(
            [FromQuery] double x, 
            [FromQuery] double y, 
            [FromQuery] int projectId, 
            [FromQuery] int siteId, 
            [FromQuery] string? excludePointId = null)
        {
            try
            {
                var result = await _drillPointPatternService.ValidateUniqueCoordinatesAsync(x, y, projectId, siteId, excludePointId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating unique coordinates");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("validate-drill-point-count")]
        public async Task<ActionResult<bool>> ValidateDrillPointCount([FromQuery] int projectId, [FromQuery] int siteId, [FromQuery] int maxPoints)
        {
            try
            {
                var result = await _drillPointPatternService.ValidateDrillPointCountAsync(projectId, siteId, maxPoints);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating drill point count");
                return BadRequest(ex.Message);
            }
        }
    }

    // Request DTOs for validation endpoints
    public class ValidateCoordinatesRequest
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
} 