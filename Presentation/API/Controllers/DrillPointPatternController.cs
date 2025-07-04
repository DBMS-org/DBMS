using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.DrillingOperations;
using Application.DTOs.DrillingOperations;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireAdminRole")]
    public class DrillPointPatternController : BaseApiController
    {
        private readonly IDrillPointPatternService _drillPointPatternService;

        public DrillPointPatternController(IDrillPointPatternService drillPointPatternService)
        {
            _drillPointPatternService = drillPointPatternService;
        }

        [HttpPost("drill-points")]
        public async Task<IActionResult> CreateDrillPoint([FromBody] CreateDrillPointRequest request)
            {
                var result = await _drillPointPatternService.CreateDrillPointAsync(request);
                return Ok(result);
        }

        [HttpPut("drill-points/{pointId}/position")]
        public async Task<IActionResult> UpdateDrillPointPosition(string pointId, [FromBody] UpdateDrillPointPositionRequest request)
            {
                request.PointId = pointId;
                var result = await _drillPointPatternService.UpdateDrillPointPositionAsync(request);
                return Ok(result);
        }

        [HttpDelete("drill-points/{pointId}")]
        public async Task<IActionResult> RemoveDrillPoint(string pointId, [FromQuery] int projectId, [FromQuery] int siteId)
            {
                var result = await _drillPointPatternService.RemoveDrillPointAsync(pointId, projectId, siteId);
                return Ok(result);
        }

        [HttpGet("drill-points")]
        public async Task<IActionResult> GetDrillPoints([FromQuery] int projectId, [FromQuery] int siteId)
            {
                var result = await _drillPointPatternService.GetDrillPointsAsync(projectId, siteId);
                return Ok(result);
        }

        [HttpDelete("drill-points")]
        public async Task<IActionResult> ClearAllDrillPoints([FromQuery] int projectId, [FromQuery] int siteId)
            {
                var result = await _drillPointPatternService.ClearAllDrillPointsAsync(projectId, siteId);
                return Ok(result);
        }

        [HttpGet("pattern-data")]
        public async Task<IActionResult> GetPatternData([FromQuery] int projectId, [FromQuery] int siteId)
            {
                var result = await _drillPointPatternService.GetPatternDataAsync(projectId, siteId);
                return Ok(result);
        }

        [HttpPost("save-pattern")]
        public async Task<IActionResult> SavePattern([FromBody] SavePatternRequest request)
            {
                var result = await _drillPointPatternService.SavePatternAsync(request);
                return Ok(result);
        }

        [HttpGet("pattern-settings")]
        public async Task<IActionResult> GetPatternSettings([FromQuery] int projectId, [FromQuery] int siteId)
            {
                var result = await _drillPointPatternService.GetPatternSettingsAsync(projectId, siteId);
                return Ok(result);
        }

        [HttpPut("pattern-settings")]
        public async Task<IActionResult> UpdatePatternSettings([FromQuery] int projectId, [FromQuery] int siteId, [FromBody] PatternSettingsDto settings)
            {
                await _drillPointPatternService.UpdatePatternSettingsAsync(projectId, siteId, settings);
                return Ok();
        }

        [HttpPost("process-csv")]
        public async Task<IActionResult> ProcessUploadedCsvData([FromBody] ProcessCsvDataRequest request)
            {
                var result = await _drillPointPatternService.ProcessUploadedCsvDataAsync(request);
                return Ok(result);
        }

        [HttpGet("calculate-grid-pitch")]
        public async Task<IActionResult> CalculateGridPitch([FromQuery] int projectId, [FromQuery] int siteId)
            {
                var result = await _drillPointPatternService.CalculateGridPitchAsync(projectId, siteId);
                return Ok(new { spacing = result.spacing, burden = result.burden });
        }

        [HttpGet("export-for-blast-designer")]
        public async Task<IActionResult> ExportForBlastDesigner([FromQuery] int projectId, [FromQuery] int siteId)
            {
                var result = await _drillPointPatternService.ExportPatternForBlastDesignerAsync(projectId, siteId);
                return Ok(result);
        }

        [HttpPost("validate-coordinates")]
        public async Task<IActionResult> ValidateCoordinates([FromBody] ValidateCoordinatesRequest request)
            {
                var result = await _drillPointPatternService.ValidateCoordinatesAsync(request.X, request.Y);
                return Ok(result);
        }

        [HttpGet("validate-unique-coordinates")]
        public async Task<IActionResult> ValidateUniqueCoordinates(
            [FromQuery] double x, 
            [FromQuery] double y, 
            [FromQuery] int projectId, 
            [FromQuery] int siteId, 
            [FromQuery] string? excludePointId = null)
            {
                var result = await _drillPointPatternService.ValidateUniqueCoordinatesAsync(x, y, projectId, siteId, excludePointId);
                return Ok(result);
        }

        [HttpGet("validate-drill-point-count")]
        public async Task<IActionResult> ValidateDrillPointCount([FromQuery] int projectId, [FromQuery] int siteId, [FromQuery] int maxPoints)
            {
                var result = await _drillPointPatternService.ValidateDrillPointCountAsync(projectId, siteId, maxPoints);
                return Ok(result);
            }
    }

    public class ValidateCoordinatesRequest
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
} 