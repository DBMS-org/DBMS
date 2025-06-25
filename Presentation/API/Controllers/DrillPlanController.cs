using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;
using Application.DTOs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrillPlanController : ControllerBase
    {
        private readonly ILogger<DrillPlanController> _logger;
        private readonly IDrillHoleService _drillHoleService;

        public DrillPlanController(ILogger<DrillPlanController> logger, IDrillHoleService drillHoleService)
        {
            _logger = logger;
            _drillHoleService = drillHoleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DrillHole>>> GetAllDrillHoles()
        {
            var drillHoles = await _drillHoleService.GetAllDrillHolesAsync();
            return Ok(drillHoles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DrillHole>> GetDrillHole(string id)
        {
            var drillHole = await _drillHoleService.GetDrillHoleByIdAsync(id);
            if (drillHole == null)
            {
                return NotFound();
            }
            return Ok(drillHole);
        }

        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadAndParseCsv(IFormFile file)
        {
            try
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

                var drillHoles = await _drillHoleService.CreateDrillHolesFromCsvAsync(csvRequest);
                
                if (!drillHoles.Any())
                {
                    _logger.LogWarning("No valid drill holes found in the CSV");
                    return BadRequest("No valid drill holes found in the CSV file. Please check the file format.");
                }

                _logger.LogInformation("Successfully parsed {DrillHoleCount} drill holes", drillHoles.Count());
                return Ok(drillHoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing uploaded CSV file: {ErrorMessage}", ex.Message);
                return StatusCode(500, $"An error occurred while processing the CSV file: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<DrillHole>> CreateDrillHole(DrillHole drillHole)
        {
            try
            {
                var createdDrillHole = await _drillHoleService.CreateDrillHoleAsync(drillHole);
                return CreatedAtAction(nameof(GetDrillHole), new { id = createdDrillHole.Id }, createdDrillHole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating drill hole: {ErrorMessage}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDrillHole(string id, DrillHole drillHole)
        {
            if (id != drillHole.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                await _drillHoleService.UpdateDrillHoleAsync(drillHole);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating drill hole: {ErrorMessage}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrillHole(string id)
        {
            try
            {
                await _drillHoleService.DeleteDrillHoleAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting drill hole: {ErrorMessage}", ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
} 
