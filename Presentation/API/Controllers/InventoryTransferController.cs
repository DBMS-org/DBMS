using Microsoft.AspNetCore.Mvc;
using Application.DTOs.ExplosiveInventory;
using Application.Interfaces.ExplosiveInventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Domain.Entities.ExplosiveInventory.Enums;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryTransferController : BaseApiController
    {
        private readonly IInventoryTransferService _transferService;
        private readonly ILogger<InventoryTransferController> _logger;

        public InventoryTransferController(
            IInventoryTransferService transferService,
            ILogger<InventoryTransferController> logger)
        {
            _transferService = transferService;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated list of transfer requests
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "ReadInventoryData")]
        public async Task<IActionResult> GetTransferRequests(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] TransferRequestStatus? status = null,
            [FromQuery] int? destinationStoreId = null,
            [FromQuery] int? requestedByUserId = null,
            [FromQuery] bool? isOverdue = null,
            [FromQuery] bool? isUrgent = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool sortDescending = false)
        {
            var filter = new TransferRequestFilterDto
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Status = status,
                DestinationStoreId = destinationStoreId,
                RequestedByUserId = requestedByUserId,
                IsOverdue = isOverdue,
                IsUrgent = isUrgent,
                SortBy = sortBy,
                SortDescending = sortDescending
            };

            var result = await _transferService.GetAllRequestsAsync(filter);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get transfer request by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Policy = "ReadInventoryData")]
        public async Task<IActionResult> GetTransferRequestById(int id)
        {
            var result = await _transferService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get pending transfer requests
        /// </summary>
        [HttpGet("pending")]
        [Authorize(Policy = "ReadInventoryData")]
        public async Task<IActionResult> GetPendingRequests()
        {
            var result = await _transferService.GetPendingRequestsAsync();

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get urgent transfer requests
        /// </summary>
        [HttpGet("urgent")]
        [Authorize(Policy = "ReadInventoryData")]
        public async Task<IActionResult> GetUrgentRequests()
        {
            var result = await _transferService.GetUrgentRequestsAsync();

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get overdue transfer requests
        /// </summary>
        [HttpGet("overdue")]
        [Authorize(Policy = "ReadInventoryData")]
        public async Task<IActionResult> GetOverdueRequests()
        {
            var result = await _transferService.GetOverdueRequestsAsync();

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get transfer requests by user
        /// </summary>
        [HttpGet("user/{userId}")]
        [Authorize(Policy = "ReadInventoryData")]
        public async Task<IActionResult> GetRequestsByUser(int userId)
        {
            var result = await _transferService.GetByUserAsync(userId);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get my transfer requests (current user)
        /// </summary>
        [HttpGet("my-requests")]
        [Authorize(Policy = "ReadInventoryData")]
        public async Task<IActionResult> GetMyRequests()
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _transferService.GetByUserAsync(userId);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get transfer requests by store
        /// </summary>
        [HttpGet("store/{storeId}")]
        [Authorize(Policy = "ReadInventoryData")]
        public async Task<IActionResult> GetRequestsByStore(int storeId)
        {
            var result = await _transferService.GetByDestinationStoreAsync(storeId);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Create new transfer request
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "ManageInventory")]
        public async Task<IActionResult> CreateTransferRequest([FromBody] CreateTransferRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _transferService.CreateTransferRequestAsync(request, userId);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(
                nameof(GetTransferRequestById),
                new { id = result.Value.Id },
                result.Value);
        }

        /// <summary>
        /// Approve transfer request
        /// </summary>
        [HttpPost("{id}/approve")]
        [Authorize(Policy = "ApproveTransfers")]
        public async Task<IActionResult> ApproveTransferRequest(
            int id,
            [FromBody] ApproveTransferRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _transferService.ApproveTransferRequestAsync(id, request, userId);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Reject transfer request
        /// </summary>
        [HttpPost("{id}/reject")]
        [Authorize(Policy = "ApproveTransfers")]
        public async Task<IActionResult> RejectTransferRequest(
            int id,
            [FromBody] RejectTransferRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _transferService.RejectTransferRequestAsync(id, request, userId);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Dispatch transfer request
        /// </summary>
        [HttpPost("{id}/dispatch")]
        [Authorize(Policy = "ManageInventory")]
        public async Task<IActionResult> DispatchTransferRequest(
            int id,
            [FromBody] DispatchTransferRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _transferService.DispatchTransferRequestAsync(id, request, userId);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Confirm delivery of transfer request
        /// </summary>
        [HttpPost("{id}/confirm-delivery")]
        [Authorize(Policy = "ManageInventory")]
        public async Task<IActionResult> ConfirmDelivery(int id)
        {
            var result = await _transferService.ConfirmDeliveryAsync(id);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Complete transfer request
        /// </summary>
        [HttpPost("{id}/complete")]
        [Authorize(Policy = "ManageInventory")]
        public async Task<IActionResult> CompleteTransferRequest(int id)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _transferService.CompleteTransferRequestAsync(id, userId);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Cancel transfer request
        /// </summary>
        [HttpPost("{id}/cancel")]
        [Authorize(Policy = "ManageInventory")]
        public async Task<IActionResult> CancelTransferRequest(int id, [FromBody] CancelTransferRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _transferService.CancelTransferRequestAsync(id, request.Reason);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }
    }

    /// <summary>
    /// Request model for cancelling a transfer
    /// </summary>
    public class CancelTransferRequestDto
    {
        public string Reason { get; set; } = string.Empty;
    }
}
