using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.DrillingOperations;
using Domain.Entities.DrillingOperations;
using Application.DTOs.DrillingOperations;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    public class DrillPlanController : BaseApiController
    {
        private readonly IDrillHoleService _drillHoleService;
        private readonly ICsvImportService _csvImportService;

        public DrillPlanController(
            IDrillHoleService drillHoleService,
            ICsvImportService csvImportService)
        {
            _drillHoleService = drillHoleService;
            _csvImportService = csvImportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrillHoles()
        {
            var result = await _drillHoleService.GetAllDrillHolesAsync();
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDrillHole(string id)
        {
            var result = await _drillHoleService.GetDrillHoleByIdAsync(id);
            if (result.IsFailure)
            {
                return NotFound();
            }
            return Ok(result.Value);
        }

        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadAndParseCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file or an empty file was provided.");
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (fileExtension != ".csv")
            {
                return BadRequest($"Only CSV files are allowed. Received file type: {fileExtension}");
            }
            
            var csvRequest = new CsvUploadRequest
            {
                FileStream = file.OpenReadStream(),
                FileName = file.FileName,
                FileSize = file.Length
            };

            var result = await _csvImportService.CreateDrillHolesFromCsvAsync(csvRequest);
            
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            
            if (!result.Value.Any())
            {
                return BadRequest("No valid drill holes found in the CSV file. Please check the file format.");
            }
            
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDrillHole(DrillHole drillHole)
        {
            var result = await _drillHoleService.CreateDrillHoleAsync(drillHole);
            if (result.IsFailure)
            {
                return Conflict(result.Error);
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
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrillHole(string id)
        {
            var result = await _drillHoleService.DeleteDrillHoleAsync(id);
            if (result.IsFailure)
            {
                return NotFound();
            }
            return Ok();
        }
    }
} 