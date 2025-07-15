using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.DrillingOperations;
using Domain.Entities.DrillingOperations;
using Application.DTOs.DrillingOperations;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [Authorize(Policy = "ReadDrillData")]
        public async Task<IActionResult> GetAllDrillHoles()
        {
            var result = await _drillHoleService.GetAllDrillHolesDtoAsync();
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ReadDrillData")]
        public async Task<IActionResult> GetDrillHole(string id)
        {
            var result = await _drillHoleService.GetDrillHoleDtoByIdAsync(id);
            if (result.IsFailure)
            {
                return NotFound();
            }
            return Ok(result.Value);
        }

        [HttpPost("upload-csv")]
        [Authorize(Policy = "ManageDrillData")]
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
        [Authorize(Policy = "ManageDrillData")]
        public async Task<IActionResult> CreateDrillHole(CreateDrillHoleRequest request)
        {
            var result = await _drillHoleService.CreateDrillHoleFromDtoAsync(request);
            if (result.IsFailure)
            {
                return Conflict(result.Error);
            }
            return CreatedAtAction(nameof(GetDrillHole), new { id = result.Value.Id }, result.Value);
        }

        [HttpPost("projects/{projectId:int}/sites/{siteId:int}")]
        [Authorize(Policy = "ManageDrillData")]
        public async Task<IActionResult> CreateDrillHoleForSite(int projectId, int siteId, CreateDrillHoleRequest request)
        {
            // Set the project and site IDs from the route parameters
            request.ProjectId = projectId;
            request.SiteId = siteId;
            
            var result = await _drillHoleService.CreateDrillHoleFromDtoAsync(request);
            if (result.IsFailure)
            {
                return Conflict(result.Error);
            }
            return CreatedAtAction(nameof(GetDrillHole), new { id = result.Value.Id }, result.Value);
        }

        [HttpPost("projects/{projectId:int}/sites/{siteId:int}/batch")]
        [Authorize(Policy = "ManageDrillData")]
        public async Task<IActionResult> CreateMultipleDrillHolesForSite(int projectId, int siteId, List<CreateDrillHoleRequest> requests)
        {
            // Set the project and site IDs for all requests
            foreach (var request in requests)
            {
                request.ProjectId = projectId;
                request.SiteId = siteId;
            }
            
            var results = new List<object>();
            var errors = new List<string>();
            
            foreach (var request in requests)
            {
                var result = await _drillHoleService.CreateDrillHoleFromDtoAsync(request);
                if (result.IsFailure)
                {
                    errors.Add($"Failed to create drill hole '{request.Name}': {result.Error}");
                }
                else
                {
                    results.Add(result.Value);
                }
            }
            
            if (errors.Any())
            {
                return BadRequest(new { 
                    message = "Some drill holes failed to create", 
                    errors = errors,
                    successful = results.Count,
                    failed = errors.Count
                });
            }
            
            return Ok(new { 
                message = $"Successfully created {results.Count} drill holes",
                data = results 
            });
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "ManageDrillData")]
        public async Task<IActionResult> UpdateDrillHole(string id, UpdateDrillHoleRequest request)
        {
            var result = await _drillHoleService.UpdateDrillHoleFromDtoAsync(id, request);
            if (result.IsFailure)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("site/{projectId:int}/{siteId:int}")]
        [Authorize(Policy = "ManageDrillData")]
        public async Task<IActionResult> DeleteDrillHolesBySite(int projectId, int siteId)
        {
            var result = await _drillHoleService.DeleteDrillHolesBySiteIdAsync(projectId, siteId);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "ManageDrillData")]
        public async Task<IActionResult> DeleteDrillHole(string id)
        {
            var result = await _drillHoleService.DeleteDrillHoleAsync(id);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }
    }
}