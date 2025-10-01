using Application.DTOs.StoreManagement;
using Application.DTOs.Shared;
using Domain.Entities.StoreManagement;

namespace Application.Interfaces.StoreManagement
{
    public interface IStoreService
    {
        Task<Result<IEnumerable<StoreDto>>> GetAllStoresAsync();
        Task<Result<StoreDto>> GetStoreByIdAsync(int id);
        Task<Result<StoreDto>> CreateStoreAsync(CreateStoreRequest request);
        Task<Result> UpdateStoreAsync(int id, UpdateStoreRequest request);
        Task<Result> DeleteStoreAsync(int id);
        Task<Result<IEnumerable<StoreDto>>> GetStoresByRegionAsync(int regionId);
        Task<Result<IEnumerable<StoreDto>>> GetStoresByProjectAsync(int projectId);
        Task<Result<IEnumerable<StoreDto>>> SearchStoresAsync(string? storeName = null, string? city = null, string? status = null);
        Task<Result<StoreDto>> GetStoreByManagerAsync(int managerUserId);
        Task<Result> UpdateStoreStatusAsync(int id, Domain.Entities.StoreManagement.Enums.StoreStatus status);
        Task<Result<decimal>> GetStoreUtilizationAsync(int id);
        Task<Result<StoreStatisticsDto>> GetStoreStatisticsAsync();
    }
}