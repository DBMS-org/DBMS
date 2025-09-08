using Domain.Entities.DrillingOperations;
using Application.DTOs.Shared;
using Application.DTOs.DrillingOperations;
using System.Threading;

namespace Application.Interfaces.DrillingOperations
{
    public interface IExplosiveCalculationResultService
    {
        Task<Result<IEnumerable<ExplosiveCalculationResult>>> GetAllExplosiveCalculationResultsAsync();
        Task<Result<ExplosiveCalculationResult>> GetExplosiveCalculationResultByIdAsync(int id);
        Task<Result<ExplosiveCalculationResult>> GetExplosiveCalculationResultByCalculationIdAsync(string calculationId);
        Task<Result<ExplosiveCalculationResult>> CreateExplosiveCalculationResultAsync(ExplosiveCalculationResult explosiveCalculationResult);
        Task<Result> UpdateExplosiveCalculationResultAsync(ExplosiveCalculationResult explosiveCalculationResult);
        Task<Result> DeleteExplosiveCalculationResultAsync(int id);
        Task<Result<IEnumerable<ExplosiveCalculationResult>>> GetExplosiveCalculationResultsByProjectIdAsync(int projectId);
        Task<Result<IEnumerable<ExplosiveCalculationResult>>> GetExplosiveCalculationResultsBySiteIdAsync(int projectId, int siteId);
        Task<Result<IEnumerable<ExplosiveCalculationResult>>> GetExplosiveCalculationResultsByUserIdAsync(int userId);
        Task<Result> DeleteExplosiveCalculationResultsByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);
        Task<Result> DeleteExplosiveCalculationResultsBySiteIdAsync(int projectId, int siteId, CancellationToken cancellationToken = default);
        Task<Result<int>> GetExplosiveCalculationResultCountAsync();
        Task<Result<int>> GetExplosiveCalculationResultCountByProjectIdAsync(int projectId);
        Task<Result<int>> GetExplosiveCalculationResultCountBySiteIdAsync(int projectId, int siteId);
        Task<Result<int>> GetExplosiveCalculationResultCountByUserIdAsync(int userId);
        
        // DTO-based methods for better frontend-backend mapping
        Task<Result<IEnumerable<ExplosiveCalculationResultDto>>> GetAllExplosiveCalculationResultsDtoAsync();
        Task<Result<ExplosiveCalculationResultDto>> GetExplosiveCalculationResultDtoByIdAsync(int id);
        Task<Result<ExplosiveCalculationResultDto>> GetExplosiveCalculationResultDtoByCalculationIdAsync(string calculationId);
        Task<Result<ExplosiveCalculationResultDto>> CreateExplosiveCalculationResultFromDtoAsync(CreateExplosiveCalculationResultRequest request);
        Task<Result<ExplosiveCalculationResultDto>> CreateExplosiveCalculationResultWithConfirmationAsync(CreateExplosiveCalculationResultRequest request);
        Task<Result> UpdateExplosiveCalculationResultFromDtoAsync(int id, UpdateExplosiveCalculationResultRequest request);
        Task<Result<IEnumerable<ExplosiveCalculationResultDto>>> GetExplosiveCalculationResultsDtoByProjectIdAsync(int projectId);
        Task<Result<IEnumerable<ExplosiveCalculationResultDto>>> GetExplosiveCalculationResultsDtoBySiteIdAsync(int projectId, int siteId);
        Task<Result<IEnumerable<ExplosiveCalculationResultDto>>> GetExplosiveCalculationResultsDtoByUserIdAsync(int userId);
    }
}