using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.MachineManagement;
using Domain.Events.MaintenanceOperations;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Handlers.MaintenanceOperations
{
    /// <summary>
    /// Handles MaterialsConsumedEvent - Deducts drilling components from inventory
    /// Triggered by: Mechanical Engineer completing maintenance job with materials
    /// Impact: Decrements drill bits, drill rods, and shanks from AccessoryInventory
    /// </summary>
    public class MaterialsConsumedEventHandler : IDomainEventHandler<MaterialsConsumedEvent>
    {
        private readonly IAccessoryRepository _accessoryRepository;
        private readonly ILogger<MaterialsConsumedEventHandler> _logger;

        public MaterialsConsumedEventHandler(
            IAccessoryRepository accessoryRepository,
            ILogger<MaterialsConsumedEventHandler> logger)
        {
            _accessoryRepository = accessoryRepository;
            _logger = logger;
        }

        public async Task HandleAsync(MaterialsConsumedEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var job = domainEvent.Job;

            _logger.LogInformation(
                "Processing MaterialsConsumedEvent for job {JobId}",
                job.Id);

            try
            {
                // Deduct drill bits
                if (job.DrillBitsUsed.HasValue && job.DrillBitsUsed > 0 && !string.IsNullOrWhiteSpace(job.DrillBitType))
                {
                    await DeductAccessoryAsync("Drill Bit", job.DrillBitType, job.DrillBitsUsed.Value, job.Id);
                }

                // Deduct drill rods
                if (job.DrillRodsUsed.HasValue && job.DrillRodsUsed > 0 && !string.IsNullOrWhiteSpace(job.DrillRodType))
                {
                    await DeductAccessoryAsync("Drill Rod", job.DrillRodType, job.DrillRodsUsed.Value, job.Id);
                }

                // Deduct shanks
                if (job.ShanksUsed.HasValue && job.ShanksUsed > 0 && !string.IsNullOrWhiteSpace(job.ShankType))
                {
                    await DeductAccessoryAsync("Shank", job.ShankType, job.ShanksUsed.Value, job.Id);
                }

                _logger.LogInformation(
                    "Materials deducted for job {JobId}. Bits: {Bits}, Rods: {Rods}, Shanks: {Shanks}",
                    job.Id,
                    job.DrillBitsUsed ?? 0,
                    job.DrillRodsUsed ?? 0,
                    job.ShanksUsed ?? 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error handling MaterialsConsumedEvent for job {JobId}",
                    job.Id);
                // Don't throw - we don't want to fail the job completion if inventory update fails
                // Log the error and continue
            }
        }

        private async Task DeductAccessoryAsync(string category, string type, int quantity, int jobId)
        {
            // Search for accessory by category and type
            // This is a simplified approach - in production, you might want more sophisticated matching
            var accessories = await _accessoryRepository.SearchAsync(
                searchTerm: type,
                category: null,
                supplier: null,
                status: null);

            var accessory = accessories.FirstOrDefault(a =>
                a.Name.Contains(type, StringComparison.OrdinalIgnoreCase) ||
                a.PartNumber.Contains(type, StringComparison.OrdinalIgnoreCase));

            if (accessory == null)
            {
                _logger.LogWarning(
                    "Accessory not found for deduction: Category={Category}, Type={Type}, Job={JobId}",
                    category, type, jobId);
                return;
            }

            if (accessory.Quantity < quantity)
            {
                _logger.LogWarning(
                    "Insufficient quantity for accessory {AccessoryId}. Available: {Available}, Required: {Required}, Job={JobId}",
                    accessory.Id, accessory.Quantity, quantity, jobId);
                // Still deduct what's available
            }

            // Deduct quantity
            accessory.AdjustStock(
                accessory.Quantity - quantity,  // New quantity
                Domain.Entities.MachineManagement.StockAdjustmentType.Remove,
                Domain.Entities.MachineManagement.StockAdjustmentReason.Usage,
                "System",  // Adjusted by
                $"Consumed in maintenance job #{jobId}"); // Notes

            await _accessoryRepository.UpdateAsync(accessory);

            _logger.LogInformation(
                "Deducted {Quantity} units of {Accessory} (ID: {AccessoryId}) for job {JobId}",
                quantity, accessory.Name, accessory.Id, jobId);
        }
    }
}
