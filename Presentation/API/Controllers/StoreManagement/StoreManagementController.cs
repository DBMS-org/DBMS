using Application.DTOs.StoreManagement;
using Application.Interfaces.StoreManagement;
using Domain.Entities.StoreManagement.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.StoreManagement
{
    [ApiController]
    [Route("api/storemanagement")]
    [Authorize(Roles = "Admin,ExplosiveManager,StoreManager")]
    public class StoreManagementController : BaseApiController
    {
        private readonly IStoreService _storeService;

        public StoreManagementController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        /// <summary>
        /// Get all stores
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllStores()
        {
            var result = await _storeService.GetAllStoresAsync();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        /// <summary>
        /// Get store by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreById(int id)
        {
            var result = await _storeService.GetStoreByIdAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Error);
        }

        /// <summary>
        /// Create a new store
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Administrator,ExplosiveManager")]
        public async Task<IActionResult> CreateStore([FromBody] CreateStoreRequest request)
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

        /// <summary>
        /// Update an existing store
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Administrator,ExplosiveManager")]
        public async Task<IActionResult> UpdateStore(int id, [FromBody] UpdateStoreRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _storeService.UpdateStoreAsync(id, request);

            if (result.IsSuccess)
            {
                var updatedStore = await _storeService.GetStoreByIdAsync(id);
                return Ok(updatedStore.Value);
            }

            return BadRequest(result.Error);
        }

        /// <summary>
        /// Delete a store
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var result = await _storeService.DeleteStoreAsync(id);

            if (result.IsSuccess)
            {
                return Ok();
            }

            return NotFound(result.Error);
        }

        /// <summary>
        /// Get store statistics
        /// </summary>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStoreStatistics()
        {
            var result = await _storeService.GetStoreStatisticsAsync();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
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
            var result = await _storeService.SearchStoresAsync(storeName, city, status);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        /// <summary>
        /// Get stores by region
        /// </summary>
        [HttpGet("region/{regionId}")]
        public async Task<IActionResult> GetStoresByRegion(int regionId)
        {
            var result = await _storeService.GetStoresByRegionAsync(regionId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }



        /// <summary>
        /// Get store by manager
        /// </summary>
        [HttpGet("manager/{userId}")]
        public async Task<IActionResult> GetStoreByManager(int userId)
        {
            var result = await _storeService.GetStoreByManagerAsync(userId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Error);
        }

        /// <summary>
        /// Update store status
        /// </summary>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Administrator,ExplosiveManager")]
        public async Task<IActionResult> UpdateStoreStatus(int id, [FromBody] UpdateStoreStatusRequest request)
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

        /// <summary>
        /// Get store utilization
        /// </summary>
        [HttpGet("{id}/utilization")]
        public async Task<IActionResult> GetStoreUtilization(int id)
        {
            var result = await _storeService.GetStoreUtilizationAsync(id);

            if (result.IsSuccess)
            {
                return Ok(new { UtilizationPercentage = result.Value });
            }

            return NotFound(result.Error);
        }
    }
}