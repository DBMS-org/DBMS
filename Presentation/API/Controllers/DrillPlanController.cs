using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.DrillingOperations;
using Domain.Entities.DrillingOperations;
using Application.DTOs.DrillingOperations;
using Application.DTOs.Shared;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrillPlanController : ControllerBase
    {
        private readonly ILogger<DrillPlanController> _logger;
        private readonly IDrillHoleService _drillHoleService;
        private readonly ICsvImportService _csvImportService;

        public DrillPlanController(
            ILogger<DrillPlanController> logger, 
            IDrillHoleService drillHoleService,
            ICsvImportService csvImportService)
        {
            _logger = logger;
            _drillHoleService = drillHoleService;
            _csvImportService = csvImportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DrillHole>>> GetAllDrillHoles()
        {
            var result = await _drillHoleService.GetAllDrillHolesAsync();
            
            if (result.IsFailure)
            {
                _logger.LogError("Error occurred while fetching drill holes: {Error}", result.Error);
                return StatusCode(500, result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DrillHole>> GetDrillHole(string id)
        {
            var result = await _drillHoleService.GetDrillHoleByIdAsync(id);
            
            if (result.IsFailure)
            {
                if (result.Error.Contains("not found"))
                {
                    return NotFound(result.Error);
                }
                
                _logger.LogError("Error occurred while fetching drill hole with ID {DrillHoleId}: {Error}", id, result.Error);
                return StatusCode(500, result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadAndParseCsv(IFormFile file)
        {
            _logger.LogInformation("Starting CSV file upload process");
            
            if (file == null)
            {
                _logger.LogWarning("File is null");
                return BadRequest("No file was provided");
            }

            if (file.Length == 0)
            {
                _logger.LogWarning("File is empty");
                return BadRequest("The uploaded file is empty");
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (fileExtension != ".csv")
            {
                _logger.LogWarning("Invalid file type uploaded: {Extension}", fileExtension);
                return BadRequest($"Only CSV files are allowed. Received file type: {fileExtension}");
            }

            // Convert IFormFile to DTO
            var csvRequest = new CsvUploadRequest
            {
                FileStream = file.OpenReadStream(),
                FileName = file.FileName,
                FileSize = file.Length
            };

            var result = await _csvImportService.CreateDrillHolesFromCsvAsync(csvRequest);
            
            if (result.IsFailure)
            {
                _logger.LogError("Error processing uploaded CSV file: {Error}", result.Error);
                return BadRequest(result.Error);
            }
            
            if (!result.Value.Any())
            {
                _logger.LogWarning("No valid drill holes found in the CSV");
                return BadRequest("No valid drill holes found in the CSV file. Please check the file format.");
            }

            _logger.LogInformation("Successfully parsed {DrillHoleCount} drill holes", result.Value.Count());
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<DrillHole>> CreateDrillHole(DrillHole drillHole)
        {
            var result = await _drillHoleService.CreateDrillHoleAsync(drillHole);
            
            if (result.IsFailure)
            {
                if (result.Error.Contains("already exists"))
                {
                    return BadRequest(result.Error);
                }
                
                _logger.LogError("Error creating drill hole: {Error}", result.Error);
                return StatusCode(500, result.Error);
            }

            return CreatedAtAction(nameof(GetDrillHole), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDrillHole(string id, DrillHole drillHole)
        {
            if (id != drillHole.Id)
            {
                return BadRequest("ID mismatch");
            }

            var result = await _drillHoleService.UpdateDrillHoleAsync(drillHole);
            
            if (result.IsFailure)
            {
                if (result.Error.Contains("not found"))
                {
                    return NotFound(result.Error);
                }
                
                _logger.LogError("Error updating drill hole: {Error}", result.Error);
                return StatusCode(500, result.Error);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrillHole(string id)
        {
            var result = await _drillHoleService.DeleteDrillHoleAsync(id);
            
            if (result.IsFailure)
            {
                if (result.Error.Contains("not found"))
                {
                    return NotFound(result.Error);
                }
                
                _logger.LogError("Error deleting drill hole: {Error}", result.Error);
                return StatusCode(500, result.Error);
            }

            return NoContent();
        }
    }
} 