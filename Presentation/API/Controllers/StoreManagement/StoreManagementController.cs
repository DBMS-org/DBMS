using Application.DTOs.StoreManagement;
using Application.Interfaces.StoreManagement;
using Domain.Entities.StoreManagement.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.StoreManagement
{
    [ApiController]
    [Route("api/stores")]
    [Authorize(Roles = "Admin,Administrator,Explosive Manager")]
    public class StoreManagementController : BaseApiController
    {
        private readonly IStoreService _storeService;
        private readonly ILogger<StoreManagementController> _logger;

        public StoreManagementController(
            IStoreService storeService,
            ILogger<StoreManagementController> logger)
        {
            _storeService = storeService;
            _logger = logger;
        }

        /// <summary>
        /// Get all stores
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllStores()
        {
            try
            {
                var result = await _storeService.GetAllStoresAsync();
                
                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all stores");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get store by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreById(int id)
        {
            try
            {
                var result = await _storeService.GetStoreByIdAsync(id);
                
                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                
                return NotFound(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting store with ID: {StoreId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Create a new store
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Administrator,Explosive Manager")]
        public async Task<IActionResult> CreateStore([FromBody] CreateStoreRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _storeService.CreateStoreAsync(request);
                
                if (result.IsSuccess)
                {
                    return CreatedAtAction(nameof(GetStoreById), new { id = result.Value!.Id }, result.Value);
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating store");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Update an existing store
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Administrator,Explosive Manager")]
        public async Task<IActionResult> UpdateStore(int id, [FromBody] UpdateStoreRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _storeService.UpdateStoreAsync(id, request);
                
                if (result.IsSuccess)
                {
                    return Ok();
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating store with ID: {StoreId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Delete a store
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            try
            {
                var result = await _storeService.DeleteStoreAsync(id);
                
                if (result.IsSuccess)
                {
                    return Ok();
                }
                
                return NotFound(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting store with ID: {StoreId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get store statistics
        /// </summary>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStoreStatistics()
        {
            try
            {
                var result = await _storeService.GetStoreStatisticsAsync();
                
                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting store statistics");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Search stores with filters
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchStores(
            [FromQuery] string? storeName = null,
            [FromQuery] string? city = null,
            [FromQuery] string? status = null)
        {
            try
            {
                var result = await _storeService.SearchStoresAsync(storeName, city, status);
                
                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching stores");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get stores by region
        /// </summary>
        [HttpGet("region/{regionId}")]
        public async Task<IActionResult> GetStoresByRegion(int regionId)
        {
            try
            {
                var result = await _storeService.GetStoresByRegionAsync(regionId);
                
                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting stores by region: {RegionId}", regionId);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get stores by project
        /// </summary>
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetStoresByProject(int projectId)
        {
            try
            {
                var result = await _storeService.GetStoresByProjectAsync(projectId);
                
                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting stores by project: {ProjectId}", projectId);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get store by manager
        /// </summary>
        [HttpGet("manager/{userId}")]
        public async Task<IActionResult> GetStoreByManager(int userId)
        {
            try
            {
                var result = await _storeService.GetStoreByManagerAsync(userId);
                
                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                
                return NotFound(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting store by manager: {UserId}", userId);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Update store status
        /// </summary>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Administrator,Explosive Manager")]
        public async Task<IActionResult> UpdateStoreStatus(int id, [FromBody] UpdateStoreStatusRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _storeService.UpdateStoreStatusAsync(id, request.Status);
                
                if (result.IsSuccess)
                {
                    return Ok();
                }
                
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating store status for ID: {StoreId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get store utilization
        /// </summary>
        [HttpGet("{id}/utilization")]
        public async Task<IActionResult> GetStoreUtilization(int id)
        {
            try
            {
                var result = await _storeService.GetStoreUtilizationAsync(id);
                
                if (result.IsSuccess)
                {
                    return Ok(new { UtilizationPercentage = result.Value });
                }
                
                return NotFound(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting store utilization for ID: {StoreId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }

    /// <summary>
    /// Request model for updating store status
    /// </summary>
    public class UpdateStoreStatusRequest
    {
        public StoreStatus Status { get; set; }
    }
}