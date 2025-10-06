using Microsoft.AspNetCore.Mvc;
using Application.DTOs.ExplosiveInventory;
using Application.Interfaces.ExplosiveInventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Domain.Entities.StoreManagement.Enums;
using Domain.Entities.ExplosiveInventory.Enums;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CentralInventoryController : BaseApiController
    {
        private readonly ICentralInventoryService _inventoryService;
        private readonly ILogger<CentralInventoryController> _logger;

        public CentralInventoryController(
            ICentralInventoryService inventoryService,
            ILogger<CentralInventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated list of central warehouse inventory
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "ReadInventoryData")]
        public async Task<IActionResult> GetInventory(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] ExplosiveType? explosiveType = null,
            [FromQuery] InventoryStatus? status = null,
            [FromQuery] string? supplier = null,
            [FromQuery] string? batchId = null,
            [FromQuery] bool? isExpiringSoon = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool sortDescending = false)
        {
            var filter = new InventoryFilterDto
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                ExplosiveType = explosiveType,
                Status = status,
                Supplier = supplier,
                BatchId = batchId,
                IsExpiringSoon = isExpiringSoon,
                SortBy = sortBy,
                SortDescending = sortDescending
            };

            var result = await _inventoryService.GetAllInventoryAsync(filter);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get inventory item by ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetInventoryById(int id)
        {
            var result = await _inventoryService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get inventory item by batch ID
        /// </summary>
        [HttpGet("batch/{batchId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetInventoryByBatchId(string batchId)
        {
            var result = await _inventoryService.GetByBatchIdAsync(batchId);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get inventory by explosive type
        /// </summary>
        [HttpGet("type/{type}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetInventoryByType(ExplosiveType type)
        {
            var result = await _inventoryService.GetByExplosiveTypeAsync(type);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get dashboard statistics
        /// </summary>
        [HttpGet("dashboard")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDashboardData()
        {
            var result = await _inventoryService.GetDashboardDataAsync();

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get expiring batches within specified days
        /// </summary>
        [HttpGet("expiring")]
        [Authorize(Policy = "ReadInventoryData")]
        public async Task<IActionResult> GetExpiringBatches([FromQuery] int daysThreshold = 30)
        {
            var result = await _inventoryService.GetExpiringBatchesAsync(daysThreshold);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get expired batches
        /// </summary>
        [HttpGet("expired")]
        [Authorize(Policy = "ReadInventoryData")]
        public async Task<IActionResult> GetExpiredBatches()
        {
            var result = await _inventoryService.GetExpiredBatchesAsync();

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Create new ANFO batch inventory
        /// </summary>
        [HttpPost("anfo")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateANFOBatch([FromBody] CreateANFOInventoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _inventoryService.CreateANFOBatchAsync(request);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(
                nameof(GetInventoryById),
                new { id = result.Value.Id },
                result.Value);
        }

        /// <summary>
        /// Update ANFO batch inventory
        /// </summary>
        [HttpPut("anfo/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateANFOBatch(int id, [FromBody] UpdateANFOInventoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _inventoryService.UpdateANFOBatchAsync(id, request);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Create new Emulsion batch inventory
        /// </summary>
        [HttpPost("emulsion")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateEmulsionBatch([FromBody] CreateEmulsionInventoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _inventoryService.CreateEmulsionBatchAsync(request);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(
                nameof(GetInventoryById),
                new { id = result.Value.Id },
                result.Value);
        }

        /// <summary>
        /// Update Emulsion batch inventory
        /// </summary>
        [HttpPut("emulsion/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateEmulsionBatch(int id, [FromBody] UpdateEmulsionInventoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _inventoryService.UpdateEmulsionBatchAsync(id, request);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Quarantine inventory batch
        /// </summary>
        [HttpPost("{id}/quarantine")]
        [Authorize(Policy = "ManageInventory")]
        public async Task<IActionResult> QuarantineBatch(int id, [FromBody] QuarantineBatchRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _inventoryService.QuarantineBatchAsync(id, request.Reason);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        /// <summary>
        /// Release batch from quarantine
        /// </summary>
        [HttpPost("{id}/release")]
        [Authorize(Policy = "ManageInventory")]
        public async Task<IActionResult> ReleaseFromQuarantine(int id)
        {
            var result = await _inventoryService.ReleaseFromQuarantineAsync(id);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        /// <summary>
        /// Mark batch as expired
        /// </summary>
        [HttpPost("{id}/expire")]
        [Authorize(Policy = "ManageInventory")]
        public async Task<IActionResult> MarkAsExpired(int id)
        {
            var result = await _inventoryService.MarkAsExpiredAsync(id);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        /// <summary>
        /// Update storage location
        /// </summary>
        [HttpPatch("{id}/storage-location")]
        [Authorize(Policy = "ManageInventory")]
        public async Task<IActionResult> UpdateStorageLocation(int id, [FromBody] UpdateStorageLocationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _inventoryService.UpdateStorageLocationAsync(id, request.NewLocation);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        /// <summary>
        /// Delete inventory batch
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var result = await _inventoryService.DeleteBatchAsync(id);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok();
        }
    }

    /// <summary>
    /// Request model for quarantine operation
    /// </summary>
    public class QuarantineBatchRequest
    {
        public string Reason { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model for updating storage location
    /// </summary>
    public class UpdateStorageLocationRequest
    {
        public string NewLocation { get; set; } = string.Empty;
    }
}
