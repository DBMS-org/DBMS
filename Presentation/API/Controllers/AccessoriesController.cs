using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces.MachineManagement;
using Application.DTOs.MachineManagement;
using Domain.Entities.MachineManagement;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api/accessories")]
    [AllowAnonymous] // Temporarily allow anonymous access for testing
    public class AccessoriesController : BaseApiController
    {
        private readonly IAccessoryRepository _repository;
        private readonly ILogger<AccessoriesController> _logger;

        public AccessoriesController(
            IAccessoryRepository repository,
            ILogger<AccessoriesController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: api/accessories
        [HttpGet]
        public async Task<IActionResult> GetAccessories(
            [FromQuery] string? search = null,
            [FromQuery] string? category = null,
            [FromQuery] string? supplier = null,
            [FromQuery] string? status = null)
        {
            try
            {
                IEnumerable<Accessory> accessories;

                // If any filters are provided, use search
                if (!string.IsNullOrEmpty(search) || !string.IsNullOrEmpty(category) ||
                    !string.IsNullOrEmpty(supplier) || !string.IsNullOrEmpty(status))
                {
                    AccessoryCategory? categoryEnum = null;
                    if (!string.IsNullOrEmpty(category) && Enum.TryParse<AccessoryCategory>(category, true, out var parsedCategory))
                    {
                        categoryEnum = parsedCategory;
                    }

                    accessories = await _repository.SearchAsync(search, categoryEnum, supplier, status);
                }
                else
                {
                    accessories = await _repository.GetAllAsync();
                }

                var accessoryDtos = accessories.Select(MapToDto).ToList();
                return base.Ok(accessoryDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving accessories");
                return InternalServerError("An error occurred while retrieving accessories");
            }
        }

        // GET: api/accessories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccessory(int id)
        {
            try
            {
                var accessory = await _repository.GetByIdAsync(id);

                if (accessory == null)
                {
                    return NotFound($"Accessory with ID {id} not found");
                }

                return base.Ok(MapToDto(accessory));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving accessory {AccessoryId}", id);
                return InternalServerError("An error occurred while retrieving the accessory");
            }
        }

        // GET: api/accessories/{id}/stock-history
        [HttpGet("{id}/stock-history")]
        public async Task<IActionResult> GetStockHistory(int id)
        {
            try
            {
                var accessory = await _repository.GetByIdWithHistoryAsync(id);

                if (accessory == null)
                {
                    return NotFound($"Accessory with ID {id} not found");
                }

                var historyDtos = accessory.StockAdjustments
                    .Select(sa => new StockAdjustmentHistoryDto
                    {
                        Id = sa.Id,
                        AdjustmentType = sa.AdjustmentType.ToString(),
                        QuantityChanged = sa.QuantityChanged,
                        PreviousQuantity = sa.PreviousQuantity,
                        NewQuantity = sa.NewQuantity,
                        Reason = sa.Reason.ToString(),
                        Notes = sa.Notes,
                        AdjustedBy = sa.AdjustedBy,
                        AdjustedDate = sa.AdjustedDate
                    })
                    .OrderByDescending(h => h.AdjustedDate)
                    .ToList();

                return base.Ok(historyDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stock history for accessory {AccessoryId}", id);
                return InternalServerError("An error occurred while retrieving stock history");
            }
        }

        // GET: api/accessories/statistics
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var statistics = new AccessoryStatisticsDto
                {
                    TotalAvailable = await _repository.CountAvailableAsync(),
                    LowStock = await _repository.CountLowStockAsync(),
                    OutOfStock = await _repository.CountOutOfStockAsync(),
                    TotalItems = await _repository.CountAsync()
                };

                return base.Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving accessory statistics");
                return InternalServerError("An error occurred while retrieving statistics");
            }
        }

        // POST: api/accessories
        [HttpPost]
        // [Authorize(Policy = "ManageMachines")] // Temporarily disabled for testing
        public async Task<IActionResult> CreateAccessory([FromBody] CreateAccessoryRequest request)
        {
            try
            {
                // Check if part number already exists
                if (await _repository.ExistsByPartNumberAsync(request.PartNumber))
                {
                    return Conflict($"An accessory with part number '{request.PartNumber}' already exists");
                }

                // Parse enums
                if (!Enum.TryParse<AccessoryCategory>(request.Category, true, out var category))
                {
                    return BadRequest($"Invalid category '{request.Category}'");
                }

                if (!Enum.TryParse<AccessoryUnit>(request.Unit, true, out var unit))
                {
                    return BadRequest($"Invalid unit '{request.Unit}'");
                }

                // Create accessory
                var accessory = Accessory.Create(
                    request.Name,
                    category,
                    request.PartNumber,
                    request.Quantity,
                    unit,
                    request.MinStockLevel,
                    request.Supplier,
                    request.Description,
                    request.Location);

                await _repository.AddAsync(accessory);

                _logger.LogInformation("Created accessory {AccessoryId} - {AccessoryName}", accessory.Id, accessory.Name);

                return CreatedAtAction(
                    nameof(GetAccessory),
                    new { id = accessory.Id },
                    MapToDto(accessory));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating accessory");
                return InternalServerError("An error occurred while creating the accessory");
            }
        }

        // PUT: api/accessories/{id}
        [HttpPut("{id}")]
        // [Authorize(Policy = "ManageMachines")] // Temporarily disabled for testing
        public async Task<IActionResult> UpdateAccessory(int id, [FromBody] UpdateAccessoryRequest request)
        {
            try
            {
                var accessory = await _repository.GetByIdAsync(id);

                if (accessory == null)
                {
                    return NotFound($"Accessory with ID {id} not found");
                }

                // Check if part number is being changed and if it conflicts
                if (accessory.PartNumber != request.PartNumber)
                {
                    if (await _repository.ExistsByPartNumberAsync(request.PartNumber, id))
                    {
                        return Conflict($"An accessory with part number '{request.PartNumber}' already exists");
                    }
                }

                // Parse enums
                if (!Enum.TryParse<AccessoryCategory>(request.Category, true, out var category))
                {
                    return BadRequest($"Invalid category '{request.Category}'");
                }

                if (!Enum.TryParse<AccessoryUnit>(request.Unit, true, out var unit))
                {
                    return BadRequest($"Invalid unit '{request.Unit}'");
                }

                // Update accessory
                accessory.Update(
                    request.Name,
                    category,
                    request.PartNumber,
                    unit,
                    request.MinStockLevel,
                    request.Supplier,
                    request.Description,
                    request.Location);

                await _repository.UpdateAsync(accessory);

                _logger.LogInformation("Updated accessory {AccessoryId} - {AccessoryName}", accessory.Id, accessory.Name);

                return base.Ok(MapToDto(accessory));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating accessory {AccessoryId}", id);
                return InternalServerError("An error occurred while updating the accessory");
            }
        }

        // POST: api/accessories/{id}/adjust-stock
        [HttpPost("{id}/adjust-stock")]
        // [Authorize(Policy = "ManageMachines")] // Temporarily disabled for testing
        public async Task<IActionResult> AdjustStock(int id, [FromBody] StockAdjustmentRequest request)
        {
            try
            {
                var accessory = await _repository.GetByIdAsync(id);

                if (accessory == null)
                {
                    return NotFound($"Accessory with ID {id} not found");
                }

                // Parse enums
                if (!Enum.TryParse<StockAdjustmentType>(request.Type, true, out var adjustmentType))
                {
                    return BadRequest($"Invalid adjustment type '{request.Type}'");
                }

                if (!Enum.TryParse<StockAdjustmentReason>(request.Reason, true, out var reason))
                {
                    return BadRequest($"Invalid reason '{request.Reason}'");
                }

                // Get user name from claims
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "Unknown";

                // Calculate new quantity based on adjustment type
                int newQuantity;
                switch (adjustmentType)
                {
                    case StockAdjustmentType.Add:
                        newQuantity = accessory.Quantity + request.Quantity;
                        break;
                    case StockAdjustmentType.Remove:
                        newQuantity = Math.Max(0, accessory.Quantity - request.Quantity);
                        break;
                    case StockAdjustmentType.Set:
                        newQuantity = request.Quantity;
                        break;
                    default:
                        return BadRequest("Invalid adjustment type");
                }

                // Adjust stock
                accessory.AdjustStock(newQuantity, adjustmentType, reason, userName, request.Notes);

                await _repository.UpdateAsync(accessory);

                _logger.LogInformation(
                    "Adjusted stock for accessory {AccessoryId} - {AccessoryName}: {Type} {Quantity} (New: {NewQuantity})",
                    accessory.Id, accessory.Name, adjustmentType, request.Quantity, newQuantity);

                return base.Ok(MapToDto(accessory));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adjusting stock for accessory {AccessoryId}", id);
                return InternalServerError("An error occurred while adjusting stock");
            }
        }

        // DELETE: api/accessories/{id}
        [HttpDelete("{id}")]
        // [Authorize(Policy = "ManageMachines")] // Temporarily disabled for testing
        public async Task<IActionResult> DeleteAccessory(int id)
        {
            try
            {
                var accessory = await _repository.GetByIdAsync(id);

                if (accessory == null)
                {
                    return NotFound($"Accessory with ID {id} not found");
                }

                await _repository.DeleteAsync(accessory);

                _logger.LogInformation("Deleted accessory {AccessoryId} - {AccessoryName}", accessory.Id, accessory.Name);

                return base.Ok(new { message = "Accessory deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting accessory {AccessoryId}", id);
                return InternalServerError("An error occurred while deleting the accessory");
            }
        }

        // GET: api/accessories/export
        [HttpGet("export")]
        public async Task<IActionResult> ExportToCsv(
            [FromQuery] string? search = null,
            [FromQuery] string? category = null,
            [FromQuery] string? supplier = null,
            [FromQuery] string? status = null)
        {
            try
            {
                IEnumerable<Accessory> accessories;

                // Apply same filters as GetAccessories
                if (!string.IsNullOrEmpty(search) || !string.IsNullOrEmpty(category) ||
                    !string.IsNullOrEmpty(supplier) || !string.IsNullOrEmpty(status))
                {
                    AccessoryCategory? categoryEnum = null;
                    if (!string.IsNullOrEmpty(category) && Enum.TryParse<AccessoryCategory>(category, true, out var parsedCategory))
                    {
                        categoryEnum = parsedCategory;
                    }

                    accessories = await _repository.SearchAsync(search, categoryEnum, supplier, status);
                }
                else
                {
                    accessories = await _repository.GetAllAsync();
                }

                // Generate CSV content
                var csv = new StringBuilder();
                csv.AppendLine("Name,Category,Part Number,Quantity,Unit,Min Stock,Supplier,Status,Location");

                foreach (var accessory in accessories)
                {
                    csv.AppendLine($"\"{accessory.Name}\",\"{accessory.Category}\",\"{accessory.PartNumber}\"," +
                                   $"{accessory.Quantity},\"{accessory.Unit}\",{accessory.MinStockLevel}," +
                                   $"\"{accessory.Supplier}\",\"{accessory.GetStockStatus()}\",\"{accessory.Location ?? ""}\"");
                }

                var bytes = Encoding.UTF8.GetBytes(csv.ToString());
                var fileName = $"accessories-inventory-{DateTime.UtcNow:yyyy-MM-dd}.csv";

                return File(bytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting accessories to CSV");
                return InternalServerError("An error occurred while exporting data");
            }
        }

        // Helper method to map entity to DTO
        private static AccessoryDto MapToDto(Accessory accessory)
        {
            return new AccessoryDto
            {
                Id = accessory.Id,
                Name = accessory.Name,
                Category = accessory.Category.ToString(),
                PartNumber = accessory.PartNumber,
                Description = accessory.Description,
                Quantity = accessory.Quantity,
                Unit = accessory.Unit.ToString(),
                MinStockLevel = accessory.MinStockLevel,
                Supplier = accessory.Supplier,
                Location = accessory.Location,
                Status = accessory.GetStockStatus(),
                IsLowStock = accessory.IsLowStock(),
                IsOutOfStock = accessory.IsOutOfStock(),
                CreatedAt = accessory.CreatedAt,
                UpdatedAt = accessory.UpdatedAt
            };
        }
    }
}
