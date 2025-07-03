using Domain.Entities.DrillingOperations;
using Application.DTOs.DrillingOperations;
using Application.DTOs.Shared;

namespace Application.Interfaces.DrillingOperations
{
    public interface ICsvImportService
    {
        Task<Result<IEnumerable<DrillHole>>> CreateDrillHolesFromCsvAsync(CsvUploadRequest csvRequest, CancellationToken cancellationToken = default);
    }
} 